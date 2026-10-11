using System;
using System.Collections.Generic;

namespace ArmSoft.SME.Trade.Common.CustomTypes;

public class MTIncomePriceAndDiscountPriceForListRequest
{
	public Guid ISN { get; set; }

	public string CurrCode { get; set; }

	public int PartId { get; set; }

	public DateOnly Date { get; set; }

	public List<MTByQntUnit> Materials { get; set; } = new List<MTByQntUnit>();
}
