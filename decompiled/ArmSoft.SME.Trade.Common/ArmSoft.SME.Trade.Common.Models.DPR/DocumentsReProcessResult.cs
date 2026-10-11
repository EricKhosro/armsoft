using System;
using System.Collections.Generic;

namespace ArmSoft.SME.Trade.Common.Models.DPR;

public class DocumentsReProcessResult
{
	public Dictionary<string, Guid> Errors { get; set; }
}
