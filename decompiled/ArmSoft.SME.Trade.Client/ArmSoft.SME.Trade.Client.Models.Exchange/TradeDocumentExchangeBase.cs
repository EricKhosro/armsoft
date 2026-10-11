using ArmSoft.SME.Client.Models;

namespace ArmSoft.SME.Trade.Client.Models.Exchange;

public abstract class TradeDocumentExchangeBase : ExchangeBase
{
	public bool IsDraft { get; set; }

	public bool IsDeleted { get; set; }

	public string Owner { get; set; } = string.Empty;
}
