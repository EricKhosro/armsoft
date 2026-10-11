using ArmSoft.SME.Client;
using ArmSoft.SME.Client.Data;
using ArmSoft.SME.Common.Models;

namespace ArmSoft.SME.Trade.Client.Data.Directories;

public class DiscountGroupsDataProvider : DataProvider<DiscountGroupsDataProvider.DataRow, DiscountGroupsDataProvider.Param>
{
	public class DataRow : ExtendableRow, IRowWithKey, IMatchedUpdateKey
	{
		public string Code { get; set; }

		public string Caption { get; set; }

		public string Type { get; set; }

		public string TypeCaption { get; set; }

		public byte Priority { get; set; }

		public byte[] TS { get; set; }

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

	public override string Name => "DiscountGroups";

	public DiscountGroupsDataProvider(TradeClient client)
		: base((ApiClient)client)
	{
	}
}
