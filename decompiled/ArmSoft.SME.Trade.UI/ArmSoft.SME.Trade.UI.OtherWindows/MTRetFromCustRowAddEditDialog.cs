using System;
using System.Linq;
using System.Windows;
using Ardalis.SmartEnum;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Extensions;
using ArmSoft.SME.Common.Models.SmartEnums;
using ArmSoft.SME.Common.Parameters;
using ArmSoft.SME.Common.SmartEnums;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Core.UI.Controls;
using ArmSoft.SME.Core.UI.DataViews;
using ArmSoft.SME.Core.UI.DropDowns;
using ArmSoft.SME.Core.UI.Helpers;
using ArmSoft.SME.Trade.Client.Data.Directories;
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

public class MTRetFromCustRowAddEditDialog : DialogWindow
{
	private DropDownControl _itemType;

	private DropDownControl _storage;

	private DropDownControl _mtGroup;

	private DropDownControl _mtCode;

	private DropDownControl _qntUnit;

	private PopupCalcEditExt _quantity;

	private PopupCalcEditExt _price;

	private PopupCalcEditExt _summa;

	private PopupCalcEditExt _costPrice;

	private PopupCalcEditExt _costPriceVAT;

	private PopupCalcEditExt _currCostPrice;

	private CheckEditExt _vat;

	private DropDownControl _dealType;

	private DateEditControl _useLife;

	private DropDownControl _country;

	private DropDownControl _producer;

	private DropDownControl _property1;

	private DropDownControl _property2;

	private int _rowID;

	private readonly MTReturnFromCustomerWindow.MTRetFromCustSpecificationRow _sourceRow;

	private bool _editableMode;

	private readonly bool _costColVisible;

	private bool _vatTypeExist;

	private readonly decimal _tradeConstVAT = Settings.ParametersService.VAT();

	private readonly MTReturnFromCustomerWindow _mtRetFromCustWindow;

	public int AddedRows { get; set; }

	public MTRetFromCustRowAddEditDialog(DialogWindowMode mode, MTReturnFromCustomerWindow mtRetFromCustWindow, MTReturnFromCustomerWindow.MTRetFromCustSpecificationRow sourceRow = null)
	{
		Mode = mode;
		_mtRetFromCustWindow = mtRetFromCustWindow;
		AddedRows = 0;
		_costColVisible = _mtRetFromCustWindow._currCostPriceCol.Visible;
		Owner = mtRetFromCustWindow;
		if (sourceRow != null)
		{
			_sourceRow = sourceRow;
		}
		Caption = ArmSoft.SME.Trade.Common.Resources.DocMTReturnFromCustomer;
	}

	public bool? Invoke()
	{
		_itemType = AddDropDownControl(_mtRetFromCustWindow.Definition.Grids["MTList"].Columns["ItemType"], new SmartEnum(typeof(GoodsTypesSE)));
		_itemType.EditValueChanged += new SafeEventHandler<EditValueChangedEventArgs>(ItemType_EditValueChanged).Handler;
		_storage = AddDropDownControl(_mtRetFromCustWindow.Definition.Grids["MTList"].Columns["Storage"], new Storages());
		_storage.Code = _mtRetFromCustWindow._storage.Code;
		_storage.EditValueChanged += new SafeEventHandler<EditValueChangedEventArgs>(Storage_EditValueChanged).Handler;
		_mtGroup = AddDropDownControl("MTGroup", ArmSoft.SME.Common.Resources.Group, new GoodsGroups());
		_mtGroup.EditValueChanged += new SafeEventHandler<EditValueChangedEventArgs>(MTGroup_EditValueChanged).Handler;
		_mtCode = AddDropDownControl(_mtRetFromCustWindow.Definition.Grids["MTList"].Columns["ItemCode"], new GoodsFilter(shortMode: true, ItemTypesSE.Goods.Value), allowAdditionButtons: true);
		_mtCode.ClickDropDown += new SafeEventHandler<DropDownControlEventArgs>(MTCode_ClickDropDown).Handler;
		_mtCode.EditValueChanged += new SafeEventHandler<EditValueChangedEventArgs>(MTCode_EditValueChanged).Handler;
		_mtCode.EditAdditionalValueChanged += new SafeEventHandler<EventArgs>(MTCode_EditAdditionalValueChanged).Handler;
		_qntUnit = AddDropDownControl(_mtRetFromCustWindow.Definition.Grids["MTList"].Columns["UnitCode"], new QntUnits());
		_qntUnit.ClickDropDown += new SafeEventHandler<DropDownControlEventArgs>(QntUnit_ClickDropDown).Handler;
		_qntUnit.EditValueChanged += new SafeEventHandler<EditValueChangedEventArgs>(QntUnit_EditValueChanged).Handler;
		_quantity = AddNumericControl(_mtRetFromCustWindow.Definition.Grids["MTList"].Columns["Quantity"]);
		_quantity.EditValueChanged += new SafeEventHandler<EditValueChangedEventArgs>(Quantity_EditValueChanged).Handler;
		LayoutGroupExt container = AddHorizontalLayoutGroup();
		_price = AddNumericControl(container, _mtRetFromCustWindow.Definition.Grids["MTList"].Columns["Price"]);
		((LayoutItemWithRequired)(object)_price.Parent).Label = ((_mtRetFromCustWindow._currCode.Code == "AMD") ? ArmSoft.SME.Trade.Common.Resources.PriceSumma : ArmSoft.SME.Trade.Common.Resources.PriceSummaCurr);
		_price.EditValueChanged += new SafeEventHandler<EditValueChangedEventArgs>(Price_EditValueChanged).Handler;
		_summa = AddNumericControl(container, _mtRetFromCustWindow.Definition.Grids["MTList"].Columns["Summa"]);
		_summa.EditValueChanged += new SafeEventHandler<EditValueChangedEventArgs>(Summa_EditValueChanged).Handler;
		((LayoutItemWithRequired)(object)_summa.Parent).Label = "";
		if (_costColVisible)
		{
			LayoutGroupExt container2 = AddVerticalLayoutGroup();
			_costPrice = AddNumericControl(container2, _mtRetFromCustWindow.Definition.Grids["MTList"].Columns["CostPrice"]);
			_costPrice.EditValueChanged += new SafeEventHandler<EditValueChangedEventArgs>(CostPrice_EditValueChanged).Handler;
			_costPriceVAT = AddNumericControl(container2, _mtRetFromCustWindow.Definition.Grids["MTList"].Columns["CostPriceVAT"]);
			_costPriceVAT.EditValueChanged += new SafeEventHandler<EditValueChangedEventArgs>(CostPriceVAT_EditValueChanged).Handler;
			_currCostPrice = AddNumericControl(container2, _mtRetFromCustWindow.Definition.Grids["MTList"].Columns["CurrCostPrice"]);
			((LayoutItemWithRequired)(object)_currCostPrice.Parent).Label = string.Format(ArmSoft.SME.Trade.Common.Resources.CostSummaCurr, _mtRetFromCustWindow._paramCurrCode);
		}
		_vatTypeExist = _mtRetFromCustWindow._vatType.Code.InList("1", "2", "5");
		if (_vatTypeExist)
		{
			_vat = AddBooleanControl(_mtRetFromCustWindow.Definition.Grids["MTList"].Columns["VAT"]);
			_vat.EditValueChanged += new SafeEventHandler<EditValueChangedEventArgs>(VAT_EditValueChanged).Handler;
		}
		_dealType = AddDropDownControl(_mtRetFromCustWindow.Definition.Grids["MTList"].Columns["DealType"], new SmartEnum(_vatTypeExist ? typeof(InvoiceDealTypesSE) : typeof(AccDealTypesSE), null, padLeft: false, '0', allSubTypes: false, ValueMember.ValueUI));
		if (_costColVisible && Settings.ParametersService.ShowPartyUSEABLEDATE())
		{
			_useLife = AddDateControl(_mtRetFromCustWindow.Definition.Grids["MTList"].Columns["UseLife"]);
		}
		if (_costColVisible && Settings.ParametersService.ShowPartyCountry())
		{
			_country = AddDropDownControl(_mtRetFromCustWindow.Definition.Grids["MTList"].Columns["Country"], new Countries());
		}
		if (_costColVisible && Settings.ParametersService.ShowPartyProducer())
		{
			_producer = AddDropDownControl(_mtRetFromCustWindow.Definition.Grids["MTList"].Columns["Producer"], new Producer());
		}
		if (_costColVisible && Settings.ParametersService.ShowPartyProperty1())
		{
			_property1 = AddDropDownControl(_mtRetFromCustWindow.Definition.Grids["MTList"].Columns["Property1"], new PropertyList());
			_property1.IsReadOnly = true;
			_property1.EditValueChanged += new SafeEventHandler<EditValueChangedEventArgs>(Property1_EditValueChanged).Handler;
			_property1.ClickAddDropDown += additionButtonsHandler;
			_property1.ClickEditDropDown += additionButtonsHandler;
		}
		if (_costColVisible && Settings.ParametersService.ShowPartyProperty2())
		{
			_property2 = AddDropDownControl(_mtRetFromCustWindow.Definition.Grids["MTList"].Columns["Property2"], new PropertyList());
			_property2.IsReadOnly = true;
			_property2.EditValueChanged += new SafeEventHandler<EditValueChangedEventArgs>(Property2_EditValueChanged).Handler;
			_property2.ClickAddDropDown += additionButtonsHandler2;
			_property2.ClickEditDropDown += additionButtonsHandler2;
		}
		WindowActivated += MTRetFromCustRow_WindowActivated;
		WindowValidation += new SafeEventHandler<DialogWindowValidationArgs>(MTRetFromCustRow_WindowValidation).Handler;
		NextPressed += new SafeEventHandler<EventArgs>(MTRetFromCustRow_NextPressed).Handler;
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

	private void MTRetFromCustRow_WindowActivated(DialogWindow window)
	{
		_editableMode = false;
		if (_sourceRow != null)
		{
			if (Mode == DialogWindowMode.NewModeWithNext)
			{
				_rowID = _mtRetFromCustWindow._maxRowID;
			}
			else
			{
				_rowID = _sourceRow.RowID;
			}
			_itemType.Code = _sourceRow.ItemType;
			_storage.Code = _sourceRow.Storage;
			_mtCode.Code = _sourceRow.ItemCode;
			_qntUnit.Code = _sourceRow.UnitCode;
			_quantity.Value = _sourceRow.Quantity;
			_price.Value = _sourceRow.Price;
			_summa.Value = _sourceRow.Summa;
			if (_vatTypeExist)
			{
				_vat.IsChecked = _sourceRow.VAT;
			}
			if (!string.IsNullOrWhiteSpace(_sourceRow.DealType))
			{
				if (_vatTypeExist)
				{
					if (_vat.IsChecked.Value)
					{
						_dealType.Code = "";
						_dealType.IsReadOnly = true;
					}
					else
					{
						_dealType.IsReadOnly = false;
						_dealType.Code = (from invoiceDealTypesSE in SmartEnum<InvoiceDealTypesSE, short>.List
							where invoiceDealTypesSE.Value.ToString() == _sourceRow.DealType.Trim()
							select invoiceDealTypesSE.ValueUI).FirstOrDefault().ToString();
					}
				}
				else
				{
					_dealType.IsReadOnly = false;
					_dealType.Code = (from accDealTypesSE in SmartEnum<AccDealTypesSE, short>.List
						where accDealTypesSE.Value.ToString() == _sourceRow.DealType.Trim()
						select accDealTypesSE.ValueUI).FirstOrDefault().ToString();
				}
			}
			if (_costColVisible)
			{
				_costPrice.Value = _sourceRow.CostPrice;
				_costPriceVAT.Value = _sourceRow.CostPriceVAT;
				_currCostPrice.Value = _sourceRow.CurrCostPrice;
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
			if (Mode == DialogWindowMode.ViewMode)
			{
				SetControlsReadonly(value: true);
			}
			else if (!_costColVisible)
			{
				_mtGroup.IsReadOnly = true;
				_mtCode.IsReadOnly = true;
				_qntUnit.IsReadOnly = true;
			}
		}
		else if (Mode == DialogWindowMode.NewModeWithNext)
		{
			_rowID = _mtRetFromCustWindow._maxRowID;
			if (string.IsNullOrWhiteSpace(_itemType.Code))
			{
				_itemType.Code = GoodsTypesSE.Good.Value;
			}
		}
		if (Mode == DialogWindowMode.NewModeWithNext)
		{
			ControlsHelper.SetFocus(_storage);
		}
		else
		{
			ControlsHelper.SetFocus(_mtCode);
		}
		_editableMode = true;
	}

	private void MTCode_EditAdditionalValueChanged(object sender, EventArgs e)
	{
		UpdateMaterialData();
	}

	private void ItemType_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		CleanAllField();
		if (!string.IsNullOrWhiteSpace(_itemType.Code) && _itemType.DoValidate())
		{
			ChangeItemTypeAdditonalView(e.NewValue.ToString());
		}
		if (_itemType.Code == GoodsTypesSE.Good.Value && string.IsNullOrWhiteSpace(_storage.Code))
		{
			_storage.Code = _mtRetFromCustWindow._storage.Code;
		}
		_itemType.DoValidate();
		_storage.DoValidate();
	}

	private void Storage_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		if (!(_itemType.Code == GoodsTypesSE.Service.Value) && Mode != DialogWindowMode.ViewMode && !string.IsNullOrWhiteSpace(_storage.Code) && _storage.DoValidate() && !TradeSettings.CreateTradeClient().CommonServices.IsStorageAvailable(_storage.Code))
		{
			ArmSoft.SME.Core.UI.MessageBox.Show(string.Format(ArmSoft.SME.Trade.Common.Resources.StorageNotAvailForUser, Settings.LoginService.UserName, _storage.Code), ArmSoft.SME.Trade.Common.Resources.ProgramName, MessageBoxButton.OK, MessageBoxImage.Exclamation);
			_storage.Code = "";
			_storage.DoValidate();
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
			CleanAllField();
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
			_qntUnit.DataView = new MTQntUnits(_mtGroup.Code, isShowQntUnitCaption: false, isShowOnlyComplects: false, "", SmartEnum<ItemTypesSE, string>.TryFromValue(_itemType.Code, out var _) ? _itemType.Code : ItemTypesSE.Goods.Value);
			_qntUnit.Code = "";
		}
	}

	private void MTCode_ClickDropDown(object sender, DropDownControlEventArgs e)
	{
		GoodsFilter goodsFilter = new GoodsFilter(shortMode: true, _itemType.Code, _mtGroup.Code, isShowOnlyInPriceList: true);
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
		MTQntUnits mTQntUnits = new MTQntUnits(_mtGroup.Code, isShowQntUnitCaption: false, isShowOnlyComplects: false, "", _itemType.Code);
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
		if (string.IsNullOrWhiteSpace(_mtRetFromCustWindow._priceListType.Code))
		{
			_price.EditValue = PriceHelper.GetPriceForUnit(e.OldValue.ToString(), _qntUnit.Code, _price.Value, _mtCode.Code);
			return;
		}
		PricesDataProvider.DataRow price = TradeSettings.CreateTradeClient().CommonServices.GetPrice(_mtRetFromCustWindow._priceListType.Code, item.Id, _qntUnit.Code, _mtRetFromCustWindow._date.EditValue.Value, _mtRetFromCustWindow._currCode.Code);
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
			_qntUnit.DataView = new MTQntUnits(_mtCode.Code, isShowQntUnitCaption: false, isShowOnlyComplects: false, "", _itemType.Code);
			_editableMode = false;
			_qntUnit.Code = item.Unit;
			_editableMode = true;
			_quantity.EditValue = 0m;
			_price.EditValue = TradeSettings.CreateTradeClient().CommonServices.GetPriceForGivenPriceListAndVATType(_mtRetFromCustWindow._priceListType.Code, _mtRetFromCustWindow._vatType.Code, item.Id, item.Unit, _mtRetFromCustWindow._date.EditValue.Value, _mtRetFromCustWindow._currCode.Code);
			_summa.EditValue = 0m;
			if (_vatTypeExist)
			{
				_vat.IsChecked = item.VAT;
				if (!_vat.IsChecked.Value)
				{
					_dealType.Code = _mtRetFromCustWindow._dealType.Code;
				}
			}
			else
			{
				_dealType.Code = _mtRetFromCustWindow._dealType.Code;
			}
			if (_costColVisible)
			{
				_costPrice.EditValue = 0m;
				_costPriceVAT.EditValue = 0m;
				_currCostPrice.EditValue = 0m;
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
			if (_sourceRow != null && (decimal)e.OldValue != 0m && _quantity.Value != 0m)
			{
				_sourceRow.UsedBonus = (_quantity.Value * _sourceRow.UsedBonus / (decimal)e.OldValue).RoundAs(2);
				_sourceRow.AccumulatedBonus = (_quantity.Value * _sourceRow.AccumulatedBonus / (decimal)e.OldValue).RoundAs(2);
			}
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
			_editableMode = true;
		}
	}

	private void CostPrice_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		if (e.NewValue != null && _editableMode)
		{
			_editableMode = false;
			if (_vatTypeExist && _vat.IsChecked.Value)
			{
				_costPriceVAT.EditValue = (_costPriceVAT.Value * (100m + _tradeConstVAT) / 100m).RoundAs(2);
			}
			else
			{
				_costPriceVAT.EditValue = _costPrice.Value;
			}
			_costPriceVAT.DoValidate();
			_currCostPrice.EditValue = (_costPriceVAT.Value * _mtRetFromCustWindow._paramCurrExChngBase / _mtRetFromCustWindow._paramCurrExChngRate).RoundAs(2);
			_currCostPrice.DoValidate();
			_editableMode = true;
		}
	}

	private void CostPriceVAT_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		if (e.NewValue != null && _editableMode)
		{
			_editableMode = false;
			_currCostPrice.EditValue = (_costPriceVAT.Value * _mtRetFromCustWindow._paramCurrExChngBase / _mtRetFromCustWindow._paramCurrExChngRate).RoundAs(2);
			_currCostPrice.DoValidate();
			_editableMode = true;
		}
	}

	private void VAT_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		if (e.NewValue != null && _editableMode && _costColVisible)
		{
			_editableMode = false;
			if (_vat.IsChecked.Value)
			{
				_costPriceVAT.EditValue = (_costPrice.Value * (100m + _tradeConstVAT) / 100m).RoundAs(2);
			}
			else
			{
				_costPriceVAT.EditValue = _costPrice.Value;
			}
			_costPriceVAT.DoValidate();
			_currCostPrice.EditValue = (_costPriceVAT.Value * _mtRetFromCustWindow._paramCurrExChngBase / _mtRetFromCustWindow._paramCurrExChngRate).RoundAs(2);
			_currCostPrice.DoValidate();
			_editableMode = true;
		}
		if (_vat.IsChecked.Value)
		{
			_dealType.IsEnabled = false;
			_dealType.Code = "";
		}
		else
		{
			_dealType.IsEnabled = true;
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

	private void MTRetFromCustRow_WindowValidation(object sender, DialogWindowValidationArgs args)
	{
		try
		{
			if (_price.Value < 0m)
			{
				ControlsHelper.SetFocus(_price);
				throw new ASException(string.Format(ArmSoft.SME.Trade.Common.Resources.ValidatePriceIsPositive, default(ReadOnlySpan<object>)));
			}
			MTReturnFromCustomerWindow.MTRetFromCustSpecificationRow mTRetFromCustSpecificationRow;
			if (Mode == DialogWindowMode.NewModeWithNext)
			{
				mTRetFromCustSpecificationRow = new MTReturnFromCustomerWindow.MTRetFromCustSpecificationRow(_mtRetFromCustWindow);
				_mtRetFromCustWindow._mtRetFromCustList.Add(mTRetFromCustSpecificationRow);
			}
			else
			{
				mTRetFromCustSpecificationRow = _sourceRow;
			}
			DocumentGridAddingRowEventArgs e = new DocumentGridAddingRowEventArgs(_mtRetFromCustWindow._mtRetFromCustGrid);
			MaterialCacheRow item = TradeSettings.TradeCacheService.Material.GetItem(_mtCode.Code);
			if (item == null)
			{
				throw new ASException(string.Format(ArmSoft.SME.Trade.Common.Resources.MaterialNotExist, _mtCode.Code));
			}
			mTRetFromCustSpecificationRow.ItemType = _itemType.Code;
			mTRetFromCustSpecificationRow.Storage = _storage.Code;
			mTRetFromCustSpecificationRow.ItemID = item.Id;
			mTRetFromCustSpecificationRow.ItemCode = item.MTCode;
			mTRetFromCustSpecificationRow.ItemName = item.Caption;
			mTRetFromCustSpecificationRow.UnitCode = _qntUnit.Code;
			mTRetFromCustSpecificationRow.UnitBrief = TradeSettings.TradeCacheService.QntUnit.GetItem(mTRetFromCustSpecificationRow.UnitCode)?.Brief ?? "";
			mTRetFromCustSpecificationRow.Quantity = _quantity.Value;
			mTRetFromCustSpecificationRow.Price = _price.Value;
			mTRetFromCustSpecificationRow.Summa = _summa.Value;
			if (_vatTypeExist)
			{
				mTRetFromCustSpecificationRow.VAT = Convert.ToBoolean(_vat.IsChecked);
			}
			if (!string.IsNullOrWhiteSpace(_dealType.Code))
			{
				string text = ((!_vatTypeExist) ? (from accDealTypesSE in SmartEnum<AccDealTypesSE, short>.List
					where accDealTypesSE.ValueUI.Trim() == _dealType.Code.Trim()
					select accDealTypesSE.Value).FirstOrDefault().ToString() : (from invoiceDealTypesSE in SmartEnum<InvoiceDealTypesSE, short>.List
					where invoiceDealTypesSE.ValueUI.Trim() == _dealType.Code.Trim()
					select invoiceDealTypesSE.Value).FirstOrDefault().ToString());
				if (text != null)
				{
					mTRetFromCustSpecificationRow.DealType = text;
				}
			}
			if (_costColVisible)
			{
				mTRetFromCustSpecificationRow.CostPrice = _costPrice.Value;
				mTRetFromCustSpecificationRow.CostPriceVAT = _costPriceVAT.Value;
				mTRetFromCustSpecificationRow.CurrCostPrice = _currCostPrice.Value;
				if (_useLife != null && _useLife.EditValue.HasValue)
				{
					mTRetFromCustSpecificationRow.UseLife = _useLife.EditValue;
				}
				if (_country != null)
				{
					mTRetFromCustSpecificationRow.Country = _country.Code;
				}
				if (_producer != null)
				{
					mTRetFromCustSpecificationRow.Producer = _producer.Code;
				}
				if (_property1 != null)
				{
					mTRetFromCustSpecificationRow.Property1 = _property1.Code;
					mTRetFromCustSpecificationRow.Property1Caption = _property1.Comment;
				}
				if (_property2 != null)
				{
					mTRetFromCustSpecificationRow.Property2 = _property2.Code;
					mTRetFromCustSpecificationRow.Property2Caption = _property2.Comment;
				}
			}
			if (Mode == DialogWindowMode.NewModeWithNext)
			{
				mTRetFromCustSpecificationRow.RowID = _rowID;
				_mtRetFromCustWindow._maxRowID++;
				AddedRows++;
				e.RowNumber = _mtRetFromCustWindow._mtRetFromCustList.Count - 1;
				_mtRetFromCustWindow._mtRetFromCustGrid.InnerAddRow(e);
			}
			_mtRetFromCustWindow._mtRetFromCustGrid.Grid.UpdateTotalSummary();
			_mtRetFromCustWindow._mtRetFromCustGrid.View.CommitEditing();
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

	private void MTRetFromCustRow_NextPressed(object sender, EventArgs e)
	{
		_editableMode = false;
		CleanAllField(isCleanItemCodeField: false);
		_rowID = _mtRetFromCustWindow._maxRowID;
		ControlsHelper.SetFocus(_mtCode);
		_editableMode = true;
	}

	private void CleanAllField(bool isCleanItemCodeField = true)
	{
		if (isCleanItemCodeField)
		{
			_mtCode.Code = "";
			_qntUnit.Code = "";
		}
		_quantity.EditValue = 0m;
		_price.EditValue = 0m;
		_summa.EditValue = 0m;
		if (isCleanItemCodeField && _vatTypeExist)
		{
			_vat.IsChecked = false;
		}
		_dealType.Code = "";
		if (!_costColVisible)
		{
			return;
		}
		_costPrice.EditValue = 0m;
		_costPriceVAT.EditValue = 0m;
		_currCostPrice.EditValue = 0m;
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

	private void ChangeItemTypeAdditonalView(string typeValue)
	{
		if (SmartEnum<GoodsTypesSE, string>.TryFromValue(typeValue, out var result))
		{
			bool flag = result == GoodsTypesSE.Good;
			_storage.Code = "";
			_storage.IsRequired = flag;
			_storage.IsReadOnly = !flag;
			_mtCode.DataView = new GoodsFilter(shortMode: true, result.Value, _mtGroup.Code, isShowOnlyInPriceList: true);
			_qntUnit.DataView = new MTQntUnits(_mtCode.Code, isShowQntUnitCaption: false, isShowOnlyComplects: false, "", result.Value);
		}
	}
}
