using System;

namespace ArmSoft.SME.Core.UI.Report;

internal class ReportMenuItem
{
	private readonly ContextMenuFunction _funcRef;

	public ReportMenuItem(ContextMenuFunction func)
	{
		_funcRef = func;
	}

	private void InvokeWrapper(object rowInfo)
	{
		try
		{
			_funcRef(rowInfo);
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	public ContextMenuFunction Invoker()
	{
		return InvokeWrapper;
	}
}
