using System;
using System.Windows;
using ArmSoft.SME.Common;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Core.UI.Controls;
using ArmSoft.SME.Trade.Client.Routes;
using ArmSoft.SME.Trade.Common;
using ArmSoft.SME.Trade.Common.Models.Caches;
using ArmSoft.SME.Trade.UI.CustomUIElements.UserControls;
using DevExpress.Xpf.Bars;
using DevExpress.Xpf.Editors;

namespace ArmSoft.SME.Trade.UI.CustomUIElements;

public class UsePrepaymentDialog : DialogWindow
{
	public string PrepaymentPartnerCode = "";

	public string PrepaymentContractCode = "";

	private PartnerContractGroupControls _buyerContainer;

	private PopupCalcEditExt _prePaymentSum;

	private readonly string _buyerCode;

	private readonly string _contractCode;

	private readonly bool _editMode;

	private readonly decimal _remainSum;

	private readonly decimal _summaChange;

	private CheckEditExt _editPartner;

	public decimal EnteredPrePaymentSum { get; set; }

	public UsePrepaymentDialog(Window owner, decimal prePaymentSum, decimal remainSum, string buyerCode, string contractCode, bool editMode, string prepaymentPartner, string prepaymentContract, decimal summaChange)
	{
		Owner = owner;
		EnteredPrePaymentSum = prePaymentSum;
		_remainSum = remainSum;
		_buyerCode = buyerCode;
		_contractCode = contractCode;
		PrepaymentPartnerCode = prepaymentPartner;
		PrepaymentContractCode = prepaymentContract;
		_editMode = editMode;
		_summaChange = summaChange;
		InnerInitialize();
	}

	private void InnerInitialize()
	{
		Caption = ArmSoft.SME.Trade.Common.Resources.UsePrePayment;
		Name = "UsePrePayment";
		bool flag = !string.IsNullOrWhiteSpace(_buyerCode);
		if (!_editMode)
		{
			_editPartner = AddBooleanControl("EditPartner", ArmSoft.SME.Common.Resources.Edit);
			_editPartner.EditValueChanged += new SafeEventHandler<EditValueChangedEventArgs>(EditPartner_EditValueChanged).Handler;
		}
		LayoutGroupExt lgPartCont = AddVerticalLayoutGroup();
		_buyerContainer = new PartnerContractGroupControls(this);
		_buyerContainer.ConstructDlgUI(lgPartCont, storeValue: true, ArmSoft.SME.Common.Resources.Buyer, isFilterBySuppliers: false, isFilterByCustomers: true, isShowContractCurrCode: true, isShowSummaDebt: true, "", "", showOnlyBonusContracts: false, showOnlyCurCodeNCC: true, connectTob2bArmsoftDiscountServerforBonusSum: false, "", showRefreshButtonCaption: true);
		((LayoutItemWithRequired)(object)_buyerContainer.PartnerGroup.Parent).Visibility = Visibility.Collapsed;
		_buyerContainer.PartnerCode.Code = ((!string.IsNullOrWhiteSpace(PrepaymentPartnerCode)) ? PrepaymentPartnerCode : _buyerCode);
		_buyerContainer.Contract.Code = ((!string.IsNullOrWhiteSpace(PrepaymentPartnerCode)) ? PrepaymentContractCode : _contractCode);
		_buyerContainer.IsReadOnly = true;
		LayoutGroupExt container = AddHorizontalLayoutGroup();
		_prePaymentSum = AddNumericControl(container, "PrePaymentSum", ArmSoft.SME.Common.Resources.Summa, 16, 2, isRequired: false, allowNegativeNumbers: true);
		_prePaymentSum.EditValue = Math.Abs(EnteredPrePaymentSum);
		BarButtonItem barButtonItem = new BarButtonItem
		{
			Content = ArmSoft.SME.Trade.Common.Resources.DebtSum
		};
		barButtonItem.ItemClick += new SafeEventHandler<ItemClickEventArgs>(FillDebtButton_ItemClick).Handler;
		AddSplitButton(container, "Fill", "fill_left_16.svg", ArmSoft.SME.Common.Resources.ButtonLoad, "", ArmSoft.SME.Common.Resources.ButtonLoad, isTabStop: false, HorizontalAlignment.Left, 16.0, 16.0, new BarButtonItem[1] { barButtonItem }, showGlyphsInPopupMenu: false).Click += new SafeEventHandler<RoutedEventArgs>(AutoFillButton_Click).Handler;
		if (_editMode)
		{
			_prePaymentSum.IsReadOnly = true;
			if (flag)
			{
				_buyerContainer.PartnerCode.IsReadOnly = true;
				_buyerContainer.Contract.IsReadOnly = true;
				_buyerContainer.ContractCurrCode.IsReadOnly = true;
			}
			Mode = DialogWindowMode.ViewMode;
		}
		SetFocusOnActivation = _prePaymentSum;
		WindowValidation += new SafeEventHandler<DialogWindowValidationArgs>(UsePrePaymentDialog_WindowValidation).Handler;
		SafeEventHandler<EventArgs> safeEventHandler = new SafeEventHandler<EventArgs>(PartnerCodeEditValueChanged);
		new SafeEventHandler<EventArgs>(ContractCodeEditValueChanged);
		_buyerContainer.PartnerCodeEditValueChanged += safeEventHandler.Handler;
		_buyerContainer.PartnerCodeEditAdditionalValueChanged += safeEventHandler.Handler;
	}

	private void FillDebtButton_ItemClick(object sender, ItemClickEventArgs e)
	{
		SumAutoFill(fillDebt: true);
	}

	private void AutoFillButton_Click(object sender, RoutedEventArgs e)
	{
		SumAutoFill(fillDebt: false);
	}

	private void UsePrePaymentDialog_WindowValidation(object window, DialogWindowValidationArgs args)
	{
		EnteredPrePaymentSum = _prePaymentSum.Value;
		bool flag = _buyerCode.Trim() == _buyerContainer.Contract.Code.Trim() && _contractCode.Trim() == _buyerContainer.Contract.Code.Trim();
		PrepaymentPartnerCode = (flag ? "" : _buyerContainer.PartnerCode.Code);
		PrepaymentContractCode = (flag ? "" : _buyerContainer.Contract.Code);
	}

	private void SumAutoFill(bool fillDebt)
	{
		if (!_prePaymentSum.IsReadOnly)
		{
			if (!fillDebt)
			{
				_prePaymentSum.EditValue = _remainSum;
			}
			else if (_buyerContainer.SummaPartDebt.Value != 0m)
			{
				_prePaymentSum.EditValue = ((_buyerContainer.SummaPartDebt.Value > 0m) ? 0m : Math.Min(-_buyerContainer.SummaPartDebt.Value, _summaChange));
			}
		}
	}

	private void EditPartner_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		if (_editPartner.IsChecked.HasValue && _editPartner.IsChecked.Value)
		{
			_buyerContainer.PartnerCode.IsReadOnly = false;
			_buyerContainer.Contract.IsReadOnly = false;
			_buyerContainer.PartnerCode.IsRequired = true;
			if (!string.IsNullOrWhiteSpace(_buyerContainer.PartnerCode.Code))
			{
				_buyerContainer.Contract.IsRequired = true;
			}
		}
		else
		{
			_buyerContainer.PartnerCode.IsReadOnly = true;
			_buyerContainer.Contract.IsReadOnly = true;
			_buyerContainer.PartnerCode.IsRequired = false;
		}
	}

	private void PartnerCodeEditValueChanged(object sender, EventArgs e)
	{
		if (!string.IsNullOrWhiteSpace(_buyerContainer.PartnerCode.Code) && _buyerContainer.PartnerCode.DoValidate())
		{
			_buyerContainer.Contract.IsRequired = true;
			PartnerCacheRow item = TradeSettings.TradeCacheService.Partner.GetItem(_buyerContainer.PartnerCode.Code);
			if (item != null)
			{
				PartnerCacheRow.ContractRow defaultContract = TradeSettings.TradeCacheService.Partner.GetDefaultContract(item, "AMD");
				if (defaultContract != null)
				{
					_buyerContainer.Contract.Code = defaultContract.Contract;
					_buyerContainer.Contract.IsEnabled = true;
					SummCurrSummNccPair currentPartnerRemByContract = TradeSettings.CreateTradeClient().CommonServices.GetCurrentPartnerRemByContract(item.Id, _buyerContainer.Contract.Code, "");
					_buyerContainer.SummaPartDebt.EditValue = currentPartnerRemByContract.SummCurr;
				}
			}
		}
		else if (string.IsNullOrWhiteSpace(_buyerContainer.PartnerCode.Code))
		{
			_buyerContainer.Contract.IsRequired = false;
		}
	}

	private void ContractCodeEditValueChanged(object sender, EventArgs e)
	{
		if (!string.IsNullOrWhiteSpace(_buyerContainer.PartnerCode.Code) && _buyerContainer.PartnerCode.DoValidate())
		{
			PartnerCacheRow item = TradeSettings.TradeCacheService.Partner.GetItem(_buyerContainer.PartnerCode.Code);
			if (item != null && !string.IsNullOrWhiteSpace(_buyerContainer.Contract.Code) && _buyerContainer.Contract.DoValidate())
			{
				_buyerContainer.Contract.Code = _buyerContainer.Contract.Code;
				_buyerContainer.Contract.IsEnabled = true;
				SummCurrSummNccPair currentPartnerRemByContract = TradeSettings.CreateTradeClient().CommonServices.GetCurrentPartnerRemByContract(item.Id, _buyerContainer.Contract.Code, "");
				_buyerContainer.SummaPartDebt.EditValue = currentPartnerRemByContract.SummCurr;
			}
		}
		else if (string.IsNullOrWhiteSpace(_buyerContainer.PartnerCode.Code))
		{
			_buyerContainer.Contract.IsRequired = false;
		}
	}

	public override void OnScanner(string scannerData)
	{
		try
		{
			if (!_editMode && !string.IsNullOrWhiteSpace(scannerData) && MCRInfo.GetInstance().IsValidCard(scannerData))
			{
				if (!_editMode && _editPartner != null)
				{
					_editPartner.IsChecked = true;
				}
				string cardNumber = MCRInfo.GetInstance().GetCardNumber(scannerData);
				PartnerCacheRow.ContractRow contractByDiscountCard = TradeSettings.TradeCacheService.Partner.GetContractByDiscountCard(cardNumber, out var partnerCacheRow);
				if (partnerCacheRow.PartCode.Trim() != _buyerContainer.PartnerCode.Code.Trim() && contractByDiscountCard != null)
				{
					_buyerContainer.Contract.Code = contractByDiscountCard.Contract;
					_buyerContainer.ContractCurrCode.Code = contractByDiscountCard.CurrCode;
					_buyerContainer.PartnerCode.Code = partnerCacheRow.PartCode;
				}
			}
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}
}
