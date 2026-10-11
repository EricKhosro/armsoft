using System.Windows;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.MetaData;
using ArmSoft.SME.Core.UI.Controls;

namespace ArmSoft.SME.Core.UI.MetaData.DocumentExtension;

public class DateDataItemDialogWindow : DataItemDialogWindow
{
	private CheckEditExt _requiredControl;

	private DateDataItem DateDataItem => (DateDataItem)DataItem;

	public DateDataItemDialogWindow(DateDataItem dataItem, Window owner = null)
		: base(dataItem, owner)
	{
	}

	protected override void AddSpecificControls()
	{
		_requiredControl = AddBooleanControl("Required", ArmSoft.SME.Common.Resources.IsRequired);
		_requiredControl.IsChecked = DateDataItem.Required;
	}

	protected override void UpdateSpecificProperties()
	{
		DateDataItem.Required = _requiredControl.IsChecked == true;
	}
}
