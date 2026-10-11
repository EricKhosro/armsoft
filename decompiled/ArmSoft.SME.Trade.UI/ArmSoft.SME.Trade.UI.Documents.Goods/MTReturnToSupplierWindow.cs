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
using ArmSoft.SME.Common.CustomTypes;
using ArmSoft.SME.Common.Extensions;
using ArmSoft.SME.Common.Models;
using ArmSoft.SME.Common.Models.Documents;
using ArmSoft.SME.Common.Parameters;
using ArmSoft.SME.Common.PersistableObject;
using ArmSoft.SME.Common.SmartEnums;
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
using ArmSoft.SME.Trade.Common;
using ArmSoft.SME.Trade.Common.CustomTypes;
using ArmSoft.SME.Trade.Common.Documents.Definition;
using ArmSoft.SME.Trade.Common.Helpers;
using ArmSoft.SME.Trade.Common.Models.Caches;
using ArmSoft.SME.Trade.Common.Models.Documents;
using ArmSoft.SME.Trade.Common.Parameters;
using ArmSoft.SME.Trade.Common.PersistableObject;
using ArmSoft.SME.Trade.Common.SmartEnums;
using ArmSoft.SME.Trade.UI.CustomUIElements;
using ArmSoft.SME.Trade.UI.CustomUIElements.UserControls;
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

namespace ArmSoft.SME.Trade.UI.Documents.Goods;

[DocumentWindow(14)]
public class MTReturnToSupplierWindow : TradeDocumentWindow<MTReturnToSupplier>
{
	public class MTRetToSupSpecificationRow : DocumentGridRow
	{
		[ObservableProperty]
		private string storage;

		[ObservableProperty]
		private string mTCode = string.Empty;

		[ObservableProperty]
		private int mTID = -1;

		[ObservableProperty]
		private string unitCode = string.Empty;

		[ObservableProperty]
		private string unitBrief;

		[ObservableProperty]
		private decimal quantity;

		[ObservableProperty]
		private decimal discount;

		[ObservableProperty]
		private decimal discountPrice;

		[ObservableProperty]
		private decimal price;

		[ObservableProperty]
		private decimal summa;

		[ObservableProperty]
		private decimal salePrice;

		[ObservableProperty]
		private decimal saleSumma;

		[ObservableProperty]
		private string vatCodeForConvert;

		[ObservableProperty]
		private bool vAT;

		[ObservableProperty]
		private short rowNum;

		[ObservableProperty]
		private int rowID;

		[ObservableProperty]
		private bool isPartySelected;

		[ObservableProperty]
		private string mTName;

		[ObservableProperty]
		private string fullName;

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
		public string Storage
		{
			get
			{
				return storage;
			}
			[MemberNotNull("storage")]
			set
			{
				if (!EqualityComparer<string>.Default.Equals(storage, value))
				{
					OnPropertyChanging(__KnownINotifyPropertyChangingArgs.Storage);
					storage = value;
					OnStorageChanged(value);
					OnPropertyChanged(__KnownINotifyPropertyChangedArgs.Storage);
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
		public decimal Discount
		{
			get
			{
				return discount;
			}
			set
			{
				if (!EqualityComparer<decimal>.Default.Equals(discount, value))
				{
					OnPropertyChanging(__KnownINotifyPropertyChangingArgs.Discount);
					discount = value;
					OnPropertyChanged(__KnownINotifyPropertyChangedArgs.Discount);
				}
			}
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public decimal DiscountPrice
		{
			get
			{
				return discountPrice;
			}
			set
			{
				if (!EqualityComparer<decimal>.Default.Equals(discountPrice, value))
				{
					OnPropertyChanging(__KnownINotifyPropertyChangingArgs.DiscountPrice);
					discountPrice = value;
					OnDiscountPriceChanged(value);
					OnPropertyChanged(__KnownINotifyPropertyChangedArgs.DiscountPrice);
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
		public decimal SaleSumma
		{
			get
			{
				return saleSumma;
			}
			set
			{
				if (!EqualityComparer<decimal>.Default.Equals(saleSumma, value))
				{
					OnPropertyChanging(__KnownINotifyPropertyChangingArgs.SaleSumma);
					saleSumma = value;
					OnPropertyChanged(__KnownINotifyPropertyChangedArgs.SaleSumma);
				}
			}
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public string VatCodeForConvert
		{
			get
			{
				return vatCodeForConvert;
			}
			[MemberNotNull("vatCodeForConvert")]
			set
			{
				if (!EqualityComparer<string>.Default.Equals(vatCodeForConvert, value))
				{
					OnPropertyChanging(__KnownINotifyPropertyChangingArgs.VatCodeForConvert);
					vatCodeForConvert = value;
					OnPropertyChanged(__KnownINotifyPropertyChangedArgs.VatCodeForConvert);
				}
			}
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public bool VAT
		{
			get
			{
				return vAT;
			}
			set
			{
				if (!EqualityComparer<bool>.Default.Equals(vAT, value))
				{
					OnPropertyChanging(__KnownINotifyPropertyChangingArgs.VAT);
					vAT = value;
					OnPropertyChanged(__KnownINotifyPropertyChangedArgs.VAT);
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

		static MTRetToSupSpecificationRow()
		{
			DocumentGridRow.RegisterPropertyValidators(typeof(MTRetToSupSpecificationRow), cacheValidationResults: true, new Dictionary<string, Action<DocumentGridRow, DevExpress.XtraEditors.DXErrorProvider.ErrorInfo>>
			{
				{
					"Storage",
					(DocumentGridRow row, DevExpress.XtraEditors.DXErrorProvider.ErrorInfo info) =>
					{
						ValidateStorage((MTRetToSupSpecificationRow)row, info);
					}
				},
				{
					"MTCode",
					(DocumentGridRow row, DevExpress.XtraEditors.DXErrorProvider.ErrorInfo info) =>
					{
						ValidateGoodCode((MTRetToSupSpecificationRow)row, info);
					}
				},
				{
					"UnitCode",
					(DocumentGridRow row, DevExpress.XtraEditors.DXErrorProvider.ErrorInfo info) =>
					{
						ValidateUnitCode((MTRetToSupSpecificationRow)row, info);
					}
				},
				{
					"Quantity",
					(DocumentGridRow row, DevExpress.XtraEditors.DXErrorProvider.ErrorInfo info) =>
					{
						ValidateQuantity((MTRetToSupSpecificationRow)row, info);
					}
				},
				{
					"Summa",
					(DocumentGridRow row, DevExpress.XtraEditors.DXErrorProvider.ErrorInfo info) =>
					{
						ValidateSumma((MTRetToSupSpecificationRow)row, info);
					}
				}
			});
		}

		private static void ValidateStorage(MTRetToSupSpecificationRow row, DevExpress.XtraEditors.DXErrorProvider.ErrorInfo info)
		{
			TradePropertyValidator.ValidateStorage(row.Storage, info, TradeAllDocDefinitions.MTReturnToSupplier.Grids["MTList"].Columns["Storage"]);
		}

		private static void ValidateGoodCode(MTRetToSupSpecificationRow row, DevExpress.XtraEditors.DXErrorProvider.ErrorInfo info)
		{
			TradePropertyValidator.ValidateGoodCode(row.MTCode, info, (StringFieldDefinition)TradeAllDocDefinitions.MTReturnToSupplier.Grids["MTList"].Columns["MTCode"]);
		}

		private static void ValidateUnitCode(MTRetToSupSpecificationRow row, DevExpress.XtraEditors.DXErrorProvider.ErrorInfo info)
		{
			TradePropertyValidator.ValidateGoodUnitCode(row.UnitCode, row.MTCode, info, (StringFieldDefinition)TradeAllDocDefinitions.MTReturnToSupplier.Grids["MTList"].Columns["UnitCode"]);
		}

		private static void ValidateQuantity(MTRetToSupSpecificationRow row, DevExpress.XtraEditors.DXErrorProvider.ErrorInfo info)
		{
			PropertyValidator.PositiveValidate(row.Quantity, info, ArmSoft.SME.Common.Resources.Quantity);
		}

		private static void ValidateSumma(MTRetToSupSpecificationRow row, DevExpress.XtraEditors.DXErrorProvider.ErrorInfo info)
		{
			if (Settings.ParametersService.AllowPurchaseWith0PriceOr100Percent())
			{
				PropertyValidator.NonNegativeValidate(row.Summa, info, ArmSoft.SME.Common.Resources.Summa);
			}
			else
			{
				PropertyValidator.PositiveValidate(row.Summa, info, ArmSoft.SME.Common.Resources.Summa);
			}
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		private void OnStorageChanged(string value)
		{
			ResetForceValidationBit("Storage", "Storage");
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		private void OnMTCodeChanged(string value)
		{
			ResetForceValidationBit("MTCode");
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
		private void OnDiscountPriceChanged(decimal value)
		{
			ResetForceValidationBit("DiscountPrice");
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		private void OnPriceChanged(decimal value)
		{
			ResetForceValidationBit("Price", "Summa");
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		private void OnSummaChanged(decimal value)
		{
			ResetForceValidationBit("Summa");
		}
	}

	public class MTReturnToSupplierFilteredSearchList : IResultList
	{
		private readonly MTReturnToSupplierWindow _doc;

		private readonly ObservableCollection<MTRetToSupSpecificationRow> _itemsSource;

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

		public IEnumerable<DocumentGridRow> ItemsSource => _itemsSource;

		public MTReturnToSupplierFilteredSearchList(MTReturnToSupplierWindow doc)
		{
			_doc = doc;
			_itemsSource = new ObservableCollection<MTRetToSupSpecificationRow>();
		}

		public int AddToItemsSource(object key, params object[] args)
		{
			string mtCode = key.ToString();
			decimal num = Convert.ToDecimal(args[0]);
			int num2 = _itemsSource.FindIndex((MTRetToSupSpecificationRow x) => x.MTCode == mtCode);
			if (num2 == -1)
			{
				MaterialCacheRow item = TradeSettings.TradeCacheService.Material.GetItem(mtCode);
				MTRetToSupSpecificationRow item2 = new MTRetToSupSpecificationRow
				{
					MTID = item.Id,
					MTCode = item.MTCode,
					MTName = item.Caption,
					UnitCode = item.Unit,
					VAT = item.VAT,
					IsPartySelected = false
				};
				_itemsSource.Add(item2);
				num2 = _itemsSource.Count - 1;
			}
			_itemsSource[num2].Quantity += num;
			return num2;
		}
	}

	protected delegate void RowInfoChangeEventHandler(MTRetToSupSpecificationRow Row);

	private delegate void DelegateSetRowInfoToInfoPanel(MTRetToSupSpecificationRow Row);

	private delegate void DelegateClearInfoPanel();

	private DropDownCommentControl _baseDocumentControl;

	private TextEditExt _barCode;

	private Button _codeEnter;

	private DXTabControl _tabs;

	private BarEditItemExt _viewAvailAndTurn;

	private PartnerContractGroupControls _supplierContractContainer;

	private TextEditExt _supBook;

	private PopupCalcEditExt _supBookPage;

	private PopupCalcEditExt _supBookRow;

	private DropDownControl _salesConsultant;

	private ComboBoxEditExt _comment;

	private DropDownControl _owner;

	private TextEditExt _baseDocTaxInvoiceSeries;

	private TextEditExt _baseDocTaxInvoiceNumber;

	private DateEditControl _baseDocTaxInvoiceDate;

	private TextEditExt _taxInvoiceSeries;

	private TextEditExt _taxInvoiceNumber;

	private TextEditExt _supplierName;

	private DropDownCommentControl _supplierBusinessAddress;

	private DropDownControl _supplierSettlAcc;

	private TextEditExt _supplierTaxCode;

	private ComboBoxEditExt _supplierMngPost;

	private TextEditExt _supplierMngName;

	private ComboBoxEditExt _supplierChiefAccPost;

	private TextEditExt _supplierChiefAccName;

	private TextEditExt _buyerStateRegNum;

	private TextEditExt _buyerPassport;

	private TextEditExt _buyerName;

	private TextEditExt _buyerLegalAddress;

	private ComboBoxEditExt _buyerBusinessAddress;

	private TextEditExt _supplierLegalAddress;

	private DropDownControl _buyerSettlAcc;

	private TextEditExt _buyerTaxCode;

	private ComboBoxEditExt _buyerMngPost;

	private TextEditExt _buyerMngName;

	private ComboBoxEditExt _buyerChiefAccPost;

	private TextEditExt _buyerChiefAccName;

	public DropDownControl _currCode;

	private ExchangeRateControl _currExChngRate;

	public DropDownControl _vatType;

	public DropDownControl _priceListType;

	public DropDownControl _storage;

	public PopupCalcEditExt _discountPercent;

	private PopupCalcEditExt _vatSumma;

	private PopupCalcEditExt _totalSumma;

	public DocumentGridControl _mtRetToSupGrid;

	private TableView _mtRetToSupTableView;

	private BarEditItemExt _filteredSearch;

	private BarEditItemExt _fillLatestPurchasePrices;

	public ObservableCollection<MTRetToSupSpecificationRow> _mtRetToSupList;

	private DocumentGridColumn _storageCol;

	private DocumentGridColumn _mtCodeCol;

	private DocumentGridColumn _quantityCol;

	private DocumentGridColumn _unitCodeCol;

	private DocumentGridColumn _unitCol;

	private DocumentGridColumn _priceCol;

	private DocumentGridColumn _discountCol;

	private DocumentGridColumn _discountPriceCol;

	private DocumentGridColumn _summaCol;

	private DocumentGridColumn _salePriceCol;

	private DocumentGridColumn _saleSummaCol;

	private DocumentGridColumn _vatCol;

	private DocumentGridColumn _partyCol;

	private bool _isChanged = true;

	private TextEditExt _infoItemName;

	private InfoDisplayControls _infoDisplayControls;

	private LayoutGroup _infoDisplay;

	private Dictionary<(int, string), decimal> _availabilityByGoodId = new Dictionary<(int, string), decimal>();

	private SafeDocumentEventHandler<FocusedRowHandleChangedEventArgs> _focusedRowHandleChangedEvent;

	private int _priceListTabControlIndex;

	public int _maxRowID;

	private Guid _baseISN = Guid.Empty;

	private Document _baseDocument;

	private string _mOldDocCurrCode;

	private decimal _mOldDocCurrExChngRate = 1m;

	private decimal _mOldDocCurrExChngBase = 1m;

	private bool _isAskQuestionConfirmSummChange = true;

	private string _rowAddMode = Settings.ParametersService.ScannerMode();

	private readonly bool _isParamAllowDiscountInReturn = Settings.ParametersService.AllowDiscountInPurchase();

	public decimal _useDiscount;

	private ReportBarButtonItem _barButtonJumpTo;

	private ReportBarButtonItem _barButtonGoodsOperationsJournal;

	private ReportBarButtonItem _barButtonGoodsRetailOperationJournal;

	private ReportBarButtonItem _barButtonPartnerOperationJournal;

	private BarSubItem _barSubItemActions = new BarSubItem();

	private BarSubItem _barSubItemJournals = new BarSubItem();

	private bool _allowPurchaseWith0PriceOr100Percent;

	protected override double WidthValue => 1090.0;

	protected override double HeightValue => 440.0;

	protected override double MinWidthValue => 700.0;

	protected override double MinHeightValue => 250.0;

	private Document BaseDocument
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
				_baseDocumentControl.Text = TradeSettings.CreateTradeClient().CommonServices.GetDocumentDescription(BaseISN, ArmSoft.SME.Common.Constants.Common.DocumentDescriptionMode.TypeDateNum);
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
			if (ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Trade.Common.Resources.FillUsingBaseQ, Settings.ProgramName, MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.Yes) == MessageBoxResult.Yes)
			{
				CreateBasedOnDoc();
			}
		}
	}

	protected event RowInfoChangeEventHandler RowInfoChange;

	public MTReturnToSupplierWindow()
	{
	}

	public MTReturnToSupplierWindow(MTReturnToSupplier doc)
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
		DocumentsJournal idv = new DocumentsJournal(this, ReportTypesSE.AllDocument, ((TradeDocDefinition)Definition).BaseDocTypes)
		{
			DocType = ((TradeDocDefinition)Definition).BaseDocTypes.First().ToString().PadLeft(2, '0'),
			DocState = ArmSoft.SME.Trade.Common.SmartEnums.DocumentStateSE.DraftOrProcessed.ValueUI,
			DocOwner = Settings.LoginService.UserName
		};
		_baseDocumentControl = AddDropDownCommentControl(null, ArmSoft.SME.Common.Resources.BaseDoc, isRequired: false, idv, 50, HorizontalAlignment.Stretch, "BaseDocumentControl");
		_baseDocumentControl.ShowClearButton = true;
		_baseDocumentControl.ShowSearchButton = true;
		_baseDocumentControl.IsTextEditable = false;
		_baseDocumentControl.ClickDropDown += new SafeDocumentEventHandler<DropDownControlEventArgs>(BaseDocument_ClickDropDown, this).Handler;
		_baseDocumentControl.EditKeyValueChanged += new SafeDocumentEventHandler<EditKeyValueChangedEventArgs>(BaseDocument_EditKeyValueChanged, this).Handler;
		_baseDocumentControl.BtnClearClicked += new SafeDocumentEventHandler<EditKeyValueChangedEventArgs>(BaseDocumentDelete_Click, this).Handler;
		_baseDocumentControl.BtnSearchClicked += new SafeDocumentEventHandler<EditKeyValueChangedEventArgs>(BaseDocumentSearch_Click, this).Handler;
		LayoutGroupExt container2 = AddLayoutGroup(null, LayoutGroupView.Group, Orientation.Horizontal);
		_barCode = AddStringControl(container2, ArmSoft.SME.Common.Resources.BarCode + " (F2)", isRequired: false, 20, HorizontalAlignment.Stretch);
		_barCode.KeyDown += new SafeDocumentEventHandler<KeyEventArgs>(BarCode_KeyDown, this).Handler;
		_barCode.NullText = ArmSoft.SME.Trade.Common.Resources.BarCodeAndCodeInputNullText;
		string text = ArmSoft.SME.Trade.Common.Resources.cmdCodeEnter + " (F8)";
		_codeEnter = AddButton(container2, null, text, null, text, isTabStop: true);
		_codeEnter.Margin = new Thickness(0.0, 0.0, 18.0, 0.0);
		_codeEnter.Click += new SafeDocumentEventHandler<RoutedEventArgs>(CodeEnter_Click, this).Handler;
		_tabs = AddTabControl(null);
		LayoutGroup container3 = AddTabbedGroup(_tabs, ArmSoft.SME.Common.Resources.General, "General");
		_supplierContractContainer = AddPartnerContractGroupControls(container3, (PartnerContractGroupDefinition)Definition.Fields["PartnerContractGroup"], _date, _currCode, _totalSumma);
		_supplierContractContainer.PartnerCodeEditValueChanged += new SafeDocumentEventHandler<EditValueChangedEventArgs>(SupplierCodeEditValueChanged, this).Handler;
		_supplierContractContainer.PartnerCodeEditAdditionalValueChanged += new SafeDocumentEventHandler<EventArgs>(SupplierCode_EditAdditionalValueChanged, this).Handler;
		_supplierContractContainer.ContractCodeEditValueChanged += new SafeDocumentEventHandler<EventArgs>(SupplierContractCodeEditValueChanged, this).Handler;
		LayoutGroupExt container4 = AddLayoutGroup(container3, LayoutGroupView.Group, Orientation.Horizontal);
		_supBook = AddStringControl(container4, Definition.Fields["BookNum"]);
		_supBookPage = AddNumericControl(container4, Definition.Fields["BookPage"]);
		_supBookRow = AddNumericControl(container4, Definition.Fields["BookLine"]);
		_salesConsultant = AddDropDownControl(container3, Definition.Fields["SalesConsultant"], new Employees(), allowAdditionButtons: true);
		LayoutGroupExt container5 = AddLayoutGroup(container3, LayoutGroupView.Group, Orientation.Horizontal);
		_baseDocTaxInvoiceSeries = AddStringControl(container5, Definition.Fields["BaseDocInvoiceSeries"]);
		_baseDocTaxInvoiceNumber = AddStringControl(container5, Definition.Fields["BaseDocInvoiceNumber"]);
		_baseDocTaxInvoiceDate = AddDateControl(container3, Definition.Fields["BaseDocInvoiceDate"]);
		LayoutGroupExt container6 = AddLayoutGroup(container3, LayoutGroupView.Group, Orientation.Horizontal);
		_taxInvoiceSeries = AddStringControl(container6, Definition.Fields["TaxInvoiceSeries"]);
		_taxInvoiceNumber = AddStringControl(container6, Definition.Fields["TaxInvoiceNumber"]);
		_comment = AddComboBoxControl(container3, Definition.Fields["Comment"], TradeSettings.TradeCacheService.StandardComments.GetCaptionsList(), HorizontalAlignment.Stretch);
		_owner = AddDropDownControl(container3, Definition.Fields["Owner"], new UsersFilter());
		_owner.CodeWidth = 12;
		_owner.ShowComment = false;
		_owner.InnerCodeEdit.Width = _owner.InnerCodeEdit.MinWidth;
		if (!Settings.LoginService.IsAdmin && Settings.ParametersService.DenyAccessToOtherOwnerDocs())
		{
			((LayoutItemWithRequired)(object)_owner.Parent).Visibility = Visibility.Collapsed;
		}
		LayoutGroup container7 = AddTabbedGroup(_tabs, ArmSoft.SME.Trade.Common.Resources.MTLIST, "MtList");
		_priceListTabControlIndex = _tabs.Items.Count - 1;
		LayoutGroupExt container8 = AddLayoutGroup(container7, LayoutGroupView.Group, Orientation.Horizontal);
		_currCode = AddDropDownControl(container8, Definition.Fields["CurrCode"], new Currency(shortMode: true), allowAdditionButtons: false, fieldWithCaption: true, HorizontalAlignment.Stretch);
		_currCode.EditValueChanged += new SafeDocumentEventHandler<EditValueChangedEventArgs>(CurrCode_EditValueChanged, this).Handler;
		_discountPercent = AddNumericControl(container8, Definition.Fields["Discount"], useCurrencyFormat: true, HorizontalAlignment.Right);
		_discountPercent.EditValueChanged += new SafeDocumentEventHandler<EditValueChangedEventArgs>(DiscountPercent_EditValueChanged, this).Handler;
		_currExChngRate = AddExchangeRateControl(container7, Definition.Fields["ExchangeRate"]);
		_vatType = AddDropDownControl(container7, Definition.Fields["VATType"], new VATTypes(), allowAdditionButtons: false, fieldWithCaption: true, HorizontalAlignment.Stretch);
		_vatType.EditValueChanged += new SafeDocumentEventHandler<EditValueChangedEventArgs>(VATType_EditValueChanged, this).Handler;
		_priceListType = AddDropDownControl(container7, Definition.Fields["PriceListType"], new PriceTypes(), allowAdditionButtons: false, fieldWithCaption: true, HorizontalAlignment.Stretch);
		_priceListType.EditValueChanged += new SafeDocumentEventHandler<EditValueChangedEventArgs>(PriceListType_EditValueChanged, this).Handler;
		_storage = AddDropDownControl(container7, Definition.Fields["Storage"], new Storages(), allowAdditionButtons: false, fieldWithCaption: true, HorizontalAlignment.Stretch);
		_storage.EditValueChanged += new SafeDocumentEventHandler<EditValueChangedEventArgs>(Storage_EditValueChanged, this).Handler;
		_mtRetToSupGrid = AddGrid(container7, Definition.Grids["MTList"], HorizontalAlignment.Stretch, VerticalAlignment.Stretch, isSupportDialogMode: true);
		_mtRetToSupGrid.Name = "MTRetToSupGrid";
		_viewAvailAndTurn = _mtRetToSupGrid.AddButton(null, ArmSoft.SME.Trade.Common.Resources.AvailAndTurn, ArmSoft.SME.Trade.Common.Resources.AvailabilityAndTurn, 130.0);
		_viewAvailAndTurn.Click += new SafeDocumentEventHandler<RoutedEventArgs>(ViewAvailAndTurn_Click, this).Handler;
		_filteredSearch = _mtRetToSupGrid.AddButton(null, ArmSoft.SME.Trade.Common.Resources.GroupAddition, ArmSoft.SME.Trade.Common.Resources.GroupAddition, 130.0);
		_filteredSearch.Click += new SafeDocumentEventHandler<RoutedEventArgs>(FilteredSearch_Click, this).Handler;
		if (Settings.ParametersService.ShowGoodsCostInReportsAndDocs())
		{
			_fillLatestPurchasePrices = _mtRetToSupGrid.AddButton(null, ArmSoft.SME.Trade.Common.Resources.FillLatestPurchasePrices, ArmSoft.SME.Trade.Common.Resources.FillLatestPurchasePrices, 265.0);
			_fillLatestPurchasePrices.Click += new SafeDocumentEventHandler<RoutedEventArgs>(FillFromLastPurchase_Click, this).Handler;
		}
		_mtRetToSupGrid.IsRequired = true;
		_mtRetToSupGrid.AllowMoveUp = true;
		_mtRetToSupGrid.AllowMoveDown = true;
		_mtRetToSupGrid.ToolTip = ArmSoft.SME.Trade.Common.Resources.MTLIST;
		_mtRetToSupTableView = _mtRetToSupGrid.View;
		_mtRetToSupTableView.CellValueChanged += new SafeDocumentEventHandler<CellValueChangedEventArgs>(MTRetToSupTableView_CellValueChanged, this).Handler;
		_focusedRowHandleChangedEvent = new SafeDocumentEventHandler<FocusedRowHandleChangedEventArgs>(MTRetToSupTableView_FocusedRowHandleChanged, this);
		_mtRetToSupTableView.FocusedRowHandleChanged += _focusedRowHandleChangedEvent.Handler;
		_mtRetToSupGrid.AddRowIndicator(35);
		_storageCol = _mtRetToSupGrid.AddDropDownColumn(Definition.Grids["MTList"].Columns["Storage"], new Storages());
		_mtCodeCol = _mtRetToSupGrid.AddDropDownColumn(Definition.Grids["MTList"].Columns["MTCode"], new ArmSoft.SME.Trade.UI.DropDown.GoodsFilter(), allowAdditionButtons: true, isOwnProcessingClickDropDown: true);
		_mtCodeCol.Width = 125.0;
		DocumentGridColumn documentGridColumn = _mtRetToSupGrid.AddStringColumn(Definition.Grids["MTList"].Columns["MTName"]);
		documentGridColumn.AllowEditing = DefaultBoolean.False;
		documentGridColumn.Width = 225.0;
		_unitCodeCol = _mtRetToSupGrid.AddDropDownColumn(Definition.Grids["MTList"].Columns["UnitCode"]);
		_unitCol = _mtRetToSupGrid.AddStringColumn(Definition.Grids["MTList"].Columns["UnitBrief"]);
		_unitCol.AllowEditing = DefaultBoolean.False;
		_quantityCol = _mtRetToSupGrid.AddNumberColumn(Definition.Grids["MTList"].Columns["Quantity"], showTotal: true);
		_priceCol = _mtRetToSupGrid.AddNumberColumn(Definition.Grids["MTList"].Columns["Price"]);
		_discountCol = _mtRetToSupGrid.AddNumberColumn(Definition.Grids["MTList"].Columns["Discount"]);
		_discountPriceCol = _mtRetToSupGrid.AddNumberColumn(Definition.Grids["MTList"].Columns["DiscountPrice"]);
		_summaCol = _mtRetToSupGrid.AddNumberColumn(Definition.Grids["MTList"].Columns["Summa"], showTotal: true);
		_salePriceCol = _mtRetToSupGrid.AddNumberColumn(Definition.Grids["MTList"].Columns["SalePrice"], showTotal: false, useCurrencyFormat: true, isOwnProcessingClickDropDown: true);
		_salePriceCol.AllowEditing = DefaultBoolean.False;
		_saleSummaCol = _mtRetToSupGrid.AddNumberColumn(Definition.Grids["MTList"].Columns["SaleSumma"], showTotal: true);
		_saleSummaCol.AllowEditing = DefaultBoolean.False;
		_vatCol = _mtRetToSupGrid.AddBooleanColumn(Definition.Grids["MTList"].Columns["VAT"]);
		_vatCol.Width = 45.0;
		_partyCol = _mtRetToSupGrid.AddDropDownColumn("PartyMethodName", ArmSoft.SME.Common.Resources.Party, 19, trimValue: false, GlyphKind.Regular);
		((ButtonEditSettings)_partyCol.EditSettings).IsTextEditable = false;
		_mtRetToSupList = new ObservableCollection<MTRetToSupSpecificationRow>();
		_mtRetToSupGrid.AddingRow += new SafeDocumentEventHandler<DocumentGridAddingRowEventArgs>(MTRetToSupGrid_AddingRow, this).Handler;
		_mtRetToSupGrid.AddingRowWithDialog += new SafeDocumentEventHandler<DocumentGridAddingRowEventArgs>(MTRetToSupGrid_AddingRowWithDialog, this).Handler;
		_mtRetToSupGrid.DuplicatingRowWithDialog += new SafeDocumentEventHandler<DocumentGridDuplicatingRowEventArgs>(MTRetToSupGrid_DuplicatingRowWithDialog, this).Handler;
		_mtRetToSupGrid.EditingRowWithDialog += new SafeDocumentEventHandler<DocumentGridEditingRowEventArgs>(MTRetToSupGrid_EditingRowWithDialog, this).Handler;
		_mtRetToSupGrid.ClickDropDown += new SafeDocumentEventHandler<DocumentGridClickDropDownEventArgs>(MTRetToSupGrid_ClickDropDown, this).Handler;
		_mtRetToSupGrid.DeletedRow += new SafeDocumentEventHandler<EventArgs>(MTRetToSupGrid_DeletedRow, this).Handler;
		_mtRetToSupGrid.DeletingRow += new SafeDocumentEventHandler<DocumentGridDeletingRowEventArgs>(MTRetToSupGrid_DeletingRow, this).Handler;
		_mtRetToSupGrid.DuplicatingRow += new SafeDocumentEventHandler<DocumentGridDuplicatingRowEventArgs>(MTRetToSupGrid_DuplicatingRow, this).Handler;
		_mtRetToSupGrid.MovingRowUp += new SafeDocumentEventHandler<DocumentGridMovingRowUpOrDownEventArgs>(MTRetToSupGrid_MovingRowUp, this).Handler;
		_mtRetToSupGrid.MovingRowDown += new SafeDocumentEventHandler<DocumentGridMovingRowUpOrDownEventArgs>(MTRetToSupGrid_MovingRowDown, this).Handler;
		_mtRetToSupGrid.RefreshButtons += new SafeDocumentEventHandler<EventArgs>(MTRetToSupGrid_RefreshButtons, this).Handler;
		foreach (GridColumn column in _mtRetToSupGrid.Grid.Columns)
		{
			column.AllowSorting = DefaultBoolean.True;
			column.AllowColumnFiltering = DefaultBoolean.True;
			column.FilterPopupMode = FilterPopupMode.CheckedList;
		}
		_vatSumma = AddNumericControl(container7, Definition.Fields["SummVAT"], useCurrencyFormat: true, HorizontalAlignment.Right);
		_totalSumma = AddNumericControl(container7, Definition.Fields["Summ"], useCurrencyFormat: true, HorizontalAlignment.Right);
		_supplierContractContainer.DocSummaControl = _totalSumma;
		_supplierContractContainer.DocCurrCodeControl = _currCode;
		_infoDisplayControls = new InfoDisplayControls(this, ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTReturnToSupplier, isShowItemPictureOnInfoPanel: false, isShowStorageAvailabilityOnInfoPanel: true, isShowWholeAvailabilityOnInfoPanel: true);
		_infoDisplayControls.ConstructUI(container7);
		_infoDisplay = _infoDisplayControls.InfoDisplay;
		_infoItemName = _infoDisplayControls.InfoItemName;
		_infoDisplay.Collapsed += new SafeDocumentEventHandler<EventArgs>(InfoDisplay_Collapsed, this).Handler;
		LayoutGroup container9 = AddTabbedGroup(_tabs, ArmSoft.SME.Common.Resources.Supplier, "Supplier");
		_buyerName = AddStringControl(container9, Definition.Fields["BuyerName"], HorizontalAlignment.Stretch);
		_buyerLegalAddress = AddStringControl(container9, Definition.Fields["BuyerLegalAddress"], HorizontalAlignment.Stretch);
		_buyerBusinessAddress = AddComboBoxControl(container9, Definition.Fields["BuyerBusinessAddress"], null, HorizontalAlignment.Stretch);
		_buyerSettlAcc = AddDropDownControl(container9, Definition.Fields["BuyerSettlAcc"], new PartnerAccountInBank(), allowAdditionButtons: false, fieldWithCaption: true, HorizontalAlignment.Stretch);
		_buyerSettlAcc.CheckValue = false;
		_buyerTaxCode = AddStringControl(container9, Definition.Fields["BuyerTaxCode"], HorizontalAlignment.Stretch);
		IEnumerable<string> itemsSource = SmartEnum<ManagerPositionsSE, string>.List.Select((ManagerPositionsSE row) => row.Caption);
		_buyerMngPost = AddComboBoxControl(container9, Definition.Fields["BuyerMngPost"], itemsSource, HorizontalAlignment.Stretch);
		_buyerMngName = AddStringControl(container9, Definition.Fields["BuyerMngName"], HorizontalAlignment.Stretch);
		IEnumerable<string> itemsSource2 = SmartEnum<ChiefAccPositionsSE, string>.List.Select((ChiefAccPositionsSE row) => row.Caption);
		_buyerChiefAccPost = AddComboBoxControl(container9, Definition.Fields["BuyerChiefAccPost"], itemsSource2, HorizontalAlignment.Stretch);
		_buyerChiefAccName = AddStringControl(container9, Definition.Fields["BuyerChiefAccName"], HorizontalAlignment.Stretch);
		_buyerStateRegNum = AddStringControl(container9, Definition.Fields["BuyerStateRegNum"], HorizontalAlignment.Stretch);
		_buyerPassport = AddStringControl(container9, Definition.Fields["BuyerPassport"], HorizontalAlignment.Stretch);
		LayoutGroup container10 = AddTabbedGroup(_tabs, ArmSoft.SME.Common.Resources.Buyer, "Buyer");
		_supplierName = AddStringControl(container10, Definition.Fields["SupplierName"], HorizontalAlignment.Stretch);
		_supplierLegalAddress = AddStringControl(container10, Definition.Fields["SupplierLegalAddress"], HorizontalAlignment.Stretch);
		_supplierBusinessAddress = AddDropDownCommentControl(container10, Definition.Fields["SupplierBusinessAddress"], new Storages(), HorizontalAlignment.Stretch);
		_supplierBusinessAddress.MaxLength = 100;
		_supplierSettlAcc = AddDropDownControl(container10, Definition.Fields["SupplierSettlAcc"], new AccountInBanks(isKeySettlAccount: true), allowAdditionButtons: false, fieldWithCaption: true, HorizontalAlignment.Stretch);
		_supplierSettlAcc.CheckValue = false;
		_supplierTaxCode = AddStringControl(container10, Definition.Fields["SupplierTaxCode"], HorizontalAlignment.Stretch);
		_supplierMngPost = AddComboBoxControl(container10, Definition.Fields["SupplierMngPost"], itemsSource, HorizontalAlignment.Stretch);
		_supplierMngName = AddStringControl(container10, Definition.Fields["SupplierMngName"], HorizontalAlignment.Stretch);
		_supplierChiefAccPost = AddComboBoxControl(container10, Definition.Fields["SupplierChiefAccPost"], itemsSource2, HorizontalAlignment.Stretch);
		_supplierChiefAccName = AddStringControl(container10, Definition.Fields["SupplierChiefAccName"], HorizontalAlignment.Stretch);
		_infoDisplay.IsCollapsed = _infoDisplayControls.InfoPanelCollapsed;
		RowInfoChange += ChangeRowInfo;
	}

	protected override void OnNew()
	{
		base.OnNew();
		if (BaseDocument == null || BaseDocument.POType != (short)ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.ReceivedTaxInvoice)
		{
			_date.EditValue = Settings.HostingEnvironment.CurrentDate;
		}
		else
		{
			ReceivedTaxInvoice receivedTaxInvoice = (ReceivedTaxInvoice)BaseDocument;
			if (receivedTaxInvoice.IsCorrector && receivedTaxInvoice.Type == Convert.ToInt16(TaxAccAndInvoiceDocsTypeSE.AccDocGoods))
			{
				_vatType.Code = "3";
			}
		}
		_owner.Code = Settings.LoginService.UserName;
		if (_viewMode == WindowViewMode.Copy)
		{
			return;
		}
		string text = Settings.ParametersService.DefaultStorage();
		StoragesCacheRow storagesCacheRow = null;
		if (!string.IsNullOrWhiteSpace(text))
		{
			storagesCacheRow = TradeSettings.TradeCacheService.Storage.GetItem(text);
			if (storagesCacheRow != null && !storagesCacheRow.Retail && TradeSettings.CreateTradeClient().CommonServices.IsStorageAvailable(text))
			{
				_storage.Code = text;
			}
		}
		if (_baseISN == Guid.Empty)
		{
			string text2 = Settings.ParametersService.DefaultSupplier();
			if (!string.IsNullOrWhiteSpace(text2))
			{
				PartnerCacheRow item = TradeSettings.TradeCacheService.Partner.GetItem(text2);
				if (item != null && item.Supplier)
				{
					_supplierContractContainer.PartnerCode.Code = text2;
				}
			}
			_salesConsultant.Code = Settings.ParametersService.DefaultSalesConsultant();
			if (string.IsNullOrWhiteSpace(_priceListType.Code))
			{
				_priceListType.Code = Settings.ParametersService.DefaultBuyingPrice();
				string priceListRowVATType = _vatType.Code;
				ArmSoft.SME.Trade.UI.Helpers.PriceHelper.GetPriceListRowVATType(_priceListType.Code, out priceListRowVATType, out var _);
			}
			if (!string.IsNullOrWhiteSpace(Settings.ParametersService.VATType()))
			{
				_vatType.Code = Settings.ParametersService.VATType();
			}
		}
		_supplierName.Text = Settings.ParametersService.CompanyName();
		_supplierBusinessAddress.Text = Settings.ParametersService.CompanyBusinessAddress();
		_supplierLegalAddress.Text = Settings.ParametersService.CompanyAddress();
		_supplierSettlAcc.Code = TradeSettings.TradeCacheService.AccountInBank.GetDefaultSettlementAccount(isSettlAccount: true);
		_supplierMngPost.EditValue = Settings.ParametersService.ManagerPost();
		_supplierMngName.EditValue = Settings.ParametersService.ManagerName();
		_supplierChiefAccPost.EditValue = Settings.ParametersService.ChiefAccountantPost();
		_supplierChiefAccName.EditValue = Settings.ParametersService.ChiefAccountantName();
		_supplierTaxCode.Text = Settings.ParametersService.TaxCode();
		if (storagesCacheRow != null && !string.IsNullOrWhiteSpace(storagesCacheRow.Address))
		{
			_supplierBusinessAddress.Text = storagesCacheRow.Address;
		}
		CurrCodeValueChanged();
	}

	protected override void OnNewAfterSave()
	{
		base.OnNewAfterSave();
		_mtRetToSupList.Clear();
		_maxRowID = 0;
		CalcSummAndSummVAT();
		_owner.Code = Settings.LoginService.UserName;
	}

	protected override void OnEdit()
	{
		base.OnEdit();
		_setFocusOnActivation = _docNum;
	}

	private void CurrCodeValueChanged()
	{
		if (!string.IsNullOrWhiteSpace(_currCode.Code) && _currCode.Code != "AMD" && _date.DoValidate() && _currCode.DoValidate())
		{
			try
			{
				ExchangeRate exchangeRates = TradeSettings.CreateTradeClient().CommonServices.GetExchangeRates(_currCode.Code, _date.EditValue.Value);
				_currExChngRate.Rate = exchangeRates.Rate;
				_currExChngRate.Base = exchangeRates.Base;
			}
			catch (Exception ex)
			{
				Settings.ErrorHandler(ex);
			}
		}
	}

	protected override void SetControlsReadonly()
	{
		base.SetControlsReadonly();
		if (_viewMode == WindowViewMode.View)
		{
			return;
		}
		_vatSumma.IsReadOnly = true;
		_totalSumma.IsReadOnly = true;
		bool flag = State == ArmSoft.SME.Trade.Common.SmartEnums.DocumentStateSE.Draft.Value || State == ArmSoft.SME.Trade.Common.SmartEnums.DocumentStateSE.Deleted.Value;
		bool flag2 = _viewMode == WindowViewMode.Edit && !flag;
		_date.IsReadOnly = flag2;
		_storage.IsReadOnly = flag2;
		_currCode.IsReadOnly = flag2;
		_currExChngRate.IsReadOnly = flag2 || _currCode.Code == "AMD";
		_baseDocumentControl.IsReadOnly = flag2;
		_barCode.IsReadOnly = flag2;
		_codeEnter.IsEnabled = !flag2;
		_vatType.IsReadOnly = flag2;
		_priceListType.IsReadOnly = flag2;
		_discountPercent.IsReadOnly = flag2;
		_baseDocTaxInvoiceDate.IsReadOnly = flag2;
		_baseDocTaxInvoiceSeries.IsReadOnly = flag2;
		_baseDocTaxInvoiceNumber.IsReadOnly = flag2;
		_taxInvoiceSeries.IsReadOnly = flag2;
		_taxInvoiceNumber.IsReadOnly = flag2;
		foreach (GridColumn column in _mtRetToSupGrid.Grid.Columns)
		{
			column.ReadOnly = flag2;
		}
		_mtRetToSupGrid.AllowAdd = !flag2;
		_mtRetToSupGrid.AllowEdit = !flag2;
		_mtRetToSupGrid.AllowDelete = !flag2;
		_mtRetToSupGrid.AllowDuplicate = !flag2;
		_mtRetToSupGrid.AllowMoveUp = !flag2;
		_mtRetToSupGrid.AllowMoveDown = !flag2;
		GridColumn gridColumn = _mtRetToSupGrid.Grid.Columns["PartyMethodName"];
		gridColumn.ReadOnly = false;
		((ButtonEditSettings)gridColumn.EditSettings).IsTextEditable = false;
	}

	private void Storage_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		bool flag = true;
		if (string.IsNullOrWhiteSpace(e.NewValue.ToString()))
		{
			return;
		}
		if (!PO.StoredInDb)
		{
			StoragesCacheRow item = TradeSettings.TradeCacheService.Storage.GetItem(e.NewValue.ToString());
			if (item != null)
			{
				if (!TradeSettings.CreateTradeClient().CommonServices.IsStorageAvailable(e.NewValue.ToString()))
				{
					e.Handled = true;
					_storage.Code = e.OldValue.ToString();
					ArmSoft.SME.Core.UI.MessageBox.Show(string.Format(ArmSoft.SME.Trade.Common.Resources.StorageNotAvailForUser, Settings.LoginService.UserName, e.NewValue.ToString()), Settings.ProgramName, MessageBoxButton.OK, MessageBoxImage.Exclamation);
				}
				else if (!string.IsNullOrWhiteSpace(item.Address))
				{
					_supplierBusinessAddress.Text = item.Address;
				}
			}
		}
		if (_mtRetToSupList.Any((MTRetToSupSpecificationRow p) => !string.IsNullOrWhiteSpace(p.Storage) && p.Storage != _storage.Code) && ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Trade.Common.Resources.ChangeStorageValues, Settings.ProgramName, MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No) == MessageBoxResult.No)
		{
			flag = false;
		}
		if (flag)
		{
			foreach (MTRetToSupSpecificationRow mtRetToSup in _mtRetToSupList)
			{
				mtRetToSup.Storage = _storage.Code;
			}
		}
		_mtRetToSupGrid.Grid.RefreshData();
		RowInfoChange?.Invoke((MTRetToSupSpecificationRow)_mtRetToSupTableView.Grid.CurrentItem);
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
			BaseDocument = Settings.DocumentService.Load(_baseISN);
		}
		_mtRetToSupList.Clear();
		_focusedRowHandleChangedEvent.HandleOnLoadingDocumentToControls = true;
		foreach (MTReturnToSupplier.MTRetToSupSpecificationRow mT in PO.MTList)
		{
			MTRetToSupSpecificationRow item = new MTRetToSupSpecificationRow
			{
				Storage = mT.Storage,
				MTCode = mT.MTCode,
				MTID = mT.MTID,
				UnitCode = mT.UnitCode,
				UnitBrief = mT.UnitBrief,
				Quantity = mT.Quantity,
				Discount = mT.Discount,
				DiscountPrice = mT.DiscountPrice,
				Price = mT.Price,
				Summa = mT.Summa,
				SalePrice = mT.SalePrice,
				SaleSumma = mT.SaleSumma,
				VatCodeForConvert = mT.VATCodeForConvert,
				VAT = mT.VAT,
				RowNum = mT.RowNum,
				RowID = mT.RowID,
				IsPartySelected = mT.IsPartySelected,
				MTName = mT.MTName
			};
			_mtRetToSupList.Add(item);
			_useDiscount += Math.Abs(mT.Discount);
		}
		_maxRowID = PO.MaxRowID;
		if (!(_baseISN == Guid.Empty) && !StoredInDb && _viewMode != WindowViewMode.Copy)
		{
			CreateBasedOnDoc();
		}
		if (!_isParamAllowDiscountInReturn && _useDiscount == 0m)
		{
			((LayoutItemWithRequired)(object)_discountPercent.Parent).Visibility = Visibility.Collapsed;
			_discountCol.Visible = false;
			_discountCol.ShowInColumnChooser = false;
			_discountPriceCol.Visible = false;
			_discountPriceCol.ShowInColumnChooser = false;
		}
		VATType_EditValueChangedForLoad();
		_allowPurchaseWith0PriceOr100Percent = Settings.ParametersService.AllowPurchaseWith0PriceOr100Percent() || _mtRetToSupList.Any((MTRetToSupSpecificationRow row) => row.Summa == 0m);
		_totalSumma.IsRequired = !_allowPurchaseWith0PriceOr100Percent;
		_mtRetToSupGrid.ItemsSource = _mtRetToSupList;
		_supplierContractContainer.ValueChangedForLoadDoc();
		CalculatePartSumma();
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
			decimal qty = 1m;
			string unitCode = null;
			string unitBrief = null;
			string barCode = _barCode.Text.Trim();
			MaterialCacheRow materialCacheRow = ArmSoft.SME.Trade.UI.Helpers.DocumentHelpers.TryGetItemByBarCode(ref barCode, out qty, out unitCode, out unitBrief);
			if (materialCacheRow == null || materialCacheRow.Type == GoodsTypesSE.Service.Value)
			{
				ControlsHelper.SetFocus(_barCode);
				throw new ASException(string.Format(ArmSoft.SME.Common.Resources.ErrBarCodeNotExist, _barCode.Text.Trim()));
			}
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
			if (_tabs.SelectedIndex != _priceListTabControlIndex)
			{
				_tabs.SelectedContainer = (DXTabItem)_tabs.Items[_priceListTabControlIndex];
			}
			AddMTToList(materialCacheRow, qty, unitCode);
			_barCode.Text = "";
			CalcSummAndSummVAT();
			RowInfoChange?.Invoke(_mtRetToSupList[_mtRetToSupList.Count - 1]);
			_vatSumma.DoValidate();
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	private void AddMTToList(MaterialCacheRow goodsRow, decimal quantity, string unitCode = "")
	{
		if (_rowAddMode == "3")
		{
			AddMTToListNewRow(goodsRow, string.IsNullOrWhiteSpace(unitCode) ? goodsRow.Unit : unitCode, quantity);
		}
		else
		{
			AddNewRowOrMergeInner(goodsRow, _rowAddMode == "2", quantity, string.IsNullOrWhiteSpace(unitCode) ? goodsRow.Unit : unitCode);
		}
	}

	private void AddNewRowOrMergeInner(MaterialCacheRow goodsRow, bool forFocusedRow, decimal quantity, string unitCode)
	{
		MTRetToSupSpecificationRow mTRetToSupSpecificationRow = null;
		if (forFocusedRow && _mtRetToSupList.Count != 0)
		{
			mTRetToSupSpecificationRow = (MTRetToSupSpecificationRow)_mtRetToSupTableView.Grid.CurrentItem;
			if (mTRetToSupSpecificationRow == null)
			{
				return;
			}
		}
		for (int i = 0; i <= _mtRetToSupList.Count - 1; i++)
		{
			if ((!forFocusedRow || mTRetToSupSpecificationRow.RowID == _mtRetToSupList[i].RowID) && goodsRow.Id == _mtRetToSupList[i].MTID && unitCode == _mtRetToSupList[i].UnitCode && _storage.Code == _mtRetToSupList[i].Storage)
			{
				FillMTIncGridRow(_mtRetToSupList[i], goodsRow, unitCode, quantity);
				int rowHandleByListIndex = _mtRetToSupGrid.Grid.GetRowHandleByListIndex(i);
				_mtRetToSupGrid.Grid.RefreshRow(rowHandleByListIndex);
				POWindow<MTReturnToSupplier>.SelectCellInGrid(_mtRetToSupTableView, _mtRetToSupGrid, rowHandleByListIndex, "Quantity");
				return;
			}
		}
		AddMTToListNewRow(goodsRow, unitCode, quantity);
		RowInfoChange?.Invoke(mTRetToSupSpecificationRow);
	}

	private void AddMTToListNewRow(MaterialCacheRow goodsRow, string unitCode, decimal quantity = 1m)
	{
		MTRetToSupSpecificationRow mTRetToSupSpecificationRow = new MTRetToSupSpecificationRow();
		FillMTIncGridRow(mTRetToSupSpecificationRow, goodsRow, unitCode, quantity);
		mTRetToSupSpecificationRow.RowID = _maxRowID;
		_mtRetToSupList.Add(mTRetToSupSpecificationRow);
		_maxRowID++;
		int rowHandleByListIndex = _mtRetToSupGrid.Grid.GetRowHandleByListIndex(_mtRetToSupList.Count - 1);
		_mtRetToSupGrid.Grid.RefreshRow(rowHandleByListIndex);
		POWindow<MTReturnToSupplier>.SelectCellInGrid(_mtRetToSupTableView, _mtRetToSupGrid, rowHandleByListIndex, "Quantity");
		_mtRetToSupGrid.ConfigureButtons();
		RowInfoChange?.Invoke(mTRetToSupSpecificationRow);
	}

	private void FillMTIncGridRow(MTRetToSupSpecificationRow mtRetToSupGridRow, MaterialCacheRow goodsRow, string unitCode, decimal quantity = 1m)
	{
		mtRetToSupGridRow.Storage = _storage.Code;
		mtRetToSupGridRow.UnitCode = unitCode;
		QntUnitCacheRow item = TradeSettings.TradeCacheService.QntUnit.GetItem(mtRetToSupGridRow.UnitCode);
		mtRetToSupGridRow.UnitBrief = ((item == null) ? "" : item.Brief);
		if (goodsRow != null)
		{
			mtRetToSupGridRow.MTCode = goodsRow.MTCode;
			mtRetToSupGridRow.MTID = goodsRow.Id;
			mtRetToSupGridRow.MTName = goodsRow.Caption;
			if (_vatType.Code.InList("1", "2"))
			{
				mtRetToSupGridRow.VAT = goodsRow.VAT;
			}
		}
		mtRetToSupGridRow.Quantity += quantity;
		if (mtRetToSupGridRow.Price == 0m && _date.DoValidate())
		{
			mtRetToSupGridRow.Price = TradeSettings.CreateTradeClient().CommonServices.GetPriceForGivenPriceListAndVATType(_priceListType.Code, _vatType.Code, mtRetToSupGridRow.MTID, mtRetToSupGridRow.UnitCode, _date.EditValue.Value, _currCode.Code);
		}
		if (_isParamAllowDiscountInReturn)
		{
			mtRetToSupGridRow.Discount = _discountPercent.Value;
		}
		mtRetToSupGridRow.DiscountPrice = (mtRetToSupGridRow.Price - mtRetToSupGridRow.Price * mtRetToSupGridRow.Discount / 100m).RoundAs(2);
		mtRetToSupGridRow.Summa = (mtRetToSupGridRow.Quantity * mtRetToSupGridRow.DiscountPrice).RoundAs(2);
		mtRetToSupGridRow.SaleSumma = (mtRetToSupGridRow.Quantity * mtRetToSupGridRow.SalePrice).RoundAs(2);
		mtRetToSupGridRow.IsPartySelected = false;
	}

	private void SupplierCodeEditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		BuyerCodeEditValueChanged();
		CalculatePartSumma();
	}

	private void SupplierCode_EditAdditionalValueChanged(object sender, EventArgs e)
	{
		BuyerCodeEditValueChanged();
		CalculatePartSumma();
	}

	private void SupplierContractCodeEditValueChanged(object sender, EventArgs e)
	{
		if (TradeSettings.TradeCacheService.Partner.GetItem(_supplierContractContainer.PartnerCode.Code) != null)
		{
			ContractCodeEditValueChanged();
		}
		CalculatePartSumma();
	}

	private void BuyerCodeEditValueChanged()
	{
		if (!string.IsNullOrWhiteSpace(_supplierContractContainer.PartnerCode.Code) && _supplierContractContainer.PartnerCode.DoValidate())
		{
			PartnerCacheRow item = TradeSettings.TradeCacheService.Partner.GetItem(_supplierContractContainer.PartnerCode.Code, throwExceptionNotExits: true);
			_buyerName.Text = item.Caption;
			_buyerLegalAddress.Text = item.Address;
			_buyerBusinessAddress.Text = item.BusinessAddress;
			_buyerBusinessAddress.ItemsSource = item.BusinessAddress.Select((char row) => row);
			_buyerSettlAcc.Code = item.SettlAcc;
			_buyerSettlAcc.DataView = new PartnerAccountInBank(item.PartCode);
			_buyerTaxCode.Text = item.TaxCode;
			_buyerMngPost.EditValue = item.ManagerPost;
			_buyerMngName.Text = item.ManagerFIO;
			_buyerChiefAccPost.EditValue = item.ChiefaccPost;
			_buyerChiefAccName.Text = item.ChiefaccFIO;
			_buyerStateRegNum.Text = item.PartStateRegister;
			_buyerPassport.Text = item.Passport;
			ContractCodeEditValueChanged();
		}
		else
		{
			_buyerName.Text = "";
			_buyerLegalAddress.Text = "";
			_buyerBusinessAddress.Text = "";
			_buyerBusinessAddress.ItemsSource = null;
			_buyerSettlAcc.Code = "";
			_buyerTaxCode.Text = "";
			_buyerMngPost.EditValue = "";
			_buyerMngName.Text = "";
			_buyerChiefAccPost.EditValue = "";
			_buyerChiefAccName.Text = "";
			_buyerStateRegNum.Text = "";
			_buyerPassport.Text = "";
			_buyerSettlAcc.DataView = new PartnerAccountInBank();
		}
	}

	private void ContractCodeEditValueChanged()
	{
		if (_isAskQuestionConfirmSummChange && (_mtRetToSupList.Count == 0 || string.IsNullOrWhiteSpace(_priceListType.Code)) && !string.IsNullOrWhiteSpace(_supplierContractContainer.Contract.Code) && _supplierContractContainer.Contract.DoValidate())
		{
			PartnerCacheRow.ContractRow contract = TradeSettings.TradeCacheService.Partner.GetContract(_supplierContractContainer.PartnerCode.Code, _supplierContractContainer.Contract.Code);
			if (contract != null && !string.IsNullOrWhiteSpace(contract.PriceType))
			{
				_isAskQuestionConfirmSummChange = false;
				_priceListType.Code = contract.PriceType;
				if (_isParamAllowDiscountInReturn)
				{
					_discountPercent.EditValue = contract.Percent;
				}
				_isAskQuestionConfirmSummChange = true;
				if (_mtRetToSupList.Count == 0)
				{
					string priceListRowVATType = "";
					if (ArmSoft.SME.Trade.UI.Helpers.PriceHelper.GetPriceListRowVATType(contract.PriceType, out priceListRowVATType, out var _))
					{
						_vatType.Code = priceListRowVATType;
					}
					_currCode.Code = contract.CurrCode;
				}
			}
		}
		CalculatePartSumma();
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
		if (_baseISN != Guid.Empty)
		{
			Settings.DocumentService.ShowWindow(BaseISN, WindowViewMode.Edit);
		}
	}

	private void CreateBasedOnDoc()
	{
		_comment.EditValue = BaseDocument.Comment;
		_isAskQuestionConfirmSummChange = false;
		_mtRetToSupList.Clear();
		PO.MTExpDocSelectedPartiesList.Clear();
		_maxRowID = 0;
		if (BaseDocument.POType == (short)ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.ReceivedTaxInvoice)
		{
			ReceivedTaxInvoice receivedDoc = (ReceivedTaxInvoice)_baseDocument;
			string text = Settings.ParametersService.DefaultStorage();
			if (!string.IsNullOrWhiteSpace(text) && TradeSettings.CreateTradeClient().CommonServices.IsStorageAvailable(text))
			{
				_storage.Code = text;
			}
			_date.EditValue = receivedDoc.DocDate;
			PartnerCacheRow partnerCacheRow = TradeSettings.TradeCacheService.Partner.GetList((PartnerCacheRow p) => p.TaxCode == receivedDoc.SupplierTIN && p.Supplier).FirstOrDefault();
			if (partnerCacheRow != null)
			{
				_supplierContractContainer.PartnerCode.Code = partnerCacheRow.PartCode;
				PartnerCacheRow.ContractRow defaultContract = TradeSettings.TradeCacheService.Partner.GetDefaultContract(partnerCacheRow);
				if (defaultContract != null)
				{
					_supplierContractContainer.Contract.Code = defaultContract.Contract;
				}
			}
			_comment.Text = receivedDoc.AdditionalInfo;
			_baseDocTaxInvoiceSeries.Text = receivedDoc.BaseSeries;
			_baseDocTaxInvoiceNumber.Text = receivedDoc.BaseNumber;
			_baseDocTaxInvoiceDate.EditValue = receivedDoc.BaseDate;
			_taxInvoiceSeries.Text = receivedDoc.Series;
			_taxInvoiceNumber.Text = receivedDoc.TaxNumber;
			bool excise = receivedDoc.Type == Convert.ToInt16(TaxAccAndInvoiceDocsTypeSE.AccDocGoods);
			LoadMTListReceivedTaxInvoice(receivedDoc.Items, excise);
			_isAskQuestionConfirmSummChange = true;
			_mtRetToSupGrid.Grid.RefreshData();
			CalcSummAndSummVAT();
			return;
		}
		TradeDocument tradeDocument = (TradeDocument)BaseDocument;
		_currCode.Code = tradeDocument.CurrCode;
		_currExChngRate.Rate = tradeDocument.ExchangeRate.Rate;
		_currExChngRate.Base = tradeDocument.ExchangeRate.Base;
		_supplierContractContainer.PartnerCode.Code = tradeDocument.PartnerContractGroup.PartnerCode;
		_supplierContractContainer.Contract.Code = tradeDocument.PartnerContractGroup.ContractCode;
		BuyerCodeEditValueChanged();
		if (BaseDocument.POType == (short)ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTIncomeImport)
		{
			MTIncomeImport mTIncomeImport = (MTIncomeImport)BaseDocument;
			_priceListType.Code = mTIncomeImport.PriceType;
			if (!string.IsNullOrWhiteSpace(mTIncomeImport.InvoiceNumber))
			{
				_baseDocTaxInvoiceSeries.Text = mTIncomeImport.InvoiceNumber[0].ToString();
				_baseDocTaxInvoiceNumber.Text = mTIncomeImport.InvoiceNumber.Substring(1);
			}
			else
			{
				_baseDocTaxInvoiceSeries.Text = "";
				_baseDocTaxInvoiceNumber.Text = "";
			}
			_baseDocTaxInvoiceDate.EditValue = mTIncomeImport.InvoiceDate;
			_taxInvoiceNumber.Text = "";
			_taxInvoiceSeries.Text = "";
			LoadMTList(mTIncomeImport.MTList, mTIncomeImport.Storage);
		}
		else if (BaseDocument.POType == (short)ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTIncome)
		{
			MTIncomeBase mTIncomeBase = (MTIncomeBase)BaseDocument;
			_priceListType.Code = mTIncomeBase.PriceType;
			_vatType.Code = mTIncomeBase.VATType;
			_discountPercent.Value = mTIncomeBase.Discount;
			if (!string.IsNullOrWhiteSpace(mTIncomeBase.InvoiceNumber))
			{
				_baseDocTaxInvoiceSeries.Text = mTIncomeBase.InvoiceNumber[0].ToString();
				_baseDocTaxInvoiceNumber.Text = mTIncomeBase.InvoiceNumber.Substring(1);
			}
			else
			{
				_baseDocTaxInvoiceSeries.Text = "";
				_baseDocTaxInvoiceNumber.Text = "";
			}
			_baseDocTaxInvoiceDate.EditValue = mTIncomeBase.InvoiceDate;
			_taxInvoiceNumber.Text = "";
			_taxInvoiceSeries.Text = "";
			LoadMTList(mTIncomeBase.MTList, mTIncomeBase.Storage);
		}
		else if (BaseDocument.POType == (short)ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTIncomeRetail)
		{
			MTIncomeRetail mTIncomeRetail = (MTIncomeRetail)BaseDocument;
			_priceListType.Code = mTIncomeRetail.PriceType;
			_vatType.Code = mTIncomeRetail.VATType;
			_discountPercent.Value = mTIncomeRetail.Discount;
			if (!string.IsNullOrWhiteSpace(mTIncomeRetail.InvoiceNumber))
			{
				_baseDocTaxInvoiceSeries.Text = mTIncomeRetail.InvoiceNumber[0].ToString();
				_baseDocTaxInvoiceNumber.Text = mTIncomeRetail.InvoiceNumber.Substring(1);
			}
			else
			{
				_baseDocTaxInvoiceSeries.Text = "";
				_baseDocTaxInvoiceNumber.Text = "";
			}
			_baseDocTaxInvoiceDate.EditValue = mTIncomeRetail.InvoiceDate;
			_taxInvoiceNumber.Text = "";
			_taxInvoiceSeries.Text = "";
			LoadMTList(mTIncomeRetail.MTList, mTIncomeRetail.Storage);
		}
		_storage.Code = tradeDocument.Storage;
		_isAskQuestionConfirmSummChange = true;
		_mtRetToSupGrid.Grid.RefreshData();
		CalcSummAndSummVAT();
	}

	private void LoadMTList(List<MTIncomeBase.MTIncomeSpecificationRow> mtList, string storage)
	{
		foreach (MTIncomeBase.MTIncomeSpecificationRow mt in mtList)
		{
			_mtRetToSupList.Add(new MTRetToSupSpecificationRow
			{
				Storage = storage,
				MTID = mt.MTID,
				MTCode = mt.MTCode,
				MTName = mt.MTName,
				FullName = mt.FullName,
				UnitCode = mt.UnitCode,
				UnitBrief = mt.UnitBrief,
				Quantity = mt.Quantity,
				Price = mt.Price,
				Discount = mt.Discount,
				DiscountPrice = mt.DiscountPrice,
				Summa = mt.Summa,
				SalePrice = mt.SalePrice,
				SaleSumma = mt.SaleSumma,
				VAT = (mt.VAT.HasValue && mt.VAT.Value),
				RowID = _maxRowID,
				VatCodeForConvert = mt.VATCodeForConvert,
				IsPartySelected = true
			});
			MaterialCacheRow item = TradeSettings.TradeCacheService.Material.GetItem(mt.MTCode);
			if (item == null)
			{
				continue;
			}
			PartyRow partyRow = new PartyRow();
			if (item.ExpenseMethod == "3")
			{
				Guid partyBaseRowForAverage = TradeSettings.CreateTradeClient().CommonServices.GetPartyBaseRowForAverage(mt.MTID);
				if (partyBaseRowForAverage != Guid.Empty)
				{
					partyRow.Base = partyBaseRowForAverage;
					partyRow.Row = 1;
					partyRow.GridRow = _maxRowID;
					partyRow.IncomeDate = BaseDocument.DocDate;
					partyRow.GoodID = mt.MTID;
					partyRow.Qty = mt.Quantity;
					partyRow.ExpQty = mt.Quantity;
					partyRow.Summa = mt.Summa;
					partyRow.Cost = mt.DiscountPrice;
					partyRow.ExpSumma = partyRow.Summa;
					PO.MTExpDocSelectedPartiesList.Add(partyRow);
				}
			}
			else
			{
				partyRow.Base = BaseDocument.ISN;
				partyRow.Row = mt.RowID;
				partyRow.GridRow = _maxRowID;
				partyRow.IncomeDate = BaseDocument.DocDate;
				partyRow.GoodID = mt.MTID;
				partyRow.Qty = mt.Quantity;
				partyRow.ExpQty = mt.Quantity;
				partyRow.Summa = mt.Summa;
				partyRow.Cost = mt.DiscountPrice;
				partyRow.ExpSumma = partyRow.Summa;
				PO.MTExpDocSelectedPartiesList.Add(partyRow);
			}
			_maxRowID++;
		}
	}

	private void LoadMTList(List<MTIncomeImport.MTIncomeImportSpecificationRow> mtList, string Storage)
	{
		foreach (MTIncomeImport.MTIncomeImportSpecificationRow mt in mtList)
		{
			_mtRetToSupList.Add(new MTRetToSupSpecificationRow
			{
				Storage = Storage,
				MTID = mt.MTID,
				MTCode = mt.MTCode,
				MTName = mt.MTName,
				FullName = mt.FullName,
				UnitCode = mt.UnitCode,
				UnitBrief = mt.UnitBrief,
				Quantity = mt.Quantity,
				Price = mt.Price,
				Discount = 0m,
				DiscountPrice = mt.Price,
				Summa = mt.Summa,
				SalePrice = 0m,
				SaleSumma = 0m,
				VAT = mt.VAT,
				RowID = _maxRowID,
				IsPartySelected = true
			});
			PartyRow partyRow = new PartyRow();
			partyRow.Base = BaseDocument.ISN;
			partyRow.Row = mt.RowID;
			partyRow.GridRow = _maxRowID;
			partyRow.IncomeDate = BaseDocument.DocDate;
			partyRow.GoodID = mt.MTID;
			partyRow.Qty = mt.Quantity;
			partyRow.ExpQty = mt.Quantity;
			partyRow.Summa = mt.Summa;
			partyRow.Cost = mt.Price;
			partyRow.ExpSumma = partyRow.Summa;
			PO.MTExpDocSelectedPartiesList.Add(partyRow);
			_maxRowID++;
		}
	}

	private void LoadMTListReceivedTaxInvoice(List<TaxDocument.SpecificationRow> recInvList, bool excise)
	{
		foreach (TaxDocument.SpecificationRow recInv in recInvList)
		{
			if (string.IsNullOrWhiteSpace(recInv.ItemCode) || !(recInv.ItemType.Trim() == InvoiceRowTypeSE.Goods.Value))
			{
				continue;
			}
			MTRetToSupSpecificationRow mTRetToSupSpecificationRow = new MTRetToSupSpecificationRow();
			mTRetToSupSpecificationRow.RowID = _maxRowID;
			string text = Settings.ParametersService.DefaultStorage();
			if (!string.IsNullOrWhiteSpace(text) && TradeSettings.CreateTradeClient().CommonServices.IsStorageAvailable(text))
			{
				mTRetToSupSpecificationRow.Storage = text;
			}
			MaterialCacheRow item = TradeSettings.TradeCacheService.Material.GetItem(recInv.ItemCode);
			if (item != null)
			{
				mTRetToSupSpecificationRow.MTID = item.Id;
				mTRetToSupSpecificationRow.MTCode = item.MTCode;
				mTRetToSupSpecificationRow.MTName = item.Caption;
				mTRetToSupSpecificationRow.FullName = item.FullCaption;
				mTRetToSupSpecificationRow.UnitCode = recInv.UnitCodeFromDirectory;
				mTRetToSupSpecificationRow.UnitBrief = recInv.UnitBriefFromDirectory;
			}
			mTRetToSupSpecificationRow.Quantity = Math.Truncate(recInv.Quantity * 1000m) / 1000m;
			mTRetToSupSpecificationRow.Summa = recInv.Sum.RoundAs(2);
			if (_vatType.Code == VatTypesSE.CostWithInVAT.Value)
			{
				mTRetToSupSpecificationRow.Summa += recInv.SumVAT.RoundAs(2);
			}
			if (mTRetToSupSpecificationRow.Quantity != 0m)
			{
				mTRetToSupSpecificationRow.Price = (mTRetToSupSpecificationRow.Summa / mTRetToSupSpecificationRow.Quantity).RoundAs(2);
			}
			else
			{
				mTRetToSupSpecificationRow.Price = recInv.Price.RoundAs(2);
			}
			if (excise)
			{
				if (mTRetToSupSpecificationRow.Quantity == 0m)
				{
					mTRetToSupSpecificationRow.Price += recInv.Excise.RoundAs(2);
				}
				else
				{
					mTRetToSupSpecificationRow.Price += (recInv.Excise / mTRetToSupSpecificationRow.Quantity).RoundAs(2);
				}
				mTRetToSupSpecificationRow.Summa += recInv.Excise.RoundAs(2);
			}
			mTRetToSupSpecificationRow.Discount = 0m;
			mTRetToSupSpecificationRow.DiscountPrice = mTRetToSupSpecificationRow.Price;
			mTRetToSupSpecificationRow.SalePrice = 0m;
			mTRetToSupSpecificationRow.SaleSumma = 0m;
			mTRetToSupSpecificationRow.VAT = recInv.SumVAT > 0m;
			mTRetToSupSpecificationRow.IsPartySelected = false;
			_mtRetToSupList.Add(mTRetToSupSpecificationRow);
			_maxRowID++;
		}
	}

	protected override void AddToolbarItems()
	{
		base.AddToolbarItems();
		Ribbon.ToolbarItemLinks.Add(_barButtonJumpTo.ConvertForToolbar("t_doc_JumpTo_16.svg"));
		Ribbon.ToolbarItemLinks.Add(_barSubItemJournals.ConvertForToolbar("t_journal_16.svg"));
	}

	protected override void AddItemsToRibbon()
	{
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		base.AddItemsToRibbon();
		CreateRibbonPageGroupActions();
		CreateRibbonPageGroupReports();
		_barButtonJumpTo = new ReportBarButtonItem(ArmSoft.SME.Trade.Common.SmartEnums.DataProviderTypeSE.AllDocumentsJournal, showDialog: false, ArmSoft.SME.Common.Resources.JumpTo, null, "doc_JumpTo_16.svg", "doc_JumpTo_32.svg");
		_barButtonJumpTo.AddInBarSubItem(ref _ribbonPageGroupActions, ref _barSubItemActions);
		_barSubItemJournals.Content = "";
		_barSubItemJournals.Glyph = ImageService.GetImage("report_16.svg");
		_barSubItemJournals.LargeGlyph = ImageService.GetImage("report_32.svg");
		_barButtonGoodsOperationsJournal = new ReportBarButtonItem(ArmSoft.SME.Trade.Common.SmartEnums.DataProviderTypeSE.GoodsRepOperations.Value, showDialog: false, ArmSoft.SME.Trade.Common.Resources.MTOperations, new List<PropertyWithValue>
		{
			new PropertyWithValue("DocISN", PO.ISN.ToString()),
			new PropertyWithValue("AdditionalCaption", GetReportsAdditionalCaption())
		});
		_barButtonGoodsOperationsJournal.AddInBarSubItem(ref _ribbonPageGroupReports, ref _barSubItemJournals);
		_barButtonGoodsRetailOperationJournal = new ReportBarButtonItem(ArmSoft.SME.Trade.Common.SmartEnums.DataProviderTypeSE.MatRepOperationsRetail.Value, showDialog: false, ArmSoft.SME.Trade.Common.Resources.MTOperationsRetail, new List<PropertyWithValue>
		{
			new PropertyWithValue("DocISN", PO.ISN.ToString()),
			new PropertyWithValue("AdditionalCaption", GetReportsAdditionalCaption())
		});
		_barButtonGoodsRetailOperationJournal.AddInBarSubItem(ref _ribbonPageGroupReports, ref _barSubItemJournals);
		_barButtonPartnerOperationJournal = new ReportBarButtonItem(ArmSoft.SME.Trade.Common.SmartEnums.DataProviderTypeSE.PartOpJournal.Value, showDialog: false, ArmSoft.SME.Trade.Common.Resources.PartnerOp, new List<PropertyWithValue>
		{
			new PropertyWithValue("DocISN", PO.ISN.ToString()),
			new PropertyWithValue("AdditionalCaption", GetReportsAdditionalCaption()),
			new PropertyWithValue("ShowLinks", true.ToString())
		});
		_barButtonPartnerOperationJournal.AddInBarSubItem(ref _ribbonPageGroupReports, ref _barSubItemJournals);
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
		ReportBarButtonItem barButtonPartnerOperationJournal = _barButtonPartnerOperationJournal;
		num = 3;
		List<PropertyWithValue> list4 = new List<PropertyWithValue>(num);
		CollectionsMarshal.SetCount(list4, num);
		Span<PropertyWithValue> span4 = CollectionsMarshal.AsSpan(list4);
		span4[0] = new PropertyWithValue("DocISN", PO.ISN.ToString());
		span4[1] = new PropertyWithValue("AdditionalCaption", GetReportsAdditionalCaption());
		span4[2] = new PropertyWithValue("ShowLinks", true.ToString());
		barButtonPartnerOperationJournal.PropertyWithValues = list4;
	}

	protected override void SetReportBarButtonsEnable(bool isEnabled)
	{
		base.SetReportBarButtonsEnable(isEnabled);
		_barButtonJumpTo.IsEnabled = isEnabled;
		_barButtonGoodsOperationsJournal.IsEnabled = isEnabled;
		_barButtonGoodsRetailOperationJournal.IsEnabled = isEnabled;
		_barButtonPartnerOperationJournal.IsEnabled = isEnabled;
	}

	protected override void CreateChildDocument(short childDocType, MTReturnToSupplier baseDoc)
	{
		base.CreateChildDocument(childDocType, baseDoc);
		if (childDocType == 7 || childDocType == 20)
		{
			CreateBasedOnDocHelper.CreateDocument((ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE)(SmartEnum<ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE, short>)childDocType, PO.ISN, DVPID);
		}
	}

	protected override void FillDocument(StoreRequest sr)
	{
		base.FillDocument(sr);
		PO.BaseISN = BaseISN;
		if (_date.EditValue.HasValue)
		{
			PO.DocDate = _date.EditValue.Value;
		}
		else
		{
			PO.DocDate = default;
		}
		PO.MTList.Clear();
		foreach (MTRetToSupSpecificationRow mtRetToSup in _mtRetToSupList)
		{
			PO.MTList.Add(new MTReturnToSupplier.MTRetToSupSpecificationRow
			{
				Storage = mtRetToSup.Storage,
				MTCode = mtRetToSup.MTCode,
				MTID = mtRetToSup.MTID,
				UnitCode = mtRetToSup.UnitCode,
				UnitBrief = mtRetToSup.UnitBrief,
				Quantity = mtRetToSup.Quantity,
				Discount = mtRetToSup.Discount,
				DiscountPrice = mtRetToSup.DiscountPrice,
				Price = mtRetToSup.Price,
				Summa = mtRetToSup.Summa,
				SalePrice = mtRetToSup.SalePrice,
				SaleSumma = mtRetToSup.SaleSumma,
				VATCodeForConvert = mtRetToSup.VatCodeForConvert,
				VAT = mtRetToSup.VAT,
				RowNum = mtRetToSup.RowNum,
				RowID = mtRetToSup.RowID,
				IsPartySelected = mtRetToSup.IsPartySelected,
				MTName = mtRetToSup.MTName,
				FullName = mtRetToSup.FullName
			});
		}
		PO.MaxRowID = _maxRowID;
	}

	protected override void StoreSettings(RegistryKey rk)
	{
		base.StoreSettings(rk);
		rk.SetValue("InfoPanelCollapsed", _infoDisplayControls.InfoPanelCollapsed ? "1" : "0");
		_mtRetToSupGrid.StoreSettings(rk);
	}

	protected override void RestoreSettings(double DefaultWidth, double DefaultHeight, RegistryKey rk)
	{
		base.RestoreSettings(DefaultWidth, DefaultHeight, rk);
		if (rk != null)
		{
			_infoDisplayControls.InfoPanelCollapsed = rk.GetValue("InfoPanelCollapsed", false).ToString() == "1";
		}
		_mtRetToSupGrid.RestoreSettings(rk);
	}

	private void MTRetToSupGrid_AddingRow(object sender, DocumentGridAddingRowEventArgs e)
	{
		if (!_date.DoValidate())
		{
			ControlsHelper.SetFocus(_date);
			throw new ASException(ArmSoft.SME.Common.Resources.ValidateEmptyDate);
		}
		MTRetToSupSpecificationRow mTRetToSupSpecificationRow = new MTRetToSupSpecificationRow
		{
			IsPartySelected = false,
			Storage = _storage.Code,
			RowID = _maxRowID
		};
		if (_isParamAllowDiscountInReturn)
		{
			mTRetToSupSpecificationRow.Discount = _discountPercent.Value;
		}
		_mtRetToSupList.Add(mTRetToSupSpecificationRow);
		_maxRowID++;
		e.RowNumber = _mtRetToSupList.Count - 1;
		RowInfoChange?.Invoke(_mtRetToSupList[_mtRetToSupList.Count - 1]);
	}

	private void MTRetToSupGrid_AddingRowWithDialog(object sender, DocumentGridAddingRowEventArgs e)
	{
		if (!_date.DoValidate())
		{
			ControlsHelper.SetFocus(_date);
			throw new ASException(ArmSoft.SME.Common.Resources.ValidateEmptyDate);
		}
		GoodsSelectPartyWindow goodsSelectPartyWindow = new GoodsSelectPartyWindow(DialogWindowMode.NewModeWithNext, this);
		goodsSelectPartyWindow.ShowDialog();
		int addedRows = goodsSelectPartyWindow.AddedRows;
		if (addedRows != 0)
		{
			_maxRowID += addedRows;
			_mtRetToSupGrid.Grid.RefreshData();
			CalcSummAndSummVAT();
			RowInfoChangeEventHandler rowInfoChangeEventHandler = RowInfoChange;
			if (rowInfoChangeEventHandler != null)
			{
				ObservableCollection<MTRetToSupSpecificationRow> mtRetToSupList = _mtRetToSupList;
				rowInfoChangeEventHandler(mtRetToSupList[mtRetToSupList.Count - 1]);
			}
		}
	}

	private void MTRetToSupGrid_DuplicatingRowWithDialog(object sender, DocumentGridDuplicatingRowEventArgs e)
	{
		MTRetToSupSpecificationRow mTRetToSupSpecificationRow = (MTRetToSupSpecificationRow)_mtRetToSupGrid.Grid.GetRow(e.RowHandle);
		if (mTRetToSupSpecificationRow == null)
		{
			return;
		}
		if (!_date.DoValidate())
		{
			ControlsHelper.SetFocus(_date);
			throw new ASException(ArmSoft.SME.Common.Resources.ValidateEmptyDate);
		}
		GoodsSelectPartyWindow goodsSelectPartyWindow = new GoodsSelectPartyWindow(DialogWindowMode.NewModeWithNext, this, mTRetToSupSpecificationRow);
		goodsSelectPartyWindow.ShowDialog();
		int addedRows = goodsSelectPartyWindow.AddedRows;
		if (addedRows != 0)
		{
			_maxRowID += addedRows;
			_mtRetToSupGrid.Grid.RefreshData();
			CalcSummAndSummVAT();
			RowInfoChangeEventHandler rowInfoChangeEventHandler = RowInfoChange;
			if (rowInfoChangeEventHandler != null)
			{
				ObservableCollection<MTRetToSupSpecificationRow> mtRetToSupList = _mtRetToSupList;
				rowInfoChangeEventHandler(mtRetToSupList[mtRetToSupList.Count - 1]);
			}
		}
	}

	private void MTRetToSupGrid_EditingRowWithDialog(object sender, DocumentGridEditingRowEventArgs e)
	{
		MTRetToSupSpecificationRow mTRetToSupSpecificationRow = (MTRetToSupSpecificationRow)_mtRetToSupGrid.Grid.GetRow(e.RowHandle);
		if (mTRetToSupSpecificationRow != null)
		{
			if (!_date.DoValidate())
			{
				ControlsHelper.SetFocus(_date);
				throw new ASException(ArmSoft.SME.Common.Resources.ValidateEmptyDate);
			}
			GoodsSelectPartyWindow goodsSelectPartyWindow = new GoodsSelectPartyWindow(e.AllowEdit ? DialogWindowMode.EditMode : DialogWindowMode.ViewMode, this, mTRetToSupSpecificationRow);
			e.RowEdited = Convert.ToBoolean(goodsSelectPartyWindow.ShowDialog());
			if (e.AllowEdit && e.RowEdited)
			{
				_mtRetToSupGrid.Grid.RefreshData();
				CalcSummAndSummVAT();
				RowInfoChange?.Invoke(mTRetToSupSpecificationRow);
			}
		}
	}

	private void MTRetToSupGrid_ClickDropDown(object sender, DocumentGridClickDropDownEventArgs e)
	{
		try
		{
			MTRetToSupSpecificationRow mTRetToSupSpecificationRow = (MTRetToSupSpecificationRow)_mtRetToSupGrid.Grid.GetRow(e.RowHandle);
			if (mTRetToSupSpecificationRow == null)
			{
				return;
			}
			if (e.Column == _mtCodeCol)
			{
				if (!_date.DoValidate() || !_date.EditValue.HasValue)
				{
					ControlsHelper.SetFocus(_date);
					throw new ASException(ArmSoft.SME.Common.Resources.ValidateEmptyDate);
				}
				StoragesCacheRow item = TradeSettings.TradeCacheService.Storage.GetItem(mTRetToSupSpecificationRow.Storage);
				bool showAll = TradeSettings.CreateTradeClient().CommonServices.IsNegativExpAllow(mTRetToSupSpecificationRow.Storage);
				ArmSoft.SME.Trade.UI.DropDown.GoodsAvailabilityFilter goodsAvailabilityFilter = new ArmSoft.SME.Trade.UI.DropDown.GoodsAvailabilityFilter(_date.EditValue.Value, mTRetToSupSpecificationRow.Storage, item != null && (TradeSettings.TradeCacheService.Storage.GetItem(mTRetToSupSpecificationRow.Storage)?.Retail ?? false), includeServices: false, showAll);
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
			else if (e.Column == _salePriceCol)
			{
				if (mTRetToSupSpecificationRow.MTID != -1)
				{
					StoragesCacheRow item2 = TradeSettings.TradeCacheService.Storage.GetItem(mTRetToSupSpecificationRow.Storage);
					if (item2 != null && item2.Retail && TradeSettings.TradeCacheService.Material.GetItem(mTRetToSupSpecificationRow.MTCode).Type == ItemTypesSE.Goods.Value)
					{
						MaterialCacheRow.QntUnitRow qntUnit = TradeSettings.TradeCacheService.Material.GetQntUnit(mTRetToSupSpecificationRow.MTID, mTRetToSupSpecificationRow.UnitCode);
						if (qntUnit != null)
						{
							MTRetailQtyAndPrice mTRetailQtyAndPrice = new MTRetailQtyAndPrice(mTRetToSupSpecificationRow.MTCode, qntUnit.Coef, mTRetToSupSpecificationRow.Storage, _date.EditValue);
							ModalBrowser modalBrowser2 = new ModalBrowser(mTRetailQtyAndPrice);
							modalBrowser2.LookUpValue = mTRetToSupSpecificationRow.UnitCode;
							modalBrowser2.Owner = e.Owner;
							modalBrowser2.Left = e.Left;
							modalBrowser2.Top = e.Top;
							if (modalBrowser2.Show())
							{
								mTRetToSupSpecificationRow.SalePrice = Convert.ToDecimal(mTRetailQtyAndPrice.CommentMember(modalBrowser2.SelectedRowIndex));
								mTRetToSupSpecificationRow.SaleSumma = (mTRetToSupSpecificationRow.SalePrice * mTRetToSupSpecificationRow.Quantity).RoundAs(2);
							}
						}
					}
				}
			}
			else if (e.Column == _partyCol)
			{
				if (!_date.DoValidate() || !_date.EditValue.HasValue)
				{
					ControlsHelper.SetFocus(_date);
					throw new ASException(ArmSoft.SME.Common.Resources.ValidateEmptyDate);
				}
				if (string.IsNullOrWhiteSpace(mTRetToSupSpecificationRow.Storage))
				{
					throw new ASException(ArmSoft.SME.Common.Resources.ValidateEmptyStorage);
				}
				if (string.IsNullOrWhiteSpace(mTRetToSupSpecificationRow.MTCode))
				{
					throw new ASException(ArmSoft.SME.Common.Resources.ValidateEmptyCode);
				}
				(mTRetToSupSpecificationRow.Quantity * ArmSoft.SME.Trade.UI.Helpers.PriceHelper.GetMTQntUnitCoeff(mTRetToSupSpecificationRow.MTCode, mTRetToSupSpecificationRow.UnitCode)).RoundAs(3);
				MaterialCacheRow item3 = TradeSettings.TradeCacheService.Material.GetItem(mTRetToSupSpecificationRow.MTCode);
				MTSelectPartyWindow mTSelectPartyWindow = new MTSelectPartyWindow(mTRetToSupSpecificationRow.Storage, _date.EditValue.Value, mTRetToSupSpecificationRow.MTCode, item3.Unit, mTRetToSupSpecificationRow.Quantity, mTRetToSupSpecificationRow.RowID, PO.StoredInDb && !IsDraftOrDeleted)
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
						mTRetToSupSpecificationRow.MTID = item3.Id;
						mTRetToSupSpecificationRow.MTName = item3.Caption;
						mTRetToSupSpecificationRow.MTCode = mTSelectPartyWindow.MTCode;
						mTRetToSupSpecificationRow.UnitCode = item3.Unit;
						mTRetToSupSpecificationRow.UnitBrief = item3.UnitRow.Brief;
						mTRetToSupSpecificationRow.Quantity = mTSelectPartyWindow.TotalExpQty;
						mTRetToSupSpecificationRow.Summa = (mTRetToSupSpecificationRow.Quantity * mTRetToSupSpecificationRow.DiscountPrice).RoundAs(2);
						mTRetToSupSpecificationRow.IsPartySelected = mTSelectPartyWindow.IsPartySelected.Value;
						mTRetToSupSpecificationRow.ValidateWithoutResponse();
						_mtRetToSupGrid.Grid.RefreshRow(e.RowHandle);
					}
				}
			}
			_mtRetToSupGrid.Grid.RefreshData();
			CalcSummAndSummVAT();
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	private void MTRetToSupGrid_DeletedRow(object sender, EventArgs e)
	{
		CalcSummAndSummVAT();
		_mtRetToSupGrid.Grid.RefreshData();
	}

	private void MTRetToSupGrid_DeletingRow(object sender, DocumentGridDeletingRowEventArgs e)
	{
		MTRetToSupSpecificationRow mtInvGridRow = (MTRetToSupSpecificationRow)_mtRetToSupGrid.Grid.GetRow(e.RowHandle);
		if (mtInvGridRow != null)
		{
			PO.MTExpDocSelectedPartiesList.RemoveAll((PartyRow x) => x.GridRow == mtInvGridRow.RowID);
			RowInfoChange?.Invoke((MTRetToSupSpecificationRow)_mtRetToSupTableView.Grid.CurrentItem);
		}
	}

	private void MTRetToSupGrid_DuplicatingRow(object sender, DocumentGridDuplicatingRowEventArgs e)
	{
		MTRetToSupSpecificationRow mTRetToSupSpecificationRow = (MTRetToSupSpecificationRow)_mtRetToSupGrid.Grid.GetRow(e.RowHandle);
		if (mTRetToSupSpecificationRow != null)
		{
			MTRetToSupSpecificationRow mTRetToSupSpecificationRow2 = new MTRetToSupSpecificationRow();
			mTRetToSupSpecificationRow.Clone(mTRetToSupSpecificationRow2);
			mTRetToSupSpecificationRow2.IsPartySelected = false;
			mTRetToSupSpecificationRow2.RowID = _maxRowID;
			_mtRetToSupList.Add(mTRetToSupSpecificationRow2);
			_maxRowID++;
			e.NewRowNumber = _mtRetToSupList.Count - 1;
			CalcSummAndSummVAT();
			ChangeRowInfo(mTRetToSupSpecificationRow2);
		}
	}

	private void MTRetToSupGrid_MovingRowUp(object sender, DocumentGridMovingRowUpOrDownEventArgs e)
	{
		if (_mtRetToSupGrid.Grid.SortInfo.Count > 0 || _mtRetToSupTableView.Grid.VisibleRowCount != _mtRetToSupList.Count)
		{
			throw new ASException(ArmSoft.SME.Common.Resources.NotAllowRowMoveInSortedGrid);
		}
		MTRetToSupSpecificationRow mTRetToSupSpecificationRow = (MTRetToSupSpecificationRow)_mtRetToSupGrid.Grid.GetRow(e.RowHandle);
		if (mTRetToSupSpecificationRow != null)
		{
			_mtRetToSupList.RemoveAt(e.RowHandle);
			_mtRetToSupList.Insert(e.RowHandle - 1, mTRetToSupSpecificationRow);
		}
		_mtRetToSupGrid.Grid.RefreshData();
	}

	private void MTRetToSupGrid_MovingRowDown(object sender, DocumentGridMovingRowUpOrDownEventArgs e)
	{
		if (_mtRetToSupGrid.Grid.SortInfo.Count > 0 || _mtRetToSupTableView.Grid.VisibleRowCount != _mtRetToSupList.Count)
		{
			throw new ASException(ArmSoft.SME.Common.Resources.NotAllowRowMoveInSortedGrid);
		}
		MTRetToSupSpecificationRow mTRetToSupSpecificationRow = (MTRetToSupSpecificationRow)_mtRetToSupGrid.Grid.GetRow(e.RowHandle);
		if (mTRetToSupSpecificationRow != null)
		{
			_mtRetToSupList.RemoveAt(e.RowHandle);
			_mtRetToSupList.Insert(e.RowHandle + 1, mTRetToSupSpecificationRow);
		}
		_mtRetToSupGrid.Grid.RefreshData();
	}

	private void MTRetToSupTableView_CellValueChanged(object sender, CellValueChangedEventArgs e)
	{
		MTRetToSupSpecificationRow mtRetToSupGridRow = (MTRetToSupSpecificationRow)_mtRetToSupGrid.Grid.GetRow(e.RowHandle);
		if (mtRetToSupGridRow == null)
		{
			return;
		}
		string text = string.Empty;
		if (e.OldValue != null)
		{
			text = e.OldValue.ToString();
		}
		if (e.Column == _storageCol)
		{
			if (e.Value != null)
			{
				if (string.IsNullOrWhiteSpace(e.Value.ToString()))
				{
					mtRetToSupGridRow.SalePrice = 0m;
					mtRetToSupGridRow.SaleSumma = 0m;
				}
				else if (!TradeSettings.CreateTradeClient().CommonServices.IsStorageAvailable(e.Value.ToString()))
				{
					ArmSoft.SME.Core.UI.MessageBox.Show(string.Format(ArmSoft.SME.Trade.Common.Resources.StorageNotAvailForUser, Settings.LoginService.UserName, e.Value.ToString()), Settings.ProgramName, MessageBoxButton.OK, MessageBoxImage.Exclamation);
					mtRetToSupGridRow.Storage = text;
				}
				else
				{
					if (mtRetToSupGridRow.IsPartySelected)
					{
						PO.MTExpDocSelectedPartiesList.RemoveAll((PartyRow x) => x.GridRow == mtRetToSupGridRow.RowID);
						mtRetToSupGridRow.IsPartySelected = false;
					}
					StoragesCacheRow item = TradeSettings.TradeCacheService.Storage.GetItem(e.Value.ToString());
					if (item != null && item.Retail)
					{
						if (mtRetToSupGridRow.MTID != -1)
						{
							MaterialCacheRow item2 = TradeSettings.TradeCacheService.Material.GetItem(mtRetToSupGridRow.MTCode);
							mtRetToSupGridRow.UnitCode = item2.Unit;
							mtRetToSupGridRow.UnitBrief = item2.UnitRow.Brief;
							_unitCodeCol.DataView = new MTQntUnits(item2.MTCode);
							MaterialCacheRow.QntUnitRow qntUnit = TradeSettings.TradeCacheService.Material.GetQntUnit(mtRetToSupGridRow.MTID, mtRetToSupGridRow.UnitCode);
							if (qntUnit != null)
							{
								MTRetailQtyAndPriceDataProvider.Param param = new MTRetailQtyAndPriceDataProvider.Param
								{
									MTCode = mtRetToSupGridRow.MTCode,
									UnitCoef = qntUnit.Coef,
									Storage = mtRetToSupGridRow.Storage,
									Date = _date.EditValue
								};
								List<MTRetailQtyAndPriceDataProvider.DataRow> rows = new MTRetailQtyAndPriceDataProvider(TradeSettings.CreateTradeClient()).GetRows(param);
								if (rows.Count > 0)
								{
									mtRetToSupGridRow.SalePrice = rows[0].Price;
								}
							}
						}
					}
					else
					{
						mtRetToSupGridRow.SalePrice = 0m;
						mtRetToSupGridRow.SaleSumma = 0m;
					}
				}
				RowInfoChange?.Invoke(mtRetToSupGridRow);
			}
		}
		else if (e.Column == _mtCodeCol)
		{
			try
			{
				if (e.Value == null)
				{
					PO.MTExpDocSelectedPartiesList.RemoveAll((PartyRow x) => x.GridRow == mtRetToSupGridRow.RowID);
					mtRetToSupGridRow.MTID = -1;
					mtRetToSupGridRow.MTCode = "";
					mtRetToSupGridRow.MTName = "";
					mtRetToSupGridRow.UnitBrief = "";
					mtRetToSupGridRow.UnitCode = "";
					mtRetToSupGridRow.Quantity = 0m;
					mtRetToSupGridRow.Discount = 0m;
					mtRetToSupGridRow.DiscountPrice = 0m;
					mtRetToSupGridRow.Price = 0m;
					mtRetToSupGridRow.Summa = 0m;
					mtRetToSupGridRow.SalePrice = 0m;
					mtRetToSupGridRow.SaleSumma = 0m;
					mtRetToSupGridRow.VAT = false;
					mtRetToSupGridRow.IsPartySelected = false;
				}
				else
				{
					if (string.IsNullOrWhiteSpace(e.Value.ToString()))
					{
						throw new Exception();
					}
					MaterialCacheRow item3 = TradeSettings.TradeCacheService.Material.GetItem(mtRetToSupGridRow.MTCode, throwExceptionNotExits: true);
					mtRetToSupGridRow.MTID = item3.Id;
					mtRetToSupGridRow.MTName = item3.Caption;
					mtRetToSupGridRow.UnitCode = TradeSettings.CreateTradeClient().CommonServices.GetMTQntUnitByMTIDPriceTypeAndDocDate(mtRetToSupGridRow.MTID, _priceListType.Code, _date.EditValue.Value);
					if (string.IsNullOrWhiteSpace(mtRetToSupGridRow.UnitCode))
					{
						mtRetToSupGridRow.UnitCode = item3.Unit;
						mtRetToSupGridRow.UnitBrief = item3.UnitRow.Brief;
					}
					else
					{
						QntUnitCacheRow item4 = TradeSettings.TradeCacheService.QntUnit.GetItem(mtRetToSupGridRow.UnitCode);
						mtRetToSupGridRow.UnitBrief = ((item4 != null) ? item4.Brief : "");
					}
					_unitCodeCol.DataView = new MTQntUnits(mtRetToSupGridRow.MTCode);
					mtRetToSupGridRow.Price = TradeSettings.CreateTradeClient().CommonServices.GetPriceForGivenPriceListAndVATType(_priceListType.Code, _vatType.Code, mtRetToSupGridRow.MTID, mtRetToSupGridRow.UnitCode, _date.EditValue.Value, _currCode.Code);
					if (_isParamAllowDiscountInReturn)
					{
						mtRetToSupGridRow.Discount = _discountPercent.Value;
					}
					mtRetToSupGridRow.DiscountPrice = (mtRetToSupGridRow.Price - mtRetToSupGridRow.Price * mtRetToSupGridRow.Discount / 100m).RoundAs(2);
					mtRetToSupGridRow.Summa = (mtRetToSupGridRow.Quantity * mtRetToSupGridRow.DiscountPrice).RoundAs(2);
					if (_vatType.Code.Trim().InList("1", "2"))
					{
						mtRetToSupGridRow.VAT = item3.VAT;
					}
					mtRetToSupGridRow.IsPartySelected = false;
					if (!string.IsNullOrWhiteSpace(mtRetToSupGridRow.Storage))
					{
						StoragesCacheRow item5 = TradeSettings.TradeCacheService.Storage.GetItem(mtRetToSupGridRow.Storage);
						if (item5 != null && item5.Retail)
						{
							MaterialCacheRow.QntUnitRow qntUnit2 = TradeSettings.TradeCacheService.Material.GetQntUnit(mtRetToSupGridRow.MTID, mtRetToSupGridRow.UnitCode);
							if (qntUnit2 != null)
							{
								MTRetailQtyAndPriceDataProvider.Param param2 = new MTRetailQtyAndPriceDataProvider.Param
								{
									MTCode = mtRetToSupGridRow.MTCode,
									UnitCoef = qntUnit2.Coef,
									Storage = mtRetToSupGridRow.Storage,
									Date = _date.EditValue
								};
								List<MTRetailQtyAndPriceDataProvider.DataRow> rows2 = new MTRetailQtyAndPriceDataProvider(TradeSettings.CreateTradeClient()).GetRows(param2);
								if (rows2.Count > 0)
								{
									mtRetToSupGridRow.SalePrice = rows2[0].Price;
									mtRetToSupGridRow.SaleSumma = (mtRetToSupGridRow.SalePrice * mtRetToSupGridRow.Quantity).RoundAs(2);
								}
							}
						}
					}
				}
			}
			catch (Exception)
			{
				mtRetToSupGridRow.MTID = -1;
				mtRetToSupGridRow.MTCode = "";
				mtRetToSupGridRow.MTName = "";
				mtRetToSupGridRow.UnitBrief = "";
				mtRetToSupGridRow.UnitCode = "";
				mtRetToSupGridRow.Quantity = 0m;
				mtRetToSupGridRow.Price = 0m;
				mtRetToSupGridRow.Discount = 0m;
				mtRetToSupGridRow.DiscountPrice = 0m;
				mtRetToSupGridRow.Summa = 0m;
				mtRetToSupGridRow.SalePrice = 0m;
				mtRetToSupGridRow.SaleSumma = 0m;
				mtRetToSupGridRow.VAT = false;
				mtRetToSupGridRow.IsPartySelected = false;
			}
			finally
			{
				RowInfoChange?.Invoke(mtRetToSupGridRow);
			}
		}
		else if (e.Column == _unitCodeCol)
		{
			try
			{
				if (e.Value != null && !string.IsNullOrWhiteSpace(e.Value.ToString()) && text != e.Value.ToString() && !string.IsNullOrWhiteSpace(mtRetToSupGridRow.MTCode))
				{
					QntUnitCacheRow item6 = TradeSettings.TradeCacheService.QntUnit.GetItem(e.Value.ToString(), throwExceptionNotExits: true);
					if (item6 != null)
					{
						mtRetToSupGridRow.UnitBrief = item6.Brief;
						if (!string.IsNullOrWhiteSpace(_priceListType.Code))
						{
							PricesDataProvider.DataRow price = TradeSettings.CreateTradeClient().CommonServices.GetPrice(_priceListType.Code, mtRetToSupGridRow.MTID, item6.Code, _date.EditValue.Value, _currCode.Code);
							if (price.Price == 0m)
							{
								mtRetToSupGridRow.Price = ArmSoft.SME.Trade.UI.Helpers.PriceHelper.GetPriceForUnit(text, item6.Code, mtRetToSupGridRow.Price, mtRetToSupGridRow.MTCode);
							}
							else
							{
								mtRetToSupGridRow.Price = price.Price;
							}
							decimal priceForGivenPriceListAndVATType = TradeSettings.CreateTradeClient().CommonServices.GetPriceForGivenPriceListAndVATType(_priceListType.Code, _vatType.Code, mtRetToSupGridRow.MTID, item6.Code, _date.EditValue.Value, _currCode.Code);
							if (priceForGivenPriceListAndVATType != 0m)
							{
								mtRetToSupGridRow.Price = priceForGivenPriceListAndVATType;
							}
						}
						else
						{
							MaterialCacheRow.QntUnitRow qntUnit3 = TradeSettings.TradeCacheService.Material.GetQntUnit(mtRetToSupGridRow.MTID, item6.Code);
							MaterialCacheRow.QntUnitRow qntUnit4 = TradeSettings.TradeCacheService.Material.GetQntUnit(mtRetToSupGridRow.MTID, text);
							if (qntUnit3 != null && qntUnit4 != null)
							{
								mtRetToSupGridRow.Price = ArmSoft.SME.Trade.UI.Helpers.PriceHelper.GetPriceForUnit(text, item6.Code, mtRetToSupGridRow.Price, mtRetToSupGridRow.MTCode);
								mtRetToSupGridRow.SalePrice = ArmSoft.SME.Trade.UI.Helpers.PriceHelper.GetPriceForUnit(text, item6.Code, mtRetToSupGridRow.SalePrice, mtRetToSupGridRow.MTCode);
							}
							mtRetToSupGridRow.SaleSumma = (mtRetToSupGridRow.SalePrice * mtRetToSupGridRow.Quantity).RoundAs(2);
						}
						mtRetToSupGridRow.DiscountPrice = (mtRetToSupGridRow.Price - mtRetToSupGridRow.Price * mtRetToSupGridRow.Discount / 100m).RoundAs(2);
						mtRetToSupGridRow.Summa = (mtRetToSupGridRow.Quantity * mtRetToSupGridRow.DiscountPrice).RoundAs(2);
					}
					else
					{
						mtRetToSupGridRow.UnitBrief = "";
					}
				}
				RowInfoChange?.Invoke(mtRetToSupGridRow);
			}
			catch (Exception ex2)
			{
				Settings.ErrorHandler(ex2);
			}
		}
		else if (e.Column == _quantityCol)
		{
			if (Convert.ToDecimal(e.OldValue) != Convert.ToDecimal(e.Value) && Convert.ToDecimal(e.OldValue) != 0m && mtRetToSupGridRow.IsPartySelected)
			{
				ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Trade.Common.Resources.ForRowSelectedParties, Settings.ProgramName, MessageBoxButton.OK, MessageBoxImage.Asterisk);
				PO.MTExpDocSelectedPartiesList.RemoveAll((PartyRow x) => x.GridRow == mtRetToSupGridRow.RowID);
				mtRetToSupGridRow.IsPartySelected = false;
			}
			if (mtRetToSupGridRow.Price == 0m && !string.IsNullOrWhiteSpace(_priceListType.Code))
			{
				mtRetToSupGridRow.Price = TradeSettings.CreateTradeClient().CommonServices.GetPriceForGivenPriceListAndVATType(_priceListType.Code, _vatType.Code, mtRetToSupGridRow.MTID, mtRetToSupGridRow.UnitCode, _date.EditValue.Value, _currCode.Code);
				mtRetToSupGridRow.DiscountPrice = (mtRetToSupGridRow.Price - mtRetToSupGridRow.Price * mtRetToSupGridRow.Discount / 100m).RoundAs(2);
			}
			mtRetToSupGridRow.Summa = (mtRetToSupGridRow.DiscountPrice * Convert.ToDecimal(e.Value)).RoundAs(2);
			mtRetToSupGridRow.SaleSumma = (mtRetToSupGridRow.SalePrice * Convert.ToDecimal(e.Value)).RoundAs(2);
		}
		else if (e.Column == _priceCol)
		{
			mtRetToSupGridRow.Price = Convert.ToDecimal(e.Value);
			mtRetToSupGridRow.DiscountPrice = (mtRetToSupGridRow.Price - mtRetToSupGridRow.Price * mtRetToSupGridRow.Discount / 100m).RoundAs(2);
			mtRetToSupGridRow.Summa = (mtRetToSupGridRow.DiscountPrice * mtRetToSupGridRow.Quantity).RoundAs(2);
		}
		else if (e.Column == _summaCol && mtRetToSupGridRow.Quantity != 0m)
		{
			mtRetToSupGridRow.Price = (Convert.ToDecimal(e.Value) / mtRetToSupGridRow.Quantity).RoundAs(2);
			mtRetToSupGridRow.DiscountPrice = (mtRetToSupGridRow.Price - mtRetToSupGridRow.Price * mtRetToSupGridRow.Discount / 100m).RoundAs(2);
		}
		else if (e.Column == _salePriceCol)
		{
			mtRetToSupGridRow.SaleSumma = (Convert.ToDecimal(e.Value) * mtRetToSupGridRow.Quantity).RoundAs(2);
		}
		else if (e.Column == _saleSummaCol && mtRetToSupGridRow.Quantity != 0m)
		{
			mtRetToSupGridRow.SalePrice = (Convert.ToDecimal(e.Value) / mtRetToSupGridRow.Quantity).RoundAs(2);
		}
		else if (e.Column == _discountCol)
		{
			mtRetToSupGridRow.DiscountPrice = (mtRetToSupGridRow.Price - mtRetToSupGridRow.Price * mtRetToSupGridRow.Discount / 100m).RoundAs(2);
			mtRetToSupGridRow.Summa = (mtRetToSupGridRow.DiscountPrice * mtRetToSupGridRow.Quantity).RoundAs(2);
		}
		else if (e.Column == _discountPriceCol)
		{
			mtRetToSupGridRow.DiscountPrice = Convert.ToDecimal(e.Value);
			mtRetToSupGridRow.Summa = (mtRetToSupGridRow.DiscountPrice * mtRetToSupGridRow.Quantity).RoundAs(2);
			if (mtRetToSupGridRow.Price != 0m)
			{
				mtRetToSupGridRow.Discount = (100m * (mtRetToSupGridRow.Price - mtRetToSupGridRow.DiscountPrice) / mtRetToSupGridRow.Price).RoundAs(2);
				if (mtRetToSupGridRow.Discount / 1000m >= 1m || mtRetToSupGridRow.Discount / 1000m <= -1m)
				{
					mtRetToSupGridRow.Discount = 0m;
					mtRetToSupGridRow.DiscountPrice = mtRetToSupGridRow.Price;
					mtRetToSupGridRow.Summa = mtRetToSupGridRow.Price * mtRetToSupGridRow.Quantity;
				}
			}
		}
		_mtRetToSupGrid.Grid.RefreshData();
		CalcSummAndSummVAT();
	}

	private void MTRetToSupTableView_FocusedRowHandleChanged(object sender, FocusedRowHandleChangedEventArgs e)
	{
		int focusedRowHandle = _mtRetToSupTableView.FocusedRowHandle;
		MTRetToSupSpecificationRow mTRetToSupSpecificationRow = (MTRetToSupSpecificationRow)_mtRetToSupGrid.Grid.GetRow(focusedRowHandle);
		if (mTRetToSupSpecificationRow != null)
		{
			ChangeColumnAdditonalView(mTRetToSupSpecificationRow.MTCode);
			RowInfoChange?.Invoke(mTRetToSupSpecificationRow);
		}
	}

	private void ChangeColumnAdditonalView(string mtCode)
	{
		_unitCodeCol.DataView = new MTQntUnits((!string.IsNullOrWhiteSpace(mtCode)) ? mtCode : "");
	}

	private void Date_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		if (!_date.DoValidate() || !_date.EditValue.HasValue)
		{
			ControlsHelper.SetFocus(_date);
			ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Common.Resources.ValidateEmptyDate, Settings.ProgramName, MessageBoxButton.OK, MessageBoxImage.Exclamation);
		}
	}

	private void CurrCode_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		bool flag = false;
		if (_isAskQuestionConfirmSummChange && _totalSumma.Value != 0m && !string.IsNullOrWhiteSpace(_currCode.Code) && _currCode.DoValidate() && ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Trade.Common.Resources.ValidateChangePrices, Settings.ProgramName, MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No) == MessageBoxResult.Yes)
		{
			flag = true;
		}
		ChangedCurrCodeOrDate();
		if (flag)
		{
			UpdatePricesForCurr();
		}
		if (!string.IsNullOrWhiteSpace(_currCode.Code) && _currCode.DoValidate())
		{
			_mOldDocCurrCode = _currCode.Code;
			_mOldDocCurrExChngRate = _currExChngRate.Rate;
			_mOldDocCurrExChngBase = _currExChngRate.Base;
		}
	}

	private void PriceListType_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		bool flag = false;
		string priceListRowVATType;
		string priceListRowCurr;
		if (_isAskQuestionConfirmSummChange && !string.IsNullOrWhiteSpace(_priceListType.Code) && _priceListType.DoValidate())
		{
			if (_totalSumma.Value != 0m)
			{
				if (ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Trade.Common.Resources.ValidateChangePrices, Settings.ProgramName, MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No) == MessageBoxResult.Yes)
				{
					flag = true;
				}
			}
			else
			{
				flag = true;
				if (ArmSoft.SME.Trade.UI.Helpers.PriceHelper.GetPriceListRowVATType(_priceListType.Code, out priceListRowVATType, out priceListRowCurr))
				{
					_vatType.Code = priceListRowVATType;
				}
			}
		}
		if (flag)
		{
			UpdateMTRetToSupPrices();
			ArmSoft.SME.Trade.UI.Helpers.PriceHelper.GetPriceListRowVATType(_priceListType.Code, out priceListRowVATType, out priceListRowCurr);
			if (priceListRowVATType != _vatType.Code)
			{
				VATTypeValidate(priceListRowVATType, _vatType.Code);
			}
			_mtRetToSupGrid.Grid.RefreshData();
			CalcSummAndSummVAT();
		}
	}

	private void VATType_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		if (!string.IsNullOrWhiteSpace(_vatType.Code))
		{
			VATType_EditValueChangedForLoad();
			VATTypeValidate(e.OldValue.ToString(), _vatType.Code);
			_mtRetToSupGrid.Grid.RefreshData();
		}
	}

	private void VATType_EditValueChangedForLoad()
	{
		bool flag = _vatType.Code.InList("1", "2");
		_mtRetToSupGrid.Grid.Columns["VAT"].Visible = flag;
		_mtRetToSupGrid.Grid.Columns["VAT"].ShowInColumnChooser = flag;
		if (!flag)
		{
			_vatSumma.EditValue = 0m;
		}
	}

	private void VATTypeValidate(string VATTypeOldValue, string VATTypeNewValue)
	{
		decimal num = 0m;
		decimal num2 = 0m;
		if (_mtRetToSupList.Count > 0)
		{
			switch (VATTypeNewValue)
			{
			case "1":
				if (VATTypeOldValue == "3" || VATTypeOldValue == "4")
				{
					foreach (MTRetToSupSpecificationRow mtRetToSup in _mtRetToSupList)
					{
						num += ArmSoft.SME.Trade.UI.Helpers.PriceHelper.CalcVATAmountByType(mtRetToSup.Summa, "1", mtRetToSup.VAT);
						num2 += mtRetToSup.Summa;
					}
				}
				else
				{
					foreach (MTRetToSupSpecificationRow mtRetToSup2 in _mtRetToSupList)
					{
						mtRetToSup2.Price -= ArmSoft.SME.Trade.UI.Helpers.PriceHelper.CalcVATAmountByType(mtRetToSup2.Price, VATTypeOldValue, mtRetToSup2.VAT);
						mtRetToSup2.DiscountPrice = (mtRetToSup2.Price * (1m - mtRetToSup2.Discount / 100m)).RoundAs(2);
						mtRetToSup2.Summa = (mtRetToSup2.DiscountPrice * mtRetToSup2.Quantity).RoundAs(2);
						num += ArmSoft.SME.Trade.UI.Helpers.PriceHelper.CalcVATAmountByType(mtRetToSup2.Summa, "1", mtRetToSup2.VAT);
						num2 += mtRetToSup2.Summa;
					}
				}
				_vatSumma.EditValue = num;
				_totalSumma.EditValue = num2 + num;
				break;
			case "2":
				switch (VATTypeOldValue)
				{
				case "3":
				case "4":
					foreach (MTRetToSupSpecificationRow mtRetToSup3 in _mtRetToSupList)
					{
						num += ArmSoft.SME.Trade.UI.Helpers.PriceHelper.CalcVATAmountByType(mtRetToSup3.Summa, "1", mtRetToSup3.VAT);
						mtRetToSup3.Price += ArmSoft.SME.Trade.UI.Helpers.PriceHelper.CalcVATAmountByType(mtRetToSup3.Price, "1", mtRetToSup3.VAT);
						mtRetToSup3.DiscountPrice = (mtRetToSup3.Price * (1m - mtRetToSup3.Discount / 100m)).RoundAs(2);
						mtRetToSup3.Summa = (mtRetToSup3.DiscountPrice * mtRetToSup3.Quantity).RoundAs(2);
						num2 += mtRetToSup3.Summa;
					}
					_vatSumma.EditValue = num;
					_totalSumma.EditValue = num2;
					break;
				case "1":
					foreach (MTRetToSupSpecificationRow mtRetToSup4 in _mtRetToSupList)
					{
						num += ArmSoft.SME.Trade.UI.Helpers.PriceHelper.CalcVATAmountByType(mtRetToSup4.Summa, "1", mtRetToSup4.VAT);
						mtRetToSup4.Price += ArmSoft.SME.Trade.UI.Helpers.PriceHelper.CalcVATAmountByType(mtRetToSup4.Price, "1", mtRetToSup4.VAT);
						mtRetToSup4.DiscountPrice = (mtRetToSup4.Price * (1m - mtRetToSup4.Discount / 100m)).RoundAs(2);
						mtRetToSup4.Summa = (mtRetToSup4.DiscountPrice * mtRetToSup4.Quantity).RoundAs(2);
						num2 += mtRetToSup4.Summa;
					}
					_vatSumma.EditValue = num;
					_totalSumma.EditValue = num2;
					break;
				}
				break;
			case "3":
			case "4":
				if (VATTypeOldValue == "2")
				{
					foreach (MTRetToSupSpecificationRow mtRetToSup5 in _mtRetToSupList)
					{
						mtRetToSup5.Price -= ArmSoft.SME.Trade.UI.Helpers.PriceHelper.CalcVATAmountByType(mtRetToSup5.Price, VATTypeOldValue, mtRetToSup5.VAT);
						mtRetToSup5.DiscountPrice = (mtRetToSup5.Price * (1m - mtRetToSup5.Discount / 100m)).RoundAs(2);
						mtRetToSup5.Summa = (mtRetToSup5.DiscountPrice * mtRetToSup5.Quantity).RoundAs(2);
						num2 += mtRetToSup5.Summa;
					}
					_vatSumma.EditValue = 0m;
					_totalSumma.EditValue = num2;
				}
				else
				{
					if (!(VATTypeOldValue == "1"))
					{
						break;
					}
					foreach (MTRetToSupSpecificationRow mtRetToSup6 in _mtRetToSupList)
					{
						num2 += mtRetToSup6.Summa;
					}
					_vatSumma.EditValue = 0m;
					_totalSumma.EditValue = num2;
				}
				break;
			}
		}
		CalculatePartSumma();
	}

	private void UpdateMTRetToSupPrices()
	{
		decimal num = 1m;
		decimal num2 = 1m;
		ArmSoft.SME.Trade.UI.Helpers.PriceHelper.GetPriceListRowVATType(_priceListType.Code, out var _, out var priceListRowCurr);
		if (priceListRowCurr != "AMD")
		{
			ExchangeRate exchangeRates = TradeSettings.CreateTradeClient().CommonServices.GetExchangeRates(priceListRowCurr, _date.EditValue.Value);
			num = exchangeRates.Rate;
			num2 = exchangeRates.Base;
		}
		foreach (MTRetToSupSpecificationRow mtRetToSup in _mtRetToSupList)
		{
			PricesDataProvider.DataRow price = TradeSettings.CreateTradeClient().CommonServices.GetPrice(_priceListType.Code, mtRetToSup.MTID, mtRetToSup.UnitCode, _date.EditValue.Value);
			if (priceListRowCurr == _currCode.Code)
			{
				mtRetToSup.Price = price.Price;
			}
			else if (priceListRowCurr == "AMD")
			{
				mtRetToSup.Price = ArmSoft.SME.Trade.Common.Helpers.PriceHelper.GetSumCurr(price.Price, new ExchangeRate(_currExChngRate.Base, _currExChngRate.Rate));
			}
			else
			{
				mtRetToSup.Price = (price.Price * num / num2 / _currExChngRate.Rate * _currExChngRate.Base).RoundAs(2);
			}
			mtRetToSup.DiscountPrice = (mtRetToSup.Price - mtRetToSup.Price * mtRetToSup.Discount / 100m).RoundAs(2);
			mtRetToSup.Summa = (mtRetToSup.DiscountPrice * mtRetToSup.Quantity).RoundAs(2);
		}
	}

	private void ChangedCurrCodeOrDate()
	{
		if (string.IsNullOrWhiteSpace(_currCode.Code) || _currCode.Code == "AMD" || !_currCode.DoValidate())
		{
			_currExChngRate.IsReadOnly = true;
			_currExChngRate.Rate = 1m;
			_currExChngRate.Base = 1m;
		}
		else
		{
			bool flag = State == ArmSoft.SME.Trade.Common.SmartEnums.DocumentStateSE.Draft.Value || State == ArmSoft.SME.Trade.Common.SmartEnums.DocumentStateSE.Deleted.Value;
			_currExChngRate.IsReadOnly = PO.StoredInDb && !flag;
			try
			{
				if (!_date.DoValidate())
				{
					throw new ASException(ArmSoft.SME.Common.Resources.ValidateEmptyDate);
				}
				ExchangeRate exchangeRates = TradeSettings.CreateTradeClient().CommonServices.GetExchangeRates(_currCode.Code, _date.EditValue.Value);
				_currExChngRate.Rate = exchangeRates.Rate;
				_currExChngRate.Base = exchangeRates.Base;
			}
			catch (Exception ex)
			{
				Settings.ErrorHandler(ex);
			}
		}
		_currExChngRate.DoValidate();
	}

	private void UpdatePricesForCurr()
	{
		foreach (MTRetToSupSpecificationRow mtRetToSup in _mtRetToSupList)
		{
			if (_mOldDocCurrCode == "AMD")
			{
				if (_currExChngRate.Rate != 0m)
				{
					mtRetToSup.Price = ArmSoft.SME.Trade.Common.Helpers.PriceHelper.GetSumCurr(mtRetToSup.Price, new ExchangeRate(_currExChngRate.Base, _currExChngRate.Rate));
					mtRetToSup.SalePrice = ArmSoft.SME.Trade.Common.Helpers.PriceHelper.GetSumCurr(mtRetToSup.SalePrice, new ExchangeRate(_currExChngRate.Base, _currExChngRate.Rate));
				}
			}
			else if (!(_currExChngRate.Rate == 0m) && !(_mOldDocCurrExChngBase == 0m))
			{
				mtRetToSup.Price = (mtRetToSup.Price * _mOldDocCurrExChngRate / _mOldDocCurrExChngBase / _currExChngRate.Rate * _currExChngRate.Base).RoundAs(2);
				mtRetToSup.SalePrice = (mtRetToSup.SalePrice * _mOldDocCurrExChngRate / _mOldDocCurrExChngBase / _currExChngRate.Rate * _currExChngRate.Base).RoundAs(2);
			}
			mtRetToSup.Summa = (mtRetToSup.Price * mtRetToSup.Quantity).RoundAs(2);
			mtRetToSup.SaleSumma = (mtRetToSup.SalePrice * mtRetToSup.Quantity).RoundAs(2);
		}
		CalculateDiscount(_mtRetToSupList, _discountPercent.Value, gDiscount: false);
		_mtRetToSupGrid.Grid.RefreshData();
		CalcSummAndSummVAT();
	}

	private void CalculateDiscount(ObservableCollection<MTRetToSupSpecificationRow> mtList, decimal discount, bool gDiscount = true)
	{
		if (gDiscount)
		{
			foreach (MTRetToSupSpecificationRow mt in mtList)
			{
				mt.Discount = discount;
				mt.DiscountPrice = (mt.Price * (1m - discount / 100m)).RoundAs(2);
				mt.Summa = (mt.Quantity * mt.DiscountPrice).RoundAs(2);
			}
			return;
		}
		foreach (MTRetToSupSpecificationRow mt2 in mtList)
		{
			mt2.DiscountPrice = (mt2.Price * (1m - mt2.Discount / 100m)).RoundAs(2);
			mt2.Summa = (mt2.Quantity * mt2.DiscountPrice).RoundAs(2);
		}
	}

	private bool GetDiscountUse()
	{
		return _mtRetToSupList.Any((MTRetToSupSpecificationRow row) => row.Price != row.DiscountPrice);
	}

	private void DiscountPercent_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		if (e.NewValue == null)
		{
			return;
		}
		if (Convert.ToDecimal(e.NewValue.ToString()) != Convert.ToDecimal(e.OldValue.ToString()) && _mtRetToSupList.Count > 0)
		{
			if (Convert.ToDecimal(e.OldValue) != 0m || GetDiscountUse())
			{
				if (ArmSoft.SME.Core.UI.MessageBox.Show(string.Format(ArmSoft.SME.Common.Resources.ComfirmDiscountChange, default(ReadOnlySpan<object>)), Settings.ProgramName, MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No) == MessageBoxResult.Yes)
				{
					CalculateDiscount(_mtRetToSupList, Convert.ToDecimal(e.NewValue));
					CalcSummAndSummVAT();
					_mtRetToSupGrid.Grid.RefreshData();
				}
				else
				{
					_discountPercent.EditValue = Convert.ToDecimal(e.OldValue);
					_discountPercent.DoValidate();
				}
			}
			else
			{
				CalculateDiscount(_mtRetToSupList, Convert.ToDecimal(e.NewValue));
				CalcSummAndSummVAT();
				_mtRetToSupGrid.Grid.RefreshData();
			}
		}
		else
		{
			CalculateDiscount(_mtRetToSupList, Convert.ToDecimal(e.NewValue));
			CalcSummAndSummVAT();
			_mtRetToSupGrid.Grid.RefreshData();
		}
	}

	private void CalcSummAndSummVAT()
	{
		decimal num = 0m;
		decimal num2 = 0m;
		if (_vatType.Code.Trim().InList("1", "2"))
		{
			foreach (MTRetToSupSpecificationRow mtRetToSup in _mtRetToSupList)
			{
				num += ArmSoft.SME.Trade.UI.Helpers.PriceHelper.CalcVATAmountByType(mtRetToSup.Summa, _vatType.Code, mtRetToSup.VAT);
				num2 += mtRetToSup.Summa;
			}
			_vatSumma.EditValue = num;
			_totalSumma.EditValue = num2;
			if (_vatType.Code.Trim() == "1")
			{
				_totalSumma.EditValue = Convert.ToDecimal(_totalSumma.EditValue) + Convert.ToDecimal(_vatSumma.EditValue);
			}
		}
		else
		{
			_totalSumma.EditValue = _mtRetToSupList.Sum((MTRetToSupSpecificationRow row) => row.Summa);
			_vatSumma.EditValue = 0m;
		}
		CalculatePartSumma();
	}

	private void CalculatePartSumma()
	{
		decimal summaNCC = _totalSumma.Value;
		if (_currExChngRate.Base != 0m)
		{
			summaNCC = ArmSoft.SME.Trade.Common.Helpers.PriceHelper.GetSumNNC(_totalSumma.Value, new ExchangeRate(_currExChngRate.Base, _currExChngRate.Rate));
		}
		_supplierContractContainer.CalculatePartSumma(summaNCC);
	}

	private void FilteredSearch_Click(object sender, RoutedEventArgs e)
	{
		try
		{
			if (!_date.DoValidate())
			{
				ControlsHelper.SetFocus(_date);
				throw new ASException(ArmSoft.SME.Common.Resources.ValidateEmptyDate);
			}
			FilteredSearch filteredSearch = new FilteredSearch();
			filteredSearch.FilterDataView = new GoodsGroups(showAsTree: true, filteredSearch: true);
			filteredSearch.SearchDataView = new ArmSoft.SME.Trade.UI.DropDown.GoodsFilter(shortMode: true, GoodsTypesSE.Good);
			if (!string.IsNullOrWhiteSpace(_supplierContractContainer.PartnerCode.Code))
			{
				filteredSearch.FilteredSearchDataView = new ArmSoft.SME.Trade.UI.DropDown.GoodsFilter(shortMode: true, GoodsTypesSE.Good, "", isShowOnlyInPriceList: false, "", applyDefaultDataViewDefinition: true, _supplierContractContainer.PartnerCode.Code);
			}
			filteredSearch.ResultsList = new MTReturnToSupplierFilteredSearchList(this);
			FilteredSearchWindow filteredSearchWindow = new FilteredSearchWindow(filteredSearch, this, isBasketMode: true);
			if (filteredSearchWindow.ShowDialog().Value)
			{
				foreach (MTRetToSupSpecificationRow item2 in filteredSearchWindow.BasketResult)
				{
					item2.Storage = _storage.Code;
					string mTQntUnitByMTIDPriceTypeAndDocDate = TradeSettings.CreateTradeClient().CommonServices.GetMTQntUnitByMTIDPriceTypeAndDocDate(item2.MTID, _priceListType.Code, _date.EditValue.Value);
					if (!string.IsNullOrWhiteSpace(mTQntUnitByMTIDPriceTypeAndDocDate))
					{
						item2.UnitCode = mTQntUnitByMTIDPriceTypeAndDocDate;
					}
					QntUnitCacheRow item = TradeSettings.TradeCacheService.QntUnit.GetItem(item2.UnitCode);
					item2.UnitBrief = ((item == null) ? "" : item.Brief);
					item2.Price = TradeSettings.CreateTradeClient().CommonServices.GetPriceForGivenPriceListAndVATType(_priceListType.Code, _vatType.Code, item2.MTID, item2.UnitCode, _date.EditValue.Value, _currCode.Code);
					if (_isParamAllowDiscountInReturn && _discountPercent.Value != 0m)
					{
						item2.Discount = _discountPercent.Value;
					}
					item2.DiscountPrice = (item2.Price - item2.Price * item2.Discount / 100m).RoundAs(2);
					item2.Summa = (item2.Quantity * item2.DiscountPrice).RoundAs(2);
					item2.RowID = _maxRowID;
					_mtRetToSupList.Add(item2);
					_maxRowID++;
				}
				_mtRetToSupGrid.Grid.RefreshData();
				CalcSummAndSummVAT();
			}
			RowInfoChange?.Invoke((MTRetToSupSpecificationRow)_mtRetToSupTableView.Grid.CurrentItem);
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	private void ViewAvailAndTurn_Click(object sender, RoutedEventArgs e)
	{
		try
		{
			MTRetToSupSpecificationRow mTRetToSupSpecificationRow = (MTRetToSupSpecificationRow)_mtRetToSupTableView.Grid.CurrentItem;
			if (mTRetToSupSpecificationRow != null)
			{
				new MTAvailAndTurnWindow(DialogWindowMode.ViewMode, this, mTRetToSupSpecificationRow.Storage, _supplierContractContainer.PartnerCode.Code, mTRetToSupSpecificationRow.MTID, mTRetToSupSpecificationRow.MTCode, mTRetToSupSpecificationRow.UnitCode).Invoke();
			}
			else
			{
				new MTAvailAndTurnWindow(DialogWindowMode.ViewMode, this, "", _supplierContractContainer.PartnerCode.Code, -1, "", "").Invoke();
			}
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	private void MTRetToSupGrid_RefreshButtons(object sender, EventArgs e)
	{
		_viewAvailAndTurn.IsEnabled = Settings.UserAccessService.GetProviderPermission(ArmSoft.SME.Trade.Common.SmartEnums.DataProviderTypeSE.MatRepAvailability).AllowView || Settings.UserAccessService.GetProviderPermission(ArmSoft.SME.Trade.Common.SmartEnums.DataProviderTypeSE.MatRepAvailabilityTreeView).AllowView || Settings.UserAccessService.GetProviderPermission(ArmSoft.SME.Trade.Common.SmartEnums.DataProviderTypeSE.GoodsRepOperations).AllowView || Settings.UserAccessService.GetProviderPermission(ArmSoft.SME.Trade.Common.SmartEnums.DataProviderTypeSE.MatRepSalesAnalysis).AllowView;
		_filteredSearch.IsEnabled = _mtRetToSupGrid.AllowAdd;
	}

	protected override void OnF8()
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
				ArmSoft.SME.Trade.UI.DropDown.GoodsFilter goodsFilter = new ArmSoft.SME.Trade.UI.DropDown.GoodsFilter(shortMode: true, GoodsTypesSE.Good);
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
			AddMTToList(item, 1m);
			_barCode.Text = "";
			CalcSummAndSummVAT();
			_vatSumma.DoValidate();
			RowInfoChange?.Invoke(_mtRetToSupList[_mtRetToSupList.Count - 1]);
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

	private void FillFromLastPurchase_Click(object sender, RoutedEventArgs e)
	{
		try
		{
			if (_date.EditValue.HasValue)
			{
				if (_mtRetToSupList.Count == 0)
				{
					return;
				}
				int partId = -1;
				if (!string.IsNullOrWhiteSpace(_supplierContractContainer.PartnerCode.Code))
				{
					PartnerCacheRow item = TradeSettings.TradeCacheService.Partner.GetItem(_supplierContractContainer.PartnerCode.Code);
					if (item != null)
					{
						partId = item.Id;
					}
				}
				foreach (MTRetToSupSpecificationRow mtRetToSup in _mtRetToSupList)
				{
					if (mtRetToSup.MTID != -1)
					{
						MaterialIncomeDiscountPrices mTIncomePriceAndDiscountPrice = TradeSettings.CreateTradeClient().CommonServices.GetMTIncomePriceAndDiscountPrice(mtRetToSup.MTID, mtRetToSup.UnitCode, PO.ISN, _currCode.Code, partId, _date.EditValue.Value);
						if (mTIncomePriceAndDiscountPrice.MaterialHasPrice)
						{
							mtRetToSup.Price = mTIncomePriceAndDiscountPrice.Price;
							mtRetToSup.DiscountPrice = mTIncomePriceAndDiscountPrice.DiscountPrice;
							mtRetToSup.Summa = (mTIncomePriceAndDiscountPrice.DiscountPrice * mtRetToSup.Quantity).RoundAs(2);
							mtRetToSup.UnitBrief = TradeSettings.TradeCacheService.QntUnit.GetItem(mtRetToSup.UnitCode).Brief;
							mtRetToSup.ValidateWithoutResponse();
						}
					}
				}
				_mtRetToSupGrid.Grid.RefreshData();
				_mtRetToSupGrid.ConfigureButtons();
			}
			else
			{
				ControlsHelper.SetFocus(_date);
				ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Common.Resources.ValidateEmptyDate, Settings.ProgramName, MessageBoxButton.OK, MessageBoxImage.Exclamation);
			}
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	private void ChangeRowInfo(MTRetToSupSpecificationRow Row = null)
	{
		if (Row == null)
		{
			((DispatcherObject)this).Dispatcher.BeginInvoke((Delegate)new DelegateClearInfoPanel(CleanInfoPanel), Array.Empty<object>());
			return;
		}
		((DispatcherObject)this).Dispatcher.BeginInvoke((Delegate)new DelegateSetRowInfoToInfoPanel(SetRowInfoToPanel), new object[1] { Row });
	}

	private void SetRowInfoToPanel(MTRetToSupSpecificationRow row)
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

	private void SetAvailabilityInfoToPanel(MTRetToSupSpecificationRow row)
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

	protected override bool AllowChangeGridMode()
	{
		return true;
	}

	protected override bool AllowPrint()
	{
		return true;
	}

	protected override bool AllowPrintOnNew()
	{
		return true;
	}
}
