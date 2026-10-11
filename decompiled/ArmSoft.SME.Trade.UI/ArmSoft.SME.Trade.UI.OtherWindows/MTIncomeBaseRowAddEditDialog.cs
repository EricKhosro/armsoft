using System;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Extensions;
using ArmSoft.SME.Common.Parameters;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Core.UI.Controls;
using ArmSoft.SME.Core.UI.DataViews;
using ArmSoft.SME.Core.UI.Helpers;
using ArmSoft.SME.Trade.Client.Data.Directories;
using ArmSoft.SME.Trade.Common;
using ArmSoft.SME.Trade.Common.Models.Caches;
using ArmSoft.SME.Trade.Common.Models.Documents;
using ArmSoft.SME.Trade.Common.Parameters;
using ArmSoft.SME.Trade.Common.SmartEnums;
using ArmSoft.SME.Trade.UI.Documents;
using ArmSoft.SME.Trade.UI.DropDown;
using ArmSoft.SME.Trade.UI.DropDownDataProvider;
using ArmSoft.SME.Trade.UI.Helpers;
using DevExpress.Xpf.Editors;

namespace ArmSoft.SME.Trade.UI.OtherWindows;

public class MTIncomeBaseRowAddEditDialog<T> : DialogWindow where T : MTIncomeBase, new()
{
	private DropDownControl _mtGroup;

	private DropDownControl _mtCode;

	private DropDownControl _qntUnit;

	private PopupCalcEditExt _quantity;

	private PopupCalcEditExt _price;

	private PopupCalcEditExt _discountPercent;

	private PopupCalcEditExt _discountPrice;

	private PopupCalcEditExt _summa;

	private PopupCalcEditExt _extraPercent;

	private PopupCalcEditExt _salePrice;

	private PopupCalcEditExt _saleSumma;

	private CheckEditExt _vat;

	private DateEditControl _useLife;

	private DropDownControl _country;

	private DropDownControl _producer;

	private TextEditExt _description;

	private DropDownControl _property1;

	private DropDownControl _property2;

	private int _rowID;

	private readonly MTIncomeBaseWindow<T>.MTIncomeSpecificationRow _sourceRow;

	private bool _editableMode;

	private readonly bool _allowAddRowAndNotViewMode;

	private readonly bool _existDiscountControls;

	private bool _vatTypeExist;

	private readonly decimal _tradeConstVAT = Settings.ParametersService.VAT();

	private readonly MTIncomeBaseWindow<T> _incomeWindow;

	public int AddedRows { get; set; }

	public MTIncomeBaseRowAddEditDialog(DialogWindowMode mode, MTIncomeBaseWindow<T> incomeBaseWindow, MTIncomeBaseWindow<T>.MTIncomeSpecificationRow sourceRow = null)
	{
		Mode = mode;
		_incomeWindow = incomeBaseWindow;
		_allowAddRowAndNotViewMode = _incomeWindow._mtIncomeGrid.AllowAdd && mode != DialogWindowMode.ViewMode;
		_existDiscountControls = _incomeWindow._allowDiscountInPurchase || _incomeWindow.UseDiscount != 0m;
		AddedRows = 0;
		Owner = incomeBaseWindow;
		if (sourceRow != null)
		{
			_sourceRow = sourceRow;
		}
		Caption = ArmSoft.SME.Trade.Common.Resources.NewMaterialInMtIncome;
	}

	public bool? Invoke()
	{
		_mtGroup = AddDropDownControl("MTGroup", ArmSoft.SME.Common.Resources.Group, new GoodsGroups());
		_mtGroup.EditValueChanged += new SafeEventHandler<EditValueChangedEventArgs>(MTGroup_EditValueChanged).Handler;
		if (!string.IsNullOrWhiteSpace(_incomeWindow._priceListType.Code) && _incomeWindow.ShowGoodsListWithPrices)
		{
			_mtCode = AddDropDownControl(_incomeWindow.Definition.Grids["MTList"].Columns["MTCode"], new GoodsWithPrices(_incomeWindow._date.EditValue.Value, _incomeWindow._priceListType.Code, Name, "", ItemTypesSE.Goods.Value), allowAdditionButtons: true);
		}
		else
		{
			_mtCode = AddDropDownControl(_incomeWindow.Definition.Grids["MTList"].Columns["MTCode"], new GoodsFilter(shortMode: true, ItemTypesSE.Goods.Value), allowAdditionButtons: true);
		}
		_mtCode.ClickDropDown += new SafeEventHandler<DropDownControlEventArgs>(MTCode_ClickDropDown).Handler;
		_mtCode.EditValueChanged += new SafeEventHandler<EditValueChangedEventArgs>(MTCode_EditValueChanged).Handler;
		_mtCode.EditAdditionalValueChanged += new SafeEventHandler<EventArgs>(MTCode_EditAdditionalValueChanged).Handler;
		_qntUnit = AddDropDownControl(_incomeWindow.Definition.Grids["MTList"].Columns["UnitCode"], new QntUnits());
		_qntUnit.ClickDropDown += new SafeEventHandler<DropDownControlEventArgs>(QntUnit_ClickDropDown).Handler;
		_qntUnit.EditValueChanged += new SafeEventHandler<EditValueChangedEventArgs>(QntUnit_EditValueChanged).Handler;
		_quantity = AddNumericControl(_incomeWindow.Definition.Grids["MTList"].Columns["Quantity"]);
		_quantity.EditValueChanged += new SafeEventHandler<EditValueChangedEventArgs>(Quantity_EditValueChanged).Handler;
		if (_existDiscountControls)
		{
			_price = AddNumericControl(_incomeWindow.Definition.Grids["MTList"].Columns["Price"]);
			_discountPercent = AddNumericControl(_incomeWindow.Definition.Grids["MTList"].Columns["Discount"]);
			_discountPercent.EditValueChanged += new SafeEventHandler<EditValueChangedEventArgs>(DiscountPercent_EditValueChanged).Handler;
			_discountPrice = AddNumericControl(_incomeWindow.Definition.Grids["MTList"].Columns["DiscountPrice"]);
			_discountPrice.EditValueChanged += new SafeEventHandler<EditValueChangedEventArgs>(DiscountPrice_EditValueChanged).Handler;
			_summa = AddNumericControl(_incomeWindow.Definition.Grids["MTList"].Columns["Summa"]);
		}
		else
		{
			LayoutGroupExt container = AddHorizontalLayoutGroup();
			_price = AddNumericControl(container, _incomeWindow.Definition.Grids["MTList"].Columns["Price"]);
			_summa = AddNumericControl(container, _incomeWindow.Definition.Grids["MTList"].Columns["Summa"]);
		}
		_summa.IsRequired = !Settings.ParametersService.AllowPurchaseWith0PriceOr100Percent();
		((LayoutItemWithRequired)(object)_price.Parent).Label = ((_incomeWindow._currCode.Code == "AMD") ? ArmSoft.SME.Common.Resources.Price : ArmSoft.SME.Common.Resources.FCPrice);
		((LayoutItemWithRequired)(object)_summa.Parent).Label = ((_incomeWindow._currCode.Code == "AMD") ? ArmSoft.SME.Common.Resources.Summa : ArmSoft.SME.Common.Resources.FCSumma);
		_price.EditValueChanged += new SafeEventHandler<EditValueChangedEventArgs>(Price_EditValueChanged).Handler;
		_summa.EditValueChanged += new SafeEventHandler<EditValueChangedEventArgs>(Summa_EditValueChanged).Handler;
		if (_incomeWindow.PO.POType == DocumentTypeSE.MTIncomeRetail.Value)
		{
			_extraPercent = AddNumericControl("ExtraPercent", ArmSoft.SME.Trade.Common.Resources.Extra, 6, 2, isRequired: false, allowNegativeNumbers: true);
			_extraPercent.EditValueChanged += new SafeEventHandler<EditValueChangedEventArgs>(ExtraPercent_EditValueChanged).Handler;
			LayoutGroupExt container2 = AddHorizontalLayoutGroup();
			_salePrice = AddNumericControl(container2, "SalePrice", ArmSoft.SME.Common.Resources.MTTAccSalePrice, 16, 2);
			_salePrice.EditValueChanged += new SafeEventHandler<EditValueChangedEventArgs>(SalePrice_EditValueChanged).Handler;
			_saleSumma = AddNumericControl(container2, "SaleSumma", "", 16, 2);
			_saleSumma.IsReadOnly = true;
			_saleSumma.IsTabStop = false;
		}
		_description = AddStringControl(_incomeWindow.Definition.Grids["MTList"].Columns["RowDescription"]);
		_vatTypeExist = _incomeWindow._vatType.Code.InList("1", "2");
		if (_vatTypeExist)
		{
			_vat = AddBooleanControl(_incomeWindow.Definition.Grids["MTList"].Columns["VAT"]);
			_vat.EditValueChanged += new SafeEventHandler<EditValueChangedEventArgs>(VAT_EditValueChanged).Handler;
		}
		if (Settings.ParametersService.ShowPartyUSEABLEDATE())
		{
			_useLife = AddDateControl(_incomeWindow.Definition.Grids["MTList"].Columns["UseLife"]);
		}
		if (Settings.ParametersService.ShowPartyCountry())
		{
			_country = AddDropDownControl(_incomeWindow.Definition.Grids["MTList"].Columns["Country"], new Countries());
		}
		if (Settings.ParametersService.ShowPartyProducer())
		{
			_producer = AddDropDownControl(_incomeWindow.Definition.Grids["MTList"].Columns["Producer"], new Producer());
		}
		if (Settings.ParametersService.ShowPartyProperty1())
		{
			_property1 = AddDropDownControl(_incomeWindow.Definition.Grids["MTList"].Columns["Property1"], new PropertyList());
			_property1.IsReadOnly = true;
			_property1.EditValueChanged += new SafeEventHandler<EditValueChangedEventArgs>(Property1_EditValueChanged).Handler;
			_property1.ClickAddDropDown += additionButtonsHandler;
			_property1.ClickEditDropDown += additionButtonsHandler;
		}
		if (Settings.ParametersService.ShowPartyProperty2())
		{
			_property2 = AddDropDownControl(_incomeWindow.Definition.Grids["MTList"].Columns["Property2"], new PropertyList());
			_property2.IsReadOnly = true;
			_property2.EditValueChanged += new SafeEventHandler<EditValueChangedEventArgs>(Property2_EditValueChanged).Handler;
			_property2.ClickAddDropDown += additionButtonsHandler2;
			_property2.ClickEditDropDown += additionButtonsHandler2;
		}
		WindowActivated += IncomeRow_WindowActivated;
		WindowValidation += new SafeEventHandler<DialogWindowValidationArgs>(IncomeRow_WindowValidation).Handler;
		NextPressed += new SafeEventHandler<EventArgs>(IncomeRow_NextPressed).Handler;
		return ShowDialog();
		void additionButtonsHandler(object s, DropDownControlAddEventArgs e)
		{
			_property1.DoValidate();
			((PropertyList)_property1.DataView).DestCode = _property1.Code;
		}
		void additionButtonsHandler2(object s, DropDownControlAddEventArgs e)
		{
			_property2.DoValidate();
			((PropertyList)_property2.DataView).DestCode = _property2.Code;
		}
	}

	private void IncomeRow_WindowActivated(DialogWindow window)
	{
		_editableMode = false;
		if (_sourceRow != null)
		{
			if (Mode == DialogWindowMode.NewModeWithNext)
			{
				_rowID = _incomeWindow._maxRowId;
			}
			else
			{
				_rowID = _sourceRow.RowID;
			}
			_mtCode.Code = _sourceRow.MTCode;
			_qntUnit.Code = _sourceRow.UnitCode;
			_quantity.Value = _sourceRow.Quantity;
			_price.Value = _sourceRow.Price;
			if (_existDiscountControls)
			{
				_discountPercent.Value = _sourceRow.Discount;
				_discountPrice.Value = _sourceRow.DiscountPrice;
			}
			_summa.Value = _sourceRow.Summa;
			if (_vatTypeExist)
			{
				_vat.IsChecked = _sourceRow.VAT;
			}
			_description.EditValue = _sourceRow.RowDescription;
			if (_incomeWindow.PO.POType == DocumentTypeSE.MTIncomeRetail.Value)
			{
				_extraPercent.Value = _sourceRow.ExtraPercent;
				_salePrice.Value = _sourceRow.SalePrice;
				_saleSumma.Value = _sourceRow.SaleSumma;
			}
			if (_useLife != null)
			{
				_useLife.EditValue = _sourceRow.UseLife;
			}
			if (_country != null)
			{
				_country.Code = _sourceRow.Country;
			}
			if (_producer != null)
			{
				_producer.Code = _sourceRow.Producer;
			}
			if (_property1 != null)
			{
				_property1.Code = _sourceRow.Property1;
			}
			if (_property2 != null)
			{
				_property2.Code = _sourceRow.Property2;
			}
			if (!string.IsNullOrWhiteSpace(_mtCode.Code) && Mode != DialogWindowMode.ViewMode)
			{
				MaterialCacheRow item = TradeSettings.TradeCacheService.Material.GetItem(_mtCode.Code);
				if (item != null)
				{
					_qntUnit.DataView = new MTQntUnits(_mtCode.Code);
					_qntUnit.DoValidate();
					SetReadOnlyProperties(item.Property1, item.Property2);
				}
			}
			if (!_allowAddRowAndNotViewMode)
			{
				SetControlsReadonly(value: true);
			}
		}
		else if (Mode == DialogWindowMode.NewModeWithNext)
		{
			_rowID = _incomeWindow._maxRowId;
			if (_existDiscountControls)
			{
				_discountPercent.Value = _incomeWindow._discountPercent.Value;
			}
		}
		ControlsHelper.SetFocus(_mtCode);
		_editableMode = true;
	}

	private void MTCode_EditAdditionalValueChanged(object sender, EventArgs e)
	{
		UpdateMaterialData();
	}

	private void MTGroup_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		if (_mtGroup.DoValidate() && !string.IsNullOrWhiteSpace(_mtCode.Code) && _mtCode.DoValidate() && TradeSettings.TradeCacheService.Material.GetItem(_mtCode.Code) != null)
		{
			_qntUnit.DataView = new MTQntUnits(_mtCode.Code);
		}
		if (!string.IsNullOrWhiteSpace(_mtGroup.Code) && !string.IsNullOrWhiteSpace(_mtCode.Code) && !_mtCode.DoValidate())
		{
			CleanAllField();
		}
	}

	private void Property1_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		if (_property1.DoValidate() && !string.IsNullOrWhiteSpace(_property1.Code))
		{
			string comment = string.Empty;
			string realValue = string.Empty;
			_property1.DataView.Validate(_property1.Code, ref comment, ref realValue);
			_property1.Code = realValue;
		}
	}

	private void Property2_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		if (_property2.DoValidate() && !string.IsNullOrWhiteSpace(_property2.Code))
		{
			string comment = string.Empty;
			string realValue = string.Empty;
			_property2.DataView.Validate(_property2.Code, ref comment, ref realValue);
			_property2.Code = realValue;
		}
	}

	private void MTCode_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		if (!string.IsNullOrWhiteSpace(_mtCode.Code) && _mtCode.DoValidate())
		{
			if (_editableMode)
			{
				UpdateMaterialData();
			}
		}
		else
		{
			_qntUnit.DataView = new MTQntUnits(_mtGroup.Code, isShowQntUnitCaption: false, isShowOnlyComplects: false, "", ItemTypesSE.Goods.Value);
			_qntUnit.Code = "";
		}
	}

	private void MTCode_ClickDropDown(object sender, DropDownControlEventArgs e)
	{
		IDropDownView dropDownView = ((string.IsNullOrWhiteSpace(_incomeWindow._priceListType.Code) || !_incomeWindow.ShowGoodsListWithPrices) ? ((IDropDownView)new GoodsFilter(shortMode: true, ItemTypesSE.Goods.Value, _mtGroup.Code, isShowOnlyInPriceList: true)) : ((IDropDownView)new GoodsWithPrices(_incomeWindow._date.EditValue.Value, _incomeWindow._priceListType.Code, Name, "", ItemTypesSE.Goods.Value)));
		ModalBrowser modalBrowser = new ModalBrowser(dropDownView)
		{
			LookUpValue = _mtCode.Code,
			Owner = e.Owner,
			Left = e.Left,
			Top = e.Top
		};
		if (modalBrowser.Show())
		{
			_mtCode.Code = dropDownView.ValueMember(modalBrowser.SelectedRowIndex);
		}
		e.Cancel = true;
	}

	private void QntUnit_ClickDropDown(object sender, DropDownControlEventArgs e)
	{
		if (!string.IsNullOrWhiteSpace(_mtCode.Code) && _mtCode.DoValidate())
		{
			return;
		}
		MTQntUnits mTQntUnits = new MTQntUnits(_mtGroup.Code, isShowQntUnitCaption: false, isShowOnlyComplects: false, "", ItemTypesSE.Goods.Value);
		ModalBrowser modalBrowser = new ModalBrowser(mTQntUnits)
		{
			Owner = e.Owner,
			Left = e.Left,
			Top = e.Top
		};
		if (modalBrowser.Show())
		{
			MTQntUnitDataProvider.DataRow.Key key = (MTQntUnitDataProvider.DataRow.Key)mTQntUnits.KeyMember(modalBrowser.SelectedRowIndex);
			if (key != null)
			{
				MaterialCacheRow itemById = TradeSettings.TradeCacheService.Material.GetItemById(key.MTID);
				if (itemById != null)
				{
					_mtCode.Code = itemById.MTCode;
					_mtCode.DoValidate();
				}
			}
			_qntUnit.Code = mTQntUnits.ValueMember(modalBrowser.SelectedRowIndex);
		}
		e.Cancel = true;
	}

	private void QntUnit_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		if (string.IsNullOrWhiteSpace(_qntUnit.Code) || !_qntUnit.DoValidate() || string.IsNullOrWhiteSpace(_mtCode.Code) || !_mtCode.DoValidate() || !_editableMode)
		{
			return;
		}
		MaterialCacheRow item = TradeSettings.TradeCacheService.Material.GetItem(_mtCode.Code);
		if (item == null)
		{
			throw new ASException(string.Format(ArmSoft.SME.Trade.Common.Resources.MaterialNotExist, _mtCode.Code));
		}
		if (string.IsNullOrWhiteSpace(_incomeWindow._priceListType.Code))
		{
			_price.EditValue = PriceHelper.GetPriceForUnit(e.OldValue.ToString(), _qntUnit.Code, _price.Value, _mtCode.Code);
			return;
		}
		PricesDataProvider.DataRow price = TradeSettings.CreateTradeClient().CommonServices.GetPrice(_incomeWindow._priceListType.Code, item.Id, _qntUnit.Code, _incomeWindow._date.EditValue.Value, _incomeWindow._currCode.Code);
		if (price.Price != 0m)
		{
			_price.EditValue = price.Price;
		}
		else
		{
			_price.EditValue = PriceHelper.GetPriceForUnit(e.OldValue.ToString().ToString(), _qntUnit.Code, _price.Value, _mtCode.Code);
		}
	}

	private void UpdateMaterialData()
	{
		try
		{
			MaterialCacheRow item = TradeSettings.TradeCacheService.Material.GetItem(_mtCode.Code);
			if (item == null)
			{
				throw new ASException(string.Format(ArmSoft.SME.Trade.Common.Resources.MaterialNotExist, _mtCode.Code));
			}
			_qntUnit.DataView = new MTQntUnits(_mtCode.Code);
			_editableMode = false;
			_qntUnit.Code = item.Unit;
			_editableMode = true;
			_quantity.EditValue = 0m;
			_price.EditValue = TradeSettings.CreateTradeClient().CommonServices.GetPriceForGivenPriceListAndVATType(_incomeWindow._priceListType.Code, _incomeWindow._vatType.Code, item.Id, item.Unit, _incomeWindow._date.EditValue.Value, _incomeWindow._currCode.Code);
			_summa.EditValue = 0m;
			if (_incomeWindow.PO.POType == DocumentTypeSE.MTIncomeRetail.Value)
			{
				_extraPercent.EditValue = 0m;
				_salePrice.EditValue = 0m;
				_saleSumma.EditValue = 0m;
				_extraPercent.Value = ((item.Extra == 0m) ? Settings.ParametersService.Extra() : item.Extra);
			}
			if (_vatTypeExist)
			{
				_vat.IsChecked = item.VAT;
			}
			if (_useLife != null)
			{
				_useLife.EditValue = null;
			}
			if (_country != null)
			{
				_country.Code = item.Country;
			}
			if (_producer != null)
			{
				_producer.Code = item.Producer;
			}
			if (_property1 != null)
			{
				_property1.Code = "";
			}
			if (_property2 != null)
			{
				_property2.Code = "";
			}
			if (_description != null)
			{
				_description.Text = "";
			}
			SetReadOnlyProperties(item.Property1, item.Property2);
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	private void SetReadOnlyProperties(string property1, string property2)
	{
		if (_property1 != null)
		{
			if (string.IsNullOrWhiteSpace(property1))
			{
				_property1.Code = "";
				_property1.IsReadOnly = true;
			}
			else
			{
				_property1.IsReadOnly = false;
				_property1.DataView = new PropertyList(property1);
			}
			_property1.DoValidate();
		}
		if (_property2 != null)
		{
			if (string.IsNullOrWhiteSpace(property2))
			{
				_property2.Code = "";
				_property2.IsReadOnly = true;
			}
			else
			{
				_property2.IsReadOnly = false;
				_property2.DataView = new PropertyList(property2);
			}
			_property2.DoValidate();
		}
	}

	private void Quantity_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		if (e.NewValue != null && _editableMode)
		{
			_editableMode = false;
			if (_existDiscountControls)
			{
				_summa.EditValue = PriceHelper.CalculateSum(_discountPrice.Value, _quantity.Value);
			}
			else
			{
				_summa.EditValue = PriceHelper.CalculateSum(_price.Value, _quantity.Value);
			}
			_summa.DoValidate();
			if (_incomeWindow.PO.POType == DocumentTypeSE.MTIncomeRetail.Value)
			{
				UpdateSalePriceAndSaleSumma();
			}
			_editableMode = true;
		}
	}

	private void Price_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		if (e.NewValue != null && _editableMode)
		{
			_editableMode = false;
			if (_existDiscountControls)
			{
				_discountPrice.EditValue = PriceHelper.CalculateDiscountPrice(_price.Value, _incomeWindow._discountPercent.Value);
				_discountPrice.DoValidate();
				_summa.EditValue = PriceHelper.CalculateSum(_discountPrice.Value, _quantity.Value);
			}
			else
			{
				_summa.EditValue = PriceHelper.CalculateSum(_price.Value, _quantity.Value);
			}
			_summa.DoValidate();
			if (_incomeWindow.PO.POType == DocumentTypeSE.MTIncomeRetail.Value)
			{
				UpdateSalePriceAndSaleSumma();
			}
			_editableMode = true;
		}
	}

	private void DiscountPercent_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		if (e.NewValue != null && _editableMode)
		{
			_editableMode = false;
			_discountPrice.EditValue = PriceHelper.CalculateDiscountPrice(_price.Value, _discountPercent.Value);
			_discountPrice.DoValidate();
			_summa.EditValue = PriceHelper.CalculateSum(_discountPrice.Value, _quantity.Value);
			_summa.DoValidate();
			if (_incomeWindow.PO.POType == DocumentTypeSE.MTIncomeRetail.Value)
			{
				UpdateSalePriceAndSaleSumma();
			}
			_editableMode = true;
		}
	}

	private void DiscountPrice_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		if (e.NewValue != null && _editableMode)
		{
			_editableMode = false;
			_summa.EditValue = PriceHelper.CalculateSum(_discountPrice.Value, _quantity.Value);
			_summa.DoValidate();
			if (_price.Value != 0m)
			{
				_discountPercent.EditValue = (100m * (_price.Value - _discountPrice.Value) / _price.Value).RoundAs(2);
				_discountPercent.DoValidate();
			}
			if (_incomeWindow.PO.POType == DocumentTypeSE.MTIncomeRetail.Value)
			{
				UpdateSalePriceAndSaleSumma();
			}
			_editableMode = true;
		}
	}

	private void Summa_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		if (e.NewValue == null || !_editableMode || !(_quantity.Value > 0m))
		{
			return;
		}
		_editableMode = false;
		if (_existDiscountControls)
		{
			_discountPrice.EditValue = (_summa.Value / _quantity.Value).RoundAs(2);
			_discountPercent.DoValidate();
		}
		else
		{
			_price.EditValue = (_summa.Value / _quantity.Value).RoundAs(2);
			_price.DoValidate();
		}
		if (_existDiscountControls)
		{
			if (_price.Value != 0m)
			{
				_discountPercent.EditValue = (100m * (_price.Value - _discountPrice.Value) / _price.Value).RoundAs(2);
				_discountPercent.DoValidate();
			}
			else
			{
				_price.EditValue = _discountPrice.Value;
				_price.DoValidate();
			}
		}
		if (_incomeWindow.PO.POType == DocumentTypeSE.MTIncomeRetail.Value)
		{
			UpdateSalePriceAndSaleSumma();
		}
		_editableMode = true;
	}

	private void ExtraPercent_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		if (e.NewValue != null && _editableMode)
		{
			_editableMode = false;
			UpdateSalePriceAndSaleSumma();
			_editableMode = true;
		}
	}

	private void SalePrice_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		if (e.NewValue != null && _editableMode)
		{
			_editableMode = false;
			_saleSumma.EditValue = PriceHelper.CalculateSum(_salePrice.Value, _quantity.Value);
			if (_existDiscountControls)
			{
				_extraPercent.EditValue = CalcExtraPercent(_discountPrice.Value, _salePrice.Value);
			}
			else
			{
				_extraPercent.EditValue = CalcExtraPercent(_price.Value, _salePrice.Value);
			}
			_editableMode = true;
		}
	}

	private void VAT_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		if (_editableMode && _incomeWindow.PO.POType == DocumentTypeSE.MTIncomeRetail.Value)
		{
			_editableMode = false;
			UpdateSalePriceAndSaleSumma();
			_editableMode = true;
		}
	}

	private void UpdateSalePriceAndSaleSumma()
	{
		if (_existDiscountControls)
		{
			_salePrice.EditValue = CalcSalePrice(_discountPrice.Value, _extraPercent.Value);
		}
		else
		{
			_salePrice.EditValue = CalcSalePrice(_price.Value, _extraPercent.Value);
		}
		_saleSumma.EditValue = PriceHelper.CalculateSum(_salePrice.Value, _quantity.Value);
		_salePrice.DoValidate();
		_saleSumma.DoValidate();
	}

	private decimal CalcSalePrice(decimal price, decimal extraPercent)
	{
		decimal num = ((_vatTypeExist && _vat.IsChecked.Value) ? _tradeConstVAT : 0m);
		if (_incomeWindow._currCode.Code != "AMD" && _incomeWindow._currExChngRate.Base != 0m)
		{
			price = (price * _incomeWindow._currExChngRate.Rate / _incomeWindow._currExChngRate.Base).RoundAs(2);
		}
		if (_incomeWindow._vatType.Code == "1")
		{
			price += (price * num / 100m).RoundAs(2);
		}
		return (price + price * extraPercent / 100m).RoundAs(2);
	}

	private decimal CalcExtraPercent(decimal price, decimal salePrice)
	{
		decimal num = ((_vatTypeExist && _vat.IsChecked.Value) ? _tradeConstVAT : 0m);
		if (price == 0m)
		{
			return 999.99m;
		}
		if (_incomeWindow._currCode.Code != "AMD" && _incomeWindow._currExChngRate.Base != 0m)
		{
			price = (price * _incomeWindow._currExChngRate.Rate / _incomeWindow._currExChngRate.Base).RoundAs(2);
		}
		if (_incomeWindow._vatType.Code == "1")
		{
			price += (price * num / 100m).RoundAs(2);
		}
		decimal num2 = 0m;
		if (price != 0m)
		{
			num2 = ((salePrice - price) * 100m / price).RoundAs(2);
		}
		if (num2 > 999.99m)
		{
			num2 = 999.99m;
		}
		return num2;
	}

	private void IncomeRow_WindowValidation(object sender, DialogWindowValidationArgs args)
	{
		try
		{
			if (_price.Value < 0m)
			{
				ControlsHelper.SetFocus(_price);
				throw new ASException(string.Format(ArmSoft.SME.Trade.Common.Resources.ErrFieldCantBeNegative, ArmSoft.SME.Common.Resources.Price));
			}
			if (_existDiscountControls && _discountPrice.Value < 0m)
			{
				ControlsHelper.SetFocus(_discountPrice);
				throw new ASException(string.Format(ArmSoft.SME.Trade.Common.Resources.ErrFieldCantBeNegative, ArmSoft.SME.Common.Resources.DiscountedPrice));
			}
			if (_incomeWindow.PO.POType == DocumentTypeSE.MTIncomeRetail.Value)
			{
				if (_extraPercent.Value < -99.99m || _extraPercent.Value > 999.99m)
				{
					ControlsHelper.SetFocus(_extraPercent);
					throw new ASException(ArmSoft.SME.Trade.Common.Resources.ValidExtraValue);
				}
				if (_salePrice.Value <= 0m)
				{
					ControlsHelper.SetFocus(_salePrice);
					throw new ASException(string.Format(ArmSoft.SME.Trade.Common.Resources.ErrFieldIsGtZero, ArmSoft.SME.Common.Resources.MTTAccSalePrice));
				}
			}
			MTIncomeBaseWindow<T>.MTIncomeSpecificationRow mTIncomeSpecificationRow;
			if (Mode == DialogWindowMode.NewModeWithNext)
			{
				mTIncomeSpecificationRow = new MTIncomeBaseWindow<T>.MTIncomeSpecificationRow();
				_incomeWindow._mtIncomeList.Add(mTIncomeSpecificationRow);
			}
			else
			{
				mTIncomeSpecificationRow = _sourceRow;
			}
			DocumentGridAddingRowEventArgs e = new DocumentGridAddingRowEventArgs(_incomeWindow._mtIncomeGrid);
			MaterialCacheRow item = TradeSettings.TradeCacheService.Material.GetItem(_mtCode.Code);
			if (item == null)
			{
				throw new ASException(string.Format(ArmSoft.SME.Trade.Common.Resources.MaterialNotExist, _mtCode.Code));
			}
			mTIncomeSpecificationRow.MTID = item.Id;
			mTIncomeSpecificationRow.MTCode = item.MTCode;
			mTIncomeSpecificationRow.MTName = item.Caption;
			mTIncomeSpecificationRow.UnitCode = _qntUnit.Code;
			QntUnitCacheRow item2 = TradeSettings.TradeCacheService.QntUnit.GetItem(mTIncomeSpecificationRow.UnitCode);
			if (item2 != null)
			{
				mTIncomeSpecificationRow.UnitBrief = item2.Brief;
			}
			else
			{
				mTIncomeSpecificationRow.UnitBrief = "";
			}
			mTIncomeSpecificationRow.Quantity = _quantity.Value;
			mTIncomeSpecificationRow.Price = _price.Value;
			if (_existDiscountControls)
			{
				mTIncomeSpecificationRow.Discount = _discountPercent.Value;
				mTIncomeSpecificationRow.DiscountPrice = _discountPrice.Value;
			}
			else
			{
				mTIncomeSpecificationRow.Discount = 0m;
				mTIncomeSpecificationRow.DiscountPrice = mTIncomeSpecificationRow.Price;
			}
			mTIncomeSpecificationRow.Summa = _summa.Value;
			mTIncomeSpecificationRow.RowDescription = _description.Text;
			if (_incomeWindow.PO.POType == DocumentTypeSE.MTIncomeRetail.Value)
			{
				mTIncomeSpecificationRow.ExtraPercent = _extraPercent.Value;
				mTIncomeSpecificationRow.SalePrice = _salePrice.Value;
				mTIncomeSpecificationRow.SaleSumma = _saleSumma.Value;
			}
			if (_vatTypeExist)
			{
				mTIncomeSpecificationRow.VAT = Convert.ToBoolean(_vat.IsChecked);
			}
			if (_useLife != null && _useLife.EditValue.HasValue)
			{
				mTIncomeSpecificationRow.UseLife = _useLife.EditValue;
			}
			if (_country != null)
			{
				mTIncomeSpecificationRow.Country = _country.Code;
			}
			if (_producer != null)
			{
				mTIncomeSpecificationRow.Producer = _producer.Code;
			}
			if (_property1 != null)
			{
				mTIncomeSpecificationRow.Property1 = _property1.Code;
				mTIncomeSpecificationRow.Property1Caption = _property1.Comment;
			}
			if (_property2 != null)
			{
				mTIncomeSpecificationRow.Property2 = _property2.Code;
				mTIncomeSpecificationRow.Property2Caption = _property2.Comment;
			}
			if (Mode == DialogWindowMode.NewModeWithNext)
			{
				mTIncomeSpecificationRow.RowID = _rowID;
				_incomeWindow._maxRowId++;
				AddedRows++;
				e.RowNumber = _incomeWindow._mtIncomeList.Count - 1;
				_incomeWindow._mtIncomeGrid.InnerAddRow(e);
			}
			_incomeWindow._mtIncomeGrid.Grid.UpdateTotalSummary();
			_incomeWindow._mtIncomeGrid.View.CommitEditing();
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
			args.isValid = false;
			if (Mode == DialogWindowMode.NewModeWithNext)
			{
				args.SetFocusWhenInvalidSave = _mtCode;
			}
		}
	}

	private void IncomeRow_NextPressed(object sender, EventArgs e)
	{
		_editableMode = false;
		CleanAllField(isCleanItemCodeField: false);
		_rowID = _incomeWindow._maxRowId;
		ControlsHelper.SetFocus(_mtCode);
		_editableMode = true;
	}

	private void CleanAllField(bool isCleanItemCodeField = true)
	{
		if (isCleanItemCodeField)
		{
			_mtCode.Code = "";
			_qntUnit.Code = "";
			_price.EditValue = 0m;
			if (_existDiscountControls)
			{
				_discountPrice.EditValue = 0m;
			}
		}
		_quantity.EditValue = 0m;
		_summa.EditValue = 0m;
		if (_incomeWindow.PO.POType == DocumentTypeSE.MTIncomeRetail.Value)
		{
			_extraPercent.EditValue = 0m;
			if (isCleanItemCodeField)
			{
				_salePrice.EditValue = 0m;
			}
			_saleSumma.EditValue = 0m;
		}
		_description.EditValue = "";
		if (isCleanItemCodeField)
		{
			if (_vatTypeExist)
			{
				_vat.IsChecked = false;
			}
			if (_useLife != null)
			{
				_useLife.EditValue = null;
			}
			if (_country != null)
			{
				_country.Code = "";
			}
			if (_producer != null)
			{
				_producer.Code = "";
			}
			if (_property1 != null)
			{
				_property1.Code = "";
			}
			if (_property2 != null)
			{
				_property2.Code = "";
			}
		}
	}
}
