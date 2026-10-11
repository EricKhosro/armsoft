using System.Collections.Generic;
using ArmSoft.SME.Client.Data;
using ArmSoft.SME.Core.UI.DataViews;
using ArmSoft.SME.Core.UI.DropDownsData;

namespace ArmSoft.SME.Core.UI.DropDowns;

public class Columns : DropDownView<CodeCaptionPairsDataProvider.DataRow, CodeCaptionPairsDataProvider.Param>
{
	public Columns(Dictionary<string, string> columns)
	{
		DataProvider = new ColumnsDataProvider(columns);
		FillColumnsFromDataProvider();
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
		return 6;
	}
}
