namespace ArmSoft.SME.Common.Models.Exchange;

public class DocumentExchangeResponse : ExchangeResponse
{
	public int Isn { get; set; }

	public DocumentExchangeResponse()
	{
		Success = false;
		Isn = 0;
		Message = string.Empty;
	}

	public DocumentExchangeResponse(int isn)
	{
		Success = true;
		Isn = isn;
		Message = string.Empty;
	}

	public DocumentExchangeResponse(string message)
	{
		Success = false;
		Isn = 0;
		Message = message;
	}
}
