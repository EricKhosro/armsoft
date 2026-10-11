using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Markup;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Helpers;
using ArmSoft.SME.Core.UI.Helpers;
using ArmSoft.SME.Core.UI.Services;
using DevExpress.Xpf.Core;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Win32;

namespace ArmSoft.SME.Core.UI;

public partial class OfflineLoginWindow : ThemedWindow, IComponentConnector
{
	private readonly string _lastDB;

	private readonly string _pin;

	public OfflineLoginWindow()
	{
		InitializeComponent();
		Owner = Settings.HostingEnvironment;
		using (RegistryKey registryKey = Registry.CurrentUser.OpenSubKey(Settings.RegistryKey))
		{
			if (registryKey != null)
			{
				_lastDB = (string)registryKey.GetValue("CloudLastDB", "");
			}
		}
		CloudDB.Text = _lastDB;
		ILocalDBLoginInfo requiredService = Settings.ServiceProvider.GetRequiredService<ILocalDBLoginInfo>();
		requiredService.Database = _lastDB;
		if (requiredService.ExistsDatabase())
		{
			LoginName.Text = requiredService.UserName;
			_pin = requiredService.PIN;
		}
		OkButton.Click += new SafeEventHandler<RoutedEventArgs>(OkButton_Click).Handler;
		CloseTitleButton.Click += new SafeEventHandler<RoutedEventArgs>(CloseButton_Click).Handler;
		if (string.IsNullOrWhiteSpace(_pin))
		{
			TBUserName.Margin = new Thickness(0.0, 20.0, 0.0, 5.0);
			LoginName.Margin = new Thickness(0.0, 0.0, 0.0, 5.0);
			TBDatabase.Margin = new Thickness(0.0, 20.0, 0.0, 5.0);
			CloudDB.Margin = new Thickness(0.0, 0.0, 0.0, 20.0);
			TBPinCode.Visibility = Visibility.Collapsed;
			PBPin.Visibility = Visibility.Collapsed;
		}
		else
		{
			PBPin.Focus();
		}
		ASFontHelper.SetFontSettings(this);
	}

	private void CloseButton_Click(object sender, RoutedEventArgs e)
	{
		DialogResult = false;
	}

	private void OkButton_Click(object sender, RoutedEventArgs e)
	{
		Settings.LoginService.Logout();
		try
		{
			if (EncryptHelper.GenerateHashedString(PBPin.Text.TrimEnd()) != _pin && (!string.IsNullOrWhiteSpace(PBPin.Text) || !string.IsNullOrWhiteSpace(_pin)))
			{
				PBPin.Focus();
				throw new ASException(ArmSoft.SME.Common.Resources.PINIsIncorrect);
			}
			Mouse.OverrideCursor = Cursors.Wait;
			ILocalDBLoginInfo requiredService = Settings.ServiceProvider.GetRequiredService<ILocalDBLoginInfo>();
			ADHelper.ADB2CToken = requiredService.ADB2CToken;
			ApplicationServiceConfig.Token = requiredService.Token;
			Settings.LoginService.RestoreFromLocalDB();
			Settings.LoginService.IsLoggedIn = true;
			Settings.IsOffline = true;
			Settings.ParametersService.Refresh();
			Settings.UserAccessService.LoadAllPermissionsIfNecessary();
			Mouse.OverrideCursor = null;
			DialogResult = true;
		}
		catch (Exception ex)
		{
			if (Mouse.OverrideCursor != null)
			{
				Mouse.OverrideCursor = null;
			}
			Settings.ErrorHandler(ex);
		}
	}
}
