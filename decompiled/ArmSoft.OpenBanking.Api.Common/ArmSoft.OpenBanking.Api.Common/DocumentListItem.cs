using System;
using ArmSoft.OnlineBanking.Common.Enums;
using Newtonsoft.Json;

namespace ArmSoft.OpenBanking.Api.Common;

public class DocumentListItem
{
	public Guid Id { get; set; }

	public int? CreatorUserId { get; set; }

	public byte[] Version { get; set; } = Array.Empty<byte>();

	public string Type { get; set; } = string.Empty;

	public string DisplayType { get; set; } = string.Empty;

	public DocumentState State { get; set; }

	public string DisplayStateDetailed { get; set; } = string.Empty;

	public string DisplayState { get; set; } = string.Empty;

	public CancelRequestState CancelRequestState { get; set; }

	public string DisplaySubState { get; set; } = string.Empty;

	public string Number { get; set; } = string.Empty;

	[JsonConverter(typeof(DateConverter))]
	public DateTime Date { get; set; }

	public string? Details { get; set; }

	public bool CanBeSigned { get; set; }

	public bool CanBeDeleted { get; set; }

	public bool CanBeCanceled { get; set; }

	public string? Receiver { get; set; }

	public string? UtilityCode { get; set; }

	public UtilityType UtilityType { get; set; }

	public string Category { get; set; } = string.Empty;

	public decimal? Amount { get; set; }

	public string? Currency { get; set; }

	public string FileName { get; set; } = string.Empty;

	public string Comments { get; set; } = string.Empty;

	public string PayerAccountOrCard { get; set; } = string.Empty;

	public string ReceiverId { get; set; } = string.Empty;

	public bool AreFirstSignaturesCompleted { get; set; }

	public bool AreSecondSignaturesCompleted { get; set; }

	public bool AreThirdSignaturesCompleted { get; set; }

	public bool IsImported { get; set; }

	public bool IsClientSignatureRequired { get; set; }

	public bool IsSignedByPartner { get; set; }

	public string? CreatorFirstName { get; set; }

	public string? CreatorLastName { get; set; }
}
