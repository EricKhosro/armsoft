using System;
using ArmSoft.SME.Common;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Core.UI.Controls;
using ArmSoft.SME.Core.UI.DataViews;
using ArmSoft.SME.Core.UI.Documents;
using ArmSoft.SME.Trade.Common;
using ArmSoft.SME.Trade.Common.Parameters;
using ArmSoft.SME.Trade.UI.DataViewDataProvider;
using ArmSoft.SME.Trade.UI.DropDown;
using DevExpress.Xpf.Editors;

namespace ArmSoft.SME.Trade.UI.DataView.Directories;

[DataView(10122)]
public class BonusSumsComparisonDataView : TradeDataView<BonusSumsComparisonDataProvider.DataRow, BonusSumsComparisonDataProvider.Param>
{
	public class DataViewDialog : DataViewParametersDialogWindow
	{
		private readonly BonusSumsComparisonDataView _dataView;

		private readonly DateEditControl _startDate;

		private readonly DateEditControl _endDate;

		private readonly TextEditExt _cardNumber;

		private readonly DropDownControl _cashDesk;

		private readonly DropDownControl _shiftNumber;

		public DataViewDialog(BonusSumsComparisonDataView dataView, bool useParamValues, bool isRefreshMode)
			: base(dataView)
		{
			_dataView = dataView;
			Caption = _dataView.Caption;
			Name = _dataView.Name;
			HelpId = dataView.HelpId;
			LayoutGroupExt container = AddHorizontalLayoutGroup();
			if (!isRefreshMode)
			{
				AddStoreValuesButton(container);
			}
			_startDate = AddDateControl(container, "StartDate", ArmSoft.SME.Common.Resources.DatePeriod, useLongDate: false, isRequired: false, !useParamValues);
			_startDate.EditValue = (useParamValues ? _dataView.StartDate : new DateOnly?(Settings.HostingEnvironment.DateBegin));
			_endDate = AddDateControl(container, "EndDate", string.Empty, useLongDate: false, isRequired: false, !useParamValues);
			_endDate.EditValue = (useParamValues ? _dataView.EndDate : new DateOnly?(Settings.HostingEnvironment.DateEnd));
			AddSpecialDateButton(container, _startDate, _endDate);
			string text = Settings.ParametersService.DefaultCash();
			_cashDesk = AddDropDownControl("CashDesk", ArmSoft.SME.Common.Resources.Cash, new ArmSoft.SME.Trade.UI.DropDown.Cashes());
			_cashDesk.Code = (useParamValues ? _dataView.CashDesk : text);
			_cashDesk.EditValueChanged += new SafeEventHandler<EditValueChangedEventArgs>(CashDesk_EditValueChanged).Handler;
			_shiftNumber = AddDropDownControl("CashierShiftNumber", ArmSoft.SME.Trade.Common.Resources.CashierShift, new ArmSoft.SME.Trade.UI.DropDown.CashierShift(), isRequired: false, allowAdditionButtons: true);
			_shiftNumber.Code = (useParamValues ? _dataView.ShiftNumber : TradeSettings.CreateTradeClient().CommonServices.GetOpenCashierShiftNumberByCashDesk(text));
			_shiftNumber.IsRequired = !string.IsNullOrWhiteSpace(_shiftNumber.Code) || !string.IsNullOrWhiteSpace(_cashDesk.Code);
			_shiftNumber.EditValueChanged += new SafeEventHandler<EditValueChangedEventArgs>(ShiftNumber_EditValueChanged).Handler;
			_cashDesk.IsRequired = !string.IsNullOrWhiteSpace(_shiftNumber.Code) || !string.IsNullOrWhiteSpace(_cashDesk.Code);
			_cardNumber = AddStringControl("CardNumber", ArmSoft.SME.Trade.Common.Resources.DiscountCardCode, 255, isRequired: false, storeValue: true);
			_cardNumber.Text = (useParamValues ? _dataView.CardNumber : string.Empty);
		}

		private void ShiftNumber_EditValueChanged(object sender, EditValueChangedEventArgs e)
		{
			_cashDesk.IsRequired = !string.IsNullOrWhiteSpace(_shiftNumber.Code) || !string.IsNullOrWhiteSpace(_cashDesk.Code);
			_shiftNumber.IsRequired = !string.IsNullOrWhiteSpace(_shiftNumber.Code) || !string.IsNullOrWhiteSpace(_cashDesk.Code);
			_cashDesk.DoValidate();
		}

		private void CashDesk_EditValueChanged(object sender, EditValueChangedEventArgs e)
		{
			if (!string.IsNullOrWhiteSpace(_cashDesk.Code))
			{
				if (e.OldValue != null && e.OldValue.ToString() != _cashDesk.Code)
				{
					_shiftNumber.DataView = new ArmSoft.SME.Trade.UI.DropDown.CashierShift(_cashDesk.Code.Trim());
				}
			}
			else
			{
				_shiftNumber.DataView = new ArmSoft.SME.Trade.UI.DropDown.CashierShift();
			}
			_cashDesk.IsRequired = !string.IsNullOrWhiteSpace(_shiftNumber.Code) || !string.IsNullOrWhiteSpace(_cashDesk.Code);
			_shiftNumber.IsRequired = !string.IsNullOrWhiteSpace(_shiftNumber.Code) || !string.IsNullOrWhiteSpace(_cashDesk.Code);
			_shiftNumber.DoValidate();
		}

		protected override void DialogValidation(DialogWindowValidationArgs args)
		{
			base.DialogValidation(args);
			ArmSoft.SME.Core.UI.ValidationService.ValidateWrongPeriod(_startDate.EditValue, _endDate.EditValue);
		}

		public override void ApplyParams(bool useParamValues, bool isRefreshMode)
		{
			_dataView.CardNumber = _cardNumber.Text;
			_dataView.StartDate = _startDate.EditValue;
			_dataView.EndDate = _endDate.EditValue;
			_dataView.CashDesk = _cashDesk.Code;
			_dataView.ShiftNumber = _shiftNumber.Code;
		}
	}

	public override string HelpId => "b2b_armsoft_am_DiscountServer_CompareWithTheDatabaseData";

	public override bool DialogSupported => true;

	public string CardNumber
	{
		get
		{
			return (string)Parameters["CardNumber"].Value;
		}
		set
		{
			Parameters["CardNumber"].Value = value;
		}
	}

	public DateOnly? StartDate
	{
		get
		{
			return (DateOnly?)Parameters["StartDate"].Value;
		}
		set
		{
			Parameters["StartDate"].Value = value;
		}
	}

	public DateOnly? EndDate
	{
		get
		{
			return (DateOnly?)Parameters["EndDate"].Value;
		}
		set
		{
			Parameters["EndDate"].Value = value;
		}
	}

	public string CashDesk
	{
		get
		{
			return (string)Parameters["CashDesk"].Value;
		}
		set
		{
			Parameters["CashDesk"].Value = value;
		}
	}

	public string ShiftNumber
	{
		get
		{
			return (string)Parameters["ShiftNumber"].Value;
		}
		set
		{
			Parameters["ShiftNumber"].Value = value;
		}
	}

	public BonusSumsComparisonDataView()
	{
		DataProvider = new BonusSumsComparisonDataProvider();
		AddParameter("CardNumber", ArmSoft.SME.Trade.Common.Resources.PartnerCard, string.Empty);
		AddParameter("StartDate", ArmSoft.SME.Common.Resources.DatePeriodBegin, Settings.HostingEnvironment.DateBegin, "dd/MM/yy");
		AddParameter("EndDate", ArmSoft.SME.Common.Resources.DatePeriodEnd, Settings.HostingEnvironment.DateEnd, "dd/MM/yy");
		AddParameter("CashDesk", ArmSoft.SME.Common.Resources.Cash, string.Empty);
		AddParameter("ShiftNumber", ArmSoft.SME.Trade.Common.Resources.CashierShift, string.Empty);
		FillColumnsFromDataProvider();
	}

	public override void BeforeLoadData(DataViewPanel dvp)
	{
		base.BeforeLoadData(dvp);
		if (!dvp.IsSorted)
		{
			dvp.Set_ColumnSortOrder("BonusCardNumber");
		}
		bool value = string.IsNullOrWhiteSpace(CashDesk) || string.IsNullOrWhiteSpace(ShiftNumber);
		dvp.Set_ColumnVisible("DatabaseInitialBonusRemain", value);
		dvp.Set_ColumnVisible("DiscountServerInitialBonusRemain", value);
		dvp.Set_ColumnVisible("DatabaseFinalBonusRemain", value);
		dvp.Set_ColumnVisible("DiscountServerFinalBonusRemain", value);
	}

	public override DataViewParametersDialogWindow CreateDialog(bool useParamValues, bool isRefreshMode)
	{
		return new DataViewDialog(this, useParamValues, isRefreshMode);
	}

	protected override void ApplyParametersToDataProvider()
	{
		ParametersProvider.CardNumber = CardNumber;
		ParametersProvider.StartDate = StartDate;
		ParametersProvider.EndDate = EndDate;
		ParametersProvider.CashDesk = CashDesk;
		ParametersProvider.ShiftNumber = ShiftNumber;
	}
}
