using ArmSoft.SME.Client;
using ArmSoft.SME.Client.Data;
using ArmSoft.SME.Common.Models;

namespace ArmSoft.SME.Trade.Client.Data.Directories;

public class UserStorageOptionsDataProvider : DataProvider<UserStorageOptionsDataProvider.DataRow, UserStorageOptionsDataProvider.Param>
{
	public class DataRow : ExtendableRow, IMatchedUpdateKey, IRowWithKey
	{
		public class UserStorageOptionsRowKey
		{
			public string Storage { get; set; } = string.Empty;

			public int UserId { get; set; }
		}

		public int UserId { get; set; }

		public string Storage { get; set; }

		public string Caption { get; set; }

		public bool AllowOper { get; set; }

		public bool AllowReportView { get; set; }

		public bool AllowNegativExp { get; set; }

		public bool SalesWithZeroCost { get; set; }

		public bool Matched(object key)
		{
			UserStorageOptionsRowKey userStorageOptionsRowKey = (UserStorageOptionsRowKey)key;
			if (userStorageOptionsRowKey.Storage == Storage)
			{
				return userStorageOptionsRowKey.UserId == UserId;
			}
			return false;
		}

		public object GetKey()
		{
			return new UserStorageOptionsRowKey
			{
				Storage = Storage,
				UserId = UserId
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

	public class Param
	{
		public int UserId { get; set; }
	}

	public override string Name => "UserStorageOptions";

	public UserStorageOptionsDataProvider(TradeClient client)
		: base((ApiClient)client)
	{
	}
}
