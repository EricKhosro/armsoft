using ArmSoft.SME.Core.UI.DataViews;
using ArmSoft.SME.Trade.UI.DropDownDataProvider;

namespace ArmSoft.SME.Trade.UI.DropDown;

public class OfflineGoodsFilter : DropDownView<OfflineGoodsFilterDataProvider.DataRow, OfflineGoodsFilterDataProvider.Param>
{
	public string PriceType { get; set; }

	public string LikeName { get; set; }

	public OfflineGoodsFilter(string priceType, string likeName = "")
	{
		PriceType = priceType;
		LikeName = likeName;
		DataProvider = new OfflineGoodsFilterDataProvider();
		FillColumnsFromDataProvider();
	}

	protected override void ApplyParametersToDataProvider()
	{
		ParametersProvider.PriceType = PriceType;
		ParametersProvider.LikeName = LikeName;
	}

	public override short ValueMemberLength()
	{
		return 20;
	}

	public override string ValueMember(int index)
	{
		return Data[index].MTCode;
	}

	public override string CommentMember(int index)
	{
		return Data[index].Caption;
	}
}
