using System.Collections.Generic;
using ArmSoft.SME.Common.Models.PrintableElements;
using ArmSoft.SME.Trade.Common.Models.Other;

namespace ArmSoft.SME.Trade.Common.Models.PrintableElements;

public class IDCard : PrintableElement
{
	public string Caption { get; set; }

	public List<PartnerContractInfo> ContractsList { get; set; }

	public string Prefix { get; set; }

	public string Suffix { get; set; }
}
