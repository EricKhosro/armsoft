using System.Windows;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.MetaData;
using ArmSoft.SME.Core.UI.Controls;

namespace ArmSoft.SME.Core.UI.MetaData.DocumentExtension;

public class TimeDataItemDialogWindow : DataItemDialogWindow
{
	private CheckEditExt _requiredControl;

	private CheckEditExt _showSecondsControl;

	private TimeDataItem TimeDataItem => (TimeDataItem)DataItem;

	public TimeDataItemDialogWindow(TimeDataItem dataItem, Window owner = null)
		: base(dataItem, owner)
	{
	}

	protected override void AddSpecificControls()
	{
		_requiredControl = AddBooleanControl("Required", ArmSoft.SME.Common.Resources.IsRequired);
		_requiredControl.IsChecked = TimeDataItem.Required;
		_showSecondsControl = AddBooleanControl("ShowSeconds", ArmSoft.SME.Common.Resources.ShowSeconds);
		_showSecondsControl.IsChecked = TimeDataItem.ShowSeconds;
	}

	protected override void UpdateSpecificProperties()
	{
		TimeDataItem.Required = _requiredControl.IsChecked == true;
		TimeDataItem.ShowSeconds = _showSecondsControl.IsChecked == true;
	}
}
