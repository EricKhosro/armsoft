using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using ArmSoft.SME.Client.Models.Directories;
using ArmSoft.SME.Client.Models.b2b;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Enums;
using ArmSoft.SME.Common.Extensions;
using ArmSoft.SME.Common.Models;
using ArmSoft.SME.Common.Models.Caches;
using ArmSoft.SME.Common.Models.Directories;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Core.UI.Constants;
using ArmSoft.SME.Core.UI.Controls;
using ArmSoft.SME.Core.UI.DPR;
using ArmSoft.SME.Core.UI.DataViews;
using ArmSoft.SME.Core.UI.Directories;
using ArmSoft.SME.Core.UI.Documents;
using ArmSoft.SME.Core.UI.DropDowns;
using ArmSoft.SME.Core.UI.Enums;
using ArmSoft.SME.Core.UI.Extensions;
using ArmSoft.SME.Core.UI.Helpers;
using ArmSoft.SME.Core.UI.PersistableObject;
using ArmSoft.SME.Trade.Common;
using ArmSoft.SME.Trade.Common.Directories.Definition;
using ArmSoft.SME.Trade.Common.Models;
using ArmSoft.SME.Trade.Common.Models.Caches;
using ArmSoft.SME.Trade.Common.Models.DPR;
using ArmSoft.SME.Trade.Common.Models.Directories;
using ArmSoft.SME.Trade.Common.Models.PrintableElements;
using ArmSoft.SME.Trade.Common.Parameters;
using ArmSoft.SME.Trade.Common.SmartEnums;
using ArmSoft.SME.Trade.UI.CustomUIElements;
using ArmSoft.SME.Trade.UI.DataView;
using ArmSoft.SME.Trade.UI.Documents;
using ArmSoft.SME.Trade.UI.DropDown;
using ArmSoft.SME.Trade.UI.Helpers;
using ArmSoft.SME.Trade.UI.OtherWindows;
using ArmSoft.SME.Trade.UI.ReportViewers.Goods;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.ComponentModel.__Internals;
using DevExpress.Utils;
using DevExpress.Xpf.Bars;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Editors;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.LayoutControl;
using DevExpress.Xpf.Ribbon;
using DevExpress.XtraEditors.DXErrorProvider;

namespace ArmSoft.SME.Trade.UI.Directory;

[DirectoryDocumentWindow(1)]
public class GoodDocumentSimpleWindow : DirectoryDocumentWindow<Good>
{
	public class GoodBarCode(IPOWindowInfo docWindow) : DocumentGridRow(docWindow)
	{
		[ObservableProperty]
		private string unitCode = "";

		[ObservableProperty]
		private string brief = "";

		[ObservableProperty]
		private string barCode = "";

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
		public string Brief
		{
			get
			{
				return brief;
			}
			[MemberNotNull("brief")]
			set
			{
				if (!EqualityComparer<string>.Default.Equals(brief, value))
				{
					OnPropertyChanging(__KnownINotifyPropertyChangingArgs.Brief);
					brief = value;
					OnPropertyChanged(__KnownINotifyPropertyChangedArgs.Brief);
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
					OnBarCodeChanged(value);
					OnPropertyChanged(__KnownINotifyPropertyChangedArgs.BarCode);
				}
			}
		}

		static GoodBarCode()
		{
			DocumentGridRow.RegisterPropertyValidators(typeof(GoodBarCode), cacheValidationResults: false, new Dictionary<string, Action<DocumentGridRow, DevExpress.XtraEditors.DXErrorProvider.ErrorInfo>>
			{
				{
					"UnitCode",
					(DocumentGridRow row, DevExpress.XtraEditors.DXErrorProvider.ErrorInfo info) =>
					{
						ValidateQntUnitCode((GoodBarCode)row, info);
					}
				},
				{
					"BarCode",
					(DocumentGridRow row, DevExpress.XtraEditors.DXErrorProvider.ErrorInfo info) =>
					{
						ValidateBarCode((GoodBarCode)row, info);
					}
				}
			});
		}

		private static void ValidateQntUnitCode(GoodBarCode row, DevExpress.XtraEditors.DXErrorProvider.ErrorInfo info)
		{
			TradePropertyValidator.ValidateGoodUnitCode(row.UnitCode, row.DocWindow.GetControlValue("Code").ToString(), info, TradeAllDirectoryDefinitions.Good.Grids["GoodBarCodes"].Columns["BarCode"]);
		}

		private static void ValidateBarCode(GoodBarCode row, DevExpress.XtraEditors.DXErrorProvider.ErrorInfo info)
		{
			PropertyValidator.EmptyStringValidate(row.BarCode, info, ArmSoft.SME.Common.Resources.BarCode);
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		private void OnUnitCodeChanged(string value)
		{
			ResetForceValidationBit("UnitCode");
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		private void OnBarCodeChanged(string value)
		{
			ResetForceValidationBit("BarCode");
		}
	}

	private class PricesGridRow : DocumentGridRow
	{
		[ObservableProperty]
		public decimal unitPrice;

		[ObservableProperty]
		public decimal altUnitPrice;

		public string Code { get; set; }

		public string Caption { get; set; }

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public decimal UnitPrice
		{
			get
			{
				return unitPrice;
			}
			set
			{
				if (!EqualityComparer<decimal>.Default.Equals(unitPrice, value))
				{
					OnPropertyChanging(__KnownINotifyPropertyChangingArgs.UnitPrice);
					unitPrice = value;
					OnPropertyChanged(__KnownINotifyPropertyChangedArgs.UnitPrice);
				}
			}
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public decimal AltUnitPrice
		{
			get
			{
				return altUnitPrice;
			}
			set
			{
				if (!EqualityComparer<decimal>.Default.Equals(altUnitPrice, value))
				{
					OnPropertyChanging(__KnownINotifyPropertyChangingArgs.AltUnitPrice);
					altUnitPrice = value;
					OnPropertyChanged(__KnownINotifyPropertyChangedArgs.AltUnitPrice);
				}
			}
		}
	}

	private ButtonEditExt _name;

	private ButtonEditExt _fullCaption;

	private DXImage _img;

	private ImageEdit _myImageEdit;

	private DropDownControl _type;

	private DropDownControl _group;

	private DropDownControl _mainSupplier;

	private DropDownControl _cpaCode;

	private DropDownControl _unit;

	private DropDownControl _altUnit;

	private PopupCalcEditExt _altUnitCoeff;

	private TextBlock _txtBlockUnit;

	private TextBlock _txtBlockAltUnit;

	private ButtonEditExt _barCode;

	private CheckEditExt _vat;

	private CheckEditExt _checkMarking;

	private CheckEditExt _isWeight;

	private TextEditExt _description;

	private DropDownControl _labelUnit;

	private PopupCalcEditExt _labelCoeff;

	private TextBlock _txtBlockLabelPrice;

	private PopupCalcEditExt _bonus;

	private PopupCalcEditExt _discount;

	private PopupCalcEditExt _pluCode;

	private DocumentGridControl _barCodesGrid;

	private TableView _barCodesTableView;

	private ObservableCollection<GoodBarCode> _barCodesList;

	private DocumentGridColumn _barCodeQntUnitCodeColumn;

	private DocumentGridColumn _barCodeColumn;

	private BarEditItemExt _newBarCode;

	private ButtonInfo _b2bCaptionButton;

	private ButtonInfo _b2bFullCaptionButton;

	private ButtonInfo _b2bBarCodeButton;

	private ButtonInfo _b2bCompareButton;

	private Button _fillCurrentPrices;

	private BindingList<PricesGridRow> _prices;

	private DocumentGridControl _pricesGrid;

	private Dictionary<string, MTPrice> _mtPricesByTypesAndBaseUnit;

	private Dictionary<string, MTPrice> _mtPricesByTypesAndAltUnit;

	private IReadOnlyDictionary<string, PriceTypeCacheRow> _mtPriceType;

	private BarSubItem _barSubItemReports = new BarSubItem();

	private BarSubItem _barSubItemActions = new BarSubItem();

	private BarSubItem _barSubItemSubDirectories = new BarSubItem();

	private BarSubItem _barSubItemReportsWhole = new BarSubItem();

	private BarSubItem _barSubItemReportsRetail = new BarSubItem();

	private ReportBarButtonItem _barButtonSalesAnalysis;

	private BarButtonItem _barButtonTAccount;

	private ReportBarButtonItem _barButtonMatRepAvail;

	private ReportBarButtonItem _barButtonMatRepTurn;

	private ReportBarButtonItem _barButtonMatRepJournal;

	private ReportBarButtonItem _barButtonMTRepRetailAvail;

	private ReportBarButtonItem _barButtonMTRepRetailTurn;

	private ReportBarButtonItem _barButtonMTRepRetailOpJournal;

	private ReportBarButtonItem _barButtonJumpTo;

	private ReportBarButtonItem _barButtonPrices;

	private BarButtonItem _barButtonPricesFormation;

	private ReportBarButtonItem _barButtonMTQntUnits;

	private ReportBarButtonItem _barButtonMTBarCodes;

	private ReportBarButtonItem _barButtonComplectPart;

	private BarButtonItem _barButtonPrintLabel;

	private ReportBarButtonItem _barButtonPartnersMT;

	private ReportBarButtonItem _barButtonSegments;

	private ReportBarButtonItem _barButtonOrderMinMax;

	private RibbonPageGroup _ribbonPageGroupSubDirectories;

	private bool _setNextBarcode;

	private readonly bool _useMarking = Settings.ParametersService.UseMarking();

	private LayoutGroup _lgGeneral;

	protected override double WidthValue => 800.0;

	protected override double HeightValue => 600.0;

	protected override bool WriteInHistory => true;

	public GoodDocumentSimpleWindow()
	{
	}

	public GoodDocumentSimpleWindow(Good doc)
		: base(doc)
	{
	}

	protected override void ConstructUI()
	{
		//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_023e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0244: Unknown result type (might be due to invalid IL or missing references)
		//IL_076a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0770: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ca3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ca9: Unknown result type (might be due to invalid IL or missing references)
		base.ConstructUI();
		LayoutGroupExt container = AddLayoutGroup(null, LayoutGroupView.Group, Orientation.Horizontal);
		LayoutGroupExt layoutGroupExt = AddLayoutGroup(container, LayoutGroupView.Group, Orientation.Vertical);
		layoutGroupExt.HorizontalAlignment = HorizontalAlignment.Stretch;
		LayoutGroupExt container2 = AddLayoutGroup(container, LayoutGroupView.Group, Orientation.Horizontal);
		_myImageEdit = AddImageControl(container2, 120.0, 120.0, isTabStop: false, showBorder: true, showMenu: true, ShowMenuMode.Hover, ShowLoadDialogOnClickMode.Always, Stretch.Uniform, VerticalAlignment.Top, HorizontalAlignment.Left, "Image");
		_myImageEdit.EditValueChanged += new SafeDocumentEventHandler<EditValueChangedEventArgs>(ImageEditValueCahnged, this).Handler;
		_code = AddButtonEditControl(layoutGroupExt, Definition.Fields["Code"], 1);
		_code.AllowMagicButton = true;
		_code.MagicToolTip = ArmSoft.SME.Common.Resources.CodeNextNumbers;
		ImageSource image = ImageService.GetImage("cloud_14_gray.svg");
		_img = new DXImage
		{
			Source = image,
			Width = 14.0,
			Height = 14.0,
			Margin = new Thickness(-2.0, 0.0, -2.0, 0.0)
		};
		_b2bCaptionButton = new ButtonInfo
		{
			GlyphKind = GlyphKind.User,
			Content = _img,
			ToolTip = ArmSoft.SME.Common.Resources.FillProductInfoFromb2bByName + " (Ctrl+Shift+M)"
		};
		_name = AddButtonEditControl(layoutGroupExt, Definition.Fields["Name"], 1, HorizontalAlignment.Stretch);
		_name.EditValueChanged += new SafeEventHandler<EditValueChangedEventArgs>(Caption_EditValueChanged).Handler;
		_name.PreviewKeyDown += new SafeDocumentEventHandler<KeyEventArgs>(Caption_PreviewKeyDown, this).Handler;
		_b2bCaptionButton.Click += new SafeDocumentEventHandler<RoutedEventArgs>(B2BCaptionButton_Click, this).Handler;
		_name.Buttons.Add(_b2bCaptionButton);
		image = ImageService.GetImage("cloud_14_gray.svg");
		_img = new DXImage
		{
			Source = image,
			Width = 14.0,
			Height = 14.0,
			Margin = new Thickness(-2.0, 0.0, -2.0, 0.0)
		};
		_fullCaption = AddButtonEditControl(layoutGroupExt, Definition.Fields["FullName"], 1, HorizontalAlignment.Stretch);
		_b2bFullCaptionButton = new ButtonInfo
		{
			GlyphKind = GlyphKind.User,
			Content = _img,
			ToolTip = ArmSoft.SME.Common.Resources.FillProductInfoFromb2bByName + " (Ctrl+Shift+M)"
		};
		_fullCaption.Buttons.Add(_b2bFullCaptionButton);
		_b2bFullCaptionButton.Click += new SafeEventHandler<RoutedEventArgs>(B2BFullCaptionButton_Click).Handler;
		_fullCaption.PreviewKeyDown += new SafeDocumentEventHandler<KeyEventArgs>(B2BFullCaptionButton_PreviewKeyDown, this).Handler;
		_cpaCode = AddDropDownControl(layoutGroupExt, Definition.Fields["CPACode"], new ArmSoft.SME.Core.UI.DropDowns.CPACodes(), allowAdditionButtons: true);
		_type = AddSmartEnumDropDownControl(layoutGroupExt, Definition.Fields["Type"], allowAdditionButtons: false, fieldWithCaption: true, HorizontalAlignment.Stretch);
		_type.EditValueChanged += new SafeEventHandler<EditValueChangedEventArgs>(Type_EditValueChanged).Handler;
		DXTabControl tc = AddTabControl(null);
		_lgGeneral = AddTabbedGroup(tc, ArmSoft.SME.Common.Resources.General, "General");
		_group = AddDropDownControl(_lgGeneral, Definition.Fields["Group"], new GoodsGroups(), allowAdditionButtons: true, fieldWithCaption: true, HorizontalAlignment.Stretch);
		LayoutGroupExt layoutGroupExt2 = AddLayoutGroup(_lgGeneral, LayoutGroupView.Group, Orientation.Horizontal);
		layoutGroupExt2.Height = 22.0;
		_unit = AddDropDownControl(layoutGroupExt2, Definition.Fields["Unit"], new ArmSoft.SME.Trade.UI.DropDown.QntUnits(), allowAdditionButtons: false, fieldWithCaption: true, HorizontalAlignment.Stretch);
		_unit.EditValueChanged += new SafeEventHandler<EditValueChangedEventArgs>(Unit_EditValueChanged).Handler;
		LayoutGroupExt layoutGroupExt3 = AddLayoutGroup(_lgGeneral, LayoutGroupView.Group, Orientation.Horizontal);
		layoutGroupExt3.Height = 22.0;
		_altUnit = AddDropDownControl(layoutGroupExt3, Definition.Fields["AltUnit"], new ArmSoft.SME.Trade.UI.DropDown.QntUnits(), allowAdditionButtons: false, fieldWithCaption: true, HorizontalAlignment.Stretch);
		_altUnit.EditValueChanged += new SafeEventHandler<EditValueChangedEventArgs>(AltUnit_EditValueChanged).Handler;
		_altUnitCoeff = AddNumericControl(_lgGeneral, ArmSoft.SME.Trade.Common.Resources.AltUnitCoeff, isRequired: true, 19, 10);
		_altUnitCoeff.EditValueChanged += new SafeEventHandler<EditValueChangedEventArgs>(AltUnitCoeff_EditValueChanged).Handler;
		_altUnitCoeff.IsReadOnly = true;
		_txtBlockUnit = new TextBlock
		{
			Text = "",
			Foreground = ColorsAS.Foreground.BlueBrush,
			TextWrapping = TextWrapping.NoWrap,
			FontWeight = FontWeights.Thin,
			Margin = new Thickness(0.0, 0.0, 0.0, 0.0),
			TextDecorations = TextDecorations.Underline
		};
		_txtBlockUnit.MouseLeftButtonDown += new SafeEventHandler<MouseButtonEventArgs>(OpenAltUnitCoeffDialog).Handler;
		_txtBlockAltUnit = new TextBlock
		{
			Text = "",
			Foreground = ColorsAS.Foreground.BlueBrush,
			TextWrapping = TextWrapping.NoWrap,
			FontWeight = FontWeights.Thin,
			Margin = new Thickness(0.0, 0.0, 0.0, 0.0)
		};
		layoutGroupExt2.Children.Add(_txtBlockUnit);
		layoutGroupExt3.Children.Add(_txtBlockAltUnit);
		_barCode = AddButtonEditControl(_lgGeneral, Definition.Fields["BarCode"], 1, HorizontalAlignment.Stretch);
		_barCode.AllowMagicButton = true;
		_barCode.MagicToolTip = ArmSoft.SME.Trade.Common.Resources.NewBarcode;
		_barCode.MagicClick += new SafeEventHandler<MagicClickEventArgs>(BarCode_MagicClick).Handler;
		_barCode.PreviewKeyDown += new SafeDocumentEventHandler<KeyEventArgs>(B2BBarCode_PreviewKeyDown, this).Handler;
		image = ImageService.GetImage("cloud_14_gray.svg");
		_img = new DXImage
		{
			Source = image,
			Width = 14.0,
			Height = 14.0,
			Margin = new Thickness(-2.0, 0.0, -2.0, 0.0)
		};
		_b2bBarCodeButton = AddButtonInfoToButtonEdit(_barCode, GlyphKind.User, ArmSoft.SME.Common.Resources.FillProductInfoFromb2bByBarCode + " (Ctrl+Shift+M)", _img);
		_b2bBarCodeButton.Click += new SafeEventHandler<RoutedEventArgs>(B2BBarCodeButton_Click).Handler;
		_b2bCompareButton = AddButtonInfoToButtonEdit(_barCode, GlyphKind.Search, ArmSoft.SME.Common.Resources.ViewGoodFromB2B);
		_b2bCompareButton.Click += new SafeEventHandler<RoutedEventArgs>(B2BCompareButton_Click).Handler;
		_vat = AddBooleanControl(_lgGeneral, Definition.Fields["VAT"]);
		if (_useMarking)
		{
			_checkMarking = AddBooleanControl(_lgGeneral, Definition.Fields["CheckMarking"]);
		}
		_isWeight = AddBooleanControl(_lgGeneral, Definition.Fields["IsWeight"]);
		if (Settings.ParametersService.UseBonus())
		{
			AddBooleanControl(_lgGeneral, Definition.Fields["IgnoreBonus"]);
			if (Settings.ParametersService.BonusType() == BonusTypeSE.ByPercent.Value)
			{
				_bonus = AddNumericControl(_lgGeneral, Definition.Fields["Bonus"]);
			}
			else
			{
				_bonus = AddNumericControl(_lgGeneral, Definition.Fields["Point"]);
			}
		}
		_discount = AddNumericControl(_lgGeneral, Definition.Fields["Discount"]);
		_pluCode = AddNumericControl(_lgGeneral, Definition.Fields["PLUCode"], useCurrencyFormat: false);
		_pluCode.AllowNullInput = true;
		_mainSupplier = AddDropDownControl(_lgGeneral, Definition.Fields["MainSupplier"], new PartnersFilter(PartnersFilter.ShowPartnersModeType.CodeCaption, "", isFilterBySuppliers: true), allowAdditionButtons: true, fieldWithCaption: true, HorizontalAlignment.Stretch);
		_description = AddStringControl(_lgGeneral, Definition.Fields["Description"], HorizontalAlignment.Stretch);
		LayoutGroupExt layoutGroupExt4 = AddLayoutGroup(_lgGeneral, LayoutGroupView.GroupBox, Orientation.Vertical, ArmSoft.SME.Trade.Common.Resources.LabelPriceSettings);
		layoutGroupExt4.HorizontalAlignment = HorizontalAlignment.Stretch;
		layoutGroupExt4.IsCollapsible = true;
		_labelUnit = AddDropDownControl(layoutGroupExt4, Definition.Fields["LabelUnit"], new ArmSoft.SME.Trade.UI.DropDown.QntUnits(), allowAdditionButtons: false, fieldWithCaption: true, HorizontalAlignment.Stretch);
		_labelUnit.EditValueChanged += new SafeEventHandler<EditValueChangedEventArgs>(LabelPriceUnit_EditValueChanged).Handler;
		LayoutGroupExt layoutGroupExt5 = AddLayoutGroup(layoutGroupExt4, LayoutGroupView.Group, Orientation.Horizontal);
		layoutGroupExt5.Height = 22.0;
		_labelCoeff = AddNumericControl(layoutGroupExt5, Definition.Fields["LabelCoeff"]);
		_labelCoeff.EditValueChanged += new SafeEventHandler<EditValueChangedEventArgs>(LabelPriceCoeff_EditValueChanged).Handler;
		_txtBlockLabelPrice = new TextBlock
		{
			Text = "",
			Foreground = ColorsAS.Foreground.BlueBrush,
			TextWrapping = TextWrapping.NoWrap,
			FontWeight = FontWeights.Thin,
			Margin = new Thickness(10.0, 0.0, 0.0, 0.0),
			TextDecorations = TextDecorations.Underline,
			HorizontalAlignment = HorizontalAlignment.Left
		};
		_txtBlockLabelPrice.MouseLeftButtonDown += new SafeEventHandler<MouseButtonEventArgs>(OpenLabelPriceDialog).Handler;
		layoutGroupExt5.Children.Add(_txtBlockLabelPrice);
		LayoutGroupExt layoutGroupExt6 = AddLayoutGroup(layoutGroupExt4, LayoutGroupView.Group, Orientation.Horizontal);
		layoutGroupExt6.Height = 35.0;
		layoutGroupExt6.Padding = new Thickness(12.0);
		layoutGroupExt6.Children.Add(new DXImage
		{
			Width = 16.0,
			Height = 16.0,
			Source = ImageService.GetImage("information_16.svg"),
			Margin = new Thickness(0.0, -6.0, 0.0, 0.0),
			VerticalAlignment = VerticalAlignment.Center
		});
		layoutGroupExt6.Children.Add(new TextBlock
		{
			Text = string.Format(ArmSoft.SME.Trade.Common.Resources.LabelPriceLawText, "²", "³"),
			Foreground = ColorsAS.Foreground.BlueBrush,
			TextWrapping = TextWrapping.Wrap,
			FontWeight = FontWeights.Thin,
			Margin = new Thickness(0.0, -6.0, 0.0, 0.0),
			VerticalAlignment = VerticalAlignment.Center
		});
		LayoutGroup container3 = AddTabbedGroup(tc, ArmSoft.SME.Common.Resources.BarCodes, "BarCodes");
		_barCodesGrid = AddGrid(container3, Definition.Grids["GoodBarCodes"], HorizontalAlignment.Stretch, VerticalAlignment.Stretch);
		_barCodesTableView = _barCodesGrid.View;
		_newBarCode = _barCodesGrid.AddButton("", ArmSoft.SME.Trade.Common.Resources.NewBarcodeShort, ArmSoft.SME.Trade.Common.Resources.NewBarcode, 110.0);
		_barCodeQntUnitCodeColumn = _barCodesGrid.AddDropDownColumn(Definition.Grids["GoodBarCodes"].Columns["UnitCode"], new ArmSoft.SME.Trade.UI.DropDown.MTQntUnits(PO.Code), allowAdditionButtons: false, isOwnProcessingClickDropDown: true);
		_barCodeQntUnitCodeColumn.Width = 150.0;
		DocumentGridColumn documentGridColumn = _barCodesGrid.AddStringColumn(Definition.Grids["GoodBarCodes"].Columns["Brief"]);
		documentGridColumn.Width = 100.0;
		documentGridColumn.AllowEditing = DefaultBoolean.False;
		_barCodeColumn = _barCodesGrid.AddDropDownColumn(Definition.Grids["GoodBarCodes"].Columns["BarCode"], trimValue: true, GlyphKind.Apply);
		_barCodeColumn.Width = 150.0;
		_barCodesList = new ObservableCollection<GoodBarCode>();
		_barCodesTableView.CellValueChanging += new SafeEventHandler<CellValueChangedEventArgs>(BarCodesTableView_CellValueChanging).Handler;
		_barCodesGrid.ClickDropDown += new SafeEventHandler<DocumentGridClickDropDownEventArgs>(BarCodesGrid_ClickDropDown).Handler;
		_barCodesGrid.AddingRow += new SafeEventHandler<DocumentGridAddingRowEventArgs>(BarCodesGrid_AddingRow).Handler;
		_barCodesGrid.DuplicatingRow += new SafeEventHandler<DocumentGridDuplicatingRowEventArgs>(BarCodesGrid_DuplicatingRow).Handler;
		_newBarCode.Click += new SafeEventHandler<RoutedEventArgs>(NewBarCode_Click).Handler;
		LayoutGroup container4 = AddTabbedGroup(tc, ArmSoft.SME.Common.Resources.Prices, "Prices");
		LayoutGroupExt container5 = AddLayoutGroup(container4, LayoutGroupView.Group, Orientation.Horizontal);
		_fillCurrentPrices = AddButton(container5, "", ArmSoft.SME.Trade.Common.Resources.FillCurrentPrices, "", ArmSoft.SME.Trade.Common.Resources.FillCurrentPrices, isTabStop: true);
		_fillCurrentPrices.Click += new SafeEventHandler<RoutedEventArgs>(FillCurrentPrices_Click).Handler;
		_pricesGrid = AddGrid(container4, "", HorizontalAlignment.Stretch, VerticalAlignment.Stretch, isSupportDialogMode: false, "PriceGrid");
		_pricesGrid.AddStringColumn("Caption", ArmSoft.SME.Trade.Common.Resources.PriceType, 50).AllowEditing = DefaultBoolean.False;
		_pricesGrid.AddNumberColumn("UnitPrice", ArmSoft.SME.Trade.Common.Resources.NewPrices, 16, 2).AllowEditing = DefaultBoolean.True;
		bool flag = _viewMode == WindowViewMode.View || !Settings.UserAccessService.GetProviderPermission(DataProviderTypeSE.MTPrices).AllowOperation;
		_pricesGrid.AddNumberColumn("AltUnitPrice", ArmSoft.SME.Trade.Common.Resources.NewPrices, 16, 2).AllowEditing = (flag ? DefaultBoolean.False : DefaultBoolean.False);
		_pricesGrid.Grid.Columns["AltUnitPrice"].Visible = false;
		_prices = new BindingList<PricesGridRow>();
		_pricesGrid.ItemsSource = _prices;
		_pricesGrid.ShowButtons = false;
		_mtPriceType = (from p in TradeSettings.TradeCacheService.PriceType.GetList()
			orderby p.Key
			select p).Take(10).ToDictionary((PriceTypeCacheRow p) => p.Key, (PriceTypeCacheRow p) => p);
		foreach (PriceTypeCacheRow value in _mtPriceType.Values)
		{
			_prices.Add(new PricesGridRow
			{
				Code = value.Code,
				Caption = value.Caption + " (" + value.CurrCode + (string.IsNullOrWhiteSpace(PO.UnitBrief) ? "" : (", " + PO.UnitBrief)) + ")",
				UnitPrice = 0m,
				AltUnitPrice = 0m
			});
		}
	}

	private void JournalPrices_Click(object sender, RoutedEventArgs e)
	{
		Prices prices = new Prices();
		prices.MTCode = PO.Code;
		prices.Show(useParamValues: true, openAsChild: true, this);
	}

	protected override void AddToolbarItems()
	{
		base.AddToolbarItems();
		Ribbon.ToolbarItemLinks.Add(_barButtonJumpTo.ConvertForToolbar("t_doc_JumpTo_16.svg"));
		Ribbon.ToolbarItemLinks.Add(_barSubItemActions.ConvertForToolbar("t_action_16.svg"));
		Ribbon.ToolbarItemLinks.Add(_barSubItemReports.ConvertForToolbar("t_report_16.svg"));
		Ribbon.ToolbarItemLinks.Add(_barSubItemSubDirectories.ConvertForToolbar("t_table_16.svg"));
	}

	protected override void AddItemsToRibbon()
	{
		base.AddItemsToRibbon();
		AddActionItemsToRibbon();
		AddReportItemsToRibbon();
		AddSubDirectoriesItemsToRibbon();
	}

	private void AddActionItemsToRibbon()
	{
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0193: Unknown result type (might be due to invalid IL or missing references)
		//IL_0199: Unknown result type (might be due to invalid IL or missing references)
		//IL_01af: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b5: Unknown result type (might be due to invalid IL or missing references)
		CreateRibbonPageGroupActions();
		_barButtonJumpTo = new ReportBarButtonItem(DataProviderTypeSE.Goods, showDialog: false, ArmSoft.SME.Common.Resources.JumpToDirectory, null, "doc_JumpTo_16.svg", "doc_JumpTo_32.svg");
		_barButtonJumpTo.AddInBarSubItem(ref _ribbonPageGroupActions, ref _barSubItemActions);
		_barSubItemActions = new BarSubItem
		{
			Content = "",
			Glyph = ImageService.GetImage("action_16.svg"),
			LargeGlyph = ImageService.GetImage("action_32.svg")
		};
		_barButtonPricesFormation = new BarButtonItem
		{
			Content = ArmSoft.SME.Trade.Common.Resources.PricesFormation,
			KeyGesture = new KeyGesture((Key)53, (ModifierKeys)2),
			Glyph = ImageService.GetImage("action_16.svg"),
			LargeGlyph = ImageService.GetImage("action_32.svg")
		};
		_barManager.Items.Add(_barButtonPricesFormation);
		_barButtonPricesFormation.ItemClick += new SafeDocumentEventHandler<ItemClickEventArgs>(BarButtonPriceFormation_ItemClick, this).Handler;
		_ribbonPageGroupActions.ItemLinks.Add(_barButtonPricesFormation.CreateLink());
		BarItemLink barItemLink = _barButtonPricesFormation.CreateLink();
		barItemLink.MergeType = BarItemMergeType.Replace;
		_barSubItemActions.ItemLinks.Add(barItemLink);
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
		barItemLink = _barButtonPrintLabel.CreateLink();
		barItemLink.MergeType = BarItemMergeType.Replace;
		_barSubItemActions.ItemLinks.Add(barItemLink);
		_barManager.Items.Add(_barSubItemActions);
	}

	private void AddReportItemsToRibbon()
	{
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0169: Unknown result type (might be due to invalid IL or missing references)
		//IL_016f: Unknown result type (might be due to invalid IL or missing references)
		//IL_018a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0190: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e2: Unknown result type (might be due to invalid IL or missing references)
		CreateRibbonPageGroupReports();
		_barSubItemReports.Content = "";
		_barSubItemReports.Glyph = ImageService.GetImage("report_16.svg");
		_barSubItemReports.LargeGlyph = ImageService.GetImage("report_32.svg");
		_barButtonSalesAnalysis = new ReportBarButtonItem(DataProviderTypeSE.MatRepSalesAnalysis.Value, showDialog: true, ArmSoft.SME.Trade.Common.Resources.MTRepSalesAnalysis);
		_barButtonSalesAnalysis.AddInBarSubItem(ref _ribbonPageGroupReports, ref _barSubItemReports);
		_barButtonTAccount = new BarButtonItem
		{
			Content = ArmSoft.SME.Trade.Common.Resources.TAccount,
			Glyph = ImageService.GetImage("report_16.svg"),
			LargeGlyph = ImageService.GetImage("report_32.svg")
		};
		_barButtonTAccount.ItemClick += new SafeDocumentEventHandler<ItemClickEventArgs>(BarButtonTAccount_ItemClick, this).Handler;
		_ribbonPageGroupReports.ItemLinks.Add(_barButtonTAccount.CreateLink());
		BarItemLink barItemLink = _barButtonTAccount.CreateLink();
		barItemLink.MergeType = BarItemMergeType.Replace;
		_barSubItemReports.ItemLinks.Add(barItemLink);
		_barSubItemReportsRetail.Content = ArmSoft.SME.Trade.Common.Resources.RetailWord;
		_barSubItemReportsRetail.Glyph = ImageService.GetImage("report_16.svg");
		_barSubItemReportsRetail.LargeGlyph = ImageService.GetImage("report_32.svg");
		_barSubItemReportsWhole.Content = ArmSoft.SME.Trade.Common.Resources.WholeSaleWord;
		_barSubItemReportsWhole.Glyph = ImageService.GetImage("report_16.svg");
		_barSubItemReportsWhole.LargeGlyph = ImageService.GetImage("report_32.svg");
		_ribbonPageGroupReports.ItemLinks.Add(_barSubItemReportsWhole);
		_ribbonPageGroupReports.ItemLinks.Add(_barSubItemReportsRetail);
		_barButtonMatRepAvail = new ReportBarButtonItem(DataProviderTypeSE.MatRepAvailability, showDialog: true, ArmSoft.SME.Trade.Common.Resources.MatRepAvailShort);
		_barButtonMatRepAvail.AddInBarSubItem(ref _barSubItemReports);
		_barButtonMatRepAvail.AddInBarSubItem(ref _barSubItemReportsWhole);
		_barButtonMatRepTurn = new ReportBarButtonItem(DataProviderTypeSE.GoodsTurnOver, showDialog: true, ArmSoft.SME.Trade.Common.Resources.MatRepTurnShort);
		_barButtonMatRepTurn.AddInBarSubItem(ref _barSubItemReports);
		_barButtonMatRepTurn.AddInBarSubItem(ref _barSubItemReportsWhole);
		_barButtonMatRepJournal = new ReportBarButtonItem(DataProviderTypeSE.GoodsRepOperations.Value, showDialog: true, ArmSoft.SME.Trade.Common.Resources.MatRepJournalShort);
		_barButtonMatRepJournal.AddInBarSubItem(ref _barSubItemReports);
		_barButtonMatRepJournal.AddInBarSubItem(ref _barSubItemReportsWhole);
		short dataProviderType = DataProviderTypeSE.MatRepAvailabilityRetail;
		string matRepAvailShort = ArmSoft.SME.Trade.Common.Resources.MatRepAvailShort;
		int num = 1;
		List<PropertyWithValue> list = new List<PropertyWithValue>(num);
		CollectionsMarshal.SetCount(list, num);
		CollectionsMarshal.AsSpan(list)[0] = new PropertyWithValue("Good", PO.Code.ToString());
		_barButtonMTRepRetailAvail = new ReportBarButtonItem(dataProviderType, showDialog: true, matRepAvailShort, list);
		_barButtonMTRepRetailAvail.AddInBarSubItem(ref _barSubItemReports);
		_barButtonMTRepRetailAvail.AddInBarSubItem(ref _barSubItemReportsRetail);
		_barButtonMTRepRetailTurn = new ReportBarButtonItem(DataProviderTypeSE.MatRepTurnOverRetail, showDialog: true, ArmSoft.SME.Trade.Common.Resources.MatRepTurnShort);
		_barButtonMTRepRetailTurn.AddInBarSubItem(ref _barSubItemReports);
		_barButtonMTRepRetailTurn.AddInBarSubItem(ref _barSubItemReportsRetail);
		_barButtonMTRepRetailOpJournal = new ReportBarButtonItem(DataProviderTypeSE.MatRepOperationsRetail.Value, showDialog: true, ArmSoft.SME.Trade.Common.Resources.MatRepJournalShort);
		_barButtonMTRepRetailOpJournal.AddInBarSubItem(ref _barSubItemReports);
		_barButtonMTRepRetailOpJournal.AddInBarSubItem(ref _barSubItemReportsRetail);
	}

	private void AddSubDirectoriesItemsToRibbon()
	{
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		AddRibbonPageGroupSubDirectories();
		_barSubItemSubDirectories.Content = "";
		_barSubItemSubDirectories.Glyph = ImageService.GetImage("table_16.svg");
		_barSubItemSubDirectories.LargeGlyph = ImageService.GetImage("table_32.svg");
		_barButtonPrices = new ReportBarButtonItem(DataProviderTypeSE.MTPrices, showDialog: true, ArmSoft.SME.Common.Resources.Prices);
		_barButtonPrices.AddInBarSubItem(ref _ribbonPageGroupSubDirectories, ref _barSubItemSubDirectories);
		_barButtonMTQntUnits = new ReportBarButtonItem(DataProviderTypeSE.MTQntUnits, showDialog: false, ArmSoft.SME.Common.Resources.MeasureUnits);
		_barButtonMTQntUnits.AddInBarSubItem(ref _ribbonPageGroupSubDirectories, ref _barSubItemSubDirectories);
		_barButtonMTBarCodes = new ReportBarButtonItem(DataProviderTypeSE.MTBarCodes, showDialog: false, ArmSoft.SME.Common.Resources.BarCodes);
		_barButtonMTBarCodes.AddInBarSubItem(ref _ribbonPageGroupSubDirectories, ref _barSubItemSubDirectories);
		_barButtonComplectPart = new ReportBarButtonItem(DataProviderTypeSE.MTComplectsParts, showDialog: false, ArmSoft.SME.Trade.Common.Resources.MTComplectation);
		_barButtonComplectPart.AddInBarSubItem(ref _ribbonPageGroupSubDirectories, ref _barSubItemSubDirectories);
		_barButtonPartnersMT = new ReportBarButtonItem(DataProviderTypeSE.PartnersMT, showDialog: false, ArmSoft.SME.Common.Resources.Suppliers);
		_barButtonPartnersMT.AddInBarSubItem(ref _ribbonPageGroupSubDirectories, ref _barSubItemSubDirectories);
		_barButtonSegments = new ReportBarButtonItem(DataProviderTypeSE.GoodsBySegment, showDialog: false, ArmSoft.SME.Trade.Common.Resources.Segments);
		_barButtonSegments.AddInBarSubItem(ref _ribbonPageGroupSubDirectories, ref _barSubItemSubDirectories);
		_barButtonOrderMinMax = new ReportBarButtonItem(DataProviderTypeSE.OrderMinMax, showDialog: false, ArmSoft.SME.Trade.Common.Resources.MTOrderMinMax);
		_barButtonOrderMinMax.AddInBarSubItem(ref _ribbonPageGroupSubDirectories, ref _barSubItemSubDirectories);
	}

	protected override void SetReportBarButtonsProperties()
	{
		base.SetReportBarButtonsProperties();
		ReportBarButtonItem barButtonJumpTo = _barButtonJumpTo;
		int num = 2;
		List<PropertyWithValue> list = new List<PropertyWithValue>(num);
		CollectionsMarshal.SetCount(list, num);
		Span<PropertyWithValue> span = CollectionsMarshal.AsSpan(list);
		span[0] = new PropertyWithValue("MtCode", PO.Code.ToString());
		span[1] = new PropertyWithValue("Extended", false.ToString());
		barButtonJumpTo.PropertyWithValues = list;
		ReportBarButtonItem barButtonSalesAnalysis = _barButtonSalesAnalysis;
		num = 3;
		List<PropertyWithValue> list2 = new List<PropertyWithValue>(num);
		CollectionsMarshal.SetCount(list2, num);
		Span<PropertyWithValue> span2 = CollectionsMarshal.AsSpan(list2);
		span2[0] = new PropertyWithValue("Good", PO.Code.ToString());
		span2[1] = new PropertyWithValue("StartDate", Settings.HostingEnvironment.DateBegin.ToString());
		span2[2] = new PropertyWithValue("EndDate", Settings.HostingEnvironment.DateEnd.ToString());
		barButtonSalesAnalysis.PropertyWithValues = list2;
		if (PO.Type == GoodsTypesSE.Good.Value)
		{
			_barButtonTAccount.IsVisible = true;
			_barSubItemReportsRetail.IsVisible = true;
			_barSubItemReportsWhole.IsVisible = true;
			_barButtonMatRepAvail.IsVisible = true;
			_barButtonMatRepTurn.IsVisible = true;
			_barButtonMatRepJournal.IsVisible = true;
			_barButtonMTRepRetailAvail.IsVisible = true;
			_barButtonMTRepRetailTurn.IsVisible = true;
			_barButtonMTRepRetailOpJournal.IsVisible = true;
			ReportBarButtonItem barButtonMatRepAvail = _barButtonMatRepAvail;
			num = 1;
			List<PropertyWithValue> list3 = new List<PropertyWithValue>(num);
			CollectionsMarshal.SetCount(list3, num);
			CollectionsMarshal.AsSpan(list3)[0] = new PropertyWithValue("MTCode", PO.Code.ToString());
			barButtonMatRepAvail.PropertyWithValues = list3;
			ReportBarButtonItem barButtonMatRepTurn = _barButtonMatRepTurn;
			num = 1;
			List<PropertyWithValue> list4 = new List<PropertyWithValue>(num);
			CollectionsMarshal.SetCount(list4, num);
			CollectionsMarshal.AsSpan(list4)[0] = new PropertyWithValue("Good", PO.Code.ToString());
			barButtonMatRepTurn.PropertyWithValues = list4;
			ReportBarButtonItem barButtonMatRepJournal = _barButtonMatRepJournal;
			num = 3;
			List<PropertyWithValue> list5 = new List<PropertyWithValue>(num);
			CollectionsMarshal.SetCount(list5, num);
			Span<PropertyWithValue> span3 = CollectionsMarshal.AsSpan(list5);
			span3[0] = new PropertyWithValue("GoodCode", PO.Code.ToString());
			span3[1] = new PropertyWithValue("StartDate", Settings.HostingEnvironment.DateBegin.ToString());
			span3[2] = new PropertyWithValue("EndDate", Settings.HostingEnvironment.DateEnd.ToString());
			barButtonMatRepJournal.PropertyWithValues = list5;
			ReportBarButtonItem barButtonMTRepRetailAvail = _barButtonMTRepRetailAvail;
			num = 1;
			List<PropertyWithValue> list6 = new List<PropertyWithValue>(num);
			CollectionsMarshal.SetCount(list6, num);
			CollectionsMarshal.AsSpan(list6)[0] = new PropertyWithValue("Good", PO.Code.ToString());
			barButtonMTRepRetailAvail.PropertyWithValues = list6;
			ReportBarButtonItem barButtonMTRepRetailTurn = _barButtonMTRepRetailTurn;
			num = 1;
			List<PropertyWithValue> list7 = new List<PropertyWithValue>(num);
			CollectionsMarshal.SetCount(list7, num);
			CollectionsMarshal.AsSpan(list7)[0] = new PropertyWithValue("Good", PO.Code.ToString());
			barButtonMTRepRetailTurn.PropertyWithValues = list7;
			ReportBarButtonItem barButtonMTRepRetailOpJournal = _barButtonMTRepRetailOpJournal;
			num = 3;
			List<PropertyWithValue> list8 = new List<PropertyWithValue>(num);
			CollectionsMarshal.SetCount(list8, num);
			Span<PropertyWithValue> span4 = CollectionsMarshal.AsSpan(list8);
			span4[0] = new PropertyWithValue("GoodCode", PO.Code.ToString());
			span4[1] = new PropertyWithValue("StartDate", Settings.HostingEnvironment.DateBegin.ToString());
			span4[2] = new PropertyWithValue("EndDate", Settings.HostingEnvironment.DateEnd.ToString());
			barButtonMTRepRetailOpJournal.PropertyWithValues = list8;
		}
		else
		{
			_barButtonTAccount.IsVisible = false;
			_barSubItemReportsRetail.IsVisible = false;
			_barSubItemReportsWhole.IsVisible = false;
			_barButtonMatRepAvail.IsVisible = false;
			_barButtonMatRepTurn.IsVisible = false;
			_barButtonMatRepJournal.IsVisible = false;
			_barButtonMTRepRetailAvail.IsVisible = false;
			_barButtonMTRepRetailTurn.IsVisible = false;
			_barButtonMTRepRetailOpJournal.IsVisible = false;
		}
		ReportBarButtonItem barButtonPrices = _barButtonPrices;
		num = 1;
		List<PropertyWithValue> list9 = new List<PropertyWithValue>(num);
		CollectionsMarshal.SetCount(list9, num);
		CollectionsMarshal.AsSpan(list9)[0] = new PropertyWithValue("MTCode", PO.Code.ToString());
		barButtonPrices.PropertyWithValues = list9;
		ReportBarButtonItem barButtonMTQntUnits = _barButtonMTQntUnits;
		num = 1;
		List<PropertyWithValue> list10 = new List<PropertyWithValue>(num);
		CollectionsMarshal.SetCount(list10, num);
		CollectionsMarshal.AsSpan(list10)[0] = new PropertyWithValue("MTCode", PO.Code.ToString());
		barButtonMTQntUnits.PropertyWithValues = list10;
		ReportBarButtonItem barButtonMTBarCodes = _barButtonMTBarCodes;
		num = 1;
		List<PropertyWithValue> list11 = new List<PropertyWithValue>(num);
		CollectionsMarshal.SetCount(list11, num);
		CollectionsMarshal.AsSpan(list11)[0] = new PropertyWithValue("MTCode", PO.Code.ToString());
		barButtonMTBarCodes.PropertyWithValues = list11;
		ReportBarButtonItem barButtonComplectPart = _barButtonComplectPart;
		num = 1;
		List<PropertyWithValue> list12 = new List<PropertyWithValue>(num);
		CollectionsMarshal.SetCount(list12, num);
		CollectionsMarshal.AsSpan(list12)[0] = new PropertyWithValue("ComplectCode", PO.Code.ToString());
		barButtonComplectPart.PropertyWithValues = list12;
		ReportBarButtonItem barButtonPartnersMT = _barButtonPartnersMT;
		num = 1;
		List<PropertyWithValue> list13 = new List<PropertyWithValue>(num);
		CollectionsMarshal.SetCount(list13, num);
		CollectionsMarshal.AsSpan(list13)[0] = new PropertyWithValue("MaterialCode", PO.Code.ToString());
		barButtonPartnersMT.PropertyWithValues = list13;
		ReportBarButtonItem barButtonSegments = _barButtonSegments;
		num = 1;
		List<PropertyWithValue> list14 = new List<PropertyWithValue>(num);
		CollectionsMarshal.SetCount(list14, num);
		CollectionsMarshal.AsSpan(list14)[0] = new PropertyWithValue("GoodCode", PO.Code.ToString());
		barButtonSegments.PropertyWithValues = list14;
		ReportBarButtonItem barButtonOrderMinMax = _barButtonOrderMinMax;
		num = 1;
		List<PropertyWithValue> list15 = new List<PropertyWithValue>(num);
		CollectionsMarshal.SetCount(list15, num);
		CollectionsMarshal.AsSpan(list15)[0] = new PropertyWithValue("MTCode", PO.Code.ToString());
		barButtonOrderMinMax.PropertyWithValues = list15;
	}

	private void AddRibbonPageGroupSubDirectories()
	{
		if (_ribbonPageOther == null)
		{
			AddOtherRibbonPage();
		}
		if (_ribbonPageGroupSubDirectories == null)
		{
			_ribbonPageGroupSubDirectories = new RibbonPageGroup
			{
				AllowCollapse = false
			};
			_ribbonPageOther.Groups.Add(_ribbonPageGroupSubDirectories);
			_ribbonPageGroupSubDirectories.Caption = ArmSoft.SME.Trade.Common.Resources.SubDirectories;
		}
	}

	private void BarButtonPriceFormation_ItemClick(object sender, ItemClickEventArgs e)
	{
		int num = 1;
		List<PricesFormationWindow.GoodsRow> list = new List<PricesFormationWindow.GoodsRow>(num);
		CollectionsMarshal.SetCount(list, num);
		CollectionsMarshal.AsSpan(list)[0] = new PricesFormationWindow.GoodsRow
		{
			Code = PO.Code
		};
		new PricesFormationWindow(list, "PricesFormationDialog").Invoke();
	}

	protected void BarButtonPrintLabel_ItemClick(object sender, ItemClickEventArgs e)
	{
		ArmSoft.SME.Trade.Common.Models.PrintableElements.Label.GoodLabelRow goodLabelRow = InitContextFunctionsHelper.BuildLabelGoodRow(TradeSettings.TradeCacheService.Material.GetItemById(PO.Id));
		int num = 1;
		List<ArmSoft.SME.Trade.Common.Models.PrintableElements.Label.GoodLabelRow> list = new List<ArmSoft.SME.Trade.Common.Models.PrintableElements.Label.GoodLabelRow>(num);
		CollectionsMarshal.SetCount(list, num);
		CollectionsMarshal.AsSpan(list)[0] = goodLabelRow;
		new PrintLabelDialog(null, null, list).Invoke();
	}

	private void BarButtonTAccount_ItemClick(object sender, ItemClickEventArgs e)
	{
		MTAccountReportViewer mTAccountReportViewer = new MTAccountReportViewer();
		mTAccountReportViewer.GoodCode = _code.Text;
		mTAccountReportViewer.StartDate = Settings.HostingEnvironment.DateBegin;
		mTAccountReportViewer.EndDate = Settings.HostingEnvironment.DateEnd;
		mTAccountReportViewer.Show(useParamValues: true, openAsChild: false);
	}

	protected override void SetReportBarButtonsEnable(bool isEnabled)
	{
		base.SetReportBarButtonsEnable(isEnabled);
		_barButtonSalesAnalysis.IsEnabled = isEnabled;
		if (PO.Type == GoodsTypesSE.Good.Value)
		{
			_barButtonTAccount.IsEnabled = isEnabled;
			_barButtonMatRepAvail.IsEnabled = isEnabled;
			_barButtonMatRepTurn.IsEnabled = isEnabled;
			_barButtonMatRepJournal.IsEnabled = isEnabled;
			_barButtonMTRepRetailAvail.IsEnabled = isEnabled;
			_barButtonMTRepRetailTurn.IsEnabled = isEnabled;
			_barButtonMTRepRetailOpJournal.IsEnabled = isEnabled;
			_barButtonJumpTo.IsEnabled = isEnabled;
		}
		_barButtonPrices.IsEnabled = isEnabled;
		_barButtonPricesFormation.IsEnabled = isEnabled;
		_barButtonMTQntUnits.IsEnabled = isEnabled;
		_barButtonMTBarCodes.IsEnabled = isEnabled;
		_barButtonComplectPart.IsEnabled = isEnabled;
		_barButtonPrintLabel.IsEnabled = isEnabled;
		_barButtonPartnersMT.IsEnabled = isEnabled;
		_barButtonSegments.IsEnabled = isEnabled;
		_barButtonOrderMinMax.IsEnabled = isEnabled;
	}

	protected override void AfterStore(DirStoreRequest sr, bool isNewDocument)
	{
		base.AfterStore(sr, isNewDocument);
		if (_setNextBarcode)
		{
			if (Settings.ParametersService.InternalBarcodeEANFormat())
			{
				Settings.ParametersService.InternalBarcodeNext_set(ArmSoft.SME.Common.Extensions.StringExtensions.NextDocNum(Settings.ParametersService.InternalBarcodeNext(), 10));
			}
			else
			{
				Settings.ParametersService.InternalBarcodeNext_set(ArmSoft.SME.Common.Extensions.StringExtensions.NextDocNum(Settings.ParametersService.InternalBarcodeNext(), 20));
			}
			_setNextBarcode = false;
		}
		SaveNewPrices();
	}

	protected override void UpdateDataViewPanel(DataViewPanel dvp)
	{
		if (dvp.IdvName() == "Goods")
		{
			dvp.Update(PO.Id);
		}
		else
		{
			dvp.Refresh();
		}
	}

	private void FillCurrentPrices_Click(object sender, RoutedEventArgs e)
	{
		_mtPricesByTypesAndBaseUnit = TradeSettings.CreateTradeClient().CommonServices.GetMTPricesByTypesAndBaseUnit(PO.Id, _prices.Select((PricesGridRow p) => p.Code).ToList());
		_mtPricesByTypesAndAltUnit = TradeSettings.CreateTradeClient().CommonServices.GetMTPricesByTypesAndAltUnit(PO.Id, _prices.Select((PricesGridRow p) => p.Code).ToList());
		foreach (KeyValuePair<string, MTPrice> price in _mtPricesByTypesAndBaseUnit)
		{
			PricesGridRow pricesGridRow = _prices.FirstOrDefault((PricesGridRow p) => p.Code == price.Key);
			if (pricesGridRow != null)
			{
				pricesGridRow.UnitPrice = ((price.Value.Unit == _unit.Code) ? price.Value.Price : 0m);
			}
		}
		foreach (KeyValuePair<string, MTPrice> price2 in _mtPricesByTypesAndAltUnit)
		{
			PricesGridRow pricesGridRow2 = _prices.FirstOrDefault((PricesGridRow p) => p.Code == price2.Key);
			if (pricesGridRow2 != null)
			{
				pricesGridRow2.AltUnitPrice = ((price2.Value.Unit == _altUnit.Code) ? price2.Value.Price : 0m);
			}
		}
	}

	private void SaveNewPrices()
	{
		_mtPricesByTypesAndBaseUnit = TradeSettings.CreateTradeClient().CommonServices.GetMTPricesByTypesAndBaseUnit(PO.Id, _prices.Select((PricesGridRow p) => p.Code).ToList());
		_mtPricesByTypesAndAltUnit = TradeSettings.CreateTradeClient().CommonServices.GetMTPricesByTypesAndAltUnit(PO.Id, _prices.Select((PricesGridRow p) => p.Code).ToList());
		_pricesGrid.View.CommitEditing();
		List<PriceFormationRequestProperties> list = new List<PriceFormationRequestProperties>();
		foreach (PricesGridRow price in _prices)
		{
			if (price.UnitPrice != 0m && _mtPricesByTypesAndBaseUnit.TryGetValue(price.Code, out var value))
			{
				if (!value.HasPriceType)
				{
					PriceTypeCacheRow priceTypeCacheRow = _mtPriceType[price.Code];
					value.Curr = priceTypeCacheRow.CurrCode;
					value.PriceType = priceTypeCacheRow.Code;
					value.Unit = PO.Unit;
					value.Date = Settings.HostingEnvironment.CurrentDate;
				}
				list.Add(new PriceFormationRequestProperties
				{
					MTID = value.MTId,
					MTCode = PO.Code,
					PriceType = value.PriceType,
					QntUnit = value.Unit,
					PriceDate = value.Date,
					Curr = value.Curr,
					ItemPrice = price.UnitPrice,
					MTCodeOld = PO.Code,
					QntUnitOld = value.Unit,
					PriceTypeOld = value.PriceType,
					PriceDateOld = value.Date
				});
			}
			if (PO.Unit != PO.AltUnit && price.AltUnitPrice != 0m && _mtPricesByTypesAndAltUnit.TryGetValue(price.Code, out var value2))
			{
				if (!value2.HasPriceType)
				{
					PriceTypeCacheRow priceTypeCacheRow2 = _mtPriceType[price.Code];
					value2.Curr = priceTypeCacheRow2.CurrCode;
					value2.PriceType = priceTypeCacheRow2.Code;
					value2.Unit = PO.AltUnit;
					value2.Date = Settings.HostingEnvironment.CurrentDate;
				}
				list.Add(new PriceFormationRequestProperties
				{
					MTID = value2.MTId,
					MTCode = PO.Code,
					PriceType = value2.PriceType,
					QntUnit = value2.Unit,
					PriceDate = value2.Date,
					Curr = value2.Curr,
					ItemPrice = price.AltUnitPrice,
					MTCodeOld = PO.Code,
					QntUnitOld = value2.Unit,
					PriceTypeOld = value2.PriceType,
					PriceDateOld = value2.Date
				});
			}
		}
		if (list.Count == 0)
		{
			return;
		}
		PriceFormationRequestModel request = new PriceFormationRequestModel
		{
			Properties = list
		};
		DataProcessingRequestHelper.ExecuteThroughHandler<PriceFormationRequestModel, DirectoryStoreRequestModel<Price>>(DPRType.Other, "PriceFormation", request);
		foreach (PricesGridRow price2 in _prices)
		{
			if (price2.UnitPrice > 0m)
			{
				price2.UnitPrice = 0m;
				price2.AltUnitPrice = 0m;
			}
		}
		TradeSettings.TradeCacheService.CurrentPrices.Refresh();
	}

	private void Caption_PreviewKeyDown(object sender, KeyEventArgs e)
	{
		if (HotKeys.WithModifier.Control_Shift_M.Matches(this, e))
		{
			B2BCaptionButton_Click(_name);
		}
	}

	private void B2BCaptionButton_Click(object sender, RoutedEventArgs e)
	{
		B2BCaptionButton_Click(_name);
	}

	private void B2BFullCaptionButton_PreviewKeyDown(object sender, KeyEventArgs e)
	{
		if (HotKeys.WithModifier.Control_Shift_M.Matches(this, e))
		{
			B2BCaptionButton_Click(_fullCaption);
		}
	}

	private void B2BFullCaptionButton_Click(object sender, RoutedEventArgs e)
	{
		B2BCaptionButton_Click(_fullCaption);
	}

	private void B2BCaptionButton_Click(ButtonEditExt captionControl)
	{
		if (!captionControl.DoValidate() || string.IsNullOrWhiteSpace(captionControl.Text))
		{
			return;
		}
		List<b2bUnitedProductRowBase> goodFromName = BarcodeFromb2bCloudDialogWindow.GetGoodFromName(captionControl.Text);
		if (goodFromName == null)
		{
			return;
		}
		if (goodFromName.Count == 0)
		{
			throw new ASException(ArmSoft.SME.Common.Resources.ProductWithNameDoesNotExistInB2B);
		}
		if (goodFromName.Count == 1)
		{
			FillControlsValuesFromProduct(goodFromName[0]);
		}
		else
		{
			if (goodFromName.Count <= 1)
			{
				return;
			}
			ModalBrowser modalBrowser = new ModalBrowser(new b2bUnitedProducts(goodFromName))
			{
				Owner = this,
				WindowStartupLocation = WindowStartupLocation.CenterScreen
			};
			if (modalBrowser.Show())
			{
				b2bUnitedProductRowBase b2bUnitedProductRowBase2 = (b2bUnitedProductRowBase)modalBrowser.Data[modalBrowser.SelectedRowIndex];
				if (!string.IsNullOrWhiteSpace(b2bUnitedProductRowBase2.Barcode))
				{
					FillControlsValuesFromProduct(b2bUnitedProductRowBase2);
				}
			}
		}
	}

	protected override void OnNew()
	{
		base.OnNew();
		_setFocusOnActivation = _code;
	}

	protected override void OnNewAfterSave()
	{
		base.OnNewAfterSave();
		_altUnit.Code = _unit.Code;
		_name.Text = "";
		_fullCaption.Text = "";
		_barCode.EditValue = "";
		_discount.EditValue = 0m;
		_pluCode.EditValue = null;
		_barCodesList.Clear();
		_myImageEdit.EditValue = null;
		_setFocusOnActivation = _name;
	}

	protected override void OnEdit()
	{
		base.OnEdit();
		_setFocusOnActivation = _name;
	}

	private void BarCode_MagicClick(object sender, MagicClickEventArgs e)
	{
		if (_barCode.DoValidate() && !string.IsNullOrWhiteSpace(_barCode.Text) && ArmSoft.SME.Core.UI.MessageBox.Show(string.Format(ArmSoft.SME.Trade.Common.Resources.GenerateBarcodeConfirm, _barCode.Text.Trim()), Settings.ProgramName, MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No) == MessageBoxResult.No)
		{
			return;
		}
		try
		{
			if (Settings.ParametersService.InternalBarcodeEANFormat())
			{
				_barCode.EditValue = MaterialHelpers.GenerateInnerBarcode();
			}
			else
			{
				_barCode.EditValue = Settings.ParametersService.InternalBarcodeNext();
			}
			_setNextBarcode = true;
			_barCode.SelectAll();
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	private void B2BBarCode_PreviewKeyDown(object sender, KeyEventArgs e)
	{
		if (HotKeys.WithModifier.Control_Shift_M.Matches(this, e))
		{
			B2BBarCodeButton_Click(sender, e);
		}
	}

	private void B2BBarCodeButton_Click(object sender, RoutedEventArgs e)
	{
		if (!_barCode.DoValidate() || string.IsNullOrWhiteSpace(_barCode.Text))
		{
			return;
		}
		try
		{
			b2bUnitedProductRowBase goodFromBarCode = BarcodeFromb2bCloudDialogWindow.GetGoodFromBarCode(_barCode.Text);
			if (goodFromBarCode != null)
			{
				FillControlsValuesFromProduct(goodFromBarCode);
				return;
			}
			throw new ASException(ArmSoft.SME.Common.Resources.ProductWithBarCodeDoesNotExistInB2B);
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	private void B2BCompareButton_Click(object sender, RoutedEventArgs e)
	{
		if (!_barCode.DoValidate() || string.IsNullOrWhiteSpace(_barCode.Text))
		{
			return;
		}
		try
		{
			b2bUnitedProductRowBase goodFromBarCode = BarcodeFromb2bCloudDialogWindow.GetGoodFromBarCode(_barCode.Text);
			if (goodFromBarCode != null)
			{
				DialogWindow dialogWindow = new DialogWindow
				{
					Caption = ArmSoft.SME.Common.Resources.ProductDataFromB2B,
					Name = "B2B",
					Mode = DialogWindowMode.ViewMode,
					Owner = this,
					HelpId = Definition.HelpId
				};
				TextEditExt textEditExt = dialogWindow.AddStringControl(null, "Code", ArmSoft.SME.Common.Resources.Code, 20);
				textEditExt.EditValue = _code.EditValue;
				textEditExt.IsReadOnly = true;
				TextEditExt textEditExt2 = dialogWindow.AddStringControl(null, "BarCode", ArmSoft.SME.Common.Resources.BarCode, 20);
				textEditExt2.EditValue = _barCode.Text;
				textEditExt2.IsReadOnly = true;
				LayoutGroupExt container = dialogWindow.AddHorizontalLayoutGroup();
				LayoutGroupExt container2 = dialogWindow.AddVerticalLayoutGroup(container, LayoutGroupView.GroupBox, ArmSoft.SME.Common.Resources.DataStoredInDB);
				LayoutGroupExt container3 = dialogWindow.AddVerticalLayoutGroup(container, LayoutGroupView.GroupBox, ArmSoft.SME.Common.Resources.DataStoredInB2B);
				TextEditExt textEditExt3 = dialogWindow.AddStringControl(container2, "Caption", ArmSoft.SME.Common.Resources.Name, 255);
				textEditExt3.EditValue = (string.IsNullOrWhiteSpace(_fullCaption.Text) ? _name.EditValue : _fullCaption.EditValue);
				textEditExt3.IsReadOnly = true;
				TextEditExt textEditExt4 = dialogWindow.AddStringControl(container3, "FullCaption_B2B", "", 255);
				textEditExt4.EditValue = goodFromBarCode.Name;
				textEditExt4.IsReadOnly = true;
				TextEditExt textEditExt5 = dialogWindow.AddStringControl(container2, "QuantityUnit", ArmSoft.SME.Common.Resources.QuantityUnitShort, 50);
				textEditExt5.EditValue = _unit.Comment;
				textEditExt5.IsReadOnly = true;
				TextEditExt textEditExt6 = dialogWindow.AddStringControl(container3, "QuantityUnit_B2B", "", 50);
				textEditExt6.EditValue = (goodFromBarCode.IsWeight ? ArmSoft.SME.Common.Resources.Weight : ArmSoft.SME.Common.Resources.Piece);
				textEditExt6.IsReadOnly = true;
				TextEditExt textEditExt7 = dialogWindow.AddStringControl(container2, "CPACode", ArmSoft.SME.Common.Resources.CPACodeShort, 255);
				CPACodesCacheRow item = Settings.CoreCacheService.CPACode.GetItem(_cpaCode.Code);
				if (item == null)
				{
					textEditExt7.EditValue = _cpaCode.Code;
				}
				else
				{
					textEditExt7.EditValue = _cpaCode.Code + " - " + item.Name;
				}
				textEditExt7.IsReadOnly = true;
				TextEditExt textEditExt8 = dialogWindow.AddStringControl(container3, "CPACode_B2B", "", 50);
				item = Settings.CoreCacheService.CPACode.GetItem(goodFromBarCode.CPACode);
				if (item == null)
				{
					textEditExt8.EditValue = goodFromBarCode.CPACode;
				}
				else
				{
					textEditExt8.EditValue = goodFromBarCode.CPACode + " - " + item.Name;
				}
				textEditExt8.IsReadOnly = true;
				CheckEditExt checkEditExt = dialogWindow.AddBooleanControl(container2, "VAT", ArmSoft.SME.Common.Resources.OptionsVAT);
				checkEditExt.IsChecked = _vat.IsChecked;
				checkEditExt.IsReadOnly = true;
				CheckEditExt checkEditExt2 = dialogWindow.AddBooleanControl(container3, "VAT_B2B", "");
				checkEditExt2.IsChecked = goodFromBarCode.IsVatTaxable;
				checkEditExt2.IsReadOnly = true;
				dialogWindow.ShowDialog();
				return;
			}
			throw new ASException(string.Format(ArmSoft.SME.Common.Resources.ProductNotExistInB2B, _barCode.Text));
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	private void FillControlsValuesFromProduct(b2bUnitedProductRowBase good)
	{
		if (good != null)
		{
			_name.EditValue = good.Name;
			_fullCaption.EditValue = good.FullName;
			_unit.Code = (good.IsWeight ? "303" : "001");
			_altUnit.Code = _unit.Code;
			_cpaCode.Code = good.CPACode;
			_vat.IsChecked = good.IsVatTaxable;
			_isWeight.IsChecked = good.IsWeight;
			_barCode.EditValue = good.Barcode;
		}
	}

	private void NewBarCode_Click(object sender, RoutedEventArgs e)
	{
		GoodBarCode goodBarCode = (GoodBarCode)_barCodesGrid.Grid.CurrentItem;
		if (goodBarCode != null)
		{
			goodBarCode.BarCode = AssignNewBarcode();
			_barCodesGrid.Grid.CurrentItem = goodBarCode;
			ControlsHelper.SetFocus(this);
		}
	}

	private void BarCodesGrid_DuplicatingRow(object sender, DocumentGridDuplicatingRowEventArgs e)
	{
		GoodBarCode goodBarCode = (GoodBarCode)_barCodesGrid.Grid.GetRow(e.RowHandle);
		if (goodBarCode != null)
		{
			_barCodesList.Add(new GoodBarCode(this)
			{
				BarCode = goodBarCode.BarCode,
				Brief = goodBarCode.Brief,
				UnitCode = goodBarCode.UnitCode
			});
			e.NewRowNumber = _barCodesList.Count - 1;
		}
	}

	private void BarCodesGrid_AddingRow(object sender, DocumentGridAddingRowEventArgs e)
	{
		bool flag = true;
		if (!PO.StoredInDb)
		{
			flag = ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Trade.Common.Resources.MustStoreGoodDocForAddUnits, Settings.ProgramName, MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No) == MessageBoxResult.Yes && (SaveDocument() ? true : false);
		}
		if (flag)
		{
			_barCodesList.Add(new GoodBarCode(this));
			_barCodeQntUnitCodeColumn.AllowEditing = DefaultBoolean.True;
			e.RowNumber = _barCodesList.Count - 1;
		}
	}

	private void BarCodesGrid_ClickDropDown(object sender, DocumentGridClickDropDownEventArgs e)
	{
		GoodBarCode goodBarCode = (GoodBarCode)_barCodesGrid.Grid.GetRow(e.RowHandle);
		if (goodBarCode == null)
		{
			return;
		}
		if (e.Column == _barCodeQntUnitCodeColumn)
		{
			if (e.Value == null)
			{
				return;
			}
			ArmSoft.SME.Trade.UI.DropDown.MTQntUnits mTQntUnits = new ArmSoft.SME.Trade.UI.DropDown.MTQntUnits(PO.Code);
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
				QntUnitCacheRow item = TradeSettings.TradeCacheService.QntUnit.GetItem(e.NewValue.ToString());
				if (item != null)
				{
					goodBarCode.Brief = item.Brief;
					_barCodesGrid.Grid.RefreshRow(e.RowHandle);
				}
			}
		}
		else if (e.Column == _barCodeColumn)
		{
			goodBarCode.BarCode = AssignNewBarcode();
			_barCodesGrid.Grid.RefreshRow(e.RowHandle);
		}
	}

	private void BarCodesTableView_CellValueChanging(object sender, CellValueChangedEventArgs e)
	{
		GoodBarCode goodBarCode = (GoodBarCode)_barCodesGrid.Grid.GetRow(e.RowHandle);
		if (goodBarCode != null && e.Column == _barCodeQntUnitCodeColumn && e.Value != null && !string.IsNullOrWhiteSpace(e.Value.ToString()))
		{
			MaterialCacheRow.QntUnitRow qntUnit = TradeSettings.TradeCacheService.Material.GetQntUnit(PO.Id, e.Value.ToString());
			if (qntUnit != null)
			{
				goodBarCode.Brief = TradeSettings.TradeCacheService.QntUnit.GetItem(qntUnit.QntUnit).Brief;
				_barCodesTableView.Grid.RefreshRow(e.RowHandle);
			}
		}
	}

	private void ImageEditValueCahnged(object sender, EditValueChangedEventArgs e)
	{
		CommonEventHandlers.ConvertImageEditValueChanged(sender, new ImageEditValueChangedEventArgs(e));
		PO.ImageEdited = true;
	}

	private void Caption_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		if (string.IsNullOrWhiteSpace((string)_fullCaption.EditValue))
		{
			_fullCaption.EditValue = _name.EditValue;
		}
	}

	private void Unit_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		if (!string.IsNullOrWhiteSpace(_unit.Code))
		{
			_altUnit.IsReadOnly = false;
			_altUnit.IsRequired = true;
			if (string.IsNullOrWhiteSpace(_altUnit.Code))
			{
				_altUnit.Code = _unit.Code;
				_txtBlockUnit.Text = "";
				_txtBlockAltUnit.Text = "";
			}
			else if (_unit.Code != _altUnit.Code)
			{
				_altUnitCoeff.IsReadOnly = false;
				_altUnitCoeff.IsRequired = true;
			}
			QntUnitCacheRow item = TradeSettings.TradeCacheService.QntUnit.GetItem(_unit.Code);
			QntUnitCacheRow item2 = TradeSettings.TradeCacheService.QntUnit.GetItem(_altUnit.Code);
			if (item != null)
			{
				_pricesGrid.Grid.Columns["UnitPrice"].Header = ArmSoft.SME.Trade.Common.Resources.NewPrices + " (" + item.Brief + ")";
			}
			else
			{
				_pricesGrid.Grid.Columns["UnitPrice"].Header = ArmSoft.SME.Trade.Common.Resources.NewPrices ?? "";
			}
			if (item2 != null)
			{
				_pricesGrid.Grid.Columns["AltUnitPrice"].Header = ArmSoft.SME.Trade.Common.Resources.NewPrices + " (" + item2.Brief + ")";
				if (item2.Code != item.Code)
				{
					_pricesGrid.Grid.Columns["AltUnitPrice"].Visible = true;
				}
				else
				{
					_pricesGrid.Grid.Columns["AltUnitPrice"].Visible = false;
				}
			}
			else
			{
				_pricesGrid.Grid.Columns["AltUnitPrice"].Header = ArmSoft.SME.Trade.Common.Resources.NewPrices ?? "";
			}
		}
		LabelPriceUnit_EditValueChanged();
	}

	private void AltUnit_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		if (_altUnit.Code != _unit.Code)
		{
			_altUnitCoeff.IsReadOnly = false;
			_altUnitCoeff.IsRequired = true;
			_pricesGrid.Grid.Columns["AltUnitPrice"].AllowEditing = DefaultBoolean.True;
			_txtBlockUnit.Text = ConstructUnitsFormulas(_unit.Code, _altUnit.Code, _altUnitCoeff.Value, calcUnitQtyFormAltUnit: true);
			_txtBlockAltUnit.Text = ConstructUnitsFormulas(_unit.Code, _altUnit.Code, _altUnitCoeff.Value, calcUnitQtyFormAltUnit: false);
		}
		else
		{
			_altUnitCoeff.Value = 1m;
			_altUnitCoeff.IsReadOnly = true;
			_altUnitCoeff.IsRequired = false;
			_pricesGrid.Grid.Columns["AltUnitPrice"].AllowEditing = DefaultBoolean.False;
			_txtBlockUnit.Text = "";
			_txtBlockAltUnit.Text = "";
		}
		QntUnitCacheRow item = TradeSettings.TradeCacheService.QntUnit.GetItem(_unit.Code);
		QntUnitCacheRow item2 = TradeSettings.TradeCacheService.QntUnit.GetItem(_altUnit.Code);
		if (item != null)
		{
			_pricesGrid.Grid.Columns["UnitPrice"].Header = ArmSoft.SME.Trade.Common.Resources.NewPrices + " (" + item.Brief + ")";
		}
		else
		{
			_pricesGrid.Grid.Columns["UnitPrice"].Header = ArmSoft.SME.Trade.Common.Resources.NewPrices ?? "";
		}
		if (item2 != null)
		{
			_pricesGrid.Grid.Columns["AltUnitPrice"].Header = ArmSoft.SME.Trade.Common.Resources.NewPrices + " (" + item2.Brief + ")";
			if (item != null && item2.Code != item.Code)
			{
				_pricesGrid.Grid.Columns["AltUnitPrice"].Visible = true;
			}
			else
			{
				_pricesGrid.Grid.Columns["AltUnitPrice"].Visible = false;
			}
		}
		else
		{
			_pricesGrid.Grid.Columns["AltUnitPrice"].Header = ArmSoft.SME.Trade.Common.Resources.NewPrices ?? "";
		}
	}

	private void AltUnitCoeff_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		if (!string.IsNullOrWhiteSpace(_altUnit.Code))
		{
			_txtBlockUnit.Text = ConstructUnitsFormulas(_unit.Code, _altUnit.Code, _altUnitCoeff.Value, calcUnitQtyFormAltUnit: true);
			_txtBlockAltUnit.Text = ConstructUnitsFormulas(_unit.Code, _altUnit.Code, _altUnitCoeff.Value, calcUnitQtyFormAltUnit: false);
		}
		else
		{
			_txtBlockUnit.Text = "";
			_txtBlockAltUnit.Text = "";
		}
	}

	public static string ConstructUnitsFormulas(string unit, string altUnit, decimal coeff, bool calcUnitQtyFormAltUnit, bool showViewExample = false)
	{
		string result = "";
		QntUnitCacheRow item = TradeSettings.TradeCacheService.QntUnit.GetItem(unit);
		QntUnitCacheRow item2 = TradeSettings.TradeCacheService.QntUnit.GetItem(altUnit);
		if (item != null && item2 != null && coeff != 0m)
		{
			result = ((!calcUnitQtyFormAltUnit) ? $"1 {item2.Brief} = {coeff.ToString("0.##########", CultureInfo.InvariantCulture)} {item.Brief}" : ($"1 {item.Brief} = {(1m / coeff).RoundAs(10).ToString("0.##########", CultureInfo.InvariantCulture)} {item2.Brief}" + (showViewExample ? (" (" + ArmSoft.SME.Trade.Common.Resources.ViewExaple + ")") : "")));
		}
		return result;
	}

	protected override void SetControlsReadonly()
	{
		base.SetControlsReadonly();
		if (_viewMode == WindowViewMode.View)
		{
			return;
		}
		if (PO.StoredInDb)
		{
			_code.IsReadOnly = true;
			_type.IsReadOnly = true;
			_pricesGrid.Grid.Columns["UnitPrice"].AllowEditing = DefaultBoolean.True;
			_pricesGrid.Grid.Columns["AltUnitPrice"].AllowEditing = DefaultBoolean.True;
			if (PO.GoodQntUnits.Count == 1 && !TradeSettings.CreateTradeClient().CommonServices.ExistInHIOrSales(PO.Id, PO.Type) && !TradeSettings.CreateTradeClient().CommonServices.ExistInPrices(PO.Id, PO.Unit, throwError: false) && !TradeSettings.CreateTradeClient().CommonServices.ExistInMTComplects(PO.Id, PO.Unit))
			{
				_unit.IsReadOnly = false;
				_unit.IsRequired = true;
				_altUnit.IsReadOnly = false;
				_altUnit.IsRequired = true;
				_altUnit.DataView = new ArmSoft.SME.Trade.UI.DropDown.QntUnits();
			}
			else
			{
				_unit.IsReadOnly = true;
				_altUnit.IsReadOnly = false;
				_altUnit.IsRequired = true;
				_altUnit.DataView = new ArmSoft.SME.Trade.UI.DropDown.QntUnits();
			}
			_altUnitCoeff.IsReadOnly = _unit.Code == _altUnit.Code;
			if (PO.GoodBarCodes.Count > 1)
			{
				_barCode.IsReadOnly = true;
			}
			else
			{
				_barCode.IsReadOnly = false;
			}
		}
		else
		{
			_altUnit.IsReadOnly = true;
		}
	}

	protected override void LoadDataToControls()
	{
		base.LoadDataToControls();
		DocumentGridRow.RefreshDocWindow(this);
		_myImageEdit.EditValue = PO.Img;
		Good.GoodQntUnit goodQntUnit = PO.GoodQntUnits.FirstOrDefault((Good.GoodQntUnit p) => p.Code == PO.AltUnit);
		if (goodQntUnit != null)
		{
			_altUnitCoeff.Value = goodQntUnit.Coeff;
		}
		else
		{
			_altUnitCoeff.Value = 1m;
		}
		_barCodesList.Clear();
		foreach (Good.GoodBarCode goodBarCode in PO.GoodBarCodes)
		{
			_barCodesList.Add(new GoodBarCode(this)
			{
				BarCode = goodBarCode.BarCode,
				Brief = goodBarCode.Brief,
				UnitCode = goodBarCode.UnitCode
			});
		}
		_barCodesGrid.ItemsSource = _barCodesList;
		ReportBarButtonItem barButtonJumpTo = _barButtonJumpTo;
		int num = 2;
		List<PropertyWithValue> list = new List<PropertyWithValue>(num);
		CollectionsMarshal.SetCount(list, num);
		Span<PropertyWithValue> span = CollectionsMarshal.AsSpan(list);
		span[0] = new PropertyWithValue("MtCode", PO.Code.ToString());
		span[1] = new PropertyWithValue("Extended", false.ToString());
		barButtonJumpTo.PropertyWithValues = list;
	}

	protected override void FillDocument(DirStoreRequest sr)
	{
		base.FillDocument(sr);
		PO.Code = _code.Text;
		PO.Name = _name.Text;
		PO.FullName = _fullCaption.Text;
		PO.Type = _type.Code;
		PO.Group = _group.Code;
		PO.SubstituteItemsGroup = "";
		PO.CPACode = _cpaCode.Code;
		PO.Unit = _unit.Code;
		PO.AltUnit = _altUnit.Code;
		Good pO = PO;
		string expenseMethod;
		if (_type.Code == GoodsTypesSE.Good.Value)
		{
			expenseMethod = (PO.StoredInDb ? PO.ExpenseMethod : ((string)MTExpenseMethodsSE.FIFO));
		}
		else
		{
			expenseMethod = "";
		}
		pO.ExpenseMethod = expenseMethod;
		PO.ShowInPriceList = true;
		PO.MainSupplier = _mainSupplier.Code;
		PO.Description = _description.Text;
		PO.ExternalCode = "";
		PO.Country = "";
		PO.Producer = "";
		PO.Property1 = "";
		PO.Property2 = "";
		PO.MinQTY = 0m;
		PO.MaxQTY = 0m;
		PO.Extra = 0m;
		PO.Coeff = 1m;
		PO.Bonus = ((_bonus != null) ? _bonus.Value : 0m);
		PO.Point = ((_bonus != null) ? _bonus.Value : 0m);
		PO.VAT = _vat.IsChecked.Value;
		PO.EcoFeeRate = 0m;
		PO.IsWeight = _isWeight.IsChecked.Value;
		PO.Discount = _discount.Value;
		PO.PLUCode = (int)_pluCode.Value;
		PO.CaptionEN = "";
		PO.CaptionRU = "";
		PO.AdditionalDescr1 = "";
		PO.AdditionalDescr2 = "";
		try
		{
			PO.Img = (byte[])_myImageEdit.EditValue;
		}
		catch (Exception)
		{
			try
			{
				BitmapSource bitmapSource = _myImageEdit.EditValue as InteropBitmap;
				if (bitmapSource != null)
				{
					JpegBitmapEncoder jpegBitmapEncoder = new JpegBitmapEncoder();
					jpegBitmapEncoder.Frames.Add(BitmapFrame.Create(bitmapSource));
					jpegBitmapEncoder.QualityLevel = 50;
					using MemoryStream memoryStream = new MemoryStream();
					jpegBitmapEncoder.Frames.Add(BitmapFrame.Create(bitmapSource));
					jpegBitmapEncoder.Save(memoryStream);
					PO.Img = memoryStream.ToArray();
				}
			}
			catch (Exception)
			{
				PO.Img = null;
			}
		}
		if (PO.UnitOld != _unit.Code)
		{
			Good.GoodQntUnit goodQntUnit = PO.GoodQntUnits.FirstOrDefault((Good.GoodQntUnit u) => u.Code == PO.UnitOld);
			if (goodQntUnit != null)
			{
				goodQntUnit.IsDefault = false;
			}
			PO.GoodQntUnits.Add(new Good.GoodQntUnit
			{
				Code = _unit.Code,
				Coeff = 1m,
				IsDefault = true
			});
		}
		Good.GoodQntUnit goodQntUnit2 = PO.GoodQntUnits.FirstOrDefault((Good.GoodQntUnit u) => u.Code == _altUnit.Code);
		if (goodQntUnit2 == null)
		{
			if (_unit.Code != _altUnit.Code)
			{
				PO.GoodQntUnits.Add(new Good.GoodQntUnit
				{
					Code = _altUnit.Code,
					Coeff = _altUnitCoeff.Value,
					IsDefault = false
				});
			}
		}
		else if (goodQntUnit2.Coeff != _altUnitCoeff.Value)
		{
			goodQntUnit2.Coeff = _altUnitCoeff.Value;
		}
		PO.GoodBarCodes.Clear();
		foreach (GoodBarCode barCodes in _barCodesList)
		{
			PO.GoodBarCodes.Add(new Good.GoodBarCode
			{
				BarCode = barCodes.BarCode,
				Brief = barCodes.Brief,
				UnitCode = barCodes.UnitCode
			});
		}
	}

	private static string AssignNewBarcode()
	{
		string result = "";
		try
		{
			result = ((!Settings.ParametersService.InternalBarcodeEANFormat()) ? GenerateNewBarcodeForOneRow() : GenerateNewEANBarcodeForOneRow());
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
		return result;
	}

	private static string GenerateNewEANBarcodeForOneRow()
	{
		string result = MaterialHelpers.GenerateInnerBarcode();
		Settings.ParametersService.InternalBarcodeNext_set(ArmSoft.SME.Common.Extensions.StringExtensions.NextDocNum(Settings.ParametersService.InternalBarcodeNext(), 10));
		return result;
	}

	private static string GenerateNewBarcodeForOneRow()
	{
		string result = Settings.ParametersService.InternalBarcodeNext();
		Settings.ParametersService.InternalBarcodeNext_set(ArmSoft.SME.Common.Extensions.StringExtensions.NextDocNum(Settings.ParametersService.InternalBarcodeNext(), 20));
		return result;
	}

	private void ByStorages_Click(object sender, RoutedEventArgs e)
	{
		if (!string.IsNullOrWhiteSpace(_code.Text))
		{
			ArmSoft.SME.Trade.UI.DataView.OrderMinMax orderMinMax = new ArmSoft.SME.Trade.UI.DataView.OrderMinMax();
			orderMinMax.MTCode = _code.Text;
			orderMinMax.Show(useParamValues: false, openAsChild: false, null, withoutShowDialog: true);
		}
	}

	protected override bool BeforeStore(DirStoreRequest sr)
	{
		if (!base.BeforeStore(sr))
		{
			return false;
		}
		if (Settings.ParametersService.MaterialCheckRepetition())
		{
			MaterialCacheRow firstItemByCaption = TradeSettings.TradeCacheService.Material.GetFirstItemByCaption(_name.Text, PO.Id);
			if (firstItemByCaption != null && ArmSoft.SME.Core.UI.MessageBox.Show(string.Format(ArmSoft.SME.Trade.Common.Resources.MaterialNameAlreadyExist, _name.Text.Trim(), firstItemByCaption.MTCode), Settings.ProgramName, MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No) == MessageBoxResult.No)
			{
				ControlsHelper.SetFocus(_name);
				return false;
			}
		}
		return true;
	}

	protected override void OnF2()
	{
		if (!_barCode.IsReadOnly)
		{
			ControlsHelper.SetFocus(_barCode);
		}
	}

	public override void OnScanner(string scannerData)
	{
		if (TabControls[0].IsSelected)
		{
			if (!_barCode.IsReadOnly)
			{
				_barCode.Text = scannerData;
			}
		}
		else if (TabControls[1].IsSelected)
		{
			BarCodesGrid_OnScanner(scannerData);
		}
	}

	private void BarCodesGrid_OnScanner(string scannerData)
	{
		if (_barCodesGrid.IsReadOnly || _barCodesGrid.Grid.CurrentItem == null)
		{
			return;
		}
		GoodBarCode goodBarCode = (GoodBarCode)_barCodesGrid.Grid.CurrentItem;
		if (goodBarCode != null)
		{
			goodBarCode.BarCode = scannerData;
			if (_barCodesTableView.ActiveEditor != null && _barCodesGrid.Grid.CurrentColumn.Name == _barCodeColumn.Name)
			{
				_barCodesTableView.ActiveEditor.EditValue = scannerData;
			}
			_barCodesGrid.Grid.RefreshData();
		}
	}

	private void OpenAltUnitCoeffDialog(object sender, MouseButtonEventArgs e)
	{
		if (!(_unit.Code == _altUnit.Code))
		{
			DialogWindow dialogWindow = new DialogWindow(this)
			{
				Caption = ArmSoft.SME.Trade.Common.Resources.AltUnitCoeff
			};
			LayoutGroupExt container = dialogWindow.AddHorizontalLayoutGroup();
			PopupCalcEditExt popupCalcEditExt = dialogWindow.AddNumericControl(container, "Coeff", "1 " + TradeSettings.TradeCacheService.QntUnit.GetItem(_unit.Code)?.Brief + " = ", 19, 10, isRequired: true);
			popupCalcEditExt.EditValue = (1m / _altUnitCoeff.Value).RoundAs(10);
			dialogWindow.AddTextBlock(container, " " + TradeSettings.TradeCacheService.QntUnit.GetItem(_altUnit.Code)?.Brief, TextBlockImage.None);
			if (dialogWindow.ShowDialog().Value)
			{
				_altUnitCoeff.EditValue = (1m / popupCalcEditExt.Value).RoundAs(10);
			}
		}
	}

	private void Type_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		bool flag = _type.Code == GoodsTypesSE.Good.Value;
		if (!flag)
		{
			_isWeight.IsChecked = false;
			_pluCode.EditValue = null;
			if (_useMarking)
			{
				_checkMarking.IsChecked = false;
			}
		}
		if (_useMarking)
		{
			_checkMarking.IsReadOnly = !flag;
		}
		_isWeight.IsReadOnly = !flag;
		_pluCode.IsReadOnly = !flag;
	}

	private void LabelPriceUnit_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		LabelPriceUnit_EditValueChanged();
	}

	private void LabelPriceUnit_EditValueChanged()
	{
		if (!string.IsNullOrWhiteSpace(_labelUnit.Code) && _labelCoeff.Value != 0m)
		{
			_txtBlockLabelPrice.Text = ConstructUnitsFormulas(_unit.Code, _labelUnit.Code, 1m / _labelCoeff.Value, calcUnitQtyFormAltUnit: true, showViewExample: true);
		}
		else
		{
			_txtBlockLabelPrice.Text = "";
		}
	}

	private void LabelPriceCoeff_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		if (!string.IsNullOrWhiteSpace(_altUnit.Code) && _labelCoeff.Value != 0m)
		{
			_txtBlockLabelPrice.Text = ConstructUnitsFormulas(_unit.Code, _labelUnit.Code, 1m / _labelCoeff.Value, calcUnitQtyFormAltUnit: true, showViewExample: true);
		}
		else
		{
			_txtBlockLabelPrice.Text = "";
		}
	}

	private void OpenLabelPriceDialog(object sender, MouseButtonEventArgs e)
	{
		if (!string.IsNullOrWhiteSpace(_labelUnit.Code))
		{
			LabelPriceDialogWindow labelPriceDialogWindow = new LabelPriceDialogWindow(PO.Id, _unit.Code, _labelUnit.Code, _labelCoeff.Value, this);
			if (labelPriceDialogWindow.ShowDialog().Value)
			{
				_labelCoeff.EditValue = labelPriceDialogWindow.Coeff.EditValue;
			}
		}
	}
}
