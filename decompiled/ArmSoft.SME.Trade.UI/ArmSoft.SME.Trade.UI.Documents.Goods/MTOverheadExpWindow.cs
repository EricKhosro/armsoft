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
using ArmSoft.SME.Common.Constants;
using ArmSoft.SME.Common.CustomTypes;
using ArmSoft.SME.Common.Extensions;
using ArmSoft.SME.Common.Models;
using ArmSoft.SME.Common.Models.Documents;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Core.UI.Controls;
using ArmSoft.SME.Core.UI.Documents;
using ArmSoft.SME.Core.UI.DropDowns;
using ArmSoft.SME.Core.UI.Enums;
using ArmSoft.SME.Core.UI.Extensions;
using ArmSoft.SME.Core.UI.PersistableObject;
using ArmSoft.SME.Trade.Common;
using ArmSoft.SME.Trade.Common.Documents.Definition;
using ArmSoft.SME.Trade.Common.Helpers;
using ArmSoft.SME.Trade.Common.Models.Caches;
using ArmSoft.SME.Trade.Common.Models.Documents;
using ArmSoft.SME.Trade.Common.Parameters;
using ArmSoft.SME.Trade.Common.PersistableObject;
using ArmSoft.SME.Trade.Common.SmartEnums;
using ArmSoft.SME.Trade.UI.DropDown;
using ArmSoft.SME.Trade.UI.Helpers;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.ComponentModel.__Internals;
using DevExpress.Utils;
using DevExpress.Xpf.Bars;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Editors;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.LayoutControl;
using DevExpress.XtraEditors.DXErrorProvider;
using Microsoft.Win32;

namespace ArmSoft.SME.Trade.UI.Documents.Goods;

[DocumentWindow(13)]
public class MTOverheadExpWindow : TradeDocumentWindow<MTOverheadExp>
{
	public class MTOverheadExpSpecificationRow(IPOWindowInfo docWindow) : DocumentGridRow(docWindow)
	{
		[ObservableProperty]
		private int mTID = -1;

		[ObservableProperty]
		private string mTCode;

		[ObservableProperty]
		private string mTName = "";

		[ObservableProperty]
		private string unitCode;

		[ObservableProperty]
		private string unitBrief = "";

		[ObservableProperty]
		private decimal quantity;

		[ObservableProperty]
		private decimal summa;

		[ObservableProperty]
		private decimal distributedSumma;

		[ObservableProperty]
		private decimal distributedVATSumma;

		[ObservableProperty]
		private decimal distributedCurrSumma;

		[ObservableProperty]
		private bool include;

		[ObservableProperty]
		private bool vAT;

		[ObservableProperty]
		private int rowID;

		[ObservableProperty]
		private string fullName = "";

		[ObservableProperty]
		public decimal coeff;

		public short RowNum;

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
					OnPropertyChanged(__KnownINotifyPropertyChangedArgs.Quantity);
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
					OnPropertyChanged(__KnownINotifyPropertyChangedArgs.Summa);
				}
			}
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public decimal DistributedSumma
		{
			get
			{
				return distributedSumma;
			}
			set
			{
				if (!EqualityComparer<decimal>.Default.Equals(distributedSumma, value))
				{
					OnPropertyChanging(__KnownINotifyPropertyChangingArgs.DistributedSumma);
					distributedSumma = value;
					OnDistributedSummaChanged(value);
					OnPropertyChanged(__KnownINotifyPropertyChangedArgs.DistributedSumma);
				}
			}
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public decimal DistributedVATSumma
		{
			get
			{
				return distributedVATSumma;
			}
			set
			{
				if (!EqualityComparer<decimal>.Default.Equals(distributedVATSumma, value))
				{
					OnPropertyChanging(__KnownINotifyPropertyChangingArgs.DistributedVATSumma);
					distributedVATSumma = value;
					OnDistributedVATSummaChanged(value);
					OnPropertyChanged(__KnownINotifyPropertyChangedArgs.DistributedVATSumma);
				}
			}
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public decimal DistributedCurrSumma
		{
			get
			{
				return distributedCurrSumma;
			}
			set
			{
				if (!EqualityComparer<decimal>.Default.Equals(distributedCurrSumma, value))
				{
					OnPropertyChanging(__KnownINotifyPropertyChangingArgs.DistributedCurrSumma);
					distributedCurrSumma = value;
					OnDistributedCurrSummaChanged(value);
					OnPropertyChanged(__KnownINotifyPropertyChangedArgs.DistributedCurrSumma);
				}
			}
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public bool Include
		{
			get
			{
				return include;
			}
			set
			{
				if (!EqualityComparer<bool>.Default.Equals(include, value))
				{
					OnPropertyChanging(__KnownINotifyPropertyChangingArgs.Include);
					include = value;
					OnPropertyChanged(__KnownINotifyPropertyChangedArgs.Include);
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
		public decimal Coeff
		{
			get
			{
				return coeff;
			}
			set
			{
				if (!EqualityComparer<decimal>.Default.Equals(coeff, value))
				{
					OnPropertyChanging(__KnownINotifyPropertyChangingArgs.Coeff);
					coeff = value;
					OnPropertyChanged(__KnownINotifyPropertyChangedArgs.Coeff);
				}
			}
		}

		static MTOverheadExpSpecificationRow()
		{
			DocumentGridRow.RegisterPropertyValidators(typeof(MTOverheadExpSpecificationRow), cacheValidationResults: true, new Dictionary<string, Action<DocumentGridRow, DevExpress.XtraEditors.DXErrorProvider.ErrorInfo>>
			{
				{
					"DistributedSumma",
					(DocumentGridRow row, DevExpress.XtraEditors.DXErrorProvider.ErrorInfo info) =>
					{
						ValidateDistributedSumma((MTOverheadExpSpecificationRow)row, info);
					}
				},
				{
					"DistributedVATSumma",
					(DocumentGridRow row, DevExpress.XtraEditors.DXErrorProvider.ErrorInfo info) =>
					{
						ValidateDistributedVATSumma((MTOverheadExpSpecificationRow)row, info);
					}
				},
				{
					"DistributedCurrSumma",
					(DocumentGridRow row, DevExpress.XtraEditors.DXErrorProvider.ErrorInfo info) =>
					{
						ValidateDistributedCurrSumma((MTOverheadExpSpecificationRow)row, info);
					}
				}
			});
		}

		private static void ValidateDistributedSumma(MTOverheadExpSpecificationRow row, DevExpress.XtraEditors.DXErrorProvider.ErrorInfo info)
		{
			if (row.DocWindow.GetControlValue("DistrType").ToString() == DistrTypeSE.Manually.Value)
			{
				PropertyValidator.NonNegativeValidate(row.DistributedSumma, info, ArmSoft.SME.Trade.Common.Resources.DistributedSumma);
			}
		}

		private static void ValidateDistributedVATSumma(MTOverheadExpSpecificationRow row, DevExpress.XtraEditors.DXErrorProvider.ErrorInfo info)
		{
			if (row.DocWindow.GetControlValue("DistrType").ToString() == DistrTypeSE.Manually.Value)
			{
				PropertyValidator.NonNegativeValidate(row.DistributedVATSumma, info, ArmSoft.SME.Trade.Common.Resources.DistributedVATSumma);
			}
		}

		private static void ValidateDistributedCurrSumma(MTOverheadExpSpecificationRow row, DevExpress.XtraEditors.DXErrorProvider.ErrorInfo info)
		{
			if (row.DocWindow.GetControlValue("DistrType").ToString() == DistrTypeSE.Manually.Value)
			{
				PropertyValidator.NonNegativeValidate(row.DistributedCurrSumma, info, ArmSoft.SME.Trade.Common.Resources.DistributedCurrSumma);
			}
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		private void OnDistributedSummaChanged(decimal value)
		{
			ResetForceValidationBit("DistributedSumma");
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		private void OnDistributedVATSummaChanged(decimal value)
		{
			ResetForceValidationBit("DistributedVATSumma");
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		private void OnDistributedCurrSummaChanged(decimal value)
		{
			ResetForceValidationBit("DistributedCurrSumma");
		}
	}

	private DropDownCommentControl _baseDocumentControl;

	private PopupCalcEditExt _vatSumma;

	private PopupCalcEditExt _totalSumma;

	private ComboBoxEditExt _comment;

	private DropDownControl _owner;

	private PartnerContractGroupControls _supplierContractContainer;

	private TextEditExt _invoiceNumber;

	private DropDownControl _currCode;

	private ExchangeRateControl _currExChngRate;

	private string _oldDocCurrCode;

	private decimal _oldDocCurrExChngRate = 1m;

	private decimal _oldDocCurrExChngBase = 1m;

	private DropDownControl _vatType;

	private DropDownControl _distrType;

	private DropDownControl _distrSumBy;

	private DropDownControl _gridViewCols;

	private PopupCalcEditExt _summa;

	private CheckEditExt _allowMTListEdit;

	private DocumentGridControl _mtOverheadExpGrid;

	private TableView _mtOverheadExpTableView;

	private ObservableCollection<MTOverheadExpSpecificationRow> _mtOverheadExpsList;

	private DocumentGridColumn _includeCol;

	private DocumentGridColumn _mtCodeCol;

	private DocumentGridColumn _quantityCol;

	private DocumentGridColumn _distrSummaCol;

	private DocumentGridColumn _distrSummaWithVatCol;

	private DocumentGridColumn _distrCurSummaWithVatCol;

	private DocumentGridColumn _coeffCol;

	private Guid _baseISN = Guid.Empty;

	private TradeDocument _baseDocument;

	private int _rowID;

	private BarSubItem _barSubItemActions = new BarSubItem();

	private BarSubItem _barSubItemJournals = new BarSubItem();

	private ReportBarButtonItem _barButtonJumpTo;

	private ReportBarButtonItem _barButtonGoodsOperationsJournal;

	private ReportBarButtonItem _barButtonPartnerOperationJournal;

	protected override double WidthValue => 1110.0;

	protected override double HeightValue => 440.0;

	protected override double MinWidthValue => 720.0;

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
			if (_baseDocument == null)
			{
				_baseDocumentControl.Text = "";
			}
			else
			{
				_baseDocumentControl.Text = ArmSoft.SME.Trade.UI.Helpers.DocumentHelpers.GetDocumentDescription(_baseDocument, ArmSoft.SME.Common.Constants.Common.DocumentDescriptionMode.TypeDateNum);
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
			if (((IComparable)value).CompareTo((object?)_baseISN) == 0 && PO.State != (byte)DocumentStateSE.Processed)
			{
				return;
			}
			_baseISN = value;
			_mtOverheadExpsList.Clear();
			if (_baseISN == Guid.Empty)
			{
				BaseDocument = null;
				return;
			}
			BaseDocument = (TradeDocument)Settings.DocumentService.Load(_baseISN, withoutAccessCheck: true);
			if (PO.State == (byte)DocumentStateSE.Processed)
			{
				if (ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Trade.Common.Resources.FillUsingBaseQ, Settings.ProgramName, MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.Yes) == MessageBoxResult.Yes)
				{
					CreateBasedOnDoc();
				}
			}
			else
			{
				CreateBasedOnDoc(ForDraftDoc: true);
			}
			if (_distrType.Code == DistrTypeSE.Automatically && _summa.Value != 0m)
			{
				SummValueChanged();
			}
		}
	}

	public MTOverheadExpWindow()
	{
	}

	public MTOverheadExpWindow(MTOverheadExp doc)
		: base(doc)
	{
	}

	protected override void ConstructUI()
	{
		base.ConstructUI();
		LayoutGroup container = AddLayoutGroup(null, LayoutGroupView.Group, Orientation.Horizontal);
		_date = AddDateControl(container, Definition.Fields["DocDate"]);
		_date.EditValueChanged += new SafeDocumentEventHandler<EditValueChangedEventArgs>(Date_EditValueChanged, this).Handler;
		_date.Margin = new Thickness(0.0, 0.0, 10.0, 0.0);
		_docNum = AddButtonEditControl(container, Definition.Fields["DocNum"], 1);
		_docNum.MagicClick += new SafeDocumentEventHandler<MagicClickEventArgs>(DocNum_MagicClick, this).Handler;
		_docNum.EditValueChanged += new SafeDocumentEventHandler<EditValueChangedEventArgs>(DocNum_EditValueChanged, this).Handler;
		_docNum.AllowMagicButton = true;
		_docNum.MagicToolTip = ArmSoft.SME.Common.Resources.DocNextNumbers;
		LayoutGroup layoutGroup = AddLayoutGroup(null, LayoutGroupView.Group, Orientation.Horizontal);
		layoutGroup.HorizontalAlignment = HorizontalAlignment.Stretch;
		DocumentsJournal idv = new DocumentsJournal(this, ReportTypesSE.AllDocument, ((TradeDocDefinition)Definition).BaseDocTypes)
		{
			DocType = ((TradeDocDefinition)Definition).BaseDocTypes.First().ToString().PadLeft(2, '0'),
			DocState = DocumentStateSE.DraftOrProcessed.ValueUI
		};
		_baseDocumentControl = AddDropDownCommentControl(layoutGroup, ArmSoft.SME.Common.Resources.BaseDoc, isRequired: true, idv, 50, HorizontalAlignment.Stretch, "BaseDocumentControl");
		_baseDocumentControl.ClickDropDown += new SafeDocumentEventHandler<DropDownControlEventArgs>(BaseDocument_ClickDropDown, this).Handler;
		_baseDocumentControl.EditKeyValueChanged += new SafeDocumentEventHandler<EditKeyValueChangedEventArgs>(BaseDocument_EditKeyValueChanged, this).Handler;
		_baseDocumentControl.BtnClearClicked += new SafeDocumentEventHandler<EditKeyValueChangedEventArgs>(BaseDocumentDelete_Click, this).Handler;
		_baseDocumentControl.BtnSearchClicked += new SafeDocumentEventHandler<EditKeyValueChangedEventArgs>(BaseDocumentSearch_Click, this).Handler;
		_baseDocumentControl.ShowClearButton = true;
		_baseDocumentControl.ShowSearchButton = true;
		_baseDocumentControl.IsTextEditable = false;
		DXTabControl tc = AddTabControl(null);
		LayoutGroup container2 = AddTabbedGroup(tc, ArmSoft.SME.Common.Resources.General, "General");
		_supplierContractContainer = AddPartnerContractGroupControls(container2, (PartnerContractGroupDefinition)Definition.Fields["PartnerContractGroup"], _date, null, null);
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
		LayoutGroup container3 = AddTabbedGroup(tc, ArmSoft.SME.Trade.Common.Resources.MTLIST, "MtList");
		LayoutGroupExt container4 = AddLayoutGroup(container3, LayoutGroupView.Group, Orientation.Horizontal);
		_gridViewCols = AddSmartEnumDropDownControl(container4, Definition.Fields["GridViewColsType"], allowAdditionButtons: false, fieldWithCaption: true, HorizontalAlignment.Stretch);
		_gridViewCols.EditValueChanged += new SafeDocumentEventHandler<EditValueChangedEventArgs>(GridViewCols_EditValueChanged, this).Handler;
		LayoutGroup container5 = AddLayoutGroup(container3, LayoutGroupView.Group, Orientation.Horizontal);
		_currCode = AddDropDownControl(container5, Definition.Fields["CurrCode"], new Currency(shortMode: true), allowAdditionButtons: false, fieldWithCaption: true, HorizontalAlignment.Stretch);
		_currCode.EditValueChanged += new SafeDocumentEventHandler<EditValueChangedEventArgs>(CurrCode_EditValueChanged, this).Handler;
		LayoutGroup container6 = AddLayoutGroup(container3, LayoutGroupView.Group, Orientation.Horizontal);
		_currExChngRate = AddExchangeRateControl(container6, Definition.Fields["ExchangeRate"]);
		_currExChngRate.EditValueChanged += new SafeDocumentEventHandler<RateEditValueChangedEventArgs>(CurrExChngRate_EditValueChanged, this).Handler;
		_summa = AddNumericControl(container5, Definition.Fields["SummWithoutVAT"]);
		_summa.EditValueChanged += new SafeDocumentEventHandler<EditValueChangedEventArgs>(Summ_EditValueChanged, this).Handler;
		_vatSumma = AddNumericControl(container6, Definition.Fields["SummVAT"], useCurrencyFormat: true, HorizontalAlignment.Right);
		LayoutGroup container7 = AddLayoutGroup(container3, LayoutGroupView.Group, Orientation.Horizontal);
		_vatType = AddDropDownControl(container7, Definition.Fields["VATType"], new VATTypes(), allowAdditionButtons: false, fieldWithCaption: true, HorizontalAlignment.Stretch);
		_vatType.EditValueChanged += new SafeDocumentEventHandler<EditValueChangedEventArgs>(VATType_EditValueChanged, this).Handler;
		_totalSumma = AddNumericControl(container7, Definition.Fields["Summ"], useCurrencyFormat: true, HorizontalAlignment.Right);
		_distrType = AddSmartEnumDropDownControl(container3, Definition.Fields["DistrType"], allowAdditionButtons: false, fieldWithCaption: true, HorizontalAlignment.Stretch);
		_distrType.EditValueChanged += new SafeDocumentEventHandler<EditValueChangedEventArgs>(DistrType_EditValueChanged, this).Handler;
		_distrSumBy = AddSmartEnumDropDownControl(container3, Definition.Fields["DistrSumBy"], allowAdditionButtons: false, fieldWithCaption: true, HorizontalAlignment.Stretch);
		_distrSumBy.EditValueChanged += new SafeDocumentEventHandler<EditValueChangedEventArgs>(DistrSumBy_EditValueChanged, this).Handler;
		_allowMTListEdit = AddBooleanControl(container3, Definition.Fields["AllowMTListEdit"]);
		_allowMTListEdit.EditValueChanged += new SafeDocumentEventHandler<EditValueChangedEventArgs>(AllowMTListEdit_EditValueChanged, this).Handler;
		_mtOverheadExpGrid = AddGrid(container3, Definition.Grids["MTList"], HorizontalAlignment.Stretch, VerticalAlignment.Stretch);
		_mtOverheadExpTableView = _mtOverheadExpGrid.View;
		_mtOverheadExpTableView.CellValueChanged += new SafeDocumentEventHandler<CellValueChangedEventArgs>(MTOverheadExpTableView_CellValueChanged, this).Handler;
		_mtOverheadExpTableView.ColumnHeaderClick += new SafeDocumentEventHandler<ColumnHeaderClickEventArgs>(MTOverheadExpTableView_ColumnHeaderClick, this).Handler;
		_mtOverheadExpGrid.IsRequired = true;
		_mtOverheadExpGrid.AllowAdd = false;
		_mtOverheadExpGrid.AllowDelete = false;
		_mtOverheadExpGrid.AllowDuplicate = false;
		_mtOverheadExpGrid.ShowButtons = false;
		_mtOverheadExpGrid.AddRowIndicator(35);
		_includeCol = _mtOverheadExpGrid.AddBooleanColumn("Include", "");
		_includeCol.Width = 20.0;
		_includeCol.AllowEditing = DefaultBoolean.False;
		_includeCol.AllowSorting = DefaultBoolean.True;
		_mtCodeCol = _mtOverheadExpGrid.AddDropDownColumn(Definition.Grids["MTList"].Columns["MTCode"], new GoodsFilter(), allowAdditionButtons: true);
		_mtCodeCol.Width = 125.0;
		_mtCodeCol.AllowEditing = DefaultBoolean.False;
		DocumentGridColumn documentGridColumn = _mtOverheadExpGrid.AddStringColumn(Definition.Grids["MTList"].Columns["MTName"]);
		documentGridColumn.AllowEditing = DefaultBoolean.False;
		documentGridColumn.Width = 225.0;
		_mtOverheadExpGrid.AddStringColumn(Definition.Grids["MTList"].Columns["UnitBrief"]).AllowEditing = DefaultBoolean.False;
		_quantityCol = _mtOverheadExpGrid.AddNumberColumn(Definition.Grids["MTList"].Columns["Quantity"], showTotal: true);
		_quantityCol.AllowEditing = DefaultBoolean.False;
		_distrSummaCol = _mtOverheadExpGrid.AddNumberColumn(Definition.Grids["MTList"].Columns["DistributedSumma"]);
		_distrSummaCol.AllowEditing = DefaultBoolean.False;
		_distrSummaWithVatCol = _mtOverheadExpGrid.AddNumberColumn(Definition.Grids["MTList"].Columns["DistributedVATSumma"]);
		_distrSummaWithVatCol.AllowEditing = DefaultBoolean.False;
		_distrCurSummaWithVatCol = _mtOverheadExpGrid.AddNumberColumn(Definition.Grids["MTList"].Columns["DistributedCurrSumma"]);
		_distrCurSummaWithVatCol.AllowEditing = DefaultBoolean.False;
		_coeffCol = _mtOverheadExpGrid.AddNumberColumn(Definition.Grids["MTList"].Columns["Coeff"]);
		_coeffCol.AllowEditing = DefaultBoolean.False;
		_coeffCol.Visible = false;
		_coeffCol.ShowInColumnChooser = false;
		_mtOverheadExpsList = new ObservableCollection<MTOverheadExpSpecificationRow>();
		foreach (GridColumn column in _mtOverheadExpGrid.Grid.Columns)
		{
			column.AllowSorting = DefaultBoolean.True;
			column.AllowColumnFiltering = DefaultBoolean.True;
			column.FilterPopupMode = FilterPopupMode.CheckedList;
		}
		_supplierContractContainer.DocSummaControl = _totalSumma;
		_supplierContractContainer.DocCurrCodeControl = _currCode;
		_vatType.Code = "1";
	}

	protected override void OnNew()
	{
		base.OnNew();
		if (_baseISN == Guid.Empty)
		{
			_date.EditValue = Settings.HostingEnvironment.CurrentDate;
		}
		CurrCodeValueChanged();
		_owner.Code = Settings.LoginService.UserName;
		_gridViewCols.Code = MTOverheadExpGridColsViewSE.NCCSummWithVAT.Value.ToString();
		_setFocusOnActivation = _date;
	}

	protected override void OnNewAfterSave()
	{
		base.OnNewAfterSave();
		ObservableCollection<MTOverheadExpSpecificationRow> observableCollection = new ObservableCollection<MTOverheadExpSpecificationRow>();
		foreach (MTOverheadExpSpecificationRow mtOverheadExps in _mtOverheadExpsList)
		{
			observableCollection.Add(mtOverheadExps);
		}
		_mtOverheadExpsList.Clear();
		foreach (MTOverheadExpSpecificationRow item2 in observableCollection)
		{
			MTOverheadExpSpecificationRow item = new MTOverheadExpSpecificationRow(this)
			{
				MTCode = item2.MTCode,
				MTID = item2.MTID,
				RowNum = item2.RowNum,
				UnitCode = item2.UnitCode,
				Quantity = item2.Quantity,
				Summa = item2.Summa,
				DistributedCurrSumma = item2.DistributedCurrSumma,
				DistributedVATSumma = item2.DistributedVATSumma,
				DistributedSumma = item2.DistributedSumma,
				VAT = item2.VAT,
				MTName = item2.MTName,
				FullName = item2.FullName,
				UnitBrief = item2.UnitBrief,
				Coeff = item2.Coeff,
				Include = item2.Include,
				RowID = item2.RowID
			};
			_mtOverheadExpsList.Add(item);
		}
		_summa.EditValue = 0m;
		_vatSumma.EditValue = 0m;
		_totalSumma.EditValue = 0m;
		_owner.Code = Settings.LoginService.UserName;
	}

	protected override void OnEdit()
	{
		base.OnEdit();
		_setFocusOnActivation = _docNum;
	}

	protected override void FillDocument(StoreRequest sr)
	{
		base.FillDocument(sr);
		PO.BaseISN = BaseISN;
		PO.MTList.Clear();
		foreach (MTOverheadExpSpecificationRow mtOverheadExps in _mtOverheadExpsList)
		{
			PO.MTList.Add(new MTOverheadExp.MTOverheadExpSpecificationRow
			{
				MTCode = mtOverheadExps.MTCode,
				MTID = mtOverheadExps.MTID,
				MTName = mtOverheadExps.MTName,
				RowNum = mtOverheadExps.RowNum,
				UnitCode = mtOverheadExps.UnitCode,
				Quantity = mtOverheadExps.Quantity,
				Summa = mtOverheadExps.Summa,
				VAT = mtOverheadExps.VAT,
				Coeff = mtOverheadExps.Coeff,
				RowID = mtOverheadExps.RowID,
				DistributedCurrSumma = mtOverheadExps.DistributedCurrSumma,
				DistributedSumma = mtOverheadExps.DistributedSumma,
				DistributedVATSumma = mtOverheadExps.DistributedVATSumma,
				Include = mtOverheadExps.Include
			});
		}
	}

	protected override void LoadDataToControls()
	{
		base.LoadDataToControls();
		_mtOverheadExpGrid.ItemsSource = _mtOverheadExpsList;
		_baseISN = PO.BaseISN;
		if (_baseISN == Guid.Empty)
		{
			BaseDocument = null;
		}
		else
		{
			BaseDocument = (TradeDocument)Settings.DocumentService.Load(_baseISN, withoutAccessCheck: true);
			if (BaseDocument.POType == DocumentTypeSE.MTIncome.Value)
			{
				BaseDocument = (MTIncomeBase)BaseDocument;
			}
			else if (BaseDocument.POType == DocumentTypeSE.MTIncomeRetail.Value)
			{
				BaseDocument = (MTIncomeRetail)BaseDocument;
			}
			else
			{
				BaseDocument = (MTIncomeImport)BaseDocument;
			}
			if (string.IsNullOrWhiteSpace(PO.Storage) && BaseDocument != null)
			{
				PO.Storage = BaseDocument.Storage;
			}
		}
		_mtOverheadExpsList.Clear();
		foreach (MTOverheadExp.MTOverheadExpSpecificationRow mT in PO.MTList)
		{
			MTOverheadExpSpecificationRow item = new MTOverheadExpSpecificationRow(this)
			{
				MTCode = mT.MTCode,
				MTID = mT.MTID,
				RowNum = mT.RowNum,
				UnitCode = mT.UnitCode,
				Quantity = mT.Quantity,
				Summa = mT.Summa,
				DistributedCurrSumma = mT.DistributedCurrSumma,
				DistributedVATSumma = mT.DistributedVATSumma,
				DistributedSumma = mT.DistributedSumma,
				VAT = mT.VAT,
				MTName = mT.MTName,
				FullName = mT.FullName,
				UnitBrief = mT.UnitBrief,
				Coeff = mT.Coeff,
				Include = mT.Include,
				RowID = mT.RowID
			};
			_mtOverheadExpsList.Add(item);
		}
		_mtOverheadExpGrid.ItemsSource = _mtOverheadExpsList;
		if (IsDraftOrDeleted && _allowMTListEdit.IsChecked.Value)
		{
			foreach (MTOverheadExpSpecificationRow row in _mtOverheadExpsList)
			{
				MTOverheadExp.MTOverheadExpSpecificationRow mTOverheadExpSpecificationRow = PO.MTList.Find((MTOverheadExp.MTOverheadExpSpecificationRow r) => r.RowID == row.RowID && r.MTID == row.MTID);
				if (mTOverheadExpSpecificationRow != null)
				{
					row.Include = mTOverheadExpSpecificationRow.Include;
					if (_distrType.Code == DistrTypeSE.Manually.Value)
					{
						row.DistributedSumma = mTOverheadExpSpecificationRow.DistributedSumma;
						row.DistributedVATSumma = mTOverheadExpSpecificationRow.DistributedVATSumma;
						row.DistributedCurrSumma = mTOverheadExpSpecificationRow.DistributedCurrSumma;
					}
				}
				else
				{
					row.Include = false;
					row.DistributedSumma = 0m;
					row.DistributedVATSumma = 0m;
					row.DistributedCurrSumma = 0m;
				}
			}
			if (_distrType.Code == DistrTypeSE.Automatically && _summa.Value != 0m)
			{
				SummValueChanged();
			}
		}
		if (!(_baseISN == Guid.Empty) && !PO.StoredInDb && _viewMode != WindowViewMode.Copy)
		{
			CreateBasedOnDoc();
		}
		_supplierContractContainer.ValueChangedForLoadDoc();
		CalculatePartSumma();
		DistrType_EditValueChanged();
		GridViewCols_EditValueChanged();
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
			return;
		}
		_vatSumma.IsReadOnly = true;
		_totalSumma.IsReadOnly = true;
		bool flag = State == DocumentStateSE.Draft.Value || State == DocumentStateSE.Deleted.Value;
		bool flag2 = _viewMode == WindowViewMode.Edit && !flag;
		_date.IsReadOnly = flag2;
		_currCode.IsReadOnly = flag2;
		_currExChngRate.IsReadOnly = flag2 || _currCode.Code == "AMD";
		_baseDocumentControl.IsReadOnly = flag2;
		_summa.IsReadOnly = flag2;
		_allowMTListEdit.IsReadOnly = flag2;
		_gridViewCols.IsReadOnly = flag2;
		_vatType.IsReadOnly = flag2;
		_distrType.IsReadOnly = flag2;
		_distrSumBy.IsReadOnly = flag2;
		if (_distrType.Code == DistrTypeSE.Manually.Value)
		{
			_allowMTListEdit.IsReadOnly = true;
		}
		else
		{
			_allowMTListEdit.IsReadOnly = flag2;
		}
		_includeCol.AllowEditing = ((!_allowMTListEdit.IsChecked.Value || flag2) ? DefaultBoolean.False : DefaultBoolean.True);
		foreach (GridColumn column in _mtOverheadExpGrid.Grid.Columns)
		{
			column.ReadOnly = flag2;
		}
	}

	private void Date_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		if (_date.DoValidate() && _date.EditValue.HasValue)
		{
			CurrCodeValueChanged();
		}
	}

	private void CurrCode_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		if (_distrType.Code == DistrTypeSE.Automatically)
		{
			bool flag = false;
			if (_summa.Value != 0m && ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Trade.Common.Resources.ValidateChangePrices, Settings.ProgramName, MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.Yes) == MessageBoxResult.Yes)
			{
				CurrCode_Validate();
				flag = true;
				UpdateSumms();
				SummValueChanged();
			}
			if (!flag)
			{
				CurrCode_Validate();
			}
			if (!string.IsNullOrWhiteSpace(_currCode.Code) && _currCode.DoValidate())
			{
				_oldDocCurrCode = _currCode.Code;
				_oldDocCurrExChngRate = _currExChngRate.Rate;
				_oldDocCurrExChngBase = _currExChngRate.Base;
			}
		}
		else
		{
			CurrCode_Validate();
			CalcTotalSumms();
		}
		_mtOverheadExpGrid.Grid.RefreshData();
	}

	private void CurrCode_Validate()
	{
		if (string.IsNullOrWhiteSpace(_currCode.Code) || _currCode.Code == "AMD" || !_currCode.DoValidate())
		{
			_currExChngRate.IsReadOnly = true;
			_currExChngRate.Rate = 1m;
			_currExChngRate.Base = 1m;
		}
		else
		{
			_currExChngRate.IsReadOnly = PO.StoredInDb && PO.State == DocumentStateSE.Processed.Value;
			CurrCodeValueChanged();
		}
		_currExChngRate.DoValidate();
	}

	private void CalcDistributedCurrSumma()
	{
		ExchangeRate exchangeRates = TradeSettings.CreateTradeClient().CommonServices.GetExchangeRates(_currCode.Code, _date.EditValue.Value);
		foreach (MTOverheadExpSpecificationRow mtOverheadExps in _mtOverheadExpsList)
		{
			mtOverheadExps.DistributedSumma = ArmSoft.SME.Trade.Common.Helpers.PriceHelper.GetSumCurr(mtOverheadExps.DistributedSumma, exchangeRates);
		}
	}

	private void GetCurrExChngRate()
	{
		if (string.IsNullOrWhiteSpace(_currCode.Code) || _currCode.Code == "AMD" || !_currCode.DoValidate())
		{
			_currExChngRate.IsReadOnly = true;
			_currExChngRate.Rate = 1m;
			_currExChngRate.Base = 1m;
		}
		else
		{
			_currExChngRate.IsReadOnly = PO.StoredInDb && PO.State == (byte)DocumentStateSE.Processed;
			CurrCodeValueChanged();
		}
		_currExChngRate.DoValidate();
	}

	private void UpdateSumms()
	{
		if (_oldDocCurrCode == "AMD")
		{
			if (_currExChngRate.Rate != 0m)
			{
				_summa.Value = ArmSoft.SME.Trade.Common.Helpers.PriceHelper.GetSumCurr(_summa.Value, new ExchangeRate(_currExChngRate.Base, _currExChngRate.Rate));
			}
		}
		else if (!(_currExChngRate.Rate == 0m) && !(_oldDocCurrExChngBase == 0m))
		{
			_summa.Value = (_summa.Value * _oldDocCurrExChngRate / _oldDocCurrExChngBase / _currExChngRate.Rate * _currExChngRate.Base).RoundAs(2);
		}
	}

	private void CalcTotalSumms()
	{
		_summa.Value = 0m;
		foreach (MTOverheadExpSpecificationRow mtOverheadExps in _mtOverheadExpsList)
		{
			if (mtOverheadExps.Include)
			{
				_summa.Value += (mtOverheadExps.DistributedSumma * _currExChngRate.Base / _currExChngRate.Rate).RoundAs(2);
			}
		}
	}

	private void CurrExChngRate_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		if (_currExChngRate.DoValidate())
		{
			if (_summa.Value != 0m)
			{
				SummValueChanged();
			}
			CalculatePartSumma();
		}
	}

	private void CurrCodeValueChanged()
	{
		if (!string.IsNullOrWhiteSpace(_currCode.Code) && _currCode.Code != "AMD" && _date.DoValidate() && _currCode.DoValidate() && !_currExChngRate.IsReadOnly)
		{
			try
			{
				ExchangeRate exchangeRates = TradeSettings.CreateTradeClient().CommonServices.GetExchangeRates(_currCode.Code, _date.EditValue.Value);
				_currExChngRate.Rate = exchangeRates.Rate;
				_currExChngRate.Base = exchangeRates.Base;
			}
			catch (Exception ex)
			{
				Settings.ErrorHandler(ex);
			}
		}
	}

	private void VATType_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		if (_distrType.Code == DistrTypeSE.Automatically)
		{
			VATTypeValidate(Convert.ToString(e.OldValue));
		}
		else
		{
			VATTypeValidateManually(Convert.ToString(e.OldValue));
		}
		_mtOverheadExpGrid.Grid.RefreshData();
	}

	private void VATTypeValidateManually(string VATTypeOldValue)
	{
		foreach (MTOverheadExpSpecificationRow mtOverheadExps in _mtOverheadExpsList)
		{
			CalcSumms(CommonConst.SummaTypes.NCCSummWithoutVAT, mtOverheadExps);
			CalcTotalSumms();
		}
	}

	private void VATTypeValidate(string VATTypeOldValue)
	{
		switch (_vatType.Code.Trim())
		{
		case "1":
			switch (VATTypeOldValue)
			{
			case "3":
			case "4":
				_vatSumma.EditValue = ArmSoft.SME.Trade.UI.Helpers.PriceHelper.CalcVATAmountByType(_summa.Value, "1", itemVAT: true);
				break;
			case "2":
				_vatSumma.EditValue = ArmSoft.SME.Trade.UI.Helpers.PriceHelper.CalcVATAmountByType(_summa.Value, "2", itemVAT: true);
				_summa.EditValue = Convert.ToDecimal(_summa.EditValue) - Convert.ToDecimal(_vatSumma.EditValue);
				break;
			default:
				_vatSumma.EditValue = ArmSoft.SME.Trade.UI.Helpers.PriceHelper.CalcVATAmountByType(_summa.Value, "1", itemVAT: true);
				break;
			}
			_totalSumma.EditValue = Convert.ToDecimal(_vatSumma.EditValue) + Convert.ToDecimal(_summa.EditValue);
			break;
		case "2":
			switch (VATTypeOldValue)
			{
			case "3":
			case "4":
				_vatSumma.EditValue = ArmSoft.SME.Trade.UI.Helpers.PriceHelper.CalcVATAmountByType(_summa.Value, "1", itemVAT: true);
				_summa.EditValue = Convert.ToDecimal(_vatSumma.EditValue) + Convert.ToDecimal(_summa.EditValue);
				break;
			case "2":
				_vatSumma.EditValue = ArmSoft.SME.Trade.UI.Helpers.PriceHelper.CalcVATAmountByType(_summa.Value, "2", itemVAT: true);
				break;
			default:
				_vatSumma.EditValue = ArmSoft.SME.Trade.UI.Helpers.PriceHelper.CalcVATAmountByType(_summa.Value, "1", itemVAT: true);
				_summa.EditValue = Convert.ToDecimal(_vatSumma.EditValue) + Convert.ToDecimal(_summa.EditValue);
				break;
			}
			_totalSumma.EditValue = _summa.EditValue;
			break;
		case "3":
		case "4":
			switch (VATTypeOldValue)
			{
			case "2":
				_vatSumma.EditValue = 0m;
				_summa.EditValue = Convert.ToDecimal(_summa.EditValue) - ArmSoft.SME.Trade.UI.Helpers.PriceHelper.CalcVATAmountByType(_summa.Value, "2", itemVAT: true);
				break;
			default:
				_vatSumma.EditValue = 0m;
				break;
			case "3":
			case "4":
				break;
			}
			_totalSumma.EditValue = _summa.EditValue;
			break;
		}
		CalculatePartSumma();
	}

	private void Summ_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		CalcSummAndSummVAT();
		SummValueChanged();
	}

	private void CalcSummAndSummVAT()
	{
		if (_vatType.Code.Trim().InList("1", "2"))
		{
			_vatSumma.EditValue = ArmSoft.SME.Trade.UI.Helpers.PriceHelper.CalcVATAmountByType(Convert.ToDecimal(_summa.EditValue), _vatType.Code, itemVAT: true);
		}
		else
		{
			_vatSumma.EditValue = 0m;
		}
		if (_vatType.Code.Trim() == "1")
		{
			_totalSumma.EditValue = Convert.ToDecimal(_summa.EditValue) + Convert.ToDecimal(_vatSumma.EditValue);
		}
		else
		{
			_totalSumma.EditValue = _summa.EditValue;
		}
		CalculatePartSumma();
	}

	private void SummValueChanged()
	{
		try
		{
			if (!(_distrType.Code == DistrTypeSE.Automatically) || _mtOverheadExpsList.Count == 0)
			{
				return;
			}
			List<PartyRow> mTDocRowsInMTHI = TradeSettings.CreateTradeClient().CommonServices.GetMTDocRowsInMTHI(BaseISN);
			ExchangeRate exchangeRate = new ExchangeRate(_currExChngRate.Base, _currExChngRate.Rate);
			decimal num = 0m;
			decimal num2 = 0m;
			decimal num3 = 0m;
			if (!mTDocRowsInMTHI.Any())
			{
				throw new ASException(ArmSoft.SME.Trade.Common.Resources.BaseDocNotInDocs);
			}
			if (_currExChngRate.DoValidate())
			{
				num = ((!(_vatType.Code == "2")) ? ArmSoft.SME.Trade.Common.Helpers.PriceHelper.GetSumNNC(_summa.Value, exchangeRate) : ArmSoft.SME.Trade.Common.Helpers.PriceHelper.GetSumNNC(_summa.Value - _vatSumma.Value, exchangeRate));
				num2 = ArmSoft.SME.Trade.Common.Helpers.PriceHelper.GetSumNNC(_vatSumma.Value, exchangeRate);
				ExchangeRate exchangeRates = TradeSettings.CreateTradeClient().CommonServices.GetExchangeRates(Settings.ParametersService.MTCurr(), _date.EditValue.Value);
				num3 = ArmSoft.SME.Trade.Common.Helpers.PriceHelper.GetSumCurr(num2 + num, exchangeRates);
			}
			decimal num4 = 0m;
			decimal num5 = 0m;
			decimal num6 = 0m;
			decimal num7 = 0m;
			decimal num8 = 0m;
			decimal num9 = 0m;
			decimal num10 = 0m;
			decimal num11 = 0m;
			decimal num12 = 0m;
			decimal num13 = 0m;
			foreach (MTOverheadExpSpecificationRow mtOverheadExps in _mtOverheadExpsList)
			{
				if (mtOverheadExps.Include)
				{
					if (_distrSumBy.Code == DistrSumByTypeSE.Sum.Value)
					{
						num7 += mtOverheadExps.Summa;
						_rowID = mtOverheadExps.RowID;
						PartyRow partyRow = mTDocRowsInMTHI.FirstOrDefault((PartyRow x) => x.GridRow == _rowID);
						if (partyRow != null)
						{
							num9 += partyRow.VATSumm;
							num10 += partyRow.CurrSumm;
						}
					}
					else if (_distrSumBy.Code == DistrSumByTypeSE.Quantity.Value)
					{
						num8 += mtOverheadExps.Quantity;
					}
					else
					{
						num8 += mtOverheadExps.Quantity * mtOverheadExps.Coeff;
					}
				}
				if (num == 0m)
				{
					mtOverheadExps.DistributedSumma = 0m;
					mtOverheadExps.DistributedVATSumma = 0m;
					mtOverheadExps.DistributedCurrSumma = 0m;
				}
			}
			if (num == 0m)
			{
				num4 = num7;
				num5 = num9;
				num6 = num10;
			}
			else
			{
				if (num7 != 0m || num8 != 0m)
				{
					foreach (MTOverheadExpSpecificationRow mtOverheadExps2 in _mtOverheadExpsList)
					{
						if (mtOverheadExps2.Include)
						{
							_rowID = mtOverheadExps2.RowID;
							PartyRow partyRow = mTDocRowsInMTHI.FirstOrDefault((PartyRow x) => x.GridRow == _rowID);
							if (_distrSumBy.Code == DistrSumByTypeSE.Sum.Value)
							{
								if (num7 != 0m)
								{
									mtOverheadExps2.DistributedSumma = (mtOverheadExps2.Summa / num7 * num).RoundAs(2);
								}
								if (partyRow != null)
								{
									if (num9 != 0m)
									{
										mtOverheadExps2.DistributedVATSumma = (partyRow.VATSumm / num9 * (num2 + num)).RoundAs(2);
									}
									if (num10 != 0m)
									{
										mtOverheadExps2.DistributedCurrSumma = (partyRow.CurrSumm / num10 * num3).RoundAs(2);
									}
								}
							}
							else if (_distrSumBy.Code == DistrSumByTypeSE.Quantity.Value)
							{
								if (num8 != 0m)
								{
									mtOverheadExps2.DistributedSumma = (mtOverheadExps2.Quantity / num8 * num).RoundAs(2);
								}
								if (partyRow != null && num8 != 0m)
								{
									mtOverheadExps2.DistributedVATSumma = (partyRow.Qty / num8 * (num2 + num)).RoundAs(2);
									mtOverheadExps2.DistributedCurrSumma = (partyRow.Qty / num8 * num3).RoundAs(2);
								}
							}
							else
							{
								if (num8 != 0m)
								{
									mtOverheadExps2.DistributedSumma = (mtOverheadExps2.Quantity * mtOverheadExps2.Coeff / num8 * num).RoundAs(2);
								}
								if (partyRow != null && num8 != 0m)
								{
									mtOverheadExps2.DistributedVATSumma = (partyRow.Qty * mtOverheadExps2.Coeff / num8 * (num2 + num)).RoundAs(2);
									mtOverheadExps2.DistributedCurrSumma = (partyRow.Qty * mtOverheadExps2.Coeff / num8 * num3).RoundAs(2);
								}
							}
							num4 += mtOverheadExps2.DistributedSumma;
							num5 += mtOverheadExps2.DistributedVATSumma;
							num6 += mtOverheadExps2.DistributedCurrSumma;
						}
						else
						{
							mtOverheadExps2.DistributedSumma = 0m;
							mtOverheadExps2.DistributedVATSumma = 0m;
							mtOverheadExps2.DistributedCurrSumma = 0m;
						}
					}
				}
				else
				{
					foreach (MTOverheadExpSpecificationRow mtOverheadExps3 in _mtOverheadExpsList)
					{
						mtOverheadExps3.DistributedSumma = 0m;
						mtOverheadExps3.DistributedVATSumma = 0m;
						mtOverheadExps3.DistributedCurrSumma = 0m;
					}
				}
				num11 = num - num4;
				num12 = num + num2 - num5;
				num13 = num3 - num6;
				if (num11 != 0m)
				{
					for (int num14 = _mtOverheadExpsList.Count - 1; num14 >= 0; num14 += -1)
					{
						if (_mtOverheadExpsList[num14].Include)
						{
							_mtOverheadExpsList[num14].DistributedSumma += num11;
							break;
						}
					}
				}
				if (num12 != 0m)
				{
					for (int num15 = _mtOverheadExpsList.Count - 1; num15 >= 0; num15 += -1)
					{
						if (_mtOverheadExpsList[num15].Include)
						{
							_mtOverheadExpsList[num15].DistributedVATSumma += num12;
							break;
						}
					}
				}
				if (num13 != 0m)
				{
					for (int num16 = _mtOverheadExpsList.Count - 1; num16 >= 0; num16 += -1)
					{
						if (_mtOverheadExpsList[num16].Include)
						{
							_mtOverheadExpsList[num16].DistributedCurrSumma += num13;
							break;
						}
					}
				}
			}
			_mtOverheadExpGrid.Grid.RefreshData();
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	private void DistrSumBy_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		_coeffCol.Visible = Convert.ToString(e.NewValue) == Convert.ToString(DistrSumByTypeSE.Coeff.Value);
		_coeffCol.ShowInColumnChooser = _coeffCol.Visible;
		SummValueChanged();
	}

	private void MTOverheadExpTableView_CellValueChanged(object sender, CellValueChangedEventArgs e)
	{
		if (_mtOverheadExpGrid.IsReadOnly)
		{
			return;
		}
		MTOverheadExpSpecificationRow mtOverheadExpGridRow = (MTOverheadExpSpecificationRow)_mtOverheadExpGrid.Grid.GetRow(e.RowHandle);
		if (e.Column == _includeCol)
		{
			if (!_allowMTListEdit.IsChecked.Value)
			{
				return;
			}
			SummValueChanged();
		}
		else if (e.Column == _distrSummaCol)
		{
			CalcSumms(CommonConst.SummaTypes.NCCSummWithoutVAT, mtOverheadExpGridRow);
		}
		else if (e.Column == _distrSummaWithVatCol)
		{
			CalcSumms(CommonConst.SummaTypes.NCCSummWithVAT, mtOverheadExpGridRow);
		}
		else if (e.Column == _distrCurSummaWithVatCol)
		{
			CalcSumms(CommonConst.SummaTypes.CurrSummWithVAT, mtOverheadExpGridRow);
		}
		if (_distrType.Code == DistrTypeSE.Manually)
		{
			CalcTotalSumms();
		}
	}

	private void CalcSumms(CommonConst.SummaTypes costType, MTOverheadExpSpecificationRow mtOverheadExpGridRow)
	{
		ExchangeRate exchangeRates = TradeSettings.CreateTradeClient().CommonServices.GetExchangeRates(Settings.ParametersService.MTCurr(), _date.EditValue.Value);
		decimal rate = exchangeRates.Rate;
		decimal num = exchangeRates.Base;
		switch (costType)
		{
		case CommonConst.SummaTypes.NCCSummWithoutVAT:
		{
			decimal num4 = ArmSoft.SME.Trade.UI.Helpers.PriceHelper.CalcVATAmountByType(mtOverheadExpGridRow.DistributedSumma, _vatType.Code, itemVAT: true);
			mtOverheadExpGridRow.DistributedVATSumma = mtOverheadExpGridRow.DistributedSumma + num4;
			mtOverheadExpGridRow.DistributedCurrSumma = (mtOverheadExpGridRow.DistributedVATSumma * num / rate).RoundAs(2);
			break;
		}
		case CommonConst.SummaTypes.NCCSummWithVAT:
		{
			string vatType = ((_vatType.Code == "1" || _vatType.Code == "2") ? "2" : _vatType.Code);
			decimal num3 = ArmSoft.SME.Trade.UI.Helpers.PriceHelper.CalcVATAmountByType(mtOverheadExpGridRow.DistributedVATSumma, vatType, itemVAT: true);
			mtOverheadExpGridRow.DistributedSumma = mtOverheadExpGridRow.DistributedVATSumma - num3;
			mtOverheadExpGridRow.DistributedCurrSumma = (mtOverheadExpGridRow.DistributedVATSumma * num / rate).RoundAs(2);
			break;
		}
		case CommonConst.SummaTypes.CurrSummWithVAT:
		{
			mtOverheadExpGridRow.DistributedVATSumma = (mtOverheadExpGridRow.DistributedCurrSumma * rate / num).RoundAs(2);
			decimal num2 = ArmSoft.SME.Trade.UI.Helpers.PriceHelper.CalcVATAmountByType(mtOverheadExpGridRow.DistributedVATSumma, _vatType.Code, itemVAT: true);
			mtOverheadExpGridRow.DistributedSumma = mtOverheadExpGridRow.DistributedVATSumma - num2;
			break;
		}
		}
	}

	private void MTOverheadExpTableView_ColumnHeaderClick(object sender, ColumnHeaderClickEventArgs e)
	{
		if (e.Column == _includeCol && _mtOverheadExpGrid.Grid.VisibleRowCount != 0)
		{
			SelectDeselect();
			e.Handled = true;
		}
	}

	private void SelectDeselect()
	{
		if (_includeCol.AllowEditing == DefaultBoolean.False)
		{
			return;
		}
		bool flag = true;
		foreach (MTOverheadExpSpecificationRow mtOverheadExps in _mtOverheadExpsList)
		{
			if (!mtOverheadExps.Include)
			{
				flag = false;
				break;
			}
		}
		for (int i = 0; i <= _mtOverheadExpGrid.Grid.VisibleRowCount - 1; i++)
		{
			int rowHandleByVisibleIndex = _mtOverheadExpGrid.Grid.GetRowHandleByVisibleIndex(i);
			MTOverheadExpSpecificationRow mTOverheadExpSpecificationRow = (MTOverheadExpSpecificationRow)_mtOverheadExpGrid.Grid.GetRow(rowHandleByVisibleIndex);
			if (mTOverheadExpSpecificationRow != null)
			{
				if (flag)
				{
					mTOverheadExpSpecificationRow.Include = false;
				}
				else
				{
					mTOverheadExpSpecificationRow.Include = true;
				}
			}
		}
		_mtOverheadExpGrid.Grid.RefreshData();
	}

	private void AllowMTListEdit_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		if (_allowMTListEdit.IsChecked.Value)
		{
			_mtOverheadExpGrid.Grid.Columns["Include"].AllowEditing = DefaultBoolean.True;
			return;
		}
		foreach (MTOverheadExpSpecificationRow mtOverheadExps in _mtOverheadExpsList)
		{
			mtOverheadExps.Include = true;
		}
		_mtOverheadExpGrid.Grid.Columns["Include"].AllowEditing = DefaultBoolean.False;
		SummValueChanged();
	}

	protected override void SetReportBarButtonsEnable(bool isEnabled)
	{
		base.SetReportBarButtonsEnable(isEnabled);
		_barButtonJumpTo.IsEnabled = isEnabled;
		_barButtonGoodsOperationsJournal.IsEnabled = isEnabled;
		_barButtonPartnerOperationJournal.IsEnabled = isEnabled;
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

	protected override void CreateChildDocument(short childDocType, MTOverheadExp baseDoc)
	{
		base.CreateChildDocument(childDocType, baseDoc);
		switch (childDocType)
		{
		case 1:
		case 3:
		case 5:
			CreateBasedOnDocHelper.CreateDocument((DocumentTypeSE)(SmartEnum<DocumentTypeSE, short>)childDocType, PO.ISN, DVPID);
			break;
		case 2:
		case 4:
			break;
		}
	}

	protected override void StoreSettings(RegistryKey rk)
	{
		base.StoreSettings(rk);
		_mtOverheadExpGrid.StoreSettings(rk);
	}

	protected override void RestoreSettings(double DefaultWidth, double DefaultHeight, RegistryKey rk)
	{
		base.RestoreSettings(DefaultWidth, DefaultHeight, rk);
		_mtOverheadExpGrid.RestoreSettings(rk);
	}

	protected override Control ControlByName(string name)
	{
		switch (name)
		{
		case "Date":
			return _date;
		case "DocNum":
			return _docNum;
		case "BaseDocument":
			return _baseDocumentControl;
		case "SummVAT":
			return _vatSumma;
		case "ExchangeBase":
		case "ExchangeRate":
			return _currExChngRate;
		case "Summ":
			return _totalSumma;
		case "TotalSumma":
			return _totalSumma;
		case "PartnerCode":
			return _supplierContractContainer.PartnerCode;
		case "Contract":
			return _supplierContractContainer.Contract;
		case "SummaPart":
			return _supplierContractContainer.SummaPart;
		case "MTList":
			return _mtOverheadExpGrid;
		default:
			return null;
		}
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

	private void CreateBasedOnDoc(bool ForDraftDoc = false)
	{
		if (!ForDraftDoc)
		{
			_date.EditValue = BaseDocument.DocDate;
			_comment.EditValue = BaseDocument.Comment;
			_currCode.Code = BaseDocument.CurrCode;
			_currExChngRate.Rate = BaseDocument.ExchangeRate.Rate;
			_currExChngRate.Base = BaseDocument.ExchangeRate.Base;
			_summa.EditValue = 0m;
			_vatSumma.EditValue = 0m;
			_totalSumma.EditValue = 0m;
			_supplierContractContainer.PartnerCode.Code = BaseDocument.PartnerContractGroup.PartnerCode;
			_supplierContractContainer.Contract.Code = BaseDocument.PartnerContractGroup.ContractCode;
			if (BaseDocument.POType == 7)
			{
				_vatType.Code = ((MTIncomeBase)BaseDocument).VATType;
			}
			else if (BaseDocument.POType == 20)
			{
				_vatType.Code = ((MTIncomeRetail)BaseDocument).VATType;
			}
		}
		_mtOverheadExpsList.Clear();
		switch (BaseDocument.POType)
		{
		case 7:
		case 20:
		{
			MTIncomeBase mTIncomeBase = (MTIncomeBase)BaseDocument;
			decimal rate2 = BaseDocument.ExchangeRate.Rate;
			decimal exchangeRate2 = BaseDocument.ExchangeRate.Base;
			foreach (MTIncomeBase.MTIncomeSpecificationRow mT in mTIncomeBase.MTList)
			{
				MTOverheadExpSpecificationRow mTOverheadExpSpecificationRow2 = new MTOverheadExpSpecificationRow(this);
				mTOverheadExpSpecificationRow2.MTCode = mT.MTCode;
				mTOverheadExpSpecificationRow2.MTID = mT.MTID;
				mTOverheadExpSpecificationRow2.MTName = mT.MTName;
				mTOverheadExpSpecificationRow2.UnitCode = mT.UnitCode;
				mTOverheadExpSpecificationRow2.UnitBrief = mT.UnitBrief;
				mTOverheadExpSpecificationRow2.Quantity = mT.Quantity;
				mTOverheadExpSpecificationRow2.Summa = ArmSoft.SME.Trade.Common.Helpers.PriceHelper.GetSumNNC(mT.Summa, new ExchangeRate(rate2, exchangeRate2));
				mTOverheadExpSpecificationRow2.DistributedSumma = 0m;
				MaterialCacheRow item2 = TradeSettings.TradeCacheService.Material.GetItem(mT.MTCode);
				if (item2 != null)
				{
					mTOverheadExpSpecificationRow2.Coeff = item2.Coeff;
				}
				mTOverheadExpSpecificationRow2.RowID = mT.RowID;
				mTOverheadExpSpecificationRow2.Include = true;
				_mtOverheadExpsList.Add(mTOverheadExpSpecificationRow2);
			}
			PO.OverheadExpDate = BaseDocument.DocDate;
			break;
		}
		case 19:
		{
			MTIncomeImport mTIncomeImport = (MTIncomeImport)BaseDocument;
			decimal rate = BaseDocument.ExchangeRate.Rate;
			decimal exchangeRate = BaseDocument.ExchangeRate.Base;
			foreach (MTIncomeImport.MTIncomeImportSpecificationRow mT2 in mTIncomeImport.MTList)
			{
				MTOverheadExpSpecificationRow mTOverheadExpSpecificationRow = new MTOverheadExpSpecificationRow(this);
				mTOverheadExpSpecificationRow.MTCode = mT2.MTCode;
				mTOverheadExpSpecificationRow.MTID = mT2.MTID;
				mTOverheadExpSpecificationRow.MTName = mT2.MTName;
				mTOverheadExpSpecificationRow.UnitCode = mT2.UnitCode;
				mTOverheadExpSpecificationRow.UnitBrief = mT2.UnitBrief;
				mTOverheadExpSpecificationRow.Quantity = mT2.Quantity;
				mTOverheadExpSpecificationRow.Summa = ArmSoft.SME.Trade.Common.Helpers.PriceHelper.GetSumNNC(mT2.Summa, new ExchangeRate(rate, exchangeRate));
				mTOverheadExpSpecificationRow.DistributedSumma = 0m;
				MaterialCacheRow item = TradeSettings.TradeCacheService.Material.GetItem(mT2.MTCode);
				if (item != null)
				{
					mTOverheadExpSpecificationRow.Coeff = item.Coeff;
				}
				mTOverheadExpSpecificationRow.RowID = mT2.RowID;
				mTOverheadExpSpecificationRow.Include = true;
				_mtOverheadExpsList.Add(mTOverheadExpSpecificationRow);
			}
			PO.OverheadExpDate = BaseDocument.DocDate;
			break;
		}
		}
		PO.Storage = BaseDocument.Storage;
		if (!ForDraftDoc && _summa.Value != 0m)
		{
			SummValueChanged();
		}
	}

	protected override bool AllowPrint()
	{
		return false;
	}

	protected override bool AllowPrintOnNew()
	{
		return false;
	}

	private void GridViewCols_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		GridViewCols_EditValueChanged();
	}

	private void GridViewCols_EditValueChanged()
	{
		if (!string.IsNullOrWhiteSpace(_gridViewCols.Code))
		{
			switch (Convert.ToInt16(_gridViewCols.Code))
			{
			case 1:
				_distrSummaCol.Visible = true;
				_distrSummaWithVatCol.Visible = false;
				_distrCurSummaWithVatCol.Visible = false;
				break;
			case 2:
				_distrSummaCol.Visible = false;
				_distrSummaWithVatCol.Visible = true;
				_distrCurSummaWithVatCol.Visible = false;
				break;
			case 3:
				_distrSummaCol.Visible = false;
				_distrSummaWithVatCol.Visible = false;
				_distrCurSummaWithVatCol.Visible = true;
				break;
			case 4:
				_distrSummaCol.Visible = true;
				_distrSummaWithVatCol.Visible = true;
				_distrCurSummaWithVatCol.Visible = true;
				break;
			}
		}
		int num = 0;
		foreach (GridColumn column in _mtOverheadExpGrid.Grid.Columns)
		{
			if (column.Visible)
			{
				column.VisibleIndex = num;
				num++;
			}
		}
	}

	private void DistrType_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		DistrType_EditValueChanged();
	}

	private void DistrType_EditValueChanged()
	{
		if (_distrType.Code == DistrTypeSE.Manually)
		{
			_allowMTListEdit.IsChecked = true;
			_allowMTListEdit.IsReadOnly = true;
			_distrSumBy.IsRequired = false;
			_totalSumma.IsRequired = false;
			_summa.IsRequired = false;
			_summa.DoValidate();
			_distrSummaCol.AllowEditing = DefaultBoolean.True;
			_distrSummaWithVatCol.AllowEditing = DefaultBoolean.True;
			_distrCurSummaWithVatCol.AllowEditing = DefaultBoolean.True;
		}
		else
		{
			_allowMTListEdit.IsReadOnly = false;
			_distrSumBy.IsRequired = true;
			_totalSumma.IsRequired = false;
			_summa.IsRequired = true;
			_distrSummaCol.AllowEditing = DefaultBoolean.False;
			_distrSummaWithVatCol.AllowEditing = DefaultBoolean.False;
			_distrCurSummaWithVatCol.AllowEditing = DefaultBoolean.False;
		}
	}
}
