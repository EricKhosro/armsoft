using System;
using Newtonsoft.Json;

namespace ArmSoft.Cloud.Client.AS7X;

public class KeyInfo
{
	public string Serial { get; set; }

	public string License { get; set; }

	public short Application { get; set; }

	public string AvailableAppVersion { get; set; }

	public int WSLimit { get; set; }

	public int MobileLimit { get; set; }

	public byte? ContractType { get; set; }

	[JsonConverter(typeof(OnlyDateConverter))]
	public DateTime? ContractEndDate { get; set; }

	[JsonConverter(typeof(OnlyDateConverter))]
	public DateTime? ExpirationDate { get; set; }

	public int MobileLimit2 { get; set; }

	public int CustomerId { get; set; }

	public bool Disabled { get; set; }

	public string AvailableUIVersion { get; set; }

	public bool OfflineModeIsAvailable { get; set; }

	public KeyObjective Objective { get; set; }
}
