using System;
using Newtonsoft.Json;

namespace ArmSoft.Cloud.Client.Discount;

public class CardCreateRequest
{
	public string CardNumber { get; set; }

	public string BarCode { get; set; }

	public string CardHolder { get; set; }

	public string Gender { get; set; }

	[JsonConverter(typeof(OnlyDateConverter))]
	public DateTime? BirthDate { get; set; }

	public string PhoneNumber { get; set; }

	public string Email { get; set; }

	public decimal BonusPercent { get; set; }

	public DateTime? ValidPeriodStart { get; set; }

	public DateTime? ValidPeriodEnd { get; set; }

	public bool Canceled { get; set; }

	public bool Synced { get; set; }

	public DateTime? SyncDate { get; set; }
}
