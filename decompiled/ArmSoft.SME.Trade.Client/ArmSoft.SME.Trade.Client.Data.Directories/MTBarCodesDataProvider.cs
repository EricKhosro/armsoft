using ArmSoft.SME.Client;
using ArmSoft.SME.Client.Data;
using ArmSoft.SME.Common.Models;

namespace ArmSoft.SME.Trade.Client.Data.Directories;

public class MTBarCodesDataProvider : DataProvider<MTBarCodesDataProvider.DataRow, MTBarCodesDataProvider.Param>
{
	public class DataRow : ExtendableRow, IRowWithKey, IMatchedUpdateKey
	{
		public string BarCode { get; set; }

		public int MTID { get; set; }

		public string MTCode { get; set; }

		public string MTCaption { get; set; }

		public string QntUnit { get; set; }

		public bool IsWeight { get; set; }

		public string QntUnitCaption { get; set; }

		public string QntUnitBrief { get; set; }

		public byte[] Image { get; set; }

		public bool ImageExists { get; set; }

		public string ImgStr => MTID + "," + QntUnit.Trim();

		public object GetKey()
		{
			return BarCode;
		}

		public string GetCode()
		{
			return BarCode;
		}

		public string GetName()
		{
			return "";
		}

		public bool Matched(object key)
		{
			return (string)key == BarCode;
		}
	}

	public class Param
	{
		public string MTCode { get; set; }
	}

	public override string Name => "MTBarCodes";

	public MTBarCodesDataProvider(TradeClient client)
		: base((ApiClient)client)
	{
	}
}
