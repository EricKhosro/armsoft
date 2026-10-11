using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using DevExpress.Xpf.Editors;

namespace ArmSoft.SME.Core.UI.Controls;

public class CheckEditExt : CheckEdit
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

	public CheckEditExt()
	{
		KeyDown += new SafeEventHandler<KeyEventArgs>(CheckEditExt_KeyDown).Handler;
		if (ArmSoft.SME.Core.UI.Settings.IsWin11Theme)
		{
			CheckBoxWidth = 16.0;
			CheckBoxHeight = 16.0;
		}
	}

	private void CheckEditExt_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Invalid comparison between Unknown and I4
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Expected Obj, but got Unknown
		if ((int)e.Key == 6)
		{
			((UIElement)sender).MoveFocus(new TraversalRequest((FocusNavigationDirection)0));
		}
	}
}
