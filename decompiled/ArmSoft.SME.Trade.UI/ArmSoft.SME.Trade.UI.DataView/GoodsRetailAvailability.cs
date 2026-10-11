using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using ArmSoft.SME.Client.Data;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.CustomTypes;
using ArmSoft.SME.Common.Extensions;
using ArmSoft.SME.Common.Parameters;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Core.UI.Controls;
using ArmSoft.SME.Core.UI.Converters;
using ArmSoft.SME.Core.UI.DataViews;
using ArmSoft.SME.Core.UI.Documents;
using ArmSoft.SME.Core.UI.DropDowns;
using ArmSoft.SME.Trade.Client.Data.Directories;
using ArmSoft.SME.Trade.Client.Data.Reports;
using ArmSoft.SME.Trade.Common;
using ArmSoft.SME.Trade.Common.Models.Caches;
using ArmSoft.SME.Trade.Common.Models.Documents;
using ArmSoft.SME.Trade.Common.Models.PrintableElements;
using ArmSoft.SME.Trade.Common.Parameters;
using ArmSoft.SME.Trade.Common.SmartEnums;
using ArmSoft.SME.Trade.UI.Cache;
using ArmSoft.SME.Trade.UI.CustomUIElements;
using ArmSoft.SME.Trade.UI.DropDown;
using ArmSoft.SME.Trade.UI.Helpers;
using ArmSoft.SME.Trade.UI.ReportViewers.Goods;
using DevExpress.Data;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Editors;
using DevExpress.Xpf.LayoutControl;

namespace ArmSoft.SME.Trade.UI.DataView;

[DataView(10045)]
public class GoodsRetailAvailability : TradeDataView<GoodsRetailAvailabilityDataProvider.DataRow, GoodsRetailAvailabilityDataProvider.Param>
{
	private class DataViewDialog : TradeDataViewParametersDialogWindow
	{
		private readonly GoodsRetailAvailability _dataView;

		private DateEditControl _asOfDate;

		private DropDownControl _subTotalType;

		private DropDownControl _goodGroup;

		private DropDownControl _good;

		private MultiSelectDropDownControl _storage;

		private CheckEditExt _showAltUnit;

		private CheckEditExt _detailByStorages;

		private readonly GoodsFilter _goods;

		public DataViewDialog(GoodsRetailAvailability dataView, bool useParamValues, bool isRefreshMode)
			: base(dataView)
		{
			_dataView = dataView;
			_goods = new GoodsFilter(shortMode: true, ItemTypesSE.Goods, "", isShowOnlyInPriceList: true);
			Caption = ArmSoft.SME.Trade.Common.Resources.MTRepRetailAvail;
			LayoutGroupExt container = AddHorizontalLayoutGroup();
			if (!isRefreshMode)
			{
				AddStoreValuesButton(container);
			}
			_asOfDate = AddDateControl(container, "RemDate", ArmSoft.SME.Common.Resources.DateWord, useLongDate: false, isRequired: true, !useParamValues);
			_asOfDate.EditValue = (useParamValues ? dataView.AsOfDate : Settings.HostingEnvironment.CurrentDate);
			AddSpecialDateButton(container, _asOfDate);
			DXTabControl tc = AddTabControl(null);
			LayoutGroup layoutGroup = AddTabedGroup(tc, ArmSoft.SME.Common.Resources.General);
			_storage = AddMultiSelectDropDownControl(layoutGroup, "Storage", ArmSoft.SME.Common.Resources.Storage, new ArmSoft.SME.Trade.UI.DropDown.Storages(true, true), isRequired: false, !useParamValues);
			_storage.SelectedString = (useParamValues ? dataView.Storage : string.Empty);
			_goodGroup = AddDropDownControl(layoutGroup, "GoodGroup", ArmSoft.SME.Common.Resources.Group, new GoodsGroups(), isRequired: false, allowAdditionButtons: false, !useParamValues);
			_goodGroup.Code = (useParamValues ? dataView.GoodGroup : string.Empty);
			_goodGroup.EditValueChanged += new SafeEventHandler<EditValueChangedEventArgs>(GoodsGroup_EditValueChanged).Handler;
			_good = AddDropDownControl(layoutGroup, "GoodCode", ArmSoft.SME.Trade.Common.Resources.MaterialValue, _goods, isRequired: false, allowAdditionButtons: false, !useParamValues);
			_good.Code = (useParamValues ? dataView.Good : string.Empty);
			AddLine(layoutGroup);
			_showAltUnit = AddBooleanControl(layoutGroup, "ShowAltUnit", ArmSoft.SME.Trade.Common.Resources.ShowALTQUNIT, !useParamValues);
			_showAltUnit.IsChecked = useParamValues && dataView.ShowAltUnit;
			AddLine(layoutGroup);
			_detailByStorages = AddBooleanControl(layoutGroup, "DetailByStorage", ArmSoft.SME.Trade.Common.Resources.DetailByStorage, !useParamValues);
			_detailByStorages.IsChecked = useParamValues && dataView.DetailByStorage;
			AddLine(layoutGroup);
			_subTotalType = AddDropDownControl(layoutGroup, "SubTotalType", ArmSoft.SME.Common.Resources.ShowSubTotals, new SmartEnum(typeof(SubTotalTypesSE)), isRequired: false, allowAdditionButtons: false, !useParamValues);
			_subTotalType.Code = (useParamValues ? dataView.SubTotalType : string.Empty);
			_subTotalType.EditValueChanged += SubTotalType_EditValueChanged;
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

		protected override void DialogValidation(DialogWindowValidationArgs args)
		{
			try
			{
				args.isValid = TradeDataViewParametersDialogWindow.CheckPeriodForActualness(_asOfDate.EditValue.Value);
				base.DialogValidation(args);
			}
			catch (Exception ex)
			{
				Settings.ErrorHandler(ex);
				args.isValid = false;
			}
		}

		public override void ApplyParams(bool useParamValues, bool isRefreshMode)
		{
			_dataView.AsOfDate = _asOfDate.EditValue.Value;
			_dataView.Storage = _storage.SelectedString;
			_dataView.GoodGroup = _goodGroup.Code;
			_dataView.Good = _good.Code;
			_dataView.ShowAltUnit = _showAltUnit.IsChecked.Value;
			_dataView.DetailByStorage = _detailByStorages.IsChecked.Value;
			_dataView.SubTotalType = _subTotalType.Code;
		}

		private void SubTotalType_EditValueChanged(object sender, EditValueChangedEventArgs e)
		{
			if (!string.IsNullOrWhiteSpace(_subTotalType.Code) && _subTotalType.DoValidate() && _subTotalType.Code == "1")
			{
				_detailByStorages.IsChecked = true;
				_detailByStorages.IsReadOnly = true;
			}
			else
			{
				_detailByStorages.IsReadOnly = false;
			}
		}
	}

	private List<ViewColumn> potentiallyColumns;

	public override string Name => "GoodsRetailAvailability";

	public override LoadIndication LoadingIndication => LoadIndication.Always;

	public override string HelpId => "Sales_Reports_RemainsRetail";

	public override bool DialogSupported => true;

	public override string Caption => ArmSoft.SME.Trade.Common.Resources.MTRepRetailAvail + " - " + AsOfDate.FormatDDMMYY();

	protected override bool AllowFindRowByMTBarcode => true;

	protected override string MTCodeColumnName => "GoodCode";

	public override List<ViewColumn> PotentiallyColumns => potentiallyColumns;

	public DateOnly AsOfDate
	{
		get
		{
			return (DateOnly)Parameters["RemDate"].Value;
		}
		set
		{
			Parameters["RemDate"].Value = value;
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

	public string Good
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

	public bool DetailByStorage
	{
		get
		{
			return Convert.ToBoolean(Parameters["DetailByStorage"].Value);
		}
		set
		{
			Parameters["DetailByStorage"].Value = value;
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

	public GoodsRetailAvailability()
	{
		DataProvider = new GoodsRetailAvailabilityDataProvider(TradeSettings.CreateTradeClient());
		AddParameter("RemDate", ArmSoft.SME.Common.Resources.DateWord, Settings.HostingEnvironment.CurrentDate, "dd/MM/yy");
		AddParameter("GoodCode", ArmSoft.SME.Trade.Common.Resources.MaterialValue, "");
		AddParameter("Storage", ArmSoft.SME.Common.Resources.Storage, "");
		AddParameter("GoodGroup", ArmSoft.SME.Common.Resources.Group, "");
		AddParameter("ShowAltUnit", ArmSoft.SME.Trade.Common.Resources.ShowALTQUNIT, false);
		AddParameter("DetailByStorage", ArmSoft.SME.Trade.Common.Resources.DetailByStorage, false);
		AddParameter("SubTotalType", ArmSoft.SME.Common.Resources.ShowSubTotals, "");
		FillColumnsFromDataProvider();
		Columns["ImageExists"].Converter = new BooleanToImageExists();
		potentiallyColumns = new List<ViewColumn>();
		potentiallyColumns.AddRange(Columns.ViewList);
		potentiallyColumns.AddRange(MaterialCache.GetDataViewColumns(ArmSoft.SME.Trade.Common.Resources.MaterialValue, "", ""));
		potentiallyColumns.AddRange(ProducerCache.GetDataViewColumns(ArmSoft.SME.Common.Resources.Producer, "", ""));
	}

	protected override void SetColumnsProperties()
	{
		base.SetColumnsProperties();
		Columns["Storage"].IsSystem = true;
		Columns["StorageCaption"].IsSystem = true;
		Columns["GroupCodeCaption"].IsSystem = true;
	}

	public override string GetDefaultFunction()
	{
		return "ShowMTTAccount";
	}

	public override void BeforeLoadData(DataViewPanel dvp)
	{
		base.BeforeLoadData(dvp);
		dvp.RemoveGrouping();
		dvp.Set_ColumnVisible("Storage", DetailByStorage);
		dvp.Set_ColumnVisible("StorageCaption", DetailByStorage);
		dvp.Set_ColumnVisible("GroupCodeCaption", value: false);
		dvp.Set_ColumnVisible("UnitBrief", !ShowAltUnit);
		if (!dvp.IsSorted)
		{
			dvp.Set_ColumnSortOrder("GoodCode");
			dvp.Set_ColumnSortOrder("Storage");
		}
		if (!string.IsNullOrWhiteSpace(SubTotalType))
		{
			dvp.Set_ColumnGroupSummary("Quantity", SummaryItemType.Sum);
			dvp.Set_ColumnGroupSummary("Price", SummaryItemType.Sum);
			dvp.Set_ColumnGroupSummary("Sum", SummaryItemType.Sum);
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
		if (Settings.HostingEnvironment.CurrentViewPanel.FocusedRow() != null)
		{
			if (Settings.UserAccessService.GetProviderPermission(DataProviderTypeSE.MatRepTAccount).AllowView)
			{
				Settings.HostingEnvironment.CurrentViewPanel.AddContextFunction("ContextFunctions", "ShowMTTAccount", ArmSoft.SME.Trade.Common.Resources.TAccount, ShowMTTAccount, "", "", storeOpContext: true);
			}
			Settings.HostingEnvironment.CurrentViewPanel.AddContextFunction("ContextFunctions", "CreateDoc", ArmSoft.SME.Common.Resources.CreateDoc, CreateDoc, "doc_new_16.svg");
			Settings.HostingEnvironment.CurrentViewPanel.AddContextFunction("ContextFunctions", "PrintLabel", ArmSoft.SME.Trade.Common.Resources.PrintLabel, PrintLabel);
		}
	}

	protected override void SetCacheRow(ExtendableRow row)
	{
		GoodsRetailAvailabilityDataProvider.DataRow dataRow = row as GoodsRetailAvailabilityDataProvider.DataRow;
		dataRow.MaterialCacheRow = TradeSettings.TradeCacheService.Material.GetItem(dataRow.GoodCode);
		dataRow.ProducerCacheRow = TradeSettings.TradeCacheService.Producer.GetItem(dataRow.MaterialCacheRow?.Producer);
	}

	private void ShowMTTAccount()
	{
		List<GoodsRetailAvailabilityDataProvider.DataRow> selectedOrFocusedRows = Settings.HostingEnvironment.CurrentViewPanel.GetSelectedOrFocusedRows<GoodsRetailAvailabilityDataProvider.DataRow>();
		MTAccountReportViewer mTAccountReportViewer;
		if (selectedOrFocusedRows.Count == 1)
		{
			mTAccountReportViewer = new MTAccountReportViewer();
			mTAccountReportViewer.GoodCode = selectedOrFocusedRows[0].GoodCode;
			if (DetailByStorage)
			{
				mTAccountReportViewer.Storage = selectedOrFocusedRows[0].Storage;
			}
		}
		else
		{
			List<ListOfSelectedMaterialStorage> list = new List<ListOfSelectedMaterialStorage>();
			foreach (GoodsRetailAvailabilityDataProvider.DataRow item in selectedOrFocusedRows)
			{
				ListOfSelectedMaterialStorage listOfSelectedMaterialStorage = new ListOfSelectedMaterialStorage
				{
					GoodCode = item.GoodCode
				};
				if (DetailByStorage)
				{
					listOfSelectedMaterialStorage.Storage = item.Storage;
				}
				list.Add(listOfSelectedMaterialStorage);
			}
			mTAccountReportViewer = new MTAccountReportViewer(isForSelectedRows: true, list);
		}
		mTAccountReportViewer.ShowFromTurns = true;
		mTAccountReportViewer.ShowAltUnit = ShowAltUnit;
		mTAccountReportViewer.StartDate = Settings.HostingEnvironment.DateBegin;
		mTAccountReportViewer.EndDate = Settings.HostingEnvironment.DateEnd;
		mTAccountReportViewer.Show(useParamValues: true);
	}

	private void PrintLabel()
	{
		List<ArmSoft.SME.Trade.Common.Models.PrintableElements.Label.GoodLabelRow> list = new List<ArmSoft.SME.Trade.Common.Models.PrintableElements.Label.GoodLabelRow>();
		ArmSoft.SME.Trade.Common.Models.PrintableElements.Label.GoodLabelRow goodLabelRow = new ArmSoft.SME.Trade.Common.Models.PrintableElements.Label.GoodLabelRow();
		foreach (GoodsRetailAvailabilityDataProvider.DataRow selectedOrFocusedRow in Settings.HostingEnvironment.CurrentViewPanel.GetSelectedOrFocusedRows<GoodsRetailAvailabilityDataProvider.DataRow>())
		{
			MaterialCacheRow item = TradeSettings.TradeCacheService.Material.GetItem(selectedOrFocusedRow.GoodCode);
			if (item != null && item.Type == GoodsTypesSE.Good)
			{
				goodLabelRow = InitContextFunctionsHelper.BuildLabelGoodRow(item);
				goodLabelRow.Quantity = selectedOrFocusedRow.Quantity;
				list.Add(goodLabelRow);
			}
		}
		new PrintLabelDialog(null, null, list, isFromGoodsAvailability: true).Invoke();
	}

	private void CreateDoc()
	{
		if (!DetailByStorage)
		{
			throw new ASException(ArmSoft.SME.Trade.Common.Resources.ValidCreateDocFromGoodsAvail);
		}
		int num = 9;
		List<short> list = new List<short>(num);
		CollectionsMarshal.SetCount(list, num);
		Span<short> span = CollectionsMarshal.AsSpan(list);
		span[0] = DocumentTypeSE.MTSaleRetail.Value;
		span[1] = DocumentTypeSE.MTBill.Value;
		span[2] = DocumentTypeSE.MTMove.Value;
		span[3] = DocumentTypeSE.MTMoveWithConfirmation.Value;
		span[4] = DocumentTypeSE.MTStockTakingRetail.Value;
		span[5] = DocumentTypeSE.MTIncomeRetail.Value;
		span[6] = DocumentTypeSE.MTExpense.Value;
		span[7] = DocumentTypeSE.MTRevaluationRetail.Value;
		span[8] = DocumentTypeSE.MTReturnToSupplier.Value;
		List<short> list2 = list;
		if (!Settings.ParametersService.MTMoveWithConfirmation())
		{
			list2.Remove(DocumentTypeSE.MTMoveWithConfirmation.Value);
		}
		DocumentTypeSE documentTypeSE = InitContextFunctionsHelper.ChooseDocTypeFromMenu(list2);
		if (!(documentTypeSE == null))
		{
			bool fillAvailQty = InitContextFunctionsHelper.FillAllAvailabilityAvailabilityOrTurnOver(documentTypeSE);
			if ((short)documentTypeSE == DocumentTypeSE.MTSaleRetail.Value)
			{
				CreateNewMTInvoiceRetailDoc(fillAvailQty);
			}
			else if ((short)documentTypeSE == DocumentTypeSE.MTBill.Value)
			{
				CreateNewMTBillDoc(fillAvailQty);
			}
			else if ((short)documentTypeSE == DocumentTypeSE.MTMove.Value)
			{
				CreateNewMTMoveDoc(fillAvailQty);
			}
			else if ((short)documentTypeSE == DocumentTypeSE.MTMoveWithConfirmation.Value)
			{
				CreateNewMTMoveWithConfirmationDoc(fillAvailQty);
			}
			else if ((short)documentTypeSE == DocumentTypeSE.MTStockTakingRetail.Value)
			{
				CreateNewMTStockTakingRetailDoc(fillAvailQty);
			}
			else if ((short)documentTypeSE == DocumentTypeSE.MTIncomeRetail.Value)
			{
				InitContextFunctionsHelper.CreateNewMTIncomeRetailDoc(AsOfDate);
			}
			else if ((short)documentTypeSE == DocumentTypeSE.MTExpense.Value)
			{
				CreateNewMTExpenseDoc(fillAvailQty);
			}
			else if ((short)documentTypeSE == DocumentTypeSE.MTRevaluationRetail.Value)
			{
				CreateNewMTRevaluationRetailDoc(fillAvailQty);
			}
			else if ((short)documentTypeSE == DocumentTypeSE.MTReturnToSupplier.Value)
			{
				CreateNewMTReturnToSupplierDoc(fillAvailQty);
			}
		}
	}

	private void CreateNewMTInvoiceRetailDoc(bool FillAvailQty)
	{
		MTInvoiceRetail doc = Settings.DocumentService.Create<MTInvoiceRetail>(DocumentTypeSE.MTSaleRetail.Value);
		int num = 0;
		MTInvoiceBase.MTInvoiceSpecificationRow mTInvoiceSpecificationRow = null;
		try
		{
			doc.DocDate = AsOfDate;
			doc.SalesConsultant = Settings.ParametersService.DefaultSalesConsultant();
			doc.SupplierBusinessAddress = Settings.ParametersService.CompanyBusinessAddress();
			string text = Settings.ParametersService.DefaultCustomer();
			if (!string.IsNullOrWhiteSpace(text))
			{
				PartnerCacheRow item = TradeSettings.TradeCacheService.Partner.GetItem(text);
				if (item != null && item.Customer)
				{
					doc.PartnerContractGroup.PartnerCode = text;
					PartnerCacheRow.ContractRow defaultContract = TradeSettings.TradeCacheService.Partner.GetDefaultContract(item);
					if (defaultContract != null)
					{
						doc.PartnerContractGroup.ContractCode = defaultContract.Contract;
						doc.Discount = defaultContract.Percent;
					}
				}
			}
			string text2 = "";
			foreach (GoodsRetailAvailabilityDataProvider.DataRow selectedOrFocusedRow in Settings.HostingEnvironment.CurrentViewPanel.GetSelectedOrFocusedRows<GoodsRetailAvailabilityDataProvider.DataRow>())
			{
				if (!string.IsNullOrWhiteSpace(text2) && text2 != selectedOrFocusedRow.Storage)
				{
					throw new ASException(ArmSoft.SME.Trade.Common.Resources.SaleRetailForSameStorage);
				}
				text2 = selectedOrFocusedRow.Storage;
				mTInvoiceSpecificationRow = GetCurrentMTListRow(doc, selectedOrFocusedRow, FillAvailQty, num);
				if (mTInvoiceSpecificationRow != null)
				{
					doc.MTList.Add(mTInvoiceSpecificationRow);
				}
				num++;
			}
			doc.Storage = text2;
			StoragesCacheRow item2 = TradeSettings.TradeCacheService.Storage.GetItem(doc.Storage);
			if (!string.IsNullOrWhiteSpace(item2.Address))
			{
				doc.SupplierBusinessAddress = item2.Address;
			}
			doc.MaxRowID = num;
			doc.SummVAT = doc.MTList.Sum((MTInvoiceBase.MTInvoiceSpecificationRow p) => PriceHelper.CalcVATAmountByType(p.Summa, doc.VATType, Convert.ToBoolean(p.VAT.HasValue && p.VAT.Value))).RoundAs(2);
			doc.Summ = doc.MTList.Sum((MTInvoiceBase.MTInvoiceSpecificationRow p) => p.Summa).RoundAs(2);
			doc.SupplierName = Settings.ParametersService.CompanyName();
			doc.SupplierLegalAddress = Settings.ParametersService.CompanyAddress();
			doc.SupplierSettlAcc = TradeSettings.TradeCacheService.AccountInBank.GetDefaultSettlementAccount(isSettlAccount: true);
			doc.SupplierTaxCode = Settings.ParametersService.TaxCode();
			doc.SupplierMngPost = Settings.ParametersService.ManagerPost();
			doc.SupplierMngName = Settings.ParametersService.ManagerName();
			doc.SupplierChiefAccPost = Settings.ParametersService.ChiefAccountantPost();
			doc.SupplierChiefAccName = Settings.ParametersService.ChiefAccountantName();
			Settings.DocumentService.CreateWindow(doc).Show();
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	private MTInvoiceBase.MTInvoiceSpecificationRow GetCurrentMTListRow(MTInvoiceBase doc, GoodsRetailAvailabilityDataProvider.DataRow sourceRow, bool fillAvailQty, int rowId, string priceListType = "", string discountPriceListType = "", string discountPriceTypeRoundDegree = "", string discountPriceListVATType = "", string discountPriceListCurr = "", decimal discountPriceListCurrRate = 1m, decimal discountPriceListCurrBase = 1m)
	{
		MTInvoiceBase.MTInvoiceSpecificationRow mTInvoiceSpecificationRow = null;
		MaterialCacheRow itemById = TradeSettings.TradeCacheService.Material.GetItemById(sourceRow.GoodID);
		if (itemById != null)
		{
			mTInvoiceSpecificationRow = new MTInvoiceBase.MTInvoiceSpecificationRow();
			mTInvoiceSpecificationRow.RowID = rowId;
			mTInvoiceSpecificationRow.ItemType = GoodsTypesSE.Good.Value;
			mTInvoiceSpecificationRow.Storage = sourceRow.Storage;
			mTInvoiceSpecificationRow.ItemID = sourceRow.GoodID;
			mTInvoiceSpecificationRow.ItemCode = itemById.MTCode;
			mTInvoiceSpecificationRow.ItemName = itemById.Caption;
			mTInvoiceSpecificationRow.UnitCode = sourceRow.Unit;
			mTInvoiceSpecificationRow.UnitBrief = sourceRow.UnitBrief;
			MaterialCacheRow.BarCodeRow barCode = TradeSettings.TradeCacheService.Material.GetBarCode(mTInvoiceSpecificationRow.ItemID, mTInvoiceSpecificationRow.UnitCode);
			mTInvoiceSpecificationRow.BarCode = ((barCode == null) ? "" : barCode.BarCode);
			mTInvoiceSpecificationRow.Quantity = (fillAvailQty ? sourceRow.Quantity : 1m);
			mTInvoiceSpecificationRow.Price = sourceRow.Price;
			if (Settings.ParametersService.DiscountUse())
			{
				mTInvoiceSpecificationRow.Discount = doc.Discount;
				if (string.IsNullOrWhiteSpace(discountPriceListType) && (mTInvoiceSpecificationRow.Discount == 0m || itemById.Discount != 0m))
				{
					mTInvoiceSpecificationRow.Discount = itemById.Discount;
				}
			}
			if (!string.IsNullOrWhiteSpace(discountPriceListType))
			{
				PricesDataProvider.DataRow price = TradeSettings.CreateTradeClient().CommonServices.GetPrice(discountPriceListType, mTInvoiceSpecificationRow.ItemID, mTInvoiceSpecificationRow.UnitCode, AsOfDate);
				if (price.Price != 0m)
				{
					if (discountPriceListCurr == "AMD")
					{
						mTInvoiceSpecificationRow.DiscountPrice = price.Price;
					}
					else
					{
						mTInvoiceSpecificationRow.DiscountPrice = (price.Price * discountPriceListCurrRate / discountPriceListCurrBase).RoundAs(2);
					}
					if (discountPriceListVATType == "1" && mTInvoiceSpecificationRow.VAT.HasValue && mTInvoiceSpecificationRow.VAT.Value)
					{
						mTInvoiceSpecificationRow.DiscountPrice += PriceHelper.CalcVATAmountByType(mTInvoiceSpecificationRow.DiscountPrice, "1", Convert.ToBoolean(mTInvoiceSpecificationRow.VAT.HasValue && mTInvoiceSpecificationRow.VAT.Value));
					}
					mTInvoiceSpecificationRow.DiscountPrice = Methods.RoundPrice(mTInvoiceSpecificationRow.DiscountPrice, discountPriceTypeRoundDegree);
					if (mTInvoiceSpecificationRow.Price != 0m)
					{
						mTInvoiceSpecificationRow.Discount = (100m * (mTInvoiceSpecificationRow.Price - mTInvoiceSpecificationRow.DiscountPrice) / mTInvoiceSpecificationRow.Price).RoundAs(2);
					}
				}
				else if (doc.Discount != 0m)
				{
					mTInvoiceSpecificationRow.Discount = doc.Discount;
					mTInvoiceSpecificationRow.DiscountPrice = Methods.RoundPrice(mTInvoiceSpecificationRow.Price * (100m - mTInvoiceSpecificationRow.Discount) / 100m, discountPriceTypeRoundDegree);
				}
				else
				{
					mTInvoiceSpecificationRow.DiscountPrice = mTInvoiceSpecificationRow.Price;
					mTInvoiceSpecificationRow.Discount = 0m;
				}
			}
			else if (!string.IsNullOrWhiteSpace(priceListType))
			{
				if (Settings.ParametersService.DiscountUse())
				{
					string roundDegree = "";
					if (string.IsNullOrWhiteSpace(priceListType))
					{
						roundDegree = TradeSettings.CreateTradeClient().CommonServices.GetPriceTypeRoungDgr(priceListType);
					}
					mTInvoiceSpecificationRow.DiscountPrice = TradeSettings.CreateTradeClient().CommonServices.GetPriceForGivenPriceListAndVATType(priceListType, "2", mTInvoiceSpecificationRow.ItemID, mTInvoiceSpecificationRow.UnitCode, Settings.HostingEnvironment.CurrentDate, "AMD");
					mTInvoiceSpecificationRow.DiscountPrice = Methods.RoundPrice(mTInvoiceSpecificationRow.DiscountPrice, roundDegree);
					if (mTInvoiceSpecificationRow.Price == 0m)
					{
						if (mTInvoiceSpecificationRow.Discount != 0m)
						{
							mTInvoiceSpecificationRow.Price = (mTInvoiceSpecificationRow.DiscountPrice * 100m / mTInvoiceSpecificationRow.Discount).RoundAs(2);
						}
						else
						{
							mTInvoiceSpecificationRow.Price = mTInvoiceSpecificationRow.DiscountPrice;
						}
					}
					else
					{
						mTInvoiceSpecificationRow.Discount = (100m * (mTInvoiceSpecificationRow.Price - mTInvoiceSpecificationRow.DiscountPrice) / mTInvoiceSpecificationRow.Price).RoundAs(2);
					}
				}
			}
			else
			{
				mTInvoiceSpecificationRow.DiscountPrice = (mTInvoiceSpecificationRow.Price * (1m - Convert.ToDecimal(mTInvoiceSpecificationRow.Discount) / 100m)).RoundAs(2);
			}
			mTInvoiceSpecificationRow.Summa = (mTInvoiceSpecificationRow.Quantity * mTInvoiceSpecificationRow.DiscountPrice).RoundAs(2);
			if (Settings.ParametersService.DiscountUse())
			{
				mTInvoiceSpecificationRow.DiscountSumma = (mTInvoiceSpecificationRow.Quantity * mTInvoiceSpecificationRow.Price - mTInvoiceSpecificationRow.Summa).RoundAs(2);
			}
			mTInvoiceSpecificationRow.VAT = itemById.VAT;
			mTInvoiceSpecificationRow.IsPartySelected = false;
		}
		return mTInvoiceSpecificationRow;
	}

	private void CreateNewMTBillDoc(bool fillAvailQty)
	{
		MTBill doc = Settings.DocumentService.Create<MTBill>(DocumentTypeSE.MTBill.Value);
		int num = 0;
		MTInvoiceBase.MTInvoiceSpecificationRow mTInvoiceSpecificationRow = null;
		string priceListType = Settings.ParametersService.DefaultMTCheckPriceType();
		string text = Settings.ParametersService.DefaultMTCheckDiscountPriceType();
		string discountPriceTypeRoundDegree = string.Empty;
		string priceListRowVATType = string.Empty;
		string priceListRowCurr = string.Empty;
		decimal discountPriceListCurrRate = 1m;
		decimal discountPriceListCurrBase = 1m;
		try
		{
			doc.DocDate = AsOfDate;
			doc.SalesConsultant = Settings.ParametersService.DefaultSalesConsultant();
			doc.CashDesk = Settings.ParametersService.DefaultCash();
			string text2 = Settings.ParametersService.DefaultCustomer();
			if (!string.IsNullOrWhiteSpace(text2))
			{
				PartnerCacheRow item = TradeSettings.TradeCacheService.Partner.GetItem(text2);
				if (item != null && item.Customer)
				{
					doc.PartnerContractGroup.PartnerCode = text2;
					PartnerCacheRow.ContractRow defaultContract = TradeSettings.TradeCacheService.Partner.GetDefaultContract(item);
					if (defaultContract != null)
					{
						doc.PartnerContractGroup.ContractCode = defaultContract.Contract;
						doc.Discount = defaultContract.Percent;
					}
				}
			}
			if (Settings.ParametersService.DiscountUse() && !string.IsNullOrWhiteSpace(text))
			{
				discountPriceTypeRoundDegree = TradeSettings.CreateTradeClient().CommonServices.GetPriceTypeRoungDgr(text);
				PriceHelper.GetPriceListRowVATType(text, out priceListRowVATType, out priceListRowCurr);
				discountPriceListCurrRate = 1m;
				discountPriceListCurrBase = 1m;
				if (priceListRowCurr != doc.CurrCode && priceListRowCurr != "AMD")
				{
					ExchangeRate exchangeRates = TradeSettings.CreateTradeClient().CommonServices.GetExchangeRates(priceListRowCurr, doc.DocDate);
					discountPriceListCurrRate = exchangeRates.Rate;
					discountPriceListCurrBase = exchangeRates.Base;
				}
			}
			string text3 = "";
			foreach (GoodsRetailAvailabilityDataProvider.DataRow selectedOrFocusedRow in Settings.HostingEnvironment.CurrentViewPanel.GetSelectedOrFocusedRows<GoodsRetailAvailabilityDataProvider.DataRow>())
			{
				if (!string.IsNullOrWhiteSpace(text3) && text3 != selectedOrFocusedRow.Storage)
				{
					throw new ASException(ArmSoft.SME.Trade.Common.Resources.SaleRetailForSameStorage);
				}
				text3 = selectedOrFocusedRow.Storage;
				mTInvoiceSpecificationRow = GetCurrentMTListRow(doc, selectedOrFocusedRow, fillAvailQty, num, priceListType, text, discountPriceTypeRoundDegree, priceListRowVATType, priceListRowCurr, discountPriceListCurrRate, discountPriceListCurrBase);
				if (mTInvoiceSpecificationRow != null)
				{
					doc.MTList.Add(mTInvoiceSpecificationRow);
					num++;
				}
			}
			doc.Storage = text3;
			doc.MaxRowID = num;
			doc.SummVAT = doc.MTList.Sum((MTInvoiceBase.MTInvoiceSpecificationRow p) => PriceHelper.CalcVATAmountByType(p.Summa, doc.VATType, Convert.ToBoolean(p.VAT.HasValue && p.VAT.Value))).RoundAs(2);
			doc.Summ = doc.MTList.Sum((MTInvoiceBase.MTInvoiceSpecificationRow p) => p.Summa).RoundAs(2);
			doc.SummaChange = doc.Summ;
			Settings.DocumentService.CreateWindow(doc).Show();
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	private void CreateNewMTMoveDoc(bool FillAvailQty)
	{
		MTMove mTMove = Settings.DocumentService.Create<MTMove>(DocumentTypeSE.MTMove.Value);
		int rowId = 0;
		try
		{
			List<GoodsRetailAvailabilityDataProvider.DataRow> selectedOrFocusedRows = Settings.HostingEnvironment.CurrentViewPanel.GetSelectedOrFocusedRows<GoodsRetailAvailabilityDataProvider.DataRow>();
			if (selectedOrFocusedRows.Select((GoodsRetailAvailabilityDataProvider.DataRow p) => p.Storage).Distinct().Count() > 1)
			{
				throw new ASException(ArmSoft.SME.Trade.Common.Resources.RowsMustHaveSameStorage);
			}
			string storage = selectedOrFocusedRows[0].Storage;
			mTMove.DocDate = AsOfDate;
			if (!string.IsNullOrWhiteSpace(storage))
			{
				StoragesCacheRow item = TradeSettings.TradeCacheService.Storage.GetItem(storage);
				if (item != null && item.Retail && TradeSettings.CreateTradeClient().CommonServices.IsStorageAvailable(storage))
				{
					mTMove.StorageExpense = storage;
				}
			}
			foreach (GoodsRetailAvailabilityDataProvider.DataRow item2 in selectedOrFocusedRows)
			{
				if (item2.Quantity >= 0m)
				{
					AddRowToMTMoveList(mTMove, item2, FillAvailQty, ref rowId);
					continue;
				}
				throw new ASException(string.Format(ArmSoft.SME.Trade.Common.Resources.MTHasNegativeQty, item2.GoodCode));
			}
			mTMove.MaxRowID = rowId;
			mTMove.Summ = mTMove.MTList.Sum((MTMoveBase.MTMoveSpecificationRow p) => Convert.ToDecimal(p.Summa)).RoundAs(2);
			mTMove.Chief = Settings.ParametersService.ChiefAccountantName();
			Settings.DocumentService.CreateWindow(mTMove).Show();
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	private void CreateNewMTMoveWithConfirmationDoc(bool fillAvailQty)
	{
		MTMoveWithConfirmation mTMoveWithConfirmation = Settings.DocumentService.Create<MTMoveWithConfirmation>(DocumentTypeSE.MTMoveWithConfirmation.Value);
		int rowId = 0;
		try
		{
			List<GoodsRetailAvailabilityDataProvider.DataRow> selectedOrFocusedRows = Settings.HostingEnvironment.CurrentViewPanel.GetSelectedOrFocusedRows<GoodsRetailAvailabilityDataProvider.DataRow>();
			if (selectedOrFocusedRows.Select((GoodsRetailAvailabilityDataProvider.DataRow p) => p.Storage).Distinct().Count() > 1)
			{
				throw new ASException(ArmSoft.SME.Trade.Common.Resources.RowsMustHaveSameStorage);
			}
			string storage = selectedOrFocusedRows[0].Storage;
			mTMoveWithConfirmation.DocDate = AsOfDate;
			if (!string.IsNullOrWhiteSpace(storage))
			{
				StoragesCacheRow item = TradeSettings.TradeCacheService.Storage.GetItem(storage);
				if (item != null && item.Retail && TradeSettings.CreateTradeClient().CommonServices.IsStorageAvailable(storage))
				{
					mTMoveWithConfirmation.StorageExpense = storage;
				}
			}
			foreach (GoodsRetailAvailabilityDataProvider.DataRow item2 in selectedOrFocusedRows)
			{
				if (item2.Quantity >= 0m)
				{
					AddRowToMTMoveList(mTMoveWithConfirmation, item2, fillAvailQty, ref rowId);
					continue;
				}
				throw new ASException(string.Format(ArmSoft.SME.Trade.Common.Resources.MTHasNegativeQty, item2.GoodCode));
			}
			mTMoveWithConfirmation.MaxRowID = rowId;
			mTMoveWithConfirmation.Summ = mTMoveWithConfirmation.MTList.Sum((MTMoveBase.MTMoveSpecificationRow p) => Convert.ToDecimal(p.Summa)).RoundAs(2);
			mTMoveWithConfirmation.Chief = Settings.ParametersService.ChiefAccountantName();
			Settings.DocumentService.CreateWindow(mTMoveWithConfirmation).Show();
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	private void AddRowToMTMoveList(MTMoveWithConfirmationBase doc, GoodsRetailAvailabilityDataProvider.DataRow sourceRow, bool fillAvailQty, ref int rowId)
	{
		MaterialCacheRow itemById = TradeSettings.TradeCacheService.Material.GetItemById(sourceRow.GoodID);
		if (itemById != null)
		{
			MTMoveBase.MTMoveSpecificationRow mTMoveSpecificationRow = new MTMoveBase.MTMoveSpecificationRow
			{
				RowID = rowId,
				MTID = sourceRow.GoodID,
				MTCode = itemById.MTCode,
				MTName = itemById.Caption,
				IsPartySelected = false,
				Quantity = ((!fillAvailQty) ? ((sourceRow.Quantity > 0m) ? 1m : 0m) : ((sourceRow.Quantity > 0m) ? sourceRow.Quantity : 0m)),
				UnitCode = sourceRow.Unit,
				UnitBrief = sourceRow.UnitBrief,
				Price = sourceRow.Price
			};
			if (mTMoveSpecificationRow.Price.HasValue)
			{
				mTMoveSpecificationRow.Summa = (mTMoveSpecificationRow.Quantity * Convert.ToDecimal(mTMoveSpecificationRow.Price)).RoundAs(2);
			}
			doc.MTList.Add(mTMoveSpecificationRow);
			rowId++;
		}
	}

	private void CreateNewMTStockTakingRetailDoc(bool FillAvailQty)
	{
		MTStockTakingRetail mTStockTakingRetail = Settings.DocumentService.Create<MTStockTakingRetail>(DocumentTypeSE.MTStockTakingRetail.Value);
		int rowId = 0;
		try
		{
			mTStockTakingRetail.DocDate = AsOfDate;
			List<GoodsRetailAvailabilityDataProvider.DataRow> selectedOrFocusedRows = Settings.HostingEnvironment.CurrentViewPanel.GetSelectedOrFocusedRows<GoodsRetailAvailabilityDataProvider.DataRow>();
			if (selectedOrFocusedRows.Select((GoodsRetailAvailabilityDataProvider.DataRow p) => p.Storage).Distinct().Count() > 1)
			{
				throw new ASException(ArmSoft.SME.Trade.Common.Resources.RowsMustHaveSameStorage);
			}
			string storage = selectedOrFocusedRows[0].Storage;
			if (!string.IsNullOrWhiteSpace(storage) && TradeSettings.CreateTradeClient().CommonServices.IsStorageAvailable(storage))
			{
				mTStockTakingRetail.Storage = storage;
			}
			foreach (GoodsRetailAvailabilityDataProvider.DataRow item in selectedOrFocusedRows)
			{
				AddRowToMTStockTakingRetailList(mTStockTakingRetail, item, FillAvailQty, ref rowId);
			}
			Settings.DocumentService.CreateWindow(mTStockTakingRetail).Show();
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	private void AddRowToMTStockTakingRetailList(MTStockTakingRetail doc, GoodsRetailAvailabilityDataProvider.DataRow sourceRow, bool fillAvailQty, ref int rowId)
	{
		MaterialCacheRow itemById = TradeSettings.TradeCacheService.Material.GetItemById(sourceRow.GoodID);
		if (itemById != null)
		{
			MTStockTakingBase.MTStockTakingSpecificationRow mTStockTakingSpecificationRow = new MTStockTakingBase.MTStockTakingSpecificationRow
			{
				MTID = sourceRow.GoodID,
				MTCode = itemById.MTCode,
				MTName = itemById.Caption,
				AccQuantity = sourceRow.Quantity,
				UnitCode = sourceRow.Unit,
				UnitBrief = sourceRow.UnitBrief,
				AccSum = sourceRow.Sum,
				Price = sourceRow.Price
			};
			mTStockTakingSpecificationRow.ActualSum = ((mTStockTakingSpecificationRow.AccQuantity == mTStockTakingSpecificationRow.ActualQuantity) ? mTStockTakingSpecificationRow.AccSum : (mTStockTakingSpecificationRow.Price * mTStockTakingSpecificationRow.ActualQuantity).RoundAs(2));
			mTStockTakingSpecificationRow.VarianceQuantity = mTStockTakingSpecificationRow.ActualQuantity - mTStockTakingSpecificationRow.AccQuantity;
			mTStockTakingSpecificationRow.ActualQuantity = (fillAvailQty ? mTStockTakingSpecificationRow.AccQuantity : 0m);
			doc.MTList.Add(mTStockTakingSpecificationRow);
			rowId++;
		}
	}

	private void CreateNewMTExpenseDoc(bool fillAvailQty)
	{
		MTExpense mTExpense = Settings.DocumentService.Create<MTExpense>(DocumentTypeSE.MTExpense.Value);
		int rowId = 0;
		try
		{
			List<GoodsRetailAvailabilityDataProvider.DataRow> selectedOrFocusedRows = Settings.HostingEnvironment.CurrentViewPanel.GetSelectedOrFocusedRows<GoodsRetailAvailabilityDataProvider.DataRow>();
			if (selectedOrFocusedRows.Select((GoodsRetailAvailabilityDataProvider.DataRow p) => p.Storage).Distinct().Count() > 1)
			{
				throw new ASException(ArmSoft.SME.Trade.Common.Resources.RowsMustHaveSameStorage);
			}
			string storage = selectedOrFocusedRows[0].Storage;
			mTExpense.DocDate = AsOfDate;
			if (!string.IsNullOrWhiteSpace(storage))
			{
				StoragesCacheRow item = TradeSettings.TradeCacheService.Storage.GetItem(storage);
				if (item != null && item.Retail && TradeSettings.CreateTradeClient().CommonServices.IsStorageAvailable(storage))
				{
					mTExpense.Storage = storage;
				}
			}
			foreach (GoodsRetailAvailabilityDataProvider.DataRow item2 in selectedOrFocusedRows)
			{
				if (item2.Quantity >= 0m)
				{
					AddRowToMTExpenseList(mTExpense, fillAvailQty, item2, ref rowId);
					continue;
				}
				throw new ASException(string.Format(ArmSoft.SME.Trade.Common.Resources.MTHasNegativeQty, item2.GoodCode));
			}
			mTExpense.MaxRowID = rowId;
			mTExpense.Summ = mTExpense.MTList.Sum((MTExpense.MTExpenseSpecificationRow p) => p.Summa).RoundAs(2);
			mTExpense.Chief = Settings.ParametersService.ChiefAccountantName();
			Settings.DocumentService.CreateWindow(mTExpense).Show();
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	private void AddRowToMTExpenseList(MTExpense doc, bool fillAvailQty, GoodsRetailAvailabilityDataProvider.DataRow sourceRow, ref int rowId)
	{
		MaterialCacheRow itemById = TradeSettings.TradeCacheService.Material.GetItemById(sourceRow.GoodID);
		if (itemById != null)
		{
			MTExpense.MTExpenseSpecificationRow mTExpenseSpecificationRow = new MTExpense.MTExpenseSpecificationRow();
			mTExpenseSpecificationRow.RowID = rowId;
			mTExpenseSpecificationRow.MTID = sourceRow.GoodID;
			mTExpenseSpecificationRow.MTCode = itemById.MTCode;
			mTExpenseSpecificationRow.MTName = itemById.Caption;
			mTExpenseSpecificationRow.IsPartySelected = false;
			MTExpense.MTExpenseSpecificationRow mTExpenseSpecificationRow2 = mTExpenseSpecificationRow;
			decimal quantity;
			if (fillAvailQty)
			{
				quantity = ((sourceRow.Quantity > 0m) ? sourceRow.Quantity : 0m);
			}
			else
			{
				quantity = ((sourceRow.Quantity > 0m) ? 1m : 0m);
			}
			mTExpenseSpecificationRow2.Quantity = quantity;
			mTExpenseSpecificationRow.UnitCode = sourceRow.Unit;
			mTExpenseSpecificationRow.UnitBrief = sourceRow.UnitBrief;
			mTExpenseSpecificationRow.SalePrice = sourceRow.Price;
			mTExpenseSpecificationRow.Summa = (sourceRow.Price * sourceRow.Quantity).RoundAs(2);
			doc.MTList.Add(mTExpenseSpecificationRow);
			rowId++;
		}
	}

	private void CreateNewMTRevaluationRetailDoc(bool fillAvailQty)
	{
		MTRevaluationRetail mTRevaluationRetail = Settings.DocumentService.Create<MTRevaluationRetail>(DocumentTypeSE.MTRevaluationRetail.Value);
		int rowId = 0;
		try
		{
			List<GoodsRetailAvailabilityDataProvider.DataRow> selectedOrFocusedRows = Settings.HostingEnvironment.CurrentViewPanel.GetSelectedOrFocusedRows<GoodsRetailAvailabilityDataProvider.DataRow>();
			if (selectedOrFocusedRows.Select((GoodsRetailAvailabilityDataProvider.DataRow p) => p.Storage).Distinct().Count() > 1)
			{
				throw new ASException(ArmSoft.SME.Trade.Common.Resources.RowsMustHaveSameStorage);
			}
			string storage = selectedOrFocusedRows[0].Storage;
			mTRevaluationRetail.DocDate = AsOfDate;
			if (!string.IsNullOrWhiteSpace(storage))
			{
				StoragesCacheRow item = TradeSettings.TradeCacheService.Storage.GetItem(storage);
				if (item != null && item.Retail && TradeSettings.CreateTradeClient().CommonServices.IsStorageAvailable(storage))
				{
					mTRevaluationRetail.Storage = storage;
				}
			}
			foreach (GoodsRetailAvailabilityDataProvider.DataRow item2 in selectedOrFocusedRows)
			{
				if (item2.Quantity >= 0m)
				{
					AddRowToMTRevaluationRetailList(mTRevaluationRetail, item2, fillAvailQty, ref rowId);
					continue;
				}
				throw new ASException(string.Format(ArmSoft.SME.Trade.Common.Resources.MTHasNegativeQty, item2.GoodCode));
			}
			mTRevaluationRetail.MaxRowID = rowId;
			Settings.DocumentService.CreateWindow(mTRevaluationRetail).Show();
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	private void AddRowToMTRevaluationRetailList(MTRevaluationRetail doc, GoodsRetailAvailabilityDataProvider.DataRow sourceRow, bool fillAvailQty, ref int rowId)
	{
		MaterialCacheRow itemById = TradeSettings.TradeCacheService.Material.GetItemById(sourceRow.GoodID);
		if (itemById != null)
		{
			doc.MTList.Add(new MTRevaluationRetail.MTRevaluationRetailSpecificationRow
			{
				RowID = rowId,
				MTID = sourceRow.GoodID,
				MTCode = itemById.MTCode,
				MTName = itemById.Caption,
				Quantity = ((!fillAvailQty) ? ((sourceRow.Quantity > 0m) ? 1m : 0m) : ((sourceRow.Quantity > 0m) ? sourceRow.Quantity : 0m)),
				UnitCode = sourceRow.Unit,
				UnitBrief = sourceRow.UnitBrief,
				Cost = sourceRow.Price,
				NewCost = sourceRow.Price
			});
			rowId++;
		}
	}

	private void CreateNewMTReturnToSupplierDoc(bool fillAvailQty)
	{
		MTReturnToSupplier doc = Settings.DocumentService.Create<MTReturnToSupplier>(DocumentTypeSE.MTReturnToSupplier.Value);
		int num = 0;
		MTReturnToSupplier.MTRetToSupSpecificationRow mTRetToSupSpecificationRow = null;
		try
		{
			string text = Settings.ParametersService.DefaultStorage();
			doc.DocDate = AsOfDate;
			if (!string.IsNullOrWhiteSpace(text))
			{
				StoragesCacheRow item = TradeSettings.TradeCacheService.Storage.GetItem(text);
				if (item != null && item.Retail && TradeSettings.CreateTradeClient().CommonServices.IsStorageAvailable(text))
				{
					doc.Storage = text;
				}
			}
			doc.SalesConsultant = Settings.ParametersService.DefaultSalesConsultant();
			string text2 = Settings.ParametersService.DefaultSupplier();
			if (!string.IsNullOrWhiteSpace(text2))
			{
				PartnerCacheRow item2 = TradeSettings.TradeCacheService.Partner.GetItem(text2);
				if (item2 != null && item2.Customer)
				{
					doc.PartnerContractGroup.PartnerCode = text2;
					PartnerCacheRow.ContractRow defaultContract = TradeSettings.TradeCacheService.Partner.GetDefaultContract(item2);
					if (defaultContract != null)
					{
						doc.PartnerContractGroup.ContractCode = defaultContract.Contract;
					}
				}
			}
			foreach (GoodsRetailAvailabilityDataProvider.DataRow selectedOrFocusedRow in Settings.HostingEnvironment.CurrentViewPanel.GetSelectedOrFocusedRows<GoodsRetailAvailabilityDataProvider.DataRow>())
			{
				if (selectedOrFocusedRow.Quantity >= 0m)
				{
					mTRetToSupSpecificationRow = GetMTReturnToSupplierCurrentMTListRow(doc, selectedOrFocusedRow, fillAvailQty, num);
					if (mTRetToSupSpecificationRow != null)
					{
						doc.MTList.Add(mTRetToSupSpecificationRow);
						num++;
					}
					continue;
				}
				throw new ASException(string.Format(ArmSoft.SME.Trade.Common.Resources.MTHasNegativeQty, selectedOrFocusedRow.GoodCode));
			}
			doc.MaxRowID = num;
			doc.SummVAT = doc.MTList.Sum((MTReturnToSupplier.MTRetToSupSpecificationRow p) => PriceHelper.CalcVATAmountByType(p.Summa, doc.VATType, p.VAT)).RoundAs(2);
			doc.Summ = doc.MTList.Sum((MTReturnToSupplier.MTRetToSupSpecificationRow p) => p.Summa).RoundAs(2);
			doc.SupplierName = Settings.ParametersService.CompanyName();
			doc.SupplierLegalAddress = Settings.ParametersService.CompanyAddress();
			doc.SupplierBusinessAddress = Settings.ParametersService.CompanyBusinessAddress();
			doc.SupplierSettlAcc = TradeSettings.TradeCacheService.AccountInBank.GetDefaultSettlementAccount(isSettlAccount: true);
			doc.SupplierTaxCode = Settings.ParametersService.TaxCode();
			doc.SupplierMngPost = Settings.ParametersService.ManagerPost();
			doc.SupplierMngName = Settings.ParametersService.ManagerName();
			doc.SupplierChiefAccPost = Settings.ParametersService.ChiefAccountantPost();
			doc.SupplierChiefAccName = Settings.ParametersService.ChiefAccountantName();
			Settings.DocumentService.CreateWindow(doc).Show();
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	private MTReturnToSupplier.MTRetToSupSpecificationRow GetMTReturnToSupplierCurrentMTListRow(MTReturnToSupplier doc, GoodsRetailAvailabilityDataProvider.DataRow currentRow, bool fillAvailQty, int rowID)
	{
		MTReturnToSupplier.MTRetToSupSpecificationRow mTRetToSupSpecificationRow = null;
		MaterialCacheRow itemById = TradeSettings.TradeCacheService.Material.GetItemById(currentRow.GoodID);
		if (itemById != null)
		{
			mTRetToSupSpecificationRow = new MTReturnToSupplier.MTRetToSupSpecificationRow
			{
				RowID = rowID,
				Storage = currentRow.Storage,
				MTID = currentRow.GoodID,
				MTCode = itemById.MTCode,
				MTName = itemById.Caption,
				IsPartySelected = false,
				Quantity = ((!fillAvailQty) ? ((currentRow.Quantity > 0m) ? 1m : 0m) : ((currentRow.Quantity > 0m) ? currentRow.Quantity : 0m)),
				UnitCode = currentRow.Unit,
				UnitBrief = currentRow.UnitBrief,
				VAT = itemById.VAT,
				Price = 0m,
				Summa = 0m,
				SalePrice = currentRow.Price
			};
			mTRetToSupSpecificationRow.SaleSumma = (fillAvailQty ? currentRow.Sum : mTRetToSupSpecificationRow.SalePrice);
		}
		return mTRetToSupSpecificationRow;
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
		ParametersProvider.RemDate = AsOfDate;
		ParametersProvider.Storage = Storage;
		ParametersProvider.GoodGroup = GoodGroup;
		ParametersProvider.GoodCode = Good;
		ParametersProvider.ShowAltUnit = ShowAltUnit;
		ParametersProvider.DetailByStorage = DetailByStorage;
	}

	public override ToolTip ToolTipContent(object sender)
	{
		GoodsRetailAvailabilityDataProvider.DataRow dataRow = (GoodsRetailAvailabilityDataProvider.DataRow)((Image)sender).Tag;
		if (dataRow == null)
		{
			return null;
		}
		int goodID = dataRow.GoodID;
		if (dataRow.ImageExists && dataRow.Image == null)
		{
			dataRow.Image = Settings.ImagesService.GetItemImage(ImageItemTypeSE.Materials.Value, TradeSettings.TradeCacheService.Material.GetQntUnit(goodID, dataRow.Unit)?.ImageGUID.ToString());
		}
		if (dataRow.Image != null && dataRow.Image.Length != 0)
		{
			LayoutItemWithRequired layoutItemWithRequired = new LayoutItemWithRequired();
			layoutItemWithRequired.LabelPosition = LayoutItemLabelPosition.Top;
			layoutItemWithRequired.Label = dataRow.Caption;
			ImageEdit imageEdit = new ImageEdit();
			imageEdit.IsReadOnly = true;
			imageEdit.ShowLoadDialogOnClickMode = ShowLoadDialogOnClickMode.Never;
			imageEdit.EditMode = EditMode.InplaceInactive;
			imageEdit.EditValue = dataRow.Image;
			imageEdit.Width = 200.0;
			imageEdit.Height = 200.0;
			imageEdit.VerticalAlignment = VerticalAlignment.Stretch;
			imageEdit.HorizontalAlignment = HorizontalAlignment.Stretch;
			layoutItemWithRequired.Content = imageEdit;
			return new ToolTip
			{
				Content = layoutItemWithRequired
			};
		}
		return new ToolTip
		{
			Content = ArmSoft.SME.Common.Resources.NoImage
		};
	}
}
