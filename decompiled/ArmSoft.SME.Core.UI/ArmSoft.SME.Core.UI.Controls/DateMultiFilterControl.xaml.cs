using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using Ardalis.SmartEnum;
using ArmSoft.SME.Client.Data;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Enums;
using ArmSoft.SME.Common.Extensions;
using ArmSoft.SME.Common.SmartEnums;
using DevExpress.Xpf.Bars;

namespace ArmSoft.SME.Core.UI.Controls;

public partial class DateMultiFilterControl : UserControl, IComponentConnector
{
	public class DateMultiFilterValidationEventArgs(DateOnly? value, DateOnly? endValue, FilterTypesSE current)
	{
		public object ErrorContent { get; set; }

		public bool IsValid { get; set; } = true;

		public DateOnly? Value { get; private set; } = value;

		public DateOnly? EndValue { get; private set; } = endValue;

		public FilterTypesSE Current { get; } = current;
	}

	public delegate void DateMultiFilterValidateEventHandler(object sender, DateMultiFilterValidationEventArgs e);

	private class DateRangeToStringMultiValueConverter : IMultiValueConverter
	{
		public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
		{
			if (values.Length != 0 && values[0] is DateTime?)
			{
				DateTime? dateTime = (DateTime?)values[0];
				string text = (dateTime.HasValue ? dateTime.Value.FormatYYYYMMDD() : string.Empty);
				if (values.Length > 1 && values[1] is DateTime?)
				{
					DateTime? dateTime2 = (DateTime?)values[1];
					if (dateTime2.HasValue)
					{
						text = text + "," + dateTime2.Value.FormatYYYYMMDD();
					}
				}
				return text;
			}
			return string.Empty;
		}

		public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
		{
			string text = value as string;
			if (!string.IsNullOrEmpty(text))
			{
				string[] array = text.Split(',');
				if (array.Length != 0)
				{
					object obj = (string.IsNullOrWhiteSpace(array[0]) ? null : ((object)array[0].YYYYMMDDToDateOnly()));
					object obj2 = ((array.Length > 1 && !string.IsNullOrWhiteSpace(array[1])) ? ((object)array[1].YYYYMMDDToDateOnly()) : null);
					return new object[2] { obj, obj2 };
				}
			}
			return new object[2];
		}
	}

	private readonly PropertyChangeNotifier _isReadOnlyChangeNotifier;

	private readonly PropertyChangeNotifier _isRequiredChangeNotifier;

	private readonly MultiFilterTypes _supportedFilterTypes;

	private FilterTypesSE _currentFilter;

	private FilterTypesSE _defaultFilter;

	public static readonly DependencyProperty IsReadOnlyProperty = DependencyProperty.Register("IsReadOnly", typeof(bool), typeof(DateMultiFilterControl));

	public static readonly DependencyProperty IsRequiredProperty = DependencyProperty.Register("IsRequired", typeof(bool), typeof(DateMultiFilterControl));

	public static readonly DependencyProperty CodeProperty = DependencyProperty.Register("Code", typeof(string), typeof(DateMultiFilterControl));

	public static readonly DependencyProperty HasValidationErrorProperty = DependencyProperty.Register("HasValidationError", typeof(bool), typeof(DateMultiFilterControl));

	internal bool SkipValidationIfReadOnly
	{
		get
		{
			return DateEdit.SkipValidationIfReadOnly;
		}
		set
		{
			DateEdit.SkipValidationIfReadOnly = value;
			EndDateEdit.SkipValidationIfReadOnly = value;
		}
	}

	public bool HasValidationError
	{
		get
		{
			return (bool)((DependencyObject)this).GetValue(HasValidationErrorProperty);
		}
		internal set
		{
			((DependencyObject)this).SetValue(HasValidationErrorProperty, (object)value);
		}
	}

	private bool SupportsRange
	{
		get
		{
			if ((_supportedFilterTypes & MultiFilterTypes.Between) == 0)
			{
				return (_supportedFilterTypes & MultiFilterTypes.NotBetween) != 0;
			}
			return true;
		}
	}

	private bool CurrentFilterIsRange
	{
		get
		{
			if ((short)_currentFilter != 20)
			{
				return (short)_currentFilter == 21;
			}
			return true;
		}
	}

	public bool IsReadOnly
	{
		get
		{
			return (bool)((DependencyObject)this).GetValue(IsReadOnlyProperty);
		}
		set
		{
			DateEdit.IsReadOnly = value;
			EndDateEdit.IsReadOnly = value;
			SpecialDateButton.IsEnabled = !value;
			MultiFilter.IsEnabled = !value;
			((DependencyObject)this).SetValue(IsReadOnlyProperty, (object)value);
			ResetStyle();
		}
	}

	public bool IsRequired
	{
		get
		{
			return (bool)((DependencyObject)this).GetValue(IsRequiredProperty);
		}
		set
		{
			if (value && ((_supportedFilterTypes & MultiFilterTypes.IsBlank) != 0 || (_supportedFilterTypes & MultiFilterTypes.IsNotBlank) != 0))
			{
				throw new Exception(string.Format(ArmSoft.SME.Common.Resources.MultiFilterIsRequiredConflictError, Name));
			}
			DateEdit.IsRequired = value;
			EndDateEdit.IsRequired = value;
			((DependencyObject)this).SetValue(IsRequiredProperty, (object)value);
			ResetStyle();
		}
	}

	internal MultiFilterTypes FilterType
	{
		get
		{
			if ((short)_currentFilter != -1)
			{
				return _currentFilter.Type;
			}
			return _defaultFilter.Type;
		}
		set
		{
			foreach (BarCheckItem item in Filters.Items.Cast<BarCheckItem>())
			{
				if (SmartEnum<FilterTypesSE, short>.FromValue(short.Parse(item.Tag.ToString())).Type == value && item.IsVisible)
				{
					FilterClick(item);
					break;
				}
			}
		}
	}

	public DateOnly? EditValue
	{
		get
		{
			return DateEdit.EditValue;
		}
		set
		{
			DateEdit.EditValue = value;
		}
	}

	public DateOnly? EndEditValue
	{
		get
		{
			return EndDateEdit.EditValue;
		}
		set
		{
			EndDateEdit.EditValue = value;
		}
	}

	internal string Code
	{
		get
		{
			return Convert.ToString(((DependencyObject)this).GetValue(CodeProperty));
		}
		set
		{
			((DependencyObject)this).SetValue(CodeProperty, (object)value);
		}
	}

	public MultiFilterValue<DateOnly?> Value
	{
		get
		{
			return new MultiFilterValue<DateOnly?>(EditValue, _currentFilter.Type, EndEditValue);
		}
		set
		{
			EditValue = value.Value;
			EndEditValue = value.EndValue;
			FilterType = value.FilterType;
		}
	}

	public bool ShowSpecialDateButton { get; }

	public event DateMultiFilterValidateEventHandler Validate;

	public DateMultiFilterControl(MultiFilterTypes supportedFilterTypes, bool useLongDate = false, bool showSpecialDateButton = true, bool validateDateRep = false)
	{
		//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0114: Unknown result type (might be due to invalid IL or missing references)
		//IL_011a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0139: Unknown result type (might be due to invalid IL or missing references)
		//IL_013f: Unknown result type (might be due to invalid IL or missing references)
		//IL_015e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0164: Unknown result type (might be due to invalid IL or missing references)
		//IL_0183: Unknown result type (might be due to invalid IL or missing references)
		//IL_0189: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0219: Unknown result type (might be due to invalid IL or missing references)
		//IL_021f: Unknown result type (might be due to invalid IL or missing references)
		if ((supportedFilterTypes | MultiFilterTypes.Date) != MultiFilterTypes.Date)
		{
			throw new ASException(ArmSoft.SME.Common.Resources.UnsupportedFilterTypesMessage);
		}
		InitializeComponent();
		_supportedFilterTypes = supportedFilterTypes;
		_currentFilter = FilterTypesSE.IsUndefined;
		IsRequired = false;
		IsReadOnly = false;
		ElementRegistrator.GlobalSkipUniquenessCheck = true;
		ShowSpecialDateButton = showSpecialDateButton && SupportsRange;
		MinWidth = SizeCalculator.WidthForDate(useLongDate) * (double)((!SupportsRange) ? 1 : 2) + (double)(showSpecialDateButton ? 70 : 48);
		Container.Background = Settings.GetControlBackground();
		IsEqual.Glyph = ImageService.GetImage("equals.svg", isAppPathPreferred: false, "Filter/");
		NotEqual.Glyph = ImageService.GetImage("does_not_equal.svg", isAppPathPreferred: false, "Filter/");
		Greater.Glyph = ImageService.GetImage("greater.svg", isAppPathPreferred: false, "Filter/");
		GreaterOrEqual.Glyph = ImageService.GetImage("greater_or_equal.svg", isAppPathPreferred: false, "Filter/");
		Less.Glyph = ImageService.GetImage("less.svg", isAppPathPreferred: false, "Filter/");
		LessOrEqual.Glyph = ImageService.GetImage("less_or_equal.svg", isAppPathPreferred: false, "Filter/");
		Blank.Glyph = ImageService.GetImage("blank.svg", isAppPathPreferred: false, "Filter/");
		NotBlank.Glyph = ImageService.GetImage("not_blank.svg", isAppPathPreferred: false, "Filter/");
		Between.Glyph = ImageService.GetImage("between.svg", isAppPathPreferred: false, "Filter/");
		NotBetween.Glyph = ImageService.GetImage("not_between.svg", isAppPathPreferred: false, "Filter/");
		InitFilters();
		ErrorImage.Source = ImageService.GetImage("control_error.svg");
		MultiBinding multiBinding = new MultiBinding
		{
			Converter = new DateRangeToStringMultiValueConverter(),
			Mode = BindingMode.TwoWay
		};
		Binding item = new Binding("EditValue")
		{
			Source = DateEdit,
			Mode = BindingMode.TwoWay
		};
		multiBinding.Bindings.Add(item);
		item = new Binding("EditValue")
		{
			Source = EndDateEdit,
			Mode = BindingMode.TwoWay
		};
		multiBinding.Bindings.Add(item);
		BindingOperations.SetBinding((DependencyObject)(object)this, CodeProperty, multiBinding);
		DateEdit.UseLongDate = useLongDate;
		DateEdit.Foreground = Settings.GetForegroundBrush();
		DateEdit.Background = Brushes.Transparent;
		DateEdit.AfterValidate += DateEdit_AfterValidate;
		if (!SupportsRange || !CurrentFilterIsRange)
		{
			Container.Children.Remove(EndDateEdit);
		}
		EndDateEdit.UseLongDate = useLongDate;
		EndDateEdit.Foreground = Settings.GetForegroundBrush();
		EndDateEdit.Background = Brushes.Transparent;
		EndDateEdit.AfterValidate += DateEdit_AfterValidate;
		if (ShowSpecialDateButton)
		{
			SpecialDateButton.DateControl = DateEdit;
		}
		else
		{
			Container.Children.Remove(SpecialDateButton);
		}
		Validate += new SafeEventHandler<DateMultiFilterValidationEventArgs>(DateMultiFilterControl_Validate).Handler;
		PreviewKeyDown += new SafeEventHandler<KeyEventArgs>(DateMultiFilterControl_PreviewKeyDown).Handler;
		_isRequiredChangeNotifier = new PropertyChangeNotifier((DependencyObject)(object)this, IsRequiredProperty);
		_isRequiredChangeNotifier.ValueChanged += new SafeEventHandler<EventArgs>(IsRequiredReadonlyChanged).Handler;
		_isReadOnlyChangeNotifier = new PropertyChangeNotifier((DependencyObject)(object)this, IsReadOnlyProperty);
		_isReadOnlyChangeNotifier.ValueChanged += new SafeEventHandler<EventArgs>(IsRequiredReadonlyChanged).Handler;
		ResetStyle();
	}

	private void InitFilters()
	{
		IsEqual.Content = GetCaption(MultiFilterTypes.Equals);
		IsEqual.IsVisible = (_supportedFilterTypes & MultiFilterTypes.Equals) != 0;
		NotEqual.Content = GetCaption(MultiFilterTypes.DoesNotEqual);
		NotEqual.IsVisible = (_supportedFilterTypes & MultiFilterTypes.DoesNotEqual) != 0;
		Greater.Content = GetCaption(MultiFilterTypes.IsGreater);
		Greater.IsVisible = (_supportedFilterTypes & MultiFilterTypes.IsGreater) != 0;
		GreaterOrEqual.Content = GetCaption(MultiFilterTypes.IsGreaterOrEqual);
		GreaterOrEqual.IsVisible = (_supportedFilterTypes & MultiFilterTypes.IsGreaterOrEqual) != 0;
		Less.Content = GetCaption(MultiFilterTypes.IsLess);
		Less.IsVisible = (_supportedFilterTypes & MultiFilterTypes.IsLess) != 0;
		LessOrEqual.Content = GetCaption(MultiFilterTypes.IsLessOrEqual);
		LessOrEqual.IsVisible = (_supportedFilterTypes & MultiFilterTypes.IsLessOrEqual) != 0;
		Blank.Content = GetCaption(MultiFilterTypes.IsBlank);
		Blank.IsVisible = (_supportedFilterTypes & MultiFilterTypes.IsBlank) != 0;
		NotBlank.Content = GetCaption(MultiFilterTypes.IsNotBlank);
		NotBlank.IsVisible = (_supportedFilterTypes & MultiFilterTypes.IsNotBlank) != 0;
		Between.Content = GetCaption(MultiFilterTypes.Between);
		Between.IsVisible = (_supportedFilterTypes & MultiFilterTypes.Between) != 0;
		NotBetween.Content = GetCaption(MultiFilterTypes.NotBetween);
		NotBetween.IsVisible = (_supportedFilterTypes & MultiFilterTypes.NotBetween) != 0;
		BarCheckItem barCheckItem = Filters.Items.Cast<BarCheckItem>().First((BarCheckItem x) => x.IsVisible);
		_defaultFilter = SmartEnum<FilterTypesSE, short>.FromValue(short.Parse(barCheckItem.Tag.ToString()));
		_currentFilter = _defaultFilter;
		FilterImage.Source = barCheckItem.Glyph;
	}

	public static string GetCaption(MultiFilterTypes multiFilterType)
	{
		return multiFilterType switch
		{
			MultiFilterTypes.Equals => ArmSoft.SME.Common.Resources.IsEqual, 
			MultiFilterTypes.DoesNotEqual => ArmSoft.SME.Common.Resources.NotEqual, 
			MultiFilterTypes.IsGreater => ArmSoft.SME.Common.Resources.Greater, 
			MultiFilterTypes.IsGreaterOrEqual => ArmSoft.SME.Common.Resources.GreaterOrEqual, 
			MultiFilterTypes.IsLess => ArmSoft.SME.Common.Resources.Less, 
			MultiFilterTypes.IsLessOrEqual => ArmSoft.SME.Common.Resources.LessOrEqual, 
			MultiFilterTypes.IsBlank => ArmSoft.SME.Common.Resources.Blank, 
			MultiFilterTypes.IsNotBlank => ArmSoft.SME.Common.Resources.NotBlank, 
			MultiFilterTypes.Between => ArmSoft.SME.Common.Resources.Between, 
			MultiFilterTypes.NotBetween => ArmSoft.SME.Common.Resources.NotBetween, 
			_ => ArmSoft.SME.Common.Resources.Unknown, 
		};
	}

	private void BarCheckItem_ItemClick(object sender, ItemClickEventArgs e)
	{
		FilterClick(sender as BarCheckItem);
		if (DateEdit.IsEnabled && DateEdit.IsVisible)
		{
			DateEdit.Focus();
		}
	}

	private void Filters_Opening(object sender, CancelEventArgs e)
	{
		Filters.Items.Cast<BarCheckItem>().First((BarCheckItem x) => x.Tag.ToString() == _currentFilter.Value.ToString()).IsChecked = false;
	}

	private void FilterClick(BarCheckItem barCheckItem)
	{
		FilterImage.Source = barCheckItem.Glyph;
		if (SupportsRange && !CurrentFilterIsRange && !Container.Children.Contains(EndDateEdit))
		{
			Container.Children.Add(EndDateEdit);
			SpecialDateButton.DatePeriodEndControl = EndDateEdit;
		}
		if ((short)_currentFilter == 18 || (short)_currentFilter == 19)
		{
			DateEdit.IsEnabled = true;
			EndDateEdit.IsEnabled = true;
			ResetStyle();
		}
		_currentFilter = SmartEnum<FilterTypesSE, short>.FromValue(short.Parse(barCheckItem.Tag.ToString()));
		if (SupportsRange && !CurrentFilterIsRange && Container.Children.Contains(EndDateEdit))
		{
			EndDateEdit.EditValue = null;
			Container.Children.Remove(EndDateEdit);
			SpecialDateButton.DatePeriodEndControl = null;
		}
		if ((short)_currentFilter == 18 || (short)_currentFilter == 19)
		{
			DateEdit.EditValue = null;
			DateEdit.IsEnabled = false;
			EndDateEdit.EditValue = null;
			EndDateEdit.IsEnabled = false;
			ResetStyle();
		}
	}

	private void MultiFilter_Click(object sender, RoutedEventArgs e)
	{
		Filters.PlacementTarget = sender as Button;
		Filters.IsOpen = true;
	}

	private void DateEdit_AfterValidate(object sender, DateEditValidationEventArgs e)
	{
		if (sender == DateEdit)
		{
			CommonValidate((DateOnly?)e.Value, EndEditValue, e.IsValid, !EndDateEdit.HasValidationError);
		}
		else
		{
			CommonValidate(EditValue, (DateOnly?)e.Value, !DateEdit.HasValidationError, e.IsValid);
		}
	}

	private void CommonValidate(DateOnly? value, DateOnly? endValue, bool valueIsValid, bool endValueIsValid)
	{
		bool flag = valueIsValid && (!CurrentFilterIsRange | endValueIsValid);
		ErrorImage.Visibility = Visibility.Collapsed;
		if (flag && Validate != null)
		{
			DateMultiFilterValidationEventArgs e = new DateMultiFilterValidationEventArgs(value, endValue, _currentFilter);
			Validate(this, e);
			if (!e.IsValid)
			{
				flag = false;
				ErrorImage.Visibility = Visibility.Visible;
				ErrorImage.ToolTip = e.ErrorContent;
			}
		}
		HasValidationError = !flag;
	}

	public ImageSource GetCurrentFilterImage()
	{
		return FilterImage.Source;
	}

	public bool DoValidate()
	{
		HasValidationError = false;
		DateEdit.DoValidate();
		EndDateEdit.DoValidate();
		return !HasValidationError;
	}

	private void DateMultiFilterControl_Validate(object sender, DateMultiFilterValidationEventArgs e)
	{
		if (CurrentFilterIsRange && e.Value.HasValue && e.EndValue.HasValue && e.Value.Value > e.EndValue.Value)
		{
			e.IsValid = false;
			e.ErrorContent = ArmSoft.SME.Common.Resources.ValidateWrongPeriod;
		}
		else if (CurrentFilterIsRange)
		{
			if (e.EndValue.HasValue && !e.Value.HasValue)
			{
				e.IsValid = false;
				e.ErrorContent = ArmSoft.SME.Common.Resources.StartDateMissingMessage;
			}
			else if (e.Value.HasValue && !e.EndValue.HasValue)
			{
				e.IsValid = false;
				e.ErrorContent = ArmSoft.SME.Common.Resources.EndDateMissingMessage;
			}
		}
	}

	private void DateMultiFilterControl_PreviewKeyDown(object sender, KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Invalid comparison between Unknown and I4
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Invalid comparison between Unknown and I4
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Invalid comparison between Unknown and I4
		if ((int)e.Key == 26 && (int)Keyboard.Modifiers == 6)
		{
			Filters.IsOpen = true;
			e.Handled = true;
		}
		else if ((int)e.Key == 13 && Filters.IsOpen)
		{
			Filters.IsOpen = false;
			e.Handled = true;
		}
	}

	private void IsRequiredReadonlyChanged(object sender, EventArgs e)
	{
		ResetStyle();
		SetTabStop(!IsReadOnly);
	}

	private void ResetStyle()
	{
		DateEditControl dateEdit = DateEdit;
		Brush background = (EndDateEdit.Background = Brushes.Transparent);
		dateEdit.Background = background;
		if (IsRequired && !IsReadOnly)
		{
			Container.Background = Settings.RequiredBrush;
		}
		else
		{
			Container.Background = Settings.GetControlBackground();
		}
		if (!IsReadOnly)
		{
			((PropertyDescriptor)(object)DependencyPropertyDescriptor.FromProperty(UIElement.OpacityProperty, typeof(Grid))).ResetValue((object)Container);
		}
	}

	private void SetTabStop(bool isTabStop)
	{
		IsTabStop = isTabStop;
		DateEdit.IsTabStop = isTabStop;
		EndDateEdit.IsTabStop = isTabStop;
		SpecialDateButton.IsTabStop = isTabStop;
	}

	private void CtrlBorder_GotFocus(object sender, RoutedEventArgs e)
	{
	}

	private void CtrlBorder_LostFocus(object sender, RoutedEventArgs e)
	{
	}

	[DebuggerNonUserCode]
	[GeneratedCode("PresentationBuildTasks", "10.0.9.0")]
	internal Delegate _CreateDelegate(Type delegateType, string handler)
	{
		return Delegate.CreateDelegate(delegateType, this, handler);
	}
}
