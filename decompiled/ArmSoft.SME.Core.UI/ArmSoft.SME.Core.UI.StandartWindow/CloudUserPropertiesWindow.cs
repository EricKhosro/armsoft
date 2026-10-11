using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;
using ArmSoft.Cloud.Client;
using ArmSoft.Cloud.Client.AS7X;
using ArmSoft.SME.Client;
using ArmSoft.SME.Common;
using ArmSoft.SME.Core.UI.Controls;
using ArmSoft.SME.Core.UI.HelpClasses;

namespace ArmSoft.SME.Core.UI.StandartWindow;

public class CloudUserPropertiesWindow : DialogWindow
{
	private Button _changePasswordButton;

	public CloudUserPropertiesWindow()
	{
		WindowValidation += CloudUserPropertiesWindow_WindowValidation;
	}

	public void Invoke()
	{
		CloudUser currentUser;
		using (CloudClient cloudClient = Settings.CloudClient)
		{
			currentUser = cloudClient.AS7X.GetCurrentUser();
		}
		Caption = ArmSoft.SME.Common.Resources.UserProperties;
		Name = "ShowCloudUserProperties";
		AddStringControl("Caption", ArmSoft.SME.Common.Resources.UserDescription, 50).EditValue = currentUser.Caption;
		TextEditExt textEditExt = AddStringControl("Email", ArmSoft.SME.Common.Resources.Email, 129);
		textEditExt.EditValue = currentUser.Email;
		textEditExt.IsReadOnly = true;
		LayoutGroupExt container = AddHorizontalLayoutGroup();
		LayoutGroupExt container2 = AddVerticalLayoutGroup(container);
		AddStringControl(container2, "Phone", ArmSoft.SME.Common.Resources.PhoneNumber, 16).EditValue = currentUser.Phone;
		TextEditExt textEditExt2 = AddStringControl(container2, "IdentityProvider", ArmSoft.SME.Common.Resources.IdentityProvider, 16);
		textEditExt2.EditValue = currentUser.IdentityProvider;
		textEditExt2.IsReadOnly = true;
		if (currentUser.IdentityProvider == "LocalAccount")
		{
			LayoutGroupExt layoutGroupExt = AddVerticalLayoutGroup(container);
			layoutGroupExt.VerticalAlignment = VerticalAlignment.Center;
			_changePasswordButton = AddButton(layoutGroupExt, "ChangePassword", "", ArmSoft.SME.Common.Resources.ChangePassword.Replace(" ", "\r\n"));
			_changePasswordButton.Width = 125.0;
			_changePasswordButton.Height = 40.0;
			_changePasswordButton.VerticalAlignment = VerticalAlignment.Center;
			_changePasswordButton.Click += new SafeEventHandler<RoutedEventArgs>(ChangePasswordButon_ClickAsync).Handler;
		}
		if (ShowDialog().Value)
		{
			using (CloudClient cloudClient2 = Settings.CloudClient)
			{
				currentUser = cloudClient2.AS7X.GetCurrentUser();
			}
			Settings.HostingEnvironment.SetCloudUserHeaderInfo(currentUser.Caption, currentUser.Email);
		}
	}

	private void CloudUserPropertiesWindow_WindowValidation(DialogWindow window, DialogWindowValidationArgs args)
	{
		try
		{
			Mouse.OverrideCursor = Cursors.Wait;
			using (CloudClient cloudClient = new CloudClient("", Settings.B2BCloudBaseAddress))
			{
				cloudClient.UserToken = ADHelper.ADB2CToken;
				cloudClient.AS7X.EditCurrentUser(new CloudUserUpdateRequest
				{
					Caption = base["Caption"].ToString().Trim(),
					Phone = base["Phone"].ToString().Trim()
				});
				UserRoutes userRoutes = Settings.CreateApiClient().UserRoutes;
				string aDB2CToken = ADHelper.ADB2CToken;
				int num = 1;
				List<int> list = new List<int>(num);
				CollectionsMarshal.SetCount(list, num);
				CollectionsMarshal.AsSpan(list)[0] = Settings.LoginService.CloudUserId;
				userRoutes.UpdateUsers(aDB2CToken, list);
				Settings.CoreCacheService.User.Refresh();
			}
			Mouse.OverrideCursor = null;
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
			args.isValid = false;
		}
	}

	private async void ChangePasswordButon_ClickAsync(object sender, RoutedEventArgs e)
	{
		try
		{
			await ADHelper.ChangePasswordAsync(RegistryHelper.ReadStringFromRegistry("CloudLastLogin"), new WindowInteropHelper(this).Handle);
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}
}
