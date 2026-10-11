using System.Windows.Controls;

namespace ArmSoft.SME.Core.UI;

public class DialogWindowValidationArgs
{
	public bool isValid = true;

	public string ErrorContent;

	public Control SetFocusWhenInvalidSave;
}
