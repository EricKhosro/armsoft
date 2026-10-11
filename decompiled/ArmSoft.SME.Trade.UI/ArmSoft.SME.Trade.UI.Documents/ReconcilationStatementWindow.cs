using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Constants;
using ArmSoft.SME.Common.Models;
using ArmSoft.SME.Common.Models.Documents;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Core.UI.Controls;
using ArmSoft.SME.Core.UI.Documents;
using ArmSoft.SME.Core.UI.DropDowns;
using ArmSoft.SME.Core.UI.Enums;
using ArmSoft.SME.Core.UI.Extensions;
using ArmSoft.SME.Core.UI.Helpers;
using ArmSoft.SME.Trade.Client.Routes;
using ArmSoft.SME.Trade.Common;
using ArmSoft.SME.Trade.Common.Models.Caches;
using ArmSoft.SME.Trade.Common.Models.Documents;
using ArmSoft.SME.Trade.Common.Parameters;
using ArmSoft.SME.Trade.Common.PersistableObject;
using ArmSoft.SME.Trade.Common.SmartEnums;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.ComponentModel.__Internals;
using DevExpress.Xpf.Bars;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Editors;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.LayoutControl;
using Microsoft.Win32;

namespace ArmSoft.SME.Trade.UI.Documents;

[DocumentWindow(34)]
public class ReconcilationStatementWindow : TradeDocumentWindow<Reconcilation>
{
	public class ReconcilationSpecificationRow : DocumentGridRow
	{
		[ObservableProperty]
		private string type = string.Empty;

		[ObservableProperty]
		private short rowNum;

		[ObservableProperty]
		private DateOnly date;

		[ObservableProperty]
		private string contractCode = string.Empty;

		[ObservableProperty]
		private Guid baseDocISN;

		[ObservableProperty]
		private string baseDocDescr = string.Empty;

		[ObservableProperty]
		private string transactionComment = string.Empty;

		[ObservableProperty]
		private decimal summIn;

		[ObservableProperty]
		private decimal summOut;

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public string Type
		{
			get
			{
				return type;
			}
			[MemberNotNull("type")]
			set
			{
				if (!EqualityComparer<string>.Default.Equals(type, value))
				{
					OnPropertyChanging(__KnownINotifyPropertyChangingArgs.Type);
					type = value;
					OnPropertyChanged(__KnownINotifyPropertyChangedArgs.Type);
				}
			}
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public short RowNum
		{
			get
			{
				return rowNum;
			}
			set
			{
				if (!EqualityComparer<short>.Default.Equals(rowNum, value))
				{
					OnPropertyChanging(__KnownINotifyPropertyChangingArgs.RowNum);
					rowNum = value;
					OnPropertyChanged(__KnownINotifyPropertyChangedArgs.RowNum);
				}
			}
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public DateOnly Date
		{
			get
			{
				return date;
			}
			set
			{
				if (!EqualityComparer<DateOnly>.Default.Equals(date, value))
				{
					OnPropertyChanging(__KnownINotifyPropertyChangingArgs.Date);
					date = value;
					OnPropertyChanged(__KnownINotifyPropertyChangedArgs.Date);
				}
			}
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public string ContractCode
		{
			get
			{
				return contractCode;
			}
			[MemberNotNull("contractCode")]
			set
			{
				if (!EqualityComparer<string>.Default.Equals(contractCode, value))
				{
					OnPropertyChanging(__KnownINotifyPropertyChangingArgs.ContractCode);
					contractCode = value;
					OnPropertyChanged(__KnownINotifyPropertyChangedArgs.ContractCode);
				}
			}
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public Guid BaseDocISN
		{
			get
			{
				return baseDocISN;
			}
			set
			{
				if (!EqualityComparer<Guid>.Default.Equals(baseDocISN, value))
				{
					OnPropertyChanging(__KnownINotifyPropertyChangingArgs.BaseDocISN);
					baseDocISN = value;
					OnPropertyChanged(__KnownINotifyPropertyChangedArgs.BaseDocISN);
				}
			}
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public string BaseDocDescr
		{
			get
			{
				return baseDocDescr;
			}
			[MemberNotNull("baseDocDescr")]
			set
			{
				if (!EqualityComparer<string>.Default.Equals(baseDocDescr, value))
				{
					OnPropertyChanging(__KnownINotifyPropertyChangingArgs.BaseDocDescr);
					baseDocDescr = value;
					OnPropertyChanged(__KnownINotifyPropertyChangedArgs.BaseDocDescr);
				}
			}
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public string TransactionComment
		{
			get
			{
				return transactionComment;
			}
			[MemberNotNull("transactionComment")]
			set
			{
				if (!EqualityComparer<string>.Default.Equals(transactionComment, value))
				{
					OnPropertyChanging(__KnownINotifyPropertyChangingArgs.TransactionComment);
					transactionComment = value;
					OnPropertyChanged(__KnownINotifyPropertyChangedArgs.TransactionComment);
				}
			}
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public decimal SummIn
		{
			get
			{
				return summIn;
			}
			set
			{
				if (!EqualityComparer<decimal>.Default.Equals(summIn, value))
				{
					OnPropertyChanging(__KnownINotifyPropertyChangingArgs.SummIn);
					summIn = value;
					OnPropertyChanged(__KnownINotifyPropertyChangedArgs.SummIn);
				}
			}
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public decimal SummOut
		{
			get
			{
				return summOut;
			}
			set
			{
				if (!EqualityComparer<decimal>.Default.Equals(summOut, value))
				{
					OnPropertyChanging(__KnownINotifyPropertyChangingArgs.SummOut);
					summOut = value;
					OnPropertyChanged(__KnownINotifyPropertyChangedArgs.SummOut);
				}
			}
		}
	}

	private PartnerContractGroupControls _partnerContractContainer;

	private DateEditControl _periodStart;

	private DateEditControl _periodEnd;

	private ComboBoxEditExt _comment;

	private DropDownControl _owner;

	private BarEditItemExt _loadOurRows;

	private DocumentGridControl _ourCalcsGrid;

	private TableView _ourCalcsTableView;

	private ObservableCollection<ReconcilationSpecificationRow> _ourCalcsList = new ObservableCollection<ReconcilationSpecificationRow>();

	private PopupCalcEditExt _ourStartRem;

	private PopupCalcEditExt _ourEndRem;

	private DXTabControl _tabs;

	private ReportBarButtonItem _barButtonJumpTo;

	protected override double WidthValue => 1090.0;

	protected override double HeightValue => 500.0;

	protected override double MinWidthValue => 620.0;

	protected override double MinHeightValue => 250.0;

	protected override bool AllowPrint()
	{
		return true;
	}

	protected override bool AllowPrintOnNew()
	{
		return true;
	}

	public ReconcilationStatementWindow()
	{
	}

	public ReconcilationStatementWindow(Reconcilation doc)
		: base(doc)
	{
	}

	protected override void ConstructUI()
	{
		base.ConstructUI();
		LayoutGroupExt container = AddLayoutGroup(null, LayoutGroupView.Group, Orientation.Horizontal);
		_date = AddDateControl(container, Definition.Fields["DocDate"]);
		_date.Margin = new Thickness(0.0, 0.0, 10.0, 0.0);
		_date.EditValueChanged += new SafeDocumentEventHandler<EditValueChangedEventArgs>(Date_EditValueChanged, this).Handler;
		_docNum = AddButtonEditControl(container, Definition.Fields["DocNum"], 1);
		_docNum.AllowMagicButton = true;
		_docNum.MagicToolTip = ArmSoft.SME.Common.Resources.DocNextNumbers;
		_docNum.MagicClick += new SafeDocumentEventHandler<MagicClickEventArgs>(DocNum_MagicClick, this).Handler;
		_docNum.EditValueChanged += new SafeDocumentEventHandler<EditValueChangedEventArgs>(DocNum_EditValueChanged, this).Handler;
		_partnerContractContainer = AddPartnerContractGroupControls(null, (PartnerContractGroupDefinition)Definition.Fields["PartnerContractGroup"], _date, null, null, null, isShowContractCurrCode: true, isShowSummaDebt: false, showOnlyCurCodeNCC: false, isContractRequired: false);
		_partnerContractContainer.PartnerCode.IsRequired = true;
		LayoutGroupExt container2 = AddLayoutGroup(null, LayoutGroupView.Group, Orientation.Horizontal);
		_periodStart = AddDateControl(container2, Definition.Fields["PeriodStart"]);
		_periodStart.Margin = new Thickness(0.0, 0.0, 10.0, 0.0);
		_periodEnd = AddDateControl(container2, Definition.Fields["PeriodEnd"]);
		_periodEnd.Margin = new Thickness(0.0, 0.0, 10.0, 0.0);
		_comment = AddComboBoxControl(null, Definition.Fields["Comment"], TradeSettings.TradeCacheService.StandardComments.GetCaptionsList(), HorizontalAlignment.Stretch);
		_owner = AddDropDownControl(null, Definition.Fields["Owner"], new UsersFilter());
		_owner.CodeWidth = 12;
		_owner.ShowComment = false;
		_owner.InnerCodeEdit.Width = _owner.InnerCodeEdit.MinWidth;
		if (!Settings.LoginService.IsAdmin && Settings.ParametersService.DenyAccessToOtherOwnerDocs())
		{
			((LayoutItemWithRequired)(object)_owner.Parent).Visibility = Visibility.Collapsed;
		}
		_tabs = AddTabControl(null);
		LayoutGroup container3 = AddTabbedGroup(_tabs, ArmSoft.SME.Common.Resources.OurCalculations, "OurCalculations");
		_ourCalcsGrid = AddGrid(container3, Definition.Grids["OurCalcsList"], HorizontalAlignment.Stretch, VerticalAlignment.Stretch);
		_ourCalcsGrid.AddingRow += new SafeDocumentEventHandler<DocumentGridAddingRowEventArgs>(OurCalcsGrid_AddingRow, this).Handler;
		_ourCalcsGrid.DuplicatingRow += new SafeDocumentEventHandler<DocumentGridDuplicatingRowEventArgs>(OurCalcsGrid_DuplicatingRow, this).Handler;
		_loadOurRows = _ourCalcsGrid.AddButton("", ArmSoft.SME.Common.Resources.ButtonLoad, ArmSoft.SME.Common.Resources.ButtonLoad, 54.0);
		_loadOurRows.Click += new SafeDocumentEventHandler<RoutedEventArgs>(LoadOurRows_Click, this).Handler;
		_ourCalcsTableView = _ourCalcsGrid.View;
		_ourCalcsGrid.ToolTip = ArmSoft.SME.Common.Resources.DeedsList;
		_ourCalcsGrid.AddDateColumn(Definition.Grids["OurCalcsList"].Columns["Date"]).ReadOnly = true;
		_ourCalcsGrid.AddStringColumn(Definition.Grids["OurCalcsList"].Columns["ContractCode"]).ReadOnly = true;
		_ourCalcsGrid.AddStringColumn(Definition.Grids["OurCalcsList"].Columns["BaseDocDescr"]).ReadOnly = true;
		_ourCalcsGrid.AddStringColumn(Definition.Grids["OurCalcsList"].Columns["TransactionComment"]);
		_ourCalcsGrid.AddNumberColumn(Definition.Grids["OurCalcsList"].Columns["SummIn"], showTotal: true).ReadOnly = true;
		_ourCalcsGrid.AddNumberColumn(Definition.Grids["OurCalcsList"].Columns["SummOut"], showTotal: true).ReadOnly = true;
		_ourCalcsGrid.AllowAdd = false;
		_ourCalcsGrid.AllowDelete = false;
		_ourCalcsGrid.AllowDuplicate = false;
		LayoutGroupExt container4 = AddLayoutGroup(container3, LayoutGroupView.Group, Orientation.Horizontal);
		_ourStartRem = AddNumericControl(container4, Definition.Fields["OurStartRem"], useCurrencyFormat: true, HorizontalAlignment.Right);
		_ourEndRem = AddNumericControl(container4, Definition.Fields["OurEndRem"], useCurrencyFormat: true, HorizontalAlignment.Right);
	}

	protected override void OnNew()
	{
		base.OnNew();
		_owner.Code = Settings.LoginService.UserName;
		if (_ourCalcsList.Count == 0)
		{
			_periodStart.EditValue = Settings.HostingEnvironment.DateBegin;
			_periodEnd.EditValue = Settings.HostingEnvironment.DateEnd;
		}
		_date.EditValue = Settings.HostingEnvironment.CurrentDate;
	}

	protected override void OnNewAfterSave()
	{
		base.OnNewAfterSave();
		_ourCalcsList.Clear();
		_ourStartRem.EditValue = 0m;
		_ourEndRem.EditValue = 0m;
		_owner.Code = Settings.LoginService.UserName;
	}

	private void OurCalcsGrid_AddingRow(object sender, DocumentGridAddingRowEventArgs e)
	{
		if (!_date.DoValidate())
		{
			ControlsHelper.SetFocus(_date);
			throw new ASException(ArmSoft.SME.Common.Resources.ValidateEmptyDate);
		}
		_ourCalcsList.Add(new ReconcilationSpecificationRow
		{
			Type = string.Empty,
			Date = _date.EditValue.Value
		});
		e.RowNumber = _ourCalcsList.Count - 1;
	}

	private void OurCalcsGrid_DuplicatingRow(object sender, DocumentGridDuplicatingRowEventArgs e)
	{
		ReconcilationSpecificationRow reconcilationSpecificationRow = (ReconcilationSpecificationRow)_ourCalcsGrid.Grid.GetRow(e.RowHandle);
		if (reconcilationSpecificationRow != null)
		{
			ReconcilationSpecificationRow reconcilationSpecificationRow2 = new ReconcilationSpecificationRow();
			reconcilationSpecificationRow.Clone(reconcilationSpecificationRow2);
			_ourCalcsList.Add(reconcilationSpecificationRow2);
			e.NewRowNumber = _ourCalcsList.Count - 1;
		}
	}

	private void LoadOurRows_Click(object sender, RoutedEventArgs e)
	{
		new List<PartCashBankAccOperJournalShortRow>();
		decimal num = 0m;
		try
		{
			if (!_periodStart.DoValidate())
			{
				ControlsHelper.SetFocus(_periodStart);
				throw new ASException(ArmSoft.SME.Common.Resources.ValidateEmptyDate);
			}
			if (!_periodEnd.DoValidate())
			{
				ControlsHelper.SetFocus(_periodEnd);
				throw new ASException(ArmSoft.SME.Common.Resources.ValidateEmptyDate);
			}
			if (string.IsNullOrWhiteSpace(_partnerContractContainer.PartnerCode.Code))
			{
				throw new ASException(string.Format(ArmSoft.SME.Common.Resources.FieldIsEmpty, ArmSoft.SME.Common.Resources.Partner));
			}
			if (_ourCalcsList.Count > 0)
			{
				if (ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Common.Resources.RefreshCalcGrid, Settings.ProgramName, MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No) == MessageBoxResult.No)
				{
					return;
				}
				_ourCalcsList.Clear();
			}
			PartnerCacheRow item = TradeSettings.TradeCacheService.Partner.GetItem(_partnerContractContainer.PartnerCode.Code);
			SummCurrSummNccPair summCurrSummNccPair = (string.IsNullOrWhiteSpace(_partnerContractContainer.Contract.Code) ? TradeSettings.CreateTradeClient().CommonServices.GetPartRemByPartIDByDate(CommonConst.AccountingsTypes.PartnerContract, _periodStart.EditValue.Value.AddDays(-1), item.Id, "") : TradeSettings.CreateTradeClient().CommonServices.GetAccRem(CommonConst.AccountingsTypes.PartnerContract, _partnerContractContainer.Contract.Code, item.Id, "", _periodStart.EditValue.Value.AddDays(-1)));
			List<PartCashBankAccOperJournalShortRow> partnerOperJournal = TradeSettings.CreateTradeClient().CommonServices.GetPartnerOperJournal(_periodStart.EditValue.Value, _periodEnd.EditValue.Value, item.Id, _partnerContractContainer.Contract.Code);
			decimal RowTurnSumm = 0m;
			foreach (PartCashBankAccOperJournalShortRow item2 in partnerOperJournal)
			{
				if (AddRowToOurCalcsList(item2, ref RowTurnSumm))
				{
					num += RowTurnSumm;
				}
			}
			if (!string.IsNullOrWhiteSpace(_partnerContractContainer.Contract.Code))
			{
				_ourStartRem.Value = summCurrSummNccPair.SummCurr;
				_ourEndRem.Value = summCurrSummNccPair.SummCurr + num;
			}
			else
			{
				_ourStartRem.Value = summCurrSummNccPair.SummNcc;
				_ourEndRem.Value = summCurrSummNccPair.SummNcc + num;
			}
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	private bool AddRowToOurCalcsList(PartCashBankAccOperJournalShortRow sourceRow, ref decimal RowTurnSumm)
	{
		bool flag = true;
		if (!string.IsNullOrWhiteSpace(_partnerContractContainer.Contract.Code) && _partnerContractContainer.Contract.Code.Trim() != sourceRow.AccountCode.Trim())
		{
			flag = false;
		}
		if (flag)
		{
			ReconcilationSpecificationRow reconcilationSpecificationRow = new ReconcilationSpecificationRow();
			reconcilationSpecificationRow.Type = "1";
			reconcilationSpecificationRow.Date = sourceRow.Date;
			reconcilationSpecificationRow.BaseDocISN = sourceRow.BaseISN;
			reconcilationSpecificationRow.ContractCode = sourceRow.AccountCode;
			reconcilationSpecificationRow.TransactionComment = sourceRow.Comment;
			reconcilationSpecificationRow.BaseDocDescr = TradeSettings.CreateTradeClient().CommonServices.GetDocumentDescription(sourceRow.BaseISN, ArmSoft.SME.Common.Constants.Common.DocumentDescriptionMode.TypeDateNum);
			if (!string.IsNullOrWhiteSpace(_partnerContractContainer.Contract.Code))
			{
				reconcilationSpecificationRow.SummIn = sourceRow.CurrSummIn;
				reconcilationSpecificationRow.SummOut = sourceRow.CurrSummOut;
			}
			else
			{
				reconcilationSpecificationRow.SummIn = sourceRow.SummIn;
				reconcilationSpecificationRow.SummOut = sourceRow.SummOut;
			}
			reconcilationSpecificationRow.TransactionComment = sourceRow.Comment;
			RowTurnSumm = reconcilationSpecificationRow.SummIn - reconcilationSpecificationRow.SummOut;
			_ourCalcsList.Add(reconcilationSpecificationRow);
			int rowHandleByListIndex = _ourCalcsGrid.Grid.GetRowHandleByListIndex(_ourCalcsList.Count - 1);
			_ourCalcsGrid.Grid.RefreshRow(rowHandleByListIndex);
			_ourCalcsGrid.ConfigureButtons();
		}
		return flag;
	}

	protected override void LoadDataToControls()
	{
		base.LoadDataToControls();
		_ourCalcsGrid.ItemsSource = _ourCalcsList;
		_ourCalcsList.Clear();
		foreach (Reconcilation.ReconcilationSpecificationRow ourCalcs in PO.OurCalcsList)
		{
			ReconcilationSpecificationRow reconcilationSpecificationRow = new ReconcilationSpecificationRow();
			reconcilationSpecificationRow.Date = ourCalcs.Date.Value;
			reconcilationSpecificationRow.Type = ourCalcs.Type;
			reconcilationSpecificationRow.ContractCode = ourCalcs.ContractCode;
			reconcilationSpecificationRow.BaseDocDescr = ourCalcs.BaseDocDescr;
			reconcilationSpecificationRow.BaseDocISN = ourCalcs.BaseDocISN.Value;
			reconcilationSpecificationRow.TransactionComment = ourCalcs.TransactionComment;
			reconcilationSpecificationRow.SummOut = ourCalcs.SummOut.Value;
			reconcilationSpecificationRow.SummIn = ourCalcs.SummIn.Value;
			_ourCalcsList.Add(reconcilationSpecificationRow);
		}
		_partnerContractContainer.ValueChangedForLoadDoc();
		ReportBarButtonItem barButtonJumpTo = _barButtonJumpTo;
		int num = 4;
		List<PropertyWithValue> list = new List<PropertyWithValue>(num);
		CollectionsMarshal.SetCount(list, num);
		Span<PropertyWithValue> span = CollectionsMarshal.AsSpan(list);
		span[0] = new PropertyWithValue("DocTypeParam", PO.POType.ToString());
		span[1] = new PropertyWithValue("StartDate", PO.DocDate.ToString());
		span[2] = new PropertyWithValue("EndDate", PO.DocDate.ToString());
		span[3] = new PropertyWithValue("SelectedRowDocISN", PO.ISN.ToString());
		barButtonJumpTo.PropertyWithValues = list;
	}

	protected override void SetControlsReadonly()
	{
		base.SetControlsReadonly();
		if (_viewMode != WindowViewMode.View)
		{
			_ourStartRem.IsReadOnly = true;
			_ourEndRem.IsReadOnly = true;
		}
	}

	protected override void AddToolbarItems()
	{
		base.AddToolbarItems();
		Ribbon.ToolbarItemLinks.Add(_barButtonJumpTo.ConvertForToolbar("t_doc_JumpTo_16.svg"));
	}

	protected override void AddItemsToRibbon()
	{
		base.AddItemsToRibbon();
		CreateRibbonPageGroupActions();
		BarSubItem barSubItem = new BarSubItem();
		_barButtonJumpTo = new ReportBarButtonItem(DataProviderTypeSE.AllDocumentsJournal, showDialog: false, ArmSoft.SME.Common.Resources.JumpTo, null, "doc_JumpTo_16.svg", "doc_JumpTo_32.svg");
		_barButtonJumpTo.AddInBarSubItem(ref _ribbonPageGroupActions, ref barSubItem);
	}

	protected override void SetReportBarButtonsProperties()
	{
		base.SetReportBarButtonsProperties();
		ReportBarButtonItem barButtonJumpTo = _barButtonJumpTo;
		int num = 4;
		List<PropertyWithValue> list = new List<PropertyWithValue>(num);
		CollectionsMarshal.SetCount(list, num);
		Span<PropertyWithValue> span = CollectionsMarshal.AsSpan(list);
		span[0] = new PropertyWithValue("DocTypeParam", PO.POType.ToString());
		span[1] = new PropertyWithValue("StartDate", PO.DocDate.ToString());
		span[2] = new PropertyWithValue("EndDate", PO.DocDate.ToString());
		span[3] = new PropertyWithValue("SelectedRowDocISN", PO.ISN.ToString());
		barButtonJumpTo.PropertyWithValues = list;
	}

	protected override void SetReportBarButtonsEnable(bool isEnabled)
	{
		base.SetReportBarButtonsEnable(isEnabled);
		_barButtonJumpTo.IsEnabled = isEnabled;
	}

	protected override void FillDocument(StoreRequest sr)
	{
		base.FillDocument(sr);
		PO.OurCalcsList.Clear();
		foreach (ReconcilationSpecificationRow ourCalcs in _ourCalcsList)
		{
			Reconcilation.ReconcilationSpecificationRow item = new Reconcilation.ReconcilationSpecificationRow
			{
				Date = ourCalcs.Date,
				Type = ourCalcs.Type,
				ContractCode = ourCalcs.ContractCode,
				BaseDocDescr = ourCalcs.BaseDocDescr,
				BaseDocISN = ourCalcs.BaseDocISN,
				TransactionComment = ourCalcs.TransactionComment,
				SummOut = ourCalcs.SummOut,
				SummIn = ourCalcs.SummIn
			};
			PO.OurCalcsList.Add(item);
		}
	}

	private void Date_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		if (!_date.DoValidate() || !_date.EditValue.HasValue)
		{
			ControlsHelper.SetFocus(_date);
			ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Common.Resources.ValidateEmptyDate, Settings.ProgramName, MessageBoxButton.OK, MessageBoxImage.Exclamation);
		}
	}

	protected override void StoreSettings(RegistryKey rk)
	{
		base.StoreSettings(rk);
		_ourCalcsGrid.StoreSettings(rk);
	}

	protected override void RestoreSettings(double defaultWidth, double defaultHeight, RegistryKey rk)
	{
		base.RestoreSettings(defaultWidth, defaultHeight, rk);
		_ourCalcsGrid.RestoreSettings(rk);
	}
}
