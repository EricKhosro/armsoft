using System;
using ArmSoft.SME.Client;
using ArmSoft.SME.Client.Data;

namespace ArmSoft.SME.Trade.Client.Data.Reports;

public abstract class CashBankRemainsDataProvider : DataProvider<CashBankRemainsDataProvider.DataRow, CashBankRemainsDataProvider.Param>
{
	public class DataRow : ExtendableRow
	{
		public string Code { get; set; }

		public string Name { get; set; }

		public string SettlAccountCode { get; set; }

		public string CurrCode { get; set; }

		public decimal RemNCC { get; set; }

		public decimal RemCurr { get; set; }
	}

	public class Param
	{
		public DateOnly Date { get; set; }

		public string Code { get; set; }

		public string CurrCode { get; set; }

		public string ReportCurrTypes { get; set; }
	}

	public CashBankRemainsDataProvider(TradeClient client)
		: base((ApiClient)client)
	{
	}
}
