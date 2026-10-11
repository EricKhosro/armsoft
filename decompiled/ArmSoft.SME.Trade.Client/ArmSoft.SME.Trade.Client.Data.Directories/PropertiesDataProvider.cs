using ArmSoft.SME.Client;
using ArmSoft.SME.Client.Data;
using ArmSoft.SME.Common.Models;

namespace ArmSoft.SME.Trade.Client.Data.Directories;

public class PropertiesDataProvider : DataProvider<PropertiesDataProvider.DataRow, PropertiesDataProvider.Param>
{
	public class DataRow : ExtendableRow, IRowWithKey, IMatchedUpdateKey
	{
		public string NextProperty { get; set; }

		public string Code { get; set; }

		public string Caption { get; set; }

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
			return Code == (string)key;
		}
	}

	public class Param
	{
	}

	public override string Name => "Properties";

	public PropertiesDataProvider(TradeClient client)
		: base((ApiClient)client)
	{
	}
}
