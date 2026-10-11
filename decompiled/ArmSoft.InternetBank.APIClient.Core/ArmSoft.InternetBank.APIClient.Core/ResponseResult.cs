namespace ArmSoft.InternetBank.APIClient.Core;

public class ResponseResult<T>
{
	public T Value { get; set; }

	public string Message { get; set; }

	public string ResponseCode { get; set; }
}
