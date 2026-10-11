using System.Collections.Generic;
using ArmSoft.InternetBank.Web.API.Common.Models;

namespace ArmSoft.InternetBank.APIClient;

public class RateManager : BaseManager
{
	internal RateManager(Client client)
		: base(client)
	{
	}

	public ClientRate GetClientRate(string currency1, string currency2)
	{
		Dictionary<string, object> queryParams = new Dictionary<string, object>
		{
			["currency1"] = currency1,
			["currency2"] = currency2
		};
		Rate rate = client.Get<Rate>("exchangeRates/bankRate", requireAuthorization: true, null, queryParams);
		Rate rate2 = client.Get<Rate>("exchangeRates/bankRateByDeviation", requireAuthorization: true, null, queryParams);
		ClientRate clientRate = new ClientRate
		{
			OfferedRateValue = rate2.AppliedRateDeviationsByAmount[0].Value.RateValue,
			RateFor = rate2.RateFor,
			Currency1 = rate2.Currency1,
			Currency2 = rate2.Currency2,
			RateLabel = rate.RateLabel,
			AmountCurrency = rate2.AmountCurrency,
			BankRateValue = rate.RateValue
		};
		if (clientRate.OfferedRateValue != clientRate.BankRateValue)
		{
			clientRate.RateDeviationMessage = rate2.AppliedRateDeviationsByAmount[0].Value.RateLabel;
		}
		return clientRate;
	}
}
