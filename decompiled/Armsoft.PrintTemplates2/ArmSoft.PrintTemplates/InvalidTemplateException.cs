using System;

namespace ArmSoft.PrintTemplates;

public class InvalidTemplateException : Exception
{
	public InvalidTemplateException()
	{
	}

	public InvalidTemplateException(string message)
		: base(message)
	{
	}

	public InvalidTemplateException(string message, Exception inner)
		: base(message, inner)
	{
	}
}
