using System;
using System.Windows;
using System.Windows.Controls;
using ArmSoft.SME.Core.UI.DataViews;

namespace ArmSoft.SME.Core.UI.Controls;

public class DropDownControlAddEventArgs : EventArgs
{
	public Control MyControl { get; set; }

	public double Top { get; set; }

	public double Left { get; set; }

	public Window Owner { get; set; }

	public IDropDownView DataView { get; set; }

	public bool Handled { get; set; }

	public string Value { get; set; }

	public string Comment { get; set; }

	public DropDownControlAddEventArgs(Control myControl, IDropDownView dataView, Window owner, double top, double left)
	{
		MyControl = myControl;
		DataView = dataView;
		Owner = owner;
		Top = top;
		Left = left;
		Handled = false;
		Value = "";
		Comment = "";
	}
}
