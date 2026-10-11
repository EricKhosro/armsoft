namespace ArmSoft.SME.Core.UI;

public class InvisibleException : ASException
{
	public InvisibleException()
	{
	}

	public InvisibleException(string message)
		: base(message)
	{
	}

	public InvisibleException(string message, string field)
		: base(message, field)
	{
	}
}
