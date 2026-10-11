using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Threading;
using System.Xml;
using System.Xml.Linq;
using ArmSoft.SME.Client;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Models.Exchange;
using ArmSoft.SME.Core.UI.HelpClasses;
using DevExpress.Xpf.Bars;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.Ribbon;
using DevExpress.XtraEditors.DXErrorProvider;
using Microsoft.Win32;

namespace ArmSoft.SME.Core.UI.Exchange;

public partial class ImportWindow : ThemedWindow, IComponentConnector
{
	private class SummarizedContentRow
	{
		public string TypeName { get; set; }

		public string TypeCaption { get; set; }
	}

	private class ContentRow : ParseResult, IDXDataErrorInfo
	{
		public int Number { get; set; }

		public RowState State { get; set; }

		public string StateName => State switch
		{
			RowState.ErrorWord => ArmSoft.SME.Common.Resources.ErrorWord, 
			RowState.Imported => ArmSoft.SME.Common.Resources.Imported, 
			RowState.NotProcessed => ArmSoft.SME.Common.Resources.NotProcessed, 
			_ => "", 
		};

		public string TypeName => Caption;

		public ContentRow(ParseResult pr, int number)
		{
			EntityXML = pr.EntityXML;
			EntityName = pr.EntityName;
			Caption = pr.Caption;
			LineNumber = pr.LineNumber;
			LinePosition = pr.LinePosition;
			Number = number;
			State = RowState.NotProcessed;
		}

		public void GetError(ErrorInfo info)
		{
			if (State == RowState.ErrorWord)
			{
				info.ErrorType = ErrorType.Critical;
				info.ErrorText = ArmSoft.SME.Common.Resources.ErrorWord;
			}
		}

		public void GetPropertyError(string propertyName, ErrorInfo info)
		{
		}
	}

	private enum RowState
	{
		NotProcessed,
		Imported,
		ErrorWord,
		Unknown
	}

	private enum State
	{
		Wait,
		ReadyForImport,
		Importing
	}

	private State _state;

	private ObservableCollection<SummarizedContentRow> _summarizedContent;

	private ObservableCollection<ContentRow> _fileContent;

	private Dictionary<int, ContentRow> _selectedRows;

	private readonly BackgroundWorker _worker;

	private int _importedCount;

	private int _importErrorsCount;

	private const double _defaultWidth = 870.0;

	private const double _defaultHeight = 560.0;

	private bool _restoringSettings;

	private bool _ribbonToolbarShowAbove = true;

	private bool _isLogoutConnectionWhenClosing = true;

	private ImportMode _docImportMode;

	public bool SetLogoutConnectionWhenClosing
	{
		set
		{
			_isLogoutConnectionWhenClosing = value;
		}
	}

	public ImportWindow()
	{
		InitializeComponent();
		if (Settings.IsWin11Theme)
		{
			SummarizedSelected.CellTemplate = (DataTemplate)Resources["GridCellWin11Template"];
			SelectedColumn.CellTemplate = (DataTemplate)Resources["GridCellWin11Template"];
			SummarizedContentGrid.Margin = new Thickness(2.0);
			FileContentGrid.Margin = new Thickness(2.0);
		}
		else
		{
			SummarizedSelected.CellTemplate = (DataTemplate)Resources["GridCellTemplate"];
			SelectedColumn.CellTemplate = (DataTemplate)Resources["GridCellTemplate"];
		}
		BarButtonOpen.ItemClick += new SafeEventHandler<ItemClickEventArgs>(BarButtonOpen_ItemClick).Handler;
		BarButtonClose.ItemClick += new SafeEventHandler<ItemClickEventArgs>(BarButtonClose_ItemClick).Handler;
		BarButtonImport.ItemClick += new SafeEventHandler<ItemClickEventArgs>(BarButtonImport_ItemClick).Handler;
		BarButtonSave.ItemClick += new SafeEventHandler<ItemClickEventArgs>(BarButtonSave_ItemClick).Handler;
		BarButtonSelectAll.ItemClick += new SafeEventHandler<ItemClickEventArgs>(BarButtonSelectAll_ItemClick).Handler;
		BarButtonUnselectAll.ItemClick += new SafeEventHandler<ItemClickEventArgs>(BarButtonUnselectAll_ItemClick).Handler;
		BarButtonHelp.ItemClick += new SafeEventHandler<ItemClickEventArgs>(BarButtonHelp_ItemClick).Handler;
		BarCheckItemPreserveState.CheckedChanged += new SafeEventHandler<ItemClickEventArgs>(BarCheckItemPreserveState_CheckedChanged).Handler;
		BarCheckItemRegistredMakeDraft.CheckedChanged += new SafeEventHandler<ItemClickEventArgs>(BarCheckItemRegistredMakeDraft_CheckedChanged).Handler;
		BarCheckItemReregisterAll.CheckedChanged += new SafeEventHandler<ItemClickEventArgs>(BarCheckItemReregisterAll_CheckedChanged).Handler;
		Ribbon.SizeChanged += new SafeEventHandler<SizeChangedEventArgs>(Ribbon_SizeChanged).Handler;
		FileContentGrid.CustomUnboundColumnData += new SafeEventHandler<GridColumnDataEventArgs>(FileContentGrid_CustomUnboundColumnData).Handler;
		FileContentGridView.KeyDown += new SafeEventHandler<KeyEventArgs>(FileContentGridView_KeyDown).Handler;
		SummarizedContentGrid.KeyDown += new SafeEventHandler<KeyEventArgs>(SummarizedContentGrid_KeyDown).Handler;
		FileContentGridView.FocusedRowHandleChanged += new SafeEventHandler<FocusedRowHandleChangedEventArgs>(FileContentGridView_FocusedRowHandleChanged).Handler;
		SummarizedContentGrid.CustomUnboundColumnData += new SafeEventHandler<GridColumnDataEventArgs>(SummarizedContentGrid_CustomUnboundColumnData).Handler;
		SummarizedContentGridView.FocusedRowHandleChanged += new SafeEventHandler<FocusedRowHandleChangedEventArgs>(SummarizedContentGridView_FocusedRowHandleChanged).Handler;
		FileContentGridView.GotFocus += new SafeEventHandler<RoutedEventArgs>(GridViewGotFocus).Handler;
		SummarizedContentGridView.GotFocus += new SafeEventHandler<RoutedEventArgs>(GridViewGotFocus).Handler;
		FileContentGridView.LostFocus += new SafeEventHandler<RoutedEventArgs>(GridViewLostFocus).Handler;
		SummarizedContentGridView.LostFocus += new SafeEventHandler<RoutedEventArgs>(GridViewLostFocus).Handler;
		_worker = new BackgroundWorker();
		_worker.RunWorkerCompleted += Worker_RunWorkerCompleted;
		_worker.DoWork += Worker_DoWork;
		SetButtons();
		RestoreSettings();
		Activated += new SafeEventHandler<EventArgs>(ImportWindow_Activated).Handler;
		Loaded += new SafeEventHandler<RoutedEventArgs>(ImportWindow_Loaded).Handler;
		Closing += new SafeEventHandler<CancelEventArgs>(ImportWindow_Closing).Handler;
	}

	private void ImportWindow_Activated(object sender, EventArgs e)
	{
		FontFamily = Settings.FontFamily;
		FontSize = Settings.GetFontSize();
		SummarizedTab.Focus();
	}

	private void ImportWindow_Loaded(object sender, RoutedEventArgs e)
	{
		Title = ArmSoft.SME.Common.Resources.Import;
		RibbonPageMain.Caption = ArmSoft.SME.Common.Resources.Home;
		RibbonPageOptions.Caption = ArmSoft.SME.Common.Resources.Options;
		RibbonPageImportFile.Caption = ArmSoft.SME.Common.Resources.File;
		RibbonPageSelectedRows.Caption = ArmSoft.SME.Common.Resources.SelectedRows;
		RibbonPageList.Caption = ArmSoft.SME.Common.Resources.AllRows;
		RibbonPageDocImportMode.Caption = ArmSoft.SME.Common.Resources.DocumentsImportMode;
		BarButtonOpen.Content = ArmSoft.SME.Common.Resources.Open;
		BarButtonClose.Content = ArmSoft.SME.Common.Resources.Close;
		BarButtonImport.Content = ArmSoft.SME.Common.Resources.Import;
		BarButtonSave.Content = ArmSoft.SME.Common.Resources.Save;
		BarButtonSelectAll.Content = ArmSoft.SME.Common.Resources.SelectWord;
		BarButtonUnselectAll.Content = ArmSoft.SME.Common.Resources.Deselect;
		BarCheckItemPreserveState.Content = ArmSoft.SME.Common.Resources.PreserveState;
		BarCheckItemReregisterAll.Content = ArmSoft.SME.Common.Resources.ReregisterAll;
		BarCheckItemRegistredMakeDraft.Content = ArmSoft.SME.Common.Resources.RegistredMakeDraft;
		BarCheckItemRegistredMakeDraft.IsVisible = Settings.SystemType != SystemTypes.MarketPlace;
		BarEditItemImportProgress.Content = ArmSoft.SME.Common.Resources.ImportProgress;
		SummarizedTab.Header = ArmSoft.SME.Common.Resources.Summarized;
		DetailedTab.Header = ArmSoft.SME.Common.Resources.Details;
		LGDetailedView.Header = ArmSoft.SME.Common.Resources.ImportedData;
		LGErrorView.Header = ArmSoft.SME.Common.Resources.ErrorWord;
		RibbonPageHelp.Caption = ArmSoft.SME.Common.Resources.Help;
		BarButtonHelp.Content = ArmSoft.SME.Common.Resources.Help;
		TypeCaptionColumn.Header = ArmSoft.SME.Common.Resources.ImportDataType;
		CountColumn.Header = ArmSoft.SME.Common.Resources.Quantity;
		CountSelectedColumn.Header = ArmSoft.SME.Common.Resources.SelectedRows;
		ImportedColumn.Header = ArmSoft.SME.Common.Resources.Imported;
		ErrorsColumn.Header = ArmSoft.SME.Common.Resources.ImportErrorsCount;
		NumberColumn.Header = ArmSoft.SME.Common.Resources.Number;
		CaptionColumn.Header = ArmSoft.SME.Common.Resources.Name;
		LineNumberColumn.Header = ArmSoft.SME.Common.Resources.LineInFile;
		LinePositionColumn.Header = ArmSoft.SME.Common.Resources.PositionInRow;
		StateColumn.Header = ArmSoft.SME.Common.Resources.State;
		ErrorMessageColumn.Header = ArmSoft.SME.Common.Resources.ErrorMessage;
		BarCheckItemPreserveState.IsChecked = true;
		_summarizedContent = new ObservableCollection<SummarizedContentRow>();
		SummarizedContentGrid.ItemsSource = _summarizedContent;
		_fileContent = new ObservableCollection<ContentRow>();
		FileContentGrid.ItemsSource = _fileContent;
		SummarizedContentGridView.AllowMoveColumnToDropArea = false;
		SummarizedContentGridView.IsColumnMenuEnabled = false;
		FileContentGridView.AllowMoveColumnToDropArea = false;
		FileContentGridView.IsColumnMenuEnabled = false;
		_selectedRows = new Dictionary<int, ContentRow>();
		SetButtons();
		RestoreSettings();
	}

	private void ImportWindow_Closing(object sender, CancelEventArgs e)
	{
		if (_state == State.Importing)
		{
			e.Cancel = true;
		}
		if (!e.Cancel)
		{
			StoreSettings();
		}
	}

	private void SetButtons()
	{
		if (_state == State.Wait)
		{
			BarButtonOpen.IsEnabled = true;
			BarButtonClose.IsEnabled = false;
			BarButtonImport.IsEnabled = false;
			BarButtonSave.IsEnabled = false;
			BarButtonSelectAll.IsEnabled = false;
			BarButtonUnselectAll.IsEnabled = false;
			BarCheckItemPreserveState.IsEnabled = true;
			BarCheckItemReregisterAll.IsEnabled = true;
			BarCheckItemRegistredMakeDraft.IsEnabled = true;
			BarEditItemImportProgress.IsVisible = false;
		}
		else if (_state == State.ReadyForImport)
		{
			BarButtonOpen.IsEnabled = true;
			BarButtonClose.IsEnabled = true;
			BarButtonImport.IsEnabled = _selectedRows.Count > 0;
			BarButtonSave.IsEnabled = _selectedRows.Count > 0;
			BarButtonSelectAll.IsEnabled = _fileContent.Count > 0;
			BarButtonUnselectAll.IsEnabled = _fileContent.Count > 0;
			BarCheckItemPreserveState.IsEnabled = true;
			BarCheckItemReregisterAll.IsEnabled = true;
			BarCheckItemRegistredMakeDraft.IsEnabled = true;
			BarEditItemImportProgress.IsVisible = false;
		}
		else if (_state == State.Importing)
		{
			BarButtonOpen.IsEnabled = false;
			BarButtonClose.IsEnabled = false;
			BarButtonImport.IsEnabled = false;
			BarButtonSave.IsEnabled = false;
			BarButtonSelectAll.IsEnabled = false;
			BarButtonUnselectAll.IsEnabled = false;
			BarCheckItemPreserveState.IsEnabled = false;
			BarCheckItemReregisterAll.IsEnabled = false;
			BarCheckItemRegistredMakeDraft.IsEnabled = false;
			BarEditItemImportProgress.IsVisible = true;
		}
	}

	private void Summarize()
	{
		_summarizedContent.Clear();
		foreach (ContentRow item in _fileContent)
		{
			string typeName = item.TypeName;
			SummarizedContentRow summarizedContentRow = null;
			foreach (SummarizedContentRow item2 in _summarizedContent)
			{
				if (item2.TypeName == typeName)
				{
					summarizedContentRow = item2;
				}
			}
			if (summarizedContentRow == null)
			{
				summarizedContentRow = new SummarizedContentRow
				{
					TypeName = typeName,
					TypeCaption = item.Caption
				};
				_summarizedContent.Add(summarizedContentRow);
			}
		}
	}

	private void SetSummarizedSelected(string typeName, bool value)
	{
		for (int i = 0; i <= _fileContent.Count - 1; i++)
		{
			ContentRow contentRow = _fileContent[i];
			if (contentRow.TypeName == typeName)
			{
				Set_Selected(contentRow, value);
				int rowHandleByListIndex = FileContentGrid.GetRowHandleByListIndex(i);
				FileContentGrid.RefreshRow(rowHandleByListIndex);
			}
		}
		SummarizedContentGridRefreshRow(typeName);
	}

	private void SumByTypeName(string typeName, ref int totalCount, ref int selectedCount, ref int importedCount, ref int errorsCount)
	{
		totalCount = 0;
		selectedCount = 0;
		importedCount = 0;
		errorsCount = 0;
		foreach (ContentRow item in _fileContent)
		{
			if (item.TypeName == typeName)
			{
				totalCount++;
				if (Get_Selected(item))
				{
					selectedCount++;
				}
				if (item.State == RowState.Imported)
				{
					importedCount++;
				}
				else if (item.State == RowState.ErrorWord)
				{
					errorsCount++;
				}
			}
		}
	}

	private void RowChanged(TableView view)
	{
		int focusedRowHandle = view.FocusedRowHandle;
		GridControl grid = view.Grid;
		string content = ((!CommonEventHandlers.IsDataRow(focusedRowHandle, grid)) ? string.Format(ArmSoft.SME.Common.Resources.StatusRows, 0, grid.VisibleRowCount) : string.Format(ArmSoft.SME.Common.Resources.StatusRows, grid.GetRowVisibleIndexByHandle(focusedRowHandle) + 1, grid.VisibleRowCount));
		StatusBarCurrentRow.Content = content;
	}

	private void AllDetailed(bool op)
	{
		for (int i = 0; i <= FileContentGrid.VisibleRowCount - 1; i++)
		{
			int rowHandleByVisibleIndex = FileContentGrid.GetRowHandleByVisibleIndex(i);
			ContentRow row = (ContentRow)FileContentGrid.GetRow(rowHandleByVisibleIndex);
			Set_Selected(row, op);
		}
		FileContentGrid.RefreshData();
		SummarizedContentGrid.RefreshData();
		StatusBarSelected.Content = _selectedRows.Count;
	}

	private bool Get_Selected(ContentRow row)
	{
		return _selectedRows.ContainsKey(row.Number);
	}

	private void Set_Selected(ContentRow row, bool value)
	{
		if (value)
		{
			_selectedRows.TryAdd(row.Number, row);
		}
		else if (_selectedRows.ContainsKey(row.Number))
		{
			_selectedRows.Remove(row.Number);
		}
	}

	private void SummarizedContentGridRefreshRow(string typeName)
	{
		for (int i = 0; i <= _summarizedContent.Count - 1; i++)
		{
			if (_summarizedContent[i].TypeName == typeName)
			{
				int rowHandleByListIndex = SummarizedContentGrid.GetRowHandleByListIndex(i);
				SummarizedContentGrid.RefreshRow(rowHandleByListIndex);
				break;
			}
		}
	}

	private void RefreshDetails()
	{
		if (FileContentGridView.Grid.CurrentItem != null)
		{
			ContentRow contentRow = (ContentRow)FileContentGridView.Grid.CurrentItem;
			TEDetailedView.EditValue = FormatXml(contentRow.EntityXML);
			TEErrorView.EditValue = contentRow.ErrorMessage;
		}
		else
		{
			TEDetailedView.EditValue = "";
			TEErrorView.EditValue = "";
		}
		LGErrorView.Visibility = (string.IsNullOrEmpty((string)TEErrorView.EditValue) ? Visibility.Collapsed : Visibility.Visible);
	}

	protected void RestoreSettings()
	{
		bool flag = false;
		try
		{
			_restoringSettings = true;
			using RegistryKey registryKey = Registry.CurrentUser.OpenSubKey(Settings.RegistryKey + "\\ImportWindow", writable: false);
			if (registryKey == null)
			{
				flag = true;
			}
			else
			{
				if (double.TryParse(registryKey.GetValue("Width", 870.0).ToString(), out var result) && result > 0.0)
				{
					Width = result;
				}
				if (double.TryParse(registryKey.GetValue("Height", 560.0).ToString(), out var result2) && result2 > 0.0)
				{
					Height = result2;
				}
				bool flag2 = registryKey.GetValue("RibbonToolbarMode", "1").ToString() == "1";
				if (registryKey.GetValue("State", "1").ToString() == "0")
				{
					WindowState = WindowState.Maximized;
				}
				else
				{
					WindowState = WindowState.Normal;
				}
				Ribbon.ToolbarShowMode = ((!flag2) ? RibbonQuickAccessToolbarShowMode.ShowBelow : RibbonQuickAccessToolbarShowMode.Default);
			}
		}
		catch (Exception)
		{
			flag = true;
		}
		finally
		{
			_restoringSettings = false;
		}
		if (flag)
		{
			_restoringSettings = true;
			Width = SizeCalculator.Width(870.0);
			Height = SizeCalculator.Height(560.0);
			Ribbon.ToolbarShowMode = RibbonQuickAccessToolbarShowMode.Default;
			Ribbon.IsMinimized = true;
			_ribbonToolbarShowAbove = Ribbon.ToolbarShowMode == RibbonQuickAccessToolbarShowMode.Default;
			_restoringSettings = false;
		}
	}

	protected void StoreSettings()
	{
		try
		{
			using RegistryKey registryKey = Registry.CurrentUser.CreateSubKey(Settings.RegistryKey + "\\ImportWindow");
			if (registryKey != null)
			{
				if (WindowState == WindowState.Normal)
				{
					registryKey.SetValue("Width", Width.ToString());
					registryKey.SetValue("Height", Height.ToString());
					registryKey.SetValue("State", "1");
				}
				else if (WindowState == WindowState.Maximized)
				{
					registryKey.SetValue("State", "0");
				}
				registryKey.SetValue("RibbonMinimized", Ribbon.IsMinimized ? "1" : "0");
				registryKey.SetValue("RibbonToolbarMode", (Ribbon.ToolbarShowMode == RibbonQuickAccessToolbarShowMode.Default) ? "1" : "0");
			}
		}
		catch (Exception)
		{
		}
	}

	private void BarButtonOpen_ItemClick(object sender, ItemClickEventArgs e)
	{
		try
		{
			OpenFileDialog openFileDialog = new OpenFileDialog
			{
				InitialDirectory = RegistryHelper.ReadImportDirectory(),
				DefaultExt = ".xml",
				Filter = "XML Files (.xml)|*.xml"
			};
			if (!openFileDialog.ShowDialog().Value)
			{
				return;
			}
			Mouse.OverrideCursor = Cursors.Wait;
			string fileName = openFileDialog.FileName;
			RegistryHelper.WriteImportDirectory(Path.GetDirectoryName(fileName));
			List<ParseResult> list = ExchangeHelper.Parse(fileName);
			_fileContent.Clear();
			int num = 0;
			foreach (ParseResult item in list)
			{
				num++;
				_fileContent.Add(new ContentRow(item, num));
			}
			Summarize();
			StatusBarCurrentFile.Content = fileName;
			_selectedRows.Clear();
			StatusBarSelected.Content = _selectedRows.Count;
			if (FileContentGrid.VisibleRowCount > 0)
			{
				int rowHandleByVisibleIndex = FileContentGrid.GetRowHandleByVisibleIndex(0);
				FileContentGridView.Grid.CurrentItem = FileContentGrid.GetRow(rowHandleByVisibleIndex);
			}
			Mouse.OverrideCursor = null;
			_state = State.ReadyForImport;
		}
		finally
		{
			SetButtons();
		}
	}

	private void BarButtonClose_ItemClick(object sender, ItemClickEventArgs e)
	{
		try
		{
			StatusBarCurrentFile.Content = "";
			_state = State.Wait;
			_summarizedContent.Clear();
			_fileContent.Clear();
			_selectedRows.Clear();
			StatusBarSelected.Content = _selectedRows.Count;
		}
		finally
		{
			SetButtons();
		}
	}

	private void BarButtonSelectAll_ItemClick(object sender, ItemClickEventArgs e)
	{
		try
		{
			AllDetailed(op: true);
		}
		finally
		{
			SetButtons();
		}
	}

	private void BarButtonUnselectAll_ItemClick(object sender, ItemClickEventArgs e)
	{
		try
		{
			AllDetailed(op: false);
		}
		finally
		{
			SetButtons();
		}
	}

	private void BarButtonHelp_ItemClick(object sender, ItemClickEventArgs e)
	{
		Settings.HostingEnvironment.ShowHelp("AdminTools_ImportData");
	}

	private void BarButtonImport_ItemClick(object sender, ItemClickEventArgs e)
	{
		try
		{
			Mouse.OverrideCursor = Cursors.Wait;
			List<ContentRow> list = new List<ContentRow>();
			for (int i = 0; i <= _fileContent.Count - 1; i++)
			{
				ContentRow contentRow = _fileContent[i];
				if (Get_Selected(contentRow))
				{
					list.Add(contentRow);
				}
			}
			_state = State.Importing;
			SetButtons();
			if (BarCheckItemPreserveState.IsChecked.Value)
			{
				_docImportMode = ImportMode.PreserveState;
			}
			else if (BarCheckItemReregisterAll.IsChecked.Value)
			{
				_docImportMode = ImportMode.MakeRegistered;
			}
			else
			{
				_docImportMode = ImportMode.MakeDraft;
			}
			_worker.RunWorkerAsync(list);
		}
		finally
		{
			SetButtons();
		}
	}

	private void Worker_DoWork(object sender, DoWorkEventArgs e)
	{
		LanguageService.RestoreCulture(Settings.GetCurrentCultureName());
		ApiClient apiClient = Settings.CreateApiClient();
		_importedCount = 0;
		_importErrorsCount = 0;
		List<ContentRow> list = (List<ContentRow>)e.Argument;
		int num = 0;
		foreach (ContentRow item in list)
		{
			item.State = RowState.NotProcessed;
			item.ErrorMessage = null;
			((DispatcherObject)this).Dispatcher.BeginInvoke((Delegate)new Action<ContentRow, int, int>(RowProcessed), (DispatcherPriority)5, new object[3] { item, list.Count, num });
		}
		foreach (ContentRow item2 in list)
		{
			try
			{
				ExchangeResponse exchangeResponse = apiClient.Exchange.Import(new ExchangeRequest(item2.EntityName, item2.EntityXML, _docImportMode));
				if (exchangeResponse.Success)
				{
					item2.State = RowState.Imported;
					item2.ErrorMessage = null;
					_importedCount++;
				}
				else
				{
					item2.ErrorMessage = exchangeResponse.Message;
					item2.State = RowState.ErrorWord;
					_importErrorsCount++;
				}
				num++;
			}
			catch (Exception ex)
			{
				item2.ErrorMessage = ex.Message;
				item2.State = RowState.ErrorWord;
				_importErrorsCount++;
			}
			finally
			{
				((DispatcherObject)this).Dispatcher.BeginInvoke((Delegate)new Action<ContentRow, int, int>(RowProcessed), (DispatcherPriority)5, new object[3] { item2, list.Count, num });
			}
		}
	}

	private void Worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
	{
		Mouse.OverrideCursor = null;
		if (_importErrorsCount == 0)
		{
			MessageBox.Show(ArmSoft.SME.Common.Resources.ImportSuccessfullyCompleted, Settings.ProgramName, MessageBoxButton.OK, MessageBoxImage.None, MessageBoxResult.None, this);
		}
		else
		{
			MessageBox.Show(string.Format(ArmSoft.SME.Common.Resources.ImportCompletedWithErrors, _importedCount, _importErrorsCount), Settings.ProgramName, MessageBoxButton.OK, MessageBoxImage.Hand, MessageBoxResult.None, this);
		}
		Settings.CacheFunctionsService.RefreshCaches();
		_state = State.ReadyForImport;
		SetButtons();
	}

	private void RowProcessed(ContentRow row, int total, int processed)
	{
		int rowHandleByListIndex = FileContentGrid.GetRowHandleByListIndex(row.Number - 1);
		BarEditItemImportProgress.EditValue = (double)processed / (double)total * 100.0;
		FileContentGrid.RefreshRow(rowHandleByListIndex);
		if (rowHandleByListIndex == FileContentGridView.FocusedRowHandle)
		{
			RefreshDetails();
		}
		SummarizedContentGridRefreshRow(row.TypeName);
	}

	private void BarButtonSave_ItemClick(object sender, ItemClickEventArgs e)
	{
		try
		{
			string fileNameFromExportDialog = Settings.CommonCoreHelper.GetFileNameFromExportDialog(".xml", "XML Files (.xml)|*.xml", "FromImportWindow_" + DateTime.Now.ToString("MMdd_HHmm"));
			if (!string.IsNullOrWhiteSpace(fileNameFromExportDialog))
			{
				XmlDocument xmlDocument = new XmlDocument();
				XmlElement xmlElement = xmlDocument.CreateElement("Exchange", "http://www.armsoft.am/Accountant/6.0");
				xmlDocument.AppendChild(xmlElement);
				foreach (ContentRow item in _fileContent)
				{
					if (Get_Selected(item))
					{
						XmlDocumentFragment xmlDocumentFragment = xmlDocument.CreateDocumentFragment();
						xmlDocumentFragment.InnerXml = FormatXml(item.EntityXML);
						xmlElement.AppendChild(xmlDocumentFragment);
						if (!string.IsNullOrWhiteSpace(item.ErrorMessage))
						{
							xmlElement.LastChild.InsertAfter(xmlDocument.CreateComment(item.ErrorMessage), xmlElement.LastChild);
						}
					}
				}
				xmlDocument.Save(fileNameFromExportDialog);
			}
			Mouse.OverrideCursor = null;
		}
		finally
		{
			SetButtons();
		}
	}

	private void BarCheckItemPreserveState_CheckedChanged(object sender, ItemClickEventArgs e)
	{
		if (BarCheckItemPreserveState.IsChecked.Value)
		{
			BarCheckItemRegistredMakeDraft.IsChecked = false;
			BarCheckItemReregisterAll.IsChecked = false;
		}
	}

	private void BarCheckItemRegistredMakeDraft_CheckedChanged(object sender, ItemClickEventArgs e)
	{
		if (BarCheckItemRegistredMakeDraft.IsChecked.Value)
		{
			BarCheckItemPreserveState.IsChecked = false;
			BarCheckItemReregisterAll.IsChecked = false;
		}
	}

	private void BarCheckItemReregisterAll_CheckedChanged(object sender, ItemClickEventArgs e)
	{
		if (BarCheckItemReregisterAll.IsChecked.Value)
		{
			BarCheckItemPreserveState.IsChecked = false;
			BarCheckItemRegistredMakeDraft.IsChecked = false;
		}
	}

	private void Ribbon_SizeChanged(object sender, SizeChangedEventArgs e)
	{
		if (!_restoringSettings)
		{
			if ((!_ribbonToolbarShowAbove) ? (Ribbon.ToolbarShowMode != RibbonQuickAccessToolbarShowMode.ShowBelow) : (Ribbon.ToolbarShowMode != RibbonQuickAccessToolbarShowMode.Default))
			{
				StoreSettings();
			}
			_ribbonToolbarShowAbove = Ribbon.ToolbarShowMode == RibbonQuickAccessToolbarShowMode.Default;
		}
	}

	private void FileContentGrid_CustomUnboundColumnData(object sender, GridColumnDataEventArgs e)
	{
		ContentRow contentRow = _fileContent[e.ListSourceRowIndex];
		if (e.IsGetData)
		{
			e.Value = Get_Selected(contentRow);
			SummarizedContentGridRefreshRow(contentRow.TypeName);
		}
		if (e.IsSetData && _state != State.Importing)
		{
			Set_Selected(contentRow, (bool)e.Value);
			StatusBarSelected.Content = _selectedRows.Count;
			SetButtons();
		}
	}

	private void FileContentGridView_FocusedRowHandleChanged(object sender, FocusedRowHandleChangedEventArgs e)
	{
		RefreshDetails();
		RowChanged((TableView)sender);
	}

	private void SummarizedContentGridView_FocusedRowHandleChanged(object sender, FocusedRowHandleChangedEventArgs e)
	{
		RowChanged((TableView)sender);
	}

	private void GridViewGotFocus(object sender, RoutedEventArgs e)
	{
		RowChanged((TableView)sender);
	}

	private void GridViewLostFocus(object sender, RoutedEventArgs e)
	{
		StatusBarCurrentRow.Content = "";
	}

	private void FileContentGridView_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Invalid comparison between Unknown and I4
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Invalid comparison between Unknown and I4
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a2: Invalid comparison between Unknown and I4
		//IL_012a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0134: Invalid comparison between Unknown and I4
		//IL_0137: Unknown result type (might be due to invalid IL or missing references)
		//IL_013e: Invalid comparison between Unknown and I4
		//IL_0154: Unknown result type (might be due to invalid IL or missing references)
		//IL_015e: Invalid comparison between Unknown and I4
		//IL_0161: Unknown result type (might be due to invalid IL or missing references)
		//IL_0168: Invalid comparison between Unknown and I4
		if (((int)e.Key == 31) | ((int)e.Key == 18))
		{
			int focusedRowHandle = FileContentGridView.FocusedRowHandle;
			ContentRow contentRow = (ContentRow)FileContentGridView.Grid.CurrentItem;
			if (contentRow != null)
			{
				Set_Selected(contentRow, !Get_Selected(contentRow));
				FileContentGrid.RefreshRow(focusedRowHandle);
				SummarizedContentGridRefreshRow(contentRow.TypeName);
				FileContentGridView.MoveNextRow();
				StatusBarSelected.Content = _selectedRows.Count;
			}
			e.Handled = true;
		}
		else if ((int)e.Key == 84)
		{
			for (int i = 0; i <= FileContentGrid.VisibleRowCount - 1; i++)
			{
				int focusedRowHandle = FileContentGrid.GetRowHandleByVisibleIndex(i);
				ContentRow contentRow = (ContentRow)FileContentGrid.GetRow(focusedRowHandle);
				Set_Selected(contentRow, !Get_Selected(contentRow));
			}
			FileContentGrid.RefreshData();
			SummarizedContentGrid.RefreshData();
			StatusBarSelected.Content = _selectedRows.Count;
			e.Handled = true;
		}
		else if (((int)e.Key == 141) | ((int)e.Key == 85))
		{
			AllDetailed(op: true);
			e.Handled = true;
		}
		else if (((int)e.Key == 143) | ((int)e.Key == 87))
		{
			AllDetailed(op: false);
			e.Handled = true;
		}
		SetButtons();
	}

	private void SummarizedContentGrid_CustomUnboundColumnData(object sender, GridColumnDataEventArgs e)
	{
		SummarizedContentRow summarizedContentRow = _summarizedContent[e.ListSourceRowIndex];
		int totalCount = 0;
		int selectedCount = 0;
		int importedCount = 0;
		int errorsCount = 0;
		if (e.IsGetData)
		{
			SumByTypeName(summarizedContentRow.TypeName, ref totalCount, ref selectedCount, ref importedCount, ref errorsCount);
			if (e.Column == SummarizedSelected)
			{
				if (totalCount == selectedCount)
				{
					e.Value = true;
				}
				else if (selectedCount == 0)
				{
					e.Value = false;
				}
				else
				{
					e.Value = null;
				}
			}
			else if (e.Column == CountColumn)
			{
				e.Value = totalCount;
			}
			else if (e.Column == CountSelectedColumn)
			{
				e.Value = selectedCount;
			}
			else if (e.Column == ImportedColumn)
			{
				e.Value = importedCount;
			}
			else if (e.Column == ErrorsColumn)
			{
				e.Value = errorsCount;
			}
		}
		else if (_state != State.Importing && e.Column == SummarizedSelected)
		{
			bool value = (bool)e.Value;
			SetSummarizedSelected(summarizedContentRow.TypeName, value);
			StatusBarSelected.Content = _selectedRows.Count;
			SetButtons();
		}
	}

	private void SummarizedContentGrid_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Invalid comparison between Unknown and I4
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Invalid comparison between Unknown and I4
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Invalid comparison between Unknown and I4
		//IL_0128: Unknown result type (might be due to invalid IL or missing references)
		//IL_0132: Invalid comparison between Unknown and I4
		//IL_0135: Unknown result type (might be due to invalid IL or missing references)
		//IL_013c: Invalid comparison between Unknown and I4
		//IL_0152: Unknown result type (might be due to invalid IL or missing references)
		//IL_015c: Invalid comparison between Unknown and I4
		//IL_015f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0166: Invalid comparison between Unknown and I4
		int totalCount = 0;
		int selectedCount = 0;
		int importedCount = 0;
		int errorsCount = 0;
		if (((int)e.Key == 31) | ((int)e.Key == 18))
		{
			SummarizedContentRow summarizedContentRow = (SummarizedContentRow)SummarizedContentGridView.Grid.CurrentItem;
			if (summarizedContentRow != null)
			{
				SumByTypeName(summarizedContentRow.TypeName, ref totalCount, ref selectedCount, ref importedCount, ref errorsCount);
				SetSummarizedSelected(summarizedContentRow.TypeName, totalCount != selectedCount);
				SummarizedContentGridView.MoveNextRow();
				StatusBarSelected.Content = _selectedRows.Count;
			}
			e.Handled = true;
		}
		else if ((int)e.Key == 84)
		{
			for (int i = 0; i <= SummarizedContentGrid.VisibleRowCount - 1; i++)
			{
				int rowHandleByVisibleIndex = SummarizedContentGrid.GetRowHandleByVisibleIndex(i);
				SummarizedContentRow summarizedContentRow = (SummarizedContentRow)SummarizedContentGrid.GetRow(rowHandleByVisibleIndex);
				SumByTypeName(summarizedContentRow.TypeName, ref totalCount, ref selectedCount, ref importedCount, ref errorsCount);
				SetSummarizedSelected(summarizedContentRow.TypeName, totalCount != selectedCount);
			}
			StatusBarSelected.Content = _selectedRows.Count;
			e.Handled = true;
		}
		else if (((int)e.Key == 141) | ((int)e.Key == 85))
		{
			AllDetailed(op: true);
			e.Handled = true;
		}
		else if (((int)e.Key == 143) | ((int)e.Key == 87))
		{
			AllDetailed(op: false);
			e.Handled = true;
		}
		SetButtons();
	}

	private static string FormatXml(string xml)
	{
		try
		{
			return XDocument.Parse(xml).ToString();
		}
		catch (Exception)
		{
			return xml;
		}
	}
}
