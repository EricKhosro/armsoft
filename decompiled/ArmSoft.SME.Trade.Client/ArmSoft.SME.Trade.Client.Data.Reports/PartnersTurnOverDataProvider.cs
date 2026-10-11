using System;
using ArmSoft.SME.Client;
using ArmSoft.SME.Client.Data;
using ArmSoft.SME.Trade.Common.Models.Caches;

namespace ArmSoft.SME.Trade.Client.Data.Reports;

public class PartnersTurnOverDataProvider : DataProvider<PartnersTurnOverDataProvider.DataRow, PartnersTurnOverDataProvider.Param>
{
	public class DataRow : ExtendableRow
	{
		public int PartID { get; set; }

		public string PartCode { get; set; }

		public string PartName { get; set; }

		public string PartGroup { get; set; }

		public string ContractCode { get; set; }

		public string ContractName { get; set; }

		public string CurrCode { get; set; }

		public decimal StartRemCurr { get; set; }

		public decimal StartRemNCC { get; set; }

		public decimal TurnOverInCurr { get; set; }

		public decimal TurnOverOutCurr { get; set; }

		public decimal TurnOverInNCC { get; set; }

		public decimal TurnOverOutNCC { get; set; }

		public decimal OverHeadExpSumOutNCC { get; set; }

		public decimal OverHeadExpSumOutCurr { get; set; }

		public decimal SupRetSumNCC { get; set; }

		public decimal SupRetSumCurr { get; set; }

		public decimal RealizSumNCC { get; set; }

		public decimal RealizSumCurr { get; set; }

		public decimal OtherSumInNCC { get; set; }

		public decimal OtherSumInCurr { get; set; }

		public decimal CustPaySumNCC { get; set; }

		public decimal CustPaySumCurr { get; set; }

		public decimal CustRetSumNCC { get; set; }

		public decimal CustRetSumCurr { get; set; }

		public decimal MtInSumNCC { get; set; }

		public decimal MtInSumCurr { get; set; }

		public decimal OtherSumOutNCC { get; set; }

		public decimal OtherSumOutCurr { get; set; }

		public decimal EndRemCurr { get; set; }

		public decimal EndRemNCC { get; set; }

		public PartnerCacheRow PartnerCacheRow { get; set; }

		public override object ReferenceValue(string name, string source)
		{
			if (PartnerCacheRow != null)
			{
				return PartnerCacheRow.GetReferenceValue(source);
			}
			return null;
		}
	}

	public class Param
	{
		public DateOnly StartDate { get; set; }

		public DateOnly EndDate { get; set; }

		public string PartnerGroup { get; set; }

		public string PartnerCode { get; set; }

		public string Contract { get; set; }

		public string CurrCode { get; set; }

		public string ReportCurrTypes { get; set; }

		public bool DetailsByContracts { get; set; }

		public bool DetailsByOperations { get; set; }

		public bool ShowAlsoZeroRows { get; set; }
	}

	public override string Name => "PartnersTurnOver";

	public PartnersTurnOverDataProvider(TradeClient client)
		: base((ApiClient)client)
	{
	}
}
