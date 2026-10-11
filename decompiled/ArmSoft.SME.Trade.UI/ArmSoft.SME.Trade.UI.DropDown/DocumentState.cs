using ArmSoft.SME.Core.UI.DataViews;
using ArmSoft.SME.Trade.Client.Data.DropDowns;

namespace ArmSoft.SME.Trade.UI.DropDown;

public class DocumentState : DropDownView<DocumentStateSEDataProvider.DataRow, DocumentStateSEDataProvider.Param>
{
	public DocumentState()
	{
		DataProvider = new DocumentStateSEDataProvider();
		FillColumnsFromDataProvider();
		Columns.AddSorting("Code");
		Columns["Code"].Visible = false;
	}

	public override string ValueMember(int index)
	{
		return Data[index].ValueUI;
	}

	public override object KeyMember(int index)
	{
		return Data[index].Code;
	}

	public override string CommentMember(int index)
	{
		return Data[index].Caption;
	}

	public override short ValueMemberLength()
	{
		return 2;
	}
}
