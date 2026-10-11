namespace ArmSoft.SME.Common.Models.Exchange;

public class ExchangeResponse
{
	public bool Success { get; set; }

	public string Message { get; set; }

	public ExchangeResponse()
	{
		Success = true;
		Message = string.Empty;
	}

	public ExchangeResponse(string message)
	{
		Success = false;
		Message = message;
	}
}
