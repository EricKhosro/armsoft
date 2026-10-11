using System;
using System.Windows;
using System.Windows.Controls;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Extensions;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Core.UI.Controls;
using ArmSoft.SME.Trade.Common;
using ArmSoft.SME.Trade.Common.Models.Caches;

namespace ArmSoft.SME.Trade.UI.CustomUIElements;

public class BonusUseDialog : DialogWindow
{
	private PartnerContractGroupControls BuyerContainer;

	private PopupCalcEditExt _usedBonusSumm;

	private TextEditExt _bonusCard;

	private Button _loadSumm;

	private readonly string _buyerCode;

	private readonly string _contractCode;

	private readonly bool _editMode;

	private readonly decimal _sum;

	private readonly decimal _totalSum;

	private readonly decimal _bonusUseMaxPercent;

	private readonly bool _connectTob2barmsoftDiscountServerforBonusSum;

	public decimal UsedBonusSumm { get; set; }

	public BonusUseDialog(Window owner, decimal summaForPay, decimal totalSum, string buyerCode, string contractCode, bool editMode, bool connectTob2barmsoftDiscountServer, decimal bonusUseMaxPercent)
	{
		Owner = owner;
		_sum = summaForPay;
		_totalSum = totalSum;
		_buyerCode = buyerCode;
		_contractCode = contractCode;
		_editMode = editMode;
		_connectTob2barmsoftDiscountServerforBonusSum = connectTob2barmsoftDiscountServer;
		_bonusUseMaxPercent = bonusUseMaxPercent;
		InnerInitialize();
	}

	private void InnerInitialize()
	{
		Caption = ArmSoft.SME.Trade.Common.Resources.BonusUse;
		Name = "BonusUse";
		LayoutGroupExt lgPartCont = AddVerticalLayoutGroup();
		BuyerContainer = new PartnerContractGroupControls(this);
		BuyerContainer.ConstructDlgUI(lgPartCont, storeValue: true, ArmSoft.SME.Common.Resources.Buyer, isFilterBySuppliers: false, isFilterByCustomers: true, isShowContractCurrCode: true, isShowSummaDebt: true, "", ArmSoft.SME.Trade.Common.Resources.AccumulatedBonus, showOnlyBonusContracts: true, showOnlyCurCodeNCC: false, _connectTob2barmsoftDiscountServerforBonusSum);
		((LayoutItemWithRequired)(object)BuyerContainer.PartnerGroup.Parent).Visibility = Visibility.Collapsed;
		_bonusCard = AddStringControl("DiscountCard", ArmSoft.SME.Trade.Common.Resources.DiscountCardCode, 50);
		_bonusCard.IsReadOnly = true;
		if (!string.IsNullOrWhiteSpace(_buyerCode))
		{
			PartnerCacheRow.ContractRow contract = TradeSettings.TradeCacheService.Partner.GetContract(_buyerCode, _contractCode);
			if (contract != null)
			{
				_bonusCard.Text = contract.DiscountCard;
			}
		}
		BuyerContainer.PartnerCode.Code = _buyerCode;
		BuyerContainer.Contract.Code = _contractCode;
		BuyerContainer.PartnerCode.IsReadOnly = true;
		BuyerContainer.Contract.IsRequired = !_editMode;
		BuyerContainer.ContractCodeEditValueChanged += new SafeEventHandler<EventArgs>(BuyerContainer_ContractCodeEditValueChanged).Handler;
		LayoutGroupExt container = AddHorizontalLayoutGroup();
		_usedBonusSumm = AddNumericControl(container, "CreditSum", ArmSoft.SME.Common.Resources.Summa, 16, 2, isRequired: false, allowNegativeNumbers: true);
		if (_editMode)
		{
			_usedBonusSumm.EditValue = _sum;
		}
		else
		{
			_usedBonusSumm.EditValue = Math.Min((BuyerContainer.SummaPartDebt.Value <= 0m) ? 0m : BuyerContainer.SummaPartDebt.Value, _sum);
		}
		_loadSumm = AddButton(container, "Fill", "fill_left_16.svg", "", "", ArmSoft.SME.Common.Resources.ButtonLoad);
		_loadSumm.Click += new SafeEventHandler<RoutedEventArgs>(LoadSumm_Click).Handler;
		if (_editMode)
		{
			_usedBonusSumm.IsReadOnly = true;
			BuyerContainer.PartnerCode.IsReadOnly = true;
			BuyerContainer.Contract.IsReadOnly = true;
			BuyerContainer.ContractCurrCode.IsReadOnly = true;
			_loadSumm.IsEnabled = false;
			Mode = DialogWindowMode.ViewMode;
		}
		SetFocusOnActivation = _usedBonusSumm;
		WindowValidation += new SafeEventHandler<DialogWindowValidationArgs>(BonusUseDialog_WindowValidation).Handler;
	}

	private void BuyerContainer_ContractCodeEditValueChanged(object sender, EventArgs e)
	{
		PartnerCacheRow.ContractRow contract = TradeSettings.TradeCacheService.Partner.GetContract(_buyerCode, BuyerContainer.Contract.Code);
		if (contract != null)
		{
			_bonusCard.Text = contract.DiscountCard;
		}
	}

	private void LoadSumm_Click(object sender, RoutedEventArgs e)
	{
		_usedBonusSumm.EditValue = Math.Min((BuyerContainer.SummaPartDebt.Value <= 0m) ? 0m : BuyerContainer.SummaPartDebt.Value, _sum);
	}

	private void BonusUseDialog_WindowValidation(object sender, DialogWindowValidationArgs e)
	{
		try
		{
			UsedBonusSumm = (decimal)_usedBonusSumm.EditValue;
			if (Math.Abs(BuyerContainer.SummaPartDebt.Value) >= UsedBonusSumm)
			{
				if (_bonusUseMaxPercent < 100m)
				{
					if ((UsedBonusSumm * 100m / _totalSum).RoundAs(2) > _bonusUseMaxPercent)
					{
						throw new ASException(string.Format(ArmSoft.SME.Trade.Common.Resources.UsedMaxBonusValidate, _sum));
					}
					_usedBonusSumm.EditValue = 0m;
				}
				else
				{
					_usedBonusSumm.EditValue = 0m;
				}
				return;
			}
			throw new ASException(ArmSoft.SME.Trade.Common.Resources.UsedBonusValidate);
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
			e.isValid = false;
			e.SetFocusWhenInvalidSave = _usedBonusSumm;
		}
	}
}
