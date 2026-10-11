using System.Collections.Generic;
using ArmSoft.SME.Client.Data;
using ArmSoft.SME.Core.UI.DataViews;
using ArmSoft.SME.Core.UI.DropDownsData;

namespace ArmSoft.SME.Core.UI.DropDowns.MetaData;

[DropDown(21)]
public class SingleElements : DropDownView<SingleElementsDataProvider.DataRow, ExtendableParameter>
{
	private const short _valueLength = 20;

	public SingleElements(string caption, IEnumerable<string> items)
	{
		DataProvider = new SingleElementsDataProvider(caption, items);
		FillColumnsFromDataProvider();
	}

	public override string ValueMember(int index)
	{
		return Data[index].Name;
	}

	public override short ValueMemberLength()
	{
		return 20;
	}

	public override string CommentMember(int index)
	{
		return Data[index].Name;
	}

	public override object KeyMember(int index)
	{
		return Data[index].Name;
	}
}
