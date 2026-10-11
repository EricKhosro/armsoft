using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace ArmSoft.InternetBank.Web.API.Common.Models;

public class CA : CABase
{
	[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
	[Obsolete("Use UIExtension instead.")]
	public string MobileExtension { get; set; }

	public string UIExtension { get; set; }

	public List<CAField> CustomFields { get; set; }
}
