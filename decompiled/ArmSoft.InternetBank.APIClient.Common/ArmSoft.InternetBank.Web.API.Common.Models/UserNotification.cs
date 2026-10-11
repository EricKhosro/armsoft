using System;
using ArmSoft.OnlineBanking.Common.Enums;
using Newtonsoft.Json;

namespace ArmSoft.InternetBank.Web.API.Common.Models;

public class UserNotification
{
	public int NotificationId { get; set; }

	[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
	public string TargetId { get; set; }

	[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
	public PushNotificationType Type { get; set; }

	public DateTime NotificationDate { get; set; }

	public string Title { get; set; }

	[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
	public string Text { get; set; }

	[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
	public bool IsSeen { get; set; }

	[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
	public DateTime? SeenDate { get; set; }

	[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
	public bool IsRead { get; set; }

	[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
	public DateTime? ReadDate { get; set; }

	[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
	public DateTime? SentDate { get; set; }

	[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
	public bool IsClicked { get; set; }

	[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
	public DateTime? ClickedDate { get; set; }

	[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
	public string DocumentType { get; set; }

	[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
	public string DocumentUtilityCode { get; set; }

	[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
	public string IconUrl { get; set; }

	[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
	public UtilityType DocumentUtilityType { get; set; }

	[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
	public string ViolationId { get; set; }

	[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
	public string TaskId { get; set; }

	[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
	public int? VehicleId { get; set; }
}
