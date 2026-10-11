using System;

namespace ArmSoft.InternetBank.Web.API.Common.Models.Evoca;

public class EvocaBondConfiguration
{
	public string Guid { get; set; }

	public string BondName { get; set; }

	public string Currency { get; set; }

	public int MaxCount { get; set; }

	public int MinCount { get; set; }

	public decimal PercentPerDay { get; set; }

	public decimal NominalAmount { get; set; }

	public int Duration { get; set; }

	public string TermUrl { get; set; }

	public DateTime? StartDate { get; set; }

	public DateTime? EndDate { get; set; }
}
