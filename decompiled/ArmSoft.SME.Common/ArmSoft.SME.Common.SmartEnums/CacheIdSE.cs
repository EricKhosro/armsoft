using System;

namespace ArmSoft.SME.Common.SmartEnums;

[SmartEnumPartial(SmartEnumType.Base)]
public class CacheIdSE : ASSmartEnum<CacheIdSE, short>
{
	public static readonly CacheIdSE Currencies = new CacheIdSE("Currencies", 2, () => Resources.Currency);

	public static readonly CacheIdSE Parameters = new CacheIdSE("Parameters", 4, () => Resources.Parameter);

	public static readonly CacheIdSE UserParameters = new CacheIdSE("UserParameters", 5, () => Resources.UserParameter);

	public static readonly CacheIdSE Templates = new CacheIdSE("Templates", 7, () => Resources.PrintTemplate);

	public static readonly CacheIdSE Users = new CacheIdSE("Users", 8, () => Resources.User);

	public static readonly CacheIdSE UserAccess = new CacheIdSE("UserAccess", 9, () => Resources.UserAccess);

	public static readonly CacheIdSE Tree = new CacheIdSE("Tree", 10, () => Resources.TreeDef);

	public static readonly CacheIdSE Banks = new CacheIdSE("Banks", 108, () => Resources.Bank);

	public static readonly CacheIdSE CPAClassifiers = new CacheIdSE("CPAClassifiers", 109, () => Resources.CPAClassification);

	public static readonly CacheIdSE SystemConfig = new CacheIdSE("SystemConfig", 200, () => "SystemConfig");

	public static readonly CacheIdSE StringResources = new CacheIdSE("StringResources", 110, () => "StringResources");

	public static readonly CacheIdSE DocumentExtensions = new CacheIdSE("DocumentExtensions", 111, () => "DocumentExtensions");

	public static readonly CacheIdSE CurrentExchangeRates = new CacheIdSE("CurrentExchangeRates", 112, () => Resources.ExchangeRates);

	public CacheIdSE(string name, short value, Func<string> caption)
		: base(name, value, caption)
	{
	}
}
