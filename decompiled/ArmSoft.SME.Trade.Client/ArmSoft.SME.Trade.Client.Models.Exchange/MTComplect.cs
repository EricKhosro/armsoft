using System.Collections.Generic;
using ArmSoft.SME.Client.Models;

namespace ArmSoft.SME.Trade.Client.Models.Exchange;

public class MTComplect : ExchangeBase
{
	public List<ComplectPart> ComplectParts { get; set; }
}
