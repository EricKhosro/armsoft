using System;

namespace ArmSoft.SME.Common.SmartEnums;

public class DebitCreditSE : ASSmartEnum<DebitCreditSE, string>
{
	public static readonly DebitCreditSE Debit = new DebitCreditSE("Debit", "D", () => "Debit");

	public static readonly DebitCreditSE Credit = new DebitCreditSE("Credit", "C", () => "Credit");

	protected DebitCreditSE(string name, string value, Func<string> caption)
		: base(name, value, caption)
	{
	}
}
