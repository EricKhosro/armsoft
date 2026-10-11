using ArmSoft.SME.Client.Data;
using ArmSoft.SME.Common.Models;

namespace ArmSoft.SME.Trade.Client.Models;

public class AccountInBanks : ExtendableRow, IRowWithKey, IMatchedUpdateKey
{
	public string Code { get; set; }

	public string Caption { get; set; }

	public string Curr { get; set; }

	public string SettlAccount { get; set; }

	public bool IsDefault { get; set; }

	public string NextNum { get; set; }

	public string BankingModeCode { get; set; }

	public string BankingModeCaption { get; set; }

	public object GetKey()
	{
		return Code;
	}

	public string GetCode()
	{
		return Code;
	}

	public string GetName()
	{
		return Caption;
	}

	public bool Matched(object key)
	{
		return Code == (string)key;
	}
}
