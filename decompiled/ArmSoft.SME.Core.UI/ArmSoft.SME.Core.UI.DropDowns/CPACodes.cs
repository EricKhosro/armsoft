using System.Windows;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Models.Directories;
using ArmSoft.SME.Common.SmartEnums;
using ArmSoft.SME.Core.UI.DataViews;
using ArmSoft.SME.Core.UI.Directories;
using ArmSoft.SME.Core.UI.DropDownsData;

namespace ArmSoft.SME.Core.UI.DropDowns;

[DropDown(5)]
public class CPACodes : DropDownView<CPACodeDataProvider.DataRow, CPACodeDataProvider.Param>
{
	private readonly string _type;

	public override string Caption
	{
		get
		{
			if (_type == ((byte)1).ToString())
			{
				return Resources.ADGGAAClassification;
			}
			return Resources.CPAClassifications;
		}
	}

	public override ReloadMode ReloadMode => ReloadMode.Reloadable;

	public override bool AddNewSupported => Settings.UserAccessService.GetDirectoryPermission(DirectoryTypeSE.CPAClassifier.Value).AllowAdd;

	public override bool EditSupported => Settings.UserAccessService.GetDirectoryPermission(DirectoryTypeSE.CPAClassifier.Value).AllowEdit;

	public CPACodes()
	{
		DataProvider = new CPACodeDataProvider();
		FillColumnsFromDataProvider();
		Columns.AddSorting("TypeName", descending: true);
		Columns.AddSorting("Code");
	}

	public CPACodes(string type = "")
	{
		_type = type;
		DataProvider = new CPACodeDataProvider();
		FillColumnsFromDataProvider();
		Columns.AddSorting("TypeName", descending: true);
		Columns.AddSorting("Code");
	}

	protected override void ApplyParametersToDataProvider()
	{
		ParametersProvider.Type = _type;
	}

	public override string ValueMember(int index)
	{
		return Data[index].Code;
	}

	public override short ValueMemberLength()
	{
		return 12;
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
		IDirectoryDialogWindow directoryDialogWindow = (isNew ? Settings.DirectoryService.CreateDialogWindow(DirectoryTypeSE.CPAClassifier, DialogWindowMode.NewMode) : Settings.DirectoryService.CreateDialogWindow(Settings.DirectoryService.Load(DirectoryTypeSE.CPAClassifier, value), DialogWindowMode.EditMode));
		Window window = (Window)directoryDialogWindow;
		window.Owner = owner;
		window.Top = top;
		window.Left = left;
		if (directoryDialogWindow.ShowDialog() == true)
		{
			DataLoadedAtLeastOnce = false;
			result = true;
			Directory directory = (Directory)directoryDialogWindow.GetType().GetProperty("Directory").GetValue(directoryDialogWindow);
			value = directory.Code;
			comment = directory.Name;
		}
		return result;
	}
}
