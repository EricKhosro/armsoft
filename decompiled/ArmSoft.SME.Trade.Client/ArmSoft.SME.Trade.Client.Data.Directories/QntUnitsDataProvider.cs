using ArmSoft.SME.Client;
using ArmSoft.SME.Client.Data;
using ArmSoft.SME.Common.Models;

namespace ArmSoft.SME.Trade.Client.Data.Directories;

public class QntUnitsDataProvider : DataProvider<QntUnitsDataProvider.DataRow, QntUnitsDataProvider.Param>
{
	public class DataRow : ExtendableRow, IRowWithKey, IMatchedUpdateKey
	{
		public string Code { get; set; }

		public string Caption { get; set; }

		public string Brief { get; set; }

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
			return "";
		}

		public bool Matched(object key)
		{
			return Code == key.ToString();
		}
	}

	public class Param
	{
		public string Unit { get; set; } = "";
	}

	public override string Name => "QntUnits";

	public QntUnitsDataProvider(TradeClient client)
		: base((ApiClient)client)
	{
	}
}
