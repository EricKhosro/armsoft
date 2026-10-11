using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace ArmSoft.SME.Trade.AddIn.Client.Models;

[ComVisible(true)]
[Guid("DCAE47D7-AB4B-4140-81B2-AECB72B7F7D1")]
[ClassInterface(ClassInterfaceType.AutoDual)]
public class MTExpense : ExchangeBase
{
	public string DocumentNumber { get; set; } = string.Empty;

	public DateTime DocumentDate { get; set; }

	public string Comment { get; set; } = string.Empty;

	public string Storage { get; set; } = string.Empty;

	public string Trust { get; set; } = string.Empty;

	public string Mediator { get; set; } = string.Empty;

	public string Machine { get; set; } = string.Empty;

	public string ChiefAccountant { get; set; } = string.Empty;

	public string Allower { get; set; } = string.Empty;

	public string Demand { get; set; } = string.Empty;

	public string BaseISN { get; set; } = string.Empty;

	public string ISN { get; set; } = string.Empty;

	[DataMember(IsRequired = false, Order = 11)]
	public List<MTExpenseSpecificationRow> Specification { get; set; }

	public MTExpenseSpecificationRow AddRow()
	{
		if (Specification == null)
		{
			Specification = new List<MTExpenseSpecificationRow>();
		}
		MTExpenseSpecificationRow mTExpenseSpecificationRow = new MTExpenseSpecificationRow();
		Specification.Add(mTExpenseSpecificationRow);
		return mTExpenseSpecificationRow;
	}
}
