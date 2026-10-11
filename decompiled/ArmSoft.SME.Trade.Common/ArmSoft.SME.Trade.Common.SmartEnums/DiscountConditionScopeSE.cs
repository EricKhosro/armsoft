using System;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.SmartEnums;

namespace ArmSoft.SME.Trade.Common.SmartEnums;

public class DiscountConditionScopeSE : ASSmartEnum<DiscountConditionScopeSE, string>
{
	public static readonly DiscountConditionScopeSE Document = new DiscountConditionScopeSE("Document", "1", () => ArmSoft.SME.Common.Resources.Document);

	public static readonly DiscountConditionScopeSE Row = new DiscountConditionScopeSE("Document", "2", () => ArmSoft.SME.Common.Resources.Row);

	public const string DocumentConst = "1";

	public const string RowConst = "2";

	public DiscountConditionScopeSE(string name, string value, Func<string> caption)
		: base(name, value, caption)
	{
	}
}
