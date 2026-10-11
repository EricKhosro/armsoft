using System;
using System.Windows;
using ArmSoft.SME.Client;
using ArmSoft.SME.Client.Data.Reports;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Extensions;
using ArmSoft.SME.Common.Helpers;
using ArmSoft.SME.Common.Parameters;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Core.UI.Controls;
using ArmSoft.SME.Core.UI.Directories;
using ArmSoft.SME.Trade.Common;
using DevExpress.Xpf.Editors;

namespace ArmSoft.SME.Trade.UI.Directories;

public class UserWindow : CloudUserWindow
{
	private TextEditExt _cardNumber;

	private PasswordBoxEditExt _pin;

	private PasswordBoxEditExt _confirmPin;

	private bool _pinChanged;

	public UserWindow(UsersDataProvider.DataRow user, DialogWindowMode mode, Window owner = null)
		: base(user, mode, owner)
	{
	}

	protected override void ConstructUI()
	{
		base.ConstructUI();
		AddLine();
		_cardNumber = AddStringControl("CardNumber", ArmSoft.SME.Trade.Common.Resources.UserCardNumber, 50);
		_pin = AddPasswordControl("PIN", ArmSoft.SME.Common.Resources.PINCode, 4);
		_pin.EditValue = string.Empty;
		SafeEventHandler<EditValueChangedEventArgs> safeEventHandler = new SafeEventHandler<EditValueChangedEventArgs>(PinChanged);
		_pin.EditValueChanged += safeEventHandler.Handler;
		_confirmPin = AddPasswordControl("ConfirmPIN", ArmSoft.SME.Common.Resources.ConfirmPINCode, 4);
		_confirmPin.EditValueChanged += safeEventHandler.Handler;
		_confirmPin.EditValue = string.Empty;
	}

	private void PinChanged(object sender, EditValueChangedEventArgs e)
	{
		_pinChanged = true;
	}

	protected override void SetControlsReadOnly()
	{
		base.SetControlsReadOnly();
		_cardNumber.IsReadOnly = Mode == DialogWindowMode.ViewMode;
		_pin.IsReadOnly = Mode == DialogWindowMode.ViewMode;
		_confirmPin.IsReadOnly = Mode == DialogWindowMode.ViewMode;
	}

	protected override void LoadData()
	{
		base.LoadData();
		if (Mode == DialogWindowMode.EditMode || Mode == DialogWindowMode.ViewMode || Mode == DialogWindowMode.CreateCopyMode)
		{
			ApiClient apiClient = Settings.CreateApiClient();
			_cardNumber.EditValue = apiClient.Parameter.GetValue(Definitions.UserCardNumber.Id, _userRow.LocalUserId ?? _userRow.UserId);
			DialogWindowMode mode = Mode;
			bool flag = (uint)(mode - 3) <= 1u;
			if (flag && !string.IsNullOrWhiteSpace(apiClient.Parameter.GetValue(Definitions.UserPIN.Id, _userRow.LocalUserId ?? _userRow.UserId)))
			{
				_pin.EditValue = "****";
				_confirmPin.EditValue = _pin.EditValue;
				_pinChanged = false;
			}
		}
		else
		{
			_cardNumber.EditValue = string.Empty;
		}
	}

	protected override void DialogValidation(DialogWindowValidationArgs args)
	{
		try
		{
			string text = _pin.Text.Trim();
			if (_pinChanged)
			{
				if (text != _confirmPin.Text.Trim())
				{
					throw new ASException(ArmSoft.SME.Common.Resources.PINCodesNotMatched);
				}
				if (!string.IsNullOrWhiteSpace(text))
				{
					if (!text.IsNumeric())
					{
						throw new ASException(ArmSoft.SME.Common.Resources.IsNotNumeric);
					}
					if (text.Length != 4)
					{
						throw new ASException(string.Format(ArmSoft.SME.Common.Resources.FieldLengthMustBeEqualTo, ArmSoft.SME.Common.Resources.PINCode, 4));
					}
				}
			}
			string text2 = _cardNumber.Text;
			if (Mode == DialogWindowMode.NewMode)
			{
				if (!string.IsNullOrWhiteSpace(text2))
				{
					TradeSettings.CreateTradeClient().CommonServices.CheckUserCardsForUnique(text2, -1, isForEdit: false);
					TradeSettings.CreateTradeClient().CommonServices.CheckDiscountCardForUnique(text2, -1, "", isForEdit: false);
					TradeSettings.CreateTradeClient().CommonServices.CheckEmployeeCardForUnique(text2, -1, isForEdit: false);
				}
			}
			else if (Mode == DialogWindowMode.EditMode && !string.IsNullOrWhiteSpace(text2))
			{
				TradeSettings.CreateTradeClient().CommonServices.CheckUserCardsForUnique(text2, Convert.ToInt32(_innerUserID.EditValue), isForEdit: true);
				TradeSettings.CreateTradeClient().CommonServices.CheckDiscountCardForUnique(text2, -1, "", isForEdit: false);
				TradeSettings.CreateTradeClient().CommonServices.CheckEmployeeCardForUnique(text2, -1, isForEdit: false);
			}
			base.DialogValidation(args);
			DialogWindowMode mode = Mode;
			if ((mode == DialogWindowMode.NewMode || mode == DialogWindowMode.EditMode) ? true : false)
			{
				Settings.ParametersService.UserCardNumber_set(text2, Convert.ToInt32(_innerUserID.EditValue));
				if (_pinChanged)
				{
					Settings.ParametersService.UserPIN_set(string.IsNullOrWhiteSpace(text) ? "" : EncryptHelper.GenerateHashedString(text), Convert.ToInt32(_innerUserID.EditValue));
				}
			}
		}
		catch (Exception ex)
		{
			args.isValid = false;
			Settings.ErrorHandler(ex);
		}
	}
}
