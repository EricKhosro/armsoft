using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace ArmSoft.SME.Trade.AddIn.Client.Models;

[ComVisible(true)]
[Guid("7129FD2F-C26B-4D2B-A08C-B5C83A61409F")]
[ClassInterface(ClassInterfaceType.AutoDual)]
public class PartnerRemGroupCorrect : ExchangeBase
{
	public DateTime DocumentDate { get; set; }

	public string DocumentNumber { get; set; } = string.Empty;

	public string CurrencyCode { get; set; } = string.Empty;

	public decimal CurrencyExchangeRate { get; set; }

	public decimal CurrencyExchangeBase { get; set; }

	public string Comment { get; set; } = string.Empty;

	public List<PartRemSpecificationRow> Specification { get; set; }

	public PartRemSpecificationRow AddRow()
	{
		if (Specification == null)
		{
			Specification = new List<PartRemSpecificationRow>();
		}
		PartRemSpecificationRow partRemSpecificationRow = new PartRemSpecificationRow();
		Specification.Add(partRemSpecificationRow);
		return partRemSpecificationRow;
	}
}
