using System.Text.Json;
using ArmSoft.SME.Common.Models.Caches;

namespace ArmSoft.SME.Trade.Common.Models.Caches;

public class AccountInBankCacheRow : CacheRowBase<string>
{
	public string Code { get; set; }

	public string Caption { get; set; }

	public string Curr { get; set; }

	public string SettlAccount { get; set; }

	public bool IsDefault { get; set; }

	public string NextNum { get; set; }

	public string BankingModeCode { get; set; }

	public string BankingModeCaption { get; set; }

	public override string KeyJSON => JsonSerializer.Serialize(Code);

	public AccountInBankCacheRow(string code, string caption, string curr, string settlAccount, bool isDefault, string nextNum, string bankingModeCode, string bankingModeCaption, byte[] ts)
	{
		Code = code;
		Caption = caption;
		Curr = curr;
		SettlAccount = settlAccount;
		IsDefault = isDefault;
		NextNum = nextNum;
		BankingModeCode = bankingModeCode;
		BankingModeCaption = bankingModeCaption;
		TS = ts;
	}

	public AccountInBankCacheRow()
	{
	}
}
