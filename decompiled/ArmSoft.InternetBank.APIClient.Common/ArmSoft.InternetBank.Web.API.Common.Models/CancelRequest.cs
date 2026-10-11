using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Newtonsoft.Json;

namespace ArmSoft.InternetBank.Web.API.Common.Models;

[ComVisible(true)]
[Guid("3EA44AF0-6B74-48CB-88DB-AD5B4E073A01")]
[ClassInterface(ClassInterfaceType.AutoDual)]
public class CancelRequest : Document
{
	[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
	public Guid TargetId { get; set; }

	public string TargetAmount { get; set; }

	public string TargetDate { get; set; }

	public string TargetNum { get; set; }

	public string TargetCategory { get; set; }

	public string TargetType { get; set; }

	public string DisplayTargetType { get; set; }

	[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
	public List<Link> Links { get; set; }
}
