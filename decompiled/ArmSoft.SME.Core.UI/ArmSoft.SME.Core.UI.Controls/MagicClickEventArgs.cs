using System;
using System.Windows;

namespace ArmSoft.SME.Core.UI.Controls;

public class MagicClickEventArgs : EventArgs
{
	public double Top { get; set; }

	public double Left { get; set; }

	public Window Owner { get; set; }

	public MagicClickEventArgs(Window owner, double top, double left)
	{
		Owner = owner;
		Top = top;
		Left = left;
	}
}
