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
using System.Windows.Threading;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Constants;
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
using ArmSoft.SME.Trade.Client.Data.DropDowns;
using ArmSoft.SME.Trade.Client.Routes;
using ArmSoft.SME.Trade.Common;
using ArmSoft.SME.Trade.Common.Documents.Definition;
using ArmSoft.SME.Trade.Common.Models.Caches;
using ArmSoft.SME.Trade.Common.Models.Documents;
using ArmSoft.SME.Trade.Common.Parameters;
using ArmSoft.SME.Trade.Common.SmartEnums;
using ArmSoft.SME.Trade.UI.CustomUIElements;
using ArmSoft.SME.Trade.UI.CustomUIElements.UserControls;
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

[DocumentWindow(9)]
public class MTExpenseWindow : TradeDocumentWindow<MTExpense>
{
	public class MTExpenseSpecificationRow : DocumentGridRow
	{
		[ObservableProperty]
		private int mTID = -1;

		[ObservableProperty]
		private string mTCode = string.Empty;

		[ObservableProperty]
		private string mTName;

		[ObservableProperty]
		private short rowNum;

		[ObservableProperty]
		private int rowID;

		[ObservableProperty]
		private string unitCode = string.Empty;

		[ObservableProperty]
		private decimal quantity;

		[ObservableProperty]
		private decimal salePrice;

		[ObservableProperty]
		private decimal summa;

		[ObservableProperty]
		private bool isPartySelected;

		[ObservableProperty]
		private string unitBrief;

		public bool UpdateRemainsOnInfoPanel = true;

		public string PartyMethodName
		{
			get
			{
				if (!IsPartySelected)
				{
					return ArmSoft.SME.Common.Resources.AutoSelection;
				}
				return ArmSoft.SME.Common.Resources.PartySelected;
			}
		}

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
		public decimal SalePrice
		{
			get
			{
				return salePrice;
			}
			set
			{
				if (!EqualityComparer<decimal>.Default.Equals(salePrice, value))
				{
					OnPropertyChanging(__KnownINotifyPropertyChangingArgs.SalePrice);
					salePrice = value;
					OnPropertyChanged(__KnownINotifyPropertyChangedArgs.SalePrice);
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

		public MTExpenseSpecificationRow()
		{
		}

		public MTExpenseSpecificationRow(MTExpenseWindow docWindow)
			: base(docWindow)
		{
		}

		static MTExpenseSpecificationRow()
		{
			DocumentGridRow.RegisterPropertyValidators(typeof(MTExpenseSpecificationRow), cacheValidationResults: true, new Dictionary<string, Action<DocumentGridRow, DevExpress.XtraEditors.DXErrorProvider.ErrorInfo>>
			{
				{
					"MTCode",
					(DocumentGridRow row, DevExpress.XtraEditors.DXErrorProvider.ErrorInfo info) =>
					{
						ValidateGoodCode((MTExpenseSpecificationRow)row, info);
					}
				},
				{
					"UnitCode",
					(DocumentGridRow row, DevExpress.XtraEditors.DXErrorProvider.ErrorInfo info) =>
					{
						ValidateUnitCode((MTExpenseSpecificationRow)row, info);
					}
				},
				{
					"Quantity",
					(DocumentGridRow row, DevExpress.XtraEditors.DXErrorProvider.ErrorInfo info) =>
					{
						ValidateQuantity((MTExpenseSpecificationRow)row, info);
					}
				}
			});
		}

		private static void ValidateGoodCode(MTExpenseSpecificationRow row, DevExpress.XtraEditors.DXErrorProvider.ErrorInfo info)
		{
			TradePropertyValidator.ValidateMaterialCode(row.MTCode, info, TradeAllDocDefinitions.MTExpense.Grids["MTList"].Columns["MTCode"]);
		}

		private static void ValidateUnitCode(MTExpenseSpecificationRow row, DevExpress.XtraEditors.DXErrorProvider.ErrorInfo info)
		{
			TradePropertyValidator.ValidateGoodUnitCode(row.UnitCode, row.MTCode, info, TradeAllDocDefinitions.MTExpense.Grids["MTList"].Columns["UnitCode"]);
		}

		private static void ValidateQuantity(MTExpenseSpecificationRow row, DevExpress.XtraEditors.DXErrorProvider.ErrorInfo info)
		{
			PropertyValidator.PositiveValidate(row.Quantity, info, ArmSoft.SME.Common.Resources.Quantity);
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		private void OnMTCodeChanged(string value)
		{
			ResetForceValidationBit("MTCode", "MTName", "UnitCode");
			MaterialCacheRow item = TradeSettings.TradeCacheService.Material.GetItem(MTCode);
			MTName = ((item == null) ? string.Empty : item.Caption);
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		private void OnUnitCodeChanged(string value)
		{
			ResetForceValidationBit("UnitCode");
			QntUnitCacheRow item = TradeSettings.TradeCacheService.QntUnit.GetItem(UnitCode);
			UnitBrief = ((item == null) ? string.Empty : item.Brief);
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		private void OnQuantityChanged(decimal value)
		{
			ResetForceValidationBit("Quantity");
		}
	}

	public class MTExpenseFilteredSearchList : IResultList
	{
		private readonly ObservableCollection<MTExpenseSpecificationRow> _itemSource = new ObservableCollection<MTExpenseSpecificationRow>();

		public IEnumerable<DocumentGridRow> ItemsSource => _itemSource;

		public List<FilteredSearchGridColumn> Columns => new List<FilteredSearchGridColumn>
		{
			new FilteredSearchGridColumn
			{
				Header = TradeAllDocDefinitions.MTExpense.Grids["MTList"].Columns["MTCode"].Caption,
				FieldName = "MTCode",
				CustomWidth = 125.0
			},
			new FilteredSearchGridColumn
			{
				Header = TradeAllDocDefinitions.MTExpense.Grids["MTList"].Columns["MTName"].Caption,
				FieldName = "MTName",
				CustomWidth = 300.0
			},
			new FilteredSearchGridColumn
			{
				Header = TradeAllDocDefinitions.MTExpense.Grids["MTList"].Columns["Quantity"].Caption,
				FieldName = "Quantity",
				AllowEditing = true,
				NumericFormat = DecimalExtension.GetCurrencyFormat(13, 3)
			}
		};

		public int AddToItemsSource(object key, params object[] args)
		{
			string MTCode = (string)key;
			decimal num = (decimal)args[0];
			int num2 = _itemSource.FindIndex((MTExpenseSpecificationRow x) => x.MTCode == MTCode);
			if (num2 == -1)
			{
				MaterialCacheRow item = TradeSettings.TradeCacheService.Material.GetItem(MTCode);
				if (item == null)
				{
					return -1;
				}
				MTExpenseSpecificationRow item2 = new MTExpenseSpecificationRow
				{
					MTID = item.Id,
					MTCode = item.MTCode,
					UnitCode = item.Unit
				};
				_itemSource.Add(item2);
				num2 = _itemSource.Count - 1;
			}
			_itemSource[num2].Quantity += num;
			return num2;
		}
	}

	protected delegate void RowInfoChangeEventHandler(MTExpenseSpecificationRow Row);

	private delegate void DelegateSetRowInfoToInfoPanel(MTExpenseSpecificationRow Row);

	private delegate void DelegateClearInfoPanel();

	public DropDownControl _storage;

	private TextEditExt _barCode;

	private Button _codeEnter;

	private BarEditItemExt _filteredSearch;

	private ComboBoxEditExt _comment;

	private DropDownControl _docOwner;

	public DocumentGridControl _mtExpenseGrid;

	private TableView _mtExpenseTableView;

	public ObservableCollection<MTExpenseSpecificationRow> _mtExpenseList;

	private DocumentGridColumn _codeCol;

	private DocumentGridColumn _nameCol;

	private DocumentGridColumn _unitCodeCol;

	private DocumentGridColumn _unitBriefCol;

	private DocumentGridColumn _quantityCol;

	private DocumentGridColumn _salePriceCol;

	private DocumentGridColumn _summaCol;

	private DocumentGridColumn _partyCol;

	private DXTabControl _tabs;

	private TextEditExt _chiefAccountant;

	private ComboBoxEditExt _demand;

	private bool _isChanged = true;

	private Dictionary<(int, string), decimal> _availabilityByGoodId = new Dictionary<(int, string), decimal>();

	private DropDownCommentControl _baseDocumentControl;

	public int _maxRowID;

	private SafeDocumentEventHandler<FocusedRowHandleChangedEventArgs> _focusedRowHandleChangedEvent;

	private ReportBarButtonItem _barButtonJumpTo;

	private BarButtonItem _barButtonDocumentSettings = new BarButtonItem();

	private BarSubItem _barSubItemJournals;

	private ReportBarButtonItem _barButtonGoodsOperationsJournal;

	private ReportBarButtonItem _barButtonGoodsRetailOperationJournal;

	public bool _distributeByFEFO;

	private bool _storageIsRetail;

	private TradeDocument _baseDocument;

	private Guid _baseISN = Guid.Empty;

	private TextEditExt _infoItemName;

	private InfoDisplayControls _infoDisplayControls;

	private LayoutGroup _infoDisplay;

	protected override double WidthValue => 1090.0;

	protected override double HeightValue => 440.0;

	protected override double MinWidthValue => 700.0;

	protected override double MinHeightValue => 250.0;

	protected override bool AllowCreateChildDoc => false;

	private TradeDocument BaseDocument
	{
		get
		{
			return _baseDocument;
		}
		set
		{
			_baseDocument = value;
			if (_baseDocument == null)
			{
				_baseDocumentControl.Text = "";
			}
			else
			{
				_baseDocumentControl.Text = DocumentHelpers.GetDocumentDescription(_baseDocument, ArmSoft.SME.Common.Constants.Common.DocumentDescriptionMode.TypeDateNum);
			}
		}
	}

	private Guid BaseISN
	{
		get
		{
			return _baseISN;
		}
		set
		{
			if (((IComparable)value).CompareTo((object?)_baseISN) == 0 && PO.State != DocumentStateSE.Draft.Value && PO.State != DocumentStateSE.Deleted.Value)
			{
				return;
			}
			_baseISN = value;
			if (_baseISN == Guid.Empty)
			{
				BaseDocument = null;
				return;
			}
			BaseDocument = (TradeDocument)Settings.DocumentService.Load(_baseISN, withoutAccessCheck: true);
			if (ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Trade.Common.Resources.FillUsingBaseQ, Settings.ProgramName, MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.Yes) == MessageBoxResult.Yes)
			{
				CreateBasedOnDoc();
			}
		}
	}

	protected event RowInfoChangeEventHandler RowInfoChange;

	public MTExpenseWindow()
	{
	}

	public MTExpenseWindow(MTExpense doc)
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
		_docNum.MagicClick += new SafeDocumentEventHandler<MagicClickEventArgs>(DocNum_MagicClick, this).Handler;
		_docNum.EditValueChanged += new SafeDocumentEventHandler<EditValueChangedEventArgs>(DocNum_EditValueChanged, this).Handler;
		_storage = AddDropDownControl(null, Definition.Fields["Storage"], new Storages());
		_storage.EditValueChanged += new SafeDocumentEventHandler<EditValueChangedEventArgs>(Storage_EditValueChanged, this).Handler;
		DocumentsJournal idv = new DocumentsJournal(this, ReportTypesSE.AllDocument, ((TradeDocDefinition)Definition).BaseDocTypes)
		{
			DocType = ((TradeDocDefinition)Definition).BaseDocTypes.First().ToString().PadLeft(2, '0'),
			DocState = DocumentStateSE.DraftOrProcessed.ValueUI
		};
		_baseDocumentControl = AddDropDownCommentControl(null, ArmSoft.SME.Common.Resources.BaseDoc, isRequired: false, idv, 0, HorizontalAlignment.Stretch, "BaseDocumentControl");
		_baseDocumentControl.ShowClearButton = true;
		_baseDocumentControl.ShowSearchButton = true;
		_baseDocumentControl.IsTextEditable = false;
		_baseDocumentControl.BtnClearClicked += new SafeDocumentEventHandler<EditKeyValueChangedEventArgs>(BaseDocument_BtnClearClicked, this).Handler;
		_baseDocumentControl.EditKeyValueChanged += new SafeDocumentEventHandler<EditKeyValueChangedEventArgs>(BaseDocument_EditKeyValueChanged, this).Handler;
		_baseDocumentControl.ClickDropDown += new SafeDocumentEventHandler<DropDownControlEventArgs>(BaseDocument_ClickDropDown, this).Handler;
		_baseDocumentControl.BtnSearchClicked += new SafeDocumentEventHandler<EditKeyValueChangedEventArgs>(BaseDocumentSearch_Click, this).Handler;
		LayoutGroupExt container2 = AddLayoutGroup(null, LayoutGroupView.Group, Orientation.Horizontal);
		_barCode = AddStringControl(container2, ArmSoft.SME.Common.Resources.BarCode + " (F2)", isRequired: false, 20, HorizontalAlignment.Stretch);
		_barCode.NullText = ArmSoft.SME.Trade.Common.Resources.BarCodeAndCodeInputNullText;
		_barCode.KeyDown += new SafeDocumentEventHandler<KeyEventArgs>(BarCode_KeyDown, this).Handler;
		_codeEnter = AddButton(container2, null, ArmSoft.SME.Trade.Common.Resources.cmdCodeEnter + " (F8)", null, ArmSoft.SME.Trade.Common.Resources.cmdCodeEnter + " (F8)", isTabStop: true);
		_codeEnter.Margin = new Thickness(0.0, 0.0, 18.0, 0.0);
		_codeEnter.Click += new SafeDocumentEventHandler<RoutedEventArgs>(CodeEnter_Click, this).Handler;
		_tabs = AddTabControl(null);
		LayoutGroup container3 = AddTabbedGroup(_tabs, ArmSoft.SME.Common.Resources.General, "General");
		_comment = AddComboBoxControl(container3, Definition.Fields["Comment"], TradeSettings.TradeCacheService.StandardComments.GetCaptionsList(), HorizontalAlignment.Stretch);
		_docOwner = AddDropDownControl(container3, Definition.Fields["Owner"], new UsersFilter());
		_docOwner.CodeWidth = 12;
		_docOwner.ShowComment = false;
		_docOwner.InnerCodeEdit.Width = _docOwner.InnerCodeEdit.MinWidth;
		if (!Settings.LoginService.IsAdmin && Settings.ParametersService.DenyAccessToOtherOwnerDocs())
		{
			((LayoutItemWithRequired)(object)_docOwner.Parent).Visibility = Visibility.Collapsed;
		}
		_mtExpenseGrid = AddGrid(container3, Definition.Grids["MTList"], HorizontalAlignment.Stretch, VerticalAlignment.Stretch, isSupportDialogMode: true);
		_mtExpenseGrid.AddRowIndicator(35);
		_filteredSearch = _mtExpenseGrid.AddButton(null, ArmSoft.SME.Trade.Common.Resources.GroupAddition, ArmSoft.SME.Trade.Common.Resources.GroupAddition, 130.0);
		_filteredSearch.Click += new SafeDocumentEventHandler<RoutedEventArgs>(FilteredSearch_Click, this).Handler;
		_codeCol = _mtExpenseGrid.AddDropDownColumn(Definition.Grids["MTList"].Columns["MTCode"], new ArmSoft.SME.Trade.UI.DropDown.GoodsAvailabilityFilter(Settings.HostingEnvironment.CurrentDate, _storage.Code, _storageIsRetail), allowAdditionButtons: true, isOwnProcessingClickDropDown: true);
		_codeCol.Width = 125.0;
		_nameCol = _mtExpenseGrid.AddStringColumn(Definition.Grids["MTList"].Columns["MTName"]);
		_nameCol.AllowEditing = DefaultBoolean.False;
		_nameCol.Width = 225.0;
		_unitCodeCol = _mtExpenseGrid.AddDropDownColumn(Definition.Grids["MTList"].Columns["UnitCode"], new MTQntUnits());
		_unitBriefCol = _mtExpenseGrid.AddStringColumn(Definition.Grids["MTList"].Columns["UnitBrief"]);
		_unitBriefCol.AllowEditing = DefaultBoolean.False;
		_quantityCol = _mtExpenseGrid.AddNumberColumn(Definition.Grids["MTList"].Columns["Quantity"], showTotal: true);
		_salePriceCol = _mtExpenseGrid.AddNumberColumn(Definition.Grids["MTList"].Columns["SalePrice"], showTotal: false, useCurrencyFormat: true, isOwnProcessingClickDropDown: true);
		_summaCol = _mtExpenseGrid.AddNumberColumn(Definition.Grids["MTList"].Columns["Summa"], showTotal: true);
		_summaCol.AllowEditing = DefaultBoolean.False;
		_summaCol.Visible = Settings.ParametersService.ShowGoodsCostInReportsAndDocs();
		_summaCol.ShowInColumnChooser = _summaCol.Visible;
		_partyCol = _mtExpenseGrid.AddDropDownColumn("PartyMethodName", ArmSoft.SME.Common.Resources.Party, 19, trimValue: false, GlyphKind.Regular);
		((ButtonEditSettings)_partyCol.EditSettings).IsTextEditable = false;
		foreach (GridColumn column in _mtExpenseGrid.Grid.Columns)
		{
			column.AllowSorting = DefaultBoolean.True;
			column.AllowColumnFiltering = DefaultBoolean.True;
			column.FilterPopupMode = FilterPopupMode.CheckedList;
		}
		_mtExpenseList = new ObservableCollection<MTExpenseSpecificationRow>();
		_infoDisplayControls = AddInfoDisplayControls(DocumentTypeSE.MTExpense, isShowItemPictureOnInfoPanel: false, isShowStorageAvailabilityOnInfoPanel: true, isShowWholeAvailabilityOnInfoPanel: true, isShowPriceAvailabilityOnInfoPanel: false, showDescriptionOnInfoPanel: false, enableSubstituteItems: false, 22.0, container3);
		_infoDisplay = _infoDisplayControls.InfoDisplay;
		_infoDisplay.Collapsed += new SafeDocumentEventHandler<EventArgs>(InfoDisplay_Collapsed, this).Handler;
		_infoItemName = _infoDisplayControls.InfoItemName;
		LayoutGroup container4 = AddTabbedGroup(_tabs, ArmSoft.SME.Common.Resources.Additional, "Additional");
		AddStringControl(container4, Definition.Fields["Trust"], HorizontalAlignment.Stretch);
		AddDropDownCommentControl(container4, Definition.Fields["Mediator"], new Employees(), HorizontalAlignment.Stretch);
		AddStringControl(container4, Definition.Fields["Machine"], HorizontalAlignment.Stretch);
		_chiefAccountant = AddStringControl(container4, Definition.Fields["Chief"], HorizontalAlignment.Stretch);
		AddDropDownCommentControl(container4, Definition.Fields["Allower"], new Employees(), HorizontalAlignment.Stretch);
		_demand = AddComboBoxControl(container4, Definition.Fields["Demand"], new List<string>(), HorizontalAlignment.Stretch);
		_mtExpenseGrid.AddingRow += new SafeDocumentEventHandler<DocumentGridAddingRowEventArgs>(MTExpenseGrid_AddingRow, this).Handler;
		_mtExpenseGrid.DuplicatingRow += new SafeDocumentEventHandler<DocumentGridDuplicatingRowEventArgs>(MTExpenseGrid_DuplicatingRow, this).Handler;
		_mtExpenseGrid.DeletedRow += new SafeDocumentEventHandler<EventArgs>(MTExpenseGrid_DeletedRow, this).Handler;
		_mtExpenseGrid.DeletingRow += new SafeDocumentEventHandler<DocumentGridDeletingRowEventArgs>(MTExpenseGrid_DeletingRow, this).Handler;
		_mtExpenseGrid.AddingRowWithDialog += new SafeDocumentEventHandler<DocumentGridAddingRowEventArgs>(MTExpenseGrid_AddingRowWithDialog, this).Handler;
		_mtExpenseGrid.DuplicatingRowWithDialog += new SafeDocumentEventHandler<DocumentGridDuplicatingRowEventArgs>(MTExpenseGrid_DuplicatingRowWithDialog, this).Handler;
		_mtExpenseGrid.EditingRowWithDialog += new SafeDocumentEventHandler<DocumentGridEditingRowEventArgs>(MTExpenseGrid_EditingRowWithDialog, this).Handler;
		_mtExpenseGrid.MovingRowUp += new SafeDocumentEventHandler<DocumentGridMovingRowUpOrDownEventArgs>(MTExpenseGrid_MovingRowUp, this).Handler;
		_mtExpenseGrid.MovingRowDown += new SafeDocumentEventHandler<DocumentGridMovingRowUpOrDownEventArgs>(MTExpenseGrid_MovingRowDown, this).Handler;
		_mtExpenseGrid.ClickDropDown += new SafeDocumentEventHandler<DocumentGridClickDropDownEventArgs>(MTExpenseGrid_ClickDropDown, this).Handler;
		_mtExpenseGrid.RefreshButtons += new SafeDocumentEventHandler<EventArgs>(MTExpenseGrid_RefreshButtons, this).Handler;
		_mtExpenseTableView = _mtExpenseGrid.View;
		_mtExpenseTableView.CellValueChanged += new SafeDocumentEventHandler<CellValueChangedEventArgs>(MTExpenseTableView_CellValueChanged, this).Handler;
		_focusedRowHandleChangedEvent = new SafeDocumentEventHandler<FocusedRowHandleChangedEventArgs>(MTExpenseTableView_FocusedRowHandleChanged, this);
		_mtExpenseTableView.FocusedRowHandleChanged += _focusedRowHandleChangedEvent.Handler;
		_mtExpenseGrid.AllowMoveUp = true;
		_mtExpenseGrid.AllowMoveDown = true;
		RowInfoChange += ChangeRowInfo;
	}

	private void InfoDisplay_Collapsed(object sender, EventArgs e)
	{
		_infoDisplayControls.InfoPanelCollapsed = true;
	}

	private void ChangeRowInfo(MTExpenseSpecificationRow row = null)
	{
		if (row == null)
		{
			((DispatcherObject)this).Dispatcher.BeginInvoke((Delegate)new DelegateClearInfoPanel(CleanInfoPanel), Array.Empty<object>());
			return;
		}
		((DispatcherObject)this).Dispatcher.BeginInvoke((Delegate)new DelegateSetRowInfoToInfoPanel(SetRowInfoToPanel), new object[1] { row });
	}

	private void CleanInfoPanel()
	{
		_isChanged = IsChanged;
		_infoItemName.Text = string.Empty;
		_infoDisplayControls.ClearInfoAvailability();
		IsChanged = _isChanged;
	}

	private void SetRowInfoToPanel(MTExpenseSpecificationRow row)
	{
		try
		{
			if (string.IsNullOrWhiteSpace(row.MTCode))
			{
				CleanInfoPanel();
				return;
			}
			_isChanged = IsChanged;
			_infoItemName.Text = $"{row.MTCode} - {row.MTName}";
			SetAvailabilityInfoToPanel(row);
			IsChanged = _isChanged;
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	private void SetAvailabilityInfoToPanel(MTExpenseSpecificationRow row)
	{
		if (string.IsNullOrWhiteSpace(row.UnitCode))
		{
			_infoDisplayControls.SetStringAccBalance("-");
			_infoDisplayControls.SetStringAccUnitBrief("");
			_infoDisplayControls.SetStringWholeBalance("-");
			_infoDisplayControls.SetStringWholeUnitBrief("");
		}
		else
		{
			_infoDisplayControls.SetAccBalance(string.IsNullOrWhiteSpace(_storage.Code) ? 0m : MaterialHelpers.GetGoodRemains(row.UpdateRemainsOnInfoPanel, row.MTID, row.UnitCode, ref _availabilityByGoodId, _storage.Code));
			_infoDisplayControls.SetAccUnitBrief(row.UnitBrief);
			_infoDisplayControls.SetWholeBalance(MaterialHelpers.GetGoodRemains(row.UpdateRemainsOnInfoPanel, row.MTID, row.UnitCode, ref _availabilityByGoodId));
			_infoDisplayControls.SetWholeUnitBrief(row.UnitBrief);
			row.UpdateRemainsOnInfoPanel = false;
		}
	}

	protected override void AddToolbarItems()
	{
		base.AddToolbarItems();
		Ribbon.ToolbarItemLinks.Add(_barButtonJumpTo.ConvertForToolbar("t_doc_JumpTo_16.svg"));
		Ribbon.ToolbarItemLinks.Add(_barSubItemJournals.ConvertForToolbar("t_report_16.svg"));
		Ribbon.ToolbarItemLinks.Add(_barButtonDocumentSettings.ConvertForToolbar("t_page_white_gear_16.svg"));
	}

	protected override void AddItemsToRibbon()
	{
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00be: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00df: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
		base.AddItemsToRibbon();
		CreateRibbonPageGroupActions();
		CreateRibbonPageGroupReports();
		BarSubItem barSubItem = new BarSubItem();
		_barButtonJumpTo = new ReportBarButtonItem(DataProviderTypeSE.AllDocumentsJournal, showDialog: false, ArmSoft.SME.Common.Resources.JumpTo, null, "doc_JumpTo_16.svg", "doc_JumpTo_32.svg");
		_barButtonJumpTo.AddInBarSubItem(ref _ribbonPageGroupActions, ref barSubItem);
		_barSubItemJournals = new BarSubItem
		{
			Content = "",
			Glyph = ImageService.GetImage("report_16.svg"),
			LargeGlyph = ImageService.GetImage("report_32.svg")
		};
		_barButtonDocumentSettings.Content = ArmSoft.SME.Common.Resources.Settings;
		_barButtonDocumentSettings.Glyph = ImageService.GetImage("page_white_gear_16.svg");
		_barButtonDocumentSettings.LargeGlyph = ImageService.GetImage("page_white_gear_32.svg");
		_barButtonDocumentSettings.ItemClick += new SafeDocumentEventHandler<ItemClickEventArgs>(BarButtonDocumentSettings_ItemClick, this).Handler;
		_ribbonPageGroupActions.ItemLinks.Add(_barButtonDocumentSettings.CreateLink());
		_barButtonGoodsOperationsJournal = new ReportBarButtonItem(DataProviderTypeSE.GoodsRepOperations.Value, showDialog: false, ArmSoft.SME.Trade.Common.Resources.MTOperations);
		_barButtonGoodsOperationsJournal.AddInBarSubItem(ref _ribbonPageGroupReports, ref _barSubItemJournals);
		_barButtonGoodsRetailOperationJournal = new ReportBarButtonItem(DataProviderTypeSE.MatRepOperationsRetail.Value, showDialog: false, ArmSoft.SME.Trade.Common.Resources.MTOperationsRetail);
		_barButtonGoodsRetailOperationJournal.AddInBarSubItem(ref _ribbonPageGroupReports, ref _barSubItemJournals);
	}

	protected override void SetReportBarButtonsProperties()
	{
		base.SetReportBarButtonsProperties();
		ReportBarButtonItem barButtonJumpTo = _barButtonJumpTo;
		int num = 4;
		List<PropertyWithValue> list = new List<PropertyWithValue>(num);
		CollectionsMarshal.SetCount(list, num);
		Span<PropertyWithValue> span = CollectionsMarshal.AsSpan(list);
		span[0] = new PropertyWithValue("DocTypeParam", PO.POType.ToString().PadLeft(2, '0'));
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
		ReportBarButtonItem barButtonGoodsRetailOperationJournal = _barButtonGoodsRetailOperationJournal;
		num = 2;
		List<PropertyWithValue> list3 = new List<PropertyWithValue>(num);
		CollectionsMarshal.SetCount(list3, num);
		Span<PropertyWithValue> span3 = CollectionsMarshal.AsSpan(list3);
		span3[0] = new PropertyWithValue("DocISN", PO.ISN.ToString());
		span3[1] = new PropertyWithValue("AdditionalCaption", GetReportsAdditionalCaption());
		barButtonGoodsRetailOperationJournal.PropertyWithValues = list3;
	}

	protected override void OnNew()
	{
		base.OnNew();
		_date.EditValue = Settings.HostingEnvironment.CurrentDate;
		_docOwner.Code = Settings.LoginService.UserName;
		if (!string.IsNullOrWhiteSpace(_storage.Code))
		{
			_storageIsRetail = TradeSettings.TradeCacheService.Storage.GetItem(_storage.Code)?.Retail ?? false;
		}
		_salePriceCol.Visible = _storageIsRetail;
		_salePriceCol.ShowInColumnChooser = _storageIsRetail;
		_summaCol.Header = (_storageIsRetail ? ArmSoft.SME.Trade.Common.Resources.SaleSum : ArmSoft.SME.Common.Resources.Summa);
		if (_viewMode != WindowViewMode.Copy)
		{
			_chiefAccountant.EditValue = Settings.ParametersService.ChiefAccountantName();
		}
		else if (_distributeByFEFO)
		{
			PO.MTExpDocSelectedPartiesList.Clear();
			for (int i = 0; i < _mtExpenseList.Count; i++)
			{
				DistributeQtyByExpiredDate(_mtExpenseList[i].RowID, _storage.Code, _mtExpenseList[i].MTCode, _mtExpenseList[i].UnitCode, _mtExpenseList[i].Quantity, out var ExpSumm);
				_mtExpenseList[i].Summa = ExpSumm;
				_mtExpenseList[i].IsPartySelected = true;
			}
		}
	}

	protected override void OnNewAfterSave()
	{
		base.OnNewAfterSave();
		_docOwner.Code = Settings.LoginService.UserName;
		_baseDocumentControl.Text = string.Empty;
		_maxRowID = 0;
		_mtExpenseList.Clear();
	}

	protected override void OnEdit()
	{
		base.OnEdit();
		if (!string.IsNullOrWhiteSpace(_storage.Code))
		{
			_storageIsRetail = TradeSettings.TradeCacheService.Storage.GetItem(_storage.Code)?.Retail ?? false;
		}
		_salePriceCol.Visible = _storageIsRetail;
		_salePriceCol.ShowInColumnChooser = _storageIsRetail;
		_summaCol.Header = (_storageIsRetail ? ArmSoft.SME.Trade.Common.Resources.SaleSum : ArmSoft.SME.Common.Resources.Summa);
		_setFocusOnActivation = _docNum;
	}

	protected override void OnView()
	{
		base.OnView();
		_partyCol.ReadOnly = false;
		((ButtonEditSettings)_partyCol.EditSettings).IsTextEditable = false;
	}

	private void Storage_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		if (!_storage.IsReadOnly && !string.IsNullOrWhiteSpace((string)e.NewValue))
		{
			StoragesCacheRow item = TradeSettings.TradeCacheService.Storage.GetItem((string)e.NewValue);
			if (item != null)
			{
				_storageIsRetail = item.Retail;
				if (!TradeSettings.CreateTradeClient().CommonServices.IsStorageAvailable((string)e.NewValue))
				{
					e.Handled = true;
					_storage.Code = (string)e.OldValue;
					ArmSoft.SME.Core.UI.MessageBox.Show(string.Format(ArmSoft.SME.Trade.Common.Resources.StorageNotAvailForUser, Settings.LoginService.UserName, (string)e.NewValue), Settings.ProgramName, MessageBoxButton.OK, MessageBoxImage.Exclamation);
					return;
				}
			}
		}
		if (!PO.StoredInDb && !string.IsNullOrWhiteSpace(_storage.Code))
		{
			StoragesCacheRow item = TradeSettings.TradeCacheService.Storage.GetItem(_storage.Code);
			if (item != null)
			{
				_demand.EditValue = item.Response;
			}
		}
		if (!string.IsNullOrWhiteSpace(_storage.Code))
		{
			_storageIsRetail = TradeSettings.TradeCacheService.Storage.GetItem(_storage.Code)?.Retail ?? false;
		}
		if (_mtExpenseList.Count > 0 && e.NewValue != e.OldValue)
		{
			if (!_distributeByFEFO && _mtExpenseList.Any((MTExpenseSpecificationRow x) => x.IsPartySelected))
			{
				ArmSoft.SME.Core.UI.MessageBox.Show(string.Format(ArmSoft.SME.Trade.Common.Resources.PartiesValuesChanged, ArmSoft.SME.Common.Resources.Storage), Caption, MessageBoxButton.OK, MessageBoxImage.Asterisk);
			}
			if (_distributeByFEFO)
			{
				PO.MTExpDocSelectedPartiesList.Clear();
				for (int num = 0; num <= _mtExpenseList.Count - 1; num++)
				{
					DistributeQtyByExpiredDate(_mtExpenseList[num].RowID, _storage.Code, _mtExpenseList[num].MTCode, _mtExpenseList[num].UnitCode, _mtExpenseList[num].Quantity, out var ExpSumm);
					_mtExpenseList[num].Summa = ExpSumm;
					_mtExpenseList[num].IsPartySelected = true;
				}
			}
			else
			{
				for (int num2 = 0; num2 <= _mtExpenseList.Count - 1; num2++)
				{
					_mtExpenseList[num2].IsPartySelected = false;
					_mtExpenseList[num2].Summa = 0m;
				}
				PO.MTExpDocSelectedPartiesList.Clear();
			}
		}
		_salePriceCol.Visible = _storageIsRetail;
		_salePriceCol.ShowInColumnChooser = _storageIsRetail;
		_summaCol.Header = (_storageIsRetail ? ArmSoft.SME.Trade.Common.Resources.SaleSum : ArmSoft.SME.Common.Resources.Summa);
		RowInfoChange?.Invoke((MTExpenseSpecificationRow)_mtExpenseTableView.Grid.CurrentItem);
	}

	private void MTExpenseGrid_AddingRow(object sender, DocumentGridAddingRowEventArgs e)
	{
		ValidateDocFields();
		_mtExpenseList.Add(new MTExpenseSpecificationRow
		{
			IsPartySelected = false,
			RowID = _maxRowID
		});
		_maxRowID++;
		e.RowNumber = _mtExpenseList.Count - 1;
		RowInfoChangeEventHandler rowInfoChangeEventHandler = RowInfoChange;
		if (rowInfoChangeEventHandler != null)
		{
			ObservableCollection<MTExpenseSpecificationRow> mtExpenseList = _mtExpenseList;
			rowInfoChangeEventHandler(mtExpenseList[mtExpenseList.Count - 1]);
		}
	}

	private void MTExpenseGrid_DuplicatingRow(object sender, DocumentGridDuplicatingRowEventArgs e)
	{
		MTExpenseSpecificationRow mTExpenseSpecificationRow = (MTExpenseSpecificationRow)_mtExpenseGrid.Grid.GetRow(e.RowHandle);
		if (mTExpenseSpecificationRow != null)
		{
			MTExpenseSpecificationRow mTExpenseSpecificationRow2 = new MTExpenseSpecificationRow();
			mTExpenseSpecificationRow.Clone(mTExpenseSpecificationRow2);
			mTExpenseSpecificationRow2.IsPartySelected = false;
			mTExpenseSpecificationRow2.RowID = _maxRowID;
			_mtExpenseList.Add(mTExpenseSpecificationRow2);
			if (_distributeByFEFO)
			{
				DistributeQtyByExpiredDate(mTExpenseSpecificationRow2.RowID, _storage.Code, mTExpenseSpecificationRow2.MTCode, mTExpenseSpecificationRow2.UnitCode, mTExpenseSpecificationRow2.Quantity, out var ExpSumm);
				mTExpenseSpecificationRow2.Summa = ExpSumm;
				mTExpenseSpecificationRow2.IsPartySelected = true;
			}
			_maxRowID++;
			e.NewRowNumber = _mtExpenseList.Count - 1;
		}
	}

	private void MTExpenseGrid_DeletedRow(object sender, EventArgs e)
	{
		RowInfoChange?.Invoke((MTExpenseSpecificationRow)_mtExpenseTableView.Grid.CurrentItem);
	}

	private void MTExpenseGrid_DeletingRow(object sender, DocumentGridDeletingRowEventArgs e)
	{
		MTExpenseSpecificationRow mtExpGridRow = (MTExpenseSpecificationRow)_mtExpenseGrid.Grid.GetRow(e.RowHandle);
		if (mtExpGridRow != null)
		{
			PO.MTExpDocSelectedPartiesList.RemoveAll((PartyRow x) => x.GridRow == mtExpGridRow.RowID);
		}
		RowInfoChange?.Invoke((MTExpenseSpecificationRow)_mtExpenseTableView.Grid.CurrentItem);
	}

	private void MTExpenseGrid_MovingRowUp(object sender, DocumentGridMovingRowUpOrDownEventArgs e)
	{
		if (_mtExpenseGrid.Grid.SortInfo.Count > 0 || _mtExpenseTableView.Grid.VisibleRowCount != _mtExpenseList.Count)
		{
			throw new ASException(ArmSoft.SME.Common.Resources.NotAllowRowMoveInSortedGrid);
		}
		MTExpenseSpecificationRow mTExpenseSpecificationRow = (MTExpenseSpecificationRow)_mtExpenseGrid.Grid.GetRow(e.RowHandle);
		if (mTExpenseSpecificationRow != null)
		{
			_mtExpenseList.RemoveAt(e.RowHandle);
			_mtExpenseList.Insert(e.RowHandle - 1, mTExpenseSpecificationRow);
		}
	}

	private void MTExpenseGrid_MovingRowDown(object sender, DocumentGridMovingRowUpOrDownEventArgs e)
	{
		if (_mtExpenseGrid.Grid.SortInfo.Count > 0 || _mtExpenseTableView.Grid.VisibleRowCount != _mtExpenseList.Count)
		{
			throw new ASException(ArmSoft.SME.Common.Resources.NotAllowRowMoveInSortedGrid);
		}
		MTExpenseSpecificationRow mTExpenseSpecificationRow = (MTExpenseSpecificationRow)_mtExpenseGrid.Grid.GetRow(e.RowHandle);
		if (mTExpenseSpecificationRow != null)
		{
			_mtExpenseList.RemoveAt(e.RowHandle);
			_mtExpenseList.Insert(e.RowHandle + 1, mTExpenseSpecificationRow);
		}
	}

	protected override void SetReportBarButtonsEnable(bool isEnabled)
	{
		base.SetReportBarButtonsEnable(isEnabled);
		_barButtonJumpTo.IsEnabled = isEnabled;
		_barButtonGoodsOperationsJournal.IsEnabled = isEnabled;
		_barButtonGoodsRetailOperationJournal.IsEnabled = isEnabled;
	}

	protected override void LoadDataToControls()
	{
		base.LoadDataToControls();
		_baseISN = PO.BaseISN;
		if (_baseISN == Guid.Empty)
		{
			BaseDocument = null;
		}
		else
		{
			BaseDocument = (TradeDocument)Settings.DocumentService.Load(_baseISN, withoutAccessCheck: true);
		}
		_mtExpenseList.Clear();
		_focusedRowHandleChangedEvent.HandleOnLoadingDocumentToControls = true;
		foreach (MTExpense.MTExpenseSpecificationRow mT in PO.MTList)
		{
			MTExpenseSpecificationRow item = new MTExpenseSpecificationRow
			{
				RowID = mT.RowID,
				MTCode = mT.MTCode,
				MTName = mT.MTName,
				MTID = mT.MTID,
				Quantity = mT.Quantity,
				SalePrice = mT.SalePrice,
				Summa = mT.Summa,
				RowNum = mT.RowNum,
				UnitCode = mT.UnitCode,
				UnitBrief = mT.UnitBrief,
				IsPartySelected = mT.IsPartySelected
			};
			_mtExpenseList.Add(item);
		}
		_mtExpenseGrid.ItemsSource = _mtExpenseList;
		_maxRowID = PO.MaxRowID;
		if (!(_baseISN == Guid.Empty) && !StoredInDb && _viewMode != WindowViewMode.Copy)
		{
			CreateBasedOnDoc();
		}
		ReportBarButtonItem barButtonJumpTo = _barButtonJumpTo;
		int num = 4;
		List<PropertyWithValue> list = new List<PropertyWithValue>(num);
		CollectionsMarshal.SetCount(list, num);
		Span<PropertyWithValue> span = CollectionsMarshal.AsSpan(list);
		span[0] = new PropertyWithValue("DocTypeParam", PO.POType.ToString().PadLeft(2, '0'));
		span[1] = new PropertyWithValue("StartDate", PO.DocDate.ToString());
		span[2] = new PropertyWithValue("EndDate", PO.DocDate.ToString());
		span[3] = new PropertyWithValue("SelectedRowDocISN", PO.ISN.ToString());
		barButtonJumpTo.PropertyWithValues = list;
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
		_storage.IsReadOnly = flag2;
		_baseDocumentControl.IsReadOnly = flag2;
		_barCode.IsReadOnly = flag2;
		_codeEnter.IsEnabled = !flag2;
		foreach (GridColumn column in _mtExpenseGrid.Grid.Columns)
		{
			column.ReadOnly = flag2;
		}
		_mtExpenseGrid.AllowAdd = !flag2;
		_mtExpenseGrid.AllowEdit = !flag2;
		_mtExpenseGrid.AllowDelete = !flag2;
		_mtExpenseGrid.AllowDuplicate = !flag2;
		_mtExpenseGrid.AllowMoveUp = !flag2;
		_mtExpenseGrid.AllowMoveDown = !flag2;
		_partyCol.ReadOnly = false;
		((ButtonEditSettings)_partyCol.EditSettings).IsTextEditable = false;
	}

	private void CreateBasedOnDoc()
	{
		_comment.EditValue = BaseDocument.Comment;
		MTStockTakingBase mTStockTakingBase = (MTStockTakingBase)BaseDocument;
		_storage.Code = mTStockTakingBase.Storage;
		_mtExpenseList.Clear();
		_maxRowID = 0;
		PO.MTExpDocSelectedPartiesList.Clear();
		foreach (MTStockTakingBase.MTStockTakingSpecificationRow mT in mTStockTakingBase.MTList)
		{
			if (mT.VarianceQuantity < 0m)
			{
				MTExpenseSpecificationRow mTExpenseSpecificationRow = new MTExpenseSpecificationRow
				{
					MTCode = mT.MTCode,
					Quantity = Math.Abs(mT.VarianceQuantity),
					MTID = mT.MTID,
					UnitCode = mT.UnitCode
				};
				if (BaseDocument.POType == DocumentTypeSE.MTStockTakingRetail.Value)
				{
					mTExpenseSpecificationRow.SalePrice = mT.Price;
					mTExpenseSpecificationRow.Summa = (mTExpenseSpecificationRow.SalePrice * mTExpenseSpecificationRow.Quantity).RoundAs(2);
				}
				else
				{
					mTExpenseSpecificationRow.SalePrice = 0m;
					mTExpenseSpecificationRow.Summa = 0m;
				}
				mTExpenseSpecificationRow.IsPartySelected = false;
				mTExpenseSpecificationRow.RowID = _maxRowID;
				_mtExpenseList.Add(mTExpenseSpecificationRow);
				_maxRowID++;
			}
		}
		_mtExpenseGrid.Grid.RefreshData();
	}

	private void MTExpenseGrid_AddingRowWithDialog(object sender, DocumentGridAddingRowEventArgs e)
	{
		ValidateDocFields();
		GoodsSelectPartyWindow goodsSelectPartyWindow = new GoodsSelectPartyWindow(DialogWindowMode.NewModeWithNext, this);
		goodsSelectPartyWindow.ShowDialog();
		int addedRows = goodsSelectPartyWindow.AddedRows;
		if (addedRows != 0)
		{
			_maxRowID += addedRows;
			_mtExpenseGrid.Grid.RefreshData();
			RowInfoChangeEventHandler rowInfoChangeEventHandler = RowInfoChange;
			if (rowInfoChangeEventHandler != null)
			{
				ObservableCollection<MTExpenseSpecificationRow> mtExpenseList = _mtExpenseList;
				rowInfoChangeEventHandler(mtExpenseList[mtExpenseList.Count - 1]);
			}
		}
	}

	private void MTExpenseGrid_DuplicatingRowWithDialog(object sender, DocumentGridDuplicatingRowEventArgs e)
	{
		MTExpenseSpecificationRow mTExpenseSpecificationRow = (MTExpenseSpecificationRow)_mtExpenseGrid.Grid.GetRow(e.RowHandle);
		if (mTExpenseSpecificationRow != null)
		{
			ValidateDocFields();
			GoodsSelectPartyWindow goodsSelectPartyWindow = new GoodsSelectPartyWindow(DialogWindowMode.NewModeWithNext, this, mTExpenseSpecificationRow);
			goodsSelectPartyWindow.ShowDialog();
			int addedRows = goodsSelectPartyWindow.AddedRows;
			if (addedRows != 0)
			{
				_maxRowID += addedRows;
				_mtExpenseGrid.Grid.RefreshData();
				RowInfoChange(mTExpenseSpecificationRow);
			}
		}
	}

	private void MTExpenseGrid_EditingRowWithDialog(object sender, DocumentGridEditingRowEventArgs e)
	{
		MTExpenseSpecificationRow sourceRow = (MTExpenseSpecificationRow)_mtExpenseGrid.Grid.GetRow(e.RowHandle);
		if (sourceRow == null)
		{
			return;
		}
		ValidateDocFields();
		GoodsSelectPartyWindow goodsSelectPartyWindow = new GoodsSelectPartyWindow(e.AllowEdit ? DialogWindowMode.EditMode : DialogWindowMode.ViewMode, this, sourceRow);
		e.RowEdited = Convert.ToBoolean(goodsSelectPartyWindow.ShowDialog());
		if (_distributeByFEFO && sourceRow.IsPartySelected && _distributeByFEFO)
		{
			PO.MTExpDocSelectedPartiesList.RemoveAll((PartyRow x) => x.GridRow == sourceRow.RowID);
			DistributeQtyByExpiredDate(sourceRow.RowID, _storage.Code, sourceRow.MTCode, sourceRow.UnitCode, sourceRow.Quantity, out var ExpSumm);
			sourceRow.Summa = ExpSumm;
			sourceRow.IsPartySelected = true;
		}
		if (e.AllowEdit && e.RowEdited)
		{
			_mtExpenseGrid.Grid.RefreshData();
			RowInfoChange(sourceRow);
		}
	}

	private void MTExpenseTableView_CellValueChanged(object sender, CellValueChangedEventArgs e)
	{
		MTExpenseSpecificationRow mtExpGridRow = (MTExpenseSpecificationRow)_mtExpenseGrid.Grid.GetRow(e.RowHandle);
		if (mtExpGridRow == null)
		{
			return;
		}
		string unitCodeOld = string.Empty;
		if (e.OldValue != null)
		{
			unitCodeOld = e.OldValue.ToString();
		}
		if (e.Column == _codeCol)
		{
			try
			{
				if (e.Value == null)
				{
					PO.MTExpDocSelectedPartiesList.RemoveAll((PartyRow x) => x.GridRow == mtExpGridRow.RowID);
					mtExpGridRow.MTID = -1;
					mtExpGridRow.MTName = "";
					mtExpGridRow.UnitCode = "";
					mtExpGridRow.Summa = 0m;
					mtExpGridRow.IsPartySelected = false;
				}
				else
				{
					if (e.OldValue != null && e.OldValue.ToString() != e.Value.ToString())
					{
						PO.MTExpDocSelectedPartiesList.RemoveAll((PartyRow x) => x.GridRow == mtExpGridRow.RowID);
						mtExpGridRow.Summa = 0m;
						mtExpGridRow.IsPartySelected = false;
					}
					string text = e.Value.ToString();
					foreach (MTExpenseSpecificationRow mtExpense in _mtExpenseList)
					{
						if (mtExpGridRow != mtExpense && text == mtExpense.MTCode)
						{
							if (ArmSoft.SME.Core.UI.MessageBox.Show(string.Format(ArmSoft.SME.Trade.Common.Resources.MTExistInMTList, text), Settings.ProgramName, MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.Yes) == MessageBoxResult.No)
							{
								mtExpGridRow.MTCode = "";
								return;
							}
							break;
						}
					}
					if (string.IsNullOrWhiteSpace(text))
					{
						throw new Exception();
					}
					MaterialCacheRow item = TradeSettings.TradeCacheService.Material.GetItem(text, throwExceptionNotExits: true);
					mtExpGridRow.MTID = item.Id;
					_unitCodeCol.DataView = new MTQntUnits(item.MTCode);
					mtExpGridRow.UnitCode = item.Unit;
					mtExpGridRow.Summa = 0m;
					if (_storageIsRetail)
					{
						_salePriceCol.AllowEditing = DefaultBoolean.False;
						MaterialCacheRow.QntUnitRow qntUnit = TradeSettings.TradeCacheService.Material.GetQntUnit(mtExpGridRow.MTID, mtExpGridRow.UnitCode);
						if (qntUnit != null)
						{
							MTRetailQtyAndPriceDataProvider.Param param = new MTRetailQtyAndPriceDataProvider.Param
							{
								MTCode = mtExpGridRow.MTCode,
								Storage = _storage.Code,
								Date = _date.EditValue,
								UnitCoef = qntUnit.Coef
							};
							List<MTRetailQtyAndPriceDataProvider.DataRow> rows = new MTRetailQtyAndPriceDataProvider(TradeSettings.CreateTradeClient()).GetRows(param);
							if (rows.Count > 0)
							{
								mtExpGridRow.SalePrice = rows[0].Price;
							}
							MTRetailQtyAndPrice dataView = new MTRetailQtyAndPrice(mtExpGridRow.MTCode, qntUnit.Coef, _storage.Code, _date.EditValue);
							_salePriceCol.DataView = dataView;
						}
						mtExpGridRow.Summa = (mtExpGridRow.SalePrice * mtExpGridRow.Quantity).RoundAs(2);
					}
					mtExpGridRow.IsPartySelected = false;
					if (_distributeByFEFO)
					{
						DistributeQtyByExpiredDate(mtExpGridRow.RowID, _storage.Code, mtExpGridRow.MTCode, mtExpGridRow.UnitCode, mtExpGridRow.Quantity, out var ExpSumm);
						mtExpGridRow.Summa = ExpSumm;
						mtExpGridRow.IsPartySelected = true;
					}
				}
			}
			catch (Exception ex)
			{
				mtExpGridRow.MTName = "";
				mtExpGridRow.MTID = -1;
				mtExpGridRow.UnitCode = "";
				mtExpGridRow.Quantity = 0m;
				mtExpGridRow.Summa = 0m;
				mtExpGridRow.IsPartySelected = false;
				if (e.Value != null && !string.IsNullOrWhiteSpace(e.Value.ToString()))
				{
					Settings.ErrorHandler(ex);
				}
			}
			finally
			{
				RowInfoChange?.Invoke(mtExpGridRow);
			}
		}
		else if (e.Column == _unitCodeCol)
		{
			try
			{
				if (e.Value != null && !string.IsNullOrWhiteSpace(e.Value.ToString()) && !string.IsNullOrWhiteSpace(mtExpGridRow.MTCode))
				{
					UnitChanged(e.Value.ToString(), unitCodeOld, mtExpGridRow);
				}
			}
			catch (Exception ex2)
			{
				Settings.ErrorHandler(ex2);
			}
			finally
			{
				RowInfoChange?.Invoke(mtExpGridRow);
			}
		}
		else if (e.Column == _quantityCol)
		{
			if (Convert.ToDecimal(e.OldValue) != Convert.ToDecimal(e.Value))
			{
				if (Convert.ToDecimal(e.OldValue) != 0m)
				{
					if (_distributeByFEFO)
					{
						PO.MTExpDocSelectedPartiesList.RemoveAll((PartyRow x) => x.GridRow == mtExpGridRow.RowID);
						DistributeQtyByExpiredDate(mtExpGridRow.RowID, _storage.Code, mtExpGridRow.MTCode, mtExpGridRow.UnitCode, Convert.ToDecimal(e.Value), out var ExpSumm2);
						mtExpGridRow.Summa = ExpSumm2;
						mtExpGridRow.IsPartySelected = true;
					}
					else
					{
						if (mtExpGridRow.IsPartySelected)
						{
							ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Trade.Common.Resources.ForRowSelectedParties, ArmSoft.SME.Trade.Common.Resources.ProgramName, MessageBoxButton.OK, MessageBoxImage.Asterisk);
						}
						PO.MTExpDocSelectedPartiesList.RemoveAll((PartyRow x) => x.GridRow == mtExpGridRow.RowID);
						mtExpGridRow.IsPartySelected = false;
					}
				}
				else if (_distributeByFEFO)
				{
					DistributeQtyByExpiredDate(mtExpGridRow.RowID, _storage.Code, mtExpGridRow.MTCode, mtExpGridRow.UnitCode, Convert.ToDecimal(e.Value), out var ExpSumm3);
					mtExpGridRow.Summa = ExpSumm3;
					mtExpGridRow.IsPartySelected = true;
				}
			}
			if (_storageIsRetail)
			{
				mtExpGridRow.Summa = (mtExpGridRow.Quantity * mtExpGridRow.SalePrice).RoundAs(2);
			}
			else if (!_distributeByFEFO)
			{
				mtExpGridRow.Summa = 0m;
			}
			RowInfoChange?.Invoke(mtExpGridRow);
		}
		RowInfoChange?.Invoke(mtExpGridRow);
	}

	private void MTExpenseTableView_FocusedRowHandleChanged(object sender, FocusedRowHandleChangedEventArgs e)
	{
		MTExpenseSpecificationRow mTExpenseSpecificationRow = (MTExpenseSpecificationRow)_mtExpenseTableView.Grid.GetRow(_mtExpenseTableView.FocusedRowHandle);
		if (mTExpenseSpecificationRow != null)
		{
			ChangeColumnAdditonalView(mTExpenseSpecificationRow.MTCode);
		}
		RowInfoChange(mTExpenseSpecificationRow);
	}

	private void ChangeColumnAdditonalView(string itemCode)
	{
		_unitCodeCol.DataView = new MTQntUnits(itemCode);
	}

	private void DistributeQtyByExpiredDate(int RowID, string Storage, string GoodCode, string UnitCode, decimal Quantity, out decimal ExpSumm, string BaseUnit = "")
	{
		decimal num = Quantity;
		List<PartyRow> list = new List<PartyRow>();
		List<PartyRow> list2 = new List<PartyRow>();
		List<PartyRow> list3 = new List<PartyRow>();
		bool flag = TradeSettings.CreateTradeClient().CommonServices.IsNegativExpAllow(Storage);
		string empty = string.Empty;
		ExpSumm = 0m;
		if (string.IsNullOrWhiteSpace(GoodCode))
		{
			return;
		}
		GoodsEngineServicesRoutes.PartyRowAndExpMethod parties = TradeSettings.CreateTradeClient().GoodsEngineServices.GetParties(Storage, GoodCode, _date.EditValue.Value);
		List<PartyRow> partyRows = parties.partyRows;
		empty = parties.expMethod;
		MaterialCacheRow item = TradeSettings.TradeCacheService.Material.GetItem(GoodCode, throwExceptionNotExits: true);
		BaseUnit = item.Unit;
		Quantity = (Quantity * PriceHelper.GetMTQntUnitCoeff(item.MTCode, UnitCode)).RoundAs(3);
		num = Quantity;
		UnitCode = BaseUnit;
		foreach (PartyRow item2 in partyRows)
		{
			item2.ExpQty = 0m;
			item2.ExpSumma = 0m;
			item2.GridRow = RowID;
			if (!item2.UseableDate.HasValue)
			{
				list2.Add(item2);
			}
			else
			{
				list3.Add(item2);
			}
		}
		list.AddRange(from ListRow in list3
			orderby ListRow.UseableDate, ListRow.IncomeDate, ListRow.OrderNum, ListRow.Row
			select ListRow);
		if (list != null)
		{
			foreach (PartyRow item3 in list)
			{
				if (!flag)
				{
					if (item3.Qty > 0m)
					{
						if (num >= item3.Qty)
						{
							item3.ExpQty = item3.Qty;
							PartyRow partyRow = item3;
							decimal expSumma;
							if (item3.ExpQty == item3.Qty)
							{
								expSumma = item3.Summa;
							}
							else
							{
								expSumma = ((!flag) ? (item3.Cost * item3.ExpQty).RoundAs(2) : ((item3.Cost * item3.ExpQty < item3.Qty) ? item3.ExpQty : item3.Qty).RoundAs(2));
							}
							partyRow.ExpSumma = expSumma;
							num -= item3.Qty;
						}
						else
						{
							item3.ExpQty = num;
							PartyRow partyRow2 = item3;
							decimal expSumma2;
							if (item3.ExpQty == item3.Qty)
							{
								expSumma2 = item3.Summa;
							}
							else
							{
								expSumma2 = ((!flag) ? (item3.Cost * item3.ExpQty).RoundAs(2) : ((item3.Cost * item3.ExpQty < item3.Qty) ? item3.ExpQty : item3.Qty).RoundAs(2));
							}
							partyRow2.ExpSumma = expSumma2;
							num = 0m;
						}
					}
				}
				else if (item3.Qty > 0m)
				{
					if (num >= item3.Qty)
					{
						item3.ExpQty = item3.Qty;
						PartyRow partyRow3 = item3;
						decimal expSumma3;
						if (item3.ExpQty == item3.Qty)
						{
							expSumma3 = item3.Summa;
						}
						else
						{
							expSumma3 = ((!flag) ? (item3.Cost * item3.ExpQty).RoundAs(2) : ((item3.Cost * item3.ExpQty < item3.Qty) ? item3.ExpQty : item3.Qty).RoundAs(2));
						}
						partyRow3.ExpSumma = expSumma3;
						num -= item3.Qty;
					}
					else
					{
						item3.ExpQty = num;
						PartyRow partyRow4 = item3;
						decimal expSumma4;
						if (item3.ExpQty == item3.Qty)
						{
							expSumma4 = item3.Summa;
						}
						else
						{
							expSumma4 = ((!flag) ? (item3.Cost * item3.ExpQty).RoundAs(2) : ((item3.Cost * item3.ExpQty < item3.Qty) ? item3.ExpQty : item3.Qty).RoundAs(2));
						}
						partyRow4.ExpSumma = expSumma4;
						num = 0m;
					}
				}
			}
			if ((num > 0m) & flag)
			{
				partyRows[partyRows.Count - 1].ExpQty += num;
			}
		}
		if (num > 0m && list2 != null)
		{
			foreach (PartyRow item4 in list2)
			{
				if (!flag)
				{
					if (item4.Qty > 0m)
					{
						if (num >= item4.Qty)
						{
							item4.ExpQty = item4.Qty;
							PartyRow partyRow5 = item4;
							decimal expSumma5;
							if (item4.ExpQty == item4.Qty)
							{
								expSumma5 = item4.Summa;
							}
							else
							{
								expSumma5 = ((!flag) ? (item4.Cost * item4.ExpQty).RoundAs(2) : ((item4.Cost * item4.ExpQty < item4.Qty) ? item4.ExpQty : item4.Qty).RoundAs(2));
							}
							partyRow5.ExpSumma = expSumma5;
							num -= item4.Qty;
						}
						else
						{
							item4.ExpQty = num;
							PartyRow partyRow6 = item4;
							decimal expSumma6;
							if (item4.ExpQty == item4.Qty)
							{
								expSumma6 = item4.Summa;
							}
							else
							{
								expSumma6 = ((!flag) ? (item4.Cost * item4.ExpQty).RoundAs(2) : ((item4.Cost * item4.ExpQty < item4.Qty) ? item4.ExpQty : item4.Qty).RoundAs(2));
							}
							partyRow6.ExpSumma = expSumma6;
							num = 0m;
						}
					}
				}
				else if (num >= item4.Qty)
				{
					if (item4 == partyRows[partyRows.Count - 1])
					{
						item4.ExpQty = num;
						PartyRow partyRow7 = item4;
						decimal expSumma7;
						if (item4.ExpQty == item4.Qty)
						{
							expSumma7 = item4.Summa;
						}
						else
						{
							expSumma7 = ((!flag) ? (item4.Cost * item4.ExpQty).RoundAs(2) : ((item4.Cost * item4.ExpQty < item4.Qty) ? item4.ExpQty : item4.Qty).RoundAs(2));
						}
						partyRow7.ExpSumma = expSumma7;
						num = 0m;
					}
					else
					{
						item4.ExpQty = item4.Qty;
						PartyRow partyRow8 = item4;
						decimal expSumma8;
						if (item4.ExpQty == item4.Qty)
						{
							expSumma8 = item4.Summa;
						}
						else
						{
							expSumma8 = ((!flag) ? (item4.Cost * item4.ExpQty).RoundAs(2) : ((item4.Cost * item4.ExpQty < item4.Qty) ? item4.ExpQty : item4.Qty).RoundAs(2));
						}
						partyRow8.ExpSumma = expSumma8;
						num -= item4.Qty;
					}
				}
				else
				{
					item4.ExpQty = num;
					PartyRow partyRow9 = item4;
					decimal expSumma9;
					if (item4.ExpQty == item4.Qty)
					{
						expSumma9 = item4.Summa;
					}
					else
					{
						expSumma9 = ((!flag) ? (item4.Cost * item4.ExpQty).RoundAs(2) : ((item4.Cost * item4.ExpQty < item4.Qty) ? item4.ExpQty : item4.Qty).RoundAs(2));
					}
					partyRow9.ExpSumma = expSumma9;
					num = 0m;
				}
			}
		}
		List<PartyRow> list4 = list2;
		int num2 = list.Count + list4.Count;
		List<PartyRow> list5 = new List<PartyRow>(num2);
		CollectionsMarshal.SetCount(list5, num2);
		Span<PartyRow> span = CollectionsMarshal.AsSpan(list5);
		int num3 = 0;
		Span<PartyRow> span2 = CollectionsMarshal.AsSpan(list);
		span2.CopyTo(span.Slice(num3, span2.Length));
		num3 += span2.Length;
		Span<PartyRow> span3 = CollectionsMarshal.AsSpan(list4);
		span3.CopyTo(span.Slice(num3, span3.Length));
		num3 += span3.Length;
		partyRows = list5;
		List<PartyRow> list6 = new List<PartyRow>();
		if (partyRows.Count > 1)
		{
			if (empty == "2")
			{
				list6.AddRange(from ListRow in partyRows
					orderby ListRow.IncomeDate descending, ListRow.OrderNum, ListRow.Row
					select ListRow);
			}
			else
			{
				list6.AddRange(from ListRow in partyRows
					orderby ListRow.IncomeDate, ListRow.OrderNum, ListRow.Row
					select ListRow);
			}
		}
		else
		{
			list6 = partyRows;
		}
		foreach (PartyRow item5 in partyRows)
		{
			PO.MTExpDocSelectedPartiesList.Add(item5.Clone());
			ExpSumm += item5.ExpSumma;
		}
	}

	private void UnitChanged(string unitCodeNew, string unitCodeOld, MTExpenseSpecificationRow mtExpGridRow, bool distributeparties = true)
	{
		QntUnitCacheRow item = TradeSettings.TradeCacheService.QntUnit.GetItem(unitCodeNew);
		if (item == null)
		{
			mtExpGridRow.UnitBrief = string.Empty;
			return;
		}
		mtExpGridRow.UnitBrief = item.Brief;
		TradeSettings.TradeCacheService.Material.GetItem(mtExpGridRow.MTCode);
		TradeSettings.TradeCacheService.Material.GetQntUnit(mtExpGridRow.MTID, item.Code);
		if (_storageIsRetail)
		{
			mtExpGridRow.SalePrice = PriceHelper.GetPriceForUnit(unitCodeOld, unitCodeNew, mtExpGridRow.SalePrice, mtExpGridRow.MTCode);
		}
		if (_distributeByFEFO & distributeparties)
		{
			PO.MTExpDocSelectedPartiesList.RemoveAll((PartyRow x) => x.GridRow == mtExpGridRow.RowID);
			DistributeQtyByExpiredDate(mtExpGridRow.RowID, _storage.Code, mtExpGridRow.MTCode, mtExpGridRow.UnitCode, mtExpGridRow.Quantity, out var ExpSumm);
			mtExpGridRow.Summa = ExpSumm;
			mtExpGridRow.IsPartySelected = true;
		}
	}

	private void ValidateDocFields()
	{
		if (!_date.DoValidate())
		{
			ControlsHelper.SetFocus(_date);
			throw new ASException(ArmSoft.SME.Common.Resources.ValidateEmptyDate);
		}
		if (string.IsNullOrWhiteSpace(_storage.Code))
		{
			ControlsHelper.SetFocus(_storage);
			throw new ASException(ArmSoft.SME.Common.Resources.ValidateEmptyStorage);
		}
		if (!_storage.DoValidate())
		{
			ControlsHelper.SetFocus(_storage);
			throw new ASException(string.Format(ArmSoft.SME.Trade.Common.Resources.StorageNotExist, _storage.Code));
		}
	}

	public override void OnScanner(string ScannerData)
	{
		if (!_barCode.IsReadOnly)
		{
			_barCode.Text = ScannerData;
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

	private void ScannerAdd()
	{
		_barCode.DoValidate();
		try
		{
			string barCode = _barCode.Text.Trim();
			if (string.IsNullOrWhiteSpace(_storage.Code))
			{
				ControlsHelper.SetFocus(_storage);
				throw new ASException(string.Format(ArmSoft.SME.Common.Resources.FieldIsEmpty, ArmSoft.SME.Common.Resources.Storage));
			}
			if (!_storage.DoValidate())
			{
				ControlsHelper.SetFocus(_storage);
				throw new ASException(string.Format(ArmSoft.SME.Trade.Common.Resources.StorageNotExist, _storage.Code));
			}
			MaterialCacheRow materialCacheRow = DocumentHelpers.TryGetItemByBarCode(ref barCode, out var qty, out var unitCode, out var unitBrief);
			if (materialCacheRow == null || materialCacheRow.Type == ItemTypesSE.Services.Value)
			{
				ControlsHelper.SetFocus(_barCode);
				throw new ASException(string.Format(ArmSoft.SME.Common.Resources.ErrBarCodeNotExist, barCode));
			}
			AddMTToList(materialCacheRow, qty, unitCode, unitBrief);
			_barCode.Text = "";
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
		if ((int)e.Key == 6 && (int)Keyboard.Modifiers == 0 && !_barCode.IsReadOnly && !string.IsNullOrWhiteSpace(_barCode.Text))
		{
			ScannerAdd();
		}
	}

	private void AddMTToList(MaterialCacheRow GoodsRow, decimal Quantity, string UnitCode = "", string UnitBrief = "")
	{
		string text = Settings.ParametersService.ScannerMode();
		if (text == ScanerModesSE.AddNewRow)
		{
			AddMTToListNewRow(GoodsRow, string.IsNullOrWhiteSpace(UnitCode) ? GoodsRow.Unit : UnitCode, string.IsNullOrWhiteSpace(UnitBrief) ? GoodsRow.UnitRow.Brief : UnitBrief, Quantity);
		}
		else
		{
			AddNewRowOrMergeInner(GoodsRow, text == ScanerModesSE.MergeWithCurrentRow, string.IsNullOrWhiteSpace(UnitCode) ? GoodsRow.Unit : UnitCode, string.IsNullOrWhiteSpace(UnitBrief) ? GoodsRow.UnitRow.Brief : UnitBrief, Quantity);
		}
	}

	private void AddNewRowOrMergeInner(MaterialCacheRow GoodRow, bool ForFocusedRow, string UnitCode, string UnitBrief, decimal Quantity)
	{
		MTExpenseSpecificationRow mTExpenseSpecificationRow = null;
		if (ForFocusedRow && _mtExpenseList.Count != 0)
		{
			mTExpenseSpecificationRow = (MTExpenseSpecificationRow)_mtExpenseTableView.Grid.CurrentItem;
			if (mTExpenseSpecificationRow == null)
			{
				return;
			}
		}
		for (int i = 0; i <= _mtExpenseList.Count - 1; i++)
		{
			if ((!ForFocusedRow || mTExpenseSpecificationRow.RowID == _mtExpenseList[i].RowID) && GoodRow.Id == _mtExpenseList[i].MTID && UnitCode == _mtExpenseList[i].UnitCode && !_mtExpenseList[i].IsPartySelected)
			{
				mTExpenseSpecificationRow = _mtExpenseList[i];
				FillMTInvGridRow(mTExpenseSpecificationRow, GoodRow, UnitCode, UnitBrief, Quantity);
				int rowHandleByListIndex = _mtExpenseGrid.Grid.GetRowHandleByListIndex(i);
				POWindow<MTExpense>.SelectCellInGrid(_mtExpenseTableView, _mtExpenseGrid, rowHandleByListIndex, "Quantity");
				return;
			}
		}
		AddMTToListNewRow(GoodRow, UnitCode, UnitBrief, Quantity);
	}

	private void AddMTToListNewRow(MaterialCacheRow GoodRow, string UnitCode, string UnitBrief, decimal Quantity)
	{
		MTExpenseSpecificationRow row = new MTExpenseSpecificationRow();
		FillMTInvGridRow(row, GoodRow, UnitCode, UnitBrief, Quantity);
		row.RowID = _maxRowID;
		_mtExpenseList.Add(row);
		_maxRowID++;
		int rowHandleByListIndex = _mtExpenseGrid.Grid.GetRowHandleByListIndex(_mtExpenseList.Count - 1);
		if (row != null && _distributeByFEFO)
		{
			PO.MTExpDocSelectedPartiesList.RemoveAll((PartyRow x) => x.GridRow == row.RowID);
			DistributeQtyByExpiredDate(row.RowID, _storage.Code, GoodRow.MTCode, GoodRow.Unit, row.Quantity, out var ExpSumm, GoodRow.Unit);
			row.Summa = ExpSumm;
			row.IsPartySelected = true;
		}
		POWindow<MTExpense>.SelectCellInGrid(_mtExpenseTableView, _mtExpenseGrid, rowHandleByListIndex, "Quantity");
		_mtExpenseGrid.ConfigureButtons();
		RowInfoChange(row);
	}

	private void FillMTInvGridRow(MTExpenseSpecificationRow mtExpenseGridRow, MaterialCacheRow GoodsRow, string UnitCode, string UnitBrief, decimal Quantity = 1m)
	{
		if (GoodsRow != null)
		{
			mtExpenseGridRow.MTID = GoodsRow.Id;
			mtExpenseGridRow.MTCode = GoodsRow.MTCode;
		}
		mtExpenseGridRow.UnitCode = UnitCode;
		mtExpenseGridRow.UnitBrief = UnitBrief;
		mtExpenseGridRow.Quantity += Quantity;
		if (_storageIsRetail)
		{
			_salePriceCol.AllowEditing = DefaultBoolean.False;
			MaterialCacheRow.QntUnitRow qntUnit = TradeSettings.TradeCacheService.Material.GetQntUnit(mtExpenseGridRow.MTID, mtExpenseGridRow.UnitCode);
			if (qntUnit != null)
			{
				MTRetailQtyAndPriceDataProvider.Param param = new MTRetailQtyAndPriceDataProvider.Param
				{
					MTCode = mtExpenseGridRow.MTCode,
					Storage = _storage.Code,
					Date = _date.EditValue,
					UnitCoef = qntUnit.Coef
				};
				List<MTRetailQtyAndPriceDataProvider.DataRow> rows = new MTRetailQtyAndPriceDataProvider(TradeSettings.CreateTradeClient()).GetRows(param);
				if (rows.Count > 0)
				{
					mtExpenseGridRow.SalePrice = rows[0].Price;
				}
			}
			mtExpenseGridRow.Summa = (mtExpenseGridRow.SalePrice * mtExpenseGridRow.Quantity).RoundAs(2);
		}
		mtExpenseGridRow.IsPartySelected = false;
	}

	private void MTExpenseGrid_ClickDropDown(object sender, DocumentGridClickDropDownEventArgs e)
	{
		MTExpenseSpecificationRow mTExpenseSpecificationRow = (MTExpenseSpecificationRow)_mtExpenseGrid.Grid.GetRow(e.RowHandle);
		if (e.Column == _codeCol)
		{
			ValidateDocFields();
			bool showAll = TradeSettings.CreateTradeClient().CommonServices.IsNegativExpAllow(_storage.Code);
			ArmSoft.SME.Trade.UI.DropDown.GoodsAvailabilityFilter goodsAvailabilityFilter = new ArmSoft.SME.Trade.UI.DropDown.GoodsAvailabilityFilter(_date.EditValue.Value, _storage.Code, _storageIsRetail, includeServices: false, showAll);
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
		else if (e.Column == _unitCodeCol)
		{
			if (mTExpenseSpecificationRow == null)
			{
				return;
			}
			MTQntUnits mTQntUnits = new MTQntUnits(mTExpenseSpecificationRow.MTCode);
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
		else if (e.Column == _salePriceCol)
		{
			if (_storageIsRetail)
			{
				if (mTExpenseSpecificationRow == null)
				{
					return;
				}
				MaterialCacheRow.QntUnitRow qntUnit = TradeSettings.TradeCacheService.Material.GetQntUnit(mTExpenseSpecificationRow.MTID, mTExpenseSpecificationRow.UnitCode);
				if (qntUnit != null)
				{
					MTRetailQtyAndPrice mTRetailQtyAndPrice = new MTRetailQtyAndPrice(mTExpenseSpecificationRow.MTCode, qntUnit.Coef, _storage.Code, _date.EditValue.Value);
					ModalBrowser modalBrowser3 = new ModalBrowser(mTRetailQtyAndPrice)
					{
						LookUpValue = mTExpenseSpecificationRow.UnitCode,
						Owner = e.Owner,
						Left = e.Left,
						Top = e.Top
					};
					if (modalBrowser3.Show())
					{
						mTExpenseSpecificationRow.SalePrice = Convert.ToDecimal(mTRetailQtyAndPrice.CommentMember(modalBrowser3.SelectedRowIndex));
						mTExpenseSpecificationRow.Summa = (mTExpenseSpecificationRow.SalePrice * mTExpenseSpecificationRow.Quantity).RoundAs(2);
					}
				}
			}
		}
		else if (e.Column == _partyCol)
		{
			if (mTExpenseSpecificationRow == null)
			{
				return;
			}
			ValidateDocFields();
			if (string.IsNullOrWhiteSpace(mTExpenseSpecificationRow.MTCode))
			{
				throw new ASException(ArmSoft.SME.Common.Resources.ValidateEmptyCode);
			}
			MaterialCacheRow item = TradeSettings.TradeCacheService.Material.GetItem(mTExpenseSpecificationRow.MTCode, throwExceptionNotExits: true);
			decimal quantity = (mTExpenseSpecificationRow.Quantity * PriceHelper.GetMTQntUnitCoeff(item.MTCode, mTExpenseSpecificationRow.UnitCode)).RoundAs(3);
			MTSelectPartyWindow mTSelectPartyWindow = new MTSelectPartyWindow(_storage.Code, _date.EditValue.Value, mTExpenseSpecificationRow.MTCode, item.Unit, quantity, mTExpenseSpecificationRow.RowID, PO.StoredInDb && !IsDraftOrDeleted)
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
					mTExpenseSpecificationRow.MTCode = mTSelectPartyWindow.MTCode;
					mTExpenseSpecificationRow.MTID = item.Id;
					mTExpenseSpecificationRow.Quantity = mTSelectPartyWindow.TotalExpQty;
					string unitCode = mTExpenseSpecificationRow.UnitCode;
					mTExpenseSpecificationRow.UnitCode = item.Unit;
					mTExpenseSpecificationRow.UnitBrief = item.UnitRow.Brief;
					if (_distributeByFEFO)
					{
						UnitChanged(item.Unit, unitCode, mTExpenseSpecificationRow, distributeparties: false);
					}
					if (!_storageIsRetail)
					{
						mTExpenseSpecificationRow.Summa = mTSelectPartyWindow.TotalExpSumm;
					}
					mTExpenseSpecificationRow.IsPartySelected = mTSelectPartyWindow.IsPartySelected.Value;
					mTExpenseSpecificationRow.ValidateWithoutResponse();
					PO.MTExpDocSelectedPartiesList = mTSelectPartyWindow.MTExpDocSelectedPartiesList;
				}
			}
		}
		_mtExpenseGrid.Grid.RefreshData();
	}

	private void FilteredSearch_Click(object sender, RoutedEventArgs e)
	{
		ValidateDocFields();
		FilteredSearchWindow filteredSearchWindow = new FilteredSearchWindow(new FilteredSearch
		{
			FilterDataView = new GoodsGroups(showAsTree: true, filteredSearch: true),
			SearchDataView = new ArmSoft.SME.Trade.UI.DropDown.GoodsFilter(shortMode: true, ItemTypesSE.Goods.Value),
			ResultsList = new MTExpenseFilteredSearchList()
		}, this, isBasketMode: true);
		if (filteredSearchWindow.ShowDialog().Value)
		{
			foreach (MTExpenseSpecificationRow item in filteredSearchWindow.BasketResult)
			{
				if (_storageIsRetail)
				{
					MaterialCacheRow.QntUnitRow qntUnit = TradeSettings.TradeCacheService.Material.GetQntUnit(item.MTID, item.UnitCode);
					MTRetailQtyAndPriceDataProvider.Param param = new MTRetailQtyAndPriceDataProvider.Param
					{
						MTCode = item.MTCode,
						Storage = _storage.Code,
						Date = _date.EditValue,
						UnitCoef = qntUnit.Coef
					};
					if (qntUnit != null)
					{
						List<MTRetailQtyAndPriceDataProvider.DataRow> rows = new MTRetailQtyAndPriceDataProvider(TradeSettings.CreateTradeClient()).GetRows(param);
						if (rows.Count > 0)
						{
							item.SalePrice = rows[0].Price;
						}
					}
					item.Summa = (item.SalePrice * item.Quantity).RoundAs(2);
				}
				item.RowID = _maxRowID;
				_mtExpenseList.Add(item);
				if (_distributeByFEFO)
				{
					DistributeQtyByExpiredDate(item.RowID, _storage.Code, item.MTCode, item.UnitCode, item.Quantity, out var ExpSumm);
					item.Summa = ExpSumm;
					item.IsPartySelected = true;
				}
				_maxRowID++;
			}
		}
		RowInfoChange((MTExpenseSpecificationRow)_mtExpenseTableView.Grid.CurrentItem);
	}

	private void MTExpenseGrid_RefreshButtons(object sender, EventArgs e)
	{
		_filteredSearch.IsEnabled = _mtExpenseGrid.AllowAdd && !_mtExpenseGrid.IsReadOnly;
	}

	protected override bool AllowPrint()
	{
		return true;
	}

	protected override bool AllowPrintOnNew()
	{
		return true;
	}

	protected override void FillDocument(StoreRequest sr)
	{
		base.FillDocument(sr);
		PO.MaxRowID = _maxRowID;
		PO.MTList.Clear();
		foreach (MTExpenseSpecificationRow mtExpense in _mtExpenseList)
		{
			MTExpense.MTExpenseSpecificationRow item = new MTExpense.MTExpenseSpecificationRow
			{
				RowID = mtExpense.RowID,
				MTID = mtExpense.MTID,
				MTCode = mtExpense.MTCode,
				MTName = mtExpense.MTName,
				Quantity = mtExpense.Quantity,
				SalePrice = mtExpense.SalePrice,
				Summa = mtExpense.Summa,
				RowNum = mtExpense.RowNum,
				UnitCode = mtExpense.UnitCode,
				UnitBrief = mtExpense.UnitBrief,
				IsPartySelected = mtExpense.IsPartySelected
			};
			PO.MTList.Add(item);
		}
	}

	protected override bool AllowChangeGridMode()
	{
		return true;
	}

	private void BaseDocument_BtnClearClicked(object sender, EditKeyValueChangedEventArgs e)
	{
		_baseDocumentControl.KeyValue = Guid.Empty;
		_baseDocumentControl.DoValidate();
	}

	private void BaseDocument_EditKeyValueChanged(object sender, EditKeyValueChangedEventArgs e)
	{
		BaseISN = ((e.KeyValue != null) ? ((Guid)e.KeyValue) : Guid.Empty);
	}

	private void BaseDocument_ClickDropDown(object sender, DropDownControlEventArgs e)
	{
		DocumentsJournal documentsJournal = (DocumentsJournal)e.DataView;
		documentsJournal.DialogTop = e.Top;
		documentsJournal.DialogLeft = e.Left;
		if (!e.DataView.ShowDialog())
		{
			e.Cancel = true;
		}
	}

	private void BaseDocumentSearch_Click(object sender, EditKeyValueChangedEventArgs e)
	{
		if (BaseISN != Guid.Empty)
		{
			Settings.DocumentService.ShowWindow(BaseISN, WindowViewMode.Edit);
		}
	}

	protected override void OnF8()
	{
		CodeEnterClick();
	}

	private void CodeEnterClick()
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
			ArmSoft.SME.Trade.UI.DropDown.GoodsFilter goodsFilter = new ArmSoft.SME.Trade.UI.DropDown.GoodsFilter(shortMode: true, ItemTypesSE.Goods);
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
		AddMTToList(item, 1m, null, null);
		_barCode.Text = "";
		RowInfoChangeEventHandler rowInfoChangeEventHandler = RowInfoChange;
		if (rowInfoChangeEventHandler != null)
		{
			ObservableCollection<MTExpenseSpecificationRow> mtExpenseList = _mtExpenseList;
			rowInfoChangeEventHandler(mtExpenseList[mtExpenseList.Count - 1]);
		}
	}

	private void BarButtonDocumentSettings_ItemClick(object sender, ItemClickEventArgs e)
	{
		DialogWindow dialogWindow = new DialogWindow
		{
			Caption = ArmSoft.SME.Common.Resources.Settings,
			Owner = this
		};
		LayoutGroupExt layoutGroupExt = dialogWindow.AddHorizontalLayoutGroup();
		layoutGroupExt.Orientation = Orientation.Vertical;
		layoutGroupExt.HorizontalAlignment = HorizontalAlignment.Stretch;
		layoutGroupExt.Header = ArmSoft.SME.Common.Resources.ItemList;
		layoutGroupExt.View = LayoutGroupView.GroupBox;
		dialogWindow.AddBooleanControl(layoutGroupExt, "DistributByFEFO", ArmSoft.SME.Trade.Common.Resources.FEFODistibuteLong).IsChecked = _distributeByFEFO;
		if (dialogWindow.ShowDialog().Value)
		{
			_distributeByFEFO = (bool)dialogWindow["DistributByFEFO"];
		}
	}

	private void CodeEnter_Click(object sender, RoutedEventArgs e)
	{
		CodeEnterClick();
	}

	protected override void StoreSettings(RegistryKey rk)
	{
		base.StoreSettings(rk);
		rk.SetValue("DistributByFEFO", _distributeByFEFO ? "1" : "0");
		_mtExpenseGrid.StoreSettings(rk);
	}

	protected override void RestoreSettings(double DefaultWidth, double DefaultHeight, RegistryKey rk)
	{
		base.RestoreSettings(DefaultWidth, DefaultHeight, rk);
		if (rk != null)
		{
			_distributeByFEFO = rk.GetValue("DistributByFEFO", false).ToString() == "1";
			_mtExpenseGrid.RestoreSettings(rk);
		}
	}
}
