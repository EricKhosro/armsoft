using DevExpress.Xpf.Editors;

namespace ArmSoft.SME.Core.UI;

public class ImageEditValueChangedEventArgs : EditValueChangedEventArgs
{
	public int QualityLevel { get; }

	public ImageEditValueChangedEventArgs(EditValueChangedEventArgs editValue, int qualityLevel = 50)
		: base(editValue.OldValue, editValue.NewValue)
	{
		QualityLevel = qualityLevel;
	}
}
