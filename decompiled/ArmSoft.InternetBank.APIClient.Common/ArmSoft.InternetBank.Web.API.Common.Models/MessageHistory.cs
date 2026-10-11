using System;
using ArmSoft.OnlineBanking.Common.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace ArmSoft.InternetBank.Web.API.Common.Models;

public class MessageHistory
{
	public Guid MessageId { get; set; }

	[JsonConverter(typeof(IsoDateTimeConverter))]
	public DateTime OperationDate { get; set; }

	public DocumentOperationType OperationType { get; set; }

	public string DisplayOperationType { get; set; }

	public string UserName { get; set; }

	public SystemType SystemType { get; set; }

	public string DisplaySystemType { get; set; }

	public string DeviceModel { get; set; }

	public string DevicePlatform { get; set; }

	public string ClientVersion { get; set; }
}
