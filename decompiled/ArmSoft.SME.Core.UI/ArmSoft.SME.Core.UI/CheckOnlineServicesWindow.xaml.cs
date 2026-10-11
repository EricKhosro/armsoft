using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Navigation;
using ArmSoft.SME.Common;
using ArmSoft.SME.Core.UI.Helpers;
using DevExpress.Xpf.Core;

namespace ArmSoft.SME.Core.UI;

public partial class CheckOnlineServicesWindow : ThemedWindow, IComponentConnector
{
	private readonly ImageSource _imageChecked;

	private readonly ImageSource _imageFailed;

	private readonly ImageSource _imageRun;

	private readonly ImageSource _imageHelp;

	private string _token;

	public CheckOnlineServicesWindow()
	{
		//IL_015c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0162: Unknown result type (might be due to invalid IL or missing references)
		//IL_0178: Unknown result type (might be due to invalid IL or missing references)
		//IL_017e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0194: Unknown result type (might be due to invalid IL or missing references)
		//IL_019a: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b6: Unknown result type (might be due to invalid IL or missing references)
		InitializeComponent();
		Owner = Settings.HostingEnvironment;
		Title = ArmSoft.SME.Common.Resources.CheckOnlineServices;
		ServicesTab.Header = ArmSoft.SME.Common.Resources.Services;
		VersionsUpdatesLabel.Content = ArmSoft.SME.Common.Resources.VersionsUpdates;
		WCFServicesLabel.Content = ArmSoft.SME.Common.Resources.WCFServices;
		RESTServicesLabel.Content = ArmSoft.SME.Common.Resources.RESTServices;
		CloudServicesLabel.Content = ArmSoft.SME.Common.Resources.CloudServices;
		EdgeServicesLabel.Content = ArmSoft.SME.Common.Resources.EdgeServices;
		TokenTab.Header = ArmSoft.SME.Common.Resources.Token;
		VersionsUpdatesHyperlink.Inlines.Add(VersionsUpdatesHyperlink.NavigateUri.ToString());
		WCFServicesHyperlink.Inlines.Add(WCFServicesHyperlink.NavigateUri.ToString());
		RESTServicesHyperlink.Inlines.Add(RESTServicesHyperlink.NavigateUri.ToString());
		CloudServicesHyperlink.Inlines.Add(CloudServicesHyperlink.NavigateUri.ToString());
		EdgeServicesHyperlink.Inlines.Add(EdgeServicesHyperlink.NavigateUri.ToString());
		CheckButton.Content = ArmSoft.SME.Common.Resources.Check;
		CancelButton.Content = ArmSoft.SME.Common.Resources.Cancel;
		_imageChecked = ImageService.GetImage("check_32.svg");
		_imageFailed = ImageService.GetImage("delete_32.svg");
		_imageRun = ImageService.GetImage("run_32.svg");
		_imageHelp = ImageService.GetImage("help_32.svg");
		CheckButton.Click += new SafeEventHandler<RoutedEventArgs>(CheckButton_Click).Handler;
		TE_TokenPayload.PreviewKeyDown += new SafeEventHandler<KeyEventArgs>(TE_TokenPayload_KeyDown).Handler;
		ASFontHelper.SetFontSettings(this);
	}

	private async void CheckButton_Click(object sender, RoutedEventArgs e)
	{
		await CheckOnlineServices();
	}

	private async Task CheckOnlineServices()
	{
		CheckButton.IsEnabled = false;
		CancelButton.IsEnabled = false;
		VersionsUpdatesImage.Source = _imageHelp;
		WCFServicesImage.Source = _imageHelp;
		RESTServicesImage.Source = _imageHelp;
		EdgeServicesImage.Source = _imageHelp;
		CloudServicesImage.Source = _imageHelp;
		b2cLoginImage.Source = _imageHelp;
		await CheckOnlineServicesInner(VersionsUpdatesHyperlink, VersionsUpdatesImage);
		await CheckOnlineServicesInner(WCFServicesHyperlink, WCFServicesImage);
		await CheckOnlineServicesInner(RESTServicesHyperlink, RESTServicesImage);
		bool cloudOk = await CheckOnlineServicesInner(CloudServicesHyperlink, CloudServicesImage);
		await CheckOnlineServicesInner(EdgeServicesHyperlink, EdgeServicesImage);
		await CheckOnlineServicesInner(b2cLoginHyperlink, b2cLoginImage);
		if (cloudOk && Settings.LoginService.IsLoggedIn)
		{
			try
			{
				await Task.Run(() => Task.Delay(1));
				Settings.ClearApplicationToken();
				_token = Settings.GetApplicationToken();
			}
			catch (Exception)
			{
			}
		}
		CheckButton.IsEnabled = true;
		CancelButton.IsEnabled = true;
	}

	private async Task<bool> CheckOnlineServicesInner(Hyperlink hyperlink, Image image)
	{
		HttpClient httpClient = new HttpClient();
		HttpResponseMessage result = null;
		try
		{
			image.Source = _imageRun;
			result = await httpClient.GetAsync(hyperlink.NavigateUri);
		}
		catch (Exception)
		{
		}
		bool flag = result?.IsSuccessStatusCode ?? false;
		image.Source = (flag ? _imageChecked : _imageFailed);
		return flag;
	}

	private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
	{
		Process.Start(new ProcessStartInfo
		{
			UseShellExecute = true,
			FileName = e.Uri.ToString()
		});
		e.Handled = true;
	}

	private void TE_TokenPayload_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Invalid comparison between Unknown and I4
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Invalid comparison between Unknown and I4
		if (!string.IsNullOrWhiteSpace(_token) && (int)e.Key == 63 && (int)Keyboard.Modifiers == 2)
		{
			Clipboard.SetDataObject(_token);
			e.Handled = true;
			MessageBox.Show("B2B services token copied to clipboard.");
		}
	}
}
