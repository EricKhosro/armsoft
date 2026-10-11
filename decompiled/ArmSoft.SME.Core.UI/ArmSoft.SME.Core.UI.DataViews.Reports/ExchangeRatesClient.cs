using System;
using System.Collections.Generic;
using System.ServiceModel;
using ArmSoft.SME.Client.Data;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Extensions;
using CBA.ExchangeRates;

namespace ArmSoft.SME.Core.UI.DataViews.Reports;

public class ExchangeRatesClient
{
	public static List<RatesOfExchangeDataProvider.DataRow> GetExchangeRates(DateOnly startDate, DateOnly endDate, List<string> currencies)
	{
		List<RatesOfExchangeDataProvider.DataRow> list = new List<RatesOfExchangeDataProvider.DataRow>();
		using GateSoapClient gateSoapClient = Settings.CBAExchangeRatesClient;
		foreach (string currency in currencies)
		{
			DateOnly? dateOnly = startDate;
			do
			{
				ExchangeRates exchangeRatesByDateByISOResult;
				try
				{
					exchangeRatesByDateByISOResult = gateSoapClient.ExchangeRatesByDateByISO(new ExchangeRatesByDateByISORequest
					{
						date = dateOnly.Value.ToDateTime(),
						ISO = currency
					}).ExchangeRatesByDateByISOResult;
				}
				catch (TimeoutException)
				{
					throw new ASException(string.Format(Resources.ServiceCallTimeOut, "ExchangeRates"));
				}
				catch (CommunicationException)
				{
					throw new ASException(string.Format(Resources.CommunicationFail, "ExchangeRates"));
				}
				catch (Exception)
				{
					throw new ASException(string.Format(Resources.ServiceCallUnexpectedError, "ExchangeRates"));
				}
				if (exchangeRatesByDateByISOResult != null && exchangeRatesByDateByISOResult.CurrentDate.ToDateOnly() >= dateOnly.Value && exchangeRatesByDateByISOResult.Rates.Length != 0)
				{
					list.Add(new RatesOfExchangeDataProvider.DataRow
					{
						Date = exchangeRatesByDateByISOResult.CurrentDate.ToDateOnly(),
						Curr = exchangeRatesByDateByISOResult.Rates[0].ISO,
						Rate = Convert.ToDecimal(exchangeRatesByDateByISOResult.Rates[0].Rate),
						Base = Convert.ToDecimal(exchangeRatesByDateByISOResult.Rates[0].Amount)
					});
				}
				dateOnly = exchangeRatesByDateByISOResult.NextAvailableDate.ToDateOnly();
			}
			while (dateOnly.HasValue && dateOnly.Value < endDate);
		}
		return list;
	}
}
