using System;

namespace ArmSoft.OpenBanking.Api.Common;

public class Card
{
	public string Id { get; set; }

	public int ExternalId { get; set; }

	public string Number { get; set; } = string.Empty;

	public string DisplayNumber { get; set; } = string.Empty;

	public int AccountId { get; set; }

	public string AccountNumber { get; set; } = string.Empty;

	public decimal? AccountInterestRate { get; set; }

	public string Currency { get; set; } = string.Empty;

	public string Type { get; set; } = string.Empty;

	public string DisplayType { get; set; } = string.Empty;

	public string TypeColor { get; set; } = string.Empty;

	public int? DesignId { get; set; }

	public string? ImageUrl { get; set; }

	public string? SmallImageUrl { get; set; }

	public byte TypeGlobal { get; set; }

	public string Name { get; set; } = string.Empty;

	public string Email { get; set; } = string.Empty;

	public DateTime ValidFrom { get; set; }

	public DateTime ValidThru { get; set; }

	public string Note { get; set; } = string.Empty;

	public string MainCardNumber { get; set; } = string.Empty;

	public string DisplayMainCardNumber { get; set; } = string.Empty;

	public bool IsDigital { get; set; }

	public decimal? AccountBalance { get; set; }

	public decimal? AccountExpectedBalance { get; set; }
}
