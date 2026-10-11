using System;

namespace ArmSoft.SME.Core.UI.Controls;

public class EditKeyValueChangedEventArgs : EventArgs
{
	public object KeyValue { get; set; }

	public EditKeyValueChangedEventArgs(object keyValue)
	{
		KeyValue = keyValue;
	}
}
