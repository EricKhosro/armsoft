namespace ArmSoft.SME.Trade.AddIn;

public sealed class Enums
{
	public enum SummaTypes : short
	{
		Unknown,
		NCCSummWithoutVAT,
		NCCSummWithVAT,
		CurrSummWithVAT
	}

	public enum SaleSumTypes : short
	{
		Unknown,
		SaleSum,
		SaleCostSum,
		SaleDiscountSum
	}
}
