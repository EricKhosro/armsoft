using System.Windows;
using ArmSoft.SME.Client.Data;
using ArmSoft.SME.Core.UI.DataViews;
using ArmSoft.SME.Core.UI.DataViews.TreeNodes;

namespace ArmSoft.SME.Core.UI.DropDowns;

[DropDown(3)]
public class TreesList : DropDownView<TreesListDataProvider.DataRow, TreesListDataProvider.Param>
{
	public override string HelpId => "";

	public override ReloadMode ReloadMode => ReloadMode.Reloadable;

	public bool? ParamIsSystem { get; set; }

	public TreesList()
	{
		DataProvider = new TreesListDataProvider(Settings.CreateApiClient());
		FillColumnsFromDataProvider();
	}

	protected override void ApplyParametersToDataProvider()
	{
		ParametersProvider.IsSystem = ParamIsSystem;
	}

	public override short ValueMemberLength()
	{
		return 8;
	}

	public override string ValueMember(int index)
	{
		return Data[index].Code;
	}

	public override string CommentMember(int index)
	{
		return Data[index].Caption;
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
		TreeAddEditDialog treeAddEditDialog = new TreeAddEditDialog(mode, (mode == DialogWindowMode.EditMode) ? value : "")
		{
			Owner = owner,
			Top = top,
			Left = left
		};
		if (treeAddEditDialog.ShowDialog().Value)
		{
			DataLoadedAtLeastOnce = false;
			value = treeAddEditDialog.TreeId;
			comment = treeAddEditDialog.TreeCaption;
			return true;
		}
		return false;
	}
}
