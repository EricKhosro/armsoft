using System;

namespace ArmSoft.SME.Common.Models.Directories;

public class RatesOfExchange : Directory
{
	public DateOnly Date { get; set; }

	public decimal Rate { get; set; }

	public decimal Base { get; set; }
}
