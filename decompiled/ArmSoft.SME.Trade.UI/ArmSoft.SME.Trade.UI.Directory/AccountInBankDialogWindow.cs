using System;
using System.Windows;
using ArmSoft.SME.Common;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Core.UI.Controls;
using ArmSoft.SME.Core.UI.DataViews;
using ArmSoft.SME.Core.UI.Directories;
using ArmSoft.SME.Core.UI.DropDowns;
using ArmSoft.SME.Trade.Common.Models.Directories;

namespace ArmSoft.SME.Trade.UI.Directory;

[DirectoryWindow(5)]
public class AccountInBankDialogWindow : DirectoryDialogWindow<AccountInBank>
{
	private TextEditExt _settlAccount;

	private TextEditExt _name;

	private DropDownControl _currCode;

	private TextEditExt _nextNum;

	private CheckEditExt _isDefault;

	public AccountInBankDialogWindow(DialogWindowMode mode, Window owner = null)
		: base(mode, owner)
	{
	}

	public AccountInBankDialogWindow(AccountInBank directory, DialogWindowMode mode = DialogWindowMode.NewMode, Window owner = null)
		: base(directory, mode, owner)
	{
	}

	protected override void ConstructUI()
	{
		_code = AddButtonEditControl(Definition.Fields["Code"], 1);
		_code.AllowMagicButton = true;
		_code.MagicToolTip = ArmSoft.SME.Common.Resources.CodeNextNumbers;
		_code.MagicClick += new SafeEventHandler<MagicClickEventArgs>(Code_MagicClick).Handler;
		_settlAccount = AddStringControl(Definition.Fields["SettlementAccount"]);
		_name = AddStringControl(Definition.Fields["Name"]);
		_currCode = AddDropDownControl(Definition.Fields["Curr"], new ArmSoft.SME.Core.UI.DropDowns.Currency(shortMode: true));
		_nextNum = AddStringControl(Definition.Fields["NextPayOrdNumber"]);
		_isDefault = AddBooleanControl(Definition.Fields["IsDefault"]);
		_settlAccount.EditValueChanged += new SafeEventHandler<RoutedEventArgs>(SettlAccount_EditValueChanged).Handler;
		if (Mode == DialogWindowMode.EditMode)
		{
			SetFocusOnActivation = _name;
		}
	}

	protected override void SetControlsReadOnlyInner()
	{
		base.SetControlsReadOnlyInner();
		_code.IsReadOnly = Mode == DialogWindowMode.EditMode;
		_currCode.IsReadOnly = Mode == DialogWindowMode.EditMode && TradeSettings.CreateTradeClient().CommonServices.CheckEntriesExistenceByBank((string)_settlAccount.EditValue);
		_isDefault.IsReadOnly = Mode == DialogWindowMode.EditMode && Convert.ToBoolean(_isDefault.IsChecked);
	}

	private void SettlAccount_EditValueChanged(object sender, RoutedEventArgs e)
	{
		if ((Mode == DialogWindowMode.NewMode || Mode == DialogWindowMode.CreateCopyMode) && !string.IsNullOrWhiteSpace(_settlAccount.Text))
		{
			string bankCaptionBySettlementAccount = Settings.CoreCacheService.Bank.GetBankCaptionBySettlementAccount(_settlAccount.Text, out var bankCode);
			if (!string.IsNullOrWhiteSpace(bankCode))
			{
				_name.EditValue = bankCaptionBySettlementAccount;
				_name.DoValidate();
			}
		}
	}

	protected override void UpdateDataViewPanel(DataViewPanel dvp)
	{
		if (Directory.IsDefault)
		{
			dvp.Refresh();
		}
		else
		{
			base.UpdateDataViewPanel(dvp);
		}
	}
}
