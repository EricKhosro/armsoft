using System.Collections.Generic;
using ArmSoft.OnlineBanking.Common.Enums;

namespace ArmSoft.InternetBank.Web.API.Common.Models;

public class AttachedCard
{
	public int CardId { get; set; }

	public string CardNumber { get; set; }

	public CardTypeGlobal CardType { get; set; }

	public string CardHolderName { get; set; }

	public int ExpirationYear { get; set; }

	public int ExpirationMonth { get; set; }

	public string BankName { get; set; }

	public string Color { get; set; }

	public List<Link> Links { get; set; }

	public bool? IsOwnedByUser { get; set; }
}
