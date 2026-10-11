using System;

namespace ArmSoft.SME.Core.UI.Controls;

public class ComboBoxEditExtEventArgs : EventArgs
{
	public ComboBoxEditExt MyControl { get; set; }

	public bool Cancel { get; set; }

	public ComboBoxEditExtEventArgs(ComboBoxEditExt myControl)
	{
		MyControl = myControl;
	}
}
