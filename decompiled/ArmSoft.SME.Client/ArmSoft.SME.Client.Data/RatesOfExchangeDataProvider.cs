using System;
using ArmSoft.SME.Common.Models;

namespace ArmSoft.SME.Client.Data;

public class RatesOfExchangeDataProvider : DataProvider<RatesOfExchangeDataProvider.DataRow, RatesOfExchangeDataProvider.Param>
{
	public class DataRow : ExtendableRow, IRowWithKey, IMatchedUpdateKey
	{
		public class RatesOfExchangeRowKey
		{
			public DateOnly Date { get; set; }

			public string Currency { get; set; }
		}

		public DateOnly Date { get; set; }

		public string Curr { get; set; }

		public decimal Rate { get; set; }

		public decimal Base { get; set; }

		public bool Matched(object key)
		{
			RatesOfExchangeRowKey ratesOfExchangeRowKey = (RatesOfExchangeRowKey)key;
			if (ratesOfExchangeRowKey.Currency == Curr)
			{
				return ratesOfExchangeRowKey.Date == Date;
			}
			return false;
		}

		public object GetKey()
		{
			return new RatesOfExchangeRowKey
			{
				Date = Date,
				Currency = Curr
			};
		}

		public string GetCode()
		{
			return "";
		}

		public string GetName()
		{
			return "";
		}
	}

	public class Param : ExtendableParameter
	{
		public string Currencies { get; set; }

		public DateOnly? StartDate { get; set; }

		public DateOnly? EndDate { get; set; }
	}

	public override string Name => "RatesOfExchange";

	public RatesOfExchangeDataProvider(ApiClient client)
		: base(client)
	{
	}
}
