using System.Windows;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Core.UI.DataViews;
using ArmSoft.SME.Trade.Common;
using ArmSoft.SME.Trade.Common.SmartEnums;
using ArmSoft.SME.Trade.UI.Directory;
using ArmSoft.SME.Trade.UI.DropDownDataProvider;

namespace ArmSoft.SME.Trade.UI.DropDown;

public class GoodsGroups : DropDownView<GoodGroupDataProvider.DataRow, GoodGroupDataProvider.Param>
{
	public override string Caption => Resources.MaterGrps;

	public override string Name => "MTGroup";

	public override ReloadMode ReloadMode => ReloadMode.Reloadable;

	public override bool AddNewSupported => Settings.UserAccessService.GetDirectoryPermission(DirectoryTypeSE.MTGroup).AllowAdd;

	public override bool EditSupported => Settings.UserAccessService.GetDirectoryPermission(DirectoryTypeSE.MTGroup).AllowEdit;

	public GoodsGroups(bool showAsTree = true, bool filteredSearch = false)
	{
		DataProvider = new GoodGroupDataProvider();
		FillColumnsFromDataProvider();
		Columns["Path"].Visible = false;
		Columns["Parent"].Visible = !filteredSearch;
		Columns["CodeForTree"].Visible = showAsTree && !filteredSearch;
		Columns["Code"].Visible = !showAsTree | filteredSearch;
		Columns["CaptionEn"].Visible = false;
		Columns["CaptionRu"].Visible = false;
		Columns.AddSorting("Path");
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

	public override string ParentFieldName()
	{
		return "Parent";
	}

	public override string KeyFieldName()
	{
		return "Code";
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
		MTGroupDialogWindow mTGroupDialogWindow = (isNew ? ((MTGroupDialogWindow)Settings.DirectoryService.CreateDialogWindow(DirectoryTypeSE.MTGroup, DialogWindowMode.NewMode)) : ((MTGroupDialogWindow)Settings.DirectoryService.CreateDialogWindow(Settings.DirectoryService.Load(DirectoryTypeSE.MTGroup, value), DialogWindowMode.EditMode)));
		mTGroupDialogWindow.Owner = owner;
		mTGroupDialogWindow.Top = top;
		mTGroupDialogWindow.Left = left;
		if (mTGroupDialogWindow.ShowDialog().Value)
		{
			DataLoadedAtLeastOnce = false;
			result = true;
			value = mTGroupDialogWindow.Directory.NewCode;
			comment = mTGroupDialogWindow.Directory.Name;
			LoadData();
		}
		return result;
	}
}
