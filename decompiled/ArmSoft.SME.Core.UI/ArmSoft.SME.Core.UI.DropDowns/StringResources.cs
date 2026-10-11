using System.Windows;
using ArmSoft.SME.Client.Data;
using ArmSoft.SME.Client.Data.Reports;
using ArmSoft.SME.Common;
using ArmSoft.SME.Core.UI.DataViews;
using ArmSoft.SME.Core.UI.DropDownsData;
using ArmSoft.SME.Core.UI.MetaData;

namespace ArmSoft.SME.Core.UI.DropDowns;

[DropDown(15)]
public class StringResources : DropDownView<UserResourcesDataProvider.DataRow, ExtendableParameter>
{
	public override string HelpId => string.Empty;

	public override ReloadMode ReloadMode => ReloadMode.Reloadable;

	public override bool AddNewSupported => Settings.LoginService.IsAdmin;

	public override bool EditSupported => Settings.LoginService.IsAdmin;

	public override bool EditEnabled => Settings.LoginService.IsAdmin;

	public override bool AddNewEnabled => Settings.LoginService.IsAdmin;

	public StringResources()
	{
		DataProvider = new StringResourcesDataProvider();
		Caption = Resources.ResourcesCaption;
		FillColumnsFromDataProvider();
	}

	public override string ValueMember(int index)
	{
		return Data[index].Name;
	}

	public override string CommentMember(int index)
	{
		return Data[index].Caption;
	}

	public override short ValueMemberLength()
	{
		return 50;
	}

	public override bool AddNew(ref string value, ref string comment, Window owner, double top, double left)
	{
		return AddOrEdit(ref value, ref comment, owner, top, left, DialogWindowMode.NewModeWithNext);
	}

	public override bool Edit(ref string value, ref string comment, Window owner, double top, double left, object selectedRow)
	{
		return AddOrEdit(ref value, ref comment, owner, top, left, DialogWindowMode.EditMode);
	}

	private bool AddOrEdit(ref string value, ref string comment, Window owner, double top, double left, DialogWindowMode mode)
	{
		UserStringResourceWindow userStringResourceWindow = new UserStringResourceWindow(mode, (mode == DialogWindowMode.EditMode) ? value : "")
		{
			Owner = owner,
			Top = top,
			Left = left
		};
		if (userStringResourceWindow.Invoke())
		{
			DataLoadedAtLeastOnce = false;
			value = userStringResourceWindow.NameEdit.Text;
			comment = userStringResourceWindow.CaptionEdit.Text;
			return true;
		}
		return false;
	}
}
