using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using Ardalis.SmartEnum;
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
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.ComponentModel.__Internals;
using DevExpress.Data.Extensions;
using DevExpress.Utils;
using DevExpress.Xpf.Bars;
using DevExpress.Xpf.Editors;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.LayoutControl;
using DevExpress.XtraEditors.DXErrorProvider;
using Microsoft.Win32;

namespace ArmSoft.SME.Trade.UI.Documents;

[DocumentWindow(32)]
public class MTPurchaseOrderWindow : TradeDocumentWindow<MTPurchaseOrder>
{
	public class MTPurchaseOrderSpecificationRow : DocumentGridRow
	{
		[ObservableProperty]
		private int mTID = -1;

		[ObservableProperty]
		private string mTCode;

		[ObservableProperty]
		private string mTName;

		[ObservableProperty]
		private short rowNum;

		[ObservableProperty]
		private string unitCode;

		[ObservableProperty]
		private string fullName;

		[ObservableProperty]
		private string unitBrief;

		[ObservableProperty]
		private decimal quantity;

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
					OnPropertyChanging(__KnownINotifyPropertyChangingArgs.UnitCode);
					unitCode = value;
					OnUnitCodeChanged(value);
					OnPropertyChanged(__KnownINotifyPropertyChangedArgs.UnitCode);
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

		public MTPurchaseOrderSpecificationRow()
		{
		}

		public MTPurchaseOrderSpecificationRow(MTPurchaseOrderWindow docWindow)
			: base(docWindow)
		{
		}

		static MTPurchaseOrderSpecificationRow()
		{
			DocumentGridRow.RegisterPropertyValidators(typeof(MTPurchaseOrderSpecificationRow), cacheValidationResults: true, new Dictionary<string, Action<DocumentGridRow, DevExpress.XtraEditors.DXErrorProvider.ErrorInfo>>
			{
				{
					"MTCode",
					(DocumentGridRow row, DevExpress.XtraEditors.DXErrorProvider.ErrorInfo info) =>
					{
						ValidateCode((MTPurchaseOrderSpecificationRow)row, info);
					}
				},
				{
					"UnitCode",
					(DocumentGridRow row, DevExpress.XtraEditors.DXErrorProvider.ErrorInfo info) =>
					{
						ValidateUnitCode((MTPurchaseOrderSpecificationRow)row, info);
					}
				},
				{
					"Quantity",
					(DocumentGridRow row, DevExpress.XtraEditors.DXErrorProvider.ErrorInfo info) =>
					{
						ValidateQuantity((MTPurchaseOrderSpecificationRow)row, info);
					}
				}
			});
		}

		private static void ValidateCode(MTPurchaseOrderSpecificationRow row, DevExpress.XtraEditors.DXErrorProvider.ErrorInfo info)
		{
			TradePropertyValidator.ValidateMaterialCode(row.MTCode, info, TradeAllDocDefinitions.MTPurchaseOrder.Grids["MTList"].Columns["MTCode"]);
		}

		private static void ValidateUnitCode(MTPurchaseOrderSpecificationRow row, DevExpress.XtraEditors.DXErrorProvider.ErrorInfo info)
		{
			TradePropertyValidator.ValidateGoodUnitCode(row.UnitCode, row.MTCode, info, TradeAllDocDefinitions.MTPurchaseOrder.Grids["MTList"].Columns["UnitCode"]);
		}

		private static void ValidateQuantity(MTPurchaseOrderSpecificationRow row, DevExpress.XtraEditors.DXErrorProvider.ErrorInfo info)
		{
			PropertyValidator.PositiveValidate(row.Quantity, info, ArmSoft.SME.Common.Resources.Quantity);
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		private void OnMTCodeChanged(string value)
		{
			ResetForceValidationBit("MTCode", "UnitCode");
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
			ResetForceValidationBit("Quantity");
		}
	}

	public class MTPurchaseOrderFilteredSearchList : IResultList
	{
		private readonly ObservableCollection<MTPurchaseOrderSpecificationRow> _itemSource = new ObservableCollection<MTPurchaseOrderSpecificationRow>();

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
					Header = TradeAllDocDefinitions.MTPurchaseOrder.Grids["MTList"].Columns["MTCode"].Caption,
					FieldName = "MTCode",
					CustomWidth = 125.0
				};
				span[1] = new FilteredSearchGridColumn
				{
					Header = TradeAllDocDefinitions.MTPurchaseOrder.Grids["MTList"].Columns["MTName"].Caption,
					FieldName = "MTName",
					CustomWidth = 300.0
				};
				span[2] = new FilteredSearchGridColumn
				{
					Header = TradeAllDocDefinitions.MTPurchaseOrder.Grids["MTList"].Columns["Quantity"].Caption,
					FieldName = "Quantity",
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
			int num2 = _itemSource.FindIndex((MTPurchaseOrderSpecificationRow x) => x.MTCode == mtCode);
			if (num2 == -1)
			{
				MaterialCacheRow item = TradeSettings.TradeCacheService.Material.GetItem(mtCode);
				if (item == null)
				{
					return -1;
				}
				MTPurchaseOrderSpecificationRow item2 = new MTPurchaseOrderSpecificationRow
				{
					MTID = item.Id,
					MTCode = item.MTCode,
					MTName = item.Caption,
					UnitCode = item.Unit,
					UnitBrief = item.UnitRow.Brief,
					Quantity = 0m
				};
				_itemSource.Add(item2);
				num2 = _itemSource.Count - 1;
			}
			_itemSource[num2].Quantity += num;
			return num2;
		}
	}

	private BarEditItemExt _viewAvailAndTurn;

	private DropDownControl _supplierCode;

	private TextEditExt _supplierEmail;

	private DropDownControl _owner;

	private DocumentGridControl _mtPurchaseOrderGrid;

	private TableView _mtPurchaseOrderTableView;

	private BarEditItemExt _filteredSearch;

	private ObservableCollection<MTPurchaseOrderSpecificationRow> _mtPurchaseOrderList;

	private DocumentGridColumn _mtCodeCol;

	private DocumentGridColumn _unitCodeCol;

	private int _maxRowId;

	private ReportBarButtonItem _barButtonJumpTo;

	protected override double WidthValue => 1090.0;

	protected override double HeightValue => 440.0;

	protected override double MinWidthValue => 620.0;

	protected override double MinHeightValue => 250.0;

	public MTPurchaseOrderWindow()
	{
	}

	public MTPurchaseOrderWindow(MTPurchaseOrder doc)
		: base(doc)
	{
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
		_supplierCode = AddDropDownControl(null, ((PartnerContractGroupDefinition)Definition.Fields["PartnerContractGroup"]).PartnerFieldDefinition, new PartnersFilter(PartnersFilter.ShowPartnersModeType.CodeCaption, "", isFilterBySuppliers: true), allowAdditionButtons: true);
		_supplierCode.IsRequired = true;
		_supplierCode.ShowComment = true;
		_supplierCode.EditValueChanged += new SafeDocumentEventHandler<EditValueChangedEventArgs>(SupplierCode_EditValueChanged, this).Handler;
		_supplierCode.EditAdditionalValueChanged += new SafeDocumentEventHandler<EventArgs>(SupplierCode_EditAdditionalValueChanged, this).Handler;
		_supplierEmail = AddStringControl(null, Definition.Fields["SupplierEMail"], HorizontalAlignment.Stretch);
		AddComboBoxControl(null, Definition.Fields["Comment"], TradeSettings.TradeCacheService.StandardComments.GetCaptionsList(), HorizontalAlignment.Stretch);
		_owner = AddDropDownControl(null, Definition.Fields["Owner"], new UsersFilter());
		_owner.CodeWidth = 12;
		_owner.ShowComment = false;
		_owner.InnerCodeEdit.Width = _owner.InnerCodeEdit.MinWidth;
		if (!Settings.LoginService.IsAdmin && Settings.ParametersService.DenyAccessToOtherOwnerDocs())
		{
			((LayoutItemWithRequired)(object)_owner.Parent).Visibility = Visibility.Collapsed;
		}
		_mtPurchaseOrderGrid = AddGrid(null, Definition.Grids["MTList"], HorizontalAlignment.Stretch, VerticalAlignment.Stretch);
		_mtPurchaseOrderGrid.AddingRow += new SafeDocumentEventHandler<DocumentGridAddingRowEventArgs>(MTPurchaseOrderGrid_AddingRow, this).Handler;
		_mtPurchaseOrderGrid.DuplicatingRow += new SafeDocumentEventHandler<DocumentGridDuplicatingRowEventArgs>(MTPurchaseOrderGrid_DuplicatingRow, this).Handler;
		_mtPurchaseOrderGrid.RefreshButtons += new SafeDocumentEventHandler<EventArgs>(MTPurchaseOrderGrid_RefreshButtons, this).Handler;
		_viewAvailAndTurn = _mtPurchaseOrderGrid.AddButton("", ArmSoft.SME.Trade.Common.Resources.AvailAndTurn, ArmSoft.SME.Trade.Common.Resources.AvailabilityAndTurn, 130.0);
		_viewAvailAndTurn.Click += new SafeDocumentEventHandler<RoutedEventArgs>(ViewAvailAndTurn_Click, this).Handler;
		_filteredSearch = _mtPurchaseOrderGrid.AddButton("", ArmSoft.SME.Trade.Common.Resources.GroupAddition, ArmSoft.SME.Trade.Common.Resources.GroupAddition, 130.0);
		_filteredSearch.Click += new SafeDocumentEventHandler<RoutedEventArgs>(FilteredSearch_Click, this).Handler;
		_mtPurchaseOrderTableView = _mtPurchaseOrderGrid.View;
		_mtPurchaseOrderTableView.CellValueChanged += new SafeDocumentEventHandler<CellValueChangedEventArgs>(MTPurchaseOrderTableView_CellValueChanged, this).Handler;
		_mtPurchaseOrderTableView.FocusedRowHandleChanged += new SafeDocumentEventHandler<FocusedRowHandleChangedEventArgs>(MTPurchaseOrderTableView_FocusedRowHandleChangedEventArgs, this).Handler;
		_mtPurchaseOrderGrid.ClickDropDown += new SafeDocumentEventHandler<DocumentGridClickDropDownEventArgs>(MTPurchaseOrderGrid_ClickDropDown, this).Handler;
		_mtPurchaseOrderGrid.AddRowIndicator(35);
		_mtCodeCol = _mtPurchaseOrderGrid.AddDropDownColumn(Definition.Grids["MTList"].Columns["MTCode"], new GoodsFilter(shortMode: true, ItemTypesSE.Goods.Value, "", isShowOnlyInPriceList: true), allowAdditionButtons: true);
		_mtCodeCol.Width = 125.0;
		DocumentGridColumn documentGridColumn = _mtPurchaseOrderGrid.AddStringColumn(Definition.Grids["MTList"].Columns["MTName"]);
		documentGridColumn.Width = 225.0;
		documentGridColumn.AllowEditing = DefaultBoolean.False;
		_unitCodeCol = _mtPurchaseOrderGrid.AddDropDownColumn(Definition.Grids["MTList"].Columns["UnitCode"]);
		_mtPurchaseOrderGrid.AddStringColumn(Definition.Grids["MTList"].Columns["UnitBrief"]).AllowEditing = DefaultBoolean.False;
		_mtPurchaseOrderGrid.AddNumberColumn(Definition.Grids["MTList"].Columns["Quantity"], showTotal: true);
		_mtPurchaseOrderList = new ObservableCollection<MTPurchaseOrderSpecificationRow>();
	}

	protected override void OnNew()
	{
		base.OnNew();
		_date.EditValue = Settings.HostingEnvironment.CurrentDate;
		_owner.Code = Settings.LoginService.UserName;
	}

	protected override void OnNewAfterSave()
	{
		base.OnNewAfterSave();
		_mtPurchaseOrderList.Clear();
		_owner.Code = Settings.LoginService.UserName;
	}

	protected override void OnEdit()
	{
		base.OnEdit();
		LoadDataToControls();
		_setFocusOnActivation = _docNum;
	}

	protected override void LoadDataToControls()
	{
		base.LoadDataToControls();
		_supplierCode.Code = PO.PartnerContractGroup.PartnerCode;
		_maxRowId = PO.MaxRowId;
		_mtPurchaseOrderList.Clear();
		foreach (MTPurchaseOrder.MTPurchaseOrderSpecificationRow mT in PO.MTList)
		{
			MTPurchaseOrderSpecificationRow item = new MTPurchaseOrderSpecificationRow
			{
				MTID = mT.MTID,
				MTCode = mT.MTCode,
				MTName = mT.MTName,
				Quantity = mT.Quantity,
				RowID = mT.RowID,
				RowNum = mT.RowNum,
				FullName = mT.FullName,
				UnitCode = mT.UnitCode,
				UnitBrief = mT.UnitBrief
			};
			_mtPurchaseOrderList.Add(item);
		}
		_mtPurchaseOrderGrid.ItemsSource = _mtPurchaseOrderList;
		if (!(PO.BaseISN == Guid.Empty) && !PO.StoredInDb && _viewMode != WindowViewMode.Copy)
		{
			CreateBasedOnDoc();
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

	private void CreateBasedOnDoc()
	{
		TradeDocument tradeDocument = (TradeDocument)Settings.DocumentService.Load(PO.BaseISN);
		if (tradeDocument != null)
		{
			_supplierCode.Code = tradeDocument.PartnerContractGroup.PartnerCode;
			if (!string.IsNullOrWhiteSpace(_supplierCode.Code))
			{
				_supplierEmail.Text = TradeSettings.TradeCacheService.Partner.GetItem(_supplierCode.Code)?.Email;
			}
		}
		int num = 0;
		if (tradeDocument.POType == (short)DocumentTypeSE.MTIncome)
		{
			foreach (MTIncomeBase.MTIncomeSpecificationRow mT in ((MTIncome)tradeDocument).MTList)
			{
				MTPurchaseOrderSpecificationRow item = new MTPurchaseOrderSpecificationRow
				{
					MTID = mT.MTID,
					MTCode = mT.MTCode,
					MTName = mT.MTName,
					Quantity = mT.Quantity,
					RowID = num,
					RowNum = mT.RowNum,
					FullName = mT.FullName,
					UnitCode = mT.UnitCode,
					UnitBrief = mT.UnitBrief
				};
				_mtPurchaseOrderList.Add(item);
				num++;
			}
		}
		else if (tradeDocument.POType == (short)DocumentTypeSE.MTMoveOrder)
		{
			foreach (MTMoveBase.MTMoveSpecificationRow mT2 in ((MTMoveOrder)tradeDocument).MTList)
			{
				MTPurchaseOrderSpecificationRow item2 = new MTPurchaseOrderSpecificationRow
				{
					MTID = mT2.MTID,
					MTCode = mT2.MTCode,
					MTName = mT2.MTName,
					Quantity = mT2.Quantity,
					RowID = num,
					RowNum = mT2.RowNum,
					FullName = mT2.FullName,
					UnitCode = mT2.UnitCode,
					UnitBrief = mT2.UnitBrief
				};
				_mtPurchaseOrderList.Add(item2);
				num++;
			}
		}
		_maxRowId = num;
	}

	protected override void AddToolbarItems()
	{
		base.AddToolbarItems();
		Ribbon.ToolbarItemLinks.Add(_barButtonJumpTo.ConvertForToolbar("t_doc_JumpTo_16.svg"));
	}

	protected override void AddItemsToRibbon()
	{
		base.AddItemsToRibbon();
		CreateRibbonPageGroupActions();
		CreateRibbonPageGroupReports();
		BarSubItem barSubItem = new BarSubItem();
		_barButtonJumpTo = new ReportBarButtonItem(DataProviderTypeSE.AllDocumentsJournal, showDialog: false, ArmSoft.SME.Common.Resources.JumpTo, null, "doc_JumpTo_16.svg", "doc_JumpTo_32.svg");
		_barButtonJumpTo.AddInBarSubItem(ref _ribbonPageGroupActions, ref barSubItem);
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

	protected override void SetReportBarButtonsEnable(bool isEnabled)
	{
		base.SetReportBarButtonsEnable(isEnabled);
		_barButtonJumpTo.IsEnabled = isEnabled;
	}

	protected override void SetControlsReadonly()
	{
		base.SetControlsReadonly();
		if (_viewMode != WindowViewMode.View)
		{
			_supplierEmail.IsReadOnly = true;
			bool flag = State == DocumentStateSE.Draft.Value || State == DocumentStateSE.Deleted.Value;
			bool flag2 = _viewMode == WindowViewMode.Edit && !flag;
			_date.IsReadOnly = flag2;
			_mtPurchaseOrderGrid.IsReadOnly = flag2;
			_mtPurchaseOrderGrid.AllowAdd = !flag2;
			_mtPurchaseOrderGrid.AllowDelete = !flag2;
			_mtPurchaseOrderGrid.AllowDuplicate = !flag2;
		}
	}

	private void FilteredSearch_Click(object sender, RoutedEventArgs e)
	{
		FilteredSearch filteredSearch = new FilteredSearch
		{
			FilterDataView = new GoodsGroups(),
			SearchDataView = new GoodsFilter(shortMode: true, ItemTypesSE.Goods),
			ResultsList = new MTPurchaseOrderFilteredSearchList()
		};
		if (!string.IsNullOrWhiteSpace(_supplierCode.Code))
		{
			filteredSearch.FilteredSearchDataView = new GoodsFilter(shortMode: true, ItemTypesSE.Goods, "", isShowOnlyInPriceList: false, "", applyDefaultDataViewDefinition: true, _supplierCode.Code);
		}
		FilteredSearchWindow filteredSearchWindow = new FilteredSearchWindow(filteredSearch, this, isBasketMode: true);
		if (!filteredSearchWindow.ShowDialog().Value)
		{
			return;
		}
		foreach (MTPurchaseOrderSpecificationRow item in filteredSearchWindow.BasketResult)
		{
			item.RowID = _maxRowId;
			_mtPurchaseOrderList.Add(item);
			_maxRowId++;
		}
	}

	private void ViewAvailAndTurn_Click(object sender, RoutedEventArgs e)
	{
		MTPurchaseOrderSpecificationRow mTPurchaseOrderSpecificationRow = (MTPurchaseOrderSpecificationRow)_mtPurchaseOrderTableView.Grid.CurrentItem;
		MTAvailAndTurnWindow mTAvailAndTurnWindow = ((mTPurchaseOrderSpecificationRow == null) ? new MTAvailAndTurnWindow(DialogWindowMode.ViewMode, this, "", _supplierCode.Code, -1, "", "") : new MTAvailAndTurnWindow(DialogWindowMode.ViewMode, this, "", _supplierCode.Code, mTPurchaseOrderSpecificationRow.MTID, mTPurchaseOrderSpecificationRow.MTCode, mTPurchaseOrderSpecificationRow.UnitCode));
		mTAvailAndTurnWindow.Invoke();
	}

	private void MTPurchaseOrderGrid_AddingRow(object sender, DocumentGridAddingRowEventArgs e)
	{
		_mtPurchaseOrderList.Add(new MTPurchaseOrderSpecificationRow
		{
			RowID = _maxRowId
		});
		_maxRowId++;
		e.RowNumber = _mtPurchaseOrderList.Count - 1;
	}

	private void MTPurchaseOrderGrid_DuplicatingRow(object sender, DocumentGridDuplicatingRowEventArgs e)
	{
		MTPurchaseOrderSpecificationRow mTPurchaseOrderSpecificationRow = (MTPurchaseOrderSpecificationRow)_mtPurchaseOrderGrid.Grid.GetRow(e.RowHandle);
		if (mTPurchaseOrderSpecificationRow != null)
		{
			MTPurchaseOrderSpecificationRow mTPurchaseOrderSpecificationRow2 = new MTPurchaseOrderSpecificationRow();
			mTPurchaseOrderSpecificationRow.Clone(mTPurchaseOrderSpecificationRow2);
			mTPurchaseOrderSpecificationRow2.RowID = _maxRowId;
			_mtPurchaseOrderList.Add(mTPurchaseOrderSpecificationRow2);
			_maxRowId++;
			e.NewRowNumber = _mtPurchaseOrderList.Count - 1;
		}
	}

	private void MTPurchaseOrderGrid_RefreshButtons(object sender, EventArgs e)
	{
		_viewAvailAndTurn.IsEnabled = Settings.UserAccessService.GetProviderPermission(DataProviderTypeSE.MatRepAvailability).AllowView || Settings.UserAccessService.GetProviderPermission(DataProviderTypeSE.MatRepAvailabilityTreeView).AllowView || Settings.UserAccessService.GetProviderPermission(DataProviderTypeSE.GoodsRepOperations).AllowView || Settings.UserAccessService.GetProviderPermission(DataProviderTypeSE.MatRepSalesAnalysis).AllowView;
		_filteredSearch.IsEnabled = !_mtPurchaseOrderGrid.IsReadOnly && _mtPurchaseOrderGrid.AllowAdd;
	}

	private void SupplierCode_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		SupplierCodeEditValueChanged();
	}

	private void SupplierCode_EditAdditionalValueChanged(object sender, EventArgs e)
	{
		SupplierCodeEditValueChanged();
	}

	private void SupplierCodeEditValueChanged()
	{
		if (!string.IsNullOrWhiteSpace(_supplierCode.Code) && _supplierCode.DoValidate())
		{
			PartnerCacheRow item = TradeSettings.TradeCacheService.Partner.GetItem(_supplierCode.Code, throwExceptionNotExits: true);
			if (item != null)
			{
				_supplierEmail.EditValue = item.Email;
				_supplierEmail.DoValidate();
			}
		}
	}

	protected override void FillDocument(StoreRequest sr)
	{
		base.FillDocument(sr);
		PO.PartnerContractGroup.PartnerCode = _supplierCode.Code;
		PO.MTList.Clear();
		PO.MaxRowId = _maxRowId;
		foreach (MTPurchaseOrderSpecificationRow mtPurchaseOrder in _mtPurchaseOrderList)
		{
			MTPurchaseOrder.MTPurchaseOrderSpecificationRow item = new MTPurchaseOrder.MTPurchaseOrderSpecificationRow
			{
				MTID = mtPurchaseOrder.MTID,
				MTCode = mtPurchaseOrder.MTCode,
				MTName = mtPurchaseOrder.MTName,
				Quantity = mtPurchaseOrder.Quantity,
				RowID = mtPurchaseOrder.RowID,
				RowNum = mtPurchaseOrder.RowNum,
				FullName = mtPurchaseOrder.FullName,
				UnitCode = mtPurchaseOrder.UnitCode,
				UnitBrief = mtPurchaseOrder.UnitBrief
			};
			PO.MTList.Add(item);
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

	private void MTPurchaseOrderTableView_CellValueChanged(object sender, CellValueChangedEventArgs e)
	{
		MTPurchaseOrderSpecificationRow mTPurchaseOrderSpecificationRow = (MTPurchaseOrderSpecificationRow)_mtPurchaseOrderGrid.Grid.GetRow(e.RowHandle);
		if (mTPurchaseOrderSpecificationRow == null)
		{
			return;
		}
		if (e.Column == _mtCodeCol)
		{
			try
			{
				if (e.Value == null || string.IsNullOrWhiteSpace(e.Value.ToString()))
				{
					mTPurchaseOrderSpecificationRow.MTID = -1;
					mTPurchaseOrderSpecificationRow.MTName = "";
					mTPurchaseOrderSpecificationRow.UnitCode = string.Empty;
					mTPurchaseOrderSpecificationRow.UnitBrief = "";
				}
				else
				{
					string text = e.Value.ToString();
					foreach (MTPurchaseOrderSpecificationRow mtPurchaseOrder in _mtPurchaseOrderList)
					{
						if (mTPurchaseOrderSpecificationRow != mtPurchaseOrder && text == mtPurchaseOrder.MTCode)
						{
							if (ArmSoft.SME.Core.UI.MessageBox.Show(string.Format(ArmSoft.SME.Trade.Common.Resources.MTExistInMTList, text), Settings.ProgramName, MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.Yes) == MessageBoxResult.No)
							{
								mTPurchaseOrderSpecificationRow.MTCode = string.Empty;
								_mtPurchaseOrderGrid.Grid.RefreshData();
								return;
							}
							break;
						}
					}
					if (string.IsNullOrWhiteSpace(text))
					{
						throw new Exception();
					}
					MaterialCacheRow item = TradeSettings.TradeCacheService.Material.GetItem(text, throwExceptionNotExits: true);
					mTPurchaseOrderSpecificationRow.MTID = item.Id;
					mTPurchaseOrderSpecificationRow.MTName = item.Caption;
					mTPurchaseOrderSpecificationRow.UnitCode = item.Unit;
					mTPurchaseOrderSpecificationRow.UnitBrief = item.UnitRow.Brief;
					_unitCodeCol.DataView = new MTQntUnits(mTPurchaseOrderSpecificationRow.MTCode);
				}
			}
			catch (Exception ex)
			{
				mTPurchaseOrderSpecificationRow.MTName = "";
				mTPurchaseOrderSpecificationRow.MTID = -1;
				mTPurchaseOrderSpecificationRow.UnitCode = string.Empty;
				mTPurchaseOrderSpecificationRow.UnitBrief = "";
				mTPurchaseOrderSpecificationRow.Quantity = 0m;
				if (e.Value != null && !string.IsNullOrWhiteSpace(e.Value.ToString()))
				{
					Settings.ErrorHandler(ex);
				}
			}
		}
		_mtPurchaseOrderGrid.Grid.RefreshData();
	}

	private void MTPurchaseOrderGrid_ClickDropDown(object sender, DocumentGridClickDropDownEventArgs e)
	{
		try
		{
			MTPurchaseOrderSpecificationRow mTPurchaseOrderSpecificationRow = (MTPurchaseOrderSpecificationRow)_mtPurchaseOrderGrid.Grid.GetRow(e.RowHandle);
			if (e.Column == _unitCodeCol && mTPurchaseOrderSpecificationRow != null)
			{
				MTQntUnits mTQntUnits = new MTQntUnits(mTPurchaseOrderSpecificationRow.MTCode);
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
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	private void MTPurchaseOrderTableView_FocusedRowHandleChangedEventArgs(object sender, FocusedRowHandleChangedEventArgs e)
	{
		int focusedRowHandle = _mtPurchaseOrderTableView.FocusedRowHandle;
		MTPurchaseOrderSpecificationRow mTPurchaseOrderSpecificationRow = (MTPurchaseOrderSpecificationRow)_mtPurchaseOrderGrid.Grid.GetRow(focusedRowHandle);
		if (mTPurchaseOrderSpecificationRow != null)
		{
			ChangeColumnAdditonalView(mTPurchaseOrderSpecificationRow.MTCode);
		}
	}

	private void ChangeColumnAdditonalView(string mtCode)
	{
		_unitCodeCol.DataView = new MTQntUnits(string.IsNullOrWhiteSpace(mtCode) ? "" : mtCode);
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
		_mtPurchaseOrderGrid.StoreSettings(rk);
	}

	protected override void RestoreSettings(double defaultWidth, double defaultHeight, RegistryKey rk)
	{
		base.RestoreSettings(defaultWidth, defaultHeight, rk);
		_mtPurchaseOrderGrid.RestoreSettings(rk);
	}

	protected override void CreateChildDocument(short childDocType, MTPurchaseOrder baseDoc)
	{
		base.CreateChildDocument(childDocType, baseDoc);
		if (childDocType == 7 || childDocType == 20)
		{
			CreateBasedOnDocHelper.CreateDocument((DocumentTypeSE)(SmartEnum<DocumentTypeSE, short>)childDocType, PO.ISN, DVPID);
		}
	}
}
