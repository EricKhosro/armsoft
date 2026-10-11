namespace ArmSoft.SME.Core.AddIn;

public class ErrorDetail
{
	public string Message { get; set; }

	public int Number { get; set; }

	public ErrorDetail(string message, int number = -1)
	{
		Message = message;
		Number = number;
	}
}
