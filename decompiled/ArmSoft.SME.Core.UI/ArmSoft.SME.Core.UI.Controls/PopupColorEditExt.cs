using System.Windows.Input;
using DevExpress.Xpf.Editors;

namespace ArmSoft.SME.Core.UI.Controls;

public class PopupColorEditExt : PopupColorEdit
{
	public PopupColorEditExt()
	{
		KeyDown += new SafeEventHandler<KeyEventArgs>(PopupColorEditExt_KeyDown).Handler;
	}

	private void PopupColorEditExt_KeyDown(object sender, KeyEventArgs e)
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
