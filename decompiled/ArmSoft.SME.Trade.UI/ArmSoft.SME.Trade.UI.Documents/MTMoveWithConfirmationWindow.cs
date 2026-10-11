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
using ArmSoft.SME.Core.UI.HelpClasses;
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

[DocumentWindow(39)]
public class MTMoveWithConfirmationWindow : TradeDocumentWindow<MTMoveWithConfirmation>
{
	public class MTMoveSpecificationRow : DocumentGridRow
	{
		public bool UpdateRemainsOnInfoPanel = true;

		[ObservableProperty]
		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public int MTID
		{
			get
			{
				return field;
			}
			set
			{
				if (!EqualityComparer<int>.Default.Equals(field, value))
				{
					OnPropertyChanging(__KnownINotifyPropertyChangingArgs.MTID);
					field = value;
					OnPropertyChanged(__KnownINotifyPropertyChangedArgs.MTID);
				}
			}
		} = -1;

		[ObservableProperty]
		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public string MTCode
		{
			get
			{
				return field;
			}
			set
			{
				if (!EqualityComparer<string>.Default.Equals(field, value))
				{
					OnPropertyChanging(__KnownINotifyPropertyChangingArgs.MTCode);
					field = value;
					OnMTCodeChanged(value);
					OnPropertyChanged(__KnownINotifyPropertyChangedArgs.MTCode);
				}
			}
		} = "";

		[ObservableProperty]
		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public string MTName
		{
			get
			{
				return field;
			}
			set
			{
				if (!EqualityComparer<string>.Default.Equals(field, value))
				{
					OnPropertyChanging(__KnownINotifyPropertyChangingArgs.MTName);
					field = value;
					OnPropertyChanged(__KnownINotifyPropertyChangedArgs.MTName);
				}
			}
		} = "";

		[ObservableProperty]
		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public string UnitCode
		{
			get
			{
				return field;
			}
			set
			{
				if (!EqualityComparer<string>.Default.Equals(field, value))
				{
					OnPropertyChanging(__KnownINotifyPropertyChangingArgs.UnitCode);
					field = value;
					OnUnitCodeChanged(value);
					OnPropertyChanged(__KnownINotifyPropertyChangedArgs.UnitCode);
				}
			}
		} = "";

		[ObservableProperty]
		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public string UnitBrief
		{
			get
			{
				return field;
			}
			set
			{
				if (!EqualityComparer<string>.Default.Equals(field, value))
				{
					OnPropertyChanging(__KnownINotifyPropertyChangingArgs.UnitBrief);
					field = value;
					OnPropertyChanged(__KnownINotifyPropertyChangedArgs.UnitBrief);
				}
			}
		} = "";

		[ObservableProperty]
		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public decimal Quantity
		{
			get
			{
				return field;
			}
			set
			{
				if (!EqualityComparer<decimal>.Default.Equals(field, value))
				{
					OnPropertyChanging(__KnownINotifyPropertyChangingArgs.Quantity);
					field = value;
					OnQuantityChanged(value);
					OnPropertyChanged(__KnownINotifyPropertyChangedArgs.Quantity);
				}
			}
		}

		[ObservableProperty]
		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public decimal ActualQuantity
		{
			get
			{
				return field;
			}
			set
			{
				if (!EqualityComparer<decimal>.Default.Equals(field, value))
				{
					OnPropertyChanging(__KnownINotifyPropertyChangingArgs.ActualQuantity);
					field = value;
					OnActualQuantityChanged(value);
					OnPropertyChanged(__KnownINotifyPropertyChangedArgs.ActualQuantity);
				}
			}
		}

		[ObservableProperty]
		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public decimal VarianceQuantity
		{
			get
			{
				return field;
			}
			set
			{
				if (!EqualityComparer<decimal>.Default.Equals(field, value))
				{
					OnPropertyChanging(__KnownINotifyPropertyChangingArgs.VarianceQuantity);
					field = value;
					OnPropertyChanged(__KnownINotifyPropertyChangedArgs.VarianceQuantity);
				}
			}
		}

		[ObservableProperty]
		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public short RowNum
		{
			get
			{
				return field;
			}
			set
			{
				if (!EqualityComparer<short>.Default.Equals(field, value))
				{
					OnPropertyChanging(__KnownINotifyPropertyChangingArgs.RowNum);
					field = value;
					OnPropertyChanged(__KnownINotifyPropertyChangedArgs.RowNum);
				}
			}
		}

		[ObservableProperty]
		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public decimal Price
		{
			get
			{
				return field;
			}
			set
			{
				if (!EqualityComparer<decimal>.Default.Equals(field, value))
				{
					OnPropertyChanging(__KnownINotifyPropertyChangingArgs.Price);
					field = value;
					OnPriceChanged(value);
					OnPropertyChanged(__KnownINotifyPropertyChangedArgs.Price);
				}
			}
		}

		[ObservableProperty]
		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public decimal Summa
		{
			get
			{
				return field;
			}
			set
			{
				if (!EqualityComparer<decimal>.Default.Equals(field, value))
				{
					OnPropertyChanging(__KnownINotifyPropertyChangingArgs.Summa);
					field = value;
					OnSummaChanged(value);
					OnPropertyChanged(__KnownINotifyPropertyChangedArgs.Summa);
				}
			}
		}

		[ObservableProperty]
		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public bool IsPartySelected
		{
			get
			{
				return field;
			}
			set
			{
				if (!EqualityComparer<bool>.Default.Equals(field, value))
				{
					OnPropertyChanging(__KnownINotifyPropertyChangingArgs.IsPartySelected);
					field = value;
					OnPropertyChanged(__KnownINotifyPropertyChangedArgs.IsPartySelected);
				}
			}
		}

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

		[ObservableProperty]
		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public int RowID
		{
			get
			{
				return field;
			}
			set
			{
				if (!EqualityComparer<int>.Default.Equals(field, value))
				{
					OnPropertyChanging(__KnownINotifyPropertyChangingArgs.RowID);
					field = value;
					OnPropertyChanged(__KnownINotifyPropertyChangedArgs.RowID);
				}
			}
		}

		[ObservableProperty]
		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public string RowDescription
		{
			get
			{
				return field;
			}
			set
			{
				if (!EqualityComparer<string>.Default.Equals(field, value))
				{
					OnPropertyChanging(__KnownINotifyPropertyChangingArgs.RowDescription);
					field = value;
					OnPropertyChanged(__KnownINotifyPropertyChangedArgs.RowDescription);
				}
			}
		} = "";

		[ObservableProperty]
		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public string FullName
		{
			get
			{
				return field;
			}
			set
			{
				if (!EqualityComparer<string>.Default.Equals(field, value))
				{
					OnPropertyChanging(__KnownINotifyPropertyChangingArgs.FullName);
					field = value;
					OnPropertyChanged(__KnownINotifyPropertyChangedArgs.FullName);
				}
			}
		} = "";

		public MTMoveSpecificationRow()
		{
		}

		public MTMoveSpecificationRow(MTMoveWithConfirmationWindow docWindow)
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
				},
				{
					"ActualQuantity",
					(DocumentGridRow row, DevExpress.XtraEditors.DXErrorProvider.ErrorInfo info) =>
					{
						ValidateActualQuantity((MTMoveSpecificationRow)row, info);
					}
				}
			});
		}

		private static void ValidateMTCode(MTMoveSpecificationRow row, DevExpress.XtraEditors.DXErrorProvider.ErrorInfo info)
		{
			TradePropertyValidator.ValidateMaterialCode(row.MTCode, info, TradeAllDocDefinitions.MTMoveWithConfirmation.Grids["MTList"].Columns["MTCode"]);
		}

		private static void ValidateUnitCode(MTMoveSpecificationRow row, DevExpress.XtraEditors.DXErrorProvider.ErrorInfo info)
		{
			TradePropertyValidator.ValidateGoodUnitCode(row.UnitCode, row.MTCode, info, TradeAllDocDefinitions.MTMoveWithConfirmation.Grids["MTList"].Columns["UnitCode"]);
		}

		private static void ValidateQuantity(MTMoveSpecificationRow row, DevExpress.XtraEditors.DXErrorProvider.ErrorInfo info)
		{
			PropertyValidator.PositiveValidate(row.Quantity, info, ArmSoft.SME.Common.Resources.Quantity);
		}

		private static void ValidateActualQuantity(MTMoveSpecificationRow row, DevExpress.XtraEditors.DXErrorProvider.ErrorInfo info)
		{
			PropertyValidator.NonNegativeValidate(row.ActualQuantity, info, ArmSoft.SME.Common.Resources.ActualQTY);
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
			VarianceQuantity = ActualQuantity - Quantity;
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		private void OnActualQuantityChanged(decimal value)
		{
			ResetForceValidationBit("ActualQuantity");
			VarianceQuantity = ActualQuantity - Quantity;
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

	public class MTMoveWithConfirmationFilteredSearchList : IResultList
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

	protected delegate void RowInfoChangeEventHandler(MTMoveSpecificationRow row);

	private delegate void DelegateSetRowInfoToInfoPanel(MTMoveSpecificationRow row);

	private delegate void DelegateClearInfoPanel();

	private DropDownCommentControl _baseDocumentControl;

	public DropDownControl _storageExpense;

	public TextEditExt _status;

	private DropDownControl _storageIncome;

	private CheckEditExt _confirmed;

	public DropDownControl _pricesTypes;

	private ComboBoxEditExt _comment;

	private DropDownControl _owner;

	private TextEditExt _barCode;

	private Button _codeEnter;

	public DocumentGridControl _mtMoveWithConfirmationGrid;

	private TableView _mtMoveWithConfirmationTableView;

	private BarEditItemExt _filteredSearch;

	private BarEditItemExt _actualQuantitiesCheck;

	public ObservableCollection<MTMoveSpecificationRow> _mtMoveWithConfirmationList;

	private DocumentGridColumn _mtCodeCol;

	private DocumentGridColumn _quantityCol;

	private DocumentGridColumn _actualQuantityCol;

	private DocumentGridColumn _varianceQuantityCol;

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

	private bool _storageExpenseAvailable;

	public bool _storageIncomeIsRetail;

	private bool _storageIncomeAvailable;

	private readonly bool _isAskQuestionConfirmSummChange = true;

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

	private bool _showStocktakingColumns;

	public int _maxRowID;

	protected override double WidthValue => 890.0;

	protected override double HeightValue => 480.0;

	protected override double MinWidthValue => 890.0;

	protected override double MinHeightValue => 250.0;

	public TradeDocument BaseDocument
	{
		get;
		set
		{
			field = value;
			if (field == null)
			{
				_baseDocumentControl.Text = "";
			}
			else
			{
				_baseDocumentControl.Text = DocumentHelpers.GetDocumentDescription(field, ArmSoft.SME.Common.Constants.Common.DocumentDescriptionMode.TypeDateNum);
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
			if (value.CompareTo(_baseISN) == 0)
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

	public MTMoveWithConfirmationWindow()
	{
	}

	public MTMoveWithConfirmationWindow(MTMoveWithConfirmation doc)
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
		_baseDocumentControl = AddDropDownCommentControl(null, ArmSoft.SME.Common.Resources.BaseDoc, isRequired: false, idv, 50, HorizontalAlignment.Stretch);
		_baseDocumentControl.ShowClearButton = true;
		_baseDocumentControl.ShowSearchButton = true;
		_baseDocumentControl.IsTextEditable = false;
		_baseDocumentControl.ClickDropDown += new SafeDocumentEventHandler<DropDownControlEventArgs>(BaseDocument_ClickDropDown, this).Handler;
		_baseDocumentControl.EditKeyValueChanged += new SafeDocumentEventHandler<EditKeyValueChangedEventArgs>(BaseDocument_EditKeyValueChanged, this).Handler;
		_baseDocumentControl.BtnClearClicked += new SafeDocumentEventHandler<EditKeyValueChangedEventArgs>(BaseDocumentDelete_Click, this).Handler;
		_baseDocumentControl.BtnSearchClicked += new SafeDocumentEventHandler<EditKeyValueChangedEventArgs>(BaseDocumentSearch_Click, this).Handler;
		LayoutGroupExt container2 = AddLayoutGroup(null, LayoutGroupView.Group, Orientation.Horizontal);
		LayoutGroupExt container3 = AddLayoutGroup(container2, LayoutGroupView.Group, Orientation.Vertical);
		_storageExpense = AddDropDownControl(container3, Definition.Fields["StorageExpense"], new Storages(), allowAdditionButtons: false, fieldWithCaption: true, HorizontalAlignment.Stretch);
		_storageExpense.EditValueChanged += new SafeDocumentEventHandler<EditValueChangedEventArgs>(StorageExpense_EditValueChanged, this).Handler;
		_storageIncome = AddDropDownControl(container3, Definition.Fields["StorageIncome"], new Storages(), allowAdditionButtons: false, fieldWithCaption: true, HorizontalAlignment.Stretch);
		_storageIncome.EditValueChanged += new SafeDocumentEventHandler<EditValueChangedEventArgs>(StorageIncome_EditValueChanged, this).Handler;
		container3 = AddLayoutGroup(container2, LayoutGroupView.Group, Orientation.Vertical);
		_status = AddStringControl(container3, ArmSoft.SME.Trade.Common.Resources.Status, isRequired: false, 100, HorizontalAlignment.Stretch, "", "Status");
		_status.IsEnabled = false;
		_confirmed = AddBooleanControl(container3, Definition.Fields["Confirmed"]);
		_confirmed.EditValueChanged += new SafeDocumentEventHandler<EditValueChangedEventArgs>(Confirmed_EditValueChanged, this).Handler;
		LayoutGroup container4 = AddLayoutGroup(null, LayoutGroupView.Group, Orientation.Horizontal);
		_barCode = AddStringControl(container4, ArmSoft.SME.Common.Resources.BarCode + " (F2)", isRequired: false, 20, HorizontalAlignment.Stretch);
		_barCode.KeyDown += new SafeDocumentEventHandler<KeyEventArgs>(BarCode_KeyDown, this).Handler;
		_barCode.NullText = ArmSoft.SME.Trade.Common.Resources.BarCodeAndCodeInputNullText;
		string text = ArmSoft.SME.Trade.Common.Resources.cmdCodeEnter + " (F8)";
		_codeEnter = AddButton(container4, "", text, "", text, isTabStop: true);
		_codeEnter.Margin = new Thickness(0.0, 0.0, 18.0, 0.0);
		_codeEnter.Click += new SafeDocumentEventHandler<RoutedEventArgs>(CodeEnter_Click, this).Handler;
		DXTabControl tc = AddTabControl(null);
		LayoutGroup container5 = AddTabbedGroup(tc, ArmSoft.SME.Common.Resources.General, "General");
		_pricesTypes = AddDropDownControl(container5, Definition.Fields["PriceType"], new PriceTypes(), allowAdditionButtons: false, fieldWithCaption: true, HorizontalAlignment.Stretch);
		_pricesTypes.EditValueChanged += new SafeDocumentEventHandler<EditValueChangedEventArgs>(PricesTypes_EditValueChanged, this).Handler;
		List<string> captionsList = TradeSettings.TradeCacheService.StandardComments.GetCaptionsList();
		_comment = AddComboBoxControl(container5, Definition.Fields["Comment"], captionsList, HorizontalAlignment.Stretch);
		_owner = AddDropDownControl(container5, Definition.Fields["Owner"], new UsersFilter());
		_owner.CodeWidth = 12;
		_owner.ShowComment = false;
		_owner.InnerCodeEdit.Width = _owner.InnerCodeEdit.MinWidth;
		if (!Settings.LoginService.IsAdmin && Settings.ParametersService.DenyAccessToOtherOwnerDocs())
		{
			((LayoutItemWithRequired)(object)_owner.Parent).Visibility = Visibility.Collapsed;
		}
		_mtMoveWithConfirmationGrid = AddGrid(container5, Definition.Grids["MTList"], HorizontalAlignment.Stretch, VerticalAlignment.Stretch, isSupportDialogMode: true);
		_mtMoveWithConfirmationGrid.AllowMoveUp = true;
		_mtMoveWithConfirmationGrid.AllowMoveDown = true;
		_mtMoveWithConfirmationGrid.IsRequired = true;
		_mtMoveWithConfirmationTableView = _mtMoveWithConfirmationGrid.View;
		FormatCondition item = new FormatCondition
		{
			Expression = "[VarianceQuantity] = 0.0m",
			FieldName = "VarianceQuantity",
			ApplyToRow = false,
			Format = ConditionalFormattingHelper.CreateDevExFormat(new UserDefinedFormat
			{
				IconUri = "accept_16.svg"
			})
		};
		FormatCondition item2 = new FormatCondition
		{
			Expression = "[VarianceQuantity] <> 0.0m",
			FieldName = "VarianceQuantity",
			ApplyToRow = false,
			Format = ConditionalFormattingHelper.CreateDevExFormat(new UserDefinedFormat
			{
				IconUri = "warning_circle_16.svg"
			})
		};
		_mtMoveWithConfirmationTableView.FormatConditions.Add(item);
		_mtMoveWithConfirmationTableView.FormatConditions.Add(item2);
		_focusedRowHandleChangedEvent = new SafeDocumentEventHandler<FocusedRowHandleChangedEventArgs>(MTMoveWithConfirmationTableView_FocusedRowHandleChanged, this);
		_mtMoveWithConfirmationTableView.FocusedRowHandleChanged += _focusedRowHandleChangedEvent.Handler;
		_mtMoveWithConfirmationGrid.AddRowIndicator(35);
		_filteredSearch = _mtMoveWithConfirmationGrid.AddButton(null, ArmSoft.SME.Trade.Common.Resources.GroupAddition, ArmSoft.SME.Trade.Common.Resources.GroupAddition, 130.0);
		_filteredSearch.Click += new SafeDocumentEventHandler<RoutedEventArgs>(FilteredSearch_Click, this).Handler;
		_actualQuantitiesCheck = _mtMoveWithConfirmationGrid.AddButton("cheked_all_16.svg", ArmSoft.SME.Trade.Common.Resources.ActualQuantitiesCheck, ArmSoft.SME.Trade.Common.Resources.ActualQuantitiesCheck, 255.0);
		_actualQuantitiesCheck.Click += new SafeDocumentEventHandler<RoutedEventArgs>(ActualQuantitiesCheck_Click, this).Handler;
		_mtCodeCol = _mtMoveWithConfirmationGrid.AddDropDownColumn(Definition.Grids["MTList"].Columns["MTCode"], new ArmSoft.SME.Trade.UI.DropDown.GoodsFilter(shortMode: true, GoodsTypesSE.Good, "", isShowOnlyInPriceList: true), allowAdditionButtons: true, isOwnProcessingClickDropDown: true);
		DocumentGridColumn documentGridColumn = _mtMoveWithConfirmationGrid.AddStringColumn(Definition.Grids["MTList"].Columns["MTName"]);
		documentGridColumn.AllowEditing = DefaultBoolean.False;
		documentGridColumn.Width = 225.0;
		_unitCodeCol = _mtMoveWithConfirmationGrid.AddDropDownColumn(Definition.Grids["MTList"].Columns["UnitCode"], new MTQntUnits());
		_mtMoveWithConfirmationGrid.AddStringColumn(Definition.Grids["MTList"].Columns["UnitBrief"]).AllowEditing = DefaultBoolean.False;
		_quantityCol = _mtMoveWithConfirmationGrid.AddNumberColumn(Definition.Grids["MTList"].Columns["Quantity"], showTotal: true);
		_actualQuantityCol = _mtMoveWithConfirmationGrid.AddNumberColumn(Definition.Grids["MTList"].Columns["ActualQuantity"], showTotal: true);
		_varianceQuantityCol = _mtMoveWithConfirmationGrid.AddNumberColumn("VarianceQuantity", ArmSoft.SME.Common.Resources.VarianceQty, 13, 3, allowNegativeNumbers: true, showTotal: true);
		if (string.IsNullOrWhiteSpace(PO.StorageExpense))
		{
			if (!string.IsNullOrWhiteSpace(Settings.ParametersService.DefaultStorage()))
			{
				_storageExpenseIsRetail = TradeSettings.TradeCacheService.Storage.GetItem(Settings.ParametersService.DefaultStorage())?.Retail ?? false;
				_storageExpenseAvailable = TradeSettings.CreateTradeClient().CommonServices.IsStorageAvailable(Settings.ParametersService.DefaultStorage());
			}
			else
			{
				_storageExpenseAvailable = true;
			}
		}
		else
		{
			if (!string.IsNullOrWhiteSpace(PO.StorageExpense))
			{
				_storageExpenseIsRetail = TradeSettings.TradeCacheService.Storage.GetItem(PO.StorageExpense)?.Retail ?? false;
				_storageExpenseAvailable = TradeSettings.CreateTradeClient().CommonServices.IsStorageAvailable(PO.StorageExpense);
			}
			if (!string.IsNullOrWhiteSpace(PO.StorageIncome))
			{
				_storageIncomeIsRetail = TradeSettings.TradeCacheService.Storage.GetItem(PO.StorageIncome)?.Retail ?? false;
				_storageIncomeAvailable = TradeSettings.CreateTradeClient().CommonServices.IsStorageAvailable(PO.StorageIncome);
			}
		}
		_priceCol = _mtMoveWithConfirmationGrid.AddNumberColumn(Definition.Grids["MTList"].Columns["Price"], showTotal: false, useCurrencyFormat: true, isOwnProcessingClickDropDown: true);
		_summaCol = _mtMoveWithConfirmationGrid.AddNumberColumn(Definition.Grids["MTList"].Columns["Summa"], showTotal: true);
		_summaCol.AllowEditing = DefaultBoolean.False;
		if (_storageExpenseIsRetail)
		{
			_pricesTypes.Code = "";
		}
		SetPriceSumColumnsReadOnlyProperty();
		_partyCol = _mtMoveWithConfirmationGrid.AddDropDownColumn("PartyMethodName", ArmSoft.SME.Common.Resources.Party, 19, trimValue: false, GlyphKind.Regular);
		((ButtonEditSettings)_partyCol.EditSettings).IsTextEditable = false;
		_mtMoveWithConfirmationList = new ObservableCollection<MTMoveSpecificationRow>();
		_mtMoveWithConfirmationGrid.AddStringColumn(Definition.Grids["MTList"].Columns["RowDescription"]);
		_mtMoveWithConfirmationGrid.DeletingRow += new SafeDocumentEventHandler<DocumentGridDeletingRowEventArgs>(MTMoveWithConfirmationGrid_DeletingRow, this).Handler;
		_mtMoveWithConfirmationGrid.AddingRow += new SafeDocumentEventHandler<DocumentGridAddingRowEventArgs>(MTMoveWithConfirmationGrid_AddingRow, this).Handler;
		_mtMoveWithConfirmationGrid.DuplicatingRow += new SafeDocumentEventHandler<DocumentGridDuplicatingRowEventArgs>(MTMoveWithConfirmationGrid_DuplicatingRow, this).Handler;
		_mtMoveWithConfirmationGrid.AddingRowWithDialog += new SafeDocumentEventHandler<DocumentGridAddingRowEventArgs>(MTMoveWithConfirmationGrid_AddingRowWithDialog, this).Handler;
		_mtMoveWithConfirmationGrid.DuplicatingRowWithDialog += new SafeDocumentEventHandler<DocumentGridDuplicatingRowEventArgs>(MTMoveWithConfirmationGrid_DuplicatingRowWithDialog, this).Handler;
		_mtMoveWithConfirmationGrid.EditingRowWithDialog += new SafeDocumentEventHandler<DocumentGridEditingRowEventArgs>(MTMoveWithConfirmationGrid_EditingRowWithDialog, this).Handler;
		_mtMoveWithConfirmationGrid.MovingRowUp += new SafeDocumentEventHandler<DocumentGridMovingRowUpOrDownEventArgs>(MTMoveWithConfirmationGrid_MovingRowUp, this).Handler;
		_mtMoveWithConfirmationGrid.MovingRowDown += new SafeDocumentEventHandler<DocumentGridMovingRowUpOrDownEventArgs>(MTMoveWithConfirmationGrid_MovingRowDown, this).Handler;
		_mtMoveWithConfirmationGrid.RefreshButtons += new SafeDocumentEventHandler<EventArgs>(MTMoveWithConfirmationGrid_RefreshButtons, this).Handler;
		_mtMoveWithConfirmationGrid.ClickDropDown += new SafeDocumentEventHandler<DocumentGridClickDropDownEventArgs>(MTMoveWithConfirmationGrid_ClickDropDown, this).Handler;
		foreach (GridColumn column in _mtMoveWithConfirmationGrid.Grid.Columns)
		{
			column.AllowSorting = DefaultBoolean.True;
			column.AllowColumnFiltering = DefaultBoolean.True;
			column.FilterPopupMode = FilterPopupMode.CheckedList;
		}
		_infoDisplayControls = AddInfoDisplayControls(DocumentTypeSE.MTMoveWithConfirmation, isShowItemPictureOnInfoPanel: false, isShowStorageAvailabilityOnInfoPanel: true, isShowWholeAvailabilityOnInfoPanel: true, isShowPriceAvailabilityOnInfoPanel: false, showDescriptionOnInfoPanel: false, enableSubstituteItems: false, 22.0, container5);
		_infoDisplay = _infoDisplayControls.InfoDisplay;
		_infoDisplay.Collapsed += new SafeDocumentEventHandler<EventArgs>(InfoDisplay_Collapsed, this).Handler;
		_infoItemName = _infoDisplayControls.InfoItemName;
		LayoutGroup container6 = AddTabbedGroup(tc, ArmSoft.SME.Common.Resources.Additional, "Additional");
		_mediator = AddDropDownCommentControl(container6, Definition.Fields["Mediator"], new Employees(), HorizontalAlignment.Stretch);
		_chiefAcc = AddStringControl(container6, Definition.Fields["Chief"], HorizontalAlignment.Stretch);
		_allower = AddDropDownCommentControl(container6, Definition.Fields["Allower"], new Employees(), HorizontalAlignment.Stretch);
		LayoutGroup container7 = AddLayoutGroup(container6, LayoutGroupView.Group, Orientation.Horizontal);
		_supBookN = AddStringControl(container7, Definition.Fields["BookNum"]);
		_bookPage = AddNumericControl(container7, Definition.Fields["BookPage"], useCurrencyFormat: false);
		_bookRow = AddNumericControl(container7, Definition.Fields["BookLine"], useCurrencyFormat: false);
		_exportedType = AddSmartEnumDropDownControl(container6, Definition.Fields["ExportedType"], allowAdditionButtons: false, fieldWithCaption: true, HorizontalAlignment.Stretch);
		LayoutGroup container8 = AddLayoutGroup(container6, LayoutGroupView.Group, Orientation.Horizontal);
		_taxInvoiceSeries = AddStringControl(container8, Definition.Fields["TaxInvoiceSeries"]);
		_taxInvoiceNumber = AddStringControl(container8, Definition.Fields["TaxInvoiceNumber"]);
		_taxInvoiceDate = AddDateControl(container6, Definition.Fields["InvoiceDate"]);
		_transportationMethod = AddComboBoxControl(container6, Definition.Fields["TransportationMethod"], captionsList, HorizontalAlignment.Stretch);
		_motorVehicle = AddBooleanControl(container6, Definition.Fields["MotorVehicle"]);
		_motorVehicle.EditValueChanged += new SafeDocumentEventHandler<EditValueChangedEventArgs>(MotorVehicle_EditValueChanged, this).Handler;
		_brand = AddStringControl(container6, Definition.Fields["Brand"], HorizontalAlignment.Stretch);
		_licensePlate = AddStringControl(container6, Definition.Fields["LicensePlate"], HorizontalAlignment.Stretch);
		_infoDisplay.IsCollapsed = _infoDisplayControls.InfoPanelCollapsed;
		_mtMoveWithConfirmationTableView.CellValueChanged += new SafeDocumentEventHandler<CellValueChangedEventArgs>(MTMoveWithConfirmationTableView_CellValueChanged, this).Handler;
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
		if (string.IsNullOrWhiteSpace(_storageExpense.Code))
		{
			_storageExpenseAvailable = true;
		}
		if (_viewMode == WindowViewMode.Copy && _distributeByFEFO)
		{
			PO.MTExpDocSelectedPartiesList.Clear();
			decimal expSumm = 0m;
			for (int i = 0; i <= _mtMoveWithConfirmationList.Count - 1; i++)
			{
				DistributeQtyByExpiredDate(_mtMoveWithConfirmationList[i].RowID, _storageExpense.Code, _mtMoveWithConfirmationList[i].MTCode, _mtMoveWithConfirmationList[i].UnitCode, _mtMoveWithConfirmationList[i].Quantity, ref expSumm);
				_mtMoveWithConfirmationList[i].Summa = expSumm;
				_mtMoveWithConfirmationList[i].IsPartySelected = true;
			}
		}
		Confirmed_EditValueChanged();
	}

	protected override void OnNewAfterSave()
	{
		base.OnNewAfterSave();
		_mtMoveWithConfirmationList.Clear();
		_maxRowID = 0;
		_owner.Code = Settings.LoginService.UserName;
		if (_storageExpenseIsRetail)
		{
			_pricesTypes.Code = "";
		}
		_confirmed.IsChecked = false;
		Confirmed_EditValueChanged();
	}

	protected override void OnEdit()
	{
		base.OnEdit();
		_setFocusOnActivation = _docNum;
		if (!string.IsNullOrWhiteSpace(_storageExpense.Code))
		{
			_storageExpenseIsRetail = TradeSettings.TradeCacheService.Storage.GetItem(_storageExpense.Code)?.Retail ?? false;
			_storageExpenseAvailable = TradeSettings.CreateTradeClient().CommonServices.IsStorageAvailable(_storageExpense.Code);
		}
		if (!string.IsNullOrWhiteSpace(_storageIncome.Code))
		{
			_storageIncomeIsRetail = TradeSettings.TradeCacheService.Storage.GetItem(_storageIncome.Code)?.Retail ?? false;
			_storageIncomeAvailable = TradeSettings.CreateTradeClient().CommonServices.IsStorageAvailable(_storageIncome.Code);
		}
		Confirmed_EditValueChanged();
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
		_mtMoveWithConfirmationList.Clear();
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
				ActualQuantity = mT.ActualQuantity,
				RowNum = mT.RowNum,
				Price = mT.Price.GetValueOrDefault(),
				Summa = mT.Summa.GetValueOrDefault(),
				IsPartySelected = mT.IsPartySelected,
				RowID = mT.RowID,
				RowDescription = mT.RowDescription
			};
			_mtMoveWithConfirmationList.Add(item);
		}
		_maxRowID = PO.MaxRowID;
		_mtMoveWithConfirmationGrid.ItemsSource = _mtMoveWithConfirmationList;
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
		SetStocktakingColumnsVisibility();
	}

	protected override void FillDocument(StoreRequest sr)
	{
		base.FillDocument(sr);
		PO.BaseISN = _baseISN;
		PO.MTList.Clear();
		foreach (MTMoveSpecificationRow mtMoveWithConfirmation in _mtMoveWithConfirmationList)
		{
			MTMoveBase.MTMoveSpecificationRow item = new MTMoveBase.MTMoveSpecificationRow
			{
				MTID = mtMoveWithConfirmation.MTID,
				MTCode = mtMoveWithConfirmation.MTCode,
				MTName = mtMoveWithConfirmation.MTName,
				FullName = mtMoveWithConfirmation.FullName,
				UnitCode = mtMoveWithConfirmation.UnitCode,
				UnitBrief = mtMoveWithConfirmation.UnitBrief,
				Quantity = mtMoveWithConfirmation.Quantity,
				ActualQuantity = mtMoveWithConfirmation.ActualQuantity,
				RowNum = mtMoveWithConfirmation.RowNum,
				Price = mtMoveWithConfirmation.Price,
				Summa = mtMoveWithConfirmation.Summa,
				IsPartySelected = mtMoveWithConfirmation.IsPartySelected,
				RowID = mtMoveWithConfirmation.RowID,
				RowDescription = mtMoveWithConfirmation.RowDescription
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
		if (!_motorVehicle.IsChecked.Value)
		{
			_brand.IsReadOnly = true;
			_licensePlate.IsReadOnly = true;
		}
		bool flag = State == DocumentStateSE.Draft.Value || State == DocumentStateSE.Deleted.Value;
		bool flag2 = (_viewMode == WindowViewMode.Edit && !flag) || !_storageExpenseAvailable;
		_date.IsReadOnly = flag2;
		_storageIncome.IsReadOnly = flag2;
		_storageExpense.IsReadOnly = flag2;
		_confirmed.IsEnabled = _storageIncomeAvailable;
		_baseDocumentControl.IsReadOnly = flag2;
		_barCode.IsReadOnly = flag2;
		_codeEnter.IsEnabled = !flag2;
		foreach (GridColumn column in _mtMoveWithConfirmationGrid.Grid.Columns)
		{
			column.ReadOnly = flag2;
		}
		_varianceQuantityCol.ReadOnly = true;
		_mtMoveWithConfirmationGrid.AllowAdd = !flag2;
		_mtMoveWithConfirmationGrid.AllowEdit = !flag2;
		_mtMoveWithConfirmationGrid.AllowDelete = !flag2;
		_mtMoveWithConfirmationGrid.AllowDuplicate = !flag2;
		_mtMoveWithConfirmationGrid.AllowMoveUp = !flag2;
		_mtMoveWithConfirmationGrid.AllowMoveDown = !flag2;
		SetPriceSumColumnsReadOnlyProperty();
		_partyCol.ReadOnly = false;
		((ButtonEditSettings)_partyCol.EditSettings).IsTextEditable = false;
		_actualQuantityCol.ReadOnly = !_storageIncomeAvailable;
		_actualQuantitiesCheck.IsEnabled = _storageIncomeAvailable && _showStocktakingColumns;
		if (!_storageExpenseAvailable)
		{
			_docNum.IsReadOnly = true;
			_pricesTypes.IsReadOnly = true;
			_comment.IsReadOnly = true;
			_owner.IsReadOnly = true;
			_chiefAcc.IsReadOnly = true;
			_mediator.IsReadOnly = true;
			_allower.IsReadOnly = true;
			_supBookN.IsReadOnly = true;
			_exportedType.IsReadOnly = true;
			_bookPage.IsReadOnly = true;
			_bookRow.IsReadOnly = true;
			_taxInvoiceSeries.IsReadOnly = true;
			_taxInvoiceNumber.IsReadOnly = true;
			_taxInvoiceDate.IsReadOnly = true;
			_transportationMethod.IsReadOnly = true;
			_motorVehicle.IsReadOnly = true;
			_brand.IsReadOnly = true;
			_licensePlate.IsReadOnly = true;
		}
	}

	private void DistributeQtyByExpiredDate(int rowID, string storage, string goodCode, string unitCode, decimal quantity, ref decimal expSumm, string baseUnit = "")
	{
		decimal num = quantity;
		List<PartyRow> list = new List<PartyRow>();
		List<PartyRow> list2 = new List<PartyRow>();
		List<PartyRow> list3 = new List<PartyRow>();
		bool flag = TradeSettings.CreateTradeClient().CommonServices.IsNegativExpAllow(storage);
		string text = "";
		GoodsEngineServicesRoutes.PartyRowAndExpMethod parties = TradeSettings.CreateTradeClient().GoodsEngineServices.GetParties(storage, goodCode, _date.EditValue.Value);
		MaterialCacheRow item = TradeSettings.TradeCacheService.Material.GetItem(goodCode);
		baseUnit = item.Unit;
		quantity = (quantity * PriceHelper.GetMTQntUnitCoeff(item.MTCode, unitCode)).RoundAs(3);
		num = quantity;
		unitCode = baseUnit;
		foreach (PartyRow partyRow10 in parties.partyRows)
		{
			partyRow10.ExpQty = 0m;
			partyRow10.ExpSumma = 0m;
			partyRow10.GridRow = rowID;
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
					List<PartyRow> partyRows2 = parties.partyRows;
					if (item3 == partyRows2[partyRows2.Count - 1])
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

	private void MTMoveWithConfirmationGrid_DeletingRow(object sender, DocumentGridDeletingRowEventArgs e)
	{
		MTMoveSpecificationRow mtMoveWithConfirmationGridRow = (MTMoveSpecificationRow)_mtMoveWithConfirmationGrid.Grid.GetRow(e.RowHandle);
		if (mtMoveWithConfirmationGridRow != null)
		{
			PO.MTExpDocSelectedPartiesList.RemoveAll((PartyRow x) => x.GridRow == mtMoveWithConfirmationGridRow.RowID);
			RowInfoChange?.Invoke((MTMoveSpecificationRow)_mtMoveWithConfirmationTableView.Grid.CurrentItem);
		}
	}

	private void MTMoveWithConfirmationGrid_AddingRow(object sender, DocumentGridAddingRowEventArgs e)
	{
		ValidateDocFields();
		if (string.IsNullOrWhiteSpace(_storageExpense.Code))
		{
			ControlsHelper.SetFocus(_storageExpense);
			throw new ASException(ArmSoft.SME.Common.Resources.ValidEmptyExpenseStorage);
		}
		_mtMoveWithConfirmationList.Add(new MTMoveSpecificationRow
		{
			IsPartySelected = false,
			RowID = _maxRowID
		});
		_maxRowID++;
		e.RowNumber = _mtMoveWithConfirmationList.Count - 1;
		RowInfoChangeEventHandler rowInfoChangeEventHandler = RowInfoChange;
		if (rowInfoChangeEventHandler != null)
		{
			ObservableCollection<MTMoveSpecificationRow> mtMoveWithConfirmationList = _mtMoveWithConfirmationList;
			rowInfoChangeEventHandler(mtMoveWithConfirmationList[mtMoveWithConfirmationList.Count - 1]);
		}
	}

	private void MTMoveWithConfirmationGrid_DuplicatingRow(object sender, DocumentGridDuplicatingRowEventArgs e)
	{
		MTMoveSpecificationRow mTMoveSpecificationRow = (MTMoveSpecificationRow)_mtMoveWithConfirmationGrid.Grid.GetRow(e.RowHandle);
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
			_mtMoveWithConfirmationList.Add(mTMoveSpecificationRow2);
			decimal expSumm = 0m;
			if (_distributeByFEFO)
			{
				DistributeQtyByExpiredDate(mTMoveSpecificationRow2.RowID, _storageExpense.Code, mTMoveSpecificationRow2.MTCode, mTMoveSpecificationRow2.UnitCode, mTMoveSpecificationRow2.Quantity, ref expSumm);
				mTMoveSpecificationRow2.Summa = expSumm;
				mTMoveSpecificationRow2.IsPartySelected = true;
			}
			_maxRowID++;
			e.NewRowNumber = _mtMoveWithConfirmationList.Count - 1;
		}
	}

	private void MTMoveWithConfirmationGrid_AddingRowWithDialog(object sender, DocumentGridAddingRowEventArgs e)
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
				ObservableCollection<MTMoveSpecificationRow> mtMoveWithConfirmationList = _mtMoveWithConfirmationList;
				rowInfoChangeEventHandler(mtMoveWithConfirmationList[mtMoveWithConfirmationList.Count - 1]);
			}
		}
	}

	private void MTMoveWithConfirmationGrid_DuplicatingRowWithDialog(object sender, DocumentGridDuplicatingRowEventArgs e)
	{
		MTMoveSpecificationRow mTMoveSpecificationRow = (MTMoveSpecificationRow)_mtMoveWithConfirmationGrid.Grid.GetRow(e.RowHandle);
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

	private void MTMoveWithConfirmationGrid_EditingRowWithDialog(object sender, DocumentGridEditingRowEventArgs e)
	{
		MTMoveSpecificationRow sourceRow = (MTMoveSpecificationRow)_mtMoveWithConfirmationGrid.Grid.GetRow(e.RowHandle);
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

	private void MTMoveWithConfirmationGrid_MovingRowUp(object sender, DocumentGridMovingRowUpOrDownEventArgs e)
	{
		if (_mtMoveWithConfirmationGrid.Grid.SortInfo.Count > 0 || _mtMoveWithConfirmationTableView.Grid.VisibleRowCount != _mtMoveWithConfirmationList.Count)
		{
			throw new ASException(ArmSoft.SME.Common.Resources.NotAllowRowMoveInSortedGrid);
		}
		MTMoveSpecificationRow mTMoveSpecificationRow = (MTMoveSpecificationRow)_mtMoveWithConfirmationGrid.Grid.GetRow(e.RowHandle);
		if (mTMoveSpecificationRow != null)
		{
			_mtMoveWithConfirmationList.RemoveAt(e.RowHandle);
			_mtMoveWithConfirmationList.Insert(e.RowHandle - 1, mTMoveSpecificationRow);
		}
	}

	private void MTMoveWithConfirmationGrid_MovingRowDown(object sender, DocumentGridMovingRowUpOrDownEventArgs e)
	{
		if (_mtMoveWithConfirmationGrid.Grid.SortInfo.Count > 0 || _mtMoveWithConfirmationTableView.Grid.VisibleRowCount != _mtMoveWithConfirmationList.Count)
		{
			throw new ASException(ArmSoft.SME.Common.Resources.NotAllowRowMoveInSortedGrid);
		}
		MTMoveSpecificationRow mTMoveSpecificationRow = (MTMoveSpecificationRow)_mtMoveWithConfirmationGrid.Grid.GetRow(e.RowHandle);
		if (mTMoveSpecificationRow != null)
		{
			_mtMoveWithConfirmationList.RemoveAt(e.RowHandle);
			_mtMoveWithConfirmationList.Insert(e.RowHandle + 1, mTMoveSpecificationRow);
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

	private void MTMoveWithConfirmationGrid_ClickDropDown(object sender, DocumentGridClickDropDownEventArgs e)
	{
		MTMoveSpecificationRow mTMoveSpecificationRow = (MTMoveSpecificationRow)_mtMoveWithConfirmationGrid.Grid.GetRow(e.RowHandle);
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
			MTSelectPartyWindow mTSelectPartyWindow = new MTSelectPartyWindow(_storageExpense.Code, _date.EditValue.Value, mTMoveSpecificationRow.MTCode, item.Unit, quantity, mTMoveSpecificationRow.RowID, (PO.StoredInDb && !IsDraftOrDeleted) || !_storageExpenseAvailable)
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
				_mtMoveWithConfirmationGrid.Grid.RefreshRow(e.RowHandle);
			}
		}
	}

	private void MTMoveWithConfirmationTableView_CellValueChanged(object sender, CellValueChangedEventArgs e)
	{
		MTMoveSpecificationRow mtMoveWithConfirmationGridRow = (MTMoveSpecificationRow)_mtMoveWithConfirmationGrid.Grid.GetRow(e.RowHandle);
		if (mtMoveWithConfirmationGridRow == null)
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
					PO.MTExpDocSelectedPartiesList.RemoveAll((PartyRow x) => x.GridRow == mtMoveWithConfirmationGridRow.RowID);
					mtMoveWithConfirmationGridRow.MTID = -1;
					mtMoveWithConfirmationGridRow.MTCode = "";
					mtMoveWithConfirmationGridRow.MTName = "";
					mtMoveWithConfirmationGridRow.UnitCode = "";
					mtMoveWithConfirmationGridRow.UnitBrief = "";
					mtMoveWithConfirmationGridRow.RowDescription = "";
					mtMoveWithConfirmationGridRow.Price = 0m;
					mtMoveWithConfirmationGridRow.Summa = 0m;
					mtMoveWithConfirmationGridRow.IsPartySelected = false;
				}
				else
				{
					if (e.OldValue != null && !string.IsNullOrWhiteSpace(text) && text != e.Value.ToString())
					{
						PO.MTExpDocSelectedPartiesList.RemoveAll((PartyRow x) => x.GridRow == mtMoveWithConfirmationGridRow.RowID);
						mtMoveWithConfirmationGridRow.Summa = 0m;
						mtMoveWithConfirmationGridRow.IsPartySelected = false;
					}
					string text2 = e.Value.ToString();
					foreach (MTMoveSpecificationRow mtMoveWithConfirmation in _mtMoveWithConfirmationList)
					{
						if (mtMoveWithConfirmationGridRow != mtMoveWithConfirmation && text2 == mtMoveWithConfirmation.MTCode)
						{
							if (ArmSoft.SME.Core.UI.MessageBox.Show(string.Format(ArmSoft.SME.Trade.Common.Resources.MTExistInMTList, text2), Settings.ProgramName, MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.Yes) != MessageBoxResult.No)
							{
								break;
							}
							mtMoveWithConfirmationGridRow.MTCode = "";
							return;
						}
					}
					if (string.IsNullOrWhiteSpace(text2))
					{
						throw new Exception();
					}
					MaterialCacheRow item = TradeSettings.TradeCacheService.Material.GetItem(text2, throwExceptionNotExits: true);
					mtMoveWithConfirmationGridRow.MTID = item.Id;
					mtMoveWithConfirmationGridRow.MTName = item.Caption;
					_unitCodeCol.DataView = new MTQntUnits(item.MTCode);
					mtMoveWithConfirmationGridRow.IsPartySelected = false;
					mtMoveWithConfirmationGridRow.UnitCode = item.Unit;
					mtMoveWithConfirmationGridRow.UnitBrief = item.UnitRow.Brief;
					_unitCodeCol.DataView = new MTQntUnits(item.MTCode);
					if (!_storageExpenseIsRetail)
					{
						if (_storageIncomeIsRetail)
						{
							mtMoveWithConfirmationGridRow.Price = PriceHelper.GetPriceForGivenPriceListAndVATType(_pricesTypes.Code, "2", mtMoveWithConfirmationGridRow.MTID, mtMoveWithConfirmationGridRow.UnitCode, _date.EditValue.Value, "AMD");
						}
					}
					else
					{
						MaterialCacheRow.QntUnitRow qntUnit = TradeSettings.TradeCacheService.Material.GetQntUnit(mtMoveWithConfirmationGridRow.MTID, mtMoveWithConfirmationGridRow.UnitCode);
						if (qntUnit != null)
						{
							MTRetailQtyAndPriceDataProvider.Param param = new MTRetailQtyAndPriceDataProvider.Param
							{
								MTCode = mtMoveWithConfirmationGridRow.MTCode,
								Storage = _storageExpense.Code,
								Date = _date.EditValue,
								UnitCoef = qntUnit.Coef
							};
							List<MTRetailQtyAndPriceDataProvider.DataRow> rows = new MTRetailQtyAndPriceDataProvider(TradeSettings.CreateTradeClient()).GetRows(param);
							if (rows.Count > 0)
							{
								mtMoveWithConfirmationGridRow.Price = rows[0].Price;
							}
							MTRetailQtyAndPrice dataView = new MTRetailQtyAndPrice(mtMoveWithConfirmationGridRow.MTCode, qntUnit.Coef, _storageExpense.Code, _date.EditValue);
							_priceCol.DataView = dataView;
						}
					}
					mtMoveWithConfirmationGridRow.Summa = (mtMoveWithConfirmationGridRow.Price * mtMoveWithConfirmationGridRow.Quantity).RoundAs(2);
					if (_distributeByFEFO)
					{
						decimal expSumm = 0m;
						DistributeQtyByExpiredDate(mtMoveWithConfirmationGridRow.RowID, _storageExpense.Code, mtMoveWithConfirmationGridRow.MTCode, mtMoveWithConfirmationGridRow.UnitCode, mtMoveWithConfirmationGridRow.Quantity, ref expSumm);
						mtMoveWithConfirmationGridRow.Summa = expSumm;
						mtMoveWithConfirmationGridRow.IsPartySelected = true;
					}
				}
				return;
			}
			catch (Exception ex)
			{
				mtMoveWithConfirmationGridRow.MTID = -1;
				mtMoveWithConfirmationGridRow.MTCode = "";
				mtMoveWithConfirmationGridRow.MTName = "";
				mtMoveWithConfirmationGridRow.UnitCode = "";
				mtMoveWithConfirmationGridRow.UnitBrief = "";
				mtMoveWithConfirmationGridRow.RowDescription = "";
				mtMoveWithConfirmationGridRow.Quantity = 0m;
				mtMoveWithConfirmationGridRow.Summa = 0m;
				if (e.Value != null && !string.IsNullOrWhiteSpace(e.Value.ToString()))
				{
					Settings.ErrorHandler(ex);
				}
				return;
			}
			finally
			{
				RowInfoChange?.Invoke(mtMoveWithConfirmationGridRow);
			}
		}
		if (e.Column == _unitCodeCol)
		{
			try
			{
				if (e.Value != null && !string.IsNullOrWhiteSpace(e.Value.ToString()))
				{
					QntUnitCacheRow item2 = TradeSettings.TradeCacheService.QntUnit.GetItem(e.Value.ToString(), throwExceptionNotExits: true);
					mtMoveWithConfirmationGridRow.UnitBrief = ((item2 == null) ? "" : item2.Brief);
					UnitChanged(e.Value.ToString(), text, mtMoveWithConfirmationGridRow);
					if (!_storageExpenseIsRetail && _storageIncomeIsRetail)
					{
						if (string.IsNullOrWhiteSpace(_pricesTypes.Code))
						{
							mtMoveWithConfirmationGridRow.Price = PriceHelper.GetPriceForUnit(text, Convert.ToString(e.Value), mtMoveWithConfirmationGridRow.Price, mtMoveWithConfirmationGridRow.MTCode);
						}
						else
						{
							PricesDataProvider.DataRow price = TradeSettings.CreateTradeClient().CommonServices.GetPrice(_pricesTypes.Code, mtMoveWithConfirmationGridRow.MTID, e.Value.ToString(), _date.EditValue.Value, "AMD");
							if (price.Price != 0m)
							{
								mtMoveWithConfirmationGridRow.Price = price.Price;
							}
							else
							{
								mtMoveWithConfirmationGridRow.Price = PriceHelper.GetPriceForUnit(text, Convert.ToString(e.Value), mtMoveWithConfirmationGridRow.Price, mtMoveWithConfirmationGridRow.MTCode);
							}
						}
					}
					mtMoveWithConfirmationGridRow.Summa = (mtMoveWithConfirmationGridRow.Price * mtMoveWithConfirmationGridRow.Quantity).RoundAs(2);
				}
				RowInfoChange?.Invoke(mtMoveWithConfirmationGridRow);
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
						PO.MTExpDocSelectedPartiesList.RemoveAll((PartyRow x) => x.GridRow == mtMoveWithConfirmationGridRow.RowID);
						decimal expSumm2 = 0m;
						DistributeQtyByExpiredDate(mtMoveWithConfirmationGridRow.RowID, _storageExpense.Code, mtMoveWithConfirmationGridRow.MTCode, mtMoveWithConfirmationGridRow.UnitCode, Convert.ToDecimal(e.Value), ref expSumm2);
						mtMoveWithConfirmationGridRow.Summa = expSumm2;
						mtMoveWithConfirmationGridRow.IsPartySelected = true;
					}
					else
					{
						if (mtMoveWithConfirmationGridRow.IsPartySelected)
						{
							ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Trade.Common.Resources.ForRowSelectedParties, Settings.ProgramName, MessageBoxButton.OK, MessageBoxImage.Asterisk);
						}
						PO.MTExpDocSelectedPartiesList.RemoveAll((PartyRow x) => x.GridRow == mtMoveWithConfirmationGridRow.RowID);
						mtMoveWithConfirmationGridRow.IsPartySelected = false;
					}
				}
				else if (_distributeByFEFO)
				{
					decimal expSumm3 = 0m;
					DistributeQtyByExpiredDate(mtMoveWithConfirmationGridRow.RowID, _storageExpense.Code, mtMoveWithConfirmationGridRow.MTCode, mtMoveWithConfirmationGridRow.UnitCode, Convert.ToDecimal(e.Value), ref expSumm3);
					mtMoveWithConfirmationGridRow.Summa = expSumm3;
					mtMoveWithConfirmationGridRow.IsPartySelected = true;
				}
			}
			if (_storageExpenseIsRetail || _storageIncomeIsRetail)
			{
				mtMoveWithConfirmationGridRow.Summa = mtMoveWithConfirmationGridRow.Quantity * mtMoveWithConfirmationGridRow.Price;
			}
			else if (!_distributeByFEFO)
			{
				mtMoveWithConfirmationGridRow.Summa = 0m;
			}
		}
		else if (e.Column == _priceCol && Convert.ToDecimal(e.OldValue) != Convert.ToDecimal(e.Value))
		{
			mtMoveWithConfirmationGridRow.Price = Convert.ToDecimal(e.Value);
			mtMoveWithConfirmationGridRow.Summa = (mtMoveWithConfirmationGridRow.Price * mtMoveWithConfirmationGridRow.Quantity).RoundAs(2);
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
				_storageExpenseAvailable = TradeSettings.CreateTradeClient().CommonServices.IsStorageAvailable(_storageExpense.Code);
				if (_mtMoveWithConfirmationList.Count > 0)
				{
					if (!_distributeByFEFO)
					{
						foreach (MTMoveSpecificationRow mtMoveWithConfirmation in _mtMoveWithConfirmationList)
						{
							if (mtMoveWithConfirmation.IsPartySelected)
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
						for (int i = 0; i <= _mtMoveWithConfirmationList.Count - 1; i++)
						{
							num = 0m;
							DistributeQtyByExpiredDate(_mtMoveWithConfirmationList[i].RowID, _storageExpense.Code, _mtMoveWithConfirmationList[i].MTCode, _mtMoveWithConfirmationList[i].UnitCode, _mtMoveWithConfirmationList[i].Quantity, ref num);
							_mtMoveWithConfirmationList[i].Summa = num;
							_mtMoveWithConfirmationList[i].IsPartySelected = true;
						}
					}
					else
					{
						for (int j = 0; j <= _mtMoveWithConfirmationList.Count - 1; j++)
						{
							_mtMoveWithConfirmationList[j].IsPartySelected = false;
							_mtMoveWithConfirmationList[j].Summa = 0m;
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
			if (_storageExpenseIsRetail && _mtMoveWithConfirmationList.Count != 0)
			{
				foreach (MTMoveSpecificationRow mtMoveWithConfirmation2 in _mtMoveWithConfirmationList)
				{
					mtMoveWithConfirmation2.Price = 0m;
					mtMoveWithConfirmation2.Summa = 0m;
				}
			}
			RowInfoChange?.Invoke((MTMoveSpecificationRow)_mtMoveWithConfirmationTableView.Grid.CurrentItem);
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
				return;
			}
			_storageIncomeIsRetail = TradeSettings.TradeCacheService.Storage.GetItem(_storageIncome.Code)?.Retail ?? false;
			_storageIncomeAvailable = TradeSettings.CreateTradeClient().CommonServices.IsStorageAvailable(_storageIncome.Code);
			SetPriceSumColumnsReadOnlyProperty();
			_confirmed.IsEnabled = _storageIncomeAvailable;
			_actualQuantityCol.ReadOnly = !_storageIncomeAvailable;
			_actualQuantitiesCheck.IsEnabled = _storageIncomeAvailable && _showStocktakingColumns;
		}
	}

	private void SetStocktakingColumnsVisibility()
	{
		_actualQuantityCol.Visible = _showStocktakingColumns;
		_varianceQuantityCol.Visible = _showStocktakingColumns;
		_actualQuantityCol.ShowInColumnChooser = _actualQuantityCol.Visible;
		_varianceQuantityCol.ShowInColumnChooser = _varianceQuantityCol.Visible;
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
			string barCode = _barCode.Text.Trim();
			MaterialCacheRow materialCacheRow = DocumentHelpers.TryGetItemByBarCode(ref barCode, out var qty, out var unitCode, out var unitBrief);
			if (materialCacheRow == null)
			{
				ControlsHelper.SetFocus(_barCode);
				throw new ASException(string.Format(ArmSoft.SME.Common.Resources.ErrBarCodeNotExist, _barCode.Text.Trim()));
			}
			MTMoveSpecificationRow gridRow = null;
			AddMTToList(materialCacheRow, ref gridRow, unitCode, unitBrief, qty);
			_barCode.Text = "";
			RowInfoChangeEventHandler rowInfoChangeEventHandler = RowInfoChange;
			if (rowInfoChangeEventHandler != null)
			{
				ObservableCollection<MTMoveSpecificationRow> mtMoveWithConfirmationList = _mtMoveWithConfirmationList;
				rowInfoChangeEventHandler(mtMoveWithConfirmationList[mtMoveWithConfirmationList.Count - 1]);
			}
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
		if (forFocusedRow && _mtMoveWithConfirmationList.Count != 0)
		{
			row = (MTMoveSpecificationRow)_mtMoveWithConfirmationTableView.Grid.CurrentItem;
			if (row == null)
			{
				return;
			}
		}
		for (int i = 0; i <= _mtMoveWithConfirmationList.Count - 1; i++)
		{
			if ((!forFocusedRow || row.RowID == _mtMoveWithConfirmationList[i].RowID) && goodRow.Id == _mtMoveWithConfirmationList[i].MTID && unitCode == _mtMoveWithConfirmationList[i].UnitCode && !_mtMoveWithConfirmationList[i].IsPartySelected)
			{
				row = _mtMoveWithConfirmationList[i];
				FillMTInvGridRow(row, goodRow, unitCode, unitBrief, quantity);
				int rowHandleByListIndex = _mtMoveWithConfirmationGrid.Grid.GetRowHandleByListIndex(i);
				POWindow<MTMoveWithConfirmation>.SelectCellInGrid(_mtMoveWithConfirmationTableView, _mtMoveWithConfirmationGrid, rowHandleByListIndex, "Quantity");
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
		_mtMoveWithConfirmationList.Add(row);
		_maxRowID++;
		int rowHandleByListIndex = _mtMoveWithConfirmationGrid.Grid.GetRowHandleByListIndex(_mtMoveWithConfirmationList.Count - 1);
		if (row != null && _distributeByFEFO)
		{
			PO.MTExpDocSelectedPartiesList.RemoveAll((PartyRow x) => x.GridRow == row.RowID);
			decimal expSumm = 0m;
			DistributeQtyByExpiredDate(row.RowID, _storageExpense.Code, goodRow.MTCode, row.UnitCode, row.Quantity, ref expSumm, goodRow.Unit);
			row.Summa = expSumm;
			row.IsPartySelected = true;
		}
		POWindow<MTMoveWithConfirmation>.SelectCellInGrid(_mtMoveWithConfirmationTableView, _mtMoveWithConfirmationGrid, rowHandleByListIndex, "Quantity");
		_mtMoveWithConfirmationGrid.ConfigureButtons();
		RowInfoChange?.Invoke(row);
	}

	private void FillMTInvGridRow(MTMoveSpecificationRow mtMoveWithConfirmationGridRow, MaterialCacheRow goodsRow, string unitCode, string unitBrief, decimal quantity = 1m)
	{
		if (goodsRow != null)
		{
			mtMoveWithConfirmationGridRow.MTID = goodsRow.Id;
			mtMoveWithConfirmationGridRow.MTCode = goodsRow.MTCode;
			mtMoveWithConfirmationGridRow.MTName = goodsRow.Caption;
			mtMoveWithConfirmationGridRow.UnitCode = unitCode;
			mtMoveWithConfirmationGridRow.UnitBrief = unitBrief;
			mtMoveWithConfirmationGridRow.Quantity += quantity;
			if (mtMoveWithConfirmationGridRow.Price == 0m)
			{
				mtMoveWithConfirmationGridRow.Price = GetRowPrice(mtMoveWithConfirmationGridRow);
			}
			mtMoveWithConfirmationGridRow.Summa = (mtMoveWithConfirmationGridRow.Price * mtMoveWithConfirmationGridRow.Quantity).RoundAs(2);
			mtMoveWithConfirmationGridRow.IsPartySelected = false;
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
		if (_mtMoveWithConfirmationList.Count > 0)
		{
			FillDocument(new StoreRequest());
			new PrintLabelDialog(this, PO).Invoke();
		}
	}

	private void BarButtonPriceFormation_ItemClick(object sender, ItemClickEventArgs e)
	{
		List<PricesFormationWindow.GoodsRow> list = new List<PricesFormationWindow.GoodsRow>();
		List<SelectedMaterialsRow> list2 = new List<SelectedMaterialsRow>();
		foreach (MTMoveSpecificationRow mtMoveWithConfirmation in _mtMoveWithConfirmationList)
		{
			MaterialCacheRow item = TradeSettings.TradeCacheService.Material.GetItem(mtMoveWithConfirmation.MTCode);
			list.Add(new PricesFormationWindow.GoodsRow(mtMoveWithConfirmation.MTID, mtMoveWithConfirmation.MTCode, mtMoveWithConfirmation.UnitCode, item.AltUnit));
			SelectedMaterialsRow selectedMaterialsRow = new SelectedMaterialsRow
			{
				RowNum = mtMoveWithConfirmation.RowNum,
				MTID = mtMoveWithConfirmation.MTID,
				MTCode = mtMoveWithConfirmation.MTCode,
				MTName = mtMoveWithConfirmation.MTName
			};
			if (mtMoveWithConfirmation.UnitCode != item.Unit)
			{
				MaterialCacheRow.QntUnitRow qntUnit = TradeSettings.TradeCacheService.Material.GetQntUnit(mtMoveWithConfirmation.MTID, mtMoveWithConfirmation.UnitCode);
				MaterialCacheRow.QntUnitRow qntUnit2 = TradeSettings.TradeCacheService.Material.GetQntUnit(mtMoveWithConfirmation.MTID, item.Unit);
				if (qntUnit != null && qntUnit2 != null)
				{
					selectedMaterialsRow.QntUnit = qntUnit2.QntUnit;
					selectedMaterialsRow.QntUnitName = TradeSettings.TradeCacheService.QntUnit.GetItem(qntUnit2.QntUnit).Brief;
					if (qntUnit.Coef != 0m)
					{
						selectedMaterialsRow.PurchasePrice = (mtMoveWithConfirmation.Price * qntUnit2.Coef / qntUnit.Coef).RoundAs(2);
						selectedMaterialsRow.SalePrice = selectedMaterialsRow.PurchasePrice;
					}
				}
			}
			else
			{
				selectedMaterialsRow.QntUnit = mtMoveWithConfirmation.UnitCode;
				selectedMaterialsRow.QntUnitName = mtMoveWithConfirmation.UnitBrief;
				selectedMaterialsRow.PurchasePrice = mtMoveWithConfirmation.Price;
				selectedMaterialsRow.SalePrice = mtMoveWithConfirmation.Price;
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
			if (_mtMoveWithConfirmationList.Count <= 0 || ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Trade.Common.Resources.LoadMaterialsList, Settings.ProgramName, MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No) != MessageBoxResult.No)
			{
				_mtMoveWithConfirmationList.Clear();
				PO.MTExpDocSelectedPartiesList.Clear();
				_maxRowID = 0;
				TerminalHelper.TerminalImporter terminalImporter = new TerminalHelper.TerminalImporter(this);
				MTMoveSpecificationRow mtMoveWithConfirmationGridRow = null;
				if (terminalImporter.ImportFromFile((decimal quantity, MaterialCacheRow goodRow, MaterialCacheRow.BarCodeRow goodBarcodeRow) =>
				{
					AddMTToListNewRow(goodRow, mtMoveWithConfirmationGridRow, goodBarcodeRow.QntUnit, TradeSettings.TradeCacheService.QntUnit.GetItem(goodBarcodeRow.QntUnit).Brief, quantity);
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

	protected override void CreateChildDocument(short childDocType, MTMoveWithConfirmation baseDoc)
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
		_mtMoveWithConfirmationList.Clear();
		_maxRowID = 0;
		PO.MTExpDocSelectedPartiesList.Clear();
		if (BaseDocument.POType == DocumentTypeSE.MTIncomeImport.Value)
		{
			MTIncomeImport mTIncomeImport = (MTIncomeImport)BaseDocument;
			_storageExpense.Code = mTIncomeImport.Storage;
			_storageExpenseIsRetail = TradeSettings.TradeCacheService.Storage.GetItem(_storageExpense.Code)?.Retail ?? false;
			_storageExpenseAvailable = TradeSettings.CreateTradeClient().CommonServices.IsStorageAvailable(_storageExpense.Code);
			if (!string.IsNullOrWhiteSpace(_storageIncome.Code))
			{
				_storageIncomeIsRetail = TradeSettings.TradeCacheService.Storage.GetItem(_storageIncome.Code)?.Retail ?? false;
				_storageIncomeAvailable = TradeSettings.CreateTradeClient().CommonServices.IsStorageAvailable(_storageIncome.Code);
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
				_mtMoveWithConfirmationList.Add(new MTMoveSpecificationRow
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
			_storageExpenseAvailable = TradeSettings.CreateTradeClient().CommonServices.IsStorageAvailable(_storageExpense.Code);
			_storageIncomeIsRetail = TradeSettings.TradeCacheService.Storage.GetItem(_storageIncome.Code)?.Retail ?? false;
			_storageIncomeAvailable = TradeSettings.CreateTradeClient().CommonServices.IsStorageAvailable(_storageIncome.Code);
			foreach (MTMoveBase.MTMoveSpecificationRow mT2 in mTMoveOrder.MTList)
			{
				MTMoveSpecificationRow mTMoveSpecificationRow = new MTMoveSpecificationRow
				{
					MTCode = mT2.MTCode,
					MTName = mT2.MTName,
					Quantity = mT2.Quantity,
					MTID = mT2.MTID,
					UnitCode = mT2.UnitCode,
					UnitBrief = mT2.UnitBrief,
					IsPartySelected = false,
					RowID = _maxRowID
				};
				mTMoveSpecificationRow.Price = GetRowPrice(mTMoveSpecificationRow, fromMoveOrder: true);
				mTMoveSpecificationRow.Summa = (mTMoveSpecificationRow.Price * mTMoveSpecificationRow.Quantity).RoundAs(2);
				_mtMoveWithConfirmationList.Add(mTMoveSpecificationRow);
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
				_mtMoveWithConfirmationList.Add(mTMoveSpecificationRow2);
				_maxRowID++;
			}
		}
		else
		{
			MTIncomeBase mTIncomeBase = (MTIncomeBase)BaseDocument;
			_storageExpense.Code = mTIncomeBase.Storage;
			_storageExpenseIsRetail = TradeSettings.TradeCacheService.Storage.GetItem(_storageExpense.Code)?.Retail ?? false;
			_storageExpenseAvailable = TradeSettings.CreateTradeClient().CommonServices.IsStorageAvailable(_storageExpense.Code);
			if (!string.IsNullOrWhiteSpace(_storageIncome.Code))
			{
				_storageIncomeIsRetail = TradeSettings.TradeCacheService.Storage.GetItem(_storageIncome.Code)?.Retail ?? false;
				_storageIncomeAvailable = TradeSettings.CreateTradeClient().CommonServices.IsStorageAvailable(_storageIncome.Code);
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
				_mtMoveWithConfirmationList.Add(mTMoveSpecificationRow3);
				_maxRowID++;
			}
		}
		if (_storageExpenseIsRetail)
		{
			_pricesTypes.Code = "";
		}
		_pricesTypes.IsReadOnly = _storageExpenseIsRetail;
		SetPriceSumColumnsReadOnlyProperty();
		_mtMoveWithConfirmationGrid.Grid.RefreshData();
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
		if (_storageIncomeIsRetail && _isAskQuestionConfirmSummChange && _mtMoveWithConfirmationList.Count != 0 && Convert.ToString(e.NewValue) != Convert.ToString(e.OldValue) && !string.IsNullOrWhiteSpace(_pricesTypes.Code) && _pricesTypes.DoValidate() && ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Trade.Common.Resources.ValidateChangePrices, Settings.ProgramName, MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No) == MessageBoxResult.Yes)
		{
			UpdateMTMoveWithConfirmationPrices();
		}
	}

	private void UpdateMTMoveWithConfirmationPrices()
	{
		if (_mtMoveWithConfirmationList.Count == 0)
		{
			return;
		}
		foreach (MTMoveSpecificationRow mtMoveWithConfirmation in _mtMoveWithConfirmationList)
		{
			mtMoveWithConfirmation.Price = PriceHelper.GetPriceForGivenPriceListAndVATType(_pricesTypes.Code, "2", mtMoveWithConfirmation.MTID, mtMoveWithConfirmation.UnitCode, _date.EditValue.Value, "AMD");
			mtMoveWithConfirmation.Summa = (mtMoveWithConfirmation.Price * mtMoveWithConfirmation.Quantity).RoundAs(2);
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
			ResultsList = new MTMoveWithConfirmationFilteredSearchList()
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
				_mtMoveWithConfirmationList.Add(item);
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
		RowInfoChange?.Invoke((MTMoveSpecificationRow)_mtMoveWithConfirmationTableView.Grid.CurrentItem);
	}

	private void MTMoveWithConfirmationGrid_RefreshButtons(object sender, EventArgs e)
	{
		_filteredSearch.IsEnabled = _mtMoveWithConfirmationGrid.AllowAdd && !_mtMoveWithConfirmationGrid.IsReadOnly;
		_actualQuantitiesCheck.IsEnabled = _storageIncomeAvailable && _showStocktakingColumns;
	}

	private void ActualQuantitiesCheck_Click(object sender, RoutedEventArgs e)
	{
		_mtMoveWithConfirmationTableView.CommitEditing();
		ValidateDocFields();
		ActualQuantitiesCheckWindow actualQuantitiesCheckWindow = new ActualQuantitiesCheckWindow(this, _mtMoveWithConfirmationList, _date.EditValue.Value, _docNum.EditValue.ToString());
		if (!actualQuantitiesCheckWindow.ShowDialog().Value)
		{
			return;
		}
		foreach (var item2 in (from x in actualQuantitiesCheckWindow.QCResult
			group x by new { x.MTCode, x.UnitCode } into x
			select (MTCode: x.Key.MTCode, UnitCode: x.Key.UnitCode, x.Sum((ActualQuantitiesCheck.ActualQuantitiesCheckRow z) => z.VarianceQuantity))).ToList())
		{
			string mtCode = item2.MTCode;
			string unitCode = item2.UnitCode;
			decimal item = item2.Item3;
			List<MTMoveSpecificationRow> list = _mtMoveWithConfirmationList.Where((MTMoveSpecificationRow x) => x.MTCode == mtCode && x.UnitCode == unitCode).ToList();
			if (list.Count <= 0)
			{
				continue;
			}
			if (item < 0m)
			{
				decimal num = item;
				for (int num2 = list.Count - 1; num2 >= 0; num2--)
				{
					MTMoveSpecificationRow mTMoveSpecificationRow = list[num2];
					if (num + mTMoveSpecificationRow.Quantity < 0m)
					{
						mTMoveSpecificationRow.ActualQuantity = 0m;
						num += mTMoveSpecificationRow.Quantity;
					}
					else
					{
						mTMoveSpecificationRow.ActualQuantity = num + mTMoveSpecificationRow.Quantity;
						num = 0m;
					}
				}
				continue;
			}
			for (int num3 = 0; num3 <= list.Count - 1; num3++)
			{
				MTMoveSpecificationRow mTMoveSpecificationRow2 = list[num3];
				if (num3 == 0)
				{
					mTMoveSpecificationRow2.ActualQuantity = item + mTMoveSpecificationRow2.Quantity;
				}
				else
				{
					mTMoveSpecificationRow2.ActualQuantity = mTMoveSpecificationRow2.Quantity;
				}
			}
		}
		IsChanged = true;
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
		RowInfoChangeEventHandler rowInfoChangeEventHandler = RowInfoChange;
		if (rowInfoChangeEventHandler != null)
		{
			ObservableCollection<MTMoveSpecificationRow> mtMoveWithConfirmationList = _mtMoveWithConfirmationList;
			rowInfoChangeEventHandler(mtMoveWithConfirmationList[mtMoveWithConfirmationList.Count - 1]);
		}
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
		CheckEditExt checkEditExt = dialogWindow.AddBooleanControl(layoutGroup, "DistributByFEFO", ArmSoft.SME.Trade.Common.Resources.FEFODistibuteLong);
		checkEditExt.IsChecked = _distributeByFEFO;
		CheckEditExt checkEditExt2 = dialogWindow.AddBooleanControl(layoutGroup, "ShowStocktakingColumns", ArmSoft.SME.Trade.Common.Resources.ShowStocktakingColumns);
		checkEditExt2.IsChecked = _showStocktakingColumns;
		if (dialogWindow.ShowDialog().Value)
		{
			_distributeByFEFO = checkEditExt.IsChecked.Value;
			_showStocktakingColumns = checkEditExt2.IsChecked.Value;
			_actualQuantitiesCheck.IsEnabled = _storageIncomeAvailable && _showStocktakingColumns;
			SetStocktakingColumnsVisibility();
		}
	}

	protected override void SetReportBarButtonsEnable(bool isEnabled)
	{
		base.SetReportBarButtonsEnable(isEnabled);
		_barButtonJumpTo.IsEnabled = isEnabled;
		_barButtonGoodsOperationsJournal.IsEnabled = isEnabled;
		_barButtonGoodsRetailOperationJournal.IsEnabled = isEnabled;
	}

	private void ChangeRowInfo(MTMoveSpecificationRow row = null)
	{
		if (row == null)
		{
			((DispatcherObject)this).Dispatcher.BeginInvoke((Delegate)new DelegateClearInfoPanel(CleanInfoPanel), Array.Empty<object>());
			return;
		}
		((DispatcherObject)this).Dispatcher.BeginInvoke((Delegate)new DelegateSetRowInfoToInfoPanel(SetRowInfoToPanel), new object[1] { row });
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

	private void MTMoveWithConfirmationTableView_FocusedRowHandleChanged(object sender, FocusedRowHandleChangedEventArgs e)
	{
		int focusedRowHandle = _mtMoveWithConfirmationTableView.FocusedRowHandle;
		MTMoveSpecificationRow mTMoveSpecificationRow = (MTMoveSpecificationRow)_mtMoveWithConfirmationTableView.Grid.GetRow(focusedRowHandle);
		if (mTMoveSpecificationRow != null)
		{
			ChangeColumnAdditonalView(mTMoveSpecificationRow.MTCode);
			RowInfoChange?.Invoke(mTMoveSpecificationRow);
		}
	}

	private void ChangeColumnAdditonalView(string itemCode)
	{
		_unitCodeCol.DataView = new MTQntUnits(string.IsNullOrWhiteSpace(itemCode) ? "" : itemCode);
	}

	protected override void StoreSettings(RegistryKey rk)
	{
		base.StoreSettings(rk);
		rk.SetValue("DistributByFEFO", _distributeByFEFO ? "1" : "0");
		rk.SetValue("ShowStocktakingColumns", _showStocktakingColumns ? "1" : "0");
		_mtMoveWithConfirmationGrid.StoreSettings(rk);
	}

	protected override void RestoreSettings(double defaultWidth, double defaultHeight, RegistryKey rk)
	{
		base.RestoreSettings(defaultWidth, defaultHeight, rk);
		if (rk != null)
		{
			_distributeByFEFO = rk.GetValue("DistributByFEFO", false).ToString() == "1";
			_showStocktakingColumns = rk.GetValue("ShowStocktakingColumns", false).ToString() == "1";
			_mtMoveWithConfirmationGrid.RestoreSettings(rk);
		}
	}

	private void MotorVehicle_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		_brand.IsRequired = _motorVehicle.IsChecked.Value;
		_brand.IsReadOnly = !_motorVehicle.IsChecked.Value;
		_licensePlate.IsRequired = _motorVehicle.IsChecked.Value;
		_licensePlate.IsReadOnly = !_motorVehicle.IsChecked.Value;
	}

	private void Confirmed_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		Confirmed_EditValueChanged();
	}

	private void Confirmed_EditValueChanged()
	{
		if (!StoredInDb)
		{
			_status.Text = MTMoveStatusSE.None.Caption;
		}
		else if (_confirmed.IsChecked.Value)
		{
			_status.Text = MTMoveStatusSE.Completed.Caption;
		}
		else
		{
			_status.Text = MTMoveStatusSE.OnWay.Caption;
		}
	}
}
