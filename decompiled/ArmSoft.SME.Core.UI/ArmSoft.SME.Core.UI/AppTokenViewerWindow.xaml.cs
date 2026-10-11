using System;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Windows;
using System.Windows.Markup;
using ArmSoft.SME.Core.UI.Helpers;
using ArmSoft.SME.Core.UI.Services;
using DevExpress.Xpf.Core;
using Microsoft.IdentityModel.JsonWebTokens;

namespace ArmSoft.SME.Core.UI;

public partial class AppTokenViewerWindow : ThemedWindow, IComponentConnector
{
	private static readonly JsonSerializerOptions _options = new JsonSerializerOptions
	{
		WriteIndented = true,
		IndentSize = 4,
		Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
	};

	public AppTokenViewerWindow()
	{
		InitializeComponent();
		Icon = Settings.HostingEnvironment.Icon;
		string token = ApplicationServiceConfig.Token;
		TE_ServiceToken.EditValue = token;
		TE_ServiceTokenPayload.EditValue = GetJWTTokenPayloadJSON(token);
		string applicationToken = Settings.GetApplicationToken();
		TE_Token.EditValue = applicationToken;
		TE_TokenPayload.EditValue = GetJWTTokenPayloadJSON(applicationToken);
		string aDB2CToken = ADHelper.ADB2CToken;
		TE_UserToken.EditValue = aDB2CToken;
		TE_UserTokenPayload.EditValue = GetJWTTokenPayloadJSON(aDB2CToken);
		ButtonGetToken.Click += new SafeEventHandler<RoutedEventArgs>(ButtonGetToken_Click).Handler;
		ButtonCopy.Click += new SafeEventHandler<RoutedEventArgs>(ButtonCopy_Click).Handler;
		ButtonCopyUser.Click += new SafeEventHandler<RoutedEventArgs>(ButtonCopyUser_Click).Handler;
		ButtonCopyService.Click += new SafeEventHandler<RoutedEventArgs>(ButtonCopyService_Click).Handler;
		ASFontHelper.SetFontSettings(this);
	}

	public static void ShowWindow()
	{
		AppTokenViewerWindow appTokenViewerWindow = new AppTokenViewerWindow();
		appTokenViewerWindow.Owner = Settings.HostingEnvironment;
		appTokenViewerWindow.ShowDialog();
	}

	public static string GetJWTTokenPayloadJSON(string token)
	{
		string text = new JsonWebToken(token).EncodedPayload;
		int num = 4 - text.Length % 4;
		if (num < 4)
		{
			text = text.PadRight(text.Length + num, '=');
		}
		byte[] bytes = Convert.FromBase64String(text);
		return JsonSerializer.Serialize(JsonSerializer.Deserialize<JsonElement>(Encoding.UTF8.GetString(bytes)), _options);
	}

	private void ButtonGetToken_Click(object sender, RoutedEventArgs e)
	{
		try
		{
			string applicationToken = Settings.GetApplicationToken();
			TE_Token.EditValue = applicationToken;
			TE_TokenPayload.EditValue = GetJWTTokenPayloadJSON(applicationToken);
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	private void ButtonCopy_Click(object sender, RoutedEventArgs e)
	{
		Clipboard.SetDataObject(TE_Token.Text);
	}

	private void ButtonCopyUser_Click(object sender, RoutedEventArgs e)
	{
		Clipboard.SetDataObject(TE_UserToken.Text);
	}

	private void ButtonCopyService_Click(object sender, RoutedEventArgs e)
	{
		Clipboard.SetDataObject(TE_ServiceToken.Text);
	}
}
