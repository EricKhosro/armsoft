using System.Net;

namespace ArmSoft.EMarkReceiptClient.Exceptions;

public class EMarkErrorDetails
{
	public HttpStatusCode StatusCode { get; set; }

	public EMarkErrorCode ErrorCode { get; set; }

	public string Message { get; set; } = string.Empty;

	public string ResponseContent { get; set; } = string.Empty;
}
