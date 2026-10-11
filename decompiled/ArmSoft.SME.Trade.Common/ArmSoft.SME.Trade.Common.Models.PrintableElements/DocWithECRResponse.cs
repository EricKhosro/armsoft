using System;
using ArmSoft.SME.Common.Models.PrintableElements;

namespace ArmSoft.SME.Trade.Common.Models.PrintableElements;

public class DocWithECRResponse : PrintableElement
{
	public Guid DocISN { get; set; }

	public short DocumentType { get; set; }
}
