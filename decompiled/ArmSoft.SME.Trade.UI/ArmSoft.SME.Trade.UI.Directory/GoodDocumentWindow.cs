using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text.Json;
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
using ArmSoft.SME.Common.Models.SmartEnums;
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
using ArmSoft.SME.Trade.Client.Data.Directories;
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
public class GoodDocumentWindow : DirectoryDocumentWindow<Good>
{
	public class GoodQntUnit : DocumentGridRow
	{
		[ObservableProperty]
		private string code = string.Empty;

		[ObservableProperty]
		private string brief = string.Empty;

		[ObservableProperty]
		private decimal coeff = 1m;

		[ObservableProperty]
		private bool isDefault;

		[ObservableProperty]
		private byte[] image;

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public string Code
		{
			get
			{
				return code;
			}
			[MemberNotNull("code")]
			set
			{
				if (!EqualityComparer<string>.Default.Equals(code, value))
				{
					OnPropertyChanging(__KnownINotifyPropertyChangingArgs.Code);
					code = value;
					OnCodeChanged(value);
					OnPropertyChanged(__KnownINotifyPropertyChangedArgs.Code);
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
					OnCoeffChanged(value);
					OnPropertyChanged(__KnownINotifyPropertyChangedArgs.Coeff);
				}
			}
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public bool IsDefault
		{
			get
			{
				return isDefault;
			}
			set
			{
				if (!EqualityComparer<bool>.Default.Equals(isDefault, value))
				{
					OnPropertyChanging(__KnownINotifyPropertyChangingArgs.IsDefault);
					isDefault = value;
					OnPropertyChanged(__KnownINotifyPropertyChangedArgs.IsDefault);
				}
			}
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public byte[] Image
		{
			get
			{
				return image;
			}
			[MemberNotNull("image")]
			set
			{
				if (!EqualityComparer<byte[]>.Default.Equals(image, value))
				{
					OnPropertyChanging(__KnownINotifyPropertyChangingArgs.Image);
					image = value;
					OnPropertyChanged(__KnownINotifyPropertyChangedArgs.Image);
				}
			}
		}

		static GoodQntUnit()
		{
			DocumentGridRow.RegisterPropertyValidators(typeof(GoodQntUnit), cacheValidationResults: false, new Dictionary<string, Action<DocumentGridRow, DevExpress.XtraEditors.DXErrorProvider.ErrorInfo>>
			{
				{
					"Code",
					(DocumentGridRow row, DevExpress.XtraEditors.DXErrorProvider.ErrorInfo info) =>
					{
						ValidateQntUnitCode((GoodQntUnit)row, info);
					}
				},
				{
					"Coeff",
					(DocumentGridRow row, DevExpress.XtraEditors.DXErrorProvider.ErrorInfo info) =>
					{
						ValidateCoeff((GoodQntUnit)row, info);
					}
				}
			});
		}

		private static void ValidateQntUnitCode(GoodQntUnit row, DevExpress.XtraEditors.DXErrorProvider.ErrorInfo info)
		{
			TradePropertyValidator.ValidateUnitCode(row.Code, info, ArmSoft.SME.Common.Resources.Code);
		}

		private static void ValidateCoeff(GoodQntUnit row, DevExpress.XtraEditors.DXErrorProvider.ErrorInfo info)
		{
			PropertyValidator.PositiveValidate(row.Coeff, info, ArmSoft.SME.Common.Resources.Coefficient);
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		private void OnCodeChanged(string value)
		{
			ResetForceValidationBit("Code");
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		private void OnCoeffChanged(decimal value)
		{
			ResetForceValidationBit("Coeff");
		}
	}

	public class GoodBarCode(IPOWindowInfo docWindow) : DocumentGridRow(docWindow)
	{
		[ObservableProperty]
		private string unitCode = string.Empty;

		[ObservableProperty]
		private string brief = string.Empty;

		[ObservableProperty]
		private string barCode = string.Empty;

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
		public decimal oldPrice;

		[ObservableProperty]
		public decimal newPrice;

		public string Code { get; set; }

		public string Caption { get; set; }

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public decimal OldPrice
		{
			get
			{
				return oldPrice;
			}
			set
			{
				if (!EqualityComparer<decimal>.Default.Equals(oldPrice, value))
				{
					OnPropertyChanging(__KnownINotifyPropertyChangingArgs.OldPrice);
					oldPrice = value;
					OnPropertyChanged(__KnownINotifyPropertyChangedArgs.OldPrice);
				}
			}
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public decimal NewPrice
		{
			get
			{
				return newPrice;
			}
			set
			{
				if (!EqualityComparer<decimal>.Default.Equals(newPrice, value))
				{
					OnPropertyChanging(__KnownINotifyPropertyChangingArgs.NewPrice);
					newPrice = value;
					OnPropertyChanged(__KnownINotifyPropertyChangedArgs.NewPrice);
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

	private PopupCalcEditExt _ecoFeeRate;

	private PopupCalcEditExt _maxQTY;

	private DropDownControl _unit;

	private DropDownControl _altUnit;

	private TextBlock _txtBlockUnit;

	private TextBlock _txtBlockAltUnit;

	private DropDownControl _expMethod;

	private ButtonEditExt _barCode;

	private DropDownControl _labelUnit;

	private PopupCalcEditExt _labelCoeff;

	private TextBlock _txtBlockLabelPrice;

	private DropDownControl _country;

	private DropDownControl _producer;

	private DropDownControl _property1;

	private DropDownControl _property2;

	private CheckEditExt _showInPriceList;

	private CheckEditExt _vat;

	private CheckEditExt _checkMarking;

	private CheckEditExt _isWeight;

	private TextEditExt _description;

	private TextEditExt _externalCode;

	private PopupCalcEditExt _minQTY;

	private Button _byStorages;

	private PopupCalcEditExt _extra;

	private DropDownControl _substituteItemsGroup;

	private PopupCalcEditExt _bonus;

	private PopupCalcEditExt _overHeadExpDistrCoeff;

	private PopupCalcEditExt _discount;

	private PopupCalcEditExt _pluCode;

	private DocumentGridControl _qntUnitsGrid;

	private TableView _qntUnitsTableView;

	private DocumentGridControl _barCodesGrid;

	private TableView _barCodesTableView;

	private ObservableCollection<GoodQntUnit> _qntUnitsList;

	private ObservableCollection<GoodBarCode> _barCodesList;

	private DocumentGridColumn _qntUnitCodeColumn;

	private DocumentGridColumn _coeffColumn;

	private DocumentGridColumn _defaultUnitColumn;

	private DocumentGridColumn _barCodeQntUnitCodeColumn;

	private DocumentGridColumn _barCodeColumn;

	private BarEditItemExt _newBarCode;

	private ButtonInfo _b2bCaptionButton;

	private ButtonInfo _b2bFullCaptionButton;

	private ButtonInfo _b2bBarCodeButton;

	private ButtonInfo _b2bCompareButton;

	private Button _fillCurrentPrices;

	private BindingList<PricesGridRow> _prices;

	private DocumentGridControl pricesGrid;

	private Button _journalPrices;

	private Dictionary<string, MTPrice> _mtPricesByTypesAndBaseUnit;

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

	private readonly bool _useMarking = Settings.ParametersService.UseMarking();

	private bool _setNextBarcode;

	private LayoutGroup _lgQntUnits;

	private LayoutGroup _lgGeneral;

	private TextEditExt _mtNameEn;

	private TextEditExt _mtNameRu;

	private TextEditExt _additionalDescription1;

	private TextEditExt _additionalDescription2;

	protected override double WidthValue => 800.0;

	protected override double HeightValue => 600.0;

	protected override bool WriteInHistory => true;

	public GoodDocumentWindow()
	{
	}

	public GoodDocumentWindow(Good doc)
		: base(doc)
	{
	}

	protected override void ConstructUI()
	{
		//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_023e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0244: Unknown result type (might be due to invalid IL or missing references)
		//IL_07ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_07b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0941: Unknown result type (might be due to invalid IL or missing references)
		//IL_0947: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ce7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ced: Unknown result type (might be due to invalid IL or missing references)
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
		_b2bCaptionButton.Click += new SafeDocumentEventHandler<RoutedEventArgs>(b2bCaptionButton_Click, this).Handler;
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
		_b2bFullCaptionButton.Click += new SafeEventHandler<RoutedEventArgs>(b2bFullCaptionButton_Click).Handler;
		_fullCaption.PreviewKeyDown += new SafeDocumentEventHandler<KeyEventArgs>(b2bFullCaptionButton_PreviewKeyDown, this).Handler;
		_cpaCode = AddDropDownControl(layoutGroupExt, Definition.Fields["CPACode"], new ArmSoft.SME.Core.UI.DropDowns.CPACodes(), allowAdditionButtons: true);
		DXTabControl tc = AddTabControl(null);
		_lgGeneral = AddTabbedGroup(tc, ArmSoft.SME.Common.Resources.General, "General");
		_type = AddSmartEnumDropDownControl(_lgGeneral, Definition.Fields["Type"], allowAdditionButtons: false, fieldWithCaption: true, HorizontalAlignment.Stretch);
		_type.EditValueChanged += new SafeEventHandler<EditValueChangedEventArgs>(Type_EditValueChanged).Handler;
		_group = AddDropDownControl(_lgGeneral, Definition.Fields["Group"], new GoodsGroups(), allowAdditionButtons: true, fieldWithCaption: true, HorizontalAlignment.Stretch);
		LayoutGroupExt layoutGroupExt2 = AddLayoutGroup(_lgGeneral, LayoutGroupView.Group, Orientation.Horizontal);
		layoutGroupExt2.Height = 22.0;
		LayoutGroupExt layoutGroupExt3 = AddLayoutGroup(_lgGeneral, LayoutGroupView.Group, Orientation.Horizontal);
		layoutGroupExt3.Height = 22.0;
		_unit = AddDropDownControl(layoutGroupExt2, Definition.Fields["Unit"], new ArmSoft.SME.Trade.UI.DropDown.QntUnits(), allowAdditionButtons: false, fieldWithCaption: true, HorizontalAlignment.Stretch);
		_unit.EditValueChanged += new SafeEventHandler<EditValueChangedEventArgs>(Unit_EditValueChanged).Handler;
		_altUnit = AddDropDownControl(layoutGroupExt3, Definition.Fields["AltUnit"], new ArmSoft.SME.Trade.UI.DropDown.QntUnits(), allowAdditionButtons: false, fieldWithCaption: true, HorizontalAlignment.Stretch);
		_altUnit.EditValueChanged += new SafeEventHandler<EditValueChangedEventArgs>(AltUnit_EditValueChanged).Handler;
		_txtBlockUnit = new TextBlock
		{
			Text = "",
			Foreground = ColorsAS.Foreground.BlueBrush,
			TextWrapping = TextWrapping.NoWrap,
			FontWeight = FontWeights.Thin,
			Margin = new Thickness(0.0, 0.0, 0.0, 0.0),
			TextDecorations = TextDecorations.Underline
		};
		_txtBlockUnit.MouseLeftButtonDown += new SafeEventHandler<MouseButtonEventArgs>(OpenUnitCoeffDialog).Handler;
		_txtBlockAltUnit = new TextBlock
		{
			Text = "",
			Foreground = ColorsAS.Foreground.BlueBrush,
			TextWrapping = TextWrapping.NoWrap,
			FontWeight = FontWeights.Thin,
			Margin = new Thickness(0.0, 0.0, 0.0, 0.0),
			TextDecorations = TextDecorations.Underline
		};
		_txtBlockAltUnit.MouseLeftButtonDown += new SafeEventHandler<MouseButtonEventArgs>(OpenAltUnitCoeffDialog).Handler;
		layoutGroupExt2.Children.Add(_txtBlockUnit);
		layoutGroupExt3.Children.Add(_txtBlockAltUnit);
		_expMethod = AddDropDownControl(_lgGeneral, Definition.Fields["ExpenseMethod"], new SmartEnum(typeof(MTExpenseMethodsSE), null, padLeft: false, '0', allSubTypes: false, ValueMember.Code, CaptionMember.FullCaption), allowAdditionButtons: false, fieldWithCaption: true, HorizontalAlignment.Stretch);
		_expMethod.IsRequired = _type.Code != InvoiceRowTypeSE.Services.Value;
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
		_b2bBarCodeButton.Click += new SafeEventHandler<RoutedEventArgs>(b2bBarCodeButton_Click).Handler;
		_b2bCompareButton = AddButtonInfoToButtonEdit(_barCode, GlyphKind.Search, ArmSoft.SME.Common.Resources.ViewGoodFromB2B);
		_b2bCompareButton.Click += new SafeEventHandler<RoutedEventArgs>(b2bCompareButton_Click).Handler;
		TextBlock element = new TextBlock
		{
			Text = " - " + ArmSoft.SME.Common.Resources.FillProductInfoFromb2bByEAN,
			Foreground = ColorsAS.Foreground.BlueBrush,
			TextWrapping = TextWrapping.Wrap,
			FontWeight = FontWeights.Thin,
			Margin = new Thickness(0.0, -6.0, 0.0, 0.0)
		};
		_img = new DXImage
		{
			Width = 16.0,
			Height = 16.0,
			Source = ImageService.GetImage("cloud_16_blue.svg"),
			Margin = new Thickness(0.0, -6.0, 0.0, 0.0)
		};
		LayoutGroupExt layoutGroupExt4 = AddLayoutGroup(_lgGeneral, LayoutGroupView.Group, Orientation.Horizontal);
		layoutGroupExt4.Height = 28.0;
		layoutGroupExt4.Padding = new Thickness(12.0);
		layoutGroupExt4.Children.Add(_img);
		layoutGroupExt4.Children.Add(element);
		_vat = AddBooleanControl(_lgGeneral, Definition.Fields["VAT"]);
		_showInPriceList = AddBooleanControl(_lgGeneral, Definition.Fields["ShowInPriceList"]);
		if (_useMarking)
		{
			_checkMarking = AddBooleanControl(_lgGeneral, Definition.Fields["CheckMarking"]);
		}
		_isWeight = AddBooleanControl(_lgGeneral, Definition.Fields["IsWeight"]);
		_mainSupplier = AddDropDownControl(_lgGeneral, Definition.Fields["MainSupplier"], new PartnersFilter(PartnersFilter.ShowPartnersModeType.CodeCaption, "", isFilterBySuppliers: true), allowAdditionButtons: true, fieldWithCaption: true, HorizontalAlignment.Stretch);
		LayoutGroupExt layoutGroupExt5 = AddLayoutGroup(_lgGeneral, LayoutGroupView.GroupBox, Orientation.Vertical, ArmSoft.SME.Trade.Common.Resources.LabelPriceSettings);
		layoutGroupExt5.HorizontalAlignment = HorizontalAlignment.Stretch;
		layoutGroupExt5.IsCollapsible = true;
		_labelUnit = AddDropDownControl(layoutGroupExt5, Definition.Fields["LabelUnit"], new ArmSoft.SME.Trade.UI.DropDown.QntUnits(), allowAdditionButtons: false, fieldWithCaption: true, HorizontalAlignment.Stretch);
		_labelUnit.EditValueChanged += new SafeEventHandler<EditValueChangedEventArgs>(LabelPriceUnit_EditValueChanged).Handler;
		LayoutGroupExt layoutGroupExt6 = AddLayoutGroup(layoutGroupExt5, LayoutGroupView.Group, Orientation.Horizontal);
		layoutGroupExt6.Height = 22.0;
		_labelCoeff = AddNumericControl(layoutGroupExt6, Definition.Fields["LabelCoeff"]);
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
		layoutGroupExt6.Children.Add(_txtBlockLabelPrice);
		LayoutGroupExt layoutGroupExt7 = AddLayoutGroup(layoutGroupExt5, LayoutGroupView.Group, Orientation.Horizontal);
		layoutGroupExt7.Height = 35.0;
		layoutGroupExt7.Padding = new Thickness(12.0);
		layoutGroupExt7.Children.Add(new DXImage
		{
			Width = 16.0,
			Height = 16.0,
			Source = ImageService.GetImage("information_16.svg"),
			Margin = new Thickness(0.0, -6.0, 0.0, 0.0),
			VerticalAlignment = VerticalAlignment.Center
		});
		layoutGroupExt7.Children.Add(new TextBlock
		{
			Text = string.Format(ArmSoft.SME.Trade.Common.Resources.LabelPriceLawText, "²", "³"),
			Foreground = ColorsAS.Foreground.BlueBrush,
			TextWrapping = TextWrapping.Wrap,
			FontWeight = FontWeights.Thin,
			Margin = new Thickness(0.0, -6.0, 0.0, 0.0),
			VerticalAlignment = VerticalAlignment.Center
		});
		LayoutGroupExt layoutGroupExt8 = AddLayoutGroup(_lgGeneral, LayoutGroupView.GroupBox, Orientation.Vertical, ArmSoft.SME.Common.Resources.POForming);
		layoutGroupExt8.HorizontalAlignment = HorizontalAlignment.Stretch;
		layoutGroupExt8.IsCollapsible = true;
		LayoutGroupExt container3 = AddLayoutGroup(layoutGroupExt8, LayoutGroupView.Group, Orientation.Horizontal);
		_minQTY = AddNumericControl(container3, Definition.Fields["MinQTY"]);
		_maxQTY = AddNumericControl(layoutGroupExt8, Definition.Fields["MaxQTY"]);
		_byStorages = AddButton(container3, "", ArmSoft.SME.Trade.Common.Resources.ByStorages, "", ArmSoft.SME.Trade.Common.Resources.ByStorages, isTabStop: true);
		_byStorages.Click += new SafeDocumentEventHandler<RoutedEventArgs>(ByStorages_Click, this).Handler;
		LayoutGroup container4 = AddTabbedGroup(tc, ArmSoft.SME.Trade.Common.Resources.OtherInformation, "Other");
		_country = AddDropDownControl(container4, Definition.Fields["Country"], new ArmSoft.SME.Trade.UI.DropDown.Countries(), allowAdditionButtons: false, fieldWithCaption: true, HorizontalAlignment.Stretch);
		_producer = AddDropDownControl(container4, Definition.Fields["Producer"], new Producer(), allowAdditionButtons: false, fieldWithCaption: true, HorizontalAlignment.Stretch);
		_property1 = AddDropDownControl(container4, Definition.Fields["Property1"], new ArmSoft.SME.Trade.UI.DropDown.Properties(), allowAdditionButtons: false, fieldWithCaption: true, HorizontalAlignment.Stretch);
		_property1.EditValueChanged += new SafeEventHandler<EditValueChangedEventArgs>(Property1_EditValueChanged).Handler;
		_property2 = AddDropDownControl(container4, Definition.Fields["Property2"], new ArmSoft.SME.Trade.UI.DropDown.Properties(), allowAdditionButtons: false, fieldWithCaption: true, HorizontalAlignment.Stretch);
		_description = AddStringControl(container4, Definition.Fields["Description"], HorizontalAlignment.Stretch);
		_externalCode = AddStringControl(container4, Definition.Fields["ExternalCode"], HorizontalAlignment.Stretch);
		if (Settings.ParametersService.UseBonus())
		{
			AddBooleanControl(container4, Definition.Fields["IgnoreBonus"]);
			if (Settings.ParametersService.BonusType() == BonusTypeSE.ByPercent.Value)
			{
				_bonus = AddNumericControl(container4, Definition.Fields["Bonus"]);
			}
			else
			{
				_bonus = AddNumericControl(container4, Definition.Fields["Point"]);
			}
		}
		_extra = AddNumericControl(container4, Definition.Fields["Extra"]);
		_overHeadExpDistrCoeff = AddNumericControl(container4, Definition.Fields["Coeff"]);
		_discount = AddNumericControl(container4, Definition.Fields["Discount"]);
		_discount.Width = _extra.Width;
		if (Settings.ParametersService.EnvironmentalFeePayer())
		{
			_ecoFeeRate = AddNumericControl(container4, Definition.Fields["EcoFeeRate"]);
		}
		if (Settings.ParametersService.EnableSubstituteItems())
		{
			_substituteItemsGroup = AddDropDownControl(container4, Definition.Fields["SubstituteItemsGroup"], new ArmSoft.SME.Trade.UI.DropDown.SubstituteItemsGroups(), allowAdditionButtons: true, fieldWithCaption: true, HorizontalAlignment.Stretch);
		}
		_pluCode = AddNumericControl(container4, Definition.Fields["PLUCode"], useCurrencyFormat: false);
		_pluCode.AllowNullInput = true;
		_lgQntUnits = AddTabbedGroup(tc, ArmSoft.SME.Common.Resources.MeasureUnits, "QntUnits");
		_qntUnitsGrid = AddGrid(_lgQntUnits, Definition.Grids["GoodQntUnits"], HorizontalAlignment.Stretch, VerticalAlignment.Stretch);
		_qntUnitsTableView = _qntUnitsGrid.View;
		_qntUnitCodeColumn = _qntUnitsGrid.AddDropDownColumn(Definition.Grids["GoodQntUnits"].Columns["Code"], new ArmSoft.SME.Trade.UI.DropDown.MTQntUnits(), allowAdditionButtons: false, isOwnProcessingClickDropDown: true);
		_qntUnitCodeColumn.Width = 150.0;
		DocumentGridColumn documentGridColumn = _qntUnitsGrid.AddStringColumn(Definition.Grids["GoodQntUnits"].Columns["Brief"]);
		documentGridColumn.Width = 100.0;
		documentGridColumn.ReadOnly = true;
		_coeffColumn = _qntUnitsGrid.AddNumberColumn(Definition.Grids["GoodQntUnits"].Columns["Coeff"]);
		_coeffColumn.Width = 110.0;
		_defaultUnitColumn = _qntUnitsGrid.AddBooleanColumn(Definition.Grids["GoodQntUnits"].Columns["IsDefault"]);
		_defaultUnitColumn.ReadOnly = true;
		_qntUnitsList = new ObservableCollection<GoodQntUnit>();
		_qntUnitsGrid.AddingRow += new SafeEventHandler<DocumentGridAddingRowEventArgs>(QntUnitsGrid_AddingRow).Handler;
		_qntUnitsGrid.ClickDropDown += new SafeEventHandler<DocumentGridClickDropDownEventArgs>(QntUnitsGrid_ClickDropDown).Handler;
		_qntUnitsGrid.DeletingRow += new SafeEventHandler<DocumentGridDeletingRowEventArgs>(QntUnitsGrid_DeletingRow).Handler;
		_qntUnitsTableView.ShowingEditor += new SafeEventHandler<ShowingEditorEventArgs>(QntUnitsTableView_ShowingEditor).Handler;
		_qntUnitsTableView.CellValueChanging += new SafeEventHandler<CellValueChangedEventArgs>(QntUnitsTableView_CellValueChanging).Handler;
		_qntUnitsTableView.CellValueChanged += new SafeEventHandler<CellValueChangedEventArgs>(QntUnitsTableView_CellValueChanged).Handler;
		_qntUnitsGrid.DuplicatingRow += new SafeEventHandler<DocumentGridDuplicatingRowEventArgs>(QntUnitsGrid_DuplicatingRow).Handler;
		LayoutGroup container5 = AddTabbedGroup(tc, ArmSoft.SME.Common.Resources.BarCodes, "BarCodes");
		_barCodesGrid = AddGrid(container5, Definition.Grids["GoodBarCodes"], HorizontalAlignment.Stretch, VerticalAlignment.Stretch);
		_barCodesTableView = _barCodesGrid.View;
		_newBarCode = _barCodesGrid.AddButton("", ArmSoft.SME.Trade.Common.Resources.NewBarcodeShort, ArmSoft.SME.Trade.Common.Resources.NewBarcode, 110.0);
		_barCodeQntUnitCodeColumn = _barCodesGrid.AddDropDownColumn(Definition.Grids["GoodBarCodes"].Columns["UnitCode"], new ArmSoft.SME.Trade.UI.DropDown.MTQntUnits(PO.Code), allowAdditionButtons: false, isOwnProcessingClickDropDown: true);
		_barCodeQntUnitCodeColumn.Width = 150.0;
		DocumentGridColumn documentGridColumn2 = _barCodesGrid.AddStringColumn(Definition.Grids["GoodBarCodes"].Columns["Brief"]);
		documentGridColumn2.Width = 100.0;
		documentGridColumn2.AllowEditing = DefaultBoolean.False;
		_barCodeColumn = _barCodesGrid.AddDropDownColumn(Definition.Grids["GoodBarCodes"].Columns["BarCode"], trimValue: true, GlyphKind.Apply);
		_barCodeColumn.Width = 150.0;
		_barCodesList = new ObservableCollection<GoodBarCode>();
		_barCodesTableView.CellValueChanging += new SafeEventHandler<CellValueChangedEventArgs>(BarCodesTableView_CellValueChanging).Handler;
		_barCodesGrid.ClickDropDown += new SafeEventHandler<DocumentGridClickDropDownEventArgs>(BarCodesGrid_ClickDropDown).Handler;
		_barCodesGrid.AddingRow += new SafeEventHandler<DocumentGridAddingRowEventArgs>(BarCodesGrid_AddingRow).Handler;
		_barCodesGrid.DuplicatingRow += new SafeEventHandler<DocumentGridDuplicatingRowEventArgs>(BarCodesGrid_DuplicatingRow).Handler;
		_newBarCode.Click += new SafeEventHandler<RoutedEventArgs>(NewBarCode_Click).Handler;
		LayoutGroup container6 = AddTabbedGroup(tc, ArmSoft.SME.Common.Resources.Prices, "Prices");
		LayoutGroupExt container7 = AddLayoutGroup(container6, LayoutGroupView.Group, Orientation.Horizontal);
		_fillCurrentPrices = AddButton(container7, "", ArmSoft.SME.Trade.Common.Resources.FillCurrentPrices, "", ArmSoft.SME.Trade.Common.Resources.FillCurrentPrices, isTabStop: true);
		_fillCurrentPrices.Click += new SafeEventHandler<RoutedEventArgs>(FillCurrentPrices_Click).Handler;
		_journalPrices = AddButton(container7, "", ArmSoft.SME.Trade.Common.Resources.JournalPrices, "", ArmSoft.SME.Trade.Common.Resources.JournalPrices, isTabStop: true);
		_journalPrices.Click += new SafeEventHandler<RoutedEventArgs>(JournalPrices_Click).Handler;
		pricesGrid = AddGrid(container6, "", HorizontalAlignment.Stretch, VerticalAlignment.Stretch, isSupportDialogMode: false, "PriceGrid");
		pricesGrid.AddStringColumn("Caption", ArmSoft.SME.Trade.Common.Resources.PriceType, 50).AllowEditing = DefaultBoolean.False;
		pricesGrid.AddNumberColumn("OldPrice", ArmSoft.SME.Trade.Common.Resources.CurrentPrices, 16, 2).AllowEditing = DefaultBoolean.False;
		bool flag = _viewMode == WindowViewMode.View || !Settings.UserAccessService.GetProviderPermission(DataProviderTypeSE.MTPrices).AllowOperation;
		pricesGrid.AddNumberColumn("NewPrice", ArmSoft.SME.Trade.Common.Resources.NewPrices, 16, 2).AllowEditing = (flag ? DefaultBoolean.False : DefaultBoolean.True);
		_prices = new BindingList<PricesGridRow>();
		pricesGrid.ItemsSource = _prices;
		pricesGrid.ShowButtons = false;
		_mtPriceType = (from p in TradeSettings.TradeCacheService.PriceType.GetList()
			orderby p.Key
			select p).Take(10).ToDictionary((PriceTypeCacheRow p) => p.Key, (PriceTypeCacheRow p) => p);
		foreach (PriceTypeCacheRow value in _mtPriceType.Values)
		{
			_prices.Add(new PricesGridRow
			{
				Code = value.Code,
				Caption = value.Caption + " (" + value.CurrCode + (string.IsNullOrWhiteSpace(PO.UnitBrief) ? "" : (", " + PO.UnitBrief)) + ")",
				OldPrice = 0m,
				NewPrice = 0m
			});
		}
		LayoutGroup container8 = AddTabbedGroup(tc, ArmSoft.SME.Trade.Common.Resources.OnlineShop, "OnlineShop");
		_mtNameEn = AddStringControl(container8, Definition.Fields["CaptionEN"]);
		_mtNameRu = AddStringControl(container8, Definition.Fields["CaptionRU"]);
		_additionalDescription1 = AddStringControl(container8, Definition.Fields["AdditionalDescr1"]);
		_additionalDescription2 = AddStringControl(container8, Definition.Fields["AdditionalDescr2"]);
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
		_barButtonMTRepRetailAvail = new ReportBarButtonItem(DataProviderTypeSE.MatRepAvailabilityRetail, showDialog: true, ArmSoft.SME.Trade.Common.Resources.MatRepAvailShort);
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
		List<PricesFormationWindow.GoodsRow> list = new List<PricesFormationWindow.GoodsRow>();
		list.Add(new PricesFormationWindow.GoodsRow
		{
			Code = PO.Code
		});
		new PricesFormationWindow(list, "PricesFormationDialog").Invoke();
	}

	protected void BarButtonPrintLabel_ItemClick(object sender, ItemClickEventArgs e)
	{
		List<ArmSoft.SME.Trade.Common.Models.PrintableElements.Label.GoodLabelRow> list = new List<ArmSoft.SME.Trade.Common.Models.PrintableElements.Label.GoodLabelRow>();
		ArmSoft.SME.Trade.Common.Models.PrintableElements.Label.GoodLabelRow goodLabelRow = InitContextFunctionsHelper.BuildLabelGoodRow(TradeSettings.TradeCacheService.Material.GetItem(PO.Code));
		if (goodLabelRow != null)
		{
			list.Add(goodLabelRow);
		}
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
		foreach (KeyValuePair<string, MTPrice> price in _mtPricesByTypesAndBaseUnit)
		{
			PricesGridRow pricesGridRow = _prices.FirstOrDefault((PricesGridRow p) => p.Code == price.Key);
			if (pricesGridRow != null)
			{
				pricesGridRow.OldPrice = price.Value.Price;
			}
		}
	}

	private void SaveNewPrices()
	{
		_mtPricesByTypesAndBaseUnit = TradeSettings.CreateTradeClient().CommonServices.GetMTPricesByTypesAndBaseUnit(PO.Id, _prices.Select((PricesGridRow p) => p.Code).ToList());
		pricesGrid.View.CommitEditing();
		List<PriceFormationRequestProperties> list = new List<PriceFormationRequestProperties>();
		foreach (PricesGridRow price in _prices)
		{
			if (price.NewPrice != 0m && _mtPricesByTypesAndBaseUnit.TryGetValue(price.Code, out var value))
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
					ItemPrice = price.NewPrice,
					MTCodeOld = PO.Code,
					QntUnitOld = value.Unit,
					PriceTypeOld = value.PriceType,
					PriceDateOld = value.Date
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
			if (price2.NewPrice > 0m)
			{
				price2.OldPrice = price2.NewPrice;
				price2.NewPrice = 0m;
			}
		}
		TradeSettings.TradeCacheService.CurrentPrices.Refresh();
	}

	private void Caption_PreviewKeyDown(object sender, KeyEventArgs e)
	{
		if (HotKeys.WithModifier.Control_Shift_M.Matches(this, e))
		{
			b2bCaptionButton_Click(_name);
		}
	}

	private void b2bCaptionButton_Click(object sender, RoutedEventArgs e)
	{
		b2bCaptionButton_Click(_name);
	}

	private void b2bFullCaptionButton_PreviewKeyDown(object sender, KeyEventArgs e)
	{
		if (HotKeys.WithModifier.Control_Shift_M.Matches(this, e))
		{
			b2bCaptionButton_Click(_fullCaption);
		}
	}

	private void b2bFullCaptionButton_Click(object sender, RoutedEventArgs e)
	{
		b2bCaptionButton_Click(_fullCaption);
	}

	private void b2bCaptionButton_Click(ButtonEditExt captionControl)
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
		_externalCode.Text = "";
		_overHeadExpDistrCoeff.EditValue = 1m;
		_discount.EditValue = 0m;
		_pluCode.EditValue = null;
		_qntUnitsList.Clear();
		_barCodesList.Clear();
		_myImageEdit.EditValue = null;
		_setFocusOnActivation = _name;
		foreach (PricesGridRow price in _prices)
		{
			price.OldPrice = 0m;
		}
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
			b2bBarCodeButton_Click(sender, e);
		}
	}

	private void b2bBarCodeButton_Click(object sender, RoutedEventArgs e)
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

	private void b2bCompareButton_Click(object sender, RoutedEventArgs e)
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

	private void QntUnitsTableView_CellValueChanged(object sender, CellValueChangedEventArgs e)
	{
		if ((GoodQntUnit)_qntUnitsGrid.Grid.GetRow(e.RowHandle) != null && e.Column == _coeffColumn)
		{
			AltUnit_EditValueChanged();
		}
	}

	private void QntUnitsTableView_CellValueChanging(object sender, CellValueChangedEventArgs e)
	{
		GoodQntUnit goodQntUnit = (GoodQntUnit)_qntUnitsGrid.Grid.GetRow(e.RowHandle);
		if (goodQntUnit != null && e.Column == _qntUnitCodeColumn && e.Value != null && !string.IsNullOrWhiteSpace(e.Value.ToString()))
		{
			QntUnitCacheRow item = TradeSettings.TradeCacheService.QntUnit.GetItem(e.Value.ToString());
			if (item != null)
			{
				goodQntUnit.Brief = item.Brief;
				goodQntUnit.IsDefault = false;
				_qntUnitsGrid.Grid.RefreshData();
			}
		}
	}

	private void QntUnitsGrid_DuplicatingRow(object sender, DocumentGridDuplicatingRowEventArgs e)
	{
		if (!PO.StoredInDb)
		{
			if (ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Trade.Common.Resources.MustStoreGoodDocForEditUnits, Settings.ProgramName, MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No) == MessageBoxResult.Yes)
			{
				SaveDocument();
			}
			return;
		}
		GoodQntUnit goodQntUnit = (GoodQntUnit)_qntUnitsGrid.Grid.GetRow(e.RowHandle);
		if (goodQntUnit != null)
		{
			_qntUnitsList.Add(new GoodQntUnit
			{
				Code = goodQntUnit.Code,
				Brief = goodQntUnit.Brief,
				Coeff = goodQntUnit.Coeff,
				IsDefault = false,
				Image = goodQntUnit.Image
			});
			e.NewRowNumber = _qntUnitsList.Count - 1;
		}
	}

	private void QntUnitsTableView_ShowingEditor(object sender, ShowingEditorEventArgs e)
	{
		GoodQntUnit goodQntUnit = (GoodQntUnit)e.Row;
		if (goodQntUnit != null && goodQntUnit.IsDefault)
		{
			e.Cancel = true;
		}
	}

	private void QntUnitsGrid_DeletingRow(object sender, DocumentGridDeletingRowEventArgs e)
	{
		if (!PO.StoredInDb)
		{
			if (ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Trade.Common.Resources.MustStoreGoodDocForEditUnits, Settings.ProgramName, MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No) == MessageBoxResult.Yes)
			{
				SaveDocument();
			}
			e.Cancel = true;
			return;
		}
		GoodQntUnit goodQntUnit = (GoodQntUnit)_qntUnitsGrid.Grid.GetRow(e.RowHandle);
		if (goodQntUnit == null || string.IsNullOrWhiteSpace(goodQntUnit.Code))
		{
			return;
		}
		if (goodQntUnit.IsDefault)
		{
			ArmSoft.SME.Core.UI.MessageBox.Show(string.Format(ArmSoft.SME.Trade.Common.Resources.CantDeleteDef, ArmSoft.SME.Trade.Common.Resources.BaseQUNIT), Settings.ProgramName, MessageBoxButton.OK);
			e.Cancel = true;
			return;
		}
		if (goodQntUnit.Code == _altUnit.Code)
		{
			e.Cancel = true;
			throw new ASException(ArmSoft.SME.Trade.Common.Resources.CantDeleteAlt);
		}
		try
		{
			Settings.DirectoryService.ValidateDelete(DirectoryTypeSE.MTQntUnit.Value, JsonSerializer.Serialize(new MTQntUnitsDataProvider.DataRow.MTQntUnitsRowKey
			{
				MTID = PO.Id,
				QntUnit = goodQntUnit.Code
			}));
		}
		catch (Exception ex)
		{
			e.Cancel = true;
			throw new ASException(ex.Message, ex);
		}
	}

	private void QntUnitsGrid_ClickDropDown(object sender, DocumentGridClickDropDownEventArgs e)
	{
		if (!PO.StoredInDb)
		{
			if (ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Trade.Common.Resources.MustStoreGoodDocForEditUnits, Settings.ProgramName, MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No) == MessageBoxResult.Yes)
			{
				SaveDocument();
			}
			return;
		}
		GoodQntUnit goodQntUnit = (GoodQntUnit)_qntUnitsGrid.Grid.GetRow(e.RowHandle);
		if (goodQntUnit == null || goodQntUnit.IsDefault || e.Column != _qntUnitCodeColumn || e.Value == null)
		{
			return;
		}
		ArmSoft.SME.Trade.UI.DropDown.QntUnits qntUnits = new ArmSoft.SME.Trade.UI.DropDown.QntUnits();
		ModalBrowser modalBrowser = new ModalBrowser(qntUnits)
		{
			LookUpValue = e.Value,
			Owner = e.Owner,
			Left = e.Left,
			Top = e.Top
		};
		if (modalBrowser.Show())
		{
			e.NewValue = qntUnits.ValueMember(modalBrowser.SelectedRowIndex);
			QntUnitCacheRow item = TradeSettings.TradeCacheService.QntUnit.GetItem(e.NewValue);
			if (item != null)
			{
				goodQntUnit.Brief = item.Brief;
				goodQntUnit.IsDefault = false;
				_qntUnitsGrid.Grid.RefreshData();
			}
		}
	}

	private void QntUnitsGrid_AddingRow(object sender, DocumentGridAddingRowEventArgs e)
	{
		bool flag = true;
		if (!PO.StoredInDb)
		{
			flag = ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Trade.Common.Resources.MustStoreGoodDocForAddUnits, Settings.ProgramName, MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No) == MessageBoxResult.Yes && (SaveDocument() ? true : false);
		}
		if (flag)
		{
			_qntUnitsList.Add(new GoodQntUnit());
			e.RowNumber = _qntUnitsList.Count - 1;
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

	private void Property1_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		if (!string.IsNullOrWhiteSpace(_property1.Code))
		{
			_property2.IsReadOnly = false;
		}
		else if ((e.NewValue == null || e.NewValue.ToString() == "") && e.OldValue != null && e.OldValue.ToString() != "" && !string.IsNullOrWhiteSpace(_property2.Code))
		{
			string code = _property2.Code;
			_property2.Code = "";
			_property2.IsReadOnly = true;
			_property1.Code = code;
			_property2.DoValidate();
		}
		else
		{
			_property2.Code = "";
			_property2.IsReadOnly = true;
		}
		_property1.DoValidate();
	}

	private void Unit_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		string text = e.NewValue.ToString();
		foreach (GoodQntUnit qntUnits in _qntUnitsList)
		{
			qntUnits.Coeff = 1m;
			if (qntUnits.Code == text)
			{
				qntUnits.IsDefault = true;
			}
			else
			{
				qntUnits.IsDefault = false;
			}
		}
		_qntUnitsGrid.Grid.RefreshData();
		if (!_unit.IsReadOnly)
		{
			_altUnit.Code = _unit.Code;
		}
		string text2 = TradeSettings.TradeCacheService.QntUnit.GetItem(_unit.Code)?.Brief ?? "";
		foreach (PricesGridRow price in _prices)
		{
			if (_mtPriceType.TryGetValue(price.Code, out var value))
			{
				price.Caption = value.Caption + " (" + value.CurrCode + (string.IsNullOrWhiteSpace(text2) ? "" : (", " + text2)) + ")";
			}
		}
		pricesGrid.Grid.RefreshData();
		LabelPriceUnit_EditValueChanged();
	}

	private void AltUnit_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		AltUnit_EditValueChanged();
	}

	private void AltUnit_EditValueChanged()
	{
		if (_unit.Code != _altUnit.Code)
		{
			GoodQntUnit goodQntUnit = _qntUnitsList.FirstOrDefault((GoodQntUnit r) => r.Code == _altUnit.Code);
			if (goodQntUnit != null)
			{
				_txtBlockUnit.Text = GoodDocumentSimpleWindow.ConstructUnitsFormulas(_unit.Code, _altUnit.Code, goodQntUnit.Coeff, calcUnitQtyFormAltUnit: true);
				_txtBlockAltUnit.Text = GoodDocumentSimpleWindow.ConstructUnitsFormulas(_unit.Code, _altUnit.Code, goodQntUnit.Coeff, calcUnitQtyFormAltUnit: false);
			}
		}
		else
		{
			_txtBlockUnit.Text = "";
			_txtBlockAltUnit.Text = "";
		}
	}

	private void Type_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		bool flag = _type.Code == GoodsTypesSE.Good.Value;
		if (flag)
		{
			if (e.OldValue != null && e.NewValue != null && e.NewValue != e.OldValue)
			{
				_expMethod.Code = "1";
			}
			_property1.IsReadOnly = false;
			if (string.IsNullOrWhiteSpace(_property1.Code))
			{
				_property2.IsReadOnly = true;
			}
			else
			{
				_property2.IsReadOnly = false;
			}
		}
		else
		{
			_expMethod.Code = string.Empty;
			_property1.Code = string.Empty;
			_property2.Code = string.Empty;
			if (_useMarking)
			{
				_checkMarking.IsChecked = false;
			}
			_isWeight.IsChecked = false;
			_pluCode.EditValue = null;
			_minQTY.EditValue = 0m;
			_maxQTY.EditValue = 0m;
			_extra.EditValue = 0m;
			_property1.IsReadOnly = true;
			_property2.IsReadOnly = true;
		}
		_expMethod.IsReadOnly = !flag;
		_expMethod.IsRequired = flag;
		_expMethod.DoValidate();
		if (_useMarking)
		{
			_checkMarking.IsReadOnly = !flag;
		}
		_isWeight.IsReadOnly = !flag;
		_pluCode.IsReadOnly = !flag;
		_maxQTY.IsReadOnly = !flag;
		_minQTY.IsReadOnly = !flag;
		_byStorages.IsEnabled = flag;
		_extra.IsReadOnly = !flag;
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
			_byStorages.IsEnabled = _type.Code == GoodsTypesSE.Good;
			pricesGrid.Grid.Columns["NewPrice"].AllowEditing = DefaultBoolean.True;
			if (PO.GoodQntUnits.Count == 1 && !TradeSettings.CreateTradeClient().CommonServices.ExistInHIOrSales(PO.Id, PO.Type) && !TradeSettings.CreateTradeClient().CommonServices.ExistInPrices(PO.Id, PO.Unit, throwError: false) && !TradeSettings.CreateTradeClient().CommonServices.ExistInMTComplects(PO.Id, PO.Unit))
			{
				_unit.IsReadOnly = false;
				_unit.IsRequired = true;
				_altUnit.IsReadOnly = true;
				_altUnit.DataView = new ArmSoft.SME.Trade.UI.DropDown.QntUnits();
			}
			else
			{
				_unit.IsReadOnly = true;
				_altUnit.IsReadOnly = false;
				_altUnit.IsRequired = true;
				_altUnit.DataView = new ArmSoft.SME.Trade.UI.DropDown.MTQntUnits(PO.Code, isShowQntUnitCaption: true);
			}
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
			_byStorages.IsEnabled = false;
		}
		bool allowView = Settings.UserAccessService.GetProviderPermission(DataProviderTypeSE.MTPrices).AllowView;
		_journalPrices.IsEnabled = (_viewMode == WindowViewMode.Edit) & allowView;
		_fillCurrentPrices.IsEnabled = (_viewMode == WindowViewMode.Edit) & allowView;
	}

	protected override void LoadDataToControls()
	{
		base.LoadDataToControls();
		DocumentGridRow.RefreshDocWindow(this);
		_qntUnitsList.Clear();
		_myImageEdit.EditValue = PO.Img;
		foreach (Good.GoodQntUnit goodQntUnit in PO.GoodQntUnits)
		{
			_qntUnitsList.Add(new GoodQntUnit
			{
				Code = goodQntUnit.Code,
				Brief = goodQntUnit.Brief,
				Coeff = goodQntUnit.Coeff,
				IsDefault = goodQntUnit.IsDefault,
				Image = goodQntUnit.Image
			});
		}
		_qntUnitsGrid.ItemsSource = _qntUnitsList;
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
		AltUnit_EditValueChanged();
	}

	protected override void FillDocument(DirStoreRequest sr)
	{
		base.FillDocument(sr);
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
		PO.GoodQntUnits.Clear();
		foreach (GoodQntUnit qntUnits in _qntUnitsList)
		{
			if (qntUnits.Code == PO.Unit)
			{
				qntUnits.IsDefault = true;
			}
			else
			{
				qntUnits.IsDefault = false;
			}
			PO.GoodQntUnits.Add(new Good.GoodQntUnit
			{
				Code = qntUnits.Code,
				Brief = qntUnits.Brief,
				Coeff = qntUnits.Coeff,
				Image = qntUnits.Image,
				IsDefault = qntUnits.IsDefault
			});
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
		else if (TabControls[3].IsSelected)
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

	private void OpenUnitCoeffDialog(object sender, MouseButtonEventArgs e)
	{
		if (!(_unit.Code == _altUnit.Code))
		{
			DialogWindow dialogWindow = new DialogWindow(this)
			{
				Caption = ArmSoft.SME.Trade.Common.Resources.AltUnitCoeff
			};
			LayoutGroupExt container = dialogWindow.AddHorizontalLayoutGroup();
			PopupCalcEditExt popupCalcEditExt = dialogWindow.AddNumericControl(container, "Coeff", "1 " + TradeSettings.TradeCacheService.QntUnit.GetItem(_unit.Code)?.Brief + " = ", 19, 10, isRequired: true);
			GoodQntUnit goodQntUnit = _qntUnitsList.FirstOrDefault((GoodQntUnit r) => r.Code == _altUnit.Code);
			if (goodQntUnit != null)
			{
				popupCalcEditExt.EditValue = (1m / goodQntUnit.Coeff).RoundAs(10);
			}
			dialogWindow.AddTextBlock(container, " " + TradeSettings.TradeCacheService.QntUnit.GetItem(_altUnit.Code)?.Brief, TextBlockImage.None);
			if (dialogWindow.ShowDialog().Value && goodQntUnit != null)
			{
				goodQntUnit.Coeff = (1m / popupCalcEditExt.Value).RoundAs(10);
				AltUnit_EditValueChanged();
			}
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
			PopupCalcEditExt popupCalcEditExt = dialogWindow.AddNumericControl(container, "Coeff", "1 " + TradeSettings.TradeCacheService.QntUnit.GetItem(_altUnit.Code)?.Brief + " = ", 19, 10, isRequired: true);
			GoodQntUnit goodQntUnit = _qntUnitsList.FirstOrDefault((GoodQntUnit r) => r.Code == _altUnit.Code);
			if (goodQntUnit != null)
			{
				popupCalcEditExt.EditValue = goodQntUnit.Coeff;
			}
			dialogWindow.AddTextBlock(container, " " + TradeSettings.TradeCacheService.QntUnit.GetItem(_unit.Code)?.Brief, TextBlockImage.None);
			if (dialogWindow.ShowDialog().Value && goodQntUnit != null)
			{
				goodQntUnit.Coeff = popupCalcEditExt.Value;
				AltUnit_EditValueChanged();
			}
		}
	}

	private void LabelPriceUnit_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		LabelPriceUnit_EditValueChanged();
	}

	private void LabelPriceUnit_EditValueChanged()
	{
		if (!string.IsNullOrWhiteSpace(_labelUnit.Code) && _labelCoeff.Value != 0m)
		{
			_txtBlockLabelPrice.Text = GoodDocumentSimpleWindow.ConstructUnitsFormulas(_unit.Code, _labelUnit.Code, 1m / _labelCoeff.Value, calcUnitQtyFormAltUnit: true, showViewExample: true);
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
			_txtBlockLabelPrice.Text = GoodDocumentSimpleWindow.ConstructUnitsFormulas(_unit.Code, _labelUnit.Code, 1m / _labelCoeff.Value, calcUnitQtyFormAltUnit: true, showViewExample: true);
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
