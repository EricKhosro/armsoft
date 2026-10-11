using System;

namespace ArmSoft.OnlineBanking.Common.Constants;

public static class ActionName
{
	public const string View = "View";

	public const string Print = "Print";

	public const string History = "History";

	public const string Edit = "Edit";

	public const string Copy = "Copy";

	public const string Sign = "Sign";

	public const string Delete = "Delete";

	public const string CancelDocument = "CancelDocument";

	public const string CancelOnlineTransfer = "CancelOnlineTransfer";

	public const string SavePayee = "SavePayee";

	public const string SwiftGpiTracker = "SwiftGpiTracker";

	public const string ViewSignatures = "ViewSignatures";

	public const string ShareAction = "ShareAction";

	public const string Extend = "Extend";

	public const string Reject = "Reject";

	public const string RefuseDocument = "RefuseDocument";

	public const string SendToEdit = "SendToEdit";

	public const string Details = "Details";

	public const string Transactions = "Transactions";

	public const string Schedule = "Schedule";

	public const string CreditRepayment = "CreditRepayment";

	public const string FactoringSubcontracts = "FactoringSubcontracts";

	public const string BankRequisites = "BankRequisites";

	public const string CreateCashWithdrawal = "CreateCashWithdrawal";

	public const string QrReceiveMoney = "QrReceiveMoney";

	public const string OnlineTransactions = "OnlineTransactions";

	public const string BlockCard = "BlockCard";

	public const string UnblockCard = "UnblockCard";

	public const string CardSettings = "UpdateCardSettings";

	public const string CardLimits = "UpdateCardLimits";

	public const string ChangePin = "ChangePin";

	[Obsolete("Use UpdateNote instead. Remove when mobile min version is 8.12.0.")]
	public const string EditNote = "UpdateAccountNote";

	public const string UpdateNote = "UpdateNote";

	public const string MerchantTransactions = "MerchantTransactions";

	public const string CBStatement = "CBStatement";

	public const string StatementGenerationRequest = "StatementGenerationRequest";

	public const string CancelStatementGenerationRequest = "CancelStatementGenerationRequest";

	public const string CBCardStatement = "CBCardStatement";

	public const string Statement = "Statement";

	public const string DepositReduction = "DepositReduction";

	public const string DepositTermination = "DepositTermination";

	public const string MakePayment = "MakePayment";

	public const string ActivateCard = "ActivateCard";

	public const string ActivateCardAndSetPin = "ActivateCardAndSetPin";

	public const string DepositReplenishment = "DepositReplenishment";

	public const string StateRevenueCommitteeStatement = "StateRevenueCommitteeStatement";

	public const string CreateDocument = "CreateDocument";

	public const string Enable = "Enable";

	public const string Disable = "Disable";
}
