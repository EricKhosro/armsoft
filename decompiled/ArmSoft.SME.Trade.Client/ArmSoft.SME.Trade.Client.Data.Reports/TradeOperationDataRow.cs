using System;
using ArmSoft.SME.Client.Data;
using ArmSoft.SME.Common.Models;

namespace ArmSoft.SME.Trade.Client.Data.Reports;

public class TradeOperationDataRow : ExtendableRow, IMatchedUpdateKey, IRowWithISN
{
	public Guid BaseISN { get; set; }

	public short DocType { get; set; }

	public short GetDocType()
	{
		return DocType;
	}

	public Guid GetDocISN()
	{
		return BaseISN;
	}

	public bool Matched(object key)
	{
		return (Guid)key == BaseISN;
	}
}
