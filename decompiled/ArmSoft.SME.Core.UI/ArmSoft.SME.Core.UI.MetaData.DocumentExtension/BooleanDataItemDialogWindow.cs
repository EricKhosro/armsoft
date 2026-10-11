using System.Windows;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.MetaData;
using ArmSoft.SME.Core.UI.Controls;

namespace ArmSoft.SME.Core.UI.MetaData.DocumentExtension;

public class BooleanDataItemDialogWindow : DataItemDialogWindow
{
	private CheckEditExt _overriddenDefaultValueControl;

	private BooleanDataItem BooleanDataItem => (BooleanDataItem)DataItem;

	public BooleanDataItemDialogWindow(BooleanDataItem dataItem, Window owner = null)
		: base(dataItem, owner)
	{
	}

	protected override void AddSpecificControls()
	{
		_overriddenDefaultValueControl = AddBooleanControl("OverriddenDefaultValue", ArmSoft.SME.Common.Resources.DefaultValue);
		_overriddenDefaultValueControl.IsChecked = BooleanDataItem.OverriddenDefaultValue;
	}

	protected override void UpdateSpecificProperties()
	{
		BooleanDataItem.OverriddenDefaultValue = _overriddenDefaultValueControl.IsChecked == true;
	}
}
