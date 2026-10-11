using System.Windows;
using ArmSoft.SME.Client.Data;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Core.UI.DataViews;
using ArmSoft.SME.Trade.Common;
using ArmSoft.SME.Trade.Common.SmartEnums;
using ArmSoft.SME.Trade.UI.Directories;
using ArmSoft.SME.Trade.UI.DropDownDataProvider;

namespace ArmSoft.SME.Trade.UI.DropDown;

public class SubstituteItemsGroups : DropDownView<CodeCaptionPairsDataProvider.DataRow, CodeCaptionPairsDataProvider.Param>
{
	public override string Caption => Resources.SubstituteItemsGroups;

	public override ReloadMode ReloadMode => ReloadMode.Reloadable;

	public override bool AddNewSupported => Settings.UserAccessService.GetDirectoryPermission(DirectoryTypeSE.SubstituteItemsGroups).AllowAdd;

	public override bool EditSupported => Settings.UserAccessService.GetDirectoryPermission(DirectoryTypeSE.SubstituteItemsGroups).AllowEdit;

	public SubstituteItemsGroups()
	{
		DataProvider = new SubstituteItemsGroupDataProvider();
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

	public override bool AddNew(ref string value, ref string comment, Window owner, double top, double left)
	{
		return AddOrEdit(ref value, ref comment, isNew: true, owner, top, left);
	}

	public override bool Edit(ref string value, ref string comment, Window owner, double top, double left, object selectedRow)
	{
		return AddOrEdit(ref value, ref comment, isNew: false, owner, top, left);
	}

	private bool AddOrEdit(ref string value, ref string comment, bool isNew, Window owner, double top, double left)
	{
		bool result = false;
		SubstituteItemsGroupsDialogWindow substituteItemsGroupsDialogWindow = (isNew ? ((SubstituteItemsGroupsDialogWindow)Settings.DirectoryService.CreateDialogWindow(DirectoryTypeSE.SubstituteItemsGroups, DialogWindowMode.NewMode)) : ((SubstituteItemsGroupsDialogWindow)Settings.DirectoryService.CreateDialogWindow(Settings.DirectoryService.Load(DirectoryTypeSE.SubstituteItemsGroups, value), DialogWindowMode.EditMode)));
		substituteItemsGroupsDialogWindow.Owner = owner;
		substituteItemsGroupsDialogWindow.Top = top;
		substituteItemsGroupsDialogWindow.Left = left;
		if (substituteItemsGroupsDialogWindow.ShowDialog().Value)
		{
			DataLoadedAtLeastOnce = false;
			result = true;
			value = substituteItemsGroupsDialogWindow.Directory.Code;
			comment = substituteItemsGroupsDialogWindow.Directory.Name;
			LoadData();
		}
		return result;
	}
}
