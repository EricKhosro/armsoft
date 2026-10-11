namespace ArmSoft.OpenBanking.Api.Client.Core;

public class ActionNotSupportedException : NotFoundException
{
	public ActionNotSupportedException(string? content)
		: base(content)
	{
	}

	public ActionNotSupportedException(string responseCode, string? content, string message)
		: base(responseCode, content, message)
	{
	}
}
