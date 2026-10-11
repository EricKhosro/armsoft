using ArmSoft.SME.Core.UI.DataViews;
using ArmSoft.SME.Trade.UI.DropDownDataProvider;

namespace ArmSoft.SME.Trade.UI.DropDown;

public class Cashes : DropDownView<CashDataProvider.DataRow, CashDataProvider.Param>
{
	public bool WithoutCheckAccess { get; set; }

	public bool ShowOnlyECRCashes { get; set; }

	public override ReloadMode ReloadMode => ReloadMode.Reloadable;

	public Cashes(bool withoutCheckAccess = false, bool showOnlyECRCashes = false)
	{
		DataProvider = new CashDataProvider();
		FillColumnsFromDataProvider();
		Columns.AddSorting("IsDefault", descending: true);
		Columns.AddSorting("Code");
		WithoutCheckAccess = withoutCheckAccess;
		ShowOnlyECRCashes = showOnlyECRCashes;
	}

	public override short ValueMemberLength()
	{
		return 8;
	}

	public override string ValueMember(int index)
	{
		return Data[index].Code;
	}

	public override string CommentMember(int index)
	{
		return Data[index].Caption;
	}

	protected override void ApplyParametersToDataProvider()
	{
		ParametersProvider.WithoutCheckAccess = WithoutCheckAccess;
		ParametersProvider.ShowOnlyECRCashes = ShowOnlyECRCashes;
	}
}
