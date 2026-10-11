using System;
using ArmSoft.SME.Client;
using ArmSoft.SME.Client.Data;

namespace ArmSoft.SME.Trade.Client.Data.Reports;

public class DiscountLogsDataProvider : DataProvider<DiscountLogsDataProvider.DataRow, DiscountLogsDataProvider.Param>
{
	public class DataRow : ExtendableRow
	{
		public Guid ISN { get; set; } = Guid.Empty;

		public string DocNum { get; set; } = string.Empty;

		public DateOnly DocDate { get; set; }

		public short RowNumber { get; set; }

		public int GoodId { get; set; } = -1;

		public string GoodCode { get; set; } = string.Empty;

		public string GoodCaption { get; set; } = string.Empty;

		public string DiscountGroupCode { get; set; } = string.Empty;

		public byte DiscountGroupPriority { get; set; }

		public string GroupMergeMethod { get; set; } = string.Empty;

		public string GroupMergeMethodCaption { get; set; } = string.Empty;

		public string DiscountCode { get; set; } = string.Empty;

		public string DiscountCaption { get; set; } = string.Empty;

		public byte DiscountPriority { get; set; }

		public decimal DiscountedSum { get; set; }

		public decimal Percent { get; set; }
	}

	public class Param
	{
		public Guid ISN { get; set; } = Guid.Empty;
	}

	public override string Name => "DiscountLogs";

	public DiscountLogsDataProvider(TradeClient client)
		: base((ApiClient)client)
	{
	}
}
