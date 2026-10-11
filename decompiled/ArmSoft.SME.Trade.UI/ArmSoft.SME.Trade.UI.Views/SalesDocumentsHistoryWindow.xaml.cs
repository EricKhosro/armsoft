using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Threading;
using ArmSoft.SME.Common.SmartEnums;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Core.UI.DataViews;
using ArmSoft.SME.Core.UI.DropDowns;
using ArmSoft.SME.Core.UI.Helpers;
using ArmSoft.SME.Core.UI.OtherWindows;
using ArmSoft.SME.Trade.Common.Parameters;
using ArmSoft.SME.Trade.Common.SmartEnums;
using ArmSoft.SME.Trade.UI.DataViewModels;
using ArmSoft.SME.Trade.UI.DropDownDataProvider;
using ArmSoft.SME.Trade.UI.Helpers;
using DevExpress.Xpf.Grid;

namespace ArmSoft.SME.Trade.UI.Views;

public partial class SalesDocumentsHistoryWindow : BaseThemedWindow, IComponentConnector
{
	private IDropDownView _salesDocumentsDropDownView;

	private SalesDocumentsHistoryViewModel _salesDocumentsHistoryViewModel;

	public SalesDocumentsHistoryWindow(Window owner, string title, IDropDownView dropDownView)
	{
		InitializeComponent();
		Owner = owner;
		Title = title;
		_salesDocumentsDropDownView = dropDownView;
		_salesDocumentsHistoryViewModel = (SalesDocumentsHistoryViewModel)DataContext;
		FillValues();
		ConstructUI();
		ASFontHelper.SetFontSettings(this);
	}

	private void FillValues()
	{
		AddColumn("DocDate");
		AddColumn("DocNum");
		AddColumn("ECRCheckNum");
		AddColumn("DocTypeName");
		AddColumn("Summ", isNumeric: true);
		AddColumn("SummaCash", isNumeric: true);
		AddColumn("SummaBank", isNumeric: true);
		AddColumn("SummaryOfOtherSums", isNumeric: true);
		SalesDocumentsGridControl.View.ShowSearchPanel(moveFocusInSearchPanel: true);
	}

	private void Grid_CustomRowFilter(object sender, RowFilterEventArgs e)
	{
		if (!(sender is GridControl gridControl) || !(gridControl?.View is TableView { SearchString: var searchString }) || string.IsNullOrWhiteSpace(searchString))
		{
			return;
		}
		bool visible = false;
		string[] array = new string[4] { "DocNum", "DocDate", "DocTypeName", "ECRCheckNum" };
		foreach (string fieldName in array)
		{
			object cellValueByListIndex = gridControl.GetCellValueByListIndex(e.ListSourceRowIndex, fieldName);
			if (cellValueByListIndex != null && cellValueByListIndex.ToString().Contains(searchString))
			{
				visible = true;
				break;
			}
		}
		string value = searchString.Replace(",", "");
		array = new string[4] { "Summ", "SummaCash", "SummaBank", "SummaryOfOtherSums" };
		foreach (string fieldName2 in array)
		{
			object cellValueByListIndex2 = gridControl.GetCellValueByListIndex(e.ListSourceRowIndex, fieldName2);
			if (cellValueByListIndex2 != null && cellValueByListIndex2.ToString().Replace(",", "").Contains(value))
			{
				visible = true;
				break;
			}
		}
		e.Visible = visible;
		e.Handled = true;
	}

	private void ConstructUI()
	{
		SalesDocumentsTableView.RowStyle = StyleHelper.GetTableRowWithSelectionStyle();
		PrintButton.IsEnabled = Settings.ParametersService.AllowPrintMTCheck() && Settings.ParametersService.MTBillPrintBillShowButton();
	}

	private void ReloadReceiptUI()
	{
		if (_salesDocumentsHistoryViewModel != null)
		{
			ECRChecksListInsidePOSDataProvider.DataRow dataRow = (ECRChecksListInsidePOSDataProvider.DataRow)(SalesDocumentsGridControl?.SelectedItem);
			if (dataRow != null)
			{
				_salesDocumentsHistoryViewModel.ChangeSelectedDocumentCommand.ExecuteAsync(dataRow);
			}
		}
	}

	private void ExitButton_Click(object sender, RoutedEventArgs e)
	{
		try
		{
			Close();
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	private void PrintDocumentButton_Clicked(object sender, RoutedEventArgs e)
	{
		try
		{
			if (_salesDocumentsHistoryViewModel != null && _salesDocumentsHistoryViewModel.SelectedDocumentAsReceipt != null && (Settings.ParametersService.AllowPrintMTCheck() & Settings.ParametersService.MTBillPrintBillShowButton()))
			{
				if ((_salesDocumentsHistoryViewModel.SelectedDocumentAsReceipt.DocTypeCode == ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTBill.Value && !string.IsNullOrWhiteSpace(Settings.ParametersService.MTCheckTemplate())) || (_salesDocumentsHistoryViewModel.SelectedDocumentAsReceipt.DocTypeCode == ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTBillReturn.Value && !string.IsNullOrWhiteSpace(Settings.ParametersService.RetCheckTemplate())))
				{
					PrintDocument(1);
				}
				else
				{
					PrintDocumentInSelectedTemplateWithCount();
				}
			}
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	private void PrintTemplateSelectTouchDialogButton_Click(object sender, SaleWindowHelper.PrintButtonClickEventArgs e)
	{
		PrintDocument(e.PrintCopies, e.PrintTemplate);
	}

	private void SalesDocumentsGridControl_SelectedItemChanged(object sender, SelectedItemChangedEventArgs e)
	{
		ReloadReceiptUI();
	}

	private void SalesDocumentsGridControl_FilterChanged(object sender, RoutedEventArgs e)
	{
		((DispatcherObject)this).Dispatcher.BeginInvoke((Delegate)(Action)(() =>
		{
			if (SalesDocumentsGridControl.VisibleItems.Count > 0)
			{
				SalesDocumentsGridControl.SelectedItem = SalesDocumentsGridControl.VisibleItems[0];
			}
		}), Array.Empty<object>());
	}

	private void Grid_PreviewKeyDown(object sender, KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Invalid comparison between Unknown and I4
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Invalid comparison between Unknown and I4
		if ((int)e.Key == 26)
		{
			if (SalesDocumentsGridControl.View is TableView tableView)
			{
				int num = tableView.FocusedRowHandle + 1;
				if (SalesDocumentsGridControl.VisibleRowCount > num)
				{
					object row = SalesDocumentsGridControl.GetRow(num);
					SalesDocumentsGridControl.SelectedItem = row;
					tableView.FocusedRowHandle = num;
					e.Handled = true;
				}
			}
		}
		else if ((int)e.Key == 24 && SalesDocumentsGridControl.View is TableView tableView2)
		{
			int num2 = tableView2.FocusedRowHandle - 1;
			if (num2 >= 0)
			{
				object row2 = SalesDocumentsGridControl.GetRow(num2);
				SalesDocumentsGridControl.SelectedItem = row2;
				tableView2.FocusedRowHandle = num2;
				e.Handled = true;
			}
		}
	}

	private void BaseThemedWindow_Loaded(object sender, RoutedEventArgs e)
	{
		try
		{
			_salesDocumentsDropDownView.BeforeLoadData(SalesDocumentsGridControl);
			_salesDocumentsDropDownView.LoadData();
			SalesDocumentsGridControl.ItemsSource = _salesDocumentsDropDownView.Data;
			ReloadReceiptUI();
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
			Close();
		}
	}

	private void BaseThemedWindow_SizeChanged(object sender, SizeChangedEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		Size newSize = e.NewSize;
		double height = newSize.Height;
		if (_salesDocumentsHistoryViewModel != null)
		{
			_salesDocumentsHistoryViewModel.ConstrcutUI(height);
		}
	}

	private void PrintDocumentInSelectedTemplateWithCount()
	{
		try
		{
			if (_salesDocumentsHistoryViewModel != null && _salesDocumentsHistoryViewModel.SelectedDocumentAsReceipt != null)
			{
				PrintTemplateTouchGridControl printTemplateTouchGridControl = new PrintTemplateTouchGridControl(this, new Template(_salesDocumentsHistoryViewModel.SelectedDocumentAsReceipt.DocTypeCode, PrintTemplateFileFormatSE.XML));
				printTemplateTouchGridControl.ButtonClick += PrintTemplateSelectTouchDialogButton_Click;
				printTemplateTouchGridControl.ShowDialog();
			}
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	private void PrintDocument(short printCopies = 1, string templateCode = "")
	{
		try
		{
			if (_salesDocumentsHistoryViewModel != null && _salesDocumentsHistoryViewModel.SelectedDocumentAsReceipt != null)
			{
				PrintingHelper.PrintTradeDocument(_salesDocumentsHistoryViewModel.SelectedDocument, printCopies, templateCode);
			}
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	private void AddColumn(string fieldName, bool isNumeric = false)
	{
		MyGridColumn myGridColumn = Mapper.Convert(_salesDocumentsDropDownView.Columns[fieldName], _salesDocumentsDropDownView.GetColumnType);
		if (isNumeric)
		{
			myGridColumn.ColumnFilterMode = ColumnFilterMode.Value;
		}
		SalesDocumentsGridControl.Columns.Add(myGridColumn);
	}
}
