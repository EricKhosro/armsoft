using System;
using System.Collections.Generic;
using System.Windows.Controls;
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
using DevExpress.Xpf.Editors;

namespace ArmSoft.SME.Trade.UI.OtherWindows;

public class ECRPrintReportWindow : DialogWindow
{
	private ComboBoxExt _ECRReportType;

	private ComboBoxExt _ECRTransactionType;

	private TextEditExt _ECRDepartment;

	private TextEditExt _ECRCashier;

	private DropDownControl _cashes;

	private DateEditControl _startDate;

	private DateEditControl _endDate;

	private ECR _ECR;

	private Dictionary<string, Utilities.FiscalReportTransactionTypes> _ECRTransactionTypes;

	private Dictionary<string, Utilities.FiscalReportTypes> _ECRReportTypes;

	private readonly string _touchMode = CashierDisplayInfo.GetInstance().TouchMode;

	public ECRPrintReportWindow(string cashDeskCode = "")
	{
		HelpId = "PrintERCReport";
		Caption = ArmSoft.SME.Common.Resources.PrintECRReport;
		_ECRReportTypes = new Dictionary<string, Utilities.FiscalReportTypes>
		{
			{
				"X",
				Utilities.FiscalReportTypes.X
			},
			{
				"Z",
				Utilities.FiscalReportTypes.Z
			}
		};
		_ECRTransactionTypes = new Dictionary<string, Utilities.FiscalReportTransactionTypes>
		{
			{
				"",
				(Utilities.FiscalReportTransactionTypes)0
			},
			{
				ArmSoft.SME.Common.Resources.CashSum,
				Utilities.FiscalReportTransactionTypes.ByCash
			},
			{
				ArmSoft.SME.Common.Resources.PlCardSum,
				Utilities.FiscalReportTransactionTypes.ByCard
			}
		};
		if (_touchMode == POSScreenModesSE.TouchScreen.Value || _touchMode == POSScreenModesSE.TouchScreenWithFavorites.Value || _touchMode == POSScreenModesSE.SampleScreen.Value || _touchMode == POSScreenModesSE.SampleScreenWithFavorites.Value)
		{
			_okButton.Height = 40.0;
			_cancelButton.Height = 40.0;
		}
		LayoutGroupExt container = AddHorizontalLayoutGroup();
		_startDate = AddDateControl(container, "DateBegin", ArmSoft.SME.Common.Resources.DatePeriod, useLongDate: false, isRequired: true);
		_startDate.EditValue = Settings.HostingEnvironment.CurrentDate;
		_endDate = AddDateControl(container, "DateEnd", "", useLongDate: false, isRequired: true);
		_endDate.EditValue = Settings.HostingEnvironment.CurrentDate;
		_cashes = AddDropDownControl("Cashes", ArmSoft.SME.Common.Resources.Cash, new Cashes(), isRequired: true);
		_ECRDepartment = AddStringControl("ECRDepartment", ArmSoft.SME.Common.Resources.SectionShort, 20);
		_ECRCashier = AddStringControl("ECRCashier", ArmSoft.SME.Common.Resources.ECROperator, 12);
		_cashes.EditValueChanged += new SafeEventHandler<EditValueChangedEventArgs>(Cashes_EditValueChanged).Handler;
		if (string.IsNullOrWhiteSpace(cashDeskCode))
		{
			_cashes.Code = Settings.ParametersService.DefaultCash();
		}
		else
		{
			_cashes.Code = cashDeskCode;
		}
		_ECRTransactionType = new ComboBoxExt
		{
			Width = 100.0,
			ItemsSource = _ECRTransactionTypes.Keys,
			SelectedIndex = 0
		};
		AddControl("ECRTransactionType", ArmSoft.SME.Common.Resources.Transaction, _ECRTransactionType);
		_ECRReportType = new ComboBoxExt
		{
			Width = 50.0,
			ItemsSource = _ECRReportTypes.Keys,
			SelectedValue = Utilities.FiscalReportTypes.X,
			SelectedIndex = 0
		};
		AddControl("ECRReportType", ArmSoft.SME.Common.Resources.Report, _ECRReportType);
		_ECRReportType.SelectionChanged += new SafeEventHandler<SelectionChangedEventArgs>(ECRReportType_SelectionChanged).Handler;
		if (Settings.IsOffline)
		{
			_cashes.IsReadOnly = true;
		}
		else
		{
			_cashes.IsReadOnly = false;
		}
	}

	private void Cashes_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		if (e.NewValue != null)
		{
			CashCacheRow item = TradeSettings.TradeCacheService.Cash.GetItem((string)e.NewValue);
			if (item != null)
			{
				_ECRCashier.Text = item.ECROperator;
			}
		}
	}

	protected override void DialogValidation(DialogWindowValidationArgs args)
	{
		bool flag = false;
		bool flag2 = false;
		try
		{
			base.DialogValidation(args);
			CashCacheRow item = TradeSettings.TradeCacheService.Cash.GetItem(_cashes.Code);
			if (item == null)
			{
				throw new ASException(string.Format(ArmSoft.SME.Trade.Common.Resources.NotInKassa, _cashes.Code));
			}
			if (string.IsNullOrWhiteSpace(item.ECROperator) || string.IsNullOrWhiteSpace(item.ECROperatorPassword))
			{
				throw new ASException(string.Format(ArmSoft.SME.Trade.Common.Resources.ValidateECROperatorOrECROperatorPin, _cashes.Code), ArmSoft.SME.Common.Resources.PrintECRCheck);
			}
			if (!item.ECROperator.IsNumeric())
			{
				throw new ASException(ArmSoft.SME.Common.Resources.ECRInitFailed, ArmSoft.SME.Common.Resources.PrintECRCheck);
			}
			ECRHelpers.InitECR(ref _ECR, _cashes.Code);
			if (_ECR == null)
			{
				throw new ASException(ArmSoft.SME.Common.Resources.ECRInitFailed, ArmSoft.SME.Common.Resources.PrintECRCheck);
			}
			_ECR.Connect();
			flag = true;
			_ECR.OperatorLogin(int.Parse(item.ECROperator), item.ECROperatorPassword);
			flag2 = true;
			_ECR.PrintFiscalReport(_startDate.EditValue.Value.ToDateTime().ToUniversalTime(), _endDate.EditValue.Value.ToDateTime().ToUniversalTime(), Convert.ToInt32(string.IsNullOrWhiteSpace(_ECRDepartment.Text) ? "0" : _ECRDepartment.Text), Convert.ToInt32(string.IsNullOrWhiteSpace(_ECRCashier.Text) ? "0" : _ECRCashier.Text), (int)_ECRTransactionTypes[(string)_ECRTransactionType.SelectedItem], (int)_ECRReportTypes[(string)_ECRReportType.SelectedItem]);
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
						_ECR.OperatorLogout();
					}
					_ECR.DisConnect();
				}
			}
			catch (Exception)
			{
			}
		}
	}

	private void ECRReportType_SelectionChanged(object sender, SelectionChangedEventArgs e)
	{
		if (_ECRReportTypes[(string)e.AddedItems[0]] == Utilities.FiscalReportTypes.X)
		{
			_ECRDepartment.Text = "";
			_ECRDepartment.IsEnabled = true;
			_ECRDepartment.IsReadOnly = false;
			_ECRTransactionType.SelectedIndex = 0;
			_ECRTransactionType.IsEnabled = true;
			_ECRTransactionType.IsReadOnly = false;
		}
		else
		{
			_ECRDepartment.IsEnabled = false;
			_ECRDepartment.IsReadOnly = true;
			_ECRTransactionType.IsEnabled = false;
			_ECRTransactionType.IsReadOnly = true;
		}
	}
}
