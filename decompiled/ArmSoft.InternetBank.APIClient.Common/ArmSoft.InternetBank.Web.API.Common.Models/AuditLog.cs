using System;
using ArmSoft.OnlineBanking.Common.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace ArmSoft.InternetBank.Web.API.Common.Models;

public class AuditLog
{
	[JsonConverter(typeof(IsoDateTimeConverter))]
	public DateTime ActionDate { get; set; }

	public UserActionCategory ActionCategory { get; set; }

	public string DisplayActionCategory { get; set; }

	public UserActionType ActionType { get; set; }

	public string DisplayActionType { get; set; }

	public UserActionReason ActionReason { get; set; }

	public string DisplayActionReason { get; set; }

	public string HostAddress { get; set; }

	public string Location { get; set; }

	[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
	public string DisplayLocation { get; set; }

	public SystemType SystemType { get; set; }

	public string DisplaySystemType { get; set; }

	public string ClientVersion { get; set; }

	public string DeviceModel { get; set; }

	public string DevicePlatform { get; set; }
}
