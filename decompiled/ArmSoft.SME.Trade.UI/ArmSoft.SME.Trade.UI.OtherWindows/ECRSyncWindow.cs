using System;
using System.Windows;
using ArmSoft.ECRConnector;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Extensions;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Core.UI.Controls;
using ArmSoft.SME.Trade.Common;
using ArmSoft.SME.Trade.Common.Models.Caches;
using ArmSoft.SME.Trade.Common.Parameters;
using ArmSoft.SME.Trade.UI.CustomUIElements.UserControls;
using ArmSoft.SME.Trade.UI.DropDown;
using ArmSoft.SME.Trade.UI.Helpers;
using ArmSoft.SME.Trade.UI.SmartEnums;

namespace ArmSoft.SME.Trade.UI.OtherWindows;

public class ECRSyncWindow : DialogWindow
{
	private DropDownControl _cashes;

	private ECR ECR;

	private string _cashDeskCode;

	private readonly string _touchMode = CashierDisplayInfo.GetInstance().TouchMode;

	public ECRSyncWindow(string cashDeskCode = "")
	{
		_cashDeskCode = cashDeskCode;
		Caption = ArmSoft.SME.Common.Resources.SyncECR;
		if (string.IsNullOrWhiteSpace(_cashDeskCode))
		{
			_cashes = AddDropDownControl("Cashes", ArmSoft.SME.Common.Resources.Cash, new Cashes(), isRequired: true);
			_cashes.Code = Settings.ParametersService.DefaultCash();
		}
		if (_touchMode == POSScreenModesSE.TouchScreen.Value || _touchMode == POSScreenModesSE.TouchScreenWithFavorites.Value || _touchMode == POSScreenModesSE.SampleScreen.Value || _touchMode == POSScreenModesSE.SampleScreenWithFavorites.Value)
		{
			_okButton.Height = 40.0;
			_cancelButton.Height = 40.0;
		}
		WindowValidation += ECRSyncWindow_Validation;
	}

	private void ECRSyncWindow_Validation(DialogWindow window, DialogWindowValidationArgs args)
	{
		bool flag = false;
		bool flag2 = false;
		try
		{
			if (string.IsNullOrWhiteSpace(_cashDeskCode))
			{
				_cashDeskCode = _cashes.Code;
			}
			CashCacheRow item = TradeSettings.TradeCacheService.Cash.GetItem(_cashDeskCode);
			if (item == null)
			{
				throw new ASException(string.Format(ArmSoft.SME.Trade.Common.Resources.NotInKassa, _cashDeskCode));
			}
			if (string.IsNullOrWhiteSpace(item.ECROperator) || string.IsNullOrWhiteSpace(item.ECROperatorPassword))
			{
				throw new ASException(string.Format(ArmSoft.SME.Trade.Common.Resources.ValidateECROperatorOrECROperatorPin, _cashDeskCode), ArmSoft.SME.Common.Resources.SyncECR);
			}
			if (!item.ECROperator.IsNumeric())
			{
				throw new ASException(ArmSoft.SME.Common.Resources.ECRInitFailed, ArmSoft.SME.Common.Resources.SyncECR);
			}
			ECRHelpers.InitECR(ref ECR, _cashDeskCode);
			if (ECR == null)
			{
				throw new ASException(ArmSoft.SME.Common.Resources.ECRInitFailed, ArmSoft.SME.Common.Resources.SyncECR);
			}
			ECR.Connect();
			flag = true;
			ECR.OperatorLogin(int.Parse(item.ECROperator), item.ECROperatorPassword);
			flag2 = true;
			ECR.SyncECR();
			ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Common.Resources.SyncCompleted, ArmSoft.SME.Common.Resources.SyncECR, MessageBoxButton.OK, MessageBoxImage.Asterisk);
		}
		catch (Utilities.ECRException ex)
		{
			args.isValid = false;
			Settings.ErrorHandler(new ASException(ex.Message));
		}
		catch (Exception ex2)
		{
			args.isValid = false;
			Settings.ErrorHandler(ex2);
		}
		finally
		{
			try
			{
				if (flag)
				{
					if (flag2)
					{
						ECR.OperatorLogout();
					}
					ECR.DisConnect();
				}
			}
			catch (Exception)
			{
			}
		}
	}
}
