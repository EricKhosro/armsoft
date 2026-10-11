using ArmSoft.SME.Common;
using ArmSoft.SME.Common.PersistableObject;
using ArmSoft.SME.Trade.Common.PersistableObject;

namespace ArmSoft.SME.Trade.Common.Documents.Definition;

public static class PartnerBonusGroupCorrectBase
{
	internal static void AddDefToPartBonusGroupCorrectGrid(GridDefinition grid)
	{
		grid.AddFieldDefinition(new PartnerFieldDefinition("PartCode", () => Resources.PartnerCode, isSupplier: false, isCustomer: false, isRequired: true));
		grid.AddFieldDefinition(new StringFieldDefinition("PartName", () => ArmSoft.SME.Common.Resources.Name, 50));
	}
}
