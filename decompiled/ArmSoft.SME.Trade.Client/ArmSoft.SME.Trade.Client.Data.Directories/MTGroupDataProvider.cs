using ArmSoft.SME.Client;
using ArmSoft.SME.Client.Data;
using ArmSoft.SME.Common.Models;

namespace ArmSoft.SME.Trade.Client.Data.Directories;

public class MTGroupDataProvider : DataProvider<MTGroupDataProvider.DataRow, MTGroupDataProvider.Param>
{
	public class DataRow : ExtendableRow, IMatchedUpdateKey, IRowWithKey, IFilterKey
	{
		public string Code { get; set; }

		public string Caption { get; set; }

		public string CodeForTree { get; set; }

		public string CaptionEn { get; set; }

		public string CaptionRu { get; set; }

		public string Path { get; set; }

		public string Parent { get; set; }

		public string Body { get; set; }

		public bool IsLeaf { get; set; }

		public object FilterKey => Code;

		public object FilterPath => Path;

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
			return Caption;
		}

		public bool Matched(object key)
		{
			return Code == key.ToString();
		}
	}

	public class Param
	{
	}

	public override string Name => "MTGroup";

	public MTGroupDataProvider(TradeClient client)
		: base((ApiClient)client)
	{
	}
}
