using System;
using System.Linq;
using System.Threading;
using ArmSoft.OnlineBanking.Common.Constants;
using ArmSoft.OnlineBanking.Common.Enums;
using ArmSoft.OnlineBanking.Common.Evoca;
using ArmSoft.OnlineBanking.Common.Resources;

namespace ArmSoft.OnlineBanking.Common.Utility;

public static class Decode
{
	public static string Value(object value, bool isOperator, string format = null, object model = null)
	{
		if (value == null)
		{
			return string.Empty;
		}
		if (value is decimal value2)
		{
			if (!string.IsNullOrEmpty(format))
			{
				return value2.ToString(format);
			}
			return Format.Amount(value2);
		}
		if (value is DateTime value3)
		{
			if (!string.IsNullOrEmpty(format))
			{
				return Format.Date(value3, format);
			}
			return Format.Date(value3);
		}
		if (value is bool)
		{
			if (!(bool)value)
			{
				return ArmSoft.OnlineBanking.Common.Resources.Resources.No;
			}
			return ArmSoft.OnlineBanking.Common.Resources.Resources.Yes;
		}
		if (value is DocumentState documentState)
		{
			return DocState(documentState, model, isOperator);
		}
		return value.ToString();
	}

	public static string FieldName(string typeName, string fieldName)
	{
		return ArmSoft.OnlineBanking.Common.Resources.Resources.Get("Field_" + typeName + "_" + fieldName);
	}

	public static string FieldName(Type entityType, string fieldName)
	{
		if (entityType == typeof(object))
		{
			return ArmSoft.OnlineBanking.Common.Resources.Resources.Get("Field_" + fieldName);
		}
		string entityName = entityType.Name;
		foreach (string item in new string[6] { "Dto", "ListItem", "Extended", "Information", "Model", "Base" }.Where((string sufix) => entityName.EndsWith(sufix)))
		{
			entityName = entityName.Substring(0, entityName.Length - item.Length);
		}
		string text = ArmSoft.OnlineBanking.Common.Resources.Resources.Get("Field_" + entityName + "_" + fieldName);
		if (string.IsNullOrEmpty(text))
		{
			text = FieldName(entityType.BaseType, fieldName);
		}
		return text;
	}

	public static string SecurityProviderType(string type, bool isUsingAppNames = true)
	{
		return type switch
		{
			"DPGOX" => ArmSoft.OnlineBanking.Common.Resources.Resources.SecurityProviderDigipassGoSeries, 
			"DP2XX" => ArmSoft.OnlineBanking.Common.Resources.Resources.SecurityProviderDigipass250300Series, 
			"DPMOB" => ArmSoft.OnlineBanking.Common.Resources.Resources.SecurityProviderDigipassMobile, 
			"MPSMS" => ArmSoft.OnlineBanking.Common.Resources.Resources.SecurityProviderMobileSMS, 
			"GOOGL" => ArmSoft.OnlineBanking.Common.Resources.Resources.SecurityProviderGoogleAuth, 
			"DIGIPASS" => ArmSoft.OnlineBanking.Common.Resources.Resources.SecurityProviderDigipass, 
			"EMPTY" => ArmSoft.OnlineBanking.Common.Resources.Resources.SecurityProviderDeactivate, 
			_ => ArmSoft.OnlineBanking.Common.Resources.Resources.SecurityProviderNone, 
		};
	}

	public static string SystemType(SystemType type)
	{
		switch (type)
		{
		case ArmSoft.OnlineBanking.Common.Enums.SystemType.WebSite:
			return ArmSoft.OnlineBanking.Common.Resources.Resources.SystemTypeWebSite;
		case ArmSoft.OnlineBanking.Common.Enums.SystemType.MobileSite:
			return ArmSoft.OnlineBanking.Common.Resources.Resources.SystemTypeMobileSite;
		case ArmSoft.OnlineBanking.Common.Enums.SystemType.MobileApplication:
			return ArmSoft.OnlineBanking.Common.Resources.Resources.SystemTypeMobileApplication;
		case ArmSoft.OnlineBanking.Common.Enums.SystemType.Web:
			return ArmSoft.OnlineBanking.Common.Resources.Resources.SystemTypeWeb;
		case ArmSoft.OnlineBanking.Common.Enums.SystemType.ImportService:
			return ArmSoft.OnlineBanking.Common.Resources.Resources.SystemTypeImportService;
		case ArmSoft.OnlineBanking.Common.Enums.SystemType.ASAccountant:
			return ArmSoft.OnlineBanking.Common.Resources.Resources.SystemTypeAccountant;
		case ArmSoft.OnlineBanking.Common.Enums.SystemType.AdminWebSite:
			return ArmSoft.OnlineBanking.Common.Resources.Resources.SystemTypeAdminWebSite;
		case ArmSoft.OnlineBanking.Common.Enums.SystemType.WebApiClientAdmin:
		case ArmSoft.OnlineBanking.Common.Enums.SystemType.WebApiClient:
		case ArmSoft.OnlineBanking.Common.Enums.SystemType.ASBank:
		case ArmSoft.OnlineBanking.Common.Enums.SystemType.ASEnterprise:
		case ArmSoft.OnlineBanking.Common.Enums.SystemType.ASHumanResourceManagement:
		case ArmSoft.OnlineBanking.Common.Enums.SystemType.ASAccountantApplication:
		case ArmSoft.OnlineBanking.Common.Enums.SystemType.ASBookkeeper:
		case ArmSoft.OnlineBanking.Common.Enums.SystemType.ASSalesManagement:
		case ArmSoft.OnlineBanking.Common.Enums.SystemType.ASOrderApplication:
		case ArmSoft.OnlineBanking.Common.Enums.SystemType.ASTrade:
		case ArmSoft.OnlineBanking.Common.Enums.SystemType.ASShop:
		case ArmSoft.OnlineBanking.Common.Enums.SystemType.ASMarketPlace:
		case ArmSoft.OnlineBanking.Common.Enums.SystemType.ASFinMobile:
			return ArmSoft.OnlineBanking.Common.Resources.Resources.SystemTypeOther;
		case ArmSoft.OnlineBanking.Common.Enums.SystemType.CorporateApiClient:
			return ArmSoft.OnlineBanking.Common.Resources.Resources.SystemTypeCorporateApi;
		case ArmSoft.OnlineBanking.Common.Enums.SystemType.WorkerService:
			return ArmSoft.OnlineBanking.Common.Resources.Resources.SystemTypeWorkerService;
		case ArmSoft.OnlineBanking.Common.Enums.SystemType.Android:
			return ArmSoft.OnlineBanking.Common.Resources.Resources.SystemTypeAndroid;
		case ArmSoft.OnlineBanking.Common.Enums.SystemType.IOS:
			return ArmSoft.OnlineBanking.Common.Resources.Resources.SystemTypeIOS;
		case ArmSoft.OnlineBanking.Common.Enums.SystemType.CreditPlatform:
			return ArmSoft.OnlineBanking.Common.Resources.Resources.SystemTypeCreditPlatform;
		case ArmSoft.OnlineBanking.Common.Enums.SystemType.OwnApplication:
			return ArmSoft.OnlineBanking.Common.Resources.Resources.SystemTypeCustomApplication;
		case ArmSoft.OnlineBanking.Common.Enums.SystemType.All:
			return ArmSoft.OnlineBanking.Common.Resources.Resources.SystemTypeAll;
		default:
			return string.Empty;
		}
	}

	public static string DocumentOperationType(DocumentOperationType operationType)
	{
		return operationType switch
		{
			ArmSoft.OnlineBanking.Common.Enums.DocumentOperationType.NotSet => ArmSoft.OnlineBanking.Common.Resources.Resources.DocumentOperationNotSet, 
			ArmSoft.OnlineBanking.Common.Enums.DocumentOperationType.Created => ArmSoft.OnlineBanking.Common.Resources.Resources.DocumentOperationCreated, 
			ArmSoft.OnlineBanking.Common.Enums.DocumentOperationType.Edited => ArmSoft.OnlineBanking.Common.Resources.Resources.DocumentOperationEdited, 
			ArmSoft.OnlineBanking.Common.Enums.DocumentOperationType.Signed => ArmSoft.OnlineBanking.Common.Resources.Resources.DocumentOperationSigned, 
			ArmSoft.OnlineBanking.Common.Enums.DocumentOperationType.FirstSign => ArmSoft.OnlineBanking.Common.Resources.Resources.DocumentOperationSignedByFirst, 
			ArmSoft.OnlineBanking.Common.Enums.DocumentOperationType.SecondSign => ArmSoft.OnlineBanking.Common.Resources.Resources.DocumentOperationSignedBySecond, 
			ArmSoft.OnlineBanking.Common.Enums.DocumentOperationType.ThirdSign => ArmSoft.OnlineBanking.Common.Resources.Resources.DocumentOperationSignedByThird, 
			ArmSoft.OnlineBanking.Common.Enums.DocumentOperationType.Sent => ArmSoft.OnlineBanking.Common.Resources.Resources.DocumentOperationSent, 
			ArmSoft.OnlineBanking.Common.Enums.DocumentOperationType.Confirmed => ArmSoft.OnlineBanking.Common.Resources.Resources.DocumentOperationConfirmed, 
			ArmSoft.OnlineBanking.Common.Enums.DocumentOperationType.Refused => ArmSoft.OnlineBanking.Common.Resources.Resources.DocumentOperationRefused, 
			ArmSoft.OnlineBanking.Common.Enums.DocumentOperationType.Canceled => ArmSoft.OnlineBanking.Common.Resources.Resources.DocumentOperationCanceled, 
			ArmSoft.OnlineBanking.Common.Enums.DocumentOperationType.SendToEdit => ArmSoft.OnlineBanking.Common.Resources.Resources.DocumentOperationSentToEdit, 
			ArmSoft.OnlineBanking.Common.Enums.DocumentOperationType.ConfirmedCommissionChange => ArmSoft.OnlineBanking.Common.Resources.Resources.DocumentOperationConfirmedCommissionChange, 
			ArmSoft.OnlineBanking.Common.Enums.DocumentOperationType.SignatureQueued => ArmSoft.OnlineBanking.Common.Resources.Resources.DocumentOperationSignatureQueued, 
			ArmSoft.OnlineBanking.Common.Enums.DocumentOperationType.SignatureFailed => ArmSoft.OnlineBanking.Common.Resources.Resources.DocumentOperationSignatureFailed, 
			ArmSoft.OnlineBanking.Common.Enums.DocumentOperationType.SignatureIgnored => ArmSoft.OnlineBanking.Common.Resources.Resources.DocumentOperationSignatureIgnored, 
			_ => string.Empty, 
		};
	}

	public static string SecurityProviderRefreshOption(SecurityProviderRefreshOption type)
	{
		return type switch
		{
			ArmSoft.OnlineBanking.Common.Enums.SecurityProviderRefreshOption.WithOtp => ArmSoft.OnlineBanking.Common.Resources.Resources.SecurityProviderRefreshTypeWithOtp, 
			ArmSoft.OnlineBanking.Common.Enums.SecurityProviderRefreshOption.WithBiometric => ArmSoft.OnlineBanking.Common.Resources.Resources.SecurityProviderRefreshTypeWithBiometric, 
			_ => string.Empty, 
		};
	}

	public static string ResidenceTypes(ResidenceType type)
	{
		return type switch
		{
			ResidenceType.Resident => ArmSoft.OnlineBanking.Common.Resources.Resources.Resident, 
			ResidenceType.NonResident => ArmSoft.OnlineBanking.Common.Resources.Resources.NonResident, 
			_ => string.Empty, 
		};
	}

	public static string ClientStatus(ClientStatus type)
	{
		return type switch
		{
			ArmSoft.OnlineBanking.Common.Enums.ClientStatus.NotSet => string.Empty, 
			ArmSoft.OnlineBanking.Common.Enums.ClientStatus.Legal => ArmSoft.OnlineBanking.Common.Resources.Resources.ClientStatusLegal, 
			ArmSoft.OnlineBanking.Common.Enums.ClientStatus.Natural => ArmSoft.OnlineBanking.Common.Resources.Resources.ClientStatusNatural, 
			ArmSoft.OnlineBanking.Common.Enums.ClientStatus.Entrepreneur => ArmSoft.OnlineBanking.Common.Resources.Resources.ClientStatusEntrepreneur, 
			ArmSoft.OnlineBanking.Common.Enums.ClientStatus.All => ArmSoft.OnlineBanking.Common.Resources.Resources.ClientStatusAll, 
			_ => string.Empty, 
		};
	}

	public static string RecipientLegalStatus(ReceiverLegalStatus type)
	{
		return type switch
		{
			ReceiverLegalStatus.NotSet => string.Empty, 
			ReceiverLegalStatus.Legal => ArmSoft.OnlineBanking.Common.Resources.Resources.ReceiverLegalStatusLegal, 
			ReceiverLegalStatus.Natural => ArmSoft.OnlineBanking.Common.Resources.Resources.ClientStatusNatural, 
			_ => string.Empty, 
		};
	}

	public static string PassportTypes(PassportType type)
	{
		return type switch
		{
			PassportType.RA => ArmSoft.OnlineBanking.Common.Resources.Resources.PassportRA, 
			PassportType.Foreign => ArmSoft.OnlineBanking.Common.Resources.Resources.PassportForeign, 
			PassportType.DeathCertificate => ArmSoft.OnlineBanking.Common.Resources.Resources.PassportDeathCertificate, 
			_ => string.Empty, 
		};
	}

	public static string AccountTypes(AccountType type)
	{
		return type switch
		{
			AccountType.Settlement => ArmSoft.OnlineBanking.Common.Resources.Resources.AccountTypeSettlement, 
			AccountType.Card => ArmSoft.OnlineBanking.Common.Resources.Resources.AccountTypeCard, 
			AccountType.Other => ArmSoft.OnlineBanking.Common.Resources.Resources.AccountTypeOther, 
			_ => string.Empty, 
		};
	}

	public static string CardStatus(CardStatus cardStatus)
	{
		switch (cardStatus)
		{
		case ArmSoft.OnlineBanking.Common.Enums.CardStatus.Active:
			return ArmSoft.OnlineBanking.Common.Resources.Resources.CardStatusActive;
		case ArmSoft.OnlineBanking.Common.Enums.CardStatus.BlockedByUser:
			return ArmSoft.OnlineBanking.Common.Resources.Resources.CardStatusBlocked;
		case ArmSoft.OnlineBanking.Common.Enums.CardStatus.BlockedByBank:
			return ArmSoft.OnlineBanking.Common.Resources.Resources.CardStatusBlockedByBank;
		case ArmSoft.OnlineBanking.Common.Enums.CardStatus.Expired:
			return ArmSoft.OnlineBanking.Common.Resources.Resources.CardStatusExpired;
		case ArmSoft.OnlineBanking.Common.Enums.CardStatus.MustBeActivatedWithPin:
		case ArmSoft.OnlineBanking.Common.Enums.CardStatus.MustBeActivatedWithoutPin:
			return ArmSoft.OnlineBanking.Common.Resources.Resources.CardStatusInactive;
		default:
			return string.Empty;
		}
	}

	public static string DocType(string docType, bool onlyArmenianOrEnglish = false)
	{
		string text = Thread.CurrentThread.CurrentUICulture.Name;
		if (onlyArmenianOrEnglish && text != "hy-AM")
		{
			text = "en-US";
		}
		return DocTypeByLanguage(docType, text);
	}

	public static string TaxPropertyType(TaxPropertyType type, string language = null)
	{
		string result = string.Empty;
		switch (type)
		{
		case ArmSoft.OnlineBanking.Common.Enums.TaxPropertyType.GarbageCollection:
			result = ArmSoft.OnlineBanking.Common.Resources.Resources.Get("TaxPropertyTypeGarbageCollection", language);
			break;
		case ArmSoft.OnlineBanking.Common.Enums.TaxPropertyType.Land:
			result = ArmSoft.OnlineBanking.Common.Resources.Resources.Get("TaxPropertyTypeLand", language);
			break;
		case ArmSoft.OnlineBanking.Common.Enums.TaxPropertyType.Realty:
			result = ArmSoft.OnlineBanking.Common.Resources.Resources.Get("TaxPropertyTypeRealty", language);
			break;
		case ArmSoft.OnlineBanking.Common.Enums.TaxPropertyType.Transport:
			result = ArmSoft.OnlineBanking.Common.Resources.Resources.Get("TaxPropertyTypeTransport", language);
			break;
		case ArmSoft.OnlineBanking.Common.Enums.TaxPropertyType.WaterTransport:
			result = ArmSoft.OnlineBanking.Common.Resources.Resources.Get("TaxPropertyTypeWaterTransport", language);
			break;
		}
		return result;
	}

	public static string DocTypeByLanguage(string docType, string language, LocalizableLabel customizationName = null)
	{
		string result = string.Empty;
		string documentSubType = GetDocumentSubType(docType);
		if (!string.IsNullOrEmpty(documentSubType))
		{
			if (GetDocumentGeneralType(docType) == "AL" && AdvancedLocalPaymentTypes.Contains(documentSubType))
			{
				switch (documentSubType)
				{
				case "ALSP":
					result = ArmSoft.OnlineBanking.Common.Resources.Resources.Get("DocumentTypeSoldierPayment", language);
					break;
				case "ALCP":
					result = ArmSoft.OnlineBanking.Common.Resources.Resources.Get("AdvancedPaymentCreditRepayment", language);
					break;
				case "ALGP":
					result = ArmSoft.OnlineBanking.Common.Resources.Resources.Get("DocumentTypeAdvancedTransfer", language);
					break;
				}
			}
			else if (customizationName != null)
			{
				result = customizationName.GetTextByLanguage(language);
			}
		}
		else
		{
			switch (GetDocumentGeneralType(docType))
			{
			case "PP":
				result = ArmSoft.OnlineBanking.Common.Resources.Resources.Get("DocumentTypePassportPayment", language);
				break;
			case "DA":
				result = ArmSoft.OnlineBanking.Common.Resources.Resources.Get("DocumentTypeDepositAttraction", language);
				break;
			case "OA":
				result = ArmSoft.OnlineBanking.Common.Resources.Resources.Get("DocumentTypeOpenAccountApplication", language);
				break;
			case "AC":
				result = ArmSoft.OnlineBanking.Common.Resources.Resources.Get("DocumentTypeCloseAccountApplication", language);
				break;
			case "BP":
				result = ArmSoft.OnlineBanking.Common.Resources.Resources.Get("DocumentTypeBudgetaryPayment", language);
				break;
			case "PO":
				result = ArmSoft.OnlineBanking.Common.Resources.Resources.Get("DocumentTypeLocalPayment", language);
				break;
			case "GA":
				result = ArmSoft.OnlineBanking.Common.Resources.Resources.Get("DocumentTypeGoalAccountClose", language);
				break;
			case "MP":
				result = ArmSoft.OnlineBanking.Common.Resources.Resources.Get("DocumentTypeMultiplePayment", language);
				break;
			case "PF":
				result = ArmSoft.OnlineBanking.Common.Resources.Resources.Get("DocumentTypeForeignPayment", language);
				break;
			case "PR":
				result = ArmSoft.OnlineBanking.Common.Resources.Resources.Get("DocumentTypeForeignPaymentRus", language);
				break;
			case "CP":
				result = ArmSoft.OnlineBanking.Common.Resources.Resources.Get("DocumentTypeCommunalPayment", language);
				break;
			case "CC":
				result = ArmSoft.OnlineBanking.Common.Resources.Resources.Get("DocumentTypeCurrencyConversion", language);
				break;
			case "CR":
				result = ArmSoft.OnlineBanking.Common.Resources.Resources.Get("DocumentTypeCreditRepayment", language);
				break;
			case "RA":
				result = ArmSoft.OnlineBanking.Common.Resources.Resources.Get("DocumentTypeCreditRepaymentWithAttachedCard", language);
				break;
			case "RC":
				result = ArmSoft.OnlineBanking.Common.Resources.Resources.Get("DocumentTypeRequestCancel", language);
				break;
			case "MC":
				result = ArmSoft.OnlineBanking.Common.Resources.Resources.Get("DocumentTypeMultipleCommunal", language);
				break;
			case "OD":
				result = ArmSoft.OnlineBanking.Common.Resources.Resources.Get("DocumentTypeOpenDepositApplication", language);
				break;
			case "OC":
				result = ArmSoft.OnlineBanking.Common.Resources.Resources.Get("DocumentTypeOpenCreditApplication", language);
				break;
			case "DC":
				result = ArmSoft.OnlineBanking.Common.Resources.Resources.Get("DocumentTypeOpenDigitalCardApplication", language);
				break;
			case "EL":
				result = ArmSoft.OnlineBanking.Common.Resources.Resources.Get("DocumentTypeEvocaGetLoanApplication", language);
				break;
			case "EG":
				result = ArmSoft.OnlineBanking.Common.Resources.Resources.Get("DocumentTypeEvocaGiftCardApplication", language);
				break;
			case "ED":
				result = ArmSoft.OnlineBanking.Common.Resources.Resources.Get("DocumentTypeEvocaDigitalGiftCardApplication", language);
				break;
			case "EB":
				result = ArmSoft.OnlineBanking.Common.Resources.Resources.Get("DocumentTypeEvocaGetBondApplication", language);
				break;
			case "EM":
				result = ArmSoft.OnlineBanking.Common.Resources.Resources.Get("DocumentTypeEvocaGetLoanApplication", language);
				break;
			case "AS":
				result = ArmSoft.OnlineBanking.Common.Resources.Resources.Get("DocumentTypeArmSoftApplication", language);
				break;
			case "CW":
				result = ArmSoft.OnlineBanking.Common.Resources.Resources.Get("DocumentTypeCashWithdrawalApplication", language);
				break;
			case "TP":
				result = ArmSoft.OnlineBanking.Common.Resources.Resources.Get("DocumentTypeTaxPayment", language);
				break;
			case "GC":
				result = ArmSoft.OnlineBanking.Common.Resources.Resources.Get("DocumentTypeGarbageCollectionPayment", language);
				break;
			case "PT":
				result = ArmSoft.OnlineBanking.Common.Resources.Resources.Get("DocumentTypePropertyTaxPayment", language);
				break;
			case "VT":
				result = ArmSoft.OnlineBanking.Common.Resources.Resources.Get("DocumentTypeVehicleTaxPayment", language);
				break;
			case "RT":
				result = ArmSoft.OnlineBanking.Common.Resources.Resources.Get("DocumentTypeRealEstateTaxPayment", language);
				break;
			case "DP":
				result = ArmSoft.OnlineBanking.Common.Resources.Resources.Get("DocumentTypePaymentsPackage", language);
				break;
			case "AP":
				result = ArmSoft.OnlineBanking.Common.Resources.Resources.Get("DocumentTypeCreditRepaymentViaPartner", language);
				break;
			case "FC":
				result = ArmSoft.OnlineBanking.Common.Resources.Resources.Get("DocumentTypeFriendCreditRepayment", language);
				break;
			case "RP":
				result = ArmSoft.OnlineBanking.Common.Resources.Resources.Get("DocumentTypeRoadPolicePayment", language);
				break;
			case "CT":
				result = ArmSoft.OnlineBanking.Common.Resources.Resources.Get("DocumentTypeContactTransfer", language);
				break;
			case "AR":
				result = ArmSoft.OnlineBanking.Common.Resources.Resources.Get("DocumentTypeAccountReplenishment", language);
				break;
			case "TC":
				result = ArmSoft.OnlineBanking.Common.Resources.Resources.Get("DocumentTypeCardTransfer", language);
				break;
			case "CM":
				result = ArmSoft.OnlineBanking.Common.Resources.Resources.Get("DocumentTypeCashOutByPhone", language);
				break;
			case "CE":
				result = ArmSoft.OnlineBanking.Common.Resources.Resources.Get("DocumentTypeCompulsoryEnforcementPayment", language);
				break;
			case "AW":
				result = ArmSoft.OnlineBanking.Common.Resources.Resources.Get("DocumentTypeASWAPayment", language);
				break;
			case "CA":
				result = ArmSoft.OnlineBanking.Common.Resources.Resources.Get("DocumentTypeCustomizedApplication", language);
				break;
			case "AL":
				result = ArmSoft.OnlineBanking.Common.Resources.Resources.Get("OtherTransfer", language);
				break;
			case "AT":
				result = ArmSoft.OnlineBanking.Common.Resources.Resources.Get("DocumentTypeArCaP2PTransfer", language);
				break;
			case "PB":
				result = ArmSoft.OnlineBanking.Common.Resources.Resources.Get("DocumentTypePurchaseBondApplication", language);
				break;
			case "MO":
				result = ArmSoft.OnlineBanking.Common.Resources.Resources.Get("DocumentTypeMemorialOrder", language);
				break;
			case "DR":
				result = ArmSoft.OnlineBanking.Common.Resources.Resources.Get("DocumentTypeDepositPartialReductionApplication", language);
				break;
			case "DT":
				result = ArmSoft.OnlineBanking.Common.Resources.Resources.Get("DocumentTypeDepositTerminationApplication", language);
				break;
			case "SF":
				result = ArmSoft.OnlineBanking.Common.Resources.Resources.Get("DocumentTypeSignableFileDocument", language);
				break;
			case "AA":
				result = ArmSoft.OnlineBanking.Common.Resources.Resources.Get("DocumentTypeAmioLoanApplication", language);
				break;
			case "GP":
				result = ArmSoft.OnlineBanking.Common.Resources.Resources.Get("DocumentTypeGarbageCollectionTaxPayment", language);
				break;
			case "CD":
				result = ArmSoft.OnlineBanking.Common.Resources.Resources.Get("DocumentTypeCreditLineDisbursementApplication", language);
				break;
			case "QP":
				result = ArmSoft.OnlineBanking.Common.Resources.Resources.Get("DocumentTypeQrPayment", language);
				break;
			}
		}
		return result;
	}

	public static string GetDocumentGeneralType(string docType)
	{
		if (!string.IsNullOrWhiteSpace(docType))
		{
			return docType.Split('.')[0];
		}
		return null;
	}

	public static string GetDocumentSubType(string docType)
	{
		if (string.IsNullOrWhiteSpace(docType))
		{
			return null;
		}
		string[] array = docType.Split('.');
		if (array.Count() <= 1)
		{
			return null;
		}
		return array[1];
	}

	public static string RegistrationConfigurationField(string fieldName)
	{
		return fieldName switch
		{
			"RegistrationFieldAddress" => ArmSoft.OnlineBanking.Common.Resources.Resources.Address, 
			"RegistrationFieldPhoneNumber" => ArmSoft.OnlineBanking.Common.Resources.Resources.PhoneNumber, 
			"RegistrationFieldEmail" => ArmSoft.OnlineBanking.Common.Resources.Resources.Email, 
			"RegistrationFieldStatementDeliveryMethod" => ArmSoft.OnlineBanking.Common.Resources.Resources.StatementDeliveryMethod, 
			_ => string.Empty, 
		};
	}

	public static string RateSuggestionOperationType(RateSuggestionOperationType operationType)
	{
		return operationType switch
		{
			ArmSoft.OnlineBanking.Common.Enums.RateSuggestionOperationType.Created => ArmSoft.OnlineBanking.Common.Resources.Resources.RateSuggestionOperationCreated, 
			ArmSoft.OnlineBanking.Common.Enums.RateSuggestionOperationType.Assigned => ArmSoft.OnlineBanking.Common.Resources.Resources.RateSuggestionOperationAssigned, 
			ArmSoft.OnlineBanking.Common.Enums.RateSuggestionOperationType.Suggested => ArmSoft.OnlineBanking.Common.Resources.Resources.RateSuggestionOperationStarted, 
			ArmSoft.OnlineBanking.Common.Enums.RateSuggestionOperationType.AcceptedByClient => ArmSoft.OnlineBanking.Common.Resources.Resources.RateSuggestionOperationAcceptedByClient, 
			ArmSoft.OnlineBanking.Common.Enums.RateSuggestionOperationType.Signed => ArmSoft.OnlineBanking.Common.Resources.Resources.RateSuggestionOperationSigned, 
			ArmSoft.OnlineBanking.Common.Enums.RateSuggestionOperationType.ConfirmedByBank => ArmSoft.OnlineBanking.Common.Resources.Resources.RateSuggestionOperationConfirmedByBank, 
			ArmSoft.OnlineBanking.Common.Enums.RateSuggestionOperationType.RefusedByClient => ArmSoft.OnlineBanking.Common.Resources.Resources.RateSuggestionOperationRefusedByClient, 
			ArmSoft.OnlineBanking.Common.Enums.RateSuggestionOperationType.RefusedByOperator => ArmSoft.OnlineBanking.Common.Resources.Resources.RateSuggestionOperationRefusedByOperator, 
			ArmSoft.OnlineBanking.Common.Enums.RateSuggestionOperationType.RefusedByBank => ArmSoft.OnlineBanking.Common.Resources.Resources.RateSuggestionOperationRefusedByBank, 
			ArmSoft.OnlineBanking.Common.Enums.RateSuggestionOperationType.Expired => ArmSoft.OnlineBanking.Common.Resources.Resources.RateSuggestionOperationExpired, 
			_ => string.Empty, 
		};
	}

	public static string RateSuggestionState(RateSuggestionState state, bool isOperator)
	{
		switch (state)
		{
		case ArmSoft.OnlineBanking.Common.Enums.RateSuggestionState.New:
			return ArmSoft.OnlineBanking.Common.Resources.Resources.RateSuggestionStateNew;
		case ArmSoft.OnlineBanking.Common.Enums.RateSuggestionState.Assigned:
			return ArmSoft.OnlineBanking.Common.Resources.Resources.RateSuggestionStateAssigned;
		case ArmSoft.OnlineBanking.Common.Enums.RateSuggestionState.Started:
			return ArmSoft.OnlineBanking.Common.Resources.Resources.RateSuggestionStateStarted;
		case ArmSoft.OnlineBanking.Common.Enums.RateSuggestionState.AcceptedByClient:
			return ArmSoft.OnlineBanking.Common.Resources.Resources.RateSuggestionStateAcceptedByClient;
		case ArmSoft.OnlineBanking.Common.Enums.RateSuggestionState.Signed:
			return ArmSoft.OnlineBanking.Common.Resources.Resources.RateSuggestionStateSigned;
		case ArmSoft.OnlineBanking.Common.Enums.RateSuggestionState.ConfirmedByBank:
			return ArmSoft.OnlineBanking.Common.Resources.Resources.RateSuggestionStateConfirmedByBank;
		case ArmSoft.OnlineBanking.Common.Enums.RateSuggestionState.RefusedByClient:
			return ArmSoft.OnlineBanking.Common.Resources.Resources.RateSuggestionStateRefusedByClient;
		case ArmSoft.OnlineBanking.Common.Enums.RateSuggestionState.RefusedByOperator:
			return ArmSoft.OnlineBanking.Common.Resources.Resources.RateSuggestionStateRefusedByOperator;
		case ArmSoft.OnlineBanking.Common.Enums.RateSuggestionState.RefusedByBank:
			if (!isOperator)
			{
				return ArmSoft.OnlineBanking.Common.Resources.Resources.RateSuggestionStateRefusedByOperator;
			}
			return ArmSoft.OnlineBanking.Common.Resources.Resources.RateSuggestionStateRefusedByBank;
		case ArmSoft.OnlineBanking.Common.Enums.RateSuggestionState.Expired:
			return ArmSoft.OnlineBanking.Common.Resources.Resources.RateSuggestionStateExpired;
		default:
			return string.Empty;
		}
	}

	public static string RateSuggestionStateGroup(RateSuggestionStateGroup state)
	{
		return state switch
		{
			ArmSoft.OnlineBanking.Common.Enums.RateSuggestionStateGroup.New => ArmSoft.OnlineBanking.Common.Resources.Resources.RateSuggestionStateGroupNew, 
			ArmSoft.OnlineBanking.Common.Enums.RateSuggestionStateGroup.Assigned => ArmSoft.OnlineBanking.Common.Resources.Resources.RateSuggestionStateGroupAssigned, 
			ArmSoft.OnlineBanking.Common.Enums.RateSuggestionStateGroup.InProcess => ArmSoft.OnlineBanking.Common.Resources.Resources.RateSuggestionStateGroupInProcess, 
			ArmSoft.OnlineBanking.Common.Enums.RateSuggestionStateGroup.WaitingToBeSigned => ArmSoft.OnlineBanking.Common.Resources.Resources.RateSuggestionStateGroupWaitingToBeSigned, 
			ArmSoft.OnlineBanking.Common.Enums.RateSuggestionStateGroup.Completed => ArmSoft.OnlineBanking.Common.Resources.Resources.RateSuggestionStateGroupCompleted, 
			ArmSoft.OnlineBanking.Common.Enums.RateSuggestionStateGroup.AssignedToAnotherOperator => ArmSoft.OnlineBanking.Common.Resources.Resources.RateSuggestionStateGroupAssignedToAnotherOperator, 
			_ => string.Empty, 
		};
	}

	public static string RegistrationType(RegistrationType type)
	{
		return type switch
		{
			ArmSoft.OnlineBanking.Common.Enums.RegistrationType.NewClient => ArmSoft.OnlineBanking.Common.Resources.Resources.RegistrationTypeNewClient, 
			ArmSoft.OnlineBanking.Common.Enums.RegistrationType.ExistingClient => ArmSoft.OnlineBanking.Common.Resources.Resources.RegistrationTypeExistingClient, 
			_ => string.Empty, 
		};
	}

	public static DateTime GetDateTo(Period timePeriod)
	{
		DateTime result = DateTime.Today;
		switch (timePeriod)
		{
		case Period.Yesterday:
			result = DateTime.Today.AddDays(-1.0);
			break;
		case Period.PreviousMonth:
			result = DateTime.Today.AddDays(-DateTime.Today.Day);
			break;
		}
		return result;
	}

	public static DateTime GetDateFrom(Period timePeriod)
	{
		DateTime result = DateTime.Today;
		switch (timePeriod)
		{
		case Period.Today:
			result = DateTime.Today;
			break;
		case Period.Yesterday:
			result = DateTime.Today.AddDays(-1.0);
			break;
		case Period.Last3Days:
			result = DateTime.Today.AddDays(-2.0);
			break;
		case Period.Last7Days:
			result = DateTime.Today.AddDays(-6.0);
			break;
		case Period.Last31Days:
			result = DateTime.Today.AddDays(-30.0);
			break;
		case Period.Last3Months:
			result = DateTime.Today.AddMonths(-3);
			break;
		case Period.CurrentMonth:
			result = DateTime.Today.AddDays(1 - DateTime.Today.Day);
			break;
		case Period.PreviousMonth:
			result = DateTime.Today.AddDays(1 - DateTime.Today.Day).AddMonths(-1);
			break;
		case Period.CurrentYear:
			result = DateTime.Today.AddDays(1 - DateTime.Today.Day).AddMonths(1 - DateTime.Today.Month);
			break;
		}
		return result;
	}

	public static string CreditReceiveMethod(CreditReceiveMethod method)
	{
		return method switch
		{
			ArmSoft.OnlineBanking.Common.Enums.CreditReceiveMethod.Cash => ArmSoft.OnlineBanking.Common.Resources.Resources.CreditReceiveMethodCash, 
			ArmSoft.OnlineBanking.Common.Enums.CreditReceiveMethod.WithCard => ArmSoft.OnlineBanking.Common.Resources.Resources.CreditReceiveMethodWithCard, 
			ArmSoft.OnlineBanking.Common.Enums.CreditReceiveMethod.WithAccount => ArmSoft.OnlineBanking.Common.Resources.Resources.CreditReceiveMethodWithAccount, 
			ArmSoft.OnlineBanking.Common.Enums.CreditReceiveMethod.WithAttachedCard => ArmSoft.OnlineBanking.Common.Resources.Resources.CreditReceiveMethodWithAttachedCard, 
			_ => string.Empty, 
		};
	}

	public static string CardTypeGlobal(CardTypeGlobal type)
	{
		return type switch
		{
			ArmSoft.OnlineBanking.Common.Enums.CardTypeGlobal.Unknown => string.Empty, 
			ArmSoft.OnlineBanking.Common.Enums.CardTypeGlobal.Visa => ArmSoft.OnlineBanking.Common.Resources.Resources.CardTypeVisa, 
			ArmSoft.OnlineBanking.Common.Enums.CardTypeGlobal.MasterCard => ArmSoft.OnlineBanking.Common.Resources.Resources.CardTypeMasterCard, 
			ArmSoft.OnlineBanking.Common.Enums.CardTypeGlobal.ArCa => ArmSoft.OnlineBanking.Common.Resources.Resources.CardTypeArCa, 
			ArmSoft.OnlineBanking.Common.Enums.CardTypeGlobal.Amex => ArmSoft.OnlineBanking.Common.Resources.Resources.CardTypeAmex, 
			ArmSoft.OnlineBanking.Common.Enums.CardTypeGlobal.DinersClub => ArmSoft.OnlineBanking.Common.Resources.Resources.CardTypeDinersClub, 
			ArmSoft.OnlineBanking.Common.Enums.CardTypeGlobal.UnionPay => ArmSoft.OnlineBanking.Common.Resources.Resources.CardTypeUnionPay, 
			ArmSoft.OnlineBanking.Common.Enums.CardTypeGlobal.Jcb => ArmSoft.OnlineBanking.Common.Resources.Resources.CardTypeJcb, 
			ArmSoft.OnlineBanking.Common.Enums.CardTypeGlobal.DiscoverCard => ArmSoft.OnlineBanking.Common.Resources.Resources.CardTypeDiscoverCard, 
			ArmSoft.OnlineBanking.Common.Enums.CardTypeGlobal.Maestro => ArmSoft.OnlineBanking.Common.Resources.Resources.CardTypeMaestro, 
			ArmSoft.OnlineBanking.Common.Enums.CardTypeGlobal.Mir => ArmSoft.OnlineBanking.Common.Resources.Resources.CardTypeMir, 
			_ => string.Empty, 
		};
	}

	public static string IdentificationDocumentType(string type)
	{
		return type switch
		{
			"01" => ArmSoft.OnlineBanking.Common.Resources.Resources.IdentificationDocumentTypeRANonBiometricPassport, 
			"02" => ArmSoft.OnlineBanking.Common.Resources.Resources.IdentificationDocumentTypeMilitaryCard, 
			"03" => ArmSoft.OnlineBanking.Common.Resources.Resources.IdentificationDocumentTypeTemporaryIdentityDocument, 
			"04" => ArmSoft.OnlineBanking.Common.Resources.Resources.IdentificationDocumentTypeBirthCertificate, 
			"05" => ArmSoft.OnlineBanking.Common.Resources.Resources.IdentificationDocumentTypeRefugeeCertificate, 
			"06" => ArmSoft.OnlineBanking.Common.Resources.Resources.IdentificationDocumentTypeTravelDocument, 
			"07" => ArmSoft.OnlineBanking.Common.Resources.Resources.IdentificationDocumentTypeForeignPassport, 
			"08" => ArmSoft.OnlineBanking.Common.Resources.Resources.IdentificationDocumentTypeInternationallyRecognizedIdentityDocuments, 
			"09" => ArmSoft.OnlineBanking.Common.Resources.Resources.IdentificationDocumentTypeRASpecialPassport, 
			"10" => ArmSoft.OnlineBanking.Common.Resources.Resources.IdentificationDocumentTypeResidenceCard, 
			"11" => ArmSoft.OnlineBanking.Common.Resources.Resources.IdentificationDocumentTypeOther, 
			"12" => ArmSoft.OnlineBanking.Common.Resources.Resources.IdentificationDocumentTypeRAIDCard, 
			"13" => ArmSoft.OnlineBanking.Common.Resources.Resources.IdentificationDocumentTypeRABiometricPassport, 
			"14" => ArmSoft.OnlineBanking.Common.Resources.Resources.IdentificationDocumentTypeRAReturnCertificate, 
			"15" => ArmSoft.OnlineBanking.Common.Resources.Resources.IdentificationDocumentTypeTemporaryResidenceCard, 
			"16" => ArmSoft.OnlineBanking.Common.Resources.Resources.IdentificationDocumentTypeTemporaryAsylumCertificate, 
			_ => string.Empty, 
		};
	}

	public static string TransactionDirection(TransactionDirection transactionDirection)
	{
		if (transactionDirection == ArmSoft.OnlineBanking.Common.Enums.TransactionDirection.Credit)
		{
			return ArmSoft.OnlineBanking.Common.Resources.Resources.TransactionDirection_Credit;
		}
		return ArmSoft.OnlineBanking.Common.Resources.Resources.TransactionDirection_Debit;
	}

	public static string ChargeTypes(string chargeType, string bankDisplayName)
	{
		string result = string.Empty;
		switch (chargeType)
		{
		case "OUR":
			result = string.Format(ArmSoft.OnlineBanking.Common.Resources.Resources.ChargesOUR, bankDisplayName);
			break;
		case "BEN":
			result = ArmSoft.OnlineBanking.Common.Resources.Resources.ChargesBEN;
			break;
		case "SHA":
			result = ArmSoft.OnlineBanking.Common.Resources.Resources.ChargesSHA;
			break;
		case "OURG":
			result = ArmSoft.OnlineBanking.Common.Resources.Resources.ChargesOURG;
			break;
		}
		return result;
	}

	public static string PayerOptions(string payerOption)
	{
		string result = string.Empty;
		if (payerOption == null || payerOption.Length != 0)
		{
			if (payerOption == "F")
			{
				result = ArmSoft.OnlineBanking.Common.Resources.Resources.Field_OptionF;
			}
		}
		else
		{
			result = ArmSoft.OnlineBanking.Common.Resources.Resources.Field_DefaultOption;
		}
		return result;
	}

	public static string NameAndAddressCodes(string nameAndAddressCode)
	{
		string arg = string.Empty;
		switch (nameAndAddressCode)
		{
		case "1":
			arg = ArmSoft.OnlineBanking.Common.Resources.Resources.NameAndAddressCodesName;
			break;
		case "2":
			arg = ArmSoft.OnlineBanking.Common.Resources.Resources.NameAndAddressCodesAddress;
			break;
		case "3":
			arg = ArmSoft.OnlineBanking.Common.Resources.Resources.NameAndAddressCodesCountryTown;
			break;
		case "4":
			arg = ArmSoft.OnlineBanking.Common.Resources.Resources.NameAndAddressCodesBirthDate;
			break;
		case "5":
			arg = ArmSoft.OnlineBanking.Common.Resources.Resources.NameAndAddressCodesBirthPlace;
			break;
		case "6":
			arg = ArmSoft.OnlineBanking.Common.Resources.Resources.NameAndAddressCodesCustomerIdentificationNumber;
			break;
		case "7":
			arg = ArmSoft.OnlineBanking.Common.Resources.Resources.NameAndAddressCodesNationalIdentityNumber;
			break;
		case "8":
			arg = ArmSoft.OnlineBanking.Common.Resources.Resources.NameAndAddressCodesAdditionalInformation;
			break;
		}
		return $"({nameAndAddressCode}) {arg}";
	}

	public static string EvocaMobileCreditType(EvocaMobileLoanType creditType)
	{
		return creditType switch
		{
			EvocaMobileLoanType.Action => ArmSoft.OnlineBanking.Common.Resources.Resources.EvocaMobileLoanType_Action, 
			EvocaMobileLoanType.Overdraft => ArmSoft.OnlineBanking.Common.Resources.Resources.EvocaMobileLoanType_Overdraft, 
			_ => string.Empty, 
		};
	}

	public static string PushAuthenticationStatus(DevicePushAuthenticationStatus pushStatus)
	{
		return pushStatus switch
		{
			DevicePushAuthenticationStatus.IsEnabled => ArmSoft.OnlineBanking.Common.Resources.Resources.PushStatusIsEnabled, 
			DevicePushAuthenticationStatus.IsDisabled => ArmSoft.OnlineBanking.Common.Resources.Resources.PushStatusIsDisabled, 
			DevicePushAuthenticationStatus.IsInactive => ArmSoft.OnlineBanking.Common.Resources.Resources.PushStatusIsInactive, 
			DevicePushAuthenticationStatus.IsTurnedOff => ArmSoft.OnlineBanking.Common.Resources.Resources.PushStatusIsTurnedOff, 
			_ => string.Empty, 
		};
	}

	public static string AccountTransactionOperationType(string transactionType)
	{
		return transactionType switch
		{
			"TRF" => ArmSoft.OnlineBanking.Common.Resources.Resources.AccountTransactionTypeTransfer, 
			"MSC" => ArmSoft.OnlineBanking.Common.Resources.Resources.AccountTransactionTypeMiscellaneous, 
			"QIT" => ArmSoft.OnlineBanking.Common.Resources.Resources.AccountTransactionTypeRepayment, 
			"RVL" => ArmSoft.OnlineBanking.Common.Resources.Resources.AccountTransactionTypeRevaluation, 
			"PRC" => ArmSoft.OnlineBanking.Common.Resources.Resources.AccountTransactionTypeInterestCalculation, 
			"INT" => ArmSoft.OnlineBanking.Common.Resources.Resources.AccountTransactionTypeInterestCapitalization, 
			"CEX" => ArmSoft.OnlineBanking.Common.Resources.Resources.AccountTransactionTypeCurrencyExchange, 
			"RST" => ArmSoft.OnlineBanking.Common.Resources.Resources.AccountTransactionTypeProvision, 
			"FEE" => ArmSoft.OnlineBanking.Common.Resources.Resources.AccountTransactionTypeCommission, 
			"FEX" => ArmSoft.OnlineBanking.Common.Resources.Resources.AccountTransactionTypeCommissionWithCurrencyExchange, 
			"PEN" => ArmSoft.OnlineBanking.Common.Resources.Resources.AccountTransactionTypePenalty, 
			"PEX" => ArmSoft.OnlineBanking.Common.Resources.Resources.AccountTransactionTypePenaltyWithCurrencyExchange, 
			"PER" => ArmSoft.OnlineBanking.Common.Resources.Resources.AccountTransactionTypeInterest, 
			"PRX" => ArmSoft.OnlineBanking.Common.Resources.Resources.AccountTransactionTypeInterestWithCurrencyExchange, 
			"OVD" => ArmSoft.OnlineBanking.Common.Resources.Resources.AccountTransactionTypeOverdraftRepayment, 
			"OVO" => ArmSoft.OnlineBanking.Common.Resources.Resources.AccountTransactionTypeOverdraftRepaymentWithOtherAccount, 
			"OVX" => ArmSoft.OnlineBanking.Common.Resources.Resources.AccountTransactionTypeOverdraftRepaymentWithCurrencyExchange, 
			"ADJ" => ArmSoft.OnlineBanking.Common.Resources.Resources.AccountTransactionTypeBalanceAdjustment, 
			_ => string.Empty, 
		};
	}

	public static string OpenCreditFileType(OpenCreditFileType type)
	{
		return type switch
		{
			ArmSoft.OnlineBanking.Common.Enums.OpenCreditFileType.Contract => ArmSoft.OnlineBanking.Common.Resources.Resources.OpenCreditFileTypeContract, 
			ArmSoft.OnlineBanking.Common.Enums.OpenCreditFileType.Arbitrage => ArmSoft.OnlineBanking.Common.Resources.Resources.OpenCreditFileTypeArbitrage, 
			ArmSoft.OnlineBanking.Common.Enums.OpenCreditFileType.IndividualSheet => ArmSoft.OnlineBanking.Common.Resources.Resources.OpenCreditFileTypeIndividualSheet, 
			_ => string.Empty, 
		};
	}

	public static string ServiceFeeRateDivisorCategoryDescription(ServiceFeeRateDivisorCategory category, decimal? serviceFeeRate = null)
	{
		if (serviceFeeRate.HasValue && serviceFeeRate.Value != 0m)
		{
			return category switch
			{
				ServiceFeeRateDivisorCategory.Daily => ArmSoft.OnlineBanking.Common.Resources.Resources.Get("ServiceFeeRateDivisorCategoryDescription_Daily"), 
				ServiceFeeRateDivisorCategory.Weekly => ArmSoft.OnlineBanking.Common.Resources.Resources.Get("ServiceFeeRateDivisorCategoryDescription_Weekly"), 
				ServiceFeeRateDivisorCategory.Monthly => ArmSoft.OnlineBanking.Common.Resources.Resources.Get("ServiceFeeRateDivisorCategoryDescription_Monthly"), 
				ServiceFeeRateDivisorCategory.Yearly => ArmSoft.OnlineBanking.Common.Resources.Resources.Get("ServiceFeeRateDivisorCategoryDescription_Yearly"), 
				_ => string.Empty, 
			};
		}
		return string.Empty;
	}

	public static string RepaymentTypes(RepaymentType type)
	{
		return type switch
		{
			RepaymentType.ScheduledPayment => ArmSoft.OnlineBanking.Common.Resources.Resources.LoanScheduledPayment, 
			RepaymentType.PrincipalPrepayment => ArmSoft.OnlineBanking.Common.Resources.Resources.PrepaymentOfPrincipal, 
			_ => string.Empty, 
		};
	}

	public static string BondIssuanceType(BondIssuanceType type)
	{
		return type switch
		{
			ArmSoft.OnlineBanking.Common.Enums.BondIssuanceType.InterestBearing => ArmSoft.OnlineBanking.Common.Resources.Resources.BondIssuanceTypeInterestBearing, 
			ArmSoft.OnlineBanking.Common.Enums.BondIssuanceType.NonInterestBearing => ArmSoft.OnlineBanking.Common.Resources.Resources.BondIssuanceTypeNonInterestBearing, 
			_ => string.Empty, 
		};
	}

	public static string StatementSendingType(StatementSendingType type)
	{
		return type switch
		{
			ArmSoft.OnlineBanking.Common.Enums.StatementSendingType.Email => ArmSoft.OnlineBanking.Common.Resources.Resources.Email, 
			ArmSoft.OnlineBanking.Common.Enums.StatementSendingType.Mail => ArmSoft.OnlineBanking.Common.Resources.Resources.Mail, 
			ArmSoft.OnlineBanking.Common.Enums.StatementSendingType.Manually => ArmSoft.OnlineBanking.Common.Resources.Resources.Manually, 
			_ => string.Empty, 
		};
	}

	public static string PropertyTaxOwnerLegalStatuses(string type)
	{
		if (!(type == "physical"))
		{
			if (type == "legal")
			{
				return ArmSoft.OnlineBanking.Common.Resources.Resources.ClientStatusLegal;
			}
			return string.Empty;
		}
		return ArmSoft.OnlineBanking.Common.Resources.Resources.ClientStatusNatural;
	}

	public static string PropertyTypes(string type)
	{
		if (!(type == "real_estate"))
		{
			if (type == "vehicles")
			{
				return ArmSoft.OnlineBanking.Common.Resources.Resources.TaxPropertyTypeTransport;
			}
			return string.Empty;
		}
		return ArmSoft.OnlineBanking.Common.Resources.Resources.TaxPropertyTypeRealty;
	}

	public static string PropertyTaxDebtPeriodType(string period)
	{
		if (!(period == "until_now"))
		{
			if (period == "annual")
			{
				return ArmSoft.OnlineBanking.Common.Resources.Resources.PropertyTaxDebtPeriodIncludingCurrentYear;
			}
			return string.Empty;
		}
		return ArmSoft.OnlineBanking.Common.Resources.Resources.PropertyTaxDebtPeriodIncludingCurrentMonth;
	}

	public static string AdvancedPaymentMethods(AdvancedPaymentMethod paymentMethod)
	{
		return paymentMethod switch
		{
			AdvancedPaymentMethod.ToAccount => ArmSoft.OnlineBanking.Common.Resources.Resources.AdvancedPaymentMethodToAccount, 
			AdvancedPaymentMethod.NoAccount => ArmSoft.OnlineBanking.Common.Resources.Resources.AdvancedPaymentMethodNoAccount, 
			_ => string.Empty, 
		};
	}

	public static string AdvancedLoanPaymentTypes(string paymentType)
	{
		return paymentType switch
		{
			"F" => ArmSoft.OnlineBanking.Common.Resources.Resources.AdvancedPaymentCreditPaymentPenalty, 
			"L" => ArmSoft.OnlineBanking.Common.Resources.Resources.AdvancedPaymentCreditPaymentPrincipal, 
			"P" => ArmSoft.OnlineBanking.Common.Resources.Resources.AdvancedPaymentCreditPaymentInterest, 
			"M" => ArmSoft.OnlineBanking.Common.Resources.Resources.AdvancedPaymentCreditPaymentCombined, 
			_ => string.Empty, 
		};
	}

	public static string PaymentPublicityTypes(string publicityType)
	{
		if (!(publicityType == "0"))
		{
			if (publicityType == "1")
			{
				return ArmSoft.OnlineBanking.Common.Resources.Resources.AnonymousPayment;
			}
			return string.Empty;
		}
		return ArmSoft.OnlineBanking.Common.Resources.Resources.PublicPayment;
	}

	public static string CardTransactionInfoReceivingOption(CardReceiveTransactionInfoOption option)
	{
		return option switch
		{
			CardReceiveTransactionInfoOption.NotToReceive => ArmSoft.OnlineBanking.Common.Resources.Resources.ReceiveTransactionInfoOff, 
			CardReceiveTransactionInfoOption.ViaPush => ArmSoft.OnlineBanking.Common.Resources.Resources.ReceiveTransactionInfoViaPushNotification, 
			CardReceiveTransactionInfoOption.ViaSms => ArmSoft.OnlineBanking.Common.Resources.Resources.ReceiveTransactionInfoViaSms, 
			_ => string.Empty, 
		};
	}

	public static string CardSmsLanguages(CardSmsLanguage cardSmsLanguage)
	{
		return cardSmsLanguage switch
		{
			CardSmsLanguage.Armenian => ArmSoft.OnlineBanking.Common.Resources.Resources.SmsLanguageArmenian, 
			CardSmsLanguage.ArmenianLatinized => ArmSoft.OnlineBanking.Common.Resources.Resources.SmsLanguageArmenianLatinized, 
			CardSmsLanguage.English => ArmSoft.OnlineBanking.Common.Resources.Resources.SmsLanguageEnglish, 
			_ => string.Empty, 
		};
	}

	public static string CardSmsFormats(CardSmsFormat format)
	{
		return format switch
		{
			CardSmsFormat.WithoutRemainder => ArmSoft.OnlineBanking.Common.Resources.Resources.SmsFormatWithoutBalance, 
			CardSmsFormat.WithRemainder => ArmSoft.OnlineBanking.Common.Resources.Resources.SmsFormatWithBalance, 
			_ => string.Empty, 
		};
	}

	public static string UserSignatureMode(UserSignatureMode userSignatureMode)
	{
		return userSignatureMode switch
		{
			ArmSoft.OnlineBanking.Common.Enums.UserSignatureMode.Fingerprint => ArmSoft.OnlineBanking.Common.Resources.Resources.UserSignatureModeFingerprint, 
			ArmSoft.OnlineBanking.Common.Enums.UserSignatureMode.Pin => ArmSoft.OnlineBanking.Common.Resources.Resources.UserSignatureModePin, 
			ArmSoft.OnlineBanking.Common.Enums.UserSignatureMode.FaceId => ArmSoft.OnlineBanking.Common.Resources.Resources.UserSignatureModeFaceId, 
			_ => string.Empty, 
		};
	}

	public static string DataUpdateType(ClientDataUpdateType userSignatureMode)
	{
		return userSignatureMode switch
		{
			ClientDataUpdateType.Disabled => ArmSoft.OnlineBanking.Common.Resources.Resources.DataUpdateTypeDisabled, 
			ClientDataUpdateType.Remind => ArmSoft.OnlineBanking.Common.Resources.Resources.DataUpdateTypeRemind, 
			ClientDataUpdateType.Required => ArmSoft.OnlineBanking.Common.Resources.Resources.DataUpdateTypeRequired, 
			_ => string.Empty, 
		};
	}

	public static string SignableFileState(SignableFileState signableFileState, bool isOperator)
	{
		switch (signableFileState)
		{
		case ArmSoft.OnlineBanking.Common.Enums.SignableFileState.New:
			return ArmSoft.OnlineBanking.Common.Resources.Resources.SignableFileStateNew;
		case ArmSoft.OnlineBanking.Common.Enums.SignableFileState.SentToClient:
			if (!isOperator)
			{
				return ArmSoft.OnlineBanking.Common.Resources.Resources.SignableFileStateSentToClientForClient;
			}
			return ArmSoft.OnlineBanking.Common.Resources.Resources.SignableFileStateSentToClientForOperator;
		case ArmSoft.OnlineBanking.Common.Enums.SignableFileState.SentToOperator:
			return ArmSoft.OnlineBanking.Common.Resources.Resources.SignableFileStateSentToOperator;
		case ArmSoft.OnlineBanking.Common.Enums.SignableFileState.Uploaded:
			return ArmSoft.OnlineBanking.Common.Resources.Resources.SignableFileStateApproved;
		case ArmSoft.OnlineBanking.Common.Enums.SignableFileState.RejectedByOperator:
			return ArmSoft.OnlineBanking.Common.Resources.Resources.SignableFileStateRejectedByOperator;
		case ArmSoft.OnlineBanking.Common.Enums.SignableFileState.RejectedByClient:
			return ArmSoft.OnlineBanking.Common.Resources.Resources.SignableFileStateRejectedByClient;
		case ArmSoft.OnlineBanking.Common.Enums.SignableFileState.Expired:
			return ArmSoft.OnlineBanking.Common.Resources.Resources.SignableFileStateExpired;
		case ArmSoft.OnlineBanking.Common.Enums.SignableFileState.Approved:
			if (!isOperator)
			{
				return ArmSoft.OnlineBanking.Common.Resources.Resources.SignableFileStateApproved;
			}
			return ArmSoft.OnlineBanking.Common.Resources.Resources.SignableFileStateProblematic;
		default:
			return string.Empty;
		}
	}

	public static string UserResetOptions(BankQuestion question)
	{
		return question switch
		{
			BankQuestion.Account => ArmSoft.OnlineBanking.Common.Resources.Resources.BankQuestionAccount, 
			BankQuestion.Card => ArmSoft.OnlineBanking.Common.Resources.Resources.BankQuestionCard, 
			BankQuestion.CreditAgreement => ArmSoft.OnlineBanking.Common.Resources.Resources.BankQuestionCreditAgreement, 
			BankQuestion.DepositAgreement => ArmSoft.OnlineBanking.Common.Resources.Resources.BankQuestionDepositAgreement, 
			BankQuestion.SocialCard => ArmSoft.OnlineBanking.Common.Resources.Resources.BankQuestionSocialCard, 
			BankQuestion.TaxCode => ArmSoft.OnlineBanking.Common.Resources.Resources.BankQuestionTaxCode, 
			_ => string.Empty, 
		};
	}

	public static LocalizableLabel UserResetOptionsLocalized(BankQuestion question)
	{
		return question switch
		{
			BankQuestion.Account => LocalizableLabel.FromResource(typeof(ArmSoft.OnlineBanking.Common.Resources.Resources), "BankQuestionAccount"), 
			BankQuestion.Card => LocalizableLabel.FromResource(typeof(ArmSoft.OnlineBanking.Common.Resources.Resources), "BankQuestionCard"), 
			BankQuestion.CreditAgreement => LocalizableLabel.FromResource(typeof(ArmSoft.OnlineBanking.Common.Resources.Resources), "BankQuestionCreditAgreement"), 
			BankQuestion.DepositAgreement => LocalizableLabel.FromResource(typeof(ArmSoft.OnlineBanking.Common.Resources.Resources), "BankQuestionDepositAgreement"), 
			BankQuestion.SocialCard => LocalizableLabel.FromResource(typeof(ArmSoft.OnlineBanking.Common.Resources.Resources), "BankQuestionSocialCard"), 
			BankQuestion.TaxCode => LocalizableLabel.FromResource(typeof(ArmSoft.OnlineBanking.Common.Resources.Resources), "BankQuestionTaxCode"), 
			_ => new LocalizableLabel(string.Empty, string.Empty, string.Empty), 
		};
	}

	public static string DocState(DocumentState documentState, object model, bool isOperator = false, bool includeInnerState = false)
	{
		string text = null;
		string text2;
		switch (documentState)
		{
		case DocumentState.Editable:
			text2 = ArmSoft.OnlineBanking.Common.Resources.Resources.DocumentStateEditable;
			break;
		case DocumentState.PartiallySigned:
			text2 = ArmSoft.OnlineBanking.Common.Resources.Resources.DocumentStatePartiallySigned;
			break;
		case DocumentState.ReadyToBeSent:
			text2 = (isOperator ? ArmSoft.OnlineBanking.Common.Resources.Resources.DocumentStateReadyToBeSent : ArmSoft.OnlineBanking.Common.Resources.Resources.DocumentStateAccepted);
			break;
		case DocumentState.Sent:
			text2 = (isOperator ? ArmSoft.OnlineBanking.Common.Resources.Resources.DocumentStateSent : ArmSoft.OnlineBanking.Common.Resources.Resources.DocumentStateAccepted);
			text = (isOperator ? null : ArmSoft.OnlineBanking.Common.Resources.Resources.DocumentStateSent);
			break;
		case DocumentState.InProcessOnline:
			text2 = (isOperator ? ArmSoft.OnlineBanking.Common.Resources.Resources.DocumentStateInProcessOnline : ArmSoft.OnlineBanking.Common.Resources.Resources.DocumentStateAccepted);
			text = ArmSoft.OnlineBanking.Common.Resources.Resources.DocumentStateInProcessOnline;
			break;
		case DocumentState.InProcessBank:
			text2 = ((!(model is IDocument { IsClientSignatureRequired: not false, IsEditableByPartner: false })) ? ArmSoft.OnlineBanking.Common.Resources.Resources.DocumentStateInProcessBank : ArmSoft.OnlineBanking.Common.Resources.Resources.DocumentStateClientSignature);
			break;
		case DocumentState.Accepted:
			text2 = ArmSoft.OnlineBanking.Common.Resources.Resources.DocumentStateAccepted;
			break;
		case DocumentState.Confirmed:
			text2 = ArmSoft.OnlineBanking.Common.Resources.Resources.DocumentStateConfirmed;
			break;
		case DocumentState.Refused:
			text2 = ArmSoft.OnlineBanking.Common.Resources.Resources.DocumentStateRefused;
			break;
		case DocumentState.Canceled:
			text2 = ArmSoft.OnlineBanking.Common.Resources.Resources.DocumentStateCanceled;
			break;
		case DocumentState.Template:
			text2 = ArmSoft.OnlineBanking.Common.Resources.Resources.DocumentStateTemplate;
			break;
		case DocumentState.Ready:
			text2 = ArmSoft.OnlineBanking.Common.Resources.Resources.DocumentStateReady;
			break;
		default:
			text2 = ArmSoft.OnlineBanking.Common.Resources.Resources.DocumentStateNotSet;
			break;
		}
		if (includeInnerState && !string.IsNullOrEmpty(text))
		{
			text2 = text2 + " (" + text + ")";
		}
		return text2;
	}

	public static string InterestPayPeriod(InterestPayPeriod value)
	{
		return value switch
		{
			ArmSoft.OnlineBanking.Common.Enums.InterestPayPeriod.Monthly => ArmSoft.OnlineBanking.Common.Resources.Resources.InterestPayPeriodMonthly, 
			ArmSoft.OnlineBanking.Common.Enums.InterestPayPeriod.Quarterly => ArmSoft.OnlineBanking.Common.Resources.Resources.InterestPayPeriodQuarterly, 
			ArmSoft.OnlineBanking.Common.Enums.InterestPayPeriod.Yearly => ArmSoft.OnlineBanking.Common.Resources.Resources.InterestPayPeriodYearly, 
			ArmSoft.OnlineBanking.Common.Enums.InterestPayPeriod.HalfYearly => ArmSoft.OnlineBanking.Common.Resources.Resources.InterestPayPeriodHalfYearly, 
			ArmSoft.OnlineBanking.Common.Enums.InterestPayPeriod.StartOfTerm => ArmSoft.OnlineBanking.Common.Resources.Resources.InterestPayPeriodStartOfTerm, 
			ArmSoft.OnlineBanking.Common.Enums.InterestPayPeriod.EndOfTerm => ArmSoft.OnlineBanking.Common.Resources.Resources.InterestPayPeriodEndOfTerm, 
			_ => string.Empty, 
		};
	}

	public static string DecodePeriodicPaymentMethod(string value)
	{
		return value switch
		{
			"01" => ArmSoft.OnlineBanking.Common.Resources.Resources.DebitAccountPositiveBalance, 
			"02" => ArmSoft.OnlineBanking.Common.Resources.Resources.DebitAccountNegativeBalance, 
			"03" => ArmSoft.OnlineBanking.Common.Resources.Resources.CreditAccountPositiveBalance, 
			"04" => ArmSoft.OnlineBanking.Common.Resources.Resources.CreditAccountNegativeBalance, 
			"05" => ArmSoft.OnlineBanking.Common.Resources.Resources.MinimumOfPositiveBalances, 
			"06" => ArmSoft.OnlineBanking.Common.Resources.Resources.CreditTurnoverOfDebitAccount, 
			"07" => ArmSoft.OnlineBanking.Common.Resources.Resources.CreditTurnoverOfDebitAccountMinusDebitTurnover, 
			"08" => ArmSoft.OnlineBanking.Common.Resources.Resources.DebitTurnoverOfDebitAccount, 
			"09" => ArmSoft.OnlineBanking.Common.Resources.Resources.DebitTurnoverOfDebitAccountMinusCreditTurnover, 
			"10" => ArmSoft.OnlineBanking.Common.Resources.Resources.FixedAmount, 
			"11" => ArmSoft.OnlineBanking.Common.Resources.Resources.DebtAmount, 
			_ => string.Empty, 
		};
	}

	public static string Periodicity(string periodicity)
	{
		if (string.IsNullOrEmpty(periodicity))
		{
			return string.Empty;
		}
		string text = periodicity.Split(new char[1] { '/' }, StringSplitOptions.None)[0].Trim();
		string text2 = periodicity.Split(new char[1] { '/' }, StringSplitOptions.None)[1].Trim();
		if (text == "0" && text2 != "0")
		{
			return string.Format(ArmSoft.OnlineBanking.Common.Resources.Resources.PeriodicityDays, text2);
		}
		if (text != "0" && text2 == "0")
		{
			return string.Format(ArmSoft.OnlineBanking.Common.Resources.Resources.PeriodicityMonths, text);
		}
		if (text != "0" && text2 != "0")
		{
			return string.Format(ArmSoft.OnlineBanking.Common.Resources.Resources.PeriodicityMonthsDays, text, text2);
		}
		return string.Empty;
	}

	public static string DocumentSignQueueStatus(DocumentSignQueueStatus status)
	{
		return status switch
		{
			ArmSoft.OnlineBanking.Common.Enums.DocumentSignQueueStatus.Created => ArmSoft.OnlineBanking.Common.Resources.Resources.DocumentSignQueueStatusCreated, 
			ArmSoft.OnlineBanking.Common.Enums.DocumentSignQueueStatus.Started => ArmSoft.OnlineBanking.Common.Resources.Resources.DocumentSignQueueStatusStarted, 
			ArmSoft.OnlineBanking.Common.Enums.DocumentSignQueueStatus.Succeeded => ArmSoft.OnlineBanking.Common.Resources.Resources.DocumentSignQueueStatusSucceeded, 
			ArmSoft.OnlineBanking.Common.Enums.DocumentSignQueueStatus.Failed => ArmSoft.OnlineBanking.Common.Resources.Resources.DocumentSignQueueStatusFailed, 
			_ => string.Empty, 
		};
	}

	public static string DocumentSignQueueRowStatus(DocumentSignQueueRowStatus status)
	{
		return status switch
		{
			ArmSoft.OnlineBanking.Common.Enums.DocumentSignQueueRowStatus.New => ArmSoft.OnlineBanking.Common.Resources.Resources.DocumentSignQueueRowStatusNew, 
			ArmSoft.OnlineBanking.Common.Enums.DocumentSignQueueRowStatus.Started => ArmSoft.OnlineBanking.Common.Resources.Resources.DocumentSignQueueRowStatusStarted, 
			ArmSoft.OnlineBanking.Common.Enums.DocumentSignQueueRowStatus.Signed => ArmSoft.OnlineBanking.Common.Resources.Resources.DocumentSignQueueRowStatusSigned, 
			ArmSoft.OnlineBanking.Common.Enums.DocumentSignQueueRowStatus.SignFailed => ArmSoft.OnlineBanking.Common.Resources.Resources.DocumentSignQueueRowStatusSignFailed, 
			ArmSoft.OnlineBanking.Common.Enums.DocumentSignQueueRowStatus.Ignored => ArmSoft.OnlineBanking.Common.Resources.Resources.DocumentSignQueueRowStatusIgnored, 
			_ => string.Empty, 
		};
	}

	public static string TaxPaymentType(TaxDebtType type)
	{
		return type switch
		{
			TaxDebtType.Debt => ArmSoft.OnlineBanking.Common.Resources.Resources.TaxPaymentTypeDebt, 
			TaxDebtType.Fine => ArmSoft.OnlineBanking.Common.Resources.Resources.TaxPaymentTypeFine, 
			_ => string.Empty, 
		};
	}

	public static string WorkerIntanceStatus(WorkerInstanceStatus status)
	{
		return status switch
		{
			WorkerInstanceStatus.Running => ArmSoft.OnlineBanking.Common.Resources.Resources.WorkerInstanceStatusRunning, 
			WorkerInstanceStatus.StoppedUnexpectedly => ArmSoft.OnlineBanking.Common.Resources.Resources.WorkerInstanceStatusStoppedUnexpectedly, 
			WorkerInstanceStatus.StoppedGracefully => ArmSoft.OnlineBanking.Common.Resources.Resources.WorkerInstanceStatusStoppedGracefully, 
			WorkerInstanceStatus.Unknown => ArmSoft.OnlineBanking.Common.Resources.Resources.WorkerInstanceStatusUnknown, 
			_ => string.Empty, 
		};
	}

	public static string CreditOverpaymentType(CreditOverpaymentType status)
	{
		return status switch
		{
			ArmSoft.OnlineBanking.Common.Enums.CreditOverpaymentType.LastAmounts => ArmSoft.OnlineBanking.Common.Resources.Resources.CreditOverpaymentTypeLastAmounts, 
			ArmSoft.OnlineBanking.Common.Enums.CreditOverpaymentType.UpcomingAmounts => ArmSoft.OnlineBanking.Common.Resources.Resources.CreditOverpaymentTypeUpcomingAmounts, 
			ArmSoft.OnlineBanking.Common.Enums.CreditOverpaymentType.UpcomingAmountThenLast => ArmSoft.OnlineBanking.Common.Resources.Resources.CreditOverpaymentTypeUpcomingAmountThenLast, 
			ArmSoft.OnlineBanking.Common.Enums.CreditOverpaymentType.WithScheduleRedistribution => ArmSoft.OnlineBanking.Common.Resources.Resources.CreditOverpaymentTypeWithScheduleRedistribution, 
			_ => string.Empty, 
		};
	}

	public static string CreditOverpaymentTypeDescription(CreditOverpaymentType status)
	{
		return status switch
		{
			ArmSoft.OnlineBanking.Common.Enums.CreditOverpaymentType.LastAmounts => ArmSoft.OnlineBanking.Common.Resources.Resources.CreditOverpaymentTypeLastAmountsDescription, 
			ArmSoft.OnlineBanking.Common.Enums.CreditOverpaymentType.UpcomingAmounts => ArmSoft.OnlineBanking.Common.Resources.Resources.CreditOverpaymentTypeUpcomingAmountsDescription, 
			ArmSoft.OnlineBanking.Common.Enums.CreditOverpaymentType.UpcomingAmountThenLast => ArmSoft.OnlineBanking.Common.Resources.Resources.CreditOverpaymentTypeUpcomingAmountThenLastDescription, 
			ArmSoft.OnlineBanking.Common.Enums.CreditOverpaymentType.WithScheduleRedistribution => ArmSoft.OnlineBanking.Common.Resources.Resources.CreditOverpaymentTypeWithScheduleRedistributionDescription, 
			_ => string.Empty, 
		};
	}

	public static string DecodeLinkType(LinkType type)
	{
		return type switch
		{
			LinkType.Url => ArmSoft.OnlineBanking.Common.Resources.Resources.LinkUrl, 
			LinkType.DocumentType => ArmSoft.OnlineBanking.Common.Resources.Resources.LinkDocumentType, 
			LinkType.Module => ArmSoft.OnlineBanking.Common.Resources.Resources.LinkModule, 
			_ => string.Empty, 
		};
	}

	public static MerchantCategoryCode? GetMerchantCategoryCodeEnum(string code)
	{
		if (string.IsNullOrEmpty(code) || !int.TryParse(code, out var result))
		{
			return null;
		}
		if (result >= 3000 && result <= 3350)
		{
			return ArmSoft.OnlineBanking.Common.Enums.MerchantCategoryCode.Airlines;
		}
		if (result >= 3351 && result <= 3441)
		{
			return ArmSoft.OnlineBanking.Common.Enums.MerchantCategoryCode.CarRental;
		}
		if ((result >= 3501 && result <= 3839) || (result >= 7011 && result <= 7012))
		{
			return ArmSoft.OnlineBanking.Common.Enums.MerchantCategoryCode.HotelsAndLodging;
		}
		return result switch
		{
			4121 => (MerchantCategoryCode?)ArmSoft.OnlineBanking.Common.Enums.MerchantCategoryCode.TaxisAndLimousines, 
			5812 => ArmSoft.OnlineBanking.Common.Enums.MerchantCategoryCode.Restaurants, 
			5814 => ArmSoft.OnlineBanking.Common.Enums.MerchantCategoryCode.FastFoodRestaurants, 
			5411 => ArmSoft.OnlineBanking.Common.Enums.MerchantCategoryCode.Supermarkets, 
			5542 => ArmSoft.OnlineBanking.Common.Enums.MerchantCategoryCode.AutomatedFuelDispensers, 
			5541 => ArmSoft.OnlineBanking.Common.Enums.MerchantCategoryCode.ServiceStations, 
			8011 => ArmSoft.OnlineBanking.Common.Enums.MerchantCategoryCode.Doctors, 
			8021 => ArmSoft.OnlineBanking.Common.Enums.MerchantCategoryCode.Dentists, 
			8062 => ArmSoft.OnlineBanking.Common.Enums.MerchantCategoryCode.Hospitals, 
			5499 => ArmSoft.OnlineBanking.Common.Enums.MerchantCategoryCode.FoodStore, 
			5651 => ArmSoft.OnlineBanking.Common.Enums.MerchantCategoryCode.ClothingStore, 
			4722 => ArmSoft.OnlineBanking.Common.Enums.MerchantCategoryCode.TravelAgency, 
			5211 => ArmSoft.OnlineBanking.Common.Enums.MerchantCategoryCode.BuildingMaterialsStore, 
			5399 => ArmSoft.OnlineBanking.Common.Enums.MerchantCategoryCode.GeneralStore, 
			5691 => ArmSoft.OnlineBanking.Common.Enums.MerchantCategoryCode.ApparelAndAccessoriesStore, 
			5944 => ArmSoft.OnlineBanking.Common.Enums.MerchantCategoryCode.JewelryStore, 
			7399 => ArmSoft.OnlineBanking.Common.Enums.MerchantCategoryCode.Service, 
			_ => null, 
		};
	}

	public static string MerchantCategoryCode(string code)
	{
		MerchantCategoryCode? merchantCategoryCodeEnum = GetMerchantCategoryCodeEnum(code);
		if (!merchantCategoryCodeEnum.HasValue)
		{
			return ArmSoft.OnlineBanking.Common.Resources.Resources.MerchantCategoryCodeOther;
		}
		return merchantCategoryCodeEnum.Value switch
		{
			ArmSoft.OnlineBanking.Common.Enums.MerchantCategoryCode.Airlines => ArmSoft.OnlineBanking.Common.Resources.Resources.MerchantCategoryCodeAirlines, 
			ArmSoft.OnlineBanking.Common.Enums.MerchantCategoryCode.CarRental => ArmSoft.OnlineBanking.Common.Resources.Resources.MerchantCategoryCodeCarRental, 
			ArmSoft.OnlineBanking.Common.Enums.MerchantCategoryCode.HotelsAndLodging => ArmSoft.OnlineBanking.Common.Resources.Resources.MerchantCategoryCodeLodgingHotelsMotelsResorts, 
			ArmSoft.OnlineBanking.Common.Enums.MerchantCategoryCode.TaxisAndLimousines => ArmSoft.OnlineBanking.Common.Resources.Resources.MerchantCategoryCodeTaxisAndLimousines, 
			ArmSoft.OnlineBanking.Common.Enums.MerchantCategoryCode.Restaurants => ArmSoft.OnlineBanking.Common.Resources.Resources.MerchantCategoryCodeEatingPlacesAndRestaurants, 
			ArmSoft.OnlineBanking.Common.Enums.MerchantCategoryCode.FastFoodRestaurants => ArmSoft.OnlineBanking.Common.Resources.Resources.MerchantCategoryCodeFastFoodRestaurants, 
			ArmSoft.OnlineBanking.Common.Enums.MerchantCategoryCode.Supermarkets => ArmSoft.OnlineBanking.Common.Resources.Resources.MerchantCategoryCodeGroceryStoresAndSupermarkets, 
			ArmSoft.OnlineBanking.Common.Enums.MerchantCategoryCode.AutomatedFuelDispensers => ArmSoft.OnlineBanking.Common.Resources.Resources.MerchantCategoryCodeAutomatedFuelDispensers, 
			ArmSoft.OnlineBanking.Common.Enums.MerchantCategoryCode.ServiceStations => ArmSoft.OnlineBanking.Common.Resources.Resources.MerchantCategoryCodeServiceStations, 
			ArmSoft.OnlineBanking.Common.Enums.MerchantCategoryCode.Doctors => ArmSoft.OnlineBanking.Common.Resources.Resources.MerchantCategoryCodeDoctors, 
			ArmSoft.OnlineBanking.Common.Enums.MerchantCategoryCode.Dentists => ArmSoft.OnlineBanking.Common.Resources.Resources.MerchantCategoryCodeDentists, 
			ArmSoft.OnlineBanking.Common.Enums.MerchantCategoryCode.Hospitals => ArmSoft.OnlineBanking.Common.Resources.Resources.MerchantCategoryCodeHospitals, 
			ArmSoft.OnlineBanking.Common.Enums.MerchantCategoryCode.FoodStore => ArmSoft.OnlineBanking.Common.Resources.Resources.MerchantCategoryCodeFoodStore, 
			ArmSoft.OnlineBanking.Common.Enums.MerchantCategoryCode.ClothingStore => ArmSoft.OnlineBanking.Common.Resources.Resources.MerchantCategoryCodeClothingStore, 
			ArmSoft.OnlineBanking.Common.Enums.MerchantCategoryCode.TravelAgency => ArmSoft.OnlineBanking.Common.Resources.Resources.MerchantCategoryCodeTravelAgency, 
			ArmSoft.OnlineBanking.Common.Enums.MerchantCategoryCode.BuildingMaterialsStore => ArmSoft.OnlineBanking.Common.Resources.Resources.MerchantCategoryCodeBuildingMaterialsStore, 
			ArmSoft.OnlineBanking.Common.Enums.MerchantCategoryCode.GeneralStore => ArmSoft.OnlineBanking.Common.Resources.Resources.MerchantCategoryCodeGeneralStore, 
			ArmSoft.OnlineBanking.Common.Enums.MerchantCategoryCode.ApparelAndAccessoriesStore => ArmSoft.OnlineBanking.Common.Resources.Resources.MerchantCategoryCodeApparelAndAccessoriesStore, 
			ArmSoft.OnlineBanking.Common.Enums.MerchantCategoryCode.JewelryStore => ArmSoft.OnlineBanking.Common.Resources.Resources.MerchantCategoryCodeJewelryStore, 
			ArmSoft.OnlineBanking.Common.Enums.MerchantCategoryCode.Service => ArmSoft.OnlineBanking.Common.Resources.Resources.MerchantCategoryCodeService, 
			_ => ArmSoft.OnlineBanking.Common.Resources.Resources.MerchantCategoryCodeOther, 
		};
	}
}
