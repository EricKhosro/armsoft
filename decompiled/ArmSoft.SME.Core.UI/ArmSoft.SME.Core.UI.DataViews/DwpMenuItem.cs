using System;
using DevExpress.Xpf.Bars;

namespace ArmSoft.SME.Core.UI.DataViews;

internal class DwpMenuItem
{
	private readonly ButtonClickFunction _funcRef;

	private readonly bool _storeOpContext;

	public BarButtonItem BtnItem;

	public bool IsRibbonItem;

	public bool ShowInContextMenu;

	public string WindowCaption { get; set; }

	public DwpMenuItem(BarButtonItem btn, bool isRibbon, ButtonClickFunction func, bool showInMenu, bool storeOpContext = false, bool isEnabled = true)
	{
		_funcRef = func;
		BtnItem = btn;
		BtnItem.IsEnabled = isEnabled;
		IsRibbonItem = isRibbon;
		ShowInContextMenu = showInMenu;
		_storeOpContext = storeOpContext;
	}

	private void InvokeWrapper()
	{
		try
		{
			_funcRef();
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	public ButtonClickFunction Invoker()
	{
		return InvokeWrapper;
	}
}
