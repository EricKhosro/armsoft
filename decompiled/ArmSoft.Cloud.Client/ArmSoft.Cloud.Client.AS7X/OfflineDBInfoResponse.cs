using System;
using Newtonsoft.Json;

namespace ArmSoft.Cloud.Client.AS7X;

public class OfflineDBInfoResponse
{
	[JsonConverter(typeof(OnlyDateConverter))]
	public DateTime? DeleteDocsBefore { get; set; }
}
