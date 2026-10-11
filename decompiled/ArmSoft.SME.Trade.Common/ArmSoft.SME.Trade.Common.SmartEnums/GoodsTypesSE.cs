using System;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.SmartEnums;

namespace ArmSoft.SME.Trade.Common.SmartEnums;

[SmartEnumProviderSupport(1, "TypeWord")]
public class GoodsTypesSE : ASSmartEnum<GoodsTypesSE, string>
{
	public static readonly GoodsTypesSE Good = new GoodsTypesSE("Good", ((short)1).ToString(), () => Resources.MaterialValue);

	public static readonly GoodsTypesSE Service = new GoodsTypesSE("Service", ((short)2).ToString(), () => ArmSoft.SME.Common.Resources.Service);

	public GoodsTypesSE(string name, string value, Func<string> caption)
		: base(name, value, caption)
	{
	}
}
