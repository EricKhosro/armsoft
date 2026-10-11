using System.Globalization;
using System.Windows;
using DevExpress.Xpf.Editors;
using DevExpress.Xpf.Editors.Validation.Native;

namespace ArmSoft.SME.Core.UI.Controls;

public class DateEditValidationEventArgs : ValidationEventArgs
{
	public DateEditValidationEventArgs(RoutedEvent routedEvent, object source, object value, CultureInfo culture, UpdateEditorSource updateSource)
		: base(routedEvent, source, value, culture, updateSource)
	{
	}
}
