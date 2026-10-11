using System;
using System.Collections;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using ArmSoft.SME.Core.UI.Controls;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.LayoutControl;

namespace ArmSoft.SME.Core.UI;

public class TabValidationHelper
{
	private class ValidationErrorsChecker
	{
		private bool _validationErrorFound;

		public DXTabItem TabItem { get; set; }

		public bool CheckValidationErrors()
		{
			_validationErrorFound = false;
			TabElementHandler((LayoutGroup)TabItem.Content);
			return _validationErrorFound;
		}

		public void TabElementHandler(FrameworkElement el)
		{
			if (_validationErrorFound)
			{
				return;
			}
			if (el is CheckEditExt checkEditExt)
			{
				_validationErrorFound = checkEditExt.HasValidationError;
			}
			else if (el is DropDownControl dropDownControl)
			{
				_validationErrorFound = dropDownControl.InnerCodeEdit.HasValidationError;
			}
			else if (el is MultiSelectDropDownControl multiSelectDropDownControl)
			{
				_validationErrorFound = multiSelectDropDownControl.InnerCodeEdit.HasValidationError;
			}
			else if (el is DropDownCommentControl dropDownCommentControl)
			{
				_validationErrorFound = dropDownCommentControl.InnerCodeEdit.HasValidationError;
			}
			else if (el is ComboBoxEditExt comboBoxEditExt)
			{
				_validationErrorFound = comboBoxEditExt.HasValidationError;
			}
			else if (el is TextEditExt textEditExt)
			{
				_validationErrorFound = textEditExt.HasValidationError;
			}
			else if (el is ButtonEditExt buttonEditExt)
			{
				_validationErrorFound = buttonEditExt.HasValidationError;
			}
			else if (el is PopupCalcEditExt popupCalcEditExt)
			{
				_validationErrorFound = popupCalcEditExt.HasValidationError;
			}
			else if (el is DateEditControl dateEditControl)
			{
				_validationErrorFound = dateEditControl.HasValidationError;
			}
			else if (el is ExchangeRateControl exchangeRateControl)
			{
				_validationErrorFound = exchangeRateControl.HasValidationError;
			}
			else if (el is DocumentGridControl documentGridControl)
			{
				_validationErrorFound = documentGridControl.HasValidationError;
			}
			else if (el is TimeEditControl timeEditControl)
			{
				_validationErrorFound = timeEditControl.HasValidationError;
			}
			else if (el is MonthYearControl monthYearControl)
			{
				_validationErrorFound = monthYearControl.HasValidationError;
			}
			else if (el is PasswordBoxEditExt passwordBoxEditExt)
			{
				_validationErrorFound = passwordBoxEditExt.HasValidationError;
			}
			else if (el is SpinEditExt spinEditExt)
			{
				_validationErrorFound = spinEditExt.HasValidationError;
			}
			else if (el is MemoEditControl memoEditControl)
			{
				_validationErrorFound = memoEditControl.HasValidationError;
			}
			if (_validationErrorFound)
			{
				return;
			}
			IEnumerable children = LogicalTreeHelper.GetChildren(el);
			if (children == null)
			{
				return;
			}
			foreach (object item in children)
			{
				if (item is FrameworkElement el2)
				{
					TabElementHandler(el2);
				}
			}
		}
	}

	public static readonly DependencyProperty TabHasValidationErrorProperty = DependencyProperty.RegisterAttached("TabHasValidationError", typeof(bool), typeof(TabValidationHelper), (PropertyMetadata)(object)new UIPropertyMetadata((object)false));

	public static readonly DependencyProperty EditorHasValidationErrorProperty = DependencyProperty.RegisterAttached("EditorHasValidationError", typeof(bool), typeof(TabValidationHelper), (PropertyMetadata)(object)new UIPropertyMetadata(false, EditorHasValidationErrorChanged));

	public static void EditorHasValidationErrorChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
	{
		DXTabItem parentTabItem = GetParentTabItem(sender);
		if (Convert.ToBoolean(e.NewValue))
		{
			SetTabHasValidationError((DependencyObject)(object)parentTabItem, value: true);
			return;
		}
		ValidationErrorsChecker validationErrorsChecker = new ValidationErrorsChecker
		{
			TabItem = parentTabItem
		};
		SetTabHasValidationError((DependencyObject)(object)parentTabItem, validationErrorsChecker.CheckValidationErrors());
	}

	public static void ValidateTab(DXTabItem tabItem)
	{
		ValidationErrorsChecker validationErrorsChecker = new ValidationErrorsChecker();
		validationErrorsChecker.TabItem = tabItem;
		validationErrorsChecker.CheckValidationErrors();
	}

	public static bool GetTabHasValidationError(DependencyObject obj)
	{
		return Convert.ToBoolean(obj.GetValue(TabHasValidationErrorProperty));
	}

	public static void SetTabHasValidationError(DependencyObject obj, bool value)
	{
		obj.SetValue(TabHasValidationErrorProperty, (object)value);
	}

	public static DXTabItem GetParentTabItem(DependencyObject el)
	{
		for (DependencyObject parent = LogicalTreeHelper.GetParent(el); parent != null; parent = LogicalTreeHelper.GetParent(parent))
		{
			if (parent is DXTabItem result)
			{
				return result;
			}
		}
		return null;
	}

	public static void RegisterControl(Control control)
	{
		if (GetParentTabItem((DependencyObject)(object)control) != null)
		{
			Binding binding = new Binding("HasValidationError");
			binding.Mode = BindingMode.OneWay;
			binding.RelativeSource = RelativeSource.Self;
			control.SetBinding(EditorHasValidationErrorProperty, binding);
		}
	}

	static TabValidationHelper()
	{
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Expected Obj, but got Unknown
	}
}
