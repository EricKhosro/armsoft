using System;

namespace ArmSoft.SME.Common.SmartEnums;

[SmartEnumProviderSupport(2, "ArmSoft.SME.Common.Resources.ExchangeTypes")]
[SmartEnumPartial(SmartEnumType.Base)]
public class ExchangeTypeSE : ASSmartEnum<ExchangeTypeSE, string>
{
	public static readonly ExchangeTypeSE Bank = new ExchangeTypeSE("Bank", "Bank", () => Resources.Bank);

	public static readonly ExchangeTypeSE Currency = new ExchangeTypeSE("Currency", "Currency", () => Resources.Currency);

	public static readonly ExchangeTypeSE RatesOfExchange = new ExchangeTypeSE("RatesOfExchange", "RatesOfExchange", () => Resources.ExchangeRate);

	public static readonly ExchangeTypeSE CPAClassifier = new ExchangeTypeSE("CPAClassifier", "CPAClassifier", () => Resources.CPAClassification);

	public static readonly ExchangeTypeSE TreeNode = new ExchangeTypeSE("TreeNode", "TreeNode", () => Resources.TreeDef);

	protected ExchangeTypeSE(string name, string value, Func<string> caption)
		: base(name, value, caption)
	{
	}
}
