using System.Collections.Generic;
using Newtonsoft.Json;

namespace ArmSoft.InternetBank.Web.API.Common.Models;

public class Application : Document
{
	public string IconUrl { get; set; }

	[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
	public List<Link> Links { get; set; }
}
