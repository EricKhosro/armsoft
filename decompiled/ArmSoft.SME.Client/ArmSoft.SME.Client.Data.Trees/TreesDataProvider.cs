using ArmSoft.SME.Common.SmartEnums;

namespace ArmSoft.SME.Client.Data.Trees;

public class TreesDataProvider : DataProvider<TreesDataProvider.DataRow, TreesDataProvider.Param>
{
	public class DataRow : ExtendableRow, IMatchedUpdateKey
	{
		public string Id { get; set; }

		public string Caption { get; set; }

		public int NodesCount { get; set; }

		public bool Multilevel { get; set; }

		public bool HasExtension { get; set; }

		public bool System { get; set; }

		public bool IsLogAvailable { get; set; }

		public bool Matched(object key)
		{
			return (string)key == Id;
		}
	}

	public class Param
	{
	}

	public override string Name => DataProviderTypeSE.Trees.Name;

	public TreesDataProvider(ApiClient client)
		: base(client)
	{
	}
}
