using System;
using System.Collections.Generic;

namespace ArmSoft.SME.Common.Models.Directories;

public class ImportExchangeRatesRequestModel
{
	public DateOnly StartDate { get; set; }

	public DateOnly EndDate { get; set; }

	public List<RatesOfExchangeDataRow> RatesOfExchange { get; set; }
}
