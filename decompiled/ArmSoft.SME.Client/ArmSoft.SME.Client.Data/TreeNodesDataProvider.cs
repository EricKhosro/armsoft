using ArmSoft.SME.Common.Models;

namespace ArmSoft.SME.Client.Data;

public class TreeNodesDataProvider : DataProvider<TreeNodesDataProvider.DataRow, TreeNodesDataProvider.Param>
{
	public class DataRow : ExtendableRow, IRowWithKey, IMatchedUpdateKey
	{
		public string Code { get; set; }

		public string Name { get; set; }

		public string Parent { get; set; }

		public bool IsClosed { get; set; }

		public string Spec { get; set; }

		public bool Leaf { get; set; }

		public bool Matched(object key)
		{
			return key.ToString() == Code;
		}

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
			return Name;
		}
	}

	public class Param
	{
		public string TreeId { get; set; }

		public string Parent { get; set; }

		public bool ShowAsTree { get; set; }

		public bool ShowOnlyLeafNodes { get; set; }

		public bool ShowClosed { get; set; }

		public bool ShowExpanded { get; set; }
	}

	private readonly string _treeId;

	private readonly bool _isSystem;

	public override string Name
	{
		get
		{
			if (!_isSystem)
			{
				return "TreeNodes";
			}
			return "Tree_" + _treeId;
		}
	}

	public TreeNodesDataProvider(string treeId, bool isSystem, ApiClient client)
		: base(client)
	{
		_treeId = treeId;
		_isSystem = isSystem;
	}
}
