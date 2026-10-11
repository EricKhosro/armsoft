using System;
using System.Text.Json.Serialization;
using ArmSoft.TaxService.Enums;

namespace ArmSoft.TaxService.Models;

public class TaxDoc
{
	private string sortName = string.Empty;

	private InvoiceSort sort;

	private string correctionStateName = string.Empty;

	private CorrectionState correctionState;

	private string correctionTypeName = string.Empty;

	private CorrectionType correctionType;

	private string approvalStateName = string.Empty;

	private ApprovalState approvalState;

	private string cancellationStateName = string.Empty;

	private CancellationState cancellationState;

	private string statusName = string.Empty;

	private Status status;

	private string deletedNotifiedName = string.Empty;

	private bool deletedNotified;

	private string invalidatedNotifiedName = string.Empty;

	private bool invalidatedNotified;

	private string canceledNotifiedName = string.Empty;

	private bool canceledNotified;

	private string canceledResponseNotifiedName = string.Empty;

	private bool canceledResponseNotified;

	private string benCanceledNotifiedName = string.Empty;

	private bool benCanceledNotified;

	private string benInvalidatedNotifiedName = string.Empty;

	private bool benInvalidatedNotified;

	private string benDeletedNotifiedName = string.Empty;

	private bool benDeletedNotified;

	private string benIssuedNotifiedName = string.Empty;

	private bool benIssuedNotified;

	public Guid Id { get; set; }

	public string SerialNo { get; set; } = string.Empty;

	[JsonInclude]
	[JsonPropertyName("type")]
	public string TypeName { get; set; } = string.Empty;

	[JsonIgnore]
	public InvoiceType InvoiceType
	{
		get
		{
			try
			{
				return EncodeDecodeTypes.DecodeInvoiceType(TypeName);
			}
			catch
			{
				return InvoiceType.None;
			}
		}
	}

	[JsonIgnore]
	public AccDocType AccDocType
	{
		get
		{
			try
			{
				return EncodeDecodeTypes.DecodeAccDocType(TypeName);
			}
			catch
			{
				return AccDocType.None;
			}
		}
	}

	[JsonInclude]
	[JsonPropertyName("sort")]
	private string SortName
	{
		get
		{
			return sortName;
		}
		set
		{
			sortName = value;
			sort = EncodeDecodeTypes.DecodeInvoiceSort(sortName);
		}
	}

	[JsonIgnore]
	public InvoiceSort Sort
	{
		get
		{
			return sort;
		}
		set
		{
			sort = value;
			sortName = EncodeDecodeTypes.EncodeInvoiceSort(sort);
		}
	}

	[JsonIgnore]
	public string SortCaption => EncodeDecodeTypes.GetInvoiceSortCaption(Sort);

	[JsonInclude]
	[JsonPropertyName("correctionState")]
	private string CorrectionStateName
	{
		get
		{
			return correctionStateName;
		}
		set
		{
			correctionStateName = value;
			correctionState = EncodeDecodeTypes.DecodeCorrectionState(correctionStateName);
		}
	}

	[JsonIgnore]
	public CorrectionState CorrectionState
	{
		get
		{
			return correctionState;
		}
		set
		{
			correctionState = value;
			correctionStateName = EncodeDecodeTypes.EncodeCorrectionState(correctionState);
		}
	}

	[JsonInclude]
	[JsonPropertyName("correctionType")]
	private string CorrectionTypeName
	{
		get
		{
			return correctionTypeName;
		}
		set
		{
			correctionTypeName = value;
			correctionType = EncodeDecodeTypes.DecodeCorrectionType(correctionTypeName);
		}
	}

	[JsonIgnore]
	public CorrectionType CorrectionType
	{
		get
		{
			return correctionType;
		}
		set
		{
			correctionType = value;
			correctionTypeName = EncodeDecodeTypes.EncodeCorrectionType(correctionType);
		}
	}

	[JsonInclude]
	[JsonPropertyName("approvalState")]
	private string ApprovalStateName
	{
		get
		{
			return approvalStateName;
		}
		set
		{
			approvalStateName = value;
			approvalState = EncodeDecodeTypes.DecodeApprovalState(approvalStateName);
		}
	}

	[JsonIgnore]
	public ApprovalState ApprovalState
	{
		get
		{
			return approvalState;
		}
		set
		{
			approvalState = value;
			approvalStateName = EncodeDecodeTypes.EncodeApprovalState(approvalState);
		}
	}

	[JsonInclude]
	[JsonPropertyName("cancellationState")]
	private string CancellationStateName
	{
		get
		{
			return cancellationStateName;
		}
		set
		{
			cancellationStateName = value;
			cancellationState = EncodeDecodeTypes.DecodeCancellationState(cancellationStateName);
		}
	}

	[JsonIgnore]
	public CancellationState CancellationState
	{
		get
		{
			return cancellationState;
		}
		set
		{
			cancellationState = value;
			cancellationStateName = EncodeDecodeTypes.EncodeCancellationState(cancellationState);
		}
	}

	public string CancellationReason { get; set; } = string.Empty;

	[JsonInclude]
	[JsonPropertyName("status")]
	private string StatusName
	{
		get
		{
			return statusName;
		}
		set
		{
			statusName = value;
			status = EncodeDecodeTypes.DecodeStatus(statusName);
		}
	}

	[JsonIgnore]
	public Status Status
	{
		get
		{
			return status;
		}
		set
		{
			status = value;
			statusName = EncodeDecodeTypes.EncodeStatus(status);
		}
	}

	[JsonIgnore]
	public string StatusCaption => EncodeDecodeTypes.GetStatusCaption(Status);

	[JsonInclude]
	[JsonPropertyName("userName")]
	public string CreatedBy { get; set; } = string.Empty;

	public string BuyerIs { get; set; } = string.Empty;

	public DateTime? CreatedAt { get; set; }

	public DateTime? DealtAt { get; set; }

	public DateTime? IssuedAt { get; set; }

	public DateTime? ApprovedAt { get; set; }

	public DateTime? DeliveredAt { get; set; }

	public DateTime? CancelledAt { get; set; }

	public DateTime? CancellationResponseAt { get; set; }

	public DateTime? InvalidedAt { get; set; }

	public DateTime? DeletedAt { get; set; }

	public string SupplierTin { get; set; } = string.Empty;

	public string SupplierName { get; set; } = string.Empty;

	public string SupplierTaxRegime { get; set; } = string.Empty;

	[JsonIgnore]
	public string SupplierTaxRegimeCaption => EncodeDecodeTypes.GetSupplierTaxRegimeCaption(SupplierTaxRegime);

	public DateTime? SupplierTaxRegimeStart { get; set; }

	public string BuyerTin { get; set; } = string.Empty;

	public string BuyerName { get; set; } = string.Empty;

	public string BeneficiaryTin { get; set; } = string.Empty;

	public string BeneficiaryName { get; set; } = string.Empty;

	public string DeliveryAddress { get; set; } = string.Empty;

	public string DestinationAddress { get; set; } = string.Empty;

	public string AdditionalInfo { get; set; } = string.Empty;

	public decimal? ExcTax { get; set; }

	public decimal? EnvTax { get; set; }

	[JsonInclude]
	[JsonPropertyName("totalValue")]
	public decimal? TotalWithoutVat { get; set; }

	public decimal? TotalVatAmount { get; set; }

	public decimal? Total { get; set; }

	public bool FinalUse { get; set; }

	public bool HasCodes { get; set; }

	public string SubStatus { get; set; }

	[JsonIgnore]
	public string SubStatusCaption => EncodeDecodeTypes.GetSubStatusCaption(SubStatus);

	public string CrmNo { get; set; }

	[JsonInclude]
	[JsonPropertyName("deletedNotified")]
	private string DeletedNotifiedName
	{
		get
		{
			return deletedNotifiedName;
		}
		set
		{
			deletedNotifiedName = value;
			deletedNotified = EncodeDecodeTypes.DecodeNotifyType(deletedNotifiedName);
		}
	}

	[JsonIgnore]
	public bool DeletedNotified
	{
		get
		{
			return deletedNotified;
		}
		set
		{
			deletedNotified = value;
			deletedNotifiedName = EncodeDecodeTypes.EncodeNotifyType(deletedNotified);
		}
	}

	[JsonInclude]
	[JsonPropertyName("invalidatedNotified")]
	private string InvalidatedNotifiedName
	{
		get
		{
			return invalidatedNotifiedName;
		}
		set
		{
			invalidatedNotifiedName = value;
			invalidatedNotified = EncodeDecodeTypes.DecodeNotifyType(invalidatedNotifiedName);
		}
	}

	[JsonIgnore]
	public bool InvalidatedNotified
	{
		get
		{
			return invalidatedNotified;
		}
		set
		{
			invalidatedNotified = value;
			invalidatedNotifiedName = EncodeDecodeTypes.EncodeNotifyType(invalidatedNotified);
		}
	}

	[JsonInclude]
	[JsonPropertyName("canceledNotified")]
	private string CanceledNotifiedName
	{
		get
		{
			return canceledNotifiedName;
		}
		set
		{
			canceledNotifiedName = value;
			canceledNotified = EncodeDecodeTypes.DecodeNotifyType(canceledNotifiedName);
		}
	}

	[JsonIgnore]
	public bool CanceledNotified
	{
		get
		{
			return canceledNotified;
		}
		set
		{
			canceledNotified = value;
			canceledNotifiedName = EncodeDecodeTypes.EncodeNotifyType(canceledNotified);
		}
	}

	[JsonInclude]
	[JsonPropertyName("canceledResponseNotified")]
	private string CanceledResponseNotifiedName
	{
		get
		{
			return canceledResponseNotifiedName;
		}
		set
		{
			canceledResponseNotifiedName = value;
			canceledResponseNotified = EncodeDecodeTypes.DecodeNotifyType(canceledResponseNotifiedName);
		}
	}

	[JsonIgnore]
	public bool CanceledResponseNotified
	{
		get
		{
			return canceledResponseNotified;
		}
		set
		{
			canceledResponseNotified = value;
			canceledResponseNotifiedName = EncodeDecodeTypes.EncodeNotifyType(canceledResponseNotified);
		}
	}

	[JsonInclude]
	[JsonPropertyName("benCanceledNotified")]
	private string BenCanceledNotifiedName
	{
		get
		{
			return benCanceledNotifiedName;
		}
		set
		{
			benCanceledNotifiedName = value;
			benCanceledNotified = EncodeDecodeTypes.DecodeNotifyType(benCanceledNotifiedName);
		}
	}

	[JsonIgnore]
	public bool BenCanceledNotified
	{
		get
		{
			return benCanceledNotified;
		}
		set
		{
			benCanceledNotified = value;
			benCanceledNotifiedName = EncodeDecodeTypes.EncodeNotifyType(benCanceledNotified);
		}
	}

	[JsonInclude]
	[JsonPropertyName("benInvalidatedNotified")]
	private string BenInvalidatedNotifiedName
	{
		get
		{
			return benInvalidatedNotifiedName;
		}
		set
		{
			benInvalidatedNotifiedName = value;
			benInvalidatedNotified = EncodeDecodeTypes.DecodeNotifyType(benInvalidatedNotifiedName);
		}
	}

	[JsonIgnore]
	public bool BenInvalidatedNotified
	{
		get
		{
			return benInvalidatedNotified;
		}
		set
		{
			benInvalidatedNotified = value;
			benInvalidatedNotifiedName = EncodeDecodeTypes.EncodeNotifyType(benInvalidatedNotified);
		}
	}

	[JsonInclude]
	[JsonPropertyName("benDeletedNotified")]
	private string BenDeletedNotifiedName
	{
		get
		{
			return benDeletedNotifiedName;
		}
		set
		{
			benDeletedNotifiedName = value;
			benDeletedNotified = EncodeDecodeTypes.DecodeNotifyType(benDeletedNotifiedName);
		}
	}

	[JsonIgnore]
	public bool BenDeletedNotified
	{
		get
		{
			return benDeletedNotified;
		}
		set
		{
			benDeletedNotified = value;
			benDeletedNotifiedName = EncodeDecodeTypes.EncodeNotifyType(benDeletedNotified);
		}
	}

	[JsonInclude]
	[JsonPropertyName("benIssuedNotified")]
	private string BenIssuedNotifiedName
	{
		get
		{
			return benIssuedNotifiedName;
		}
		set
		{
			benIssuedNotifiedName = value;
			benIssuedNotified = EncodeDecodeTypes.DecodeNotifyType(benIssuedNotifiedName);
		}
	}

	[JsonIgnore]
	public bool BenIssuedNotified
	{
		get
		{
			return benIssuedNotified;
		}
		set
		{
			benIssuedNotified = value;
			benIssuedNotifiedName = EncodeDecodeTypes.EncodeNotifyType(benIssuedNotified);
		}
	}
}
