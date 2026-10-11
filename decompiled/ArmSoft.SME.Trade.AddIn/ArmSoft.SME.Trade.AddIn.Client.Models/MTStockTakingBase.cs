using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace ArmSoft.SME.Trade.AddIn.Client.Models;

[ComVisible(true)]
[Guid("DF09F79C-9E2B-4D55-9A9A-E69D523A79D4")]
[ClassInterface(ClassInterfaceType.AutoDual)]
public class MTStockTakingBase : ExchangeBase
{
	public DateTime DocumentDate { get; set; }

	public string DocumentNumber { get; set; } = string.Empty;

	public string Storage { get; set; } = string.Empty;

	public string GoodGroup { get; set; } = string.Empty;

	public string CurrencyCode { get; set; } = string.Empty;

	public decimal CurrencyExchangeRate { get; set; }

	public decimal CurrencyExchangeBase { get; set; }

	public string PriceType { get; set; } = string.Empty;

	public string Comment { get; set; } = string.Empty;

	public string ISN { get; set; } = string.Empty;

	public List<MTStockTakingSpecificationRow> Specification { get; set; }

	public MTStockTakingSpecificationRow AddRow()
	{
		if (Specification == null)
		{
			Specification = new List<MTStockTakingSpecificationRow>();
		}
		MTStockTakingSpecificationRow mTStockTakingSpecificationRow = new MTStockTakingSpecificationRow();
		Specification.Add(mTStockTakingSpecificationRow);
		return mTStockTakingSpecificationRow;
	}
}
