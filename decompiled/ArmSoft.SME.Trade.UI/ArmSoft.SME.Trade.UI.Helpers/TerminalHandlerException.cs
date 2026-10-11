using System;

namespace ArmSoft.SME.Trade.UI.Helpers;

public class TerminalHandlerException : Exception
{
	public int LineNumber;

	public TerminalHandlerException(int lineNumber, string message)
		: base(message)
	{
		LineNumber = lineNumber;
	}
}
