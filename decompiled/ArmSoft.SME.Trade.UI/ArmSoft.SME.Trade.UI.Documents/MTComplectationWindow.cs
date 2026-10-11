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
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Extensions;
using ArmSoft.SME.Common.Models;
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
using ArmSoft.SME.Trade.UI.Documents.Goods;
using ArmSoft.SME.Trade.UI.DropDown;
using ArmSoft.SME.Trade.UI.Helpers;
using ArmSoft.SME.Trade.UI.OtherWindows;
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

[DocumentWindow(11)]
public class MTComplectationWindow : TradeDocumentWindow<MTComplectation>
{
	public class MTComplectationSpecificationRow : DocumentGridRow
	{
		[ObservableProperty]
		private int rowID;

		[ObservableProperty]
		private short rowNum;

		[ObservableProperty]
		private int itemID = -1;

		[ObservableProperty]
		private string itemCode = string.Empty;

		[ObservableProperty]
		private string itemType = string.Empty;

		[ObservableProperty]
		private string itemName = string.Empty;

		[ObservableProperty]
		private decimal quantity;

		[ObservableProperty]
		private string unitCode = string.Empty;

		[ObservableProperty]
		private string unitBrief = string.Empty;

		[ObservableProperty]
		private decimal summa;

		[ObservableProperty]
		private bool? isPartySelected;

		public string PartyMethodName
		{
			get
			{
				if (!IsPartySelected.HasValue)
				{
					return string.Empty;
				}
				if (IsPartySelected.Value)
				{
					return ArmSoft.SME.Common.Resources.PartySelected;
				}
				return ArmSoft.SME.Common.Resources.AutoSelection;
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
		public int ItemID
		{
			get
			{
				return itemID;
			}
			set
			{
				if (!EqualityComparer<int>.Default.Equals(itemID, value))
				{
					OnPropertyChanging(__KnownINotifyPropertyChangingArgs.ItemID);
					itemID = value;
					OnPropertyChanged(__KnownINotifyPropertyChangedArgs.ItemID);
				}
			}
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public string ItemCode
		{
			get
			{
				return itemCode;
			}
			[MemberNotNull("itemCode")]
			set
			{
				if (!EqualityComparer<string>.Default.Equals(itemCode, value))
				{
					OnPropertyChanging(__KnownINotifyPropertyChangingArgs.ItemCode);
					itemCode = value;
					OnItemCodeChanged(value);
					OnPropertyChanged(__KnownINotifyPropertyChangedArgs.ItemCode);
				}
			}
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public string ItemType
		{
			get
			{
				return itemType;
			}
			[MemberNotNull("itemType")]
			set
			{
				if (!EqualityComparer<string>.Default.Equals(itemType, value))
				{
					OnPropertyChanging(__KnownINotifyPropertyChangingArgs.ItemType);
					itemType = value;
					OnPropertyChanged(__KnownINotifyPropertyChangedArgs.ItemType);
				}
			}
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public string ItemName
		{
			get
			{
				return itemName;
			}
			[MemberNotNull("itemName")]
			set
			{
				if (!EqualityComparer<string>.Default.Equals(itemName, value))
				{
					OnPropertyChanging(__KnownINotifyPropertyChangingArgs.ItemName);
					itemName = value;
					OnItemNameChanged(value);
					OnPropertyChanged(__KnownINotifyPropertyChangedArgs.ItemName);
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
					OnUnitBriefChanged(value);
					OnPropertyChanged(__KnownINotifyPropertyChangedArgs.UnitBrief);
				}
			}
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public decimal Summa
		{
			get
			{
				return summa;
			}
			set
			{
				if (!EqualityComparer<decimal>.Default.Equals(summa, value))
				{
					OnPropertyChanging(__KnownINotifyPropertyChangingArgs.Summa);
					summa = value;
					OnPropertyChanged(__KnownINotifyPropertyChangedArgs.Summa);
				}
			}
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public bool? IsPartySelected
		{
			get
			{
				return isPartySelected;
			}
			set
			{
				if (!EqualityComparer<bool?>.Default.Equals(isPartySelected, value))
				{
					OnPropertyChanging(__KnownINotifyPropertyChangingArgs.IsPartySelected);
					isPartySelected = value;
					OnPropertyChanged(__KnownINotifyPropertyChangedArgs.IsPartySelected);
				}
			}
		}

		static MTComplectationSpecificationRow()
		{
			DocumentGridRow.RegisterPropertyValidators(typeof(MTComplectationSpecificationRow), cacheValidationResults: true, new Dictionary<string, Action<DocumentGridRow, DevExpress.XtraEditors.DXErrorProvider.ErrorInfo>>
			{
				{
					"ItemCode",
					(DocumentGridRow row, DevExpress.XtraEditors.DXErrorProvider.ErrorInfo info) =>
					{
						ValidateItemCode((MTComplectationSpecificationRow)row, info);
					}
				},
				{
					"UnitCode",
					(DocumentGridRow row, DevExpress.XtraEditors.DXErrorProvider.ErrorInfo info) =>
					{
						ValidateUnitCode((MTComplectationSpecificationRow)row, info);
					}
				},
				{
					"Quantity",
					(DocumentGridRow row, DevExpress.XtraEditors.DXErrorProvider.ErrorInfo info) =>
					{
						ValidateQuantity((MTComplectationSpecificationRow)row, info);
					}
				}
			});
		}

		private static void ValidateItemCode(MTComplectationSpecificationRow row, DevExpress.XtraEditors.DXErrorProvider.ErrorInfo info)
		{
			if (row.ItemType == ItemTypesSE.Goods.Value)
			{
				TradePropertyValidator.ValidateMaterialCode(row.ItemCode, info, TradeAllDocDefinitions.MTComplectation.Grids["ComplectPartsList"].Columns["ItemCode"]);
			}
			else
			{
				TradePropertyValidator.ValidateServiceCode(row.ItemCode, info, TradeAllDocDefinitions.MTComplectation.Grids["ComplectPartsList"].Columns["ItemCode"]);
			}
		}

		private static void ValidateUnitCode(MTComplectationSpecificationRow row, DevExpress.XtraEditors.DXErrorProvider.ErrorInfo info)
		{
			TradePropertyValidator.ValidateGoodUnitCode(row.UnitCode, row.ItemCode, info, TradeAllDocDefinitions.MTComplectation.Grids["ComplectPartsList"].Columns["UnitCode"]);
		}

		private static void ValidateQuantity(MTComplectationSpecificationRow row, DevExpress.XtraEditors.DXErrorProvider.ErrorInfo info)
		{
			PropertyValidator.ValidateNumber(row.Quantity, info, TradeAllDocDefinitions.MTComplectation.Grids["ComplectPartsList"].Columns["Quantity"]);
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		private void OnItemCodeChanged(string value)
		{
			ResetForceValidationBit("ItemCode");
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		private void OnItemNameChanged(string value)
		{
			ResetForceValidationBit("ItemName");
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		private void OnQuantityChanged(decimal value)
		{
			ResetForceValidationBit("Quantity", "Quantity");
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		private void OnUnitCodeChanged(string value)
		{
			ResetForceValidationBit("UnitCode");
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		private void OnUnitBriefChanged(string value)
		{
			ResetForceValidationBit("UnitBrief");
		}
	}

	public class MTComplectationFilteredSearchList : IResultList
	{
		private readonly MTComplectation _doc;

		private readonly ObservableCollection<MTComplectationSpecificationRow> _itemsSource;

		public List<FilteredSearchGridColumn> Columns => new List<FilteredSearchGridColumn>
		{
			new FilteredSearchGridColumn
			{
				Header = ArmSoft.SME.Common.Resources.Code,
				FieldName = "ItemCode",
				CustomWidth = 125.0
			},
			new FilteredSearchGridColumn
			{
				Header = ArmSoft.SME.Common.Resources.Name,
				FieldName = "ItemName",
				CustomWidth = 300.0
			},
			new FilteredSearchGridColumn
			{
				Header = ArmSoft.SME.Common.Resources.Quantity,
				FieldName = "Quantity",
				AllowEditing = true,
				NumericFormat = DecimalExtension.GetCurrencyFormat(3, 3)
			}
		};

		public IEnumerable<DocumentGridRow> ItemsSource => _itemsSource;

		public MTComplectationFilteredSearchList(MTComplectation doc)
		{
			_doc = doc;
			_itemsSource = new ObservableCollection<MTComplectationSpecificationRow>();
		}

		public int AddToItemsSource(object key, params object[] args)
		{
			string mtCode = key.ToString();
			decimal num = Convert.ToDecimal(args[0]);
			int num2 = _itemsSource.FindIndex((MTComplectationSpecificationRow x) => x.ItemCode == mtCode);
			if (num2 == -1)
			{
				MaterialCacheRow item = TradeSettings.TradeCacheService.Material.GetItem(mtCode);
				MTComplectationSpecificationRow mTComplectationSpecificationRow = new MTComplectationSpecificationRow
				{
					ItemType = item.Type,
					ItemID = item.Id,
					ItemCode = item.MTCode,
					ItemName = item.Caption,
					UnitCode = item.Unit,
					UnitBrief = item.UnitRow.Brief
				};
				if (item.Type == ItemTypesSE.Goods.Value)
				{
					mTComplectationSpecificationRow.IsPartySelected = false;
				}
				_itemsSource.Add(mTComplectationSpecificationRow);
				num2 = _itemsSource.Count - 1;
			}
			_itemsSource[num2].Quantity += num;
			return num2;
		}
	}

	private TextEditExt _barCode;

	private Button _codeEnter;

	public DropDownControl _storageExpense;

	private DropDownControl _storageIncome;

	public DropDownControl _complect;

	private PopupCalcEditExt _quantity;

	private DropDownControl _quantityUnit;

	private ComboBoxEditExt _comment;

	private DropDownControl _owner;

	private BarEditItemExt _loadComplectPartsList;

	private DropDownControl _country;

	private DropDownControl _producer;

	private DropDownControl _property1;

	private DropDownControl _property2;

	private DateEditControl _useLife;

	public int _maxRowID;

	private TableView _mtComplectTableView;

	private BarEditItemExt _filteredSearch;

	public ObservableCollection<MTComplectationSpecificationRow> _mtComplectPartsList;

	public DocumentGridControl _mtComplectPartsGrid;

	private DocumentGridColumn _itemTypeCol;

	private DocumentGridColumn _itemCodeCol;

	private DocumentGridColumn _itemNameCol;

	private DocumentGridColumn _unitCodeCol;

	private DocumentGridColumn _quantityCol;

	private DocumentGridColumn _summaCol;

	private DocumentGridColumn _partyMethodCol;

	private string _rowAddMode = Settings.ParametersService.ScannerMode();

	private string _goodsTypes_Goods = GoodsTypesSE.Good;

	private ReportBarButtonItem _barButtonJumpTo;

	private ReportBarButtonItem _barButtonGoodsOperationsJournal;

	private BarSubItem _barSubItemActions = new BarSubItem();

	private BarSubItem _barSubItemJournals = new BarSubItem();

	protected override double WidthValue => 1090.0;

	protected override double HeightValue => 600.0;

	protected override double MinWidthValue => 620.0;

	protected override double MinHeightValue => 250.0;

	public MTComplectationWindow()
	{
	}

	public MTComplectationWindow(MTComplectation doc)
		: base(doc)
	{
	}

	protected override void ConstructUI()
	{
		base.ConstructUI();
		LayoutGroup container = AddLayoutGroup(null, LayoutGroupView.Group, Orientation.Horizontal);
		_date = AddDateControl(container, Definition.Fields["DocDate"]);
		_date.Margin = new Thickness(0.0, 0.0, 10.0, 0.0);
		_docNum = AddButtonEditControl(container, Definition.Fields["DocNum"], 1);
		_docNum.AllowMagicButton = true;
		_docNum.MagicToolTip = ArmSoft.SME.Common.Resources.DocNextNumbers;
		_docNum.MagicClick += new SafeDocumentEventHandler<MagicClickEventArgs>(DocNum_MagicClick, this).Handler;
		_docNum.EditValueChanged += new SafeDocumentEventHandler<EditValueChangedEventArgs>(DocNum_EditValueChanged, this).Handler;
		_storageExpense = AddDropDownControl(null, Definition.Fields["StorageExpense"], new Storages(false), allowAdditionButtons: false, fieldWithCaption: true, HorizontalAlignment.Stretch);
		_storageExpense.EditValueChanged += new SafeDocumentEventHandler<EditValueChangedEventArgs>(StorageExpense_EditValueChanged, this).Handler;
		_storageIncome = AddDropDownControl(null, Definition.Fields["StorageIncome"], new Storages(false), allowAdditionButtons: false, fieldWithCaption: true, HorizontalAlignment.Stretch);
		LayoutGroup container2 = AddLayoutGroup(null, LayoutGroupView.Group, Orientation.Horizontal);
		_barCode = AddStringControl(container2, ArmSoft.SME.Common.Resources.BarCode + " (F2)", isRequired: false, 20, HorizontalAlignment.Stretch);
		_barCode.NullText = ArmSoft.SME.Trade.Common.Resources.BarCodeAndCodeInputNullText;
		_barCode.KeyDown += new SafeDocumentEventHandler<KeyEventArgs>(BarCode_KeyDown, this).Handler;
		string text = ArmSoft.SME.Trade.Common.Resources.cmdCodeEnter + " (F8)";
		_codeEnter = AddButton(container2, null, text, null, text, isTabStop: true);
		_codeEnter.Click += new SafeDocumentEventHandler<RoutedEventArgs>(CodeEnter_Click, this).Handler;
		DXTabControl tc = AddTabControl(null);
		LayoutGroup container3 = AddTabbedGroup(tc, ArmSoft.SME.Common.Resources.General, "General");
		LayoutGroup container4 = AddLayoutGroup(container3, LayoutGroupView.Group, Orientation.Horizontal);
		LayoutGroup container5 = AddLayoutGroup(container4, LayoutGroupView.Group, Orientation.Vertical);
		LayoutGroup container6 = AddLayoutGroup(container4, LayoutGroupView.Group, Orientation.Vertical);
		_complect = AddDropDownControl(container5, Definition.Fields["MTComplectCode"], new Complects(), allowAdditionButtons: true, fieldWithCaption: true, HorizontalAlignment.Stretch);
		_complect.EditValueChanged += new SafeDocumentEventHandler<EditValueChangedEventArgs>(Complect_EditValueChanged, this).Handler;
		_quantityUnit = AddDropDownControl(container5, Definition.Fields["QuantityUnit"], new QntUnits(), allowAdditionButtons: false, fieldWithCaption: true, HorizontalAlignment.Stretch);
		_quantity = AddNumericControl(container5, Definition.Fields["Quantity"]);
		_country = AddDropDownControl(container6, Definition.Fields["Country"], new Countries(), allowAdditionButtons: false, fieldWithCaption: true, HorizontalAlignment.Stretch);
		_producer = AddDropDownControl(container6, Definition.Fields["Producer"], new Producer(), allowAdditionButtons: false, fieldWithCaption: true, HorizontalAlignment.Stretch);
		_property1 = AddDropDownControl(container6, Definition.Fields["Property1"], new PropertyList(), allowAdditionButtons: true, fieldWithCaption: true, HorizontalAlignment.Stretch);
		_useLife = AddDateControl(container5, Definition.Fields["UseLife"]);
		_property2 = AddDropDownControl(container6, Definition.Fields["Property2"], new PropertyList(), allowAdditionButtons: true, fieldWithCaption: true, HorizontalAlignment.Stretch);
		if (!Settings.ParametersService.ShowPartyProducer())
		{
			((LayoutItemWithRequired)(object)_producer.Parent).Visibility = Visibility.Collapsed;
		}
		if (!Settings.ParametersService.ShowPartyCountry())
		{
			((LayoutItemWithRequired)(object)_country.Parent).Visibility = Visibility.Collapsed;
		}
		if (!Settings.ParametersService.ShowPartyUSEABLEDATE())
		{
			((LayoutItemWithRequired)(object)_useLife.Parent).Visibility = Visibility.Collapsed;
		}
		if (!Settings.ParametersService.ShowPartyProperty1())
		{
			((LayoutItemWithRequired)(object)_property1.Parent).Visibility = Visibility.Collapsed;
		}
		if (!Settings.ParametersService.ShowPartyProperty2())
		{
			((LayoutItemWithRequired)(object)_property2.Parent).Visibility = Visibility.Collapsed;
		}
		_mtComplectPartsGrid = AddGrid(container3, Definition.Grids["ComplectPartsList"], HorizontalAlignment.Stretch, VerticalAlignment.Stretch, isSupportDialogMode: true);
		_mtComplectPartsGrid.ToolTip = ArmSoft.SME.Trade.Common.Resources.MTComplectsParts;
		_mtComplectPartsGrid.IsRequired = true;
		_mtComplectPartsGrid.AllowMoveUp = true;
		_mtComplectPartsGrid.AllowMoveDown = true;
		_mtComplectTableView = _mtComplectPartsGrid.View;
		_mtComplectTableView.FocusedRowHandleChanged += new SafeDocumentEventHandler<FocusedRowHandleChangedEventArgs>(MTComplectTableView_FocusedRowHandleChanged, this).Handler;
		_mtComplectTableView.ShowingEditor += new SafeDocumentEventHandler<ShowingEditorEventArgs>(MTComplectTableView_ShowingEditor, this).Handler;
		_mtComplectTableView.CellValueChanged += new SafeDocumentEventHandler<CellValueChangedEventArgs>(MTComplectTableView_CellValueChanged, this).Handler;
		_loadComplectPartsList = _mtComplectPartsGrid.AddButton("fill_down_16.svg", ArmSoft.SME.Common.Resources.ButtonLoad, ArmSoft.SME.Common.Resources.ButtonLoad, 90.0);
		_loadComplectPartsList.Click += new SafeDocumentEventHandler<RoutedEventArgs>(LoadComplectPartsList_Click, this).Handler;
		_mtComplectPartsGrid.AddingRow += new SafeDocumentEventHandler<DocumentGridAddingRowEventArgs>(MTComplectPartsGrid_AddingRow, this).Handler;
		_mtComplectPartsGrid.DuplicatingRow += new SafeDocumentEventHandler<DocumentGridDuplicatingRowEventArgs>(MTComplectPartsGrid_DuplicatingRow, this).Handler;
		_mtComplectPartsGrid.MovingRowUp += new SafeDocumentEventHandler<DocumentGridMovingRowUpOrDownEventArgs>(MTComplectPartsGrid_MovingRowUp, this).Handler;
		_mtComplectPartsGrid.MovingRowDown += new SafeDocumentEventHandler<DocumentGridMovingRowUpOrDownEventArgs>(MTComplectPartsGrid_MovingRowDown, this).Handler;
		_mtComplectPartsGrid.AddingRowWithDialog += new SafeDocumentEventHandler<DocumentGridAddingRowEventArgs>(MTComplectPartsGrid_AddingRowWithDialog, this).Handler;
		_mtComplectPartsGrid.DuplicatingRowWithDialog += new SafeDocumentEventHandler<DocumentGridDuplicatingRowEventArgs>(MTComplectPartsGrid_DuplicatingRowWithDialog, this).Handler;
		_mtComplectPartsGrid.EditingRowWithDialog += new SafeDocumentEventHandler<DocumentGridEditingRowEventArgs>(MTComplectPartsGrid_EditingRowWithDialog, this).Handler;
		_mtComplectPartsGrid.DeletingRow += new SafeDocumentEventHandler<DocumentGridDeletingRowEventArgs>(MTComplectPartsGrid_DeletingRow, this).Handler;
		_mtComplectPartsGrid.DeletedRow += new SafeDocumentEventHandler<EventArgs>(MTComplectPartsGrid_DeletedRow, this).Handler;
		_mtComplectPartsGrid.ClickDropDown += new SafeDocumentEventHandler<DocumentGridClickDropDownEventArgs>(MTComplectPartsGrid_ClickDropDown, this).Handler;
		_filteredSearch = _mtComplectPartsGrid.AddButton(null, ArmSoft.SME.Trade.Common.Resources.GroupAddition, ArmSoft.SME.Trade.Common.Resources.GroupAddition, 130.0);
		_filteredSearch.Click += new SafeDocumentEventHandler<RoutedEventArgs>(FilteredSearch_Click, this).Handler;
		_mtComplectPartsGrid.AddRowIndicator(35);
		_itemTypeCol = _mtComplectPartsGrid.AddSmartEnumDropDownColumn(Definition.Grids["ComplectPartsList"].Columns["ItemType"]);
		_itemTypeCol.Width = 55.0;
		_itemCodeCol = _mtComplectPartsGrid.AddDropDownColumn(Definition.Grids["ComplectPartsList"].Columns["ItemCode"], new GoodsFilter(), allowAdditionButtons: true, isOwnProcessingClickDropDown: true);
		_itemCodeCol.Width = 125.0;
		_itemNameCol = _mtComplectPartsGrid.AddStringColumn(Definition.Grids["ComplectPartsList"].Columns["ItemName"]);
		_itemNameCol.Width = 225.0;
		_unitCodeCol = _mtComplectPartsGrid.AddDropDownColumn(Definition.Grids["ComplectPartsList"].Columns["UnitCode"]);
		_mtComplectPartsGrid.AddStringColumn(Definition.Grids["ComplectPartsList"].Columns["UnitBrief"]).AllowEditing = DefaultBoolean.False;
		_quantityCol = _mtComplectPartsGrid.AddNumberColumn(Definition.Grids["ComplectPartsList"].Columns["Quantity"], showTotal: true);
		_summaCol = _mtComplectPartsGrid.AddNumberColumn(Definition.Grids["ComplectPartsList"].Columns["Summa"], showTotal: true);
		_summaCol.Visible = Settings.ParametersService.ShowGoodsCostInReportsAndDocs();
		_summaCol.ShowInColumnChooser = _summaCol.Visible;
		_partyMethodCol = _mtComplectPartsGrid.AddDropDownColumn("PartyMethodName", ArmSoft.SME.Common.Resources.Party, 19, trimValue: false, GlyphKind.Regular);
		((ButtonEditSettings)_partyMethodCol.EditSettings).IsTextEditable = false;
		_mtComplectPartsList = new ObservableCollection<MTComplectationSpecificationRow>();
		foreach (GridColumn column in _mtComplectPartsGrid.Grid.Columns)
		{
			column.AllowSorting = DefaultBoolean.True;
			column.AllowColumnFiltering = DefaultBoolean.True;
			column.FilterPopupMode = FilterPopupMode.CheckedList;
		}
		LayoutGroup container7 = AddLayoutGroup(container3, LayoutGroupView.Group, Orientation.Horizontal);
		_comment = AddComboBoxControl(container7, Definition.Fields["Comment"], TradeSettings.TradeCacheService.StandardComments.GetCaptionsList(), HorizontalAlignment.Stretch);
		_owner = AddDropDownControl(container7, Definition.Fields["Owner"], new UsersFilter());
		_owner.CodeWidth = 12;
		_owner.ShowComment = false;
		_owner.InnerCodeEdit.Width = _owner.InnerCodeEdit.MinWidth;
		if (!Settings.LoginService.IsAdmin && Settings.ParametersService.DenyAccessToOtherOwnerDocs())
		{
			((LayoutItemWithRequired)(object)_owner.Parent).Visibility = Visibility.Collapsed;
			_owner.Visibility = Visibility.Collapsed;
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
		Ribbon.ToolbarItemLinks.Add(_barSubItemJournals.ConvertForToolbar("t_report_16.svg"));
	}

	protected override void AddItemsToRibbon()
	{
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		base.AddItemsToRibbon();
		CreateRibbonPageGroupActions();
		CreateRibbonPageGroupReports();
		_barSubItemJournals.Content = "";
		_barSubItemJournals.Glyph = ImageService.GetImage("report_16.svg");
		_barSubItemJournals.LargeGlyph = ImageService.GetImage("report_32.svg");
		_barButtonJumpTo = new ReportBarButtonItem(DataProviderTypeSE.AllDocumentsJournal, showDialog: false, ArmSoft.SME.Common.Resources.JumpTo, null, "doc_JumpTo_16.svg", "doc_JumpTo_32.svg");
		_barButtonJumpTo.AddInBarSubItem(ref _ribbonPageGroupActions, ref _barSubItemActions);
		_barButtonGoodsOperationsJournal = new ReportBarButtonItem(DataProviderTypeSE.GoodsRepOperations.Value, showDialog: false, ArmSoft.SME.Trade.Common.Resources.MTOperations);
		_barButtonGoodsOperationsJournal.AddInBarSubItem(ref _ribbonPageGroupReports, ref _barSubItemJournals);
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
		ReportBarButtonItem barButtonGoodsOperationsJournal = _barButtonGoodsOperationsJournal;
		num = 2;
		List<PropertyWithValue> list2 = new List<PropertyWithValue>(num);
		CollectionsMarshal.SetCount(list2, num);
		Span<PropertyWithValue> span2 = CollectionsMarshal.AsSpan(list2);
		span2[0] = new PropertyWithValue("DocISN", PO.ISN.ToString());
		span2[1] = new PropertyWithValue("AdditionalCaption", GetReportsAdditionalCaption());
		barButtonGoodsOperationsJournal.PropertyWithValues = list2;
	}

	protected override void SetReportBarButtonsEnable(bool isEnabled)
	{
		base.SetReportBarButtonsEnable(isEnabled);
		_barButtonJumpTo.IsEnabled = isEnabled;
		_barButtonGoodsOperationsJournal.IsEnabled = isEnabled;
	}

	private void MTComplectPartsGrid_ClickDropDown(object sender, DocumentGridClickDropDownEventArgs e)
	{
		try
		{
			MTComplectationSpecificationRow mTComplectationSpecificationRow = (MTComplectationSpecificationRow)_mtComplectPartsGrid.Grid.GetRow(e.RowHandle);
			if (e.Column == _itemCodeCol)
			{
				if (mTComplectationSpecificationRow.ItemType == ItemTypesSE.Services)
				{
					GoodsFilter goodsFilter = new GoodsFilter(shortMode: true, ItemTypesSE.Services, "", isShowOnlyInPriceList: true);
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
					return;
				}
				if (!_date.DoValidate())
				{
					throw new ASException(ArmSoft.SME.Common.Resources.ValidateEmptyDate);
				}
				bool showAll = TradeSettings.CreateTradeClient().CommonServices.IsNegativExpAllow(_storageExpense.Code);
				GoodsAvailabilityFilter goodsAvailabilityFilter = new GoodsAvailabilityFilter(_date.EditValue.Value, _storageExpense.Code, forRetail: false, includeServices: false, showAll);
				ModalBrowser modalBrowser2 = new ModalBrowser(goodsAvailabilityFilter)
				{
					LookUpValue = e.Value,
					Owner = e.Owner,
					Left = e.Left,
					Top = e.Top
				};
				if (modalBrowser2.Show())
				{
					e.NewValue = goodsAvailabilityFilter.ValueMember(modalBrowser2.SelectedRowIndex);
				}
			}
			else if (e.Column == _unitCodeCol)
			{
				if (mTComplectationSpecificationRow != null)
				{
					MTQntUnits mTQntUnits = new MTQntUnits(mTComplectationSpecificationRow.ItemCode);
					ModalBrowser modalBrowser3 = new ModalBrowser(mTQntUnits)
					{
						LookUpValue = e.Value,
						Owner = e.Owner,
						Left = e.Left,
						Top = e.Top
					};
					if (modalBrowser3.Show())
					{
						e.NewValue = mTQntUnits.ValueMember(modalBrowser3.SelectedRowIndex);
					}
				}
			}
			else
			{
				if (e.Column != _partyMethodCol || mTComplectationSpecificationRow.ItemType != _goodsTypes_Goods || mTComplectationSpecificationRow == null)
				{
					return;
				}
				if (!_date.DoValidate())
				{
					throw new ASException(ArmSoft.SME.Common.Resources.ValidateEmptyDate);
				}
				if (string.IsNullOrWhiteSpace(mTComplectationSpecificationRow.ItemCode))
				{
					throw new ASException(ArmSoft.SME.Common.Resources.ValidateEmptyCode);
				}
				if (string.IsNullOrWhiteSpace(_storageExpense.Code))
				{
					throw new ASException(ArmSoft.SME.Common.Resources.ValidEmptyExpenseStorage);
				}
				decimal quantity = (mTComplectationSpecificationRow.Quantity * PriceHelper.GetMTQntUnitCoeff(mTComplectationSpecificationRow.ItemCode, mTComplectationSpecificationRow.UnitCode)).RoundAs(3);
				MTSelectPartyWindow mTSelectPartyWindow = new MTSelectPartyWindow(_storageExpense.Code, _date.EditValue.Value, mTComplectationSpecificationRow.ItemCode, mTComplectationSpecificationRow.UnitCode, quantity, mTComplectationSpecificationRow.RowID, PO.StoredInDb && !IsDraftOrDeleted)
				{
					MTExpDocSelectedPartiesList = PO.MTExpDocSelectedPartiesList
				};
				mTSelectPartyWindow.Load();
				mTSelectPartyWindow.Owner = this;
				if (mTSelectPartyWindow.ShowDialog().Value)
				{
					PO.MTExpDocSelectedPartiesList = mTSelectPartyWindow.MTExpDocSelectedPartiesList;
					if (mTSelectPartyWindow.IsPartySelected.HasValue)
					{
						MaterialCacheRow item = TradeSettings.TradeCacheService.Material.GetItem(mTSelectPartyWindow.MTCode);
						mTComplectationSpecificationRow.ItemCode = mTSelectPartyWindow.MTCode;
						mTComplectationSpecificationRow.ItemID = item.Id;
						mTComplectationSpecificationRow.ItemName = item.Caption;
						mTComplectationSpecificationRow.UnitCode = item.Unit;
						mTComplectationSpecificationRow.UnitBrief = item.UnitRow.Brief;
						mTComplectationSpecificationRow.Quantity = mTSelectPartyWindow.TotalExpQty;
						mTComplectationSpecificationRow.Summa = mTSelectPartyWindow.TotalExpSumm;
						mTComplectationSpecificationRow.IsPartySelected = mTSelectPartyWindow.IsPartySelected;
						mTComplectationSpecificationRow.ValidateWithoutResponse();
						_mtComplectPartsGrid.Grid.RefreshRow(e.RowHandle);
						PO.MTExpDocSelectedPartiesList = mTSelectPartyWindow.MTExpDocSelectedPartiesList;
					}
				}
			}
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	private void LoadComplectPartsList_Click(object sender, RoutedEventArgs e)
	{
		try
		{
			ValidateDocFields();
			if (_mtComplectPartsList.Count > 0)
			{
				if (ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Common.Resources.RefreshComplectGrid, Settings.ProgramName, MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No) == MessageBoxResult.No)
				{
					return;
				}
				_mtComplectPartsList.Clear();
				PO.MTExpDocSelectedPartiesList.Clear();
			}
			MaterialCacheRow item = TradeSettings.TradeCacheService.Material.GetItem(_complect.Code);
			decimal value = _quantity.Value;
			if (item == null)
			{
				return;
			}
			value *= TradeSettings.TradeCacheService.Material.GetQntUnit(item, _quantityUnit.Code)?.Coef ?? 1m;
			List<MaterialCacheRow.ComplectPartRow> complectParts = TradeSettings.TradeCacheService.Material.GetItem(item.MTCode).ComplectParts;
			if (complectParts.Count > 0)
			{
				if (ExistMaterialInComplectsParts(complectParts) && !_storageExpense.DoValidate())
				{
					ControlsHelper.SetFocus(_storageExpense);
					throw new ASException(string.Format(ArmSoft.SME.Trade.Common.Resources.StorageNotExist, _storageExpense.Code));
				}
				_maxRowID = 0;
				foreach (MaterialCacheRow.ComplectPartRow item2 in complectParts)
				{
					MTComplectationSpecificationRow mTComplectationSpecificationRow = new MTComplectationSpecificationRow();
					mTComplectationSpecificationRow.ItemType = item2.ItemType;
					mTComplectationSpecificationRow.ItemID = item2.ItemId;
					mTComplectationSpecificationRow.UnitCode = item2.QntUnit;
					mTComplectationSpecificationRow.Quantity = (item2.Quantity * value).RoundAs(3);
					if (item2.ComplectQnt != 0m)
					{
						mTComplectationSpecificationRow.Quantity = (mTComplectationSpecificationRow.Quantity / item2.ComplectQnt).RoundAs(3);
					}
					MaterialCacheRow itemById = TradeSettings.TradeCacheService.Material.GetItemById(item2.ItemId, throwExceptionNotExits: true);
					mTComplectationSpecificationRow.ItemCode = itemById.MTCode;
					mTComplectationSpecificationRow.ItemName = itemById.Caption;
					mTComplectationSpecificationRow.UnitBrief = TradeSettings.TradeCacheService.QntUnit.GetItem(item2.QntUnit).Brief;
					mTComplectationSpecificationRow.Summa = 0m;
					if (mTComplectationSpecificationRow.ItemType != _goodsTypes_Goods)
					{
						mTComplectationSpecificationRow.Summa = (item2.Sum * mTComplectationSpecificationRow.Quantity).RoundAs(3);
					}
					if (mTComplectationSpecificationRow.ItemType == _goodsTypes_Goods)
					{
						mTComplectationSpecificationRow.IsPartySelected = false;
					}
					mTComplectationSpecificationRow.RowID = _maxRowID;
					_mtComplectPartsList.Add(mTComplectationSpecificationRow);
					_maxRowID++;
				}
			}
			_mtComplectPartsGrid.Grid.RefreshData();
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	private void MTComplectTableView_CellValueChanged(object sender, CellValueChangedEventArgs e)
	{
		MTComplectationSpecificationRow mtComplectPartsGridRow = (MTComplectationSpecificationRow)_mtComplectPartsGrid.Grid.GetRow(e.RowHandle);
		if (mtComplectPartsGridRow == null)
		{
			return;
		}
		string text = string.Empty;
		string text2 = string.Empty;
		if (e.OldValue != null)
		{
			text = e.OldValue.ToString();
		}
		if (e.Value != null)
		{
			text2 = e.Value.ToString();
		}
		if (e.Column == _itemTypeCol)
		{
			if (text != text2)
			{
				mtComplectPartsGridRow.ItemCode = "";
				mtComplectPartsGridRow.ItemID = -1;
				mtComplectPartsGridRow.ItemName = "";
				mtComplectPartsGridRow.Quantity = 0m;
				mtComplectPartsGridRow.Summa = 0m;
				mtComplectPartsGridRow.UnitCode = "";
				mtComplectPartsGridRow.UnitBrief = "";
				mtComplectPartsGridRow.IsPartySelected = false;
			}
			if (text == _goodsTypes_Goods)
			{
				PO.MTExpDocSelectedPartiesList.RemoveAll((PartyRow x) => x.GridRow == mtComplectPartsGridRow.RowID);
			}
			if (text2 != _goodsTypes_Goods)
			{
				mtComplectPartsGridRow.IsPartySelected = null;
			}
			if (text2 != _goodsTypes_Goods && text2 != ItemTypesSE.Services)
			{
				mtComplectPartsGridRow.ItemType = text;
			}
			ChangeItemCodeColumnAdditonalView(text2, "");
			return;
		}
		if (e.Column == _itemCodeCol)
		{
			try
			{
				if (e.Value == null || string.IsNullOrWhiteSpace(e.Value.ToString()))
				{
					PO.MTExpDocSelectedPartiesList.RemoveAll((PartyRow x) => x.GridRow == mtComplectPartsGridRow.RowID);
					mtComplectPartsGridRow.ItemID = -1;
					mtComplectPartsGridRow.ItemCode = "";
					mtComplectPartsGridRow.ItemName = "";
					mtComplectPartsGridRow.UnitBrief = "";
					mtComplectPartsGridRow.UnitCode = "";
					mtComplectPartsGridRow.Quantity = 0m;
					mtComplectPartsGridRow.IsPartySelected = null;
					mtComplectPartsGridRow.Summa = 0m;
				}
				else
				{
					if (!string.IsNullOrWhiteSpace(text) && text != e.Value.ToString() && PO.MTExpDocSelectedPartiesList != null)
					{
						PO.MTExpDocSelectedPartiesList.RemoveAll((PartyRow x) => x.GridRow == mtComplectPartsGridRow.RowID);
						mtComplectPartsGridRow.IsPartySelected = false;
						mtComplectPartsGridRow.Summa = 0m;
					}
					string text3 = e.Value.ToString();
					if (text3 == _complect.Code)
					{
						mtComplectPartsGridRow.ItemCode = "";
						throw new ASException(ArmSoft.SME.Common.Resources.InvalidComplectMt);
					}
					foreach (MTComplectationSpecificationRow mtComplectParts in _mtComplectPartsList)
					{
						if (mtComplectPartsGridRow != mtComplectParts && text3 == mtComplectParts.ItemCode)
						{
							mtComplectPartsGridRow.ItemCode = "";
							throw new ASException(string.Format(ArmSoft.SME.Trade.Common.Resources.WrongComplectPart, text3, _complect.Code));
						}
					}
					MaterialCacheRow item = TradeSettings.TradeCacheService.Material.GetItem(text3, throwExceptionNotExits: true);
					mtComplectPartsGridRow.ItemID = item.Id;
					mtComplectPartsGridRow.ItemName = item.Caption;
					mtComplectPartsGridRow.UnitCode = item.Unit;
					mtComplectPartsGridRow.UnitBrief = item.UnitRow.Brief;
					_unitCodeCol.DataView = new MTQntUnits(item.MTCode);
					mtComplectPartsGridRow.Summa = 0m;
					mtComplectPartsGridRow.IsPartySelected = ((mtComplectPartsGridRow.ItemType == _goodsTypes_Goods) ? new bool?(false) : ((bool?)null));
				}
			}
			catch (Exception ex)
			{
				mtComplectPartsGridRow.ItemName = "";
				mtComplectPartsGridRow.ItemID = -1;
				mtComplectPartsGridRow.UnitCode = "";
				mtComplectPartsGridRow.UnitBrief = "";
				mtComplectPartsGridRow.Quantity = 0m;
				mtComplectPartsGridRow.Summa = 0m;
				mtComplectPartsGridRow.IsPartySelected = false;
				Settings.ErrorHandler(ex);
			}
			_mtComplectPartsGrid.Grid.RefreshRow(e.RowHandle);
			return;
		}
		if (e.Column == _unitCodeCol)
		{
			try
			{
				if (!string.IsNullOrWhiteSpace(text2))
				{
					QntUnitCacheRow item2 = TradeSettings.TradeCacheService.QntUnit.GetItem(text2, throwExceptionNotExits: true);
					mtComplectPartsGridRow.UnitBrief = ((item2 == null) ? "" : item2.Brief);
				}
				return;
			}
			catch (Exception ex2)
			{
				Settings.ErrorHandler(ex2);
				return;
			}
		}
		if (e.Column != _quantityCol)
		{
			return;
		}
		if (Convert.ToDecimal(e.OldValue) != Convert.ToDecimal(e.Value) && Convert.ToDecimal(e.OldValue) != 0m)
		{
			if (mtComplectPartsGridRow.IsPartySelected.HasValue && mtComplectPartsGridRow.IsPartySelected.Value)
			{
				ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Trade.Common.Resources.ForRowSelectedParties, Settings.ProgramName, MessageBoxButton.OK, MessageBoxImage.Asterisk);
			}
			if (mtComplectPartsGridRow.ItemType == _goodsTypes_Goods && PO.MTExpDocSelectedPartiesList != null)
			{
				PO.MTExpDocSelectedPartiesList.RemoveAll((PartyRow x) => x.GridRow == mtComplectPartsGridRow.RowID);
				mtComplectPartsGridRow.IsPartySelected = false;
				mtComplectPartsGridRow.Summa = 0m;
			}
		}
		_mtComplectPartsGrid.Grid.RefreshRow(e.RowHandle);
	}

	private void MTComplectTableView_ShowingEditor(object sender, ShowingEditorEventArgs e)
	{
		if (e.Row == null)
		{
			return;
		}
		if (e.Column == _partyMethodCol)
		{
			if (((MTComplectationSpecificationRow)e.Row).ItemType == ItemTypesSE.Services)
			{
				e.Cancel = true;
			}
		}
		else if (e.Column == _itemNameCol)
		{
			e.Cancel = true;
		}
		else if (e.Column == _summaCol && ((MTComplectationSpecificationRow)e.Row).ItemType == _goodsTypes_Goods)
		{
			e.Cancel = true;
		}
	}

	private bool ExistMaterialInComplectsParts(List<MaterialCacheRow.ComplectPartRow> complectPartsList)
	{
		return complectPartsList.Any((MaterialCacheRow.ComplectPartRow x) => x.ItemType == _goodsTypes_Goods);
	}

	private bool ExistMaterialInComplectsParts()
	{
		return _mtComplectPartsList.Any((MTComplectationSpecificationRow x) => x.ItemType == _goodsTypes_Goods);
	}

	protected override void OnF8()
	{
		On_CodeEnterClick();
	}

	private void CodeEnter_Click(object sender, RoutedEventArgs e)
	{
		On_CodeEnterClick();
	}

	private void On_CodeEnterClick()
	{
		if (_codeEnter == null || !_codeEnter.IsEnabled)
		{
			return;
		}
		_barCode.DoValidate();
		try
		{
			string text = _barCode.Text;
			if (string.IsNullOrWhiteSpace(text))
			{
				if (!_date.DoValidate() || !_date.EditValue.HasValue)
				{
					ControlsHelper.SetFocus(_date);
					throw new ASException(ArmSoft.SME.Common.Resources.ValidateEmptyDate);
				}
				GoodsFilter goodsFilter = new GoodsFilter(shortMode: true, _goodsTypes_Goods);
				ModalBrowser modalBrowser = new ModalBrowser(goodsFilter);
				modalBrowser.Owner = this;
				modalBrowser.WindowStartupLocation = WindowStartupLocation.CenterOwner;
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
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	private void MTComplectTableView_FocusedRowHandleChanged(object sender, FocusedRowHandleChangedEventArgs e)
	{
		int focusedRowHandle = _mtComplectTableView.FocusedRowHandle;
		MTComplectationSpecificationRow mTComplectationSpecificationRow = (MTComplectationSpecificationRow)_mtComplectPartsGrid.Grid.GetRow(focusedRowHandle);
		if (mTComplectationSpecificationRow != null)
		{
			ChangeItemCodeColumnAdditonalView(mTComplectationSpecificationRow.ItemType, mTComplectationSpecificationRow.ItemCode);
		}
	}

	private void FilteredSearch_Click(object sender, RoutedEventArgs e)
	{
		try
		{
			ValidateEmptyFieldsBeforeAddition();
			FilteredSearchWindow filteredSearchWindow = new FilteredSearchWindow(new FilteredSearch
			{
				FilterDataView = new GoodsGroups(showAsTree: true, filteredSearch: true),
				SearchDataView = new GoodsFilter(),
				ResultsList = new MTComplectationFilteredSearchList(PO)
			}, this, isBasketMode: true);
			if (!filteredSearchWindow.ShowDialog().Value)
			{
				return;
			}
			foreach (DocumentGridRow item in filteredSearchWindow.BasketResult)
			{
				bool flag = false;
				MTComplectationSpecificationRow mTComplectationSpecificationRow = (MTComplectationSpecificationRow)item;
				if (mTComplectationSpecificationRow.ItemCode == _complect.Code)
				{
					ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Common.Resources.InvalidComplectMt, Settings.ProgramName, MessageBoxButton.OK, MessageBoxImage.Hand);
					continue;
				}
				foreach (MTComplectationSpecificationRow mtComplectParts in _mtComplectPartsList)
				{
					if (mTComplectationSpecificationRow != mtComplectParts && mTComplectationSpecificationRow.ItemID == mtComplectParts.ItemID)
					{
						mtComplectParts.Quantity += mTComplectationSpecificationRow.Quantity;
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					mTComplectationSpecificationRow.RowID = _maxRowID;
					_mtComplectPartsList.Add(mTComplectationSpecificationRow);
					_maxRowID++;
				}
			}
			_mtComplectPartsGrid.Grid.RefreshData();
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	private void MTComplectPartsGrid_DeletedRow(object sender, EventArgs e)
	{
		_mtComplectPartsGrid.Grid.RefreshData();
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

	private void MTComplectPartsGrid_DeletingRow(object sender, DocumentGridDeletingRowEventArgs e)
	{
		MTComplectationSpecificationRow mtComplectPartsGridRow = (MTComplectationSpecificationRow)_mtComplectPartsGrid.Grid.GetRow(e.RowHandle);
		if (mtComplectPartsGridRow != null)
		{
			PO.MTExpDocSelectedPartiesList.RemoveAll((PartyRow x) => x.GridRow == mtComplectPartsGridRow.RowID);
		}
	}

	protected override bool AllowChangeGridMode()
	{
		return true;
	}

	private void MTComplectPartsGrid_AddingRowWithDialog(object sender, DocumentGridAddingRowEventArgs e)
	{
		ValidateDocFields();
		GoodsSelectPartyWindow goodsSelectPartyWindow = new GoodsSelectPartyWindow(DialogWindowMode.NewModeWithNext, this);
		goodsSelectPartyWindow.ShowDialog();
		int addedRows = goodsSelectPartyWindow.AddedRows;
		if (addedRows != 0)
		{
			_maxRowID += addedRows;
			_mtComplectPartsGrid.Grid.RefreshData();
		}
	}

	private void MTComplectPartsGrid_DuplicatingRowWithDialog(object sender, DocumentGridDuplicatingRowEventArgs e)
	{
		MTComplectationSpecificationRow mTComplectationSpecificationRow = (MTComplectationSpecificationRow)_mtComplectPartsGrid.Grid.GetRow(e.RowHandle);
		if (mTComplectationSpecificationRow != null)
		{
			ValidateDocFields();
			GoodsSelectPartyWindow goodsSelectPartyWindow = new GoodsSelectPartyWindow(DialogWindowMode.NewModeWithNext, this, mTComplectationSpecificationRow);
			goodsSelectPartyWindow.ShowDialog();
			int addedRows = goodsSelectPartyWindow.AddedRows;
			if (addedRows != 0)
			{
				_maxRowID += addedRows;
				_mtComplectPartsGrid.Grid.RefreshData();
			}
		}
	}

	private void MTComplectPartsGrid_EditingRowWithDialog(object sender, DocumentGridEditingRowEventArgs e)
	{
		MTComplectationSpecificationRow mTComplectationSpecificationRow = (MTComplectationSpecificationRow)_mtComplectPartsGrid.Grid.GetRow(e.RowHandle);
		if (mTComplectationSpecificationRow != null)
		{
			ValidateDocFields();
			GoodsSelectPartyWindow goodsSelectPartyWindow = new GoodsSelectPartyWindow(e.AllowEdit ? DialogWindowMode.EditMode : DialogWindowMode.ViewMode, this, mTComplectationSpecificationRow);
			e.RowEdited = Convert.ToBoolean(goodsSelectPartyWindow.ShowDialog());
			if (e.AllowEdit && e.RowEdited)
			{
				_mtComplectPartsGrid.Grid.RefreshData();
			}
		}
	}

	private void ValidateDocFields()
	{
		if (!_date.DoValidate())
		{
			ControlsHelper.SetFocus(_date);
			throw new ASException(ArmSoft.SME.Common.Resources.ValidateEmptyDate);
		}
		if (string.IsNullOrWhiteSpace(_complect.Code))
		{
			ControlsHelper.SetFocus(_complect);
			throw new ASException(ArmSoft.SME.Common.Resources.EmptyComplect);
		}
		if (!_complect.DoValidate())
		{
			ControlsHelper.SetFocus(_complect);
			throw new ASException(string.Format(ArmSoft.SME.Trade.Common.Resources.MTEgineInavidMaterial, _complect.Code));
		}
		if (string.IsNullOrWhiteSpace(_quantityUnit.Code))
		{
			ControlsHelper.SetFocus(_quantityUnit);
			throw new ASException(string.Format(ArmSoft.SME.Common.Resources.FieldIsEmpty, ArmSoft.SME.Common.Resources.Unit));
		}
		if (!_quantityUnit.DoValidate())
		{
			ControlsHelper.SetFocus(_quantityUnit);
			throw new ASException(ArmSoft.SME.Trade.Common.Resources.InvalidQntUnit);
		}
		if (!_quantity.DoValidate())
		{
			ControlsHelper.SetFocus(_quantity);
			throw new ASException(ArmSoft.SME.Trade.Common.Resources.ErrNegOrZeroQuantity);
		}
	}

	private void MTComplectPartsGrid_MovingRowDown(object sender, DocumentGridMovingRowUpOrDownEventArgs e)
	{
		if (_mtComplectPartsGrid.Grid.SortInfo.Count > 0 || _mtComplectTableView.Grid.VisibleRowCount != _mtComplectPartsList.Count)
		{
			throw new ASException(ArmSoft.SME.Common.Resources.NotAllowRowMoveInSortedGrid);
		}
		MTComplectationSpecificationRow mTComplectationSpecificationRow = (MTComplectationSpecificationRow)_mtComplectPartsGrid.Grid.GetRow(e.RowHandle);
		if (mTComplectationSpecificationRow != null)
		{
			_mtComplectPartsList.RemoveAt(e.RowHandle);
			_mtComplectPartsList.Insert(e.RowHandle + 1, mTComplectationSpecificationRow);
		}
		_mtComplectPartsGrid.Grid.RefreshData();
	}

	private void MTComplectPartsGrid_MovingRowUp(object sender, DocumentGridMovingRowUpOrDownEventArgs e)
	{
		if (_mtComplectPartsGrid.Grid.SortInfo.Count > 0 || _mtComplectTableView.Grid.VisibleRowCount != _mtComplectPartsList.Count)
		{
			throw new ASException(ArmSoft.SME.Common.Resources.NotAllowRowMoveInSortedGrid);
		}
		MTComplectationSpecificationRow mTComplectationSpecificationRow = (MTComplectationSpecificationRow)_mtComplectPartsGrid.Grid.GetRow(e.RowHandle);
		if (mTComplectationSpecificationRow != null)
		{
			_mtComplectPartsList.RemoveAt(e.RowHandle);
			_mtComplectPartsList.Insert(e.RowHandle - 1, mTComplectationSpecificationRow);
		}
		_mtComplectPartsGrid.Grid.RefreshData();
	}

	private void MTComplectPartsGrid_DuplicatingRow(object sender, DocumentGridDuplicatingRowEventArgs e)
	{
		MTComplectationSpecificationRow mTComplectationSpecificationRow = (MTComplectationSpecificationRow)_mtComplectPartsGrid.Grid.GetRow(e.RowHandle);
		if (mTComplectationSpecificationRow != null)
		{
			throw new ASException(string.Format(ArmSoft.SME.Trade.Common.Resources.WrongComplectPart, mTComplectationSpecificationRow.ItemCode, _complect.Code));
		}
	}

	private void MTComplectPartsGrid_AddingRow(object sender, DocumentGridAddingRowEventArgs e)
	{
		ValidateEmptyFieldsBeforeAddition();
		_mtComplectPartsList.Add(new MTComplectationSpecificationRow
		{
			ItemType = _goodsTypes_Goods,
			IsPartySelected = false,
			RowID = _maxRowID
		});
		_maxRowID++;
		e.RowNumber = _mtComplectPartsList.Count - 1;
	}

	private void ValidateEmptyFieldsBeforeAddition()
	{
		if (!_date.DoValidate())
		{
			ControlsHelper.SetFocus(_date);
			throw new ASException(ArmSoft.SME.Common.Resources.ValidateEmptyDate);
		}
		if (string.IsNullOrWhiteSpace(_complect.Code))
		{
			ControlsHelper.SetFocus(_complect);
			throw new ASException(ArmSoft.SME.Common.Resources.EmptyComplect);
		}
	}

	private void Complect_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		if (!(e.OldValue.ToString() != e.NewValue.ToString()))
		{
			return;
		}
		PO.MTComplectCode = _complect.Code;
		try
		{
			if (!string.IsNullOrWhiteSpace(_complect.Code))
			{
				MaterialCacheRow item = TradeSettings.TradeCacheService.Material.GetItem(_complect.Code, throwExceptionNotExits: true);
				_quantityUnit.DataView = new MTQntUnits(item.MTCode);
				_quantityUnit.Code = item.Unit;
				_country.Code = item.Country;
				_producer.Code = item.Producer;
				_property2.Code = string.Empty;
				_property1.Code = string.Empty;
				if (!string.IsNullOrWhiteSpace(item.Property1))
				{
					_property1.IsReadOnly = false;
					_property1.DataView = new PropertyList(item.Property1, isFromCode: true);
					if (!string.IsNullOrWhiteSpace(item.Property2))
					{
						_property2.IsReadOnly = false;
						_property2.DataView = new PropertyList(item.Property2, isFromCode: true);
					}
					else
					{
						_property2.IsReadOnly = true;
					}
				}
				else
				{
					_property1.IsReadOnly = true;
					_property2.IsReadOnly = true;
				}
			}
			else
			{
				_country.Code = string.Empty;
				_producer.Code = string.Empty;
				_property1.Code = string.Empty;
				_property1.IsReadOnly = true;
				_property2.Code = string.Empty;
				_property2.IsReadOnly = true;
			}
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	private void BarCode_KeyDown(object sender, KeyEventArgs e)
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
			if (materialCacheRow == null || materialCacheRow.Type == GoodsTypesSE.Service.Name)
			{
				ControlsHelper.SetFocus(_barCode);
				throw new ASException(string.Format(ArmSoft.SME.Common.Resources.ErrBarCodeNotExist, _barCode.Text.Trim()));
			}
			AddMTToList(materialCacheRow, unitCode, unitBrief, qty);
			_barCode.Text = "";
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	private void AddMTToList(MaterialCacheRow goodsRow, string unitCode = "", string unitBrief = "", decimal quantity = 1m)
	{
		if (_rowAddMode == "3")
		{
			AddNewRowInner(goodsRow, string.IsNullOrWhiteSpace(unitCode) ? goodsRow.Unit : unitCode, string.IsNullOrWhiteSpace(unitBrief) ? goodsRow.UnitRow.Brief : unitBrief, quantity);
		}
		else
		{
			AddNewRowOrMergeInner(goodsRow, _rowAddMode == "2", string.IsNullOrWhiteSpace(unitCode) ? goodsRow.Unit : unitCode, string.IsNullOrWhiteSpace(unitBrief) ? goodsRow.UnitRow.Brief : unitBrief, quantity);
		}
	}

	private void AddNewRowInner(MaterialCacheRow goodsRow, string unitCode, string unitBrief, decimal quantity = 1m)
	{
		MTComplectationSpecificationRow mTComplectationSpecificationRow = new MTComplectationSpecificationRow();
		FillMTInvGridRow(mTComplectationSpecificationRow, goodsRow, unitCode, unitBrief, quantity);
		mTComplectationSpecificationRow.RowID = _maxRowID;
		_mtComplectPartsList.Add(mTComplectationSpecificationRow);
		_maxRowID++;
		int rowHandleByListIndex = _mtComplectPartsGrid.Grid.GetRowHandleByListIndex(_mtComplectPartsList.Count - 1);
		_mtComplectPartsGrid.Grid.RefreshRow(rowHandleByListIndex);
		POWindow<MTComplectation>.SelectCellInGrid(_mtComplectTableView, _mtComplectPartsGrid, rowHandleByListIndex, "Quantity");
		_mtComplectPartsGrid.ConfigureButtons();
	}

	private void AddNewRowOrMergeInner(MaterialCacheRow goodsRow, bool forFocusedRow, string unitCode, string unitBrief, decimal quantity = 1m)
	{
		MTComplectationSpecificationRow mTComplectationSpecificationRow = null;
		if (forFocusedRow && _mtComplectPartsList.Count != 0)
		{
			mTComplectationSpecificationRow = (MTComplectationSpecificationRow)_mtComplectTableView.Grid.CurrentItem;
			if (mTComplectationSpecificationRow == null)
			{
				return;
			}
		}
		for (int i = 0; i <= _mtComplectPartsList.Count - 1; i++)
		{
			if ((!forFocusedRow || mTComplectationSpecificationRow.RowID == _mtComplectPartsList[i].RowID) && goodsRow.Id == _mtComplectPartsList[i].ItemID && unitCode == _mtComplectPartsList[i].UnitCode && _mtComplectPartsList[i].IsPartySelected.HasValue && !_mtComplectPartsList[i].IsPartySelected.Value)
			{
				FillMTInvGridRow(_mtComplectPartsList[i], goodsRow, unitCode, unitBrief, quantity);
				int rowHandleByListIndex = _mtComplectPartsGrid.Grid.GetRowHandleByListIndex(i);
				_mtComplectPartsGrid.Grid.RefreshRow(rowHandleByListIndex);
				POWindow<MTComplectation>.SelectCellInGrid(_mtComplectTableView, _mtComplectPartsGrid, rowHandleByListIndex, "Quantity");
				return;
			}
		}
		AddNewRowInner(goodsRow, unitCode, unitBrief, quantity);
	}

	private void FillMTInvGridRow(MTComplectationSpecificationRow mtComplectPartGridRow, MaterialCacheRow goodsRow, string unitCode, string unitBrief, decimal quantity = 1m)
	{
		if (goodsRow != null)
		{
			mtComplectPartGridRow.ItemType = goodsRow.Type;
			mtComplectPartGridRow.ItemID = goodsRow.Id;
			mtComplectPartGridRow.ItemCode = goodsRow.MTCode;
			mtComplectPartGridRow.ItemName = goodsRow.Caption;
		}
		mtComplectPartGridRow.UnitCode = unitCode;
		mtComplectPartGridRow.UnitBrief = unitBrief;
		mtComplectPartGridRow.Quantity += quantity;
		mtComplectPartGridRow.Summa = 0m;
		mtComplectPartGridRow.IsPartySelected = false;
	}

	protected override void LoadDataToControls()
	{
		base.LoadDataToControls();
		_maxRowID = PO.MaxRowID.Value;
		_mtComplectPartsList.Clear();
		foreach (MTComplectation.MTComplectationSpecificationRow complectParts in PO.ComplectPartsList)
		{
			_mtComplectPartsList.Add(new MTComplectationSpecificationRow
			{
				ItemID = complectParts.ItemID.Value,
				ItemName = complectParts.ItemName,
				ItemCode = complectParts.ItemCode,
				RowID = complectParts.RowID.Value,
				RowNum = complectParts.RowNum,
				Quantity = complectParts.Quantity.Value,
				UnitBrief = complectParts.UnitBrief,
				UnitCode = complectParts.UnitCode,
				Summa = complectParts.Summa.Value,
				IsPartySelected = complectParts.IsPartySelected,
				ItemType = complectParts.ItemType
			});
		}
		_mtComplectPartsGrid.ItemsSource = _mtComplectPartsList;
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
		PO.MaxRowID = _maxRowID;
		PO.ComplectPartsList.Clear();
		foreach (MTComplectationSpecificationRow mtComplectParts in _mtComplectPartsList)
		{
			PO.ComplectPartsList.Add(new MTComplectation.MTComplectationSpecificationRow
			{
				ItemID = mtComplectParts.ItemID,
				ItemName = mtComplectParts.ItemName,
				ItemCode = mtComplectParts.ItemCode,
				RowID = mtComplectParts.RowID,
				RowNum = mtComplectParts.RowNum,
				Quantity = mtComplectParts.Quantity,
				UnitBrief = mtComplectParts.UnitBrief,
				UnitCode = mtComplectParts.UnitCode,
				Summa = mtComplectParts.Summa,
				IsPartySelected = mtComplectParts.IsPartySelected,
				ItemType = mtComplectParts.ItemType
			});
		}
	}

	protected override void OnNew()
	{
		base.OnNew();
		_date.EditValue = Settings.HostingEnvironment.CurrentDate;
		_owner.Code = Settings.LoginService.UserName;
		if (_viewMode == WindowViewMode.Copy)
		{
			return;
		}
		string text = Settings.ParametersService.DefaultStorage();
		if (!string.IsNullOrWhiteSpace(text))
		{
			StoragesCacheRow item = TradeSettings.TradeCacheService.Storage.GetItem(text);
			if ((item == null || !item.Retail) && TradeSettings.CreateTradeClient().CommonServices.IsStorageAvailable(text))
			{
				_storageExpense.Code = text;
			}
		}
	}

	protected override void OnNewAfterSave()
	{
		base.OnNewAfterSave();
		SetNextDocumentNumber();
		_mtComplectPartsList.Clear();
		_maxRowID = 0;
		_owner.Code = Settings.LoginService.UserName;
		_barButtonJumpTo.IsEnabled = false;
		_barButtonGoodsOperationsJournal.IsEnabled = false;
	}

	protected override void OnEdit()
	{
		base.OnEdit();
		LoadDataToControls();
		_setFocusOnActivation = _docNum;
		_storageExpense.EditValueChanged += new SafeDocumentEventHandler<EditValueChangedEventArgs>(StorageExpense_EditValueChanged, this).Handler;
	}

	protected override void SetControlsReadonly()
	{
		base.SetControlsReadonly();
		if (_viewMode == WindowViewMode.View)
		{
			return;
		}
		_property1.IsReadOnly = Settings.ParametersService.ShowPartyProperty1();
		_property2.IsReadOnly = Settings.ParametersService.ShowPartyProperty2();
		bool flag = State == DocumentStateSE.Draft.Value || State == DocumentStateSE.Deleted.Value;
		bool flag2 = _viewMode == WindowViewMode.Edit && !flag;
		_date.IsReadOnly = flag2;
		_barCode.IsReadOnly = flag2;
		_codeEnter.IsEnabled = !flag2;
		_complect.IsReadOnly = flag2;
		_storageIncome.IsReadOnly = flag2;
		_storageExpense.IsReadOnly = flag2;
		_quantity.IsReadOnly = flag2;
		_quantityUnit.IsReadOnly = flag2;
		_country.IsReadOnly = flag2;
		_producer.IsReadOnly = flag2;
		MaterialCacheRow item = TradeSettings.TradeCacheService.Material.GetItem(_complect.Code);
		if (item != null && !string.IsNullOrWhiteSpace(item.Property1))
		{
			_property1.IsReadOnly = flag2;
			if (!string.IsNullOrWhiteSpace(item.Property2))
			{
				_property2.IsReadOnly = flag2;
			}
		}
		foreach (GridColumn column in _mtComplectPartsGrid.Grid.Columns)
		{
			column.ReadOnly = flag2;
		}
		_mtComplectPartsGrid.AllowAdd = !flag2;
		_mtComplectPartsGrid.AllowEdit = !flag2;
		_mtComplectPartsGrid.AllowDelete = !flag2;
		_mtComplectPartsGrid.AllowDuplicate = !flag2;
		_mtComplectPartsGrid.AllowMoveUp = !flag2;
		_mtComplectPartsGrid.AllowMoveDown = !flag2;
		_loadComplectPartsList.IsVisible = !flag2;
		_filteredSearch.IsVisible = !flag2;
		_partyMethodCol.ReadOnly = false;
		((ButtonEditSettings)_partyMethodCol.EditSettings).IsTextEditable = false;
	}

	protected override void StoreSettings(RegistryKey rk)
	{
		base.StoreSettings(rk);
		_mtComplectPartsGrid.StoreSettings(rk);
	}

	protected override void RestoreSettings(double defaultWidth, double defaultHeight, RegistryKey rk)
	{
		base.RestoreSettings(defaultWidth, defaultHeight, rk);
		_mtComplectPartsGrid.RestoreSettings(rk);
	}

	private void StorageExpense_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		if (!(e.NewValue.ToString() != e.OldValue.ToString()))
		{
			return;
		}
		if (_storageExpense.DoValidate() && !TradeSettings.CreateTradeClient().CommonServices.IsStorageAvailable(_storageExpense.Code))
		{
			ArmSoft.SME.Core.UI.MessageBox.Show(string.Format(ArmSoft.SME.Trade.Common.Resources.StorageNotAvailForUser, Settings.LoginService.UserName, _storageExpense.Code), Settings.ProgramName, MessageBoxButton.OK, MessageBoxImage.Exclamation);
			_storageExpense.Code = "";
		}
		if (_mtComplectPartsList.Count <= 0 || !ExistMaterialInComplectsParts())
		{
			return;
		}
		foreach (MTComplectationSpecificationRow mtComplectParts in _mtComplectPartsList)
		{
			if (mtComplectParts.PartyMethodName != ArmSoft.SME.Common.Resources.AutoSelection)
			{
				ArmSoft.SME.Core.UI.MessageBox.Show(string.Format(ArmSoft.SME.Trade.Common.Resources.PartiesValuesChanged, ArmSoft.SME.Trade.Common.Resources.OutStorage), Caption, MessageBoxButton.OK, MessageBoxImage.Asterisk);
				break;
			}
		}
		for (int i = 0; i <= _mtComplectPartsList.Count - 1; i++)
		{
			if (_mtComplectPartsList[i].ItemType == _goodsTypes_Goods)
			{
				_mtComplectPartsList[i].IsPartySelected = false;
				_mtComplectPartsList[i].Summa = 0m;
				int rowHandleByListIndex = _mtComplectPartsGrid.Grid.GetRowHandleByListIndex(i);
				_mtComplectPartsGrid.Grid.RefreshRow(rowHandleByListIndex);
			}
		}
		PO.MTExpDocSelectedPartiesList.Clear();
	}

	private void ChangeItemCodeColumnAdditonalView(string typeValue, string itemCode)
	{
		ItemTypesSE itemTypesSE = ItemTypesSE.All;
		if (typeValue == _goodsTypes_Goods)
		{
			itemTypesSE = ItemTypesSE.Goods;
		}
		else if (typeValue == ItemTypesSE.Services)
		{
			itemTypesSE = ItemTypesSE.Services;
		}
		_itemCodeCol.DataView = new GoodsFilter(shortMode: true, itemTypesSE.Value, "", isShowOnlyInPriceList: true);
		_unitCodeCol.DataView = new MTQntUnits((!string.IsNullOrWhiteSpace(itemCode)) ? itemCode : "", isShowQntUnitCaption: false, isShowOnlyComplects: false, "", itemTypesSE.Value);
	}
}
