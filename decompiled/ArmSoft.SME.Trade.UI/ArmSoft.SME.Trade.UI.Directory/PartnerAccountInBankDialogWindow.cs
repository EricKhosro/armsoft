using System.Windows;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Core.UI.Controls;
using ArmSoft.SME.Core.UI.DataViews;
using ArmSoft.SME.Core.UI.Directories;
using ArmSoft.SME.Trade.Client.Data.Directories;
using ArmSoft.SME.Trade.Common.Models.Caches;
using ArmSoft.SME.Trade.Common.Models.Directories;
using ArmSoft.SME.Trade.UI.DropDown;
using DevExpress.Xpf.Editors;

namespace ArmSoft.SME.Trade.UI.Directory;

[DirectoryWindow(30)]
public class PartnerAccountInBankDialogWindow : DirectoryDialogWindow<ArmSoft.SME.Trade.Common.Models.Directories.PartnerAccountInBank>
{
	private new DropDownControl _code;

	private TextEditExt _settlementAccount;

	private TextEditExt _settlementAccountCaption;

	private CheckEditExt _isDefault;

	private string _oldAccCode;

	public bool IsFilteredPartner { get; set; }

	public PartnerAccountInBankDialogWindow(DialogWindowMode mode, Window owner = null)
		: base(mode, owner)
	{
	}

	public PartnerAccountInBankDialogWindow(ArmSoft.SME.Trade.Common.Models.Directories.PartnerAccountInBank directory, DialogWindowMode mode = DialogWindowMode.NewMode, Window owner = null)
		: base(directory, mode, owner)
	{
	}

	protected override void ConstructUI()
	{
		_code = AddDropDownControl(Definition.Fields["Code"], new PartnersFilter(), allowAdditionButtons: true);
		_settlementAccount = AddStringControl(Definition.Fields["SettlementAccount"]);
		_settlementAccount.EditValueChanged += new SafeEventHandler<EditValueChangedEventArgs>(SettlementAccount_EditValueChanged).Handler;
		_settlementAccountCaption = AddStringControl(Definition.Fields["SettlementAccountCaption"]);
		_isDefault = AddBooleanControl(Definition.Fields["IsDefault"]);
		base.ConstructUI();
	}

	private void SettlementAccount_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		if (Mode != DialogWindowMode.ViewMode && !string.IsNullOrWhiteSpace(_settlementAccount.Text))
		{
			string bankCaptionBySettlementAccount = Settings.CoreCacheService.Bank.GetBankCaptionBySettlementAccount(_settlementAccount.Text, out var _);
			if (!string.IsNullOrWhiteSpace(bankCaptionBySettlementAccount))
			{
				_settlementAccountCaption.EditValue = bankCaptionBySettlementAccount;
				_settlementAccountCaption.DoValidate();
			}
		}
	}

	protected override void SetControlsReadOnlyInner()
	{
		base.SetControlsReadOnlyInner();
		_code.IsReadOnly = Mode == DialogWindowMode.EditMode || IsFilteredPartner;
		_isDefault.IsReadOnly = Mode == DialogWindowMode.EditMode && _isDefault.IsChecked.Value;
	}

	protected override void UpdateDataViewPanel(DataViewPanel dvp)
	{
		if (Directory.IsDefault || _oldAccCode != Directory.SettlementAccount)
		{
			dvp.Refresh();
			return;
		}
		PartnerCacheRow item = TradeSettings.TradeCacheService.Partner.GetItem(Directory.Code);
		if (item != null)
		{
			PartnerAccountsInBankDataProvider.DataRow.PartnerAccountsInBankRowKey key = new PartnerAccountsInBankDataProvider.DataRow.PartnerAccountsInBankRowKey
			{
				PartID = item.Id,
				SettlAccount = Directory.SettlementAccount
			};
			dvp.Update(key);
		}
	}

	protected override void LoadDataToControls()
	{
		base.LoadDataToControls();
		if (Mode == DialogWindowMode.EditMode || Mode == DialogWindowMode.ViewMode)
		{
			_oldAccCode = Directory.SettlementAccount;
		}
	}

	protected override void FillDocument()
	{
		base.FillDocument();
		Directory.OldSettlementAccount = (string.IsNullOrWhiteSpace(_oldAccCode) ? _settlementAccount.Text : _oldAccCode);
	}
}
