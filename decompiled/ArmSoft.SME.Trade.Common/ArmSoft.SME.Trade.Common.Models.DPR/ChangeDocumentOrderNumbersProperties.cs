using System;

namespace ArmSoft.SME.Trade.Common.Models.DPR;

public class ChangeDocumentOrderNumbersProperties
{
	public Guid ISN { get; set; }

	public DateOnly DocDate { get; set; }

	public int DocOrderNum { get; set; }
}
