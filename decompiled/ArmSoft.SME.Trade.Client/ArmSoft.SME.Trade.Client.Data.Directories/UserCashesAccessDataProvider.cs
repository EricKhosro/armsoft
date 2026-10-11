using ArmSoft.SME.Client;
using ArmSoft.SME.Client.Data;
using ArmSoft.SME.Common.Models;

namespace ArmSoft.SME.Trade.Client.Data.Directories;

public class UserCashesAccessDataProvider : DataProvider<UserCashesAccessDataProvider.DataRow, UserCashesAccessDataProvider.Param>
{
	public class DataRow : ExtendableRow, IMatchedUpdateKey, IRowWithKey
	{
		public class UserCashesAccessRowKey
		{
			public string CashDesk { get; set; } = string.Empty;

			public int UserId { get; set; }
		}

		public string CashDesk { get; set; } = string.Empty;

		public string Caption { get; set; } = string.Empty;

		public bool Allow { get; set; }

		public int UserId { get; set; }

		public object GetKey()
		{
			return new UserCashesAccessRowKey
			{
				CashDesk = CashDesk,
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

		public bool Matched(object key)
		{
			UserCashesAccessRowKey userCashesAccessRowKey = (UserCashesAccessRowKey)key;
			if (userCashesAccessRowKey.CashDesk == CashDesk)
			{
				return userCashesAccessRowKey.UserId == UserId;
			}
			return false;
		}
	}

	public class Param
	{
		public int UserId { get; set; }
	}

	public override string Name => "UserCashesAccess";

	public UserCashesAccessDataProvider(TradeClient client)
		: base((ApiClient)client)
	{
	}
}
