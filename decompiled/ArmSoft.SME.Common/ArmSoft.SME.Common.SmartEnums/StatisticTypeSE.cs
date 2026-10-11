using Ardalis.SmartEnum;

namespace ArmSoft.SME.Common.SmartEnums;

public class StatisticTypeSE : SmartEnum<StatisticTypeSE, string>
{
	public static readonly StatisticTypeSE DataView = new StatisticTypeSE("DataView", "DataView");

	public static readonly StatisticTypeSE Document = new StatisticTypeSE("Document", "Document");

	public static readonly StatisticTypeSE DPR = new StatisticTypeSE("DPR", "DPR");

	public static readonly StatisticTypeSE Navigator = new StatisticTypeSE("Navigator", "Navigator");

	public static readonly StatisticTypeSE MainRibbon = new StatisticTypeSE("MainRibbon", "MainRibbon");

	public static readonly StatisticTypeSE PORibbon = new StatisticTypeSE("PORibbon", "PORibbon");

	public static readonly StatisticTypeSE ContextFunction = new StatisticTypeSE("ContextFunction", "ContextFunction");

	public static readonly StatisticTypeSE HotKey = new StatisticTypeSE("HotKey", "HotKey");

	public static readonly StatisticTypeSE Dialog = new StatisticTypeSE("Dialog", "Dialog");

	public static readonly StatisticTypeSE Button = new StatisticTypeSE("Button", "Button");

	public static readonly StatisticTypeSE ServiceCaches = new StatisticTypeSE("ServiceCaches", "ServiceCaches");

	public static readonly StatisticTypeSE UICaches = new StatisticTypeSE("UICaches", "UICaches");

	public static readonly StatisticTypeSE Images = new StatisticTypeSE("Images", "Images");

	public StatisticTypeSE(string name, string value)
		: base(name, value)
	{
	}
}
