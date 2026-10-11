using System;

namespace ArmSoft.Cloud.Client.AS7X;

public class ContractBalance
{
	public string Key { get; set; }

	public string Contract { get; set; }

	public string Name { get; set; }

	public short Application { get; set; }

	public DateTime? ContractEndDate { get; set; }

	public int? RemainingSupportDays { get; set; }

	public decimal? Balance { get; set; }

	public decimal? NextPayment { get; set; }
}
