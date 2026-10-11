using ArmSoft.SME.Common.Models;

namespace ArmSoft.SME.Client.Data;

public class CodeCaptionPairsDataProvider : DataProvider<CodeCaptionPairsDataProvider.DataRow, CodeCaptionPairsDataProvider.Param>
{
	public class DataRow : ExtendableRow, IRowWithKey, IMatchedUpdateKey
	{
		public string Code { get; set; }

		public string Caption { get; set; }

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
			return "";
		}

		public bool Matched(object key)
		{
			return Code == (string)key;
		}
	}

	public class Param : ExtendableParameter
	{
	}

	public override string Name => "CodeCaptionPairs";

	public CodeCaptionPairsDataProvider(ApiClient client)
		: base(client)
	{
	}
}
