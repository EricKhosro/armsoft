using System;

namespace ArmSoft.SME.Common.SmartEnums;

[SmartEnumPartial(SmartEnumType.Base)]
public class DBCheckIntegrityElementSE : ASSmartEnum<DBCheckIntegrityElementSE, short>
{
	public static readonly DBCheckIntegrityElementSE DBCC = new DBCheckIntegrityElementSE("DBCC", 0, () => Resources.CheckPhysicalIntegrity);

	protected DBCheckIntegrityElementSE(string name, short value, Func<string> caption)
		: base(name, value, caption)
	{
	}
}
