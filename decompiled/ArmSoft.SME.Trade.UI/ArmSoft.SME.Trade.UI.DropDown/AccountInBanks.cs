using ArmSoft.SME.Core.UI.DataViews;
using ArmSoft.SME.Core.UI.DropDowns;
using ArmSoft.SME.Trade.UI.DropDownDataProvider;

namespace ArmSoft.SME.Trade.UI.DropDown;

[DropDown(1)]
public class AccountInBanks : DropDownView<AccountInBankDataProvider.DataRow, AccountInBankDataProvider.Param>
{
	public override ReloadMode ReloadMode => ReloadMode.Reloadable;

	public bool IsKeySettlAccount { get; set; }

	public string CurrCode { get; set; } = string.Empty;

	public string BankingMode { get; set; } = string.Empty;

	public AccountInBanks()
	{
		IsKeySettlAccount = false;
		BankingMode = "";
		DataProvider = new AccountInBankDataProvider(IsKeySettlAccount);
		FillColumnsFromDataProvider();
		Columns.AddSorting("IsDefault", descending: true);
		Columns.AddSorting("Code");
	}

	public AccountInBanks(string bankigMode = "")
		: this(isKeySettlAccount: true, bankigMode)
	{
	}

	public AccountInBanks(bool isKeySettlAccount, string bankigMode = "")
	{
		IsKeySettlAccount = isKeySettlAccount;
		BankingMode = bankigMode;
		DataProvider = new AccountInBankDataProvider(isKeySettlAccount);
		FillColumnsFromDataProvider();
		if (IsKeySettlAccount)
		{
			Columns.Move("Code", "SettlAccount", after: true);
		}
		Columns.AddSorting("IsDefault", descending: true);
		Columns.AddSorting(IsKeySettlAccount ? "SettlAccount" : "Code");
	}

	protected override void ApplyParametersToDataProvider()
	{
		ParametersProvider.CurrCode = CurrCode;
		ParametersProvider.BankingMode = BankingMode;
	}

	public override short ValueMemberLength()
	{
		if (!IsKeySettlAccount)
		{
			return 8;
		}
		return 22;
	}

	public override string ValueMember(int index)
	{
		if (!IsKeySettlAccount)
		{
			return Data[index].Code;
		}
		return Data[index].SettlAccount;
	}

	public override string CommentMember(int index)
	{
		if (!IsKeySettlAccount)
		{
			return Data[index].SettlAccount;
		}
		return Data[index].Caption;
	}
}
