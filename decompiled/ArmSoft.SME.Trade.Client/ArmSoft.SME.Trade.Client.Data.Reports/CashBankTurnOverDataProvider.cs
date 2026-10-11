using System;
using ArmSoft.SME.Client;
using ArmSoft.SME.Client.Data;

namespace ArmSoft.SME.Trade.Client.Data.Reports;

public abstract class CashBankTurnOverDataProvider : DataProvider<CashBankTurnOverDataProvider.DataRow, CashBankTurnOverDataProvider.Param>
{
	public class DataRow : ExtendableRow
	{
		public string Code { get; set; }

		public string Name { get; set; }

		public string CurrCode { get; set; }

		public decimal StartRemCurr { get; set; }

		public decimal StartRemNCC { get; set; }

		public decimal TurnOverInCurr { get; set; }

		public decimal TurnOverOutCurr { get; set; }

		public decimal TurnOverInNCC { get; set; }

		public decimal TurnOverOutNCC { get; set; }

		public decimal EndRemCurr { get; set; }

		public decimal EndRemNCC { get; set; }
	}

	public class Param
	{
		public DateOnly StartDate { get; set; }

		public DateOnly EndDate { get; set; }

		public string Code { get; set; }

		public string CurrCode { get; set; }

		public string ReportCurrTypes { get; set; }
	}

	public CashBankTurnOverDataProvider(TradeClient client)
		: base((ApiClient)client)
	{
	}
}
