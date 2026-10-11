using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Models.SmartEnums;
using ArmSoft.SME.Common.PersistableObject;
using ArmSoft.SME.Trade.Common.SmartEnums;

namespace ArmSoft.SME.Trade.Common.Documents.Definition;

public static class InvoiceBase
{
	public static GridDefinition Grid;

	static InvoiceBase()
	{
		Grid = new GridDefinition("MTList", () => ArmSoft.SME.Common.Resources.ItemList, isRequired: true);
		AddDefinitionsToMTInvoiceGrid(Grid);
	}

	internal static void AddDefinitionsToMTInvoiceGrid(GridDefinition grid)
	{
		grid.AddFieldDefinition(new NumberFieldDefinition("RowID", () => Resources.RowID, (short)int.MaxValue.ToString().Length, 0, isNonNegative: true, isRequired: false, typeof(int), logChanges: true, null, "RowId"));
		grid.AddFieldDefinition(new SmartEnumFieldDefinition("ItemType", () => ArmSoft.SME.Common.Resources.TypeWord, typeof(GoodsTypesSE), isRequired: true, logChanges: true, ValueMember.Code, null, null, "Type"));
		grid.AddFieldDefinition(new StringFieldDefinition("ItemCode", () => ArmSoft.SME.Common.Resources.Code, 20, isRequired: true, logChanges: true, null, "Code"));
		grid.AddFieldDefinition(new StringFieldDefinition("ItemName", () => ArmSoft.SME.Common.Resources.Name, 200, isRequired: false, logChanges: true, null, "Name"));
		grid.AddFieldDefinition(new StringFieldDefinition("UnitCode", () => ArmSoft.SME.Common.Resources.Unit, 3, isRequired: true, logChanges: true, null, "Unit"));
		grid.AddFieldDefinition(new StringFieldDefinition("UnitBrief", () => ArmSoft.SME.Common.Resources.Brief, 6, isRequired: false, logChanges: true, null, "UnitAbbreviation"));
		grid.AddFieldDefinition(new PercentFieldDefinition("Discount", () => ArmSoft.SME.Common.Resources.DiscountPercent, isRequired: false, isNonNegative: false, logChanges: true, null, "DiscountPercent"));
		grid.AddFieldDefinition(new PriceFieldDefinition("DiscountPrice", () => ArmSoft.SME.Common.Resources.DiscountedPrice, isRequired: false, logChanges: true, null, "DiscountedPrice"));
		grid.AddFieldDefinition(new SumFieldDefinition("Summa", () => ArmSoft.SME.Common.Resources.Summa, isRequired: false, isNonNegative: true, logChanges: true, null, "DiscountedAmount"));
		grid.AddFieldDefinition(new BoolFieldDefinition("VAT", () => ArmSoft.SME.Common.Resources.OptionsVAT, isRequired: true, logChanges: true, null, "VAT"));
	}
}
