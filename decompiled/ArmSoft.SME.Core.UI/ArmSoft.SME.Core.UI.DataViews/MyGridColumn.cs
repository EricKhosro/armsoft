using System;
using System.ComponentModel;
using System.Windows;
using DevExpress.Xpf.Grid;

namespace ArmSoft.SME.Core.UI.DataViews;

public class MyGridColumn : GridColumn, INotifyPropertyChanged
{
	private int _columnIndex;

	private bool _isBoolean;

	private bool _mySelected;

	private string _myShowTemplate = "";

	public static readonly DependencyProperty ColSortIndexProperty = DependencyProperty.Register("ColSortIndex", typeof(int), typeof(MyGridColumn));

	public bool IsColumnWidthSpecified { get; set; }

	public int ColumnVisibleIndex { get; set; }

	public bool IsDynamic { get; set; }

	public int ColumnIndex
	{
		get
		{
			return _columnIndex;
		}
		set
		{
			_columnIndex = value;
		}
	}

	public bool IsBooleanBased
	{
		get
		{
			return _isBoolean;
		}
		set
		{
			_isBoolean = value;
		}
	}

	public bool IsSelected
	{
		get
		{
			return _mySelected;
		}
		set
		{
			_mySelected = value;
			MyPropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsSelected"));
		}
	}

	public string ShowTemplate
	{
		get
		{
			return _myShowTemplate;
		}
		set
		{
			_myShowTemplate = value;
		}
	}

	public int ColSortIndex
	{
		get
		{
			return Convert.ToInt32(((DependencyObject)this).GetValue(ColSortIndexProperty));
		}
		set
		{
			((DependencyObject)this).SetValue(ColSortIndexProperty, (object)value);
			MyPropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ColSortIndex"));
		}
	}

	public event PropertyChangedEventHandler MyPropertyChanged;
}
