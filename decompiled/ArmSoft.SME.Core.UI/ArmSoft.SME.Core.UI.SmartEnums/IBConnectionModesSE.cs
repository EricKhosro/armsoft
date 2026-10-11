using System;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.SmartEnums;

namespace ArmSoft.SME.Core.UI.SmartEnums;

[SmartEnumProviderSupport(3, "ConnectionMode")]
public class IBConnectionModesSE : ASSmartEnum<IBConnectionModesSE, string>
{
	public static readonly IBConnectionModesSE NotConnected = new IBConnectionModesSE("NotConnected", "0", () => Resources.IsNotConnected, 0);

	public static readonly IBConnectionModesSE InternetBank = new IBConnectionModesSE("InternetBank", "1", () => Resources.InternetBankWithoutAS, 1);

	public static readonly IBConnectionModesSE OpenBank = new IBConnectionModesSE("OpenBank", "2", () => Resources.InternetBankNewProtocol, 2);

	public static readonly IBConnectionModesSE BankDefault = new IBConnectionModesSE("BankDefault", "3", () => Resources.DefaultValue, 3);

	public short OrderID { get; }

	private IBConnectionModesSE(string name, string value, Func<string> caption, short orderId)
		: base(name, value, caption)
	{
		OrderID = orderId;
	}
}
