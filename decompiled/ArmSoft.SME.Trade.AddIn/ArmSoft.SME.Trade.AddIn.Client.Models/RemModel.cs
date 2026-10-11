using System;
using System.Collections.Generic;

namespace ArmSoft.SME.Trade.AddIn.Client.Models;

public class RemModel
{
	public DateTime? RemDate { get; set; }

	public List<string> Partners { get; set; }

	public string Cur { get; set; }

	public short Supplier { get; set; }

	public short Customer { get; set; }

	public string PrGroup { get; set; }

	public string Contract { get; set; }

	public string Partner { get; set; }
}
