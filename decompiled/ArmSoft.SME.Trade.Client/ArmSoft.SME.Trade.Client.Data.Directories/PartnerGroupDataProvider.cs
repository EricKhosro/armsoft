using ArmSoft.SME.Client;
using ArmSoft.SME.Client.Data;
using ArmSoft.SME.Common.Models;

namespace ArmSoft.SME.Trade.Client.Data.Directories;

public class PartnerGroupDataProvider : DataProvider<PartnerGroupDataProvider.DataRow, PartnerGroupDataProvider.Param>
{
	public class DataRow : ExtendableRow, IRowWithKey, IFilterKey, IMatchedUpdateKey
	{
		public string Code { get; set; }

		public string Caption { get; set; }

		public string CodeForTree { get; set; }

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
			return (string)key == Code;
		}
	}

	public class Param
	{
	}

	public override string Name => "PartnerGroup";

	public PartnerGroupDataProvider(TradeClient client)
		: base((ApiClient)client)
	{
	}
}
