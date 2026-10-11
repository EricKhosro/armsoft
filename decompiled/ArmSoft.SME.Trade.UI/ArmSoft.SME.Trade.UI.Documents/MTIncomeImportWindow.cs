using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using Ardalis.SmartEnum;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.CustomTypes;
using ArmSoft.SME.Common.Extensions;
using ArmSoft.SME.Common.Models;
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

[DocumentWindow(19)]
public class MTIncomeImportWindow : TradeDocumentWindow<MTIncomeImport>
{
	public class MTIncomeImportSpecificationRow : DocumentGridRow
	{
		[ObservableProperty]
		private int mTID = -1;

		[ObservableProperty]
		private string mTCode = "";

		[ObservableProperty]
		private string mTName;

		[ObservableProperty]
		private string unitBrief;

		[ObservableProperty]
		private string unitCode = "";

		[ObservableProperty]
		private decimal quantity;

		[ObservableProperty]
		private decimal price;

		[ObservableProperty]
		private decimal summa;

		[ObservableProperty]
		private decimal customSumma;

		[ObservableProperty]
		private decimal customPercent;

		[ObservableProperty]
		private decimal customDuty;

		[ObservableProperty]
		private string vATCodeForConvert;

		[ObservableProperty]
		private bool vAT;

		[ObservableProperty]
		private decimal vATSumma;

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
		private string fullName;

		[ObservableProperty]
		private string producerCaption;

		[ObservableProperty]
		private string property1Caption;

		[ObservableProperty]
		private string property2Caption;

		[ObservableProperty]
		private string rowDescription;

		[ObservableProperty]
		private decimal lastPurchasePrice;

		[ObservableProperty]
		private decimal variancePercent;

		public short RowNum;

		[ObservableProperty]
		private int rowID;

		public bool UpdateRemainsOnInfoPanel = true;

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
					OnMTNameChanged(value);
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
		public decimal CustomSumma
		{
			get
			{
				return customSumma;
			}
			set
			{
				if (!EqualityComparer<decimal>.Default.Equals(customSumma, value))
				{
					OnPropertyChanging(__KnownINotifyPropertyChangingArgs.CustomSumma);
					customSumma = value;
					OnCustomSummaChanged(value);
					OnPropertyChanged(__KnownINotifyPropertyChangedArgs.CustomSumma);
				}
			}
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public decimal CustomPercent
		{
			get
			{
				return customPercent;
			}
			set
			{
				if (!EqualityComparer<decimal>.Default.Equals(customPercent, value))
				{
					OnPropertyChanging(__KnownINotifyPropertyChangingArgs.CustomPercent);
					customPercent = value;
					OnPropertyChanged(__KnownINotifyPropertyChangedArgs.CustomPercent);
				}
			}
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public decimal CustomDuty
		{
			get
			{
				return customDuty;
			}
			set
			{
				if (!EqualityComparer<decimal>.Default.Equals(customDuty, value))
				{
					OnPropertyChanging(__KnownINotifyPropertyChangingArgs.CustomDuty);
					customDuty = value;
					OnPropertyChanged(__KnownINotifyPropertyChangedArgs.CustomDuty);
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
		public decimal VATSumma
		{
			get
			{
				return vATSumma;
			}
			set
			{
				if (!EqualityComparer<decimal>.Default.Equals(vATSumma, value))
				{
					OnPropertyChanging(__KnownINotifyPropertyChangingArgs.VATSumma);
					vATSumma = value;
					OnPropertyChanged(__KnownINotifyPropertyChangedArgs.VATSumma);
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

		static MTIncomeImportSpecificationRow()
		{
			DocumentGridRow.RegisterPropertyValidators(typeof(MTIncomeImportSpecificationRow), cacheValidationResults: true, new Dictionary<string, Action<DocumentGridRow, DevExpress.XtraEditors.DXErrorProvider.ErrorInfo>>
			{
				{
					"MTCode",
					(DocumentGridRow row, DevExpress.XtraEditors.DXErrorProvider.ErrorInfo info) =>
					{
						ValidateGoodCode((MTIncomeImportSpecificationRow)row, info);
					}
				},
				{
					"UnitCode",
					(DocumentGridRow row, DevExpress.XtraEditors.DXErrorProvider.ErrorInfo info) =>
					{
						ValidateUnitCode((MTIncomeImportSpecificationRow)row, info);
					}
				},
				{
					"Quantity",
					(DocumentGridRow row, DevExpress.XtraEditors.DXErrorProvider.ErrorInfo info) =>
					{
						ValidateQuantity((MTIncomeImportSpecificationRow)row, info);
					}
				},
				{
					"Summa",
					(DocumentGridRow row, DevExpress.XtraEditors.DXErrorProvider.ErrorInfo info) =>
					{
						ValidateSumma((MTIncomeImportSpecificationRow)row, info);
					}
				},
				{
					"CustomSumma",
					(DocumentGridRow row, DevExpress.XtraEditors.DXErrorProvider.ErrorInfo info) =>
					{
						ValidateCustomSumma((MTIncomeImportSpecificationRow)row, info);
					}
				},
				{
					"Producer",
					(DocumentGridRow row, DevExpress.XtraEditors.DXErrorProvider.ErrorInfo info) =>
					{
						ValidateProducer((MTIncomeImportSpecificationRow)row, info);
					}
				},
				{
					"Property1",
					(DocumentGridRow row, DevExpress.XtraEditors.DXErrorProvider.ErrorInfo info) =>
					{
						ValidateProperty1((MTIncomeImportSpecificationRow)row, info);
					}
				},
				{
					"Property2",
					(DocumentGridRow row, DevExpress.XtraEditors.DXErrorProvider.ErrorInfo info) =>
					{
						ValidateProperty2((MTIncomeImportSpecificationRow)row, info);
					}
				},
				{
					"Property1Caption",
					(DocumentGridRow row, DevExpress.XtraEditors.DXErrorProvider.ErrorInfo info) =>
					{
						ValidateProperty1Caption((MTIncomeImportSpecificationRow)row, info);
					}
				},
				{
					"Property2Caption",
					(DocumentGridRow row, DevExpress.XtraEditors.DXErrorProvider.ErrorInfo info) =>
					{
						ValidateProperty2Caption((MTIncomeImportSpecificationRow)row, info);
					}
				}
			});
		}

		protected static void ValidateGoodCode(MTIncomeImportSpecificationRow row, DevExpress.XtraEditors.DXErrorProvider.ErrorInfo info)
		{
			TradePropertyValidator.ValidateMaterialCode(row.MTCode, info, TradeAllDocDefinitions.MTIncomeImport.Grids["MTList"].Columns["MTCode"]);
		}

		protected static void ValidateUnitCode(MTIncomeImportSpecificationRow row, DevExpress.XtraEditors.DXErrorProvider.ErrorInfo info)
		{
			TradePropertyValidator.ValidateGoodUnitCode(row.UnitCode, row.MTCode, info, TradeAllDocDefinitions.MTIncomeImport.Grids["MTList"].Columns["UnitCode"]);
		}

		protected static void ValidateCountry(MTIncomeImportSpecificationRow row, DevExpress.XtraEditors.DXErrorProvider.ErrorInfo info)
		{
			if (!string.IsNullOrWhiteSpace(row.Country))
			{
				TradePropertyValidator.ValidateCountry(row.Country, info, TradeAllDocDefinitions.MTIncomeImport.Grids["MTList"].Columns["Country"]);
			}
		}

		protected static void ValidateProducer(MTIncomeImportSpecificationRow row, DevExpress.XtraEditors.DXErrorProvider.ErrorInfo info)
		{
			if (!string.IsNullOrWhiteSpace(row.Producer))
			{
				TradePropertyValidator.ValidateProducer(row.Producer, info, TradeAllDocDefinitions.MTIncomeImport.Grids["MTList"].Columns["Producer"]);
			}
		}

		protected static void ValidateProperty1(MTIncomeImportSpecificationRow row, DevExpress.XtraEditors.DXErrorProvider.ErrorInfo info)
		{
			if (!string.IsNullOrWhiteSpace(row.Property1))
			{
				TradePropertyValidator.ValidateProperty(row.Property1, row.MTCode, isProperty1: true, info, TradeAllDocDefinitions.MTIncomeImport.Grids["MTList"].Columns["Property1"]);
			}
		}

		protected static void ValidateProperty2(MTIncomeImportSpecificationRow row, DevExpress.XtraEditors.DXErrorProvider.ErrorInfo info)
		{
			if (!string.IsNullOrWhiteSpace(row.Property2))
			{
				TradePropertyValidator.ValidateProperty(row.Property2, row.MTCode, isProperty1: false, info, TradeAllDocDefinitions.MTIncomeImport.Grids["MTList"].Columns["Property2"]);
			}
		}

		protected static void ValidateProperty1Caption(MTIncomeImportSpecificationRow row, DevExpress.XtraEditors.DXErrorProvider.ErrorInfo info)
		{
			if (!string.IsNullOrWhiteSpace(row.Property1Caption))
			{
				TradePropertyValidator.ValidatePropertyValue(row.Property1Caption, row.MTCode, isProperty1: true, info, TradeAllDocDefinitions.MTIncomeImport.Grids["MTList"].Columns["Property1Caption"]);
			}
		}

		protected static void ValidateProperty2Caption(MTIncomeImportSpecificationRow row, DevExpress.XtraEditors.DXErrorProvider.ErrorInfo info)
		{
			if (!string.IsNullOrWhiteSpace(row.Property2Caption))
			{
				TradePropertyValidator.ValidatePropertyValue(row.Property2Caption, row.MTCode, isProperty1: false, info, TradeAllDocDefinitions.MTIncomeImport.Grids["MTList"].Columns["Property2Caption"]);
			}
		}

		private static void ValidateQuantity(MTIncomeImportSpecificationRow row, DevExpress.XtraEditors.DXErrorProvider.ErrorInfo info)
		{
			PropertyValidator.PositiveValidate(row.Quantity, info, ArmSoft.SME.Common.Resources.Quantity);
		}

		private static void ValidateSumma(MTIncomeImportSpecificationRow row, DevExpress.XtraEditors.DXErrorProvider.ErrorInfo info)
		{
			PropertyValidator.PositiveValidate(row.Summa, info, ArmSoft.SME.Common.Resources.Summa);
		}

		private static void ValidateCustomSumma(MTIncomeImportSpecificationRow row, DevExpress.XtraEditors.DXErrorProvider.ErrorInfo info)
		{
			PropertyValidator.PositiveValidate(row.CustomSumma, info, ArmSoft.SME.Trade.Common.Resources.CustomSumma);
		}

		public MTIncomeImportSpecificationRow()
		{
		}

		public MTIncomeImportSpecificationRow(MTIncomeImportWindow docWindow)
			: base(docWindow)
		{
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		private void OnMTCodeChanged(string value)
		{
			ResetForceValidationBit("MTCode", "MTName", "UnitCode");
			MaterialCacheRow item = TradeSettings.TradeCacheService.Material.GetItem(MTCode);
			MTName = ((item == null) ? "" : item.Caption);
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		private void OnMTNameChanged(string value)
		{
			ResetForceValidationBit("Name");
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
			ResetForceValidationBit("Quantity");
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
			Price = ((Quantity == 0m) ? 0m : (value / Quantity).RoundAs(2));
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		private void OnCustomSummaChanged(decimal value)
		{
			ResetForceValidationBit("CustomSumma");
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

	public class MTIncomeImportFilteredSearchList : IResultList
	{
		private readonly ObservableCollection<MTIncomeImportSpecificationRow> _itemSource = new ObservableCollection<MTIncomeImportSpecificationRow>();

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
			int num2 = _itemSource.FindIndex((MTIncomeImportSpecificationRow x) => x.MTCode == mtCode);
			if (num2 == -1)
			{
				MaterialCacheRow item = TradeSettings.TradeCacheService.Material.GetItem(mtCode);
				MTIncomeImportSpecificationRow item2 = new MTIncomeImportSpecificationRow
				{
					MTID = item.Id,
					MTCode = item.MTCode,
					MTName = item.Caption,
					UnitCode = item.Unit,
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

	protected delegate void RowInfoChangeEventHandler(MTIncomeImportSpecificationRow Row);

	private delegate void DelegateSetRowInfoToInfoPanel(MTIncomeImportSpecificationRow Row);

	private delegate void DelegateClearInfoPanel();

	private Button _overHeads;

	private DropDownControl _storage;

	private TextEditExt _barCode;

	private Button _codeEnter;

	private BarEditItemExt _viewAvailAndTurn;

	private PartnerContractGroupControls _supplierContractContainer;

	private PartnerContractGroupControls _customContractContainer;

	private PopupCalcEditExt _customFee;

	private LayoutGroup _infoDisplay;

	private TextEditExt _infoItemName;

	private InfoDisplayControls _infoDisplayControls;

	private TextEditExt _invoiceNumber;

	private ComboBoxEditExt _comment;

	private DropDownControl _owner;

	public DropDownControl _currCode;

	public ExchangeRateControl _currExChngRate;

	public DropDownControl _priceListType;

	public DocumentGridControl _mtIncomeImportGrid;

	private TableView _mtIncomeImportTableView;

	private BarEditItemExt _filteredSearch;

	private BarEditItemExt _actualQuantitiesCheck;

	public ObservableCollection<MTIncomeImportSpecificationRow> _mtIncomeImportList;

	private DocumentGridColumn _codeCol;

	private DocumentGridColumn _unitCodeCol;

	private DocumentGridColumn _quantityCol;

	private DocumentGridColumn _lastPurchasePriceCol;

	private DocumentGridColumn _variancePercentCol;

	private DocumentGridColumn _priceCol;

	private DocumentGridColumn _summaCol;

	private DocumentGridColumn _customSummaCol;

	private DocumentGridColumn _customPercentCol;

	private DocumentGridColumn _customDutyCol;

	private DocumentGridColumn _vatCol;

	private DocumentGridColumn _vatSummaCol;

	private DocumentGridColumn _useLifeCol;

	private DocumentGridColumn _countryCol;

	private DocumentGridColumn _producerCol;

	private DocumentGridColumn _property1Col;

	private DocumentGridColumn _property2Col;

	private DocumentGridColumn _rowDescCol;

	private PopupCalcEditExt _totalSumma;

	private PopupCalcEditExt _customDuty;

	private PopupCalcEditExt _vatSumma;

	private TextEditExt _trust;

	private DropDownControl _mediator;

	private TextEditExt _machine;

	private TextEditExt _chief;

	private DropDownCommentControl _allower;

	private DropDownCommentControl _response;

	private DXTabControl _tabs;

	private int _priceListTabControlIndex;

	protected bool InfoPanelCollapsed;

	private bool _isChanged = true;

	private Dictionary<(int, string), decimal> _availabilityByGoodId = new Dictionary<(int, string), decimal>();

	private SafeDocumentEventHandler<FocusedRowHandleChangedEventArgs> _focusedRowHandleChangedEvent;

	private string _oldDocCurrCode;

	private decimal _oldDocCurrExChngRate = 1m;

	private decimal _oldDocCurrExChngBase = 1m;

	public int _maxRowID;

	private bool _isAskQuestionConfirmSummChange = true;

	private bool _showCostCalculationReport = Settings.UserAccessService.GetProviderPermission(DataProviderTypeSE.GoodsCostCalculation).AllowView;

	private decimal _mTradeConstVAT;

	private string _rowAddMode = Settings.ParametersService.ScannerMode();

	private bool _showLastPurchasePrices;

	private BarButtonItem _barButtonDocumentSettings = new BarButtonItem();

	private BarSubItem _barSubItemActions = new BarSubItem();

	private BarButtonItem _barButtonPrintLabel = new BarButtonItem();

	private BarButtonItem _barButtonPriceFormation = new BarButtonItem();

	private BarButtonItem _barButtonImportFromFile = new BarButtonItem();

	private ReportBarButtonItem _barButtonJumpTo;

	private ReportBarButtonItem _barButtonGoodsOperationsJournal;

	private ReportBarButtonItem _barButtonPartnerOperationJournal;

	private BarSubItem _barSubItemJournals = new BarSubItem();

	protected override double WidthValue => 1090.0;

	protected override double HeightValue => 600.0;

	protected override double MinWidthValue => 700.0;

	protected override double MinHeightValue => 250.0;

	protected event RowInfoChangeEventHandler RowInfoChange;

	protected override bool AllowPrint()
	{
		return true;
	}

	protected override bool AllowPrintOnNew()
	{
		return true;
	}

	public MTIncomeImportWindow()
	{
	}

	public MTIncomeImportWindow(MTIncomeImport doc)
		: base(doc)
	{
	}

	protected override void ConstructUI()
	{
		base.ConstructUI();
		_mTradeConstVAT = Settings.ParametersService.VAT();
		_showLastPurchasePrices = Settings.ParametersService.ShowLastPurchasePrices();
		LayoutGroupExt container = AddLayoutGroup(null, LayoutGroupView.Group, Orientation.Horizontal);
		_date = AddDateControl(container, Definition.Fields["DocDate"]);
		_date.Margin = new Thickness(0.0, 0.0, 10.0, 0.0);
		_date.EditValueChanged += new SafeDocumentEventHandler<EditValueChangedEventArgs>(Date_EditValueChanged, this).Handler;
		_docNum = AddButtonEditControl(container, Definition.Fields["DocNum"], 1);
		_docNum.AllowMagicButton = true;
		_docNum.MagicToolTip = ArmSoft.SME.Common.Resources.DocNextNumbers;
		_docNum.MagicClick += new SafeDocumentEventHandler<MagicClickEventArgs>(DocNum_MagicClick, this).Handler;
		_docNum.EditValueChanged += new SafeDocumentEventHandler<EditValueChangedEventArgs>(DocNum_EditValueChanged, this).Handler;
		_storage = AddDropDownControl(null, Definition.Fields["Storage"], new ArmSoft.SME.Trade.UI.DropDown.Storages(false));
		_storage.EditValueChanged += new SafeDocumentEventHandler<EditValueChangedEventArgs>(Storage_EditValueChanged, this).Handler;
		if (_showCostCalculationReport)
		{
			_overHeads = AddButton(container, "", ArmSoft.SME.Trade.Common.Resources.GoodsCostCalculation);
			_overHeads.Click += new SafeDocumentEventHandler<RoutedEventArgs>(OverHeads_Click, this).Handler;
		}
		LayoutGroupExt container2 = AddLayoutGroup(null, LayoutGroupView.Group, Orientation.Horizontal);
		_barCode = AddStringControl(container2, ArmSoft.SME.Common.Resources.BarCode + " (F2)", isRequired: false, 20, HorizontalAlignment.Stretch);
		_barCode.NullText = ArmSoft.SME.Trade.Common.Resources.BarCodeAndCodeInputNullText;
		_barCode.KeyDown += new SafeDocumentEventHandler<KeyEventArgs>(BarCode_KeyDown, this).Handler;
		string text = ArmSoft.SME.Trade.Common.Resources.cmdCodeEnter + " (F8)";
		_codeEnter = AddButton(container2, "", text, "", text, isTabStop: true);
		_codeEnter.Click += new SafeDocumentEventHandler<RoutedEventArgs>(OnCodeEnter_Click, this).Handler;
		_tabs = AddTabControl(null);
		LayoutGroup container3 = AddTabbedGroup(_tabs, ArmSoft.SME.Common.Resources.General, "General");
		LayoutGroupExt container4 = AddLayoutGroup(container3, LayoutGroupView.GroupBox, Orientation.Vertical);
		_supplierContractContainer = AddPartnerContractGroupControls(container4, (PartnerContractGroupDefinition)Definition.Fields["PartnerContractGroup"], _date, null, null);
		_supplierContractContainer.PartnerCode.IsRequired = false;
		_supplierContractContainer.PartnerCodeEditValueChanged += new SafeDocumentEventHandler<EditValueChangedEventArgs>(PartnerCode_EditValueChanged, this).Handler;
		_supplierContractContainer.PartnerCodeEditAdditionalValueChanged += new SafeDocumentEventHandler<EventArgs>(PartnerCode_EditAdditionalValueChanged, this).Handler;
		_supplierContractContainer.ContractCodeEditValueChanged += new SafeDocumentEventHandler<EventArgs>(PartnerContractCode_EditValueChanged, this).Handler;
		LayoutGroupExt container5 = AddLayoutGroup(container3, LayoutGroupView.GroupBox, Orientation.Vertical);
		_customContractContainer = AddPartnerContractGroupControls(container5, (PartnerContractGroupDefinition)Definition.Fields["CustomContractContainer"], _date, null, null);
		_customContractContainer.PartnerCodeEditValueChanged += new SafeDocumentEventHandler<EditValueChangedEventArgs>(CustomCode_EditValueChanged, this).Handler;
		_customContractContainer.PartnerCodeEditAdditionalValueChanged += new SafeDocumentEventHandler<EventArgs>(CustomCode_EditAdditionalValueChanged, this).Handler;
		_customContractContainer.ContractCodeEditValueChanged += new SafeDocumentEventHandler<EventArgs>(CustomContractCode_EditValueChanged, this).Handler;
		_customContractContainer.PartnerCode.IsRequired = false;
		_customFee = AddNumericControl(container5, Definition.Fields["CustomFee"]);
		_customFee.EditValueChanged += new SafeDocumentEventHandler<EditValueChangedEventArgs>(CustomFee_EditValueChanged, this).Handler;
		_invoiceNumber = AddStringControl(container3, Definition.Fields["InvoiceNumber"]);
		_comment = AddComboBoxControl(container3, Definition.Fields["Comment"], TradeSettings.TradeCacheService.StandardComments.GetCaptionsList(), HorizontalAlignment.Stretch);
		_owner = AddDropDownControl(container3, Definition.Fields["Owner"], new UsersFilter());
		_owner.CodeWidth = 12;
		_owner.ShowComment = false;
		_owner.InnerCodeEdit.Width = _owner.InnerCodeEdit.MinWidth;
		if (!Settings.LoginService.IsAdmin && Settings.ParametersService.DenyAccessToOtherOwnerDocs())
		{
			((LayoutItemWithRequired)(object)_owner.Parent).Visibility = Visibility.Collapsed;
		}
		LayoutGroup container6 = AddTabbedGroup(_tabs, ArmSoft.SME.Trade.Common.Resources.MTLIST, "MtList");
		_priceListTabControlIndex = _tabs.Items.Count - 1;
		LayoutGroupExt container7 = AddLayoutGroup(container6, LayoutGroupView.Group, Orientation.Horizontal);
		_currCode = AddDropDownControl(container7, Definition.Fields["CurrCode"], new Currency(shortMode: true), allowAdditionButtons: false, fieldWithCaption: true, HorizontalAlignment.Stretch);
		_currCode.EditValueChanged += new SafeDocumentEventHandler<EditValueChangedEventArgs>(CurrCode_EditValueChanged, this).Handler;
		_totalSumma = AddNumericControl(container7, Definition.Fields["Summ"], useCurrencyFormat: true, HorizontalAlignment.Right);
		LayoutGroupExt container8 = AddLayoutGroup(container6, LayoutGroupView.Group, Orientation.Horizontal);
		_currExChngRate = AddExchangeRateControl(container8, Definition.Fields["ExchangeRate"]);
		_currExChngRate.EditValueChanged += new SafeDocumentEventHandler<EditValueChangedEventArgs>(CurrExChngRate_EditValueChanged, this).Handler;
		_customDuty = AddNumericControl(container8, Definition.Fields["CustomDuty"], useCurrencyFormat: true, HorizontalAlignment.Right);
		LayoutGroupExt container9 = AddLayoutGroup(container6, LayoutGroupView.Group, Orientation.Horizontal);
		_priceListType = AddDropDownControl(container9, Definition.Fields["PriceType"], new ArmSoft.SME.Trade.UI.DropDown.PriceTypes());
		_priceListType.EditValueChanged += new SafeDocumentEventHandler<EditValueChangedEventArgs>(PriceListType_EditValueChanged, this).Handler;
		_vatSumma = AddNumericControl(container9, Definition.Fields["SummVAT"], useCurrencyFormat: true, HorizontalAlignment.Right);
		_mtIncomeImportGrid = AddGrid(container6, Definition.Grids["MTList"], HorizontalAlignment.Stretch, VerticalAlignment.Stretch, isSupportDialogMode: true);
		_viewAvailAndTurn = _mtIncomeImportGrid.AddButton(null, ArmSoft.SME.Trade.Common.Resources.AvailAndTurn, ArmSoft.SME.Trade.Common.Resources.AvailabilityAndTurn, 130.0);
		_viewAvailAndTurn.Click += new SafeDocumentEventHandler<RoutedEventArgs>(ViewAvailAndTurn_Click, this).Handler;
		_filteredSearch = _mtIncomeImportGrid.AddButton(ArmSoft.SME.Trade.Common.Resources.GroupAddition, ArmSoft.SME.Trade.Common.Resources.GroupAddition, "", 130.0);
		_filteredSearch.Click += new SafeDocumentEventHandler<RoutedEventArgs>(FilteredSearch_Click, this).Handler;
		_actualQuantitiesCheck = _mtIncomeImportGrid.AddButton("cheked_all_16.svg", ArmSoft.SME.Trade.Common.Resources.ActualQuantitiesCheck, ArmSoft.SME.Trade.Common.Resources.ActualQuantitiesCheck, 225.0);
		_actualQuantitiesCheck.Click += new SafeDocumentEventHandler<RoutedEventArgs>(ActualQuantitiesCheck_Click, this).Handler;
		_mtIncomeImportGrid.Name = "MTIncomeImportGrid";
		_mtIncomeImportGrid.IsRequired = true;
		_mtIncomeImportGrid.AllowMoveUp = true;
		_mtIncomeImportGrid.AllowMoveDown = true;
		_mtIncomeImportTableView = _mtIncomeImportGrid.View;
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
		_mtIncomeImportTableView.FormatConditions.Add(item);
		_mtIncomeImportTableView.FormatConditions.Add(item2);
		_mtIncomeImportGrid.AddRowIndicator(35);
		_codeCol = _mtIncomeImportGrid.AddDropDownColumn(Definition.Grids["MTList"].Columns["MTCode"], new GoodsFilter(shortMode: true, ItemTypesSE.Goods.Value, "", isShowOnlyInPriceList: true), allowAdditionButtons: true);
		_codeCol.Width = 125.0;
		DocumentGridColumn documentGridColumn = _mtIncomeImportGrid.AddStringColumn(Definition.Grids["MTList"].Columns["MTName"]);
		documentGridColumn.AllowEditing = DefaultBoolean.False;
		documentGridColumn.Width = 225.0;
		_unitCodeCol = _mtIncomeImportGrid.AddDropDownColumn(Definition.Grids["MTList"].Columns["UnitCode"], new ArmSoft.SME.Trade.UI.DropDown.MTQntUnits(), allowAdditionButtons: true);
		_mtIncomeImportGrid.AddStringColumn(Definition.Grids["MTList"].Columns["UnitBrief"]).AllowEditing = DefaultBoolean.False;
		_quantityCol = _mtIncomeImportGrid.AddNumberColumn(Definition.Grids["MTList"].Columns["Quantity"], showTotal: true);
		_lastPurchasePriceCol = _mtIncomeImportGrid.AddNumberColumn(Definition.Grids["MTList"].Columns["LastPurchasePrice"]);
		_lastPurchasePriceCol.AllowEditing = DefaultBoolean.False;
		_variancePercentCol = _mtIncomeImportGrid.AddNumberColumn(Definition.Grids["MTList"].Columns["VariancePercent"]);
		_variancePercentCol.AllowEditing = DefaultBoolean.False;
		SetLastPurchasePricesAndVariancePercentColumns();
		_priceCol = _mtIncomeImportGrid.AddNumberColumn(Definition.Grids["MTList"].Columns["Price"]);
		_summaCol = _mtIncomeImportGrid.AddNumberColumn(Definition.Grids["MTList"].Columns["Summa"], showTotal: true);
		_customSummaCol = _mtIncomeImportGrid.AddNumberColumn(Definition.Grids["MTList"].Columns["CustomSumma"], showTotal: true);
		_customPercentCol = _mtIncomeImportGrid.AddNumberColumn(Definition.Grids["MTList"].Columns["CustomPercent"]);
		_customDutyCol = _mtIncomeImportGrid.AddNumberColumn(Definition.Grids["MTList"].Columns["CustomDuty"], showTotal: true);
		_vatCol = _mtIncomeImportGrid.AddBooleanColumn(Definition.Grids["MTList"].Columns["VAT"]);
		_vatCol.Width = 45.0;
		_vatSummaCol = _mtIncomeImportGrid.AddNumberColumn(Definition.Grids["MTList"].Columns["VATSumma"], showTotal: true);
		_useLifeCol = _mtIncomeImportGrid.AddDateColumn(Definition.Grids["MTList"].Columns["UseLife"]);
		_useLifeCol.Visible = Settings.ParametersService.ShowPartyUSEABLEDATE();
		_useLifeCol.ShowInColumnChooser = _useLifeCol.Visible;
		_countryCol = _mtIncomeImportGrid.AddDropDownColumn(Definition.Grids["MTList"].Columns["Country"], new ArmSoft.SME.Trade.UI.DropDown.Countries());
		_countryCol.Visible = Settings.ParametersService.ShowPartyCountry();
		_countryCol.ShowInColumnChooser = _countryCol.Visible;
		_producerCol = _mtIncomeImportGrid.AddDropDownColumn(Definition.Grids["MTList"].Columns["Producer"], new Producer());
		_producerCol.Width = SizeCalculator.WidthForString(5);
		_producerCol.Visible = Settings.ParametersService.ShowPartyProducer();
		_producerCol.ShowInColumnChooser = _producerCol.Visible;
		ArmSoft.SME.Trade.UI.DropDown.PropertyList idv = new ArmSoft.SME.Trade.UI.DropDown.PropertyList();
		_property1Col = _mtIncomeImportGrid.AddDropDownCommentColumn(Definition.Grids["MTList"].Columns["Property1Caption"], idv, 50);
		_property1Col.IsOwnProcessingClickDropDown = true;
		_property1Col.Width = SizeCalculator.WidthForString(25);
		_property1Col.Visible = Settings.ParametersService.ShowPartyProperty1();
		_property1Col.ShowInColumnChooser = _property1Col.Visible;
		_property2Col = _mtIncomeImportGrid.AddDropDownCommentColumn(Definition.Grids["MTList"].Columns["Property2Caption"], idv, 50);
		_property2Col.IsOwnProcessingClickDropDown = true;
		_property2Col.Width = SizeCalculator.WidthForString(25);
		_property2Col.Visible = Settings.ParametersService.ShowPartyProperty2();
		_property2Col.ShowInColumnChooser = _property2Col.Visible;
		_rowDescCol = _mtIncomeImportGrid.AddStringColumn(Definition.Grids["MTList"].Columns["RowDescription"]);
		foreach (GridColumn column in _mtIncomeImportGrid.Grid.Columns)
		{
			column.AllowSorting = DefaultBoolean.True;
			column.AllowColumnFiltering = DefaultBoolean.True;
			column.FilterPopupMode = FilterPopupMode.CheckedList;
		}
		_mtIncomeImportList = new ObservableCollection<MTIncomeImportSpecificationRow>();
		_mtIncomeImportGrid.AddingRow += new SafeDocumentEventHandler<DocumentGridAddingRowEventArgs>(MTIncomeImportGrid_AddingRow, this).Handler;
		_mtIncomeImportGrid.DeletingRow += new SafeDocumentEventHandler<DocumentGridDeletingRowEventArgs>(MTIncomeImportGrid_DeletedRow, this).Handler;
		_mtIncomeImportGrid.DuplicatingRow += new SafeDocumentEventHandler<DocumentGridDuplicatingRowEventArgs>(MTIncomeImportGrid_DuplicatingRow, this).Handler;
		_mtIncomeImportGrid.MovingRowUp += new SafeDocumentEventHandler<DocumentGridMovingRowUpOrDownEventArgs>(MTIncomeImportGrid_MovingRowUp, this).Handler;
		_mtIncomeImportGrid.MovingRowDown += new SafeDocumentEventHandler<DocumentGridMovingRowUpOrDownEventArgs>(MTIncomeImportGrid_MovingRowDown, this).Handler;
		_mtIncomeImportGrid.RefreshButtons += new SafeDocumentEventHandler<EventArgs>(MTIncomeImportGrid_RefreshButtons, this).Handler;
		_mtIncomeImportGrid.ClickDropDown += new SafeDocumentEventHandler<DocumentGridClickDropDownEventArgs>(MTIncomeImportGrid_ClickDropDown, this).Handler;
		_mtIncomeImportGrid.AddingRowWithDialog += new SafeDocumentEventHandler<DocumentGridAddingRowEventArgs>(MTIncomeImportGrid_AddingRowWithDialog, this).Handler;
		_mtIncomeImportGrid.DuplicatingRowWithDialog += new SafeDocumentEventHandler<DocumentGridDuplicatingRowEventArgs>(MTIncomeImportGrid_DuplicatingRowWithDialog, this).Handler;
		_mtIncomeImportGrid.EditingRowWithDialog += new SafeDocumentEventHandler<DocumentGridEditingRowEventArgs>(MTIncomeImportGrid_EditingRowWithDialog, this).Handler;
		_infoDisplayControls = new InfoDisplayControls(this, DocumentTypeSE.MTIncomeImport, isShowItemPictureOnInfoPanel: false, isShowStorageAvailabilityOnInfoPanel: true, isShowWholeAvailabilityOnInfoPanel: true, Settings.ParametersService.ShowGoodsCostInReportsAndDocs() && !_showLastPurchasePrices, showDescriptionOnInfoPanel: true);
		_infoDisplayControls.ConstructUI(container6);
		_infoDisplay = _infoDisplayControls.InfoDisplay;
		_infoDisplay.Collapsed += new SafeDocumentEventHandler<EventArgs>(InfoDisplay_Collapsed, this).Handler;
		_infoItemName = _infoDisplayControls.InfoItemName;
		_supplierContractContainer.DocSummaControl = _totalSumma;
		_supplierContractContainer.DocCurrCodeControl = _currCode;
		_customContractContainer.DocCurrCodeControl = _currCode;
		LayoutGroup container10 = AddTabbedGroup(_tabs, ArmSoft.SME.Common.Resources.Additional, "Additional");
		_trust = AddStringControl(container10, Definition.Fields["Trust"], HorizontalAlignment.Stretch);
		_mediator = AddDropDownControl(container10, Definition.Fields["Mediator"], new ArmSoft.SME.Trade.UI.DropDown.Employees(), allowAdditionButtons: false, fieldWithCaption: true, HorizontalAlignment.Stretch);
		_machine = AddStringControl(container10, Definition.Fields["Machine"], HorizontalAlignment.Stretch);
		_chief = AddStringControl(container10, Definition.Fields["Chief"], HorizontalAlignment.Stretch);
		_allower = AddDropDownCommentControl(container10, Definition.Fields["Allower"], new ArmSoft.SME.Trade.UI.DropDown.Employees(), HorizontalAlignment.Stretch);
		_response = AddDropDownCommentControl(container10, Definition.Fields["Response"], new ArmSoft.SME.Trade.UI.DropDown.Employees(), HorizontalAlignment.Stretch);
		_mtIncomeImportTableView.CellValueChanged += new SafeDocumentEventHandler<CellValueChangedEventArgs>(MTIncomeImportTableView_CellValueChanged, this).Handler;
		_focusedRowHandleChangedEvent = new SafeDocumentEventHandler<FocusedRowHandleChangedEventArgs>(MTIncomeImportTableView_FocusedRowHandleChanged, this);
		_mtIncomeImportTableView.FocusedRowHandleChanged += _focusedRowHandleChangedEvent.Handler;
		_mtIncomeImportTableView.ShowingEditor += new SafeDocumentEventHandler<ShowingEditorEventArgs>(MTIncomeImportTableView_ShowingEditor, this).Handler;
		_infoDisplay.IsCollapsed = _infoDisplayControls.InfoPanelCollapsed;
		RowInfoChange += ChangeRowInfo;
	}

	private void ActualQuantitiesCheck_Click(object sender, RoutedEventArgs e)
	{
		try
		{
			if ((!StoredInDb || IsChanged) && !SaveDocument(isAsDraft: true))
			{
				return;
			}
			ActualQuantitiesCheckWindow actualQuantitiesCheckWindow = new ActualQuantitiesCheckWindow(this, _mtIncomeImportList, _date.EditValue.Value, _docNum.EditValue.ToString());
			if (actualQuantitiesCheckWindow.ShowDialog().Value)
			{
				foreach (ActualQuantitiesCheck.ActualQuantitiesCheckRow item2 in actualQuantitiesCheckWindow.QCResult)
				{
					if (item2.VarianceQuantity == 0m)
					{
						continue;
					}
					if (!item2.UserAdd)
					{
						for (int i = 0; i <= _mtIncomeImportList.Count - 1; i++)
						{
							if (item2.MTID == _mtIncomeImportList[i].MTID && item2.UnitCode == _mtIncomeImportList[i].UnitCode)
							{
								MTIncomeImportSpecificationRow mTIncomeImportSpecificationRow = _mtIncomeImportList[i];
								decimal num = mTIncomeImportSpecificationRow.Quantity + item2.VarianceQuantity;
								TradeSettings.TradeCacheService.Material.GetItem(item2.MTCode);
								MTIncomeImportSpecificationRow mTIncomeImportSpecificationRow2 = (MTIncomeImportSpecificationRow)_mtIncomeImportGrid.Grid.GetRowByListIndex(i);
								if (!(num < 0m))
								{
									mTIncomeImportSpecificationRow.Quantity = item2.ActualQuantity;
									mTIncomeImportSpecificationRow2.Summa = (mTIncomeImportSpecificationRow.Quantity * mTIncomeImportSpecificationRow2.Price).RoundAs(2);
									UpdateCustomSumms(mTIncomeImportSpecificationRow2);
									item2.VarianceQuantity = 0m;
									break;
								}
								mTIncomeImportSpecificationRow.Quantity = 0m;
								mTIncomeImportSpecificationRow2.Summa = (mTIncomeImportSpecificationRow.Quantity * mTIncomeImportSpecificationRow2.Price).RoundAs(2);
								UpdateCustomSumms(mTIncomeImportSpecificationRow2);
								item2.VarianceQuantity = num;
							}
						}
					}
					else
					{
						MaterialCacheRow item = TradeSettings.TradeCacheService.Material.GetItem(item2.MTCode);
						AddNewRowOrMergeInner(item, forFocusedRow: false, item2.UnitCode, item2.UnitBrief, item2.VarianceQuantity);
					}
				}
				IsChanged = true;
			}
			_mtIncomeImportGrid.Grid.RefreshData();
			CalcSummAndSummVAT();
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	private void FilteredSearch_Click(object sender, RoutedEventArgs e)
	{
		try
		{
			FilteredSearch filteredSearch = new FilteredSearch();
			filteredSearch.FilterDataView = new GoodsGroups(showAsTree: true, filteredSearch: true);
			filteredSearch.SearchDataView = new GoodsFilter(shortMode: true, ItemTypesSE.Goods, "", isShowOnlyInPriceList: true);
			if (!string.IsNullOrWhiteSpace(_supplierContractContainer.PartnerCode.Code))
			{
				filteredSearch.FilteredSearchDataView = new GoodsFilter(shortMode: true, ItemTypesSE.Goods, "", isShowOnlyInPriceList: false, "", applyDefaultDataViewDefinition: true, _supplierContractContainer.PartnerCode.Code);
			}
			filteredSearch.ResultsList = new MTIncomeImportFilteredSearchList();
			FilteredSearchWindow filteredSearchWindow = new FilteredSearchWindow(filteredSearch, this, isBasketMode: true);
			if (!filteredSearchWindow.ShowDialog().Value)
			{
				return;
			}
			foreach (MTIncomeImportSpecificationRow item2 in filteredSearchWindow.BasketResult)
			{
				string mTQntUnitByMTIDPriceTypeAndDocDate = TradeSettings.CreateTradeClient().CommonServices.GetMTQntUnitByMTIDPriceTypeAndDocDate(item2.MTID, _priceListType.Code, _date.EditValue.Value);
				if (!string.IsNullOrWhiteSpace(mTQntUnitByMTIDPriceTypeAndDocDate))
				{
					item2.UnitCode = mTQntUnitByMTIDPriceTypeAndDocDate;
				}
				QntUnitCacheRow item = TradeSettings.TradeCacheService.QntUnit.GetItem(item2.UnitCode);
				item2.UnitBrief = ((item == null) ? "" : item.Brief);
				item2.Price = TradeSettings.CreateTradeClient().CommonServices.GetPriceForGivenPriceListAndVATType(_priceListType.Code, "1", item2.MTID, item2.UnitCode, _date.EditValue.Value, _currCode.Code);
				item2.Summa = (item2.Quantity * item2.Price).RoundAs(2);
				UpdateCustomSumms(item2);
				item2.RowID = _maxRowID;
				_mtIncomeImportList.Add(item2);
				_maxRowID++;
			}
			_mtIncomeImportGrid.Grid.RefreshData();
			CalcSummAndSummVAT();
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
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
		_barCode.DoValidate();
		try
		{
			string text = _barCode.Text.Trim();
			if (string.IsNullOrWhiteSpace(text))
			{
				if (!_date.DoValidate() || !_date.EditValue.HasValue)
				{
					ControlsHelper.SetFocus(_date);
					throw new Exception(ArmSoft.SME.Common.Resources.ValidateEmptyDate);
				}
				GoodsFilter goodsFilter = new GoodsFilter(shortMode: true, ItemTypesSE.Goods, "", isShowOnlyInPriceList: true);
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
			if (item == null || item.Type == GoodsTypesSE.Service)
			{
				ControlsHelper.SetFocus(_barCode);
				throw new ASException(string.Format(ArmSoft.SME.Trade.Common.Resources.MTEgineInavidMaterial, text));
			}
			AddMTToList(item, 1m);
			if (_tabs.SelectedIndex != _priceListTabControlIndex)
			{
				_tabs.SelectedContainer = (DXTabItem)_tabs.Items[_priceListTabControlIndex];
			}
			_barCode.Text = "";
			CalcSummAndSummVAT();
			_vatSumma.DoValidate();
			RowInfoChangeEventHandler rowInfoChangeEventHandler = RowInfoChange;
			if (rowInfoChangeEventHandler != null)
			{
				ObservableCollection<MTIncomeImportSpecificationRow> mtIncomeImportList = _mtIncomeImportList;
				rowInfoChangeEventHandler(mtIncomeImportList[mtIncomeImportList.Count - 1]);
			}
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
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
			decimal qty = 1m;
			string unitCode = "";
			string unitBrief = "";
			string barCode = _barCode.Text.Trim();
			MaterialCacheRow materialCacheRow = ArmSoft.SME.Trade.UI.Helpers.DocumentHelpers.TryGetItemByBarCode(ref barCode, out qty, out unitCode, out unitBrief);
			if (materialCacheRow == null || materialCacheRow.Type == ItemTypesSE.Services.Value)
			{
				if (ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Common.Resources.FindeInUnitedProducts, ArmSoft.SME.Common.Resources.ProgramNameTrade, MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No) != MessageBoxResult.Yes)
				{
					return;
				}
				string text = ArmSoft.SME.Trade.UI.Helpers.DocumentHelpers.CreateNewGoodFromUnitedProdAndAddInDoc(barCode, this);
				materialCacheRow = TradeSettings.TradeCacheService.Material.GetItem(text);
				if (!string.IsNullOrWhiteSpace(text))
				{
					AddMTToList(materialCacheRow, qty, unitCode);
					_barCode.Text = "";
					CalcSummAndSummVAT();
					_vatSumma.DoValidate();
					RowInfoChangeEventHandler rowInfoChangeEventHandler = RowInfoChange;
					if (rowInfoChangeEventHandler != null)
					{
						ObservableCollection<MTIncomeImportSpecificationRow> mtIncomeImportList = _mtIncomeImportList;
						rowInfoChangeEventHandler(mtIncomeImportList[mtIncomeImportList.Count - 1]);
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
				_barCode.Text = "";
				CalcSummAndSummVAT();
				_vatSumma.DoValidate();
				RowInfoChange?.Invoke(_mtIncomeImportList[_mtIncomeImportList.Count - 1]);
			}
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	private void AddMTToList(MaterialCacheRow goodRow, decimal quantity, string unitCode = "", string unitBrief = "")
	{
		if (_rowAddMode == "3")
		{
			AddMTToListNewRow(goodRow, string.IsNullOrWhiteSpace(unitCode) ? goodRow.Unit : unitCode, string.IsNullOrWhiteSpace(unitBrief) ? goodRow.UnitRow.Brief : unitBrief, quantity);
		}
		else
		{
			AddNewRowOrMergeInner(goodRow, _rowAddMode == "2", string.IsNullOrWhiteSpace(unitCode) ? goodRow.Unit : unitCode, string.IsNullOrWhiteSpace(unitBrief) ? goodRow.UnitRow.Brief : unitBrief, quantity);
		}
	}

	private void AddNewRowOrMergeInner(MaterialCacheRow goodRow, bool forFocusedRow, string unitCode, string unitBrief, decimal quantity)
	{
		MTIncomeImportSpecificationRow mTIncomeImportSpecificationRow = new MTIncomeImportSpecificationRow();
		if (forFocusedRow && _mtIncomeImportList.Count != 0)
		{
			mTIncomeImportSpecificationRow = (MTIncomeImportSpecificationRow)_mtIncomeImportTableView.Grid.CurrentItem;
			if (mTIncomeImportSpecificationRow == null)
			{
				return;
			}
		}
		for (int i = 0; i <= _mtIncomeImportList.Count - 1; i++)
		{
			if ((!forFocusedRow || mTIncomeImportSpecificationRow.RowID == _mtIncomeImportList[i].RowID) && goodRow.Id == _mtIncomeImportList[i].MTID && unitCode == _mtIncomeImportList[i].UnitCode)
			{
				FillMTIncGridRow(_mtIncomeImportList[i], goodRow, unitCode, unitBrief, quantity);
				int rowHandleByListIndex = _mtIncomeImportGrid.Grid.GetRowHandleByListIndex(i);
				_mtIncomeImportGrid.Grid.RefreshRow(rowHandleByListIndex);
				POWindow<MTIncomeImport>.SelectCellInGrid(_mtIncomeImportTableView, _mtIncomeImportGrid, rowHandleByListIndex, "Quantity");
				return;
			}
		}
		AddMTToListNewRow(goodRow, unitCode, unitBrief, quantity);
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
		//IL_03ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_03cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d3: Unknown result type (might be due to invalid IL or missing references)
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
		_barButtonPriceFormation.ItemClick += new SafeDocumentEventHandler<ItemClickEventArgs>(BarButtonPriceFormation_ItemClick, this).Handler;
		_barManager.Items.Add(_barButtonPriceFormation);
		_ribbonPageGroupActions.ItemLinks.Add(_barButtonPriceFormation.CreateLink());
		barItemLink = _barButtonPriceFormation.CreateLink();
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
		_barButtonDocumentSettings.Content = ArmSoft.SME.Common.Resources.Settings;
		_barButtonDocumentSettings.Glyph = ImageService.GetImage("page_white_gear_16.svg");
		_barButtonDocumentSettings.LargeGlyph = ImageService.GetImage("page_white_gear_32.svg");
		_barButtonDocumentSettings.ItemClick += new SafeDocumentEventHandler<ItemClickEventArgs>(BarButtonDocumentSettings_ItemClick, this).Handler;
		_ribbonPageGroupActions.ItemLinks.Add(_barButtonDocumentSettings.CreateLink());
		_barButtonGoodsOperationsJournal = new ReportBarButtonItem(DataProviderTypeSE.GoodsRepOperations.Value, showDialog: false, ArmSoft.SME.Trade.Common.Resources.MTOperations);
		_barButtonGoodsOperationsJournal.AddInBarSubItem(ref _ribbonPageGroupReports, ref _barSubItemJournals);
		_barButtonPartnerOperationJournal = new ReportBarButtonItem(DataProviderTypeSE.PartOpJournal.Value, showDialog: false, ArmSoft.SME.Trade.Common.Resources.PartnerOp);
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
		ReportBarButtonItem barButtonPartnerOperationJournal = _barButtonPartnerOperationJournal;
		num = 3;
		List<PropertyWithValue> list3 = new List<PropertyWithValue>(num);
		CollectionsMarshal.SetCount(list3, num);
		Span<PropertyWithValue> span3 = CollectionsMarshal.AsSpan(list3);
		span3[0] = new PropertyWithValue("DocISN", PO.ISN.ToString());
		span3[1] = new PropertyWithValue("AdditionalCaption", GetReportsAdditionalCaption());
		span3[2] = new PropertyWithValue("ShowLinks", true.ToString());
		barButtonPartnerOperationJournal.PropertyWithValues = list3;
	}

	private void BarButtonDocumentSettings_ItemClick(object sender, ItemClickEventArgs e)
	{
		DialogWindow dialogWindow = new DialogWindow();
		bool showLastPurchasePrices = _showLastPurchasePrices;
		dialogWindow.Caption = ArmSoft.SME.Common.Resources.Settings;
		dialogWindow.Owner = this;
		if (Settings.ParametersService.ShowGoodsCostInReportsAndDocs())
		{
			dialogWindow.AddBooleanControl(null, "bShowLastPurchasePrices", ArmSoft.SME.Trade.Common.Resources.ShowLastPurchasePrices).IsChecked = _showLastPurchasePrices;
		}
		if (Convert.ToBoolean(dialogWindow.ShowDialog()) && Settings.ParametersService.ShowGoodsCostInReportsAndDocs())
		{
			_showLastPurchasePrices = (bool)dialogWindow["bShowLastPurchasePrices"];
			Settings.ParametersService.ShowLastPurchasePrices_set(_showLastPurchasePrices, Settings.LoginService.UserId);
		}
		if (showLastPurchasePrices != _showLastPurchasePrices)
		{
			SetLastPurchasePricesAndVariancePercentColumns();
			_infoDisplayControls.isShowPriceAvailabilityOnInfoPanel = !_showLastPurchasePrices;
			if (_mtIncomeImportList.Count != 0)
			{
				_infoDisplayControls.FillAllInfo(!_showLastPurchasePrices);
				SetAvailabilityInfoToPanel((MTIncomeImportSpecificationRow)_mtIncomeImportGrid.Grid.CurrentItem);
			}
			if (!_showLastPurchasePrices && _mtIncomeImportList.Count != 0)
			{
				SetLastPurchaseAndVarianceInfoPanel((MTIncomeImportSpecificationRow)_mtIncomeImportGrid.Grid.CurrentItem);
			}
		}
		_mtIncomeImportGrid.Grid.RefreshData();
	}

	protected void BarButtonPrintLabel_ItemClick(object sender, ItemClickEventArgs e)
	{
		if (_mtIncomeImportList.Count > 0)
		{
			FillDocument(new StoreRequest());
			new PrintLabelDialog(this, PO).Invoke();
		}
	}

	private void BarButtonPriceFormation_ItemClick(object sender, ItemClickEventArgs e)
	{
		if (_mtIncomeImportList.Count <= 0)
		{
			return;
		}
		List<PricesFormationWindow.GoodsRow> list = new List<PricesFormationWindow.GoodsRow>();
		List<SelectedMaterialsRow> list2 = new List<SelectedMaterialsRow>();
		foreach (MTIncomeImportSpecificationRow mtIncomeImport in _mtIncomeImportList)
		{
			MaterialCacheRow item = TradeSettings.TradeCacheService.Material.GetItem(mtIncomeImport.MTCode);
			list.Add(new PricesFormationWindow.GoodsRow(mtIncomeImport.MTID, mtIncomeImport.MTCode, mtIncomeImport.UnitCode, item.AltUnit));
			SelectedMaterialsRow selectedMaterialsRow = new SelectedMaterialsRow
			{
				RowNum = mtIncomeImport.RowNum,
				MTID = mtIncomeImport.MTID,
				MTCode = mtIncomeImport.MTCode,
				MTName = mtIncomeImport.MTName
			};
			if (mtIncomeImport.UnitCode != item.Unit)
			{
				MaterialCacheRow.QntUnitRow qntUnit = TradeSettings.TradeCacheService.Material.GetQntUnit(mtIncomeImport.MTID, mtIncomeImport.UnitCode);
				MaterialCacheRow.QntUnitRow qntUnit2 = TradeSettings.TradeCacheService.Material.GetQntUnit(mtIncomeImport.MTID, item.Unit);
				if (qntUnit != null && qntUnit2 != null)
				{
					selectedMaterialsRow.QntUnit = qntUnit2.QntUnit;
					selectedMaterialsRow.QntUnitName = TradeSettings.TradeCacheService.QntUnit.GetItem(qntUnit2.QntUnit).Brief;
					if (qntUnit.Coef != 0m)
					{
						selectedMaterialsRow.PurchasePrice = (mtIncomeImport.Price * qntUnit2.Coef / qntUnit.Coef).RoundAs(2);
						selectedMaterialsRow.SalePrice = selectedMaterialsRow.PurchasePrice;
						selectedMaterialsRow.DiscountPrice = selectedMaterialsRow.PurchasePrice;
					}
				}
			}
			else
			{
				selectedMaterialsRow.QntUnit = mtIncomeImport.UnitCode;
				selectedMaterialsRow.QntUnitName = mtIncomeImport.UnitBrief;
				selectedMaterialsRow.PurchasePrice = mtIncomeImport.Price;
				selectedMaterialsRow.SalePrice = mtIncomeImport.Price;
				selectedMaterialsRow.DiscountPrice = mtIncomeImport.Price;
			}
			list2.Add(selectedMaterialsRow);
		}
		new PricesFormationWindow(list, "PricesFormation" + Name, showPurchasePriceRow: true, showSalePriceRow: false, showRetailAvailabilityPriceRow: false, _currCode.Code, list2, this, showDiscountPriceRow: true).Invoke();
	}

	private void BarButtonImportFromFile_ItemClick(object sender, ItemClickEventArgs e)
	{
		if (!_date.IsReadOnly && _date.DoValidate() && (_mtIncomeImportList.Count <= 0 || ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Trade.Common.Resources.LoadMaterialsList, ArmSoft.SME.Trade.Common.Resources.ProgramName, MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No) != MessageBoxResult.No))
		{
			_mtIncomeImportList.Clear();
			_maxRowID = 0;
			TerminalHelper.TerminalImporter terminalImporter = new TerminalHelper.TerminalImporter(this);
			if (terminalImporter.ImportFromFile((decimal quantity, MaterialCacheRow goodRow, MaterialCacheRow.BarCodeRow goodBarcodeRow) =>
			{
				AddMTToListNewRow(goodRow, goodBarcodeRow.QntUnit, TradeSettings.TradeCacheService.QntUnit.GetItem(goodBarcodeRow.QntUnit).Brief, quantity);
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
		List<DocumentExchangeRow> list = new List<DocumentExchangeRow>
		{
			new DocumentExchangeRow
			{
				DocType = PO.POType,
				ISN = PO.ISN
			}
		};
		foreach (Guid basedOvExpenseISN in TradeSettings.CreateTradeClient().CommonServices.GetBasedOvExpenseISNList(PO.ISN))
		{
			list.Add(new DocumentExchangeRow
			{
				DocType = DocumentTypeSE.MTOverheadExp,
				ISN = basedOvExpenseISN
			});
		}
		fileStream.Write(Settings.CreateApiClient().Exchange.ExportDocument(list));
	}

	protected override void CreateChildDocument(short childDocType, MTIncomeImport baseDoc)
	{
		base.CreateChildDocument(childDocType, baseDoc);
		if (childDocType == 10 || (uint)(childDocType - 13) <= 1u)
		{
			CreateBasedOnDocHelper.CreateDocument((DocumentTypeSE)(SmartEnum<DocumentTypeSE, short>)childDocType, PO.ISN, DVPID);
		}
	}

	private void AddMTToListNewRow(MaterialCacheRow goodsRow, string unitCode, string unitBrief, decimal quantity)
	{
		MTIncomeImportSpecificationRow mTIncomeImportSpecificationRow = new MTIncomeImportSpecificationRow();
		FillMTIncGridRow(mTIncomeImportSpecificationRow, goodsRow, unitCode, unitBrief, quantity);
		mTIncomeImportSpecificationRow.RowID = _maxRowID;
		_mtIncomeImportList.Add(mTIncomeImportSpecificationRow);
		_maxRowID++;
		int rowHandleByListIndex = _mtIncomeImportGrid.Grid.GetRowHandleByListIndex(_mtIncomeImportList.Count - 1);
		_mtIncomeImportGrid.Grid.RefreshRow(rowHandleByListIndex);
		POWindow<MTIncomeImport>.SelectCellInGrid(_mtIncomeImportTableView, _mtIncomeImportGrid, rowHandleByListIndex, "Quantity");
		_mtIncomeImportGrid.ConfigureButtons();
	}

	private void FillMTIncGridRow(MTIncomeImportSpecificationRow mtIncGridRow, MaterialCacheRow goodsRow, string unitCode, string unitBrief, decimal quantity = 1m)
	{
		mtIncGridRow.UnitCode = unitCode;
		mtIncGridRow.UnitBrief = unitBrief;
		if (goodsRow != null)
		{
			mtIncGridRow.MTCode = goodsRow.MTCode;
			mtIncGridRow.MTID = goodsRow.Id;
			mtIncGridRow.MTName = goodsRow.Caption;
			mtIncGridRow.VAT = goodsRow.VAT;
			mtIncGridRow.Price = TradeSettings.CreateTradeClient().CommonServices.GetPriceForGivenPriceListAndVATType(_priceListType.Code, "1", mtIncGridRow.MTID, mtIncGridRow.UnitCode, _date.EditValue.Value, _currCode.Code);
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
		mtIncGridRow.Summa = (mtIncGridRow.Quantity * mtIncGridRow.Price).RoundAs(2);
		UpdateCustomSumms(mtIncGridRow);
	}

	public void SetLastPurchasePricesAndVariancePercentColumns()
	{
		_lastPurchasePriceCol.Visible = Settings.ParametersService.ShowGoodsCostInReportsAndDocs() && _showLastPurchasePrices;
		_variancePercentCol.Visible = Settings.ParametersService.ShowGoodsCostInReportsAndDocs() && _showLastPurchasePrices;
		_lastPurchasePriceCol.ShowInColumnChooser = _lastPurchasePriceCol.Visible;
		_variancePercentCol.ShowInColumnChooser = _variancePercentCol.Visible;
		if (_mtIncomeImportList != null && Settings.ParametersService.ShowGoodsCostInReportsAndDocs() && _showLastPurchasePrices)
		{
			SetLastPurchaseInfoToColumn();
		}
	}

	protected override void StoreSettings(RegistryKey rk)
	{
		base.StoreSettings(rk);
		rk.SetValue("InfoPanelCollapsed", _infoDisplayControls.InfoPanelCollapsed ? "1" : "0");
		_mtIncomeImportGrid.StoreSettings(rk);
	}

	protected override void RestoreSettings(double defaultWidth, double defaultHeight, RegistryKey rk)
	{
		base.RestoreSettings(defaultWidth, defaultHeight, rk);
		if (rk != null)
		{
			_infoDisplayControls.InfoPanelCollapsed = rk.GetValue("InfoPanelCollapsed", false).ToString() == "1";
			_mtIncomeImportGrid.RestoreSettings(rk);
		}
	}

	private void ChangeRowInfo(MTIncomeImportSpecificationRow row = null)
	{
		if (row == null)
		{
			((DispatcherObject)this).Dispatcher.BeginInvoke((Delegate)new DelegateClearInfoPanel(CleanInfoPanel), Array.Empty<object>());
			return;
		}
		((DispatcherObject)this).Dispatcher.BeginInvoke((Delegate)new DelegateSetRowInfoToInfoPanel(SetRowInfoToPanel), new object[1] { row });
	}

	private void SetRowInfoToPanel(MTIncomeImportSpecificationRow row)
	{
		try
		{
			if (string.IsNullOrWhiteSpace(row.MTName))
			{
				CleanInfoPanel();
				return;
			}
			_isChanged = IsChanged;
			(row.Quantity * row.Price).RoundAs(2);
			_infoItemName.Text = $"{row.MTCode} - {row.MTName}";
			SetAvailabilityInfoToPanel(row);
			if (Settings.ParametersService.ShowGoodsCostInReportsAndDocs())
			{
				if (!_showLastPurchasePrices)
				{
					SetLastPurchaseAndVarianceInfoPanel(row);
				}
				else
				{
					SetLastPurchaseInfoToColumn(row);
				}
			}
			IsChanged = _isChanged;
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	private void SetAvailabilityInfoToPanel(MTIncomeImportSpecificationRow row)
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

	private void SetLastPurchaseAndVarianceInfoPanel(MTIncomeImportSpecificationRow row)
	{
		decimal num = 0m;
		int partId = -1;
		decimal number = 0m;
		if (!string.IsNullOrWhiteSpace(_supplierContractContainer.PartnerCode.Code))
		{
			PartnerCacheRow item = TradeSettings.TradeCacheService.Partner.GetItem(_supplierContractContainer.PartnerCode.Code);
			if (item != null)
			{
				partId = item.Id;
			}
		}
		MTPriceForUnit mTIncomeImportPrice = TradeSettings.CreateTradeClient().CommonServices.GetMTIncomeImportPrice(row.MTID, row.UnitCode, PO.ISN, _currCode.Code, partId, _date.EditValue.Value);
		if (row.MTID != -1 && _date.EditValue.HasValue && _date.EditValue.HasValue && mTIncomeImportPrice != null)
		{
			row.UnitBrief = TradeSettings.TradeCacheService.QntUnit.GetItem(row.UnitCode).Brief;
			number = number.RoundAs(2);
			(number * row.Quantity).RoundAs(2);
			if (row.Price != 0m)
			{
				num = ((!(number != 0m)) ? 0m : ((row.Price - number) / number * 100m).RoundAs(2));
				if (num / 1000m >= 1m)
				{
					num = 999.99m;
				}
				else if (num / 1000m <= -1m)
				{
					num = -99.99m;
				}
			}
			_infoDisplayControls.SetPrice(number);
			_infoDisplayControls.SetVariance(num);
		}
		else
		{
			_infoDisplayControls.SetStringPrice(ArmSoft.SME.Common.Resources.NothingWasFound);
			_infoDisplayControls.SetVariance(num);
		}
	}

	private void SetLastPurchaseInfoToColumn(MTIncomeImportSpecificationRow row = null)
	{
		int partId = -1;
		decimal lastPurchasePrice = 0m;
		if (!string.IsNullOrWhiteSpace(_supplierContractContainer.PartnerCode.Code))
		{
			PartnerCacheRow item = TradeSettings.TradeCacheService.Partner.GetItem(_supplierContractContainer.PartnerCode.Code);
			if (item != null)
			{
				partId = item.Id;
			}
		}
		if (row == null)
		{
			foreach (MTIncomeImportSpecificationRow mtIncomeImport in _mtIncomeImportList)
			{
				MTPriceForUnit mTIncomeImportPrice = TradeSettings.CreateTradeClient().CommonServices.GetMTIncomeImportPrice(mtIncomeImport.MTID, mtIncomeImport.UnitCode, PO.ISN, _currCode.Code, partId, _date.EditValue.Value);
				if (mtIncomeImport.MTID != -1 && _date.EditValue.HasValue && _date.EditValue.HasValue && mTIncomeImportPrice != null)
				{
					mtIncomeImport.LastPurchasePrice = lastPurchasePrice;
				}
				else
				{
					mtIncomeImport.LastPurchasePrice = 0m;
				}
			}
			SetVariancePercent();
		}
		else
		{
			MTPriceForUnit mTIncomeImportPrice2 = TradeSettings.CreateTradeClient().CommonServices.GetMTIncomeImportPrice(row.MTID, row.UnitCode, PO.ISN, _currCode.Code, partId, _date.EditValue.Value);
			if (row.MTID != -1 && _date.EditValue.HasValue && _date.EditValue.HasValue && mTIncomeImportPrice2 != null)
			{
				row.LastPurchasePrice = mTIncomeImportPrice2.Price;
			}
			else
			{
				row.LastPurchasePrice = 0m;
			}
			SetVariancePercent(row);
		}
		_mtIncomeImportGrid.Grid.RefreshData();
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

	protected override void OnNew()
	{
		base.OnNew();
		_ = PO.MTList.Count;
		_date.EditValue = Settings.HostingEnvironment.CurrentDate;
		_owner.Code = Settings.LoginService.UserName;
		if (_viewMode == WindowViewMode.Copy)
		{
			return;
		}
		string text = Settings.ParametersService.DefaultStorage();
		if (!string.IsNullOrWhiteSpace(text))
		{
			StoragesCacheRow item = TradeSettings.TradeCacheService.Storage.GetItem(text);
			if ((item == null || !item.Retail) && TradeSettings.CreateTradeClient().CommonServices.IsStorageAvailable(text))
			{
				_storage.Code = text;
			}
		}
		_priceListType.Code = Settings.ParametersService.DefaultBuyingPrice();
		string text2 = Settings.ParametersService.DefaultSupplier();
		if (!string.IsNullOrWhiteSpace(text2))
		{
			PartnerCacheRow item2 = TradeSettings.TradeCacheService.Partner.GetItem(text2);
			if (item2 != null && item2.Supplier)
			{
				_supplierContractContainer.PartnerCode.Code = text2;
			}
		}
		_chief.EditValue = Settings.ParametersService.ChiefAccountantName();
	}

	protected override void OnNewAfterSave()
	{
		base.OnNewAfterSave();
		_mtIncomeImportList.Clear();
		_maxRowID = 0;
		CalcSummAndSummVAT();
		_owner.Code = Settings.LoginService.UserName;
	}

	protected override void OnEdit()
	{
		base.OnEdit();
		_setFocusOnActivation = _docNum;
		_supplierContractContainer.CalculatePartDebtRem();
		_customContractContainer.CalculatePartDebtRem();
	}

	protected override void LoadDataToControls()
	{
		base.LoadDataToControls();
		_supplierContractContainer.ValueChangedForLoadDoc();
		_customContractContainer.ValueChangedForLoadDoc();
		CalculatePartSumma();
		CalculateCustomPartSumma();
		_mtIncomeImportList.Clear();
		_focusedRowHandleChangedEvent.HandleOnLoadingDocumentToControls = true;
		foreach (MTIncomeImport.MTIncomeImportSpecificationRow mT in PO.MTList)
		{
			MTIncomeImportSpecificationRow item = new MTIncomeImportSpecificationRow
			{
				MTCode = mT.MTCode,
				MTID = mT.MTID,
				RowNum = mT.RowNum,
				RowID = mT.RowID,
				UnitCode = mT.UnitCode,
				Quantity = mT.Quantity,
				LastPurchasePrice = mT.LastPurchasePrice,
				VariancePercent = mT.VariancePercent,
				Price = mT.Price,
				Summa = mT.Summa,
				CustomSumma = mT.CustomSumma,
				CustomPercent = mT.CustomPercent,
				CustomDuty = mT.CustomDuty,
				VATCodeForConvert = mT.VATCodeForConvert,
				VAT = mT.VAT,
				VATSumma = mT.VATSumma,
				UseLife = mT.UseLife,
				Country = mT.Country,
				Producer = mT.Producer,
				Property1 = mT.Property1,
				Property2 = mT.Property2,
				MTName = mT.MTName,
				FullName = mT.FullName,
				UnitBrief = mT.UnitBrief,
				ProducerCaption = mT.ProducerCaption,
				Property1Caption = mT.Property1Caption,
				Property2Caption = mT.Property2Caption,
				RowDescription = mT.RowDescription
			};
			_mtIncomeImportList.Add(item);
		}
		_maxRowID = PO.MaxRowID;
		_mtIncomeImportGrid.ItemsSource = _mtIncomeImportList;
		_mtIncomeImportGrid.Grid.RefreshData();
		RowInfoChange?.Invoke((MTIncomeImportSpecificationRow)_mtIncomeImportTableView.Grid.CurrentItem);
		SetTotalSumCaption();
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

	protected override void SetReportBarButtonsEnable(bool isEnabled)
	{
		base.SetReportBarButtonsEnable(isEnabled);
		_barButtonJumpTo.IsEnabled = isEnabled;
		_barButtonGoodsOperationsJournal.IsEnabled = isEnabled;
		_barButtonPartnerOperationJournal.IsEnabled = isEnabled;
	}

	protected override void SetControlsReadonly()
	{
		base.SetControlsReadonly();
		if (_viewMode != WindowViewMode.View)
		{
			_totalSumma.IsReadOnly = true;
			_customDuty.IsReadOnly = true;
			_vatSumma.IsReadOnly = true;
			bool flag = State == DocumentStateSE.Draft.Value || State == DocumentStateSE.Deleted.Value;
			bool flag2 = _viewMode == WindowViewMode.Edit && !flag;
			_date.IsReadOnly = flag2;
			_customFee.IsReadOnly = flag2;
			_storage.IsReadOnly = flag2;
			_barCode.IsReadOnly = flag2;
			_codeEnter.IsEnabled = !flag2;
			_currCode.IsReadOnly = flag2;
			_currExChngRate.IsReadOnly = (_currCode.Code == "AMD") | flag2;
			_priceListType.IsReadOnly = flag2;
			_mtIncomeImportGrid.IsReadOnly = flag2;
		}
	}

	protected override void FillDocument(StoreRequest sr)
	{
		base.FillDocument(sr);
		PO.MTList.Clear();
		foreach (MTIncomeImportSpecificationRow mtIncomeImport in _mtIncomeImportList)
		{
			MTIncomeImport.MTIncomeImportSpecificationRow mTIncomeImportSpecificationRow = new MTIncomeImport.MTIncomeImportSpecificationRow();
			mTIncomeImportSpecificationRow.MTCode = mtIncomeImport.MTCode;
			mTIncomeImportSpecificationRow.MTID = mtIncomeImport.MTID;
			mTIncomeImportSpecificationRow.UnitCode = mtIncomeImport.UnitCode;
			mTIncomeImportSpecificationRow.Quantity = mtIncomeImport.Quantity;
			mTIncomeImportSpecificationRow.Price = mtIncomeImport.Price;
			mTIncomeImportSpecificationRow.LastPurchasePrice = mtIncomeImport.LastPurchasePrice;
			mTIncomeImportSpecificationRow.VariancePercent = mtIncomeImport.VariancePercent;
			mTIncomeImportSpecificationRow.Summa = mtIncomeImport.Summa;
			mTIncomeImportSpecificationRow.VATCodeForConvert = mtIncomeImport.VATCodeForConvert;
			mTIncomeImportSpecificationRow.VAT = mtIncomeImport.VAT;
			mTIncomeImportSpecificationRow.VATSumma = mtIncomeImport.VATSumma;
			mTIncomeImportSpecificationRow.RowID = mtIncomeImport.RowID;
			mTIncomeImportSpecificationRow.RowNum = mtIncomeImport.RowNum;
			mTIncomeImportSpecificationRow.CustomSumma = mtIncomeImport.CustomSumma;
			mTIncomeImportSpecificationRow.CustomPercent = mtIncomeImport.CustomPercent;
			mTIncomeImportSpecificationRow.CustomDuty = mtIncomeImport.CustomDuty;
			mTIncomeImportSpecificationRow.UseLife = mtIncomeImport.UseLife;
			mTIncomeImportSpecificationRow.Country = mtIncomeImport.Country;
			mTIncomeImportSpecificationRow.Producer = mtIncomeImport.Producer;
			mTIncomeImportSpecificationRow.Property1 = mtIncomeImport.Property1;
			mTIncomeImportSpecificationRow.Property2 = mtIncomeImport.Property2;
			mTIncomeImportSpecificationRow.ProducerCaption = mtIncomeImport.ProducerCaption;
			mTIncomeImportSpecificationRow.Property1Caption = mtIncomeImport.Property1Caption;
			mTIncomeImportSpecificationRow.Property2Caption = mtIncomeImport.Property2Caption;
			mTIncomeImportSpecificationRow.RowDescription = mtIncomeImport.RowDescription;
			mTIncomeImportSpecificationRow.MTName = mtIncomeImport.MTName;
			mTIncomeImportSpecificationRow.FullName = mtIncomeImport.FullName;
			mTIncomeImportSpecificationRow.UnitBrief = mtIncomeImport.UnitBrief;
			PO.MTList.Add(mTIncomeImportSpecificationRow);
		}
		PO.MaxRowID = _maxRowID;
	}

	private void PriceListType_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		bool flag = false;
		if (_isAskQuestionConfirmSummChange && !string.IsNullOrWhiteSpace(_priceListType.Code) && _priceListType.DoValidate())
		{
			flag = true;
		}
		if (!flag)
		{
			return;
		}
		UpdatePrices();
		UpdateCustomSumms();
		if (Settings.ParametersService.ShowGoodsCostInReportsAndDocs())
		{
			if (_showLastPurchasePrices)
			{
				SetVariancePercent();
			}
			else if (_mtIncomeImportGrid.Grid.CurrentItem != null)
			{
				SetLastPurchaseAndVarianceInfoPanel((MTIncomeImportSpecificationRow)_mtIncomeImportGrid.Grid.CurrentItem);
			}
		}
		_mtIncomeImportGrid.Grid.RefreshData();
	}

	private void CustomFee_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		CalculateCustomPartSumma();
	}

	private void SetTotalSumCaption()
	{
		string text = _currCode.Code;
		if (string.IsNullOrWhiteSpace(_currCode.Code) || !_currCode.DoValidate())
		{
			text = "AMD";
		}
		Set_ControlCaption(_totalSumma, ArmSoft.SME.Common.Resources.Summa + " (" + text + ")");
	}

	private void CurrCode_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		SetTotalSumCaption();
		bool flag = false;
		if (_isAskQuestionConfirmSummChange && !string.IsNullOrWhiteSpace(_currCode.Code) && _currCode.DoValidate())
		{
			flag = true;
		}
		_isAskQuestionConfirmSummChange = false;
		ChangedCurrCodeOrDate();
		_isAskQuestionConfirmSummChange = true;
		if (flag)
		{
			UpdatePricesForCurr();
		}
		if (!string.IsNullOrWhiteSpace(_currCode.Code) && _currCode.DoValidate())
		{
			_oldDocCurrCode = _currCode.Code;
			_oldDocCurrExChngRate = _currExChngRate.Rate;
			_oldDocCurrExChngBase = _currExChngRate.Base;
		}
		if (Settings.ParametersService.ShowGoodsCostInReportsAndDocs() && _showLastPurchasePrices)
		{
			SetLastPurchaseInfoToColumn();
		}
		else if (!_showLastPurchasePrices && _mtIncomeImportGrid.Grid.CurrentItem != null)
		{
			SetLastPurchaseAndVarianceInfoPanel((MTIncomeImportSpecificationRow)_mtIncomeImportGrid.Grid.CurrentItem);
		}
		_mtIncomeImportGrid.Grid.RefreshData();
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
			bool flag = State == DocumentStateSE.Draft.Value || State == DocumentStateSE.Deleted.Value;
			_currExChngRate.IsReadOnly = StoredInDb && !flag;
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

	private void UpdatePricesForCurr()
	{
		foreach (MTIncomeImportSpecificationRow mtIncomeImport in _mtIncomeImportList)
		{
			if (_oldDocCurrCode == "AMD")
			{
				if (_currExChngRate.Rate != 0m)
				{
					mtIncomeImport.Price = (mtIncomeImport.Price / _currExChngRate.Rate * _currExChngRate.Base).RoundAs(2);
				}
			}
			else if (!(_currExChngRate.Rate == 0m) && !(_oldDocCurrExChngBase == 0m))
			{
				mtIncomeImport.Price = (mtIncomeImport.Price * _oldDocCurrExChngRate / _oldDocCurrExChngBase / _currExChngRate.Rate * _currExChngRate.Base).RoundAs(2);
			}
			mtIncomeImport.Summa = (mtIncomeImport.Price * mtIncomeImport.Quantity).RoundAs(2);
			UpdateCustomSumms(mtIncomeImport);
		}
		_mtIncomeImportGrid.Grid.RefreshData();
		CalcSummAndSummVAT();
	}

	private void UpdateCustomSumms(MTIncomeImportSpecificationRow MTIncGridRow)
	{
		decimal num = MTIncGridRow.Summa;
		if (_currExChngRate.Base != 0m)
		{
			num = (num * _currExChngRate.Rate / _currExChngRate.Base).RoundAs(2);
		}
		MTIncGridRow.CustomSumma = num;
		UpdateCustomDuty(MTIncGridRow);
	}

	private void UpdateCustomSumms()
	{
		foreach (MTIncomeImportSpecificationRow mtIncomeImport in _mtIncomeImportList)
		{
			UpdateCustomSumms(mtIncomeImport);
		}
		_mtIncomeImportGrid.Grid.RefreshData();
		CalcSummAndSummVAT();
	}

	private void CalcSummAndSummVAT()
	{
		decimal num = 0m;
		decimal num2 = 0m;
		decimal num3 = 0m;
		foreach (MTIncomeImportSpecificationRow mtIncomeImport in _mtIncomeImportList)
		{
			num += mtIncomeImport.Summa;
			num2 += mtIncomeImport.CustomDuty;
			num3 += mtIncomeImport.VATSumma;
		}
		_totalSumma.EditValue = num;
		_customDuty.EditValue = num2;
		_vatSumma.EditValue = num3;
		CalculatePartSumma();
		CalculateCustomPartSumma();
	}

	private void UpdateCustomDuty(MTIncomeImportSpecificationRow MTIncGridRow)
	{
		MTIncGridRow.CustomDuty = (MTIncGridRow.CustomSumma * MTIncGridRow.CustomPercent / 100m).RoundAs(2);
		UpdateVATSumma(MTIncGridRow);
	}

	private void UpdateVATSumma(MTIncomeImportSpecificationRow MTIncGridRow)
	{
		decimal num = (MTIncGridRow.VAT ? _mTradeConstVAT : 0m);
		MTIncGridRow.VATSumma = ((MTIncGridRow.CustomSumma + MTIncGridRow.CustomDuty) * num / 100m).RoundAs(2);
	}

	private void UpdatePrices()
	{
		decimal num = 1m;
		decimal num2 = 1m;
		if (_mtIncomeImportList.Count == 0)
		{
			return;
		}
		ArmSoft.SME.Trade.UI.Helpers.PriceHelper.GetPriceListRowVATType(_priceListType.Code, out var _, out var priceListRowCurr);
		if (priceListRowCurr != "AMD")
		{
			ExchangeRate exchangeRates = TradeSettings.CreateTradeClient().CommonServices.GetExchangeRates(priceListRowCurr, _date.EditValue.Value);
			num = exchangeRates.Rate;
			num2 = exchangeRates.Base;
		}
		foreach (MTIncomeImportSpecificationRow mtIncomeImport in _mtIncomeImportList)
		{
			decimal priceForGivenPriceListAndVATType = TradeSettings.CreateTradeClient().CommonServices.GetPriceForGivenPriceListAndVATType(_priceListType.Code, "1", mtIncomeImport.MTID, mtIncomeImport.UnitCode, _date.EditValue.Value, _currCode.Code);
			if (priceListRowCurr == _currCode.Code)
			{
				mtIncomeImport.Price = priceForGivenPriceListAndVATType;
			}
			else if (priceListRowCurr == "AMD")
			{
				mtIncomeImport.Price = (priceForGivenPriceListAndVATType / _currExChngRate.Rate * _currExChngRate.Base).RoundAs(2);
			}
			else
			{
				mtIncomeImport.Price = (priceForGivenPriceListAndVATType * num / num2 / _currExChngRate.Rate * _currExChngRate.Base).RoundAs(2);
			}
			mtIncomeImport.Summa = (mtIncomeImport.Price * mtIncomeImport.Quantity).RoundAs(2);
		}
	}

	private void Storage_EditValueChanged(object sender, EditValueChangedEventArgs e)
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
		RowInfoChange?.Invoke((MTIncomeImportSpecificationRow)_mtIncomeImportTableView.Grid.CurrentItem);
	}

	private void MTIncomeImportGrid_DeletedRow(object sender, DocumentGridDeletingRowEventArgs e)
	{
		CalcSummAndSummVAT();
		_mtIncomeImportGrid.Grid.RefreshData();
		RowInfoChange?.Invoke((MTIncomeImportSpecificationRow)_mtIncomeImportTableView.Grid.CurrentItem);
	}

	private void MTIncomeImportGrid_AddingRow(object sender, DocumentGridAddingRowEventArgs e)
	{
		_mtIncomeImportList.Add(new MTIncomeImportSpecificationRow
		{
			RowID = _maxRowID
		});
		_maxRowID++;
		e.RowNumber = _mtIncomeImportList.Count - 1;
		RowInfoChange?.Invoke(_mtIncomeImportList[_mtIncomeImportList.Count - 1]);
	}

	private void ViewAvailAndTurn_Click(object sender, RoutedEventArgs e)
	{
		try
		{
			MTIncomeImportSpecificationRow mTIncomeImportSpecificationRow = (MTIncomeImportSpecificationRow)_mtIncomeImportTableView.Grid.CurrentItem;
			if (mTIncomeImportSpecificationRow != null)
			{
				new MTAvailAndTurnWindow(DialogWindowMode.ViewMode, this, _storage.Code, _supplierContractContainer.PartnerCode.Code, mTIncomeImportSpecificationRow.MTID, mTIncomeImportSpecificationRow.MTCode, mTIncomeImportSpecificationRow.UnitCode).Invoke();
			}
			else
			{
				new MTAvailAndTurnWindow(DialogWindowMode.ViewMode, this, _storage.Code, _supplierContractContainer.PartnerCode.Code, -1, "", "").Invoke();
			}
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	private void MTIncomeImportGrid_DuplicatingRow(object sender, DocumentGridDuplicatingRowEventArgs e)
	{
		MTIncomeImportSpecificationRow mTIncomeImportSpecificationRow = (MTIncomeImportSpecificationRow)_mtIncomeImportGrid.Grid.GetRow(e.RowHandle);
		if (mTIncomeImportSpecificationRow != null)
		{
			MTIncomeImportSpecificationRow mTIncomeImportSpecificationRow2 = new MTIncomeImportSpecificationRow();
			mTIncomeImportSpecificationRow.Clone(mTIncomeImportSpecificationRow2);
			mTIncomeImportSpecificationRow2.RowID = _maxRowID;
			_mtIncomeImportList.Add(mTIncomeImportSpecificationRow2);
			_maxRowID++;
			e.NewRowNumber = _mtIncomeImportList.Count - 1;
			_mtIncomeImportGrid.Grid.RefreshData();
			CalcSummAndSummVAT();
			ChangeRowInfo(mTIncomeImportSpecificationRow2);
			if (Settings.ParametersService.ShowGoodsCostInReportsAndDocs() && _showLastPurchasePrices)
			{
				SetLastPurchaseInfoToColumn(mTIncomeImportSpecificationRow2);
			}
		}
	}

	private void MTIncomeImportGrid_AddingRowWithDialog(object sender, DocumentGridAddingRowEventArgs e)
	{
		MTIncomeImportRowAddEditDialog mTIncomeImportRowAddEditDialog = new MTIncomeImportRowAddEditDialog(DialogWindowMode.NewModeWithNext, this);
		mTIncomeImportRowAddEditDialog.Invoke();
		int addedRows = mTIncomeImportRowAddEditDialog.AddedRows;
		if (addedRows != 0)
		{
			_maxRowID += addedRows;
			_mtIncomeImportGrid.Grid.RefreshData();
			CalcSummAndSummVAT();
			RowInfoChangeEventHandler rowInfoChangeEventHandler = RowInfoChange;
			if (rowInfoChangeEventHandler != null)
			{
				ObservableCollection<MTIncomeImportSpecificationRow> mtIncomeImportList = _mtIncomeImportList;
				rowInfoChangeEventHandler(mtIncomeImportList[mtIncomeImportList.Count - 1]);
			}
		}
	}

	private void MTIncomeImportGrid_DuplicatingRowWithDialog(object sender, DocumentGridDuplicatingRowEventArgs e)
	{
		MTIncomeImportSpecificationRow mTIncomeImportSpecificationRow = (MTIncomeImportSpecificationRow)_mtIncomeImportGrid.Grid.GetRow(e.RowHandle);
		if (mTIncomeImportSpecificationRow == null)
		{
			return;
		}
		MTIncomeImportRowAddEditDialog mTIncomeImportRowAddEditDialog = new MTIncomeImportRowAddEditDialog(DialogWindowMode.NewModeWithNext, this, mTIncomeImportSpecificationRow);
		mTIncomeImportRowAddEditDialog.Invoke();
		int addedRows = mTIncomeImportRowAddEditDialog.AddedRows;
		if (addedRows != 0)
		{
			_maxRowID += addedRows;
			_mtIncomeImportGrid.Grid.RefreshData();
			CalcSummAndSummVAT();
			RowInfoChangeEventHandler rowInfoChangeEventHandler = RowInfoChange;
			if (rowInfoChangeEventHandler != null)
			{
				ObservableCollection<MTIncomeImportSpecificationRow> mtIncomeImportList = _mtIncomeImportList;
				rowInfoChangeEventHandler(mtIncomeImportList[mtIncomeImportList.Count - 1]);
			}
		}
	}

	private void MTIncomeImportGrid_EditingRowWithDialog(object sender, DocumentGridEditingRowEventArgs e)
	{
		MTIncomeImportSpecificationRow mTIncomeImportSpecificationRow = (MTIncomeImportSpecificationRow)_mtIncomeImportGrid.Grid.GetRow(e.RowHandle);
		if (mTIncomeImportSpecificationRow == null)
		{
			return;
		}
		MTIncomeImportRowAddEditDialog mTIncomeImportRowAddEditDialog = new MTIncomeImportRowAddEditDialog(e.AllowEdit ? DialogWindowMode.EditMode : DialogWindowMode.ViewMode, this, mTIncomeImportSpecificationRow);
		e.RowEdited = Convert.ToBoolean(mTIncomeImportRowAddEditDialog.Invoke());
		if (e.AllowEdit && e.RowEdited)
		{
			_mtIncomeImportGrid.Grid.RefreshData();
			CalcSummAndSummVAT();
			RowInfoChangeEventHandler rowInfoChangeEventHandler = RowInfoChange;
			if (rowInfoChangeEventHandler != null)
			{
				ObservableCollection<MTIncomeImportSpecificationRow> mtIncomeImportList = _mtIncomeImportList;
				rowInfoChangeEventHandler(mtIncomeImportList[mtIncomeImportList.Count - 1]);
			}
		}
	}

	private void MTIncomeImportGrid_ClickDropDown(object sender, DocumentGridClickDropDownEventArgs e)
	{
		try
		{
			MTIncomeImportSpecificationRow mTIncomeImportSpecificationRow = (MTIncomeImportSpecificationRow)_mtIncomeImportGrid.Grid.GetRow(e.RowHandle);
			if (e.Column == _codeCol)
			{
				GoodsFilter goodsFilter = new GoodsFilter(shortMode: true, "1", "", isShowOnlyInPriceList: true);
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
			else if (e.Column == _property1Col || e.Column == _property2Col)
			{
				if (mTIncomeImportSpecificationRow == null)
				{
					return;
				}
				MaterialCacheRow item = TradeSettings.TradeCacheService.Material.GetItem(mTIncomeImportSpecificationRow.MTCode);
				if (item == null || (string.IsNullOrWhiteSpace(item.Property1) && e.Column == _property1Col) || (string.IsNullOrWhiteSpace(item.Property2) && e.Column == _property2Col))
				{
					return;
				}
				ArmSoft.SME.Trade.UI.DropDown.PropertyList propertyList = new ArmSoft.SME.Trade.UI.DropDown.PropertyList((e.Column == _property1Col) ? item.Property1 : item.Property2);
				ModalBrowser modalBrowser2 = new ModalBrowser(propertyList)
				{
					Owner = e.Owner,
					Left = e.Left,
					Top = e.Top
				};
				if (modalBrowser2.Show())
				{
					e.NewValue = propertyList.CommentMember(modalBrowser2.SelectedRowIndex);
					if (e.Column == _property1Col)
					{
						mTIncomeImportSpecificationRow.Property1 = propertyList.ValueMember(modalBrowser2.SelectedRowIndex);
					}
					else
					{
						mTIncomeImportSpecificationRow.Property2 = propertyList.ValueMember(modalBrowser2.SelectedRowIndex);
					}
				}
			}
			_mtIncomeImportGrid.Grid.RefreshData();
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	private void MTIncomeImportGrid_MovingRowUp(object sender, DocumentGridMovingRowUpOrDownEventArgs e)
	{
		if (_mtIncomeImportGrid.Grid.SortInfo.Count > 0 || _mtIncomeImportTableView.Grid.VisibleRowCount != _mtIncomeImportList.Count)
		{
			throw new ASException(ArmSoft.SME.Common.Resources.NotAllowRowMoveInSortedGrid);
		}
		MTIncomeImportSpecificationRow mTIncomeImportSpecificationRow = (MTIncomeImportSpecificationRow)_mtIncomeImportGrid.Grid.GetRow(e.RowHandle);
		if (mTIncomeImportSpecificationRow != null)
		{
			_mtIncomeImportList.RemoveAt(e.RowHandle);
			_mtIncomeImportList.Insert(e.RowHandle - 1, mTIncomeImportSpecificationRow);
		}
		_mtIncomeImportGrid.Grid.RefreshData();
	}

	private void MTIncomeImportGrid_MovingRowDown(object sender, DocumentGridMovingRowUpOrDownEventArgs e)
	{
		if (_mtIncomeImportGrid.Grid.SortInfo.Count > 0 && _mtIncomeImportTableView.Grid.VisibleRowCount != _mtIncomeImportList.Count)
		{
			throw new ASException(ArmSoft.SME.Common.Resources.NotAllowRowMoveInSortedGrid);
		}
		MTIncomeImportSpecificationRow mTIncomeImportSpecificationRow = (MTIncomeImportSpecificationRow)_mtIncomeImportGrid.Grid.GetRow(e.RowHandle);
		if (mTIncomeImportSpecificationRow != null)
		{
			_mtIncomeImportList.RemoveAt(e.RowHandle);
			_mtIncomeImportList.Insert(e.RowHandle + 1, mTIncomeImportSpecificationRow);
		}
		_mtIncomeImportGrid.Grid.RefreshData();
	}

	private void MTIncomeImportGrid_RefreshButtons(object sender, EventArgs e)
	{
		_viewAvailAndTurn.IsEnabled = Settings.UserAccessService.GetProviderPermission(DataProviderTypeSE.MatRepAvailability).AllowView || Settings.UserAccessService.GetProviderPermission(DataProviderTypeSE.MatRepAvailabilityTreeView).AllowView || Settings.UserAccessService.GetProviderPermission(DataProviderTypeSE.GoodsRepOperations).AllowView || Settings.UserAccessService.GetProviderPermission(DataProviderTypeSE.MatRepSalesAnalysis).AllowView;
		_filteredSearch.IsEnabled = _mtIncomeImportGrid.AllowAdd && !_mtIncomeImportGrid.IsReadOnly;
		_actualQuantitiesCheck.IsEnabled = _mtIncomeImportGrid.AllowAdd && !_mtIncomeImportGrid.IsReadOnly;
	}

	private void MTIncomeImportTableView_CellValueChanged(object sender, CellValueChangedEventArgs e)
	{
		MTIncomeImportSpecificationRow mTIncomeImportSpecificationRow = (MTIncomeImportSpecificationRow)_mtIncomeImportGrid.Grid.GetRow(e.RowHandle);
		if (mTIncomeImportSpecificationRow == null)
		{
			return;
		}
		string oldUnit = "";
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
					mTIncomeImportSpecificationRow.MTID = -1;
					mTIncomeImportSpecificationRow.MTName = "";
					mTIncomeImportSpecificationRow.UnitCode = "";
					mTIncomeImportSpecificationRow.UnitBrief = "";
				}
				else
				{
					string text = e.Value.ToString();
					if (string.IsNullOrWhiteSpace(text))
					{
						throw new Exception();
					}
					MaterialCacheRow item = TradeSettings.TradeCacheService.Material.GetItem(text, throwExceptionNotExits: true);
					mTIncomeImportSpecificationRow.MTID = item.Id;
					mTIncomeImportSpecificationRow.MTName = item.Caption;
					mTIncomeImportSpecificationRow.UnitCode = TradeSettings.CreateTradeClient().CommonServices.GetMTQntUnitByMTIDPriceTypeAndDocDate(mTIncomeImportSpecificationRow.MTID, _priceListType.Code, _date.EditValue.Value);
					if (string.IsNullOrWhiteSpace(mTIncomeImportSpecificationRow.UnitCode))
					{
						mTIncomeImportSpecificationRow.UnitCode = item.Unit;
					}
					QntUnitCacheRow item2 = TradeSettings.TradeCacheService.QntUnit.GetItem(mTIncomeImportSpecificationRow.UnitCode);
					mTIncomeImportSpecificationRow.UnitBrief = ((item2 == null) ? "" : item2.Brief);
					_unitCodeCol.DataView = new ArmSoft.SME.Trade.UI.DropDown.MTQntUnits(item.MTCode);
					mTIncomeImportSpecificationRow.Price = TradeSettings.CreateTradeClient().CommonServices.GetPriceForGivenPriceListAndVATType(_priceListType.Code, "1", mTIncomeImportSpecificationRow.MTID, mTIncomeImportSpecificationRow.UnitCode, _date.EditValue.Value, _currCode.Code);
					mTIncomeImportSpecificationRow.Summa = (mTIncomeImportSpecificationRow.Quantity * mTIncomeImportSpecificationRow.Price).RoundAs(2);
					mTIncomeImportSpecificationRow.VAT = item.VAT;
					UpdateCustomSumms(mTIncomeImportSpecificationRow);
					mTIncomeImportSpecificationRow.Country = item.Country;
					mTIncomeImportSpecificationRow.Producer = item.Producer;
					mTIncomeImportSpecificationRow.Property1 = "";
					mTIncomeImportSpecificationRow.Property2 = "";
					mTIncomeImportSpecificationRow.Property1Caption = "";
					mTIncomeImportSpecificationRow.Property2Caption = "";
					_property1Col.DataView = new ArmSoft.SME.Trade.UI.DropDown.PropertyList(item.Property1);
					_property2Col.DataView = new ArmSoft.SME.Trade.UI.DropDown.PropertyList(item.Property1);
				}
			}
			catch (Exception ex)
			{
				mTIncomeImportSpecificationRow.MTName = "";
				mTIncomeImportSpecificationRow.MTID = -1;
				mTIncomeImportSpecificationRow.UnitCode = "";
				mTIncomeImportSpecificationRow.UnitBrief = "";
				mTIncomeImportSpecificationRow.Quantity = 0m;
				mTIncomeImportSpecificationRow.Summa = 0m;
				mTIncomeImportSpecificationRow.Price = 0m;
				mTIncomeImportSpecificationRow.VAT = false;
				mTIncomeImportSpecificationRow.Country = "";
				mTIncomeImportSpecificationRow.Producer = "";
				mTIncomeImportSpecificationRow.Property1 = "";
				mTIncomeImportSpecificationRow.Property2 = "";
				if (e.Value != null && !string.IsNullOrWhiteSpace(e.Value.ToString()))
				{
					Settings.ErrorHandler(ex);
				}
			}
			finally
			{
				RowInfoChange?.Invoke(mTIncomeImportSpecificationRow);
			}
		}
		else if (e.Column == _unitCodeCol)
		{
			if (e.Value != null && !string.IsNullOrWhiteSpace(e.Value.ToString()) && !string.IsNullOrWhiteSpace(mTIncomeImportSpecificationRow.MTCode))
			{
				QntUnitCacheRow item3 = TradeSettings.TradeCacheService.QntUnit.GetItem(e.Value.ToString(), throwExceptionNotExits: true);
				if (item3 != null)
				{
					mTIncomeImportSpecificationRow.UnitCode = item3.Code;
					mTIncomeImportSpecificationRow.UnitBrief = item3.Brief;
					if (!string.IsNullOrWhiteSpace(_priceListType.Code))
					{
						PricesDataProvider.DataRow price = TradeSettings.CreateTradeClient().CommonServices.GetPrice(_priceListType.Code, mTIncomeImportSpecificationRow.MTID, mTIncomeImportSpecificationRow.UnitCode, _date.EditValue.Value, _currCode.Code);
						if (price.Price != 0m)
						{
							mTIncomeImportSpecificationRow.Price = price.Price;
						}
						else
						{
							mTIncomeImportSpecificationRow.Price = ArmSoft.SME.Trade.UI.Helpers.PriceHelper.GetPriceForUnit(oldUnit, mTIncomeImportSpecificationRow.UnitCode, mTIncomeImportSpecificationRow.Price, mTIncomeImportSpecificationRow.MTCode);
						}
						decimal priceForGivenPriceListAndVATType = TradeSettings.CreateTradeClient().CommonServices.GetPriceForGivenPriceListAndVATType(_priceListType.Code, "1", mTIncomeImportSpecificationRow.MTID, mTIncomeImportSpecificationRow.UnitCode, _date.EditValue.Value, _currCode.Code);
						if (priceForGivenPriceListAndVATType != 0m)
						{
							mTIncomeImportSpecificationRow.Price = priceForGivenPriceListAndVATType;
						}
					}
					else
					{
						mTIncomeImportSpecificationRow.Price = ArmSoft.SME.Trade.UI.Helpers.PriceHelper.GetPriceForUnit(oldUnit, mTIncomeImportSpecificationRow.UnitCode, mTIncomeImportSpecificationRow.Price, mTIncomeImportSpecificationRow.MTCode);
					}
					mTIncomeImportSpecificationRow.Summa = (mTIncomeImportSpecificationRow.Quantity * mTIncomeImportSpecificationRow.Price).RoundAs(2);
					UpdateCustomSumms(mTIncomeImportSpecificationRow);
				}
				else
				{
					mTIncomeImportSpecificationRow.UnitBrief = "";
				}
			}
			else
			{
				mTIncomeImportSpecificationRow.UnitBrief = "";
			}
		}
		else if (e.Column == _quantityCol)
		{
			mTIncomeImportSpecificationRow.Summa = (Convert.ToDecimal(e.Value) * mTIncomeImportSpecificationRow.Price).RoundAs(2);
			UpdateCustomSumms(mTIncomeImportSpecificationRow);
		}
		else if (e.Column == _priceCol)
		{
			mTIncomeImportSpecificationRow.Summa = (Convert.ToDecimal(e.Value) * mTIncomeImportSpecificationRow.Quantity).RoundAs(2);
			if (Settings.ParametersService.ShowGoodsCostInReportsAndDocs())
			{
				if (_showLastPurchasePrices)
				{
					SetVariancePercent(mTIncomeImportSpecificationRow);
				}
				else
				{
					SetLastPurchaseAndVarianceInfoPanel((MTIncomeImportSpecificationRow)_mtIncomeImportGrid.Grid.CurrentItem);
				}
			}
			UpdateCustomSumms(mTIncomeImportSpecificationRow);
		}
		else if (e.Column == _summaCol && mTIncomeImportSpecificationRow.Quantity != 0m)
		{
			mTIncomeImportSpecificationRow.Price = (Convert.ToDecimal(e.Value) / mTIncomeImportSpecificationRow.Quantity).RoundAs(2);
			UpdateCustomSumms(mTIncomeImportSpecificationRow);
		}
		else if (e.Column == _customSummaCol)
		{
			UpdateCustomDuty(mTIncomeImportSpecificationRow);
		}
		else if (e.Column == _customPercentCol)
		{
			UpdateCustomDuty(mTIncomeImportSpecificationRow);
		}
		else if (e.Column == _customDutyCol)
		{
			UpdateVATSumma(mTIncomeImportSpecificationRow);
		}
		else if (e.Column == _vatCol)
		{
			UpdateVATSumma(mTIncomeImportSpecificationRow);
		}
		_mtIncomeImportGrid.Grid.RefreshData();
		CalcSummAndSummVAT();
	}

	private void MTIncomeImportTableView_FocusedRowHandleChanged(object sender, FocusedRowHandleChangedEventArgs e)
	{
		int focusedRowHandle = _mtIncomeImportTableView.FocusedRowHandle;
		MTIncomeImportSpecificationRow mTIncomeImportSpecificationRow = (MTIncomeImportSpecificationRow)_mtIncomeImportTableView.Grid.GetRow(focusedRowHandle);
		if (mTIncomeImportSpecificationRow != null)
		{
			ChangeColumnAdditonalView(mTIncomeImportSpecificationRow.MTCode);
		}
		RowInfoChange?.Invoke(mTIncomeImportSpecificationRow);
	}

	private void ChangeColumnAdditonalView(string ItemCode)
	{
		_unitCodeCol.DataView = new ArmSoft.SME.Trade.UI.DropDown.MTQntUnits(string.IsNullOrWhiteSpace(ItemCode) ? "" : ItemCode);
	}

	private void MTIncomeImportTableView_ShowingEditor(object sender, ShowingEditorEventArgs e)
	{
		if (((!_property1Col.Visible || e.Column != _property1Col) && (!_property2Col.Visible || e.Column != _property2Col)) || e.Row == null)
		{
			return;
		}
		MTIncomeImportSpecificationRow mTIncomeImportSpecificationRow = (MTIncomeImportSpecificationRow)e.Row;
		MaterialCacheRow item = TradeSettings.TradeCacheService.Material.GetItem(mTIncomeImportSpecificationRow.MTCode);
		if (item != null)
		{
			if ((string.IsNullOrWhiteSpace(item.Property1) && e.Column == _property1Col) || (string.IsNullOrWhiteSpace(item.Property2) && e.Column == _property2Col))
			{
				e.Cancel = true;
			}
		}
		else
		{
			e.Cancel = true;
		}
	}

	private void SetVariancePercent(MTIncomeImportSpecificationRow row = null)
	{
		if (row == null)
		{
			foreach (MTIncomeImportSpecificationRow mtIncomeImport in _mtIncomeImportList)
			{
				if (mtIncomeImport.LastPurchasePrice != 0m)
				{
					mtIncomeImport.VariancePercent = ((mtIncomeImport.Price - mtIncomeImport.LastPurchasePrice) / mtIncomeImport.LastPurchasePrice * 100m).RoundAs(2);
					if (mtIncomeImport.VariancePercent / 1000m >= 1m)
					{
						mtIncomeImport.VariancePercent = 999.99m;
					}
					else if (mtIncomeImport.VariancePercent / 1000m <= -1m)
					{
						mtIncomeImport.VariancePercent = -99.99m;
					}
				}
				else
				{
					mtIncomeImport.VariancePercent = 0m;
				}
			}
			return;
		}
		if (row.LastPurchasePrice != 0m)
		{
			row.VariancePercent = ((row.Price - row.LastPurchasePrice) / row.LastPurchasePrice * 100m).RoundAs(2);
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

	private void Date_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		if (_date.DoValidate() && _date.EditValue.HasValue)
		{
			if (Settings.ParametersService.ShowGoodsCostInReportsAndDocs() && _showLastPurchasePrices)
			{
				SetLastPurchaseInfoToColumn();
			}
			else if (!_showLastPurchasePrices && _mtIncomeImportGrid.Grid.CurrentItem != null)
			{
				SetLastPurchaseAndVarianceInfoPanel((MTIncomeImportSpecificationRow)_mtIncomeImportGrid.Grid.CurrentItem);
			}
		}
		else
		{
			ControlsHelper.SetFocus(_date);
			ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Common.Resources.ValidateEmptyDate, Settings.ProgramName, MessageBoxButton.OK, MessageBoxImage.Exclamation);
		}
		_mtIncomeImportGrid.Grid.RefreshData();
	}

	private void CurrExChngRate_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		if (_currExChngRate.DoValidate())
		{
			bool flag = false;
			if (_isAskQuestionConfirmSummChange && !string.IsNullOrWhiteSpace(_currCode.Code) && _currCode.DoValidate())
			{
				flag = true;
			}
			if (flag)
			{
				UpdateCustomSumms();
			}
			else
			{
				CalculatePartSumma();
				CalculateCustomPartSumma();
			}
			if (!string.IsNullOrWhiteSpace(_currCode.Code) && _currCode.Code != "AMD" && _currCode.DoValidate())
			{
				_oldDocCurrExChngRate = _currExChngRate.Rate;
				_oldDocCurrExChngBase = _currExChngRate.Base;
			}
		}
	}

	private void PartnerCode_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		if (Settings.ParametersService.ShowGoodsCostInReportsAndDocs() && _showLastPurchasePrices)
		{
			SetLastPurchaseInfoToColumn();
			_mtIncomeImportGrid.Grid.RefreshData();
		}
		RowInfoChange?.Invoke((MTIncomeImportSpecificationRow)_mtIncomeImportTableView.Grid.CurrentItem);
		CalculatePartSumma();
	}

	private void PartnerCode_EditAdditionalValueChanged(object sender, EventArgs e)
	{
		CalculatePartSumma();
	}

	private void PartnerContractCode_EditValueChanged(object sender, EventArgs e)
	{
		if (_isAskQuestionConfirmSummChange && (_mtIncomeImportList.Count == 0 || string.IsNullOrWhiteSpace(_priceListType.Code)) && !string.IsNullOrWhiteSpace(_supplierContractContainer.Contract.Code) && _supplierContractContainer.Contract.DoValidate())
		{
			PartnerCacheRow item = TradeSettings.TradeCacheService.Partner.GetItem(_supplierContractContainer.PartnerCode.Code, throwExceptionNotExits: true);
			if (item != null)
			{
				PartnerCacheRow.ContractRow contract = TradeSettings.TradeCacheService.Partner.GetContract(item, _supplierContractContainer.Contract.Code);
				if (contract != null && !string.IsNullOrWhiteSpace(contract.PriceType))
				{
					_isAskQuestionConfirmSummChange = false;
					_priceListType.Code = contract.PriceType;
					_isAskQuestionConfirmSummChange = true;
					if (_mtIncomeImportList.Count > 0)
					{
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

	private void CustomCode_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		CalculateCustomPartSumma();
	}

	private void CustomCode_EditAdditionalValueChanged(object sender, EventArgs e)
	{
		CalculateCustomPartSumma();
	}

	private void CustomContractCode_EditValueChanged(object sender, EventArgs e)
	{
		if (!string.IsNullOrWhiteSpace(_customContractContainer.ContractCurrCode.Code) && _customContractContainer.ContractCurrCode.DoValidate() && _customContractContainer.ContractCurrCode.Code != "AMD")
		{
			ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Trade.Common.Resources.ValidateCustomContrIsAMD, ArmSoft.SME.Trade.Common.Resources.ProgramName, MessageBoxButton.OK, MessageBoxImage.Asterisk);
			_customContractContainer.Contract.Code = "";
			_customContractContainer.ContractCurrCode.Code = "";
			_customContractContainer.CalculatePartSumma();
			_customContractContainer.CalculatePartDebtRem();
		}
		else
		{
			CalculateCustomPartSumma();
		}
	}

	private void CalculateCustomPartSumma()
	{
		if (!string.IsNullOrWhiteSpace(_customContractContainer.PartnerCode.Code) && _customContractContainer.PartnerCode.DoValidate())
		{
			_customContractContainer.SummaPart.EditValue = _customDuty.Value + _customFee.Value + _vatSumma.Value;
		}
	}

	protected override bool AllowChangeGridMode()
	{
		return true;
	}
}
