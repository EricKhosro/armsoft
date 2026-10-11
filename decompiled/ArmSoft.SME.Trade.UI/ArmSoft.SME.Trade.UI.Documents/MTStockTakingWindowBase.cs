using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Extensions;
using ArmSoft.SME.Common.Models;
using ArmSoft.SME.Common.Models.Documents;
using ArmSoft.SME.Common.PersistableObject;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Core.UI.Controls;
using ArmSoft.SME.Core.UI.DataViews;
using ArmSoft.SME.Core.UI.Documents;
using ArmSoft.SME.Core.UI.Enums;
using ArmSoft.SME.Core.UI.Extensions;
using ArmSoft.SME.Core.UI.Helpers;
using ArmSoft.SME.Core.UI.PersistableObject;
using ArmSoft.SME.Trade.Common;
using ArmSoft.SME.Trade.Common.Documents.Definition;
using ArmSoft.SME.Trade.Common.Models;
using ArmSoft.SME.Trade.Common.Models.Caches;
using ArmSoft.SME.Trade.Common.Models.Documents;
using ArmSoft.SME.Trade.Common.SmartEnums;
using ArmSoft.SME.Trade.UI.CustomUIElements;
using ArmSoft.SME.Trade.UI.DropDown;
using ArmSoft.SME.Trade.UI.Helpers;
using ArmSoft.SME.Trade.UI.OtherWindows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.ComponentModel.__Internals;
using DevExpress.Data.Extensions;
using DevExpress.Xpf.Bars;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Editors;
using DevExpress.Xpf.Grid;
using DevExpress.XtraEditors.DXErrorProvider;
using Microsoft.Win32;

namespace ArmSoft.SME.Trade.UI.Documents;

public abstract class MTStockTakingWindowBase<T> : TradeDocumentWindow<T> where T : MTStockTakingBase, new()
{
	public class MTStockTakingSpecificationRow : DocumentGridRow
	{
		[ObservableProperty]
		private int mTID = -1;

		[ObservableProperty]
		private string mTCode = "";

		[ObservableProperty]
		private string mTName;

		[ObservableProperty]
		private short rowNum;

		[ObservableProperty]
		private string unitCode = "";

		private string _oldUnitCode;

		[ObservableProperty]
		private decimal accQuantity;

		[ObservableProperty]
		private decimal actualQuantity;

		[ObservableProperty]
		private decimal varianceQuantity;

		[ObservableProperty]
		private decimal accSum;

		[ObservableProperty]
		private decimal price;

		[ObservableProperty]
		private decimal salePrice;

		[ObservableProperty]
		private decimal actualSum;

		[ObservableProperty]
		private decimal saleSum;

		[ObservableProperty]
		private bool userAdd;

		[ObservableProperty]
		private string fullName;

		[ObservableProperty]
		private string unitBrief;

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
					OnPropertyChanged(__KnownINotifyPropertyChangedArgs.MTName);
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
					string oldValue = unitCode;
					OnPropertyChanging(__KnownINotifyPropertyChangingArgs.UnitCode);
					unitCode = value;
					OnUnitCodeChanged(oldValue, value);
					OnPropertyChanged(__KnownINotifyPropertyChangedArgs.UnitCode);
				}
			}
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public decimal AccQuantity
		{
			get
			{
				return accQuantity;
			}
			set
			{
				if (!EqualityComparer<decimal>.Default.Equals(accQuantity, value))
				{
					OnPropertyChanging(__KnownINotifyPropertyChangingArgs.AccQuantity);
					accQuantity = value;
					OnAccQuantityChanged(value);
					OnPropertyChanged(__KnownINotifyPropertyChangedArgs.AccQuantity);
				}
			}
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public decimal ActualQuantity
		{
			get
			{
				return actualQuantity;
			}
			set
			{
				if (!EqualityComparer<decimal>.Default.Equals(actualQuantity, value))
				{
					OnPropertyChanging(__KnownINotifyPropertyChangingArgs.ActualQuantity);
					actualQuantity = value;
					OnActualQuantityChanged(value);
					OnPropertyChanged(__KnownINotifyPropertyChangedArgs.ActualQuantity);
				}
			}
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public decimal VarianceQuantity
		{
			get
			{
				return varianceQuantity;
			}
			set
			{
				if (!EqualityComparer<decimal>.Default.Equals(varianceQuantity, value))
				{
					OnPropertyChanging(__KnownINotifyPropertyChangingArgs.VarianceQuantity);
					varianceQuantity = value;
					OnPropertyChanged(__KnownINotifyPropertyChangedArgs.VarianceQuantity);
				}
			}
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public decimal AccSum
		{
			get
			{
				return accSum;
			}
			set
			{
				if (!EqualityComparer<decimal>.Default.Equals(accSum, value))
				{
					OnPropertyChanging(__KnownINotifyPropertyChangingArgs.AccSum);
					accSum = value;
					OnPropertyChanged(__KnownINotifyPropertyChangedArgs.AccSum);
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
		public decimal ActualSum
		{
			get
			{
				return actualSum;
			}
			set
			{
				if (!EqualityComparer<decimal>.Default.Equals(actualSum, value))
				{
					OnPropertyChanging(__KnownINotifyPropertyChangingArgs.ActualSum);
					actualSum = value;
					OnActualSumChanged(value);
					OnPropertyChanged(__KnownINotifyPropertyChangedArgs.ActualSum);
				}
			}
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public decimal SaleSum
		{
			get
			{
				return saleSum;
			}
			set
			{
				if (!EqualityComparer<decimal>.Default.Equals(saleSum, value))
				{
					OnPropertyChanging(__KnownINotifyPropertyChangingArgs.SaleSum);
					saleSum = value;
					OnSaleSumChanged(value);
					OnPropertyChanged(__KnownINotifyPropertyChangedArgs.SaleSum);
				}
			}
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public bool UserAdd
		{
			get
			{
				return userAdd;
			}
			set
			{
				if (!EqualityComparer<bool>.Default.Equals(userAdd, value))
				{
					OnPropertyChanging(__KnownINotifyPropertyChangingArgs.UserAdd);
					userAdd = value;
					OnPropertyChanged(__KnownINotifyPropertyChangedArgs.UserAdd);
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

		public MTStockTakingSpecificationRow()
		{
		}

		public MTStockTakingSpecificationRow(IPOWindowInfo docWindow)
			: base(docWindow)
		{
		}

		static MTStockTakingSpecificationRow()
		{
			DocumentGridRow.RegisterPropertyValidators(typeof(MTStockTakingSpecificationRow), cacheValidationResults: true, new Dictionary<string, Action<DocumentGridRow, ErrorInfo>>
			{
				{
					"MTCode",
					(DocumentGridRow row, ErrorInfo info) =>
					{
						ValidateGoodCode((MTStockTakingSpecificationRow)row, info);
					}
				},
				{
					"UnitCode",
					(DocumentGridRow row, ErrorInfo info) =>
					{
						ValidateUnitCode((MTStockTakingSpecificationRow)row, info);
					}
				},
				{
					"ActualQuantity",
					(DocumentGridRow row, ErrorInfo info) =>
					{
						ValidateQuantity((MTStockTakingSpecificationRow)row, info);
					}
				},
				{
					"Price",
					(DocumentGridRow row, ErrorInfo info) =>
					{
						ValidatePrice((MTStockTakingSpecificationRow)row, info);
					}
				},
				{
					"SalePrice",
					(DocumentGridRow row, ErrorInfo info) =>
					{
						ValidateSalePrice((MTStockTakingSpecificationRow)row, info);
					}
				},
				{
					"ActualSum",
					(DocumentGridRow row, ErrorInfo info) =>
					{
						ValidateSaleSum((MTStockTakingSpecificationRow)row, info);
					}
				}
			});
		}

		private static void ValidateGoodCode(MTStockTakingSpecificationRow row, ErrorInfo info)
		{
			TradePropertyValidator.ValidateGoodCode(row.MTCode, info, (StringFieldDefinition)TradeAllDocDefinitions.MTStockTaking.Grids["MTList"].Columns["MTCode"]);
		}

		private static void ValidateUnitCode(MTStockTakingSpecificationRow row, ErrorInfo info)
		{
			TradePropertyValidator.ValidateGoodUnitCode(row.UnitCode, row.MTCode, info, TradeAllDocDefinitions.MTStockTaking.Grids["MTList"].Columns["UnitCode"]);
		}

		private static void ValidateQuantity(MTStockTakingSpecificationRow row, ErrorInfo info)
		{
			PropertyValidator.NonNegativeValidate(row.ActualQuantity, info, ArmSoft.SME.Common.Resources.ActualQTY);
		}

		private static void ValidatePrice(MTStockTakingSpecificationRow row, ErrorInfo info)
		{
			PropertyValidator.NonNegativeValidate(row.Price, info, ArmSoft.SME.Common.Resources.Price);
		}

		private static void ValidateSalePrice(MTStockTakingSpecificationRow row, ErrorInfo info)
		{
			PropertyValidator.NonNegativeValidate(row.SalePrice, info, ArmSoft.SME.Common.Resources.MTTAccSalePrice);
		}

		private static void ValidateSaleSum(MTStockTakingSpecificationRow row, ErrorInfo info)
		{
			PropertyValidator.NonNegativeValidate(row.ActualSum, info, ArmSoft.SME.Common.Resources.ActualSumm);
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		private void OnMTCodeChanged(string value)
		{
			ResetForceValidationBit("MTCode", "UnitCode");
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		private void OnUnitCodeChanged(string? oldValue, string newValue)
		{
			ResetForceValidationBit("UnitCode");
			if (string.IsNullOrWhiteSpace(newValue))
			{
				return;
			}
			QntUnitCacheRow item = TradeSettings.TradeCacheService.QntUnit.GetItem(UnitCode);
			UnitBrief = ((item == null) ? string.Empty : item.Brief);
			if (string.IsNullOrWhiteSpace(oldValue) && string.IsNullOrWhiteSpace(_oldUnitCode))
			{
				_oldUnitCode = newValue;
				return;
			}
			MaterialCacheRow.QntUnitRow qntUnit = TradeSettings.TradeCacheService.Material.GetQntUnit(MTID, string.IsNullOrWhiteSpace(newValue) ? string.Empty : newValue);
			MaterialCacheRow.QntUnitRow qntUnit2 = TradeSettings.TradeCacheService.Material.GetQntUnit(MTID, string.IsNullOrWhiteSpace(_oldUnitCode) ? oldValue : _oldUnitCode);
			if (qntUnit != null && qntUnit2 != null)
			{
				_oldUnitCode = newValue;
				decimal coef = qntUnit.Coef;
				decimal coef2 = qntUnit2.Coef;
				ActualQuantity = (ActualQuantity * coef2 / coef).RoundAs(3);
				AccQuantity = (AccQuantity * coef2 / coef).RoundAs(3);
			}
			if (ActualQuantity == 0m)
			{
				Price = AccSum;
			}
			else
			{
				Price = (AccSum / AccQuantity).RoundAs(2);
			}
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		private void OnAccQuantityChanged(decimal value)
		{
			VarianceQuantity = ActualQuantity - AccQuantity;
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		private void OnActualQuantityChanged(decimal value)
		{
			ResetForceValidationBit("ActualQuantity");
			VarianceQuantity = ActualQuantity - AccQuantity;
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		private void OnPriceChanged(decimal value)
		{
			ResetForceValidationBit("Price");
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		private void OnSalePriceChanged(decimal value)
		{
			ResetForceValidationBit("SalePrice");
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		private void OnActualSumChanged(decimal value)
		{
			ResetForceValidationBit("ActualSum");
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		private void OnSaleSumChanged(decimal value)
		{
			ResetForceValidationBit("SaleSum");
		}
	}

	public class MTStockTakingFilteredSearchList : IResultList
	{
		private readonly ObservableCollection<MTStockTakingSpecificationRow> _itemSource = new ObservableCollection<MTStockTakingSpecificationRow>();

		public IEnumerable<DocumentGridRow> ItemsSource => _itemSource;

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
					Header = TradeAllDocDefinitions.MTStockTaking.Grids["MTList"].Columns["MTCode"].Caption,
					FieldName = "MTCode",
					CustomWidth = 125.0
				};
				span[1] = new FilteredSearchGridColumn
				{
					Header = TradeAllDocDefinitions.MTStockTaking.Grids["MTList"].Columns["MTName"].Caption,
					FieldName = "MTName",
					CustomWidth = 300.0
				};
				span[2] = new FilteredSearchGridColumn
				{
					Header = TradeAllDocDefinitions.MTStockTaking.Grids["MTList"].Columns["ActualQuantity"].Caption,
					FieldName = "ActualQuantity",
					AllowEditing = true,
					NumericFormat = DecimalExtension.GetCurrencyFormat(13, 3)
				};
				return list;
			}
		}

		public int AddToItemsSource(object key, params object[] args)
		{
			string mtCode = (string)key;
			decimal num = (decimal)args[0];
			int num2 = _itemSource.FindIndex((MTStockTakingSpecificationRow x) => x.MTCode == mtCode);
			if (num2 == -1)
			{
				MaterialCacheRow item = TradeSettings.TradeCacheService.Material.GetItem(mtCode);
				if (item == null)
				{
					return -1;
				}
				MTStockTakingSpecificationRow item2 = new MTStockTakingSpecificationRow
				{
					MTID = item.Id,
					MTCode = item.MTCode,
					MTName = item.Caption,
					UnitCode = item.Unit,
					UserAdd = true
				};
				_itemSource.Add(item2);
				num2 = _itemSource.Count - 1;
			}
			_itemSource[num2].ActualQuantity += num;
			return num2;
		}
	}

	protected DocumentGridControl _mtStockTakingGrid;

	protected TableView _mtStockTakingTableView;

	protected ObservableCollection<MTStockTakingSpecificationRow> _mtStockTakingList;

	protected DropDownControl _storage;

	protected DropDownControl _goodGroup;

	protected TextEditExt _inputValue;

	protected Button _codeEnter;

	protected DropDownControl _currCode;

	protected ExchangeRateControl _currExChngRate;

	protected DropDownControl _owner;

	protected DocumentGridColumn _codeCol;

	protected DocumentGridColumn _nameCol;

	protected DocumentGridColumn _unitCodeCol;

	protected DocumentGridColumn _accQuantityCol;

	protected DocumentGridColumn _actualQuantityCol;

	protected DocumentGridColumn _varianceQuantityCol;

	protected DocumentGridColumn _accSumCol;

	protected DocumentGridColumn _actualSumCol;

	protected DocumentGridColumn _priceCol;

	protected DocumentGridColumn _salePriceCol;

	protected DocumentGridColumn _saleSumCol;

	protected DXTabControl _tabs;

	protected BarEditItemExt _filteredSearch;

	protected BarEditItemExt _loadGoodsList;

	protected BarEditItemExt _refreshGoodsList;

	protected BarButtonItem _barButtonImportFromFile;

	protected BarButtonItem _barButtonExportForTerminal;

	protected ReportBarButtonItem _barButtonJumpTo;

	protected BarSubItem _barSubItemActions = new BarSubItem();

	protected bool _isLoaded;

	protected override double WidthValue => 1240.0;

	protected override double HeightValue => 440.0;

	protected override double MinWidthValue => 600.0;

	protected override double MinHeightValue => 250.0;

	public MTStockTakingWindowBase()
	{
	}

	public MTStockTakingWindowBase(T doc)
		: base(doc)
	{
	}

	protected override void ConstructUI()
	{
		base.ConstructUI();
	}

	protected override void SetControlsReadonly()
	{
		base.SetControlsReadonly();
		if (_viewMode != WindowViewMode.View)
		{
			SetReadOnlyGeneralFields(_mtStockTakingList.Count > 0);
		}
	}

	protected virtual void SetReadOnlyGeneralFields(bool isReadOnly)
	{
		_storage.IsReadOnly = isReadOnly;
		_goodGroup.IsReadOnly = isReadOnly;
	}

	protected override void AddToolbarItems()
	{
		base.AddToolbarItems();
		Ribbon.ToolbarItemLinks.Add(_barButtonJumpTo.ConvertForToolbar("t_doc_JumpTo_16.svg"));
		Ribbon.ToolbarItemLinks.Add(_barButtonImportFromFile.ConvertForToolbar("t_import_16.svg"));
		Ribbon.ToolbarItemLinks.Add(_barButtonExportForTerminal.ConvertForToolbar("t_export_16.svg"));
	}

	protected override void AddItemsToRibbon()
	{
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		//IL_013a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0140: Unknown result type (might be due to invalid IL or missing references)
		//IL_0156: Unknown result type (might be due to invalid IL or missing references)
		//IL_015c: Unknown result type (might be due to invalid IL or missing references)
		base.AddItemsToRibbon();
		CreateRibbonPageGroupActions();
		CreateRibbonPageGroupReports();
		_barButtonJumpTo = new ReportBarButtonItem(DataProviderTypeSE.AllDocumentsJournal, showDialog: false, ArmSoft.SME.Common.Resources.JumpTo, null, "doc_JumpTo_16.svg", "doc_JumpTo_32.svg");
		_barButtonJumpTo.AddInBarSubItem(ref _ribbonPageGroupActions, ref _barSubItemActions);
		_barButtonImportFromFile = new BarButtonItem
		{
			Content = ArmSoft.SME.Trade.Common.Resources.Import,
			KeyGesture = new KeyGesture((Key)52, (ModifierKeys)2),
			Glyph = ImageService.GetImage("Import_16.svg"),
			LargeGlyph = ImageService.GetImage("Import_32.svg")
		};
		_barManager.Items.Add(_barButtonImportFromFile);
		_ribbonPageGroupActions.ItemLinks.Add(_barButtonImportFromFile.CreateLink());
		_barButtonImportFromFile.ItemClick += new SafeDocumentEventHandler<ItemClickEventArgs>(BarButtonImportFromFile_ItemClick, this).Handler;
		Ribbon.ToolbarItemLinks.Add(_barButtonImportFromFile.CreateLink());
		_barButtonExportForTerminal = new BarButtonItem
		{
			Content = ArmSoft.SME.Trade.Common.Resources.ExportForTerminal,
			Glyph = ImageService.GetImage("export_16.svg"),
			LargeGlyph = ImageService.GetImage("export_32.svg")
		};
		_barManager.Items.Add(_barButtonExportForTerminal);
		_ribbonPageGroupActions.ItemLinks.Add(_barButtonExportForTerminal.CreateLink());
		_barButtonExportForTerminal.ItemClick += new SafeDocumentEventHandler<ItemClickEventArgs>(BarButtonExportForTerminal_ItemClick, this).Handler;
		Ribbon.ToolbarItemLinks.Add(_barButtonExportForTerminal.CreateLink());
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
	}

	protected override void LoadDataToControls()
	{
		base.LoadDataToControls();
		_mtStockTakingList.Clear();
		foreach (MTStockTakingBase.MTStockTakingSpecificationRow mT in PO.MTList)
		{
			MTStockTakingSpecificationRow item = new MTStockTakingSpecificationRow
			{
				MTCode = mT.MTCode,
				MTName = mT.MTName,
				MTID = mT.MTID,
				AccQuantity = mT.AccQuantity,
				ActualQuantity = mT.ActualQuantity,
				VarianceQuantity = mT.VarianceQuantity,
				AccSum = mT.AccSum,
				ActualSum = mT.ActualSum,
				SalePrice = mT.SalePrice,
				Price = mT.Price,
				SaleSum = mT.SaleSum,
				RowNum = mT.RowNum,
				FullName = mT.FullName,
				UnitCode = mT.UnitCode
			};
			_mtStockTakingList.Add(item);
		}
		_mtStockTakingGrid.ItemsSource = _mtStockTakingList;
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

	protected override void FillDocument(StoreRequest sr)
	{
		base.FillDocument(sr);
		PO.MTList.Clear();
		foreach (MTStockTakingSpecificationRow mtStockTaking in _mtStockTakingList)
		{
			MTStockTakingBase.MTStockTakingSpecificationRow item = new MTStockTakingBase.MTStockTakingSpecificationRow
			{
				MTID = mtStockTaking.MTID,
				MTCode = mtStockTaking.MTCode,
				MTName = mtStockTaking.MTName,
				AccQuantity = mtStockTaking.AccQuantity,
				ActualQuantity = mtStockTaking.ActualQuantity,
				VarianceQuantity = mtStockTaking.VarianceQuantity,
				AccSum = mtStockTaking.AccSum,
				ActualSum = mtStockTaking.ActualSum,
				Price = mtStockTaking.Price,
				SalePrice = mtStockTaking.SalePrice,
				SaleSum = mtStockTaking.SaleSum,
				RowNum = mtStockTaking.RowNum,
				UnitCode = mtStockTaking.UnitCode,
				UnitBrief = mtStockTaking.UnitBrief
			};
			PO.MTList.Add(item);
		}
	}

	protected override void SetReportBarButtonsEnable(bool isEnabled)
	{
		base.SetReportBarButtonsEnable(isEnabled);
		_barButtonJumpTo.IsEnabled = isEnabled;
		_barButtonImportFromFile.IsEnabled = IsEnabled;
		_barButtonExportForTerminal.IsEnabled = IsEnabled;
	}

	private void BarButtonImportFromFile_ItemClick(object sender, ItemClickEventArgs e)
	{
		if (_date.IsReadOnly || !_date.DoValidate())
		{
			return;
		}
		if (string.IsNullOrWhiteSpace(_storage.Code))
		{
			throw new ASException(ArmSoft.SME.Trade.Common.Resources.ErrorStorageFromFile);
		}
		if (_mtStockTakingList.Count <= 0 || ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Trade.Common.Resources.LoadMaterialsList, Settings.ProgramName, MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No) != MessageBoxResult.No)
		{
			_mtStockTakingList.Clear();
			TerminalHelper.TerminalImporter terminalImporter = new TerminalHelper.TerminalImporter(this);
			if (terminalImporter.ImportFromFile((decimal quantity, MaterialCacheRow goodRow, MaterialCacheRow.BarCodeRow goodBarcodeRow) =>
			{
				AddMTToList(quantity, goodRow, goodBarcodeRow);
			}, allowZeroQuantity: true))
			{
				AddMissingAvailabilityRows();
				terminalImporter.ShowReport();
			}
		}
	}

	private void BarButtonExportForTerminal_ItemClick(object sender, ItemClickEventArgs e)
	{
		new TerminalExportWindow(GetSourceList()).ShowDialog();
	}

	private List<TerminalRow> GetSourceList()
	{
		List<TerminalRow> list = new List<TerminalRow>();
		if (_mtStockTakingList == null)
		{
			return list;
		}
		foreach (MTStockTakingSpecificationRow mtStockTaking in _mtStockTakingList)
		{
			TerminalRow item = new TerminalRow
			{
				Id = mtStockTaking.MTID,
				Code = mtStockTaking.MTCode,
				Name = mtStockTaking.MTName,
				Unit = mtStockTaking.UnitCode,
				AlternativeUnit = string.Empty,
				BarCode = null,
				Quantity = mtStockTaking.AccQuantity
			};
			list.Add(item);
		}
		return list;
	}

	protected abstract void AddMissingAvailabilityRows();

	protected abstract void AddMTToList(decimal actualQuantity, MaterialCacheRow goodRow, MaterialCacheRow.BarCodeRow goodBarcodeRow);

	protected abstract void AddMTToList(MaterialCacheRow goodsRow, string unitCode = "", string unitBrief = "", decimal quantity = 1m);

	protected abstract void AddMTToListNewRow(MaterialCacheRow goodsRow, string unitCode, decimal quantity = 1m);

	protected abstract void SetControlsState(bool ignore_IsLoad = false);

	protected void Storage_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		if (_storage.DoValidate() && !TradeSettings.CreateTradeClient().CommonServices.IsStorageAvailable(_storage.Code))
		{
			_storage.Code = "";
			throw new ASException(string.Format(ArmSoft.SME.Trade.Common.Resources.StorageNotAvailForUser, Settings.LoginService.UserName, _storage.Code));
		}
	}

	protected void LoadGoodsList_Click(object sender, RoutedEventArgs e)
	{
		ValidateDocFields();
		if (_mtStockTakingList.Count > 0)
		{
			if (ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Trade.Common.Resources.LoadMaterialsList, Settings.ProgramName, MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.Yes) == MessageBoxResult.No)
			{
				return;
			}
			_mtStockTakingList.Clear();
		}
		LoadGoodsList();
		_isLoaded = _mtStockTakingList.Count > 0;
		SetControlsState();
	}

	protected void RefreshGoodsList_Click(object sender, RoutedEventArgs e)
	{
		ValidateDocFields();
		if (_mtStockTakingList.Count == 0)
		{
			LoadGoodsList();
		}
		else if (_isLoaded)
		{
			RefreshGoodsList();
		}
		SetControlsState(ignore_IsLoad: true);
	}

	protected abstract void RefreshGoodsList();

	protected abstract void LoadGoodsList();

	protected void MTStockTakingGrid_AddingRow(object sender, DocumentGridAddingRowEventArgs e)
	{
		ValidateDocFields();
		_mtStockTakingList.Add(new MTStockTakingSpecificationRow
		{
			AccQuantity = 0m,
			AccSum = 0m,
			UserAdd = true
		});
		e.RowNumber = _mtStockTakingList.Count - 1;
		if (_mtStockTakingList.Count > 0)
		{
			SetReadOnlyGeneralFields(isReadOnly: true);
		}
	}

	protected virtual void MTStockTakingGrid_DeletedRow(object sender, EventArgs e)
	{
		_storage.IsReadOnly = _mtStockTakingList.Count > 0;
		_goodGroup.IsReadOnly = _storage.IsReadOnly;
	}

	protected void MTStockTakingGrid_MovingRowUp(object sender, DocumentGridMovingRowUpOrDownEventArgs e)
	{
		if (_mtStockTakingGrid.Grid.SortInfo.Count > 0 || _mtStockTakingTableView.Grid.VisibleRowCount != _mtStockTakingList.Count)
		{
			throw new ASException(ArmSoft.SME.Common.Resources.NotAllowRowMoveInSortedGrid);
		}
		MTStockTakingSpecificationRow mTStockTakingSpecificationRow = (MTStockTakingSpecificationRow)_mtStockTakingGrid.Grid.GetRow(e.RowHandle);
		if (mTStockTakingSpecificationRow != null)
		{
			_mtStockTakingList.RemoveAt(e.RowHandle);
			_mtStockTakingList.Insert(e.RowHandle - 1, mTStockTakingSpecificationRow);
		}
	}

	protected void MTStockTakingGrid_MovingRowDown(object sender, DocumentGridMovingRowUpOrDownEventArgs e)
	{
		if (_mtStockTakingGrid.Grid.SortInfo.Count > 0 || _mtStockTakingTableView.Grid.VisibleRowCount != _mtStockTakingList.Count)
		{
			throw new ASException(ArmSoft.SME.Common.Resources.NotAllowRowMoveInSortedGrid);
		}
		MTStockTakingSpecificationRow mTStockTakingSpecificationRow = (MTStockTakingSpecificationRow)_mtStockTakingGrid.Grid.GetRow(e.RowHandle);
		if (mTStockTakingSpecificationRow != null)
		{
			_mtStockTakingList.RemoveAt(e.RowHandle);
			_mtStockTakingList.Insert(e.RowHandle + 1, mTStockTakingSpecificationRow);
		}
	}

	protected void MTStockTakingGrid_ClickDropDown(object sender, DocumentGridClickDropDownEventArgs e)
	{
		if (e.Column != _unitCodeCol)
		{
			return;
		}
		MTStockTakingSpecificationRow mTStockTakingSpecificationRow = (MTStockTakingSpecificationRow)_mtStockTakingGrid.Grid.GetRow(e.RowHandle);
		if (mTStockTakingSpecificationRow != null)
		{
			MTQntUnits mTQntUnits = new MTQntUnits(mTStockTakingSpecificationRow.MTCode);
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

	protected void MTStockTakingTableView_FocusedRowHandleChanged(object sender, FocusedRowHandleChangedEventArgs e)
	{
		MTStockTakingSpecificationRow mTStockTakingSpecificationRow = (MTStockTakingSpecificationRow)_mtStockTakingTableView.Grid.GetRow(_mtStockTakingTableView.FocusedRowHandle);
		if (mTStockTakingSpecificationRow != null)
		{
			ChangeColumnAdditonalView(mTStockTakingSpecificationRow.MTCode);
		}
	}

	protected void ChangeColumnAdditonalView(string itemCode)
	{
		_unitCodeCol.DataView = new MTQntUnits(string.IsNullOrWhiteSpace(itemCode) ? string.Empty : itemCode);
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

	protected void BarCode_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Invalid comparison between Unknown and I4
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		if ((int)e.Key == 6 && (int)Keyboard.Modifiers == 0 && !_inputValue.IsReadOnly && !string.IsNullOrWhiteSpace(_inputValue.Text))
		{
			ScannerAdd();
		}
	}

	private void ScannerAdd()
	{
		_inputValue.DoValidate();
		try
		{
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
			string barCode = _inputValue.Text.Trim();
			MaterialCacheRow materialCacheRow = DocumentHelpers.TryGetItemByBarCode(ref barCode, out var qty, out var unitCode, out var unitBrief);
			if (materialCacheRow == null || materialCacheRow.Type == ItemTypesSE.Services.Value)
			{
				ControlsHelper.SetFocus(_inputValue);
				throw new ASException(string.Format(ArmSoft.SME.Common.Resources.ErrBarCodeNotExist, barCode));
			}
			AddMTToList(materialCacheRow, unitCode, unitBrief, qty);
			_inputValue.Text = "";
			SetReadOnlyGeneralFields(isReadOnly: true);
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	protected override void OnF8()
	{
		CodeEnterClick();
	}

	private void CodeEnterClick()
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
					throw new ASException(ArmSoft.SME.Common.Resources.ValidateEmptyDate);
				}
				GoodsFilter goodsFilter = new GoodsFilter(shortMode: true, ItemTypesSE.Goods.Value);
				ModalBrowser modalBrowser = new ModalBrowser(goodsFilter)
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
				text = goodsFilter.ValueMember(modalBrowser.SelectedRowIndex);
			}
			if (text.Length > 20)
			{
				throw new ASException(string.Format(ArmSoft.SME.Trade.Common.Resources.ErrMTCodeLen, 20, text));
			}
			MaterialCacheRow item = TradeSettings.TradeCacheService.Material.GetItem(text);
			if (item == null)
			{
				ControlsHelper.SetFocus(_inputValue);
				throw new ASException(string.Format(ArmSoft.SME.Trade.Common.Resources.MTEgineInavidMaterial, text));
			}
			AddMTToList(item);
			SetReadOnlyGeneralFields(isReadOnly: true);
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	protected void CodeEnter_Click(object sender, RoutedEventArgs e)
	{
		CodeEnterClick();
	}

	protected virtual void ValidateDocFields()
	{
		if (!_date.DoValidate())
		{
			ControlsHelper.SetFocus(_date);
			throw new ASException(ArmSoft.SME.Common.Resources.ValidateEmptyDate);
		}
		if (string.IsNullOrWhiteSpace(_storage.Code))
		{
			ControlsHelper.SetFocus(_storage);
			throw new ASException(ArmSoft.SME.Common.Resources.ValidateEmptyStorage);
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

	protected override void StoreSettings(RegistryKey rk)
	{
		base.StoreSettings(rk);
		_mtStockTakingGrid.StoreSettings(rk);
	}

	protected override void RestoreSettings(double defaultWidth, double defaultHeight, RegistryKey rk)
	{
		base.RestoreSettings(defaultWidth, defaultHeight, rk);
		_mtStockTakingGrid.RestoreSettings(rk);
	}
}
