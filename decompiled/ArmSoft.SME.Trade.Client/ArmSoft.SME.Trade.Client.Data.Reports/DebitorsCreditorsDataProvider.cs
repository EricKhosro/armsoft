using System;
using ArmSoft.SME.Client;
using ArmSoft.SME.Client.Data;

namespace ArmSoft.SME.Trade.Client.Data.Reports;

public class DebitorsCreditorsDataProvider : DataProvider<DebitorsCreditorsDataProvider.DataRow, DebitorsCreditorsDataProvider.Param>
{
	public class DataRow : ExtendableRow
	{
		public DateOnly PeriodStartDate { get; set; }

		public string PeriodCaption { get; set; } = string.Empty;

		public decimal DbRem { get; set; }

		public decimal CrRem { get; set; }
	}

	public class Param
	{
		public DateOnly StartDate { get; set; }

		public DateOnly EndDate { get; set; }

		public short PeriodMode { get; set; }
	}

	public override string Name => "DebitorsCreditors";

	public DebitorsCreditorsDataProvider(TradeClient client)
		: base((ApiClient)client)
	{
	}
}
