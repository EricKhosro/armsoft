using System;

namespace ArmSoft.SME.Common.Models.Exchange;

public class DocumentExchangeRow
{
	public short DocType { get; set; }

	public Guid ISN { get; set; }
}
