using ArmSoft.SME.Common.Models;

namespace ArmSoft.SME.Client.Data.Reports;

public class UsersDataProvider : DataProvider<UsersDataProvider.DataRow, UsersDataProvider.Param>
{
	public class DataRow : ExtendableRow, IMatchedUpdateKey, IRowWithKey
	{
		public int UserId { get; set; }

		public string Name { get; set; }

		public string Caption { get; set; }

		public bool Login { get; set; }

		public bool Admin { get; set; }

		public bool IsDbAdmin { get; set; }

		public string CardNumber { get; set; }

		public int? LocalUserId { get; set; }

		public int? CloudUserId { get; set; }

		public object GetKey()
		{
			return UserId;
		}

		public string GetCode()
		{
			return Name;
		}

		public string GetName()
		{
			return Caption;
		}

		public bool Matched(object key)
		{
			return (int)key == UserId;
		}
	}

	public class Param : ExtendableParameter
	{
		public int UserId { get; set; }
	}

	public override string Name => "Users";

	public UsersDataProvider(ApiClient client)
		: base(client)
	{
	}
}
