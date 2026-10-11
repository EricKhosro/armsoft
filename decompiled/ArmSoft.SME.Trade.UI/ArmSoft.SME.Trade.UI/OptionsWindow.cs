using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Media;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Extensions;
using ArmSoft.SME.Common.Models.Parameter;
using ArmSoft.SME.Common.Models.SmartEnums;
using ArmSoft.SME.Common.Parameters;
using ArmSoft.SME.Common.SmartEnums;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Core.UI.Constants;
using ArmSoft.SME.Core.UI.Controls;
using ArmSoft.SME.Core.UI.DropDowns;
using ArmSoft.SME.Trade.Common;
using ArmSoft.SME.Trade.Common.Models.Caches;
using ArmSoft.SME.Trade.Common.Parameters;
using ArmSoft.SME.Trade.Common.SmartEnums;
using ArmSoft.SME.Trade.UI.CustomUIElements.UserControls;
using ArmSoft.SME.Trade.UI.DropDown;
using DevExpress.Xpf.Editors;
using DevExpress.Xpf.LayoutControl;

namespace ArmSoft.SME.Trade.UI;

public class OptionsWindow : OptionsWindowBase
{
	protected LayoutControl _panelDevices;

	protected DeviceSettings _devicesSettings;

	protected TreeViewItem _menuItemDevices;

	private DateEditControl _dateBegin;

	private DateEditControl _dateEnd;

	private DropDownControl _vatType;

	private DropDownControl _dealType;

	private DropDownControl _mtCurr;

	private CheckEditExt _retailAccounting;

	private CheckEditExt _mtMoveWithConfirmation;

	private CheckEditExt _discountUse;

	private CheckEditExt _discountExtensionUse;

	private CheckEditExt _useBonus;

	private CheckEditExt _bonusExtensionUse;

	private DropDownControl _bonusType;

	private DropDownControl _bonusAccumulationMethod;

	private CheckEditExt _accumulateBonusForDiscountedGoods;

	private DropDownControl _bonusAccumulationBase;

	private PopupCalcEditExt _bonusAccumulationConditionSumGreater;

	private DateEditControl _bonusAccumulationConditionPeriodStart;

	private DateEditControl _bonusAccumulationConditionPeriodEnd;

	private PopupCalcEditExt _bonusUseMaxPercent;

	private CheckEditExt _noCalcBonusesWhenRePay;

	private CheckEditExt _connectTob2barmsoftDiscountServer;

	private CheckEditExt _syncContractsWithb2barmsoftDiscountServer;

	private SpinEditExt _syncContractsWithDicountServerInterval;

	private SpinEditExt _weightItemPrefix;

	private SpinEditExt _weightItemCodeLength;

	private SpinEditExt _weightItemPrecision;

	private DropDownControl _weightItemUnit;

	private SpinEditExt _pieceItemPrefix;

	private SpinEditExt _pieceItemCodeLength;

	private SpinEditExt _pieceItemPrecision;

	private DropDownControl _pieceItemUnit;

	private CheckEditExt _internalBarcodeEANFormat;

	private SpinEditExt _internalBarcodePrefix;

	private TextEditExt _internalBarcodeNext;

	private System.Windows.Controls.Button _barcodePrefixDelimiterBrowse;

	private TextEditExt _partnerEmployeeCardCodePrefixes;

	private CheckEditExt _usePartnerCardLenght;

	private SpinEditExt _partnerCardLenght;

	private CheckEditExt _autoSave;

	private SpinEditExt _autoSaveInterval;

	protected override bool GroupTemplatesByTypes => false;

	protected override bool GroupDocNumsByTypes => false;

	public OptionsWindow()
	{
		//IL_02ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b5: Expected Obj, but got Unknown
		//IL_02d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02dc: Expected Obj, but got Unknown
		//IL_02f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0303: Expected Obj, but got Unknown
		//IL_0320: Unknown result type (might be due to invalid IL or missing references)
		//IL_032a: Expected Obj, but got Unknown
		//IL_0347: Unknown result type (might be due to invalid IL or missing references)
		//IL_0351: Expected Obj, but got Unknown
		//IL_037a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0384: Expected Obj, but got Unknown
		//IL_03a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ab: Expected Obj, but got Unknown
		//IL_03c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d2: Expected Obj, but got Unknown
		_menuItemGeneral = AddMenuTreeViewItem("MenuItemGeneral", ArmSoft.SME.Common.Resources.General, "Options_General");
		_menuItemComp = AddMenuTreeViewItem("MenuItemComp", ArmSoft.SME.Common.Resources.Company, "Options_Company");
		_menuItemThemesAndFonts = AddMenuTreeViewItem("MenuItemThemesAndFonts", ArmSoft.SME.Common.Resources.FontSizesAndColors, "Options_ThemesAndFonts");
		_menuItemPrintOptions = AddMenuTreeViewItem("MenuItemPrintOptions", ArmSoft.SME.Common.Resources.PreviewOptions, "Options_PrintingOptions");
		_menuItemPrintTemplates = AddMenuTreeViewItem("MenuItemPrintTemplates", ArmSoft.SME.Common.Resources.PrintTemplates, "Options_PrintingTemplates");
		_menuItemDocNum = AddMenuTreeViewItem("MenuItemDocNum", ArmSoft.SME.Common.Resources.DocumentsNumbers, "Options_DocNumbers");
		_menuItemDevices = AddMenuTreeViewItem("MenuItemDevices", ArmSoft.SME.Common.Resources.OptionsDevices, "Options_Equipment");
		_menuItemSystemConfigs = AddMenuTreeViewItem("MenuItemSystemConfigs", ArmSoft.SME.Common.Resources.System, "Options_System");
		_panelGeneral = AddMenuLayoutControl(_menuItemGeneral, "PanelGeneral");
		_panelCompany = AddMenuLayoutControl(_menuItemComp, "PanelCompany");
		_panelDocNumbers = AddMenuLayoutControl(_menuItemDocNum, "PanelDocNumbers");
		_panelDevices = AddMenuLayoutControl(_menuItemDevices, "DevicesPanel");
		_devicesSettings = new DeviceSettings
		{
			Name = "DevicesSettings",
			VerticalAlignment = VerticalAlignment.Stretch
		};
		BindVisibility(_devicesSettings, _menuItemDevices);
		_panelDevices.Children.Add(_devicesSettings);
		_panelPageSetup = new AsPageSetup
		{
			Name = "PanelPageSetup",
			VerticalAlignment = VerticalAlignment.Stretch
		};
		BindVisibility(_panelPageSetup, _menuItemPrintOptions);
		_optionsPanels.Children.Add(_panelPageSetup);
		_panelPrintTemplates = AddMenuLayoutControl(_menuItemPrintTemplates, "PanelPrintTemplates");
		_panelThemes = new AsFontsPanel
		{
			Name = "PanelThemes",
			VerticalAlignment = VerticalAlignment.Stretch
		};
		BindVisibility(_panelThemes, _menuItemThemesAndFonts);
		_optionsPanels.Children.Add(_panelThemes);
		_panelSystemConfigs = AddMenuLayoutControl(_menuItemSystemConfigs, "PanelSystemConfigs");
		if (Settings.IsOffline)
		{
			_menuItemComp.Visibility = Visibility.Collapsed;
			_menuItemDocNum.Visibility = Visibility.Collapsed;
			_menuItemPrintTemplates.Visibility = Visibility.Collapsed;
			_menuItemGeneral.Visibility = Visibility.Collapsed;
			_menuItemSystemConfigs.Visibility = Visibility.Collapsed;
			_menuItemDevices.IsSelected = true;
		}
		else
		{
			_panelDocNumbers.IsVisibleChanged += new SafeEventHandler<DependencyPropertyChangedEventArgs>(PanelDocNumbers_IsVisibleChanged).Handler;
			_panelPrintTemplates.IsVisibleChanged += new SafeEventHandler<DependencyPropertyChangedEventArgs>(PanelPrintTemplates_IsVisibleChanged).Handler;
			_panelCompany.IsVisibleChanged += new SafeEventHandler<DependencyPropertyChangedEventArgs>(PanelCompany_IsVisibleChanged).Handler;
			_panelGeneral.IsVisibleChanged += new SafeEventHandler<DependencyPropertyChangedEventArgs>(PanelGeneral_IsVisibleChanged).Handler;
			_panelSystemConfigs.IsVisibleChanged += new SafeEventHandler<DependencyPropertyChangedEventArgs>(PanelSystemConfigs_IsVisibleChanged).Handler;
			_menuItemGeneral.IsSelected = true;
		}
		_panelPageSetup.IsVisibleChanged += new SafeEventHandler<DependencyPropertyChangedEventArgs>(PanelPageSetup_IsVisibleChanged).Handler;
		_panelThemes.IsVisibleChanged += new SafeEventHandler<DependencyPropertyChangedEventArgs>(PanelThemes_IsVisibleChanged).Handler;
		_panelDevices.IsVisibleChanged += new SafeEventHandler<DependencyPropertyChangedEventArgs>(DevicesPanel_IsVisibleChanged).Handler;
		_mnuTree.SelectedItemChanged += new SafeEventHandler<RoutedPropertyChangedEventArgs<object>>(MnuTree_SelectedItemChanged).Handler;
		_panelPageSetup.ReportKind = "";
		try
		{
			if (!Settings.IsOffline && Settings.LoginService.IsAdmin)
			{
				OptionsWindowBase.ParametersFromService = Settings.ParametersService.GetGeneralValuesFromService();
			}
			else
			{
				OptionsWindowBase.ParametersFromService = Settings.ParametersService.GetValuesFromService(Settings.LoginService.UserId);
			}
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	private void DevicesPanel_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
	{
		if (!_loadedPanels.Contains(_panelDevices.Name) && Convert.ToBoolean(e.NewValue))
		{
			_devicesSettings.FillSettings();
			_loadedPanels.Add(_panelDevices.Name);
		}
	}

	protected override void FillPanelGeneral()
	{
		//IL_0b22: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b28: Unknown result type (might be due to invalid IL or missing references)
		//IL_112a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1130: Unknown result type (might be due to invalid IL or missing references)
		if (!_loadedPanels.Contains(_panelGeneral.Name))
		{
			LayoutGroupExt container = OptionsWindowBase.AddLayoutGroup(_panelGeneral, System.Windows.Controls.Orientation.Vertical, "", LayoutGroupView.Group, System.Windows.HorizontalAlignment.Stretch, "", isCollapsible: false, isCollapsed: false);
			LayoutGroupExt container2 = OptionsWindowBase.AddLayoutGroup(container, System.Windows.Controls.Orientation.Horizontal, "", LayoutGroupView.Group, System.Windows.HorizontalAlignment.Stretch, "", isCollapsible: false, isCollapsed: false);
			_dateBegin = OptionsWindowBase.AddDateControl(container2, ArmSoft.SME.Common.Resources.OptionWorkPeriod, isRequired: true);
			_dateBegin.EditValue = (Settings.IsOffline ? Settings.ParametersService.DateBegin() : new DateOnly?(Settings.ParametersService.GetGeneralValue(ArmSoft.SME.Trade.Common.Parameters.Definitions.DateBegin.Id).YYYYMMDDToDateOnly()));
			_dateBegin.IsEnabled = Settings.LoginService.IsAdmin;
			_dateEnd = OptionsWindowBase.AddDateControl(container2, "", isRequired: true);
			_dateEnd.EditValue = (Settings.IsOffline ? Settings.ParametersService.DateEnd() : new DateOnly?(Settings.ParametersService.GetGeneralValue(ArmSoft.SME.Trade.Common.Parameters.Definitions.DateEnd.Id).YYYYMMDDToDateOnly()));
			_dateEnd.IsEnabled = Settings.LoginService.IsAdmin;
			AddSpecialDateButton(container2, _dateBegin, _dateEnd);
			AddBooleanControl(_panelGeneral, container, ArmSoft.SME.Common.Parameters.Definitions.SpreadsheetMode, out var usersParamButton, showOnlyUserValue: true);
			AddBooleanControl(_panelGeneral, container, ArmSoft.SME.Common.Parameters.Definitions.FilterPopupModeExcel, out usersParamButton, showOnlyUserValue: true, System.Windows.HorizontalAlignment.Left, string.Format(ArmSoft.SME.Common.Resources.FilterPopupModeExcel, Environment.NewLine));
			AddBooleanControl(_panelGeneral, container, ArmSoft.SME.Common.Parameters.Definitions.AutoFilterRowOldStyle, out usersParamButton, showOnlyUserValue: true);
			container = OptionsWindowBase.AddLayoutGroup(_panelGeneral, System.Windows.Controls.Orientation.Vertical, ArmSoft.SME.Common.Resources.TaxesAndRates, LayoutGroupView.GroupBox, System.Windows.HorizontalAlignment.Stretch, "TaxesAndRates", isCollapsible: true, isCollapsed: false);
			LayoutGroupExt container3 = OptionsWindowBase.AddLayoutGroup(container, System.Windows.Controls.Orientation.Vertical, ArmSoft.SME.Common.Resources.OptionsVAT, LayoutGroupView.GroupBox, System.Windows.HorizontalAlignment.Stretch, "", isCollapsible: true, isCollapsed: false);
			LayoutGroupExt container4 = OptionsWindowBase.AddLayoutGroup(container3, System.Windows.Controls.Orientation.Horizontal, "", LayoutGroupView.Group, System.Windows.HorizontalAlignment.Stretch, "", isCollapsible: false, isCollapsed: false);
			LayoutGroupExt container5 = OptionsWindowBase.AddLayoutGroup(container4, System.Windows.Controls.Orientation.Vertical, "", LayoutGroupView.Group, System.Windows.HorizontalAlignment.Stretch, "", isCollapsible: false, isCollapsed: false);
			LayoutGroupExt container6 = OptionsWindowBase.AddLayoutGroup(container4, System.Windows.Controls.Orientation.Vertical, "", LayoutGroupView.Group, System.Windows.HorizontalAlignment.Stretch, "", isCollapsible: false, isCollapsed: false);
			_vatType = AddDropDownControl(_panelGeneral, container5, ArmSoft.SME.Common.Parameters.Definitions.VATType, isRequired: false, new VATTypes(), out usersParamButton);
			_vatType.EditValueChanged += new SafeEventHandler<EditValueChangedEventArgs>(VATType_EditValueChanged).Handler;
			AddNumericControl(_panelGeneral, container6, ArmSoft.SME.Common.Parameters.Definitions.VAT, isRequired: false, 6, 2, out usersParamButton, allowNegativeNumbers: false, useCurrencyFormat: true, System.Windows.HorizontalAlignment.Left, $"{ArmSoft.SME.Common.Resources.OptionsVAT} (%)");
			AddDropDownControl(_panelGeneral, container5, ArmSoft.SME.Trade.Common.Parameters.Definitions.VATTypeRetail, isRequired: false, new VATTypes(includeVATRetail: false, onlyVATRetailTypes: true), out usersParamButton);
			AddNumericControl(_panelGeneral, container6, ArmSoft.SME.Trade.Common.Parameters.Definitions.VATIncluded, isRequired: false, 6, 2, out usersParamButton, allowNegativeNumbers: false, useCurrencyFormat: true, System.Windows.HorizontalAlignment.Left, $"{ArmSoft.SME.Common.Resources.CostWithInVAT} (%)");
			_dealType = AddDropDownControl(_panelGeneral, container3, ArmSoft.SME.Trade.Common.Parameters.Definitions.DealType, isRequired: false, new SmartEnum((!Settings.ParametersService.VATType().InList(VatTypesSE.CostWithVAT.Value, VatTypesSE.CostWithInVAT.Value)) ? typeof(AccDealTypesSE) : typeof(InvoiceDealTypesSE), null, padLeft: false, '0', allSubTypes: false, ValueMember.ValueUI), out usersParamButton, System.Windows.HorizontalAlignment.Stretch);
			AddBooleanControl(_panelGeneral, container3, ArmSoft.SME.Trade.Common.Parameters.Definitions.IncludeVATInCost, out usersParamButton);
			AddBooleanControl(_panelGeneral, container, ArmSoft.SME.Trade.Common.Parameters.Definitions.EnvironmentalFeePayer, out usersParamButton);
			AddBooleanControl(_panelGeneral, container, ArmSoft.SME.Trade.Common.Parameters.Definitions.UseMarking, out usersParamButton).IsEnabled |= _tradeShop;
			AddDropDownControl(_panelGeneral, container, ArmSoft.SME.Trade.Common.Parameters.Definitions.CPACheckMode, isRequired: true, new SmartEnum(typeof(CheckModeSE)), out usersParamButton);
			container = OptionsWindowBase.AddLayoutGroup(_panelGeneral, System.Windows.Controls.Orientation.Vertical, "", LayoutGroupView.Group);
			AddBooleanControl(_panelGeneral, container, ArmSoft.SME.Trade.Common.Parameters.Definitions.ShowPartnerTaxCode, out usersParamButton, showOnlyUserValue: false, System.Windows.HorizontalAlignment.Left, null, showCaptionAtRight: true, addPadding: true);
			AddBooleanControl(_panelGeneral, container, ArmSoft.SME.Trade.Common.Parameters.Definitions.ShowQuickStartCentre, out usersParamButton, showOnlyUserValue: false, System.Windows.HorizontalAlignment.Left, null, showCaptionAtRight: true, addPadding: true);
			AddBooleanControl(_panelGeneral, container, ArmSoft.SME.Trade.Common.Parameters.Definitions.CheckActualnessDateForReports, out usersParamButton, showOnlyUserValue: false, System.Windows.HorizontalAlignment.Left, null, showCaptionAtRight: true, addPadding: true);
			AddBooleanControl(_panelGeneral, container, ArmSoft.SME.Common.Parameters.Definitions.KeepUICacheUpToDate, out usersParamButton, showOnlyUserValue: true, System.Windows.HorizontalAlignment.Left, null, showCaptionAtRight: true, addPadding: true);
			container = OptionsWindowBase.AddLayoutGroup(_panelGeneral, System.Windows.Controls.Orientation.Vertical, ArmSoft.SME.Common.Resources.Documents, LayoutGroupView.GroupBox, System.Windows.HorizontalAlignment.Stretch, "Documents");
			AddBooleanControl(_panelGeneral, container, ArmSoft.SME.Trade.Common.Parameters.Definitions.DraftSelected, out usersParamButton, showOnlyUserValue: false, System.Windows.HorizontalAlignment.Left, null, showCaptionAtRight: true, addPadding: true);
			AddBooleanControl(_panelGeneral, container, ArmSoft.SME.Common.Parameters.Definitions.QuestWhenCloseWind, out usersParamButton, showOnlyUserValue: true, System.Windows.HorizontalAlignment.Left, null, showCaptionAtRight: true, addPadding: true);
			container = OptionsWindowBase.AddLayoutGroup(_panelGeneral, System.Windows.Controls.Orientation.Vertical, ArmSoft.SME.Trade.Common.Resources.TradeStorageAndOtherAccounting, LayoutGroupView.GroupBox, System.Windows.HorizontalAlignment.Stretch, "TradeStorageAndOtherAccounting");
			AddNumericControl(_panelGeneral, container, ArmSoft.SME.Trade.Common.Parameters.Definitions.Extra, isRequired: false, 6, 2, out usersParamButton, allowNegativeNumbers: false, useCurrencyFormat: false);
			_mtCurr = AddDropDownControl(_panelGeneral, container, ArmSoft.SME.Trade.Common.Parameters.Definitions.MTCurr, isRequired: true, new Currency(shortMode: true, withOutNCC: true), out usersParamButton);
			_mtCurr.IsEnabled = !Settings.IsOffline && !TradeSettings.CreateTradeClient().GoodsEngineServices.ExistOperation();
			AddBooleanControl(_panelGeneral, container, ArmSoft.SME.Trade.Common.Parameters.Definitions.EnableSubstituteItems, out usersParamButton, showOnlyUserValue: false, System.Windows.HorizontalAlignment.Left, ArmSoft.SME.Trade.Common.Resources.EnableSubstituteItems, showCaptionAtRight: false, addPadding: true);
			AddBooleanControl(_panelGeneral, container, ArmSoft.SME.Trade.Common.Parameters.Definitions.MaterialCheckRepetition, out usersParamButton, showOnlyUserValue: false, System.Windows.HorizontalAlignment.Left, null, showCaptionAtRight: false, addPadding: true);
			_retailAccounting = AddBooleanControl(_panelGeneral, container, ArmSoft.SME.Trade.Common.Parameters.Definitions.RetailAccounting, out usersParamButton, showOnlyUserValue: false, System.Windows.HorizontalAlignment.Left, null, showCaptionAtRight: false, addPadding: true);
			_mtMoveWithConfirmation = AddBooleanControl(_panelGeneral, container, ArmSoft.SME.Trade.Common.Parameters.Definitions.MTMoveWithConfirmation, out usersParamButton, showOnlyUserValue: false, System.Windows.HorizontalAlignment.Left, null, showCaptionAtRight: false, addPadding: true);
			container = OptionsWindowBase.AddLayoutGroup(_panelGeneral, System.Windows.Controls.Orientation.Vertical, ArmSoft.SME.Trade.Common.Resources.Discounts, LayoutGroupView.GroupBox, System.Windows.HorizontalAlignment.Stretch, "Discount");
			_discountUse = AddBooleanControl(_panelGeneral, container, ArmSoft.SME.Trade.Common.Parameters.Definitions.DiscountUse, out usersParamButton, showOnlyUserValue: false, System.Windows.HorizontalAlignment.Left, null, showCaptionAtRight: false, addPadding: true);
			_discountUse.EditValueChanged += new SafeEventHandler<EditValueChangedEventArgs>(DiscountUse_EditValueChanged).Handler;
			_discountUse.IsEnabled |= _tradeShop;
			_discountExtensionUse = AddBooleanControl(_panelGeneral, container, ArmSoft.SME.Trade.Common.Parameters.Definitions.DiscountExtensionUse, out usersParamButton, showOnlyUserValue: false, System.Windows.HorizontalAlignment.Left, "  " + ArmSoft.SME.Trade.Common.Parameters.Definitions.DiscountExtensionUse.Caption, showCaptionAtRight: false, addPadding: true);
			_discountExtensionUse.IsEnabled = Settings.LoginService.IsAdmin && Settings.ParametersService.DiscountUse();
			AddBooleanControl(_panelGeneral, container, ArmSoft.SME.Trade.Common.Parameters.Definitions.AllowDiscountInPurchase, out usersParamButton, showOnlyUserValue: false, System.Windows.HorizontalAlignment.Left, null, showCaptionAtRight: false, addPadding: true).IsEnabled |= _tradeShop;
			container = OptionsWindowBase.AddLayoutGroup(_panelGeneral, System.Windows.Controls.Orientation.Vertical, ArmSoft.SME.Trade.Common.Resources.Bonuses, LayoutGroupView.GroupBox, System.Windows.HorizontalAlignment.Stretch, "Bonuses");
			_useBonus = AddBooleanControl(_panelGeneral, container, ArmSoft.SME.Trade.Common.Parameters.Definitions.UseBonus, out usersParamButton);
			_useBonus.EditValueChanged += new SafeEventHandler<EditValueChangedEventArgs>(UseBonus_EditValueChanged).Handler;
			_bonusExtensionUse = AddBooleanControl(_panelGeneral, container, ArmSoft.SME.Trade.Common.Parameters.Definitions.BonusExtensionUse, out usersParamButton, showOnlyUserValue: false, System.Windows.HorizontalAlignment.Left, "  " + ArmSoft.SME.Trade.Common.Parameters.Definitions.BonusExtensionUse.Caption, showCaptionAtRight: false, addPadding: true);
			_bonusExtensionUse.EditValueChanged += new SafeEventHandler<EditValueChangedEventArgs>(BonusExtensionUse_EditValueChanged).Handler;
			_bonusType = AddDropDownControl(_panelGeneral, container, ArmSoft.SME.Trade.Common.Parameters.Definitions.BonusType, isRequired: true, new SmartEnum(typeof(BonusTypeSE)), out usersParamButton, System.Windows.HorizontalAlignment.Left, "  " + ArmSoft.SME.Trade.Common.Parameters.Definitions.BonusType.Caption);
			_bonusAccumulationMethod = AddDropDownControl(_panelGeneral, container, ArmSoft.SME.Trade.Common.Parameters.Definitions.BonusAccumulationMethod, isRequired: true, new SmartEnum(typeof(BonusAccumulationMethodSE)), out usersParamButton, System.Windows.HorizontalAlignment.Left, "  " + ArmSoft.SME.Trade.Common.Parameters.Definitions.BonusAccumulationMethod.Caption);
			_accumulateBonusForDiscountedGoods = AddBooleanControl(_panelGeneral, container, ArmSoft.SME.Trade.Common.Parameters.Definitions.AccumulateBonusForDiscountedGoods, out usersParamButton, showOnlyUserValue: false, System.Windows.HorizontalAlignment.Left, "  " + ArmSoft.SME.Trade.Common.Parameters.Definitions.AccumulateBonusForDiscountedGoods.Caption, showCaptionAtRight: false, addPadding: true);
			_bonusAccumulationBase = AddDropDownControl(_panelGeneral, container, ArmSoft.SME.Trade.Common.Parameters.Definitions.BonusAccumulationBase, isRequired: true, new SmartEnum(typeof(BonusAccumulationBaseSE)), out usersParamButton, System.Windows.HorizontalAlignment.Left, "  " + ArmSoft.SME.Trade.Common.Parameters.Definitions.BonusAccumulationBase.Caption);
			_bonusAccumulationBase.EditValueChanged += new SafeEventHandler<EditValueChangedEventArgs>(BonusAccumulationBase_EditValueChanged).Handler;
			LayoutGroupExt layoutGroupExt = OptionsWindowBase.AddLayoutGroup(container, System.Windows.Controls.Orientation.Horizontal, "", LayoutGroupView.Group);
			_bonusAccumulationConditionSumGreater = AddNumericControl(_panelGeneral, layoutGroupExt, ArmSoft.SME.Trade.Common.Parameters.Definitions.BonusAccumulationConditionSumGreater, Settings.ParametersService.BonusAccumulationBase() != BonusAccumulationBaseSE.WithoutCondition.Value, 16, 2, out usersParamButton, allowNegativeNumbers: false, useCurrencyFormat: true, System.Windows.HorizontalAlignment.Left, "    " + ArmSoft.SME.Trade.Common.Parameters.Definitions.BonusAccumulationConditionSumGreater.Caption);
			System.Windows.Controls.Label element = new System.Windows.Controls.Label
			{
				Content = ArmSoft.SME.Trade.Common.Resources.NCCName
			};
			layoutGroupExt.Children.Add(element);
			LayoutGroupExt container7 = OptionsWindowBase.AddLayoutGroup(container, System.Windows.Controls.Orientation.Horizontal, "", LayoutGroupView.Group);
			_bonusAccumulationConditionPeriodStart = AddDateControl(_panelGeneral, container7, ArmSoft.SME.Trade.Common.Parameters.Definitions.BonusAccumulationConditionPeriodStart, isRequired: false, out usersParamButton, showOnlyUserValue: false, useLongDate: false, System.Windows.HorizontalAlignment.Left, "    " + ArmSoft.SME.Trade.Common.Parameters.Definitions.BonusAccumulationConditionPeriodStart.Caption);
			_bonusAccumulationConditionPeriodEnd = AddDateControl(_panelGeneral, container7, ArmSoft.SME.Trade.Common.Parameters.Definitions.BonusAccumulationConditionPeriodEnd, isRequired: false, out usersParamButton, showOnlyUserValue: false, useLongDate: false, System.Windows.HorizontalAlignment.Left, "");
			_bonusUseMaxPercent = AddNumericControl(_panelGeneral, container, ArmSoft.SME.Trade.Common.Parameters.Definitions.BonusUseMaxPercent, isRequired: false, 6, 2, out usersParamButton, allowNegativeNumbers: false, useCurrencyFormat: true, System.Windows.HorizontalAlignment.Left, "  " + ArmSoft.SME.Trade.Common.Parameters.Definitions.BonusUseMaxPercent.Caption);
			_noCalcBonusesWhenRePay = AddBooleanControl(_panelGeneral, container, ArmSoft.SME.Trade.Common.Parameters.Definitions.NoCalcBonusesWhenRePay, out usersParamButton, showOnlyUserValue: false, System.Windows.HorizontalAlignment.Left, "  " + ArmSoft.SME.Trade.Common.Parameters.Definitions.NoCalcBonusesWhenRePay.Caption, showCaptionAtRight: false, addPadding: true);
			_connectTob2barmsoftDiscountServer = AddBooleanControl(_panelGeneral, container, ArmSoft.SME.Trade.Common.Parameters.Definitions.ConnectTob2barmsoftDiscountServer, out usersParamButton);
			((LayoutItemWithRequired)(object)_connectTob2barmsoftDiscountServer.Parent).FontWeight = FontWeights.Bold;
			_connectTob2barmsoftDiscountServer.EditValueChanged += new SafeEventHandler<EditValueChangedEventArgs>(ConnectTob2barmsoftDiscountServer_EditValueChanged).Handler;
			_syncContractsWithb2barmsoftDiscountServer = AddBooleanControl(_panelGeneral, container, ArmSoft.SME.Trade.Common.Parameters.Definitions.SyncContractsWithb2barmsoftDiscountServer, out usersParamButton);
			_syncContractsWithb2barmsoftDiscountServer.IsEnabled = Settings.LoginService.IsDBAdmin && Settings.ParametersService.ConnectTob2barmsoftDiscountServer();
			_syncContractsWithb2barmsoftDiscountServer.EditValueChanged += new SafeEventHandler<EditValueChangedEventArgs>(SyncContractsWithb2barmsoftDiscountServer_EditValueChanged).Handler;
			_syncContractsWithDicountServerInterval = AddSpinEditControl(_panelGeneral, container, ArmSoft.SME.Trade.Common.Parameters.Definitions.DiscountServerSyncInterval, 5m, 999999999m, out usersParamButton);
			_syncContractsWithDicountServerInterval.IsEnabled = Settings.LoginService.IsDBAdmin && Settings.ParametersService.SyncContractsWithb2barmsoftDiscountServer();
			LayoutGroupExt layoutGroupExt2 = OptionsWindowBase.AddLayoutGroup(container, System.Windows.Controls.Orientation.Horizontal, "", LayoutGroupView.Group);
			ImageEdit element2 = new ImageEdit
			{
				Source = ImageService.GetImage("information_16.svg"),
				Padding = new Thickness(0.0, 0.0, 0.0, 0.0),
				IsEnabled = false,
				ShowBorder = false,
				Stretch = Stretch.Fill,
				VerticalAlignment = VerticalAlignment.Top,
				Margin = new Thickness(0.0, 0.0, 0.0, 0.0),
				Height = 16.0,
				Width = 16.0
			};
			layoutGroupExt2.Children.Add(element2);
			TextBlock textBlock = OptionsWindowBase.AddTextBlock(layoutGroupExt2, ArmSoft.SME.Trade.Common.Resources.ConnectTob2barmsoftDiscServInfoMsg, new Thickness(0.0, 0.0, 0.0, 0.0));
			textBlock.Foreground = ColorsAS.Foreground.BlueBrush;
			textBlock.Width = SizeCalculator.Width(640.0);
			container = OptionsWindowBase.AddLayoutGroup(_panelGeneral, System.Windows.Controls.Orientation.Vertical, ArmSoft.SME.Trade.Common.Resources.WeightItemOptions, LayoutGroupView.GroupBox, System.Windows.HorizontalAlignment.Stretch, "WeightItemOptions");
			_weightItemPrefix = OptionsWindowBase.AddSpinEditControl(container, ArmSoft.SME.Trade.Common.Resources.Prefix, 20m, 29m);
			_weightItemPrefix.EditValue = WeightItemInfo.GetInstance().Prefix;
			_weightItemPrefix.IsEnabled = Settings.LoginService.IsAdmin || _tradeShop;
			_weightItemCodeLength = OptionsWindowBase.AddSpinEditControl(container, ArmSoft.SME.Trade.Common.Resources.ItemCodeLength, 5m, 7m);
			_weightItemCodeLength.EditValue = WeightItemInfo.GetInstance().CodeLength;
			_weightItemCodeLength.IsEnabled = Settings.LoginService.IsAdmin || _tradeShop;
			_weightItemPrecision = OptionsWindowBase.AddSpinEditControl(container, ArmSoft.SME.Trade.Common.Resources.ItemPrecision, 0m, 3m);
			_weightItemPrecision.EditValue = WeightItemInfo.GetInstance().Precision;
			_weightItemPrecision.IsEnabled = Settings.LoginService.IsAdmin || _tradeShop;
			_weightItemUnit = AddDropDownControl(container, ArmSoft.SME.Trade.Common.Resources.QntUnit, isRequired: true, new QntUnits());
			_weightItemUnit.Code = WeightItemInfo.GetInstance().Unit;
			_weightItemUnit.IsEnabled = Settings.LoginService.IsAdmin || _tradeShop;
			container = OptionsWindowBase.AddLayoutGroup(_panelGeneral, System.Windows.Controls.Orientation.Vertical, ArmSoft.SME.Trade.Common.Resources.PieceItemOptions, LayoutGroupView.GroupBox, System.Windows.HorizontalAlignment.Stretch, "PieceItemOptions");
			_pieceItemPrefix = OptionsWindowBase.AddSpinEditControl(container, ArmSoft.SME.Trade.Common.Resources.Prefix, 20m, 29m);
			_pieceItemPrefix.EditValue = PieceItemInfo.GetInstance().Prefix;
			_pieceItemPrefix.IsEnabled = Settings.LoginService.IsAdmin || _tradeShop;
			_pieceItemCodeLength = OptionsWindowBase.AddSpinEditControl(container, ArmSoft.SME.Trade.Common.Resources.ItemCodeLength, 5m, 7m);
			_pieceItemCodeLength.EditValue = PieceItemInfo.GetInstance().CodeLength;
			_pieceItemCodeLength.IsEnabled = Settings.LoginService.IsAdmin || _tradeShop;
			_pieceItemPrecision = OptionsWindowBase.AddSpinEditControl(container, ArmSoft.SME.Trade.Common.Resources.ItemPrecision, 0m, 3m);
			_pieceItemPrecision.EditValue = PieceItemInfo.GetInstance().Precision;
			_pieceItemPrecision.IsEnabled = Settings.LoginService.IsAdmin || _tradeShop;
			_pieceItemUnit = AddDropDownControl(container, ArmSoft.SME.Trade.Common.Resources.QntUnit, isRequired: true, new QntUnits());
			_pieceItemUnit.Code = PieceItemInfo.GetInstance().Unit;
			_pieceItemUnit.IsEnabled = Settings.LoginService.IsAdmin || _tradeShop;
			container = OptionsWindowBase.AddLayoutGroup(_panelGeneral, System.Windows.Controls.Orientation.Vertical, ArmSoft.SME.Trade.Common.Resources.PartnerEmployeeCardCodeOptions, LayoutGroupView.GroupBox, System.Windows.HorizontalAlignment.Stretch, "PartnerEmployeeCardCodeOptions");
			AddBooleanControl(_panelGeneral, container, ArmSoft.SME.Trade.Common.Parameters.Definitions.UsePartnerEmployeeSearchNewMode, out usersParamButton).IsEnabled |= true;
			LayoutGroupExt container8 = OptionsWindowBase.AddLayoutGroup(container, System.Windows.Controls.Orientation.Horizontal, "", LayoutGroupView.Group);
			_partnerEmployeeCardCodePrefixes = AddStringControl(_panelGeneral, container8, ArmSoft.SME.Trade.Common.Parameters.Definitions.PartnerEmployeeCardCodePrefixes, isRequired: false, 100, out usersParamButton);
			_partnerEmployeeCardCodePrefixes.IsEnabled = false;
			_barcodePrefixDelimiterBrowse = OptionsWindowBase.AddButton(container8, "", " ... ");
			_barcodePrefixDelimiterBrowse.IsEnabled = Settings.LoginService.IsAdmin || _tradeShop;
			_barcodePrefixDelimiterBrowse.Click += new SafeEventHandler<RoutedEventArgs>(BarcodePrefixDelimiterBrowse_Click).Handler;
			_usePartnerCardLenght = AddBooleanControl(_panelGeneral, container, ArmSoft.SME.Trade.Common.Parameters.Definitions.UsePartnerCardLenght, out usersParamButton);
			_usePartnerCardLenght.EditValueChanged += new SafeEventHandler<EditValueChangedEventArgs>(UsePartnerCardLenght_EditValueChanged).Handler;
			_usePartnerCardLenght.IsEnabled |= _tradeShop;
			_partnerCardLenght = AddSpinEditControl(_panelGeneral, container, ArmSoft.SME.Trade.Common.Parameters.Definitions.PartnerCardLenght, 1m, 50m, out usersParamButton);
			_partnerCardLenght.IsEnabled = Settings.ParametersService.UsePartnerCardLenght();
			LayoutGroupExt layoutGroupExt3 = OptionsWindowBase.AddLayoutGroup(container, System.Windows.Controls.Orientation.Horizontal, "", LayoutGroupView.Group);
			ImageEdit element3 = new ImageEdit
			{
				Source = ImageService.GetImage("information_16.svg"),
				Padding = new Thickness(0.0, 0.0, 0.0, 0.0),
				IsEnabled = false,
				ShowBorder = false,
				Stretch = Stretch.Fill,
				VerticalAlignment = VerticalAlignment.Top,
				Margin = new Thickness(0.0, 0.0, 0.0, 0.0),
				Height = 16.0,
				Width = 16.0
			};
			layoutGroupExt3.Children.Add(element3);
			TextBlock textBlock2 = OptionsWindowBase.AddTextBlock(layoutGroupExt3, ArmSoft.SME.Trade.Common.Resources.PartnerEmployeeBarcodePrefixButtonInfo, new Thickness(0.0, 0.0, 0.0, 0.0));
			textBlock2.Foreground = ColorsAS.Foreground.BlueBrush;
			textBlock2.Width = SizeCalculator.Width(600.0);
			container = OptionsWindowBase.AddLayoutGroup(_panelGeneral, System.Windows.Controls.Orientation.Vertical, ArmSoft.SME.Trade.Common.Resources.InternalBarcodeParams, LayoutGroupView.GroupBox, System.Windows.HorizontalAlignment.Stretch, "InternalBarcodeParams");
			_internalBarcodeEANFormat = AddBooleanControl(_panelGeneral, container, ArmSoft.SME.Trade.Common.Parameters.Definitions.InternalBarcodeEANFormat, out usersParamButton);
			_internalBarcodeEANFormat.EditValueChanged += new SafeEventHandler<EditValueChangedEventArgs>(InternalBarcodeEANFormat_EditValueChanged).Handler;
			_internalBarcodeEANFormat.IsEnabled |= _tradeShop;
			_internalBarcodePrefix = AddSpinEditControl(_panelGeneral, container, ArmSoft.SME.Trade.Common.Parameters.Definitions.InternalBarcodePrefix, 20m, 29m, out usersParamButton);
			_internalBarcodePrefix.IsEnabled = (Settings.LoginService.IsAdmin || _tradeShop) && Settings.ParametersService.InternalBarcodeEANFormat();
			_internalBarcodeNext = AddStringControl(_panelGeneral, container, ArmSoft.SME.Trade.Common.Parameters.Definitions.InternalBarcodeNext, isRequired: false, 10, out usersParamButton);
			_internalBarcodeNext.IsEnabled |= _tradeShop;
			container = OptionsWindowBase.AddLayoutGroup(_panelGeneral, System.Windows.Controls.Orientation.Vertical, ArmSoft.SME.Trade.Common.Resources.PartyOptions, LayoutGroupView.GroupBox, System.Windows.HorizontalAlignment.Stretch, "PartyOptions");
			AddBooleanControl(_panelGeneral, container, ArmSoft.SME.Trade.Common.Parameters.Definitions.ShowPartySUPPLIER, out usersParamButton, showOnlyUserValue: false, System.Windows.HorizontalAlignment.Left, null, showCaptionAtRight: true, addPadding: true);
			AddBooleanControl(_panelGeneral, container, ArmSoft.SME.Trade.Common.Parameters.Definitions.ShowPartyCONTRACT, out usersParamButton, showOnlyUserValue: false, System.Windows.HorizontalAlignment.Left, null, showCaptionAtRight: true, addPadding: true);
			AddBooleanControl(_panelGeneral, container, ArmSoft.SME.Trade.Common.Parameters.Definitions.ShowPartyDocType, out usersParamButton, showOnlyUserValue: false, System.Windows.HorizontalAlignment.Left, null, showCaptionAtRight: true, addPadding: true);
			AddBooleanControl(_panelGeneral, container, ArmSoft.SME.Trade.Common.Parameters.Definitions.ShowPartyDocNum, out usersParamButton, showOnlyUserValue: false, System.Windows.HorizontalAlignment.Left, null, showCaptionAtRight: true, addPadding: true);
			AddBooleanControl(_panelGeneral, container, ArmSoft.SME.Trade.Common.Parameters.Definitions.ShowPartyIncomeDate, out usersParamButton, showOnlyUserValue: false, System.Windows.HorizontalAlignment.Left, null, showCaptionAtRight: true, addPadding: true);
			AddBooleanControl(_panelGeneral, container, ArmSoft.SME.Trade.Common.Parameters.Definitions.ShowPartyUSEABLEDATE, out usersParamButton, showOnlyUserValue: false, System.Windows.HorizontalAlignment.Left, null, showCaptionAtRight: true, addPadding: true);
			AddBooleanControl(_panelGeneral, container, ArmSoft.SME.Trade.Common.Parameters.Definitions.ShowPartyCountry, out usersParamButton, showOnlyUserValue: false, System.Windows.HorizontalAlignment.Left, null, showCaptionAtRight: true, addPadding: true);
			AddBooleanControl(_panelGeneral, container, ArmSoft.SME.Trade.Common.Parameters.Definitions.ShowPartyProducer, out usersParamButton, showOnlyUserValue: false, System.Windows.HorizontalAlignment.Left, null, showCaptionAtRight: true, addPadding: true);
			AddBooleanControl(_panelGeneral, container, ArmSoft.SME.Trade.Common.Parameters.Definitions.ShowPartyProperty1, out usersParamButton, showOnlyUserValue: false, System.Windows.HorizontalAlignment.Left, null, showCaptionAtRight: true, addPadding: true);
			AddBooleanControl(_panelGeneral, container, ArmSoft.SME.Trade.Common.Parameters.Definitions.ShowPartyProperty2, out usersParamButton, showOnlyUserValue: false, System.Windows.HorizontalAlignment.Left, null, showCaptionAtRight: true, addPadding: true);
			container = OptionsWindowBase.AddLayoutGroup(_panelGeneral, System.Windows.Controls.Orientation.Vertical, ArmSoft.SME.Common.Resources.CloudServices, LayoutGroupView.GroupBox, System.Windows.HorizontalAlignment.Stretch, "CloudServices");
			AddBooleanControl(_panelGeneral, container, ArmSoft.SME.Common.Parameters.Definitions.ImportExchangeRates, out usersParamButton, showOnlyUserValue: false, System.Windows.HorizontalAlignment.Left, null, showCaptionAtRight: true, addPadding: true);
			EnableBonusSettings(Settings.ParametersService.UseBonus());
			_loadedPanels.Add(_panelGeneral.Name);
		}
	}

	protected override void FillPanelCompany()
	{
		if (_loadedPanels.Contains(_panelCompany.Name))
		{
			return;
		}
		LayoutGroupExt container = OptionsWindowBase.AddLayoutGroup(_panelCompany, System.Windows.Controls.Orientation.Vertical, ArmSoft.SME.Common.Resources.Main, LayoutGroupView.GroupBox, System.Windows.HorizontalAlignment.Stretch, "", isCollapsible: false, isCollapsed: false);
		AddStringControl(_panelCompany, container, ArmSoft.SME.Common.Parameters.Definitions.CompanyName, isRequired: false, 100, out var usersParamButton, System.Windows.HorizontalAlignment.Stretch);
		AddStringControl(_panelCompany, container, ArmSoft.SME.Trade.Common.Parameters.Definitions.CompanyBusinessAddress, isRequired: false, 255, out usersParamButton, System.Windows.HorizontalAlignment.Stretch);
		AddStringControl(_panelCompany, container, ArmSoft.SME.Trade.Common.Parameters.Definitions.CompanyAddress, isRequired: false, 255, out usersParamButton, System.Windows.HorizontalAlignment.Stretch);
		AddStringControl(_panelCompany, container, ArmSoft.SME.Trade.Common.Parameters.Definitions.PhoneNumber, isRequired: false, 100, out usersParamButton, System.Windows.HorizontalAlignment.Stretch);
		AddStringControl(_panelCompany, container, ArmSoft.SME.Trade.Common.Parameters.Definitions.Email, isRequired: false, 129, out usersParamButton, System.Windows.HorizontalAlignment.Stretch);
		container = OptionsWindowBase.AddLayoutGroup(_panelCompany, System.Windows.Controls.Orientation.Vertical, ArmSoft.SME.Common.Resources.Numbers, LayoutGroupView.GroupBox, System.Windows.HorizontalAlignment.Stretch, "", isCollapsible: false, isCollapsed: false);
		AddStringControl(_panelCompany, container, ArmSoft.SME.Common.Parameters.Definitions.TaxCode, isRequired: false, 20, out usersParamButton, System.Windows.HorizontalAlignment.Stretch);
		LayoutGroupExt container2 = OptionsWindowBase.AddLayoutGroup(container, System.Windows.Controls.Orientation.Horizontal, "", LayoutGroupView.Group, System.Windows.HorizontalAlignment.Stretch, "", isCollapsible: false, isCollapsed: false);
		AddStringControl(_panelCompany, container2, ArmSoft.SME.Trade.Common.Parameters.Definitions.StateRegister, isRequired: false, 100, out usersParamButton, System.Windows.HorizontalAlignment.Stretch);
		container = OptionsWindowBase.AddLayoutGroup(_panelCompany, System.Windows.Controls.Orientation.Vertical, ArmSoft.SME.Common.Resources.ResponsiblePersons, LayoutGroupView.GroupBox, System.Windows.HorizontalAlignment.Stretch, "", isCollapsible: false, isCollapsed: false);
		AddStringControl(_panelCompany, container, ArmSoft.SME.Trade.Common.Parameters.Definitions.ManagerPost, isRequired: false, 100, out usersParamButton, System.Windows.HorizontalAlignment.Stretch);
		AddStringControl(_panelCompany, container, ArmSoft.SME.Trade.Common.Parameters.Definitions.ManagerName, isRequired: false, 100, out usersParamButton, System.Windows.HorizontalAlignment.Stretch);
		AddStringControl(_panelCompany, container, ArmSoft.SME.Trade.Common.Parameters.Definitions.ChiefAccountantPost, isRequired: false, 100, out usersParamButton, System.Windows.HorizontalAlignment.Stretch);
		AddStringControl(_panelCompany, container, ArmSoft.SME.Trade.Common.Parameters.Definitions.ChiefAccountantName, isRequired: false, 100, out usersParamButton, System.Windows.HorizontalAlignment.Stretch);
		AddStringControl(_panelCompany, container, ArmSoft.SME.Trade.Common.Parameters.Definitions.CashierName, isRequired: false, 100, out usersParamButton, System.Windows.HorizontalAlignment.Stretch);
		if (_tradeShop && _controlsByPanels.TryGetValue(_panelCompany.Name, out var value))
		{
			foreach (ControlDesc item in value)
			{
				item.Control.IsEnabled = true;
			}
		}
		_panelCompany.IsEnabled = Settings.LoginService.IsAdmin || _tradeShop;
		_loadedPanels.Add(_panelCompany.Name);
	}

	protected override Dictionary<string, List<string>> InitDocTemplatesList()
	{
		Dictionary<string, List<string>> dictionary = new Dictionary<string, List<string>>();
		int num = 29;
		List<string> list = new List<string>(num);
		CollectionsMarshal.SetCount(list, num);
		Span<string> span = CollectionsMarshal.AsSpan(list);
		span[0] = ArmSoft.SME.Trade.Common.SmartEnums.PrintTemplateDocumentTypeSE.PayOrd.Value;
		span[1] = ArmSoft.SME.Trade.Common.SmartEnums.PrintTemplateDocumentTypeSE.CashIn.Value;
		span[2] = ArmSoft.SME.Trade.Common.SmartEnums.PrintTemplateDocumentTypeSE.CashOut.Value;
		span[3] = ArmSoft.SME.Trade.Common.SmartEnums.PrintTemplateDocumentTypeSE.PartnerRemCorrect.Value;
		span[4] = ArmSoft.SME.Trade.Common.SmartEnums.PrintTemplateDocumentTypeSE.MTIncome.Value;
		span[5] = ArmSoft.SME.Trade.Common.SmartEnums.PrintTemplateDocumentTypeSE.MTInvoice.Value;
		span[6] = ArmSoft.SME.Trade.Common.SmartEnums.PrintTemplateDocumentTypeSE.MTExpense.Value;
		span[7] = ArmSoft.SME.Trade.Common.SmartEnums.PrintTemplateDocumentTypeSE.MTMove.Value;
		span[8] = ArmSoft.SME.Trade.Common.SmartEnums.PrintTemplateDocumentTypeSE.MTMoveWithConfirmation.Value;
		span[9] = ArmSoft.SME.Trade.Common.SmartEnums.PrintTemplateDocumentTypeSE.MTComplectation.Value;
		span[10] = ArmSoft.SME.Trade.Common.SmartEnums.PrintTemplateDocumentTypeSE.MTDeComplectation.Value;
		span[11] = ArmSoft.SME.Trade.Common.SmartEnums.PrintTemplateDocumentTypeSE.MTReturnToSupplier.Value;
		span[12] = ArmSoft.SME.Trade.Common.SmartEnums.PrintTemplateDocumentTypeSE.MTReturnFromCustomer.Value;
		span[13] = ArmSoft.SME.Trade.Common.SmartEnums.PrintTemplateDocumentTypeSE.MTStockTaking.Value;
		span[14] = ArmSoft.SME.Trade.Common.SmartEnums.PrintTemplateDocumentTypeSE.MTBill.Value;
		span[15] = ArmSoft.SME.Trade.Common.SmartEnums.PrintTemplateDocumentTypeSE.MTIncomeImport.Value;
		span[16] = ArmSoft.SME.Trade.Common.SmartEnums.PrintTemplateDocumentTypeSE.MTIncomeRetail.Value;
		span[17] = ArmSoft.SME.Trade.Common.SmartEnums.PrintTemplateDocumentTypeSE.MTSaleRetail.Value;
		span[18] = ArmSoft.SME.Trade.Common.SmartEnums.PrintTemplateDocumentTypeSE.MTStockTakingRetail.Value;
		span[19] = ArmSoft.SME.Trade.Common.SmartEnums.PrintTemplateDocumentTypeSE.MTRevaluationRetail.Value;
		span[20] = ArmSoft.SME.Trade.Common.SmartEnums.PrintTemplateDocumentTypeSE.MTIncomeRem.Value;
		span[21] = ArmSoft.SME.Trade.Common.SmartEnums.PrintTemplateDocumentTypeSE.PartnerRemMove.Value;
		span[22] = ArmSoft.SME.Trade.Common.SmartEnums.PrintTemplateDocumentTypeSE.ReceivedService.Value;
		span[23] = ArmSoft.SME.Trade.Common.SmartEnums.PrintTemplateDocumentTypeSE.MTPurchaseOrder.Value;
		span[24] = ArmSoft.SME.Trade.Common.SmartEnums.PrintTemplateDocumentTypeSE.InvoiceForPay.Value;
		span[25] = ArmSoft.SME.Trade.Common.SmartEnums.PrintTemplateDocumentTypeSE.Reconciliation.Value;
		span[26] = ArmSoft.SME.Trade.Common.SmartEnums.PrintTemplateDocumentTypeSE.MTRevaluation.Value;
		span[27] = ArmSoft.SME.Trade.Common.SmartEnums.PrintTemplateDocumentTypeSE.MTMoveOrder.Value;
		span[28] = ArmSoft.SME.Trade.Common.SmartEnums.PrintTemplateDocumentTypeSE.MTBillReturn.Value;
		List<string> value = list;
		dictionary.Add("", value);
		return dictionary;
	}

	protected override List<GridParamValueRow> InitDocNumsList()
	{
		int num = 36;
		List<GridParamValueRow> list = new List<GridParamValueRow>(num);
		CollectionsMarshal.SetCount(list, num);
		Span<GridParamValueRow> span = CollectionsMarshal.AsSpan(list);
		span[0] = new GridParamValueRow(ArmSoft.SME.Trade.Common.Parameters.Definitions.ReceivedServiceNumber);
		span[1] = new GridParamValueRow(ArmSoft.SME.Trade.Common.Parameters.Definitions.MTIncomeNumber);
		span[2] = new GridParamValueRow(ArmSoft.SME.Trade.Common.Parameters.Definitions.MTIncomeImportNumber);
		span[3] = new GridParamValueRow(ArmSoft.SME.Trade.Common.Parameters.Definitions.MTIncomeRetailNumber);
		span[4] = new GridParamValueRow(ArmSoft.SME.Trade.Common.Parameters.Definitions.OverheadExpNumber);
		span[5] = new GridParamValueRow(ArmSoft.SME.Trade.Common.Parameters.Definitions.MTReturnToSupNumber);
		span[6] = new GridParamValueRow(ArmSoft.SME.Trade.Common.Parameters.Definitions.MTInvoiceNumber);
		span[7] = new GridParamValueRow(ArmSoft.SME.Trade.Common.Parameters.Definitions.InvoiceForPayNumber);
		span[8] = new GridParamValueRow(ArmSoft.SME.Trade.Common.Parameters.Definitions.MTSaleRetailNumber);
		span[9] = new GridParamValueRow(ArmSoft.SME.Trade.Common.Parameters.Definitions.MTBillNumber);
		span[10] = new GridParamValueRow(ArmSoft.SME.Trade.Common.Parameters.Definitions.MTRetFromCustNumber);
		span[11] = new GridParamValueRow(ArmSoft.SME.Trade.Common.Parameters.Definitions.MTBillReturnNumber);
		span[12] = new GridParamValueRow(ArmSoft.SME.Trade.Common.Parameters.Definitions.CashInNumber);
		span[13] = new GridParamValueRow(ArmSoft.SME.Trade.Common.Parameters.Definitions.CashOutNumber);
		span[14] = new GridParamValueRow(ArmSoft.SME.Trade.Common.Parameters.Definitions.PayOrdNumber);
		span[15] = new GridParamValueRow(ArmSoft.SME.Trade.Common.Parameters.Definitions.NextBankIn);
		span[16] = new GridParamValueRow(ArmSoft.SME.Trade.Common.Parameters.Definitions.NextBankOut);
		span[17] = new GridParamValueRow(ArmSoft.SME.Trade.Common.Parameters.Definitions.MTMoveNumber);
		span[18] = new GridParamValueRow(ArmSoft.SME.Trade.Common.Parameters.Definitions.NextMTMoveWithConfirmationDoc);
		span[19] = new GridParamValueRow(ArmSoft.SME.Trade.Common.Parameters.Definitions.MTMoveOrderNumber);
		span[20] = new GridParamValueRow(ArmSoft.SME.Trade.Common.Parameters.Definitions.MTPurchaseOrderNumber);
		span[21] = new GridParamValueRow(ArmSoft.SME.Trade.Common.Parameters.Definitions.MTExpenseNumber);
		span[22] = new GridParamValueRow(ArmSoft.SME.Trade.Common.Parameters.Definitions.MTInventoryNumber);
		span[23] = new GridParamValueRow(ArmSoft.SME.Trade.Common.Parameters.Definitions.MTInventoryRetailNumber);
		span[24] = new GridParamValueRow(ArmSoft.SME.Trade.Common.Parameters.Definitions.MTRevaluationNumber);
		span[25] = new GridParamValueRow(ArmSoft.SME.Trade.Common.Parameters.Definitions.MTRetailRevaluationNumber);
		span[26] = new GridParamValueRow(ArmSoft.SME.Trade.Common.Parameters.Definitions.MTComplect);
		span[27] = new GridParamValueRow(ArmSoft.SME.Trade.Common.Parameters.Definitions.MTDecomplect);
		span[28] = new GridParamValueRow(ArmSoft.SME.Trade.Common.Parameters.Definitions.Revaluation);
		span[29] = new GridParamValueRow(ArmSoft.SME.Trade.Common.Parameters.Definitions.NextPartnerCredit);
		span[30] = new GridParamValueRow(ArmSoft.SME.Trade.Common.Parameters.Definitions.PartnerRemGroupCorrectNumber);
		span[31] = new GridParamValueRow(ArmSoft.SME.Trade.Common.Parameters.Definitions.MTIncomeRemNumber);
		span[32] = new GridParamValueRow(ArmSoft.SME.Trade.Common.Parameters.Definitions.PartnersGroupRevaluationNumber);
		span[33] = new GridParamValueRow(ArmSoft.SME.Trade.Common.Parameters.Definitions.PartnerRemMoveNumber);
		span[34] = new GridParamValueRow(ArmSoft.SME.Trade.Common.Parameters.Definitions.BonusGroupCorrectNumber);
		span[35] = new GridParamValueRow(ArmSoft.SME.Trade.Common.Parameters.Definitions.ReconcilationStatementNumber);
		List<GridParamValueRow> list2 = list;
		if (Settings.ParametersService.UseMarking())
		{
			list2.Add(new GridParamValueRow(ArmSoft.SME.Trade.Common.Parameters.Definitions.NextEMarkReceiptDoc));
		}
		return list2;
	}

	protected override void FillPanelSystemConfigs()
	{
		//IL_011f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0125: Unknown result type (might be due to invalid IL or missing references)
		if (!_loadedPanels.Contains(_panelSystemConfigs.Name))
		{
			LayoutGroupExt container = OptionsWindowBase.AddLayoutGroup(_panelSystemConfigs, System.Windows.Controls.Orientation.Vertical, ArmSoft.SME.Common.Resources.AutoSave, LayoutGroupView.GroupBox, System.Windows.HorizontalAlignment.Stretch, "AutoSave", isCollapsible: true, isCollapsed: false);
			LayoutGroupExt container2 = OptionsWindowBase.AddLayoutGroup(container, System.Windows.Controls.Orientation.Horizontal, "", LayoutGroupView.Group, System.Windows.HorizontalAlignment.Left);
			_autoSave = AddBooleanControl(_panelSystemConfigs, container2, ArmSoft.SME.Common.Parameters.Definitions.AutoSave, out var usersParamButton, showOnlyUserValue: false, System.Windows.HorizontalAlignment.Left, ArmSoft.SME.Trade.Common.Resources.OptionAutoSave, showCaptionAtRight: true);
			_autoSave.EditValueChanged += new SafeEventHandler<EditValueChangedEventArgs>(AutoSave_EditValueChanged).Handler;
			_autoSaveInterval = AddSpinEditControl(_panelSystemConfigs, container2, ArmSoft.SME.Common.Parameters.Definitions.AutoSaveInterval, 1m, 60m, out usersParamButton, 50.0, System.Windows.HorizontalAlignment.Left, null, showCaptionAtRight: true);
			_autoSaveInterval.IsEnabled = Settings.LoginService.IsAdmin && _autoSave.IsChecked.Value;
			LayoutGroupExt layoutGroupExt = OptionsWindowBase.AddLayoutGroup(container, System.Windows.Controls.Orientation.Horizontal, "", LayoutGroupView.Group);
			layoutGroupExt.VerticalAlignment = VerticalAlignment.Top;
			ImageEdit element = new ImageEdit
			{
				Source = ImageService.GetImage("information_16.svg"),
				Padding = new Thickness(0.0, 0.0, 0.0, 0.0),
				IsEnabled = false,
				ShowBorder = false,
				Stretch = Stretch.Fill,
				VerticalAlignment = VerticalAlignment.Top,
				Margin = new Thickness(0.0, 0.0, 0.0, 0.0),
				Height = 16.0,
				Width = 16.0
			};
			layoutGroupExt.Children.Add(element);
			TextBlock textBlock = OptionsWindowBase.AddTextBlock(layoutGroupExt, ArmSoft.SME.Trade.Common.Resources.AutoSaveInfoMessage, new Thickness(0.0, 0.0, 0.0, 0.0));
			textBlock.Foreground = ColorsAS.Foreground.BlueBrush;
			textBlock.TextWrapping = TextWrapping.WrapWithOverflow;
			textBlock.Width = SizeCalculator.Width(600.0);
			container = OptionsWindowBase.AddLayoutGroup(_panelSystemConfigs, System.Windows.Controls.Orientation.Vertical, "", LayoutGroupView.Group, System.Windows.HorizontalAlignment.Stretch, "QueriesTimeOutGroup");
			AddNumericControl(_panelSystemConfigs, container, ArmSoft.SME.Trade.Common.Parameters.Definitions.OfflineDocumentsKeepDaysCount, isRequired: false, 5, 0, out usersParamButton);
			AddNumericControl(_panelSystemConfigs, container, ArmSoft.SME.Common.Parameters.Definitions.QueryTimeout, isRequired: false, 5, 0, out usersParamButton);
			AddNumericControl(_panelSystemConfigs, container, ArmSoft.SME.Common.Parameters.Definitions.ReportQueryTimeout, isRequired: false, 5, 0, out usersParamButton);
			AddBooleanControl(_panelSystemConfigs, container, ArmSoft.SME.Common.Parameters.Definitions.UseWindowsNotifications, out usersParamButton, showOnlyUserValue: true);
			_participateCEIP = OptionsWindowBase.AddBooleanControl(_panelSystemConfigs, ArmSoft.SME.Common.Resources.ParticipateCEIP);
			_participateCEIP.IsChecked = OptionsWindowBase.IsParamParticipateCEIP();
			container = OptionsWindowBase.AddLayoutGroup(_panelSystemConfigs, System.Windows.Controls.Orientation.Vertical, ArmSoft.SME.Common.Resources.Backups, LayoutGroupView.GroupBox, System.Windows.HorizontalAlignment.Stretch, "BackupGroup");
			LayoutGroupExt container3 = OptionsWindowBase.AddLayoutGroup(container, System.Windows.Controls.Orientation.Horizontal, "", LayoutGroupView.Group, System.Windows.HorizontalAlignment.Left);
			AddBooleanControl(_panelSystemConfigs, container3, ArmSoft.SME.Common.Parameters.Definitions.AutoBackUp, out usersParamButton, showOnlyUserValue: false, System.Windows.HorizontalAlignment.Left, ArmSoft.SME.Common.Resources.OptionAutoBackUp, showCaptionAtRight: true);
			AddSpinEditControl(_panelSystemConfigs, container3, ArmSoft.SME.Common.Parameters.Definitions.AutoBackUpInterval, 1m, 99m, out usersParamButton, 50.0, System.Windows.HorizontalAlignment.Left, null, showCaptionAtRight: true);
			LayoutGroupExt container4 = OptionsWindowBase.AddLayoutGroup(container, System.Windows.Controls.Orientation.Vertical, ArmSoft.SME.Common.Resources.LocalBackups, LayoutGroupView.GroupBox, System.Windows.HorizontalAlignment.Stretch, "LocalBackups", isCollapsible: false, isCollapsed: false);
			_localBackUp = AddBooleanControl(_panelSystemConfigs, container4, ArmSoft.SME.Common.Parameters.Definitions.LocalBackUp, out usersParamButton, showOnlyUserValue: false, System.Windows.HorizontalAlignment.Left, ArmSoft.SME.Common.Resources.OptionLocalBackUpExportPath, showCaptionAtRight: true);
			_localBackUpPath = AddDropDownCommentControl(_panelSystemConfigs, container4, ArmSoft.SME.Common.Parameters.Definitions.LocalBackUpPath, isRequired: false, new SmartEnum(typeof(CheckModeSE)), 256, out usersParamButton, System.Windows.HorizontalAlignment.Left, "");
			_localBackUpPath.ClickDropDown += new SafeEventHandler<DropDownControlEventArgs>(LocalBackUpPath_ClickDropDown).Handler;
			FillAutoBackupProperties(container);
			_loadedPanels.Add(_panelSystemConfigs.Name);
		}
	}

	private void VATType_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		_dealType.DataView = new SmartEnum((!_vatType.Code.InList(VatTypesSE.CostWithVAT.Value, VatTypesSE.CostWithInVAT.Value)) ? typeof(AccDealTypesSE) : typeof(InvoiceDealTypesSE), null, padLeft: false, '0', allSubTypes: false, ValueMember.ValueUI);
		if (((e.OldValue.ToString() == VatTypesSE.CostWithVAT.Value || e.OldValue.ToString() == VatTypesSE.CostWithInVAT.Value) && (e.NewValue.ToString() == VatTypesSE.CostWithoutVAT.Value || e.NewValue.ToString() == VatTypesSE.CostZeroVAT.Value)) || ((e.OldValue.ToString() == VatTypesSE.CostWithoutVAT.Value || e.OldValue.ToString() == VatTypesSE.CostZeroVAT.Value) && (e.NewValue.ToString() == VatTypesSE.CostWithVAT.Value || e.NewValue.ToString() == VatTypesSE.CostWithInVAT.Value)))
		{
			_dealType.Code = "";
		}
	}

	private void DiscountUse_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		_discountExtensionUse.IsEnabled = Settings.LoginService.IsAdmin && _discountUse.IsChecked.Value;
	}

	private void UseBonus_EditValueChanged(object sender, RoutedEventArgs e)
	{
		EnableBonusSettings(_useBonus.IsChecked.Value);
	}

	private void BonusExtensionUse_EditValueChanged(object sender, RoutedEventArgs e)
	{
		EnableBonusSettings(!_bonusExtensionUse.IsChecked.Value, extensionHandler: true);
	}

	private void EnableBonusSettings(bool enable, bool extensionHandler = false)
	{
		enable = enable && Settings.LoginService.IsAdmin;
		if (!extensionHandler)
		{
			_bonusExtensionUse.IsEnabled = enable;
			_bonusType.IsEnabled = enable;
			_connectTob2barmsoftDiscountServer.IsEnabled = enable;
			enable = enable && !_bonusExtensionUse.IsChecked.Value;
		}
		_bonusAccumulationMethod.IsEnabled = enable;
		_accumulateBonusForDiscountedGoods.IsEnabled = enable;
		_bonusAccumulationBase.IsEnabled = enable;
		_bonusAccumulationConditionSumGreater.IsEnabled = enable && Settings.ParametersService.BonusAccumulationBase() != BonusAccumulationBaseSE.WithoutCondition.Value;
		bool isEnabled = enable && Settings.ParametersService.BonusAccumulationBase() == BonusAccumulationBaseSE.AccumulativeSaleSumma.Value;
		_bonusAccumulationConditionPeriodStart.IsEnabled = isEnabled;
		_bonusAccumulationConditionPeriodStart.IsRequired = false;
		_bonusAccumulationConditionPeriodEnd.IsEnabled = isEnabled;
		_bonusAccumulationConditionPeriodEnd.IsRequired = false;
		_noCalcBonusesWhenRePay.IsEnabled = enable;
		_bonusUseMaxPercent.IsEnabled = enable;
	}

	private void BonusAccumulationBase_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		_bonusAccumulationConditionSumGreater.IsRequired = _bonusAccumulationBase.Code != BonusAccumulationBaseSE.WithoutCondition.Value;
		_bonusAccumulationConditionSumGreater.IsEnabled = _bonusAccumulationConditionSumGreater.IsRequired;
		if (_bonusAccumulationConditionSumGreater.IsRequired)
		{
			_bonusAccumulationConditionSumGreater.DoValidate();
		}
		else
		{
			_bonusAccumulationConditionSumGreater.DoValidateWithoutRequired();
		}
		_bonusAccumulationConditionPeriodStart.IsRequired = false;
		_bonusAccumulationConditionPeriodStart.IsEnabled = _bonusAccumulationBase.Code == BonusAccumulationBaseSE.AccumulativeSaleSumma.Value;
		_bonusAccumulationConditionPeriodStart.DoValidateWithoutRequired();
		_bonusAccumulationConditionPeriodEnd.IsRequired = false;
		_bonusAccumulationConditionPeriodEnd.IsEnabled = _bonusAccumulationBase.Code == BonusAccumulationBaseSE.AccumulativeSaleSumma.Value;
		_bonusAccumulationConditionPeriodEnd.DoValidateWithoutRequired();
	}

	private void ConnectTob2barmsoftDiscountServer_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		_syncContractsWithb2barmsoftDiscountServer.IsEnabled = _connectTob2barmsoftDiscountServer.IsChecked.Value;
		if (!_syncContractsWithb2barmsoftDiscountServer.IsEnabled)
		{
			_syncContractsWithb2barmsoftDiscountServer.IsChecked = false;
		}
	}

	private void SyncContractsWithb2barmsoftDiscountServer_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		_syncContractsWithDicountServerInterval.IsEnabled = _syncContractsWithb2barmsoftDiscountServer.IsChecked.Value;
	}

	private void InternalBarcodeEANFormat_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		_internalBarcodePrefix.IsEnabled = _internalBarcodeEANFormat.IsChecked.Value;
	}

	private void BarcodePrefixDelimiterBrowse_Click(object sender, RoutedEventArgs e)
	{
		DialogWindow dialogWindow = new DialogWindow();
		List<ButtonEditExt> prefixList = new List<ButtonEditExt>();
		dialogWindow.Caption = ArmSoft.SME.Trade.Common.Resources.Prefixes;
		dialogWindow.Name = "PrefixDlg";
		dialogWindow.HelpId = "Options_General";
		prefixList.Add(dialogWindow.AddButtonEditControl("Prefix1", ArmSoft.SME.Trade.Common.Resources.Prefix + " 1 ", 7, isRequired: false, 1));
		prefixList.Add(dialogWindow.AddButtonEditControl("Prefix2", ArmSoft.SME.Trade.Common.Resources.Prefix + " 2 ", 7, isRequired: false, 1));
		prefixList.Add(dialogWindow.AddButtonEditControl("Prefix3", ArmSoft.SME.Trade.Common.Resources.Prefix + " 3 ", 7, isRequired: false, 1));
		prefixList.Add(dialogWindow.AddButtonEditControl("Prefix4", ArmSoft.SME.Trade.Common.Resources.Prefix + " 4 ", 7, isRequired: false, 1));
		prefixList.Add(dialogWindow.AddButtonEditControl("Prefix5", ArmSoft.SME.Trade.Common.Resources.Prefix + " 5 ", 7, isRequired: false, 1));
		prefixList.Add(dialogWindow.AddButtonEditControl("Prefix6", ArmSoft.SME.Trade.Common.Resources.Prefix + " 6 ", 7, isRequired: false, 1));
		prefixList.Add(dialogWindow.AddButtonEditControl("Prefix7", ArmSoft.SME.Trade.Common.Resources.Prefix + " 7 ", 7, isRequired: false, 1));
		if (!string.IsNullOrWhiteSpace(_partnerEmployeeCardCodePrefixes.Text))
		{
			string[] array = _partnerEmployeeCardCodePrefixes.Text.Split('|', StringSplitOptions.RemoveEmptyEntries);
			for (int i = 0; i <= array.Length - 1; i++)
			{
				prefixList[i].Text = array[i];
			}
		}
		dialogWindow.WindowValidation += (DialogWindow window, DialogWindowValidationArgs args) =>
		{
			ValidatePrefixContainSymbol(window, args, prefixList);
		};
		dialogWindow.ShowDialog();
	}

	public void ValidatePrefixContainSymbol(DialogWindow window, DialogWindowValidationArgs args, List<ButtonEditExt> prefixList)
	{
		_partnerEmployeeCardCodePrefixes.Text = string.Empty;
		foreach (ButtonEditExt prefix in prefixList)
		{
			if (prefix != null && !string.IsNullOrWhiteSpace(prefix.Text))
			{
				if (prefix.Text.Contains('|'))
				{
					throw new ASException(ArmSoft.SME.Trade.Common.Resources.ValidatePrefixContainSymbol);
				}
				if (!string.IsNullOrWhiteSpace(_partnerEmployeeCardCodePrefixes.Text))
				{
					TextEditExt partnerEmployeeCardCodePrefixes = _partnerEmployeeCardCodePrefixes;
					partnerEmployeeCardCodePrefixes.Text = partnerEmployeeCardCodePrefixes.Text + "||" + prefix.Text;
				}
				else
				{
					_partnerEmployeeCardCodePrefixes.Text += prefix.Text;
				}
			}
		}
	}

	private void UsePartnerCardLenght_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		_partnerCardLenght.IsEnabled = _usePartnerCardLenght.IsChecked.Value;
	}

	private void AutoSave_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		_autoSaveInterval.IsEnabled = _autoSave.IsChecked.Value;
	}

	private void LocalBackUpPath_ClickDropDown(object sender, DropDownControlEventArgs e)
	{
		FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog
		{
			SelectedPath = _localBackUpPath.Text,
			Description = ArmSoft.SME.Common.Resources.SelectFolder,
			UseDescriptionForTitle = true
		};
		if (folderBrowserDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
		{
			_localBackUpPath.Text = folderBrowserDialog.SelectedPath;
		}
		e.Cancel = true;
	}

	protected override void StoreOptionsControlsInner(string panelName)
	{
		base.StoreOptionsControlsInner(panelName);
		if (panelName == _panelGeneral.Name)
		{
			StoreWorkPeriodParams();
			StoreWeightItemSettings();
			StorePieceItemSettings();
		}
		if (panelName == _panelDevices.Name)
		{
			StoreDevicesOptions();
		}
	}

	private void StoreWorkPeriodParams()
	{
		if (Settings.LoginService.IsAdmin)
		{
			if (_dateBegin.EditValue != Settings.ParametersService.GetGeneralValue(ArmSoft.SME.Trade.Common.Parameters.Definitions.DateBegin.Id).YYYYMMDDToDateOnly())
			{
				_parameterStoreRequest.Add(new ParameterStoreRequestModel
				{
					Id = ArmSoft.SME.Trade.Common.Parameters.Definitions.DateBegin.Id,
					Value = Convertor.ConvertDateOnlyToString(_dateBegin.EditValue)
				});
			}
			if (_dateEnd.EditValue != Settings.ParametersService.GetGeneralValue(ArmSoft.SME.Trade.Common.Parameters.Definitions.DateEnd.Id).YYYYMMDDToDateOnly())
			{
				_parameterStoreRequest.Add(new ParameterStoreRequestModel
				{
					Id = ArmSoft.SME.Trade.Common.Parameters.Definitions.DateEnd.Id,
					Value = Convertor.ConvertDateOnlyToString(_dateEnd.EditValue)
				});
			}
		}
	}

	private void StoreWeightItemSettings()
	{
		WeightItemInfo instance = WeightItemInfo.GetInstance();
		if (_weightItemPrefix.IsEnabled)
		{
			instance.Prefix = Convert.ToString(_weightItemPrefix.EditValue);
		}
		if (_weightItemCodeLength.IsEnabled)
		{
			instance.CodeLength = Convert.ToInt16(_weightItemCodeLength.Value);
		}
		if (_weightItemPrecision.IsEnabled)
		{
			instance.Precision = Convert.ToInt16(_weightItemPrecision.Value);
		}
		if (!_weightItemUnit.IsReadOnly && _weightItemUnit.DoValidate())
		{
			instance.Unit = _weightItemUnit.Code;
		}
		instance.StoreSettings();
	}

	private void StorePieceItemSettings()
	{
		PieceItemInfo instance = PieceItemInfo.GetInstance();
		if (_pieceItemPrefix.IsEnabled)
		{
			instance.Prefix = Convert.ToString(_pieceItemPrefix.EditValue);
		}
		if (_pieceItemCodeLength.IsEnabled)
		{
			instance.CodeLength = Convert.ToInt16(_pieceItemCodeLength.Value);
		}
		if (_pieceItemPrecision.IsEnabled)
		{
			instance.Precision = Convert.ToInt16(_pieceItemPrecision.Value);
		}
		if (!_pieceItemUnit.IsReadOnly && _pieceItemUnit.DoValidate())
		{
			instance.Unit = _pieceItemUnit.Code;
		}
		instance.StoreSettings();
	}

	private void StoreDevicesOptions()
	{
		_devicesSettings.StoreSettings();
	}

	protected override void Validate()
	{
		base.Validate();
		if (_loadedPanels.Contains(_panelGeneral.Name))
		{
			CheckWorkPeriod();
			CheckRetailAccounting();
			CheckMTMoveWithConfirmation();
			CheckInternalBarcodeEANFormat();
			CheckBonusOptions();
			CheckMTCurr();
		}
	}

	private void CheckWorkPeriod()
	{
		if (_dateBegin.EditValue > _dateEnd.EditValue)
		{
			throw new ASException(ArmSoft.SME.Common.Resources.InvalidWorkingPeriod);
		}
	}

	private void CheckRetailAccounting()
	{
		if (_retailAccounting.IsChecked.Value)
		{
			return;
		}
		foreach (StoragesCacheRow item in TradeSettings.TradeCacheService.Storage.GetList())
		{
			if (item.Retail)
			{
				throw new ASException(ArmSoft.SME.Trade.Common.Resources.RetailAccountingDeactivateErr);
			}
		}
	}

	private void CheckMTMoveWithConfirmation()
	{
		if (!_mtMoveWithConfirmation.IsChecked.Value && TradeSettings.CreateTradeClient().CommonServices.ExistAnyDocByType(ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTMoveWithConfirmation.Value))
		{
			throw new ASException(string.Format(ArmSoft.SME.Trade.Common.Resources.OptionCannotBeDeactivatedError, ArmSoft.SME.Trade.Common.Parameters.Definitions.MTMoveWithConfirmation.Caption));
		}
	}

	private void CheckInternalBarcodeEANFormat()
	{
		if (_internalBarcodeEANFormat.IsChecked.Value)
		{
			if (Convert.ToInt16(_weightItemPrefix.EditValue) == Convert.ToInt16(_internalBarcodePrefix.EditValue))
			{
				throw new ASException(ArmSoft.SME.Trade.Common.Resources.ErrInternalWeightBarcodesPrefix);
			}
			if (string.IsNullOrWhiteSpace(_internalBarcodeNext.Text))
			{
				return;
			}
			if (_internalBarcodeNext.Text.Length != 10)
			{
				throw new ASException(string.Format(ArmSoft.SME.Trade.Common.Resources.ErrInternalBarcodeLength, (short)10));
			}
			string text = _internalBarcodeNext.Text;
			for (int i = 0; i <= text.Length - 1; i++)
			{
				if (!int.TryParse(text[i].ToString(), out var _))
				{
					throw new ASException(ArmSoft.SME.Trade.Common.Resources.ErrInternalBarcodeNumeric);
				}
			}
		}
		else if (!string.IsNullOrWhiteSpace(_internalBarcodeNext.Text))
		{
			if (_internalBarcodeNext.Text.Length < 4)
			{
				throw new ASException(string.Format(ArmSoft.SME.Trade.Common.Resources.ErrInternalBarcodeLenMin, (short)4));
			}
			if (_internalBarcodeNext.Text.Length > 20)
			{
				throw new ASException(string.Format(ArmSoft.SME.Trade.Common.Resources.ErrInternalBarcodeLenMax, (short)20));
			}
		}
	}

	private void CheckBonusOptions()
	{
		if (_bonusAccumulationBase.Code != BonusAccumulationBaseSE.WithoutCondition.Value && _bonusAccumulationConditionSumGreater.IsEnabled && _bonusAccumulationConditionSumGreater.IsRequired && !_bonusAccumulationConditionSumGreater.DoValidate())
		{
			throw new ASException(string.Format(ArmSoft.SME.Common.Resources.FieldValueGraterZero, ArmSoft.SME.Trade.Common.Resources.BonusAccumulationConditionSumGreater));
		}
		if (Settings.LoginService.IsAdmin && _bonusAccumulationConditionPeriodStart.EditValue > _bonusAccumulationConditionPeriodEnd.EditValue)
		{
			throw new ASException(ArmSoft.SME.Common.Resources.WrongDatePeriod, _bonusAccumulationConditionPeriodStart.Name);
		}
	}

	private void CheckMTCurr()
	{
		if (_mtCurr.IsEnabled && _mtCurr.Code != OptionsWindowBase.ParametersFromService[ArmSoft.SME.Trade.Common.Parameters.Definitions.MTCurr.Id] && TradeSettings.CreateTradeClient().GoodsEngineServices.ExistOperation())
		{
			_mtCurr.Code = Settings.ParametersService.MTCurr();
		}
	}
}
