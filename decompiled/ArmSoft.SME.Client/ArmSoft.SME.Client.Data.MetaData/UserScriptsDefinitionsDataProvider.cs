using ArmSoft.SME.Common.Models;

namespace ArmSoft.SME.Client.Data.MetaData;

public class UserScriptsDefinitionsDataProvider : DataProvider<UserScriptsDefinitionsDataProvider.DataRow, UserScriptsDefinitionsDataProvider.Param>
{
	public class DataRow : ExtendableRow, IRowWithKey, IMatchedUpdateKey
	{
		public string Name { get; set; }

		public string Caption { get; set; }

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

	public override string Name => "UserScriptsDefinitions";

	public UserScriptsDefinitionsDataProvider(ApiClient client)
		: base(client)
	{
	}
}
