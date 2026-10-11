using ArmSoft.SME.Common.Models;

namespace ArmSoft.SME.Client.Data;

public class CurrencyDataProvider : DataProvider<CurrencyDataProvider.DataRow, CurrencyDataProvider.Param>
{
	public class DataRow : ExtendableRow, IRowWithKey, IMatchedUpdateKey
	{
		public string Curr { get; set; }

		public string Caption { get; set; }

		public string UnitName { get; set; }

		public string CentName { get; set; }

		public object GetKey()
		{
			return Curr;
		}

		public string GetCode()
		{
			return Curr;
		}

		public string GetName()
		{
			return Caption;
		}

		public bool Matched(object key)
		{
			return key.ToString() == Curr;
		}
	}

	public class Param : ExtendableParameter
	{
		public string Currency { get; set; }

		public bool WithOutNCC { get; set; }
	}

	public override string Name => "Currency";

	public CurrencyDataProvider(ApiClient client)
		: base(client)
	{
	}
}
