using System;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.SmartEnums;

namespace ArmSoft.SME.Trade.Common.SmartEnums;

[SmartEnumProviderSupport(1, "ASECRCheckNamesHeader")]
public class ECRCheckItemNameSE : ASSmartEnum<ECRCheckItemNameSE, string>
{
	public static readonly ECRCheckItemNameSE ItemName = new ECRCheckItemNameSE("ItemName", "1", () => ArmSoft.SME.Common.Resources.Name);

	public static readonly ECRCheckItemNameSE ItemFullName = new ECRCheckItemNameSE("ItemFullName", "2", () => ArmSoft.SME.Common.Resources.FullCaption);

	public static readonly ECRCheckItemNameSE ItemDescr = new ECRCheckItemNameSE("ItemDescr", "3", () => ArmSoft.SME.Common.Resources.Specification);

	public static readonly ECRCheckItemNameSE GroupName = new ECRCheckItemNameSE("GroupName", "4", () => Resources.GroupsName);

	public static readonly ECRCheckItemNameSE CPACaption = new ECRCheckItemNameSE("CPACaption", "5", () => ArmSoft.SME.Common.Resources.CPACaption);

	public const string ItemNameConst = "1";

	public const string ItemFullNameConst = "2";

	public const string ItemDescrConst = "3";

	public const string GroupNameConst = "4";

	public const string CPACaptionConst = "5";

	public ECRCheckItemNameSE(string name, string value, Func<string> caption)
		: base(name, value, caption)
	{
	}
}
