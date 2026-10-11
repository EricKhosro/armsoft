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
using ArmSoft.SME.Common.Constants;
using ArmSoft.SME.Common.CustomTypes;
using ArmSoft.SME.Common.Extensions;
using ArmSoft.SME.Common.Models;
using ArmSoft.SME.Common.Models.Documents;
using ArmSoft.SME.Common.Parameters;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Core.UI.Controls;
using ArmSoft.SME.Core.UI.DataViews;
using ArmSoft.SME.Core.UI.Documents;
using ArmSoft.SME.Core.UI.DropDowns;
using ArmSoft.SME.Core.UI.Enums;
using ArmSoft.SME.Core.UI.Extensions;
using ArmSoft.SME.Core.UI.Helpers;
using ArmSoft.SME.Core.UI.PersistableObject;
using ArmSoft.SME.Trade.Client.Data.Directories;
using ArmSoft.SME.Trade.Client.Data.DropDowns;
using ArmSoft.SME.Trade.Client.Data.Reports;
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
using DevExpress.Xpf.Editors.Settings;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.LayoutControl;
using DevExpress.XtraEditors.DXErrorProvider;
using Microsoft.Win32;

namespace ArmSoft.SME.Trade.UI.Documents;

[DocumentWindow(23)]
public class MTRevaluationRetailWindow : TradeDocumentWindow<MTRevaluationRetail>
{
	public class MTRevaluationRetailFilteredSearchList : IResultList
	{
		private readonly MTRevaluationRetailWindow _docWindow;

		private readonly ObservableCollection<MTRevaluationRetailSpecificationRow> itemSource = new ObservableCollection<MTRevaluationRetailSpecificationRow>();

		public IEnumerable<DocumentGridRow> ItemsSource => itemSource;

		public List<FilteredSearchGridColumn> Columns => new List<FilteredSearchGridColumn>
		{
			new FilteredSearchGridColumn
			{
				Header = TradeAllDocDefinitions.MTRevaluationRetail.Grids["MTList"].Columns["MTCode"].Caption,
				FieldName = "MTCode",
				CustomWidth = 125.0
			},
			new FilteredSearchGridColumn
			{
				Header = TradeAllDocDefinitions.MTRevaluationRetail.Grids["MTList"].Columns["MTName"].Caption,
				FieldName = "MTName",
				CustomWidth = 300.0
			},
			new FilteredSearchGridColumn
			{
				Header = TradeAllDocDefinitions.MTRevaluationRetail.Grids["MTList"].Columns["Quantity"].Caption,
				FieldName = "Quantity",
				AllowEditing = true,
				NumericFormat = DecimalExtension.GetCurrencyFormat(13, 3)
			}
		};

		public MTRevaluationRetailFilteredSearchList(MTRevaluationRetailWindow docWindow)
		{
			_docWindow = docWindow;
		}

		public int AddToItemsSource(object key, params object[] args)
		{
			string mtCode = key.ToString();
			decimal num = Convert.ToDecimal(args[0]);
			int num2 = itemSource.FindIndex((MTRevaluationRetailSpecificationRow x) => x.MTCode == mtCode);
			if (num2 == -1)
			{
				MaterialCacheRow item = TradeSettings.TradeCacheService.Material.GetItem(mtCode);
				if (item == null)
				{
					return -1;
				}
				MTRevaluationRetailSpecificationRow item2 = new MTRevaluationRetailSpecificationRow(_docWindow)
				{
					MTID = item.Id,
					MTCode = item.MTCode,
					MTName = item.Caption,
					UnitCode = item.Unit,
					UnitBrief = item.UnitRow.Brief
				};
				itemSource.Add(item2);
				num2 = itemSource.Count - 1;
			}
			itemSource[num2].Quantity += num;
			return num2;
		}
	}

	public class MTRevaluationRetailSpecificationRow : DocumentGridRow
	{
		[ObservableProperty]
		private string mTCode = string.Empty;

		[ObservableProperty]
		private int mTID = -1;

		[ObservableProperty]
		private string unitCode = string.Empty;

		[ObservableProperty]
		private decimal quantity;

		[ObservableProperty]
		private decimal cost;

		[ObservableProperty]
		private decimal newCost;

		[ObservableProperty]
		private int rowID;

		[ObservableProperty]
		private short rowNum;

		[ObservableProperty]
		private string mTName = string.Empty;

		[ObservableProperty]
		private string fullName = string.Empty;

		[ObservableProperty]
		private string unitBrief = string.Empty;

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
		public decimal Cost
		{
			get
			{
				return cost;
			}
			set
			{
				if (!EqualityComparer<decimal>.Default.Equals(cost, value))
				{
					OnPropertyChanging(__KnownINotifyPropertyChangingArgs.Cost);
					cost = value;
					OnCostChanged(value);
					OnPropertyChanged(__KnownINotifyPropertyChangedArgs.Cost);
				}
			}
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public decimal NewCost
		{
			get
			{
				return newCost;
			}
			set
			{
				if (!EqualityComparer<decimal>.Default.Equals(newCost, value))
				{
					OnPropertyChanging(__KnownINotifyPropertyChangingArgs.NewCost);
					newCost = value;
					OnNewCostChanged(value);
					OnPropertyChanged(__KnownINotifyPropertyChangedArgs.NewCost);
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
		public string FullName
		{
			get
			{
				return fullName;
			}
			[MemberNotNull("fullName")]
			set
			{
				if (!EqualityComparer<string>.Default.Equals(fullName, value))
				{
					OnPropertyChanging(__KnownINotifyPropertyChangingArgs.FullName);
					fullName = value;
					OnPropertyChanged(__KnownINotifyPropertyChangedArgs.FullName);
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

		static MTRevaluationRetailSpecificationRow()
		{
			DocumentGridRow.RegisterPropertyValidators(typeof(MTRevaluationRetailSpecificationRow), cacheValidationResults: true, new Dictionary<string, Action<DocumentGridRow, DevExpress.XtraEditors.DXErrorProvider.ErrorInfo>>
			{
				{
					"MTCode",
					(DocumentGridRow row, DevExpress.XtraEditors.DXErrorProvider.ErrorInfo info) =>
					{
						ValidateGoodCode((MTRevaluationRetailSpecificationRow)row, info);
					}
				},
				{
					"UnitCode",
					(DocumentGridRow row, DevExpress.XtraEditors.DXErrorProvider.ErrorInfo info) =>
					{
						ValidateUnitCode((MTRevaluationRetailSpecificationRow)row, info);
					}
				}
			});
		}

		private static void ValidateGoodCode(MTRevaluationRetailSpecificationRow row, DevExpress.XtraEditors.DXErrorProvider.ErrorInfo info)
		{
			TradePropertyValidator.ValidateMaterialCode(row.MTCode, info, TradeAllDocDefinitions.MTRevaluationRetail.Grids["MTList"].Columns["MTCode"]);
		}

		private static void ValidateUnitCode(MTRevaluationRetailSpecificationRow row, DevExpress.XtraEditors.DXErrorProvider.ErrorInfo info)
		{
			TradePropertyValidator.ValidateGoodUnitCode(row.UnitCode, row.MTCode, info, TradeAllDocDefinitions.MTRevaluationRetail.Grids["MTList"].Columns["UnitCode"]);
		}

		public MTRevaluationRetailSpecificationRow()
		{
		}

		public MTRevaluationRetailSpecificationRow(MTRevaluationRetailWindow docWindow)
			: base(docWindow)
		{
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		private void OnMTCodeChanged(string value)
		{
			ResetForceValidationBit("MTCode", "UnitCode");
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

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		private void OnCostChanged(decimal value)
		{
			ResetForceValidationBit("Cost");
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		private void OnNewCostChanged(decimal value)
		{
			ResetForceValidationBit("NewCost");
		}
	}

	private DropDownCommentControl _baseDocumentControl;

	private DropDownControl _storage;

	private TextEditExt _barCode;

	private Button _codeEnter;

	private DropDownControl _currCode;

	private DropDownControl _priceType;

	private ComboBoxEditExt _comment;

	private DropDownControl _owner;

	private BarEditItemExt _loadMTList;

	private BarEditItemExt _newPriceFormation;

	private DocumentGridControl _mtRevaluationRetailGrid;

	private TableView _mtRevaluationRetailTableView;

	private BarEditItemExt _filteredSearch;

	private ObservableCollection<MTRevaluationRetailSpecificationRow> _mtRevaluationRetailList = new ObservableCollection<MTRevaluationRetailSpecificationRow>();

	private DocumentGridColumn _mtCodeCol;

	private DocumentGridColumn _mtNameCol;

	private DocumentGridColumn _unitBriefCol;

	private DocumentGridColumn _quantityCol;

	private DocumentGridColumn _oldCostCol;

	private DocumentGridColumn _newCostCol;

	private bool _isPriceTypeWithVAT;

	private int _maxRowID;

	private Guid _baseISN = Guid.Empty;

	private TradeDocument _baseDocument;

	private ExchangeRate _exchangeRate = new ExchangeRate();

	private BarSubItem _barSubItemJournals = new BarSubItem();

	private BarSubItem _barSubItemActions = new BarSubItem();

	private ReportBarButtonItem _barButtonJumpTo;

	private BarButtonItem _barButtonPrintLabel;

	private BarButtonItem _barButtonImportFromFile;

	private ReportBarButtonItem _barButtonGoodsRetailOperationJournal;

	protected override double WidthValue => 1240.0;

	protected override double HeightValue => 440.0;

	protected override double MinWidthValue => 600.0;

	protected override double MinHeightValue => 250.0;

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
			if (((IComparable)value).CompareTo((object?)_baseISN) == 0)
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
			if (ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Trade.Common.Resources.FillUsingBaseQ, ArmSoft.SME.Trade.Common.Resources.ProgramName, MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.Yes) == MessageBoxResult.Yes)
			{
				CreateBasedOnDoc();
			}
		}
	}

	public MTRevaluationRetailWindow()
	{
	}

	public MTRevaluationRetailWindow(MTRevaluationRetail doc)
		: base(doc)
	{
	}

	protected override void ConstructUI()
	{
		base.ConstructUI();
		LayoutGroup container = AddLayoutGroup(null, LayoutGroupView.Group, Orientation.Horizontal);
		_date = AddDateControl(container, base.Definition.Fields["DocDate"]);
		_date.Margin = new Thickness(0.0, 0.0, 10.0, 0.0);
		_docNum = AddButtonEditControl(container, Definition.Fields["DocNum"], 1);
		_docNum.AllowMagicButton = true;
		_docNum.MagicToolTip = ArmSoft.SME.Common.Resources.DocNextNumbers;
		_docNum.MagicClick += new SafeDocumentEventHandler<MagicClickEventArgs>(DocNum_MagicClick, this).Handler;
		_docNum.EditValueChanged += new SafeDocumentEventHandler<EditValueChangedEventArgs>(DocNum_EditValueChanged, this).Handler;
		DocumentsJournal idv = new DocumentsJournal(this, ReportTypesSE.AllDocument, ((TradeDocDefinition)Definition).BaseDocTypes)
		{
			DocType = ((TradeDocDefinition)Definition).BaseDocTypes.First().ToString(),
			DocState = DocumentStateSE.DraftOrProcessed.ValueUI
		};
		_baseDocumentControl = AddDropDownCommentControl(null, ArmSoft.SME.Common.Resources.BaseDoc, isRequired: false, idv, 50, HorizontalAlignment.Stretch, "", "BaseDocument");
		_baseDocumentControl.ShowClearButton = true;
		_baseDocumentControl.ShowSearchButton = true;
		_baseDocumentControl.IsTextEditable = false;
		_baseDocumentControl.ClickDropDown += new SafeDocumentEventHandler<DropDownControlEventArgs>(BaseDocumentControl_ClickDropDown, this).Handler;
		_baseDocumentControl.EditKeyValueChanged += new SafeDocumentEventHandler<EditKeyValueChangedEventArgs>(BaseDocument_EditKeyValueChanged, this).Handler;
		_baseDocumentControl.BtnClearClicked += new SafeDocumentEventHandler<EditKeyValueChangedEventArgs>(BaseDocumentDelete_Click, this).Handler;
		_baseDocumentControl.BtnSearchClicked += new SafeDocumentEventHandler<EditKeyValueChangedEventArgs>(BaseDocumentSearch_Click, this).Handler;
		_storage = AddDropDownControl(null, Definition.Fields["Storage"], new Storages(true));
		_storage.EditValueChanged += new SafeDocumentEventHandler<EditValueChangedEventArgs>(Storage_EditValueChanged, this).Handler;
		LayoutGroup container2 = AddLayoutGroup(null, LayoutGroupView.Group, Orientation.Horizontal);
		_barCode = AddStringControl(container2, ArmSoft.SME.Common.Resources.BarCode + " (F2)", isRequired: false, 20, HorizontalAlignment.Stretch, "", "BarCode");
		_barCode.NullText = ArmSoft.SME.Trade.Common.Resources.BarCodeAndCodeInputNullText;
		_barCode.KeyDown += new SafeDocumentEventHandler<KeyEventArgs>(BarCode_KeyDown, this).Handler;
		string text = ArmSoft.SME.Trade.Common.Resources.cmdCodeEnter + " (F8)";
		_codeEnter = AddButton(container2, null, text, null, text, isTabStop: true);
		_codeEnter.Margin = new Thickness(0.0, 0.0, 18.0, 0.0);
		_codeEnter.Click += new SafeDocumentEventHandler<RoutedEventArgs>(CodeEnter_Click, this).Handler;
		DXTabControl tc = AddTabControl(null);
		LayoutGroup container3 = AddTabbedGroup(tc, ArmSoft.SME.Common.Resources.General, "General");
		_currCode = AddDropDownControl(container3, Definition.Fields["CurrCode"], new Currency(shortMode: true), allowAdditionButtons: false, fieldWithCaption: true, HorizontalAlignment.Stretch);
		_priceType = AddDropDownControl(container3, Definition.Fields["PriceType"], new PriceTypes("AMD"), allowAdditionButtons: false, fieldWithCaption: true, HorizontalAlignment.Stretch);
		_priceType.EditValueChanged += new SafeDocumentEventHandler<EditValueChangedEventArgs>(PriceType_EditValueChanged, this).Handler;
		_comment = AddComboBoxControl(container3, Definition.Fields["Comment"], TradeSettings.TradeCacheService.StandardComments.GetCaptionsList(), HorizontalAlignment.Stretch);
		_owner = AddDropDownControl(container3, Definition.Fields["Owner"], new UsersFilter());
		_owner.CodeWidth = 12;
		_owner.ShowComment = false;
		_owner.InnerCodeEdit.Width = _owner.InnerCodeEdit.MinWidth;
		if (!Settings.LoginService.IsAdmin && Settings.ParametersService.DenyAccessToOtherOwnerDocs())
		{
			((LayoutItemWithRequired)(object)_owner.Parent).Visibility = Visibility.Collapsed;
		}
		_mtRevaluationRetailGrid = AddGrid(container3, Definition.Grids["MTList"], HorizontalAlignment.Stretch, VerticalAlignment.Stretch);
		_mtRevaluationRetailGrid.IsRequired = true;
		_mtRevaluationRetailGrid.AllowMoveUp = true;
		_mtRevaluationRetailGrid.AllowMoveDown = true;
		_mtRevaluationRetailGrid.AllowDuplicate = false;
		_mtRevaluationRetailGrid.AddingRow += new SafeDocumentEventHandler<DocumentGridAddingRowEventArgs>(MTRevaluationRetailGrid_AddingRow, this).Handler;
		_mtRevaluationRetailGrid.MovingRowUp += new SafeDocumentEventHandler<DocumentGridMovingRowUpOrDownEventArgs>(MTRevaluationRetailGrid_MovingRowUp, this).Handler;
		_mtRevaluationRetailGrid.MovingRowDown += new SafeDocumentEventHandler<DocumentGridMovingRowUpOrDownEventArgs>(MTRevaluationRetailGrid_MovingRowDown, this).Handler;
		_mtRevaluationRetailGrid.DeletedRow += new SafeDocumentEventHandler<EventArgs>(MTRevaluationRetailGrid_DeletedRow, this).Handler;
		_mtRevaluationRetailGrid.ClickDropDown += new SafeDocumentEventHandler<DocumentGridClickDropDownEventArgs>(MTRevaluationRetailGrid_ClickDropDown, this).Handler;
		_mtRevaluationRetailTableView = _mtRevaluationRetailGrid.View;
		_mtRevaluationRetailTableView.CellValueChanged += new SafeDocumentEventHandler<CellValueChangedEventArgs>(MTRevaluationRetailTableView_CellValueChanged, this).Handler;
		_loadMTList = _mtRevaluationRetailGrid.AddButton("fill_down_16.svg", ArmSoft.SME.Common.Resources.ButtonLoad, "", 90.0);
		_loadMTList.Click += new SafeDocumentEventHandler<RoutedEventArgs>(LoadMTList_Click, this).Handler;
		_newPriceFormation = _mtRevaluationRetailGrid.AddButton("refresh_16.svg", ArmSoft.SME.Trade.Common.Resources.ChangeNewPrices, "", 150.0);
		_newPriceFormation.Click += new SafeDocumentEventHandler<RoutedEventArgs>(NewPriceFormation_Click, this).Handler;
		_filteredSearch = _mtRevaluationRetailGrid.AddButton(ArmSoft.SME.Trade.Common.Resources.GroupAddition, ArmSoft.SME.Trade.Common.Resources.GroupAddition, ArmSoft.SME.Trade.Common.Resources.GroupAddition, 130.0);
		_filteredSearch.Click += new SafeDocumentEventHandler<RoutedEventArgs>(FilteredSearch_Click, this).Handler;
		_mtRevaluationRetailGrid.AddRowIndicator(35);
		_mtCodeCol = _mtRevaluationRetailGrid.AddDropDownColumn(Definition.Grids["MTList"].Columns["MTCode"], new ArmSoft.SME.Trade.UI.DropDown.GoodsFilter(shortMode: true, ItemTypesSE.Goods.Value, "", isShowOnlyInPriceList: true), allowAdditionButtons: true);
		_mtCodeCol.Width = 125.0;
		_mtNameCol = _mtRevaluationRetailGrid.AddStringColumn(Definition.Grids["MTList"].Columns["MTName"]);
		_mtNameCol.AllowEditing = DefaultBoolean.False;
		_unitBriefCol = _mtRevaluationRetailGrid.AddDropDownColumn(Definition.Grids["MTList"].Columns["UnitBrief"], trimValue: false, GlyphKind.Down);
		((ButtonEditSettings)_unitBriefCol.EditSettings).IsTextEditable = false;
		_quantityCol = _mtRevaluationRetailGrid.AddNumberColumn(Definition.Grids["MTList"].Columns["Quantity"], showTotal: true);
		_quantityCol.AllowEditing = DefaultBoolean.Default;
		_oldCostCol = _mtRevaluationRetailGrid.AddNumberColumn(Definition.Grids["MTList"].Columns["Cost"], showTotal: false, useCurrencyFormat: true, isOwnProcessingClickDropDown: true);
		_oldCostCol.AllowEditing = DefaultBoolean.False;
		_oldCostCol.AllowFocus = true;
		_newCostCol = _mtRevaluationRetailGrid.AddNumberColumn(Definition.Grids["MTList"].Columns["NewCost"]);
		_mtRevaluationRetailList = new ObservableCollection<MTRevaluationRetailSpecificationRow>();
		foreach (GridColumn column in _mtRevaluationRetailGrid.Grid.Columns)
		{
			column.AllowSorting = DefaultBoolean.True;
			column.AllowColumnFiltering = DefaultBoolean.True;
			column.FilterPopupMode = FilterPopupMode.CheckedList;
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

	protected override void OnNew()
	{
		base.OnNew();
		_date.EditValue = Settings.HostingEnvironment.CurrentDate;
		_owner.Code = Settings.LoginService.UserName;
		if (_viewMode == WindowViewMode.Copy)
		{
			return;
		}
		if (_baseISN == Guid.Empty)
		{
			string text = Settings.ParametersService.DefaultStorage();
			if (!string.IsNullOrWhiteSpace(text))
			{
				StoragesCacheRow item = TradeSettings.TradeCacheService.Storage.GetItem(text);
				if (item != null && item.Retail && TradeSettings.CreateTradeClient().CommonServices.IsStorageAvailable(text))
				{
					_storage.Code = text;
				}
			}
		}
		else if (BaseDocument != null)
		{
			CreateBasedOnDoc();
		}
	}

	protected override void OnNewAfterSave()
	{
		base.OnNewAfterSave();
		_mtRevaluationRetailList.Clear();
		_priceType.Code = "";
		_maxRowID = 0;
		_owner.Code = Settings.LoginService.UserName;
	}

	protected override void OnEdit()
	{
		base.OnEdit();
		if (string.IsNullOrWhiteSpace(_docNum.Text))
		{
			SetNextDocumentNumber();
		}
		if (!_date.EditValue.HasValue)
		{
			_date.EditValue = Settings.HostingEnvironment.CurrentDate;
		}
		_setFocusOnActivation = _docNum;
	}

	protected override void SetControlsReadonly()
	{
		base.SetControlsReadonly();
		if (_viewMode != WindowViewMode.View)
		{
			_currCode.IsReadOnly = true;
			bool flag = State == DocumentStateSE.Draft.Value || State == DocumentStateSE.Deleted.Value;
			bool flag2 = _viewMode == WindowViewMode.Edit && !flag;
			_date.IsReadOnly = flag2;
			_baseDocumentControl.IsReadOnly = flag2;
			_storage.IsReadOnly = _mtRevaluationRetailList.Count > 0 && _viewMode != WindowViewMode.NewAfterSave;
			_barCode.IsReadOnly = flag2;
			_codeEnter.IsEnabled = !flag2;
			_priceType.IsReadOnly = flag2;
			_mtRevaluationRetailGrid.IsReadOnly = flag2;
		}
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
		_maxRowID = PO.MaxRowID;
		_mtRevaluationRetailList.Clear();
		foreach (MTRevaluationRetail.MTRevaluationRetailSpecificationRow mT in PO.MTList)
		{
			MTRevaluationRetailSpecificationRow mTRevaluationRetailSpecificationRow = new MTRevaluationRetailSpecificationRow();
			mTRevaluationRetailSpecificationRow.MTCode = mT.MTCode;
			mTRevaluationRetailSpecificationRow.MTID = mT.MTID;
			mTRevaluationRetailSpecificationRow.UnitCode = mT.UnitCode;
			mTRevaluationRetailSpecificationRow.Quantity = mT.Quantity;
			mTRevaluationRetailSpecificationRow.Cost = mT.Cost;
			mTRevaluationRetailSpecificationRow.NewCost = mT.NewCost;
			mTRevaluationRetailSpecificationRow.RowID = mT.RowID;
			mTRevaluationRetailSpecificationRow.RowNum = mT.RowNum;
			mTRevaluationRetailSpecificationRow.MTName = mT.MTName;
			mTRevaluationRetailSpecificationRow.FullName = mT.FullName;
			mTRevaluationRetailSpecificationRow.UnitBrief = mT.UnitBrief;
			_mtRevaluationRetailList.Add(mTRevaluationRetailSpecificationRow);
		}
		_mtRevaluationRetailGrid.ItemsSource = _mtRevaluationRetailList;
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
		Ribbon.ToolbarItemLinks.Add(_barButtonPrintLabel.ConvertForToolbar("t_action_16.svg"));
		Ribbon.ToolbarItemLinks.Add(_barButtonImportFromFile.ConvertForToolbar("t_import_16.svg"));
		Ribbon.ToolbarItemLinks.Add(_barSubItemJournals.ConvertForToolbar("t_report_16.svg"));
	}

	protected override void AddItemsToRibbon()
	{
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_011c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0122: Unknown result type (might be due to invalid IL or missing references)
		//IL_0138: Unknown result type (might be due to invalid IL or missing references)
		//IL_013e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0226: Unknown result type (might be due to invalid IL or missing references)
		//IL_022c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0242: Unknown result type (might be due to invalid IL or missing references)
		//IL_0248: Unknown result type (might be due to invalid IL or missing references)
		base.AddItemsToRibbon();
		CreateRibbonPageGroupActions();
		CreateRibbonPageGroupReports();
		_barSubItemJournals.Content = "";
		_barSubItemJournals.Glyph = ImageService.GetImage("report_16.svg");
		_barSubItemJournals.LargeGlyph = ImageService.GetImage("report_32.svg");
		_barButtonJumpTo = new ReportBarButtonItem(DataProviderTypeSE.AllDocumentsJournal, showDialog: false, ArmSoft.SME.Common.Resources.JumpTo, null, "doc_JumpTo_16.svg", "doc_JumpTo_32.svg");
		_barButtonJumpTo.AddInBarSubItem(ref _ribbonPageGroupActions, ref _barSubItemActions);
		_barSubItemActions.Content = "";
		_barSubItemActions.Glyph = ImageService.GetImage("action_16.svg");
		_barSubItemActions.LargeGlyph = ImageService.GetImage("action_32.svg");
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
		BarItemLink barItemLink = _barButtonPrintLabel.CreateLink();
		barItemLink.MergeType = BarItemMergeType.Replace;
		_barSubItemActions.ItemLinks.Add(barItemLink);
		_barManager.Items.Add(_barSubItemActions);
		Ribbon.ToolbarItemLinks.Add(_barSubItemActions.CreateLink());
		_barButtonImportFromFile = new BarButtonItem
		{
			Content = ArmSoft.SME.Trade.Common.Resources.Import,
			KeyGesture = new KeyGesture((Key)52, (ModifierKeys)2),
			Glyph = ImageService.GetImage("Import_16.svg"),
			LargeGlyph = ImageService.GetImage("Import_32.svg")
		};
		_barButtonImportFromFile.ItemClick += new SafeDocumentEventHandler<ItemClickEventArgs>(BarButtonImportFromFile_ItemClick, this).Handler;
		_barManager.Items.Add(_barButtonImportFromFile);
		_ribbonPageGroupActions.ItemLinks.Add(_barButtonImportFromFile.CreateLink());
		Ribbon.ToolbarItemLinks.Add(_barButtonImportFromFile.CreateLink());
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
		span[0] = new PropertyWithValue("DocTypeParam", PO.POType.ToString());
		span[1] = new PropertyWithValue("StartDate", PO.DocDate.ToString());
		span[2] = new PropertyWithValue("EndDate", PO.DocDate.ToString());
		span[3] = new PropertyWithValue("SelectedRowDocISN", PO.ISN.ToString());
		barButtonJumpTo.PropertyWithValues = list;
		ReportBarButtonItem barButtonGoodsRetailOperationJournal = _barButtonGoodsRetailOperationJournal;
		num = 2;
		List<PropertyWithValue> list2 = new List<PropertyWithValue>(num);
		CollectionsMarshal.SetCount(list2, num);
		Span<PropertyWithValue> span2 = CollectionsMarshal.AsSpan(list2);
		span2[0] = new PropertyWithValue("DocISN", PO.ISN.ToString());
		span2[1] = new PropertyWithValue("AdditionalCaption", GetReportsAdditionalCaption());
		barButtonGoodsRetailOperationJournal.PropertyWithValues = list2;
	}

	protected void BarButtonPrintLabel_ItemClick(object sender, ItemClickEventArgs e)
	{
		if (_mtRevaluationRetailList.Count > 0)
		{
			FillDocument(new StoreRequest());
			new PrintLabelDialog(this, PO).Invoke();
		}
	}

	private void BarButtonImportFromFile_ItemClick(object sender, ItemClickEventArgs e)
	{
		if (_date.IsReadOnly || !_date.DoValidate())
		{
			return;
		}
		if (string.IsNullOrWhiteSpace(_storage.Code))
		{
			throw new ASException(ArmSoft.SME.Trade.Common.Resources.ErrorRetailStorageFromFile);
		}
		if (_mtRevaluationRetailList.Count <= 0 || ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Trade.Common.Resources.LoadMaterialsList, ArmSoft.SME.Trade.Common.Resources.ProgramName, MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No) != MessageBoxResult.No)
		{
			_mtRevaluationRetailList.Clear();
			TerminalHelper.TerminalImporter terminalImporter = new TerminalHelper.TerminalImporter(this);
			if (terminalImporter.ImportFromFile((decimal quantity, MaterialCacheRow goodRow, MaterialCacheRow.BarCodeRow goodBarcodeRow) =>
			{
				AddMTToList(goodRow, quantity);
			}))
			{
				AddMissingAvailabilityRows();
				terminalImporter.ShowReport();
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

	private void ScannerAdd()
	{
		try
		{
			_barCode.DoValidate();
			ValidateEmptyDateAndStorage();
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
			decimal qty = 1m;
			string unitCode = string.Empty;
			string unitBrief = string.Empty;
			string barCode = _barCode.Text.Trim();
			MaterialCacheRow materialCacheRow = DocumentHelpers.TryGetItemByBarCode(ref barCode, out qty, out unitCode, out unitBrief);
			if (materialCacheRow == null || materialCacheRow.Type == ItemTypesSE.Services.Value)
			{
				ControlsHelper.SetFocus(_barCode);
				throw new ASException(string.Format(ArmSoft.SME.Common.Resources.ErrBarCodeNotExist, _barCode.Text.Trim()));
			}
			AddMTToList(materialCacheRow, qty, unitCode, unitBrief);
			_barCode.Text = string.Empty;
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	private void AddMTToList(MaterialCacheRow goodsRow, decimal quantity, string unitCode = "", string unitBrief = "")
	{
		if (Settings.ParametersService.ScannerMode() == "3")
		{
			AddMTToListNewRow(goodsRow, quantity, string.IsNullOrWhiteSpace(unitCode) ? goodsRow.Unit : unitCode, string.IsNullOrWhiteSpace(unitBrief) ? goodsRow.UnitRow.Brief : unitBrief);
		}
		else
		{
			AddNewRowOrMergeInner(goodsRow, Settings.ParametersService.ScannerMode() == "2", quantity, string.IsNullOrWhiteSpace(unitCode) ? goodsRow.Unit : unitCode, string.IsNullOrWhiteSpace(unitBrief) ? goodsRow.UnitRow.Brief : unitBrief);
		}
	}

	private void AddNewRowOrMergeInner(MaterialCacheRow goodsRow, bool forFocusedRow, decimal quantity, string unitCode, string unitBrief)
	{
		MTRevaluationRetail.MTRevaluationRetailSpecificationRow mTRevaluationRetailSpecificationRow = null;
		if (forFocusedRow && _mtRevaluationRetailList.Count != 0)
		{
			mTRevaluationRetailSpecificationRow = (MTRevaluationRetail.MTRevaluationRetailSpecificationRow)_mtRevaluationRetailTableView.Grid.CurrentItem;
			if (mTRevaluationRetailSpecificationRow == null)
			{
				return;
			}
		}
		for (int i = 0; i <= _mtRevaluationRetailList.Count - 1; i++)
		{
			if ((forFocusedRow && mTRevaluationRetailSpecificationRow.RowID != _mtRevaluationRetailList[i].RowID) || goodsRow.Id != _mtRevaluationRetailList[i].MTID || !(unitCode == _mtRevaluationRetailList[i].UnitCode))
			{
				continue;
			}
			MaterialCacheRow.QntUnitRow qntUnit = TradeSettings.TradeCacheService.Material.GetQntUnit(_mtRevaluationRetailList[i].MTCode, _mtRevaluationRetailList[i].UnitCode);
			decimal num = 0m;
			if (qntUnit != null)
			{
				MTRetailQtyAndPriceDataProvider.Param param = new MTRetailQtyAndPriceDataProvider.Param
				{
					MTCode = _mtRevaluationRetailList[i].MTCode,
					UnitCoef = qntUnit.Coef,
					Storage = _storage.Code,
					Date = _date.EditValue.Value
				};
				List<MTRetailQtyAndPriceDataProvider.DataRow> rows = new MTRetailQtyAndPriceDataProvider(TradeSettings.CreateTradeClient()).GetRows(param);
				if (rows.Count > 0)
				{
					num = rows.First().Price;
				}
			}
			if (num == _mtRevaluationRetailList[i].Cost)
			{
				FillMTRevaluationRetailGridRow(_mtRevaluationRetailList[i], goodsRow, unitCode, unitBrief, quantity, num);
				int rowHandleByListIndex = _mtRevaluationRetailGrid.Grid.GetRowHandleByListIndex(i);
				_mtRevaluationRetailGrid.Grid.RefreshRow(rowHandleByListIndex);
				POWindow<MTRevaluationRetail>.SelectCellInGrid(_mtRevaluationRetailTableView, _mtRevaluationRetailGrid, rowHandleByListIndex, "Quantity");
				return;
			}
		}
		AddMTToListNewRow(goodsRow, quantity, unitCode, unitBrief);
	}

	private void AddMTToListNewRow(MaterialCacheRow goodsRow, decimal quantity, string unitCode, string unitBrief)
	{
		MTRevaluationRetailSpecificationRow mTRevaluationRetailSpecificationRow = new MTRevaluationRetailSpecificationRow();
		FillMTRevaluationRetailGridRow(mTRevaluationRetailSpecificationRow, goodsRow, unitCode, unitBrief, quantity);
		mTRevaluationRetailSpecificationRow.RowID = _maxRowID;
		_mtRevaluationRetailList.Add(mTRevaluationRetailSpecificationRow);
		_maxRowID++;
		int rowHandleByListIndex = _mtRevaluationRetailGrid.Grid.GetRowHandleByListIndex(_mtRevaluationRetailList.Count - 1);
		_mtRevaluationRetailGrid.Grid.RefreshRow(rowHandleByListIndex);
		POWindow<MTRevaluationRetail>.SelectCellInGrid(_mtRevaluationRetailTableView, _mtRevaluationRetailGrid, rowHandleByListIndex, "Quantity");
		_mtRevaluationRetailGrid.ConfigureButtons();
	}

	private void AddMissingAvailabilityRows()
	{
		foreach (GoodsRetailAvailabilityDataProvider.DataRow item in from p in AvailabilityRows()
			where !_mtRevaluationRetailList.Any((MTRevaluationRetailSpecificationRow q) => q.MTID == p.GoodID && q.UnitCode == p.Unit)
			select p)
		{
			AddNewRow(item);
		}
	}

	private List<GoodsRetailAvailabilityDataProvider.DataRow> AvailabilityRows()
	{
		GoodsRetailAvailabilityDataProvider.Param param = new GoodsRetailAvailabilityDataProvider.Param
		{
			RemDate = _date.EditValue.Value,
			Storage = _storage.Code
		};
		return new GoodsRetailAvailabilityDataProvider(TradeSettings.CreateTradeClient()).GetRows(param);
	}

	private void AddNewRow(GoodsRetailAvailabilityDataProvider.DataRow sourceRow)
	{
		MTRevaluationRetailSpecificationRow mTRevaluationRetailSpecificationRow = new MTRevaluationRetailSpecificationRow();
		mTRevaluationRetailSpecificationRow.MTCode = sourceRow.GoodCode;
		mTRevaluationRetailSpecificationRow.MTID = sourceRow.GoodID;
		mTRevaluationRetailSpecificationRow.MTName = sourceRow.Caption;
		mTRevaluationRetailSpecificationRow.UnitCode = sourceRow.Unit;
		QntUnitCacheRow item = TradeSettings.TradeCacheService.QntUnit.GetItem(mTRevaluationRetailSpecificationRow.UnitCode);
		mTRevaluationRetailSpecificationRow.UnitBrief = ((item == null) ? "" : item.Brief);
		mTRevaluationRetailSpecificationRow.Quantity = sourceRow.Quantity;
		MaterialCacheRow.QntUnitRow qntUnit = TradeSettings.TradeCacheService.Material.GetQntUnit(mTRevaluationRetailSpecificationRow.MTCode, mTRevaluationRetailSpecificationRow.UnitBrief);
		if (qntUnit != null)
		{
			MTRetailQtyAndPriceDataProvider.Param param = new MTRetailQtyAndPriceDataProvider.Param
			{
				MTCode = mTRevaluationRetailSpecificationRow.MTCode,
				UnitCoef = qntUnit.Coef,
				Storage = _storage.Code,
				Date = _date.EditValue.Value
			};
			List<MTRetailQtyAndPriceDataProvider.DataRow> rows = new MTRetailQtyAndPriceDataProvider(TradeSettings.CreateTradeClient()).GetRows(param);
			if (rows.Count > 0)
			{
				mTRevaluationRetailSpecificationRow.Cost = rows[0].Price;
			}
		}
		mTRevaluationRetailSpecificationRow.NewCost = PriceHelper.GetPriceForGivenPriceListAndVATType(_priceType.Code, "2", mTRevaluationRetailSpecificationRow.MTID, mTRevaluationRetailSpecificationRow.UnitCode, _date.EditValue.Value, _currCode.Code);
		mTRevaluationRetailSpecificationRow.RowID = _maxRowID;
		_maxRowID++;
		_mtRevaluationRetailList.Add(mTRevaluationRetailSpecificationRow);
	}

	private void FillMTRevaluationRetailGridRow(MTRevaluationRetailSpecificationRow mtRevalRetGridRow, MaterialCacheRow goodsRow, string unitCode, string unitBrief, decimal quantity = 1m, decimal cost = -1m)
	{
		mtRevalRetGridRow.UnitCode = unitCode;
		mtRevalRetGridRow.UnitBrief = unitBrief;
		if (goodsRow != null)
		{
			mtRevalRetGridRow.MTID = goodsRow.Id;
			mtRevalRetGridRow.MTCode = goodsRow.MTCode;
			mtRevalRetGridRow.MTName = goodsRow.Caption;
			if (cost == -1m)
			{
				MaterialCacheRow.QntUnitRow qntUnit = TradeSettings.TradeCacheService.Material.GetQntUnit(mtRevalRetGridRow.MTCode, mtRevalRetGridRow.UnitCode);
				if (qntUnit != null)
				{
					MTRetailQtyAndPriceDataProvider.Param param = new MTRetailQtyAndPriceDataProvider.Param
					{
						MTCode = mtRevalRetGridRow.MTCode,
						UnitCoef = qntUnit.Coef,
						Storage = _storage.Code,
						Date = _date.EditValue.Value
					};
					List<MTRetailQtyAndPriceDataProvider.DataRow> rows = new MTRetailQtyAndPriceDataProvider(TradeSettings.CreateTradeClient()).GetRows(param);
					if (rows.Count > 0)
					{
						mtRevalRetGridRow.Cost = rows.First().Price;
					}
				}
			}
			mtRevalRetGridRow.NewCost = PriceHelper.GetPriceForGivenPriceListAndVATType(_priceType.Code, "2", mtRevalRetGridRow.MTID, mtRevalRetGridRow.UnitCode, _date.EditValue.Value, _currCode.Code);
		}
		mtRevalRetGridRow.Quantity += quantity;
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
			string text = _barCode.Text.Trim();
			if (string.IsNullOrWhiteSpace(text))
			{
				if (!_date.DoValidate() || !_date.EditValue.HasValue)
				{
					ControlsHelper.SetFocus(_date);
					throw new ASException(ArmSoft.SME.Common.Resources.ValidateEmptyDate);
				}
				ArmSoft.SME.Trade.UI.DropDown.GoodsFilter goodsFilter = new ArmSoft.SME.Trade.UI.DropDown.GoodsFilter(shortMode: true, ItemTypesSE.Goods.Value);
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
			AddMTToList(item, 1m);
			_barCode.Text = "";
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	protected override void SetReportBarButtonsEnable(bool isEnabled)
	{
		base.SetReportBarButtonsEnable(isEnabled);
		_barButtonJumpTo.IsEnabled = isEnabled;
		_barButtonGoodsRetailOperationJournal.IsEnabled = isEnabled;
	}

	private void Storage_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		if (_storage.DoValidate())
		{
			CheckStorageAccess(_storage.Code);
		}
	}

	private void CheckStorageAccess(string storage)
	{
		try
		{
			if (!TradeSettings.CreateTradeClient().CommonServices.IsStorageAvailable(storage))
			{
				_storage.Code = "";
				throw new ASException(string.Format(ArmSoft.SME.Trade.Common.Resources.StorageNotAvailForUser, Settings.LoginService.UserName, storage));
			}
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	private static decimal GetNewPriceForGridRow(List<PricesDataProvider.DataRow> priceList, int mtID, string Unit, decimal exchRate, decimal exchBase)
	{
		IEnumerable<PricesDataProvider.DataRow> source = priceList.Where((PricesDataProvider.DataRow p) => p.MTID == mtID && p.QntUnit == Unit);
		if (source.Any())
		{
			return (source.LastOrDefault().Price * exchRate / exchBase).RoundAs(2);
		}
		return 0m;
	}

	private void PriceType_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		try
		{
			if (!_priceType.DoValidate() || string.IsNullOrWhiteSpace(_priceType.Code) || !_date.DoValidate())
			{
				return;
			}
			string text = "";
			PriceTypesDataProvider.Param param = new PriceTypesDataProvider.Param();
			param.Code = _priceType.Code;
			List<PriceTypesDataProvider.DataRow> rows = new PriceTypesDataProvider(TradeSettings.CreateTradeClient()).GetRows(param);
			if (rows.Count == 0)
			{
				return;
			}
			text = rows[0].CurrCode;
			_isPriceTypeWithVAT = rows[0].VAT;
			if (text != "AMD")
			{
				TradeSettings.CreateTradeClient().CommonServices.GetExchangeRates(text, _date.EditValue.Value);
			}
			else
			{
				_exchangeRate.Rate = 1m;
				_exchangeRate.Base = 1m;
			}
			if (_mtRevaluationRetailList.Count <= 0)
			{
				return;
			}
			if (ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Trade.Common.Resources.ValidateChangePrices, Settings.ProgramName, MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No) == MessageBoxResult.Yes)
			{
				Settings.ParametersService.VAT();
				PricesDataProvider.Param param2 = new PricesDataProvider.Param
				{
					EndDate = _date.EditValue.Value,
					PriceType = _priceType.Code,
					MTCode = string.Join(",", _mtRevaluationRetailList.Select((MTRevaluationRetailSpecificationRow x) => x.MTCode))
				};
				List<PricesDataProvider.DataRow> rows2 = new PricesDataProvider(TradeSettings.CreateTradeClient()).GetRows(param2, withoutAccessCheck: true);
				if (_isPriceTypeWithVAT)
				{
					foreach (MTRevaluationRetailSpecificationRow mtRevaluationRetail in _mtRevaluationRetailList)
					{
						mtRevaluationRetail.NewCost = GetNewPriceForGridRow(rows2, mtRevaluationRetail.MTID, mtRevaluationRetail.UnitCode, _exchangeRate.Rate, _exchangeRate.Base);
					}
				}
				else
				{
					decimal num = 0m;
					foreach (MTRevaluationRetailSpecificationRow mtRevaluationRetail2 in _mtRevaluationRetailList)
					{
						num = GetNewPriceForGridRow(rows2, mtRevaluationRetail2.MTID, mtRevaluationRetail2.UnitCode, _exchangeRate.Rate, _exchangeRate.Base);
						mtRevaluationRetail2.NewCost = (num + PriceHelper.CalcVATAmountByType(num, "1", itemVAT: true)).RoundAs(2);
					}
				}
			}
			_mtRevaluationRetailGrid.Grid.RefreshData();
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	private void LoadMTList_Click(object sender, RoutedEventArgs e)
	{
		try
		{
			if (_date.IsReadOnly)
			{
				return;
			}
			ValidateEmptyDateAndStorage();
			if (_mtRevaluationRetailList.Count > 0)
			{
				if (ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Trade.Common.Resources.LoadMaterialsList, Settings.ProgramName, MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.Yes) == MessageBoxResult.No)
				{
					return;
				}
				_mtRevaluationRetailList.Clear();
				_maxRowID = 0;
			}
			LoadMTList();
			SetControlsState();
			_mtRevaluationRetailGrid.Grid.RefreshData();
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	private void NewPriceFormation_Click(object sender, RoutedEventArgs e)
	{
		if (_mtRevaluationRetailList.Count > 0 && ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Trade.Common.Resources.ValidateChangePrices, Settings.ProgramName, MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No) == MessageBoxResult.Yes)
		{
			ShowPriceFormationDialog();
		}
	}

	private void MTRevaluationRetailGrid_AddingRow(object sender, DocumentGridAddingRowEventArgs e)
	{
		ValidateEmptyDateAndStorage();
		_mtRevaluationRetailList.Add(new MTRevaluationRetailSpecificationRow
		{
			Quantity = 0m,
			NewCost = 0m,
			Cost = 0m,
			RowID = _maxRowID
		});
		_maxRowID++;
		e.RowNumber = _mtRevaluationRetailList.Count - 1;
		if (_mtRevaluationRetailList.Count > 0)
		{
			_storage.IsReadOnly = true;
		}
	}

	private void MTRevaluationRetailGrid_MovingRowUp(object sender, DocumentGridMovingRowUpOrDownEventArgs e)
	{
		if (_mtRevaluationRetailGrid.Grid.SortInfo.Count > 0 || _mtRevaluationRetailTableView.Grid.VisibleRowCount != _mtRevaluationRetailList.Count)
		{
			throw new ASException(ArmSoft.SME.Common.Resources.NotAllowRowMoveInSortedGrid);
		}
		MTRevaluationRetailSpecificationRow mTRevaluationRetailSpecificationRow = (MTRevaluationRetailSpecificationRow)_mtRevaluationRetailGrid.Grid.GetRow(e.RowHandle);
		if (mTRevaluationRetailSpecificationRow != null)
		{
			_mtRevaluationRetailList.RemoveAt(e.RowHandle);
			_mtRevaluationRetailList.Insert(e.RowHandle - 1, mTRevaluationRetailSpecificationRow);
		}
		_mtRevaluationRetailGrid.Grid.RefreshData();
	}

	private void MTRevaluationRetailGrid_MovingRowDown(object sender, DocumentGridMovingRowUpOrDownEventArgs e)
	{
		if (_mtRevaluationRetailGrid.Grid.SortInfo.Count > 0 || _mtRevaluationRetailTableView.Grid.VisibleRowCount != _mtRevaluationRetailList.Count)
		{
			throw new ASException(ArmSoft.SME.Common.Resources.NotAllowRowMoveInSortedGrid);
		}
		MTRevaluationRetailSpecificationRow mTRevaluationRetailSpecificationRow = (MTRevaluationRetailSpecificationRow)_mtRevaluationRetailGrid.Grid.GetRow(e.RowHandle);
		if (mTRevaluationRetailSpecificationRow != null)
		{
			_mtRevaluationRetailList.RemoveAt(e.RowHandle);
			_mtRevaluationRetailList.Insert(e.RowHandle + 1, mTRevaluationRetailSpecificationRow);
		}
		_mtRevaluationRetailGrid.Grid.RefreshData();
	}

	private void MTRevaluationRetailGrid_DeletedRow(object sender, EventArgs e)
	{
		_storage.IsReadOnly = _mtRevaluationRetailList.Count > 0;
		_currCode.IsReadOnly = _currCode.IsReadOnly;
		_mtRevaluationRetailGrid.Grid.RefreshData();
	}

	private void MTRevaluationRetailGrid_ClickDropDown(object sender, DocumentGridClickDropDownEventArgs e)
	{
		try
		{
			MTRevaluationRetailSpecificationRow mTRevaluationRetailSpecificationRow = (MTRevaluationRetailSpecificationRow)_mtRevaluationRetailGrid.Grid.GetRow(e.RowHandle);
			if (mTRevaluationRetailSpecificationRow == null)
			{
				return;
			}
			ValidateEmptyDateAndStorage();
			if (e.Column == _unitBriefCol)
			{
				MTQntUnits mTQntUnits = new MTQntUnits(mTRevaluationRetailSpecificationRow.MTCode);
				ModalBrowser modalBrowser = new ModalBrowser(mTQntUnits)
				{
					LookUpValue = mTRevaluationRetailSpecificationRow.UnitCode,
					Owner = e.Owner,
					Left = e.Left,
					Top = e.Top
				};
				if (modalBrowser.Show())
				{
					string text = mTQntUnits.ValueMember(modalBrowser.SelectedRowIndex);
					string unitCode = mTRevaluationRetailSpecificationRow.UnitCode;
					if (mTRevaluationRetailSpecificationRow.UnitCode != text)
					{
						QntUnitCacheRow item = TradeSettings.TradeCacheService.QntUnit.GetItem(text);
						if (item != null)
						{
							mTRevaluationRetailSpecificationRow.UnitCode = item.Code;
							e.NewValue = item.Brief;
						}
						if (!string.IsNullOrWhiteSpace(text))
						{
							MaterialCacheRow.QntUnitRow qntUnit = TradeSettings.TradeCacheService.Material.GetQntUnit(mTRevaluationRetailSpecificationRow.MTID, mTRevaluationRetailSpecificationRow.UnitCode);
							MaterialCacheRow.QntUnitRow qntUnit2 = TradeSettings.TradeCacheService.Material.GetQntUnit(mTRevaluationRetailSpecificationRow.MTID, unitCode);
							if (qntUnit != null && qntUnit2 != null)
							{
								decimal coef = qntUnit.Coef;
								decimal coef2 = qntUnit2.Coef;
								MTRetailQtyAndPriceDataProvider.Param param = new MTRetailQtyAndPriceDataProvider.Param
								{
									MTCode = mTRevaluationRetailSpecificationRow.MTCode,
									UnitCoef = coef,
									Storage = _storage.Code,
									Date = _date.EditValue.Value
								};
								List<MTRetailQtyAndPriceDataProvider.DataRow> rows = new MTRetailQtyAndPriceDataProvider(TradeSettings.CreateTradeClient()).GetRows(param);
								if (rows.Count > 0)
								{
									mTRevaluationRetailSpecificationRow.Cost = rows.FirstOrDefault().Price;
									mTRevaluationRetailSpecificationRow.NewCost = (mTRevaluationRetailSpecificationRow.NewCost / coef2 * coef).RoundAs(2);
								}
							}
						}
					}
				}
			}
			else if (e.Column == _oldCostCol)
			{
				MaterialCacheRow.QntUnitRow qntUnit3 = TradeSettings.TradeCacheService.Material.GetQntUnit(mTRevaluationRetailSpecificationRow.MTID, mTRevaluationRetailSpecificationRow.UnitCode);
				if (qntUnit3 != null)
				{
					MTRetailQtyAndPrice mTRetailQtyAndPrice = new MTRetailQtyAndPrice(mTRevaluationRetailSpecificationRow.MTCode, qntUnit3.Coef, _storage.Code, _date.EditValue.Value);
					ModalBrowser modalBrowser2 = new ModalBrowser(mTRetailQtyAndPrice);
					modalBrowser2.LookUpValue = mTRevaluationRetailSpecificationRow.UnitCode;
					modalBrowser2.Owner = e.Owner;
					modalBrowser2.Left = e.Left;
					modalBrowser2.Top = e.Top;
					if (modalBrowser2.Show())
					{
						mTRevaluationRetailSpecificationRow.Cost = Convert.ToDecimal(mTRetailQtyAndPrice.CommentMember(modalBrowser2.SelectedRowIndex));
					}
				}
			}
			_mtRevaluationRetailGrid.Grid.RefreshRow(e.RowHandle);
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	private void ValidateEmptyDateAndStorage()
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
	}

	private void MTRevaluationRetailTableView_CellValueChanged(object sender, CellValueChangedEventArgs e)
	{
		try
		{
			MTRevaluationRetailSpecificationRow mTRevaluationRetailSpecificationRow = (MTRevaluationRetailSpecificationRow)_mtRevaluationRetailGrid.Grid.GetRow(e.RowHandle);
			if (mTRevaluationRetailSpecificationRow == null)
			{
				return;
			}
			ValidateEmptyDateAndStorage();
			string text = string.Empty;
			if (e.OldValue != null)
			{
				text = e.OldValue.ToString();
			}
			if (e.Column == _mtCodeCol)
			{
				if (e.Value != null)
				{
					string text2 = e.Value.ToString();
					foreach (MTRevaluationRetailSpecificationRow mtRevaluationRetail in _mtRevaluationRetailList)
					{
						if (mTRevaluationRetailSpecificationRow != mtRevaluationRetail && text2 == mtRevaluationRetail.MTCode)
						{
							mTRevaluationRetailSpecificationRow.MTCode = "";
							throw new ASException(string.Format(ArmSoft.SME.Trade.Common.Resources.ValidMtInventory, text2));
						}
					}
					if (text != e.Value.ToString())
					{
						if (!string.IsNullOrWhiteSpace(text2))
						{
							MaterialCacheRow item = TradeSettings.TradeCacheService.Material.GetItem(text2, throwExceptionNotExits: true);
							mTRevaluationRetailSpecificationRow.MTID = item.Id;
							mTRevaluationRetailSpecificationRow.MTName = item.Caption;
							mTRevaluationRetailSpecificationRow.UnitCode = item.Unit;
							mTRevaluationRetailSpecificationRow.UnitBrief = item.UnitRow.Brief;
							MaterialCacheRow.QntUnitRow qntUnit = TradeSettings.TradeCacheService.Material.GetQntUnit(mTRevaluationRetailSpecificationRow.MTID, mTRevaluationRetailSpecificationRow.UnitCode);
							if (qntUnit != null)
							{
								MTRetailQtyAndPriceDataProvider.Param param = new MTRetailQtyAndPriceDataProvider.Param
								{
									MTCode = mTRevaluationRetailSpecificationRow.MTCode,
									UnitCoef = qntUnit.Coef,
									Storage = _storage.Code,
									Date = _date.EditValue.Value
								};
								List<MTRetailQtyAndPriceDataProvider.DataRow> rows = new MTRetailQtyAndPriceDataProvider(TradeSettings.CreateTradeClient()).GetRows(param);
								if (rows.Count > 0)
								{
									mTRevaluationRetailSpecificationRow.Cost = rows[0].Price;
								}
							}
							mTRevaluationRetailSpecificationRow.NewCost = PriceHelper.GetPriceForGivenPriceListAndVATType(_priceType.Code, "2", mTRevaluationRetailSpecificationRow.MTID, mTRevaluationRetailSpecificationRow.UnitCode, _date.EditValue.Value, _currCode.Code);
						}
						else
						{
							mTRevaluationRetailSpecificationRow.MTID = -1;
							mTRevaluationRetailSpecificationRow.MTName = string.Empty;
							mTRevaluationRetailSpecificationRow.UnitCode = string.Empty;
							mTRevaluationRetailSpecificationRow.UnitBrief = string.Empty;
							mTRevaluationRetailSpecificationRow.Quantity = 0m;
							mTRevaluationRetailSpecificationRow.Cost = 0m;
							mTRevaluationRetailSpecificationRow.NewCost = 0m;
						}
					}
				}
			}
			else if (e.Column == _quantityCol && (Convert.ToDecimal(e.Value) == Convert.ToDecimal(e.OldValue) || Convert.ToDecimal(e.Value) < 0m))
			{
				return;
			}
			_mtRevaluationRetailGrid.Grid.RefreshRow(e.RowHandle);
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	private void SetControlsState()
	{
		if (_mtRevaluationRetailList.Count > 0)
		{
			_storage.IsReadOnly = true;
		}
		else if (_currCode.Code != "AMD")
		{
			_storage.IsReadOnly = false;
		}
		else if (_currCode.Code == "AMD")
		{
			_storage.IsReadOnly = false;
		}
	}

	private void LoadMTList()
	{
		GoodsRetailAvailabilityDataProvider.Param param = new GoodsRetailAvailabilityDataProvider.Param
		{
			RemDate = _date.EditValue.Value,
			Storage = _storage.Code
		};
		List<GoodsRetailAvailabilityDataProvider.DataRow> rows = new GoodsRetailAvailabilityDataProvider(TradeSettings.CreateTradeClient()).GetRows(param);
		PricesDataProvider.Param param2 = new PricesDataProvider.Param
		{
			EndDate = _date.EditValue.Value,
			PriceType = _priceType.Code,
			MTCode = string.Join(",", rows.Select((GoodsRetailAvailabilityDataProvider.DataRow x) => x.GoodCode))
		};
		List<PricesDataProvider.DataRow> priceList = null;
		if (!string.IsNullOrWhiteSpace(_priceType.Code))
		{
			priceList = new PricesDataProvider(TradeSettings.CreateTradeClient()).GetRows(param2, withoutAccessCheck: true);
		}
		if (rows.Count <= 0)
		{
			return;
		}
		foreach (GoodsRetailAvailabilityDataProvider.DataRow item in rows)
		{
			MTRevaluationRetailSpecificationRow mTRevaluationRetailSpecificationRow = new MTRevaluationRetailSpecificationRow();
			_maxRowID++;
			mTRevaluationRetailSpecificationRow.MTCode = item.GoodCode;
			mTRevaluationRetailSpecificationRow.MTID = item.GoodID;
			mTRevaluationRetailSpecificationRow.MTName = item.Caption;
			mTRevaluationRetailSpecificationRow.UnitCode = item.Unit;
			mTRevaluationRetailSpecificationRow.UnitBrief = item.UnitBrief;
			mTRevaluationRetailSpecificationRow.Quantity = item.Quantity;
			mTRevaluationRetailSpecificationRow.Cost = item.Price;
			if (!string.IsNullOrWhiteSpace(_priceType.Code))
			{
				if (_isPriceTypeWithVAT)
				{
					mTRevaluationRetailSpecificationRow.NewCost = GetNewPriceForGridRow(priceList, mTRevaluationRetailSpecificationRow.MTID, mTRevaluationRetailSpecificationRow.UnitCode, _exchangeRate.Rate, _exchangeRate.Base);
				}
				else
				{
					decimal newPriceForGridRow = GetNewPriceForGridRow(priceList, mTRevaluationRetailSpecificationRow.MTID, mTRevaluationRetailSpecificationRow.UnitCode, _exchangeRate.Rate, _exchangeRate.Base);
					mTRevaluationRetailSpecificationRow.NewCost = (newPriceForGridRow + PriceHelper.CalcVATAmountByType(newPriceForGridRow, "1", itemVAT: true)).RoundAs(2);
				}
			}
			else
			{
				mTRevaluationRetailSpecificationRow.NewCost = mTRevaluationRetailSpecificationRow.Cost;
			}
			mTRevaluationRetailSpecificationRow.RowID = _maxRowID;
			_mtRevaluationRetailList.Add(mTRevaluationRetailSpecificationRow);
		}
	}

	private void ShowPriceFormationDialog()
	{
		DialogWindow dialogWindow = new DialogWindow();
		dialogWindow.Owner = this;
		dialogWindow.Caption = ArmSoft.SME.Trade.Common.Resources.ChangeNewPrices;
		dialogWindow.AddNumericControl("Percent", ArmSoft.SME.Common.Resources.Percent, 6, 2, isRequired: false, allowNegativeNumbers: true).Value = 0m;
		dialogWindow.AddDropDownControl("RoundDgr", ArmSoft.SME.Common.Resources.DegreeOfRound, new SmartEnum(typeof(RoundDegreesSE)));
		dialogWindow.WindowValidation += PriceFormationDialogValidation;
		dialogWindow.ShowDialog();
	}

	private void PriceFormationDialogValidation(DialogWindow dialog, DialogWindowValidationArgs args)
	{
		decimal num = (decimal)dialog["Percent"];
		string text = (string)dialog["RoundDgr"];
		if (num == 0m && string.IsNullOrWhiteSpace(text))
		{
			ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Trade.Common.Resources.PricesNotChange);
		}
		else
		{
			if (_mtRevaluationRetailList.Count <= 0)
			{
				return;
			}
			foreach (MTRevaluationRetailSpecificationRow mtRevaluationRetail in _mtRevaluationRetailList)
			{
				mtRevaluationRetail.NewCost = Methods.RoundPrice((mtRevaluationRetail.Cost * (1m + num / 100m)).RoundAs(2), text);
			}
			_mtRevaluationRetailGrid.Grid.RefreshData();
		}
	}

	private void FilteredSearch_Click(object sender, RoutedEventArgs e)
	{
		try
		{
			ValidateEmptyDateAndStorage();
			FilteredSearchWindow filteredSearchWindow = new FilteredSearchWindow(new FilteredSearch
			{
				FilterDataView = new GoodsGroups(),
				SearchDataView = new ArmSoft.SME.Trade.UI.DropDown.GoodsFilter(shortMode: true, ItemTypesSE.Goods.Value),
				ResultsList = new MTRevaluationRetailFilteredSearchList(this)
			}, this, isBasketMode: true);
			if (!filteredSearchWindow.ShowDialog().Value)
			{
				return;
			}
			foreach (DocumentGridRow item in filteredSearchWindow.BasketResult)
			{
				bool flag = false;
				MTRevaluationRetailSpecificationRow mTRevaluationRetailSpecificationRow = (MTRevaluationRetailSpecificationRow)item;
				foreach (MTRevaluationRetailSpecificationRow mtRevaluationRetail in _mtRevaluationRetailList)
				{
					if (mTRevaluationRetailSpecificationRow != mtRevaluationRetail && mTRevaluationRetailSpecificationRow.MTID == mtRevaluationRetail.MTID)
					{
						mtRevaluationRetail.Quantity += mTRevaluationRetailSpecificationRow.Quantity;
						flag = true;
						break;
					}
				}
				if (flag)
				{
					continue;
				}
				MaterialCacheRow.QntUnitRow qntUnit = TradeSettings.TradeCacheService.Material.GetQntUnit(mTRevaluationRetailSpecificationRow.MTID, mTRevaluationRetailSpecificationRow.UnitCode);
				if (qntUnit != null)
				{
					MTRetailQtyAndPriceDataProvider.Param param = new MTRetailQtyAndPriceDataProvider.Param
					{
						MTCode = mTRevaluationRetailSpecificationRow.MTCode,
						UnitCoef = qntUnit.Coef,
						Storage = _storage.Code,
						Date = _date.EditValue.Value
					};
					List<MTRetailQtyAndPriceDataProvider.DataRow> rows = new MTRetailQtyAndPriceDataProvider(TradeSettings.CreateTradeClient()).GetRows(param);
					if (rows.Count > 0)
					{
						mTRevaluationRetailSpecificationRow.Cost = rows[0].Price;
					}
				}
				mTRevaluationRetailSpecificationRow.NewCost = PriceHelper.GetPriceForGivenPriceListAndVATType(_priceType.Code, "2", mTRevaluationRetailSpecificationRow.MTID, mTRevaluationRetailSpecificationRow.UnitCode, _date.EditValue.Value, _currCode.Code);
				mTRevaluationRetailSpecificationRow.RowID = _maxRowID;
				_mtRevaluationRetailList.Add(mTRevaluationRetailSpecificationRow);
				_maxRowID++;
			}
			_mtRevaluationRetailGrid.Grid.RefreshData();
			if (_mtRevaluationRetailList.Count > 0)
			{
				_storage.IsReadOnly = true;
			}
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	protected override void FillDocument(StoreRequest sr)
	{
		base.FillDocument(sr);
		PO.BaseISN = BaseISN;
		PO.MaxRowID = _maxRowID;
		PO.MTList.Clear();
		foreach (MTRevaluationRetailSpecificationRow mtRevaluationRetail in _mtRevaluationRetailList)
		{
			MTRevaluationRetail.MTRevaluationRetailSpecificationRow mTRevaluationRetailSpecificationRow = new MTRevaluationRetail.MTRevaluationRetailSpecificationRow();
			mTRevaluationRetailSpecificationRow.MTCode = mtRevaluationRetail.MTCode;
			mTRevaluationRetailSpecificationRow.MTID = mtRevaluationRetail.MTID;
			mTRevaluationRetailSpecificationRow.UnitCode = mtRevaluationRetail.UnitCode;
			mTRevaluationRetailSpecificationRow.Quantity = mtRevaluationRetail.Quantity;
			mTRevaluationRetailSpecificationRow.Cost = mtRevaluationRetail.Cost;
			mTRevaluationRetailSpecificationRow.NewCost = mtRevaluationRetail.NewCost;
			mTRevaluationRetailSpecificationRow.RowID = mtRevaluationRetail.RowID;
			mTRevaluationRetailSpecificationRow.RowNum = mtRevaluationRetail.RowNum;
			mTRevaluationRetailSpecificationRow.MTName = mtRevaluationRetail.MTName;
			mTRevaluationRetailSpecificationRow.FullName = mtRevaluationRetail.FullName;
			mTRevaluationRetailSpecificationRow.UnitBrief = mtRevaluationRetail.UnitBrief;
			PO.MTList.Add(mTRevaluationRetailSpecificationRow);
		}
	}

	protected override void StoreSettings(RegistryKey rk)
	{
		base.StoreSettings(rk);
		_mtRevaluationRetailGrid.StoreSettings(rk);
	}

	protected override void RestoreSettings(double DefaultWidth, double DefaultHeight, RegistryKey rk)
	{
		base.RestoreSettings(DefaultWidth, DefaultHeight, rk);
		_mtRevaluationRetailGrid.RestoreSettings(rk);
	}

	private void BaseDocumentControl_ClickDropDown(object sender, DropDownControlEventArgs e)
	{
		DocumentsJournal documentsJournal = (DocumentsJournal)e.DataView;
		documentsJournal.DialogTop = e.Top;
		documentsJournal.DialogLeft = e.Left;
		if (!e.DataView.ShowDialog())
		{
			e.Cancel = true;
		}
	}

	private void BaseDocument_EditKeyValueChanged(object sender, EditKeyValueChangedEventArgs e)
	{
		if (e.KeyValue != null)
		{
			BaseISN = (Guid)e.KeyValue;
		}
		else
		{
			BaseISN = Guid.Empty;
		}
	}

	private void BaseDocumentSearch_Click(object sender, EditKeyValueChangedEventArgs e)
	{
		if (_baseISN != Guid.Empty)
		{
			Settings.DocumentService.ShowWindow(BaseISN, WindowViewMode.Edit);
		}
	}

	private void BaseDocumentDelete_Click(object sender, EditKeyValueChangedEventArgs e)
	{
		_baseDocumentControl.KeyValue = Guid.Empty;
		_baseDocumentControl.DoValidate();
	}

	private void CreateBasedOnDoc()
	{
		_comment.Text = BaseDocument.Comment;
		_priceType.Code = "";
		PricesDataProvider.Param param = new PricesDataProvider.Param
		{
			EndDate = _date.EditValue.Value,
			PriceType = _priceType.Code
		};
		List<PricesDataProvider.DataRow> priceList = null;
		if (BaseDocument.POType == DocumentTypeSE.MTMove.Value || BaseDocument.POType == DocumentTypeSE.MTMoveWithConfirmation.Value)
		{
			MTMoveWithConfirmationBase mTMoveWithConfirmationBase = (MTMoveWithConfirmationBase)BaseDocument;
			StoragesCacheRow item = TradeSettings.TradeCacheService.Storage.GetItem(mTMoveWithConfirmationBase.StorageIncome);
			if (item == null || !item.Retail)
			{
				throw new ASException(ArmSoft.SME.Trade.Common.Resources.RetailRevalOnlyForRetailStorage, Convert.ToInt32(MessageBoxImage.Asterisk));
			}
			if (!string.IsNullOrWhiteSpace(_priceType.Code))
			{
				param.MTCode = string.Join(",", mTMoveWithConfirmationBase.MTList.Select((MTMoveBase.MTMoveSpecificationRow x) => x.MTCode));
				priceList = new PricesDataProvider(TradeSettings.CreateTradeClient()).GetRows(param, withoutAccessCheck: true);
			}
			CheckStorageAccess(mTMoveWithConfirmationBase.StorageIncome);
			_storage.Code = mTMoveWithConfirmationBase.StorageIncome;
			_mtRevaluationRetailList.Clear();
			_maxRowID = 0;
			{
				foreach (MTMoveBase.MTMoveSpecificationRow mT in mTMoveWithConfirmationBase.MTList)
				{
					MTRevaluationRetailSpecificationRow mTRevaluationRetailSpecificationRow = new MTRevaluationRetailSpecificationRow();
					_maxRowID++;
					mTRevaluationRetailSpecificationRow.MTCode = mT.MTCode;
					mTRevaluationRetailSpecificationRow.MTID = mT.MTID;
					mTRevaluationRetailSpecificationRow.MTName = mT.MTName;
					mTRevaluationRetailSpecificationRow.UnitCode = mT.UnitCode;
					mTRevaluationRetailSpecificationRow.UnitBrief = mT.UnitBrief;
					mTRevaluationRetailSpecificationRow.Cost = mT.Price.Value;
					mTRevaluationRetailSpecificationRow.Quantity = mT.Quantity;
					if (!string.IsNullOrWhiteSpace(_priceType.Code))
					{
						if (_isPriceTypeWithVAT)
						{
							mTRevaluationRetailSpecificationRow.NewCost = GetNewPriceForGridRow(priceList, mTRevaluationRetailSpecificationRow.MTID, mTRevaluationRetailSpecificationRow.UnitCode, _exchangeRate.Rate, _exchangeRate.Base);
						}
						else
						{
							decimal newPriceForGridRow = GetNewPriceForGridRow(priceList, mTRevaluationRetailSpecificationRow.MTID, mTRevaluationRetailSpecificationRow.UnitCode, _exchangeRate.Rate, _exchangeRate.Base);
							mTRevaluationRetailSpecificationRow.NewCost = (newPriceForGridRow + PriceHelper.CalcVATAmountByType(newPriceForGridRow, "1", itemVAT: true)).RoundAs(2);
						}
					}
					else
					{
						mTRevaluationRetailSpecificationRow.NewCost = mTRevaluationRetailSpecificationRow.Cost;
					}
					mTRevaluationRetailSpecificationRow.RowID = _maxRowID;
					_mtRevaluationRetailList.Add(mTRevaluationRetailSpecificationRow);
				}
				return;
			}
		}
		MTIncomeRetail mTIncomeRetail = (MTIncomeRetail)BaseDocument;
		StoragesCacheRow item2 = TradeSettings.TradeCacheService.Storage.GetItem(mTIncomeRetail.Storage);
		if (item2 == null || !item2.Retail)
		{
			throw new ASException(ArmSoft.SME.Trade.Common.Resources.RetailRevalOnlyForRetailStorage, Convert.ToInt32(MessageBoxImage.Asterisk));
		}
		if (!string.IsNullOrWhiteSpace(_priceType.Code))
		{
			param.MTCode = string.Join(",", mTIncomeRetail.MTList.Select((MTIncomeBase.MTIncomeSpecificationRow x) => x.MTCode));
			priceList = new PricesDataProvider(TradeSettings.CreateTradeClient()).GetRows(param, withoutAccessCheck: true);
		}
		CheckStorageAccess(mTIncomeRetail.Storage);
		_storage.Code = mTIncomeRetail.Storage;
		_mtRevaluationRetailList.Clear();
		_maxRowID = 0;
		foreach (MTIncomeBase.MTIncomeSpecificationRow mT2 in mTIncomeRetail.MTList)
		{
			MTRevaluationRetailSpecificationRow mTRevaluationRetailSpecificationRow2 = new MTRevaluationRetailSpecificationRow();
			_maxRowID++;
			mTRevaluationRetailSpecificationRow2.MTCode = mT2.MTCode;
			mTRevaluationRetailSpecificationRow2.MTID = mT2.MTID;
			mTRevaluationRetailSpecificationRow2.MTName = mT2.MTName;
			mTRevaluationRetailSpecificationRow2.UnitCode = mT2.UnitCode;
			mTRevaluationRetailSpecificationRow2.UnitBrief = mT2.UnitBrief;
			mTRevaluationRetailSpecificationRow2.Cost = mT2.Price;
			mTRevaluationRetailSpecificationRow2.Quantity = mT2.Quantity;
			if (!string.IsNullOrWhiteSpace(_priceType.Code))
			{
				if (_isPriceTypeWithVAT)
				{
					mTRevaluationRetailSpecificationRow2.NewCost = GetNewPriceForGridRow(priceList, mTRevaluationRetailSpecificationRow2.MTID, mTRevaluationRetailSpecificationRow2.UnitCode, _exchangeRate.Rate, _exchangeRate.Base);
				}
				else
				{
					decimal newPriceForGridRow2 = GetNewPriceForGridRow(priceList, mTRevaluationRetailSpecificationRow2.MTID, mTRevaluationRetailSpecificationRow2.UnitCode, _exchangeRate.Rate, _exchangeRate.Base);
					mTRevaluationRetailSpecificationRow2.NewCost = (newPriceForGridRow2 + PriceHelper.CalcVATAmountByType(newPriceForGridRow2, "1", itemVAT: true)).RoundAs(2);
				}
			}
			else
			{
				mTRevaluationRetailSpecificationRow2.NewCost = mT2.SalePrice;
			}
			mTRevaluationRetailSpecificationRow2.RowID = _maxRowID;
			_mtRevaluationRetailList.Add(mTRevaluationRetailSpecificationRow2);
		}
	}
}
