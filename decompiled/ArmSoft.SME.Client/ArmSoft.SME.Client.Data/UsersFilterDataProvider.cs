namespace ArmSoft.SME.Client.Data;

public class UsersFilterDataProvider : DataProvider<UsersFilterDataProvider.DataRow, UsersFilterDataProvider.Param>
{
	public class DataRow : ExtendableRow
	{
		public int UserID { get; set; }

		public string Name { get; set; }

		public string Description { get; set; }

		public bool Login { get; set; }

		public bool Admin { get; set; }

		public int? LocalUserId { get; set; }

		public int CloudUserId { get; set; }

		public DataRow(int userId, string name, string description, bool login, bool admin, int? localUserId)
		{
			UserID = userId;
			Name = name;
			Description = description;
			Login = login;
			Admin = admin;
			LocalUserId = localUserId;
			CloudUserId = userId;
		}

		public DataRow()
		{
		}
	}

	public class Param : ExtendableParameter
	{
	}

	public override string Name => "UsersFilter";

	public UsersFilterDataProvider(ApiClient client)
		: base(client)
	{
	}
}
