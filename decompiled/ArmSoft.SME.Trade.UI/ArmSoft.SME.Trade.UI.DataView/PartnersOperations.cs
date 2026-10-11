using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using ArmSoft.SME.Client.Data;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Extensions;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Core.UI.Controls;
using ArmSoft.SME.Core.UI.DataViews;
using ArmSoft.SME.Core.UI.Documents;
using ArmSoft.SME.Core.UI.DropDowns;
using ArmSoft.SME.Trade.Client.Data.Reports;
using ArmSoft.SME.Trade.Common;
using ArmSoft.SME.Trade.Common.Helpers;
using ArmSoft.SME.Trade.Common.SmartEnums;
using ArmSoft.SME.Trade.UI.Cache;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.LayoutControl;

namespace ArmSoft.SME.Trade.UI.DataView;

[DataView(10052)]
public class PartnersOperations : TradeDataView<PartnersOperationsDataProvider.DataRow, PartnersOperationsDataProvider.Param>
{
	private class DataViewDialog : TradeDataViewParametersDialogWindow
	{
		private readonly PartnersOperations _dataView;

		protected readonly DateEditControl _startDate;

		protected readonly DateEditControl _endDate;

		private readonly PartnerContractGroupControls _partnerContractControl;

		private readonly DropDownControl _repCurrType;

		private readonly MultiSelectDropDownControl _docTypes;

		private readonly CheckEditExt _showLinks;

		public DataViewDialog(PartnersOperations dataView, bool useParamValues, bool isRefreshMode)
			: base(dataView)
		{
			_dataView = dataView;
			Caption = dataView.InnerCaption;
			Name = dataView.Name;
			HelpId = dataView.HelpId;
			LayoutGroupExt container = AddHorizontalLayoutGroup();
			if (!isRefreshMode)
			{
				AddStoreValuesButton(container);
			}
			_startDate = AddDateControl(container, "StartDate", ArmSoft.SME.Common.Resources.DatePeriod, useLongDate: false, isRequired: true, !useParamValues, ArmSoft.SME.Common.Resources.DatePeriodBegin);
			_startDate.EditValue = (useParamValues ? dataView.StartDate : new DateOnly?(Settings.HostingEnvironment.DateBegin));
			_endDate = AddDateControl(container, "EndDate", "", useLongDate: false, isRequired: true, !useParamValues, ArmSoft.SME.Common.Resources.DatePeriodEnd);
			_endDate.EditValue = (useParamValues ? dataView.EndDate : new DateOnly?(Settings.HostingEnvironment.DateEnd));
			AddSpecialDateButton(container, _startDate, _endDate);
			DXTabControl tc = null;
			LayoutGroup layoutGroup = null;
			if (dataView.DocISN != Guid.Empty)
			{
				_startDate.IsRequired = false;
				((Control)(object)_startDate.Parent).Visibility = Visibility.Collapsed;
				_endDate.IsRequired = false;
				((Control)(object)_endDate.Parent).Visibility = Visibility.Collapsed;
			}
			else
			{
				tc = AddTabControl(null);
				layoutGroup = AddTabedGroup(tc, ArmSoft.SME.Common.Resources.General);
			}
			LayoutGroup layoutGroup2 = AddVerticalLayoutGroup(layoutGroup);
			_partnerContractControl = AddPartnerContractGroupControls();
			_partnerContractControl.ConstructDlgUI(layoutGroup2, !useParamValues);
			if (useParamValues)
			{
				_partnerContractControl.PartnerGroup.Code = dataView.PartnerGroup;
				_partnerContractControl.PartnerCode.Code = dataView.PartnerCode;
				_partnerContractControl.Contract.Code = dataView.Contract;
				_partnerContractControl.ContractCurrCode.Code = dataView.CurrCode;
			}
			AddLine(layoutGroup2);
			_repCurrType = AddDropDownControl(layoutGroup2, "ReportCurrTypes", ArmSoft.SME.Common.Resources.ReportCurr, new SmartEnum(typeof(ReportCurrTypesSE)), isRequired: true, allowAdditionButtons: false, storeValue: true);
			_repCurrType.Code = (useParamValues ? dataView.ReportCurrTypes : "1");
			_docTypes = AddMultiSelectDropDownControl(layoutGroup2, "DocType", ArmSoft.SME.Common.Resources.DocumentType, new DocumentType(CommonHelpers.GetDocumentTypesByReportType(ReportTypesSE.PartnerDocument)), isRequired: false, !useParamValues);
			_docTypes.SelectedString = (useParamValues ? dataView.DocType : string.Empty);
			AddLine(layoutGroup2);
			_showLinks = AddBooleanControl(layoutGroup2, "ShowLinks", ArmSoft.SME.Trade.Common.Resources.ActivateLinksForCodeColumn, !useParamValues);
			_showLinks.EditValue = useParamValues && dataView.ShowLinks;
			if (!isRefreshMode)
			{
				AddColumnsSelectionTab(tc, layoutGroup, null);
			}
		}

		protected override void DialogValidation(DialogWindowValidationArgs args)
		{
			try
			{
				if (_dataView.DocISN == Guid.Empty)
				{
					ValidationService.ValidateWrongPeriod(_startDate.EditValue, _endDate.EditValue);
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

		public override void ApplyParams(bool useParamValues, bool isRefreshMode)
		{
			if (_dataView.DocISN == Guid.Empty)
			{
				_dataView.StartDate = _startDate.EditValue.Value;
				_dataView.EndDate = _endDate.EditValue.Value;
			}
			_dataView.PartnerGroup = _partnerContractControl.PartnerGroup.Code;
			_dataView.PartnerCode = _partnerContractControl.PartnerCode.Code;
			_dataView.Contract = _partnerContractControl.Contract.Code;
			_dataView.CurrCode = _partnerContractControl.ContractCurrCode.Code;
			_dataView.ReportCurrTypes = _repCurrType.Code;
			_dataView.DocType = _docTypes.SelectedString;
			_dataView.ShowLinks = _showLinks.IsChecked.Value;
		}
	}

	private List<ViewColumn> potentiallyColumns;

	public override string HelpId => "Purchases_Reports_PartnerReports_PartnersOperations";

	public override bool DialogSupported => true;

	public override bool AllowEdit => AllowOperation;

	public override bool AllowView => true;

	public override bool AllowDelete => AllowOperation;

	public override bool AllowCreateCopy => AllowOperation;

	public override bool AllowPreview => true;

	public override bool AllowDocumentHistory => true;

	public override List<ViewColumn> PotentiallyColumns => potentiallyColumns;

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

	private string InnerCaption => base.Caption ?? "";

	public override LoadIndication LoadingIndication => LoadIndication.AlwaysInside;

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

	public string PartnerGroup
	{
		get
		{
			return (string)Parameters["PartnerGroup"].Value;
		}
		set
		{
			Parameters["PartnerGroup"].Value = value;
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

	public string Contract
	{
		get
		{
			return (string)Parameters["Contract"].Value;
		}
		set
		{
			Parameters["Contract"].Value = value;
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

	public bool ShowLinks
	{
		get
		{
			return (bool)Parameters["ShowLinks"].Value;
		}
		set
		{
			Parameters["ShowLinks"].Value = value;
		}
	}

	public PartnersOperations()
	{
		DataProvider = new PartnersOperationsDataProvider(TradeSettings.CreateTradeClient());
		AddParameter("StartDate", ArmSoft.SME.Common.Resources.DatePeriodBegin, null, "dd/MM/yy");
		AddParameter("EndDate", ArmSoft.SME.Common.Resources.DatePeriodEnd, null, "dd/MM/yy");
		AddParameter("DocISN", ArmSoft.SME.Common.Resources.DocumentISN, Guid.Empty);
		AddParameter("PartnerGroup", ArmSoft.SME.Common.Resources.Group, string.Empty);
		AddParameter("PartnerCode", ArmSoft.SME.Common.Resources.Partner, string.Empty);
		AddParameter("Contract", ArmSoft.SME.Common.Resources.Contract, string.Empty);
		AddParameter("CurrCode", ArmSoft.SME.Common.Resources.Curr, string.Empty);
		AddParameter("ReportCurrTypes", ArmSoft.SME.Common.Resources.ReportCurr, "1");
		AddParameter("DocType", ArmSoft.SME.Common.Resources.DocumentType, string.Empty);
		AddParameter("AdditionalCaption", ArmSoft.SME.Trade.Common.Resources.AdditionalCaption, string.Empty);
		AddParameter("ShowLinks", ArmSoft.SME.Trade.Common.Resources.ActivateLinksForCodeColumn, true);
		FillColumnsFromDataProvider();
		potentiallyColumns = new List<ViewColumn>();
		potentiallyColumns.AddRange(Columns.ViewList);
		potentiallyColumns.AddRange(PartnerCache.GetDataViewColumns(ArmSoft.SME.Common.Resources.Partner, "", ""));
	}

	protected override void SetColumnsProperties()
	{
		base.SetColumnsProperties();
		Columns["SummIn"].IsSystem = true;
		Columns["CurrSummIn"].IsSystem = true;
		Columns["SummOut"].IsSystem = true;
		Columns["CurrSummOut"].IsSystem = true;
	}

	protected override void SetCacheRow(ExtendableRow row)
	{
		PartnersOperationsDataProvider.DataRow dataRow = row as PartnersOperationsDataProvider.DataRow;
		dataRow.PartnerCacheRow = TradeSettings.TradeCacheService.Partner.GetItem(dataRow.PartCode);
	}

	public override DataViewParametersDialogWindow CreateDialog(bool useParamValues, bool isRefreshMode)
	{
		return new DataViewDialog(this, useParamValues, isRefreshMode);
	}

	public override void BeforeLoadData(DataViewPanel dvp)
	{
		base.BeforeLoadData(dvp);
		bool value = !(ReportCurrTypes == "1");
		dvp.Set_ColumnVisible("CurrSummIn", value);
		dvp.Set_ColumnVisible("CurrSummOut", value);
		bool value2 = !(ReportCurrTypes == "2");
		dvp.Set_ColumnVisible("SummIn", value2);
		dvp.Set_ColumnVisible("SummOut", value2);
		dvp.Set_ColumnFooterSum("SummIn", value: true);
		dvp.Set_ColumnFooterSum("SummOut", value: true);
		dvp.Set_ColumnFooterSum("CurrSummIn", !string.IsNullOrWhiteSpace(CurrCode));
		dvp.Set_ColumnFooterSum("CurrSummOut", !string.IsNullOrWhiteSpace(CurrCode));
		if (!dvp.IsSorted)
		{
			dvp.Set_ColumnSortOrder("Date");
		}
		Columns.AddDirectoryLinkColumn(DirectoryTypeSE.Partner, "PartCode", "PartName", "", "", ShowLinks);
	}

	protected override void ApplyParametersToDataProvider()
	{
		ParametersProvider.StartDate = StartDate;
		ParametersProvider.EndDate = EndDate;
		ParametersProvider.DocISN = DocISN;
		ParametersProvider.PartnerGroup = PartnerGroup;
		ParametersProvider.PartnerCode = PartnerCode;
		ParametersProvider.Contract = Contract;
		ParametersProvider.CurrCode = CurrCode;
		ParametersProvider.ReportCurrTypes = ReportCurrTypes;
		ParametersProvider.DocType = DocType;
	}
}
