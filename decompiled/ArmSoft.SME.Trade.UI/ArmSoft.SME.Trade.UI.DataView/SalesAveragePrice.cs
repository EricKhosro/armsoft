using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Extensions;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Core.UI.Controls;
using ArmSoft.SME.Core.UI.DataViews;
using ArmSoft.SME.Core.UI.Documents;
using ArmSoft.SME.Core.UI.DropDowns;
using ArmSoft.SME.Trade.Client.Data.Reports;
using ArmSoft.SME.Trade.Common;
using ArmSoft.SME.Trade.Common.Models.Caches;
using ArmSoft.SME.Trade.Common.Models.PrintableElements;
using ArmSoft.SME.Trade.Common.SmartEnums;
using ArmSoft.SME.Trade.UI.CustomUIElements;
using ArmSoft.SME.Trade.UI.DropDown;
using ArmSoft.SME.Trade.UI.Helpers;
using ArmSoft.SME.Trade.UI.OtherWindows;
using DevExpress.Xpf.Editors;

namespace ArmSoft.SME.Trade.UI.DataView;

[DataView(10068)]
public class SalesAveragePrice : TradeDataView<SalesAveragePriceDataProvider.DataRow, SalesAveragePriceDataProvider.Param>
{
	private class DataViewDialog : DataViewParametersDialogWindow
	{
		private readonly SalesAveragePrice _dataView;

		private readonly DateEditControl _startDate;

		private readonly DateEditControl _endDate;

		private readonly MultiSelectDropDownControl _storage;

		private readonly DropDownControl _goodGroup;

		private readonly DropDownControl _good;

		private MultiSelectDropDownControl _docType;

		private readonly MultiSelectDropDownControl _priceListType;

		private readonly GoodsFilter _goods;

		public DataViewDialog(SalesAveragePrice dataView, bool useParamValues, bool isRefreshMode)
			: base(dataView)
		{
			_dataView = dataView;
			Caption = ArmSoft.SME.Trade.Common.Resources.SalesAveragePrice;
			_goods = new GoodsFilter(shortMode: true, ItemTypesSE.Goods, "", isShowOnlyInPriceList: true);
			LayoutGroupExt container = AddHorizontalLayoutGroup();
			if (!isRefreshMode)
			{
				AddStoreValuesButton(container);
			}
			int num = 3;
			List<short> list = new List<short>(num);
			CollectionsMarshal.SetCount(list, num);
			Span<short> span = CollectionsMarshal.AsSpan(list);
			span[0] = DocumentTypeSE.MTInvoice.Value;
			span[1] = DocumentTypeSE.MTBill.Value;
			span[2] = DocumentTypeSE.MTSaleRetail.Value;
			List<short> values = list;
			_startDate = AddDateControl(container, "StartDate", ArmSoft.SME.Common.Resources.DatePeriod, useLongDate: false, isRequired: true, !useParamValues, ArmSoft.SME.Common.Resources.DatePeriodBegin);
			_startDate.EditValue = (useParamValues ? dataView.StartDate : new DateOnly?(Settings.HostingEnvironment.DateBegin));
			_endDate = AddDateControl(container, "EndDate", "", useLongDate: false, isRequired: true, !useParamValues, ArmSoft.SME.Common.Resources.DatePeriodEnd);
			_endDate.EditValue = (useParamValues ? dataView.EndDate : new DateOnly?(Settings.HostingEnvironment.DateEnd));
			AddSpecialDateButton(container, _startDate, _endDate);
			string storage = ArmSoft.SME.Common.Resources.Storage;
			bool? checkViewOrOperationAccess = true;
			_storage = AddMultiSelectDropDownControl("Storage", storage, new ArmSoft.SME.Trade.UI.DropDown.Storages(null, checkViewOrOperationAccess), isRequired: false, !useParamValues);
			_storage.SelectedString = (useParamValues ? dataView.Storage : string.Empty);
			_goodGroup = AddDropDownControl("GoodGroup", ArmSoft.SME.Common.Resources.Group, new GoodsGroups(), isRequired: false, allowAdditionButtons: false, !useParamValues);
			_goodGroup.Code = (useParamValues ? dataView.GoodGroup : string.Empty);
			_goodGroup.EditValueChanged += new SafeEventHandler<EditValueChangedEventArgs>(GoodsGroup_EditValueChanged).Handler;
			_good = AddDropDownControl("GoodCode", ArmSoft.SME.Trade.Common.Resources.MaterialValue, _goods, isRequired: false, allowAdditionButtons: false, !useParamValues);
			_good.Code = (useParamValues ? dataView.Good : string.Empty);
			_docType = AddMultiSelectDropDownControl("DocType", ArmSoft.SME.Trade.Common.Resources.MatRepOperationType, new DocumentType(values, checkViewAccess: true, checkAddAccess: true), isRequired: false, !useParamValues);
			_docType.SelectedString = (useParamValues ? dataView.DocType : string.Empty);
			_priceListType = AddMultiSelectDropDownControl("PriceListType", ArmSoft.SME.Trade.Common.Resources.ShowPricesByPT, new ArmSoft.SME.Trade.UI.DropDown.PriceTypes(), isRequired: false, !useParamValues);
			_priceListType.SelectedString = (useParamValues ? dataView.PriceListType : string.Empty);
		}

		protected override void DialogValidation(DialogWindowValidationArgs args)
		{
			try
			{
				ArmSoft.SME.Core.UI.ValidationService.ValidateWrongPeriod(_startDate.EditValue, _endDate.EditValue);
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
			_dataView.StartDate = _startDate.EditValue;
			_dataView.EndDate = _endDate.EditValue;
			_dataView.Storage = _storage.SelectedString;
			_dataView.GoodGroup = _goodGroup.Code;
			_dataView.Good = _good.Code;
			_dataView.DocType = _docType.SelectedString;
			_dataView.PriceListType = _priceListType.SelectedString;
		}

		private void GoodsGroup_EditValueChanged(object sender, EditValueChangedEventArgs e)
		{
			if (e.OldValue != e.NewValue)
			{
				_goods.Group = (string)e.NewValue;
			}
		}
	}

	private bool _isShowForPanel;

	public override string Name => "SalesAvgPrice";

	public override string Caption => ArmSoft.SME.Trade.Common.Resources.SalesAveragePrice + " " + StartDate.FormatDDMMYY() + " - " + EndDate.FormatDDMMYY();

	public override string HelpId => "Sales_Report_SalesAveragePrice";

	public override bool DialogSupported => true;

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

	public string DocType
	{
		get
		{
			return Convert.ToString(Parameters["DocType"].Value);
		}
		set
		{
			Parameters["DocType"].Value = value;
		}
	}

	public string PriceListType
	{
		get
		{
			return (string)Parameters["PriceListType"].Value;
		}
		set
		{
			Parameters["PriceListType"].Value = value;
		}
	}

	public SalesAveragePrice(bool forPanel = false)
	{
		_isShowForPanel = forPanel;
		DataProvider = new SalesAveragePriceDataProvider(TradeSettings.CreateTradeClient());
		AddParameter("StartDate", ArmSoft.SME.Common.Resources.DatePeriodBegin, Settings.HostingEnvironment.DateBegin, "dd/MM/yy");
		AddParameter("EndDate", ArmSoft.SME.Common.Resources.DatePeriodEnd, Settings.HostingEnvironment.DateBegin, "dd/MM/yy");
		AddParameter("Storage", ArmSoft.SME.Common.Resources.Storage, string.Empty);
		AddParameter("GoodGroup", ArmSoft.SME.Common.Resources.TypeWord, string.Empty);
		AddParameter("GoodCode", ArmSoft.SME.Trade.Common.Resources.MaterialOrService, string.Empty);
		AddParameter("PriceListType", ArmSoft.SME.Common.Resources.PartnersGroup, string.Empty);
		AddParameter("DocType", ArmSoft.SME.Common.Resources.Buyer, string.Empty);
		FillColumnsFromDataProvider();
	}

	public override void BeforeLoadData(DataViewPanel dvp)
	{
		base.BeforeLoadData(dvp);
		List<string> list = PriceListType.SplitSelected().SelectMany((string priceType) => new string[1] { "PT_" + priceType }).ToList();
		foreach (ViewColumn view in Columns.ViewList)
		{
			if (view.Name.Substring(0, 3) == "PT_")
			{
				dvp.Set_ColumnVisible(view.Name, list.Contains(view.Name));
			}
		}
		if (!dvp.IsSorted)
		{
			dvp.Set_ColumnSortOrder("ItemCode");
		}
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

	public override void InitContextFunctions()
	{
		base.InitContextFunctions();
		if (Settings.HostingEnvironment.CurrentViewPanel.FocusedRow() != null)
		{
			if (Settings.UserAccessService.GetProviderPermission(DataProviderTypeSE.MTPrices).AllowView)
			{
				Settings.HostingEnvironment.CurrentViewPanel.AddContextFunction("ContextFunctions", "ShowPricesFormation", ArmSoft.SME.Trade.Common.Resources.PricesFormation, ShowPricesFormation);
			}
			Settings.HostingEnvironment.CurrentViewPanel.AddContextFunction("ContextFunctions", "PrintLabel", ArmSoft.SME.Trade.Common.Resources.PrintLabel, PrintLabel);
		}
	}

	private void PrintLabel()
	{
		List<Label.GoodLabelRow> list = new List<Label.GoodLabelRow>();
		Label.GoodLabelRow goodLabelRow = new Label.GoodLabelRow();
		foreach (SalesAveragePriceDataProvider.DataRow selectedOrFocusedRow in Settings.HostingEnvironment.CurrentViewPanel.GetSelectedOrFocusedRows<SalesAveragePriceDataProvider.DataRow>())
		{
			MaterialCacheRow item = TradeSettings.TradeCacheService.Material.GetItem(selectedOrFocusedRow.ItemCode);
			if (item != null && item.Type == GoodsTypesSE.Good)
			{
				goodLabelRow = InitContextFunctionsHelper.BuildLabelGoodRow(item);
				goodLabelRow.Quantity = selectedOrFocusedRow.Quantity;
				list.Add(goodLabelRow);
			}
		}
		new PrintLabelDialog(null, null, list, isFromGoodsAvailability: true, isFromSalesAvgPrice: true).Invoke();
	}

	private void ShowPricesFormation()
	{
		List<PricesFormationWindow.GoodsRow> list = new List<PricesFormationWindow.GoodsRow>();
		foreach (SalesAveragePriceDataProvider.DataRow selectedOrFocusedRow in Settings.HostingEnvironment.CurrentViewPanel.GetSelectedOrFocusedRows<SalesAveragePriceDataProvider.DataRow>())
		{
			MaterialCacheRow item = TradeSettings.TradeCacheService.Material.GetItem(selectedOrFocusedRow.ItemCode);
			list.Add(new PricesFormationWindow.GoodsRow(item.Id, selectedOrFocusedRow.ItemCode, selectedOrFocusedRow.UnitCode, item.AltUnit, selectedOrFocusedRow.SaleAvgPrice));
		}
		new PricesFormationWindow(list, "PricesFormation" + Name, showPurchasePriceRow: true, showSalePriceRow: true, showRetailAvailabilityPriceRow: true, "", null, null, showDiscountPriceRow: false, isAveragePrice: true).Invoke();
	}

	protected override void ApplyParametersToDataProvider()
	{
		ParametersProvider.StartDate = StartDate;
		ParametersProvider.EndDate = EndDate;
		ParametersProvider.Storage = Storage;
		ParametersProvider.GoodGroup = GoodGroup;
		ParametersProvider.GoodCode = Good;
		ParametersProvider.DocType = DocType;
		ParametersProvider.PriceListType = PriceListType;
	}
}
