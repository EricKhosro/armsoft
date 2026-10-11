using System;
using DevExpress.Xpf.Core;

namespace ArmSoft.SME.Core.UI.CustomWindowControls;

public class CWCSimpleButton : SimpleButton
{
	protected override void OnClick()
	{
		try
		{
			base.OnClick();
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}
}
