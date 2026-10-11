using ArmSoft.SME.Client;
using ArmSoft.SME.Client.Data;
using ArmSoft.SME.Common.Models;

namespace ArmSoft.SME.Trade.Client.Data.Directories;

public class StoragesDataProvider : DataProvider<StoragesDataProvider.DataRow, StoragesDataProvider.Param>
{
	public class DataRow : ExtendableRow, IRowWithKey, IMatchedUpdateKey
	{
		public string Code { get; set; }

		public string Caption { get; set; }

		public string Response { get; set; }

		public string Address { get; set; }

		public bool Retail { get; set; }

		public bool IsDefault { get; set; }

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

		public DataRow()
		{
			Code = string.Empty;
			Caption = string.Empty;
			Response = string.Empty;
			Address = string.Empty;
			Retail = false;
			IsDefault = false;
		}

		public bool Matched(object key)
		{
			return (string)key == Code;
		}
	}

	public class Param : ExtendableParameter
	{
		public bool? IsRetail { get; set; }
	}

	public override string Name => "Storages";

	public StoragesDataProvider(TradeClient client)
		: base((ApiClient)client)
	{
	}
}
