using ArmSoft.SME.Core.UI.DataViews;
using ArmSoft.SME.Trade.Client.Data.Directories;
using ArmSoft.SME.Trade.Common;

namespace ArmSoft.SME.Trade.UI.DropDown;

public class GoodsBySegment : DropDownView<GoodsBySegmentDataProvider.DataRow, GoodsBySegmentDataProvider.Param>
{
	private string segmentCode = string.Empty;

	public override string Caption => Resources.GoodBySegment;

	public override string Name => "GoodBySegment";

	public GoodsBySegment(string segmentCode = "")
	{
		this.segmentCode = segmentCode;
		DataProvider = new GoodsBySegmentDataProvider(TradeSettings.CreateTradeClient());
	}

	protected override void ApplyParametersToDataProvider()
	{
		ParametersProvider.SegmentCode = segmentCode;
		base.ApplyParametersToDataProvider();
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

	public override string KeyFieldName()
	{
		return "MTCode";
	}
}
