using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Ardalis.SmartEnum;
using ArmSoft.SME.Common;
using ArmSoft.SME.Core.UI.Controls;
using ArmSoft.SME.Core.UI.HelpClasses;
using ArmSoft.SME.Core.UI.InternetBank;
using ArmSoft.SME.Core.UI.SmartEnums;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.LayoutControl;

namespace ArmSoft.SME.Core.UI;

public class InternetBankLoginDialog : DialogWindow
{
	private readonly TextEditExt _login;

	private readonly PasswordBoxEditExt _password;

	public readonly TextEditExt _securityKey;

	public readonly TextBlock _challenge;

	private readonly InternetBankLoginRequest _request;

	private readonly InternetBankLoginResponse _response;

	private readonly LayoutGroup _loginPassLayoutGroup;

	public string LoginToken => _response.Token;

	public InternetBankLoginDialog(IBBankSE bank, InternetBankLoginRequest request)
	{
		Cursor = null;
		Caption = string.Format(ArmSoft.SME.Common.Resources.IBLogin, bank.Caption);
		MinWidth = 460.0;
		MinHeight = 236.0;
		_request = request;
		_response = new InternetBankLoginResponse();
		LayoutGroupExt container = AddVerticalLayoutGroup();
		LayoutGroupExt layoutGroupExt = AddHorizontalLayoutGroup(container);
		layoutGroupExt.Background = Settings.GetBackgroundBrush();
		DXImage dXImage = AddImageControl(layoutGroupExt, bank);
		dXImage.HorizontalAlignment = HorizontalAlignment.Left;
		dXImage.Margin = new Thickness(15.0, 0.0, 0.0, 0.0);
		TextBlock textBlock = AddTextBlock(layoutGroupExt, bank.Caption, TextBlockImage.None);
		textBlock.HorizontalAlignment = HorizontalAlignment.Center;
		((LayoutItemWithRequired)(object)textBlock.Parent).VerticalAlignment = VerticalAlignment.Center;
		textBlock.FontWeight = FontWeights.Bold;
		textBlock.FontSize = 18.0;
		textBlock.Margin = new Thickness(-75.0, 0.0, 0.0, 0.0);
		AddLine(container);
		_loginPassLayoutGroup = AddVerticalLayoutGroup(container);
		_login = AddStringControl(_loginPassLayoutGroup, "_login", ArmSoft.SME.Common.Resources.UserName, 32, isRequired: true);
		_login.EditValue = RegistryHelper.ReadStringFromRegistry(bank.Value, "Internet Bank");
		((LayoutItemWithRequired)(object)_login.Parent).Margin = new Thickness(0.0, 10.0, 0.0, 5.0);
		_password = AddPasswordControl(_loginPassLayoutGroup, "_password", ArmSoft.SME.Common.Resources.Password, 32, isRequired: true);
		LayoutGroupExt layoutGroupExt2 = AddVerticalLayoutGroup(container);
		layoutGroupExt2.HorizontalAlignment = HorizontalAlignment.Center;
		_challenge = AddTextBlock(layoutGroupExt2, string.Empty, TextBlockImage.None);
		_challenge.TextWrapping = TextWrapping.Wrap;
		_challenge.Visibility = Visibility.Collapsed;
		_challenge.VerticalAlignment = VerticalAlignment.Center;
		_securityKey = AddStringControl(layoutGroupExt2, "_securityKey", string.Empty, 32);
		_securityKey.HorizontalAlignment = HorizontalAlignment.Center;
		_challenge.MaxWidth = _securityKey.Width;
		_challenge.Visibility = Visibility.Collapsed;
		GetLayoutItem(_securityKey).Visibility = Visibility.Collapsed;
		if (request.IsOnlyDigiPass)
		{
			_loginPassLayoutGroup.Visibility = Visibility.Collapsed;
			_login.IsRequired = false;
			_password.IsRequired = false;
			_challenge.Text = request.Description;
			_challenge.Visibility = Visibility.Visible;
			GetLayoutItem(_securityKey).Visibility = Visibility.Visible;
		}
		if (!request.IsOnlyDigiPass)
		{
			_login.PreviewKeyDown += new SafeEventHandler<KeyEventArgs>(CommonEventHandlers.OnKeyDownEnterMoveNext).Handler;
			_password.PreviewKeyDown += new SafeEventHandler<KeyEventArgs>(CommonEventHandlers.OnKeyDownEnterMoveNext).Handler;
		}
		_securityKey.PreviewKeyDown += new SafeEventHandler<KeyEventArgs>(CommonEventHandlers.OnKeyDownEnterMoveNext).Handler;
	}

	protected override void DialogValidation(DialogWindowValidationArgs args)
	{
		base.DialogValidation(args);
		if (string.IsNullOrWhiteSpace(_response.Token) && !_request.IsOnlyDigiPass)
		{
			_request.User = _login.Text;
			_request.Password = _password.Text;
			_request.SecurityKey = _securityKey.Text;
			InternetBankLoginResponse internetBankLoginResponse = Settings.InternetBankService.Login(_request);
			if (internetBankLoginResponse.ResponseType == InternetBankResponseTypes.SecureAuthenticationRequired)
			{
				args.isValid = false;
				_loginPassLayoutGroup.Visibility = Visibility.Collapsed;
				_challenge.Visibility = Visibility.Visible;
				_challenge.Text = internetBankLoginResponse.Message;
				GetLayoutItem(_securityKey).Visibility = Visibility.Visible;
			}
			else
			{
				args.isValid = true;
				_response.Token = internetBankLoginResponse.Token;
				RegistryHelper.StoreToRegistry(_request.BankCode, _login.Text, "Internet Bank");
			}
		}
		else
		{
			args.isValid = true;
		}
	}

	private static DXImage AddImageControl(LayoutGroup container, IBBankSE bankCode)
	{
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		DXImage dXImage = new DXImage
		{
			Source = ImageService.GetImage(bankCode.Icon, isAppPathPreferred: false, "Bank/"),
			Width = 60.0,
			Height = 60.0,
			Stretch = Stretch.Uniform
		};
		container.Children.Add(dXImage);
		return dXImage;
	}

	public static IBBankSE GetInfo(string account)
	{
		try
		{
			if (account.Length >= 3)
			{
				return SmartEnum<IBBankSE, string>.FromValue(account.Substring(0, 3));
			}
			return null;
		}
		catch (SmartEnumNotFoundException)
		{
			throw new ASException(string.Format(ArmSoft.SME.Common.Resources.CouldNotLoginToIBWithAccount, account));
		}
	}
}
