using System.Windows;
using ArmSoft.SME.Core.UI.SmartEnums;
using DevExpress.Xpf.LayoutControl;

namespace ArmSoft.SME.Core.UI.Controls;

public class LayoutItemWithRequired : LayoutItem
{
	public static readonly DependencyProperty IsArmSoftRequiredProperty = DependencyProperty.Register("IsArmSoftRequired", typeof(bool), typeof(LayoutItemWithRequired));

	public static readonly DependencyProperty IsReadOnlyProperty = DependencyProperty.Register("IsReadOnly", typeof(bool), typeof(LayoutItemWithRequired));

	public static readonly DependencyProperty IsStoreValueProperty = DependencyProperty.Register("IsStoreValue", typeof(bool), typeof(LayoutItemWithRequired));

	public bool IsReadOnly
	{
		get
		{
			return (bool)((DependencyObject)this).GetValue(IsReadOnlyProperty);
		}
		set
		{
			((DependencyObject)this).SetValue(IsReadOnlyProperty, (object)value);
		}
	}

	public bool IsArmSoftRequired
	{
		get
		{
			return (bool)((DependencyObject)this).GetValue(IsArmSoftRequiredProperty);
		}
		set
		{
			((DependencyObject)this).SetValue(IsArmSoftRequiredProperty, (object)value);
		}
	}

	public bool IsStoreValue
	{
		get
		{
			return (bool)((DependencyObject)this).GetValue(IsStoreValueProperty);
		}
		set
		{
			((DependencyObject)this).SetValue(IsStoreValueProperty, (object)value);
		}
	}

	public bool IsNew { get; set; }

	public FeatureStatusTypeSE ControlStatus
	{
		get
		{
			if (!IsNew)
			{
				return FeatureStatusTypeSE.Regular;
			}
			return FeatureStatusTypeSE.New;
		}
	}

	public LayoutItemWithRequired()
		: this(isNew: false)
	{
	}

	public LayoutItemWithRequired(bool isNew)
	{
		IsRequired = false;
		IsReadOnly = false;
		IsNew = isNew;
		LabelTemplate = (DataTemplate)Settings.GenericResources["LayoutItemRequired"];
	}
}
