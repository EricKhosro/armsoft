using System;
using System.Linq;
using System.Reflection;
using ArmSoft.OnlineBanking.Common.Attributes;
using ArmSoft.OnlineBanking.Common.Enums;
using ArmSoft.OnlineBanking.Common.Utility;

namespace ArmSoft.OnlineBanking.Common.Constants;

public static class DocumentTypes
{
	[AccountPayment]
	[OldDocument]
	[NewDocument]
	public const string LocalPayment = "PO";

	[AccountPayment(IsDetailsVisible = false)]
	[OldDocument]
	[NewDocument]
	public const string GoalAccountCloseApplication = "GA";

	[AccountPayment(ReceiverIdMask = "", ReceiverNameMask = "")]
	[OldDocument]
	[NewDocument]
	public const string MultiplePayment = "MP";

	[AccountPayment]
	[OldDocument]
	[NewDocument]
	public const string ForeignPayment = "PF";

	[AccountPayment]
	[OldDocument]
	[NewDocument]
	public const string ForeignPaymentRus = "PR";

	[AccountPayment(IsOnlyAmd = true, IsDetailsVisible = false)]
	[OldDocument]
	[NewDocument]
	public const string CommunalPayment = "CP";

	[AccountPayment(IsOnlyAmd = true, IsDetailsVisible = false)]
	[OldDocument]
	[NewDocument]
	public const string MultipleCommunal = "MC";

	[AccountPayment]
	[OldDocument]
	[NewDocument(ModelName = "LocalPayment")]
	public const string CurrencyConversion = "CC";

	[AccountPayment(IsDetailsVisible = false)]
	[OldDocument]
	[NewDocument]
	public const string CreditRepayment = "CR";

	[AttachedCardPayment(IsAvailableForBank = false, IsAvailableForCreditOrganization = true, IsDetailsVisible = false)]
	[OldDocument]
	[NewDocument]
	public const string CreditRepaymentWithAttachedCard = "RA";

	[AccountPayment(IsDetailsVisible = false)]
	[OldDocument]
	[NewDocument]
	public const string DepositAttraction = "DA";

	[CancellationRequest]
	[OldDocument]
	[NewDocument]
	public const string CancelRequestApplication = "RC";

	[AccountPayment(ReceiverIdMask = "")]
	[OldDocument]
	[NewDocument]
	public const string MemorialOrder = "MO";

	[Application(IsCustomDocument = true, IsDetailsVisible = false)]
	[OldDocument]
	public const string EvocaGetLoanApplication = "EL";

	[Application(IsCustomDocument = true, IsDetailsVisible = false)]
	[OldDocument]
	public const string EvocaGetMobileLoanApplication = "EM";

	[Application(IsCustomDocument = true, IsDetailsVisible = false)]
	[OldDocument]
	public const string AmioLoanApplication = "AA";

	[Application(IsCustomDocument = true, IsDetailsVisible = false)]
	[OldDocument]
	public const string EvocaGiftCardApplication = "EG";

	[Application(IsCustomDocument = true, IsDetailsVisible = false)]
	[OldDocument]
	public const string EvocaDigitalGiftCardApplication = "ED";

	[Application(IsCustomDocument = true, IsDetailsVisible = false)]
	[OldDocument]
	public const string EvocaGetBondApplication = "EB";

	[Application(IsDetailsVisible = false)]
	[OldDocument]
	[NewDocument]
	public const string OpenAccountApplication = "OA";

	[Application]
	[NewDocument]
	public const string CloseAccountApplication = "AC";

	[Application(IsDetailsVisible = false)]
	[OldDocument]
	[NewDocument]
	public const string OpenDepositApplication = "OD";

	[Application(IsDetailsVisible = false)]
	[OldDocument]
	[NewDocument]
	public const string PurchaseBondApplication = "PB";

	[Application(IsAvailableForBank = true, IsAvailableForCreditOrganization = true, IsDetailsVisible = false)]
	[OldDocument]
	[NewDocument]
	public const string OpenCreditApplication = "OC";

	[Application(IsDetailsVisible = false)]
	[OldDocument]
	[NewDocument]
	public const string OpenDigitalCardApplication = "DC";

	public const string ArmSoftApplication = "AS";

	[AccountPayment]
	[OldDocument]
	[NewDocument]
	public const string PassportPayment = "PP";

	[Application]
	[OldDocument]
	[NewDocument]
	public const string CashWithdrawalApplication = "CW";

	[AccountPayment(IsOnlyAmd = true, IsDetailsVisible = false)]
	[OldDocument]
	[NewDocument]
	public const string TaxPayment = "TP";

	[AccountPayment(IsOnlyAmd = true, IsDetailsVisible = false)]
	[OldDocument]
	[NewDocument]
	public const string GarbageCollectionPayment = "GC";

	[AccountPayment(IsOnlyAmd = true, IsDetailsVisible = false)]
	[OldDocument]
	[NewDocument]
	public const string PropertyTaxPayment = "PT";

	[AccountPayment(IsOnlyAmd = true, IsDetailsVisible = false)]
	[OldDocument]
	[NewDocument]
	public const string VehicleTaxPayment = "VT";

	[AccountPayment(IsOnlyAmd = true, IsDetailsVisible = false)]
	[OldDocument]
	[NewDocument]
	public const string RealEstateTaxPayment = "RT";

	[AccountPayment(IsOnlyAmd = true, IsDetailsVisible = false)]
	[NewDocument]
	[OldDocument]
	public const string GarbageCollectionTaxPayment = "GP";

	[AccountPayment(IsOnlyAmd = true)]
	[OldDocument]
	[NewDocument]
	public const string BudgetaryPayment = "BP";

	[AccountPayment(IsOnlyAmd = true, IsDetailsVisible = false)]
	[OldDocument]
	[NewDocument]
	public const string RoadPolicePayment = "RP";

	[Application(IsAvailableForCreditOrganization = true)]
	[OldDocument]
	[NewDocument]
	public const string CustomizedApplication = "CA";

	[AccountPayment]
	[OldDocument]
	public const string PaymentsPackage = "DP";

	[AccountPayment(IsOnlyAmd = true)]
	[OldDocument]
	[NewDocument]
	public const string CreditRepaymentViaPartnerPayment = "AP";

	[AccountPayment(IsOnlyAmd = true)]
	[OldDocument]
	[NewDocument]
	public const string FriendCreditRepayment = "FC";

	[AccountPayment(ReceiverIdMask = "***", ReceiverNameMask = "***", IsMaskDependsOnDocumentState = true)]
	[OldDocument]
	[NewDocument]
	public const string ContactTransfer = "CT";

	[AccountPayment(IsOnlyAmd = true, ReceiverNameMask = "***", IsMaskDependsOnDocumentState = true)]
	[OldDocument]
	[NewDocument]
	public const string ArCaPayTransfer = "AT";

	[AccountPayment]
	[OldDocument]
	[NewDocument]
	public const string AdvancedLocalPayment = "AL";

	[AccountPayment(ReceiverNameMask = "***", IsMaskDependsOnDocumentState = true)]
	[OldDocument]
	[NewDocument]
	public const string CardTransfer = "TC";

	[AttachedCardPayment(IsDetailsVisible = false)]
	[OldDocument]
	[NewDocument]
	public const string AccountReplenishment = "AR";

	[AccountPayment(IsOnlyAmd = true, ReceiverIdMask = "", ReceiverNameMask = "", IsDetailsVisible = false)]
	[OldDocument]
	[NewDocument]
	public const string CashOutByPhone = "CM";

	[AccountPayment(IsOnlyAmd = true, IsDetailsVisible = false)]
	[OldDocument]
	[NewDocument]
	public const string CompulsoryEnforcementPayment = "CE";

	[AccountPayment(IsOnlyAmd = true, IsDetailsVisible = false)]
	[OldDocument]
	[NewDocument]
	public const string AswaPayment = "AW";

	[Application]
	[NewDocument]
	public const string DepositPartialReductionApplication = "DR";

	[Application]
	[NewDocument]
	public const string DepositTerminationApplication = "DT";

	[DocumentType(IsDetailsVisible = false)]
	[NewDocument]
	public const string SignableFileDocument = "SF";

	[Application]
	[NewDocument]
	public const string CreditLineDisbursementApplication = "CD";

	[AccountPayment(IsOnlyAmd = true, IsDetailsVisible = false)]
	[NewDocument]
	public const string QrPayment = "QP";

	public static DocumentCategories GetCategory(string documentType)
	{
		return (typeof(DocumentTypes).GetFields(BindingFlags.Static | BindingFlags.Public).FirstOrDefault((FieldInfo x) => (string)x.GetValue(null) == documentType)?.GetCustomAttribute<DocumentTypeBaseAttribute>())?.Category ?? DocumentCategories.NotSet;
	}

	public static bool IsOnlyAmd(string documentType)
	{
		return (typeof(DocumentTypes).GetFields(BindingFlags.Static | BindingFlags.Public).FirstOrDefault((FieldInfo x) => (string)x.GetValue(null) == documentType)?.GetCustomAttribute<AccountPaymentAttribute>())?.IsOnlyAmd ?? false;
	}

	public static string GetName(string documentType)
	{
		FieldInfo? fieldInfo = typeof(DocumentTypes).GetFields(BindingFlags.Static | BindingFlags.Public).FirstOrDefault((FieldInfo x) => (string)x.GetValue(null) == Decode.GetDocumentGeneralType(documentType));
		if (fieldInfo == null)
		{
			throw new NotSupportedException();
		}
		return fieldInfo.Name;
	}

	public static string MaskReceiverId(string documentType, DocumentState state)
	{
		DocumentTypeBaseAttribute documentTypeBaseAttribute = typeof(DocumentTypes).GetFields(BindingFlags.Static | BindingFlags.Public).FirstOrDefault((FieldInfo x) => (string)x.GetValue(null) == documentType)?.GetCustomAttribute<DocumentTypeBaseAttribute>();
		if (documentTypeBaseAttribute == null || (documentTypeBaseAttribute.IsMaskDependsOnDocumentState && state == DocumentState.Confirmed))
		{
			return null;
		}
		return documentTypeBaseAttribute.ReceiverIdMask;
	}

	public static string MaskReceiverName(string documentType, DocumentState state, string name)
	{
		if ((documentType == "TC" || documentType == "AT") && name.Contains("*"))
		{
			return null;
		}
		DocumentTypeBaseAttribute documentTypeBaseAttribute = typeof(DocumentTypes).GetFields(BindingFlags.Static | BindingFlags.Public).FirstOrDefault((FieldInfo x) => (string)x.GetValue(null) == documentType)?.GetCustomAttribute<DocumentTypeBaseAttribute>();
		if (documentTypeBaseAttribute == null || (documentTypeBaseAttribute.IsMaskDependsOnDocumentState && state == DocumentState.Confirmed))
		{
			return null;
		}
		return documentTypeBaseAttribute.ReceiverNameMask;
	}
}
