using System;
using System.Collections.Generic;

namespace ArmSoft.SME.Trade.Common.Models.DPR;

public class DocumentsReProcessParam
{
	public DateOnly EndActualnessDate { get; set; }

	public bool BringIncomesToDayBegin { get; set; }

	public bool BringExpensesToDayEnd { get; set; }

	public string BringComplectationsType { get; set; }

	public string BringMovesType { get; set; }

	public List<short> DocTypes { get; set; }
}
