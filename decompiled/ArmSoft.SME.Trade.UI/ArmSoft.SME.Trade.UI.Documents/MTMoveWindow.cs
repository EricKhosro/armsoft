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
using Ardalis.SmartEnum;
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
using ArmSoft.SME.Trade.Client.Data.Directories;
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

[DocumentWindow(10)]
public class MTMoveWindow : TradeDocumentWindow<MTMove>
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
		private decimal price;

		[ObservableProperty]
		private decimal summa;

		[ObservableProperty]
		private bool isPartySelected;

		[ObservableProperty]
		private int rowID;

		[ObservableProperty]
		private string rowDescription = "";

		[ObservableProperty]
		private string fullName = "";

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
		public decimal Price
		{
			get
			{
				return price;
			}
			set
			{
				if (!EqualityComparer<decimal>.Default.Equals(price, value))
				{
					OnPropertyChanging(__KnownINotifyPropertyChangingArgs.Price);
					price = value;
					OnPriceChanged(value);
					OnPropertyChanged(__KnownINotifyPropertyChangedArgs.Price);
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
					OnSummaChanged(value);
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

		public MTMoveSpecificationRow()
		{
		}

		public MTMoveSpecificationRow(MTMoveWindow docWindow)
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
			TradePropertyValidator.ValidateMaterialCode(row.MTCode, info, TradeAllDocDefinitions.MTMove.Grids["MTList"].Columns["MTCode"]);
		}

		private static void ValidateUnitCode(MTMoveSpecificationRow row, DevExpress.XtraEditors.DXErrorProvider.ErrorInfo info)
		{
			TradePropertyValidator.ValidateGoodUnitCode(row.UnitCode, row.MTCode, info, TradeAllDocDefinitions.MTMove.Grids["MTList"].Columns["UnitCode"]);
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
			ResetForceValidationBit("Quantity", "Summa");
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		private void OnPriceChanged(decimal value)
		{
			ResetForceValidationBit("Price", "Summa");
			Summa = (value * Quantity).RoundAs(2);
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		private void OnSummaChanged(decimal value)
		{
			ResetForceValidationBit("Summa");
			Price = ((Quantity == 0m) ? 0m : (value / Quantity).RoundAs(2));
		}
	}

	public class MTMoveFilteredSearchList : IResultList
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
			string mtCode = key.ToString();
			decimal num = Convert.ToDecimal(args[0]);
			int num2 = _itemSource.FindIndex((MTMoveSpecificationRow x) => x.MTCode == mtCode);
			if (num2 == -1)
			{
				MaterialCacheRow item = TradeSettings.TradeCacheService.Material.GetItem(mtCode);
				MTMoveSpecificationRow item2 = new MTMoveSpecificationRow
				{
					MTID = item.Id,
					MTCode = item.MTCode,
					MTName = item.Caption,
					UnitCode = item.Unit,
					UnitBrief = item.UnitRow.Brief,
					IsPartySelected = false
				};
				_itemSource.Add(item2);
				num2 = _itemSource.Count - 1;
			}
			_itemSource[num2].Quantity += num;
			return num2;
		}
	}

	protected delegate void RowInfoChangeEventHandler(MTMoveSpecificationRow Row);

	private delegate void DelegateSetRowInfoToInfoPanel(MTMoveSpecificationRow row);

	private delegate void DelegateClearInfoPanel();

	private DropDownCommentControl _baseDocumentControl;

	public DropDownControl _storageExpense;

	private DropDownControl _storageIncome;

	public DropDownControl _pricesTypes;

	private ComboBoxEditExt _comment;

	private DropDownControl _owner;

	private TextEditExt _barCode;

	private Button _codeEnter;

	public DocumentGridControl _mtMoveGrid;

	private TableView _mtMoveTableView;

	private BarEditItemExt _filteredSearch;

	private BarEditItemExt _actualQuantitiesCheck;

	public ObservableCollection<MTMoveSpecificationRow> _mtMoveList;

	private DocumentGridColumn _mtCodeCol;

	private DocumentGridColumn _quantityCol;

	private DocumentGridColumn _unitCodeCol;

	private DocumentGridColumn _priceCol;

	private DocumentGridColumn _summaCol;

	private DocumentGridColumn _partyCol;

	private TextEditExt _chiefAcc;

	private DropDownCommentControl _mediator;

	private DropDownCommentControl _allower;

	private TextEditExt _supBookN;

	private DropDownControl _exportedType;

	private PopupCalcEditExt _bookPage;

	private PopupCalcEditExt _bookRow;

	private TextEditExt _taxInvoiceSeries;

	private TextEditExt _taxInvoiceNumber;

	private DateEditControl _taxInvoiceDate;

	private ComboBoxEditExt _transportationMethod;

	private CheckEditExt _motorVehicle;

	private TextEditExt _brand;

	private TextEditExt _licensePlate;

	private Guid _baseISN = Guid.Empty;

	public bool _storageExpenseIsRetail;

	public bool _storageIncomeIsRetail;

	private readonly bool _isAskQuestionConfirmSummChange = true;

	private bool? _createForSurplus;

	private BarSubItem _barSubItemActions = new BarSubItem();

	private BarSubItem _barSubItemJournals;

	private BarButtonItem _barButtonImportFromFile;

	private BarButtonItem _barButtonPrintLabel;

	private BarButtonItem _barButtonPriceFormation;

	private ReportBarButtonItem _barButtonJumpTo;

	private ReportBarButtonItem _barButtonGoodsOperationsJournal;

	private ReportBarButtonItem _barButtonGoodsRetailOperationJournal;

	private bool _isChanged = true;

	private TextEditExt _infoItemName;

	private InfoDisplayControls _infoDisplayControls;

	private LayoutGroup _infoDisplay;

	private Dictionary<(int, string), decimal> _availabilityByGoodId = new Dictionary<(int, string), decimal>();

	private SafeDocumentEventHandler<FocusedRowHandleChangedEventArgs> _focusedRowHandleChangedEvent;

	private BarButtonItem _barButtonDocumentSettings;

	public bool _distributeByFEFO;

	public int _maxRowID;

	private TradeDocument _baseDocument;

	protected override double WidthValue => 890.0;

	protected override double HeightValue => 480.0;

	protected override double MinWidthValue => 890.0;

	protected override double MinHeightValue => 250.0;

	public TradeDocument BaseDocument
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
			if (BaseDocument.POType == DocumentTypeSE.MTMoveWithConfirmation.Value)
			{
				FillByStockTakingResultsDialogWindow fillByStockTakingResultsDialogWindow = new FillByStockTakingResultsDialogWindow(this);
				if (Convert.ToBoolean(fillByStockTakingResultsDialogWindow.ShowDialog()))
				{
					_createForSurplus = fillByStockTakingResultsDialogWindow.CreateForSurplus;
					CreateBasedOnDoc();
				}
			}
			else if (ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Trade.Common.Resources.FillUsingBaseQ, Settings.ProgramName, MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.Yes) == MessageBoxResult.Yes)
			{
				CreateBasedOnDoc();
			}
		}
	}

	protected event RowInfoChangeEventHandler RowInfoChange;

	public MTMoveWindow()
	{
	}

	public MTMoveWindow(MTMove doc)
		: base(doc)
	{
	}

	protected override void ConstructUI()
	{
		base.ConstructUI();
		new Storages();
		LayoutGroup container = AddLayoutGroup(null, LayoutGroupView.Group, Orientation.Horizontal);
		_date = AddDateControl(container, Definition.Fields["DocDate"]);
		_docNum = AddButtonEditControl(container, Definition.Fields["DocNum"], 1);
		_docNum.MagicClick += new SafeDocumentEventHandler<MagicClickEventArgs>(DocNum_MagicClick, this).Handler;
		_docNum.EditValueChanged += new SafeDocumentEventHandler<EditValueChangedEventArgs>(DocNum_EditValueChanged, this).Handler;
		_docNum.AllowMagicButton = true;
		_docNum.MagicToolTip = ArmSoft.SME.Common.Resources.DocNextNumbers;
		DocumentsJournal idv = new DocumentsJournal(this, ReportTypesSE.AllDocument, ((TradeDocDefinition)Definition).BaseDocTypes)
		{
			DocType = ((TradeDocDefinition)Definition).BaseDocTypes.First().ToString().PadLeft(2, '0'),
			DocState = DocumentStateSE.DraftOrProcessed.ValueUI
		};
		_baseDocumentControl = AddDropDownCommentControl(null, ArmSoft.SME.Common.Resources.BaseDoc, isRequired: false, idv, 50, HorizontalAlignment.Stretch, "BaseDocumentControl");
		_baseDocumentControl.ShowClearButton = true;
		_baseDocumentControl.ShowSearchButton = true;
		_baseDocumentControl.IsTextEditable = false;
		_baseDocumentControl.ClickDropDown += new SafeDocumentEventHandler<DropDownControlEventArgs>(BaseDocument_ClickDropDown, this).Handler;
		_baseDocumentControl.EditKeyValueChanged += new SafeDocumentEventHandler<EditKeyValueChangedEventArgs>(BaseDocument_EditKeyValueChanged, this).Handler;
		_baseDocumentControl.BtnClearClicked += new SafeDocumentEventHandler<EditKeyValueChangedEventArgs>(BaseDocumentDelete_Click, this).Handler;
		_baseDocumentControl.BtnSearchClicked += new SafeDocumentEventHandler<EditKeyValueChangedEventArgs>(BaseDocumentSearch_Click, this).Handler;
		_storageExpense = AddDropDownControl(null, Definition.Fields["StorageExpense"], new Storages(), allowAdditionButtons: false, fieldWithCaption: true, HorizontalAlignment.Stretch);
		_storageExpense.EditValueChanged += new SafeDocumentEventHandler<EditValueChangedEventArgs>(StorageExpense_EditValueChanged, this).Handler;
		_storageIncome = AddDropDownControl(null, Definition.Fields["StorageIncome"], new Storages(), allowAdditionButtons: false, fieldWithCaption: true, HorizontalAlignment.Stretch);
		_storageIncome.EditValueChanged += new SafeDocumentEventHandler<EditValueChangedEventArgs>(StorageIncome_EditValueChanged, this).Handler;
		LayoutGroup container2 = AddLayoutGroup(null, LayoutGroupView.Group, Orientation.Horizontal);
		_barCode = AddStringControl(container2, ArmSoft.SME.Common.Resources.BarCode + " (F2)", isRequired: false, 20, HorizontalAlignment.Stretch);
		_barCode.KeyDown += new SafeDocumentEventHandler<KeyEventArgs>(BarCode_KeyDown, this).Handler;
		_barCode.NullText = ArmSoft.SME.Trade.Common.Resources.BarCodeAndCodeInputNullText;
		string text = ArmSoft.SME.Trade.Common.Resources.cmdCodeEnter + " (F8)";
		_codeEnter = AddButton(container2, "", text, "", text, isTabStop: true);
		_codeEnter.Margin = new Thickness(0.0, 0.0, 18.0, 0.0);
		_codeEnter.Click += new SafeDocumentEventHandler<RoutedEventArgs>(CodeEnter_Click, this).Handler;
		DXTabControl tc = AddTabControl(null);
		LayoutGroup container3 = AddTabbedGroup(tc, ArmSoft.SME.Common.Resources.General, "General");
		_pricesTypes = AddDropDownControl(container3, Definition.Fields["PriceType"], new PriceTypes(), allowAdditionButtons: false, fieldWithCaption: true, HorizontalAlignment.Stretch);
		_pricesTypes.EditValueChanged += new SafeDocumentEventHandler<EditValueChangedEventArgs>(PricesTypes_EditValueChanged, this).Handler;
		List<string> captionsList = TradeSettings.TradeCacheService.StandardComments.GetCaptionsList();
		_comment = AddComboBoxControl(container3, Definition.Fields["Comment"], captionsList, HorizontalAlignment.Stretch);
		_owner = AddDropDownControl(container3, Definition.Fields["Owner"], new UsersFilter());
		_owner.CodeWidth = 12;
		_owner.ShowComment = false;
		_owner.InnerCodeEdit.Width = _owner.InnerCodeEdit.MinWidth;
		if (!Settings.LoginService.IsAdmin && Settings.ParametersService.DenyAccessToOtherOwnerDocs())
		{
			((LayoutItemWithRequired)(object)_owner.Parent).Visibility = Visibility.Collapsed;
		}
		_mtMoveGrid = AddGrid(container3, Definition.Grids["MTList"], HorizontalAlignment.Stretch, VerticalAlignment.Stretch, isSupportDialogMode: true);
		_mtMoveGrid.AllowMoveUp = true;
		_mtMoveGrid.AllowMoveDown = true;
		_mtMoveGrid.IsRequired = true;
		_mtMoveTableView = _mtMoveGrid.View;
		_focusedRowHandleChangedEvent = new SafeDocumentEventHandler<FocusedRowHandleChangedEventArgs>(MTMoveTableView_FocusedRowHandleChanged, this);
		_mtMoveTableView.FocusedRowHandleChanged += _focusedRowHandleChangedEvent.Handler;
		_mtMoveGrid.AddRowIndicator(35);
		_filteredSearch = _mtMoveGrid.AddButton(null, ArmSoft.SME.Trade.Common.Resources.GroupAddition, ArmSoft.SME.Trade.Common.Resources.GroupAddition, 130.0);
		_filteredSearch.Click += new SafeDocumentEventHandler<RoutedEventArgs>(FilteredSearch_Click, this).Handler;
		_actualQuantitiesCheck = _mtMoveGrid.AddButton("cheked_all_16.svg", ArmSoft.SME.Trade.Common.Resources.ActualQuantitiesCheck, ArmSoft.SME.Trade.Common.Resources.ActualQuantitiesCheck, 255.0);
		_actualQuantitiesCheck.Click += new SafeDocumentEventHandler<RoutedEventArgs>(ActualQuantitiesCheck_Click, this).Handler;
		_mtCodeCol = _mtMoveGrid.AddDropDownColumn(Definition.Grids["MTList"].Columns["MTCode"], new ArmSoft.SME.Trade.UI.DropDown.GoodsFilter(shortMode: true, GoodsTypesSE.Good, "", isShowOnlyInPriceList: true), allowAdditionButtons: true, isOwnProcessingClickDropDown: true);
		DocumentGridColumn documentGridColumn = _mtMoveGrid.AddStringColumn(Definition.Grids["MTList"].Columns["MTName"]);
		documentGridColumn.AllowEditing = DefaultBoolean.False;
		documentGridColumn.Width = 225.0;
		_unitCodeCol = _mtMoveGrid.AddDropDownColumn(Definition.Grids["MTList"].Columns["UnitCode"], new MTQntUnits());
		_mtMoveGrid.AddStringColumn(Definition.Grids["MTList"].Columns["UnitBrief"]).AllowEditing = DefaultBoolean.False;
		_quantityCol = _mtMoveGrid.AddNumberColumn(Definition.Grids["MTList"].Columns["Quantity"], showTotal: true);
		if (string.IsNullOrWhiteSpace(PO.StorageExpense))
		{
			if (!string.IsNullOrWhiteSpace(Settings.ParametersService.DefaultStorage()))
			{
				_storageExpenseIsRetail = TradeSettings.TradeCacheService.Storage.GetItem(Settings.ParametersService.DefaultStorage())?.Retail ?? false;
			}
		}
		else
		{
			if (!string.IsNullOrWhiteSpace(PO.StorageExpense))
			{
				_storageExpenseIsRetail = TradeSettings.TradeCacheService.Storage.GetItem(PO.StorageExpense)?.Retail ?? false;
			}
			if (!string.IsNullOrWhiteSpace(PO.StorageIncome))
			{
				_storageIncomeIsRetail = TradeSettings.TradeCacheService.Storage.GetItem(PO.StorageIncome)?.Retail ?? false;
			}
		}
		_priceCol = _mtMoveGrid.AddNumberColumn(Definition.Grids["MTList"].Columns["Price"], showTotal: false, useCurrencyFormat: true, isOwnProcessingClickDropDown: true);
		_summaCol = _mtMoveGrid.AddNumberColumn(Definition.Grids["MTList"].Columns["Summa"], showTotal: true);
		_summaCol.AllowEditing = DefaultBoolean.False;
		if (_storageExpenseIsRetail)
		{
			_pricesTypes.Code = "";
		}
		SetPriceSumColumnsReadOnlyProperty();
		_partyCol = _mtMoveGrid.AddDropDownColumn("PartyMethodName", ArmSoft.SME.Common.Resources.Party, 19, trimValue: false, GlyphKind.Regular);
		((ButtonEditSettings)_partyCol.EditSettings).IsTextEditable = false;
		_mtMoveList = new ObservableCollection<MTMoveSpecificationRow>();
		_mtMoveGrid.AddStringColumn(Definition.Grids["MTList"].Columns["RowDescription"]);
		_mtMoveGrid.DeletingRow += new SafeDocumentEventHandler<DocumentGridDeletingRowEventArgs>(MTMoveGrid_DeletingRow, this).Handler;
		_mtMoveGrid.AddingRow += new SafeDocumentEventHandler<DocumentGridAddingRowEventArgs>(MTMoveGrid_AddingRow, this).Handler;
		_mtMoveGrid.DuplicatingRow += new SafeDocumentEventHandler<DocumentGridDuplicatingRowEventArgs>(MTMoveGrid_DuplicatingRow, this).Handler;
		_mtMoveGrid.AddingRowWithDialog += new SafeDocumentEventHandler<DocumentGridAddingRowEventArgs>(MTMoveGrid_AddingRowWithDialog, this).Handler;
		_mtMoveGrid.DuplicatingRowWithDialog += new SafeDocumentEventHandler<DocumentGridDuplicatingRowEventArgs>(MTMoveGrid_DuplicatingRowWithDialog, this).Handler;
		_mtMoveGrid.EditingRowWithDialog += new SafeDocumentEventHandler<DocumentGridEditingRowEventArgs>(MTMoveGrid_EditingRowWithDialog, this).Handler;
		_mtMoveGrid.MovingRowUp += new SafeDocumentEventHandler<DocumentGridMovingRowUpOrDownEventArgs>(MTMoveGrid_MovingRowUp, this).Handler;
		_mtMoveGrid.MovingRowDown += new SafeDocumentEventHandler<DocumentGridMovingRowUpOrDownEventArgs>(MTMoveGrid_MovingRowDown, this).Handler;
		_mtMoveGrid.RefreshButtons += new SafeDocumentEventHandler<EventArgs>(MTMoveGrid_RefreshButtons, this).Handler;
		_mtMoveGrid.ClickDropDown += new SafeDocumentEventHandler<DocumentGridClickDropDownEventArgs>(MTMoveGrid_ClickDropDown, this).Handler;
		foreach (GridColumn column in _mtMoveGrid.Grid.Columns)
		{
			column.AllowSorting = DefaultBoolean.True;
			column.AllowColumnFiltering = DefaultBoolean.True;
			column.FilterPopupMode = FilterPopupMode.CheckedList;
		}
		_infoDisplayControls = AddInfoDisplayControls(DocumentTypeSE.MTMove, isShowItemPictureOnInfoPanel: false, isShowStorageAvailabilityOnInfoPanel: true, isShowWholeAvailabilityOnInfoPanel: true, isShowPriceAvailabilityOnInfoPanel: false, showDescriptionOnInfoPanel: false, enableSubstituteItems: false, 22.0, container3);
		_infoDisplay = _infoDisplayControls.InfoDisplay;
		_infoDisplay.Collapsed += new SafeDocumentEventHandler<EventArgs>(InfoDisplay_Collapsed, this).Handler;
		_infoItemName = _infoDisplayControls.InfoItemName;
		LayoutGroup container4 = AddTabbedGroup(tc, ArmSoft.SME.Common.Resources.Additional, "Additional");
		_mediator = AddDropDownCommentControl(container4, Definition.Fields["Mediator"], new Employees(), HorizontalAlignment.Stretch);
		_chiefAcc = AddStringControl(container4, Definition.Fields["Chief"], HorizontalAlignment.Stretch);
		_allower = AddDropDownCommentControl(container4, Definition.Fields["Allower"], new Employees(), HorizontalAlignment.Stretch);
		LayoutGroup container5 = AddLayoutGroup(container4, LayoutGroupView.Group, Orientation.Horizontal);
		_supBookN = AddStringControl(container5, Definition.Fields["BookNum"]);
		_bookPage = AddNumericControl(container5, Definition.Fields["BookPage"], useCurrencyFormat: false);
		_bookRow = AddNumericControl(container5, Definition.Fields["BookLine"], useCurrencyFormat: false);
		_exportedType = AddSmartEnumDropDownControl(container4, Definition.Fields["ExportedType"], allowAdditionButtons: false, fieldWithCaption: true, HorizontalAlignment.Stretch);
		LayoutGroup container6 = AddLayoutGroup(container4, LayoutGroupView.Group, Orientation.Horizontal);
		_taxInvoiceSeries = AddStringControl(container6, Definition.Fields["TaxInvoiceSeries"]);
		_taxInvoiceNumber = AddStringControl(container6, Definition.Fields["TaxInvoiceNumber"]);
		_taxInvoiceDate = AddDateControl(container4, Definition.Fields["InvoiceDate"]);
		_transportationMethod = AddComboBoxControl(container4, Definition.Fields["TransportationMethod"], captionsList, HorizontalAlignment.Stretch);
		_motorVehicle = AddBooleanControl(container4, Definition.Fields["MotorVehicle"]);
		_motorVehicle.EditValueChanged += new SafeDocumentEventHandler<EditValueChangedEventArgs>(MotorVehicle_EditValueChanged, this).Handler;
		_brand = AddStringControl(container4, Definition.Fields["Brand"], HorizontalAlignment.Stretch);
		_licensePlate = AddStringControl(container4, Definition.Fields["LicensePlate"], HorizontalAlignment.Stretch);
		_infoDisplay.IsCollapsed = _infoDisplayControls.InfoPanelCollapsed;
		_mtMoveTableView.CellValueChanged += new SafeDocumentEventHandler<CellValueChangedEventArgs>(MTMoveTableView_CellValueChanged, this).Handler;
		RowInfoChange += ChangeRowInfo;
	}

	protected override void OnNew()
	{
		base.OnNew();
		_date.EditValue = Settings.HostingEnvironment.CurrentDate;
		_owner.Code = Settings.LoginService.UserName;
		if (_baseISN == Guid.Empty && string.IsNullOrWhiteSpace(_storageExpense.Code))
		{
			string text = Settings.ParametersService.DefaultStorage();
			if (!string.IsNullOrWhiteSpace(text) && TradeSettings.CreateTradeClient().CommonServices.IsStorageAvailable(text))
			{
				_storageExpense.Code = text;
			}
			_chiefAcc.Text = Settings.ParametersService.ChiefAccountantName();
		}
		if (_viewMode == WindowViewMode.Copy && _distributeByFEFO)
		{
			PO.MTExpDocSelectedPartiesList.Clear();
			decimal expSumm = 0m;
			for (int i = 0; i <= _mtMoveList.Count - 1; i++)
			{
				DistributeQtyByExpiredDate(_mtMoveList[i].RowID, _storageExpense.Code, _mtMoveList[i].MTCode, _mtMoveList[i].UnitCode, _mtMoveList[i].Quantity, ref expSumm);
				_mtMoveList[i].Summa = expSumm;
				_mtMoveList[i].IsPartySelected = true;
			}
		}
	}

	protected override void OnNewAfterSave()
	{
		base.OnNewAfterSave();
		_mtMoveList.Clear();
		_maxRowID = 0;
		_owner.Code = Settings.LoginService.UserName;
		if (_storageExpenseIsRetail)
		{
			_pricesTypes.Code = "";
		}
	}

	protected override void OnEdit()
	{
		base.OnEdit();
		_setFocusOnActivation = _docNum;
		if (!string.IsNullOrWhiteSpace(_storageExpense.Code))
		{
			_storageExpenseIsRetail = TradeSettings.TradeCacheService.Storage.GetItem(_storageExpense.Code)?.Retail ?? false;
		}
		if (!string.IsNullOrWhiteSpace(_storageIncome.Code))
		{
			_storageIncomeIsRetail = TradeSettings.TradeCacheService.Storage.GetItem(_storageIncome.Code)?.Retail ?? false;
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
		_mtMoveList.Clear();
		_focusedRowHandleChangedEvent.HandleOnLoadingDocumentToControls = true;
		foreach (MTMoveBase.MTMoveSpecificationRow mT in PO.MTList)
		{
			MTMoveSpecificationRow item = new MTMoveSpecificationRow
			{
				MTID = mT.MTID,
				MTCode = mT.MTCode,
				MTName = mT.MTName,
				FullName = mT.FullName,
				UnitCode = mT.UnitCode,
				UnitBrief = mT.UnitBrief,
				Quantity = mT.Quantity,
				RowNum = mT.RowNum,
				Price = mT.Price.GetValueOrDefault(),
				Summa = mT.Summa.GetValueOrDefault(),
				IsPartySelected = mT.IsPartySelected,
				RowID = mT.RowID,
				RowDescription = mT.RowDescription
			};
			_mtMoveList.Add(item);
		}
		_maxRowID = PO.MaxRowID;
		_mtMoveGrid.ItemsSource = _mtMoveList;
		if (!(_baseISN == Guid.Empty) && !PO.StoredInDb && _viewMode != WindowViewMode.Copy)
		{
			CreateBasedOnDoc();
		}
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
		PO.BaseISN = _baseISN;
		PO.MTList.Clear();
		foreach (MTMoveSpecificationRow mtMove in _mtMoveList)
		{
			MTMoveBase.MTMoveSpecificationRow item = new MTMoveBase.MTMoveSpecificationRow
			{
				MTID = mtMove.MTID,
				MTCode = mtMove.MTCode,
				MTName = mtMove.MTName,
				FullName = mtMove.FullName,
				UnitCode = mtMove.UnitCode,
				UnitBrief = mtMove.UnitBrief,
				Quantity = mtMove.Quantity,
				RowNum = mtMove.RowNum,
				Price = mtMove.Price,
				Summa = mtMove.Summa,
				IsPartySelected = mtMove.IsPartySelected,
				RowID = mtMove.RowID,
				RowDescription = mtMove.RowDescription
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
		_pricesTypes.IsReadOnly = _storageExpenseIsRetail;
		SetMotorVehicleReadOnlyProperty();
		bool flag = State == DocumentStateSE.Draft.Value || State == DocumentStateSE.Deleted.Value;
		bool flag2 = _viewMode == WindowViewMode.Edit && !flag;
		_date.IsReadOnly = flag2;
		_storageIncome.IsReadOnly = flag2;
		_storageExpense.IsReadOnly = flag2;
		_baseDocumentControl.IsReadOnly = flag2;
		_barCode.IsReadOnly = flag2;
		_codeEnter.IsEnabled = !flag2;
		foreach (GridColumn column in _mtMoveGrid.Grid.Columns)
		{
			column.ReadOnly = flag2;
		}
		_mtMoveGrid.AllowAdd = !flag2;
		_mtMoveGrid.AllowEdit = !flag2;
		_mtMoveGrid.AllowDelete = !flag2;
		_mtMoveGrid.AllowDuplicate = !flag2;
		_mtMoveGrid.AllowMoveUp = !flag2;
		_mtMoveGrid.AllowMoveDown = !flag2;
		SetPriceSumColumnsReadOnlyProperty();
		GridColumn gridColumn = _mtMoveGrid.Grid.Columns["PartyMethodName"];
		gridColumn.ReadOnly = false;
		((ButtonEditSettings)gridColumn.EditSettings).IsTextEditable = false;
	}

	private void DistributeQtyByExpiredDate(int RowID, string Storage, string GoodCode, string UnitCode, decimal Quantity, ref decimal expSumm, string BaseUnit = "")
	{
		decimal num = Quantity;
		List<PartyRow> list = new List<PartyRow>();
		List<PartyRow> list2 = new List<PartyRow>();
		List<PartyRow> list3 = new List<PartyRow>();
		bool flag = TradeSettings.CreateTradeClient().CommonServices.IsNegativExpAllow(Storage);
		string text = "";
		GoodsEngineServicesRoutes.PartyRowAndExpMethod parties = TradeSettings.CreateTradeClient().GoodsEngineServices.GetParties(Storage, GoodCode, _date.EditValue.Value);
		MaterialCacheRow item = TradeSettings.TradeCacheService.Material.GetItem(GoodCode);
		BaseUnit = item.Unit;
		Quantity = (Quantity * PriceHelper.GetMTQntUnitCoeff(item.MTCode, UnitCode)).RoundAs(3);
		num = Quantity;
		UnitCode = BaseUnit;
		foreach (PartyRow partyRow10 in parties.partyRows)
		{
			partyRow10.ExpQty = 0m;
			partyRow10.ExpSumma = 0m;
			partyRow10.GridRow = RowID;
			if (!partyRow10.UseableDate.HasValue)
			{
				list2.Add(partyRow10);
			}
			else
			{
				list3.Add(partyRow10);
			}
		}
		list.AddRange(from ListRow in list3
			orderby ListRow.UseableDate, ListRow.IncomeDate, ListRow.OrderNum, ListRow.Row
			select ListRow);
		if (list != null)
		{
			foreach (PartyRow item2 in list)
			{
				if (!flag)
				{
					if (item2.Qty > 0m)
					{
						if (num >= item2.Qty)
						{
							item2.ExpQty = item2.Qty;
							PartyRow partyRow = item2;
							decimal expSumma;
							if (item2.ExpQty == item2.Qty)
							{
								expSumma = item2.Summa;
							}
							else
							{
								expSumma = ((!flag) ? (item2.Cost * item2.ExpQty).RoundAs(2) : ((item2.Cost * item2.ExpQty < item2.Qty) ? item2.ExpQty : item2.Qty).RoundAs(2));
							}
							partyRow.ExpSumma = expSumma;
							num -= item2.Qty;
						}
						else
						{
							item2.ExpQty = num;
							PartyRow partyRow2 = item2;
							decimal expSumma2;
							if (item2.ExpQty == item2.Qty)
							{
								expSumma2 = item2.Summa;
							}
							else
							{
								expSumma2 = ((!flag) ? (item2.Cost * item2.ExpQty).RoundAs(2) : ((item2.Cost * item2.ExpQty < item2.Qty) ? item2.ExpQty : item2.Qty).RoundAs(2));
							}
							partyRow2.ExpSumma = expSumma2;
							num = 0m;
						}
					}
				}
				else if (item2.Qty > 0m)
				{
					if (num >= item2.Qty)
					{
						item2.ExpQty = item2.Qty;
						PartyRow partyRow3 = item2;
						decimal expSumma3;
						if (item2.ExpQty == item2.Qty)
						{
							expSumma3 = item2.Summa;
						}
						else
						{
							expSumma3 = ((!flag) ? (item2.Cost * item2.ExpQty).RoundAs(2) : ((item2.Cost * item2.ExpQty < item2.Qty) ? item2.ExpQty : item2.Qty).RoundAs(2));
						}
						partyRow3.ExpSumma = expSumma3;
						num -= item2.Qty;
					}
					else
					{
						item2.ExpQty = num;
						PartyRow partyRow4 = item2;
						decimal expSumma4;
						if (item2.ExpQty == item2.Qty)
						{
							expSumma4 = item2.Summa;
						}
						else
						{
							expSumma4 = ((!flag) ? (item2.Cost * item2.ExpQty).RoundAs(2) : ((item2.Cost * item2.ExpQty < item2.Qty) ? item2.ExpQty : item2.Qty).RoundAs(2));
						}
						partyRow4.ExpSumma = expSumma4;
						num = 0m;
					}
				}
			}
			if ((num > 0m) & flag)
			{
				List<PartyRow> partyRows = parties.partyRows;
				partyRows[partyRows.Count - 1].ExpQty += num;
			}
		}
		if (num > 0m && list2 != null)
		{
			foreach (PartyRow item3 in list2)
			{
				if (!flag)
				{
					if (item3.Qty > 0m)
					{
						if (num >= item3.Qty)
						{
							item3.ExpQty = item3.Qty;
							PartyRow partyRow5 = item3;
							decimal expSumma5;
							if (item3.ExpQty == item3.Qty)
							{
								expSumma5 = item3.Summa;
							}
							else
							{
								expSumma5 = ((!flag) ? (item3.Cost * item3.ExpQty).RoundAs(2) : ((item3.Cost * item3.ExpQty < item3.Qty) ? item3.ExpQty : item3.Qty).RoundAs(2));
							}
							partyRow5.ExpSumma = expSumma5;
							num -= item3.Qty;
						}
						else
						{
							item3.ExpQty = num;
							PartyRow partyRow6 = item3;
							decimal expSumma6;
							if (item3.ExpQty == item3.Qty)
							{
								expSumma6 = item3.Summa;
							}
							else
							{
								expSumma6 = ((!flag) ? (item3.Cost * item3.ExpQty).RoundAs(2) : ((item3.Cost * item3.ExpQty < item3.Qty) ? item3.ExpQty : item3.Qty).RoundAs(2));
							}
							partyRow6.ExpSumma = expSumma6;
							num = 0m;
						}
					}
				}
				else if (num >= item3.Qty)
				{
					if (item3 == parties.partyRows[parties.partyRows.Count - 1])
					{
						item3.ExpQty = num;
						PartyRow partyRow7 = item3;
						decimal expSumma7;
						if (item3.ExpQty == item3.Qty)
						{
							expSumma7 = item3.Summa;
						}
						else
						{
							expSumma7 = ((!flag) ? (item3.Cost * item3.ExpQty).RoundAs(2) : ((item3.Cost * item3.ExpQty < item3.Qty) ? item3.ExpQty : item3.Qty).RoundAs(2));
						}
						partyRow7.ExpSumma = expSumma7;
						num = 0m;
					}
					else
					{
						item3.ExpQty = item3.Qty;
						PartyRow partyRow8 = item3;
						decimal expSumma8;
						if (item3.ExpQty == item3.Qty)
						{
							expSumma8 = item3.Summa;
						}
						else
						{
							expSumma8 = ((!flag) ? (item3.Cost * item3.ExpQty).RoundAs(2) : ((item3.Cost * item3.ExpQty < item3.Qty) ? item3.ExpQty : item3.Qty).RoundAs(2));
						}
						partyRow8.ExpSumma = expSumma8;
						num -= item3.Qty;
					}
				}
				else
				{
					item3.ExpQty = num;
					PartyRow partyRow9 = item3;
					decimal expSumma9;
					if (item3.ExpQty == item3.Qty)
					{
						expSumma9 = item3.Summa;
					}
					else
					{
						expSumma9 = ((!flag) ? (item3.Cost * item3.ExpQty).RoundAs(2) : ((item3.Cost * item3.ExpQty < item3.Qty) ? item3.ExpQty : item3.Qty).RoundAs(2));
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
		parties.partyRows = list5;
		List<PartyRow> list6 = new List<PartyRow>();
		if (parties.partyRows.Count > 1)
		{
			if (text == "2")
			{
				list6.AddRange(from ListRow in parties.partyRows
					orderby ListRow.IncomeDate descending, ListRow.OrderNum, ListRow.Row
					select ListRow);
			}
			else
			{
				list6.AddRange(from ListRow in parties.partyRows
					orderby ListRow.IncomeDate, ListRow.OrderNum, ListRow.Row
					select ListRow);
			}
		}
		else
		{
			list6 = parties.partyRows;
		}
		expSumm = 0m;
		foreach (PartyRow partyRow11 in parties.partyRows)
		{
			PO.MTExpDocSelectedPartiesList.Add(partyRow11.Clone());
			expSumm += partyRow11.ExpSumma;
		}
	}

	private void UnitChanged(string unitCodeNew, string unitCodeOld, MTMoveSpecificationRow mtExpGridRow, bool distributeparties = true)
	{
		QntUnitCacheRow item = TradeSettings.TradeCacheService.QntUnit.GetItem(unitCodeNew);
		if (item == null)
		{
			mtExpGridRow.UnitBrief = "";
			return;
		}
		mtExpGridRow.UnitBrief = item.Brief;
		if (_storageExpenseIsRetail)
		{
			mtExpGridRow.Price = PriceHelper.GetPriceForUnit(unitCodeOld, unitCodeNew, mtExpGridRow.Price, mtExpGridRow.MTCode);
		}
		if (_distributeByFEFO & distributeparties)
		{
			decimal expSumm = 0m;
			PO.MTExpDocSelectedPartiesList.RemoveAll((PartyRow x) => x.GridRow == mtExpGridRow.RowID);
			DistributeQtyByExpiredDate(mtExpGridRow.RowID, _storageExpense.Code, mtExpGridRow.MTCode, mtExpGridRow.UnitCode, mtExpGridRow.Quantity, ref expSumm);
			mtExpGridRow.Summa = expSumm;
			mtExpGridRow.IsPartySelected = true;
		}
	}

	private void MTMoveGrid_DeletingRow(object sender, DocumentGridDeletingRowEventArgs e)
	{
		MTMoveSpecificationRow MTMoveGridRow = (MTMoveSpecificationRow)_mtMoveGrid.Grid.GetRow(e.RowHandle);
		if (MTMoveGridRow != null)
		{
			PO.MTExpDocSelectedPartiesList.RemoveAll((PartyRow x) => x.GridRow == MTMoveGridRow.RowID);
			RowInfoChange?.Invoke((MTMoveSpecificationRow)_mtMoveTableView.Grid.CurrentItem);
		}
	}

	private void MTMoveGrid_AddingRow(object sender, DocumentGridAddingRowEventArgs e)
	{
		ValidateDocFields();
		if (string.IsNullOrWhiteSpace(_storageExpense.Code))
		{
			ControlsHelper.SetFocus(_storageExpense);
			throw new ASException(ArmSoft.SME.Common.Resources.ValidEmptyExpenseStorage);
		}
		_mtMoveList.Add(new MTMoveSpecificationRow
		{
			IsPartySelected = false,
			RowID = _maxRowID
		});
		_maxRowID++;
		e.RowNumber = _mtMoveList.Count - 1;
		RowInfoChangeEventHandler rowInfoChangeEventHandler = RowInfoChange;
		if (rowInfoChangeEventHandler != null)
		{
			ObservableCollection<MTMoveSpecificationRow> mtMoveList = _mtMoveList;
			rowInfoChangeEventHandler(mtMoveList[mtMoveList.Count - 1]);
		}
	}

	private void MTMoveGrid_DuplicatingRow(object sender, DocumentGridDuplicatingRowEventArgs e)
	{
		MTMoveSpecificationRow mTMoveSpecificationRow = (MTMoveSpecificationRow)_mtMoveGrid.Grid.GetRow(e.RowHandle);
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
			mTMoveSpecificationRow2.IsPartySelected = false;
			mTMoveSpecificationRow2.RowID = _maxRowID;
			_mtMoveList.Add(mTMoveSpecificationRow2);
			decimal expSumm = 0m;
			if (_distributeByFEFO)
			{
				DistributeQtyByExpiredDate(mTMoveSpecificationRow2.RowID, _storageExpense.Code, mTMoveSpecificationRow2.MTCode, mTMoveSpecificationRow2.UnitCode, mTMoveSpecificationRow2.Quantity, ref expSumm);
				mTMoveSpecificationRow2.Summa = expSumm;
				mTMoveSpecificationRow2.IsPartySelected = true;
			}
			_maxRowID++;
			e.NewRowNumber = _mtMoveList.Count - 1;
		}
	}

	private void MTMoveGrid_AddingRowWithDialog(object sender, DocumentGridAddingRowEventArgs e)
	{
		ValidateDocFields();
		GoodsSelectPartyWindow goodsSelectPartyWindow = new GoodsSelectPartyWindow(DialogWindowMode.NewModeWithNext, this);
		goodsSelectPartyWindow.ShowDialog();
		int addedRows = goodsSelectPartyWindow.AddedRows;
		if (addedRows != 0)
		{
			_maxRowID += addedRows;
			RowInfoChangeEventHandler rowInfoChangeEventHandler = RowInfoChange;
			if (rowInfoChangeEventHandler != null)
			{
				ObservableCollection<MTMoveSpecificationRow> mtMoveList = _mtMoveList;
				rowInfoChangeEventHandler(mtMoveList[mtMoveList.Count - 1]);
			}
		}
	}

	private void MTMoveGrid_DuplicatingRowWithDialog(object sender, DocumentGridDuplicatingRowEventArgs e)
	{
		MTMoveSpecificationRow mTMoveSpecificationRow = (MTMoveSpecificationRow)_mtMoveGrid.Grid.GetRow(e.RowHandle);
		if (mTMoveSpecificationRow != null)
		{
			ValidateDocFields();
			GoodsSelectPartyWindow goodsSelectPartyWindow = new GoodsSelectPartyWindow(DialogWindowMode.NewModeWithNext, this, mTMoveSpecificationRow);
			goodsSelectPartyWindow.ShowDialog();
			int addedRows = goodsSelectPartyWindow.AddedRows;
			if (addedRows != 0)
			{
				_maxRowID += addedRows;
				RowInfoChange(mTMoveSpecificationRow);
			}
		}
	}

	private void MTMoveGrid_EditingRowWithDialog(object sender, DocumentGridEditingRowEventArgs e)
	{
		MTMoveSpecificationRow sourceRow = (MTMoveSpecificationRow)_mtMoveGrid.Grid.GetRow(e.RowHandle);
		if (sourceRow == null)
		{
			return;
		}
		ValidateDocFields();
		GoodsSelectPartyWindow goodsSelectPartyWindow = new GoodsSelectPartyWindow(e.AllowEdit ? DialogWindowMode.EditMode : DialogWindowMode.ViewMode, this, sourceRow);
		e.RowEdited = Convert.ToBoolean(goodsSelectPartyWindow.ShowDialog());
		if (_distributeByFEFO && sourceRow.IsPartySelected && _distributeByFEFO)
		{
			decimal expSumm = 0m;
			PO.MTExpDocSelectedPartiesList.RemoveAll((PartyRow x) => x.GridRow == sourceRow.RowID);
			DistributeQtyByExpiredDate(sourceRow.RowID, _storageExpense.Code, sourceRow.MTCode, sourceRow.UnitCode, sourceRow.Quantity, ref expSumm);
			sourceRow.Summa = expSumm;
			sourceRow.IsPartySelected = true;
		}
		if (e.AllowEdit && e.RowEdited)
		{
			RowInfoChange?.Invoke(sourceRow);
		}
	}

	private void MTMoveGrid_MovingRowUp(object sender, DocumentGridMovingRowUpOrDownEventArgs e)
	{
		if (_mtMoveGrid.Grid.SortInfo.Count > 0 || _mtMoveTableView.Grid.VisibleRowCount != _mtMoveList.Count)
		{
			throw new ASException(ArmSoft.SME.Common.Resources.NotAllowRowMoveInSortedGrid);
		}
		MTMoveSpecificationRow mTMoveSpecificationRow = (MTMoveSpecificationRow)_mtMoveGrid.Grid.GetRow(e.RowHandle);
		if (mTMoveSpecificationRow != null)
		{
			_mtMoveList.RemoveAt(e.RowHandle);
			_mtMoveList.Insert(e.RowHandle - 1, mTMoveSpecificationRow);
		}
	}

	private void MTMoveGrid_MovingRowDown(object sender, DocumentGridMovingRowUpOrDownEventArgs e)
	{
		if (_mtMoveGrid.Grid.SortInfo.Count > 0 || _mtMoveTableView.Grid.VisibleRowCount != _mtMoveList.Count)
		{
			throw new ASException(ArmSoft.SME.Common.Resources.NotAllowRowMoveInSortedGrid);
		}
		MTMoveSpecificationRow mTMoveSpecificationRow = (MTMoveSpecificationRow)_mtMoveGrid.Grid.GetRow(e.RowHandle);
		if (mTMoveSpecificationRow != null)
		{
			_mtMoveList.RemoveAt(e.RowHandle);
			_mtMoveList.Insert(e.RowHandle + 1, mTMoveSpecificationRow);
		}
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

	private void MTMoveGrid_ClickDropDown(object sender, DocumentGridClickDropDownEventArgs e)
	{
		MTMoveSpecificationRow mTMoveSpecificationRow = (MTMoveSpecificationRow)_mtMoveGrid.Grid.GetRow(e.RowHandle);
		if (e.Column == _mtCodeCol)
		{
			ValidateInnerDocFields();
			bool showAll = TradeSettings.CreateTradeClient().CommonServices.IsNegativExpAllow(_storageExpense.Code);
			ArmSoft.SME.Trade.UI.DropDown.GoodsAvailabilityFilter goodsAvailabilityFilter = new ArmSoft.SME.Trade.UI.DropDown.GoodsAvailabilityFilter(_date.EditValue.Value, _storageExpense.Code, _storageExpenseIsRetail, includeServices: false, showAll);
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
			if (mTMoveSpecificationRow != null)
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
		}
		else if (e.Column == _priceCol)
		{
			if (!_storageExpenseIsRetail || mTMoveSpecificationRow == null)
			{
				return;
			}
			MaterialCacheRow.QntUnitRow qntUnit = TradeSettings.TradeCacheService.Material.GetQntUnit(mTMoveSpecificationRow.MTID, mTMoveSpecificationRow.UnitCode);
			if (qntUnit != null)
			{
				MTRetailQtyAndPrice mTRetailQtyAndPrice = new MTRetailQtyAndPrice(mTMoveSpecificationRow.MTCode, qntUnit.Coef, _storageExpense.Code, _date.EditValue.Value);
				ModalBrowser modalBrowser3 = new ModalBrowser(mTRetailQtyAndPrice)
				{
					LookUpValue = mTMoveSpecificationRow.UnitCode,
					Owner = e.Owner,
					Left = e.Left,
					Top = e.Top
				};
				if (modalBrowser3.Show())
				{
					mTMoveSpecificationRow.Price = Convert.ToDecimal(mTRetailQtyAndPrice.CommentMember(modalBrowser3.SelectedRowIndex));
					mTMoveSpecificationRow.Summa = (mTMoveSpecificationRow.Price * mTMoveSpecificationRow.Quantity).RoundAs(2);
				}
			}
		}
		else
		{
			if (e.Column != _partyCol || mTMoveSpecificationRow == null)
			{
				return;
			}
			if (!_date.DoValidate())
			{
				throw new ASException(ArmSoft.SME.Common.Resources.ValidateEmptyDate);
			}
			if (string.IsNullOrWhiteSpace(mTMoveSpecificationRow.MTCode))
			{
				throw new ASException(ArmSoft.SME.Common.Resources.ValidateEmptyCode);
			}
			if (string.IsNullOrWhiteSpace(_storageExpense.Code))
			{
				throw new ASException(ArmSoft.SME.Common.Resources.ValidEmptyExpenseStorage);
			}
			MaterialCacheRow item = TradeSettings.TradeCacheService.Material.GetItem(mTMoveSpecificationRow.MTCode);
			decimal quantity = (mTMoveSpecificationRow.Quantity * PriceHelper.GetMTQntUnitCoeff(item.MTCode, mTMoveSpecificationRow.UnitCode)).RoundAs(3);
			MTSelectPartyWindow mTSelectPartyWindow = new MTSelectPartyWindow(_storageExpense.Code, _date.EditValue.Value, mTMoveSpecificationRow.MTCode, item.Unit, quantity, mTMoveSpecificationRow.RowID, PO.StoredInDb && !IsDraftOrDeleted)
			{
				MTExpDocSelectedPartiesList = PO.MTExpDocSelectedPartiesList
			};
			mTSelectPartyWindow.Load();
			mTSelectPartyWindow.Owner = this;
			if (!mTSelectPartyWindow.ShowDialog().Value)
			{
				return;
			}
			PO.MTExpDocSelectedPartiesList = mTSelectPartyWindow.MTExpDocSelectedPartiesList;
			if (mTSelectPartyWindow.IsPartySelected.HasValue)
			{
				mTMoveSpecificationRow.MTID = item.Id;
				mTMoveSpecificationRow.MTName = item.Caption;
				mTMoveSpecificationRow.Quantity = mTSelectPartyWindow.TotalExpQty;
				string unitCode = mTMoveSpecificationRow.UnitCode;
				mTMoveSpecificationRow.UnitCode = item.Unit;
				mTMoveSpecificationRow.UnitBrief = item.UnitRow.Brief;
				if (_distributeByFEFO)
				{
					UnitChanged(item.Unit, unitCode, mTMoveSpecificationRow, distributeparties: false);
				}
				mTMoveSpecificationRow.Summa = (mTSelectPartyWindow.TotalExpQty * mTMoveSpecificationRow.Price).RoundAs(2);
				mTMoveSpecificationRow.IsPartySelected = mTSelectPartyWindow.IsPartySelected.Value;
				mTMoveSpecificationRow.ValidateWithoutResponse();
				_mtMoveGrid.Grid.RefreshRow(e.RowHandle);
			}
		}
	}

	private void MTMoveTableView_CellValueChanged(object sender, CellValueChangedEventArgs e)
	{
		MTMoveSpecificationRow mtMoveGridRow = (MTMoveSpecificationRow)_mtMoveGrid.Grid.GetRow(e.RowHandle);
		if (mtMoveGridRow == null)
		{
			return;
		}
		string text = "";
		if (e.OldValue != null)
		{
			text = e.OldValue.ToString();
		}
		if (e.Column == _mtCodeCol)
		{
			try
			{
				if (e.Value == null || string.IsNullOrWhiteSpace(e.Value.ToString()))
				{
					PO.MTExpDocSelectedPartiesList.RemoveAll((PartyRow x) => x.GridRow == mtMoveGridRow.RowID);
					mtMoveGridRow.MTID = -1;
					mtMoveGridRow.MTCode = "";
					mtMoveGridRow.MTName = "";
					mtMoveGridRow.UnitCode = "";
					mtMoveGridRow.UnitBrief = "";
					mtMoveGridRow.RowDescription = "";
					mtMoveGridRow.Price = 0m;
					mtMoveGridRow.Summa = 0m;
					mtMoveGridRow.IsPartySelected = false;
				}
				else
				{
					if (e.OldValue != null && !string.IsNullOrWhiteSpace(text) && text != e.Value.ToString())
					{
						PO.MTExpDocSelectedPartiesList.RemoveAll((PartyRow x) => x.GridRow == mtMoveGridRow.RowID);
						mtMoveGridRow.Summa = 0m;
						mtMoveGridRow.IsPartySelected = false;
					}
					string text2 = e.Value.ToString();
					foreach (MTMoveSpecificationRow mtMove in _mtMoveList)
					{
						if (mtMoveGridRow != mtMove && text2 == mtMove.MTCode)
						{
							if (ArmSoft.SME.Core.UI.MessageBox.Show(string.Format(ArmSoft.SME.Trade.Common.Resources.MTExistInMTList, text2), Settings.ProgramName, MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.Yes) != MessageBoxResult.No)
							{
								break;
							}
							mtMoveGridRow.MTCode = "";
							return;
						}
					}
					if (string.IsNullOrWhiteSpace(text2))
					{
						throw new Exception();
					}
					MaterialCacheRow item = TradeSettings.TradeCacheService.Material.GetItem(text2, throwExceptionNotExits: true);
					mtMoveGridRow.MTID = item.Id;
					mtMoveGridRow.MTName = item.Caption;
					_unitCodeCol.DataView = new MTQntUnits(item.MTCode);
					mtMoveGridRow.IsPartySelected = false;
					mtMoveGridRow.UnitCode = item.Unit;
					mtMoveGridRow.UnitBrief = item.UnitRow.Brief;
					_unitCodeCol.DataView = new MTQntUnits(item.MTCode);
					if (!_storageExpenseIsRetail)
					{
						if (_storageIncomeIsRetail)
						{
							mtMoveGridRow.Price = PriceHelper.GetPriceForGivenPriceListAndVATType(_pricesTypes.Code, "2", mtMoveGridRow.MTID, mtMoveGridRow.UnitCode, _date.EditValue.Value, "AMD");
						}
					}
					else
					{
						MaterialCacheRow.QntUnitRow qntUnit = TradeSettings.TradeCacheService.Material.GetQntUnit(mtMoveGridRow.MTID, mtMoveGridRow.UnitCode);
						if (qntUnit != null)
						{
							MTRetailQtyAndPriceDataProvider.Param param = new MTRetailQtyAndPriceDataProvider.Param
							{
								MTCode = mtMoveGridRow.MTCode,
								Storage = _storageExpense.Code,
								Date = _date.EditValue,
								UnitCoef = qntUnit.Coef
							};
							List<MTRetailQtyAndPriceDataProvider.DataRow> rows = new MTRetailQtyAndPriceDataProvider(TradeSettings.CreateTradeClient()).GetRows(param);
							if (rows.Count > 0)
							{
								mtMoveGridRow.Price = rows[0].Price;
							}
							MTRetailQtyAndPrice dataView = new MTRetailQtyAndPrice(mtMoveGridRow.MTCode, qntUnit.Coef, _storageExpense.Code, _date.EditValue);
							_priceCol.DataView = dataView;
						}
					}
					mtMoveGridRow.Summa = (mtMoveGridRow.Price * mtMoveGridRow.Quantity).RoundAs(2);
					if (_distributeByFEFO)
					{
						decimal expSumm = 0m;
						DistributeQtyByExpiredDate(mtMoveGridRow.RowID, _storageExpense.Code, mtMoveGridRow.MTCode, mtMoveGridRow.UnitCode, mtMoveGridRow.Quantity, ref expSumm);
						mtMoveGridRow.Summa = expSumm;
						mtMoveGridRow.IsPartySelected = true;
					}
				}
				return;
			}
			catch (Exception ex)
			{
				mtMoveGridRow.MTID = -1;
				mtMoveGridRow.MTCode = "";
				mtMoveGridRow.MTName = "";
				mtMoveGridRow.UnitCode = "";
				mtMoveGridRow.UnitBrief = "";
				mtMoveGridRow.RowDescription = "";
				mtMoveGridRow.Quantity = 0m;
				mtMoveGridRow.Summa = 0m;
				if (e.Value != null && !string.IsNullOrWhiteSpace(e.Value.ToString()))
				{
					Settings.ErrorHandler(ex);
				}
				return;
			}
			finally
			{
				RowInfoChange?.Invoke(mtMoveGridRow);
			}
		}
		if (e.Column == _unitCodeCol)
		{
			try
			{
				if (e.Value != null && !string.IsNullOrWhiteSpace(e.Value.ToString()))
				{
					QntUnitCacheRow item2 = TradeSettings.TradeCacheService.QntUnit.GetItem(e.Value.ToString(), throwExceptionNotExits: true);
					mtMoveGridRow.UnitBrief = ((item2 == null) ? "" : item2.Brief);
					UnitChanged(e.Value.ToString(), text, mtMoveGridRow);
					if (!_storageExpenseIsRetail && _storageIncomeIsRetail)
					{
						if (string.IsNullOrWhiteSpace(_pricesTypes.Code))
						{
							mtMoveGridRow.Price = PriceHelper.GetPriceForUnit(text, Convert.ToString(e.Value), mtMoveGridRow.Price, mtMoveGridRow.MTCode);
						}
						else
						{
							PricesDataProvider.DataRow price = TradeSettings.CreateTradeClient().CommonServices.GetPrice(_pricesTypes.Code, mtMoveGridRow.MTID, e.Value.ToString(), _date.EditValue.Value, "AMD");
							if (price.Price != 0m)
							{
								mtMoveGridRow.Price = price.Price;
							}
							else
							{
								mtMoveGridRow.Price = PriceHelper.GetPriceForUnit(text, Convert.ToString(e.Value), mtMoveGridRow.Price, mtMoveGridRow.MTCode);
							}
						}
					}
					mtMoveGridRow.Summa = (mtMoveGridRow.Price * mtMoveGridRow.Quantity).RoundAs(2);
				}
				RowInfoChange?.Invoke(mtMoveGridRow);
				return;
			}
			catch (Exception ex2)
			{
				Settings.ErrorHandler(ex2);
				return;
			}
		}
		if (e.Column == _quantityCol)
		{
			if (Convert.ToDecimal(e.OldValue) != Convert.ToDecimal(e.Value))
			{
				if (Convert.ToDecimal(e.OldValue) != 0m)
				{
					if (_distributeByFEFO)
					{
						PO.MTExpDocSelectedPartiesList.RemoveAll((PartyRow x) => x.GridRow == mtMoveGridRow.RowID);
						decimal expSumm2 = 0m;
						DistributeQtyByExpiredDate(mtMoveGridRow.RowID, _storageExpense.Code, mtMoveGridRow.MTCode, mtMoveGridRow.UnitCode, Convert.ToDecimal(e.Value), ref expSumm2);
						mtMoveGridRow.Summa = expSumm2;
						mtMoveGridRow.IsPartySelected = true;
					}
					else
					{
						if (mtMoveGridRow.IsPartySelected)
						{
							ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Trade.Common.Resources.ForRowSelectedParties, Settings.ProgramName, MessageBoxButton.OK, MessageBoxImage.Asterisk);
						}
						PO.MTExpDocSelectedPartiesList.RemoveAll((PartyRow x) => x.GridRow == mtMoveGridRow.RowID);
						mtMoveGridRow.IsPartySelected = false;
					}
				}
				else if (_distributeByFEFO)
				{
					decimal expSumm3 = 0m;
					DistributeQtyByExpiredDate(mtMoveGridRow.RowID, _storageExpense.Code, mtMoveGridRow.MTCode, mtMoveGridRow.UnitCode, Convert.ToDecimal(e.Value), ref expSumm3);
					mtMoveGridRow.Summa = expSumm3;
					mtMoveGridRow.IsPartySelected = true;
				}
			}
			if (_storageExpenseIsRetail || _storageIncomeIsRetail)
			{
				mtMoveGridRow.Summa = mtMoveGridRow.Quantity * mtMoveGridRow.Price;
			}
			else if (!_distributeByFEFO)
			{
				mtMoveGridRow.Summa = 0m;
			}
		}
		else if (e.Column == _priceCol && Convert.ToDecimal(e.OldValue) != Convert.ToDecimal(e.Value))
		{
			mtMoveGridRow.Price = Convert.ToDecimal(e.Value);
			mtMoveGridRow.Summa = (mtMoveGridRow.Price * mtMoveGridRow.Quantity).RoundAs(2);
		}
	}

	private void StorageExpense_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		if (!_storageExpense.IsReadOnly && !string.IsNullOrWhiteSpace(Convert.ToString(e.NewValue.ToString())) && TradeSettings.TradeCacheService.Storage.GetItem(Convert.ToString(e.NewValue.ToString())) != null && !TradeSettings.CreateTradeClient().CommonServices.IsStorageAvailable(Convert.ToString(e.NewValue.ToString())))
		{
			e.Handled = true;
			_storageExpense.Code = Convert.ToString(e.OldValue);
			ArmSoft.SME.Core.UI.MessageBox.Show(string.Format(ArmSoft.SME.Trade.Common.Resources.StorageNotAvailForUser, Settings.LoginService.UserName, Convert.ToString(e.NewValue)), Settings.ProgramName, MessageBoxButton.OK, MessageBoxImage.Exclamation);
		}
		else
		{
			if (!(Convert.ToString(e.NewValue).TrimEnd() != Convert.ToString(e.OldValue).TrimEnd()))
			{
				return;
			}
			if (_storageExpense.DoValidate())
			{
				if (_storageExpense.Code == _storageIncome.Code.TrimEnd())
				{
					ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Trade.Common.Resources.MTMoveErrSameStorages, Settings.ProgramName, MessageBoxButton.OK, MessageBoxImage.Exclamation);
					_storageExpense.Code = "";
					return;
				}
				_storageExpenseIsRetail = TradeSettings.TradeCacheService.Storage.GetItem(_storageExpense.Code)?.Retail ?? false;
				if (_mtMoveList.Count > 0)
				{
					if (!_distributeByFEFO)
					{
						foreach (MTMoveSpecificationRow mtMove in _mtMoveList)
						{
							if (mtMove.IsPartySelected)
							{
								ArmSoft.SME.Core.UI.MessageBox.Show(string.Format(ArmSoft.SME.Trade.Common.Resources.PartiesValuesChanged, ArmSoft.SME.Common.Resources.Storage), Caption, MessageBoxButton.OK, MessageBoxImage.Asterisk);
								break;
							}
						}
					}
					if (_distributeByFEFO)
					{
						PO.MTExpDocSelectedPartiesList.Clear();
						decimal num = 0m;
						for (int i = 0; i <= _mtMoveList.Count - 1; i++)
						{
							num = 0m;
							DistributeQtyByExpiredDate(_mtMoveList[i].RowID, _storageExpense.Code, _mtMoveList[i].MTCode, _mtMoveList[i].UnitCode, _mtMoveList[i].Quantity, ref num);
							_mtMoveList[i].Summa = num;
							_mtMoveList[i].IsPartySelected = true;
						}
					}
					else
					{
						for (int j = 0; j <= _mtMoveList.Count - 1; j++)
						{
							_mtMoveList[j].IsPartySelected = false;
							_mtMoveList[j].Summa = 0m;
						}
						PO.MTExpDocSelectedPartiesList.Clear();
					}
					if (_storageExpenseIsRetail)
					{
						_pricesTypes.Code = "";
					}
				}
				_pricesTypes.IsReadOnly = _storageExpenseIsRetail;
				SetPriceSumColumnsReadOnlyProperty();
			}
			if (_storageExpenseIsRetail && _mtMoveList.Count != 0)
			{
				foreach (MTMoveSpecificationRow mtMove2 in _mtMoveList)
				{
					mtMove2.Price = 0m;
					mtMove2.Summa = 0m;
				}
			}
			RowInfoChange?.Invoke((MTMoveSpecificationRow)_mtMoveTableView.Grid.CurrentItem);
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
				SetPriceSumColumnsReadOnlyProperty();
			}
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
			string unitCode = "";
			string unitBrief = "";
			string barCode = _barCode.Text.Trim();
			MaterialCacheRow materialCacheRow = DocumentHelpers.TryGetItemByBarCode(ref barCode, out var qty, out unitCode, out unitBrief);
			if (materialCacheRow == null)
			{
				ControlsHelper.SetFocus(_barCode);
				throw new ASException(string.Format(ArmSoft.SME.Common.Resources.ErrBarCodeNotExist, _barCode.Text.Trim()));
			}
			MTMoveSpecificationRow gridRow = null;
			AddMTToList(materialCacheRow, ref gridRow, unitCode, unitBrief, qty);
			_barCode.Text = "";
			RowInfoChange?.Invoke(_mtMoveList[_mtMoveList.Count - 1]);
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	private void AddMTToList(MaterialCacheRow goodsRow, ref MTMoveSpecificationRow gridRow, string unitCode = "", string unitBrief = "", decimal quantity = 1m)
	{
		if (Settings.ParametersService.ScannerMode() == ScanerModesSE.AddNewRow)
		{
			AddMTToListNewRow(goodsRow, gridRow, string.IsNullOrWhiteSpace(unitCode) ? goodsRow.Unit : unitCode, string.IsNullOrWhiteSpace(unitBrief) ? goodsRow.UnitRow.Brief : unitBrief, quantity);
		}
		else
		{
			AddNewRowOrMergeInner(goodsRow, ref gridRow, Settings.ParametersService.ScannerMode() == ScanerModesSE.MergeWithCurrentRow, string.IsNullOrWhiteSpace(unitCode) ? goodsRow.Unit : unitCode, string.IsNullOrWhiteSpace(unitBrief) ? goodsRow.UnitRow.Brief : unitBrief, quantity);
		}
	}

	private void AddNewRowOrMergeInner(MaterialCacheRow goodRow, ref MTMoveSpecificationRow row, bool forFocusedRow, string unitCode, string unitBrief, decimal quantity = 1m)
	{
		if (forFocusedRow && _mtMoveList.Count != 0)
		{
			row = (MTMoveSpecificationRow)_mtMoveTableView.Grid.CurrentItem;
			if (row == null)
			{
				return;
			}
		}
		for (int i = 0; i <= _mtMoveList.Count - 1; i++)
		{
			if ((!forFocusedRow || row.RowID == _mtMoveList[i].RowID) && goodRow.Id == _mtMoveList[i].MTID && unitCode == _mtMoveList[i].UnitCode && !_mtMoveList[i].IsPartySelected)
			{
				row = _mtMoveList[i];
				FillMTInvGridRow(row, goodRow, unitCode, unitBrief, quantity);
				int rowHandleByListIndex = _mtMoveGrid.Grid.GetRowHandleByListIndex(i);
				POWindow<MTMove>.SelectCellInGrid(_mtMoveTableView, _mtMoveGrid, rowHandleByListIndex, "Quantity");
				return;
			}
		}
		AddMTToListNewRow(goodRow, row, unitCode, unitBrief, quantity);
		RowInfoChange?.Invoke(row);
	}

	private void AddMTToListNewRow(MaterialCacheRow goodRow, MTMoveSpecificationRow row, string unitCode, string unitBrief, decimal quantity = 1m)
	{
		row = new MTMoveSpecificationRow();
		FillMTInvGridRow(row, goodRow, unitCode, unitBrief, quantity);
		row.RowID = _maxRowID;
		_mtMoveList.Add(row);
		_maxRowID++;
		int rowHandleByListIndex = _mtMoveGrid.Grid.GetRowHandleByListIndex(_mtMoveList.Count - 1);
		if (row != null && _distributeByFEFO)
		{
			PO.MTExpDocSelectedPartiesList.RemoveAll((PartyRow x) => x.GridRow == row.RowID);
			decimal expSumm = 0m;
			DistributeQtyByExpiredDate(row.RowID, _storageExpense.Code, goodRow.MTCode, row.UnitCode, row.Quantity, ref expSumm, goodRow.Unit);
			row.Summa = expSumm;
			row.IsPartySelected = true;
		}
		POWindow<MTMove>.SelectCellInGrid(_mtMoveTableView, _mtMoveGrid, rowHandleByListIndex, "Quantity");
		_mtMoveGrid.ConfigureButtons();
		RowInfoChange?.Invoke(row);
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
			if (mtMoveGridRow.Price == 0m)
			{
				mtMoveGridRow.Price = GetRowPrice(mtMoveGridRow);
			}
			mtMoveGridRow.Summa = (mtMoveGridRow.Price * mtMoveGridRow.Quantity).RoundAs(2);
			mtMoveGridRow.IsPartySelected = false;
		}
	}

	private decimal GetRowPrice(MTMoveSpecificationRow row, bool fromMoveOrder = false)
	{
		if (!_storageExpenseIsRetail)
		{
			if (_storageIncomeIsRetail)
			{
				decimal priceForGivenPriceListAndVATType = TradeSettings.CreateTradeClient().CommonServices.GetPriceForGivenPriceListAndVATType(_pricesTypes.Code, "2", row.MTID, row.UnitCode, _date.EditValue.Value, "AMD");
				if (priceForGivenPriceListAndVATType != 0m)
				{
					return priceForGivenPriceListAndVATType;
				}
				if (fromMoveOrder)
				{
					MaterialCacheRow.QntUnitRow qntUnit = TradeSettings.TradeCacheService.Material.GetQntUnit(row.MTID, row.UnitCode);
					if (qntUnit != null && qntUnit != null)
					{
						MTRetailQtyAndPriceDataProvider.Param param = new MTRetailQtyAndPriceDataProvider.Param
						{
							MTCode = row.MTCode,
							Storage = _storageIncome.Code,
							Date = _date.EditValue,
							UnitCoef = qntUnit.Coef
						};
						List<MTRetailQtyAndPriceDataProvider.DataRow> rows = new MTRetailQtyAndPriceDataProvider(TradeSettings.CreateTradeClient()).GetRows(param);
						if (rows.Count > 0)
						{
							return rows[0].Price;
						}
					}
				}
			}
		}
		else
		{
			MaterialCacheRow.QntUnitRow qntUnit2 = TradeSettings.TradeCacheService.Material.GetQntUnit(row.MTID, row.UnitCode);
			if (qntUnit2 != null && qntUnit2 != null)
			{
				MTRetailQtyAndPriceDataProvider.Param param2 = new MTRetailQtyAndPriceDataProvider.Param
				{
					MTCode = row.MTCode,
					Storage = _storageExpense.Code,
					Date = _date.EditValue,
					UnitCoef = qntUnit2.Coef
				};
				List<MTRetailQtyAndPriceDataProvider.DataRow> rows2 = new MTRetailQtyAndPriceDataProvider(TradeSettings.CreateTradeClient()).GetRows(param2);
				if (rows2.Count > 0)
				{
					return rows2[0].Price;
				}
			}
		}
		return 0m;
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
		Ribbon.ToolbarItemLinks.Add(_barSubItemActions.ConvertForToolbar("t_action_16.svg"));
		Ribbon.ToolbarItemLinks.Add(_barButtonImportFromFile.ConvertForToolbar("t_import_16.svg"));
		Ribbon.ToolbarItemLinks.Add(_barSubItemJournals.ConvertForToolbar("t_report_16.svg"));
		Ribbon.ToolbarItemLinks.Add(_barButtonDocumentSettings.ConvertForToolbar("t_page_white_gear_16.svg"));
	}

	protected override void AddItemsToRibbon()
	{
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00be: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00df: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_011a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0120: Unknown result type (might be due to invalid IL or missing references)
		//IL_0136: Unknown result type (might be due to invalid IL or missing references)
		//IL_013c: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_0373: Unknown result type (might be due to invalid IL or missing references)
		//IL_0379: Unknown result type (might be due to invalid IL or missing references)
		//IL_038f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0395: Unknown result type (might be due to invalid IL or missing references)
		base.AddItemsToRibbon();
		CreateRibbonPageGroupActions();
		CreateRibbonPageGroupReports();
		_barSubItemJournals = new BarSubItem
		{
			Content = "",
			Glyph = ImageService.GetImage("report_16.svg"),
			LargeGlyph = ImageService.GetImage("report_32.svg")
		};
		BarSubItem barSubItem = new BarSubItem();
		_barButtonJumpTo = new ReportBarButtonItem(DataProviderTypeSE.AllDocumentsJournal, showDialog: false, ArmSoft.SME.Common.Resources.JumpTo, null, "doc_JumpTo_16.svg", "doc_JumpTo_32.svg");
		_barButtonJumpTo.AddInBarSubItem(ref _ribbonPageGroupActions, ref barSubItem);
		_barSubItemActions.Content = "";
		_barSubItemActions.Glyph = ImageService.GetImage("action_16.svg");
		_barSubItemActions.LargeGlyph = ImageService.GetImage("action_32.svg");
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
		BarItemLink barItemLink = _barButtonPrintLabel.CreateLink();
		barItemLink.MergeType = BarItemMergeType.Replace;
		_barSubItemActions.ItemLinks.Add(barItemLink);
		_barButtonPriceFormation = new BarButtonItem
		{
			Content = ArmSoft.SME.Trade.Common.Resources.PricesFormation,
			KeyGesture = new KeyGesture((Key)53, (ModifierKeys)2),
			Glyph = ImageService.GetImage("action_16.svg"),
			LargeGlyph = ImageService.GetImage("action_32.svg")
		};
		_barManager.Items.Add(_barButtonPriceFormation);
		_barButtonPriceFormation.ItemClick += new SafeDocumentEventHandler<ItemClickEventArgs>(BarButtonPriceFormation_ItemClick, this).Handler;
		barItemLink = _barButtonPriceFormation.CreateLink();
		barItemLink.MergeType = BarItemMergeType.Replace;
		_barSubItemActions.ItemLinks.Add(barItemLink);
		_ribbonPageGroupActions.ItemLinks.Add(_barButtonPriceFormation.CreateLink());
		_barButtonDocumentSettings = new BarButtonItem
		{
			Content = ArmSoft.SME.Common.Resources.Settings,
			Glyph = ImageService.GetImage("page_white_gear_16.svg"),
			LargeGlyph = ImageService.GetImage("page_white_gear_32.svg")
		};
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
		ReportBarButtonItem barButtonGoodsRetailOperationJournal = _barButtonGoodsRetailOperationJournal;
		num = 2;
		List<PropertyWithValue> list3 = new List<PropertyWithValue>(num);
		CollectionsMarshal.SetCount(list3, num);
		Span<PropertyWithValue> span3 = CollectionsMarshal.AsSpan(list3);
		span3[0] = new PropertyWithValue("DocISN", PO.ISN.ToString());
		span3[1] = new PropertyWithValue("AdditionalCaption", GetReportsAdditionalCaption());
		barButtonGoodsRetailOperationJournal.PropertyWithValues = list3;
	}

	private void BarButtonPrintLabel_ItemClick(object sender, ItemClickEventArgs e)
	{
		if (_mtMoveList.Count > 0)
		{
			FillDocument(new StoreRequest());
			new PrintLabelDialog(this, PO).Invoke();
		}
	}

	private void BarButtonPriceFormation_ItemClick(object sender, ItemClickEventArgs e)
	{
		List<PricesFormationWindow.GoodsRow> list = new List<PricesFormationWindow.GoodsRow>();
		List<SelectedMaterialsRow> list2 = new List<SelectedMaterialsRow>();
		foreach (MTMoveSpecificationRow mtMove in _mtMoveList)
		{
			MaterialCacheRow item = TradeSettings.TradeCacheService.Material.GetItem(mtMove.MTCode);
			list.Add(new PricesFormationWindow.GoodsRow(mtMove.MTID, mtMove.MTCode, mtMove.UnitCode, item.AltUnit));
			SelectedMaterialsRow selectedMaterialsRow = new SelectedMaterialsRow
			{
				RowNum = mtMove.RowNum,
				MTID = mtMove.MTID,
				MTCode = mtMove.MTCode,
				MTName = mtMove.MTName
			};
			if (mtMove.UnitCode != item.Unit)
			{
				MaterialCacheRow.QntUnitRow qntUnit = TradeSettings.TradeCacheService.Material.GetQntUnit(mtMove.MTID, mtMove.UnitCode);
				MaterialCacheRow.QntUnitRow qntUnit2 = TradeSettings.TradeCacheService.Material.GetQntUnit(mtMove.MTID, item.Unit);
				if (qntUnit != null && qntUnit2 != null)
				{
					selectedMaterialsRow.QntUnit = qntUnit2.QntUnit;
					selectedMaterialsRow.QntUnitName = TradeSettings.TradeCacheService.QntUnit.GetItem(qntUnit2.QntUnit).Brief;
					if (qntUnit.Coef != 0m)
					{
						selectedMaterialsRow.PurchasePrice = (mtMove.Price * qntUnit2.Coef / qntUnit.Coef).RoundAs(2);
						selectedMaterialsRow.SalePrice = selectedMaterialsRow.PurchasePrice;
					}
				}
			}
			else
			{
				selectedMaterialsRow.QntUnit = mtMove.UnitCode;
				selectedMaterialsRow.QntUnitName = mtMove.UnitBrief;
				selectedMaterialsRow.PurchasePrice = mtMove.Price;
				selectedMaterialsRow.SalePrice = mtMove.Price;
			}
			list2.Add(selectedMaterialsRow);
		}
		new PricesFormationWindow(list, "PricesFormation" + Name, showPurchasePriceRow: true, showSalePriceRow: false, showRetailAvailabilityPriceRow: false, "AMD", list2, this).Invoke();
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
			if (_mtMoveList.Count <= 0 || ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Trade.Common.Resources.LoadMaterialsList, Settings.ProgramName, MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No) != MessageBoxResult.No)
			{
				_mtMoveList.Clear();
				PO.MTExpDocSelectedPartiesList.Clear();
				_maxRowID = 0;
				TerminalHelper.TerminalImporter terminalImporter = new TerminalHelper.TerminalImporter(this);
				MTMoveSpecificationRow MTMoveGridRow = null;
				if (terminalImporter.ImportFromFile((decimal quantity, MaterialCacheRow goodRow, MaterialCacheRow.BarCodeRow goodBarcodeRow) =>
				{
					AddMTToListNewRow(goodRow, MTMoveGridRow, goodBarcodeRow.QntUnit, TradeSettings.TradeCacheService.QntUnit.GetItem(goodBarcodeRow.QntUnit).Brief, quantity);
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

	protected override bool AllowChangeGridMode()
	{
		return true;
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

	private void BaseDocument_EditKeyValueChanged(object sender, EditKeyValueChangedEventArgs e)
	{
		BaseISN = ((e.KeyValue != null) ? ((Guid)e.KeyValue) : Guid.Empty);
	}

	private void BaseDocumentDelete_Click(object sender, EditKeyValueChangedEventArgs e)
	{
		_baseDocumentControl.KeyValue = Guid.Empty;
		_baseDocumentControl.DoValidate();
	}

	private void BaseDocumentSearch_Click(object sender, EditKeyValueChangedEventArgs e)
	{
		if (BaseISN != Guid.Empty)
		{
			Settings.DocumentService.ShowWindow(BaseISN, WindowViewMode.Edit);
		}
	}

	protected override void CreateChildDocument(short childDocType, MTMove baseDoc)
	{
		base.CreateChildDocument(childDocType, baseDoc);
		if (childDocType == DocumentTypeSE.MTRevaluation.Value && (State == DocumentStateSE.Draft.Value || State == DocumentStateSE.Deleted.Value) && StoredInDb)
		{
			ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Common.Resources.ForActionDocMustBeRegistered, Settings.ProgramName, MessageBoxButton.OK, MessageBoxImage.Exclamation);
		}
		else
		{
			CreateBasedOnDocHelper.CreateDocument((DocumentTypeSE)(SmartEnum<DocumentTypeSE, short>)childDocType, PO.ISN, DVPID);
		}
	}

	private void CreateBasedOnDoc()
	{
		_comment.EditValue = BaseDocument.Comment;
		_mtMoveList.Clear();
		_maxRowID = 0;
		PO.MTExpDocSelectedPartiesList.Clear();
		if (BaseDocument.POType == DocumentTypeSE.MTIncomeImport.Value)
		{
			MTIncomeImport mTIncomeImport = (MTIncomeImport)BaseDocument;
			_storageExpense.Code = mTIncomeImport.Storage;
			_storageExpenseIsRetail = TradeSettings.TradeCacheService.Storage.GetItem(_storageExpense.Code)?.Retail ?? false;
			if (!string.IsNullOrWhiteSpace(_storageIncome.Code))
			{
				_storageIncomeIsRetail = TradeSettings.TradeCacheService.Storage.GetItem(_storageIncome.Code)?.Retail ?? false;
			}
			if (!string.IsNullOrWhiteSpace(mTIncomeImport.Mediator))
			{
				EmployeeCacheRow item = TradeSettings.TradeCacheService.Employee.GetItem(mTIncomeImport.Mediator);
				if (item != null)
				{
					_mediator.Text = item.Caption;
				}
			}
			_allower.Text = mTIncomeImport.Allower;
			_chiefAcc.Text = mTIncomeImport.Chief;
			foreach (MTIncomeImport.MTIncomeImportSpecificationRow mT in mTIncomeImport.MTList)
			{
				_mtMoveList.Add(new MTMoveSpecificationRow
				{
					MTCode = mT.MTCode,
					MTName = mT.MTName,
					Quantity = mT.Quantity,
					MTID = mT.MTID,
					UnitCode = mT.UnitCode,
					UnitBrief = mT.UnitBrief,
					IsPartySelected = false,
					RowID = _maxRowID
				});
				_maxRowID++;
			}
		}
		else if (BaseDocument.POType == DocumentTypeSE.MTMoveOrder.Value)
		{
			MTMoveOrder mTMoveOrder = (MTMoveOrder)BaseDocument;
			_storageExpense.Code = mTMoveOrder.StorageExpense;
			_storageIncome.Code = mTMoveOrder.StorageIncome;
			_date.EditValue = Settings.HostingEnvironment.CurrentDate;
			_storageExpenseIsRetail = TradeSettings.TradeCacheService.Storage.GetItem(_storageExpense.Code)?.Retail ?? false;
			_storageIncomeIsRetail = TradeSettings.TradeCacheService.Storage.GetItem(_storageIncome.Code)?.Retail ?? false;
			foreach (MTMoveBase.MTMoveSpecificationRow mT2 in mTMoveOrder.MTList)
			{
				MTMoveSpecificationRow mTMoveSpecificationRow = new MTMoveSpecificationRow();
				mTMoveSpecificationRow.MTCode = mT2.MTCode;
				mTMoveSpecificationRow.MTName = mT2.MTName;
				mTMoveSpecificationRow.Quantity = mT2.Quantity;
				mTMoveSpecificationRow.MTID = mT2.MTID;
				mTMoveSpecificationRow.UnitCode = mT2.UnitCode;
				mTMoveSpecificationRow.UnitBrief = mT2.UnitBrief;
				mTMoveSpecificationRow.Price = GetRowPrice(mTMoveSpecificationRow, fromMoveOrder: true);
				mTMoveSpecificationRow.Summa = (mTMoveSpecificationRow.Price * mTMoveSpecificationRow.Quantity).RoundAs(2);
				mTMoveSpecificationRow.IsPartySelected = false;
				mTMoveSpecificationRow.RowID = _maxRowID;
				_mtMoveList.Add(mTMoveSpecificationRow);
				_maxRowID++;
			}
		}
		else if (BaseDocument.POType == DocumentTypeSE.InvoiceForPay.Value)
		{
			InvoiceForPay invoiceForPay = (InvoiceForPay)BaseDocument;
			_pricesTypes.Code = invoiceForPay.PriceListType;
			_storageExpense.Code = invoiceForPay.Storage;
			foreach (MTInvoiceBase.MTInvoiceSpecificationRow mT3 in invoiceForPay.MTList)
			{
				MTMoveSpecificationRow mTMoveSpecificationRow2 = new MTMoveSpecificationRow
				{
					MTCode = mT3.ItemCode,
					MTName = mT3.ItemName,
					Quantity = mT3.Quantity,
					MTID = mT3.ItemID,
					UnitCode = mT3.UnitCode,
					UnitBrief = mT3.UnitBrief,
					IsPartySelected = false,
					RowID = _maxRowID
				};
				mTMoveSpecificationRow2.Price = GetRowPrice(mTMoveSpecificationRow2, fromMoveOrder: true);
				mTMoveSpecificationRow2.Summa = (mTMoveSpecificationRow2.Price * mTMoveSpecificationRow2.Quantity).RoundAs(2);
				_mtMoveList.Add(mTMoveSpecificationRow2);
				_maxRowID++;
			}
		}
		else if (BaseDocument.POType == DocumentTypeSE.MTIncome.Value || BaseDocument.POType == DocumentTypeSE.MTIncomeRetail.Value)
		{
			MTIncomeBase mTIncomeBase = (MTIncomeBase)BaseDocument;
			_storageExpense.Code = mTIncomeBase.Storage;
			_storageExpenseIsRetail = TradeSettings.TradeCacheService.Storage.GetItem(_storageExpense.Code)?.Retail ?? false;
			if (!string.IsNullOrWhiteSpace(_storageIncome.Code))
			{
				_storageIncomeIsRetail = TradeSettings.TradeCacheService.Storage.GetItem(_storageIncome.Code)?.Retail ?? false;
			}
			if (!string.IsNullOrWhiteSpace(mTIncomeBase.Mediator))
			{
				EmployeeCacheRow item2 = TradeSettings.TradeCacheService.Employee.GetItem(mTIncomeBase.Mediator);
				if (item2 != null)
				{
					_mediator.Text = item2.Caption;
				}
			}
			_allower.Text = mTIncomeBase.Allower;
			_chiefAcc.Text = mTIncomeBase.Chief;
			foreach (MTIncomeBase.MTIncomeSpecificationRow mT4 in mTIncomeBase.MTList)
			{
				MTMoveSpecificationRow mTMoveSpecificationRow3 = new MTMoveSpecificationRow();
				mTMoveSpecificationRow3.MTCode = mT4.MTCode;
				mTMoveSpecificationRow3.MTName = mT4.MTName;
				mTMoveSpecificationRow3.Quantity = mT4.Quantity;
				mTMoveSpecificationRow3.MTID = mT4.MTID;
				mTMoveSpecificationRow3.UnitCode = mT4.UnitCode;
				mTMoveSpecificationRow3.UnitBrief = mT4.UnitBrief;
				if (BaseDocument.POType == DocumentTypeSE.MTIncomeRetail.Value)
				{
					mTMoveSpecificationRow3.Price = mT4.SalePrice;
					mTMoveSpecificationRow3.Summa = mT4.SaleSumma;
				}
				mTMoveSpecificationRow3.IsPartySelected = false;
				mTMoveSpecificationRow3.RowID = _maxRowID;
				_mtMoveList.Add(mTMoveSpecificationRow3);
				_maxRowID++;
			}
		}
		else if (BaseDocument.POType == DocumentTypeSE.MTMoveWithConfirmation.Value)
		{
			MTMoveWithConfirmation mTMoveWithConfirmation = (MTMoveWithConfirmation)BaseDocument;
			_pricesTypes.Code = mTMoveWithConfirmation.PriceType;
			_storageExpense.Code = "";
			_storageIncome.Code = "";
			if (!_createForSurplus.HasValue)
			{
				FillByStockTakingResultsDialogWindow fillByStockTakingResultsDialogWindow = new FillByStockTakingResultsDialogWindow();
				if (Convert.ToBoolean(fillByStockTakingResultsDialogWindow.ShowDialog()))
				{
					_createForSurplus = fillByStockTakingResultsDialogWindow.CreateForSurplus;
				}
			}
			if (!_createForSurplus.HasValue)
			{
				return;
			}
			if (_createForSurplus.Value)
			{
				_storageExpense.Code = mTMoveWithConfirmation.StorageExpense;
				_storageIncome.Code = mTMoveWithConfirmation.StorageIncome;
			}
			else
			{
				_storageExpense.Code = mTMoveWithConfirmation.StorageIncome;
				_storageIncome.Code = mTMoveWithConfirmation.StorageExpense;
			}
			if (!string.IsNullOrWhiteSpace(_storageExpense.Code))
			{
				_storageExpenseIsRetail = TradeSettings.TradeCacheService.Storage.GetItem(_storageExpense.Code)?.Retail ?? false;
			}
			if (!string.IsNullOrWhiteSpace(_storageIncome.Code))
			{
				_storageIncomeIsRetail = TradeSettings.TradeCacheService.Storage.GetItem(_storageIncome.Code)?.Retail ?? false;
			}
			foreach (MTMoveBase.MTMoveSpecificationRow item3 in mTMoveWithConfirmation.MTList.Where((MTMoveBase.MTMoveSpecificationRow x) => (!_createForSurplus.Value) ? (x.ActualQuantity < x.Quantity) : (x.ActualQuantity > x.Quantity)))
			{
				MTMoveSpecificationRow mTMoveSpecificationRow4 = new MTMoveSpecificationRow
				{
					MTCode = item3.MTCode,
					MTID = item3.MTID,
					MTName = item3.MTName,
					UnitCode = item3.UnitCode,
					UnitBrief = item3.UnitBrief,
					Quantity = Math.Abs(item3.ActualQuantity - item3.Quantity),
					RowID = _maxRowID
				};
				decimal valueOrDefault = item3.Price.GetValueOrDefault();
				mTMoveSpecificationRow4.Price = ((valueOrDefault != 0m) ? valueOrDefault : GetRowPrice(mTMoveSpecificationRow4));
				mTMoveSpecificationRow4.Summa = (mTMoveSpecificationRow4.Price * mTMoveSpecificationRow4.Quantity).RoundAs(2);
				_mtMoveList.Add(mTMoveSpecificationRow4);
				_maxRowID++;
			}
		}
		if (_storageExpenseIsRetail)
		{
			_pricesTypes.Code = "";
		}
		_pricesTypes.IsReadOnly = _storageExpenseIsRetail;
		SetPriceSumColumnsReadOnlyProperty();
		_mtMoveGrid.Grid.RefreshData();
	}

	public void SetPriceSumColumnsReadOnlyProperty()
	{
		if (_storageExpenseIsRetail)
		{
			_priceCol.AllowEditing = DefaultBoolean.False;
		}
		else if (_storageIncomeIsRetail && Settings.ParametersService.AllowChangeSalePrices())
		{
			_priceCol.AllowEditing = DefaultBoolean.True;
		}
		else
		{
			_priceCol.AllowEditing = DefaultBoolean.False;
		}
		_priceCol.Visible = _storageExpenseIsRetail || _storageIncomeIsRetail;
		_summaCol.Visible = _priceCol.Visible;
		_summaCol.ShowInColumnChooser = _priceCol.Visible;
		_priceCol.ShowInColumnChooser = _priceCol.Visible;
	}

	private void PricesTypes_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		if (_storageIncomeIsRetail && _isAskQuestionConfirmSummChange && _mtMoveList.Count != 0 && Convert.ToString(e.NewValue) != Convert.ToString(e.OldValue) && !string.IsNullOrWhiteSpace(_pricesTypes.Code) && _pricesTypes.DoValidate() && ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Trade.Common.Resources.ValidateChangePrices, Settings.ProgramName, MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No) == MessageBoxResult.Yes)
		{
			UpdateMTMovePrices();
		}
	}

	private void UpdateMTMovePrices()
	{
		if (_mtMoveList.Count == 0)
		{
			return;
		}
		foreach (MTMoveSpecificationRow mtMove in _mtMoveList)
		{
			mtMove.Price = PriceHelper.GetPriceForGivenPriceListAndVATType(_pricesTypes.Code, "2", mtMove.MTID, mtMove.UnitCode, _date.EditValue.Value, "AMD");
			mtMove.Summa = (mtMove.Price * mtMove.Quantity).RoundAs(2);
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
			SearchDataView = new ArmSoft.SME.Trade.UI.DropDown.GoodsFilter(shortMode: true, ItemTypesSE.Goods.Value),
			ResultsList = new MTMoveFilteredSearchList()
		}, this, isBasketMode: true);
		if (filteredSearchWindow.ShowDialog().Value)
		{
			foreach (MTMoveSpecificationRow item in filteredSearchWindow.BasketResult)
			{
				if (_storageExpenseIsRetail)
				{
					MaterialCacheRow.QntUnitRow qntUnit = TradeSettings.TradeCacheService.Material.GetQntUnit(item.MTID, item.UnitCode);
					if (qntUnit != null)
					{
						MTRetailQtyAndPriceDataProvider.Param param = new MTRetailQtyAndPriceDataProvider.Param
						{
							MTCode = item.MTCode,
							UnitCoef = qntUnit.Coef,
							Storage = _storageExpense.Code,
							Date = _date.EditValue
						};
						List<MTRetailQtyAndPriceDataProvider.DataRow> rows = new MTRetailQtyAndPriceDataProvider(TradeSettings.CreateTradeClient()).GetRows(param);
						if (rows.Count > 0)
						{
							item.Price = rows[0].Price;
						}
					}
				}
				else if (_storageIncomeIsRetail)
				{
					item.Price = PriceHelper.GetPriceForGivenPriceListAndVATType(_pricesTypes.Code, "2", item.MTID, item.UnitCode, _date.EditValue.Value, "AMD");
				}
				item.Summa = (item.Price * item.Quantity).RoundAs(2);
				item.RowID = _maxRowID;
				_mtMoveList.Add(item);
				if (_distributeByFEFO)
				{
					decimal expSumm = 0m;
					DistributeQtyByExpiredDate(item.RowID, _storageExpense.Code, item.MTCode, item.UnitCode, item.Quantity, ref expSumm);
					item.Summa = expSumm;
					item.IsPartySelected = true;
				}
				_maxRowID++;
			}
		}
		RowInfoChange?.Invoke((MTMoveSpecificationRow)_mtMoveTableView.Grid.CurrentItem);
	}

	private void MTMoveGrid_RefreshButtons(object sender, EventArgs e)
	{
		_filteredSearch.IsEnabled = _mtMoveGrid.AllowAdd && !_mtMoveGrid.IsReadOnly;
		_actualQuantitiesCheck.IsEnabled = _mtMoveGrid.AllowAdd && !_mtMoveGrid.IsReadOnly;
	}

	private void ActualQuantitiesCheck_Click(object sender, RoutedEventArgs e)
	{
		try
		{
			ValidateDocFields();
			if ((!PO.StoredInDb || IsChanged) && !SaveDocument(isAsDraft: true))
			{
				return;
			}
			ActualQuantitiesCheckWindow actualQuantitiesCheckWindow = new ActualQuantitiesCheckWindow(this, _mtMoveList, _date.EditValue.Value, _docNum.EditValue.ToString());
			if (!actualQuantitiesCheckWindow.ShowDialog().Value)
			{
				return;
			}
			foreach (ActualQuantitiesCheck.ActualQuantitiesCheckRow item2 in actualQuantitiesCheckWindow.QCResult)
			{
				if (item2.VarianceQuantity == 0m)
				{
					continue;
				}
				if (!item2.UserAdd)
				{
					for (int i = 0; i <= _mtMoveList.Count - 1; i++)
					{
						if (item2.MTID != _mtMoveList[i].MTID || !(item2.UnitCode == _mtMoveList[i].UnitCode))
						{
							continue;
						}
						MTMoveSpecificationRow mTMoveSpecificationRow = _mtMoveList[i];
						decimal num = mTMoveSpecificationRow.Quantity + item2.VarianceQuantity;
						TradeSettings.TradeCacheService.Material.GetItem(item2.MTCode);
						MTMoveSpecificationRow mtMoveGridRow = (MTMoveSpecificationRow)_mtMoveGrid.Grid.GetRowByListIndex(i);
						if (num < 0m)
						{
							mTMoveSpecificationRow.Quantity = 0m;
							if (_distributeByFEFO)
							{
								PO.MTExpDocSelectedPartiesList.RemoveAll((PartyRow x) => x.GridRow == mtMoveGridRow.RowID);
								decimal expSumm = 0m;
								DistributeQtyByExpiredDate(mtMoveGridRow.RowID, _storageExpense.Code, mtMoveGridRow.MTCode, mtMoveGridRow.UnitCode, mTMoveSpecificationRow.Quantity, ref expSumm);
								mtMoveGridRow.Summa = expSumm;
								mtMoveGridRow.IsPartySelected = true;
							}
							else
							{
								PO.MTExpDocSelectedPartiesList.RemoveAll((PartyRow x) => x.GridRow == mtMoveGridRow.RowID);
								mtMoveGridRow.IsPartySelected = false;
							}
							if (_storageExpenseIsRetail || _storageIncomeIsRetail)
							{
								mtMoveGridRow.Summa = mtMoveGridRow.Quantity * mtMoveGridRow.Price;
							}
							else if (!_distributeByFEFO)
							{
								mtMoveGridRow.Summa = 0m;
							}
							item2.VarianceQuantity = num;
							continue;
						}
						mTMoveSpecificationRow.Quantity = item2.ActualQuantity;
						if (_distributeByFEFO)
						{
							PO.MTExpDocSelectedPartiesList.RemoveAll((PartyRow x) => x.GridRow == mtMoveGridRow.RowID);
							decimal expSumm2 = 0m;
							DistributeQtyByExpiredDate(mtMoveGridRow.RowID, _storageExpense.Code, mtMoveGridRow.MTCode, mtMoveGridRow.UnitCode, mTMoveSpecificationRow.Quantity, ref expSumm2);
							mtMoveGridRow.Summa = expSumm2;
							mtMoveGridRow.IsPartySelected = true;
						}
						else
						{
							PO.MTExpDocSelectedPartiesList.RemoveAll((PartyRow x) => x.GridRow == mtMoveGridRow.RowID);
							mtMoveGridRow.IsPartySelected = false;
						}
						if (_storageExpenseIsRetail || _storageIncomeIsRetail)
						{
							mtMoveGridRow.Summa = mtMoveGridRow.Quantity * mtMoveGridRow.Price;
						}
						else if (!_distributeByFEFO)
						{
							mtMoveGridRow.Summa = 0m;
						}
						item2.VarianceQuantity = 0m;
						break;
					}
				}
				else
				{
					MaterialCacheRow item = TradeSettings.TradeCacheService.Material.GetItem(item2.MTCode);
					MTMoveSpecificationRow row = null;
					AddNewRowOrMergeInner(item, ref row, forFocusedRow: false, item2.UnitCode, item2.UnitBrief, item2.VarianceQuantity);
				}
			}
			IsChanged = true;
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	protected override void OnF8()
	{
		OnCodeEnterClick();
	}

	private void OnCodeEnterClick()
	{
		if (_codeEnter == null || !_codeEnter.IsEnabled)
		{
			return;
		}
		_barCode.DoValidate();
		string text = _barCode.Text;
		if (string.IsNullOrWhiteSpace(text))
		{
			if (!_date.DoValidate() || !_date.EditValue.HasValue)
			{
				ControlsHelper.SetFocus(_date);
				throw new ASException(ArmSoft.SME.Common.Resources.ValidateEmptyDate);
			}
			ArmSoft.SME.Trade.UI.DropDown.GoodsFilter goodsFilter = new ArmSoft.SME.Trade.UI.DropDown.GoodsFilter(shortMode: true, ItemTypesSE.Goods.Value);
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
		MTMoveSpecificationRow gridRow = null;
		AddMTToList(item, ref gridRow);
		_barCode.Text = "";
		RowInfoChange?.Invoke(_mtMoveList[_mtMoveList.Count - 1]);
	}

	private void CodeEnter_Click(object sender, RoutedEventArgs e)
	{
		OnCodeEnterClick();
	}

	private void BarButtonDocumentSettings_ItemClick(object sender, ItemClickEventArgs e)
	{
		DialogWindow dialogWindow = new DialogWindow
		{
			Caption = ArmSoft.SME.Common.Resources.Settings,
			Owner = this
		};
		LayoutGroup layoutGroup = dialogWindow.AddHorizontalLayoutGroup();
		layoutGroup.Orientation = Orientation.Vertical;
		layoutGroup.HorizontalAlignment = HorizontalAlignment.Stretch;
		layoutGroup.Header = ArmSoft.SME.Common.Resources.ItemList;
		layoutGroup.View = LayoutGroupView.GroupBox;
		dialogWindow.AddBooleanControl(layoutGroup, "DistributByFEFO", ArmSoft.SME.Trade.Common.Resources.FEFODistibuteLong).IsChecked = _distributeByFEFO;
		if (dialogWindow.ShowDialog().Value)
		{
			_distributeByFEFO = (bool)dialogWindow["DistributByFEFO"];
		}
	}

	protected override void SetReportBarButtonsEnable(bool isEnabled)
	{
		base.SetReportBarButtonsEnable(isEnabled);
		_barButtonJumpTo.IsEnabled = isEnabled;
		_barButtonGoodsOperationsJournal.IsEnabled = isEnabled;
		_barButtonGoodsRetailOperationJournal.IsEnabled = isEnabled;
	}

	private void ChangeRowInfo(MTMoveSpecificationRow Row = null)
	{
		if (Row == null)
		{
			((DispatcherObject)this).Dispatcher.BeginInvoke((Delegate)new DelegateClearInfoPanel(CleanInfoPanel), Array.Empty<object>());
			return;
		}
		((DispatcherObject)this).Dispatcher.BeginInvoke((Delegate)new DelegateSetRowInfoToInfoPanel(SetRowInfoToPanel), new object[1] { Row });
	}

	private void SetRowInfoToPanel(MTMoveSpecificationRow row)
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

	private void SetAvailabilityInfoToPanel(MTMoveSpecificationRow row)
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
			_infoDisplayControls.SetAccBalance(string.IsNullOrWhiteSpace(_storageExpense.Code) ? 0m : MaterialHelpers.GetGoodRemains(row.UpdateRemainsOnInfoPanel, row.MTID, row.UnitCode, ref _availabilityByGoodId, _storageExpense.Code));
			_infoDisplayControls.SetAccUnitBrief(row.UnitBrief);
			_infoDisplayControls.SetWholeBalance(MaterialHelpers.GetGoodRemains(row.UpdateRemainsOnInfoPanel, row.MTID, row.UnitCode, ref _availabilityByGoodId));
			_infoDisplayControls.SetWholeUnitBrief(row.UnitBrief);
			row.UpdateRemainsOnInfoPanel = false;
		}
	}

	private void CleanInfoPanel()
	{
		_isChanged = IsChanged;
		_infoItemName.Text = "";
		_infoDisplayControls.ClearInfoAvailability();
		IsChanged = _isChanged;
	}

	private void InfoDisplay_Collapsed(object sender, EventArgs e)
	{
		_infoDisplayControls.InfoPanelCollapsed = true;
	}

	private void MTMoveTableView_FocusedRowHandleChanged(object sender, FocusedRowHandleChangedEventArgs e)
	{
		int focusedRowHandle = _mtMoveTableView.FocusedRowHandle;
		MTMoveSpecificationRow mTMoveSpecificationRow = (MTMoveSpecificationRow)_mtMoveTableView.Grid.GetRow(focusedRowHandle);
		if (mTMoveSpecificationRow != null)
		{
			ChangeColumnAdditonalView(mTMoveSpecificationRow.MTCode);
			RowInfoChange?.Invoke(mTMoveSpecificationRow);
		}
	}

	private void ChangeColumnAdditonalView(string ItemCode)
	{
		_unitCodeCol.DataView = new MTQntUnits(string.IsNullOrWhiteSpace(ItemCode) ? "" : ItemCode);
	}

	protected override void StoreSettings(RegistryKey rk)
	{
		base.StoreSettings(rk);
		rk.SetValue("DistributByFEFO", _distributeByFEFO ? "1" : "0");
		_mtMoveGrid.StoreSettings(rk);
	}

	protected override void RestoreSettings(double DefaultWidth, double DefaultHeight, RegistryKey rk)
	{
		base.RestoreSettings(DefaultWidth, DefaultHeight, rk);
		if (rk != null)
		{
			_distributeByFEFO = rk.GetValue("DistributByFEFO", false).ToString() == "1";
			_mtMoveGrid.RestoreSettings(rk);
		}
	}

	private void MotorVehicle_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		SetMotorVehicleReadOnlyProperty();
	}

	private void SetMotorVehicleReadOnlyProperty()
	{
		bool value = _motorVehicle.IsChecked.Value;
		_brand.IsRequired = value;
		_brand.IsReadOnly = !value;
		_licensePlate.IsRequired = value;
		_licensePlate.IsReadOnly = !value;
	}
}
