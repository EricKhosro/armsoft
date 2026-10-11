using ArmSoft.SME.Core.UI.DataViews;
using ArmSoft.SME.Core.UI.DropDowns;
using ArmSoft.SME.Trade.UI.DropDownDataProvider;

namespace ArmSoft.SME.Trade.UI.DropDown;

[DropDown(2)]
public class PartnerAccountInBank : DropDownView<PartnerAccountsInBankDataProvider.DataRow, PartnerAccountsInBankDataProvider.Param>
{
	public override string HelpId => "Purchases_Lists_Partnerssublists_PartnerSettlementAccounts";

	public override ReloadMode ReloadMode => ReloadMode.Reloadable;

	public string PartnerCode { get; set; }

	private bool ShowPartColumn => PartnerCode == "";

	public PartnerAccountInBank(string partnerCode = "")
	{
		PartnerCode = partnerCode;
		DataProvider = new PartnerAccountsInBankDataProvider();
		FillColumnsFromDataProvider();
		if (!ShowPartColumn)
		{
			Columns["PartCode"].Visible = false;
			Columns["PartCaption"].Visible = false;
		}
		Columns.AddSorting("IsDefault", descending: true);
		Columns.AddSorting("SettlAccount");
	}

	protected override void ApplyParametersToDataProvider()
	{
		ParametersProvider.PartnerCode = PartnerCode;
	}

	public override short ValueMemberLength()
	{
		return 50;
	}

	public override string ValueMember(int index)
	{
		return Data[index].SettlAccount;
	}

	public override string CommentMember(int index)
	{
		return Data[index].Caption;
	}
}
