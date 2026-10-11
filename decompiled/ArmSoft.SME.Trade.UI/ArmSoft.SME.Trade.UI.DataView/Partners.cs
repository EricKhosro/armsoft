using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using Ardalis.SmartEnum;
using ArmSoft.SME.Client.Data;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Extensions;
using ArmSoft.SME.Common.Parameters;
using ArmSoft.SME.Common.SmartEnums;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Core.UI.Controls;
using ArmSoft.SME.Core.UI.DataViews;
using ArmSoft.SME.Core.UI.Documents;
using ArmSoft.SME.Core.UI.DropDowns;
using ArmSoft.SME.Core.UI.MetaData;
using ArmSoft.SME.Trade.Client.Data.Directories;
using ArmSoft.SME.Trade.Common;
using ArmSoft.SME.Trade.Common.SmartEnums;
using ArmSoft.SME.Trade.UI.Cache;
using ArmSoft.SME.Trade.UI.DataView.Reports;
using ArmSoft.SME.Trade.UI.DropDown;
using ArmSoft.SME.Trade.UI.ReportViewers.Partner;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Editors;
using DevExpress.Xpf.LayoutControl;
using Microsoft.Extensions.DependencyInjection;

namespace ArmSoft.SME.Trade.UI.DataView;

[DataView(10009)]
public class Partners : TradeDirectoryDataView<PartnersWithLogDataProvider.DataRow, PartnersWithLogDataProvider.Param>
{
	private class DataViewDialog : DataViewParametersDialogWindow
	{
		private readonly Partners _dataView;

		private readonly DropDownControl _group;

		private readonly MultiSelectDropDownControl _partner;

		private readonly CheckEditExt _showExpandedDialog;

		private readonly PartnersFilter _partners;

		public void LoadParams()
		{
			LoadValues();
			ApplyParams(useParamValues: false, isRefreshMode: true);
		}

		public DataViewDialog(Partners dataView, bool useParamValues, bool isRefreshMode)
			: base(dataView)
		{
			_dataView = dataView;
			Caption = _dataView.Caption;
			bool showAsTransactionPanel = _dataView.ShowAsTransactionPanel;
			Name = _dataView.Name + (showAsTransactionPanel ? "Panel" : "");
			_partners = new PartnersFilter();
			LayoutGroupExt layoutGroupExt = AddHorizontalLayoutGroup();
			DXTabControl tc = (_dataView._hasExtension ? AddTabControl(null) : null);
			LayoutGroup layoutGroup = (_dataView._hasExtension ? AddTabedGroup(tc, ArmSoft.SME.Common.Resources.General) : null);
			if (!isRefreshMode | showAsTransactionPanel)
			{
				AddStoreValuesButton(layoutGroupExt);
			}
			if (!showAsTransactionPanel)
			{
				_group = AddDropDownControl(_dataView._hasExtension ? layoutGroup : layoutGroupExt, "Group", ArmSoft.SME.Common.Resources.Group, new PartnerGroups(), isRequired: false, allowAdditionButtons: false, !useParamValues);
				_group.EditValueChanged += new SafeEventHandler<EditValueChangedEventArgs>(Group_EditValueChanged).Handler;
				_group.Code = (useParamValues ? dataView.Group : string.Empty);
				_partner = AddMultiSelectDropDownControl(layoutGroup, "PartnerCode", ArmSoft.SME.Common.Resources.Partner, _partners, isRequired: false, !useParamValues);
				_partner.SelectedString = (useParamValues ? dataView.PartnerCode : string.Empty);
				AddLine(layoutGroup);
			}
			_showExpandedDialog = AddBooleanControl(layoutGroup, "ShowExtended", ArmSoft.SME.Common.Resources.Extended, storeValue: true);
			_showExpandedDialog.IsChecked = useParamValues && dataView.ShowExtendedDialog;
			if (!isRefreshMode && _dataView._hasExtension)
			{
				AddColumnsSelectionTab(tc, layoutGroup, null);
			}
		}

		private void Group_EditValueChanged(object sender, EditValueChangedEventArgs e)
		{
			if (e.OldValue != e.NewValue)
			{
				_partners.PartType = (string)e.NewValue;
			}
		}

		public override void ApplyParams(bool useParamValues, bool isRefreshMode)
		{
			_dataView.ShowExtendedDialog = _showExpandedDialog.IsChecked.Value;
			if (!_dataView.ShowAsTransactionPanel)
			{
				_dataView.PartnerCode = _partner.SelectedString;
				_dataView.Group = _group.Code;
			}
		}
	}

	private readonly bool _hasExtension;

	private readonly List<ArmSoft.SME.Core.UI.DataViews.ViewColumn> _potentiallyColumns;

	public override short DirectoryType => ArmSoft.SME.Trade.Common.SmartEnums.DirectoryTypeSE.Partner.Value;

	protected override bool IsPOWindow => true;

	public override string HelpId => "Purchases_Lists_Partners";

	public override bool DialogSupported => true;

	public override LoadIndication LoadingIndication
	{
		get
		{
			if (!ShowAsTransactionPanel)
			{
				return LoadIndication.AlwaysInside;
			}
			return LoadIndication.None;
		}
	}

	public override bool AllowEditSelected => base.AllowEdit;

	public override bool AllowEditCodeOfDirectories => Settings.ParametersService.AllowEditCodeOfDirectories();

	public override List<ArmSoft.SME.Core.UI.DataViews.ViewColumn> PotentiallyColumns => _potentiallyColumns;

	public string Group
	{
		get
		{
			return (string)Parameters["Group"].Value;
		}
		set
		{
			Parameters["Group"].Value = value;
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

	public bool ShowExtendedDialog
	{
		get
		{
			return Convert.ToBoolean(Parameters["ShowExtended"].Value);
		}
		set
		{
			Parameters["ShowExtended"].Value = value;
		}
	}

	private static string PartnerB2BStateExpression
	{
		get
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (byte item in new List<byte> { 3, 5, 6, 7, 8, 9, 10, 11, 14, 254 })
			{
				if (SmartEnum<PartnerStatusInB2BSE, byte>.TryFromValue(item, out var result))
				{
					StringBuilder stringBuilder2 = stringBuilder;
					StringBuilder.AppendInterpolatedStringHandler handler = new StringBuilder.AppendInterpolatedStringHandler(11, 2, stringBuilder2);
					handler.AppendLiteral("[");
					handler.AppendFormatted("B2BStatusName");
					handler.AppendLiteral("] = '");
					handler.AppendFormatted(result.Caption);
					handler.AppendLiteral("' or ");
					stringBuilder2.Append(ref handler);
				}
			}
			return stringBuilder.ToString().Left(stringBuilder.Length - 4);
		}
	}

	public Partners()
	{
		DocumentExtensionDefinition documentExtensionDefinition = Settings.ServiceProvider.GetRequiredService<IDocumentExtensionDefinitionService>().Load(POTypeSE.Directory.Value, ArmSoft.SME.Trade.Common.SmartEnums.DirectoryTypeSE.Partner.Value);
		_hasExtension = documentExtensionDefinition != null && documentExtensionDefinition.DataItems?.Count > 0;
		DataProvider = new PartnersWithLogDataProvider(TradeSettings.CreateTradeClient());
		FillColumnsFromDataProvider();
		AddParameter("PartnerCode", ArmSoft.SME.Common.Resources.Partner, string.Empty);
		AddParameter("Group", ArmSoft.SME.Common.Resources.Group, string.Empty);
		AddParameter("ShowExtended", ArmSoft.SME.Common.Resources.Extended, false);
		AddConditionalFormatting();
		_potentiallyColumns = new List<ArmSoft.SME.Core.UI.DataViews.ViewColumn>();
		_potentiallyColumns.AddRange(Columns.ViewList);
		_potentiallyColumns.AddRange(PartnerCache.GetDataViewColumns(ArmSoft.SME.Common.Resources.Partner, "", "", includeCode: false, includeCaption: false, includeTaxCode: true, onlyContract: false, onlyExtendedFields: true));
	}

	public override void BeforeLoadData(DataViewPanel dvp)
	{
		base.BeforeLoadData(dvp);
		dvp.Set_ColumnVisible("CreatorName", ShowExtendedDialog);
		dvp.Set_ColumnVisible("LastModifierName", ShowExtendedDialog);
		dvp.Set_ColumnVisible("CreationDate", ShowExtendedDialog);
		dvp.Set_ColumnVisible("LastModifyDate", ShowExtendedDialog);
		if (!dvp.IsSorted)
		{
			dvp.Set_ColumnSortOrder("PartCode");
		}
	}

	protected override void SetCacheRow(ExtendableRow row)
	{
		PartnersWithLogDataProvider.DataRow dataRow = row as PartnersWithLogDataProvider.DataRow;
		if (dataRow.PartCode != null)
		{
			dataRow.PartnerRow = TradeSettings.TradeCacheService.Partner.GetItem(dataRow.PartCode);
		}
	}

	public override void InitContextFunctions()
	{
		base.InitContextFunctions();
		if (ParentViewPanel.FocusedRow() != null)
		{
			if (Settings.UserAccessService.GetProviderPermission(ArmSoft.SME.Trade.Common.SmartEnums.DataProviderTypeSE.PartnerAccountsInBank).AllowView)
			{
				ParentViewPanel.AddContextFunction("ContextFunctions", "OpenPartAccInBank", ArmSoft.SME.Common.Resources.SettlementAccounts, OpenPartAccInBank);
			}
			if (Settings.UserAccessService.GetProviderPermission(ArmSoft.SME.Trade.Common.SmartEnums.DataProviderTypeSE.PartContracts).AllowView)
			{
				ParentViewPanel.AddContextFunction("ContextFunctions", "Contract", ArmSoft.SME.Common.Resources.Contracts, OpenContracts);
			}
			ParentViewPanel.AddContextSeparator("ContextFunctions");
			ParentViewPanel.AddContextFunction("ContextFunctions", "OpenPartnerReports", ArmSoft.SME.Common.Resources.Reports, OpenPartnerReports, "report_16.svg");
			if (Settings.UserAccessService.GetProviderPermission(ArmSoft.SME.Trade.Common.SmartEnums.DataProviderTypeSE.AllDocumentsJournal).AllowView)
			{
				ParentViewPanel.AddContextFunction("ContextFunctions", "Documents", ArmSoft.SME.Common.Resources.Documents, OpenDocumentsJournal, "journal_16.svg");
			}
		}
		if (Settings.UserAccessService.GetProviderPermission(ArmSoft.SME.Trade.Common.SmartEnums.DataProviderTypeSE.PartnersMT).AllowView)
		{
			ParentViewPanel.AddContextSeparator("ContextFunctions");
			ParentViewPanel.AddContextFunction("ContextFunctions", "OpenPartnersMTDirectory", ArmSoft.SME.Trade.Common.Resources.PartnersMTDirectory, OpenPartnersMTDirectory);
		}
	}

	private void OpenPartAccInBank()
	{
		if (ParentViewPanel != null)
		{
			PartnersWithLogDataProvider.DataRow dataRow = (PartnersWithLogDataProvider.DataRow)ParentViewPanel.FocusedRow();
			if (dataRow != null)
			{
				PartnerAccountInBank partnerAccountInBank = new PartnerAccountInBank();
				partnerAccountInBank.PartnerCode = dataRow.PartCode;
				partnerAccountInBank.Show();
			}
		}
	}

	private void OpenContracts()
	{
		if (ParentViewPanel != null)
		{
			PartnersWithLogDataProvider.DataRow dataRow = (PartnersWithLogDataProvider.DataRow)ParentViewPanel.FocusedRow();
			if (dataRow != null)
			{
				Contracts contracts = new Contracts();
				contracts.PartnerCode = dataRow.PartCode;
				contracts.Show();
			}
		}
	}

	private void OpenPartnerReports()
	{
		if (ParentViewPanel == null)
		{
			return;
		}
		PartnersWithLogDataProvider.DataRow dataRow = (PartnersWithLogDataProvider.DataRow)ParentViewPanel.FocusedRow();
		if (dataRow != null)
		{
			PartnerReports partnerReports = new PartnerReports();
			ModalBrowser modalBrowser = new ModalBrowser(partnerReports)
			{
				Owner = Settings.HostingEnvironment,
				WindowStartupLocation = WindowStartupLocation.CenterScreen
			};
			if (modalBrowser.Show())
			{
				CreatePartnerReport(short.Parse(partnerReports.ValueMember(modalBrowser.SelectedRowIndex)), dataRow.PartCode);
			}
		}
	}

	private static void CreatePartnerReport(short reportType, string partCode)
	{
		switch (reportType)
		{
		case 10051:
		{
			PartnersRemains partnersRemains = new PartnersRemains();
			partnersRemains.PartnerCode = partCode;
			partnersRemains.Date = Settings.HostingEnvironment.CurrentDate;
			partnersRemains.Show(useParamValues: true);
			break;
		}
		case 10066:
		{
			PartnersTurnOver partnersTurnOver = new PartnersTurnOver();
			partnersTurnOver.PartnerCode = partCode;
			partnersTurnOver.StartDate = Settings.HostingEnvironment.DateBegin;
			partnersTurnOver.EndDate = Settings.HostingEnvironment.DateEnd;
			partnersTurnOver.Show(useParamValues: true);
			break;
		}
		case 10052:
		{
			PartnersOperations partnersOperations = new PartnersOperations();
			partnersOperations.PartnerCode = partCode;
			partnersOperations.StartDate = Settings.HostingEnvironment.DateBegin;
			partnersOperations.EndDate = Settings.HostingEnvironment.DateEnd;
			partnersOperations.Show(useParamValues: true);
			break;
		}
		case 10070:
		{
			PartnerTAccountReportViewer partnerTAccountReportViewer = new PartnerTAccountReportViewer();
			partnerTAccountReportViewer.PartnerCode = partCode;
			partnerTAccountReportViewer.DateBegin = Settings.HostingEnvironment.DateBegin;
			partnerTAccountReportViewer.DateEnd = Settings.HostingEnvironment.DateEnd;
			partnerTAccountReportViewer.Show(useParamValues: true);
			break;
		}
		case 10057:
		{
			SalesAnalysis salesAnalysis = new SalesAnalysis();
			salesAnalysis.PartnerCode = partCode;
			salesAnalysis.StartDate = Settings.HostingEnvironment.DateBegin;
			salesAnalysis.EndDate = Settings.HostingEnvironment.DateEnd;
			salesAnalysis.Show(useParamValues: true);
			break;
		}
		}
	}

	private void OpenDocumentsJournal()
	{
		if (ParentViewPanel != null)
		{
			PartnersWithLogDataProvider.DataRow dataRow = (PartnersWithLogDataProvider.DataRow)ParentViewPanel.FocusedRow();
			if (dataRow != null)
			{
				AllDocumentsJournalDataView allDocumentsJournalDataView = new AllDocumentsJournalDataView();
				allDocumentsJournalDataView.StartDate = Settings.HostingEnvironment.DateBegin;
				allDocumentsJournalDataView.EndDate = Settings.HostingEnvironment.DateEnd;
				allDocumentsJournalDataView.PartnerCode = dataRow.PartCode;
				allDocumentsJournalDataView.Show(useParamValues: true);
			}
		}
	}

	private void OpenPartnersMTDirectory()
	{
		if (ParentViewPanel != null)
		{
			PartnersWithLogDataProvider.DataRow dataRow = (PartnersWithLogDataProvider.DataRow)ParentViewPanel.FocusedRow();
			if (dataRow != null)
			{
				PartnersGoods partnersGoods = new PartnersGoods();
				partnersGoods.PartCode = dataRow.PartCode;
				partnersGoods.Show(useParamValues: false, openAsChild: false, null, withoutShowDialog: true);
			}
		}
	}

	public override DataViewParametersDialogWindow CreateDialog(bool useParamValues, bool isRefreshMode)
	{
		return new DataViewDialog(this, useParamValues, isRefreshMode);
	}

	public void LoadParams()
	{
		((DataViewDialog)CreateDialog(useParamValues: false, isRefreshMode: true)).LoadParams();
	}

	protected override void ApplyParametersToDataProvider()
	{
		ParametersProvider.ShowExtended = ShowExtendedDialog;
		ParametersProvider.PartnerCode = PartnerCode;
		ParametersProvider.Group = Group;
	}

	private void AddConditionalFormatting()
	{
		UserDefinedFormat customFormat = new UserDefinedFormat
		{
			ForegroundColor = "#E50000"
		};
		AsCondititionFormat asCondititionFormat = new AsCondititionFormat
		{
			Expression = PartnerB2BStateExpression,
			FieldName = "TaxCode",
			FormatName = "Custom",
			CustomFormat = customFormat,
			IsFixed = true
		};
		AsCondititionFormat asCondititionFormat2 = new AsCondititionFormat
		{
			Expression = PartnerB2BStateExpression,
			FieldName = "B2BStatusName",
			FormatName = "Custom",
			CustomFormat = customFormat,
			IsFixed = true
		};
		ConditionalFormattingList.AddRange(new _003C_003Ez__ReadOnlyArray<AsFormatting>(new AsFormatting[2] { asCondititionFormat, asCondititionFormat2 }));
	}

	protected override void AddControlsInEditSelectedDialog(DialogWindow dialog, out bool doRefresh)
	{
		base.AddControlsInEditSelectedDialog(dialog, out doRefresh);
		DialogChangeDemandControls.AddChangeDemandDropDownControl(dialog, "Type", ArmSoft.SME.Common.Resources.Group, new PartnerGroups());
		DialogChangeDemandControls.AddChangeDemandBooleanControl(dialog, "Supplier", ArmSoft.SME.Common.Resources.Supplier);
		DialogChangeDemandControls.AddChangeDemandBooleanControl(dialog, "Customer", ArmSoft.SME.Common.Resources.Buyer);
		DialogChangeDemandControls.AddChangeDemandDropDownControl(dialog, "Sex", ArmSoft.SME.Common.Resources.Sex, new SmartEnum(typeof(GenderSE)));
	}
}
