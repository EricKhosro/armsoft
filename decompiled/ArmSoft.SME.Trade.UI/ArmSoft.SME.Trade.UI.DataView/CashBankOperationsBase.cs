using System;
using System.Windows;
using System.Windows.Controls;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Extensions;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Core.UI.Controls;
using ArmSoft.SME.Core.UI.DataViews;
using ArmSoft.SME.Core.UI.DropDowns;
using ArmSoft.SME.Trade.Client.Data.Reports;
using ArmSoft.SME.Trade.Common;
using ArmSoft.SME.Trade.Common.Helpers;
using ArmSoft.SME.Trade.Common.Models.Caches;
using ArmSoft.SME.Trade.Common.Parameters;
using ArmSoft.SME.Trade.Common.SmartEnums;
using ArmSoft.SME.Trade.UI.DropDown;
using DevExpress.Xpf.Editors;

namespace ArmSoft.SME.Trade.UI.DataView;

public class CashBankOperationsBase<R, P> : TradeDataView<CashBankOperationsDataProvider.DataRow, CashBankOperationsDataProvider.Param>
{
	private class DataViewDialog : TradeDataViewParametersDialogWindow
	{
		private readonly CashBankOperationsBase<R, P> _dataView;

		private readonly DateEditControl _startDate;

		private readonly DateEditControl _endDate;

		private readonly DropDownControl _cashBankCode;

		private readonly DropDownControl _currCode;

		private readonly DropDownControl _repCurrType;

		private readonly CheckEditExt _showPartners;

		private readonly DropDownControl _partner;

		private readonly MultiSelectDropDownControl _docTypes;

		public DataViewDialog(CashBankOperationsBase<R, P> dataView, bool useParamValues, bool isRefreshMode)
			: base(dataView)
		{
			_dataView = dataView;
			Caption = (dataView._isCashDeskOp ? ArmSoft.SME.Trade.Common.Resources.KassaOpJournal : ArmSoft.SME.Trade.Common.Resources.BankOpJournal);
			LayoutGroupExt container = AddVerticalLayoutGroup();
			LayoutGroupExt container2 = AddHorizontalLayoutGroup(container);
			if (!isRefreshMode)
			{
				AddStoreValuesButton(container2);
			}
			_startDate = AddDateControl(container2, "StartDate", ArmSoft.SME.Common.Resources.DatePeriod, useLongDate: false, isRequired: true, !useParamValues, ArmSoft.SME.Common.Resources.DatePeriodBegin);
			_startDate.EditValue = (useParamValues ? dataView.StartDate : new DateOnly?(Settings.HostingEnvironment.DateBegin));
			_endDate = AddDateControl(container2, "EndDate", "", useLongDate: false, isRequired: true, !useParamValues, ArmSoft.SME.Common.Resources.DatePeriodEnd);
			_endDate.EditValue = (useParamValues ? dataView.EndDate : new DateOnly?(Settings.HostingEnvironment.DateEnd));
			AddSpecialDateButton(container2, _startDate, _endDate);
			if (dataView.DocISN != Guid.Empty)
			{
				_startDate.IsRequired = false;
				((Control)(object)_startDate.Parent).Visibility = Visibility.Collapsed;
				_endDate.IsRequired = false;
				((Control)(object)_endDate.Parent).Visibility = Visibility.Collapsed;
			}
			if (_dataView._isCashDeskOp)
			{
				_cashBankCode = AddDropDownControl(container, "CashBankCode", ArmSoft.SME.Common.Resources.Cash, new ArmSoft.SME.Trade.UI.DropDown.Cashes(), isRequired: false, allowAdditionButtons: false, !useParamValues);
			}
			else
			{
				_cashBankCode = AddDropDownControl(container, "CashBankCode", ArmSoft.SME.Common.Resources.Account, new ArmSoft.SME.Trade.UI.DropDown.AccountInBanks(isKeySettlAccount: false), isRequired: false, allowAdditionButtons: false, !useParamValues);
				_cashBankCode.EditValueChanged += new SafeEventHandler<EditValueChangedEventArgs>(AccInBankCode_EditValueChanged).Handler;
			}
			_currCode = AddDropDownControl(container, "CurrCode", ArmSoft.SME.Common.Resources.Curr, new Currency(shortMode: true), isRequired: false, allowAdditionButtons: false, !useParamValues);
			_currCode.Code = (useParamValues ? dataView.CurrCode : string.Empty);
			_cashBankCode.Code = (useParamValues ? dataView.CashBankCode : string.Empty);
			AddLine();
			_repCurrType = AddDropDownControl("ReportCurrTypes", ArmSoft.SME.Common.Resources.ReportCurr, new SmartEnum(typeof(ReportCurrTypesSE)), isRequired: true, allowAdditionButtons: false, storeValue: true);
			_repCurrType.Code = (useParamValues ? dataView.ReportCurrTypes : "1");
			if (_dataView._isCashDeskOp)
			{
				_docTypes = AddMultiSelectDropDownControl("DocType", ArmSoft.SME.Common.Resources.DocumentType, new DocumentType(CommonHelpers.GetDocumentTypesByReportType(ReportTypesSE.CashDocument)), isRequired: false, !useParamValues);
			}
			else
			{
				_docTypes = AddMultiSelectDropDownControl("DocType", ArmSoft.SME.Common.Resources.DocumentType, new DocumentType(CommonHelpers.GetDocumentTypesByReportType(ReportTypesSE.BankDocument)), isRequired: false, !useParamValues);
			}
			_docTypes.SelectedString = (useParamValues ? dataView.DocType : string.Empty);
			_showPartners = AddBooleanControl("ShowPartners", ArmSoft.SME.Common.Resources.ShowPartners, !useParamValues);
			_showPartners.EditValueChanged += new SafeEventHandler<EditValueChangedEventArgs>(ShowPartners_EditValueChanged).Handler;
			_partner = AddDropDownControl("PartnerCode", ArmSoft.SME.Common.Resources.Partner, new PartnersFilter(), isRequired: false, allowAdditionButtons: false, !useParamValues);
			_partner.Code = (useParamValues ? dataView.PartnerCode : string.Empty);
			_showPartners.IsChecked = useParamValues && dataView.ShowPartners;
			ShowPartners_EditValueChanged();
		}

		protected override void DialogValidation(DialogWindowValidationArgs args)
		{
			try
			{
				if (_dataView.DocISN == Guid.Empty)
				{
					ArmSoft.SME.Core.UI.ValidationService.ValidateWrongPeriod(_startDate.EditValue, _endDate.EditValue);
					args.isValid = TradeDataViewParametersDialogWindow.CheckPeriodForActualness(_endDate.EditValue.Value);
					base.DialogValidation(args);
				}
			}
			catch (Exception ex)
			{
				Settings.ErrorHandler(ex);
				args.isValid = false;
			}
		}

		private void AccInBankCode_EditValueChanged(object sender, EditValueChangedEventArgs e)
		{
			if (!string.IsNullOrWhiteSpace(_cashBankCode.Code))
			{
				AccountInBankCacheRow item = TradeSettings.TradeCacheService.AccountInBank.GetItem(_cashBankCode.Code);
				if (item != null)
				{
					_currCode.Code = item.Curr;
					_currCode.IsReadOnly = true;
				}
			}
			else
			{
				_currCode.IsReadOnly = false;
				_currCode.Code = string.Empty;
			}
		}

		private void ShowPartners_EditValueChanged(object sender, EditValueChangedEventArgs e)
		{
			ShowPartners_EditValueChanged();
		}

		private void ShowPartners_EditValueChanged()
		{
			_partner.IsReadOnly = !_showPartners.IsChecked.Value;
			if (_partner.IsReadOnly)
			{
				_partner.Code = string.Empty;
			}
		}

		public override void ApplyParams(bool useParamValues, bool isRefreshMode)
		{
			if (_dataView.DocISN == Guid.Empty)
			{
				_dataView.StartDate = _startDate.EditValue.Value;
				_dataView.EndDate = _endDate.EditValue.Value;
			}
			_dataView.CashBankCode = _cashBankCode.Code;
			_dataView.CurrCode = _currCode.Code;
			_dataView.ReportCurrTypes = _repCurrType.Code;
			_dataView.ShowPartners = _showPartners.IsChecked.Value;
			_dataView.PartnerCode = _partner.Code;
			_dataView.DocType = _docTypes.SelectedString;
		}
	}

	private readonly bool _isCashDeskOp;

	public override bool DialogSupported => true;

	public override bool AllowEdit => AllowOperation;

	public override bool AllowPreview => true;

	public override bool AllowDelete => AllowOperation;

	public override bool AllowCreateCopy => AllowOperation;

	public override bool AllowDocumentHistory => true;

	public override string Caption
	{
		get
		{
			if (DocISN == Guid.Empty)
			{
				return $"{base.Caption} - {StartDate.FormatDDMMYY()} - {EndDate.FormatDDMMYY()}";
			}
			return base.Caption + " - " + AdditionalCaption;
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

	public string CashBankCode
	{
		get
		{
			return (string)Parameters["CashBankCode"].Value;
		}
		set
		{
			Parameters["CashBankCode"].Value = value;
		}
	}

	public string CurrCode
	{
		get
		{
			return (string)Parameters["CurrCode"].Value;
		}
		set
		{
			Parameters["CurrCode"].Value = value;
		}
	}

	public string ReportCurrTypes
	{
		get
		{
			return (string)Parameters["ReportCurrTypes"].Value;
		}
		set
		{
			Parameters["ReportCurrTypes"].Value = value;
		}
	}

	public bool ShowPartners
	{
		get
		{
			return (bool)Parameters["ShowPartners"].Value;
		}
		set
		{
			Parameters["ShowPartners"].Value = value;
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

	public string DocType
	{
		get
		{
			return (string)Parameters["DocType"].Value;
		}
		set
		{
			Parameters["DocType"].Value = value;
		}
	}

	public Guid DocISN
	{
		get
		{
			return (Guid)Parameters["DocISN"].Value;
		}
		set
		{
			Parameters["DocISN"].Value = value;
		}
	}

	public string AdditionalCaption
	{
		get
		{
			return (string)Parameters["AdditionalCaption"].Value;
		}
		set
		{
			Parameters["AdditionalCaption"].Value = value;
		}
	}

	public CashBankOperationsBase(bool isCashDesk)
	{
		_isCashDeskOp = isCashDesk;
		AddParameter("StartDate", ArmSoft.SME.Common.Resources.DatePeriodBegin, null, "dd/MM/yy");
		AddParameter("EndDate", ArmSoft.SME.Common.Resources.DatePeriodEnd, null, "dd/MM/yy");
		AddParameter("CashBankCode", _isCashDeskOp ? ArmSoft.SME.Common.Resources.Cash : ArmSoft.SME.Common.Resources.Account, string.Empty);
		AddParameter("CurrCode", ArmSoft.SME.Common.Resources.Curr, string.Empty);
		AddParameter("ReportCurrTypes", ArmSoft.SME.Common.Resources.ReportCurr, "1");
		AddParameter("ShowPartners", ArmSoft.SME.Common.Resources.ShowPartners, false);
		AddParameter("PartnerCode", ArmSoft.SME.Common.Resources.Partner, string.Empty);
		AddParameter("DocType", ArmSoft.SME.Common.Resources.DocumentType, string.Empty);
		AddParameter("DocISN", ArmSoft.SME.Common.Resources.DocumentISN, Guid.Empty);
		AddParameter("AdditionalCaption", ArmSoft.SME.Trade.Common.Resources.AdditionalCaption, string.Empty);
	}

	public override DataViewParametersDialogWindow CreateDialog(bool useParamValues, bool isRefreshMode)
	{
		return new DataViewDialog(this, useParamValues, isRefreshMode);
	}

	public override void BeforeLoadData(DataViewPanel dvp)
	{
		base.BeforeLoadData(dvp);
		dvp.Set_ColumnVisible("BaseISN", value: false);
		dvp.Set_ColumnVisible("OpCode", value: false);
		dvp.Set_ColumnVisible("DocType", value: false);
		dvp.Set_ColumnVisible("PartId", value: false);
		dvp.Set_ColumnVisible("DbCr", value: false);
		dvp.Set_ColumnVisible("Summ", value: false);
		dvp.Set_ColumnVisible("CurrSumm", value: false);
		bool value = ReportCurrTypes != "1";
		dvp.Set_ColumnVisible("CurrSummIn", value);
		dvp.Set_ColumnVisible("CurrSummOut", value);
		bool value2 = ReportCurrTypes != "2";
		dvp.Set_ColumnVisible("SummIn", value2);
		dvp.Set_ColumnVisible("SummOut", value2);
		bool value3 = !string.IsNullOrWhiteSpace(CurrCode);
		dvp.Set_ColumnFooterSum("CurrSummIn", value3);
		dvp.Set_ColumnFooterSum("CurrSummOut", value3);
		dvp.Set_ColumnVisible("PartCode", ShowPartners);
		dvp.Set_ColumnVisible("PartName", ShowPartners);
		dvp.Set_ColumnVisible("PartnerGroup", ShowPartners);
		dvp.Set_ColumnVisible("ContractName", ShowPartners);
		dvp.Set_ColumnVisible("TaxCode", ShowPartners && Settings.ParametersService.ShowPartnerTaxCode());
		dvp.Set_ColumnFooterSum("SummIn", value: true);
		dvp.Set_ColumnFooterSum("SummOut", value: true);
		dvp.Set_ColumnFooterSum("CurrSummIn", !string.IsNullOrWhiteSpace(CurrCode));
		dvp.Set_ColumnFooterSum("CurrSummOut", !string.IsNullOrWhiteSpace(CurrCode));
		if (!dvp.IsSorted)
		{
			dvp.Set_ColumnSortOrder("Date");
		}
	}

	protected override void ApplyParametersToDataProvider()
	{
		ParametersProvider.StartDate = StartDate;
		ParametersProvider.EndDate = EndDate;
		ParametersProvider.CashBankCode = CashBankCode;
		ParametersProvider.CurrCode = CurrCode;
		ParametersProvider.ReportCurrTypes = ReportCurrTypes;
		ParametersProvider.ShowPartners = ShowPartners;
		ParametersProvider.PartnerCode = PartnerCode;
		ParametersProvider.DocType = DocType;
		ParametersProvider.DocISN = DocISN;
	}
}
