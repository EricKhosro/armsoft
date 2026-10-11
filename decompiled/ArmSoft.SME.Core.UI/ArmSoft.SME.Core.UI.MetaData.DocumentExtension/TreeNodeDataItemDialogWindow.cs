using System.Windows;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.MetaData;
using ArmSoft.SME.Core.UI.Controls;
using ArmSoft.SME.Core.UI.DropDowns;

namespace ArmSoft.SME.Core.UI.MetaData.DocumentExtension;

public class TreeNodeDataItemDialogWindow : DataItemDialogWindow
{
	private CheckEditExt _requiredControl;

	private CheckEditExt _leafsOnlyControl;

	private DropDownControl _treeNameControl;

	private TextEditExt _overriddenDefaultValueControl;

	private CheckEditExt _allowAdditionButtonsControl;

	private TreeNodeDataItem TreeNodeDataItem => (TreeNodeDataItem)DataItem;

	public TreeNodeDataItemDialogWindow(TreeNodeDataItem dataItem, Window owner = null)
		: base(dataItem, owner)
	{
	}

	protected override void AddSpecificControls()
	{
		_requiredControl = AddBooleanControl("Required", ArmSoft.SME.Common.Resources.IsRequired);
		_requiredControl.IsChecked = TreeNodeDataItem.Required;
		_leafsOnlyControl = AddBooleanControl("LeafsOnly", ArmSoft.SME.Common.Resources.OnlyLeafs);
		_leafsOnlyControl.IsChecked = TreeNodeDataItem.LeafsOnly;
		_treeNameControl = AddDropDownControl("TreeName", ArmSoft.SME.Common.Resources.TreeName, new TreesList(), isRequired: true);
		if (!string.IsNullOrWhiteSpace(TreeNodeDataItem.TreeName))
		{
			_treeNameControl.Code = TreeNodeDataItem.TreeName;
		}
		_overriddenDefaultValueControl = AddStringControl("OverriddenDefaultValue", ArmSoft.SME.Common.Resources.DefaultValue, 100);
		_overriddenDefaultValueControl.Text = TreeNodeDataItem.OverriddenDefaultValue ?? string.Empty;
		_allowAdditionButtonsControl = AddBooleanControl("AllowAdditionButtons", ArmSoft.SME.Common.Resources.AllowAdditionButtons);
		_allowAdditionButtonsControl.IsChecked = TreeNodeDataItem.AllowAdditionButtons;
	}

	protected override void UpdateSpecificProperties()
	{
		TreeNodeDataItem.Required = _requiredControl.IsChecked == true;
		TreeNodeDataItem.LeafsOnly = _leafsOnlyControl.IsChecked ?? true;
		TreeNodeDataItem.TreeName = _treeNameControl.Code?.Trim() ?? string.Empty;
		TreeNodeDataItem.OverriddenDefaultValue = _overriddenDefaultValueControl.Text?.Trim() ?? string.Empty;
		TreeNodeDataItem.AllowAdditionButtons = _allowAdditionButtonsControl.IsChecked == true;
	}
}
