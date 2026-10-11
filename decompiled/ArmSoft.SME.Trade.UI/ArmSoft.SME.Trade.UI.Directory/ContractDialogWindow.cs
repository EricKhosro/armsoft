using System;
using System.Windows;
using System.Windows.Controls;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Extensions;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Core.UI.Controls;
using ArmSoft.SME.Core.UI.DataViews;
using ArmSoft.SME.Core.UI.Directories;
using ArmSoft.SME.Core.UI.DropDowns;
using ArmSoft.SME.Trade.Client.Data.Directories;
using ArmSoft.SME.Trade.Common;
using ArmSoft.SME.Trade.Common.Models.Caches;
using ArmSoft.SME.Trade.Common.Models.Directories;
using ArmSoft.SME.Trade.Common.Parameters;
using ArmSoft.SME.Trade.Common.SmartEnums;
using ArmSoft.SME.Trade.UI.DropDown;
using DevExpress.Xpf.Editors;
using DevExpress.Xpf.LayoutControl;

namespace ArmSoft.SME.Trade.UI.Directory;

[DirectoryWindow(29)]
public class ContractDialogWindow : DirectoryDialogWindow<Contract>
{
	private DropDownControl _partCode;

	private TextEditExt _name;

	private DropDownControl _currCode;

	private DropDownControl _priceType;

	private DateEditControl _contractDate;

	private TextEditExt _description;

	private PopupCalcEditExt _percent;

	private CheckEditExt _controlReceivables;

	private PopupCalcEditExt _receivablesLimit;

	private CheckEditExt _isDefault;

	private TextEditExt _discountCard;

	private CheckEditExt _isBonusCard;

	private PopupCalcEditExt _bonus;

	private DateEditControl _bonusCardValidPeriodStart;

	private DateEditControl _bonusCardValidPeriodEnd;

	private CheckEditExt _bonusCardCanceled;

	public ContractDialogWindow(DialogWindowMode mode, Window owner = null)
		: base(mode, owner)
	{
	}

	public ContractDialogWindow(Contract directory, DialogWindowMode mode = DialogWindowMode.NewMode, Window owner = null)
		: base(directory, mode, owner)
	{
	}

	protected override void ConstructUI()
	{
		if (Settings.ParametersService.UseBonus())
		{
			Settings.ParametersService.ConnectTob2barmsoftDiscountServer();
		}
		else
			_ = 0;
		_partCode = AddDropDownControl(Definition.Fields["PartCode"], new PartnersFilter(), allowAdditionButtons: true);
		_partCode.EditValueChanged += new SafeEventHandler<EditValueChangedEventArgs>(PartCode_EditValueChanged).Handler;
		_code = AddButtonEditControl(Definition.Fields["Code"], 1);
		_code.AllowMagicButton = true;
		_code.MagicToolTip = ArmSoft.SME.Common.Resources.CodeNextNumbers;
		_code.MagicClick += new SafeEventHandler<MagicClickEventArgs>(Code_MagicClick).Handler;
		_name = AddStringControl(Definition.Fields["Name"]);
		_currCode = AddDropDownControl(Definition.Fields["CurrCode"], new ArmSoft.SME.Core.UI.DropDowns.Currency(shortMode: true));
		_currCode.EditValueChanged += new SafeEventHandler<EditValueChangedEventArgs>(CurrCode_EditValueChanged).Handler;
		_priceType = AddDropDownControl(Definition.Fields["PriceType"], new PriceTypes());
		_contractDate = AddDateControl(Definition.Fields["ContractDate"]);
		_description = AddStringControl(Definition.Fields["Description"]);
		_percent = AddNumericControl(Definition.Fields["DiscountPercent"]);
		_controlReceivables = AddBooleanControl(Definition.Fields["ControlReceivables"]);
		_controlReceivables.EditValueChanged += new SafeEventHandler<EditValueChangedEventArgs>(ControlReceivables_EditValueChanged).Handler;
		_receivablesLimit = AddNumericControl(Definition.Fields["ReceivablesLimit"]);
		_isDefault = AddBooleanControl(Definition.Fields["IsDefault"]);
		LayoutGroupExt layoutGroupExt = AddVerticalLayoutGroup(null, LayoutGroupView.GroupBox, ArmSoft.SME.Trade.Common.Resources.CardCapitalLetter);
		layoutGroupExt.HorizontalAlignment = HorizontalAlignment.Stretch;
		_discountCard = AddStringControl(layoutGroupExt, Definition.Fields["DiscountCard"]);
		_isBonusCard = AddBooleanControl(layoutGroupExt, Definition.Fields["IsBonusCard"]);
		_isBonusCard.EditValueChanged += new SafeEventHandler<EditValueChangedEventArgs>(IsBonusCard_EditValueChanged).Handler;
		((Control)(object)_isBonusCard.Parent).Visibility = ((!Settings.ParametersService.UseBonus()) ? Visibility.Collapsed : Visibility.Visible);
		if (Settings.ParametersService.BonusType() == BonusTypeSE.ByPercent.Value)
		{
			_bonus = AddNumericControl(layoutGroupExt, Definition.Fields["Bonus"]);
		}
		else
		{
			_bonus = AddNumericControl(layoutGroupExt, Definition.Fields["Point"]);
		}
		((Control)(object)_bonus.Parent).Visibility = ((!Settings.ParametersService.UseBonus()) ? Visibility.Collapsed : Visibility.Visible);
		LayoutGroupExt container = AddHorizontalLayoutGroup(layoutGroupExt);
		_bonusCardValidPeriodStart = AddDateControl(container, Definition.Fields["BonusCardValidPeriodStart"]);
		_bonusCardValidPeriodEnd = AddDateControl(container, Definition.Fields["BonusCardValidPeriodEnd"]);
		_bonusCardCanceled = AddBooleanControl(layoutGroupExt, Definition.Fields["BonusCardCanceled"]);
		if (Mode == DialogWindowMode.EditMode)
		{
			SetFocusOnActivation = _name;
		}
		base.ConstructUI();
	}

	private void ControlReceivables_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		_receivablesLimit.IsReadOnly = !Convert.ToBoolean(_controlReceivables.IsChecked);
		if (!Convert.ToBoolean(_controlReceivables.IsChecked))
		{
			_receivablesLimit.EditValue = 0m;
		}
	}

	private void PartCode_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		SetNextCode();
	}

	private void IsBonusCard_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		_bonus.IsReadOnly = !Convert.ToBoolean(_isBonusCard.IsChecked);
		if (!Convert.ToBoolean(_isBonusCard.IsChecked))
		{
			_bonus.EditValue = 0m;
		}
	}

	private void CurrCode_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		_priceType.Code = "";
		_priceType.DataView = new PriceTypes(_currCode.Code);
		_priceType.DoValidate();
		if (Settings.ParametersService.UseBonus())
		{
			bool flag = _currCode.Code == "AMD";
			_isBonusCard.IsReadOnly = !flag;
			_bonus.IsReadOnly = !flag || !Convert.ToBoolean(_isBonusCard.IsChecked);
			if (!flag)
			{
				_isBonusCard.IsChecked = false;
				_bonus.EditValue = 0m;
			}
		}
	}

	protected override void SetControlsReadOnlyInner()
	{
		base.SetControlsReadOnlyInner();
		_partCode.IsReadOnly = Mode == DialogWindowMode.EditMode;
		_code.IsReadOnly = Mode == DialogWindowMode.EditMode;
		_receivablesLimit.IsReadOnly = Mode == DialogWindowMode.NewMode || !_controlReceivables.IsChecked.Value;
		_isDefault.IsReadOnly = Mode == DialogWindowMode.EditMode && _isDefault.IsChecked.Value;
		_isBonusCard.IsReadOnly = Mode == DialogWindowMode.NewMode || _isBonusCard.IsReadOnly;
		_bonus.IsReadOnly = Mode == DialogWindowMode.NewMode || !_isBonusCard.IsChecked.Value;
		_bonusCardCanceled.IsReadOnly = Mode == DialogWindowMode.NewMode;
		if (Mode != DialogWindowMode.EditMode)
		{
			return;
		}
		PartnerCacheRow item = TradeSettings.TradeCacheService.Partner.GetItem(_partCode.Code);
		if (item != null)
		{
			_currCode.IsReadOnly = TradeSettings.CreateTradeClient().CommonServices.CheckEntriesExistenceByContract(Convert.ToString(_code.EditValue), item.Id);
			if (TradeSettings.CreateTradeClient().CommonServices.CheckEntriesExistenceByBonusContract(_code.Text, item.Id))
			{
				_isBonusCard.IsReadOnly = true;
			}
		}
	}

	protected override void UpdateDataViewPanel(DataViewPanel dvp)
	{
		if (Directory.IsDefault)
		{
			dvp.Refresh();
			return;
		}
		ContractsDataProvider.DataRow.Key key = new ContractsDataProvider.DataRow.Key
		{
			PartnerId = Directory.PartID,
			Contract = Directory.Code
		};
		dvp.Update(key);
	}

	protected override void SetNextCode()
	{
		PartnerCacheRow item = TradeSettings.TradeCacheService.Partner.GetItem(_partCode.Code);
		if (item != null)
		{
			_code.EditValue = item.NextContractNumber;
		}
		else
		{
			_code.EditValue = StringExtensions.NextDocNum(new string('0', 3), 3);
		}
	}
}
