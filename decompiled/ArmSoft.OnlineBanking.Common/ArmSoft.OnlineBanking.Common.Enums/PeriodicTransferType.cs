namespace ArmSoft.OnlineBanking.Common.Enums;

public enum PeriodicTransferType : byte
{
	NotSet,
	MemorialOrder,
	CurrencyConversion,
	LocalPayment,
	BudgetaryPayment,
	ForeignPayment,
	DepositReplenishment,
	SecuritiesFreeDeliveryOrder,
	PaymentWithData,
	TransferWithoutAccount
}
