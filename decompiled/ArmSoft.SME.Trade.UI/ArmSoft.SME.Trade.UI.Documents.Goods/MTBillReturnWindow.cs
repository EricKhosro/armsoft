using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using ArmSoft.Arcus2Client;
using ArmSoft.Cloud.Client.Discount;
using ArmSoft.ECRConnector;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Constants;
using ArmSoft.SME.Common.CustomTypes;
using ArmSoft.SME.Common.Enums;
using ArmSoft.SME.Common.Extensions;
using ArmSoft.SME.Common.Models;
using ArmSoft.SME.Common.Models.Documents;
using ArmSoft.SME.Common.PersistableObject;
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
using ArmSoft.SME.Core.UI.Services;
using ArmSoft.SME.Trade.Client.Data.Directories;
using ArmSoft.SME.Trade.Client.Data.DropDowns;
using ArmSoft.SME.Trade.Client.Data.Reports;
using ArmSoft.SME.Trade.Common;
using ArmSoft.SME.Trade.Common.Documents.Definition;
using ArmSoft.SME.Trade.Common.Helpers;
using ArmSoft.SME.Trade.Common.Models.Caches;
using ArmSoft.SME.Trade.Common.Models.Documents;
using ArmSoft.SME.Trade.Common.Parameters;
using ArmSoft.SME.Trade.Common.SmartEnums;
using ArmSoft.SME.Trade.UI.CustomUIElements;
using ArmSoft.SME.Trade.UI.CustomUIElements.UserControls;
using ArmSoft.SME.Trade.UI.DataView;
using ArmSoft.SME.Trade.UI.DropDown;
using ArmSoft.SME.Trade.UI.DropDownDataProvider;
using ArmSoft.SME.Trade.UI.Helpers;
using ArmSoft.SME.Trade.UI.Models;
using ArmSoft.SME.Trade.UI.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.ComponentModel.__Internals;
using DevExpress.Utils;
using DevExpress.Xpf.Bars;
using DevExpress.Xpf.Editors;
using DevExpress.Xpf.Editors.Settings;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.LayoutControl;
using DevExpress.XtraEditors.DXErrorProvider;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Win32;

namespace ArmSoft.SME.Trade.UI.Documents.Goods;

[DocumentWindow(38)]
public class MTBillReturnWindow : TradeDocumentWindow<MTBillReturn>
{
	public class MTRetFromCustSpecificationRow : DocumentGridRow, ISaleRow
	{
		[ObservableProperty]
		private string itemType = "";

		[ObservableProperty]
		private string storage = "";

		[ObservableProperty]
		private string itemCode = "";

		[ObservableProperty]
		private int itemID = -1;

		[ObservableProperty]
		private short? rowNum;

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
		private decimal usedBonus;

		[ObservableProperty]
		private decimal accumulatedBonus;

		[ObservableProperty]
		private decimal summa;

		[ObservableProperty]
		private decimal discountSumma;

		[ObservableProperty]
		private decimal beginSumma;

		[ObservableProperty]
		private decimal beginPrice;

		[ObservableProperty]
		private decimal costPrice;

		[ObservableProperty]
		private decimal costPriceVAT;

		[ObservableProperty]
		private decimal currCostPrice;

		[ObservableProperty]
		private string barCode = "";

		[ObservableProperty]
		private decimal envFeePercent;

		[ObservableProperty]
		private decimal envFeeSumma;

		[ObservableProperty]
		private bool? vAT = false;

		[ObservableProperty]
		private bool isRowMarkable;

		[ObservableProperty]
		private int rowID;

		[ObservableProperty]
		private int? baseRowID = -1;

		[ObservableProperty]
		private bool? isPartySelected;

		[ObservableProperty]
		private string partyMethodName;

		[ObservableProperty]
		private string rowDescription = "";

		[ObservableProperty]
		private string externalCode = "";

		[ObservableProperty]
		private string rowSpecField = "";

		[ObservableProperty]
		private bool isDeleted;

		[ObservableProperty]
		private bool isGift;

		[ObservableProperty]
		private string markingStatus;

		[ObservableProperty]
		private HashSet<string> markingList = new HashSet<string>();

		[ObservableProperty]
		private bool markingHasMismatch;

		internal bool _isUpdatingMarking;

		public bool UpdateRemainsOnInfoPanel = true;

		public string ShortName = "";

		public string FullName = "";

		public bool IsStorageRequired = true;

		public Guid partyBase;

		public int? partyRow;

		public DateOnly? useLife;

		public string Country = "";

		public string Producer = "";

		public string ProducerCaption = "";

		public string Property1 = "";

		public string Property1Caption = "";

		public string Property2 = "";

		public string Property2Caption = "";

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
		public short? RowNum
		{
			get
			{
				return rowNum;
			}
			set
			{
				if (!EqualityComparer<short?>.Default.Equals(rowNum, value))
				{
					OnPropertyChanging(__KnownINotifyPropertyChangingArgs.RowNum);
					rowNum = value;
					OnPropertyChanged(__KnownINotifyPropertyChangedArgs.RowNum);
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
		public decimal EnvFeePercent
		{
			get
			{
				return envFeePercent;
			}
			set
			{
				if (!EqualityComparer<decimal>.Default.Equals(envFeePercent, value))
				{
					OnPropertyChanging(__KnownINotifyPropertyChangingArgs.EnvFeePercent);
					envFeePercent = value;
					OnPropertyChanged(__KnownINotifyPropertyChangedArgs.EnvFeePercent);
				}
			}
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public decimal EnvFeeSumma
		{
			get
			{
				return envFeeSumma;
			}
			set
			{
				if (!EqualityComparer<decimal>.Default.Equals(envFeeSumma, value))
				{
					OnPropertyChanging(__KnownINotifyPropertyChangingArgs.EnvFeeSumma);
					envFeeSumma = value;
					OnPropertyChanged(__KnownINotifyPropertyChangedArgs.EnvFeeSumma);
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
		public int? BaseRowID
		{
			get
			{
				return baseRowID;
			}
			set
			{
				if (!EqualityComparer<int?>.Default.Equals(baseRowID, value))
				{
					OnPropertyChanging(__KnownINotifyPropertyChangingArgs.BaseRowID);
					baseRowID = value;
					OnPropertyChanged(__KnownINotifyPropertyChangedArgs.BaseRowID);
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
					OnIsPartySelectedChanged(value);
					OnPropertyChanged(__KnownINotifyPropertyChangedArgs.IsPartySelected);
				}
			}
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public string PartyMethodName
		{
			get
			{
				return partyMethodName;
			}
			[MemberNotNull("partyMethodName")]
			set
			{
				if (!EqualityComparer<string>.Default.Equals(partyMethodName, value))
				{
					OnPropertyChanging(__KnownINotifyPropertyChangingArgs.PartyMethodName);
					partyMethodName = value;
					OnPropertyChanged(__KnownINotifyPropertyChangedArgs.PartyMethodName);
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
		public string ExternalCode
		{
			get
			{
				return externalCode;
			}
			[MemberNotNull("externalCode")]
			set
			{
				if (!EqualityComparer<string>.Default.Equals(externalCode, value))
				{
					OnPropertyChanging(__KnownINotifyPropertyChangingArgs.ExternalCode);
					externalCode = value;
					OnPropertyChanged(__KnownINotifyPropertyChangedArgs.ExternalCode);
				}
			}
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public string RowSpecField
		{
			get
			{
				return rowSpecField;
			}
			[MemberNotNull("rowSpecField")]
			set
			{
				if (!EqualityComparer<string>.Default.Equals(rowSpecField, value))
				{
					OnPropertyChanging(__KnownINotifyPropertyChangingArgs.RowSpecField);
					rowSpecField = value;
					OnPropertyChanged(__KnownINotifyPropertyChangedArgs.RowSpecField);
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

		public MTRetFromCustSpecificationRow()
		{
		}

		public MTRetFromCustSpecificationRow(MTBillReturnWindow docWindow)
			: base(docWindow)
		{
		}

		static MTRetFromCustSpecificationRow()
		{
			Dictionary<string, Action<DocumentGridRow, DevExpress.XtraEditors.DXErrorProvider.ErrorInfo>> dictionary = new Dictionary<string, Action<DocumentGridRow, DevExpress.XtraEditors.DXErrorProvider.ErrorInfo>>
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
						ValidateGoodCode((MTRetFromCustSpecificationRow)row, info);
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
					"BeginPrice",
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
				}
			};
			if (Settings.ParametersService.UseMarking())
			{
				dictionary.Add("MarkingStatus", (DocumentGridRow row, DevExpress.XtraEditors.DXErrorProvider.ErrorInfo info) =>
				{
					ValidateMarkingListCount((MTRetFromCustSpecificationRow)row, info);
				});
			}
			DocumentGridRow.RegisterPropertyValidators(typeof(MTRetFromCustSpecificationRow), cacheValidationResults: true, dictionary);
		}

		private static void ValidateMarkingListCount(MTRetFromCustSpecificationRow row, DevExpress.XtraEditors.DXErrorProvider.ErrorInfo info)
		{
			if (row._isUpdatingMarking)
			{
				return;
			}
			row._isUpdatingMarking = true;
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
			row._isUpdatingMarking = false;
		}

		private static void ValidateStorage(MTRetFromCustSpecificationRow row, DevExpress.XtraEditors.DXErrorProvider.ErrorInfo info)
		{
			if (!((MTBillReturnWindow)row.DocWindow).IsOfflineDoc && row.ItemType == GoodsTypesSE.Good.Value && row.IsStorageRequired)
			{
				TradePropertyValidator.ValidateStorage(row.Storage, info, ArmSoft.SME.Common.Resources.Storage, isRequired: true);
			}
		}

		private static void ValidateGoodCode(MTRetFromCustSpecificationRow row, DevExpress.XtraEditors.DXErrorProvider.ErrorInfo info)
		{
			TradePropertyValidator.ValidateGoodCode(row.ItemCode, info, (StringFieldDefinition)TradeAllDocDefinitions.MTBillReturn.Grids["MTList"].Columns["ItemCode"], verifyingOnlyGoods: false);
		}

		private static void ValidateUnitCode(MTRetFromCustSpecificationRow row, DevExpress.XtraEditors.DXErrorProvider.ErrorInfo info)
		{
			TradePropertyValidator.ValidateGoodUnitCode(row.UnitCode, row.ItemCode, info, TradeAllDocDefinitions.MTBillReturn.Grids["MTList"].Columns["UnitCode"]);
		}

		private static void ValidateQuantity(MTRetFromCustSpecificationRow row, DevExpress.XtraEditors.DXErrorProvider.ErrorInfo info)
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

		private static void ValidatePrice(MTRetFromCustSpecificationRow row, DevExpress.XtraEditors.DXErrorProvider.ErrorInfo info)
		{
			if (!row.IsDeleted && !row.IsGift)
			{
				PropertyValidator.PositiveValidate(row.BeginPrice, info, ArmSoft.SME.Common.Resources.MTTAccSalePrice);
			}
			else
			{
				PropertyValidator.NonNegativeValidate(row.BeginPrice, info, ArmSoft.SME.Common.Resources.MTTAccSalePrice);
			}
		}

		private static void ValidateDiscountPrice(MTRetFromCustSpecificationRow row, DevExpress.XtraEditors.DXErrorProvider.ErrorInfo info)
		{
			PropertyValidator.NonNegativeValidate(row.DiscountPrice, info, ArmSoft.SME.Common.Resources.DiscountedPrice);
		}

		private static void ValidateSumma(MTRetFromCustSpecificationRow row, DevExpress.XtraEditors.DXErrorProvider.ErrorInfo info)
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
			ResetForceValidationBit("Quantity");
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		private void OnPriceChanged(decimal value)
		{
			ResetForceValidationBit("BeginPrice");
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
		private void OnIsPartySelectedChanged(bool? value)
		{
			if (!value.HasValue)
			{
				PartyMethodName = "";
			}
			else if (value.Value)
			{
				PartyMethodName = ArmSoft.SME.Common.Resources.PartySelected;
			}
			else
			{
				PartyMethodName = ArmSoft.SME.Common.Resources.AutoSelection;
			}
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		private void OnMarkingStatusChanged(string value)
		{
			ResetForceValidationBit("MarkingStatus");
		}
	}

	private delegate void RowInfoChangeEventHandler(MTRetFromCustSpecificationRow row);

	private delegate void DelegateSetRowInfoToInfoPanel(MTRetFromCustSpecificationRow row);

	private delegate void DelegateSetRowInfoToPoleDisplay(MTRetFromCustSpecificationRow row);

	private delegate void DelegateSetTotalInfoToPoleDisplay(decimal total, decimal discountSum, decimal cashSum, decimal nonCashSum, decimal usedBonusSum, decimal fromPrePaySumm, decimal creditSaleSumm, decimal sumForPay);

	private delegate void DelegateSetRowCountInfoToPoleDisplay(int rowCount);

	private delegate void DelegateSetBuyerInfoToPoleDisplay(string buyerCode, string buyerName, string contractCode, decimal currentAccumulatedBonus, decimal currentBonusRemain);

	private delegate void DelegateSetChangeInfoToPoleDisplay(decimal change);

	private delegate void DelegateClearInfoPanel();

	private delegate void DelegateClearPoleDisplay(bool clearAll, string messageForPoleDisplay = "");

	private DropDownCommentControl _baseDocument;

	private TextEditExt _ecrCheckNum;

	private TextEditExt _prePaymentECRCheckNum;

	private TextEditExt _inputValue;

	private PopupCalcEditExt _summaCash;

	private PopupCalcEditExt _summaPlCard;

	private PopupCalcEditExt _summaCredit;

	private SafeDocumentEventHandler<EditValueChangedEventArgs> _otherPaymentTypes_EditValueChangedHandler;

	private PopupCalcEditExt _summaPayByBonus;

	private PopupCalcEditExt _summaPrePayment;

	private PopupCalcEditExt _summa;

	private PopupCalcEditExt _summaChange;

	private BarEditItemExt _increase;

	private BarEditItemExt _decrease;

	private BarEditItemExt _weight;

	private Button _salePrice;

	private BarEditItemExt _discount;

	private Button _barCodeEnter;

	private Button _codeEnter;

	private Button _mtListLikeName;

	private Button _qtyEnter;

	private Button _printBill;

	private Button _printECRCheck;

	private Button _plCardPayment;

	private Button _credit;

	private BarEditItemExt _prices;

	private BarEditItemExt _mtFavorites;

	private BarEditItemExt _selectMarking;

	private Button _usePrePayment;

	private Button _summaPayCash;

	private Button _erasePayments;

	private TextEditExt _infoItemName;

	private TextEditExt _infoFormula;

	private ImageEdit _hintImage;

	private TextEditExt _substitutionInfo;

	private InfoDisplayControls _infoDisplayControls;

	private TextEditExt _infoDescription;

	private DropDownControl _currCode;

	private DropDownControl _storage;

	private DropDownControl _cashDesk;

	private SafeDocumentEventHandler<EditValueChangedEventArgs> _cashDeskEditValueChangedEventHandler;

	private TextEditExt _cashierShift;

	private DropDownControl _partner;

	private DropDownControl _salesConsultant;

	private ComboBoxEditExt _comment;

	private DropDownControl _owner;

	private DocumentGridControl _mtBillReturnGrid;

	private TableView _mtBillReturnTableView;

	private BindingList<MTRetFromCustSpecificationRow> _mtBillReturnList;

	private DocumentGridColumn _storageCol;

	private DocumentGridColumn _barCodeCol;

	private DocumentGridColumn _itemCodeCol;

	private DocumentGridColumn _itemNameCol;

	private DocumentGridColumn _quantityCol;

	private DocumentGridColumn _unitCodeCol;

	private DocumentGridColumn _unitCol;

	private DocumentGridColumn _priceCol;

	private DocumentGridColumn _discountCol;

	private DocumentGridColumn _discountPriceCol;

	private DocumentGridColumn _summaCol;

	private DocumentGridColumn _discountSummCol;

	private DocumentGridColumn _usedBonusSummCol;

	private DocumentGridColumn _accumulatedBonusCol;

	private DocumentGridColumn _canceledCol;

	private DocumentGridColumn _rowDescCol;

	private DocumentGridColumn _rowMarkingListCol;

	private DocumentGridColumn _vatCol;

	private DocumentGridColumn _isRowMarkable;

	private int _maxRowID;

	private bool _mDiscountUse;

	private string _contract;

	private string _partCode = "";

	private string _contractCurrency;

	private bool _retail;

	private decimal _summaCashValue;

	private decimal _summaPlCardValue;

	private decimal _summaPayByBonusValue;

	private decimal _summaCreditValue;

	private decimal _summaPrePaymentValue;

	private decimal _calculatedBonusValue;

	private decimal _summaCashSubtractBonus;

	private decimal _summaPlCardSubtractBonus;

	private decimal _summaPrePaymentSubtractBonus;

	private decimal _mQty_Weight;

	private bool _useMarking = Settings.ParametersService.UseMarking();

	private bool _allowChangeDate = Settings.ParametersService.AllowChangeDateInBill();

	private bool _mtBillSimpleMode = Settings.ParametersService.MTBillSimpleMode();

	private bool _useDiscount = Settings.ParametersService.DiscountUse();

	private bool _isCalcDiscountAuto = Settings.ParametersService.CalcDiscountAuto();

	private bool _allowChangeSalePrices = Settings.ParametersService.AllowChangeSalePrices();

	private bool _allowChangePriceType = Settings.ParametersService.AllowChangePriceType();

	private bool _allowChangeDiscountPrices = Settings.ParametersService.AllowChangeDiscountPrice();

	private string _priceListTypeParam = Settings.ParametersService.DefaultMTCheckPriceType();

	private decimal _usedDiscount;

	private bool _storedBonus = Settings.ParametersService.UseBonus();

	private bool _isAllowChangeStorageInBill = Settings.ParametersService.AllowChangeStorageInBill();

	private bool _isAllowChangeCashInBill = Settings.ParametersService.AllowChangeCashInBill();

	private bool _isAllowChangeBuyerInBill = Settings.ParametersService.AllowChangeBuyerInBill();

	private bool _isAllowChangeSaleConsultantInBill = Settings.ParametersService.AllowChangeSaleConsultantInBill();

	private bool _isAllowChangeDocNumberInBill = Settings.ParametersService.AllowChangeDocNumberInBill();

	private bool _isAllowChangeGoodRowInBill = Settings.ParametersService.AllowChangeMTBillGoodRow();

	private bool _iIsShowUnitCodeColInBill = Settings.ParametersService.ShowUnitCodeColInBill();

	private bool _isShowDiscountPercentColInBill = Settings.ParametersService.ShowDiscountPercentColInBill();

	private bool _isShowDiscountedPriceColInBill = Settings.ParametersService.ShowDiscountedPriceColInBill();

	private bool _isShowDiscountSummColInBill = Settings.ParametersService.ShowDiscountSummColInBill();

	private bool _isShowVATColInBill = Settings.ParametersService.ShowVATColInBill();

	private bool _isShowDescrColInBill = Settings.ParametersService.ShowRowDescrColInBill();

	private bool _isShowInfoPanel = Settings.ParametersService.ShowMTBillInfoPanel();

	private bool _isShowItemPictureOnInfoPanel = Settings.ParametersService.ShowItemPictureOnInfoPanel();

	private bool _isShowStorageAvailabilityOnInfoPanel = Settings.ParametersService.ShowStorageAvailabilityOnInfoPanel();

	private bool _isShowWholeAvailabilityOnInfoPanel = Settings.ParametersService.ShowWholeAvailabilityOnInfoPanel();

	private bool _isShowPoleDisplay = Settings.ParametersService.ShowPoleDisplay();

	private bool _isShowStorageColInMTBill = Settings.ParametersService.MTBillAllowDifferentStorages();

	private string _rowAddMode = Settings.ParametersService.ScannerMode();

	private bool _connectTob2barmsoftDiscountServer = Settings.ParametersService.ConnectTob2barmsoftDiscountServer();

	private string _prepaymentPartnerCode = "";

	private string _prepaymentContractCode = "";

	private short _nonCashPaymentMethod;

	private bool _bPartClickDropDown;

	private bool _bPartReadMagneticCard;

	private string _vatType;

	private string _priceListType;

	private string _priceListTypeRoundDgr = "";

	private decimal _tmpDiscountSumm;

	private decimal _sumFromRound;

	private bool _applyLastPrice;

	private bool _searchBarCodeInProperty1;

	private bool _immediatlyEditRow;

	private Dictionary<(int, string), decimal> _availabilityByGoodId = new Dictionary<(int, string), decimal>();

	private SafeDocumentEventHandler<FocusedRowHandleChangedEventArgs> _focusedRowHandleChangedEvent;

	private ImageEdit _itemImage;

	private bool _isPartContractWithBonus;

	private bool _bIsChanged;

	private BarSubItem _barSubItemActions = new BarSubItem();

	private BarSubItem _barSubItemJournals = new BarSubItem();

	private BarButtonItem _barButtonPrintLabel;

	private BarButtonItem _barButtonImportFromFile;

	private BarButtonItem _barButtonDocumentSettings;

	private ReportBarButtonItem _barButtonJumpTo;

	private ReportBarButtonItem _barButtonMTSalesAnalysis;

	private ReportBarButtonItem _barButtonGoodsOperationsJournal;

	private ReportBarButtonItem _barButtonGoodsRetailOperationJournal;

	private ReportBarButtonItem _barButtonPartnerOperationJournal;

	private ReportBarButtonItem _barButtonBonusOperationJournal;

	private ReportBarButtonItem _barButtonCashOperationJournal;

	private BarStaticItem _barStatusConnectToDS;

	private Guid _mBaseISN = Guid.Empty;

	private TradeDocument _mBaseDocument;

	private string _baseECRCheckNum = "";

	private string _basePrePaymentECRCheckNum = "";

	private decimal _baseECRSummPLCard;

	private decimal _baseECRSummCash;

	private decimal _baseECRSummCredit;

	private decimal _baseECRSummPrePayment;

	private decimal _usedBonus;

	private decimal _accumulatedBonus;

	private List<MTInvoiceBase.MTInvoiceSpecificationRow> _baseDocList;

	private MTBillReturn _clonedBillReturn;

	private ECR _ecr;

	private bool _readOnlyAfterCheckPrint;

	protected override double WidthValue => 1090.0;

	protected override double HeightValue => 500.0;

	protected override double MinWidthValue => 620.0;

	protected override double MinHeightValue => 250.0;

	private double _fontSizeLarge => _mtBillSimpleMode ? 24 : 22;

	private TradeDocument BaseDocument
	{
		get
		{
			return _mBaseDocument;
		}
		set
		{
			_mBaseDocument = value;
			if (_mBaseDocument == null)
			{
				_baseDocument.Text = "";
				if (_printECRCheck != null)
				{
					_printECRCheck.IsEnabled = false;
				}
				foreach (MTRetFromCustSpecificationRow mtBillReturn in _mtBillReturnList)
				{
					mtBillReturn.AccumulatedBonus = 0m;
					mtBillReturn.UsedBonus = 0m;
				}
				_accumulatedBonusCol.Visible = false;
				_accumulatedBonusCol.ShowInColumnChooser = false;
				_usedBonusSummCol.Visible = false;
				_usedBonusSummCol.ShowInColumnChooser = false;
			}
			else
			{
				MTBill mTBill = (MTBill)BaseDocument;
				_baseECRCheckNum = mTBill.ECRCheckNum;
				_basePrePaymentECRCheckNum = mTBill.PrePaymentECRCheckNum;
				_baseECRSummPLCard = mTBill.SummaPlCard.Value;
				_baseECRSummCash = mTBill.SummaCash.Value - mTBill.SumAddedBonus.Value;
				_baseECRSummPrePayment = mTBill.SummaPrePayment.Value;
				_baseECRSummCredit = mTBill.SummaCredit.Value;
				if (_printECRCheck != null)
				{
					_printECRCheck.IsEnabled = !string.IsNullOrWhiteSpace(_baseECRCheckNum);
				}
				_baseDocument.Text = ArmSoft.SME.Trade.UI.Helpers.DocumentHelpers.GetDocumentDescription(_mBaseDocument, ArmSoft.SME.Common.Constants.Common.DocumentDescriptionMode.TypeDateNum, _baseECRCheckNum);
			}
		}
	}

	private Guid BaseISN
	{
		get
		{
			return _mBaseISN;
		}
		set
		{
			object obj = value;
			bool baseAlreadySelected = false;
			if (((IComparable)obj).CompareTo((object?)_mBaseISN) != 0)
			{
				if (_mBaseISN != Guid.Empty)
				{
					baseAlreadySelected = true;
				}
				_mBaseISN = value;
				if (_mBaseISN == Guid.Empty)
				{
					BaseDocument = null;
					return;
				}
				BaseDocument = (TradeDocument)Settings.DocumentService.Load(_mBaseISN, withoutAccessCheck: true);
				CreateBasedOnDoc(baseAlreadySelected);
				_usedBonusSummCol.Visible = _storedBonus;
				_usedBonusSummCol.ShowInColumnChooser = _storedBonus;
				_accumulatedBonusCol.Visible = _storedBonus;
				_accumulatedBonusCol.ShowInColumnChooser = _storedBonus;
			}
		}
	}

	private event RowInfoChangeEventHandler _rowInfoChange;

	public MTBillReturnWindow()
	{
	}

	public MTBillReturnWindow(MTBillReturn doc)
		: base(doc)
	{
	}

	protected override bool AllowChangeGridMode()
	{
		return true;
	}

	protected override string NextDocNumberRelatedInfo()
	{
		return _cashDesk.Code;
	}

	protected override void ConstructUI()
	{
		base.ConstructUI();
		_priceListType = _priceListTypeParam;
		string text = "";
		LayoutGroupExt container = AddLayoutGroup(null, LayoutGroupView.Group, Orientation.Horizontal);
		LayoutGroupExt container2 = AddLayoutGroup(container, LayoutGroupView.Group, Orientation.Vertical);
		LayoutGroupExt container3 = AddLayoutGroup(container2, LayoutGroupView.Group, Orientation.Horizontal);
		_date = AddDateControl(container3, Definition.Fields["DocDate"]);
		_date.Margin = new Thickness(0.0, 0.0, 10.0, 0.0);
		_docNum = (_mtBillSimpleMode ? AddButtonEditControl(container2, Definition.Fields["DocNum"], 1) : AddButtonEditControl(container3, Definition.Fields["DocNum"], 1));
		_docNum.AllowMagicButton = true;
		_docNum.MagicToolTip = ArmSoft.SME.Common.Resources.DocNextNumbers;
		_docNum.MagicClick += new SafeDocumentEventHandler<MagicClickEventArgs>(DocNum_MagicClick, this).Handler;
		_docNum.EditValueChanged += new SafeDocumentEventHandler<EditValueChangedEventArgs>(DocNum_EditValueChanged, this).Handler;
		if (IsOfflineDoc)
		{
			_baseDocument = AddDropDownCommentControl(container2, ArmSoft.SME.Common.Resources.BaseDoc, isRequired: false, null, 50, HorizontalAlignment.Left, "", "BaseDocument");
		}
		else
		{
			DocumentsJournal idv = new DocumentsJournal(this, ReportTypesSE.BillDocument, ((TradeDocDefinition)Definition).BaseDocTypes)
			{
				DocType = ((TradeDocDefinition)Definition).BaseDocTypes.First().ToString(),
				DocState = DocumentStateSE.DraftOrProcessed.ValueUI
			};
			_baseDocument = AddDropDownCommentControl(container2, ArmSoft.SME.Common.Resources.BaseDoc, isRequired: true, idv, 50, HorizontalAlignment.Left, "", "BaseDocument");
		}
		_baseDocument.ShowClearButton = true;
		_baseDocument.ShowSearchButton = true;
		_baseDocument.IsTextEditable = false;
		_baseDocument.Width = 600.0;
		_baseDocument.ClickDropDown += new SafeDocumentEventHandler<DropDownControlEventArgs>(BaseDocument_ClickDropDown, this).Handler;
		_baseDocument.EditKeyValueChanged += new SafeDocumentEventHandler<EditKeyValueChangedEventArgs>(BaseDocument_EditKeyValueChanged, this).Handler;
		_baseDocument.BtnClearClicked += new SafeDocumentEventHandler<EditKeyValueChangedEventArgs>(BaseDocumentDelete_Click, this).Handler;
		_baseDocument.BtnSearchClicked += new SafeDocumentEventHandler<EditKeyValueChangedEventArgs>(BaseDocumentSearch_Click, this).Handler;
		_ecrCheckNum = AddStringControl(container2, Definition.Fields["ECRCheckNum"]);
		_ecrCheckNum.Width = 223.0;
		_prePaymentECRCheckNum = AddStringControl(container2, Definition.Fields["PrePaymentECRCheckNum"]);
		_prePaymentECRCheckNum.Width = 223.0;
		_prePaymentECRCheckNum.IsEnabled = Settings.ParametersService.ECRCheckPrintBonusType() == PrintBonusTypesSE.DiscountAndPrePayment.Value;
		_currCode = AddDropDownControl(container2, Definition.Fields["CurrCode"], new Currency(shortMode: true));
		_currCode.IsEnabled = false;
		_storage = AddDropDownControl(container2, Definition.Fields["Storage"], new ArmSoft.SME.Trade.UI.DropDown.Storages(_isShowStorageColInMTBill ? new bool?(false) : ((bool?)null)));
		_storage.EditValueChanged += new SafeDocumentEventHandler<EditValueChangedEventArgs>(Storage_EditValueChanged, this).Handler;
		if (!_isShowStorageColInMTBill && Settings.ParametersService.ShowStorageInMTBill())
		{
			_storage.IsRequired = true;
		}
		_cashDesk = AddDropDownControl(container2, Definition.Fields["CashDesk"], new ArmSoft.SME.Trade.UI.DropDown.Cashes(withoutCheckAccess: true));
		_cashDeskEditValueChangedEventHandler = new SafeDocumentEventHandler<EditValueChangedEventArgs>(CashDesk_EditValueChanged, this);
		_cashDesk.EditValueChanged += _cashDeskEditValueChangedEventHandler.Handler;
		_cashierShift = AddStringControl(container2, Definition.Fields["CashierShiftNumber"]);
		_cashierShift.Width = 223.0;
		_partner = AddDropDownControl(container2, ArmSoft.SME.Common.Resources.Buyer, isRequired: false, new ArmSoft.SME.Trade.UI.DropDown.Contracts("", "AMD", isExceptCurrCodeNCC: false, isFilterBySuppliers: false, isFilterByCustomers: true, isValueMemberPartner: true), allowAdditionButtons: true, HorizontalAlignment.Left, "", "Buyer");
		_partner.ClickDropDown += new SafeDocumentEventHandler<DropDownControlEventArgs>(Partner_ClickDropDown, this).Handler;
		_partner.EditValueChanged += new SafeDocumentEventHandler<EditValueChangedEventArgs>(Partner_EditValueChanged, this).Handler;
		_salesConsultant = AddDropDownControl(container2, Definition.Fields["SalesConsultant"], new ArmSoft.SME.Trade.UI.DropDown.Employees(), allowAdditionButtons: true);
		LayoutGroupExt layoutGroupExt = AddLayoutGroup(container, LayoutGroupView.Group, Orientation.Vertical);
		layoutGroupExt.HorizontalAlignment = HorizontalAlignment.Right;
		_summa = AddNumericControl(layoutGroupExt, Definition.Fields["Summ"], useCurrencyFormat: true, HorizontalAlignment.Right);
		_summa.EditValueChanged += new SafeDocumentEventHandler<EditValueChangedEventArgs>(Summa_EditValueChanged, this).Handler;
		((LayoutItemWithRequired)(object)_summa.Parent).FontSize = _fontSizeLarge;
		((LayoutItemWithRequired)(object)_summa.Parent).FontWeight = FontWeights.Bold;
		_summa.IsTabStop = false;
		_summa.IsRequired = !Settings.ParametersService.AllowUse100PercentSale();
		_summaCash = AddNumericControl(layoutGroupExt, Definition.Fields["ECRReturnByCashSum"], useCurrencyFormat: true, HorizontalAlignment.Right);
		((LayoutItemWithRequired)(object)_summaCash.Parent).Label = ArmSoft.SME.Trade.Common.Resources.CashPaymentForPrint + ((string.IsNullOrWhiteSpace(Settings.ParametersService.MTBillSummaCashHotKey()) || _isCalcDiscountAuto) ? "" : ("(" + Settings.ParametersService.MTBillSummaCashHotKey() + ")"));
		_summaCash.EditValueChanged += new SafeDocumentEventHandler<EditValueChangedEventArgs>(SummaCash_EditValueChanged, this).Handler;
		if (_isCalcDiscountAuto)
		{
			_summaCash.IsTabStop = false;
		}
		((LayoutItemWithRequired)(object)_summaCash.Parent).FontSize = _fontSizeLarge;
		((LayoutItemWithRequired)(object)_summaCash.Parent).FontWeight = FontWeights.Bold;
		_otherPaymentTypes_EditValueChangedHandler = new SafeDocumentEventHandler<EditValueChangedEventArgs>(OtherPaymentTypes_EditValueChanged, this);
		_summaPlCard = AddNumericControl(layoutGroupExt, Definition.Fields["ECRReturnByCardSum"], useCurrencyFormat: true, HorizontalAlignment.Right);
		_summaPlCard.EditValueChanged += _otherPaymentTypes_EditValueChangedHandler.Handler;
		_summaPlCard.IsTabStop = false;
		((LayoutItemWithRequired)(object)_summaPlCard.Parent).FontSize = _fontSizeLarge;
		((LayoutItemWithRequired)(object)_summaPlCard.Parent).FontWeight = FontWeights.Bold;
		((LayoutItemWithRequired)(object)_summaPlCard.Parent).Visibility = Visibility.Collapsed;
		_summaPayByBonus = AddNumericControl(layoutGroupExt, Definition.Fields["SummaPayByBonus"], useCurrencyFormat: true, HorizontalAlignment.Right);
		_summaPayByBonus.EditValueChanged += _otherPaymentTypes_EditValueChangedHandler.Handler;
		((LayoutItemWithRequired)(object)_summaPayByBonus.Parent).FontSize = _fontSizeLarge;
		((LayoutItemWithRequired)(object)_summaPayByBonus.Parent).FontWeight = FontWeights.Bold;
		_summaPayByBonus.IsTabStop = false;
		((LayoutItemWithRequired)(object)_summaPayByBonus.Parent).Visibility = Visibility.Collapsed;
		_summaCredit = AddNumericControl(layoutGroupExt, Definition.Fields["SummaCredit"], useCurrencyFormat: true, HorizontalAlignment.Right);
		_summaCredit.EditValueChanged += _otherPaymentTypes_EditValueChangedHandler.Handler;
		_summaCredit.IsTabStop = false;
		((LayoutItemWithRequired)(object)_summaCredit.Parent).FontSize = _fontSizeLarge;
		((LayoutItemWithRequired)(object)_summaCredit.Parent).FontWeight = FontWeights.Bold;
		((LayoutItemWithRequired)(object)_summaCredit.Parent).Visibility = Visibility.Collapsed;
		_summaPrePayment = AddNumericControl(layoutGroupExt, Definition.Fields["ECRReturnByPrePaymentSum"], useCurrencyFormat: true, HorizontalAlignment.Right);
		_summaPrePayment.EditValueChanged += new SafeDocumentEventHandler<EditValueChangedEventArgs>(SummaPrePayment_EditValueChanged, this).Handler;
		_summaPrePayment.IsTabStop = false;
		((LayoutItemWithRequired)(object)_summaPrePayment.Parent).FontSize = _fontSizeLarge;
		((LayoutItemWithRequired)(object)_summaPrePayment.Parent).FontWeight = FontWeights.Bold;
		((LayoutItemWithRequired)(object)_summaPrePayment.Parent).Visibility = Visibility.Collapsed;
		_summaChange = AddNumericControl(layoutGroupExt, Definition.Fields["SummaChange"], useCurrencyFormat: true, HorizontalAlignment.Right);
		_summaChange.EditValueChanged += new SafeDocumentEventHandler<EditValueChangedEventArgs>(SummaChange_EditValueChanged, this).Handler;
		((LayoutItemWithRequired)(object)_summaChange.Parent).FontSize = _fontSizeLarge;
		((LayoutItemWithRequired)(object)_summaChange.Parent).FontWeight = FontWeights.Bold;
		_summaChange.IsTabStop = false;
		_summaCash.Width = 250.0;
		_summa.Foreground = _summaCash.Foreground;
		_summa.Width = _summaCash.Width;
		_summaChange.Foreground = _summaCash.Foreground;
		_summaChange.Width = _summaCash.Width;
		_summaCredit.Foreground = _summaCash.Foreground;
		_summaCredit.Width = _summaCash.Width;
		_summaPlCard.Foreground = _summaCash.Foreground;
		_summaPlCard.Width = _summaCash.Width;
		_summaPayByBonus.Foreground = _summaCash.Foreground;
		_summaPayByBonus.Width = _summaCash.Width;
		_summaPrePayment.Foreground = _summaCash.Foreground;
		_summaPrePayment.Width = _summaCash.Width;
		BindingOperations.ClearBinding((DependencyObject)(object)(LayoutItemWithRequired)(object)_summa.Parent, LayoutItemWithRequired.IsReadOnlyProperty);
		BindingOperations.ClearBinding((DependencyObject)(object)(LayoutItemWithRequired)(object)_summaChange.Parent, LayoutItemWithRequired.IsReadOnlyProperty);
		BindingOperations.ClearBinding((DependencyObject)(object)(LayoutItemWithRequired)(object)_summaCredit.Parent, LayoutItemWithRequired.IsReadOnlyProperty);
		BindingOperations.ClearBinding((DependencyObject)(object)(LayoutItemWithRequired)(object)_summaPlCard.Parent, LayoutItemWithRequired.IsReadOnlyProperty);
		BindingOperations.ClearBinding((DependencyObject)(object)(LayoutItemWithRequired)(object)_summaPayByBonus.Parent, LayoutItemWithRequired.IsReadOnlyProperty);
		BindingOperations.ClearBinding((DependencyObject)(object)(LayoutItemWithRequired)(object)_summaPrePayment.Parent, LayoutItemWithRequired.IsReadOnlyProperty);
		BindingOperations.ClearBinding((DependencyObject)(object)(LayoutItemWithRequired)(object)_summaCash.Parent, LayoutItemWithRequired.IsReadOnlyProperty);
		int num = (_mtBillSimpleMode ? 135 : 136);
		LayoutGroupExt layoutGroupExt2 = AddLayoutGroup(null, LayoutGroupView.Group, Orientation.Horizontal);
		_inputValue = AddStringControl(layoutGroupExt2, ArmSoft.SME.Trade.Common.Resources.InputBoxCaption + (string.IsNullOrWhiteSpace(Settings.ParametersService.MTBillInputValueHotKey()) ? "" : (" (" + Settings.ParametersService.MTBillInputValueHotKey() + ")")), isRequired: false, 110, HorizontalAlignment.Stretch, "", "InputValue");
		_inputValue.KeyDown += new SafeDocumentEventHandler<KeyEventArgs>(InputValue_KeyDown, this).Handler;
		text = ArmSoft.SME.Trade.Common.Resources.cmdBarCodeEnter + (string.IsNullOrWhiteSpace(Settings.ParametersService.MTBillBarCodeEnterHotKey()) ? "" : ("(" + Settings.ParametersService.MTBillBarCodeEnterHotKey() + ")"));
		_barCodeEnter = AddButton(layoutGroupExt2, "", text, "", text, isTabStop: true);
		_barCodeEnter.Width = num;
		_barCodeEnter.Click += new SafeDocumentEventHandler<RoutedEventArgs>(BarCodeEnter_Click, this).Handler;
		text = ArmSoft.SME.Trade.Common.Resources.cmdCodeEnter + (string.IsNullOrWhiteSpace(Settings.ParametersService.MTBillCodeEnterHotKey()) ? "" : ("(" + Settings.ParametersService.MTBillCodeEnterHotKey() + ")"));
		_codeEnter = AddButton(layoutGroupExt2, "", text, "", text, isTabStop: true);
		_codeEnter.Width = num;
		_codeEnter.Click += new SafeDocumentEventHandler<RoutedEventArgs>(CodeEnter_Click, this).Handler;
		text = ArmSoft.SME.Trade.Common.Resources.cmdNameEnter + (string.IsNullOrWhiteSpace(Settings.ParametersService.MTBillMTListLikeNameHotKey()) ? "" : ("(" + Settings.ParametersService.MTBillMTListLikeNameHotKey() + ")"));
		_mtListLikeName = AddButton(layoutGroupExt2, "", text, "", text, isTabStop: true);
		_mtListLikeName.Width = num;
		_mtListLikeName.Click += new SafeDocumentEventHandler<RoutedEventArgs>(MTListLikeName_Click, this).Handler;
		layoutGroupExt = AddLayoutGroup(null, LayoutGroupView.Group, Orientation.Horizontal);
		layoutGroupExt.HorizontalAlignment = HorizontalAlignment.Right;
		text = ArmSoft.SME.Trade.Common.Resources.cmdQtyEnter + (string.IsNullOrWhiteSpace(Settings.ParametersService.MTBillQtyEnterHotKey()) ? "" : ("(" + Settings.ParametersService.MTBillQtyEnterHotKey() + ")"));
		_qtyEnter = AddButton(_mtBillSimpleMode ? layoutGroupExt : layoutGroupExt2, "", text, "", text, isTabStop: true);
		_qtyEnter.Width = num;
		_qtyEnter.Click += new SafeDocumentEventHandler<RoutedEventArgs>(BE_QtyEnter_Click, this).Handler;
		if (_allowChangeSalePrices)
		{
			text = ArmSoft.SME.Trade.Common.Resources.SalePriceShort + (string.IsNullOrWhiteSpace(Settings.ParametersService.MTBillSalePriceHotKey()) ? "" : ("(" + Settings.ParametersService.MTBillSalePriceHotKey() + ")"));
			_salePrice = AddButton(_mtBillSimpleMode ? layoutGroupExt : layoutGroupExt2, "", text, "", text, isTabStop: true);
			_salePrice.IsTabStop = true;
			_salePrice.Width = num;
			_salePrice.Click += new SafeDocumentEventHandler<RoutedEventArgs>(SalePrice_Click, this).Handler;
		}
		if (_isCalcDiscountAuto)
		{
			text = ArmSoft.SME.Trade.Common.Resources.ErasePayments + (string.IsNullOrWhiteSpace(Settings.ParametersService.MTBillErasePaymentsHotKey()) ? "" : ("(" + Settings.ParametersService.MTBillErasePaymentsHotKey() + ")"));
			_erasePayments = AddButton(layoutGroupExt, "", text, "", text, isTabStop: true);
			_erasePayments.Width = num;
			_erasePayments.Click += new SafeDocumentEventHandler<RoutedEventArgs>(ErasePayments_Click, this).Handler;
			text = ArmSoft.SME.Trade.Common.Resources.CashPaymentForPrint + (string.IsNullOrWhiteSpace(Settings.ParametersService.MTBillSummaCashHotKey()) ? "" : ("(" + Settings.ParametersService.MTBillSummaCashHotKey() + ")"));
			_summaPayCash = AddButton(layoutGroupExt, "", text, "", text, isTabStop: true);
			_summaPayCash.Width = num;
			_summaPayCash.Click += new SafeDocumentEventHandler<RoutedEventArgs>(SummaCash_Click, this).Handler;
		}
		if (!_mtBillSimpleMode)
		{
			if (Settings.ParametersService.MTBillUsePrePaymentShowButton())
			{
				text = ArmSoft.SME.Trade.Common.Resources.UsePrePaymentShort + (string.IsNullOrWhiteSpace(Settings.ParametersService.MTBillUsePrePaymentHotKey()) ? "" : ("(" + Settings.ParametersService.MTBillUsePrePaymentHotKey() + ")"));
				_usePrePayment = AddButton(layoutGroupExt, "", text, "", text, isTabStop: true);
				_usePrePayment.Width = num;
				_usePrePayment.Click += new SafeDocumentEventHandler<RoutedEventArgs>(UsePrePayment_Click, this).Handler;
			}
			if (Settings.ParametersService.MTBillPlCardPaymentShowButton())
			{
				text = ArmSoft.SME.Trade.Common.Resources.PlasticCartPayment + (string.IsNullOrWhiteSpace(Settings.ParametersService.MTBillPlCardPaymentHotKey()) ? "" : ("(" + Settings.ParametersService.MTBillPlCardPaymentHotKey() + ")"));
				_plCardPayment = AddButton(layoutGroupExt, "", text, "", text, isTabStop: true);
				_plCardPayment.Width = num;
				_plCardPayment.Click += new SafeDocumentEventHandler<RoutedEventArgs>(PlCardPayment_Click, this).Handler;
			}
			if (Settings.ParametersService.MTBillCreditShowButton())
			{
				text = ArmSoft.SME.Trade.Common.Resources.CreditSaleShort + (string.IsNullOrWhiteSpace(Settings.ParametersService.MTBillCreditHotKey()) ? "" : ("(" + Settings.ParametersService.MTBillCreditHotKey() + ")"));
				_credit = AddButton(layoutGroupExt, "", text, "", text, isTabStop: true);
				_credit.Width = num;
				_credit.Click += new SafeDocumentEventHandler<RoutedEventArgs>(Credit_Click, this).Handler;
			}
		}
		if (Settings.ParametersService.MTBillPrintECRCheckShowButton())
		{
			text = ArmSoft.SME.Common.Resources.ECRC + (string.IsNullOrWhiteSpace(Settings.ParametersService.MTBillPrintECRCheckHotKey()) ? "" : ("(" + Settings.ParametersService.MTBillPrintECRCheckHotKey() + ")"));
			_printECRCheck = AddButton(layoutGroupExt, "", text, "", text, isTabStop: true);
			_printECRCheck.Width = num;
			_printECRCheck.Click += new SafeDocumentEventHandler<RoutedEventArgs>(BE_PrintECRCheck_Click, this).Handler;
		}
		if (Settings.ParametersService.MTBillPrintBillShowButton())
		{
			text = ArmSoft.SME.Trade.Common.Resources.PrintCheck + (string.IsNullOrWhiteSpace(Settings.ParametersService.MTBillPrintBillHotKey()) ? "" : ("(" + Settings.ParametersService.MTBillPrintBillHotKey() + ")"));
			_printBill = AddButton(layoutGroupExt, "", text, "", text, isTabStop: true);
			_printBill.Width = num;
			_printBill.Click += new SafeDocumentEventHandler<RoutedEventArgs>(BE_PrintBill_Click, this).Handler;
		}
		_mtBillReturnGrid = AddGrid(null, Definition.Grids["MTList"], HorizontalAlignment.Stretch, VerticalAlignment.Stretch, isSupportDialogMode: true);
		_mtBillReturnGrid.AddingRow += new SafeDocumentEventHandler<DocumentGridAddingRowEventArgs>(MTBillReturnGrid_AddingRow, this).Handler;
		_mtBillReturnGrid.DuplicatingRow += new SafeDocumentEventHandler<DocumentGridDuplicatingRowEventArgs>(MTBillReturnGrid_DuplicatingRow, this).Handler;
		_mtBillReturnGrid.MovingRowUp += new SafeDocumentEventHandler<DocumentGridMovingRowUpOrDownEventArgs>(MTBillReturnGrid_MovingRowUp, this).Handler;
		_mtBillReturnGrid.MovingRowDown += new SafeDocumentEventHandler<DocumentGridMovingRowUpOrDownEventArgs>(MTBillReturnGrid_MovingRowDown, this).Handler;
		_mtBillReturnGrid.DeletingRow += new SafeDocumentEventHandler<DocumentGridDeletingRowEventArgs>(MTBillReturnGrid_DeletingRow, this).Handler;
		_mtBillReturnGrid.DeletedRow += new SafeDocumentEventHandler<EventArgs>(MTBillReturnGrid_DeletedRow, this).Handler;
		_mtBillReturnGrid.AddingRowWithDialog += new SafeDocumentEventHandler<DocumentGridAddingRowEventArgs>(MTBillReturnGrid_AddingRowWithDialog, this).Handler;
		_mtBillReturnGrid.RefreshButtons += new SafeDocumentEventHandler<EventArgs>(MTBillReturnGrid_RefreshButtons, this).Handler;
		_mtBillReturnGrid.ClickDropDown += new SafeDocumentEventHandler<DocumentGridClickDropDownEventArgs>(MTBillReturnGrid_ClickDropDown, this).Handler;
		_mtBillReturnGrid.HasRowDeletedColumn = true;
		text = ArmSoft.SME.Trade.Common.Resources.Increase;
		_increase = _mtBillReturnGrid.AddButton("", text, text, num);
		_increase.Click += new SafeDocumentEventHandler<RoutedEventArgs>(Increase_Click, this).Handler;
		text = ArmSoft.SME.Trade.Common.Resources.Decrease;
		_decrease = _mtBillReturnGrid.AddButton("", text, text, num);
		_decrease.Click += new SafeDocumentEventHandler<RoutedEventArgs>(Decrease_Click, this).Handler;
		if (!string.IsNullOrWhiteSpace(ElScaleInfo.GetInstance().ElScaleType))
		{
			text = ArmSoft.SME.Trade.Common.Resources.Weight + (string.IsNullOrWhiteSpace(Settings.ParametersService.MTBillWeightHotKey()) ? "" : ("(" + Settings.ParametersService.MTBillWeightHotKey() + ")"));
			_weight = _mtBillReturnGrid.AddButton("", text, text, num);
			_weight.Click += new SafeDocumentEventHandler<RoutedEventArgs>(Weight_Click, this).Handler;
		}
		if (!_mtBillSimpleMode && _useDiscount)
		{
			text = ArmSoft.SME.Trade.Common.Resources.GiveDiscount + (string.IsNullOrWhiteSpace(Settings.ParametersService.MTBillDiscountHotKey()) ? "" : ("(" + Settings.ParametersService.MTBillDiscountHotKey() + ")"));
			_discount = _mtBillReturnGrid.AddButton("", text, text, num);
		}
		if (Settings.ParametersService.MTBillPricesShowButton())
		{
			text = ArmSoft.SME.Common.Resources.Prices + (string.IsNullOrWhiteSpace(Settings.ParametersService.MTBillPricesHotKey()) ? "" : ("(" + Settings.ParametersService.MTBillPricesHotKey() + ")"));
			_prices = _mtBillReturnGrid.AddButton("", text, text, num);
			_prices.Click += new SafeDocumentEventHandler<RoutedEventArgs>(Prices_Click, this).Handler;
		}
		if (Settings.ParametersService.MTBillFavoritesShowButton())
		{
			text = ArmSoft.SME.Common.Resources.RibbonPageFavorites + (string.IsNullOrWhiteSpace(Settings.ParametersService.MTBillFavoritesHotKey()) ? "" : ("(" + Settings.ParametersService.MTBillFavoritesHotKey() + ")"));
			_mtFavorites = _mtBillReturnGrid.AddButton("", text, text, num);
			_mtFavorites.Click += new SafeDocumentEventHandler<RoutedEventArgs>(MTFavorites_Click, this).Handler;
		}
		text = ArmSoft.SME.Common.Resources.Marking;
		_selectMarking = _mtBillReturnGrid.AddButton("datamatrix_blue_16.svg", text, text, num);
		_selectMarking.Click += new SafeDocumentEventHandler<RoutedEventArgs>(SelectMarking_Click, this).Handler;
		_mtBillReturnGrid.AllowMoveUp = true;
		_mtBillReturnGrid.AllowMoveDown = true;
		_mtBillReturnGrid.IsRequired = true;
		_mtBillReturnGrid.ToolTip = ArmSoft.SME.Common.Resources.ItemList;
		_mtBillReturnGrid.View.AutoWidth = Settings.ParametersService.MTBillGridAutoWidth();
		_mtBillReturnTableView = _mtBillReturnGrid.View;
		_focusedRowHandleChangedEvent = new SafeDocumentEventHandler<FocusedRowHandleChangedEventArgs>(MTBillReturnTableView_FocusedRowHandleChanged, this);
		_mtBillReturnTableView.FocusedRowHandleChanged += _focusedRowHandleChangedEvent.Handler;
		_mtBillReturnTableView.CellValueChanged += new SafeDocumentEventHandler<CellValueChangedEventArgs>(MTBillReturnTableView_CellValueChanged, this).Handler;
		_mtBillReturnTableView.ShowingEditor += new SafeDocumentEventHandler<ShowingEditorEventArgs>(MTBillReturnTableView_ShowingEditor, this).Handler;
		_mtBillReturnTableView.MouseLeftButtonDown += new SafeDocumentEventHandler<MouseButtonEventArgs>(MTBillReturnTableView_MouseLeftButtonDown, this).Handler;
		_mtBillReturnGrid.AddRowIndicator(35);
		if (!_isAllowChangeGoodRowInBill)
		{
			_canceledCol = _mtBillReturnGrid.AddBooleanColumn(Definition.Grids["MTList"].Columns["IsDeleted"]);
			_canceledCol.ReadOnly = true;
			_canceledCol.TabStop = false;
			_canceledCol.AllowFocus = false;
			_canceledCol.Width = 35.0;
		}
		_storageCol = _mtBillReturnGrid.AddDropDownColumn(Definition.Grids["MTList"].Columns["Storage"], trimValue: false);
		_barCodeCol = _mtBillReturnGrid.AddDropDownCommentColumn(Definition.Grids["MTList"].Columns["BarCode"], new ArmSoft.SME.Trade.UI.DropDown.MTBarCodes(isShowQntUnitCaption: false, showBarCodeFirst: true, isShowOnlyInclInPriceList: false, showAlsoMTQntUnitsEmptyBarCodes: true), 20);
		_barCodeCol.IsOwnProcessingClickDropDown = true;
		_barCodeCol.Width = 120.0;
		_itemCodeCol = _mtBillReturnGrid.AddDropDownColumn(Definition.Grids["MTList"].Columns["ItemCode"], new ArmSoft.SME.Trade.UI.DropDown.GoodsFilter(), allowAdditionButtons: true, isOwnProcessingClickDropDown: true);
		_itemCodeCol.Width = 125.0;
		_itemNameCol = _mtBillReturnGrid.AddStringColumn(Definition.Grids["MTList"].Columns["ItemName"]);
		_itemNameCol.Width = 225.0;
		_unitCodeCol = _mtBillReturnGrid.AddDropDownColumn(Definition.Grids["MTList"].Columns["UnitCode"], trimValue: false);
		_unitCol = _mtBillReturnGrid.AddStringColumn(Definition.Grids["MTList"].Columns["UnitBrief"]);
		_unitCol.AllowEditing = DefaultBoolean.False;
		_unitCol.AllowFocus = false;
		_quantityCol = _mtBillReturnGrid.AddNumberColumn(Definition.Grids["MTList"].Columns["Quantity"], _isAllowChangeGoodRowInBill);
		if (IsOfflineDoc)
		{
			_retail = false;
		}
		else if (string.IsNullOrWhiteSpace(PO.Storage))
		{
			_retail = !string.IsNullOrWhiteSpace(Settings.ParametersService.DefaultStorage()) && TradeSettings.TradeCacheService.Storage.GetItem(Settings.ParametersService.DefaultStorage()).Retail;
		}
		else
		{
			_retail = TradeSettings.TradeCacheService.Storage.GetItem(PO.Storage).Retail;
		}
		_priceCol = _mtBillReturnGrid.AddNumberColumn(Definition.Grids["MTList"].Columns["BeginPrice"], showTotal: false, useCurrencyFormat: true, isOwnProcessingClickDropDown: true);
		_discountCol = _mtBillReturnGrid.AddNumberColumn(Definition.Grids["MTList"].Columns["Discount"]);
		_discountPriceCol = _mtBillReturnGrid.AddNumberColumn(Definition.Grids["MTList"].Columns["DiscountPrice"]);
		_summaCol = _mtBillReturnGrid.AddNumberColumn(Definition.Grids["MTList"].Columns["Summa"], _isAllowChangeGoodRowInBill);
		if (_useDiscount)
		{
			_summaCol.Header = ArmSoft.SME.Trade.Common.Resources.DiscountedSumm;
		}
		_discountSummCol = _mtBillReturnGrid.AddNumberColumn(Definition.Grids["MTList"].Columns["DiscountSumma"], _isAllowChangeGoodRowInBill);
		_discountSummCol.AllowEditing = DefaultBoolean.False;
		_discountSummCol.AllowFocus = false;
		if (!_useDiscount)
		{
			_discountSummCol.Visible = false;
			_discountSummCol.ShowInColumnChooser = false;
		}
		_usedBonusSummCol = _mtBillReturnGrid.AddNumberColumn(Definition.Grids["MTList"].Columns["UsedBonus"], _isAllowChangeGoodRowInBill);
		_usedBonusSummCol.AllowEditing = DefaultBoolean.False;
		_usedBonusSummCol.AllowFocus = false;
		_rowMarkingListCol = _mtBillReturnGrid.AddDropDownColumn("MarkingStatus", ArmSoft.SME.Common.Resources.Marking, 19, trimValue: false, GlyphKind.Regular);
		((ButtonEditSettings)_rowMarkingListCol.EditSettings).IsTextEditable = false;
		_isRowMarkable = _mtBillReturnGrid.AddBooleanColumn(Definition.Grids["MTList"].Columns["IsRowMarkable"]);
		_isRowMarkable.ReadOnly = true;
		_isRowMarkable.TabStop = false;
		_isRowMarkable.AllowFocus = false;
		_isRowMarkable.Visible = false;
		_vatCol = _mtBillReturnGrid.AddBooleanColumn(Definition.Grids["MTList"].Columns["VAT"]);
		_vatCol.Width = 45.0;
		_accumulatedBonusCol = _mtBillReturnGrid.AddNumberColumn(Definition.Grids["MTList"].Columns["AccumulatedBonus"], _isAllowChangeGoodRowInBill);
		_accumulatedBonusCol.AllowEditing = DefaultBoolean.False;
		_accumulatedBonusCol.AllowFocus = false;
		_rowDescCol = _mtBillReturnGrid.AddStringColumn(Definition.Grids["MTList"].Columns["RowDescription"]);
		_rowDescCol.AllowEditing = DefaultBoolean.True;
		foreach (FormatConditionBase item in ConditionalFormattingHelper.ConvertConditions(CreateFormatingList()))
		{
			_mtBillReturnGrid.View.AddFormatCondition(item);
		}
		if (Settings.ParametersService.ShowASInfoInMTBill())
		{
			ArmSoft.SME.Trade.UI.Helpers.DocumentHelpers.ShowASInfoInGrid(_mtBillReturnGrid);
		}
		foreach (GridColumn column in _mtBillReturnGrid.Grid.Columns)
		{
			column.AllowSorting = DefaultBoolean.True;
			column.AllowColumnFiltering = DefaultBoolean.True;
			column.FilterPopupMode = FilterPopupMode.CheckedList;
		}
		_mtBillReturnList = new BindingList<MTRetFromCustSpecificationRow>();
		if (_isShowInfoPanel)
		{
			_infoDisplayControls = AddInfoDisplayControls(DocumentTypeSE.MTBillReturn, _isShowItemPictureOnInfoPanel, _isShowStorageAvailabilityOnInfoPanel, _isShowWholeAvailabilityOnInfoPanel, isShowPriceAvailabilityOnInfoPanel: false, Settings.ParametersService.ShowDescriptionOnInfoPanel(), Settings.ParametersService.EnableSubstituteItems(), _mtBillSimpleMode ? 24 : 22);
			_infoItemName = _infoDisplayControls.InfoItemName;
			_infoFormula = _infoDisplayControls.InfoFormula;
			_infoDescription = _infoDisplayControls.InfoDescription;
			_substitutionInfo = _infoDisplayControls.SubstitutionInfo;
			_hintImage = _infoDisplayControls.HintImage;
			_itemImage = _infoDisplayControls.ItemImage;
		}
		LayoutGroupExt container4 = AddLayoutGroup(null, LayoutGroupView.Group, Orientation.Horizontal);
		_comment = AddComboBoxControl(container4, Definition.Fields["Comment"], TradeSettings.TradeCacheService.StandardComments.GetCaptionsList(), (!Settings.LoginService.IsAdmin && Settings.ParametersService.DenyAccessToOtherOwnerDocs()) ? HorizontalAlignment.Stretch : HorizontalAlignment.Left);
		_owner = AddDropDownControl(container4, Definition.Fields["Owner"], new UsersFilter(), allowAdditionButtons: false, fieldWithCaption: true, HorizontalAlignment.Right);
		_owner.CodeWidth = 12;
		_owner.ShowComment = false;
		_owner.InnerCodeEdit.Width = _owner.InnerCodeEdit.MinWidth;
		if (!Settings.ParametersService.ShowOwnerInMTBill())
		{
			((LayoutItemWithRequired)(object)_owner.Parent).Visibility = Visibility.Collapsed;
		}
		else if (!Settings.LoginService.IsAdmin && Settings.ParametersService.DenyAccessToOtherOwnerDocs())
		{
			((LayoutItemWithRequired)(object)_owner.Parent).Visibility = Visibility.Collapsed;
		}
		else
		{
			_owner.IsEnabled = Settings.ParametersService.AllowChangeOwnerInMTBill();
		}
		if (!_mtBillSimpleMode)
		{
			if (!Settings.ParametersService.ShowStorageInMTBill())
			{
				((LayoutItemWithRequired)(object)_storage.Parent).Visibility = Visibility.Collapsed;
			}
			else
			{
				_storage.IsEnabled = _isAllowChangeStorageInBill;
			}
			if (!Settings.ParametersService.ShowCashInMTBill())
			{
				((LayoutItemWithRequired)(object)_cashDesk.Parent).Visibility = Visibility.Collapsed;
			}
			else
			{
				_cashDesk.IsEnabled = _isAllowChangeCashInBill;
			}
			if (!Settings.ParametersService.ShowCashierShiftNumInMTBill())
			{
				((LayoutItemWithRequired)(object)_cashierShift.Parent).Visibility = Visibility.Collapsed;
			}
			_partner.IsEnabled = _isAllowChangeBuyerInBill;
			if (!Settings.ParametersService.ShowSalesConsultantInMTBill())
			{
				((LayoutItemWithRequired)(object)_salesConsultant.Parent).Visibility = Visibility.Collapsed;
			}
			else
			{
				_salesConsultant.IsEnabled = _isAllowChangeSaleConsultantInBill;
			}
			if (!Settings.ParametersService.ShowCommentInMTBill())
			{
				((LayoutItemWithRequired)(object)_comment.Parent).Visibility = Visibility.Collapsed;
			}
			else
			{
				_comment.IsEnabled = Settings.ParametersService.AllowChangeCommentInMTBill();
			}
			_docNum.IsEnabled = _isAllowChangeDocNumberInBill;
		}
		else
		{
			_cashDesk.IsEnabled = false;
			_docNum.IsEnabled = false;
			_date.IsEnabled = false;
		}
		if (!_isShowStorageColInMTBill)
		{
			_storageCol.Visible = false;
			_storageCol.ShowInColumnChooser = false;
		}
		if (!_iIsShowUnitCodeColInBill)
		{
			_unitCodeCol.Visible = false;
			_unitCodeCol.ShowInColumnChooser = false;
		}
		if (!_isShowDiscountPercentColInBill)
		{
			_discountCol.Visible = false;
			_discountCol.ShowInColumnChooser = false;
		}
		if (!_isShowDiscountedPriceColInBill)
		{
			_discountPriceCol.Visible = false;
			_discountPriceCol.ShowInColumnChooser = false;
		}
		if (!_isShowDiscountSummColInBill)
		{
			_discountSummCol.Visible = false;
			_discountSummCol.ShowInColumnChooser = false;
		}
		if (!_isShowVATColInBill)
		{
			_vatCol.Visible = false;
			_vatCol.ShowInColumnChooser = false;
		}
		if (!_isShowDescrColInBill)
		{
			_rowDescCol.Visible = false;
			_rowDescCol.ShowInColumnChooser = false;
		}
		if (!_useMarking)
		{
			_rowMarkingListCol.Visible = false;
			_rowMarkingListCol.ShowInColumnChooser = false;
			_selectMarking.IsVisible = false;
		}
		ControlsHelper.SetFocus(_inputValue);
		PreviewKeyDown += new SafeDocumentEventHandler<KeyEventArgs>(DocumentWindow_PreviewKeyDown, this).Handler;
		_rowInfoChange += OnRowInfoChange;
	}

	private List<AsFormatting> CreateFormatingList()
	{
		List<AsFormatting> list = new List<AsFormatting>();
		if (_useMarking)
		{
			UserDefinedFormat customFormat = new UserDefinedFormat
			{
				IconUri = "datamatrix_blue_16.svg"
			};
			AsCondititionFormat item = new AsCondititionFormat
			{
				Expression = "[Quantity] <> 0.0m And [IsRowMarkable] = True",
				FieldName = "Quantity",
				FormatName = "Custom",
				ApplyToRow = false,
				CustomFormat = customFormat,
				IsFixed = true
			};
			list.Add(item);
			customFormat = new UserDefinedFormat
			{
				IconUri = "datamatrix_red_16.svg"
			};
			item = new AsCondititionFormat
			{
				Expression = "[MarkingHasMismatch] = True",
				FieldName = "Quantity",
				FormatName = "Custom",
				ApplyToRow = false,
				CustomFormat = customFormat,
				IsFixed = true
			};
			list.Add(item);
			customFormat = (Settings.IsDarkTheme ? new UserDefinedFormat
			{
				ForegroundColor = "#FF6666",
				BackgroundColor = "#663333"
			} : new UserDefinedFormat
			{
				ForegroundColor = "#E50000",
				BackgroundColor = "#FFCCCC"
			});
			item = new AsCondititionFormat
			{
				Expression = "[MarkingHasMismatch] = True",
				FieldName = "MarkingStatus",
				FormatName = "Custom",
				ApplyToRow = false,
				CustomFormat = customFormat,
				IsFixed = true
			};
			list.Add(item);
		}
		return list;
	}

	protected override void OnNew()
	{
		base.OnNew();
		_connectTob2barmsoftDiscountServer = Settings.ParametersService.ConnectTob2barmsoftDiscountServer();
		_owner.Code = Settings.LoginService.UserName;
		_date.EditValue = ((!_allowChangeDate) ? DateTime.Today.ToDateOnly() : Settings.HostingEnvironment.CurrentDate);
		if (string.IsNullOrWhiteSpace(_cashDesk.Code))
		{
			_cashDesk.Code = Settings.ParametersService.DefaultCash();
			if (!string.IsNullOrWhiteSpace(_cashDesk.Code) && TradeSettings.TradeCacheService.Cash.GetItem(_cashDesk.Code) != null && !TradeSettings.CreateTradeClient().CommonServices.IsCashDeskAvailable(_cashDesk.Code))
			{
				ArmSoft.SME.Core.UI.MessageBox.Show(string.Format(ArmSoft.SME.Trade.Common.Resources.CashDeskNotAvailForUser, Settings.LoginService.UserName, _cashDesk.Code), ArmSoft.SME.Trade.Common.Resources.ProgramName, MessageBoxButton.OK, MessageBoxImage.Exclamation);
			}
		}
		SetCashierShiftNumber();
		if (!TradeSettings.CreateTradeClient().CommonServices.IsStorageAvailable(Settings.ParametersService.DefaultStorage()))
		{
			ArmSoft.SME.Core.UI.MessageBox.Show(string.Format(ArmSoft.SME.Trade.Common.Resources.StorageNotAvailForUser, Settings.LoginService.UserName, Settings.ParametersService.DefaultStorage()), Settings.ProgramName, MessageBoxButton.OK, MessageBoxImage.Asterisk, MessageBoxResult.OK);
			_storage.Code = "";
		}
		else if (string.IsNullOrWhiteSpace(_storage.Code))
		{
			_storage.Code = Settings.ParametersService.DefaultStorage();
		}
		if (string.IsNullOrWhiteSpace(PO.Storage))
		{
			_retail = TradeSettings.TradeCacheService.Storage.GetItem(Settings.ParametersService.DefaultStorage())?.Retail ?? false;
		}
		else
		{
			_retail = TradeSettings.TradeCacheService.Storage.GetItem(PO.Storage)?.Retail ?? false;
		}
		if (_viewMode != WindowViewMode.Copy)
		{
			_sumFromRound = 0m;
			_currCode.Code = (_retail ? "AMD" : Settings.ParametersService.DefaultMTBillCurr());
			_salesConsultant.Code = Settings.ParametersService.DefaultSalesConsultant();
			if (!string.IsNullOrWhiteSpace(_cashDesk.Code))
			{
				CashCacheRow.NonCashInfo nonCashInfo = TradeSettings.TradeCacheService.Cash.GetNonCashInfoListByCashCode(_cashDesk.Code).FirstOrDefault((CashCacheRow.NonCashInfo x) => x.Type == 5);
				if (nonCashInfo != null)
				{
					PO.POSTerminalType = nonCashInfo.Device;
					PO.BankAcquiring.PartnerCode = nonCashInfo.Partner;
					PO.BankAcquiring.ContractCode = nonCashInfo.Contract;
					PO.POSTerminalAddress = nonCashInfo.POSTerminalAddress;
				}
			}
			else
			{
				PO.POSTerminalType = POSTerminalTypesSE.None.Value;
			}
		}
		else
		{
			decimal num = 0m;
			if (!_isShowStorageColInMTBill)
			{
				_isShowStorageColInMTBill = false;
				foreach (MTRetFromCustSpecificationRow mtBillReturn in _mtBillReturnList)
				{
					mtBillReturn.Storage = _storage.Code;
					if (!mtBillReturn.IsDeleted)
					{
						num += mtBillReturn.Summa;
					}
				}
			}
			else
			{
				foreach (MTRetFromCustSpecificationRow mtBillReturn2 in _mtBillReturnList)
				{
					if (!mtBillReturn2.IsDeleted)
					{
						num += mtBillReturn2.Summa;
					}
				}
			}
			_summaPayByBonus.EditValue = 0m;
			if (!string.IsNullOrWhiteSpace(Settings.ParametersService.MTBillRoundDegree()))
			{
				_summa.Value = num;
				_sumFromRound = Methods.RoundPrice(num, Settings.ParametersService.MTBillRoundDegree(), Settings.ParametersService.MTBillSummRoundUp()) - _summa.Value;
				_summa.EditValue = _summa.Value + _sumFromRound;
			}
			UpdateChangeOrPaySum();
		}
		if (string.IsNullOrWhiteSpace(_currCode.Code) || _currCode.Code == "AMD")
		{
			_currCode.Code = "AMD";
			PO.ExchangeRate.Rate = 1m;
			PO.ExchangeRate.Base = 1m;
		}
		else
		{
			ExchangeRate exchangeRates = TradeSettings.CreateTradeClient().CommonServices.GetExchangeRates(_currCode.Code, _date.EditValue.Value);
			PO.ExchangeRate.Rate = exchangeRates.Rate;
			PO.ExchangeRate.Base = exchangeRates.Base;
		}
		if (BaseDocument != null && BaseDocument.POType == (short)DocumentTypeSE.MTBill)
		{
			MTBill mTBill = (MTBill)BaseDocument;
			_salesConsultant.Code = mTBill.SalesConsultant;
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
							_summaCash.Value = (mTBill.SummaCash - mTBill.SummaChange - mTBill.SumAddedBonus).Value;
							goto IL_0781;
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
							_summaPlCard.Value = mTBill.SummaPlCard.Value;
							_summaCash.Value = 0m;
							((LayoutItemWithRequired)(object)_summaCash.Parent).Visibility = Visibility.Visible;
						}
					}
				}
			}
			goto IL_0781;
		}
		goto IL_07e7;
		IL_07e7:
		if (_mtBillSimpleMode)
		{
			HideFieldsForMTBillReturnSimpleMode();
		}
		InitPoleDisplay(_viewMode == WindowViewMode.Copy);
		return;
		IL_0781:
		_sumFromRound = Methods.RoundPrice(_summa.Value, Settings.ParametersService.MTBillRoundDegree(), Settings.ParametersService.MTBillSummRoundUp()) - _summa.Value;
		_summa.EditValue = _summa.Value + _sumFromRound;
		UpdateChangeOrPaySum();
		goto IL_07e7;
	}

	private void HideFieldsForMTBillReturnSimpleMode()
	{
		((LayoutItemWithRequired)(object)_currCode.Parent).Visibility = Visibility.Collapsed;
		((LayoutItemWithRequired)(object)_storage.Parent).Visibility = Visibility.Collapsed;
		((LayoutItemWithRequired)(object)_partner.Parent).Visibility = Visibility.Collapsed;
		((LayoutItemWithRequired)(object)_salesConsultant.Parent).Visibility = Visibility.Collapsed;
		((LayoutItemWithRequired)(object)_comment.Parent).Visibility = Visibility.Collapsed;
		((LayoutItemWithRequired)(object)_owner.Parent).Visibility = Visibility.Collapsed;
		if (_summaCredit.Value == 0m)
		{
			_summaCredit.Visibility = Visibility.Collapsed;
		}
		if (_summaPlCard.Value == 0m)
		{
			_summaPlCard.Visibility = Visibility.Collapsed;
		}
		if (_summaPrePayment.Value == 0m)
		{
			_summaPrePayment.Visibility = Visibility.Collapsed;
		}
		if (_summaPayByBonus.Value == 0m)
		{
			_summaPayByBonus.Visibility = Visibility.Collapsed;
		}
		_accumulatedBonusCol.Visible = false;
		_accumulatedBonusCol.ShowInColumnChooser = false;
		_usedBonusSummCol.Visible = false;
		_usedBonusSummCol.ShowInColumnChooser = false;
	}

	protected override void OnNewAfterSave()
	{
		_connectTob2barmsoftDiscountServer = Settings.ParametersService.ConnectTob2barmsoftDiscountServer();
		_ecrCheckNum.Text = "";
		_prePaymentECRCheckNum.Text = "";
		base.OnNewAfterSave();
		BaseDocument = null;
		_mtBillReturnList.Clear();
		_maxRowID = 0;
		_sumFromRound = 0m;
		_partner.Code = "";
		_nonCashPaymentMethod = NonCashPaymentMethodsSE.None.Value;
		if (!string.IsNullOrWhiteSpace(_cashDesk.Code))
		{
			CashCacheRow.NonCashInfo nonCashInfo = TradeSettings.TradeCacheService.Cash.GetNonCashInfoListByCashCode(_cashDesk.Code).FirstOrDefault((CashCacheRow.NonCashInfo x) => x.Type == 5);
			if (nonCashInfo != null)
			{
				PO.POSTerminalType = nonCashInfo.Device;
				PO.BankAcquiring.PartnerCode = nonCashInfo.Partner;
				PO.BankAcquiring.ContractCode = nonCashInfo.Contract;
				PO.POSTerminalAddress = nonCashInfo.POSTerminalAddress;
			}
		}
		else
		{
			PO.POSTerminalType = POSTerminalTypesSE.None.Value;
		}
		PO.PartnerContractGroup.ContractCode = _contract;
		_priceListType = _priceListTypeParam;
		_summa.EditValue = 0m;
		_summaCash.EditValue = 0m;
		_summaChange.EditValue = 0m;
		_summaCredit.EditValue = 0m;
		_summaPlCard.EditValue = 0m;
		_summaPayByBonus.EditValue = 0m;
		_summaPrePayment.EditValue = 0m;
		_owner.Code = Settings.LoginService.UserName;
		_comment.Text = "";
		((LayoutItemWithRequired)(object)_summaCredit.Parent).Visibility = Visibility.Collapsed;
		((LayoutItemWithRequired)(object)_summaPlCard.Parent).Visibility = Visibility.Collapsed;
		((LayoutItemWithRequired)(object)_summaPrePayment.Parent).Visibility = Visibility.Collapsed;
		((LayoutItemWithRequired)(object)_summaPayByBonus.Parent).Visibility = Visibility.Collapsed;
		((LayoutItemWithRequired)(object)_summaChange.Parent).Label = ArmSoft.SME.Trade.Common.Resources.ForReturn;
		((LayoutItemWithRequired)(object)_summaChange.Parent).Foreground = (SolidColorBrush)new BrushConverter().ConvertFrom("#dd0326");
		_summaChange.Foreground = (SolidColorBrush)new BrushConverter().ConvertFrom("#dd0326");
		if (_mtBillSimpleMode)
		{
			HideFieldsForMTBillReturnSimpleMode();
		}
		if (_isShowInfoPanel)
		{
			((DispatcherObject)this).Dispatcher.BeginInvoke((Delegate)new DelegateClearInfoPanel(CleanInfoPanel), Array.Empty<object>());
		}
		InitPoleDisplay();
		_setFocusOnActivation = _inputValue;
	}

	protected override void OnEdit()
	{
		base.OnEdit();
		_connectTob2barmsoftDiscountServer = Settings.ParametersService.ConnectTob2barmsoftDiscountServer();
		if (BaseISN != Guid.Empty)
		{
			_mtBillReturnGrid.Grid.Columns["ItemCode"].ReadOnly = true;
			_mtBillReturnGrid.Grid.Columns["UnitCode"].ReadOnly = true;
			_mtBillReturnGrid.AllowAdd = false;
			_mtBillReturnGrid.AllowDuplicate = false;
		}
		if (_mtBillSimpleMode)
		{
			HideFieldsForMTBillReturnSimpleMode();
		}
		InitPoleDisplay(onEdit: true);
	}

	protected override void LoadDataToControls()
	{
		base.LoadDataToControls();
		_vatType = PO.VATType;
		_nonCashPaymentMethod = PO.NonCashPaymentMethod;
		_mBaseISN = PO.BaseISN;
		if (_mBaseISN == Guid.Empty)
		{
			BaseDocument = null;
		}
		else if (IsOfflineDoc)
		{
			BaseDocument = (TradeDocument)Settings.LocalDBDocumentService.Load(_mBaseISN);
		}
		else
		{
			BaseDocument = (TradeDocument)Settings.DocumentService.Load(_mBaseISN, withoutAccessCheck: true);
		}
		if (BaseDocument != null)
		{
			MTBill mTBill = (MTBill)BaseDocument;
			_baseECRSummPLCard = mTBill.SummaPlCard.Value;
			_baseECRSummCash = mTBill.SummaCash.Value - mTBill.SumAddedBonus.Value;
			_baseECRSummPrePayment = mTBill.SummaPrePayment.Value;
			_baseECRSummCredit = mTBill.SummaCredit.Value;
		}
		if (_currCode.Code == "AMD")
		{
			((LayoutItemWithRequired)(object)_currCode.Parent).Visibility = Visibility.Collapsed;
		}
		_prePaymentECRCheckNum.Text = PO.PrePaymentECRCheckNum;
		_basePrePaymentECRCheckNum = PO.BasePrePaymentECRCheckNum;
		_partner.Code = PO.PartnerContractGroup.PartnerCode;
		_contract = PO.PartnerContractGroup.ContractCode;
		_prepaymentPartnerCode = PO.PrepaymentGroup.PartnerCode;
		_prepaymentContractCode = PO.PrepaymentGroup.ContractCode;
		PartnerCacheRow.ContractRow contract = TradeSettings.TradeCacheService.Partner.GetContract(PO.PartnerContractGroup.PartnerCode, PO.PartnerContractGroup.ContractCode);
		if (contract != null)
		{
			_isPartContractWithBonus = contract.IsBonusCard;
			_contractCurrency = contract.CurrCode;
		}
		if (_summaPlCard.Value != 0m)
		{
			((LayoutItemWithRequired)(object)_summaPlCard.Parent).Visibility = Visibility.Visible;
		}
		if (_summaCredit.Value != 0m)
		{
			((LayoutItemWithRequired)(object)_summaCredit.Parent).Visibility = Visibility.Visible;
		}
		if (_summaPrePayment.Value != 0m)
		{
			((LayoutItemWithRequired)(object)_summaPrePayment.Parent).Visibility = Visibility.Visible;
		}
		_summaCash.Value = PO.ECRReturnByCashSum + PO.SummaChange;
		_mtBillReturnList.Clear();
		_focusedRowHandleChangedEvent.HandleOnLoadingDocumentToControls = true;
		foreach (MTReturnFromCustomerBase.MTRetFromCustSpecificationRow mT in PO.MTList)
		{
			MTRetFromCustSpecificationRow mTRetFromCustSpecificationRow = new MTRetFromCustSpecificationRow(this)
			{
				RowID = mT.RowID.Value,
				ItemID = mT.ItemID.Value,
				ItemType = mT.ItemType,
				Storage = mT.Storage,
				BarCode = mT.BarCode,
				ItemCode = mT.ItemCode,
				ItemName = mT.ItemName,
				UnitCode = mT.UnitCode,
				UnitBrief = mT.UnitBrief,
				Quantity = mT.Quantity.Value,
				BeginPrice = mT.BeginPrice.Value,
				Price = mT.Price.Value,
				Discount = mT.Discount.Value,
				DiscountPrice = mT.DiscountPrice.Value,
				Summa = mT.Summa.Value,
				DiscountSumma = mT.DiscountSumma.Value,
				IsRowMarkable = mT.IsRowMarkable,
				VAT = mT.VAT.Value,
				AccumulatedBonus = mT.AccumulatedBonus.Value,
				RowDescription = mT.RowDescription,
				MarkingStatus = mT.MarkingStatus,
				MarkingList = mT.MarkingList,
				IsGift = mT.IsGift.Value,
				IsDeleted = mT.IsDeleted.Value,
				IsStorageRequired = mT.IsStorageRequired.Value,
				IsPartySelected = mT.IsPartySelected,
				UsedBonus = mT.UsedBonus.Value
			};
			if (!mT.IsDeleted.Value)
			{
				_usedDiscount += Math.Abs(mT.Discount.Value);
				_usedBonus += Math.Abs(mT.UsedBonus.Value);
				_accumulatedBonus += Math.Abs(mT.AccumulatedBonus.Value);
			}
			if (_viewMode == WindowViewMode.Copy)
			{
				mTRetFromCustSpecificationRow.MarkingList.Clear();
			}
			ChangeMarkingStatus(mTRetFromCustSpecificationRow);
			_mtBillReturnList.Add(mTRetFromCustSpecificationRow);
		}
		_maxRowID = PO.MaxRowID;
		UpdateChangeOrPaySum();
		_storedBonus = (_usedBonus > 0m) | (_accumulatedBonus > 0m);
		_usedBonusSummCol.Visible = _storedBonus;
		_usedBonusSummCol.ShowInColumnChooser = _storedBonus;
		_accumulatedBonusCol.Visible = _storedBonus;
		_accumulatedBonusCol.ShowInColumnChooser = _storedBonus;
		if (_mtBillReturnList.Count > 0 && (from row in _mtBillReturnList
			where !string.IsNullOrWhiteSpace(row.Storage)
			select row.Storage).Distinct().Count() > 1)
		{
			_storageCol.Visible = true;
			_storageCol.ShowInColumnChooser = true;
			_isShowStorageColInMTBill = true;
		}
		_mtBillReturnGrid.ItemsSource = _mtBillReturnList;
		if (_summaPayByBonus.Value == 0m && !_storedBonus)
		{
			((LayoutItemWithRequired)(object)_summaPayByBonus.Parent).Visibility = Visibility.Collapsed;
		}
		else if (_summaPayByBonus.Value != 0m)
		{
			((LayoutItemWithRequired)(object)_summaPayByBonus.Parent).Visibility = Visibility.Visible;
		}
		else
		{
			((LayoutItemWithRequired)(object)_summaPayByBonus.Parent).Visibility = Visibility.Collapsed;
		}
		if (_useDiscount)
		{
			_mDiscountUse = true;
		}
		else
		{
			_mDiscountUse = false;
			_discountCol.Visible = false;
			_discountCol.ShowInColumnChooser = false;
			_discountPriceCol.Visible = false;
			_discountPriceCol.ShowInColumnChooser = false;
		}
		if (_isCalcDiscountAuto)
		{
			_discountCol.ReadOnly = true;
			_discountCol.AllowFocus = false;
			_discountPriceCol.ReadOnly = true;
			_discountPriceCol.AllowFocus = false;
			if (_useDiscount)
			{
				_summaCol.ReadOnly = true;
				_summaCol.AllowFocus = false;
			}
			else
			{
				_summaCol.ReadOnly = false;
			}
		}
		if (_vatType == "3" || _vatType == "4")
		{
			_vatCol.Visible = false;
			_vatCol.ShowInColumnChooser = false;
		}
		Partner_EditValueChanged(isFromLoadData: true);
		if (!(PO.BaseISN == Guid.Empty) && !PO.StoredInDb)
		{
			CreateBasedOnDoc(baseAlreadySelected: false);
			_usedBonusSummCol.Visible = _storedBonus;
			_usedBonusSummCol.ShowInColumnChooser = _storedBonus;
			_accumulatedBonusCol.Visible = _storedBonus;
			_accumulatedBonusCol.ShowInColumnChooser = _storedBonus;
			if (_summaPayByBonus.Value == 0m && !_storedBonus)
			{
				((LayoutItemWithRequired)(object)_summaPayByBonus.Parent).Visibility = Visibility.Collapsed;
			}
			else if (_summaPayByBonus.Value != 0m)
			{
				((LayoutItemWithRequired)(object)_summaPayByBonus.Parent).Visibility = Visibility.Visible;
			}
			else
			{
				((LayoutItemWithRequired)(object)_summaPayByBonus.Parent).Visibility = Visibility.Collapsed;
			}
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

	protected override void SetControlsReadonly()
	{
		base.SetControlsReadonly();
		if (_viewMode == WindowViewMode.View)
		{
			SetControlsDisabledAndReadOnly();
			if (!IsOfflineDoc)
			{
				_printECRCheck.IsEnabled = (string.IsNullOrWhiteSpace(_ecrCheckNum.Text) || _ecrCheckNum.Text == "XXXXXXXX") && !string.IsNullOrWhiteSpace(_baseDocument.Text);
			}
		}
		else
		{
			SetReadOnlyInner();
			SetOnlineModeDocReadOnlyControls();
		}
	}

	private void SetControlsDisabledAndReadOnly()
	{
		if (_baseDocument != null)
		{
			_baseDocument.IsReadOnly = true;
		}
		if (_ecrCheckNum != null)
		{
			_ecrCheckNum.IsReadOnly = true;
		}
		if (_prePaymentECRCheckNum != null)
		{
			_prePaymentECRCheckNum.IsReadOnly = true;
		}
		if (_inputValue != null)
		{
			_inputValue.IsReadOnly = true;
			_inputValue.IsEnabled = false;
		}
		if (_summaCash != null)
		{
			_summaCash.IsReadOnly = true;
		}
		if (_summaPlCard != null)
		{
			_summaPlCard.IsReadOnly = true;
		}
		if (_summaCredit != null)
		{
			_summaCredit.IsReadOnly = true;
		}
		if (_summaPayByBonus != null)
		{
			_summaPayByBonus.IsReadOnly = true;
		}
		if (_summaPrePayment != null)
		{
			_summaPrePayment.IsReadOnly = true;
		}
		if (_summa != null)
		{
			_summa.IsReadOnly = true;
		}
		if (_summaChange != null)
		{
			_summaChange.IsReadOnly = true;
		}
		if (_increase != null)
		{
			_increase.IsReadOnly = true;
			_increase.IsEnabled = false;
		}
		if (_decrease != null)
		{
			_decrease.IsReadOnly = true;
			_decrease.IsEnabled = false;
		}
		if (_weight != null)
		{
			_weight.IsReadOnly = true;
			_weight.IsEnabled = false;
		}
		if (_discount != null)
		{
			_discount.IsReadOnly = true;
		}
		if (_prices != null)
		{
			_prices.IsReadOnly = true;
			_prices.IsEnabled = false;
		}
		if (_mtFavorites != null)
		{
			_mtFavorites.IsReadOnly = true;
			_mtFavorites.IsEnabled = false;
		}
		if (_selectMarking != null)
		{
			_selectMarking.IsReadOnly = true;
			_selectMarking.IsEnabled = false;
		}
		if (_infoItemName != null)
		{
			_infoItemName.IsReadOnly = true;
			_infoItemName.IsEnabled = false;
		}
		if (_infoFormula != null)
		{
			_infoFormula.IsReadOnly = true;
			_infoFormula.IsEnabled = false;
		}
		if (_hintImage != null)
		{
			_hintImage.IsReadOnly = true;
			_hintImage.IsEnabled = false;
		}
		if (_substitutionInfo != null)
		{
			_substitutionInfo.IsEnabled = false;
			_substitutionInfo.IsReadOnly = true;
		}
		if (_infoDescription != null)
		{
			_infoDescription.IsReadOnly = true;
			_infoDescription.IsEnabled = false;
		}
		if (_itemImage != null)
		{
			_itemImage.IsReadOnly = true;
			_itemImage.IsEnabled = false;
		}
		if (_salePrice != null)
		{
			_salePrice.IsEnabled = false;
		}
		if (_barCodeEnter != null)
		{
			_barCodeEnter.IsEnabled = false;
		}
		if (_codeEnter != null)
		{
			_codeEnter.IsEnabled = false;
		}
		if (_mtListLikeName != null)
		{
			_mtListLikeName.IsEnabled = false;
		}
		if (_qtyEnter != null)
		{
			_qtyEnter.IsEnabled = false;
		}
		if (_printBill != null)
		{
			_printBill.IsEnabled = true;
		}
		if (_printECRCheck != null)
		{
			_printECRCheck.IsEnabled = false;
		}
		if (_plCardPayment != null)
		{
			_plCardPayment.IsEnabled = false;
		}
		if (_credit != null)
		{
			_credit.IsEnabled = false;
		}
		if (_usePrePayment != null)
		{
			_usePrePayment.IsEnabled = false;
		}
		if (_summaPayCash != null)
		{
			_summaPayCash.IsEnabled = false;
		}
		if (_erasePayments != null)
		{
			_erasePayments.IsEnabled = false;
		}
		if (_barSubItemActions != null)
		{
			_barSubItemActions.IsEnabled = false;
		}
		if (_barSubItemJournals != null)
		{
			_barSubItemJournals.IsEnabled = false;
		}
		if (_barButtonPrintLabel != null)
		{
			_barButtonPrintLabel.IsEnabled = false;
		}
		if (_barButtonImportFromFile != null)
		{
			_barButtonImportFromFile.IsEnabled = false;
		}
		if (_barButtonDocumentSettings != null)
		{
			_barButtonDocumentSettings.IsEnabled = false;
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
		if (_barStatusConnectToDS != null)
		{
			_barStatusConnectToDS.IsEnabled = false;
		}
	}

	private void SetReadOnlyInner()
	{
		_date.IsReadOnly = !_allowChangeDate;
		_ecrCheckNum.IsReadOnly = true;
		_prePaymentECRCheckNum.IsReadOnly = true;
		_currCode.IsReadOnly = true;
		_cashierShift.IsReadOnly = true;
		_summa.IsReadOnly = true;
		_summaCash.IsReadOnly = _isCalcDiscountAuto;
		_summaPlCard.IsReadOnly = true;
		_summaPayByBonus.IsReadOnly = true;
		_summaCredit.IsReadOnly = true;
		_summaPrePayment.IsReadOnly = true;
		_summaChange.IsReadOnly = true;
		_owner.IsReadOnly = !Settings.ParametersService.AllowChangeOwnerInMTBill();
		if (!_mtBillSimpleMode)
		{
			_storage.IsReadOnly = !_isAllowChangeStorageInBill;
			_cashDesk.IsReadOnly = !_isAllowChangeCashInBill;
			_partner.IsReadOnly = !_isAllowChangeBuyerInBill || !string.IsNullOrWhiteSpace(_ecrCheckNum.Text);
			_salesConsultant.IsReadOnly = !_isAllowChangeSaleConsultantInBill;
			_comment.IsReadOnly = !Settings.ParametersService.AllowChangeCommentInMTBill();
			_docNum.IsReadOnly = !_isAllowChangeDocNumberInBill;
		}
		else
		{
			_cashDesk.IsReadOnly = true;
			_docNum.IsReadOnly = true;
			_date.IsReadOnly = true;
		}
	}

	private void SetOnlineModeDocReadOnlyControls()
	{
		bool flag = State == DocumentStateSE.Draft.Value || State == DocumentStateSE.Deleted.Value;
		bool flag2 = _viewMode == WindowViewMode.Edit && !flag;
		_readOnlyAfterCheckPrint = flag2 || !string.IsNullOrWhiteSpace(_ecrCheckNum.Text);
		_baseDocument.IsReadOnly = _readOnlyAfterCheckPrint;
		if (_isCalcDiscountAuto && !_readOnlyAfterCheckPrint)
		{
			AutoCalcDiscountAdjustControlsAccessibility();
		}
		else
		{
			if (!_allowChangeDate)
			{
				_date.IsReadOnly = true;
			}
			else
			{
				_date.IsReadOnly = _readOnlyAfterCheckPrint;
			}
			if (_isAllowChangeStorageInBill)
			{
				_storage.IsReadOnly = _readOnlyAfterCheckPrint;
			}
			if (_isAllowChangeCashInBill)
			{
				_cashDesk.IsReadOnly = _readOnlyAfterCheckPrint;
			}
			if (_isAllowChangeBuyerInBill)
			{
				_partner.IsReadOnly = flag2;
			}
			_summaCash.IsReadOnly = _readOnlyAfterCheckPrint;
			if (_isAllowChangeSaleConsultantInBill)
			{
				_salesConsultant.IsReadOnly = flag2;
			}
			_mtBillReturnGrid.AllowAdd = !_readOnlyAfterCheckPrint && _isAllowChangeGoodRowInBill && string.IsNullOrWhiteSpace(_baseDocument.Text);
			_mtBillReturnGrid.AllowEdit = !_readOnlyAfterCheckPrint && _isAllowChangeGoodRowInBill;
			_mtBillReturnGrid.AllowDuplicate = !_readOnlyAfterCheckPrint && _isAllowChangeGoodRowInBill && string.IsNullOrWhiteSpace(_baseDocument.Text);
			_mtBillReturnGrid.AllowDelete = !_readOnlyAfterCheckPrint;
			_mtBillReturnGrid.AllowMoveUp = !_readOnlyAfterCheckPrint;
			_mtBillReturnGrid.AllowMoveDown = !_readOnlyAfterCheckPrint;
			foreach (GridColumn column in _mtBillReturnGrid.Grid.Columns)
			{
				column.ReadOnly = _readOnlyAfterCheckPrint || !_isAllowChangeGoodRowInBill;
			}
			_barCodeEnter.IsEnabled = string.IsNullOrWhiteSpace(_baseDocument.Text);
			_codeEnter.IsEnabled = string.IsNullOrWhiteSpace(_baseDocument.Text);
			_mtListLikeName.IsEnabled = string.IsNullOrWhiteSpace(_baseDocument.Text);
			_qtyEnter.IsEnabled = string.IsNullOrWhiteSpace(_baseDocument.Text);
			_increase.IsEnabled = !_readOnlyAfterCheckPrint;
			if (_allowChangeSalePrices)
			{
				_salePrice.IsEnabled = !_readOnlyAfterCheckPrint;
			}
		}
		_itemCodeCol.ReadOnly = _itemCodeCol.ReadOnly || !string.IsNullOrWhiteSpace(_baseDocument.Text);
		_itemNameCol.ReadOnly = _itemNameCol.ReadOnly || !string.IsNullOrWhiteSpace(_baseDocument.Text);
		_unitCodeCol.ReadOnly = _unitCodeCol.ReadOnly || !string.IsNullOrWhiteSpace(_baseDocument.Text);
		_barCodeCol.ReadOnly = _barCodeCol.ReadOnly || !string.IsNullOrWhiteSpace(_baseDocument.Text);
		if (_printECRCheck != null)
		{
			_printECRCheck.IsEnabled = ((string.IsNullOrWhiteSpace(_ecrCheckNum.Text) || _ecrCheckNum.Text == "XXXXXXXX") && !string.IsNullOrWhiteSpace(_baseDocument.Text)) || (string.IsNullOrWhiteSpace(_prePaymentECRCheckNum.Text) && Settings.ParametersService.ECRCheckPrintBonusType() == PrintBonusTypesSE.DiscountAndPrePayment.Value && CalcBonusFromRows() > 0m) || _prePaymentECRCheckNum.Text == "XXXXXXXX";
		}
		if (_erasePayments != null)
		{
			_erasePayments.IsEnabled = !_readOnlyAfterCheckPrint;
		}
		if (_isAllowChangeDocNumberInBill)
		{
			_docNum.IsReadOnly = _readOnlyAfterCheckPrint;
		}
		_inputValue.IsReadOnly = _readOnlyAfterCheckPrint;
		_rowMarkingListCol.ReadOnly = false;
		((ButtonEditSettings)_rowMarkingListCol.EditSettings).IsTextEditable = false;
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

	private void Partner_ClickDropDown(object sender, DropDownControlEventArgs e)
	{
		IDropDownView dataView = _partner.DataView;
		ModalBrowser modalBrowser = new ModalBrowser(dataView)
		{
			Owner = e.Owner,
			Left = e.Left,
			Top = e.Top
		};
		if (modalBrowser.Show())
		{
			ContractDataProvider.DataRow.Key key = (ContractDataProvider.DataRow.Key)dataView.KeyMember(modalBrowser.SelectedRowIndex);
			if (key != null)
			{
				_contract = key.Contract;
				PO.PartnerContractGroup.ContractCode = _contract;
				_partCode = key.PartCode;
				if (!_readOnlyAfterCheckPrint)
				{
					PartnerCacheRow.ContractRow contract = TradeSettings.TradeCacheService.Partner.GetContract(key.PartCode, key.Contract);
					if (contract != null)
					{
						_isPartContractWithBonus = contract.IsBonusCard;
						_contractCurrency = contract.CurrCode;
						if (!string.IsNullOrWhiteSpace(contract.PriceType) && _priceListType != contract.PriceType)
						{
							_priceListType = contract.PriceType;
							PriceListTypeEditValueChanged(_priceListType);
						}
					}
					if (!_isPartContractWithBonus)
					{
						foreach (MTRetFromCustSpecificationRow mtBillReturn in _mtBillReturnList)
						{
							mtBillReturn.AccumulatedBonus = 0m;
							mtBillReturn.UsedBonus = 0m;
						}
						_summaPayByBonus.EditValue = 0m;
						((LayoutItemWithRequired)(object)_summaPayByBonus.Parent).Visibility = Visibility.Collapsed;
						UpdateChangeOrPaySum();
					}
					if (!string.IsNullOrWhiteSpace(_prepaymentPartnerCode) && _prepaymentPartnerCode == _partCode && _prepaymentContractCode == PO.PartnerContractGroup.ContractCode)
					{
						_prepaymentPartnerCode = "";
						_prepaymentContractCode = "";
					}
				}
			}
			_bPartClickDropDown = true;
			_partner.Code = dataView.ValueMember(modalBrowser.SelectedRowIndex);
		}
		e.Cancel = true;
	}

	private void Partner_EditValueChanged(object sender, EditValueChangedEventArgs edit)
	{
		Partner_EditValueChanged();
	}

	private void Partner_EditValueChanged(bool isFromLoadData = false)
	{
		PartnerCacheRow item = TradeSettings.TradeCacheService.Partner.GetItem(_partner.Code);
		if (item != null)
		{
			PartnerCacheRow.ContractRow contractRow = ((!isFromLoadData) ? TradeSettings.TradeCacheService.Partner.GetDefaultContract(item, "AMD") : TradeSettings.TradeCacheService.Partner.GetContract(item, PO.PartnerContractGroup.ContractCode));
			if (!_readOnlyAfterCheckPrint)
			{
				if (contractRow != null)
				{
					if (!_bPartClickDropDown && !_bPartReadMagneticCard)
					{
						_isPartContractWithBonus = contractRow.IsBonusCard;
						if (!string.IsNullOrWhiteSpace(contractRow.PriceType) && _priceListType != contractRow.PriceType && !isFromLoadData)
						{
							_priceListType = contractRow.PriceType;
							PriceListTypeEditValueChanged(_priceListType);
						}
						PO.PartnerContractGroup.ContractCode = contractRow.Contract;
						_contract = PO.PartnerContractGroup.ContractCode;
						_contractCurrency = contractRow.CurrCode;
						_partCode = item.PartCode;
						if (!_isPartContractWithBonus)
						{
							foreach (MTRetFromCustSpecificationRow mtBillReturn in _mtBillReturnList)
							{
								mtBillReturn.AccumulatedBonus = 0m;
								mtBillReturn.UsedBonus = 0m;
							}
							_summaPayByBonus.EditValue = 0m;
							((LayoutItemWithRequired)(object)_summaPayByBonus.Parent).Visibility = Visibility.Collapsed;
						}
					}
					else
					{
						PO.PartnerContractGroup.ContractCode = _contract;
						contractRow = TradeSettings.TradeCacheService.Partner.GetContract(item, _contract);
						if (contractRow != null)
						{
							_isPartContractWithBonus = contractRow.IsBonusCard;
							_contractCurrency = contractRow.CurrCode;
							if (!string.IsNullOrWhiteSpace(contractRow.PriceType) && _priceListType != contractRow.PriceType)
							{
								_priceListType = contractRow.PriceType;
								PriceListTypeEditValueChanged(_priceListType);
							}
							_partCode = item.PartCode;
							if (!_isPartContractWithBonus)
							{
								foreach (MTRetFromCustSpecificationRow mtBillReturn2 in _mtBillReturnList)
								{
									mtBillReturn2.AccumulatedBonus = 0m;
									mtBillReturn2.UsedBonus = 0m;
								}
								_summaPayByBonus.EditValue = 0m;
								((LayoutItemWithRequired)(object)_summaPayByBonus.Parent).Visibility = Visibility.Collapsed;
							}
						}
						_bPartClickDropDown = false;
						_bPartReadMagneticCard = false;
					}
					if (!string.IsNullOrWhiteSpace(_prepaymentPartnerCode) && _prepaymentPartnerCode == _partner.Code && _prepaymentContractCode.Trim() == PO.PartnerContractGroup.ContractCode)
					{
						_prepaymentPartnerCode = "";
						_prepaymentContractCode = "";
					}
				}
				else
				{
					PO.PartnerContractGroup.ContractCode = "";
					_contractCurrency = "AMD";
				}
			}
			else if (contractRow != null)
			{
				PO.PartnerContractGroup.ContractCode = contractRow.Contract;
				_contract = PO.PartnerContractGroup.ContractCode;
				_contractCurrency = contractRow.CurrCode;
				_partCode = item.PartCode;
			}
		}
		else
		{
			PO.PartnerContractGroup.ContractCode = "";
			if (!_readOnlyAfterCheckPrint)
			{
				_isPartContractWithBonus = false;
				_contract = PO.PartnerContractGroup.ContractCode;
				_partCode = "";
				_contractCurrency = "AMD";
				foreach (MTRetFromCustSpecificationRow mtBillReturn3 in _mtBillReturnList)
				{
					mtBillReturn3.AccumulatedBonus = 0m;
					mtBillReturn3.UsedBonus = 0m;
				}
				_summaPayByBonus.EditValue = 0m;
				((LayoutItemWithRequired)(object)_summaPayByBonus.Parent).Visibility = Visibility.Collapsed;
			}
		}
		UpdateChangeOrPaySum();
		SetBuyerInfoToPoleDisplay();
	}

	private void CashDesk_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		if (!string.IsNullOrWhiteSpace(_cashDesk.Code) && TradeSettings.TradeCacheService.Cash.GetItem(_cashDesk.Code) != null && !TradeSettings.CreateTradeClient().CommonServices.IsCashDeskAvailable((string)e.NewValue))
		{
			e.Handled = true;
			_cashDesk.Code = (string)e.OldValue;
			ArmSoft.SME.Core.UI.MessageBox.Show(string.Format(ArmSoft.SME.Trade.Common.Resources.CashDeskNotAvailForUser, Settings.LoginService.UserName, (string)e.NewValue), Settings.ProgramName, MessageBoxButton.OK, MessageBoxImage.Exclamation);
			return;
		}
		if ((string)e.NewValue != (string)e.OldValue)
		{
			SetPOSTerminalInfo();
		}
		SetCashierShiftNumber();
		SetNextDocumentNumber();
	}

	private void SetPOSTerminalInfo()
	{
		CashCacheRow item = TradeSettings.TradeCacheService.Cash.GetItem(_cashDesk.Code);
		CashCacheRow.NonCashInfo nonCashInfo = null;
		if (item != null)
		{
			nonCashInfo = TradeSettings.TradeCacheService.Cash.GetNonCashInfoListByCashCode(_cashDesk.Code).FirstOrDefault((CashCacheRow.NonCashInfo x) => x.Type == 5);
		}
		else
		{
			PO.POSTerminalType = "0";
			PO.BankAcquiring.PartnerCode = "";
			PO.BankAcquiring.ContractCode = "";
			PO.POSTerminalAddress = "";
			_nonCashPaymentMethod = 0;
		}
		if (nonCashInfo != null)
		{
			PO.POSTerminalType = nonCashInfo.Device;
			PO.BankAcquiring.PartnerCode = nonCashInfo.Partner;
			PO.BankAcquiring.ContractCode = nonCashInfo.Contract;
			PO.POSTerminalAddress = nonCashInfo.POSTerminalAddress;
			_nonCashPaymentMethod = 0;
		}
	}

	private void InputValue_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Invalid comparison between Unknown and I4
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		if ((int)e.Key == 6 && (int)Keyboard.Modifiers == 0 && !_inputValue.IsReadOnly && !string.IsNullOrWhiteSpace(_inputValue.Text))
		{
			e.Handled = true;
			On_BarCodeEnterClick();
		}
	}

	private void Summa_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		if (_isShowPoleDisplay)
		{
			int num = CalcMTBillReturnListRowCount(includeDeleted: false);
			if (num == 0)
			{
				ClearPoleDisplay(string.IsNullOrWhiteSpace(_partCode));
				return;
			}
			SetTotalInfoToPoleDisplay();
			SetRowCountInfoToPoleDisplay(num);
		}
	}

	protected override void OnClosed(EventArgs e)
	{
		if (_isShowPoleDisplay && TradeSettings.PoleDisplay != null)
		{
			ClearPoleDisplay(clearAll: true);
		}
		base.OnClosed(e);
	}

	protected override void OnClosing(CancelEventArgs e)
	{
		if (_isAllowChangeGoodRowInBill || (PO.State != DocumentStateSE.Draft.Value && (_mtBillReturnList.Count == 0 || PO.StoredInDb)) || !IsChanged)
		{
			base.OnClosing(e);
		}
		else
		{
			e.Cancel = true;
		}
	}

	private void SelectMarking_Click(object sender, RoutedEventArgs e)
	{
		if (_mtBillReturnTableView.Grid.CurrentItem == null)
		{
			return;
		}
		MTRetFromCustSpecificationRow mTRetFromCustSpecificationRow = (MTRetFromCustSpecificationRow)_mtBillReturnTableView.Grid.CurrentItem;
		if (mTRetFromCustSpecificationRow == null)
		{
			return;
		}
		if (!MarkingHelpers.IsGoodMarkable(mTRetFromCustSpecificationRow.ItemCode, _useMarking, mTRetFromCustSpecificationRow.MarkingList.Count != 0, _date.EditValue.Value, out var isRowMarkable))
		{
			if (string.IsNullOrWhiteSpace(mTRetFromCustSpecificationRow.ItemCode))
			{
				throw new ASException(ArmSoft.SME.Common.Resources.ValidateEmptyCode);
			}
			ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Common.Resources.NotMarkable, Settings.ProgramName, MessageBoxButton.OK, MessageBoxImage.Asterisk, MessageBoxResult.OK);
			return;
		}
		mTRetFromCustSpecificationRow.IsRowMarkable = isRowMarkable;
		List<string> allMarkings = _mtBillReturnList.SelectMany((MTRetFromCustSpecificationRow row) => row.MarkingList).ToList();
		MarkingSelectWindow markingSelectWindow = new MarkingSelectWindow(this, _readOnlyAfterCheckPrint, mTRetFromCustSpecificationRow.IsRowMarkable, mTRetFromCustSpecificationRow.ItemCode, mTRetFromCustSpecificationRow.ItemName, mTRetFromCustSpecificationRow.MarkingList, allMarkings, mTRetFromCustSpecificationRow.Quantity, isReturn: true);
		if (markingSelectWindow.ShowDialog().Value && !markingSelectWindow.IsViewOnlyMode)
		{
			mTRetFromCustSpecificationRow.MarkingList.Clear();
			foreach (MarkingSelectWindow.MarkingRow marking in markingSelectWindow.MarkingList)
			{
				mTRetFromCustSpecificationRow.MarkingList.Add(marking.Marking);
			}
		}
		CalcSummAndSummVAT();
		UpdateChangeOrPaySum();
		ChangeMarkingStatus(mTRetFromCustSpecificationRow);
		_rowInfoChange?.Invoke(mTRetFromCustSpecificationRow);
	}

	private void MTBillReturnGrid_AddingRow(object sender, DocumentGridAddingRowEventArgs e)
	{
		if (!_date.DoValidate() || !_date.EditValue.HasValue)
		{
			ControlsHelper.SetFocus(_date);
			throw new ASException(ArmSoft.SME.Common.Resources.ValidateEmptyDate);
		}
		_mtBillReturnList.Add(new MTRetFromCustSpecificationRow(this)
		{
			ItemType = ItemTypesSE.Goods.Value,
			Storage = _storage.Code,
			RowID = _maxRowID
		});
		_maxRowID++;
		e.RowNumber = _mtBillReturnList.Count - 1;
	}

	private void MTBillReturnGrid_DuplicatingRow(object sender, DocumentGridDuplicatingRowEventArgs e)
	{
		MTRetFromCustSpecificationRow mTRetFromCustSpecificationRow = (MTRetFromCustSpecificationRow)_mtBillReturnGrid.Grid.GetRow(e.RowHandle);
		if (mTRetFromCustSpecificationRow != null)
		{
			MTRetFromCustSpecificationRow mTRetFromCustSpecificationRow2 = new MTRetFromCustSpecificationRow(this);
			mTRetFromCustSpecificationRow.Clone(mTRetFromCustSpecificationRow2);
			mTRetFromCustSpecificationRow2.UpdateRemainsOnInfoPanel = false;
			mTRetFromCustSpecificationRow2.AccumulatedBonus = 0m;
			mTRetFromCustSpecificationRow2.UsedBonus = 0m;
			mTRetFromCustSpecificationRow2.RowID = _maxRowID;
			_mtBillReturnList.Add(mTRetFromCustSpecificationRow2);
			_maxRowID++;
			e.NewRowNumber = _mtBillReturnList.Count - 1;
			CalculateDocumentSumma();
			ChangeRowInfo(mTRetFromCustSpecificationRow2);
		}
	}

	private void MTBillReturnGrid_MovingRowUp(object sender, DocumentGridMovingRowUpOrDownEventArgs e)
	{
		if (_mtBillReturnGrid.Grid.SortInfo.Count > 0 || _mtBillReturnTableView.Grid.VisibleRowCount != _mtBillReturnList.Count)
		{
			throw new ASException(ArmSoft.SME.Common.Resources.NotAllowRowMoveInSortedGrid);
		}
		MTRetFromCustSpecificationRow mTRetFromCustSpecificationRow = (MTRetFromCustSpecificationRow)_mtBillReturnGrid.Grid.GetRow(e.RowHandle);
		if (mTRetFromCustSpecificationRow != null)
		{
			_mtBillReturnList.RemoveAt(e.RowHandle);
			_mtBillReturnList.Insert(e.RowHandle - 1, mTRetFromCustSpecificationRow);
		}
	}

	private void MTBillReturnGrid_MovingRowDown(object sender, DocumentGridMovingRowUpOrDownEventArgs e)
	{
		if (_mtBillReturnGrid.Grid.SortInfo.Count > 0 || _mtBillReturnTableView.Grid.VisibleRowCount != _mtBillReturnList.Count)
		{
			throw new ASException(ArmSoft.SME.Common.Resources.NotAllowRowMoveInSortedGrid);
		}
		MTRetFromCustSpecificationRow mTRetFromCustSpecificationRow = (MTRetFromCustSpecificationRow)_mtBillReturnGrid.Grid.GetRow(e.RowHandle);
		if (mTRetFromCustSpecificationRow != null)
		{
			_mtBillReturnList.RemoveAt(e.RowHandle);
			_mtBillReturnList.Insert(e.RowHandle + 1, mTRetFromCustSpecificationRow);
		}
	}

	private void MTBillReturnGrid_DeletingRow(object sender, DocumentGridDeletingRowEventArgs e)
	{
		MTRetFromCustSpecificationRow mTRetFromCustSpecificationRow = (MTRetFromCustSpecificationRow)_mtBillReturnGrid.Grid.GetRow(e.RowHandle);
		if (_isAllowChangeGoodRowInBill)
		{
			if (mTRetFromCustSpecificationRow != null)
			{
				_rowInfoChange?.Invoke(null);
			}
			return;
		}
		e.Cancel = true;
		if (mTRetFromCustSpecificationRow != null)
		{
			mTRetFromCustSpecificationRow.IsDeleted = !mTRetFromCustSpecificationRow.IsDeleted;
			mTRetFromCustSpecificationRow.AccumulatedBonus = 0m;
			mTRetFromCustSpecificationRow.UsedBonus = 0m;
			_rowInfoChange?.Invoke(null);
		}
		CalculateDocumentSumma();
	}

	private void MTBillReturnGrid_DeletedRow(object sender, EventArgs e)
	{
		CalculateDocumentSumma();
	}

	private void MTBillReturnGrid_AddingRowWithDialog(object sender, DocumentGridAddingRowEventArgs e)
	{
		ValidateDocFields();
	}

	private void ValidateDocFields()
	{
		if (!_date.DoValidate() || !_date.EditValue.HasValue)
		{
			ControlsHelper.SetFocus(_date);
			throw new ASException(ArmSoft.SME.Common.Resources.ValidateEmptyDate);
		}
		if (string.IsNullOrWhiteSpace(_storage.Code) && !_isShowStorageColInMTBill)
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

	private void MTBillReturnGrid_RefreshButtons(object sender, EventArgs e)
	{
		if (!IsOfflineDoc && _viewMode != WindowViewMode.View && !_mtBillSimpleMode && (_readOnlyAfterCheckPrint || !_isCalcDiscountAuto))
		{
			if (_useDiscount)
			{
				_discount.IsEnabled = !_readOnlyAfterCheckPrint;
			}
			_increase.IsEnabled = !_readOnlyAfterCheckPrint;
			_decrease.IsEnabled = !_readOnlyAfterCheckPrint;
			if (_mtFavorites != null)
			{
				_mtFavorites.IsEnabled = !_readOnlyAfterCheckPrint;
			}
		}
	}

	private void MTBillReturnGrid_ClickDropDown(object sender, DocumentGridClickDropDownEventArgs e)
	{
		MTRetFromCustSpecificationRow mTRetFromCustSpecificationRow = (MTRetFromCustSpecificationRow)_mtBillReturnGrid.Grid.GetRow(e.RowHandle);
		if (!_date.DoValidate() || !_date.EditValue.HasValue)
		{
			ControlsHelper.SetFocus(_date);
			throw new ASException(ArmSoft.SME.Common.Resources.ValidateEmptyDate);
		}
		if (e.Column == _itemCodeCol)
		{
			string text = "";
			ArmSoft.SME.Trade.UI.DropDown.GoodsAvailabilityFilter goodsAvailabilityFilter = new ArmSoft.SME.Trade.UI.DropDown.GoodsAvailabilityFilter(storage: (!string.IsNullOrWhiteSpace(mTRetFromCustSpecificationRow.Storage)) ? mTRetFromCustSpecificationRow.Storage : _storage.Code, asOfDate: _date.EditValue.Value, forRetail: _retail, includeServices: !_retail);
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
		else if (e.Column == _storageCol)
		{
			if (mTRetFromCustSpecificationRow.ItemType != GoodsTypesSE.Good.Value)
			{
				return;
			}
			ArmSoft.SME.Trade.UI.DropDown.Storages storages = new ArmSoft.SME.Trade.UI.DropDown.Storages(false);
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
		else if (e.Column == _barCodeCol)
		{
			if (mTRetFromCustSpecificationRow == null)
			{
				return;
			}
			ArmSoft.SME.Trade.UI.DropDown.MTBarCodes mTBarCodes = new ArmSoft.SME.Trade.UI.DropDown.MTBarCodes(isShowQntUnitCaption: false, showBarCodeFirst: true, Settings.ParametersService.ShowMTInclInPrList(), showAlsoMTQntUnitsEmptyBarCodes: true);
			ModalBrowser modalBrowser3 = new ModalBrowser(mTBarCodes)
			{
				LookUpValue = e.Value,
				Owner = e.Owner,
				Left = e.Left,
				Top = e.Top
			};
			if (modalBrowser3.Show())
			{
				e.NewValue = mTBarCodes.ValueMember(modalBrowser3.SelectedRowIndex);
				mTRetFromCustSpecificationRow.ItemCode = mTBarCodes.CommentMember(modalBrowser3.SelectedRowIndex);
				MaterialCacheRow item = TradeSettings.TradeCacheService.Material.GetItem(mTRetFromCustSpecificationRow.ItemCode, throwExceptionNotExits: true);
				mTRetFromCustSpecificationRow.ItemType = item.Type;
				mTRetFromCustSpecificationRow.ItemName = item.Caption;
				mTRetFromCustSpecificationRow.ItemID = item.Id;
				string qntUnit = "";
				MTBarCodeDataProvider.DataRow rowByIndex = mTBarCodes.GetRowByIndex(modalBrowser3.SelectedRowIndex);
				mTRetFromCustSpecificationRow.BarCode = rowByIndex.BarCode;
				if (string.IsNullOrWhiteSpace(mTRetFromCustSpecificationRow.BarCode))
				{
					qntUnit = rowByIndex.QntUnit;
				}
				else
				{
					TradeSettings.TradeCacheService.Material.GetItemByBarCode(mTRetFromCustSpecificationRow.BarCode, out qntUnit, throwExceptionNotExits: true);
					if (qntUnit == null)
					{
						qntUnit = item.Unit;
					}
				}
				mTRetFromCustSpecificationRow.UnitCode = qntUnit;
				_unitCodeCol.DataView = new ArmSoft.SME.Trade.UI.DropDown.MTQntUnits(item.MTCode);
				mTRetFromCustSpecificationRow.VAT = item.VAT;
				mTRetFromCustSpecificationRow.Discount = 0m;
				if (!_retail || mTRetFromCustSpecificationRow.ItemType == ItemTypesSE.Services.Value)
				{
					mTRetFromCustSpecificationRow.Price = TradeSettings.CreateTradeClient().CommonServices.GetPriceForGivenPriceListAndVATType(_priceListType, _vatType, mTRetFromCustSpecificationRow.ItemID, mTRetFromCustSpecificationRow.UnitCode, _date.EditValue.Value, _currCode.Code);
					mTRetFromCustSpecificationRow.Price = Methods.RoundPrice(mTRetFromCustSpecificationRow.Price, _priceListTypeRoundDgr);
					CalculateGridRowSummaAndDiscountSumma(mTRetFromCustSpecificationRow, calcDiscountPrice: false);
				}
				else
				{
					MaterialCacheRow.QntUnitRow qntUnit2 = TradeSettings.TradeCacheService.Material.GetQntUnit(mTRetFromCustSpecificationRow.ItemID, mTRetFromCustSpecificationRow.UnitCode);
					if (qntUnit2 != null)
					{
						MTRetailQtyAndPriceDataProvider.Param param = new MTRetailQtyAndPriceDataProvider.Param
						{
							MTCode = mTRetFromCustSpecificationRow.ItemCode,
							Storage = mTRetFromCustSpecificationRow.Storage,
							Date = _date.EditValue,
							UnitCoef = qntUnit2.Coef
						};
						List<MTRetailQtyAndPriceDataProvider.DataRow> rows = new MTRetailQtyAndPriceDataProvider(TradeSettings.CreateTradeClient()).GetRows(param);
						if (rows.Count > 0)
						{
							mTRetFromCustSpecificationRow.Price = Methods.RoundPrice(rows[0].Price, _priceListTypeRoundDgr);
						}
					}
					if (!string.IsNullOrWhiteSpace(_priceListType))
					{
						if (_useDiscount)
						{
							mTRetFromCustSpecificationRow.Price = TradeSettings.CreateTradeClient().CommonServices.GetPriceForGivenPriceListAndVATType(_priceListType, _vatType, mTRetFromCustSpecificationRow.ItemID, mTRetFromCustSpecificationRow.UnitCode, _date.EditValue.Value, _currCode.Code);
							mTRetFromCustSpecificationRow.Price = Methods.RoundPrice(mTRetFromCustSpecificationRow.Price, _priceListTypeRoundDgr);
							if (mTRetFromCustSpecificationRow.Price == 0m)
							{
								mTRetFromCustSpecificationRow.Price = mTRetFromCustSpecificationRow.Price;
							}
						}
						else
						{
							CalculateGridRowSummaAndDiscountSumma(mTRetFromCustSpecificationRow, calcDiscountPrice: false);
						}
					}
					else
					{
						CalculateGridRowSummaAndDiscountSumma(mTRetFromCustSpecificationRow, calcDiscountPrice: false);
					}
				}
				QntUnitCacheRow item2 = TradeSettings.TradeCacheService.QntUnit.GetItem(mTRetFromCustSpecificationRow.UnitCode);
				mTRetFromCustSpecificationRow.UnitBrief = ((item2 != null) ? item2.Brief : "");
			}
		}
		else if (e.Column == _priceCol)
		{
			if (_retail)
			{
				if (mTRetFromCustSpecificationRow == null)
				{
					return;
				}
				if (mTRetFromCustSpecificationRow.ItemID != -1 && TradeSettings.TradeCacheService.Material.GetItem(mTRetFromCustSpecificationRow.ItemCode).Type == GoodsTypesSE.Good.Value)
				{
					MaterialCacheRow.QntUnitRow qntUnit3 = TradeSettings.TradeCacheService.Material.GetQntUnit(mTRetFromCustSpecificationRow.ItemID, mTRetFromCustSpecificationRow.UnitCode);
					if (qntUnit3 != null)
					{
						MTRetailQtyAndPrice mTRetailQtyAndPrice = new MTRetailQtyAndPrice(mTRetFromCustSpecificationRow.ItemCode, qntUnit3.Coef, mTRetFromCustSpecificationRow.Storage, _date.EditValue.Value);
						ModalBrowser modalBrowser4 = new ModalBrowser(mTRetailQtyAndPrice)
						{
							LookUpValue = mTRetFromCustSpecificationRow.UnitCode,
							Owner = e.Owner,
							Left = e.Left,
							Top = e.Top
						};
						if (modalBrowser4.Show())
						{
							mTRetFromCustSpecificationRow.Price = Methods.RoundPrice(decimal.Parse(mTRetailQtyAndPrice.CommentMember(modalBrowser4.SelectedRowIndex)), _priceListTypeRoundDgr);
							CalculateGridRowSummaAndDiscountSumma(mTRetFromCustSpecificationRow, mTRetFromCustSpecificationRow.Price == 0m);
						}
					}
				}
			}
		}
		else if (e.Column == _rowMarkingListCol)
		{
			if (mTRetFromCustSpecificationRow == null || mTRetFromCustSpecificationRow.ItemType != GoodsTypesSE.Good.Value)
			{
				return;
			}
			OpenMarkingSelectWindow(mTRetFromCustSpecificationRow);
		}
		CalcSummAndSummVAT();
		UpdateChangeOrPaySum();
		_rowInfoChange?.Invoke(mTRetFromCustSpecificationRow);
	}

	private void MTBillReturnTableView_CellValueChanged(object sender, CellValueChangedEventArgs e)
	{
		MTRetFromCustSpecificationRow mTRetFromCustSpecificationRow = (MTRetFromCustSpecificationRow)_mtBillReturnGrid.Grid.GetRow(e.RowHandle);
		if (mTRetFromCustSpecificationRow == null)
		{
			return;
		}
		string text = "";
		if (e.OldValue != null)
		{
			text = e.OldValue.ToString();
		}
		if (!_date.DoValidate() || !_date.EditValue.HasValue)
		{
			ControlsHelper.SetFocus(_date);
			ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Common.Resources.ValidateEmptyDate, Settings.ProgramName, MessageBoxButton.OK, MessageBoxImage.Exclamation);
			return;
		}
		if (e.Column == _itemCodeCol)
		{
			try
			{
				if (e.Value == null)
				{
					mTRetFromCustSpecificationRow.Storage = "";
					mTRetFromCustSpecificationRow.BarCode = "";
					mTRetFromCustSpecificationRow.ItemID = -1;
					mTRetFromCustSpecificationRow.ItemCode = "";
					mTRetFromCustSpecificationRow.ItemName = "";
					mTRetFromCustSpecificationRow.UnitBrief = "";
					mTRetFromCustSpecificationRow.UnitCode = "";
					mTRetFromCustSpecificationRow.Quantity = 0m;
					mTRetFromCustSpecificationRow.Price = 0m;
					mTRetFromCustSpecificationRow.Summa = 0m;
					mTRetFromCustSpecificationRow.Discount = 0m;
					mTRetFromCustSpecificationRow.DiscountPrice = 0m;
					mTRetFromCustSpecificationRow.DiscountSumma = 0m;
					mTRetFromCustSpecificationRow.MarkingList.Clear();
					mTRetFromCustSpecificationRow.MarkingStatus = "";
				}
				else
				{
					ChangeItemCodeValue(e.Value.ToString(), mTRetFromCustSpecificationRow);
				}
			}
			catch (Exception)
			{
				mTRetFromCustSpecificationRow.Storage = "";
				mTRetFromCustSpecificationRow.BarCode = "";
				mTRetFromCustSpecificationRow.ItemID = -1;
				mTRetFromCustSpecificationRow.ItemCode = "";
				mTRetFromCustSpecificationRow.ItemName = "";
				mTRetFromCustSpecificationRow.UnitBrief = "";
				mTRetFromCustSpecificationRow.UnitCode = "";
				mTRetFromCustSpecificationRow.Quantity = 0m;
				mTRetFromCustSpecificationRow.Price = 0m;
				mTRetFromCustSpecificationRow.Summa = 0m;
				mTRetFromCustSpecificationRow.Discount = 0m;
				mTRetFromCustSpecificationRow.DiscountPrice = 0m;
				mTRetFromCustSpecificationRow.DiscountSumma = 0m;
				mTRetFromCustSpecificationRow.MarkingList.Clear();
				mTRetFromCustSpecificationRow.MarkingStatus = "";
			}
		}
		else if (e.Column == _storageCol)
		{
			if (e.Value != null && !string.IsNullOrEmpty(e.Value.ToString()) && TradeSettings.TradeCacheService.Storage.GetItem(e.Value.ToString()) != null)
			{
				StoragesCacheRow item = TradeSettings.TradeCacheService.Storage.GetItem(e.Value.ToString());
				if (item != null && item.Retail)
				{
					mTRetFromCustSpecificationRow.Storage = "";
				}
				else if (!TradeSettings.CreateTradeClient().CommonServices.IsStorageAvailable(e.Value.ToString()))
				{
					ArmSoft.SME.Core.UI.MessageBox.Show(string.Format(ArmSoft.SME.Trade.Common.Resources.StorageNotAvailForUser, Settings.LoginService.UserName, e.Value.ToString()), Settings.ProgramName, MessageBoxButton.OK, MessageBoxImage.Exclamation);
					mTRetFromCustSpecificationRow.Storage = text;
				}
			}
		}
		else if (e.Column == _barCodeCol)
		{
			if (e.Value != null)
			{
				MaterialCacheRow itemByBarCode = TradeSettings.TradeCacheService.Material.GetItemByBarCode(e.Value.ToString(), out var qntUnit);
				if (itemByBarCode != null)
				{
					mTRetFromCustSpecificationRow.BarCode = itemByBarCode.BarCode;
					mTRetFromCustSpecificationRow.ItemType = itemByBarCode.Type;
					mTRetFromCustSpecificationRow.ItemName = itemByBarCode.Caption;
					mTRetFromCustSpecificationRow.ItemID = itemByBarCode.Id;
					mTRetFromCustSpecificationRow.ItemCode = itemByBarCode.MTCode;
					mTRetFromCustSpecificationRow.UnitCode = qntUnit;
					_unitCodeCol.DataView = new ArmSoft.SME.Trade.UI.DropDown.MTQntUnits(itemByBarCode.MTCode);
					mTRetFromCustSpecificationRow.VAT = itemByBarCode.VAT;
					if (string.IsNullOrWhiteSpace(mTRetFromCustSpecificationRow.Storage) && itemByBarCode.Type == GoodsTypesSE.Good.Value)
					{
						mTRetFromCustSpecificationRow.Storage = _storage.Code;
					}
					else if (itemByBarCode.Type == GoodsTypesSE.Service.Value)
					{
						mTRetFromCustSpecificationRow.IsStorageRequired = false;
					}
					if (!_retail || mTRetFromCustSpecificationRow.ItemType == GoodsTypesSE.Service.Value)
					{
						mTRetFromCustSpecificationRow.Price = TradeSettings.CreateTradeClient().CommonServices.GetPriceForGivenPriceListAndVATType(_priceListType, _vatType, mTRetFromCustSpecificationRow.ItemID, mTRetFromCustSpecificationRow.UnitCode, _date.EditValue.Value, _currCode.Code);
						mTRetFromCustSpecificationRow.Price = Methods.RoundPrice(mTRetFromCustSpecificationRow.Price, _priceListTypeRoundDgr);
					}
					else if (!string.IsNullOrWhiteSpace(_priceListType))
					{
						if (_useDiscount)
						{
							mTRetFromCustSpecificationRow.Price = TradeSettings.CreateTradeClient().CommonServices.GetPriceForGivenPriceListAndVATType(_priceListType, _vatType, mTRetFromCustSpecificationRow.ItemID, mTRetFromCustSpecificationRow.UnitCode, _date.EditValue.Value, _currCode.Code);
							mTRetFromCustSpecificationRow.Price = Methods.RoundPrice(mTRetFromCustSpecificationRow.Price, _priceListTypeRoundDgr);
							if (mTRetFromCustSpecificationRow.Price == 0m)
							{
								if (mTRetFromCustSpecificationRow.Discount != 0m)
								{
									mTRetFromCustSpecificationRow.Price = (mTRetFromCustSpecificationRow.Price * 100m / (100m - mTRetFromCustSpecificationRow.Discount)).RoundAs(2);
								}
							}
							else
							{
								mTRetFromCustSpecificationRow.Discount = (100m * (mTRetFromCustSpecificationRow.Price - mTRetFromCustSpecificationRow.Price) / mTRetFromCustSpecificationRow.Price).RoundAs(2);
							}
							CalculateGridRowSummaAndDiscountSumma(mTRetFromCustSpecificationRow, calcDiscountPrice: false);
						}
					}
					else
					{
						CalculateGridRowSummaAndDiscountSumma(mTRetFromCustSpecificationRow, mTRetFromCustSpecificationRow.Price == 0m);
					}
					QntUnitCacheRow item2 = TradeSettings.TradeCacheService.QntUnit.GetItem(mTRetFromCustSpecificationRow.UnitCode);
					mTRetFromCustSpecificationRow.UnitBrief = ((item2 != null) ? item2.Brief : "");
					if (mTRetFromCustSpecificationRow.ItemType != GoodsTypesSE.Good.Value)
					{
						mTRetFromCustSpecificationRow.Storage = "";
					}
				}
			}
		}
		else if (e.Column == _unitCodeCol)
		{
			if (e.Value != null && !string.IsNullOrWhiteSpace(e.Value.ToString()))
			{
				UnitChanged(e.Value.ToString(), text, mTRetFromCustSpecificationRow);
			}
		}
		else if (e.Column == _quantityCol)
		{
			if ((decimal)e.OldValue != (decimal)e.Value && (decimal)e.OldValue != 0m)
			{
				mTRetFromCustSpecificationRow.Quantity = (decimal)e.Value;
				if (mTRetFromCustSpecificationRow.ItemType == GoodsTypesSE.Good.Value && (decimal)e.OldValue != 0m && (decimal)e.Value != 0m)
				{
					mTRetFromCustSpecificationRow.UsedBonus = ((decimal)e.Value * mTRetFromCustSpecificationRow.UsedBonus / (decimal)e.OldValue).RoundAs(2);
					mTRetFromCustSpecificationRow.AccumulatedBonus = ((decimal)e.Value * mTRetFromCustSpecificationRow.AccumulatedBonus / (decimal)e.OldValue).RoundAs(2);
					_usedBonus = _usedBonus - (decimal)e.OldValue + mTRetFromCustSpecificationRow.UsedBonus;
				}
				if ((!_retail || mTRetFromCustSpecificationRow.ItemType == GoodsTypesSE.Service.Value) && mTRetFromCustSpecificationRow.Price == 0m && _priceListType != "")
				{
					mTRetFromCustSpecificationRow.Price = TradeSettings.CreateTradeClient().CommonServices.GetPriceForGivenPriceListAndVATType(_priceListType, _vatType, mTRetFromCustSpecificationRow.ItemID, mTRetFromCustSpecificationRow.UnitCode, _date.EditValue.Value, _currCode.Code);
					mTRetFromCustSpecificationRow.Price = Methods.RoundPrice(mTRetFromCustSpecificationRow.Price, _priceListTypeRoundDgr);
				}
				ChangeMarkingStatus(mTRetFromCustSpecificationRow);
			}
			mTRetFromCustSpecificationRow.Summa = ((decimal)e.Value * mTRetFromCustSpecificationRow.Price).RoundAs(2);
			CalculateGridRowSummaAndDiscountSumma(mTRetFromCustSpecificationRow, calcDiscountPrice: false);
		}
		else if (e.Column == _priceCol)
		{
			if ((decimal)e.OldValue != (decimal)e.Value)
			{
				mTRetFromCustSpecificationRow.Price = Methods.RoundPrice((decimal)e.Value, _priceListTypeRoundDgr);
				mTRetFromCustSpecificationRow.BeginPrice = mTRetFromCustSpecificationRow.Price;
				mTRetFromCustSpecificationRow.DiscountPrice = mTRetFromCustSpecificationRow.Price;
				CalculateGridRowSummaAndDiscountSumma(mTRetFromCustSpecificationRow);
			}
		}
		else if (e.Column == _discountCol)
		{
			CalculateGridRowSummaAndDiscountSumma(mTRetFromCustSpecificationRow);
		}
		else if (e.Column == _discountPriceCol)
		{
			mTRetFromCustSpecificationRow.DiscountPrice = Methods.RoundPrice((decimal)e.Value, _priceListTypeRoundDgr);
			if (!(mTRetFromCustSpecificationRow.BeginPrice == 0m))
			{
				mTRetFromCustSpecificationRow.Discount = (100m * (mTRetFromCustSpecificationRow.BeginPrice - mTRetFromCustSpecificationRow.DiscountPrice) / mTRetFromCustSpecificationRow.BeginPrice).RoundAs(2);
				CalculateGridRowSummaAndDiscountSumma(mTRetFromCustSpecificationRow, calcDiscountPrice: false);
				if (mTRetFromCustSpecificationRow.Discount / 1000m >= 1m || mTRetFromCustSpecificationRow.Discount / 1000m <= -1m)
				{
					mTRetFromCustSpecificationRow.Discount = 0m;
					mTRetFromCustSpecificationRow.DiscountPrice = mTRetFromCustSpecificationRow.BeginPrice;
					mTRetFromCustSpecificationRow.DiscountSumma = 0m;
					mTRetFromCustSpecificationRow.Summa = mTRetFromCustSpecificationRow.BeginPrice * mTRetFromCustSpecificationRow.Quantity;
				}
			}
		}
		else if (e.Column == _summaCol)
		{
			if (!(mTRetFromCustSpecificationRow.Quantity == 0m))
			{
				mTRetFromCustSpecificationRow.DiscountPrice = Methods.RoundPrice((mTRetFromCustSpecificationRow.Summa / mTRetFromCustSpecificationRow.Quantity).RoundAs(2), _priceListTypeRoundDgr);
				if (!_useDiscount)
				{
					mTRetFromCustSpecificationRow.Price = mTRetFromCustSpecificationRow.DiscountPrice;
				}
			}
			if (!(mTRetFromCustSpecificationRow.Price == 0m) && _useDiscount)
			{
				mTRetFromCustSpecificationRow.Discount = (100m * (mTRetFromCustSpecificationRow.Price - mTRetFromCustSpecificationRow.DiscountPrice) / mTRetFromCustSpecificationRow.Price).RoundAs(2);
				mTRetFromCustSpecificationRow.DiscountSumma = (mTRetFromCustSpecificationRow.Quantity * mTRetFromCustSpecificationRow.Price - mTRetFromCustSpecificationRow.Summa).RoundAs(2);
			}
		}
		else if (e.Column == _discountSummCol)
		{
			mTRetFromCustSpecificationRow.Summa = (mTRetFromCustSpecificationRow.Quantity * mTRetFromCustSpecificationRow.Price - mTRetFromCustSpecificationRow.Summa).RoundAs(16);
			mTRetFromCustSpecificationRow.Price = Methods.RoundPrice((mTRetFromCustSpecificationRow.Summa / mTRetFromCustSpecificationRow.Quantity).RoundAs(2), _priceListTypeRoundDgr);
			if (!(mTRetFromCustSpecificationRow.Price == 0m))
			{
				mTRetFromCustSpecificationRow.Discount = (100m * (mTRetFromCustSpecificationRow.Price - mTRetFromCustSpecificationRow.DiscountPrice) / mTRetFromCustSpecificationRow.Price).RoundAs(2);
			}
		}
		CalculateDocumentSumma();
		_rowInfoChange?.Invoke(mTRetFromCustSpecificationRow);
	}

	private void ChangeItemCodeValue(string newCode, MTRetFromCustSpecificationRow row, bool preservePreviousUnit = false, bool isSubstitution = false)
	{
		MaterialCacheRow item = TradeSettings.TradeCacheService.Material.GetItem(newCode);
		if (string.IsNullOrWhiteSpace(row.Storage) && item.Type == GoodsTypesSE.Good.Value)
		{
			row.Storage = _storage.Code;
		}
		else if (item.Type == GoodsTypesSE.Service.Value)
		{
			row.IsStorageRequired = false;
		}
		row.BarCode = item.BarCode;
		row.ItemType = item.Type;
		row.ItemName = item.Caption;
		row.ItemID = item.Id;
		row.ItemCode = item.MTCode;
		if (!preservePreviousUnit)
		{
			row.UnitBrief = item.UnitRow.Brief;
			row.UnitCode = item.Unit;
		}
		_unitCodeCol.DataView = new ArmSoft.SME.Trade.UI.DropDown.MTQntUnits(item.MTCode);
		row.VAT = item.VAT;
		if (!_retail || row.ItemType == GoodsTypesSE.Service.Value)
		{
			if (!isSubstitution)
			{
				row.UnitCode = TradeSettings.CreateTradeClient().CommonServices.GetMTQntUnitByMTIDPriceTypeAndDocDate(row.ItemID, _priceListType, _date.EditValue.Value);
				if (string.IsNullOrWhiteSpace(row.UnitCode))
				{
					row.UnitCode = item.Unit;
				}
				else
				{
					QntUnitCacheRow item2 = TradeSettings.TradeCacheService.QntUnit.GetItem(row.UnitCode);
					row.UnitBrief = ((item2 != null) ? item2.Brief : "");
				}
			}
			row.Price = TradeSettings.CreateTradeClient().CommonServices.GetPriceForGivenPriceListAndVATType(_priceListType, _vatType, row.ItemID, row.UnitCode, _date.EditValue.Value, _currCode.Code);
			row.Price = Methods.RoundPrice(row.Price, _priceListTypeRoundDgr);
			CalculateGridRowSummaAndDiscountSumma(row, row.Price == 0m);
		}
		else
		{
			MaterialCacheRow.QntUnitRow qntUnit = TradeSettings.TradeCacheService.Material.GetQntUnit(row.ItemID, row.UnitCode);
			if (qntUnit != null)
			{
				MTRetailQtyAndPriceDataProvider.Param param = new MTRetailQtyAndPriceDataProvider.Param
				{
					MTCode = row.ItemCode,
					UnitCoef = qntUnit.Coef,
					Storage = row.Storage,
					Date = _date.EditValue
				};
				List<MTRetailQtyAndPriceDataProvider.DataRow> rows = new MTRetailQtyAndPriceDataProvider(TradeSettings.CreateTradeClient()).GetRows(param);
				if (rows.Count > 0)
				{
					row.Price = Methods.RoundPrice(rows[0].Price, _priceListTypeRoundDgr);
				}
			}
			if (!string.IsNullOrWhiteSpace(_priceListType))
			{
				if (_useDiscount)
				{
					row.Price = TradeSettings.CreateTradeClient().CommonServices.GetPriceForGivenPriceListAndVATType(_priceListType, _vatType, row.ItemID, row.UnitCode, _date.EditValue.Value, _currCode.Code);
					row.Price = Methods.RoundPrice(row.Price, _priceListTypeRoundDgr);
					if (row.Price == 0m)
					{
						if (row.Discount != 0m)
						{
							row.Price = (row.Price * 100m / (100m - row.Discount)).RoundAs(2);
						}
					}
					else
					{
						row.Discount = (100m * (row.Price - row.Price) / row.Price).RoundAs(2);
					}
					CalculateGridRowSummaAndDiscountSumma(row, calcDiscountPrice: false);
				}
				else
				{
					CalculateGridRowSummaAndDiscountSumma(row, calcDiscountPrice: false);
				}
			}
			else
			{
				CalculateGridRowSummaAndDiscountSumma(row, row.Price == 0m);
			}
		}
		row.MarkingList.Clear();
		row.MarkingStatus = "";
	}

	private void MTBillReturnTableView_FocusedRowHandleChanged(object sender, FocusedRowHandleChangedEventArgs e)
	{
		MTRetFromCustSpecificationRow mTRetFromCustSpecificationRow = (MTRetFromCustSpecificationRow)_mtBillReturnGrid.Grid.GetRow(_mtBillReturnTableView.FocusedRowHandle);
		if (mTRetFromCustSpecificationRow != null)
		{
			ChangeColumnAdditonalView(mTRetFromCustSpecificationRow.ItemCode);
			_rowInfoChange?.Invoke(mTRetFromCustSpecificationRow);
		}
	}

	private void ChangeColumnAdditonalView(string itemCode)
	{
		_unitCodeCol.DataView = new ArmSoft.SME.Trade.UI.DropDown.MTQntUnits(itemCode);
	}

	private void MTBillReturnTableView_ShowingEditor(object sender, ShowingEditorEventArgs e)
	{
		if (e.Row == null)
		{
			return;
		}
		if (e.Column == _storageCol)
		{
			if (((MTRetFromCustSpecificationRow)e.Row).ItemType == GoodsTypesSE.Service.Value)
			{
				e.Cancel = true;
			}
		}
		else if (e.Column == _itemNameCol)
		{
			if (((MTRetFromCustSpecificationRow)e.Row).ItemType == GoodsTypesSE.Good.Value)
			{
				e.Cancel = true;
			}
		}
		else if (e.Column == _rowMarkingListCol && ((MTRetFromCustSpecificationRow)e.Row).ItemType != GoodsTypesSE.Good.Value)
		{
			e.Cancel = true;
		}
	}

	private void MTBillReturnTableView_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
	{
		TableView view = (TableView)sender;
		TableView tableView = view;
		object originalSource = e.OriginalSource;
		TableViewHitInfo hitInfo = tableView.CalcHitInfo((DependencyObject)((originalSource is DependencyObject) ? originalSource : null));
		if (!hitInfo.InRowCell)
		{
			return;
		}
		object row = _mtBillReturnGrid.Grid.GetRow(hitInfo.RowHandle);
		MTRetFromCustSpecificationRow row2 = row as MTRetFromCustSpecificationRow;
		if (row2 == null || row2.ItemType != GoodsTypesSE.Good.Value || hitInfo.Column == null)
		{
			return;
		}
		((DispatcherObject)this).Dispatcher.BeginInvoke((Delegate)(Action)(() =>
		{
			if (hitInfo.Column == _rowMarkingListCol)
			{
				OpenMarkingSelectWindow(row2);
			}
			view.CommitEditing();
		}), (DispatcherPriority)4, Array.Empty<object>());
	}

	private void OpenMarkingSelectWindow(MTRetFromCustSpecificationRow mtBillReturnGridRow)
	{
		if (!MarkingHelpers.IsGoodMarkable(mtBillReturnGridRow.ItemCode, _useMarking, mtBillReturnGridRow.MarkingList.Count != 0, _date.EditValue.Value, out var isRowMarkable))
		{
			if (string.IsNullOrWhiteSpace(mtBillReturnGridRow.ItemCode))
			{
				throw new ASException(ArmSoft.SME.Common.Resources.ValidateEmptyCode);
			}
			ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Common.Resources.NotMarkable, Settings.ProgramName, MessageBoxButton.OK, MessageBoxImage.Asterisk, MessageBoxResult.OK);
			return;
		}
		mtBillReturnGridRow.IsRowMarkable = isRowMarkable;
		List<string> allMarkings = _mtBillReturnList.SelectMany((MTRetFromCustSpecificationRow row) => row.MarkingList).ToList();
		MarkingSelectWindow markingSelectWindow = new MarkingSelectWindow(this, _readOnlyAfterCheckPrint, mtBillReturnGridRow.IsRowMarkable, mtBillReturnGridRow.ItemCode, mtBillReturnGridRow.ItemName, mtBillReturnGridRow.MarkingList, allMarkings, mtBillReturnGridRow.Quantity, isReturn: true);
		if (!markingSelectWindow.ShowDialog().Value || markingSelectWindow.IsViewOnlyMode)
		{
			return;
		}
		mtBillReturnGridRow.MarkingList.Clear();
		foreach (MarkingSelectWindow.MarkingRow marking in markingSelectWindow.MarkingList)
		{
			mtBillReturnGridRow.MarkingList.Add(marking.Marking);
		}
		ChangeMarkingStatus(mtBillReturnGridRow);
	}

	private void Increase_Click(object sender, RoutedEventArgs e)
	{
		IncreaseDecreaseRowQty();
	}

	private void Decrease_Click(object sender, RoutedEventArgs e)
	{
		IncreaseDecreaseRowQty(increase: false);
	}

	private void Weight_Click(object sender, RoutedEventArgs e)
	{
		On_WeightClick();
	}

	private void SalePrice_Click(object sender, RoutedEventArgs e)
	{
		On_SalePriceClick();
	}

	private void BarCodeEnter_Click(object sender, RoutedEventArgs e)
	{
		On_BarCodeEnterClick();
	}

	private void CodeEnter_Click(object sender, RoutedEventArgs e)
	{
		On_CodeEnterClick();
	}

	private void MTListLikeName_Click(object sender, RoutedEventArgs e)
	{
		On_MTListLikeNameClick();
	}

	private void BE_QtyEnter_Click(object sender, RoutedEventArgs e)
	{
		On_QtyEnterClick();
	}

	private void BE_PrintECRCheck_Click(object sender, RoutedEventArgs e)
	{
		On_PrintECRCheckClick();
	}

	private void BE_PrintBill_Click(object sender, RoutedEventArgs e)
	{
		On_PrintMTBillReturnClick();
	}

	private void Credit_Click(object sender, RoutedEventArgs e)
	{
		On_CreditClick();
	}

	private void PlCardPayment_Click(object sender, RoutedEventArgs e)
	{
		On_PlCardPaymentClick();
	}

	private void MTFavorites_Click(object sender, RoutedEventArgs e)
	{
		On_MTFavoritesClick();
	}

	private void Prices_Click(object sender, RoutedEventArgs e)
	{
		On_PricesClick();
	}

	private void UsePrePayment_Click(object sender, RoutedEventArgs e)
	{
		On_UsePrePaymentClick();
	}

	private void SummaCash_Click(object sender, RoutedEventArgs e)
	{
		On_SummaCashClick();
	}

	private void ErasePayments_Click(object sender, RoutedEventArgs e)
	{
		On_ErasePaymentsClick();
	}

	private void SummaCash_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		if ((decimal)e.NewValue != (decimal)e.OldValue)
		{
			UpdateChangeOrPaySum();
			if (_isCalcDiscountAuto)
			{
				AutoCalcDiscountAdjustControlsAccessibility();
			}
		}
	}

	private void OtherPaymentTypes_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		if ((decimal)e.NewValue != (decimal)e.OldValue && _isCalcDiscountAuto)
		{
			AutoCalcDiscountAdjustControlsAccessibility();
		}
	}

	private void SummaPrePayment_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		if ((decimal)e.NewValue != (decimal)e.OldValue && _isCalcDiscountAuto)
		{
			AutoCalcDiscountAdjustControlsAccessibility();
		}
	}

	private void SummaChange_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		if ((decimal)e.NewValue < 0m)
		{
			_summaChange.Foreground = Brushes.Red;
		}
		else
		{
			_summaChange.Foreground = _summaCash.Foreground;
		}
		if (_isCalcDiscountAuto)
		{
			AutoCalcDiscountAdjustControlsAccessibility();
		}
		if (_isShowPoleDisplay)
		{
			int num = CalcMTBillReturnListRowCount(includeDeleted: false);
			if (num == 0)
			{
				ClearPoleDisplay(string.IsNullOrWhiteSpace(_partCode));
				return;
			}
			SetTotalInfoToPoleDisplay();
			SetRowCountInfoToPoleDisplay(num);
		}
	}

	private int CalcMTBillReturnListRowCount(bool includeDeleted)
	{
		if (!includeDeleted)
		{
			return _mtBillReturnList.Count((MTRetFromCustSpecificationRow x) => !x.IsDeleted);
		}
		return _mtBillReturnList.Count;
	}

	private void CalcSummAndSummVAT()
	{
		decimal summVAT = 0m;
		decimal num = 0m;
		foreach (MTRetFromCustSpecificationRow mtBillReturn in _mtBillReturnList)
		{
			if (!mtBillReturn.IsDeleted)
			{
				num += mtBillReturn.Summa;
				summVAT += ArmSoft.SME.Trade.UI.Helpers.PriceHelper.CalcVATAmountByType(mtBillReturn.Summa, _vatType, mtBillReturn.VAT.Value);
			}
		}
		decimal num2 = num;
		if (!string.IsNullOrWhiteSpace(Settings.ParametersService.MTBillRoundDegree()))
		{
			num2 = Methods.RoundPrice(num, Settings.ParametersService.MTBillRoundDegree(), Settings.ParametersService.MTBillSummRoundUp());
		}
		_sumFromRound = num2 - num;
		_summa.EditValue = num2;
		PO.SummVAT = summVAT;
	}

	private void BaseDocument_ClickDropDown(object sender, DropDownControlEventArgs e)
	{
		if (!_date.DoValidate() || _date == null)
		{
			e.Cancel = true;
			ControlsHelper.SetFocus(_date);
			throw new ASException(ArmSoft.SME.Common.Resources.ValidateEmptyDate);
		}
		DocumentsJournal documentsJournal = (DocumentsJournal)e.DataView;
		documentsJournal.PartnerCode = _partner.Code;
		documentsJournal.DocState = DocumentStateSE.Processed.ValueUI;
		documentsJournal.DialogTop = e.Top;
		documentsJournal.DialogLeft = e.Left;
		if (!e.DataView.ShowDialog())
		{
			e.Cancel = true;
		}
	}

	private void BaseDocument_EditKeyValueChanged(object sender, EditKeyValueChangedEventArgs e)
	{
		if (e.KeyValue == null)
		{
			BaseISN = Guid.Empty;
			return;
		}
		BaseISN = (Guid)e.KeyValue;
		if (_printECRCheck != null)
		{
			_printECRCheck.IsEnabled = !string.IsNullOrWhiteSpace(_baseECRCheckNum);
		}
	}

	private void BaseDocumentDelete_Click(object sender, EditKeyValueChangedEventArgs e)
	{
		_baseDocument.KeyValue = Guid.Empty;
		_baseDocument.DoValidate();
		_mtBillReturnGrid.Grid.Columns["ItemCode"].ReadOnly = false;
		_mtBillReturnGrid.Grid.Columns["UnitCode"].ReadOnly = false;
		_mtBillReturnGrid.AllowAdd = true;
		_mtBillReturnGrid.AllowDuplicate = true;
	}

	private void BaseDocumentSearch_Click(object sender, EditKeyValueChangedEventArgs e)
	{
		if (BaseISN != Guid.Empty)
		{
			Settings.DocumentService.ShowWindow(BaseISN, WindowViewMode.Edit);
		}
	}

	private void CreateBasedOnDoc(bool baseAlreadySelected)
	{
		_comment.EditValue = BaseDocument.Comment;
		_currCode.Code = BaseDocument.CurrCode;
		_partner.Code = BaseDocument.PartnerContractGroup.PartnerCode;
		_partCode = BaseDocument.PartnerContractGroup.PartnerCode;
		_contract = BaseDocument.PartnerContractGroup.ContractCode;
		PartnerCacheRow.ContractRow contract = TradeSettings.TradeCacheService.Partner.GetContract(_partCode, _contract);
		if (contract != null)
		{
			_isPartContractWithBonus = contract.IsBonusCard;
			_contractCurrency = contract.CurrCode;
		}
		_summa.EditValue = BaseDocument.Summ;
		if (BaseDocument.POType == DocumentTypeSE.MTBill.Value)
		{
			MTBill mTBill = (MTBill)BaseDocument;
			_summaPlCard.Value = mTBill.SummaPlCard.Value;
			_summaCash.Value = mTBill.SummaCash.Value - mTBill.SummaChange.Value - mTBill.SumAddedBonus.Value;
			_summaPrePayment.Value = mTBill.SummaPrePayment.Value;
			_summaCredit.Value = mTBill.SummaCredit.Value;
			_summaPayByBonus.Value = mTBill.SummaPayByBonus.Value;
			if (_summaPlCard.Value == 0m)
			{
				((LayoutItemWithRequired)(object)_summaPlCard.Parent).Visibility = Visibility.Collapsed;
			}
			else
			{
				((LayoutItemWithRequired)(object)_summaPlCard.Parent).Visibility = Visibility.Visible;
			}
			if (_summaPrePayment.Value == 0m)
			{
				((LayoutItemWithRequired)(object)_summaPrePayment.Parent).Visibility = Visibility.Collapsed;
			}
			else
			{
				((LayoutItemWithRequired)(object)_summaPrePayment.Parent).Visibility = Visibility.Visible;
				_prepaymentPartnerCode = mTBill.PrepaymentGroup.PartnerCode;
				_prepaymentContractCode = mTBill.PrepaymentGroup.ContractCode;
			}
			if (_summaPayByBonus.Value == 0m)
			{
				((LayoutItemWithRequired)(object)_summaPayByBonus.Parent).Visibility = Visibility.Collapsed;
			}
			else
			{
				((LayoutItemWithRequired)(object)_summaPayByBonus.Parent).Visibility = Visibility.Visible;
			}
			if (_summaCredit.Value == 0m)
			{
				((LayoutItemWithRequired)(object)_summaCredit.Parent).Visibility = Visibility.Collapsed;
			}
			else
			{
				((LayoutItemWithRequired)(object)_summaCredit.Parent).Visibility = Visibility.Visible;
			}
			_baseECRCheckNum = mTBill.ECRCheckNum;
			_basePrePaymentECRCheckNum = mTBill.PrePaymentECRCheckNum;
			_baseECRSummPLCard = mTBill.SummaPlCard.Value;
			_baseECRSummCash = mTBill.SummaCash.Value;
			_baseECRSummPrePayment = mTBill.SummaPrePayment.Value;
			_baseECRSummCredit = mTBill.SummaCredit.Value;
			_nonCashPaymentMethod = mTBill.NonCashPaymentMethod;
			PO.BankAcquiring.PartnerCode = mTBill.BankAcquiring.PartnerCode;
			PO.BankAcquiring.ContractCode = mTBill.BankAcquiring.ContractCode;
			PO.POSTerminalType = mTBill.POSTerminalType;
			_vatType = mTBill.VATType;
			List<MTInvoiceBase.MTInvoiceSpecificationRow> list = new List<MTInvoiceBase.MTInvoiceSpecificationRow>();
			if (baseAlreadySelected)
			{
				LoadMTList(mTBill.MTList);
			}
			else if (_mtBillReturnList.Count != 0 && mTBill.MTList.Count != _mtBillReturnList.Count)
			{
				foreach (MTRetFromCustSpecificationRow item in _mtBillReturnList)
				{
					MTInvoiceBase.MTInvoiceSpecificationRow mTInvoiceSpecificationRow = mTBill.MTList.FirstOrDefault((MTInvoiceBase.MTInvoiceSpecificationRow r) => r.ItemID == item.ItemID);
					if (mTInvoiceSpecificationRow != null)
					{
						list.Add(mTInvoiceSpecificationRow);
					}
					ChangeMarkingStatus(item);
				}
				LoadMTList(list);
			}
			else
			{
				LoadMTList(mTBill.MTList);
			}
			_salesConsultant.Code = mTBill.SalesConsultant;
			_sumFromRound = mTBill.SumFromRound;
		}
		UpdateChangeOrPaySum();
		_mtBillReturnGrid.Grid.Columns["ItemCode"].ReadOnly = true;
		_mtBillReturnGrid.Grid.Columns["ItemName"].ReadOnly = true;
		_mtBillReturnGrid.Grid.Columns["UnitCode"].ReadOnly = true;
		_mtBillReturnGrid.Grid.Columns["BarCode"].ReadOnly = true;
		_mtBillReturnGrid.AllowAdd = false;
		_mtBillReturnGrid.AllowDuplicate = false;
		_codeEnter.IsEnabled = false;
		_barCodeEnter.IsEnabled = false;
		_mtListLikeName.IsEnabled = false;
		_increase.IsEnabled = false;
		_qtyEnter.IsEnabled = false;
		if (_mtBillReturnList.Count > 0)
		{
			SetTotalInfoToPoleDisplay();
			SetRowCountInfoToPoleDisplay(_mtBillReturnList.Count);
		}
		CalcSummAndSummVAT();
		_mtBillReturnGrid.Grid.RefreshData();
	}

	private void LoadMTList(List<MTInvoiceBase.MTInvoiceSpecificationRow> mtList)
	{
		_baseDocList = mtList;
		_mtBillReturnList.Clear();
		_maxRowID = 0;
		foreach (MTInvoiceBase.MTInvoiceSpecificationRow mt in mtList)
		{
			if (mt.IsDeleted)
			{
				continue;
			}
			MTRetFromCustSpecificationRow mTRetFromCustSpecificationRow = new MTRetFromCustSpecificationRow(this)
			{
				IsDeleted = false,
				ItemType = mt.ItemType,
				ItemID = mt.ItemID,
				ItemCode = mt.ItemCode,
				BarCode = mt.BarCode,
				ItemName = mt.ItemName,
				FullName = mt.FullName,
				UnitCode = mt.UnitCode,
				UnitBrief = mt.UnitBrief,
				Quantity = mt.Quantity,
				Discount = mt.Discount,
				BeginSumma = mt.BeginSumma,
				BeginPrice = mt.Price,
				DiscountPrice = mt.DiscountPrice,
				Price = mt.Price,
				Summa = mt.Summa,
				DiscountSumma = mt.DiscountSumma,
				UsedBonus = mt.UsedBonus,
				AccumulatedBonus = mt.AccumulatedBonus,
				VAT = mt.VAT.Value,
				RowDescription = mt.RowDescription,
				RowID = mt.RowID
			};
			mTRetFromCustSpecificationRow.Storage = ((mTRetFromCustSpecificationRow.ItemType == GoodsTypesSE.Good.Value) ? mt.Storage : "");
			if (mTRetFromCustSpecificationRow.UsedBonus != 0m || mTRetFromCustSpecificationRow.AccumulatedBonus != 0m)
			{
				_storedBonus = true;
			}
			if (BaseDocument.POType == DocumentTypeSE.MTBill.Value && mTRetFromCustSpecificationRow.ItemType == GoodsTypesSE.Good.Value)
			{
				if (!string.IsNullOrEmpty(_storage.Code))
				{
					mTRetFromCustSpecificationRow.Storage = _storage.Code;
				}
				else if (!string.IsNullOrEmpty(Settings.ParametersService.DefaultStorage()))
				{
					mTRetFromCustSpecificationRow.Storage = Settings.ParametersService.DefaultStorage();
				}
			}
			ChangeMarkingStatus(mTRetFromCustSpecificationRow);
			_mtBillReturnList.Add(mTRetFromCustSpecificationRow);
			_maxRowID++;
		}
	}

	private void CalcAccoumulatedAndUsedBonuses()
	{
		_usedBonus = 0m;
		_accumulatedBonus = 0m;
		foreach (MTRetFromCustSpecificationRow mtBillReturn in _mtBillReturnList)
		{
			if (!mtBillReturn.IsDeleted)
			{
				_usedBonus += Math.Abs(mtBillReturn.UsedBonus);
				_accumulatedBonus += Math.Abs(mtBillReturn.AccumulatedBonus);
			}
		}
		_summaPayByBonus.Value = _usedBonus;
		if (_summaPayByBonus.Value != 0m)
		{
			((LayoutItemWithRequired)(object)_summaPayByBonus.Parent).Visibility = Visibility.Visible;
		}
		else
		{
			((LayoutItemWithRequired)(object)_summaPayByBonus.Parent).Visibility = Visibility.Collapsed;
		}
	}

	private void CalculateGridRowSummaAndDiscountSumma(MTRetFromCustSpecificationRow mtBillReturnGridRow, bool calcDiscountPrice = true)
	{
		if (_mDiscountUse)
		{
			if (calcDiscountPrice)
			{
				if (!string.IsNullOrWhiteSpace(_priceListTypeRoundDgr))
				{
					mtBillReturnGridRow.DiscountPrice = Methods.RoundPrice(mtBillReturnGridRow.BeginPrice * (100m - mtBillReturnGridRow.Discount) / 100m, _priceListTypeRoundDgr);
				}
				else
				{
					mtBillReturnGridRow.DiscountPrice = (mtBillReturnGridRow.BeginPrice * (100m - mtBillReturnGridRow.Discount) / 100m).RoundAs(2);
				}
			}
			mtBillReturnGridRow.Summa = (mtBillReturnGridRow.Quantity * mtBillReturnGridRow.DiscountPrice).RoundAs(2);
			mtBillReturnGridRow.DiscountSumma = (mtBillReturnGridRow.Quantity * mtBillReturnGridRow.BeginPrice - mtBillReturnGridRow.Summa).RoundAs(2);
		}
		else
		{
			mtBillReturnGridRow.DiscountPrice = mtBillReturnGridRow.Price;
			mtBillReturnGridRow.Summa = (mtBillReturnGridRow.Quantity * mtBillReturnGridRow.BeginPrice).RoundAs(2);
			mtBillReturnGridRow.DiscountSumma = 0m;
		}
	}

	private void CalculateDocumentSumma()
	{
		CalcSummAndSummVAT();
		UpdateChangeOrPaySum();
		if (_currCode.Code == "AMD")
		{
			PO.SummNCC = _summa.Value;
			return;
		}
		ExchangeRate exchangeRates = TradeSettings.CreateTradeClient().CommonServices.GetExchangeRates(_currCode.Code, _date.EditValue.Value);
		PO.SummNCC = ArmSoft.SME.Trade.Common.Helpers.PriceHelper.GetSumNNC(_summa.Value, exchangeRates);
	}

	private void UpdateChangeOrPaySum()
	{
		decimal num = PaidSum() - _summa.Value;
		_summaChange.EditValue = Math.Abs(num);
		if (num <= 0m)
		{
			((LayoutItemWithRequired)(object)_summaChange.Parent).Label = ArmSoft.SME.Trade.Common.Resources.ForReturn;
			((LayoutItemWithRequired)(object)_summaChange.Parent).Foreground = (SolidColorBrush)new BrushConverter().ConvertFrom("#dd0326");
			_summaChange.Foreground = (SolidColorBrush)new BrushConverter().ConvertFrom("#dd0326");
		}
		else
		{
			((LayoutItemWithRequired)(object)_summaChange.Parent).Label = ArmSoft.SME.Trade.Common.Resources.GetChange;
			((LayoutItemWithRequired)(object)_summaChange.Parent).Foreground = _summaCash.Foreground;
			_summaChange.Foreground = _summaCash.Foreground;
		}
	}

	private void UnitChanged(string unitCodeNew, string unitCodeOld, MTRetFromCustSpecificationRow mtBillReturnGridRow)
	{
		QntUnitCacheRow item = TradeSettings.TradeCacheService.QntUnit.GetItem(unitCodeNew);
		if (item != null)
		{
			mtBillReturnGridRow.UnitBrief = item.Brief;
			MaterialCacheRow item2 = TradeSettings.TradeCacheService.Material.GetItem(mtBillReturnGridRow.ItemCode);
			MaterialCacheRow.QntUnitRow qntUnit = TradeSettings.TradeCacheService.Material.GetQntUnit(mtBillReturnGridRow.ItemID, item.Code);
			MaterialCacheRow.BarCodeRow barCode = TradeSettings.TradeCacheService.Material.GetBarCode(mtBillReturnGridRow.ItemID, item.Code);
			mtBillReturnGridRow.BarCode = ((barCode == null) ? "" : barCode.BarCode);
			if (!_retail || item2.Type == GoodsTypesSE.Service.Value)
			{
				if (!string.IsNullOrWhiteSpace(_priceListType))
				{
					decimal num = ArmSoft.SME.Trade.UI.Helpers.PriceHelper.GetPriceForGivenPriceListAndVATType(_priceListType, _vatType, mtBillReturnGridRow.ItemID, item.Code, _date.EditValue.Value, _currCode.Code);
					if (num == 0m)
					{
						num = ArmSoft.SME.Trade.UI.Helpers.PriceHelper.GetPriceForUnit(unitCodeOld, unitCodeNew, mtBillReturnGridRow.Price, mtBillReturnGridRow.ItemCode);
					}
					mtBillReturnGridRow.Price = Methods.RoundPrice(num, _priceListTypeRoundDgr);
				}
				else
				{
					MaterialCacheRow.QntUnitRow qntUnit2 = TradeSettings.TradeCacheService.Material.GetQntUnit(mtBillReturnGridRow.ItemID, unitCodeOld);
					if (barCode != null && qntUnit2 != null)
					{
						mtBillReturnGridRow.Price = (mtBillReturnGridRow.Price / qntUnit2.Coef * qntUnit.Coef).RoundAs(2);
						mtBillReturnGridRow.Price = Methods.RoundPrice(mtBillReturnGridRow.Price, _priceListTypeRoundDgr);
					}
				}
				CalculateGridRowSummaAndDiscountSumma(mtBillReturnGridRow, calcDiscountPrice: false);
			}
			else
			{
				mtBillReturnGridRow.Price = ArmSoft.SME.Trade.UI.Helpers.PriceHelper.GetPriceForUnit(unitCodeOld, unitCodeNew, mtBillReturnGridRow.Price, mtBillReturnGridRow.ItemCode);
				if (!string.IsNullOrWhiteSpace(_priceListType))
				{
					CalculateGridRowSummaAndDiscountSumma(mtBillReturnGridRow, calcDiscountPrice: false);
				}
				else
				{
					CalculateGridRowSummaAndDiscountSumma(mtBillReturnGridRow, calcDiscountPrice: false);
				}
			}
		}
		else
		{
			mtBillReturnGridRow.UnitBrief = "";
			mtBillReturnGridRow.BarCode = "";
		}
	}

	public override void OnScanner(string scannerData)
	{
		if (!_readOnlyAfterCheckPrint)
		{
			_inputValue.Text = scannerData;
			On_BarCodeEnterClick();
		}
	}

	public override void OnMagneticCardReader(string mcrData)
	{
		if (!string.IsNullOrWhiteSpace(mcrData) && !_readOnlyAfterCheckPrint && MCRInfo.GetInstance().IsValidCard(mcrData))
		{
			ArmSoft.SME.Trade.UI.Helpers.DocumentHelpers.GetPartnerEmployeeDataByBarCode(MCRInfo.GetInstance().GetCardNumber(mcrData), out var partnerCacheRow, out var partnerContract, out var employee, "AMD");
			FillPartnerEmployeeData(partnerCacheRow, partnerContract, employee);
		}
	}

	private void DocumentWindow_PreviewKeyDown(object sender, KeyEventArgs e)
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Invalid comparison between Unknown and I4
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Invalid comparison between Unknown and I4
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Invalid comparison between Unknown and I4
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0086: Invalid comparison between Unknown and I4
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Invalid comparison between Unknown and I4
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ab: Invalid comparison between Unknown and I4
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_008e: Invalid comparison between Unknown and I4
		//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d0: Invalid comparison between Unknown and I4
		//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b3: Invalid comparison between Unknown and I4
		//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f5: Invalid comparison between Unknown and I4
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d8: Invalid comparison between Unknown and I4
		//IL_0113: Unknown result type (might be due to invalid IL or missing references)
		//IL_011a: Invalid comparison between Unknown and I4
		//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fd: Invalid comparison between Unknown and I4
		//IL_0138: Unknown result type (might be due to invalid IL or missing references)
		//IL_013f: Invalid comparison between Unknown and I4
		//IL_011c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0122: Invalid comparison between Unknown and I4
		//IL_015d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0164: Invalid comparison between Unknown and I4
		//IL_0141: Unknown result type (might be due to invalid IL or missing references)
		//IL_0147: Invalid comparison between Unknown and I4
		//IL_0182: Unknown result type (might be due to invalid IL or missing references)
		//IL_0189: Invalid comparison between Unknown and I4
		//IL_0166: Unknown result type (might be due to invalid IL or missing references)
		//IL_016c: Invalid comparison between Unknown and I4
		//IL_01a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ae: Invalid comparison between Unknown and I4
		//IL_018b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0191: Invalid comparison between Unknown and I4
		//IL_01cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d3: Invalid comparison between Unknown and I4
		//IL_01b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b6: Invalid comparison between Unknown and I4
		//IL_01f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f8: Invalid comparison between Unknown and I4
		//IL_01d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01db: Invalid comparison between Unknown and I4
		//IL_0216: Unknown result type (might be due to invalid IL or missing references)
		//IL_021d: Invalid comparison between Unknown and I4
		//IL_01fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0200: Invalid comparison between Unknown and I4
		//IL_023b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0242: Invalid comparison between Unknown and I4
		//IL_021f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0225: Invalid comparison between Unknown and I4
		//IL_0260: Unknown result type (might be due to invalid IL or missing references)
		//IL_0267: Invalid comparison between Unknown and I4
		//IL_0244: Unknown result type (might be due to invalid IL or missing references)
		//IL_024a: Invalid comparison between Unknown and I4
		//IL_0285: Unknown result type (might be due to invalid IL or missing references)
		//IL_028c: Invalid comparison between Unknown and I4
		//IL_0269: Unknown result type (might be due to invalid IL or missing references)
		//IL_026f: Invalid comparison between Unknown and I4
		//IL_02aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b1: Invalid comparison between Unknown and I4
		//IL_028e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0294: Invalid comparison between Unknown and I4
		//IL_02b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b9: Invalid comparison between Unknown and I4
		if (!_comment.IsKeyboardFocusWithin && !_docNum.IsKeyboardFocusWithin)
		{
			if ((int)e.Key == 85 && (int)Keyboard.Modifiers == 0)
			{
				OnPlus();
				e.Handled = true;
			}
			else if ((int)e.Key == 87 && (int)Keyboard.Modifiers == 0)
			{
				OnMinus();
				e.Handled = true;
			}
			else if ((int)e.Key == 91 && (int)Keyboard.Modifiers == 2)
			{
				HotKeyRedirector("Ctrl+F2");
				e.Handled = true;
			}
			else if ((int)e.Key == 92 && (int)Keyboard.Modifiers == 2)
			{
				HotKeyRedirector("Ctrl+F3");
				e.Handled = true;
			}
			else if ((int)e.Key == 94 && (int)Keyboard.Modifiers == 2)
			{
				HotKeyRedirector("Ctrl+F5");
				e.Handled = true;
			}
			else if ((int)e.Key == 36 && (int)Keyboard.Modifiers == 2)
			{
				HotKeyRedirector("Ctrl+2");
				e.Handled = true;
			}
			else if ((int)e.Key == 37 && (int)Keyboard.Modifiers == 2)
			{
				HotKeyRedirector("Ctrl+3");
				e.Handled = true;
			}
			else if ((int)e.Key == 38 && (int)Keyboard.Modifiers == 2)
			{
				HotKeyRedirector("Ctrl+4");
				e.Handled = true;
			}
			else if ((int)e.Key == 39 && (int)Keyboard.Modifiers == 2)
			{
				HotKeyRedirector("Ctrl+5");
				e.Handled = true;
			}
			else if ((int)e.Key == 40 && (int)Keyboard.Modifiers == 2)
			{
				HotKeyRedirector("Ctrl+6");
				e.Handled = true;
			}
			else if ((int)e.Key == 41 && (int)Keyboard.Modifiers == 2)
			{
				HotKeyRedirector("Ctrl+7");
				e.Handled = true;
			}
			else if ((int)e.Key == 42 && (int)Keyboard.Modifiers == 2)
			{
				HotKeyRedirector("Ctrl+8");
				e.Handled = true;
			}
			else if ((int)e.Key == 43 && (int)Keyboard.Modifiers == 2)
			{
				HotKeyRedirector("Ctrl+9");
				e.Handled = true;
			}
			else if ((int)e.Key == 91 && (int)Keyboard.Modifiers == 4)
			{
				HotKeyRedirector("Shift+F2");
				e.Handled = true;
			}
			else if ((int)e.Key == 92 && (int)Keyboard.Modifiers == 4)
			{
				HotKeyRedirector("Shift+F3");
				e.Handled = true;
			}
			else if ((int)e.Key == 94 && (int)Keyboard.Modifiers == 4)
			{
				HotKeyRedirector("Shift+F5");
				e.Handled = true;
			}
			else if ((int)e.Key == 95 && (int)Keyboard.Modifiers == 4)
			{
				HotKeyRedirector("Shift+F6");
				e.Handled = true;
			}
			else if ((int)e.Key == 96 && (int)Keyboard.Modifiers == 4)
			{
				HotKeyRedirector("Shift+F7");
				e.Handled = true;
			}
			else if ((int)e.Key == 100 && (int)Keyboard.Modifiers == 4)
			{
				HotKeyRedirector("Shift+F11");
				e.Handled = true;
			}
		}
	}

	private void OnPlus()
	{
		IncreaseDecreaseRowQty();
	}

	private void OnMinus()
	{
		IncreaseDecreaseRowQty(increase: false);
	}

	private MTRetFromCustSpecificationRow AddItemToList(MaterialCacheRow materialCacheRow, string value, bool barCode = true, decimal retailPrice = 0m, string qntUnit = "")
	{
		MTRetFromCustSpecificationRow mtBillReturnGridRow = null;
		int count = _mtBillReturnList.Count;
		int num = ((!(_rowAddMode == "3")) ? AddNewRowOrMergeInner(materialCacheRow, value, ref mtBillReturnGridRow, barCode, retailPrice, _rowAddMode == "2", qntUnit) : AddNewRowInner(materialCacheRow, value, ref mtBillReturnGridRow, barCode, retailPrice, qntUnit));
		if (num == -1)
		{
			return null;
		}
		_mtBillReturnTableView.Grid.CurrentItem = mtBillReturnGridRow;
		bool flag = count != _mtBillReturnList.Count;
		if (string.IsNullOrWhiteSpace(_priceListType) || (flag && (!_retail || mtBillReturnGridRow.ItemType == GoodsTypesSE.Service.Value)))
		{
			CalculateGridRowSummaAndDiscountSumma(mtBillReturnGridRow, mtBillReturnGridRow.Price == 0m);
		}
		else
		{
			CalculateGridRowSummaAndDiscountSumma(mtBillReturnGridRow, calcDiscountPrice: false);
		}
		_mtBillReturnGrid.Grid.RefreshRow(num);
		_mtBillReturnGrid.ConfigureButtons();
		CalculateDocumentSumma();
		_inputValue.Text = "";
		ControlsHelper.SetFocus(_inputValue);
		_rowInfoChange?.Invoke(mtBillReturnGridRow);
		return mtBillReturnGridRow;
	}

	private void AddItemToListByProperty1(int goodID)
	{
		MTRetFromCustSpecificationRow mtBillReturnGridRow = null;
		MaterialCacheRow itemById = TradeSettings.TradeCacheService.Material.GetItemById(goodID);
		if (itemById != null)
		{
			int num = AddNewRowInner(itemById, "", ref mtBillReturnGridRow, isBarCode: false);
			if (num != -1)
			{
				_mtBillReturnTableView.Grid.CurrentItem = mtBillReturnGridRow;
				CalculateGridRowSummaAndDiscountSumma(mtBillReturnGridRow, calcDiscountPrice: false);
				_mtBillReturnGrid.Grid.RefreshRow(num);
				_mtBillReturnGrid.ConfigureButtons();
				CalculateDocumentSumma();
				_inputValue.Text = "";
				ControlsHelper.SetFocus(_inputValue);
				_rowInfoChange?.Invoke(mtBillReturnGridRow);
			}
		}
	}

	private int AddNewRowInner(MaterialCacheRow materialCacheRow, string value, ref MTRetFromCustSpecificationRow mtBillReturnGridRow, bool isBarCode = true, decimal retailPrice = 0m, string qntUnit = "")
	{
		string qntUnit2 = "";
		string text = null;
		if (materialCacheRow == null)
		{
			_inputValue.Text = "";
			ControlsHelper.SetFocus(_inputValue);
			return -1;
		}
		if (isBarCode)
		{
			if (TradeSettings.TradeCacheService.Material.GetItemByBarCode(value, out qntUnit2) != null)
			{
				text = value;
			}
		}
		else
		{
			qntUnit2 = (string.IsNullOrWhiteSpace(qntUnit) ? materialCacheRow.Unit : qntUnit);
			text = TradeSettings.TradeCacheService.Material.GetBarCode(materialCacheRow.Id, qntUnit2)?.BarCode;
		}
		decimal price = 0m;
		if (!_retail || materialCacheRow.Type == GoodsTypesSE.Service.Value)
		{
			price = (_applyLastPrice ? ArmSoft.SME.Trade.UI.Helpers.PriceHelper.GetPriceCalcForUnitByPriceList(_priceListType, _vatType, materialCacheRow.MTCode, qntUnit2, _date.EditValue.Value, _currCode.Code) : ArmSoft.SME.Trade.UI.Helpers.PriceHelper.GetPriceForGivenPriceListAndVATType(_priceListType, _vatType, materialCacheRow.Id, qntUnit2, _date.EditValue.Value, _currCode.Code));
			price = Methods.RoundPrice(price, _priceListTypeRoundDgr);
		}
		else
		{
			MaterialCacheRow.QntUnitRow qntUnit3 = TradeSettings.TradeCacheService.Material.GetQntUnit(materialCacheRow.Id, qntUnit2);
			if (qntUnit3 != null)
			{
				if (retailPrice == 0m)
				{
					MTRetailQtyAndPrice mTRetailQtyAndPrice = new MTRetailQtyAndPrice(materialCacheRow.MTCode, qntUnit3.Coef, _storage.Code, _date.EditValue.Value);
					mTRetailQtyAndPrice.LoadData();
					if (mTRetailQtyAndPrice.RowsCount == 1)
					{
						price = decimal.Parse(mTRetailQtyAndPrice.CommentMember(0));
					}
					else if (mTRetailQtyAndPrice.RowsCount > 1)
					{
						ModalBrowser modalBrowser = new ModalBrowser(mTRetailQtyAndPrice)
						{
							Owner = this,
							WindowStartupLocation = WindowStartupLocation.CenterOwner
						};
						if (!modalBrowser.Show())
						{
							_inputValue.Text = "";
							ControlsHelper.SetFocus(_inputValue);
							return -1;
						}
						price = decimal.Parse(mTRetailQtyAndPrice.CommentMember(modalBrowser.SelectedRowIndex));
					}
				}
				else
				{
					price = retailPrice;
				}
			}
		}
		mtBillReturnGridRow = new MTRetFromCustSpecificationRow(this);
		if (text != null)
		{
			mtBillReturnGridRow.BarCode = text;
		}
		FillMTBillReturnGridRow(mtBillReturnGridRow, materialCacheRow, qntUnit2, price);
		mtBillReturnGridRow.RowID = _maxRowID;
		_mtBillReturnList.Add(mtBillReturnGridRow);
		_maxRowID++;
		return _mtBillReturnGrid.Grid.GetRowHandleByListIndex(_mtBillReturnList.Count - 1);
	}

	private int AddNewRowOrMergeInner(MaterialCacheRow materialCacheRow, string value, ref MTRetFromCustSpecificationRow mtBillReturnGridRow, bool barCode = true, decimal retailPrice = 0m, bool forFocusedRow = false, string qntUnit = "")
	{
		decimal num = 0m;
		int result = -1;
		MTRetFromCustSpecificationRow mTRetFromCustSpecificationRow = null;
		if (forFocusedRow && CalcMTBillReturnListRowCount(includeDeleted: true) != 0)
		{
			mTRetFromCustSpecificationRow = (MTRetFromCustSpecificationRow)_mtBillReturnTableView.Grid.CurrentItem;
			if (mTRetFromCustSpecificationRow == null)
			{
				return -1;
			}
		}
		for (int i = 0; i < _mtBillReturnList.Count; i++)
		{
			if ((forFocusedRow && mTRetFromCustSpecificationRow.RowID != _mtBillReturnList[i].RowID) || _mtBillReturnList[i].IsDeleted || ((!barCode || !(_mtBillReturnList[i].BarCode == value)) && (barCode || !(_mtBillReturnList[i].ItemCode == value) || (!string.IsNullOrWhiteSpace(qntUnit) && !(_mtBillReturnList[i].UnitCode == qntUnit)))))
			{
				continue;
			}
			if (!_retail || materialCacheRow.Type == GoodsTypesSE.Service.Value)
			{
				_mtBillReturnList[i].Quantity += _mQty_Weight;
				mtBillReturnGridRow = _mtBillReturnList[i];
				result = _mtBillReturnGrid.Grid.GetRowHandleByListIndex(i);
				break;
			}
			MaterialCacheRow.QntUnitRow qntUnit2 = TradeSettings.TradeCacheService.Material.GetQntUnit(materialCacheRow.Id, materialCacheRow.Unit);
			if (qntUnit2 == null)
			{
				break;
			}
			if (retailPrice == 0m)
			{
				MTRetailQtyAndPrice mTRetailQtyAndPrice = new MTRetailQtyAndPrice(materialCacheRow.MTCode, qntUnit2.Coef, _storage.Code, _date.EditValue.Value);
				mTRetailQtyAndPrice.LoadData();
				if (mTRetailQtyAndPrice.RowsCount == 1)
				{
					num = decimal.Parse(mTRetailQtyAndPrice.CommentMember(0));
				}
				else if (mTRetailQtyAndPrice.RowsCount > 1)
				{
					ModalBrowser modalBrowser = new ModalBrowser(mTRetailQtyAndPrice)
					{
						Owner = this,
						WindowStartupLocation = WindowStartupLocation.CenterOwner
					};
					if (!modalBrowser.Show())
					{
						_inputValue.Text = "";
						ControlsHelper.SetFocus(_inputValue);
						return -1;
					}
					num = decimal.Parse(mTRetailQtyAndPrice.CommentMember(modalBrowser.SelectedRowIndex));
				}
			}
			else
			{
				num = retailPrice;
			}
			if (num == _mtBillReturnList[i].Price)
			{
				_mtBillReturnList[i].Quantity += _mQty_Weight;
				mtBillReturnGridRow = _mtBillReturnList[i];
				result = _mtBillReturnGrid.Grid.GetRowHandleByListIndex(i);
				break;
			}
			for (int j = 0; j < _mtBillReturnList.Count; j++)
			{
				if (_mtBillReturnList[j].ItemID == materialCacheRow.Id && _mtBillReturnList[j].UnitCode == materialCacheRow.Unit && _mtBillReturnList[j].Price == num)
				{
					_mtBillReturnList[j].Quantity += _mQty_Weight;
					mtBillReturnGridRow = _mtBillReturnList[j];
					result = _mtBillReturnGrid.Grid.GetRowHandleByListIndex(j);
					break;
				}
			}
			if (mtBillReturnGridRow == null)
			{
				mtBillReturnGridRow = new MTRetFromCustSpecificationRow(this);
				FillMTBillReturnGridRow(mtBillReturnGridRow, materialCacheRow, materialCacheRow.Unit, num);
				MaterialCacheRow.BarCodeRow barCode2 = TradeSettings.TradeCacheService.Material.GetBarCode(materialCacheRow.Id, materialCacheRow.Unit);
				if (barCode2 != null)
				{
					mtBillReturnGridRow.BarCode = barCode2.BarCode;
				}
				mtBillReturnGridRow.RowID = _maxRowID;
				_mtBillReturnList.Add(mtBillReturnGridRow);
				_maxRowID++;
				result = _mtBillReturnGrid.Grid.GetRowHandleByListIndex(_mtBillReturnList.Count - 1);
			}
			break;
		}
		if (mtBillReturnGridRow == null || mtBillReturnGridRow.IsDeleted)
		{
			result = AddNewRowInner(materialCacheRow, value, ref mtBillReturnGridRow, barCode, retailPrice, qntUnit);
		}
		return result;
	}

	private void AddNewMTFromFile(MaterialCacheRow materialCacheRow, string unitCode, decimal quantity)
	{
		decimal price = 0m;
		if (!_retail)
		{
			price = ArmSoft.SME.Trade.UI.Helpers.PriceHelper.GetPriceForGivenPriceListAndVATType(_priceListType, _vatType, materialCacheRow.Id, unitCode, _date.EditValue.Value, _currCode.Code);
			price = Methods.RoundPrice(price, _priceListTypeRoundDgr);
		}
		else
		{
			MaterialCacheRow.QntUnitRow qntUnit = TradeSettings.TradeCacheService.Material.GetQntUnit(materialCacheRow.Id, unitCode);
			if (qntUnit != null)
			{
				MTRetailQtyAndPriceDataProvider.Param param = new MTRetailQtyAndPriceDataProvider.Param
				{
					MTCode = materialCacheRow.MTCode,
					UnitCoef = qntUnit.Coef,
					Storage = _storage.Code,
					Date = _date.EditValue
				};
				List<MTRetailQtyAndPriceDataProvider.DataRow> rows = new MTRetailQtyAndPriceDataProvider(TradeSettings.CreateTradeClient()).GetRows(param);
				if (rows.Count > 0)
				{
					price = Methods.RoundPrice(rows[0].Price, _priceListTypeRoundDgr);
				}
			}
		}
		MTRetFromCustSpecificationRow mtBillReturnGridRow = AddMTToListNewRow(materialCacheRow, unitCode, quantity, price);
		int rowHandleByListIndex = _mtBillReturnGrid.Grid.GetRowHandleByListIndex(_mtBillReturnList.Count - 1);
		CalculateGridRowSummaAndDiscountSumma(mtBillReturnGridRow, calcDiscountPrice: false);
		_mtBillReturnGrid.Grid.RefreshRow(rowHandleByListIndex);
		POWindow<MTBillReturn>.SelectCellInGrid(_mtBillReturnTableView, _mtBillReturnGrid, rowHandleByListIndex, "Quantity");
		_mtBillReturnGrid.ConfigureButtons();
	}

	private MTRetFromCustSpecificationRow AddMTToListNewRow(MaterialCacheRow materialCacheRow, string unitCode, decimal quantity, decimal price)
	{
		MTRetFromCustSpecificationRow mTRetFromCustSpecificationRow = new MTRetFromCustSpecificationRow(this);
		FillMTBillReturnGridRow(mTRetFromCustSpecificationRow, materialCacheRow, unitCode, price, quantity);
		MaterialCacheRow.BarCodeRow barCode = TradeSettings.TradeCacheService.Material.GetBarCode(materialCacheRow.Id, unitCode);
		if (barCode != null)
		{
			mTRetFromCustSpecificationRow.BarCode = barCode.BarCode;
		}
		mTRetFromCustSpecificationRow.RowID = _maxRowID;
		_mtBillReturnList.Add(mTRetFromCustSpecificationRow);
		_maxRowID++;
		return mTRetFromCustSpecificationRow;
	}

	protected override void OnF2()
	{
		HotKeyRedirector("F2");
	}

	protected override void OnF3()
	{
		HotKeyRedirector("F3");
	}

	protected override void OnF4()
	{
		HotKeyRedirector("F4");
	}

	protected override void OnF6()
	{
		HotKeyRedirector("F6");
	}

	protected override void OnF7()
	{
		HotKeyRedirector("F7");
	}

	protected override void OnF8()
	{
		HotKeyRedirector("F8");
	}

	protected override void OnF9()
	{
		HotKeyRedirector("F9");
	}

	protected override void OnF11()
	{
		HotKeyRedirector("F11");
	}

	protected override void OnShiftF8()
	{
		HotKeyRedirector("Shift+F8");
	}

	protected override void OnShiftF9()
	{
		HotKeyRedirector("Shift+F9");
	}

	protected override void OnCtrlF9()
	{
		HotKeyRedirector("Ctrl+F9");
	}

	protected override void OnCtrlF11()
	{
		HotKeyRedirector("Ctrl+F11");
	}

	protected override void OnCtrl1()
	{
		HotKeyRedirector("Ctrl+1");
	}

	private void HotKeyRedirector(string hotKey)
	{
		if (hotKey == Settings.ParametersService.MTBillInputValueHotKey())
		{
			if (!_inputValue.IsReadOnly)
			{
				ControlsHelper.SetFocus(_inputValue);
			}
		}
		else if (hotKey == Settings.ParametersService.MTBillSummaCashHotKey())
		{
			if (_isCalcDiscountAuto)
			{
				On_SummaCashClick();
			}
			else if (!_summaCash.IsReadOnly)
			{
				ControlsHelper.SetFocus(_summaCash);
			}
		}
		else if (hotKey == Settings.ParametersService.MTBillSetFocusOnGridHotKey())
		{
			ControlsHelper.SetFocus(_mtBillReturnGrid.Grid);
		}
		else if (hotKey == Settings.ParametersService.MTBillBarCodeEnterHotKey())
		{
			On_BarCodeEnterClick();
		}
		else if (hotKey == Settings.ParametersService.MTBillCodeEnterHotKey())
		{
			On_CodeEnterClick();
		}
		else if (hotKey == Settings.ParametersService.MTBillMTListLikeNameHotKey())
		{
			On_MTListLikeNameClick();
		}
		else if (hotKey == Settings.ParametersService.MTBillSalePriceHotKey())
		{
			On_SalePriceClick();
		}
		else if (hotKey == Settings.ParametersService.MTBillUsePrePaymentHotKey())
		{
			On_UsePrePaymentClick();
		}
		else if (hotKey == Settings.ParametersService.MTBillPlCardPaymentHotKey())
		{
			On_PlCardPaymentClick();
		}
		else if (hotKey == Settings.ParametersService.MTBillCreditHotKey())
		{
			On_CreditClick();
		}
		else if (hotKey == Settings.ParametersService.MTBillPrintECRCheckHotKey())
		{
			On_PrintECRCheckClick();
		}
		else if (hotKey == Settings.ParametersService.MTBillPrintBillHotKey())
		{
			On_PrintMTBillReturnClick();
		}
		else if (hotKey == Settings.ParametersService.MTBillWeightHotKey())
		{
			On_WeightClick();
		}
		else if (hotKey == Settings.ParametersService.MTBillPricesHotKey())
		{
			On_PricesClick();
		}
		else if (hotKey == Settings.ParametersService.MTBillQtyEnterHotKey())
		{
			On_QtyEnterClick();
		}
		else if (hotKey == Settings.ParametersService.MTBillFavoritesHotKey())
		{
			On_MTFavoritesClick();
		}
		else if (hotKey == Settings.ParametersService.MTBillErasePaymentsHotKey())
		{
			On_ErasePaymentsClick();
		}
	}

	private void On_CodeEnterClick()
	{
		if (_codeEnter == null || !_codeEnter.IsEnabled)
		{
			return;
		}
		_inputValue.DoValidate();
		_mQty_Weight = 1m;
		string text = _inputValue.Text.Trim();
		if (string.IsNullOrWhiteSpace(text))
		{
			if (!_date.DoValidate() || !_date.EditValue.HasValue)
			{
				ControlsHelper.SetFocus(_date);
				throw new ASException(ArmSoft.SME.Common.Resources.ValidateEmptyDate);
			}
			ArmSoft.SME.Trade.UI.DropDown.GoodsAvailabilityFilterForInvoices goodsAvailabilityFilterForInvoices = new ArmSoft.SME.Trade.UI.DropDown.GoodsAvailabilityFilterForInvoices(_date.EditValue.Value, _storage.Code, _priceListType, _retail);
			ModalBrowser modalBrowser = new ModalBrowser(goodsAvailabilityFilterForInvoices)
			{
				Owner = this,
				WindowStartupLocation = WindowStartupLocation.CenterOwner
			};
			if (!modalBrowser.Show())
			{
				_inputValue.Text = "";
				ControlsHelper.SetFocus(_inputValue);
				return;
			}
			text = goodsAvailabilityFilterForInvoices.ValueMember(modalBrowser.SelectedRowIndex);
		}
		if (Settings.ParametersService.UsePartnerEmployeeSearchNewMode())
		{
			if (ArmSoft.SME.Trade.UI.Helpers.DocumentHelpers.IsValidPartnerEmployeeCardCode(text, Settings.ParametersService.PartnerEmployeeCardCodePrefixes(), Settings.ParametersService.UsePartnerCardLenght(), Settings.ParametersService.PartnerCardLenght()))
			{
				_inputValue.Text = "";
				_inputValue.Focus();
				ArmSoft.SME.Trade.UI.Helpers.DocumentHelpers.GetPartnerEmployeeDataByBarCode(text, out var partnerCacheRow, out var partnerContract, out var employee, "AMD");
				FillPartnerEmployeeData(partnerCacheRow, partnerContract, employee);
				return;
			}
		}
		else if (MCRInfo.GetInstance().IsValidCard(text))
		{
			_inputValue.Text = "";
			_inputValue.Focus();
			ArmSoft.SME.Trade.UI.Helpers.DocumentHelpers.GetPartnerEmployeeDataByBarCode(MCRInfo.GetInstance().GetCardNumber(text), out var partnerCacheRow2, out var partnerContract2, out var employee2, "AMD");
			FillPartnerEmployeeData(partnerCacheRow2, partnerContract2, employee2);
			return;
		}
		if (text.Length > 20)
		{
			throw new ASException(string.Format(ArmSoft.SME.Trade.Common.Resources.ErrMTCodeLen, 20, text));
		}
		MaterialCacheRow item = TradeSettings.TradeCacheService.Material.GetItem(text);
		if (item == null)
		{
			ControlsHelper.SetFocus(_inputValue);
			throw new ASException(string.Format(ArmSoft.SME.Trade.Common.Resources.MaterialNotExist, text));
		}
		AddItemToList(item, text, barCode: false);
	}

	private void On_BarCodeEnterClick()
	{
		if (_barCodeEnter == null || !_barCodeEnter.IsEnabled)
		{
			return;
		}
		_inputValue.DoValidate();
		_mQty_Weight = 1m;
		string barCode = _inputValue.Text.Trim();
		if (!_date.DoValidate() || !_date.EditValue.HasValue)
		{
			ControlsHelper.SetFocus(_date);
			throw new ASException(ArmSoft.SME.Common.Resources.ValidateEmptyDate);
		}
		if (string.IsNullOrWhiteSpace(barCode))
		{
			ArmSoft.SME.Trade.UI.DropDown.MTBarCodes mTBarCodes = new ArmSoft.SME.Trade.UI.DropDown.MTBarCodes(isShowQntUnitCaption: false, showBarCodeFirst: true, Settings.ParametersService.ShowMTInclInPrList(), showAlsoMTQntUnitsEmptyBarCodes: true);
			ModalBrowser modalBrowser = new ModalBrowser(mTBarCodes)
			{
				Owner = this,
				WindowStartupLocation = WindowStartupLocation.CenterOwner
			};
			if (!modalBrowser.Show())
			{
				_inputValue.Text = "";
				ControlsHelper.SetFocus(_inputValue);
				return;
			}
			barCode = mTBarCodes.ValueMember(modalBrowser.SelectedRowIndex);
			string text = mTBarCodes.CommentMember(modalBrowser.SelectedRowIndex);
			if (string.IsNullOrWhiteSpace(barCode))
			{
				AddItemToList(TradeSettings.TradeCacheService.Material.GetItem(text), text, barCode: false, 0m, mTBarCodes.GetRowByIndex(modalBrowser.SelectedRowIndex).QntUnit);
				return;
			}
		}
		if (Settings.ParametersService.UsePartnerEmployeeSearchNewMode())
		{
			if (ArmSoft.SME.Trade.UI.Helpers.DocumentHelpers.IsValidPartnerEmployeeCardCode(barCode, Settings.ParametersService.PartnerEmployeeCardCodePrefixes(), Settings.ParametersService.UsePartnerCardLenght(), Settings.ParametersService.PartnerCardLenght()))
			{
				_inputValue.Text = "";
				_inputValue.Focus();
				ArmSoft.SME.Trade.UI.Helpers.DocumentHelpers.GetPartnerEmployeeDataByBarCode(barCode, out var partnerCacheRow, out var partnerContract, out var employee, "AMD");
				FillPartnerEmployeeData(partnerCacheRow, partnerContract, employee);
				return;
			}
		}
		else if (MCRInfo.GetInstance().IsValidCard(barCode))
		{
			_inputValue.Text = "";
			_inputValue.Focus();
			ArmSoft.SME.Trade.UI.Helpers.DocumentHelpers.GetPartnerEmployeeDataByBarCode(MCRInfo.GetInstance().GetCardNumber(barCode), out var partnerCacheRow2, out var partnerContract2, out var employee2, "AMD");
			FillPartnerEmployeeData(partnerCacheRow2, partnerContract2, employee2);
			return;
		}
		if (_searchBarCodeInProperty1)
		{
			BarCodeEnterClickOnProperty1(barCode);
			return;
		}
		MaterialCacheRow materialCacheRow = ArmSoft.SME.Trade.UI.Helpers.DocumentHelpers.TryGetItemByBarCode(ref barCode, out _mQty_Weight, out var _, out var _);
		if (materialCacheRow == null)
		{
			ControlsHelper.SetFocus(_inputValue);
			throw new ASException(string.Format(ArmSoft.SME.Trade.Common.Resources.BarCodeNotExist, barCode));
		}
		AddItemToList(materialCacheRow, barCode);
	}

	private void BarCodeEnterClickOnProperty1(string barCode)
	{
		if (string.IsNullOrWhiteSpace(barCode))
		{
			return;
		}
		GoodsPartiesAvailibilityListByProperty1DataProvider.Param param = new GoodsPartiesAvailibilityListByProperty1DataProvider.Param
		{
			PartiesStorage = _storage.Code,
			PartiesProperty1Value = barCode,
			PartiesDate = _date.EditValue.Value
		};
		List<GoodsPartiesAvailibilityListByProperty1DataProvider.DataRow> rows = new GoodsPartiesAvailibilityListByProperty1DataProvider(TradeSettings.CreateTradeClient()).GetRows(param);
		if (rows.Count == 0)
		{
			ControlsHelper.SetFocus(_inputValue);
			throw new ASException(string.Format(ArmSoft.SME.Trade.Common.Resources.NoAvailablePartiesWithBarcodeInProperty1, barCode));
		}
		if (rows.Count == 1)
		{
			GoodsPartiesAvailibilityListByProperty1DataProvider.DataRow row = rows.First();
			AddGoodByProperty1ToMTExpDocSelectedPartiesListAndItemList(row);
			return;
		}
		GoodsPartiesAvailabilityListByProperty1 goodsPartiesAvailabilityListByProperty = new GoodsPartiesAvailabilityListByProperty1(_storage.Code, barCode, _date.EditValue.Value);
		ModalBrowser modalBrowser = new ModalBrowser(goodsPartiesAvailabilityListByProperty)
		{
			Owner = this,
			WindowStartupLocation = WindowStartupLocation.CenterOwner
		};
		if (modalBrowser.Show())
		{
			string value = goodsPartiesAvailabilityListByProperty.ValueMember(modalBrowser.SelectedRowIndex);
			GoodsPartiesAvailibilityListByProperty1DataProvider.DataRow rowByValue = goodsPartiesAvailabilityListByProperty.GetRowByValue(value);
			if (!string.IsNullOrWhiteSpace(value))
			{
				AddGoodByProperty1ToMTExpDocSelectedPartiesListAndItemList(rowByValue);
			}
		}
		else
		{
			_inputValue.Text = "";
			ControlsHelper.SetFocus(_inputValue);
		}
	}

	private void AddGoodByProperty1ToMTExpDocSelectedPartiesListAndItemList(GoodsPartiesAvailibilityListByProperty1DataProvider.DataRow row)
	{
		bool flag = TradeSettings.CreateTradeClient().CommonServices.IsNegativExpAllow(_storage.Code);
		PartyRow partyRow = new PartyRow
		{
			GoodID = row.GoodID,
			GridRow = _maxRowID,
			Base = row.PartyBase,
			Row = row.PartyRow,
			Qty = row.Quantity,
			ExpQty = 1m,
			Comment = row.Comment,
			Cost = row.Cost
		};
		if (partyRow.ExpQty == partyRow.Qty)
		{
			partyRow.ExpSumma = partyRow.Summa;
		}
		else if (!flag)
		{
			partyRow.ExpSumma = (partyRow.Cost * partyRow.ExpQty).RoundAs(2);
		}
		else
		{
			partyRow.ExpSumma = (partyRow.Cost * ((partyRow.ExpQty < partyRow.Qty) ? partyRow.ExpQty : partyRow.Qty)).RoundAs(2);
		}
		partyRow.IncomeDate = row.IncomeDate.Value;
		AddItemToListByProperty1(row.GoodID);
	}

	private void On_QtyEnterClick()
	{
		if (_qtyEnter == null || !_qtyEnter.IsEnabled)
		{
			return;
		}
		_inputValue.DoValidate();
		if (_mtBillReturnTableView.Grid.CurrentItem == null)
		{
			return;
		}
		if (string.IsNullOrWhiteSpace(_inputValue.Text))
		{
			throw new ASException(ArmSoft.SME.Trade.Common.Resources.InputBoxIsEmpty);
		}
		string text = _inputValue.Text.Trim();
		bool flag = text.StartsWith("/");
		if (flag)
		{
			text = text.Substring("/".Length);
		}
		if (!text.IsNumeric())
		{
			throw new ASException(ArmSoft.SME.Trade.Common.Resources.IsNotNumeric);
		}
		if (!decimal.TryParse(text, out var result))
		{
			return;
		}
		decimal num = result.RoundAs(3);
		if (num < 0m)
		{
			throw new ASException(ArmSoft.SME.Trade.Common.Resources.ValidateQuantIsPositive);
		}
		if (decimal.Truncate(num).ToString().Length > 9)
		{
			throw new ASException(ArmSoft.SME.Common.Resources.ErrNumberOfDigitsBeforeDecimalPart);
		}
		MTRetFromCustSpecificationRow mTRetFromCustSpecificationRow = (MTRetFromCustSpecificationRow)_mtBillReturnTableView.Grid.CurrentItem;
		if (!_isAllowChangeGoodRowInBill && num < mTRetFromCustSpecificationRow.Quantity)
		{
			throw new ASException(ArmSoft.SME.Trade.Common.Resources.InputQtyMustBeGreater);
		}
		if (mTRetFromCustSpecificationRow.ItemType == GoodsTypesSE.Good.Value && mTRetFromCustSpecificationRow.Quantity != 0m && num != 0m)
		{
			mTRetFromCustSpecificationRow.UsedBonus = (num * mTRetFromCustSpecificationRow.UsedBonus / mTRetFromCustSpecificationRow.Quantity).RoundAs(2);
			mTRetFromCustSpecificationRow.AccumulatedBonus = (num * mTRetFromCustSpecificationRow.AccumulatedBonus / mTRetFromCustSpecificationRow.Quantity).RoundAs(2);
		}
		mTRetFromCustSpecificationRow.Quantity = num;
		MaterialCacheRow item = TradeSettings.TradeCacheService.Material.GetItem(mTRetFromCustSpecificationRow.ItemCode);
		if (item != null)
		{
			if (flag)
			{
				string text2 = "";
				if (mTRetFromCustSpecificationRow.UnitCode == item.AltUnit)
				{
					text2 = item.Unit;
				}
				else if (mTRetFromCustSpecificationRow.UnitCode == item.Unit)
				{
					text2 = item.AltUnit;
				}
				UnitChanged(text2, mTRetFromCustSpecificationRow.UnitCode, mTRetFromCustSpecificationRow);
				mTRetFromCustSpecificationRow.UnitCode = text2;
			}
			else
			{
				CalculateGridRowSummaAndDiscountSumma(mTRetFromCustSpecificationRow, calcDiscountPrice: false);
			}
		}
		CalculateDocumentSumma();
		_rowInfoChange?.Invoke(mTRetFromCustSpecificationRow);
		_inputValue.Text = "";
		ControlsHelper.SetFocus(_inputValue);
	}

	private void On_SalePriceClick()
	{
		if (!_isAllowChangeGoodRowInBill)
		{
			throw new ASException(ArmSoft.SME.Common.Resources.AccessDenied, 20008);
		}
		_inputValue.DoValidate();
		if (_mtBillReturnTableView.Grid.CurrentItem == null)
		{
			return;
		}
		if (string.IsNullOrWhiteSpace(_inputValue.Text))
		{
			throw new ASException(ArmSoft.SME.Trade.Common.Resources.InputBoxIsEmpty);
		}
		if (!_inputValue.Text.Trim().IsNumeric())
		{
			throw new ASException(ArmSoft.SME.Trade.Common.Resources.IsNotNumeric);
		}
		if (!decimal.TryParse(_inputValue.Text.Trim(), out var result))
		{
			return;
		}
		decimal price = result.RoundAs(2);
		if (price < 0m)
		{
			throw new ASException(ArmSoft.SME.Trade.Common.Resources.ValidatePriceIsPositive);
		}
		if (decimal.Truncate(price).ToString().Length > 9)
		{
			throw new ASException(ArmSoft.SME.Common.Resources.ErrNumberOfDigitsBeforeDecimalPart);
		}
		MTRetFromCustSpecificationRow mTRetFromCustSpecificationRow = (MTRetFromCustSpecificationRow)_mtBillReturnTableView.Grid.CurrentItem;
		if (!_retail || mTRetFromCustSpecificationRow.ItemType == GoodsTypesSE.Service.Value)
		{
			mTRetFromCustSpecificationRow.Price = price;
			if (TradeSettings.TradeCacheService.Material.GetItem(mTRetFromCustSpecificationRow.ItemCode) != null)
			{
				CalculateGridRowSummaAndDiscountSumma(mTRetFromCustSpecificationRow, calcDiscountPrice: false);
			}
		}
		else
		{
			MaterialCacheRow.QntUnitRow qntUnit = TradeSettings.TradeCacheService.Material.GetQntUnit(mTRetFromCustSpecificationRow.ItemID, mTRetFromCustSpecificationRow.UnitCode);
			if (qntUnit == null)
			{
				return;
			}
			MTRetailQtyAndPriceDataProvider.Param param = new MTRetailQtyAndPriceDataProvider.Param
			{
				MTCode = mTRetFromCustSpecificationRow.ItemCode,
				Storage = _storage.Code,
				Date = _date.EditValue,
				UnitCoef = qntUnit.Coef
			};
			if (new MTRetailQtyAndPriceDataProvider(TradeSettings.CreateTradeClient()).GetRows(param).Any((MTRetailQtyAndPriceDataProvider.DataRow x) => x.Price == price))
			{
				mTRetFromCustSpecificationRow.Price = price;
				if (TradeSettings.TradeCacheService.Material.GetItem(mTRetFromCustSpecificationRow.ItemCode) != null)
				{
					CalculateGridRowSummaAndDiscountSumma(mTRetFromCustSpecificationRow, calcDiscountPrice: false);
				}
			}
			else
			{
				MTRetailQtyAndPrice mTRetailQtyAndPrice = new MTRetailQtyAndPrice(mTRetFromCustSpecificationRow.ItemCode, qntUnit.Coef, _storage.Code, _date.EditValue.Value);
				mTRetailQtyAndPrice.LoadData();
				ModalBrowser modalBrowser = new ModalBrowser(mTRetailQtyAndPrice)
				{
					Owner = this,
					WindowStartupLocation = WindowStartupLocation.CenterOwner
				};
				if (!modalBrowser.Show())
				{
					_inputValue.Text = "";
					ControlsHelper.SetFocus(_inputValue);
					return;
				}
				price = decimal.Parse(mTRetailQtyAndPrice.CommentMember(modalBrowser.SelectedRowIndex));
				mTRetFromCustSpecificationRow.Price = price;
			}
		}
		CalculateDocumentSumma();
		_rowInfoChange?.Invoke(mTRetFromCustSpecificationRow);
		_inputValue.Text = "";
		ControlsHelper.SetFocus(_inputValue);
	}

	private void On_WeightClick()
	{
		if (_weight != null && _weight.IsEnabled && _mtBillReturnTableView.Grid.CurrentItem != null)
		{
			decimal weight = ScaleConnector.GetWeight();
			MTRetFromCustSpecificationRow mTRetFromCustSpecificationRow = (MTRetFromCustSpecificationRow)_mtBillReturnTableView.Grid.CurrentItem;
			if (mTRetFromCustSpecificationRow != null && mTRetFromCustSpecificationRow.UnitCode == WeightItemInfo.GetInstance().Unit)
			{
				mTRetFromCustSpecificationRow.Quantity = weight;
				mTRetFromCustSpecificationRow.Summa = (mTRetFromCustSpecificationRow.Quantity * mTRetFromCustSpecificationRow.Price * (100m - mTRetFromCustSpecificationRow.Discount) / 100m).RoundAs(2);
				mTRetFromCustSpecificationRow.DiscountSumma = (mTRetFromCustSpecificationRow.Quantity * mTRetFromCustSpecificationRow.Price - mTRetFromCustSpecificationRow.Summa).RoundAs(2);
				CalculateDocumentSumma();
				_rowInfoChange?.Invoke(mTRetFromCustSpecificationRow);
			}
		}
	}

	private void On_MTListLikeNameClick()
	{
		if (_mtListLikeName == null || !_mtListLikeName.IsEnabled)
		{
			return;
		}
		_inputValue.DoValidate();
		_mQty_Weight = 1m;
		string likeName = _inputValue.Text.Trim();
		if (!_date.DoValidate() || !_date.EditValue.HasValue)
		{
			ControlsHelper.SetFocus(_date);
			throw new ASException(ArmSoft.SME.Common.Resources.ValidateEmptyDate);
		}
		ArmSoft.SME.Trade.UI.DropDown.GoodsAvailabilityFilterForInvoices goodsAvailabilityFilterForInvoices = new ArmSoft.SME.Trade.UI.DropDown.GoodsAvailabilityFilterForInvoices(_date.EditValue.Value, _storage.Code, _priceListType, _retail, likeName);
		ModalBrowser modalBrowser = new ModalBrowser(goodsAvailabilityFilterForInvoices)
		{
			Owner = this,
			WindowStartupLocation = WindowStartupLocation.CenterOwner
		};
		if (modalBrowser.Show())
		{
			string text = goodsAvailabilityFilterForInvoices.ValueMember(modalBrowser.SelectedRowIndex);
			MaterialCacheRow item = TradeSettings.TradeCacheService.Material.GetItem(text);
			if (item == null)
			{
				ControlsHelper.SetFocus(_inputValue);
				throw new ASException(string.Format(ArmSoft.SME.Trade.Common.Resources.MaterialNotExist, text));
			}
			AddItemToList(item, text, barCode: false);
		}
		else
		{
			_inputValue.Text = "";
			ControlsHelper.SetFocus(_inputValue);
		}
	}

	private void On_PlCardPaymentClick()
	{
		if (_plCardPayment == null || !_plCardPayment.IsEnabled)
		{
			return;
		}
		decimal num = 0m;
		decimal num2 = ((_summaPlCard.Value != 0m) ? _summaPlCard.Value : RemainSumForCardPayment());
		if (_currCode.Code != "AMD")
		{
			ExchangeRate exchangeRates = TradeSettings.CreateTradeClient().CommonServices.GetExchangeRates(_currCode.Code, _date.EditValue.Value);
			num2 = (num2 * exchangeRates.Rate / exchangeRates.Base).RoundAs(2);
		}
		if (_readOnlyAfterCheckPrint)
		{
			if (_currCode.Code != "AMD")
			{
				ExchangeRate exchangeRates = TradeSettings.CreateTradeClient().CommonServices.GetExchangeRates(_currCode.Code, _date.EditValue.Value);
				num = (_summaPlCard.Value * exchangeRates.Rate / exchangeRates.Base).RoundAs(2);
			}
			else
			{
				num = _summaPlCard.Value;
			}
		}
		decimal plCardPaySum = (_readOnlyAfterCheckPrint ? num : num2);
		NonCashPaymentDialog nonCashPaymentDialog = new NonCashPaymentDialog(this, plCardPaySum, RemainSumForCardPayment(), _nonCashPaymentMethod, _readOnlyAfterCheckPrint, PO.BankAcquiring.PartnerCode, PO.BankAcquiring.ContractCode, _cashDesk.Code, PO.POSTerminalType, PO.POSTerminalAddress);
		if (nonCashPaymentDialog.ShowDialog().Value)
		{
			decimal num3;
			if (_currCode.Code != "AMD")
			{
				ExchangeRate exchangeRates = TradeSettings.CreateTradeClient().CommonServices.GetExchangeRates(_currCode.Code, _date.EditValue.Value);
				num3 = (nonCashPaymentDialog.EnteredNonCashSum * exchangeRates.Base / exchangeRates.Rate).RoundAs(2);
			}
			else
			{
				num3 = nonCashPaymentDialog.EnteredNonCashSum;
			}
			_summaPlCard.EditValue = num3;
			UpdateChangeOrPaySum();
			_nonCashPaymentMethod = nonCashPaymentDialog.NonCashPaymentMethod;
			PO.BankAcquiring.PartnerCode = nonCashPaymentDialog.EnteredBankAcquiringPartnerCode;
			PO.BankAcquiring.ContractCode = nonCashPaymentDialog.EnteredBankAcquiringContractCode;
			PO.POSTerminalType = nonCashPaymentDialog.POSTerminalType;
			PO.POSTerminalAddress = nonCashPaymentDialog.POSTerminalAddress;
		}
		if (_summaPlCard.Value != 0m)
		{
			((LayoutItemWithRequired)(object)_summaPlCard.Parent).Visibility = Visibility.Visible;
		}
		else
		{
			((LayoutItemWithRequired)(object)_summaPlCard.Parent).Visibility = Visibility.Collapsed;
		}
	}

	private void On_PricesClick()
	{
		if (_prices != null && _prices.IsEnabled)
		{
			PricesDialog pricesDialog = new PricesDialog(this, _vatType, _priceListType, _priceListTypeRoundDgr, "", _retail, _useDiscount, _allowChangePriceType, _allowChangeDiscountPrices, AllowChangeDiscountPriceType: false, !_readOnlyAfterCheckPrint);
			if (pricesDialog.ShowDialog().Value)
			{
				_priceListType = pricesDialog.PriceListType;
				_priceListTypeRoundDgr = pricesDialog.PriceListTypeRoundDgr;
				_vatType = pricesDialog.VATType;
				PriceListTypeEditValueChanged(_priceListType);
			}
			UpdateChangeOrPaySum();
		}
	}

	private void On_UsePrePaymentClick()
	{
		if (_usePrePayment == null || !_usePrePayment.IsEnabled)
		{
			return;
		}
		decimal num = 0m;
		decimal num2 = ((!(_summaPrePayment.Value != 0m)) ? 0m : _summaPrePayment.Value);
		if (_currCode.Code != "AMD")
		{
			ExchangeRate exchangeRates = TradeSettings.CreateTradeClient().CommonServices.GetExchangeRates(_currCode.Code, _date.EditValue.Value);
			num2 = (num2 * exchangeRates.Rate / exchangeRates.Base).RoundAs(2);
		}
		if (_readOnlyAfterCheckPrint)
		{
			if (_currCode.Code != "AMD")
			{
				ExchangeRate exchangeRates = TradeSettings.CreateTradeClient().CommonServices.GetExchangeRates(_currCode.Code, _date.EditValue.Value);
				num = (_summaPrePayment.Value * exchangeRates.Rate / exchangeRates.Base).RoundAs(2);
			}
			else
			{
				num = _summaPrePayment.Value;
			}
		}
		decimal prePaymentSum = (_readOnlyAfterCheckPrint ? num : num2);
		UsePrepaymentDialog usePrepaymentDialog = new UsePrepaymentDialog(this, prePaymentSum, RemainSumForPrePayment(), _partner.Code, _contract, _readOnlyAfterCheckPrint, _prepaymentPartnerCode, _prepaymentContractCode, _summaChange.Value);
		if (usePrepaymentDialog.ShowDialog().Value)
		{
			if (_currCode.Code != "AMD")
			{
				ExchangeRate exchangeRates = TradeSettings.CreateTradeClient().CommonServices.GetExchangeRates(_currCode.Code, _date.EditValue.Value);
				_summaPrePayment.EditValue = (usePrepaymentDialog.EnteredPrePaymentSum * exchangeRates.Base / exchangeRates.Rate).RoundAs(2);
			}
			else
			{
				_summaPrePayment.EditValue = usePrepaymentDialog.EnteredPrePaymentSum;
			}
			_prepaymentPartnerCode = usePrepaymentDialog.PrepaymentPartnerCode;
			_prepaymentContractCode = usePrepaymentDialog.PrepaymentContractCode;
			UpdateChangeOrPaySum();
		}
		if (_summaPrePayment.Value != 0m)
		{
			((LayoutItemWithRequired)(object)_summaPrePayment.Parent).Visibility = Visibility.Visible;
		}
		else
		{
			((LayoutItemWithRequired)(object)_summaPrePayment.Parent).Visibility = Visibility.Collapsed;
		}
	}

	private void On_ErasePaymentsClick()
	{
		if (_erasePayments != null && _erasePayments.IsEnabled)
		{
			_summaCash.Value = 0m;
			_summaCredit.Value = 0m;
			_summaPlCard.Value = 0m;
			_nonCashPaymentMethod = 0;
			_summaPrePayment.Value = 0m;
			((LayoutItemWithRequired)(object)_summaPrePayment.Parent).Visibility = Visibility.Collapsed;
			((LayoutItemWithRequired)(object)_summaPlCard.Parent).Visibility = Visibility.Collapsed;
			((LayoutItemWithRequired)(object)_summaCredit.Parent).Visibility = Visibility.Collapsed;
			UpdateChangeOrPaySum();
		}
	}

	private void On_SummaCashClick()
	{
		if (_summaPayCash == null || !_summaPayCash.IsEnabled)
		{
			return;
		}
		decimal num = 0m;
		decimal num2 = ((!(_summaCash.Value != 0m)) ? RemainSumForCashPayment() : _summaCash.Value);
		if (_currCode.Code != "AMD")
		{
			ExchangeRate exchangeRates = TradeSettings.CreateTradeClient().CommonServices.GetExchangeRates(_currCode.Code, _date.EditValue.Value);
			num2 = (num2 * exchangeRates.Rate / exchangeRates.Base).RoundAs(2);
		}
		if (_readOnlyAfterCheckPrint)
		{
			if (_currCode.Code != "AMD")
			{
				ExchangeRate exchangeRates = TradeSettings.CreateTradeClient().CommonServices.GetExchangeRates(_currCode.Code, _date.EditValue.Value);
				num = (_summaCash.Value * exchangeRates.Rate / exchangeRates.Base).RoundAs(2);
			}
			else
			{
				num = _summaCash.Value;
			}
		}
		decimal cashSum = (_readOnlyAfterCheckPrint ? num : num2);
		CashPaymentDialog cashPaymentDialog = new CashPaymentDialog(this, cashSum, RemainSumForCashPayment(), _readOnlyAfterCheckPrint);
		if (cashPaymentDialog.ShowDialog().Value)
		{
			if (_currCode.Code != "AMD")
			{
				ExchangeRate exchangeRates = TradeSettings.CreateTradeClient().CommonServices.GetExchangeRates(_currCode.Code, _date.EditValue.Value);
				_summaCash.EditValue = (cashPaymentDialog.EnteredCashSum * exchangeRates.Base / exchangeRates.Rate).RoundAs(2);
			}
			else
			{
				_summaCash.EditValue = cashPaymentDialog.EnteredCashSum;
			}
			UpdateChangeOrPaySum();
		}
	}

	private void On_CreditClick()
	{
		if (_credit == null || !_credit.IsEnabled)
		{
			return;
		}
		decimal num = 0m;
		if ((string.IsNullOrWhiteSpace(_partner.Code) || !_partner.DoValidate()) && !_readOnlyAfterCheckPrint)
		{
			ControlsHelper.SetFocus(_partner);
			throw new ASException(ArmSoft.SME.Trade.Common.Resources.CreditSaleBuyer);
		}
		decimal num2 = ((!(_summaCredit.Value != 0m)) ? RemainSumForCreditPayment() : _summaCredit.Value);
		if (_currCode.Code != "AMD")
		{
			ExchangeRate exchangeRates = TradeSettings.CreateTradeClient().CommonServices.GetExchangeRates(_currCode.Code, _date.EditValue.Value);
			num2 = (num2 * exchangeRates.Rate / exchangeRates.Base).RoundAs(2);
		}
		if (_readOnlyAfterCheckPrint)
		{
			if (_currCode.Code != "AMD")
			{
				ExchangeRate exchangeRates = TradeSettings.CreateTradeClient().CommonServices.GetExchangeRates(_currCode.Code, _date.EditValue.Value);
				num = (_summaCredit.Value * exchangeRates.Rate / exchangeRates.Base).RoundAs(2);
			}
			else
			{
				num = _summaCredit.Value;
			}
		}
		decimal creditSum = (_readOnlyAfterCheckPrint ? num : num2);
		CreditSaleDialog creditSaleDialog = new CreditSaleDialog(this, creditSum, RemainSumForCreditPayment(), _partner.Code, _contract, _readOnlyAfterCheckPrint);
		if (creditSaleDialog.ShowDialog().Value)
		{
			if (_currCode.Code != "AMD")
			{
				ExchangeRate exchangeRates = TradeSettings.CreateTradeClient().CommonServices.GetExchangeRates(_currCode.Code, _date.EditValue.Value);
				_summaCredit.EditValue = (creditSaleDialog.EnteredCreditSum * exchangeRates.Base / exchangeRates.Rate).RoundAs(2);
			}
			else
			{
				_summaCredit.EditValue = creditSaleDialog.EnteredCreditSum;
			}
			UpdateChangeOrPaySum();
		}
		if (_summaCredit.Value != 0m)
		{
			((LayoutItemWithRequired)(object)_summaCredit.Parent).Visibility = Visibility.Visible;
		}
		else
		{
			((LayoutItemWithRequired)(object)_summaCredit.Parent).Visibility = Visibility.Collapsed;
		}
	}

	private void AddFromFavorites(string itemCode, string itemUnitCode)
	{
		MaterialCacheRow item = TradeSettings.TradeCacheService.Material.GetItem(itemCode);
		if (item == null)
		{
			ControlsHelper.SetFocus(_mtBillReturnGrid.Grid);
			throw new ASException(string.Format(ArmSoft.SME.Trade.Common.Resources.MaterialNotExist, itemCode));
		}
		AddItemToList(item, itemCode, barCode: false, 0m, itemUnitCode);
	}

	private void On_MTFavoritesClick()
	{
		if (_mtFavorites != null && _mtFavorites.IsEnabled)
		{
			if (!TradeSettings.CreateTradeClient().CommonServices.FavoriteMTsExists(Settings.LoginService.UserId))
			{
				ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Trade.Common.Resources.MTFavoriteInstruction, ArmSoft.SME.Common.Resources.RibbonPageFavorites, MessageBoxButton.OK, MessageBoxImage.Asterisk);
				return;
			}
			_mQty_Weight = 1m;
			MTFavoritesBrowser mTFavoritesBrowser = new MTFavoritesBrowser(new ArmSoft.SME.Trade.UI.DropDown.MTFavorites(Settings.LoginService.UserId, onlyMaterials: false, onlyGroups: false, _priceListType, _date.EditValue.Value, _vatType, _currCode.Code), AddFromFavorites, _priceListType, _date.EditValue.Value, showTree: false, showCardView: true);
			mTFavoritesBrowser.Owner = this;
			mTFavoritesBrowser.WindowStartupLocation = WindowStartupLocation.CenterOwner;
			mTFavoritesBrowser.Show();
		}
	}

	private void On_PrintMTBillReturnClick()
	{
		if (_printBill != null && _printBill.IsEnabled)
		{
			PrintMTBillReturnClick();
		}
	}

	private void PrintMTBillReturnClick()
	{
		if (!PO.StoredInDb || PO.State == (byte)DocumentStateSE.Draft || PO.State == (byte)DocumentStateSE.Deleted)
		{
			PO.PartnerID = -1;
			FillDocument(new StoreRequest());
		}
		if (!Settings.ParametersService.PrintMTCheckAndNew() || (PO.StoredInDb && !IsChanged) || SaveDocument())
		{
			PrintMTBillReturn();
			if (Settings.ParametersService.PrintMTCheckAndNew())
			{
				OnNewAfterSaveUpper();
			}
		}
	}

	private void PrintMTBillReturn()
	{
		FillDocument(new StoreRequest());
		string text = Settings.ParametersService.RetCheckTemplate();
		if (string.IsNullOrWhiteSpace(text))
		{
			text = Settings.CoreCacheService.Template.GetDefaultPrintTemplate(short.Parse(PrintTemplateDocumentTypeSE.MTBill.Value), throwExceptionNotExits: true).Name;
		}
		Settings.ServiceProvider.GetRequiredService<ITemplateSubstitutionService>().PrintPreview(PO, PrintMode.PrintDirect, text, CheckPrinterInfo.GetInstance().CheckPrinterName);
	}

	private void VATTypeValidate(string priceListRowVATType)
	{
		decimal num = 0m;
		decimal num2 = 0m;
		if (_mtBillReturnList.Count <= 0)
		{
			return;
		}
		switch (priceListRowVATType)
		{
		case "1":
			foreach (MTRetFromCustSpecificationRow mtBillReturn in _mtBillReturnList)
			{
				num += ArmSoft.SME.Trade.UI.Helpers.PriceHelper.CalcVATAmountByType(mtBillReturn.Summa, "1", mtBillReturn.VAT.Value);
				if (!_retail)
				{
					if (_vatType == "2" || _vatType == "5")
					{
						mtBillReturn.BeginPrice += ArmSoft.SME.Trade.UI.Helpers.PriceHelper.CalcVATAmountByType(mtBillReturn.BeginPrice, "1", mtBillReturn.VAT.Value);
						mtBillReturn.BeginPrice = Methods.RoundPrice(mtBillReturn.BeginPrice, _priceListTypeRoundDgr);
						mtBillReturn.Price += ArmSoft.SME.Trade.UI.Helpers.PriceHelper.CalcVATAmountByType(mtBillReturn.Price, "1", mtBillReturn.VAT.Value);
						mtBillReturn.Price = Methods.RoundPrice(mtBillReturn.Price, _priceListTypeRoundDgr);
					}
					mtBillReturn.BeginSumma = (mtBillReturn.Quantity * mtBillReturn.BeginPrice).RoundAs(2);
					mtBillReturn.Summa = (mtBillReturn.Quantity * mtBillReturn.Price).RoundAs(2);
				}
				num2 += mtBillReturn.Summa;
			}
			_summa.EditValue = num2;
			break;
		case "2":
		case "5":
			foreach (MTRetFromCustSpecificationRow mtBillReturn2 in _mtBillReturnList)
			{
				num2 += mtBillReturn2.Summa;
				if (!_retail)
				{
					if (_vatType.InList("1", "3", "4"))
					{
						mtBillReturn2.Price -= ArmSoft.SME.Trade.UI.Helpers.PriceHelper.CalcVATAmountByType(mtBillReturn2.Price, "5", mtBillReturn2.VAT.Value);
						mtBillReturn2.Price = Methods.RoundPrice(mtBillReturn2.Price, _priceListTypeRoundDgr);
					}
					mtBillReturn2.Summa = (mtBillReturn2.Quantity * mtBillReturn2.Price).RoundAs(2);
				}
			}
			_summa.EditValue = num2;
			break;
		}
	}

	private bool PriceListTypeEditValueChanged(string priceListType)
	{
		bool flag = false;
		if (!string.IsNullOrWhiteSpace(priceListType))
		{
			if (_summa.Value != 0m)
			{
				if (ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Trade.Common.Resources.ValidateChangePrices, Settings.ProgramName, MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No) == MessageBoxResult.Yes)
				{
					flag = true;
				}
			}
			else
			{
				flag = true;
			}
		}
		if (flag)
		{
			if (!_date.DoValidate() || !_date.EditValue.HasValue)
			{
				ControlsHelper.SetFocus(_date);
				throw new ASException(ArmSoft.SME.Common.Resources.ValidateEmptyDate);
			}
			ArmSoft.SME.Trade.UI.Helpers.PriceHelper.GetPriceListRowVATType(priceListType, out var priceListRowVATType, out var priceListRowCurr);
			ExchangeRate currRate = new ExchangeRate();
			if (_currCode.Code != "AMD")
			{
				currRate = TradeSettings.CreateTradeClient().CommonServices.GetExchangeRates(_currCode.Code, _date.EditValue.Value);
			}
			string priceTypeRoungDgr = TradeSettings.CreateTradeClient().CommonServices.GetPriceTypeRoungDgr(priceListType);
			UpdatePrices(_date.EditValue.Value, priceListRowCurr, priceTypeRoungDgr, currRate);
			if (priceListRowVATType != Methods.VATAndRetailVATMapping(_vatType, oppositeMapping: true))
			{
				VATTypeValidate(priceListRowVATType);
			}
			foreach (MTRetFromCustSpecificationRow mtBillReturn in _mtBillReturnList)
			{
				CalculateGridRowSummaAndDiscountSumma(mtBillReturn);
			}
			CalcSummAndSummVAT();
		}
		return flag;
	}

	private void UpdatePrices(DateOnly docDate, string priceTypeCurr, string priceTypeRoundDegree, ExchangeRate currRate)
	{
		if (_mtBillReturnList.Count == 0)
		{
			return;
		}
		ExchangeRate exchangeRate = new ExchangeRate();
		if (priceTypeCurr != _currCode.Code && priceTypeCurr != "AMD")
		{
			exchangeRate = TradeSettings.CreateTradeClient().CommonServices.GetExchangeRates(priceTypeCurr, docDate);
		}
		foreach (MTRetFromCustSpecificationRow mtBillReturn in _mtBillReturnList)
		{
			PricesDataProvider.DataRow price = TradeSettings.CreateTradeClient().CommonServices.GetPrice(_priceListType, mtBillReturn.ItemID, mtBillReturn.UnitCode, docDate);
			if (priceTypeCurr == _currCode.Code)
			{
				mtBillReturn.BeginPrice = price.Price;
			}
			else if (priceTypeCurr == "AMD")
			{
				mtBillReturn.BeginPrice = (price.Price / currRate.Rate * currRate.Base).RoundAs(2);
			}
			else
			{
				mtBillReturn.BeginPrice = (price.Price * exchangeRate.Rate / exchangeRate.Base / currRate.Rate * currRate.Base).RoundAs(2);
			}
			mtBillReturn.BeginPrice = Methods.RoundPrice(mtBillReturn.BeginPrice, priceTypeRoundDegree);
			mtBillReturn.Price = mtBillReturn.BeginPrice;
			mtBillReturn.BeginSumma = Methods.RoundPrice(mtBillReturn.BeginPrice * mtBillReturn.Quantity, priceTypeRoundDegree);
		}
	}

	private void FillMTBillReturnGridRow(MTRetFromCustSpecificationRow mtBillReturnGridRow, MaterialCacheRow materialCacheRow, string unitCode, decimal price, decimal quantity = -1m)
	{
		mtBillReturnGridRow.ItemType = materialCacheRow.Type;
		if (materialCacheRow.Type == GoodsTypesSE.Good.Value)
		{
			mtBillReturnGridRow.Storage = _storage.Code;
		}
		else
		{
			mtBillReturnGridRow.Storage = "";
		}
		mtBillReturnGridRow.ItemID = materialCacheRow.Id;
		mtBillReturnGridRow.ItemCode = materialCacheRow.MTCode;
		mtBillReturnGridRow.ItemName = materialCacheRow.Caption;
		QntUnitCacheRow item = TradeSettings.TradeCacheService.QntUnit.GetItem(unitCode);
		mtBillReturnGridRow.UnitCode = unitCode;
		mtBillReturnGridRow.UnitBrief = ((item == null) ? "" : item.Brief);
		mtBillReturnGridRow.VAT = materialCacheRow.VAT;
		mtBillReturnGridRow.Quantity = ((quantity != -1m) ? quantity : _mQty_Weight);
		mtBillReturnGridRow.Price = price;
		if (_useDiscount)
		{
			mtBillReturnGridRow.Discount = 0m;
		}
	}

	private void Storage_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		if (!_storage.IsReadOnly && !string.IsNullOrWhiteSpace((string)e.NewValue) && TradeSettings.TradeCacheService.Storage.GetItem((string)e.NewValue) != null)
		{
			if (!TradeSettings.CreateTradeClient().CommonServices.IsStorageAvailable((string)e.NewValue))
			{
				e.Handled = true;
				_storage.Code = (string)e.OldValue;
				ArmSoft.SME.Core.UI.MessageBox.Show(string.Format(ArmSoft.SME.Trade.Common.Resources.StorageNotAvailForUser, Settings.LoginService.UserName, (string)e.NewValue), Settings.ProgramName, MessageBoxButton.OK, MessageBoxImage.Exclamation);
				return;
			}
			if (_mtBillReturnList.Count != 0 && !_isShowStorageColInMTBill)
			{
				foreach (MTRetFromCustSpecificationRow mtBillReturn in _mtBillReturnList)
				{
					if (mtBillReturn.ItemType == GoodsTypesSE.Good.Value)
					{
						mtBillReturn.Storage = (string)e.NewValue;
					}
				}
			}
		}
		if (!(((string)e.NewValue).TrimEnd() != ((string)e.OldValue).TrimEnd()) || !_storage.DoValidate())
		{
			return;
		}
		_retail = TradeSettings.TradeCacheService.Storage.GetItem(_storage.Code)?.Retail ?? false;
		if (_retail)
		{
			_priceListTypeRoundDgr = "";
		}
		else
		{
			_priceListTypeRoundDgr = TradeSettings.CreateTradeClient().CommonServices.GetPriceTypeRoungDgr(_priceListType);
		}
		if (_retail)
		{
			_priceCol.AllowEditing = DefaultBoolean.False;
		}
		else if (_allowChangeSalePrices)
		{
			_priceCol.AllowEditing = DefaultBoolean.True;
		}
		if (_mtBillReturnList.Count != 0)
		{
			for (int i = 0; i <= _mtBillReturnList.Count - 1; i++)
			{
				if (_mtBillReturnList[i].ItemType == GoodsTypesSE.Good.Value)
				{
					int rowHandleByListIndex = _mtBillReturnGrid.Grid.GetRowHandleByListIndex(i);
					_mtBillReturnGrid.Grid.RefreshRow(rowHandleByListIndex);
				}
			}
		}
		if (!_useDiscount)
		{
			if (!_allowChangeSalePrices)
			{
				_priceCol.AllowEditing = DefaultBoolean.False;
				_priceCol.AllowFocus = false;
				_summaCol.AllowEditing = DefaultBoolean.False;
				_summaCol.AllowFocus = false;
			}
			else if (_retail)
			{
				_priceCol.AllowEditing = DefaultBoolean.False;
				_priceCol.AllowFocus = true;
				_summaCol.AllowEditing = DefaultBoolean.False;
				_summaCol.AllowFocus = false;
			}
			else
			{
				_priceCol.AllowEditing = DefaultBoolean.True;
				_priceCol.AllowFocus = true;
				_summaCol.AllowEditing = DefaultBoolean.True;
				_summaCol.AllowFocus = true;
			}
		}
		else if (!_allowChangeSalePrices && !_allowChangeDiscountPrices)
		{
			_priceCol.AllowEditing = DefaultBoolean.False;
			_priceCol.AllowFocus = false;
			_discountCol.AllowEditing = DefaultBoolean.False;
			_discountCol.AllowFocus = false;
			_discountPriceCol.AllowEditing = DefaultBoolean.False;
			_discountPriceCol.AllowFocus = false;
			_summaCol.AllowEditing = DefaultBoolean.False;
			_summaCol.AllowFocus = false;
		}
		else if (_allowChangeSalePrices && !_allowChangeDiscountPrices)
		{
			if (_retail)
			{
				_priceCol.AllowEditing = DefaultBoolean.False;
			}
			else
			{
				_priceCol.AllowEditing = DefaultBoolean.True;
			}
			_priceCol.AllowFocus = true;
			_discountCol.AllowEditing = DefaultBoolean.False;
			_discountCol.AllowFocus = false;
			_discountPriceCol.AllowEditing = DefaultBoolean.False;
			_discountPriceCol.AllowFocus = false;
			_summaCol.AllowEditing = DefaultBoolean.False;
			_summaCol.AllowFocus = false;
		}
		else if (!_allowChangeSalePrices && _allowChangeDiscountPrices)
		{
			_priceCol.AllowEditing = DefaultBoolean.False;
			_priceCol.AllowFocus = false;
			_discountCol.AllowEditing = DefaultBoolean.True;
			_discountCol.AllowFocus = true;
			_discountPriceCol.AllowEditing = DefaultBoolean.True;
			_discountPriceCol.AllowFocus = true;
			_summaCol.AllowEditing = DefaultBoolean.True;
			_summaCol.AllowFocus = true;
		}
		else
		{
			if (_retail)
			{
				_priceCol.AllowEditing = DefaultBoolean.False;
			}
			else
			{
				_priceCol.AllowEditing = DefaultBoolean.True;
			}
			_priceCol.AllowFocus = true;
			_discountCol.AllowEditing = DefaultBoolean.True;
			_discountCol.AllowFocus = true;
			_discountPriceCol.AllowEditing = DefaultBoolean.True;
			_discountPriceCol.AllowFocus = true;
			_summaCol.AllowEditing = DefaultBoolean.True;
			_summaCol.AllowFocus = true;
		}
	}

	private void FillPartnerEmployeeData(PartnerCacheRow partnerCacheRow, PartnerCacheRow.ContractRow contractRow, EmployeeCacheRow employeeCacheRow)
	{
		if (partnerCacheRow != null)
		{
			_contract = contractRow.Contract;
			_contractCurrency = contractRow.CurrCode;
			_bPartReadMagneticCard = true;
			_partner.Code = partnerCacheRow.PartCode;
		}
		else
		{
			_salesConsultant.Code = employeeCacheRow.EmployeeCode;
		}
	}

	private void IncreaseDecreaseRowQty(bool increase = true)
	{
		if (!_mtBillReturnGrid.AllowEdit && (_isAllowChangeGoodRowInBill || increase))
		{
			throw new ASException(ArmSoft.SME.Common.Resources.AccessDenied, 20008);
		}
		if (_mtBillReturnTableView.Grid.CurrentItem == null)
		{
			return;
		}
		MTRetFromCustSpecificationRow mTRetFromCustSpecificationRow = (MTRetFromCustSpecificationRow)_mtBillReturnTableView.Grid.CurrentItem;
		if (mTRetFromCustSpecificationRow == null)
		{
			return;
		}
		if (!increase)
		{
			if (mTRetFromCustSpecificationRow.Quantity > 1m)
			{
				if (mTRetFromCustSpecificationRow.ItemType == GoodsTypesSE.Good.Value && mTRetFromCustSpecificationRow.Quantity != 0m && mTRetFromCustSpecificationRow.Quantity - 1m != 0m)
				{
					mTRetFromCustSpecificationRow.UsedBonus = ((mTRetFromCustSpecificationRow.Quantity - 1m) * mTRetFromCustSpecificationRow.UsedBonus / mTRetFromCustSpecificationRow.Quantity).RoundAs(2);
					mTRetFromCustSpecificationRow.AccumulatedBonus = ((mTRetFromCustSpecificationRow.Quantity - 1m) * mTRetFromCustSpecificationRow.AccumulatedBonus / mTRetFromCustSpecificationRow.Quantity).RoundAs(2);
				}
				mTRetFromCustSpecificationRow.Quantity -= 1m;
				mTRetFromCustSpecificationRow.Summa = (mTRetFromCustSpecificationRow.Quantity * mTRetFromCustSpecificationRow.DiscountPrice).RoundAs(2);
				mTRetFromCustSpecificationRow.DiscountSumma = (mTRetFromCustSpecificationRow.Quantity * mTRetFromCustSpecificationRow.Price - mTRetFromCustSpecificationRow.Summa).RoundAs(2);
			}
			else if (_isAllowChangeGoodRowInBill)
			{
				_mtBillReturnList.Remove(mTRetFromCustSpecificationRow);
				mTRetFromCustSpecificationRow = null;
				_rowInfoChange?.Invoke(null);
			}
			else if (!mTRetFromCustSpecificationRow.IsDeleted)
			{
				mTRetFromCustSpecificationRow.IsDeleted = true;
				mTRetFromCustSpecificationRow.AccumulatedBonus = 0m;
				mTRetFromCustSpecificationRow.UsedBonus = 0m;
			}
		}
		else
		{
			mTRetFromCustSpecificationRow.Quantity += 1m;
			mTRetFromCustSpecificationRow.Summa = (mTRetFromCustSpecificationRow.Quantity * mTRetFromCustSpecificationRow.DiscountPrice).RoundAs(2);
			mTRetFromCustSpecificationRow.DiscountSumma = (mTRetFromCustSpecificationRow.Quantity * mTRetFromCustSpecificationRow.Price - mTRetFromCustSpecificationRow.Summa).RoundAs(2);
		}
		ChangeMarkingStatus(mTRetFromCustSpecificationRow);
		_mtBillReturnTableView.CommitEditing();
		CalculateDocumentSumma();
		_rowInfoChange?.Invoke(mTRetFromCustSpecificationRow);
	}

	private void ChangeMarkingStatus(MTRetFromCustSpecificationRow row)
	{
		if (row != null)
		{
			if (MarkingHelpers.IsGoodMarkable(row.ItemCode, _useMarking, row.MarkingList.Count != 0, _date.EditValue.Value, out var isRowMarkable))
			{
				row.MarkingStatus = MarkingHelper.GetMarkingStatus(row.MarkingList.Count, row.Quantity);
			}
			else
			{
				row.MarkingStatus = "";
			}
			row.IsRowMarkable = isRowMarkable;
		}
	}

	protected override void FillDocument(StoreRequest sr)
	{
		base.FillDocument(sr);
		PO.VATType = Methods.VATAndRetailVATMapping(_vatType, oppositeMapping: false);
		PO.PartnerContractGroup.PartnerCode = _partner.Code;
		PO.PartnerContractGroup.ContractCode = _contract;
		PO.PrepaymentGroup.PartnerCode = _prepaymentPartnerCode;
		PO.PrepaymentGroup.ContractCode = _prepaymentContractCode;
		PO.PriceType = _priceListType;
		PO.ECRReturnCheckNum = _ecrCheckNum.Text;
		PO.ECRBaseCheckNum = _baseECRCheckNum;
		PO.BasePrePaymentECRCheckNum = _basePrePaymentECRCheckNum;
		PO.ECRReturnByCashSum = _summaCash.Value - _summaChange.Value;
		PO.SummaChange = 0m;
		PO.NonCashPaymentMethod = _nonCashPaymentMethod;
		PO.BaseISN = BaseISN;
		PO.MTList.Clear();
		PO.UsedBonusSum = _usedBonus;
		PO.AccumulatedBonusSum = _accumulatedBonus;
		foreach (MTRetFromCustSpecificationRow mtBillReturn in _mtBillReturnList)
		{
			mtBillReturn.Price = mtBillReturn.DiscountPrice;
			PO.MTList.Add(new MTReturnFromCustomerBase.MTRetFromCustSpecificationRow
			{
				AccumulatedBonus = mtBillReturn.AccumulatedBonus,
				BarCode = mtBillReturn.BarCode,
				BaseRowID = mtBillReturn.BaseRowID,
				BeginPrice = mtBillReturn.BeginPrice,
				BeginSumma = mtBillReturn.BeginSumma,
				CostPrice = mtBillReturn.CostPrice,
				CostPriceVAT = mtBillReturn.CostPriceVAT,
				Country = mtBillReturn.Country,
				CurrCostPrice = mtBillReturn.CurrCostPrice,
				Discount = mtBillReturn.Discount,
				DiscountPrice = mtBillReturn.DiscountPrice,
				DiscountSumma = mtBillReturn.DiscountSumma,
				EnvFeePercent = mtBillReturn.EnvFeePercent,
				EnvFeeSumma = mtBillReturn.EnvFeeSumma,
				ExternalCode = mtBillReturn.ExternalCode,
				FullName = mtBillReturn.FullName,
				IsDeleted = mtBillReturn.IsDeleted,
				IsGift = mtBillReturn.IsGift,
				IsRowMarkable = mtBillReturn.IsRowMarkable,
				IsStorageRequired = mtBillReturn.IsStorageRequired,
				ItemCode = mtBillReturn.ItemCode,
				ItemID = mtBillReturn.ItemID,
				ItemName = mtBillReturn.ItemName,
				ItemType = mtBillReturn.ItemType,
				MarkingList = mtBillReturn.MarkingList,
				MarkingStatus = mtBillReturn.MarkingStatus,
				PartyBase = mtBillReturn.partyBase,
				PartyRow = mtBillReturn.partyRow,
				Price = mtBillReturn.Price,
				Producer = mtBillReturn.Producer,
				ProducerCaption = mtBillReturn.ProducerCaption,
				Property1 = mtBillReturn.Property1,
				Property1Caption = mtBillReturn.Property1Caption,
				Property2 = mtBillReturn.Property2,
				Property2Caption = mtBillReturn.Property2Caption,
				Quantity = mtBillReturn.Quantity,
				RowDescription = mtBillReturn.RowDescription,
				RowID = mtBillReturn.RowID,
				RowNum = mtBillReturn.RowNum,
				RowSpecField = mtBillReturn.RowSpecField,
				ShortName = mtBillReturn.ShortName,
				Storage = mtBillReturn.Storage,
				Summa = mtBillReturn.Summa,
				UnitBrief = mtBillReturn.UnitBrief,
				UnitCode = mtBillReturn.UnitCode,
				UsedBonus = mtBillReturn.UsedBonus,
				UseLife = mtBillReturn.useLife,
				VAT = mtBillReturn.VAT,
				IsPartySelected = mtBillReturn.IsPartySelected
			});
		}
		PO.MaxRowID = _maxRowID;
	}

	protected override bool BeforeStore(StoreRequest sr)
	{
		if (sr.State != DocumentStateSE.Draft.Value)
		{
			if (_isCalcDiscountAuto && IsUnderpaid())
			{
				ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Common.Resources.PaymentsSmallerThanTotal, Settings.ProgramName, MessageBoxButton.OK, MessageBoxImage.Hand);
				return false;
			}
			if (_nonCashPaymentMethod != 0 && _summaPlCard.Value == 0m)
			{
				ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Trade.Common.Resources.WrongNonCashSum, Settings.ProgramName, MessageBoxButton.OK, MessageBoxImage.Hand);
				return false;
			}
			if (!string.IsNullOrWhiteSpace(_partner.Code) && !string.IsNullOrWhiteSpace(_contract) && _summaCredit.Value > 0m)
			{
				PartnerCacheRow item = TradeSettings.TradeCacheService.Partner.GetItem(_partner.Code);
				int intPartID = item?.Id ?? (-1);
				PartnerCacheRow.ContractRow contract = TradeSettings.TradeCacheService.Partner.GetContract(item.PartCode, _contract);
				if (contract != null && contract.ControlReceivables && (TradeSettings.CreateTradeClient().CommonServices.GetCurrentPartnerRemByContract(intPartID, contract.Contract, "", PO.ISN).SummCurr + _summaCredit.Value).RoundAs(2) > contract.ReceivablesLimit)
				{
					if (!Settings.ParametersService.AllowExceedDebtLimit())
					{
						ArmSoft.SME.Core.UI.MessageBox.Show(string.Format(ArmSoft.SME.Trade.Common.Resources.ReceivablesExceedDenied, _contract), Settings.ProgramName, MessageBoxButton.OK, MessageBoxImage.Hand);
						return false;
					}
					if (ArmSoft.SME.Core.UI.MessageBox.Show(string.Format(ArmSoft.SME.Trade.Common.Resources.ReceivablesConfirmExceed, contract.Contract.Trim()), Settings.ProgramName, MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No) == MessageBoxResult.No)
					{
						return false;
					}
				}
			}
		}
		return true;
	}

	protected override void AfterStore(StoreRequest sr, bool isNewDocument)
	{
		base.AfterStore(sr, isNewDocument);
		PartnerCacheRow.ContractRow contract = TradeSettings.TradeCacheService.Partner.GetContract(_partCode, _contract);
		if (string.IsNullOrWhiteSpace(_partCode) || contract == null || !contract.IsBonusCard || string.IsNullOrWhiteSpace(contract.DiscountCard) || !_connectTob2barmsoftDiscountServer)
		{
			return;
		}
		if (State != DocumentStateSE.Draft.Value && State != DocumentStateSE.Deleted.Value)
		{
			if (PO.TransactionISN == Guid.Empty)
			{
				CardTransactionRequest cardTransactionRequest = new CardTransactionRequest();
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
				PO.AccumulatedBonusSum = PO.MTList.Sum((MTReturnFromCustomerBase.MTRetFromCustSpecificationRow el) => el.AccumulatedBonus).Value;
				if (PO.AccumulatedBonusSum != 0m)
				{
					cardTransactionRequest.Rows.Add(new CardTransactionRow
					{
						Op = OpType.Accumulate,
						Amount = -PO.AccumulatedBonusSum
					});
				}
				PO.UsedBonusSum = PO.MTList.Sum((MTReturnFromCustomerBase.MTRetFromCustSpecificationRow row) => row.UsedBonus).Value;
				if (PO.UsedBonusSum != 0m)
				{
					cardTransactionRequest.Rows.Add(new CardTransactionRow
					{
						Op = OpType.Use,
						Amount = PO.UsedBonusSum
					});
				}
				CardTransaction cardTransaction = DiscountServerService.RecordDiscountServerTransaction(cardTransactionRequest);
				if (cardTransaction != null)
				{
					PO.TransactionISN = cardTransaction.ISN;
					int num = 1;
					List<PropertyWithValue> list = new List<PropertyWithValue>(num);
					CollectionsMarshal.SetCount(list, num);
					CollectionsMarshal.AsSpan(list)[0] = new PropertyWithValue("TransactionISN", PO.TransactionISN.ToString());
					List<PropertyWithValue> properties = list;
					sr.LogComment = $"{ArmSoft.SME.Trade.Common.Resources.RecordInDiscountServer} - {PO.TransactionISN}";
					PO.TS = Settings.DocumentService.EditFields(PO.ISN, properties, sr);
				}
			}
		}
		else if (State == DocumentStateSE.Processed.Value)
		{
			DiscountServerService.DeleteTransactionsByISN(PO.TransactionISN);
			PO.TransactionISN = Guid.Empty;
			int num = 1;
			List<PropertyWithValue> list2 = new List<PropertyWithValue>(num);
			CollectionsMarshal.SetCount(list2, num);
			CollectionsMarshal.AsSpan(list2)[0] = new PropertyWithValue("TransactionISN", PO.TransactionISN.ToString());
			List<PropertyWithValue> properties2 = list2;
			sr.LogComment = $"{ArmSoft.SME.Trade.Common.Resources.DeleteFromDiscountServer} - {PO.TransactionISN}";
			PO.TS = Settings.DocumentService.EditFields(PO.ISN, properties2, sr);
		}
	}

	protected override void StoreSettings(RegistryKey rk)
	{
		base.StoreSettings(rk);
		rk.SetValue("SearchBarCodeInProperty1", _searchBarCodeInProperty1 ? "1" : "0");
		rk.SetValue("ImmediatlyEditRow", _immediatlyEditRow ? "1" : "0");
		rk.SetValue("ApplyLastPrice", _applyLastPrice ? "1" : "0");
		_mtBillReturnGrid.StoreSettings(rk);
	}

	protected override void RestoreSettings(double defaultWidth, double defaultHeight, RegistryKey rk)
	{
		base.RestoreSettings(defaultWidth, defaultHeight, rk);
		if (rk != null)
		{
			_searchBarCodeInProperty1 = rk.GetValue("SearchBarCodeInProperty1", false).ToString() == "1";
			_immediatlyEditRow = rk.GetValue("ImmediatlyEditRow", false).ToString() == "1";
			_applyLastPrice = rk.GetValue("ApplyLastPrice", false).ToString() == "1";
			_mtBillReturnGrid.RestoreSettings(rk);
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

	private void On_PrintECRCheckClick()
	{
		if (_printECRCheck != null && _printECRCheck.IsEnabled)
		{
			PrintReturnCheck();
		}
		if (Settings.ParametersService.PrintECRAndProductCheck() && !Settings.ParametersService.PrintECRCheckOnCheckPrinter())
		{
			PrintMTBillReturnClick();
		}
	}

	private void PrintReturnCheck()
	{
		if (_ecrCheckNum.Text == "XXXXXXXX" || _prePaymentECRCheckNum.Text == "XXXXXXXX")
		{
			ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Common.Resources.ECRMessageForXXXXXXXX, Settings.ProgramName, MessageBoxButton.OK, MessageBoxImage.Asterisk);
			return;
		}
		try
		{
			bool flag = !string.IsNullOrWhiteSpace(_ecrCheckNum.Text) && _ecrCheckNum.Text != "XXXXXXXX";
			CashCacheRow cashRow = TradeSettings.TradeCacheService.Cash.GetItem(_cashDesk.Code);
			if (string.IsNullOrWhiteSpace(_cashDesk.Code) && BaseDocument != null && BaseDocument.POType == DocumentTypeSE.MTBill.Value)
			{
				cashRow = TradeSettings.TradeCacheService.Cash.GetItem(BaseDocument.CashDesk);
			}
			if (!ECRPreliminaryChecksAndInitalization(cashRow, ((MTBill)BaseDocument).ECRCRN))
			{
				return;
			}
			_summaCashValue = _summaCash.Value;
			_summaPlCardValue = _summaPlCard.Value;
			_summaPayByBonusValue = _summaPayByBonus.Value;
			_summaCreditValue = _summaCredit.Value;
			_summaPrePaymentValue = _summaPrePayment.Value;
			_calculatedBonusValue = 0m;
			_summaCashSubtractBonus = 0m;
			_summaPlCardSubtractBonus = 0m;
			_summaPrePaymentSubtractBonus = 0m;
			bool bonus = false;
			if (BaseDocument.POType == DocumentTypeSE.MTBill.Value)
			{
				MTBill mTBill = (MTBill)BaseDocument;
				bonus = (!string.IsNullOrWhiteSpace(mTBill.PrePaymentECRCheckNum) && mTBill.PrePaymentECRCheckNum.Trim() != "XXXXXXXX") || Settings.ParametersService.ECRCheckPrintBonusType() == PrintBonusTypesSE.DiscountAndPrePayment.Value;
				if (bonus)
				{
					CalcAllSummaBasedFromCalculatedBonuses();
				}
				bonus = !string.IsNullOrWhiteSpace(mTBill.PrePaymentECRCheckNum) && mTBill.PrePaymentECRCheckNum.Trim() != "XXXXXXXX" && (_summaPlCardSubtractBonus > 0m || _summaCashSubtractBonus > 0m);
				if (mTBill.PrePaymentECRCheckNum == "XXXXXXXX")
				{
					ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Common.Resources.ECRMessageForXXXXXXXX, ArmSoft.SME.Trade.Common.Resources.ProgramName, MessageBoxButton.OK, MessageBoxImage.Asterisk);
					return;
				}
			}
			if ((PO.POSTerminalType == "1") & bonus)
			{
				throw new ASException(ArmSoft.SME.Trade.Common.Resources.CanNotPrintBonusAsPerPaymentWithECRPOS);
			}
			_clonedBillReturn = (MTBillReturn)Settings.DocumentService.Load(POISN(), withoutAccessCheck: true);
			_clonedBillReturn.BasePrePaymentECRCheckNum = PO.BasePrePaymentECRCheckNum;
			_clonedBillReturn.ECRBaseCheckNum = PO.ECRBaseCheckNum;
			foreach (MTReturnFromCustomerBase.MTRetFromCustSpecificationRow mT in _clonedBillReturn.MTList)
			{
				mT.DiscountPrice = (mT.DiscountPrice - mT.AccumulatedBonus / mT.Quantity).Value.RoundAs(2);
				mT.Discount = 100m - (mT.DiscountPrice / mT.Price * (decimal?)100).Value.RoundAs(2);
				mT.Summa = (mT.Summa - mT.AccumulatedBonus).Value.RoundAs(2);
				mT.DiscountSumma = (mT.DiscountSumma + mT.AccumulatedBonus).Value.RoundAs(2);
			}
			bool flag2;
			if (!flag)
			{
				PosTerminalResponse posTerminalResponse = RefundPOSTerminalIfChosen();
				ECRPrintCheckWindow eCRPrintCheckWindow = new ECRPrintCheckWindow(this, _ecr, cashRow.ECROperator, cashRow.ECROperatorPassword, cashRow.ShowECRTracking, CreateReturnRequest(cashRow, posTerminalResponse), ECRPrintCheckWindow.ECRCheckType.Return, (ECRPrintCheckWindow window, Utilities.PrintReceiptResponse response) =>
				{
					if (!bonus || Settings.ParametersService.PrintPrepaymentCheckSeparately())
					{
						ECRHelpers.PrintOnOtherPrinter(window, response, cashRow, bonus ? _clonedBillReturn : PO, $"{DocumentTypeSE.MTBillReturn.Value}.3");
					}
				}, (string checkNumber, string errorMessage) =>
				{
					int num2 = 1;
					List<PropertyWithValue> list2 = new List<PropertyWithValue>(num2);
					CollectionsMarshal.SetCount(list2, num2);
					CollectionsMarshal.AsSpan(list2)[0] = new PropertyWithValue("ECRCheckNum", checkNumber);
					List<PropertyWithValue> list3 = list2;
					if (bonus)
					{
						list3.Add(new PropertyWithValue("PrePaymentECRCheckNum", "XXXXXXXX"));
						_prePaymentECRCheckNum.Text = "XXXXXXXX";
						PO.PrePaymentECRCheckNum = "XXXXXXXX";
					}
					PO.TS = ECRHelpers.ChangeDocECRCheckInfo(checkNumber, errorMessage, POISN(), list3, PO.TS);
					_ecrCheckNum.Text = checkNumber;
					PO.ECRCheckNum = checkNumber;
				});
				flag2 = eCRPrintCheckWindow.Succeeded;
				if (flag2)
				{
					UpdateDocAndWindowAfterCheckPrint(eCRPrintCheckWindow.CheckNumber, forBonus: false, bonus);
				}
			}
			else
			{
				flag2 = true;
			}
			if (flag2 & bonus)
			{
				PO.PrePaymentCashOutAmount = _summaCashSubtractBonus;
				PO.PrePaymentCardOutAmount = _summaPlCardSubtractBonus;
				ECRPrintCheckWindow eCRPrintCheckWindow2 = new ECRPrintCheckWindow(this, _ecr, cashRow.ECROperator, cashRow.ECROperatorPassword, cashRow.ShowECRTracking, CreateReturnRequestForBonus(cashRow), ECRPrintCheckWindow.ECRCheckType.Return, (ECRPrintCheckWindow window, Utilities.PrintReceiptResponse response) =>
				{
					if (!Settings.ParametersService.PrintPrepaymentCheckSeparately())
					{
						_clonedBillReturn.PrePaymentECRCheckNum = PO.PrePaymentECRCheckNum;
						_clonedBillReturn.ECRCheckNum = PO.ECRCheckNum;
						_clonedBillReturn.ECRBaseCheckNum = PO.ECRBaseCheckNum + "/" + ((MTBill)BaseDocument).PrePaymentECRCheckNum;
					}
					ECRHelpers.PrintOnOtherPrinter(window, response, cashRow, Settings.ParametersService.PrintPrepaymentCheckSeparately() ? PO : _clonedBillReturn, Settings.ParametersService.PrintPrepaymentCheckSeparately() ? $"{DocumentTypeSE.MTBillReturn.Value}.5" : $"{DocumentTypeSE.MTBillReturn.Value}.3");
				}, (string checkNumber, string errorMessage) =>
				{
					int num2 = 1;
					List<PropertyWithValue> list2 = new List<PropertyWithValue>(num2);
					CollectionsMarshal.SetCount(list2, num2);
					CollectionsMarshal.AsSpan(list2)[0] = new PropertyWithValue("PrePaymentECRCheckNum", checkNumber);
					List<PropertyWithValue> editedProperties2 = list2;
					if (checkNumber != "XXXXXXXX")
					{
						PO.TS = ECRHelpers.ChangeDocECRCheckInfo(checkNumber, errorMessage, POISN(), editedProperties2, PO.TS);
						_prePaymentECRCheckNum.Text = checkNumber;
						PO.PrePaymentECRCheckNum = checkNumber;
					}
				});
				if (eCRPrintCheckWindow2.Succeeded)
				{
					UpdateDocAndWindowAfterCheckPrint(eCRPrintCheckWindow2.CheckNumber, forBonus: true);
				}
			}
			_summaCashValue = _summaCash.Value;
			_summaPlCardValue = _summaPlCard.Value;
			_summaPayByBonusValue = _summaPayByBonus.Value;
			_summaCreditValue = _summaCredit.Value;
			_summaPrePaymentValue = _summaPrePayment.Value;
			_calculatedBonusValue = 0m;
			_summaCashSubtractBonus = 0m;
			_summaPlCardSubtractBonus = 0m;
		}
		catch (Arcus2Exception ex)
		{
			int num = 1;
			List<PropertyWithValue> list = new List<PropertyWithValue>(num);
			CollectionsMarshal.SetCount(list, num);
			CollectionsMarshal.AsSpan(list)[0] = new PropertyWithValue("ECRCheckNum", "");
			List<PropertyWithValue> editedProperties = list;
			PO.TS = ECRHelpers.ChangeDocECRCheckInfo("", ex.Message, POISN(), editedProperties, PO.TS);
			_ecrCheckNum.Text = "";
			PO.ECRCheckNum = "";
			Settings.ErrorHandler(new ASException(ex.Message));
		}
		catch (Exception ex2)
		{
			Settings.ErrorHandler(ex2);
		}
	}

	private void CalcAllSummaBasedFromCalculatedBonuses()
	{
		_summaPrePaymentValue += _summaPayByBonusValue;
		_summaPayByBonusValue = 0m;
		_calculatedBonusValue += CalcBonusFromRows();
		if (_summaCashValue >= _calculatedBonusValue)
		{
			_summaCashSubtractBonus = _calculatedBonusValue;
			_summaCashValue -= _calculatedBonusValue;
			return;
		}
		_summaCashSubtractBonus = _summaCashValue;
		_calculatedBonusValue -= _summaCashValue;
		_summaCashValue = 0m;
		if (_summaPlCardValue >= _calculatedBonusValue)
		{
			_summaPlCardSubtractBonus = _calculatedBonusValue;
			_summaPlCardValue -= _calculatedBonusValue;
			return;
		}
		_summaPlCardSubtractBonus = _summaPlCardValue;
		_calculatedBonusValue -= _summaPlCardValue;
		_summaPlCardValue = 0m;
		if (_summaPrePaymentValue >= _calculatedBonusValue)
		{
			_summaPrePaymentSubtractBonus = _calculatedBonusValue;
			_summaPrePaymentValue -= _calculatedBonusValue;
		}
		else
		{
			_summaCashSubtractBonus += _calculatedBonusValue;
			_summaCreditValue -= _calculatedBonusValue;
		}
	}

	private decimal CalcBonusFromRows()
	{
		return PO.MTList.Where((MTReturnFromCustomerBase.MTRetFromCustSpecificationRow r) => !r.IsDeleted.Value).Sum((MTReturnFromCustomerBase.MTRetFromCustSpecificationRow r) => r.AccumulatedBonus.Value);
	}

	private Utilities.PrintReturnReceiptRequest CreateReturnRequest(CashCacheRow cashRow, PosTerminalResponse posTerminalResponse)
	{
		List<Utilities.ReturnItem> list = new List<Utilities.ReturnItem>();
		List<string> list2 = new List<string>();
		MTBill mTBill = (MTBill)BaseDocument;
		_baseDocList = mTBill.MTList;
		string cashDesk = BaseDocument.CashDesk;
		string eCRCRN = mTBill.ECRCRN;
		Dictionary<int, int> eCRRowNumByRowIdDictionary = ECRHelpers.GetECRRowNumByRowIdDictionary(_baseDocList);
		decimal cashAmountForReturn = 0m;
		if (Settings.ParametersService.ECRCheckPrintCreditAsCash())
		{
			cashAmountForReturn += _summaCreditValue;
		}
		foreach (MTReturnFromCustomerBase.MTRetFromCustSpecificationRow mT in PO.MTList)
		{
			if (mT.IsDeleted.Value)
			{
				continue;
			}
			Utilities.ReturnItem item = new Utilities.ReturnItem
			{
				rpid = eCRRowNumByRowIdDictionary[mT.RowID.Value],
				quantity = decimal.ToDouble(mT.Quantity.Value)
			};
			list.Add(item);
			foreach (string marking in mT.MarkingList)
			{
				list2.Add(marking);
			}
			if (!_useMarking)
			{
				continue;
			}
			MaterialCacheRow item2 = TradeSettings.TradeCacheService.Material.GetItem(mT.ItemCode);
			if (!item2.IsWeight && item2.CheckMarking)
			{
				decimal num = mT.MarkingList.Count;
				decimal? quantity = mT.Quantity;
				if (!((num == quantity.GetValueOrDefault()) & quantity.HasValue) && MarkingHelpers.IsGoodMarkable(mT.ItemCode, _useMarking, mT.MarkingList.Count != 0, _date.EditValue.Value, out var _))
				{
					throw new ASException(string.Format(ArmSoft.SME.Trade.Common.Resources.MarkingHasMismatch, mT.ItemCode));
				}
			}
		}
		cashAmountForReturn += _summaCashValue + _summaPayByBonusValue - _summaChange.Value;
		Utilities.PrintReturnReceiptRequest printReturnReceiptRequest = new Utilities.PrintReturnReceiptRequest();
		if (string.IsNullOrWhiteSpace(eCRCRN))
		{
			if (!string.IsNullOrWhiteSpace(cashDesk) && cashRow.Code.Trim() != cashDesk.Trim())
			{
				CashCacheRow item3 = TradeSettings.TradeCacheService.Cash.GetItem(cashDesk);
				printReturnReceiptRequest.crn = ((item3 == null) ? cashRow.ECRCrn : item3.ECRCrn);
			}
			else
			{
				printReturnReceiptRequest.crn = cashRow.ECRCrn;
			}
		}
		else
		{
			printReturnReceiptRequest.crn = eCRCRN;
		}
		printReturnReceiptRequest.returnTicketId = int.Parse(_baseECRCheckNum);
		printReturnReceiptRequest.cardAmountForReturn = _summaPlCardValue;
		printReturnReceiptRequest.cashAmountForReturn = cashAmountForReturn;
		printReturnReceiptRequest.prePaymentAmountForReturn = _summaPrePaymentValue;
		printReturnReceiptRequest.returnItemList = list;
		printReturnReceiptRequest.eMarks = list2;
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

	private object CreateReturnRequestForBonus(CashCacheRow cashRow)
	{
		MTBill mTBill = (MTBill)BaseDocument;
		Utilities.PrintReturnReceiptRequest printReturnReceiptRequest = new Utilities.PrintReturnReceiptRequest();
		if (string.IsNullOrWhiteSpace(mTBill.ECRCRN))
		{
			if (!string.IsNullOrWhiteSpace(mTBill.CashDesk) && cashRow.Code.Trim() != mTBill.CashDesk.Trim())
			{
				CashCacheRow item = TradeSettings.TradeCacheService.Cash.GetItem(mTBill.CashDesk);
				printReturnReceiptRequest.crn = ((item == null) ? cashRow.ECRCrn : item.ECRCrn);
			}
			else
			{
				printReturnReceiptRequest.crn = cashRow.ECRCrn;
			}
		}
		else
		{
			printReturnReceiptRequest.crn = mTBill.ECRCRN;
		}
		int.TryParse(mTBill.PrePaymentECRCheckNum, out var result);
		printReturnReceiptRequest.returnTicketId = result;
		printReturnReceiptRequest.cardAmountForReturn = _summaPlCardSubtractBonus;
		printReturnReceiptRequest.cashAmountForReturn = _summaCashSubtractBonus;
		return printReturnReceiptRequest;
	}

	private PosTerminalResponse RefundPOSTerminalIfChosen()
	{
		PosTerminalResponse result = null;
		if (BaseDocument.POType != DocumentTypeSE.MTBill.Value || PO.PaymentRefundAccepted || _summaPlCard.Value == 0m)
		{
			return result;
		}
		MTBill mTBill = (MTBill)BaseDocument;
		if (!mTBill.POSTerminalType.InList("2", "4"))
		{
			return result;
		}
		string text;
		if (string.IsNullOrWhiteSpace(mTBill.PaymentId))
		{
			text = "";
		}
		else
		{
			text = (mTBill.PaymentId.Contains(':') ? mTBill.PaymentId.Split(':')[1].Trim() : mTBill.PaymentId.Trim());
		}
		if (mTBill.POSTerminalType == "2")
		{
			result = Arcus2Helpers.CheckAndRefund(mTBill.POSTerminalID, _summaPlCard.Value, mTBill.SummaPlCard.Value, text);
		}
		if (mTBill.POSTerminalType == "4")
		{
			result = PossoftPayClientHelper.Refund(text, PO.POSTerminalAddress, _summaPlCard.Value);
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
			PO.TS = Settings.DocumentService.EditFields(PO.ISN, properties, sr);
			LoadDataToControls();
			IsChanged = false;
			UpdateDataViewPanel(DVPID);
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
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
		if (string.IsNullOrWhiteSpace(_baseECRCheckNum))
		{
			ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Common.Resources.ValidateEmptyECRNumForBaseDoc, Settings.ProgramName, MessageBoxButton.OK, MessageBoxImage.Asterisk);
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
		if (State == DocumentStateSE.Deleted.Value)
		{
			ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Common.Resources.MustBeSaveDocBeforePrintECRCheck, ArmSoft.SME.Common.Resources.ECRReturnCheck, MessageBoxButton.OK, MessageBoxImage.Asterisk);
			return false;
		}
		if ((!StoredInDb || IsChanged || State == DocumentStateSE.Draft.Value) && !SaveDocument())
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
		decimal value = _summaPlCard.Value;
		decimal num = _summaCash.Value - _summaChange.Value;
		decimal value2 = _summaPrePayment.Value;
		decimal value3 = _summaCredit.Value;
		decimal num2 = 0m;
		if (!string.IsNullOrWhiteSpace(Settings.ParametersService.ECRCheckPrintBonusType()))
		{
			num2 = _usedBonus;
		}
		if (value + num + num2 + value3 + value2 == 0m)
		{
			ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Trade.Common.Resources.ReturnSumsNotSet, ArmSoft.SME.Common.Resources.ECRReturnCheck, MessageBoxButton.OK, MessageBoxImage.Asterisk);
			return false;
		}
		if (value + num + num2 + value3 + value2 < _summa.Value)
		{
			ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Trade.Common.Resources.ReturnSumLessThanTotalSum, ArmSoft.SME.Common.Resources.ECRReturnCheck, MessageBoxButton.OK, MessageBoxImage.Asterisk);
			return false;
		}
		if (value + num + num2 + value3 + value2 > _summa.Value)
		{
			ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Trade.Common.Resources.ReturnSumGreaterThanTotalSum, ArmSoft.SME.Common.Resources.ECRReturnCheck, MessageBoxButton.OK, MessageBoxImage.Asterisk);
			return false;
		}
		if (value > _baseECRSummPLCard)
		{
			ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Trade.Common.Resources.ReturnByCardSumGreaterThanBaseDocByCardSum, ArmSoft.SME.Common.Resources.ECRReturnCheck, MessageBoxButton.OK, MessageBoxImage.Asterisk);
			return false;
		}
		if (value2 > _baseECRSummPrePayment)
		{
			ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Trade.Common.Resources.ReturnByPrePaymentSumGreaterThanBaseDocByPrePaymentSum, ArmSoft.SME.Common.Resources.ECRReturnCheck, MessageBoxButton.OK, MessageBoxImage.Asterisk);
			return false;
		}
		if (num > _baseECRSummCash)
		{
			ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Trade.Common.Resources.ReturnByCashSumGreaterThanBaseDocByCashSum, ArmSoft.SME.Common.Resources.ECRReturnCheck, MessageBoxButton.OK, MessageBoxImage.Asterisk);
			return false;
		}
		if (value3 > _baseECRSummCredit)
		{
			ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Trade.Common.Resources.ReturnByCreditSumGreaterThanBaseDocByCreditSum, ArmSoft.SME.Common.Resources.ECRReturnCheck, MessageBoxButton.OK, MessageBoxImage.Asterisk);
			return false;
		}
		return true;
	}

	private void UpdateDocAndWindowAfterCheckPrint(string checkNumber, bool forBonus = false, bool fillXInBonus = false)
	{
		if (fillXInBonus)
		{
			_prePaymentECRCheckNum.Text = "XXXXXXXX";
			PO.PrePaymentECRCheckNum = "XXXXXXXX";
		}
		if (forBonus)
		{
			_prePaymentECRCheckNum.Text = checkNumber;
			PO.PrePaymentECRCheckNum = checkNumber;
		}
		else
		{
			_ecrCheckNum.Text = checkNumber;
			PO.ECRCheckNum = checkNumber;
		}
		List<PropertyWithValue> list = new List<PropertyWithValue>();
		if (forBonus)
		{
			list.Add(new PropertyWithValue("PrePaymentECRCheckNum", PO.PrePaymentECRCheckNum));
		}
		else
		{
			list.Add(new PropertyWithValue("ECRCheckNum", PO.ECRCheckNum));
		}
		PO.TS = ECRHelpers.ChangeDocECRCheckInfo(checkNumber, "", POISN(), list, PO.TS);
		LoadDataToControls();
		IsChanged = false;
		SetControlsReadonly();
		UpdateDataViewPanel(DVPID);
		if (Settings.ParametersService.PrintECRCheckAndNew() && !Settings.ParametersService.PrintECRAndProductCheck() && !fillXInBonus)
		{
			_viewMode = WindowViewMode.NewAfterSave;
			OnNewAfterSaveUpper();
		}
	}

	private void AutoCalcDiscountAdjustControlsAccessibility(bool onlyGridButtons = false)
	{
		bool flag = PaymentIsInProcess();
		_increase.IsEnabled = !flag;
		_decrease.IsEnabled = !flag;
		if (_weight != null)
		{
			_weight.IsEnabled = !flag;
		}
		if (_discount != null)
		{
			_discount.IsEnabled = !flag;
		}
		if (_prices != null)
		{
			_prices.IsEnabled = !flag;
		}
		if (_mtFavorites != null)
		{
			_mtFavorites.IsEnabled = !flag;
		}
		if (onlyGridButtons)
		{
			return;
		}
		_mtBillReturnGrid.AllowAdd = !flag && _isAllowChangeGoodRowInBill && string.IsNullOrWhiteSpace(_baseDocument.Text);
		_mtBillReturnGrid.AllowEdit = !flag && _isAllowChangeGoodRowInBill;
		_mtBillReturnGrid.AllowDuplicate = !flag && _isAllowChangeGoodRowInBill && string.IsNullOrWhiteSpace(_baseDocument.Text);
		_mtBillReturnGrid.AllowDelete = !flag;
		_mtBillReturnGrid.AllowMoveUp = !flag;
		_mtBillReturnGrid.AllowMoveDown = !flag;
		foreach (GridColumn item in _mtBillReturnGrid.Grid.Columns.Except(new DocumentGridColumn[3] { _discountCol, _discountPriceCol, _summaCol }))
		{
			item.ReadOnly = flag || !_isAllowChangeGoodRowInBill;
		}
		_date.IsReadOnly = flag || !_allowChangeDate;
		_storage.IsReadOnly = flag || !_isAllowChangeStorageInBill;
		_cashDesk.IsReadOnly = flag || !_isAllowChangeCashInBill;
		_partner.IsReadOnly = flag || !_isAllowChangeBuyerInBill || !string.IsNullOrWhiteSpace(_ecrCheckNum.Text);
		_salesConsultant.IsReadOnly = flag || !_isAllowChangeSaleConsultantInBill;
		_owner.IsReadOnly = flag;
		_barCodeEnter.IsEnabled = false;
		_codeEnter.IsEnabled = false;
		_mtListLikeName.IsEnabled = false;
		_qtyEnter.IsEnabled = false;
		_increase.IsEnabled = false;
		if (_salePrice != null)
		{
			_salePrice.IsEnabled = !flag;
		}
	}

	private void OnRowInfoChange(MTRetFromCustSpecificationRow row)
	{
		if (_isShowInfoPanel || _isShowPoleDisplay)
		{
			ChangeRowInfo(row);
		}
	}

	private void ChangeRowInfo(MTRetFromCustSpecificationRow row = null)
	{
		if (row == null)
		{
			if (_isShowInfoPanel)
			{
				((DispatcherObject)this).Dispatcher.BeginInvoke((Delegate)new DelegateClearInfoPanel(CleanInfoPanel), Array.Empty<object>());
			}
			if (_isShowPoleDisplay && TradeSettings.PoleDisplay != null)
			{
				ClearPoleDisplay(string.IsNullOrWhiteSpace(_partCode));
				if (CalcMTBillReturnListRowCount(includeDeleted: false) != 0)
				{
					SetTotalInfoToPoleDisplay();
				}
			}
		}
		else
		{
			if (_isShowInfoPanel)
			{
				((DispatcherObject)this).Dispatcher.BeginInvoke((Delegate)new DelegateSetRowInfoToInfoPanel(SetRowInfoToPanel), new object[1] { row });
			}
			if (_isShowPoleDisplay && TradeSettings.PoleDisplay != null)
			{
				((DispatcherObject)this).Dispatcher.BeginInvoke((Delegate)new DelegateSetRowInfoToPoleDisplay(TradeSettings.PoleDisplay.SetRowInfo), new object[1] { row });
			}
		}
	}

	private void SetRowInfoToPanel(MTRetFromCustSpecificationRow row)
	{
		try
		{
			if (string.IsNullOrWhiteSpace(row.ItemName))
			{
				CleanInfoPanel();
				return;
			}
			short num = 2;
			_bIsChanged = IsChanged;
			decimal num2 = (row.Quantity * row.Price).RoundAs(2);
			_infoItemName.Text = row.ItemName.Trim();
			TextEditExt infoFormula = _infoFormula;
			InlineArray4<object> buffer = default;
			buffer[0] = row.Quantity.CurrencyFormat(16, (short)((!(row.Quantity == decimal.Floor(row.Quantity))) ? 3 : 0));
			buffer[1] = row.UnitBrief.Trim();
			buffer[2] = row.Price.CurrencyFormat(16, (short)((!(row.Price == decimal.Floor(row.Price))) ? num : 0));
			buffer[3] = num2.CurrencyFormat(16, (short)((!(num2 == decimal.Floor(num2))) ? num : 0));
			infoFormula.Text = string.Format("{0} {1} x {2} = {3}", (ReadOnlySpan<object?>)buffer);
			if (Settings.ParametersService.EnableSubstituteItems())
			{
				if (TradeSettings.TradeCacheService.Material.HasSubstitution(row.ItemCode))
				{
					_substitutionInfo.Text = ArmSoft.SME.Trade.Common.Resources.SubstitutionsExists;
					((LayoutGroup)(object)_hintImage.Parent).Visibility = Visibility.Visible;
				}
				else
				{
					_substitutionInfo.Text = "";
					((LayoutGroup)(object)_hintImage.Parent).Visibility = Visibility.Collapsed;
				}
			}
			if ((_isShowStorageAvailabilityOnInfoPanel || _isShowWholeAvailabilityOnInfoPanel) && !IsOfflineDoc)
			{
				SetAvailabilityInfoToPanel(row);
			}
			SetDescriptionInfoPanel(row);
			if (_isShowItemPictureOnInfoPanel)
			{
				MaterialCacheRow.QntUnitRow qntUnit = TradeSettings.TradeCacheService.Material.GetQntUnit(row.ItemID, row.UnitCode);
				if (qntUnit != null && qntUnit.ImageGUID.HasValue)
				{
					if (!_images.TryGetValue(qntUnit.ImageGUID.Value, out var value))
					{
						value = Settings.ImagesService.GetItemImage(ImageItemTypeSE.Materials.Value, qntUnit.ImageGUID.ToString());
						_images.Add(qntUnit.ImageGUID.Value, value);
					}
					_itemImage.EditValue = value;
					_itemImage.UpdateLayout();
				}
			}
			IsChanged = _bIsChanged;
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	private void SetAvailabilityInfoToPanel(MTRetFromCustSpecificationRow row)
	{
		_infoDisplayControls.ClearInfoAvailability();
		if (row.ItemType == GoodsTypesSE.Good.Value)
		{
			if (_isShowStorageAvailabilityOnInfoPanel)
			{
				_infoDisplayControls.SetAccBalance(MaterialHelpers.GetGoodRemains(row.UpdateRemainsOnInfoPanel, row.ItemID, row.UnitCode, ref _availabilityByGoodId, row.Storage));
				_infoDisplayControls.SetAccUnitBrief(row.UnitBrief.Trim());
			}
			if (_isShowWholeAvailabilityOnInfoPanel)
			{
				_infoDisplayControls.SetWholeBalance(MaterialHelpers.GetGoodRemains(row.UpdateRemainsOnInfoPanel, row.ItemID, row.UnitCode, ref _availabilityByGoodId));
				_infoDisplayControls.SetWholeUnitBrief(row.UnitBrief.Trim());
			}
		}
		row.UpdateRemainsOnInfoPanel = false;
	}

	private void SetDescriptionInfoPanel(MTRetFromCustSpecificationRow row)
	{
		if (Settings.ParametersService.ShowDescriptionOnInfoPanel())
		{
			MaterialCacheRow item = TradeSettings.TradeCacheService.Material.GetItem(row.ItemCode);
			_infoDescription.Text = ((item == null) ? "" : item.Description.Trim());
		}
	}

	private void CleanInfoPanel()
	{
		_infoItemName.Text = "";
		_infoFormula.Text = "";
		if (_substitutionInfo != null)
		{
			_substitutionInfo.Text = "";
		}
		_infoDisplayControls.ClearInfoAvailability();
		if (_infoDescription != null)
		{
			_infoDescription.Text = "";
		}
		if (_itemImage != null)
		{
			_itemImage.EditValue = null;
			_itemImage.UpdateLayout();
		}
	}

	private void SetTotalInfoToPoleDisplay()
	{
		if (TradeSettings.PoleDisplay != null)
		{
			((DispatcherObject)this).Dispatcher.BeginInvoke((Delegate)new DelegateSetTotalInfoToPoleDisplay(TradeSettings.PoleDisplay.SetTotalInfo), new object[8]
			{
				_summa.Value,
				_tmpDiscountSumm,
				_summaCash.Value,
				_summaPlCard.Value,
				_summaPayByBonus.Value,
				_summaPrePayment.Value,
				_summaCredit.Value,
				PaidSum() - _summa.Value
			});
		}
	}

	private void SetRowCountInfoToPoleDisplay(int rowCount = 0)
	{
		if (TradeSettings.PoleDisplay != null)
		{
			((DispatcherObject)this).Dispatcher.BeginInvoke((Delegate)new DelegateSetRowCountInfoToPoleDisplay(TradeSettings.PoleDisplay.SetRowCountInfo), new object[1] { rowCount });
		}
	}

	private void InitPoleDisplay(bool onEdit = false)
	{
		if (_isShowPoleDisplay)
		{
			if (TradeSettings.PoleDisplay == null || PresentationSource.FromVisual(TradeSettings.PoleDisplay) == null)
			{
				TradeSettings.PoleDisplay = new PoleDisplayWindow();
			}
			else if (onEdit)
			{
				SetTotalInfoToPoleDisplay();
			}
			else
			{
				ClearPoleDisplay(clearAll: true);
			}
			TradeSettings.PoleDisplay.Show();
		}
	}

	private void ClearPoleDisplay(bool clearAll)
	{
		if (TradeSettings.PoleDisplay != null)
		{
			((DispatcherObject)this).Dispatcher.BeginInvoke((Delegate)new DelegateClearPoleDisplay(TradeSettings.PoleDisplay.ClearScreen), new object[2] { clearAll, "" });
		}
	}

	private void SetBuyerInfoToPoleDisplay()
	{
		if (TradeSettings.PoleDisplay == null)
		{
			return;
		}
		decimal num = 0m;
		decimal num2 = 0m;
		if (_storedBonus)
		{
			num = _mtBillReturnList.Sum((MTRetFromCustSpecificationRow row) => row.AccumulatedBonus).RoundAs(2);
			if (!string.IsNullOrWhiteSpace(_contract))
			{
				decimal num3 = 0m;
				if (_connectTob2barmsoftDiscountServer)
				{
					PartnerCacheRow.ContractRow contract = TradeSettings.TradeCacheService.Partner.GetContract(_partCode, _contract);
					if (contract != null)
					{
						num3 = DiscountServerService.GetCardBalance(contract.DiscountCard);
					}
				}
				else
				{
					TradeSettings.TradeCacheService.Partner.GetContract(_partCode, _contract, out var partnerCacheRow);
					num3 = TradeSettings.CreateTradeClient().CommonServices.GetAccRem(CommonConst.AccountingsTypes.Bonus, _contract, partnerCacheRow.Id, _currCode.Code, _date.EditValue.Value).SummNcc;
				}
				num2 = num3;
			}
		}
		string text = "";
		if (!string.IsNullOrWhiteSpace(_partCode))
		{
			PartnerCacheRow item = TradeSettings.TradeCacheService.Partner.GetItem(_partCode);
			if (item != null)
			{
				text = item.Caption;
			}
		}
		((DispatcherObject)this).Dispatcher.BeginInvoke((Delegate)new DelegateSetBuyerInfoToPoleDisplay(TradeSettings.PoleDisplay.SetBuyerInfo), new object[5] { _partCode, text, _contract, num, num2 });
	}

	protected override void AddToolbarItems()
	{
		base.AddToolbarItems();
		Ribbon.ToolbarItemLinks.Add(_barButtonJumpTo.ConvertForToolbar("t_doc_JumpTo_16.svg"));
		Ribbon.ToolbarItemLinks.Add(_barButtonPrintLabel.ConvertForToolbar("t_action_16.svg"));
		Ribbon.ToolbarItemLinks.Add(_barButtonImportFromFile.ConvertForToolbar("t_import_16.svg"));
		Ribbon.ToolbarItemLinks.Add(_barSubItemJournals.ConvertForToolbar("t_report_16.svg"));
		Ribbon.ToolbarItemLinks.Add(_barButtonDocumentSettings.ConvertForToolbar("t_page_white_gear_16.svg"));
	}

	protected override void AddItemsToRibbon()
	{
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0148: Unknown result type (might be due to invalid IL or missing references)
		//IL_014e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0164: Unknown result type (might be due to invalid IL or missing references)
		//IL_016a: Unknown result type (might be due to invalid IL or missing references)
		//IL_020a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0210: Unknown result type (might be due to invalid IL or missing references)
		//IL_0226: Unknown result type (might be due to invalid IL or missing references)
		//IL_022c: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f7: Unknown result type (might be due to invalid IL or missing references)
		base.AddItemsToRibbon();
		CreateRibbonPageGroupActions();
		CreateRibbonPageGroupReports();
		_barButtonJumpTo = new ReportBarButtonItem(DataProviderTypeSE.AllDocumentsJournal, showDialog: false, ArmSoft.SME.Common.Resources.JumpTo, null, "doc_JumpTo_16.svg", "doc_JumpTo_32.svg");
		_barButtonJumpTo.AddInBarSubItem(ref _ribbonPageGroupActions, ref _barSubItemActions);
		_barButtonPrintLabel = new BarButtonItem
		{
			Content = ArmSoft.SME.Trade.Common.Resources.PrintLabel,
			KeyGesture = new KeyGesture((Key)55, (ModifierKeys)2),
			Glyph = ImageService.GetImage("action_16.svg"),
			LargeGlyph = ImageService.GetImage("action_32.svg")
		};
		_barButtonPrintLabel.ItemClick += new SafeDocumentEventHandler<ItemClickEventArgs>(BarButtonPrintLabel_ItemClick, this).Handler;
		_barManager.Items.Add(_barButtonPrintLabel);
		_ribbonPageGroupActions.ItemLinks.Add(_barButtonPrintLabel.CreateLink());
		Ribbon.ToolbarItemLinks.Add(_barButtonPrintLabel.CreateLink());
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
		_barButtonDocumentSettings = new BarButtonItem
		{
			Content = ArmSoft.SME.Common.Resources.Settings,
			Glyph = ImageService.GetImage("page_white_gear_16.svg"),
			LargeGlyph = ImageService.GetImage("page_white_gear_32.svg")
		};
		_barButtonDocumentSettings.ItemClick += new SafeDocumentEventHandler<ItemClickEventArgs>(BarButtonDocumentSettings_ItemClick, this).Handler;
		_barManager.Items.Add(_barButtonDocumentSettings);
		_ribbonPageGroupActions.ItemLinks.Add(_barButtonDocumentSettings.CreateLink());
		Ribbon.ToolbarItemLinks.Add(_barButtonDocumentSettings.CreateLink());
		_barSubItemJournals.Content = "";
		_barSubItemJournals.Glyph = ImageService.GetImage("report_16.svg");
		_barSubItemJournals.LargeGlyph = ImageService.GetImage("report_16.svg");
		_barButtonMTSalesAnalysis = new ReportBarButtonItem(DataProviderTypeSE.MatRepSalesAnalysis.Value, showDialog: false, ArmSoft.SME.Trade.Common.Resources.MTRepSalesAnalysis);
		_barButtonMTSalesAnalysis.AddInBarSubItem(ref _ribbonPageGroupReports, ref _barSubItemJournals);
		_barButtonGoodsOperationsJournal = new ReportBarButtonItem(DataProviderTypeSE.GoodsRepOperations.Value, showDialog: false, ArmSoft.SME.Trade.Common.Resources.MTOperations);
		_barButtonGoodsOperationsJournal.AddInBarSubItem(ref _ribbonPageGroupReports, ref _barSubItemJournals);
		_barButtonGoodsRetailOperationJournal = new ReportBarButtonItem(DataProviderTypeSE.MatRepOperationsRetail.Value, showDialog: false, ArmSoft.SME.Trade.Common.Resources.MTOperationsRetail);
		_barButtonGoodsRetailOperationJournal.AddInBarSubItem(ref _ribbonPageGroupReports, ref _barSubItemJournals);
		_barButtonPartnerOperationJournal = new ReportBarButtonItem(DataProviderTypeSE.PartOpJournal.Value, showDialog: false, ArmSoft.SME.Trade.Common.Resources.PartnerOp);
		_barButtonPartnerOperationJournal.AddInBarSubItem(ref _ribbonPageGroupReports, ref _barSubItemJournals);
		_barButtonBonusOperationJournal = new ReportBarButtonItem(DataProviderTypeSE.BonusOpJournal.Value, showDialog: false, ArmSoft.SME.Trade.Common.Resources.BonusOp);
		_barButtonBonusOperationJournal.AddInBarSubItem(ref _ribbonPageGroupReports, ref _barSubItemJournals);
		_barButtonCashOperationJournal = new ReportBarButtonItem(DataProviderTypeSE.CashOperations.Value, showDialog: false, ArmSoft.SME.Trade.Common.Resources.KassaOp);
		_barButtonCashOperationJournal.AddInBarSubItem(ref _ribbonPageGroupReports, ref _barSubItemJournals);
		if (_connectTob2barmsoftDiscountServer)
		{
			_barStatusConnectToDS = new BarStaticItem
			{
				ItemMinWidth = 165.0,
				AutoSizeMode = BarItemAutoSizeMode.Fill,
				ContentAlignment = HorizontalAlignment.Center,
				ContentTemplate = GetStatusBarContentTemplate()
			};
			_barManager.Items.Add(_barStatusConnectToDS);
			_ribbonStatusBar.RightItemLinks.AddWithSeparator(_barStatusConnectToDS.CreateLink());
		}
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
		ReportBarButtonItem barButtonPartnerOperationJournal = _barButtonPartnerOperationJournal;
		num = 3;
		List<PropertyWithValue> list2 = new List<PropertyWithValue>(num);
		CollectionsMarshal.SetCount(list2, num);
		Span<PropertyWithValue> span2 = CollectionsMarshal.AsSpan(list2);
		span2[0] = new PropertyWithValue("DocISN", PO.ISN.ToString());
		span2[1] = new PropertyWithValue("AdditionalCaption", GetReportsAdditionalCaption());
		span2[2] = new PropertyWithValue("ShowLinks", true.ToString());
		barButtonPartnerOperationJournal.PropertyWithValues = list2;
		ReportBarButtonItem barButtonMTSalesAnalysis = _barButtonMTSalesAnalysis;
		num = 3;
		List<PropertyWithValue> list3 = new List<PropertyWithValue>(num);
		CollectionsMarshal.SetCount(list3, num);
		Span<PropertyWithValue> span3 = CollectionsMarshal.AsSpan(list3);
		span3[0] = new PropertyWithValue("DocISN", PO.ISN.ToString());
		span3[1] = new PropertyWithValue("AdditionalCaption", GetReportsAdditionalCaption());
		span3[2] = new PropertyWithValue("ShowNCCVATSumm", true.ToString());
		barButtonMTSalesAnalysis.PropertyWithValues = list3;
		ReportBarButtonItem barButtonGoodsOperationsJournal = _barButtonGoodsOperationsJournal;
		num = 2;
		List<PropertyWithValue> list4 = new List<PropertyWithValue>(num);
		CollectionsMarshal.SetCount(list4, num);
		Span<PropertyWithValue> span4 = CollectionsMarshal.AsSpan(list4);
		span4[0] = new PropertyWithValue("DocISN", PO.ISN.ToString());
		span4[1] = new PropertyWithValue("AdditionalCaption", GetReportsAdditionalCaption());
		barButtonGoodsOperationsJournal.PropertyWithValues = list4;
		ReportBarButtonItem barButtonGoodsRetailOperationJournal = _barButtonGoodsRetailOperationJournal;
		num = 2;
		List<PropertyWithValue> list5 = new List<PropertyWithValue>(num);
		CollectionsMarshal.SetCount(list5, num);
		Span<PropertyWithValue> span5 = CollectionsMarshal.AsSpan(list5);
		span5[0] = new PropertyWithValue("DocISN", PO.ISN.ToString());
		span5[1] = new PropertyWithValue("AdditionalCaption", GetReportsAdditionalCaption());
		barButtonGoodsRetailOperationJournal.PropertyWithValues = list5;
		ReportBarButtonItem barButtonBonusOperationJournal = _barButtonBonusOperationJournal;
		num = 2;
		List<PropertyWithValue> list6 = new List<PropertyWithValue>(num);
		CollectionsMarshal.SetCount(list6, num);
		Span<PropertyWithValue> span6 = CollectionsMarshal.AsSpan(list6);
		span6[0] = new PropertyWithValue("DocISN", PO.ISN.ToString());
		span6[1] = new PropertyWithValue("AdditionalCaption", GetReportsAdditionalCaption());
		barButtonBonusOperationJournal.PropertyWithValues = list6;
		ReportBarButtonItem barButtonCashOperationJournal = _barButtonCashOperationJournal;
		num = 2;
		List<PropertyWithValue> list7 = new List<PropertyWithValue>(num);
		CollectionsMarshal.SetCount(list7, num);
		Span<PropertyWithValue> span7 = CollectionsMarshal.AsSpan(list7);
		span7[0] = new PropertyWithValue("DocISN", PO.ISN.ToString());
		span7[1] = new PropertyWithValue("AdditionalCaption", GetReportsAdditionalCaption());
		barButtonCashOperationJournal.PropertyWithValues = list7;
	}

	private void BarButtonDocumentSettings_ItemClick(object sender, ItemClickEventArgs e)
	{
		DialogWindow dialogWindow = new DialogWindow
		{
			Caption = ArmSoft.SME.Common.Resources.Settings,
			Owner = this
		};
		CheckEditExt checkEditExt = dialogWindow.AddBooleanControl("ApplyLastPrice", ArmSoft.SME.Trade.Common.Resources.ApplyLastPrice);
		checkEditExt.IsChecked = _applyLastPrice;
		CheckEditExt checkEditExt2 = dialogWindow.AddBooleanControl("ImmediatlyEditRow", ArmSoft.SME.Trade.Common.Resources.ImmediatlyEditRow);
		checkEditExt2.IsChecked = _immediatlyEditRow;
		CheckEditExt checkEditExt3 = dialogWindow.AddBooleanControl("SearchBarCodeInProperty1", ArmSoft.SME.Trade.Common.Resources.SearchBarCodeInPartiesWithProperty1);
		checkEditExt3.IsChecked = _searchBarCodeInProperty1;
		if (dialogWindow.ShowDialog().Value)
		{
			_applyLastPrice = checkEditExt.IsChecked.Value;
			_searchBarCodeInProperty1 = checkEditExt3.IsChecked.Value;
			_immediatlyEditRow = checkEditExt2.IsChecked.Value;
		}
	}

	private void BarButtonImportFromFile_ItemClick(object sender, ItemClickEventArgs e)
	{
		try
		{
			if (_date.IsReadOnly || !_date.DoValidate())
			{
				return;
			}
			if (string.IsNullOrWhiteSpace(_storage.Code))
			{
				throw new ASException(ArmSoft.SME.Trade.Common.Resources.ErrorStorageFromFile);
			}
			if (_mtBillReturnList.Count <= 0 || ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Trade.Common.Resources.LoadMaterialsList, ArmSoft.SME.Trade.Common.Resources.ProgramName, MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No) != MessageBoxResult.No)
			{
				_mtBillReturnList.Clear();
				_maxRowID = 0;
				TerminalHelper.TerminalImporter terminalImporter = new TerminalHelper.TerminalImporter(this);
				if (terminalImporter.ImportFromFile((decimal quantity, MaterialCacheRow goodRow, MaterialCacheRow.BarCodeRow goodBarcodeRow) =>
				{
					AddNewMTFromFile(goodRow, goodBarcodeRow.QntUnit, quantity);
				}))
				{
					CalculateDocumentSumma();
					terminalImporter.ShowReport();
				}
			}
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
		_barButtonMTSalesAnalysis.IsEnabled = isEnabled;
		_barButtonGoodsOperationsJournal.IsEnabled = isEnabled;
		_barButtonGoodsRetailOperationJournal.IsEnabled = isEnabled;
		_barButtonPartnerOperationJournal.IsEnabled = isEnabled;
		_barButtonBonusOperationJournal.IsEnabled = isEnabled;
		_barButtonCashOperationJournal.IsEnabled = isEnabled;
	}

	public DataTemplate GetStatusBarContentTemplate()
	{
		return (DataTemplate)Application.Current.FindResource("MyStatusBarContentTemplate");
	}

	private void BarButtonPrintLabel_ItemClick(object sender, ItemClickEventArgs e)
	{
		if (_mtBillReturnList.Count > 0)
		{
			FillDocument(new StoreRequest());
			new PrintLabelDialog(this, PO).Invoke();
		}
	}

	private bool PaymentIsInProcess()
	{
		return PaidSum() - _summaPayByBonus.Value > 0m;
	}

	private bool IsUnderpaid()
	{
		return _summa.Value > PaidSum();
	}

	private decimal PaidSum()
	{
		CalcAccoumulatedAndUsedBonuses();
		return _summaCash.Value + _summaCredit.Value + _summaPayByBonus.Value + _summaPlCard.Value + _summaPrePayment.Value;
	}

	private decimal RemainSumForCreditPayment()
	{
		decimal num = _summa.Value - _summaCash.Value - 0m - _summaPlCard.Value - _summaPayByBonus.Value;
		num -= _summaPrePayment.Value;
		if (!(num <= 0m))
		{
			return num;
		}
		return 0m;
	}

	private decimal RemainSumForCardPayment()
	{
		decimal num = _summa.Value - _summaCash.Value - _summaCredit.Value - _summaPayByBonus.Value;
		num -= _summaPrePayment.Value;
		if (!(num <= 0m))
		{
			return num;
		}
		return 0m;
	}

	private decimal RemainSumForCashPayment()
	{
		decimal num = _summa.Value - _summaCredit.Value - _summaPlCard.Value - _summaPayByBonus.Value;
		num -= _summaPrePayment.Value;
		if (!(num <= 0m))
		{
			return num;
		}
		return 0m;
	}

	private decimal RemainSumForPrePayment()
	{
		decimal num = _summa.Value - _summaCash.Value - _summaPayByBonus.Value - _summaPlCard.Value - _summaCredit.Value;
		if (!(num <= 0m))
		{
			return num;
		}
		return 0m;
	}
}
