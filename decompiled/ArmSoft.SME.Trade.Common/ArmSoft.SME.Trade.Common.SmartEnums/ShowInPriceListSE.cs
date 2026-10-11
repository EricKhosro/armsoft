using System;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.SmartEnums;

namespace ArmSoft.SME.Trade.Common.SmartEnums;

[SmartEnumProviderSupport(1, "ShowAll")]
public class ShowInPriceListSE : ASSmartEnum<ShowInPriceListSE, string>
{
	public static readonly ShowInPriceListSE IncludedInPriceList = new ShowInPriceListSE("IncludedInPriceList", ((short)1).ToString(), () => Resources.ShowOnlyInPrice);

	public static readonly ShowInPriceListSE NotIncludedInPriceList = new ShowInPriceListSE("NotIncludedInPriceList", ((short)2).ToString(), () => Resources.ShowOnlyNotInPrice);

	public static readonly ShowInPriceListSE All = new ShowInPriceListSE("All", ((short)3).ToString(), () => ArmSoft.SME.Common.Resources.All);

	public ShowInPriceListSE(string name, string value, Func<string> caption)
		: base(name, value, caption)
	{
	}
}
