using System;

namespace ArmSoft.SME.Core.UI;

public class SafeEventHandler<TEventArgs>
{
	private readonly EventHandler<TEventArgs> _innerHandler;

	public SafeEventHandler(EventHandler<TEventArgs> eventHandler)
	{
		_innerHandler = eventHandler;
	}

	public void Handler(object sender, TEventArgs e)
	{
		try
		{
			_innerHandler(sender, e);
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}
}
