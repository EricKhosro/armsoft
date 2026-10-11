using System;
using ArmSoft.SME.Client;
using ArmSoft.SME.Client.Data;

namespace ArmSoft.SME.Trade.Client.Data.Reports;

public class BonusRemainsDataProvider : DataProvider<BonusRemainsDataProvider.DataRow, BonusRemainsDataProvider.Param>
{
	public class DataRow : ExtendableRow
	{
		public int PartID { get; set; } = -1;

		public string PartCode { get; set; } = string.Empty;

		public string PartName { get; set; } = string.Empty;

		public string ContractCode { get; set; } = string.Empty;

		public string ContractName { get; set; } = string.Empty;

		public DateOnly? ContractDate { get; set; }

		public string PartnerGroup { get; set; } = string.Empty;

		public string TaxCode { get; set; } = string.Empty;

		public string Address { get; set; } = string.Empty;

		public string Phone { get; set; } = string.Empty;

		public decimal RemNCC { get; set; }
	}

	public class Param
	{
		public DateOnly Date { get; set; }

		public string PartnerGroup { get; set; }

		public string PartnerCode { get; set; }

		public string Contract { get; set; }

		public bool DetailsByContracts { get; set; }

		public bool ShowPartnerData { get; set; }
	}

	public override string Name => "BonusRemains";

	public BonusRemainsDataProvider(TradeClient client)
		: base((ApiClient)client)
	{
	}
}
