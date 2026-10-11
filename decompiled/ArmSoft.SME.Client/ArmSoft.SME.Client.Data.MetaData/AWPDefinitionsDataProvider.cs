using ArmSoft.SME.Common.Models;

namespace ArmSoft.SME.Client.Data.MetaData;

public class AWPDefinitionsDataProvider : DataProvider<AWPDefinitionsDataProvider.DataRow, AWPDefinitionsDataProvider.Param>
{
	public class DataRow : ExtendableRow, IRowWithKey, IMatchedUpdateKey
	{
		public string Name { get; set; }

		public string CaptionId { get; set; }

		public string Caption { get; set; }

		public bool System { get; set; }

		public bool Matched(object key)
		{
			return key.ToString() == Name;
		}

		public object GetKey()
		{
			return Name;
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
	}

	public override string Name => "AWPDefinitions";

	public AWPDefinitionsDataProvider(ApiClient client)
		: base(client)
	{
	}
}
