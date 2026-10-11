using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ArmSoft.SME.Common;
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
using ArmSoft.SME.Trade.Common;
using ArmSoft.SME.Trade.Common.Documents.Definition;
using ArmSoft.SME.Trade.Common.Models.Caches;
using ArmSoft.SME.Trade.Common.Models.Documents;
using ArmSoft.SME.Trade.Common.Parameters;
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

[DocumentWindow(12)]
public class MTDecomplectationWindow : TradeDocumentWindow<MTDecomplectation>
{
	public class MTDecomplectationSpecificationRow : DocumentGridRow
	{
		[ObservableProperty]
		private int itemID = -1;

		[ObservableProperty]
		private string itemCode = string.Empty;

		[ObservableProperty]
		private string itemName = string.Empty;

		[ObservableProperty]
		private string unitCode = string.Empty;

		[ObservableProperty]
		private string unitBrief = string.Empty;

		[ObservableProperty]
		private decimal quantity;

		[ObservableProperty]
		private decimal pricePart;

		[ObservableProperty]
		private decimal summaPart;

		[ObservableProperty]
		private DateOnly? useLife;

		[ObservableProperty]
		private string country = string.Empty;

		[ObservableProperty]
		private string producer = string.Empty;

		[ObservableProperty]
		private string producerCaption = string.Empty;

		[ObservableProperty]
		private string property1 = string.Empty;

		[ObservableProperty]
		private string property1Caption = string.Empty;

		[ObservableProperty]
		private string property2 = string.Empty;

		[ObservableProperty]
		private string property2Caption = string.Empty;

		[ObservableProperty]
		private int rowID;

		[ObservableProperty]
		private short rowNum;

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
					OnItemNameChanged(value);
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
					OnUnitBriefChanged(value);
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
		public decimal PricePart
		{
			get
			{
				return pricePart;
			}
			set
			{
				if (!EqualityComparer<decimal>.Default.Equals(pricePart, value))
				{
					OnPropertyChanging(__KnownINotifyPropertyChangingArgs.PricePart);
					pricePart = value;
					OnPricePartChanged(value);
					OnPropertyChanged(__KnownINotifyPropertyChangedArgs.PricePart);
				}
			}
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public decimal SummaPart
		{
			get
			{
				return summaPart;
			}
			set
			{
				if (!EqualityComparer<decimal>.Default.Equals(summaPart, value))
				{
					OnPropertyChanging(__KnownINotifyPropertyChangingArgs.SummaPart);
					summaPart = value;
					OnSummaPartChanged(value);
					OnPropertyChanged(__KnownINotifyPropertyChangedArgs.SummaPart);
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

		static MTDecomplectationSpecificationRow()
		{
			DocumentGridRow.RegisterPropertyValidators(typeof(MTDecomplectationSpecificationRow), cacheValidationResults: true, new Dictionary<string, Action<DocumentGridRow, DevExpress.XtraEditors.DXErrorProvider.ErrorInfo>>
			{
				{
					"ItemCode",
					(DocumentGridRow row, DevExpress.XtraEditors.DXErrorProvider.ErrorInfo info) =>
					{
						ValidateGoodCode((MTDecomplectationSpecificationRow)row, info);
					}
				},
				{
					"UnitCode",
					(DocumentGridRow row, DevExpress.XtraEditors.DXErrorProvider.ErrorInfo info) =>
					{
						ValidateUnitCode((MTDecomplectationSpecificationRow)row, info);
					}
				},
				{
					"Quantity",
					(DocumentGridRow row, DevExpress.XtraEditors.DXErrorProvider.ErrorInfo info) =>
					{
						ValidateQuantity((MTDecomplectationSpecificationRow)row, info);
					}
				},
				{
					"PricePart",
					(DocumentGridRow row, DevExpress.XtraEditors.DXErrorProvider.ErrorInfo info) =>
					{
						ValidatePricePart((MTDecomplectationSpecificationRow)row, info);
					}
				},
				{
					"SummaPart",
					(DocumentGridRow row, DevExpress.XtraEditors.DXErrorProvider.ErrorInfo info) =>
					{
						ValidateSummaPart((MTDecomplectationSpecificationRow)row, info);
					}
				},
				{
					"Country",
					(DocumentGridRow row, DevExpress.XtraEditors.DXErrorProvider.ErrorInfo info) =>
					{
						ValidateCountry((MTDecomplectationSpecificationRow)row, info);
					}
				},
				{
					"Producer",
					(DocumentGridRow row, DevExpress.XtraEditors.DXErrorProvider.ErrorInfo info) =>
					{
						ValidateProducer((MTDecomplectationSpecificationRow)row, info);
					}
				},
				{
					"Property1",
					(DocumentGridRow row, DevExpress.XtraEditors.DXErrorProvider.ErrorInfo info) =>
					{
						ValidateProperty1((MTDecomplectationSpecificationRow)row, info);
					}
				},
				{
					"Property2",
					(DocumentGridRow row, DevExpress.XtraEditors.DXErrorProvider.ErrorInfo info) =>
					{
						ValidateProperty2((MTDecomplectationSpecificationRow)row, info);
					}
				},
				{
					"Property1Caption",
					(DocumentGridRow row, DevExpress.XtraEditors.DXErrorProvider.ErrorInfo info) =>
					{
						ValidateProperty1Caption((MTDecomplectationSpecificationRow)row, info);
					}
				},
				{
					"Property2Caption",
					(DocumentGridRow row, DevExpress.XtraEditors.DXErrorProvider.ErrorInfo info) =>
					{
						ValidateProperty2Caption((MTDecomplectationSpecificationRow)row, info);
					}
				}
			});
		}

		private static void ValidateGoodCode(MTDecomplectationSpecificationRow row, DevExpress.XtraEditors.DXErrorProvider.ErrorInfo info)
		{
			TradePropertyValidator.ValidateMaterialCode(row.ItemCode, info, TradeAllDocDefinitions.MTDecomplectation.Grids["ComplectPartsList"].Columns["ItemCode"]);
		}

		private static void ValidateUnitCode(MTDecomplectationSpecificationRow row, DevExpress.XtraEditors.DXErrorProvider.ErrorInfo info)
		{
			TradePropertyValidator.ValidateGoodUnitCode(row.UnitCode, row.ItemCode, info, TradeAllDocDefinitions.MTDecomplectation.Grids["ComplectPartsList"].Columns["UnitCode"]);
		}

		private static void ValidateQuantity(MTDecomplectationSpecificationRow row, DevExpress.XtraEditors.DXErrorProvider.ErrorInfo info)
		{
			PropertyValidator.PositiveValidate(row.Quantity, info, ArmSoft.SME.Common.Resources.Quantity);
		}

		private static void ValidatePricePart(MTDecomplectationSpecificationRow row, DevExpress.XtraEditors.DXErrorProvider.ErrorInfo info)
		{
			PropertyValidator.PositiveValidate(row.PricePart, info, ArmSoft.SME.Trade.Common.Resources.SummaPart);
		}

		private static void ValidateSummaPart(MTDecomplectationSpecificationRow row, DevExpress.XtraEditors.DXErrorProvider.ErrorInfo info)
		{
			PropertyValidator.NonNegativeValidate(row.SummaPart, info, ArmSoft.SME.Trade.Common.Resources.SummPart);
		}

		private static void ValidateCountry(MTDecomplectationSpecificationRow row, DevExpress.XtraEditors.DXErrorProvider.ErrorInfo info)
		{
			if (!string.IsNullOrWhiteSpace(row.Country))
			{
				TradePropertyValidator.ValidateCountry(row.Country, info, TradeAllDocDefinitions.MTDecomplectation.Grids["ComplectPartsList"].Columns["Country"]);
			}
		}

		private static void ValidateProducer(MTDecomplectationSpecificationRow row, DevExpress.XtraEditors.DXErrorProvider.ErrorInfo info)
		{
			if (!string.IsNullOrWhiteSpace(row.Producer))
			{
				TradePropertyValidator.ValidateProducer(row.Producer, info, TradeAllDocDefinitions.MTDecomplectation.Grids["ComplectPartsList"].Columns["Producer"]);
			}
		}

		private static void ValidateProperty1(MTDecomplectationSpecificationRow row, DevExpress.XtraEditors.DXErrorProvider.ErrorInfo info)
		{
			if (!string.IsNullOrWhiteSpace(row.Property1))
			{
				TradePropertyValidator.ValidateProperty(row.Property1, row.ItemCode, isProperty1: true, info, TradeAllDocDefinitions.MTDecomplectation.Grids["ComplectPartsList"].Columns["Property1"]);
			}
		}

		private static void ValidateProperty2(MTDecomplectationSpecificationRow row, DevExpress.XtraEditors.DXErrorProvider.ErrorInfo info)
		{
			if (!string.IsNullOrWhiteSpace(row.Property2))
			{
				TradePropertyValidator.ValidateProperty(row.Property2, row.ItemCode, isProperty1: false, info, TradeAllDocDefinitions.MTDecomplectation.Grids["ComplectPartsList"].Columns["Property2"]);
			}
		}

		private static void ValidateProperty1Caption(MTDecomplectationSpecificationRow row, DevExpress.XtraEditors.DXErrorProvider.ErrorInfo info)
		{
			if (!string.IsNullOrWhiteSpace(row.Property1Caption))
			{
				TradePropertyValidator.ValidatePropertyValue(row.Property1Caption, row.ItemCode, isProperty1: true, info, TradeAllDocDefinitions.MTDecomplectation.Grids["ComplectPartsList"].Columns["Property1Caption"]);
			}
		}

		private static void ValidateProperty2Caption(MTDecomplectationSpecificationRow row, DevExpress.XtraEditors.DXErrorProvider.ErrorInfo info)
		{
			if (!string.IsNullOrWhiteSpace(row.Property2Caption))
			{
				TradePropertyValidator.ValidatePropertyValue(row.Property2Caption, row.ItemCode, isProperty1: false, info, TradeAllDocDefinitions.MTDecomplectation.Grids["ComplectPartsList"].Columns["Property2Caption"]);
			}
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		private void OnItemCodeChanged(string value)
		{
			ResetForceValidationBit("ItemCode");
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		private void OnItemNameChanged(string value)
		{
			ResetForceValidationBit("ItemName");
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		private void OnUnitCodeChanged(string value)
		{
			ResetForceValidationBit("UnitCode");
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		private void OnUnitBriefChanged(string value)
		{
			ResetForceValidationBit("UnitBrief");
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		private void OnQuantityChanged(decimal value)
		{
			ResetForceValidationBit("Quantity", "Quantity");
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		private void OnPricePartChanged(decimal value)
		{
			ResetForceValidationBit("PricePart");
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		private void OnSummaPartChanged(decimal value)
		{
			ResetForceValidationBit("SummaPart");
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

	public class MTDecomplectationFilteredSearchList : IResultList
	{
		private readonly MTDecomplectation _doc;

		private readonly BindingList<MTDecomplectationSpecificationRow> _itemsSource;

		public List<FilteredSearchGridColumn> Columns => new List<FilteredSearchGridColumn>
		{
			new FilteredSearchGridColumn
			{
				Header = ArmSoft.SME.Common.Resources.Code,
				FieldName = "ItemCode",
				CustomWidth = 125.0
			},
			new FilteredSearchGridColumn
			{
				Header = ArmSoft.SME.Common.Resources.Name,
				FieldName = "ItemName",
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

		public MTDecomplectationFilteredSearchList(MTDecomplectation doc)
		{
			_doc = doc;
			_itemsSource = new BindingList<MTDecomplectationSpecificationRow>();
		}

		public int AddToItemsSource(object key, params object[] args)
		{
			string mtCode = key.ToString();
			decimal num = Convert.ToDecimal(args[0]);
			int num2 = _itemsSource.FindIndex((MTDecomplectationSpecificationRow x) => x.ItemCode == mtCode);
			if (num2 == -1)
			{
				MaterialCacheRow item = TradeSettings.TradeCacheService.Material.GetItem(mtCode);
				MTDecomplectationSpecificationRow item2 = new MTDecomplectationSpecificationRow
				{
					ItemID = item.Id,
					ItemCode = item.MTCode,
					ItemName = item.Caption,
					UnitCode = item.Unit,
					UnitBrief = item.UnitRow.Brief,
					Country = item.Country,
					Producer = item.Producer
				};
				_itemsSource.Add(item2);
				num2 = _itemsSource.Count - 1;
			}
			_itemsSource[num2].Quantity += num;
			return num2;
		}
	}

	private TextEditExt _barCode;

	private Button _codeEnter;

	private DropDownControl _storageExpense;

	private DropDownControl _storageIncome;

	public DropDownControl _complect;

	public PopupCalcEditExt _quantity;

	private DropDownControl _quantityUnit;

	private DropDownCommentControl _partyMethod;

	private ComboBoxEditExt _comment;

	private DropDownControl _owner;

	private BarEditItemExt _loadComplectPartsList;

	public int _maxRowID;

	private TableView _mtComplectTableView;

	private BarEditItemExt _filteredSearch;

	public ObservableCollection<MTDecomplectationSpecificationRow> _mtComplectPartsList;

	public DocumentGridControl _mtComplectPartsGrid;

	private DocumentGridColumn _itemCodeCol;

	private DocumentGridColumn _unitCodeCol;

	private DocumentGridColumn _quantityCol;

	private DocumentGridColumn _pricePart;

	private DocumentGridColumn _summaPart;

	private DocumentGridColumn _useLifeCol;

	private DocumentGridColumn _countryCol;

	private DocumentGridColumn _producerCol;

	private DocumentGridColumn _property1Col;

	private DocumentGridColumn _property2Col;

	private ReportBarButtonItem _barButtonJumpTo;

	private ReportBarButtonItem _barButtonGoodsOperationsJournal;

	private BarSubItem _barSubItemActions = new BarSubItem();

	private BarSubItem _barSubItemJournals = new BarSubItem();

	private string _partyMethodName
	{
		get
		{
			if (!PO.IsPartySelected)
			{
				return ArmSoft.SME.Common.Resources.AutoSelection;
			}
			return ArmSoft.SME.Common.Resources.PartySelected;
		}
	}

	public MTDecomplectationWindow()
	{
	}

	public MTDecomplectationWindow(MTDecomplectation doc)
		: base(doc)
	{
	}

	protected override void ConstructUI()
	{
		base.ConstructUI();
		Storages idv = new Storages(false);
		LayoutGroup container = AddLayoutGroup(null, LayoutGroupView.Group, Orientation.Horizontal);
		_date = AddDateControl(container, Definition.Fields["DocDate"]);
		_date.Margin = new Thickness(0.0, 0.0, 10.0, 0.0);
		_docNum = AddButtonEditControl(container, Definition.Fields["DocNum"], 1);
		_docNum.AllowMagicButton = true;
		_docNum.MagicToolTip = ArmSoft.SME.Common.Resources.DocNextNumbers;
		_docNum.MagicClick += new SafeDocumentEventHandler<MagicClickEventArgs>(DocNum_MagicClick, this).Handler;
		_docNum.EditValueChanged += new SafeDocumentEventHandler<EditValueChangedEventArgs>(DocNum_EditValueChanged, this).Handler;
		_storageExpense = AddDropDownControl(null, Definition.Fields["StorageExpense"], idv, allowAdditionButtons: false, fieldWithCaption: true, HorizontalAlignment.Stretch);
		_storageExpense.EditValueChanged += new SafeDocumentEventHandler<EditValueChangedEventArgs>(StorageExpense_EditValueChanged, this).Handler;
		_storageIncome = AddDropDownControl(null, Definition.Fields["StorageIncome"], idv, allowAdditionButtons: false, fieldWithCaption: true, HorizontalAlignment.Stretch);
		LayoutGroup container2 = AddLayoutGroup(null, LayoutGroupView.Group, Orientation.Horizontal);
		_barCode = AddStringControl(container2, ArmSoft.SME.Common.Resources.BarCode + " (F2)", isRequired: false, 20, HorizontalAlignment.Stretch);
		_barCode.NullText = ArmSoft.SME.Trade.Common.Resources.BarCodeAndCodeInputNullText;
		_barCode.KeyDown += new SafeDocumentEventHandler<KeyEventArgs>(BarCode_KeyDown, this).Handler;
		string text = ArmSoft.SME.Trade.Common.Resources.cmdCodeEnter + " (F8)";
		_codeEnter = AddButton(container2, null, text, null, text, isTabStop: true);
		_codeEnter.Click += new SafeDocumentEventHandler<RoutedEventArgs>(CodeEnter_Click, this).Handler;
		DXTabControl tc = AddTabControl(null);
		LayoutGroup container3 = AddTabbedGroup(tc, ArmSoft.SME.Common.Resources.General, "General");
		_complect = AddDropDownControl(container3, Definition.Fields["MTComplectCode"], new Complects(), allowAdditionButtons: true, fieldWithCaption: true, HorizontalAlignment.Stretch);
		_complect.EditValueChanged += new SafeDocumentEventHandler<EditValueChangedEventArgs>(Complect_EditValueChanged, this).Handler;
		_quantityUnit = AddDropDownControl(container3, Definition.Fields["QuantityUnit"], new QntUnits(), allowAdditionButtons: false, fieldWithCaption: true, HorizontalAlignment.Stretch);
		_quantity = AddNumericControl(container3, Definition.Fields["Quantity"]);
		_partyMethod = AddDropDownCommentControl(container3, ArmSoft.SME.Common.Resources.Party, isRequired: true, new StandardComments(), 19, HorizontalAlignment.Stretch);
		_partyMethod.Text = _partyMethodName;
		_partyMethod.IsTextEditable = false;
		_partyMethod.ClickDropDown += new SafeDocumentEventHandler<DropDownControlEventArgs>(PartyMethod_ClickDropDown, this).Handler;
		_comment = AddComboBoxControl(container3, Definition.Fields["Comment"], TradeSettings.TradeCacheService.StandardComments.GetCaptionsList(), HorizontalAlignment.Stretch);
		_owner = AddDropDownControl(container3, Definition.Fields["Owner"], new UsersFilter());
		_owner.CodeWidth = 12;
		_owner.ShowComment = false;
		_owner.InnerCodeEdit.Width = _owner.InnerCodeEdit.MinWidth;
		if (!Settings.LoginService.IsAdmin && Settings.ParametersService.DenyAccessToOtherOwnerDocs())
		{
			_owner.Visibility = Visibility.Collapsed;
		}
		_mtComplectPartsGrid = AddGrid(container3, Definition.Grids["ComplectPartsList"], HorizontalAlignment.Stretch, VerticalAlignment.Stretch, isSupportDialogMode: true);
		_mtComplectPartsGrid.ToolTip = ArmSoft.SME.Trade.Common.Resources.MTComplectsParts;
		_mtComplectPartsGrid.IsRequired = true;
		_mtComplectPartsGrid.AddingRow += new SafeDocumentEventHandler<DocumentGridAddingRowEventArgs>(MTComplectPartsGrid_AddingRow, this).Handler;
		_mtComplectPartsGrid.DuplicatingRow += new SafeDocumentEventHandler<DocumentGridDuplicatingRowEventArgs>(MTComplectPartsGrid_DuplicatingRow, this).Handler;
		_mtComplectPartsGrid.MovingRowUp += new SafeDocumentEventHandler<DocumentGridMovingRowUpOrDownEventArgs>(MTComplectPartsGrid_MovingRowUp, this).Handler;
		_mtComplectPartsGrid.MovingRowDown += new SafeDocumentEventHandler<DocumentGridMovingRowUpOrDownEventArgs>(MTComplectPartsGrid_MovingRowDown, this).Handler;
		_mtComplectPartsGrid.DeletedRow += new SafeDocumentEventHandler<EventArgs>(MTComplectPartsGrid_DeletedRow, this).Handler;
		_mtComplectPartsGrid.AddingRowWithDialog += new SafeDocumentEventHandler<DocumentGridAddingRowEventArgs>(MTComplectPartsGrid_AddingRowWithDialog, this).Handler;
		_mtComplectPartsGrid.DuplicatingRowWithDialog += new SafeDocumentEventHandler<DocumentGridDuplicatingRowEventArgs>(MTComplectPartsGrid_DuplicatingRowWithDialog, this).Handler;
		_mtComplectPartsGrid.EditingRowWithDialog += new SafeDocumentEventHandler<DocumentGridEditingRowEventArgs>(MTComplectPartsGrid_EditingRowWithDialog, this).Handler;
		_mtComplectPartsGrid.ClickDropDown += new SafeDocumentEventHandler<DocumentGridClickDropDownEventArgs>(MTComplectPartsGrid_ClickDropDown, this).Handler;
		_mtComplectPartsGrid.BeforeAddNewDropDown += new SafeDocumentEventHandler<DocumentGridClickDropDownEventArgs>(MTComplectPartsGrid_BeforeAddNewDropDown, this).Handler;
		_mtComplectPartsGrid.AllowMoveUp = true;
		_mtComplectPartsGrid.AllowMoveDown = true;
		_mtComplectTableView = _mtComplectPartsGrid.View;
		_mtComplectTableView.CellValueChanged += new SafeDocumentEventHandler<CellValueChangedEventArgs>(MTComplectTableView_CellValueChanged, this).Handler;
		_mtComplectTableView.ShowingEditor += new SafeDocumentEventHandler<ShowingEditorEventArgs>(MTComplectTableView_ShowingEditor, this).Handler;
		_mtComplectTableView.FocusedRowHandleChanged += new SafeDocumentEventHandler<FocusedRowHandleChangedEventArgs>(MTComplectTableView_FocusedRowHandleChanged, this).Handler;
		_loadComplectPartsList = _mtComplectPartsGrid.AddButton("fill_down_16.svg", ArmSoft.SME.Common.Resources.ButtonLoad, ArmSoft.SME.Common.Resources.ButtonLoad, 90.0);
		_loadComplectPartsList.Click += new SafeDocumentEventHandler<RoutedEventArgs>(LoadComplectPartsList_Click, this).Handler;
		_filteredSearch = _mtComplectPartsGrid.AddButton(null, ArmSoft.SME.Trade.Common.Resources.GroupAddition, ArmSoft.SME.Trade.Common.Resources.GroupAddition, 130.0);
		_filteredSearch.Click += new SafeDocumentEventHandler<RoutedEventArgs>(FilteredSearch_Click, this).Handler;
		_mtComplectPartsGrid.AddRowIndicator(35);
		_itemCodeCol = _mtComplectPartsGrid.AddDropDownColumn(Definition.Grids["ComplectPartsList"].Columns["ItemCode"], new GoodsFilter(shortMode: true, ItemTypesSE.Goods.Value), allowAdditionButtons: true);
		_itemCodeCol.Width = 125.0;
		DocumentGridColumn documentGridColumn = _mtComplectPartsGrid.AddStringColumn(Definition.Grids["ComplectPartsList"].Columns["ItemName"]);
		documentGridColumn.AllowEditing = DefaultBoolean.False;
		documentGridColumn.Width = 225.0;
		_unitCodeCol = _mtComplectPartsGrid.AddDropDownColumn(Definition.Grids["ComplectPartsList"].Columns["UnitCode"]);
		_mtComplectPartsGrid.AddStringColumn(Definition.Grids["ComplectPartsList"].Columns["UnitBrief"]).AllowEditing = DefaultBoolean.False;
		_quantityCol = _mtComplectPartsGrid.AddNumberColumn(Definition.Grids["ComplectPartsList"].Columns["Quantity"], showTotal: true);
		_pricePart = _mtComplectPartsGrid.AddNumberColumn(Definition.Grids["ComplectPartsList"].Columns["PricePart"], showTotal: true);
		_summaPart = _mtComplectPartsGrid.AddNumberColumn(Definition.Grids["ComplectPartsList"].Columns["SummaPart"], showTotal: true);
		_useLifeCol = _mtComplectPartsGrid.AddDateColumn("UseLife", ArmSoft.SME.Trade.Common.Resources.UseLife);
		_useLifeCol.Visible = Settings.ParametersService.ShowPartyUSEABLEDATE();
		_useLifeCol.ShowInColumnChooser = _useLifeCol.Visible;
		_countryCol = _mtComplectPartsGrid.AddDropDownColumn(Definition.Grids["ComplectPartsList"].Columns["Country"], new Countries());
		_countryCol.Visible = Settings.ParametersService.ShowPartyCountry();
		_countryCol.ShowInColumnChooser = _countryCol.Visible;
		_producerCol = _mtComplectPartsGrid.AddDropDownColumn(Definition.Grids["ComplectPartsList"].Columns["Producer"], new Producer());
		_producerCol.Width = SizeCalculator.WidthForString(5);
		_producerCol.Visible = Settings.ParametersService.ShowPartyProducer();
		_producerCol.ShowInColumnChooser = _producerCol.Visible;
		_property1Col = _mtComplectPartsGrid.AddDropDownColumn(Definition.Grids["ComplectPartsList"].Columns["Property1Caption"], new PropertyList(), allowAdditionButtons: true, isOwnProcessingClickDropDown: true);
		_property1Col.Width = SizeCalculator.WidthForString(Convert.ToInt32(25));
		_property1Col.Visible = Settings.ParametersService.ShowPartyProperty1();
		_property1Col.ShowInColumnChooser = _property1Col.Visible;
		_property2Col = _mtComplectPartsGrid.AddDropDownColumn(Definition.Grids["ComplectPartsList"].Columns["Property2Caption"], new PropertyList(), allowAdditionButtons: true, isOwnProcessingClickDropDown: true);
		_property2Col.Width = SizeCalculator.WidthForString(Convert.ToInt32(25));
		_property2Col.Visible = Settings.ParametersService.ShowPartyProperty2();
		_property2Col.ShowInColumnChooser = _property2Col.Visible;
		_mtComplectPartsList = new ObservableCollection<MTDecomplectationSpecificationRow>();
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
		Ribbon.ToolbarItemLinks.Add(_barSubItemJournals.ConvertForToolbar("t_report_16.svg"));
	}

	protected override void AddItemsToRibbon()
	{
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		base.AddItemsToRibbon();
		CreateRibbonPageGroupActions();
		CreateRibbonPageGroupReports();
		_barSubItemJournals.Content = "";
		_barSubItemJournals.Glyph = ImageService.GetImage("report_16.svg");
		_barSubItemJournals.LargeGlyph = ImageService.GetImage("report_32.svg");
		_barButtonJumpTo = new ReportBarButtonItem(DataProviderTypeSE.AllDocumentsJournal, showDialog: false, ArmSoft.SME.Common.Resources.JumpTo, null, "doc_JumpTo_16.svg", "doc_JumpTo_32.svg");
		_barButtonJumpTo.AddInBarSubItem(ref _ribbonPageGroupActions, ref _barSubItemActions);
		_barButtonGoodsOperationsJournal = new ReportBarButtonItem(DataProviderTypeSE.GoodsRepOperations.Value, showDialog: false, ArmSoft.SME.Trade.Common.Resources.MTOperations);
		_barButtonGoodsOperationsJournal.AddInBarSubItem(ref _ribbonPageGroupReports, ref _barSubItemJournals);
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
	}

	protected override void SetReportBarButtonsEnable(bool isEnabled)
	{
		base.SetReportBarButtonsEnable(isEnabled);
		_barButtonJumpTo.IsEnabled = isEnabled;
		_barButtonGoodsOperationsJournal.IsEnabled = isEnabled;
	}

	protected override void LoadDataToControls()
	{
		base.LoadDataToControls();
		_maxRowID = PO.MaxRowID.Value;
		_mtComplectPartsList.Clear();
		foreach (MTDecomplectation.MTDecomplectationSpecificationRow complectParts in PO.ComplectPartsList)
		{
			_mtComplectPartsList.Add(new MTDecomplectationSpecificationRow
			{
				ItemID = complectParts.ItemID.Value,
				ItemName = complectParts.ItemName,
				ItemCode = complectParts.ItemCode,
				RowID = complectParts.RowID.Value,
				RowNum = complectParts.RowNum,
				Quantity = complectParts.Quantity.Value,
				UnitBrief = complectParts.UnitBrief,
				UnitCode = complectParts.UnitCode,
				UseLife = complectParts.UseLife,
				PricePart = complectParts.PricePart.Value,
				SummaPart = complectParts.SummaPart.Value,
				Country = complectParts.Country,
				Producer = complectParts.Producer,
				ProducerCaption = complectParts.ProducerCaption,
				Property1 = complectParts.Property1,
				Property1Caption = complectParts.Property1Caption,
				Property2 = complectParts.Property2,
				Property2Caption = complectParts.Property2Caption
			});
		}
		_mtComplectPartsGrid.ItemsSource = _mtComplectPartsList;
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

	private void PartyMethod_ClickDropDown(object sender, DropDownControlEventArgs e)
	{
		try
		{
			ValidateDocFields();
			bool flag = State == DocumentStateSE.Draft.Value || State == DocumentStateSE.Deleted.Value;
			MTSelectPartyWindow mTSelectPartyWindow = new MTSelectPartyWindow(_storageExpense.Code, _date.EditValue.Value, _complect.Code, _quantityUnit.Code, _quantity.Value, 0, PO.StoredInDb && !flag);
			mTSelectPartyWindow.MTExpDocSelectedPartiesList = PO.MTExpDocSelectedPartiesList;
			mTSelectPartyWindow.Load();
			mTSelectPartyWindow.Owner = this;
			if (mTSelectPartyWindow.ShowDialog().Value)
			{
				PO.IsPartySelected = mTSelectPartyWindow.IsPartySelected.HasValue && mTSelectPartyWindow.IsPartySelected.Value;
				_partyMethod.Text = _partyMethodName;
				_quantity.Value = mTSelectPartyWindow.TotalExpQty;
				PO.MTExpDocSelectedPartiesList = mTSelectPartyWindow.MTExpDocSelectedPartiesList;
			}
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
			e.Cancel = true;
		}
		e.Cancel = true;
	}

	protected override void OnNew()
	{
		base.OnNew();
		_date.EditValue = Settings.HostingEnvironment.CurrentDate;
		_owner.Code = Settings.LoginService.UserName;
		if (string.IsNullOrWhiteSpace(_complect.Code))
		{
			PO.IsPartySelected = false;
			_partyMethod.Text = _partyMethodName;
		}
		if (_viewMode != WindowViewMode.Copy)
		{
			string text = Settings.ParametersService.DefaultStorage();
			if (!string.IsNullOrWhiteSpace(text))
			{
				StoragesCacheRow item = TradeSettings.TradeCacheService.Storage.GetItem(text);
				if ((item == null || !item.Retail) && TradeSettings.CreateTradeClient().CommonServices.IsStorageAvailable(text))
				{
					_storageExpense.Code = text;
				}
			}
		}
		_storageExpense.EditValueChanged += StorageExpense_EditValueChanged;
	}

	private void Complect_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		if (!(e.OldValue.ToString() != e.NewValue.ToString()))
		{
			return;
		}
		PO.MTComplectCode = _complect.Code;
		if (!string.IsNullOrWhiteSpace(e.OldValue.ToString()))
		{
			PO.MTExpDocSelectedPartiesList.Clear();
			PO.IsPartySelected = false;
			_partyMethod.Text = _partyMethodName;
		}
		try
		{
			if (!string.IsNullOrWhiteSpace(_complect.Code))
			{
				MaterialCacheRow item = TradeSettings.TradeCacheService.Material.GetItem(_complect.Code, throwExceptionNotExits: true);
				_quantityUnit.DataView = new MTQntUnits(item.MTCode);
				_quantityUnit.Code = item.Unit;
			}
			else
			{
				_quantityUnit.Code = string.Empty;
			}
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	protected override void OnNewAfterSave()
	{
		base.OnNewAfterSave();
		PO.MTExpDocSelectedPartiesList.Clear();
		PO.IsPartySelected = false;
		_partyMethod.Text = _partyMethodName;
		_mtComplectPartsList.Clear();
		_maxRowID = 0;
		_owner.Code = Settings.LoginService.UserName;
		_storageExpense.EditValueChanged += StorageExpense_EditValueChanged;
	}

	protected override void OnEdit()
	{
		base.OnEdit();
		LoadDataToControls();
		_setFocusOnActivation = _docNum;
		_storageExpense.EditValueChanged += new SafeDocumentEventHandler<EditValueChangedEventArgs>(StorageExpense_EditValueChanged, this).Handler;
	}

	protected override void SetControlsReadonly()
	{
		base.SetControlsReadonly();
		if (_viewMode == WindowViewMode.View)
		{
			return;
		}
		bool flag = State == DocumentStateSE.Draft.Value || State == DocumentStateSE.Deleted.Value;
		bool flag2 = _viewMode == WindowViewMode.Edit && !flag;
		_date.IsReadOnly = flag2;
		_barCode.IsReadOnly = flag2;
		_codeEnter.IsEnabled = !flag2;
		_complect.IsReadOnly = flag2;
		_storageIncome.IsReadOnly = flag2;
		_storageExpense.IsReadOnly = flag2;
		_quantity.IsReadOnly = flag2;
		_quantityUnit.IsReadOnly = flag2;
		foreach (GridColumn column in _mtComplectPartsGrid.Grid.Columns)
		{
			column.ReadOnly = flag2;
		}
		_mtComplectPartsGrid.AllowAdd = !flag2;
		_mtComplectPartsGrid.AllowEdit = !flag2;
		_mtComplectPartsGrid.AllowDelete = !flag2;
		_mtComplectPartsGrid.AllowDuplicate = !flag2;
		_mtComplectPartsGrid.AllowMoveUp = !flag2;
		_mtComplectPartsGrid.AllowMoveDown = !flag2;
		_loadComplectPartsList.IsVisible = !flag2;
		_filteredSearch.IsVisible = _loadComplectPartsList.IsVisible;
	}

	private void MTComplectPartsGrid_AddingRow(object sender, DocumentGridAddingRowEventArgs e)
	{
		ValidateEmptyFieldsBeforeAddition();
		_mtComplectPartsList.Add(new MTDecomplectationSpecificationRow
		{
			RowID = _maxRowID
		});
		_maxRowID++;
		e.RowNumber = _mtComplectPartsList.Count - 1;
	}

	private void MTComplectPartsGrid_DuplicatingRow(object sender, DocumentGridDuplicatingRowEventArgs e)
	{
		MTDecomplectationSpecificationRow mTDecomplectationSpecificationRow = (MTDecomplectationSpecificationRow)_mtComplectPartsGrid.Grid.GetRow(e.RowHandle);
		if (mTDecomplectationSpecificationRow != null)
		{
			throw new ASException(string.Format(ArmSoft.SME.Trade.Common.Resources.WrongComplectPart, mTDecomplectationSpecificationRow.ItemCode, _complect.Code));
		}
	}

	private void MTComplectPartsGrid_MovingRowDown(object sender, DocumentGridMovingRowUpOrDownEventArgs e)
	{
		if (_mtComplectPartsGrid.Grid.SortInfo.Count > 0 || _mtComplectTableView.Grid.VisibleRowCount != _mtComplectPartsList.Count)
		{
			throw new ASException(ArmSoft.SME.Common.Resources.NotAllowRowMoveInSortedGrid);
		}
		MTDecomplectationSpecificationRow mTDecomplectationSpecificationRow = (MTDecomplectationSpecificationRow)_mtComplectPartsGrid.Grid.GetRow(e.RowHandle);
		if (mTDecomplectationSpecificationRow != null)
		{
			_mtComplectPartsList.RemoveAt(e.RowHandle);
			_mtComplectPartsList.Insert(e.RowHandle + 1, mTDecomplectationSpecificationRow);
		}
		_mtComplectPartsGrid.Grid.RefreshData();
	}

	private void MTComplectPartsGrid_MovingRowUp(object sender, DocumentGridMovingRowUpOrDownEventArgs e)
	{
		if (_mtComplectPartsGrid.Grid.SortInfo.Count > 0 || _mtComplectTableView.Grid.VisibleRowCount != _mtComplectPartsList.Count)
		{
			throw new ASException(ArmSoft.SME.Common.Resources.NotAllowRowMoveInSortedGrid);
		}
		MTDecomplectationSpecificationRow mTDecomplectationSpecificationRow = (MTDecomplectationSpecificationRow)_mtComplectPartsGrid.Grid.GetRow(e.RowHandle);
		if (mTDecomplectationSpecificationRow != null)
		{
			_mtComplectPartsList.RemoveAt(e.RowHandle);
			_mtComplectPartsList.Insert(e.RowHandle - 1, mTDecomplectationSpecificationRow);
		}
		_mtComplectPartsGrid.Grid.RefreshData();
	}

	private void MTComplectPartsGrid_ClickDropDown(object sender, DocumentGridClickDropDownEventArgs e)
	{
		try
		{
			MTDecomplectationSpecificationRow mTDecomplectationSpecificationRow = (MTDecomplectationSpecificationRow)_mtComplectPartsGrid.Grid.GetRow(e.RowHandle);
			if (e.Column == _unitCodeCol)
			{
				if (mTDecomplectationSpecificationRow != null)
				{
					TradeSettings.TradeCacheService.Material.GetItem(_complect.Code, throwExceptionNotExits: true);
					MTQntUnits mTQntUnits = new MTQntUnits(mTDecomplectationSpecificationRow.ItemCode);
					ModalBrowser modalBrowser = new ModalBrowser(mTQntUnits)
					{
						LookUpValue = e.Value,
						Owner = e.Owner,
						Left = e.Left,
						Top = e.Top
					};
					if (modalBrowser.Show())
					{
						e.NewValue = mTQntUnits.ValueMember(modalBrowser.SelectedRowIndex);
					}
				}
			}
			else if (e.Column == _property1Col || e.Column == _property2Col)
			{
				if (mTDecomplectationSpecificationRow == null)
				{
					return;
				}
				MaterialCacheRow item = TradeSettings.TradeCacheService.Material.GetItem(mTDecomplectationSpecificationRow.ItemCode);
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
						mTDecomplectationSpecificationRow.Property1 = propertyList.ValueMember(modalBrowser2.SelectedRowIndex);
					}
					else
					{
						mTDecomplectationSpecificationRow.Property2 = propertyList.ValueMember(modalBrowser2.SelectedRowIndex);
					}
				}
			}
			_mtComplectPartsGrid.Grid.RefreshData();
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

	private void MTComplectPartsGrid_EditingRowWithDialog(object sender, DocumentGridEditingRowEventArgs e)
	{
		MTDecomplectationSpecificationRow mTDecomplectationSpecificationRow = (MTDecomplectationSpecificationRow)_mtComplectPartsGrid.Grid.GetRow(e.RowHandle);
		if (mTDecomplectationSpecificationRow != null)
		{
			ValidateInnerDocFields();
			MTDecomplectationRowAddEditDialog mTDecomplectationRowAddEditDialog = new MTDecomplectationRowAddEditDialog(e.AllowEdit ? DialogWindowMode.EditMode : DialogWindowMode.ViewMode, this, mTDecomplectationSpecificationRow);
			e.RowEdited = Convert.ToBoolean(mTDecomplectationRowAddEditDialog.Invoke());
			if (e.AllowEdit && e.AllowEdit)
			{
				_mtComplectPartsGrid.Grid.RefreshData();
			}
		}
	}

	private void MTComplectPartsGrid_AddingRowWithDialog(object sender, DocumentGridAddingRowEventArgs e)
	{
		ValidateInnerDocFields();
		MTDecomplectationRowAddEditDialog mTDecomplectationRowAddEditDialog = new MTDecomplectationRowAddEditDialog(DialogWindowMode.NewModeWithNext, this);
		mTDecomplectationRowAddEditDialog.Invoke();
		int addedRows = mTDecomplectationRowAddEditDialog.AddedRows;
		if (addedRows != 0)
		{
			_maxRowID += addedRows;
			_mtComplectPartsGrid.Grid.RefreshData();
		}
	}

	private void MTComplectPartsGrid_DuplicatingRowWithDialog(object sender, DocumentGridDuplicatingRowEventArgs e)
	{
		if ((MTDecomplectationSpecificationRow)_mtComplectPartsGrid.Grid.GetRow(e.RowHandle) != null)
		{
			ValidateInnerDocFields();
			MTDecomplectationRowAddEditDialog mTDecomplectationRowAddEditDialog = new MTDecomplectationRowAddEditDialog(DialogWindowMode.NewModeWithNext, this);
			mTDecomplectationRowAddEditDialog.Invoke();
			int addedRows = mTDecomplectationRowAddEditDialog.AddedRows;
			if (addedRows != 0)
			{
				_maxRowID += addedRows;
				_mtComplectPartsGrid.Grid.RefreshData();
			}
		}
	}

	private void MTComplectPartsGrid_DeletedRow(object sender, EventArgs e)
	{
		_mtComplectPartsGrid.Grid.RefreshData();
	}

	private void MTComplectTableView_CellValueChanged(object sender, CellValueChangedEventArgs e)
	{
		MTDecomplectationSpecificationRow mTDecomplectationSpecificationRow = (MTDecomplectationSpecificationRow)_mtComplectPartsGrid.Grid.GetRow(e.RowHandle);
		if (mTDecomplectationSpecificationRow == null)
		{
			return;
		}
		if (e.Column == _itemCodeCol)
		{
			try
			{
				if (e.Value == null || string.IsNullOrWhiteSpace(e.Value.ToString()))
				{
					mTDecomplectationSpecificationRow.ItemID = -1;
					mTDecomplectationSpecificationRow.ItemCode = "";
					mTDecomplectationSpecificationRow.ItemName = "";
					mTDecomplectationSpecificationRow.UnitBrief = "";
					mTDecomplectationSpecificationRow.UnitCode = "";
					mTDecomplectationSpecificationRow.Quantity = 0m;
					mTDecomplectationSpecificationRow.PricePart = 0m;
					mTDecomplectationSpecificationRow.SummaPart = 0m;
				}
				else
				{
					string text = e.Value.ToString();
					if (text == _complect.Code)
					{
						mTDecomplectationSpecificationRow.ItemCode = "";
						throw new ASException(ArmSoft.SME.Common.Resources.InvalidComplectMt);
					}
					foreach (MTDecomplectationSpecificationRow mtComplectParts in _mtComplectPartsList)
					{
						if (mTDecomplectationSpecificationRow != mtComplectParts && text == mtComplectParts.ItemCode)
						{
							mTDecomplectationSpecificationRow.ItemCode = "";
							throw new ASException(string.Format(ArmSoft.SME.Trade.Common.Resources.WrongComplectPart, text, _complect.Code));
						}
					}
					MaterialCacheRow item = TradeSettings.TradeCacheService.Material.GetItem(text);
					if (item != null)
					{
						mTDecomplectationSpecificationRow.ItemID = item.Id;
						mTDecomplectationSpecificationRow.ItemName = item.Caption;
						mTDecomplectationSpecificationRow.UnitCode = item.Unit;
						mTDecomplectationSpecificationRow.UnitBrief = item.UnitRow.Brief;
						mTDecomplectationSpecificationRow.Quantity = 0m;
						mTDecomplectationSpecificationRow.PricePart = 0m;
						mTDecomplectationSpecificationRow.SummaPart = 0m;
						mTDecomplectationSpecificationRow.Country = item.Country;
						mTDecomplectationSpecificationRow.Producer = item.Producer;
						mTDecomplectationSpecificationRow.Property1 = "";
						mTDecomplectationSpecificationRow.Property2 = "";
						mTDecomplectationSpecificationRow.Property1Caption = "";
						mTDecomplectationSpecificationRow.Property2Caption = "";
						_property1Col.DataView = new PropertyList(item.Property1);
						_property2Col.DataView = new PropertyList(item.Property2);
						_unitCodeCol.DataView = new MTQntUnits(item.MTCode);
					}
				}
			}
			catch (Exception ex)
			{
				mTDecomplectationSpecificationRow.ItemName = "";
				mTDecomplectationSpecificationRow.ItemID = -1;
				mTDecomplectationSpecificationRow.UnitCode = "";
				mTDecomplectationSpecificationRow.UnitBrief = "";
				mTDecomplectationSpecificationRow.Quantity = 0m;
				mTDecomplectationSpecificationRow.PricePart = 0m;
				mTDecomplectationSpecificationRow.SummaPart = 0m;
				mTDecomplectationSpecificationRow.Country = "";
				mTDecomplectationSpecificationRow.Producer = "";
				mTDecomplectationSpecificationRow.Property1 = "";
				mTDecomplectationSpecificationRow.Property1Caption = "";
				mTDecomplectationSpecificationRow.Property2Caption = "";
				mTDecomplectationSpecificationRow.Property2 = "";
				Settings.ErrorHandler(ex);
			}
			_mtComplectPartsGrid.Grid.RefreshRow(e.RowHandle);
			return;
		}
		if (e.Column == _unitCodeCol)
		{
			try
			{
				if (e.Value != null && !string.IsNullOrWhiteSpace(e.Value.ToString()))
				{
					QntUnitCacheRow item2 = TradeSettings.TradeCacheService.QntUnit.GetItem(e.Value.ToString(), throwExceptionNotExits: true);
					mTDecomplectationSpecificationRow.UnitBrief = ((item2 == null) ? "" : item2.Brief);
				}
				_mtComplectPartsGrid.Grid.RefreshRow(e.RowHandle);
				return;
			}
			catch (Exception ex2)
			{
				Settings.ErrorHandler(ex2);
				return;
			}
		}
		if (e.Column == _pricePart)
		{
			mTDecomplectationSpecificationRow.SummaPart = (Convert.ToDecimal(e.Value) * mTDecomplectationSpecificationRow.Quantity).RoundAs(3);
		}
		else if (e.Column == _summaPart)
		{
			if (mTDecomplectationSpecificationRow.Quantity != 0m)
			{
				mTDecomplectationSpecificationRow.PricePart = (Convert.ToDecimal(e.Value) / mTDecomplectationSpecificationRow.Quantity).RoundAs(3);
			}
			_mtComplectPartsGrid.Grid.RefreshRow(e.RowHandle);
		}
		else if (e.Column == _quantityCol)
		{
			if (mTDecomplectationSpecificationRow.PricePart != 0m)
			{
				mTDecomplectationSpecificationRow.SummaPart = (Convert.ToDecimal(e.Value) * mTDecomplectationSpecificationRow.PricePart).RoundAs(3);
			}
			else if (mTDecomplectationSpecificationRow.SummaPart != 0m && Convert.ToDecimal(e.Value) != 0m)
			{
				mTDecomplectationSpecificationRow.PricePart = (mTDecomplectationSpecificationRow.SummaPart / Convert.ToDecimal(e.Value)).RoundAs(3);
			}
			_mtComplectPartsGrid.Grid.RefreshRow(e.RowHandle);
		}
		else if (e.Column == _property1Col)
		{
			MaterialCacheRow itemById = TradeSettings.TradeCacheService.Material.GetItemById(mTDecomplectationSpecificationRow.ItemID);
			string destCode = ((itemById != null) ? itemById.Property1 : string.Empty);
			_property1Col.DataView = new PropertyList(destCode);
			if (!string.IsNullOrWhiteSpace(mTDecomplectationSpecificationRow.Property1Caption))
			{
				PropertyListCacheRow.RowKey key = new PropertyListCacheRow.RowKey
				{
					DestCode = destCode,
					Code = mTDecomplectationSpecificationRow.Property1
				};
				PropertyListCacheRow item3 = TradeSettings.TradeCacheService.PropertyList.GetItem(key);
				if (item3 != null)
				{
					mTDecomplectationSpecificationRow.Property1 = item3.Code;
				}
			}
			else
			{
				mTDecomplectationSpecificationRow.Property1 = string.Empty;
			}
		}
		else
		{
			if (e.Column != _property2Col)
			{
				return;
			}
			MaterialCacheRow itemById2 = TradeSettings.TradeCacheService.Material.GetItemById(mTDecomplectationSpecificationRow.ItemID);
			string destCode2 = ((itemById2 != null) ? itemById2.Property2 : string.Empty);
			_property2Col.DataView = new PropertyList(destCode2);
			if (!string.IsNullOrWhiteSpace(mTDecomplectationSpecificationRow.Property2Caption))
			{
				PropertyListCacheRow.RowKey key2 = new PropertyListCacheRow.RowKey
				{
					DestCode = destCode2,
					Code = mTDecomplectationSpecificationRow.Property2
				};
				PropertyListCacheRow item4 = TradeSettings.TradeCacheService.PropertyList.GetItem(key2);
				if (item4 != null)
				{
					mTDecomplectationSpecificationRow.Property2 = item4.Code;
				}
			}
			else
			{
				mTDecomplectationSpecificationRow.Property2 = string.Empty;
			}
		}
	}

	private void MTComplectPartsGrid_BeforeAddNewDropDown(object sender, DocumentGridClickDropDownEventArgs e)
	{
		try
		{
			MTDecomplectationSpecificationRow mTDecomplectationSpecificationRow = (MTDecomplectationSpecificationRow)_mtComplectPartsGrid.Grid.GetRow(e.RowHandle);
			if (string.IsNullOrWhiteSpace(mTDecomplectationSpecificationRow.ItemCode))
			{
				return;
			}
			MaterialCacheRow item = TradeSettings.TradeCacheService.Material.GetItem(mTDecomplectationSpecificationRow.ItemCode);
			if (item != null)
			{
				if (e.Column == _property1Col)
				{
					_property1Col.DataView = new PropertyList(item.Property1);
				}
				else if (e.Column == _property2Col)
				{
					_property2Col.DataView = new PropertyList(item.Property2);
				}
			}
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	private void MTComplectTableView_ShowingEditor(object sender, ShowingEditorEventArgs e)
	{
		if (((!_property1Col.Visible || e.Column != _property1Col) && (!_property2Col.Visible || e.Column != _property2Col)) || e.Row == null)
		{
			return;
		}
		MTDecomplectationSpecificationRow mTDecomplectationSpecificationRow = (MTDecomplectationSpecificationRow)e.Row;
		MaterialCacheRow item = TradeSettings.TradeCacheService.Material.GetItem(mTDecomplectationSpecificationRow.ItemCode);
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

	private void StorageExpense_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		if (e.NewValue != e.OldValue)
		{
			if (_storageExpense.DoValidate() && !TradeSettings.CreateTradeClient().CommonServices.IsStorageAvailable(_storageExpense.Code))
			{
				ArmSoft.SME.Core.UI.MessageBox.Show(string.Format(ArmSoft.SME.Trade.Common.Resources.StorageNotAvailForUser, Settings.LoginService.UserName, _storageExpense.Code), Settings.ProgramName, MessageBoxButton.OK, MessageBoxImage.Exclamation);
				_storageExpense.Code = "";
			}
			if (!string.IsNullOrWhiteSpace(e.OldValue.ToString()))
			{
				PO.MTExpDocSelectedPartiesList.Clear();
				PO.IsPartySelected = false;
				_partyMethod.Text = _partyMethodName;
			}
		}
	}

	private void ValidateDocFields()
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
		ValidateInnerDocFields();
	}

	private void ValidateInnerDocFields()
	{
		if (string.IsNullOrWhiteSpace(_complect.Code))
		{
			ControlsHelper.SetFocus(_complect);
			throw new ASException(ArmSoft.SME.Common.Resources.EmptyComplect);
		}
		if (!_complect.DoValidate())
		{
			ControlsHelper.SetFocus(_complect);
			throw new ASException(string.Format(ArmSoft.SME.Trade.Common.Resources.MTEgineInavidMaterial, _complect.Code));
		}
		if (string.IsNullOrWhiteSpace(_quantityUnit.Code))
		{
			ControlsHelper.SetFocus(_quantityUnit);
			throw new ASException(string.Format(ArmSoft.SME.Common.Resources.FieldIsEmpty, ArmSoft.SME.Common.Resources.Unit));
		}
		if (!_quantityUnit.DoValidate())
		{
			ControlsHelper.SetFocus(_quantityUnit);
			throw new ASException(ArmSoft.SME.Trade.Common.Resources.InvalidQntUnit);
		}
		if (!_quantity.DoValidate())
		{
			ControlsHelper.SetFocus(_quantity);
			throw new ASException(ArmSoft.SME.Trade.Common.Resources.ErrNegOrZeroQuantity);
		}
	}

	private void LoadComplectPartsList_Click(object sender, RoutedEventArgs e)
	{
		try
		{
			ValidateInnerDocFields();
			if (_mtComplectPartsList.Count > 0)
			{
				if (ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Common.Resources.RefreshComplectGrid, Settings.ProgramName, MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No) == MessageBoxResult.No)
				{
					return;
				}
				_mtComplectPartsList.Clear();
			}
			MaterialCacheRow item = TradeSettings.TradeCacheService.Material.GetItem(_complect.Code);
			decimal value = _quantity.Value;
			if (item == null)
			{
				return;
			}
			value *= TradeSettings.TradeCacheService.Material.GetQntUnit(item, _quantityUnit.Code)?.Coef ?? 1m;
			List<MaterialCacheRow.ComplectPartRow> complectParts = TradeSettings.TradeCacheService.Material.GetItem(item.MTCode).ComplectParts;
			if (complectParts.Count > 0)
			{
				_maxRowID = 0;
				foreach (MaterialCacheRow.ComplectPartRow item2 in complectParts)
				{
					MTDecomplectationSpecificationRow mTDecomplectationSpecificationRow = new MTDecomplectationSpecificationRow();
					mTDecomplectationSpecificationRow.ItemID = item2.ItemId;
					mTDecomplectationSpecificationRow.UnitCode = item2.QntUnit;
					MaterialCacheRow itemById = TradeSettings.TradeCacheService.Material.GetItemById(mTDecomplectationSpecificationRow.ItemID);
					mTDecomplectationSpecificationRow.UnitBrief = TradeSettings.TradeCacheService.QntUnit.GetItem(mTDecomplectationSpecificationRow.UnitCode).Brief;
					mTDecomplectationSpecificationRow.ItemCode = itemById.MTCode;
					mTDecomplectationSpecificationRow.ItemName = itemById.Caption;
					mTDecomplectationSpecificationRow.Country = itemById.Country;
					mTDecomplectationSpecificationRow.Producer = itemById.Producer;
					mTDecomplectationSpecificationRow.Quantity = (item2.Quantity * value).RoundAs(3);
					if (item2.ComplectQnt != 0m)
					{
						mTDecomplectationSpecificationRow.Quantity = (mTDecomplectationSpecificationRow.Quantity / item2.ComplectQnt).RoundAs(3);
					}
					mTDecomplectationSpecificationRow.PricePart = item2.PricePart;
					mTDecomplectationSpecificationRow.SummaPart = (item2.PricePart * mTDecomplectationSpecificationRow.Quantity).RoundAs(3);
					mTDecomplectationSpecificationRow.RowID = _maxRowID;
					_mtComplectPartsList.Add(mTDecomplectationSpecificationRow);
					_maxRowID++;
				}
			}
			_mtComplectPartsGrid.Grid.RefreshData();
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
			string unitCode = string.Empty;
			string unitBrief = string.Empty;
			string barCode = _barCode.Text.Trim();
			MaterialCacheRow materialCacheRow = DocumentHelpers.TryGetItemByBarCode(ref barCode, out qty, out unitCode, out unitBrief);
			if (materialCacheRow == null || materialCacheRow.Type == ItemTypesSE.Services.Value)
			{
				ControlsHelper.SetFocus(_barCode);
				throw new ASException(string.Format(ArmSoft.SME.Common.Resources.ErrBarCodeNotExist, _barCode.Text.Trim()));
			}
			AddMTToList(materialCacheRow, unitCode, unitBrief, qty);
			_barCode.Text = "";
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	protected override void OnF8()
	{
		On_CodeEnterClick();
	}

	private void CodeEnter_Click(object sender, RoutedEventArgs e)
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
			if (item == null)
			{
				ControlsHelper.SetFocus(_barCode);
				throw new ASException(string.Format(ArmSoft.SME.Trade.Common.Resources.MTEgineInavidMaterial, text));
			}
			AddMTToList(item);
			_barCode.Text = "";
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
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

	private void AddNewRowInner(MaterialCacheRow goodsRow, string unitCode, string unitBrief, decimal quantity = 1m)
	{
		MTDecomplectationSpecificationRow mTDecomplectationSpecificationRow = new MTDecomplectationSpecificationRow();
		FillMTInvGridRow(mTDecomplectationSpecificationRow, goodsRow, unitCode, unitBrief, quantity);
		mTDecomplectationSpecificationRow.RowID = _maxRowID;
		_mtComplectPartsList.Add(mTDecomplectationSpecificationRow);
		_maxRowID++;
		int rowHandleByListIndex = _mtComplectPartsGrid.Grid.GetRowHandleByListIndex(_mtComplectPartsList.Count - 1);
		_mtComplectPartsGrid.Grid.RefreshRow(rowHandleByListIndex);
		POWindow<MTDecomplectation>.SelectCellInGrid(_mtComplectTableView, _mtComplectPartsGrid, rowHandleByListIndex, "Quantity");
		_mtComplectPartsGrid.ConfigureButtons();
	}

	private void AddNewRowOrMergeInner(MaterialCacheRow goodsRow, bool forFocusedRow, string unitCode, string unitBrief, decimal quantity = 1m)
	{
		MTDecomplectationSpecificationRow mTDecomplectationSpecificationRow = null;
		if (forFocusedRow && _mtComplectPartsList.Count != 0)
		{
			mTDecomplectationSpecificationRow = (MTDecomplectationSpecificationRow)_mtComplectTableView.Grid.CurrentItem;
			if (mTDecomplectationSpecificationRow == null)
			{
				return;
			}
		}
		for (int i = 0; i <= _mtComplectPartsList.Count - 1; i++)
		{
			if ((!forFocusedRow || mTDecomplectationSpecificationRow.RowID == _mtComplectPartsList[i].RowID) && goodsRow.Id == _mtComplectPartsList[i].ItemID && unitCode == _mtComplectPartsList[i].UnitCode)
			{
				FillMTInvGridRow(_mtComplectPartsList[i], goodsRow, unitCode, unitBrief, quantity);
				int rowHandleByListIndex = _mtComplectPartsGrid.Grid.GetRowHandleByListIndex(i);
				_mtComplectPartsGrid.Grid.RefreshRow(rowHandleByListIndex);
				POWindow<MTDecomplectation>.SelectCellInGrid(_mtComplectTableView, _mtComplectPartsGrid, rowHandleByListIndex, "Quantity");
				return;
			}
		}
		AddNewRowInner(goodsRow, unitCode, unitBrief, quantity);
	}

	private void FillMTInvGridRow(MTDecomplectationSpecificationRow mtInvGridRow, MaterialCacheRow goodsRow, string unitCode, string unitBrief, decimal quantity = 1m)
	{
		if (goodsRow != null)
		{
			mtInvGridRow.ItemID = goodsRow.Id;
			mtInvGridRow.ItemCode = goodsRow.MTCode;
			mtInvGridRow.ItemName = goodsRow.Caption;
			mtInvGridRow.Country = goodsRow.Country;
			mtInvGridRow.Producer = goodsRow.Producer;
		}
		mtInvGridRow.UnitCode = unitCode;
		mtInvGridRow.UnitBrief = unitBrief;
		mtInvGridRow.Quantity += quantity;
		mtInvGridRow.PricePart = 0m;
		mtInvGridRow.SummaPart = 0m;
	}

	private void MTComplectTableView_FocusedRowHandleChanged(object sender, FocusedRowHandleChangedEventArgs e)
	{
		int focusedRowHandle = _mtComplectTableView.FocusedRowHandle;
		MTDecomplectationSpecificationRow mTDecomplectationSpecificationRow = (MTDecomplectationSpecificationRow)_mtComplectPartsGrid.Grid.GetRow(focusedRowHandle);
		if (mTDecomplectationSpecificationRow != null)
		{
			ChangeItemCodeColumnAdditonalView(mTDecomplectationSpecificationRow.ItemCode);
		}
	}

	private void ChangeItemCodeColumnAdditonalView(string itemCode)
	{
		_unitCodeCol.DataView = new MTQntUnits((!string.IsNullOrWhiteSpace(itemCode)) ? itemCode : "");
	}

	private void FilteredSearch_Click(object sender, RoutedEventArgs e)
	{
		try
		{
			ValidateEmptyFieldsBeforeAddition();
			FilteredSearchWindow filteredSearchWindow = new FilteredSearchWindow(new FilteredSearch
			{
				FilterDataView = new GoodsGroups(showAsTree: true, filteredSearch: true),
				SearchDataView = new GoodsFilter(shortMode: true, ItemTypesSE.Goods.Value),
				ResultsList = new MTDecomplectationFilteredSearchList(PO)
			}, this, isBasketMode: true);
			if (!filteredSearchWindow.ShowDialog().Value)
			{
				return;
			}
			foreach (DocumentGridRow item in filteredSearchWindow.BasketResult)
			{
				bool flag = false;
				MTDecomplectationSpecificationRow mTDecomplectationSpecificationRow = (MTDecomplectationSpecificationRow)item;
				if (mTDecomplectationSpecificationRow.ItemCode == _complect.Code)
				{
					ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Common.Resources.InvalidComplectMt, Settings.ProgramName, MessageBoxButton.OK, MessageBoxImage.Hand);
					continue;
				}
				foreach (MTDecomplectationSpecificationRow mtComplectParts in _mtComplectPartsList)
				{
					if (mTDecomplectationSpecificationRow != mtComplectParts && mTDecomplectationSpecificationRow.ItemID == mtComplectParts.ItemID)
					{
						mtComplectParts.Quantity += mTDecomplectationSpecificationRow.Quantity;
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					mTDecomplectationSpecificationRow.RowID = _maxRowID;
					_mtComplectPartsList.Add(mTDecomplectationSpecificationRow);
					_maxRowID++;
				}
			}
			_mtComplectPartsGrid.Grid.RefreshData();
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	private void ValidateEmptyFieldsBeforeAddition()
	{
		if (!_date.DoValidate())
		{
			ControlsHelper.SetFocus(_date);
			throw new ASException(ArmSoft.SME.Common.Resources.ValidateEmptyDate);
		}
		if (string.IsNullOrWhiteSpace(_complect.Code))
		{
			ControlsHelper.SetFocus(_complect);
			throw new ASException(ArmSoft.SME.Common.Resources.EmptyComplect);
		}
	}

	protected override void FillDocument(StoreRequest sr)
	{
		base.FillDocument(sr);
		PO.MaxRowID = _maxRowID;
		PO.ComplectPartsList.Clear();
		foreach (MTDecomplectationSpecificationRow mtComplectParts in _mtComplectPartsList)
		{
			PO.ComplectPartsList.Add(new MTDecomplectation.MTDecomplectationSpecificationRow
			{
				ItemID = mtComplectParts.ItemID,
				ItemName = mtComplectParts.ItemName,
				ItemCode = mtComplectParts.ItemCode,
				RowID = mtComplectParts.RowID,
				RowNum = mtComplectParts.RowNum,
				Quantity = mtComplectParts.Quantity,
				UnitBrief = mtComplectParts.UnitBrief,
				UnitCode = mtComplectParts.UnitCode,
				UseLife = mtComplectParts.UseLife,
				PricePart = mtComplectParts.PricePart,
				SummaPart = mtComplectParts.SummaPart,
				Country = mtComplectParts.Country,
				Producer = mtComplectParts.Producer,
				ProducerCaption = mtComplectParts.ProducerCaption,
				Property1 = mtComplectParts.Property1,
				Property1Caption = mtComplectParts.Property1Caption,
				Property2 = mtComplectParts.Property2,
				Property2Caption = mtComplectParts.Property2Caption
			});
		}
	}

	protected override void StoreSettings(RegistryKey rk)
	{
		base.StoreSettings(rk);
		_mtComplectPartsGrid.StoreSettings(rk);
	}

	protected override void RestoreSettings(double defaultWidth, double defaultHeight, RegistryKey rk)
	{
		base.RestoreSettings(defaultWidth, defaultHeight, rk);
		_mtComplectPartsGrid.RestoreSettings(rk);
	}
}
