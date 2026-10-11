using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Ardalis.SmartEnum;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Extensions;
using ArmSoft.SME.Common.Models;
using ArmSoft.SME.Common.Models.Caches;
using ArmSoft.SME.Common.Models.Documents;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Core.UI.Controls;
using ArmSoft.SME.Core.UI.DataViews;
using ArmSoft.SME.Core.UI.Documents;
using ArmSoft.SME.Core.UI.DropDowns;
using ArmSoft.SME.Core.UI.Enums;
using ArmSoft.SME.Core.UI.Extensions;
using ArmSoft.SME.Core.UI.Helpers;
using ArmSoft.SME.Core.UI.PersistableObject;
using ArmSoft.SME.Trade.Common;
using ArmSoft.SME.Trade.Common.Documents.Definition;
using ArmSoft.SME.Trade.Common.Models.Caches;
using ArmSoft.SME.Trade.Common.Models.Documents;
using ArmSoft.SME.Trade.Common.Parameters;
using ArmSoft.SME.Trade.Common.SmartEnums;
using ArmSoft.SME.Trade.UI.CustomUIElements;
using ArmSoft.SME.Trade.UI.DropDown;
using ArmSoft.SME.Trade.UI.Helpers;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.ComponentModel.__Internals;
using DevExpress.Data.Extensions;
using DevExpress.Utils;
using DevExpress.Xpf.Bars;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Editors;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.LayoutControl;
using DevExpress.XtraEditors.DXErrorProvider;
using Microsoft.Win32;

namespace ArmSoft.SME.Trade.UI.Documents;

[DocumentWindow(36)]
public class MTMoveOrderWindow : TradeDocumentWindow<MTMoveOrder>
{
	public class MTMoveSpecificationRow : DocumentGridRow
	{
		[ObservableProperty]
		private int mTID = -1;

		[ObservableProperty]
		private string mTCode = "";

		[ObservableProperty]
		private string mTName = "";

		[ObservableProperty]
		private string unitCode = "";

		[ObservableProperty]
		private string unitBrief = "";

		[ObservableProperty]
		private decimal quantity;

		[ObservableProperty]
		private short rowNum;

		[ObservableProperty]
		private bool isPartySelected;

		[ObservableProperty]
		private int rowID;

		[ObservableProperty]
		private string rowDescription = "";

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public int MTID
		{
			get
			{
				return mTID;
			}
			set
			{
				if (!EqualityComparer<int>.Default.Equals(mTID, value))
				{
					OnPropertyChanging(__KnownINotifyPropertyChangingArgs.MTID);
					mTID = value;
					OnPropertyChanged(__KnownINotifyPropertyChangedArgs.MTID);
				}
			}
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public string MTCode
		{
			get
			{
				return mTCode;
			}
			[MemberNotNull("mTCode")]
			set
			{
				if (!EqualityComparer<string>.Default.Equals(mTCode, value))
				{
					OnPropertyChanging(__KnownINotifyPropertyChangingArgs.MTCode);
					mTCode = value;
					OnMTCodeChanged(value);
					OnPropertyChanged(__KnownINotifyPropertyChangedArgs.MTCode);
				}
			}
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public string MTName
		{
			get
			{
				return mTName;
			}
			[MemberNotNull("mTName")]
			set
			{
				if (!EqualityComparer<string>.Default.Equals(mTName, value))
				{
					OnPropertyChanging(__KnownINotifyPropertyChangingArgs.MTName);
					mTName = value;
					OnPropertyChanged(__KnownINotifyPropertyChangedArgs.MTName);
				}
			}
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public string UnitCode
		{
			get
			{
				return unitCode;
			}
			[MemberNotNull("unitCode")]
			set
			{
				if (!EqualityComparer<string>.Default.Equals(unitCode, value))
				{
					OnPropertyChanging(__KnownINotifyPropertyChangingArgs.UnitCode);
					unitCode = value;
					OnUnitCodeChanged(value);
					OnPropertyChanged(__KnownINotifyPropertyChangedArgs.UnitCode);
				}
			}
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public string UnitBrief
		{
			get
			{
				return unitBrief;
			}
			[MemberNotNull("unitBrief")]
			set
			{
				if (!EqualityComparer<string>.Default.Equals(unitBrief, value))
				{
					OnPropertyChanging(__KnownINotifyPropertyChangingArgs.UnitBrief);
					unitBrief = value;
					OnPropertyChanged(__KnownINotifyPropertyChangedArgs.UnitBrief);
				}
			}
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public decimal Quantity
		{
			get
			{
				return quantity;
			}
			set
			{
				if (!EqualityComparer<decimal>.Default.Equals(quantity, value))
				{
					OnPropertyChanging(__KnownINotifyPropertyChangingArgs.Quantity);
					quantity = value;
					OnQuantityChanged(value);
					OnPropertyChanged(__KnownINotifyPropertyChangedArgs.Quantity);
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
		public bool IsPartySelected
		{
			get
			{
				return isPartySelected;
			}
			set
			{
				if (!EqualityComparer<bool>.Default.Equals(isPartySelected, value))
				{
					OnPropertyChanging(__KnownINotifyPropertyChangingArgs.IsPartySelected);
					isPartySelected = value;
					OnPropertyChanged(__KnownINotifyPropertyChangedArgs.IsPartySelected);
				}
			}
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public int RowID
		{
			get
			{
				return rowID;
			}
			set
			{
				if (!EqualityComparer<int>.Default.Equals(rowID, value))
				{
					OnPropertyChanging(__KnownINotifyPropertyChangingArgs.RowID);
					rowID = value;
					OnPropertyChanged(__KnownINotifyPropertyChangedArgs.RowID);
				}
			}
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public string RowDescription
		{
			get
			{
				return rowDescription;
			}
			[MemberNotNull("rowDescription")]
			set
			{
				if (!EqualityComparer<string>.Default.Equals(rowDescription, value))
				{
					OnPropertyChanging(__KnownINotifyPropertyChangingArgs.RowDescription);
					rowDescription = value;
					OnPropertyChanged(__KnownINotifyPropertyChangedArgs.RowDescription);
				}
			}
		}

		public MTMoveSpecificationRow()
		{
		}

		public MTMoveSpecificationRow(MTMoveOrderWindow docWindow)
			: base(docWindow)
		{
		}

		static MTMoveSpecificationRow()
		{
			DocumentGridRow.RegisterPropertyValidators(typeof(MTMoveSpecificationRow), cacheValidationResults: true, new Dictionary<string, Action<DocumentGridRow, DevExpress.XtraEditors.DXErrorProvider.ErrorInfo>>
			{
				{
					"MTCode",
					(DocumentGridRow row, DevExpress.XtraEditors.DXErrorProvider.ErrorInfo info) =>
					{
						ValidateMTCode((MTMoveSpecificationRow)row, info);
					}
				},
				{
					"UnitCode",
					(DocumentGridRow row, DevExpress.XtraEditors.DXErrorProvider.ErrorInfo info) =>
					{
						ValidateUnitCode((MTMoveSpecificationRow)row, info);
					}
				},
				{
					"Quantity",
					(DocumentGridRow row, DevExpress.XtraEditors.DXErrorProvider.ErrorInfo info) =>
					{
						ValidateQuantity((MTMoveSpecificationRow)row, info);
					}
				}
			});
		}

		private static void ValidateMTCode(MTMoveSpecificationRow row, DevExpress.XtraEditors.DXErrorProvider.ErrorInfo info)
		{
			TradePropertyValidator.ValidateMaterialCode(row.MTCode, info, TradeAllDocDefinitions.MTMoveOrder.Grids["MTList"].Columns["MTCode"]);
		}

		private static void ValidateUnitCode(MTMoveSpecificationRow row, DevExpress.XtraEditors.DXErrorProvider.ErrorInfo info)
		{
			TradePropertyValidator.ValidateGoodUnitCode(row.UnitCode, row.MTCode, info, TradeAllDocDefinitions.MTMoveOrder.Grids["MTList"].Columns["UnitCode"]);
		}

		private static void ValidateQuantity(MTMoveSpecificationRow row, DevExpress.XtraEditors.DXErrorProvider.ErrorInfo info)
		{
			PropertyValidator.PositiveValidate(row.Quantity, info, ArmSoft.SME.Common.Resources.Quantity);
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		private void OnMTCodeChanged(string value)
		{
			ResetForceValidationBit("MTCode", "MTName", "UnitCode");
			MaterialCacheRow item = TradeSettings.TradeCacheService.Material.GetItem(MTCode);
			MTName = ((item == null) ? "" : item.Caption);
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		private void OnUnitCodeChanged(string value)
		{
			ResetForceValidationBit("UnitCode");
			QntUnitCacheRow item = TradeSettings.TradeCacheService.QntUnit.GetItem(UnitCode);
			UnitBrief = ((item == null) ? "" : item.Brief);
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		private void OnQuantityChanged(decimal value)
		{
			ResetForceValidationBit("Quantity");
		}
	}

	public class MTMoveOrderFilteredSearchList : IResultList
	{
		private readonly ObservableCollection<MTMoveSpecificationRow> _itemSource = new ObservableCollection<MTMoveSpecificationRow>();

		public IEnumerable<DocumentGridRow> ItemsSource => _itemSource;

		public List<FilteredSearchGridColumn> Columns => new List<FilteredSearchGridColumn>
		{
			new FilteredSearchGridColumn
			{
				Header = ArmSoft.SME.Common.Resources.Code,
				FieldName = "MTCode",
				CustomWidth = 125.0
			},
			new FilteredSearchGridColumn
			{
				Header = ArmSoft.SME.Common.Resources.Name,
				FieldName = "MTName",
				CustomWidth = 300.0
			},
			new FilteredSearchGridColumn
			{
				Header = ArmSoft.SME.Common.Resources.Quantity,
				FieldName = "Quantity",
				AllowEditing = true,
				NumericFormat = DecimalExtension.GetCurrencyFormat(13, 3)
			}
		};

		public int AddToItemsSource(object key, params object[] args)
		{
			string MTCode = key.ToString();
			decimal num = Convert.ToDecimal(args[0]);
			int num2 = _itemSource.FindIndex((MTMoveSpecificationRow x) => x.MTCode == MTCode);
			if (num2 == -1)
			{
				MaterialCacheRow item = TradeSettings.TradeCacheService.Material.GetItem(MTCode);
				MTMoveSpecificationRow item2 = new MTMoveSpecificationRow
				{
					MTID = item.Id,
					MTCode = item.MTCode,
					MTName = item.Caption,
					UnitCode = item.Unit
				};
				_itemSource.Add(item2);
				num2 = _itemSource.Count - 1;
			}
			_itemSource[num2].Quantity += num;
			return num2;
		}
	}

	private DateEditControl _supplyDate;

	private DropDownControl _storageExpense;

	private DropDownControl _storageIncome;

	private TextEditExt _barCode;

	private Button _codeEnter;

	private DropDownControl _owner;

	private CheckEditExt _isMade;

	private DocumentGridControl _mtMoveOrderGrid;

	private TableView _mtMoveOrderTableView;

	private BarEditItemExt _filteredSearch;

	private ObservableCollection<MTMoveSpecificationRow> _mtMoveOrderList;

	private DocumentGridColumn _mtCodeCol;

	private DocumentGridColumn _unitCodeCol;

	private bool _storageExpenseIsRetail;

	private bool _storageIncomeIsRetail;

	private BarButtonItem _barButtonImportFromFile;

	private BarButtonItem _barButtonPrintLabel;

	private ReportBarButtonItem _barButtonJumpTo;

	private BarSubItem _barSubItemJournals = new BarSubItem();

	private BarSubItem _barSubItemActions = new BarSubItem();

	private int _maxRowID;

	protected override double WidthValue => 890.0;

	protected override double HeightValue => 480.0;

	protected override double MinWidthValue => 890.0;

	protected override double MinHeightValue => 250.0;

	public MTMoveOrderWindow()
	{
	}

	public MTMoveOrderWindow(MTMoveOrder doc)
		: base(doc)
	{
	}

	protected override void ConstructUI()
	{
		base.ConstructUI();
		Storages idv = new Storages();
		LayoutGroupExt container = AddLayoutGroup(null, LayoutGroupView.Group, Orientation.Horizontal);
		_date = AddDateControl(container, Definition.Fields["DocDate"]);
		_docNum = AddButtonEditControl(container, Definition.Fields["DocNum"], 1);
		_docNum.AllowMagicButton = true;
		_docNum.MagicToolTip = ArmSoft.SME.Common.Resources.DocNextNumbers;
		_docNum.MagicClick += new SafeDocumentEventHandler<MagicClickEventArgs>(DocNum_MagicClick, this).Handler;
		_docNum.EditValueChanged += new SafeDocumentEventHandler<EditValueChangedEventArgs>(DocNum_EditValueChanged, this).Handler;
		_supplyDate = AddDateControl(null, Definition.Fields["SupplyDate"]);
		_storageExpense = AddDropDownControl(null, Definition.Fields["StorageExpense"], idv, allowAdditionButtons: false, fieldWithCaption: true, HorizontalAlignment.Stretch);
		_storageExpense.EditValueChanged += new SafeDocumentEventHandler<EditValueChangedEventArgs>(StorageExpense_EditValueChanged, this).Handler;
		_storageIncome = AddDropDownControl(null, Definition.Fields["StorageIncome"], idv, allowAdditionButtons: false, fieldWithCaption: true, HorizontalAlignment.Stretch);
		_storageIncome.EditValueChanged += new SafeDocumentEventHandler<EditValueChangedEventArgs>(StorageIncome_EditValueChanged, this).Handler;
		LayoutGroupExt container2 = AddLayoutGroup(null, LayoutGroupView.Group, Orientation.Horizontal);
		_barCode = AddStringControl(container2, ArmSoft.SME.Common.Resources.BarCode + " (F2)", isRequired: false, 20, HorizontalAlignment.Stretch);
		_barCode.NullText = ArmSoft.SME.Trade.Common.Resources.BarCodeAndCodeInputNullText;
		_barCode.KeyDown += new SafeDocumentEventHandler<KeyEventArgs>(BarCode_KeyDown, this).Handler;
		string text = ArmSoft.SME.Trade.Common.Resources.cmdCodeEnter + " (F8)";
		_codeEnter = AddButton(container2, null, text, null, text, isTabStop: true);
		_codeEnter.Margin = new Thickness(0.0, 0.0, 18.0, 0.0);
		_codeEnter.Click += new SafeDocumentEventHandler<RoutedEventArgs>(CodeEnter_Click, this).Handler;
		DXTabControl tc = AddTabControl(null);
		LayoutGroup container3 = AddTabbedGroup(tc, ArmSoft.SME.Common.Resources.General, "General");
		IEnumerable<string> itemsSource = from p in TradeSettings.TradeCacheService.StandardComments.GetList()
			select p.Name;
		AddComboBoxControl(container3, Definition.Fields["Comment"], itemsSource, HorizontalAlignment.Stretch);
		LayoutGroupExt container4 = AddLayoutGroup(container3, LayoutGroupView.Group, Orientation.Horizontal);
		_owner = AddDropDownControl(container4, Definition.Fields["Owner"], new UsersFilter());
		_owner.CodeWidth = 12;
		_owner.ShowComment = false;
		_owner.InnerCodeEdit.Width = _owner.InnerCodeEdit.MinWidth;
		if (!Settings.LoginService.IsAdmin && Settings.ParametersService.DenyAccessToOtherOwnerDocs())
		{
			((LayoutItemWithRequired)(object)_owner.Parent).Visibility = Visibility.Collapsed;
		}
		_isMade = AddBooleanControl(container4, Definition.Fields["IsMade"], HorizontalAlignment.Right);
		_isMade.Margin = new Thickness(0.0, 0.0, 20.0, 0.0);
		_mtMoveOrderGrid = AddGrid(container3, Definition.Grids["MTList"], HorizontalAlignment.Stretch, VerticalAlignment.Stretch);
		_mtMoveOrderGrid.AddRowIndicator(35);
		_filteredSearch = _mtMoveOrderGrid.AddButton(null, ArmSoft.SME.Trade.Common.Resources.GroupAddition, ArmSoft.SME.Trade.Common.Resources.GroupAddition, 130.0);
		_filteredSearch.Click += new SafeDocumentEventHandler<RoutedEventArgs>(FilteredSearch_Click, this).Handler;
		_mtMoveOrderTableView = _mtMoveOrderGrid.View;
		_mtMoveOrderGrid.IsRequired = true;
		_mtMoveOrderGrid.ToolTip = ArmSoft.SME.Trade.Common.Resources.MTLIST;
		_mtMoveOrderGrid.AllowMoveUp = true;
		_mtMoveOrderGrid.AllowMoveDown = true;
		_mtCodeCol = _mtMoveOrderGrid.AddDropDownColumn(Definition.Grids["MTList"].Columns["MTCode"], new GoodsFilter(shortMode: true, GoodsTypesSE.Good, "", isShowOnlyInPriceList: true), allowAdditionButtons: true, isOwnProcessingClickDropDown: true);
		_mtCodeCol.Width = 125.0;
		DocumentGridColumn documentGridColumn = _mtMoveOrderGrid.AddStringColumn(Definition.Grids["MTList"].Columns["MTName"]);
		documentGridColumn.AllowEditing = DefaultBoolean.False;
		documentGridColumn.Width = 225.0;
		_unitCodeCol = _mtMoveOrderGrid.AddDropDownColumn(Definition.Grids["MTList"].Columns["UnitCode"], new MTQntUnits());
		_mtMoveOrderGrid.AddStringColumn(Definition.Grids["MTList"].Columns["UnitBrief"]).AllowEditing = DefaultBoolean.False;
		_mtMoveOrderGrid.AddNumberColumn(Definition.Grids["MTList"].Columns["Quantity"], showTotal: true);
		_mtMoveOrderGrid.AddStringColumn(Definition.Grids["MTList"].Columns["RowDescription"]);
		_mtMoveOrderList = new ObservableCollection<MTMoveSpecificationRow>();
		_mtMoveOrderGrid.AddingRow += new SafeDocumentEventHandler<DocumentGridAddingRowEventArgs>(MTMoveOrderGrid_AddingRow, this).Handler;
		_mtMoveOrderGrid.DuplicatingRow += new SafeDocumentEventHandler<DocumentGridDuplicatingRowEventArgs>(MTMoveOrderGrid_DuplicatingRow, this).Handler;
		_mtMoveOrderGrid.MovingRowUp += new SafeDocumentEventHandler<DocumentGridMovingRowUpOrDownEventArgs>(MTMoveOrderGrid_MovingRowUp, this).Handler;
		_mtMoveOrderGrid.MovingRowDown += new SafeDocumentEventHandler<DocumentGridMovingRowUpOrDownEventArgs>(MTMoveOrderGrid_MovingRowDown, this).Handler;
		_mtMoveOrderGrid.ClickDropDown += new SafeDocumentEventHandler<DocumentGridClickDropDownEventArgs>(MTMoveOrderGrid_ClickDropDown, this).Handler;
		_mtMoveOrderGrid.RefreshButtons += new SafeDocumentEventHandler<EventArgs>(MTMoveOrderGrid_RefreshButtons, this).Handler;
		_mtMoveOrderTableView.FocusedRowHandleChanged += new SafeDocumentEventHandler<FocusedRowHandleChangedEventArgs>(MTMoveOrderTableView_FocusedRowHandleChanged, this).Handler;
		_mtMoveOrderTableView.CellValueChanged += new SafeDocumentEventHandler<CellValueChangedEventArgs>(MTMoveOrderTableView_CellValueChanged, this).Handler;
		foreach (GridColumn column in _mtMoveOrderGrid.Grid.Columns)
		{
			column.AllowSorting = DefaultBoolean.True;
			column.AllowColumnFiltering = DefaultBoolean.True;
			column.FilterPopupMode = FilterPopupMode.CheckedList;
		}
	}

	protected override void OnNew()
	{
		base.OnNew();
		_date.EditValue = Settings.HostingEnvironment.CurrentDate;
		_supplyDate.EditValue = Settings.HostingEnvironment.CurrentDate;
		_owner.Code = Settings.LoginService.UserName;
		if (!string.IsNullOrWhiteSpace(PO.StorageIncome))
		{
			_storageIncomeIsRetail = TradeSettings.TradeCacheService.Storage.GetItem(PO.StorageIncome)?.Retail ?? false;
		}
		_docNum.EditValue = Settings.ParametersService.MTMoveOrderNumber();
	}

	protected override void OnNewAfterSave()
	{
		PO = new MTMoveOrder();
		base.OnNewAfterSave();
		_docNum.EditValue = Settings.ParametersService.MTMoveOrderNumber();
		_mtMoveOrderList.Clear();
		_maxRowID = 0;
		_owner.Code = Settings.LoginService.UserName;
		SetReportBarButtonsEnable(isEnabled: false);
	}

	protected override void OnEdit()
	{
		base.OnEdit();
		if (!string.IsNullOrWhiteSpace(_storageExpense.Code))
		{
			_storageExpenseIsRetail = TradeSettings.TradeCacheService.Storage.GetItem(_storageExpense.Code)?.Retail ?? false;
		}
		if (!string.IsNullOrWhiteSpace(_storageIncome.Code))
		{
			_storageIncomeIsRetail = TradeSettings.TradeCacheService.Storage.GetItem(_storageIncome.Code)?.Retail ?? false;
		}
		_setFocusOnActivation = _docNum;
	}

	protected override void LoadDataToControls()
	{
		base.LoadDataToControls();
		_mtMoveOrderList.Clear();
		_mtMoveOrderGrid.ItemsSource = _mtMoveOrderList;
		foreach (MTMoveBase.MTMoveSpecificationRow mT in PO.MTList)
		{
			_mtMoveOrderList.Add(new MTMoveSpecificationRow
			{
				MTID = mT.MTID,
				MTCode = mT.MTCode,
				MTName = mT.MTName,
				UnitCode = mT.UnitCode,
				UnitBrief = mT.UnitBrief,
				Quantity = mT.Quantity,
				RowNum = mT.RowNum,
				RowID = mT.RowID,
				RowDescription = mT.RowDescription
			});
		}
		_maxRowID = PO.MaxRowID;
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

	protected override void FillDocument(StoreRequest sr)
	{
		base.FillDocument(sr);
		PO.MTList.Clear();
		foreach (MTMoveSpecificationRow mtMoveOrder in _mtMoveOrderList)
		{
			MTMoveBase.MTMoveSpecificationRow item = new MTMoveBase.MTMoveSpecificationRow
			{
				MTID = mtMoveOrder.MTID,
				MTCode = mtMoveOrder.MTCode,
				MTName = mtMoveOrder.MTName,
				UnitCode = mtMoveOrder.UnitCode,
				UnitBrief = mtMoveOrder.UnitBrief,
				Quantity = mtMoveOrder.Quantity,
				RowNum = mtMoveOrder.RowNum,
				RowID = mtMoveOrder.RowID,
				RowDescription = mtMoveOrder.RowDescription
			};
			PO.MTList.Add(item);
		}
		PO.MaxRowID = _maxRowID;
	}

	protected override void SetControlsReadonly()
	{
		base.SetControlsReadonly();
		if (_viewMode == WindowViewMode.View)
		{
			return;
		}
		bool flag = State == DocumentStateSE.Draft.Value || State == DocumentStateSE.Deleted.Value;
		bool flag2 = _viewMode == WindowViewMode.Edit && !flag;
		_date.IsReadOnly = flag2;
		_supplyDate.IsReadOnly = flag2;
		_storageIncome.IsReadOnly = flag2;
		_storageExpense.IsReadOnly = flag2;
		_barCode.IsReadOnly = flag2;
		_codeEnter.IsEnabled = !flag2;
		foreach (GridColumn column in _mtMoveOrderGrid.Grid.Columns)
		{
			column.ReadOnly = flag2;
		}
		_mtMoveOrderGrid.AllowAdd = !flag2;
		_mtMoveOrderGrid.AllowEdit = !flag2;
		_mtMoveOrderGrid.AllowDelete = !flag2;
		_mtMoveOrderGrid.AllowDuplicate = !flag2;
		_mtMoveOrderGrid.AllowMoveUp = !flag2;
		_mtMoveOrderGrid.AllowMoveDown = !flag2;
	}

	private void MTMoveOrderGrid_AddingRow(object sender, DocumentGridAddingRowEventArgs e)
	{
		ValidateDocFields();
		if (string.IsNullOrWhiteSpace(_storageExpense.Code))
		{
			ControlsHelper.SetFocus(_storageExpense);
			throw new ASException(ArmSoft.SME.Common.Resources.ValidEmptyExpenseStorage);
		}
		_mtMoveOrderList.Add(new MTMoveSpecificationRow
		{
			RowID = _maxRowID
		});
		_maxRowID++;
		e.RowNumber = _mtMoveOrderList.Count - 1;
	}

	private void MTMoveOrderGrid_DuplicatingRow(object sender, DocumentGridDuplicatingRowEventArgs e)
	{
		MTMoveSpecificationRow mTMoveSpecificationRow = (MTMoveSpecificationRow)_mtMoveOrderGrid.Grid.GetRow(e.RowHandle);
		if (mTMoveSpecificationRow != null)
		{
			if (!_date.DoValidate())
			{
				ControlsHelper.SetFocus(_date);
				throw new ASException(ArmSoft.SME.Common.Resources.ValidateEmptyDate);
			}
			if (string.IsNullOrWhiteSpace(_storageExpense.Code))
			{
				ControlsHelper.SetFocus(_storageExpense);
				throw new ASException(ArmSoft.SME.Common.Resources.ValidEmptyExpenseStorage);
			}
			MTMoveSpecificationRow mTMoveSpecificationRow2 = new MTMoveSpecificationRow();
			mTMoveSpecificationRow.Clone(mTMoveSpecificationRow2);
			mTMoveSpecificationRow2.RowID = _maxRowID;
			_mtMoveOrderList.Add(mTMoveSpecificationRow2);
			_maxRowID++;
			e.NewRowNumber = _mtMoveOrderList.Count - 1;
		}
	}

	private void MTMoveOrderGrid_MovingRowUp(object sender, DocumentGridMovingRowUpOrDownEventArgs e)
	{
		if (_mtMoveOrderGrid.Grid.SortInfo.Count > 0 || _mtMoveOrderTableView.Grid.VisibleRowCount != _mtMoveOrderList.Count)
		{
			throw new ASException(ArmSoft.SME.Common.Resources.NotAllowRowMoveInSortedGrid);
		}
		MTMoveSpecificationRow mTMoveSpecificationRow = (MTMoveSpecificationRow)_mtMoveOrderGrid.Grid.GetRow(e.RowHandle);
		if (mTMoveSpecificationRow != null)
		{
			_mtMoveOrderList.RemoveAt(e.RowHandle);
			_mtMoveOrderList.Insert(e.RowHandle - 1, mTMoveSpecificationRow);
		}
	}

	private void MTMoveOrderGrid_MovingRowDown(object sender, DocumentGridMovingRowUpOrDownEventArgs e)
	{
		if (_mtMoveOrderGrid.Grid.SortInfo.Count > 0 || _mtMoveOrderTableView.Grid.VisibleRowCount != _mtMoveOrderList.Count)
		{
			throw new ASException(ArmSoft.SME.Common.Resources.NotAllowRowMoveInSortedGrid);
		}
		MTMoveSpecificationRow mTMoveSpecificationRow = (MTMoveSpecificationRow)_mtMoveOrderGrid.Grid.GetRow(e.RowHandle);
		if (mTMoveSpecificationRow != null)
		{
			_mtMoveOrderList.RemoveAt(e.RowHandle);
			_mtMoveOrderList.Insert(e.RowHandle + 1, mTMoveSpecificationRow);
		}
	}

	private void MTMoveOrderTableView_FocusedRowHandleChanged(object sender, FocusedRowHandleChangedEventArgs e)
	{
		int focusedRowHandle = _mtMoveOrderTableView.FocusedRowHandle;
		MTMoveSpecificationRow mTMoveSpecificationRow = (MTMoveSpecificationRow)_mtMoveOrderTableView.Grid.GetRow(focusedRowHandle);
		if (mTMoveSpecificationRow != null)
		{
			ChangeColumnAdditonalView(mTMoveSpecificationRow.MTCode);
		}
	}

	private void ChangeColumnAdditonalView(string ItemCode)
	{
		_unitCodeCol.DataView = new MTQntUnits(string.IsNullOrWhiteSpace(ItemCode) ? "" : ItemCode);
	}

	private void ValidateDocFields()
	{
		ValidateInnerDocFields();
		if (string.IsNullOrWhiteSpace(_storageIncome.Code))
		{
			ControlsHelper.SetFocus(_storageIncome);
			throw new ASException(string.Format(ArmSoft.SME.Common.Resources.FieldIsEmpty, ArmSoft.SME.Trade.Common.Resources.StorageIn));
		}
		if (!_storageIncome.DoValidate())
		{
			ControlsHelper.SetFocus(_storageIncome);
			throw new ASException(string.Format(ArmSoft.SME.Trade.Common.Resources.StorageNotExist, _storageIncome.Code));
		}
	}

	private void ValidateInnerDocFields()
	{
		if (!_date.DoValidate())
		{
			ControlsHelper.SetFocus(_date);
			throw new ASException(ArmSoft.SME.Common.Resources.ValidateEmptyDate);
		}
		if (string.IsNullOrWhiteSpace(_storageExpense.Code))
		{
			ControlsHelper.SetFocus(_storageExpense);
			throw new ASException(ArmSoft.SME.Common.Resources.ValidEmptyExpenseStorage);
		}
		if (!_storageExpense.DoValidate())
		{
			ControlsHelper.SetFocus(_storageExpense);
			throw new ASException(string.Format(ArmSoft.SME.Trade.Common.Resources.StorageNotExist, _storageExpense.Code));
		}
	}

	private void MTMoveOrderGrid_ClickDropDown(object sender, DocumentGridClickDropDownEventArgs e)
	{
		MTMoveSpecificationRow mTMoveSpecificationRow = (MTMoveSpecificationRow)_mtMoveOrderGrid.Grid.GetRow(e.RowHandle);
		if (e.Column == _mtCodeCol)
		{
			ValidateInnerDocFields();
			bool showAll = TradeSettings.CreateTradeClient().CommonServices.IsNegativExpAllow(_storageExpense.Code);
			GoodsAvailabilityFilter goodsAvailabilityFilter = new GoodsAvailabilityFilter(_date.EditValue.Value, _storageExpense.Code, _storageExpenseIsRetail, includeServices: false, showAll);
			ModalBrowser modalBrowser = new ModalBrowser(goodsAvailabilityFilter)
			{
				LookUpValue = e.Value,
				Owner = e.Owner,
				Left = e.Left,
				Top = e.Top
			};
			if (modalBrowser.Show())
			{
				e.NewValue = goodsAvailabilityFilter.ValueMember(modalBrowser.SelectedRowIndex);
			}
		}
		else if (e.Column == _unitCodeCol && mTMoveSpecificationRow != null)
		{
			MTQntUnits mTQntUnits = new MTQntUnits(mTMoveSpecificationRow.MTCode);
			ModalBrowser modalBrowser2 = new ModalBrowser(mTQntUnits)
			{
				LookUpValue = e.Value,
				Owner = e.Owner,
				Left = e.Left,
				Top = e.Top
			};
			if (modalBrowser2.Show())
			{
				e.NewValue = mTQntUnits.ValueMember(modalBrowser2.SelectedRowIndex);
			}
		}
		_mtMoveOrderGrid.Grid.RefreshData();
	}

	private void MTMoveOrderGrid_RefreshButtons(object sender, EventArgs e)
	{
		_filteredSearch.IsEnabled = _mtMoveOrderGrid.AllowAdd && !_mtMoveOrderGrid.IsReadOnly;
	}

	private void MTMoveOrderTableView_CellValueChanged(object sender, CellValueChangedEventArgs e)
	{
		MTMoveSpecificationRow mTMoveSpecificationRow = (MTMoveSpecificationRow)_mtMoveOrderGrid.Grid.GetRow(e.RowHandle);
		if (mTMoveSpecificationRow == null || e.Column != _mtCodeCol)
		{
			return;
		}
		try
		{
			if (e.Value == null || string.IsNullOrWhiteSpace(e.Value.ToString()))
			{
				mTMoveSpecificationRow.MTID = -1;
				mTMoveSpecificationRow.MTCode = "";
				mTMoveSpecificationRow.MTName = "";
				mTMoveSpecificationRow.UnitCode = "";
				mTMoveSpecificationRow.RowDescription = "";
				return;
			}
			string text = e.Value.ToString();
			foreach (MTMoveSpecificationRow mtMoveOrder in _mtMoveOrderList)
			{
				if (mTMoveSpecificationRow != mtMoveOrder && text == mtMoveOrder.MTCode)
				{
					if (ArmSoft.SME.Core.UI.MessageBox.Show(string.Format(ArmSoft.SME.Trade.Common.Resources.MTExistInMTList, text), Settings.ProgramName, MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.Yes) == MessageBoxResult.No)
					{
						mTMoveSpecificationRow.MTCode = "";
						return;
					}
					break;
				}
			}
			if (!string.IsNullOrWhiteSpace(text))
			{
				MaterialCacheRow item = TradeSettings.TradeCacheService.Material.GetItem(text, throwExceptionNotExits: true);
				mTMoveSpecificationRow.MTID = item.Id;
				mTMoveSpecificationRow.MTName = item.Caption;
				mTMoveSpecificationRow.UnitCode = item.Unit;
				_unitCodeCol.DataView = new MTQntUnits(item.MTCode);
				return;
			}
			throw new Exception();
		}
		catch (Exception ex)
		{
			mTMoveSpecificationRow.MTID = -1;
			mTMoveSpecificationRow.MTCode = "";
			mTMoveSpecificationRow.MTName = "";
			mTMoveSpecificationRow.UnitCode = "";
			mTMoveSpecificationRow.RowDescription = "";
			mTMoveSpecificationRow.Quantity = 0m;
			if (e.Value != null && !string.IsNullOrWhiteSpace(e.Value.ToString()))
			{
				Settings.ErrorHandler(ex);
			}
		}
	}

	private void StorageExpense_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		if (!_storageExpense.IsReadOnly && !string.IsNullOrWhiteSpace(Convert.ToString(e.NewValue)) && TradeSettings.TradeCacheService.Storage.GetItem(Convert.ToString(e.NewValue)) != null && !TradeSettings.CreateTradeClient().CommonServices.IsStorageAvailable(Convert.ToString(e.NewValue)))
		{
			e.Handled = true;
			_storageExpense.Code = Convert.ToString(e.OldValue);
			ArmSoft.SME.Core.UI.MessageBox.Show(string.Format(ArmSoft.SME.Trade.Common.Resources.StorageNotAvailForUser, Settings.LoginService.UserName, Convert.ToString(e.NewValue)), Settings.ProgramName, MessageBoxButton.OK, MessageBoxImage.Exclamation);
		}
		else if (Convert.ToString(e.NewValue).TrimEnd() != Convert.ToString(e.OldValue).TrimEnd() && _storageExpense.DoValidate())
		{
			if (_storageExpense.Code == _storageIncome.Code.TrimEnd())
			{
				ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Trade.Common.Resources.MTMoveErrSameStorages, Settings.ProgramName, MessageBoxButton.OK, MessageBoxImage.Exclamation);
				_storageExpense.Code = "";
			}
			else
			{
				_storageExpenseIsRetail = TradeSettings.TradeCacheService.Storage.GetItem(_storageExpense.Code)?.Retail ?? false;
			}
		}
	}

	private void StorageIncome_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		if (Convert.ToString(e.NewValue).TrimEnd() != Convert.ToString(e.OldValue).TrimEnd() && _storageIncome.DoValidate())
		{
			if (_storageIncome.Code == _storageExpense.Code.TrimEnd())
			{
				ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Trade.Common.Resources.MTMoveErrSameStorages, Settings.ProgramName, MessageBoxButton.OK, MessageBoxImage.Exclamation);
				_storageIncome.Code = "";
			}
			else
			{
				_storageIncomeIsRetail = TradeSettings.TradeCacheService.Storage.GetItem(_storageIncome.Code)?.Retail ?? false;
			}
		}
	}

	public override void OnScanner(string scannerData)
	{
		if (!_barCode.IsReadOnly)
		{
			_barCode.Text = scannerData;
			ScannerAdd();
		}
	}

	protected override void OnF2()
	{
		if (!_barCode.IsReadOnly)
		{
			ControlsHelper.SetFocus(_barCode);
		}
	}

	protected override void OnF8()
	{
		OnCodeEnterClick();
	}

	private void CodeEnter_Click(object sender, RoutedEventArgs e)
	{
		OnCodeEnterClick();
	}

	private void BarCode_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Invalid comparison between Unknown and I4
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		if ((int)e.Key == 6 && (int)Keyboard.Modifiers == 0 && !_barCode.IsReadOnly && !string.IsNullOrWhiteSpace(_barCode.Text))
		{
			ScannerAdd();
		}
	}

	private void OnCodeEnterClick()
	{
		if (_codeEnter == null || !_codeEnter.IsEnabled)
		{
			return;
		}
		_barCode.DoValidate();
		string text = _barCode.Text.Trim();
		if (string.IsNullOrWhiteSpace(text))
		{
			if (!_date.DoValidate() || !_date.EditValue.HasValue)
			{
				ControlsHelper.SetFocus(_date);
				throw new ASException(ArmSoft.SME.Common.Resources.ValidateEmptyDate);
			}
			GoodsFilter goodsFilter = new GoodsFilter(shortMode: true, ItemTypesSE.Goods.Value);
			ModalBrowser modalBrowser = new ModalBrowser(goodsFilter)
			{
				Owner = this,
				WindowStartupLocation = WindowStartupLocation.CenterOwner
			};
			if (!modalBrowser.Show())
			{
				_barCode.Text = "";
				ControlsHelper.SetFocus(_barCode);
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
			ControlsHelper.SetFocus(_barCode);
			throw new ASException(string.Format(ArmSoft.SME.Trade.Common.Resources.MTEgineInavidMaterial, text));
		}
		AddMTToList(item);
		_barCode.Text = "";
	}

	private void ScannerAdd()
	{
		_barCode.DoValidate();
		if (string.IsNullOrWhiteSpace(_storageExpense.Code))
		{
			ControlsHelper.SetFocus(_storageExpense);
			throw new ASException(string.Format(ArmSoft.SME.Common.Resources.FieldIsEmpty, ArmSoft.SME.Common.Resources.Storage));
		}
		if (!_storageExpense.DoValidate())
		{
			ControlsHelper.SetFocus(_storageExpense);
			throw new ASException(string.Format(ArmSoft.SME.Trade.Common.Resources.StorageNotExist, _storageExpense.Code));
		}
		string barCode = _barCode.Text.Trim();
		MaterialCacheRow materialCacheRow = DocumentHelpers.TryGetItemByBarCode(ref barCode, out var qty, out var unitCode, out var unitBrief);
		if (materialCacheRow == null)
		{
			ControlsHelper.SetFocus(_barCode);
			throw new ASException(string.Format(ArmSoft.SME.Common.Resources.ErrBarCodeNotExist, _barCode.Text.Trim()));
		}
		AddMTToList(materialCacheRow, unitCode, unitBrief, qty);
		_barCode.Text = "";
	}

	private void AddMTToList(MaterialCacheRow goodsRow, string unitCode = "", string unitBrief = "", decimal quantity = 1m)
	{
		if (Settings.ParametersService.ScannerMode() == ScanerModesSE.AddNewRow)
		{
			AddMTToListNewRow(goodsRow, string.IsNullOrWhiteSpace(unitCode) ? goodsRow.Unit : unitCode, string.IsNullOrWhiteSpace(unitBrief) ? goodsRow.UnitRow.Brief : unitBrief, quantity);
		}
		else
		{
			AddNewRowOrMergeInner(goodsRow, Settings.ParametersService.ScannerMode() == ScanerModesSE.MergeWithCurrentRow, string.IsNullOrWhiteSpace(unitCode) ? goodsRow.Unit : unitCode, string.IsNullOrWhiteSpace(unitBrief) ? goodsRow.UnitRow.Brief : unitBrief, quantity);
		}
	}

	private void AddNewRowOrMergeInner(MaterialCacheRow goodsRow, bool forFocusedRow, string unitCode, string unitBrief, decimal quantity = 1m)
	{
		MTMoveSpecificationRow mTMoveSpecificationRow = null;
		if (forFocusedRow && _mtMoveOrderList.Count != 0)
		{
			mTMoveSpecificationRow = (MTMoveSpecificationRow)_mtMoveOrderTableView.Grid.CurrentItem;
			if (mTMoveSpecificationRow == null)
			{
				return;
			}
		}
		for (int i = 0; i <= _mtMoveOrderList.Count - 1; i++)
		{
			if ((!forFocusedRow || mTMoveSpecificationRow.RowID == _mtMoveOrderList[i].RowID) && goodsRow.Id == _mtMoveOrderList[i].MTID && unitCode == _mtMoveOrderList[i].UnitCode)
			{
				FillMTInvGridRow(_mtMoveOrderList[i], goodsRow, unitCode, unitBrief, quantity);
				int rowHandleByListIndex = _mtMoveOrderGrid.Grid.GetRowHandleByListIndex(i);
				_mtMoveOrderGrid.Grid.RefreshRow(rowHandleByListIndex);
				POWindow<MTMoveOrder>.SelectCellInGrid(_mtMoveOrderTableView, _mtMoveOrderGrid, rowHandleByListIndex, "Quantity");
				return;
			}
		}
		AddMTToListNewRow(goodsRow, unitCode, unitBrief, quantity);
	}

	private void AddMTToListNewRow(MaterialCacheRow goodsRow, string unitCode, string unitBrief, decimal quantity = 1m)
	{
		MTMoveSpecificationRow mTMoveSpecificationRow = new MTMoveSpecificationRow();
		FillMTInvGridRow(mTMoveSpecificationRow, goodsRow, unitCode, unitBrief, quantity);
		mTMoveSpecificationRow.RowID = _maxRowID;
		_mtMoveOrderList.Add(mTMoveSpecificationRow);
		_maxRowID++;
		int rowHandleByListIndex = _mtMoveOrderGrid.Grid.GetRowHandleByListIndex(_mtMoveOrderList.Count - 1);
		_mtMoveOrderGrid.Grid.RefreshRow(rowHandleByListIndex);
		POWindow<MTMoveOrder>.SelectCellInGrid(_mtMoveOrderTableView, _mtMoveOrderGrid, rowHandleByListIndex, "Quantity");
		_mtMoveOrderGrid.ConfigureButtons();
	}

	private void FillMTInvGridRow(MTMoveSpecificationRow mtMoveGridRow, MaterialCacheRow goodsRow, string unitCode, string unitBrief, decimal quantity = 1m)
	{
		if (goodsRow != null)
		{
			mtMoveGridRow.MTID = goodsRow.Id;
			mtMoveGridRow.MTCode = goodsRow.MTCode;
			mtMoveGridRow.MTName = goodsRow.Caption;
			mtMoveGridRow.UnitCode = unitCode;
			mtMoveGridRow.UnitBrief = unitBrief;
			mtMoveGridRow.Quantity += quantity;
		}
	}

	private void FilteredSearch_Click(object sender, RoutedEventArgs e)
	{
		ValidateDocFields();
		if (string.IsNullOrWhiteSpace(_storageExpense.Code))
		{
			ControlsHelper.SetFocus(_storageExpense);
			throw new ASException(ArmSoft.SME.Common.Resources.ValidEmptyExpenseStorage);
		}
		FilteredSearchWindow filteredSearchWindow = new FilteredSearchWindow(new FilteredSearch
		{
			FilterDataView = new GoodsGroups(showAsTree: true, filteredSearch: true),
			SearchDataView = new GoodsFilter(shortMode: true, ItemTypesSE.Goods.Value),
			ResultsList = new MTMoveOrderFilteredSearchList()
		}, this, isBasketMode: true);
		if (!filteredSearchWindow.ShowDialog().Value)
		{
			return;
		}
		foreach (MTMoveSpecificationRow item in filteredSearchWindow.BasketResult)
		{
			item.RowID = _maxRowID;
			_mtMoveOrderList.Add(item);
			_maxRowID++;
		}
	}

	protected override void CreateChildDocument(short childDocType, MTMoveOrder baseDoc)
	{
		base.CreateChildDocument(childDocType, baseDoc);
		if (childDocType == DocumentTypeSE.MTPurchaseOrder.Value)
		{
			CreateBasedOnDocHelper.CreateNewMTPODocCommon(PO);
		}
		else
		{
			CreateBasedOnDocHelper.CreateDocument((DocumentTypeSE)(SmartEnum<DocumentTypeSE, short>)childDocType, PO.ISN, DVPID);
		}
	}

	protected override bool AllowPrint()
	{
		return true;
	}

	protected override bool AllowPrintOnNew()
	{
		return true;
	}

	protected override void AddToolbarItems()
	{
		base.AddToolbarItems();
		Ribbon.ToolbarItemLinks.Add(_barButtonJumpTo.ConvertForToolbar("t_doc_JumpTo_16.svg"));
		Ribbon.ToolbarItemLinks.Add(_barButtonImportFromFile.ConvertForToolbar("t_import_16.svg"));
		Ribbon.ToolbarItemLinks.Add(_barButtonPrintLabel.ConvertForToolbar("t_action_16.svg"));
	}

	protected override void AddItemsToRibbon()
	{
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		//IL_012d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0133: Unknown result type (might be due to invalid IL or missing references)
		//IL_0149: Unknown result type (might be due to invalid IL or missing references)
		//IL_014f: Unknown result type (might be due to invalid IL or missing references)
		base.AddItemsToRibbon();
		CreateRibbonPageGroupActions();
		CreateRibbonPageGroupReports();
		_barButtonJumpTo = new ReportBarButtonItem(DataProviderTypeSE.AllDocumentsJournal, showDialog: false, ArmSoft.SME.Common.Resources.JumpTo, null, "doc_JumpTo_16.svg", "doc_JumpTo_32.svg");
		_barButtonJumpTo.AddInBarSubItem(ref _ribbonPageGroupActions, ref _barSubItemActions);
		_barButtonImportFromFile = new BarButtonItem
		{
			Content = ArmSoft.SME.Trade.Common.Resources.Import,
			KeyGesture = new KeyGesture((Key)52, (ModifierKeys)2),
			Glyph = ImageService.GetImage("Import_16.svg"),
			LargeGlyph = ImageService.GetImage("Import_32.svg")
		};
		_barManager.Items.Add(_barButtonImportFromFile);
		_barButtonImportFromFile.ItemClick += new SafeDocumentEventHandler<ItemClickEventArgs>(BarButtonImportFromFile_ItemClick, this).Handler;
		_ribbonPageGroupActions.ItemLinks.Add(_barButtonImportFromFile.CreateLink());
		_barButtonPrintLabel = new BarButtonItem
		{
			Content = ArmSoft.SME.Trade.Common.Resources.PrintLabel,
			KeyGesture = new KeyGesture((Key)55, (ModifierKeys)2),
			Glyph = ImageService.GetImage("action_16.svg"),
			LargeGlyph = ImageService.GetImage("action_32.svg")
		};
		_barManager.Items.Add(_barButtonPrintLabel);
		_barButtonPrintLabel.ItemClick += new SafeDocumentEventHandler<ItemClickEventArgs>(BarButtonPrintLabel_ItemClick, this).Handler;
		_ribbonPageGroupActions.ItemLinks.Add(_barButtonPrintLabel.CreateLink());
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

	private void BarButtonPrintLabel_ItemClick(object sender, ItemClickEventArgs e)
	{
		PrintLabel();
	}

	private void PrintLabel()
	{
		if (_mtMoveOrderList.Count > 0)
		{
			FillDocument(new StoreRequest());
			new PrintLabelDialog(this, PO).Invoke();
		}
	}

	private void BarButtonImportFromFile_ItemClick(object sender, ItemClickEventArgs e)
	{
		ImportFromFile_Click();
	}

	private void ImportFromFile_Click()
	{
		try
		{
			if (_date.IsReadOnly || !_date.DoValidate())
			{
				return;
			}
			if (string.IsNullOrWhiteSpace(_storageExpense.Code))
			{
				throw new ASException(ArmSoft.SME.Trade.Common.Resources.ErrorOutStorageFromFile);
			}
			if (_mtMoveOrderList.Count <= 0 || ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Trade.Common.Resources.LoadMaterialsList, Settings.ProgramName, MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No) != MessageBoxResult.No)
			{
				_mtMoveOrderList.Clear();
				_maxRowID = 0;
				TerminalHelper.TerminalImporter terminalImporter = new TerminalHelper.TerminalImporter(this);
				if (terminalImporter.ImportFromFile((decimal quantity, MaterialCacheRow goodRow, MaterialCacheRow.BarCodeRow goodBarcodeRow) =>
				{
					AddMTToListNewRow(goodRow, goodBarcodeRow.QntUnit, TradeSettings.TradeCacheService.QntUnit.GetItem(goodBarcodeRow.QntUnit).Brief, quantity);
				}))
				{
					terminalImporter.ShowReport();
				}
			}
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	protected override void StoreSettings(RegistryKey rk)
	{
		base.StoreSettings(rk);
		_mtMoveOrderGrid.StoreSettings(rk);
	}

	protected override void RestoreSettings(double defaultWidth, double defaultHeight, RegistryKey rk)
	{
		base.RestoreSettings(defaultWidth, defaultHeight, rk);
		_mtMoveOrderGrid.RestoreSettings(rk);
	}
}
