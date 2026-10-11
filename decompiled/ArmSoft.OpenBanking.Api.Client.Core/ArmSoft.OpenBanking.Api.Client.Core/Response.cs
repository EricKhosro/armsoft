namespace ArmSoft.OpenBanking.Api.Client.Core;

public class Response<T>
{
	public string ResponseCode { get; set; }

	public string Message { get; set; }

	public T? Value { get; set; }
}
