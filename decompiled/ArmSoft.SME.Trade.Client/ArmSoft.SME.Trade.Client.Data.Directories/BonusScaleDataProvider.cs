using System;
using ArmSoft.SME.Client;
using ArmSoft.SME.Client.Data;
using ArmSoft.SME.Common.Models;

namespace ArmSoft.SME.Trade.Client.Data.Directories;

public class BonusScaleDataProvider : DataProvider<BonusScaleDataProvider.DataRow, BonusScaleDataProvider.Param>
{
	public class DataRow : ExtendableRow, IRowWithKey, IMatchedUpdateKey
	{
		public short ScaleRowID { get; set; }

		public decimal BeginSum { get; set; }

		public decimal EndSum { get; set; }

		public decimal Percent { get; set; }

		public decimal Point { get; set; }

		public decimal TS { get; set; }

		public object GetKey()
		{
			return (int)ScaleRowID;
		}

		public string GetCode()
		{
			return "";
		}

		public string GetName()
		{
			return "";
		}

		public bool Matched(object key)
		{
			return ScaleRowID == Convert.ToInt16(key);
		}
	}

	public class Param
	{
	}

	public override string Name => "BonusScale";

	public BonusScaleDataProvider(TradeClient client)
		: base((ApiClient)client)
	{
	}
}
