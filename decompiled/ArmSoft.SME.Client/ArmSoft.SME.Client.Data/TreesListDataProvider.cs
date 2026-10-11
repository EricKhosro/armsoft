namespace ArmSoft.SME.Client.Data;

public class TreesListDataProvider : DataProvider<TreesListDataProvider.DataRow, TreesListDataProvider.Param>
{
	public class DataRow : ExtendableRow
	{
		public string Code { get; set; }

		public string Caption { get; set; }
	}

	public class Param : ExtendableParameter
	{
		public bool? IsSystem { get; set; }
	}

	public override string Name => "TreesList";

	public TreesListDataProvider(ApiClient client)
		: base(client)
	{
	}
}
