using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Enums;
using ArmSoft.SME.Common.Extensions;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Core.UI.Controls;
using ArmSoft.SME.Core.UI.DropDowns;
using ArmSoft.SME.Core.UI.Services;
using ArmSoft.SME.Trade.Common;
using ArmSoft.SME.Trade.Common.Models.Caches;
using ArmSoft.SME.Trade.Common.Models.Documents;
using ArmSoft.SME.Trade.Common.Models.PrintableElements;
using ArmSoft.SME.Trade.Common.Parameters;
using ArmSoft.SME.Trade.Common.SmartEnums;
using ArmSoft.SME.Trade.UI.CustomUIElements.UserControls;
using ArmSoft.SME.Trade.UI.DropDown;
using ArmSoft.SME.Trade.UI.Helpers;
using DevExpress.Utils;
using DevExpress.Xpf.Editors;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.LayoutControl;
using Microsoft.Extensions.DependencyInjection;

namespace ArmSoft.SME.Trade.UI.CustomUIElements;

public class PrintLabelDialog : DialogWindow
{
	private Window _owner;

	private List<Label.GoodLabelRow> _selectedGoods;

	private TradeDocument _doc;

	private DocumentGridControl _mtListGrid;

	private TableView _mtListView;

	private DropDownControl _good;

	private DropDownControl _unit;

	private DropDownControl _altUnit;

	private DateEditControl _date;

	private DropDownControl _priceType;

	private DropDownControl _discountPriceType;

	private DropDownControl _avgPriceType;

	private DropDownControl _template;

	private CheckEditExt _groupByCode;

	private CheckEditExt _forAltUnit;

	private CheckEditExt _printImmediately;

	private DocumentGridColumn _includeCol;

	private DocumentGridColumn _mtCodeCol;

	private DocumentGridColumn _mtNameCol;

	private DocumentGridColumn _priceCol;

	private DocumentGridColumn _quantityCol;

	private DocumentGridColumn _unitCol;

	private DocumentGridColumn _useLifeCol;

	private DocumentGridColumn _countryCol;

	private DocumentGridColumn _producerCol;

	private DocumentGridColumn _property1Col;

	private DocumentGridColumn _property2Col;

	private CheckEditExt _takeRowQty;

	private SpinEditExt _quantity;

	private List<Label.PrintLabelWindowGridRow> _mtList;

	private bool _isFromGoodsAvailability;

	private bool _isFromSalesAvgPrice;

	private bool isMTIncomeRetailDoc = true;

	private bool isMTIncomeDoc;

	public PrintLabelDialog(Window ownWindow = null, TradeDocument doc = null, List<Label.GoodLabelRow> selectedGoods = null, bool isFromGoodsAvailability = false, bool isFromSalesAvgPrice = false)
	{
		_owner = ownWindow;
		_doc = doc;
		_isFromGoodsAvailability = isFromGoodsAvailability;
		_isFromSalesAvgPrice = isFromSalesAvgPrice;
		_mtList = new List<Label.PrintLabelWindowGridRow>();
		if (_doc != null)
		{
			switch (_doc.POType)
			{
			case 7:
			case 20:
				foreach (MTIncomeBase.MTIncomeSpecificationRow mT in ((MTIncomeBase)_doc).MTList)
				{
					_mtList.Add(new Label.PrintLabelWindowGridRow
					{
						Include = true,
						MTID = mT.MTID,
						MTCode = mT.MTCode,
						MTName = mT.MTName,
						RowID = mT.RowID,
						Unit = mT.UnitCode,
						UnitBrief = mT.UnitBrief,
						UnitFromDoc = mT.UnitCode,
						UnitBriefFromDoc = mT.UnitBrief,
						Price = mT.SalePrice,
						Quantity = mT.Quantity,
						UseLife = mT.UseLife,
						Country = mT.Country,
						Producer = mT.Producer,
						Property1 = mT.Property1,
						Property1Caption = mT.Property1Caption,
						Property2 = mT.Property2,
						Property2Caption = mT.Property2Caption
					});
				}
				break;
			case 19:
				foreach (MTIncomeImport.MTIncomeImportSpecificationRow mT2 in ((MTIncomeImport)_doc).MTList)
				{
					_mtList.Add(new Label.PrintLabelWindowGridRow
					{
						Include = true,
						MTID = mT2.MTID,
						MTCode = mT2.MTCode,
						MTName = mT2.MTName,
						RowID = mT2.RowID,
						Unit = mT2.UnitCode,
						UnitBrief = mT2.UnitBrief,
						UnitFromDoc = mT2.UnitCode,
						UnitBriefFromDoc = mT2.UnitBrief,
						Price = mT2.Price,
						Quantity = mT2.Quantity,
						UseLife = mT2.UseLife,
						Country = mT2.Country,
						Producer = mT2.Producer,
						Property1 = mT2.Property1,
						Property1Caption = mT2.Property1Caption,
						Property2 = mT2.Property2,
						Property2Caption = mT2.Property2Caption
					});
				}
				break;
			case 10:
				foreach (MTMoveBase.MTMoveSpecificationRow mT3 in ((MTMove)_doc).MTList)
				{
					_mtList.Add(new Label.PrintLabelWindowGridRow
					{
						Include = true,
						MTID = mT3.MTID,
						MTCode = mT3.MTCode,
						MTName = mT3.MTName,
						RowID = mT3.RowID,
						Unit = mT3.UnitCode,
						UnitBrief = mT3.UnitBrief,
						UnitFromDoc = mT3.UnitCode,
						UnitBriefFromDoc = mT3.UnitBrief,
						Quantity = mT3.Quantity
					});
				}
				break;
			case 8:
			case 18:
				foreach (MTInvoiceBase.MTInvoiceSpecificationRow mT4 in ((MTInvoiceBase)_doc).MTList)
				{
					if (!mT4.IsDeleted && !mT4.IsGift)
					{
						_mtList.Add(new Label.PrintLabelWindowGridRow
						{
							Include = true,
							MTID = mT4.ItemID,
							MTCode = mT4.ItemCode,
							MTName = mT4.ItemName,
							RowID = mT4.RowID,
							Unit = mT4.UnitCode,
							UnitBrief = mT4.UnitBrief,
							UnitFromDoc = mT4.UnitCode,
							UnitBriefFromDoc = mT4.UnitBrief,
							Quantity = (int)mT4.Quantity
						});
					}
				}
				break;
			case 15:
				foreach (MTReturnFromCustomerBase.MTRetFromCustSpecificationRow mT5 in ((MTReturnFromCustomer)_doc).MTList)
				{
					if (!mT5.IsDeleted.Value)
					{
						_mtList.Add(new Label.PrintLabelWindowGridRow
						{
							Include = true,
							MTID = mT5.ItemID.Value,
							RowID = mT5.RowID.Value,
							MTCode = mT5.ItemCode,
							MTName = mT5.ItemName,
							Unit = mT5.UnitCode,
							UnitBrief = mT5.UnitBrief,
							UnitFromDoc = mT5.UnitCode,
							UnitBriefFromDoc = mT5.UnitBrief,
							Quantity = mT5.Quantity.Value
						});
					}
				}
				break;
			case 23:
				foreach (MTRevaluationRetail.MTRevaluationRetailSpecificationRow mT6 in ((MTRevaluationRetail)_doc).MTList)
				{
					_mtList.Add(new Label.PrintLabelWindowGridRow
					{
						Include = true,
						MTID = mT6.MTID,
						MTCode = mT6.MTCode,
						MTName = mT6.MTName,
						RowID = mT6.RowID,
						Unit = mT6.UnitCode,
						UnitBrief = mT6.UnitBrief,
						UnitFromDoc = mT6.UnitCode,
						UnitBriefFromDoc = mT6.UnitBrief,
						Quantity = mT6.Quantity
					});
				}
				break;
			case 36:
				foreach (MTMoveBase.MTMoveSpecificationRow mT7 in ((MTMoveOrder)_doc).MTList)
				{
					_mtList.Add(new Label.PrintLabelWindowGridRow
					{
						Include = true,
						MTID = mT7.MTID,
						MTCode = mT7.MTCode,
						MTName = mT7.MTName,
						RowID = mT7.RowID,
						Unit = mT7.UnitCode,
						UnitBrief = mT7.UnitBrief,
						Quantity = mT7.Quantity
					});
				}
				break;
			case 38:
				foreach (MTReturnFromCustomerBase.MTRetFromCustSpecificationRow mT8 in ((MTBillReturn)_doc).MTList)
				{
					_mtList.Add(new Label.PrintLabelWindowGridRow
					{
						Include = true,
						MTID = mT8.ItemID.Value,
						RowID = mT8.RowID.Value,
						MTCode = mT8.ItemCode,
						MTName = mT8.ItemName,
						Unit = mT8.UnitCode,
						UnitBrief = mT8.UnitBrief,
						Quantity = (int)mT8.Quantity.Value
					});
				}
				break;
			}
			_selectedGoods = new List<Label.GoodLabelRow>();
			{
				foreach (Label.PrintLabelWindowGridRow mt in _mtList)
				{
					MaterialCacheRow item = TradeSettings.TradeCacheService.Material.GetItem(mt.MTCode);
					if (item != null)
					{
						Label.GoodLabelRow goodLabelRow = InitContextFunctionsHelper.BuildLabelGoodRow(item);
						if (goodLabelRow != null)
						{
							_selectedGoods.Add(goodLabelRow);
						}
					}
				}
				return;
			}
		}
		_selectedGoods = selectedGoods;
	}

	internal void Invoke()
	{
		Caption = ArmSoft.SME.Trade.Common.Resources.PrintLabel;
		Name = "DlgPrintLabel";
		WindowStartupLocation = WindowStartupLocation.CenterOwner;
		if (_owner != null)
		{
			Owner = _owner;
		}
		LayoutGroup container = AddHorizontalLayoutGroup();
		if (_selectedGoods.Count == 1)
		{
			Label.GoodLabelRow goodLabelRow = _selectedGoods[0];
			_good = AddDropDownControl(container, "Code", ArmSoft.SME.Common.Resources.Code, new GoodsFilter(), isRequired: true, allowAdditionButtons: false, storeValue: false, isShowComment: true, 8);
			_good.Code = goodLabelRow.MaterialeRow.MTCode;
			_good.IsReadOnly = true;
			_unit = AddDropDownControl("Unit", ArmSoft.SME.Trade.Common.Resources.QntUnit, new MTQntUnits(goodLabelRow.MaterialeRow.MTCode), isRequired: true);
			_unit.Code = goodLabelRow.MaterialeRow.Unit;
			_unit.IsReadOnly = true;
			_altUnit = AddDropDownControl("AltUnit", ArmSoft.SME.Trade.Common.Resources.ALTQUNIT, new MTQntUnits(goodLabelRow.MaterialeRow.MTCode), isRequired: true);
			_altUnit.Code = goodLabelRow.MaterialeRow.AltUnit;
			_altUnit.IsReadOnly = true;
			_quantity = AddSpinEdit("Qty", ArmSoft.SME.Common.Resources.Quantity, 1m, 999m, 50.0, storeValue: true);
		}
		if (_selectedGoods.Count != 1)
		{
			_quantity = AddSpinEdit(container, "Qty", ArmSoft.SME.Common.Resources.Quantity, 1m, 999m, 50.0, storeValue: true);
		}
		AddStoreValuesButton(container);
		_quantity.Value = 1m;
		_date = AddDateControl("Date", ArmSoft.SME.Common.Resources.DateWord, useLongDate: false, isRequired: true);
		_date.EditValue = Settings.HostingEnvironment.CurrentDate;
		if (_doc == null || _doc.POType != 20)
		{
			isMTIncomeRetailDoc = false;
			_priceType = AddDropDownControl("PriceType", ArmSoft.SME.Trade.Common.Resources.PriceType, new PriceTypes(), isRequired: false, allowAdditionButtons: false, storeValue: true);
			_priceType.Code = Settings.ParametersService.DefaultSalePrice();
			_discountPriceType = AddDropDownControl("DiscountPriceType", ArmSoft.SME.Trade.Common.Resources.DicountPriceType, new PriceTypes(), isRequired: false, allowAdditionButtons: false, storeValue: true);
			_avgPriceType = AddDropDownControl("AvgPriceType", ArmSoft.SME.Trade.Common.Resources.AvgPriceType, new PriceTypes(), isRequired: false, allowAdditionButtons: false, storeValue: true);
		}
		_template = AddDropDownControl("Template", ArmSoft.SME.Common.Resources.PrintTemplate, new Template(short.Parse(PrintTemplateDocumentTypeSE.Label)), isRequired: true, allowAdditionButtons: false, storeValue: true);
		AddLine();
		if (_selectedGoods.Count > 1)
		{
			_groupByCode = AddBooleanControl("GroupByCode", ArmSoft.SME.Common.Resources.Grouping, storeValue: true);
		}
		_forAltUnit = AddBooleanControl("PrintAltUnit", ArmSoft.SME.Trade.Common.Resources.PrintForAltUnit, storeValue: true);
		_forAltUnit.EditValueChanged += new SafeEventHandler<EditValueChangedEventArgs>(ForAltUnit_EditValueChanged).Handler;
		_printImmediately = AddBooleanControl("PrintImmediately", ArmSoft.SME.Common.Resources.PrintImmediately, storeValue: true);
		if (!_isFromSalesAvgPrice)
		{
			AddLine();
		}
		if (_doc != null || _isFromGoodsAvailability)
		{
			_takeRowQty = AddBooleanControl("TakeRowQty", ArmSoft.SME.Trade.Common.Resources.TakeRowQty, storeValue: true);
			_takeRowQty.EditValueChanged += new SafeEventHandler<EditValueChangedEventArgs>(TakeRowQty_EditValueChanged).Handler;
		}
		if (_isFromSalesAvgPrice)
		{
			((LayoutItemWithRequired)(object)_takeRowQty.Parent).Visibility = Visibility.Collapsed;
		}
		if (_doc != null)
		{
			isMTIncomeDoc = _doc.POType == 7 || _doc.POType == 20;
			_mtListGrid = AddGridControl("MTList");
			_mtListGrid.AllowAdd = false;
			_mtListGrid.AllowDelete = false;
			_mtListGrid.AllowDuplicate = false;
			_mtListGrid.AllowDrop = false;
			_mtListGrid.ShowButtons = false;
			_mtListGrid.AllowEdit = true;
			_mtListGrid.IsReadOnly = true;
			_mtListGrid.View.Height = 150.0;
			_mtListGrid.View.AutoWidth = true;
			_mtListGrid.Width = 700.0;
			_mtListView = _mtListGrid.View;
			_mtListView.AllowMoveColumnToDropArea = false;
			_mtListView.IsColumnMenuEnabled = false;
			_mtListView.AllowColumnMoving = true;
			_mtListView.ColumnHeaderClick += new SafeEventHandler<ColumnHeaderClickEventArgs>(MTListView_ColumnHeaderClick).Handler;
			_includeCol = _mtListGrid.AddBooleanColumn("Include", "");
			_includeCol.Width = 50.0;
			_includeCol.AllowEditing = DefaultBoolean.False;
			_includeCol.AllowSorting = DefaultBoolean.True;
			_mtCodeCol = _mtListGrid.AddStringColumn("MTCode", ArmSoft.SME.Common.Resources.Code, 20);
			_mtCodeCol.Width = 125.0;
			_mtCodeCol.AllowEditing = DefaultBoolean.False;
			_mtNameCol = _mtListGrid.AddStringColumn("MTName", ArmSoft.SME.Common.Resources.Name, 200);
			_mtNameCol.AllowEditing = DefaultBoolean.False;
			_priceCol = _mtListGrid.AddNumberColumn("Price", ArmSoft.SME.Common.Resources.MTTAccSalePrice, 16, 2);
			_priceCol.AllowEditing = DefaultBoolean.False;
			_priceCol.Visible = isMTIncomeRetailDoc;
			_quantityCol = _mtListGrid.AddNumberColumn("Quantity", ArmSoft.SME.Common.Resources.Quantity, 13, 3);
			_quantityCol.AllowEditing = DefaultBoolean.False;
			_unitCol = _mtListGrid.AddStringColumn("UnitBrief", ArmSoft.SME.Common.Resources.Unit, 6);
			_unitCol.AllowEditing = DefaultBoolean.False;
			if (isMTIncomeDoc)
			{
				_useLifeCol = _mtListGrid.AddDateColumn("UseLife", ArmSoft.SME.Trade.Common.Resources.UseLife);
				_useLifeCol.Visible = Settings.ParametersService.ShowPartyUSEABLEDATE();
				_useLifeCol.AllowEditing = DefaultBoolean.False;
				_countryCol = _mtListGrid.AddDropDownColumn("Country", ArmSoft.SME.Common.Resources.Country, new Countries());
				_countryCol.Visible = Settings.ParametersService.ShowPartyCountry();
				_countryCol.AllowEditing = DefaultBoolean.False;
				_producerCol = _mtListGrid.AddDropDownColumn("Producer", ArmSoft.SME.Common.Resources.Producer, new Producer());
				_producerCol.Width = SizeCalculator.WidthForString(5);
				_producerCol.Visible = Settings.ParametersService.ShowPartyProducer();
				_producerCol.AllowEditing = DefaultBoolean.False;
				_property1Col = _mtListGrid.AddDropDownColumn("Property1Caption", ArmSoft.SME.Trade.Common.Resources.Attribute1, new PropertyList());
				_property1Col.AllowEditing = DefaultBoolean.False;
				_property1Col.Width = SizeCalculator.WidthForString(12);
				_property1Col.Visible = Settings.ParametersService.ShowPartyProperty1();
				_property2Col = _mtListGrid.AddDropDownColumn("Property2Caption", ArmSoft.SME.Trade.Common.Resources.Attribute2, new PropertyList());
				_property2Col.AllowEditing = DefaultBoolean.False;
				_property2Col.Width = SizeCalculator.WidthForString(12);
				_property2Col.Visible = Settings.ParametersService.ShowPartyProperty2();
			}
			_mtListGrid.ItemsSource = _mtList;
			_quantity.EditValueChanged += new SafeEventHandler<EditValueChangedEventArgs>(Quantity_EditValueChanged).Handler;
		}
		SetFocusOnActivation = _quantity;
		if (!ShowDialog().Value)
		{
			return;
		}
		if (_isFromGoodsAvailability && _takeRowQty.IsChecked.Value)
		{
			_selectedGoods = _selectedGoods.Where((Label.GoodLabelRow p) => p.Quantity > 0m).ToList();
		}
		PrintLabel();
	}

	private void PrintLabel()
	{
		Label printableElement = new Label
		{
			Caption = ArmSoft.SME.Trade.Common.Resources.PrintLabel,
			SelectedGoods = _selectedGoods,
			Qty = (int)_quantity.Value,
			Date = _date.EditValue.Value,
			PriceType = (isMTIncomeRetailDoc ? string.Empty : _priceType.Code),
			DiscountPriceType = (isMTIncomeRetailDoc ? string.Empty : _discountPriceType.Code),
			AvgPriceType = (isMTIncomeRetailDoc ? string.Empty : _avgPriceType.Code),
			TemplateName = _template.Code,
			PrintAltUnit = _forAltUnit.IsChecked.Value,
			GroupByCode = (_selectedGoods.Count <= 1 || _groupByCode.IsChecked.Value),
			TakeRowQty = ((_doc != null || _isFromGoodsAvailability) && _takeRowQty.IsChecked.Value),
			GoodsList = ((_doc != null) ? _mtList : null),
			Doc = (isMTIncomeDoc ? _doc : null)
		};
		if (_printImmediately.IsChecked.Value)
		{
			Settings.ServiceProvider.GetRequiredService<ITemplateSubstitutionService>().PrintPreview(printableElement, PrintMode.PrintDirect, _template.Code, LabelPrinterInfo.GetInstance().LabelPrinterName, 1, includePrintInfo: false);
		}
		else
		{
			Settings.ServiceProvider.GetRequiredService<ITemplateSubstitutionService>().PrintPreview(printableElement, PrintMode.PrintPreview, _template.Code, LabelPrinterInfo.GetInstance().LabelPrinterName, 1, includePrintInfo: false);
		}
	}

	private void MTListView_ColumnHeaderClick(object sender, ColumnHeaderClickEventArgs e)
	{
		if (e.Column == _includeCol && _mtListGrid.Grid.VisibleRowCount != 0)
		{
			SelectDeselect();
			e.Handled = true;
		}
	}

	private void SelectDeselect()
	{
		if (_includeCol.AllowEditing == DefaultBoolean.False)
		{
			return;
		}
		bool flag = true;
		foreach (Label.PrintLabelWindowGridRow mt in _mtList)
		{
			if (!mt.Include)
			{
				flag = false;
				break;
			}
		}
		for (int i = 0; i <= _mtListGrid.Grid.VisibleRowCount - 1; i++)
		{
			int rowHandleByVisibleIndex = _mtListGrid.Grid.GetRowHandleByVisibleIndex(i);
			Label.PrintLabelWindowGridRow printLabelWindowGridRow = (Label.PrintLabelWindowGridRow)_mtListGrid.Grid.GetRow(rowHandleByVisibleIndex);
			if (printLabelWindowGridRow != null)
			{
				if (flag)
				{
					printLabelWindowGridRow.Include = false;
				}
				else
				{
					printLabelWindowGridRow.Include = true;
				}
			}
		}
		_mtListGrid.Grid.RefreshData();
	}

	private void TakeRowQty_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		if (_takeRowQty.IsChecked.Value)
		{
			if (_doc != null)
			{
				_mtListGrid.IsReadOnly = false;
				_includeCol.AllowEditing = DefaultBoolean.True;
				_quantityCol.AllowEditing = DefaultBoolean.True;
				_mtListView.Grid.Focus();
			}
			else
			{
				_quantity.Value = 1m;
				_quantity.IsEnabled = false;
			}
		}
		else if (_doc != null)
		{
			_mtListGrid.IsReadOnly = true;
			_includeCol.AllowEditing = DefaultBoolean.False;
			_quantityCol.AllowEditing = DefaultBoolean.False;
		}
		else
		{
			_quantity.Value = 1m;
			_quantity.IsEnabled = true;
		}
	}

	private void ForAltUnit_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		ChangePricesForUnit();
	}

	private void ChangePricesForUnit()
	{
		if (_doc == null)
		{
			return;
		}
		bool flag = Convert.ToBoolean(_forAltUnit.IsChecked);
		foreach (Label.PrintLabelWindowGridRow mt in _mtList)
		{
			MaterialCacheRow item = TradeSettings.TradeCacheService.Material.GetItem(mt.MTCode);
			if (item == null)
			{
				continue;
			}
			MaterialCacheRow.QntUnitRow qntUnit = TradeSettings.TradeCacheService.Material.GetQntUnit(mt.MTCode, mt.Unit);
			if (flag)
			{
				if (!(mt.Unit != item.AltUnit))
				{
					continue;
				}
				mt.Unit = item.AltUnit;
				mt.UnitBrief = item.AltUnitRow.Brief;
				if (_doc.POType == DocumentTypeSE.MTIncomeRetail.Value)
				{
					MaterialCacheRow.QntUnitRow qntUnit2 = TradeSettings.TradeCacheService.Material.GetQntUnit(mt.MTCode, item.AltUnit);
					if (qntUnit != null && qntUnit2 != null)
					{
						mt.Price = (qntUnit2.Coef * mt.Price / qntUnit.Coef).RoundAs(2);
					}
				}
			}
			else
			{
				if (!(mt.Unit != mt.UnitFromDoc))
				{
					continue;
				}
				mt.Unit = mt.UnitFromDoc;
				mt.UnitBrief = mt.UnitBriefFromDoc;
				if (_doc.POType == DocumentTypeSE.MTIncomeRetail.Value)
				{
					MaterialCacheRow.QntUnitRow qntUnit2 = TradeSettings.TradeCacheService.Material.GetQntUnit(mt.MTCode, mt.UnitFromDoc);
					if (qntUnit != null && qntUnit2 != null)
					{
						mt.Price = (qntUnit2.Coef * mt.Price / qntUnit.Coef).RoundAs(2);
					}
				}
			}
		}
		_mtListView.Grid.RefreshData();
	}

	private void Quantity_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		if (_doc == null || !_takeRowQty.IsChecked.Value)
		{
			return;
		}
		foreach (Label.PrintLabelWindowGridRow mt in _mtList)
		{
			if (mt.Include)
			{
				mt.Quantity = _quantity.Value;
			}
		}
		_mtListGrid.Grid.RefreshData();
	}
}
