using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Enums;
using ArmSoft.SME.Common.Extensions;
using ArmSoft.SME.Common.Models;
using ArmSoft.SME.Common.Parameters;
using ArmSoft.SME.Core.UI.Controls;
using ArmSoft.SME.Core.UI.Extensions;
using ArmSoft.SME.Core.UI.Services;
using DevExpress.Data;
using DevExpress.Drawing.Printing;
using DevExpress.Utils;
using DevExpress.Xpf.Bars;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Docking.Base;
using DevExpress.Xpf.Editors;
using DevExpress.Xpf.Editors.Settings;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.Printing;
using DevExpress.Xpf.Ribbon;
using DevExpress.XtraPrinting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Win32;

namespace ArmSoft.SME.Core.UI.Report;

public partial class AsReportViewer : ASReportRibbonSupportedPanel, IComponentConnector, IStyleConnector
{
	private delegate void ErrorHandlerDelegate(Exception ex, bool interactive = true);

	public BarButtonItemLink AddFunction;

	public BarButtonItemLink EditFunction;

	public BarButtonItemLink DeleteFunction;

	public BarButtonItemLink CreateCopyFunction;

	public BarButtonItemLink PreviewFunction;

	public BarButtonItemLink DocumentHistoryFunction;

	private const int _maxRowCount = 60;

	private readonly AsReport _myReport;

	public List<AsRepRow> RepRows;

	internal List<AsRepRow> headerRows;

	internal List<AsRepRow> footerRows;

	internal AsFindInfo gFindInfo = new AsFindInfo();

	private FlowDocument _myFlowDocument;

	private FlowDocument _myHeader;

	private FlowDocument _myFooter;

	private FlowDocument _detailsDocument;

	private int _filledStartValue;

	private int _filledEndValue;

	private double _filledWidth;

	public Dictionary<string, AsRepTableDef> TablesDefs;

	internal Dictionary<int, string> pageBreaks;

	public ReportActiveRow ActiveRow;

	private object _activeCell;

	public SortedDictionary<int, object> SelectedRows;

	private ObservableCollection<string> _coloredCells;

	private bool _flag;

	private ScrollViewer _scrollViewer;

	private int _bulletColWidth;

	private bool _inRefresh;

	private Brush _selectedRowBrush;

	private Dictionary<Color, int> _totalsDct;

	private List<TotalDetails> _cellsTotals;

	private AsSpreadSheet _mySS;

	private bool _stopRepeat;

	public int UnmovableHeaderLinesCount { get; set; }

	public int UnmovableFooterLinesCount { get; set; }

	public bool EvenOddStyle { get; set; }

	public string ExportName { get; set; }

	public bool ShowInner { get; set; }

	public string ReportName => _myReport.Name;

	public double PageWidth => _myFlowDocument.PageWidth;

	public AsReport Report => _myReport;

	public AsChart Chart { get; set; }

	public bool IsSpreadSheetMode { get; set; }

	private int BulletColWidth
	{
		get
		{
			if (_bulletColWidth == 0)
			{
				if (RepRows.Count <= 9)
				{
					_bulletColWidth = 25;
				}
				else if (RepRows.Count <= 99)
				{
					_bulletColWidth = 30;
				}
				else if (RepRows.Count <= 999)
				{
					_bulletColWidth = 35;
				}
				else
				{
					_bulletColWidth = 45;
				}
			}
			return _bulletColWidth;
		}
	}

	private int MouseWheelCount { get; set; } = 1;

	public string ReportTitle()
	{
		return _myReport.ReportTitle();
	}

	public RibbonDefaultPageCategory InnerRibbonDefaultPageCategory()
	{
		return RibbonDefaultPageCategory;
	}

	public AsReportViewer(AsReport rep)
	{
		//IL_0133: Unknown result type (might be due to invalid IL or missing references)
		//IL_013d: Expected Obj, but got Unknown
		InitializeComponent();
		InitReport();
		FloatOnDoubleClick = false;
		AllowDrag = true;
		_flag = false;
		_myReport = rep;
		SearchPanel.ShowCaption = true;
		SearchPanel.Caption = ArmSoft.SME.Common.Resources.Find;
		SearchPanel.AllowFloat = false;
		SearchPanel.AllowSizing = false;
		_cmdColors.ContentTemplate = (DataTemplate)FindResource("ColorsItem");
		ScrollViewHeader.PreviewMouseDoubleClick += new SafeEventHandler<MouseButtonEventArgs>(Header_PreviewMouseDoubleClick).Handler;
		ScrollViewFooter.PreviewMouseDoubleClick += new SafeEventHandler<MouseButtonEventArgs>(Footer_PreviewMouseDoubleClick).Handler;
		try
		{
			IsSpreadSheetMode = Settings.ParametersService.SpreadsheetMode();
		}
		catch (Exception)
		{
			IsSpreadSheetMode = false;
		}
		GotFocus += new SafeEventHandler<RoutedEventArgs>(AsReportViewer_GotFocus).Handler;
		IsStylusCaptureWithinChanged += new SafeEventHandler<DependencyPropertyChangedEventArgs>(AsReportViewer_IsStylusCaptureWithinChanged).Handler;
		Loaded += new SafeEventHandler<RoutedEventArgs>(AsReport_Loaded).Handler;
		PreviewKeyDown += new SafeEventHandler<KeyEventArgs>(FlowDocument_PreviewKeyDown).Handler;
		SizeChanged += new SafeEventHandler<SizeChangedEventArgs>(AsReportViewer_SizeChanged).Handler;
		LostKeyboardFocus += new SafeEventHandler<KeyboardFocusChangedEventArgs>(AsReportViewer_LostKeyboardFocus).Handler;
		SearchPanel.Loaded += new SafeEventHandler<EventArgs>(SearchPanel_Loaded).Handler;
		FindNextButton.Click += new SafeEventHandler<RoutedEventArgs>(ButtonFind_Click).Handler;
		FindTextComboBox.PreviewKeyDown += new SafeEventHandler<KeyEventArgs>(FindTextComboBox_PreviewKeyDown).Handler;
		SearchUpCheckEdit.EditValueChanged += new SafeEventHandler<EditValueChangedEventArgs>(SearchUp_EditValueChanged).Handler;
		ScrollBar.Scroll += new SafeEventHandler<ScrollEventArgs>(Scroll).Handler;
		_myHeader.ContextMenuOpening += new SafeEventHandler<ContextMenuEventArgs>(Header_ContextMenuOpening).Handler;
		_myHeader.PreviewKeyDown += new SafeEventHandler<KeyEventArgs>(Header_PreviewKeyDown).Handler;
		_myFooter.ContextMenuOpening += new SafeEventHandler<ContextMenuEventArgs>(Footer_ContextMenuOpening).Handler;
		HelpBarButtonItem.ItemClick += new SafeEventHandler<ItemClickEventArgs>(BarButtonHelp_ItemClick).Handler;
		DetailLayoutManager.DockItemClosing += new SafeEventHandler<ItemCancelEventArgs>(MainLayoutManager_DockItemClosing).Handler;
		DetailsPanel.PreviewKeyDown += new SafeEventHandler<KeyEventArgs>(DetailsPanel_PreviewKeyDown).Handler;
	}

	private void SearchPanel_Loaded(object sender, EventArgs e)
	{
		FindTextComboBox.ItemsSource = gFindInfo.History;
		MatchWholeWordCheckEdit.IsChecked = gFindInfo.WholeWord;
		IgnoreCaseCheckEdit.IsChecked = gFindInfo.IgnoreCase;
		Keyboard.Focus(FindTextComboBox);
	}

	private void SearchPanel_PreviewKeyDown(object sender, KeyEventArgs e)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Invalid comparison between Unknown and I4
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Invalid comparison between Unknown and I4
		if ((int)Keyboard.Modifiers == 0 && (int)e.Key == 6)
		{
			SearchTextInReport();
			e.Handled = true;
		}
		else if ((int)Keyboard.Modifiers == 0 && (int)e.Key == 13)
		{
			SearchPanel.Visibility = Visibility.Collapsed;
			MainReportPanel.IsActive = true;
			e.Handled = true;
		}
		else
		{
			e.Handled = false;
		}
	}

	private void ButtonFind_Click(object sender, RoutedEventArgs e)
	{
		SearchTextInReport();
		e.Handled = true;
	}

	private void SearchTextInReport()
	{
		if (!string.IsNullOrEmpty(FindTextComboBox.Text.Trim()) && !gFindInfo.History.Contains(FindTextComboBox.Text.Trim()))
		{
			gFindInfo.History.Insert(0, FindTextComboBox.Text);
			if (gFindInfo.History.Count > 50)
			{
				gFindInfo.History.RemoveAt(gFindInfo.History.Count - 1);
			}
		}
		FindTextComboBox.ItemsSource = gFindInfo.History;
		if (!SearchUpCheckEdit.IsChecked.Value)
		{
			gFindInfo.SearchDirection = FindDirection.Forward;
		}
		else
		{
			gFindInfo.SearchDirection = FindDirection.Back;
		}
		gFindInfo.IgnoreCase = IgnoreCaseCheckEdit.IsChecked.Value;
		gFindInfo.WholeWord = MatchWholeWordCheckEdit.IsChecked.Value;
		gFindInfo.Pattern = FindTextComboBox.Text.Trim();
		if (Settings.HostingEnvironment.CurrentPanel is AsReportViewer asReportViewer)
		{
			asReportViewer.FindText(gFindInfo);
		}
		MainReportPanel.IsActive = true;
	}

	public void ShowSearchPanel()
	{
		SearchPanel.Visibility = Visibility.Visible;
		if (SearchPanel.Parent == null)
		{
			MainLayoutGroup.Insert(0, SearchPanel);
		}
		SearchPanel.IsActive = true;
		FocusManager.SetFocusedElement((DependencyObject)(object)SearchPanel, FindTextComboBox);
		FindTextComboBox.CaretIndex = 0;
	}

	private void FindTextComboBox_PreviewKeyDown(object sender, KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Invalid comparison between Unknown and I4
		if ((int)e.Key == 6)
		{
			Keyboard.Focus(FindNextButton);
			SearchTextInReport();
			e.Handled = true;
			MainReportPanel.IsActive = true;
		}
	}

	private void SearchUp_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		if (SearchUpCheckEdit.IsChecked.Value)
		{
			FindNextButton.Content = ArmSoft.SME.Common.Resources.FindPrevious;
		}
		else
		{
			FindNextButton.Content = ArmSoft.SME.Common.Resources.FindNext;
		}
	}

	protected override void BarManager_ItemClick(object sender, ItemClickEventArgs e)
	{
		try
		{
			if (e.Item is BarButtonItem barButtonItem && !(barButtonItem is BarCheckItem))
			{
				string name = barButtonItem.Name;
				if (name == null)
				{
					return;
				}
				switch (name.Length)
				{
				case 13:
					switch (name[6])
					{
					case 'i':
						if (name == "bDataViewEdit")
						{
							if (AllowEdit())
							{
								Edit();
							}
							else if (AllowView())
							{
								View();
							}
						}
						break;
					case 'P':
						if (name == "bPrintPreview")
						{
							if (ChartPanel.IsActive)
							{
								PreviewChart();
							}
							else
							{
								PreviewReport();
							}
						}
						break;
					case 'O':
						if (name == "bPrintOptions")
						{
							WndPageSetup wndPageSetup = new WndPageSetup();
							wndPageSetup.ReportKind = ReportName;
							wndPageSetup.ShowDialog();
						}
						break;
					}
					break;
				case 19:
					switch (name[1])
					{
					case 'D':
						if (name == "bDataViewCreateCopy")
						{
							CreateCopy();
						}
						break;
					case 'S':
						if (name == "bShowDataViewDialog" && DialogSupported())
						{
							ShowDialog();
						}
						break;
					case 'E':
						if (name == "bExportDocumentWord")
						{
							ExportWord();
						}
						break;
					}
					break;
				case 16:
					switch (name[1])
					{
					case 'D':
						if (name == "bDocumentHistory")
						{
							DocumentHistory();
						}
						break;
					case 'R':
						if (name == "bRefreshDataView")
						{
							Refresh();
						}
						break;
					}
					break;
				case 10:
					switch (name[7])
					{
					case 'C':
						if (name == "bExportCsv")
						{
							ExportToExcelOrPdf("csv", "export.csv", "Sheet1", showSaveDialog: true, showFile: false);
							MessageBox.Show(ArmSoft.SME.Common.Resources.OperationSucceeded);
						}
						break;
					case 'P':
						if (name == "bExportPDF")
						{
							ExportToExcelOrPdf("pdf");
						}
						break;
					}
					break;
				case 7:
					switch (name[3])
					{
					case 'F':
						if (name == "cmdFind")
						{
							ShowSearchPanel();
						}
						break;
					case 'C':
						if (name == "cmdCopy")
						{
							CopySelection();
						}
						break;
					}
					break;
				case 12:
					if (name == "bDataViewAdd")
					{
						Add();
					}
					break;
				case 15:
					if (name == "bDataViewDelete")
					{
						Delete();
					}
					break;
				case 17:
					if (name == "bDataViewDocument")
					{
						Preview();
					}
					break;
				case 6:
					if (name == "bPrint")
					{
						if (ChartPanel.IsActive)
						{
							PreviewChart();
						}
						else
						{
							PrintReport();
						}
					}
					break;
				case 20:
					if (name == "bExportDocumentExcel")
					{
						ExportExcel();
					}
					break;
				case 18:
					if (name == "bExportDocumentPDF")
					{
						ExportPdf();
					}
					break;
				case 11:
					if (name == "bExportXlsX")
					{
						ExportToExcelOrPdf("xlsx");
					}
					break;
				case 9:
					if (name == "cmdColors")
					{
						SetSelectedColor();
					}
					break;
				case 8:
				case 14:
					break;
				}
			}
			else if (e.Item is BarCheckItem { Name: "chkReportDetails", IsChecked: var isChecked })
			{
				if (isChecked.Value)
				{
					FillDetail();
					DetailsPanel.Visibility = Visibility.Visible;
				}
				else
				{
					DetailsPanel.Visibility = Visibility.Collapsed;
				}
			}
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	private void SetBarStates()
	{
		RefreshDataViewBarButtonItem.IsEnabled = Report.AllowRefresh;
		ShowDataViewDialogBarButtonItem.IsEnabled = DialogSupported();
		DataViewAddBarButtonItem.IsEnabled = AllowAdd();
		DataViewEditBarButtonItem.IsEnabled = AllowEdit() || AllowView();
		DataViewDeleteBarButtonItem.IsEnabled = AllowDelete();
		DataViewCreateCopyBarButtonItem.IsEnabled = AllowCreateCopy();
		DataViewDocumentBarButtonItem.IsEnabled = AllowPreview();
		DocumentHistoryBarButtonItem.IsEnabled = AllowDocumentHistory();
		if (Settings.SystemType == SystemTypes.MarketPlace)
		{
			DocumentHistoryBarButtonItem.IsVisible = false;
		}
		ExportDocumentWordBarButtonItem.IsEnabled = AllowPreview();
		ExportDocumentExcelBarButtonItem.IsEnabled = AllowPreview();
		ExportDocumentPDFBarButtonItem.IsEnabled = AllowPreview();
		_bExportDocumentXML.IsEnabled = false;
		_bPrintPreview.IsEnabled = true;
		_cmdGroupPanel.IsEnabled = false;
		_cmdAutoFilter.IsEnabled = false;
		_btnFilterSelected.IsEnabled = false;
		_cmdClearSort.IsEnabled = false;
		_bPrint.IsEnabled = true;
		_bExportToExcel.IsEnabled = true;
		_bExportPDF.IsEnabled = true;
		_bSelect.IsEnabled = false;
		_cmdSelectMarked.IsEnabled = false;
		_cmdSelectAll.IsEnabled = true;
		_cmdFind.IsEnabled = true;
		_cmdCopy.IsEnabled = true;
		_cmdColors.IsEnabled = true;
		_chkReportDetails.IsEnabled = true;
		AddToolbarItems();
	}

	public void AddToolbarItems()
	{
		RibbonControl.ToolbarItemLinks.Clear();
		AddFunction = DataViewAddBarButtonItem.ConvertForToolbar("t_add_16.svg");
		EditFunction = DataViewEditBarButtonItem.ConvertForToolbar("t_edit_16.svg");
		DeleteFunction = DataViewDeleteBarButtonItem.ConvertForToolbar("t_delete_16.svg");
		CreateCopyFunction = DataViewCreateCopyBarButtonItem.ConvertForToolbar("t_doc_copy_16.svg");
		PreviewFunction = DataViewDocumentBarButtonItem.ConvertForToolbar("t_doc_preview_16.svg");
		RibbonControl.ToolbarItemLinks.Add(AddFunction);
		RibbonControl.ToolbarItemLinks.Add(EditFunction);
		RibbonControl.ToolbarItemLinks.Add(DeleteFunction);
		RibbonControl.ToolbarItemLinks.Add(CreateCopyFunction);
		RibbonControl.ToolbarItemLinks.Add(ShowDataViewDialogBarButtonItem.ConvertForToolbar("t_table_refresh_16.svg"));
		RibbonControl.ToolbarItemLinks.Add(RefreshDataViewBarButtonItem.ConvertForToolbar("t_refresh_16.svg"));
		RibbonControl.ToolbarItemLinks.Add(_bExportToExcel.ConvertForToolbar("t_table_export_excel_16.svg"));
		RibbonControl.ToolbarItemLinks.Add(_bPrintPreview.ConvertForToolbar("t_table_preview_16.svg"));
		RibbonControl.ToolbarItemLinks.Add(_bPrint.ConvertForToolbar("t_print_16.svg"));
		RibbonControl.ToolbarItemLinks.Add(ExportDocumentExcelBarButtonItem.ConvertForToolbar("t_doc_export_excel_16.svg"));
		if (Settings.SystemType == SystemTypes.MarketPlace)
		{
			DocumentHistoryFunction = null;
			return;
		}
		DocumentHistoryFunction = DocumentHistoryBarButtonItem.ConvertForToolbar("t_history_16.svg");
		RibbonControl.ToolbarItemLinks.Add(DocumentHistoryFunction);
		if (Settings.SystemType == SystemTypes.Trade)
		{
			ScrollView.SelectionBrush = (SolidColorBrush)new BrushConverter().ConvertFrom("#DEF0E0");
		}
		else if (Settings.SystemType == SystemTypes.Accountant)
		{
			ScrollView.SelectionBrush = (SolidColorBrush)new BrushConverter().ConvertFrom("#DEF0E0");
		}
		else if (Settings.SystemType == SystemTypes.SalesManagement)
		{
			ScrollView.SelectionBrush = (SolidColorBrush)new BrushConverter().ConvertFrom("#DEF0E0");
		}
	}

	private void PreviewChart()
	{
		SimpleLink simpleLink = new SimpleLink
		{
			DetailCount = 1,
			DetailTemplate = (DataTemplate)Settings.GenericResources["ChartPreviewData"]
		};
		simpleLink.CreateDetail += new SafeEventHandler<DevExpress.Xpf.Printing.CreateAreaEventArgs>(CreateDetail).Handler;
		PrintInfo printInfo = new PrintInfo(ReportName);
		simpleLink.PrintingSystem.PageSettings.PaperKind = DXPaperKind.A4;
		simpleLink.PrintingSystem.PageSettings.Margins.Left = printInfo.MarginLeft * 100f;
		simpleLink.PrintingSystem.PageSettings.Margins.Right = printInfo.MarginRight * 100f;
		simpleLink.PrintingSystem.PageSettings.Margins.Top = printInfo.MarginTop * 100f;
		simpleLink.PrintingSystem.PageSettings.Margins.Bottom = printInfo.MarginBottom * 100f;
		if (printInfo.PaperOrientation == "Landscape")
		{
			simpleLink.Landscape = true;
		}
		else
		{
			simpleLink.Landscape = false;
		}
		simpleLink.CreateDocument(buildForInstantPreview: true);
		simpleLink.ShowPrintPreviewDialog(Settings.HostingEnvironment, Caption.ToString());
	}

	private void CreateDetail(object sender, DevExpress.Xpf.Printing.CreateAreaEventArgs e)
	{
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		VisualBrush brush = new VisualBrush(Chart);
		DrawingVisual drawingVisual = new DrawingVisual();
		DrawingContext drawingContext = drawingVisual.RenderOpen();
		SimpleLink simpleLink = (SimpleLink)sender;
		drawingContext.DrawRectangle(brush, null, new Rect(0.0, 0.0, (double)(simpleLink.PrintingSystem.PageSettings.UsablePageSize.Width - 50f), Chart.ActualHeight));
		drawingContext.Close();
		RenderTargetBitmap renderTargetBitmap = new RenderTargetBitmap((int)simpleLink.PrintingSystem.PageSettings.UsablePageSizeInPixels.Width, (int)simpleLink.PrintingSystem.PageSettings.UsablePageSizeInPixels.Height, 96.0, 96.0, PixelFormats.Pbgra32);
		renderTargetBitmap.Render(drawingVisual);
		e.Data = renderTargetBitmap;
	}

	private void InitReport()
	{
		_myFlowDocument = new FlowDocument();
		_myHeader = new FlowDocument();
		_myFooter = new FlowDocument();
		_selectedRowBrush = Settings.GetSelectedRowBrush();
		_myHeader.Background = Settings.GetBackgroundBrush();
		_myFooter.Background = Settings.GetBackgroundBrush();
		ScrollView.Document = _myFlowDocument;
		_detailsDocument = new FlowDocument();
		Binding binding = new Binding("FontFamily")
		{
			Source = Settings.HostingEnvironment
		};
		_detailsDocument.SetBinding(System.Windows.Controls.Control.FontFamilyProperty, binding);
		DetailsPanel.Caption = ArmSoft.SME.Common.Resources.ReportDetails;
		_detailsDocument.FontSize = 3.0;
		ScrollViewFooter.Document = _myFooter;
		ScrollViewHeader.Document = _myHeader;
		RepRows = new List<AsRepRow>();
		pageBreaks = new Dictionary<int, string>();
		_filledStartValue = -1;
		_filledEndValue = -1;
		_filledWidth = 0.0;
		SelectedRows = new SortedDictionary<int, object>();
		ActiveRow = new ReportActiveRow
		{
			ActiveRow = -1,
			SelectedCount = 0
		};
		ActiveRow.PropertyChanged += new SafeEventHandler<PropertyChangedEventArgs>(ShowStatus).Handler;
		TablesDefs = new Dictionary<string, AsRepTableDef>();
		InitColorsTotals();
	}

	public void ClearAll()
	{
		RepRows.Clear();
		pageBreaks.Clear();
		_filledStartValue = -1;
		_filledEndValue = -1;
		_filledWidth = 0.0;
		UnmovableHeaderLinesCount = 0;
		UnmovableFooterLinesCount = 0;
		ActiveRow.ActiveRow = -1;
		TablesDefs = new Dictionary<string, AsRepTableDef>();
		_flag = false;
	}

	public void ReDrawActivePage()
	{
		//IL_0004: Unknown result type (might be due to invalid IL or missing references)
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		int rowNum = 0;
		ScrollViewer sc = null;
		Rect startRect = Rect.Empty;
		FindFirstVisibleRow(ref startRect, ref rowNum, ref sc);
		FillVisiblePart(rowNum);
		NavigateToRow(rowNum);
	}

	public AsRepRowCell AddString(string text, AsRepRowStyle rowType = AsRepRowStyle.StringRow, object rowInfo = null, decimal width = 0m)
	{
		AsRepRow asRepRow = new AsRepRow(AsRepRowType.Cells, rowType)
		{
			BlockName = "",
			RowStyle = rowType
		};
		AsRepRowCell asRepRowCell = new AsRepRowCell
		{
			Value = text,
			CellType = rowType
		};
		if (width == 0m)
		{
			asRepRowCell.WidthD = 800.0;
		}
		else
		{
			asRepRowCell.WidthD = (double)width;
		}
		asRepRow.RowText = text;
		asRepRow.RowType = AsRepRowType.Text;
		asRepRow.HiddenData = rowInfo;
		asRepRow.IsTableCaption = false;
		int num = 1;
		List<AsRepRowCell> list = new List<AsRepRowCell>(num);
		CollectionsMarshal.SetCount(list, num);
		CollectionsMarshal.AsSpan(list)[0] = asRepRowCell;
		asRepRow.RowCells = list;
		RepRows.Add(asRepRow);
		return asRepRowCell;
	}

	public void AddTableDefinition(string tableName, List<AsRepColumn> columnsDefinitions)
	{
		AsRepTableDef value = new AsRepTableDef
		{
			TableName = tableName,
			ColumnDefinitions = columnsDefinitions
		};
		TablesDefs.Add(tableName.ToUpper(), value);
	}

	public void AddTableColumnsHeader(string tableName, List<string> values = null, object rowInfo = null)
	{
		AsRepRow asRepRow = new AsRepRow(AsRepRowType.Cells, AsRepRowStyle.TableColumnsHeader);
		AsRepTableDef asRepTableDef = TablesDefs[tableName.ToUpper()];
		asRepRow.RowCells = new List<AsRepRowCell>();
		if (asRepTableDef == null)
		{
			return;
		}
		asRepRow.HiddenData = rowInfo;
		List<AsRepColumn> columnDefinitions = asRepTableDef.ColumnDefinitions;
		if (columnDefinitions != null)
		{
			asRepRow.BlockName = tableName.ToUpper();
			int num = values?.Count ?? columnDefinitions.Count;
			for (int i = 0; i <= num - 1; i++)
			{
				string value = ((values == null || values.Count <= i) ? columnDefinitions[i].Caption : values[i]);
				AsRepRowCell asRepRowCell = new AsRepRowCell
				{
					Value = value,
					ValueStyle = columnDefinitions[i].ValuesStyle,
					Borders = new ObservableCollectionCore<BorderType>()
				};
				asRepRowCell.Borders.Add(BorderType.AllBorders);
				asRepRowCell.BackGround = (Settings.IsDarkTheme ? Colors.DimGray : Colors.LightGray);
				asRepRow.RowCells.Add(asRepRowCell);
			}
			asRepRow.IsTableCaption = true;
			RepRows.Add(asRepRow);
		}
	}

	public void AddTableRow(string tableName, List<object> values, object rowInfo = null)
	{
		AsRepRow asRepRow = new AsRepRow(AsRepRowType.Cells);
		AsRepTableDef asRepTableDef = TablesDefs[tableName.ToUpper()];
		asRepRow.RowCells = new List<AsRepRowCell>();
		if (asRepTableDef == null)
		{
			return;
		}
		asRepRow.HiddenData = rowInfo;
		if (asRepTableDef.ColumnDefinitions != null)
		{
			asRepRow.BlockName = tableName.ToUpper();
			for (int i = 0; i <= values.Count - 1; i++)
			{
				object value = ((values == null || values.Count <= i) ? "" : values[i]);
				AsRepRowCell item = new AsRepRowCell
				{
					Value = value,
					ValueStyle = asRepTableDef.ColumnDefinitions[i].ValuesStyle,
					XlsxFormatString = asRepTableDef.ColumnDefinitions[i].XlsxFormatString
				};
				asRepRow.RowCells.Add(item);
			}
			asRepRow.IsTableCaption = false;
			RepRows.Add(asRepRow);
		}
	}

	public void AddCells(List<AsRepRowCell> values, AsRepRowStyle type = AsRepRowStyle.TableRow, object rowInfo = null)
	{
		if (values != null)
		{
			AsRepRow item = new AsRepRow(AsRepRowType.Cells, type)
			{
				BlockName = "",
				RowCells = values,
				IsTableCaption = false,
				HiddenData = rowInfo
			};
			RepRows.Add(item);
		}
	}

	public void AddTableSpanRow(string tableName, List<AsRepRowCell> values, AsRepRowStyle type = AsRepRowStyle.SubCaptionRow, object rowInfo = null)
	{
		AsRepRow asRepRow = new AsRepRow(AsRepRowType.Cells, AsRepRowStyle.TableColumnsHeader);
		if (values == null || values.Count == 0)
		{
			return;
		}
		asRepRow.BlockName = tableName.ToUpper();
		asRepRow.RowStyle = type;
		asRepRow.RowCells = values;
		int num = 0;
		for (int i = 0; i <= values.Count - 1; i++)
		{
			num += values[i].ColumnSpan;
		}
		if (TablesDefs.ContainsKey(tableName.ToUpper()))
		{
			AsRepTableDef asRepTableDef = TablesDefs[tableName.ToUpper()];
			if (num < asRepTableDef.ColumnDefinitions.Count)
			{
				for (int j = num + 1; j <= asRepTableDef.ColumnDefinitions.Count; j++)
				{
					AsRepRowCell item = new AsRepRowCell
					{
						Value = "",
						ColumnSpan = 1
					};
					values.Add(item);
				}
			}
		}
		asRepRow.HiddenData = rowInfo;
		asRepRow.IsTableCaption = false;
		RepRows.Add(asRepRow);
	}

	public int RowsCount()
	{
		if (RepRows == null)
		{
			return 0;
		}
		return RepRows.Count;
	}

	public void AddPageBreak(string blockTitle)
	{
		pageBreaks.Add(RepRows.Count - 1, blockTitle);
	}

	public object ActiveRowInfo()
	{
		if (ActiveRow.ActiveRow > -1 && ActiveRow.ActiveRow < RepRows.Count)
		{
			return RepRows[ActiveRow.ActiveRow].HiddenData;
		}
		return null;
	}

	public void Show(Exception ex = null, bool isSpreadSheet = false)
	{
		if (RepRows.Count > 0 && !IsSpreadSheetMode)
		{
			FillVisiblePart(0);
			ActiveRow.ActiveRow = 0;
		}
		if (Chart == null)
		{
			ChartPanel.Visibility = Visibility.Collapsed;
		}
		else
		{
			ChartPanel.Content = Chart;
			ChartPanel.Visibility = Visibility.Visible;
		}
		if (ex == null)
		{
			if (!isSpreadSheet && !IsSpreadSheetMode)
			{
				Settings.HostingEnvironment.AddPanelToDocumentGroup(this, ShowInner);
				SetBarStates();
				Settings.HostingEnvironment.LayoutManager.Activate(this);
			}
			else
			{
				bool flag = false;
				if (_mySS == null)
				{
					flag = true;
					_mySS = new AsSpreadSheet();
				}
				_mySS.FillReport(this, 0, forceRefresh: true);
				IsSpreadSheetMode = true;
				SetBarStates();
				if (flag)
				{
					_mySS.Show();
				}
			}
		}
		if (ex != null)
		{
			((DispatcherObject)Settings.HostingEnvironment).Dispatcher.BeginInvoke((Delegate)new ErrorHandlerDelegate(Settings.ErrorHandler), new object[1] { ex });
		}
	}

	public ReportGrid PreviewReport()
	{
		AsReportPrintPreview asReportPrintPreview = new AsReportPrintPreview(this);
		for (int i = 0; i <= RepRows.Count - 1; i++)
		{
			GetRowSizeAndStyle(RepRows[i]);
		}
		if (string.IsNullOrWhiteSpace(Name) && string.IsNullOrWhiteSpace(ExportName))
		{
			ExportName = ReportName;
		}
		return asReportPrintPreview.CreateReport(showReport: true);
	}

	public void PrintReport()
	{
		try
		{
			AsReportPrintPreview asReportPrintPreview = new AsReportPrintPreview(this);
			for (int i = 0; i <= RepRows.Count - 1; i++)
			{
				GetRowSizeAndStyle(RepRows[i]);
			}
			if (string.IsNullOrWhiteSpace(Name) && string.IsNullOrWhiteSpace(ExportName))
			{
				ExportName = ReportName;
			}
			PrintHelper.Print(asReportPrintPreview.CreateReport(showReport: false));
		}
		catch (Exception ex)
		{
			MessageBox.Show(ArmSoft.SME.Common.Resources.ErrorWord + " - " + ex.Message, ArmSoft.SME.Common.Resources.Print, MessageBoxButton.OK, MessageBoxImage.Hand);
		}
	}

	public void ExportToExcelOrPdf(string exportFormat, string fileName = "c:\\tmp\\export", string sheetName = "Sheet1", bool showSaveDialog = true, bool showFile = true)
	{
		if (showSaveDialog && !ShowSaveDialog(exportFormat, ref fileName))
		{
			return;
		}
		AsReportPrintPreview asReportPrintPreview = new AsReportPrintPreview(this);
		for (int i = 0; i <= RepRows.Count - 1; i++)
		{
			GetRowSizeAndStyle(RepRows[i]);
		}
		try
		{
			ReportGrid reportGrid = asReportPrintPreview.CreateReport(showReport: false, exportFormat.Equals("PDF", StringComparison.CurrentCultureIgnoreCase));
			if (exportFormat.Equals("PDF", StringComparison.CurrentCultureIgnoreCase))
			{
				PdfExportOptions options = new PdfExportOptions
				{
					ConvertImagesToJpeg = true
				};
				reportGrid.ExportToPdf(fileName, options);
			}
			else if (exportFormat.Equals("CSV", StringComparison.CurrentCultureIgnoreCase))
			{
				CsvExportOptions options2 = new CsvExportOptions(";", Encoding.UTF8);
				reportGrid.ExportToCsv(fileName, options2);
			}
			else
			{
				XlsxExportOptions options3 = new XlsxExportOptions
				{
					ShowGridLines = false,
					ExportMode = XlsxExportMode.SingleFile,
					SheetName = sheetName,
					TextExportMode = TextExportMode.Value
				};
				reportGrid.ExportToXlsx(fileName, options3);
			}
			if (showFile)
			{
				Process.Start(new ProcessStartInfo
				{
					FileName = fileName,
					UseShellExecute = true
				});
			}
		}
		catch (Exception ex)
		{
			ex.ThrowStorageException(fileName);
		}
	}

	private bool ShowSaveDialog(string exportFormat, ref string fileName)
	{
		SaveFileDialog saveFileDialog = new SaveFileDialog
		{
			DefaultExt = exportFormat
		};
		if (exportFormat.Equals("PDF", StringComparison.CurrentCultureIgnoreCase))
		{
			saveFileDialog.Filter = "PDF files|*.pdf";
		}
		else if (exportFormat.Equals("CSV", StringComparison.CurrentCultureIgnoreCase))
		{
			saveFileDialog.Filter = "CSV UTF-8(Comma delimited)|*.csv";
		}
		else
		{
			saveFileDialog.Filter = "Excel Workbooks|*.xlsx";
		}
		saveFileDialog.AddExtension = true;
		if (string.IsNullOrWhiteSpace(ExportName))
		{
			saveFileDialog.FileName = ReportName;
		}
		else
		{
			saveFileDialog.FileName = ExportName;
		}
		saveFileDialog.OverwritePrompt = true;
		saveFileDialog.Title = "Save file";
		saveFileDialog.ValidateNames = true;
		if (saveFileDialog.ShowDialog().Value)
		{
			fileName = saveFileDialog.FileName;
			return true;
		}
		return false;
	}

	public void ExportToPdf(string filename = "c:\\tmp\\export.pdf", bool showDocumentMap = true)
	{
		AsReportPrintPreview asReportPrintPreview = new AsReportPrintPreview(this);
		int i = 0;
		for (int num = RepRows.Count - 1; i <= num; i++)
		{
			GetRowSizeAndStyle(RepRows[i]);
		}
		asReportPrintPreview.CreateReport(showReport: false, isPreview: true, showDocumentMap).ExportToPdf(filename);
	}

	private void CalculatePageWidth()
	{
		double num = 0.0;
		foreach (string key in TablesDefs.Keys)
		{
			double num2 = 0.0;
			for (int i = 0; i <= TablesDefs[key].ColumnDefinitions.Count - 1; i++)
			{
				num2 = ((!(TablesDefs[key].ColumnDefinitions[i].WidthD > 0.0)) ? (num2 + DefaultColWidth(TablesDefs[key].ColumnDefinitions[i].ValuesStyle)) : (num2 + TablesDefs[key].ColumnDefinitions[i].WidthD));
			}
			if (num < num2)
			{
				num = num2;
			}
		}
		for (int j = 0; j <= RepRows.Count - 1; j++)
		{
			if (string.IsNullOrWhiteSpace(RepRows[j].BlockName) || !TablesDefs.ContainsKey(RepRows[j].BlockName))
			{
				double rowWidth = GetRowWidth(RepRows[j]);
				if (num < rowWidth)
				{
					num = rowWidth;
				}
			}
		}
		num += (double)BulletColWidth;
		_myHeader.FontSize = 0.2;
		_myFlowDocument.FontSize = 0.2;
		_myFooter.FontSize = 0.2;
		_myHeader.Foreground = Settings.GetForegroundBrush();
		num += _myFlowDocument.FontSize;
		_myFlowDocument.PageWidth = num;
		_myFlowDocument.Foreground = Settings.GetForegroundBrush();
		_myFooter.PageWidth = num;
		_myHeader.PageWidth = num;
		_myFooter.Foreground = Settings.GetForegroundBrush();
	}

	private static double GetRowWidth(AsRepRow repRow)
	{
		List<AsRepRowCell> rowCells = repRow.RowCells;
		double num = 0.0;
		for (int i = 0; i <= rowCells.Count - 1; i++)
		{
			num = ((rowCells[i].WidthD != 0.0) ? (num + rowCells[i].WidthD) : (num + DefaultColWidth(rowCells[i].ValueStyle)));
		}
		return num;
	}

	private void AsReportViewer_GotFocus(object sender, RoutedEventArgs e)
	{
		if (!IsFloating)
		{
			Settings.HostingEnvironment.StatusBarSelectedRows(string.Format(ArmSoft.SME.Common.Resources.StatusSelectedRows, SelectedRows.Count));
			Settings.HostingEnvironment.StatusBarCurrentRow(string.Format(ArmSoft.SME.Common.Resources.StatusRows, ActiveRow.ActiveRow + 1, RepRows.Count));
		}
		else
		{
			_statusBarSelectedRows.Content = string.Format(ArmSoft.SME.Common.Resources.StatusSelectedRows, SelectedRows.Count);
			_statusBarCurrentRow.Content = string.Format(ArmSoft.SME.Common.Resources.StatusRows, ActiveRow.ActiveRow + 1, RepRows.Count);
		}
		if (IsFloating)
		{
			RibbonStatusBar.Visibility = Visibility.Visible;
		}
		FocusManager.SetFocusedElement((DependencyObject)(object)this, _myFlowDocument);
	}

	private void AsReportViewer_IsStylusCaptureWithinChanged(object sender, DependencyPropertyChangedEventArgs e)
	{
	}

	private void AsReport_Loaded(object sender, RoutedEventArgs e)
	{
		ScrollBar.Minimum = 0.0;
		ScrollBar.Maximum = RepRows.Count - 1;
		ScrollBar.Value = 0.0;
		_myFlowDocument.FontFamily = new FontFamily(Settings.HostingEnvironment.FontFamily.Source);
		_scrollViewer = FindScrollViewer((DependencyObject)(object)ScrollView);
		_myHeader.FontFamily = _myFlowDocument.FontFamily;
		_myFooter.FontFamily = _myFlowDocument.FontFamily;
		FocusManager.SetFocusedElement((DependencyObject)(object)this, _myFlowDocument);
		_statusBarCompanyName.Content = Settings.HostingEnvironment.StatusBarCompanyName;
	}

	private void FindFillableArea(ref int startPos, ref int endPos)
	{
		if (startPos == -1 && endPos == -1)
		{
			startPos = (int)ScrollBar.Value;
			endPos = startPos + 60;
		}
		else if (startPos > -1 && endPos == -1)
		{
			endPos = startPos + 60;
		}
		else if (startPos == -1 && endPos > -1)
		{
			if (endPos >= RepRows.Count)
			{
				endPos = RepRows.Count - 1;
			}
			startPos = endPos - 60;
		}
		if (startPos < 0)
		{
			startPos = 0;
		}
		if (endPos < 0)
		{
			endPos = 0;
		}
		if (startPos > RepRows.Count - 1)
		{
			startPos = RepRows.Count - 1;
		}
		if (endPos > RepRows.Count - 1)
		{
			endPos = RepRows.Count - 1;
		}
	}

	private void FillVisiblePart(int firstRowNum = -1, bool forceRefresh = false)
	{
		int rowNum = -1;
		int endPos = -1;
		if (RepRows.Count == 0)
		{
			_myFlowDocument.Blocks.Clear();
			return;
		}
		if (firstRowNum > -1)
		{
			rowNum = firstRowNum;
		}
		else
		{
			ScrollViewer sc = null;
			Rect startRect = default;
			FindFirstVisibleRow(ref startRect, ref rowNum, ref sc);
		}
		if (!_flag)
		{
			CalculatePageWidth();
		}
		if (!_flag && UnmovableHeaderLinesCount > 0)
		{
			_myHeader.Blocks.Clear();
			for (int i = 0; i <= UnmovableHeaderLinesCount - 1; i++)
			{
				RepRows[i].RowType = AsRepRowType.Text;
				AddRepRow(RepRows[i], i, appendMode: true, _myHeader);
			}
			headerRows = new List<AsRepRow>();
			for (int i = 0; i <= UnmovableHeaderLinesCount - 1; i++)
			{
				headerRows.Add(RepRows[0]);
				RepRows.RemoveAt(0);
			}
			ScrollViewHeader.Document.PageHeight = ScrollView.ActualHeight;
			ScrollViewHeader.BorderThickness = new Thickness(0.0, 0.0, 0.0, 2.0);
		}
		else if (UnmovableHeaderLinesCount == 0)
		{
			ScrollViewHeader.Height = 0.0;
		}
		if (!_flag && UnmovableFooterLinesCount > 0)
		{
			_myFooter.Blocks.Clear();
			footerRows = new List<AsRepRow>();
			for (int i = RepRows.Count - UnmovableFooterLinesCount; i <= RepRows.Count - 1; i++)
			{
				AddRepRow(RepRows[i], i, appendMode: true, _myFooter);
				footerRows.Add(RepRows[i]);
			}
			ScrollViewFooter.BorderThickness = new Thickness(0.0, 2.0, 0.0, 0.0);
			ScrollViewFooter.UpdateLayout();
			for (int i = 0; i <= UnmovableFooterLinesCount - 1; i++)
			{
				RepRows.RemoveAt(RepRows.Count - 1);
			}
			ScrollViewFooter.Document.PageHeight = ScrollView.ActualHeight;
		}
		else if (UnmovableFooterLinesCount == 0)
		{
			ScrollViewFooter.Height = 0.0;
		}
		if (RepRows.Count == 0)
		{
			_myFlowDocument.Blocks.Clear();
			return;
		}
		if (!_flag)
		{
			if (pageBreaks != null && pageBreaks.Count > 0)
			{
				Dictionary<int, string> dictionary = new Dictionary<int, string>();
				for (int i = 0; i <= pageBreaks.Count - 1; i++)
				{
					if (UnmovableHeaderLinesCount > 0 && pageBreaks.Keys.ElementAtOrDefault(i) > UnmovableHeaderLinesCount)
					{
						dictionary.Add(pageBreaks.Keys.ElementAtOrDefault(i) - UnmovableHeaderLinesCount, pageBreaks.Values.ElementAtOrDefault(i));
					}
					else
					{
						dictionary.Add(pageBreaks.Keys.ElementAtOrDefault(i), pageBreaks.Values.ElementAtOrDefault(i));
					}
				}
				pageBreaks.Clear();
				pageBreaks = dictionary;
			}
			ActiveRow.ActiveRow = 0;
			_myFlowDocument.PageHeight = ScrollView.ActualHeight;
		}
		_flag = true;
		FindFillableArea(ref rowNum, ref endPos);
		if (rowNum > RepRows.Count - 1)
		{
			rowNum = RepRows.Count - 1;
		}
		if (endPos > RepRows.Count - 1)
		{
			endPos = RepRows.Count - 1;
		}
		if (rowNum != _filledStartValue || !(ScrollView.ActualHeight < _filledWidth) || forceRefresh)
		{
			_myFlowDocument.Blocks.Clear();
			for (int i = rowNum; i <= endPos; i++)
			{
				AddRepRow(RepRows[i], i);
			}
			_filledStartValue = rowNum;
			_filledEndValue = endPos;
			NavigateToRow(rowNum);
			double num = ScrollBar.ViewportSize / ((double)RepRows.Count + ScrollBar.ViewportSize);
			ScrollBar.ViewportSize -= num;
		}
	}

	private void ShiftUp()
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		Rect startRect = default;
		int rowNum = 0;
		ScrollViewer sc = new ScrollViewer();
		FindLastVisibleRow(ref startRect, ref rowNum, ref sc);
		if (_filledEndValue < RepRows.Count - 1)
		{
			AddRepRow(RepRows[_filledEndValue + 1], _filledEndValue + 1);
		}
		ScrollView.UpdateLayout();
		rowNum++;
		if (rowNum >= RepRows.Count)
		{
			rowNum = RepRows.Count - 1;
		}
		NavigateToRow(rowNum);
		ScrollBar.Value = rowNum;
	}

	private void ShiftDown()
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		Rect startRect = default;
		int rowNum = 0;
		ScrollViewer sc = new ScrollViewer();
		FindFirstVisibleRow(ref startRect, ref rowNum, ref sc);
		if (_filledStartValue > 0)
		{
			AddRepRow(RepRows[_filledStartValue - 1], _filledStartValue - 1, appendMode: false);
		}
		ScrollView.UpdateLayout();
		rowNum = ((rowNum >= 0) ? (rowNum - 1) : 0);
		NavigateToRow(rowNum);
		ScrollBar.Value = rowNum;
	}

	private void HomePressed(bool controlPressed)
	{
		if (controlPressed)
		{
			if (RepRows.Count != 0 && ActiveRow.ActiveRow != 0)
			{
				ScrollBar.Value = 0.0;
				FillVisiblePart(0);
				ActiveRow.ActiveRow = 0;
				NavigateToRow(0);
				ScrollView.UpdateLayout();
			}
		}
		else
		{
			MainScrollViewer.ScrollToLeftEnd();
		}
	}

	private void EndPressed(bool controlPressed)
	{
		if (controlPressed)
		{
			if (RepRows.Count != 0)
			{
				ScrollBar.Value = ScrollBar.Maximum;
				int num = RepRows.Count - 1;
				int num2 = num - 60;
				if (num2 < 0)
				{
					num2 = 0;
				}
				_myFlowDocument.Blocks.Clear();
				for (int i = num2; i <= num; i++)
				{
					AddRepRow(RepRows[i], i);
				}
				ActiveRow.ActiveRow = RepRows.Count - 1;
				_filledStartValue = num2;
				_filledEndValue = num;
				_filledWidth = ScrollView.ActualHeight;
				ScrollBar.Value = ScrollBar.Maximum;
				NavigateToRow(RepRows.Count - 1);
				ScrollView.UpdateLayout();
			}
		}
		else
		{
			MainScrollViewer.ScrollToRightEnd();
		}
	}

	private void SelectPgDown(bool fastMode = false)
	{
		int rowNum = -1;
		ScrollViewer sc = null;
		Rect startRect = default;
		FindFirstVisibleRow(ref startRect, ref rowNum, ref sc);
		ScrollViewer sc2 = null;
		FindLastVisibleRow(ref startRect, ref rowNum, ref sc2);
		int num = ((!fastMode) ? (rowNum + 1) : ((int)Math.Round((double)rowNum + (double)RepRows.Count * 0.05)));
		if (num > RepRows.Count - 1)
		{
			num = RepRows.Count - 1;
		}
		int num2 = num + 60;
		if (num2 > RepRows.Count - 1)
		{
			num2 = RepRows.Count - 1;
		}
		_myFlowDocument.Blocks.Clear();
		for (int i = num; i <= num2; i++)
		{
			AddRepRow(RepRows[i], i);
		}
		_filledStartValue = num;
		_filledEndValue = num2;
		_filledWidth = ScrollView.ActualHeight;
		ActiveRow.ActiveRow = num;
		NavigateToRow(num);
		ScrollBar.Value = ActiveRow.ActiveRow;
	}

	private void SelectPgUp(bool fastMode = false)
	{
		int rowNum = -1;
		int activeRow = ActiveRow.ActiveRow;
		ScrollViewer sc = null;
		Rect startRect = default;
		FindFirstVisibleRow(ref startRect, ref rowNum, ref sc);
		activeRow -= rowNum;
		if (activeRow < 0)
		{
			activeRow = 0;
		}
		int num = ((!fastMode) ? (rowNum - 60) : ((int)Math.Round((double)rowNum - (double)RepRows.Count * 0.05)));
		if (num < 0)
		{
			num = 0;
		}
		int num2 = num + 60;
		if (num2 > RepRows.Count - 1)
		{
			num2 = RepRows.Count - 1;
		}
		_myFlowDocument.Blocks.Clear();
		for (int i = num; i <= num2; i++)
		{
			AddRepRow(RepRows[i], i);
		}
		_filledStartValue = num;
		_filledEndValue = num2;
		_filledWidth = ScrollView.ActualHeight;
		if (num + activeRow > num2)
		{
			ActiveRow.ActiveRow = num2;
		}
		else
		{
			ActiveRow.ActiveRow = num + activeRow;
		}
		NavigateToRow(ActiveRow.ActiveRow);
		ScrollBar.Value = ActiveRow.ActiveRow;
	}

	internal void NavigateToRowByIndex(int rowNum)
	{
		int startPos = rowNum;
		int rowNum2 = -1;
		ScrollViewer sc = null;
		Rect startRect = default;
		FindLastVisibleRow(ref startRect, ref rowNum2, ref sc);
		if (rowNum <= rowNum2 && GetRowByIndex(rowNum) != null)
		{
			ActiveRow.ActiveRow = rowNum;
			NavigateToRow(rowNum);
			return;
		}
		rowNum2 = -1;
		FindFillableArea(ref startPos, ref rowNum2);
		_myFlowDocument.Blocks.Clear();
		ActiveRow.ActiveRow = startPos;
		for (int i = startPos; i <= rowNum2; i++)
		{
			AddRepRow(RepRows[i], i);
		}
		_filledStartValue = startPos;
		_filledEndValue = rowNum2;
		_filledWidth = ScrollView.ActualHeight;
		ScrollBar.Value = rowNum;
	}

	private void AddRepRow(AsRepRow repRow, int reportRowNum, bool appendMode = true, FlowDocument flowDocument = null)
	{
		List<AsRepRowCell> rowCells = repRow.RowCells;
		GetRowSizeAndStyle(repRow);
		System.Windows.Documents.TableRow tableRow = new System.Windows.Documents.TableRow();
		string blockName = repRow.BlockName;
		if (flowDocument == null)
		{
			flowDocument = _myFlowDocument;
		}
		tableRow.Name = "Row" + reportRowNum;
		if (flowDocument == _myFlowDocument)
		{
			tableRow.MouseLeftButtonDown += new SafeEventHandler<RoutedEventArgs>(SetActiveRow).Handler;
			tableRow.ContextMenuOpening += new SafeEventHandler<ContextMenuEventArgs>(TableRow_ContextMenuOpening).Handler;
		}
		TableCell item = CreateBulletCell(reportRowNum, flowDocument == _myFlowDocument);
		tableRow.Cells.Add(item);
		Table table = null;
		if (appendMode && flowDocument.Blocks.Count > 0 && flowDocument.Blocks.LastBlock.Tag != null && flowDocument.Blocks.LastBlock.Tag.ToString() == blockName && !string.IsNullOrWhiteSpace(blockName))
		{
			table = (Table)flowDocument.Blocks.LastBlock;
		}
		else if (!appendMode && flowDocument.Blocks.Count > 0 && flowDocument.Blocks.FirstBlock.Tag != null && flowDocument.Blocks.FirstBlock.Tag.ToString() == blockName && !string.IsNullOrWhiteSpace(blockName))
		{
			table = (Table)flowDocument.Blocks.FirstBlock;
		}
		if (table == null)
		{
			table = new Table
			{
				Margin = new Thickness(0.0),
				CellSpacing = 0.0,
				Tag = blockName
			};
			table.RowGroups.Add(new TableRowGroup());
			table.Columns.Add(new TableColumn());
			table.Columns[0].Width = new GridLength(BulletColWidth);
			if (appendMode)
			{
				flowDocument.Blocks.Add(table);
			}
			else
			{
				flowDocument.Blocks.InsertBefore(_myFlowDocument.Blocks.FirstBlock, table);
			}
		}
		else
		{
			table.Columns[0].Width = new GridLength(BulletColWidth);
		}
		if (!string.IsNullOrWhiteSpace(blockName) && TablesDefs.TryGetValue(blockName, out var value) && table.RowGroups[0].Rows.Count == 0)
		{
			foreach (AsRepColumn columnDefinition in value.ColumnDefinitions)
			{
				TableColumn item2 = new TableColumn
				{
					Width = new GridLength(columnDefinition.WidthD)
				};
				table.Columns.Add(item2);
			}
		}
		for (int i = 0; i <= rowCells.Count - 1; i++)
		{
			TableCell tableCell = CreateTableCell(reportRowNum, rowCells[i], i);
			if (flowDocument == _myFlowDocument)
			{
				tableCell.MouseLeftButtonDown += new SafeEventHandler<MouseButtonEventArgs>(SelectCell).Handler;
			}
			tableRow.Cells.Add(tableCell);
		}
		FillColor(tableRow);
		int num = 0;
		foreach (Block block in flowDocument.Blocks)
		{
			if (block is Table table2)
			{
				num += table2.RowGroups[0].Rows.Count;
			}
		}
		if (flowDocument == _myFlowDocument)
		{
			if (!appendMode)
			{
				table.RowGroups[0].Rows.Insert(0, tableRow);
				_filledStartValue = reportRowNum;
				if (num <= 70)
				{
					return;
				}
				while (num > 60)
				{
					Table table3 = (Table)flowDocument.Blocks.ElementAt(flowDocument.Blocks.Count - 1);
					num--;
					table3.RowGroups[0].Rows.RemoveAt(table3.RowGroups[0].Rows.Count - 1);
					if (table3.RowGroups[0].Rows.Count == 0)
					{
						flowDocument.Blocks.Remove(table3);
					}
					table3 = (Table)flowDocument.Blocks.LastBlock;
					System.Windows.Documents.TableRowCollection rows = table3.RowGroups[0].Rows;
					System.Windows.Documents.TableRow tableRow2 = rows[rows.Count - 1];
					_filledEndValue = Convert.ToInt32(tableRow2.Name.Mid(4));
				}
				_scrollViewer.ScrollToHome();
				ScrollView.UpdateLayout();
				return;
			}
			_filledEndValue = reportRowNum;
			table.RowGroups[0].Rows.Add(tableRow);
			if (num <= 70)
			{
				return;
			}
			while (num > 60)
			{
				Table table4 = (Table)flowDocument.Blocks.ElementAt(0);
				num--;
				table4.RowGroups[0].Rows.RemoveAt(0);
				if (table4.RowGroups[0].Rows.Count == 0)
				{
					flowDocument.Blocks.Remove(table4);
				}
				table4 = (Table)flowDocument.Blocks.FirstBlock;
				System.Windows.Documents.TableRow tableRow3 = table4.RowGroups[0].Rows[0];
				_filledStartValue = Convert.ToInt32(tableRow3.Name.Mid(4));
			}
			_scrollViewer.ScrollToHome();
			ScrollView.UpdateLayout();
		}
		else if (!appendMode)
		{
			table.RowGroups[0].Rows.Insert(0, tableRow);
		}
		else
		{
			_filledEndValue = reportRowNum;
			table.RowGroups[0].Rows.Add(tableRow);
		}
	}

	private TableCell CreateBulletCell(int rowNum, bool fillNumber = true)
	{
		TableCell tableCell = new TableCell
		{
			Padding = new Thickness(2.0, 2.0, 0.0, 0.0),
			BorderBrush = (Settings.IsDarkTheme ? Brushes.White : Brushes.Black),
			BorderThickness = new Thickness(0.5, 0.5, 1.0, 0.5),
			Name = "Cell" + rowNum
		};
		StackPanel stackPanel = new StackPanel
		{
			HorizontalAlignment = HorizontalAlignment.Stretch,
			VerticalAlignment = VerticalAlignment.Center,
			Background = Brushes.Transparent,
			Orientation = Orientation.Horizontal
		};
		if (fillNumber)
		{
			tableCell.Blocks.Add(new BlockUIContainer(stackPanel));
			TextBlock textBlock = new TextBlock
			{
				HorizontalAlignment = HorizontalAlignment.Left
			};
			_ = RepRows.Count.ToString().Length;
			textBlock.Width = BulletColWidth - 13;
			textBlock.Text = (rowNum + 1).ToString();
			stackPanel.Children.Add(textBlock);
			tableCell.MouseLeftButtonDown += new SafeEventHandler<RoutedEventArgs>(SelectRow).Handler;
			tableCell.Background = Settings.RepBulletBackgroundBrush();
			tableCell.Foreground = Settings.GetForegroundBrush();
			tableCell.FontSize = 10.0;
		}
		return tableCell;
	}

	private string CopySelection()
	{
		if (SelectedRows.Count == 0 && (ActiveRow == null || ActiveRow.ActiveRow == -1))
		{
			return "";
		}
		string text = "";
		if (SelectedRows.Count == 0)
		{
			for (int i = 0; i <= RepRows[ActiveRow.ActiveRow].RowCells.Count - 1; i++)
			{
				text = text + "\t" + RepRows[ActiveRow.ActiveRow].RowCells[i].Value.ToString();
			}
			text += Environment.NewLine;
		}
		else
		{
			foreach (int key in SelectedRows.Keys)
			{
				for (int j = 0; j <= RepRows[key].RowCells.Count - 1; j++)
				{
					text = text + "\t" + RepRows[key].RowCells[j].Value.ToString();
				}
				text += Environment.NewLine;
			}
		}
		Clipboard.Clear();
		Clipboard.SetDataObject(text);
		return text;
	}

	private void CopyAll()
	{
		int num = 0;
		int num2 = RepRows.Count - 1;
		MemoryStream memoryStream = new MemoryStream();
		if (MessageBox.Show(ArmSoft.SME.Common.Resources.CopyAll, ArmSoft.SME.Common.Resources.Copy, MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No) != MessageBoxResult.Yes)
		{
			return;
		}
		StreamWriter streamWriter = new StreamWriter(memoryStream);
		for (int i = num; i <= num2; i++)
		{
			string text = "";
			for (int j = 0; j <= RepRows[i].RowCells.Count - 1; j++)
			{
				text = text + "\t" + RepRows[i].RowCells[j].Value.ToString();
			}
			streamWriter.WriteLine(text);
		}
		streamWriter.Flush();
		StreamReader streamReader = new StreamReader(memoryStream);
		Clipboard.Clear();
		memoryStream.Seek(0L, SeekOrigin.Begin);
		Clipboard.SetDataObject(streamReader.ReadToEnd());
		MessageBox.Show(ArmSoft.SME.Common.Resources.CopiedAll, ArmSoft.SME.Common.Resources.CopyAll);
	}

	private static double DefaultColWidth(AsRepColStyle style)
	{
		return style switch
		{
			AsRepColStyle.TextStyle => 250.0, 
			AsRepColStyle.CurrencyStyle => 150.0, 
			AsRepColStyle.BooleanStyle => 50.0, 
			AsRepColStyle.DateStyle => 80.0, 
			_ => 150.0, 
		};
	}

	private void GetRowSizeAndStyle(AsRepRow repRow)
	{
		if (repRow.Rendered)
		{
			return;
		}
		FormatRow(repRow);
		List<AsRepRowCell> rowCells = repRow.RowCells;
		if (TablesDefs.TryGetValue(repRow.BlockName, out var value))
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
		}
		else
		{
			for (int l = 0; l <= rowCells.Count - 1; l++)
			{
				if (rowCells[l].WidthD == 0.0)
				{
					rowCells[l].WidthD = DefaultColWidth(rowCells[l].ValueStyle);
				}
				FormatCell(rowCells[l], repRow);
			}
		}
		repRow.Rendered = true;
	}

	private void FormatRow(AsRepRow repRow)
	{
		if (repRow.StyleCollection.TryGetValue("FontStyle", out var value))
		{
			repRow.FontStyle = (FontStyle)value;
		}
		if (repRow.StyleCollection.TryGetValue("FontWeight", out value))
		{
			repRow.FontWeight = (FontWeight)value;
		}
		if (repRow.StyleCollection.TryGetValue("FontSize", out value))
		{
			repRow.FontSize = (int)value;
		}
		if (repRow.RowType == AsRepRowType.Text && repRow.RowCells.Count > 0 && repRow.RowCells[0].WidthD <= 0.0)
		{
			repRow.RowCells[0].WidthD = _myFlowDocument.PageWidth;
		}
	}

	private static object DefaultRowFormat(AsRepRow repRow, string formatKey)
	{
		double fontSize = Settings.GetFontSize();
		if (formatKey == "FontStyle")
		{
			return FontStyles.Normal;
		}
		switch (repRow.RowStyle)
		{
		case AsRepRowStyle.CaptionRow:
		{
			string text = formatKey ?? "";
			if (!(text == "FontWeight"))
			{
				if (!(text == "FontSize"))
				{
					break;
				}
				return fontSize;
			}
			return FontWeights.Bold;
		}
		case AsRepRowStyle.TableColumnsHeader:
		{
			string text = formatKey ?? "";
			if (!(text == "FontWeight"))
			{
				if (!(text == "FontSize"))
				{
					break;
				}
				return fontSize;
			}
			return FontWeights.Bold;
		}
		default:
		{
			string text = formatKey ?? "";
			if (!(text == "FontWeight"))
			{
				if (!(text == "FontSize"))
				{
					break;
				}
				return fontSize;
			}
			return FontWeights.Normal;
		}
		}
		return null;
	}

	private void FillColor(object reportRow, bool deselect = false)
	{
		if (reportRow == null)
		{
			return;
		}
		int num;
		if (reportRow.ToString().IsNumeric())
		{
			num = Convert.ToInt32(reportRow);
			reportRow = GetRowByIndex(num);
		}
		else
		{
			num = GetRowIndex(reportRow);
		}
		if (reportRow == null || !(reportRow is System.Windows.Documents.TableRow tableRow))
		{
			return;
		}
		if (num == ActiveRow.ActiveRow && !deselect)
		{
			tableRow.Background = Settings.GetActiveRowBrush();
			SetBullet(tableRow, modeOn: true);
			if (SelectedRows.ContainsKey(num))
			{
				tableRow.Cells[0].Background = _selectedRowBrush;
			}
			else
			{
				tableRow.Cells[0].Background = tableRow.Background;
			}
		}
		else if (SelectedRows.ContainsKey(num))
		{
			if (_selectedRowBrush == null)
			{
				tableRow.Background = Brushes.SandyBrown;
			}
			else
			{
				tableRow.Background = _selectedRowBrush;
			}
			tableRow.Cells[0].Background = tableRow.Background;
		}
		else if (RepRows[num].IsTableCaption)
		{
			tableRow.Background = Settings.GetBackgroundBrush();
			tableRow.FontWeight = FontWeights.Bold;
			tableRow.Cells[0].Background = Settings.RepBulletBackgroundBrush();
		}
		else
		{
			if (RepRows[num].RowType == AsRepRowType.Text)
			{
				tableRow.Background = Settings.GetBackgroundBrush();
			}
			else
			{
				tableRow.Background = Settings.GetBackgroundBrush();
			}
			tableRow.Cells[0].Background = Settings.RepBulletBackgroundBrush();
		}
		if (pageBreaks.ContainsKey(num) && tableRow != null)
		{
			for (int i = 1; i < tableRow.Cells.Count; i++)
			{
				tableRow.Cells[i].BorderBrush = Settings.GetReportSeparatorBrush();
				tableRow.Cells[i].BorderThickness = new Thickness(0.0, 0.0, 0.0, 2.0);
			}
		}
	}

	private void Scroll(object sender, ScrollEventArgs e)
	{
		if (RepRows.Count != 0)
		{
			if ((e.ScrollEventType == ScrollEventType.SmallIncrement) | (e.ScrollEventType == ScrollEventType.LargeIncrement))
			{
				ShiftUp();
				e.Handled = true;
			}
			else if ((e.ScrollEventType == ScrollEventType.SmallDecrement) | (e.ScrollEventType == ScrollEventType.LargeDecrement))
			{
				ShiftDown();
				e.Handled = true;
			}
			else if (e.ScrollEventType != ScrollEventType.ThumbPosition && e.ScrollEventType < ScrollEventType.ThumbTrack)
			{
				int num = (int)ScrollBar.Value;
				FillVisiblePart(num);
				ActiveRow.ActiveRow = num;
				e.Handled = true;
			}
			if (e.NewValue == ScrollBar.Maximum)
			{
				EndPressed(controlPressed: true);
			}
			e.Handled = true;
		}
	}

	private void TableRow_ContextMenuOpening(object sender, ContextMenuEventArgs e)
	{
		if (ScrollView.Selection != null)
		{
			TextPointer start = ScrollView.Selection.Start;
			TextPointer end = ScrollView.Selection.End;
			if (!string.IsNullOrWhiteSpace(new TextRange(start, end).Text.Trim()))
			{
				BarManager.SetDXContextMenu(this, null);
				e.Handled = false;
				return;
			}
		}
		SetActiveRow(sender, null);
		if (RepRows.Count != 0)
		{
			object hiddenData = RepRows[ActiveRow.ActiveRow].HiddenData;
			if (hiddenData == null)
			{
				e.Handled = true;
				return;
			}
			_myReport.GetContextMenu(hiddenData);
		}
		if (ReportContextMenu == null || ReportContextMenu.ItemLinks.Count == 0)
		{
			e.Handled = true;
			return;
		}
		BarManager.SetDXContextMenu(this, ReportContextMenu);
		ContextMenuManager.ShowElementContextMenu(this);
		e.Handled = true;
	}

	private void FlowDocument_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
	{
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Invalid comparison between Unknown and I4
		if (RepRows.Count == 0)
		{
			return;
		}
		if (MouseWheelCount == 1)
		{
			MouseWheelCount = 2;
			return;
		}
		MouseWheelCount = 2;
		if ((int)Keyboard.Modifiers == 2)
		{
			if (MainScrollViewer != null)
			{
				MainScrollViewer.ScrollToHorizontalOffset(MainScrollViewer.HorizontalOffset - (double)e.Delta);
			}
			e.Handled = true;
		}
		else if (e.Delta < 0)
		{
			for (int i = 1; i <= 5; i++)
			{
				if (_filledEndValue < RepRows.Count - 1)
				{
					AddRepRow(RepRows[_filledEndValue + 1], _filledEndValue + 1);
				}
			}
			_scrollViewer.ScrollToVerticalOffset(_scrollViewer.VerticalOffset - (double)(e.Delta / 5));
			ScrollBar.Value = _filledEndValue;
		}
		else
		{
			for (int j = 1; j <= 5; j++)
			{
				if (_filledStartValue > 0)
				{
					AddRepRow(RepRows[_filledStartValue - 1], _filledStartValue - 1, appendMode: false);
				}
			}
			_scrollViewer.ScrollToVerticalOffset(_scrollViewer.VerticalOffset - (double)(e.Delta / 5));
			ScrollBar.Value = _filledStartValue;
		}
		e.Handled = true;
	}

	private void NavigateToRow(int rowNum)
	{
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		object rowByIndex = GetRowByIndex(rowNum);
		if (rowByIndex == null)
		{
			return;
		}
		ScrollViewer scrollViewer = FindScrollViewer((DependencyObject)(object)ScrollView);
		if (!(rowByIndex is System.Windows.Documents.TableRow tableRow))
		{
			return;
		}
		Rect characterRect = tableRow.ContentStart.GetCharacterRect(LogicalDirection.Forward);
		if (scrollViewer != null && (characterRect.Bottom > scrollViewer.ViewportHeight || characterRect.Top < 0.0))
		{
			if (characterRect.Top < 0.0)
			{
				scrollViewer.ScrollToVerticalOffset(scrollViewer.VerticalOffset + characterRect.Top);
			}
			else
			{
				scrollViewer.ScrollToVerticalOffset(scrollViewer.VerticalOffset + characterRect.Bottom - scrollViewer.ViewportHeight);
			}
		}
	}

	private DependencyObject GetNext(int rowNum)
	{
		return LogicalTreeHelper.FindLogicalNode((DependencyObject)(object)ScrollView, "Row" + (rowNum + 1));
	}

	private DependencyObject GetPrevious(int rowNum)
	{
		return LogicalTreeHelper.FindLogicalNode((DependencyObject)(object)ScrollView, "Row" + (rowNum - 1));
	}

	private DependencyObject GetRowByIndex(int index)
	{
		return LogicalTreeHelper.FindLogicalNode((DependencyObject)(object)ScrollView, "Row" + index);
	}

	private static int GetRowIndex(object tableOrParagraph)
	{
		string str = "";
		if (tableOrParagraph is Paragraph paragraph)
		{
			str = paragraph.Name;
		}
		else if (tableOrParagraph is System.Windows.Documents.TableRow tableRow)
		{
			str = tableRow.Name;
		}
		if (str.Mid(4).IsNumeric())
		{
			return Convert.ToInt32(str.Mid(4));
		}
		return -1;
	}

	private ScrollViewer FindScrollViewer(DependencyObject el)
	{
		if (_scrollViewer != null)
		{
			return _scrollViewer;
		}
		if (VisualTreeHelper.GetChildrenCount(el) == 0)
		{
			return null;
		}
		for (int i = 0; i <= VisualTreeHelper.GetChildrenCount(el); i++)
		{
			DependencyObject child = VisualTreeHelper.GetChild(el, i);
			if (child is ScrollViewer result)
			{
				return result;
			}
			ScrollViewer scrollViewer = FindScrollViewer(child);
			if (scrollViewer != null)
			{
				return scrollViewer;
			}
		}
		return null;
	}

	private static Border FindBorder(DependencyObject el)
	{
		if (VisualTreeHelper.GetChildrenCount(el) == 0)
		{
			return null;
		}
		for (int i = 0; i <= VisualTreeHelper.GetChildrenCount(el); i++)
		{
			DependencyObject child = VisualTreeHelper.GetChild(el, i);
			if (child is Border result)
			{
				return result;
			}
			Border border = FindBorder(child);
			if (border != null)
			{
				return border;
			}
		}
		return null;
	}

	private void SelectRow()
	{
		if (GetRowByIndex(ActiveRow.ActiveRow) == null)
		{
			ScrollBar.Value = ActiveRow.ActiveRow;
			FillVisiblePart(ActiveRow.ActiveRow);
		}
		if (SelectedRows.ContainsKey(ActiveRow.ActiveRow))
		{
			SelectedRows.Remove(ActiveRow.ActiveRow);
			ActiveRow.SelectedCount--;
		}
		else
		{
			SelectedRows.Add(ActiveRow.ActiveRow, RepRows[ActiveRow.ActiveRow].HiddenData);
			ActiveRow.SelectedCount++;
		}
		if (ActiveRow.ActiveRow == RepRows.Count - 1)
		{
			FillColor(ActiveRow.ActiveRow);
			return;
		}
		if (GetNext(ActiveRow.ActiveRow) == null)
		{
			AddRepRow(RepRows[ActiveRow.ActiveRow + 1], ActiveRow.ActiveRow + 1);
			GetRowByIndex(ActiveRow.ActiveRow + 1);
		}
		ActiveRow.ActiveRow++;
		NavigateToRow(ActiveRow.ActiveRow);
		ScrollBar.Value++;
		ScrollView.UpdateLayout();
	}

	private void SelectNextRow()
	{
		if (ActiveRow.ActiveRow != RepRows.Count - 1)
		{
			if (GetRowByIndex(ActiveRow.ActiveRow) == null)
			{
				ScrollBar.Value = ActiveRow.ActiveRow;
				FillVisiblePart(ActiveRow.ActiveRow);
			}
			if (GetNext(ActiveRow.ActiveRow) == null)
			{
				int num = ActiveRow.ActiveRow + 1;
				AddRepRow(RepRows[num], num);
			}
			ActiveRow.ActiveRow++;
			NavigateToRow(ActiveRow.ActiveRow);
			ScrollBar.Value++;
		}
	}

	private void SelectPreviousRow()
	{
		if (ActiveRow.ActiveRow != 0)
		{
			if (GetRowByIndex(ActiveRow.ActiveRow) == null)
			{
				ScrollBar.Value = ActiveRow.ActiveRow;
				FillVisiblePart(ActiveRow.ActiveRow);
			}
			if (GetPrevious(ActiveRow.ActiveRow) == null)
			{
				AddRepRow(RepRows[ActiveRow.ActiveRow - 1], ActiveRow.ActiveRow - 1, appendMode: false);
			}
			ActiveRow.ActiveRow--;
			NavigateToRow(ActiveRow.ActiveRow);
			ScrollBar.Value--;
		}
	}

	private object FindFirstVisibleRow(ref Rect startRect, [Optional][DefaultParameterValue(-1)] ref int rowNum, [Optional][DefaultParameterValue(null)] ref ScrollViewer sc)
	{
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		object result = null;
		sc = FindScrollViewer((DependencyObject)(object)ScrollView);
		if (sc == null)
		{
			result = LogicalTreeHelper.FindLogicalNode((DependencyObject)(object)ScrollView, "Row" + _filledStartValue);
			rowNum = _filledStartValue;
			return result;
		}
		for (int i = _filledStartValue; i <= _filledEndValue; i++)
		{
			object rowByIndex = GetRowByIndex(i);
			if (rowByIndex != null && rowByIndex is System.Windows.Documents.TableRow tableRow)
			{
				startRect = tableRow.ContentEnd.GetCharacterRect(LogicalDirection.Forward);
				if (startRect.Bottom >= 0.0)
				{
					rowNum = i;
					result = tableRow;
					break;
				}
			}
		}
		if (startRect != Rect.Empty)
		{
			return result;
		}
		return null;
	}

	private void FindRowPosition(int rowNum, ref Rect rect)
	{
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		if (LogicalTreeHelper.FindLogicalNode((DependencyObject)(object)ScrollView, "Row" + rowNum) is System.Windows.Documents.TableRow tableRow)
		{
			rect = tableRow.ContentEnd.GetCharacterRect(LogicalDirection.Forward);
		}
	}

	private object FindLastVisibleRow(ref Rect startRect, ref int rowNum, ref ScrollViewer sc)
	{
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		object result = null;
		sc = FindScrollViewer((DependencyObject)(object)ScrollView);
		if (sc == null)
		{
			return LogicalTreeHelper.FindLogicalNode((DependencyObject)(object)ScrollView, "Row" + _filledEndValue);
		}
		for (int num = _filledEndValue; num >= _filledStartValue; num--)
		{
			if (LogicalTreeHelper.FindLogicalNode((DependencyObject)(object)ScrollView, "Row" + num) is System.Windows.Documents.TableRow tableRow)
			{
				startRect = tableRow.ContentEnd.GetCharacterRect(LogicalDirection.Forward);
				if (startRect.Top <= sc.ViewportHeight)
				{
					rowNum = num;
					result = tableRow;
					break;
				}
			}
		}
		if (startRect != Rect.Empty)
		{
			return result;
		}
		return null;
	}

	private void FlowDocument_PreviewKeyDown(object sender, KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Invalid comparison between Unknown and I4
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Invalid comparison between Unknown and I4
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Invalid comparison between Unknown and I4
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Invalid comparison between Unknown and I4
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_008d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Invalid comparison between Unknown and I4
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		//IL_009c: Invalid comparison between Unknown and I4
		if ((int)e.Key == 49 && (int)Keyboard.Modifiers == 2)
		{
			e.Handled = true;
			ShowSearchPanel();
		}
		else if ((int)e.Key == 92)
		{
			if ((int)Keyboard.Modifiers == 2)
			{
				SearchUpCheckEdit.IsChecked = true;
			}
			else if ((int)Keyboard.Modifiers == 0)
			{
				SearchUpCheckEdit.IsChecked = false;
			}
			SearchTextInReport();
			e.Handled = true;
		}
		else if (SearchPanel.IsActive && SearchPanel.Visibility == Visibility.Visible && (int)Keyboard.Modifiers == 0 && ((int)e.Key == 6 || (int)e.Key == 6))
		{
			SearchPanel_PreviewKeyDown(sender, e);
		}
		else if (!MainReportPanel.IsActive)
		{
			e.Handled = false;
		}
		else
		{
			Report_PreviewKeyDown(sender, e);
		}
	}

	private void Report_PreviewKeyDown(object sender, KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Invalid comparison between Unknown and I4
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Invalid comparison between Unknown and I4
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Invalid comparison between Unknown and I4
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Invalid comparison between Unknown and I4
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Invalid comparison between Unknown and I4
		//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_00af: Invalid comparison between Unknown and I4
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Invalid comparison between Unknown and I4
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Invalid comparison between Unknown and I4
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Expected I4, but got Unknown
		//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ba: Invalid comparison between Unknown and I4
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a2: Invalid comparison between Unknown and I4
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Invalid comparison between Unknown and I4
		//IL_0574: Unknown result type (might be due to invalid IL or missing references)
		//IL_0576: Invalid comparison between Unknown and I4
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Invalid comparison between Unknown and I4
		//IL_026d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0273: Invalid comparison between Unknown and I4
		//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c5: Invalid comparison between Unknown and I4
		//IL_04bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c2: Invalid comparison between Unknown and I4
		//IL_02f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fc: Invalid comparison between Unknown and I4
		//IL_039f: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a5: Invalid comparison between Unknown and I4
		//IL_0378: Unknown result type (might be due to invalid IL or missing references)
		//IL_037e: Invalid comparison between Unknown and I4
		//IL_04c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ca: Invalid comparison between Unknown and I4
		//IL_033f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0345: Invalid comparison between Unknown and I4
		//IL_0235: Unknown result type (might be due to invalid IL or missing references)
		//IL_023b: Invalid comparison between Unknown and I4
		//IL_019a: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a0: Invalid comparison between Unknown and I4
		//IL_0586: Unknown result type (might be due to invalid IL or missing references)
		//IL_0157: Unknown result type (might be due to invalid IL or missing references)
		//IL_015d: Invalid comparison between Unknown and I4
		if ((int)e.Key == 93 && (int)Keyboard.Modifiers == 2)
		{
			Settings.HostingEnvironment.ClosePanel(this);
			e.Handled = true;
			return;
		}
		Key key = e.Key;
		if ((int)key <= 72)
		{
			if ((int)key <= 46)
			{
				switch (key - 19)
				{
				default:
					if ((int)key != 46)
					{
						break;
					}
					if (RepRows.Count > 0 && (int)Keyboard.Modifiers == 2)
					{
						CopySelection();
					}
					return;
				case 7:
					if (RepRows.Count <= 0)
					{
						return;
					}
					if (e.IsRepeat && _stopRepeat)
					{
						_stopRepeat = false;
						e.Handled = true;
						return;
					}
					if ((int)Keyboard.Modifiers != 4)
					{
						SelectNextRow();
						e.Handled = true;
					}
					else
					{
						e.Handled = false;
					}
					_stopRepeat = true;
					return;
				case 5:
					if (RepRows.Count > 0)
					{
						if (e.IsRepeat && _stopRepeat)
						{
							_stopRepeat = false;
							e.Handled = true;
						}
						else
						{
							SelectPreviousRow();
							_stopRepeat = true;
							e.Handled = true;
						}
					}
					return;
				case 1:
					if (RepRows.Count <= 0)
					{
						return;
					}
					if (e.IsRepeat && _stopRepeat)
					{
						_stopRepeat = false;
						e.Handled = true;
						return;
					}
					if ((int)Keyboard.Modifiers == 2)
					{
						SelectPgDown(fastMode: true);
					}
					else
					{
						SelectPgDown();
					}
					e.Handled = true;
					_stopRepeat = true;
					return;
				case 0:
					if (RepRows.Count <= 0)
					{
						return;
					}
					if (e.IsRepeat && _stopRepeat)
					{
						_stopRepeat = false;
						e.Handled = true;
						return;
					}
					if ((int)Keyboard.Modifiers == 2)
					{
						SelectPgUp(fastMode: true);
					}
					else
					{
						SelectPgUp();
					}
					_stopRepeat = true;
					e.Handled = true;
					return;
				case 3:
					if (RepRows.Count > 0)
					{
						HomePressed((int)Keyboard.Modifiers == 2);
						e.Handled = true;
					}
					return;
				case 2:
					if (RepRows.Count > 0)
					{
						EndPressed((int)Keyboard.Modifiers == 2);
						e.Handled = true;
					}
					return;
				case 4:
					if (RepRows.Count > 0)
					{
						MainScrollViewer.ScrollToHorizontalOffset(MainScrollViewer.HorizontalOffset - 100.0);
						e.Handled = true;
					}
					return;
				case 6:
					if (RepRows.Count > 0)
					{
						MainScrollViewer.ScrollToHorizontalOffset(MainScrollViewer.HorizontalOffset + 100.0);
						e.Handled = true;
					}
					return;
				case 12:
					if (RepRows.Count > 0)
					{
						SelectRow();
						e.Handled = true;
					}
					return;
				case 8:
				case 9:
				case 10:
				case 11:
					break;
				}
			}
			else
			{
				if ((int)key == 49)
				{
					if (RepRows.Count > 0 && (int)Keyboard.Modifiers == 2)
					{
						ShowSearchPanel();
						e.Handled = true;
					}
					return;
				}
				if ((int)key == 72)
				{
					if (RepRows.Count > 0 && ActiveRow.ActiveRow > -1)
					{
						object hiddenData = RepRows[ActiveRow.ActiveRow].HiddenData;
						if (hiddenData != null)
						{
							GetRowByIndex(ActiveRow.ActiveRow);
							_myReport.GetContextMenu(hiddenData);
							BarManager.SetDXContextMenu(this, ReportContextMenu);
							ContextMenuManager.ShowElementContextMenu(this);
						}
					}
					return;
				}
			}
		}
		else if ((int)key <= 87)
		{
			if ((int)key == 85)
			{
				goto IL_04a9;
			}
			if ((int)key == 87)
			{
				goto IL_0445;
			}
		}
		else
		{
			if ((int)key == 92)
			{
				if (RepRows.Count <= 0)
				{
					return;
				}
				if ((int)Keyboard.Modifiers == 2)
				{
					if (string.IsNullOrWhiteSpace(gFindInfo.Pattern))
					{
						ShowSearchPanel();
					}
					else
					{
						gFindInfo.SearchDirection = FindDirection.Back;
						FindText(gFindInfo);
					}
				}
				else if (string.IsNullOrWhiteSpace(gFindInfo.Pattern))
				{
					ShowSearchPanel();
				}
				else
				{
					gFindInfo.SearchDirection = FindDirection.Forward;
					FindText(gFindInfo);
				}
				e.Handled = true;
				return;
			}
			if ((int)key == 141)
			{
				goto IL_04a9;
			}
			if ((int)key == 143)
			{
				goto IL_0445;
			}
		}
		if ((int)key == 6)
		{
			if (RepRows.Count > 0 && (int)Keyboard.Modifiers == 0)
			{
				ExecDefaultFunction();
				e.Handled = true;
			}
		}
		else
		{
			e.Handled = false;
		}
		return;
		IL_04a9:
		if (RepRows.Count <= 0)
		{
			return;
		}
		if ((int)e.Key == 44 && (int)Keyboard.Modifiers != 2)
		{
			e.Handled = false;
			return;
		}
		SelectedRows.Clear();
		for (int i = 0; i <= RepRows.Count - 1; i++)
		{
			SelectedRows.Add(i, RepRows[i].HiddenData);
		}
		ActiveRow.SelectedCount = RepRows.Count;
		for (int j = _filledStartValue; j <= _filledEndValue; j++)
		{
			object rowByIndex = GetRowByIndex(j);
			if (rowByIndex != null)
			{
				FillColor(rowByIndex);
			}
		}
		ScrollView.UpdateLayout();
		e.Handled = true;
		return;
		IL_0445:
		if (RepRows.Count <= 0)
		{
			return;
		}
		SelectedRows.Clear();
		ActiveRow.SelectedCount = 0;
		for (int k = _filledStartValue; k <= _filledEndValue; k++)
		{
			object rowByIndex2 = GetRowByIndex(k);
			if (rowByIndex2 != null)
			{
				FillColor(rowByIndex2);
			}
		}
		ScrollView.UpdateLayout();
		e.Handled = true;
	}

	private static void FormatCell(AsRepRowCell repRowCell, AsRepColumn repColumn, AsRepRow repRow)
	{
		object value;
		if (!repRowCell.StyleCollection.ContainsKey("Alignment"))
		{
			if (repRow.IsTableCaption)
			{
				if (repColumn.StyleCollection.TryGetValue("CaptionAlignment", out value))
				{
					repRowCell.Alignment = (System.Windows.TextAlignment)value;
				}
			}
			else if (repColumn.StyleCollection.TryGetValue("DataAlignment", out value))
			{
				repRowCell.Alignment = (System.Windows.TextAlignment)value;
			}
		}
		if (!repRowCell.StyleCollection.ContainsKey("Alignment"))
		{
			repRowCell.Alignment = System.Windows.TextAlignment.Left;
		}
		if (!repRowCell.StyleCollection.ContainsKey("FontWeight"))
		{
			if (repRow.StyleCollection.TryGetValue("FontWeight", out value))
			{
				repRowCell.FontWeight = (FontWeight)value;
			}
			else if (repRow.IsTableCaption && repColumn.StyleCollection.TryGetValue("CaptionFontWeight", out value))
			{
				repRowCell.FontWeight = (FontWeight)value;
			}
			else if (repColumn.StyleCollection.TryGetValue("DataFontWeight", out value))
			{
				repRowCell.FontWeight = (FontWeight)value;
			}
		}
		if (!repRowCell.StyleCollection.ContainsKey("FontWeight"))
		{
			repRowCell.FontWeight = (FontWeight)DefaultRowFormat(repRow, "FontWeight");
		}
		if (!repRowCell.StyleCollection.ContainsKey("FontStyle"))
		{
			if (repRow.StyleCollection.TryGetValue("FontStyle", out value))
			{
				repRowCell.FontStyle = (FontStyle)value;
			}
			else if (repRow.IsTableCaption && repColumn.StyleCollection.TryGetValue("CaptionFontStyle", out value))
			{
				repRowCell.FontStyle = (FontStyle)value;
			}
			else if (repColumn.StyleCollection.TryGetValue("DataFontStyle", out value))
			{
				repRowCell.FontStyle = (FontStyle)value;
			}
		}
		if (!repRowCell.StyleCollection.ContainsKey("FontStyle"))
		{
			repRowCell.FontStyle = (FontStyle)DefaultRowFormat(repRow, "FontStyle");
		}
		if (!repRowCell.StyleCollection.ContainsKey("FontSize"))
		{
			if (repRow.StyleCollection.TryGetValue("FontSize", out value))
			{
				repRowCell.FontSize = (int)value;
			}
			else if (repRow.IsTableCaption && repColumn.StyleCollection.TryGetValue("CaptionFontSize", out value))
			{
				repRowCell.FontSize = (int)value;
			}
			else if (repColumn.StyleCollection.TryGetValue("DataFontSize", out value))
			{
				repRowCell.FontSize = (int)value;
			}
		}
		if (!repRowCell.StyleCollection.ContainsKey("FontSize"))
		{
			repRowCell.FontSize = Convert.ToInt32(DefaultRowFormat(repRow, "FontSize"));
		}
		if (!repRowCell.StyleCollection.ContainsKey("CurrencyLen") && repColumn.StyleCollection.TryGetValue("CurrencyLen", out value))
		{
			repRowCell.CurrencyLen = (short)value;
		}
		if (!repRowCell.StyleCollection.ContainsKey("CurrencyLen"))
		{
			repRowCell.CurrencyLen = 16;
		}
		if (!repRowCell.StyleCollection.ContainsKey("CurrencyPrecision") && repColumn.StyleCollection.TryGetValue("CurrencyPrecision", out value))
		{
			repRowCell.CurrencyPrecision = (short)value;
		}
		if (!repRowCell.StyleCollection.ContainsKey("CurrencyPrecision"))
		{
			repRowCell.CurrencyPrecision = 2;
		}
		if (repRowCell.Borders == null || repRowCell.Borders.Count == 0)
		{
			if (repRow.IsTableCaption && repColumn.StyleCollection.TryGetValue("CaptionBorders", out value))
			{
				repRowCell.Borders = (ObservableCollection<BorderType>)value;
			}
			else if (repColumn.StyleCollection.TryGetValue("DataBorders", out value))
			{
				repRowCell.Borders = (ObservableCollection<BorderType>)value;
			}
		}
		if (!repRowCell.StyleCollection.ContainsKey("BackgroundColor"))
		{
			if (repRow.IsTableCaption && repColumn.StyleCollection.TryGetValue("CaptionBackgroundColor", out value))
			{
				repRowCell.BackGround = (Color)value;
			}
			else if (repColumn.StyleCollection.TryGetValue("DataBackgroundColor", out value))
			{
				repRowCell.BackGround = (Color)value;
			}
		}
		if (!repRowCell.StyleCollection.ContainsKey("BackgroundColor"))
		{
			repRowCell.BackGround = Colors.Transparent;
		}
		if (!repRowCell.StyleCollection.ContainsKey("ForeGroundColor"))
		{
			if (repRow.IsTableCaption && repColumn.StyleCollection.TryGetValue("CaptionForegroundColor", out value))
			{
				repRowCell.ForeGround = (Color)value;
			}
			else if (repColumn.StyleCollection.TryGetValue("DataForegroundColor", out value))
			{
				repRowCell.ForeGround = (Color)value;
			}
		}
		if (!repRowCell.StyleCollection.ContainsKey("ForeGroundColor"))
		{
			repRowCell.ForeGround = Settings.GetForegroundColor();
		}
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
				repRowCell.FontStyle = (FontStyle)value2;
			}
			else
			{
				repRowCell.FontStyle = (FontStyle)DefaultRowFormat(repRow, "FontStyle");
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

	private void Header_ContextMenuOpening(object sender, ContextMenuEventArgs e)
	{
		e.Handled = true;
	}

	private void Header_PreviewKeyDown(object sender, KeyEventArgs e)
	{
		e.Handled = true;
	}

	private void ShowStatus(object sender, PropertyChangedEventArgs e)
	{
		if (e.PropertyName == "ActiveRowChanged")
		{
			if (RepRows.Count == 0)
			{
				return;
			}
			if (!IsFloating)
			{
				Settings.HostingEnvironment.StatusBarSelectedRows(string.Format(ArmSoft.SME.Common.Resources.StatusSelectedRows, SelectedRows.Count));
				Settings.HostingEnvironment.StatusBarCurrentRow(string.Format(ArmSoft.SME.Common.Resources.StatusRows, ActiveRow.ActiveRow + 1, RepRows.Count));
			}
			else
			{
				_statusBarSelectedRows.Content = string.Format(ArmSoft.SME.Common.Resources.StatusSelectedRows, SelectedRows.Count);
				_statusBarCurrentRow.Content = string.Format(ArmSoft.SME.Common.Resources.StatusRows, ActiveRow.ActiveRow + 1, RepRows.Count);
			}
			if (ActiveRow.OldRowValue >= 0)
			{
				object rowByIndex = GetRowByIndex(ActiveRow.OldRowValue);
				if (rowByIndex != null && rowByIndex != sender)
				{
					FillColor(rowByIndex, deselect: true);
					SetBullet(rowByIndex, modeOn: false);
				}
			}
			FillColor(ActiveRow.ActiveRow);
		}
		else if (e.PropertyName == "SelectedCountChanged")
		{
			if (!IsFloating)
			{
				Settings.HostingEnvironment.StatusBarSelectedRows(string.Format(ArmSoft.SME.Common.Resources.StatusSelectedRows, SelectedRows.Count));
				Settings.HostingEnvironment.StatusBarCurrentRow(string.Format(ArmSoft.SME.Common.Resources.StatusRows, ActiveRow.ActiveRow + 1, RepRows.Count));
			}
			else
			{
				_statusBarSelectedRows.Content = string.Format(ArmSoft.SME.Common.Resources.StatusSelectedRows, SelectedRows.Count);
				_statusBarCurrentRow.Content = string.Format(ArmSoft.SME.Common.Resources.StatusRows, ActiveRow.ActiveRow + 1, RepRows.Count);
			}
		}
	}

	private static void SetBullet(object repRow, bool modeOn)
	{
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		if (!(repRow is System.Windows.Documents.TableRow tableRow))
		{
			return;
		}
		BlockUIContainer blockUIContainer = (BlockUIContainer)tableRow.Cells[0].Blocks.ElementAt(0);
		StackPanel stackPanel = null;
		if (blockUIContainer != null && blockUIContainer.Child != null)
		{
			stackPanel = blockUIContainer.Child as StackPanel;
		}
		if (stackPanel == null)
		{
			return;
		}
		if (modeOn)
		{
			DXImage dXImage = new DXImage
			{
				Source = ImageService.GetImage("bullet-arrow-right2.svg"),
				Width = 8.0,
				Height = 10.0
			};
			RenderOptions.SetBitmapScalingMode((DependencyObject)(object)dXImage, BitmapScalingMode.HighQuality);
			dXImage.HorizontalAlignment = HorizontalAlignment.Left;
			dXImage.VerticalAlignment = VerticalAlignment.Center;
			dXImage.Stretch = Stretch.Uniform;
			dXImage.Margin = new Thickness(0.0, 1.0, 2.0, 0.0);
			if (stackPanel.Children.Count < 2)
			{
				stackPanel.Children.Add(dXImage);
			}
		}
		else if (stackPanel.Children.Count > 1)
		{
			stackPanel.Children.RemoveAt(1);
		}
	}

	public void Refresh()
	{
		if (!_myReport.AllowRefresh)
		{
			return;
		}
		if (IsSpreadSheetMode)
		{
			_myReport.UpdateData();
			SetBarStates();
			return;
		}
		_inRefresh = true;
		if (SelectedRows != null)
		{
			SelectedRows.Clear();
		}
		if (_coloredCells != null && _coloredCells.Count > 0)
		{
			_coloredCells.Clear();
			InitColorsTotals();
			TotalsGrid.RefreshData();
		}
		ActiveRow.ActiveCell = 0;
		TotalsGrid.Visibility = Visibility.Collapsed;
		TotalsPanel.Visibility = Visibility.Collapsed;
		ActiveRow.SelectedCount = 0;
		ScrollBar.Value = 0.0;
		ScrollBar.Maximum = RepRows.Count - 1;
		FillVisiblePart(0);
		_myReport.UpdateData();
		SetBarStates();
		if (ReportContextMenu == null)
		{
			PopupMenu popupMenu = (ReportContextMenu = new PopupMenu());
		}
		ReportContextMenu.ItemLinks.Clear();
	}

	public void ShowUpdated()
	{
		Caption = _myReport.Caption();
		ToolTip = _myReport.CaptionToolTip();
		if (IsSpreadSheetMode)
		{
			Show(null, isSpreadSheet: true);
			return;
		}
		if (RepRows.Count > 0)
		{
			FillVisiblePart(0);
		}
		ScrollBar.Maximum = RepRows.Count - 1;
		FillDetail();
		if (Chart == null)
		{
			ChartPanel.Visibility = Visibility.Collapsed;
			return;
		}
		ChartPanel.Content = Chart;
		ChartPanel.Visibility = Visibility.Visible;
	}

	public void Add()
	{
		_myReport.Add(ActiveRowInfo());
	}

	public void Edit()
	{
		_myReport.Edit(ActiveRowInfo());
	}

	public void View()
	{
		_myReport.View(ActiveRowInfo());
	}

	public void Delete()
	{
		_myReport.Delete(ActiveRowInfo());
	}

	public void CreateCopy()
	{
		_myReport.CreateCopy(ActiveRowInfo());
	}

	public void DocumentHistory()
	{
		_myReport.DocumentHistory(ActiveRowInfo());
	}

	private void CreateDoc(PrintMode mode)
	{
		List<Guid> list = new List<Guid>();
		if (SelectedRows.Count > 0)
		{
			foreach (object value in SelectedRows.Values)
			{
				if (value != null && value is IRowWithISN rowWithISN)
				{
					list.Add(rowWithISN.GetDocISN());
				}
			}
		}
		else
		{
			object obj = ActiveRowInfo();
			if (obj == null || !(obj is IRowWithISN rowWithISN2))
			{
				return;
			}
			list.Add(rowWithISN2.GetDocISN());
		}
		EnumerateErrorsReportViewer enumerateErrorsReportViewer = new EnumerateErrorsReportViewer(ArmSoft.SME.Common.Resources.PreviewErrorsReportTitle, "PrintPreview");
		Settings.ServiceProvider.GetRequiredService<ITemplateSubstitutionService>().PrintPreview(list, mode, null, insertPageBreaks: true, null, enumerateErrorsReportViewer);
		if (enumerateErrorsReportViewer.HasErrors)
		{
			enumerateErrorsReportViewer.Show();
		}
	}

	public void Preview()
	{
		CreateDoc(PrintMode.PrintPreview);
	}

	public void ExportExcel()
	{
		CreateDoc(PrintMode.ExportToExcel);
	}

	public void ExportPdf()
	{
		CreateDoc(PrintMode.ExportToPdf);
	}

	public void ExportWord()
	{
		CreateDoc(PrintMode.ExportToWord);
	}

	public void ExecDefaultFunction()
	{
		_myReport.ExecuteDefaultFunction();
	}

	public void ShowDialog()
	{
		if (_myReport.DialogSupported && _myReport.ShowDialog(useParamValues: true, isRefreshMode: true))
		{
			SetBarStates();
			Refresh();
			ShowUpdated();
		}
	}

	public bool DialogSupported()
	{
		if (_myReport == null)
		{
			return false;
		}
		return _myReport.DialogSupported;
	}

	public void SetFocus()
	{
		Keyboard.Focus(_myFlowDocument);
	}

	public int FindText(AsFindInfo searchInfo)
	{
		string text = searchInfo.Pattern.Replace("\\", "\\\\");
		if (searchInfo.WholeWord)
		{
			text = "\\b" + text + "\\b";
		}
		text = text.Replace(".", "\\.");
		int num = ActiveRow.ActiveRow;
		if (num < 0)
		{
			num = 0;
		}
		int num2 = -1;
		if (searchInfo.SearchDirection == FindDirection.Forward)
		{
			for (int i = num + 1; i <= RepRows.Count - 1; i++)
			{
				if (RepRows[i].FindText(text, searchInfo.IgnoreCase) >= 0)
				{
					num2 = i;
					break;
				}
			}
		}
		else
		{
			for (int num3 = num - 1; num3 >= 0; num3--)
			{
				if (RepRows[num3].FindText(text, searchInfo.IgnoreCase) >= 0)
				{
					num2 = num3;
					break;
				}
			}
		}
		if (num2 > -1)
		{
			NavigateToRowByIndex(num2);
		}
		else
		{
			MessageBox.Show(ArmSoft.SME.Common.Resources.NoFound, ArmSoft.SME.Common.Resources.Find);
		}
		return num2;
	}

	private TableCell CreateTableCell(int rowNum, AsRepRowCell cell, int cellNum)
	{
		TableCell tableCell = new TableCell
		{
			Name = "C" + cellNum,
			ColumnSpan = cell.ColumnSpan
		};
		if (RepRows[rowNum].RowType != AsRepRowType.Cells)
		{
			tableCell.BorderThickness = new Thickness(0.0);
			tableCell.BorderBrush = Brushes.Transparent;
		}
		else
		{
			tableCell.BorderThickness = new Thickness(0.5, 0.0, 0.5, 0.0);
			tableCell.BorderBrush = Brushes.Transparent;
		}
		tableCell.Background = new SolidColorBrush(cell.BackGround);
		if (_coloredCells != null && _coloredCells.Contains("R" + rowNum + "C" + cellNum))
		{
			tableCell.Background = new SolidColorBrush(cell.UserBackGround);
		}
		if (cell.HasCustomForeGround)
		{
			tableCell.Foreground = new SolidColorBrush(cell.ForeGround);
		}
		if (cell.Borders != null && cell.Borders.Count > 0)
		{
			if (cell.Borders.Contains(BorderType.AllBorders))
			{
				tableCell.BorderThickness = new Thickness(0.5);
				tableCell.BorderBrush = (Settings.IsDarkTheme ? Brushes.White : Brushes.Black);
			}
			else if (cell.Borders.Contains(BorderType.None))
			{
				tableCell.BorderThickness = new Thickness(0.5);
				tableCell.BorderBrush = Brushes.Transparent;
			}
			else
			{
				Thickness borderThickness = default;
				Thickness thickness = default;
				if (cell.Borders.Contains(BorderType.LeftBorder))
				{
					borderThickness.Left = 0.5;
					thickness.Left = -0.5;
				}
				else
				{
					borderThickness.Left = 0.0;
					thickness.Left = 0.5;
				}
				if (cell.Borders.Contains(BorderType.RightBorder))
				{
					borderThickness.Right = 0.5;
					thickness.Right = -0.5;
				}
				else
				{
					borderThickness.Right = 0.0;
					thickness.Right = 0.5;
				}
				if (cell.Borders.Contains(BorderType.TopBorder))
				{
					borderThickness.Top = 0.5;
					thickness.Top = 0.0;
				}
				else
				{
					borderThickness.Top = 0.0;
					thickness.Top = 0.0;
				}
				if (cell.Borders.Contains(BorderType.BottomBorder))
				{
					borderThickness.Bottom = 0.5;
					thickness.Bottom = 0.0;
				}
				else
				{
					borderThickness.Bottom = 0.0;
					thickness.Bottom = 0.0;
				}
				tableCell.BorderThickness = borderThickness;
				tableCell.BorderBrush = (Settings.IsDarkTheme ? Brushes.White : Brushes.Black);
			}
		}
		if (cell.ValueStyle == AsRepColStyle.BooleanStyle && (cell.Value.ToString() == "0" || cell.Value.ToString() == "1" || cell.Value.ToString() == "-1" || cell.Value.ToString() == "True" || cell.Value.ToString() == "False"))
		{
			CheckEdit uiElement = new CheckEdit
			{
				IsReadOnly = true,
				IsChecked = (bool?)cell.Value,
				VerticalAlignment = VerticalAlignment.Top
			};
			tableCell.Blocks.Add(new BlockUIContainer(uiElement));
		}
		else
		{
			Paragraph paragraph = new Paragraph();
			if (RepRows[rowNum].IsTableCaption)
			{
				paragraph.Padding = new Thickness(1.0, 3.0, 1.0, 3.0);
			}
			else
			{
				paragraph.Padding = new Thickness(2.0, 1.0, 2.0, 1.0);
			}
			paragraph.Name = "Cell" + cellNum;
			if (_coloredCells != null && _coloredCells.Contains("R" + rowNum + "C" + cellNum))
			{
				paragraph.Background = new SolidColorBrush(cell.UserBackGround);
			}
			else
			{
				paragraph.Background = new SolidColorBrush(cell.BackGround);
			}
			if (cell.ForeGround.ColorContext != null)
			{
				paragraph.Foreground = new SolidColorBrush(cell.ForeGround);
			}
			paragraph.FontSize = cell.FontSize;
			paragraph.FontWeight = cell.FontWeight;
			paragraph.FontStyle = cell.FontStyle;
			paragraph.TextAlignment = cell.Alignment;
			switch (cell.ValueStyle)
			{
			case AsRepColStyle.DateStyle:
				if (cell.Value is DateTime)
				{
					paragraph.Inlines.Add(new Run(Convert.ToDateTime(cell.Value).FormatDDMMYY()));
				}
				else
				{
					paragraph.Inlines.Add(new Run(cell.Value.ToString()));
				}
				break;
			case AsRepColStyle.CurrencyStyle:
				if (Convert.ToString(cell.Value).IsNumeric())
				{
					paragraph.Inlines.Add(new Run(Convert.ToDecimal(cell.Value).CurrencyFormat(cell.CurrencyLen, cell.CurrencyPrecision)));
					paragraph.TextAlignment = System.Windows.TextAlignment.Right;
				}
				else
				{
					paragraph.Inlines.Add(new Run(cell.Value.ToString()));
					paragraph.TextAlignment = cell.Alignment;
				}
				break;
			default:
				paragraph.Inlines.Add(new Run(cell.Value.ToString()));
				paragraph.TextAlignment = cell.Alignment;
				break;
			}
			tableCell.Blocks.Add(paragraph);
		}
		return tableCell;
	}

	private void SetActiveRow(object sender, RoutedEventArgs e)
	{
		int num = Convert.ToInt32(((System.Windows.Documents.TableRow)sender).Name.Mid(4));
		ActiveRow.ActiveRow = num;
		ScrollBar.Value = num;
	}

	private void SelectRow(object sender, RoutedEventArgs e)
	{
		int activeRow = Convert.ToInt32(((TableCell)sender).Name.Mid(5));
		ActiveRow.ActiveRow = activeRow;
		if (SelectedRows.ContainsKey(ActiveRow.ActiveRow))
		{
			SelectedRows.Remove(ActiveRow.ActiveRow);
			ActiveRow.SelectedCount--;
		}
		else
		{
			SelectedRows.Add(ActiveRow.ActiveRow, RepRows[ActiveRow.ActiveRow].HiddenData);
			ActiveRow.SelectedCount++;
		}
		FillColor(ActiveRow.ActiveRow);
	}

	public void SetSelectedColor()
	{
		TextSelection selection = ScrollView.Selection;
		if (selection == null || ScrollView.Selection.Start.Paragraph == null || ScrollView.Selection.End.GetNextInsertionPosition(LogicalDirection.Backward).Paragraph == null)
		{
			return;
		}
		TableCell tableCell = (TableCell)(object)ScrollView.Selection.Start.Paragraph.Parent;
		TableCell tableCell2 = (TableCell)(object)ScrollView.Selection.End.GetNextInsertionPosition(LogicalDirection.Backward).Paragraph.Parent;
		string text = ((System.Windows.Documents.TableRow)(object)tableCell.Parent).Name.Mid(4);
		if (!text.IsNumeric())
		{
			return;
		}
		string text2 = ((System.Windows.Documents.TableRow)(object)tableCell2.Parent).Name.Mid(4);
		if (!text2.IsNumeric())
		{
			return;
		}
		List<TableCell> selectedCells = GetSelectedCells(selection, Convert.ToInt32(text), Convert.ToInt32(text2), tableCell2.Name);
		if (selectedCells.Count == 0 && _activeCell is TableCell)
		{
			TableCell tableCell3 = (TableCell)_activeCell;
			if (tableCell3.Name != null && tableCell3.Name != "")
			{
				selectedCells.Add((TableCell)_activeCell);
			}
		}
		foreach (TableCell item in selectedCells)
		{
			SetCellBackground(item);
		}
	}

	public List<TableCell> GetSelectedCells(TextSelection selection, int rowStart, int rowEnd, string lastCellName)
	{
		List<TableCell> list = new List<TableCell>();
		for (int i = rowStart; i <= rowEnd; i++)
		{
			System.Windows.Documents.TableRow tableRow = (System.Windows.Documents.TableRow)(object)GetRowByIndex(i);
			if (tableRow == null)
			{
				continue;
			}
			foreach (TableCell item in (IEnumerable<TableCell>)tableRow.Cells)
			{
				if (selection.Contains(item.ContentEnd))
				{
					list.Add(item);
					if (i == rowEnd && item.Name == lastCellName)
					{
						break;
					}
				}
			}
		}
		return list;
	}

	public void SetCellBackground(TableCell cl = null)
	{
		Color key = default;
		if (cl == null)
		{
			cl = (TableCell)_activeCell;
		}
		TableCell tableCell = cl;
		int num = Convert.ToInt32(((System.Windows.Documents.TableRow)(object)cl.Parent).Name.Substring(3));
		int index = Convert.ToInt32(cl.Name.Substring(1));
		SolidColorBrush solidColorBrush = Settings.HostingEnvironment.gUserColor;
		if (solidColorBrush == null)
		{
			solidColorBrush = ((!ApplicationThemeHelper.ApplicationThemeName.Contains("Dark")) ? new SolidColorBrush(Colors.Yellow) : new SolidColorBrush(Colors.DarkRed));
		}
		if (tableCell == null)
		{
			return;
		}
		SolidColorBrush solidColorBrush2 = (SolidColorBrush)tableCell.Background;
		tableCell.Background = new SolidColorBrush(solidColorBrush.Color);
		if (_coloredCells == null)
		{
			_coloredCells = new ObservableCollection<string>();
		}
		_coloredCells.Add("R" + num + "C" + index);
		if (solidColorBrush2 != null)
		{
			key = Color.FromArgb(solidColorBrush2.Color.A, solidColorBrush2.Color.R, solidColorBrush2.Color.G, solidColorBrush2.Color.B);
		}
		if (num < RepRows.Count)
		{
			RepRows[num].RowCells[index].UserBackGround = solidColorBrush.Color;
		}
		string text = ((Run)((Paragraph)tableCell.Blocks.FirstOrDefault()).Inlines.FirstOrDefault()).Text;
		if (!decimal.TryParse(text, out var _))
		{
			return;
		}
		if (_totalsDct == null)
		{
			_totalsDct = new Dictionary<Color, int>();
			_cellsTotals = new List<TotalDetails>();
		}
		TotalsGrid.Visibility = Visibility.Visible;
		TotalsPanel.Visibility = Visibility.Visible;
		if (_totalsDct.TryGetValue(solidColorBrush.Color, out var value))
		{
			_cellsTotals[value].Sum += Convert.ToDecimal(text);
			_cellsTotals[value].Cnt++;
			_cellsTotals[value].Avg = (_cellsTotals[value].Sum / (decimal)_cellsTotals[value].Cnt).RoundAs(4);
			TotalsGrid.RefreshData();
		}
		else
		{
			TotalDetails totalDetails = new TotalDetails
			{
				CellColor = solidColorBrush.Color,
				Sum = Convert.ToDecimal(text),
				Cnt = 1
			};
			totalDetails.Avg = totalDetails.Sum;
			_cellsTotals.Add(totalDetails);
			_totalsDct.Add(solidColorBrush.Color, _cellsTotals.Count - 1);
			TotalsGrid.RefreshData();
		}
		if (!_totalsDct.TryGetValue(key, out value))
		{
			return;
		}
		TotalDetails totalDetails2 = _cellsTotals[value];
		totalDetails2.Sum -= Convert.ToDecimal(text);
		totalDetails2.Cnt--;
		if (totalDetails2.Cnt == 0)
		{
			_cellsTotals.RemoveAt(value);
			_totalsDct.Remove(key);
			Dictionary<Color, int>.KeyCollection keys = _totalsDct.Keys;
			int i = 0;
			for (int num2 = keys.Count - 1; i <= num2; i++)
			{
				if (_totalsDct[keys.ElementAtOrDefault(i)] > value)
				{
					_totalsDct[keys.ElementAtOrDefault(i)]--;
				}
			}
		}
		else
		{
			totalDetails2.Avg = (totalDetails2.Sum / (decimal)totalDetails2.Cnt).RoundAs(4);
		}
		TotalsGrid.RefreshData();
	}

	private void RestoreBackgroundAllCells()
	{
		foreach (string coloredCell in _coloredCells)
		{
			string[] array = coloredCell.Split("C");
			int index = Convert.ToInt32(array[0].Mid(2));
			int num = Convert.ToInt32(array[1]);
			if (GetRowByIndex(Convert.ToInt32(array[0].Mid(2))) != null)
			{
				((System.Windows.Documents.TableRow)(object)GetRowByIndex(Convert.ToInt32(array[0].Mid(2)))).Cells[num + 1].Background = new SolidColorBrush(RepRows[index].RowCells[num].BackGround);
			}
		}
	}

	public void ClearUserColorsActiveCell()
	{
		Color key = default;
		if (!(_activeCell is TableCell tableCell) || _coloredCells == null || !_coloredCells.Contains("R" + ActiveRow.ActiveRow + "C" + ActiveRow.ActiveCell))
		{
			return;
		}
		SolidColorBrush solidColorBrush = (SolidColorBrush)tableCell.Background;
		if (solidColorBrush != null)
		{
			key = Color.FromArgb(solidColorBrush.Color.A, solidColorBrush.Color.R, solidColorBrush.Color.G, solidColorBrush.Color.B);
		}
		tableCell.Background = new SolidColorBrush(RepRows[ActiveRow.ActiveRow].RowCells[ActiveRow.ActiveCell].BackGround);
		_coloredCells.Remove("R" + ActiveRow.ActiveRow + "C" + ActiveRow.ActiveCell);
		if (!decimal.TryParse(((Run)((Paragraph)tableCell.Blocks.ElementAt(0)).Inlines.ElementAt(0)).Text, out var result) || _totalsDct == null || !_totalsDct.TryGetValue(key, out var value))
		{
			return;
		}
		_cellsTotals[value].Sum -= result;
		_cellsTotals[value].Cnt--;
		if (_cellsTotals[value].Cnt == 0)
		{
			_cellsTotals.RemoveAt(value);
			_totalsDct.Remove(key);
			Dictionary<Color, int>.KeyCollection keys = _totalsDct.Keys;
			for (int i = 0; i <= keys.Count - 1; i++)
			{
				if (_totalsDct[keys.ElementAtOrDefault(i)] > value)
				{
					_totalsDct[keys.ElementAtOrDefault(i)]--;
				}
			}
		}
		else
		{
			_cellsTotals[value].Avg = (_cellsTotals[value].Sum / (decimal)_cellsTotals[value].Cnt).RoundAs(4);
		}
		TotalsGrid.RefreshData();
		if (_cellsTotals.Count == 0)
		{
			TotalsGrid.Visibility = Visibility.Collapsed;
			TotalsPanel.Visibility = Visibility.Collapsed;
		}
	}

	public void ClearUserColorsAll()
	{
		if (_coloredCells != null && _coloredCells.Count > 0)
		{
			_coloredCells.Clear();
			InitColorsTotals();
			TotalsGrid.RefreshData();
		}
		TotalsPanel.Visibility = Visibility.Collapsed;
		TotalsGrid.Visibility = Visibility.Collapsed;
		ReDrawActivePage();
	}

	public void FillDetail()
	{
		if (_detailsDocument == null)
		{
			_detailsDocument = new FlowDocument();
		}
		_detailsDocument.Blocks.Clear();
		FlowDocumentScrollViewer detailsPanelSc = DetailsPanelSc;
		if (detailsPanelSc.Document == null)
		{
			FlowDocument flowDocument = (detailsPanelSc.Document = _detailsDocument);
		}
		Paragraph paragraph = new Paragraph
		{
			FontSize = Application.Current.MainWindow.FontSize
		};
		paragraph.Inlines.Add(new Run(ReportTitle()));
		_detailsDocument.Blocks.Add(paragraph);
	}

	public void InitColorsTotals()
	{
		GridControl totalsGrid = TotalsGrid;
		DataControlBase.AllowInfiniteGridSize = true;
		_totalsDct = new Dictionary<Color, int>();
		_cellsTotals = new List<TotalDetails>();
		totalsGrid.View.NavigationStyle = GridViewNavigationStyle.Cell;
		totalsGrid.ItemsSource = _cellsTotals;
		totalsGrid.Columns.Clear();
		GridColumn item = new GridColumn
		{
			Header = ArmSoft.SME.Common.Resources.Color,
			Name = "CellColor",
			FieldName = "CellColor",
			AllowEditing = DefaultBoolean.False,
			ReadOnly = true,
			UnboundType = UnboundColumnType.Object,
			CellTemplate = (DataTemplate)Resources["ColorCelltemplate"],
			Width = 60.0
		};
		totalsGrid.Columns.Add(item);
		item = new GridColumn
		{
			Header = ArmSoft.SME.Common.Resources.Summa,
			FieldName = "Sum",
			AllowEditing = DefaultBoolean.True,
			ReadOnly = true
		};
		CalcEditSettings editSettings = new CalcEditSettings
		{
			DisplayFormat = DecimalExtension.GetCurrencyFormat(16, 2),
			Precision = 4
		};
		item.EditSettings = editSettings;
		totalsGrid.Columns.Add(item);
		item = new GridColumn
		{
			Header = ArmSoft.SME.Common.Resources.RepCount,
			FieldName = "Cnt",
			AllowEditing = DefaultBoolean.True,
			ReadOnly = true
		};
		totalsGrid.Columns.Add(item);
		item = new GridColumn
		{
			Header = ArmSoft.SME.Common.Resources.RepAvg,
			FieldName = "Avg",
			AllowEditing = DefaultBoolean.True,
			ReadOnly = true
		};
		editSettings = new CalcEditSettings
		{
			DisplayFormat = DecimalExtension.GetCurrencyFormat(16, 4),
			Precision = 4
		};
		item.EditSettings = editSettings;
		totalsGrid.Columns.Add(item);
		item = new GridColumn
		{
			Header = ArmSoft.SME.Common.Resources.Comment,
			FieldName = "Comment",
			AllowEditing = DefaultBoolean.True,
			ReadOnly = false,
			Width = 200.0
		};
		totalsGrid.Columns.Add(item);
		TableView view = new TableView
		{
			SearchPanelPosition = SearchPanelPosition.OverGroupPanel,
			SearchPanelParseMode = SearchPanelParseMode.Mixed,
			ShowGroupPanel = false
		};
		totalsGrid.View = view;
		totalsGrid.Visibility = Visibility.Collapsed;
	}

	public bool AllowAdd()
	{
		return _myReport.AllowAdd;
	}

	public bool AllowEdit()
	{
		return _myReport.AllowEdit;
	}

	public bool AllowView()
	{
		return _myReport.AllowView;
	}

	public bool AllowDelete()
	{
		return _myReport.AllowDelete;
	}

	public bool AllowPreview()
	{
		return _myReport.AllowPreview;
	}

	public bool AllowDocumentHistory()
	{
		return _myReport.AllowDocumentHistory;
	}

	public bool AllowCreateCopy()
	{
		return _myReport.AllowCreateCopy;
	}

	private void Footer_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
	{
		e.Handled = true;
	}

	private void Header_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
	{
		e.Handled = true;
	}

	private void Footer_ContextMenuOpening(object sender, ContextMenuEventArgs e)
	{
		e.Handled = true;
	}

	private void BarButtonHelp_ItemClick(object sender, ItemClickEventArgs e)
	{
		try
		{
			Settings.HostingEnvironment.ShowHelp(_myReport.HelpId);
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	private void MainLayoutManager_DockItemClosing(object sender, ItemCancelEventArgs e)
	{
		e.Item.Visibility = Visibility.Collapsed;
		e.Cancel = true;
		if (e.Item == DetailsPanel)
		{
			_chkReportDetails.IsChecked = false;
		}
	}

	private void AsReportViewer_SizeChanged(object sender, SizeChangedEventArgs e)
	{
		if (IsFloating)
		{
			RibbonStatusBar.Visibility = Visibility.Visible;
		}
	}

	private void AsReportViewer_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
	{
		if (_inRefresh)
		{
			Keyboard.Focus(_myFlowDocument);
			FocusManager.SetFocusedElement((DependencyObject)(object)this, _myFlowDocument);
			_inRefresh = false;
		}
		e.Handled = true;
	}

	private void DetailsPanel_PreviewKeyDown(object sender, KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Invalid comparison between Unknown and I4
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Invalid comparison between Unknown and I4
		if ((int)e.Key == 49 && (int)Keyboard.Modifiers == 2)
		{
			e.Handled = true;
		}
	}

	private void ShowColorEdit(object sender, RoutedEventArgs e)
	{
		AsColors asColors = new AsColors();
		SolidColorBrush gUserColor = Settings.HostingEnvironment.gUserColor;
		if (gUserColor == null)
		{
			asColors.ColorChooser.Color = asColors.ColorChooser.DefaultColor;
		}
		else
		{
			asColors.ColorChooser.Color = gUserColor.Color;
		}
		if (asColors.ShowDialog().Value && gUserColor != null)
		{
			gUserColor.Color = asColors.ColorChooser.Color;
		}
	}

	private void ClearUserColors(object sender, RoutedEventArgs e)
	{
		if (Settings.HostingEnvironment.CurrentPanel is AsReportViewer asReportViewer)
		{
			asReportViewer.ClearUserColorsAll();
		}
	}

	private void DefaultColorCell(object sender, RoutedEventArgs e)
	{
		if (Settings.HostingEnvironment.CurrentPanel is AsReportViewer asReportViewer)
		{
			asReportViewer.ClearUserColorsActiveCell();
		}
	}

	private void SelectCell(object sender, MouseButtonEventArgs e)
	{
		TableCell tableCell = (TableCell)(_activeCell = (TableCell)sender);
		ActiveRow.ActiveCell = Convert.ToInt32(tableCell.Name.Mid(2));
	}

	protected override void ScV_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
	{
		ExecDefaultFunction();
		e.Handled = true;
	}

	[DebuggerNonUserCode]
	[GeneratedCode("PresentationBuildTasks", "10.0.9.0")]
	internal Delegate _CreateDelegate(Type delegateType, string handler)
	{
		return Delegate.CreateDelegate(delegateType, this, handler);
	}
}
