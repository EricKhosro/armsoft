using System;
using ArmSoft.SME.Client;
using ArmSoft.SME.Client.Data;
using ArmSoft.SME.Common.Models;

namespace ArmSoft.SME.Trade.Client.Data.Directories;

public class PricesDataProvider : DataProvider<PricesDataProvider.DataRow, PricesDataProvider.Param>
{
	public class DataRow : ExtendableRow, IRowWithKey, IMatchedUpdateKey
	{
		public class PricesRowKey
		{
			public int MTID { get; set; }

			public string PriceType { get; set; }

			public string QntUnit { get; set; }

			public DateOnly Date { get; set; }
		}

		public int MTID { get; set; }

		public string MTCode { get; set; }

		public string Caption { get; set; }

		public string PriceType { get; set; }

		public string PriceTypeCaption { get; set; }

		public string QntUnit { get; set; }

		public string UnitCaption { get; set; }

		public DateOnly Date { get; set; }

		public string CurrCode { get; set; }

		public decimal Price { get; set; }

		public string Comment { get; set; }

		public string Group { get; set; }

		public string GroupCaption { get; set; }

		public object GetKey()
		{
			return new PricesRowKey
			{
				MTID = MTID,
				PriceType = PriceType,
				QntUnit = QntUnit,
				Date = Date
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

		public bool Matched(object key)
		{
			PricesRowKey pricesRowKey = (PricesRowKey)key;
			if (pricesRowKey.MTID == MTID && pricesRowKey.PriceType == PriceType && pricesRowKey.QntUnit == QntUnit)
			{
				return pricesRowKey.Date == Date;
			}
			return false;
		}
	}

	public class Param
	{
		public string MTCode { get; set; }

		public string MTGroup { get; set; }

		public DateOnly? StartDate { get; set; }

		public DateOnly? EndDate { get; set; }

		public string PriceType { get; set; }
	}

	public override string Name => "Prices";

	public PricesDataProvider(TradeClient client)
		: base((ApiClient)client)
	{
	}
}
