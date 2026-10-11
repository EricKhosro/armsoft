using System;

namespace ArmSoft.TaxService.Enums;

public static class EncodeDecodeTypes
{
	public static string EncodeBehalfOfType(BehalfOfType value)
	{
		return value switch
		{
			BehalfOfType.None => "", 
			BehalfOfType.TaxPayer => "TAXPAYER", 
			BehalfOfType.Supplier => "SUPPLIER", 
			BehalfOfType.Principal => "PRINCIPAL", 
			BehalfOfType.JointCaseMng => "JOINT_CASE_MNG", 
			_ => throw new ArgumentException("Wrong param value"), 
		};
	}

	public static BehalfOfType DecodeBehalfOfType(string value)
	{
		if (value != null && (value == null || value.Length != 0))
		{
			return value switch
			{
				"TAXPAYER" => BehalfOfType.TaxPayer, 
				"SUPPLIER" => BehalfOfType.Supplier, 
				"PRINCIPAL" => BehalfOfType.Principal, 
				"JOINT_CASE_MNG" => BehalfOfType.JointCaseMng, 
				_ => throw new ArgumentException("Wrong param value"), 
			};
		}
		return BehalfOfType.None;
	}

	public static BehalfOfType DecodeBehalfOf(TaxDocType taxDocType, int procedure)
	{
		if (taxDocType == TaxDocType.Invoice)
		{
			switch (procedure)
			{
			case 2:
				return BehalfOfType.TaxPayer;
			case 3:
				return BehalfOfType.Supplier;
			case 4:
				return BehalfOfType.Principal;
			case 6:
				return BehalfOfType.JointCaseMng;
			}
		}
		else
		{
			switch (procedure)
			{
			case 1:
				return BehalfOfType.TaxPayer;
			case 2:
				return BehalfOfType.Principal;
			}
		}
		return BehalfOfType.None;
	}

	public static int EncodeBehalfOf(TaxDocType taxDocType, BehalfOfType behalfOfType)
	{
		if (taxDocType == TaxDocType.Invoice)
		{
			switch (behalfOfType)
			{
			case BehalfOfType.TaxPayer:
				return 2;
			case BehalfOfType.Supplier:
				return 3;
			case BehalfOfType.Principal:
				return 4;
			case BehalfOfType.JointCaseMng:
				return 6;
			}
		}
		else
		{
			switch (behalfOfType)
			{
			case BehalfOfType.TaxPayer:
				return 1;
			case BehalfOfType.Principal:
				return 2;
			}
		}
		return 0;
	}

	public static string GetBehalfOfCaption(BehalfOfType value)
	{
		return value switch
		{
			BehalfOfType.None => "", 
			BehalfOfType.TaxPayer => Resources.Get(ResourceKey.TaxPayer), 
			BehalfOfType.Supplier => Resources.Get(ResourceKey.Supplier), 
			BehalfOfType.Principal => Resources.Get(ResourceKey.Principal), 
			BehalfOfType.JointCaseMng => Resources.Get(ResourceKey.JointCaseMng), 
			_ => throw new ArgumentException("Wrong param value"), 
		};
	}

	public static string EncodeInvoiceType(InvoiceType value)
	{
		return value switch
		{
			InvoiceType.None => "", 
			InvoiceType.Goods => "GOODS", 
			InvoiceType.Services => "SERVICES", 
			InvoiceType.Excise => "EXCISE", 
			InvoiceType.Leasing => "LEASING", 
			InvoiceType.VatReturn => "VAT_RETURN", 
			_ => throw new ArgumentException("Wrong param value"), 
		};
	}

	public static InvoiceType DecodeInvoiceType(string value)
	{
		if (value != null && (value == null || value.Length != 0))
		{
			return value switch
			{
				"GOODS" => InvoiceType.Goods, 
				"SERVICES" => InvoiceType.Services, 
				"EXCISE" => InvoiceType.Excise, 
				"LEASING" => InvoiceType.Leasing, 
				"VAT_RETURN" => InvoiceType.VatReturn, 
				_ => throw new ArgumentException("Wrong param value"), 
			};
		}
		return InvoiceType.None;
	}

	public static string GetInvoiceCaption(InvoiceType value)
	{
		return value switch
		{
			InvoiceType.None => "", 
			InvoiceType.Goods => Resources.Get(ResourceKey.InvoiceGoods), 
			InvoiceType.Services => Resources.Get(ResourceKey.InvoiceServices), 
			InvoiceType.Excise => Resources.Get(ResourceKey.InvoiceExcise), 
			InvoiceType.Leasing => Resources.Get(ResourceKey.InvoiceLeasing), 
			InvoiceType.VatReturn => Resources.Get(ResourceKey.InvoiceVatReturn), 
			_ => throw new ArgumentException("Wrong param value"), 
		};
	}

	public static string EncodeAccDocType(AccDocType value)
	{
		return value switch
		{
			AccDocType.None => "", 
			AccDocType.AccDocGoods => "ACC_DOC_GOODS", 
			AccDocType.AccDocServices => "ACC_DOC_SERVICES", 
			AccDocType.AccDocExportEEU => "ACC_DOC_EXPORT_EEU", 
			AccDocType.AccDocTraceableG => "ACC_DOC_TRACEABLE_G", 
			AccDocType.AccDocTransportation => "ACC_DOC_TRANSPORTATION", 
			_ => throw new ArgumentException("Wrong param value"), 
		};
	}

	public static AccDocType DecodeAccDocType(string value)
	{
		if (value == null || value.Length != 0)
		{
			return value switch
			{
				"ACC_DOC_GOODS" => AccDocType.AccDocGoods, 
				"ACC_DOC_SERVICES" => AccDocType.AccDocServices, 
				"ACC_DOC_EXPORT_EEU" => AccDocType.AccDocExportEEU, 
				"ACC_DOC_TRACEABLE_G" => AccDocType.AccDocTraceableG, 
				"ACC_DOC_TRANSPORTATION" => AccDocType.AccDocTransportation, 
				_ => throw new ArgumentException("Wrong param value"), 
			};
		}
		return AccDocType.None;
	}

	public static string GetAccDocCaption(AccDocType value)
	{
		return value switch
		{
			AccDocType.None => "", 
			AccDocType.AccDocGoods => Resources.Get(ResourceKey.AccDocGoods), 
			AccDocType.AccDocServices => Resources.Get(ResourceKey.AccDocServices), 
			AccDocType.AccDocExportEEU => Resources.Get(ResourceKey.AccDocExportEEU), 
			AccDocType.AccDocTraceableG => Resources.Get(ResourceKey.AccDocTraceableG), 
			AccDocType.AccDocTransportation => Resources.Get(ResourceKey.AccDocTransportation), 
			_ => throw new ArgumentException("Wrong param value"), 
		};
	}

	public static string EncodeStatus(Status value)
	{
		return value switch
		{
			Status.None => "", 
			Status.Issued => "ISSUED", 
			Status.Approved => "APPROVED", 
			Status.Draft => "DRAFT", 
			Status.Invalid => "INVALID", 
			Status.WaitingInvalidationAcceptance => "WAITING_INVALIDATION_ACCEPTANCE", 
			Status.Cancelled => "CANCELLED", 
			Status.Deleted => "DELETED", 
			_ => throw new ArgumentException("Wrong param value"), 
		};
	}

	public static Status DecodeStatus(string value)
	{
		switch (value)
		{
		case "":
		case null:
			return Status.None;
		case "ISSUED":
			return Status.Issued;
		case "APPROVED":
			return Status.Approved;
		case "DRAFT":
			return Status.Draft;
		case "INVALID":
			return Status.Invalid;
		case "WAITING_INVALIDATION_ACCEPTANCE":
			return Status.WaitingInvalidationAcceptance;
		case "CANCELLED":
			return Status.Cancelled;
		case "DELETED":
			return Status.Deleted;
		default:
			throw new ArgumentException("Wrong param value");
		}
	}

	public static string GetStatusCaption(Status value)
	{
		return value switch
		{
			Status.None => "", 
			Status.Issued => Resources.Get(ResourceKey.SubjectToApproval), 
			Status.Approved => Resources.Get(ResourceKey.ApprovedByBuyer), 
			Status.Draft => Resources.Get(ResourceKey.Draft), 
			Status.Invalid => Resources.Get(ResourceKey.Invalid), 
			Status.WaitingInvalidationAcceptance => Resources.Get(ResourceKey.PendingCnclSt), 
			Status.Cancelled => Resources.Get(ResourceKey.Cancelled), 
			Status.Deleted => Resources.Get(ResourceKey.Deleted), 
			_ => throw new ArgumentException("Wrong param value"), 
		};
	}

	public static string EncodeInvoiceSort(InvoiceSort value)
	{
		return value switch
		{
			InvoiceSort.None => "", 
			InvoiceSort.Special => "SPECIAL", 
			InvoiceSort.TaxPayer => "TAXPAYER", 
			InvoiceSort.Supplier => "SUPPLIER", 
			InvoiceSort.Principal => "PRINCIPAL", 
			InvoiceSort.JointCaseMng => "JOINT_CASE_MNG", 
			InvoiceSort.VatReturn => "VAT_RETURN", 
			_ => throw new ArgumentException("Wrong param value"), 
		};
	}

	public static InvoiceSort DecodeInvoiceSort(string value)
	{
		switch (value)
		{
		case "":
		case null:
			return InvoiceSort.None;
		case "SPECIAL":
			return InvoiceSort.Special;
		case "TAXPAYER":
			return InvoiceSort.TaxPayer;
		case "SUPPLIER":
			return InvoiceSort.Supplier;
		case "PRINCIPAL":
			return InvoiceSort.Principal;
		case "JOINT_CASE_MNG":
			return InvoiceSort.JointCaseMng;
		case "VAT_RETURN":
			return InvoiceSort.VatReturn;
		default:
			throw new ArgumentException("Wrong param value");
		}
	}

	public static string GetInvoiceSortCaption(InvoiceSort value)
	{
		return value switch
		{
			InvoiceSort.None => "", 
			InvoiceSort.Special => Resources.Get(ResourceKey.Special), 
			InvoiceSort.TaxPayer => Resources.Get(ResourceKey.TaxPayer), 
			InvoiceSort.Supplier => Resources.Get(ResourceKey.Supplier), 
			InvoiceSort.Principal => Resources.Get(ResourceKey.Principal), 
			InvoiceSort.JointCaseMng => Resources.Get(ResourceKey.JointCaseMng), 
			InvoiceSort.VatReturn => Resources.Get(ResourceKey.VatReturn), 
			_ => throw new ArgumentException("Wrong param value"), 
		};
	}

	public static string EncodeBuyerIsType(BuyerIsType value)
	{
		return value switch
		{
			BuyerIsType.None => "", 
			BuyerIsType.NoTin => "NO_TIN", 
			BuyerIsType.NaturalPerson => "NATURAL_PERSON", 
			_ => throw new ArgumentException("Wrong param value"), 
		};
	}

	public static BuyerIsType DecodeBuyerIsType(string value)
	{
		if (value != null && (value == null || value.Length != 0))
		{
			if (!(value == "NO_TIN"))
			{
				if (value == "NATURAL_PERSON")
				{
					return BuyerIsType.NaturalPerson;
				}
				throw new ArgumentException("Wrong param value");
			}
			return BuyerIsType.NoTin;
		}
		return BuyerIsType.None;
	}

	public static string EncodeCorrectionState(CorrectionState value)
	{
		return value switch
		{
			CorrectionState.None => "", 
			CorrectionState.Correcting => "CORRECTING", 
			CorrectionState.Corrected => "CORRECTED", 
			CorrectionState.Corrector => "CORRECTOR", 
			_ => throw new ArgumentException("Wrong param value"), 
		};
	}

	public static CorrectionState DecodeCorrectionState(string value)
	{
		if (value != null && (value == null || value.Length != 0))
		{
			return value switch
			{
				"CORRECTING" => CorrectionState.Correcting, 
				"CORRECTED" => CorrectionState.Corrected, 
				"CORRECTOR" => CorrectionState.Corrector, 
				_ => throw new ArgumentException("Wrong param value"), 
			};
		}
		return CorrectionState.None;
	}

	public static string GetCorrectionStateCaption(CorrectionState value)
	{
		return value switch
		{
			CorrectionState.None => "", 
			CorrectionState.Correcting => Resources.Get(ResourceKey.Correcting), 
			CorrectionState.Corrected => Resources.Get(ResourceKey.Corrected), 
			CorrectionState.Corrector => Resources.Get(ResourceKey.Corrector), 
			_ => throw new ArgumentException("Wrong param value"), 
		};
	}

	public static string EncodeCorrectionType(CorrectionType value)
	{
		return value switch
		{
			CorrectionType.None => "", 
			CorrectionType.Increasing => "INCREASING", 
			CorrectionType.Decreasing => "DECREASING", 
			_ => throw new ArgumentException("Wrong param value"), 
		};
	}

	public static CorrectionType DecodeCorrectionType(string value)
	{
		if (value != null && (value == null || value.Length != 0))
		{
			if (!(value == "INCREASING"))
			{
				if (value == "DECREASING")
				{
					return CorrectionType.Decreasing;
				}
				throw new ArgumentException("Wrong param value");
			}
			return CorrectionType.Increasing;
		}
		return CorrectionType.None;
	}

	public static string GetCorrectionTypeCaption(CorrectionType value)
	{
		return value switch
		{
			CorrectionType.None => "", 
			CorrectionType.Increasing => Resources.Get(ResourceKey.Increasing), 
			CorrectionType.Decreasing => Resources.Get(ResourceKey.Decreasing), 
			_ => throw new ArgumentException("Wrong param value"), 
		};
	}

	public static string EncodeApprovalState(ApprovalState value)
	{
		return value switch
		{
			ApprovalState.None => "", 
			ApprovalState.Approved => "APPROVED", 
			ApprovalState.Pending => "PENDING", 
			_ => throw new ArgumentException("Wrong param value"), 
		};
	}

	public static ApprovalState DecodeApprovalState(string value)
	{
		if (value != null && (value == null || value.Length != 0))
		{
			if (!(value == "APPROVED"))
			{
				if (value == "PENDING")
				{
					return ApprovalState.Pending;
				}
				throw new ArgumentException("Wrong param value");
			}
			return ApprovalState.Approved;
		}
		return ApprovalState.None;
	}

	public static string GetApprovalStateCaption(ApprovalState value)
	{
		return value switch
		{
			ApprovalState.None => "", 
			ApprovalState.Approved => Resources.Get(ResourceKey.Approved), 
			ApprovalState.Pending => Resources.Get(ResourceKey.PendingAppSt), 
			_ => throw new ArgumentException("Wrong param value"), 
		};
	}

	public static string EncodeCancellationState(CancellationState value)
	{
		return value switch
		{
			CancellationState.None => "", 
			CancellationState.Pending => "PENDING", 
			CancellationState.Accept => "ACCEPT", 
			CancellationState.Decline => "DECLINE", 
			_ => throw new ArgumentException("Wrong param value"), 
		};
	}

	public static CancellationState DecodeCancellationState(string value)
	{
		if (value != null && (value == null || value.Length != 0))
		{
			return value switch
			{
				"PENDING" => CancellationState.Pending, 
				"ACCEPT" => CancellationState.Accept, 
				"DECLINE" => CancellationState.Decline, 
				_ => throw new ArgumentException("Wrong param value"), 
			};
		}
		return CancellationState.None;
	}

	public static string GetCancellationStateCaption(CancellationState value)
	{
		return value switch
		{
			CancellationState.None => "", 
			CancellationState.Pending => Resources.Get(ResourceKey.PendingCnclSt), 
			CancellationState.Accept => Resources.Get(ResourceKey.Accepted), 
			CancellationState.Decline => Resources.Get(ResourceKey.Declined), 
			_ => throw new ArgumentException("Wrong param value"), 
		};
	}

	public static string EncodeNotifyType(bool value)
	{
		if (value)
		{
			return "TRUTH";
		}
		return "FALSE";
	}

	public static bool DecodeNotifyType(string value)
	{
		if (!(value == "TRUTH"))
		{
			if (value == "FALSE")
			{
				return false;
			}
			throw new ArgumentException("Wrong param value");
		}
		return true;
	}

	public static string EncodeIdentityType(IdentityType value)
	{
		return value switch
		{
			IdentityType.None => "", 
			IdentityType.RAPassport => "RA_PASSPORT", 
			IdentityType.BirthCert => "BIRTH_CERT", 
			IdentityType.ResidenceCard => "RESIDENCE_CARD", 
			IdentityType.ConventionalTravelDoc => "CONVENTIONAL_TRAVEL_DOCUMENT", 
			IdentityType.IdCard => "ID_CARD", 
			IdentityType.BiometricPassport => "BIOMETRIC_PASSPORT", 
			IdentityType.ForeignPassportWork => "FOREIGN_PASSPORT_WORK", 
			IdentityType.ForeignPassport => "FOREIGN_PASSPORT", 
			IdentityType.ForeignBirthCert => "FOREIGN_BIRTH_CERT", 
			_ => throw new ArgumentException("Wrong param value"), 
		};
	}

	public static IdentityType DecodeIdentityType(string value)
	{
		switch (value)
		{
		case "":
		case null:
			return IdentityType.None;
		case "RA_PASSPORT":
			return IdentityType.RAPassport;
		case "BIRTH_CERT":
			return IdentityType.BirthCert;
		case "RESIDENCE_CARD":
			return IdentityType.ResidenceCard;
		case "CONVENTIONAL_TRAVEL_DOCUMENT":
			return IdentityType.ConventionalTravelDoc;
		case "ID_CARD":
			return IdentityType.IdCard;
		case "BIOMETRIC_PASSPORT":
			return IdentityType.BiometricPassport;
		case "FOREIGN_PASSPORT_WORK":
			return IdentityType.ForeignPassportWork;
		case "FOREIGN_PASSPORT":
			return IdentityType.ForeignPassport;
		case "FOREIGN_BIRTH_CERT":
			return IdentityType.ForeignBirthCert;
		default:
			throw new ArgumentException("Wrong param value");
		}
	}

	public static string EncodeVatRate(VatRate value)
	{
		return value switch
		{
			VatRate.None => "", 
			VatRate.NoVat => "NONE", 
			VatRate.Vat20 => "VAT_20", 
			_ => throw new ArgumentException("Wrong param value"), 
		};
	}

	public static VatRate DecodeVatRate(string value)
	{
		if (value != null && (value == null || value.Length != 0))
		{
			if (!(value == "NONE"))
			{
				if (value == "VAT_20")
				{
					return VatRate.Vat20;
				}
				throw new ArgumentException("Wrong param value");
			}
			return VatRate.NoVat;
		}
		return VatRate.None;
	}

	public static string EncodePreviousDocumentType(PreviousDocumentType value)
	{
		return value switch
		{
			PreviousDocumentType.None => "", 
			PreviousDocumentType.CustomsDeclaration => "CUSTOMS_DECLARATION", 
			PreviousDocumentType.AccountingDocument => "ACCOUNTING_DOCUMENT", 
			PreviousDocumentType.Other => "OTHER", 
			_ => throw new ArgumentException("Wrong param value"), 
		};
	}

	public static PreviousDocumentType DecodePreviousDocumentType(string value)
	{
		if (value != null && (value == null || value.Length != 0))
		{
			return value switch
			{
				"CUSTOMS_DECLARATION" => PreviousDocumentType.CustomsDeclaration, 
				"ACCOUNTING_DOCUMENT" => PreviousDocumentType.AccountingDocument, 
				"OTHER" => PreviousDocumentType.Other, 
				_ => throw new ArgumentException("Wrong param value"), 
			};
		}
		return PreviousDocumentType.None;
	}

	public static string EncodeTransportationMethod(TransportationMethod value)
	{
		return value switch
		{
			TransportationMethod.None => "", 
			TransportationMethod.Air => "AIR", 
			TransportationMethod.Overland => "OVERLAND", 
			TransportationMethod.Other => "OTHER", 
			_ => throw new ArgumentException("Wrong param value"), 
		};
	}

	public static TransportationMethod DecodeTransportationMethod(string value)
	{
		if (value != null && (value == null || value.Length != 0))
		{
			return value switch
			{
				"AIR" => TransportationMethod.Air, 
				"OVERLAND" => TransportationMethod.Overland, 
				"OTHER" => TransportationMethod.Other, 
				_ => throw new ArgumentException("Wrong param value"), 
			};
		}
		return TransportationMethod.None;
	}

	public static string EncodeEeuCountry(EeuCountry value)
	{
		return value switch
		{
			EeuCountry.None => "", 
			EeuCountry.AM => "AM", 
			EeuCountry.BY => "BY", 
			EeuCountry.KZ => "KZ", 
			EeuCountry.KG => "KG", 
			EeuCountry.RU => "RU", 
			_ => throw new ArgumentException("Wrong param value"), 
		};
	}

	public static EeuCountry DecodeEeuCountry(string value)
	{
		if (value != null && (value == null || value.Length != 0))
		{
			return value switch
			{
				"AM" => EeuCountry.AM, 
				"BY" => EeuCountry.BY, 
				"KZ" => EeuCountry.KZ, 
				"KG" => EeuCountry.KG, 
				"RU" => EeuCountry.RU, 
				_ => throw new ArgumentException("Wrong param value"), 
			};
		}
		return EeuCountry.None;
	}

	public static string EncodeSource(Source value)
	{
		return value switch
		{
			Source.None => "", 
			Source.Import => "IMPORT", 
			Source.Web => "WEB", 
			Source.Batch => "BATCH", 
			Source.API => "API", 
			_ => throw new ArgumentException("Wrong param value"), 
		};
	}

	public static Source DecodeSource(string value)
	{
		if (value != null && (value == null || value.Length != 0))
		{
			return value switch
			{
				"IMPORT" => Source.Import, 
				"WEB" => Source.Web, 
				"BATCH" => Source.Batch, 
				"API" => Source.API, 
				_ => throw new ArgumentException("Wrong param value"), 
			};
		}
		return Source.None;
	}

	public static string EncodeSignState(SignState value)
	{
		return value switch
		{
			SignState.HashCalculating => "HASH_CALCULATING", 
			SignState.SignWaiting => "SIGN_WAITING", 
			SignState.Failed => "FAILED", 
			SignState.SignProcessing => "SIGN_PROCESSING", 
			SignState.Signed => "SIGNED", 
			SignState.SignFailed => "SIGN_FAILED", 
			_ => throw new ArgumentException("Wrong param value"), 
		};
	}

	public static SignState DecodeSignState(string value)
	{
		return value switch
		{
			"HASH_CALCULATING" => SignState.HashCalculating, 
			"SIGN_WAITING" => SignState.SignWaiting, 
			"FAILED" => SignState.Failed, 
			"SIGN_PROCESSING" => SignState.SignProcessing, 
			"SIGNED" => SignState.Signed, 
			"SIGN_FAILED" => SignState.SignFailed, 
			_ => throw new ArgumentException("Wrong param value"), 
		};
	}

	public static string GetSignStateCaption(string value)
	{
		return value switch
		{
			"HASH_CALCULATING" => Resources.Get(ResourceKey.HashCalculating), 
			"SIGN_WAITING" => Resources.Get(ResourceKey.SignWaiting), 
			"FAILED" => Resources.Get(ResourceKey.Failed), 
			"SIGN_PROCESSING" => Resources.Get(ResourceKey.SignProcessing), 
			"SIGNED" => Resources.Get(ResourceKey.Signed), 
			"SIGN_FAILED" => Resources.Get(ResourceKey.SignFailed), 
			_ => throw new ArgumentException("Wrong param value - " + value), 
		};
	}

	public static string EncodeSocialCostType(SocialCostType value)
	{
		return value switch
		{
			SocialCostType.None => "", 
			SocialCostType.EducationalCost => "EDUCATIONAL_COST", 
			SocialCostType.HealthInsurance => "HEALTH_INSURANCE", 
			SocialCostType.DentalServices => "DENTAL_SERVICES", 
			SocialCostType.Other => "OTHER", 
			_ => throw new ArgumentException("Wrong param value"), 
		};
	}

	public static SocialCostType DecodeSocialCostType(string value)
	{
		if (value == null || value.Length != 0)
		{
			return value switch
			{
				"EDUCATIONAL_COST" => SocialCostType.EducationalCost, 
				"HEALTH_INSURANCE" => SocialCostType.HealthInsurance, 
				"DENTAL_SERVICES" => SocialCostType.DentalServices, 
				"OTHER" => SocialCostType.Other, 
				_ => throw new ArgumentException("Wrong param value"), 
			};
		}
		return SocialCostType.None;
	}

	public static string EncodeRequestType(RequestType value)
	{
		return value switch
		{
			RequestType.Issue => "ISSUE", 
			RequestType.Approve => "APPROVE", 
			_ => throw new ArgumentException("Wrong param value"), 
		};
	}

	public static RequestType DecodeRequestType(string value)
	{
		if (!(value == "ISSUE"))
		{
			if (value == "APPROVE")
			{
				return RequestType.Approve;
			}
			throw new ArgumentException("Wrong param value");
		}
		return RequestType.Issue;
	}

	public static string GetInvoiceTypeCaption(string value)
	{
		return value switch
		{
			"GOODS" => Resources.Get(ResourceKey.InvoiceGoods), 
			"SERVICES" => Resources.Get(ResourceKey.InvoiceServices), 
			"EXCISE" => Resources.Get(ResourceKey.InvoiceExcise), 
			"LEASING" => Resources.Get(ResourceKey.InvoiceLeasing), 
			"VAT_RETURN" => Resources.Get(ResourceKey.InvoiceVatReturn), 
			"ACC_DOC_GOODS" => Resources.Get(ResourceKey.AccDocGoods), 
			"ACC_DOC_SERVICES" => Resources.Get(ResourceKey.AccDocServices), 
			"ACC_DOC_EXPORT_EEU" => Resources.Get(ResourceKey.AccDocExportEEU), 
			"ACC_DOC_TRACEABLE_G" => Resources.Get(ResourceKey.AccDocTraceableG), 
			"ACC_DOC_TRANSPORTATION" => Resources.Get(ResourceKey.AccDocTransportation), 
			_ => throw new ArgumentException("Wrong param value"), 
		};
	}

	public static string EncodeInvoiceDealType(InvoiceDealType? value)
	{
		return value switch
		{
			null => "", 
			InvoiceDealType.VatOther => "VAT_OTHER", 
			InvoiceDealType.VatZeroRate => "VAT_ZERO_RATE", 
			InvoiceDealType.VatExempt => "VAT_EXEMPT", 
			_ => throw new ArgumentException("Wrong param value"), 
		};
	}

	public static InvoiceDealType? DecodeInvoiceDealType(string value)
	{
		switch (value)
		{
		case "":
		case null:
			return null;
		case "VAT_OTHER":
			return InvoiceDealType.VatOther;
		case "VAT_ZERO_RATE":
			return InvoiceDealType.VatZeroRate;
		case "VAT_EXEMPT":
			return InvoiceDealType.VatExempt;
		default:
			throw new ArgumentException("Wrong param value");
		}
	}

	public static string GetInvoiceDealTypeCaption(string value)
	{
		return value switch
		{
			"VAT_OTHER" => Resources.Get(ResourceKey.VatOther), 
			"VAT_ZERO_RATE" => Resources.Get(ResourceKey.VatZeroRate), 
			"VAT_EXEMPT" => Resources.Get(ResourceKey.VatExempt), 
			_ => throw new ArgumentException("Wrong param value"), 
		};
	}

	public static string EncodeAccDealType(AccDealType? value)
	{
		return value switch
		{
			null => "", 
			AccDealType.SpecialTaxSystem => "SPECIAL_TAX_SYSTEM", 
			AccDealType.VatExempt => "VAT_EXEMPT", 
			AccDealType.VatZeroRate => "VAT_ZERO_RATE", 
			AccDealType.Export => "EXPORT", 
			AccDealType.ExportEAEUAir => "EXPORT_EAEU_AIR", 
			AccDealType.ExcludedFromVAT => "EXCLUDED_FROM_VAT", 
			AccDealType.VATNonPayers => "VAT_NON_PAYERS", 
			_ => throw new ArgumentException("Wrong param value"), 
		};
	}

	public static AccDealType? DecodeAccDealType(string value)
	{
		if (!(value == ""))
		{
			switch (value)
			{
			case null:
				break;
			case "SPECIAL_TAX_SYSTEM":
				return AccDealType.SpecialTaxSystem;
			case "VAT_EXEMPT":
				return AccDealType.VatExempt;
			case "VAT_ZERO_RATE":
				return AccDealType.VatZeroRate;
			case "EXPORT":
				return AccDealType.Export;
			case "EXPORT_EAEU_AIR":
				return AccDealType.ExportEAEUAir;
			case "EXCLUDED_FROM_VAT":
				return AccDealType.ExcludedFromVAT;
			case "VAT_NON_PAYERS":
				return AccDealType.VATNonPayers;
			default:
				throw new ArgumentException("Wrong param value");
			}
		}
		return null;
	}

	public static string GetAccDealTypeCaption(string value)
	{
		return value switch
		{
			"SPECIAL_TAX_SYSTEM" => Resources.Get(ResourceKey.SpecialTaxSystem), 
			"VAT_EXEMPT" => Resources.Get(ResourceKey.VatExempt), 
			"VAT_ZERO_RATE" => Resources.Get(ResourceKey.VatZeroRate), 
			"EXPORT" => Resources.Get(ResourceKey.Export), 
			"EXPORT_EAEU_AIR" => Resources.Get(ResourceKey.ExportEAEUAir), 
			"EXCLUDED_FROM_VAT" => Resources.Get(ResourceKey.ExcludedFromVAT), 
			"VAT_NON_PAYERS" => Resources.Get(ResourceKey.VatNonPayers), 
			_ => throw new ArgumentException("Wrong param value"), 
		};
	}

	public static string EncodeSubStatus(SubStatusType value)
	{
		return value switch
		{
			SubStatusType.Pending => "PENDING", 
			SubStatusType.CheckedInProgress => "CHECKED_IN_PROGRESS", 
			SubStatusType.CheckedChanged => "CHECKED_CHANGED", 
			SubStatusType.CheckedNotChanged => "CHECKED_NOT_CHANGED", 
			SubStatusType.CheckedPartiallyChanged => "CHECKED_PARTIAL_CHANGED", 
			SubStatusType.UncheckedInProgress => "UNCHECKED_IN_PROGRESS", 
			SubStatusType.UncheckedChanged => "UNCHECKED_CHANGED", 
			SubStatusType.UncheckedNotChanged => "UNCHECKED_NOT_CHANGED", 
			SubStatusType.UncheckedPartiallyChanged => "UNCHECKED_PARTIAL_CHANGED", 
			SubStatusType.UncheckedAutoChanged => "UNCHECKED_AUTO_CHANGED", 
			SubStatusType.UncheckedAutoNotChanged => "UNCHECKED_AUTO_NOT_CHANGED", 
			SubStatusType.UncheckedAutoPartiallyChanged => "UNCHECKED_AUTO_PARTIAL_CHANGED", 
			SubStatusType.CheckedRevInProgress => "CHECKED_REV_IN_PROGRESS", 
			SubStatusType.CheckedRevChanged => "CHECKED_REV_CHANGED", 
			SubStatusType.CheckedRevNotChanged => "CHECKED_REV_NOT_CHANGED", 
			SubStatusType.CheckedRevPartiallyChanged => "CHECKED_REV_PARTIAL_CHANGED", 
			SubStatusType.UncheckedRevInProgress => "UNCHECKED_REV_IN_PROGRESS", 
			SubStatusType.UncheckedRevChanged => "UNCHECKED_REV_CHANGED", 
			SubStatusType.UncheckedRevNotChanged => "UNCHECKED_REV_NOT_CHANGED", 
			SubStatusType.UncheckedRevPartiallyChanged => "UNCHECKED_REV_PARTIAL_CHANGED", 
			_ => throw new ArgumentException("Wrong param value"), 
		};
	}

	public static SubStatusType DecodeSubStatus(string value)
	{
		return value switch
		{
			"PENDING" => SubStatusType.Pending, 
			"CHECKED_IN_PROGRESS" => SubStatusType.CheckedInProgress, 
			"CHECKED_CHANGED" => SubStatusType.CheckedChanged, 
			"CHECKED_NOT_CHANGED" => SubStatusType.CheckedNotChanged, 
			"CHECKED_PARTIAL_CHANGED" => SubStatusType.CheckedPartiallyChanged, 
			"UNCHECKED_IN_PROGRESS" => SubStatusType.UncheckedInProgress, 
			"UNCHECKED_CHANGED" => SubStatusType.UncheckedChanged, 
			"UNCHECKED_NOT_CHANGED" => SubStatusType.UncheckedNotChanged, 
			"UNCHECKED_PARTIAL_CHANGED" => SubStatusType.UncheckedPartiallyChanged, 
			"UNCHECKED_AUTO_CHANGED" => SubStatusType.UncheckedAutoChanged, 
			"UNCHECKED_AUTO_NOT_CHANGED" => SubStatusType.UncheckedAutoNotChanged, 
			"UNCHECKED_AUTO_PARTIAL_CHANGED" => SubStatusType.UncheckedAutoPartiallyChanged, 
			"CHECKED_REV_IN_PROGRESS" => SubStatusType.CheckedRevInProgress, 
			"CHECKED_REV_CHANGED" => SubStatusType.CheckedRevChanged, 
			"CHECKED_REV_NOT_CHANGED" => SubStatusType.CheckedRevNotChanged, 
			"CHECKED_REV_PARTIAL_CHANGED" => SubStatusType.CheckedRevPartiallyChanged, 
			"UNCHECKED_REV_IN_PROGRESS" => SubStatusType.UncheckedRevInProgress, 
			"UNCHECKED_REV_CHANGED" => SubStatusType.UncheckedRevChanged, 
			"UNCHECKED_REV_NOT_CHANGED" => SubStatusType.UncheckedRevNotChanged, 
			"UNCHECKED_REV_PARTIAL_CHANGED" => SubStatusType.UncheckedRevPartiallyChanged, 
			_ => throw new ArgumentException("Wrong param value"), 
		};
	}

	public static string GetSubStatusCaption(string value)
	{
		switch (value)
		{
		case "":
		case null:
			return "";
		case "PENDING":
			return Resources.Get(ResourceKey.Pending);
		case "CHECKED_IN_PROGRESS":
			return Resources.Get(ResourceKey.CheckedInProgress);
		case "CHECKED_CHANGED":
			return Resources.Get(ResourceKey.CheckedChanged);
		case "CHECKED_NOT_CHANGED":
			return Resources.Get(ResourceKey.CheckedNotChanged);
		case "CHECKED_PARTIAL_CHANGED":
			return Resources.Get(ResourceKey.CheckedPartiallyChanged);
		case "UNCHECKED_IN_PROGRESS":
			return Resources.Get(ResourceKey.UncheckedInProgress);
		case "UNCHECKED_CHANGED":
			return Resources.Get(ResourceKey.UncheckedChanged);
		case "UNCHECKED_NOT_CHANGED":
			return Resources.Get(ResourceKey.UncheckedNotChanged);
		case "UNCHECKED_PARTIAL_CHANGED":
			return Resources.Get(ResourceKey.UncheckedPartiallyChanged);
		case "UNCHECKED_AUTO_CHANGED":
			return Resources.Get(ResourceKey.UncheckedAutoChanged);
		case "UNCHECKED_AUTO_NOT_CHANGED":
			return Resources.Get(ResourceKey.UncheckedAutoNotChanged);
		case "UNCHECKED_AUTO_PARTIAL_CHANGED":
			return Resources.Get(ResourceKey.UncheckedAutoPartiallyChanged);
		case "CHECKED_REV_IN_PROGRESS":
			return Resources.Get(ResourceKey.CheckedRevInProgress);
		case "CHECKED_REV_CHANGED":
			return Resources.Get(ResourceKey.CheckedRevChanged);
		case "CHECKED_REV_NOT_CHANGED":
			return Resources.Get(ResourceKey.CheckedRevNotChanged);
		case "CHECKED_REV_PARTIAL_CHANGED":
			return Resources.Get(ResourceKey.CheckedRevPartiallyChanged);
		case "UNCHECKED_REV_IN_PROGRESS":
			return Resources.Get(ResourceKey.UncheckedRevInProgress);
		case "UNCHECKED_REV_CHANGED":
			return Resources.Get(ResourceKey.UncheckedRevChanged);
		case "UNCHECKED_REV_NOT_CHANGED":
			return Resources.Get(ResourceKey.UncheckedRevNotChanged);
		case "UNCHECKED_REV_PARTIAL_CHANGED":
			return Resources.Get(ResourceKey.UncheckedRevPartiallyChanged);
		default:
			throw new ArgumentException("Wrong param value");
		}
	}

	public static string EncodeSupplierTaxRegime(SupplierTaxRegime value)
	{
		return value switch
		{
			SupplierTaxRegime.None => "", 
			SupplierTaxRegime.Vat => "VAT", 
			SupplierTaxRegime.Turnover => "TURNOVER", 
			SupplierTaxRegime.Micro => "MICRO", 
			SupplierTaxRegime.Any => "ANY", 
			_ => throw new ArgumentException("Wrong param value"), 
		};
	}

	public static SupplierTaxRegime DecodeSupplierTaxRegime(string value)
	{
		if (value == null || value.Length != 0)
		{
			return value switch
			{
				"VAT" => SupplierTaxRegime.Vat, 
				"TURNOVER" => SupplierTaxRegime.Turnover, 
				"MICRO" => SupplierTaxRegime.Micro, 
				"ANY" => SupplierTaxRegime.Any, 
				_ => throw new ArgumentException("Wrong param value"), 
			};
		}
		return SupplierTaxRegime.None;
	}

	public static string GetSupplierTaxRegimeCaption(string value)
	{
		if (value != null && (value == null || value.Length != 0))
		{
			return value switch
			{
				"VAT" => Resources.Get(ResourceKey.VatPayer), 
				"TURNOVER" => Resources.Get(ResourceKey.SalesTaxPayer), 
				"MICRO" => Resources.Get(ResourceKey.MicroenterpriseEntity), 
				"ANY" => Resources.Get(ResourceKey.OtherTaxPayer), 
				_ => throw new ArgumentException("Wrong param value"), 
			};
		}
		return "";
	}
}
