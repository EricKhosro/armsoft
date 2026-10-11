namespace ArmSoft.SME.Common.CustomTypes;

public class ExchangeRate
{
	public decimal Base { get; set; } = 1m;

	public decimal Rate { get; set; } = 1m;

	public ExchangeRate(decimal exchangeBase, decimal exchangeRate)
	{
		Base = exchangeBase;
		Rate = exchangeRate;
	}

	public ExchangeRate()
	{
	}
}
