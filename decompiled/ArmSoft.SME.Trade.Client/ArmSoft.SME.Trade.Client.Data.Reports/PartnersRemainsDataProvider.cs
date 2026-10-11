using System;
using ArmSoft.SME.Client;
using ArmSoft.SME.Client.Data;
using ArmSoft.SME.Trade.Common.Models.Caches;

namespace ArmSoft.SME.Trade.Client.Data.Reports;

public class PartnersRemainsDataProvider : DataProvider<PartnersRemainsDataProvider.DataRow, PartnersRemainsDataProvider.Param>
{
	public class DataRow : ExtendableRow
	{
		public int PartID { get; set; }

		public string PartCode { get; set; }

		public string PartName { get; set; }

		public byte B2BStatusCode { get; set; }

		public string B2BStatusName { get; set; }

		public string ContractCode { get; set; }

		public string ContractName { get; set; }

		public DateOnly? ContractDate { get; set; }

		public string CurrCode { get; set; }

		public decimal RemNCC { get; set; }

		public decimal RemCurr { get; set; }

		public PartnerCacheRow PartnerCacheRow { get; set; }

		public PartnerCacheRow.ContractRow ContractCacheRow { get; set; }

		public override object ReferenceValue(string name, string source)
		{
			if (source.StartsWith("Partner"))
			{
				if (PartnerCacheRow != null)
				{
					return PartnerCacheRow.GetReferenceValue(source);
				}
			}
			else if (source.StartsWith("Contract") && ContractCacheRow != null)
			{
				return ContractCacheRow.GetReferenceValue(source);
			}
			return null;
		}
	}

	public class Param
	{
		public DateOnly Date { get; set; }

		public string PartnerGroup { get; set; }

		public string PartnerCode { get; set; }

		public string Contract { get; set; }

		public string CurrCode { get; set; }

		public string ReportCurrTypes { get; set; }

		public bool DetailsByContracts { get; set; }
	}

	public override string Name => "PartnersRemains";

	public PartnersRemainsDataProvider(TradeClient client)
		: base((ApiClient)client)
	{
	}
}
