using ArmSoft.SME.Core.UI.DataViews;
using ArmSoft.SME.Core.UI.DropDownsData;

namespace ArmSoft.SME.Core.UI.DropDowns;

public class DropBoxFiles : DropDownView<DropBoxFilesDataProvider.DataRow, DropBoxFilesDataProvider.Param>
{
	public DropBoxFiles(string token)
	{
		DataProvider = new DropBoxFilesDataProvider(token);
		FillColumnsFromDataProvider();
		Columns.AddSorting("ModifiedDate", descending: true);
	}

	public override string ValueMember(int index)
	{
		return Data[index].Name;
	}

	public override string CommentMember(int index)
	{
		return Data[index].Name;
	}
}
