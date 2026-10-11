namespace ArmSoft.OnlineBanking.Common.Enums;

public enum TransactionType : byte
{
	NotSet,
	TransferToMyAccount,
	TransferToAnotherBeneficiary,
	InternationalPayment,
	Payment,
	CurrencyConversion,
	TransferOutsideOfBank,
	CashOut,
	CreditRepayment,
	DepositReplenishment,
	QrPayment,
	TransferToCard,
	IntrabankTransfer,
	CashWithdrawalApplication,
	TransferToCardOutsideOfBank,
	ArCaPayTransfer,
	AccountReplenishment
}
