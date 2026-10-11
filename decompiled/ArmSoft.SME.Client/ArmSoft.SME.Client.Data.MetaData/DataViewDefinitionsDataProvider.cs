using System;
using ArmSoft.SME.Common.Models;

namespace ArmSoft.SME.Client.Data.MetaData;

public class DataViewDefinitionsDataProvider : DataProvider<DataViewDefinitionsDataProvider.DataRow, DataViewDefinitionsDataProvider.Param>
{
	public class DataRow : ExtendableRow, IMatchedUpdateKey, IRowWithKey
	{
		public class RowKey
		{
			public string Group { get; set; }

			public string Name { get; set; }

			public RowKey(string group, string name)
			{
				Group = group;
				Name = name;
			}
		}

		public string Name { get; set; }

		public string Caption { get; set; }

		public string Group { get; set; }

		public string GroupName { get; set; }

		public bool System { get; set; }

		public bool Enabled { get; set; }

		public bool Default { get; set; }

		public bool DropDown { get; set; }

		public DateTime? LastExecutionTime { get; set; }

		public int ExecutionsCount { get; set; }

		public bool Matched(object key)
		{
			RowKey rowKey = (RowKey)key;
			if (rowKey.Group == Group)
			{
				return rowKey.Name == Name;
			}
			return false;
		}

		public object GetKey()
		{
			return new RowKey(Group, Name);
		}

		public string GetCode()
		{
			return Name;
		}

		public string GetName()
		{
			return Caption;
		}
	}

	public class Param : ExtendableParameter
	{
		public string Group { get; set; }

		public short ProviderType { get; set; }
	}

	public override string Name => "DataViewDefinitions";

	public DataViewDefinitionsDataProvider(ApiClient client)
		: base(client)
	{
	}
}
