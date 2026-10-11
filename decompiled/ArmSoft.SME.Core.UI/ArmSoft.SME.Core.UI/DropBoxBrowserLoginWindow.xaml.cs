using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Threading;
using ArmSoft.SME.Client;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Parameters;
using ArmSoft.b2b.V2;
using DevExpress.Xpf.Core;
using Dropbox.Api;

namespace ArmSoft.SME.Core.UI;

public partial class DropBoxBrowserLoginWindow : ThemedWindow, IComponentConnector
{
	private string _oauth2State;

	private readonly CancellationTokenSource _cts;

	private string _accessToken;

	private string _userId;

	private string _accountMail;

	public string AccessToken => _accessToken;

	public string UserId => _userId;

	public string AccountMail => _accountMail;

	public DropBoxBrowserLoginWindow(string appKey)
	{
		InitializeComponent();
		if (!Settings.IsWin11Theme)
		{
			WindowStyle = WindowStyle.ToolWindow;
		}
		Owner = Settings.HostingEnvironment;
		_cts = new CancellationTokenSource();
		Title = ArmSoft.SME.Common.Resources.AuthenticateWithDropbox;
		lblMessage.Content = ArmSoft.SME.Common.Resources.WaitingResponseFromBrowser;
		cmdCancel.Content = ArmSoft.SME.Common.Resources.Cancel;
		((DispatcherObject)this).Dispatcher.BeginInvoke((Delegate)new Action<string, CancellationToken>(Start), new object[2] { appKey, _cts.Token });
	}

	private async void Start(string appKey, CancellationToken ct)
	{
		switch (Settings.SystemType)
		{
		case SystemTypes.Accountant:
			_oauth2State = "A";
			break;
		case SystemTypes.Trade:
			_oauth2State = "T";
			break;
		case SystemTypes.SalesManagement:
			_oauth2State = "S";
			break;
		}
		_oauth2State += LanguageService.CurrentUICultureName.Substring(0, 2);
		_oauth2State += Guid.NewGuid().ToString("N");
		Uri resultUri = null;
		int count = 120;
		using (B2BClient b2bClient = Settings.B2BClient)
		{
			Uri authorizationUri = b2bClient.DropBox.GetAuthorizationUri();
			Uri authorizeUri = DropboxOAuth2Helper.GetAuthorizeUri(OAuthResponseType.Token, appKey, authorizationUri, _oauth2State);
			Process.Start(new ProcessStartInfo
			{
				UseShellExecute = true,
				FileName = authorizeUri.ToString()
			});
			while (!ct.IsCancellationRequested && (object)resultUri == null && count > 0)
			{
				try
				{
					resultUri = b2bClient.DropBox.GetRedirectUri(_oauth2State);
				}
				catch (Exception)
				{
				}
				count--;
				await Task.Delay(new TimeSpan(0, 0, 1), ct);
			}
		}
		if (ct.IsCancellationRequested || (object)resultUri == null || count <= 0)
		{
			DialogResult = false;
			return;
		}
		if (resultUri.ToString().Contains("&error="))
		{
			DialogResult = false;
			return;
		}
		OAuth2Response oAuth2Response = DropboxOAuth2Helper.ParseTokenFragment(resultUri);
		_accessToken = oAuth2Response.AccessToken;
		_userId = oAuth2Response.Uid;
		using (DropboxClient dbx = new DropboxClient(oAuth2Response.AccessToken))
		{
			_ = 1;
			try
			{
				_accountMail = (await dbx.Users.GetCurrentAccountAsync()).Email;
			}
			catch (Exception)
			{
			}
		}
		DialogResult = true;
	}

	private void CancelClick(object sender, RoutedEventArgs e)
	{
		_cts.Cancel();
	}

	public static string GetAccessToken(string appKey)
	{
		string text = Settings.ParametersService.DropBoxAccountToken();
		if (!string.IsNullOrWhiteSpace(text) && ValidateAccessToken(text))
		{
			return text;
		}
		DropBoxBrowserLoginWindow dropBoxBrowserLoginWindow = new DropBoxBrowserLoginWindow(appKey);
		if (dropBoxBrowserLoginWindow.ShowDialog().Value)
		{
			text = dropBoxBrowserLoginWindow.AccessToken;
			Settings.ParametersService.DropBoxAccountEMail_set(dropBoxBrowserLoginWindow.AccountMail);
			Settings.ParametersService.DropBoxAccountToken_set(text);
			return text;
		}
		return string.Empty;
	}

	public static bool ValidateAccessToken(string token)
	{
		bool result = true;
		using (DropboxClient dropboxClient = new DropboxClient(token))
		{
			try
			{
				AsyncHelper.RunSync(dropboxClient.Users.GetCurrentAccountAsync);
			}
			catch (Exception)
			{
				result = false;
			}
		}
		return result;
	}
}
