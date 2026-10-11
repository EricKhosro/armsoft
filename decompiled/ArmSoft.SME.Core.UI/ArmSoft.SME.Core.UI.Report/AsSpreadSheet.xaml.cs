using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Extensions;
using ArmSoft.SME.Core.UI.Extensions;
using DevExpress.Drawing.Printing;
using DevExpress.Spreadsheet;
using DevExpress.Xpf.Bars;
using DevExpress.Xpf.Ribbon;
using DevExpress.Xpf.Spreadsheet;
using DevExpress.Xpf.Spreadsheet.Internal;
using DevExpress.Xpf.Spreadsheet.Menu;
using DevExpress.XtraPrinting;
using DevExpress.XtraSpreadsheet;
using DevExpress.XtraSpreadsheet.Commands;
using DevExpress.XtraSpreadsheet.Services;
using Microsoft.Win32;
using Serilog;

namespace ArmSoft.SME.Core.UI.Report;

public partial class AsSpreadSheet : SpreadSheetRibbonSupportedPanel, IComponentConnector
{
	public delegate void ShowDialogCommand();

	public BarButtonItemLink AddFunction;

	public BarButtonItemLink EditFunction;

	public BarButtonItemLink DeleteFunction;

	public BarButtonItemLink CreateCopyFunction;

	public BarButtonItemLink PreviewFunction;

	public BarButtonItemLink DocumentHistoryFunction;

	private AsReportViewer _report;

	private string _fileName;

	private int _paramsRowCount;

	private PrintInfo _reportPrintInfo;

	public List<CellRange> MergedRanges = new List<CellRange>();

	public bool IsRepViewerBased { get; set; }

	public string ExportName { get; set; }

	public ShowDialogCommand ShowDialogSub { get; set; }

	public int MaxColumnsCount { get; set; } = 1;

	public AsSpreadSheet()
	{
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		InitializeComponent();
		CaptionImage = ImageService.GetImage("table_export_excel_16.svg");
		_fileName = "";
		Caption = "";
		ToolTip = "";
		_mainDocument.Document.Options.Culture = Thread.CurrentThread.CurrentCulture;
		LayoutUpdated += new SafeEventHandler<EventArgs>(AsSpreadSheet_LayoutUpdated).Handler;
		Loaded += new SafeEventHandler<RoutedEventArgs>(AsSpreadSheet_Loaded).Handler;
		PreviewKeyDown += new SafeEventHandler<KeyEventArgs>(AsSpreadSheet_PreviewKeyDown).Handler;
		_mainDocument.PreviewMouseDoubleClick += new SafeEventHandler<MouseButtonEventArgs>(MyDocument_MouseDoubleClick).Handler;
		_mainDocument.SelectionChanged += new SafeEventHandler<EventArgs>(FlowDocument_SelectionChanged).Handler;
		_mainDocument.UnhandledException += new SafeEventHandler<SpreadsheetUnhandledExceptionEventArgs>(MyDocument_UnhandledException).Handler;
	}

	private void InitSpreadSheet()
	{
		FloatOnDoubleClick = false;
		AllowDrag = true;
		_mainDocument.PopupMenuShowing += new SafeEventHandler<PopupMenuShowingEventArgs>(ShowSPSMenu).Handler;
		_mainDocument.ShowCellToolTipMode = ShowCellToolTipMode.Auto;
		_mainDocument.Options.Behavior.Selection.AllowMultiSelection = true;
	}

	private void SetBarStates()
	{
		if (_report != null)
		{
			RefreshDataViewBarButtonItem.IsEnabled = _report.Report.AllowRefresh;
			RefreshDataViewBarButtonItem.Tag = _report.RefreshDataViewBarButtonItem;
			ShowDataViewDialogBarButtonItem.IsEnabled = _report.Report.DialogSupported;
			ShowDataViewDialogBarButtonItem.Tag = _report.ShowDataViewDialogBarButtonItem;
			ShowDataViewDialogBarButtonItem.ItemClick += new SafeEventHandler<ItemClickEventArgs>(ShowDataViewDialog_ItemClick).Handler;
			DataViewDocumentBarButtonItem.IsEnabled = _report.Report.AllowPreview;
			DataViewDocumentBarButtonItem.Tag = _report.DataViewDocumentBarButtonItem;
			PrintOptionsBarButtonItem.IsEnabled = true;
			PrintOptionsBarButtonItem.Tag = _report.PrintOptionsBarButtonItem;
			ExportDocumentExcelBarButtonItem.IsEnabled = _report.Report.AllowPreview;
			ExportDocumentExcelBarButtonItem.Tag = _report.ExportDocumentExcelBarButtonItem;
			ExportDocumentPDFBarButtonItem.IsEnabled = _report.Report.AllowPreview;
			ExportDocumentPDFBarButtonItem.Tag = _report.ExportDocumentPDFBarButtonItem;
			_bExportPDF.IsEnabled = true;
			DocumentHistoryBarButtonItem.IsEnabled = _report.Report.AllowDocumentHistory;
			DocumentHistoryBarButtonItem.Tag = _report.DocumentHistoryBarButtonItem;
			DataViewAddBarButtonItem.IsEnabled = _report.Report.AllowAdd;
			DataViewAddBarButtonItem.Tag = _report.DataViewAddBarButtonItem;
			DataViewEditBarButtonItem.IsEnabled = _report.Report.AllowEdit || _report.Report.AllowView;
			DataViewEditBarButtonItem.Tag = _report.DataViewEditBarButtonItem;
			DataViewDeleteBarButtonItem.IsEnabled = _report.Report.AllowDelete;
			DataViewDeleteBarButtonItem.Tag = _report.DataViewDeleteBarButtonItem;
			DataViewCreateCopyBarButtonItem.IsEnabled = _report.Report.AllowCreateCopy;
			DataViewCreateCopyBarButtonItem.Tag = _report.DataViewCreateCopyBarButtonItem;
			ExportDocumentWordBarButtonItem.Tag = _report.ExportDocumentWordBarButtonItem;
			ExportDocumentWordBarButtonItem.IsEnabled = _report.Report.AllowPreview;
			_bExportDocumentXML.IsEnabled = false;
			_chkReportDetails.IsEnabled = true;
			IsRepViewerBased = true;
		}
		else
		{
			RefreshDataViewBarButtonItem.IsEnabled = false;
			RefreshDataViewBarButtonItem.Tag = null;
			ShowDataViewDialogBarButtonItem.IsEnabled = ShowDialogSub != null;
			ShowDataViewDialogBarButtonItem.Tag = null;
			DataViewDocumentBarButtonItem.Tag = null;
			PrintOptionsBarButtonItem.Tag = null;
			_chkReportDetails.IsEnabled = false;
			PrintOptionsBarButtonItem.IsEnabled = true;
			ExportDocumentExcelBarButtonItem.Tag = null;
			DocumentHistoryBarButtonItem.Tag = null;
			DataViewAddBarButtonItem.IsEnabled = false;
			DataViewEditBarButtonItem.IsEnabled = false;
			DataViewEditBarButtonItem.Tag = null;
			DataViewDeleteBarButtonItem.IsEnabled = false;
			DataViewCreateCopyBarButtonItem.IsEnabled = false;
			DataViewDocumentBarButtonItem.IsEnabled = false;
			DocumentHistoryBarButtonItem.IsEnabled = false;
			ExportDocumentWordBarButtonItem.IsEnabled = false;
			ExportDocumentExcelBarButtonItem.IsEnabled = false;
			_bExportDocumentXML.IsEnabled = false;
			ExportDocumentPDFBarButtonItem.IsEnabled = false;
		}
		if (Settings.SystemType == SystemTypes.MarketPlace)
		{
			DocumentHistoryBarButtonItem.IsVisible = false;
		}
	}

	public void Show(bool openAsChild = true)
	{
		InitSpreadSheet();
		SetBarStates();
		Settings.HostingEnvironment.AddPanelToDocumentGroup(this, openAsChild);
		Settings.HostingEnvironment.LayoutManager.Activate(this);
		Settings.HostingEnvironment.InitSpreadsheetFunctions(_mainDocument.ActiveWorksheet.Workbook.CustomFunctions);
	}

	public void ShowFormulaBar()
	{
		_formulaBar.SpreadsheetControl = _mainDocument;
	}

	public void FillReport(AsReportViewer rep, int firstRowNum = 1, bool forceRefresh = false)
	{
		int num = 0;
		_report = rep;
		try
		{
			if (_mainDocument.ActiveWorksheet.IsProtected)
			{
				_mainDocument.ActiveWorksheet.Unprotect("");
			}
		}
		catch (Exception)
		{
		}
		if (_mainDocument.ActiveWorksheet.IsProtected)
		{
			MessageBox.Show("Active Sheet is protected!");
			return;
		}
		IsRepViewerBased = true;
		_mainDocument.ActiveWorksheet.ClearContents(_mainDocument.ActiveWorksheet.GetUsedRange());
		_mainDocument.ActiveWorksheet.ClearFormats(_mainDocument.ActiveWorksheet.GetUsedRange());
		_mainDocument.ActiveWorksheet.ClearPrintRange();
		_mainDocument.ActiveWorksheet.UnMergeCells(_mainDocument.ActiveWorksheet.GetUsedRange());
		if (rep.RepRows.Count == 0)
		{
			return;
		}
		if (_paramsRowCount > 0)
		{
			ActiveSheet().Rows.Unhide(0, _paramsRowCount);
		}
		MergedRanges.Clear();
		_mainDocument.ActiveWorksheet.HorizontalPageBreaks.Clear();
		_mainDocument.Document.BeginUpdate();
		_mainDocument.FontFamily = Settings.FontFamily;
		_mainDocument.FontSize = Settings.GetFontSize();
		_mainDocument.ActiveWorksheet.Range["$A:$XFD"].Protection.Locked = false;
		string[] array = rep.ReportTitle().Split("\r\n");
		_paramsRowCount = 0;
		bool value = _chkReportDetails.IsChecked.Value;
		for (int i = 0; i <= array.GetUpperBound(0); i++)
		{
			Cell cell = ActiveSheet().Cells[i, 0];
			cell.Value = array[i];
			cell.Alignment.Horizontal = SpreadsheetHorizontalAlignment.Left;
			cell.Alignment.WrapText = false;
			cell.Font.Name = Settings.FontFamily.FamilyNames.FirstOrDefault().Value;
			cell.Font.Size = Settings.GetFontSize() * 0.8;
			cell.ColumnWidthInCharacters = 5.0;
			if (!value)
			{
				ActiveSheet().Rows.Hide(i, i);
			}
		}
		if (array.Length != 0)
		{
			_paramsRowCount = array.Length + 1;
		}
		int num2 = -1;
		for (int j = 0; j <= rep.RepRows.Count - 1; j++)
		{
			if (rep.RepRows[j].IsTableCaption && num2 == -1)
			{
				num2 = Convert.ToInt32(j);
			}
			AddRepRow(rep.RepRows[j], num + _paramsRowCount, j);
			num++;
		}
		if (num2 != -1)
		{
			try
			{
				int num3 = 0;
				for (int k = 0; k <= rep.RepRows[num2].RowCells.Count - 1; k++)
				{
					num3 += rep.RepRows[num2].RowCells[k].ColumnSpan;
				}
				num2 += _paramsRowCount;
				CellRange range = _mainDocument.ActiveWorksheet.Range.FromLTRB(0, num2, num3 - 1, rep.RepRows.Count + _paramsRowCount - 1);
				_mainDocument.ActiveWorksheet.AutoFilter.Apply(range);
			}
			catch (Exception)
			{
			}
		}
		_mainDocument.Document.DocumentSettings.Calculation.Mode = CalculationMode.Automatic;
		_mainDocument.ActiveWorksheet.PrintOptions.AutoPageBreaks = false;
		if (array.Length != 0)
		{
			ActiveSheet().Rows.Hide(0, array.GetUpperBound(0) + 1);
		}
		if (_report.UnmovableHeaderLinesCount > 0 || _paramsRowCount > 0)
		{
			_mainDocument.ActiveWorksheet.FreezeRows(_report.UnmovableHeaderLinesCount - 1 + _paramsRowCount);
		}
		SetBarStates();
		_mainDocument.ActiveWorksheet.ActiveView.ShowGridlines = false;
		_mainDocument.ActiveWorksheet.Protect("", WorksheetProtectionPermissions.Default | WorksheetProtectionPermissions.FormatCells | WorksheetProtectionPermissions.FormatColumns | WorksheetProtectionPermissions.FormatRows | WorksheetProtectionPermissions.AutoFilters);
		SetCaption();
		SetPrintSettings(ActiveSheet().Index);
		_mainDocument.Document.EndUpdate();
	}

	public static void OpenFile()
	{
		OpenFileDialog openFileDialog = new OpenFileDialog
		{
			FileName = "",
			DefaultExt = ".xlsx",
			Filter = "Excel documents (.xls)|*.xls;*.xlsx;*.xlsm"
		};
		bool? flag = openFileDialog.ShowDialog();
		if (flag.HasValue && flag.Value)
		{
			string fileName = openFileDialog.FileName;
			AsSpreadSheet asSpreadSheet = new AsSpreadSheet();
			asSpreadSheet.LoadFile(fileName);
			asSpreadSheet.Show();
		}
	}

	public void LoadFile(string filePath)
	{
		string extension = new FileInfo(filePath).Extension;
		DocumentFormat format = DocumentFormat.Xls;
		if (extension == ".xlsx")
		{
			format = DocumentFormat.Xlsx;
		}
		else if (extension == ".xlsm")
		{
			format = DocumentFormat.Xlsm;
		}
		_mainDocument.LoadDocument(filePath, format);
		Settings.HostingEnvironment.InitSpreadsheetFunctions(_mainDocument.ActiveWorksheet.Workbook.CustomFunctions);
	}

	public Worksheet ActiveSheet()
	{
		return _mainDocument.ActiveWorksheet;
	}

	public SpreadsheetControl GetSpreadSheetControl()
	{
		return _mainDocument;
	}

	public void SetPrintSettings(int index = -1)
	{
		Worksheet worksheet = ((index != -1) ? _mainDocument.Document.Worksheets[index] : ActiveSheet());
		worksheet.PrintOptions.PageOrder = PageOrder.OverThenDown;
		if (IsRepViewerBased && worksheet.IsProtected)
		{
			_reportPrintInfo = new PrintInfo("SPS_" + _report.ReportName);
		}
		else
		{
			_reportPrintInfo = new PrintInfo("SPS_");
		}
		PrintInfo printInfo = new PrintInfo();
		_reportPrintInfo.PrintASInfo = printInfo.PrintASInfo;
		worksheet.PrintOptions.AutoPageBreaks = false;
		worksheet.PrintOptions.Scale = _reportPrintInfo.ScalePercent;
		if (IsRepViewerBased && worksheet.IsProtected)
		{
			if (_reportPrintInfo.EnablePageBreaks)
			{
				FillPageBreaks();
				_mainDocument.ActiveWorksheet.PrintOptions.AutoPageBreaks = false;
			}
			else
			{
				ActiveSheet().HorizontalPageBreaks.Clear();
				_mainDocument.ActiveWorksheet.PrintOptions.AutoPageBreaks = true;
			}
		}
		_mainDocument.ActiveWorksheet.PrintOptions.PrintGridlines = _reportPrintInfo.ShowLines;
		if (worksheet.ActiveView != null)
		{
			worksheet.ActiveView.PaperKind = DXPaperKind.A4;
			worksheet.ActiveView.Orientation = ((!(_reportPrintInfo.PaperOrientation == "Landscape")) ? PageOrientation.Portrait : PageOrientation.Landscape);
			worksheet.ActiveView.Margins.Left = _reportPrintInfo.MarginLeft * 100f;
			worksheet.ActiveView.Margins.Right = _reportPrintInfo.MarginRight * 100f;
			worksheet.ActiveView.Margins.Top = _reportPrintInfo.MarginTop * 100f;
			worksheet.ActiveView.Margins.Bottom = _reportPrintInfo.MarginBottom * 100f;
			worksheet.ActiveView.Margins.Header = 0f;
			WorksheetHeaderFooterOptions headerFooterOptions = _mainDocument.ActiveWorksheet.HeaderFooterOptions;
			headerFooterOptions.AlignWithMargins = true;
			if (_reportPrintInfo.PrintDate)
			{
				headerFooterOptions.EvenFooter.Left = DateTime.Now.FormatDDMMYY();
				headerFooterOptions.OddFooter.Left = DateTime.Now.FormatDDMMYY();
			}
			_ = _reportPrintInfo.PrintCompanyName;
			if (_reportPrintInfo.PrintPageNumbers)
			{
				headerFooterOptions.FirstFooter.Right = string.Format("Page {0} of {1}", "&P", "&N");
				headerFooterOptions.OddFooter.Right = string.Format("Page {0} of {1}", "&P", "&N");
				headerFooterOptions.EvenFooter.Right = string.Format("Page {0} of {1}", "&P", "&N");
			}
		}
		CellRange usedRange = ActiveSheet().GetUsedRange();
		IList<CellRange> selectedRanges = ActiveSheet().GetSelectedRanges();
		bool flag = false;
		foreach (CellRange item in selectedRanges)
		{
			if (!item.IsMerged && item.GetReferenceA1() != ActiveSheet().SelectedCell.GetReferenceA1())
			{
				flag = true;
				break;
			}
		}
		if (flag)
		{
			ActiveSheet().ClearPrintRange();
			{
				foreach (CellRange item2 in selectedRanges)
				{
					CellRange cellRange = usedRange.Intersect(item2);
					if (cellRange != null)
					{
						ActiveSheet().AddPrintRange(cellRange);
					}
				}
				return;
			}
		}
		if (IsRepViewerBased && ActiveSheet().IsProtected && _paramsRowCount > 0)
		{
			if (_reportPrintInfo.PrintHeader == "DoNotPrint")
			{
				ActiveSheet().SetPrintRange(ActiveSheet().GetUsedRange().Intersect(ActiveSheet().Range.FromLTRB(0, _paramsRowCount - 1, usedRange.RightColumnIndex, usedRange.BottomRowIndex)));
			}
			else
			{
				ActiveSheet().SetPrintRange(ActiveSheet().Range.FromLTRB(0, 0, usedRange.RightColumnIndex, usedRange.BottomRowIndex));
			}
		}
	}

	private void AddRepRow(AsRepRow repRow, int rowNum, int rowInfoNum)
	{
		List<AsRepRowCell> rowCells = repRow.RowCells;
		GetRowSizeAndStyle(repRow);
		int num = 0;
		if (rowCells.Count > MaxColumnsCount)
		{
			MaxColumnsCount = rowCells.Count;
		}
		for (int i = 0; i <= rowCells.Count - 1; i++)
		{
			if (i == 0)
			{
				CreateCell(rowNum, rowCells[i], i, rowInfoNum);
			}
			else
			{
				CreateCell(rowNum, rowCells[i], num, rowInfoNum);
			}
			num += rowCells[i].ColumnSpan;
		}
	}

	private static double DefaultColWidth(AsRepColStyle style)
	{
		return style switch
		{
			AsRepColStyle.TextStyle => 250.0, 
			AsRepColStyle.CurrencyStyle => 50.0, 
			AsRepColStyle.BooleanStyle => 50.0, 
			AsRepColStyle.DateStyle => 80.0, 
			_ => 50.0, 
		};
	}

	private void GetRowSizeAndStyle(AsRepRow repRow)
	{
		FormatRow(repRow);
		List<AsRepRowCell> rowCells = repRow.RowCells;
		if (_report.TablesDefs.TryGetValue(repRow.BlockName, out var value))
		{
			for (int i = 0; i <= rowCells.Count - 1; i++)
			{
				rowCells[i].WidthD = 0.0;
				if (rowCells[i].ColumnSpan < 1)
				{
					rowCells[i].ColumnSpan = 1;
				}
			}
			int num = 0;
			for (int j = 0; j <= rowCells.Count - 1; j++)
			{
				for (int k = num; k <= num + rowCells[j].ColumnSpan - 1; k++)
				{
					if (value.ColumnDefinitions[k].WidthD == 0.0)
					{
						rowCells[j].WidthD += DefaultColWidth(value.ColumnDefinitions[k].ValuesStyle);
					}
					else
					{
						rowCells[j].WidthD += value.ColumnDefinitions[k].WidthD;
					}
				}
				FormatCell(rowCells[j], value.ColumnDefinitions[num], repRow);
				num = ((rowCells[j].ColumnSpan <= 1) ? (num + 1) : (num + rowCells[j].ColumnSpan));
			}
			return;
		}
		for (int l = 0; l <= rowCells.Count - 1; l++)
		{
			if (rowCells[l].WidthD == 0.0)
			{
				rowCells[l].WidthD = DefaultColWidth(rowCells[l].ValueStyle);
			}
			FormatCell(rowCells[l], repRow);
		}
	}

	private static void FormatRow(AsRepRow repRow)
	{
		if (repRow.StyleCollection.TryGetValue("FontStyle", out var value))
		{
			repRow.FontStyle = (System.Windows.FontStyle)value;
		}
		if (repRow.StyleCollection.TryGetValue("FontWeight", out var value2))
		{
			repRow.FontWeight = (FontWeight)value2;
		}
		if (repRow.StyleCollection.TryGetValue("FontSize", out var value3))
		{
			repRow.FontSize = (int)value3;
		}
		if (repRow.RowType == AsRepRowType.Text)
		{
			_ = repRow.RowCells.Count;
			_ = 0;
		}
	}

	private static object DefaultRowFormat(AsRepRow repRow, string formatKey)
	{
		int num = 10;
		if (formatKey == "FontStyle")
		{
			return FontStyles.Normal;
		}
		if (formatKey == "FontWeight")
		{
			return FontWeights.Normal;
		}
		switch (repRow.RowStyle)
		{
		case AsRepRowStyle.CaptionRow:
			if (!(formatKey == "FontWeight"))
			{
				if (!(formatKey == "FontSize"))
				{
					break;
				}
				return num;
			}
			return FontWeights.Bold;
		case AsRepRowStyle.TableColumnsHeader:
			if (!(formatKey == "FontWeight"))
			{
				if (!(formatKey == "FontSize"))
				{
					break;
				}
				return num;
			}
			return FontWeights.Bold;
		default:
			if (!(formatKey == "FontWeight"))
			{
				if (!(formatKey == "FontSize"))
				{
					break;
				}
				return num;
			}
			return FontWeights.Normal;
		}
		return null;
	}

	private static void FormatCell(AsRepRowCell repRowCell, AsRepColumn repColumn, AsRepRow repRow)
	{
		if (!repRowCell.StyleCollection.ContainsKey("Alignment"))
		{
			object value2;
			if (repRow.IsTableCaption)
			{
				if (repColumn.StyleCollection.TryGetValue("CaptionAlignment", out var value))
				{
					repRowCell.Alignment = (System.Windows.TextAlignment)value;
				}
			}
			else if (repColumn.StyleCollection.TryGetValue("DataAlignment", out value2))
			{
				repRowCell.Alignment = (System.Windows.TextAlignment)value2;
			}
		}
		if (!repRowCell.StyleCollection.ContainsKey("Alignment"))
		{
			repRowCell.Alignment = System.Windows.TextAlignment.Left;
		}
		if (!repRowCell.StyleCollection.ContainsKey("FontWeight"))
		{
			object value5;
			if (repRow.StyleCollection.TryGetValue("FontWeight", out var value3))
			{
				repRowCell.FontWeight = (FontWeight)value3;
			}
			else if (repRow.IsTableCaption)
			{
				if (repColumn.StyleCollection.TryGetValue("CaptionFontWeight", out var value4))
				{
					repRowCell.FontWeight = (FontWeight)value4;
				}
			}
			else if (repColumn.StyleCollection.TryGetValue("DataFontWeight", out value5))
			{
				repRowCell.FontWeight = (FontWeight)value5;
			}
		}
		if (!repRowCell.StyleCollection.ContainsKey("FontWeight"))
		{
			repRowCell.FontWeight = (FontWeight)DefaultRowFormat(repRow, "FontWeight");
		}
		if (!repRowCell.StyleCollection.ContainsKey("FontStyle"))
		{
			object value8;
			if (repRow.StyleCollection.TryGetValue("FontStyle", out var value6))
			{
				repRowCell.FontStyle = (System.Windows.FontStyle)value6;
			}
			else if (repRow.IsTableCaption)
			{
				if (repColumn.StyleCollection.TryGetValue("CaptionFontStyle", out var value7))
				{
					repRowCell.FontStyle = (System.Windows.FontStyle)value7;
				}
			}
			else if (repColumn.StyleCollection.TryGetValue("DataFontStyle", out value8))
			{
				repRowCell.FontStyle = (System.Windows.FontStyle)value8;
			}
		}
		if (!repRowCell.StyleCollection.ContainsKey("FontStyle"))
		{
			repRowCell.FontStyle = (System.Windows.FontStyle)DefaultRowFormat(repRow, "FontStyle");
		}
		if (!repRowCell.StyleCollection.ContainsKey("FontSize"))
		{
			object value11;
			if (repRow.StyleCollection.TryGetValue("FontSize", out var value9))
			{
				repRowCell.FontSize = (int)value9;
			}
			else if (repRow.IsTableCaption)
			{
				if (repColumn.StyleCollection.TryGetValue("CaptionFontSize", out var value10))
				{
					repRowCell.FontSize = (int)value10;
				}
			}
			else if (repColumn.StyleCollection.TryGetValue("DataFontSize", out value11))
			{
				repRowCell.FontSize = (int)value11;
			}
		}
		if (!repRowCell.StyleCollection.ContainsKey("FontSize"))
		{
			repRowCell.FontSize = Convert.ToInt32(DefaultRowFormat(repRow, "FontSize"));
		}
		if (!repRowCell.StyleCollection.ContainsKey("CurrencyLen") && repColumn.StyleCollection.TryGetValue("CurrencyLen", out var value12))
		{
			repRowCell.CurrencyLen = (short)value12;
		}
		if (!repRowCell.StyleCollection.ContainsKey("CurrencyLen"))
		{
			repRowCell.CurrencyLen = 16;
		}
		if (!repRowCell.StyleCollection.ContainsKey("CurrencyPrecision") && repColumn.StyleCollection.TryGetValue("CurrencyPrecision", out var value13))
		{
			repRowCell.CurrencyPrecision = (short)value13;
		}
		if (!repRowCell.StyleCollection.ContainsKey("CurrencyPrecision"))
		{
			repRowCell.CurrencyPrecision = 2;
		}
		if (repRowCell.Borders == null || repRowCell.Borders.Count == 0)
		{
			object value15;
			if (repRow.IsTableCaption)
			{
				if (repColumn.StyleCollection.TryGetValue("CaptionBorders", out var value14))
				{
					repRowCell.Borders = (ObservableCollection<BorderType>)value14;
				}
			}
			else if (repColumn.StyleCollection.TryGetValue("DataBorders", out value15))
			{
				repRowCell.Borders = (ObservableCollection<BorderType>)value15;
			}
		}
		if (!repRowCell.StyleCollection.ContainsKey("BackgroundColor"))
		{
			object value17;
			if (repRow.IsTableCaption)
			{
				if (repColumn.StyleCollection.TryGetValue("CaptionBackgroundColor", out var value16))
				{
					repRowCell.BackGround = (System.Windows.Media.Color)value16;
				}
			}
			else if (repColumn.StyleCollection.TryGetValue("DataBackgroundColor", out value17))
			{
				repRowCell.BackGround = (System.Windows.Media.Color)value17;
			}
		}
		if (!repRowCell.StyleCollection.ContainsKey("BackgroundColor"))
		{
			repRowCell.BackGround = Colors.Transparent;
		}
		if (!repRowCell.StyleCollection.ContainsKey("ForeGroundColor"))
		{
			object value19;
			if (repRow.IsTableCaption)
			{
				if (repColumn.StyleCollection.TryGetValue("CaptionForegroundColor", out var value18))
				{
					repRowCell.ForeGround = (System.Windows.Media.Color)value18;
				}
			}
			else if (repColumn.StyleCollection.TryGetValue("DataForegroundColor", out value19))
			{
				repRowCell.ForeGround = (System.Windows.Media.Color)value19;
			}
		}
		repRowCell.StyleCollection.ContainsKey("ForeGroundColor");
	}

	private static void FormatCell(AsRepRowCell repRowCell, AsRepRow repRow)
	{
		if (!repRowCell.StyleCollection.ContainsKey("Alignment"))
		{
			repRowCell.Alignment = System.Windows.TextAlignment.Left;
		}
		if (!repRowCell.StyleCollection.ContainsKey("FontWeight"))
		{
			if (repRow.StyleCollection.TryGetValue("FontWeight", out var value))
			{
				repRowCell.FontWeight = (FontWeight)value;
			}
			else
			{
				repRowCell.FontWeight = (FontWeight)DefaultRowFormat(repRow, "FontWeight");
			}
		}
		if (!repRowCell.StyleCollection.ContainsKey("FontStyle"))
		{
			if (repRow.StyleCollection.TryGetValue("FontStyle", out var value2))
			{
				repRowCell.FontStyle = (System.Windows.FontStyle)value2;
			}
			else
			{
				repRowCell.FontStyle = (System.Windows.FontStyle)DefaultRowFormat(repRow, "FontStyle");
			}
		}
		if (!repRowCell.StyleCollection.ContainsKey("FontSize"))
		{
			if (repRow.StyleCollection.TryGetValue("FontSize", out var value3))
			{
				repRowCell.FontSize = (int)value3;
			}
			else
			{
				repRowCell.FontSize = Convert.ToInt32(DefaultRowFormat(repRow, "FontSize"));
			}
		}
	}

	private static System.Drawing.Color GetColor()
	{
		System.Windows.Media.Color color = ((SolidColorBrush)Settings.GetActiveRowBrush()).Color;
		return System.Drawing.Color.FromArgb(color.A, color.R, color.G, color.B);
	}

	private void CreateCell(int rowNum, AsRepRowCell cell, int cellNum, int rowInfoNum)
	{
		Worksheet activeWorksheet = _mainDocument.ActiveWorksheet;
		CellRange cellRange = activeWorksheet.Range.FromLTRB(cellNum, rowNum, cellNum + cell.ColumnSpan - 1, rowNum);
		if (cell.ColumnSpan > 1)
		{
			cellRange.Merge(MergeCellsMode.IgnoreIntersections);
			MergedRanges.Add(cellRange);
		}
		if (cell.Borders != null && cell.Borders.Count > 0)
		{
			if (cell.Borders.Contains(BorderType.AllBorders))
			{
				cellRange.Borders.BottomBorder.Color = System.Drawing.Color.White;
				cellRange.Borders.BottomBorder.LineStyle = BorderLineStyle.Medium;
				cellRange.Borders.LeftBorder.Color = System.Drawing.Color.White;
				cellRange.Borders.LeftBorder.LineStyle = BorderLineStyle.Medium;
				cellRange.Borders.TopBorder.Color = System.Drawing.Color.White;
				cellRange.Borders.TopBorder.LineStyle = BorderLineStyle.Medium;
				cellRange.Borders.RightBorder.Color = System.Drawing.Color.White;
				cellRange.Borders.RightBorder.LineStyle = BorderLineStyle.Medium;
				cellRange.FillColor = GetColor();
			}
			else if (cell.Borders.Contains(BorderType.LeftBorder))
			{
				cellRange.Borders.LeftBorder.Color = GetColor();
				cellRange.Borders.LeftBorder.LineStyle = BorderLineStyle.Medium;
			}
			else if (cell.Borders.Contains(BorderType.TopBorder))
			{
				cellRange.Borders.TopBorder.Color = GetColor();
				cellRange.Borders.TopBorder.LineStyle = BorderLineStyle.Medium;
			}
			else if (cell.Borders.Contains(BorderType.RightBorder))
			{
				cellRange.Borders.RightBorder.Color = GetColor();
				cellRange.Borders.RightBorder.LineStyle = BorderLineStyle.Medium;
			}
			else if (cell.Borders.Contains(BorderType.BottomBorder))
			{
				cellRange.Borders.BottomBorder.Color = GetColor();
				cellRange.Borders.BottomBorder.LineStyle = BorderLineStyle.Medium;
			}
		}
		if (cell.ColumnSpan > 1)
		{
			cellRange = activeWorksheet.Cells[rowNum, cellNum];
		}
		if (cell.ValueStyle == AsRepColStyle.CurrencyStyle && cell.Value.ToString().IsNumeric())
		{
			int num = cell.ValueToString.InStr(".");
			int num2 = ((num > 0) ? cell.ValueToString.Mid(num + 1).Length : 0);
			cellRange.NumberFormat = GetExcelNumberFormatByPrecision(num2);
			cellRange.Value = Convert.ToDecimal(cell.Value).CurrencyFormat(16, (short)num2);
			cellRange.Alignment.Horizontal = SpreadsheetHorizontalAlignment.Right;
			cellRange.Alignment.WrapText = false;
		}
		else if ((cell.ValueStyle == AsRepColStyle.DateStyle) & (cell.Value is DateTime))
		{
			cellRange.Value = Convert.ToDateTime(cell.Value);
			cellRange.Alignment.Horizontal = SpreadsheetHorizontalAlignment.Left;
			cellRange.Alignment.WrapText = false;
			cellRange.NumberFormat = "dd/mm/yy";
		}
		else if (cell.ValueStyle == AsRepColStyle.BooleanStyle)
		{
			cellRange.Value = (((bool)cell.Value) ? "Yes" : "No");
			cellRange.Alignment.Horizontal = SpreadsheetHorizontalAlignment.Center;
			cellRange.Alignment.WrapText = false;
		}
		else
		{
			cellRange.Value = Convert.ToString(cell.Value);
			switch (cell.Alignment)
			{
			case System.Windows.TextAlignment.Center:
				cellRange.Alignment.Horizontal = SpreadsheetHorizontalAlignment.Center;
				break;
			case System.Windows.TextAlignment.Left:
				cellRange.Alignment.Horizontal = SpreadsheetHorizontalAlignment.Left;
				break;
			case System.Windows.TextAlignment.Right:
				cellRange.Alignment.Horizontal = SpreadsheetHorizontalAlignment.Right;
				break;
			case System.Windows.TextAlignment.Justify:
				cellRange.Alignment.Horizontal = SpreadsheetHorizontalAlignment.Justify;
				break;
			default:
				cellRange.Alignment.Horizontal = SpreadsheetHorizontalAlignment.Center;
				break;
			}
			if (_report.RepRows[rowInfoNum].RowCells.Count == 1 || cell.ColumnSpan > 1)
			{
				cellRange.Alignment.WrapText = false;
			}
			else
			{
				cellRange.Alignment.WrapText = true;
			}
		}
		if (cell.ForeGround.ColorContext != null)
		{
			cellRange.Font.Color = System.Drawing.Color.FromArgb(cell.ForeGround.A, cell.ForeGround.R, cell.ForeGround.G, cell.ForeGround.B);
		}
		cellRange.Font.Size = cell.FontSize;
		double num3 = 3.1;
		if ((cell.FontWeight == FontWeights.Bold) | _report.RepRows[rowInfoNum].IsTableCaption)
		{
			cellRange.Font.Bold = true;
			cellRange.Font.Size = (double)cell.FontSize * 0.9;
		}
		if (cell.FontStyle == FontStyles.Italic)
		{
			cellRange.Font.Italic = true;
		}
		cellRange.Alignment.Vertical = SpreadsheetVerticalAlignment.Center;
		if (_report.RepRows[rowInfoNum].RowCells.Count > 1 && (cellRange.TopRowIndex == 0 || cellRange.Worksheet.Columns[cellRange.LeftColumnIndex].Width < cell.WidthD * num3) && cell.ColumnSpan == 1)
		{
			cellRange.Worksheet.Columns[cellRange.LeftColumnIndex].Width = cell.WidthD * num3;
		}
		cellRange.Font.Name = Settings.FontFamily.FamilyNames.FirstOrDefault().Value;
		cellRange.Protection.Locked = true;
	}

	private void ShowSPSMenu(object sender, PopupMenuShowingEventArgs e)
	{
		if (!IsRepViewerBased || !ActiveSheet().IsProtected || e.MenuType != SpreadsheetMenuType.Cell)
		{
			return;
		}
		int num = _mainDocument.Document.Worksheets.ActiveWorksheet.SelectedCell.TopRowIndex - _paramsRowCount;
		if (num > _report.RepRows.Count - 1)
		{
			return;
		}
		bool flag = false;
		if (_mainDocument.ActiveWorksheet.SelectedCell.Protection.Locked)
		{
			flag = true;
		}
		_report.Report.GetContextMenu(_report.RepRows[num].HiddenData);
		PopupMenu reportContextMenu = _report.ReportContextMenu;
		foreach (BarItemLinkBase itemLink in e.Menu.ItemLinks)
		{
			try
			{
				if (!(itemLink is BarItemLink barItemLink))
				{
					continue;
				}
				if (barItemLink.Item.CategoryName == "AS Items")
				{
					e.Menu.ItemLinks.Remove(itemLink);
				}
				else if (flag)
				{
					if (barItemLink.Item.Content.ToString() != "Copy")
					{
						itemLink.IsVisible = false;
					}
					else if (!flag)
					{
						itemLink.IsVisible = true;
					}
				}
				else
				{
					itemLink.IsVisible = true;
				}
			}
			catch (Exception)
			{
			}
		}
		if (_report.RepRows[num].HiddenData == null)
		{
			return;
		}
		foreach (BarItemLink item in reportContextMenu.ItemLinks.Cast<BarItemLink>())
		{
			if (item.Item is BarButtonItem { IsEnabled: not false } barButtonItem)
			{
				BarButtonItem barButtonItem2 = new BarButtonItem
				{
					Glyph = barButtonItem.Glyph,
					Content = barButtonItem.Content,
					CategoryName = "AS Items",
					Tag = barButtonItem
				};
				barButtonItem2.ItemClick += new SafeEventHandler<ItemClickEventArgs>(ExecMenuFunction).Handler;
				BarManager.Items.Add(barButtonItem2);
				e.Menu.ItemLinks.Add(barButtonItem2.CreateLink());
			}
		}
	}

	protected override void BarManager_ItemClick(object sender, ItemClickEventArgs e)
	{
		base.BarManager_ItemClick(sender, e);
		if (e.Item is BarButtonItem barButtonItem)
		{
			BarButtonItem barButtonItem2 = (BarButtonItem)barButtonItem.Tag;
			if (barButtonItem2 != null)
			{
				_report.ActiveRow.ActiveRow = _mainDocument.ActiveWorksheet.SelectedCell.TopRowIndex - _paramsRowCount;
				barButtonItem2.PerformClick();
			}
		}
	}

	private void ExecMenuFunction(object sender, ItemClickEventArgs e)
	{
		if (ActiveSheet().IsProtected)
		{
			BarButtonItem barButtonItem = (BarButtonItem)((BarButtonItem)sender).Tag;
			if (barButtonItem != null)
			{
				_report.ActiveRow.ActiveRow = _mainDocument.ActiveWorksheet.SelectedCell.TopRowIndex - _paramsRowCount;
				barButtonItem.PerformClick();
			}
		}
	}

	private void SetCaption()
	{
		if (IsRepViewerBased)
		{
			Caption = _report.Report.Caption().ToString();
			ToolTip = _report.ReportTitle();
		}
		else
		{
			Caption = "";
			ToolTip = "";
		}
		if (!string.IsNullOrWhiteSpace(_mainDocument.Options.Save.CurrentFileName))
		{
			string name = new FileInfo(_mainDocument.Options.Save.CurrentFileName).Name;
			Caption = Caption.ToString() + "(" + name + ")";
			if (!IsRepViewerBased)
			{
				ToolTip = _mainDocument.Options.Save.CurrentFileName;
			}
			else
			{
				ToolTip = _report.ReportTitle() + "\r\n" + _mainDocument.Options.Save.CurrentFileName;
			}
		}
	}

	private void FillPageBreaks()
	{
		for (int i = 0; i <= _report.RepRows.Count - 1; i++)
		{
			if (_report.pageBreaks.ContainsKey(i))
			{
				_mainDocument.ActiveWorksheet.HorizontalPageBreaks.Add(i + _paramsRowCount);
			}
		}
	}

	private static string GetExcelNumberFormatByPrecision(int precision)
	{
		return precision switch
		{
			1 => "#,##0.0;[Red](#,##0.0)", 
			2 => "#,##0.00;[Red](#,##0.00)", 
			3 => "#,##0.000;[Red](#,##0.000)", 
			4 => "#,##0.0000;[Red](#,##0.0000)", 
			_ => "#,##0;[Red](#,##0)", 
		};
	}

	private void MyDocument_MouseDoubleClick(object sender, MouseButtonEventArgs e)
	{
		if (e.OriginalSource is CellControl && ActiveSheet().IsProtected && IsRepViewerBased)
		{
			int num = _mainDocument.Document.Worksheets.ActiveWorksheet.SelectedCell.TopRowIndex - _paramsRowCount;
			if (num <= _report.RepRows.Count - 1)
			{
				_report.ActiveRow.ActiveRow = num;
				_report.ExecDefaultFunction();
				e.Handled = true;
			}
		}
	}

	private void FlowDocument_SelectionChanged(object sender, EventArgs e)
	{
		try
		{
			decimal value = 0m;
			IList<CellRange> selectedRanges = _mainDocument.GetSelectedRanges();
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			decimal result = 0m;
			foreach (CellRange item in selectedRanges)
			{
				Worksheet worksheet = item.Worksheet;
				CellRange cellRange = worksheet.GetUsedRange().Intersect(item);
				if (cellRange != null)
				{
					for (int i = cellRange.TopRowIndex; i <= cellRange.BottomRowIndex; i++)
					{
						for (int j = cellRange.LeftColumnIndex; j <= cellRange.RightColumnIndex; j++)
						{
							string key = worksheet.Name + "R" + i + "C" + j;
							if (dictionary.TryAdd(key, ""))
							{
								CellValue value2 = worksheet.Cells[i, j].Value;
								if (value2.Type == CellValueType.Numeric)
								{
									value += (decimal)value2.NumericValue;
								}
								else if (value2.Type == CellValueType.Text && decimal.TryParse(value2.TextValue, out result))
								{
									value += result;
								}
							}
						}
					}
				}
				Settings.HostingEnvironment.StatusBarCurrentRow("Sum= " + value.CurrencyFormat(20, 2));
			}
		}
		catch (Exception)
		{
			Settings.HostingEnvironment.StatusBarCurrentRow("Sum= 0.00");
		}
	}

	private void AsSpreadSheet_LayoutUpdated(object sender, EventArgs e)
	{
		try
		{
			if (_fileName != null && _fileName != _mainDocument.Options.Save.CurrentFileName)
			{
				_fileName = _mainDocument.Options.Save.CurrentFileName;
				SetCaption();
			}
		}
		catch (Exception ex)
		{
			string text;
			if (_mainDocument == null)
			{
				text = "MyDocument Is Nothing";
			}
			else if (_mainDocument.Options == null)
			{
				text = "Options Is Nothing";
			}
			else
			{
				text = ((_mainDocument.Options.Save.CurrentFileName != null) ? "Set Caption failed" : "Save.CurrentFileName Is Nothing");
			}
			Log.Warning("AsSpreadSheet_LayoutUpdated", text + ": " + ex.Message);
		}
	}

	private void AsSpreadSheet_Loaded(object sender, RoutedEventArgs e)
	{
		RibbonControl ribbonControl = SpreadSheetRibbonSupportedPanel.FindRibbon((DependencyObject)(object)_mainDocument);
		if (ribbonControl == null)
		{
			return;
		}
		ribbonControl.SelectedPageOnMerging = SelectedPageOnMerging.ParentSelectedPage;
		RibbonPage item = CreateMainPage();
		int num = ribbonControl.ActualCategories.Count - 1;
		for (int i = 0; i <= num; i++)
		{
			RibbonPageCategory ribbonPageCategory = new RibbonPageCategory
			{
				Color = ((SolidColorBrush)Settings.GetActiveRowBrush()).Color
			};
			while (ribbonControl.ActualCategories[i].Pages.Count > 0)
			{
				RibbonPage item2 = ribbonControl.ActualCategories[i].Pages[0];
				ribbonControl.ActualCategories[i].Pages.RemoveAt(0);
				ribbonPageCategory.Pages.Add(item2);
			}
			ribbonControl.Categories.Add(ribbonPageCategory);
		}
		ribbonControl.ActualCategories[0].Pages.Insert(0, item);
	}

	private void AsSpreadSheet_PreviewKeyDown(object sender, KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Invalid comparison between Unknown and I4
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Invalid comparison between Unknown and I4
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Invalid comparison between Unknown and I4
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Invalid comparison between Unknown and I4
		if ((int)e.Key == 58 && (int)Keyboard.Modifiers == 2)
		{
			Settings.HostingEnvironment.ShowOptionsWindow();
			e.Handled = true;
		}
		else if (((int)e.Key == 6 || (int)e.Key == 6) && ActiveSheet().IsProtected && IsRepViewerBased)
		{
			int num = _mainDocument.Document.Worksheets.ActiveWorksheet.SelectedCell.TopRowIndex - _paramsRowCount;
			if (num <= _report.RepRows.Count - 1)
			{
				_report.ActiveRow.ActiveRow = num;
				_report.ExecDefaultFunction();
				e.Handled = true;
			}
		}
	}

	private void ShowPrintOptions(object sender, ItemClickEventArgs e)
	{
		WndPageSetup wndPageSetup = new WndPageSetup();
		if (IsRepViewerBased && ActiveSheet().IsProtected)
		{
			wndPageSetup.ReportKind = _report.ReportName;
		}
		else
		{
			wndPageSetup.ReportKind = "";
		}
		if (wndPageSetup.ShowDialog().Value)
		{
			SetPrintSettings();
		}
	}

	private void ShowDataViewDialog_ItemClick(object sender, ItemClickEventArgs e)
	{
		ShowDialogSub?.Invoke();
	}

	private void OpenFile(object sender, ItemClickEventArgs e)
	{
		OpenFile();
	}

	private void NewSpreadSheet(object sender, ItemClickEventArgs e)
	{
		new AsSpreadSheet().Show();
	}

	private void MyDocument_UnhandledException(object sender, SpreadsheetUnhandledExceptionEventArgs e)
	{
		MessageBox.Show(e.Exception.Message);
		e.Handled = true;
	}

	private void ShowReportParams(object sender, ItemClickEventArgs e)
	{
		if (_chkReportDetails.IsChecked.Value)
		{
			if (_paramsRowCount > 0)
			{
				ActiveSheet().Rows.Unhide(0, _paramsRowCount - 1);
			}
		}
		else if (_paramsRowCount > 0)
		{
			ActiveSheet().Rows.Hide(0, _paramsRowCount - 1);
		}
	}

	private void RefreshReport(object sender, ItemClickEventArgs e)
	{
		if (IsRepViewerBased)
		{
			_report.Refresh();
		}
	}

	private void ShowDialog(object sender, ItemClickEventArgs e)
	{
		if (IsRepViewerBased && _report.DialogSupported())
		{
			_report.ShowDialog();
			SetCaption();
		}
	}

	private void FilePrintPreview(object sender, ItemClickEventArgs e)
	{
		SetPrintSettings();
		SpreadsheetCommand spreadsheetCommand = ((ISpreadsheetCommandFactoryService)_mainDocument.GetService(typeof(ISpreadsheetCommandFactoryService))).CreateCommand(SpreadsheetCommandId.FilePrintPreview);
		spreadsheetCommand.ForceExecute(spreadsheetCommand.CreateDefaultCommandUIState());
	}

	private void FilePrint(object sender, ItemClickEventArgs e)
	{
		SetPrintSettings();
		SpreadsheetCommand spreadsheetCommand = ((ISpreadsheetCommandFactoryService)_mainDocument.GetService(typeof(ISpreadsheetCommandFactoryService))).CreateCommand(SpreadsheetCommandId.FilePrint);
		spreadsheetCommand.ForceExecute(spreadsheetCommand.CreateDefaultCommandUIState());
	}

	private void ExportSpreadSheetToPdf(object sender, ItemClickEventArgs e)
	{
		SetPrintSettings();
		SaveFileDialog saveFileDialog = new SaveFileDialog
		{
			DefaultExt = "pdf",
			Filter = "PDF files|*.pdf",
			AddExtension = true
		};
		if (string.IsNullOrEmpty(ExportName))
		{
			saveFileDialog.FileName = _report?.ReportName ?? "";
		}
		else
		{
			saveFileDialog.FileName = ExportName;
		}
		saveFileDialog.OverwritePrompt = true;
		saveFileDialog.Title = "Save file";
		saveFileDialog.ValidateNames = true;
		if (!saveFileDialog.ShowDialog().Value)
		{
			return;
		}
		string fileName = saveFileDialog.FileName;
		try
		{
			new PdfExportOptions();
			_mainDocument.Document.ExportToPdf(fileName);
			try
			{
				Settings.ShowFileUsingDefaultApp(fileName);
			}
			catch (Exception)
			{
				MessageBox.Show(ArmSoft.SME.Common.Resources.ExportOk + " " + fileName, ArmSoft.SME.Common.Resources.FileExport);
			}
		}
		catch (Exception ex2)
		{
			ex2.ThrowStorageException(fileName);
		}
	}
}
