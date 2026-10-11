using System;
using ArmSoft.OnlineBanking.Common.Enums;

namespace ArmSoft.InternetBank.Web.API.Common;

public class DocumentsQueryParams
{
	public string Number { get; set; }

	public string Type { get; set; }

	public Period? Period { get; set; }

	public DateTime? DateFrom { get; set; }

	public DateTime? DateTo { get; set; }

	public int? Count { get; set; }

	public DocumentState? State { get; set; }

	public decimal? AmountFrom { get; set; }

	public decimal? AmountTo { get; set; }

	public string Currency { get; set; }

	public string Account { get; set; }

	public string Receiver { get; set; }

	public DocumentAccessLevels? AccessLevel { get; set; }

	public bool IncludeLinks { get; set; }

	public DocumentsQueryParams()
	{
		IncludeLinks = true;
	}
}
