using System;
using System.Windows;
using System.Windows.Data;

namespace ArmSoft.SME.Core.UI.Controls;

public sealed class PropertyChangeNotifier : DependencyObject, IDisposable
{
	public delegate void ValueChangedEventHandler(object sender, EventArgs e);

	private WeakReference _propertySource;

	public static readonly DependencyProperty ValueProperty = DependencyProperty.Register("Value", typeof(object), typeof(PropertyChangeNotifier), (PropertyMetadata)(object)new FrameworkPropertyMetadata((object)null, (PropertyChangedCallback)MyOnPropertyChanged));

	public DependencyObject PropertySource
	{
		get
		{
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Expected Obj, but got Unknown
			try
			{
				if (_propertySource.IsAlive)
				{
					return (DependencyObject)_propertySource.Target;
				}
				return null;
			}
			catch (Exception)
			{
				return null;
			}
		}
	}

	public object Value
	{
		get
		{
			return ((DependencyObject)this).GetValue(ValueProperty);
		}
		set
		{
			((DependencyObject)this).SetValue(ValueProperty, value);
		}
	}

	public event ValueChangedEventHandler ValueChanged;

	public PropertyChangeNotifier(DependencyObject propertySource, string path)
	{
		Init(propertySource, new PropertyPath(path));
	}

	public PropertyChangeNotifier(DependencyObject propertySource, DependencyProperty property)
	{
		Init(propertySource, new PropertyPath(property));
	}

	public PropertyChangeNotifier(DependencyObject propertySource, PropertyPath property)
	{
		Init(propertySource, property);
	}

	private void Init(DependencyObject propertySource, PropertyPath property)
	{
		ArgumentNullException.ThrowIfNull(propertySource, "propertySource");
		ArgumentNullException.ThrowIfNull(property, "property");
		_propertySource = new WeakReference(propertySource);
		Binding binding = new Binding
		{
			Path = property,
			Mode = BindingMode.OneWay,
			Source = propertySource
		};
		BindingOperations.SetBinding((DependencyObject)(object)this, ValueProperty, binding);
	}

	private static void MyOnPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
	{
		((PropertyChangeNotifier)(object)d).RaiseValueChanged();
	}

	private void RaiseValueChanged()
	{
		ValueChanged?.Invoke(this, EventArgs.Empty);
	}

	public void Dispose()
	{
		BindingOperations.ClearBinding((DependencyObject)(object)this, ValueProperty);
	}

	static PropertyChangeNotifier()
	{
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Expected Obj, but got Unknown
	}
}
