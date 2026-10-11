using DevExpress.Xpf.Editors;

namespace ArmSoft.SME.Core.UI.Controls;

public class RateEditValueChangedEventArgs : EditValueChangedEventArgs
{
	public bool IsRateChanged { get; set; }

	public RateEditValueChangedEventArgs(object oldValue, object newValue)
		: base(oldValue, newValue)
	{
	}

	public RateEditValueChangedEventArgs(EditValueChangedEventArgs editArgs)
		: base(editArgs.OldValue, editArgs.NewValue)
	{
	}
}
