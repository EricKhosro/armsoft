using System;

namespace ArmSoft.PossoftPayClient;

public class PossoftPayClientException : Exception
{
	public RequestResponseModel Response { get; private set; }

	public PossoftPayClientException(RequestResponseModel response)
		: base(response.responseCode + ": " + response.responseText)
	{
		Response = response;
	}

	public PossoftPayClientException(string message)
		: base(message)
	{
		Response = null;
	}
}
