using System.Windows;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Exceptions;
using ArmSoft.SME.Common.Extensions;
using ArmSoft.SME.Common.MetaData;
using ArmSoft.SME.Core.UI.Controls;
using DevExpress.Xpf.Editors;

namespace ArmSoft.SME.Core.UI.MetaData.DocumentExtension;

public class NumberDataItemDialogWindow : DataItemDialogWindow
{
	private const short _numberMaxLength = 28;

	private const short _precisionMaxLength = 6;

	private CheckEditExt _requiredControl;

	private SpinEditExt _lengthControl;

	private SpinEditExt _precisionControl;

	private CheckEditExt _allowNegativeNumbersControl;

	private PopupCalcEditExt _overriddenDefaultValueControl;

	private NumberDataItem NumberDataItem => (NumberDataItem)DataItem;

	public NumberDataItemDialogWindow(NumberDataItem dataItem, Window owner = null)
		: base(dataItem, owner)
	{
	}

	protected override void AddSpecificControls()
	{
		_requiredControl = AddBooleanControl("Required", ArmSoft.SME.Common.Resources.IsRequired);
		_requiredControl.IsChecked = NumberDataItem.Required;
		_lengthControl = AddSpinEdit("Length", ArmSoft.SME.Common.Resources.Length, 1m, 18m, 60.0);
		_lengthControl.Value = NumberDataItem.Length;
		_lengthControl.EditValueType = typeof(short);
		_lengthControl.EditValueChanged += LengthControl_EditValueChanged;
		_precisionControl = AddSpinEdit("Precision", ArmSoft.SME.Common.Resources.PrecisionDigitsCount, 0m, 4m, 60.0);
		_precisionControl.Value = NumberDataItem.Precision;
		_precisionControl.EditValueType = typeof(short);
		_precisionControl.EditValueChanged += PrecisionControl_EditValueChanged;
		_allowNegativeNumbersControl = AddBooleanControl("AllowNegativeNumbers", ArmSoft.SME.Common.Resources.AllowNegativeNumbers);
		_allowNegativeNumbersControl.IsChecked = NumberDataItem.AllowNegativeNumbers;
		short length = (short)((NumberDataItem.Length > 0) ? NumberDataItem.Length : 28);
		short precision = (short)((NumberDataItem.Precision > 0) ? NumberDataItem.Precision : 6);
		_overriddenDefaultValueControl = AddNumericControl("OverriddenDefaultValue", ArmSoft.SME.Common.Resources.DefaultValue, length, precision, isRequired: false, NumberDataItem.AllowNegativeNumbers, NumberDataItem.UseCurrencyFormat);
		_overriddenDefaultValueControl.Value = NumberDataItem.OverriddenDefaultValue;
		UpdateDefaultValueControl();
	}

	private void UseCurrencyFormatControl_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		UpdateDefaultValueControl();
	}

	protected override void UpdateSpecificProperties()
	{
		NumberDataItem.Required = _requiredControl.IsChecked == true;
		NumberDataItem.Length = (short)_lengthControl.Value;
		NumberDataItem.Precision = (short)_precisionControl.Value;
		NumberDataItem.AllowNegativeNumbers = _allowNegativeNumbersControl.IsChecked == true;
		NumberDataItem.OverriddenDefaultValue = _overriddenDefaultValueControl.Value;
	}

	private void LengthControl_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		if (e.NewValue is short maxLength)
		{
			_overriddenDefaultValueControl.MaxLength = maxLength;
			UpdateDefaultValueControl();
		}
	}

	private void PrecisionControl_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		if (e.NewValue is short precision)
		{
			_overriddenDefaultValueControl.Precision = precision;
			UpdateDefaultValueControl();
		}
	}

	protected override void DialogValidation(DialogWindowValidationArgs args)
	{
		if (_precisionControl.Value + 2m > _lengthControl.Value)
		{
			throw new RESTException(string.Format(ArmSoft.SME.Common.Resources.InvalidNumberPrecisionForLength, _nameControl.EditText, _lengthControl.Value, _precisionControl.Value));
		}
		base.DialogValidation(args);
	}

	private void UpdateDefaultValueControl()
	{
		_overriddenDefaultValueControl.Mask = (NumberDataItem.UseCurrencyFormat ? DecimalExtension.GetCurrencyFormat((short)_lengthControl.Value, (short)_precisionControl.Value) : DecimalExtension.GetNumberFormat((short)_lengthControl.Value, (short)_precisionControl.Value));
	}
}
