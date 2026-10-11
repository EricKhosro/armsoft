using System.Collections.Generic;
using System.Runtime.InteropServices;
using Newtonsoft.Json;

namespace ArmSoft.InternetBank.Web.API.Common.Models;

[ComVisible(true)]
[Guid("3351ac37-d37d-46d4-9b4c-ba7dbac1ddf0")]
[ClassInterface(ClassInterfaceType.AutoDual)]
public class MP : AccountPayment
{
	public List<MPRow> Rows { get; set; }

	[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
	public string TaxCode { get; set; }

	[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
	public string SocialCard { get; set; }
}
