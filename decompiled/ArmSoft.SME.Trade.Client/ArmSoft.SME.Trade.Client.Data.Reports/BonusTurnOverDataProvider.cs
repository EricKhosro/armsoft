using System;
using ArmSoft.SME.Client;
using ArmSoft.SME.Client.Data;

namespace ArmSoft.SME.Trade.Client.Data.Reports;

public class BonusTurnOverDataProvider : DataProvider<BonusTurnOverDataProvider.DataRow, BonusTurnOverDataProvider.Param>
{
	public class DataRow : ExtendableRow
	{
		public int PartID { get; set; }

		public string PartCode { get; set; }

		public string PartName { get; set; }

		public string PartGroup { get; set; }

		public string ContractCode { get; set; }

		public string ContractName { get; set; }

		public decimal StartRemNCC { get; set; }

		public decimal TurnOverInNCC { get; set; }

		public decimal TurnOverOutNCC { get; set; }

		public decimal EndRemNCC { get; set; }
	}

	public class Param
	{
		public DateOnly StartDate { get; set; }

		public DateOnly EndDate { get; set; }

		public string PartnerGroup { get; set; }

		public string PartnerCode { get; set; }

		public string Contract { get; set; }

		public bool DetailsByContracts { get; set; }
	}

	public override string Name => "BonusTurnOver";

	public BonusTurnOverDataProvider(TradeClient client)
		: base((ApiClient)client)
	{
	}
}
