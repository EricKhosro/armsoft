namespace ArmSoft.SME.Common.SmartEnums;

public class PartnerStateInB2BSE : ASSmartEnum<PartnerStateInB2BSE, byte>
{
	public static readonly PartnerStateInB2BSE NotExist = new PartnerStateInB2BSE("NotExist", 0);

	public static readonly PartnerStateInB2BSE ExistAndIsNotActor = new PartnerStateInB2BSE("ExistAndIsNotActor", 1);

	public static readonly PartnerStateInB2BSE IsActor = new PartnerStateInB2BSE("IsActor", 2);

	private PartnerStateInB2BSE(string name, byte value)
		: base(name, value)
	{
	}
}
