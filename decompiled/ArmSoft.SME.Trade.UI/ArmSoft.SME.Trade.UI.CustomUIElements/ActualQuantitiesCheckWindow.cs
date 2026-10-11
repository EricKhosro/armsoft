using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Extensions;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Core.UI.Controls;
using ArmSoft.SME.Core.UI.DataViews;
using ArmSoft.SME.Trade.Common;
using ArmSoft.SME.Trade.Common.Models.Caches;
using ArmSoft.SME.Trade.Common.SmartEnums;
using ArmSoft.SME.Trade.UI.Documents;
using ArmSoft.SME.Trade.UI.Documents.Goods;
using ArmSoft.SME.Trade.UI.DropDown;
using ArmSoft.SME.Trade.UI.Helpers;
using DevExpress.Utils;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Editors;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.LayoutControl;
using DevExpress.XtraPrinting;
using Microsoft.Win32;

namespace ArmSoft.SME.Trade.UI.CustomUIElements;

public class ActualQuantitiesCheckWindow : DialogWindow
{
	private LayoutGroup _mtAddLayoutGroup;

	private TextEditExt _barCode;

	private Button _codeEnter;

	private DocumentGridControl _qcGrid;

	private DocumentGridColumn _mtCodeCol;

	private DocumentGridColumn _unitBriefCol;

	private DocumentGridColumn _actualQuantityCol;

	private DocumentGridColumn _varianceQuantityCol;

	private TableView _qcTableView;

	private BarEditItemExt _showOnlyVariations;

	private BarEditItemExt _exportToExcelButton;

	private readonly DocumentTypeSE _docType;

	private readonly DateOnly _docDate;

	private readonly string _docNum;

	private int _maxRowID;

	private bool _hasVariations;

	private ObservableCollection<ActualQuantitiesCheck.ActualQuantitiesCheckRow> _mtList = new ObservableCollection<ActualQuantitiesCheck.ActualQuantitiesCheckRow>();

	private bool _isClickedUnitBriefDropDown;

	private readonly bool _ignoreVariationsOnClose = true;

	public List<ActualQuantitiesCheck.ActualQuantitiesCheckRow> QCResult { get; set; }

	public ActualQuantitiesCheckWindow(Window owner)
		: base(owner)
	{
	}

	public ActualQuantitiesCheckWindow(Window mtInvoiceWindow, ObservableCollection<MTIncomeImportWindow.MTIncomeImportSpecificationRow> mtInvoiceList, DateOnly DocDate, string DocNum)
		: base(mtInvoiceWindow)
	{
		_docType = DocumentTypeSE.MTIncome;
		_docDate = DocDate;
		_docNum = DocNum;
		Owner = mtInvoiceWindow;
		ConstructUI();
		foreach (MTIncomeImportWindow.MTIncomeImportSpecificationRow mtInvoice in mtInvoiceList)
		{
			MaterialCacheRow item = TradeSettings.TradeCacheService.Material.GetItem(mtInvoice.MTCode);
			AddNewRowOrMergeInner(item, mtInvoice.UnitCode, mtInvoice.UnitBrief, 0m, mtInvoice.Quantity, userAdd: false);
		}
	}

	public ActualQuantitiesCheckWindow(Window mtInvoiceWindow, ObservableCollection<MTBillWindow.MTBillSpecificationRow> mtInvoiceList, DateOnly DocDate, string DocNum)
		: base(mtInvoiceWindow)
	{
		_docType = DocumentTypeSE.MTBill;
		_docDate = DocDate;
		_docNum = DocNum;
		Owner = mtInvoiceWindow;
		ConstructUI();
		foreach (MTBillWindow.MTBillSpecificationRow mtInvoice in mtInvoiceList)
		{
			MaterialCacheRow item = TradeSettings.TradeCacheService.Material.GetItem(mtInvoice.ItemCode);
			AddNewRowOrMergeInner(item, mtInvoice.UnitCode, mtInvoice.UnitBrief, 0m, mtInvoice.Quantity, userAdd: false);
		}
	}

	public ActualQuantitiesCheckWindow(Window mtInvoiceWindow, ObservableCollection<MTInvoiceRetailWindow.MTInvoiceRetailSpecificationRow> mtInvoiceList, DateOnly DocDate, string DocNum)
		: base(mtInvoiceWindow)
	{
		_docType = DocumentTypeSE.MTSaleRetail;
		_docDate = DocDate;
		_docNum = DocNum;
		Owner = mtInvoiceWindow;
		ConstructUI();
		foreach (MTInvoiceRetailWindow.MTInvoiceRetailSpecificationRow mtInvoice in mtInvoiceList)
		{
			MaterialCacheRow item = TradeSettings.TradeCacheService.Material.GetItem(mtInvoice.ItemCode);
			AddNewRowOrMergeInner(item, mtInvoice.UnitCode, mtInvoice.UnitBrief, 0m, mtInvoice.Quantity, userAdd: false);
		}
	}

	public ActualQuantitiesCheckWindow(Window mtInvoiceWindow, ObservableCollection<MTInvoiceWindow.MTInvoiceSpecificationRow> mtInvoiceList, DateOnly DocDate, string DocNum)
		: base(mtInvoiceWindow)
	{
		_docType = DocumentTypeSE.MTInvoice;
		_docDate = DocDate;
		_docNum = DocNum;
		Owner = mtInvoiceWindow;
		ConstructUI();
		foreach (MTInvoiceWindow.MTInvoiceSpecificationRow mtInvoice in mtInvoiceList)
		{
			MaterialCacheRow item = TradeSettings.TradeCacheService.Material.GetItem(mtInvoice.ItemCode);
			AddNewRowOrMergeInner(item, mtInvoice.UnitCode, mtInvoice.UnitBrief, 0m, mtInvoice.Quantity, userAdd: false);
		}
	}

	public ActualQuantitiesCheckWindow(Window mtIncomeImportWindow, ObservableCollection<MTIncomeBaseWindow<ArmSoft.SME.Trade.Common.Models.Documents.MTIncomeRetail>.MTIncomeSpecificationRow> mtIncomeImportList, DateOnly DocDate, string DocNum)
		: base(mtIncomeImportWindow)
	{
		_docType = DocumentTypeSE.MTIncomeRetail;
		_docDate = DocDate;
		_docNum = DocNum;
		Owner = mtIncomeImportWindow;
		ConstructUI();
		foreach (MTIncomeBaseWindow<ArmSoft.SME.Trade.Common.Models.Documents.MTIncomeRetail>.MTIncomeSpecificationRow mtIncomeImport in mtIncomeImportList)
		{
			MaterialCacheRow item = TradeSettings.TradeCacheService.Material.GetItem(mtIncomeImport.MTCode);
			AddNewRowOrMergeInner(item, mtIncomeImport.UnitCode, mtIncomeImport.UnitBrief, 0m, mtIncomeImport.Quantity, userAdd: false);
		}
	}

	public ActualQuantitiesCheckWindow(Window mtIncomeWindow, ObservableCollection<MTIncomeBaseWindow<ArmSoft.SME.Trade.Common.Models.Documents.MTIncome>.MTIncomeSpecificationRow> mtIncomeList, DateOnly DocDate, string DocNum)
		: base(mtIncomeWindow)
	{
		_docType = DocumentTypeSE.MTIncome;
		_docDate = DocDate;
		_docNum = DocNum;
		Owner = mtIncomeWindow;
		ConstructUI();
		foreach (MTIncomeBaseWindow<ArmSoft.SME.Trade.Common.Models.Documents.MTIncome>.MTIncomeSpecificationRow mtIncome in mtIncomeList)
		{
			MaterialCacheRow item = TradeSettings.TradeCacheService.Material.GetItem(mtIncome.MTCode);
			AddNewRowOrMergeInner(item, mtIncome.UnitCode, mtIncome.UnitBrief, 0m, mtIncome.Quantity, userAdd: false);
		}
	}

	public ActualQuantitiesCheckWindow(MTMoveWindow mtMoveWindow, ObservableCollection<MTMoveWindow.MTMoveSpecificationRow> MTMoveList, DateOnly DocDate, string DocNum)
		: base(mtMoveWindow)
	{
		_docType = DocumentTypeSE.MTMove;
		_docDate = DocDate;
		_docNum = DocNum;
		Owner = mtMoveWindow;
		ConstructUI();
		foreach (MTMoveWindow.MTMoveSpecificationRow MTMove in MTMoveList)
		{
			MaterialCacheRow item = TradeSettings.TradeCacheService.Material.GetItem(MTMove.MTCode);
			AddNewRowOrMergeInner(item, MTMove.UnitCode, MTMove.UnitBrief, 0m, MTMove.Quantity, userAdd: false);
		}
	}

	public ActualQuantitiesCheckWindow(MTMoveWithConfirmationWindow mtMoveWithConfirmationWindow, ObservableCollection<MTMoveWithConfirmationWindow.MTMoveSpecificationRow> mtMoveWithConfirmationList, DateOnly docDate, string docNum)
		: base(mtMoveWithConfirmationWindow)
	{
		_docType = DocumentTypeSE.MTMoveWithConfirmation;
		_docDate = docDate;
		_docNum = docNum;
		Owner = mtMoveWithConfirmationWindow;
		ConstructUI();
		foreach (MTMoveWithConfirmationWindow.MTMoveSpecificationRow mtMoveWithConfirmation in mtMoveWithConfirmationList)
		{
			MaterialCacheRow item = TradeSettings.TradeCacheService.Material.GetItem(mtMoveWithConfirmation.MTCode);
			AddNewRowOrMergeInner(item, mtMoveWithConfirmation.UnitCode, mtMoveWithConfirmation.UnitBrief, mtMoveWithConfirmation.ActualQuantity, mtMoveWithConfirmation.Quantity, userAdd: false);
		}
		_ignoreVariationsOnClose = false;
	}

	private void ConstructUI()
	{
		ResizeMode = ResizeMode.CanResize;
		SizeToContent = SizeToContent.Manual;
		ShowInTaskbar = true;
		ShowIcon = true;
		Height = 600.0;
		Width = 900.0;
		_mtAddLayoutGroup = AddHorizontalLayoutGroup();
		_barCode = AddStringControl(_mtAddLayoutGroup, "BarCode", ArmSoft.SME.Common.Resources.BarCode + " (F2)", 20, isRequired: false, storeValue: false, "", HorizontalAlignment.Stretch);
		_codeEnter = AddButton(_mtAddLayoutGroup, "CodeEnter", "", ArmSoft.SME.Trade.Common.Resources.cmdCodeEnter + " (F8)");
		Icon = Settings.HostingEnvironment.Icon;
		WindowStartupLocation = WindowStartupLocation.CenterOwner;
		_layoutControl.ScrollBars = ScrollBars.Auto;
		Title = GetTitle();
		_okButton.Content = ArmSoft.SME.Common.Resources.Ok;
		_cancelButton.Content = ArmSoft.SME.Common.Resources.Cancel;
		_barCode.NullText = ArmSoft.SME.Trade.Common.Resources.BarCodeAndCodeInputNullText;
		_qcGrid = new DocumentGridControl
		{
			Owner = this,
			Name = "QuantityCheck",
			AllowMoveUp = true,
			AllowMoveDown = true,
			ShowButtons = true,
			ItemsSource = _mtList
		};
		_qcTableView = _qcGrid.View;
		_qcGrid.AddingRow += new SafeEventHandler<DocumentGridAddingRowEventArgs>(QuantityCheckGrid_AddingRow).Handler;
		_qcGrid.DuplicatingRow += new SafeEventHandler<DocumentGridDuplicatingRowEventArgs>(QCGrid_DuplicatingRow).Handler;
		_qcGrid.DeletingRow += new SafeEventHandler<DocumentGridDeletingRowEventArgs>(QuantityCheckGrid_DeletingRow).Handler;
		_qcGrid.DeletedRow += new SafeEventHandler<EventArgs>(MTMove_DeletedRow).Handler;
		_qcGrid.MovingRowUp += new SafeEventHandler<DocumentGridMovingRowUpOrDownEventArgs>(QuantityCheckGrid_MovingRowUp).Handler;
		_qcGrid.MovingRowDown += new SafeEventHandler<DocumentGridMovingRowUpOrDownEventArgs>(QuantityCheckGrid_MovingRowDown).Handler;
		_qcTableView.CellValueChanged += new SafeEventHandler<CellValueChangedEventArgs>(QuantityCheckTableView_CellValueChanged).Handler;
		_qcGrid.ClickDropDown += new SafeEventHandler<DocumentGridClickDropDownEventArgs>(QCGrid_ClickDropDown).Handler;
		_codeEnter.Click += new SafeEventHandler<RoutedEventArgs>(CodeButton_Click).Handler;
		_barCode.KeyDown += new SafeEventHandler<KeyEventArgs>(BarCodeText_KeyDown).Handler;
		PreviewKeyDown += new SafeEventHandler<KeyEventArgs>(ActualQuantitiesCheckWindow_PreviewKeyDown).Handler;
		_qcGrid.EscapePressed += new SafeEventHandler<EventArgs>(QCGrid_EscapePressed).Handler;
		Closing += new SafeEventHandler<CancelEventArgs>(ActualQuantitiesCheckWindow_Closing).Handler;
		_exportToExcelButton = _qcGrid.AddButton(null, ArmSoft.SME.Common.Resources.ExportToExcel, ArmSoft.SME.Common.Resources.ExportToExcel, 130.0);
		_exportToExcelButton.Click += new SafeEventHandler<RoutedEventArgs>(OnExportToExcelButton_click).Handler;
		_showOnlyVariations = _qcGrid.AddButton(null, ArmSoft.SME.Trade.Common.Resources.ShowOnlyVariations, ArmSoft.SME.Trade.Common.Resources.ShowOnlyVariations, 200.0);
		_showOnlyVariations.Click += new SafeEventHandler<RoutedEventArgs>(OnShowOnlyVariations).Handler;
		_qcGrid.AddRowIndicator(35);
		_mtCodeCol = _qcGrid.AddDropDownColumn("MTCode", ArmSoft.SME.Common.Resources.Code, 60);
		_mtCodeCol.Width = 125.0;
		_qcGrid.AddStringColumn("MTName", ArmSoft.SME.Common.Resources.Name, 50).AllowEditing = DefaultBoolean.False;
		_unitBriefCol = _qcGrid.AddDropDownColumn("UnitBrief", ArmSoft.SME.Common.Resources.Unit, 6, trimValue: false, GlyphKind.Down);
		_qcGrid.AddNumberColumn("BaseQuantity", ArmSoft.SME.Common.Resources.AccQTY, 13, 3, allowNegativeNumbers: false, showTotal: true).AllowEditing = DefaultBoolean.False;
		_actualQuantityCol = _qcGrid.AddNumberColumn("ActualQuantity", ArmSoft.SME.Common.Resources.ActualQTY, 13, 3, allowNegativeNumbers: false, showTotal: true);
		_varianceQuantityCol = _qcGrid.AddNumberColumn("VarianceQuantity", ArmSoft.SME.Common.Resources.VarianceQty, 13, 3, allowNegativeNumbers: true);
		_varianceQuantityCol.AllowEditing = DefaultBoolean.False;
		_qcGrid.AddStringColumn("VarianceState", ArmSoft.SME.Common.Resources.State, 6).AllowEditing = DefaultBoolean.False;
		foreach (GridColumn column in _qcGrid.Grid.Columns)
		{
			column.AllowSorting = DefaultBoolean.True;
			column.AllowColumnFiltering = DefaultBoolean.True;
			column.FilterPopupMode = FilterPopupMode.CheckedList;
		}
		_qcTableView = _qcGrid.View;
		_qcGrid.HorizontalAlignment = HorizontalAlignment.Stretch;
		_qcGrid.VerticalAlignment = VerticalAlignment.Stretch;
		MinWidth = SizeCalculator.Width(600.0);
		Width = SizeCalculator.Width(1150.0);
		MinHeight = SizeCalculator.Height(300.0);
		_layoutControl.Children.Add(_qcGrid);
	}

	private string GetTitle()
	{
		return $"{ArmSoft.SME.Trade.Common.Resources.ActualQuantitiesCheck} ( {_docType.FullCaption} {_docNum} {ArmSoft.SME.Common.Resources.DateShort} {_docDate.FormatDDMMYY()} )";
	}

	private void QuantityCheckGrid_AddingRow(object sender, DocumentGridAddingRowEventArgs e)
	{
		_mtList.Add(new ActualQuantitiesCheck.ActualQuantitiesCheckRow
		{
			RowID = _maxRowID,
			ActualQuantity = 1m,
			VarianceQuantity = 1m,
			VarianceState = ArmSoft.SME.Common.Resources.Surplus,
			UserAdd = true
		});
		_maxRowID++;
		e.RowNumber = _mtList.Count - 1;
	}

	private void QCGrid_DuplicatingRow(object sender, DocumentGridDuplicatingRowEventArgs e)
	{
		ActualQuantitiesCheck.ActualQuantitiesCheckRow actualQuantitiesCheckRow = (ActualQuantitiesCheck.ActualQuantitiesCheckRow)_qcGrid.Grid.GetRow(e.RowHandle);
		if (actualQuantitiesCheckRow != null)
		{
			ActualQuantitiesCheck.ActualQuantitiesCheckRow actualQuantitiesCheckRow2 = new ActualQuantitiesCheck.ActualQuantitiesCheckRow();
			actualQuantitiesCheckRow.Clone(actualQuantitiesCheckRow2);
			actualQuantitiesCheckRow2.RowID = _maxRowID;
			actualQuantitiesCheckRow2.VarianceState = string.Empty;
			actualQuantitiesCheckRow2.BaseQuantity = 0m;
			actualQuantitiesCheckRow2.ActualQuantity = 1m;
			actualQuantitiesCheckRow2.VarianceQuantity = 1m;
			actualQuantitiesCheckRow2.VarianceState = ArmSoft.SME.Common.Resources.Surplus;
			actualQuantitiesCheckRow2.UserAdd = true;
			_mtList.Add(actualQuantitiesCheckRow2);
			_maxRowID++;
			e.NewRowNumber = _mtList.Count - 1;
		}
	}

	private void QuantityCheckGrid_DeletingRow(object sender, DocumentGridDeletingRowEventArgs e)
	{
		if (!((ActualQuantitiesCheck.ActualQuantitiesCheckRow)_qcGrid.Grid.GetRow(e.RowHandle)).UserAdd)
		{
			e.Cancel = true;
			throw new ASException(ArmSoft.SME.Trade.Common.Resources.RowIsInDocument);
		}
	}

	private void MTMove_DeletedRow(object sender, EventArgs e)
	{
		_qcGrid.Grid.RefreshData();
	}

	private void QuantityCheckGrid_MovingRowUp(object sender, DocumentGridMovingRowUpOrDownEventArgs e)
	{
		if (_qcGrid.Grid.SortInfo.Count > 0 || _qcTableView.Grid.VisibleRowCount != _mtList.Count)
		{
			throw new ASException(ArmSoft.SME.Common.Resources.NotAllowRowMoveInSortedGrid);
		}
		ActualQuantitiesCheck.ActualQuantitiesCheckRow actualQuantitiesCheckRow = (ActualQuantitiesCheck.ActualQuantitiesCheckRow)_qcGrid.Grid.GetRow(e.RowHandle);
		if (actualQuantitiesCheckRow != null)
		{
			_mtList.RemoveAt(e.RowHandle);
			_mtList.Insert(e.RowHandle - 1, actualQuantitiesCheckRow);
		}
		_qcGrid.Grid.RefreshData();
	}

	private void QuantityCheckGrid_MovingRowDown(object sender, DocumentGridMovingRowUpOrDownEventArgs e)
	{
		if (_qcGrid.Grid.SortInfo.Count > 0 || _qcTableView.Grid.VisibleRowCount != _mtList.Count)
		{
			throw new ASException(ArmSoft.SME.Common.Resources.NotAllowRowMoveInSortedGrid);
		}
		ActualQuantitiesCheck.ActualQuantitiesCheckRow actualQuantitiesCheckRow = (ActualQuantitiesCheck.ActualQuantitiesCheckRow)_qcGrid.Grid.GetRow(e.RowHandle);
		if (actualQuantitiesCheckRow != null)
		{
			_mtList.RemoveAt(e.RowHandle);
			_mtList.Insert(e.RowHandle + 1, actualQuantitiesCheckRow);
		}
		_qcGrid.Grid.RefreshData();
	}

	private void QuantityCheckTableView_CellValueChanged(object sender, CellValueChangedEventArgs e)
	{
		ActualQuantitiesCheck.ActualQuantitiesCheckRow actualQuantitiesCheckRow = (ActualQuantitiesCheck.ActualQuantitiesCheckRow)_qcGrid.Grid.GetRow(e.RowHandle);
		ActualQuantitiesCheck.ActualQuantitiesCheckRow actualQuantitiesCheckRow2 = (ActualQuantitiesCheck.ActualQuantitiesCheckRow)_qcGrid.Grid.GetRow(e.RowHandle);
		try
		{
			if (actualQuantitiesCheckRow2 == null)
			{
				return;
			}
			if (actualQuantitiesCheckRow.UserAdd)
			{
				string text = string.Empty;
				if (e.OldValue != null)
				{
					text = e.OldValue.ToString();
				}
				if (e.Column == _mtCodeCol)
				{
					if (e.Value != null)
					{
						string text2 = e.Value.ToString();
						MaterialCacheRow materialCacheRow = null;
						foreach (ActualQuantitiesCheck.ActualQuantitiesCheckRow mt in _mtList)
						{
							if (actualQuantitiesCheckRow2 != mt && text2 == mt.MTCode)
							{
								if (ArmSoft.SME.Core.UI.MessageBox.Show(string.Format(ArmSoft.SME.Trade.Common.Resources.MTExistInMTList, text2), ArmSoft.SME.Trade.Common.Resources.ProgramName, MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.Yes) == MessageBoxResult.No)
								{
									actualQuantitiesCheckRow2.MTCode = string.Empty;
									_qcGrid.Grid.RefreshData();
									return;
								}
								break;
							}
						}
						if (!string.IsNullOrWhiteSpace(e.Value.ToString()) && text != e.Value.ToString())
						{
							materialCacheRow = TradeSettings.TradeCacheService.Material.GetItem(text2);
							if (materialCacheRow == null)
							{
								throw new ASException(string.Format(ArmSoft.SME.Trade.Common.Resources.MaterialNotExist, text2));
							}
							actualQuantitiesCheckRow2.MTID = materialCacheRow.Id;
							actualQuantitiesCheckRow2.MTName = materialCacheRow.Caption;
							actualQuantitiesCheckRow2.UnitCode = materialCacheRow.Unit;
							actualQuantitiesCheckRow2.UnitBrief = materialCacheRow.UnitRow.Brief;
							actualQuantitiesCheckRow2.BaseQuantity = 0m;
							actualQuantitiesCheckRow2.ActualQuantity = 1m;
							actualQuantitiesCheckRow2.VarianceQuantity = 1m;
							actualQuantitiesCheckRow2.UserAdd = true;
						}
						else if (string.IsNullOrWhiteSpace(e.Value.ToString()))
						{
							actualQuantitiesCheckRow2.MTID = -1;
							actualQuantitiesCheckRow2.MTName = "";
							actualQuantitiesCheckRow2.UnitCode = "";
							actualQuantitiesCheckRow2.UnitBrief = "";
							actualQuantitiesCheckRow2.BaseQuantity = 0m;
							actualQuantitiesCheckRow2.ActualQuantity = 0m;
							actualQuantitiesCheckRow2.VarianceQuantity = 0m;
							actualQuantitiesCheckRow2.UserAdd = true;
						}
					}
				}
				else if (e.Column == _unitBriefCol)
				{
					if (((e.Value == null) ? "" : e.Value.ToString().Trim()) == text.Trim())
					{
						return;
					}
					if (!_isClickedUnitBriefDropDown)
					{
						List<MaterialCacheRow.QntUnitRow> qntUnits = TradeSettings.TradeCacheService.Material.GetItem(actualQuantitiesCheckRow2.MTCode).QntUnits;
						string text3 = actualQuantitiesCheckRow2.UnitCode.Trim();
						foreach (MaterialCacheRow.QntUnitRow item in qntUnits)
						{
							if (TradeSettings.TradeCacheService.QntUnit.GetItem(item.QntUnit).Brief.Trim() == e.Value.ToString().Trim())
							{
								actualQuantitiesCheckRow2.UnitCode = item.QntUnit;
								break;
							}
						}
						if (text3 != actualQuantitiesCheckRow2.UnitCode)
						{
							UnitColValueChange(actualQuantitiesCheckRow2, text3);
						}
					}
					_isClickedUnitBriefDropDown = false;
				}
			}
			if (e.Column == _actualQuantityCol)
			{
				if (Convert.ToDecimal(e.Value) == Convert.ToDecimal(e.OldValue) || Convert.ToDecimal(e.Value) < 0m)
				{
					return;
				}
				actualQuantitiesCheckRow2.VarianceQuantity = actualQuantitiesCheckRow2.ActualQuantity - actualQuantitiesCheckRow2.BaseQuantity;
			}
			ChangeRowState(actualQuantitiesCheckRow2);
			_qcGrid.Grid.RefreshRow(e.RowHandle);
		}
		catch (Exception ex)
		{
			actualQuantitiesCheckRow2.MTID = -1;
			actualQuantitiesCheckRow2.MTName = "";
			actualQuantitiesCheckRow2.UnitCode = "";
			actualQuantitiesCheckRow2.UnitBrief = "";
			actualQuantitiesCheckRow2.BaseQuantity = 0m;
			actualQuantitiesCheckRow2.ActualQuantity = 0m;
			actualQuantitiesCheckRow2.VarianceQuantity = 0m;
			actualQuantitiesCheckRow2.VarianceState = "";
			actualQuantitiesCheckRow2.UserAdd = true;
			if (e.Value != null && !string.IsNullOrWhiteSpace(e.Value.ToString()))
			{
				Settings.ErrorHandler(ex);
			}
		}
	}

	private void QCGrid_ClickDropDown(object sender, DocumentGridClickDropDownEventArgs e)
	{
		try
		{
			ActualQuantitiesCheck.ActualQuantitiesCheckRow actualQuantitiesCheckRow = (ActualQuantitiesCheck.ActualQuantitiesCheckRow)_qcGrid.Grid.GetRow(e.RowHandle);
			ActualQuantitiesCheck.ActualQuantitiesCheckRow actualQuantitiesCheckRow2 = (ActualQuantitiesCheck.ActualQuantitiesCheckRow)_qcGrid.Grid.GetRow(e.RowHandle);
			if (actualQuantitiesCheckRow2 == null || !actualQuantitiesCheckRow.UserAdd)
			{
				return;
			}
			if (e.Column == _mtCodeCol)
			{
				GoodsFilter goodsFilter = new GoodsFilter(shortMode: true, ItemTypesSE.Goods.Value, null, isShowOnlyInPriceList: true);
				ModalBrowser modalBrowser = new ModalBrowser(goodsFilter)
				{
					LookUpValue = e.Value,
					Owner = e.Owner,
					Left = e.Left,
					Top = e.Top
				};
				if (modalBrowser.Show())
				{
					e.NewValue = goodsFilter.ValueMember(modalBrowser.SelectedRowIndex);
				}
			}
			else
			{
				if (e.Column != _unitBriefCol)
				{
					return;
				}
				MTQntUnits mTQntUnits = new MTQntUnits(actualQuantitiesCheckRow2.MTCode);
				ModalBrowser modalBrowser2 = new ModalBrowser(mTQntUnits)
				{
					LookUpValue = actualQuantitiesCheckRow2.UnitCode,
					Owner = e.Owner,
					Left = e.Left,
					Top = e.Top
				};
				if (!modalBrowser2.Show())
				{
					return;
				}
				_isClickedUnitBriefDropDown = true;
				string text = mTQntUnits.ValueMember(modalBrowser2.SelectedRowIndex).Trim();
				string oldUnit = actualQuantitiesCheckRow2.UnitCode.Trim();
				QntUnitCacheRow item = TradeSettings.TradeCacheService.QntUnit.GetItem(text);
				if (actualQuantitiesCheckRow2.UnitCode.Trim() != text)
				{
					if (item != null)
					{
						actualQuantitiesCheckRow2.UnitCode = item.Code;
						e.NewValue = item.Brief;
					}
					UnitColValueChange(actualQuantitiesCheckRow2, oldUnit);
				}
				else if (item != null)
				{
					e.NewValue = item.Brief;
				}
			}
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	private void UnitColValueChange(ActualQuantitiesCheck.ActualQuantitiesCheckRow row, string oldUnit)
	{
		MaterialCacheRow.QntUnitRow qntUnit = TradeSettings.TradeCacheService.Material.GetQntUnit(row.MTCode, row.UnitCode);
		MaterialCacheRow.QntUnitRow qntUnit2 = TradeSettings.TradeCacheService.Material.GetQntUnit(row.MTCode, oldUnit);
		if (qntUnit != null && qntUnit2 != null)
		{
			decimal coef = qntUnit.Coef;
			decimal coef2 = qntUnit2.Coef;
			row.ActualQuantity = (row.ActualQuantity * coef2 / coef).RoundAs(3);
			row.BaseQuantity = (row.BaseQuantity * coef2 / coef).RoundAs(3);
		}
		row.VarianceQuantity = row.ActualQuantity - row.BaseQuantity;
		ChangeRowState(row);
	}

	private void CodeButton_Click(object sender, RoutedEventArgs e)
	{
		CodeEnterClick();
	}

	private void BarCodeText_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Invalid comparison between Unknown and I4
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if ((int)e.Key == 6 && (int)Keyboard.Modifiers == 0 && !_barCode.IsReadOnly && !string.IsNullOrWhiteSpace(_barCode.Text))
			{
				ScannerAdd();
			}
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	private void ActualQuantitiesCheckWindow_PreviewKeyDown(object sender, KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Invalid comparison between Unknown and I4
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Invalid comparison between Unknown and I4
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Invalid comparison between Unknown and I4
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Invalid comparison between Unknown and I4
		try
		{
			if ((int)e.Key == 91 && (int)Keyboard.Modifiers == 0)
			{
				OnF2();
				e.Handled = true;
			}
			else if ((int)e.Key == 97 && (int)Keyboard.Modifiers == 0)
			{
				OnF8();
				e.Handled = true;
			}
			else if ((int)e.Key == 6 && (int)Keyboard.Modifiers == 2)
			{
				e.Handled = true;
			}
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	protected void OnF2()
	{
		if (!_barCode.IsReadOnly)
		{
			_barCode.Focus();
		}
	}

	protected void OnF8()
	{
		CodeEnterClick();
	}

	private void ScannerAdd()
	{
		_barCode.DoValidate();
		try
		{
			decimal qty = 1m;
			string unitCode = string.Empty;
			string unitBrief = string.Empty;
			string barCode = _barCode.Text.Trim();
			MaterialCacheRow materialCacheRow = DocumentHelpers.TryGetItemByBarCode(ref barCode, out qty, out unitCode, out unitBrief);
			if (materialCacheRow == null || materialCacheRow.Type == ItemTypesSE.Services.Value)
			{
				_barCode.Focus();
				throw new ASException(string.Format(ArmSoft.SME.Common.Resources.ErrBarCodeNotExist, _barCode.Text.Trim()));
			}
			AddNewRowOrMergeInner(materialCacheRow, unitCode, unitBrief, qty);
			_barCode.Text = string.Empty;
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	public override void OnScanner(string ScannerData)
	{
		if (!_barCode.IsReadOnly)
		{
			_barCode.Text = ScannerData;
			ScannerAdd();
		}
	}

	private void CodeEnterClick()
	{
		if (_codeEnter == null || !_codeEnter.IsEnabled)
		{
			return;
		}
		_barCode.DoValidate();
		try
		{
			string text = _barCode.Text.Trim();
			if (string.IsNullOrWhiteSpace(text))
			{
				GoodsFilter goodsFilter = new GoodsFilter(shortMode: true, ItemTypesSE.Goods.Value);
				ModalBrowser modalBrowser = new ModalBrowser(goodsFilter);
				modalBrowser.Owner = this;
				modalBrowser.WindowStartupLocation = WindowStartupLocation.CenterOwner;
				if (!modalBrowser.Show())
				{
					_barCode.Text = string.Empty;
					_barCode.Focus();
					return;
				}
				text = goodsFilter.ValueMember(modalBrowser.SelectedRowIndex);
			}
			if (text.Length > 20)
			{
				throw new ASException(string.Format(ArmSoft.SME.Trade.Common.Resources.ErrMTCodeLen, 20, text));
			}
			MaterialCacheRow item = TradeSettings.TradeCacheService.Material.GetItem(text);
			if (item == null)
			{
				_barCode.Focus();
				throw new ASException(string.Format(ArmSoft.SME.Trade.Common.Resources.MTEgineInavidMaterial, text));
			}
			AddNewRowOrMergeInner(item);
			_barCode.Text = string.Empty;
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	private void AddNewRowOrMergeInner(MaterialCacheRow goodRow, string unitCode = "", string unitBrief = "", decimal actualQuantity = 1m, decimal baseQuantity = 0m, bool userAdd = true)
	{
		unitCode = (string.IsNullOrWhiteSpace(unitCode) ? goodRow.Unit : unitCode).ToString();
		unitBrief = (string.IsNullOrWhiteSpace(unitBrief) ? goodRow.UnitRow.Brief : unitBrief).ToString();
		ActualQuantitiesCheck.ActualQuantitiesCheckRow row = null;
		for (int i = 0; i <= _mtList.Count - 1; i++)
		{
			if (goodRow.Id == _mtList[i].MTID && unitCode == _mtList[i].UnitCode)
			{
				row = _mtList[i];
				FillMTInvGridRow(row, goodRow, unitCode, unitBrief, actualQuantity, baseQuantity, userAdd);
				int rowHandleByListIndex = _qcGrid.Grid.GetRowHandleByListIndex(i);
				_qcGrid.Grid.RefreshRow(rowHandleByListIndex);
				DialogWindow.SelectCellInGrid(_qcTableView, _qcGrid, rowHandleByListIndex, _actualQuantityCol.Name);
				return;
			}
		}
		AddMTToListNewRow(goodRow, ref row, unitCode, unitBrief, actualQuantity, baseQuantity, userAdd);
	}

	private void AddMTToListNewRow(MaterialCacheRow goodRow, ref ActualQuantitiesCheck.ActualQuantitiesCheckRow row, string unitCode, string unitBrief, decimal actualQuantity = 1m, decimal baseQuantity = 0m, bool userAdd = true)
	{
		row = new ActualQuantitiesCheck.ActualQuantitiesCheckRow();
		FillMTInvGridRow(row, goodRow, unitCode, unitBrief, actualQuantity, baseQuantity, userAdd);
		row.RowID = _maxRowID;
		_mtList.Add(row);
		_maxRowID++;
		int rowHandleByListIndex = _qcGrid.Grid.GetRowHandleByListIndex(_mtList.Count - 1);
		_qcGrid.Grid.RefreshRow(rowHandleByListIndex);
		DialogWindow.SelectCellInGrid(_qcTableView, _qcGrid, rowHandleByListIndex, _actualQuantityCol.Name);
		_qcGrid.ConfigureButtons();
	}

	private void FillMTInvGridRow(ActualQuantitiesCheck.ActualQuantitiesCheckRow row, MaterialCacheRow goodsRow, string unitCode, string unitBrief, decimal actualQuantity, decimal baseQuantity, bool userAdd)
	{
		if (goodsRow != null)
		{
			row.MTID = goodsRow.Id;
			row.MTCode = goodsRow.MTCode;
			row.MTName = goodsRow.Caption;
			row.UnitCode = unitCode;
			row.UnitBrief = unitBrief;
			row.ActualQuantity += actualQuantity;
			row.BaseQuantity += baseQuantity;
			row.VarianceQuantity = row.ActualQuantity - row.BaseQuantity;
			if (!userAdd)
			{
				row.UserAdd = false;
			}
			ChangeRowState(row);
		}
	}

	private void ChangeRowState(ActualQuantitiesCheck.ActualQuantitiesCheckRow row)
	{
		if (row.VarianceQuantity == 0m)
		{
			row.VarianceState = ArmSoft.SME.Common.Resources.Match;
		}
		else if (row.VarianceQuantity > 0m)
		{
			row.VarianceState = ArmSoft.SME.Common.Resources.Surplus;
		}
		else
		{
			row.VarianceState = ArmSoft.SME.Common.Resources.Deficit;
		}
	}

	private void QCGrid_EscapePressed(object sender, EventArgs e)
	{
		DialogResult = false;
	}

	protected override void OnClosing(CancelEventArgs e)
	{
		base.OnClosing(e);
	}

	protected override void DialogValidation(DialogWindowValidationArgs args)
	{
		try
		{
			base.DialogValidation(args);
			HasVariations();
			foreach (ActualQuantitiesCheck.ActualQuantitiesCheckRow mt in _mtList)
			{
				if (Convert.ToBoolean(mt.HasValidationError))
				{
					throw new ASException(ArmSoft.SME.Common.Resources.ErrorInSave);
				}
			}
			QCResult = _mtList.ToList();
		}
		catch (Exception ex)
		{
			args.isValid = false;
			Settings.ErrorHandler(ex);
		}
	}

	private void HasVariations()
	{
		try
		{
			_hasVariations = false;
			foreach (ActualQuantitiesCheck.ActualQuantitiesCheckRow mt in _mtList)
			{
				if (mt.UserAdd)
				{
					_hasVariations = true;
					break;
				}
				if (mt.VarianceQuantity != 0m)
				{
					_hasVariations = true;
					break;
				}
			}
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	private void OnGridControlEnterPressed(object sender, EventArgs e)
	{
		_qcTableView.CommitEditing();
	}

	private void OnShowOnlyVariations(object sender, RoutedEventArgs e)
	{
		string text = _varianceQuantityCol.Name + " <> 0";
		if (!string.IsNullOrEmpty(_qcGrid.Grid.FilterString))
		{
			GridControl grid = _qcGrid.Grid;
			grid.FilterString = grid.FilterString + " AND " + text;
		}
		else
		{
			_qcGrid.Grid.FilterString = text;
		}
		_qcGrid.Grid.RefreshData();
	}

	private void OnExportToExcelButton_click(object sender, RoutedEventArgs e)
	{
		string sFilename = "Quantities Check For " + _docType.ToString() + " " + _docNum.ToString() + " " + _docDate.Day + "." + _docDate.Month + "." + _docDate.Year;
		if (!ShowSaveDialog(ref sFilename))
		{
			return;
		}
		string filterString = _qcTableView.Grid.FilterString;
		XlsxExportOptions xlsxExportOptions = new XlsxExportOptions
		{
			ShowGridLines = false,
			ExportMode = XlsxExportMode.SingleFile,
			SheetName = ArmSoft.SME.Trade.Common.Resources.ActualQuantitiesCheck,
			TextExportMode = TextExportMode.Value
		};
		xlsxExportOptions.RawDataMode = true;
		try
		{
			_qcTableView.Grid.FilterString = null;
			_qcTableView.ExportToXlsx(sFilename, xlsxExportOptions);
			ProcessStartInfo startInfo = new ProcessStartInfo(sFilename)
			{
				UseShellExecute = true
			};
			try
			{
				Process.Start(startInfo);
			}
			catch
			{
				ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Common.Resources.ExportOk + " " + sFilename, ArmSoft.SME.Common.Resources.FileExport);
			}
		}
		catch (IOException ex)
		{
			if (ex.Message.Contains("The process cannot access the file") && ex.Message.Contains("because it is being used by another process."))
			{
				Settings.ErrorHandler(new ASException(string.Format(ArmSoft.SME.Common.Resources.ApplicationCannotAccessTheFile, sFilename)));
			}
			else
			{
				Settings.ErrorHandler(ex);
			}
		}
		catch (Exception ex2)
		{
			Settings.ErrorHandler(ex2);
		}
		_qcTableView.Grid.FilterString = filterString;
	}

	private bool ShowSaveDialog(ref string sFilename)
	{
		SaveFileDialog saveFileDialog = new SaveFileDialog();
		saveFileDialog.DefaultExt = "xlsx";
		saveFileDialog.Filter = "Excel Workbooks|*.xls;*.xlsx";
		saveFileDialog.AddExtension = true;
		saveFileDialog.FileName = sFilename;
		saveFileDialog.OverwritePrompt = true;
		saveFileDialog.Title = "Save file";
		saveFileDialog.ValidateNames = true;
		if (saveFileDialog.ShowDialog().Value)
		{
			sFilename = saveFileDialog.FileName;
			return true;
		}
		return false;
	}

	private void ActualQuantitiesCheckWindow_Closing(object sender, CancelEventArgs e)
	{
		if (DialogResult.HasValue && DialogResult.Value)
		{
			if (!_ignoreVariationsOnClose)
			{
				return;
			}
			if (_hasVariations)
			{
				if (ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Trade.Common.Resources.CorrectVariations, ArmSoft.SME.Trade.Common.Resources.ProgramName, MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.Yes) == MessageBoxResult.No)
				{
					e.Cancel = true;
				}
			}
			else
			{
				DialogResult = false;
			}
		}
		else if (ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Trade.Common.Resources.QuantitiesCheckCloseConfirmation, ArmSoft.SME.Trade.Common.Resources.ProgramName, MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No) == MessageBoxResult.No)
		{
			e.Cancel = true;
		}
	}
}
