using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using ArmSoft.SME.Common;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Core.UI.Controls;
using ArmSoft.SME.Core.UI.PersistableObject;
using ArmSoft.SME.Trade.Common;
using ArmSoft.SME.Trade.Common.SmartEnums;
using DevExpress.Xpf.Editors;
using DevExpress.Xpf.LayoutControl;

namespace ArmSoft.SME.Trade.UI.CustomUIElements.UserControls;

public class InfoDisplayControls
{
	private readonly IPOWindow _ownerDocWindow;

	private readonly DocumentTypeSE _documentType;

	private readonly double _fontSizeLarge;

	private readonly double _fontSizeMedium;

	private readonly bool _isShowItemPictureOnInfoPanel;

	private readonly bool _isShowStorageAvailabilityOnInfoPanel;

	private readonly bool _isShowWholeAvailabilityOnInfoPanel;

	private readonly bool _showDescriptionOnInfoPanel;

	private readonly bool _enableSubstituteItems;

	private bool _isInfoAvailabilityCleared;

	private Span _span;

	public bool isShowPriceAvailabilityOnInfoPanel;

	private Span AccBalanceSpan { get; set; }

	private Span AccUnitBriefSpan { get; set; }

	private Span WholeBalanceSpan { get; set; }

	private Span WholeUnitBriefSpan { get; set; }

	private Span PriceSpan { get; set; }

	private Span VarianceSpan { get; set; }

	public LayoutGroup InfoDisplay { get; private set; }

	public LayoutGroup NameAndFormula { get; private set; }

	public LayoutGroup FormulaAndSubstitutionInfo { get; private set; }

	public LayoutGroup Availability { get; private set; }

	public TextEditExt InfoItemName { get; private set; }

	public TextEditExt InfoFormula { get; private set; }

	private TextBlock InfoAvailability { get; set; }

	public TextEditExt InfoDescription { get; private set; }

	public TextEditExt SubstitutionInfo { get; private set; }

	public ImageEdit ItemImage { get; private set; }

	public ImageEdit HintImage { get; private set; }

	public bool InfoPanelCollapsed { get; set; }

	public InfoDisplayControls(IPOWindow docWindow, DocumentTypeSE documentType, bool isShowItemPictureOnInfoPanel = false, bool isShowStorageAvailabilityOnInfoPanel = false, bool isShowWholeAvailabilityOnInfoPanel = false, bool isShowPriceAvailabilityOnInfoPanel = false, bool showDescriptionOnInfoPanel = false, bool enableSubstituteItems = false, double fontSizeLarge = 22.0)
	{
		_ownerDocWindow = docWindow;
		_documentType = documentType;
		_isShowItemPictureOnInfoPanel = isShowItemPictureOnInfoPanel;
		_isShowStorageAvailabilityOnInfoPanel = isShowStorageAvailabilityOnInfoPanel;
		_isShowWholeAvailabilityOnInfoPanel = isShowWholeAvailabilityOnInfoPanel;
		this.isShowPriceAvailabilityOnInfoPanel = isShowPriceAvailabilityOnInfoPanel;
		_showDescriptionOnInfoPanel = showDescriptionOnInfoPanel;
		_enableSubstituteItems = enableSubstituteItems;
		_fontSizeLarge = fontSizeLarge;
		_fontSizeMedium = _fontSizeLarge - 5.0;
	}

	public void ConstructUI(LayoutGroup container = null)
	{
		InfoDisplay = _ownerDocWindow.AddLayoutGroup(container, LayoutGroupView.GroupBox, Orientation.Horizontal, ArmSoft.SME.Trade.Common.Resources.MTBillInfoPanel);
		InfoDisplay.Expanded += new SafeEventHandler<EventArgs>(InfoDisplay_Expanded).Handler;
		NameAndFormula = _ownerDocWindow.AddLayoutGroup(InfoDisplay, LayoutGroupView.Group, Orientation.Vertical);
		InfoDisplay.IsCollapsible = true;
		InfoDisplay.VerticalAlignment = VerticalAlignment.Bottom;
		InfoItemName = _ownerDocWindow.AddStringControl(NameAndFormula, string.Empty, isRequired: false, 200, HorizontalAlignment.Stretch);
		if (_documentType == DocumentTypeSE.MTBill || _documentType == DocumentTypeSE.MTBillReturn || _documentType == DocumentTypeSE.MTInvoice || _documentType == DocumentTypeSE.InvoiceForPay)
		{
			FillMTBillInfoDisplay();
			return;
		}
		SetInfoPanelStringControlDefaultConfigs(InfoItemName);
		Availability = _ownerDocWindow.AddLayoutGroup(NameAndFormula, LayoutGroupView.Group, Orientation.Horizontal);
		InfoAvailability = AddStringControl(Availability);
		_isInfoAvailabilityCleared = true;
		if (_showDescriptionOnInfoPanel)
		{
			InfoDescription = _ownerDocWindow.AddStringControl(NameAndFormula, string.Empty, isRequired: false, 50, HorizontalAlignment.Stretch);
			SetInfoPanelStringControlDefaultConfigs(InfoDescription);
		}
	}

	public void SetStringAccBalance(string value)
	{
		CheckAndFillInfoAvailability();
		if (AccBalanceSpan != null)
		{
			AccBalanceSpan.Inlines.Clear();
			AccBalanceSpan.Inlines.Add(value);
			AccBalanceSpan.Foreground = (Settings.IsDarkTheme ? new SolidColorBrush(Color.FromRgb(40, 168, 234)) : Brushes.DarkBlue);
		}
	}

	public void SetStringAccUnitBrief(string value)
	{
		CheckAndFillInfoAvailability();
		if (AccUnitBriefSpan != null)
		{
			AccUnitBriefSpan.Inlines.Clear();
			AccUnitBriefSpan.Inlines.Add(value);
		}
	}

	public void SetAccBalance(decimal accBalance, bool isChangeColorNegetivValue = true)
	{
		CheckAndFillInfoAvailability();
		if (AccBalanceSpan != null)
		{
			AccBalanceSpan.Inlines.Clear();
			AccBalanceSpan.Inlines.Add($"{accBalance}");
			if (isChangeColorNegetivValue && accBalance < 0m)
			{
				AccBalanceSpan.Foreground = new SolidColorBrush(Color.FromArgb(byte.MaxValue, 229, 0, 0));
			}
			else
			{
				AccBalanceSpan.Foreground = (Settings.IsDarkTheme ? new SolidColorBrush(Color.FromRgb(40, 168, 234)) : Brushes.DarkBlue);
			}
		}
	}

	public void SetAccUnitBrief(string accUnitBrief)
	{
		CheckAndFillInfoAvailability();
		if (AccUnitBriefSpan != null)
		{
			AccUnitBriefSpan.Inlines.Clear();
			AccUnitBriefSpan.Inlines.Add($" {accUnitBrief}");
		}
	}

	public void SetStringWholeBalance(string value)
	{
		CheckAndFillInfoAvailability();
		if (WholeBalanceSpan != null)
		{
			WholeBalanceSpan.Inlines.Clear();
			WholeBalanceSpan.Inlines.Add(value);
			WholeBalanceSpan.Foreground = (Settings.IsDarkTheme ? new SolidColorBrush(Color.FromRgb(40, 168, 234)) : Brushes.DarkBlue);
		}
	}

	public void SetStringWholeUnitBrief(string value)
	{
		CheckAndFillInfoAvailability();
		if (WholeUnitBriefSpan != null)
		{
			WholeUnitBriefSpan.Inlines.Clear();
			WholeUnitBriefSpan.Inlines.Add(value);
		}
	}

	public void SetWholeBalance(decimal wholeBalance, bool isChangeColorNegetivValue = true)
	{
		CheckAndFillInfoAvailability();
		if (WholeBalanceSpan != null)
		{
			WholeBalanceSpan.Inlines.Clear();
			WholeBalanceSpan.Inlines.Add($"{wholeBalance}");
			if (isChangeColorNegetivValue && wholeBalance < 0m)
			{
				WholeBalanceSpan.Foreground = new SolidColorBrush(Color.FromArgb(byte.MaxValue, 229, 0, 0));
			}
			else
			{
				WholeBalanceSpan.Foreground = (Settings.IsDarkTheme ? new SolidColorBrush(Color.FromRgb(40, 168, 234)) : Brushes.DarkBlue);
			}
		}
	}

	public void SetWholeUnitBrief(string wholeUnitBrief)
	{
		CheckAndFillInfoAvailability();
		if (WholeUnitBriefSpan != null)
		{
			WholeUnitBriefSpan.Inlines.Clear();
			WholeUnitBriefSpan.Inlines.Add($" {wholeUnitBrief}");
		}
	}

	public void SetStringPrice(string value)
	{
		CheckAndFillInfoAvailability();
		if (PriceSpan != null)
		{
			PriceSpan.Inlines.Clear();
			PriceSpan.Inlines.Add(value);
			PriceSpan.Foreground = (Settings.IsDarkTheme ? new SolidColorBrush(Color.FromRgb(40, 168, 234)) : Brushes.DarkBlue);
		}
	}

	public void SetPrice(decimal price, bool isChangeColorNegetivValue = true)
	{
		CheckAndFillInfoAvailability();
		if (PriceSpan != null)
		{
			PriceSpan.Inlines.Clear();
			PriceSpan.Inlines.Add($"{price}");
			if (isChangeColorNegetivValue && price < 0m)
			{
				PriceSpan.Foreground = new SolidColorBrush(Color.FromArgb(byte.MaxValue, 229, 0, 0));
			}
			else
			{
				PriceSpan.Foreground = (Settings.IsDarkTheme ? new SolidColorBrush(Color.FromRgb(40, 168, 234)) : Brushes.DarkBlue);
			}
		}
	}

	public void SetVariance(decimal variance, bool isChangeColorNegetivValue = true)
	{
		CheckAndFillInfoAvailability();
		if (VarianceSpan != null)
		{
			VarianceSpan.Inlines.Clear();
			VarianceSpan.Inlines.Add($"{variance}");
			if (isChangeColorNegetivValue && variance < 0m)
			{
				VarianceSpan.Foreground = new SolidColorBrush(Color.FromArgb(byte.MaxValue, 229, 0, 0));
			}
			else
			{
				VarianceSpan.Foreground = (Settings.IsDarkTheme ? new SolidColorBrush(Color.FromRgb(40, 168, 234)) : Brushes.DarkBlue);
			}
		}
	}

	public void ClearInfoAvailability()
	{
		if (InfoAvailability != null)
		{
			InfoAvailability.Inlines.Clear();
			_isInfoAvailabilityCleared = true;
		}
	}

	private void CheckAndFillInfoAvailability()
	{
		if (InfoAvailability != null && _isInfoAvailabilityCleared)
		{
			FillAllInfo(isShowPriceAvailabilityOnInfoPanel);
		}
	}

	private void FillInfoAvailability()
	{
		if (_isShowStorageAvailabilityOnInfoPanel)
		{
			_span = GetSpan();
			_span.Inlines.Add($"{ArmSoft.SME.Common.Resources.AccBalance}: ");
			InfoAvailability.Inlines.Add(_span);
			AccBalanceSpan = GetSpan();
			AccBalanceSpan.Inlines.Add($"{string.Empty}");
			InfoAvailability.Inlines.Add(AccBalanceSpan);
			AccUnitBriefSpan = GetSpan();
			AccUnitBriefSpan.Inlines.Add($" {string.Empty}");
			InfoAvailability.Inlines.Add(AccUnitBriefSpan);
		}
		if (_isShowWholeAvailabilityOnInfoPanel)
		{
			_span = GetSpan();
			if (_isShowStorageAvailabilityOnInfoPanel)
			{
				_span.Inlines.Add($", {ArmSoft.SME.Trade.Common.Resources.Whole}: ");
			}
			else
			{
				_span.Inlines.Add($"{ArmSoft.SME.Trade.Common.Resources.Whole}: ");
			}
			InfoAvailability.Inlines.Add(_span);
			WholeBalanceSpan = GetSpan();
			WholeBalanceSpan.Inlines.Add($"{string.Empty}");
			InfoAvailability.Inlines.Add(WholeBalanceSpan);
			WholeUnitBriefSpan = GetSpan();
			WholeUnitBriefSpan.Inlines.Add($" {string.Empty}");
			InfoAvailability.Inlines.Add(WholeUnitBriefSpan);
		}
		_isInfoAvailabilityCleared = false;
	}

	public void FillAllInfo(bool isShowLastPurchase)
	{
		InfoAvailability.Inlines.Clear();
		FillInfoAvailability();
		FillInfoLastPurchase(isShowLastPurchase);
	}

	public void FillInfoLastPurchase(bool isShowLastPurchase)
	{
		if (isShowLastPurchase)
		{
			_span = GetSpan();
			if (_isShowStorageAvailabilityOnInfoPanel || _isShowWholeAvailabilityOnInfoPanel)
			{
				_span.Inlines.Add($", {ArmSoft.SME.Trade.Common.Resources.LastPurchasePrice}: ");
			}
			else
			{
				_span.Inlines.Add($"{ArmSoft.SME.Trade.Common.Resources.LastPurchasePrice}: ");
			}
			InfoAvailability.Inlines.Add(_span);
			PriceSpan = GetSpan();
			PriceSpan.Inlines.Add($"{string.Empty}");
			InfoAvailability.Inlines.Add(PriceSpan);
			_span = GetSpan();
			if (_isShowStorageAvailabilityOnInfoPanel || _isShowWholeAvailabilityOnInfoPanel)
			{
				_span.Inlines.Add($", {ArmSoft.SME.Trade.Common.Resources.VariancePercent}: ");
			}
			else
			{
				_span.Inlines.Add($"{ArmSoft.SME.Trade.Common.Resources.VariancePercent}: ");
			}
			InfoAvailability.Inlines.Add(_span);
			VarianceSpan = GetSpan();
			VarianceSpan.Inlines.Add($"{string.Empty}");
			InfoAvailability.Inlines.Add(VarianceSpan);
		}
	}

	private TextBlock AddStringControl(LayoutGroup container)
	{
		RichTextBox richTextBox = new RichTextBox
		{
			IsReadOnly = true,
			Background = Brushes.Transparent,
			BorderThickness = new Thickness(0.0),
			FontSize = ((_documentType == DocumentTypeSE.MTBill || _documentType == DocumentTypeSE.MTBillReturn) ? _fontSizeMedium : Settings.GetFontSize()),
			FontWeight = FontWeights.Bold
		};
		FlowDocument flowDocument = (richTextBox.Document = new FlowDocument());
		TextBlock textBlock = new TextBlock();
		Paragraph paragraph = new Paragraph();
		paragraph.Inlines.Add(textBlock);
		flowDocument.Blocks.Add(paragraph);
		LayoutGroupExt layoutGroupExt = _ownerDocWindow.AddLayoutGroup(container, LayoutGroupView.Group, Orientation.Horizontal);
		layoutGroupExt.Margin = new Thickness(0.0, 2.0, 0.0, 2.0);
		layoutGroupExt.Children.Add(richTextBox);
		return textBlock;
	}

	private static Span GetSpan()
	{
		return new Span
		{
			Foreground = (Settings.IsDarkTheme ? new SolidColorBrush(Color.FromRgb(40, 168, 234)) : Brushes.DarkBlue),
			FontWeight = FontWeights.Bold
		};
	}

	private void FillMTBillInfoDisplay()
	{
		double num = ((_documentType == DocumentTypeSE.MTInvoice) ? (Settings.GetFontSize() + 2.0) : _fontSizeLarge);
		SetInfoPanelStringControlDefaultConfigs(InfoItemName, num - (double)((!_isShowItemPictureOnInfoPanel) ? 2 : 0), Settings.IsDarkTheme ? new SolidColorBrush(Color.FromRgb(40, 168, 234)) : Brushes.DarkBlue);
		FormulaAndSubstitutionInfo = _ownerDocWindow.AddLayoutGroup(NameAndFormula, LayoutGroupView.Group, Orientation.Horizontal);
		InfoFormula = _ownerDocWindow.AddStringControl(FormulaAndSubstitutionInfo, string.Empty, isRequired: false, 50, HorizontalAlignment.Stretch);
		SetInfoPanelStringControlDefaultConfigs(InfoFormula, num + (double)(2 * ((_documentType == DocumentTypeSE.MTInvoice) ? (-1) : 0)), Settings.IsDarkTheme ? new SolidColorBrush(Color.FromRgb(40, 168, 234)) : Brushes.DarkBlue);
		if (_enableSubstituteItems)
		{
			AddSubstitutionHintOnInfoPanel(FormulaAndSubstitutionInfo);
		}
		if (_isShowStorageAvailabilityOnInfoPanel || _isShowWholeAvailabilityOnInfoPanel)
		{
			Availability = _ownerDocWindow.AddLayoutGroup(NameAndFormula, LayoutGroupView.Group, Orientation.Horizontal);
			InfoAvailability = AddStringControl(Availability);
			_isInfoAvailabilityCleared = true;
		}
		if (_showDescriptionOnInfoPanel)
		{
			InfoDescription = _ownerDocWindow.AddStringControl(NameAndFormula, string.Empty, isRequired: false, 50, HorizontalAlignment.Stretch);
			SetInfoPanelStringControlDefaultConfigs(InfoDescription, _fontSizeMedium, Settings.IsDarkTheme ? new SolidColorBrush(Color.FromRgb(40, 168, 234)) : Brushes.DarkBlue);
		}
		if (_isShowItemPictureOnInfoPanel)
		{
			ItemImage = AddImageControl(InfoDisplay, 100.0, 100.0, VerticalAlignment.Bottom, HorizontalAlignment.Right, showBorder: true);
		}
	}

	private static void SetInfoPanelStringControlDefaultConfigs(TextEditExt stringControl, double fontSize, SolidColorBrush foreground)
	{
		stringControl.ShowBorder = false;
		stringControl.IsReadOnly = true;
		stringControl.IsTabStop = false;
		stringControl.FontSize = fontSize;
		stringControl.FontWeight = FontWeights.Bold;
		stringControl.Foreground = foreground;
		stringControl.EditValue = string.Empty;
	}

	private static void SetInfoPanelStringControlDefaultConfigs(TextEditExt stringControl)
	{
		SetInfoPanelStringControlDefaultConfigs(stringControl, Settings.GetFontSize(), Settings.IsDarkTheme ? new SolidColorBrush(Color.FromRgb(40, 168, 234)) : Brushes.DarkBlue);
	}

	private void AddSubstitutionHintOnInfoPanel(LayoutGroup container)
	{
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		HintImage = AddImageControl(container, 16.0, 16.0, VerticalAlignment.Stretch, HorizontalAlignment.Right, showBorder: false);
		HintImage.Source = ImageService.GetImage("hint_16.svg");
		HintImage.Margin = new Thickness(0.0, 0.0, 0.0, 6.0);
		((LayoutGroup)(object)HintImage.Parent).Visibility = Visibility.Collapsed;
		SubstitutionInfo = _ownerDocWindow.AddStringControl(container, string.Empty, isRequired: false, 50, HorizontalAlignment.Right);
		SetInfoPanelStringControlDefaultConfigs(SubstitutionInfo, _fontSizeMedium, Settings.IsDarkTheme ? Brushes.White : Brushes.Black);
		SubstitutionInfo.Foreground = (Settings.IsDarkTheme ? Brushes.White : Brushes.Black);
	}

	private ImageEdit AddImageControl(LayoutGroup container, double height, double width, VerticalAlignment verticalAlignment, HorizontalAlignment horizontalAlignment, bool showBorder)
	{
		ImageEdit imageEdit = new ImageEdit
		{
			ShowMenu = false,
			ShowMenuMode = ShowMenuMode.Hover,
			ShowLoadDialogOnClickMode = ShowLoadDialogOnClickMode.Never,
			IsTabStop = false,
			Height = height,
			Width = width,
			Stretch = Stretch.Uniform,
			ShowBorder = showBorder
		};
		LayoutGroupExt layoutGroupExt = _ownerDocWindow.AddLayoutGroup(container, LayoutGroupView.Group, Orientation.Horizontal);
		layoutGroupExt.VerticalAlignment = verticalAlignment;
		layoutGroupExt.HorizontalAlignment = horizontalAlignment;
		layoutGroupExt.Children.Add(imageEdit);
		return imageEdit;
	}

	private void InfoDisplay_Expanded(object sender, EventArgs e)
	{
		InfoItemName.Foreground = (Settings.IsDarkTheme ? new SolidColorBrush(Color.FromRgb(40, 168, 234)) : Brushes.DarkBlue);
		if (InfoFormula != null)
		{
			InfoFormula.Foreground = (Settings.IsDarkTheme ? new SolidColorBrush(Color.FromRgb(40, 168, 234)) : Brushes.DarkBlue);
		}
		if (InfoAvailability != null)
		{
			InfoAvailability.Foreground = (Settings.IsDarkTheme ? new SolidColorBrush(Color.FromRgb(40, 168, 234)) : Brushes.DarkBlue);
		}
		if (InfoDescription != null)
		{
			InfoDescription.Foreground = (Settings.IsDarkTheme ? new SolidColorBrush(Color.FromRgb(40, 168, 234)) : Brushes.DarkBlue);
		}
		if (SubstitutionInfo != null)
		{
			SubstitutionInfo.Foreground = (Settings.IsDarkTheme ? Brushes.White : Brushes.Black);
		}
		InfoPanelCollapsed = false;
	}
}
