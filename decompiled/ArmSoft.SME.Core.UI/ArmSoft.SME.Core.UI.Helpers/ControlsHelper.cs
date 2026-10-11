using System;
using System.Collections;
using System.Windows;
using System.Windows.Controls;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.CustomTypes;
using ArmSoft.SME.Common.Extensions;
using ArmSoft.SME.Core.UI.Controls;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Editors;
using DevExpress.Xpf.LayoutControl;

namespace ArmSoft.SME.Core.UI.Helpers;

public class ControlsHelper
{
	public static object GetControlValue(Control control)
	{
		if (control is TextEditExt textEditExt)
		{
			if (textEditExt.EditValueType == typeof(decimal))
			{
				return Convert.ToDecimal(textEditExt.Text);
			}
			return textEditExt.Text;
		}
		if (control is ButtonEditExt buttonEditExt)
		{
			return buttonEditExt.Text;
		}
		if (control is DropDownControl dropDownControl)
		{
			return dropDownControl.Code;
		}
		if (control is DateEditControl dateEditControl)
		{
			return dateEditControl.EditValue;
		}
		if (control is TimeEditControl timeEditControl)
		{
			return timeEditControl.TimeSpanValue;
		}
		if (control is PopupCalcEditExt popupCalcEditExt)
		{
			if (popupCalcEditExt.EditValueType == typeof(byte))
			{
				return Convert.ToByte(popupCalcEditExt.EditValue);
			}
			if (popupCalcEditExt.EditValueType == typeof(short))
			{
				return Convert.ToInt16(popupCalcEditExt.EditValue);
			}
			return popupCalcEditExt.EditValue;
		}
		if (control is CheckEditExt checkEditExt)
		{
			return checkEditExt.IsChecked;
		}
		if (control is MultiSelectDropDownControl multiSelectDropDownControl)
		{
			return multiSelectDropDownControl.SelectedString;
		}
		if (control is DropDownCommentControl dropDownCommentControl)
		{
			return dropDownCommentControl.Text;
		}
		if (control is ComboBoxEditExt comboBoxEditExt)
		{
			return comboBoxEditExt.Text;
		}
		if (control is ExchangeRateControl exchangeRateControl)
		{
			return new ExchangeRate(exchangeRateControl.Base, exchangeRateControl.Rate);
		}
		if (control is MemoEditControl memoEditControl)
		{
			return memoEditControl.Text;
		}
		if (control is RadioButtonExt { IsChecked: var isChecked })
		{
			if (!isChecked.Value)
			{
				return "0";
			}
			return "1";
		}
		if (control is SpinEditExt spinEditExt)
		{
			return spinEditExt.EditValue;
		}
		if (control is ComboBoxExt comboBoxExt)
		{
			return comboBoxExt.SelectedIndex;
		}
		if (control is MonthYearControl monthYearControl)
		{
			DateTime? dateTime = (DateTime?)monthYearControl.EditValue;
			if (dateTime.HasValue)
			{
				return dateTime.Value.FormatYYYYMMDD();
			}
			return "";
		}
		if (control is ImageEdit imageEdit)
		{
			return (byte[])imageEdit.EditValue;
		}
		if (control is PasswordBoxEditExt passwordBoxEditExt)
		{
			return passwordBoxEditExt.Text;
		}
		if (control is DocumentGridControl result)
		{
			return result;
		}
		throw new NotImplementedException();
	}

	public static void SetControlValue(Control control, object value)
	{
		if (control is TextEditExt textEditExt)
		{
			textEditExt.Text = Convert.ToString(value);
		}
		else if (control is ButtonEditExt buttonEditExt)
		{
			buttonEditExt.Text = Convert.ToString(value);
		}
		else if (control is DropDownControl dropDownControl)
		{
			dropDownControl.Code = Convert.ToString(value);
		}
		else if (control is DateEditControl dateEditControl)
		{
			dateEditControl.EditValue = (DateOnly?)value;
		}
		else if (control is TimeEditControl timeEditControl)
		{
			timeEditControl.EditValue = timeEditControl.TimeSpanToString((TimeSpan)value);
		}
		else if (control is PopupCalcEditExt popupCalcEditExt)
		{
			if (popupCalcEditExt.EditValueType == typeof(int))
			{
				popupCalcEditExt.EditValue = (popupCalcEditExt.AllowNullInput ? ((int?)value) : new int?(Convert.ToInt32(value)));
			}
			else if (popupCalcEditExt.EditValueType == typeof(short))
			{
				popupCalcEditExt.EditValue = (popupCalcEditExt.AllowNullInput ? ((short?)value) : new short?(Convert.ToInt16(value)));
			}
			else
			{
				popupCalcEditExt.EditValue = Convert.ToDecimal(value);
			}
		}
		else if (control is CheckEditExt checkEditExt)
		{
			checkEditExt.IsChecked = (checkEditExt.IsThreeState ? ((bool?)value) : new bool?((bool)value));
		}
		else if (control is MultiSelectDropDownControl multiSelectDropDownControl)
		{
			multiSelectDropDownControl.SelectedString = Convert.ToString(value);
		}
		else if (control is DropDownCommentControl dropDownCommentControl)
		{
			dropDownCommentControl.Text = Convert.ToString(value);
		}
		else if (control is ComboBoxEditExt comboBoxEditExt)
		{
			comboBoxEditExt.Text = Convert.ToString(value);
		}
		else if (control is ExchangeRateControl exchangeRateControl)
		{
			if (value != null)
			{
				ExchangeRate exchangeRate = (ExchangeRate)value;
				exchangeRateControl.Rate = exchangeRate.Rate;
				exchangeRateControl.Base = exchangeRate.Base;
			}
		}
		else if (control is MemoEditControl memoEditControl)
		{
			memoEditControl.EditValue = Convert.ToString(value);
		}
		else if (control is RadioButtonExt radioButtonExt)
		{
			radioButtonExt.IsChecked = Convert.ToBoolean(value);
		}
		else if (control is SpinEditExt spinEditExt)
		{
			decimal result2;
			if (spinEditExt.EditValueType == typeof(byte))
			{
				if (byte.TryParse(Convert.ToString(value), out var result))
				{
					spinEditExt.EditValue = result;
				}
			}
			else if (spinEditExt.EditValueType == typeof(decimal) && decimal.TryParse(Convert.ToString(value), out result2))
			{
				spinEditExt.EditValue = result2;
			}
		}
		else if (control is ComboBoxExt comboBoxExt)
		{
			if (int.TryParse(Convert.ToString(value), out var result3))
			{
				comboBoxExt.SelectedIndex = result3;
			}
		}
		else if (control is MonthYearControl monthYearControl)
		{
			if (string.IsNullOrWhiteSpace(Convert.ToString(value)))
			{
				monthYearControl.EditValue = null;
			}
			else
			{
				monthYearControl.EditValue = (DateTime?)value;
			}
		}
		else if (control is ImageEdit imageEdit)
		{
			imageEdit.EditValue = (byte[])value;
		}
		else
		{
			_ = control is DocumentGridControl;
		}
	}

	public static void SetControlRequired(Control control)
	{
		if (control is TextEditExt textEditExt)
		{
			textEditExt.IsRequired = true;
		}
		else if (control is ButtonEditExt buttonEditExt)
		{
			buttonEditExt.IsRequired = true;
		}
		else if (control is DropDownControl dropDownControl)
		{
			dropDownControl.IsRequired = true;
		}
		else if (control is DateEditControl dateEditControl)
		{
			dateEditControl.IsRequired = true;
		}
		else if (control is TimeEditControl timeEditControl)
		{
			timeEditControl.IsRequired = true;
		}
		else if (control is PopupCalcEditExt popupCalcEditExt)
		{
			popupCalcEditExt.IsRequired = true;
		}
		else
		{
			if (control is CheckEditExt)
			{
				return;
			}
			if (control is MultiSelectDropDownControl multiSelectDropDownControl)
			{
				multiSelectDropDownControl.IsRequired = true;
			}
			else if (control is DropDownCommentControl dropDownCommentControl)
			{
				dropDownCommentControl.IsRequired = true;
			}
			else if (control is ComboBoxEditExt comboBoxEditExt)
			{
				comboBoxEditExt.IsRequired = true;
			}
			else if (control is ExchangeRateControl exchangeRateControl)
			{
				exchangeRateControl.IsRequired = true;
			}
			else if (control is MemoEditControl memoEditControl)
			{
				memoEditControl.IsRequired = true;
			}
			else
			{
				if (control is RadioButtonExt)
				{
					return;
				}
				if (control is SpinEditExt spinEditExt)
				{
					spinEditExt.IsRequired = true;
				}
				else if (!(control is ComboBoxExt))
				{
					if (control is MonthYearControl monthYearControl)
					{
						monthYearControl.IsRequired = true;
					}
					else if (!(control is ImageEdit))
					{
						_ = control is DocumentGridControl;
					}
				}
			}
		}
	}

	public static bool IsControlReadonly(Control control)
	{
		if (control is TextEditExt textEditExt)
		{
			if (!textEditExt.IsReadOnly)
			{
				return !textEditExt.IsEnabled;
			}
			return true;
		}
		if (control is ButtonEditExt buttonEditExt)
		{
			if (!buttonEditExt.IsReadOnly)
			{
				return !buttonEditExt.IsEnabled;
			}
			return true;
		}
		if (control is DropDownControl dropDownControl)
		{
			if (!dropDownControl.IsReadOnly)
			{
				return !dropDownControl.IsEnabled;
			}
			return true;
		}
		if (control is DateEditControl dateEditControl)
		{
			if (!dateEditControl.IsReadOnly)
			{
				return !dateEditControl.IsEnabled;
			}
			return true;
		}
		if (control is TimeEditControl timeEditControl)
		{
			if (!timeEditControl.IsReadOnly)
			{
				return !timeEditControl.IsEnabled;
			}
			return true;
		}
		if (control is PopupCalcEditExt popupCalcEditExt)
		{
			if (!popupCalcEditExt.IsReadOnly)
			{
				return !popupCalcEditExt.IsEnabled;
			}
			return true;
		}
		if (control is CheckEditExt checkEditExt)
		{
			if (!checkEditExt.IsReadOnly)
			{
				return !checkEditExt.IsEnabled;
			}
			return true;
		}
		if (control is MultiSelectDropDownControl multiSelectDropDownControl)
		{
			if (!multiSelectDropDownControl.IsReadOnly)
			{
				return !multiSelectDropDownControl.IsEnabled;
			}
			return true;
		}
		if (control is DropDownCommentControl dropDownCommentControl)
		{
			if (!dropDownCommentControl.IsReadOnly)
			{
				return !dropDownCommentControl.IsEnabled;
			}
			return true;
		}
		if (control is ComboBoxEditExt comboBoxEditExt)
		{
			if (!comboBoxEditExt.IsReadOnly)
			{
				return !comboBoxEditExt.IsEnabled;
			}
			return true;
		}
		if (control is ExchangeRateControl exchangeRateControl)
		{
			if (!exchangeRateControl.IsReadOnly)
			{
				return !exchangeRateControl.IsEnabled;
			}
			return true;
		}
		if (control is MemoEditControl memoEditControl)
		{
			if (!memoEditControl.IsReadOnly)
			{
				return !memoEditControl.IsEnabled;
			}
			return true;
		}
		if (control is RadioButtonExt radioButtonExt)
		{
			return !radioButtonExt.IsEnabled;
		}
		if (control is SpinEditExt spinEditExt)
		{
			if (!spinEditExt.IsReadOnly)
			{
				return !spinEditExt.IsEnabled;
			}
			return true;
		}
		if (control is ComboBoxExt comboBoxExt)
		{
			if (!comboBoxExt.IsReadOnly)
			{
				return !comboBoxExt.IsEnabled;
			}
			return true;
		}
		if (control is MonthYearControl monthYearControl)
		{
			if (!monthYearControl.IsReadOnly)
			{
				return !monthYearControl.IsEnabled;
			}
			return true;
		}
		if (control is ImageEdit imageEdit)
		{
			if (!imageEdit.IsReadOnly)
			{
				return !imageEdit.IsEnabled;
			}
			return true;
		}
		if (control is PasswordBoxEditExt passwordBoxEditExt)
		{
			if (!passwordBoxEditExt.IsReadOnly)
			{
				return !passwordBoxEditExt.IsEnabled;
			}
			return true;
		}
		if (control is DocumentGridControl documentGridControl)
		{
			if (!documentGridControl.IsReadOnly)
			{
				return !documentGridControl.IsEnabled;
			}
			return true;
		}
		if (control is SimpleButton simpleButton)
		{
			return !simpleButton.IsEnabled;
		}
		return false;
	}

	public static void SetFocus(Control focusableControl, bool selectAll = true)
	{
		if (focusableControl == null)
		{
			return;
		}
		FocusingParentTab(focusableControl);
		if (focusableControl is DropDownControl dropDownControl)
		{
			dropDownControl.InnerCodeEdit.Focus();
			if (selectAll)
			{
				dropDownControl.InnerCodeEdit.SelectAll();
			}
		}
		else if (focusableControl is DateEditControl dateEditControl)
		{
			dateEditControl.Focus();
			if (selectAll)
			{
				dateEditControl.SelectAll();
			}
		}
		else if (focusableControl is PopupCalcEditExt popupCalcEditExt)
		{
			popupCalcEditExt.Focus();
			if (selectAll)
			{
				popupCalcEditExt.SelectAll();
			}
		}
		else if (focusableControl is CheckEditExt checkEditExt)
		{
			checkEditExt.Focus();
		}
		else if (focusableControl is MultiSelectDropDownControl multiSelectDropDownControl)
		{
			multiSelectDropDownControl.InnerCodeEdit.Focus();
			if (selectAll)
			{
				multiSelectDropDownControl.InnerCodeEdit.SelectAll();
			}
		}
		else if (focusableControl is DropDownCommentControl dropDownCommentControl)
		{
			dropDownCommentControl.InnerCodeEdit.Focus();
			if (selectAll)
			{
				dropDownCommentControl.InnerCodeEdit.SelectAll();
			}
		}
		else if (focusableControl is ComboBoxEditExt comboBoxEditExt)
		{
			comboBoxEditExt.Focus();
			if (selectAll)
			{
				comboBoxEditExt.SelectAll();
			}
		}
		else if (focusableControl is ExchangeRateControl exchangeRateControl)
		{
			exchangeRateControl.InnerRateControl.Focus();
			if (selectAll)
			{
				exchangeRateControl.InnerRateControl.SelectAll();
			}
		}
		else if (focusableControl is ButtonEditExt buttonEditExt)
		{
			buttonEditExt.Focus();
			if (selectAll)
			{
				buttonEditExt.SelectAll();
			}
		}
		else if (focusableControl is TimeEditControl timeEditControl)
		{
			timeEditControl.Focus();
			if (selectAll)
			{
				timeEditControl.SelectAll();
			}
		}
		else if (focusableControl is MemoEditControl memoEditControl)
		{
			memoEditControl.Focus();
			if (selectAll)
			{
				memoEditControl.SelectAll();
			}
		}
		else if (focusableControl is TextEditExt textEditExt)
		{
			textEditExt.Focus();
			if (selectAll)
			{
				textEditExt.SelectAll();
			}
		}
		else
		{
			focusableControl.Focus();
		}
	}

	public static void FocusingParentTab(Control focusableControl)
	{
		if (focusableControl.IsVisible)
		{
			return;
		}
		DependencyObject parent = LogicalTreeHelper.GetParent((DependencyObject)(object)focusableControl);
		DXTabItem dXTabItem = null;
		while (parent != null)
		{
			if (parent is DXTabItem dXTabItem2)
			{
				dXTabItem = dXTabItem2;
			}
			parent = LogicalTreeHelper.GetParent(parent);
		}
		if (dXTabItem != null)
		{
			dXTabItem.Owner.SelectedContainer = dXTabItem;
		}
	}

	public static bool DoValidateAndSetFocus(Control control, out string gridErrorMgs, bool validateWithoutRequired = false, bool showValidationError = false)
	{
		gridErrorMgs = null;
		if (control is DropDownControl dropDownControl)
		{
			if (!((!validateWithoutRequired) ? dropDownControl.DoValidate() : dropDownControl.DoValidateWithoutRequired()))
			{
				SetFocus(dropDownControl);
				if (showValidationError)
				{
					throw new ASException(string.Format(ArmSoft.SME.Common.Resources.InvaildFieldValue, dropDownControl.GetLayoutItem()?.Label?.ToString() ?? "", dropDownControl.Code));
				}
				return false;
			}
		}
		else if (control is DateEditControl dateEditControl)
		{
			if (!((!validateWithoutRequired) ? dateEditControl.DoValidate() : dateEditControl.DoValidateWithoutRequired()))
			{
				SetFocus(dateEditControl);
				if (showValidationError)
				{
					throw new ASException(string.Format(ArmSoft.SME.Common.Resources.InvaildFieldValue, dateEditControl.GetLayoutItem()?.Label?.ToString() ?? "", dateEditControl?.EditText ?? ""));
				}
				return false;
			}
		}
		else if (control is TimeEditControl timeEditControl)
		{
			if (!((!validateWithoutRequired) ? timeEditControl.DoValidate() : timeEditControl.DoValidateWithoutRequired()))
			{
				SetFocus(timeEditControl);
				if (showValidationError)
				{
					throw new ASException(string.Format(ArmSoft.SME.Common.Resources.InvaildFieldValue, timeEditControl.GetLayoutItem()?.Label?.ToString() ?? "", timeEditControl.EditText ?? ""));
				}
				return false;
			}
		}
		else if (control is PopupCalcEditExt popupCalcEditExt)
		{
			if (!((!validateWithoutRequired) ? popupCalcEditExt.DoValidate() : popupCalcEditExt.DoValidateWithoutRequired()))
			{
				SetFocus(popupCalcEditExt);
				if (showValidationError)
				{
					throw new ASException(string.Format(ArmSoft.SME.Common.Resources.InvaildFieldValue, popupCalcEditExt.GetLayoutItem()?.Label?.ToString() ?? "", popupCalcEditExt.EditText));
				}
				return false;
			}
		}
		else if (control is CheckEditExt checkEditExt)
		{
			if (!checkEditExt.DoValidate())
			{
				SetFocus(checkEditExt);
				if (showValidationError)
				{
					throw new ASException(string.Format(ArmSoft.SME.Common.Resources.InvaildFieldValue, checkEditExt.GetLayoutItem()?.Label?.ToString() ?? "", checkEditExt.EditValue?.ToString() ?? ""));
				}
				return false;
			}
		}
		else if (control is MultiSelectDropDownControl multiSelectDropDownControl)
		{
			if (!((!validateWithoutRequired) ? multiSelectDropDownControl.DoValidate() : multiSelectDropDownControl.DoValidateWithoutRequired()))
			{
				SetFocus(multiSelectDropDownControl);
				if (showValidationError)
				{
					throw new ASException(string.Format(ArmSoft.SME.Common.Resources.InvaildFieldValue, control.GetLayoutItem()?.Label?.ToString() ?? "", multiSelectDropDownControl.CodeEdit?.EditText ?? ""));
				}
				return false;
			}
		}
		else if (control is PasswordBoxEditExt passwordBoxEditExt)
		{
			if (!passwordBoxEditExt.DoValidate())
			{
				SetFocus(passwordBoxEditExt);
				if (showValidationError)
				{
					throw new ASException(string.Format(ArmSoft.SME.Common.Resources.InvaildFieldValue, passwordBoxEditExt.GetLayoutItem()?.Label?.ToString() ?? "", passwordBoxEditExt.EditText ?? ""));
				}
				return false;
			}
		}
		else if (control is DropDownCommentControl dropDownCommentControl)
		{
			if (!((!validateWithoutRequired) ? dropDownCommentControl.DoValidate() : dropDownCommentControl.DoValidateWithoutRequired()))
			{
				SetFocus(dropDownCommentControl);
				if (showValidationError)
				{
					throw new ASException(string.Format(ArmSoft.SME.Common.Resources.InvaildFieldValue, dropDownCommentControl.GetLayoutItem()?.Label?.ToString() ?? "", dropDownCommentControl.CodeEdit?.EditText ?? ""));
				}
				return false;
			}
		}
		else if (control is ComboBoxEditExt comboBoxEditExt)
		{
			if (!((!validateWithoutRequired) ? comboBoxEditExt.DoValidate() : comboBoxEditExt.DoValidateWithoutRequired()))
			{
				SetFocus(comboBoxEditExt);
				if (showValidationError)
				{
					throw new ASException(string.Format(ArmSoft.SME.Common.Resources.InvaildFieldValue, comboBoxEditExt.GetLayoutItem()?.Label?.ToString() ?? "", comboBoxEditExt.EditText ?? ""));
				}
				return false;
			}
		}
		else if (control is ExchangeRateControl exchangeRateControl)
		{
			if (!((!validateWithoutRequired) ? exchangeRateControl.DoValidate() : exchangeRateControl.DoValidateWithoutRequired()))
			{
				SetFocus(exchangeRateControl);
				return false;
			}
		}
		else if (control is ButtonEditExt buttonEditExt)
		{
			if (!((!validateWithoutRequired) ? buttonEditExt.DoValidate() : buttonEditExt.DoValidateWithoutRequired()))
			{
				SetFocus(buttonEditExt);
				if (showValidationError)
				{
					throw new ASException(string.Format(ArmSoft.SME.Common.Resources.InvaildFieldValue, buttonEditExt.GetLayoutItem()?.Label?.ToString() ?? "", buttonEditExt.EditText ?? ""));
				}
				return false;
			}
		}
		else if (control is TextEditExt textEditExt)
		{
			if (!((!validateWithoutRequired) ? textEditExt.DoValidate() : textEditExt.DoValidateWithoutRequired()))
			{
				SetFocus(textEditExt);
				if (showValidationError)
				{
					throw new ASException(string.Format(ArmSoft.SME.Common.Resources.InvaildFieldValue, textEditExt.GetLayoutItem()?.Label?.ToString() ?? "", textEditExt.EditText ?? ""));
				}
				return false;
			}
		}
		else if (control is SpinEditExt spinEditExt)
		{
			if (!spinEditExt.DoValidate())
			{
				SetFocus(spinEditExt);
				if (showValidationError)
				{
					throw new ASException(string.Format(ArmSoft.SME.Common.Resources.InvaildFieldValue, spinEditExt.GetLayoutItem()?.Label?.ToString() ?? "", spinEditExt.EditText ?? ""));
				}
				return false;
			}
		}
		else if (control is MemoEditControl memoEditControl)
		{
			if (!((!validateWithoutRequired) ? memoEditControl.DoValidate() : memoEditControl.DoValidateWithoutRequired()))
			{
				SetFocus(memoEditControl);
				if (showValidationError)
				{
					throw new ASException(string.Format(ArmSoft.SME.Common.Resources.InvaildFieldValue, memoEditControl.GetLayoutItem()?.Label?.ToString() ?? "", memoEditControl.EditText ?? ""));
				}
				return false;
			}
		}
		else if (control is MonthYearControl monthYearControl)
		{
			if (!monthYearControl.DoValidate())
			{
				SetFocus(monthYearControl);
				if (showValidationError)
				{
					throw new ASException(string.Format(ArmSoft.SME.Common.Resources.InvaildFieldValue, monthYearControl.GetLayoutItem()?.Label?.ToString() ?? "", monthYearControl.EditText ?? ""));
				}
				return false;
			}
		}
		else if (control is DocumentGridControl documentGridControl)
		{
			documentGridControl.View.CommitEditing();
			int num = ((documentGridControl.ItemsSource is ICollection collection) ? collection.Count : 0);
			if (!validateWithoutRequired && documentGridControl.IsRequired && num == 0)
			{
				SetFocus(documentGridControl);
				gridErrorMgs = string.Format(ArmSoft.SME.Common.Resources.NamedListIsEpmty, string.IsNullOrWhiteSpace(documentGridControl.Caption) ? documentGridControl.ToolTip : documentGridControl.Caption);
				if (showValidationError)
				{
					throw new ASException(gridErrorMgs);
				}
				return false;
			}
			if (documentGridControl.CalculateHasValidationError(out var errorMessage, validateWithoutRequired))
			{
				SetFocus(documentGridControl);
				gridErrorMgs = errorMessage;
				if (showValidationError)
				{
					throw new ASException(gridErrorMgs);
				}
				return false;
			}
		}
		else if (!(control is SpecialDateButton) && !(control is Button) && !(control is ImageEdit) && !(control is Label) && !(control is RadioButtonExt))
		{
			throw new ArgumentOutOfRangeException("Not supported type of control");
		}
		return true;
	}
}
