using System;
using System.Windows;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Extensions;
using ArmSoft.SME.Common.Parameters;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Core.UI.Controls;
using ArmSoft.SME.Core.UI.DataViews;
using ArmSoft.SME.Core.UI.Helpers;
using ArmSoft.SME.Trade.Common;
using ArmSoft.SME.Trade.Common.Models.Caches;
using ArmSoft.SME.Trade.Common.Parameters;
using ArmSoft.SME.Trade.Common.SmartEnums;
using ArmSoft.SME.Trade.UI.Documents;
using ArmSoft.SME.Trade.UI.DropDown;
using ArmSoft.SME.Trade.UI.DropDownDataProvider;
using ArmSoft.SME.Trade.UI.Helpers;
using DevExpress.Xpf.Editors;

namespace ArmSoft.SME.Trade.UI.OtherWindows;

public class MTIncomeRemRowAddEditDialog : DialogWindow
{
	private DropDownControl _storage;

	private DropDownControl _mtGroup;

	private DropDownControl _mtCode;

	private DropDownControl _qntUnit;

	private PopupCalcEditExt _quantity;

	private PopupCalcEditExt _price;

	private PopupCalcEditExt _summa;

	private CheckEditExt _vat;

	private PopupCalcEditExt _vatPrice;

	private PopupCalcEditExt _vatSumma;

	private PopupCalcEditExt _currPrice;

	private PopupCalcEditExt _currSumma;

	private PopupCalcEditExt _salePrice;

	private PopupCalcEditExt _saleSumma;

	private DateEditControl _useLife;

	private DropDownControl _country;

	private DropDownControl _producer;

	private DropDownControl _property1;

	private DropDownControl _property2;

	private int _rowID;

	private readonly MTIncomeRemWindow.MTIncomeRemSpecificationRow _sourceRow;

	private bool _editableMode;

	private readonly bool _allowAddRowAndNotViewMode;

	private readonly decimal _tradeConstVAT = Settings.ParametersService.VAT();

	private readonly MTIncomeRemWindow _incomeWindow;

	public int AddedRows { get; set; }

	public MTIncomeRemRowAddEditDialog(DialogWindowMode mode, MTIncomeRemWindow incomeRemWindow, MTIncomeRemWindow.MTIncomeRemSpecificationRow sourceRow = null)
	{
		Mode = mode;
		_incomeWindow = incomeRemWindow;
		_allowAddRowAndNotViewMode = _incomeWindow._mtIncomeRemGrid.AllowAdd && mode != DialogWindowMode.ViewMode;
		AddedRows = 0;
		Owner = incomeRemWindow;
		if (sourceRow != null)
		{
			_sourceRow = sourceRow;
		}
		Caption = ArmSoft.SME.Trade.Common.Resources.NewMaterialInMtIncome;
	}

	public bool? Invoke()
	{
		_storage = AddDropDownControl(_incomeWindow.Definition.Grids["MTList"].Columns["Storage"], new Storages());
		_storage.EditValueChanged += new SafeEventHandler<EditValueChangedEventArgs>(Storage_EditValueChanged).Handler;
		_mtGroup = AddDropDownControl("MTGroup", ArmSoft.SME.Common.Resources.Group, new GoodsGroups());
		_mtGroup.EditValueChanged += new SafeEventHandler<EditValueChangedEventArgs>(MTGroup_EditValueChanged).Handler;
		_mtCode = AddDropDownControl(_incomeWindow.Definition.Grids["MTList"].Columns["MTCode"], new GoodsFilter(shortMode: true, ItemTypesSE.Goods.Value), allowAdditionButtons: true);
		_mtCode.ClickDropDown += new SafeEventHandler<DropDownControlEventArgs>(MTCode_ClickDropDown).Handler;
		_mtCode.EditValueChanged += new SafeEventHandler<EditValueChangedEventArgs>(MTCode_EditValueChanged).Handler;
		_mtCode.EditAdditionalValueChanged += new SafeEventHandler<EventArgs>(MTCode_EditAdditionalValueChanged).Handler;
		_qntUnit = AddDropDownControl(_incomeWindow.Definition.Grids["MTList"].Columns["UnitCode"], new QntUnits());
		_qntUnit.ClickDropDown += new SafeEventHandler<DropDownControlEventArgs>(QntUnit_ClickDropDown).Handler;
		_qntUnit.EditValueChanged += new SafeEventHandler<EditValueChangedEventArgs>(QntUnit_EditValueChanged).Handler;
		_quantity = AddNumericControl(_incomeWindow.Definition.Grids["MTList"].Columns["Quantity"]);
		_quantity.EditValueChanged += new SafeEventHandler<EditValueChangedEventArgs>(Quantity_EditValueChanged).Handler;
		LayoutGroupExt container = AddHorizontalLayoutGroup();
		_price = AddNumericControl(container, _incomeWindow.Definition.Grids["MTList"].Columns["Price"]);
		_price.EditValueChanged += new SafeEventHandler<EditValueChangedEventArgs>(Price_EditValueChanged).Handler;
		((LayoutItemWithRequired)(object)_price.Parent).Label = ArmSoft.SME.Trade.Common.Resources.PriceSumma + ArmSoft.SME.Trade.Common.Resources.WithoutNDS;
		_summa = AddNumericControl(container, _incomeWindow.Definition.Grids["MTList"].Columns["Summa"]);
		_summa.IsRequired = !Settings.ParametersService.AllowPurchaseWith0PriceOr100Percent();
		_summa.EditValueChanged += new SafeEventHandler<EditValueChangedEventArgs>(Summa_EditValueChanged).Handler;
		((LayoutItemWithRequired)(object)_summa.Parent).Label = "";
		_vat = AddBooleanControl(_incomeWindow.Definition.Grids["MTList"].Columns["VAT"]);
		_vat.EditValueChanged += new SafeEventHandler<EditValueChangedEventArgs>(VAT_EditValueChanged).Handler;
		LayoutGroupExt container2 = AddHorizontalLayoutGroup();
		_vatPrice = AddNumericControl(container2, _incomeWindow.Definition.Grids["MTList"].Columns["VATPrice"]);
		_vatPrice.EditValueChanged += new SafeEventHandler<EditValueChangedEventArgs>(VATPrice_EditValueChanged).Handler;
		((LayoutItemWithRequired)(object)_vatPrice.Parent).Label = ArmSoft.SME.Trade.Common.Resources.PriceSumma + ArmSoft.SME.Trade.Common.Resources.WithNDS;
		_vatSumma = AddNumericControl(container2, _incomeWindow.Definition.Grids["MTList"].Columns["VATSumma"]);
		_vatSumma.EditValueChanged += new SafeEventHandler<EditValueChangedEventArgs>(VATSumma_EditValueChanged).Handler;
		_vatSumma.IsRequired = !Settings.ParametersService.AllowPurchaseWith0PriceOr100Percent();
		((LayoutItemWithRequired)(object)_vatSumma.Parent).Label = "";
		LayoutGroupExt container3 = AddHorizontalLayoutGroup();
		_currPrice = AddNumericControl(container3, _incomeWindow.Definition.Grids["MTList"].Columns["CurrPrice"]);
		_currPrice.EditValueChanged += new SafeEventHandler<EditValueChangedEventArgs>(CurrPrice_EditValueChanged).Handler;
		((LayoutItemWithRequired)(object)_currPrice.Parent).Label = ArmSoft.SME.Trade.Common.Resources.PriceSumma + "(" + Settings.ParametersService.MTCurr() + ")";
		_currSumma = AddNumericControl(container3, _incomeWindow.Definition.Grids["MTList"].Columns["CurrSumma"]);
		_currSumma.EditValueChanged += new SafeEventHandler<EditValueChangedEventArgs>(CurrSumma_EditValueChanged).Handler;
		_currSumma.IsRequired = !Settings.ParametersService.AllowPurchaseWith0PriceOr100Percent();
		((LayoutItemWithRequired)(object)_currSumma.Parent).Label = "";
		LayoutGroupExt container4 = AddHorizontalLayoutGroup();
		_salePrice = AddNumericControl(container4, _incomeWindow.Definition.Grids["MTList"].Columns["SalePrice"]);
		_salePrice.EditValueChanged += new SafeEventHandler<EditValueChangedEventArgs>(SalePrice_EditValueChanged).Handler;
		((LayoutItemWithRequired)(object)_salePrice.Parent).Label = ArmSoft.SME.Common.Resources.MTTAccSalePrice;
		_saleSumma = AddNumericControl(container4, _incomeWindow.Definition.Grids["MTList"].Columns["SaleSumma"]);
		_saleSumma.EditValueChanged += new SafeEventHandler<EditValueChangedEventArgs>(SaleSumma_EditValueChanged).Handler;
		((LayoutItemWithRequired)(object)_saleSumma.Parent).Label = "";
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
		WindowActivated += IncomeRemRow_WindowActivated;
		WindowValidation += new SafeEventHandler<DialogWindowValidationArgs>(IncomeRemRow_WindowValidation).Handler;
		NextPressed += new SafeEventHandler<EventArgs>(IncomeRemRow_NextPressed).Handler;
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

	private void IncomeRemRow_WindowActivated(DialogWindow window)
	{
		_editableMode = false;
		if (_sourceRow != null)
		{
			if (Mode == DialogWindowMode.NewModeWithNext)
			{
				_rowID = _incomeWindow._maxRowID;
			}
			else
			{
				_rowID = _sourceRow.RowID;
			}
			_storage.Code = _sourceRow.Storage;
			_mtCode.Code = _sourceRow.MTCode;
			_qntUnit.Code = _sourceRow.UnitCode;
			_quantity.Value = _sourceRow.Quantity;
			_price.Value = _sourceRow.Price;
			_summa.Value = _sourceRow.Summa;
			_vat.IsChecked = _sourceRow.VAT;
			_vatPrice.Value = _sourceRow.VATPrice;
			_vatSumma.Value = _sourceRow.VATSumma;
			_currPrice.Value = _sourceRow.CurrPrice;
			_currSumma.Value = _sourceRow.CurrSumma;
			_salePrice.Value = _sourceRow.SalePrice;
			_saleSumma.Value = _sourceRow.SaleSumma;
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
			_rowID = _incomeWindow._maxRowID;
		}
		if (Mode == DialogWindowMode.NewModeWithNext && string.IsNullOrWhiteSpace(_storage.Code))
		{
			_storage.Code = _incomeWindow._storage.Code;
		}
		ControlsHelper.SetFocus(_mtCode);
		_editableMode = true;
	}

	private void MTCode_EditAdditionalValueChanged(object sender, EventArgs e)
	{
		UpdateMaterialData();
	}

	private void Storage_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		if (Mode == DialogWindowMode.ViewMode || string.IsNullOrWhiteSpace(_storage.Code) || !_storage.DoValidate())
		{
			return;
		}
		if (!TradeSettings.CreateTradeClient().CommonServices.IsStorageAvailable(_storage.Code))
		{
			ArmSoft.SME.Core.UI.MessageBox.Show(string.Format(ArmSoft.SME.Trade.Common.Resources.StorageNotAvailForUser, Settings.LoginService.UserName, _storage.Code), ArmSoft.SME.Trade.Common.Resources.ProgramName, MessageBoxButton.OK, MessageBoxImage.Exclamation);
			_storage.Code = "";
			_storage.DoValidate();
			return;
		}
		bool flag = TradeSettings.TradeCacheService.Storage.GetItem(_storage.Code)?.Retail ?? false;
		_salePrice.IsReadOnly = !flag;
		_saleSumma.IsReadOnly = !flag;
		_salePrice.IsRequired = flag;
		_saleSumma.IsRequired = flag;
		if (!flag)
		{
			_salePrice.EditValue = 0m;
			_saleSumma.EditValue = 0m;
		}
	}

	private void MTGroup_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		if (_mtGroup.DoValidate() && !string.IsNullOrWhiteSpace(_mtCode.Code) && _mtCode.DoValidate() && TradeSettings.TradeCacheService.Material.GetItem(_mtCode.Code) != null)
		{
			_qntUnit.DataView = new MTQntUnits(_mtCode.Code);
		}
		if (!string.IsNullOrWhiteSpace(_mtGroup.Code) && !string.IsNullOrWhiteSpace(_mtCode.Code) && !_mtCode.DoValidate())
		{
			CleanAllFieldExceptStorage();
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
		GoodsFilter goodsFilter = new GoodsFilter(shortMode: true, ItemTypesSE.Goods.Value, _mtGroup.Code, isShowOnlyInPriceList: true);
		ModalBrowser modalBrowser = new ModalBrowser(goodsFilter)
		{
			LookUpValue = _mtCode.Code,
			Owner = e.Owner,
			Left = e.Left,
			Top = e.Top
		};
		if (modalBrowser.Show())
		{
			_mtCode.Code = goodsFilter.ValueMember(modalBrowser.SelectedRowIndex);
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
		if (!string.IsNullOrWhiteSpace(_qntUnit.Code) && _qntUnit.DoValidate() && !string.IsNullOrWhiteSpace(_mtCode.Code) && _mtCode.DoValidate() && _editableMode)
		{
			if (TradeSettings.TradeCacheService.Material.GetItem(_mtCode.Code) == null)
			{
				throw new ASException(string.Format(ArmSoft.SME.Trade.Common.Resources.MaterialNotExist, _mtCode.Code));
			}
			string text = e.OldValue.ToString();
			if (TradeSettings.TradeCacheService.QntUnit.GetItem(text) != null)
			{
				_editableMode = false;
				_price.EditValue = PriceHelper.GetPriceForUnit(text, _qntUnit.Code, _price.Value, _mtCode.Code);
				_summa.EditValue = (_price.Value * _quantity.Value).RoundAs(2);
				_summa.DoValidate();
				_vatPrice.EditValue = PriceHelper.GetPriceForUnit(text, _qntUnit.Code, _vatPrice.Value, _mtCode.Code);
				UpdateVATSumm();
				_currPrice.EditValue = PriceHelper.GetPriceForUnit(text, _qntUnit.Code, _currPrice.Value, _mtCode.Code);
				UpdateCurrSumm();
				_salePrice.EditValue = PriceHelper.GetPriceForUnit(text, _qntUnit.Code, _salePrice.Value, _mtCode.Code);
				UpdateSaleSumm();
				_editableMode = true;
			}
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
			_price.EditValue = 0m;
			_summa.EditValue = 0m;
			_vat.IsChecked = item.VAT;
			_vatPrice.EditValue = 0m;
			_vatSumma.EditValue = 0m;
			_currPrice.EditValue = 0m;
			_currSumma.Value = 0m;
			_salePrice.EditValue = 0m;
			_saleSumma.EditValue = 0m;
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
			_summa.EditValue = PriceHelper.CalculateSum(_price.Value, _quantity.Value);
			_summa.DoValidate();
			UpdateVAT_Curr_Sale_Summs();
			_editableMode = true;
		}
	}

	private void Price_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		if (e.NewValue != null && _editableMode)
		{
			_editableMode = false;
			_summa.EditValue = PriceHelper.CalculateSum(_price.Value, _quantity.Value);
			_summa.DoValidate();
			UpdateVATPrice();
			_editableMode = true;
		}
	}

	private void Summa_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		if (e.NewValue != null && _editableMode && _quantity.Value > 0m)
		{
			_editableMode = false;
			_price.EditValue = (_summa.Value / _quantity.Value).RoundAs(2);
			_price.DoValidate();
			UpdateVATPrice();
			_editableMode = true;
		}
	}

	private void VAT_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		if (_editableMode)
		{
			_editableMode = false;
			UpdateVATPrice();
			_editableMode = true;
		}
	}

	private void VATPrice_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		if (e.NewValue != null && _editableMode)
		{
			_editableMode = false;
			UpdateVATSumm();
			UpdateCurrPriceSumma();
			_editableMode = true;
		}
	}

	private void VATSumma_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		if (e.NewValue != null && _editableMode && _quantity.Value > 0m)
		{
			_editableMode = false;
			_vatPrice.EditValue = (_vatSumma.Value / _quantity.Value).RoundAs(2);
			_vatPrice.DoValidate();
			UpdateCurrPriceSumma();
			_editableMode = true;
		}
	}

	private void CurrPrice_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		if (e.NewValue != null && _editableMode)
		{
			_editableMode = false;
			UpdateCurrSumm();
			_editableMode = true;
		}
	}

	private void CurrSumma_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		if (e.NewValue != null && _editableMode && _quantity.Value > 0m)
		{
			_editableMode = false;
			_currPrice.EditValue = (_currSumma.Value / _quantity.Value).RoundAs(2);
			_currPrice.DoValidate();
			_editableMode = true;
		}
	}

	private void SalePrice_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		if (e.NewValue != null && _editableMode)
		{
			_editableMode = false;
			UpdateSaleSumm();
			_editableMode = true;
		}
	}

	private void SaleSumma_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		if (e.NewValue != null && _editableMode)
		{
			_editableMode = false;
			_salePrice.EditValue = (_saleSumma.Value / _quantity.Value).RoundAs(2);
			_salePrice.DoValidate();
			_editableMode = true;
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

	private void UpdateVAT_Curr_Sale_Summs()
	{
		UpdateVATSumm();
		UpdateCurrSumm();
		UpdateSaleSumm();
	}

	private void UpdateVATSumm()
	{
		_vatSumma.EditValue = PriceHelper.CalculateSum(_vatPrice.Value, _quantity.Value);
		_vatSumma.DoValidate();
	}

	private void UpdateCurrSumm()
	{
		_currSumma.EditValue = PriceHelper.CalculateSum(_currPrice.Value, _quantity.Value);
		_currSumma.DoValidate();
	}

	private void UpdateSaleSumm()
	{
		if (!_saleSumma.IsReadOnly)
		{
			_saleSumma.EditValue = PriceHelper.CalculateSum(_salePrice.Value, _quantity.Value);
			_saleSumma.DoValidate();
		}
	}

	private void UpdateVATPrice()
	{
		if (_vat.IsChecked.Value)
		{
			_vatPrice.EditValue = (_price.Value * (100m + _tradeConstVAT) / 100m).RoundAs(2);
		}
		else
		{
			_vatPrice.EditValue = _price.Value;
		}
		UpdateVATSumm();
		UpdateCurrPriceSumma();
	}

	private void UpdateCurrPriceSumma()
	{
		if (_incomeWindow._docCurrExChngRate != 0m)
		{
			_currPrice.EditValue = (_vatPrice.Value * _incomeWindow._docCurrExChngBase / _incomeWindow._docCurrExChngRate).RoundAs(2);
			UpdateCurrSumm();
		}
	}

	private void IncomeRemRow_WindowValidation(object sender, DialogWindowValidationArgs args)
	{
		try
		{
			if (_price.Value < 0m)
			{
				ControlsHelper.SetFocus(_price);
				throw new ASException(string.Format(ArmSoft.SME.Trade.Common.Resources.ValidatePriceIsPositive, default(ReadOnlySpan<object>)));
			}
			MTIncomeRemWindow.MTIncomeRemSpecificationRow mTIncomeRemSpecificationRow;
			if (Mode == DialogWindowMode.NewModeWithNext)
			{
				mTIncomeRemSpecificationRow = new MTIncomeRemWindow.MTIncomeRemSpecificationRow(_incomeWindow);
				_incomeWindow._mtIncomeRemList.Add(mTIncomeRemSpecificationRow);
			}
			else
			{
				mTIncomeRemSpecificationRow = _sourceRow;
			}
			DocumentGridAddingRowEventArgs e = new DocumentGridAddingRowEventArgs(_incomeWindow._mtIncomeRemGrid);
			MaterialCacheRow item = TradeSettings.TradeCacheService.Material.GetItem(_mtCode.Code);
			if (item == null)
			{
				throw new ASException(string.Format(ArmSoft.SME.Trade.Common.Resources.MaterialNotExist, _mtCode.Code));
			}
			mTIncomeRemSpecificationRow.Storage = _storage.Code;
			mTIncomeRemSpecificationRow.MTID = item.Id;
			mTIncomeRemSpecificationRow.MTCode = item.MTCode;
			mTIncomeRemSpecificationRow.MTName = item.Caption;
			mTIncomeRemSpecificationRow.UnitCode = _qntUnit.Code;
			QntUnitCacheRow item2 = TradeSettings.TradeCacheService.QntUnit.GetItem(mTIncomeRemSpecificationRow.UnitCode);
			if (item2 != null)
			{
				mTIncomeRemSpecificationRow.UnitBrief = item2.Brief;
			}
			else
			{
				mTIncomeRemSpecificationRow.UnitBrief = "";
			}
			mTIncomeRemSpecificationRow.Quantity = _quantity.Value;
			mTIncomeRemSpecificationRow.Price = _price.Value;
			mTIncomeRemSpecificationRow.Summa = _summa.Value;
			mTIncomeRemSpecificationRow.VAT = Convert.ToBoolean(_vat.IsChecked);
			mTIncomeRemSpecificationRow.VATPrice = _vatPrice.Value;
			mTIncomeRemSpecificationRow.VATSumma = _vatSumma.Value;
			mTIncomeRemSpecificationRow.CurrPrice = _currPrice.Value;
			mTIncomeRemSpecificationRow.CurrSumma = _currSumma.Value;
			mTIncomeRemSpecificationRow.SalePrice = _salePrice.Value;
			mTIncomeRemSpecificationRow.SaleSumma = _saleSumma.Value;
			if (_useLife != null && _useLife.EditValue.HasValue)
			{
				mTIncomeRemSpecificationRow.UseLife = _useLife.EditValue;
			}
			if (_country != null)
			{
				mTIncomeRemSpecificationRow.Country = _country.Code;
			}
			if (_producer != null)
			{
				mTIncomeRemSpecificationRow.Producer = _producer.Code;
			}
			if (_property1 != null)
			{
				mTIncomeRemSpecificationRow.Property1 = _property1.Code;
				mTIncomeRemSpecificationRow.Property1Caption = _property1.Comment;
			}
			if (_property2 != null)
			{
				mTIncomeRemSpecificationRow.Property2 = _property2.Code;
				mTIncomeRemSpecificationRow.Property2Caption = _property2.Comment;
			}
			if (Mode == DialogWindowMode.NewModeWithNext)
			{
				mTIncomeRemSpecificationRow.RowID = _rowID;
				_incomeWindow._maxRowID++;
				AddedRows++;
				e.RowNumber = _incomeWindow._mtIncomeRemList.Count - 1;
				_incomeWindow._mtIncomeRemGrid.InnerAddRow(e);
			}
			_incomeWindow._mtIncomeRemGrid.Grid.UpdateTotalSummary();
			_incomeWindow._mtIncomeRemGrid.View.CommitEditing();
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

	private void IncomeRemRow_NextPressed(object sender, EventArgs e)
	{
		_editableMode = false;
		CleanAllFieldExceptStorage(isCleanItemCodeField: false);
		_rowID = _incomeWindow._maxRowID;
		ControlsHelper.SetFocus(_mtCode);
		_editableMode = true;
	}

	private void CleanAllFieldExceptStorage(bool isCleanItemCodeField = true)
	{
		if (isCleanItemCodeField)
		{
			_mtCode.Code = "";
			_qntUnit.Code = "";
			_price.EditValue = 0m;
			_vatPrice.EditValue = 0m;
			_currPrice.EditValue = 0m;
			_salePrice.EditValue = 0m;
		}
		_quantity.EditValue = 0m;
		if (isCleanItemCodeField)
		{
			_vat.IsChecked = false;
		}
		_summa.EditValue = 0m;
		_vatSumma.EditValue = 0m;
		_currSumma.EditValue = 0m;
		_saleSumma.EditValue = 0m;
		if (isCleanItemCodeField)
		{
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
