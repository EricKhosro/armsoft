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

[DataView(10067)]
public class GoodsOperationsJournal : TradeDataView<GoodsOperationsDataProvider.DataRow, GoodsOperationsDataProvider.Param>
{
	private class DataViewDialog : DataViewParametersDialogWindow
	{
		private readonly GoodsOperationsJournal _dataView;

		private DateEditControl _startDate;

		private DateEditControl _endDate;

		private MultiSelectDropDownControl _storage;

		private DropDownControl _goodGroup;

		private DropDownControl _good;

		private MultiSelectDropDownControl _docTypes;

		private CheckEditExt _showSums;

		private CheckEditExt _showSumsWithVat;

		private CheckEditExt _showAltUnit;

		private CheckEditExt _showCurrSums;

		private CheckEditExt _showParties;

		private CheckEditExt _showSalePrice;

		private readonly CheckEditExt _showLinks;

		private DropDownControl _subTotalType;

		private readonly GoodsFilter _goods;

		public DataViewDialog(GoodsOperationsJournal dataView, bool useParamValues, bool isRefreshMode)
			: base(dataView)
		{
			_dataView = dataView;
			Caption = ArmSoft.SME.Trade.Common.Resources.MTOperationsJournal;
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
			LayoutGroup container2 = layoutGroup;
			string storage = ArmSoft.SME.Common.Resources.Storage;
			bool? checkViewOrOperationAccess = true;
			_storage = AddMultiSelectDropDownControl(container2, "Storage", storage, new ArmSoft.SME.Trade.UI.DropDown.Storages(null, checkViewOrOperationAccess), isRequired: false, !useParamValues);
			_storage.SelectedString = (useParamValues ? dataView.Storage : string.Empty);
			_goodGroup = AddDropDownControl(layoutGroup, "GoodGroup", ArmSoft.SME.Common.Resources.Group, new GoodsGroups(), isRequired: false, allowAdditionButtons: false, !useParamValues);
			_goodGroup.Code = (useParamValues ? dataView.GoodGroup : string.Empty);
			_goodGroup.EditValueChanged += new SafeEventHandler<EditValueChangedEventArgs>(GoodsGroup_EditValueChanged).Handler;
			_good = AddDropDownControl(layoutGroup, "GoodCode", ArmSoft.SME.Trade.Common.Resources.MaterialValue, _goods, isRequired: false, allowAdditionButtons: false, !useParamValues, isShowComment: true, 8);
			_good.Code = (useParamValues ? dataView.GoodCode : string.Empty);
			int num = 18;
			List<short> list = new List<short>(num);
			CollectionsMarshal.SetCount(list, num);
			Span<short> span = CollectionsMarshal.AsSpan(list);
			span[0] = DocumentTypeSE.MTIncome.Value;
			span[1] = DocumentTypeSE.MTInvoice.Value;
			span[2] = DocumentTypeSE.MTExpense.Value;
			span[3] = DocumentTypeSE.MTMove.Value;
			span[4] = DocumentTypeSE.MTMoveWithConfirmation.Value;
			span[5] = DocumentTypeSE.MTComplectation.Value;
			span[6] = DocumentTypeSE.MTDeComplectation.Value;
			span[7] = DocumentTypeSE.MTOverheadExp.Value;
			span[8] = DocumentTypeSE.MTReturnToSupplier.Value;
			span[9] = DocumentTypeSE.MTReturnFromCustomer.Value;
			span[10] = DocumentTypeSE.MTBill.Value;
			span[11] = DocumentTypeSE.MTBillReturn.Value;
			span[12] = DocumentTypeSE.MTIncomeImport.Value;
			span[13] = DocumentTypeSE.MTIncomeRetail.Value;
			span[14] = DocumentTypeSE.MTSaleRetail.Value;
			span[15] = DocumentTypeSE.MTRevaluation.Value;
			span[16] = DocumentTypeSE.MTRevaluationRetail.Value;
			span[17] = DocumentTypeSE.MTIncomeRem.Value;
			List<short> list2 = list;
			if (!Settings.ParametersService.MTMoveWithConfirmation())
			{
				list2.Remove(DocumentTypeSE.MTMoveWithConfirmation.Value);
			}
			_docTypes = AddMultiSelectDropDownControl(layoutGroup, "DocTypes", ArmSoft.SME.Trade.Common.Resources.MatRepOperationType, new DocumentType(list2), isRequired: false, !useParamValues);
			_docTypes.SelectedString = (useParamValues ? dataView.DocTypes : string.Empty);
			AddLine(layoutGroup);
			_showAltUnit = AddBooleanControl(layoutGroup, "ShowAltUnit", ArmSoft.SME.Trade.Common.Resources.ShowALTQUNIT, !useParamValues);
			_showAltUnit.IsChecked = useParamValues && dataView.ShowAltUnit;
			_showSums = AddBooleanControl(layoutGroup, "ShowSumms", ArmSoft.SME.Trade.Common.Resources.ShowNCCSumma, !useParamValues);
			_showSums.IsReadOnly = !flag;
			_showSums.EditValueChanged += new SafeEventHandler<EditValueChangedEventArgs>(ShowNccSumm_EditValueChanged).Handler;
			_showSumsWithVat = AddBooleanControl(layoutGroup, "ShowSummsWithVAT", ArmSoft.SME.Trade.Common.Resources.ShowNCCSummaWithVAT, !useParamValues);
			_showSumsWithVat.IsReadOnly = !flag;
			_showSumsWithVat.EditValueChanged += new SafeEventHandler<EditValueChangedEventArgs>(ShowNCCVATSumm_EditValueChanged).Handler;
			_showCurrSums = AddBooleanControl(layoutGroup, "ShowCurrSumms", string.Format(ArmSoft.SME.Trade.Common.Resources.ShowCurrSumma, MTCurr), !useParamValues);
			_showCurrSums.IsReadOnly = !flag;
			_showCurrSums.EditValueChanged += new SafeEventHandler<EditValueChangedEventArgs>(ShowCurrSumm_EditValueChanged).Handler;
			_showParties = AddBooleanControl(layoutGroup, "ShowParties", ArmSoft.SME.Common.Resources.ShowParties, !useParamValues);
			_showParties.IsReadOnly = !flag;
			_showParties.IsChecked = useParamValues && dataView.ShowParties;
			AddLine(layoutGroup);
			_showSalePrice = AddBooleanControl(layoutGroup, "ShowSalePrice", ArmSoft.SME.Trade.Common.Resources.ShowPrices, !useParamValues);
			_showSalePrice.IsReadOnly = !flag;
			_showSums.IsChecked = (flag & useParamValues) && dataView.ShowSumms;
			_showSumsWithVat.IsChecked = (flag & useParamValues) && dataView.ShowSummsWithVAT;
			_showCurrSums.IsChecked = (flag & useParamValues) && dataView.ShowCurrSumms;
			_showSalePrice.IsChecked = (flag & useParamValues) && dataView.ShowSalePrice;
			_showLinks = AddBooleanControl(layoutGroup, "ShowLinks", ArmSoft.SME.Trade.Common.Resources.ActivateLinksForCodeColumns, !useParamValues);
			_showLinks.IsChecked = useParamValues && dataView.ShowLinks;
			_subTotalType = AddDropDownControl(layoutGroup, "SubTotalType", ArmSoft.SME.Common.Resources.ShowSubTotals, new SmartEnum(typeof(SubTotalTypesSE)), isRequired: false, allowAdditionButtons: false, !useParamValues);
			_subTotalType.Code = (useParamValues ? dataView.SubTotalType : string.Empty);
			UpdateShowPricesControl();
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

		private void ShowNccSumm_EditValueChanged(object sender, EditValueChangedEventArgs e)
		{
			UpdateShowPricesControl();
		}

		private void ShowNCCVATSumm_EditValueChanged(object sender, EditValueChangedEventArgs e)
		{
			UpdateShowPricesControl();
		}

		private void ShowCurrSumm_EditValueChanged(object sender, EditValueChangedEventArgs e)
		{
			UpdateShowPricesControl();
		}

		private void UpdateShowPricesControl()
		{
			_showSalePrice.IsReadOnly = !Convert.ToBoolean(_showSums.IsChecked) && !Convert.ToBoolean(_showSumsWithVat.IsChecked) && !Convert.ToBoolean(_showCurrSums.IsChecked);
			if (_showSalePrice.IsReadOnly)
			{
				_showSalePrice.IsChecked = false;
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
			_dataView.GoodCode = _good.Code;
			_dataView.DocTypes = _docTypes.SelectedString;
			_dataView.ShowSumms = _showSums.IsChecked.Value;
			_dataView.ShowSummsWithVAT = _showSumsWithVat.IsChecked.Value;
			_dataView.ShowCurrSumms = _showCurrSums.IsChecked.Value;
			_dataView.ShowSalePrice = _showSalePrice.IsChecked.Value;
			_dataView.ShowAltUnit = _showAltUnit.IsChecked.Value;
			_dataView.SubTotalType = _subTotalType.Code;
			_dataView.ShowLinks = _showLinks.IsChecked.Value;
			_dataView.ShowParties = _showParties.IsChecked.Value;
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

	public override string Name => "GoodsOperationsJournal";

	public override bool AllowEdit => AllowOperation;

	public override bool AllowDelete => AllowOperation;

	public override bool AllowPreview => true;

	public override bool AllowDocumentHistory => true;

	public override List<ViewColumn> PotentiallyColumns => potentiallyColumns;

	public override LoadIndication LoadingIndication => LoadIndication.AlwaysInside;

	public override string Caption
	{
		get
		{
			if (DocISN == Guid.Empty)
			{
				return $"{ArmSoft.SME.Trade.Common.Resources.MTOperationsJournal} - {StartDate.FormatDDMMYY()} - {EndDate.FormatDDMMYY()}";
			}
			return ArmSoft.SME.Trade.Common.Resources.MTOperationsJournal + " - " + AdditionalCaption;
		}
	}

	public override string HelpId => "Purchases_Reports_Operations";

	public override bool DialogSupported => true;

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

	public bool ShowSumms
	{
		get
		{
			return Convert.ToBoolean(Parameters["ShowSumms"].Value);
		}
		set
		{
			Parameters["ShowSumms"].Value = value;
		}
	}

	public bool ShowSummsWithVAT
	{
		get
		{
			return Convert.ToBoolean(Parameters["ShowSummsWithVAT"].Value);
		}
		set
		{
			Parameters["ShowSummsWithVAT"].Value = value;
		}
	}

	public bool ShowCurrSumms
	{
		get
		{
			return Convert.ToBoolean(Parameters["ShowCurrSumms"].Value);
		}
		set
		{
			Parameters["ShowCurrSumms"].Value = value;
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

	public bool ShowLinks
	{
		get
		{
			return Convert.ToBoolean(Parameters["ShowLinks"].Value);
		}
		set
		{
			Parameters["ShowLinks"].Value = value;
		}
	}

	public bool ShowSalePrice
	{
		get
		{
			return Convert.ToBoolean(Parameters["ShowSalePrice"].Value);
		}
		set
		{
			Parameters["ShowSalePrice"].Value = value;
		}
	}

	public bool ShowParties
	{
		get
		{
			return Convert.ToBoolean(Parameters["ShowParties"].Value);
		}
		set
		{
			Parameters["ShowParties"].Value = value;
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

	public bool ForByPartiesJournal
	{
		get
		{
			return (bool)Parameters["ForByPartiesJournal"].Value;
		}
		set
		{
			Parameters["ForByPartiesJournal"].Value = value;
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

	public GoodsOperationsJournal()
	{
		DataProvider = new GoodsOperationsDataProvider(TradeSettings.CreateTradeClient());
		AddParameter("StartDate", ArmSoft.SME.Common.Resources.DatePeriodBegin, null, "dd/MM/yy");
		AddParameter("EndDate", ArmSoft.SME.Common.Resources.DatePeriodEnd, null, "dd/MM/yy");
		AddParameter("Storage", ArmSoft.SME.Common.Resources.Storage, "");
		AddParameter("GoodGroup", ArmSoft.SME.Common.Resources.Group, "");
		AddParameter("GoodCode", ArmSoft.SME.Trade.Common.Resources.MaterialValue, "");
		AddParameter("DocTypes", ArmSoft.SME.Trade.Common.Resources.MatRepOperationType, "");
		AddParameter("ShowAltUnit", ArmSoft.SME.Trade.Common.Resources.ShowALTQUNIT, false);
		AddParameter("ShowSumms", ArmSoft.SME.Trade.Common.Resources.ShowNCCSumma, false);
		AddParameter("ShowSummsWithVAT", ArmSoft.SME.Trade.Common.Resources.ShowNCCSummaWithVAT, false);
		AddParameter("ShowCurrSumms", string.Format(ArmSoft.SME.Trade.Common.Resources.ShowCurrSumma, MTCurr), false);
		AddParameter("ShowParties", ArmSoft.SME.Common.Resources.ShowParties, false);
		AddParameter("ShowSalePrice", ArmSoft.SME.Trade.Common.Resources.ShowPrices, false);
		AddParameter("ShowLinks", ArmSoft.SME.Trade.Common.Resources.ActivateLinksForCodeColumns, true);
		AddParameter("DocISN", ArmSoft.SME.Common.Resources.DocumentISN, Guid.Empty);
		AddParameter("AdditionalCaption", ArmSoft.SME.Trade.Common.Resources.AdditionalCaption, string.Empty);
		AddParameter("SubTotalType", ArmSoft.SME.Common.Resources.ShowSubTotals, "");
		AddParameter("ForByPartiesJournal", ArmSoft.SME.Common.Resources.DocumentISN, false);
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
		Columns["CostSumm"].IsSystem = true;
		Columns["CostWithVat"].IsSystem = true;
		Columns["CostSummWithVat"].IsSystem = true;
		Columns["VATSumm"].IsSystem = true;
		Columns["CurrCost"].IsSystem = true;
		Columns["CurrCostSumm"].IsSystem = true;
		Columns["SalePrice"].IsSystem = true;
		Columns["SaleSumm"].IsSystem = true;
		Columns["VATSalePrice"].IsSystem = true;
		Columns["VATSaleSumm"].IsSystem = true;
		Columns["SaleVATSumm"].IsSystem = true;
		Columns["SaleCurPrice"].IsSystem = true;
		Columns["SaleCurSumm"].IsSystem = true;
		Columns["GroupCodeCaption"].IsSystem = true;
		Columns["IncomeDate"].IsSystem = true;
		Columns["PartyPartCaption"].IsSystem = true;
		Columns["PartyPartTaxCode"].IsSystem = true;
		Columns["PartyContractCaption"].IsSystem = true;
		Columns["PartyDocType"].IsSystem = true;
		Columns["PartyDocNum"].IsSystem = true;
		Columns["PartyDocUsableDate"].IsSystem = true;
		Columns["PartyDocCountry"].IsSystem = true;
		Columns["PartyDocProducer"].IsSystem = true;
		Columns["PartyDocPropertyType1"].IsSystem = true;
		Columns["PartyDocPropertyValue1"].IsSystem = true;
		Columns["PartyDocPropertyType2"].IsSystem = true;
		Columns["PartyDocPropertyValue2"].IsSystem = true;
	}

	public override void InitContextFunctions()
	{
		base.InitContextFunctions();
		DataViewPanel currentViewPanel = Settings.HostingEnvironment.CurrentViewPanel;
		GoodsOperationsDataProvider.DataRow dataRow = (GoodsOperationsDataProvider.DataRow)currentViewPanel.FocusedRow();
		if (dataRow != null && AllowOperation)
		{
			DefinitionMappingsService.TryResolveDocumentDefinition(dataRow.DocType, out var definition);
			if (definition.AllowStoreAsDraft || (currentViewPanel.GetSelectedAny() && Settings.ParametersService.DraftSelected()))
			{
				currentViewPanel.AddContextFunction("ContextFunctions", "SetDocDraftState", ArmSoft.SME.Common.Resources.SetDocDraft, SetDocDraftState, "", "", storeOpContext: true);
			}
		}
	}

	private void SetDocDraftState()
	{
		InitContextFunctionsHelper.ChangeDocState((from p in Settings.HostingEnvironment.CurrentViewPanel.GetSelectedOrFocusedRows<GoodsOperationsDataProvider.DataRow>()
			select p.Base).ToList(), DocumentStateSE.Draft);
	}

	protected override void SetCacheRow(ExtendableRow row)
	{
		GoodsOperationsDataProvider.DataRow dataRow = row as GoodsOperationsDataProvider.DataRow;
		dataRow.MaterialCacheRow = TradeSettings.TradeCacheService.Material.GetItem(dataRow.GoodCode);
		dataRow.ProducerCacheRow = TradeSettings.TradeCacheService.Producer.GetItem(dataRow.MaterialCacheRow?.Producer);
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
		ParametersProvider.ShowAltUnit = ShowAltUnit;
		ParametersProvider.ShowSumms = ShowSumms;
		ParametersProvider.ShowSummsWithVAT = ShowSummsWithVAT;
		ParametersProvider.ShowCurrSumms = ShowCurrSumms;
		ParametersProvider.ShowSalePrice = ShowSalePrice;
		ParametersProvider.ShowParties = ShowParties;
		ParametersProvider.DocISN = DocISN;
		ParametersProvider.ForByPartiesJournal = ForByPartiesJournal;
	}

	public override void BeforeLoadData(DataViewPanel dvp)
	{
		base.BeforeLoadData(dvp);
		dvp.RemoveGrouping();
		dvp.Set_ColumnVisible("Cost", ShowSumms);
		dvp.Set_ColumnVisible("CostSumm", ShowSumms);
		dvp.Set_ColumnVisible("CostWithVat", ShowSummsWithVAT);
		dvp.Set_ColumnVisible("CostSummWithVat", ShowSummsWithVAT);
		dvp.Set_ColumnVisible("CurrCost", ShowCurrSumms);
		dvp.Set_ColumnVisible("CurrCostSumm", ShowCurrSumms);
		dvp.Set_ColumnVisible("VATSumm", ShowSumms && ShowSummsWithVAT);
		dvp.Set_ColumnVisible("CorrObjectTaxCode", Settings.ParametersService.ShowPartnerTaxCode());
		dvp.Set_ColumnVisible("SalePrice", ShowSumms && ShowSalePrice);
		dvp.Set_ColumnVisible("VATSalePrice", ShowSalePrice && ShowSummsWithVAT);
		dvp.Set_ColumnVisible("VATSaleSumm", ShowSalePrice && ShowSummsWithVAT);
		dvp.Set_ColumnVisible("SaleSumm", ShowSalePrice && ShowSumms);
		dvp.Set_ColumnVisible("SaleVATSumm", ShowSalePrice && ShowSummsWithVAT);
		dvp.Set_ColumnVisible("SaleCurPrice", ShowSalePrice && ShowCurrSumms);
		dvp.Set_ColumnVisible("SaleCurSumm", ShowSalePrice && ShowCurrSumms);
		dvp.Set_ColumnVisible("GroupCodeCaption", value: false);
		dvp.Set_ColumnVisible("IncomeDate", ShowParties && Settings.ParametersService.ShowPartyIncomeDate());
		dvp.Set_ColumnVisible("PartyPartCaption", ShowParties && Settings.ParametersService.ShowPartySUPPLIER());
		dvp.Set_ColumnVisible("PartyPartTaxCode", ShowParties && Settings.ParametersService.ShowPartnerTaxCode());
		dvp.Set_ColumnVisible("PartyPartCaption", ShowParties && Settings.ParametersService.ShowPartySUPPLIER());
		dvp.Set_ColumnVisible("PartyContractCaption", ShowParties && Settings.ParametersService.ShowPartyCONTRACT());
		dvp.Set_ColumnVisible("PartyDocType", ShowParties && Settings.ParametersService.ShowPartyDocType());
		dvp.Set_ColumnVisible("PartyDocNum", ShowParties && Settings.ParametersService.ShowPartyDocNum());
		dvp.Set_ColumnVisible("PartyDocUsableDate", ShowParties && Settings.ParametersService.ShowPartyUSEABLEDATE());
		dvp.Set_ColumnVisible("PartyDocCountry", ShowParties && Settings.ParametersService.ShowPartyCountry());
		dvp.Set_ColumnVisible("PartyDocProducer", ShowParties && Settings.ParametersService.ShowPartyProducer());
		dvp.Set_ColumnVisible("PartyDocPropertyType1", ShowParties && Settings.ParametersService.ShowPartyProperty1());
		dvp.Set_ColumnVisible("PartyDocPropertyValue1", ShowParties && Settings.ParametersService.ShowPartyProperty1());
		dvp.Set_ColumnVisible("PartyDocPropertyType2", ShowParties && Settings.ParametersService.ShowPartyProperty2());
		dvp.Set_ColumnVisible("PartyDocPropertyValue2", ShowParties && Settings.ParametersService.ShowPartyProperty2());
		if (!dvp.IsSorted)
		{
			dvp.Set_ColumnSortOrder("OpDate");
			dvp.Set_ColumnSortOrder("DocNum");
			dvp.Set_ColumnSortOrder("Storage");
		}
		if (!string.IsNullOrWhiteSpace(SubTotalType))
		{
			dvp.Set_ColumnGroupSummary("Quantity", SummaryItemType.Sum);
			dvp.Set_ColumnGroupSummary("CostSumm", SummaryItemType.Sum);
			dvp.Set_ColumnGroupSummary("CostSummWithVat", SummaryItemType.Sum);
			dvp.Set_ColumnGroupSummary("CurrCostSumm", SummaryItemType.Sum);
			dvp.Set_ColumnGroupSummary("VATSumm", SummaryItemType.Sum);
			dvp.Set_ColumnGroupSummary("VATSaleSumm", SummaryItemType.Sum);
			dvp.Set_ColumnGroupSummary("SaleSumm", SummaryItemType.Sum);
			dvp.Set_ColumnGroupSummary("SaleVATSumm", SummaryItemType.Sum);
			dvp.Set_ColumnGroupSummary("SaleCurSumm", SummaryItemType.Sum);
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
		Columns.AddDirectoryLinkColumn(DirectoryTypeSE.Goods, "GoodCode", "Caption", "", ArmSoft.SME.Common.Resources.Product, ShowLinks);
		Columns.AddDirectoryLinkColumn(DirectoryTypeSE.Partner, "CorrObjectCode", "CorrObjectCaption", "", "", ShowLinks);
	}
}
