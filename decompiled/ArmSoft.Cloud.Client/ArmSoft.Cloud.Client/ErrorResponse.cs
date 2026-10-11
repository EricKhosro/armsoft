using System.ComponentModel;
using System.Runtime.InteropServices;
using Newtonsoft.Json;

namespace ArmSoft.Cloud.Client;

[ClassInterface(ClassInterfaceType.AutoDual)]
[ComVisible(true)]
[Guid("B870AF02-63E7-46D6-8F77-EA115B4E3074")]
public class ErrorResponse
{
	[DefaultValue("")]
	[JsonProperty("code", DefaultValueHandling = DefaultValueHandling.Ignore, Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
	public string Code { get; set; }

	public string Error { get; set; }

	[JsonProperty("stackTrace", DefaultValueHandling = DefaultValueHandling.Ignore, Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
	public string StackTrace { get; set; }
}
