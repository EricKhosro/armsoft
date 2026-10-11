using System;
using System.Windows;
using DevExpress.Xpf.Editors;

namespace ArmSoft.SME.Core.UI.CustomWindowControls;

public class CWCTextEditControl : TextEdit
{
	protected override void OnLostFocus(RoutedEventArgs e)
	{
		try
		{
			base.OnLostFocus(e);
		}
		catch (Exception ex)
		{
			ArmSoft.SME.Core.UI.Settings.ErrorHandler(ex);
		}
	}
}
