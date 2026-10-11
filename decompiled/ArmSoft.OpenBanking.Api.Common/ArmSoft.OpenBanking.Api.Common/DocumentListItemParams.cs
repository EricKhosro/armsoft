using System;
using System.ComponentModel;
using ArmSoft.OnlineBanking.Common.Enums;

namespace ArmSoft.OpenBanking.Api.Common;

public class DocumentListItemParams
{
	[DefaultValue(null)]
	public string? Number { get; set; }

	[DefaultValue(null)]
	public string? Type { get; set; }

	[DefaultValue(null)]
	public DateTime DateFrom { get; set; }

	[DefaultValue(null)]
	public DateTime DateTo { get; set; }

	[DefaultValue(null)]
	public int? Count { get; set; }

	[DefaultValue(null)]
	public DocumentState? State { get; set; }

	[DefaultValue(null)]
	public decimal? AmountFrom { get; set; }

	[DefaultValue(null)]
	public decimal? AmountTo { get; set; }

	[DefaultValue(null)]
	public string? Currency { get; set; }

	[DefaultValue(null)]
	public string? PayerAccount { get; set; }

	[DefaultValue(null)]
	public string? Receiver { get; set; }

	public bool IncludeCreatorNames { get; set; }

	[DefaultValue(null)]
	public string? Category { get; set; }
}
