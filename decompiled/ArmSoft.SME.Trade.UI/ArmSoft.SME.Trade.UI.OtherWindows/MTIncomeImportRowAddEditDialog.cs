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
using ArmSoft.SME.Trade.Common.Parameters;
using ArmSoft.SME.Trade.Common.SmartEnums;
using ArmSoft.SME.Trade.UI.Documents;
using ArmSoft.SME.Trade.UI.DropDown;
using ArmSoft.SME.Trade.UI.DropDownDataProvider;
using ArmSoft.SME.Trade.UI.Helpers;
using DevExpress.Xpf.Editors;

namespace ArmSoft.SME.Trade.UI.OtherWindows;

public class MTIncomeImportRowAddEditDialog : DialogWindow
{
	private DropDownControl _mtGroup;

	private DropDownControl _mtCode;

	private DropDownControl _qntUnit;

	private PopupCalcEditExt _quantity;

	private PopupCalcEditExt _price;

	private PopupCalcEditExt _summa;

	private PopupCalcEditExt _customSumma;

	private PopupCalcEditExt _customPercent;

	private PopupCalcEditExt _customDuty;

	private CheckEditExt _vat;

	private PopupCalcEditExt _vatSumma;

	private DateEditControl _useLife;

	private DropDownControl _country;

	private DropDownControl _producer;

	private DropDownControl _property1;

	private DropDownControl _property2;

	private int _rowID;

	private readonly MTIncomeImportWindow.MTIncomeImportSpecificationRow _sourceRow;

	private bool _editableMode;

	private readonly bool _allowAddRowAndNotViewMode;

	private readonly decimal _tradeConstVAT = Settings.ParametersService.VAT();

	private readonly MTIncomeImportWindow _incomeImport;

	public int AddedRows { get; set; }

	public MTIncomeImportRowAddEditDialog(DialogWindowMode mode, MTIncomeImportWindow incomeImportWindow, MTIncomeImportWindow.MTIncomeImportSpecificationRow sourceRow = null)
	{
		Mode = mode;
		_incomeImport = incomeImportWindow;
		_allowAddRowAndNotViewMode = _incomeImport._mtIncomeImportGrid.AllowAdd && mode != DialogWindowMode.ViewMode;
		AddedRows = 0;
		Owner = incomeImportWindow;
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
		_mtCode = AddDropDownControl(_incomeImport.Definition.Grids["MTList"].Columns["MTCode"], new GoodsFilter(shortMode: true, ItemTypesSE.Goods.Value), allowAdditionButtons: true);
		_mtCode.ClickDropDown += new SafeEventHandler<DropDownControlEventArgs>(MTCode_ClickDropDown).Handler;
		_mtCode.EditValueChanged += new SafeEventHandler<EditValueChangedEventArgs>(MTCode_EditValueChanged).Handler;
		_mtCode.EditAdditionalValueChanged += new SafeEventHandler<EventArgs>(MTCode_EditAdditionalValueChanged).Handler;
		_qntUnit = AddDropDownControl(_incomeImport.Definition.Grids["MTList"].Columns["UnitCode"], new QntUnits());
		_qntUnit.ClickDropDown += new SafeEventHandler<DropDownControlEventArgs>(QntUnit_ClickDropDown).Handler;
		_qntUnit.EditValueChanged += new SafeEventHandler<EditValueChangedEventArgs>(QntUnit_EditValueChanged).Handler;
		_quantity = AddNumericControl(_incomeImport.Definition.Grids["MTList"].Columns["Quantity"]);
		_quantity.EditValueChanged += new SafeEventHandler<EditValueChangedEventArgs>(Quantity_EditValueChanged).Handler;
		LayoutGroupExt container = AddHorizontalLayoutGroup();
		_price = AddNumericControl(container, _incomeImport.Definition.Grids["MTList"].Columns["Price"]);
		((LayoutItemWithRequired)(object)_price.Parent).Label = ((_incomeImport._currCode.Code == "AMD") ? ArmSoft.SME.Trade.Common.Resources.PriceSumma : ArmSoft.SME.Trade.Common.Resources.PriceSummaCurr);
		_price.EditValueChanged += new SafeEventHandler<EditValueChangedEventArgs>(Price_EditValueChanged).Handler;
		_summa = AddNumericControl(container, _incomeImport.Definition.Grids["MTList"].Columns["Summa"]);
		_summa.EditValueChanged += new SafeEventHandler<EditValueChangedEventArgs>(Summa_EditValueChanged).Handler;
		_customSumma = AddNumericControl(_incomeImport.Definition.Grids["MTList"].Columns["CustomSumma"]);
		_customSumma.EditValueChanged += new SafeEventHandler<EditValueChangedEventArgs>(CustomSumma_EditValueChanged).Handler;
		LayoutGroupExt container2 = AddHorizontalLayoutGroup();
		_customPercent = AddNumericControl(container2, _incomeImport.Definition.Grids["MTList"].Columns["CustomPercent"]);
		_customPercent.EditValueChanged += new SafeEventHandler<EditValueChangedEventArgs>(CustomPercent_EditValueChanged).Handler;
		_customDuty = AddNumericControl(container2, _incomeImport.Definition.Grids["MTList"].Columns["CustomDuty"]);
		_customDuty.EditValueChanged += new SafeEventHandler<EditValueChangedEventArgs>(CustomDuty_EditValueChanged).Handler;
		_vat = AddBooleanControl(_incomeImport.Definition.Grids["MTList"].Columns["VAT"]);
		_vat.EditValueChanged += new SafeEventHandler<EditValueChangedEventArgs>(VAT_EditValueChanged).Handler;
		_vatSumma = AddNumericControl(_incomeImport.Definition.Grids["MTList"].Columns["VATSumma"]);
		if (Settings.ParametersService.ShowPartyUSEABLEDATE())
		{
			_useLife = AddDateControl(_incomeImport.Definition.Grids["MTList"].Columns["UseLife"]);
		}
		if (Settings.ParametersService.ShowPartyCountry())
		{
			_country = AddDropDownControl(_incomeImport.Definition.Grids["MTList"].Columns["Country"], new Countries());
		}
		if (Settings.ParametersService.ShowPartyProducer())
		{
			_producer = AddDropDownControl(_incomeImport.Definition.Grids["MTList"].Columns["Producer"], new Producer());
		}
		if (Settings.ParametersService.ShowPartyProperty1())
		{
			_property1 = AddDropDownControl(_incomeImport.Definition.Grids["MTList"].Columns["Property1"], new PropertyList());
			_property1.IsReadOnly = true;
			_property1.EditValueChanged += new SafeEventHandler<EditValueChangedEventArgs>(Property1_EditValueChanged).Handler;
			_property1.ClickAddDropDown += additionButtonsHandler;
			_property1.ClickEditDropDown += additionButtonsHandler;
		}
		if (Settings.ParametersService.ShowPartyProperty2())
		{
			_property2 = AddDropDownControl(_incomeImport.Definition.Grids["MTList"].Columns["Property2"], new PropertyList());
			_property2.IsReadOnly = true;
			_property2.EditValueChanged += new SafeEventHandler<EditValueChangedEventArgs>(Property2_EditValueChanged).Handler;
			_property2.ClickAddDropDown += additionButtonsHandler2;
			_property2.ClickEditDropDown += additionButtonsHandler2;
		}
		WindowActivated += IncomeImportRow_WindowActivated;
		WindowValidation += new SafeEventHandler<DialogWindowValidationArgs>(IncomeImportRow_WindowValidation).Handler;
		NextPressed += new SafeEventHandler<EventArgs>(IncomeImportRow_NextPressed).Handler;
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

	private void IncomeImportRow_WindowActivated(DialogWindow window)
	{
		_editableMode = false;
		if (_sourceRow != null)
		{
			if (Mode == DialogWindowMode.NewModeWithNext)
			{
				_rowID = _incomeImport._maxRowID;
			}
			else
			{
				_rowID = _sourceRow.RowID;
			}
			_mtCode.Code = _sourceRow.MTCode;
			_qntUnit.Code = _sourceRow.UnitCode;
			_quantity.Value = _sourceRow.Quantity;
			_price.Value = _sourceRow.Price;
			_summa.Value = _sourceRow.Summa;
			_customSumma.Value = _sourceRow.CustomSumma;
			_customPercent.Value = _sourceRow.CustomPercent;
			_customDuty.Value = _sourceRow.CustomDuty;
			_vat.IsChecked = _sourceRow.VAT;
			_vatSumma.Value = _sourceRow.VATSumma;
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
			_rowID = _incomeImport._maxRowID;
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
		if (string.IsNullOrWhiteSpace(_incomeImport._priceListType.Code))
		{
			_price.EditValue = PriceHelper.GetPriceForUnit(e.OldValue.ToString(), _qntUnit.Code, _price.Value, _mtCode.Code);
			return;
		}
		PricesDataProvider.DataRow price = TradeSettings.CreateTradeClient().CommonServices.GetPrice(_incomeImport._priceListType.Code, item.Id, _qntUnit.Code, _incomeImport._date.EditValue.Value, _incomeImport._currCode.Code);
		if (price.Price != 0m)
		{
			_price.EditValue = price.Price;
			return;
		}
		_price.EditValue = PriceHelper.GetPriceForUnit(e.OldValue.ToString().ToString(), _qntUnit.Code, _price.Value, _mtCode.Code);
		decimal priceForGivenPriceListAndVATType = TradeSettings.CreateTradeClient().CommonServices.GetPriceForGivenPriceListAndVATType(_incomeImport._priceListType.Code, "1", item.Id, _qntUnit.Code, _incomeImport._date.EditValue.Value, _incomeImport._currCode.Code);
		if (priceForGivenPriceListAndVATType != 0m)
		{
			_price.EditValue = priceForGivenPriceListAndVATType;
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
			_qntUnit.Code = TradeSettings.CreateTradeClient().CommonServices.GetMTQntUnitByMTIDPriceTypeAndDocDate(item.Id, _incomeImport._priceListType.Code, _incomeImport._date.EditValue.Value);
			if (string.IsNullOrWhiteSpace(_qntUnit.Code))
			{
				_qntUnit.Code = item.Unit;
			}
			_editableMode = true;
			_quantity.EditValue = 0m;
			_price.EditValue = TradeSettings.CreateTradeClient().CommonServices.GetPriceForGivenPriceListAndVATType(_incomeImport._priceListType.Code, "1", item.Id, item.Unit, _incomeImport._date.EditValue.Value, _incomeImport._currCode.Code);
			_summa.EditValue = 0m;
			_customSumma.EditValue = 0m;
			_customDuty.EditValue = 0m;
			_vat.IsChecked = item.VAT;
			_vatSumma.EditValue = 0m;
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
			UpdateCustomSummaAndDuty();
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
			UpdateCustomSummaAndDuty();
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
			UpdateCustomSummaAndDuty();
			_editableMode = true;
		}
	}

	private void CustomSumma_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		if (e.NewValue != null && _editableMode)
		{
			_editableMode = false;
			UpdateCustomDuty();
			_editableMode = true;
		}
	}

	private void CustomPercent_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		if (e.NewValue != null && _editableMode)
		{
			_editableMode = false;
			UpdateCustomDuty();
			_editableMode = true;
		}
	}

	private void CustomDuty_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		if (e.NewValue != null && _editableMode)
		{
			_editableMode = false;
			UpdateVATSumma();
			_editableMode = true;
		}
	}

	private void VAT_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		if (_editableMode)
		{
			_editableMode = false;
			UpdateVATSumma();
			_editableMode = true;
		}
	}

	private void UpdateCustomSummaAndDuty()
	{
		decimal num = _summa.Value;
		if (_incomeImport._currCode.Code != "AMD" && _incomeImport._currExChngRate.Base != 0m)
		{
			num = (num * _incomeImport._currExChngRate.Rate / _incomeImport._currExChngRate.Base).RoundAs(2);
		}
		_customSumma.EditValue = num;
		_customSumma.DoValidate();
		UpdateCustomDuty();
	}

	private void UpdateCustomDuty()
	{
		if (_customSumma.DoValidate())
		{
			_customDuty.EditValue = (_customSumma.Value * _customPercent.Value / 100m).RoundAs(2);
		}
		else
		{
			_customDuty.EditValue = 0m;
		}
		_customDuty.DoValidate();
		UpdateVATSumma();
	}

	private void UpdateVATSumma()
	{
		decimal num = (_vat.IsChecked.Value ? _tradeConstVAT : 0m);
		_vatSumma.EditValue = ((_customSumma.Value + _customDuty.Value) * num / 100m).RoundAs(2);
		_vatSumma.DoValidate();
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

	private void IncomeImportRow_WindowValidation(object sender, DialogWindowValidationArgs args)
	{
		try
		{
			if (_price.Value < 0m)
			{
				ControlsHelper.SetFocus(_price);
				throw new ASException(string.Format(ArmSoft.SME.Trade.Common.Resources.ValidatePriceIsPositive, default(ReadOnlySpan<object>)));
			}
			MTIncomeImportWindow.MTIncomeImportSpecificationRow mTIncomeImportSpecificationRow;
			if (Mode == DialogWindowMode.NewModeWithNext)
			{
				mTIncomeImportSpecificationRow = new MTIncomeImportWindow.MTIncomeImportSpecificationRow();
				_incomeImport._mtIncomeImportList.Add(mTIncomeImportSpecificationRow);
			}
			else
			{
				mTIncomeImportSpecificationRow = _sourceRow;
			}
			DocumentGridAddingRowEventArgs e = new DocumentGridAddingRowEventArgs(_incomeImport._mtIncomeImportGrid);
			MaterialCacheRow item = TradeSettings.TradeCacheService.Material.GetItem(_mtCode.Code);
			if (item == null)
			{
				throw new ASException(string.Format(ArmSoft.SME.Trade.Common.Resources.MaterialNotExist, _mtCode.Code));
			}
			mTIncomeImportSpecificationRow.MTID = item.Id;
			mTIncomeImportSpecificationRow.MTCode = item.MTCode;
			mTIncomeImportSpecificationRow.MTName = item.Caption;
			mTIncomeImportSpecificationRow.UnitCode = _qntUnit.Code;
			QntUnitCacheRow item2 = TradeSettings.TradeCacheService.QntUnit.GetItem(mTIncomeImportSpecificationRow.UnitCode);
			if (item2 != null)
			{
				mTIncomeImportSpecificationRow.UnitBrief = item2.Brief;
			}
			else
			{
				mTIncomeImportSpecificationRow.UnitBrief = "";
			}
			mTIncomeImportSpecificationRow.Quantity = _quantity.Value;
			mTIncomeImportSpecificationRow.Price = _price.Value;
			mTIncomeImportSpecificationRow.Summa = _summa.Value;
			mTIncomeImportSpecificationRow.CustomSumma = _customSumma.Value;
			mTIncomeImportSpecificationRow.CustomPercent = _customPercent.Value;
			mTIncomeImportSpecificationRow.CustomDuty = _customDuty.Value;
			mTIncomeImportSpecificationRow.VAT = Convert.ToBoolean(_vat.IsChecked);
			mTIncomeImportSpecificationRow.VATSumma = _vatSumma.Value;
			if (_useLife != null && _useLife.EditValue.HasValue)
			{
				mTIncomeImportSpecificationRow.UseLife = _useLife.EditValue;
			}
			if (_country != null)
			{
				mTIncomeImportSpecificationRow.Country = _country.Code;
			}
			if (_producer != null)
			{
				mTIncomeImportSpecificationRow.Producer = _producer.Code;
			}
			if (_property1 != null)
			{
				mTIncomeImportSpecificationRow.Property1 = _property1.Code;
				mTIncomeImportSpecificationRow.Property1Caption = _property1.Comment;
			}
			if (_property2 != null)
			{
				mTIncomeImportSpecificationRow.Property2 = _property2.Code;
				mTIncomeImportSpecificationRow.Property2Caption = _property2.Comment;
			}
			if (Mode == DialogWindowMode.NewModeWithNext)
			{
				mTIncomeImportSpecificationRow.RowID = _rowID;
				_incomeImport._maxRowID++;
				AddedRows++;
				e.RowNumber = _incomeImport._mtIncomeImportList.Count - 1;
				_incomeImport._mtIncomeImportGrid.InnerAddRow(e);
			}
			_incomeImport._mtIncomeImportGrid.Grid.UpdateTotalSummary();
			_incomeImport._mtIncomeImportGrid.View.CommitEditing();
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

	private void IncomeImportRow_NextPressed(object sender, EventArgs e)
	{
		_editableMode = false;
		CleanAllField(isCleanItemCodeField: false);
		_rowID = _incomeImport._maxRowID;
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
		}
		_quantity.EditValue = 0m;
		_summa.EditValue = 0m;
		_customSumma.EditValue = 0m;
		_customDuty.EditValue = 0m;
		if (isCleanItemCodeField)
		{
			_vat.IsChecked = false;
			_vatSumma.EditValue = 0m;
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
