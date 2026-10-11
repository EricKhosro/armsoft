using System.Collections.Generic;
using System.Runtime.InteropServices;
using Newtonsoft.Json;

namespace ArmSoft.InternetBank.Web.API.Common.Models;

[ComVisible(true)]
[Guid("1F1B9890-9EFC-4791-8795-1F6836C9A6B4")]
[ClassInterface(ClassInterfaceType.AutoDual)]
public class Error
{
	public ErrorType Type { get; set; }

	public string Message { get; set; }

	[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
	public string ErrorCode { get; set; }

	[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
	public List<FieldError> FieldErrors { get; set; }
}
