using System.Collections.Generic;
using ArmSoft.SME.Core.UI.DataViews;
using ArmSoft.SME.Trade.Client.Data.Directories;
using ArmSoft.SME.Trade.Common;
using ArmSoft.SME.Trade.UI.DropDownDataProvider;

namespace ArmSoft.SME.Trade.UI.DropDown;

public class GiftSetRows : DropDownView<GiftSetRowsDataProvider.DataRow, GiftSetRowsDataProvider.Param>
{
	public List<MTComplectsPartsDataProvider.DataRow> GiftRows { get; set; }

	public override string Caption => Resources.SelectGift;

	public GiftSetRows(List<MTComplectsPartsDataProvider.DataRow> giftRows)
	{
		GiftRows = giftRows;
		DataProvider = new GiftSetRowsDataProvider();
		FillColumnsFromDataProvider();
	}

	protected override void ApplyParametersToDataProvider()
	{
		ParametersProvider.GiftRows = GiftRows;
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
