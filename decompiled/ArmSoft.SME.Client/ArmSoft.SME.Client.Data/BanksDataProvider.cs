using ArmSoft.SME.Common.Models;

namespace ArmSoft.SME.Client.Data;

public class BanksDataProvider : DataProvider<BanksDataProvider.DataRow, BanksDataProvider.Param>
{
	public class DataRow : ExtendableRow, IRowWithKey, IMatchedUpdateKey
	{
		public string Code { get; set; }

		public string Caption { get; set; }

		public string SWIFTCODE { get; set; }

		public DataRow(string code, string caption, string swiftCode)
		{
			Code = code;
			Caption = caption;
			SWIFTCODE = swiftCode;
		}

		public DataRow()
		{
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
			return Caption;
		}

		public bool Matched(object key)
		{
			return key.ToString() == Code;
		}
	}

	public class Param : ExtendableParameter
	{
	}

	public override string Name => "Banks";

	public BanksDataProvider(ApiClient client)
		: base(client)
	{
	}
}
