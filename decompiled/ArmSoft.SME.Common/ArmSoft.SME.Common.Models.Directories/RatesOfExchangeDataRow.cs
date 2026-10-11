using System;

namespace ArmSoft.SME.Common.Models.Directories;

public class RatesOfExchangeDataRow
{
	public DateOnly Date { get; set; }

	public string Curr { get; set; }

	public decimal Rate { get; set; }

	public decimal Base { get; set; }
}
