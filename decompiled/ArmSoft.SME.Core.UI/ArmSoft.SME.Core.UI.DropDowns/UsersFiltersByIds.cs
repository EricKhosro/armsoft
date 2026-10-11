namespace ArmSoft.SME.Core.UI.DropDowns;

[DropDown(17)]
public class UsersFiltersByIds : UsersFilter
{
	public override string ValueMember(int index)
	{
		return Data[index].UserId.ToString();
	}

	public override short ValueMemberLength()
	{
		return 7;
	}
}
