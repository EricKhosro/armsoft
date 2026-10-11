using System;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Threading;
using System.Xml;
using System.Xml.Linq;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Enums;
using ArmSoft.SME.Common.Models.Exchange;
using ArmSoft.SME.Core.UI.DPR;
using DevExpress.Utils;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Editors.Settings;
using DevExpress.Xpf.Grid;

namespace ArmSoft.SME.Core.UI.Exchange;

public partial class ExportWindow : ThemedWindow, IComponentConnector
{
	private readonly BindingList<ExportDataRow> _exportDataRows;

	private readonly BackgroundWorker _worker;

	private ExportDataRowState _innerState;

	private string _fileName;

	private string HelpId { get; set; }

	public ExportWindow()
	{
		//IL_0141: Unknown result type (might be due to invalid IL or missing references)
		//IL_0147: Unknown result type (might be due to invalid IL or missing references)
		InitializeComponent();
		Owner = Settings.HostingEnvironment;
		Title = ArmSoft.SME.Common.Resources.ExportData;
		HelpId = "AdminTools_ExportData";
		Data.Header = ArmSoft.SME.Common.Resources.NameOfDirectories;
		ProgressCount.Header = ArmSoft.SME.Common.Resources.RepCount;
		ErrorsCount.Header = ArmSoft.SME.Common.Resources.ImportErrorsCount;
		_exportDataRows = ExportDataRow.GetList();
		ExportDataRowsGrid.ItemsSource = _exportDataRows;
		ExportDataRowsView.AllowMoveColumnToDropArea = false;
		ExportDataRowsView.IsColumnMenuEnabled = false;
		ExportDataRowsView.AllowSorting = false;
		ExportDataRowsGrid.Columns[0].AllowSorting = DefaultBoolean.True;
		StartButton.Content = ArmSoft.SME.Common.Resources.Ok;
		CancelButton.Content = ArmSoft.SME.Common.Resources.Cancel;
		_worker = new BackgroundWorker();
		_worker.DoWork += Worker_DoWork;
		_worker.RunWorkerCompleted += Worker_RunWorkerCompleted;
		_innerState = ExportDataRowState.None;
		StartButton.IsEnabled = false;
		FontFamily = Settings.FontFamily;
		FontSize = Settings.GetFontSize();
		Icon = ImageService.GetImage("export_16.svg");
		Activated += new SafeEventHandler<EventArgs>(ExportWindow_Activated).Handler;
		Closing += new SafeEventHandler<CancelEventArgs>(CheckIntegrityWindow_Closing).Handler;
		ExportDataRowsView.CellValueChanging += new SafeEventHandler<CellValueChangedEventArgs>(ExportDataRowsView_CellValueChanging).Handler;
		CancelButton.Click += new SafeEventHandler<RoutedEventArgs>(CancelButton_Click).Handler;
		ExportDataRowsGrid.CustomUnboundColumnData += new SafeEventHandler<GridColumnDataEventArgs>(ExportDataRowsGrid_CustomUnboundColumnData).Handler;
		ExportDataRowsView.ColumnHeaderClick += new SafeEventHandler<ColumnHeaderClickEventArgs>(ExportDataRowsView_ColumnHeaderClick).Handler;
		ExportDataRowsView.PreviewKeyDown += new SafeEventHandler<KeyEventArgs>(View_PreviewKeyDown).Handler;
		StartButton.Click += new SafeEventHandler<RoutedEventArgs>(StartButton_Click).Handler;
	}

	private void ExportWindow_Activated(object sender, EventArgs e)
	{
		ExportDataRowsGrid.Focus();
	}

	private void ExportDataRowsView_CellValueChanging(object sender, CellValueChangedEventArgs e)
	{
		if (e.Column.EditSettings is CheckEditSettings)
		{
			ExportDataRowsView.CommitEditing();
			ExportDataRowsGrid.RefreshRow(e.RowHandle);
			CalculateStartButtonState();
		}
	}

	private void CalculateStartButtonState()
	{
		foreach (ExportDataRow exportDataRow in _exportDataRows)
		{
			if (exportDataRow.Selected)
			{
				StartButton.IsEnabled = true;
				return;
			}
		}
		StartButton.IsEnabled = false;
	}

	private void CancelButton_Click(object sender, RoutedEventArgs e)
	{
		CancelPressed();
	}

	private void ExportDataRowsGrid_CustomUnboundColumnData(object sender, GridColumnDataEventArgs e)
	{
		if (e.Column.FieldName == "IconUnbound" && e.IsGetData)
		{
			ExportDataRow exportDataRow = _exportDataRows[e.ListSourceRowIndex];
			e.Value = GetInnerImage(exportDataRow.State);
		}
	}

	private static ImageSource GetInnerImage(ExportDataRowState state)
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		return state switch
		{
			ExportDataRowState.FinishedOk => ImageService.GetImage("accept_16.svg"), 
			ExportDataRowState.FinishedError => ImageService.GetImage("error_16.svg"), 
			ExportDataRowState.Checking => ImageService.GetImage("widgets_16.svg"), 
			ExportDataRowState.Canceled => ImageService.GetImage("warning_16.svg"), 
			_ => null, 
		};
	}

	private void ExportDataRowsView_ColumnHeaderClick(object sender, ColumnHeaderClickEventArgs e)
	{
		if (e.Column.FieldName == "Selected" && ExportDataRowsGrid.VisibleRowCount != 0)
		{
			InvertSelection();
			e.Handled = true;
		}
	}

	private void InvertSelection()
	{
		if (Selected.AllowEditing != DefaultBoolean.False)
		{
			int i = 0;
			for (int num = _exportDataRows.Count - 1; i <= num; i++)
			{
				ExportDataRow exportDataRow = _exportDataRows[i];
				exportDataRow.Selected = !exportDataRow.Selected;
			}
			ExportDataRowsView.CommitEditing();
			ExportDataRowsGrid.RefreshData();
			CalculateStartButtonState();
		}
	}

	private void SelectOrDeselectAll(bool isSelect)
	{
		if (Selected.AllowEditing != DefaultBoolean.False)
		{
			int i = 0;
			for (int num = _exportDataRows.Count - 1; i <= num; i++)
			{
				_exportDataRows[i].Selected = isSelect;
			}
			ExportDataRowsView.CommitEditing();
			ExportDataRowsGrid.RefreshData();
			CalculateStartButtonState();
		}
	}

	private void View_PreviewKeyDown(object sender, KeyEventArgs e)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Invalid comparison between Unknown and I4
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Invalid comparison between Unknown and I4
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Invalid comparison between Unknown and I4
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Invalid comparison between Unknown and I4
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Invalid comparison between Unknown and I4
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Invalid comparison between Unknown and I4
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Invalid comparison between Unknown and I4
		if ((int)Keyboard.Modifiers == 0 && (int)e.Key == 13)
		{
			CancelPressed();
			e.Handled = true;
		}
		else if ((int)e.Key == 84)
		{
			InvertSelection();
			e.Handled = true;
		}
		else if ((int)e.Key == 141 || (int)e.Key == 85)
		{
			SelectOrDeselectAll(isSelect: true);
			e.Handled = true;
		}
		else if ((int)e.Key == 143 || (int)e.Key == 87)
		{
			SelectOrDeselectAll(isSelect: false);
			e.Handled = true;
		}
		else if ((int)Keyboard.Modifiers == 0 && (int)e.Key == 90)
		{
			Settings.HostingEnvironment.ShowHelp(HelpId);
			e.Handled = true;
		}
	}

	private void CancelPressed()
	{
		_innerState = ExportDataRowState.Canceled;
		DialogResult = false;
	}

	private void StartButton_Click(object sender, RoutedEventArgs e)
	{
		try
		{
			if (_innerState == ExportDataRowState.None)
			{
				_fileName = Settings.CommonCoreHelper.GetFileNameFromExportDialog(".xml", "XML Files (.xml)|*.xml", "Export_" + DateTime.Now.ToString("MMdd_HHmm"));
				if (!string.IsNullOrWhiteSpace(_fileName))
				{
					Mouse.OverrideCursor = Cursors.Wait;
					StartButton.IsEnabled = false;
					Selected.AllowEditing = DefaultBoolean.False;
					CancelButton.IsEnabled = false;
					_innerState = ExportDataRowState.Checking;
					_worker.RunWorkerAsync();
				}
			}
			else if (_innerState == ExportDataRowState.FinishedOk)
			{
				DialogResult = true;
			}
		}
		catch (Exception ex)
		{
			Mouse.OverrideCursor = null;
			Settings.ErrorHandler(ex);
		}
	}

	private void Worker_DoWork(object sender, DoWorkEventArgs e)
	{
		for (int i = 0; i <= _exportDataRows.Count - 1; i++)
		{
			if (!_exportDataRows[i].Selected)
			{
				continue;
			}
			_exportDataRows[i].State = ExportDataRowState.Checking;
			((DispatcherObject)this).Dispatcher.Invoke((Action)RefreshCurrentExportData, (DispatcherPriority)9);
			try
			{
				ExchangeExportResponse item = DataProcessingRequestHelper.ExecuteThroughHandler<ExportAllDirectoryRequest, ExchangeExportResponse>(DPRType.Other, "ExportAllDirectory", new ExportAllDirectoryRequest(_exportDataRows[i].ExportDataType)).Item2;
				_exportDataRows[i].FileId = item.FileId;
				_exportDataRows[i].ProgressCount = item.AllCount;
				_exportDataRows[i].ErrorsCount = item.ErrorCount;
				_exportDataRows[i].State = ((_exportDataRows[i].ErrorsCount == 0) ? ExportDataRowState.FinishedOk : ExportDataRowState.FinishedError);
			}
			catch (Exception)
			{
				_exportDataRows[i].State = ExportDataRowState.Canceled;
			}
			finally
			{
				((DispatcherObject)this).Dispatcher.Invoke((Delegate)new Action<string>(RefreshCurrentExportData), (DispatcherPriority)9, new object[1] { i.ToString() });
			}
		}
	}

	private void RefreshCurrentExportData()
	{
		ExportDataRowsGrid.RefreshData();
	}

	private void RefreshCurrentExportData(string exportDataIndex)
	{
		int listIndex = Convert.ToInt32(exportDataIndex);
		int rowHandleByListIndex = ExportDataRowsGrid.GetRowHandleByListIndex(listIndex);
		ExportDataRowsGrid.RefreshRow(rowHandleByListIndex);
	}

	private void Worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
	{
		try
		{
			_innerState = ExportDataRowState.FinishedOk;
			CancelButton.IsEnabled = true;
			XmlDocument xmlDocument = new XmlDocument();
			XmlElement newChild = xmlDocument.CreateElement("Exchange", "http://www.armsoft.am/Accountant/6.0");
			xmlDocument.AppendChild(newChild);
			xmlDocument.Save(_fileName);
			XElement xElement = XElement.Load(_fileName);
			string tempFileName = Settings.GetTempFileName(".xml");
			for (int i = 0; i <= _exportDataRows.Count - 1; i++)
			{
				if (_exportDataRows[i].Selected)
				{
					byte[] array = Settings.CreateApiClient().TempFS.Download(_exportDataRows[i].FileId);
					using (FileStream fileStream = File.Create(tempFileName))
					{
						fileStream.Write(array);
						fileStream.Close();
					}
					XElement xElement2 = XElement.Load(tempFileName);
					xElement.Add(xElement2.Elements());
					xElement.Save(_fileName);
				}
			}
			File.Delete(tempFileName);
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
		finally
		{
			Mouse.OverrideCursor = null;
		}
	}

	private void CheckIntegrityWindow_Closing(object sender, CancelEventArgs e)
	{
		if (_innerState == ExportDataRowState.Checking)
		{
			e.Cancel = true;
		}
	}
}
