using ArmSoft.SME.Client;
using ArmSoft.SME.Client.Data;
using ArmSoft.SME.Common.Models;

namespace ArmSoft.SME.Trade.Client.Data.Directories;

public class PropertyListDataProvider : DataProvider<PropertyListDataProvider.DataRow, PropertyListDataProvider.Param>
{
	public class DataRow : ExtendableRow, IRowWithKey, IMatchedUpdateKey
	{
		public class MTPropertyListRowKey
		{
			public string DestCode { get; set; }

			public string Code { get; set; }
		}

		public string DestCode { get; set; }

		public string Code { get; set; }

		public string Caption { get; set; }

		public object GetKey()
		{
			return new MTPropertyListRowKey
			{
				DestCode = DestCode,
				Code = Code
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
			MTPropertyListRowKey mTPropertyListRowKey = (MTPropertyListRowKey)key;
			if (mTPropertyListRowKey.DestCode == DestCode)
			{
				return mTPropertyListRowKey.Code == Code;
			}
			return false;
		}
	}

	public class Param
	{
		public string MyDestCode { get; set; }
	}

	public override string Name => "PropertyList";

	public PropertyListDataProvider(TradeClient client)
		: base((ApiClient)client)
	{
	}
}
