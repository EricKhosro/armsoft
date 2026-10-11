using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Extensions;
using ArmSoft.SME.Common.PersistableObject;
using ArmSoft.SME.Core.UI.Controls;
using ArmSoft.SME.Core.UI.Documents;
using ArmSoft.SME.Trade.Common;
using ArmSoft.SME.Trade.Common.Documents.Definition;
using ArmSoft.SME.Trade.Common.Models.Caches;
using ArmSoft.SME.Trade.Common.Models.Documents;
using ArmSoft.SME.Trade.Common.SmartEnums;
using ArmSoft.SME.Trade.UI.CustomUIElements;
using ArmSoft.SME.Trade.UI.Helpers;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.ComponentModel.__Internals;
using DevExpress.Data.Extensions;
using DevExpress.XtraEditors.DXErrorProvider;

namespace ArmSoft.SME.Trade.UI.Documents.Goods;

public abstract class MTInvoiceBaseWindow<T> : TradeDocumentWindow<T> where T : MTInvoiceBase, new()
{
	public class MTInvoiceBaseSpecificationRow : DocumentGridRow, ISaleRow
	{
		[ObservableProperty]
		private int rowID;

		[ObservableProperty]
		private short rowNum;

		[ObservableProperty]
		private int itemID = -1;

		[ObservableProperty]
		private string itemType = "";

		[ObservableProperty]
		private bool isStorageRequired;

		[ObservableProperty]
		private string storage = "";

		[ObservableProperty]
		private string itemCode = "";

		[ObservableProperty]
		private string itemName = "";

		[ObservableProperty]
		private string unitCode = "";

		[ObservableProperty]
		private string unitBrief = "";

		[ObservableProperty]
		private decimal quantity;

		[ObservableProperty]
		private decimal price;

		[ObservableProperty]
		private decimal discount;

		[ObservableProperty]
		private decimal discountPrice;

		[ObservableProperty]
		private decimal summa;

		[ObservableProperty]
		private decimal discountSumma;

		[ObservableProperty]
		private bool? vAT = false;

		[ObservableProperty]
		private string dealType = "";

		[ObservableProperty]
		private bool? isPartySelected;

		[ObservableProperty]
		private string barCode = "";

		[ObservableProperty]
		private decimal beginSumma;

		[ObservableProperty]
		private decimal accumulatedBonus;

		[ObservableProperty]
		private decimal usedBonus;

		[ObservableProperty]
		private string rowDescription = "";

		[ObservableProperty]
		private bool isDeleted;

		[ObservableProperty]
		private bool isGift;

		[ObservableProperty]
		private HashSet<string> markingList = new HashSet<string>();

		[ObservableProperty]
		private bool isRowMarkable;

		[ObservableProperty]
		private string markingStatus = "";

		[ObservableProperty]
		private Guid _rowISN;

		public bool UpdateRemainsOnInfoPanel = true;

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
					OnItemTypeChanged(value);
					OnPropertyChanged(__KnownINotifyPropertyChangedArgs.ItemType);
				}
			}
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public bool IsStorageRequired
		{
			get
			{
				return isStorageRequired;
			}
			set
			{
				if (!EqualityComparer<bool>.Default.Equals(isStorageRequired, value))
				{
					OnPropertyChanging(__KnownINotifyPropertyChangingArgs.IsStorageRequired);
					isStorageRequired = value;
					OnPropertyChanged(__KnownINotifyPropertyChangedArgs.IsStorageRequired);
				}
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
					OnPropertyChanged(__KnownINotifyPropertyChangedArgs.ItemName);
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
					OnDiscountChanged(value);
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
		public decimal DiscountSumma
		{
			get
			{
				return discountSumma;
			}
			set
			{
				if (!EqualityComparer<decimal>.Default.Equals(discountSumma, value))
				{
					OnPropertyChanging(__KnownINotifyPropertyChangingArgs.DiscountSumma);
					discountSumma = value;
					OnPropertyChanged(__KnownINotifyPropertyChangedArgs.DiscountSumma);
				}
			}
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public bool? VAT
		{
			get
			{
				return vAT;
			}
			set
			{
				if (!EqualityComparer<bool?>.Default.Equals(vAT, value))
				{
					OnPropertyChanging(__KnownINotifyPropertyChangingArgs.VAT);
					vAT = value;
					OnPropertyChanged(__KnownINotifyPropertyChangedArgs.VAT);
				}
			}
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public string DealType
		{
			get
			{
				return dealType;
			}
			[MemberNotNull("dealType")]
			set
			{
				if (!EqualityComparer<string>.Default.Equals(dealType, value))
				{
					OnPropertyChanging(__KnownINotifyPropertyChangingArgs.DealType);
					dealType = value;
					OnPropertyChanged(__KnownINotifyPropertyChangedArgs.DealType);
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

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public string BarCode
		{
			get
			{
				return barCode;
			}
			[MemberNotNull("barCode")]
			set
			{
				if (!EqualityComparer<string>.Default.Equals(barCode, value))
				{
					OnPropertyChanging(__KnownINotifyPropertyChangingArgs.BarCode);
					barCode = value;
					OnPropertyChanged(__KnownINotifyPropertyChangedArgs.BarCode);
				}
			}
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public decimal BeginSumma
		{
			get
			{
				return beginSumma;
			}
			set
			{
				if (!EqualityComparer<decimal>.Default.Equals(beginSumma, value))
				{
					OnPropertyChanging(__KnownINotifyPropertyChangingArgs.BeginSumma);
					beginSumma = value;
					OnPropertyChanged(__KnownINotifyPropertyChangedArgs.BeginSumma);
				}
			}
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public decimal AccumulatedBonus
		{
			get
			{
				return accumulatedBonus;
			}
			set
			{
				if (!EqualityComparer<decimal>.Default.Equals(accumulatedBonus, value))
				{
					OnPropertyChanging(__KnownINotifyPropertyChangingArgs.AccumulatedBonus);
					accumulatedBonus = value;
					OnPropertyChanged(__KnownINotifyPropertyChangedArgs.AccumulatedBonus);
				}
			}
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public decimal UsedBonus
		{
			get
			{
				return usedBonus;
			}
			set
			{
				if (!EqualityComparer<decimal>.Default.Equals(usedBonus, value))
				{
					OnPropertyChanging(__KnownINotifyPropertyChangingArgs.UsedBonus);
					usedBonus = value;
					OnPropertyChanged(__KnownINotifyPropertyChangedArgs.UsedBonus);
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
		public HashSet<string> MarkingList
		{
			get
			{
				return markingList;
			}
			[MemberNotNull("markingList")]
			set
			{
				if (!EqualityComparer<HashSet<string>>.Default.Equals(markingList, value))
				{
					OnPropertyChanging(__KnownINotifyPropertyChangingArgs.MarkingList);
					markingList = value;
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
		public Guid RowISN
		{
			get
			{
				return _rowISN;
			}
			set
			{
				if (!EqualityComparer<Guid>.Default.Equals(_rowISN, value))
				{
					OnPropertyChanging(__KnownINotifyPropertyChangingArgs.RowISN);
					_rowISN = value;
					OnPropertyChanged(__KnownINotifyPropertyChangedArgs.RowISN);
				}
			}
		}

		public MTInvoiceBaseSpecificationRow()
		{
		}

		public MTInvoiceBaseSpecificationRow(TradeDocumentWindow<T> docWindow)
			: base(docWindow)
		{
		}

		static MTInvoiceBaseSpecificationRow()
		{
			DocumentGridRow.RegisterPropertyValidators(typeof(MTInvoiceBaseSpecificationRow), cacheValidationResults: true, GetInvoiceBasePropertyValidators());
		}

		protected static Dictionary<string, Action<DocumentGridRow, ErrorInfo>> GetInvoiceBasePropertyValidators()
		{
			return new Dictionary<string, Action<DocumentGridRow, ErrorInfo>>
			{
				{
					"ItemCode",
					(DocumentGridRow row, ErrorInfo info) =>
					{
						ValidateGoodCode((MTInvoiceBaseSpecificationRow)row, info);
					}
				},
				{
					"UnitCode",
					(DocumentGridRow row, ErrorInfo info) =>
					{
						ValidateUnitCode((MTInvoiceBaseSpecificationRow)row, info);
					}
				},
				{
					"Quantity",
					(DocumentGridRow row, ErrorInfo info) =>
					{
						ValidateQuantity((MTInvoiceBaseSpecificationRow)row, info);
					}
				},
				{
					"Price",
					(DocumentGridRow row, ErrorInfo info) =>
					{
						ValidatePrice((MTInvoiceBaseSpecificationRow)row, info);
					}
				},
				{
					"DiscountPrice",
					(DocumentGridRow row, ErrorInfo info) =>
					{
						ValidateDiscountPrice((MTInvoiceBaseSpecificationRow)row, info);
					}
				},
				{
					"Summa",
					(DocumentGridRow row, ErrorInfo info) =>
					{
						ValidateSumma((MTInvoiceBaseSpecificationRow)row, info);
					}
				}
			};
		}

		private static void ValidateGoodCode(MTInvoiceBaseSpecificationRow row, ErrorInfo info)
		{
			TradePropertyValidator.ValidateGoodCode(row.ItemCode, info, (StringFieldDefinition)InvoiceBase.Grid.Columns["ItemCode"], verifyingOnlyGoods: false);
		}

		private static void ValidateUnitCode(MTInvoiceBaseSpecificationRow row, ErrorInfo info)
		{
			TradePropertyValidator.ValidateGoodUnitCode(row.UnitCode, row.ItemCode, info, InvoiceBase.Grid.Columns["UnitCode"]);
		}

		private static void ValidateQuantity(MTInvoiceBaseSpecificationRow row, ErrorInfo info)
		{
			if (!row.IsDeleted)
			{
				PropertyValidator.PositiveValidate(row.Quantity, info, ArmSoft.SME.Common.Resources.Quantity);
			}
			else
			{
				PropertyValidator.NonNegativeValidate(row.Quantity, info, ArmSoft.SME.Common.Resources.Quantity);
			}
		}

		private static void ValidatePrice(MTInvoiceBaseSpecificationRow row, ErrorInfo info)
		{
			if (!row.IsDeleted && !row.IsGift)
			{
				PropertyValidator.PositiveValidate(row.Price, info, ArmSoft.SME.Common.Resources.Price);
			}
			else
			{
				PropertyValidator.NonNegativeValidate(row.Price, info, ArmSoft.SME.Common.Resources.Price);
			}
		}

		private static void ValidateDiscountPrice(MTInvoiceBaseSpecificationRow row, ErrorInfo info)
		{
			PropertyValidator.NonNegativeValidate(row.DiscountPrice, info, ArmSoft.SME.Common.Resources.DiscountedPrice);
		}

		private static void ValidateSumma(MTInvoiceBaseSpecificationRow row, ErrorInfo info)
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

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		private void OnItemTypeChanged(string value)
		{
			ResetForceValidationBit("ItemType", "Storage");
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		private void OnStorageChanged(string value)
		{
			ResetForceValidationBit("Storage");
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		private void OnItemCodeChanged(string value)
		{
			ResetForceValidationBit("ItemCode", "UnitCode");
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		private void OnUnitCodeChanged(string value)
		{
			ResetForceValidationBit("UnitCode");
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		private void OnQuantityChanged(decimal value)
		{
			ResetForceValidationBit("Quantity", "MarkingStatus");
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		private void OnPriceChanged(decimal value)
		{
			ResetForceValidationBit("Price");
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		private void OnDiscountChanged(decimal value)
		{
			ResetForceValidationBit("Discount", "Summa");
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
			ResetForceValidationBit("MarkingStatus");
		}
	}

	public class MTInvoiceBaseFilteredSearchList<TRow> : IResultList where TRow : MTInvoiceBaseSpecificationRow, new()
	{
		private readonly string _currCode;

		private readonly string _vatType;

		private readonly DateOnly _docDate;

		private readonly bool _showSalePrice;

		private readonly string _priceListType;

		private readonly string _pricePrecision;

		private readonly bool _allowChangeSalePrice;

		private readonly ObservableCollection<TRow> _itemSource = new ObservableCollection<TRow>();

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
					Header = InvoiceBase.Grid.Columns["ItemCode"].Caption,
					FieldName = "ItemCode",
					CustomWidth = 125.0
				};
				span[1] = new FilteredSearchGridColumn
				{
					Header = InvoiceBase.Grid.Columns["ItemName"].Caption,
					FieldName = "ItemName",
					CustomWidth = 300.0
				};
				span[2] = new FilteredSearchGridColumn
				{
					Header = ArmSoft.SME.Common.Resources.Quantity,
					FieldName = "Quantity",
					AllowEditing = true,
					NumericFormat = DecimalExtension.GetCurrencyFormat(13, 3)
				};
				List<FilteredSearchGridColumn> list2 = list;
				if (_showSalePrice)
				{
					list2.Add(new FilteredSearchGridColumn
					{
						Header = ((string.IsNullOrWhiteSpace(_currCode) || _currCode == "AMD") ? ArmSoft.SME.Common.Resources.Price : ArmSoft.SME.Common.Resources.FCPrice),
						FieldName = "Price",
						AllowEditing = _allowChangeSalePrice,
						ShowTotalSummary = false,
						NumericFormat = DecimalExtension.GetCurrencyFormat(16, 2)
					});
					list2.Add(new FilteredSearchGridColumn
					{
						Header = ArmSoft.SME.Common.Resources.Summa,
						FieldName = "Summa",
						AllowEditing = false,
						NumericFormat = DecimalExtension.GetCurrencyFormat(16, 2)
					});
				}
				return list2;
			}
		}

		public IEnumerable<DocumentGridRow> ItemsSource => _itemSource;

		public MTInvoiceBaseFilteredSearchList(string currCode, string vatType, DateOnly docDate, bool showSalePrice = false, string priceListType = "", string pricePrecision = "", bool allowChangeSalePrice = false)
		{
			_currCode = currCode;
			_vatType = vatType;
			_docDate = docDate;
			_showSalePrice = showSalePrice;
			_priceListType = priceListType;
			_pricePrecision = pricePrecision;
			_allowChangeSalePrice = allowChangeSalePrice;
		}

		public int AddToItemsSource(object key, params object[] args)
		{
			string itemCode = (string)key;
			decimal num = (decimal)args[0];
			int num2 = _itemSource.FindIndex((TRow x) => x.ItemCode == itemCode);
			if (num2 == -1)
			{
				MaterialCacheRow item = TradeSettings.TradeCacheService.Material.GetItem(itemCode);
				if (item == null)
				{
					return -1;
				}
				TRow val = new TRow
				{
					ItemType = item.Type,
					ItemID = item.Id,
					ItemCode = item.MTCode,
					ItemName = item.Caption,
					BarCode = item.BarCode,
					UnitCode = item.Unit,
					UnitBrief = item.UnitRow.Brief,
					Discount = item.Discount,
					VAT = item.VAT
				};
				if (val.ItemType == GoodsTypesSE.Good.Value)
				{
					val.IsPartySelected = false;
				}
				if (_showSalePrice)
				{
					val.Price = PriceHelper.GetPriceForGivenPriceListAndVATType(_priceListType, _vatType, val.ItemID, val.UnitCode, _docDate, _currCode);
					val.Price = Methods.RoundPrice(val.Price, _pricePrecision);
				}
				_itemSource.Add(val);
				num2 = _itemSource.Count - 1;
			}
			TRow val2 = _itemSource[num2];
			decimal quantity = val2.Quantity + num;
			val2.Quantity = quantity;
			_itemSource[num2].Summa = (_itemSource[num2].Quantity * _itemSource[num2].Price).RoundAs(2);
			return num2;
		}
	}

	protected override double WidthValue => 1090.0;

	protected override double HeightValue => 500.0;

	protected override double MinWidthValue => 620.0;

	protected override double MinHeightValue => 250.0;

	protected MTInvoiceBaseWindow()
	{
		InnerInitialize();
	}

	protected MTInvoiceBaseWindow(T doc)
		: base(doc)
	{
		InnerInitialize();
	}

	protected virtual void InnerInitialize()
	{
	}

	protected void CalculateDiscount(IEnumerable<MTInvoiceBaseSpecificationRow> mtList, decimal discount, bool gDiscount = true, bool calcInvFee = false, string vatType = "")
	{
		if (gDiscount)
		{
			foreach (MTInvoiceBaseSpecificationRow mt in mtList)
			{
				mt.Discount = discount;
				CalculateDiscount(mt, calcInvFee, vatType);
			}
			return;
		}
		foreach (MTInvoiceBaseSpecificationRow mt2 in mtList)
		{
			CalculateDiscount(mt2, calcInvFee, vatType);
		}
	}

	protected virtual void CalculateDiscount(MTInvoiceBaseSpecificationRow row, bool calcInvFee, string vatType)
	{
		row.DiscountPrice = (row.Price * (1m - row.Discount / 100m)).RoundAs(2);
		row.Summa = (row.Quantity * row.DiscountPrice).RoundAs(2);
		row.DiscountSumma = (row.Quantity * row.Price - row.Summa).RoundAs(2);
	}
}
