using ArmSoft.SME.Common;
using ArmSoft.SME.Core.UI.DataViews;
using ArmSoft.SME.Core.UI.DropDowns;
using ArmSoft.SME.Trade.UI.DropDownDataProvider;

namespace ArmSoft.SME.Trade.UI.DropDown;

[DropDown(10005)]
public class PriceTypes : DropDownView<PriceTypeDataProvider.DataRow, PriceTypeDataProvider.Param>
{
	private readonly string _currCode;

	public override string Caption => Resources.PriceListTypes;

	public override ReloadMode ReloadMode => ReloadMode.Reloadable;

	public PriceTypes(string currCode = "")
	{
		_currCode = currCode;
		DataProvider = new PriceTypeDataProvider();
		FillColumnsFromDataProvider();
	}

	protected override void ApplyParametersToDataProvider()
	{
		ParametersProvider.CurrCode = _currCode;
	}

	public override short ValueMemberLength()
	{
		return 2;
	}

	public override string ValueMember(int index)
	{
		return Data[index].Code;
	}

	public override string CommentMember(int index)
	{
		return Data[index].Caption;
	}
}
