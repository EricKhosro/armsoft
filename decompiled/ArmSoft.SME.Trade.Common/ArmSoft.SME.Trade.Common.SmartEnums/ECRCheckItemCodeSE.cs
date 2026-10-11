using System;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.SmartEnums;

namespace ArmSoft.SME.Trade.Common.SmartEnums;

[SmartEnumProviderSupport(1, "ASECRCheckNamesHeader")]
public class ECRCheckItemCodeSE : ASSmartEnum<ECRCheckItemCodeSE, string>
{
	public static readonly ECRCheckItemCodeSE ItemCode = new ECRCheckItemCodeSE("ItemCode", "1", () => ArmSoft.SME.Common.Resources.Code);

	public static readonly ECRCheckItemCodeSE ItemGroupCode = new ECRCheckItemCodeSE("ItemGroupCode", "2", () => ArmSoft.SME.Common.Resources.GroupsCode);

	public static readonly ECRCheckItemCodeSE ItemExternalCode = new ECRCheckItemCodeSE("ItemExternalCode", "3", () => ArmSoft.SME.Common.Resources.ExternalCode);

	public const string ItemCodeConst = "1";

	public const string ItemGroupCodeConst = "2";

	public const string ItemExternalCodeConst = "3";

	public ECRCheckItemCodeSE(string name, string value, Func<string> caption)
		: base(name, value, caption)
	{
	}
}
