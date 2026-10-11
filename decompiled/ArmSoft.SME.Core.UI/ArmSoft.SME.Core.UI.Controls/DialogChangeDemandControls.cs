using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using ArmSoft.SME.Core.UI.DataViews;
using DevExpress.Xpf.Editors;
using DevExpress.Xpf.LayoutControl;

namespace ArmSoft.SME.Core.UI.Controls;

public static class DialogChangeDemandControls
{
	private class ChangeDemandDropDownControl
	{
		private readonly bool _isControlRequired;

		public DropDownControl Control;

		private readonly CheckEditExt _checkControl;

		public ChangeDemandDropDownControl(DialogWindow dialog, string controlName, string controlCaption, IDropDownView idv, bool isRequired = false, bool allowAdditionButtons = false, bool storeValue = false, bool isShowComment = true, bool isCheckValue = true, int codeLength = -1, LayoutGroup changeLayout = null, string captionForStoreValue = "", bool storeWithoutValidation = false)
		{
			_isControlRequired = isRequired;
			string dataItemName = controlName;
			controlName = controlName.Replace('.', '_');
			LayoutGroupExt layoutGroupExt = dialog.AddHorizontalLayoutGroup(changeLayout);
			_checkControl = dialog.AddBooleanControl(layoutGroupExt, controlName + "_CHB", controlCaption, storeValue);
			Control = dialog.AddDropDownControl(layoutGroupExt, controlName, "", idv, _isControlRequired, allowAdditionButtons, storeValue, isShowComment, codeLength, captionForStoreValue);
			Control.CheckValue = isCheckValue;
			dialog.Controls[Control.Name].DataItemName = dataItemName;
			dialog.Controls[Control.Name].SpecData = storeWithoutValidation.ToString();
			CommonEventHandlers.AddIsReadonlyBinding(Control, (LayoutItemWithRequired)layoutGroupExt.Children[0]);
			if (!_checkControl.IsChecked.Value)
			{
				Control.Code = "";
				Control.IsReadOnly = true;
				Control.IsRequired = false;
				Control.DoValidate();
			}
			_checkControl.EditValueChanged += new SafeEventHandler<EditValueChangedEventArgs>(CheckControl_EditValueChanged).Handler;
		}

		private void CheckControl_EditValueChanged(object sender, EditValueChangedEventArgs e)
		{
			Control.Code = "";
			if (_checkControl.IsChecked.Value)
			{
				Control.IsReadOnly = false;
				Control.IsRequired = _isControlRequired;
			}
			else
			{
				Control.IsReadOnly = true;
				Control.IsRequired = false;
				Control.DoValidate();
			}
		}
	}

	private class ChangeDemandMultiSelectDropDownControl
	{
		private readonly bool _isControlRequired;

		public MultiSelectDropDownControl Control;

		private readonly CheckEditExt _checkControl;

		public ChangeDemandMultiSelectDropDownControl(DialogWindow dialog, string controlName, string controlCaption, IDropDownView idv, bool isRequired = false, bool storeValue = false, LayoutGroup changeLayout = null, bool storeWithoutValidation = false)
		{
			_isControlRequired = isRequired;
			string dataItemName = controlName;
			controlName = controlName.Replace('.', '_');
			LayoutGroupExt layoutGroupExt = dialog.AddHorizontalLayoutGroup(changeLayout);
			_checkControl = dialog.AddBooleanControl(layoutGroupExt, controlName + "_CHB", controlCaption, storeValue);
			Control = dialog.AddMultiSelectDropDownControl(layoutGroupExt, controlName, "", idv, _isControlRequired, storeValue);
			dialog.Controls[Control.Name].DataItemName = dataItemName;
			dialog.Controls[Control.Name].SpecData = storeWithoutValidation.ToString();
			CommonEventHandlers.AddIsReadonlyBinding(Control, (LayoutItemWithRequired)layoutGroupExt.Children[0]);
			if (!_checkControl.IsChecked.Value)
			{
				Control.SelectedString = "";
				Control.IsReadOnly = true;
				Control.IsRequired = false;
				Control.DoValidate();
			}
			_checkControl.EditValueChanged += new SafeEventHandler<EditValueChangedEventArgs>(CheckControl_EditValueChanged).Handler;
		}

		private void CheckControl_EditValueChanged(object sender, EditValueChangedEventArgs e)
		{
			Control.SelectedString = "";
			if (_checkControl.IsChecked.Value)
			{
				Control.IsReadOnly = false;
				Control.IsRequired = _isControlRequired;
			}
			else
			{
				Control.IsReadOnly = true;
				Control.IsRequired = false;
				Control.DoValidate();
			}
		}
	}

	private class ChangeDemandDropDownCommentControl
	{
		private readonly bool _isControlRequired;

		public DropDownCommentControl Control;

		private readonly CheckEditExt _checkControl;

		public ChangeDemandDropDownCommentControl(DialogWindow dialog, string controlName, string controlCaption, IDropDownView idv, bool isRequired = false, bool storeValue = false, short length = 50, HorizontalAlignment horizontalAlignment = HorizontalAlignment.Left, LayoutGroup changeLayout = null, string captionForStoreValue = "", bool storeWithoutValidation = false)
		{
			_isControlRequired = isRequired;
			string dataItemName = controlName;
			controlName = controlName.Replace('.', '_');
			LayoutGroupExt layoutGroupExt = dialog.AddHorizontalLayoutGroup(changeLayout);
			_checkControl = dialog.AddBooleanControl(layoutGroupExt, controlName + "_CHB", controlCaption, storeValue);
			Control = dialog.AddDropDownCommentControl(layoutGroupExt, controlName, "", idv, _isControlRequired, storeValue, length, horizontalAlignment, captionForStoreValue);
			dialog.Controls[Control.Name].DataItemName = dataItemName;
			dialog.Controls[Control.Name].SpecData = storeWithoutValidation.ToString();
			CommonEventHandlers.AddIsReadonlyBinding(Control, (LayoutItemWithRequired)layoutGroupExt.Children[0]);
			if (!_checkControl.IsChecked.Value)
			{
				Control.Text = "";
				Control.IsReadOnly = true;
				Control.IsRequired = false;
				Control.DoValidate();
			}
			_checkControl.EditValueChanged += new SafeEventHandler<EditValueChangedEventArgs>(CheckControl_EditValueChanged).Handler;
		}

		private void CheckControl_EditValueChanged(object sender, EditValueChangedEventArgs e)
		{
			Control.Text = "";
			if (_checkControl.IsChecked.Value)
			{
				Control.IsReadOnly = false;
				Control.IsRequired = _isControlRequired;
			}
			else
			{
				Control.IsReadOnly = true;
				Control.IsRequired = false;
				Control.DoValidate();
			}
		}
	}

	private class ChangeDemandStringControl
	{
		private readonly bool _isControlRequired;

		public TextEditExt Control;

		private readonly CheckEditExt _checkControl;

		public ChangeDemandStringControl(DialogWindow dialog, string controlName, string controlCaption, short length, bool isRequired = false, bool storeValue = false, LayoutGroup changeLayout = null, bool storeWithoutValidation = false)
		{
			_isControlRequired = isRequired;
			string dataItemName = controlName;
			controlName = controlName.Replace('.', '_');
			LayoutGroupExt layoutGroupExt = dialog.AddHorizontalLayoutGroup(changeLayout);
			_checkControl = dialog.AddBooleanControl(layoutGroupExt, controlName + "_CHB", controlCaption, storeValue);
			Control = dialog.AddStringControl(layoutGroupExt, controlName, "", length, _isControlRequired, storeValue);
			dialog.Controls[Control.Name].DataItemName = dataItemName;
			dialog.Controls[Control.Name].SpecData = storeWithoutValidation.ToString();
			CommonEventHandlers.AddIsReadonlyBinding(Control, (LayoutItemWithRequired)layoutGroupExt.Children[0]);
			if (!_checkControl.IsChecked.Value)
			{
				Control.Text = "";
				Control.IsReadOnly = true;
				Control.IsRequired = false;
				Control.DoValidate();
			}
			_checkControl.EditValueChanged += new SafeEventHandler<EditValueChangedEventArgs>(CheckControl_EditValueChanged).Handler;
		}

		private void CheckControl_EditValueChanged(object sender, EditValueChangedEventArgs e)
		{
			Control.Text = "";
			if (_checkControl.IsChecked.Value)
			{
				Control.IsReadOnly = false;
				Control.IsRequired = _isControlRequired;
			}
			else
			{
				Control.IsReadOnly = true;
				Control.IsRequired = false;
				Control.DoValidate();
			}
		}
	}

	private class ChangeDemandBooleanControl
	{
		public CheckEditExt Control;

		private readonly CheckEditExt _checkControl;

		public ChangeDemandBooleanControl(DialogWindow dialog, string controlName, string controlCaption, bool storeValue = false, LayoutGroup changeLayout = null, bool storeWithoutValidation = false)
		{
			LayoutGroupExt layoutGroupExt = dialog.AddHorizontalLayoutGroup(changeLayout);
			string dataItemName = controlName;
			controlName = controlName.Replace('.', '_');
			_checkControl = dialog.AddBooleanControl(layoutGroupExt, controlName + "_CHB", controlCaption, storeValue);
			Control = dialog.AddBooleanControl(layoutGroupExt, controlName, "", storeValue);
			dialog.Controls[Control.Name].DataItemName = dataItemName;
			dialog.Controls[Control.Name].SpecData = storeWithoutValidation.ToString();
			CommonEventHandlers.AddIsReadonlyBinding(Control, (LayoutItemWithRequired)layoutGroupExt.Children[0]);
			if (!_checkControl.IsChecked.Value)
			{
				Control.IsChecked = false;
				Control.IsReadOnly = true;
				Control.DoValidate();
			}
			_checkControl.EditValueChanged += new SafeEventHandler<EditValueChangedEventArgs>(CheckControl_EditValueChanged).Handler;
		}

		private void CheckControl_EditValueChanged(object sender, EditValueChangedEventArgs e)
		{
			Control.IsChecked = false;
			if (_checkControl.IsChecked.Value)
			{
				Control.IsReadOnly = false;
				return;
			}
			Control.IsReadOnly = true;
			Control.DoValidate();
		}
	}

	private class ChangeDemandDateEditControl
	{
		private readonly bool _isControlRequired;

		public DateEditControl Control;

		private readonly CheckEditExt _checkControl;

		public ChangeDemandDateEditControl(DialogWindow dialog, string controlName, string controlCaption, bool useLongDate = false, bool isRequired = false, bool storeValue = false, LayoutGroup changeLayout = null, bool storeWithoutValidation = false)
		{
			_isControlRequired = isRequired;
			LayoutGroupExt layoutGroupExt = dialog.AddHorizontalLayoutGroup(changeLayout);
			string dataItemName = controlName;
			controlName = controlName.Replace('.', '_');
			_checkControl = dialog.AddBooleanControl(layoutGroupExt, controlName + "_CHB", controlCaption, storeValue);
			Control = dialog.AddDateControl(layoutGroupExt, controlName, "", useLongDate, _isControlRequired, storeValue);
			dialog.Controls[Control.Name].DataItemName = dataItemName;
			dialog.Controls[Control.Name].SpecData = storeWithoutValidation.ToString();
			CommonEventHandlers.AddIsReadonlyBinding(Control, (LayoutItemWithRequired)layoutGroupExt.Children[0]);
			if (!_checkControl.IsChecked.Value)
			{
				Control.EditValue = null;
				Control.IsReadOnly = true;
				Control.IsRequired = false;
				Control.DoValidate();
			}
			_checkControl.EditValueChanged += new SafeEventHandler<EditValueChangedEventArgs>(CheckControl_EditValueChanged).Handler;
		}

		private void CheckControl_EditValueChanged(object sender, EditValueChangedEventArgs e)
		{
			Control.EditValue = null;
			if (_checkControl.IsChecked.Value)
			{
				Control.IsReadOnly = false;
				Control.IsRequired = _isControlRequired;
				if (_isControlRequired && !Control.EditValue.HasValue)
				{
					Control.EditValue = Settings.HostingEnvironment.CurrentDate;
				}
			}
			else
			{
				Control.IsReadOnly = true;
				Control.IsRequired = false;
				Control.DoValidate();
			}
		}
	}

	private class ChangeDemandNumericControl
	{
		private readonly bool _isControlRequired;

		public PopupCalcEditExt Control;

		private readonly CheckEditExt _checkControl;

		public ChangeDemandNumericControl(DialogWindow dialog, string controlName, string controlCaption, short length, short precision = 0, bool isRequired = false, bool allowNegativeNumbers = false, bool useCurrencyFormat = true, bool storeValue = false, LayoutGroup changeLayout = null, bool storeWithoutValidation = false)
		{
			_isControlRequired = isRequired;
			LayoutGroupExt layoutGroupExt = dialog.AddHorizontalLayoutGroup(changeLayout);
			string dataItemName = controlName;
			controlName = controlName.Replace('.', '_');
			_checkControl = dialog.AddBooleanControl(layoutGroupExt, controlName + "_CHB", controlCaption, storeValue);
			Control = dialog.AddNumericControl(layoutGroupExt, controlName, "", length, precision, _isControlRequired, allowNegativeNumbers, useCurrencyFormat, storeValue);
			dialog.Controls[Control.Name].DataItemName = dataItemName;
			dialog.Controls[Control.Name].SpecData = storeWithoutValidation.ToString();
			CommonEventHandlers.AddIsReadonlyBinding(Control, (LayoutItemWithRequired)layoutGroupExt.Children[0]);
			if (!_checkControl.IsChecked.Value)
			{
				Control.EditValue = 0m;
				Control.IsReadOnly = true;
				Control.IsRequired = false;
				Control.DoValidate();
			}
			_checkControl.EditValueChanged += new SafeEventHandler<EditValueChangedEventArgs>(CheckControl_EditValueChanged).Handler;
		}

		private void CheckControl_EditValueChanged(object sender, EditValueChangedEventArgs e)
		{
			Control.EditValue = 0m;
			if (_checkControl.IsChecked.Value)
			{
				Control.IsReadOnly = false;
				Control.IsRequired = _isControlRequired;
			}
			else
			{
				Control.IsReadOnly = true;
				Control.IsRequired = false;
				Control.DoValidate();
			}
		}
	}

	private class ChangeDemandTimeControl
	{
		private readonly bool _isControlRequired;

		public TimeEditControl Control;

		public CheckEditExt _checkControl;

		public ChangeDemandTimeControl(DialogWindow dialog, string controlName, string controlCaption, bool showSeconds = false, bool isRequired = false, bool storeValue = false, LayoutGroup changeLayout = null, bool storeWithoutValidation = false)
		{
			_isControlRequired = isRequired;
			LayoutGroupExt layoutGroupExt = dialog.AddHorizontalLayoutGroup(changeLayout);
			string dataItemName = controlName;
			controlName = controlName.Replace('.', '_');
			_checkControl = dialog.AddBooleanControl(layoutGroupExt, controlName + "_CHB", controlCaption, storeValue);
			Control = dialog.AddTimeControl(layoutGroupExt, controlName, "", showSeconds, isRequired, storeValue);
			dialog.Controls[Control.Name].DataItemName = dataItemName;
			dialog.Controls[Control.Name].SpecData = storeWithoutValidation.ToString();
			CommonEventHandlers.AddIsReadonlyBinding(Control, (LayoutItemWithRequired)layoutGroupExt.Children[0]);
			if (!_checkControl.IsChecked.Value)
			{
				Control.EditValue = "00:00" + (showSeconds ? ":00" : "");
				Control.IsReadOnly = true;
				Control.IsRequired = false;
				Control.DoValidate();
			}
			_checkControl.EditValueChanged += new SafeEventHandler<EditValueChangedEventArgs>(CheckControl_EditValueChanged).Handler;
		}

		private void CheckControl_EditValueChanged(object sender, EditValueChangedEventArgs e)
		{
			Control.EditValue = "00:00" + (Control.ShowSeconds ? ":00" : "");
			if (_checkControl.IsChecked.Value)
			{
				Control.IsReadOnly = false;
				Control.IsRequired = _isControlRequired;
			}
			else
			{
				Control.IsReadOnly = true;
				Control.IsRequired = false;
				Control.DoValidate();
			}
		}
	}

	private class ChangeDemandDateTimeEditControl
	{
		private readonly bool _isControlRequired;

		public DateEditControl DateControl;

		public TimeEditControl TimeControl;

		private readonly CheckEditExt _checkControl;

		public ChangeDemandDateTimeEditControl(DialogWindow dialog, string controlName, string controlCaption, bool useLongDate = false, bool showSeconds = false, bool isRequired = false, bool storeValue = false, LayoutGroup changeLayout = null, bool storeWithoutValidation = false)
		{
			_isControlRequired = isRequired;
			LayoutGroupExt layoutGroupExt = dialog.AddHorizontalLayoutGroup(changeLayout);
			string dataItemName = controlName;
			controlName = controlName.Replace('.', '_');
			_checkControl = dialog.AddBooleanControl(layoutGroupExt, controlName + "_CHB", controlCaption, storeValue);
			DateControl = dialog.AddDateControl(layoutGroupExt, controlName, "", useLongDate, _isControlRequired, storeValue);
			dialog.Controls[DateControl.Name].DataItemName = dataItemName;
			dialog.Controls[DateControl.Name].SpecData = "DateTimeEditControl";
			TimeControl = dialog.AddTimeControl(layoutGroupExt, controlName + "_TIMEOFDATE", "", showSeconds, _isControlRequired, storeValue);
			dialog.Controls[TimeControl.Name].DataItemName = dataItemName;
			dialog.Controls[TimeControl.Name].SpecData = storeWithoutValidation.ToString();
			CommonEventHandlers.AddIsReadonlyBinding(DateControl, (LayoutItemWithRequired)layoutGroupExt.Children[0]);
			if (!_checkControl.IsChecked.Value)
			{
				DateControl.EditValue = null;
				DateControl.IsReadOnly = true;
				DateControl.IsRequired = false;
				DateControl.DoValidate();
				TimeControl.EditValue = "";
				TimeControl.IsReadOnly = true;
				TimeControl.IsRequired = false;
				TimeControl.DoValidate();
			}
			_checkControl.EditValueChanged += new SafeEventHandler<EditValueChangedEventArgs>(CheckControl_EditValueChanged).Handler;
		}

		private void CheckControl_EditValueChanged(object sender, EditValueChangedEventArgs e)
		{
			DateControl.EditValue = null;
			TimeControl.EditValue = "";
			if (_checkControl.IsChecked.Value)
			{
				DateControl.IsReadOnly = false;
				DateControl.IsRequired = _isControlRequired;
				TimeControl.IsReadOnly = false;
				TimeControl.IsRequired = _isControlRequired;
				if (_isControlRequired)
				{
					DateControl.EditValue = Settings.HostingEnvironment.CurrentDate;
				}
				if (_isControlRequired)
				{
					TimeControl.EditValue = (TimeControl.ShowSeconds ? DateTime.Now.ToString("HH:mm:ss") : DateTime.Now.ToString("HH:mm"));
				}
			}
			else
			{
				DateControl.IsReadOnly = true;
				DateControl.IsRequired = false;
				DateControl.DoValidate();
				TimeControl.IsReadOnly = true;
				TimeControl.IsRequired = false;
				TimeControl.DoValidate();
			}
		}
	}

	private class ChangeDemandComboBoxControl
	{
		private readonly bool _isControlRequired;

		public ComboBoxEditExt Control;

		private readonly CheckEditExt _checkControl;

		public ChangeDemandComboBoxControl(DialogWindow dialog, string controlName, string controlCaption, short length, IEnumerable<string> itemsSource, bool isRequired = false, bool storeValue = false, HorizontalAlignment horizontalAlignment = HorizontalAlignment.Left, LayoutGroup changeLayout = null, bool storeWithoutValidation = false)
		{
			_isControlRequired = isRequired;
			LayoutGroupExt layoutGroupExt = dialog.AddHorizontalLayoutGroup(changeLayout);
			string dataItemName = controlName;
			controlName = controlName.Replace('.', '_');
			_checkControl = dialog.AddBooleanControl(layoutGroupExt, controlName + "_CHB", controlCaption, storeValue);
			Control = dialog.AddComboBoxControl(layoutGroupExt, controlName, "", length, itemsSource, _isControlRequired, storeValue, horizontalAlignment);
			dialog.Controls[Control.Name].DataItemName = dataItemName;
			dialog.Controls[Control.Name].SpecData = storeWithoutValidation.ToString();
			CommonEventHandlers.AddIsReadonlyBinding(Control, (LayoutItemWithRequired)layoutGroupExt.Children[0]);
			if (!_checkControl.IsChecked.Value)
			{
				Control.Text = "";
				Control.IsReadOnly = true;
				Control.IsRequired = false;
				Control.DoValidate();
			}
			_checkControl.EditValueChanged += new SafeEventHandler<EditValueChangedEventArgs>(CheckControl_EditValueChanged).Handler;
		}

		private void CheckControl_EditValueChanged(object sender, EditValueChangedEventArgs e)
		{
			Control.Text = "";
			if (_checkControl.IsChecked.Value)
			{
				Control.IsReadOnly = false;
				Control.IsRequired = _isControlRequired;
			}
			else
			{
				Control.IsReadOnly = true;
				Control.IsRequired = false;
				Control.DoValidate();
			}
		}
	}

	private class ChangeDemandMemoControl
	{
		private readonly bool _isControlRequired;

		public MemoEditControl Control;

		private readonly CheckEditExt _checkControl;

		public ChangeDemandMemoControl(DialogWindow dialog, string controlName, string controlCaption, short lenght, bool isRequired = false, bool storeValue = false, bool isLeftAlign = false, LayoutGroup changeLayout = null, bool storeWithoutValidation = false)
		{
			_isControlRequired = isRequired;
			string dataItemName = controlName;
			controlName = controlName.Replace('.', '_');
			LayoutGroupExt layoutGroupExt = dialog.AddHorizontalLayoutGroup(changeLayout);
			_checkControl = dialog.AddBooleanControl(layoutGroupExt, controlName + "_CHB", controlCaption, storeValue);
			Control = dialog.AddMemoControl(layoutGroupExt, controlName, "", lenght, _isControlRequired, storeValue);
			Control.Height = 150.0;
			Control.Width = 450.0;
			Control.VerticalScrollBarVisibility = ScrollBarVisibility.Visible;
			Control.TextWrapping = TextWrapping.Wrap;
			Control.VerticalContentAlignment = VerticalAlignment.Top;
			Control.HorizontalAlignment = HorizontalAlignment.Stretch;
			dialog.Controls[Control.Name].DataItemName = dataItemName;
			dialog.Controls[Control.Name].SpecData = storeWithoutValidation.ToString();
			if (isLeftAlign)
			{
				Control.EditValueChanged += CommonEventHandlers.TrimEditValueChanged;
			}
			CommonEventHandlers.AddIsReadonlyBinding(Control, (LayoutItemWithRequired)layoutGroupExt.Children[0]);
			if (!_checkControl.IsChecked.Value)
			{
				Control.Text = "";
				Control.IsReadOnly = true;
				Control.IsRequired = false;
				Control.DoValidate();
			}
			_checkControl.EditValueChanged += new SafeEventHandler<EditValueChangedEventArgs>(CheckControl_EditValueChanged).Handler;
		}

		private void CheckControl_EditValueChanged(object sender, EditValueChangedEventArgs e)
		{
			Control.Text = "";
			if (_checkControl.IsChecked.Value)
			{
				Control.IsReadOnly = false;
				Control.IsRequired = _isControlRequired;
			}
			else
			{
				Control.IsReadOnly = true;
				Control.IsRequired = false;
				Control.DoValidate();
			}
		}
	}

	public const string CheckBoxNamePostfix = "_CHB";

	public const string TimeNamePostfix = "_TIMEOFDATE";

	public const string DateTimeEditControlSpec = "DateTimeEditControl";

	public static bool IsFieldChanged(DialogWindow dialog, string controlName)
	{
		controlName = controlName.Replace('.', '_').ToUpper();
		return (bool)dialog[controlName + "_CHB"];
	}

	public static void AddChangeDemandDropDownControl(DialogWindow dialog, string controlName, string controlCaption, IDropDownView idv, bool isRequired = false, bool allowAdditionButtons = false, bool storeValue = false, bool isShowComment = true, int codeLength = -1, LayoutGroup changeLayout = null, bool isCheckValue = true, string captionForStoreValue = "", bool storeWithoutValidation = false)
	{
		new ChangeDemandDropDownControl(dialog, controlName, controlCaption, idv, isRequired, allowAdditionButtons, storeValue, isShowComment, isCheckValue, codeLength, changeLayout, captionForStoreValue, storeWithoutValidation);
	}

	public static void AddChangeDemandMultiSelectDropDownControl(DialogWindow dialog, string controlName, string controlCaption, IDropDownView idv, bool isRequired = false, bool storeValue = false, LayoutGroup changeLayout = null, bool storeWithoutValidation = false)
	{
		new ChangeDemandMultiSelectDropDownControl(dialog, controlName, controlCaption, idv, isRequired, storeValue, changeLayout, storeWithoutValidation);
	}

	public static void AddChangeDemandStringControl(DialogWindow dialog, string controlName, string controlCaption, short length, bool isRequired = false, bool storeValue = false, bool isLeftAlign = false, LayoutGroup changeLayout = null, bool storeWithoutValidation = false)
	{
		new ChangeDemandStringControl(dialog, controlName, controlCaption, length, isRequired, storeValue, changeLayout, storeWithoutValidation);
	}

	public static void AddChangeDemandDropDownCommentControl(DialogWindow dialog, string controlName, string controlCaption, IDropDownView idv, bool isRequired = false, bool storeValue = false, short length = 50, HorizontalAlignment horizontalAlignment = HorizontalAlignment.Left, LayoutGroup changeLayout = null, string captionForStoreValue = "", bool storeWithoutValidation = false)
	{
		new ChangeDemandDropDownCommentControl(dialog, controlName, controlCaption, idv, isRequired, storeValue, length, horizontalAlignment, changeLayout, captionForStoreValue, storeWithoutValidation);
	}

	public static void AddChangeDemandBooleanControl(DialogWindow dialog, string controlName, string controlCaption, bool storeValue = false, LayoutGroup changeLayout = null, bool storeWithoutValidation = false)
	{
		new ChangeDemandBooleanControl(dialog, controlName, controlCaption, storeValue, changeLayout, storeWithoutValidation);
	}

	public static void AddChangeDemandDateEditControl(DialogWindow dialog, string controlName, string controlCaption, bool useLongDate = false, bool isRequired = false, bool storeValue = false, LayoutGroup changeLayout = null, bool storeWithoutValidation = false)
	{
		new ChangeDemandDateEditControl(dialog, controlName, controlCaption, useLongDate, isRequired, storeValue, changeLayout, storeWithoutValidation);
	}

	public static void AddChangeDemandNumericControl(DialogWindow dialog, string controlName, string controlCaption, short length, short precision = 0, bool isRequired = false, bool allowNegativeNumbers = false, bool useCurrencyFormat = true, bool storeValue = false, LayoutGroup changeLayout = null, bool storeWithoutValidation = false)
	{
		new ChangeDemandNumericControl(dialog, controlName, controlCaption, length, precision, isRequired, allowNegativeNumbers, useCurrencyFormat, storeValue, changeLayout, storeWithoutValidation);
	}

	public static void AddChangeDemandTimeControl(DialogWindow dialog, string controlName, string controlCaption, bool showSeconds = false, bool isRequired = false, bool storeValue = false, LayoutGroup changeLayout = null, bool storeWithoutValidation = false)
	{
		new ChangeDemandTimeControl(dialog, controlName, controlCaption, showSeconds, isRequired, storeValue, changeLayout, storeWithoutValidation);
	}

	public static void AddChangeDemandDateTimeEditControl(DialogWindow dialog, string controlName, string controlCaption, bool useLongDate = false, bool showSeconds = false, bool isRequired = false, bool storeValue = false, LayoutGroup changeLayout = null, bool storeWithoutValidation = false)
	{
		new ChangeDemandDateTimeEditControl(dialog, controlName, controlCaption, useLongDate, showSeconds, isRequired, storeValue, changeLayout, storeWithoutValidation);
	}

	public static void AddAddChangeDemandComboBoxControl(DialogWindow dialog, string controlName, string controlCaption, short length, IEnumerable<string> itemsSource, bool isRequired = false, bool storeValue = false, HorizontalAlignment horizontalAlignment = HorizontalAlignment.Left, LayoutGroup changeLayout = null, bool storeWithoutValidation = false)
	{
		new ChangeDemandComboBoxControl(dialog, controlName, controlCaption, length, itemsSource, isRequired, storeValue, horizontalAlignment, changeLayout, storeWithoutValidation);
	}

	public static void AddChangeDemandMemoControl(DialogWindow dialog, string controlName, string controlCaption, short length, bool isRequired = false, bool storeValue = false, bool isLeftAlign = false, LayoutGroup changeLayout = null, bool storeWithoutValidation = false)
	{
		new ChangeDemandMemoControl(dialog, controlName, controlCaption, length, isRequired, storeValue, isLeftAlign, changeLayout, storeWithoutValidation);
	}

	public static void AddLine(DialogWindow dialog)
	{
		dialog.AddLine();
	}
}
