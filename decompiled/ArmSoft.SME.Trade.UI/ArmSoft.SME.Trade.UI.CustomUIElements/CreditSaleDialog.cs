using System;
using System.Windows;
using ArmSoft.SME.Common;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Core.UI.Controls;
using ArmSoft.SME.Trade.Common;
using ArmSoft.SME.Trade.Common.Models.Caches;

namespace ArmSoft.SME.Trade.UI.CustomUIElements;

public class CreditSaleDialog : DialogWindow
{
	private PartnerContractGroupControls _buyerContainer;

	private PopupCalcEditExt _creditSum;

	private TextEditExt _discountCard;

	private string _buyerCode;

	private string _contractCode;

	private bool _editMode;

	private decimal _remainSum;

	public decimal EnteredCreditSum { get; set; }

	public CreditSaleDialog(Window owner, decimal creditSum, decimal remainSum, string buyerCode, string contractCode, bool editMode)
	{
		Owner = owner;
		EnteredCreditSum = creditSum;
		_remainSum = remainSum;
		_buyerCode = buyerCode;
		_contractCode = contractCode;
		_editMode = editMode;
		InnerInitialize();
	}

	private void InnerInitialize()
	{
		Caption = ArmSoft.SME.Trade.Common.Resources.CreditSale;
		Name = "CreditSale";
		LayoutGroupExt lgPartCont = AddVerticalLayoutGroup();
		_buyerContainer = new PartnerContractGroupControls(this);
		_buyerContainer.ConstructDlgUI(lgPartCont, storeValue: true, ArmSoft.SME.Common.Resources.Buyer, isFilterBySuppliers: false, isFilterByCustomers: true, isShowContractCurrCode: true, isShowSummaDebt: true, "", "", showOnlyBonusContracts: false, showOnlyCurCodeNCC: true, connectTob2bArmsoftDiscountServerforBonusSum: false, "", showRefreshButtonCaption: true);
		((LayoutItemWithRequired)(object)_buyerContainer.PartnerGroup.Parent).Visibility = Visibility.Collapsed;
		_discountCard = AddStringControl("DiscountCard", ArmSoft.SME.Trade.Common.Resources.DiscountCardCode, 50);
		_discountCard.IsReadOnly = true;
		if (!string.IsNullOrWhiteSpace(_buyerCode))
		{
			PartnerCacheRow.ContractRow contract = TradeSettings.TradeCacheService.Partner.GetContract(_buyerCode, _contractCode);
			if (contract != null)
			{
				_discountCard.Text = contract.DiscountCard;
			}
		}
		_buyerContainer.PartnerCode.Code = _buyerCode;
		_buyerContainer.Contract.Code = _contractCode;
		_buyerContainer.PartnerCode.IsReadOnly = true;
		_buyerContainer.Contract.IsReadOnly = true;
		_buyerContainer.Contract.IsRequired = !_editMode;
		LayoutGroupExt container = AddHorizontalLayoutGroup();
		_creditSum = AddNumericControl(container, "CreditSum", ArmSoft.SME.Common.Resources.Summa, 16, 2, isRequired: false, allowNegativeNumbers: true);
		_creditSum.EditValue = Math.Abs(EnteredCreditSum);
		AddButton(container, "Fill", "fill_left_16.svg", "", "", ArmSoft.SME.Common.Resources.ButtonLoad).Click += new SafeEventHandler<RoutedEventArgs>(AutoFillButton_Click).Handler;
		if (_editMode)
		{
			_creditSum.IsReadOnly = true;
			_buyerContainer.PartnerCode.IsReadOnly = true;
			_buyerContainer.Contract.IsReadOnly = true;
			_buyerContainer.ContractCurrCode.IsReadOnly = true;
			Mode = DialogWindowMode.ViewMode;
		}
		SetFocusOnActivation = _creditSum;
		WindowValidation += new SafeEventHandler<DialogWindowValidationArgs>(CreditSaleDialog_WindowValidation).Handler;
		_buyerContainer.ContractCodeEditValueChanged += new SafeEventHandler<EventArgs>(BuyerContainer_ContractCodeEditValueChanged).Handler;
	}

	private void AutoFillButton_Click(object sender, RoutedEventArgs e)
	{
		SumAutoFill();
	}

	private void BuyerContainer_ContractCodeEditValueChanged(object sender, EventArgs e)
	{
		PartnerCacheRow.ContractRow contract = TradeSettings.TradeCacheService.Partner.GetContract(_buyerCode, _buyerContainer.Contract.Code);
		if (contract != null)
		{
			_discountCard.Text = contract.DiscountCard;
		}
	}

	private void CreditSaleDialog_WindowValidation(object sender, DialogWindowValidationArgs e)
	{
		EnteredCreditSum = _creditSum.Value;
	}

	private void SumAutoFill()
	{
		if (!_creditSum.IsReadOnly)
		{
			_creditSum.EditValue = _remainSum;
		}
	}
}
