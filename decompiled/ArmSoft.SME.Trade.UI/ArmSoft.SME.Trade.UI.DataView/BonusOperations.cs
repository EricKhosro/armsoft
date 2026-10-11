using System;
using System.Windows;
using System.Windows.Controls;
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

namespace ArmSoft.SME.Trade.UI.DataView;

[DataView(10054)]
public class BonusOperations : TradeDataView<BonusOperationsDataProvider.DataRow, BonusOperationsDataProvider.Param>
{
	private class DataViewDialog : TradeDataViewParametersDialogWindow
	{
		private readonly BonusOperations _dataView;

		private readonly DateEditControl _startDate;

		private readonly DateEditControl _endDate;

		private readonly PartnerContractGroupControls _partnerContractControl;

		private readonly MultiSelectDropDownControl _docTypes;

		private readonly CheckEditExt _showLinks;

		public DataViewDialog(BonusOperations dataView, bool useParamValues, bool isRefreshMode)
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
			if (dataView.DocISN != Guid.Empty)
			{
				_startDate.IsRequired = false;
				((Control)(object)_startDate.Parent).Visibility = Visibility.Collapsed;
				_endDate.IsRequired = false;
				((Control)(object)_endDate.Parent).Visibility = Visibility.Collapsed;
			}
			LayoutGroupExt lgPartCont = AddVerticalLayoutGroup();
			_partnerContractControl = AddPartnerContractGroupControls();
			_partnerContractControl.ConstructDlgUI(lgPartCont, !useParamValues, "", isFilterBySuppliers: false, isFilterByCustomers: false, isShowContractCurrCode: false, isShowSummaDebt: false, "", "", showOnlyBonusContracts: true);
			if (useParamValues)
			{
				_partnerContractControl.PartnerGroup.Code = _dataView.PartnerGroup;
				_partnerContractControl.PartnerCode.Code = _dataView.PartnerCode;
				_partnerContractControl.Contract.Code = _dataView.Contract;
			}
			AddLine();
			_docTypes = AddMultiSelectDropDownControl("DocTypes", ArmSoft.SME.Common.Resources.TypeWord, new DocumentType(CommonHelpers.GetDocumentTypesByReportType(ReportTypesSE.BonusDocument)), isRequired: false, !useParamValues);
			_docTypes.SelectedString = (useParamValues ? dataView.DocType : string.Empty);
			AddLine();
			_showLinks = AddBooleanControl("ShowLinks", ArmSoft.SME.Trade.Common.Resources.ActivateLinksForCodeColumn, !useParamValues);
			_showLinks.EditValue = useParamValues && dataView.ShowLinks;
		}

		protected override void DialogValidation(DialogWindowValidationArgs args)
		{
			try
			{
				base.DialogValidation(args);
				if (_dataView.DocISN == Guid.Empty)
				{
					ValidationService.ValidateWrongPeriod(_startDate.EditValue, _endDate.EditValue);
					args.isValid = TradeDataViewParametersDialogWindow.CheckPeriodForActualness(_endDate.EditValue.Value);
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
			_dataView.DocType = _docTypes.SelectedString;
			_dataView.ShowLinks = _showLinks.IsChecked.Value;
		}
	}

	public override string HelpId => "Purchases_Reports_BonusReports_PartnersOperations";

	public override bool AllowEdit => AllowOperation;

	public override bool AllowView => true;

	public override bool AllowDelete => AllowOperation;

	public override bool AllowCreateCopy => AllowOperation;

	public override bool AllowPreview => true;

	public override bool AllowDocumentHistory => true;

	public override bool DialogSupported => true;

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

	public override LoadIndication LoadingIndication => LoadIndication.Always;

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

	public BonusOperations()
	{
		DataProvider = new BonusOperationsDataProvider(TradeSettings.CreateTradeClient());
		AddParameter("StartDate", ArmSoft.SME.Common.Resources.DatePeriodBegin, null, "dd/MM/yy");
		AddParameter("EndDate", ArmSoft.SME.Common.Resources.DatePeriodEnd, null, "dd/MM/yy");
		AddParameter("PartnerGroup", ArmSoft.SME.Common.Resources.Group, string.Empty);
		AddParameter("PartnerCode", ArmSoft.SME.Common.Resources.Partner, string.Empty);
		AddParameter("Contract", ArmSoft.SME.Common.Resources.Contract, string.Empty);
		AddParameter("DocType", ArmSoft.SME.Common.Resources.DocumentType, string.Empty);
		AddParameter("DocISN", ArmSoft.SME.Common.Resources.DocumentISN, Guid.Empty);
		AddParameter("ShowLinks", ArmSoft.SME.Trade.Common.Resources.ActivateLinksForCodeColumn, false);
		AddParameter("AdditionalCaption", ArmSoft.SME.Trade.Common.Resources.AdditionalCaption, string.Empty);
		FillColumnsFromDataProvider();
	}

	public override DataViewParametersDialogWindow CreateDialog(bool useParamValues, bool isRefreshMode)
	{
		return new DataViewDialog(this, useParamValues, isRefreshMode);
	}

	protected override void ApplyParametersToDataProvider()
	{
		ParametersProvider.StartDate = StartDate;
		ParametersProvider.EndDate = EndDate;
		ParametersProvider.PartnerGroup = PartnerGroup;
		ParametersProvider.PartnerCode = PartnerCode;
		ParametersProvider.Contract = Contract;
		ParametersProvider.DocType = DocType;
		ParametersProvider.DocISN = DocISN;
	}

	public override void BeforeLoadData(DataViewPanel dvp)
	{
		base.BeforeLoadData(dvp);
		if (!dvp.IsSorted)
		{
			dvp.Set_ColumnSortOrder("Date");
		}
		Columns.AddDirectoryLinkColumn(DirectoryTypeSE.Partner, "PartCode", "PartName", "", "", ShowLinks);
	}
}
