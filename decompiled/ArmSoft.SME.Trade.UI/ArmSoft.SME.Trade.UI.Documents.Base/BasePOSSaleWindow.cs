using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Windows;
using System.Windows.Threading;
using ArmSoft.ECRConnector;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.CustomTypes;
using ArmSoft.SME.Common.Enums;
using ArmSoft.SME.Common.Parameters;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Core.UI.Controls;
using ArmSoft.SME.Core.UI.Documents;
using ArmSoft.SME.Trade.Common;
using ArmSoft.SME.Trade.Common.Models.Caches;
using ArmSoft.SME.Trade.Common.Models.Documents;
using ArmSoft.SME.Trade.Common.Parameters;
using ArmSoft.SME.Trade.Common.SmartEnums;
using ArmSoft.SME.Trade.UI.CustomUIElements.UserControls;
using ArmSoft.SME.Trade.UI.Helpers;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.ComponentModel.__Internals;
using DevExpress.XtraEditors.DXErrorProvider;

namespace ArmSoft.SME.Trade.UI.Documents.Base;

public class BasePOSSaleWindow : Window
{
	public class SaleRow : DocumentGridRow, INotifyPropertyChanged, ISaleRow
	{
		[ObservableProperty]
		protected string itemCode = "";

		[ObservableProperty]
		protected string unitCode = "";

		[ObservableProperty]
		protected decimal quantity;

		[ObservableProperty]
		protected decimal price;

		[ObservableProperty]
		protected decimal discountPrice;

		[ObservableProperty]
		protected decimal summa;

		[ObservableProperty]
		protected bool isDeleted;

		[ObservableProperty]
		protected bool isGift;

		[ObservableProperty]
		protected bool isRowMarkable;

		[ObservableProperty]
		protected string markingStatus;

		[ObservableProperty]
		protected bool markingHasMismatch;

		public string ItemType { get; set; } = "";

		public string Storage { get; set; } = "";

		public decimal DiscountSumma { get; set; }

		public decimal BeginSumma { get; set; }

		public string BarCode { get; set; } = "";

		public bool? VAT { get; set; }

		public int RowID { get; set; }

		public bool? IsPartySelected { get; set; }

		public bool IsStorageRequired { get; set; }

		public string RowDescription { get; set; } = "";

		public int ItemID { get; set; } = -1;

		public string ItemName { get; set; } = "";

		public string UnitBrief { get; set; } = "";

		public decimal Discount { get; set; }

		public decimal UsedBonus { get; set; }

		public decimal AccumulatedBonus { get; set; }

		public HashSet<string> MarkingList { get; set; } = new HashSet<string>();

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
		public bool IsDeleted
		{
			get
			{
				return isDeleted;
			}
			set
			{
				if (!EqualityComparer<bool>.Default.Equals(isDeleted, value))
				{
					OnPropertyChanging(__KnownINotifyPropertyChangingArgs.IsDeleted);
					isDeleted = value;
					OnIsDeletedChanged(value);
					OnPropertyChanged(__KnownINotifyPropertyChangedArgs.IsDeleted);
				}
			}
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public bool IsGift
		{
			get
			{
				return isGift;
			}
			set
			{
				if (!EqualityComparer<bool>.Default.Equals(isGift, value))
				{
					OnPropertyChanging(__KnownINotifyPropertyChangingArgs.IsGift);
					isGift = value;
					OnPropertyChanged(__KnownINotifyPropertyChangedArgs.IsGift);
				}
			}
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public bool IsRowMarkable
		{
			get
			{
				return isRowMarkable;
			}
			set
			{
				if (!EqualityComparer<bool>.Default.Equals(isRowMarkable, value))
				{
					OnPropertyChanging(__KnownINotifyPropertyChangingArgs.IsRowMarkable);
					isRowMarkable = value;
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
				return markingStatus;
			}
			[MemberNotNull("markingStatus")]
			set
			{
				if (!EqualityComparer<string>.Default.Equals(markingStatus, value))
				{
					OnPropertyChanging(__KnownINotifyPropertyChangingArgs.MarkingStatus);
					markingStatus = value;
					OnMarkingStatusChanged(value);
					OnPropertyChanged(__KnownINotifyPropertyChangedArgs.MarkingStatus);
				}
			}
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public bool MarkingHasMismatch
		{
			get
			{
				return markingHasMismatch;
			}
			set
			{
				if (!EqualityComparer<bool>.Default.Equals(markingHasMismatch, value))
				{
					OnPropertyChanging(__KnownINotifyPropertyChangingArgs.MarkingHasMismatch);
					markingHasMismatch = value;
					OnPropertyChanged(__KnownINotifyPropertyChangedArgs.MarkingHasMismatch);
				}
			}
		}

		static SaleRow()
		{
			AddPropertyValidators();
		}

		protected static void ValidateItemCode(SaleRow row, ErrorInfo info)
		{
			PropertyValidator.EmptyStringValidate(row.ItemCode, info, ArmSoft.SME.Common.Resources.Code);
			if (info.ErrorType != ErrorType.Critical)
			{
				if (row.ItemType == ItemTypesSE.Goods.Value)
				{
					TradePropertyValidator.ValidateGoodCode(row.ItemCode, info, ArmSoft.SME.Common.Resources.Code);
				}
				else
				{
					TradePropertyValidator.ValidateGoodCode(row.ItemCode, info, ArmSoft.SME.Common.Resources.Code, verifyingOnlyGoods: false, verifyingOnlyService: true);
				}
			}
		}

		protected static void ValidateQuantity(SaleRow row, ErrorInfo info)
		{
			if (row.IsDeleted)
			{
				PropertyValidator.NonNegativeValidate(row.Quantity, info, ArmSoft.SME.Common.Resources.Quantity);
			}
			else
			{
				PropertyValidator.PositiveValidate(row.Quantity, info, ArmSoft.SME.Common.Resources.Quantity);
			}
		}

		protected static void ValidateUnitCode(SaleRow row, ErrorInfo info)
		{
			PropertyValidator.EmptyStringValidate(row.UnitCode, info, ArmSoft.SME.Common.Resources.Unit);
			if (info.ErrorType != ErrorType.Critical)
			{
				TradePropertyValidator.ValidateGoodUnitCode(row.UnitCode, row.ItemCode, info, ArmSoft.SME.Common.Resources.Unit);
			}
		}

		protected static void ValidatePrice(SaleRow row, ErrorInfo info)
		{
			if (row.IsDeleted || row.IsGift)
			{
				PropertyValidator.NonNegativeValidate(row.Price, info, ArmSoft.SME.Common.Resources.Price);
			}
			else
			{
				PropertyValidator.PositiveValidate(row.Price, info, ArmSoft.SME.Common.Resources.Price);
			}
		}

		protected static void ValidateDiscountPrice(SaleRow row, ErrorInfo info)
		{
			PropertyValidator.NonNegativeValidate(row.DiscountPrice, info, ArmSoft.SME.Common.Resources.DiscountedPrice);
		}

		protected static void ValidateSumma(SaleRow row, ErrorInfo info)
		{
			if (row.IsDeleted || row.Discount == 100m || row.IsGift)
			{
				PropertyValidator.NonNegativeValidate(row.Summa, info, ArmSoft.SME.Common.Resources.Summa);
			}
			else
			{
				PropertyValidator.PositiveValidate(row.Summa, info, ArmSoft.SME.Common.Resources.Summa);
			}
		}

		protected static void ValidateMarkingListCount(SaleRow row, ErrorInfo info)
		{
			row.MarkingHasMismatch = false;
			MaterialCacheRow item = TradeSettings.TradeCacheService.Material.GetItem(row.ItemCode);
			if (row.IsRowMarkable && item != null && !item.IsWeight && (decimal)row.MarkingList.Count != row.Quantity)
			{
				if ((decimal)row.MarkingList.Count > row.Quantity)
				{
					info.ErrorType = ErrorType.Critical;
					info.ErrorText = ArmSoft.SME.Common.Resources.GreaterMarkingListCount;
				}
				if (item.CheckMarking)
				{
					row.MarkingHasMismatch = true;
				}
			}
		}

		protected static void AddPropertyValidators()
		{
			Dictionary<string, Action<DocumentGridRow, ErrorInfo>> dictionary = new Dictionary<string, Action<DocumentGridRow, ErrorInfo>>
			{
				{
					"ItemCode",
					(DocumentGridRow row, ErrorInfo info) =>
					{
						ValidateItemCode((SaleRow)row, info);
					}
				},
				{
					"Quantity",
					(DocumentGridRow row, ErrorInfo info) =>
					{
						ValidateQuantity((SaleRow)row, info);
					}
				},
				{
					"UnitCode",
					(DocumentGridRow row, ErrorInfo info) =>
					{
						ValidateUnitCode((SaleRow)row, info);
					}
				},
				{
					"Price",
					(DocumentGridRow row, ErrorInfo info) =>
					{
						ValidatePrice((SaleRow)row, info);
					}
				},
				{
					"DiscountPrice",
					(DocumentGridRow row, ErrorInfo info) =>
					{
						ValidateDiscountPrice((SaleRow)row, info);
					}
				},
				{
					"Summa",
					(DocumentGridRow row, ErrorInfo info) =>
					{
						ValidateSumma((SaleRow)row, info);
					}
				}
			};
			if (Settings.ParametersService.UseMarking())
			{
				dictionary.Add("MarkingList", (DocumentGridRow row, ErrorInfo info) =>
				{
					ValidateMarkingListCount((SaleRow)row, info);
				});
			}
			DocumentGridRow.RegisterPropertyValidators(typeof(SaleRow), cacheValidationResults: true, dictionary);
		}

		public SaleRow()
		{
		}

		public SaleRow(MTInvoiceBase.MTInvoiceSpecificationRow mTInvoiceSpecificationRow)
		{
			ItemType = mTInvoiceSpecificationRow.ItemType;
			Storage = mTInvoiceSpecificationRow.Storage;
			ItemCode = mTInvoiceSpecificationRow.ItemCode;
			ItemID = mTInvoiceSpecificationRow.ItemID;
			ItemName = mTInvoiceSpecificationRow.ItemName;
			UnitCode = mTInvoiceSpecificationRow.UnitCode;
			UnitBrief = mTInvoiceSpecificationRow.UnitBrief;
			Quantity = mTInvoiceSpecificationRow.Quantity;
			Price = mTInvoiceSpecificationRow.Price;
			Discount = mTInvoiceSpecificationRow.Discount;
			DiscountPrice = mTInvoiceSpecificationRow.DiscountPrice;
			UsedBonus = mTInvoiceSpecificationRow.UsedBonus;
			AccumulatedBonus = mTInvoiceSpecificationRow.AccumulatedBonus;
			Summa = mTInvoiceSpecificationRow.Summa;
			DiscountSumma = mTInvoiceSpecificationRow.DiscountSumma;
			BeginSumma = mTInvoiceSpecificationRow.BeginSumma;
			BarCode = mTInvoiceSpecificationRow.BarCode;
			VAT = mTInvoiceSpecificationRow.VAT.Value;
			IsStorageRequired = mTInvoiceSpecificationRow.IsStorageRequired;
			RowID = mTInvoiceSpecificationRow.RowID;
			IsPartySelected = mTInvoiceSpecificationRow.IsPartySelected;
			RowDescription = mTInvoiceSpecificationRow.RowDescription;
			IsDeleted = mTInvoiceSpecificationRow.IsDeleted;
			IsGift = mTInvoiceSpecificationRow.IsGift;
			MarkingList = mTInvoiceSpecificationRow.MarkingList;
			IsRowMarkable = mTInvoiceSpecificationRow.IsRowMarkable;
		}

		public SaleRow(MTReturnFromCustomerBase.MTRetFromCustSpecificationRow mTRetFromCustSpecificationRow)
		{
			Storage = mTRetFromCustSpecificationRow.Storage;
			UnitCode = mTRetFromCustSpecificationRow.UnitCode;
			UnitBrief = mTRetFromCustSpecificationRow.UnitBrief;
			ItemType = mTRetFromCustSpecificationRow.ItemType;
			ItemID = mTRetFromCustSpecificationRow.ItemID.Value;
			ItemCode = mTRetFromCustSpecificationRow.ItemCode;
			ItemName = mTRetFromCustSpecificationRow.ItemName;
			UnitCode = mTRetFromCustSpecificationRow.UnitCode;
			UnitBrief = mTRetFromCustSpecificationRow.UnitBrief;
			Quantity = mTRetFromCustSpecificationRow.Quantity.Value;
			Discount = mTRetFromCustSpecificationRow.Discount.Value;
			DiscountPrice = mTRetFromCustSpecificationRow.Price.Value;
			DiscountSumma = mTRetFromCustSpecificationRow.DiscountSumma.Value;
			Price = mTRetFromCustSpecificationRow.BeginPrice.Value;
			Summa = mTRetFromCustSpecificationRow.Summa.Value;
			BeginSumma = mTRetFromCustSpecificationRow.BeginSumma.Value;
			IsDeleted = mTRetFromCustSpecificationRow.IsDeleted.Value;
			IsGift = mTRetFromCustSpecificationRow.IsGift.Value;
			IsStorageRequired = mTRetFromCustSpecificationRow.IsStorageRequired.Value;
			AccumulatedBonus = mTRetFromCustSpecificationRow.AccumulatedBonus.Value;
			UsedBonus = mTRetFromCustSpecificationRow.UsedBonus.Value;
			RowDescription = mTRetFromCustSpecificationRow.RowDescription;
			BarCode = mTRetFromCustSpecificationRow.BarCode;
			VAT = mTRetFromCustSpecificationRow.VAT.Value;
			RowID = mTRetFromCustSpecificationRow.RowID.Value;
			MarkingList = mTRetFromCustSpecificationRow.MarkingList;
			IsRowMarkable = mTRetFromCustSpecificationRow.IsRowMarkable;
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		private void OnItemCodeChanged(string value)
		{
			ResetForceValidationBit("ItemCode", "ItemCode");
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		private void OnUnitCodeChanged(string value)
		{
			ResetForceValidationBit("UnitCode", "UnitCode");
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		private void OnQuantityChanged(decimal value)
		{
			ResetForceValidationBit("Quantity", "Quantity");
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		private void OnPriceChanged(decimal value)
		{
			ResetForceValidationBit("Price");
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		private void OnDiscountPriceChanged(decimal value)
		{
			ResetForceValidationBit("DiscountPrice");
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		private void OnSummaChanged(decimal value)
		{
			ResetForceValidationBit("Summa");
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		private void OnIsDeletedChanged(bool value)
		{
			ResetForceValidationBit("IsDeleted", "Quantity", "Price", "Summa");
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		private void OnMarkingStatusChanged(string value)
		{
			ResetForceValidationBit("MarkingList");
		}
	}

	protected BindingList<SaleRow> _materialsRowsList;

	protected TradeDocument _lastAutoSavedDocument;

	protected int _columnMainSpacing = 6;

	protected int _rowMainSpacing = 6;

	protected bool _isFirstActivate;

	protected int _maxRowID;

	protected DocNumOfferSource _docNumOfferSource = DocNumOfferSource.FromParam;

	protected bool _docIsSale = true;

	protected DocumentTypeSE _docType;

	protected string _vatType = "";

	protected string _ecrCheckNum = "";

	protected string _currCode = Settings.ParametersService.DefaultMTBillCurr();

	protected ExchangeRate _exchangeRate = new ExchangeRate();

	protected DiscountsCalculator _discountsCalculator;

	protected ExchangeRate _discountPriceListExchangeRate = new ExchangeRate();

	protected string _defaultStorage = Settings.ParametersService.DefaultStorage();

	protected string _defaultCash = Settings.ParametersService.DefaultCash();

	protected string _defaultSalesConsultant = Settings.ParametersService.DefaultSalesConsultant();

	protected short _nonCashPaymentMethod;

	protected short _nonCashPaymentMethodForECRPOS;

	protected string _nonCashPaymentId;

	protected bool _paymentAccepted;

	protected string _posTerminalType = "0";

	protected string _ecrCRN = "";

	protected string _prePaymentECRCheckNum = "";

	protected string _bankAcquiringPartner = "";

	protected string _bankAcquiringContract = "";

	protected bool _nonCashPaymentMethodSelected;

	protected decimal _saleTotalAmmount;

	protected decimal _temporaryDiscountSumm;

	protected string _priceListType = Settings.ParametersService.DefaultMTCheckPriceType();

	protected string _priceListTypeRoundDgr = "";

	protected string _discountPriceListType = Settings.ParametersService.DefaultMTCheckDiscountPriceType();

	protected string _discountPriceListVATType = "";

	protected string _discountPriceListCurr = "";

	protected string _discountPriceTypeRoundDegree = "";

	protected bool _isRetail = SaleWindowHelper.IsDefaultStorageRetail();

	protected decimal _qty_Weight;

	protected bool _immediatlyOpenMarkingWindow = Settings.ParametersService.OpenMarkingWindowImmediatly();

	protected readonly bool _useMarking = Settings.ParametersService.UseMarking();

	protected string _realText = "";

	protected bool _saveAndPrintECR = Settings.ParametersService.SaveAndPrintECR();

	protected string _contract = "";

	protected int _partnerID = -1;

	protected string _partnerCode = "";

	protected bool _isPartBonusCardValid;

	protected string _saleConsultantCode = "";

	protected string _currentShiftNum = "";

	protected DateTime? _shiftBeginDate;

	protected ECR _ecr;

	protected decimal _sumFromRound;

	protected Guid _deferredISN;

	protected Guid _baseDocISN;

	protected string _baseDocCash = "";

	protected string _partnerTaxCode;

	protected bool _isDeferred;

	protected bool _shiftAlreadyClosed;

	protected Dictionary<string, SaleWindowHelper.ButtonCommands> _buttonCommandByKeyGesturesDictionary = new Dictionary<string, SaleWindowHelper.ButtonCommands>();

	protected Dictionary<(int, string), decimal> _availabilityByGoodIdsDictionary = new Dictionary<(int, string), decimal>();

	protected decimal _summaCashValue;

	protected decimal _summaPlCardValue;

	protected MTBill _clonedBill;

	protected MTBillReturn _clonedBillReturn;

	protected DispatcherTimer _timer;

	protected DispatcherTimer _autoSaveTimer;

	protected bool _connectTob2barmsoftDiscountServer = Settings.ParametersService.ConnectTob2barmsoftDiscountServer();

	public string _prepaymentPartnerCode = "";

	public string _prepaymentContractCode = "";

	public string TouchMode { get; protected set; }

	public bool FullScreen { get; protected set; }

	public bool PaymentRefundAccepted { get; set; }

	protected bool ValidateIsCashierShiftOpen()
	{
		DateTime? dateTime = TradeSettings.TradeCacheService.CashierShift.GetItem(new CashierShiftCacheRow.RowKey(_defaultCash.Trim(), _currentShiftNum))?.EndDate;
		try
		{
			if (dateTime.HasValue && !Settings.IsOffline)
			{
				throw new ASException(ArmSoft.SME.Trade.Common.Resources.ShiftAlreadyClosed);
			}
			return true;
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
			_shiftAlreadyClosed = true;
			Close();
			return false;
		}
	}

	protected static string GetCardNumberWithoutPrefixSuffix(string cardData, bool byPrefixSuffix = true)
	{
		try
		{
			if (byPrefixSuffix)
			{
				if (MCRInfo.GetInstance().IsValidCard(cardData))
				{
					return MCRInfo.GetInstance().GetCardNumber(cardData);
				}
				throw new ASException(ArmSoft.SME.Trade.Common.Resources.InvalidCardCode);
			}
			return cardData;
		}
		catch
		{
			return "";
		}
	}

	protected void StopAutoSaveTimer()
	{
		if (_autoSaveTimer != null)
		{
			_autoSaveTimer.Stop();
		}
	}

	protected void StartAutoSaveTimer()
	{
		if (Settings.ParametersService.AutoSave() && _autoSaveTimer != null)
		{
			_autoSaveTimer.Start();
		}
	}

	protected void StopDocumentDateTimer()
	{
		if (_timer != null)
		{
			_timer.Stop();
		}
	}
}
