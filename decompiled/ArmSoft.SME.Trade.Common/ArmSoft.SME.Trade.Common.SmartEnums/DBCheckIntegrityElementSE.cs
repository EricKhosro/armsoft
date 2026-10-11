using System;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.SmartEnums;

namespace ArmSoft.SME.Trade.Common.SmartEnums;

[SmartEnumPartial(SmartEnumType.Special)]
[SmartEnumPartial(SmartEnumType.Base)]
public class DBCheckIntegrityElementSE : ASSmartEnum<DBCheckIntegrityElementSE, short>
{
	public static readonly DBCheckIntegrityElementSE PartnersCashesAccounts = new DBCheckIntegrityElementSE("PartnersCashesAccounts", 1, () => Resources.PartnersCashesAccounts);

	public static readonly DBCheckIntegrityElementSE GoodsServices = new DBCheckIntegrityElementSE("GoodsServices", 2, () => Resources.MaterialsAndServices);

	public static readonly DBCheckIntegrityElementSE Sales = new DBCheckIntegrityElementSE("Sales", 3, () => ArmSoft.SME.Common.Resources.Sales);

	public static readonly DBCheckIntegrityElementSE DBCC = new DBCheckIntegrityElementSE("DBCC", 0, () => ArmSoft.SME.Common.Resources.CheckPhysicalIntegrity);

	protected DBCheckIntegrityElementSE(string name, short value, Func<string> caption)
		: base(name, value, caption)
	{
	}
}
