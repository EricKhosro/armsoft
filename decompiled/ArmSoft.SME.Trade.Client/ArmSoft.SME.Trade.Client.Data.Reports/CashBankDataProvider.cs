using System;
using ArmSoft.SME.Client;
using ArmSoft.SME.Client.Data;

namespace ArmSoft.SME.Trade.Client.Data.Reports;

public class CashBankDataProvider : DataProvider<CashBankDataProvider.DataRow, CashBankDataProvider.Param>
{
	public class DataRow : ExtendableRow
	{
		public DateOnly PeriodStartDate { get; set; }

		public string PeriodCaption { get; set; }

		public decimal CashRem { get; set; }

		public decimal BankRem { get; set; }
	}

	public class Param
	{
		public DateOnly StartDate { get; set; }

		public DateOnly EndDate { get; set; }

		public short PeriodMode { get; set; }
	}

	public override string Name => "CashBank";

	public CashBankDataProvider(TradeClient client)
		: base((ApiClient)client)
	{
	}
}
