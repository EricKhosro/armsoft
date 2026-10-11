using System;
using ArmSoft.SME.Core.UI.PersistableObject;

namespace ArmSoft.SME.Core.UI;

public class SafeDocumentEventHandler<TEventArgs>
{
	private readonly EventHandler<TEventArgs> _innerHandler;

	private readonly IPOWindow _poWindow;

	public bool HandleOnLoadingDocumentToControls { get; set; }

	public SafeDocumentEventHandler(EventHandler<TEventArgs> eventHandler, IPOWindow poWindow)
	{
		_innerHandler = eventHandler;
		_poWindow = poWindow;
		HandleOnLoadingDocumentToControls = false;
	}

	public void Handler(object sender, TEventArgs e)
	{
		try
		{
			if (HandleOnLoadingDocumentToControls || !_poWindow.IsLoadingDataToControls)
			{
				_innerHandler(sender, e);
			}
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}
}
