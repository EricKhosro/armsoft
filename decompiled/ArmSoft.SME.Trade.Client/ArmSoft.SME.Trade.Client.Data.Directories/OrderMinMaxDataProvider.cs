using ArmSoft.SME.Client;
using ArmSoft.SME.Client.Data;
using ArmSoft.SME.Common.Models;

namespace ArmSoft.SME.Trade.Client.Data.Directories;

public class OrderMinMaxDataProvider : DataProvider<OrderMinMaxDataProvider.DataRow, OrderMinMaxDataProvider.Param>
{
	public class DataRow : ExtendableRow, IRowWithKey, IMatchedUpdateKey
	{
		public class OrderMinMaxRowKey
		{
			public int MTID { get; set; }

			public string StorageCode { get; set; }
		}

		public string StorageCode { get; set; }

		public string Storage { get; set; }

		public int MTID { get; set; } = -1;

		public string MTCode { get; set; }

		public string MTName { get; set; }

		public string MTGroup { get; set; }

		public decimal MinQty { get; set; }

		public decimal MaxQty { get; set; }

		public object GetKey()
		{
			return new OrderMinMaxRowKey
			{
				MTID = MTID,
				StorageCode = StorageCode
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
			OrderMinMaxRowKey orderMinMaxRowKey = (OrderMinMaxRowKey)key;
			if (orderMinMaxRowKey.MTID == MTID)
			{
				return orderMinMaxRowKey.StorageCode.Trim() == StorageCode.Trim();
			}
			return false;
		}
	}

	public class Param
	{
		public string StorageCode { get; set; }

		public string MTCode { get; set; }

		public string MTGroup { get; set; }
	}

	public override string Name => "OrderMinMax";

	public OrderMinMaxDataProvider(TradeClient client)
		: base((ApiClient)client)
	{
	}
}
