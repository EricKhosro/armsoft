using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Extensions;
using ArmSoft.SME.Common.Helpers;
using ArmSoft.SME.Common.Parameters;
using ArmSoft.SME.Core.UI.Controls;
using ArmSoft.SME.Core.UI.Services;
using Microsoft.Extensions.DependencyInjection;

namespace ArmSoft.SME.Core.UI.StandartWindow;

public class PinChangeWindow : DialogWindow
{
	private readonly PasswordBoxEditExt _oldPin;

	private readonly PasswordBoxEditExt _pin;

	private readonly PasswordBoxEditExt _confirmPin;

	public PinChangeWindow()
	{
		Caption = ArmSoft.SME.Common.Resources.ChangePinCode;
		_oldPin = AddPasswordControl("OldPIN", ArmSoft.SME.Common.Resources.OldPINCode, 4);
		_pin = AddPasswordControl("PIN", ArmSoft.SME.Common.Resources.NewPINCode, 4);
		_confirmPin = AddPasswordControl("ConfirmPIN", ArmSoft.SME.Common.Resources.ConfirmPINCode, 4);
	}

	protected override void DialogValidation(DialogWindowValidationArgs args)
	{
		string text = Settings.ParametersService.UserPIN();
		if (EncryptHelper.GenerateHashedString(_oldPin.Text.TrimEnd()) != text && (!string.IsNullOrWhiteSpace(_oldPin.Text) || !string.IsNullOrWhiteSpace(text)))
		{
			throw new ASException(ArmSoft.SME.Common.Resources.PINIsIncorrect);
		}
		string text2 = _pin.Text.Trim();
		if (text2 != _confirmPin.Text.Trim())
		{
			throw new ASException(ArmSoft.SME.Common.Resources.PINCodesNotMatched);
		}
		if (!string.IsNullOrWhiteSpace(text2))
		{
			if (!text2.IsNumeric())
			{
				throw new ASException(ArmSoft.SME.Common.Resources.IsNotNumeric);
			}
			if (text2.Length != 4)
			{
				throw new ASException(string.Format(ArmSoft.SME.Common.Resources.FieldLengthMustBeEqualTo, ArmSoft.SME.Common.Resources.PINCode, 4));
			}
		}
		base.DialogValidation(args);
		Settings.ParametersService.UserPIN_set(string.IsNullOrWhiteSpace(text2) ? "" : EncryptHelper.GenerateHashedString(text2), Settings.LoginService.UserId);
		Settings.ServiceProvider.GetRequiredService<ILocalDBLoginInfo>().PIN = Settings.ParametersService.UserPIN();
	}
}
