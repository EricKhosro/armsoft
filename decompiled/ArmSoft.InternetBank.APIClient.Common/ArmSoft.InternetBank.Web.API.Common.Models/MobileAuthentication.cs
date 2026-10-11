using System;
using ArmSoft.OnlineBanking.Common.Enums;
using Newtonsoft.Json;

namespace ArmSoft.InternetBank.Web.API.Common.Models;

public class MobileAuthentication
{
	public Guid SessionId { get; set; }

	public string Uuid { get; set; }

	public string DevicePlatform { get; set; }

	public string DeviceModel { get; set; }

	[JsonConverter(typeof(DateTimeConverter))]
	public DateTime LoginDate { get; set; }

	public string Location { get; set; }

	public SystemType SystemType { get; set; }

	public DevicePushAuthenticationStatus PushStatus { get; set; }

	public bool IsDevicePlatformAndroid { get; set; }

	public bool IsDevicePlatformIos { get; set; }

	public DateTime? LastUsageDate { get; set; }
}
