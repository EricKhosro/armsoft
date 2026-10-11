using System.Windows;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.MetaData;
using ArmSoft.SME.Core.UI.Controls;
using DevExpress.Xpf.Editors;

namespace ArmSoft.SME.Core.UI.MetaData.DocumentExtension;

public class StringDataItemDialogWindow : DataItemDialogWindow
{
	private const short _stringLengthLen = 3;

	private const short _stringMaxLength = 512;

	private CheckEditExt _requiredControl;

	private PopupCalcEditExt _lengthControl;

	private TextEditExt _overriddenDefaultValueControl;

	private StringDataItem StringDataItem => (StringDataItem)DataItem;

	public StringDataItemDialogWindow(StringDataItem dataItem, Window owner = null)
		: base(dataItem, owner)
	{
	}

	protected override void AddSpecificControls()
	{
		_requiredControl = AddBooleanControl("Required", ArmSoft.SME.Common.Resources.IsRequired);
		_requiredControl.IsChecked = StringDataItem.Required;
		_lengthControl = AddNumericControl("Length", ArmSoft.SME.Common.Resources.Length, 3, 0, isRequired: true);
		_lengthControl.Value = StringDataItem.Length;
		_lengthControl.EditValueType = typeof(short);
		_lengthControl.EditValueChanged += LengthControl_EditValueChanged;
		short length = (short)((StringDataItem.Length > 0) ? StringDataItem.Length : 512);
		_overriddenDefaultValueControl = AddStringControl("OverriddenDefaultValue", ArmSoft.SME.Common.Resources.DefaultValue, length);
		_overriddenDefaultValueControl.Text = StringDataItem.OverriddenDefaultValue ?? string.Empty;
	}

	protected override void UpdateSpecificProperties()
	{
		StringDataItem.Required = _requiredControl.IsChecked == true;
		StringDataItem.Length = (short)_lengthControl.Value;
		StringDataItem.OverriddenDefaultValue = _overriddenDefaultValueControl.Text?.Trim() ?? string.Empty;
	}

	private void LengthControl_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		if (e.NewValue is short maxLength)
		{
			_overriddenDefaultValueControl.MaxLength = maxLength;
		}
	}
}
