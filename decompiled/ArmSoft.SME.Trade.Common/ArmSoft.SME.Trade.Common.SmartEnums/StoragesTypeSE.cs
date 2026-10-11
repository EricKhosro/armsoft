using System;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.SmartEnums;

namespace ArmSoft.SME.Trade.Common.SmartEnums;

[SmartEnumProviderSupport(1, "TypeWord")]
public class StoragesTypeSE : ASSmartEnum<StoragesTypeSE, string>
{
	public static readonly StoragesTypeSE All = new StoragesTypeSE("All", "0", () => ArmSoft.SME.Common.Resources.All);

	public static readonly StoragesTypeSE WholeSale = new StoragesTypeSE("WholeSale", "1", () => Resources.WholeSaleWord);

	public static readonly StoragesTypeSE Retail = new StoragesTypeSE("Retail", "2", () => Resources.RetailWord);

	public const string AllConst = "0";

	public const string WholeSaleConst = "1";

	public const string RetailConst = "2";

	public StoragesTypeSE(string name, string value, Func<string> caption)
		: base(name, value, caption)
	{
	}
}
