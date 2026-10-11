namespace ArmSoft.SME.Client.Data.Reports;

public class UserResourcesDataProvider : DataProvider<UserResourcesDataProvider.DataRow, ExtendableParameter>
{
	public class DataRow : ExtendableRow, IMatchedUpdateKey
	{
		public string Name { get; set; }

		public string Caption { get; set; }

		public string EnglishCaption { get; set; }

		public string RussianCaption { get; set; }

		public bool System { get; set; }

		public bool Matched(object key)
		{
			return key.ToString().Equals(Name);
		}
	}

	public override string Name => "UserResources";

	public UserResourcesDataProvider(ApiClient client)
		: base(client)
	{
	}
}
