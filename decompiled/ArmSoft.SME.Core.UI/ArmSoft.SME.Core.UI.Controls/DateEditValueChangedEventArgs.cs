using System;
using DevExpress.Xpf.Editors;

namespace ArmSoft.SME.Core.UI.Controls;

public class DateEditValueChangedEventArgs : EditValueChangedEventArgs
{
	public DateTime? OldDateValue { get; }

	public DateTime? NewDateValue { get; }

	public DateEditValueChangedEventArgs(EditValueChangedEventArgs e)
		: base(e.OldValue, e.NewValue)
	{
		if (DateTime.TryParse((string)e.OldValue, out var result))
		{
			OldDateValue = result;
		}
		if (DateTime.TryParse((string)e.NewValue, out var result2))
		{
			NewDateValue = result2;
		}
	}
}
