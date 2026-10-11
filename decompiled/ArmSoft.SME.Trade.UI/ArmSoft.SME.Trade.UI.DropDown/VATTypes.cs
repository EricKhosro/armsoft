using ArmSoft.SME.Common;
using ArmSoft.SME.Core.UI.DataViews;
using ArmSoft.SME.Trade.UI.DropDownDataProvider;

namespace ArmSoft.SME.Trade.UI.DropDown;

public class VATTypes : DropDownView<VATTypesDataProvider.DataRow, VATTypesDataProvider.Param>
{
	public bool IncludeVATRetail { get; set; }

	public bool OnlyVATRetailTypes { get; set; }

	public bool VATRetailCaption { get; set; }

	public override string Caption => Resources.VATTypes;

	public override string Name => "VatTypes";

	public VATTypes(bool includeVATRetail = false, bool onlyVATRetailTypes = false, bool vatRetailCaption = false)
	{
		DataProvider = new VATTypesDataProvider();
		FillColumnsFromDataProvider();
		Columns.AddSorting("Code");
		IncludeVATRetail = includeVATRetail;
		OnlyVATRetailTypes = onlyVATRetailTypes;
		VATRetailCaption = vatRetailCaption;
	}

	public override string ValueMember(int index)
	{
		return Data[index].Code;
	}

	public override string CommentMember(int index)
	{
		return Data[index].Caption;
	}

	public override short ValueMemberLength()
	{
		return 1;
	}

	protected override void ApplyParametersToDataProvider()
	{
		ParametersProvider.IncludeVATRetail = IncludeVATRetail;
		ParametersProvider.OnlyVATRetailTypes = OnlyVATRetailTypes;
		ParametersProvider.VATRetailCaption = VATRetailCaption;
	}
}
