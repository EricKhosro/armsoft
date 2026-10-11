using System;
using System.Collections.Generic;

namespace ArmSoft.SME.Trade.AddIn.Client.Models;

public class CashModel
{
	public DateTime? StartDate { get; set; }

	public DateTime? EndDate { get; set; }

	public string Cash { get; set; }

	public string PrGroup { get; set; }

	public List<string> Partners { get; set; }

	public string OpTypes { get; set; }

	public string Cur { get; set; }

	public string Bank { get; set; }
}
