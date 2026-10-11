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
using ArmSoft.Cloud.Client.Discount;
using ArmSoft.ECRConnector;
using ArmSoft.SME.Client.Data;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Constants;
using ArmSoft.SME.Common.CustomTypes;
using ArmSoft.SME.Common.Extensions;
using ArmSoft.SME.Common.Models;
using ArmSoft.SME.Common.Models.Documents;
using ArmSoft.SME.Common.Models.SmartEnums;
using ArmSoft.SME.Common.Parameters;
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
using ArmSoft.SME.Trade.Client.Routes;
using ArmSoft.SME.Trade.Common;
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
using ArmSoft.SME.Trade.UI.Models;
using ArmSoft.SME.Trade.UI.OtherWindows;
using ArmSoft.SME.Trade.UI.Services;
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

[DocumentWindow(15)]
public class MTReturnFromCustomerWindow : TradeDocumentWindow<MTReturnFromCustomer>
{
	public class MTRetFromCustSpecificationRow : DocumentGridRow, ISaleRow
	{
		[ObservableProperty]
		private string itemType = string.Empty;

		[ObservableProperty]
		private string storage = string.Empty;

		[ObservableProperty]
		private string barCode;

		[ObservableProperty]
		private int itemID = -1;

		[ObservableProperty]
		private string itemCode = string.Empty;

		[ObservableProperty]
		private string itemName;

		[ObservableProperty]
		private string unitCode = string.Empty;

		[ObservableProperty]
		private string unitBrief;

		[ObservableProperty]
		private short rowNum;

		[ObservableProperty]
		private int rowID;

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
		private decimal beginPrice;

		[ObservableProperty]
		private decimal beginSumma;

		[ObservableProperty]
		private decimal costPrice;

		[ObservableProperty]
		private decimal costPriceVAT;

		[ObservableProperty]
		private decimal currCostPrice;

		[ObservableProperty]
		private string vATCodeForConvert;

		[ObservableProperty]
		private bool? vAT = false;

		[ObservableProperty]
		private string dealType = string.Empty;

		[ObservableProperty]
		private int baseRowID;

		[ObservableProperty]
		private string rowDescription;

		[ObservableProperty]
		private bool isStorageRequired;

		[ObservableProperty]
		private bool mDeleted;

		[ObservableProperty]
		private bool isGift;

		[ObservableProperty]
		private bool deleted;

		[ObservableProperty]
		private bool gift;

		[ObservableProperty]
		private DateOnly? useLife;

		[ObservableProperty]
		private string country;

		[ObservableProperty]
		private string producer;

		[ObservableProperty]
		private string property1;

		[ObservableProperty]
		private string property2;

		[ObservableProperty]
		private Guid partyBase;

		[ObservableProperty]
		private int partyRow;

		[ObservableProperty]
		private string fullName;

		[ObservableProperty]
		private string producerCaption;

		[ObservableProperty]
		private string property1Caption;

		[ObservableProperty]
		private string property2Caption;

		[ObservableProperty]
		private decimal usedBonus;

		[ObservableProperty]
		private decimal accumulatedBonus;

		[ObservableProperty]
		private bool isDeleted;

		[ObservableProperty]
		private string markingStatus;

		[ObservableProperty]
		private HashSet<string> markingList = new HashSet<string>();

		[ObservableProperty]
		private bool? isPartySelected;

		[ObservableProperty]
		private bool isRowMarkable;

		public string DealTypeCaption
		{
			get
			{
				if (string.IsNullOrWhiteSpace(DealType) || DocWindow == null)
				{
					return string.Empty;
				}
				if (Convert.ToString(DocWindow.GetControlValue("VATType")).InList("3", "4"))
				{
					return SmartEnum<AccDealTypesSE, short>.FromValue(Convert.ToInt16(DealType)).Caption;
				}
				return SmartEnum<InvoiceDealTypesSE, short>.FromValue(Convert.ToInt16(DealType)).Caption;
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
		public decimal BeginPrice
		{
			get
			{
				return beginPrice;
			}
			set
			{
				if (!EqualityComparer<decimal>.Default.Equals(beginPrice, value))
				{
					OnPropertyChanging(__KnownINotifyPropertyChangingArgs.BeginPrice);
					beginPrice = value;
					OnPropertyChanged(__KnownINotifyPropertyChangedArgs.BeginPrice);
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
		public decimal CostPrice
		{
			get
			{
				return costPrice;
			}
			set
			{
				if (!EqualityComparer<decimal>.Default.Equals(costPrice, value))
				{
					OnPropertyChanging(__KnownINotifyPropertyChangingArgs.CostPrice);
					costPrice = value;
					OnPropertyChanged(__KnownINotifyPropertyChangedArgs.CostPrice);
				}
			}
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public decimal CostPriceVAT
		{
			get
			{
				return costPriceVAT;
			}
			set
			{
				if (!EqualityComparer<decimal>.Default.Equals(costPriceVAT, value))
				{
					OnPropertyChanging(__KnownINotifyPropertyChangingArgs.CostPriceVAT);
					costPriceVAT = value;
					OnPropertyChanged(__KnownINotifyPropertyChangedArgs.CostPriceVAT);
				}
			}
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public decimal CurrCostPrice
		{
			get
			{
				return currCostPrice;
			}
			set
			{
				if (!EqualityComparer<decimal>.Default.Equals(currCostPrice, value))
				{
					OnPropertyChanging(__KnownINotifyPropertyChangingArgs.CurrCostPrice);
					currCostPrice = value;
					OnPropertyChanged(__KnownINotifyPropertyChangedArgs.CurrCostPrice);
				}
			}
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public string VATCodeForConvert
		{
			get
			{
				return vATCodeForConvert;
			}
			[MemberNotNull("vATCodeForConvert")]
			set
			{
				if (!EqualityComparer<string>.Default.Equals(vATCodeForConvert, value))
				{
					OnPropertyChanging(__KnownINotifyPropertyChangingArgs.VATCodeForConvert);
					vATCodeForConvert = value;
					OnPropertyChanged(__KnownINotifyPropertyChangedArgs.VATCodeForConvert);
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
		public int BaseRowID
		{
			get
			{
				return baseRowID;
			}
			set
			{
				if (!EqualityComparer<int>.Default.Equals(baseRowID, value))
				{
					OnPropertyChanging(__KnownINotifyPropertyChangingArgs.BaseRowID);
					baseRowID = value;
					OnPropertyChanged(__KnownINotifyPropertyChangedArgs.BaseRowID);
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
		public bool MDeleted
		{
			get
			{
				return mDeleted;
			}
			set
			{
				if (!EqualityComparer<bool>.Default.Equals(mDeleted, value))
				{
					OnPropertyChanging(__KnownINotifyPropertyChangingArgs.MDeleted);
					mDeleted = value;
					OnPropertyChanged(__KnownINotifyPropertyChangedArgs.MDeleted);
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
		public bool Deleted
		{
			get
			{
				return deleted;
			}
			set
			{
				if (!EqualityComparer<bool>.Default.Equals(deleted, value))
				{
					OnPropertyChanging(__KnownINotifyPropertyChangingArgs.Deleted);
					deleted = value;
					OnPropertyChanged(__KnownINotifyPropertyChangedArgs.Deleted);
				}
			}
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public bool Gift
		{
			get
			{
				return gift;
			}
			set
			{
				if (!EqualityComparer<bool>.Default.Equals(gift, value))
				{
					OnPropertyChanging(__KnownINotifyPropertyChangingArgs.Gift);
					gift = value;
					OnPropertyChanged(__KnownINotifyPropertyChangedArgs.Gift);
				}
			}
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public DateOnly? UseLife
		{
			get
			{
				return useLife;
			}
			set
			{
				if (!EqualityComparer<DateOnly?>.Default.Equals(useLife, value))
				{
					OnPropertyChanging(__KnownINotifyPropertyChangingArgs.UseLife);
					useLife = value;
					OnPropertyChanged(__KnownINotifyPropertyChangedArgs.UseLife);
				}
			}
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public string Country
		{
			get
			{
				return country;
			}
			[MemberNotNull("country")]
			set
			{
				if (!EqualityComparer<string>.Default.Equals(country, value))
				{
					OnPropertyChanging(__KnownINotifyPropertyChangingArgs.Country);
					country = value;
					OnCountryChanged(value);
					OnPropertyChanged(__KnownINotifyPropertyChangedArgs.Country);
				}
			}
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public string Producer
		{
			get
			{
				return producer;
			}
			[MemberNotNull("producer")]
			set
			{
				if (!EqualityComparer<string>.Default.Equals(producer, value))
				{
					OnPropertyChanging(__KnownINotifyPropertyChangingArgs.Producer);
					producer = value;
					OnProducerChanged(value);
					OnPropertyChanged(__KnownINotifyPropertyChangedArgs.Producer);
				}
			}
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public string Property1
		{
			get
			{
				return property1;
			}
			[MemberNotNull("property1")]
			set
			{
				if (!EqualityComparer<string>.Default.Equals(property1, value))
				{
					OnPropertyChanging(__KnownINotifyPropertyChangingArgs.Property1);
					property1 = value;
					OnProperty1Changed(value);
					OnPropertyChanged(__KnownINotifyPropertyChangedArgs.Property1);
				}
			}
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public string Property2
		{
			get
			{
				return property2;
			}
			[MemberNotNull("property2")]
			set
			{
				if (!EqualityComparer<string>.Default.Equals(property2, value))
				{
					OnPropertyChanging(__KnownINotifyPropertyChangingArgs.Property2);
					property2 = value;
					OnProperty2Changed(value);
					OnPropertyChanged(__KnownINotifyPropertyChangedArgs.Property2);
				}
			}
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public Guid PartyBase
		{
			get
			{
				return partyBase;
			}
			set
			{
				if (!EqualityComparer<Guid>.Default.Equals(partyBase, value))
				{
					OnPropertyChanging(__KnownINotifyPropertyChangingArgs.PartyBase);
					partyBase = value;
					OnPropertyChanged(__KnownINotifyPropertyChangedArgs.PartyBase);
				}
			}
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public int PartyRow
		{
			get
			{
				return partyRow;
			}
			set
			{
				if (!EqualityComparer<int>.Default.Equals(partyRow, value))
				{
					OnPropertyChanging(__KnownINotifyPropertyChangingArgs.PartyRow);
					partyRow = value;
					OnPropertyChanged(__KnownINotifyPropertyChangedArgs.PartyRow);
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
		public string ProducerCaption
		{
			get
			{
				return producerCaption;
			}
			[MemberNotNull("producerCaption")]
			set
			{
				if (!EqualityComparer<string>.Default.Equals(producerCaption, value))
				{
					OnPropertyChanging(__KnownINotifyPropertyChangingArgs.ProducerCaption);
					producerCaption = value;
					OnPropertyChanged(__KnownINotifyPropertyChangedArgs.ProducerCaption);
				}
			}
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public string Property1Caption
		{
			get
			{
				return property1Caption;
			}
			[MemberNotNull("property1Caption")]
			set
			{
				if (!EqualityComparer<string>.Default.Equals(property1Caption, value))
				{
					OnPropertyChanging(__KnownINotifyPropertyChangingArgs.Property1Caption);
					property1Caption = value;
					OnProperty1CaptionChanged(value);
					OnPropertyChanged(__KnownINotifyPropertyChangedArgs.Property1Caption);
				}
			}
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public string Property2Caption
		{
			get
			{
				return property2Caption;
			}
			[MemberNotNull("property2Caption")]
			set
			{
				if (!EqualityComparer<string>.Default.Equals(property2Caption, value))
				{
					OnPropertyChanging(__KnownINotifyPropertyChangingArgs.Property2Caption);
					property2Caption = value;
					OnProperty2CaptionChanged(value);
					OnPropertyChanged(__KnownINotifyPropertyChangedArgs.Property2Caption);
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
					OnPropertyChanged(__KnownINotifyPropertyChangedArgs.IsDeleted);
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
					OnPropertyChanged(__KnownINotifyPropertyChangedArgs.MarkingStatus);
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

		public MTRetFromCustSpecificationRow()
		{
		}

		public MTRetFromCustSpecificationRow(MTReturnFromCustomerWindow docWindow)
			: base(docWindow)
		{
		}

		static MTRetFromCustSpecificationRow()
		{
			DocumentGridRow.RegisterPropertyValidators(typeof(MTRetFromCustSpecificationRow), cacheValidationResults: true, new Dictionary<string, Action<DocumentGridRow, DevExpress.XtraEditors.DXErrorProvider.ErrorInfo>>
			{
				{
					"Storage",
					(DocumentGridRow row, DevExpress.XtraEditors.DXErrorProvider.ErrorInfo info) =>
					{
						ValidateStorage((MTRetFromCustSpecificationRow)row, info);
					}
				},
				{
					"ItemCode",
					(DocumentGridRow row, DevExpress.XtraEditors.DXErrorProvider.ErrorInfo info) =>
					{
						ValidateItemCode((MTRetFromCustSpecificationRow)row, info);
					}
				},
				{
					"UnitCode",
					(DocumentGridRow row, DevExpress.XtraEditors.DXErrorProvider.ErrorInfo info) =>
					{
						ValidateUnitCode((MTRetFromCustSpecificationRow)row, info);
					}
				},
				{
					"Quantity",
					(DocumentGridRow row, DevExpress.XtraEditors.DXErrorProvider.ErrorInfo info) =>
					{
						ValidateQuantity((MTRetFromCustSpecificationRow)row, info);
					}
				},
				{
					"Price",
					(DocumentGridRow row, DevExpress.XtraEditors.DXErrorProvider.ErrorInfo info) =>
					{
						ValidatePrice((MTRetFromCustSpecificationRow)row, info);
					}
				},
				{
					"DiscountPrice",
					(DocumentGridRow row, DevExpress.XtraEditors.DXErrorProvider.ErrorInfo info) =>
					{
						ValidateDiscountPrice((MTRetFromCustSpecificationRow)row, info);
					}
				},
				{
					"Summa",
					(DocumentGridRow row, DevExpress.XtraEditors.DXErrorProvider.ErrorInfo info) =>
					{
						ValidateSumma((MTRetFromCustSpecificationRow)row, info);
					}
				},
				{
					"Country",
					(DocumentGridRow row, DevExpress.XtraEditors.DXErrorProvider.ErrorInfo info) =>
					{
						ValidateCountry((MTRetFromCustSpecificationRow)row, info);
					}
				},
				{
					"Producer",
					(DocumentGridRow row, DevExpress.XtraEditors.DXErrorProvider.ErrorInfo info) =>
					{
						ValidateProducer((MTRetFromCustSpecificationRow)row, info);
					}
				},
				{
					"Property1",
					(DocumentGridRow row, DevExpress.XtraEditors.DXErrorProvider.ErrorInfo info) =>
					{
						ValidateProperty1((MTRetFromCustSpecificationRow)row, info);
					}
				},
				{
					"Property2",
					(DocumentGridRow row, DevExpress.XtraEditors.DXErrorProvider.ErrorInfo info) =>
					{
						ValidateProperty2((MTRetFromCustSpecificationRow)row, info);
					}
				},
				{
					"Property1Caption",
					(DocumentGridRow row, DevExpress.XtraEditors.DXErrorProvider.ErrorInfo info) =>
					{
						ValidateProperty1Caption((MTRetFromCustSpecificationRow)row, info);
					}
				},
				{
					"Property2Caption",
					(DocumentGridRow row, DevExpress.XtraEditors.DXErrorProvider.ErrorInfo info) =>
					{
						ValidateProperty2Caption((MTRetFromCustSpecificationRow)row, info);
					}
				}
			});
		}

		private static void ValidateStorage(MTRetFromCustSpecificationRow row, DevExpress.XtraEditors.DXErrorProvider.ErrorInfo info)
		{
			if (!((MTReturnFromCustomerWindow)row.DocWindow).IsOfflineDoc && row.ItemType == ItemTypesSE.Goods.Value)
			{
				TradePropertyValidator.ValidateStorage(row.Storage, info, ArmSoft.SME.Common.Resources.Storage, isRequired: true);
			}
		}

		private static void ValidateItemCode(MTRetFromCustSpecificationRow row, DevExpress.XtraEditors.DXErrorProvider.ErrorInfo info)
		{
			if (row.ItemType == ItemTypesSE.Goods.Value)
			{
				TradePropertyValidator.ValidateMaterialCode(row.ItemCode, info, TradeAllDocDefinitions.MTReturnFromCustomer.Grids["MTList"].Columns["ItemCode"]);
			}
			else
			{
				TradePropertyValidator.ValidateServiceCode(row.ItemCode, info, TradeAllDocDefinitions.MTReturnFromCustomer.Grids["MTList"].Columns["ItemCode"]);
			}
		}

		private static void ValidateUnitCode(MTRetFromCustSpecificationRow row, DevExpress.XtraEditors.DXErrorProvider.ErrorInfo info)
		{
			TradePropertyValidator.ValidateGoodUnitCode(row.UnitCode, row.ItemCode, info, TradeAllDocDefinitions.MTReturnFromCustomer.Grids["MTList"].Columns["UnitCode"]);
		}

		private static void ValidateQuantity(MTRetFromCustSpecificationRow row, DevExpress.XtraEditors.DXErrorProvider.ErrorInfo info)
		{
			PropertyValidator.PositiveValidate(row.Quantity, info, ArmSoft.SME.Common.Resources.Quantity);
		}

		private static void ValidatePrice(MTRetFromCustSpecificationRow row, DevExpress.XtraEditors.DXErrorProvider.ErrorInfo info)
		{
			PropertyValidator.NonNegativeValidate(row.Price, info, ArmSoft.SME.Common.Resources.Price);
		}

		private static void ValidateDiscountPrice(MTRetFromCustSpecificationRow row, DevExpress.XtraEditors.DXErrorProvider.ErrorInfo info)
		{
			PropertyValidator.NonNegativeValidate(row.DiscountPrice, info, ArmSoft.SME.Common.Resources.DiscountedPrice);
		}

		private static void ValidateSumma(MTRetFromCustSpecificationRow row, DevExpress.XtraEditors.DXErrorProvider.ErrorInfo info)
		{
			if (row.Deleted || row.Discount == 100m || row.IsGift || Settings.ParametersService.AllowUse100PercentSale())
			{
				PropertyValidator.NonNegativeValidate(row.Summa, info, ArmSoft.SME.Common.Resources.Summa);
			}
			else
			{
				PropertyValidator.PositiveValidate(row.Summa, info, ArmSoft.SME.Common.Resources.Summa);
			}
		}

		protected static void ValidateCountry(MTRetFromCustSpecificationRow row, DevExpress.XtraEditors.DXErrorProvider.ErrorInfo info)
		{
			if (!string.IsNullOrWhiteSpace(row.Country))
			{
				TradePropertyValidator.ValidateCountry(row.Country, info, TradeAllDocDefinitions.MTReturnFromCustomer.Grids["MTList"].Columns["Country"]);
			}
		}

		protected static void ValidateProducer(MTRetFromCustSpecificationRow row, DevExpress.XtraEditors.DXErrorProvider.ErrorInfo info)
		{
			if (!string.IsNullOrWhiteSpace(row.Producer))
			{
				TradePropertyValidator.ValidateProducer(row.Producer, info, TradeAllDocDefinitions.MTReturnFromCustomer.Grids["MTList"].Columns["Producer"]);
			}
		}

		protected static void ValidateProperty1(MTRetFromCustSpecificationRow row, DevExpress.XtraEditors.DXErrorProvider.ErrorInfo info)
		{
			if (!string.IsNullOrWhiteSpace(row.Property1))
			{
				TradePropertyValidator.ValidateProperty(row.Property1, row.ItemCode, isProperty1: true, info, TradeAllDocDefinitions.MTReturnFromCustomer.Grids["MTList"].Columns["Property1"]);
			}
		}

		protected static void ValidateProperty2(MTRetFromCustSpecificationRow row, DevExpress.XtraEditors.DXErrorProvider.ErrorInfo info)
		{
			if (!string.IsNullOrWhiteSpace(row.Property2))
			{
				TradePropertyValidator.ValidateProperty(row.Property2, row.ItemCode, isProperty1: false, info, TradeAllDocDefinitions.MTReturnFromCustomer.Grids["MTList"].Columns["Property2"]);
			}
		}

		protected static void ValidateProperty1Caption(MTRetFromCustSpecificationRow row, DevExpress.XtraEditors.DXErrorProvider.ErrorInfo info)
		{
			if (!string.IsNullOrWhiteSpace(row.Property1Caption))
			{
				TradePropertyValidator.ValidatePropertyValue(row.Property1Caption, row.ItemCode, isProperty1: true, info, TradeAllDocDefinitions.MTReturnFromCustomer.Grids["MTList"].Columns["Property1Caption"]);
			}
		}

		protected static void ValidateProperty2Caption(MTRetFromCustSpecificationRow row, DevExpress.XtraEditors.DXErrorProvider.ErrorInfo info)
		{
			if (!string.IsNullOrWhiteSpace(row.Property2Caption))
			{
				TradePropertyValidator.ValidatePropertyValue(row.Property2Caption, row.ItemCode, isProperty1: false, info, TradeAllDocDefinitions.MTReturnFromCustomer.Grids["MTList"].Columns["Property2Caption"]);
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
			ResetForceValidationBit("Quantity");
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
		private void OnCountryChanged(string value)
		{
			ResetForceValidationBit("Country");
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		private void OnProducerChanged(string value)
		{
			ResetForceValidationBit("Producer");
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		private void OnProperty1Changed(string value)
		{
			ResetForceValidationBit("Property1");
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		private void OnProperty2Changed(string value)
		{
			ResetForceValidationBit("Property2");
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		private void OnProperty1CaptionChanged(string value)
		{
			ResetForceValidationBit("Property1Caption");
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		private void OnProperty2CaptionChanged(string value)
		{
			ResetForceValidationBit("Property2Caption");
		}
	}

	public class MTReturnFromCustomerFilteredSearchList : IResultList
	{
		private readonly MTReturnFromCustomerWindow _docWindow;

		private readonly ObservableCollection<MTRetFromCustSpecificationRow> _itemSource = new ObservableCollection<MTRetFromCustSpecificationRow>();

		public IEnumerable<DocumentGridRow> ItemsSource => _itemSource;

		public List<FilteredSearchGridColumn> Columns => new List<FilteredSearchGridColumn>
		{
			new FilteredSearchGridColumn
			{
				Header = TradeAllDocDefinitions.MTInvoice.Grids["MTList"].Columns["ItemCode"].Caption,
				FieldName = "ItemCode",
				CustomWidth = 125.0
			},
			new FilteredSearchGridColumn
			{
				Header = TradeAllDocDefinitions.MTInvoice.Grids["MTList"].Columns["ItemName"].Caption,
				FieldName = "ItemName",
				CustomWidth = 300.0
			},
			new FilteredSearchGridColumn
			{
				Header = TradeAllDocDefinitions.MTInvoice.Grids["MTList"].Columns["Quantity"].Caption,
				FieldName = "Quantity",
				AllowEditing = true,
				NumericFormat = DecimalExtension.GetCurrencyFormat(13, 3)
			}
		};

		public MTReturnFromCustomerFilteredSearchList(MTReturnFromCustomerWindow docWindow)
		{
			_docWindow = docWindow;
		}

		public int AddToItemsSource(object key, params object[] args)
		{
			string mtCode = (string)key;
			decimal num = (decimal)args[0];
			int num2 = _itemSource.FindIndex((MTRetFromCustSpecificationRow x) => x.ItemCode == mtCode);
			if (num2 == -1)
			{
				MaterialCacheRow item = TradeSettings.TradeCacheService.Material.GetItem(mtCode);
				if (item == null)
				{
					return -1;
				}
				MTRetFromCustSpecificationRow item2 = new MTRetFromCustSpecificationRow(_docWindow)
				{
					ItemType = item.Type,
					ItemID = item.Id,
					ItemCode = item.MTCode,
					ItemName = item.Caption,
					UnitCode = item.Unit,
					UnitBrief = item.UnitRow.Brief,
					VAT = item.VAT,
					Country = item.Country,
					Producer = item.Producer
				};
				_itemSource.Add(item2);
				num2 = _itemSource.Count - 1;
			}
			_itemSource[num2].Quantity += num;
			return num2;
		}
	}

	private DropDownCommentControl _baseDocumentControl;

	private TextEditExt _barCode;

	private Button _codeEnter;

	private PartnerContractGroupControls _buyerContractContainer;

	private DropDownControl _salesConsultant;

	private TextEditExt _baseDocTaxInvoiceSeria;

	private TextEditExt _baseDocTaxInvoiceNumber;

	private DateEditControl _baseDocTaxInvoiceDate;

	private DropDownControl _exportedType;

	private TextEditExt _taxInvoiceSeries;

	private TextEditExt _taxInvoiceNumber;

	private DateEditControl _taxInvoiceDate;

	private ComboBoxEditExt _comment;

	private DropDownControl _owner;

	public DropDownControl _currCode;

	public ExchangeRateControl _currExChngRate;

	public DropDownControl _vatType;

	public DropDownControl _dealType;

	private string _dealTypeKey;

	public DropDownControl _priceListType;

	public DropDownControl _storage;

	public DocumentGridControl _mtRetFromCustGrid;

	private TableView _mtRetFromCustTableView;

	public ObservableCollection<MTRetFromCustSpecificationRow> _mtRetFromCustList;

	private BarEditItemExt _filteredSearch;

	private BarEditItemExt _fillLatestPurchasePrices;

	private DocumentGridColumn _itemTypeCol;

	private DocumentGridColumn _storageCol;

	private DocumentGridColumn _itemCodeCol;

	private DocumentGridColumn _unitCodeCol;

	private DocumentGridColumn _quantityCol;

	private DocumentGridColumn _priceCol;

	private DocumentGridColumn _summaCol;

	private DocumentGridColumn _costPriceCol;

	private DocumentGridColumn _costPriceVATCol;

	public DocumentGridColumn _currCostPriceCol;

	private DocumentGridColumn _vatCol;

	private DocumentGridColumn _dealTypeCol;

	private DocumentGridColumn _usedBonusSummCol;

	private DocumentGridColumn _accumulatedBonusCol;

	private DocumentGridColumn _useLifeCol;

	private DocumentGridColumn _countryCol;

	private DocumentGridColumn _producerCol;

	private DocumentGridColumn _property1Col;

	private DocumentGridColumn _property2Col;

	private DocumentGridColumn _rowDescCol;

	private PopupCalcEditExt _vatSumma;

	private PopupCalcEditExt _totalSumma;

	private TextEditExt _supplierName;

	private TextEditExt _supplierLegalAddress;

	private DropDownCommentControl _supplierBusinessAddress;

	private DropDownControl _supplierSettlAcc;

	private TextEditExt _supplierTaxCode;

	private ComboBoxEditExt _supplierMngPost;

	private TextEditExt _supplierMngName;

	private ComboBoxEditExt _supplierChiefAccPost;

	private TextEditExt _supplierChiefAccName;

	private TextEditExt _buyerName;

	private TextEditExt _buyerLegalAddress;

	private ComboBoxEditExt _buyerBusinessAddress;

	private DropDownControl _buyerSettlAcc;

	private TextEditExt _buyerTaxCode;

	private ComboBoxEditExt _buyerMngPost;

	private TextEditExt _buyerMngName;

	private ComboBoxEditExt _buyerChiefAccPost;

	private TextEditExt _buyerChiefAccName;

	private TextEditExt _buyerStateRegNum;

	private DropDownControl _buyerIDDocType;

	private TextEditExt _buyerPassport;

	private ComboBoxEditExt _buyerAdditionalData;

	private ComboBoxEditExt _buyerShippingMethod;

	private ComboBoxEditExt _byWhomFio;

	private TextEditExt _authorizationNumber;

	private DateEditControl _authorizationDate;

	private TextEditExt _trust;

	private DropDownCommentControl _mediator;

	private TextEditExt _machine;

	private TextEditExt _chief;

	private DropDownCommentControl _allower;

	private DropDownCommentControl _response;

	private DXTabControl _tabs;

	private int _priceListTabControlIndex;

	private DropDownControl _cashDesk;

	private PartnerContractGroupControls _bankAcquiringContainer;

	private TextEditExt _cashierShift;

	private TextEditExt _baseECRCheckNum;

	private TextEditExt _returnECRCheckNum;

	private TextEditExt _ecrCheckNum;

	private PopupCalcEditExt _returnECRTotalSum;

	private PopupCalcEditExt _ecrReturnByCardSum;

	private PopupCalcEditExt _ecrReturnByCashSum;

	private PopupCalcEditExt _ecrReturnByPrePaymentSum;

	private PopupCalcEditExt _baseECRByPrePaymentSum;

	private PopupCalcEditExt _baseECRByCardSum;

	private PopupCalcEditExt _baseECRByCashSum;

	private Button _printECRCheck;

	private Guid _baseISN = Guid.Empty;

	private TradeDocument _baseDocument;

	public int _maxRowID;

	private decimal _tradeConstVAT;

	private string _oldDocCurrCode;

	private decimal _oldDocCurrExChngRate = 1m;

	private decimal _oldDocCurrExChngBase = 1m;

	public string _paramCurrCode;

	public decimal _paramCurrExChngRate = 1m;

	public decimal _paramCurrExChngBase = 1m;

	private bool _bonusUsed;

	private decimal _sumFromRound;

	private ECR _ecr;

	private List<MTInvoiceBase.MTInvoiceSpecificationRow> _baseDocList;

	private bool _isConnectedToDS = Settings.ParametersService.ConnectTob2barmsoftDiscountServer();

	private string _rowAddMode = Settings.ParametersService.ScannerMode();

	private BarButtonItem _barButtonPrintLabel;

	private BarSubItem _barSubItemActions = new BarSubItem();

	private BarSubItem _barSubItemJournals = new BarSubItem();

	private ReportBarButtonItem _barButtonJumpTo;

	private ReportBarButtonItem _barButtonMTSalesAnalysis;

	private ReportBarButtonItem _barButtonBonusOperationJournal;

	private ReportBarButtonItem _barButtonGoodsOperationsJournal;

	private ReportBarButtonItem _barButtonGoodsRetailOperationJournal;

	private ReportBarButtonItem _barButtonPartnerOperationJournal;

	private ReportBarButtonItem _barButtonCashOperationJournal;

	private string _goodsTypes_Goods = GoodsTypesSE.Good;

	protected override double WidthValue => 1090.0;

	protected override double HeightValue => 440.0;

	protected override double MinWidthValue => 700.0;

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
			bool flag = _baseDocument != null;
			if (!flag)
			{
				_baseDocumentControl.Text = "";
				_printECRCheck.IsEnabled = false;
			}
			else
			{
				_printECRCheck.IsEnabled = string.IsNullOrWhiteSpace(_baseECRCheckNum.Text) && BaseDocument?.POType == ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTBill.Value;
				_baseDocumentControl.Text = ArmSoft.SME.Trade.UI.Helpers.DocumentHelpers.GetDocumentDescription(_baseDocument, ArmSoft.SME.Common.Constants.Common.DocumentDescriptionMode.TypeDateNum);
			}
			if (Settings.ParametersService.ShowGoodsCostInReportsAndDocs())
			{
				_costPriceCol.Visible = !flag;
				_costPriceCol.ShowInColumnChooser = !flag;
			}
			else
			{
				_costPriceCol.Visible = false;
				_costPriceCol.ShowInColumnChooser = false;
			}
			_useLifeCol.Visible = !flag;
			_useLifeCol.ShowInColumnChooser = !flag;
			_property1Col.Visible = !flag;
			_property1Col.ShowInColumnChooser = !flag;
			_property2Col.Visible = !flag;
			_property2Col.ShowInColumnChooser = !flag;
			_countryCol.Visible = !flag;
			_countryCol.ShowInColumnChooser = !flag;
			_producerCol.Visible = !flag;
			if (!flag)
			{
				foreach (MTRetFromCustSpecificationRow mtRetFromCust in _mtRetFromCustList)
				{
					mtRetFromCust.AccumulatedBonus = 0m;
					mtRetFromCust.UsedBonus = 0m;
				}
				_accumulatedBonusCol.Visible = false;
				_accumulatedBonusCol.ShowInColumnChooser = false;
				_usedBonusSummCol.Visible = false;
				_usedBonusSummCol.ShowInColumnChooser = false;
			}
			_producerCol.ShowInColumnChooser = !flag;
			if (Settings.ParametersService.ShowGoodsCostInReportsAndDocs())
			{
				_currCostPriceCol.Visible = !flag;
				_currCostPriceCol.ShowInColumnChooser = !flag;
				_costPriceVATCol.Visible = !flag;
				_costPriceVATCol.ShowInColumnChooser = !flag;
			}
			else
			{
				_currCostPriceCol.Visible = false;
				_currCostPriceCol.ShowInColumnChooser = false;
				_costPriceVATCol.Visible = false;
				_costPriceVATCol.ShowInColumnChooser = false;
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
				_usedBonusSummCol.Visible = _bonusUsed;
				_usedBonusSummCol.ShowInColumnChooser = _bonusUsed;
				_accumulatedBonusCol.Visible = _bonusUsed;
				_accumulatedBonusCol.ShowInColumnChooser = _bonusUsed;
			}
		}
	}

	public MTReturnFromCustomerWindow()
	{
	}

	public MTReturnFromCustomerWindow(MTReturnFromCustomer doc)
		: base(doc)
	{
	}

	protected override void ConstructUI()
	{
		base.ConstructUI();
		_tradeConstVAT = Settings.ParametersService.VAT();
		_paramCurrCode = Settings.ParametersService.MTCurr();
		LayoutGroupExt container = AddLayoutGroup(null, LayoutGroupView.Group, Orientation.Horizontal);
		_date = AddDateControl(container, Definition.Fields["DocDate"]);
		_date.Margin = new Thickness(0.0, 0.0, 10.0, 0.0);
		_date.EditValueChanged += new SafeDocumentEventHandler<EditValueChangedEventArgs>(Date_EditValueChanged, this).Handler;
		_docNum = AddButtonEditControl(container, Definition.Fields["DocNum"], 1);
		_docNum.AllowMagicButton = true;
		_docNum.MagicToolTip = ArmSoft.SME.Common.Resources.DocNextNumbers;
		_docNum.MagicClick += new SafeDocumentEventHandler<MagicClickEventArgs>(DocNum_MagicClick, this).Handler;
		_docNum.EditValueChanged += new SafeDocumentEventHandler<EditValueChangedEventArgs>(DocNum_EditValueChanged, this).Handler;
		if (IsOfflineDoc)
		{
			_baseDocumentControl = AddDropDownCommentControl(null, ArmSoft.SME.Common.Resources.BaseDoc, isRequired: false, null, 50, HorizontalAlignment.Stretch);
		}
		else
		{
			DocumentsJournal idv = new DocumentsJournal(this, ReportTypesSE.AllDocument, ((TradeDocDefinition)Definition).BaseDocTypes)
			{
				DocType = ((TradeDocDefinition)Definition).BaseDocTypes.First().ToString().PadLeft(2, '0'),
				DocState = ArmSoft.SME.Trade.Common.SmartEnums.DocumentStateSE.DraftOrProcessed.ValueUI
			};
			_baseDocumentControl = AddDropDownCommentControl(null, ArmSoft.SME.Common.Resources.BaseDoc, isRequired: false, idv, 50, HorizontalAlignment.Stretch);
		}
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
		_buyerContractContainer = AddPartnerContractGroupControls(container3, (PartnerContractGroupDefinition)Definition.Fields["PartnerContractGroup"], _date, null, null);
		_buyerContractContainer.PartnerCodeEditValueChanged += new SafeDocumentEventHandler<EditValueChangedEventArgs>(PartnerCodeEditValueChanged, this).Handler;
		_buyerContractContainer.ContractCodeEditValueChanged += new SafeDocumentEventHandler<EventArgs>(ContractCodeEditValueChanged, this).Handler;
		_salesConsultant = AddDropDownControl(container3, Definition.Fields["SalesConsultant"], new Employees(isOnlySalesConsultant: true), allowAdditionButtons: true);
		LayoutGroupExt container4 = AddLayoutGroup(container3, LayoutGroupView.Group, Orientation.Horizontal);
		_baseDocTaxInvoiceSeria = AddStringControl(container4, Definition.Fields["BaseDocTaxInvoiceSeries"]);
		_baseDocTaxInvoiceNumber = AddStringControl(container4, Definition.Fields["BaseDocTaxInvoiceNumber"]);
		_baseDocTaxInvoiceDate = AddDateControl(container3, Definition.Fields["BaseDocTaxInvoiceDate"]);
		_exportedType = AddSmartEnumDropDownControl(container3, Definition.Fields["ExportedType"], allowAdditionButtons: false, fieldWithCaption: true, HorizontalAlignment.Stretch);
		LayoutGroupExt container5 = AddLayoutGroup(container3, LayoutGroupView.Group, Orientation.Horizontal);
		_taxInvoiceSeries = AddStringControl(container5, Definition.Fields["TaxInvoiceSeries"]);
		_taxInvoiceNumber = AddStringControl(container5, Definition.Fields["TaxInvoiceNumber"]);
		_taxInvoiceDate = AddDateControl(container3, Definition.Fields["InvoiceDate"]);
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
		LayoutGroup container6 = AddTabbedGroup(_tabs, ArmSoft.SME.Common.Resources.ItemList, "MtList");
		_priceListTabControlIndex = _tabs.Items.Count - 1;
		LayoutGroupExt container7 = AddLayoutGroup(container6, LayoutGroupView.Group, Orientation.Horizontal);
		_currCode = AddDropDownControl(container7, Definition.Fields["CurrCode"], new Currency(shortMode: true), allowAdditionButtons: false, fieldWithCaption: true, HorizontalAlignment.Stretch);
		_currCode.EditValueChanged += new SafeDocumentEventHandler<EditValueChangedEventArgs>(CurrCode_EditValueChanged, this).Handler;
		_vatSumma = AddNumericControl(container7, Definition.Fields["SummVAT"], useCurrencyFormat: true, HorizontalAlignment.Right);
		LayoutGroupExt container8 = AddLayoutGroup(container6, LayoutGroupView.Group, Orientation.Horizontal);
		_currExChngRate = AddExchangeRateControl(container8, Definition.Fields["ExchangeRate"]);
		_currExChngRate.EditValueChanged += new SafeDocumentEventHandler<RateEditValueChangedEventArgs>(CurrExChngRate_EditValueChanged, this).Handler;
		_totalSumma = AddNumericControl(container8, Definition.Fields["Summ"], useCurrencyFormat: true, HorizontalAlignment.Right);
		_totalSumma.EditValueChanged += new SafeDocumentEventHandler<EditValueChangedEventArgs>(TotalSumma_EditValueChanged, this).Handler;
		_totalSumma.IsRequired = !Settings.ParametersService.AllowUse100PercentSale();
		_vatType = AddDropDownControl(container6, Definition.Fields["VATType"], new VATTypes(includeVATRetail: true), allowAdditionButtons: false, fieldWithCaption: true, HorizontalAlignment.Stretch);
		_vatType.EditValueChanged += new SafeDocumentEventHandler<EditValueChangedEventArgs>(VATType_EditValueChanged, this).Handler;
		_dealType = AddDropDownControl(container6, Definition.Fields["DealType"], new SmartEnum(typeof(AccDealTypesSE), null, padLeft: false, '0', allSubTypes: false, ValueMember.ValueUI));
		_dealType.EditValueChanged += new SafeDocumentEventHandler<EditValueChangedEventArgs>(DealType_EditValueChanged, this).Handler;
		_dealType.ClickDropDown += new SafeDocumentEventHandler<DropDownControlEventArgs>(DealType_ClickDropDown, this).Handler;
		_priceListType = AddDropDownControl(container6, Definition.Fields["PriceType"], new PriceTypes(), allowAdditionButtons: false, fieldWithCaption: true, HorizontalAlignment.Stretch);
		_priceListType.EditValueChanged += new SafeDocumentEventHandler<EditValueChangedEventArgs>(PriceListType_EditValueChanged, this).Handler;
		_storage = AddDropDownControl(container6, Definition.Fields["Storage"], new Storages(), allowAdditionButtons: false, fieldWithCaption: true, HorizontalAlignment.Stretch);
		_storage.EditValueChanged += new SafeDocumentEventHandler<EditValueChangedEventArgs>(Storage_EditValueChanged, this).Handler;
		_mtRetFromCustGrid = AddGrid(container6, Definition.Grids["MTList"], HorizontalAlignment.Stretch, VerticalAlignment.Stretch, isSupportDialogMode: true);
		_mtRetFromCustGrid.Name = "MTRetFromCustGrid";
		_mtRetFromCustGrid.IsRequired = true;
		_mtRetFromCustGrid.AllowMoveUp = true;
		_mtRetFromCustGrid.AllowMoveDown = true;
		_mtRetFromCustGrid.AllowDuplicate = true;
		_mtRetFromCustGrid.AddingRow += new SafeDocumentEventHandler<DocumentGridAddingRowEventArgs>(MTRetFromCustGrid_AddingRow, this).Handler;
		_mtRetFromCustGrid.AddingRowWithDialog += new SafeDocumentEventHandler<DocumentGridAddingRowEventArgs>(MTRetFromCustGrid_AddingRowWithDialog, this).Handler;
		_mtRetFromCustGrid.DuplicatingRowWithDialog += new SafeDocumentEventHandler<DocumentGridDuplicatingRowEventArgs>(MTRetFromCustGrid_DuplicatingRowWithDialog, this).Handler;
		_mtRetFromCustGrid.EditingRowWithDialog += new SafeDocumentEventHandler<DocumentGridEditingRowEventArgs>(MTRetFromCustGrid_EditingRowWithDialog, this).Handler;
		_mtRetFromCustGrid.ClickDropDown += new SafeDocumentEventHandler<DocumentGridClickDropDownEventArgs>(MTRetFromCustGrid_ClickDropDown, this).Handler;
		_mtRetFromCustGrid.DeletedRow += new SafeDocumentEventHandler<EventArgs>(MTRetFromCustGrid_DeletedRow, this).Handler;
		_mtRetFromCustGrid.DuplicatingRow += new SafeDocumentEventHandler<DocumentGridDuplicatingRowEventArgs>(MTRetFromCustGrid_DuplicatingRow, this).Handler;
		_mtRetFromCustGrid.MovingRowUp += new SafeDocumentEventHandler<DocumentGridMovingRowUpOrDownEventArgs>(MTRetFromCustGrid_MovingRowUp, this).Handler;
		_mtRetFromCustGrid.MovingRowDown += new SafeDocumentEventHandler<DocumentGridMovingRowUpOrDownEventArgs>(MTRetFromCustGrid_MovingRowDown, this).Handler;
		_mtRetFromCustGrid.RefreshButtons += new SafeDocumentEventHandler<EventArgs>(MTRetFromCustGrid_RefreshButtons, this).Handler;
		_mtRetFromCustTableView = _mtRetFromCustGrid.View;
		_mtRetFromCustTableView.FocusedRowHandleChanged += new SafeDocumentEventHandler<FocusedRowHandleChangedEventArgs>(MTRetFromCustTableView_FocusedRowHandleChangedEventArgs, this).Handler;
		_mtRetFromCustTableView.CellValueChanged += new SafeDocumentEventHandler<CellValueChangedEventArgs>(MTRetFromCustTableView_CellValueChanged, this).Handler;
		_mtRetFromCustTableView.ShowingEditor += new SafeDocumentEventHandler<ShowingEditorEventArgs>(MTRetFromCustTableView_ShowingEditor, this).Handler;
		_mtRetFromCustTableView.ToolTip = ArmSoft.SME.Common.Resources.ItemList;
		_filteredSearch = _mtRetFromCustGrid.AddButton("", ArmSoft.SME.Trade.Common.Resources.GroupAddition, ArmSoft.SME.Trade.Common.Resources.GroupAddition, 130.0);
		_filteredSearch.Click += new SafeDocumentEventHandler<RoutedEventArgs>(FilteredSearch_Click, this).Handler;
		if (Settings.ParametersService.ShowGoodsCostInReportsAndDocs())
		{
			_fillLatestPurchasePrices = _mtRetFromCustGrid.AddButton("", ArmSoft.SME.Trade.Common.Resources.FillLatestCostPrices, ArmSoft.SME.Trade.Common.Resources.FillLatestPurchasePrices, 265.0);
			_fillLatestPurchasePrices.Click += new SafeDocumentEventHandler<RoutedEventArgs>(FillFromLastPurchase_Click, this).Handler;
		}
		_mtRetFromCustList = new ObservableCollection<MTRetFromCustSpecificationRow>();
		_mtRetFromCustTableView = _mtRetFromCustGrid.View;
		_mtRetFromCustGrid.AddRowIndicator(35);
		_itemTypeCol = _mtRetFromCustGrid.AddSmartEnumDropDownColumn(Definition.Grids["MTList"].Columns["ItemType"]);
		_itemTypeCol.Width = 55.0;
		_storageCol = _mtRetFromCustGrid.AddDropDownColumn(Definition.Grids["MTList"].Columns["Storage"]);
		_itemCodeCol = _mtRetFromCustGrid.AddDropDownColumn(Definition.Grids["MTList"].Columns["ItemCode"], new GoodsFilter(shortMode: true, "0", "", isShowOnlyInPriceList: true), allowAdditionButtons: true, isOwnProcessingClickDropDown: true);
		_itemCodeCol.Width = 125.0;
		DocumentGridColumn documentGridColumn = _mtRetFromCustGrid.AddStringColumn(Definition.Grids["MTList"].Columns["ItemName"]);
		documentGridColumn.AllowEditing = DefaultBoolean.False;
		documentGridColumn.Width = 225.0;
		_unitCodeCol = _mtRetFromCustGrid.AddDropDownColumn(Definition.Grids["MTList"].Columns["UnitCode"]);
		_mtRetFromCustGrid.AddStringColumn(Definition.Grids["MTList"].Columns["UnitBrief"]).AllowEditing = DefaultBoolean.False;
		_quantityCol = _mtRetFromCustGrid.AddNumberColumn(Definition.Grids["MTList"].Columns["Quantity"], showTotal: true);
		_priceCol = _mtRetFromCustGrid.AddNumberColumn(Definition.Grids["MTList"].Columns["Price"]);
		_summaCol = _mtRetFromCustGrid.AddNumberColumn(Definition.Grids["MTList"].Columns["Summa"], showTotal: true);
		_vatCol = _mtRetFromCustGrid.AddBooleanColumn(Definition.Grids["MTList"].Columns["VAT"]);
		_vatCol.Width = 45.0;
		_dealTypeCol = _mtRetFromCustGrid.AddDropDownColumn("DealTypeCaption", ArmSoft.SME.Common.Resources.DealType, 1, trimValue: false);
		((ButtonEditSettings)_dealTypeCol.EditSettings).IsTextEditable = false;
		_costPriceCol = _mtRetFromCustGrid.AddNumberColumn(Definition.Grids["MTList"].Columns["CostPrice"]);
		_costPriceVATCol = _mtRetFromCustGrid.AddNumberColumn(Definition.Grids["MTList"].Columns["CostPriceVAT"]);
		_currCostPriceCol = _mtRetFromCustGrid.AddNumberColumn(Definition.Grids["MTList"].Columns["CurrCostPrice"]);
		_currCostPriceCol.Header = string.Format(ArmSoft.SME.Trade.Common.Resources.CostSummaCurr, _paramCurrCode);
		_usedBonusSummCol = _mtRetFromCustGrid.AddNumberColumn(Definition.Grids["MTList"].Columns["UsedBonus"]);
		_usedBonusSummCol.AllowEditing = DefaultBoolean.False;
		_usedBonusSummCol.AllowFocus = false;
		_accumulatedBonusCol = _mtRetFromCustGrid.AddNumberColumn(Definition.Grids["MTList"].Columns["AccumulatedBonus"]);
		_accumulatedBonusCol.AllowEditing = DefaultBoolean.False;
		_accumulatedBonusCol.AllowFocus = false;
		_useLifeCol = _mtRetFromCustGrid.AddDateColumn(Definition.Grids["MTList"].Columns["UseLife"]);
		_useLifeCol.Visible = Settings.ParametersService.ShowPartyUSEABLEDATE();
		_useLifeCol.ShowInColumnChooser = _useLifeCol.Visible;
		_countryCol = _mtRetFromCustGrid.AddDropDownColumn(Definition.Grids["MTList"].Columns["Country"], new Countries());
		_countryCol.Visible = Settings.ParametersService.ShowPartyCountry();
		_countryCol.ShowInColumnChooser = _countryCol.Visible;
		_producerCol = _mtRetFromCustGrid.AddDropDownColumn(Definition.Grids["MTList"].Columns["Producer"], new Producer());
		_producerCol.Width = SizeCalculator.WidthForString(5);
		_producerCol.Visible = Settings.ParametersService.ShowPartyProducer();
		_producerCol.ShowInColumnChooser = _producerCol.Visible;
		PropertyList idv2 = new PropertyList();
		_property1Col = _mtRetFromCustGrid.AddDropDownCommentColumn(Definition.Grids["MTList"].Columns["Property1Caption"], idv2, 50);
		_property1Col.IsOwnProcessingClickDropDown = true;
		_property1Col.AllowEditing = DefaultBoolean.False;
		_property1Col.Width = SizeCalculator.WidthForString(25);
		_property1Col.Visible = Settings.ParametersService.ShowPartyProperty1();
		_property1Col.ShowInColumnChooser = _property1Col.Visible;
		_property2Col = _mtRetFromCustGrid.AddDropDownCommentColumn(Definition.Grids["MTList"].Columns["Property2Caption"], idv2, 50);
		_property2Col.IsOwnProcessingClickDropDown = true;
		_property2Col.AllowEditing = DefaultBoolean.False;
		_property2Col.Width = SizeCalculator.WidthForString(25);
		_property2Col.Visible = Settings.ParametersService.ShowPartyProperty2();
		_property2Col.ShowInColumnChooser = _property2Col.Visible;
		_rowDescCol = _mtRetFromCustGrid.AddStringColumn(Definition.Grids["MTList"].Columns["RowDescription"]);
		_rowDescCol.AllowEditing = DefaultBoolean.True;
		_buyerContractContainer.DocSummaControl = _totalSumma;
		_buyerContractContainer.DocCurrCodeControl = _currCode;
		LayoutGroup container9 = AddTabbedGroup(_tabs, ArmSoft.SME.Common.Resources.Supplier, "Supplier");
		_supplierName = AddStringControl(container9, Definition.Fields["SupplierName"], HorizontalAlignment.Stretch);
		_supplierLegalAddress = AddStringControl(container9, Definition.Fields["SupplierLegalAddress"], HorizontalAlignment.Stretch);
		_supplierBusinessAddress = AddDropDownCommentControl(container9, Definition.Fields["SupplierBusinessAddress"], new Storages(), HorizontalAlignment.Stretch);
		_supplierBusinessAddress.EditKeyValueChanged += new SafeDocumentEventHandler<object>(SupplierBusinessAddress_EditKeyValueChanged, this).Handler;
		_supplierBusinessAddress.MaxLength = 100;
		_supplierSettlAcc = AddDropDownControl(container9, Definition.Fields["SupplierSettlAcc"], new AccountInBanks(isKeySettlAccount: true), allowAdditionButtons: false, fieldWithCaption: true, HorizontalAlignment.Stretch);
		_supplierSettlAcc.CheckValue = false;
		_supplierTaxCode = AddStringControl(container9, Definition.Fields["SupplierTaxCode"], HorizontalAlignment.Stretch);
		IEnumerable<string> itemsSource = SmartEnum<ManagerPositionsSE, string>.List.Select((ManagerPositionsSE row) => row.Caption);
		_supplierMngPost = AddComboBoxControl(container9, Definition.Fields["SupplierMngPost"], itemsSource, HorizontalAlignment.Stretch);
		_supplierMngName = AddStringControl(container9, Definition.Fields["SupplierMngName"], HorizontalAlignment.Stretch);
		IEnumerable<string> itemsSource2 = SmartEnum<ChiefAccPositionsSE, string>.List.Select((ChiefAccPositionsSE row) => row.Caption);
		_supplierChiefAccPost = AddComboBoxControl(container9, Definition.Fields["SupplierChiefAccPost"], itemsSource2, HorizontalAlignment.Stretch);
		_supplierChiefAccName = AddStringControl(container9, Definition.Fields["SupplierChiefAccName"], HorizontalAlignment.Stretch);
		LayoutGroup container10 = AddTabbedGroup(_tabs, ArmSoft.SME.Common.Resources.Buyer, "Buyer");
		_buyerName = AddStringControl(container10, Definition.Fields["BuyerName"], HorizontalAlignment.Stretch);
		_buyerLegalAddress = AddStringControl(container10, Definition.Fields["BuyerLegalAddress"], HorizontalAlignment.Stretch);
		_buyerBusinessAddress = AddComboBoxControl(container10, Definition.Fields["BuyerBusinessAddress"], null, HorizontalAlignment.Stretch);
		_buyerSettlAcc = AddDropDownControl(container10, Definition.Fields["BuyerSettlAcc"], new PartnerAccountInBank(), allowAdditionButtons: false, fieldWithCaption: true, HorizontalAlignment.Stretch);
		_buyerSettlAcc.CheckValue = false;
		_buyerTaxCode = AddStringControl(container10, Definition.Fields["BuyerTaxCode"], HorizontalAlignment.Stretch);
		_buyerMngPost = AddComboBoxControl(container10, Definition.Fields["BuyerMngPost"], itemsSource, HorizontalAlignment.Stretch);
		_buyerMngName = AddStringControl(container10, Definition.Fields["BuyerMngName"], HorizontalAlignment.Stretch);
		_buyerChiefAccPost = AddComboBoxControl(container10, Definition.Fields["BuyerChiefAccPost"], itemsSource2, HorizontalAlignment.Stretch);
		_buyerChiefAccName = AddStringControl(container10, Definition.Fields["BuyerChiefAccName"], HorizontalAlignment.Stretch);
		_buyerStateRegNum = AddStringControl(container10, Definition.Fields["BuyerStateRegNum"], HorizontalAlignment.Stretch);
		_buyerIDDocType = AddSmartEnumDropDownControl(container10, Definition.Fields["BuyerIDDocType"], allowAdditionButtons: false, fieldWithCaption: true, HorizontalAlignment.Stretch);
		_buyerPassport = AddStringControl(container10, Definition.Fields["BuyerPassport"], HorizontalAlignment.Stretch);
		_buyerAdditionalData = AddComboBoxControl(container10, Definition.Fields["BuyerAdditionalData"], captionsList, HorizontalAlignment.Stretch);
		_buyerShippingMethod = AddComboBoxControl(container10, Definition.Fields["BuyerShippingMethod"], captionsList, HorizontalAlignment.Stretch);
		LayoutGroupExt layoutGroupExt = AddLayoutGroup(container10, LayoutGroupView.GroupBox, Orientation.Vertical, ArmSoft.SME.Common.Resources.Additional);
		IEnumerable<string> itemsSource3 = from row in TradeSettings.TradeCacheService.Partner.GetList()
			select row.Caption;
		_byWhomFio = AddComboBoxControl(layoutGroupExt, Definition.Fields["ByWhomFio"], itemsSource3, HorizontalAlignment.Stretch);
		_authorizationNumber = AddStringControl(layoutGroupExt, Definition.Fields["AuthorizationNumber"], HorizontalAlignment.Stretch);
		_authorizationDate = AddDateControl(layoutGroupExt, Definition.Fields["AuthorizationDate"]);
		layoutGroupExt.IsCollapsible = true;
		layoutGroupExt.IsCollapsed = true;
		LayoutGroup container11 = AddTabbedGroup(_tabs, ArmSoft.SME.Common.Resources.Additional, "Additional");
		_trust = AddStringControl(container11, Definition.Fields["Trust"], HorizontalAlignment.Stretch);
		_mediator = AddDropDownCommentControl(container11, Definition.Fields["Mediator"], new Employees(), HorizontalAlignment.Stretch);
		_machine = AddStringControl(container11, Definition.Fields["Machine"], HorizontalAlignment.Stretch);
		_chief = AddStringControl(container11, Definition.Fields["Chief"], HorizontalAlignment.Stretch);
		_allower = AddDropDownCommentControl(container11, Definition.Fields["Allower"], new Employees(), HorizontalAlignment.Stretch);
		_response = AddDropDownCommentControl(container11, Definition.Fields["Response"], new Employees(), HorizontalAlignment.Stretch);
		LayoutGroup container12 = AddTabbedGroup(_tabs, ArmSoft.SME.Common.Resources.PrintECRCheck, "ECRCheck");
		AddLayoutGroup(container12, LayoutGroupView.Group, Orientation.Horizontal);
		LayoutGroupExt container13 = AddLayoutGroup(container12, LayoutGroupView.Group, Orientation.Vertical);
		LayoutGroupExt container14 = AddLayoutGroup(container13, LayoutGroupView.Group, Orientation.Horizontal);
		LayoutGroupExt lgPartCont = AddLayoutGroup(container13, LayoutGroupView.Group, Orientation.Vertical);
		LayoutGroupExt container15 = AddLayoutGroup(container13, LayoutGroupView.Group, Orientation.Horizontal);
		LayoutGroupExt layoutGroupExt2 = AddLayoutGroup(container15, LayoutGroupView.GroupBox, Orientation.Vertical, ArmSoft.SME.Trade.Common.Resources.DocMTReturnFromCustomer);
		LayoutGroupExt layoutGroupExt3 = AddLayoutGroup(container15, LayoutGroupView.GroupBox, Orientation.Vertical, ArmSoft.SME.Trade.Common.Resources.BaseDocument);
		layoutGroupExt2.Height = 190.0;
		layoutGroupExt3.Height = layoutGroupExt2.Height;
		layoutGroupExt3.Width = Width / 1.7;
		layoutGroupExt2.Width = layoutGroupExt3.Width;
		_baseECRCheckNum = AddStringControl(layoutGroupExt3, Definition.Fields["ECRBaseCheckNum"]);
		_cashDesk = AddDropDownControl(container14, Definition.Fields["CashDesk"], new Cashes(withoutCheckAccess: true));
		_cashDesk.EditValueChanged += new SafeDocumentEventHandler<EditValueChangedEventArgs>(CashDesk_EditValueChanged, this).Handler;
		_bankAcquiringContainer = new PartnerContractGroupControls(this);
		_bankAcquiringContainer.ConstructUI(lgPartCont, _date, null, null, null, ArmSoft.SME.Trade.Common.Resources.POSTerminalPart, isFilterBySuppliers: false, isFilterByCustomers: false, isVisibleSummaPart: false, showOnlyBonusContracts: false, ArmSoft.SME.Trade.Common.Resources.POSTerminalContract, isShowContractCurrCode: false, isShowSummaDebt: false, showOnlyCurCodeNCC: true, isContractRequired: true, allowPartnerAdditionalButtons: false);
		((LayoutItemWithRequired)(object)_bankAcquiringContainer.PartnerGroup.Parent).Visibility = Visibility.Collapsed;
		_cashierShift = AddStringControl(container14, Definition.Fields["CashierShiftNumber"]);
		_returnECRCheckNum = AddStringControl(layoutGroupExt2, Definition.Fields["ECRReturnCheckNum"]);
		_ecrCheckNum = AddStringControl(layoutGroupExt2, Definition.Fields["ECRCheckNum"]);
		((LayoutItemWithRequired)(object)_ecrCheckNum.Parent).Visibility = Visibility.Collapsed;
		_ecrReturnByCashSum = AddNumericControl(layoutGroupExt2, Definition.Fields["ECRReturnByCashSum"]);
		_ecrReturnByCashSum.EditValueChanged += new SafeDocumentEventHandler<EditValueChangedEventArgs>(ECRReturn_CashOrCard_EditValueChanged, this).Handler;
		_ecrReturnByCashSum.Validate += new SafeEventHandler<ValidationEventArgs>(ECRReturnCash_Validate).Handler;
		_ecrReturnByCardSum = AddNumericControl(layoutGroupExt2, Definition.Fields["ECRReturnByCardSum"]);
		_ecrReturnByCardSum.EditValueChanged += new SafeDocumentEventHandler<EditValueChangedEventArgs>(ECRReturn_CashOrCard_EditValueChanged, this).Handler;
		_ecrReturnByCardSum.Validate += new SafeEventHandler<ValidationEventArgs>(ECRReturnCard_Validate).Handler;
		_ecrReturnByPrePaymentSum = AddNumericControl(layoutGroupExt2, Definition.Fields["ECRReturnByPrePaymentSum"]);
		_ecrReturnByPrePaymentSum.EditValueChanged += new SafeDocumentEventHandler<EditValueChangedEventArgs>(ECRReturn_CashOrCard_EditValueChanged, this).Handler;
		_ecrReturnByPrePaymentSum.Validate += new SafeEventHandler<ValidationEventArgs>(ECRReturnByPrePaymentSum_Validate).Handler;
		_baseECRByCashSum = AddNumericControl(layoutGroupExt3, Definition.Fields["BaseECRByCashSum"]);
		_baseECRByCardSum = AddNumericControl(layoutGroupExt3, Definition.Fields["BaseECRByCardSum"]);
		_baseECRByPrePaymentSum = AddNumericControl(layoutGroupExt3, Definition.Fields["BaseECRByPrePaymentSum"]);
		_returnECRTotalSum = AddNumericControl(layoutGroupExt2, ArmSoft.SME.Trade.Common.Resources.DocumentsTotal, isRequired: false, 16, 2);
		_printECRCheck = AddButton(layoutGroupExt2, null, ArmSoft.SME.Common.Resources.ECRC + " (F3)", ArmSoft.SME.Common.Resources.ECRCheck + " (F3)", "", isTabStop: true);
		_printECRCheck.Click += new SafeDocumentEventHandler<RoutedEventArgs>(PrintECRCheck_Click, this).Handler;
	}

	private void FillSupplierRekvizits()
	{
		_supplierName.Text = Settings.ParametersService.CompanyName();
		_supplierLegalAddress.Text = Settings.ParametersService.CompanyAddress();
		if (string.IsNullOrWhiteSpace(_supplierBusinessAddress.Text))
		{
			_supplierBusinessAddress.Text = Settings.ParametersService.CompanyBusinessAddress();
		}
		_supplierSettlAcc.Code = TradeSettings.TradeCacheService.AccountInBank.GetDefaultSettlementAccount(isSettlAccount: true);
		_supplierTaxCode.Text = Settings.ParametersService.TaxCode();
		_supplierMngPost.EditValue = Settings.ParametersService.ManagerPost();
		_supplierMngName.EditValue = Settings.ParametersService.ManagerName();
		_supplierChiefAccPost.EditValue = Settings.ParametersService.ChiefAccountantPost();
		_supplierChiefAccName.EditValue = Settings.ParametersService.ChiefAccountantName();
	}

	protected override void OnNew()
	{
		base.OnNew();
		_date.EditValue = Settings.HostingEnvironment.CurrentDate;
		_owner.Code = Settings.LoginService.UserName;
		CashDesk_EditValueForLoad();
		if (string.IsNullOrWhiteSpace(_cashDesk.Code) && (_baseISN == Guid.Empty || BaseDocument.POType == (short)ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTBill))
		{
			_cashDesk.Code = Settings.ParametersService.DefaultCash();
		}
		if (string.IsNullOrWhiteSpace(_storage.Code))
		{
			string text = Settings.ParametersService.DefaultStorage();
			if (!string.IsNullOrWhiteSpace(text) && TradeSettings.TradeCacheService.Storage.GetItem(text) != null && TradeSettings.CreateTradeClient().CommonServices.IsStorageAvailable(text))
			{
				_storage.Code = text;
			}
		}
		if (_viewMode != WindowViewMode.Copy)
		{
			_sumFromRound = 0m;
			_chief.EditValue = Settings.ParametersService.ChiefAccountantName();
			if (_baseISN == Guid.Empty)
			{
				string text2 = Settings.ParametersService.DefaultCustomer();
				if (!string.IsNullOrWhiteSpace(text2) && string.IsNullOrWhiteSpace(PO.PartnerContractGroup.PartnerCode))
				{
					PartnerCacheRow item = TradeSettings.TradeCacheService.Partner.GetItem(text2);
					if (item != null && item.Customer)
					{
						_buyerContractContainer.PartnerCode.Code = text2;
					}
				}
				_salesConsultant.Code = Settings.ParametersService.DefaultSalesConsultant();
				_priceListType.Code = Settings.ParametersService.DefaultSalePrice();
				if (_mtRetFromCustList.Count == 0)
				{
					FillSupplierRekvizits();
				}
			}
			if (Settings.ParametersService.VATType().InList("1", "2") == _vatType.Code.InList("1", "2", "5"))
			{
				_dealType.Code = Settings.ParametersService.DealType();
			}
		}
		MTBill mTBill;
		decimal? summaPlCard;
		if (BaseDocument != null && BaseDocument.POType == ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTBill.Value)
		{
			mTBill = (MTBill)BaseDocument;
			summaPlCard = mTBill.SummaPlCard;
			if ((summaPlCard.GetValueOrDefault() == 0m) & summaPlCard.HasValue)
			{
				summaPlCard = mTBill.SummaCredit;
				if ((summaPlCard.GetValueOrDefault() == 0m) & summaPlCard.HasValue)
				{
					summaPlCard = mTBill.SummaPayByBonus;
					if ((summaPlCard.GetValueOrDefault() == 0m) & summaPlCard.HasValue)
					{
						summaPlCard = mTBill.SummaPrePayment;
						if ((summaPlCard.GetValueOrDefault() == 0m) & summaPlCard.HasValue)
						{
							_ecrReturnByCashSum.Value = _returnECRTotalSum.Value;
							goto IL_03cc;
						}
					}
				}
			}
			summaPlCard = mTBill.SummaCash;
			if ((summaPlCard.GetValueOrDefault() == 0m) & summaPlCard.HasValue)
			{
				summaPlCard = mTBill.SummaCredit;
				if ((summaPlCard.GetValueOrDefault() == 0m) & summaPlCard.HasValue)
				{
					summaPlCard = mTBill.SummaPayByBonus;
					if ((summaPlCard.GetValueOrDefault() == 0m) & summaPlCard.HasValue)
					{
						summaPlCard = mTBill.SummaPrePayment;
						if ((summaPlCard.GetValueOrDefault() == 0m) & summaPlCard.HasValue)
						{
							_ecrReturnByCardSum.Value = _returnECRTotalSum.Value;
						}
					}
				}
			}
			goto IL_03cc;
		}
		if (BaseDocument == null && !string.IsNullOrWhiteSpace(_cashDesk.Code))
		{
			_ecrReturnByCashSum.Value = _returnECRTotalSum.Value;
		}
		goto IL_045c;
		IL_045c:
		if (!Settings.ParametersService.ShowGoodsCostInReportsAndDocs())
		{
			_costPriceCol.Visible = false;
			_costPriceCol.ShowInColumnChooser = false;
			_costPriceVATCol.Visible = false;
			_costPriceVATCol.ShowInColumnChooser = false;
			_currCostPriceCol.Visible = false;
			_currCostPriceCol.ShowInColumnChooser = false;
		}
		return;
		IL_03cc:
		summaPlCard = mTBill.SummaPlCard;
		if (!((summaPlCard.GetValueOrDefault() == 0m) & summaPlCard.HasValue))
		{
			_bankAcquiringContainer.PartnerCode.Code = mTBill.BankAcquiring.PartnerCode;
			_bankAcquiringContainer.Contract.Code = mTBill.BankAcquiring.ContractCode;
		}
		goto IL_045c;
	}

	protected override void OnNewAfterSave()
	{
		base.OnNewAfterSave();
		_mtRetFromCustList.Clear();
		_maxRowID = 0;
		_sumFromRound = 0m;
		BaseISN = Guid.Empty;
		_baseECRCheckNum.Text = "";
		_ecrCheckNum.Text = "";
		_returnECRCheckNum.Text = "";
		_cashDesk.Code = Settings.ParametersService.DefaultCash();
		_baseECRByCardSum.Value = 0m;
		_baseECRByCashSum.Value = 0m;
		_baseECRByPrePaymentSum.Value = 0m;
		_ecrReturnByCardSum.Value = 0m;
		_ecrReturnByPrePaymentSum.Value = 0m;
		CalcSummAndSummVAT();
		_owner.Code = Settings.LoginService.UserName;
	}

	protected override void OnEdit()
	{
		base.OnEdit();
		_setFocusOnActivation = _docNum;
		_buyerContractContainer.CalculatePartDebtRem();
	}

	protected override void StoreSettings(RegistryKey rk)
	{
		base.StoreSettings(rk);
		_mtRetFromCustGrid.StoreSettings(rk);
	}

	protected override void RestoreSettings(double defaultWidth, double defaultHeight, RegistryKey rk)
	{
		base.RestoreSettings(defaultWidth, defaultHeight, rk);
		_mtRetFromCustGrid.RestoreSettings(rk);
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
		if (!IsOfflineDoc)
		{
			_buyerContractContainer.ValueChangedForLoadDoc();
		}
		_mtRetFromCustList.Clear();
		using (List<MTReturnFromCustomerBase.MTRetFromCustSpecificationRow>.Enumerator enumerator = PO.MTList.GetEnumerator())
		{
			MTRetFromCustSpecificationRow item;
			MTReturnFromCustomerBase.MTRetFromCustSpecificationRow current;
			for (; enumerator.MoveNext(); item = new MTRetFromCustSpecificationRow(this)
			{
				ItemType = current.ItemType,
				Storage = current.Storage,
				ItemID = current.ItemID.Value,
				ItemCode = current.ItemCode,
				ItemName = current.ItemName,
				UnitCode = current.UnitCode,
				UnitBrief = current.UnitBrief,
				RowNum = current.RowNum.Value,
				RowID = current.RowID.Value,
				Quantity = current.Quantity.Value,
				Price = current.Price.Value,
				Discount = current.Discount.Value,
				DiscountPrice = current.DiscountPrice.Value,
				Summa = current.Summa.Value,
				DiscountSumma = current.DiscountSumma.Value,
				BeginSumma = current.BeginSumma.Value,
				BeginPrice = current.BeginPrice.Value,
				CostPrice = current.CostPrice.Value,
				CostPriceVAT = current.CostPriceVAT.Value,
				CurrCostPrice = current.CurrCostPrice.Value,
				VAT = current.VAT.Value,
				DealType = current.DealType,
				BaseRowID = current.BaseRowID.Value,
				RowDescription = current.RowDescription,
				UsedBonus = current.UsedBonus.Value,
				AccumulatedBonus = current.AccumulatedBonus.Value,
				UseLife = current.UseLife,
				Country = current.Country,
				Producer = current.Producer,
				ProducerCaption = current.ProducerCaption,
				Property1 = current.Property1,
				Property1Caption = current.Property1Caption,
				Property2 = current.Property2,
				Property2Caption = current.Property2Caption,
				Deleted = current.IsDeleted.Value,
				IsGift = current.IsGift.Value,
				IsStorageRequired = current.IsStorageRequired.Value,
				PartyBase = current.PartyBase,
				PartyRow = Convert.ToInt16(current.PartyRow),
				IsPartySelected = current.IsPartySelected
			}, _mtRetFromCustList.Add(item))
			{
				current = enumerator.Current;
				decimal? accumulatedBonus = current.AccumulatedBonus;
				if ((accumulatedBonus.GetValueOrDefault() == 0m) & accumulatedBonus.HasValue)
				{
					accumulatedBonus = current.UsedBonus;
					if ((accumulatedBonus.GetValueOrDefault() == 0m) & accumulatedBonus.HasValue)
					{
						continue;
					}
				}
				_bonusUsed = true;
			}
		}
		_mtRetFromCustGrid.ItemsSource = _mtRetFromCustList;
		_maxRowID = PO.MaxRowID;
		if (_currCode.Code == "AMD")
		{
			_returnECRTotalSum.Value = _totalSumma.Value;
		}
		else if (_currExChngRate.Base != 0m)
		{
			ExchangeRate exchangeRate = new ExchangeRate();
			exchangeRate.Rate = _currExChngRate.Rate;
			exchangeRate.Base = _currExChngRate.Base;
			_returnECRTotalSum.Value = ArmSoft.SME.Trade.Common.Helpers.PriceHelper.GetSumNNC(_totalSumma.Value, exchangeRate);
		}
		if (!(_baseISN == Guid.Empty) && !PO.StoredInDb && _viewMode != WindowViewMode.Copy)
		{
			CreateBasedOnDoc();
		}
		if (BaseDocument != null)
		{
			if (BaseDocument.POType == ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTInvoice.Value || BaseDocument.POType == ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTSaleRetail.Value)
			{
				MTInvoiceBase mTInvoiceBase = (MTInvoiceBase)BaseDocument;
				_baseDocList = mTInvoiceBase.MTList;
				_cashDesk.Code = string.Empty;
				_ecrReturnByCashSum.Value = 0m;
				_cashDesk.IsReadOnly = true;
				_ecrReturnByCashSum.IsReadOnly = true;
				_bankAcquiringContainer.PartnerCode.Code = string.Empty;
				_bankAcquiringContainer.PartnerCode.IsReadOnly = true;
				_bankAcquiringContainer.Contract.Code = string.Empty;
				_bankAcquiringContainer.Contract.IsReadOnly = true;
			}
			else if (BaseDocument.POType == ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTBill.Value)
			{
				MTBill mTBill = (MTBill)BaseDocument;
				_baseDocList = mTBill.MTList;
				if (string.IsNullOrWhiteSpace(_returnECRCheckNum.Text))
				{
					_cashDesk.IsReadOnly = false;
				}
				if (!string.IsNullOrWhiteSpace(mTBill.PrePaymentECRCheckNum))
				{
					throw new ASException(ArmSoft.SME.Trade.Common.Resources.ReturnToSupplierNotAllowedWithBonusAccumulation);
				}
				_baseECRByCardSum.Value = mTBill.SummaPlCard.Value;
				_baseECRByPrePaymentSum.Value = mTBill.SummaPrePayment.Value;
				_baseECRByCashSum.Value = mTBill.SummaCash.Value + mTBill.SummaCredit.Value + mTBill.SummaPayByBonus.Value - mTBill.SummaChange.Value;
				PopupCalcEditExt ecrReturnByCardSum = _ecrReturnByCardSum;
				decimal? accumulatedBonus = mTBill.SummaPlCard;
				ecrReturnByCardSum.IsReadOnly = (accumulatedBonus.GetValueOrDefault() == 0m) & accumulatedBonus.HasValue;
				_ecrReturnByCashSum.IsReadOnly = _baseECRByCashSum.Value == 0m;
				_ecrReturnByPrePaymentSum.IsReadOnly = mTBill.SummaPrePayment.Value == 0m;
				_bankAcquiringContainer.PartnerCode.Code = mTBill.BankAcquiring.PartnerCode;
				_bankAcquiringContainer.Contract.Code = mTBill.BankAcquiring.ContractCode;
				_bankAcquiringContainer.PartnerCode.IsReadOnly = false;
				_bankAcquiringContainer.Contract.IsReadOnly = false;
			}
		}
		else
		{
			_cashDesk.IsReadOnly = false;
			_bankAcquiringContainer.PartnerCode.Code = string.Empty;
			_bankAcquiringContainer.PartnerCode.IsReadOnly = true;
			_bankAcquiringContainer.Contract.Code = string.Empty;
			_bankAcquiringContainer.Contract.IsReadOnly = true;
		}
		CurrCodeChanged();
		VATType_EditValueChangedForLoad();
		SmartEnumDataProvider.DataRow dataRow = (SmartEnumDataProvider.DataRow)_dealType.DataView.GetRowByValue(_dealType.Code);
		if (dataRow != null)
		{
			_dealTypeKey = dataRow.Code;
		}
		if (!IsOfflineDoc)
		{
			_buyerContractContainer.ValueChangedForLoadDoc();
		}
		if (!IsOfflineDoc)
		{
			CalculatePartSumma();
		}
		BindPartnerDataViews();
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

	protected override void SetControlsReadonly()
	{
		base.SetControlsReadonly();
		_vatSumma.IsReadOnly = true;
		_totalSumma.IsReadOnly = true;
		_supplierName.IsReadOnly = true;
		_supplierLegalAddress.IsReadOnly = true;
		_supplierTaxCode.IsReadOnly = true;
		_cashierShift.IsReadOnly = true;
		_baseECRCheckNum.IsReadOnly = true;
		_returnECRCheckNum.IsReadOnly = true;
		_ecrCheckNum.IsReadOnly = true;
		_baseECRByCashSum.IsReadOnly = true;
		_baseECRByCardSum.IsReadOnly = true;
		_baseECRByPrePaymentSum.IsReadOnly = true;
		_returnECRTotalSum.IsReadOnly = true;
		if (_viewMode == WindowViewMode.View)
		{
			SetControlsDisabledAndReadOnly();
			if (!IsOfflineDoc)
			{
				_printECRCheck.IsEnabled = string.IsNullOrWhiteSpace(_returnECRCheckNum.Text) && BaseDocument?.POType == ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTBill.Value;
			}
		}
		else
		{
			SetOnlineModeDocReadOnlyControls();
		}
	}

	private void SetControlsDisabledAndReadOnly()
	{
		if (_codeEnter != null)
		{
			_codeEnter.IsEnabled = false;
		}
		if (_buyerContractContainer != null)
		{
			_buyerContractContainer.IsReadOnly = true;
		}
		if (_salesConsultant != null)
		{
			_salesConsultant.IsReadOnly = true;
		}
		if (_baseDocTaxInvoiceSeria != null)
		{
			_baseDocTaxInvoiceSeria.IsReadOnly = true;
		}
		if (_baseDocTaxInvoiceNumber != null)
		{
			_baseDocTaxInvoiceNumber.IsReadOnly = true;
		}
		if (_baseDocTaxInvoiceDate != null)
		{
			_baseDocTaxInvoiceDate.IsReadOnly = true;
		}
		if (_exportedType != null)
		{
			_exportedType.IsReadOnly = true;
		}
		if (_taxInvoiceSeries != null)
		{
			_taxInvoiceSeries.IsReadOnly = true;
		}
		if (_taxInvoiceNumber != null)
		{
			_taxInvoiceNumber.IsReadOnly = true;
		}
		if (_taxInvoiceDate != null)
		{
			_taxInvoiceDate.IsReadOnly = true;
		}
		if (_comment != null)
		{
			_comment.IsReadOnly = true;
		}
		if (_owner != null)
		{
			_owner.IsReadOnly = true;
		}
		if (_currCode != null)
		{
			_currCode.IsReadOnly = true;
		}
		if (_currExChngRate != null)
		{
			_currExChngRate.IsReadOnly = true;
		}
		if (_vatType != null)
		{
			_vatType.IsReadOnly = true;
		}
		if (_priceListType != null)
		{
			_priceListType.IsReadOnly = true;
		}
		if (_storage != null)
		{
			_storage.IsReadOnly = true;
		}
		if (_filteredSearch != null)
		{
			_filteredSearch.IsReadOnly = true;
			_filteredSearch.IsEnabled = false;
		}
		if (_fillLatestPurchasePrices != null)
		{
			_fillLatestPurchasePrices.IsReadOnly = true;
			_fillLatestPurchasePrices.IsEnabled = false;
		}
		if (_vatSumma != null)
		{
			_vatSumma.IsReadOnly = true;
		}
		if (_totalSumma != null)
		{
			_totalSumma.IsReadOnly = true;
		}
		if (_supplierName != null)
		{
			_supplierName.IsReadOnly = true;
		}
		if (_supplierLegalAddress != null)
		{
			_supplierLegalAddress.IsReadOnly = true;
		}
		if (_supplierBusinessAddress != null)
		{
			_supplierBusinessAddress.IsReadOnly = true;
		}
		if (_supplierSettlAcc != null)
		{
			_supplierSettlAcc.IsReadOnly = true;
		}
		if (_supplierTaxCode != null)
		{
			_supplierTaxCode.IsReadOnly = true;
		}
		if (_supplierMngPost != null)
		{
			_supplierMngPost.IsReadOnly = true;
		}
		if (_supplierMngName != null)
		{
			_supplierMngName.IsReadOnly = true;
		}
		if (_supplierChiefAccPost != null)
		{
			_supplierChiefAccPost.IsReadOnly = true;
		}
		if (_supplierChiefAccName != null)
		{
			_supplierChiefAccName.IsReadOnly = true;
		}
		if (_buyerName != null)
		{
			_buyerName.IsReadOnly = true;
		}
		if (_buyerLegalAddress != null)
		{
			_buyerLegalAddress.IsReadOnly = true;
		}
		if (_buyerBusinessAddress != null)
		{
			_buyerBusinessAddress.IsReadOnly = true;
		}
		if (_buyerSettlAcc != null)
		{
			_buyerSettlAcc.IsReadOnly = true;
		}
		if (_buyerTaxCode != null)
		{
			_buyerTaxCode.IsReadOnly = true;
		}
		if (_buyerMngPost != null)
		{
			_buyerMngPost.IsReadOnly = true;
		}
		if (_buyerMngName != null)
		{
			_buyerMngName.IsReadOnly = true;
		}
		if (_buyerChiefAccPost != null)
		{
			_buyerChiefAccPost.IsReadOnly = true;
		}
		if (_buyerChiefAccName != null)
		{
			_buyerChiefAccName.IsReadOnly = true;
		}
		if (_buyerStateRegNum != null)
		{
			_buyerStateRegNum.IsReadOnly = true;
		}
		if (_buyerIDDocType != null)
		{
			_buyerIDDocType.IsReadOnly = true;
		}
		if (_buyerPassport != null)
		{
			_buyerPassport.IsReadOnly = true;
		}
		if (_buyerAdditionalData != null)
		{
			_buyerAdditionalData.IsReadOnly = true;
		}
		if (_buyerShippingMethod != null)
		{
			_buyerShippingMethod.IsReadOnly = true;
		}
		if (_byWhomFio != null)
		{
			_byWhomFio.IsReadOnly = true;
		}
		if (_authorizationNumber != null)
		{
			_authorizationNumber.IsReadOnly = true;
		}
		if (_authorizationDate != null)
		{
			_authorizationDate.IsReadOnly = true;
		}
		if (_trust != null)
		{
			_trust.IsReadOnly = true;
		}
		if (_mediator != null)
		{
			_mediator.IsReadOnly = true;
		}
		if (_machine != null)
		{
			_machine.IsReadOnly = true;
		}
		if (_chief != null)
		{
			_chief.IsReadOnly = true;
		}
		if (_allower != null)
		{
			_allower.IsReadOnly = true;
		}
		if (_response != null)
		{
			_response.IsReadOnly = true;
		}
		if (_cashDesk != null)
		{
			_cashDesk.IsReadOnly = true;
		}
		if (_bankAcquiringContainer != null)
		{
			_bankAcquiringContainer.IsReadOnly = true;
		}
		if (_cashierShift != null)
		{
			_cashierShift.IsReadOnly = true;
		}
		if (_baseECRCheckNum != null)
		{
			_baseECRCheckNum.IsReadOnly = true;
		}
		if (_returnECRCheckNum != null)
		{
			_returnECRCheckNum.IsReadOnly = true;
		}
		if (_ecrCheckNum != null)
		{
			_ecrCheckNum.IsReadOnly = true;
		}
		if (_returnECRTotalSum != null)
		{
			_returnECRTotalSum.IsReadOnly = true;
		}
		if (_ecrReturnByCardSum != null)
		{
			_ecrReturnByCardSum.IsReadOnly = true;
		}
		if (_ecrReturnByCashSum != null)
		{
			_ecrReturnByCashSum.IsReadOnly = true;
		}
		if (_ecrReturnByPrePaymentSum != null)
		{
			_ecrReturnByPrePaymentSum.IsReadOnly = true;
		}
		if (_baseECRByPrePaymentSum != null)
		{
			_baseECRByPrePaymentSum.IsReadOnly = true;
			_baseECRByPrePaymentSum.IsEnabled = false;
		}
		if (_baseECRByCardSum != null)
		{
			_baseECRByCardSum.IsReadOnly = true;
		}
		if (_baseECRByCashSum != null)
		{
			_baseECRByCashSum.IsReadOnly = true;
		}
		if (_barSubItemJournals != null)
		{
			_barSubItemJournals.IsEnabled = false;
		}
		if (_barButtonPrintLabel != null)
		{
			_barButtonPrintLabel.IsEnabled = false;
		}
		if (_barButtonJumpTo != null)
		{
			_barButtonJumpTo.IsEnabled = false;
		}
		if (_barButtonMTSalesAnalysis != null)
		{
			_barButtonMTSalesAnalysis.IsEnabled = false;
		}
		if (_barButtonGoodsOperationsJournal != null)
		{
			_barButtonGoodsOperationsJournal.IsEnabled = false;
		}
		if (_barButtonGoodsRetailOperationJournal != null)
		{
			_barButtonGoodsRetailOperationJournal.IsEnabled = false;
		}
		if (_barButtonPartnerOperationJournal != null)
		{
			_barButtonPartnerOperationJournal.IsEnabled = false;
		}
		if (_barButtonBonusOperationJournal != null)
		{
			_barButtonBonusOperationJournal.IsEnabled = false;
		}
		if (_barButtonCashOperationJournal != null)
		{
			_barButtonCashOperationJournal.IsEnabled = false;
		}
	}

	private void SetOnlineModeDocReadOnlyControls()
	{
		bool flag = State == ArmSoft.SME.Trade.Common.SmartEnums.DocumentStateSE.Draft.Value || State == ArmSoft.SME.Trade.Common.SmartEnums.DocumentStateSE.Deleted.Value;
		bool flag2 = _viewMode == WindowViewMode.Edit && !flag;
		_date.IsReadOnly = flag2;
		_baseDocumentControl.IsReadOnly = flag2;
		_currCode.IsReadOnly = flag2;
		_currExChngRate.IsReadOnly = flag2 || _currCode.Code == "AMD";
		_vatType.IsReadOnly = flag2;
		_dealType.IsReadOnly = flag2;
		_priceListType.IsReadOnly = flag2;
		_storage.IsReadOnly = flag2;
		_barCode.IsReadOnly = flag2;
		_codeEnter.IsEnabled = !flag2;
		_mtRetFromCustGrid.IsReadOnly = flag2;
		_printECRCheck.IsEnabled = string.IsNullOrWhiteSpace(_returnECRCheckNum.Text) && BaseDocument?.POType == ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTBill.Value;
		if (!_ecrReturnByCardSum.IsReadOnly)
		{
			_ecrReturnByCardSum.IsReadOnly = !_printECRCheck.IsEnabled;
		}
		if (!_ecrReturnByCashSum.IsReadOnly)
		{
			_ecrReturnByCashSum.IsReadOnly = !_printECRCheck.IsEnabled;
		}
		if (!_ecrReturnByPrePaymentSum.IsReadOnly)
		{
			_ecrReturnByPrePaymentSum.IsReadOnly = !_printECRCheck.IsEnabled;
		}
		if (!_bankAcquiringContainer.PartnerCode.IsReadOnly)
		{
			_bankAcquiringContainer.PartnerCode.IsReadOnly = !_printECRCheck.IsEnabled;
			_bankAcquiringContainer.Contract.IsReadOnly = !_printECRCheck.IsEnabled;
		}
		if (BaseISN != Guid.Empty)
		{
			_itemCodeCol.ReadOnly = true;
			_unitCodeCol.ReadOnly = true;
			_mtRetFromCustGrid.AllowAdd = false;
			_mtRetFromCustGrid.AllowDuplicate = false;
			_usedBonusSummCol.Visible = _bonusUsed;
			_usedBonusSummCol.ShowInColumnChooser = _bonusUsed;
			_accumulatedBonusCol.Visible = _bonusUsed;
			_accumulatedBonusCol.ShowInColumnChooser = _bonusUsed;
		}
		else
		{
			_ecrReturnByCashSum.IsReadOnly = string.IsNullOrWhiteSpace(_cashDesk.Code);
			_mtRetFromCustGrid.AllowAdd = true;
			_mtRetFromCustGrid.AllowDuplicate = true;
		}
	}

	protected override bool AllowChangeGridMode()
	{
		return true;
	}

	private void MTRetFromCustGrid_DeletedRow(object sender, EventArgs e)
	{
		CalcSummAndSummVAT();
		_mtRetFromCustGrid.Grid.RefreshData();
	}

	private void MTRetFromCustGrid_AddingRow(object sender, DocumentGridAddingRowEventArgs e)
	{
		_mtRetFromCustList.Add(new MTRetFromCustSpecificationRow(this)
		{
			ItemType = _goodsTypes_Goods,
			Storage = _storage.Code,
			RowID = _maxRowID,
			DealType = _dealTypeKey
		});
		_maxRowID++;
		e.RowNumber = _mtRetFromCustList.Count - 1;
	}

	private void MTRetFromCustGrid_DuplicatingRow(object sender, DocumentGridDuplicatingRowEventArgs e)
	{
		MTRetFromCustSpecificationRow mTRetFromCustSpecificationRow = (MTRetFromCustSpecificationRow)_mtRetFromCustGrid.Grid.GetRow(e.RowHandle);
		if (mTRetFromCustSpecificationRow != null)
		{
			MTRetFromCustSpecificationRow mTRetFromCustSpecificationRow2 = new MTRetFromCustSpecificationRow(this);
			mTRetFromCustSpecificationRow.Clone(mTRetFromCustSpecificationRow2);
			mTRetFromCustSpecificationRow2.RowID = _maxRowID;
			_mtRetFromCustList.Add(mTRetFromCustSpecificationRow2);
			_maxRowID++;
			e.NewRowNumber = _mtRetFromCustList.Count - 1;
			_mtRetFromCustGrid.Grid.RefreshData();
			CalcSummAndSummVAT();
		}
	}

	private void MTRetFromCustGrid_MovingRowUp(object sender, DocumentGridMovingRowUpOrDownEventArgs e)
	{
		if (_mtRetFromCustGrid.Grid.SortInfo.Count > 0 || _mtRetFromCustTableView.Grid.VisibleRowCount != _mtRetFromCustList.Count)
		{
			throw new ASException(ArmSoft.SME.Common.Resources.NotAllowRowMoveInSortedGrid);
		}
		MTRetFromCustSpecificationRow mTRetFromCustSpecificationRow = (MTRetFromCustSpecificationRow)_mtRetFromCustGrid.Grid.GetRow(e.RowHandle);
		if (mTRetFromCustSpecificationRow != null)
		{
			_mtRetFromCustList.RemoveAt(e.RowHandle);
			_mtRetFromCustList.Insert(e.RowHandle - 1, mTRetFromCustSpecificationRow);
		}
		_mtRetFromCustGrid.Grid.RefreshData();
	}

	private void MTRetFromCustGrid_MovingRowDown(object sender, DocumentGridMovingRowUpOrDownEventArgs e)
	{
		if (_mtRetFromCustGrid.Grid.SortInfo.Count > 0 || _mtRetFromCustTableView.Grid.VisibleRowCount != _mtRetFromCustList.Count)
		{
			throw new ASException(ArmSoft.SME.Common.Resources.NotAllowRowMoveInSortedGrid);
		}
		MTRetFromCustSpecificationRow mTRetFromCustSpecificationRow = (MTRetFromCustSpecificationRow)_mtRetFromCustGrid.Grid.GetRow(e.RowHandle);
		if (mTRetFromCustSpecificationRow != null)
		{
			_mtRetFromCustList.RemoveAt(e.RowHandle);
			_mtRetFromCustList.Insert(e.RowHandle + 1, mTRetFromCustSpecificationRow);
		}
		_mtRetFromCustGrid.Grid.RefreshData();
	}

	private void MTRetFromCustGrid_AddingRowWithDialog(object sender, DocumentGridAddingRowEventArgs e)
	{
		if (!(BaseISN == Guid.Empty))
		{
			return;
		}
		if (_date.DoValidate() && _date.EditValue.HasValue)
		{
			if (_storage.DoValidate() && _vatType.DoValidate() && _dealType.DoValidate() && _priceListType.DoValidate())
			{
				MTRetFromCustRowAddEditDialog mTRetFromCustRowAddEditDialog = new MTRetFromCustRowAddEditDialog(DialogWindowMode.NewModeWithNext, this);
				mTRetFromCustRowAddEditDialog.Invoke();
				int addedRows = mTRetFromCustRowAddEditDialog.AddedRows;
				if (addedRows != 0)
				{
					_maxRowID += addedRows;
					_mtRetFromCustGrid.Grid.RefreshData();
					CalcSummAndSummVAT();
				}
			}
		}
		else
		{
			ControlsHelper.SetFocus(_date);
			ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Common.Resources.ValidateEmptyDate, ArmSoft.SME.Trade.Common.Resources.ProgramName, MessageBoxButton.OK, MessageBoxImage.Exclamation);
		}
	}

	private void MTRetFromCustGrid_DuplicatingRowWithDialog(object sender, DocumentGridDuplicatingRowEventArgs e)
	{
		if (!(BaseISN == Guid.Empty))
		{
			return;
		}
		if (_date.DoValidate() && _date.EditValue.HasValue)
		{
			MTRetFromCustSpecificationRow mTRetFromCustSpecificationRow = (MTRetFromCustSpecificationRow)_mtRetFromCustGrid.Grid.GetRow(e.RowHandle);
			if (mTRetFromCustSpecificationRow != null)
			{
				MTRetFromCustRowAddEditDialog mTRetFromCustRowAddEditDialog = new MTRetFromCustRowAddEditDialog(DialogWindowMode.NewModeWithNext, this, mTRetFromCustSpecificationRow);
				mTRetFromCustRowAddEditDialog.Invoke();
				int addedRows = mTRetFromCustRowAddEditDialog.AddedRows;
				if (addedRows != 0)
				{
					_maxRowID += addedRows;
					_mtRetFromCustGrid.Grid.RefreshData();
					CalcSummAndSummVAT();
				}
			}
		}
		else
		{
			ControlsHelper.SetFocus(_date);
			ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Common.Resources.ValidateEmptyDate, ArmSoft.SME.Trade.Common.Resources.ProgramName, MessageBoxButton.OK, MessageBoxImage.Exclamation);
		}
	}

	private void MTRetFromCustGrid_EditingRowWithDialog(object sender, DocumentGridEditingRowEventArgs e)
	{
		if (_date.DoValidate() && _date.EditValue.HasValue)
		{
			MTRetFromCustSpecificationRow mTRetFromCustSpecificationRow = (MTRetFromCustSpecificationRow)_mtRetFromCustGrid.Grid.GetRow(e.RowHandle);
			if (mTRetFromCustSpecificationRow != null)
			{
				MTRetFromCustRowAddEditDialog mTRetFromCustRowAddEditDialog = new MTRetFromCustRowAddEditDialog(e.AllowEdit ? DialogWindowMode.EditMode : DialogWindowMode.ViewMode, this, mTRetFromCustSpecificationRow);
				e.RowEdited = Convert.ToBoolean(mTRetFromCustRowAddEditDialog.Invoke());
				if (e.AllowEdit && e.RowEdited)
				{
					_mtRetFromCustGrid.Grid.RefreshData();
					CalcSummAndSummVAT();
				}
			}
		}
		else
		{
			ControlsHelper.SetFocus(_date);
			ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Common.Resources.ValidateEmptyDate, ArmSoft.SME.Trade.Common.Resources.ProgramName, MessageBoxButton.OK, MessageBoxImage.Exclamation);
		}
	}

	private void MTRetFromCustGrid_ClickDropDown(object sender, DocumentGridClickDropDownEventArgs e)
	{
		try
		{
			MTRetFromCustSpecificationRow mTRetFromCustSpecificationRow = (MTRetFromCustSpecificationRow)_mtRetFromCustGrid.Grid.GetRow(e.RowHandle);
			if (e.Column == _itemCodeCol)
			{
				GoodsFilter goodsFilter = new GoodsFilter(shortMode: true, (mTRetFromCustSpecificationRow.ItemType == ItemTypesSE.Services.Value) ? "2" : "1", "", isShowOnlyInPriceList: true);
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
			else if (e.Column == _storageCol)
			{
				if (mTRetFromCustSpecificationRow.ItemType != _goodsTypes_Goods)
				{
					return;
				}
				Storages storages = new Storages();
				ModalBrowser modalBrowser2 = new ModalBrowser(storages)
				{
					LookUpValue = e.Value,
					Owner = e.Owner,
					Left = e.Left,
					Top = e.Top
				};
				if (modalBrowser2.Show())
				{
					e.NewValue = storages.ValueMember(modalBrowser2.SelectedRowIndex);
				}
			}
			else if (e.Column == _dealTypeCol)
			{
				if (mTRetFromCustSpecificationRow == null || (_vatType.Code != "3" && _vatType.Code != "4" && mTRetFromCustSpecificationRow.VAT.Value))
				{
					return;
				}
				SmartEnum smartEnum = new SmartEnum(_vatType.Code.InList("1", "2", "5") ? typeof(InvoiceDealTypesSE) : typeof(AccDealTypesSE), null, padLeft: false, '0', allSubTypes: false, ValueMember.ValueUI);
				ModalBrowser modalBrowser3 = new ModalBrowser(smartEnum)
				{
					LookUpValue = e.Value,
					Owner = e.Owner,
					Left = e.Left,
					Top = e.Top
				};
				if (modalBrowser3.Show())
				{
					mTRetFromCustSpecificationRow.DealType = (string)smartEnum.KeyMember(modalBrowser3.SelectedRowIndex);
				}
			}
			else if (e.Column == _property1Col || e.Column == _property2Col)
			{
				if (mTRetFromCustSpecificationRow == null)
				{
					return;
				}
				MaterialCacheRow item = TradeSettings.TradeCacheService.Material.GetItem(mTRetFromCustSpecificationRow.ItemCode);
				if (item == null || (string.IsNullOrWhiteSpace(item.Property1) && e.Column == _property1Col) || (string.IsNullOrWhiteSpace(item.Property2) && e.Column == _property2Col))
				{
					return;
				}
				PropertyList propertyList = new PropertyList((e.Column == _property1Col) ? item.Property1 : item.Property2);
				ModalBrowser modalBrowser4 = new ModalBrowser(propertyList)
				{
					LookUpValue = e.Value,
					Owner = e.Owner,
					Left = e.Left,
					Top = e.Top
				};
				if (modalBrowser4.Show())
				{
					e.NewValue = propertyList.CommentMember(modalBrowser4.SelectedRowIndex);
					if (e.Column == _property1Col)
					{
						mTRetFromCustSpecificationRow.Property1 = propertyList.ValueMember(modalBrowser4.SelectedRowIndex);
					}
					else
					{
						mTRetFromCustSpecificationRow.Property2 = propertyList.ValueMember(modalBrowser4.SelectedRowIndex);
					}
				}
			}
			_mtRetFromCustGrid.Grid.RefreshData();
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	private void MTRetFromCustTableView_CellValueChanged(object sender, CellValueChangedEventArgs e)
	{
		MTRetFromCustSpecificationRow mTRetFromCustSpecificationRow = (MTRetFromCustSpecificationRow)_mtRetFromCustGrid.Grid.GetRow(e.RowHandle);
		if (mTRetFromCustSpecificationRow == null)
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
				mTRetFromCustSpecificationRow.Storage = "";
				mTRetFromCustSpecificationRow.ItemCode = "";
				mTRetFromCustSpecificationRow.ItemID = -1;
				mTRetFromCustSpecificationRow.ItemName = "";
				mTRetFromCustSpecificationRow.Quantity = 0m;
				mTRetFromCustSpecificationRow.BeginPrice = 0m;
				mTRetFromCustSpecificationRow.Discount = 0m;
				mTRetFromCustSpecificationRow.Price = 0m;
				mTRetFromCustSpecificationRow.UnitBrief = "";
				mTRetFromCustSpecificationRow.CostPrice = 0m;
				mTRetFromCustSpecificationRow.Summa = 0m;
				mTRetFromCustSpecificationRow.VAT = false;
				mTRetFromCustSpecificationRow.BeginSumma = 0m;
				if (text2 != ItemTypesSE.Goods.Value && text2 != ItemTypesSE.Services.Value)
				{
					mTRetFromCustSpecificationRow.ItemType = text;
				}
				if (mTRetFromCustSpecificationRow.ItemType == ItemTypesSE.Goods.Value && string.IsNullOrWhiteSpace(mTRetFromCustSpecificationRow.Storage))
				{
					mTRetFromCustSpecificationRow.Storage = _storage.Code;
				}
				ChangeItemCodeColumnAdditonalView(text2, "");
			}
		}
		else if (e.Column == _itemCodeCol)
		{
			try
			{
				if (e.Value == null)
				{
					mTRetFromCustSpecificationRow.ItemID = -1;
					mTRetFromCustSpecificationRow.ItemName = "";
					mTRetFromCustSpecificationRow.UnitCode = "";
					mTRetFromCustSpecificationRow.UnitBrief = "";
				}
				else
				{
					string text3 = e.Value.ToString();
					if (string.IsNullOrWhiteSpace(text3))
					{
						throw new Exception();
					}
					MaterialCacheRow item = TradeSettings.TradeCacheService.Material.GetItem(text3, throwExceptionNotExits: true);
					mTRetFromCustSpecificationRow.ItemType = item.Type;
					mTRetFromCustSpecificationRow.ItemID = item.Id;
					mTRetFromCustSpecificationRow.ItemName = item.Caption;
					mTRetFromCustSpecificationRow.UnitCode = item.Unit;
					mTRetFromCustSpecificationRow.UnitBrief = item.UnitRow.Brief;
					_unitCodeCol.DataView = new MTQntUnits(item.MTCode);
					mTRetFromCustSpecificationRow.VAT = item.VAT;
					mTRetFromCustSpecificationRow.Country = item.Country;
					mTRetFromCustSpecificationRow.Producer = item.Producer;
					mTRetFromCustSpecificationRow.Price = TradeSettings.CreateTradeClient().CommonServices.GetPriceForGivenPriceListAndVATType(_priceListType.Code, _vatType.Code, mTRetFromCustSpecificationRow.ItemID, mTRetFromCustSpecificationRow.UnitCode, _date.EditValue.Value, _currCode.Code);
					mTRetFromCustSpecificationRow.Summa = ArmSoft.SME.Trade.UI.Helpers.PriceHelper.CalculateSum(mTRetFromCustSpecificationRow.Price, mTRetFromCustSpecificationRow.Quantity);
					mTRetFromCustSpecificationRow.UsedBonus = 0m;
					mTRetFromCustSpecificationRow.AccumulatedBonus = 0m;
					if (!string.IsNullOrWhiteSpace(_dealTypeKey))
					{
						if (_vatType.Code == "3" || _vatType.Code == "4" || !item.VAT)
						{
							mTRetFromCustSpecificationRow.DealType = _dealTypeKey;
						}
						else
						{
							mTRetFromCustSpecificationRow.DealType = string.Empty;
						}
					}
				}
			}
			catch (Exception ex)
			{
				mTRetFromCustSpecificationRow.ItemName = "";
				mTRetFromCustSpecificationRow.ItemID = -1;
				mTRetFromCustSpecificationRow.UnitCode = "";
				mTRetFromCustSpecificationRow.UnitBrief = "";
				mTRetFromCustSpecificationRow.Quantity = 0m;
				mTRetFromCustSpecificationRow.Summa = 0m;
				mTRetFromCustSpecificationRow.Price = 0m;
				mTRetFromCustSpecificationRow.CostPrice = 0m;
				mTRetFromCustSpecificationRow.CostPriceVAT = 0m;
				mTRetFromCustSpecificationRow.CurrCostPrice = 0m;
				mTRetFromCustSpecificationRow.VAT = false;
				mTRetFromCustSpecificationRow.Country = "";
				mTRetFromCustSpecificationRow.Producer = "";
				mTRetFromCustSpecificationRow.Property1 = "";
				mTRetFromCustSpecificationRow.Property2 = "";
				mTRetFromCustSpecificationRow.UsedBonus = 0m;
				mTRetFromCustSpecificationRow.AccumulatedBonus = 0m;
				if (!string.IsNullOrWhiteSpace(text2))
				{
					Settings.ErrorHandler(ex);
				}
			}
		}
		else if (e.Column == _storageCol)
		{
			if (mTRetFromCustSpecificationRow.ItemType != ItemTypesSE.Goods.Value)
			{
				return;
			}
			if (e.Value != null && !string.IsNullOrWhiteSpace(e.Value.ToString()) && TradeSettings.TradeCacheService.Storage.GetItem(e.Value.ToString()) != null && !TradeSettings.CreateTradeClient().CommonServices.IsStorageAvailable(e.Value.ToString()))
			{
				ArmSoft.SME.Core.UI.MessageBox.Show(string.Format(ArmSoft.SME.Trade.Common.Resources.StorageNotAvailForUser, Settings.LoginService.UserName, e.Value.ToString()), ArmSoft.SME.Trade.Common.Resources.ProgramName, MessageBoxButton.OK, MessageBoxImage.Exclamation);
				mTRetFromCustSpecificationRow.Storage = text;
			}
		}
		else if (e.Column == _unitCodeCol)
		{
			try
			{
				if (e.Value != null && !string.IsNullOrWhiteSpace(e.Value.ToString()) && !string.IsNullOrWhiteSpace(mTRetFromCustSpecificationRow.ItemCode))
				{
					QntUnitCacheRow item2 = TradeSettings.TradeCacheService.QntUnit.GetItem(e.Value.ToString(), throwExceptionNotExits: true);
					mTRetFromCustSpecificationRow.UnitBrief = ((item2 == null) ? "" : item2.Brief);
					if (string.IsNullOrWhiteSpace(_priceListType.Code))
					{
						mTRetFromCustSpecificationRow.Price = ArmSoft.SME.Trade.UI.Helpers.PriceHelper.GetPriceForUnit(text, e.Value.ToString(), mTRetFromCustSpecificationRow.Price, mTRetFromCustSpecificationRow.ItemCode);
					}
					else
					{
						decimal priceForGivenPriceListAndVATType = TradeSettings.CreateTradeClient().CommonServices.GetPriceForGivenPriceListAndVATType(_priceListType.Code, _vatType.Code, mTRetFromCustSpecificationRow.ItemID, e.Value.ToString(), _date.EditValue.Value, _currCode.Code);
						if (priceForGivenPriceListAndVATType != 0m)
						{
							mTRetFromCustSpecificationRow.Price = priceForGivenPriceListAndVATType;
						}
						else
						{
							mTRetFromCustSpecificationRow.Price = ArmSoft.SME.Trade.UI.Helpers.PriceHelper.GetPriceForUnit(text, e.Value.ToString(), mTRetFromCustSpecificationRow.Price, mTRetFromCustSpecificationRow.ItemCode);
						}
					}
					mTRetFromCustSpecificationRow.Summa = ArmSoft.SME.Trade.UI.Helpers.PriceHelper.CalculateSum(mTRetFromCustSpecificationRow.Price, mTRetFromCustSpecificationRow.Quantity);
				}
				if (mTRetFromCustSpecificationRow.CostPrice != 0m && !string.IsNullOrWhiteSpace(e.Value.ToString()))
				{
					mTRetFromCustSpecificationRow.CostPrice = ArmSoft.SME.Trade.UI.Helpers.PriceHelper.GetPriceForUnit(text, e.Value.ToString(), mTRetFromCustSpecificationRow.CostPrice, mTRetFromCustSpecificationRow.ItemCode);
					if (mTRetFromCustSpecificationRow.VAT.Value)
					{
						mTRetFromCustSpecificationRow.CostPriceVAT = (mTRetFromCustSpecificationRow.CostPrice * (100m + _tradeConstVAT) / 100m).RoundAs(2);
					}
					else
					{
						mTRetFromCustSpecificationRow.CostPriceVAT = mTRetFromCustSpecificationRow.CostPrice;
					}
					ExchangeRate exchangeRate = new ExchangeRate();
					exchangeRate.Rate = _paramCurrExChngRate;
					exchangeRate.Base = _paramCurrExChngBase;
					mTRetFromCustSpecificationRow.CurrCostPrice = ArmSoft.SME.Trade.Common.Helpers.PriceHelper.GetSumCurr(mTRetFromCustSpecificationRow.CostPriceVAT, exchangeRate);
				}
			}
			catch (Exception ex2)
			{
				Settings.ErrorHandler(ex2);
			}
		}
		else if (e.Column == _quantityCol)
		{
			mTRetFromCustSpecificationRow.Summa = ArmSoft.SME.Trade.UI.Helpers.PriceHelper.CalculateSum(mTRetFromCustSpecificationRow.Price, Convert.ToDecimal(e.Value));
			if (mTRetFromCustSpecificationRow.ItemType == ItemTypesSE.Goods.Value && Convert.ToDecimal(e.OldValue) != 0m && Convert.ToDecimal(e.Value) != 0m)
			{
				mTRetFromCustSpecificationRow.UsedBonus = (Convert.ToDecimal(e.Value) * mTRetFromCustSpecificationRow.UsedBonus / Convert.ToDecimal(e.OldValue)).RoundAs(2);
				mTRetFromCustSpecificationRow.AccumulatedBonus = (Convert.ToDecimal(e.Value) * mTRetFromCustSpecificationRow.AccumulatedBonus / Convert.ToDecimal(e.OldValue)).RoundAs(2);
			}
		}
		else if (e.Column == _priceCol)
		{
			mTRetFromCustSpecificationRow.Summa = ArmSoft.SME.Trade.UI.Helpers.PriceHelper.CalculateSum(Convert.ToDecimal(e.Value), mTRetFromCustSpecificationRow.Quantity);
		}
		else if (e.Column == _summaCol && mTRetFromCustSpecificationRow.Quantity != 0m)
		{
			mTRetFromCustSpecificationRow.Price = (Convert.ToDecimal(e.Value) / mTRetFromCustSpecificationRow.Quantity).RoundAs(2);
		}
		else if (e.Column == _costPriceCol)
		{
			mTRetFromCustSpecificationRow.CostPrice = Convert.ToDecimal(e.Value);
			if (mTRetFromCustSpecificationRow.VAT.Value)
			{
				mTRetFromCustSpecificationRow.CostPriceVAT = (mTRetFromCustSpecificationRow.CostPrice * (100m + _tradeConstVAT) / 100m).RoundAs(2);
			}
			else
			{
				mTRetFromCustSpecificationRow.CostPriceVAT = mTRetFromCustSpecificationRow.CostPrice;
			}
			mTRetFromCustSpecificationRow.CurrCostPrice = (mTRetFromCustSpecificationRow.CostPriceVAT * _paramCurrExChngBase / _paramCurrExChngRate).RoundAs(2);
		}
		else if (e.Column == _costPriceVATCol)
		{
			mTRetFromCustSpecificationRow.CostPriceVAT = Convert.ToDecimal(e.Value);
			mTRetFromCustSpecificationRow.CostPrice = (mTRetFromCustSpecificationRow.CostPriceVAT * 100m / (100m + _tradeConstVAT)).RoundAs(2);
			ExchangeRate exchangeRate2 = new ExchangeRate();
			exchangeRate2.Rate = _paramCurrExChngRate;
			exchangeRate2.Base = _paramCurrExChngBase;
			mTRetFromCustSpecificationRow.CurrCostPrice = ArmSoft.SME.Trade.Common.Helpers.PriceHelper.GetSumCurr(mTRetFromCustSpecificationRow.CostPriceVAT, exchangeRate2);
		}
		else if (e.Column == _vatCol)
		{
			mTRetFromCustSpecificationRow.VAT = Convert.ToBoolean(e.Value);
			if (mTRetFromCustSpecificationRow.VAT.Value)
			{
				mTRetFromCustSpecificationRow.CostPriceVAT = (mTRetFromCustSpecificationRow.CostPrice * (100m + _tradeConstVAT) / 100m).RoundAs(2);
			}
			else
			{
				mTRetFromCustSpecificationRow.CostPriceVAT = mTRetFromCustSpecificationRow.CostPrice;
			}
			ExchangeRate exchangeRate3 = new ExchangeRate();
			exchangeRate3.Rate = _paramCurrExChngRate;
			exchangeRate3.Base = _paramCurrExChngBase;
			mTRetFromCustSpecificationRow.CurrCostPrice = ArmSoft.SME.Trade.Common.Helpers.PriceHelper.GetSumCurr(mTRetFromCustSpecificationRow.CostPriceVAT, exchangeRate3);
			if ((bool)e.Value)
			{
				mTRetFromCustSpecificationRow.DealType = string.Empty;
			}
		}
		_mtRetFromCustGrid.Grid.RefreshData();
		CalcSummAndSummVAT();
	}

	private void MTRetFromCustTableView_FocusedRowHandleChangedEventArgs(object sender, FocusedRowHandleChangedEventArgs e)
	{
		int focusedRowHandle = _mtRetFromCustTableView.FocusedRowHandle;
		MTRetFromCustSpecificationRow mTRetFromCustSpecificationRow = (MTRetFromCustSpecificationRow)_mtRetFromCustGrid.Grid.GetRow(focusedRowHandle);
		if (mTRetFromCustSpecificationRow != null)
		{
			ChangeItemCodeColumnAdditonalView(mTRetFromCustSpecificationRow.ItemType, mTRetFromCustSpecificationRow.ItemCode);
		}
	}

	private void ChangeItemCodeColumnAdditonalView(string typeValue, string itemCode)
	{
		ItemTypesSE itemTypesSE = ItemTypesSE.All;
		if (typeValue == _goodsTypes_Goods)
		{
			itemTypesSE = ItemTypesSE.Goods;
		}
		else if (typeValue == ItemTypesSE.Services.Value)
		{
			itemTypesSE = ItemTypesSE.Services;
		}
		_itemCodeCol.DataView = new GoodsFilter(shortMode: true, itemTypesSE.Value, "", isShowOnlyInPriceList: true);
		_unitCodeCol.DataView = new MTQntUnits(itemCode, isShowQntUnitCaption: false, isShowOnlyComplects: false, "", itemTypesSE.Value);
	}

	private void MTRetFromCustTableView_ShowingEditor(object sender, ShowingEditorEventArgs e)
	{
		if (((_property1Col.Visible && e.Column == _property1Col) || (_property2Col.Visible && e.Column == _property2Col)) && e.Row != null)
		{
			MTRetFromCustSpecificationRow mTRetFromCustSpecificationRow = (MTRetFromCustSpecificationRow)e.Row;
			MaterialCacheRow item = TradeSettings.TradeCacheService.Material.GetItem(mTRetFromCustSpecificationRow.ItemCode);
			if (item != null)
			{
				if ((string.IsNullOrWhiteSpace(item.Property1) && e.Column == _property1Col) || (string.IsNullOrWhiteSpace(item.Property2) && e.Column == _property2Col) || (e.Column == _storageCol && mTRetFromCustSpecificationRow.ItemType == ItemTypesSE.Services.Value))
				{
					e.Cancel = true;
				}
			}
			else
			{
				e.Cancel = true;
			}
		}
		if ((e.Column == _itemCodeCol || e.Column == _unitCodeCol) && BaseISN != Guid.Empty)
		{
			e.Cancel = true;
		}
	}

	private void CalcSummAndSummVAT()
	{
		decimal num = 0m;
		decimal num2 = 0m;
		if (_vatType.Code.Trim().InList("1", "2", "5"))
		{
			foreach (MTRetFromCustSpecificationRow mtRetFromCust in _mtRetFromCustList)
			{
				num += ArmSoft.SME.Trade.UI.Helpers.PriceHelper.CalcVATAmountByType(mtRetFromCust.Summa, _vatType.Code, mtRetFromCust.VAT.Value);
				num2 += mtRetFromCust.Summa;
			}
			num2 += _sumFromRound;
			_vatSumma.EditValue = num;
			_totalSumma.EditValue = num2;
			if (_vatType.Code.Trim() == "1")
			{
				_totalSumma.EditValue = Convert.ToDecimal(_totalSumma.EditValue) + Convert.ToDecimal(_vatSumma.EditValue);
			}
		}
		else
		{
			foreach (MTRetFromCustSpecificationRow mtRetFromCust2 in _mtRetFromCustList)
			{
				num2 += mtRetFromCust2.Summa;
			}
			_vatSumma.EditValue = 0m;
			_totalSumma.EditValue = num2;
		}
		CalculatePartSumma();
	}

	private decimal CalcUsedBonusSumm()
	{
		decimal result = 0m;
		foreach (MTRetFromCustSpecificationRow mtRetFromCust in _mtRetFromCustList)
		{
			result += mtRetFromCust.UsedBonus;
		}
		return result;
	}

	private void CurrCode_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		bool isConfirmedSummChange = false;
		if (_totalSumma.Value != 0m && !string.IsNullOrWhiteSpace(_currCode.Code) && _currCode.DoValidate() && ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Trade.Common.Resources.ValidateChangePrices, ArmSoft.SME.Trade.Common.Resources.ProgramName, MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No) == MessageBoxResult.Yes)
		{
			isConfirmedSummChange = true;
		}
		CurrCodeChanged(isConfirmedSummChange);
	}

	private void CurrCodeChanged(bool isConfirmedSummChange = false)
	{
		ChangedCurrCodeOrDate();
		if (isConfirmedSummChange)
		{
			UpdatePricesForCurr();
		}
		if (!string.IsNullOrWhiteSpace(_currCode.Code) && _currCode.DoValidate())
		{
			_oldDocCurrCode = _currCode.Code;
			_oldDocCurrExChngRate = _currExChngRate.Rate;
			_oldDocCurrExChngBase = _currExChngRate.Base;
		}
	}

	private void ChangedCurrCodeOrDate()
	{
		if (string.IsNullOrWhiteSpace(_currCode.Code) || _currCode.Code == "AMD" || !_currCode.DoValidate())
		{
			_currExChngRate.IsReadOnly = true;
			_priceCol.Header = ArmSoft.SME.Common.Resources.Price;
			_summaCol.Header = ArmSoft.SME.Common.Resources.Summa;
			_currExChngRate.Rate = 1m;
			_currExChngRate.Base = 1m;
		}
		else
		{
			_currExChngRate.IsReadOnly = PO.StoredInDb && PO.State != ArmSoft.SME.Trade.Common.SmartEnums.DocumentStateSE.Draft.Value && PO.State != ArmSoft.SME.Trade.Common.SmartEnums.DocumentStateSE.Deleted.Value;
			_priceCol.Header = ArmSoft.SME.Common.Resources.FCPrice;
			_summaCol.Header = ArmSoft.SME.Common.Resources.FCSumma;
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

	private void GetParamCurrCodeRates()
	{
		try
		{
			if (_currCode.Code == _paramCurrCode)
			{
				_paramCurrExChngRate = _currExChngRate.Rate;
				_paramCurrExChngBase = _currExChngRate.Base;
			}
			else if (_date.DoValidate())
			{
				ExchangeRate exchangeRates = TradeSettings.CreateTradeClient().CommonServices.GetExchangeRates(_paramCurrCode, _date.EditValue.Value);
				_paramCurrExChngRate = exchangeRates.Rate;
				_paramCurrExChngBase = exchangeRates.Base;
			}
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	private void Date_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		if (_date.DoValidate() && _date.EditValue.HasValue)
		{
			GetParamCurrCodeRates();
			return;
		}
		ControlsHelper.SetFocus(_date);
		ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Common.Resources.ValidateEmptyDate, ArmSoft.SME.Trade.Common.Resources.ProgramName, MessageBoxButton.OK, MessageBoxImage.Exclamation);
	}

	private void CurrExChngRate_EditValueChanged(object sender, RateEditValueChangedEventArgs e)
	{
		if (_currExChngRate.DoValidate())
		{
			CalculatePartSumma();
			GetParamCurrCodeRates();
		}
	}

	private void UpdatePricesForCurr()
	{
		foreach (MTRetFromCustSpecificationRow mtRetFromCust in _mtRetFromCustList)
		{
			if (_oldDocCurrCode == "AMD")
			{
				if (_currExChngRate.Rate != 0m)
				{
					ExchangeRate exchangeRate = new ExchangeRate();
					exchangeRate.Rate = _currExChngRate.Rate;
					exchangeRate.Base = _currExChngRate.Base;
					mtRetFromCust.Price = ArmSoft.SME.Trade.Common.Helpers.PriceHelper.GetSumCurr(mtRetFromCust.Price, exchangeRate);
				}
			}
			else if (!(_currExChngRate.Rate == 0m) && !(_oldDocCurrExChngBase == 0m))
			{
				mtRetFromCust.Price = (mtRetFromCust.Price * _oldDocCurrExChngRate / _oldDocCurrExChngBase / _currExChngRate.Rate * _currExChngRate.Base).RoundAs(2);
			}
			mtRetFromCust.Summa = ArmSoft.SME.Trade.UI.Helpers.PriceHelper.CalculateSum(mtRetFromCust.Price, mtRetFromCust.Quantity);
		}
		CalcSummAndSummVAT();
		_mtRetFromCustGrid.Grid.RefreshData();
	}

	private void SupplierBusinessAddress_EditKeyValueChanged(object sender, object keyValue)
	{
		if (keyValue != null)
		{
			StoragesCacheRow item = TradeSettings.TradeCacheService.Storage.GetItem(keyValue.ToString());
			if (item != null)
			{
				_supplierBusinessAddress.Text = item.Address;
			}
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
		if (_baseDocTaxInvoiceDate.EditValue.HasValue)
		{
			PO.BaseDocTaxInvoiceDate = _baseDocTaxInvoiceDate.EditValue;
		}
		PO.MTList.Clear();
		foreach (MTRetFromCustSpecificationRow mtRetFromCust in _mtRetFromCustList)
		{
			PO.MTList.Add(new MTReturnFromCustomerBase.MTRetFromCustSpecificationRow
			{
				RowID = mtRetFromCust.RowID,
				ItemType = mtRetFromCust.ItemType,
				Storage = mtRetFromCust.Storage,
				ItemCode = mtRetFromCust.ItemCode,
				ItemID = mtRetFromCust.ItemID,
				RowNum = mtRetFromCust.RowNum,
				ItemName = mtRetFromCust.ItemName,
				UnitCode = mtRetFromCust.UnitCode,
				UnitBrief = mtRetFromCust.UnitBrief,
				Quantity = mtRetFromCust.Quantity,
				Price = mtRetFromCust.Price,
				Discount = mtRetFromCust.Discount,
				DiscountPrice = mtRetFromCust.DiscountPrice,
				UsedBonus = mtRetFromCust.UsedBonus,
				AccumulatedBonus = mtRetFromCust.AccumulatedBonus,
				Summa = mtRetFromCust.Summa,
				DiscountSumma = mtRetFromCust.DiscountSumma,
				BeginSumma = mtRetFromCust.BeginSumma,
				BeginPrice = mtRetFromCust.BeginPrice,
				CostPrice = mtRetFromCust.CostPrice,
				CostPriceVAT = mtRetFromCust.CostPriceVAT,
				CurrCostPrice = mtRetFromCust.CurrCostPrice,
				BarCode = mtRetFromCust.BarCode,
				VAT = mtRetFromCust.VAT,
				DealType = mtRetFromCust.DealType,
				BaseRowID = mtRetFromCust.BaseRowID,
				IsDeleted = mtRetFromCust.Deleted,
				IsGift = mtRetFromCust.IsGift,
				PartyBase = mtRetFromCust.PartyBase,
				PartyRow = mtRetFromCust.PartyRow,
				UseLife = mtRetFromCust.UseLife,
				Country = mtRetFromCust.Country,
				Producer = mtRetFromCust.Producer,
				ProducerCaption = mtRetFromCust.ProducerCaption,
				Property1 = mtRetFromCust.Property1,
				Property1Caption = mtRetFromCust.Property1Caption,
				Property2 = mtRetFromCust.Property2,
				Property2Caption = mtRetFromCust.Property2Caption,
				RowDescription = mtRetFromCust.RowDescription,
				IsPartySelected = mtRetFromCust.IsPartySelected
			});
		}
		PO.MaxRowID = _maxRowID;
		PO.BankAcquiring.PartnerCode = _bankAcquiringContainer.PartnerCode.Code;
		PO.BankAcquiring.ContractCode = _bankAcquiringContainer.Contract.Code;
		PO.BankAcquiring.SummaPart = _ecrReturnByCardSum.Value;
	}

	private void Storage_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		bool flag = true;
		if (string.IsNullOrWhiteSpace(Convert.ToString(e.NewValue)))
		{
			return;
		}
		if (!PO.StoredInDb)
		{
			StoragesCacheRow item = TradeSettings.TradeCacheService.Storage.GetItem(e.NewValue.ToString());
			if (item != null)
			{
				if (!TradeSettings.CreateTradeClient().CommonServices.IsStorageAvailable(Convert.ToString(e.NewValue)))
				{
					e.Handled = true;
					_storage.Code = Convert.ToString(e.OldValue);
					ArmSoft.SME.Core.UI.MessageBox.Show(string.Format(ArmSoft.SME.Trade.Common.Resources.StorageNotAvailForUser, Settings.LoginService.UserName, Convert.ToString(e.NewValue)), ArmSoft.SME.Trade.Common.Resources.ProgramName, MessageBoxButton.OK, MessageBoxImage.Exclamation);
				}
				else if (!string.IsNullOrWhiteSpace(item.Address))
				{
					_supplierBusinessAddress.Text = item.Address;
				}
			}
		}
		if (_mtRetFromCustList.Any((MTRetFromCustSpecificationRow row) => row.ItemType == ItemTypesSE.Goods.Value && !string.IsNullOrWhiteSpace(row.Storage) && row.Storage != _storage.Code) && ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Trade.Common.Resources.ChangeStorageValues, ArmSoft.SME.Trade.Common.Resources.ProgramName, MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No) == MessageBoxResult.No)
		{
			flag = false;
		}
		if (flag)
		{
			foreach (MTRetFromCustSpecificationRow mtRetFromCust in _mtRetFromCustList)
			{
				if (mtRetFromCust.ItemType == ItemTypesSE.Goods.Value)
				{
					mtRetFromCust.Storage = _storage.Code;
				}
			}
		}
		_mtRetFromCustGrid.Grid.RefreshData();
	}

	private void VATType_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		VATType_EditValueChangedForLoad();
		VATTypeValidate(Convert.ToString(e.OldValue), _vatType.Code);
		_dealType.DataView = new SmartEnum(_vatType.Code.InList("1", "2", "5") ? typeof(InvoiceDealTypesSE) : typeof(AccDealTypesSE), null, padLeft: false, '0', allSubTypes: false, ValueMember.ValueUI);
		if (((e.OldValue.ToString().Trim() == "1" || e.OldValue.ToString().Trim() == "2" || e.OldValue.ToString().Trim() == "5") && (e.NewValue.ToString().Trim() == "3" || e.NewValue.ToString().Trim() == "4")) || ((e.OldValue.ToString().Trim() == "3" || e.OldValue.ToString().Trim() == "4") && (e.NewValue.ToString().Trim() == "1" || e.NewValue.ToString().Trim() == "2" || e.OldValue.ToString().Trim() == "5")))
		{
			_dealType.Code = string.Empty;
			_dealTypeKey = string.Empty;
			foreach (MTRetFromCustSpecificationRow mtRetFromCust in _mtRetFromCustList)
			{
				mtRetFromCust.DealType = string.Empty;
			}
		}
		_mtRetFromCustGrid.Grid.RefreshData();
		CalcSummAndSummVAT();
	}

	private void VATType_EditValueChangedForLoad()
	{
		bool flag = _vatType.Code.InList("1", "2", "5");
		_vatCol.Visible = flag;
		_vatCol.ShowInColumnChooser = flag;
		if (!flag)
		{
			_vatSumma.EditValue = 0m;
		}
		_dealType.DataView = new SmartEnum(_vatType.Code.InList("1", "2", "5") ? typeof(InvoiceDealTypesSE) : typeof(AccDealTypesSE), null, padLeft: false, '0', allSubTypes: false, ValueMember.ValueUI);
	}

	private void DealType_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		if (!string.IsNullOrWhiteSpace(_dealType.Code))
		{
			SmartEnumDataProvider.DataRow dataRow = (SmartEnumDataProvider.DataRow)_dealType.DataView.GetRowByValue(_dealType.Code);
			if (dataRow != null)
			{
				_dealTypeKey = dataRow.Code;
				foreach (MTRetFromCustSpecificationRow mtRetFromCust in _mtRetFromCustList)
				{
					if (_vatType.Code.InList("3", "4") || !mtRetFromCust.VAT.Value)
					{
						mtRetFromCust.DealType = dataRow.Code;
					}
					else
					{
						mtRetFromCust.DealType = string.Empty;
					}
				}
			}
		}
		else
		{
			_dealTypeKey = string.Empty;
			foreach (MTRetFromCustSpecificationRow mtRetFromCust2 in _mtRetFromCustList)
			{
				mtRetFromCust2.DealType = string.Empty;
			}
		}
		_mtRetFromCustGrid.Grid.RefreshData();
	}

	private void DealType_ClickDropDown(object sender, DropDownControlEventArgs e)
	{
		_dealType.DataView = new SmartEnum(_vatType.Code.InList("1", "2", "5") ? typeof(InvoiceDealTypesSE) : typeof(AccDealTypesSE), null, padLeft: false, '0', allSubTypes: false, ValueMember.ValueUI);
	}

	private void PriceListType_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		bool flag = false;
		string priceListRowVATType = "";
		string priceListRowCurr;
		if (!string.IsNullOrWhiteSpace(_priceListType.Code) && _priceListType.DoValidate())
		{
			if (_totalSumma.Value != 0m)
			{
				if (ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Trade.Common.Resources.ValidateChangePrices, ArmSoft.SME.Trade.Common.Resources.ProgramName, MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No) == MessageBoxResult.Yes)
				{
					flag = true;
				}
			}
			else
			{
				if (ArmSoft.SME.Trade.UI.Helpers.PriceHelper.GetPriceListRowVATType(_priceListType.Code, out priceListRowVATType, out priceListRowCurr))
				{
					_vatType.Code = priceListRowVATType;
				}
				flag = true;
			}
		}
		if (flag)
		{
			UpdateReturnFromCustPrices(_date.EditValue.Value, _priceListType.Code, _currExChngRate.Rate, _currExChngRate.Base);
			ArmSoft.SME.Trade.UI.Helpers.PriceHelper.GetPriceListRowVATType(_priceListType.Code, out priceListRowVATType, out priceListRowCurr);
			if (priceListRowVATType != _vatType.Code)
			{
				VATTypeValidate(priceListRowVATType, _vatType.Code);
			}
			CalcSummAndSummVAT();
			_mtRetFromCustGrid.Grid.RefreshData();
		}
	}

	private void TotalSumma_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		TotalSumma_EditValueChanged();
	}

	private void TotalSumma_EditValueChanged()
	{
		if (_currCode.Code == "AMD")
		{
			_returnECRTotalSum.Value = _totalSumma.Value;
		}
		else if (_currExChngRate.Base != 0m)
		{
			ExchangeRate exchangeRate = new ExchangeRate();
			exchangeRate.Rate = _currExChngRate.Rate;
			exchangeRate.Base = _currExChngRate.Base;
			_returnECRTotalSum.Value = ArmSoft.SME.Trade.Common.Helpers.PriceHelper.GetSumNNC(_totalSumma.Value, exchangeRate);
		}
		if (BaseDocument != null && BaseDocument.POType == (short)ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTBill)
		{
			MTBill mTBill = (MTBill)BaseDocument;
			decimal? summaPlCard = mTBill.SummaPlCard;
			if ((summaPlCard.GetValueOrDefault() == 0m) & summaPlCard.HasValue)
			{
				summaPlCard = mTBill.SummaCredit;
				if ((summaPlCard.GetValueOrDefault() == 0m) & summaPlCard.HasValue)
				{
					summaPlCard = mTBill.SummaPayByBonus;
					if ((summaPlCard.GetValueOrDefault() == 0m) & summaPlCard.HasValue)
					{
						summaPlCard = mTBill.SummaPrePayment;
						if ((summaPlCard.GetValueOrDefault() == 0m) & summaPlCard.HasValue)
						{
							_ecrReturnByCashSum.Value = ((_returnECRTotalSum.Value > _baseECRByCashSum.Value) ? _baseECRByCashSum.Value : _returnECRTotalSum.Value);
							return;
						}
					}
				}
			}
			summaPlCard = mTBill.SummaCash;
			if (!((summaPlCard.GetValueOrDefault() == 0m) & summaPlCard.HasValue))
			{
				return;
			}
			summaPlCard = mTBill.SummaCredit;
			if (!((summaPlCard.GetValueOrDefault() == 0m) & summaPlCard.HasValue))
			{
				return;
			}
			summaPlCard = mTBill.SummaPayByBonus;
			if ((summaPlCard.GetValueOrDefault() == 0m) & summaPlCard.HasValue)
			{
				summaPlCard = mTBill.SummaPrePayment;
				if ((summaPlCard.GetValueOrDefault() == 0m) & summaPlCard.HasValue)
				{
					_ecrReturnByCardSum.Value = ((_returnECRTotalSum.Value > _baseECRByCardSum.Value) ? _baseECRByCardSum.Value : _returnECRTotalSum.Value);
				}
			}
		}
		else if (BaseDocument == null && !string.IsNullOrWhiteSpace(_cashDesk.Code))
		{
			_ecrReturnByCashSum.Value = _returnECRTotalSum.Value;
		}
	}

	private void CashDesk_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		if (!string.IsNullOrWhiteSpace(Convert.ToString(e.NewValue)) && TradeSettings.TradeCacheService.Cash.GetItem(e.NewValue.ToString()) != null && !TradeSettings.CreateTradeClient().CommonServices.IsCashDeskAvailable(Convert.ToString(e.NewValue)))
		{
			e.Handled = true;
			_cashDesk.Code = Convert.ToString(e.OldValue);
			ArmSoft.SME.Core.UI.MessageBox.Show(string.Format(ArmSoft.SME.Trade.Common.Resources.CashDeskNotAvailForUser, Settings.LoginService.UserName, Convert.ToString(e.NewValue)), ArmSoft.SME.Trade.Common.Resources.ProgramName, MessageBoxButton.OK, MessageBoxImage.Exclamation);
		}
		else
		{
			CashDesk_EditValueForLoad();
		}
	}

	private void CashDesk_EditValueForLoad()
	{
		SetCashierShiftNumber();
		if (BaseDocument != null && BaseDocument.POType != (short)ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTBill)
		{
			return;
		}
		if (string.IsNullOrWhiteSpace(_cashDesk.Code))
		{
			if (!string.IsNullOrWhiteSpace(_buyerContractContainer.PartnerCode.Code))
			{
				_buyerContractContainer.SummaPart.Value = _returnECRTotalSum.Value;
			}
			_ecrReturnByCashSum.Value = 0m;
			_ecrReturnByCashSum.IsReadOnly = true;
		}
		else
		{
			_ecrReturnByCashSum.IsReadOnly = false;
		}
	}

	private void ECRReturn_CashOrCard_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		CalculatePartSumma();
	}

	private void ECRReturnCard_Validate(object sender, ValidationEventArgs e)
	{
		if (_ecrReturnByCashSum.Value + _ecrReturnByPrePaymentSum.Value + Convert.ToDecimal(e.Value) > _returnECRTotalSum.Value)
		{
			e.IsValid = false;
			e.ErrorContent = ArmSoft.SME.Trade.Common.Resources.ReturnSumGreaterThanTotalSum;
		}
		else
		{
			e.IsValid = true;
		}
	}

	private void ECRReturnCash_Validate(object sender, ValidationEventArgs e)
	{
		if (_ecrReturnByCardSum.Value + _ecrReturnByPrePaymentSum.Value + Convert.ToDecimal(e.Value) > _returnECRTotalSum.Value)
		{
			e.IsValid = false;
			e.ErrorContent = ArmSoft.SME.Trade.Common.Resources.ReturnSumGreaterThanTotalSum;
		}
		else
		{
			e.IsValid = true;
		}
	}

	private void ECRReturnByPrePaymentSum_Validate(object sender, ValidationEventArgs e)
	{
		if (_ecrReturnByCashSum.Value + _ecrReturnByCardSum.Value + Convert.ToDecimal(e.Value) > _returnECRTotalSum.Value)
		{
			e.IsValid = false;
			e.ErrorContent = ArmSoft.SME.Trade.Common.Resources.ReturnSumGreaterThanTotalSum;
		}
		else
		{
			e.IsValid = true;
		}
	}

	private void VATTypeValidate(string vatTypeOldValue, string vatTypeNewValue)
	{
		decimal num = 0m;
		decimal num2 = 0m;
		if (_mtRetFromCustList.Count > 0)
		{
			switch (vatTypeNewValue)
			{
			case "1":
				if (vatTypeOldValue == "3" || vatTypeOldValue == "4")
				{
					foreach (MTRetFromCustSpecificationRow mtRetFromCust in _mtRetFromCustList)
					{
						num += ArmSoft.SME.Trade.UI.Helpers.PriceHelper.CalcVATAmountByType(mtRetFromCust.Summa, "1", mtRetFromCust.VAT.Value);
						num2 += mtRetFromCust.Summa;
					}
				}
				else
				{
					foreach (MTRetFromCustSpecificationRow mtRetFromCust2 in _mtRetFromCustList)
					{
						mtRetFromCust2.Price -= ArmSoft.SME.Trade.UI.Helpers.PriceHelper.CalcVATAmountByType(mtRetFromCust2.Price, vatTypeOldValue, mtRetFromCust2.VAT.Value);
						mtRetFromCust2.Summa = ArmSoft.SME.Trade.UI.Helpers.PriceHelper.CalculateSum(mtRetFromCust2.Price, mtRetFromCust2.Quantity);
						num += ArmSoft.SME.Trade.UI.Helpers.PriceHelper.CalcVATAmountByType(mtRetFromCust2.Summa, "1", mtRetFromCust2.VAT.Value);
						num2 += mtRetFromCust2.Summa;
					}
				}
				_vatSumma.EditValue = num;
				_totalSumma.EditValue = num2 + num;
				break;
			case "2":
				switch (vatTypeOldValue)
				{
				case "3":
				case "4":
					foreach (MTRetFromCustSpecificationRow mtRetFromCust3 in _mtRetFromCustList)
					{
						num += ArmSoft.SME.Trade.UI.Helpers.PriceHelper.CalcVATAmountByType(mtRetFromCust3.Summa, "1", mtRetFromCust3.VAT.Value);
						mtRetFromCust3.Price += ArmSoft.SME.Trade.UI.Helpers.PriceHelper.CalcVATAmountByType(mtRetFromCust3.Price, "1", mtRetFromCust3.VAT.Value);
						mtRetFromCust3.Summa = ArmSoft.SME.Trade.UI.Helpers.PriceHelper.CalculateSum(mtRetFromCust3.Price, mtRetFromCust3.Quantity);
						num2 += mtRetFromCust3.Summa;
					}
					break;
				case "1":
					foreach (MTRetFromCustSpecificationRow mtRetFromCust4 in _mtRetFromCustList)
					{
						num += ArmSoft.SME.Trade.UI.Helpers.PriceHelper.CalcVATAmountByType(mtRetFromCust4.Summa, "1", mtRetFromCust4.VAT.Value);
						mtRetFromCust4.Price += ArmSoft.SME.Trade.UI.Helpers.PriceHelper.CalcVATAmountByType(mtRetFromCust4.Price, "1", mtRetFromCust4.VAT.Value);
						mtRetFromCust4.Summa = ArmSoft.SME.Trade.UI.Helpers.PriceHelper.CalculateSum(mtRetFromCust4.Price, mtRetFromCust4.Quantity);
						num2 += mtRetFromCust4.Summa;
					}
					break;
				case "5":
					foreach (MTRetFromCustSpecificationRow mtRetFromCust5 in _mtRetFromCustList)
					{
						num -= ArmSoft.SME.Trade.UI.Helpers.PriceHelper.CalcVATAmountByType(mtRetFromCust5.Summa, "5", mtRetFromCust5.VAT.Value);
						mtRetFromCust5.Price -= ArmSoft.SME.Trade.UI.Helpers.PriceHelper.CalcVATAmountByType(mtRetFromCust5.Price, "5", mtRetFromCust5.VAT.Value);
						mtRetFromCust5.Summa = ArmSoft.SME.Trade.UI.Helpers.PriceHelper.CalculateSum(mtRetFromCust5.Price, mtRetFromCust5.Quantity);
						num += ArmSoft.SME.Trade.UI.Helpers.PriceHelper.CalcVATAmountByType(mtRetFromCust5.Summa, "1", mtRetFromCust5.VAT.Value);
						mtRetFromCust5.Price += ArmSoft.SME.Trade.UI.Helpers.PriceHelper.CalcVATAmountByType(mtRetFromCust5.Price, "1", mtRetFromCust5.VAT.Value);
						mtRetFromCust5.Summa = ArmSoft.SME.Trade.UI.Helpers.PriceHelper.CalculateSum(mtRetFromCust5.Price, mtRetFromCust5.Quantity);
						num2 += mtRetFromCust5.Summa;
					}
					break;
				}
				_vatSumma.EditValue = num;
				_totalSumma.EditValue = num2;
				break;
			case "5":
				switch (vatTypeOldValue)
				{
				case "3":
				case "4":
					foreach (MTRetFromCustSpecificationRow mtRetFromCust6 in _mtRetFromCustList)
					{
						num += ArmSoft.SME.Trade.UI.Helpers.PriceHelper.CalcVATAmountByType(mtRetFromCust6.Summa, "1", mtRetFromCust6.VAT.Value);
						mtRetFromCust6.Price += ArmSoft.SME.Trade.UI.Helpers.PriceHelper.CalcVATAmountByType(mtRetFromCust6.Price, "1", mtRetFromCust6.VAT.Value);
						mtRetFromCust6.Summa = ArmSoft.SME.Trade.UI.Helpers.PriceHelper.CalculateSum(mtRetFromCust6.Price, mtRetFromCust6.Quantity);
						num2 += mtRetFromCust6.Summa;
					}
					break;
				case "1":
					foreach (MTRetFromCustSpecificationRow mtRetFromCust7 in _mtRetFromCustList)
					{
						num += ArmSoft.SME.Trade.UI.Helpers.PriceHelper.CalcVATAmountByType(mtRetFromCust7.Summa, "1", mtRetFromCust7.VAT.Value);
						mtRetFromCust7.Price += ArmSoft.SME.Trade.UI.Helpers.PriceHelper.CalcVATAmountByType(mtRetFromCust7.Price, "1", mtRetFromCust7.VAT.Value);
						mtRetFromCust7.Summa = ArmSoft.SME.Trade.UI.Helpers.PriceHelper.CalculateSum(mtRetFromCust7.Price, mtRetFromCust7.Quantity);
						num2 += mtRetFromCust7.Summa;
					}
					break;
				case "2":
					foreach (MTRetFromCustSpecificationRow mtRetFromCust8 in _mtRetFromCustList)
					{
						num -= ArmSoft.SME.Trade.UI.Helpers.PriceHelper.CalcVATAmountByType(mtRetFromCust8.Summa, "2", mtRetFromCust8.VAT.Value);
						mtRetFromCust8.Price -= ArmSoft.SME.Trade.UI.Helpers.PriceHelper.CalcVATAmountByType(mtRetFromCust8.Price, "2", mtRetFromCust8.VAT.Value);
						mtRetFromCust8.Summa = ArmSoft.SME.Trade.UI.Helpers.PriceHelper.CalculateSum(mtRetFromCust8.Price, mtRetFromCust8.Quantity);
						num += ArmSoft.SME.Trade.UI.Helpers.PriceHelper.CalcVATAmountByType(mtRetFromCust8.Summa, "1", mtRetFromCust8.VAT.Value);
						mtRetFromCust8.Price += ArmSoft.SME.Trade.UI.Helpers.PriceHelper.CalcVATAmountByType(mtRetFromCust8.Price, "1", mtRetFromCust8.VAT.Value);
						mtRetFromCust8.Summa = ArmSoft.SME.Trade.UI.Helpers.PriceHelper.CalculateSum(mtRetFromCust8.Price, mtRetFromCust8.Quantity);
						num2 += mtRetFromCust8.Summa;
					}
					break;
				}
				_vatSumma.EditValue = num;
				_totalSumma.EditValue = num2;
				break;
			case "3":
			case "4":
				switch (vatTypeOldValue)
				{
				case "2":
				case "5":
					foreach (MTRetFromCustSpecificationRow mtRetFromCust9 in _mtRetFromCustList)
					{
						mtRetFromCust9.Price -= ArmSoft.SME.Trade.UI.Helpers.PriceHelper.CalcVATAmountByType(mtRetFromCust9.Price, vatTypeOldValue, mtRetFromCust9.VAT.Value);
						mtRetFromCust9.Summa = ArmSoft.SME.Trade.UI.Helpers.PriceHelper.CalculateSum(mtRetFromCust9.Price, mtRetFromCust9.Quantity);
						num2 += mtRetFromCust9.Summa;
					}
					_vatSumma.EditValue = 0m;
					_totalSumma.EditValue = num2;
					break;
				case "1":
					foreach (MTRetFromCustSpecificationRow mtRetFromCust10 in _mtRetFromCustList)
					{
						num2 += mtRetFromCust10.Summa;
					}
					_vatSumma.EditValue = 0m;
					_totalSumma.EditValue = num2;
					break;
				}
				break;
			}
		}
		CalculatePartSumma();
	}

	private void UpdateReturnFromCustPrices(DateOnly docDate, string priceListType, decimal currRate, decimal currBase)
	{
		decimal num = 1m;
		decimal num2 = 1m;
		string priceListRowVATType = "";
		string priceListRowCurr = "";
		if (_mtRetFromCustList.Count == 0)
		{
			return;
		}
		ArmSoft.SME.Trade.UI.Helpers.PriceHelper.GetPriceListRowVATType(priceListType, out priceListRowVATType, out priceListRowCurr);
		if (priceListRowCurr != "AMD")
		{
			ExchangeRate exchangeRates = TradeSettings.CreateTradeClient().CommonServices.GetExchangeRates(priceListRowCurr, docDate);
			num = exchangeRates.Rate;
			num2 = exchangeRates.Base;
		}
		foreach (MTRetFromCustSpecificationRow mtRetFromCust in _mtRetFromCustList)
		{
			PricesDataProvider.DataRow price = TradeSettings.CreateTradeClient().CommonServices.GetPrice(priceListType, mtRetFromCust.ItemID, mtRetFromCust.UnitCode, docDate);
			if (priceListRowCurr == _currCode.Code)
			{
				mtRetFromCust.Price = price.Price;
			}
			else if (priceListRowCurr == "AMD")
			{
				ExchangeRate exchangeRate = new ExchangeRate();
				exchangeRate.Rate = currRate;
				exchangeRate.Base = currBase;
				mtRetFromCust.Price = ArmSoft.SME.Trade.Common.Helpers.PriceHelper.GetSumCurr(price.Price, exchangeRate);
			}
			else
			{
				mtRetFromCust.Price = (price.Price * num / num2 / currRate * currBase).RoundAs(2);
			}
			mtRetFromCust.Summa = ArmSoft.SME.Trade.UI.Helpers.PriceHelper.CalculateSum(mtRetFromCust.Price, mtRetFromCust.Quantity);
		}
	}

	private void MTRetFromCustGrid_RefreshButtons(object sender, EventArgs e)
	{
		if (!IsOfflineDoc && _viewMode != WindowViewMode.View)
		{
			_filteredSearch.IsEnabled = _mtRetFromCustGrid.AllowAdd && !_mtRetFromCustGrid.IsReadOnly;
			if (_fillLatestPurchasePrices != null)
			{
				_fillLatestPurchasePrices.IsEnabled = !_mtRetFromCustGrid.IsReadOnly;
			}
		}
	}

	private void FilteredSearch_Click(object sender, RoutedEventArgs e)
	{
		try
		{
			FilteredSearchWindow filteredSearchWindow = new FilteredSearchWindow(new FilteredSearch
			{
				FilterDataView = new GoodsGroups(showAsTree: true, filteredSearch: true),
				SearchDataView = new GoodsFilter(shortMode: true, "0", "", isShowOnlyInPriceList: true),
				ResultsList = new MTReturnFromCustomerFilteredSearchList(this)
			}, this, isBasketMode: true);
			if (!filteredSearchWindow.ShowDialog().Value)
			{
				return;
			}
			foreach (MTRetFromCustSpecificationRow item in filteredSearchWindow.BasketResult)
			{
				MTRetFromCustSpecificationRow mTRetFromCustSpecificationRow2 = new MTRetFromCustSpecificationRow(this)
				{
					ItemType = item.ItemType,
					ItemID = item.ItemID,
					ItemCode = item.ItemCode,
					ItemName = item.ItemName,
					BarCode = item.BarCode,
					UnitCode = item.UnitCode,
					UnitBrief = item.UnitBrief,
					Discount = item.Discount,
					VAT = item.VAT,
					Quantity = item.Quantity
				};
				if (mTRetFromCustSpecificationRow2.ItemType == ItemTypesSE.Goods.Value)
				{
					mTRetFromCustSpecificationRow2.Storage = _storage.Code;
				}
				if (!string.IsNullOrWhiteSpace(_dealTypeKey))
				{
					if (_vatType.Code == "3" || _vatType.Code == "4" || !Convert.ToBoolean(mTRetFromCustSpecificationRow2.VAT))
					{
						mTRetFromCustSpecificationRow2.DealType = _dealTypeKey;
					}
					else
					{
						mTRetFromCustSpecificationRow2.DealType = string.Empty;
					}
				}
				mTRetFromCustSpecificationRow2.Price = TradeSettings.CreateTradeClient().CommonServices.GetPriceForGivenPriceListAndVATType(_priceListType.Code, _vatType.Code, mTRetFromCustSpecificationRow2.ItemID, mTRetFromCustSpecificationRow2.UnitCode, _date.EditValue.Value, _currCode.Code);
				mTRetFromCustSpecificationRow2.Summa = ArmSoft.SME.Trade.UI.Helpers.PriceHelper.CalculateSum(mTRetFromCustSpecificationRow2.Price, mTRetFromCustSpecificationRow2.Quantity);
				mTRetFromCustSpecificationRow2.RowID = _maxRowID;
				_mtRetFromCustList.Add(mTRetFromCustSpecificationRow2);
				_maxRowID++;
			}
			_mtRetFromCustGrid.Grid.RefreshData();
			CalcSummAndSummVAT();
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	private void FillFromLastPurchase_Click(object sender, RoutedEventArgs e)
	{
		try
		{
			if (_date.EditValue.HasValue)
			{
				if (_mtRetFromCustList.Count == 0)
				{
					return;
				}
				foreach (MTRetFromCustSpecificationRow mtRetFromCust in _mtRetFromCustList)
				{
					if (mtRetFromCust.ItemID != -1)
					{
						CostAmountsFromLastPurchase costAmountsFromLastPurchase = TradeSettings.CreateTradeClient().CommonServices.GetCostAmountsFromLastPurchase(mtRetFromCust.ItemID, _date.EditValue.Value);
						mtRetFromCust.CostPrice = costAmountsFromLastPurchase.CostPrice;
						mtRetFromCust.CostPriceVAT = costAmountsFromLastPurchase.CostPriceVAT;
						decimal mTQntUnitCoeff = ArmSoft.SME.Trade.UI.Helpers.PriceHelper.GetMTQntUnitCoeff(mtRetFromCust.ItemCode, mtRetFromCust.UnitCode);
						mtRetFromCust.UnitBrief = TradeSettings.TradeCacheService.QntUnit.GetItem(mtRetFromCust.UnitCode).Brief;
						mtRetFromCust.CostPrice = (mtRetFromCust.CostPrice * mTQntUnitCoeff).RoundAs(2);
						mtRetFromCust.CostPriceVAT = (mtRetFromCust.CostPriceVAT * mTQntUnitCoeff).RoundAs(2);
						ExchangeRate exchangeRate = new ExchangeRate();
						exchangeRate.Rate = _paramCurrExChngRate;
						exchangeRate.Base = _paramCurrExChngBase;
						mtRetFromCust.CurrCostPrice = ArmSoft.SME.Trade.Common.Helpers.PriceHelper.GetSumCurr(mtRetFromCust.CostPriceVAT, exchangeRate);
						mtRetFromCust.ValidateWithoutResponse();
					}
				}
				_mtRetFromCustGrid.Grid.RefreshData();
				_mtRetFromCustGrid.ConfigureButtons();
			}
			else
			{
				ControlsHelper.SetFocus(_date);
				ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Common.Resources.ValidateEmptyDate, ArmSoft.SME.Trade.Common.Resources.ProgramName, MessageBoxButton.OK, MessageBoxImage.Exclamation);
			}
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
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

	public override void OnMagneticCardReader(string mcrData)
	{
		try
		{
			if (!string.IsNullOrWhiteSpace(mcrData) && MCRInfo.GetInstance().IsValidCard(mcrData))
			{
				ArmSoft.SME.Trade.UI.Helpers.DocumentHelpers.GetPartnerEmployeeDataByBarCode(MCRInfo.GetInstance().GetCardNumber(mcrData), out var partnerCacheRow, out var partnerContract, out var employee);
				FillPartnerEmployeeData(partnerCacheRow, partnerContract, employee);
			}
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
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

	private void CodeEnter_Click(object sender, RoutedEventArgs e)
	{
		On_CodeEnterClick();
	}

	private void ScannerAdd()
	{
		if (!(BaseISN == Guid.Empty))
		{
			return;
		}
		_barCode.DoValidate();
		try
		{
			string barCode = _barCode.Text.Trim();
			PartnerCacheRow partnerCacheRow = null;
			EmployeeCacheRow employee = null;
			if (Settings.ParametersService.UsePartnerEmployeeSearchNewMode())
			{
				if (ArmSoft.SME.Trade.UI.Helpers.DocumentHelpers.IsValidPartnerEmployeeCardCode(barCode, Settings.ParametersService.PartnerEmployeeCardCodePrefixes(), Settings.ParametersService.UsePartnerCardLenght(), Settings.ParametersService.PartnerCardLenght()))
				{
					_barCode.Text = "";
					_barCode.Focus();
					ArmSoft.SME.Trade.UI.Helpers.DocumentHelpers.GetPartnerEmployeeDataByBarCode(barCode, out partnerCacheRow, out var partnerContract, out employee);
					FillPartnerEmployeeData(partnerCacheRow, partnerContract, employee);
					return;
				}
			}
			else if (MCRInfo.GetInstance().IsValidCard(barCode))
			{
				_barCode.Text = "";
				_barCode.Focus();
				ArmSoft.SME.Trade.UI.Helpers.DocumentHelpers.GetPartnerEmployeeDataByBarCode(MCRInfo.GetInstance().GetCardNumber(barCode), out partnerCacheRow, out var partnerContract2, out employee);
				FillPartnerEmployeeData(partnerCacheRow, partnerContract2, employee);
				return;
			}
			decimal qty = 0m;
			string unitCode = string.Empty;
			string unitBrief = string.Empty;
			MaterialCacheRow materialCacheRow = ArmSoft.SME.Trade.UI.Helpers.DocumentHelpers.TryGetItemByBarCode(ref barCode, out qty, out unitCode, out unitBrief);
			if (materialCacheRow == null)
			{
				ControlsHelper.SetFocus(_barCode);
				throw new ASException(string.Format(ArmSoft.SME.Common.Resources.ErrBarCodeNotExist, barCode));
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
			AddMTToList(materialCacheRow, unitCode, unitBrief, qty);
			_barCode.Text = "";
			CalcSummAndSummVAT();
			_vatSumma.DoValidate();
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
			AddNewRowOrmergeInner(goodsRow, _rowAddMode == "2", string.IsNullOrWhiteSpace(unitCode) ? goodsRow.Unit : unitCode, string.IsNullOrWhiteSpace(unitBrief) ? goodsRow.UnitRow.Brief : unitBrief, quantity);
		}
	}

	private void AddNewRowOrmergeInner(MaterialCacheRow goodsRow, bool forFocusedRow, string unitCode, string unitBrief, decimal quantity = 1m)
	{
		MTRetFromCustSpecificationRow mTRetFromCustSpecificationRow = null;
		if (forFocusedRow && _mtRetFromCustList.Count != 0)
		{
			mTRetFromCustSpecificationRow = (MTRetFromCustSpecificationRow)_mtRetFromCustTableView.Grid.CurrentItem;
			if (mTRetFromCustSpecificationRow == null)
			{
				return;
			}
		}
		for (int i = 0; i <= _mtRetFromCustList.Count - 1; i++)
		{
			MTRetFromCustSpecificationRow mTRetFromCustSpecificationRow2 = _mtRetFromCustList[i];
			if ((!forFocusedRow || mTRetFromCustSpecificationRow.RowID == mTRetFromCustSpecificationRow2.RowID) && goodsRow.Id == mTRetFromCustSpecificationRow2.ItemID && unitCode == mTRetFromCustSpecificationRow2.UnitCode && _storage.Code == mTRetFromCustSpecificationRow2.Storage)
			{
				FillMTRetFromCustGridRow(_mtRetFromCustList[i], goodsRow, unitCode, unitBrief, quantity);
				int rowHandleByListIndex = _mtRetFromCustGrid.Grid.GetRowHandleByListIndex(i);
				POWindow<MTReturnFromCustomer>.SelectCellInGrid(_mtRetFromCustTableView, _mtRetFromCustGrid, rowHandleByListIndex, "Quantity");
				return;
			}
		}
		AddNewRowInner(goodsRow, unitCode, unitBrief, quantity);
	}

	private void FillMTRetFromCustGridRow(MTRetFromCustSpecificationRow mtRetFromCustGridRow, MaterialCacheRow goodsRow, string unitCode, string unitBrief, decimal quantity = 1m)
	{
		mtRetFromCustGridRow.ItemType = goodsRow.Type;
		mtRetFromCustGridRow.Storage = ((mtRetFromCustGridRow.ItemType == ItemTypesSE.Goods.Value) ? _storage.Code : string.Empty);
		mtRetFromCustGridRow.UnitCode = unitCode;
		mtRetFromCustGridRow.UnitBrief = unitBrief;
		if (goodsRow != null)
		{
			mtRetFromCustGridRow.ItemCode = goodsRow.MTCode;
			mtRetFromCustGridRow.ItemID = goodsRow.Id;
			mtRetFromCustGridRow.ItemName = goodsRow.Caption;
			mtRetFromCustGridRow.VAT = goodsRow.VAT;
			mtRetFromCustGridRow.Country = goodsRow.Country;
			mtRetFromCustGridRow.Producer = goodsRow.Producer;
		}
		mtRetFromCustGridRow.Quantity += quantity;
		if (mtRetFromCustGridRow.Quantity == 1m)
		{
			mtRetFromCustGridRow.Price = TradeSettings.CreateTradeClient().CommonServices.GetPriceForGivenPriceListAndVATType(_priceListType.Code, _vatType.Code, mtRetFromCustGridRow.ItemID, mtRetFromCustGridRow.UnitCode, _date.EditValue.Value, _currCode.Code);
		}
		mtRetFromCustGridRow.Summa = ArmSoft.SME.Trade.UI.Helpers.PriceHelper.CalculateSum(mtRetFromCustGridRow.Price, mtRetFromCustGridRow.Quantity);
		if (!string.IsNullOrWhiteSpace(_dealTypeKey))
		{
			if (_vatType.Code == "3" || _vatType.Code == "4" || !goodsRow.VAT)
			{
				mtRetFromCustGridRow.DealType = _dealTypeKey;
			}
			else
			{
				mtRetFromCustGridRow.DealType = string.Empty;
			}
		}
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
				GoodsFilter goodsFilter = new GoodsFilter();
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
			if (_tabs.SelectedIndex != _priceListTabControlIndex)
			{
				_tabs.SelectedContainer = (DXTabItem)_tabs.Items[_priceListTabControlIndex];
			}
			_barCode.Text = "";
			CalcSummAndSummVAT();
			_vatSumma.DoValidate();
			ControlsHelper.SetFocus(_barCode);
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	private void AddNewRowInner(MaterialCacheRow goodsRow, string unitCode, string unitBrief, decimal quantity = 1m)
	{
		MTRetFromCustSpecificationRow mTRetFromCustSpecificationRow = new MTRetFromCustSpecificationRow(this);
		FillMTRetFromCustGridRow(mTRetFromCustSpecificationRow, goodsRow, unitCode, unitBrief, quantity);
		mTRetFromCustSpecificationRow.RowID = _maxRowID;
		_mtRetFromCustList.Add(mTRetFromCustSpecificationRow);
		_maxRowID++;
		int rowHandleByListIndex = _mtRetFromCustGrid.Grid.GetRowHandleByListIndex(_mtRetFromCustList.Count - 1);
		POWindow<MTReturnFromCustomer>.SelectCellInGrid(_mtRetFromCustTableView, _mtRetFromCustGrid, rowHandleByListIndex, "Quantity");
		_mtRetFromCustGrid.ConfigureButtons();
	}

	private void FillPartnerEmployeeData(PartnerCacheRow partner, PartnerCacheRow.ContractRow contractRow, EmployeeCacheRow employee)
	{
		if (partner != null)
		{
			_buyerContractContainer.PartnerCode.Code = partner.PartCode;
			_buyerContractContainer.Contract.Code = contractRow.Contract;
			_buyerContractContainer.ContractCurrCode.Code = contractRow.CurrCode;
		}
		else
		{
			_salesConsultant.Code = employee.EmployeeCode;
		}
	}

	private void PartnerCodeEditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		BuyerCodeEditValueChanged();
		CalculatePartSumma();
	}

	private void BuyerCodeEditValueChanged()
	{
		if (!string.IsNullOrWhiteSpace(_buyerContractContainer.PartnerCode.Code) && _buyerContractContainer.PartnerCode.DoValidate())
		{
			PartnerCacheRow item = TradeSettings.TradeCacheService.Partner.GetItem(_buyerContractContainer.PartnerCode.Code, throwExceptionNotExits: true);
			_buyerName.Text = item.Caption;
			_buyerLegalAddress.Text = item.Address;
			_buyerBusinessAddress.Text = item.BusinessAddress;
			_buyerSettlAcc.Code = item.SettlAcc;
			BindPartnerDataViews(item);
			_buyerTaxCode.Text = item.TaxCode;
			_buyerMngPost.EditValue = item.ManagerPost;
			_buyerMngName.Text = item.ManagerFIO;
			_buyerChiefAccPost.EditValue = item.ChiefaccPost;
			_buyerChiefAccName.Text = item.ChiefaccFIO;
			_buyerStateRegNum.Text = item.PartStateRegister;
			_buyerIDDocType.Code = item.IDDocType;
			_buyerPassport.Text = item.Passport;
			_buyerAdditionalData.EditValue = "";
			_buyerShippingMethod.EditValue = "";
			_byWhomFio.EditValue = "";
			_authorizationNumber.EditValue = "";
			_authorizationDate.EditValue = null;
		}
		else
		{
			_buyerName.Text = "";
			_buyerLegalAddress.Text = "";
			_buyerBusinessAddress.Text = "";
			_buyerSettlAcc.Code = "";
			_buyerTaxCode.Text = "";
			_buyerMngPost.EditValue = "";
			_buyerMngName.Text = "";
			_buyerChiefAccPost.EditValue = "";
			_buyerChiefAccName.Text = "";
			_buyerStateRegNum.Text = "";
			_buyerIDDocType.Code = string.Empty;
			_buyerPassport.Text = "";
			_buyerAdditionalData.EditValue = "";
			_buyerShippingMethod.EditValue = "";
			_byWhomFio.EditValue = "";
			_authorizationNumber.EditValue = "";
			_authorizationDate.EditValue = null;
			BindPartnerDataViews();
		}
	}

	private void BindPartnerDataViews(PartnerCacheRow partnerCacheRow = null)
	{
		if (!string.IsNullOrWhiteSpace(_buyerContractContainer.PartnerCode.Code) && _buyerContractContainer.PartnerCode.DoValidate())
		{
			if (partnerCacheRow == null)
			{
				partnerCacheRow = TradeSettings.TradeCacheService.Partner.GetItem(_buyerContractContainer.PartnerCode.Code, throwExceptionNotExits: true);
			}
			_buyerBusinessAddress.ItemsSource = TradeSettings.TradeCacheService.Partner.GetAllBusinessAddresses(partnerCacheRow);
			_buyerSettlAcc.DataView = new PartnerAccountInBank(partnerCacheRow.PartCode);
		}
		else
		{
			_buyerBusinessAddress.ItemsSource = null;
			_buyerSettlAcc.DataView = new PartnerAccountInBank();
		}
	}

	private void ContractCodeEditValueChanged(object sender, EventArgs e)
	{
		CalculatePartSumma();
	}

	private void CalculatePartSumma()
	{
		decimal summaNCC = _totalSumma.Value;
		if (_currExChngRate.Base != 0m)
		{
			ExchangeRate exchangeRate = new ExchangeRate();
			exchangeRate.Rate = _currExChngRate.Rate;
			exchangeRate.Base = _currExChngRate.Base;
			summaNCC = ArmSoft.SME.Trade.Common.Helpers.PriceHelper.GetSumNNC(_totalSumma.Value, exchangeRate);
		}
		_buyerContractContainer.CalculatePartSumma(summaNCC);
		if (BaseDocument != null && BaseDocument.POType == ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTBill.Value && !string.IsNullOrWhiteSpace(_buyerContractContainer.PartnerCode.Code) && !string.IsNullOrWhiteSpace(_buyerContractContainer.Contract.Code))
		{
			foreach (MTRetFromCustSpecificationRow mtRetFromCust in _mtRetFromCustList)
			{
				_buyerContractContainer.SummaPart.Value -= mtRetFromCust.UsedBonus;
			}
		}
		if ((BaseDocument != null && BaseDocument.POType != ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTBill.Value) || string.IsNullOrWhiteSpace(_buyerContractContainer.PartnerCode.Code) || string.IsNullOrWhiteSpace(_buyerContractContainer.Contract.Code))
		{
			return;
		}
		if (_buyerContractContainer.ContractCurrCode.Code == "AMD")
		{
			_buyerContractContainer.SummaPart.Value -= _ecrReturnByCardSum.Value + _ecrReturnByCashSum.Value + _ecrReturnByPrePaymentSum.Value;
			return;
		}
		try
		{
			ExchangeRate exchangeRates = TradeSettings.CreateTradeClient().CommonServices.GetExchangeRates(_buyerContractContainer.ContractCurrCode.Code, _date.EditValue.Value);
			decimal rate = exchangeRates.Rate;
			decimal num = exchangeRates.Base;
			_buyerContractContainer.SummaPart.Value -= ((_ecrReturnByCardSum.Value + _ecrReturnByCashSum.Value + _ecrReturnByPrePaymentSum.Value) * num / rate).RoundAs(2);
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
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
		BaseDocumentDelete_Click();
		if (e.KeyValue != null && (Guid)e.KeyValue != Guid.Empty)
		{
			TradeDocument tradeDocument = (TradeDocument)Settings.DocumentService.Load((Guid)e.KeyValue);
			if (tradeDocument.POType == ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTBill.Value && !string.IsNullOrWhiteSpace(((MTBill)tradeDocument).PrePaymentECRCheckNum))
			{
				throw new ASException(ArmSoft.SME.Trade.Common.Resources.ReturnToSupplierNotAllowedWithBonusAccumulation);
			}
			BaseISN = (Guid)e.KeyValue;
			_printECRCheck.IsEnabled = BaseDocument != null && BaseDocument.POType == ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTBill.Value && string.IsNullOrWhiteSpace(_baseECRCheckNum.Text);
		}
		else
		{
			BaseISN = Guid.Empty;
			_cashDesk.IsReadOnly = false;
		}
		if (BaseDocument != null && BaseDocument.POType == (short)ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTBill)
		{
			MTBill mTBill = (MTBill)BaseDocument;
			_bankAcquiringContainer.PartnerCode.Code = mTBill.BankAcquiring.PartnerCode;
			_bankAcquiringContainer.Contract.Code = mTBill.BankAcquiring.ContractCode;
			_bankAcquiringContainer.PartnerCode.IsReadOnly = false;
			_bankAcquiringContainer.Contract.IsReadOnly = false;
		}
		else
		{
			_bankAcquiringContainer.PartnerCode.Code = string.Empty;
			_bankAcquiringContainer.Contract.Code = string.Empty;
			_bankAcquiringContainer.PartnerCode.IsReadOnly = true;
			_bankAcquiringContainer.Contract.IsReadOnly = true;
		}
	}

	private void BaseDocumentDelete_Click(object sender, EditKeyValueChangedEventArgs e)
	{
		_baseDocumentControl.KeyValue = Guid.Empty;
		_baseDocumentControl.DoValidate();
		BaseDocumentDelete_Click();
	}

	private void BaseDocumentDelete_Click()
	{
		if (!string.IsNullOrWhiteSpace(_baseDocumentControl.Text))
		{
			_itemCodeCol.ReadOnly = false;
			_unitCodeCol.ReadOnly = false;
			_mtRetFromCustGrid.AllowAdd = true;
			_mtRetFromCustGrid.AllowDuplicate = true;
			_ecrReturnByCardSum.IsReadOnly = true;
			_ecrReturnByCashSum.IsReadOnly = true;
			_bankAcquiringContainer.PartnerCode.IsReadOnly = true;
			_bankAcquiringContainer.Contract.IsReadOnly = true;
			_baseECRCheckNum.IsReadOnly = true;
			_ecrReturnByCardSum.Value = 0m;
			_ecrReturnByCashSum.Value = 0m;
			_cashDesk.Code = Settings.ParametersService.DefaultCash();
			_baseECRCheckNum.Text = "";
			_baseECRByCardSum.Value = 0m;
			_baseECRByCashSum.Value = 0m;
			_ecrReturnByPrePaymentSum.IsReadOnly = true;
			_ecrReturnByPrePaymentSum.Value = 0m;
			_ecrReturnByPrePaymentSum.Value = 0m;
			_baseECRByPrePaymentSum.Value = 0m;
			_printECRCheck.IsEnabled = false;
		}
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
		_date.EditValue = Settings.HostingEnvironment.CurrentDate;
		_comment.EditValue = BaseDocument.Comment;
		_currCode.Code = BaseDocument.CurrCode;
		_currExChngRate.Rate = BaseDocument.ExchangeRate.Rate;
		_currExChngRate.Base = BaseDocument.ExchangeRate.Base;
		_totalSumma.EditValue = BaseDocument.Summ;
		_buyerContractContainer.PartnerCode.Code = BaseDocument.PartnerContractGroup.PartnerCode;
		_buyerContractContainer.Contract.Code = BaseDocument.PartnerContractGroup.ContractCode;
		if (!string.IsNullOrWhiteSpace(_buyerContractContainer.PartnerCode.Code) && !string.IsNullOrWhiteSpace(_buyerContractContainer.Contract.Code))
		{
			_buyerContractContainer.ContractCurrCode.Code = _currCode.Code;
		}
		if (!string.IsNullOrEmpty(_buyerContractContainer.PartnerCode.Code))
		{
			PartnerCacheRow.ContractRow defaultContract = TradeSettings.TradeCacheService.Partner.GetDefaultContract(_buyerContractContainer.PartnerCode.Code);
			if (defaultContract != null)
			{
				_buyerContractContainer.ContractCurrCode.Code = defaultContract.CurrCode;
			}
		}
		string text = Settings.ParametersService.DefaultStorage();
		if (!string.IsNullOrWhiteSpace(text))
		{
			StoragesCacheRow item = TradeSettings.TradeCacheService.Storage.GetItem(text);
			if (item != null && !item.Retail && TradeSettings.CreateTradeClient().CommonServices.IsStorageAvailable(text))
			{
				_storage.Code = text;
			}
		}
		if (string.IsNullOrWhiteSpace(_cashDesk.Code))
		{
			_cashDesk.Code = Settings.ParametersService.DefaultCash();
		}
		MTBill mtBillBase;
		if (BaseDocument.POType == ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTInvoice.Value)
		{
			MTInvoice mtInvoice = (MTInvoice)BaseDocument;
			_vatType.Code = mtInvoice.VATType;
			if (!string.IsNullOrWhiteSpace(mtInvoice.DealType))
			{
				_dealType.Code = mtInvoice.DealType;
				if (_vatType.Code == "3" || _vatType.Code == "4")
				{
					short? num = (from accDealTypesSE in SmartEnum<AccDealTypesSE, short>.List
						where accDealTypesSE.ValueUI.Trim() == mtInvoice.DealType.Trim()
						select accDealTypesSE.Value).FirstOrDefault();
					_dealTypeKey = num.Value.ToString();
				}
				else
				{
					short? num2 = (from invoiceDealTypesSE in SmartEnum<InvoiceDealTypesSE, short>.List
						where invoiceDealTypesSE.ValueUI.Trim() == mtInvoice.DealType.Trim()
						select invoiceDealTypesSE.Value).FirstOrDefault();
					_dealTypeKey = num2.Value.ToString();
				}
			}
			_vatSumma.EditValue = mtInvoice.SummVAT;
			_priceListType.Code = mtInvoice.PriceListType;
			_salesConsultant.Code = mtInvoice.SalesConsultant;
			_baseDocTaxInvoiceSeria.Text = mtInvoice.TaxInvoiceSeries;
			_baseDocTaxInvoiceNumber.Text = mtInvoice.TaxInvoiceNumber;
			_baseDocTaxInvoiceDate.EditValue = mtInvoice.InvoiceDate;
			LoadMTList(mtInvoice.MTList);
			_supplierName.EditValue = mtInvoice.SupplierName;
			_supplierLegalAddress.EditValue = mtInvoice.SupplierLegalAddress;
			_supplierTaxCode.EditValue = mtInvoice.SupplierTaxCode;
			_supplierBusinessAddress.Text = mtInvoice.SupplierBusinessAddress;
			_supplierSettlAcc.Code = mtInvoice.SupplierSettlAcc;
			_supplierMngPost.EditValue = mtInvoice.SupplierMngPost;
			_supplierMngName.Text = mtInvoice.SupplierMngName;
			_supplierChiefAccPost.EditValue = mtInvoice.SupplierChiefAccPost;
			_supplierChiefAccName.Text = mtInvoice.SupplierChiefAccName;
			_buyerName.Text = mtInvoice.BuyerName;
			_buyerLegalAddress.Text = mtInvoice.BuyerLegalAddress;
			_buyerBusinessAddress.Text = mtInvoice.BuyerBusinessAddress;
			_buyerSettlAcc.Code = mtInvoice.BuyerSettlAcc;
			_buyerTaxCode.Text = mtInvoice.BuyerTaxCode;
			_buyerMngPost.EditValue = mtInvoice.BuyerMngPost;
			_buyerMngName.Text = mtInvoice.BuyerMngName;
			_buyerChiefAccPost.EditValue = mtInvoice.BuyerChiefAccPost;
			_buyerChiefAccName.Text = mtInvoice.BuyerChiefAccName;
			_buyerStateRegNum.Text = mtInvoice.BuyerStateRegNum;
			_buyerIDDocType.Code = mtInvoice.BuyerIDDocType;
			_buyerPassport.Text = mtInvoice.BuyerPassport;
			_buyerAdditionalData.Text = mtInvoice.BuyerAdditionalData;
			_buyerShippingMethod.Text = mtInvoice.BuyerShippingMethod;
			_byWhomFio.Text = mtInvoice.ByWhomFio;
			_authorizationNumber.Text = mtInvoice.AuthorizationNumber;
			_authorizationDate.EditValue = mtInvoice.AuthorizationDate;
		}
		else
		{
			if (BaseDocument.POType == ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTBill.Value)
			{
				mtBillBase = (MTBill)BaseDocument;
				if (!string.IsNullOrWhiteSpace(mtBillBase.PrepaymentGroup.PartnerCode) && (!(mtBillBase.PrepaymentGroup.PartnerCode == mtBillBase.PartnerContractGroup.PartnerCode) || !(mtBillBase.PrepaymentGroup.ContractCode == mtBillBase.PartnerContractGroup.ContractCode)))
				{
					throw new ASException(ArmSoft.SME.Trade.Common.Resources.DifferentPrepaymentPartner);
				}
				_cashDesk.IsReadOnly = false;
				_baseECRByCardSum.Value = mtBillBase.SummaPlCard.Value;
				_baseECRByPrePaymentSum.Value = mtBillBase.SummaPrePayment.Value;
				_baseECRByCashSum.Value = mtBillBase.SummaCash.Value + mtBillBase.SummaCredit.Value + mtBillBase.SummaPayByBonus.Value - mtBillBase.SummaChange.Value;
				PopupCalcEditExt ecrReturnByCardSum = _ecrReturnByCardSum;
				decimal? summaPlCard = mtBillBase.SummaPlCard;
				ecrReturnByCardSum.IsReadOnly = (summaPlCard.GetValueOrDefault() == 0m) & summaPlCard.HasValue;
				_ecrReturnByCashSum.IsReadOnly = _baseECRByCashSum.Value == 0m;
				PopupCalcEditExt ecrReturnByPrePaymentSum = _ecrReturnByPrePaymentSum;
				summaPlCard = mtBillBase.SummaPrePayment;
				ecrReturnByPrePaymentSum.IsReadOnly = (summaPlCard.GetValueOrDefault() == 0m) & summaPlCard.HasValue;
				summaPlCard = mtBillBase.SummaPlCard;
				if ((summaPlCard.GetValueOrDefault() == 0m) & summaPlCard.HasValue)
				{
					summaPlCard = mtBillBase.SummaCredit;
					if ((summaPlCard.GetValueOrDefault() == 0m) & summaPlCard.HasValue)
					{
						summaPlCard = mtBillBase.SummaPayByBonus;
						if ((summaPlCard.GetValueOrDefault() == 0m) & summaPlCard.HasValue)
						{
							summaPlCard = mtBillBase.SummaPrePayment;
							if ((summaPlCard.GetValueOrDefault() == 0m) & summaPlCard.HasValue)
							{
								_ecrReturnByCashSum.Value = _returnECRTotalSum.Value;
								goto IL_08f7;
							}
						}
					}
				}
				_ecrReturnByCashSum.Value = 0m;
				goto IL_08f7;
			}
			if (BaseDocument.POType == (short)ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTSaleRetail)
			{
				MTInvoiceRetail mtInvoiceRetail = (MTInvoiceRetail)BaseDocument;
				if (!string.IsNullOrWhiteSpace(mtInvoiceRetail.DealType))
				{
					_dealType.Code = mtInvoiceRetail.DealType;
					if (_vatType.Code == "3" || _vatType.Code == "4")
					{
						short? num3 = (from accDealTypesSE in SmartEnum<AccDealTypesSE, short>.List
							where accDealTypesSE.ValueUI.Trim() == mtInvoiceRetail.DealType.Trim()
							select accDealTypesSE.Value).FirstOrDefault();
						_dealTypeKey = num3.Value.ToString();
					}
					else
					{
						short? num4 = (from invoiceDealTypesSE in SmartEnum<InvoiceDealTypesSE, short>.List
							where invoiceDealTypesSE.ValueUI.Trim() == mtInvoiceRetail.DealType.Trim()
							select invoiceDealTypesSE.Value).FirstOrDefault();
						_dealTypeKey = num4.Value.ToString();
					}
				}
				_salesConsultant.Code = mtInvoiceRetail.SalesConsultant;
				if (_currCode.Code == "AMD")
				{
					_currExChngRate.Rate = 1m;
					_currExChngRate.Base = 1m;
				}
				LoadMTList(mtInvoiceRetail.MTList);
				_supplierName.EditValue = mtInvoiceRetail.SupplierName;
				_supplierLegalAddress.EditValue = mtInvoiceRetail.SupplierLegalAddress;
				_supplierTaxCode.EditValue = mtInvoiceRetail.SupplierTaxCode;
				_supplierBusinessAddress.Text = mtInvoiceRetail.SupplierBusinessAddress;
				_supplierSettlAcc.Code = mtInvoiceRetail.SupplierSettlAcc;
				_supplierMngPost.EditValue = mtInvoiceRetail.SupplierMngPost;
				_supplierMngName.Text = mtInvoiceRetail.SupplierMngName;
				_supplierChiefAccPost.EditValue = mtInvoiceRetail.SupplierChiefAccPost;
				_supplierChiefAccName.Text = mtInvoiceRetail.SupplierChiefAccName;
				_buyerName.Text = mtInvoiceRetail.BuyerName;
				_buyerLegalAddress.Text = mtInvoiceRetail.BuyerLegalAddress;
				_buyerBusinessAddress.Text = mtInvoiceRetail.BuyerBusinessAddress;
				_buyerSettlAcc.Code = mtInvoiceRetail.BuyerSettlAcc;
				_buyerTaxCode.Text = mtInvoiceRetail.BuyerTaxCode;
				_buyerMngPost.EditValue = mtInvoiceRetail.BuyerMngPost;
				_buyerMngName.Text = mtInvoiceRetail.BuyerMngName;
				_buyerChiefAccPost.EditValue = mtInvoiceRetail.BuyerChiefAccPost;
				_buyerChiefAccName.Text = mtInvoiceRetail.BuyerChiefAccName;
				_buyerStateRegNum.Text = mtInvoiceRetail.BuyerStateRegNum;
				_buyerIDDocType.Code = mtInvoiceRetail.BuyerIDDocType;
				_buyerPassport.Text = mtInvoiceRetail.BuyerPassport;
				_buyerAdditionalData.Text = mtInvoiceRetail.BuyerAdditionalData;
				_buyerShippingMethod.Text = mtInvoiceRetail.BuyerShippingMethod;
				_byWhomFio.Text = mtInvoiceRetail.ByWhomFio;
				_authorizationNumber.Text = mtInvoiceRetail.AuthorizationNumber;
				_authorizationDate.EditValue = mtInvoiceRetail.AuthorizationDate;
			}
		}
		goto IL_0e82;
		IL_0e82:
		_itemCodeCol.ReadOnly = true;
		_unitCodeCol.ReadOnly = true;
		_mtRetFromCustGrid.AllowAdd = false;
		_mtRetFromCustGrid.AllowDuplicate = false;
		if (BaseDocument.POType != ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTBill.Value)
		{
			_ecrReturnByCashSum.Value = 0m;
			_cashDesk.Code = string.Empty;
			_ecrReturnByCashSum.IsReadOnly = true;
			_cashDesk.IsReadOnly = true;
		}
		_mtRetFromCustGrid.Grid.RefreshData();
		CalcSummAndSummVAT();
		TotalSumma_EditValueChanged();
		return;
		IL_08f7:
		_vatType.Code = mtBillBase.VATType;
		if (!string.IsNullOrWhiteSpace(mtBillBase.DealType))
		{
			_dealType.Code = mtBillBase.DealType;
			if (_vatType.Code == "3" || _vatType.Code == "4")
			{
				short? num5 = (from accDealTypesSE in SmartEnum<AccDealTypesSE, short>.List
					where accDealTypesSE.ValueUI.Trim() == mtBillBase.DealType.Trim()
					select accDealTypesSE.Value).FirstOrDefault();
				_dealTypeKey = num5.Value.ToString();
			}
			else
			{
				short? num6 = (from invoiceDealTypesSE in SmartEnum<InvoiceDealTypesSE, short>.List
					where invoiceDealTypesSE.ValueUI.Trim() == mtBillBase.DealType.Trim()
					select invoiceDealTypesSE.Value).FirstOrDefault();
				_dealTypeKey = num6.Value.ToString();
			}
		}
		LoadMTList(mtBillBase.MTList);
		_salesConsultant.Code = mtBillBase.SalesConsultant;
		_baseECRCheckNum.Text = mtBillBase.ECRCheckNum;
		_sumFromRound = mtBillBase.SumFromRound;
		FillSupplierRekvizits();
		goto IL_0e82;
	}

	private void LoadMTList(List<MTInvoiceBase.MTInvoiceSpecificationRow> mtList)
	{
		_baseDocList = mtList;
		_mtRetFromCustList.Clear();
		_maxRowID = 0;
		foreach (MTInvoiceBase.MTInvoiceSpecificationRow mt in mtList)
		{
			if (!mt.IsDeleted)
			{
				MTRetFromCustSpecificationRow mTRetFromCustSpecificationRow = new MTRetFromCustSpecificationRow(this)
				{
					ItemType = mt.ItemType,
					Storage = ((mt.ItemType == ItemTypesSE.Goods.Value) ? mt.Storage : string.Empty),
					ItemID = mt.ItemID,
					ItemCode = mt.ItemCode,
					ItemName = mt.ItemName,
					FullName = mt.FullName,
					UnitCode = mt.UnitCode,
					UnitBrief = mt.UnitBrief,
					Quantity = mt.Quantity,
					Discount = mt.Discount,
					Price = mt.DiscountPrice,
					Summa = mt.Summa,
					BeginSumma = mt.BeginSumma,
					BeginPrice = mt.Price,
					UsedBonus = mt.UsedBonus,
					AccumulatedBonus = mt.AccumulatedBonus,
					VAT = mt.VAT.Value,
					DealType = mt.DealType,
					RowDescription = mt.RowDescription,
					RowID = mt.RowID
				};
				if (mTRetFromCustSpecificationRow.UsedBonus != 0m || mTRetFromCustSpecificationRow.AccumulatedBonus != 0m)
				{
					_bonusUsed = true;
				}
				if (!string.IsNullOrEmpty(_storage.Code) && mTRetFromCustSpecificationRow.ItemType == GoodsTypesSE.Good.Value)
				{
					mTRetFromCustSpecificationRow.Storage = _storage.Code;
				}
				_mtRetFromCustList.Add(mTRetFromCustSpecificationRow);
				_maxRowID++;
			}
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
		Ribbon.ToolbarItemLinks.Add(_barButtonPrintLabel.ConvertForToolbar("t_action_16.svg"));
		Ribbon.ToolbarItemLinks.Add(_barSubItemJournals.ConvertForToolbar("t_report_16.svg"));
	}

	protected override void AddItemsToRibbon()
	{
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0101: Unknown result type (might be due to invalid IL or missing references)
		//IL_0117: Unknown result type (might be due to invalid IL or missing references)
		//IL_011d: Unknown result type (might be due to invalid IL or missing references)
		base.AddItemsToRibbon();
		CreateRibbonPageGroupActions();
		CreateRibbonPageGroupReports();
		_barButtonJumpTo = new ReportBarButtonItem(ArmSoft.SME.Trade.Common.SmartEnums.DataProviderTypeSE.AllDocumentsJournal, showDialog: false, ArmSoft.SME.Common.Resources.JumpTo, null, "doc_JumpTo_16.svg", "doc_JumpTo_32.svg");
		_barButtonJumpTo.AddInBarSubItem(ref _ribbonPageGroupActions, ref _barSubItemActions);
		_barButtonPrintLabel = new BarButtonItem
		{
			Content = ArmSoft.SME.Trade.Common.Resources.PrintLabel,
			Glyph = ImageService.GetImage("action_16.svg"),
			LargeGlyph = ImageService.GetImage("action_32.svg")
		};
		_barButtonPrintLabel.ItemClick += new SafeDocumentEventHandler<ItemClickEventArgs>(BarButtonPrintLabel_ItemClick, this).Handler;
		_ribbonPageGroupActions.ItemLinks.Add(_barButtonPrintLabel.CreateLink());
		_barSubItemJournals = new BarSubItem
		{
			Content = string.Empty,
			Glyph = ImageService.GetImage("report_16.svg"),
			LargeGlyph = ImageService.GetImage("report_32.svg")
		};
		_barButtonMTSalesAnalysis = new ReportBarButtonItem(ArmSoft.SME.Trade.Common.SmartEnums.DataProviderTypeSE.MatRepSalesAnalysis.Value, showDialog: false, ArmSoft.SME.Trade.Common.Resources.MTRepSalesAnalysis);
		_barButtonMTSalesAnalysis.AddInBarSubItem(ref _ribbonPageGroupReports, ref _barSubItemJournals);
		_barButtonGoodsOperationsJournal = new ReportBarButtonItem(ArmSoft.SME.Trade.Common.SmartEnums.DataProviderTypeSE.GoodsRepOperations.Value, showDialog: false, ArmSoft.SME.Trade.Common.Resources.MTOperations);
		_barButtonGoodsOperationsJournal.AddInBarSubItem(ref _ribbonPageGroupReports, ref _barSubItemJournals);
		_barButtonGoodsRetailOperationJournal = new ReportBarButtonItem(ArmSoft.SME.Trade.Common.SmartEnums.DataProviderTypeSE.MatRepOperationsRetail.Value, showDialog: false, ArmSoft.SME.Trade.Common.Resources.MTOperationsRetail);
		_barButtonGoodsRetailOperationJournal.AddInBarSubItem(ref _ribbonPageGroupReports, ref _barSubItemJournals);
		_barButtonPartnerOperationJournal = new ReportBarButtonItem(ArmSoft.SME.Trade.Common.SmartEnums.DataProviderTypeSE.PartOpJournal.Value, showDialog: false, ArmSoft.SME.Trade.Common.Resources.PartnerOp);
		_barButtonPartnerOperationJournal.AddInBarSubItem(ref _ribbonPageGroupReports, ref _barSubItemJournals);
		_barButtonBonusOperationJournal = new ReportBarButtonItem(ArmSoft.SME.Trade.Common.SmartEnums.DataProviderTypeSE.BonusOpJournal.Value, showDialog: false, ArmSoft.SME.Trade.Common.Resources.BonusOp);
		_barButtonBonusOperationJournal.AddInBarSubItem(ref _ribbonPageGroupReports, ref _barSubItemJournals);
		_barButtonCashOperationJournal = new ReportBarButtonItem(ArmSoft.SME.Trade.Common.SmartEnums.DataProviderTypeSE.CashOperations.Value, showDialog: false, ArmSoft.SME.Trade.Common.Resources.KassaOp);
		_barButtonCashOperationJournal.AddInBarSubItem(ref _ribbonPageGroupReports, ref _barSubItemJournals);
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
		ReportBarButtonItem barButtonMTSalesAnalysis = _barButtonMTSalesAnalysis;
		num = 3;
		List<PropertyWithValue> list2 = new List<PropertyWithValue>(num);
		CollectionsMarshal.SetCount(list2, num);
		Span<PropertyWithValue> span2 = CollectionsMarshal.AsSpan(list2);
		span2[0] = new PropertyWithValue("DocISN", PO.ISN.ToString());
		span2[1] = new PropertyWithValue("AdditionalCaption", GetReportsAdditionalCaption());
		span2[2] = new PropertyWithValue("ShowNCCVATSumm", true.ToString());
		barButtonMTSalesAnalysis.PropertyWithValues = list2;
		ReportBarButtonItem barButtonGoodsOperationsJournal = _barButtonGoodsOperationsJournal;
		num = 2;
		List<PropertyWithValue> list3 = new List<PropertyWithValue>(num);
		CollectionsMarshal.SetCount(list3, num);
		Span<PropertyWithValue> span3 = CollectionsMarshal.AsSpan(list3);
		span3[0] = new PropertyWithValue("DocISN", PO.ISN.ToString());
		span3[1] = new PropertyWithValue("AdditionalCaption", GetReportsAdditionalCaption());
		barButtonGoodsOperationsJournal.PropertyWithValues = list3;
		ReportBarButtonItem barButtonGoodsRetailOperationJournal = _barButtonGoodsRetailOperationJournal;
		num = 2;
		List<PropertyWithValue> list4 = new List<PropertyWithValue>(num);
		CollectionsMarshal.SetCount(list4, num);
		Span<PropertyWithValue> span4 = CollectionsMarshal.AsSpan(list4);
		span4[0] = new PropertyWithValue("DocISN", PO.ISN.ToString());
		span4[1] = new PropertyWithValue("AdditionalCaption", GetReportsAdditionalCaption());
		barButtonGoodsRetailOperationJournal.PropertyWithValues = list4;
		ReportBarButtonItem barButtonPartnerOperationJournal = _barButtonPartnerOperationJournal;
		num = 3;
		List<PropertyWithValue> list5 = new List<PropertyWithValue>(num);
		CollectionsMarshal.SetCount(list5, num);
		Span<PropertyWithValue> span5 = CollectionsMarshal.AsSpan(list5);
		span5[0] = new PropertyWithValue("DocISN", PO.ISN.ToString());
		span5[1] = new PropertyWithValue("AdditionalCaption", GetReportsAdditionalCaption());
		span5[2] = new PropertyWithValue("ShowLinks", true.ToString());
		barButtonPartnerOperationJournal.PropertyWithValues = list5;
		ReportBarButtonItem barButtonBonusOperationJournal = _barButtonBonusOperationJournal;
		num = 2;
		List<PropertyWithValue> list6 = new List<PropertyWithValue>(num);
		CollectionsMarshal.SetCount(list6, num);
		Span<PropertyWithValue> span6 = CollectionsMarshal.AsSpan(list6);
		span6[0] = new PropertyWithValue("DocISN", PO.ISN.ToString());
		span6[1] = new PropertyWithValue("AdditionalCaption", GetReportsAdditionalCaption());
		barButtonBonusOperationJournal.PropertyWithValues = list6;
		ReportBarButtonItem barButtonCashOperationJournal = _barButtonCashOperationJournal;
		num = 3;
		List<PropertyWithValue> list7 = new List<PropertyWithValue>(num);
		CollectionsMarshal.SetCount(list7, num);
		Span<PropertyWithValue> span7 = CollectionsMarshal.AsSpan(list7);
		span7[0] = new PropertyWithValue("DocISN", PO.ISN.ToString());
		span7[1] = new PropertyWithValue("AdditionalCaption", GetReportsAdditionalCaption());
		span7[2] = new PropertyWithValue("ReportCurrTypes", "1");
		barButtonCashOperationJournal.PropertyWithValues = list7;
	}

	protected override void SetReportBarButtonsEnable(bool isEnabled)
	{
		base.SetReportBarButtonsEnable(isEnabled);
		_barButtonJumpTo.IsEnabled = isEnabled;
		_barButtonMTSalesAnalysis.IsEnabled = isEnabled;
		_barButtonBonusOperationJournal.IsEnabled = isEnabled;
		_barButtonGoodsOperationsJournal.IsEnabled = isEnabled;
		_barButtonGoodsRetailOperationJournal.IsEnabled = isEnabled;
		_barButtonPartnerOperationJournal.IsEnabled = isEnabled;
		_barButtonCashOperationJournal.IsEnabled = isEnabled;
	}

	protected override void CreateChildDocument(short childDocType, MTReturnFromCustomer baseDoc)
	{
		base.CreateChildDocument(childDocType, baseDoc);
		CreateBasedOnDocHelper.CreateDocument((ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE)(SmartEnum<ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE, short>)childDocType, PO.ISN, DVPID);
	}

	private void BarButtonPrintLabel_ItemClick(object sender, ItemClickEventArgs e)
	{
		if (_mtRetFromCustList.Count > 0)
		{
			FillDocument(new StoreRequest());
			new PrintLabelDialog(this, PO).Invoke();
		}
	}

	protected override void AfterStore(StoreRequest sr, bool isNewDocument)
	{
		base.AfterStore(sr, isNewDocument);
		PartnerCacheRow.ContractRow contract = TradeSettings.TradeCacheService.Partner.GetContract(PO.PartnerID, PO.PartnerContractGroup.ContractCode);
		if (PO.PartnerID == -1 || contract == null || !contract.IsBonusCard || string.IsNullOrWhiteSpace(contract.DiscountCard) || !_isConnectedToDS)
		{
			return;
		}
		if (PO.State != ArmSoft.SME.Trade.Common.SmartEnums.DocumentStateSE.Draft.Value && PO.State != ArmSoft.SME.Trade.Common.SmartEnums.DocumentStateSE.Deleted.Value)
		{
			if (PO.TransactionISN == Guid.Empty)
			{
				CardTransactionRequest cardTransactionRequest;
				CardTransactionRequest request = (cardTransactionRequest = new CardTransactionRequest());
				cardTransactionRequest.CardNumber = contract.DiscountCard;
				cardTransactionRequest.DocInfo = new CardTransactionRequestDocInfo
				{
					ISN = PO.ISN,
					Type = DocType.Return,
					Date = PO.DocDate.ToDateTime(),
					Number = PO.DocNum,
					Amount = PO.Summ.Value,
					CashCode = PO.CashDesk,
					ShiftNumber = PO.CashierShiftNumber,
					User = Settings.LoginService.UserName
				};
				cardTransactionRequest.Rows = new List<CardTransactionRow>();
				if (PO.AccumulatedBonusSum != 0m)
				{
					cardTransactionRequest.Rows.Add(new CardTransactionRow
					{
						Op = OpType.Accumulate,
						Amount = -PO.AccumulatedBonusSum
					});
				}
				if (PO.UsedBonusSum != 0m)
				{
					cardTransactionRequest.Rows.Add(new CardTransactionRow
					{
						Op = OpType.Use,
						Amount = PO.UsedBonusSum
					});
				}
				CardTransaction cardTransaction = DiscountServerService.RecordDiscountServerTransaction(request);
				if (cardTransaction != null)
				{
					PO.TransactionISN = cardTransaction.ISN;
					InitContextFunctionsHelper.UpdateDocTransactionISN(PO, cardTransaction.ISN);
				}
			}
		}
		else if (PO.StoredInDb && PO.TransactionISN != Guid.Empty)
		{
			DiscountServerService.DeleteTransactionsByISN(PO.TransactionISN);
			PO.TransactionISN = Guid.Empty;
			InitContextFunctionsHelper.UpdateDocTransactionISN(PO, Guid.Empty);
		}
	}

	private void SetCashierShiftNumber()
	{
		if (!string.IsNullOrWhiteSpace(_cashDesk.Code))
		{
			_cashierShift.Text = TradeSettings.CreateTradeClient().CommonServices.GetOpenCashierShiftNumberByCashDesk(_cashDesk.Code);
		}
		else
		{
			_cashierShift.Text = "";
		}
	}

	private void PrintECRCheck_Click(object sender, RoutedEventArgs e)
	{
		OnF3();
	}

	protected override void OnF3()
	{
		if (_printECRCheck.IsEnabled)
		{
			try
			{
				PrintECRReturnCheck();
			}
			catch (Exception ex)
			{
				Settings.ErrorHandler(ex);
			}
		}
	}

	private void PrintECRReturnCheck()
	{
		CashCacheRow cashRow = TradeSettings.TradeCacheService.Cash.GetItem(_cashDesk.Code);
		CashCacheRow cashRow2 = null;
		if (BaseDocument != null && BaseDocument.POType == ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTBill.Value)
		{
			if (!string.IsNullOrWhiteSpace(((MTBill)BaseDocument).PrePaymentECRCheckNum))
			{
				throw new ASException(ArmSoft.SME.Trade.Common.Resources.ReturnToSupplierNotAllowedWithBonusAccumulation);
			}
			if (string.IsNullOrWhiteSpace(_cashDesk.Code))
			{
				cashRow = TradeSettings.TradeCacheService.Cash.GetItem(BaseDocument.CashDesk);
			}
			cashRow2 = (string.IsNullOrWhiteSpace(BaseDocument.CashDesk) ? cashRow : TradeSettings.TradeCacheService.Cash.GetItem(BaseDocument.CashDesk));
		}
		if (ECRPreliminaryChecksAndInitalization(cashRow, (BaseDocument != null) ? ((MTBill)BaseDocument).ECRCRN : ""))
		{
			PosTerminalResponse posTerminalResponse = RefundPOSTerminalIfChosen();
			ECRPrintCheckWindow eCRPrintCheckWindow = new ECRPrintCheckWindow(this, _ecr, cashRow.ECROperator, cashRow.ECROperatorPassword, cashRow.ShowECRTracking, CreateReturnRequest(cashRow2, posTerminalResponse), ECRPrintCheckWindow.ECRCheckType.Return, (ECRPrintCheckWindow window, Utilities.PrintReceiptResponse response) =>
			{
				ECRHelpers.PrintOnOtherPrinter(window, response, cashRow, PO, $"{ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTReturnFromCustomer.Value}.3");
			});
			if (eCRPrintCheckWindow.Succeeded)
			{
				UpdateDocAndWindowAfterCheckPrint(eCRPrintCheckWindow.CheckNumber);
			}
		}
	}

	private bool ECRPreliminaryChecksAndInitalization(CashCacheRow cashRow, string baseDocECRCRN)
	{
		if (_date.EditValue != TradeSettings.CreateTradeClient().CommonServices.GetServerDate().ToDateOnly())
		{
			throw new ASException(ArmSoft.SME.Common.Resources.DocDateNotCurrent);
		}
		if (cashRow == null)
		{
			throw new ASException(ArmSoft.SME.Common.Resources.ValidEmptyCash);
		}
		if (string.IsNullOrWhiteSpace(_baseECRCheckNum.Text))
		{
			ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Common.Resources.BaseDocNeedsForECRC, ArmSoft.SME.Trade.Common.Resources.ProgramName, MessageBoxButton.OK, MessageBoxImage.Asterisk);
			return false;
		}
		if (!IsValidReturnSums())
		{
			return false;
		}
		if (string.IsNullOrWhiteSpace(cashRow.ECROperator) || string.IsNullOrWhiteSpace(cashRow.ECROperatorPassword))
		{
			ArmSoft.SME.Core.UI.MessageBox.Show(string.Format(ArmSoft.SME.Trade.Common.Resources.ValidateECROperatorOrECROperatorPin, _cashDesk.Code), ArmSoft.SME.Common.Resources.ECRReturnCheck, MessageBoxButton.OK, MessageBoxImage.Asterisk);
			return false;
		}
		if (!cashRow.ECROperator.IsNumeric())
		{
			ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Common.Resources.ECRInitFailed, ArmSoft.SME.Common.Resources.ECRReturnCheck, MessageBoxButton.OK, MessageBoxImage.Asterisk);
			return false;
		}
		if (string.IsNullOrWhiteSpace(baseDocECRCRN) && string.IsNullOrWhiteSpace(cashRow.ECRCrn))
		{
			throw new ASException(string.Format(ArmSoft.SME.Common.Resources.FieldIsEmpty, ArmSoft.SME.Common.Resources.ECRCrn));
		}
		if (State == ArmSoft.SME.Trade.Common.SmartEnums.DocumentStateSE.Deleted.Value)
		{
			ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Common.Resources.MustBeSaveDocBeforePrintECRCheck, ArmSoft.SME.Common.Resources.ECRReturnCheck, MessageBoxButton.OK, MessageBoxImage.Asterisk);
			return false;
		}
		if ((!StoredInDb || IsChanged || State == ArmSoft.SME.Trade.Common.SmartEnums.DocumentStateSE.Draft.Value) && !SaveDocument())
		{
			return false;
		}
		ECRHelpers.InitECR(ref _ecr, _cashDesk.Code);
		if (_ecr == null)
		{
			ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Common.Resources.ECRInitFailed, ArmSoft.SME.Common.Resources.ECRReturnCheck, MessageBoxButton.OK, MessageBoxImage.Asterisk);
			return false;
		}
		return true;
	}

	private bool IsValidReturnSums()
	{
		decimal value = _ecrReturnByCardSum.Value;
		decimal value2 = _ecrReturnByCashSum.Value;
		decimal value3 = _ecrReturnByPrePaymentSum.Value;
		decimal num = 0m;
		if (Settings.ParametersService.ECRCheckPrintBonusType() == PrintBonusTypesSE.Cash.Value)
		{
			num = CalcUsedBonusSumm();
		}
		if (value + value2 + num + value3 == 0m)
		{
			ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Trade.Common.Resources.ReturnSumsNotSet, ArmSoft.SME.Common.Resources.ECRReturnCheck, MessageBoxButton.OK, MessageBoxImage.Asterisk);
			return false;
		}
		if (value + value2 + num + value3 < _returnECRTotalSum.Value)
		{
			ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Trade.Common.Resources.ReturnSumLessThanTotalSum, ArmSoft.SME.Common.Resources.ECRReturnCheck, MessageBoxButton.OK, MessageBoxImage.Asterisk);
			return false;
		}
		if (value + value2 + num + value3 > _returnECRTotalSum.Value)
		{
			ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Trade.Common.Resources.ReturnSumGreaterThanTotalSum, ArmSoft.SME.Common.Resources.ECRReturnCheck, MessageBoxButton.OK, MessageBoxImage.Asterisk);
			return false;
		}
		if (value > _baseECRByCardSum.Value)
		{
			ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Trade.Common.Resources.ReturnByCardSumGreaterThanBaseDocByCardSum, ArmSoft.SME.Common.Resources.ECRReturnCheck, MessageBoxButton.OK, MessageBoxImage.Asterisk);
			return false;
		}
		if (value3 > _baseECRByPrePaymentSum.Value)
		{
			ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Trade.Common.Resources.ReturnByPrePaymentSumGreaterThanBaseDocByPrePaymentSum, ArmSoft.SME.Common.Resources.ECRReturnCheck, MessageBoxButton.OK, MessageBoxImage.Asterisk);
			return false;
		}
		if (value2 > _baseECRByCashSum.Value)
		{
			ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Trade.Common.Resources.ReturnByCashSumGreaterThanBaseDocByCashSum, ArmSoft.SME.Common.Resources.ECRReturnCheck, MessageBoxButton.OK, MessageBoxImage.Asterisk);
			return false;
		}
		return true;
	}

	private Utilities.PrintReturnReceiptRequest CreateReturnRequest(CashCacheRow cashRow, PosTerminalResponse posTerminalResponse)
	{
		List<Utilities.ReturnItem> list = new List<Utilities.ReturnItem>();
		Dictionary<int, int> eCRRowNumByRowIdDictionary = ECRHelpers.GetECRRowNumByRowIdDictionary(_baseDocList);
		MTBill mTBill = (MTBill)BaseDocument;
		foreach (MTReturnFromCustomerBase.MTRetFromCustSpecificationRow mT in PO.MTList)
		{
			Utilities.ReturnItem returnItem = new Utilities.ReturnItem();
			returnItem.rpid = eCRRowNumByRowIdDictionary[mT.RowID.Value];
			returnItem.quantity = decimal.ToDouble(mT.Quantity.Value);
			list.Add(returnItem);
		}
		Utilities.PrintReturnReceiptRequest printReturnReceiptRequest = new Utilities.PrintReturnReceiptRequest();
		if (BaseDocument != null && BaseDocument.POType == ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTBill.Value)
		{
			string eCRCRN = mTBill.ECRCRN;
			if (string.IsNullOrWhiteSpace(eCRCRN))
			{
				printReturnReceiptRequest.crn = cashRow.ECRCrn;
			}
			else
			{
				printReturnReceiptRequest.crn = eCRCRN;
			}
		}
		printReturnReceiptRequest.returnTicketId = Convert.ToInt32(PO.ECRBaseCheckNum);
		printReturnReceiptRequest.cardAmountForReturn = _ecrReturnByCardSum.Value;
		printReturnReceiptRequest.cashAmountForReturn = _ecrReturnByCashSum.Value + PO.SummaPayByBonus;
		printReturnReceiptRequest.prePaymentAmountForReturn = _ecrReturnByPrePaymentSum.Value;
		printReturnReceiptRequest.returnItemList = list;
		if (posTerminalResponse != null && !string.IsNullOrWhiteSpace(posTerminalResponse.RRN))
		{
			printReturnReceiptRequest.rrn = posTerminalResponse.RRN;
			if (!string.IsNullOrWhiteSpace(posTerminalResponse.TerminalId))
			{
				printReturnReceiptRequest.terminalId = posTerminalResponse.TerminalId;
			}
		}
		return printReturnReceiptRequest;
	}

	private PosTerminalResponse RefundPOSTerminalIfChosen()
	{
		PosTerminalResponse result = null;
		if (BaseDocument.POType != ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTBill.Value || PO.PaymentRefundAccepted)
		{
			return result;
		}
		MTBill mTBill = (MTBill)BaseDocument;
		string rrn;
		if (string.IsNullOrWhiteSpace(mTBill.PaymentId))
		{
			rrn = "";
		}
		else
		{
			rrn = (mTBill.PaymentId.Contains(':') ? mTBill.PaymentId.Split(':')[1].Trim() : mTBill.PaymentId.Trim());
		}
		if (mTBill.POSTerminalType == "2")
		{
			result = Arcus2Helpers.CheckAndRefund(mTBill.POSTerminalID, _ecrReturnByCardSum.Value, mTBill.SummaPlCard.Value, rrn);
		}
		UpdatePaymentStatus();
		return result;
	}

	private void UpdatePaymentStatus()
	{
		try
		{
			PO.PaymentRefundAccepted = true;
			int num = 1;
			List<PropertyWithValue> list = new List<PropertyWithValue>(num);
			CollectionsMarshal.SetCount(list, num);
			CollectionsMarshal.AsSpan(list)[0] = new PropertyWithValue("PaymentRefundAccepted", PO.PaymentRefundAccepted.ToString());
			List<PropertyWithValue> properties = list;
			StoreRequest sr = new StoreRequest
			{
				IsDataChanged = true,
				LogComment = ArmSoft.SME.Common.Resources.IsRefunded,
				WithoutGrids = true,
				WithoutValidation = true,
				WithoutAccessCheck = true,
				RegenerateEntries = false
			};
			PO.TS = Settings.DocumentService.EditFields(PO.ISN, properties, sr, PO.TS);
			LoadDataToControls();
			IsChanged = false;
			UpdateDataViewPanel(DVPID);
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	private void UpdateDocAndWindowAfterCheckPrint(string checkNumber)
	{
		try
		{
			_returnECRCheckNum.Text = checkNumber;
			PO.ECRReturnCheckNum = checkNumber;
			int num = 1;
			List<PropertyWithValue> list = new List<PropertyWithValue>(num);
			CollectionsMarshal.SetCount(list, num);
			CollectionsMarshal.AsSpan(list)[0] = new PropertyWithValue("ECRReturnCheckNum", PO.ECRReturnCheckNum);
			List<PropertyWithValue> editedProperties = list;
			PO.TS = ECRHelpers.ChangeDocECRCheckInfo(checkNumber, string.Empty, POISN(), editedProperties, PO.TS);
			LoadDataToControls();
			IsChanged = false;
			_cashDesk.IsReadOnly = true;
			_bankAcquiringContainer.PartnerCode.IsReadOnly = true;
			_bankAcquiringContainer.Contract.IsReadOnly = true;
			SetControlsReadonly();
			UpdateDataViewPanel(DVPID);
			if (Settings.ParametersService.PrintECRCheckAndNew())
			{
				_viewMode = WindowViewMode.NewAfterSave;
				OnNewAfterSaveUpper();
			}
		}
		catch (Exception)
		{
			throw;
		}
	}
}
