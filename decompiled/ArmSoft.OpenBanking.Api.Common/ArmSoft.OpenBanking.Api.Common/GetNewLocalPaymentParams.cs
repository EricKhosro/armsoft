namespace ArmSoft.OpenBanking.Api.Common;

public class GetNewLocalPaymentParams : GetNewSignableObjectParams
{
	public bool IsForCurrencyConversion { get; set; }
}
