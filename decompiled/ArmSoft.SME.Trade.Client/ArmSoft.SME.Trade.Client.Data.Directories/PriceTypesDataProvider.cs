using ArmSoft.SME.Client;
using ArmSoft.SME.Client.Data;
using ArmSoft.SME.Common.Models;

namespace ArmSoft.SME.Trade.Client.Data.Directories;

public class PriceTypesDataProvider : DataProvider<PriceTypesDataProvider.DataRow, PriceTypesDataProvider.Param>
{
	public class DataRow : ExtendableRow, IRowWithKey, IMatchedUpdateKey
	{
		public string Code { get; set; }

		public string Caption { get; set; }

		public string CurrCode { get; set; }

		public string Round { get; set; }

		public bool VAT { get; set; }

		public object GetKey()
		{
			return Code;
		}

		public string GetCode()
		{
			return Code;
		}

		public string GetName()
		{
			return Caption;
		}

		public bool Matched(object key)
		{
			return (string)key == Code;
		}
	}

	public class Param
	{
		public string Code { get; set; }

		public string CurrCode { get; set; } = string.Empty;

		public bool ShowPrimeCostRows { get; set; }

		public bool ShowPurchasePriceRow { get; set; }

		public bool ShowSalePriceRow { get; set; }

		public bool ShowRetailAvailabilityPriceRow { get; set; }

		public bool ShowDiscountPriceRow { get; set; }
	}

	public override string Name => "PriceTypes";

	public PriceTypesDataProvider(TradeClient client)
		: base((ApiClient)client)
	{
	}
}
