using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ArmSoft.SME.Core.UI.Controls;

public class RadioButtonExt : RadioButton
{
	public Visibility ControlVisibility
	{
		get
		{
			return ((Control)(object)Parent).Visibility;
		}
		set
		{
			((Control)(object)Parent).Visibility = value;
		}
	}

	public RadioButtonExt()
	{
		KeyDown += new SafeEventHandler<KeyEventArgs>(RadioButton_KeyDown).Handler;
	}

	private void RadioButton_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Invalid comparison between Unknown and I4
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Expected Obj, but got Unknown
		if ((int)e.Key == 6)
		{
			MoveFocus(new TraversalRequest((FocusNavigationDirection)0));
		}
	}
}
