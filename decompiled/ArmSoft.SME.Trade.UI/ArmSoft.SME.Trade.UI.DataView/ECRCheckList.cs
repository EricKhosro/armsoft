using System;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Extensions;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Core.UI.Controls;
using ArmSoft.SME.Core.UI.DataViews;
using ArmSoft.SME.Core.UI.Documents;
using ArmSoft.SME.Core.UI.DropDowns;
using ArmSoft.SME.Trade.Client.Data.Reports;
using ArmSoft.SME.Trade.Common;
using ArmSoft.SME.Trade.Common.Parameters;
using ArmSoft.SME.Trade.Common.SmartEnums;
using ArmSoft.SME.Trade.UI.DropDown;

namespace ArmSoft.SME.Trade.UI.DataView;

[DataView(10044)]
public class ECRCheckList : TradeDataView<ECRCheckListDataProvider.DataRow, ECRCheckListDataProvider.Param>
{
	private class DataViewDialog : DataViewParametersDialogWindow
	{
		private readonly ECRCheckList _dataView;

		private readonly DateEditControl _startDate;

		private readonly DateEditControl _endDate;

		private readonly DropDownControl _cashCode;

		private readonly DropDownControl _partner;

		private readonly DropDownControl _employee;

		private readonly DropDownControl _docState;

		public DataViewDialog(ECRCheckList dataView, bool useParamValues, bool isRefreshMode)
			: base(dataView)
		{
			_dataView = dataView;
			Caption = _dataView.Caption;
			LayoutGroupExt container = AddHorizontalLayoutGroup();
			if (!isRefreshMode)
			{
				AddStoreValuesButton(container);
			}
			_startDate = AddDateControl(container, "StartDate", ArmSoft.SME.Common.Resources.DatePeriod, useLongDate: false, isRequired: true, !useParamValues, ArmSoft.SME.Common.Resources.DatePeriodBegin);
			_startDate.EditValue = (useParamValues ? dataView.StartDate : Settings.HostingEnvironment.DateBegin);
			_endDate = AddDateControl(container, "EndDate", "", useLongDate: false, isRequired: true, !useParamValues, ArmSoft.SME.Common.Resources.DatePeriodEnd);
			_endDate.EditValue = (useParamValues ? dataView.EndDate : Settings.HostingEnvironment.DateEnd);
			AddSpecialDateButton(container, _startDate, _endDate);
			_cashCode = AddDropDownControl("CashCode", ArmSoft.SME.Common.Resources.Cash, new ArmSoft.SME.Trade.UI.DropDown.Cashes(), isRequired: false, allowAdditionButtons: false, !useParamValues);
			_cashCode.Code = (useParamValues ? dataView.CashCode : "");
			_partner = AddDropDownControl("PartnerCode", ArmSoft.SME.Common.Resources.Partner, new PartnersFilter(), isRequired: false, allowAdditionButtons: false, !useParamValues);
			_partner.Code = (useParamValues ? dataView.PartnerCode : "");
			_employee = AddDropDownControl("EmployeeCode", ArmSoft.SME.Common.Resources.Employee, new ArmSoft.SME.Trade.UI.DropDown.Employees(), isRequired: false, allowAdditionButtons: false, !useParamValues);
			_employee.Code = (useParamValues ? dataView.EmployeeCode : "");
			_docState = AddDropDownControl("DocState", ArmSoft.SME.Common.Resources.State, new DocumentState(), isRequired: false, allowAdditionButtons: false, !useParamValues);
			_docState.Code = (useParamValues ? dataView.DocState : "");
		}

		protected override void DialogValidation(DialogWindowValidationArgs args)
		{
			try
			{
				base.DialogValidation(args);
				ValidationService.ValidateWrongPeriod(_startDate.EditValue, _endDate.EditValue);
				args.isValid = TradeDataViewParametersDialogWindow.CheckPeriodForActualness(_endDate.EditValue.Value);
			}
			catch (Exception ex)
			{
				Settings.ErrorHandler(ex);
				args.isValid = false;
			}
		}

		public override void ApplyParams(bool useParamValues, bool isRefreshMode)
		{
			_dataView.StartDate = _startDate.EditValue.Value;
			_dataView.EndDate = _endDate.EditValue.Value;
			_dataView.CashCode = _cashCode.Code;
			_dataView.PartnerCode = _partner.Code;
			_dataView.EmployeeCode = _employee.Code;
			_dataView.DocState = _docState.Code;
		}
	}

	public override string HelpId => "ECRCheck_Documents_List";

	public override bool DialogSupported => true;

	public override bool AllowEdit => AllowOperation;

	public override bool AllowDelete => AllowOperation;

	public override bool AllowView => true;

	public override bool AllowPreview => true;

	public override bool AllowDocumentHistory => true;

	public DateOnly StartDate
	{
		get
		{
			return ((DateTime)Parameters["StartDate"].Value).ToDateOnly();
		}
		set
		{
			Parameters["StartDate"].Value = value.ToDateTime();
		}
	}

	public DateOnly EndDate
	{
		get
		{
			return ((DateTime)Parameters["EndDate"].Value).ToDateOnly();
		}
		set
		{
			Parameters["EndDate"].Value = value.ToDateTime();
		}
	}

	public string CashCode
	{
		get
		{
			return (string)Parameters["CashCode"].Value;
		}
		set
		{
			Parameters["CashCode"].Value = value;
		}
	}

	public string PartnerCode
	{
		get
		{
			return (string)Parameters["PartnerCode"].Value;
		}
		set
		{
			Parameters["PartnerCode"].Value = value;
		}
	}

	public string EmployeeCode
	{
		get
		{
			return (string)Parameters["EmployeeCode"].Value;
		}
		set
		{
			Parameters["EmployeeCode"].Value = value;
		}
	}

	public string DocState
	{
		get
		{
			return (string)Parameters["DocState"].Value;
		}
		set
		{
			Parameters["DocState"].Value = value;
		}
	}

	public ECRCheckList()
	{
		DataProvider = new ECRCheckListDataProvider(TradeSettings.CreateTradeClient());
		AddParameter("StartDate", ArmSoft.SME.Common.Resources.DatePeriodBegin, Settings.HostingEnvironment.DateBegin, "dd/MM/yy");
		AddParameter("EndDate", ArmSoft.SME.Common.Resources.DatePeriodEnd, Settings.HostingEnvironment.DateEnd, "dd/MM/yy");
		AddParameter("CashCode", ArmSoft.SME.Common.Resources.Cash, string.Empty);
		AddParameter("PartnerCode", ArmSoft.SME.Common.Resources.Partner, string.Empty);
		AddParameter("EmployeeCode", ArmSoft.SME.Common.Resources.Employee, string.Empty);
		AddParameter("DocState", ArmSoft.SME.Common.Resources.State, "", "", includeCaptionToolTip: false, new SmartEnum(typeof(DocumentStateSE)));
		FillColumnsFromDataProvider();
	}

	public override void BeforeLoadData(DataViewPanel dvp)
	{
		base.BeforeLoadData(dvp);
		dvp.Set_ColumnVisible("Discount", Settings.ParametersService.DiscountUse());
		dvp.Set_ColumnVisible("DiscountPrice", Settings.ParametersService.DiscountUse());
		dvp.Set_ColumnVisible("DiscountSumma", Settings.ParametersService.DiscountUse());
		if (Settings.ParametersService.DiscountUse())
		{
			dvp.Set_ColumnCaption("Summa", ArmSoft.SME.Trade.Common.Resources.DiscountedSumm);
		}
		dvp.Set_ColumnVisible("UsedBonus", Settings.ParametersService.UseBonus());
		dvp.Set_ColumnVisible("AccumulatedBonus", Settings.ParametersService.UseBonus());
		dvp.Set_ColumnVisible("PartTaxCode", Settings.ParametersService.ShowPartnerTaxCode());
		if (!dvp.IsSorted)
		{
			dvp.Set_ColumnSortOrder("DocDate");
			dvp.Set_ColumnSortOrder("DocNum");
		}
	}

	public override DataViewParametersDialogWindow CreateDialog(bool useParamValues, bool isRefreshMode)
	{
		if (!TradeSettings.CreateTradeClient().CommonServices.ExistsAvailableStorage())
		{
			throw new ASException(string.Format(ArmSoft.SME.Common.Resources.AccessToStoragesDenied, Settings.LoginService.UserName), 48);
		}
		if (!TradeSettings.CreateTradeClient().CommonServices.ExistsAvailableCashDesk())
		{
			throw new ASException(ArmSoft.SME.Trade.Common.Resources.AccessToCashesDenied, 48);
		}
		return new DataViewDialog(this, useParamValues, isRefreshMode);
	}

	protected override void ApplyParametersToDataProvider()
	{
		ParametersProvider.StartDate = StartDate;
		ParametersProvider.EndDate = EndDate;
		ParametersProvider.CashCode = CashCode;
		ParametersProvider.PartnerCode = PartnerCode;
		ParametersProvider.EmployeeCode = EmployeeCode;
		ParametersProvider.DocState = DocState;
	}
}
