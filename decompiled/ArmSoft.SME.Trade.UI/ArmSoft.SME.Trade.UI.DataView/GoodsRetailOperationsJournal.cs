using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
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
using ArmSoft.SME.Core.UI.Services;
using ArmSoft.SME.Trade.Client.Data.Reports;
using ArmSoft.SME.Trade.Common;
using ArmSoft.SME.Trade.Common.Parameters;
using ArmSoft.SME.Trade.Common.SmartEnums;
using ArmSoft.SME.Trade.UI.Cache;
using ArmSoft.SME.Trade.UI.DropDown;
using ArmSoft.SME.Trade.UI.Helpers;
using DevExpress.Data;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Editors;
using DevExpress.Xpf.LayoutControl;

namespace ArmSoft.SME.Trade.UI.DataView;

[DataView(10059)]
public class GoodsRetailOperationsJournal : TradeDataView<GoodsRetailOperationsJournalDataProvider.DataRow, GoodsRetailOperationsJournalDataProvider.Param>
{
	private class DataViewDialog : DataViewParametersDialogWindow
	{
		private readonly GoodsRetailOperationsJournal _dataView;

		private DateEditControl _startDate;

		private DateEditControl _endDate;

		private MultiSelectDropDownControl _storage;

		private DropDownControl _goodGroup;

		private DropDownControl _goodCode;

		private MultiSelectDropDownControl _docTypes;

		private CheckEditExt _showSums;

		private CheckEditExt _showSumsWithVat;

		private CheckEditExt _showAltUnit;

		private CheckEditExt _showCurrSums;

		private DropDownControl _subTotalType;

		private readonly GoodsFilter _goods;

		public DataViewDialog(GoodsRetailOperationsJournal dataView, bool useParamValues, bool isRefreshMode)
			: base(dataView)
		{
			_dataView = dataView;
			Caption = ArmSoft.SME.Trade.Common.Resources.MTRepRetailOpJournal;
			_goods = new GoodsFilter(shortMode: true, ItemTypesSE.Goods, "", isShowOnlyInPriceList: true);
			bool flag = Settings.ParametersService.ShowGoodsCostInReportsAndDocs();
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
			_storage = AddMultiSelectDropDownControl(layoutGroup, "Storage", ArmSoft.SME.Common.Resources.Storage, new ArmSoft.SME.Trade.UI.DropDown.Storages(true, true), isRequired: false, !useParamValues);
			_storage.SelectedString = (useParamValues ? dataView.Storage : string.Empty);
			_goodGroup = AddDropDownControl(layoutGroup, "GoodGroup", ArmSoft.SME.Common.Resources.Group, new GoodsGroups(), isRequired: false, allowAdditionButtons: false, !useParamValues);
			_goodGroup.Code = (useParamValues ? dataView.GoodGroup : string.Empty);
			_goodGroup.EditValueChanged += new SafeEventHandler<EditValueChangedEventArgs>(GoodsGroup_EditValueChanged).Handler;
			_goodCode = AddDropDownControl(layoutGroup, "GoodCode", ArmSoft.SME.Trade.Common.Resources.MaterialValue, _goods, isRequired: false, allowAdditionButtons: false, !useParamValues, isShowComment: true, 8);
			_goodCode.Code = (useParamValues ? dataView.GoodCode : string.Empty);
			int num = 17;
			List<short> list = new List<short>(num);
			CollectionsMarshal.SetCount(list, num);
			Span<short> span = CollectionsMarshal.AsSpan(list);
			span[0] = DocumentTypeSE.MTIncome.Value;
			span[1] = DocumentTypeSE.MTInvoice.Value;
			span[2] = DocumentTypeSE.MTExpense.Value;
			span[3] = DocumentTypeSE.MTMove.Value;
			span[4] = DocumentTypeSE.MTComplectation.Value;
			span[5] = DocumentTypeSE.MTDeComplectation.Value;
			span[6] = DocumentTypeSE.MTOverheadExp.Value;
			span[7] = DocumentTypeSE.MTReturnToSupplier.Value;
			span[8] = DocumentTypeSE.MTReturnFromCustomer.Value;
			span[9] = DocumentTypeSE.MTBill.Value;
			span[10] = DocumentTypeSE.MTBillReturn.Value;
			span[11] = DocumentTypeSE.MTIncomeImport.Value;
			span[12] = DocumentTypeSE.MTIncomeRetail.Value;
			span[13] = DocumentTypeSE.MTSaleRetail.Value;
			span[14] = DocumentTypeSE.MTRevaluation.Value;
			span[15] = DocumentTypeSE.MTRevaluationRetail.Value;
			span[16] = DocumentTypeSE.MTIncomeRem.Value;
			List<short> values = list;
			_docTypes = AddMultiSelectDropDownControl(layoutGroup, "DocTypes", ArmSoft.SME.Trade.Common.Resources.MatRepOperationType, new DocumentType(values), isRequired: false, !useParamValues);
			_docTypes.SelectedString = (useParamValues ? dataView.DocTypes : string.Empty);
			AddLine(layoutGroup);
			_showAltUnit = AddBooleanControl(layoutGroup, "ShowAltUnit", ArmSoft.SME.Trade.Common.Resources.ShowALTQUNIT, !useParamValues);
			_showAltUnit.IsChecked = useParamValues && dataView.ShowAltUnit;
			_showSums = AddBooleanControl(layoutGroup, "ShowSums", ArmSoft.SME.Trade.Common.Resources.ShowNCCSumma, !useParamValues);
			_showSums.IsReadOnly = !flag;
			_showSumsWithVat = AddBooleanControl(layoutGroup, "ShowSumsWithVat", ArmSoft.SME.Trade.Common.Resources.ShowNCCSummaWithVAT, !useParamValues);
			_showSumsWithVat.IsReadOnly = !flag;
			_showCurrSums = AddBooleanControl(layoutGroup, "ShowCurrSums", string.Format(ArmSoft.SME.Trade.Common.Resources.ShowCurrSumma, MTCurr), !useParamValues);
			_showCurrSums.IsReadOnly = !flag;
			if (!flag)
			{
				_showSums.IsChecked = false;
				_showSumsWithVat.IsChecked = false;
				_showCurrSums.IsChecked = false;
			}
			else
			{
				_showSums.IsChecked = useParamValues && dataView.ShowSums;
				_showSumsWithVat.IsChecked = useParamValues && dataView.ShowSumsWithVat;
				_showCurrSums.IsChecked = useParamValues && dataView.ShowCurrSums;
			}
			AddLine(layoutGroup);
			_subTotalType = AddDropDownControl(layoutGroup, "SubTotalType", ArmSoft.SME.Common.Resources.ShowSubTotals, new SmartEnum(typeof(SubTotalTypesSE)), isRequired: false, allowAdditionButtons: false, !useParamValues);
			_subTotalType.Code = (useParamValues ? dataView.SubTotalType : string.Empty);
			if (!isRefreshMode)
			{
				AddColumnsSelectionTab(tc, layoutGroup, null);
			}
		}

		private void GoodsGroup_EditValueChanged(object sender, EditValueChangedEventArgs e)
		{
			if (e.OldValue != e.NewValue)
			{
				_goods.Group = (string)e.NewValue;
			}
		}

		public override void ApplyParams(bool useParamValues, bool isRefreshMode)
		{
			if (_dataView.DocISN == Guid.Empty)
			{
				_dataView.StartDate = _startDate.EditValue.Value;
				_dataView.EndDate = _endDate.EditValue.Value;
			}
			_dataView.Storage = _storage.SelectedString;
			_dataView.GoodGroup = _goodGroup.Code;
			_dataView.GoodCode = _goodCode.Code;
			_dataView.DocTypes = _docTypes.SelectedString;
			_dataView.ShowSums = _showSums.IsChecked.Value;
			_dataView.ShowSumsWithVat = _showSumsWithVat.IsChecked.Value;
			_dataView.ShowCurrSums = _showCurrSums.IsChecked.Value;
			_dataView.ShowAltUnit = _showAltUnit.IsChecked.Value;
			_dataView.SubTotalType = _subTotalType.Code;
		}

		protected override void DialogValidation(DialogWindowValidationArgs args)
		{
			try
			{
				base.DialogValidation(args);
				if (_dataView.DocISN == Guid.Empty)
				{
					ArmSoft.SME.Core.UI.ValidationService.ValidateWrongPeriod(_startDate.EditValue, _endDate.EditValue);
					args.isValid = TradeDataViewParametersDialogWindow.CheckPeriodForActualness(_endDate.EditValue.Value);
				}
			}
			catch (Exception ex)
			{
				Settings.ErrorHandler(ex);
				args.isValid = false;
			}
		}
	}

	private static readonly string MTCurr = Settings.ParametersService.MTCurr();

	private List<ViewColumn> potentiallyColumns;

	public override string Name => "GoodsRetailOperationsJournal";

	public override LoadIndication LoadingIndication => LoadIndication.Always;

	public override string HelpId => "Sales_Reports_OperationsRetail";

	public override bool DialogSupported => true;

	public override List<ViewColumn> PotentiallyColumns => potentiallyColumns;

	public override string Caption
	{
		get
		{
			if (DocISN == Guid.Empty)
			{
				return $"{ArmSoft.SME.Trade.Common.Resources.MTRepRetailOpJournal} - {StartDate.FormatDDMMYY()} - {EndDate.FormatDDMMYY()}";
			}
			return ArmSoft.SME.Trade.Common.Resources.MTRepRetailOpJournal + " - " + AdditionalCaption;
		}
	}

	public override bool AllowEdit => AllowOperation;

	public override bool AllowView => true;

	public override bool AllowDelete => AllowOperation;

	public override bool AllowPreview => true;

	public override bool AllowDocumentHistory => true;

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

	public string Storage
	{
		get
		{
			return Convert.ToString(Parameters["Storage"].Value);
		}
		set
		{
			Parameters["Storage"].Value = value;
		}
	}

	public string GoodGroup
	{
		get
		{
			return Convert.ToString(Parameters["GoodGroup"].Value);
		}
		set
		{
			Parameters["GoodGroup"].Value = value;
		}
	}

	public string GoodCode
	{
		get
		{
			return Convert.ToString(Parameters["GoodCode"].Value);
		}
		set
		{
			Parameters["GoodCode"].Value = value;
		}
	}

	public string DocTypes
	{
		get
		{
			return Convert.ToString(Parameters["DocTypes"].Value);
		}
		set
		{
			Parameters["DocTypes"].Value = value;
		}
	}

	public bool ShowSums
	{
		get
		{
			return Convert.ToBoolean(Parameters["ShowSums"].Value);
		}
		set
		{
			Parameters["ShowSums"].Value = value;
		}
	}

	public bool ShowSumsWithVat
	{
		get
		{
			return Convert.ToBoolean(Parameters["ShowSumsWithVat"].Value);
		}
		set
		{
			Parameters["ShowSumsWithVat"].Value = value;
		}
	}

	public bool ShowCurrSums
	{
		get
		{
			return Convert.ToBoolean(Parameters["ShowCurrSums"].Value);
		}
		set
		{
			Parameters["ShowCurrSums"].Value = value;
		}
	}

	public bool ShowAltUnit
	{
		get
		{
			return Convert.ToBoolean(Parameters["ShowAltUnit"].Value);
		}
		set
		{
			Parameters["ShowAltUnit"].Value = value;
		}
	}

	public string SubTotalType
	{
		get
		{
			return Convert.ToString(Parameters["SubTotalType"].Value);
		}
		set
		{
			Parameters["SubTotalType"].Value = value;
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

	public GoodsRetailOperationsJournal()
	{
		DataProvider = new GoodsRetailOperationsJournalDataProvider(TradeSettings.CreateTradeClient());
		AddParameter("StartDate", ArmSoft.SME.Common.Resources.DatePeriodBegin, null, "dd/MM/yy");
		AddParameter("EndDate", ArmSoft.SME.Common.Resources.DatePeriodEnd, null, "dd/MM/yy");
		AddParameter("Storage", ArmSoft.SME.Common.Resources.Storage, "");
		AddParameter("GoodGroup", ArmSoft.SME.Common.Resources.Group, "");
		AddParameter("GoodCode", ArmSoft.SME.Trade.Common.Resources.MaterialValue, "");
		AddParameter("DocTypes", ArmSoft.SME.Trade.Common.Resources.MatRepOperationType, "");
		AddParameter("ShowSums", ArmSoft.SME.Trade.Common.Resources.ShowNCCSumma, false);
		AddParameter("ShowSumsWithVat", ArmSoft.SME.Trade.Common.Resources.ShowNCCSummaWithVAT, false);
		AddParameter("ShowCurrSums", string.Format(ArmSoft.SME.Trade.Common.Resources.ShowCurrSumma, MTCurr), false);
		AddParameter("ShowAltUnit", ArmSoft.SME.Trade.Common.Resources.ShowALTQUNIT, false);
		AddParameter("DocISN", ArmSoft.SME.Common.Resources.DocumentISN, Guid.Empty);
		AddParameter("SubTotalType", ArmSoft.SME.Common.Resources.ShowSubTotals, "");
		AddParameter("AdditionalCaption", ArmSoft.SME.Trade.Common.Resources.AdditionalCaption, string.Empty);
		FillColumnsFromDataProvider();
		potentiallyColumns = new List<ViewColumn>();
		potentiallyColumns.AddRange(Columns.ViewList);
		potentiallyColumns.AddRange(MaterialCache.GetDataViewColumns(ArmSoft.SME.Trade.Common.Resources.MaterialValue, "", ""));
		potentiallyColumns.AddRange(ProducerCache.GetDataViewColumns(ArmSoft.SME.Common.Resources.Producer, "", ""));
	}

	protected override void SetColumnsProperties()
	{
		base.SetColumnsProperties();
		Columns["Cost"].IsSystem = true;
		Columns["CostSum"].IsSystem = true;
		Columns["CostWithVAT"].IsSystem = true;
		Columns["CostSumWithVAT"].IsSystem = true;
		Columns["VATSum"].IsSystem = true;
		Columns["CurCost"].IsSystem = true;
		Columns["CurrCostSumm"].IsSystem = true;
		Columns["Price"].IsSystem = true;
		Columns["PriceSum"].IsSystem = true;
		Columns["GroupCodeCaption"].IsSystem = true;
	}

	public override DataViewParametersDialogWindow CreateDialog(bool useParamValues, bool isRefreshMode)
	{
		if (!TradeSettings.CreateTradeClient().CommonServices.ExistsAvailableStorage())
		{
			ArmSoft.SME.Core.UI.MessageBox.Show(string.Format(ArmSoft.SME.Common.Resources.AccessToStoragesDenied, Settings.LoginService.UserName), Settings.ProgramName, MessageBoxButton.OK, MessageBoxImage.Exclamation);
			return null;
		}
		return new DataViewDialog(this, useParamValues, isRefreshMode);
	}

	protected override void ApplyParametersToDataProvider()
	{
		ParametersProvider.StartDate = StartDate;
		ParametersProvider.EndDate = EndDate;
		ParametersProvider.Storage = Storage;
		ParametersProvider.GoodGroup = GoodGroup;
		ParametersProvider.GoodCode = GoodCode;
		ParametersProvider.DocTypes = DocTypes;
		ParametersProvider.ShowSums = ShowSums;
		ParametersProvider.ShowSumsWithVat = ShowSumsWithVat;
		ParametersProvider.ShowCurrSums = ShowCurrSums;
		ParametersProvider.ShowAltUnit = ShowAltUnit;
		ParametersProvider.DocISN = DocISN;
	}

	public override void BeforeLoadData(DataViewPanel dvp)
	{
		base.BeforeLoadData(dvp);
		dvp.RemoveGrouping();
		dvp.Set_ColumnVisible("Cost", ShowSums);
		dvp.Set_ColumnVisible("CostSum", ShowSums);
		dvp.Set_ColumnVisible("CostWithVAT", ShowSumsWithVat);
		dvp.Set_ColumnVisible("CostSumWithVAT", ShowSumsWithVat);
		dvp.Set_ColumnVisible("CurCost", ShowCurrSums);
		dvp.Set_ColumnVisible("CurrCostSumm", ShowCurrSums);
		dvp.Set_ColumnVisible("VATSum", ShowSums && ShowSumsWithVat);
		dvp.Set_ColumnVisible("GroupCodeCaption", value: false);
		if (!dvp.IsSorted)
		{
			dvp.Set_ColumnSortOrder("Date");
			dvp.Set_ColumnSortOrder("DocNum");
		}
		if (!string.IsNullOrWhiteSpace(SubTotalType))
		{
			dvp.Set_ColumnGroupSummary("Quantity", SummaryItemType.Sum);
			dvp.Set_ColumnGroupSummary("PriceSum", SummaryItemType.Sum);
			switch (SubTotalType)
			{
			case "1":
				dvp.GroupByColumns("Storage");
				break;
			case "2":
				dvp.GroupByColumns("GroupCodeCaption");
				break;
			case "3":
				dvp.GroupByColumns("GoodCode");
				break;
			}
		}
	}

	public override void InitContextFunctions()
	{
		base.InitContextFunctions();
		DataViewPanel currentViewPanel = Settings.HostingEnvironment.CurrentViewPanel;
		GoodsRetailOperationsJournalDataProvider.DataRow dataRow = (GoodsRetailOperationsJournalDataProvider.DataRow)currentViewPanel.FocusedRow();
		if (dataRow != null && AllowOperation)
		{
			DefinitionMappingsService.TryResolveDocumentDefinition(dataRow.DocType, out var definition);
			if (definition.AllowStoreAsDraft || (currentViewPanel.GetSelectedAny() && Settings.ParametersService.DraftSelected()))
			{
				currentViewPanel.AddContextFunction("ContextFunctions", "SetDocDraftState", ArmSoft.SME.Common.Resources.SetDocDraft, SetDocDraftState, "", "", storeOpContext: true);
			}
		}
	}

	protected override void SetCacheRow(ExtendableRow row)
	{
		GoodsRetailOperationsJournalDataProvider.DataRow dataRow = row as GoodsRetailOperationsJournalDataProvider.DataRow;
		dataRow.MaterialCacheRow = TradeSettings.TradeCacheService.Material.GetItem(dataRow.GoodCode);
		dataRow.ProducerCacheRow = TradeSettings.TradeCacheService.Producer.GetItem(dataRow.MaterialCacheRow?.Producer);
	}

	private void SetDocDraftState()
	{
		InitContextFunctionsHelper.ChangeDocState((from p in Settings.HostingEnvironment.CurrentViewPanel.GetSelectedOrFocusedRows<GoodsRetailOperationsJournalDataProvider.DataRow>()
			select p.Base).ToList(), DocumentStateSE.Draft);
	}
}
