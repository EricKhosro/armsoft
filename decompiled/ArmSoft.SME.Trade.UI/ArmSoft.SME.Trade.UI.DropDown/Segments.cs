using ArmSoft.SME.Client.Data;
using ArmSoft.SME.Core.UI.DataViews;
using ArmSoft.SME.Trade.Common;
using ArmSoft.SME.Trade.UI.DropDownDataProvider;

namespace ArmSoft.SME.Trade.UI.DropDown;

public class Segments : DropDownView<CodeCaptionPairsDataProvider.DataRow, CodeCaptionPairsDataProvider.Param>
{
	public override string Caption => Resources.Segments;

	public override ReloadMode ReloadMode => ReloadMode.Reloadable;

	public Segments()
	{
		DataProvider = new SegmentsDataProvider();
		FillColumnsFromDataProvider();
		Columns.AddSorting("Code");
	}

	public override short ValueMemberLength()
	{
		return 5;
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
