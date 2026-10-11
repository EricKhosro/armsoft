using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ArmSoft.EMarkReceiptClient.Configuration;
using ArmSoft.EMarkReceiptClient.Exceptions;
using ArmSoft.EMarkReceiptClient.Models;
using ArmSoft.EMarkReceiptClient.Services;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Extensions;
using ArmSoft.SME.Common.Models;
using ArmSoft.SME.Common.Models.Documents;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Core.UI.BackgroundWorks;
using ArmSoft.SME.Core.UI.Controls;
using ArmSoft.SME.Core.UI.DataViews;
using ArmSoft.SME.Core.UI.Documents;
using ArmSoft.SME.Core.UI.DropDowns;
using ArmSoft.SME.Core.UI.Enums;
using ArmSoft.SME.Core.UI.Extensions;
using ArmSoft.SME.Core.UI.Helpers;
using ArmSoft.SME.Core.UI.Services;
using ArmSoft.SME.Core.UI.StandartWindow;
using ArmSoft.SME.Trade.Common;
using ArmSoft.SME.Trade.Common.Documents.Definition;
using ArmSoft.SME.Trade.Common.Models.Caches;
using ArmSoft.SME.Trade.Common.Models.Directories;
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
using DevExpress.Xpf.Editors.Settings;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.LayoutControl;
using DevExpress.XtraEditors.DXErrorProvider;
using Microsoft.Win32;

namespace ArmSoft.SME.Trade.UI.Documents;

[DocumentWindow(37)]
public class EMarkReceiptWindow : TradeDocumentWindow<EMarkReceipt>
{
	public class EMarkReceiptSpecificationRow : DocumentGridRow
	{
		[ObservableProperty]
		private int _id = -1;

		[ObservableProperty]
		private string _code;

		[ObservableProperty]
		private string _name;

		[ObservableProperty]
		private string _unitCode;

		[ObservableProperty]
		private string _unitBrief;

		[ObservableProperty]
		private decimal _quantity;

		[ObservableProperty]
		private short _rowNum;

		[ObservableProperty]
		private string _cPACode;

		[ObservableProperty]
		private string _barcode;

		[ObservableProperty]
		private int _rowId;

		[ObservableProperty]
		private HashSet<string> _markingList = new HashSet<string>();

		[ObservableProperty]
		private bool _isRowMarkable;

		[ObservableProperty]
		private string _markingStatus = "";

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public int Id
		{
			get
			{
				return _id;
			}
			set
			{
				if (!EqualityComparer<int>.Default.Equals(_id, value))
				{
					OnPropertyChanging(__KnownINotifyPropertyChangingArgs.Id);
					_id = value;
					OnPropertyChanged(__KnownINotifyPropertyChangedArgs.Id);
				}
			}
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public string Code
		{
			get
			{
				return _code;
			}
			[MemberNotNull("_code")]
			set
			{
				if (!EqualityComparer<string>.Default.Equals(_code, value))
				{
					OnPropertyChanging(__KnownINotifyPropertyChangingArgs.Code);
					_code = value;
					OnCodeChanged(value);
					OnPropertyChanged(__KnownINotifyPropertyChangedArgs.Code);
				}
			}
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public string Name
		{
			get
			{
				return _name;
			}
			[MemberNotNull("_name")]
			set
			{
				if (!EqualityComparer<string>.Default.Equals(_name, value))
				{
					OnPropertyChanging(__KnownINotifyPropertyChangingArgs.Name);
					_name = value;
					OnPropertyChanged(__KnownINotifyPropertyChangedArgs.Name);
				}
			}
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public string UnitCode
		{
			get
			{
				return _unitCode;
			}
			[MemberNotNull("_unitCode")]
			set
			{
				if (!EqualityComparer<string>.Default.Equals(_unitCode, value))
				{
					OnPropertyChanging(__KnownINotifyPropertyChangingArgs.UnitCode);
					_unitCode = value;
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
				return _unitBrief;
			}
			[MemberNotNull("_unitBrief")]
			set
			{
				if (!EqualityComparer<string>.Default.Equals(_unitBrief, value))
				{
					OnPropertyChanging(__KnownINotifyPropertyChangingArgs.UnitBrief);
					_unitBrief = value;
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
				return _quantity;
			}
			set
			{
				if (!EqualityComparer<decimal>.Default.Equals(_quantity, value))
				{
					OnPropertyChanging(__KnownINotifyPropertyChangingArgs.Quantity);
					_quantity = value;
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
				return _rowNum;
			}
			set
			{
				if (!EqualityComparer<short>.Default.Equals(_rowNum, value))
				{
					OnPropertyChanging(__KnownINotifyPropertyChangingArgs.RowNum);
					_rowNum = value;
					OnPropertyChanged(__KnownINotifyPropertyChangedArgs.RowNum);
				}
			}
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public string CPACode
		{
			get
			{
				return _cPACode;
			}
			[MemberNotNull("_cPACode")]
			set
			{
				if (!EqualityComparer<string>.Default.Equals(_cPACode, value))
				{
					OnPropertyChanging(__KnownINotifyPropertyChangingArgs.CPACode);
					_cPACode = value;
					OnPropertyChanged(__KnownINotifyPropertyChangedArgs.CPACode);
				}
			}
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public string Barcode
		{
			get
			{
				return _barcode;
			}
			[MemberNotNull("_barcode")]
			set
			{
				if (!EqualityComparer<string>.Default.Equals(_barcode, value))
				{
					OnPropertyChanging(__KnownINotifyPropertyChangingArgs.Barcode);
					_barcode = value;
					OnPropertyChanged(__KnownINotifyPropertyChangedArgs.Barcode);
				}
			}
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public int RowId
		{
			get
			{
				return _rowId;
			}
			set
			{
				if (!EqualityComparer<int>.Default.Equals(_rowId, value))
				{
					OnPropertyChanging(__KnownINotifyPropertyChangingArgs.RowId);
					_rowId = value;
					OnPropertyChanged(__KnownINotifyPropertyChangedArgs.RowId);
				}
			}
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public HashSet<string> MarkingList
		{
			get
			{
				return _markingList;
			}
			[MemberNotNull("_markingList")]
			set
			{
				if (!EqualityComparer<HashSet<string>>.Default.Equals(_markingList, value))
				{
					OnPropertyChanging(__KnownINotifyPropertyChangingArgs.MarkingList);
					_markingList = value;
					OnPropertyChanged(__KnownINotifyPropertyChangedArgs.MarkingList);
				}
			}
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public bool IsRowMarkable
		{
			get
			{
				return _isRowMarkable;
			}
			set
			{
				if (!EqualityComparer<bool>.Default.Equals(_isRowMarkable, value))
				{
					OnPropertyChanging(__KnownINotifyPropertyChangingArgs.IsRowMarkable);
					_isRowMarkable = value;
					OnPropertyChanged(__KnownINotifyPropertyChangedArgs.IsRowMarkable);
				}
			}
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public string MarkingStatus
		{
			get
			{
				return _markingStatus;
			}
			[MemberNotNull("_markingStatus")]
			set
			{
				if (!EqualityComparer<string>.Default.Equals(_markingStatus, value))
				{
					OnPropertyChanging(__KnownINotifyPropertyChangingArgs.MarkingStatus);
					_markingStatus = value;
					OnPropertyChanged(__KnownINotifyPropertyChangedArgs.MarkingStatus);
				}
			}
		}

		public EMarkReceiptSpecificationRow(HashSet<string> markingList)
		{
			_markingList = markingList;
		}

		public EMarkReceiptSpecificationRow()
		{
		}

		public EMarkReceiptSpecificationRow(EMarkReceiptWindow docWindow)
			: base(docWindow)
		{
		}

		static EMarkReceiptSpecificationRow()
		{
			DocumentGridRow.RegisterPropertyValidators(typeof(EMarkReceiptSpecificationRow), cacheValidationResults: true, new Dictionary<string, Action<DocumentGridRow, DevExpress.XtraEditors.DXErrorProvider.ErrorInfo>>
			{
				{
					"Code",
					(DocumentGridRow row, DevExpress.XtraEditors.DXErrorProvider.ErrorInfo info) =>
					{
						ValidateCode((EMarkReceiptSpecificationRow)row, info);
					}
				},
				{
					"UnitCode",
					(DocumentGridRow row, DevExpress.XtraEditors.DXErrorProvider.ErrorInfo info) =>
					{
						ValidateUnitCode((EMarkReceiptSpecificationRow)row, info);
					}
				}
			});
		}

		private static void ValidateCode(EMarkReceiptSpecificationRow row, DevExpress.XtraEditors.DXErrorProvider.ErrorInfo info)
		{
			TradePropertyValidator.ValidateMaterialCode(row.Code, info, TradeAllDocDefinitions.EMarkReceipt.Grids["MTList"].Columns["Code"]);
		}

		private static void ValidateUnitCode(EMarkReceiptSpecificationRow row, DevExpress.XtraEditors.DXErrorProvider.ErrorInfo info)
		{
			TradePropertyValidator.ValidateGoodUnitCode(row.UnitCode, row.Code, info, TradeAllDocDefinitions.EMarkReceipt.Grids["MTList"].Columns["UnitCode"]);
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		private void OnCodeChanged(string value)
		{
			ResetForceValidationBit("Code", "UnitCode");
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		private void OnUnitCodeChanged(string value)
		{
			ResetForceValidationBit("UnitCode");
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		private void OnQuantityChanged(decimal value)
		{
			ResetForceValidationBit("Quantity");
		}
	}

	public class EMarkReceiptFilteredSearchList : IResultList
	{
		private readonly ObservableCollection<EMarkReceiptSpecificationRow> _itemSource = new ObservableCollection<EMarkReceiptSpecificationRow>();

		public IEnumerable<DocumentGridRow> ItemsSource => _itemSource;

		public List<FilteredSearchGridColumn> Columns
		{
			get
			{
				int num = 3;
				List<FilteredSearchGridColumn> list = new List<FilteredSearchGridColumn>(num);
				CollectionsMarshal.SetCount(list, num);
				Span<FilteredSearchGridColumn> span = CollectionsMarshal.AsSpan(list);
				span[0] = new FilteredSearchGridColumn
				{
					Header = TradeAllDocDefinitions.EMarkReceipt.Grids["MTList"].Columns["Code"].Caption,
					FieldName = "Code",
					CustomWidth = 125.0
				};
				span[1] = new FilteredSearchGridColumn
				{
					Header = TradeAllDocDefinitions.EMarkReceipt.Grids["MTList"].Columns["Name"].Caption,
					FieldName = "Name",
					CustomWidth = 300.0
				};
				span[2] = new FilteredSearchGridColumn
				{
					Header = TradeAllDocDefinitions.EMarkReceipt.Grids["MTList"].Columns["Quantity"].Caption,
					FieldName = "Quantity",
					AllowEditing = true,
					NumericFormat = DecimalExtension.GetCurrencyFormat(13, 3)
				};
				return list;
			}
		}

		public int AddToItemsSource(object key, params object[] args)
		{
			string mtCode = (string)key;
			int num = _itemSource.FindIndex((EMarkReceiptSpecificationRow x) => x.Code == mtCode);
			if (num == -1)
			{
				MaterialCacheRow item = TradeSettings.TradeCacheService.Material.GetItem(mtCode);
				if (item == null)
				{
					return -1;
				}
				EMarkReceiptSpecificationRow item2 = new EMarkReceiptSpecificationRow
				{
					Id = item.Id,
					Code = item.MTCode,
					Name = item.Caption,
					UnitCode = item.Unit,
					UnitBrief = item.UnitRow.Brief,
					Quantity = 0m
				};
				_itemSource.Add(item2);
				num = _itemSource.Count - 1;
			}
			return num;
		}
	}

	private Button _exportToEMarkReceipt;

	private TextEditExt _eMarkReceiptNumber;

	private TextEditExt _barcode;

	private Button _codeEnter;

	private DropDownControl _storage;

	private DropDownControl _owner;

	private DocumentGridControl _eMarkReceiptGrid;

	private TableView _eMarkReceiptTableView;

	private ObservableCollection<EMarkReceiptSpecificationRow> _eMarkReceiptList;

	private BarEditItemExt _filteredSearch;

	private BarEditItemExt _selectMarking;

	private DocumentGridColumn _mtCodeCol;

	private DocumentGridColumn _unitCodeCol;

	private DocumentGridColumn _quantityCol;

	private DocumentGridColumn _rowMarkingListCol;

	private int _maxRowId;

	private readonly string _rowAddMode = Settings.ParametersService.ScannerMode();

	private readonly bool _useMarking = Settings.ParametersService.UseMarking();

	private string _realText = "";

	private ReportBarButtonItem _barButtonJumpTo;

	protected override double WidthValue => 1090.0;

	protected override double HeightValue => 440.0;

	protected override double MinWidthValue => 620.0;

	protected override double MinHeightValue => 250.0;

	public EMarkReceiptWindow()
	{
	}

	public EMarkReceiptWindow(EMarkReceipt doc)
		: base(doc)
	{
	}

	protected override void ConstructUI()
	{
		base.ConstructUI();
		LayoutGroupExt container = AddLayoutGroup(null, LayoutGroupView.Group, Orientation.Horizontal);
		_date = AddDateControl(container, Definition.Fields["DocDate"]);
		_date.Margin = new Thickness(0.0, 0.0, 10.0, 0.0);
		_docNum = AddButtonEditControl(container, Definition.Fields["DocNum"], 1);
		_docNum.AllowMagicButton = true;
		_docNum.MagicToolTip = ArmSoft.SME.Common.Resources.DocNextNumbers;
		_exportToEMarkReceipt = AddButton(container, "emark_16.svg", " " + ArmSoft.SME.Trade.Common.Resources.EMarkReceiptButton);
		_exportToEMarkReceipt.IsTabStop = true;
		_exportToEMarkReceipt.Width = 150.0;
		_eMarkReceiptNumber = AddStringControl(null, Definition.Fields["EMarkReceiptNumber"]);
		LayoutGroupExt container2 = AddLayoutGroup(null, LayoutGroupView.Group, Orientation.Horizontal);
		_barcode = AddStringControl(container2, ArmSoft.SME.Common.Resources.BarCode + " (F2)", isRequired: false, 110, HorizontalAlignment.Stretch);
		_barcode.NullText = ArmSoft.SME.Trade.Common.Resources.BarCodeAndCodeInputNullText;
		string text = ArmSoft.SME.Trade.Common.Resources.cmdCodeEnter + " (F8)";
		_codeEnter = AddButton(container2, null, text, null, text, isTabStop: true);
		_codeEnter.Margin = new Thickness(0.0, 0.0, 18.0, 0.0);
		_storage = AddDropDownControl(null, Definition.Fields["Storage"], new Storages());
		DXTabControl tc = AddTabControl(null);
		LayoutGroup container3 = AddTabbedGroup(tc, ArmSoft.SME.Common.Resources.General, "General");
		AddComboBoxControl(container3, Definition.Fields["Comment"], TradeSettings.TradeCacheService.StandardComments.GetCaptionsList(), HorizontalAlignment.Stretch);
		_owner = AddDropDownControl(container3, Definition.Fields["Owner"], new UsersFilter());
		_owner.CodeWidth = 12;
		_owner.ShowComment = false;
		_owner.InnerCodeEdit.Width = _owner.InnerCodeEdit.MinWidth;
		if (!Settings.LoginService.IsAdmin && Settings.ParametersService.DenyAccessToOtherOwnerDocs())
		{
			((LayoutItemWithRequired)(object)_owner.Parent).Visibility = Visibility.Collapsed;
		}
		_eMarkReceiptGrid = AddGrid(container3, Definition.Grids["MTList"], HorizontalAlignment.Stretch, VerticalAlignment.Stretch);
		_eMarkReceiptGrid.AllowMoveUp = true;
		_eMarkReceiptGrid.AllowMoveDown = true;
		_eMarkReceiptGrid.IsVisibleViewEditButton = false;
		_eMarkReceiptGrid.RefreshButtons += new SafeDocumentEventHandler<EventArgs>(EMarkReceiptGrid_RefreshButtons, this).Handler;
		_eMarkReceiptTableView = _eMarkReceiptGrid.View;
		_filteredSearch = _eMarkReceiptGrid.AddButton("", ArmSoft.SME.Trade.Common.Resources.GroupAddition, ArmSoft.SME.Trade.Common.Resources.GroupAddition, 130.0);
		_selectMarking = _eMarkReceiptGrid.AddButton("datamatrix_blue_16.svg", ArmSoft.SME.Common.Resources.Marking, ArmSoft.SME.Common.Resources.Marking, 135.0);
		_eMarkReceiptGrid.AddRowIndicator(35);
		_mtCodeCol = _eMarkReceiptGrid.AddDropDownColumn(Definition.Grids["MTList"].Columns["Code"], new GoodsFilter(shortMode: true, ItemTypesSE.Goods.Value), allowAdditionButtons: true);
		_mtCodeCol.Width = 125.0;
		DocumentGridColumn documentGridColumn = _eMarkReceiptGrid.AddStringColumn(Definition.Grids["MTList"].Columns["Name"]);
		documentGridColumn.Width = 225.0;
		documentGridColumn.AllowEditing = DefaultBoolean.False;
		_unitCodeCol = _eMarkReceiptGrid.AddDropDownColumn(Definition.Grids["MTList"].Columns["UnitCode"]);
		_eMarkReceiptGrid.AddStringColumn(Definition.Grids["MTList"].Columns["UnitBrief"]).AllowEditing = DefaultBoolean.False;
		_quantityCol = _eMarkReceiptGrid.AddNumberColumn(Definition.Grids["MTList"].Columns["Quantity"], showTotal: true);
		_rowMarkingListCol = _eMarkReceiptGrid.AddDropDownColumn("MarkingStatus", ArmSoft.SME.Common.Resources.Marking, 19, trimValue: false, GlyphKind.Regular);
		((ButtonEditSettings)_rowMarkingListCol.EditSettings).IsTextEditable = false;
		_eMarkReceiptList = new ObservableCollection<EMarkReceiptSpecificationRow>();
	}

	protected override void OnNew()
	{
		base.OnNew();
		_date.EditValue = Settings.HostingEnvironment.CurrentDate;
		_owner.Code = Settings.LoginService.UserName;
		_storage.Code = Settings.ParametersService.DefaultStorage();
	}

	protected override void OnNewAfterSave()
	{
		base.OnNewAfterSave();
		_eMarkReceiptList.Clear();
		_owner.Code = Settings.LoginService.UserName;
		_maxRowId = 0;
		_eMarkReceiptNumber.Text = "";
	}

	protected override void OnEdit()
	{
		base.OnEdit();
		_setFocusOnActivation = _docNum;
	}

	protected override void LoadDataToControls()
	{
		base.LoadDataToControls();
		_maxRowId = PO.MaxRowId;
		_eMarkReceiptList.Clear();
		foreach (EMarkReceipt.EMarkReceiptSpecificationRow mT in PO.MTList)
		{
			EMarkReceiptSpecificationRow item = new EMarkReceiptSpecificationRow
			{
				Id = mT.Id,
				Code = mT.Code,
				Name = mT.Name,
				UnitCode = mT.UnitCode,
				UnitBrief = mT.UnitBrief,
				Quantity = mT.Quantity,
				RowId = mT.RowId,
				RowNum = mT.RowNum,
				CPACode = mT.CPACode,
				Barcode = mT.Barcode,
				IsRowMarkable = mT.IsRowMarkable,
				MarkingStatus = mT.MarkingStatus,
				MarkingList = mT.MarkingList
			};
			_eMarkReceiptList.Add(item);
		}
		_eMarkReceiptGrid.ItemsSource = _eMarkReceiptList;
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

	protected override void AddToolbarItems()
	{
		base.AddToolbarItems();
		Ribbon.ToolbarItemLinks.Add(_barButtonJumpTo.ConvertForToolbar("t_doc_JumpTo_16.svg"));
	}

	protected override void AddItemsToRibbon()
	{
		base.AddItemsToRibbon();
		CreateRibbonPageGroupActions();
		CreateRibbonPageGroupReports();
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

	protected override void SetControlsReadonly()
	{
		base.SetControlsReadonly();
		if (_viewMode == WindowViewMode.View)
		{
			return;
		}
		_eMarkReceiptNumber.IsReadOnly = true;
		bool flag = (PO.StoredInDb && !IsDraftOrDeleted) || !string.IsNullOrWhiteSpace(_eMarkReceiptNumber.Text);
		_date.IsReadOnly = flag;
		_docNum.IsReadOnly = flag;
		_barcode.IsReadOnly = flag;
		_codeEnter.IsEnabled = !flag;
		_storage.IsReadOnly = flag;
		foreach (GridColumn column in _eMarkReceiptGrid.Grid.Columns)
		{
			column.ReadOnly = flag;
		}
		_eMarkReceiptGrid.AllowAdd = !flag;
		_eMarkReceiptGrid.AllowDelete = !flag;
		_eMarkReceiptGrid.AllowDuplicate = !flag;
		_eMarkReceiptGrid.AllowMoveUp = !flag;
		_eMarkReceiptGrid.AllowMoveDown = !flag;
		_exportToEMarkReceipt.IsEnabled = string.IsNullOrWhiteSpace(_eMarkReceiptNumber.Text);
		_quantityCol.ReadOnly = true;
		_rowMarkingListCol.ReadOnly = false;
	}

	private void FilteredSearch_Click(object sender, RoutedEventArgs e)
	{
		FilteredSearchWindow filteredSearchWindow = new FilteredSearchWindow(new FilteredSearch
		{
			FilterDataView = new GoodsGroups(),
			SearchDataView = new GoodsFilter(shortMode: true, ItemTypesSE.Goods),
			ResultsList = new EMarkReceiptFilteredSearchList()
		}, this, isBasketMode: true);
		if (!filteredSearchWindow.ShowDialog().Value)
		{
			return;
		}
		foreach (EMarkReceiptSpecificationRow item in filteredSearchWindow.BasketResult)
		{
			item.RowId = _maxRowId;
			item.Quantity = item.MarkingList.Count;
			_eMarkReceiptList.Add(item);
			_maxRowId++;
		}
	}

	private void EMarkReceiptGrid_AddingRow(object sender, DocumentGridAddingRowEventArgs e)
	{
		_eMarkReceiptList.Add(new EMarkReceiptSpecificationRow
		{
			RowId = _maxRowId
		});
		_maxRowId++;
		e.RowNumber = _eMarkReceiptList.Count - 1;
	}

	private void EMarkReceiptGrid_DuplicatingRow(object sender, DocumentGridDuplicatingRowEventArgs e)
	{
		EMarkReceiptSpecificationRow eMarkReceiptSpecificationRow = (EMarkReceiptSpecificationRow)_eMarkReceiptGrid.Grid.GetRow(e.RowHandle);
		if (eMarkReceiptSpecificationRow != null)
		{
			EMarkReceiptSpecificationRow eMarkReceiptSpecificationRow2 = new EMarkReceiptSpecificationRow();
			eMarkReceiptSpecificationRow.Clone(eMarkReceiptSpecificationRow2);
			eMarkReceiptSpecificationRow2.RowId = _maxRowId;
			_eMarkReceiptList.Add(eMarkReceiptSpecificationRow2);
			_maxRowId++;
			e.NewRowNumber = _eMarkReceiptList.Count - 1;
		}
	}

	private void EMarkReceiptGrid_RefreshButtons(object sender, EventArgs e)
	{
		_filteredSearch.IsEnabled = !_eMarkReceiptGrid.IsReadOnly && _eMarkReceiptGrid.AllowAdd;
	}

	private void EMarkReceiptGrid_MovingRowUp(object sender, DocumentGridMovingRowUpOrDownEventArgs e)
	{
		if (_eMarkReceiptGrid.Grid.SortInfo.Count > 0 || _eMarkReceiptTableView.Grid.VisibleRowCount != _eMarkReceiptList.Count)
		{
			throw new ASException(ArmSoft.SME.Common.Resources.NotAllowRowMoveInSortedGrid);
		}
		EMarkReceiptSpecificationRow eMarkReceiptSpecificationRow = (EMarkReceiptSpecificationRow)_eMarkReceiptGrid.Grid.GetRow(e.RowHandle);
		if (eMarkReceiptSpecificationRow != null)
		{
			_eMarkReceiptList.RemoveAt(e.RowHandle);
			_eMarkReceiptList.Insert(e.RowHandle - 1, eMarkReceiptSpecificationRow);
		}
	}

	private void EMarkReceiptGrid_MovingRowDown(object sender, DocumentGridMovingRowUpOrDownEventArgs e)
	{
		if (_eMarkReceiptGrid.Grid.SortInfo.Count > 0 || _eMarkReceiptTableView.Grid.VisibleRowCount != _eMarkReceiptList.Count)
		{
			throw new ASException(ArmSoft.SME.Common.Resources.NotAllowRowMoveInSortedGrid);
		}
		EMarkReceiptSpecificationRow eMarkReceiptSpecificationRow = (EMarkReceiptSpecificationRow)_eMarkReceiptGrid.Grid.GetRow(e.RowHandle);
		if (eMarkReceiptSpecificationRow != null)
		{
			_eMarkReceiptList.RemoveAt(e.RowHandle);
			_eMarkReceiptList.Insert(e.RowHandle + 1, eMarkReceiptSpecificationRow);
		}
	}

	protected override void FillDocument(StoreRequest sr)
	{
		base.FillDocument(sr);
		PO.MTList.Clear();
		foreach (EMarkReceiptSpecificationRow eMarkReceipt in _eMarkReceiptList)
		{
			EMarkReceipt.EMarkReceiptSpecificationRow item = new EMarkReceipt.EMarkReceiptSpecificationRow
			{
				Id = eMarkReceipt.Id,
				Code = eMarkReceipt.Code,
				Name = eMarkReceipt.Name,
				UnitCode = eMarkReceipt.UnitCode,
				UnitBrief = eMarkReceipt.UnitBrief,
				Quantity = eMarkReceipt.Quantity,
				CPACode = eMarkReceipt.CPACode,
				Barcode = eMarkReceipt.Barcode,
				RowId = eMarkReceipt.RowId,
				RowNum = eMarkReceipt.RowNum,
				IsRowMarkable = eMarkReceipt.IsRowMarkable,
				MarkingStatus = eMarkReceipt.MarkingStatus,
				MarkingList = eMarkReceipt.MarkingList
			};
			PO.MTList.Add(item);
		}
	}

	protected override void SetEvents()
	{
		base.SetEvents();
		_docNum.MagicClick += new SafeDocumentEventHandler<MagicClickEventArgs>(DocNum_MagicClick, this).Handler;
		_docNum.EditValueChanged += new SafeDocumentEventHandler<EditValueChangedEventArgs>(DocNum_EditValueChanged, this).Handler;
		_barcode.PreviewTextInput += new SafeDocumentEventHandler<TextCompositionEventArgs>(BarCode_PreviewTextInput, this).Handler;
		_barcode.KeyDown += new SafeDocumentEventHandler<KeyEventArgs>(BarCode_KeyDown, this).Handler;
		_codeEnter.Click += new SafeDocumentEventHandler<RoutedEventArgs>(CodeEnter_Click, this).Handler;
		_eMarkReceiptGrid.AddingRow += new SafeDocumentEventHandler<DocumentGridAddingRowEventArgs>(EMarkReceiptGrid_AddingRow, this).Handler;
		_eMarkReceiptGrid.DuplicatingRow += new SafeDocumentEventHandler<DocumentGridDuplicatingRowEventArgs>(EMarkReceiptGrid_DuplicatingRow, this).Handler;
		_eMarkReceiptGrid.MovingRowUp += new SafeDocumentEventHandler<DocumentGridMovingRowUpOrDownEventArgs>(EMarkReceiptGrid_MovingRowUp, this).Handler;
		_eMarkReceiptGrid.MovingRowDown += new SafeDocumentEventHandler<DocumentGridMovingRowUpOrDownEventArgs>(EMarkReceiptGrid_MovingRowDown, this).Handler;
		_eMarkReceiptGrid.ClickDropDown += new SafeEventHandler<DocumentGridClickDropDownEventArgs>(EMarkReceiptGrid_ClickDropDown).Handler;
		_eMarkReceiptTableView.CellValueChanged += new SafeDocumentEventHandler<CellValueChangedEventArgs>(EMarkReceiptTableView_CellValueChanged, this).Handler;
		_eMarkReceiptTableView.FocusedRowHandleChanged += new SafeDocumentEventHandler<FocusedRowHandleChangedEventArgs>(EMarkReceiptTableView_FocusedRowHandleChangedEventArgs, this).Handler;
		_filteredSearch.Click += new SafeDocumentEventHandler<RoutedEventArgs>(FilteredSearch_Click, this).Handler;
		_selectMarking.Click += new SafeDocumentEventHandler<RoutedEventArgs>(SelectMarking_Click, this).Handler;
		_exportToEMarkReceipt.Click += new SafeDocumentEventHandler<RoutedEventArgs>(ExportEMarkReceipt_Click, this).Handler;
	}

	private void EMarkReceiptTableView_CellValueChanged(object sender, CellValueChangedEventArgs e)
	{
		EMarkReceiptSpecificationRow eMarkReceiptSpecificationRow = (EMarkReceiptSpecificationRow)_eMarkReceiptGrid.Grid.GetRow(e.RowHandle);
		if (eMarkReceiptSpecificationRow == null)
		{
			return;
		}
		if (e.Column == _mtCodeCol)
		{
			try
			{
				if (e.Value == null || string.IsNullOrWhiteSpace(e.Value.ToString()))
				{
					eMarkReceiptSpecificationRow.Id = -1;
					eMarkReceiptSpecificationRow.Name = "";
					eMarkReceiptSpecificationRow.UnitCode = "";
					eMarkReceiptSpecificationRow.UnitBrief = "";
				}
				else
				{
					string text = e.Value.ToString();
					if (string.IsNullOrWhiteSpace(text))
					{
						throw new ASException();
					}
					MaterialCacheRow item = TradeSettings.TradeCacheService.Material.GetItem(text, throwExceptionNotExits: true);
					eMarkReceiptSpecificationRow.Id = item.Id;
					eMarkReceiptSpecificationRow.Name = item.Caption;
					eMarkReceiptSpecificationRow.UnitCode = item.Unit;
					eMarkReceiptSpecificationRow.UnitBrief = item.UnitRow.Brief;
					_unitCodeCol.DataView = new MTQntUnits(eMarkReceiptSpecificationRow.Code);
				}
			}
			catch (Exception ex)
			{
				eMarkReceiptSpecificationRow.Name = "";
				eMarkReceiptSpecificationRow.Id = -1;
				eMarkReceiptSpecificationRow.UnitCode = "";
				eMarkReceiptSpecificationRow.UnitBrief = "";
				eMarkReceiptSpecificationRow.Quantity = 0m;
				if (e.Value != null && !string.IsNullOrWhiteSpace(e.Value.ToString()))
				{
					Settings.ErrorHandler(ex);
				}
			}
		}
		else if (e.Column == _unitCodeCol)
		{
			try
			{
				if (e.Value != null && !string.IsNullOrWhiteSpace(e.Value.ToString()))
				{
					QntUnitCacheRow item2 = TradeSettings.TradeCacheService.QntUnit.GetItem(e.Value.ToString());
					eMarkReceiptSpecificationRow.UnitBrief = ((item2 == null) ? "" : item2.Brief);
				}
			}
			catch (Exception ex2)
			{
				Settings.ErrorHandler(ex2);
				throw;
			}
		}
		_eMarkReceiptGrid.Grid.RefreshData();
	}

	private void EMarkReceiptGrid_ClickDropDown(object sender, DocumentGridClickDropDownEventArgs e)
	{
		try
		{
			EMarkReceiptSpecificationRow eMarkReceiptSpecificationRow = (EMarkReceiptSpecificationRow)_eMarkReceiptGrid.Grid.GetRow(e.RowHandle);
			if (eMarkReceiptSpecificationRow == null)
			{
				return;
			}
			if (e.Column == _mtCodeCol)
			{
				GoodsFilter goodsFilter = new GoodsFilter(shortMode: true, ItemTypesSE.Goods.Value);
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
			if (e.Column == _unitCodeCol)
			{
				MTQntUnits mTQntUnits = new MTQntUnits(eMarkReceiptSpecificationRow.Code);
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
			if (e.Column != _rowMarkingListCol)
			{
				return;
			}
			if (!MarkingHelpers.IsGoodMarkable(eMarkReceiptSpecificationRow.Code, _useMarking, eMarkReceiptSpecificationRow.MarkingList.Count != 0, _date.EditValue.Value, out var isRowMarkable))
			{
				if (string.IsNullOrWhiteSpace(eMarkReceiptSpecificationRow.Code))
				{
					throw new ASException(ArmSoft.SME.Common.Resources.ValidateEmptyCode);
				}
				ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Common.Resources.NotMarkable, Settings.ProgramName, MessageBoxButton.OK, MessageBoxImage.Asterisk, MessageBoxResult.OK);
			}
			else
			{
				eMarkReceiptSpecificationRow.IsRowMarkable = isRowMarkable;
				OpenMarkingSelectWindow(eMarkReceiptSpecificationRow);
			}
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	private void OpenMarkingSelectWindow(EMarkReceiptSpecificationRow gridRow)
	{
		List<string> allMarkings = _eMarkReceiptList.SelectMany((EMarkReceiptSpecificationRow row) => row.MarkingList).ToList();
		MarkingSelectWindow markingSelectWindow = new MarkingSelectWindow(this, (PO.StoredInDb && !IsDraftOrDeleted) || !string.IsNullOrWhiteSpace(_eMarkReceiptNumber.Text), gridRow.IsRowMarkable, gridRow.Code, gridRow.Name, gridRow.MarkingList, allMarkings, gridRow.Quantity);
		if (!markingSelectWindow.ShowDialog().Value || markingSelectWindow.IsViewOnlyMode)
		{
			return;
		}
		gridRow.MarkingList.Clear();
		foreach (MarkingSelectWindow.MarkingRow marking in markingSelectWindow.MarkingList)
		{
			gridRow.MarkingList.Add(marking.Marking);
		}
		gridRow.Quantity = gridRow.MarkingList.Count;
	}

	public override void OnScanner(string scannerData)
	{
		if (!_barcode.IsReadOnly)
		{
			_barcode.Text = MarkingHelper.CleanScannerSymbols(scannerData);
			ScannerAdd();
		}
	}

	protected override void OnF2()
	{
		if (!_barcode.IsReadOnly)
		{
			ControlsHelper.SetFocus(_barcode);
		}
	}

	private void ScannerAdd()
	{
		_barcode.DoValidate();
		try
		{
			string barCode = _barcode.Text.Trim();
			decimal quantity = 0m;
			EMarkReceiptSpecificationRow row = null;
			if (barCode.Length < 29)
			{
				MaterialCacheRow materialCacheRow = DocumentHelpers.TryGetItemByBarCode(ref barCode, out var _, out var unitCode, out var unitBrief);
				if (materialCacheRow == null || materialCacheRow.Type == ItemTypesSE.Services.Value)
				{
					ControlsHelper.SetFocus(_barcode);
					throw new ASException(string.Format(ArmSoft.SME.Common.Resources.ErrBarCodeNotExist, barCode));
				}
				AddMTToList(materialCacheRow, ref row, quantity, unitCode, unitBrief);
				_barcode.Text = "";
				_realText = "";
				if (Settings.ParametersService.OpenMarkingWindowImmediatly() && MarkingHelpers.IsGoodMarkable(row.Code, _useMarking, row.MarkingList.Count != 0, _date.EditValue.Value, out var isRowMarkable))
				{
					row.IsRowMarkable = isRowMarkable;
					OpenMarkingSelectWindow(row);
				}
				_eMarkReceiptGrid.Grid.RefreshData();
				return;
			}
			quantity = 1m;
			string eanBarCode = "";
			string item = barCode;
			MaterialCacheRow goodFromScanedMark = GetGoodFromScanedMark(ref eanBarCode);
			if (goodFromScanedMark == null)
			{
				_barcode.Text = "";
				_realText = "";
				ControlsHelper.SetFocus(_barcode);
				throw new ASException(string.Format(ArmSoft.SME.Trade.Common.Resources.BarCodesNotExist, eanBarCode));
			}
			if (!MarkingHelpers.IsGoodMarkable(goodFromScanedMark.MTCode, _useMarking, markingListHasValue: false, _date.EditValue.Value, out var isRowMarkable2))
			{
				throw new ASException(ArmSoft.SME.Common.Resources.CPAEmptyOrNotMarkable);
			}
			AddMTToList(goodFromScanedMark, ref row, quantity, goodFromScanedMark.Unit, goodFromScanedMark.UnitRow.Brief);
			_barcode.Text = "";
			_realText = "";
			if (!string.IsNullOrWhiteSpace(eanBarCode) && MarkingHelpers.IsGoodMarkable(row.Code, _useMarking, row.MarkingList.Count != 0, _date.EditValue.Value, out isRowMarkable2))
			{
				row.MarkingList.Add(item);
			}
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	private MaterialCacheRow GetGoodFromScanedMark(ref string eanBarCode)
	{
		byte[] bytes = Encoding.Unicode.GetBytes(_barcode.Text);
		for (int i = 0; i <= bytes.Length - 1; i += 2)
		{
			if (((bytes[i + 1] << 8) | bytes[i]) > 127)
			{
				_barcode.Text = "";
				_realText = "";
				ControlsHelper.SetFocus(_barcode);
				throw new ASException(ArmSoft.SME.Common.Resources.KeyboardLanguageForMarkingScanShouldBeEnglish);
			}
		}
		if (_eMarkReceiptList.SelectMany((EMarkReceiptSpecificationRow row) => row.MarkingList).ToList().Any((string c) => c == _barcode.Text))
		{
			_barcode.Text = "";
			_realText = "";
			ControlsHelper.SetFocus(_barcode);
			throw new ASException(ArmSoft.SME.Common.Resources.ValueIsInList);
		}
		MarkingHelper.ValidateMarkingCode(_barcode.Text);
		eanBarCode = MarkingHelpers.ExtractGTINFromDataMatrix(_barcode.Text, out var good);
		return good;
	}

	private void BarCode_PreviewTextInput(object sender, TextCompositionEventArgs e)
	{
		_realText += e.Text;
	}

	private void BarCode_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Invalid comparison between Unknown and I4
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if ((int)e.Key == 6 && (int)Keyboard.Modifiers == 0 && !_barcode.IsReadOnly && !string.IsNullOrWhiteSpace(_barcode.Text))
			{
				e.Handled = true;
				_barcode.Text = MarkingHelper.CleanScannerSymbols(_realText.Contains('\u001d') ? _realText : _barcode.Text);
				ScannerAdd();
			}
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
			_realText = "";
		}
	}

	private void AddMTToList(MaterialCacheRow goodsRow, ref EMarkReceiptSpecificationRow row, decimal quantity, string unitCode = "", string unitBrief = "")
	{
		if (_rowAddMode == "3")
		{
			AddMTToListNewRow(goodsRow, ref row, string.IsNullOrWhiteSpace(unitCode) ? goodsRow.Unit : unitCode, string.IsNullOrWhiteSpace(unitBrief) ? goodsRow.UnitRow.Brief : unitBrief, quantity);
		}
		else
		{
			AddNewRowOrMergeInner(goodsRow, ref row, _rowAddMode == "2", string.IsNullOrWhiteSpace(unitCode) ? goodsRow.Unit : unitCode, string.IsNullOrWhiteSpace(unitBrief) ? goodsRow.UnitRow.Brief : unitBrief, quantity);
		}
	}

	private void AddNewRowOrMergeInner(MaterialCacheRow goodRow, ref EMarkReceiptSpecificationRow row, bool forFocusedRow, string unitCode, string unitBrief, decimal quantity)
	{
		if (forFocusedRow && _eMarkReceiptList.Count != 0)
		{
			row = (EMarkReceiptSpecificationRow)_eMarkReceiptTableView.Grid.CurrentItem;
			if (row == null)
			{
				return;
			}
		}
		for (int i = 0; i <= _eMarkReceiptList.Count - 1; i++)
		{
			if ((!forFocusedRow || _eMarkReceiptList[i].RowId == _eMarkReceiptList[i].RowId) && goodRow.Id == _eMarkReceiptList[i].Id && unitCode == _eMarkReceiptList[i].UnitCode)
			{
				row = _eMarkReceiptList[i];
				FillMTInvGridRow(_eMarkReceiptList[i], goodRow, unitCode, unitBrief, quantity);
				int rowHandleByListIndex = _eMarkReceiptGrid.Grid.GetRowHandleByListIndex(i);
				_eMarkReceiptGrid.Grid.RefreshRow(rowHandleByListIndex);
				return;
			}
		}
		AddMTToListNewRow(goodRow, ref row, unitCode, unitBrief, quantity);
	}

	private void AddMTToListNewRow(MaterialCacheRow goodRow, ref EMarkReceiptSpecificationRow row, string unitCode, string unitBrief, decimal quantity)
	{
		row = new EMarkReceiptSpecificationRow();
		FillMTInvGridRow(row, goodRow, unitCode, unitBrief, quantity);
		row.RowId = _maxRowId;
		_eMarkReceiptList.Add(row);
		_maxRowId++;
		int rowHandleByListIndex = _eMarkReceiptGrid.Grid.GetRowHandleByListIndex(_eMarkReceiptList.Count - 1);
		_eMarkReceiptGrid.Grid.RefreshRow(rowHandleByListIndex);
		_eMarkReceiptGrid.ConfigureButtons();
	}

	private static void FillMTInvGridRow(EMarkReceiptSpecificationRow eMarkReceiptGridRow, MaterialCacheRow goodsRow, string unitCode, string unitBrief, decimal quantity = 1m)
	{
		if (goodsRow != null)
		{
			eMarkReceiptGridRow.Id = goodsRow.Id;
			eMarkReceiptGridRow.Code = goodsRow.MTCode;
			eMarkReceiptGridRow.Name = goodsRow.Caption;
		}
		eMarkReceiptGridRow.UnitCode = unitCode;
		eMarkReceiptGridRow.UnitBrief = unitBrief;
		eMarkReceiptGridRow.Quantity += quantity;
	}

	private void ExportEMarkReceipt_Click(object sender, RoutedEventArgs e)
	{
		//IL_034a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0350: Unknown result type (might be due to invalid IL or missing references)
		//IL_030d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0313: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if ((!PO.StoredInDb || IsChanged) && !SaveDocument())
			{
				return;
			}
			string text = Settings.ParametersService.EMarkReceiptDevice();
			if (string.IsNullOrWhiteSpace(text))
			{
				throw new ASException(string.Format(ArmSoft.SME.Trade.Common.Resources.UserFieldIsEmpty, Settings.LoginService.UserName, ArmSoft.SME.Trade.Common.Resources.EMarkReceiptDevice));
			}
			EMarkReceiptApp eMarkReceiptApp = Settings.DirectoryService.Load<EMarkReceiptApp>(DirectoryTypeSE.EMarkReceipt.Value, text, withoutAccessCheck: true);
			EMarkApiClient eMarkApiClient = new EMarkApiClient(new EMarkOptions
			{
				Token = eMarkReceiptApp.Token,
				RefreshToken = eMarkReceiptApp.RefreshToken,
				CRN = eMarkReceiptApp.CRN,
				Datamatrix_file_id = eMarkReceiptApp.DeviceId
			});
			PrintRequest printRequest = new PrintRequest
			{
				receipt = new List<EMarkReceiptItem>()
			};
			foreach (EMarkReceiptSpecificationRow eMarkReceipt in _eMarkReceiptList)
			{
				string text2 = eMarkReceipt.MarkingList.FirstOrDefault();
				if (string.IsNullOrWhiteSpace(text2))
				{
					throw new ASException(ArmSoft.SME.Trade.Common.Resources.ErrorsInList);
				}
				printRequest.receipt.Add(new EMarkReceiptItem
				{
					product_name = eMarkReceipt.Name,
					quantity = Convert.ToInt32(eMarkReceipt.Quantity),
					measurement_unit = eMarkReceipt.UnitBrief,
					tnved = eMarkReceipt.CPACode,
					gtin = ((text2.StartsWith("01") && text2.Substring(0, 16).IsNumeric()) ? text2.Substring(2, 14) : text2.Substring(0, 14)),
					emarks = eMarkReceipt.MarkingList.ToList()
				});
			}
			PrintResponse printResponse = eMarkApiClient.Print(printRequest);
			ToastNotificationViewModel toastNotificationViewModel = new ToastNotificationViewModel
			{
				Caption = ArmSoft.SME.Trade.Common.Resources.EMarkReceiptDoc,
				HistoryItem = new HistoryItem(HistoryItem.ItemCategory.Notification)
			};
			toastNotificationViewModel.HistoryItem.Description = toastNotificationViewModel.Caption;
			if (printResponse.succeed)
			{
				EMarkReceipt pO = PO;
				IDocumentService documentService = Settings.DocumentService;
				Guid iSN = PO.ISN;
				int num = 1;
				List<PropertyWithValue> list = new List<PropertyWithValue>(num);
				CollectionsMarshal.SetCount(list, num);
				CollectionsMarshal.AsSpan(list)[0] = new PropertyWithValue
				{
					Name = "EMarkReceiptNumber",
					Value = printResponse.receipt_id.ToString()
				};
				pO.TS = documentService.EditFields(iSN, list, new StoreRequest
				{
					WithoutAccessCheck = true,
					LogComment = ArmSoft.SME.Common.Resources.VatExport + " " + ArmSoft.SME.Trade.Common.Resources.EMarkReceiptApp + " - N" + printResponse.receipt_id
				});
				_eMarkReceiptNumber.Text = printResponse.receipt_id.ToString();
				SetControlsReadonly();
				toastNotificationViewModel.Text = ArmSoft.SME.Common.Resources.OperationSucceeded + Environment.NewLine + ArmSoft.SME.Trade.Common.Resources.EMarkReceiptNumber + printResponse.receipt_id;
				toastNotificationViewModel.Image = ImageService.GetImage("eMarkAccept_32.svg");
				toastNotificationViewModel.HistoryItem.Icon = "eMarkAccept_16.svg";
			}
			else
			{
				toastNotificationViewModel.Text = ArmSoft.SME.Common.Resources.OperationNotSucceededTryAgain;
				toastNotificationViewModel.Image = ImageService.GetImage("eMarkError_32.svg");
				toastNotificationViewModel.HistoryItem.Icon = "eMarkError_16.svg";
			}
			toastNotificationViewModel.HistoryItem.Details = toastNotificationViewModel.Text;
			Settings.HostingEnvironment.ShowNotificationAsync(toastNotificationViewModel);
		}
		catch (Exception ex)
		{
			if (ex is EMarkApiException ex2)
			{
				throw new ASException(ex2.Message);
			}
			Settings.ErrorHandler(ex);
		}
	}

	protected override void OnF8()
	{
		On_CodeEnterClick();
	}

	private void SelectMarking_Click(object sender, RoutedEventArgs e)
	{
		try
		{
			if (_eMarkReceiptTableView.Grid.CurrentItem == null)
			{
				return;
			}
			EMarkReceiptSpecificationRow eMarkReceiptSpecificationRow = (EMarkReceiptSpecificationRow)_eMarkReceiptTableView.Grid.CurrentItem;
			if (eMarkReceiptSpecificationRow == null)
			{
				return;
			}
			if (!MarkingHelpers.IsGoodMarkable(eMarkReceiptSpecificationRow.Code, _useMarking, eMarkReceiptSpecificationRow.MarkingList.Count != 0, _date.EditValue.Value, out var _))
			{
				if (string.IsNullOrWhiteSpace(eMarkReceiptSpecificationRow.Code))
				{
					throw new ASException(ArmSoft.SME.Common.Resources.ValidateEmptyCode);
				}
				ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Common.Resources.NotMarkable, ArmSoft.SME.Trade.Common.Resources.ProgramName, MessageBoxButton.OK, MessageBoxImage.Asterisk, MessageBoxResult.OK);
			}
			else
			{
				OpenMarkingSelectWindow(eMarkReceiptSpecificationRow);
				_eMarkReceiptGrid.Grid.RefreshData();
			}
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	private void On_CodeEnterClick()
	{
		if (_codeEnter == null || !_codeEnter.IsEnabled)
		{
			return;
		}
		_barcode.DoValidate();
		try
		{
			string text = _barcode.Text.Trim();
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
					_barcode.Text = "";
					_realText = "";
					ControlsHelper.SetFocus(_barcode);
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
				ControlsHelper.SetFocus(_barcode);
				throw new ASException(string.Format(ArmSoft.SME.Trade.Common.Resources.MTEgineInavidMaterial, text));
			}
			EMarkReceiptSpecificationRow row = null;
			AddMTToList(item, ref row, 0m);
			_barcode.Text = "";
			_realText = "";
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	private void CodeEnter_Click(object sender, RoutedEventArgs e)
	{
		On_CodeEnterClick();
	}

	private void EMarkReceiptTableView_FocusedRowHandleChangedEventArgs(object sender, FocusedRowHandleChangedEventArgs e)
	{
		int focusedRowHandle = _eMarkReceiptTableView.FocusedRowHandle;
		EMarkReceiptSpecificationRow eMarkReceiptSpecificationRow = (EMarkReceiptSpecificationRow)_eMarkReceiptGrid.Grid.GetRow(focusedRowHandle);
		if (eMarkReceiptSpecificationRow != null)
		{
			ChangeColumnAdditonalView(eMarkReceiptSpecificationRow.Code);
		}
	}

	private void ChangeColumnAdditonalView(string mtCode)
	{
		_unitCodeCol.DataView = new MTQntUnits(string.IsNullOrWhiteSpace(mtCode) ? "" : mtCode);
	}

	protected override void StoreSettings(RegistryKey rk)
	{
		base.StoreSettings(rk);
		_eMarkReceiptGrid.StoreSettings(rk);
	}

	protected override void RestoreSettings(double defaultWidth, double defaultHeight, RegistryKey rk)
	{
		base.RestoreSettings(defaultWidth, defaultHeight, rk);
		_eMarkReceiptGrid.RestoreSettings(rk);
	}
}
