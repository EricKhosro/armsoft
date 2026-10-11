using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.IO;
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
using ArmSoft.SME.Common.Models.Dialog;
using ArmSoft.SME.Common.Models.Documents;
using ArmSoft.SME.Common.Models.Exchange;
using ArmSoft.SME.Common.Parameters;
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
using ArmSoft.SME.Trade.UI.DataView;
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
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.LayoutControl;
using DevExpress.XtraEditors.DXErrorProvider;
using Microsoft.Win32;

namespace ArmSoft.SME.Trade.UI.Documents;

public class MTIncomeBaseWindow<T> : TradeDocumentWindow<T> where T : MTIncomeBase, new()
{
	public class MTIncomeSpecificationRow : DocumentGridRow
	{
		[ObservableProperty]
		private int mTID = -1;

		[ObservableProperty]
		private string mTCode = string.Empty;

		[ObservableProperty]
		private string unitCode = string.Empty;

		[ObservableProperty]
		private string mTName;

		[ObservableProperty]
		private string unitBrief;

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
		private decimal extraPercent;

		[ObservableProperty]
		private decimal salePrice;

		[ObservableProperty]
		private decimal saleSumma;

		[ObservableProperty]
		private string vATCodeForConvert;

		[ObservableProperty]
		private bool vAT;

		[ObservableProperty]
		private string rowDescription;

		[ObservableProperty]
		private DateOnly? useLife;

		[ObservableProperty]
		private string country;

		[ObservableProperty]
		private string producer;

		[ObservableProperty]
		private string property1;

		[ObservableProperty]
		private string property1Caption;

		[ObservableProperty]
		private string property2;

		[ObservableProperty]
		private string property2Caption;

		[ObservableProperty]
		private string fullName;

		[ObservableProperty]
		private string producerCaption;

		[ObservableProperty]
		private decimal lastPurchasePrice;

		[ObservableProperty]
		private decimal variancePercent;

		public bool UpdateRemainsOnInfoPanel = true;

		public short RowNum;

		[ObservableProperty]
		private int rowID;

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
					string oldValue = mTCode;
					OnPropertyChanging(__KnownINotifyPropertyChangingArgs.MTCode);
					mTCode = value;
					OnMTCodeChanged(oldValue, value);
					OnPropertyChanged(__KnownINotifyPropertyChangedArgs.MTCode);
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
		public decimal ExtraPercent
		{
			get
			{
				return extraPercent;
			}
			set
			{
				if (!EqualityComparer<decimal>.Default.Equals(extraPercent, value))
				{
					OnPropertyChanging(__KnownINotifyPropertyChangingArgs.ExtraPercent);
					extraPercent = value;
					OnPropertyChanged(__KnownINotifyPropertyChangedArgs.ExtraPercent);
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
		public decimal LastPurchasePrice
		{
			get
			{
				return lastPurchasePrice;
			}
			set
			{
				if (!EqualityComparer<decimal>.Default.Equals(lastPurchasePrice, value))
				{
					OnPropertyChanging(__KnownINotifyPropertyChangingArgs.LastPurchasePrice);
					lastPurchasePrice = value;
					OnPropertyChanged(__KnownINotifyPropertyChangedArgs.LastPurchasePrice);
				}
			}
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public decimal VariancePercent
		{
			get
			{
				return variancePercent;
			}
			set
			{
				if (!EqualityComparer<decimal>.Default.Equals(variancePercent, value))
				{
					OnPropertyChanging(__KnownINotifyPropertyChangingArgs.VariancePercent);
					variancePercent = value;
					OnPropertyChanged(__KnownINotifyPropertyChangedArgs.VariancePercent);
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

		public MTIncomeSpecificationRow()
		{
		}

		public MTIncomeSpecificationRow(MTIncomeBaseWindow<T> docWindow)
			: base(docWindow)
		{
		}

		static MTIncomeSpecificationRow()
		{
			DocumentGridRow.RegisterPropertyValidators(typeof(MTIncomeSpecificationRow), cacheValidationResults: true, new Dictionary<string, Action<DocumentGridRow, DevExpress.XtraEditors.DXErrorProvider.ErrorInfo>>
			{
				{
					"MTCode",
					(DocumentGridRow row, DevExpress.XtraEditors.DXErrorProvider.ErrorInfo info) =>
					{
						ValidateGoodCode((MTIncomeSpecificationRow)row, info);
					}
				},
				{
					"UnitCode",
					(DocumentGridRow row, DevExpress.XtraEditors.DXErrorProvider.ErrorInfo info) =>
					{
						ValidateUnitCode((MTIncomeSpecificationRow)row, info);
					}
				},
				{
					"Quantity",
					(DocumentGridRow row, DevExpress.XtraEditors.DXErrorProvider.ErrorInfo info) =>
					{
						ValidateQuantity((MTIncomeSpecificationRow)row, info);
					}
				},
				{
					"Summa",
					(DocumentGridRow row, DevExpress.XtraEditors.DXErrorProvider.ErrorInfo info) =>
					{
						ValidateSumma((MTIncomeSpecificationRow)row, info);
					}
				},
				{
					"Country",
					(DocumentGridRow row, DevExpress.XtraEditors.DXErrorProvider.ErrorInfo info) =>
					{
						ValidateCountry((MTIncomeSpecificationRow)row, info);
					}
				},
				{
					"Producer",
					(DocumentGridRow row, DevExpress.XtraEditors.DXErrorProvider.ErrorInfo info) =>
					{
						ValidateProducer((MTIncomeSpecificationRow)row, info);
					}
				},
				{
					"Property1",
					(DocumentGridRow row, DevExpress.XtraEditors.DXErrorProvider.ErrorInfo info) =>
					{
						ValidateProperty1((MTIncomeSpecificationRow)row, info);
					}
				},
				{
					"Property2",
					(DocumentGridRow row, DevExpress.XtraEditors.DXErrorProvider.ErrorInfo info) =>
					{
						ValidateProperty2((MTIncomeSpecificationRow)row, info);
					}
				},
				{
					"Property1Caption",
					(DocumentGridRow row, DevExpress.XtraEditors.DXErrorProvider.ErrorInfo info) =>
					{
						ValidateProperty1Caption((MTIncomeSpecificationRow)row, info);
					}
				},
				{
					"Property2Caption",
					(DocumentGridRow row, DevExpress.XtraEditors.DXErrorProvider.ErrorInfo info) =>
					{
						ValidateProperty2Caption((MTIncomeSpecificationRow)row, info);
					}
				}
			});
		}

		protected static void ValidateGoodCode(MTIncomeSpecificationRow row, DevExpress.XtraEditors.DXErrorProvider.ErrorInfo info)
		{
			TradePropertyValidator.ValidateMaterialCode(row.MTCode, info, TradeAllDocDefinitions.MTIncome.Grids["MTList"].Columns["MTCode"]);
		}

		protected static void ValidateUnitCode(MTIncomeSpecificationRow row, DevExpress.XtraEditors.DXErrorProvider.ErrorInfo info)
		{
			TradePropertyValidator.ValidateGoodUnitCode(row.UnitCode, row.MTCode, info, TradeAllDocDefinitions.MTIncome.Grids["MTList"].Columns["UnitCode"]);
		}

		private static void ValidateQuantity(MTIncomeSpecificationRow row, DevExpress.XtraEditors.DXErrorProvider.ErrorInfo info)
		{
			PropertyValidator.PositiveValidate(row.Quantity, info, ArmSoft.SME.Common.Resources.Quantity);
		}

		private static void ValidateSumma(MTIncomeSpecificationRow row, DevExpress.XtraEditors.DXErrorProvider.ErrorInfo info)
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

		protected static void ValidateCountry(MTIncomeSpecificationRow row, DevExpress.XtraEditors.DXErrorProvider.ErrorInfo info)
		{
			if (!string.IsNullOrWhiteSpace(row.Country))
			{
				TradePropertyValidator.ValidateCountry(row.Country, info, TradeAllDocDefinitions.MTIncome.Grids["MTList"].Columns["Country"]);
			}
		}

		protected static void ValidateProducer(MTIncomeSpecificationRow row, DevExpress.XtraEditors.DXErrorProvider.ErrorInfo info)
		{
			if (!string.IsNullOrWhiteSpace(row.Producer))
			{
				TradePropertyValidator.ValidateProducer(row.Producer, info, TradeAllDocDefinitions.MTIncome.Grids["MTList"].Columns["Producer"]);
			}
		}

		protected static void ValidateProperty1(MTIncomeSpecificationRow row, DevExpress.XtraEditors.DXErrorProvider.ErrorInfo info)
		{
			if (!string.IsNullOrWhiteSpace(row.Property1))
			{
				TradePropertyValidator.ValidateProperty(row.Property1, row.MTCode, isProperty1: true, info, TradeAllDocDefinitions.MTIncome.Grids["MTList"].Columns["Property1"]);
			}
		}

		protected static void ValidateProperty2(MTIncomeSpecificationRow row, DevExpress.XtraEditors.DXErrorProvider.ErrorInfo info)
		{
			if (!string.IsNullOrWhiteSpace(row.Property2))
			{
				TradePropertyValidator.ValidateProperty(row.Property2, row.MTCode, isProperty1: false, info, TradeAllDocDefinitions.MTIncome.Grids["MTList"].Columns["Property2"]);
			}
		}

		protected static void ValidateProperty1Caption(MTIncomeSpecificationRow row, DevExpress.XtraEditors.DXErrorProvider.ErrorInfo info)
		{
			if (!string.IsNullOrWhiteSpace(row.Property1Caption))
			{
				TradePropertyValidator.ValidatePropertyValue(row.Property1Caption, row.MTCode, isProperty1: true, info, TradeAllDocDefinitions.MTIncome.Grids["MTList"].Columns["Property1Caption"]);
			}
		}

		protected static void ValidateProperty2Caption(MTIncomeSpecificationRow row, DevExpress.XtraEditors.DXErrorProvider.ErrorInfo info)
		{
			if (!string.IsNullOrWhiteSpace(row.Property2Caption))
			{
				TradePropertyValidator.ValidatePropertyValue(row.Property2Caption, row.MTCode, isProperty1: false, info, TradeAllDocDefinitions.MTIncome.Grids["MTList"].Columns["Property2Caption"]);
			}
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		private void OnMTCodeChanged(string? oldValue, string newValue)
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
			ResetForceValidationBit("Quantity", "Summa");
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

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		private void OnProperty1Changed(string value)
		{
			ResetForceValidationBit("Property1");
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		private void OnProperty1CaptionChanged(string value)
		{
			ResetForceValidationBit("Property1Caption");
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		private void OnProperty2Changed(string value)
		{
			ResetForceValidationBit("Property2");
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		private void OnProperty2CaptionChanged(string value)
		{
			ResetForceValidationBit("Property2Caption");
		}
	}

	public class MTIncomeFilteredSearchList : IResultList
	{
		private readonly ObservableCollection<MTIncomeSpecificationRow> _itemSource = new ObservableCollection<MTIncomeSpecificationRow>();

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
			int num2 = _itemSource.FindIndex((MTIncomeSpecificationRow x) => x.MTCode == mtCode);
			if (num2 == -1)
			{
				MaterialCacheRow item = TradeSettings.TradeCacheService.Material.GetItem(mtCode);
				MTIncomeSpecificationRow item2 = new MTIncomeSpecificationRow
				{
					MTID = item.Id,
					MTCode = item.MTCode,
					MTName = item.Caption,
					UnitCode = item.Unit,
					VAT = item.VAT,
					Country = item.Country,
					Producer = item.Producer,
					ExtraPercent = ((item.Extra == 0m) ? Settings.ParametersService.Extra() : item.Extra)
				};
				_itemSource.Add(item2);
				num2 = _itemSource.Count - 1;
			}
			_itemSource[num2].Quantity += num;
			return num2;
		}
	}

	protected delegate void RowInfoChangeEventHandler(MTIncomeSpecificationRow Row);

	private delegate void DelegateSetRowInfoToInfoPanel(MTIncomeSpecificationRow Row);

	private delegate void DelegateClearInfoPanel();

	protected bool _isMTIncomeDoc;

	protected Button _overHeads;

	protected BarEditItemExt _viewAvailAndTurn;

	protected DropDownCommentControl _baseDocumentControl;

	protected DropDownControl _storage;

	protected TextEditExt _inputValue;

	protected Button _codeEnter;

	protected Button _mtListLikeName;

	protected PartnerContractGroupControls _supplierContractContainer;

	protected TextEditExt _invoiceNumber;

	protected DateEditControl _invoiceDate;

	protected LayoutGroup _infoDisplay;

	protected TextEditExt _infoItemName;

	protected InfoDisplayControls _infoDisplayControls;

	protected TextEditExt _infoDescription;

	protected ComboBoxEditExt _comment;

	protected DropDownControl _owner;

	public DropDownControl _currCode;

	public ExchangeRateControl _currExChngRate;

	public DropDownControl _vatType;

	private CheckEditExt _includeVATInCost;

	public DropDownControl _priceListType;

	public DocumentGridControl _mtIncomeGrid;

	protected TableView _mtIncomeTableView;

	protected BarEditItemExt _filteredSearch;

	protected BarEditItemExt _actualQuantitiesCheck;

	protected BarEditItemExt _fillLatestPurchasePrices;

	protected BarEditItemExt _fillSalePrices;

	protected BarEditItemExt _saveSalePrices;

	public ObservableCollection<MTIncomeSpecificationRow> _mtIncomeList;

	protected DocumentGridColumn _codeCol;

	protected DocumentGridColumn _unitCodeCol;

	protected DocumentGridColumn _quantityCol;

	protected DocumentGridColumn _lastPurchasePriceCol;

	protected DocumentGridColumn _variancePercentCol;

	protected DocumentGridColumn _priceCol;

	protected DocumentGridColumn _discountCol;

	protected DocumentGridColumn _discountPriceCol;

	protected DocumentGridColumn _summaCol;

	protected DocumentGridColumn _extraPercentCol;

	protected DocumentGridColumn _salePriceCol;

	protected DocumentGridColumn _saleSummaCol;

	protected DocumentGridColumn _vatCol;

	protected DocumentGridColumn _useLifeCol;

	protected DocumentGridColumn _countryCol;

	protected DocumentGridColumn _producerCol;

	protected DocumentGridColumn _property1Col;

	protected DocumentGridColumn _property2Col;

	protected DocumentGridColumn _rowDescCol;

	public PopupCalcEditExt _discountPercent;

	protected PopupCalcEditExt _vatSumma;

	protected PopupCalcEditExt _totalSumma;

	protected DXTabControl _tabs;

	protected int _priceListTabControlIndex;

	protected TextEditExt _trust;

	protected DropDownControl _mediator;

	protected TextEditExt _machine;

	protected TextEditExt _chief;

	protected DropDownCommentControl _allower;

	protected DropDownCommentControl _response;

	protected bool _isChanged = true;

	protected Dictionary<(int, string), decimal> _availabilityByGoodId = new Dictionary<(int, string), decimal>();

	private SafeDocumentEventHandler<FocusedRowHandleChangedEventArgs> _focusedRowHandleChangedEvent;

	protected string _oldDocCurrCode;

	protected decimal _oldDocCurrExChngRate = 1m;

	protected decimal _oldDocCurrExChngBase = 1m;

	protected bool _validateTotalSumm;

	protected Guid _baseISN = Guid.Empty;

	protected Document _baseDocument;

	public int _maxRowId;

	protected bool _isAskQuestionConfirmSummChange = true;

	protected bool showCostCalculationReport = Settings.UserAccessService.GetProviderPermission(DataProviderTypeSE.GoodsCostCalculation.Value).AllowView;

	public bool _allowDiscountInPurchase = Settings.ParametersService.AllowDiscountInPurchase();

	protected string _rowAddMode = Settings.ParametersService.ScannerMode();

	protected bool _showLastPurchasePrices = Settings.ParametersService.ShowLastPurchasePrices();

	protected decimal _constVAT = Settings.ParametersService.VAT();

	public decimal UseDiscount;

	protected bool RaisePriceTypeEditValueChangeOnce;

	protected string SalePriceType = string.Empty;

	protected string RoundDgr = string.Empty;

	protected string RoundToUpper = string.Empty;

	protected BarButtonItem _barButtonPrintLabel;

	protected BarButtonItem _barButtonPriceFormation;

	protected BarButtonItem _barButtonImportFromFile;

	protected ReportBarButtonItem _barButtonGoodsOperationsJournal;

	protected ReportBarButtonItem _barButtonGoodsOperationsByPartiesJournal;

	protected ReportBarButtonItem _barButtonGoodsRetailOperationJournal;

	protected ReportBarButtonItem _barButtonPartnerOperationJournal;

	private ReportBarButtonItem _barButtonJumpTo;

	private BarSubItem _barSubItemReports = new BarSubItem();

	private BarSubItem _barSubItemJournals = new BarSubItem();

	private BarSubItem _barSubItemActions = new BarSubItem();

	private BarButtonItem _barButtonDocumentSettings = new BarButtonItem();

	protected BarSubItem ItemReports;

	protected BarButtonItem DocumentSettings;

	protected ReportBarButtonItem _barButtonCostCalculationWithoutVAT;

	protected ReportBarButtonItem _barButtonCostCalculationWithVAT;

	protected ReportBarButtonItem _barButtonCostCalculationCUR;

	public bool ShowGoodsListWithPrices = true;

	protected bool ShowLastPurchasePrices = Settings.ParametersService.ShowLastPurchasePrices();

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
			else if (_baseDocument.POType != DocumentTypeSE.ReceivedTaxInvoice.Value)
			{
				_baseDocumentControl.Text = ArmSoft.SME.Trade.UI.Helpers.DocumentHelpers.GetDocumentDescription((TradeDocument)_baseDocument, ArmSoft.SME.Common.Constants.Common.DocumentDescriptionMode.TypeDateNum);
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

	protected event RowInfoChangeEventHandler RowInfoChangeHandler;

	public MTIncomeBaseWindow()
	{
	}

	public MTIncomeBaseWindow(T doc)
		: base(doc)
	{
	}

	protected override void ConstructUI()
	{
		base.ConstructUI();
		if (!PO.StoredInDb)
		{
			SetReportBarButtonsEnable(isEnabled: false);
		}
		LayoutGroupExt container = AddLayoutGroup(null, LayoutGroupView.Group, Orientation.Horizontal);
		_date = AddDateControl(container, Definition.Fields["DocDate"]);
		_date.Margin = new Thickness(0.0, 0.0, 10.0, 0.0);
		_date.EditValueChanged += new SafeDocumentEventHandler<EditValueChangedEventArgs>(Date_EditValueChanged, this).Handler;
		_docNum = AddButtonEditControl(container, Definition.Fields["DocNum"], 1);
		_docNum.AllowMagicButton = true;
		_docNum.MagicToolTip = ArmSoft.SME.Common.Resources.DocNextNumbers;
		_docNum.MagicClick += new SafeDocumentEventHandler<MagicClickEventArgs>(DocNum_MagicClick, this).Handler;
		_docNum.EditValueChanged += new SafeDocumentEventHandler<EditValueChangedEventArgs>(DocNum_EditValueChanged, this).Handler;
		if (showCostCalculationReport)
		{
			_overHeads = AddButton(container, "", ArmSoft.SME.Trade.Common.Resources.GoodsCostCalculation);
			_overHeads.Click += new SafeDocumentEventHandler<RoutedEventArgs>(OverHeads_Click, this).Handler;
		}
		DocumentsJournal idv = new DocumentsJournal(this, ReportTypesSE.AllDocument, ((TradeDocDefinition)Definition).BaseDocTypes)
		{
			DocType = ((TradeDocDefinition)Definition).BaseDocTypes.First().ToString(),
			DocState = DocumentStateSE.DraftOrProcessed.ValueUI
		};
		_baseDocumentControl = AddDropDownCommentControl(null, ArmSoft.SME.Common.Resources.BaseDoc, isRequired: false, idv, 50, HorizontalAlignment.Stretch, "BaseDocumentControl");
		_baseDocumentControl.ShowClearButton = true;
		_baseDocumentControl.ShowSearchButton = true;
		_baseDocumentControl.IsTextEditable = false;
		_baseDocumentControl.ClickDropDown += new SafeDocumentEventHandler<DropDownControlEventArgs>(BaseDocumentControl_ClickDropDown, this).Handler;
		_baseDocumentControl.EditKeyValueChanged += new SafeDocumentEventHandler<EditKeyValueChangedEventArgs>(BaseDocumentControl_EditKeyValueChanged, this).Handler;
		_baseDocumentControl.BtnClearClicked += new SafeDocumentEventHandler<EditKeyValueChangedEventArgs>(BaseDocumentDelete_Click, this).Handler;
		_baseDocumentControl.BtnSearchClicked += new SafeDocumentEventHandler<EditKeyValueChangedEventArgs>(BaseDocumentSearch_Click, this).Handler;
		if (_isMTIncomeDoc)
		{
			_storage = AddDropDownControl(null, Definition.Fields["Storage"], new ArmSoft.SME.Trade.UI.DropDown.Storages(false), allowAdditionButtons: false, fieldWithCaption: true, HorizontalAlignment.Stretch);
		}
		else
		{
			_storage = AddDropDownControl(null, Definition.Fields["Storage"], new ArmSoft.SME.Trade.UI.DropDown.Storages(), allowAdditionButtons: false, fieldWithCaption: true, HorizontalAlignment.Stretch);
		}
		LayoutGroupExt container2 = AddLayoutGroup(null, LayoutGroupView.Group, Orientation.Horizontal);
		_inputValue = AddStringControl(container2, ArmSoft.SME.Trade.Common.Resources.InputBoxCaption + " (F2)", isRequired: false, 20, HorizontalAlignment.Stretch);
		string text = ArmSoft.SME.Trade.Common.Resources.cmdCodeEnter + " (F8)";
		_inputValue.NullText = ArmSoft.SME.Trade.Common.Resources.BarCodeNameAndCodeInputNullText;
		_inputValue.KeyDown += new SafeDocumentEventHandler<KeyEventArgs>(InputValue_KeyDown, this).Handler;
		_codeEnter = AddButton(container2, null, text, null, text, isTabStop: true);
		_codeEnter.Margin = new Thickness(0.0, 0.0, 18.0, 0.0);
		_codeEnter.Click += new SafeDocumentEventHandler<RoutedEventArgs>(OnCodeEnter_Click, this).Handler;
		string text2 = ArmSoft.SME.Trade.Common.Resources.cmdNameEnter + " (F9)";
		_mtListLikeName = AddButton(container2, null, text2, null, text2, isTabStop: true);
		_mtListLikeName.Margin = new Thickness(0.0, 0.0, 18.0, 0.0);
		_mtListLikeName.Click += new SafeDocumentEventHandler<RoutedEventArgs>(MTListLikeName_Click, this).Handler;
		_tabs = AddTabControl(null);
		LayoutGroup container3 = AddTabbedGroup(_tabs, ArmSoft.SME.Common.Resources.General, "General");
		_supplierContractContainer = AddPartnerContractGroupControls(container3, (PartnerContractGroupDefinition)Definition.Fields["PartnerContractGroup"], _date, null, null);
		_supplierContractContainer.PartnerCode.IsRequired = false;
		_supplierContractContainer.PartnerCodeEditValueChanged += new SafeDocumentEventHandler<EditValueChangedEventArgs>(PartnerCode_EditValueChanged, this).Handler;
		_supplierContractContainer.PartnerCodeEditAdditionalValueChanged += new SafeDocumentEventHandler<EventArgs>(PartnerCode_EditAdditionalValueChanged, this).Handler;
		_supplierContractContainer.ContractCodeEditValueChanged += new SafeDocumentEventHandler<EventArgs>(PartnerContractCode_EditValueChanged, this).Handler;
		LayoutGroupExt container4 = AddLayoutGroup(container3, LayoutGroupView.Group, Orientation.Horizontal);
		_invoiceNumber = AddStringControl(container4, Definition.Fields["InvoiceNumber"]);
		_invoiceDate = AddDateControl(container4, Definition.Fields["InvoiceDate"]);
		_comment = AddComboBoxControl(container3, Definition.Fields["Comment"], TradeSettings.TradeCacheService.StandardComments.GetCaptionsList(), HorizontalAlignment.Stretch);
		_owner = AddDropDownControl(container3, Definition.Fields["Owner"], new UsersFilter());
		_owner.CodeWidth = 12;
		_owner.ShowComment = false;
		_owner.InnerCodeEdit.Width = _owner.InnerCodeEdit.MinWidth;
		if (!Settings.LoginService.IsAdmin && Settings.ParametersService.DenyAccessToOtherOwnerDocs())
		{
			((LayoutItemWithRequired)(object)_owner.Parent).Visibility = Visibility.Collapsed;
		}
		LayoutGroup container5 = AddTabbedGroup(_tabs, ArmSoft.SME.Trade.Common.Resources.MTLIST, "MtList");
		_priceListTabControlIndex = _tabs.Items.Count - 1;
		LayoutGroupExt container6 = AddLayoutGroup(container5, LayoutGroupView.Group, Orientation.Horizontal);
		LayoutGroupExt container7 = AddLayoutGroup(container6, LayoutGroupView.Group, Orientation.Vertical);
		LayoutGroupExt container8 = AddLayoutGroup(container6, LayoutGroupView.Group, Orientation.Vertical);
		_currCode = AddDropDownControl(container7, Definition.Fields["CurrCode"], new Currency(shortMode: true), allowAdditionButtons: false, fieldWithCaption: true, HorizontalAlignment.Stretch);
		_currCode.EditValueChanged += new SafeDocumentEventHandler<EditValueChangedEventArgs>(CurrCode_EditValueChanged, this).Handler;
		_discountPercent = AddNumericControl(container8, Definition.Fields["Discount"], useCurrencyFormat: true, HorizontalAlignment.Right);
		_vatSumma = AddNumericControl(container8, Definition.Fields["SummVAT"], useCurrencyFormat: true, HorizontalAlignment.Right);
		_currExChngRate = AddExchangeRateControl(container7, Definition.Fields["ExchangeRate"]);
		_currExChngRate.EditValueChanged += new SafeDocumentEventHandler<EditValueChangedEventArgs>(CurrExChngRate_EditValueChanged, this).Handler;
		_totalSumma = AddNumericControl(container8, Definition.Fields["Summ"], useCurrencyFormat: true, HorizontalAlignment.Right);
		_vatType = AddDropDownControl(container7, Definition.Fields["VATType"], new VATTypes(), allowAdditionButtons: false, fieldWithCaption: true, HorizontalAlignment.Stretch);
		_includeVATInCost = AddBooleanControl(container7, Definition.Fields["IncludeVATInCost"]);
		_priceListType = AddDropDownControl(container7, Definition.Fields["PriceType"], new ArmSoft.SME.Trade.UI.DropDown.PriceTypes());
		_priceListType.EditValueChanged += new SafeDocumentEventHandler<EditValueChangedEventArgs>(PriceListType_EditValueChanged, this).Handler;
		_mtIncomeGrid = AddGrid(container5, Definition.Grids["MTList"], HorizontalAlignment.Stretch, VerticalAlignment.Stretch, isSupportDialogMode: true);
		_mtIncomeGrid.AddingRow += new SafeDocumentEventHandler<DocumentGridAddingRowEventArgs>(MTIncomeGrid_AddingRow, this).Handler;
		_mtIncomeGrid.DeletedRow += new SafeDocumentEventHandler<EventArgs>(MTIncomeGrid_DeletedRow, this).Handler;
		_mtIncomeGrid.DuplicatingRow += new SafeDocumentEventHandler<DocumentGridDuplicatingRowEventArgs>(MTIncomeGrid_DuplicatingRow, this).Handler;
		_mtIncomeGrid.AddingRowWithDialog += new SafeDocumentEventHandler<DocumentGridAddingRowEventArgs>(MTIncomeGrid_AddingRowWithDialog, this).Handler;
		_mtIncomeGrid.DuplicatingRowWithDialog += new SafeDocumentEventHandler<DocumentGridDuplicatingRowEventArgs>(MTIncomeGrid_DuplicatingRowWithDialog, this).Handler;
		_mtIncomeGrid.EditingRowWithDialog += new SafeDocumentEventHandler<DocumentGridEditingRowEventArgs>(MTIncomeGrid_EditingRowWithDialog, this).Handler;
		_mtIncomeGrid.BeforeAddNewDropDown += new SafeDocumentEventHandler<DocumentGridClickDropDownEventArgs>(MTIncomeGrid_BeforeAddNewDropDown, this).Handler;
		_mtIncomeGrid.ClickDropDown += new SafeDocumentEventHandler<DocumentGridClickDropDownEventArgs>(MTIncomeGrid_ClickDropDown, this).Handler;
		_mtIncomeGrid.MovingRowUp += new SafeDocumentEventHandler<DocumentGridMovingRowUpOrDownEventArgs>(MTIncomeGrid_MovingRowUp, this).Handler;
		_mtIncomeGrid.MovingRowDown += new SafeDocumentEventHandler<DocumentGridMovingRowUpOrDownEventArgs>(MTIncomeGrid_MovingRowDown, this).Handler;
		_mtIncomeGrid.RefreshButtons += new SafeDocumentEventHandler<EventArgs>(MTIncomeGrid_RefreshButtons, this).Handler;
		_viewAvailAndTurn = _mtIncomeGrid.AddButton(null, ArmSoft.SME.Trade.Common.Resources.AvailAndTurn, ArmSoft.SME.Trade.Common.Resources.AvailabilityAndTurn, 130.0);
		_viewAvailAndTurn.Click += new SafeDocumentEventHandler<RoutedEventArgs>(ViewAvailAndTurn_Click, this).Handler;
		_filteredSearch = _mtIncomeGrid.AddButton(ArmSoft.SME.Trade.Common.Resources.GroupAddition, ArmSoft.SME.Trade.Common.Resources.GroupAddition, ArmSoft.SME.Trade.Common.Resources.GroupAddition, 130.0);
		_filteredSearch.Click += new SafeDocumentEventHandler<RoutedEventArgs>(FilteredSearch_Click, this).Handler;
		if (Settings.ParametersService.ShowGoodsCostInReportsAndDocs())
		{
			BarButtonItem barButtonItem = new BarButtonItem
			{
				Content = ArmSoft.SME.Trade.Common.Resources.UpdateLatestPurchasePrices
			};
			barButtonItem.ItemClick += new SafeEventHandler<ItemClickEventArgs>(UpdateLatestPurchasePrices_ItemClick).Handler;
			BarButtonItem barButtonItem2 = new BarButtonItem
			{
				Content = ArmSoft.SME.Trade.Common.Resources.UpdateCurrentRowLatestPurchasePrices
			};
			barButtonItem2.ItemClick += new SafeEventHandler<ItemClickEventArgs>(UpdateCurrentRowLatestPurchasePrices_ItemClick).Handler;
			_fillLatestPurchasePrices = _mtIncomeGrid.AddSplitButton("", ArmSoft.SME.Trade.Common.Resources.FillLatestPurchasePrices, ArmSoft.SME.Trade.Common.Resources.FillLatestPurchasePrices, isTabStop: true, 16.0, 16.0, new _003C_003Ez__ReadOnlyArray<IBarItem>(new IBarItem[2] { barButtonItem, barButtonItem2 }), showGlyphsInPopupMenu: false, 265.0);
			_fillLatestPurchasePrices.Click += new SafeDocumentEventHandler<RoutedEventArgs>(FillLatestPurchasePrices_Click, this).Handler;
		}
		_actualQuantitiesCheck = _mtIncomeGrid.AddButton("cheked_all_16.svg", ArmSoft.SME.Trade.Common.Resources.ActualQuantitiesCheck, ArmSoft.SME.Trade.Common.Resources.ActualQuantitiesCheck, 300.0);
		if (!_isMTIncomeDoc)
		{
			_fillSalePrices = _mtIncomeGrid.AddButton("fill_down_16.svg", ArmSoft.SME.Trade.Common.Resources.SetSalePrices, ArmSoft.SME.Trade.Common.Resources.SetSalePrices, 180.0);
			_saveSalePrices = _mtIncomeGrid.AddButton(null, ArmSoft.SME.Trade.Common.Resources.SaveSalePrices, ArmSoft.SME.Trade.Common.Resources.SaveSalePrices, 180.0);
		}
		_mtIncomeTableView = _mtIncomeGrid.View;
		_mtIncomeTableView.CellValueChanged += new SafeDocumentEventHandler<CellValueChangedEventArgs>(MTIncomeTableView_CellValueChanged, this).Handler;
		_focusedRowHandleChangedEvent = new SafeDocumentEventHandler<FocusedRowHandleChangedEventArgs>(MTIncomeTableView_FocusedRowHandleChanged, this);
		_mtIncomeTableView.FocusedRowHandleChanged += _focusedRowHandleChangedEvent.Handler;
		_mtIncomeTableView.ShowingEditor += new SafeDocumentEventHandler<ShowingEditorEventArgs>(MTIncomeTableView_ShowingEditor, this).Handler;
		_mtIncomeGrid.AllowMoveUp = true;
		_mtIncomeGrid.AllowMoveDown = true;
		FormatCondition item = new FormatCondition
		{
			Expression = "[VariancePercent] <= 0.0m",
			FieldName = "VariancePercent",
			ApplyToRow = false,
			Format = ConditionalFormattingHelper.CreateDevExFormat(new UserDefinedFormat
			{
				IconUri = "accept_16.svg"
			})
		};
		FormatCondition item2 = new FormatCondition
		{
			Expression = "[VariancePercent] > 0.0m",
			FieldName = "VariancePercent",
			ApplyToRow = false,
			Format = ConditionalFormattingHelper.CreateDevExFormat(new UserDefinedFormat
			{
				IconUri = "warning_circle_16.svg"
			})
		};
		_mtIncomeTableView.FormatConditions.Add(item);
		_mtIncomeTableView.FormatConditions.Add(item2);
		_mtIncomeGrid.AddRowIndicator(35);
		_codeCol = _mtIncomeGrid.AddDropDownColumn(Definition.Grids["MTList"].Columns["MTCode"], new GoodsFilter(shortMode: true, ItemTypesSE.Goods, null, isShowOnlyInPriceList: true), allowAdditionButtons: true, isOwnProcessingClickDropDown: true);
		_codeCol.Width = 125.0;
		DocumentGridColumn documentGridColumn = _mtIncomeGrid.AddStringColumn(Definition.Grids["MTList"].Columns["MTName"]);
		documentGridColumn.AllowEditing = DefaultBoolean.False;
		documentGridColumn.Width = 225.0;
		_unitCodeCol = _mtIncomeGrid.AddDropDownColumn(Definition.Grids["MTList"].Columns["UnitCode"], new ArmSoft.SME.Trade.UI.DropDown.QntUnits(), allowAdditionButtons: false, isOwnProcessingClickDropDown: true);
		_mtIncomeGrid.AddStringColumn(Definition.Grids["MTList"].Columns["UnitBrief"]).AllowEditing = DefaultBoolean.False;
		_quantityCol = _mtIncomeGrid.AddNumberColumn(Definition.Grids["MTList"].Columns["Quantity"], showTotal: true);
		_lastPurchasePriceCol = _mtIncomeGrid.AddNumberColumn(Definition.Grids["MTList"].Columns["LastPurchasePrice"]);
		_lastPurchasePriceCol.AllowEditing = DefaultBoolean.False;
		_variancePercentCol = _mtIncomeGrid.AddNumberColumn(Definition.Grids["MTList"].Columns["VariancePercent"]);
		_variancePercentCol.AllowEditing = DefaultBoolean.False;
		SetLastPurchasePricesAndVariancePercentToColumns();
		_priceCol = _mtIncomeGrid.AddNumberColumn(Definition.Grids["MTList"].Columns["Price"]);
		_discountCol = _mtIncomeGrid.AddNumberColumn(Definition.Grids["MTList"].Columns["Discount"]);
		_discountPriceCol = _mtIncomeGrid.AddNumberColumn(Definition.Grids["MTList"].Columns["DiscountPrice"]);
		_summaCol = _mtIncomeGrid.AddNumberColumn(Definition.Grids["MTList"].Columns["Summa"], showTotal: true);
		if (!_isMTIncomeDoc)
		{
			_extraPercentCol = _mtIncomeGrid.AddNumberColumn(Definition.Grids["MTList"].Columns["ExtraPercent"]);
			_salePriceCol = _mtIncomeGrid.AddNumberColumn(Definition.Grids["MTList"].Columns["SalePrice"]);
			_saleSummaCol = _mtIncomeGrid.AddNumberColumn(Definition.Grids["MTList"].Columns["SaleSumma"], showTotal: true);
			_saleSummaCol.AllowEditing = DefaultBoolean.False;
		}
		_vatCol = _mtIncomeGrid.AddBooleanColumn(Definition.Grids["MTList"].Columns["VAT"]);
		_vatCol.Width = 45.0;
		_useLifeCol = _mtIncomeGrid.AddDateColumn(Definition.Grids["MTList"].Columns["UseLife"]);
		_useLifeCol.Visible = Settings.ParametersService.ShowPartyUSEABLEDATE();
		_useLifeCol.ShowInColumnChooser = _useLifeCol.Visible;
		_countryCol = _mtIncomeGrid.AddDropDownColumn(Definition.Grids["MTList"].Columns["Country"], new ArmSoft.SME.Trade.UI.DropDown.Countries());
		_countryCol.Visible = Settings.ParametersService.ShowPartyCountry();
		_countryCol.ShowInColumnChooser = _countryCol.Visible;
		_producerCol = _mtIncomeGrid.AddDropDownColumn(Definition.Grids["MTList"].Columns["Producer"], new Producer());
		_producerCol.Width = SizeCalculator.WidthForString(5);
		_producerCol.Visible = Settings.ParametersService.ShowPartyProducer();
		_producerCol.ShowInColumnChooser = _producerCol.Visible;
		ArmSoft.SME.Trade.UI.DropDown.PropertyList idv2 = new ArmSoft.SME.Trade.UI.DropDown.PropertyList();
		ArmSoft.SME.Trade.UI.DropDown.PropertyList idv3 = new ArmSoft.SME.Trade.UI.DropDown.PropertyList();
		_property1Col = _mtIncomeGrid.AddDropDownColumn(Definition.Grids["MTList"].Columns["Property1Caption"], idv2, allowAdditionButtons: true, isOwnProcessingClickDropDown: true);
		_property1Col.Width = SizeCalculator.WidthForString(Convert.ToInt32(25.0));
		_property1Col.Visible = Settings.ParametersService.ShowPartyProperty1();
		_property1Col.ShowInColumnChooser = _property1Col.Visible;
		_property2Col = _mtIncomeGrid.AddDropDownColumn(Definition.Grids["MTList"].Columns["Property2Caption"], idv3, allowAdditionButtons: true, isOwnProcessingClickDropDown: true);
		_property2Col.Width = SizeCalculator.WidthForString(Convert.ToInt32(25.0));
		_property2Col.Visible = Settings.ParametersService.ShowPartyProperty2();
		_property2Col.ShowInColumnChooser = _property2Col.Visible;
		_rowDescCol = _mtIncomeGrid.AddStringColumn(Definition.Grids["MTList"].Columns["RowDescription"]);
		foreach (GridColumn column in _mtIncomeGrid.Grid.Columns)
		{
			column.AllowSorting = DefaultBoolean.True;
			column.AllowColumnFiltering = DefaultBoolean.True;
			column.FilterPopupMode = FilterPopupMode.CheckedList;
		}
		_mtIncomeList = new ObservableCollection<MTIncomeSpecificationRow>();
		_infoDisplayControls = new InfoDisplayControls(this, DocumentTypeSE.MTIncome, isShowItemPictureOnInfoPanel: false, isShowStorageAvailabilityOnInfoPanel: true, isShowWholeAvailabilityOnInfoPanel: true, Settings.ParametersService.ShowGoodsCostInReportsAndDocs() && !ShowLastPurchasePrices, showDescriptionOnInfoPanel: true);
		_infoDisplayControls.ConstructUI(container5);
		_infoDisplay = _infoDisplayControls.InfoDisplay;
		_infoItemName = _infoDisplayControls.InfoItemName;
		_infoDescription = _infoDisplayControls.InfoDescription;
		_infoDisplay.Collapsed += new SafeDocumentEventHandler<EventArgs>(InfoDisplay_Collapsed, this).Handler;
		_supplierContractContainer.DocSummaControl = _totalSumma;
		_supplierContractContainer.DocCurrCodeControl = _currCode;
		LayoutGroup container9 = AddTabbedGroup(_tabs, ArmSoft.SME.Common.Resources.Additional, "Additional");
		_trust = AddStringControl(container9, Definition.Fields["Trust"], HorizontalAlignment.Stretch);
		_mediator = AddDropDownControl(container9, Definition.Fields["Mediator"], new ArmSoft.SME.Trade.UI.DropDown.Employees(), allowAdditionButtons: false, fieldWithCaption: true, HorizontalAlignment.Stretch);
		_machine = AddStringControl(container9, Definition.Fields["Machine"], HorizontalAlignment.Stretch);
		_chief = AddStringControl(container9, Definition.Fields["Chief"], HorizontalAlignment.Stretch);
		_allower = AddDropDownCommentControl(container9, Definition.Fields["Allower"], new ArmSoft.SME.Trade.UI.DropDown.Employees(), HorizontalAlignment.Stretch);
		_response = AddDropDownCommentControl(container9, Definition.Fields["Response"], new ArmSoft.SME.Trade.UI.DropDown.Employees(), HorizontalAlignment.Stretch);
		_discountPercent.EditValueChanged += new SafeDocumentEventHandler<EditValueChangedEventArgs>(DiscountPercent_EditValueChanged, this).Handler;
		_storage.EditValueChanged += new SafeDocumentEventHandler<EditValueChangedEventArgs>(Storage_EditValueChanged, this).Handler;
		_infoDisplay.IsCollapsed = _infoDisplayControls.InfoPanelCollapsed;
		_vatType.EditValueChanged += new SafeDocumentEventHandler<EditValueChangedEventArgs>(VATType_EditValueChanged, this).Handler;
		RowInfoChangeHandler += ChangeRowInfo;
	}

	private void UpdateLatestPurchasePrices_ItemClick(object sender, ItemClickEventArgs e)
	{
		SetLastPurchaseInfoToColumn();
	}

	private void UpdateCurrentRowLatestPurchasePrices_ItemClick(object sender, ItemClickEventArgs e)
	{
		SetLastPurchaseInfoToColumn((MTIncomeSpecificationRow)_mtIncomeGrid.Grid.CurrentItem);
	}

	private void FilteredSearch_Click(object sender, RoutedEventArgs e)
	{
		try
		{
			FilteredSearch filteredSearch = new FilteredSearch
			{
				FilterDataView = new GoodsGroups(showAsTree: true, filteredSearch: true)
			};
			if (!string.IsNullOrWhiteSpace(_priceListType.Code) && ShowGoodsListWithPrices)
			{
				filteredSearch.SearchDataView = new GoodsWithPrices(_date.EditValue.Value, _priceListType.Code, Name, "", ItemTypesSE.Goods.Value);
			}
			else
			{
				filteredSearch.SearchDataView = new GoodsFilter(shortMode: true, ItemTypesSE.Goods, "", isShowOnlyInPriceList: true);
			}
			if (!string.IsNullOrWhiteSpace(_supplierContractContainer.PartnerCode.Code))
			{
				if (!string.IsNullOrWhiteSpace(_priceListType.Code) && ShowGoodsListWithPrices)
				{
					filteredSearch.FilteredSearchDataView = new GoodsWithPrices(_date.EditValue.Value, _priceListType.Code, Name, _supplierContractContainer.PartnerCode.Code, ItemTypesSE.Goods.Value);
				}
				else
				{
					filteredSearch.FilteredSearchDataView = new GoodsFilter(shortMode: true, ItemTypesSE.Goods, "", isShowOnlyInPriceList: false, "", applyDefaultDataViewDefinition: true, _supplierContractContainer.PartnerCode.Code);
				}
			}
			filteredSearch.ResultsList = new MTIncomeFilteredSearchList();
			FilteredSearchWindow filteredSearchWindow = new FilteredSearchWindow(filteredSearch, this, isBasketMode: true);
			if (filteredSearchWindow.ShowDialog().Value)
			{
				foreach (MTIncomeSpecificationRow item3 in filteredSearchWindow.BasketResult)
				{
					string mTQntUnitByMTIDPriceTypeAndDocDate = TradeSettings.CreateTradeClient().CommonServices.GetMTQntUnitByMTIDPriceTypeAndDocDate(item3.MTID, _priceListType.Code, _date.EditValue.Value);
					if (!string.IsNullOrWhiteSpace(mTQntUnitByMTIDPriceTypeAndDocDate))
					{
						item3.UnitCode = mTQntUnitByMTIDPriceTypeAndDocDate;
					}
					QntUnitCacheRow item = TradeSettings.TradeCacheService.QntUnit.GetItem(item3.UnitCode);
					item3.UnitBrief = ((item == null) ? "" : item.Brief);
					item3.Price = TradeSettings.CreateTradeClient().CommonServices.GetPriceForGivenPriceListAndVATType(_priceListType.Code, _vatType.Code, item3.MTID, item3.UnitCode, _date.EditValue.Value, _currCode.Code);
					if (_allowDiscountInPurchase && _discountPercent.Value != 0m)
					{
						item3.Discount = _discountPercent.Value;
					}
					item3.DiscountPrice = (item3.Price - item3.Price * item3.Discount / 100m).RoundAs(2);
					item3.Summa = (item3.Quantity * item3.DiscountPrice).RoundAs(2);
					if (!_isMTIncomeDoc)
					{
						if (!string.IsNullOrWhiteSpace(_storage.Code))
						{
							StoragesCacheRow item2 = TradeSettings.TradeCacheService.Storage.GetItem(_storage.Code);
							if (item2 != null && item2.Retail)
							{
								UpdateSalePriceAndSaleSumma(item3);
								goto IL_033c;
							}
						}
						FillSalePriceAndSaleSumma(item3);
					}
					goto IL_033c;
					IL_033c:
					item3.RowID = _maxRowId;
					_mtIncomeList.Add(item3);
					_maxRowId++;
					if (Settings.ParametersService.ShowGoodsCostInReportsAndDocs() && ShowLastPurchasePrices)
					{
						SetLastPurchaseInfoToColumn(item3);
					}
				}
				_mtIncomeGrid.Grid.RefreshData();
				CalcSummAndSummVAT();
			}
			RowInfoChangeHandler?.Invoke((MTIncomeSpecificationRow)_mtIncomeTableView.Grid.CurrentItem);
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
			MTIncomeSpecificationRow mTIncomeSpecificationRow = (MTIncomeSpecificationRow)_mtIncomeTableView.Grid.CurrentItem;
			if (mTIncomeSpecificationRow != null)
			{
				new MTAvailAndTurnWindow(DialogWindowMode.ViewMode, this, _storage.Code, _supplierContractContainer.PartnerCode.Code, mTIncomeSpecificationRow.MTID, mTIncomeSpecificationRow.MTCode, mTIncomeSpecificationRow.UnitCode).Invoke();
			}
			else
			{
				new MTAvailAndTurnWindow(DialogWindowMode.ViewMode, this, _storage.Code, _supplierContractContainer.PartnerCode.Code, -1, string.Empty, string.Empty).Invoke();
			}
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	protected override void OnF9()
	{
		OnMTListLikeName_Click();
	}

	private void MTListLikeName_Click(object sender, RoutedEventArgs e)
	{
		OnMTListLikeName_Click();
	}

	private void OnMTListLikeName_Click()
	{
		if (_mtListLikeName == null || !_mtListLikeName.IsEnabled)
		{
			return;
		}
		_inputValue.DoValidate();
		try
		{
			string likeName = _inputValue.Text.Trim();
			if (!_date.DoValidate() || !_date.EditValue.HasValue)
			{
				ControlsHelper.SetFocus(_date);
				throw new ASException(ArmSoft.SME.Common.Resources.ValidateEmptyDate);
			}
			GoodsFilter goodsFilter = new GoodsFilter(shortMode: true, ItemTypesSE.Goods, "", isShowOnlyInPriceList: true, likeName);
			ModalBrowser modalBrowser = new ModalBrowser(goodsFilter)
			{
				Owner = this,
				WindowStartupLocation = WindowStartupLocation.CenterOwner
			};
			if (modalBrowser.Show())
			{
				string text = goodsFilter.ValueMember(modalBrowser.SelectedRowIndex);
				MaterialCacheRow item = TradeSettings.TradeCacheService.Material.GetItem(text);
				if (item == null)
				{
					ControlsHelper.SetFocus(_inputValue);
					throw new ASException(string.Format(ArmSoft.SME.Trade.Common.Resources.MaterialNotExist, text));
				}
				AddMTToList(item, 1m);
				if (_tabs.SelectedIndex != _priceListTabControlIndex)
				{
					_tabs.SelectedContainer = (DXTabItem)_tabs.Items[_priceListTabControlIndex];
				}
				_inputValue.Text = "";
				CalcSummAndSummVAT();
				_vatSumma.DoValidate();
				RowInfoChangeEventHandler rowInfoChangeEventHandler = RowInfoChangeHandler;
				if (rowInfoChangeEventHandler != null)
				{
					ObservableCollection<MTIncomeSpecificationRow> mtIncomeList = _mtIncomeList;
					rowInfoChangeEventHandler(mtIncomeList[mtIncomeList.Count - 1]);
				}
			}
			else
			{
				_inputValue.Text = "";
				ControlsHelper.SetFocus(_inputValue);
			}
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	protected override void OnF8()
	{
		CodeEnter_Click();
	}

	private void OnCodeEnter_Click(object sender, RoutedEventArgs e)
	{
		CodeEnter_Click();
	}

	private void CodeEnter_Click()
	{
		if (_codeEnter == null || !_codeEnter.IsEnabled)
		{
			return;
		}
		_inputValue.DoValidate();
		try
		{
			string text = _inputValue.Text.Trim();
			if (string.IsNullOrWhiteSpace(text))
			{
				if (!_date.DoValidate() || !_date.EditValue.HasValue)
				{
					ControlsHelper.SetFocus(_date);
					throw new Exception(ArmSoft.SME.Common.Resources.ValidateEmptyDate);
				}
				IDropDownView dropDownView = ((string.IsNullOrWhiteSpace(_priceListType.Code) || !ShowGoodsListWithPrices) ? ((IDropDownView)new GoodsFilter(shortMode: true, ItemTypesSE.Goods, "", isShowOnlyInPriceList: true)) : ((IDropDownView)new GoodsWithPrices(_date.EditValue.Value, _priceListType.Code, Name, "", ItemTypesSE.Goods.Value)));
				ModalBrowser modalBrowser = new ModalBrowser(dropDownView)
				{
					Owner = this,
					WindowStartupLocation = WindowStartupLocation.CenterOwner
				};
				if (!modalBrowser.Show())
				{
					_inputValue.Text = string.Empty;
					ControlsHelper.SetFocus(_inputValue);
					return;
				}
				text = dropDownView.ValueMember(modalBrowser.SelectedRowIndex);
			}
			if (text.Length > 20)
			{
				throw new ASException(string.Format(ArmSoft.SME.Trade.Common.Resources.ErrMTCodeLen, 20, text));
			}
			MaterialCacheRow item = TradeSettings.TradeCacheService.Material.GetItem(text);
			if (item == null || item.Type == GoodsTypesSE.Service)
			{
				ControlsHelper.SetFocus(_inputValue);
				throw new ASException(string.Format(ArmSoft.SME.Trade.Common.Resources.MTEgineInavidMaterial, text));
			}
			AddMTToList(item, 1m);
			if (_tabs.SelectedIndex != _priceListTabControlIndex)
			{
				_tabs.SelectedContainer = (DXTabItem)_tabs.Items[_priceListTabControlIndex];
			}
			_inputValue.Text = string.Empty;
			CalcSummAndSummVAT();
			_vatSumma.DoValidate();
			RowInfoChangeEventHandler rowInfoChangeEventHandler = RowInfoChangeHandler;
			if (rowInfoChangeEventHandler != null)
			{
				ObservableCollection<MTIncomeSpecificationRow> mtIncomeList = _mtIncomeList;
				rowInfoChangeEventHandler(mtIncomeList[mtIncomeList.Count - 1]);
			}
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	public override void OnScanner(string scannerData)
	{
		if (!_inputValue.IsReadOnly)
		{
			_inputValue.Text = scannerData;
			ScannerAdd();
		}
	}

	protected override void OnF2()
	{
		if (!_inputValue.IsReadOnly)
		{
			ControlsHelper.SetFocus(_inputValue);
		}
	}

	private void InputValue_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Invalid comparison between Unknown and I4
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if ((int)e.Key == 6 && (int)Keyboard.Modifiers == 0 && !_inputValue.IsReadOnly && !string.IsNullOrWhiteSpace(_inputValue.Text))
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
		_inputValue.DoValidate();
		try
		{
			string barCode = _inputValue.Text.Trim();
			MaterialCacheRow materialCacheRow = ArmSoft.SME.Trade.UI.Helpers.DocumentHelpers.TryGetItemByBarCode(ref barCode, out var qty, out var unitCode, out var _);
			if (materialCacheRow == null || materialCacheRow.Type == GoodsTypesSE.Service)
			{
				ControlsHelper.SetFocus(_inputValue);
				if (ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Common.Resources.FindeInUnitedProducts, ArmSoft.SME.Common.Resources.ProgramNameTrade, MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No) != MessageBoxResult.Yes)
				{
					return;
				}
				string text = ArmSoft.SME.Trade.UI.Helpers.DocumentHelpers.CreateNewGoodFromUnitedProdAndAddInDoc(barCode, this);
				materialCacheRow = TradeSettings.TradeCacheService.Material.GetItem(text);
				if (!string.IsNullOrWhiteSpace(text))
				{
					AddMTToList(materialCacheRow, qty, unitCode);
					_inputValue.Text = "";
					CalcSummAndSummVAT();
					_vatSumma.DoValidate();
					RowInfoChangeEventHandler rowInfoChangeEventHandler = RowInfoChangeHandler;
					if (rowInfoChangeEventHandler != null)
					{
						ObservableCollection<MTIncomeSpecificationRow> mtIncomeList = _mtIncomeList;
						rowInfoChangeEventHandler(mtIncomeList[mtIncomeList.Count - 1]);
					}
				}
			}
			else
			{
				if (_tabs.SelectedIndex != _priceListTabControlIndex)
				{
					_tabs.SelectedContainer = (DXTabItem)_tabs.Items[_priceListTabControlIndex];
				}
				AddMTToList(materialCacheRow, qty, unitCode);
				_inputValue.Text = "";
				CalcSummAndSummVAT();
				_vatSumma.DoValidate();
				RowInfoChangeHandler?.Invoke(_mtIncomeList[_mtIncomeList.Count - 1]);
			}
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	private void AddMTToList(MaterialCacheRow goodRow, decimal quantity, string unitCode = "")
	{
		if (_rowAddMode == "3")
		{
			AddMTToListNewRow(goodRow, quantity, string.IsNullOrWhiteSpace(unitCode) ? goodRow.Unit : unitCode);
		}
		else
		{
			AddNewRowOrMergeInner(goodRow, _rowAddMode == "2", quantity, string.IsNullOrWhiteSpace(unitCode) ? goodRow.Unit : unitCode);
		}
	}

	protected void AddNewRowOrMergeInner(MaterialCacheRow goodRow, bool forFocusedRow, decimal quantity, string unitCode = "")
	{
		MTIncomeSpecificationRow mTIncomeSpecificationRow = new MTIncomeSpecificationRow();
		if (forFocusedRow && _mtIncomeList.Count != 0)
		{
			mTIncomeSpecificationRow = (MTIncomeSpecificationRow)_mtIncomeTableView.Grid.CurrentItem;
			if (mTIncomeSpecificationRow == null)
			{
				return;
			}
		}
		for (int i = 0; i <= _mtIncomeList.Count - 1; i++)
		{
			if ((!forFocusedRow || mTIncomeSpecificationRow.RowID == _mtIncomeList[i].RowID) && goodRow.Id == _mtIncomeList[i].MTID && unitCode == _mtIncomeList[i].UnitCode)
			{
				FillMTIncGridRow(_mtIncomeList[i], goodRow, unitCode, quantity);
				int rowHandleByListIndex = _mtIncomeGrid.Grid.GetRowHandleByListIndex(i);
				POWindow<T>.SelectCellInGrid(_mtIncomeTableView, _mtIncomeGrid, rowHandleByListIndex, "Quantity");
				return;
			}
		}
		AddMTToListNewRow(goodRow, quantity, unitCode);
		RowInfoChangeHandler?.Invoke(mTIncomeSpecificationRow);
	}

	private void OverHeads_Click(object sender, RoutedEventArgs e)
	{
		if (StoredInDb && State != DocumentStateSE.Draft.Value && State != DocumentStateSE.Deleted.Value)
		{
			GoodsCostCalculation goodsCostCalculation = new GoodsCostCalculation();
			goodsCostCalculation.BaseISN = PO.ISN;
			goodsCostCalculation.AdditionalCaption = GetReportsAdditionalCaption();
			goodsCostCalculation.ShowSums = true;
			goodsCostCalculation.Show();
		}
		else
		{
			ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Trade.Common.Resources.ForReportMustSaveDoc);
		}
	}

	private void FillLatestPurchasePrices_Click(object sender, RoutedEventArgs e)
	{
		if (_date.EditValue.HasValue)
		{
			if (_mtIncomeList.Count == 0)
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
			MTIncomePriceAndDiscountPriceForListRequest mTIncomePriceAndDiscountPriceForListRequest = new MTIncomePriceAndDiscountPriceForListRequest
			{
				ISN = PO.ISN,
				CurrCode = _currCode.Code,
				PartId = partId,
				Date = _date.EditValue.Value
			};
			foreach (MTIncomeSpecificationRow mtIncome in _mtIncomeList)
			{
				MTByQntUnit item2 = new MTByQntUnit(mtIncome.MTID, mtIncome.UnitCode);
				if (!mTIncomePriceAndDiscountPriceForListRequest.Materials.Contains(item2))
				{
					mTIncomePriceAndDiscountPriceForListRequest.Materials.Add(item2);
				}
			}
			List<MaterialIncomeDiscountPrices> previousPricesList = TradeSettings.CreateTradeClient().CommonServices.GetMTIncomePriceAndDiscountPriceForList(mTIncomePriceAndDiscountPriceForListRequest);
			if (previousPricesList.Count != 0 && mTIncomePriceAndDiscountPriceForListRequest.PartId != -1 && previousPricesList.Any((MaterialIncomeDiscountPrices p) => p.DiscountPrice == 0m))
			{
				previousPricesList.RemoveAll((MaterialIncomeDiscountPrices p) => p.DiscountPrice == 0m);
				mTIncomePriceAndDiscountPriceForListRequest.PartId = -1;
				mTIncomePriceAndDiscountPriceForListRequest.Materials = mTIncomePriceAndDiscountPriceForListRequest.Materials.Where((MTByQntUnit m) => !previousPricesList.Any((MaterialIncomeDiscountPrices p) => p.MaterialID == m.MTID && p.UnitCode == m.QntUnit)).ToList();
				List<MaterialIncomeDiscountPrices> mTIncomePriceAndDiscountPriceForList = TradeSettings.CreateTradeClient().CommonServices.GetMTIncomePriceAndDiscountPriceForList(mTIncomePriceAndDiscountPriceForListRequest);
				previousPricesList.AddRange(mTIncomePriceAndDiscountPriceForList);
			}
			bool flag = !string.IsNullOrWhiteSpace(_storage.Code) && (TradeSettings.TradeCacheService.Storage.GetItem(_storage.Code)?.Retail ?? false);
			foreach (MTIncomeSpecificationRow row in _mtIncomeList)
			{
				if (row.MTID == -1)
				{
					continue;
				}
				MaterialIncomeDiscountPrices materialIncomeDiscountPrices = previousPricesList.FirstOrDefault((MaterialIncomeDiscountPrices x) => x.MaterialID == row.MTID && x.UnitCode == row.UnitCode);
				if (materialIncomeDiscountPrices == null)
				{
					continue;
				}
				row.Price = materialIncomeDiscountPrices.Price;
				row.DiscountPrice = materialIncomeDiscountPrices.DiscountPrice;
				row.Summa = (materialIncomeDiscountPrices.DiscountPrice * row.Quantity).RoundAs(2);
				row.Discount = materialIncomeDiscountPrices.Discount;
				if (!_isMTIncomeDoc)
				{
					if (flag)
					{
						UpdateSalePriceAndSaleSumma(row);
					}
					else
					{
						FillSalePriceAndSaleSumma(row);
					}
				}
				row.UnitBrief = TradeSettings.TradeCacheService.QntUnit.GetItem(row.UnitCode).Brief;
				row.ValidateWithoutResponse();
			}
			CalcSummAndSummVAT();
			if (Settings.ParametersService.ShowGoodsCostInReportsAndDocs())
			{
				if (ShowLastPurchasePrices)
				{
					SetVariancePercent();
				}
				else
				{
					CalcLastPurchasePricesAndVariancePercentAndSetToPanel((MTIncomeSpecificationRow)_mtIncomeGrid.Grid.CurrentItem);
				}
			}
			_mtIncomeGrid.Grid.RefreshData();
			_mtIncomeGrid.ConfigureButtons();
		}
		else
		{
			ControlsHelper.SetFocus(_date);
			ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Common.Resources.ValidateEmptyDate, Settings.ProgramName, MessageBoxButton.OK, MessageBoxImage.Exclamation);
		}
	}

	private void SetVariancePercent(MTIncomeSpecificationRow row = null)
	{
		if (row == null)
		{
			foreach (MTIncomeSpecificationRow mtIncome in _mtIncomeList)
			{
				if (mtIncome.LastPurchasePrice != 0m)
				{
					mtIncome.VariancePercent = ((mtIncome.DiscountPrice - mtIncome.LastPurchasePrice) / mtIncome.LastPurchasePrice * 100m).RoundAs(2);
					if (mtIncome.VariancePercent / 1000m >= 1m)
					{
						mtIncome.VariancePercent = 999.99m;
					}
					else if (mtIncome.VariancePercent / 1000m <= -1m)
					{
						mtIncome.VariancePercent = -99.99m;
					}
				}
				else
				{
					mtIncome.VariancePercent = 0m;
				}
			}
			return;
		}
		if (row.LastPurchasePrice != 0m)
		{
			row.VariancePercent = ((row.DiscountPrice - row.LastPurchasePrice) / row.LastPurchasePrice * 100m).RoundAs(2);
			if (row.VariancePercent / 1000m >= 1m)
			{
				row.VariancePercent = 999.99m;
			}
			else if (row.VariancePercent / 1000m <= -1m)
			{
				row.VariancePercent = -99.99m;
			}
		}
		else
		{
			row.VariancePercent = 0m;
		}
	}

	protected void UpdateSalePriceAndSaleSumma(MTIncomeSpecificationRow row)
	{
		row.SalePrice = CalculateSalePrice(row.DiscountPrice, row.ExtraPercent, row.VAT);
		row.SaleSumma = (row.SalePrice * row.Quantity).RoundAs(2);
	}

	protected void FillSalePriceAndSaleSumma(MTIncomeSpecificationRow row)
	{
		if (!string.IsNullOrWhiteSpace(SalePriceType))
		{
			if (row.SalePrice == 0m)
			{
				PricesDataProvider.DataRow price = TradeSettings.CreateTradeClient().CommonServices.GetPrice(SalePriceType, row.MTID, row.UnitCode, _date.EditValue.Value, _currCode.Code);
				if (price.Price != 0m)
				{
					row.SalePrice = price.Price;
					row.SaleSumma = (row.SalePrice * row.Quantity).RoundAs(2);
					string roundDegree = (string.IsNullOrWhiteSpace(RoundDgr) ? TradeSettings.CreateTradeClient().CommonServices.GetPriceTypeRoungDgr(SalePriceType) : RoundDgr);
					ArmSoft.SME.Trade.UI.Helpers.PriceHelper.GetPriceListRowVATType(SalePriceType, out var priceListRowVATType, out var _);
					if (priceListRowVATType == "1")
					{
						row.SalePrice += (row.SalePrice * _constVAT / 100m).RoundAs(2);
					}
					row.SalePrice = Methods.RoundPrice(row.SalePrice, roundDegree, RoundToUpper == "1");
					row.SaleSumma = (row.SalePrice * row.Quantity).RoundAs(2);
					row.ExtraPercent = ArmSoft.SME.Trade.UI.Helpers.PriceHelper.CalculateExtraPercent(row.DiscountPrice, row.SalePrice, row.VAT, _vatType.Code, _currCode.Code, new ExchangeRate(_currExChngRate.Base, _currExChngRate.Rate));
				}
				else
				{
					row.ExtraPercent = -100m;
				}
			}
			else
			{
				row.SaleSumma = (row.SalePrice * row.Quantity).RoundAs(2);
				row.ExtraPercent = ArmSoft.SME.Trade.UI.Helpers.PriceHelper.CalculateExtraPercent(row.DiscountPrice, row.SalePrice, row.VAT, _vatType.Code, _currCode.Code, new ExchangeRate(_currExChngRate.Base, _currExChngRate.Rate));
			}
		}
		else
		{
			UpdateSalePriceAndSaleSumma(row);
		}
	}

	private decimal CalculateSalePrice(decimal price, decimal extraPercent, bool rowVAT)
	{
		ExchangeRate exchangeRate = new ExchangeRate
		{
			Rate = _currExChngRate.Rate,
			Base = _currExChngRate.Base
		};
		if (_currCode.Code != "AMD" && _currExChngRate.Base != 0m)
		{
			price = ArmSoft.SME.Trade.Common.Helpers.PriceHelper.GetSumNNC(price, exchangeRate);
		}
		if (_vatType.Code == "1")
		{
			price += ArmSoft.SME.Trade.UI.Helpers.PriceHelper.CalcVATAmountByType(price, _vatType.Code, rowVAT);
		}
		return (price + price * extraPercent / 100m).RoundAs(2);
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
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_011e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0124: Unknown result type (might be due to invalid IL or missing references)
		//IL_013a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0140: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0213: Unknown result type (might be due to invalid IL or missing references)
		//IL_0219: Unknown result type (might be due to invalid IL or missing references)
		//IL_0301: Unknown result type (might be due to invalid IL or missing references)
		//IL_0307: Unknown result type (might be due to invalid IL or missing references)
		//IL_031d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0323: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_03cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_0467: Unknown result type (might be due to invalid IL or missing references)
		//IL_046d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0488: Unknown result type (might be due to invalid IL or missing references)
		//IL_048e: Unknown result type (might be due to invalid IL or missing references)
		base.AddItemsToRibbon();
		CreateRibbonPageGroupActions();
		CreateRibbonPageGroupReports();
		_barSubItemJournals.Content = "";
		_barSubItemJournals.Glyph = ImageService.GetImage("report_16.svg");
		_barSubItemJournals.LargeGlyph = ImageService.GetImage("report_32.svg");
		BarSubItem barSubItem = new BarSubItem();
		_barButtonJumpTo = new ReportBarButtonItem(DataProviderTypeSE.AllDocumentsJournal, showDialog: false, ArmSoft.SME.Common.Resources.JumpTo, null, "doc_JumpTo_16.svg", "doc_JumpTo_32.svg");
		_barButtonJumpTo.AddInBarSubItem(ref _ribbonPageGroupActions, ref barSubItem);
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
		_barButtonPrintLabel.ItemClick += new SafeDocumentEventHandler<ItemClickEventArgs>(BarButtonPrintLabel_ItemClick, this).Handler;
		_barManager.Items.Add(_barButtonPrintLabel);
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
		_barButtonPriceFormation.ItemClick += new SafeDocumentEventHandler<ItemClickEventArgs>(BarButtonPriceFormation_ItemClick, this).Handler;
		_barManager.Items.Add(_barButtonPriceFormation);
		barItemLink = _barButtonPriceFormation.CreateLink();
		barItemLink.MergeType = BarItemMergeType.Replace;
		_barSubItemActions.ItemLinks.Add(barItemLink);
		_ribbonPageGroupActions.ItemLinks.Add(_barButtonPriceFormation.CreateLink());
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
		_barButtonDocumentSettings.Content = ArmSoft.SME.Common.Resources.Settings;
		_barButtonDocumentSettings.Glyph = ImageService.GetImage("page_white_gear_16.svg");
		_barButtonDocumentSettings.LargeGlyph = ImageService.GetImage("page_white_gear_32.svg");
		_barButtonDocumentSettings.ItemClick += new SafeDocumentEventHandler<ItemClickEventArgs>(BarButtonDocumentSettings_ItemClick, this).Handler;
		_ribbonPageGroupActions.ItemLinks.Add(_barButtonDocumentSettings.CreateLink());
		if (showCostCalculationReport)
		{
			_barSubItemReports.Content = ArmSoft.SME.Trade.Common.Resources.GoodsCostCalculation;
			_barSubItemReports.Glyph = ImageService.GetImage("report_16.svg");
			_barSubItemReports.LargeGlyph = ImageService.GetImage("report_32.svg");
			_ribbonPageGroupReports.ItemLinks.Add(_barSubItemReports);
			_barButtonCostCalculationWithoutVAT = new ReportBarButtonItem(DataProviderTypeSE.GoodsCostCalculation, showDialog: false, ArmSoft.SME.Trade.Common.Resources.WithoutNDS_no_bracket);
			_barButtonCostCalculationWithoutVAT.AddInBarSubItem(ref _barSubItemReports);
			_barButtonCostCalculationWithVAT = new ReportBarButtonItem(DataProviderTypeSE.GoodsCostCalculation, showDialog: false, ArmSoft.SME.Trade.Common.Resources.WithNDS_no_bracket);
			_barButtonCostCalculationWithVAT.AddInBarSubItem(ref _barSubItemReports);
			_barButtonCostCalculationCUR = new ReportBarButtonItem(DataProviderTypeSE.GoodsCostCalculation, showDialog: false, Settings.ParametersService.MTCurr());
			_barButtonCostCalculationCUR.AddInBarSubItem(ref _barSubItemReports);
		}
		_barButtonGoodsOperationsJournal = new ReportBarButtonItem(DataProviderTypeSE.GoodsRepOperations.Value, showDialog: false, ArmSoft.SME.Trade.Common.Resources.MTOperations);
		_barButtonGoodsOperationsJournal.AddInBarSubItem(ref _ribbonPageGroupReports, ref _barSubItemJournals);
		if (!_isMTIncomeDoc)
		{
			_barButtonGoodsRetailOperationJournal = new ReportBarButtonItem(DataProviderTypeSE.MatRepOperationsRetail.Value, showDialog: false, ArmSoft.SME.Trade.Common.Resources.MTOperationsRetail);
			_barButtonGoodsRetailOperationJournal.AddInBarSubItem(ref _ribbonPageGroupReports, ref _barSubItemJournals);
		}
		_barButtonPartnerOperationJournal = new ReportBarButtonItem(DataProviderTypeSE.PartOpJournal.Value, showDialog: false, ArmSoft.SME.Trade.Common.Resources.PartnerOp);
		_barButtonPartnerOperationJournal.AddInBarSubItem(ref _ribbonPageGroupReports, ref _barSubItemJournals);
		_barButtonGoodsOperationsByPartiesJournal = new ReportBarButtonItem(DataProviderTypeSE.GoodsRepOperations.Value, showDialog: false, ArmSoft.SME.Trade.Common.Resources.GoodsOperationJournalByParty);
		_barButtonGoodsOperationsByPartiesJournal.AddInBarSubItem(ref _ribbonPageGroupReports, ref _barSubItemJournals);
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
		if (showCostCalculationReport)
		{
			ReportBarButtonItem barButtonCostCalculationWithoutVAT = _barButtonCostCalculationWithoutVAT;
			num = 3;
			List<PropertyWithValue> list2 = new List<PropertyWithValue>(num);
			CollectionsMarshal.SetCount(list2, num);
			Span<PropertyWithValue> span2 = CollectionsMarshal.AsSpan(list2);
			span2[0] = new PropertyWithValue("BaseISN", PO.ISN.ToString());
			span2[1] = new PropertyWithValue("ShowSums", true.ToString());
			span2[2] = new PropertyWithValue("AdditionalCaption", GetReportsAdditionalCaption());
			barButtonCostCalculationWithoutVAT.PropertyWithValues = list2;
			ReportBarButtonItem barButtonCostCalculationWithVAT = _barButtonCostCalculationWithVAT;
			num = 3;
			List<PropertyWithValue> list3 = new List<PropertyWithValue>(num);
			CollectionsMarshal.SetCount(list3, num);
			Span<PropertyWithValue> span3 = CollectionsMarshal.AsSpan(list3);
			span3[0] = new PropertyWithValue("BaseISN", PO.ISN.ToString());
			span3[1] = new PropertyWithValue("ShowSumsWithVAT", true.ToString());
			span3[2] = new PropertyWithValue("AdditionalCaption", GetReportsAdditionalCaption());
			barButtonCostCalculationWithVAT.PropertyWithValues = list3;
			ReportBarButtonItem barButtonCostCalculationCUR = _barButtonCostCalculationCUR;
			num = 3;
			List<PropertyWithValue> list4 = new List<PropertyWithValue>(num);
			CollectionsMarshal.SetCount(list4, num);
			Span<PropertyWithValue> span4 = CollectionsMarshal.AsSpan(list4);
			span4[0] = new PropertyWithValue("BaseISN", PO.ISN.ToString());
			span4[1] = new PropertyWithValue("ShowCurrSums", true.ToString());
			span4[2] = new PropertyWithValue("AdditionalCaption", GetReportsAdditionalCaption());
			barButtonCostCalculationCUR.PropertyWithValues = list4;
		}
		ReportBarButtonItem barButtonGoodsOperationsJournal = _barButtonGoodsOperationsJournal;
		num = 2;
		List<PropertyWithValue> list5 = new List<PropertyWithValue>(num);
		CollectionsMarshal.SetCount(list5, num);
		Span<PropertyWithValue> span5 = CollectionsMarshal.AsSpan(list5);
		span5[0] = new PropertyWithValue("DocISN", PO.ISN.ToString());
		span5[1] = new PropertyWithValue("AdditionalCaption", GetReportsAdditionalCaption());
		barButtonGoodsOperationsJournal.PropertyWithValues = list5;
		if (!_isMTIncomeDoc)
		{
			ReportBarButtonItem barButtonGoodsRetailOperationJournal = _barButtonGoodsRetailOperationJournal;
			num = 2;
			List<PropertyWithValue> list6 = new List<PropertyWithValue>(num);
			CollectionsMarshal.SetCount(list6, num);
			Span<PropertyWithValue> span6 = CollectionsMarshal.AsSpan(list6);
			span6[0] = new PropertyWithValue("DocISN", PO.ISN.ToString());
			span6[1] = new PropertyWithValue("AdditionalCaption", GetReportsAdditionalCaption());
			barButtonGoodsRetailOperationJournal.PropertyWithValues = list6;
		}
		ReportBarButtonItem barButtonPartnerOperationJournal = _barButtonPartnerOperationJournal;
		num = 3;
		List<PropertyWithValue> list7 = new List<PropertyWithValue>(num);
		CollectionsMarshal.SetCount(list7, num);
		Span<PropertyWithValue> span7 = CollectionsMarshal.AsSpan(list7);
		span7[0] = new PropertyWithValue("DocISN", PO.ISN.ToString());
		span7[1] = new PropertyWithValue("AdditionalCaption", GetReportsAdditionalCaption());
		span7[2] = new PropertyWithValue("ShowLinks", true.ToString());
		barButtonPartnerOperationJournal.PropertyWithValues = list7;
		ReportBarButtonItem barButtonGoodsOperationsByPartiesJournal = _barButtonGoodsOperationsByPartiesJournal;
		num = 5;
		List<PropertyWithValue> list8 = new List<PropertyWithValue>(num);
		CollectionsMarshal.SetCount(list8, num);
		Span<PropertyWithValue> span8 = CollectionsMarshal.AsSpan(list8);
		span8[0] = new PropertyWithValue("DocISN", PO.ISN.ToString());
		span8[1] = new PropertyWithValue("ShowLinks", true.ToString());
		span8[2] = new PropertyWithValue("AdditionalCaption", GetReportsAdditionalCaption());
		span8[3] = new PropertyWithValue("ShowParties", true.ToString());
		span8[4] = new PropertyWithValue("ForByPartiesJournal", true.ToString());
		barButtonGoodsOperationsByPartiesJournal.PropertyWithValues = list8;
	}

	protected void BarButtonPrintLabel_ItemClick(object sender, ItemClickEventArgs e)
	{
		if (_mtIncomeList.Count > 0)
		{
			FillDocument(new StoreRequest());
			new PrintLabelDialog(this, PO).Invoke();
		}
	}

	private void BarButtonDocumentSettings_ItemClick(object sender, ItemClickEventArgs e)
	{
		DialogWindow dialogWindow = new DialogWindow();
		bool showLastPurchasePrices = ShowLastPurchasePrices;
		dialogWindow.Caption = ArmSoft.SME.Common.Resources.Settings;
		dialogWindow.Owner = this;
		dialogWindow.AddBooleanControl(null, "bShowGoodsListWIthPrices", ArmSoft.SME.Trade.Common.Resources.ShowGoodsListWithPrices).IsChecked = ShowGoodsListWithPrices;
		if (Settings.ParametersService.ShowGoodsCostInReportsAndDocs())
		{
			dialogWindow.AddBooleanControl(null, "bShowLastPurchasePrices", ArmSoft.SME.Trade.Common.Resources.ShowLastPurchasePrices).IsChecked = ShowLastPurchasePrices;
		}
		if (Convert.ToBoolean(dialogWindow.ShowDialog()))
		{
			ShowGoodsListWithPrices = (bool)dialogWindow["bShowGoodsListWIthPrices"];
			if (Settings.ParametersService.ShowGoodsCostInReportsAndDocs())
			{
				ShowLastPurchasePrices = (bool)dialogWindow["bShowLastPurchasePrices"];
				Settings.ParametersService.ShowLastPurchasePrices_set(ShowLastPurchasePrices, Settings.LoginService.UserId);
			}
		}
		if (showLastPurchasePrices != ShowLastPurchasePrices)
		{
			SetLastPurchasePricesAndVariancePercentToColumns();
			_infoDisplayControls.isShowPriceAvailabilityOnInfoPanel = !ShowLastPurchasePrices;
			if (_mtIncomeList.Count != 0)
			{
				_infoDisplayControls.FillAllInfo(!ShowLastPurchasePrices);
				SetAvailabilityInfoToPanel((MTIncomeSpecificationRow)_mtIncomeGrid.Grid.CurrentItem);
			}
			if (!ShowLastPurchasePrices && _mtIncomeList.Count != 0)
			{
				CalcLastPurchasePricesAndVariancePercentAndSetToPanel((MTIncomeSpecificationRow)_mtIncomeGrid.Grid.CurrentItem);
			}
		}
		_mtIncomeGrid.Grid.RefreshData();
	}

	private void BarButtonPriceFormation_ItemClick(object sender, ItemClickEventArgs e)
	{
		if (_mtIncomeList.Count <= 0)
		{
			return;
		}
		List<PricesFormationWindow.GoodsRow> list = new List<PricesFormationWindow.GoodsRow>();
		List<SelectedMaterialsRow> list2 = new List<SelectedMaterialsRow>();
		foreach (MTIncomeSpecificationRow mtIncome in _mtIncomeList)
		{
			MaterialCacheRow item = TradeSettings.TradeCacheService.Material.GetItem(mtIncome.MTCode);
			list.Add(new PricesFormationWindow.GoodsRow(mtIncome.MTID, mtIncome.MTCode, mtIncome.UnitCode, item.AltUnit));
			SelectedMaterialsRow selectedMaterialsRow = new SelectedMaterialsRow
			{
				RowNum = mtIncome.RowNum,
				MTID = mtIncome.MTID,
				MTCode = mtIncome.MTCode,
				MTName = mtIncome.MTName
			};
			if (mtIncome.UnitCode != item.Unit)
			{
				MaterialCacheRow.QntUnitRow qntUnit = TradeSettings.TradeCacheService.Material.GetQntUnit(mtIncome.MTID, mtIncome.UnitCode);
				MaterialCacheRow.QntUnitRow qntUnit2 = TradeSettings.TradeCacheService.Material.GetQntUnit(mtIncome.MTID, item.Unit);
				if (qntUnit != null && qntUnit2 != null)
				{
					selectedMaterialsRow.QntUnit = qntUnit2.QntUnit;
					selectedMaterialsRow.QntUnitName = TradeSettings.TradeCacheService.QntUnit.GetItem(qntUnit2.QntUnit).Brief;
					if (qntUnit.Coef != 0m)
					{
						selectedMaterialsRow.PurchasePrice = (mtIncome.Price * qntUnit2.Coef / qntUnit.Coef).RoundAs(2);
						selectedMaterialsRow.SalePrice = (mtIncome.SalePrice * qntUnit2.Coef / qntUnit.Coef).RoundAs(2);
						selectedMaterialsRow.DiscountPrice = (mtIncome.DiscountPrice * qntUnit2.Coef / qntUnit.Coef).RoundAs(2);
					}
				}
			}
			else
			{
				selectedMaterialsRow.QntUnit = mtIncome.UnitCode;
				selectedMaterialsRow.QntUnitName = mtIncome.UnitBrief;
				selectedMaterialsRow.PurchasePrice = mtIncome.Price;
				selectedMaterialsRow.SalePrice = mtIncome.SalePrice;
				selectedMaterialsRow.DiscountPrice = mtIncome.DiscountPrice;
			}
			list2.Add(selectedMaterialsRow);
		}
		string dialogName = "PricesFormation" + Name + (_isMTIncomeDoc ? "" : "Retail");
		new PricesFormationWindow(list, dialogName, showPurchasePriceRow: true, !_isMTIncomeDoc, !_isMTIncomeDoc, _currCode.Code, list2, this, showDiscountPriceRow: true).Invoke();
	}

	private void BarButtonImportFromFile_ItemClick(object sender, ItemClickEventArgs e)
	{
		if (!_date.IsReadOnly && _date.DoValidate() && (_mtIncomeList.Count <= 0 || ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Trade.Common.Resources.LoadMaterialsList, ArmSoft.SME.Trade.Common.Resources.ProgramName, MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No) != MessageBoxResult.No))
		{
			_mtIncomeList.Clear();
			_maxRowId = 0;
			TerminalHelper.TerminalImporter terminalImporter = new TerminalHelper.TerminalImporter(this);
			if (terminalImporter.ImportFromFile((decimal quantity, MaterialCacheRow goodRow, MaterialCacheRow.BarCodeRow goodBarcodeRow) =>
			{
				AddMTToListNewRow(goodRow, quantity, goodBarcodeRow.QntUnit);
			}))
			{
				CalcSummAndSummVAT();
				terminalImporter.ShowReport();
			}
		}
	}

	protected override void BarButtonExportXML_ItemClick(object sender, ItemClickEventArgs e)
	{
		ValidateWithNoneAndAlsoFillDocumentForPrint(isForExportToXML: true);
		string fileNameFromExportDialog = Settings.CommonCoreHelper.GetFileNameFromExportDialog(".xml", "XML Files (.xml)|*.xml", "Export_" + DateTime.Now.ToString("MMdd_HHmm"));
		if (string.IsNullOrWhiteSpace(fileNameFromExportDialog))
		{
			return;
		}
		using FileStream fileStream = new FileStream(fileNameFromExportDialog, FileMode.Create);
		int num = 1;
		List<DocumentExchangeRow> list = new List<DocumentExchangeRow>(num);
		CollectionsMarshal.SetCount(list, num);
		CollectionsMarshal.AsSpan(list)[0] = new DocumentExchangeRow
		{
			DocType = PO.POType,
			ISN = PO.ISN
		};
		List<DocumentExchangeRow> list2 = list;
		foreach (Guid basedOvExpenseISN in TradeSettings.CreateTradeClient().CommonServices.GetBasedOvExpenseISNList(PO.ISN))
		{
			list2.Add(new DocumentExchangeRow
			{
				DocType = DocumentTypeSE.MTOverheadExp,
				ISN = basedOvExpenseISN
			});
		}
		fileStream.Write(Settings.CreateApiClient().Exchange.ExportDocument(list2));
	}

	private void AddMTToListNewRow(MaterialCacheRow sourceRow, decimal quantity, string unitCode)
	{
		MTIncomeSpecificationRow mTIncomeSpecificationRow = new MTIncomeSpecificationRow();
		FillMTIncGridRow(mTIncomeSpecificationRow, sourceRow, unitCode, quantity);
		mTIncomeSpecificationRow.RowID = _maxRowId;
		_mtIncomeList.Add(mTIncomeSpecificationRow);
		_maxRowId++;
		int rowHandleByListIndex = _mtIncomeGrid.Grid.GetRowHandleByListIndex(_mtIncomeList.Count - 1);
		POWindow<T>.SelectCellInGrid(_mtIncomeTableView, _mtIncomeGrid, rowHandleByListIndex, "Quantity");
		_mtIncomeGrid.ConfigureButtons();
		RowInfoChangeHandler?.Invoke(mTIncomeSpecificationRow);
	}

	private void FillMTIncGridRow(MTIncomeSpecificationRow mtIncGridRow, MaterialCacheRow goodsRow, string unitCode, decimal quantity = 1m)
	{
		mtIncGridRow.UnitCode = unitCode;
		QntUnitCacheRow item = TradeSettings.TradeCacheService.QntUnit.GetItem(mtIncGridRow.UnitCode);
		mtIncGridRow.UnitBrief = ((item == null) ? "" : item.Brief);
		if (goodsRow != null)
		{
			if (!_isMTIncomeDoc)
			{
				mtIncGridRow.ExtraPercent = ((goodsRow.Extra == 0m) ? Settings.ParametersService.Extra() : goodsRow.Extra);
			}
			mtIncGridRow.MTCode = goodsRow.MTCode;
			mtIncGridRow.MTID = goodsRow.Id;
			mtIncGridRow.MTName = goodsRow.Caption;
			mtIncGridRow.VAT = goodsRow.VAT;
			if (string.IsNullOrEmpty(mtIncGridRow.Country))
			{
				mtIncGridRow.Country = goodsRow.Country;
			}
			if (string.IsNullOrEmpty(mtIncGridRow.Producer))
			{
				mtIncGridRow.Producer = goodsRow.Producer;
			}
		}
		mtIncGridRow.Quantity += quantity;
		if (mtIncGridRow.Price == 0m && _date.DoValidate())
		{
			mtIncGridRow.Price = TradeSettings.CreateTradeClient().CommonServices.GetPriceForGivenPriceListAndVATType(_priceListType.Code, _vatType.Code, mtIncGridRow.MTID, mtIncGridRow.UnitCode, _date.EditValue.Value, _currCode.Code);
		}
		if (Settings.ParametersService.AllowDiscountInPurchase())
		{
			mtIncGridRow.Discount = _discountPercent.Value;
		}
		mtIncGridRow.DiscountPrice = (mtIncGridRow.Price - mtIncGridRow.Price * mtIncGridRow.Discount / 100m).RoundAs(2);
		mtIncGridRow.Summa = (mtIncGridRow.Quantity * mtIncGridRow.DiscountPrice).RoundAs(2);
		if (Settings.ParametersService.ShowGoodsCostInReportsAndDocs() && ShowLastPurchasePrices)
		{
			SetLastPurchaseInfoToColumn(mtIncGridRow);
		}
		if (_isMTIncomeDoc)
		{
			return;
		}
		if (!string.IsNullOrWhiteSpace(_storage.Code))
		{
			StoragesCacheRow item2 = TradeSettings.TradeCacheService.Storage.GetItem(_storage.Code);
			if (item2 != null && item2.Retail)
			{
				UpdateSalePriceAndSaleSumma(mtIncGridRow);
				return;
			}
		}
		FillSalePriceAndSaleSumma(mtIncGridRow);
	}

	private void SetLastPurchaseInfoToColumn(MTIncomeSpecificationRow row = null)
	{
		int num = -1;
		if (!string.IsNullOrWhiteSpace(_supplierContractContainer.PartnerCode.Code))
		{
			PartnerCacheRow item = TradeSettings.TradeCacheService.Partner.GetItem(_supplierContractContainer.PartnerCode.Code);
			if (item != null)
			{
				num = item.Id;
			}
		}
		if (row == null)
		{
			MTIncomePriceAndDiscountPriceForListRequest mTIncomePriceAndDiscountPriceForListRequest = new MTIncomePriceAndDiscountPriceForListRequest
			{
				ISN = PO.ISN,
				CurrCode = _currCode.Code,
				PartId = num,
				Date = _date.EditValue.Value
			};
			foreach (MTIncomeSpecificationRow mtIncome in _mtIncomeList)
			{
				MTByQntUnit item2 = new MTByQntUnit(mtIncome.MTID, mtIncome.UnitCode);
				if (!mTIncomePriceAndDiscountPriceForListRequest.Materials.Contains(item2))
				{
					mTIncomePriceAndDiscountPriceForListRequest.Materials.Add(item2);
				}
			}
			List<MaterialIncomeDiscountPrices> mTIncomePriceAndDiscountPriceForList = TradeSettings.CreateTradeClient().CommonServices.GetMTIncomePriceAndDiscountPriceForList(mTIncomePriceAndDiscountPriceForListRequest);
			if (mTIncomePriceAndDiscountPriceForList.Count != 0 && mTIncomePriceAndDiscountPriceForListRequest.PartId != -1 && mTIncomePriceAndDiscountPriceForList.FirstOrDefault().DiscountPrice == 0m)
			{
				mTIncomePriceAndDiscountPriceForListRequest.PartId = -1;
				mTIncomePriceAndDiscountPriceForList = TradeSettings.CreateTradeClient().CommonServices.GetMTIncomePriceAndDiscountPriceForList(mTIncomePriceAndDiscountPriceForListRequest);
			}
			foreach (MTIncomeSpecificationRow item3 in _mtIncomeList)
			{
				MaterialIncomeDiscountPrices materialIncomeDiscountPrices = mTIncomePriceAndDiscountPriceForList.FirstOrDefault((MaterialIncomeDiscountPrices x) => x.MaterialID == item3.MTID && x.UnitCode == item3.UnitCode);
				if (materialIncomeDiscountPrices != null && item3.MTID != -1 && _date.EditValue.HasValue)
				{
					item3.LastPurchasePrice = materialIncomeDiscountPrices.DiscountPrice;
				}
				else
				{
					item3.LastPurchasePrice = 0m;
				}
			}
			SetVariancePercent();
		}
		else
		{
			MaterialIncomeDiscountPrices mTIncomePriceAndDiscountPrice = TradeSettings.CreateTradeClient().CommonServices.GetMTIncomePriceAndDiscountPrice(row.MTID, row.UnitCode, PO.ISN, _currCode.Code, num, _date.EditValue.Value);
			if (num != -1 && mTIncomePriceAndDiscountPrice.DiscountPrice == 0m)
			{
				mTIncomePriceAndDiscountPrice = TradeSettings.CreateTradeClient().CommonServices.GetMTIncomePriceAndDiscountPrice(row.MTID, row.UnitCode, PO.ISN, _currCode.Code, -1, _date.EditValue.Value);
			}
			if (row.MTID != -1 && _date.EditValue.HasValue && mTIncomePriceAndDiscountPrice != null)
			{
				row.LastPurchasePrice = mTIncomePriceAndDiscountPrice.DiscountPrice;
			}
			else
			{
				row.LastPurchasePrice = 0m;
			}
			SetVariancePercent(row);
		}
		_mtIncomeGrid.Grid.RefreshData();
	}

	protected override void CreateChildDocument(short childDocType, T baseDoc)
	{
		base.CreateChildDocument(childDocType, baseDoc);
		switch (childDocType)
		{
		case 1:
		case 3:
		case 5:
		case 8:
		case 10:
		case 13:
		case 14:
			CreateBasedOnDocHelper.CreateDocument((DocumentTypeSE)(SmartEnum<DocumentTypeSE, short>)childDocType, PO.ISN, DVPID);
			break;
		}
	}

	protected void CalcLastPurchasePricesAdnVariancePercent(MTIncomeSpecificationRow row = null)
	{
		int num = -1;
		if (!string.IsNullOrWhiteSpace(_supplierContractContainer.PartnerCode.Code))
		{
			PartnerCacheRow item = TradeSettings.TradeCacheService.Partner.GetItem(_supplierContractContainer.PartnerCode.Code);
			if (item != null)
			{
				num = item.Id;
			}
		}
		if (row == null)
		{
			MTIncomePriceAndDiscountPriceForListRequest mTIncomePriceAndDiscountPriceForListRequest = new MTIncomePriceAndDiscountPriceForListRequest
			{
				ISN = PO.ISN,
				CurrCode = _currCode.Code,
				PartId = num,
				Date = _date.EditValue.Value
			};
			foreach (MTIncomeSpecificationRow mtIncome in _mtIncomeList)
			{
				MTByQntUnit item2 = new MTByQntUnit(mtIncome.MTID, mtIncome.UnitCode);
				if (!mTIncomePriceAndDiscountPriceForListRequest.Materials.Contains(item2))
				{
					mTIncomePriceAndDiscountPriceForListRequest.Materials.Add(item2);
				}
			}
			List<MaterialIncomeDiscountPrices> mTIncomePriceAndDiscountPriceForList = TradeSettings.CreateTradeClient().CommonServices.GetMTIncomePriceAndDiscountPriceForList(mTIncomePriceAndDiscountPriceForListRequest);
			if (mTIncomePriceAndDiscountPriceForList.Count != 0 && mTIncomePriceAndDiscountPriceForListRequest.PartId != -1 && mTIncomePriceAndDiscountPriceForList.FirstOrDefault().DiscountPrice == 0m)
			{
				mTIncomePriceAndDiscountPriceForListRequest.PartId = -1;
				mTIncomePriceAndDiscountPriceForList = TradeSettings.CreateTradeClient().CommonServices.GetMTIncomePriceAndDiscountPriceForList(mTIncomePriceAndDiscountPriceForListRequest);
			}
			foreach (MTIncomeSpecificationRow item3 in _mtIncomeList)
			{
				MaterialIncomeDiscountPrices materialIncomeDiscountPrices = mTIncomePriceAndDiscountPriceForList.FirstOrDefault((MaterialIncomeDiscountPrices x) => x.MaterialID == item3.MTID && x.UnitCode == item3.UnitCode);
				if (materialIncomeDiscountPrices != null && item3.MTID != -1 && _date.EditValue.HasValue && materialIncomeDiscountPrices.MaterialHasPrice)
				{
					item3.LastPurchasePrice = materialIncomeDiscountPrices.DiscountPrice;
				}
				else
				{
					item3.LastPurchasePrice = 0m;
				}
			}
			CalcVariancePercent();
		}
		else
		{
			MaterialIncomeDiscountPrices mTIncomePriceAndDiscountPrice = TradeSettings.CreateTradeClient().CommonServices.GetMTIncomePriceAndDiscountPrice(row.MTID, row.UnitCode, PO.ISN, _currCode.Code, num, _date.EditValue.Value);
			if (num != -1 && mTIncomePriceAndDiscountPrice.DiscountPrice == 0m)
			{
				mTIncomePriceAndDiscountPrice = TradeSettings.CreateTradeClient().CommonServices.GetMTIncomePriceAndDiscountPrice(row.MTID, row.UnitCode, PO.ISN, _currCode.Code, -1, _date.EditValue.Value);
			}
			if (mTIncomePriceAndDiscountPrice != null && row.MTID != -1 && _date.EditValue.HasValue && mTIncomePriceAndDiscountPrice.MaterialHasPrice)
			{
				row.LastPurchasePrice = mTIncomePriceAndDiscountPrice.DiscountPrice;
			}
			else
			{
				row.LastPurchasePrice = 0m;
			}
			CalcVariancePercent(row);
		}
		_mtIncomeGrid.Grid.RefreshData();
	}

	protected void CalcVariancePercent(MTIncomeSpecificationRow row = null)
	{
		if (row == null)
		{
			foreach (MTIncomeSpecificationRow mtIncome in _mtIncomeList)
			{
				if (mtIncome.LastPurchasePrice != 0m)
				{
					mtIncome.VariancePercent = ((mtIncome.DiscountPrice - mtIncome.LastPurchasePrice) / mtIncome.LastPurchasePrice * 100m).RoundAs(2);
					if (mtIncome.VariancePercent / 1000m >= 1m)
					{
						mtIncome.VariancePercent = 999.99m;
					}
					else if (mtIncome.VariancePercent / 1000m <= -1m)
					{
						mtIncome.VariancePercent = -99.99m;
					}
				}
				else
				{
					mtIncome.VariancePercent = 0m;
				}
			}
			return;
		}
		if (row.LastPurchasePrice != 0m)
		{
			row.VariancePercent = ((row.DiscountPrice - row.LastPurchasePrice) / row.LastPurchasePrice * 100m).RoundAs(2);
			if (row.VariancePercent / 1000m >= 1m)
			{
				row.VariancePercent = 999.99m;
			}
			else if (row.VariancePercent / 1000m <= -1m)
			{
				row.VariancePercent = -99.99m;
			}
		}
		else
		{
			row.VariancePercent = 0m;
		}
	}

	public void SetLastPurchasePricesAndVariancePercentToColumns()
	{
		_lastPurchasePriceCol.Visible = Settings.ParametersService.ShowGoodsCostInReportsAndDocs() && ShowLastPurchasePrices;
		_variancePercentCol.Visible = Settings.ParametersService.ShowGoodsCostInReportsAndDocs() && ShowLastPurchasePrices;
		_lastPurchasePriceCol.ShowInColumnChooser = _lastPurchasePriceCol.Visible;
		_variancePercentCol.ShowInColumnChooser = _variancePercentCol.Visible;
		if (_mtIncomeList != null && Settings.ParametersService.ShowGoodsCostInReportsAndDocs() && ShowLastPurchasePrices)
		{
			CalcLastPurchasePricesAdnVariancePercent();
		}
	}

	private void CalcLastPurchasePricesAndVariancePercentAndSetToPanel(MTIncomeSpecificationRow row)
	{
		decimal num = 0m;
		decimal num2 = 0m;
		int partId = -1;
		if (!string.IsNullOrWhiteSpace(_supplierContractContainer.PartnerCode.Code))
		{
			partId = TradeSettings.TradeCacheService.Partner.GetItem(_supplierContractContainer.PartnerCode.Code)?.Id ?? (-1);
		}
		MaterialIncomeDiscountPrices mTIncomePriceAndDiscountPrice = TradeSettings.CreateTradeClient().CommonServices.GetMTIncomePriceAndDiscountPrice(row.MTID, row.UnitCode, PO.ISN, _currCode.Code, partId, _date.EditValue.Value);
		if (mTIncomePriceAndDiscountPrice != null && row.MTID != -1 && _date.EditValue.HasValue && mTIncomePriceAndDiscountPrice.MaterialHasPrice)
		{
			row.UnitBrief = TradeSettings.TradeCacheService.QntUnit.GetItem(row.UnitCode).Brief;
			num = mTIncomePriceAndDiscountPrice.Price.RoundAs(2);
			if (row.DiscountPrice != 0m)
			{
				num2 = ((!(num != 0m)) ? 0m : ((row.DiscountPrice - num) / num * 100m).RoundAs(2));
				if (num2 / 1000m >= 1m)
				{
					num2 = 999.99m;
				}
				else if (num2 / 1000m <= -1m)
				{
					num2 = -99.99m;
				}
			}
			_infoDisplayControls.SetPrice(num);
			_infoDisplayControls.SetVariance(num2);
		}
		else
		{
			_infoDisplayControls.SetStringPrice(ArmSoft.SME.Common.Resources.NothingWasFound);
			_infoDisplayControls.SetVariance(num2);
		}
	}

	private void SetAvailabilityInfoToPanel(MTIncomeSpecificationRow row)
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

	protected override void OnNew()
	{
		base.OnNew();
		if (BaseDocument == null || BaseDocument.POType != 26)
		{
			_date.EditValue = Settings.HostingEnvironment.CurrentDate;
		}
		_owner.Code = Settings.LoginService.UserName;
		if (PO.MTList.Count == 0)
		{
			if (BaseDocument == null)
			{
				_invoiceDate.EditValue = Settings.HostingEnvironment.CurrentDate;
			}
			if (BaseDocument != null && (BaseDocument.POType == 14 || BaseDocument.POType == 16 || BaseDocument.POType == 21))
			{
				return;
			}
			string text = Settings.ParametersService.DefaultStorage();
			if (!string.IsNullOrWhiteSpace(text))
			{
				if (_isMTIncomeDoc)
				{
					StoragesCacheRow item = TradeSettings.TradeCacheService.Storage.GetItem(text);
					if ((item == null || !item.Retail) && TradeSettings.CreateTradeClient().CommonServices.IsStorageAvailable(text))
					{
						_storage.Code = text;
						StoragesCacheRow item2 = TradeSettings.TradeCacheService.Storage.GetItem(text);
						if (_storage != null)
						{
							_response.Text = item2.Response;
						}
					}
				}
				else if (TradeSettings.CreateTradeClient().CommonServices.IsStorageAvailable(text))
				{
					_storage.Code = text;
				}
			}
			_isAskQuestionConfirmSummChange = false;
			_priceListType.Code = Settings.ParametersService.DefaultBuyingPrice();
			if (string.IsNullOrWhiteSpace(_vatType.Code))
			{
				_vatType.Code = Settings.ParametersService.VATType();
			}
			_isAskQuestionConfirmSummChange = true;
			if (BaseDocument != null && BaseDocument.POType == DocumentTypeSE.ReceivedTaxInvoice.Value)
			{
				PartnerCacheRow partnerCacheRow = TradeSettings.TradeCacheService.Partner.GetPartnersByTaxCode(((ReceivedTaxInvoice)BaseDocument).SupplierTIN).FirstOrDefault((PartnerCacheRow x) => x.Supplier);
				if (partnerCacheRow != null)
				{
					_supplierContractContainer.PartnerCode.Code = partnerCacheRow.PartCode;
					PartnerCacheRow.ContractRow defaultContract = TradeSettings.TradeCacheService.Partner.GetDefaultContract(partnerCacheRow);
					if (defaultContract != null)
					{
						_supplierContractContainer.Contract.Code = defaultContract.Contract;
					}
				}
				if (((ReceivedTaxInvoice)BaseDocument).SerialNumber.InList("Բ", "B"))
				{
					_vatType.Code = "3";
				}
			}
			else
			{
				string text2 = Settings.ParametersService.DefaultSupplier();
				if (!string.IsNullOrWhiteSpace(text2))
				{
					PartnerCacheRow item3 = TradeSettings.TradeCacheService.Partner.GetItem(text2);
					if (item3 != null && item3.Supplier)
					{
						_supplierContractContainer.PartnerCode.Code = text2;
						PartnerCacheRow.ContractRow defaultContract2 = TradeSettings.TradeCacheService.Partner.GetDefaultContract(item3);
						if (defaultContract2 != null)
						{
							_supplierContractContainer.Contract.Code = defaultContract2.Contract;
						}
					}
				}
			}
			_chief.EditValue = Settings.ParametersService.ChiefAccountantName();
		}
		else if (!string.IsNullOrWhiteSpace(_priceListType.Code))
		{
			PriceListType_EditValueChangedForLoad();
		}
	}

	protected override void OnNewAfterSave()
	{
		base.OnNewAfterSave();
		_mtIncomeList.Clear();
		_maxRowId = 0;
		CalcSummAndSummVAT();
		_owner.Code = Settings.LoginService.UserName;
		SetReportBarButtonsEnable(isEnabled: false);
	}

	protected override void OnEdit()
	{
		base.OnEdit();
		_setFocusOnActivation = _docNum;
		_supplierContractContainer.CalculatePartDebtRem();
	}

	protected void CalcSummAndSummVAT()
	{
		decimal num = 0m;
		decimal num2 = 0m;
		if (_vatType.Code.InList("1", "2"))
		{
			foreach (MTIncomeSpecificationRow mtIncome in _mtIncomeList)
			{
				num += ArmSoft.SME.Trade.UI.Helpers.PriceHelper.CalcVATAmountByType(mtIncome.Summa, _vatType.Code, mtIncome.VAT);
				num2 += mtIncome.Summa;
			}
			_vatSumma.EditValue = num;
			_totalSumma.EditValue = num2;
			if (_vatType.Code == "1")
			{
				_totalSumma.Value += _vatSumma.Value;
			}
		}
		else
		{
			foreach (MTIncomeSpecificationRow mtIncome2 in _mtIncomeList)
			{
				num2 += mtIncome2.Summa;
			}
			_totalSumma.EditValue = num2;
			_vatSumma.EditValue = 0m;
		}
		CalculatePartSumma();
	}

	protected override void SetControlsReadonly()
	{
		base.SetControlsReadonly();
		if (_viewMode != WindowViewMode.View)
		{
			_vatSumma.IsReadOnly = true;
			_totalSumma.IsReadOnly = true;
			bool flag = State == DocumentStateSE.Draft.Value || State == DocumentStateSE.Deleted.Value;
			bool flag2 = _viewMode == WindowViewMode.Edit && !flag;
			_date.IsReadOnly = flag2;
			_baseDocumentControl.IsReadOnly = flag2;
			_storage.IsReadOnly = flag2;
			_inputValue.IsReadOnly = flag2;
			_codeEnter.IsEnabled = !flag2;
			_mtListLikeName.IsEnabled = !flag2;
			_currCode.IsReadOnly = flag2;
			_currExChngRate.IsReadOnly = flag2 || _currCode.Code == "AMD";
			_vatType.IsReadOnly = flag2;
			_includeVATInCost.IsReadOnly = flag2 || _vatType.Code.InList(VatTypesSE.CostWithoutVAT.Value, VatTypesSE.CostZeroVAT.Value);
			_priceListType.IsReadOnly = flag2;
			_discountPercent.IsReadOnly = flag2;
			_mtIncomeGrid.IsReadOnly = flag2;
		}
	}

	private void MTIncomeGrid_RefreshButtons(object sender, EventArgs e)
	{
		_viewAvailAndTurn.IsEnabled = Settings.UserAccessService.GetProviderPermission(DataProviderTypeSE.MatRepAvailability).AllowView || Settings.UserAccessService.GetProviderPermission(DataProviderTypeSE.MatRepAvailabilityTreeView).AllowView || Settings.UserAccessService.GetProviderPermission(DataProviderTypeSE.GoodsRepOperations).AllowView || Settings.UserAccessService.GetProviderPermission(DataProviderTypeSE.MatRepSalesAnalysis).AllowView;
		_filteredSearch.IsEnabled = !_mtIncomeGrid.IsReadOnly && _mtIncomeGrid.AllowAdd;
		_actualQuantitiesCheck.IsEnabled = !_mtIncomeGrid.IsReadOnly && _mtIncomeGrid.AllowAdd;
		if (!_isMTIncomeDoc)
		{
			_saveSalePrices.IsEnabled = Settings.UserAccessService.GetProviderPermission(DataProviderTypeSE.MTPrices).AllowOperation;
		}
	}

	protected override void SetReportBarButtonsEnable(bool isEnabled)
	{
		base.SetReportBarButtonsEnable(isEnabled);
		_barButtonGoodsOperationsJournal.IsEnabled = isEnabled;
		_barButtonGoodsOperationsByPartiesJournal.IsEnabled = isEnabled;
		if (!_isMTIncomeDoc)
		{
			_barButtonGoodsRetailOperationJournal.IsEnabled = isEnabled;
		}
		_barButtonPartnerOperationJournal.IsEnabled = isEnabled;
		_barButtonJumpTo.IsEnabled = isEnabled;
		_barButtonGoodsOperationsByPartiesJournal.IsEnabled = isEnabled;
		if (showCostCalculationReport)
		{
			_barButtonCostCalculationWithoutVAT.IsEnabled = isEnabled;
			_barButtonCostCalculationWithVAT.IsEnabled = isEnabled;
			_barButtonCostCalculationCUR.IsEnabled = isEnabled;
		}
	}

	protected override void LoadDataToControls()
	{
		base.LoadDataToControls();
		DialogValues values = Settings.DialogService.GetValues("SalePriceSaveOrLoadDialog");
		if (values.Values != null)
		{
			values.Values.TryGetValue("PRICELISTTYPE", out SalePriceType);
			values.Values.TryGetValue("ROUNDDGR", out RoundDgr);
			values.Values.TryGetValue("ROUNDTOUPPER", out RoundToUpper);
		}
		_baseISN = PO.BaseISN;
		if (_baseISN == Guid.Empty)
		{
			BaseDocument = null;
		}
		else
		{
			BaseDocument = Settings.DocumentService.Load(_baseISN);
		}
		UseDiscount = 0m;
		_mtIncomeList.Clear();
		_focusedRowHandleChangedEvent.HandleOnLoadingDocumentToControls = true;
		foreach (MTIncomeBase.MTIncomeSpecificationRow mT in PO.MTList)
		{
			MTIncomeSpecificationRow mTIncomeSpecificationRow = new MTIncomeSpecificationRow
			{
				MTCode = mT.MTCode,
				MTID = mT.MTID,
				MTName = mT.MTName,
				UnitCode = mT.UnitCode,
				UnitBrief = mT.UnitBrief,
				Quantity = mT.Quantity,
				RowNum = mT.RowNum,
				Price = mT.Price,
				VariancePercent = mT.VariancePercent,
				LastPurchasePrice = mT.LastPurchasePrice,
				Discount = mT.Discount,
				DiscountPrice = mT.DiscountPrice,
				SalePrice = mT.SalePrice,
				SaleSumma = mT.SaleSumma,
				Summa = mT.Summa,
				ExtraPercent = mT.ExtraPercent,
				VAT = (mT.VAT.HasValue && mT.VAT.Value),
				UseLife = mT.UseLife,
				Producer = mT.Producer,
				Country = mT.Country,
				Property1 = mT.Property1,
				Property2 = mT.Property2,
				Property1Caption = mT.Property1Caption,
				Property2Caption = mT.Property2Caption,
				RowID = mT.RowID,
				RowDescription = mT.RowDescription
			};
			_mtIncomeList.Add(mTIncomeSpecificationRow);
			UseDiscount += Math.Abs(mTIncomeSpecificationRow.Discount);
		}
		_maxRowId = PO.MaxRowID;
		_mtIncomeGrid.ItemsSource = _mtIncomeList;
		_isAskQuestionConfirmSummChange = true;
		if (!_allowDiscountInPurchase && UseDiscount == 0m)
		{
			((LayoutItemWithRequired)(object)_discountPercent.Parent).Visibility = Visibility.Collapsed;
			_discountCol.Visible = false;
			_discountCol.ShowInColumnChooser = false;
			_discountPriceCol.Visible = false;
			_discountPriceCol.ShowInColumnChooser = false;
		}
		_allowPurchaseWith0PriceOr100Percent = Settings.ParametersService.AllowPurchaseWith0PriceOr100Percent() || _mtIncomeList.Any((MTIncomeSpecificationRow row) => row.Summa == 0m);
		_totalSumma.IsRequired = !_allowPurchaseWith0PriceOr100Percent;
		if (!(_baseISN == Guid.Empty) && !StoredInDb && _viewMode != WindowViewMode.Copy)
		{
			CreateBasedOnDoc();
		}
		if (Settings.ParametersService.ShowGoodsCostInReportsAndDocs() && ShowLastPurchasePrices)
		{
			SetLastPurchaseInfoToColumn();
		}
		if (string.IsNullOrWhiteSpace(_currCode.Code) || _currCode.Code == "AMD" || !_currCode.DoValidate())
		{
			_priceCol.Header = ArmSoft.SME.Common.Resources.Price;
			_summaCol.Header = ArmSoft.SME.Common.Resources.Summa;
			_currExChngRate.Rate = 1m;
			_currExChngRate.Base = 1m;
		}
		else
		{
			_priceCol.Header = ArmSoft.SME.Common.Resources.FCPrice;
			_summaCol.Header = ArmSoft.SME.Common.Resources.FCSumma;
		}
		VATType_EditValueChangedForLoad();
		_supplierContractContainer.ValueChangedForLoadDoc();
		CalculatePartSumma();
		RowInfoChangeHandler?.Invoke((MTIncomeSpecificationRow)_mtIncomeTableView.Grid.CurrentItem);
		ReportBarButtonItem barButtonJumpTo = _barButtonJumpTo;
		int num = 4;
		List<PropertyWithValue> list = new List<PropertyWithValue>(num);
		CollectionsMarshal.SetCount(list, num);
		Span<PropertyWithValue> span = CollectionsMarshal.AsSpan(list);
		span[0] = new PropertyWithValue("DocISN", PO.ISN.ToString());
		span[1] = new PropertyWithValue("StartDate", PO.DocDate.ToString());
		span[2] = new PropertyWithValue("EndDate", PO.DocDate.ToString());
		span[3] = new PropertyWithValue("SelectedRowDocISN", PO.ISN.ToString());
		barButtonJumpTo.PropertyWithValues = list;
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

	private void BaseDocumentControl_EditKeyValueChanged(object sender, EditKeyValueChangedEventArgs e)
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

	protected void CreateBasedOnDoc()
	{
		_mtIncomeList.Clear();
		_maxRowId = 0;
		switch (BaseDocument.POType)
		{
		case 26:
		{
			ReceivedTaxInvoice receivedTaxInvoice = (ReceivedTaxInvoice)BaseDocument;
			bool flag = receivedTaxInvoice.Type == Convert.ToInt16("3");
			PartnerCacheRow partnerCacheRow = TradeSettings.TradeCacheService.Partner.GetList((PartnerCacheRow p) => p.TaxCode == receivedTaxInvoice.SupplierTIN && p.Supplier).FirstOrDefault();
			if (partnerCacheRow != null)
			{
				_supplierContractContainer.PartnerCode.Code = partnerCacheRow.PartCode;
				PartnerCacheRow.ContractRow defaultContract2 = TradeSettings.TradeCacheService.Partner.GetDefaultContract(partnerCacheRow);
				_supplierContractContainer.Contract.Code = defaultContract2?.Contract;
			}
			_date.EditValue = receivedTaxInvoice.DocDate;
			if (receivedTaxInvoice.Series.InList("Բ", "B"))
			{
				if (!_vatType.Code.InList(VatTypesSE.CostWithoutVAT.Value, VatTypesSE.CostZeroVAT.Value))
				{
					_vatType.Code = VatTypesSE.CostWithoutVAT.Value;
				}
			}
			else if (!_vatType.Code.InList(VatTypesSE.CostWithVAT.Value, VatTypesSE.CostWithInVAT.Value))
			{
				_vatType.Code = VatTypesSE.CostWithVAT.Value;
			}
			_invoiceNumber.Text = receivedTaxInvoice.Series + receivedTaxInvoice.TaxNumber;
			_invoiceDate.EditValue = receivedTaxInvoice.InvoiceDate;
			_trust.Text = receivedTaxInvoice.MediatorLicenseNumber;
			_comment.Text = receivedTaxInvoice.AdditionalInfo;
			foreach (TaxDocument.SpecificationRow item7 in receivedTaxInvoice.Items)
			{
				if (string.IsNullOrWhiteSpace(item7.ItemCode) || !(item7.ItemType == ItemTypesSE.Goods.Value))
				{
					continue;
				}
				MTIncomeSpecificationRow mTIncomeSpecificationRow3 = new MTIncomeSpecificationRow
				{
					RowID = _maxRowId,
					Summa = item7.Sum.RoundAs(2),
					Quantity = Math.Truncate(item7.Quantity * 1000m) / 1000m,
					VAT = (item7.SumVAT > 0m)
				};
				MaterialCacheRow item3 = TradeSettings.TradeCacheService.Material.GetItem(item7.ItemCode);
				if (item3 != null)
				{
					mTIncomeSpecificationRow3.MTID = item3.Id;
					mTIncomeSpecificationRow3.MTCode = item3.MTCode;
					mTIncomeSpecificationRow3.MTName = item3.Caption;
					mTIncomeSpecificationRow3.UnitCode = item7.UnitCodeFromDirectory;
					mTIncomeSpecificationRow3.UnitBrief = item7.UnitBriefFromDirectory;
				}
				if (_vatType.Code == "2")
				{
					mTIncomeSpecificationRow3.Summa += item7.SumVAT.RoundAs(2);
				}
				if (flag)
				{
					mTIncomeSpecificationRow3.Summa += item7.Excise.RoundAs(2);
				}
				if (partnerCacheRow != null)
				{
					PartnerGoodCacheRow itemFromMapping = TradeSettings.TradeCacheService.PartnerGood.GetItemFromMapping(partnerCacheRow.Id, item7.ItemExternalCode);
					if (itemFromMapping != null && itemFromMapping.Coeff != 0m && itemFromMapping.Coeff != 1m)
					{
						mTIncomeSpecificationRow3.Quantity = (mTIncomeSpecificationRow3.Quantity * itemFromMapping.Coeff).RoundAs(3);
					}
				}
				if (mTIncomeSpecificationRow3.Quantity != 0m)
				{
					mTIncomeSpecificationRow3.DiscountPrice = (mTIncomeSpecificationRow3.Summa / mTIncomeSpecificationRow3.Quantity).RoundAs(2);
				}
				if (_allowDiscountInPurchase)
				{
					mTIncomeSpecificationRow3.Discount = item7.Discount.RoundAs(2);
					if (mTIncomeSpecificationRow3.Discount == 100m || mTIncomeSpecificationRow3.Discount == 0m)
					{
						mTIncomeSpecificationRow3.Discount = 0m;
						mTIncomeSpecificationRow3.Price = mTIncomeSpecificationRow3.DiscountPrice;
					}
					else
					{
						mTIncomeSpecificationRow3.Price = (100m * mTIncomeSpecificationRow3.DiscountPrice / (100m - mTIncomeSpecificationRow3.Discount)).RoundAs(2);
					}
				}
				else
				{
					mTIncomeSpecificationRow3.Discount = 0m;
					mTIncomeSpecificationRow3.Price = mTIncomeSpecificationRow3.DiscountPrice;
				}
				if (!_isMTIncomeDoc)
				{
					mTIncomeSpecificationRow3.ExtraPercent = ((item3 == null || item3.Extra == 0m) ? Settings.ParametersService.Extra() : item3.Extra);
					if (!string.IsNullOrWhiteSpace(_storage.Code))
					{
						StoragesCacheRow item4 = TradeSettings.TradeCacheService.Storage.GetItem(_storage.Code);
						if (item4 != null && item4.Retail)
						{
							UpdateSalePriceAndSaleSumma(mTIncomeSpecificationRow3);
							goto IL_0582;
						}
					}
					FillSalePriceAndSaleSumma(mTIncomeSpecificationRow3);
				}
				goto IL_0582;
				IL_0582:
				_mtIncomeList.Add(mTIncomeSpecificationRow3);
				_maxRowId++;
			}
			break;
		}
		case 32:
		{
			_comment.EditValue = BaseDocument.Comment;
			MTPurchaseOrder mTPurchaseOrder = (MTPurchaseOrder)BaseDocument;
			PartnerCacheRow itemById3 = TradeSettings.TradeCacheService.Partner.GetItemById(mTPurchaseOrder.PartnerID);
			_supplierContractContainer.PartnerCode.Code = itemById3?.PartCode;
			PartnerCacheRow.ContractRow defaultContract3 = TradeSettings.TradeCacheService.Partner.GetDefaultContract(itemById3);
			_supplierContractContainer.Contract.Code = defaultContract3?.Contract;
			foreach (MTPurchaseOrder.MTPurchaseOrderSpecificationRow mT in mTPurchaseOrder.MTList)
			{
				MTIncomeSpecificationRow mTIncomeSpecificationRow4 = new MTIncomeSpecificationRow
				{
					RowID = _maxRowId,
					MTCode = mT.MTCode,
					MTName = mT.MTName,
					UnitCode = mT.UnitCode,
					UnitBrief = mT.UnitBrief,
					Quantity = mT.Quantity
				};
				MaterialCacheRow item5 = TradeSettings.TradeCacheService.Material.GetItem(mT.MTCode);
				if (item5 != null)
				{
					mTIncomeSpecificationRow4.MTID = item5.Id;
					mTIncomeSpecificationRow4.VAT = item5.VAT;
					mTIncomeSpecificationRow4.Country = item5.Country;
					mTIncomeSpecificationRow4.Producer = item5.Producer;
				}
				_mtIncomeList.Add(mTIncomeSpecificationRow4);
				_maxRowId++;
			}
			if (Settings.ParametersService.ShowGoodsCostInReportsAndDocs() && ShowLastPurchasePrices)
			{
				SetLastPurchaseInfoToColumn();
			}
			break;
		}
		case 14:
		{
			MTReturnToSupplier mTReturnToSupplier = (MTReturnToSupplier)BaseDocument;
			PartnerCacheRow itemById = TradeSettings.TradeCacheService.Partner.GetItemById(mTReturnToSupplier.PartnerID);
			_supplierContractContainer.PartnerCode.Code = itemById?.PartCode;
			PartnerCacheRow.ContractRow defaultContract = TradeSettings.TradeCacheService.Partner.GetDefaultContract(itemById);
			_supplierContractContainer.Contract.Code = defaultContract?.Contract;
			_comment.EditValue = BaseDocument.Comment;
			_priceListType.Code = mTReturnToSupplier.PriceListType;
			_vatType.Code = mTReturnToSupplier.VATType;
			_currCode.Code = mTReturnToSupplier.CurrCode;
			_discountPercent.Value = mTReturnToSupplier.Discount;
			string storage = mTReturnToSupplier.MTList.First().Storage;
			if (!string.IsNullOrWhiteSpace(storage))
			{
				if (_isMTIncomeDoc)
				{
					StoragesCacheRow item2 = TradeSettings.TradeCacheService.Storage.GetItem(storage);
					if ((item2 == null || !item2.Retail) && TradeSettings.CreateTradeClient().CommonServices.IsStorageAvailable(storage))
					{
						_storage.Code = storage;
					}
				}
				else if (TradeSettings.CreateTradeClient().CommonServices.IsStorageAvailable(storage))
				{
					_storage.Code = storage;
				}
			}
			foreach (MTReturnToSupplier.MTRetToSupSpecificationRow mT2 in mTReturnToSupplier.MTList)
			{
				MTIncomeSpecificationRow mTIncomeSpecificationRow2 = new MTIncomeSpecificationRow
				{
					RowID = _maxRowId,
					MTID = mT2.MTID,
					MTCode = mT2.MTCode,
					MTName = mT2.MTName,
					FullName = mT2.FullName,
					UnitCode = mT2.UnitCode,
					UnitBrief = mT2.UnitBrief,
					Quantity = mT2.Quantity,
					Price = mT2.Price,
					Discount = mT2.Discount,
					DiscountPrice = mT2.DiscountPrice,
					Summa = mT2.Summa,
					SalePrice = mT2.SalePrice,
					SaleSumma = mT2.SaleSumma,
					VAT = mT2.VAT
				};
				MaterialCacheRow itemById2 = TradeSettings.TradeCacheService.Material.GetItemById(mT2.MTID);
				if (itemById2 != null)
				{
					mTIncomeSpecificationRow2.VAT = itemById2.VAT;
					mTIncomeSpecificationRow2.Country = itemById2.Country;
					mTIncomeSpecificationRow2.Producer = itemById2.Producer;
				}
				_mtIncomeList.Add(mTIncomeSpecificationRow2);
				_maxRowId++;
			}
			break;
		}
		case 18:
		case 21:
		{
			MTInvoiceBase mTInvoiceBase = (MTInvoiceBase)BaseDocument;
			_comment.EditValue = BaseDocument.Comment;
			_vatType.Code = mTInvoiceBase.VATType;
			_currCode.Code = mTInvoiceBase.CurrCode;
			_storage.Code = mTInvoiceBase.Storage;
			foreach (MTInvoiceBase.MTInvoiceSpecificationRow mT3 in mTInvoiceBase.MTList)
			{
				if (mT3.ItemType == InvoiceRowTypeSE.Goods && !mT3.IsDeleted)
				{
					MTIncomeSpecificationRow mTIncomeSpecificationRow5 = new MTIncomeSpecificationRow
					{
						RowID = _maxRowId,
						MTCode = mT3.ItemCode,
						MTName = mT3.ItemName,
						FullName = mT3.FullName,
						UnitCode = mT3.UnitCode,
						UnitBrief = mT3.UnitBrief,
						Quantity = mT3.Quantity,
						Summa = mT3.Summa,
						SalePrice = mT3.DiscountPrice,
						SaleSumma = mT3.Summa
					};
					if (Settings.ParametersService.AllowDiscountInPurchase())
					{
						mTIncomeSpecificationRow5.Price = mT3.Price;
						mTIncomeSpecificationRow5.Discount = mT3.Discount;
						mTIncomeSpecificationRow5.DiscountPrice = mT3.DiscountPrice;
					}
					else
					{
						mTIncomeSpecificationRow5.Price = mT3.DiscountPrice;
						mTIncomeSpecificationRow5.DiscountPrice = mTIncomeSpecificationRow5.Price;
					}
					MaterialCacheRow item6 = TradeSettings.TradeCacheService.Material.GetItem(mT3.ItemCode);
					if (item6 != null)
					{
						mTIncomeSpecificationRow5.MTID = item6.Id;
						mTIncomeSpecificationRow5.VAT = item6.VAT;
						mTIncomeSpecificationRow5.Country = item6.Country;
						mTIncomeSpecificationRow5.Producer = item6.Producer;
					}
					_mtIncomeList.Add(mTIncomeSpecificationRow5);
					_maxRowId++;
				}
			}
			break;
		}
		case 16:
		case 22:
		{
			TradeDocument tradeDocument = (TradeDocument)BaseDocument;
			_isAskQuestionConfirmSummChange = false;
			_currCode.Code = tradeDocument.CurrCode;
			if (tradeDocument.POType == (short)DocumentTypeSE.MTStockTakingRetail)
			{
				_currExChngRate.Rate = 1m;
				_currExChngRate.Base = 1m;
			}
			else if (tradeDocument.POType == (short)DocumentTypeSE.MTStockTaking)
			{
				_currExChngRate.Rate = tradeDocument.ExchangeRate.Rate;
				_currExChngRate.Base = tradeDocument.ExchangeRate.Base;
			}
			_isAskQuestionConfirmSummChange = true;
			_vatType.Code = "3";
			_comment.EditValue = tradeDocument.Comment;
			MTStockTaking mTStockTaking = (MTStockTaking)tradeDocument;
			_storage.Code = mTStockTaking.Storage;
			foreach (MTStockTakingBase.MTStockTakingSpecificationRow mT4 in mTStockTaking.MTList)
			{
				if (!(mT4.VarianceQuantity > 0m))
				{
					continue;
				}
				MTIncomeSpecificationRow mTIncomeSpecificationRow = new MTIncomeSpecificationRow
				{
					RowID = _maxRowId,
					MTID = mT4.MTID,
					MTCode = mT4.MTCode,
					MTName = mT4.MTName,
					UnitCode = mT4.UnitCode,
					UnitBrief = mT4.UnitBrief,
					Quantity = mT4.VarianceQuantity
				};
				MaterialCacheRow item = TradeSettings.TradeCacheService.Material.GetItem(mT4.MTCode);
				mTIncomeSpecificationRow.VAT = item?.VAT ?? false;
				if (_isMTIncomeDoc | (!_isMTIncomeDoc && BaseDocument.POType == (short)DocumentTypeSE.MTStockTaking))
				{
					if (mT4.ActualSum > mT4.AccSum && mTIncomeSpecificationRow.Quantity != 0m)
					{
						mTIncomeSpecificationRow.Summa = (mT4.ActualSum - mT4.AccSum).RoundAs(2);
						mTIncomeSpecificationRow.Price = (mTIncomeSpecificationRow.Summa / mTIncomeSpecificationRow.Quantity).RoundAs(2);
						mTIncomeSpecificationRow.DiscountPrice = mTIncomeSpecificationRow.Price;
					}
				}
				else
				{
					if (item != null)
					{
						mTIncomeSpecificationRow.ExtraPercent = ((item.Extra == 0m) ? Settings.ParametersService.Extra() : item.Extra);
					}
					else
					{
						mTIncomeSpecificationRow.ExtraPercent = Settings.ParametersService.Extra();
					}
					if (mTIncomeSpecificationRow.Quantity != 0m)
					{
						mTIncomeSpecificationRow.SalePrice = ((mT4.ActualSum - mT4.AccSum) / mTIncomeSpecificationRow.Quantity).RoundAs(2);
					}
					mTIncomeSpecificationRow.SaleSumma = (mTIncomeSpecificationRow.Quantity * mTIncomeSpecificationRow.SalePrice).RoundAs(2);
				}
				_mtIncomeList.Add(mTIncomeSpecificationRow);
				_maxRowId++;
			}
			break;
		}
		}
		_includeVATInCost.IsChecked = _vatType.Code.InList(VatTypesSE.CostWithVAT.Value, VatTypesSE.CostWithInVAT.Value) && Settings.ParametersService.IncludeVATInCost();
		_mtIncomeGrid.Grid.RefreshData();
		CalcSummAndSummVAT();
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
		PO.MTList.Clear();
		PO.BaseISN = _baseISN;
		foreach (MTIncomeSpecificationRow mtIncome in _mtIncomeList)
		{
			MTIncomeBase.MTIncomeSpecificationRow item = new MTIncomeBase.MTIncomeSpecificationRow
			{
				MTID = mtIncome.MTID,
				MTCode = mtIncome.MTCode,
				MTName = mtIncome.MTName,
				UnitCode = mtIncome.UnitCode,
				UnitBrief = mtIncome.UnitBrief,
				Quantity = mtIncome.Quantity,
				RowNum = mtIncome.RowNum,
				Price = mtIncome.Price,
				VariancePercent = mtIncome.VariancePercent,
				LastPurchasePrice = mtIncome.LastPurchasePrice,
				Discount = mtIncome.Discount,
				DiscountPrice = mtIncome.DiscountPrice,
				Summa = mtIncome.Summa,
				SalePrice = mtIncome.SalePrice,
				SaleSumma = mtIncome.SaleSumma,
				ExtraPercent = mtIncome.ExtraPercent,
				VAT = mtIncome.VAT,
				UseLife = mtIncome.UseLife,
				Producer = mtIncome.Producer,
				Country = mtIncome.Country,
				Property1 = mtIncome.Property1,
				Property2 = mtIncome.Property2,
				Property1Caption = mtIncome.Property1Caption,
				Property2Caption = mtIncome.Property2Caption,
				RowID = mtIncome.RowID,
				RowDescription = mtIncome.RowDescription
			};
			PO.MTList.Add(item);
		}
		PO.MaxRowID = _maxRowId;
	}

	protected override void AfterStore(StoreRequest sr, bool isNewDocument)
	{
		base.AfterStore(sr, isNewDocument);
		SetReportBarButtonsEnable(isEnabled: true);
	}

	protected override void StoreSettings(RegistryKey rk)
	{
		base.StoreSettings(rk);
		rk.SetValue("InfoPanelCollapsed", _infoDisplayControls.InfoPanelCollapsed ? "1" : "0");
		rk.SetValue("ShowGoodsListWIthPrices", ShowGoodsListWithPrices ? "1" : "0");
		_mtIncomeGrid.StoreSettings(rk);
	}

	protected override void RestoreSettings(double DefaultWidth, double DefaultHeight, RegistryKey rk)
	{
		base.RestoreSettings(DefaultWidth, DefaultHeight, rk);
		if (rk != null)
		{
			_infoDisplayControls.InfoPanelCollapsed = rk.GetValue("InfoPanelCollapsed", false).ToString() == "1";
			ShowGoodsListWithPrices = rk.GetValue("ShowGoodsListWIthPrices", true).ToString() == "1";
			_mtIncomeGrid.RestoreSettings(rk);
		}
	}

	private void InfoDisplay_Collapsed(object sender, EventArgs e)
	{
		_infoDisplayControls.InfoPanelCollapsed = true;
	}

	private void ChangeRowInfo(MTIncomeSpecificationRow Row = null)
	{
		if (Row == null)
		{
			((DispatcherObject)this).Dispatcher.BeginInvoke((Delegate)new DelegateClearInfoPanel(CleanInfoPanel), Array.Empty<object>());
			return;
		}
		((DispatcherObject)this).Dispatcher.BeginInvoke((Delegate)new DelegateSetRowInfoToInfoPanel(SetRowInfoToPanel), new object[1] { Row });
	}

	private void SetRowInfoToPanel(MTIncomeSpecificationRow row)
	{
		try
		{
			if (string.IsNullOrWhiteSpace(row.MTName))
			{
				CleanInfoPanel();
				return;
			}
			_isChanged = IsChanged;
			_infoItemName.Text = $"{row.MTCode} - {row.MTName}";
			SetAvailabilityInfoToPanel(row);
			if (Settings.ParametersService.ShowGoodsCostInReportsAndDocs())
			{
				if (!ShowLastPurchasePrices)
				{
					CalcLastPurchasePricesAndVariancePercentAndSetToPanel(row);
				}
				else
				{
					CalcLastPurchasePricesAdnVariancePercent(row);
				}
			}
			SetDescriptionInfoPanel(row);
			IsChanged = _isChanged;
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	private void SetDescriptionInfoPanel(MTIncomeSpecificationRow row)
	{
		MaterialCacheRow item = TradeSettings.TradeCacheService.Material.GetItem(row.MTCode);
		_infoDescription.Text = ((item == null) ? "" : item.Description);
	}

	private void CleanInfoPanel()
	{
		_isChanged = IsChanged;
		_infoItemName.Text = "";
		if (_infoDescription != null)
		{
			_infoDescription.Text = "";
		}
		_infoDisplayControls.ClearInfoAvailability();
		IsChanged = _isChanged;
	}

	private void CurrCode_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		CurrCode_EditValueChanged();
	}

	private void CurrCode_EditValueChanged()
	{
		bool flag = false;
		if (_isAskQuestionConfirmSummChange && _totalSumma.Value != 0m && !string.IsNullOrWhiteSpace(_currCode.Code) && _currCode.DoValidate())
		{
			flag = true;
		}
		ChangedCurrCodeOrDate();
		if (flag)
		{
			UpdatePricesForCurr();
			UpdatePriceCol();
		}
		RowInfoChangeHandler?.Invoke((MTIncomeSpecificationRow)_mtIncomeTableView.Grid.CurrentItem);
	}

	private void Date_EditValueChanged(object sender, EditValueChangedEventArgs e)
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
			_currExChngRate.IsReadOnly = StoredInDb && State != DocumentStateSE.Draft.Value && State != DocumentStateSE.Deleted.Value;
			_priceCol.Header = ArmSoft.SME.Common.Resources.FCPrice;
			_summaCol.Header = ArmSoft.SME.Common.Resources.FCSumma;
			if (!_date.DoValidate())
			{
				throw new ASException(ArmSoft.SME.Common.Resources.ValidateEmptyDate);
			}
			ExchangeRate exchangeRates = TradeSettings.CreateTradeClient().CommonServices.GetExchangeRates(_currCode.Code, _date.EditValue.Value);
			_currExChngRate.Rate = exchangeRates.Rate;
			_currExChngRate.Base = exchangeRates.Base;
		}
		_currExChngRate.DoValidate();
	}

	private void CurrExChngRate_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		if (_currExChngRate.DoValidate())
		{
			CalculatePartSumma();
		}
	}

	private void PartnerCode_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		if (Settings.ParametersService.ShowGoodsCostInReportsAndDocs() && _showLastPurchasePrices)
		{
			SetLastPurchaseInfoToColumn();
			_mtIncomeGrid.Grid.RefreshData();
		}
		CalculatePartSumma();
		RowInfoChangeHandler?.Invoke((MTIncomeSpecificationRow)_mtIncomeTableView.Grid.CurrentItem);
	}

	private void PartnerCode_EditAdditionalValueChanged(object sender, EventArgs e)
	{
		CalculatePartSumma();
	}

	private void PartnerContractCode_EditValueChanged(object sender, EventArgs e)
	{
		if (_isAskQuestionConfirmSummChange && (_mtIncomeList.Count == 0 || string.IsNullOrWhiteSpace(_priceListType.Code)) && !string.IsNullOrWhiteSpace(_supplierContractContainer.Contract.Code) && _supplierContractContainer.Contract.DoValidate())
		{
			PartnerCacheRow item = TradeSettings.TradeCacheService.Partner.GetItem(_supplierContractContainer.PartnerCode.Code);
			if (item != null)
			{
				PartnerCacheRow.ContractRow contract = TradeSettings.TradeCacheService.Partner.GetContract(item.Id, _supplierContractContainer.Contract.Code);
				if (contract != null && !string.IsNullOrWhiteSpace(contract.PriceType))
				{
					_isAskQuestionConfirmSummChange = false;
					_priceListType.Code = contract.PriceType;
					if (_allowDiscountInPurchase)
					{
						_discountPercent.EditValue = contract.Percent;
					}
					_isAskQuestionConfirmSummChange = true;
					if (_mtIncomeList.Count == 0)
					{
						ArmSoft.SME.Trade.UI.Helpers.PriceHelper.GetPriceListRowVATType(contract.PriceType, out var priceListRowVATType, out var _);
						_vatType.Code = priceListRowVATType;
						_currCode.Code = contract.CurrCode;
					}
				}
			}
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

	private void PriceListType_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		PriceListType_EditValueChangedForLoad();
	}

	private void PriceListType_EditValueChangedForLoad()
	{
		bool flag = false;
		string priceListRowVATType = "";
		string priceListRowCurr;
		if ((_isAskQuestionConfirmSummChange || RaisePriceTypeEditValueChangeOnce) && !string.IsNullOrWhiteSpace(_priceListType.Code) && _priceListType.DoValidate())
		{
			if ((_totalSumma.Value == 0m || RaisePriceTypeEditValueChangeOnce) && ArmSoft.SME.Trade.UI.Helpers.PriceHelper.GetPriceListRowVATType(_priceListType.Code, out priceListRowVATType, out priceListRowCurr))
			{
				_vatType.Code = priceListRowVATType;
			}
			flag = true;
		}
		if (flag)
		{
			UpdateMTIncomePrices(_date.EditValue.Value, _priceListType.Code, _currExChngRate.Rate, _currExChngRate.Base);
			ArmSoft.SME.Trade.UI.Helpers.PriceHelper.GetPriceListRowVATType(_priceListType.Code, out priceListRowVATType, out priceListRowCurr);
			if (priceListRowVATType != _vatType.Code)
			{
				VATTypeValidate(priceListRowVATType, _vatType.Code);
			}
			UpdatePriceCol();
			CalculateDiscount(ref _mtIncomeList, _discountPercent.Value, gDiscount: false);
			_mtIncomeGrid.Grid.RefreshData();
			CalcSummAndSummVAT();
			if (Settings.ParametersService.ShowGoodsCostInReportsAndDocs() && ShowLastPurchasePrices)
			{
				SetVariancePercent();
			}
			_mtIncomeGrid.Grid.RefreshData();
		}
		RaisePriceTypeEditValueChangeOnce = false;
	}

	private void UpdateMTIncomePrices(DateOnly DocDate, string PriceListType, decimal CurrRate, decimal CurrBase)
	{
		string priceListRowVATType = "";
		string priceListRowCurr = "";
		ExchangeRate exchangeRate = new ExchangeRate();
		if (_mtIncomeList.Count == 0)
		{
			return;
		}
		ArmSoft.SME.Trade.UI.Helpers.PriceHelper.GetPriceListRowVATType(PriceListType, out priceListRowVATType, out priceListRowCurr);
		if (priceListRowCurr != "AMD")
		{
			exchangeRate = TradeSettings.CreateTradeClient().CommonServices.GetExchangeRates(priceListRowCurr, DocDate);
		}
		foreach (MTIncomeSpecificationRow mtIncome in _mtIncomeList)
		{
			PricesDataProvider.DataRow price = TradeSettings.CreateTradeClient().CommonServices.GetPrice(PriceListType, mtIncome.MTID, mtIncome.UnitCode, DocDate);
			if (priceListRowCurr == _currCode.Code)
			{
				mtIncome.Price = price.Price;
			}
			else if (priceListRowCurr == "AMD")
			{
				mtIncome.Price = (price.Price / CurrRate * CurrBase).RoundAs(2);
			}
			else
			{
				mtIncome.Price = (price.Price * exchangeRate.Rate / exchangeRate.Base / CurrRate * CurrBase).RoundAs(2);
			}
			mtIncome.DiscountPrice = (mtIncome.Price - mtIncome.Price * mtIncome.Discount / 100m).RoundAs(2);
			mtIncome.Summa = (mtIncome.DiscountPrice * mtIncome.Quantity).RoundAs(2);
		}
	}

	private void UpdatePriceCol()
	{
		foreach (MTIncomeSpecificationRow mtIncome in _mtIncomeList)
		{
			if (_isMTIncomeDoc)
			{
				continue;
			}
			if (!string.IsNullOrWhiteSpace(_storage.Code))
			{
				StoragesCacheRow item = TradeSettings.TradeCacheService.Storage.GetItem(_storage.Code);
				if (item != null && item.Retail)
				{
					UpdateSalePriceAndSaleSumma(mtIncome);
					continue;
				}
			}
			FillSalePriceAndSaleSumma(mtIncome);
		}
		_mtIncomeGrid.Grid.RefreshData();
	}

	private void UpdatePricesForCurr()
	{
		foreach (MTIncomeSpecificationRow mtIncome in _mtIncomeList)
		{
			if (_oldDocCurrCode == "AMD")
			{
				if (_currExChngRate.Rate != 0m)
				{
					mtIncome.Price = (mtIncome.Price / _currExChngRate.Rate * _currExChngRate.Base).RoundAs(2);
				}
			}
			else if (!(_currExChngRate.Rate == 0m) && !(_oldDocCurrExChngBase == 0m))
			{
				mtIncome.Price = (mtIncome.Price * _oldDocCurrExChngRate / _oldDocCurrExChngBase / _currExChngRate.Rate * _currExChngRate.Base).RoundAs(2);
			}
			mtIncome.DiscountPrice = (mtIncome.Price - mtIncome.Price * mtIncome.Discount / 100m).RoundAs(2);
			mtIncome.Summa = (mtIncome.DiscountPrice * mtIncome.Quantity).RoundAs(2);
			if (_isMTIncomeDoc)
			{
				continue;
			}
			if (!string.IsNullOrWhiteSpace(_storage.Code))
			{
				StoragesCacheRow item = TradeSettings.TradeCacheService.Storage.GetItem(_storage.Code);
				if (item != null && item.Retail)
				{
					UpdateSalePriceAndSaleSumma(mtIncome);
					continue;
				}
			}
			FillSalePriceAndSaleSumma(mtIncome);
		}
		CalculateDiscount(ref _mtIncomeList, _discountPercent.Value, gDiscount: false);
		_mtIncomeGrid.Grid.RefreshData();
		CalcSummAndSummVAT();
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
			_currExChngRate.IsReadOnly = StoredInDb && State != DocumentStateSE.Draft.Value && State != DocumentStateSE.Deleted.Value;
			_priceCol.Header = ArmSoft.SME.Common.Resources.FCPrice;
			_summaCol.Header = ArmSoft.SME.Common.Resources.FCSumma;
			if (!_date.DoValidate())
			{
				throw new ASException(ArmSoft.SME.Common.Resources.ValidateEmptyDate);
			}
			ExchangeRate exchangeRates = TradeSettings.CreateTradeClient().CommonServices.GetExchangeRates(_currCode.Code, _date.EditValue.Value);
			_currExChngRate.Rate = exchangeRates.Rate;
			_currExChngRate.Base = exchangeRates.Base;
		}
		_currExChngRate.DoValidate();
	}

	protected void VATType_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		VATType_EditValueChangedForLoad();
		VATTypeValidate(Convert.ToString(e.OldValue), _vatType.Code);
		if (Settings.ParametersService.ShowGoodsCostInReportsAndDocs())
		{
			if (ShowLastPurchasePrices)
			{
				CalcVariancePercent();
			}
			else if (_mtIncomeGrid.Grid.CurrentItem != null)
			{
				CalcLastPurchasePricesAndVariancePercentAndSetToPanel((MTIncomeSpecificationRow)_mtIncomeGrid.Grid.CurrentItem);
			}
		}
		_includeVATInCost.IsChecked = _vatType.Code.InList(VatTypesSE.CostWithVAT.Value, VatTypesSE.CostWithInVAT.Value) && Settings.ParametersService.IncludeVATInCost();
		_mtIncomeGrid.Grid.RefreshData();
	}

	protected void VATType_EditValueChangedForLoad()
	{
		bool flag = _vatType.Code.InList("1", "2");
		_mtIncomeGrid.Grid.Columns["VAT"].Visible = flag;
		_mtIncomeGrid.Grid.Columns["VAT"].ShowInColumnChooser = flag;
		if (!flag)
		{
			_vatSumma.EditValue = 0m;
		}
		_includeVATInCost.IsReadOnly = !flag;
	}

	protected void VATTypeValidate(string VATTypeOldValue, string VATTypeNewValue)
	{
		decimal num = 0m;
		decimal num2 = 0m;
		if (_mtIncomeList.Count > 0)
		{
			switch (VATTypeNewValue)
			{
			case "1":
				if (VATTypeOldValue == "3" || VATTypeOldValue == "4")
				{
					foreach (MTIncomeSpecificationRow mtIncome in _mtIncomeList)
					{
						num += ArmSoft.SME.Trade.UI.Helpers.PriceHelper.CalcVATAmountByType(mtIncome.Summa, "1", mtIncome.VAT);
						num2 += mtIncome.Summa;
					}
				}
				else
				{
					foreach (MTIncomeSpecificationRow mtIncome2 in _mtIncomeList)
					{
						mtIncome2.Price -= ArmSoft.SME.Trade.UI.Helpers.PriceHelper.CalcVATAmountByType(mtIncome2.Price, VATTypeOldValue, mtIncome2.VAT);
						mtIncome2.DiscountPrice = (mtIncome2.Price * (1m - mtIncome2.Discount / 100m)).RoundAs(2);
						mtIncome2.Summa = (mtIncome2.DiscountPrice * mtIncome2.Quantity).RoundAs(2);
						num += ArmSoft.SME.Trade.UI.Helpers.PriceHelper.CalcVATAmountByType(mtIncome2.Summa, "1", mtIncome2.VAT);
						num2 += mtIncome2.Summa;
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
					foreach (MTIncomeSpecificationRow mtIncome3 in _mtIncomeList)
					{
						num += ArmSoft.SME.Trade.UI.Helpers.PriceHelper.CalcVATAmountByType(mtIncome3.Summa, "1", mtIncome3.VAT);
						mtIncome3.Price += ArmSoft.SME.Trade.UI.Helpers.PriceHelper.CalcVATAmountByType(mtIncome3.Price, "1", mtIncome3.VAT);
						mtIncome3.DiscountPrice = (mtIncome3.Price * (1m - mtIncome3.Discount / 100m)).RoundAs(2);
						mtIncome3.Summa = (mtIncome3.DiscountPrice * mtIncome3.Quantity).RoundAs(2);
						num2 += mtIncome3.Summa;
					}
					_vatSumma.EditValue = num;
					_totalSumma.EditValue = num2;
					break;
				case "1":
					foreach (MTIncomeSpecificationRow mtIncome4 in _mtIncomeList)
					{
						num += ArmSoft.SME.Trade.UI.Helpers.PriceHelper.CalcVATAmountByType(mtIncome4.Summa, "1", mtIncome4.VAT);
						mtIncome4.Price += ArmSoft.SME.Trade.UI.Helpers.PriceHelper.CalcVATAmountByType(mtIncome4.Price, "1", mtIncome4.VAT);
						mtIncome4.DiscountPrice = (mtIncome4.Price * (1m - mtIncome4.Discount / 100m)).RoundAs(2);
						mtIncome4.Summa = (mtIncome4.DiscountPrice * mtIncome4.Quantity).RoundAs(2);
						num2 += mtIncome4.Summa;
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
					foreach (MTIncomeSpecificationRow mtIncome5 in _mtIncomeList)
					{
						mtIncome5.Price -= ArmSoft.SME.Trade.UI.Helpers.PriceHelper.CalcVATAmountByType(mtIncome5.Price, VATTypeOldValue, mtIncome5.VAT);
						mtIncome5.DiscountPrice = (mtIncome5.Price * (1m - mtIncome5.Discount / 100m)).RoundAs(2);
						mtIncome5.Summa = (mtIncome5.DiscountPrice * mtIncome5.Quantity).RoundAs(2);
						num2 += mtIncome5.Summa;
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
					foreach (MTIncomeSpecificationRow mtIncome6 in _mtIncomeList)
					{
						num2 += mtIncome6.Summa;
					}
					_vatSumma.EditValue = 0m;
					_totalSumma.EditValue = num2;
				}
				break;
			}
		}
		CalculatePartSumma();
	}

	protected void MTIncomeGrid_AddingRow(object sender, DocumentGridAddingRowEventArgs e)
	{
		MTIncomeSpecificationRow mTIncomeSpecificationRow = new MTIncomeSpecificationRow();
		if (Settings.ParametersService.AllowDiscountInPurchase())
		{
			mTIncomeSpecificationRow.Discount = _discountPercent.Value;
		}
		mTIncomeSpecificationRow.RowID = _maxRowId;
		_mtIncomeList.Add(mTIncomeSpecificationRow);
		_maxRowId++;
		e.RowNumber = _mtIncomeList.Count - 1;
		RowInfoChangeHandler?.Invoke(_mtIncomeList[_mtIncomeList.Count - 1]);
	}

	protected void MTIncomeGrid_DuplicatingRow(object sender, DocumentGridDuplicatingRowEventArgs e)
	{
		MTIncomeSpecificationRow mTIncomeSpecificationRow = (MTIncomeSpecificationRow)_mtIncomeGrid.Grid.GetRow(e.RowHandle);
		if (mTIncomeSpecificationRow != null)
		{
			MTIncomeSpecificationRow mTIncomeSpecificationRow2 = new MTIncomeSpecificationRow();
			mTIncomeSpecificationRow.Clone(mTIncomeSpecificationRow2);
			mTIncomeSpecificationRow2.RowID = _maxRowId;
			_mtIncomeList.Add(mTIncomeSpecificationRow2);
			_maxRowId++;
			e.NewRowNumber = _mtIncomeList.Count - 1;
			_mtIncomeGrid.Grid.RefreshData();
			CalcSummAndSummVAT();
			ChangeRowInfo(mTIncomeSpecificationRow2);
			if (Settings.ParametersService.ShowGoodsCostInReportsAndDocs() && ShowLastPurchasePrices)
			{
				CalcLastPurchasePricesAdnVariancePercent();
			}
		}
	}

	protected void MTIncomeGrid_DeletedRow(object sender, EventArgs e)
	{
		CalcSummAndSummVAT();
		_mtIncomeGrid.Grid.RefreshData();
		RowInfoChangeHandler?.Invoke((MTIncomeSpecificationRow)_mtIncomeTableView.Grid.CurrentItem);
	}

	protected void MTIncomeGrid_MovingRowUp(object sender, DocumentGridMovingRowUpOrDownEventArgs e)
	{
		if (_mtIncomeGrid.Grid.SortInfo.Count > 0 || _mtIncomeTableView.Grid.VisibleRowCount != _mtIncomeList.Count)
		{
			throw new ASException(ArmSoft.SME.Common.Resources.NotAllowRowMoveInSortedGrid);
		}
		MTIncomeSpecificationRow mTIncomeSpecificationRow = (MTIncomeSpecificationRow)_mtIncomeGrid.Grid.GetRow(e.RowHandle);
		if (mTIncomeSpecificationRow != null)
		{
			_mtIncomeList.RemoveAt(e.RowHandle);
			_mtIncomeList.Insert(e.RowHandle - 1, mTIncomeSpecificationRow);
		}
		_mtIncomeGrid.Grid.RefreshData();
	}

	protected void MTIncomeGrid_MovingRowDown(object sender, DocumentGridMovingRowUpOrDownEventArgs e)
	{
		if (_mtIncomeGrid.Grid.SortInfo.Count > 0 || _mtIncomeTableView.Grid.VisibleRowCount != _mtIncomeList.Count)
		{
			throw new ASException(ArmSoft.SME.Common.Resources.NotAllowRowMoveInSortedGrid);
		}
		MTIncomeSpecificationRow mTIncomeSpecificationRow = (MTIncomeSpecificationRow)_mtIncomeGrid.Grid.GetRow(e.RowHandle);
		if (mTIncomeSpecificationRow != null)
		{
			_mtIncomeList.RemoveAt(e.RowHandle);
			_mtIncomeList.Insert(e.RowHandle + 1, mTIncomeSpecificationRow);
		}
		_mtIncomeGrid.Grid.RefreshData();
	}

	protected override bool AllowChangeGridMode()
	{
		return true;
	}

	protected void MTIncomeGrid_AddingRowWithDialog(object sender, DocumentGridAddingRowEventArgs e)
	{
		if (_date.DoValidate() && _date.EditValue.HasValue)
		{
			MTIncomeBaseRowAddEditDialog<T> mTIncomeBaseRowAddEditDialog = new MTIncomeBaseRowAddEditDialog<T>(DialogWindowMode.NewModeWithNext, this);
			mTIncomeBaseRowAddEditDialog.Invoke();
			int addedRows = mTIncomeBaseRowAddEditDialog.AddedRows;
			if (addedRows != 0)
			{
				_maxRowId += addedRows;
				_mtIncomeGrid.Grid.RefreshData();
				CalcSummAndSummVAT();
				RowInfoChangeEventHandler rowInfoChangeEventHandler = RowInfoChangeHandler;
				if (rowInfoChangeEventHandler != null)
				{
					ObservableCollection<MTIncomeSpecificationRow> mtIncomeList = _mtIncomeList;
					rowInfoChangeEventHandler(mtIncomeList[mtIncomeList.Count - 1]);
				}
			}
		}
		else
		{
			ControlsHelper.SetFocus(_date);
			ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Common.Resources.ValidateEmptyDate, ArmSoft.SME.Trade.Common.Resources.ProgramName, MessageBoxButton.OK, MessageBoxImage.Exclamation);
		}
	}

	protected void MTIncomeGrid_DuplicatingRowWithDialog(object sender, DocumentGridDuplicatingRowEventArgs e)
	{
		if (_date.DoValidate() && _date.EditValue.HasValue)
		{
			MTIncomeSpecificationRow mTIncomeSpecificationRow = (MTIncomeSpecificationRow)_mtIncomeGrid.Grid.GetRow(e.RowHandle);
			if (mTIncomeSpecificationRow == null)
			{
				return;
			}
			MTIncomeBaseRowAddEditDialog<T> mTIncomeBaseRowAddEditDialog = new MTIncomeBaseRowAddEditDialog<T>(DialogWindowMode.NewModeWithNext, this, mTIncomeSpecificationRow);
			mTIncomeBaseRowAddEditDialog.Invoke();
			int addedRows = mTIncomeBaseRowAddEditDialog.AddedRows;
			if (addedRows != 0)
			{
				_maxRowId += addedRows;
				_mtIncomeGrid.Grid.RefreshData();
				CalcSummAndSummVAT();
				RowInfoChangeEventHandler rowInfoChangeEventHandler = RowInfoChangeHandler;
				if (rowInfoChangeEventHandler != null)
				{
					ObservableCollection<MTIncomeSpecificationRow> mtIncomeList = _mtIncomeList;
					rowInfoChangeEventHandler(mtIncomeList[mtIncomeList.Count - 1]);
				}
			}
		}
		else
		{
			ControlsHelper.SetFocus(_date);
			ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Common.Resources.ValidateEmptyDate, ArmSoft.SME.Trade.Common.Resources.ProgramName, MessageBoxButton.OK, MessageBoxImage.Exclamation);
		}
	}

	protected void MTIncomeGrid_EditingRowWithDialog(object sender, DocumentGridEditingRowEventArgs e)
	{
		try
		{
			if (_date.DoValidate() && _date.EditValue.HasValue)
			{
				MTIncomeSpecificationRow mTIncomeSpecificationRow = (MTIncomeSpecificationRow)_mtIncomeGrid.Grid.GetRow(e.RowHandle);
				if (mTIncomeSpecificationRow == null)
				{
					return;
				}
				MTIncomeBaseRowAddEditDialog<T> mTIncomeBaseRowAddEditDialog = new MTIncomeBaseRowAddEditDialog<T>(e.AllowEdit ? DialogWindowMode.EditMode : DialogWindowMode.ViewMode, this, mTIncomeSpecificationRow);
				e.RowEdited = Convert.ToBoolean(mTIncomeBaseRowAddEditDialog.Invoke());
				if (e.AllowEdit && e.RowEdited)
				{
					_mtIncomeGrid.Grid.RefreshData();
					CalcSummAndSummVAT();
					RowInfoChangeEventHandler rowInfoChangeEventHandler = RowInfoChangeHandler;
					if (rowInfoChangeEventHandler != null)
					{
						ObservableCollection<MTIncomeSpecificationRow> mtIncomeList = _mtIncomeList;
						rowInfoChangeEventHandler(mtIncomeList[mtIncomeList.Count - 1]);
					}
				}
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

	protected void MTIncomeGrid_BeforeAddNewDropDown(object sender, DocumentGridClickDropDownEventArgs e)
	{
		try
		{
			MTIncomeSpecificationRow mTIncomeSpecificationRow = (MTIncomeSpecificationRow)_mtIncomeGrid.Grid.GetRow(e.RowHandle);
			if (string.IsNullOrWhiteSpace(mTIncomeSpecificationRow.MTCode))
			{
				return;
			}
			MaterialCacheRow item = TradeSettings.TradeCacheService.Material.GetItem(mTIncomeSpecificationRow.MTCode);
			if (item != null)
			{
				if (e.Column == _property1Col)
				{
					_property1Col.DataView = new ArmSoft.SME.Trade.UI.DropDown.PropertyList(item.Property1);
				}
				else if (e.Column == _property2Col)
				{
					_property2Col.DataView = new ArmSoft.SME.Trade.UI.DropDown.PropertyList(item.Property2);
				}
			}
		}
		catch
		{
		}
	}

	protected void MTIncomeGrid_ClickDropDown(object sender, DocumentGridClickDropDownEventArgs e)
	{
		try
		{
			MTIncomeSpecificationRow mTIncomeSpecificationRow = (MTIncomeSpecificationRow)_mtIncomeGrid.Grid.GetRow(e.RowHandle);
			if (e.Column == _codeCol)
			{
				IDropDownView dropDownView = null;
				dropDownView = ((string.IsNullOrWhiteSpace(_priceListType.Code) || !ShowGoodsListWithPrices) ? ((IDropDownView)new GoodsFilter(shortMode: true, "1", "", isShowOnlyInPriceList: true)) : ((IDropDownView)new GoodsWithPrices(_date.EditValue.Value, _priceListType.Code, Name, "", ItemTypesSE.Goods.Value)));
				ModalBrowser modalBrowser = new ModalBrowser(dropDownView)
				{
					LookUpValue = e.Value,
					Owner = e.Owner,
					Left = e.Left,
					Top = e.Top
				};
				if (modalBrowser.Show())
				{
					e.NewValue = dropDownView.ValueMember(modalBrowser.SelectedRowIndex);
				}
			}
			else if (e.Column == _unitCodeCol)
			{
				ArmSoft.SME.Trade.UI.DropDown.MTQntUnits mTQntUnits = new ArmSoft.SME.Trade.UI.DropDown.MTQntUnits(string.IsNullOrWhiteSpace(mTIncomeSpecificationRow?.MTCode) ? "" : mTIncomeSpecificationRow?.MTCode);
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
					if (string.IsNullOrWhiteSpace(mTIncomeSpecificationRow.MTCode) && TradeSettings.TradeCacheService.Material.GetQntUnit(mTIncomeSpecificationRow.MTID, mTIncomeSpecificationRow.UnitCode) != null)
					{
						mTIncomeSpecificationRow.MTID = mTIncomeSpecificationRow.MTID;
						MaterialCacheRow itemById = TradeSettings.TradeCacheService.Material.GetItemById(mTIncomeSpecificationRow.MTID);
						if (itemById != null)
						{
							mTIncomeSpecificationRow.MTID = itemById.Id;
							mTIncomeSpecificationRow.MTCode = itemById.MTCode;
							mTIncomeSpecificationRow.MTName = itemById.Caption;
							mTIncomeSpecificationRow.UnitCode = TradeSettings.CreateTradeClient().CommonServices.GetMTQntUnitByMTIDPriceTypeAndDocDate(mTIncomeSpecificationRow.MTID, _priceListType.Code, _date.EditValue.Value);
							if (string.IsNullOrWhiteSpace(mTIncomeSpecificationRow.UnitCode))
							{
								mTIncomeSpecificationRow.UnitCode = itemById.Unit;
							}
							QntUnitCacheRow item = TradeSettings.TradeCacheService.QntUnit.GetItem(mTIncomeSpecificationRow.UnitCode);
							mTIncomeSpecificationRow.UnitBrief = ((item == null) ? "" : item.Brief);
							_unitCodeCol.DataView = new ArmSoft.SME.Trade.UI.DropDown.QntUnits();
							mTIncomeSpecificationRow.VAT = itemById.VAT;
							mTIncomeSpecificationRow.Country = itemById.Country;
							mTIncomeSpecificationRow.Producer = itemById.Producer;
							mTIncomeSpecificationRow.Price = TradeSettings.CreateTradeClient().CommonServices.GetPriceForGivenPriceListAndVATType(_priceListType.Code, _vatType.Code, mTIncomeSpecificationRow.MTID, mTIncomeSpecificationRow.UnitCode, _date.EditValue.Value, _currCode.Code);
							mTIncomeSpecificationRow.DiscountPrice = (mTIncomeSpecificationRow.Price - mTIncomeSpecificationRow.Price * mTIncomeSpecificationRow.Discount / 100m).RoundAs(2);
							mTIncomeSpecificationRow.Summa = (mTIncomeSpecificationRow.Quantity * mTIncomeSpecificationRow.DiscountPrice).RoundAs(2);
							mTIncomeSpecificationRow.Property1 = "";
							mTIncomeSpecificationRow.Property2 = "";
							mTIncomeSpecificationRow.Property1Caption = "";
							mTIncomeSpecificationRow.Property2Caption = "";
							_property1Col.DataView = new ArmSoft.SME.Trade.UI.DropDown.PropertyList(itemById.Property1);
							_property2Col.DataView = new ArmSoft.SME.Trade.UI.DropDown.PropertyList(itemById.Property2);
						}
					}
				}
			}
			else if (e.Column == _property1Col || e.Column == _property2Col)
			{
				if (mTIncomeSpecificationRow == null)
				{
					return;
				}
				MaterialCacheRow itemById2 = TradeSettings.TradeCacheService.Material.GetItemById(mTIncomeSpecificationRow.MTID);
				if (itemById2 == null || (string.IsNullOrWhiteSpace(itemById2.Property1) && e.Column == _property1Col) || (string.IsNullOrWhiteSpace(itemById2.Property2) && e.Column == _property2Col))
				{
					return;
				}
				ArmSoft.SME.Trade.UI.DropDown.PropertyList propertyList = new ArmSoft.SME.Trade.UI.DropDown.PropertyList((e.Column == _property1Col) ? itemById2.Property1 : itemById2.Property2);
				ModalBrowser modalBrowser3 = new ModalBrowser(propertyList)
				{
					LookUpValue = e.Value,
					Owner = e.Owner,
					Left = e.Left,
					Top = e.Top
				};
				if (modalBrowser3.Show())
				{
					e.NewValue = propertyList.CommentMember(modalBrowser3.SelectedRowIndex);
					if (e.Column == _property1Col)
					{
						mTIncomeSpecificationRow.Property1 = propertyList.ValueMember(modalBrowser3.SelectedRowIndex);
					}
					else
					{
						mTIncomeSpecificationRow.Property2 = propertyList.ValueMember(modalBrowser3.SelectedRowIndex);
					}
				}
			}
			_mtIncomeGrid.Grid.RefreshData();
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	protected void MTIncomeTableView_CellValueChanged(object sender, CellValueChangedEventArgs e)
	{
		MTIncomeSpecificationRow mTIncomeSpecificationRow = (MTIncomeSpecificationRow)_mtIncomeGrid.Grid.GetRow(e.RowHandle);
		if (mTIncomeSpecificationRow == null)
		{
			return;
		}
		string oldUnit = string.Empty;
		if (e.OldValue != null)
		{
			oldUnit = e.OldValue.ToString();
		}
		if (e.Column == _codeCol)
		{
			try
			{
				if (e.Value == null)
				{
					mTIncomeSpecificationRow.MTID = -1;
					mTIncomeSpecificationRow.MTName = "";
					mTIncomeSpecificationRow.UnitCode = "";
					mTIncomeSpecificationRow.UnitBrief = "";
				}
				else if (_date.DoValidate() && _date.EditValue.HasValue)
				{
					string text = e.Value.ToString();
					if (string.IsNullOrEmpty(text))
					{
						throw new Exception();
					}
					MaterialCacheRow item = TradeSettings.TradeCacheService.Material.GetItem(text, throwExceptionNotExits: true);
					mTIncomeSpecificationRow.MTID = item.Id;
					mTIncomeSpecificationRow.MTName = item.Caption;
					mTIncomeSpecificationRow.UnitCode = TradeSettings.CreateTradeClient().CommonServices.GetMTQntUnitByMTIDPriceTypeAndDocDate(mTIncomeSpecificationRow.MTID, _priceListType.Code, _date.EditValue.Value);
					if (string.IsNullOrWhiteSpace(mTIncomeSpecificationRow.UnitCode))
					{
						mTIncomeSpecificationRow.UnitCode = item.Unit;
					}
					QntUnitCacheRow item2 = TradeSettings.TradeCacheService.QntUnit.GetItem(mTIncomeSpecificationRow.UnitCode, throwExceptionNotExits: true);
					mTIncomeSpecificationRow.UnitBrief = ((item2 == null) ? "" : item2.Brief);
					_unitCodeCol.DataView = new ArmSoft.SME.Trade.UI.DropDown.MTQntUnits(text);
					mTIncomeSpecificationRow.VAT = item.VAT;
					mTIncomeSpecificationRow.Country = item.Country;
					mTIncomeSpecificationRow.Producer = item.Producer;
					mTIncomeSpecificationRow.Price = TradeSettings.CreateTradeClient().CommonServices.GetPriceForGivenPriceListAndVATType(_priceListType.Code, _vatType.Code, mTIncomeSpecificationRow.MTID, mTIncomeSpecificationRow.UnitCode, _date.EditValue.Value, _currCode.Code);
					if (Settings.ParametersService.AllowDiscountInPurchase())
					{
						mTIncomeSpecificationRow.Discount = _discountPercent.Value;
					}
					mTIncomeSpecificationRow.DiscountPrice = (mTIncomeSpecificationRow.Price - mTIncomeSpecificationRow.Price * mTIncomeSpecificationRow.Discount / 100m).RoundAs(2);
					mTIncomeSpecificationRow.Summa = (mTIncomeSpecificationRow.Quantity * mTIncomeSpecificationRow.DiscountPrice).RoundAs(2);
					mTIncomeSpecificationRow.Property1 = "";
					mTIncomeSpecificationRow.Property2 = "";
					mTIncomeSpecificationRow.Property1Caption = "";
					mTIncomeSpecificationRow.Property2Caption = "";
					_property1Col.DataView = new ArmSoft.SME.Trade.UI.DropDown.PropertyList(item.Property1);
					_property2Col.DataView = new ArmSoft.SME.Trade.UI.DropDown.PropertyList(item.Property2);
					if (Settings.ParametersService.ShowGoodsCostInReportsAndDocs() && ShowLastPurchasePrices)
					{
						SetLastPurchaseInfoToColumn(mTIncomeSpecificationRow);
					}
					if (!_isMTIncomeDoc)
					{
						mTIncomeSpecificationRow.ExtraPercent = ((item.Extra == 0m) ? Settings.ParametersService.Extra() : item.Extra);
						if (string.IsNullOrWhiteSpace(_storage.Code))
						{
							goto IL_0369;
						}
						StoragesCacheRow item3 = TradeSettings.TradeCacheService.Storage.GetItem(_storage.Code);
						if (item3 == null || !item3.Retail)
						{
							goto IL_0369;
						}
						UpdateSalePriceAndSaleSumma(mTIncomeSpecificationRow);
					}
				}
				goto end_IL_004b;
				IL_0369:
				mTIncomeSpecificationRow.SalePrice = 0m;
				FillSalePriceAndSaleSumma(mTIncomeSpecificationRow);
				end_IL_004b:;
			}
			catch (Exception ex)
			{
				mTIncomeSpecificationRow.MTName = "";
				mTIncomeSpecificationRow.MTID = -1;
				mTIncomeSpecificationRow.UnitCode = "";
				mTIncomeSpecificationRow.UnitBrief = "";
				mTIncomeSpecificationRow.Quantity = 0m;
				mTIncomeSpecificationRow.Summa = 0m;
				mTIncomeSpecificationRow.Price = 0m;
				mTIncomeSpecificationRow.Discount = 0m;
				mTIncomeSpecificationRow.DiscountPrice = 0m;
				mTIncomeSpecificationRow.VAT = false;
				mTIncomeSpecificationRow.Country = "";
				mTIncomeSpecificationRow.Producer = "";
				mTIncomeSpecificationRow.Property1 = "";
				mTIncomeSpecificationRow.Property1Caption = "";
				mTIncomeSpecificationRow.Property2Caption = "";
				mTIncomeSpecificationRow.Property2 = "";
				mTIncomeSpecificationRow.LastPurchasePrice = 0m;
				mTIncomeSpecificationRow.VariancePercent = 0m;
				if (e.Value != null && !string.IsNullOrWhiteSpace(e.Value.ToString()))
				{
					Settings.ErrorHandler(ex);
				}
			}
			finally
			{
				RowInfoChangeHandler?.Invoke(mTIncomeSpecificationRow);
			}
		}
		else if (e.Column == _unitCodeCol)
		{
			try
			{
				if (e.Value != null && !string.IsNullOrWhiteSpace(e.Value.ToString()) && !string.IsNullOrWhiteSpace(mTIncomeSpecificationRow.MTCode))
				{
					QntUnitCacheRow item4 = TradeSettings.TradeCacheService.QntUnit.GetItem(e.Value.ToString(), throwExceptionNotExits: true);
					mTIncomeSpecificationRow.UnitBrief = ((item4 == null) ? "" : item4.Brief);
					if (string.IsNullOrWhiteSpace(_priceListType.Code))
					{
						mTIncomeSpecificationRow.Price = ArmSoft.SME.Trade.UI.Helpers.PriceHelper.GetPriceForUnit(oldUnit, e.Value.ToString(), mTIncomeSpecificationRow.Price, mTIncomeSpecificationRow.MTCode);
					}
					else
					{
						decimal priceForGivenPriceListAndVATType = TradeSettings.CreateTradeClient().CommonServices.GetPriceForGivenPriceListAndVATType(_priceListType.Code, _vatType.Code, mTIncomeSpecificationRow.MTID, e.Value.ToString(), _date.EditValue.Value, _currCode.Code);
						if (priceForGivenPriceListAndVATType != 0m)
						{
							mTIncomeSpecificationRow.Price = priceForGivenPriceListAndVATType;
						}
						else
						{
							mTIncomeSpecificationRow.Price = ArmSoft.SME.Trade.UI.Helpers.PriceHelper.GetPriceForUnit(oldUnit, e.Value.ToString(), mTIncomeSpecificationRow.Price, mTIncomeSpecificationRow.MTCode);
						}
					}
					mTIncomeSpecificationRow.DiscountPrice = (mTIncomeSpecificationRow.Price - mTIncomeSpecificationRow.Price * mTIncomeSpecificationRow.Discount / 100m).RoundAs(2);
					mTIncomeSpecificationRow.Summa = (mTIncomeSpecificationRow.Quantity * mTIncomeSpecificationRow.DiscountPrice).RoundAs(2);
					if (!_isMTIncomeDoc)
					{
						if (!string.IsNullOrWhiteSpace(_storage.Code))
						{
							StoragesCacheRow item5 = TradeSettings.TradeCacheService.Storage.GetItem(_storage.Code);
							if (item5 != null && item5.Retail)
							{
								UpdateSalePriceAndSaleSumma(mTIncomeSpecificationRow);
								goto IL_0674;
							}
						}
						FillSalePriceAndSaleSumma(mTIncomeSpecificationRow);
					}
				}
				else
				{
					mTIncomeSpecificationRow.UnitBrief = "";
				}
				goto IL_0674;
				IL_0674:
				if (Settings.ParametersService.ShowGoodsCostInReportsAndDocs() && ShowLastPurchasePrices)
				{
					CalcLastPurchasePricesAdnVariancePercent(mTIncomeSpecificationRow);
				}
				RowInfoChangeHandler?.Invoke(mTIncomeSpecificationRow);
			}
			catch (Exception ex2)
			{
				Settings.ErrorHandler(ex2);
			}
		}
		else if (e.Column == _quantityCol)
		{
			mTIncomeSpecificationRow.Summa = (Convert.ToDecimal(e.Value) * mTIncomeSpecificationRow.DiscountPrice).RoundAs(2);
			if (!_isMTIncomeDoc)
			{
				if (!string.IsNullOrWhiteSpace(_storage.Code))
				{
					StoragesCacheRow item6 = TradeSettings.TradeCacheService.Storage.GetItem(_storage.Code);
					if (item6 != null && item6.Retail)
					{
						UpdateSalePriceAndSaleSumma(mTIncomeSpecificationRow);
						goto IL_0de8;
					}
				}
				FillSalePriceAndSaleSumma(mTIncomeSpecificationRow);
			}
		}
		else if (e.Column == _priceCol)
		{
			mTIncomeSpecificationRow.Price = Convert.ToDecimal(e.Value);
			mTIncomeSpecificationRow.DiscountPrice = (mTIncomeSpecificationRow.Price - mTIncomeSpecificationRow.Price * mTIncomeSpecificationRow.Discount / 100m).RoundAs(2);
			mTIncomeSpecificationRow.Summa = (mTIncomeSpecificationRow.DiscountPrice * mTIncomeSpecificationRow.Quantity).RoundAs(2);
			if (Settings.ParametersService.ShowGoodsCostInReportsAndDocs())
			{
				if (ShowLastPurchasePrices)
				{
					CalcVariancePercent(mTIncomeSpecificationRow);
				}
				else
				{
					CalcLastPurchasePricesAndVariancePercentAndSetToPanel((MTIncomeSpecificationRow)_mtIncomeGrid.Grid.CurrentItem);
				}
			}
			if (!_isMTIncomeDoc)
			{
				if (!string.IsNullOrWhiteSpace(_storage.Code))
				{
					StoragesCacheRow item7 = TradeSettings.TradeCacheService.Storage.GetItem(_storage.Code);
					if (item7 != null && item7.Retail)
					{
						UpdateSalePriceAndSaleSumma(mTIncomeSpecificationRow);
						goto IL_0de8;
					}
				}
				FillSalePriceAndSaleSumma(mTIncomeSpecificationRow);
			}
		}
		else if (e.Column == _discountCol)
		{
			mTIncomeSpecificationRow.DiscountPrice = (mTIncomeSpecificationRow.Price - mTIncomeSpecificationRow.Price * mTIncomeSpecificationRow.Discount / 100m).RoundAs(2);
			mTIncomeSpecificationRow.Summa = (mTIncomeSpecificationRow.DiscountPrice * mTIncomeSpecificationRow.Quantity).RoundAs(2);
			if (Settings.ParametersService.ShowGoodsCostInReportsAndDocs())
			{
				if (ShowLastPurchasePrices)
				{
					CalcVariancePercent(mTIncomeSpecificationRow);
				}
				else
				{
					CalcLastPurchasePricesAndVariancePercentAndSetToPanel((MTIncomeSpecificationRow)_mtIncomeGrid.Grid.CurrentItem);
				}
			}
			if (!_isMTIncomeDoc)
			{
				if (!string.IsNullOrWhiteSpace(_storage.Code))
				{
					StoragesCacheRow item8 = TradeSettings.TradeCacheService.Storage.GetItem(_storage.Code);
					if (item8 != null && item8.Retail)
					{
						UpdateSalePriceAndSaleSumma(mTIncomeSpecificationRow);
						goto IL_0de8;
					}
				}
				FillSalePriceAndSaleSumma(mTIncomeSpecificationRow);
			}
		}
		else if (e.Column == _discountPriceCol)
		{
			mTIncomeSpecificationRow.DiscountPrice = Convert.ToDecimal(e.Value);
			mTIncomeSpecificationRow.Summa = (mTIncomeSpecificationRow.DiscountPrice * mTIncomeSpecificationRow.Quantity).RoundAs(2);
			if (Settings.ParametersService.ShowGoodsCostInReportsAndDocs())
			{
				if (ShowLastPurchasePrices)
				{
					CalcVariancePercent(mTIncomeSpecificationRow);
				}
				else
				{
					CalcLastPurchasePricesAndVariancePercentAndSetToPanel((MTIncomeSpecificationRow)_mtIncomeGrid.Grid.CurrentItem);
				}
			}
			if (mTIncomeSpecificationRow.Price != 0m)
			{
				mTIncomeSpecificationRow.Discount = (100m * (mTIncomeSpecificationRow.Price - mTIncomeSpecificationRow.DiscountPrice) / mTIncomeSpecificationRow.Price).RoundAs(2);
				if (mTIncomeSpecificationRow.Discount / 1000m >= 1m || mTIncomeSpecificationRow.Discount / 1000m <= -1m)
				{
					mTIncomeSpecificationRow.Discount = 0m;
					mTIncomeSpecificationRow.DiscountPrice = mTIncomeSpecificationRow.Price;
					mTIncomeSpecificationRow.Summa = mTIncomeSpecificationRow.Price * mTIncomeSpecificationRow.Quantity;
				}
			}
			else if (mTIncomeSpecificationRow.Discount != 100m)
			{
				mTIncomeSpecificationRow.Price = (100m * mTIncomeSpecificationRow.DiscountPrice / (100m - mTIncomeSpecificationRow.Discount)).RoundAs(2);
			}
			if (!_isMTIncomeDoc)
			{
				if (!string.IsNullOrWhiteSpace(_storage.Code))
				{
					StoragesCacheRow item9 = TradeSettings.TradeCacheService.Storage.GetItem(_storage.Code);
					if (item9 != null && item9.Retail)
					{
						UpdateSalePriceAndSaleSumma(mTIncomeSpecificationRow);
						goto IL_0de8;
					}
				}
				FillSalePriceAndSaleSumma(mTIncomeSpecificationRow);
			}
		}
		else if (e.Column == _summaCol && mTIncomeSpecificationRow.Quantity != 0m)
		{
			mTIncomeSpecificationRow.Summa = Convert.ToDecimal(e.Value);
			if (mTIncomeSpecificationRow.Quantity > 0m)
			{
				mTIncomeSpecificationRow.DiscountPrice = (mTIncomeSpecificationRow.Summa / mTIncomeSpecificationRow.Quantity).RoundAs(2);
				if (!Settings.ParametersService.AllowDiscountInPurchase())
				{
					mTIncomeSpecificationRow.Price = mTIncomeSpecificationRow.DiscountPrice;
				}
			}
			if (mTIncomeSpecificationRow.Price == 0m)
			{
				if (mTIncomeSpecificationRow.Discount != 100m)
				{
					mTIncomeSpecificationRow.Price = (100m * mTIncomeSpecificationRow.DiscountPrice / (100m - mTIncomeSpecificationRow.Discount)).RoundAs(2);
				}
			}
			else
			{
				mTIncomeSpecificationRow.Discount = (100m * (mTIncomeSpecificationRow.Price - mTIncomeSpecificationRow.DiscountPrice) / mTIncomeSpecificationRow.Price).RoundAs(2);
			}
			if (!_isMTIncomeDoc)
			{
				if (!string.IsNullOrWhiteSpace(_storage.Code))
				{
					StoragesCacheRow item10 = TradeSettings.TradeCacheService.Storage.GetItem(_storage.Code);
					if (item10 != null && item10.Retail)
					{
						UpdateSalePriceAndSaleSumma(mTIncomeSpecificationRow);
						goto IL_0de8;
					}
				}
				FillSalePriceAndSaleSumma(mTIncomeSpecificationRow);
			}
		}
		else if (e.Column == _property1Col)
		{
			MaterialCacheRow itemById = TradeSettings.TradeCacheService.Material.GetItemById(mTIncomeSpecificationRow.MTID);
			string destCode = ((itemById != null) ? itemById.Property1 : string.Empty);
			_property1Col.DataView = new ArmSoft.SME.Trade.UI.DropDown.PropertyList(destCode);
			if (!string.IsNullOrWhiteSpace(mTIncomeSpecificationRow.Property1Caption))
			{
				PropertyListCacheRow.RowKey key = new PropertyListCacheRow.RowKey
				{
					DestCode = destCode,
					Code = mTIncomeSpecificationRow.Property1
				};
				PropertyListCacheRow item11 = TradeSettings.TradeCacheService.PropertyList.GetItem(key);
				if (item11 != null)
				{
					mTIncomeSpecificationRow.Property1 = item11.Code;
				}
			}
			else
			{
				mTIncomeSpecificationRow.Property1 = string.Empty;
			}
		}
		else if (e.Column == _property2Col)
		{
			MaterialCacheRow itemById2 = TradeSettings.TradeCacheService.Material.GetItemById(mTIncomeSpecificationRow.MTID);
			string destCode2 = ((itemById2 != null) ? itemById2.Property2 : string.Empty);
			_property2Col.DataView = new ArmSoft.SME.Trade.UI.DropDown.PropertyList(destCode2);
			if (!string.IsNullOrWhiteSpace(mTIncomeSpecificationRow.Property2Caption))
			{
				PropertyListCacheRow.RowKey key2 = new PropertyListCacheRow.RowKey
				{
					DestCode = destCode2,
					Code = mTIncomeSpecificationRow.Property2
				};
				PropertyListCacheRow item12 = TradeSettings.TradeCacheService.PropertyList.GetItem(key2);
				if (item12 != null)
				{
					mTIncomeSpecificationRow.Property2 = item12.Code;
				}
			}
			else
			{
				mTIncomeSpecificationRow.Property2 = string.Empty;
			}
		}
		goto IL_0de8;
		IL_0de8:
		if (!_isMTIncomeDoc)
		{
			if (e.Column == _extraPercentCol)
			{
				UpdateSalePriceAndSaleSumma(mTIncomeSpecificationRow);
			}
			else if (e.Column == _salePriceCol)
			{
				mTIncomeSpecificationRow.SaleSumma = ((decimal)e.Value * mTIncomeSpecificationRow.Quantity).RoundAs(2);
				mTIncomeSpecificationRow.ExtraPercent = ArmSoft.SME.Trade.UI.Helpers.PriceHelper.CalculateExtraPercent(mTIncomeSpecificationRow.DiscountPrice, (decimal)e.Value, mTIncomeSpecificationRow.VAT, _vatType.Code, _currCode.Code, new ExchangeRate(_currExChngRate.Base, _currExChngRate.Rate));
			}
			else if (e.Column == _vatCol)
			{
				UpdateSalePriceAndSaleSumma(mTIncomeSpecificationRow);
			}
		}
		_mtIncomeGrid.Grid.RefreshData();
		CalcSummAndSummVAT();
	}

	protected void MTIncomeTableView_FocusedRowHandleChanged(object sender, FocusedRowHandleChangedEventArgs e)
	{
		int focusedRowHandle = _mtIncomeTableView.FocusedRowHandle;
		MTIncomeSpecificationRow mTIncomeSpecificationRow = (MTIncomeSpecificationRow)_mtIncomeGrid.Grid.GetRow(focusedRowHandle);
		if (mTIncomeSpecificationRow != null)
		{
			ChangeColumnAdditonalView(mTIncomeSpecificationRow.MTCode);
		}
		RowInfoChangeHandler?.Invoke(mTIncomeSpecificationRow);
	}

	protected void MTIncomeTableView_ShowingEditor(object sender, ShowingEditorEventArgs e)
	{
		if (((!_property1Col.Visible || e.Column != _property1Col) && (!_property2Col.Visible || e.Column != _property2Col)) || e.Row == null)
		{
			return;
		}
		MTIncomeSpecificationRow mTIncomeSpecificationRow = (MTIncomeSpecificationRow)e.Row;
		MaterialCacheRow itemById = TradeSettings.TradeCacheService.Material.GetItemById(mTIncomeSpecificationRow.MTID);
		if (itemById != null)
		{
			if ((string.IsNullOrWhiteSpace(itemById.Property1) && e.Column == _property1Col) || (string.IsNullOrWhiteSpace(itemById.Property2) && e.Column == _property2Col))
			{
				e.Cancel = true;
			}
		}
		else
		{
			e.Cancel = true;
		}
	}

	protected void ChangeColumnAdditonalView(string mtCode)
	{
		_unitCodeCol.DataView = new ArmSoft.SME.Trade.UI.DropDown.MTQntUnits(mtCode);
	}

	protected void Storage_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		if (!_storage.IsReadOnly && !string.IsNullOrWhiteSpace(Convert.ToString(e.NewValue)))
		{
			StoragesCacheRow item = TradeSettings.TradeCacheService.Storage.GetItem(Convert.ToString(e.NewValue));
			if (item != null)
			{
				if (!TradeSettings.CreateTradeClient().CommonServices.IsStorageAvailable(Convert.ToString(e.NewValue)))
				{
					e.Handled = true;
					_storage.Code = Convert.ToString(e.OldValue);
					ArmSoft.SME.Core.UI.MessageBox.Show(string.Format(ArmSoft.SME.Trade.Common.Resources.StorageNotAvailForUser, Settings.LoginService.UserName, Convert.ToString(e.NewValue)), Settings.ProgramName, MessageBoxButton.OK, MessageBoxImage.Exclamation);
				}
				else
				{
					_response.Text = item.Response;
				}
			}
		}
		RowInfoChangeHandler?.Invoke((MTIncomeSpecificationRow)_mtIncomeTableView.Grid.CurrentItem);
	}

	protected void DiscountPercent_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		if (e.NewValue == null)
		{
			return;
		}
		if (Convert.ToDecimal(e.NewValue.ToString()) != Convert.ToDecimal(e.OldValue.ToString()) && _mtIncomeList.Count > 0)
		{
			if (Convert.ToDecimal(e.OldValue) != 0m || GetDiscountUse())
			{
				if (ArmSoft.SME.Core.UI.MessageBox.Show(string.Format(ArmSoft.SME.Common.Resources.ComfirmDiscountChange, default(ReadOnlySpan<object>)), Settings.ProgramName, MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No) == MessageBoxResult.Yes)
				{
					CalculateDiscount(ref _mtIncomeList, Convert.ToDecimal(e.NewValue));
					CalcSummAndSummVAT();
					if (Settings.ParametersService.ShowGoodsCostInReportsAndDocs())
					{
						if (ShowLastPurchasePrices)
						{
							CalcVariancePercent();
						}
						else
						{
							CalcLastPurchasePricesAndVariancePercentAndSetToPanel((MTIncomeSpecificationRow)_mtIncomeGrid.Grid.CurrentItem);
						}
					}
					_mtIncomeGrid.Grid.RefreshData();
				}
				else
				{
					_discountPercent.EditValue = Convert.ToDecimal(e.OldValue);
					_discountPercent.DoValidate();
				}
				return;
			}
			CalculateDiscount(ref _mtIncomeList, Convert.ToDecimal(e.NewValue));
			CalcSummAndSummVAT();
			if (Settings.ParametersService.ShowGoodsCostInReportsAndDocs())
			{
				if (ShowLastPurchasePrices)
				{
					CalcVariancePercent();
				}
				else
				{
					CalcLastPurchasePricesAndVariancePercentAndSetToPanel((MTIncomeSpecificationRow)_mtIncomeGrid.Grid.CurrentItem);
				}
			}
			_mtIncomeGrid.Grid.RefreshData();
		}
		else
		{
			CalculateDiscount(ref _mtIncomeList, Convert.ToDecimal(e.NewValue));
			CalcSummAndSummVAT();
			_mtIncomeGrid.Grid.RefreshData();
		}
	}

	protected bool GetDiscountUse()
	{
		return _mtIncomeList.Any((MTIncomeSpecificationRow row) => row.Price != row.DiscountPrice);
	}

	protected void CalculateDiscount(ref ObservableCollection<MTIncomeSpecificationRow> MTList, decimal Discount, bool gDiscount = true)
	{
		if (gDiscount)
		{
			foreach (MTIncomeSpecificationRow MT in MTList)
			{
				MT.Discount = Discount;
				MT.DiscountPrice = (MT.Price * (1m - Discount / 100m)).RoundAs(2);
				MT.Summa = (MT.Quantity * MT.DiscountPrice).RoundAs(2);
			}
			return;
		}
		foreach (MTIncomeSpecificationRow MT2 in MTList)
		{
			MT2.DiscountPrice = (MT2.Price * (1m - MT2.Discount / 100m)).RoundAs(2);
			MT2.Summa = (MT2.Quantity * MT2.DiscountPrice).RoundAs(2);
		}
	}
}
