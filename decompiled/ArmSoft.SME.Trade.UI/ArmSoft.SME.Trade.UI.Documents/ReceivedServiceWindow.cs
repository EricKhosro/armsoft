using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using Ardalis.SmartEnum;
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
using ArmSoft.SME.Trade.Client.Data.Directories;
using ArmSoft.SME.Trade.Common;
using ArmSoft.SME.Trade.Common.Documents.Definition;
using ArmSoft.SME.Trade.Common.Models.Caches;
using ArmSoft.SME.Trade.Common.Models.Documents;
using ArmSoft.SME.Trade.Common.Parameters;
using ArmSoft.SME.Trade.Common.PersistableObject;
using ArmSoft.SME.Trade.Common.SmartEnums;
using ArmSoft.SME.Trade.UI.CustomUIElements;
using ArmSoft.SME.Trade.UI.DropDown;
using ArmSoft.SME.Trade.UI.DropDownDataProvider;
using ArmSoft.SME.Trade.UI.Helpers;
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

[DocumentWindow(29)]
public class ReceivedServiceWindow : TradeDocumentWindow<ReceivedService>
{
	public class ReceivedServiceSpecificationRow : DocumentGridRow
	{
		[ObservableProperty]
		private string servCode = string.Empty;

		[ObservableProperty]
		private string servName = string.Empty;

		[ObservableProperty]
		private int servID = -1;

		[ObservableProperty]
		private decimal quantity = 1m;

		[ObservableProperty]
		private decimal price;

		[ObservableProperty]
		private decimal priceVAT;

		[ObservableProperty]
		private bool vAT;

		[ObservableProperty]
		private decimal summa;

		[ObservableProperty]
		private short rowNum;

		[ObservableProperty]
		private string fullName = string.Empty;

		[ObservableProperty]
		private string unitCode = string.Empty;

		[ObservableProperty]
		private string unitBrief = string.Empty;

		[ObservableProperty]
		private bool unitFraction;

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public string ServCode
		{
			get
			{
				return servCode;
			}
			[MemberNotNull("servCode")]
			set
			{
				if (!EqualityComparer<string>.Default.Equals(servCode, value))
				{
					OnPropertyChanging(__KnownINotifyPropertyChangingArgs.ServCode);
					servCode = value;
					OnServCodeChanged(value);
					OnPropertyChanged(__KnownINotifyPropertyChangedArgs.ServCode);
				}
			}
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public string ServName
		{
			get
			{
				return servName;
			}
			[MemberNotNull("servName")]
			set
			{
				if (!EqualityComparer<string>.Default.Equals(servName, value))
				{
					OnPropertyChanging(__KnownINotifyPropertyChangingArgs.ServName);
					servName = value;
					OnServNameChanged(value);
					OnPropertyChanged(__KnownINotifyPropertyChangedArgs.ServName);
				}
			}
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public int ServID
		{
			get
			{
				return servID;
			}
			set
			{
				if (!EqualityComparer<int>.Default.Equals(servID, value))
				{
					OnPropertyChanging(__KnownINotifyPropertyChangingArgs.ServID);
					servID = value;
					OnPropertyChanged(__KnownINotifyPropertyChangedArgs.ServID);
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
		public decimal PriceVAT
		{
			get
			{
				return priceVAT;
			}
			set
			{
				if (!EqualityComparer<decimal>.Default.Equals(priceVAT, value))
				{
					OnPropertyChanging(__KnownINotifyPropertyChangingArgs.PriceVAT);
					priceVAT = value;
					OnPropertyChanged(__KnownINotifyPropertyChangedArgs.PriceVAT);
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
		public bool UnitFraction
		{
			get
			{
				return unitFraction;
			}
			set
			{
				if (!EqualityComparer<bool>.Default.Equals(unitFraction, value))
				{
					OnPropertyChanging(__KnownINotifyPropertyChangingArgs.UnitFraction);
					unitFraction = value;
					OnPropertyChanged(__KnownINotifyPropertyChangedArgs.UnitFraction);
				}
			}
		}

		static ReceivedServiceSpecificationRow()
		{
			DocumentGridRow.RegisterPropertyValidators(typeof(ReceivedServiceSpecificationRow), cacheValidationResults: true, new Dictionary<string, Action<DocumentGridRow, DevExpress.XtraEditors.DXErrorProvider.ErrorInfo>>
			{
				{
					"ServCode",
					(DocumentGridRow row, DevExpress.XtraEditors.DXErrorProvider.ErrorInfo info) =>
					{
						ValidateServCode((ReceivedServiceSpecificationRow)row, info);
					}
				},
				{
					"UnitCode",
					(DocumentGridRow row, DevExpress.XtraEditors.DXErrorProvider.ErrorInfo info) =>
					{
						ValidateUnitCode((ReceivedServiceSpecificationRow)row, info);
					}
				},
				{
					"Quantity",
					(DocumentGridRow row, DevExpress.XtraEditors.DXErrorProvider.ErrorInfo info) =>
					{
						ValidateQuantity((ReceivedServiceSpecificationRow)row, info);
					}
				},
				{
					"Price",
					(DocumentGridRow row, DevExpress.XtraEditors.DXErrorProvider.ErrorInfo info) =>
					{
						ValidatePrice((ReceivedServiceSpecificationRow)row, info);
					}
				},
				{
					"Summa",
					(DocumentGridRow row, DevExpress.XtraEditors.DXErrorProvider.ErrorInfo info) =>
					{
						ValidateSumma((ReceivedServiceSpecificationRow)row, info);
					}
				}
			});
		}

		private static void ValidateServCode(ReceivedServiceSpecificationRow row, DevExpress.XtraEditors.DXErrorProvider.ErrorInfo info)
		{
			TradePropertyValidator.ValidateServiceCode(row.ServCode, info, TradeAllDocDefinitions.ReceivedService.Grids["ReceivedServiceList"].Columns["ServCode"]);
		}

		private static void ValidateUnitCode(ReceivedServiceSpecificationRow row, DevExpress.XtraEditors.DXErrorProvider.ErrorInfo info)
		{
			TradePropertyValidator.ValidateGoodUnitCode(row.UnitCode, row.ServCode, info, TradeAllDocDefinitions.ReceivedService.Grids["ReceivedServiceList"].Columns["UnitCode"]);
		}

		private static void ValidateQuantity(ReceivedServiceSpecificationRow row, DevExpress.XtraEditors.DXErrorProvider.ErrorInfo info)
		{
			PropertyValidator.PositiveValidate(row.Quantity, info, ArmSoft.SME.Common.Resources.Quantity);
		}

		private static void ValidatePrice(ReceivedServiceSpecificationRow row, DevExpress.XtraEditors.DXErrorProvider.ErrorInfo info)
		{
			PropertyValidator.PositiveValidate(row.Price, info, ArmSoft.SME.Common.Resources.Price);
		}

		private static void ValidateSumma(ReceivedServiceSpecificationRow row, DevExpress.XtraEditors.DXErrorProvider.ErrorInfo info)
		{
			PropertyValidator.PositiveValidate(row.Summa, info, ArmSoft.SME.Common.Resources.Summa);
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		private void OnServCodeChanged(string value)
		{
			ResetForceValidationBit("ServCode", "ServName", "UnitCode");
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		private void OnServNameChanged(string value)
		{
			ResetForceValidationBit("ServName");
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
			ResetForceValidationBit("Summa", "Price");
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		private void OnUnitCodeChanged(string value)
		{
			ResetForceValidationBit("UnitCode");
			QntUnitCacheRow item = TradeSettings.TradeCacheService.QntUnit.GetItem(UnitCode);
			UnitBrief = ((item == null) ? string.Empty : item.Brief);
		}
	}

	public class ReceivedServicesFilteredSearchList : IResultList
	{
		private readonly ObservableCollection<ReceivedServiceSpecificationRow> _itemSource = new ObservableCollection<ReceivedServiceSpecificationRow>();

		public List<FilteredSearchGridColumn> Columns => new List<FilteredSearchGridColumn>
		{
			new FilteredSearchGridColumn
			{
				Header = TradeAllDocDefinitions.ReceivedService.Grids["ReceivedServiceList"].Columns["ServCode"].Caption,
				FieldName = "ServCode",
				CustomWidth = 125.0
			},
			new FilteredSearchGridColumn
			{
				Header = TradeAllDocDefinitions.ReceivedService.Grids["ReceivedServiceList"].Columns["ServName"].Caption,
				FieldName = "ServName",
				CustomWidth = 300.0
			},
			new FilteredSearchGridColumn
			{
				Header = TradeAllDocDefinitions.ReceivedService.Grids["ReceivedServiceList"].Columns["Quantity"].Caption,
				FieldName = "Quantity",
				AllowEditing = true,
				NumericFormat = DecimalExtension.GetCurrencyFormat(13, 3)
			}
		};

		public IEnumerable<DocumentGridRow> ItemsSource => _itemSource;

		public int AddToItemsSource(object key, params object[] args)
		{
			string servCode = (string)key;
			decimal num = (decimal)args[0];
			int num2 = _itemSource.FindIndex((ReceivedServiceSpecificationRow x) => x.ServCode == servCode);
			if (num2 == -1)
			{
				MaterialCacheRow item = TradeSettings.TradeCacheService.Material.GetItem(servCode);
				if (item == null)
				{
					return -1;
				}
				ReceivedServiceSpecificationRow item2 = new ReceivedServiceSpecificationRow
				{
					ServID = item.Id,
					ServCode = item.MTCode,
					ServName = item.Caption,
					UnitCode = item.Unit,
					VAT = item.VAT,
					Quantity = 0m
				};
				_itemSource.Add(item2);
				num2 = _itemSource.Count - 1;
			}
			_itemSource[num2].Quantity += num;
			return num2;
		}
	}

	private PartnerContractGroupControls _supplierContractContainer;

	protected TextEditExt _invoiceNumber;

	protected ComboBoxEditExt _comment;

	private DropDownControl _owner;

	private DropDownControl _currCode;

	private ExchangeRateControl _currExChngRate;

	private DropDownControl _vatType;

	private DropDownControl _priceListType;

	private DocumentGridControl _receivedServiceGrid;

	private TableView _receivedServiceTableView;

	private BarEditItemExt _filteredSearch;

	private ObservableCollection<ReceivedServiceSpecificationRow> _receivedServicesList;

	private DocumentGridColumn _servCodeCol;

	private DocumentGridColumn _servNameCol;

	private DocumentGridColumn _unitCodeCol;

	private DocumentGridColumn _unitBriefCol;

	private DocumentGridColumn _quantityCol;

	private DocumentGridColumn _priceCol;

	private DocumentGridColumn _summaCol;

	private DocumentGridColumn _vatCol;

	private PopupCalcEditExt _vatSumma;

	private PopupCalcEditExt _totalSumma;

	private string _oldDocCurrCode;

	private decimal _oldDocCurrExChngRate = 1m;

	private decimal _oldDocCurrExChngBase = 1m;

	private ReportBarButtonItem _barButtonJumpTo;

	private ReportBarButtonItem _barButtonPartnerOperationJournal;

	private BarSubItem _barSubItemActions = new BarSubItem();

	private BarSubItem _barSubItemJournals = new BarSubItem();

	private ExchangeRate _exchangeRate = new ExchangeRate();

	protected Document _baseDocument;

	protected Guid _baseISN = Guid.Empty;

	protected override double WidthValue => 700.0;

	protected override double HeightValue => 370.0;

	protected override double MinWidthValue => 620.0;

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
		}
	}

	public ReceivedServiceWindow()
	{
	}

	public ReceivedServiceWindow(ReceivedService doc)
		: base(doc)
	{
	}

	protected override void ConstructUI()
	{
		base.ConstructUI();
		LayoutGroup container = AddLayoutGroup(null, LayoutGroupView.Group, Orientation.Horizontal);
		_date = AddDateControl(container, Definition.Fields["DocDate"]);
		_date.EditValueChanged += new SafeDocumentEventHandler<EditValueChangedEventArgs>(Date_EditValueChanged, this).Handler;
		_docNum = AddButtonEditControl(container, Definition.Fields["DocNum"], 1);
		_docNum.AllowMagicButton = true;
		_docNum.MagicToolTip = ArmSoft.SME.Common.Resources.DocNextNumbers;
		_docNum.MagicClick += new SafeDocumentEventHandler<MagicClickEventArgs>(DocNum_MagicClick, this).Handler;
		_docNum.EditValueChanged += new SafeDocumentEventHandler<EditValueChangedEventArgs>(DocNum_EditValueChanged, this).Handler;
		DXTabControl tc = AddTabControl(null);
		LayoutGroup container2 = AddTabbedGroup(tc, ArmSoft.SME.Common.Resources.General, "General");
		_supplierContractContainer = AddPartnerContractGroupControls(container2, (PartnerContractGroupDefinition)Definition.Fields["PartnerContractGroup"], _date, _currCode, _totalSumma);
		_supplierContractContainer.PartnerCodeEditValueChanged += new SafeDocumentEventHandler<EditValueChangedEventArgs>(PartnerCodeEditValueChanged, this).Handler;
		_supplierContractContainer.PartnerCodeEditAdditionalValueChanged += new SafeDocumentEventHandler<EventArgs>(PartnerCode_EditAdditionalValueChanged, this).Handler;
		_supplierContractContainer.ContractCodeEditValueChanged += new SafeDocumentEventHandler<EventArgs>(ContractCodeEditValueChanged, this).Handler;
		_invoiceNumber = AddStringControl(container2, Definition.Fields["InvoiceNumber"]);
		_comment = AddComboBoxControl(container2, Definition.Fields["Comment"], TradeSettings.TradeCacheService.StandardComments.GetCaptionsList(), HorizontalAlignment.Stretch);
		_owner = AddDropDownControl(container2, Definition.Fields["Owner"], new UsersFilter());
		_owner.CodeWidth = 12;
		_owner.ShowComment = false;
		_owner.InnerCodeEdit.Width = _owner.InnerCodeEdit.MinWidth;
		if (!Settings.LoginService.IsAdmin && Settings.ParametersService.DenyAccessToOtherOwnerDocs())
		{
			((LayoutItemWithRequired)(object)_owner.Parent).Visibility = Visibility.Collapsed;
		}
		LayoutGroup container3 = AddTabbedGroup(tc, ArmSoft.SME.Common.Resources.ItemList, "PriceList");
		LayoutGroup container4 = AddLayoutGroup(container3, LayoutGroupView.Group, Orientation.Horizontal);
		_currCode = AddDropDownControl(container4, Definition.Fields["CurrCode"], new Currency(shortMode: true), allowAdditionButtons: false, fieldWithCaption: true, HorizontalAlignment.Stretch);
		_currCode.EditValueChanged += new SafeDocumentEventHandler<EditValueChangedEventArgs>(CurrCode_EditValueChanged, this).Handler;
		_vatSumma = AddNumericControl(container4, Definition.Fields["SummVAT"], useCurrencyFormat: true, HorizontalAlignment.Right);
		LayoutGroup container5 = AddLayoutGroup(container3, LayoutGroupView.Group, Orientation.Horizontal);
		_currExChngRate = AddExchangeRateControl(container5, Definition.Fields["ExchangeRate"]);
		_currExChngRate.EditValueChanged += new SafeDocumentEventHandler<RateEditValueChangedEventArgs>(CurrExChngRate_EditValueChanged, this).Handler;
		_totalSumma = AddNumericControl(container5, Definition.Fields["Summ"], useCurrencyFormat: true, HorizontalAlignment.Right);
		_vatType = AddDropDownControl(container3, Definition.Fields["VATType"], new VATTypes(), allowAdditionButtons: false, fieldWithCaption: true, HorizontalAlignment.Stretch);
		_vatType.EditValueChanged += new SafeDocumentEventHandler<EditValueChangedEventArgs>(VATType_EditValueChanged, this).Handler;
		_priceListType = AddDropDownControl(container3, Definition.Fields["PriceType"], new PriceTypes());
		_priceListType.EditValueChanged += new SafeDocumentEventHandler<EditValueChangedEventArgs>(PriceListType_EditValueChanged, this).Handler;
		_receivedServiceGrid = AddGrid(container3, Definition.Grids["ReceivedServiceList"], HorizontalAlignment.Stretch, VerticalAlignment.Stretch);
		_receivedServiceGrid.AddingRow += new SafeDocumentEventHandler<DocumentGridAddingRowEventArgs>(ReceivedServiceGrid_AddingRow, this).Handler;
		_receivedServiceGrid.DuplicatingRow += new SafeDocumentEventHandler<DocumentGridDuplicatingRowEventArgs>(ReceivedServiceGrid_DuplicatingRow, this).Handler;
		_receivedServiceGrid.DeletedRow += new SafeDocumentEventHandler<EventArgs>(ReceivedServiceGrid_DeletedRow, this).Handler;
		_receivedServiceGrid.ClickDropDown += new SafeDocumentEventHandler<DocumentGridClickDropDownEventArgs>(ReceivedServiceGrid_ClickDropDown, this).Handler;
		_receivedServiceTableView = _receivedServiceGrid.View;
		_receivedServiceTableView.CellValueChanged += new SafeDocumentEventHandler<CellValueChangedEventArgs>(ReceivedServiceTableView_CellValueChanged, this).Handler;
		_receivedServiceTableView.FocusedRowHandleChanged += new SafeDocumentEventHandler<FocusedRowHandleChangedEventArgs>(ReceivedServiceTableView_FocusedRowHandleChangedEventArgs, this).Handler;
		_receivedServiceGrid.AddRowIndicator(35);
		_filteredSearch = _receivedServiceGrid.AddButton("", ArmSoft.SME.Trade.Common.Resources.GroupAddition, ArmSoft.SME.Trade.Common.Resources.GroupAddition, 130.0);
		_filteredSearch.Click += new SafeDocumentEventHandler<RoutedEventArgs>(FilteredSearch_Click, this).Handler;
		_servCodeCol = _receivedServiceGrid.AddDropDownColumn(Definition.Grids["ReceivedServiceList"].Columns["ServCode"], new GoodsFilter(shortMode: true, ItemTypesSE.Services, "", isShowOnlyInPriceList: true), allowAdditionButtons: true);
		_servCodeCol.Width = 125.0;
		_servNameCol = _receivedServiceGrid.AddStringColumn(Definition.Grids["ReceivedServiceList"].Columns["ServName"]);
		_servNameCol.Width = 225.0;
		_unitCodeCol = _receivedServiceGrid.AddDropDownColumn(Definition.Grids["ReceivedServiceList"].Columns["UnitCode"], new MTQntUnits("", isShowQntUnitCaption: false, isShowOnlyComplects: false, "", ItemTypesSE.Services.Value), allowAdditionButtons: false, isOwnProcessingClickDropDown: true);
		_unitBriefCol = _receivedServiceGrid.AddStringColumn(Definition.Grids["ReceivedServiceList"].Columns["UnitBrief"]);
		_unitBriefCol.AllowEditing = DefaultBoolean.False;
		_quantityCol = _receivedServiceGrid.AddNumberColumn(Definition.Grids["ReceivedServiceList"].Columns["Quantity"], showTotal: true);
		_priceCol = _receivedServiceGrid.AddNumberColumn(Definition.Grids["ReceivedServiceList"].Columns["Price"]);
		_summaCol = _receivedServiceGrid.AddNumberColumn(Definition.Grids["ReceivedServiceList"].Columns["Summa"], showTotal: true);
		_vatCol = _receivedServiceGrid.AddBooleanColumn(Definition.Grids["ReceivedServiceList"].Columns["VAT"]);
		_vatCol.Width = 45.0;
		_receivedServicesList = new ObservableCollection<ReceivedServiceSpecificationRow>();
		_supplierContractContainer.DocSummaControl = _totalSumma;
		_supplierContractContainer.DocCurrCodeControl = _currCode;
		foreach (GridColumn column in _receivedServiceGrid.Grid.Columns)
		{
			column.AllowSorting = DefaultBoolean.True;
			column.AllowColumnFiltering = DefaultBoolean.True;
			column.FilterPopupMode = FilterPopupMode.CheckedList;
		}
	}

	protected override void OnNew()
	{
		base.OnNew();
		_owner.Code = Settings.LoginService.UserName;
		if (PO.BaseISN == Guid.Empty)
		{
			_date.EditValue = Settings.HostingEnvironment.CurrentDate;
		}
		if (_viewMode == WindowViewMode.Copy)
		{
			return;
		}
		_currCode.Code = "AMD";
		if (string.IsNullOrWhiteSpace(_priceListType.Code) && _baseDocument == null)
		{
			_priceListType.Code = Settings.ParametersService.DefaultBuyingPrice();
		}
		if (string.IsNullOrWhiteSpace(_vatType.Code) && !string.IsNullOrWhiteSpace(Settings.ParametersService.VATType()))
		{
			_vatType.Code = Settings.ParametersService.VATType();
		}
		string text = Settings.ParametersService.DefaultSupplier();
		if (string.IsNullOrWhiteSpace(_supplierContractContainer.PartnerCode.Code) && !string.IsNullOrWhiteSpace(text))
		{
			PartnerCacheRow item = TradeSettings.TradeCacheService.Partner.GetItem(text);
			if (item != null && item.Supplier)
			{
				_supplierContractContainer.PartnerCode.Code = text;
			}
		}
	}

	protected override void OnNewAfterSave()
	{
		base.OnNewAfterSave();
		_receivedServicesList.Clear();
		_owner.Code = Settings.LoginService.UserName;
		CalcSummAndSummVAT();
		_supplierContractContainer.CalculatePartDebtRem();
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
		_baseISN = PO.BaseISN;
		if (_baseISN == Guid.Empty)
		{
			BaseDocument = null;
		}
		else
		{
			BaseDocument = Settings.DocumentService.Load(_baseISN);
		}
		ChangedCurrCodeOrDate();
		_receivedServicesList.Clear();
		foreach (ReceivedService.ReceivedServiceSpecificationRow receivedService in PO.ReceivedServiceList)
		{
			ReceivedServiceSpecificationRow item = new ReceivedServiceSpecificationRow
			{
				ServCode = receivedService.ServCode,
				ServName = receivedService.ServName,
				ServID = receivedService.ServID,
				Quantity = receivedService.Quantity,
				Price = receivedService.Price,
				PriceVAT = receivedService.PriceVAT,
				VAT = receivedService.VAT,
				Summa = receivedService.Summa,
				RowNum = receivedService.RowNum,
				FullName = receivedService.FullName,
				UnitCode = receivedService.UnitCode,
				UnitBrief = receivedService.UnitBrief,
				UnitFraction = receivedService.UnitFraction
			};
			_receivedServicesList.Add(item);
		}
		_receivedServiceGrid.ItemsSource = _receivedServicesList;
		if (!(_baseISN == Guid.Empty) && !StoredInDb && _viewMode != WindowViewMode.Copy)
		{
			CreateBasedOnDoc();
		}
		_supplierContractContainer.ValueChangedForLoadDoc();
		VATType_EditValueChangedForLoad();
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

	protected override void SetControlsReadonly()
	{
		base.SetControlsReadonly();
		if (_viewMode != WindowViewMode.View)
		{
			_vatSumma.IsReadOnly = true;
			_totalSumma.IsReadOnly = true;
			bool flag = _viewMode == WindowViewMode.Edit && PO.State != (byte)DocumentStateSE.Draft && PO.State != (byte)DocumentStateSE.Deleted;
			_date.IsReadOnly = flag;
			_currCode.IsReadOnly = flag;
			_currExChngRate.IsReadOnly = flag || _currCode.Code == "AMD";
			_vatType.IsReadOnly = flag;
			_priceListType.IsReadOnly = flag;
			_receivedServiceGrid.IsReadOnly = flag;
		}
	}

	private void CalcSummAndSummVAT()
	{
		decimal num = 0m;
		decimal num2 = 0m;
		if (_vatType.Code.Trim().InList("1", "2"))
		{
			foreach (ReceivedServiceSpecificationRow receivedServices in _receivedServicesList)
			{
				num += PriceHelper.CalcVATAmountByType(receivedServices.Summa, _vatType.Code, receivedServices.VAT);
				num2 += receivedServices.Summa;
			}
			_vatSumma.EditValue = num;
			_totalSumma.EditValue = num2;
			if (_vatType.Code.Trim() == "1")
			{
				_totalSumma.EditValue = (decimal)_totalSumma.EditValue + (decimal)_vatSumma.EditValue;
			}
		}
		else
		{
			foreach (ReceivedServiceSpecificationRow receivedServices2 in _receivedServicesList)
			{
				num2 += receivedServices2.Summa;
			}
			_totalSumma.EditValue = num2;
			_vatSumma.EditValue = 0m;
		}
		CalculatePartSumma();
	}

	private void CalculatePartSumma()
	{
		decimal summaNCC = _totalSumma.Value;
		if (_currExChngRate.Base != 0m)
		{
			summaNCC = (_totalSumma.Value * _currExChngRate.Rate / _currExChngRate.Base).RoundAs(2);
		}
		_supplierContractContainer.CalculatePartSumma(summaNCC);
	}

	private void VATType_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		VATType_EditValueChangedForLoad();
		VATTypeValidate((string)e.OldValue, _vatType.Code);
	}

	private void VATType_EditValueChangedForLoad()
	{
		bool flag = _vatType.Code.InList("1", "2");
		_vatCol.Visible = flag;
		_vatCol.ShowInColumnChooser = flag;
		if (!flag)
		{
			_vatSumma.EditValue = 0m;
		}
	}

	private void VATTypeValidate(string vatTypeOldValue, string vatTypeNewValue)
	{
		decimal num = 0m;
		decimal num2 = 0m;
		if (_receivedServicesList.Count > 0)
		{
			switch (vatTypeNewValue)
			{
			case "1":
				if (vatTypeOldValue == "3" || vatTypeOldValue == "4")
				{
					foreach (ReceivedServiceSpecificationRow receivedServices in _receivedServicesList)
					{
						num += PriceHelper.CalcVATAmountByType(receivedServices.Summa, "1", receivedServices.VAT);
						num2 += receivedServices.Summa;
					}
				}
				else
				{
					foreach (ReceivedServiceSpecificationRow receivedServices2 in _receivedServicesList)
					{
						receivedServices2.Price -= PriceHelper.CalcVATAmountByType(receivedServices2.Price, vatTypeOldValue, receivedServices2.VAT);
						receivedServices2.Summa = PriceHelper.CalculateSum(receivedServices2.Price, receivedServices2.Quantity);
						num += PriceHelper.CalcVATAmountByType(receivedServices2.Summa, "1", receivedServices2.VAT);
						num2 += receivedServices2.Summa;
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
					foreach (ReceivedServiceSpecificationRow receivedServices3 in _receivedServicesList)
					{
						num += PriceHelper.CalcVATAmountByType(receivedServices3.Summa, "1", receivedServices3.VAT);
						receivedServices3.Price += PriceHelper.CalcVATAmountByType(receivedServices3.Price, "1", receivedServices3.VAT);
						receivedServices3.Summa = PriceHelper.CalculateSum(receivedServices3.Price, receivedServices3.Quantity);
						num2 += receivedServices3.Summa;
					}
					_vatSumma.EditValue = num;
					_totalSumma.EditValue = num2;
					break;
				case "1":
					foreach (ReceivedServiceSpecificationRow receivedServices4 in _receivedServicesList)
					{
						num += PriceHelper.CalcVATAmountByType(receivedServices4.Summa, "1", receivedServices4.VAT);
						receivedServices4.Price += PriceHelper.CalcVATAmountByType(receivedServices4.Price, "1", receivedServices4.VAT);
						receivedServices4.Summa = PriceHelper.CalculateSum(receivedServices4.Price, receivedServices4.Quantity);
						num2 += receivedServices4.Summa;
					}
					_vatSumma.EditValue = num;
					_totalSumma.EditValue = num2;
					break;
				}
				break;
			case "3":
			case "4":
				if (vatTypeOldValue == "2")
				{
					foreach (ReceivedServiceSpecificationRow receivedServices5 in _receivedServicesList)
					{
						receivedServices5.Price -= PriceHelper.CalcVATAmountByType(receivedServices5.Price, vatTypeOldValue, receivedServices5.VAT);
						receivedServices5.Summa = PriceHelper.CalculateSum(receivedServices5.Price, receivedServices5.Quantity);
						num2 += receivedServices5.Summa;
					}
					_vatSumma.EditValue = 0m;
					_totalSumma.EditValue = num2;
				}
				else
				{
					if (!(vatTypeOldValue == "1"))
					{
						break;
					}
					foreach (ReceivedServiceSpecificationRow receivedServices6 in _receivedServicesList)
					{
						num2 += receivedServices6.Summa;
					}
					_vatSumma.EditValue = 0m;
					_totalSumma.EditValue = num2;
				}
				break;
			}
		}
		CalculatePartSumma();
	}

	private void ReceivedServiceGrid_AddingRow(object sender, DocumentGridAddingRowEventArgs e)
	{
		_receivedServicesList.Add(new ReceivedServiceSpecificationRow
		{
			VAT = _vatType.Code.InList("1", "2")
		});
		e.RowNumber = _receivedServicesList.Count - 1;
	}

	private void ReceivedServiceGrid_DuplicatingRow(object sender, DocumentGridDuplicatingRowEventArgs e)
	{
		ReceivedServiceSpecificationRow receivedServiceSpecificationRow = (ReceivedServiceSpecificationRow)_receivedServiceGrid.Grid.GetRow(e.RowHandle);
		if (receivedServiceSpecificationRow != null)
		{
			ReceivedServiceSpecificationRow receivedServiceSpecificationRow2 = new ReceivedServiceSpecificationRow();
			receivedServiceSpecificationRow.Clone(receivedServiceSpecificationRow2);
			_receivedServicesList.Add(receivedServiceSpecificationRow2);
			e.NewRowNumber = _receivedServicesList.Count - 1;
			CalcSummAndSummVAT();
		}
	}

	private void ReceivedServiceGrid_DeletedRow(object sender, EventArgs e)
	{
		CalcSummAndSummVAT();
	}

	private void PriceListType_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		bool flag = false;
		string priceListRowVATType;
		string priceListRowCurr;
		if (!string.IsNullOrWhiteSpace(_priceListType.Code) && _priceListType.DoValidate())
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
				if (PriceHelper.GetPriceListRowVATType(_priceListType.Code, out priceListRowVATType, out priceListRowCurr))
				{
					_vatType.Code = priceListRowVATType;
				}
			}
		}
		if (flag)
		{
			UpdateReceivedServicePrices(_date.EditValue.Value, _priceListType.Code, _currExChngRate.Rate, _currExChngRate.Base);
			PriceHelper.GetPriceListRowVATType(_priceListType.Code, out priceListRowVATType, out priceListRowCurr);
			if (priceListRowVATType != _vatType.Code)
			{
				VATTypeValidate(priceListRowVATType, _vatType.Code);
			}
			CalcSummAndSummVAT();
		}
	}

	private void UpdateReceivedServicePrices(DateOnly docDate, string priceListType, decimal currRate, decimal currBase)
	{
		if (_receivedServicesList.Count == 0)
		{
			return;
		}
		PriceHelper.GetPriceListRowVATType(priceListType, out var _, out var priceListRowCurr);
		if (priceListRowCurr != "AMD")
		{
			TradeSettings.CreateTradeClient().CommonServices.GetExchangeRates(priceListRowCurr, docDate);
		}
		foreach (ReceivedServiceSpecificationRow receivedServices in _receivedServicesList)
		{
			if (receivedServices.ServID == -1)
			{
				break;
			}
			PricesDataProvider.DataRow price = TradeSettings.CreateTradeClient().CommonServices.GetPrice(priceListType, receivedServices.ServID, receivedServices.UnitCode, docDate);
			if (priceListRowCurr == _currCode.Code)
			{
				receivedServices.Price = price.Price;
			}
			else if (priceListRowCurr == "AMD")
			{
				receivedServices.Price = (price.Price / currRate * currBase).RoundAs(2);
			}
			else
			{
				receivedServices.Price = (price.Price * _exchangeRate.Rate / _exchangeRate.Base / currRate * currBase).RoundAs(2);
			}
			receivedServices.Summa = PriceHelper.CalculateSum(receivedServices.Price, receivedServices.Quantity);
		}
	}

	private void ReceivedServiceGrid_ClickDropDown(object sender, DocumentGridClickDropDownEventArgs e)
	{
		ReceivedServiceSpecificationRow receivedServiceSpecificationRow = (ReceivedServiceSpecificationRow)_receivedServiceGrid.Grid.GetRow(e.RowHandle);
		if (receivedServiceSpecificationRow == null)
		{
			return;
		}
		if (e.Column == _unitCodeCol)
		{
			if (!string.IsNullOrWhiteSpace(receivedServiceSpecificationRow.ServCode))
			{
				MTQntUnits mTQntUnits = new MTQntUnits(receivedServiceSpecificationRow.ServCode);
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
			else
			{
				ModalBrowser modalBrowser2 = new ModalBrowser(_unitCodeCol.DataView)
				{
					LookUpValue = e.Value,
					Owner = e.Owner,
					Left = e.Left,
					Top = e.Top
				};
				if (modalBrowser2.Show())
				{
					e.NewValue = _unitCodeCol.DataView.ValueMember(modalBrowser2.SelectedRowIndex);
					MTQntUnitDataProvider.DataRow.Key key = (MTQntUnitDataProvider.DataRow.Key)_unitCodeCol.DataView.KeyMember(modalBrowser2.SelectedRowIndex);
					receivedServiceSpecificationRow.ServID = key.MTID;
					MaterialCacheRow itemById = TradeSettings.TradeCacheService.Material.GetItemById(receivedServiceSpecificationRow.ServID);
					if (itemById != null)
					{
						receivedServiceSpecificationRow.ServCode = itemById.MTCode;
						receivedServiceSpecificationRow.ServName = itemById.Caption;
						receivedServiceSpecificationRow.VAT = itemById.VAT;
					}
				}
			}
		}
		CalcSummAndSummVAT();
	}

	private void ReceivedServiceTableView_CellValueChanged(object sender, CellValueChangedEventArgs e)
	{
		ReceivedServiceSpecificationRow receivedServiceSpecificationRow = (ReceivedServiceSpecificationRow)_receivedServiceGrid.Grid.GetRow(e.RowHandle);
		if (receivedServiceSpecificationRow == null)
		{
			return;
		}
		string oldUnit = string.Empty;
		if (e.OldValue != null)
		{
			oldUnit = e.OldValue.ToString();
		}
		if (e.Column == _servCodeCol)
		{
			try
			{
				if (e.Value == null)
				{
					receivedServiceSpecificationRow.ServID = -1;
					receivedServiceSpecificationRow.ServName = "";
					receivedServiceSpecificationRow.UnitCode = "";
					receivedServiceSpecificationRow.UnitBrief = "";
				}
				else
				{
					string text = e.Value.ToString();
					if (string.IsNullOrWhiteSpace(text))
					{
						throw new Exception();
					}
					MaterialCacheRow item = TradeSettings.TradeCacheService.Material.GetItem(text, throwExceptionNotExits: true);
					receivedServiceSpecificationRow.ServID = item.Id;
					receivedServiceSpecificationRow.ServName = item.Caption;
					receivedServiceSpecificationRow.UnitCode = TradeSettings.CreateTradeClient().CommonServices.GetMTQntUnitByMTIDPriceTypeAndDocDate(receivedServiceSpecificationRow.ServID, _priceListType.Code, _date.EditValue.Value);
					if (string.IsNullOrWhiteSpace(receivedServiceSpecificationRow.UnitCode))
					{
						receivedServiceSpecificationRow.UnitCode = item.Unit;
					}
					_unitCodeCol.DataView = new MTQntUnits(receivedServiceSpecificationRow.ServCode);
					receivedServiceSpecificationRow.VAT = item.VAT;
					receivedServiceSpecificationRow.Price = TradeSettings.CreateTradeClient().CommonServices.GetPriceForGivenPriceListAndVATType(_priceListType.Code, _vatType.Code, receivedServiceSpecificationRow.ServID, receivedServiceSpecificationRow.UnitCode, _date.EditValue.Value, _currCode.Code);
					receivedServiceSpecificationRow.Summa = PriceHelper.CalculateSum(receivedServiceSpecificationRow.Price, receivedServiceSpecificationRow.Quantity);
				}
			}
			catch (Exception ex)
			{
				receivedServiceSpecificationRow.ServName = "";
				receivedServiceSpecificationRow.ServID = -1;
				receivedServiceSpecificationRow.UnitCode = "";
				receivedServiceSpecificationRow.UnitBrief = "";
				receivedServiceSpecificationRow.Quantity = 0m;
				receivedServiceSpecificationRow.Price = 0m;
				receivedServiceSpecificationRow.VAT = false;
				if (e.Value != null && !string.IsNullOrWhiteSpace(e.Value.ToString()))
				{
					Settings.ErrorHandler(ex);
				}
			}
		}
		else if (e.Column == _unitCodeCol)
		{
			try
			{
				if (e.Value != null && !string.IsNullOrWhiteSpace(e.Value.ToString()))
				{
					if (string.IsNullOrWhiteSpace(_priceListType.Code))
					{
						receivedServiceSpecificationRow.Price = PriceHelper.GetPriceForUnit(oldUnit, (string)e.Value, receivedServiceSpecificationRow.Price, receivedServiceSpecificationRow.ServCode);
					}
					else
					{
						decimal priceForGivenPriceListAndVATType = TradeSettings.CreateTradeClient().CommonServices.GetPriceForGivenPriceListAndVATType(_priceListType.Code, _vatType.Code, receivedServiceSpecificationRow.ServID, (string)e.Value, _date.EditValue.Value, _currCode.Code);
						if (priceForGivenPriceListAndVATType != 0m)
						{
							receivedServiceSpecificationRow.Price = priceForGivenPriceListAndVATType;
						}
						else
						{
							receivedServiceSpecificationRow.Price = PriceHelper.GetPriceForUnit(oldUnit, (string)e.Value, receivedServiceSpecificationRow.Price, receivedServiceSpecificationRow.ServCode);
						}
					}
				}
			}
			catch (Exception ex2)
			{
				Settings.ErrorHandler(ex2);
			}
		}
		else if (e.Column == _quantityCol)
		{
			receivedServiceSpecificationRow.Summa = (Convert.ToDecimal(e.Value) * receivedServiceSpecificationRow.Price).RoundAs(2);
		}
		else if (e.Column == _priceCol)
		{
			receivedServiceSpecificationRow.Summa = (Convert.ToDecimal(e.Value) * receivedServiceSpecificationRow.Quantity).RoundAs(2);
		}
		else if (e.Column == _summaCol && receivedServiceSpecificationRow.Quantity != 0m)
		{
			receivedServiceSpecificationRow.Price = (Convert.ToDecimal(e.Value) / receivedServiceSpecificationRow.Quantity).RoundAs(2);
		}
		CalcSummAndSummVAT();
	}

	private void CurrCode_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		bool flag = false;
		if (_totalSumma.Value != 0m && !string.IsNullOrWhiteSpace(_currCode.Code) && _currCode.DoValidate() && ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Trade.Common.Resources.ValidateChangePrices, Settings.ProgramName, MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No) == MessageBoxResult.Yes)
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
			_currExChngRate.IsReadOnly = PO.StoredInDb && PO.State != (byte)DocumentStateSE.Draft && PO.State != (byte)DocumentStateSE.Deleted;
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

	private void Date_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		if (!_date.DoValidate() || !_date.EditValue.HasValue)
		{
			ControlsHelper.SetFocus(_date);
			ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Common.Resources.ValidateEmptyDate, Settings.ProgramName, MessageBoxButton.OK, MessageBoxImage.Exclamation);
		}
	}

	private void CurrExChngRate_EditValueChanged(object sender, RateEditValueChangedEventArgs e)
	{
		if (_currExChngRate.DoValidate())
		{
			CalculatePartSumma();
		}
	}

	private void UpdatePricesForCurr()
	{
		foreach (ReceivedServiceSpecificationRow receivedServices in _receivedServicesList)
		{
			if (_oldDocCurrCode == "AMD")
			{
				if (_currExChngRate.Rate != 0m)
				{
					receivedServices.Price = (receivedServices.Price / _currExChngRate.Rate * _currExChngRate.Base).RoundAs(2);
				}
			}
			else if (!(_currExChngRate.Rate == 0m) && !(_oldDocCurrExChngBase == 0m))
			{
				receivedServices.Price = (receivedServices.Price * _oldDocCurrExChngRate / _oldDocCurrExChngBase / _currExChngRate.Rate * _currExChngRate.Base).RoundAs(2);
			}
			receivedServices.Summa = PriceHelper.CalculateSum(receivedServices.Price, receivedServices.Quantity);
		}
		CalcSummAndSummVAT();
	}

	private void ReceivedServiceTableView_FocusedRowHandleChangedEventArgs(object sender, FocusedRowHandleChangedEventArgs e)
	{
		int focusedRowHandle = _receivedServiceTableView.FocusedRowHandle;
		ReceivedServiceSpecificationRow receivedServiceSpecificationRow = (ReceivedServiceSpecificationRow)_receivedServiceGrid.Grid.GetRow(focusedRowHandle);
		if (receivedServiceSpecificationRow != null)
		{
			ChangeColumnAdditonalView(receivedServiceSpecificationRow.ServCode);
		}
	}

	private void ChangeColumnAdditonalView(string code)
	{
		_unitCodeCol.DataView = new MTQntUnits(code, isShowQntUnitCaption: false, isShowOnlyComplects: false, "", ItemTypesSE.Services.Value);
	}

	private void FilteredSearch_Click(object sender, RoutedEventArgs e)
	{
		FilteredSearchWindow filteredSearchWindow = new FilteredSearchWindow(new FilteredSearch
		{
			FilterDataView = new GoodsGroups(showAsTree: true, filteredSearch: true),
			SearchDataView = new GoodsFilter(shortMode: true, ItemTypesSE.Services.Value),
			ResultsList = new ReceivedServicesFilteredSearchList()
		}, this, isBasketMode: true);
		if (!filteredSearchWindow.ShowDialog().Value)
		{
			return;
		}
		foreach (ReceivedServiceSpecificationRow item in filteredSearchWindow.BasketResult)
		{
			string mTQntUnitByMTIDPriceTypeAndDocDate = TradeSettings.CreateTradeClient().CommonServices.GetMTQntUnitByMTIDPriceTypeAndDocDate(item.ServID, _priceListType.Code, _date.EditValue.Value);
			if (!string.IsNullOrWhiteSpace(mTQntUnitByMTIDPriceTypeAndDocDate))
			{
				item.UnitCode = mTQntUnitByMTIDPriceTypeAndDocDate;
			}
			item.Price = TradeSettings.CreateTradeClient().CommonServices.GetPriceForGivenPriceListAndVATType(_priceListType.Code, _vatType.Code, item.ServID, item.UnitCode, _date.EditValue.Value, _currCode.Code);
			_receivedServicesList.Add(item);
		}
		CalcSummAndSummVAT();
	}

	private void PartnerCodeEditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		CalculatePartSumma();
	}

	private void PartnerCode_EditAdditionalValueChanged(object sender, EventArgs e)
	{
		CalculatePartSumma();
	}

	private void ContractCodeEditValueChanged(object sender, EventArgs e)
	{
		if ((_receivedServicesList.Count == 0 || string.IsNullOrWhiteSpace(_priceListType.Code)) && !string.IsNullOrWhiteSpace(_supplierContractContainer.Contract.Code) && _supplierContractContainer.Contract.DoValidate())
		{
			PartnerCacheRow.ContractRow contract = TradeSettings.TradeCacheService.Partner.GetContract(_supplierContractContainer.PartnerCode.Code, _supplierContractContainer.Contract.Code);
			if (contract != null && !string.IsNullOrWhiteSpace(contract.PriceType))
			{
				_priceListType.Code = contract.PriceType;
				if (_receivedServicesList.Count == 0)
				{
					if (PriceHelper.GetPriceListRowVATType(contract.PriceType, out var priceListRowVATType, out var _))
					{
						_vatType.Code = priceListRowVATType;
					}
					_currCode.Code = contract.CurrCode;
				}
			}
		}
		CalculatePartSumma();
	}

	protected override void AddToolbarItems()
	{
		base.AddToolbarItems();
		Ribbon.ToolbarItemLinks.Add(_barButtonJumpTo.ConvertForToolbar("t_doc_JumpTo_16.svg"));
		Ribbon.ToolbarItemLinks.Add(_barSubItemJournals.ConvertForToolbar("t_report_16.svg"));
	}

	protected override void AddItemsToRibbon()
	{
		base.AddItemsToRibbon();
		CreateRibbonPageGroupActions();
		CreateRibbonPageGroupReports();
		_barButtonJumpTo = new ReportBarButtonItem(DataProviderTypeSE.AllDocumentsJournal, showDialog: false, ArmSoft.SME.Common.Resources.JumpTo, null, "doc_JumpTo_16.svg", "doc_JumpTo_32.svg");
		_barButtonJumpTo.AddInBarSubItem(ref _ribbonPageGroupActions, ref _barSubItemActions);
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
		ReportBarButtonItem barButtonPartnerOperationJournal = _barButtonPartnerOperationJournal;
		num = 3;
		List<PropertyWithValue> list2 = new List<PropertyWithValue>(num);
		CollectionsMarshal.SetCount(list2, num);
		Span<PropertyWithValue> span2 = CollectionsMarshal.AsSpan(list2);
		span2[0] = new PropertyWithValue("DocISN", PO.ISN.ToString());
		span2[1] = new PropertyWithValue("AdditionalCaption", GetReportsAdditionalCaption());
		span2[2] = new PropertyWithValue("ShowLinks", true.ToString());
		barButtonPartnerOperationJournal.PropertyWithValues = list2;
	}

	protected override void CreateChildDocument(short childDocType, ReceivedService baseDoc)
	{
		base.CreateChildDocument(childDocType, baseDoc);
		CreateBasedOnDocHelper.CreateDocument((DocumentTypeSE)(SmartEnum<DocumentTypeSE, short>)childDocType, PO.ISN, DVPID);
	}

	protected override void SetReportBarButtonsEnable(bool isEnabled)
	{
		base.SetReportBarButtonsEnable(isEnabled);
		_barButtonJumpTo.IsEnabled = isEnabled;
		_barButtonPartnerOperationJournal.IsEnabled = isEnabled;
	}

	protected override void StoreSettings(RegistryKey rk)
	{
		base.StoreSettings(rk);
		_receivedServiceGrid.StoreSettings(rk);
	}

	protected override void RestoreSettings(double defaultWidth, double defaultHeight, RegistryKey rk)
	{
		base.RestoreSettings(defaultWidth, defaultHeight, rk);
		_receivedServiceGrid.RestoreSettings(rk);
	}

	protected override void FillDocument(StoreRequest sr)
	{
		base.FillDocument(sr);
		PO.ReceivedServiceList.Clear();
		foreach (ReceivedServiceSpecificationRow receivedServices in _receivedServicesList)
		{
			ReceivedService.ReceivedServiceSpecificationRow item = new ReceivedService.ReceivedServiceSpecificationRow
			{
				ServCode = receivedServices.ServCode,
				ServName = receivedServices.ServName,
				ServID = receivedServices.ServID,
				Quantity = receivedServices.Quantity,
				Price = receivedServices.Price,
				PriceVAT = receivedServices.PriceVAT,
				VAT = receivedServices.VAT,
				Summa = receivedServices.Summa,
				RowNum = receivedServices.RowNum,
				FullName = receivedServices.FullName,
				UnitCode = receivedServices.UnitCode,
				UnitBrief = receivedServices.UnitBrief,
				UnitFraction = receivedServices.UnitFraction
			};
			PO.ReceivedServiceList.Add(item);
		}
	}

	protected void CreateBasedOnDoc()
	{
		_receivedServicesList.Clear();
		if (BaseDocument.POType != DocumentTypeSE.ReceivedTaxInvoice.Value)
		{
			return;
		}
		ReceivedTaxInvoice receivedTaxInvoice = (ReceivedTaxInvoice)BaseDocument;
		bool flag = receivedTaxInvoice.Type == Convert.ToInt16("3");
		_date.EditValue = receivedTaxInvoice.DocDate;
		PartnerCacheRow partnerCacheRow = TradeSettings.TradeCacheService.Partner.GetList((PartnerCacheRow p) => p.TaxCode == receivedTaxInvoice.SupplierTIN && p.Supplier).FirstOrDefault();
		if (partnerCacheRow != null)
		{
			_supplierContractContainer.PartnerCode.Code = partnerCacheRow.PartCode;
			PartnerCacheRow.ContractRow defaultContract = TradeSettings.TradeCacheService.Partner.GetDefaultContract(partnerCacheRow);
			_supplierContractContainer.Contract.Code = defaultContract?.Contract;
		}
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
		_comment.Text = receivedTaxInvoice.AdditionalInfo;
		foreach (TaxDocument.SpecificationRow item2 in receivedTaxInvoice.Items)
		{
			if (!string.IsNullOrWhiteSpace(item2.ItemCode) && item2.ItemType == ItemTypesSE.Services.Value)
			{
				ReceivedServiceSpecificationRow receivedServiceSpecificationRow = new ReceivedServiceSpecificationRow
				{
					ServCode = item2.ItemCode,
					Quantity = ((item2.Quantity == 0m) ? 1m : (Math.Truncate(item2.Quantity * 1000m) / 1000m)),
					Summa = item2.Sum.RoundAs(2),
					VAT = (item2.SumVAT > 0m)
				};
				if (flag)
				{
					receivedServiceSpecificationRow.Summa += item2.Excise.RoundAs(2);
				}
				receivedServiceSpecificationRow.PriceVAT = (receivedServiceSpecificationRow.Summa / receivedServiceSpecificationRow.Quantity).RoundAs(2);
				if (_vatType.Code == "2")
				{
					receivedServiceSpecificationRow.Summa += item2.SumVAT.RoundAs(2);
				}
				receivedServiceSpecificationRow.Price = (receivedServiceSpecificationRow.Summa / receivedServiceSpecificationRow.Quantity).RoundAs(2);
				if (_vatType.Code != "2")
				{
					receivedServiceSpecificationRow.PriceVAT = receivedServiceSpecificationRow.Price;
				}
				_vatSumma.Value += item2.SumVAT;
				_totalSumma.Value += item2.TotalSum;
				MaterialCacheRow item = TradeSettings.TradeCacheService.Material.GetItem(receivedServiceSpecificationRow.ServCode);
				if (item != null)
				{
					receivedServiceSpecificationRow.ServID = item.Id;
					receivedServiceSpecificationRow.ServName = item.Caption;
					receivedServiceSpecificationRow.UnitCode = item2.UnitCodeFromDirectory;
					receivedServiceSpecificationRow.UnitBrief = item2.UnitBriefFromDirectory;
					receivedServiceSpecificationRow.UnitFraction = true;
				}
				_receivedServicesList.Add(receivedServiceSpecificationRow);
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
}
