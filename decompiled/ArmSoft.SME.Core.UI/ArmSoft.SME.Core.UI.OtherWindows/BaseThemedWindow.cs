using System;
using System.Windows.Input;
using DevExpress.Xpf.Core;

namespace ArmSoft.SME.Core.UI.OtherWindows;

public class BaseThemedWindow : ThemedWindow
{
	public string HelpId { get; set; }

	public BaseThemedWindow()
	{
		ConstructUI();
	}

	private void ConstructUI()
	{
		AddEvents();
	}

	private void AddEvents()
	{
		PreviewKeyDown += new SafeEventHandler<KeyEventArgs>(BaseThemedWindow_PreviewKeyDown).Handler;
	}

	private void BaseThemedWindow_PreviewKeyDown(object sender, KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Invalid comparison between Unknown and I4
		try
		{
			if ((int)e.Key == 90)
			{
				e.Handled = true;
				Settings.HostingEnvironment.ShowHelp(HelpId);
			}
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}
}
