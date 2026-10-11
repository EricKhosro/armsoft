using ArmSoft.OnlineBanking.Common.Enums;

namespace ArmSoft.InternetBank.Web.API.Common.Models;

public class PayerAccountData
{
	public int Id { get; set; }

	public string AccountNumber { get; set; }

	public string DisplayAccountNumber { get; set; }

	public string Currency { get; set; }

	public string Name { get; set; }

	public AccountType Type { get; set; }

	public string AccessType { get; set; }

	public string DisplayType { get; set; }

	public string Note { get; set; }

	public decimal? Balance { get; set; }

	public string CardNumber { get; set; }

	public string CardType { get; set; }

	public string DisplayCardType { get; set; }

	public string CardTypeColor { get; set; }

	public string CardDesign { get; set; }

	public string CardImageUrl { get; set; }

	public string CardSmallImageUrl { get; set; }

	public byte CardTypeGlobal { get; set; }
}
