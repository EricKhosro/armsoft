using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Ardalis.SmartEnum;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Constants;
using ArmSoft.SME.Common.Extensions;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Core.UI.Controls;
using ArmSoft.SME.Core.UI.Documents;
using ArmSoft.SME.Core.UI.DropDowns;
using ArmSoft.SME.Core.UI.Enums;
using ArmSoft.SME.Core.UI.PersistableObject;
using ArmSoft.SME.Trade.Client.Data.DropDowns;
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

[DocumentWindow(22)]
public class MTStockTakingRetailWindow : MTStockTakingWindowBase<MTStockTakingRetail>
{
	public MTStockTakingRetailWindow()
	{
	}

	public MTStockTakingRetailWindow(MTStockTakingRetail doc)
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
		_storage = AddDropDownControl(null, Definition.Fields["Storage"], new Storages(true));
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
		_currExChngRate = AddExchangeRateControl(container3, Definition.Fields["ExchangeRate"]);
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
		_mtStockTakingTableView.CellValueChanged += new SafeDocumentEventHandler<CellValueChangedEventArgs>(MTStockTakingRetailTableView_CellValueChanged, this).Handler;
		_mtStockTakingTableView.FocusedRowHandleChanged += new SafeDocumentEventHandler<FocusedRowHandleChangedEventArgs>(MTStockTakingTableView_FocusedRowHandleChanged, this).Handler;
		_loadGoodsList = _mtStockTakingGrid.AddButton("fill_down_16.svg", ArmSoft.SME.Common.Resources.ButtonLoad, ArmSoft.SME.Common.Resources.ButtonLoad, 90.0);
		_loadGoodsList.Click += new SafeDocumentEventHandler<RoutedEventArgs>(LoadGoodsList_Click, this).Handler;
		_refreshGoodsList = _mtStockTakingGrid.AddButton("refresh_16.svg", ArmSoft.SME.Common.Resources.Refresh, ArmSoft.SME.Common.Resources.Refresh, 110.0);
		_refreshGoodsList.Click += new SafeDocumentEventHandler<RoutedEventArgs>(RefreshGoodsList_Click, this).Handler;
		_filteredSearch = _mtStockTakingGrid.AddButton(null, ArmSoft.SME.Trade.Common.Resources.GroupAddition, ArmSoft.SME.Trade.Common.Resources.GroupAddition, 130.0);
		_filteredSearch.Click += new SafeDocumentEventHandler<RoutedEventArgs>(FilteredSearch_Click, this).Handler;
		_mtStockTakingGrid.AddRowIndicator(35);
		_codeCol = _mtStockTakingGrid.AddDropDownColumn(Definition.Grids["MTList"].Columns["MTCode"], new ArmSoft.SME.Trade.UI.DropDown.GoodsFilter(shortMode: true, ItemTypesSE.Goods.Value, "", isShowOnlyInPriceList: true), allowAdditionButtons: true);
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
		_actualSumCol = _mtStockTakingGrid.AddNumberColumn(Definition.Grids["MTList"].Columns["ActualSum"], showTotal: true);
		_mtStockTakingList = new ObservableCollection<MTStockTakingSpecificationRow>();
		foreach (GridColumn column in _mtStockTakingGrid.Grid.Columns)
		{
			column.AllowSorting = DefaultBoolean.True;
			column.AllowColumnFiltering = DefaultBoolean.True;
			column.FilterPopupMode = FilterPopupMode.CheckedList;
		}
	}

	protected override void CreateChildDocument(short childDocType, MTStockTakingRetail baseDoc)
	{
		base.CreateChildDocument(childDocType, baseDoc);
		CreateBasedOnDocHelper.CreateDocument((DocumentTypeSE)(SmartEnum<DocumentTypeSE, short>)childDocType, PO.ISN, DVPID);
	}

	protected override void OnNew()
	{
		base.OnNew();
		_date.EditValue = Settings.HostingEnvironment.CurrentDate;
		_owner.Code = Settings.LoginService.UserName;
		string text = Settings.ParametersService.DefaultStorage();
		if (_viewMode != WindowViewMode.Copy && !string.IsNullOrWhiteSpace(text))
		{
			StoragesCacheRow item = TradeSettings.TradeCacheService.Storage.GetItem(text);
			if (item != null && item.Retail && TradeSettings.CreateTradeClient().CommonServices.IsStorageAvailable(item.Code))
			{
				_storage.Code = text;
			}
		}
		else if (_viewMode == WindowViewMode.Copy)
		{
			SetReadOnlyGeneralFields(isReadOnly: true);
		}
	}

	protected override void OnNewAfterSave()
	{
		PO = new MTStockTakingRetail();
		base.OnNewAfterSave();
		_mtStockTakingList.Clear();
		SetReadOnlyGeneralFields(isReadOnly: false);
		_owner.Code = Settings.LoginService.UserName;
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
			SetReadOnlyGeneralFields(isReadOnly: true);
			_isLoaded = _mtStockTakingList.Any((MTStockTakingSpecificationRow x) => !x.UserAdd);
		}
		_setFocusOnActivation = _docNum;
	}

	private List<GoodsRetailAvailabilityDataProvider.DataRow> AvailabilityRows(string goodCode = "", bool showAltUnit = false)
	{
		GoodsRetailAvailabilityDataProvider goodsRetailAvailabilityDataProvider = new GoodsRetailAvailabilityDataProvider(TradeSettings.CreateTradeClient());
		GoodsRetailAvailabilityDataProvider.Param param = new GoodsRetailAvailabilityDataProvider.Param
		{
			RemDate = _date.EditValue.Value,
			Storage = _storage.Code,
			GoodGroup = _goodGroup.Code,
			ShowAltUnit = showAltUnit
		};
		if (!string.IsNullOrWhiteSpace(goodCode))
		{
			param.GoodCode = goodCode;
		}
		return goodsRetailAvailabilityDataProvider.GetRows(param);
	}

	private void AddNewRow(GoodsRetailAvailabilityDataProvider.DataRow sourceRow, decimal actualQuantity, bool userAdd = false)
	{
		MTStockTakingSpecificationRow mTStockTakingSpecificationRow = new MTStockTakingSpecificationRow
		{
			MTCode = sourceRow.GoodCode,
			MTID = sourceRow.GoodID,
			MTName = sourceRow.Caption,
			UnitCode = sourceRow.Unit,
			AccQuantity = sourceRow.Quantity,
			ActualQuantity = actualQuantity,
			AccSum = sourceRow.Sum,
			Price = sourceRow.Price
		};
		if (mTStockTakingSpecificationRow.AccQuantity == actualQuantity)
		{
			mTStockTakingSpecificationRow.ActualSum = mTStockTakingSpecificationRow.AccSum;
		}
		else
		{
			mTStockTakingSpecificationRow.ActualSum = (mTStockTakingSpecificationRow.Price * actualQuantity).RoundAs(2);
		}
		mTStockTakingSpecificationRow.UserAdd = userAdd;
		_mtStockTakingList.Add(mTStockTakingSpecificationRow);
	}

	protected override void AddMissingAvailabilityRows()
	{
		foreach (GoodsRetailAvailabilityDataProvider.DataRow item in from p in AvailabilityRows()
			where !_mtStockTakingList.Any((MTStockTakingSpecificationRow q) => q.MTID == p.GoodID && q.UnitCode == p.Unit)
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
		List<GoodsRetailAvailabilityDataProvider.DataRow> list = AvailabilityRows();
		HashSet<int> matchedGoodIDs = new HashSet<int>();
		MaterialCacheRow.QntUnitRow qntUnitRow;
		foreach (MTStockTakingSpecificationRow mtStockTakingRow in _mtStockTakingList)
		{
			qntUnitRow = TradeSettings.TradeCacheService.Material.GetQntUnit(mtStockTakingRow.MTID, mtStockTakingRow.UnitCode);
			GoodsRetailAvailabilityDataProvider.DataRow dataRow = list.FirstOrDefault((GoodsRetailAvailabilityDataProvider.DataRow p) => mtStockTakingRow.MTID == p.GoodID && (mtStockTakingRow.Price / qntUnitRow.Coef).RoundAs(2) == p.Price);
			if (dataRow != null)
			{
				mtStockTakingRow.MTCode = dataRow.GoodCode;
				mtStockTakingRow.MTName = dataRow.Caption;
				mtStockTakingRow.AccQuantity = (dataRow.Quantity / qntUnitRow.Coef).RoundAs(3);
				mtStockTakingRow.AccSum = dataRow.Sum;
				matchedGoodIDs.Add(mtStockTakingRow.MTID);
				list.Remove(dataRow);
			}
		}
		foreach (MTStockTakingSpecificationRow item in _mtStockTakingList.Where((MTStockTakingSpecificationRow x) => !x.UserAdd & !matchedGoodIDs.Contains(x.MTID)))
		{
			item.AccQuantity = 0m;
			item.AccSum = 0m;
		}
		List<MTStockTakingSpecificationRow> list2 = new List<MTStockTakingSpecificationRow>();
		foreach (MTStockTakingSpecificationRow item2 in _mtStockTakingList.Where((MTStockTakingSpecificationRow x) => !x.UserAdd & !matchedGoodIDs.Contains(x.MTID) & (x.ActualQuantity == 0m)))
		{
			list2.Add(item2);
		}
		for (int num = 0; num <= list2.Count - 1; num++)
		{
			_mtStockTakingList.Remove(list2[num]);
		}
		foreach (GoodsRetailAvailabilityDataProvider.DataRow item3 in list)
		{
			AddNewRow(item3, 0m);
		}
	}

	protected override void LoadGoodsList()
	{
		bool flag = false;
		if (ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Trade.Common.Resources.FillZeroForActualQuantity, Settings.ProgramName, MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.Yes) == MessageBoxResult.Yes)
		{
			flag = true;
		}
		foreach (GoodsRetailAvailabilityDataProvider.DataRow item in AvailabilityRows())
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
			return;
		}
		_storage.IsReadOnly = false;
		_goodGroup.IsReadOnly = false;
		if (!ignore_IsLoad)
		{
			_isLoaded = false;
		}
	}

	protected override void SetReadOnlyGeneralFields(bool isReadOnly)
	{
		base.SetReadOnlyGeneralFields(isReadOnly);
		_currCode.IsReadOnly = true;
		_currExChngRate.IsReadOnly = true;
	}

	private void MTStockTakingRetailTableView_CellValueChanged(object sender, CellValueChangedEventArgs e)
	{
		MTStockTakingSpecificationRow currentRow = (MTStockTakingSpecificationRow)_mtStockTakingGrid.Grid.GetRow(e.RowHandle);
		if (currentRow == null)
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
			if (e.Value != null)
			{
				string key = e.Value.ToString();
				MaterialCacheRow materialCacheRow = null;
				if (!string.IsNullOrWhiteSpace(e.Value.ToString()) && text != e.Value.ToString())
				{
					materialCacheRow = TradeSettings.TradeCacheService.Material.GetItem(key, throwExceptionNotExits: true);
					List<GoodsRetailAvailabilityDataProvider.DataRow> list = AvailabilityRows(materialCacheRow.MTCode);
					if (list.Count > 0)
					{
						currentRow.MTCode = list[0].GoodCode;
						currentRow.MTID = list[0].GoodID;
						currentRow.MTName = list[0].Caption;
						currentRow.UnitCode = list[0].Unit;
						currentRow.AccQuantity = list[0].Quantity;
						currentRow.AccQuantity = 0m;
						currentRow.AccSum = list[0].Sum;
						currentRow.Price = Math.Max(0m, (currentRow.AccSum / list[0].Quantity).RoundAs(2));
						foreach (MTStockTakingSpecificationRow mtStockTaking in _mtStockTakingList)
						{
							if (currentRow != mtStockTaking && currentRow.MTID == mtStockTaking.MTID && ((currentRow.UnitCode == mtStockTaking.UnitCode && currentRow.Price == mtStockTaking.Price) || (currentRow.UnitCode != mtStockTaking.UnitCode && SamePricesForUnit(currentRow.UnitCode, mtStockTaking.UnitCode, currentRow.Price, mtStockTaking.Price, mtStockTaking.MTID))))
							{
								currentRow.Price = 0m;
								return;
							}
						}
						if (currentRow.AccQuantity == currentRow.ActualQuantity)
						{
							currentRow.ActualSum = currentRow.AccSum;
						}
						else
						{
							currentRow.ActualSum = (currentRow.Price * currentRow.ActualQuantity).RoundAs(2);
						}
					}
					else
					{
						currentRow.MTID = materialCacheRow.Id;
						currentRow.MTName = materialCacheRow.Caption;
						currentRow.UnitCode = materialCacheRow.Unit;
						currentRow.AccQuantity = 0m;
						currentRow.ActualQuantity = 0m;
						currentRow.AccSum = 0m;
						currentRow.Price = 0m;
						currentRow.ActualSum = 0m;
					}
					currentRow.UserAdd = true;
				}
				else if (string.IsNullOrWhiteSpace(e.Value.ToString()))
				{
					currentRow.MTID = -1;
					currentRow.MTName = "";
					currentRow.UnitCode = "";
					currentRow.AccQuantity = 0m;
					currentRow.ActualQuantity = 0m;
					currentRow.AccSum = 0m;
					currentRow.Price = 0m;
					currentRow.ActualSum = 0m;
					currentRow.UserAdd = true;
				}
			}
		}
		else if (e.Column == _actualQuantityCol)
		{
			if (Convert.ToDecimal(e.Value) == Convert.ToDecimal(e.OldValue) || Convert.ToDecimal(e.Value) < 0m)
			{
				return;
			}
			currentRow.ActualSum = (currentRow.Price * currentRow.ActualQuantity).RoundAs(2);
		}
		else if (e.Column == _priceCol)
		{
			if (Convert.ToDecimal(e.Value) == Convert.ToDecimal(e.OldValue) || Convert.ToDecimal(e.Value) <= 0m)
			{
				return;
			}
			foreach (MTStockTakingSpecificationRow mtStockTaking2 in _mtStockTakingList)
			{
				if (currentRow != mtStockTaking2 && currentRow.MTID == mtStockTaking2.MTID && ((currentRow.UnitCode == mtStockTaking2.UnitCode && currentRow.Price == mtStockTaking2.Price) || (currentRow.UnitCode != mtStockTaking2.UnitCode && SamePricesForUnit(currentRow.UnitCode, mtStockTaking2.UnitCode, currentRow.Price, mtStockTaking2.Price, mtStockTaking2.MTID))))
				{
					decimal price = currentRow.Price;
					currentRow.Price = 0m;
					string validMtInventoryRetail = ArmSoft.SME.Trade.Common.Resources.ValidMtInventoryRetail2;
					InlineArray5<object> buffer = default;
					buffer[0] = mtStockTaking2.MTCode;
					buffer[1] = mtStockTaking2.UnitCode;
					buffer[2] = mtStockTaking2.Price.ToString("n2");
					buffer[3] = currentRow.UnitCode;
					buffer[4] = price.ToString("n2");
					throw new ASException(string.Format(validMtInventoryRetail, (ReadOnlySpan<object?>)buffer));
				}
			}
			MaterialCacheRow.QntUnitRow qntUnit = TradeSettings.TradeCacheService.Material.GetQntUnit(currentRow.MTID, currentRow.UnitCode);
			if (qntUnit != null)
			{
				MTRetailQtyAndPriceDataProvider.Param param = new MTRetailQtyAndPriceDataProvider.Param
				{
					MTCode = currentRow.MTCode,
					UnitCoef = qntUnit.Coef,
					Storage = _storage.Code,
					Date = _date.EditValue
				};
				MTRetailQtyAndPriceDataProvider.DataRow dataRow = new MTRetailQtyAndPriceDataProvider(TradeSettings.CreateTradeClient()).GetRows(param).FirstOrDefault((MTRetailQtyAndPriceDataProvider.DataRow x) => x.Price == currentRow.Price);
				if (dataRow != null)
				{
					currentRow.AccQuantity = dataRow.Quantity;
				}
				else
				{
					currentRow.AccQuantity = 0m;
					currentRow.AccSum = 0m;
				}
			}
			currentRow.ActualSum = (currentRow.Price * currentRow.ActualQuantity).RoundAs(2);
		}
		else if (e.Column == _actualSumCol)
		{
			if (Convert.ToDecimal(e.Value) == Convert.ToDecimal(e.OldValue) || Convert.ToDecimal(e.Value) <= 0m)
			{
				return;
			}
			if (currentRow.ActualQuantity > 0m)
			{
				currentRow.Price = (currentRow.ActualSum / currentRow.ActualQuantity).RoundAs(2);
			}
		}
		_mtStockTakingGrid.Grid.RefreshRow(e.RowHandle);
	}

	protected override void AddMTToList(decimal actualQuantity, MaterialCacheRow goodRow, MaterialCacheRow.BarCodeRow goodBarcodeRow)
	{
		foreach (MTStockTakingSpecificationRow mtStockTaking in _mtStockTakingList)
		{
			if (mtStockTaking.MTID == goodRow.Id && mtStockTaking.UnitCode == goodBarcodeRow.QntUnit)
			{
				mtStockTaking.ActualQuantity += actualQuantity;
				mtStockTaking.ActualSum = (mtStockTaking.Price * mtStockTaking.ActualQuantity).RoundAs(2);
				return;
			}
		}
		List<GoodsRetailAvailabilityDataProvider.DataRow> list = AvailabilityRows(goodRow.MTCode);
		if (list.Count > 0)
		{
			MaterialCacheRow.QntUnitRow qntUnit = TradeSettings.TradeCacheService.Material.GetQntUnit(goodRow.Id, goodBarcodeRow.QntUnit);
			if (qntUnit != null)
			{
				list[0].Unit = qntUnit.QntUnit;
				list[0].Quantity = (list[0].Quantity / qntUnit.Coef).RoundAs(3);
			}
			AddNewRow(list[0], actualQuantity);
			for (int i = 1; i <= list.Count - 1; i++)
			{
				if (qntUnit != null)
				{
					list[i].Unit = qntUnit.QntUnit;
					list[i].Quantity = (list[i].Quantity / qntUnit.Coef).RoundAs(3);
				}
				AddNewRow(list[i], 0m);
			}
		}
		else
		{
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
	}

	protected override void AddMTToList(MaterialCacheRow goodsRow, string unitCode = "", string unitBrief = "", decimal quantity = 1m)
	{
		for (int i = 0; i <= _mtStockTakingList.Count - 1; i++)
		{
			if (goodsRow.Id == _mtStockTakingList[i].MTID && (string.IsNullOrWhiteSpace(unitCode) ? goodsRow.Unit : unitCode) == _mtStockTakingList[i].UnitCode)
			{
				_mtStockTakingList[i].ActualQuantity += quantity;
				_mtStockTakingList[i].ActualSum += _mtStockTakingList[i].Price;
				int rowHandleByListIndex = _mtStockTakingGrid.Grid.GetRowHandleByListIndex(i);
				POWindow<MTStockTakingRetail>.SelectCellInGrid(_mtStockTakingTableView, _mtStockTakingGrid, rowHandleByListIndex, "ActualQuantity");
				return;
			}
		}
		List<GoodsRetailAvailabilityDataProvider.DataRow> list = AvailabilityRows(goodsRow.MTCode, !string.IsNullOrWhiteSpace(unitCode) && goodsRow.Unit != unitCode);
		if (list.Count > 0)
		{
			foreach (GoodsRetailAvailabilityDataProvider.DataRow item in list)
			{
				item.Unit = (string.IsNullOrWhiteSpace(unitCode) ? goodsRow.Unit : unitCode);
				AddNewRow(item, 1m, userAdd: true);
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
		_mtStockTakingList.Add(mTStockTakingSpecificationRow);
		int rowHandleByListIndex = _mtStockTakingGrid.Grid.GetRowHandleByListIndex(_mtStockTakingList.Count - 1);
		POWindow<MTStockTakingRetail>.SelectCellInGrid(_mtStockTakingTableView, _mtStockTakingGrid, rowHandleByListIndex, "ActualQuantity");
		_mtStockTakingGrid.ConfigureButtons();
	}

	private static bool SamePricesForUnit(string unit, string oldUnit, decimal price, decimal oldPrice, int mtID)
	{
		MaterialCacheRow.QntUnitRow qntUnit = TradeSettings.TradeCacheService.Material.GetQntUnit(mtID, unit);
		MaterialCacheRow.QntUnitRow qntUnit2 = TradeSettings.TradeCacheService.Material.GetQntUnit(mtID, oldUnit);
		if (qntUnit != null && qntUnit2 != null)
		{
			decimal coef = qntUnit.Coef;
			decimal coef2 = qntUnit2.Coef;
			if ((price / coef).RoundAs(2) == (oldPrice / coef2).RoundAs(2))
			{
				return true;
			}
			return false;
		}
		return true;
	}

	private void FilteredSearch_Click(object sender, RoutedEventArgs e)
	{
		ValidateDocFields();
		FilteredSearchWindow filteredSearchWindow = new FilteredSearchWindow(new FilteredSearch
		{
			FilterDataView = new GoodsGroups(showAsTree: true, filteredSearch: true),
			SearchDataView = new ArmSoft.SME.Trade.UI.DropDown.GoodsFilter(shortMode: true, ItemTypesSE.Goods.Value),
			ResultsList = new MTStockTakingFilteredSearchList()
		}, this, isBasketMode: true);
		if (!filteredSearchWindow.ShowDialog().Value)
		{
			return;
		}
		foreach (MTStockTakingSpecificationRow item in filteredSearchWindow.BasketResult)
		{
			List<GoodsRetailAvailabilityDataProvider.DataRow> list = AvailabilityRows(item.MTCode);
			if (list.Count > 0)
			{
				item.AccQuantity = list[0].Quantity;
				item.AccSum = list[0].Sum;
				item.Price = (item.AccSum / list[0].Quantity).RoundAs(2);
				if (item.AccQuantity == item.ActualQuantity)
				{
					item.ActualSum = item.AccSum;
				}
				else
				{
					item.ActualSum = (item.Price * item.ActualQuantity).RoundAs(2);
				}
			}
			_mtStockTakingList.Add(item);
		}
	}
}
