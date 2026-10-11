using System;
using ArmSoft.SME.Common.SmartEnums;

namespace ArmSoft.SME.Trade.Common.SmartEnums;

[SmartEnumProviderSupport(1, "MTExpMethods")]
public class MTExpenseMethodsSE : ASSmartEnum<MTExpenseMethodsSE, string>
{
	public static readonly MTExpenseMethodsSE FIFO = new MTExpenseMethodsSE("FIFO", "1", () => Resources.MTExpenseMethodFIFOShort, () => Resources.MTExpenseMethodFIFO);

	public static readonly MTExpenseMethodsSE LIFO = new MTExpenseMethodsSE("LIFO", "2", () => Resources.MTExpenseMethodLIFOShort, () => Resources.MTExpenseMethodLIFO);

	public static readonly MTExpenseMethodsSE Average = new MTExpenseMethodsSE("Average", "3", () => Resources.MTExpenseAverage, () => Resources.MTExpenseAverage);

	private readonly Func<string> fullcaptionDelegate;

	public string FullCaption => fullcaptionDelegate();

	public MTExpenseMethodsSE(string name, string value, Func<string> caption, Func<string> fullCaption)
		: base(name, value, caption)
	{
		fullcaptionDelegate = fullCaption;
	}
}
