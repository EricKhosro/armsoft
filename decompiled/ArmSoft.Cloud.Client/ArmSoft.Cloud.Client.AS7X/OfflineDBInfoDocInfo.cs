using System;
using Newtonsoft.Json;

namespace ArmSoft.Cloud.Client.AS7X;

public class OfflineDBInfoDocInfo
{
	[JsonConverter(typeof(OnlyDateConverter))]
	public DateTime DocDate { get; set; }

	public short DocType { get; set; }

	public int Total { get; set; }

	public int NotSynchronized { get; set; }
}
