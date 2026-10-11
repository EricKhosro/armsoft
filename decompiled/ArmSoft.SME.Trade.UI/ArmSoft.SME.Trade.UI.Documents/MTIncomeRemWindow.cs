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
using ArmSoft.SME.Trade.Common;
using ArmSoft.SME.Trade.Common.Documents.Definition;
using ArmSoft.SME.Trade.Common.Models.Caches;
using ArmSoft.SME.Trade.Common.Models.Documents;
using ArmSoft.SME.Trade.Common.Parameters;
using ArmSoft.SME.Trade.Common.PersistableObject;
using ArmSoft.SME.Trade.Common.SmartEnums;
using ArmSoft.SME.Trade.UI.CustomUIElements;
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

[DocumentWindow(24)]
public class MTIncomeRemWindow : TradeDocumentWindow<MTIncomeRem>
{
	public class MTIncomeRemFilteredSearchList : IResultList
	{
		private readonly ObservableCollection<MTIncomeRemSpecificationRow> itemSource = new ObservableCollection<MTIncomeRemSpecificationRow>();

		private readonly IPOWindowInfo docWindow;

		public IEnumerable<DocumentGridRow> ItemsSource => itemSource;

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

		public MTIncomeRemFilteredSearchList(IPOWindowInfo docWindow)
		{
			this.docWindow = docWindow;
		}

		public int AddToItemsSource(object key, params object[] args)
		{
			string mtCode = (string)key;
			decimal num = (decimal)args[0];
			int num2 = itemSource.FindIndex((MTIncomeRemSpecificationRow x) => x.MTCode == mtCode);
			if (num2 == -1)
			{
				MaterialCacheRow item = TradeSettings.TradeCacheService.Material.GetItem(mtCode);
				if (item == null)
				{
					return -1;
				}
				MTIncomeRemSpecificationRow item2 = new MTIncomeRemSpecificationRow(docWindow)
				{
					MTID = item.Id,
					MTCode = item.MTCode,
					MTName = item.Caption,
					UnitCode = item.Unit,
					UnitBrief = item.UnitRow.Brief,
					VAT = item.VAT,
					Country = item.Country,
					Producer = item.Producer
				};
				itemSource.Add(item2);
				num2 = itemSource.Count - 1;
			}
			itemSource[num2].Quantity += num;
			return num2;
		}
	}

	public class MTIncomeRemSpecificationRow : DocumentGridRow
	{
		[ObservableProperty]
		private string storage = "";

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
		private decimal currPrice;

		[ObservableProperty]
		private decimal currSumma;

		[ObservableProperty]
		private decimal salePrice;

		[ObservableProperty]
		private decimal saleSumma;

		[ObservableProperty]
		private string vATCodeForConvert;

		[ObservableProperty]
		private bool vAT;

		[ObservableProperty]
		private decimal vATPrice;

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

		public short RowNum;

		[ObservableProperty]
		private int rowID;

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
		public decimal CurrPrice
		{
			get
			{
				return currPrice;
			}
			set
			{
				if (!EqualityComparer<decimal>.Default.Equals(currPrice, value))
				{
					OnPropertyChanging(__KnownINotifyPropertyChangingArgs.CurrPrice);
					currPrice = value;
					OnPropertyChanged(__KnownINotifyPropertyChangedArgs.CurrPrice);
				}
			}
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public decimal CurrSumma
		{
			get
			{
				return currSumma;
			}
			set
			{
				if (!EqualityComparer<decimal>.Default.Equals(currSumma, value))
				{
					OnPropertyChanging(__KnownINotifyPropertyChangingArgs.CurrSumma);
					currSumma = value;
					OnPropertyChanged(__KnownINotifyPropertyChangedArgs.CurrSumma);
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
					OnSalePriceChanged(value);
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
					OnSaleSummaChanged(value);
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
		public decimal VATPrice
		{
			get
			{
				return vATPrice;
			}
			set
			{
				if (!EqualityComparer<decimal>.Default.Equals(vATPrice, value))
				{
					OnPropertyChanging(__KnownINotifyPropertyChangingArgs.VATPrice);
					vATPrice = value;
					OnVATPriceChanged(value);
					OnPropertyChanged(__KnownINotifyPropertyChangedArgs.VATPrice);
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
					OnVATSummaChanged(value);
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

		static MTIncomeRemSpecificationRow()
		{
			DocumentGridRow.RegisterPropertyValidators(typeof(MTIncomeRemSpecificationRow), cacheValidationResults: true, new Dictionary<string, Action<DocumentGridRow, DevExpress.XtraEditors.DXErrorProvider.ErrorInfo>>
			{
				{
					"Storage",
					(DocumentGridRow row, DevExpress.XtraEditors.DXErrorProvider.ErrorInfo info) =>
					{
						ValidateStorage((MTIncomeRemSpecificationRow)row, info);
					}
				},
				{
					"MTCode",
					(DocumentGridRow row, DevExpress.XtraEditors.DXErrorProvider.ErrorInfo info) =>
					{
						ValidateGoodCode((MTIncomeRemSpecificationRow)row, info);
					}
				},
				{
					"UnitCode",
					(DocumentGridRow row, DevExpress.XtraEditors.DXErrorProvider.ErrorInfo info) =>
					{
						ValidateUnitCode((MTIncomeRemSpecificationRow)row, info);
					}
				},
				{
					"Quantity",
					(DocumentGridRow row, DevExpress.XtraEditors.DXErrorProvider.ErrorInfo info) =>
					{
						ValidateQuantity((MTIncomeRemSpecificationRow)row, info);
					}
				},
				{
					"Summa",
					(DocumentGridRow row, DevExpress.XtraEditors.DXErrorProvider.ErrorInfo info) =>
					{
						ValidateSumma((MTIncomeRemSpecificationRow)row, info);
					}
				},
				{
					"VATSumma",
					(DocumentGridRow row, DevExpress.XtraEditors.DXErrorProvider.ErrorInfo info) =>
					{
						ValidateVATSumma((MTIncomeRemSpecificationRow)row, info);
					}
				},
				{
					"SalePrice",
					(DocumentGridRow row, DevExpress.XtraEditors.DXErrorProvider.ErrorInfo info) =>
					{
						ValidateSalePrice((MTIncomeRemSpecificationRow)row, info);
					}
				},
				{
					"SaleSumma",
					(DocumentGridRow row, DevExpress.XtraEditors.DXErrorProvider.ErrorInfo info) =>
					{
						ValidateSaleSumma((MTIncomeRemSpecificationRow)row, info);
					}
				},
				{
					"Country",
					(DocumentGridRow row, DevExpress.XtraEditors.DXErrorProvider.ErrorInfo info) =>
					{
						ValidateCountry((MTIncomeRemSpecificationRow)row, info);
					}
				},
				{
					"Producer",
					(DocumentGridRow row, DevExpress.XtraEditors.DXErrorProvider.ErrorInfo info) =>
					{
						ValidateProducer((MTIncomeRemSpecificationRow)row, info);
					}
				},
				{
					"Property1",
					(DocumentGridRow row, DevExpress.XtraEditors.DXErrorProvider.ErrorInfo info) =>
					{
						ValidateProperty1((MTIncomeRemSpecificationRow)row, info);
					}
				},
				{
					"Property2",
					(DocumentGridRow row, DevExpress.XtraEditors.DXErrorProvider.ErrorInfo info) =>
					{
						ValidateProperty2((MTIncomeRemSpecificationRow)row, info);
					}
				},
				{
					"Property1Caption",
					(DocumentGridRow row, DevExpress.XtraEditors.DXErrorProvider.ErrorInfo info) =>
					{
						ValidateProperty1Caption((MTIncomeRemSpecificationRow)row, info);
					}
				},
				{
					"Property2Caption",
					(DocumentGridRow row, DevExpress.XtraEditors.DXErrorProvider.ErrorInfo info) =>
					{
						ValidateProperty2Caption((MTIncomeRemSpecificationRow)row, info);
					}
				}
			});
		}

		public MTIncomeRemSpecificationRow(IPOWindowInfo docWindow)
			: base(docWindow)
		{
		}

		private static void ValidateStorage(MTIncomeRemSpecificationRow row, DevExpress.XtraEditors.DXErrorProvider.ErrorInfo info)
		{
			TradePropertyValidator.ValidateStorage(row.Storage, info, TradeAllDocDefinitions.MTIncomeRem.Grids["MTList"].Columns["Storage"]);
		}

		private static void ValidateGoodCode(MTIncomeRemSpecificationRow row, DevExpress.XtraEditors.DXErrorProvider.ErrorInfo info)
		{
			TradePropertyValidator.ValidateMaterialCode(row.MTCode, info, TradeAllDocDefinitions.MTIncomeRem.Grids["MTList"].Columns["MTCode"]);
		}

		private static void ValidateUnitCode(MTIncomeRemSpecificationRow row, DevExpress.XtraEditors.DXErrorProvider.ErrorInfo info)
		{
			TradePropertyValidator.ValidateGoodUnitCode(row.UnitCode, row.MTCode, info, TradeAllDocDefinitions.MTIncomeRem.Grids["MTList"].Columns["UnitCode"]);
		}

		private static void ValidateSumma(MTIncomeRemSpecificationRow row, DevExpress.XtraEditors.DXErrorProvider.ErrorInfo info)
		{
			if (((MTIncomeRemWindow)row.DocWindow).PO.CreateType == ((short)3).ToString() || Settings.ParametersService.AllowPurchaseWith0PriceOr100Percent())
			{
				PropertyValidator.NonNegativeValidate(row.Summa, info, ArmSoft.SME.Common.Resources.Summa + ArmSoft.SME.Trade.Common.Resources.WithoutNDS);
			}
			else
			{
				PropertyValidator.PositiveValidate(row.Summa, info, ArmSoft.SME.Common.Resources.Summa + ArmSoft.SME.Trade.Common.Resources.WithoutNDS);
			}
		}

		private static void ValidateSalePrice(MTIncomeRemSpecificationRow row, DevExpress.XtraEditors.DXErrorProvider.ErrorInfo info)
		{
			PropertyValidator.NonNegativeValidate(row.SalePrice, info, ArmSoft.SME.Trade.Common.Resources.NCCSalePrice);
		}

		private static void ValidateSaleSumma(MTIncomeRemSpecificationRow row, DevExpress.XtraEditors.DXErrorProvider.ErrorInfo info)
		{
			if (((MTIncomeRemWindow)row.DocWindow).PO.CreateType != ((short)3).ToString())
			{
				StoragesCacheRow item = TradeSettings.TradeCacheService.Storage.GetItem(row.Storage);
				if (item != null && item.Retail)
				{
					PropertyValidator.PositiveValidate(row.SaleSumma, info, ArmSoft.SME.Trade.Common.Resources.MatRepSaleTotalSum);
					return;
				}
			}
			PropertyValidator.NonNegativeValidate(row.SaleSumma, info, ArmSoft.SME.Trade.Common.Resources.MatRepSaleTotalSum);
		}

		private static void ValidateVATSumma(MTIncomeRemSpecificationRow row, DevExpress.XtraEditors.DXErrorProvider.ErrorInfo info)
		{
			if (((MTIncomeRemWindow)row.DocWindow).PO.CreateType == ((short)3).ToString() || Settings.ParametersService.AllowPurchaseWith0PriceOr100Percent())
			{
				PropertyValidator.NonNegativeValidate(row.VATSumma, info, ArmSoft.SME.Common.Resources.Summa + ArmSoft.SME.Trade.Common.Resources.WithNDS);
			}
			else
			{
				PropertyValidator.PositiveValidate(row.VATSumma, info, ArmSoft.SME.Common.Resources.Summa + ArmSoft.SME.Trade.Common.Resources.WithNDS);
			}
		}

		protected static void ValidateCountry(MTIncomeRemSpecificationRow row, DevExpress.XtraEditors.DXErrorProvider.ErrorInfo info)
		{
			if (!string.IsNullOrWhiteSpace(row.Country))
			{
				TradePropertyValidator.ValidateCountry(row.Country, info, TradeAllDocDefinitions.MTIncomeRem.Grids["MTList"].Columns["Country"]);
			}
		}

		private static void ValidateQuantity(MTIncomeRemSpecificationRow row, DevExpress.XtraEditors.DXErrorProvider.ErrorInfo info)
		{
			PropertyValidator.PositiveValidate(row.Quantity, info, ArmSoft.SME.Common.Resources.Quantity);
		}

		protected static void ValidateProducer(MTIncomeRemSpecificationRow row, DevExpress.XtraEditors.DXErrorProvider.ErrorInfo info)
		{
			if (!string.IsNullOrWhiteSpace(row.Producer))
			{
				TradePropertyValidator.ValidateProducer(row.Producer, info, TradeAllDocDefinitions.MTIncome.Grids["MTList"].Columns["Producer"]);
			}
		}

		protected static void ValidateProperty1(MTIncomeRemSpecificationRow row, DevExpress.XtraEditors.DXErrorProvider.ErrorInfo info)
		{
			if (!string.IsNullOrWhiteSpace(row.Property1))
			{
				TradePropertyValidator.ValidateProperty(row.Property1, row.MTCode, isProperty1: true, info, TradeAllDocDefinitions.MTIncomeRem.Grids["MTList"].Columns["Property1"]);
			}
		}

		protected static void ValidateProperty2(MTIncomeRemSpecificationRow row, DevExpress.XtraEditors.DXErrorProvider.ErrorInfo info)
		{
			if (!string.IsNullOrWhiteSpace(row.Property2))
			{
				TradePropertyValidator.ValidateProperty(row.Property2, row.MTCode, isProperty1: false, info, TradeAllDocDefinitions.MTIncomeRem.Grids["MTList"].Columns["Property2"]);
			}
		}

		protected static void ValidateProperty1Caption(MTIncomeRemSpecificationRow row, DevExpress.XtraEditors.DXErrorProvider.ErrorInfo info)
		{
			if (!string.IsNullOrWhiteSpace(row.Property1Caption))
			{
				TradePropertyValidator.ValidatePropertyValue(row.Property1Caption, row.MTCode, isProperty1: true, info, TradeAllDocDefinitions.MTIncomeRem.Grids["MTList"].Columns["Property1Caption"]);
			}
		}

		protected static void ValidateProperty2Caption(MTIncomeRemSpecificationRow row, DevExpress.XtraEditors.DXErrorProvider.ErrorInfo info)
		{
			if (!string.IsNullOrWhiteSpace(row.Property2Caption))
			{
				TradePropertyValidator.ValidatePropertyValue(row.Property2Caption, row.MTCode, isProperty1: false, info, TradeAllDocDefinitions.MTIncomeRem.Grids["MTList"].Columns["Property2Caption"]);
			}
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		private void OnStorageChanged(string value)
		{
			ResetForceValidationBit("Storage", "SaleSumma");
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
			Summa = (value * Quantity).RoundAs(2);
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		private void OnSummaChanged(decimal value)
		{
			ResetForceValidationBit("Summa");
			Price = ((Quantity == 0m) ? 0m : (value / Quantity).RoundAs(2));
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		private void OnSalePriceChanged(decimal value)
		{
			ResetForceValidationBit("SalePrice");
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		private void OnSaleSummaChanged(decimal value)
		{
			ResetForceValidationBit("SaleSumma");
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		private void OnVATPriceChanged(decimal value)
		{
			ResetForceValidationBit("VATPrice", "VATSumma");
			VATSumma = (value * Quantity).RoundAs(2);
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		private void OnVATSummaChanged(decimal value)
		{
			ResetForceValidationBit("VATSumma");
			VATPrice = ((Quantity == 0m) ? 0m : (value / Quantity).RoundAs(2));
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

	public DropDownControl _storage;

	private TextEditExt _barCode;

	private Button _codeEnter;

	private BarEditItemExt _viewAvailAndTurn;

	private PartnerContractGroupControls _supplierContractContainer;

	private ComboBoxEditExt _comment;

	private DropDownControl _owner;

	public DocumentGridControl _mtIncomeRemGrid;

	private TableView _mtIncomeRemTableView;

	private BarEditItemExt _filteredSearch;

	public ObservableCollection<MTIncomeRemSpecificationRow> _mtIncomeRemList;

	private DocumentGridColumn _storageCol;

	private DocumentGridColumn _codeCol;

	private DocumentGridColumn _unitCodeCol;

	private DocumentGridColumn _quantityCol;

	private DocumentGridColumn _priceCol;

	private DocumentGridColumn _summaCol;

	private DocumentGridColumn _vatCol;

	private DocumentGridColumn _vatPriceCol;

	private DocumentGridColumn _vatSummaCol;

	private DocumentGridColumn _currPriceCol;

	private DocumentGridColumn _currSummaCol;

	private DocumentGridColumn _salePriceCol;

	private DocumentGridColumn _saleSummaCol;

	private DocumentGridColumn _useLifeCol;

	private DocumentGridColumn _countryCol;

	private DocumentGridColumn _producerCol;

	private DocumentGridColumn _property1Col;

	private DocumentGridColumn _property2Col;

	private PopupCalcEditExt _totalSumma;

	private PopupCalcEditExt _vatSumma;

	private PopupCalcEditExt _currSumma;

	private DXTabControl _tabs;

	private int _priceListTabControlIndex;

	public decimal _docCurrExChngRate = 1m;

	public decimal _docCurrExChngBase = 1m;

	public int _maxRowID;

	private bool _allowPurchaseWith0PriceOr100Percent;

	private BarSubItem _barSubItemActions = new BarSubItem();

	private BarSubItem _barSubItemJournals = new BarSubItem();

	private ReportBarButtonItem _barButtonJumpTo;

	private ReportBarButtonItem _barButtonGoodsOperationsJournal;

	private ReportBarButtonItem _barButtonGoodsRetailOperationJournal;

	protected override double WidthValue => 1090.0;

	protected override double HeightValue => 600.0;

	protected override double MinWidthValue => 700.0;

	protected override double MinHeightValue => 250.0;

	protected override bool AllowPrint()
	{
		return true;
	}

	protected override bool AllowPrintOnNew()
	{
		return true;
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
		_storage = AddDropDownControl(null, Definition.Fields["Storage"], new Storages());
		_storage.EditValueChanged += new SafeDocumentEventHandler<EditValueChangedEventArgs>(Storage_EditValueChanged, this).Handler;
		LayoutGroupExt container2 = AddLayoutGroup(null, LayoutGroupView.Group, Orientation.Horizontal);
		_barCode = AddStringControl(container2, ArmSoft.SME.Common.Resources.BarCode + " (F2)", isRequired: false, 20, HorizontalAlignment.Stretch, "Barcode");
		_barCode.NullText = ArmSoft.SME.Trade.Common.Resources.BarCodeAndCodeInputNullText;
		_barCode.KeyDown += new SafeDocumentEventHandler<KeyEventArgs>(BarCode_KeyDown, this).Handler;
		string text = ArmSoft.SME.Trade.Common.Resources.cmdCodeEnter + " (F8)";
		_codeEnter = AddButton(container2, "", text, "", text, isTabStop: true);
		_codeEnter.Click += new SafeDocumentEventHandler<RoutedEventArgs>(OnCodeEnter_Click, this).Handler;
		_tabs = AddTabControl(null);
		LayoutGroup container3 = AddTabbedGroup(_tabs, ArmSoft.SME.Common.Resources.General, "General");
		_supplierContractContainer = AddPartnerContractGroupControls(container3, (PartnerContractGroupDefinition)Definition.Fields["PartnerContractGroup"], _date, null, null);
		_comment = AddComboBoxControl(container3, Definition.Fields["Comment"], TradeSettings.TradeCacheService.StandardComments.GetCaptionsList(), HorizontalAlignment.Stretch);
		_owner = AddDropDownControl(container3, Definition.Fields["Owner"], new UsersFilter());
		_owner.CodeWidth = 12;
		_owner.ShowComment = false;
		_owner.InnerCodeEdit.Width = _owner.InnerCodeEdit.MinWidth;
		if (!Settings.LoginService.IsAdmin && Settings.ParametersService.DenyAccessToOtherOwnerDocs())
		{
			((LayoutItemWithRequired)(object)_owner.Parent).Visibility = Visibility.Collapsed;
		}
		LayoutGroup container4 = AddTabbedGroup(_tabs, ArmSoft.SME.Trade.Common.Resources.MTLIST, "MtList");
		_priceListTabControlIndex = _tabs.Items.Count - 1;
		if (PO.CreateType != ((short)3).ToString())
		{
			_totalSumma = AddNumericControl(container4, Definition.Fields["Summ"]);
			_vatSumma = AddNumericControl(container4, Definition.Fields["SummVAT"]);
			_currSumma = AddNumericControl(container4, Definition.Fields["SummaCurr"]);
			((LayoutItemWithRequired)(object)_currSumma.Parent).Label = ArmSoft.SME.Common.Resources.TotalSum + " (" + Settings.ParametersService.MTCurr() + " )";
		}
		_mtIncomeRemGrid = AddGrid(container4, ArmSoft.SME.Trade.Common.Resources.MTLIST, HorizontalAlignment.Stretch, VerticalAlignment.Stretch, isSupportDialogMode: true);
		_mtIncomeRemGrid.Name = "MTIncomeRemGrid";
		_viewAvailAndTurn = _mtIncomeRemGrid.AddButton(null, ArmSoft.SME.Trade.Common.Resources.AvailAndTurn, ArmSoft.SME.Trade.Common.Resources.AvailabilityAndTurn, 130.0);
		_viewAvailAndTurn.Click += new SafeDocumentEventHandler<RoutedEventArgs>(ViewAvailAndTurn_Click, this).Handler;
		_filteredSearch = _mtIncomeRemGrid.AddButton(null, ArmSoft.SME.Trade.Common.Resources.GroupAddition, ArmSoft.SME.Trade.Common.Resources.GroupAddition, 130.0);
		_filteredSearch.Click += new SafeDocumentEventHandler<RoutedEventArgs>(BE_FilteredSearch_Click, this).Handler;
		_mtIncomeRemGrid.AllowMoveUp = true;
		_mtIncomeRemGrid.AllowMoveDown = true;
		_mtIncomeRemGrid.IsRequired = true;
		_mtIncomeRemTableView = _mtIncomeRemGrid.View;
		_mtIncomeRemGrid.AddRowIndicator(35);
		_mtIncomeRemGrid.AddingRow += new SafeDocumentEventHandler<DocumentGridAddingRowEventArgs>(MTIncomeRemGrid_AddingRow, this).Handler;
		_mtIncomeRemGrid.DeletedRow += new SafeDocumentEventHandler<EventArgs>(MTIncomeRemGrid_DeletedRow, this).Handler;
		_mtIncomeRemGrid.MovingRowUp += new SafeDocumentEventHandler<DocumentGridMovingRowUpOrDownEventArgs>(MTIncomeRemGrid_MovingRowUp, this).Handler;
		_mtIncomeRemGrid.MovingRowDown += new SafeDocumentEventHandler<DocumentGridMovingRowUpOrDownEventArgs>(MTIncomeRemGrid_MovingRowDown, this).Handler;
		_mtIncomeRemGrid.DuplicatingRow += new SafeDocumentEventHandler<DocumentGridDuplicatingRowEventArgs>(MTIncomeRemGrid_DuplicatingRow, this).Handler;
		_mtIncomeRemGrid.AddingRowWithDialog += new SafeDocumentEventHandler<DocumentGridAddingRowEventArgs>(MTIncomeRemGrid_AddingRowWithDialog, this).Handler;
		_mtIncomeRemGrid.DuplicatingRowWithDialog += new SafeDocumentEventHandler<DocumentGridDuplicatingRowEventArgs>(MTIncomeRemGrid_DuplicatingRowWithDialog, this).Handler;
		_mtIncomeRemGrid.EditingRowWithDialog += new SafeDocumentEventHandler<DocumentGridEditingRowEventArgs>(MTIncomeRemGrid_EditingRowWithDialog, this).Handler;
		_mtIncomeRemGrid.ClickDropDown += new SafeDocumentEventHandler<DocumentGridClickDropDownEventArgs>(MTIncomeRemGrid_ClickDropDown, this).Handler;
		_mtIncomeRemGrid.RefreshButtons += new SafeDocumentEventHandler<EventArgs>(MTIncomeRemGrid_RefreshButtons, this).Handler;
		_storageCol = _mtIncomeRemGrid.AddDropDownColumn(Definition.Grids["MTList"].Columns["Storage"], new Storages());
		_codeCol = _mtIncomeRemGrid.AddDropDownColumn(Definition.Grids["MTList"].Columns["MTCode"], new GoodsFilter(shortMode: true, ItemTypesSE.Goods.Value, "", isShowOnlyInPriceList: true), allowAdditionButtons: true, isOwnProcessingClickDropDown: true);
		_codeCol.Width = 125.0;
		DocumentGridColumn documentGridColumn = _mtIncomeRemGrid.AddStringColumn(Definition.Grids["MTList"].Columns["MTName"]);
		documentGridColumn.AllowEditing = DefaultBoolean.False;
		documentGridColumn.Width = 225.0;
		_unitCodeCol = _mtIncomeRemGrid.AddDropDownColumn(Definition.Grids["MTList"].Columns["UnitCode"], new MTQntUnits(), allowAdditionButtons: true);
		_mtIncomeRemGrid.AddStringColumn(Definition.Grids["MTList"].Columns["UnitBrief"]).AllowEditing = DefaultBoolean.False;
		_quantityCol = _mtIncomeRemGrid.AddNumberColumn(Definition.Grids["MTList"].Columns["Quantity"], showTotal: true);
		if (PO.CreateType != ((short)3).ToString())
		{
			_priceCol = _mtIncomeRemGrid.AddNumberColumn(Definition.Grids["MTList"].Columns["Price"]);
			_summaCol = _mtIncomeRemGrid.AddNumberColumn(Definition.Grids["MTList"].Columns["Summa"], showTotal: true);
			_vatCol = _mtIncomeRemGrid.AddBooleanColumn(Definition.Grids["MTList"].Columns["VAT"]);
			_vatCol.Width = 45.0;
			_vatPriceCol = _mtIncomeRemGrid.AddNumberColumn(Definition.Grids["MTList"].Columns["VATPrice"]);
			_vatSummaCol = _mtIncomeRemGrid.AddNumberColumn(Definition.Grids["MTList"].Columns["VATSumma"], showTotal: true);
			_currPriceCol = _mtIncomeRemGrid.AddNumberColumn(Definition.Grids["MTList"].Columns["CurrPrice"]);
			_currPriceCol.Header = ArmSoft.SME.Common.Resources.Price + " (" + Settings.ParametersService.MTCurr() + ")";
			_currSummaCol = _mtIncomeRemGrid.AddNumberColumn(Definition.Grids["MTList"].Columns["CurrSumma"], showTotal: true);
			_currSummaCol.Header = ArmSoft.SME.Common.Resources.Summa + " (" + Settings.ParametersService.MTCurr() + ")";
		}
		if (PO.CreateType != ((short)2).ToString())
		{
			_salePriceCol = _mtIncomeRemGrid.AddNumberColumn(Definition.Grids["MTList"].Columns["SalePrice"]);
			_saleSummaCol = _mtIncomeRemGrid.AddNumberColumn(Definition.Grids["MTList"].Columns["SaleSumma"]);
		}
		if (PO.CreateType != ((short)3).ToString())
		{
			_useLifeCol = _mtIncomeRemGrid.AddDateColumn(Definition.Grids["MTList"].Columns["UseLife"]);
			_useLifeCol.Visible = Settings.ParametersService.ShowPartyUSEABLEDATE();
			_useLifeCol.ShowInColumnChooser = _useLifeCol.Visible;
			_countryCol = _mtIncomeRemGrid.AddDropDownColumn(Definition.Grids["MTList"].Columns["Country"], new Countries());
			_countryCol.Visible = Settings.ParametersService.ShowPartyCountry();
			_countryCol.ShowInColumnChooser = _countryCol.Visible;
			_producerCol = _mtIncomeRemGrid.AddDropDownColumn(Definition.Grids["MTList"].Columns["Producer"], new Producer());
			_producerCol.Width = SizeCalculator.WidthForString(5);
			_producerCol.Visible = Settings.ParametersService.ShowPartyProducer();
			_producerCol.ShowInColumnChooser = _producerCol.Visible;
			PropertyList idv = new PropertyList();
			_property1Col = _mtIncomeRemGrid.AddDropDownCommentColumn(Definition.Grids["MTList"].Columns["Property1Caption"], idv, 50);
			_property1Col.IsOwnProcessingClickDropDown = true;
			_property1Col.AllowEditing = DefaultBoolean.False;
			_property1Col.Width = SizeCalculator.WidthForString(25);
			_property1Col.Visible = Settings.ParametersService.ShowPartyProperty1();
			_property1Col.ShowInColumnChooser = _property1Col.Visible;
			_property2Col = _mtIncomeRemGrid.AddDropDownCommentColumn(Definition.Grids["MTList"].Columns["Property2Caption"], idv, 50);
			_property2Col.IsOwnProcessingClickDropDown = true;
			_property2Col.AllowEditing = DefaultBoolean.False;
			_property2Col.Width = SizeCalculator.WidthForString(25);
			_property2Col.Visible = Settings.ParametersService.ShowPartyProperty2();
			_property2Col.ShowInColumnChooser = _property2Col.Visible;
		}
		foreach (GridColumn column in _mtIncomeRemGrid.Grid.Columns)
		{
			column.AllowSorting = DefaultBoolean.True;
			column.AllowColumnFiltering = DefaultBoolean.True;
			column.FilterPopupMode = FilterPopupMode.CheckedList;
		}
		_mtIncomeRemList = new ObservableCollection<MTIncomeRemSpecificationRow>();
		_mtIncomeRemTableView.CellValueChanged += new SafeDocumentEventHandler<CellValueChangedEventArgs>(MTIncomeRemTableView_CellValueChanged, this).Handler;
		_mtIncomeRemTableView.FocusedRowHandleChanged += new SafeDocumentEventHandler<FocusedRowHandleChangedEventArgs>(MTIncomeRemTableView_FocusedRowHandleChangedEventArgs, this).Handler;
		_mtIncomeRemTableView.ShowingEditor += new SafeDocumentEventHandler<ShowingEditorEventArgs>(MTIncomeRemTableView_ShowingEditor, this).Handler;
	}

	public MTIncomeRemWindow()
	{
	}

	public MTIncomeRemWindow(MTIncomeRem doc)
		: base(doc)
	{
	}

	protected override void OnNew()
	{
		base.OnNew();
		_date.EditValue = Settings.HostingEnvironment.CurrentDate;
		_owner.Code = Settings.LoginService.UserName;
		ChangedCurrCodeOrDate();
		if (_viewMode == WindowViewMode.Copy)
		{
			return;
		}
		string text = Settings.ParametersService.DefaultStorage();
		if (!string.IsNullOrWhiteSpace(text) && TradeSettings.CreateTradeClient().CommonServices.IsStorageAvailable(text))
		{
			_storage.Code = text;
		}
		string text2 = Settings.ParametersService.DefaultSupplier();
		if (!string.IsNullOrWhiteSpace(text2))
		{
			PartnerCacheRow item = TradeSettings.TradeCacheService.Partner.GetItem(text2);
			if (item != null && item.Supplier)
			{
				_supplierContractContainer.PartnerCode.Code = text2;
			}
		}
	}

	protected override void OnNewAfterSave()
	{
		base.OnNewAfterSave();
		_mtIncomeRemList.Clear();
		_maxRowID = 0;
		CalcSummAndSummVAT();
		_owner.Code = Settings.LoginService.UserName;
		_barButtonJumpTo.IsEnabled = false;
	}

	protected override void OnEdit()
	{
		base.OnEdit();
		_setFocusOnActivation = _docNum;
		_supplierContractContainer.CalculatePartDebtRem();
	}

	protected override void LoadDataToControls()
	{
		base.LoadDataToControls();
		_mtIncomeRemList.Clear();
		foreach (MTIncomeRem.MTIncomeRemSpecificationRow mT in PO.MTList)
		{
			MTIncomeRemSpecificationRow item = new MTIncomeRemSpecificationRow(this)
			{
				Storage = mT.Storage,
				MTCode = mT.MTCode,
				RowNum = mT.RowNum,
				RowID = mT.RowID,
				MTID = mT.MTID,
				UnitCode = mT.UnitCode,
				Quantity = mT.Quantity,
				Price = mT.Price,
				Summa = mT.Summa,
				CurrPrice = mT.CurrPrice,
				CurrSumma = mT.CurrSumma,
				SalePrice = mT.SalePrice,
				SaleSumma = mT.SaleSumma,
				VATCodeForConvert = mT.VATCodeForConvert,
				VAT = mT.VAT,
				VATPrice = mT.VATPrice,
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
				Property2Caption = mT.Property2Caption
			};
			_mtIncomeRemList.Add(item);
		}
		_allowPurchaseWith0PriceOr100Percent = Settings.ParametersService.AllowPurchaseWith0PriceOr100Percent() || _mtIncomeRemList.Any((MTIncomeRemSpecificationRow row) => row.Summa == 0m);
		_totalSumma.IsRequired = !_allowPurchaseWith0PriceOr100Percent;
		_maxRowID = PO.MaxRowID;
		_mtIncomeRemGrid.ItemsSource = _mtIncomeRemList;
		if (_viewMode != WindowViewMode.NewCreate)
		{
			ChangedCurrCodeOrDate();
		}
		_supplierContractContainer.ValueChangedForLoadDoc();
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
		Ribbon.ToolbarItemLinks.Add(_barSubItemJournals.ConvertForToolbar("t_report_16.svg"));
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
		_barButtonJumpTo = new ReportBarButtonItem(DataProviderTypeSE.AllDocumentsJournal, showDialog: false, ArmSoft.SME.Common.Resources.JumpTo, null, "doc_JumpTo_16.svg", "doc_JumpTo_32.svg");
		_barButtonJumpTo.AddInBarSubItem(ref _ribbonPageGroupActions, ref _barSubItemActions);
		_barSubItemJournals.Content = "";
		_barSubItemJournals.Glyph = ImageService.GetImage("report_16.svg");
		_barSubItemJournals.LargeGlyph = ImageService.GetImage("report_32.svg");
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

	protected override void SetControlsReadonly()
	{
		base.SetControlsReadonly();
		if (_viewMode != WindowViewMode.View)
		{
			if (PO.CreateType != ((short)3).ToString())
			{
				_totalSumma.IsReadOnly = true;
				_vatSumma.IsReadOnly = true;
				_currSumma.IsReadOnly = true;
			}
			bool flag = State == DocumentStateSE.Draft.Value || State == DocumentStateSE.Deleted.Value;
			bool flag2 = _viewMode == WindowViewMode.Edit && !flag;
			_date.IsReadOnly = flag2;
			_storage.IsReadOnly = flag2;
			_barCode.IsReadOnly = flag2;
			_codeEnter.IsEnabled = !flag2;
			_mtIncomeRemGrid.IsReadOnly = flag2;
		}
	}

	private void MTIncomeRemGrid_DeletedRow(object sender, EventArgs e)
	{
		CalcSummAndSummVAT();
		_mtIncomeRemGrid.Grid.RefreshData();
	}

	private void MTIncomeRemGrid_AddingRow(object sender, DocumentGridAddingRowEventArgs e)
	{
		_mtIncomeRemList.Add(new MTIncomeRemSpecificationRow(this)
		{
			Storage = _storage.Code,
			RowID = _maxRowID
		});
		_maxRowID++;
		e.RowNumber = _mtIncomeRemList.Count - 1;
	}

	private void MTIncomeRemGrid_DuplicatingRow(object sender, DocumentGridDuplicatingRowEventArgs e)
	{
		MTIncomeRemSpecificationRow mTIncomeRemSpecificationRow = (MTIncomeRemSpecificationRow)_mtIncomeRemGrid.Grid.GetRow(e.RowHandle);
		if (mTIncomeRemSpecificationRow != null)
		{
			MTIncomeRemSpecificationRow mTIncomeRemSpecificationRow2 = new MTIncomeRemSpecificationRow(this);
			mTIncomeRemSpecificationRow.Clone(mTIncomeRemSpecificationRow2);
			mTIncomeRemSpecificationRow2.RowID = _maxRowID;
			_mtIncomeRemList.Add(mTIncomeRemSpecificationRow2);
			_maxRowID++;
			e.NewRowNumber = _mtIncomeRemList.Count - 1;
			_mtIncomeRemGrid.Grid.RefreshData();
			CalcSummAndSummVAT();
		}
	}

	private void MTIncomeRemGrid_MovingRowUp(object sender, DocumentGridMovingRowUpOrDownEventArgs e)
	{
		if (_mtIncomeRemGrid.Grid.SortInfo.Count > 0 || _mtIncomeRemTableView.Grid.VisibleRowCount != _mtIncomeRemList.Count)
		{
			throw new ASException(ArmSoft.SME.Common.Resources.NotAllowRowMoveInSortedGrid);
		}
		MTIncomeRemSpecificationRow mTIncomeRemSpecificationRow = (MTIncomeRemSpecificationRow)_mtIncomeRemGrid.Grid.GetRow(e.RowHandle);
		if (mTIncomeRemSpecificationRow != null)
		{
			_mtIncomeRemList.RemoveAt(e.RowHandle);
			_mtIncomeRemList.Insert(e.RowHandle - 1, mTIncomeRemSpecificationRow);
		}
		_mtIncomeRemGrid.Grid.RefreshData();
	}

	private void MTIncomeRemGrid_MovingRowDown(object sender, DocumentGridMovingRowUpOrDownEventArgs e)
	{
		if (_mtIncomeRemGrid.Grid.SortInfo.Count > 0 || _mtIncomeRemTableView.Grid.VisibleRowCount != _mtIncomeRemList.Count)
		{
			throw new ASException(ArmSoft.SME.Common.Resources.NotAllowRowMoveInSortedGrid);
		}
		MTIncomeRemSpecificationRow mTIncomeRemSpecificationRow = (MTIncomeRemSpecificationRow)_mtIncomeRemGrid.Grid.GetRow(e.RowHandle);
		if (mTIncomeRemSpecificationRow != null)
		{
			_mtIncomeRemList.RemoveAt(e.RowHandle);
			_mtIncomeRemList.Insert(e.RowHandle + 1, mTIncomeRemSpecificationRow);
		}
		_mtIncomeRemGrid.Grid.RefreshData();
	}

	private void MTIncomeRemGrid_AddingRowWithDialog(object sender, DocumentGridAddingRowEventArgs e)
	{
		MTIncomeRemRowAddEditDialog mTIncomeRemRowAddEditDialog = new MTIncomeRemRowAddEditDialog(DialogWindowMode.NewModeWithNext, this);
		mTIncomeRemRowAddEditDialog.Invoke();
		int addedRows = mTIncomeRemRowAddEditDialog.AddedRows;
		if (addedRows != 0)
		{
			_maxRowID += addedRows;
			_mtIncomeRemGrid.Grid.RefreshData();
			CalcSummAndSummVAT();
		}
	}

	private void MTIncomeRemGrid_DuplicatingRowWithDialog(object sender, DocumentGridDuplicatingRowEventArgs e)
	{
		MTIncomeRemSpecificationRow mTIncomeRemSpecificationRow = (MTIncomeRemSpecificationRow)_mtIncomeRemGrid.Grid.GetRow(e.RowHandle);
		if (mTIncomeRemSpecificationRow != null)
		{
			MTIncomeRemRowAddEditDialog mTIncomeRemRowAddEditDialog = new MTIncomeRemRowAddEditDialog(DialogWindowMode.NewModeWithNext, this, mTIncomeRemSpecificationRow);
			mTIncomeRemRowAddEditDialog.Invoke();
			int addedRows = mTIncomeRemRowAddEditDialog.AddedRows;
			if (addedRows != 0)
			{
				_maxRowID += addedRows;
				_mtIncomeRemGrid.Grid.RefreshData();
				CalcSummAndSummVAT();
			}
		}
	}

	private void MTIncomeRemGrid_EditingRowWithDialog(object sender, DocumentGridEditingRowEventArgs e)
	{
		MTIncomeRemSpecificationRow mTIncomeRemSpecificationRow = (MTIncomeRemSpecificationRow)_mtIncomeRemGrid.Grid.GetRow(e.RowHandle);
		if (mTIncomeRemSpecificationRow != null)
		{
			MTIncomeRemRowAddEditDialog mTIncomeRemRowAddEditDialog = new MTIncomeRemRowAddEditDialog(e.AllowEdit ? DialogWindowMode.EditMode : DialogWindowMode.ViewMode, this, mTIncomeRemSpecificationRow);
			e.RowEdited = Convert.ToBoolean(mTIncomeRemRowAddEditDialog.Invoke());
			if (e.AllowEdit && e.RowEdited)
			{
				_mtIncomeRemGrid.Grid.RefreshData();
				CalcSummAndSummVAT();
			}
		}
	}

	private void MTIncomeRemGrid_ClickDropDown(object sender, DocumentGridClickDropDownEventArgs e)
	{
		try
		{
			MTIncomeRemSpecificationRow mTIncomeRemSpecificationRow = (MTIncomeRemSpecificationRow)_mtIncomeRemGrid.Grid.GetRow(e.RowHandle);
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
				if (mTIncomeRemSpecificationRow == null)
				{
					return;
				}
				MaterialCacheRow item = TradeSettings.TradeCacheService.Material.GetItem(mTIncomeRemSpecificationRow.MTCode, throwExceptionNotExits: true);
				if (item == null || (string.IsNullOrWhiteSpace(item.Property1) && e.Column == _property1Col) || (string.IsNullOrWhiteSpace(item.Property2) && e.Column == _property2Col))
				{
					return;
				}
				PropertyList propertyList = new PropertyList((e.Column == _property1Col) ? item.Property1 : item.Property2);
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
						mTIncomeRemSpecificationRow.Property1 = propertyList.ValueMember(modalBrowser2.SelectedRowIndex);
					}
					else
					{
						mTIncomeRemSpecificationRow.Property2 = propertyList.ValueMember(modalBrowser2.SelectedRowIndex);
					}
				}
			}
			_mtIncomeRemGrid.Grid.RefreshData();
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	private void MTIncomeRemTableView_CellValueChanged(object sender, CellValueChangedEventArgs e)
	{
		MTIncomeRemSpecificationRow mTIncomeRemSpecificationRow = (MTIncomeRemSpecificationRow)_mtIncomeRemGrid.Grid.GetRow(e.RowHandle);
		if (mTIncomeRemSpecificationRow == null)
		{
			return;
		}
		string text = "";
		if (e.OldValue != null)
		{
			text = e.OldValue.ToString();
		}
		if (e.Column == _codeCol)
		{
			try
			{
				if (e.Value == null)
				{
					mTIncomeRemSpecificationRow.MTID = -1;
					mTIncomeRemSpecificationRow.MTName = "";
					mTIncomeRemSpecificationRow.UnitCode = "";
					mTIncomeRemSpecificationRow.UnitBrief = "";
					mTIncomeRemSpecificationRow.Quantity = 0m;
					mTIncomeRemSpecificationRow.Summa = 0m;
					mTIncomeRemSpecificationRow.Price = 0m;
					mTIncomeRemSpecificationRow.Country = "";
					mTIncomeRemSpecificationRow.Producer = "";
					mTIncomeRemSpecificationRow.Property1 = "";
					mTIncomeRemSpecificationRow.Property2 = "";
					mTIncomeRemSpecificationRow.Property1Caption = "";
					mTIncomeRemSpecificationRow.Property2Caption = "";
				}
				else
				{
					string text2 = e.Value.ToString();
					if (string.IsNullOrWhiteSpace(text2))
					{
						throw new Exception();
					}
					MaterialCacheRow item = TradeSettings.TradeCacheService.Material.GetItem(text2, throwExceptionNotExits: true);
					mTIncomeRemSpecificationRow.MTID = item.Id;
					mTIncomeRemSpecificationRow.MTName = item.Caption;
					mTIncomeRemSpecificationRow.UnitCode = item.Unit;
					mTIncomeRemSpecificationRow.UnitBrief = item.UnitRow.Brief;
					_unitCodeCol.DataView = new MTQntUnits(item.MTCode);
					mTIncomeRemSpecificationRow.VAT = item.VAT;
					mTIncomeRemSpecificationRow.Country = item.Country;
					mTIncomeRemSpecificationRow.Producer = item.Producer;
					mTIncomeRemSpecificationRow.Property1 = "";
					mTIncomeRemSpecificationRow.Property2 = "";
					mTIncomeRemSpecificationRow.Property1Caption = "";
					mTIncomeRemSpecificationRow.Property2Caption = "";
					_property1Col.DataView = new PropertyList(item.Property1);
					_property2Col.DataView = new PropertyList(item.Property2);
				}
			}
			catch (Exception ex)
			{
				mTIncomeRemSpecificationRow.MTName = "";
				mTIncomeRemSpecificationRow.MTID = -1;
				mTIncomeRemSpecificationRow.UnitCode = "";
				mTIncomeRemSpecificationRow.UnitBrief = "";
				mTIncomeRemSpecificationRow.Quantity = 0m;
				mTIncomeRemSpecificationRow.Summa = 0m;
				mTIncomeRemSpecificationRow.Price = 0m;
				mTIncomeRemSpecificationRow.VAT = false;
				mTIncomeRemSpecificationRow.Country = "";
				mTIncomeRemSpecificationRow.Producer = "";
				mTIncomeRemSpecificationRow.Property1 = "";
				mTIncomeRemSpecificationRow.Property2 = "";
				mTIncomeRemSpecificationRow.Property1Caption = "";
				mTIncomeRemSpecificationRow.Property2Caption = "";
				if (e.Value != null && !string.IsNullOrWhiteSpace(e.Value.ToString()))
				{
					Settings.ErrorHandler(ex);
				}
			}
		}
		else if (e.Column == _storageCol)
		{
			if (e.Value != null && !string.IsNullOrWhiteSpace(e.Value.ToString()))
			{
				if (!TradeSettings.CreateTradeClient().CommonServices.IsStorageAvailable(e.Value.ToString()))
				{
					ArmSoft.SME.Core.UI.MessageBox.Show(string.Format(ArmSoft.SME.Trade.Common.Resources.StorageNotAvailForUser, Settings.LoginService.UserName, e.Value.ToString()), Settings.ProgramName, MessageBoxButton.OK, MessageBoxImage.Exclamation);
					mTIncomeRemSpecificationRow.Storage = text;
				}
				else
				{
					StoragesCacheRow item2 = TradeSettings.TradeCacheService.Storage.GetItem(e.Value.ToString());
					if (item2 == null || !item2.Retail)
					{
						mTIncomeRemSpecificationRow.SalePrice = 0m;
						mTIncomeRemSpecificationRow.SaleSumma = 0m;
					}
				}
			}
		}
		else if (e.Column == _unitCodeCol)
		{
			try
			{
				if (e.Value != null && !string.IsNullOrWhiteSpace(e.Value.ToString()))
				{
					QntUnitCacheRow item3 = TradeSettings.TradeCacheService.QntUnit.GetItem(e.Value.ToString(), throwExceptionNotExits: true);
					if (item3 != null)
					{
						mTIncomeRemSpecificationRow.UnitCode = e.Value.ToString();
						mTIncomeRemSpecificationRow.UnitBrief = item3.Brief;
						string text3 = text;
						if (TradeSettings.TradeCacheService.QntUnit.GetItem(text3) != null)
						{
							mTIncomeRemSpecificationRow.Price = PriceHelper.GetPriceForUnit(text3, mTIncomeRemSpecificationRow.UnitCode, mTIncomeRemSpecificationRow.Price, mTIncomeRemSpecificationRow.MTCode);
							mTIncomeRemSpecificationRow.Summa = (mTIncomeRemSpecificationRow.Price * mTIncomeRemSpecificationRow.Quantity).RoundAs(2);
							mTIncomeRemSpecificationRow.VATPrice = PriceHelper.GetPriceForUnit(text3, mTIncomeRemSpecificationRow.UnitCode, mTIncomeRemSpecificationRow.VATPrice, mTIncomeRemSpecificationRow.MTCode);
							UpdateVATSumm(mTIncomeRemSpecificationRow);
							mTIncomeRemSpecificationRow.CurrPrice = PriceHelper.GetPriceForUnit(text3, mTIncomeRemSpecificationRow.UnitCode, mTIncomeRemSpecificationRow.CurrPrice, mTIncomeRemSpecificationRow.MTCode);
							UpdateCurrSumm(mTIncomeRemSpecificationRow);
							mTIncomeRemSpecificationRow.SalePrice = PriceHelper.GetPriceForUnit(text3, mTIncomeRemSpecificationRow.UnitCode, mTIncomeRemSpecificationRow.SalePrice, mTIncomeRemSpecificationRow.MTCode);
							UpdateSaleSumm(mTIncomeRemSpecificationRow);
						}
					}
					else
					{
						mTIncomeRemSpecificationRow.UnitBrief = "";
					}
				}
				else
				{
					mTIncomeRemSpecificationRow.UnitBrief = "";
				}
			}
			catch (Exception ex2)
			{
				Settings.ErrorHandler(ex2);
			}
		}
		else if (e.Column == _quantityCol)
		{
			mTIncomeRemSpecificationRow.Summa = (Convert.ToDecimal(e.Value) * mTIncomeRemSpecificationRow.Price).RoundAs(2);
			UpdateVAT_Curr_Sale_Summs(mTIncomeRemSpecificationRow);
		}
		else if (e.Column == _priceCol)
		{
			mTIncomeRemSpecificationRow.Summa = (Convert.ToDecimal(e.Value) * mTIncomeRemSpecificationRow.Quantity).RoundAs(2);
			UpdateVATPrice(mTIncomeRemSpecificationRow);
		}
		else if (e.Column == _summaCol && mTIncomeRemSpecificationRow.Quantity != 0m)
		{
			mTIncomeRemSpecificationRow.Price = (Convert.ToDecimal(e.Value) / mTIncomeRemSpecificationRow.Quantity).RoundAs(2);
			UpdateVATPrice(mTIncomeRemSpecificationRow);
		}
		else if (e.Column == _vatCol)
		{
			UpdateVATPrice(mTIncomeRemSpecificationRow);
		}
		else if (e.Column == _vatPriceCol)
		{
			UpdateVATSumm(mTIncomeRemSpecificationRow);
			UpdateCurrPriceSumma(mTIncomeRemSpecificationRow);
		}
		else if (e.Column == _vatSummaCol && mTIncomeRemSpecificationRow.Quantity != 0m)
		{
			mTIncomeRemSpecificationRow.VATPrice = (Convert.ToDecimal(e.Value) / mTIncomeRemSpecificationRow.Quantity).RoundAs(2);
			UpdateCurrPriceSumma(mTIncomeRemSpecificationRow);
		}
		else if (e.Column == _currPriceCol)
		{
			UpdateCurrSumm(mTIncomeRemSpecificationRow);
		}
		else if (e.Column == _currSummaCol && mTIncomeRemSpecificationRow.Quantity != 0m)
		{
			mTIncomeRemSpecificationRow.CurrPrice = (Convert.ToDecimal(e.Value) / mTIncomeRemSpecificationRow.Quantity).RoundAs(2);
		}
		else if (e.Column == _salePriceCol)
		{
			UpdateSaleSumm(mTIncomeRemSpecificationRow);
		}
		else if (e.Column == _saleSummaCol && mTIncomeRemSpecificationRow.Quantity != 0m)
		{
			mTIncomeRemSpecificationRow.SalePrice = (Convert.ToDecimal(e.Value) / mTIncomeRemSpecificationRow.Quantity).RoundAs(2);
		}
		_mtIncomeRemGrid.Grid.RefreshData();
		CalcSummAndSummVAT();
	}

	private void MTIncomeRemTableView_FocusedRowHandleChangedEventArgs(object sender, FocusedRowHandleChangedEventArgs e)
	{
		int focusedRowHandle = _mtIncomeRemTableView.FocusedRowHandle;
		MTIncomeRemSpecificationRow mTIncomeRemSpecificationRow = (MTIncomeRemSpecificationRow)_mtIncomeRemGrid.Grid.GetRow(focusedRowHandle);
		if (mTIncomeRemSpecificationRow != null)
		{
			ChangeColumnAdditonalView(mTIncomeRemSpecificationRow.MTCode);
		}
	}

	private void ChangeColumnAdditonalView(string MTCode)
	{
		_unitCodeCol.DataView = new MTQntUnits(MTCode);
	}

	private void MTIncomeRemTableView_ShowingEditor(object sender, ShowingEditorEventArgs e)
	{
		if (e.Row == null)
		{
			return;
		}
		if (PO.CreateType != ((short)3).ToString() && ((_property1Col.Visible && e.Column == _property1Col) || (_property2Col.Visible && e.Column == _property2Col)))
		{
			MTIncomeRemSpecificationRow mTIncomeRemSpecificationRow = (MTIncomeRemSpecificationRow)e.Row;
			MaterialCacheRow item = TradeSettings.TradeCacheService.Material.GetItem(mTIncomeRemSpecificationRow.MTCode);
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
		else if (e.Column == _salePriceCol || e.Column == _saleSummaCol)
		{
			MTIncomeRemSpecificationRow mTIncomeRemSpecificationRow2 = (MTIncomeRemSpecificationRow)e.Row;
			StoragesCacheRow item2 = TradeSettings.TradeCacheService.Storage.GetItem(mTIncomeRemSpecificationRow2.Storage);
			if (item2 == null || !item2.Retail)
			{
				e.Cancel = true;
			}
		}
	}

	private void UpdateVAT_Curr_Sale_Summs(MTIncomeRemSpecificationRow MTIncGridRow)
	{
		UpdateVATSumm(MTIncGridRow);
		UpdateCurrSumm(MTIncGridRow);
		UpdateSaleSumm(MTIncGridRow);
	}

	private void UpdateVATSumm(MTIncomeRemSpecificationRow MTIncGridRow)
	{
		MTIncGridRow.VATSumma = (MTIncGridRow.VATPrice * MTIncGridRow.Quantity).RoundAs(2);
	}

	private void UpdateCurrSumm(MTIncomeRemSpecificationRow MTIncGridRow)
	{
		MTIncGridRow.CurrSumma = (MTIncGridRow.CurrPrice * MTIncGridRow.Quantity).RoundAs(2);
	}

	private void UpdateSaleSumm(MTIncomeRemSpecificationRow MTIncGridRow)
	{
		MTIncGridRow.SaleSumma = (MTIncGridRow.SalePrice * MTIncGridRow.Quantity).RoundAs(2);
	}

	private void UpdateVATPrice(MTIncomeRemSpecificationRow MTIncGridRow)
	{
		if (MTIncGridRow.VAT)
		{
			MTIncGridRow.VATPrice = (MTIncGridRow.Price * (100m + Settings.ParametersService.VAT()) / 100m).RoundAs(2);
		}
		else
		{
			MTIncGridRow.VATPrice = MTIncGridRow.Price;
		}
		UpdateVATSumm(MTIncGridRow);
		UpdateCurrPriceSumma(MTIncGridRow);
	}

	private void UpdateCurrPriceSumma(MTIncomeRemSpecificationRow MTIncGridRow)
	{
		if (_docCurrExChngRate != 0m)
		{
			MTIncGridRow.CurrPrice = (MTIncGridRow.VATPrice * _docCurrExChngBase / _docCurrExChngRate).RoundAs(2);
			UpdateCurrSumm(MTIncGridRow);
		}
	}

	private void CalcSummAndSummVAT()
	{
		if (!(PO.CreateType != ((short)3).ToString()))
		{
			return;
		}
		decimal num = 0m;
		decimal num2 = 0m;
		decimal num3 = 0m;
		foreach (MTIncomeRemSpecificationRow mtIncomeRem in _mtIncomeRemList)
		{
			num += mtIncomeRem.Summa;
			num2 += mtIncomeRem.CurrSumma;
			num3 += mtIncomeRem.VATSumma;
		}
		_totalSumma.EditValue = num;
		_currSumma.EditValue = num2;
		_vatSumma.EditValue = num3;
	}

	private void ChangedCurrCodeOrDate()
	{
		if (string.IsNullOrWhiteSpace(Settings.ParametersService.MTCurr()) || Settings.ParametersService.MTCurr() == "AMD")
		{
			return;
		}
		try
		{
			if (_date.DoValidate())
			{
				ExchangeRate exchangeRates = TradeSettings.CreateTradeClient().CommonServices.GetExchangeRates(Settings.ParametersService.MTCurr(), _date.EditValue.Value);
				_docCurrExChngRate = exchangeRates.Rate;
				_docCurrExChngBase = exchangeRates.Base;
				return;
			}
			throw new ASException(ArmSoft.SME.Common.Resources.ValidateEmptyDate);
		}
		catch (Exception ex)
		{
			if (IsLoadingDataToControls)
			{
				Settings.ErrorHandler(ex);
			}
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
				GoodsFilter goodsFilter = new GoodsFilter(shortMode: true, ItemTypesSE.Goods);
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
			AddMTToList(item);
			if (_tabs.SelectedIndex != _priceListTabControlIndex)
			{
				_tabs.SelectedContainer = (DXTabItem)_tabs.Items[_priceListTabControlIndex];
			}
			_barCode.Text = "";
			CalcSummAndSummVAT();
			if (PO.CreateType != ((short)3).ToString())
			{
				_vatSumma.DoValidate();
			}
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	private void BE_FilteredSearch_Click(object sender, RoutedEventArgs e)
	{
		try
		{
			FilteredSearchWindow filteredSearchWindow = new FilteredSearchWindow(new FilteredSearch
			{
				FilterDataView = new GoodsGroups(),
				SearchDataView = new GoodsFilter(shortMode: true, ItemTypesSE.Goods),
				ResultsList = new MTIncomeRemFilteredSearchList(this)
			}, this, isBasketMode: true);
			if (!filteredSearchWindow.ShowDialog().Value)
			{
				return;
			}
			foreach (MTIncomeRemSpecificationRow item in filteredSearchWindow.BasketResult)
			{
				item.Storage = _storage.Code;
				item.RowID = _maxRowID;
				_mtIncomeRemList.Add(item);
				_maxRowID++;
			}
			_mtIncomeRemGrid.Grid.RefreshData();
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
			MTIncomeRemSpecificationRow mTIncomeRemSpecificationRow = (MTIncomeRemSpecificationRow)_mtIncomeRemTableView.Grid.CurrentItem;
			if (mTIncomeRemSpecificationRow != null)
			{
				new MTAvailAndTurnWindow(DialogWindowMode.ViewMode, this, _storage.Code, _supplierContractContainer.PartnerCode.Code, mTIncomeRemSpecificationRow.MTID, mTIncomeRemSpecificationRow.MTCode, mTIncomeRemSpecificationRow.UnitCode).Invoke();
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

	private void MTIncomeRemGrid_RefreshButtons(object sender, EventArgs e)
	{
		_viewAvailAndTurn.IsEnabled = Settings.UserAccessService.GetProviderPermission(DataProviderTypeSE.MatRepAvailability).AllowView || Settings.UserAccessService.GetProviderPermission(DataProviderTypeSE.MatRepAvailabilityTreeView).AllowView || Settings.UserAccessService.GetProviderPermission(DataProviderTypeSE.GoodsRepOperations).AllowView || Settings.UserAccessService.GetProviderPermission(DataProviderTypeSE.MatRepSalesAnalysis).AllowView;
		_filteredSearch.IsEnabled = !_mtIncomeRemGrid.IsReadOnly && _mtIncomeRemGrid.AllowAdd;
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
			MaterialCacheRow materialCacheRow = DocumentHelpers.TryGetItemByBarCode(ref barCode, out qty, out unitCode, out unitBrief);
			if (materialCacheRow == null)
			{
				ControlsHelper.SetFocus(_barCode);
				if (ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Common.Resources.FindeInUnitedProducts, Settings.ProgramName, MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No) == MessageBoxResult.Yes)
				{
					string text = DocumentHelpers.CreateNewGoodFromUnitedProdAndAddInDoc(barCode, this);
					materialCacheRow = TradeSettings.TradeCacheService.Material.GetItem(text);
					if (!string.IsNullOrWhiteSpace(text))
					{
						AddMTToList(materialCacheRow);
						_barCode.Text = "";
						CalcSummAndSummVAT();
						_vatSumma.DoValidate();
					}
				}
				return;
			}
			if (materialCacheRow.Type == ItemTypesSE.Services.Value)
			{
				ControlsHelper.SetFocus(_barCode);
				throw new ASException(string.Format(ArmSoft.SME.Common.Resources.ErrBarCodeNotExist, _barCode.Text.Trim()));
			}
			if (_tabs.SelectedIndex != _priceListTabControlIndex)
			{
				_tabs.SelectedContainer = (DXTabItem)_tabs.Items[_priceListTabControlIndex];
			}
			AddMTToList(materialCacheRow, unitCode, unitBrief, qty);
			_barCode.Text = "";
			CalcSummAndSummVAT();
			if (PO.CreateType != ((short)3).ToString())
			{
				_vatSumma.DoValidate();
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

	private void AddMTToList(MaterialCacheRow goodsRow, string unitCode = "", string unitBrief = "", decimal quantity = 1m)
	{
		if (Settings.ParametersService.ScannerMode() == "3")
		{
			AddNewRowInner(goodsRow, string.IsNullOrWhiteSpace(unitCode) ? goodsRow.Unit : unitCode, string.IsNullOrWhiteSpace(unitBrief) ? goodsRow.UnitRow.Brief : unitBrief, quantity);
		}
		else
		{
			AddNewRowOrMergeInner(goodsRow, Settings.ParametersService.ScannerMode() == "2", string.IsNullOrWhiteSpace(unitCode) ? goodsRow.Unit : unitCode, string.IsNullOrWhiteSpace(unitBrief) ? goodsRow.UnitRow.Brief : unitBrief, quantity);
		}
	}

	private void AddNewRowOrMergeInner(MaterialCacheRow goodsRow, bool forFocusedRow, string unitCode, string unitBrief, decimal quantity = 1m)
	{
		MTIncomeRemSpecificationRow mTIncomeRemSpecificationRow = null;
		if (forFocusedRow && _mtIncomeRemList.Count != 0)
		{
			mTIncomeRemSpecificationRow = (MTIncomeRemSpecificationRow)_mtIncomeRemTableView.Grid.CurrentItem;
			if (mTIncomeRemSpecificationRow == null)
			{
				return;
			}
		}
		for (int i = 0; i <= _mtIncomeRemList.Count - 1; i++)
		{
			if ((!forFocusedRow || mTIncomeRemSpecificationRow.RowID == _mtIncomeRemList[i].RowID) && goodsRow.Id == _mtIncomeRemList[i].MTID && unitCode == _mtIncomeRemList[i].UnitCode)
			{
				FillMTIncGridRow(_mtIncomeRemList[i], goodsRow, unitCode, unitBrief, quantity);
				int rowHandleByListIndex = _mtIncomeRemGrid.Grid.GetRowHandleByListIndex(i);
				_mtIncomeRemGrid.Grid.RefreshRow(rowHandleByListIndex);
				POWindow<MTIncomeRem>.SelectCellInGrid(_mtIncomeRemTableView, _mtIncomeRemGrid, rowHandleByListIndex, "Quantity");
				return;
			}
		}
		AddNewRowInner(goodsRow, unitCode, unitBrief, quantity);
	}

	private void FillMTIncGridRow(MTIncomeRemSpecificationRow mtIncGridRow, MaterialCacheRow goodsRow, string unitCode, string unitBrief, decimal quantity = 1m)
	{
		mtIncGridRow.Storage = _storage.Code;
		mtIncGridRow.UnitCode = unitCode;
		mtIncGridRow.UnitBrief = unitBrief;
		if (goodsRow != null)
		{
			mtIncGridRow.MTCode = goodsRow.MTCode;
			mtIncGridRow.MTID = goodsRow.Id;
			mtIncGridRow.MTName = goodsRow.Caption;
			mtIncGridRow.VAT = goodsRow.VAT;
			mtIncGridRow.Country = goodsRow.Country;
			mtIncGridRow.Producer = goodsRow.Producer;
		}
		mtIncGridRow.Quantity += quantity;
		mtIncGridRow.Summa = (mtIncGridRow.Quantity * mtIncGridRow.Price).RoundAs(2);
		UpdateVAT_Curr_Sale_Summs(mtIncGridRow);
	}

	private void AddNewRowInner(MaterialCacheRow goodsRow, string unitCode, string unitBrief, decimal quantity = 1m)
	{
		MTIncomeRemSpecificationRow mTIncomeRemSpecificationRow = new MTIncomeRemSpecificationRow(this);
		FillMTIncGridRow(mTIncomeRemSpecificationRow, goodsRow, unitCode, unitBrief, quantity);
		mTIncomeRemSpecificationRow.RowID = _maxRowID;
		_mtIncomeRemList.Add(mTIncomeRemSpecificationRow);
		_maxRowID++;
		int rowHandleByListIndex = _mtIncomeRemGrid.Grid.GetRowHandleByListIndex(_mtIncomeRemList.Count - 1);
		_mtIncomeRemGrid.Grid.RefreshRow(rowHandleByListIndex);
		POWindow<MTIncomeRem>.SelectCellInGrid(_mtIncomeRemTableView, _mtIncomeRemGrid, rowHandleByListIndex, "Quantity");
		_mtIncomeRemGrid.ConfigureButtons();
	}

	private void Storage_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		if (!_storage.IsReadOnly && !string.IsNullOrWhiteSpace(Convert.ToString(e.NewValue)) && TradeSettings.TradeCacheService.Storage.GetItem(Convert.ToString(e.NewValue)) != null && !TradeSettings.CreateTradeClient().CommonServices.IsStorageAvailable(Convert.ToString(e.NewValue)))
		{
			e.Handled = true;
			_storage.Code = Convert.ToString(e.OldValue);
			ArmSoft.SME.Core.UI.MessageBox.Show(string.Format(ArmSoft.SME.Trade.Common.Resources.StorageNotAvailForUser, Settings.LoginService.UserName, Convert.ToString(e.NewValue)), Settings.ProgramName, MessageBoxButton.OK, MessageBoxImage.Exclamation);
		}
	}

	private void Date_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		if (!_date.DoValidate() || !_date.EditValue.HasValue)
		{
			ControlsHelper.SetFocus(_date);
			ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Common.Resources.ValidateEmptyDate, Settings.ProgramName, MessageBoxButton.OK, MessageBoxImage.Exclamation);
		}
	}

	protected override void SetReportBarButtonsEnable(bool isEnabled)
	{
		base.SetReportBarButtonsEnable(isEnabled);
		_barButtonJumpTo.IsEnabled = isEnabled;
		_barButtonGoodsOperationsJournal.IsEnabled = isEnabled;
		_barButtonGoodsRetailOperationJournal.IsEnabled = isEnabled;
	}

	protected override void FillDocument(StoreRequest sr)
	{
		base.FillDocument(sr);
		PO.MTList.Clear();
		foreach (MTIncomeRemSpecificationRow mtIncomeRem in _mtIncomeRemList)
		{
			MTIncomeRem.MTIncomeRemSpecificationRow mTIncomeRemSpecificationRow = new MTIncomeRem.MTIncomeRemSpecificationRow();
			mTIncomeRemSpecificationRow.Storage = mtIncomeRem.Storage;
			mTIncomeRemSpecificationRow.MTCode = mtIncomeRem.MTCode;
			mTIncomeRemSpecificationRow.MTID = mtIncomeRem.MTID;
			mTIncomeRemSpecificationRow.UnitCode = mtIncomeRem.UnitCode;
			mTIncomeRemSpecificationRow.Quantity = mtIncomeRem.Quantity;
			mTIncomeRemSpecificationRow.Price = mtIncomeRem.Price;
			mTIncomeRemSpecificationRow.Summa = mtIncomeRem.Summa;
			mTIncomeRemSpecificationRow.SalePrice = mtIncomeRem.SalePrice;
			mTIncomeRemSpecificationRow.SaleSumma = mtIncomeRem.SaleSumma;
			mTIncomeRemSpecificationRow.VATCodeForConvert = mtIncomeRem.VATCodeForConvert;
			mTIncomeRemSpecificationRow.VAT = mtIncomeRem.VAT;
			mTIncomeRemSpecificationRow.VATPrice = mtIncomeRem.VATPrice;
			mTIncomeRemSpecificationRow.VATSumma = mtIncomeRem.VATSumma;
			mTIncomeRemSpecificationRow.CurrPrice = mtIncomeRem.CurrPrice;
			mTIncomeRemSpecificationRow.CurrSumma = mtIncomeRem.CurrSumma;
			mTIncomeRemSpecificationRow.RowID = mtIncomeRem.RowID;
			mTIncomeRemSpecificationRow.RowNum = mtIncomeRem.RowNum;
			mTIncomeRemSpecificationRow.UseLife = mtIncomeRem.UseLife;
			mTIncomeRemSpecificationRow.Country = mtIncomeRem.Country;
			mTIncomeRemSpecificationRow.Producer = mtIncomeRem.Producer;
			mTIncomeRemSpecificationRow.Property1 = mtIncomeRem.Property1;
			mTIncomeRemSpecificationRow.Property2 = mtIncomeRem.Property2;
			mTIncomeRemSpecificationRow.ProducerCaption = mtIncomeRem.ProducerCaption;
			mTIncomeRemSpecificationRow.Property1Caption = mtIncomeRem.Property1Caption;
			mTIncomeRemSpecificationRow.Property2Caption = mtIncomeRem.Property2Caption;
			mTIncomeRemSpecificationRow.MTName = mtIncomeRem.MTName;
			mTIncomeRemSpecificationRow.FullName = mtIncomeRem.FullName;
			mTIncomeRemSpecificationRow.UnitBrief = mtIncomeRem.UnitBrief;
			PO.MTList.Add(mTIncomeRemSpecificationRow);
		}
		PO.MaxRowID = _maxRowID;
	}

	protected override void StoreSettings(RegistryKey rk)
	{
		base.StoreSettings(rk);
		_mtIncomeRemGrid.StoreSettings(rk);
	}

	protected override void RestoreSettings(double defaultWidth, double defaultHeight, RegistryKey rk)
	{
		base.RestoreSettings(defaultWidth, defaultHeight, rk);
		_mtIncomeRemGrid.RestoreSettings(rk);
	}

	protected override bool AllowChangeGridMode()
	{
		return true;
	}
}
