using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Ardalis.SmartEnum;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Constants;
using ArmSoft.SME.Common.CustomTypes;
using ArmSoft.SME.Common.Extensions;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Core.UI.Controls;
using ArmSoft.SME.Core.UI.Documents;
using ArmSoft.SME.Core.UI.DropDowns;
using ArmSoft.SME.Core.UI.Helpers;
using ArmSoft.SME.Core.UI.PersistableObject;
using ArmSoft.SME.Trade.Client.Data.Reports;
using ArmSoft.SME.Trade.Common;
using ArmSoft.SME.Trade.Common.Models.Caches;
using ArmSoft.SME.Trade.Common.Models.Documents;
using ArmSoft.SME.Trade.Common.Parameters;
using ArmSoft.SME.Trade.Common.SmartEnums;
using ArmSoft.SME.Trade.UI.CustomUIElements;
using ArmSoft.SME.Trade.UI.DropDown;
using ArmSoft.SME.Trade.UI.Helpers;
using DevExpress.Utils;
using DevExpress.Xpf.Editors;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.LayoutControl;

namespace ArmSoft.SME.Trade.UI.Documents;

[DocumentWindow(16)]
public class MTStockTakingWindow : MTStockTakingWindowBase<MTStockTaking>
{
	private DropDownControl _priceListType;

	protected override double WidthValue => 890.0;

	protected override double HeightValue => 480.0;

	protected override double MinWidthValue => 890.0;

	protected override double MinHeightValue => 250.0;

	public MTStockTakingWindow()
	{
	}

	public MTStockTakingWindow(MTStockTaking doc)
		: base(doc)
	{
	}

	protected override void ConstructUI()
	{
		base.ConstructUI();
		LayoutGroupExt container = AddLayoutGroup(null, LayoutGroupView.Group, Orientation.Horizontal);
		_date = AddDateControl(container, Definition.Fields["DocDate"]);
		_date.Margin = new Thickness(0.0, 0.0, 10.0, 0.0);
		_docNum = AddButtonEditControl(container, Definition.Fields["DocNum"], 1);
		_docNum.AllowMagicButton = true;
		_docNum.MagicToolTip = ArmSoft.SME.Common.Resources.DocNextNumbers;
		_docNum.MagicClick += new SafeDocumentEventHandler<MagicClickEventArgs>(DocNum_MagicClick, this).Handler;
		_docNum.EditValueChanged += new SafeDocumentEventHandler<EditValueChangedEventArgs>(DocNum_EditValueChanged, this).Handler;
		_storage = AddDropDownControl(null, Definition.Fields["Storage"], new Storages(false));
		_storage.EditValueChanged += new SafeDocumentEventHandler<EditValueChangedEventArgs>(Storage_EditValueChanged, this).Handler;
		_goodGroup = AddDropDownControl(null, Definition.Fields["GoodGroup"], new GoodsGroups());
		LayoutGroupExt container2 = AddLayoutGroup(null, LayoutGroupView.Group, Orientation.Horizontal);
		_inputValue = AddStringControl(container2, ArmSoft.SME.Trade.Common.Resources.InputBoxCaption + " (F2)", isRequired: false, 20, HorizontalAlignment.Stretch);
		string text = ArmSoft.SME.Trade.Common.Resources.cmdCodeEnter + " (F8)";
		_inputValue.NullText = ArmSoft.SME.Trade.Common.Resources.BarCodeAndCodeInputNullText;
		_inputValue.KeyDown += new SafeDocumentEventHandler<KeyEventArgs>(BarCode_KeyDown, this).Handler;
		_codeEnter = AddButton(container2, null, text, null, text, isTabStop: true);
		_codeEnter.Margin = new Thickness(0.0, 0.0, 18.0, 0.0);
		_codeEnter.Click += new SafeDocumentEventHandler<RoutedEventArgs>(CodeEnter_Click, this).Handler;
		_tabs = AddTabControl(null);
		LayoutGroup container3 = AddTabbedGroup(_tabs, ArmSoft.SME.Common.Resources.General, "General");
		_currCode = AddDropDownControl(container3, Definition.Fields["CurrCode"], new Currency(shortMode: true), allowAdditionButtons: false, fieldWithCaption: true, HorizontalAlignment.Stretch);
		_currCode.EditValueChanged += new SafeDocumentEventHandler<EditValueChangedEventArgs>(CurrCode_EditValueChanged, this).Handler;
		_currExChngRate = AddExchangeRateControl(container3, Definition.Fields["ExchangeRate"]);
		_priceListType = AddDropDownControl(container3, Definition.Fields["PriceType"], new PriceTypes());
		_priceListType.EditValueChanged += new SafeDocumentEventHandler<EditValueChangedEventArgs>(PriceListType_EditValueChanged, this).Handler;
		AddComboBoxControl(container3, Definition.Fields["Comment"], TradeSettings.TradeCacheService.StandardComments.GetCaptionsList(), HorizontalAlignment.Stretch);
		_owner = AddDropDownControl(container3, Definition.Fields["Owner"], new UsersFilter());
		_owner.CodeWidth = 12;
		_owner.ShowComment = false;
		_owner.InnerCodeEdit.Width = _owner.InnerCodeEdit.MinWidth;
		if (!Settings.LoginService.IsAdmin && Settings.ParametersService.DenyAccessToOtherOwnerDocs())
		{
			((LayoutItemWithRequired)(object)_owner.Parent).Visibility = Visibility.Collapsed;
		}
		_mtStockTakingGrid = AddGrid(container3, Definition.Grids["MTList"], HorizontalAlignment.Stretch, VerticalAlignment.Stretch, isSupportDialogMode: true);
		_mtStockTakingGrid.AllowMoveUp = true;
		_mtStockTakingGrid.AllowMoveDown = true;
		_mtStockTakingGrid.AllowDuplicate = false;
		_mtStockTakingGrid.IsVisibleViewEditButton = false;
		_mtStockTakingGrid.AddingRow += new SafeDocumentEventHandler<DocumentGridAddingRowEventArgs>(MTStockTakingGrid_AddingRow, this).Handler;
		_mtStockTakingGrid.DeletedRow += new SafeDocumentEventHandler<EventArgs>(MTStockTakingGrid_DeletedRow, this).Handler;
		_mtStockTakingGrid.MovingRowUp += new SafeDocumentEventHandler<DocumentGridMovingRowUpOrDownEventArgs>(MTStockTakingGrid_MovingRowUp, this).Handler;
		_mtStockTakingGrid.MovingRowDown += new SafeDocumentEventHandler<DocumentGridMovingRowUpOrDownEventArgs>(MTStockTakingGrid_MovingRowDown, this).Handler;
		_mtStockTakingGrid.ClickDropDown += new SafeDocumentEventHandler<DocumentGridClickDropDownEventArgs>(MTStockTakingGrid_ClickDropDown, this).Handler;
		_mtStockTakingTableView = _mtStockTakingGrid.View;
		_mtStockTakingTableView.CellValueChanged += new SafeDocumentEventHandler<CellValueChangedEventArgs>(MTStockTakingTableView_CellValueChanged, this).Handler;
		_mtStockTakingTableView.FocusedRowHandleChanged += new SafeDocumentEventHandler<FocusedRowHandleChangedEventArgs>(MTStockTakingTableView_FocusedRowHandleChanged, this).Handler;
		_loadGoodsList = _mtStockTakingGrid.AddButton("fill_down_16.svg", ArmSoft.SME.Common.Resources.ButtonLoad, ArmSoft.SME.Common.Resources.ButtonLoad, 90.0);
		_loadGoodsList.Click += new SafeDocumentEventHandler<RoutedEventArgs>(LoadGoodsList_Click, this).Handler;
		_refreshGoodsList = _mtStockTakingGrid.AddButton("refresh_16.svg", ArmSoft.SME.Common.Resources.Refresh, ArmSoft.SME.Common.Resources.Refresh, 110.0);
		_refreshGoodsList.Click += new SafeDocumentEventHandler<RoutedEventArgs>(RefreshGoodsList_Click, this).Handler;
		_filteredSearch = _mtStockTakingGrid.AddButton(null, ArmSoft.SME.Trade.Common.Resources.GroupAddition, ArmSoft.SME.Trade.Common.Resources.GroupAddition, 130.0);
		_filteredSearch.Click += new SafeDocumentEventHandler<RoutedEventArgs>(FilteredSearch_Click, this).Handler;
		_mtStockTakingGrid.AddRowIndicator(35);
		_codeCol = _mtStockTakingGrid.AddDropDownColumn(Definition.Grids["MTList"].Columns["MTCode"], new GoodsFilter(shortMode: true, ItemTypesSE.Goods.Value, "", isShowOnlyInPriceList: true), allowAdditionButtons: true);
		_codeCol.Width = 125.0;
		_nameCol = _mtStockTakingGrid.AddStringColumn(Definition.Grids["MTList"].Columns["MTName"]);
		_nameCol.AllowEditing = DefaultBoolean.False;
		_nameCol.Width = 225.0;
		_unitCodeCol = _mtStockTakingGrid.AddDropDownColumn(Definition.Grids["MTList"].Columns["UnitCode"], new MTQntUnits(), allowAdditionButtons: false, isOwnProcessingClickDropDown: true);
		_mtStockTakingGrid.AddStringColumn(Definition.Grids["MTList"].Columns["UnitBrief"]).AllowEditing = DefaultBoolean.False;
		_accQuantityCol = _mtStockTakingGrid.AddNumberColumn(Definition.Grids["MTList"].Columns["AccQuantity"], showTotal: true);
		_accQuantityCol.AllowEditing = DefaultBoolean.False;
		_actualQuantityCol = _mtStockTakingGrid.AddNumberColumn(Definition.Grids["MTList"].Columns["ActualQuantity"], showTotal: true);
		_varianceQuantityCol = _mtStockTakingGrid.AddNumberColumn(Definition.Grids["MTList"].Columns["VarianceQuantity"]);
		_varianceQuantityCol.AllowEditing = DefaultBoolean.False;
		_accSumCol = _mtStockTakingGrid.AddNumberColumn(Definition.Grids["MTList"].Columns["AccSum"], showTotal: true);
		_accSumCol.AllowEditing = DefaultBoolean.False;
		_priceCol = _mtStockTakingGrid.AddNumberColumn(Definition.Grids["MTList"].Columns["Price"], showTotal: true);
		_salePriceCol = _mtStockTakingGrid.AddNumberColumn(Definition.Grids["MTList"].Columns["SalePrice"]);
		_actualSumCol = _mtStockTakingGrid.AddNumberColumn(Definition.Grids["MTList"].Columns["ActualSum"], showTotal: true);
		_saleSumCol = _mtStockTakingGrid.AddNumberColumn(Definition.Grids["MTList"].Columns["SaleSum"]);
		_mtStockTakingList = new ObservableCollection<MTStockTakingSpecificationRow>();
		foreach (GridColumn column in _mtStockTakingGrid.Grid.Columns)
		{
			column.AllowSorting = DefaultBoolean.True;
			column.AllowColumnFiltering = DefaultBoolean.True;
			column.FilterPopupMode = FilterPopupMode.CheckedList;
		}
	}

	protected override void CreateChildDocument(short childDocType, MTStockTaking baseDoc)
	{
		base.CreateChildDocument(childDocType, baseDoc);
		if ((uint)(childDocType - 7) <= 2u || childDocType == 20)
		{
			CreateBasedOnDocHelper.CreateDocument((DocumentTypeSE)(SmartEnum<DocumentTypeSE, short>)childDocType, PO.ISN, DVPID);
		}
	}

	protected override void OnNew()
	{
		base.OnNew();
		_date.EditValue = Settings.HostingEnvironment.CurrentDate;
		_owner.Code = Settings.LoginService.UserName;
	}

	protected override void OnNewAfterSave()
	{
		PO = new MTStockTaking();
		_mtStockTakingList.Clear();
		base.OnNewAfterSave();
		_owner.Code = Settings.LoginService.UserName;
	}

	protected override void SetReadOnlyGeneralFields(bool isReadOnly)
	{
		base.SetReadOnlyGeneralFields(isReadOnly);
		_currCode.IsReadOnly = isReadOnly;
		_currExChngRate.IsReadOnly = isReadOnly || _currCode.Code == "AMD";
	}

	protected override void OnEdit()
	{
		base.OnEdit();
		if (string.IsNullOrWhiteSpace(_docNum.Text))
		{
			SetNextDocumentNumber();
		}
		if (!_date.EditValue.HasValue || _date.EditValue.Value == ArmSoft.SME.Common.Constants.Common.B2BDateNothing.ToDateOnly())
		{
			_date.EditValue = Settings.HostingEnvironment.CurrentDate;
		}
		if (string.IsNullOrWhiteSpace(_currCode.Code))
		{
			_currCode.Code = "AMD";
			_currExChngRate.Base = 1m;
			_currExChngRate.Rate = 1m;
		}
		if (_mtStockTakingList.Count > 0)
		{
			_isLoaded = _mtStockTakingList.Any((MTStockTakingSpecificationRow x) => !x.UserAdd);
		}
		_setFocusOnActivation = _docNum;
	}

	protected override void LoadDataToControls()
	{
		base.LoadDataToControls();
		if (string.IsNullOrWhiteSpace(_currCode.Code) || _currCode.Code == "AMD" || !_currCode.DoValidate())
		{
			_priceCol.Header = ArmSoft.SME.Common.Resources.Price;
			_accSumCol.Header = ArmSoft.SME.Common.Resources.AccSumm;
			_actualSumCol.Header = ArmSoft.SME.Common.Resources.ActualSumm;
			_currExChngRate.Rate = 1m;
			_currExChngRate.Base = 1m;
		}
		else
		{
			string text = " " + ArmSoft.SME.Trade.Common.Resources.InCurrencyShort;
			_priceCol.Header = ArmSoft.SME.Common.Resources.Price + text;
			_accSumCol.Header = ArmSoft.SME.Common.Resources.AccSumm + text;
			_actualSumCol.Header = ArmSoft.SME.Common.Resources.ActualSumm + text;
		}
	}

	private void CurrCode_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		if (string.IsNullOrWhiteSpace(_currCode.Code) || _currCode.Code == "AMD" || !_currCode.DoValidate())
		{
			_currExChngRate.IsReadOnly = true;
			_priceCol.Header = ArmSoft.SME.Common.Resources.Price;
			_accSumCol.Header = ArmSoft.SME.Common.Resources.AccSumm;
			_actualSumCol.Header = ArmSoft.SME.Common.Resources.ActualSumm;
			_currExChngRate.Rate = 1m;
			_currExChngRate.Base = 1m;
		}
		else
		{
			_currExChngRate.IsReadOnly = false;
			string text = " " + ArmSoft.SME.Trade.Common.Resources.InCurrencyShort;
			_priceCol.Header = ArmSoft.SME.Common.Resources.Price + text;
			_accSumCol.Header = ArmSoft.SME.Common.Resources.AccSumm + text;
			_actualSumCol.Header = ArmSoft.SME.Common.Resources.ActualSumm + text;
			if (!string.IsNullOrWhiteSpace(_currCode.Code) && _currCode.Code != "AMD" && _date.DoValidate() && _currCode.DoValidate())
			{
				ExchangeRate exchangeRates = TradeSettings.CreateTradeClient().CommonServices.GetExchangeRates(_currCode.Code, _date.EditValue.Value);
				_currExChngRate.Rate = exchangeRates.Rate;
				_currExChngRate.Base = exchangeRates.Base;
			}
		}
		_currExChngRate.DoValidate();
	}

	protected override void ValidateDocFields()
	{
		base.ValidateDocFields();
		if (string.IsNullOrWhiteSpace(_currCode.Code))
		{
			ControlsHelper.SetFocus(_currCode);
			throw new ASException(ArmSoft.SME.Trade.Common.Resources.ValidateEmptyCurrCode);
		}
	}

	protected override void MTStockTakingGrid_DeletedRow(object sender, EventArgs e)
	{
		base.MTStockTakingGrid_DeletedRow(sender, e);
		_currCode.IsReadOnly = _storage.IsReadOnly;
		if (_mtStockTakingList.Count == 0)
		{
			_currExChngRate.IsReadOnly = _currCode.Code == "AMD";
		}
	}

	private List<GoodsRemainsDataProvider.DataRow> AvailabilityRows(string goodCode = "", bool showAltUnit = false)
	{
		GoodsRemainsDataProvider goodsRemainsDataProvider = new GoodsRemainsDataProvider(TradeSettings.CreateTradeClient());
		GoodsRemainsDataProvider.Param param = new GoodsRemainsDataProvider.Param
		{
			RemDate = _date.EditValue.Value,
			Storage = _storage.Code,
			MTGroup = _goodGroup.Code,
			ShowSumms = true,
			ShowCurrSumms = true,
			ShowByAditionalQntUnits = showAltUnit
		};
		if (!string.IsNullOrWhiteSpace(goodCode))
		{
			param.MTCode = goodCode;
		}
		return goodsRemainsDataProvider.GetRows(param);
	}

	private void AddNewRow(GoodsRemainsDataProvider.DataRow sourceRow, decimal actualQuantity, bool userAdd = false)
	{
		MTStockTakingSpecificationRow mTStockTakingSpecificationRow = new MTStockTakingSpecificationRow
		{
			MTCode = sourceRow.MTCode,
			MTID = sourceRow.MTID,
			MTName = sourceRow.Caption,
			UnitCode = sourceRow.UnitCode,
			AccQuantity = sourceRow.Quantity,
			ActualQuantity = actualQuantity
		};
		decimal num = (mTStockTakingSpecificationRow.AccSum = ((_currCode.Code == "AMD") ? sourceRow.CostSumm : ((!(_currCode.Code == Settings.ParametersService.MTCurr())) ? (sourceRow.CostSumm * _currExChngRate.Base / _currExChngRate.Rate).RoundAs(2) : sourceRow.CurrCostSumm)));
		mTStockTakingSpecificationRow.Price = Math.Max(0m, (num / sourceRow.Quantity).RoundAs(2));
		if (mTStockTakingSpecificationRow.AccQuantity == actualQuantity)
		{
			mTStockTakingSpecificationRow.ActualSum = num;
		}
		else
		{
			mTStockTakingSpecificationRow.ActualSum = (mTStockTakingSpecificationRow.Price * actualQuantity).RoundAs(2);
		}
		mTStockTakingSpecificationRow.SalePrice = PriceHelper.GetPriceForGivenPriceListAndVATType(_priceListType.Code, "2", mTStockTakingSpecificationRow.MTID, mTStockTakingSpecificationRow.UnitCode, _date.EditValue.Value, "AMD");
		mTStockTakingSpecificationRow.SaleSum = (mTStockTakingSpecificationRow.SalePrice * mTStockTakingSpecificationRow.VarianceQuantity).RoundAs(2);
		mTStockTakingSpecificationRow.UserAdd = userAdd;
		_mtStockTakingList.Add(mTStockTakingSpecificationRow);
	}

	protected override void AddMissingAvailabilityRows()
	{
		foreach (GoodsRemainsDataProvider.DataRow item in from p in AvailabilityRows()
			where !_mtStockTakingList.Any((MTStockTakingSpecificationRow q) => q.MTID == p.MTID && q.UnitCode == p.UnitCode)
			select p)
		{
			AddNewRow(item, 0m);
		}
	}

	protected override void RefreshGoodsList()
	{
		if (string.IsNullOrWhiteSpace(_storage.Code))
		{
			return;
		}
		List<GoodsRemainsDataProvider.DataRow> list = AvailabilityRows();
		var source = from AvailabilityRow in list
			join MTStockTakingListRow in _mtStockTakingList on AvailabilityRow.MTID equals MTStockTakingListRow.MTID
			select new { AvailabilityRow, MTStockTakingListRow };
		List<int> MatchedGoodIDs = new List<int>();
		decimal num = 0m;
		for (int num2 = source.Count() - 1; num2 >= 0; num2 += -1)
		{
			var anon = source.ElementAt(num2);
			anon.MTStockTakingListRow.MTCode = anon.AvailabilityRow.MTCode;
			anon.MTStockTakingListRow.MTName = anon.AvailabilityRow.Caption;
			if (anon.AvailabilityRow.UnitCode != anon.MTStockTakingListRow.UnitCode)
			{
				MaterialCacheRow.QntUnitRow qntUnit = TradeSettings.TradeCacheService.Material.GetQntUnit(anon.AvailabilityRow.MTID, anon.AvailabilityRow.UnitCode);
				MaterialCacheRow.QntUnitRow qntUnit2 = TradeSettings.TradeCacheService.Material.GetQntUnit(anon.MTStockTakingListRow.MTID, anon.MTStockTakingListRow.UnitCode);
				if (qntUnit2 != null && qntUnit != null)
				{
					decimal coef = qntUnit2.Coef;
					decimal coef2 = qntUnit.Coef;
					anon.MTStockTakingListRow.AccQuantity = (anon.AvailabilityRow.Quantity * coef2 / coef).RoundAs(3);
				}
			}
			else
			{
				anon.MTStockTakingListRow.AccQuantity = anon.AvailabilityRow.Quantity;
			}
			if (_currCode.Code == "AMD")
			{
				num = anon.AvailabilityRow.CostSumm;
			}
			else
			{
				num = ((!(_currCode.Code == Settings.ParametersService.MTCurr())) ? (anon.AvailabilityRow.CostSumm * _currExChngRate.Base / _currExChngRate.Rate).RoundAs(2) : anon.AvailabilityRow.CurrCostSumm);
			}
			anon.MTStockTakingListRow.AccSum = num;
			MatchedGoodIDs.Add(anon.AvailabilityRow.MTID);
			list.Remove(anon.AvailabilityRow);
		}
		foreach (MTStockTakingSpecificationRow item in _mtStockTakingList.Where((MTStockTakingSpecificationRow x) => !x.UserAdd & !MatchedGoodIDs.Contains(x.MTID)))
		{
			item.AccQuantity = 0m;
			item.AccSum = 0m;
		}
		List<MTStockTakingSpecificationRow> list2 = _mtStockTakingList.Where((MTStockTakingSpecificationRow x) => !x.UserAdd & !MatchedGoodIDs.Contains(x.MTID) & (x.ActualQuantity == 0m)).ToList();
		for (int num3 = 0; num3 <= list2.Count - 1; num3++)
		{
			_mtStockTakingList.Remove(list2[num3]);
		}
		foreach (GoodsRemainsDataProvider.DataRow item2 in list)
		{
			AddNewRow(item2, 0m);
		}
	}

	protected override void LoadGoodsList()
	{
		bool flag = false;
		if (ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Trade.Common.Resources.FillZeroForActualQuantity, Settings.ProgramName, MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.Yes) == MessageBoxResult.Yes)
		{
			flag = true;
		}
		foreach (GoodsRemainsDataProvider.DataRow item in AvailabilityRows())
		{
			if (flag)
			{
				AddNewRow(item, 0m);
			}
			else
			{
				AddNewRow(item, item.Quantity);
			}
		}
	}

	protected override void SetControlsState(bool ignore_IsLoad = false)
	{
		if (_mtStockTakingList.Count > 0)
		{
			SetReadOnlyGeneralFields(isReadOnly: true);
			if (!ignore_IsLoad)
			{
				_isLoaded = true;
			}
		}
		else if (string.IsNullOrWhiteSpace(_currCode.Code))
		{
			_currCode.IsReadOnly = false;
			_currExChngRate.IsReadOnly = true;
		}
		else if (_currCode.Code != "AMD")
		{
			_currCode.IsReadOnly = false;
			_currExChngRate.IsReadOnly = false;
			_storage.IsReadOnly = false;
			_goodGroup.IsReadOnly = false;
			if (!ignore_IsLoad)
			{
				_isLoaded = false;
			}
		}
		else if (_currCode.Code == "AMD")
		{
			_currCode.IsReadOnly = false;
			_currExChngRate.IsReadOnly = true;
			_storage.IsReadOnly = false;
			_goodGroup.IsReadOnly = false;
			if (!ignore_IsLoad)
			{
				_isLoaded = false;
			}
		}
	}

	private void PriceListType_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		if (_mtStockTakingList.Count == 0 || !((string)e.NewValue != (string)e.OldValue) || string.IsNullOrWhiteSpace(_priceListType.Code) || !_priceListType.DoValidate() || ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Trade.Common.Resources.ValidateChangePrices, Settings.ProgramName, MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No) != MessageBoxResult.Yes)
		{
			return;
		}
		foreach (MTStockTakingSpecificationRow mtStockTaking in _mtStockTakingList)
		{
			mtStockTaking.SalePrice = PriceHelper.GetPriceForGivenPriceListAndVATType(_priceListType.Code, "2", mtStockTaking.MTID, mtStockTaking.UnitCode, _date.EditValue.Value, "AMD");
			mtStockTaking.SaleSum = (mtStockTaking.SalePrice * mtStockTaking.VarianceQuantity).RoundAs(2);
		}
	}

	private void MTStockTakingTableView_CellValueChanged(object sender, CellValueChangedEventArgs e)
	{
		MTStockTakingSpecificationRow mTStockTakingSpecificationRow = (MTStockTakingSpecificationRow)_mtStockTakingGrid.Grid.GetRow(e.RowHandle);
		if (mTStockTakingSpecificationRow == null)
		{
			return;
		}
		string text = string.Empty;
		if (e.OldValue != null)
		{
			text = e.OldValue.ToString();
		}
		if (e.Column == _codeCol)
		{
			if (e.Value == null)
			{
				return;
			}
			string text2 = e.Value.ToString();
			foreach (MTStockTakingSpecificationRow mtStockTaking in _mtStockTakingList)
			{
				if (mTStockTakingSpecificationRow != mtStockTaking && text2 == mtStockTaking.MTCode)
				{
					mTStockTakingSpecificationRow.MTCode = "";
					throw new ASException(string.Format(ArmSoft.SME.Trade.Common.Resources.ValidMtInventory, text2));
				}
			}
			if (!string.IsNullOrWhiteSpace(e.Value.ToString()) && text != e.Value.ToString())
			{
				MaterialCacheRow item = TradeSettings.TradeCacheService.Material.GetItem(text2, throwExceptionNotExits: true);
				List<GoodsRemainsDataProvider.DataRow> list = AvailabilityRows(item.MTCode);
				if (list.Count > 0)
				{
					mTStockTakingSpecificationRow.MTCode = list[0].MTCode;
					mTStockTakingSpecificationRow.MTID = list[0].MTID;
					mTStockTakingSpecificationRow.MTName = list[0].Caption;
					mTStockTakingSpecificationRow.UnitCode = list[0].UnitCode;
					mTStockTakingSpecificationRow.AccQuantity = list[0].Quantity;
					mTStockTakingSpecificationRow.ActualQuantity = 0m;
					if (_currCode.Code == "AMD")
					{
						mTStockTakingSpecificationRow.AccSum = list[0].CostSumm;
					}
					else if (_currCode.Code == Settings.ParametersService.MTCurr())
					{
						mTStockTakingSpecificationRow.AccSum = list[0].CurrCostSumm;
					}
					else
					{
						mTStockTakingSpecificationRow.AccSum = (list[0].CostSumm * _currExChngRate.Base / _currExChngRate.Rate).RoundAs(2);
					}
					mTStockTakingSpecificationRow.SalePrice = PriceHelper.GetPriceForGivenPriceListAndVATType(_priceListType.Code, "2", mTStockTakingSpecificationRow.MTID, mTStockTakingSpecificationRow.UnitCode, _date.EditValue.Value, "AMD");
					mTStockTakingSpecificationRow.Price = Math.Max(0m, (mTStockTakingSpecificationRow.AccSum / list[0].Quantity).RoundAs(2));
					if (mTStockTakingSpecificationRow.AccQuantity == mTStockTakingSpecificationRow.ActualQuantity)
					{
						mTStockTakingSpecificationRow.ActualSum = mTStockTakingSpecificationRow.AccSum;
					}
					else
					{
						mTStockTakingSpecificationRow.ActualSum = (mTStockTakingSpecificationRow.Price * mTStockTakingSpecificationRow.ActualQuantity).RoundAs(2);
					}
					mTStockTakingSpecificationRow.SaleSum = (mTStockTakingSpecificationRow.SalePrice * mTStockTakingSpecificationRow.VarianceQuantity).RoundAs(2);
				}
				else
				{
					mTStockTakingSpecificationRow.MTID = item.Id;
					mTStockTakingSpecificationRow.MTName = item.Caption;
					mTStockTakingSpecificationRow.UnitCode = item.Unit;
					mTStockTakingSpecificationRow.AccQuantity = 0m;
					mTStockTakingSpecificationRow.ActualQuantity = 0m;
					mTStockTakingSpecificationRow.AccSum = 0m;
					mTStockTakingSpecificationRow.SalePrice = 0m;
					mTStockTakingSpecificationRow.Price = 0m;
					mTStockTakingSpecificationRow.ActualSum = 0m;
					mTStockTakingSpecificationRow.SaleSum = 0m;
				}
				mTStockTakingSpecificationRow.UserAdd = true;
			}
			else if (string.IsNullOrWhiteSpace(e.Value.ToString()))
			{
				mTStockTakingSpecificationRow.MTID = -1;
				mTStockTakingSpecificationRow.MTName = "";
				mTStockTakingSpecificationRow.UnitCode = "";
				mTStockTakingSpecificationRow.AccQuantity = 0m;
				mTStockTakingSpecificationRow.ActualQuantity = 0m;
				mTStockTakingSpecificationRow.AccSum = 0m;
				mTStockTakingSpecificationRow.Price = 0m;
				mTStockTakingSpecificationRow.SalePrice = 0m;
				mTStockTakingSpecificationRow.ActualSum = 0m;
				mTStockTakingSpecificationRow.SaleSum = 0m;
				mTStockTakingSpecificationRow.UserAdd = true;
			}
		}
		else if (e.Column == _actualQuantityCol)
		{
			if (!(Convert.ToDecimal(e.Value) == Convert.ToDecimal(e.OldValue)) && !(Convert.ToDecimal(e.Value) < 0m))
			{
				mTStockTakingSpecificationRow.SaleSum = (mTStockTakingSpecificationRow.SalePrice * mTStockTakingSpecificationRow.VarianceQuantity).RoundAs(2);
				mTStockTakingSpecificationRow.ActualSum = (mTStockTakingSpecificationRow.Price * mTStockTakingSpecificationRow.ActualQuantity).RoundAs(2);
			}
		}
		else if (e.Column == _priceCol)
		{
			if (!(Convert.ToDecimal(e.Value) == Convert.ToDecimal(e.OldValue)) && !(Convert.ToDecimal(e.Value) <= 0m))
			{
				mTStockTakingSpecificationRow.ActualSum = (mTStockTakingSpecificationRow.Price * mTStockTakingSpecificationRow.ActualQuantity).RoundAs(2);
			}
		}
		else if (e.Column == _salePriceCol)
		{
			if (!(Convert.ToDecimal(e.Value) == Convert.ToDecimal(e.OldValue)) && !(Convert.ToDecimal(e.Value) <= 0m) && Convert.ToDecimal(e.OldValue) != Convert.ToDecimal(e.Value))
			{
				mTStockTakingSpecificationRow.SalePrice = Convert.ToDecimal(e.Value);
				mTStockTakingSpecificationRow.SaleSum = (mTStockTakingSpecificationRow.SalePrice * mTStockTakingSpecificationRow.VarianceQuantity).RoundAs(2);
			}
		}
		else if (e.Column == _actualSumCol)
		{
			if (!(Convert.ToDecimal(e.Value) == Convert.ToDecimal(e.OldValue)) && !(Convert.ToDecimal(e.Value) <= 0m) && mTStockTakingSpecificationRow.ActualQuantity > 0m)
			{
				mTStockTakingSpecificationRow.Price = (mTStockTakingSpecificationRow.ActualSum / mTStockTakingSpecificationRow.ActualQuantity).RoundAs(2);
			}
		}
		else if (e.Column == _saleSumCol && !((decimal)e.Value == (decimal)e.OldValue) && Convert.ToDecimal(e.OldValue) != Convert.ToDecimal(e.Value))
		{
			if (mTStockTakingSpecificationRow.VarianceQuantity == 0m)
			{
				mTStockTakingSpecificationRow.SaleSum = 0m;
				mTStockTakingSpecificationRow.VarianceQuantity = 1m;
			}
			else if (mTStockTakingSpecificationRow.VarianceQuantity < 0m)
			{
				mTStockTakingSpecificationRow.SaleSum = -Math.Abs(Convert.ToDecimal(e.Value));
			}
			else
			{
				mTStockTakingSpecificationRow.SaleSum = Convert.ToDecimal(e.Value);
			}
			mTStockTakingSpecificationRow.SalePrice = (mTStockTakingSpecificationRow.SaleSum / mTStockTakingSpecificationRow.VarianceQuantity).RoundAs(2);
		}
	}

	protected override void AddMTToList(decimal actualQuantity, MaterialCacheRow goodRow, MaterialCacheRow.BarCodeRow goodBarcodeRow)
	{
		for (int i = 0; i <= _mtStockTakingList.Count - 1; i++)
		{
			if (_mtStockTakingList[i].MTID == goodRow.Id && _mtStockTakingList[i].UnitCode == goodBarcodeRow.QntUnit)
			{
				_mtStockTakingList[i].ActualQuantity += actualQuantity;
				_mtStockTakingList[i].ActualSum = (_mtStockTakingList[i].Price * _mtStockTakingList[i].ActualQuantity).RoundAs(2);
				return;
			}
		}
		List<GoodsRemainsDataProvider.DataRow> list = AvailabilityRows(goodRow.MTCode);
		if (list.Count > 0)
		{
			foreach (GoodsRemainsDataProvider.DataRow item in list)
			{
				MaterialCacheRow.QntUnitRow qntUnit = TradeSettings.TradeCacheService.Material.GetQntUnit(goodRow.MTCode, goodBarcodeRow.QntUnit);
				if (qntUnit != null)
				{
					item.UnitCode = qntUnit.QntUnit;
					item.Quantity = (item.Quantity / qntUnit.Coef).RoundAs(3);
				}
				AddNewRow(item, actualQuantity);
			}
			return;
		}
		_mtStockTakingList.Add(new MTStockTakingSpecificationRow
		{
			MTCode = goodRow.MTCode,
			MTID = goodRow.Id,
			MTName = goodRow.Caption,
			UnitCode = goodBarcodeRow.QntUnit,
			AccQuantity = 0m,
			ActualQuantity = actualQuantity,
			AccSum = 0m,
			Price = 0m,
			ActualSum = 0m,
			UserAdd = false
		});
	}

	protected override void AddMTToList(MaterialCacheRow goodsRow, string unitCode = "", string unitBrief = "", decimal quantity = 1m)
	{
		for (int i = 0; i <= _mtStockTakingList.Count - 1; i++)
		{
			if (goodsRow.Id == _mtStockTakingList[i].MTID && (string.IsNullOrWhiteSpace(unitCode) ? goodsRow.Unit : unitCode) == _mtStockTakingList[i].UnitCode)
			{
				_mtStockTakingList[i].ActualQuantity += quantity;
				_mtStockTakingList[i].ActualSum += _mtStockTakingList[i].Price;
				_mtStockTakingList[i].SaleSum = (_mtStockTakingList[i].SalePrice * _mtStockTakingList[i].VarianceQuantity).RoundAs(2);
				int rowHandleByListIndex = _mtStockTakingGrid.Grid.GetRowHandleByListIndex(i);
				POWindow<MTStockTaking>.SelectCellInGrid(_mtStockTakingTableView, _mtStockTakingGrid, rowHandleByListIndex, "ActualQuantity");
				return;
			}
		}
		List<GoodsRemainsDataProvider.DataRow> list = ((!(unitCode != goodsRow.Unit) || !(unitCode != goodsRow.AltUnit)) ? AvailabilityRows(goodsRow.MTCode, !string.IsNullOrWhiteSpace(unitCode) && goodsRow.Unit != unitCode) : AvailabilityRows(goodsRow.MTCode));
		if (list.Count > 0)
		{
			foreach (GoodsRemainsDataProvider.DataRow item in list)
			{
				if (unitCode != goodsRow.Unit && unitCode != goodsRow.AltUnit)
				{
					MaterialCacheRow.QntUnitRow qntUnit = TradeSettings.TradeCacheService.Material.GetQntUnit(goodsRow.MTCode, unitCode);
					if (qntUnit != null && qntUnit.Coef != 0m)
					{
						item.Quantity = (item.Quantity / qntUnit.Coef).RoundAs(3);
					}
				}
				item.UnitCode = (string.IsNullOrWhiteSpace(unitCode) ? goodsRow.Unit : unitCode);
				item.UnitBrief = (string.IsNullOrWhiteSpace(unitBrief) ? goodsRow.UnitRow.Brief : unitBrief);
				AddNewRow(item, quantity, userAdd: true);
			}
			return;
		}
		AddMTToListNewRow(goodsRow, string.IsNullOrWhiteSpace(unitCode) ? goodsRow.Unit : unitCode, quantity);
	}

	protected override void AddMTToListNewRow(MaterialCacheRow goodsRow, string unitCode, decimal quantity = 1m)
	{
		MTStockTakingSpecificationRow mTStockTakingSpecificationRow = new MTStockTakingSpecificationRow
		{
			UnitCode = unitCode,
			AccQuantity = 0m,
			ActualQuantity = quantity,
			AccSum = 0m,
			Price = 0m,
			ActualSum = 0m,
			UserAdd = true
		};
		if (goodsRow != null)
		{
			mTStockTakingSpecificationRow.MTID = goodsRow.Id;
			mTStockTakingSpecificationRow.MTCode = goodsRow.MTCode;
			mTStockTakingSpecificationRow.MTName = goodsRow.Caption;
		}
		mTStockTakingSpecificationRow.SalePrice = TradeSettings.CreateTradeClient().CommonServices.GetPriceForGivenPriceListAndVATType(_priceListType.Code, "2", mTStockTakingSpecificationRow.MTID, mTStockTakingSpecificationRow.UnitCode, _date.EditValue.Value, "AMD");
		mTStockTakingSpecificationRow.SaleSum = (mTStockTakingSpecificationRow.SalePrice * mTStockTakingSpecificationRow.VarianceQuantity).RoundAs(2);
		_mtStockTakingList.Add(mTStockTakingSpecificationRow);
		int rowHandleByListIndex = _mtStockTakingGrid.Grid.GetRowHandleByListIndex(_mtStockTakingList.Count - 1);
		POWindow<MTStockTaking>.SelectCellInGrid(_mtStockTakingTableView, _mtStockTakingGrid, rowHandleByListIndex, "ActualQuantity");
		_mtStockTakingGrid.ConfigureButtons();
	}

	private void FilteredSearch_Click(object sender, RoutedEventArgs e)
	{
		ValidateDocFields();
		FilteredSearchWindow filteredSearchWindow = new FilteredSearchWindow(new FilteredSearch
		{
			FilterDataView = new GoodsGroups(showAsTree: true, filteredSearch: true),
			SearchDataView = new GoodsFilter(shortMode: true, ItemTypesSE.Goods.Value),
			ResultsList = new MTStockTakingFilteredSearchList()
		}, this, isBasketMode: true);
		if (filteredSearchWindow.ShowDialog().Value)
		{
			foreach (MTStockTakingSpecificationRow item in filteredSearchWindow.BasketResult)
			{
				List<GoodsRemainsDataProvider.DataRow> list = AvailabilityRows(item.MTCode);
				if (list.Count > 0)
				{
					item.AccQuantity = list[0].Quantity;
					if (_currCode.Code == "AMD")
					{
						item.AccSum = list[0].CostSumm;
					}
					else if (_currCode.Code == Settings.ParametersService.MTCurr())
					{
						item.AccSum = list[0].CurrCostSumm;
					}
					else
					{
						item.AccSum = (list[0].CostSumm * _currExChngRate.Base / _currExChngRate.Rate).RoundAs(2);
					}
					item.Price = (item.AccSum / list[0].Quantity).RoundAs(2);
					item.SalePrice = PriceHelper.GetPriceForGivenPriceListAndVATType(_priceListType.Code, "2", item.MTID, item.UnitCode, _date.EditValue.Value, "AMD");
					if (item.AccQuantity == item.ActualQuantity)
					{
						item.ActualSum = item.AccSum;
					}
					else
					{
						item.ActualSum = (item.Price * item.ActualQuantity).RoundAs(2);
					}
					item.SaleSum = (item.SalePrice * item.VarianceQuantity).RoundAs(2);
				}
				_mtStockTakingList.Add(item);
			}
		}
		if (_mtStockTakingList.Count > 0)
		{
			SetReadOnlyGeneralFields(isReadOnly: true);
		}
	}
}
