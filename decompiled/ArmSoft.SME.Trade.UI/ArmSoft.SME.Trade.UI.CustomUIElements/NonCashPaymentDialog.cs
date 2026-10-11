using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Ardalis.SmartEnum;
using ArmSoft.SME.Common;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Core.UI.Controls;
using ArmSoft.SME.Trade.Common;
using ArmSoft.SME.Trade.Common.Models.Caches;
using ArmSoft.SME.Trade.Common.SmartEnums;
using ArmSoft.SME.Trade.UI.Helpers;
using DevExpress.Xpf.Bars;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Editors;

namespace ArmSoft.SME.Trade.UI.CustomUIElements;

public class NonCashPaymentDialog : DialogWindow
{
	private ComboBoxExt _posTerminalTypes;

	private DropDownButton _cashDeskPOSTerminals;

	private string _partCode;

	private PartnerContractGroupControls _bankAcquiringContainer;

	private ComboBoxExt _nonCashPaymentMethods;

	private Button _loadSum;

	private PopupCalcEditExt _nonCashPaySum;

	private bool _editMode;

	private decimal _remainSum;

	public decimal EnteredNonCashSum { get; set; }

	public short NonCashPaymentMethod { get; set; }

	public string EnteredBankAcquiringPartnerCode { get; set; }

	public string EnteredBankAcquiringContractCode { get; set; }

	public string POSTerminalType { get; private set; } = "0";

	public string POSTerminalID { get; set; } = "";

	public string POSTerminalAddress { get; set; } = "";

	private string _cashDesk { get; set; } = "";

	public NonCashPaymentDialog(Window owner, decimal plCardPaySum, decimal remainSum, short nonCashPaymentMethod, bool editMode, string bankAcquiringPartnerCode, string bankAcquiringContractCode, string cashdeskCode = "", string posTerminalType = "0", string partCode = "", string posTerminalID = "", string posTerminalAddress = "")
	{
		Owner = owner;
		EnteredNonCashSum = plCardPaySum;
		_remainSum = remainSum;
		NonCashPaymentMethod = nonCashPaymentMethod;
		_editMode = editMode;
		EnteredBankAcquiringPartnerCode = bankAcquiringPartnerCode;
		EnteredBankAcquiringContractCode = bankAcquiringContractCode;
		_cashDesk = cashdeskCode;
		POSTerminalType = posTerminalType;
		_partCode = partCode;
		POSTerminalID = posTerminalID;
		POSTerminalAddress = posTerminalAddress;
		InnerInitialize();
	}

	private void InnerInitialize()
	{
		Caption = ArmSoft.SME.Trade.Common.Resources.PlasticCartPaymentDialog;
		Name = "PlCardPayment";
		if (NonCashPaymentMethod == 0 && !_editMode)
		{
			CashCacheRow item = TradeSettings.TradeCacheService.Cash.GetItem(_cashDesk);
			if (item != null && item.NonCashInfoDict.TryGetValue(5, out var value))
			{
				NonCashPaymentMethod = value.ECRPaymentMethod;
			}
		}
		_bankAcquiringContainer = new PartnerContractGroupControls(this);
		LayoutGroupExt lgPartCont = AddVerticalLayoutGroup();
		_bankAcquiringContainer.ConstructDlgUI(lgPartCont, storeValue: true, ArmSoft.SME.Trade.Common.Resources.POSTerminalPart, isFilterBySuppliers: false, isFilterByCustomers: false, isShowContractCurrCode: false, isShowSummaDebt: false, ArmSoft.SME.Trade.Common.Resources.POSTerminalContract, "", showOnlyBonusContracts: false, showOnlyCurCodeNCC: true);
		((LayoutItemWithRequired)(object)_bankAcquiringContainer.PartnerGroup.Parent).Visibility = Visibility.Collapsed;
		LayoutGroupExt layoutGroupExt = AddHorizontalLayoutGroup();
		layoutGroupExt.HorizontalAlignment = HorizontalAlignment.Stretch;
		_posTerminalTypes = new ComboBoxExt
		{
			ItemsSource = (from x in SmartEnum<POSTerminalTypesSE, string>.List
				orderby x.Value
				select x.Caption).ToList()
		};
		AddControl(layoutGroupExt, "POSTerminalType", ArmSoft.SME.Trade.Common.Resources.PluggedDevice, _posTerminalTypes);
		_posTerminalTypes.SelectedItem = POSTerminalHelpers.GetPOSTerminalTypeCaptionByCode(POSTerminalType);
		IEnumerable<POSTerminalHelpers.POSAcquiringInfoAsListItem> pOSInfoByCashDesk = POSTerminalHelpers.GetPOSInfoByCashDesk(_cashDesk);
		List<BarButtonItem> list = new List<BarButtonItem>();
		if (pOSInfoByCashDesk != null)
		{
			foreach (POSTerminalHelpers.POSAcquiringInfoAsListItem item2 in pOSInfoByCashDesk)
			{
				BarButtonItem barButtonItem = new BarButtonItem
				{
					Content = item2
				};
				barButtonItem.ItemClick += new SafeEventHandler<ItemClickEventArgs>(CashDeskPOSTerminals_SelectionChanged).Handler;
				list.Add(barButtonItem);
			}
		}
		_cashDeskPOSTerminals = AddDropDownButton(layoutGroupExt, "CashDeskPOSTerminals", "fill_left_16.svg", ArmSoft.SME.Common.Resources.ButtonLoad, "", "", isTabStop: false, HorizontalAlignment.Left, 16.0, 16.0, list);
		_cashDeskPOSTerminals.Width = 100.0;
		_cashDeskPOSTerminals.Margin = new Thickness(18.0, 0.0, 0.0, 0.0);
		LayoutGroupExt container = AddHorizontalLayoutGroup();
		_nonCashPaySum = AddNumericControl(container, "NonCashPaySum", ArmSoft.SME.Common.Resources.Summa, 16, 2);
		_nonCashPaySum.EditValue = Math.Abs(EnteredNonCashSum);
		_loadSum = AddButton(container, "Fill", "fill_left_16.svg", "", "", ArmSoft.SME.Common.Resources.ButtonLoad);
		_loadSum.Click += new SafeEventHandler<RoutedEventArgs>(LoadSum_Click).Handler;
		string paymentSystem = ArmSoft.SME.Common.Resources.PaymentSystem;
		_nonCashPaymentMethods = new ComboBoxExt
		{
			ItemTemplate = (DataTemplate)Application.Current.Resources["PlasticCardTemplate"],
			ItemsSource = NonCashPaymentHelpers.GetNonCashPayments()
		};
		AddControl("PlasticCartType", paymentSystem, _nonCashPaymentMethods, storeValue: true);
		_nonCashPaymentMethods.IsEnabled = true;
		_nonCashPaymentMethods.SelectedItem = NonCashPaymentHelpers.GetPaymentItem(NonCashPaymentMethod);
		_nonCashPaymentMethods.Width = _nonCashPaySum.Width - 18.0;
		if (_posTerminalTypes != null)
		{
			_posTerminalTypes.Width = _nonCashPaySum.Width - 18.0;
		}
		if (_editMode)
		{
			_nonCashPaySum.IsReadOnly = true;
			_nonCashPaymentMethods.IsReadOnly = true;
			_nonCashPaymentMethods.IsEnabled = false;
			_loadSum.IsEnabled = false;
			_posTerminalTypes.IsReadOnly = true;
			_posTerminalTypes.IsEnabled = false;
			_cashDeskPOSTerminals.IsEnabled = false;
			Mode = DialogWindowMode.ViewMode;
		}
		SetFocusOnActivation = _nonCashPaySum;
		_bankAcquiringContainer.PartnerCode.Code = EnteredBankAcquiringPartnerCode;
		_bankAcquiringContainer.Contract.Code = EnteredBankAcquiringContractCode;
		_bankAcquiringContainer.PartnerCode.IsReadOnly = _editMode;
		_bankAcquiringContainer.Contract.IsReadOnly = _editMode;
		_bankAcquiringContainer.PartnerCode.IsRequired = !_editMode;
		_bankAcquiringContainer.Contract.IsRequired = !_editMode;
		_nonCashPaySum.EditValueChanged += new SafeEventHandler<EditValueChangedEventArgs>(NonCashPaySum_EditValueChanged).Handler;
		WindowValidation += new SafeEventHandler<DialogWindowValidationArgs>(NonCashPaymentDialog_WindowValidation).Handler;
		_bankAcquiringContainer.PartnerCodeEditValueChanged += new SafeEventHandler<EditValueChangedEventArgs>(BankAcquiringContainer_PartnerCodeEditValueChanged).Handler;
	}

	private void CashDeskPOSTerminals_SelectionChanged(object sender, ItemClickEventArgs e)
	{
		POSTerminalHelpers.POSAcquiringInfoAsListItem pOSAcquiringInfoAsListItem = (POSTerminalHelpers.POSAcquiringInfoAsListItem)((BarButtonItem)sender).Content;
		if (pOSAcquiringInfoAsListItem != null)
		{
			if (!_posTerminalTypes.IsEnabled)
			{
				_posTerminalTypes.IsEnabled = true;
				_nonCashPaymentMethods.SelectedIndex = 0;
			}
			_posTerminalTypes.SelectedItem = POSTerminalHelpers.GetPOSTerminalTypeCaptionByCode(pOSAcquiringInfoAsListItem.Device);
			POSTerminalType = pOSAcquiringInfoAsListItem.Device;
			_nonCashPaymentMethods.SelectedItem = NonCashPaymentHelpers.GetPaymentItem(pOSAcquiringInfoAsListItem.NonCashPaymentMethod);
			POSTerminalID = pOSAcquiringInfoAsListItem.POSTerminalID;
			POSTerminalAddress = pOSAcquiringInfoAsListItem.POSTerminalAddress;
			_bankAcquiringContainer.Contract.Code = pOSAcquiringInfoAsListItem.Contract;
			_bankAcquiringContainer.PartnerCode.Code = pOSAcquiringInfoAsListItem.Partner;
		}
	}

	private void NonCashPaySum_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		if (_nonCashPaySum.Value == 0m)
		{
			_nonCashPaymentMethods.SelectedIndex = 0;
		}
	}

	private void NonCashPaymentDialog_WindowValidation(object sender, DialogWindowValidationArgs e)
	{
		try
		{
			EnteredNonCashSum = _nonCashPaySum.Value;
			NonCashPaymentMethod = ((NonCashPaymentHelpers.NonCashPaymentItem)_nonCashPaymentMethods.SelectedItem).PaymentType.Value;
			EnteredBankAcquiringPartnerCode = _bankAcquiringContainer.PartnerCode.Code;
			EnteredBankAcquiringContractCode = _bankAcquiringContainer.Contract.Code;
			string caption = _posTerminalTypes.SelectedItem.ToString();
			POSTerminalType = POSTerminalHelpers.GetPOSTerminalTypeCodeByCaption(caption);
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
			e.isValid = false;
			e.SetFocusWhenInvalidSave = _nonCashPaySum;
		}
	}

	private void LoadSum_Click(object sender, RoutedEventArgs e)
	{
		if (!_nonCashPaySum.IsReadOnly)
		{
			_nonCashPaySum.EditValue = _remainSum;
		}
	}

	private void BankAcquiringContainer_PartnerCodeEditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		if (!string.IsNullOrWhiteSpace(_bankAcquiringContainer.PartnerCode.Code) && !string.IsNullOrWhiteSpace(_bankAcquiringContainer.PartnerCode.Code))
		{
			PartnerCacheRow.ContractRow defaultContract = TradeSettings.TradeCacheService.Partner.GetDefaultContract(_bankAcquiringContainer.PartnerCode.Code);
			if (defaultContract != null)
			{
				_bankAcquiringContainer.Contract.Code = defaultContract.Contract;
			}
		}
	}
}
