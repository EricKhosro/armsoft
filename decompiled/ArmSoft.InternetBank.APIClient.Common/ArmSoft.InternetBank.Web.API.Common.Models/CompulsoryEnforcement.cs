using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace ArmSoft.InternetBank.Web.API.Common.Models;

public class CompulsoryEnforcement
{
	public string DebtorName { get; set; }

	public string PlaintiffName { get; set; }

	public string InquestId { get; set; }

	[JsonConverter(typeof(DateConverter))]
	public DateTime InquestDate { get; set; }

	public decimal RequestedAmount { get; set; }

	public decimal ChargeAmount { get; set; }

	public decimal PayableAmount { get; set; }

	public decimal TotalAmount { get; set; }

	public string Account { get; set; }

	public string Department { get; set; }

	public List<CompulsoryEnforcementInfo> AllCompulsoryEnforcements { get; set; }
}
