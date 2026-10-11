using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using ArmSoft.SME.Common.Models.Documents;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Trade.Common.Models.Documents;
using ArmSoft.SME.Trade.UI.DropDownDataProvider;
using ArmSoft.SME.Trade.UI.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.ComponentModel.__Internals;
using CommunityToolkit.Mvvm.Input;

namespace ArmSoft.SME.Trade.UI.DataViewModels;

public class SalesDocumentsHistoryViewModel : ObservableObject
{
	private const double InsideReceiptNotListViewHeight = 460.0;

	[ObservableProperty]
	private MTBillDocumentReceipt _selectedDocumentAsReceipt;

	[ObservableProperty]
	private double _receiptListViewMaxHeight;

	[ObservableProperty]
	private double _windowCurrentHeight;

	[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.RelayCommandGenerator", "8.4.0.0")]
	private AsyncRelayCommand<ECRChecksListInsidePOSDataProvider.DataRow>? changeSelectedDocumentCommand;

	public TradeDocument SelectedDocument { get; set; }

	[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
	[ExcludeFromCodeCoverage]
	public MTBillDocumentReceipt SelectedDocumentAsReceipt
	{
		get
		{
			return _selectedDocumentAsReceipt;
		}
		[MemberNotNull("_selectedDocumentAsReceipt")]
		set
		{
			if (!EqualityComparer<MTBillDocumentReceipt>.Default.Equals(_selectedDocumentAsReceipt, value))
			{
				OnPropertyChanging(__KnownINotifyPropertyChangingArgs.SelectedDocumentAsReceipt);
				_selectedDocumentAsReceipt = value;
				OnPropertyChanged(__KnownINotifyPropertyChangedArgs.SelectedDocumentAsReceipt);
			}
		}
	}

	[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
	[ExcludeFromCodeCoverage]
	public double ReceiptListViewMaxHeight
	{
		get
		{
			return _receiptListViewMaxHeight;
		}
		set
		{
			if (!EqualityComparer<double>.Default.Equals(_receiptListViewMaxHeight, value))
			{
				OnPropertyChanging(__KnownINotifyPropertyChangingArgs.ReceiptListViewMaxHeight);
				_receiptListViewMaxHeight = value;
				OnPropertyChanged(__KnownINotifyPropertyChangedArgs.ReceiptListViewMaxHeight);
			}
		}
	}

	[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
	[ExcludeFromCodeCoverage]
	public double WindowCurrentHeight
	{
		get
		{
			return _windowCurrentHeight;
		}
		set
		{
			if (!EqualityComparer<double>.Default.Equals(_windowCurrentHeight, value))
			{
				OnPropertyChanging(__KnownINotifyPropertyChangingArgs.WindowCurrentHeight);
				_windowCurrentHeight = value;
				OnPropertyChanged(__KnownINotifyPropertyChangedArgs.WindowCurrentHeight);
			}
		}
	}

	[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.RelayCommandGenerator", "8.4.0.0")]
	[ExcludeFromCodeCoverage]
	public IAsyncRelayCommand<ECRChecksListInsidePOSDataProvider.DataRow> ChangeSelectedDocumentCommand => changeSelectedDocumentCommand ?? (changeSelectedDocumentCommand = new AsyncRelayCommand<ECRChecksListInsidePOSDataProvider.DataRow>(ChangeSelectedDocument));

	public SalesDocumentsHistoryViewModel()
	{
		ReceiptListViewMaxHeight = 350.0;
	}

	public void ConstrcutUI(double windowCurrentHeight)
	{
		ReceiptListViewMaxHeight = windowCurrentHeight - 460.0;
	}

	[RelayCommand]
	public async Task ChangeSelectedDocument(ECRChecksListInsidePOSDataProvider.DataRow selectedDataRow)
	{
		try
		{
			if (selectedDataRow != null)
			{
				Document document = await Task.Run(() => Settings.DocumentService.Load(selectedDataRow.ISN, selectedDataRow.DocType));
				if (document is MTBill mTBill)
				{
					SelectedDocumentAsReceipt = new MTBillDocumentReceipt(mTBill);
					SelectedDocument = mTBill;
				}
				else if (document is MTBillReturn mTBillReturn)
				{
					SelectedDocumentAsReceipt = new MTBillDocumentReceipt(mTBillReturn);
					SelectedDocument = mTBillReturn;
				}
				else if (document is MTReturnFromCustomer mTReturnFromCustomer)
				{
					SelectedDocumentAsReceipt = new MTBillDocumentReceipt(mTReturnFromCustomer);
					SelectedDocument = mTReturnFromCustomer;
				}
				else
				{
					SelectedDocumentAsReceipt = new MTBillDocumentReceipt();
				}
			}
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}
}
