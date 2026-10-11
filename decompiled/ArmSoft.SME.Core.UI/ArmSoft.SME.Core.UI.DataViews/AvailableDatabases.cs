using System.Collections.Generic;
using ArmSoft.SME.Client.Data;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Models.Cloud;

namespace ArmSoft.SME.Core.UI.DataViews;

public class AvailableDatabases : DropDownView<AvailableDataBasesDataProvider.DataRow, AvailableDataBasesDataProvider.Param>
{
	public override string Caption => Resources.Databases;

	public override string Name => "Databases";

	public AvailableDatabases(List<Database> source)
	{
		DataProvider = new AvailableDataBasesDataProvider(source);
		FillColumnsFromDataProvider();
		Columns.AddSorting("Name");
	}

	public override short ValueMemberLength()
	{
		return 20;
	}

	public override string ValueMember(int index)
	{
		return Data[index].Name;
	}

	public override string CommentMember(int index)
	{
		return Data[index].Caption;
	}
}
