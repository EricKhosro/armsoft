using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Extensions;
using ArmSoft.SME.Common.Models.Documents;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Core.UI.Controls;
using ArmSoft.SME.Core.UI.DataViews;
using ArmSoft.SME.Core.UI.PersistableObject;
using ArmSoft.SME.Trade.Client.Data.Directories;
using ArmSoft.SME.Trade.Client.Data.DropDowns;
using ArmSoft.SME.Trade.Common;
using ArmSoft.SME.Trade.Common.Models.Caches;
using ArmSoft.SME.Trade.Common.Models.Documents;
using ArmSoft.SME.Trade.Common.SmartEnums;
using ArmSoft.SME.Trade.UI.Documents.Goods;
using ArmSoft.SME.Trade.UI.DropDown;
using DevExpress.Xpf.Editors;

namespace ArmSoft.SME.Trade.UI.CustomUIElements;

public class FillComplectRowsWindow<T> : DialogWindow where T : ISaleRow, new()
{
	private readonly DocumentGridControl _grid;

	private readonly ObservableCollection<T> _mtMTInvoiceBaseList;

	private DropDownControl _storage;

	private DropDownControl _goodGroup;

	private DropDownControl _goodCode;

	private DropDownControl _qntUnits;

	private PopupCalcEditExt _quantity;

	private PopupCalcEditExt _discountPercent;

	private int _maxRowId;

	private readonly Document _doc;

	private readonly string _storageField;

	private readonly string _vatType;

	private readonly string _priceType;

	private readonly string _curCode;

	private readonly DateOnly _docDate;

	private readonly bool _isUseDiscount;

	private readonly bool _calcDiscountAuto;

	private readonly decimal _discPercent;

	private readonly decimal _useDiscount;

	private readonly IPOWindowInfo _window;

	public int AddedRows { get; set; }

	public FillComplectRowsWindow(IPOWindow window, MTInvoiceBase doc, DateOnly docDate, string storage, bool isUseDiscount, bool isCalcDiscountAuto, decimal useDiscount, decimal discountPercent, int maxRowId, ObservableCollection<T> mtBillList, DocumentGridControl mtInvoiceGrid, string priceType = "", string vatType = "", string curCode = "")
		: base((Window)window)
	{
		_doc = doc;
		_docDate = docDate;
		_storageField = storage;
		_priceType = priceType;
		_vatType = vatType;
		_curCode = curCode;
		_isUseDiscount = isUseDiscount;
		_calcDiscountAuto = isCalcDiscountAuto;
		_useDiscount = useDiscount;
		_maxRowId = maxRowId;
		_mtMTInvoiceBaseList = mtBillList;
		_grid = mtInvoiceGrid;
		_discPercent = discountPercent;
		_window = (IPOWindowInfo)window;
		InnerInitialize();
	}

	private void InnerInitialize()
	{
		AddedRows = 0;
		Mode = DialogWindowMode.NewModeWithNext;
		Caption = ArmSoft.SME.Common.Resources.Complect;
		Name = "Complect";
		HelpId = HelpId;
		WindowValidation += ComplectsDialog_WindowValidation;
		if (_doc.POType == DocumentTypeSE.MTInvoice.Value)
		{
			_storage = AddDropDownControl("_storage", ArmSoft.SME.Common.Resources.Storage, new Storages(false), isRequired: true);
			_storage.Code = _storageField;
		}
		_goodGroup = AddDropDownControl("_goodGroup", ArmSoft.SME.Common.Resources.Group, new GoodsGroups());
		_goodCode = AddDropDownControl("_goodCode", ArmSoft.SME.Trade.Common.Resources.MaterialValue, new Complects(), isRequired: true, allowAdditionButtons: false, storeValue: false, isShowComment: true, 8);
		_goodCode.ClickDropDown += new SafeEventHandler<DropDownControlEventArgs>(GoodCode_ClickDropDown).Handler;
		_goodCode.EditValueChanged += new SafeEventHandler<EditValueChangedEventArgs>(GoodCode_EditValueChanged).Handler;
		_qntUnits = AddDropDownControl("_qntUnits", ArmSoft.SME.Trade.Common.Resources.QntUnit, new MTQntUnits("", isShowQntUnitCaption: false, isShowOnlyComplects: true), isRequired: true);
		_qntUnits.ClickDropDown += new SafeEventHandler<DropDownControlEventArgs>(QntUnits_ClickDropDown).Handler;
		_quantity = AddNumericControl("_quantity", ArmSoft.SME.Common.Resources.Quantity, 13, 3, isRequired: true);
		if ((_isUseDiscount && !_calcDiscountAuto) || _useDiscount != 0m)
		{
			_discountPercent = AddNumericControl("_discountPercent", ArmSoft.SME.Common.Resources.DiscountPercent, 6, 2, isRequired: false, allowNegativeNumbers: true);
			_discountPercent.Value = _discPercent;
		}
		SetFocusOnActivation = _goodGroup;
	}

	private void GoodCode_ClickDropDown(object sender, DropDownControlEventArgs e)
	{
		Complects complects = new Complects(_goodGroup.DoValidate() ? _goodGroup.Code : "");
		ModalBrowser modalBrowser = new ModalBrowser(complects)
		{
			LookUpValue = _goodCode.Code,
			Owner = e.Owner,
			Left = e.Left,
			Top = e.Top
		};
		if (modalBrowser.Show())
		{
			_goodCode.Code = complects.ValueMember(modalBrowser.SelectedRowIndex);
		}
		e.Cancel = true;
	}

	private void GoodCode_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		if (!string.IsNullOrWhiteSpace(_goodCode.Code) && _goodCode.DoValidate())
		{
			MaterialCacheRow item = TradeSettings.TradeCacheService.Material.GetItem(_goodCode.Code);
			if (item != null)
			{
				_qntUnits.DataView = new MTQntUnits(item.MTCode);
				_qntUnits.Code = item.Unit;
				if (_discountPercent != null)
				{
					_discountPercent.Value = item.Discount;
				}
			}
		}
		else
		{
			_qntUnits.DataView = new MTQntUnits(null, isShowQntUnitCaption: false, isShowOnlyComplects: true, _goodGroup.DoValidate() ? _goodGroup.Code : "");
		}
		if (!_qntUnits.DoValidate() || string.IsNullOrWhiteSpace(_goodCode.Code))
		{
			_qntUnits.Code = "";
		}
	}

	private void QntUnits_ClickDropDown(object sender, DropDownControlEventArgs e)
	{
		if (!string.IsNullOrWhiteSpace(_goodCode.Code) && _goodCode.DoValidate())
		{
			return;
		}
		MTQntUnits mTQntUnits = new MTQntUnits(string.Empty, isShowQntUnitCaption: false, isShowOnlyComplects: true, _goodGroup.DoValidate() ? _goodGroup.Code : "");
		ModalBrowser modalBrowser = new ModalBrowser(mTQntUnits)
		{
			Owner = e.Owner,
			Left = e.Left,
			Top = e.Top
		};
		if (modalBrowser.Show())
		{
			MTQntUnitsDataProvider.DataRow.MTQntUnitsRowKey xKey = (MTQntUnitsDataProvider.DataRow.MTQntUnitsRowKey)mTQntUnits.KeyMember(modalBrowser.SelectedRowIndex);
			if (xKey != null)
			{
				IReadOnlyList<MaterialCacheRow> list = TradeSettings.TradeCacheService.Material.GetList((MaterialCacheRow x) => x.Id == xKey.MTID);
				if (list.Count > 0)
				{
					_goodCode.Code = list[0].MTCode;
				}
			}
			_qntUnits.Code = mTQntUnits.ValueMember(modalBrowser.SelectedRowIndex);
		}
		e.Cancel = true;
	}

	private void ComplectsDialog_WindowValidation(DialogWindow window, DialogWindowValidationArgs args)
	{
		try
		{
			FillComplectRows();
			_goodCode.Code = "";
			_qntUnits.Code = "";
			_quantity.EditValue = 0m;
			if (_discountPercent != null)
			{
				_discountPercent.EditValue = 0m;
			}
			_goodCode.InnerCodeEdit.Focus();
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
			args.isValid = false;
			args.SetFocusWhenInvalidSave = _goodCode;
		}
	}

	private void FillComplectRows()
	{
		DocumentGridAddingRowEventArgs e = new DocumentGridAddingRowEventArgs(_grid);
		MaterialCacheRow item = TradeSettings.TradeCacheService.Material.GetItem(_goodCode.Code);
		bool flag = TradeSettings.TradeCacheService.Storage.GetItem(_storageField)?.Retail ?? false;
		decimal value = _quantity.Value;
		if (item == null)
		{
			return;
		}
		MaterialCacheRow.QntUnitRow qntUnit = TradeSettings.TradeCacheService.Material.GetQntUnit(item.MTCode, _qntUnits.Code);
		if (qntUnit != null)
		{
			value *= qntUnit.Coef;
		}
		List<MTComplectsPartsDataProvider.DataRow> rows = new MTComplectsPartsDataProvider(TradeSettings.CreateTradeClient()).GetRows(new MTComplectsPartsDataProvider.Param
		{
			ComplectCode = item.MTCode
		});
		if (rows == null || rows.Count == 0)
		{
			return;
		}
		foreach (MTComplectsPartsDataProvider.DataRow item3 in rows)
		{
			T item2 = ((!(typeof(T) == typeof(MTInvoiceWindow.MTInvoiceSpecificationRow)) && !(typeof(T) == typeof(MTInvoiceRetailWindow.MTInvoiceRetailSpecificationRow))) ? new T() : ((T)Activator.CreateInstance(typeof(T), _window)));
			ref T reference = ref item2;
			T val = default;
			if (val == null)
			{
				val = reference;
				reference = ref val;
			}
			string itemType = item3.ItemType;
			reference.ItemType = itemType;
			if (item2.ItemType != ItemTypesSE.Services.Value)
			{
				if (_doc.POType == DocumentTypeSE.MTInvoice.Value)
				{
					ref T reference2 = ref item2;
					val = default;
					if (val == null)
					{
						val = reference2;
						reference2 = ref val;
					}
					string code = _storage.Code;
					reference2.Storage = code;
				}
				else
				{
					string storageField = _storageField;
					item2.Storage = storageField;
				}
			}
			ref T reference3 = ref item2;
			val = default;
			if (val == null)
			{
				val = reference3;
				reference3 = ref val;
			}
			string mTSourceCode = item3.MTSourceCode;
			reference3.ItemCode = mTSourceCode;
			ref T reference4 = ref item2;
			val = default;
			if (val == null)
			{
				val = reference4;
				reference4 = ref val;
			}
			int mTSourceID = item3.MTSourceID;
			reference4.ItemID = mTSourceID;
			ref T reference5 = ref item2;
			val = default;
			if (val == null)
			{
				val = reference5;
				reference5 = ref val;
			}
			string mTSourceCaption = item3.MTSourceCaption;
			reference5.ItemName = mTSourceCaption;
			ref T reference6 = ref item2;
			val = default;
			if (val == null)
			{
				val = reference6;
				reference6 = ref val;
			}
			string brief = TradeSettings.TradeCacheService.QntUnit.GetItem(item3.QntUnit).Brief;
			reference6.UnitBrief = brief;
			ref T reference7 = ref item2;
			val = default;
			if (val == null)
			{
				val = reference7;
				reference7 = ref val;
			}
			string qntUnit2 = item3.QntUnit;
			reference7.UnitCode = qntUnit2;
			ref T reference8 = ref item2;
			val = default;
			if (val == null)
			{
				val = reference8;
				reference8 = ref val;
			}
			decimal quantity = item3.QTY * value;
			reference8.Quantity = quantity;
			MaterialCacheRow.QntUnitRow qntUnit3 = TradeSettings.TradeCacheService.Material.GetQntUnit(item2.ItemCode, item2.UnitCode);
			MaterialCacheRow.BarCodeRow barCode = TradeSettings.TradeCacheService.Material.GetBarCode(item2.ItemID, item2.UnitCode);
			if (_doc.POType == DocumentTypeSE.MTBill.Value)
			{
				ref T reference9 = ref item2;
				val = default;
				if (val == null)
				{
					val = reference9;
					reference9 = ref val;
				}
				string barCode2 = ((barCode == null) ? "" : barCode.BarCode);
				reference9.BarCode = barCode2;
			}
			if ((_doc.POType == DocumentTypeSE.MTBill.Value && !flag) || _doc.POType == DocumentTypeSE.MTInvoice.Value)
			{
				ref T reference10 = ref item2;
				val = default;
				if (val == null)
				{
					val = reference10;
					reference10 = ref val;
				}
				decimal priceForGivenPriceListAndVATType = TradeSettings.CreateTradeClient().CommonServices.GetPriceForGivenPriceListAndVATType(_priceType, _vatType, item2.ItemID, item2.UnitCode, _docDate, _curCode);
				reference10.Price = priceForGivenPriceListAndVATType;
			}
			else if ((((_doc.POType == DocumentTypeSE.MTBill.Value) & flag) || _doc.POType == DocumentTypeSE.MTSaleRetail.Value) && qntUnit3 != null)
			{
				MTRetailQtyAndPriceDataProvider.Param param = new MTRetailQtyAndPriceDataProvider.Param
				{
					MTCode = item2.ItemCode,
					UnitCoef = qntUnit3.Coef,
					Storage = _storageField,
					Date = _docDate
				};
				List<MTRetailQtyAndPriceDataProvider.DataRow> rows2 = new MTRetailQtyAndPriceDataProvider(TradeSettings.CreateTradeClient()).GetRows(param);
				if (rows2.Count > 0)
				{
					ref T reference11 = ref item2;
					val = default;
					if (val == null)
					{
						val = reference11;
						reference11 = ref val;
					}
					decimal price = rows2[0].Price;
					reference11.Price = price;
				}
			}
			if ((_isUseDiscount && !_calcDiscountAuto) || _useDiscount != 0m)
			{
				ref T reference12 = ref item2;
				val = default;
				if (val == null)
				{
					val = reference12;
					reference12 = ref val;
				}
				decimal value2 = _discountPercent.Value;
				reference12.Discount = value2;
				ref T reference13 = ref item2;
				val = default;
				if (val == null)
				{
					val = reference13;
					reference13 = ref val;
				}
				decimal discountPrice = (item2.Price - item2.Price * item2.Discount / 100m).RoundAs(2);
				reference13.DiscountPrice = discountPrice;
				ref T reference14 = ref item2;
				val = default;
				if (val == null)
				{
					val = reference14;
					reference14 = ref val;
				}
				decimal summa = (item2.DiscountPrice * item2.Quantity).RoundAs(2);
				reference14.Summa = summa;
			}
			else if (_calcDiscountAuto)
			{
				decimal discPercent = _discPercent;
				item2.Discount = discPercent;
				ref T reference15 = ref item2;
				val = default;
				if (val == null)
				{
					val = reference15;
					reference15 = ref val;
				}
				decimal discountPrice2 = (item2.Price - item2.Price * item2.Discount / 100m).RoundAs(2);
				reference15.DiscountPrice = discountPrice2;
				ref T reference16 = ref item2;
				val = default;
				if (val == null)
				{
					val = reference16;
					reference16 = ref val;
				}
				decimal summa2 = (item2.DiscountPrice * item2.Quantity).RoundAs(2);
				reference16.Summa = summa2;
			}
			else
			{
				ref T reference17 = ref item2;
				val = default;
				if (val == null)
				{
					val = reference17;
					reference17 = ref val;
				}
				decimal summa3 = (item2.Price * item2.Quantity).RoundAs(2);
				reference17.Summa = summa3;
			}
			ref T reference18 = ref item2;
			val = default;
			if (val == null)
			{
				val = reference18;
				reference18 = ref val;
			}
			decimal discountSumma = (item2.Price * item2.Quantity).RoundAs(2) - item2.Summa;
			reference18.DiscountSumma = discountSumma;
			ref T reference19 = ref item2;
			val = default;
			if (val == null)
			{
				val = reference19;
				reference19 = ref val;
			}
			bool? vAT = TradeSettings.TradeCacheService.Material.GetItem(item2.ItemCode).VAT;
			reference19.VAT = vAT;
			ref T reference20 = ref item2;
			val = default;
			if (val == null)
			{
				val = reference20;
				reference20 = ref val;
			}
			bool? isPartySelected = false;
			reference20.IsPartySelected = isPartySelected;
			int maxRowId = _maxRowId;
			item2.RowID = maxRowId;
			_mtMTInvoiceBaseList.Add(item2);
			_maxRowId++;
			AddedRows++;
			e.RowNumber = _mtMTInvoiceBaseList.Count - 1;
			_grid.InnerAddRow(e);
		}
		_grid.Grid.UpdateTotalSummary();
	}
}
