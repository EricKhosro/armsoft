using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Extensions;
using ArmSoft.SME.Common.Models;
using ArmSoft.SME.Common.Models.Directories;
using ArmSoft.SME.Common.Models.SmartEnums;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Core.UI.Constants;
using ArmSoft.SME.Core.UI.Controls;
using ArmSoft.SME.Core.UI.DataViews;
using ArmSoft.SME.Core.UI.Directories;
using ArmSoft.SME.Core.UI.DropDowns;
using ArmSoft.SME.Core.UI.Enums;
using ArmSoft.SME.Trade.Common;
using ArmSoft.SME.Trade.Common.Models.Caches;
using ArmSoft.SME.Trade.Common.Models.Directories;
using ArmSoft.SME.Trade.Common.PersistableObject;
using ArmSoft.SME.Trade.Common.SmartEnums;
using ArmSoft.SME.Trade.UI.DataView;
using ArmSoft.SME.Trade.UI.DropDown;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.ComponentModel.__Internals;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Editors;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.LayoutControl;
using DevExpress.XtraEditors.DXErrorProvider;

namespace ArmSoft.SME.Trade.UI.Directories;

[DirectoryDocumentWindow(4)]
public class DiscountDocumentWindow : DirectoryDocumentWindow<Discount>
{
	public class DiscountScaleRow : DocumentGridRow
	{
		[ObservableProperty]
		private decimal beginAmount;

		[ObservableProperty]
		private decimal endAmount;

		[ObservableProperty]
		private decimal discountScalePercent;

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public decimal BeginAmount
		{
			get
			{
				return beginAmount;
			}
			set
			{
				if (!EqualityComparer<decimal>.Default.Equals(beginAmount, value))
				{
					OnPropertyChanging(__KnownINotifyPropertyChangingArgs.BeginAmount);
					beginAmount = value;
					OnPropertyChanged(__KnownINotifyPropertyChangedArgs.BeginAmount);
				}
			}
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public decimal EndAmount
		{
			get
			{
				return endAmount;
			}
			set
			{
				if (!EqualityComparer<decimal>.Default.Equals(endAmount, value))
				{
					OnPropertyChanging(__KnownINotifyPropertyChangingArgs.EndAmount);
					endAmount = value;
					OnPropertyChanged(__KnownINotifyPropertyChangedArgs.EndAmount);
				}
			}
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public decimal DiscountScalePercent
		{
			get
			{
				return discountScalePercent;
			}
			set
			{
				if (!EqualityComparer<decimal>.Default.Equals(discountScalePercent, value))
				{
					OnPropertyChanging(__KnownINotifyPropertyChangingArgs.DiscountScalePercent);
					discountScalePercent = value;
					OnPropertyChanged(__KnownINotifyPropertyChangedArgs.DiscountScalePercent);
				}
			}
		}

		static DiscountScaleRow()
		{
			DocumentGridRow.RegisterPropertyValidators(typeof(DiscountScaleRow), cacheValidationResults: false, new Dictionary<string, Action<DocumentGridRow, ErrorInfo>> { 
			{
				"Percent",
				(DocumentGridRow row, ErrorInfo info) =>
				{
					ValidatePercent((DiscountScaleRow)row, info);
				}
			} });
		}

		private static void ValidatePercent(DiscountScaleRow row, ErrorInfo info)
		{
			if (info.ErrorType != ErrorType.Critical && row.DiscountScalePercent > 100m)
			{
				info.ErrorType = ErrorType.Critical;
				info.ErrorText = ArmSoft.SME.Common.Resources.PercentLessHundred;
			}
		}
	}

	private TextEditExt _name;

	private DropDownControl _discountGroup;

	private DateEditControl _dateStart;

	private DateEditControl _dateEnd;

	private CheckEditExt _isActive;

	private LayoutItemWithRequired _priorityLayoutItem;

	private SpinEditExt _prioritySpinEdit;

	private DropDownControl _storage;

	private DropDownControl _docType;

	private DropDownControl _useType;

	private DropDownControl _giftGood;

	private PopupCalcEditExt _discountPercent;

	private PopupCalcEditExt _discountAmount;

	private PopupCalcEditExt _giftQuantity;

	private PopupCalcEditExt _giftDiscountPercent;

	private CheckEditExt _isDistrInProportion;

	private CheckEditExt _isMultiple;

	private CheckEdit _selectGift;

	private DropDownControl _discountPriceType;

	private MemoEditControl _message;

	private LayoutItemWithRequired _segmentLayoutItem;

	private DropDownControl _segmentDropDown;

	private ComboBoxExt _segmentNegation;

	private Button _openList;

	private LayoutGroup _segmentInformation;

	private DropDownControl _conditionType;

	private PopupCalcEditExt _conditionLimit;

	private DateEditControl _conditionDateStart;

	private DateEditControl _conditionDateEnd;

	private CheckEditExt _conditionIncludeCurrentSale;

	private DropDownControl _conditionPartnerGroup;

	private CheckEditExt _isBirthDayDiscount;

	private SpinEditExt _beforBDaySpinEdit;

	private SpinEditExt _afterBDaySpinEdit;

	private LayoutItemWithRequired _beforBDayLayoutItem;

	private LayoutItemWithRequired _afterBDayLayoutItem;

	private LayoutGroup _conditionDaysHoursLayoutGroup;

	private CheckEditExt _conditionDaysHoursCheckEdit;

	private Button _conditionDaysHoursButton;

	private TextBlock _conditionDaysHoursTextBlock;

	private Dictionary<int, Discount.ConditionDaysAndHours> _daysHoursConditionDictionary;

	private Dictionary<int, DaysHoursDiscountControls> _daysHoursControlsDictionary;

	private CheckEditExt _useExtenstion;

	private LayoutItemWithRequired _conditionSegmentLayoutItem;

	private DropDownControl _conditionSegmentDropDown;

	private ComboBoxExt _conditionSegmentNegation;

	private Button _conditionSegmentOpenList;

	private MemoEditControl _description;

	private LayoutGroup _informationLayoutGroup;

	private TextBlock _informationTextBlock;

	private TextBlock _segmentInfo;

	private LayoutGroup _conditionInformation;

	private TextBlock _conditionInfo;

	private LayoutGroup _accumulative;

	private DocumentGridControl _discountScaleGrid;

	private TableView _discountScaleTableView;

	private ObservableCollection<DiscountScaleRow> _discountScale;

	protected override double WidthValue => 780.0;

	protected override double HeightValue => 700.0;

	public DiscountDocumentWindow()
	{
	}

	public DiscountDocumentWindow(Discount doc)
		: base(doc)
	{
	}

	protected override void ConstructUI()
	{
		//IL_0391: Unknown result type (might be due to invalid IL or missing references)
		//IL_0397: Unknown result type (might be due to invalid IL or missing references)
		//IL_09b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_09b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_1082: Unknown result type (might be due to invalid IL or missing references)
		//IL_1088: Unknown result type (might be due to invalid IL or missing references)
		base.ConstructUI();
		LayoutGroupExt container = AddLayoutGroup(null, LayoutGroupView.Group, Orientation.Horizontal);
		_code = AddButtonEditControl(container, Definition.Fields["Code"], 1);
		_code.AllowMagicButton = true;
		_code.MagicToolTip = ArmSoft.SME.Common.Resources.CodeNextNumbers;
		_isActive = AddBooleanControl(container, Definition.Fields["IsActive"]);
		_name = AddStringControl(null, Definition.Fields["Name"], HorizontalAlignment.Stretch);
		_discountGroup = AddDropDownControl(null, Definition.Fields["DiscountGroupCode"], new ArmSoft.SME.Trade.UI.DropDown.DiscountGroups());
		LayoutGroupExt container2 = AddLayoutGroup(null, LayoutGroupView.Group, Orientation.Horizontal);
		_dateStart = AddDateControl(container2, Definition.Fields["StartDate"]);
		_dateEnd = AddDateControl(container2, Definition.Fields["EndDate"]);
		_dateEnd.Name = "Discount_Period_End";
		_prioritySpinEdit = new SpinEditExt
		{
			IsFloatValue = false,
			MinValue = 0m,
			MaxValue = 99m,
			IsReadOnly = false,
			MinWidth = 50.0,
			EditValue = 0,
			Value = 0m,
			Name = "Priority"
		};
		_priorityLayoutItem = AddLayoutItem(null, _prioritySpinEdit, HorizontalAlignment.Left);
		AddControl(_prioritySpinEdit, ArmSoft.SME.Trade.Common.Resources.Priority, _priorityLayoutItem);
		_docType = AddSmartEnumDropDownControl(null, Definition.Fields["DiscountDocType"]);
		_storage = AddDropDownControl(null, Definition.Fields["Storage"], new ArmSoft.SME.Trade.UI.DropDown.Storages());
		LayoutGroupExt layoutGroupExt = AddLayoutGroup(null, LayoutGroupView.GroupBox, Orientation.Vertical, ArmSoft.SME.Trade.Common.Resources.Application);
		layoutGroupExt.HorizontalAlignment = HorizontalAlignment.Stretch;
		_useType = AddDropDownControl(layoutGroupExt, Definition.Fields["UseType"], new SmartEnum(typeof(UsageMethodsSE), null, padLeft: false, '0', allSubTypes: false, ValueMember.Code, CaptionMember.Caption, null, null, orderByCode: false), allowAdditionButtons: false, fieldWithCaption: true, HorizontalAlignment.Stretch);
		_useType.EditValueChanged += new SafeEventHandler<EditValueChangedEventArgs>(UseType_EditValueChanged).Handler;
		_giftGood = AddDropDownControl(layoutGroupExt, Definition.Fields["GiftGoodCode"], new GoodsFilter(), allowAdditionButtons: false, fieldWithCaption: true, HorizontalAlignment.Stretch);
		_informationTextBlock = new TextBlock
		{
			Text = string.Empty,
			Foreground = ColorsAS.Foreground.BlueBrush,
			TextWrapping = TextWrapping.Wrap,
			FontWeight = FontWeights.Thin
		};
		DXImage element = new DXImage
		{
			Width = 16.0,
			Height = 16.0,
			Source = ImageService.GetImage("information_16.svg")
		};
		_informationLayoutGroup = AddLayoutGroup(layoutGroupExt, LayoutGroupView.Group, Orientation.Horizontal);
		_informationLayoutGroup.Height = 20.0;
		_informationLayoutGroup.Padding = new Thickness(12.0, 2.0, 0.0, 2.0);
		_informationLayoutGroup.Children.Add(element);
		_informationLayoutGroup.Children.Add(_informationTextBlock);
		_discountScaleGrid = AddGrid(layoutGroupExt, Definition.Grids["DiscountScale"], HorizontalAlignment.Stretch, VerticalAlignment.Top);
		_discountScaleTableView = _discountScaleGrid.View;
		_discountScaleGrid.View.Height = 120.0;
		_discountScaleGrid.AddNumberColumn(Definition.Grids["DiscountScale"].Columns["BeginAmount"]).Width = 200.0;
		_discountScaleGrid.AddNumberColumn(Definition.Grids["DiscountScale"].Columns["EndAmount"]).Width = 200.0;
		_discountScaleGrid.AddNumberColumn(Definition.Grids["DiscountScale"].Columns["DiscountScalePercent"]).Width = 200.0;
		_discountScale = new ObservableCollection<DiscountScaleRow>();
		_discountScaleGrid.AddingRow += new SafeEventHandler<DocumentGridAddingRowEventArgs>(DiscountScaleGrid_AddingRow).Handler;
		_discountScaleGrid.DuplicatingRow += new SafeEventHandler<DocumentGridDuplicatingRowEventArgs>(DiscountScaleGrid_DuplicatingRow).Handler;
		_discountPercent = AddNumericControl(layoutGroupExt, Definition.Fields["Percent"]);
		_discountAmount = AddNumericControl(layoutGroupExt, Definition.Fields["Amount"]);
		container2 = AddLayoutGroup(layoutGroupExt, LayoutGroupView.Group, Orientation.Horizontal);
		_giftQuantity = AddNumericControl(container2, Definition.Fields["GiftQuantity"]);
		_giftDiscountPercent = AddNumericControl(container2, Definition.Fields["GiftDiscountPercent"]);
		_giftDiscountPercent.Name = "GiftDiscountPercent";
		_selectGift = AddBooleanControl(layoutGroupExt, Definition.Fields["SelectGift"]);
		_isDistrInProportion = AddBooleanControl(layoutGroupExt, Definition.Fields["IsDistrInProportion"]);
		_isMultiple = AddBooleanControl(layoutGroupExt, Definition.Fields["IsMultiple"]);
		_discountPriceType = AddDropDownControl(layoutGroupExt, Definition.Fields["PriceType"], new ArmSoft.SME.Trade.UI.DropDown.PriceTypes());
		_message = AddMemoControl(layoutGroupExt, Definition.Fields["Message"]);
		DataTemplate dataTemplate = new DataTemplate(typeof(CodeName));
		FrameworkElementFactory frameworkElementFactory = new FrameworkElementFactory(typeof(StackPanel));
		frameworkElementFactory.SetValue(StackPanel.OrientationProperty, Orientation.Vertical);
		FrameworkElementFactory frameworkElementFactory2 = new FrameworkElementFactory(typeof(TextBlock));
		frameworkElementFactory2.SetBinding(TextBlock.TextProperty, new Binding("Name"));
		frameworkElementFactory2.SetBinding(FrameworkElement.ToolTipProperty, new Binding("Name"));
		frameworkElementFactory.AppendChild(frameworkElementFactory2);
		dataTemplate.VisualTree = frameworkElementFactory;
		List<CodeName> itemsSource = new List<CodeName>
		{
			new CodeName
			{
				Code = "0",
				Name = ArmSoft.SME.Trade.Common.Resources.Include
			},
			new CodeName
			{
				Code = "1",
				Name = ArmSoft.SME.Trade.Common.Resources.Exclude
			}
		};
		container2 = AddLayoutGroup(layoutGroupExt, LayoutGroupView.Group, Orientation.Horizontal);
		_segmentNegation = new ComboBoxExt
		{
			ItemsSource = itemsSource,
			Width = 90.0,
			ItemTemplate = dataTemplate,
			Name = "Segment_Negation"
		};
		_segmentLayoutItem = AddLayoutItem(container2, _segmentNegation, HorizontalAlignment.Left);
		AddControl(_segmentNegation, ArmSoft.SME.Trade.Common.Resources.Segment, _segmentLayoutItem);
		_segmentNegation.SelectionChanged += new SafeEventHandler<SelectionChangedEventArgs>(SegmentNegation_SelectionChanged).Handler;
		_segmentDropDown = AddDropDownControl(container2, Definition.Fields["Segment"], new ArmSoft.SME.Trade.UI.DropDown.Segments(), allowAdditionButtons: false, fieldWithCaption: false);
		_segmentDropDown.Name = "Segment_Application";
		_openList = AddButton(container2, null, ArmSoft.SME.Trade.Common.Resources.OpenList, null, ArmSoft.SME.Trade.Common.Resources.OpenList);
		_openList.Click += new SafeEventHandler<RoutedEventArgs>(OpenList_Click).Handler;
		_segmentInfo = new TextBlock
		{
			Text = ArmSoft.SME.Trade.Common.Resources.SegmentInformation,
			Foreground = ColorsAS.Foreground.BlueBrush,
			TextWrapping = TextWrapping.Wrap,
			FontWeight = FontWeights.Thin
		};
		element = new DXImage
		{
			Width = 16.0,
			Height = 16.0,
			Source = ImageService.GetImage("information_16.svg")
		};
		_segmentInformation = AddLayoutGroup(layoutGroupExt, LayoutGroupView.Group, Orientation.Horizontal);
		_segmentInformation.MinHeight = 16.0;
		_segmentInformation.Height = _segmentInformation.MinHeight;
		_segmentInformation.Padding = new Thickness(12.0, 0.0, 12.0, 28.0);
		_segmentInformation.Children.Add(element);
		_segmentInformation.Children.Add(_segmentInfo);
		LayoutGroupExt layoutGroupExt2 = AddLayoutGroup(null, LayoutGroupView.GroupBox, Orientation.Vertical, ArmSoft.SME.Trade.Common.Resources.Conditions);
		layoutGroupExt2.HorizontalAlignment = HorizontalAlignment.Stretch;
		_conditionType = AddSmartEnumDropDownControl(layoutGroupExt2, ((ConditionDefinition)Definition.Fields["DiscountCondition"]).Type);
		_conditionType.EditValueChanged += new SafeEventHandler<EditValueChangedEventArgs>(ConditionType_EditValueChanged).Handler;
		_conditionLimit = AddNumericControl(layoutGroupExt2, ((ConditionDefinition)Definition.Fields["DiscountCondition"]).Limit);
		_accumulative = AddLayoutGroup(layoutGroupExt2, LayoutGroupView.Group, Orientation.Vertical);
		container2 = AddLayoutGroup(_accumulative, LayoutGroupView.Group, Orientation.Horizontal);
		_conditionDateStart = AddDateControl(container2, ((ConditionDefinition)Definition.Fields["DiscountCondition"]).ConditionStart);
		_conditionDateEnd = AddDateControl(container2, ((ConditionDefinition)Definition.Fields["DiscountCondition"]).ConditionEnd);
		_conditionDateEnd.Name = "DiscountCondition_Period_End";
		_conditionIncludeCurrentSale = AddBooleanControl(_accumulative, ((ConditionDefinition)Definition.Fields["DiscountCondition"]).IncludeCurrentSale);
		container2 = AddLayoutGroup(layoutGroupExt2, LayoutGroupView.Group, Orientation.Horizontal);
		_conditionSegmentNegation = new ComboBoxExt
		{
			ItemsSource = itemsSource,
			Width = 90.0,
			ItemTemplate = dataTemplate,
			Name = "Condition_Segment_Negation"
		};
		_conditionSegmentLayoutItem = AddLayoutItem(container2, _conditionSegmentNegation, HorizontalAlignment.Left);
		AddControl(_conditionSegmentNegation, ArmSoft.SME.Trade.Common.Resources.Segment, _conditionSegmentLayoutItem);
		_conditionSegmentDropDown = AddDropDownControl(container2, ((ConditionDefinition)Definition.Fields["DiscountCondition"]).Segment, new ArmSoft.SME.Trade.UI.DropDown.Segments(), allowAdditionButtons: false, fieldWithCaption: false);
		_conditionSegmentDropDown.Name = "Segment_Condition";
		_conditionSegmentOpenList = AddButton(container2, null, ArmSoft.SME.Trade.Common.Resources.OpenList, null, ArmSoft.SME.Trade.Common.Resources.OpenList);
		_conditionSegmentOpenList.Click += new SafeEventHandler<RoutedEventArgs>(ConditionSegmentOpenList_Click).Handler;
		_conditionPartnerGroup = AddDropDownControl(layoutGroupExt2, ((ConditionDefinition)Definition.Fields["DiscountCondition"]).PartnerGroup, new PartnerGroups());
		_isBirthDayDiscount = AddBooleanControl(layoutGroupExt2, ((ConditionDefinition)Definition.Fields["DiscountCondition"]).IsBirthDayDiscount);
		_isBirthDayDiscount.EditValueChanged += new SafeEventHandler<EditValueChangedEventArgs>(IsBirthDayDiscount_EditValueChanged).Handler;
		_beforBDaySpinEdit = new SpinEditExt
		{
			IsFloatValue = false,
			MinValue = 0m,
			MaxValue = 99m,
			IsReadOnly = true,
			MinWidth = 50.0,
			EditValue = 0,
			Foreground = Brushes.Black,
			Name = "Befor_Birthday_Days"
		};
		_beforBDayLayoutItem = AddLayoutItem(layoutGroupExt2, _beforBDaySpinEdit, HorizontalAlignment.Left);
		AddControl(_beforBDaySpinEdit, ArmSoft.SME.Trade.Common.Resources.BeforBDay, _beforBDayLayoutItem);
		_afterBDaySpinEdit = new SpinEditExt
		{
			IsFloatValue = false,
			MinValue = 0m,
			MaxValue = 99m,
			IsReadOnly = true,
			MinWidth = 50.0,
			EditValue = 0,
			Foreground = Brushes.Black,
			Name = "After_Birthday_Days"
		};
		_afterBDayLayoutItem = AddLayoutItem(layoutGroupExt2, _afterBDaySpinEdit, HorizontalAlignment.Left);
		AddControl(_afterBDaySpinEdit, ArmSoft.SME.Trade.Common.Resources.AfterBDay, _afterBDayLayoutItem);
		_beforBDayLayoutItem.Visibility = Visibility.Collapsed;
		_afterBDayLayoutItem.Visibility = Visibility.Collapsed;
		_daysHoursConditionDictionary = new Dictionary<int, Discount.ConditionDaysAndHours>();
		_daysHoursControlsDictionary = new Dictionary<int, DaysHoursDiscountControls>();
		_conditionDaysHoursLayoutGroup = AddLayoutGroup(layoutGroupExt2, LayoutGroupView.Group, Orientation.Horizontal);
		_conditionDaysHoursCheckEdit = AddBooleanControl(_conditionDaysHoursLayoutGroup, Definition.Fields["UseDayHours"]);
		_conditionDaysHoursCheckEdit.EditValueChanged += new SafeEventHandler<EditValueChangedEventArgs>(ConditionDaysHours_EditValueChanged).Handler;
		_conditionDaysHoursButton = AddButton(_conditionDaysHoursLayoutGroup, null, "...", null, ArmSoft.SME.Trade.Common.Resources.DiscountDaysHours);
		_conditionDaysHoursButton.Click += new SafeEventHandler<RoutedEventArgs>(ConditionDaysHours_Click).Handler;
		_conditionDaysHoursButton.IsTabStop = true;
		_conditionDaysHoursTextBlock = new TextBlock
		{
			Text = string.Empty,
			FontWeight = FontWeights.Thin,
			VerticalAlignment = VerticalAlignment.Center,
			TextTrimming = TextTrimming.WordEllipsis
		};
		_conditionDaysHoursLayoutGroup.Children.Add(_conditionDaysHoursTextBlock);
		_conditionInfo = new TextBlock
		{
			Text = string.Empty,
			Foreground = ColorsAS.Foreground.BlueBrush,
			TextWrapping = TextWrapping.Wrap,
			FontWeight = FontWeights.Thin
		};
		element = new DXImage
		{
			Width = 16.0,
			Height = 16.0,
			Source = ImageService.GetImage("information_16.svg")
		};
		_conditionInformation = AddLayoutGroup(layoutGroupExt2, LayoutGroupView.Group, Orientation.Horizontal);
		_conditionInformation.Height = 20.0;
		_conditionInformation.Padding = new Thickness(12.0, 2.0, 0.0, 2.0);
		_conditionInformation.Children.Add(element);
		_conditionInformation.Children.Add(_conditionInfo);
		LayoutGroupExt container3 = AddLayoutGroup(layoutGroupExt2, LayoutGroupView.Group, Orientation.Horizontal);
		_useExtenstion = AddBooleanControl(container3, ((ConditionDefinition)Definition.Fields["DiscountCondition"]).UseExtension);
		_useExtenstion.EditValueChanged += new SafeEventHandler<EditValueChangedEventArgs>(UseExtension_EditValueChanged).Handler;
		_description = AddMemoControl(null, Definition.Fields["Description"], HorizontalAlignment.Stretch);
	}

	protected override void FillDocument(DirStoreRequest sr)
	{
		base.FillDocument(sr);
		PO.Priority = (byte)_prioritySpinEdit.Value;
		PO.Segment = _segmentDropDown.Code;
		PO.SegmentNegation = _segmentNegation.SelectedIndex == Convert.ToInt32("1");
		if (!string.IsNullOrWhiteSpace(_giftGood.Code))
		{
			MaterialCacheRow item = TradeSettings.TradeCacheService.Material.GetItem(_giftGood.Code);
			if (item != null)
			{
				PO.GiftGoodID = item.Id;
			}
		}
		PO.DiscountScale.Clear();
		PO.Percent = 0m;
		PO.Amount = 0m;
		PO.GiftQuantity = 0m;
		PO.IsMultiple = false;
		PO.PriceType = string.Empty;
		PO.Message = string.Empty;
		PO.UseDayHours = Convert.ToBoolean(_conditionDaysHoursCheckEdit.IsChecked);
		switch (PO.UseType)
		{
		case "2":
			foreach (DiscountScaleRow item2 in _discountScale)
			{
				PO.DiscountScale.Add(new Discount.DiscountScaleRow
				{
					BeginAmount = item2.BeginAmount,
					EndAmount = item2.EndAmount,
					DiscountScalePercent = item2.DiscountScalePercent
				});
			}
			break;
		case "4":
			PO.PriceType = _discountPriceType.Code;
			break;
		case "5":
			PO.Percent = _discountPercent.Value;
			break;
		case "6":
			PO.Message = _message.Text;
			break;
		case "7":
			PO.Amount = _discountAmount.Value;
			PO.IsMultiple = _isMultiple.IsChecked.Value;
			break;
		case "8":
			PO.GiftQuantity = _giftQuantity.Value;
			PO.GiftDiscountPercent = _giftDiscountPercent.Value;
			PO.IsDistrInProportion = _isDistrInProportion.IsChecked.Value;
			PO.IsMultiple = _isMultiple.IsChecked.Value;
			break;
		case "9":
			PO.GiftQuantity = _giftQuantity.Value;
			PO.IsMultiple = _isMultiple.IsChecked.Value;
			break;
		case "10":
			PO.GiftQuantity = 1m;
			PO.IsMultiple = _isMultiple.IsChecked.Value;
			PO.SelectGift = _selectGift.IsChecked.Value;
			break;
		}
		PO.DiscountCondition = new Discount.Condition();
		PO.DiscountCondition.Type = _conditionType.Code;
		PO.DiscountCondition.Limit = _conditionLimit.Value;
		if (PO.DiscountCondition.Type == DiscountConditionTypesSE.AccumulativeSaleSumma)
		{
			PO.DiscountCondition.ConditionStart = _conditionDateStart.EditValue;
			PO.DiscountCondition.ConditionEnd = _conditionDateEnd.EditValue;
			PO.DiscountCondition.IncludeCurrentSale = _conditionIncludeCurrentSale.IsChecked.Value;
		}
		PO.DiscountCondition.PartnerGroup = _conditionPartnerGroup.Code;
		PO.DiscountCondition.IsBirthDayDiscount = _isBirthDayDiscount.IsChecked.Value;
		PO.DiscountCondition.BeforBDay = (byte)_beforBDaySpinEdit.Value;
		PO.DiscountCondition.AfterBDay = (byte)_afterBDaySpinEdit.Value;
		PO.DiscountCondition.Segment = _conditionSegmentDropDown.Code;
		PO.DiscountCondition.SegmentNegation = _conditionSegmentNegation.SelectedIndex == Convert.ToInt32("1");
		PO.DiscountCondition.DaysHours = _daysHoursConditionDictionary;
		PO.DiscountCondition.UseExtension = _useExtenstion.IsChecked.Value;
		PO.DiscountCondition.DaysHours = _daysHoursConditionDictionary;
	}

	protected override void LoadDataToControls()
	{
		base.LoadDataToControls();
		_prioritySpinEdit.Value = PO.Priority;
		_discountScale.Clear();
		foreach (Discount.DiscountScaleRow item in PO.DiscountScale)
		{
			_discountScale.Add(new DiscountScaleRow
			{
				BeginAmount = item.BeginAmount,
				EndAmount = item.EndAmount,
				DiscountScalePercent = item.DiscountScalePercent
			});
		}
		_discountScaleGrid.ItemsSource = _discountScale;
		if (PO.SegmentNegation)
		{
			_segmentNegation.SelectedIndex = int.Parse("1");
		}
		else
		{
			_segmentNegation.SelectedIndex = int.Parse("0");
		}
		_conditionType.Code = PO.DiscountCondition.Type;
		_conditionLimit.EditValue = PO.DiscountCondition.Limit;
		_conditionDateStart.EditValue = PO.DiscountCondition.ConditionStart;
		_conditionDateEnd.EditValue = PO.DiscountCondition.ConditionEnd;
		_conditionIncludeCurrentSale.EditValue = PO.DiscountCondition.IncludeCurrentSale;
		_conditionPartnerGroup.Code = PO.DiscountCondition.PartnerGroup;
		_conditionSegmentDropDown.Code = PO.DiscountCondition.Segment;
		if (PO.DiscountCondition.SegmentNegation)
		{
			_conditionSegmentNegation.SelectedIndex = int.Parse("1");
		}
		else
		{
			_conditionSegmentNegation.SelectedIndex = int.Parse("0");
		}
		_isBirthDayDiscount.IsChecked = PO.DiscountCondition.IsBirthDayDiscount;
		_beforBDaySpinEdit.Value = PO.DiscountCondition.BeforBDay;
		_afterBDaySpinEdit.Value = PO.DiscountCondition.AfterBDay;
		if (PO.DiscountCondition.DaysHours != null)
		{
			_daysHoursConditionDictionary = PO.DiscountCondition.DaysHours;
			ConditionDaysHours_Change();
		}
		_useExtenstion.IsChecked = PO.DiscountCondition.UseExtension;
	}

	protected override void OnNew()
	{
		base.OnNew();
		_dateStart.EditValue = Settings.HostingEnvironment.CurrentDate;
		if (_viewMode != WindowViewMode.Copy)
		{
			_useType.Code = UsageMethodsSE.GoodDiscountPercent.Value;
			_conditionType.Code = DiscountConditionTypesSE.OneTimeSaleSumma.Value;
		}
	}

	protected override void OnNewAfterSave()
	{
		base.OnNewAfterSave();
		_name.EditValue = "";
		_discountGroup.Code = "";
		_dateStart.EditValue = Settings.HostingEnvironment.CurrentDate;
		_dateEnd.EditValue = null;
		_isActive.IsChecked = true;
		_prioritySpinEdit.EditValue = 0m;
		_storage.Code = "";
		_docType.Code = "";
		_useType.Code = UsageMethodsSE.GoodDiscountPercent.Value;
		_giftGood.Code = "";
		_discountPercent.EditValue = 0m;
		_discountAmount.EditValue = 0m;
		_giftQuantity.EditValue = 0m;
		_giftDiscountPercent.EditValue = 100m;
		_isDistrInProportion.IsChecked = false;
		_isMultiple.IsChecked = false;
		_selectGift.IsChecked = false;
		_discountPriceType.Code = "";
		_message.Text = "";
		_segmentDropDown.Code = "";
		_segmentNegation.SelectedIndex = Convert.ToInt32("0");
		_discountScale.Clear();
		_discountScaleGrid.Grid.RefreshData();
		_conditionType.Code = DiscountConditionTypesSE.OneTimeSaleSumma.Value;
		_conditionLimit.EditValue = 0m;
		_conditionDateStart.EditValue = default(DateOnly);
		_conditionDateEnd.EditValue = default(DateOnly);
		_conditionIncludeCurrentSale.IsChecked = false;
		_conditionPartnerGroup.Code = "";
		_conditionSegmentDropDown.Code = "";
		_conditionSegmentNegation.SelectedIndex = Convert.ToInt32("0");
		_isBirthDayDiscount.IsChecked = false;
		_beforBDaySpinEdit.EditValue = 0m;
		_afterBDaySpinEdit.EditValue = 0m;
		_description.Text = "";
	}

	protected override void OnEdit()
	{
		base.OnEdit();
		_setFocusOnActivation = _name;
	}

	protected override void SetControlsReadonly()
	{
		base.SetControlsReadonly();
		if (_viewMode != WindowViewMode.View)
		{
			bool flag = State == DocumentStateSE.Draft.Value || State == DocumentStateSE.Deleted.Value;
			bool isReadOnly = _viewMode == WindowViewMode.Edit && !flag;
			_code.IsReadOnly = isReadOnly;
		}
	}

	protected override void UpdateDataViewPanel(DataViewPanel dvp)
	{
		base.UpdateDataViewPanel(dvp);
		dvp.Update(_code.EditText);
	}

	private void DiscountScaleGrid_AddingRow(object sender, DocumentGridAddingRowEventArgs e)
	{
		DiscountScaleRow discountScaleRow = new DiscountScaleRow();
		if (_discountScale.Count > 0)
		{
			discountScaleRow.BeginAmount = _discountScale[_discountScale.Count - 1].EndAmount;
		}
		_discountScale.Add(discountScaleRow);
		e.RowNumber = _discountScale.Count - 1;
	}

	private void DiscountScaleGrid_DuplicatingRow(object sender, DocumentGridDuplicatingRowEventArgs e)
	{
		DiscountScaleRow discountScaleRow = (DiscountScaleRow)_discountScaleGrid.Grid.GetRow(e.RowHandle);
		if (discountScaleRow != null)
		{
			DiscountScaleRow discountScaleRow2 = new DiscountScaleRow();
			discountScaleRow.Clone(discountScaleRow2);
			_discountScale.Add(discountScaleRow2);
			e.NewRowNumber = _discountScale.Count - 1;
		}
	}

	private void UseType_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		_informationLayoutGroup.Visibility = Visibility.Collapsed;
		_discountScaleGrid.Visibility = Visibility.Collapsed;
		_discountScaleGrid.IsRequired = false;
		((LayoutItemWithRequired)(object)_discountPercent.Parent).Visibility = Visibility.Collapsed;
		_discountPercent.IsRequired = false;
		((LayoutItemWithRequired)(object)_discountAmount.Parent).Visibility = Visibility.Collapsed;
		_discountAmount.IsRequired = false;
		((LayoutItemWithRequired)(object)_giftQuantity.Parent).Visibility = Visibility.Collapsed;
		_giftQuantity.IsRequired = false;
		((LayoutItemWithRequired)(object)_giftDiscountPercent.Parent).Visibility = Visibility.Collapsed;
		_giftDiscountPercent.IsRequired = false;
		((LayoutItemWithRequired)(object)_isDistrInProportion.Parent).Visibility = Visibility.Collapsed;
		((LayoutItemWithRequired)(object)_isMultiple.Parent).Visibility = Visibility.Collapsed;
		((LayoutItemWithRequired)(object)_selectGift.Parent).Visibility = Visibility.Collapsed;
		((LayoutItemWithRequired)(object)_discountPriceType.Parent).Visibility = Visibility.Collapsed;
		_discountPriceType.IsRequired = false;
		((LayoutItemWithRequired)(object)_message.Parent).Visibility = Visibility.Collapsed;
		_message.IsRequired = false;
		_segmentLayoutItem.Visibility = Visibility.Visible;
		((LayoutItemWithRequired)(object)_segmentDropDown.Parent).Visibility = Visibility.Visible;
		((LayoutItemWithRequired)(object)_openList.Parent).Visibility = Visibility.Visible;
		_segmentInformation.Visibility = Visibility.Visible;
		((LayoutItemWithRequired)(object)_giftGood.Parent).Visibility = Visibility.Collapsed;
		_giftGood.IsRequired = false;
		string code = _useType.Code;
		if (code == null)
		{
			return;
		}
		switch (code.Length)
		{
		case 1:
			switch (code[0])
			{
			case '1':
				_informationLayoutGroup.Visibility = Visibility.Visible;
				_informationTextBlock.Text = ArmSoft.SME.Trade.Common.Resources.GoodDiscountPercentInformation;
				break;
			case '2':
				_informationLayoutGroup.Visibility = Visibility.Visible;
				_informationTextBlock.Text = ArmSoft.SME.Trade.Common.Resources.ScaleInformation;
				_discountScaleGrid.Visibility = Visibility.Visible;
				_discountScaleGrid.IsRequired = true;
				break;
			case '3':
				_informationLayoutGroup.Visibility = Visibility.Visible;
				_informationTextBlock.Text = ArmSoft.SME.Trade.Common.Resources.PartnerCardInformation;
				break;
			case '4':
				((LayoutItemWithRequired)(object)_discountPriceType.Parent).Visibility = Visibility.Visible;
				_discountPriceType.IsRequired = true;
				break;
			case '5':
				((LayoutItemWithRequired)(object)_discountPercent.Parent).Visibility = Visibility.Visible;
				_discountPercent.IsRequired = true;
				break;
			case '6':
				((LayoutItemWithRequired)(object)_message.Parent).Visibility = Visibility.Visible;
				_message.IsRequired = true;
				_segmentLayoutItem.Visibility = Visibility.Collapsed;
				((LayoutItemWithRequired)(object)_segmentDropDown.Parent).Visibility = Visibility.Collapsed;
				((LayoutItemWithRequired)(object)_openList.Parent).Visibility = Visibility.Collapsed;
				_segmentInformation.Visibility = Visibility.Collapsed;
				break;
			case '7':
				((LayoutItemWithRequired)(object)_discountAmount.Parent).Visibility = Visibility.Visible;
				_discountAmount.IsRequired = true;
				((LayoutItemWithRequired)(object)_isMultiple.Parent).Visibility = Visibility.Visible;
				break;
			case '8':
				((LayoutItemWithRequired)(object)_giftQuantity.Parent).Visibility = Visibility.Visible;
				_giftQuantity.IsRequired = true;
				((LayoutItemWithRequired)(object)_giftDiscountPercent.Parent).Visibility = Visibility.Visible;
				_giftDiscountPercent.IsRequired = true;
				((LayoutItemWithRequired)(object)_isDistrInProportion.Parent).Visibility = Visibility.Visible;
				((LayoutItemWithRequired)(object)_isMultiple.Parent).Visibility = Visibility.Visible;
				break;
			case '9':
				((LayoutItemWithRequired)(object)_giftQuantity.Parent).Visibility = Visibility.Visible;
				_giftQuantity.IsRequired = true;
				((LayoutItemWithRequired)(object)_giftGood.Parent).Visibility = Visibility.Visible;
				_giftGood.DataView = new GoodsFilter();
				_giftGood.IsRequired = true;
				((LayoutItemWithRequired)(object)_isMultiple.Parent).Visibility = Visibility.Visible;
				break;
			}
			break;
		case 2:
			if (code == "10")
			{
				((LayoutItemWithRequired)(object)_giftGood.Parent).Visibility = Visibility.Visible;
				_giftGood.Code = "";
				_giftGood.DataView = new Complects();
				_giftGood.IsRequired = true;
				_giftQuantity.Value = 1m;
				((LayoutItemWithRequired)(object)_isMultiple.Parent).Visibility = Visibility.Visible;
				((LayoutItemWithRequired)(object)_selectGift.Parent).Visibility = Visibility.Visible;
			}
			break;
		}
	}

	private void ConditionType_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		_conditionLimit.EditValue = 0m;
		_conditionInformation.Visibility = Visibility.Collapsed;
		switch (_conditionType.Code)
		{
		case "1":
			_accumulative.Visibility = Visibility.Collapsed;
			ChangeConditionLimitMask(16, 2);
			break;
		case "2":
			_accumulative.Visibility = Visibility.Visible;
			ChangeConditionLimitMask(16, 2);
			_conditionInformation.Visibility = Visibility.Visible;
			_conditionInfo.Text = ArmSoft.SME.Trade.Common.Resources.DiscountAccumulativeCondition;
			break;
		case "4":
		case "3":
			_accumulative.Visibility = Visibility.Collapsed;
			ChangeConditionLimitMask(13, 3);
			break;
		}
		_isMultiple.IsReadOnly = _conditionType.Code == DiscountConditionTypesSE.AccumulativeSaleSumma.Value || _conditionType.Code == DiscountConditionTypesSE.OneTimeSaleRowQuantity.Value;
		if (_isMultiple.IsReadOnly)
		{
			_isMultiple.IsChecked = false;
		}
		if (_selectGift.IsReadOnly)
		{
			_selectGift.IsChecked = false;
		}
	}

	private void ChangeConditionLimitMask(short length, short precision)
	{
		_conditionLimit.Mask = DecimalExtension.GetCurrencyFormat(length, precision);
		_conditionLimit.Precision = precision;
	}

	private void OpenList_Click(object sender, RoutedEventArgs e)
	{
		if (!string.IsNullOrWhiteSpace(_segmentDropDown.Code))
		{
			GoodsBySegments goodsBySegments = new GoodsBySegments();
			goodsBySegments.SegmentCode = _segmentDropDown.Code;
			goodsBySegments.Show(useParamValues: false, openAsChild: false, null, withoutShowDialog: true);
		}
	}

	private void ConditionSegmentOpenList_Click(object sender, RoutedEventArgs e)
	{
		if (!string.IsNullOrWhiteSpace(_conditionSegmentDropDown.Code))
		{
			GoodsBySegments goodsBySegments = new GoodsBySegments();
			goodsBySegments.SegmentCode = _conditionSegmentDropDown.Code;
			goodsBySegments.Show(useParamValues: false, openAsChild: false, null, withoutShowDialog: true);
		}
	}

	private void SegmentNegation_SelectionChanged(object sender, SelectionChangedEventArgs e)
	{
		if (_segmentNegation.SelectedIndex == Convert.ToInt32("1"))
		{
			_segmentInfo.Text = ArmSoft.SME.Trade.Common.Resources.SegmentNegationInformation;
		}
		else
		{
			_segmentInfo.Text = ArmSoft.SME.Trade.Common.Resources.SegmentInformation;
		}
	}

	private void IsBirthDayDiscount_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		if (_isBirthDayDiscount.IsChecked.Value)
		{
			_beforBDayLayoutItem.Visibility = Visibility.Visible;
			_afterBDayLayoutItem.Visibility = Visibility.Visible;
			_beforBDayLayoutItem.IsEnabled = true;
			_afterBDayLayoutItem.IsEnabled = true;
			_beforBDaySpinEdit.IsReadOnly = false;
			_afterBDaySpinEdit.IsReadOnly = false;
		}
		else
		{
			_beforBDayLayoutItem.Visibility = Visibility.Collapsed;
			_afterBDayLayoutItem.Visibility = Visibility.Collapsed;
			_beforBDaySpinEdit.IsReadOnly = true;
			_afterBDaySpinEdit.IsReadOnly = true;
		}
	}

	private List<short> GetWeekDays()
	{
		int num = 7;
		List<short> list = new List<short>(num);
		CollectionsMarshal.SetCount(list, num);
		Span<short> span = CollectionsMarshal.AsSpan(list);
		span[0] = 1;
		span[1] = 2;
		span[2] = 3;
		span[3] = 4;
		span[4] = 5;
		span[5] = 6;
		span[6] = 7;
		return list;
	}

	private void ConditionDaysHours_Click(object sender, RoutedEventArgs e)
	{
		DialogWindow dialogWindow = new DialogWindow();
		dialogWindow.Name = "DiscountDaysAndHours";
		dialogWindow.Caption = ArmSoft.SME.Trade.Common.Resources.DiscountDaysHours;
		dialogWindow.Owner = this;
		dialogWindow.WindowValidation += ParametersDialogValidation;
		_daysHoursControlsDictionary.Clear();
		foreach (short weekDay in GetWeekDays())
		{
			DaysHoursDiscountControls daysHoursDiscountControls = new DaysHoursDiscountControls();
			daysHoursDiscountControls.Lg = dialogWindow.AddHorizontalLayoutGroup();
			daysHoursDiscountControls.Active = dialogWindow.AddBooleanControl(daysHoursDiscountControls.Lg, $"DayOfWeek{weekDay}", DateExtension.DayOfWeekName(weekDay));
			daysHoursDiscountControls.FirstStartTime = dialogWindow.AddTimeControl(daysHoursDiscountControls.Lg, $"FirstStartTime{weekDay}", "");
			daysHoursDiscountControls.FirstEndTime = dialogWindow.AddTimeControl(daysHoursDiscountControls.Lg, $"FirstEndTime{weekDay}", "");
			daysHoursDiscountControls.SecondStartTime = dialogWindow.AddTimeControl(daysHoursDiscountControls.Lg, $"SecondStartTime{weekDay}", "");
			daysHoursDiscountControls.SecondEndTime = dialogWindow.AddTimeControl(daysHoursDiscountControls.Lg, $"SecondEndTime{weekDay}", "");
			_daysHoursControlsDictionary.Add(weekDay, daysHoursDiscountControls);
		}
		foreach (KeyValuePair<int, DaysHoursDiscountControls> item in _daysHoursControlsDictionary)
		{
			item.Value.Active.EditValueChanged += new SafeEventHandler<EditValueChangedEventArgs>(Active_Edit).Handler;
			item.Value.Active.Margin = new Thickness(0.0, 0.0, 20.0, 0.0);
			item.Value.FirstEndTime.Margin = new Thickness(0.0, 0.0, 30.0, 0.0);
			item.Value.Active.IsReadOnly = _isReadOnly;
			if (item.Value.Active.IsChecked.Value && !_isReadOnly)
			{
				item.Value.FirstStartTime.IsReadOnly = false;
				item.Value.FirstEndTime.IsReadOnly = false;
				item.Value.SecondStartTime.IsReadOnly = false;
				item.Value.SecondEndTime.IsReadOnly = false;
			}
			else
			{
				item.Value.FirstStartTime.IsReadOnly = true;
				item.Value.FirstEndTime.IsReadOnly = true;
				item.Value.SecondStartTime.IsReadOnly = true;
				item.Value.SecondEndTime.IsReadOnly = true;
			}
		}
		ConvertDaysHoursDictionaryToControl(ref _daysHoursControlsDictionary, _daysHoursConditionDictionary);
		if (dialogWindow.ShowDialog().Value)
		{
			ConvertDaysHoursControlToDictionary(_daysHoursControlsDictionary, ref _daysHoursConditionDictionary);
			IsChanged = true;
			ConditionDaysHours_Change();
		}
	}

	private void ConvertDaysHoursDictionaryToControl(ref Dictionary<int, DaysHoursDiscountControls> controlDict, Dictionary<int, Discount.ConditionDaysAndHours> source)
	{
		foreach (KeyValuePair<int, Discount.ConditionDaysAndHours> item in source)
		{
			if (!controlDict.TryGetValue(item.Key, out var value))
			{
				value = new DaysHoursDiscountControls();
				controlDict.Add(item.Key, value);
			}
			value.Active.EditValue = item.Value.Active;
			value.FirstStartTime.EditValue = item.Value.FirstStartTime.ToString();
			value.FirstEndTime.EditValue = item.Value.FirstEndTime.ToString();
			value.SecondStartTime.EditValue = item.Value.SecondStartTime.ToString();
			value.SecondEndTime.EditValue = item.Value.SecondEndTime.ToString();
		}
	}

	private void ConvertDaysHoursControlToDictionary(Dictionary<int, DaysHoursDiscountControls> controlDict, ref Dictionary<int, Discount.ConditionDaysAndHours> source)
	{
		foreach (KeyValuePair<int, DaysHoursDiscountControls> item in controlDict)
		{
			if (!source.TryGetValue(item.Key, out var value))
			{
				value = new Discount.ConditionDaysAndHours();
				source.Add(item.Key, value);
			}
			value.Active = Convert.ToBoolean(item.Value.Active.IsChecked);
			value.FirstStartTime = item.Value.FirstStartTime.TimeSpanValue;
			value.FirstEndTime = item.Value.FirstEndTime.TimeSpanValue;
			value.SecondStartTime = item.Value.SecondStartTime.TimeSpanValue;
			value.SecondEndTime = item.Value.SecondEndTime.TimeSpanValue;
		}
	}

	private void Active_Edit(object sender, EditValueChangedEventArgs e)
	{
		foreach (KeyValuePair<int, DaysHoursDiscountControls> item in _daysHoursControlsDictionary)
		{
			if (item.Value.Active.IsChecked.Value && !_isReadOnly)
			{
				item.Value.FirstStartTime.IsReadOnly = false;
				item.Value.FirstEndTime.IsReadOnly = false;
				item.Value.SecondStartTime.IsReadOnly = false;
				item.Value.SecondEndTime.IsReadOnly = false;
			}
			else
			{
				item.Value.FirstStartTime.IsReadOnly = true;
				item.Value.FirstEndTime.IsReadOnly = true;
				item.Value.SecondStartTime.IsReadOnly = true;
				item.Value.SecondEndTime.IsReadOnly = true;
			}
		}
	}

	private void ConditionDaysHours_Change()
	{
		_conditionDaysHoursTextBlock.Text = string.Empty;
		if (!_conditionDaysHoursCheckEdit.IsChecked.Value || _daysHoursConditionDictionary == null)
		{
			return;
		}
		string text = string.Empty;
		foreach (KeyValuePair<int, Discount.ConditionDaysAndHours> item in _daysHoursConditionDictionary)
		{
			string text2 = string.Empty;
			if (item.Value.Active)
			{
				text = " " + DateExtension.DayOfWeekName(item.Key, shortMode: true);
				if (item.Value.FirstStartTime.HasValue && item.Value.FirstStartTime.ToString() != "  :  ")
				{
					text2 = text2 + " (" + item.Value.FirstStartTime.ToString().Substring(0, 5) + "-" + item.Value.FirstEndTime.ToString().Substring(0, 5) + ")";
				}
				if (item.Value.SecondStartTime.HasValue && item.Value.SecondStartTime.ToString() != "  :  ")
				{
					text2 = text2 + " (" + item.Value.SecondStartTime.ToString().Substring(0, 5) + "-" + item.Value.SecondEndTime.ToString().Substring(0, 5) + ")";
				}
			}
			if (!string.IsNullOrWhiteSpace(text2))
			{
				text += text2;
				_conditionDaysHoursTextBlock.Text += text;
			}
		}
	}

	private void ParametersDialogValidation(DialogWindow dialog, DialogWindowValidationArgs args)
	{
		TimeSpan timeSpan = default;
		TimeSpan timeSpan2 = default;
		foreach (short weekDay in GetWeekDays())
		{
			if (dialog[$"FirstStartTime{weekDay}"].ToString() == "  :  " && dialog[$"FirstEndTime{weekDay}"].ToString() != "  :  ")
			{
				throw new ASException(string.Format(ArmSoft.SME.Common.Resources.FieldIsEmpty, ArmSoft.SME.Common.Resources.BeginOfPeriodShort) + ", " + DateExtension.DayOfWeekName(weekDay) + ", " + ArmSoft.SME.Common.Resources.Period.ToLower() + " 1");
			}
			if (dialog[$"FirstStartTime{weekDay}"].ToString() != "  :  " && dialog[$"FirstEndTime{weekDay}"].ToString() == "  :  ")
			{
				throw new ASException(string.Format(ArmSoft.SME.Common.Resources.FieldIsEmpty, ArmSoft.SME.Common.Resources.EndOfPeriodShort) + ", " + DateExtension.DayOfWeekName(weekDay) + ", " + ArmSoft.SME.Common.Resources.Period.ToLower() + " 1");
			}
			if (dialog[$"SecondStartTime{weekDay}"].ToString() == "  :  " && dialog[$"SecondEndTime{weekDay}"].ToString() != "  :  ")
			{
				throw new ASException(string.Format(ArmSoft.SME.Common.Resources.FieldIsEmpty, ArmSoft.SME.Common.Resources.BeginOfPeriodShort) + ", " + DateExtension.DayOfWeekName(weekDay) + ", " + ArmSoft.SME.Common.Resources.Period.ToLower() + " 2");
			}
			if (dialog[$"SecondStartTime{weekDay}"].ToString() != "  :  " && dialog[$"SecondEndTime{weekDay}"].ToString() == "  :  ")
			{
				throw new ASException(string.Format(ArmSoft.SME.Common.Resources.FieldIsEmpty, ArmSoft.SME.Common.Resources.EndOfPeriodShort) + ", " + DateExtension.DayOfWeekName(weekDay) + ", " + ArmSoft.SME.Common.Resources.Period.ToLower() + " 2");
			}
			TimeSpan result;
			if (dialog[$"FirstStartTime{weekDay}"] != null && dialog[$"FirstEndTime{weekDay}"] != null)
			{
				if (TimeSpan.TryParse(dialog[$"FirstStartTime{weekDay}"].ToString(), out result))
				{
					timeSpan = result;
				}
				if (TimeSpan.TryParse(dialog[$"FirstEndTime{weekDay}"].ToString(), out result))
				{
					timeSpan2 = result;
				}
				if (timeSpan2 < timeSpan)
				{
					throw new ASException(string.Format(ArmSoft.SME.Trade.Common.Resources.WrongTimePeriod, DateExtension.DayOfWeekName(weekDay)) + ", " + ArmSoft.SME.Common.Resources.Period.ToLower() + " 1");
				}
			}
			if (dialog[$"SecondStartTime{weekDay}"] != null && dialog[$"SecondEndTime{weekDay}"] != null)
			{
				if (TimeSpan.TryParse(dialog[$"SecondStartTime{weekDay}"].ToString(), out result))
				{
					timeSpan = result;
				}
				if (TimeSpan.TryParse(dialog[$"SecondEndTime{weekDay}"].ToString(), out result))
				{
					timeSpan2 = result;
				}
				if (timeSpan2 < timeSpan)
				{
					throw new ASException(string.Format(ArmSoft.SME.Trade.Common.Resources.WrongTimePeriod, DateExtension.DayOfWeekName(weekDay)) + ", " + ArmSoft.SME.Common.Resources.Period.ToLower() + " 2");
				}
			}
		}
	}

	private void ConditionDaysHours_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		ConditionDaysHours_Change();
	}

	private void UseExtension_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		_ = _useExtenstion.IsChecked.Value;
	}
}
