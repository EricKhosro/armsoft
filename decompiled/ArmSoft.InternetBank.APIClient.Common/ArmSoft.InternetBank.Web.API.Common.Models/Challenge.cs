using System.Runtime.InteropServices;
using Newtonsoft.Json;

namespace ArmSoft.InternetBank.Web.API.Common.Models;

[ComVisible(true)]
[Guid("F5830566-7578-4017-B026-89C9C1C8F40A")]
[ClassInterface(ClassInterfaceType.AutoDual)]
public class Challenge
{
	[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
	public string Description { get; set; }

	[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
	public string Challenge { get; set; }

	[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
	public string SecurityProvider { get; set; }

	[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
	public string TokenSerial { get; set; }

	[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
	public string SignatureMode { get; set; }

	public int SecurityProviderCodeLength { get; set; }
}
