using ArmSoft.SME.Client.Data;
using ArmSoft.SME.Common;
using ArmSoft.SME.Core.UI.DataViews;
using ArmSoft.SME.Trade.UI.DropDownDataProvider;

namespace ArmSoft.SME.Trade.UI.DropDown;

public class Producer : DropDownView<CodeCaptionPairsDataProvider.DataRow, CodeCaptionPairsDataProvider.Param>
{
	public override string Caption => Resources.Producer;

	public override ReloadMode ReloadMode => ReloadMode.Reloadable;

	public Producer()
	{
		DataProvider = new ProducerDataProvider();
		FillColumnsFromDataProvider();
		Columns.AddSorting("Code");
	}

	public override short ValueMemberLength()
	{
		return 4;
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
