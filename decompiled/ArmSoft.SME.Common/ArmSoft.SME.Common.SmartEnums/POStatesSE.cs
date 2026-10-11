using System;

namespace ArmSoft.SME.Common.SmartEnums;

[SmartEnumProviderSupport(1, "b2bExportState")]
public class POStatesSE : ASSmartEnum<POStatesSE, string>
{
	public static readonly POStatesSE ForSend = new POStatesSE("ForSend", "0", () => Resources.ForSend);

	public static readonly POStatesSE SendedByEMailState = new POStatesSE("SendedByEMailState", "1", () => Resources.SendedbyEMail);

	public static readonly POStatesSE SendedByB2BState = new POStatesSE("SendedByB2BState", "2", () => Resources.Sendedbyb2b);

	public static readonly POStatesSE EmptySupplier = new POStatesSE("EmptySupplier", "3", () => Resources.EmptySupplier);

	public static readonly POStatesSE NoLinesInPO = new POStatesSE("NoLinesInPO", "4", () => Resources.NoLinesINPO);

	public const string ForSendConst = "0";

	public const string SendedByEMailStateConst = "1";

	public const string SendedByB2BStateConst = "2";

	public const string EmptySupplierConst = "3";

	public const string NoLinesInPOConst = "4";

	public POStatesSE(string name, string value, Func<string> caption)
		: base(name, value, caption)
	{
	}
}
