using System;
using ArmSoft.SME.Client;
using ArmSoft.SME.Client.Data;

namespace ArmSoft.SME.Trade.Client.Data.Reports;

public class ABCAnalysisDataProvider : DataProvider<ABCAnalysisDataProvider.DataRow, ABCAnalysisDataProvider.Param>
{
	public class DataRow : ExtendableRow
	{
		public string Code { get; set; }

		public string Name { get; set; }

		public string QntUnitBrief { get; set; }

		public decimal ConsumptionValue { get; set; }

		public decimal? Percentage { get; set; }

		public decimal? CumulativePercentage { get; set; }

		public string ABCCategory { get; set; }

		public string ForPie { get; set; }
	}

	public class Param
	{
		public DateOnly StartDate { get; set; }

		public DateOnly EndDate { get; set; }

		public string Storage { get; set; }

		public string Group { get; set; }

		public int CategorizationCriteria { get; set; }

		public byte SummaType { get; set; }

		public decimal PercentA { get; set; }

		public decimal PercentB { get; set; }

		public bool AnalyzeByPartners { get; set; }
	}

	public override string Name
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	public ABCAnalysisDataProvider(TradeClient client)
		: base((ApiClient)client)
	{
	}
}
