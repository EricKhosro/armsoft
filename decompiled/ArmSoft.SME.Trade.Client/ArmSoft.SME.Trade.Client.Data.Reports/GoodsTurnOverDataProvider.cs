using System;
using ArmSoft.SME.Client;
using ArmSoft.SME.Client.Data;
using ArmSoft.SME.Trade.Common.Models.Caches;

namespace ArmSoft.SME.Trade.Client.Data.Reports;

public class GoodsTurnOverDataProvider : DataProvider<GoodsTurnOverDataProvider.DataRow, GoodsTurnOverDataProvider.Param>
{
	public class DataRow : ExtendableRow
	{
		public int GoodID { get; set; } = -1;

		public string GoodCode { get; set; } = "";

		public string Caption { get; set; } = "";

		public string Unit { get; set; } = "";

		public string UnitBrief { get; set; } = "";

		public string Storage { get; set; } = "";

		public string StorageCaption { get; set; } = "";

		public decimal QtyBegin { get; set; }

		public decimal SumBegin { get; set; }

		public decimal SumVATBegin { get; set; }

		public decimal SumCurrBegin { get; set; }

		public decimal QtyIn { get; set; }

		public decimal SumIn { get; set; }

		public decimal SumVATIn { get; set; }

		public decimal SumCurrIn { get; set; }

		public decimal QtyOut { get; set; }

		public decimal SumOut { get; set; }

		public decimal SumVATOut { get; set; }

		public decimal SumCurrOut { get; set; }

		public decimal QtyEnd
		{
			get
			{
				return QtyBegin + QtyIn - QtyOut;
			}
			set
			{
			}
		}

		public decimal SumEnd
		{
			get
			{
				return SumBegin + SumIn - SumOut;
			}
			set
			{
			}
		}

		public decimal SumVATEnd
		{
			get
			{
				return SumVATBegin + SumVATIn - SumVATOut;
			}
			set
			{
			}
		}

		public decimal SumCurrEnd
		{
			get
			{
				return SumCurrBegin + SumCurrIn - SumCurrOut;
			}
			set
			{
			}
		}

		public string GroupCodeCaption { get; set; } = "";

		public decimal QTYInIncome { get; set; }

		public decimal SumInIncome { get; set; }

		public decimal SumVatInIncome { get; set; }

		public decimal SumCurrInIncome { get; set; }

		public decimal SumInOverheadExp { get; set; }

		public decimal SumVatInOverheadExp { get; set; }

		public decimal SumCurrInOverheadExp { get; set; }

		public decimal QTYInBillReturn { get; set; }

		public decimal SumInBillReturn { get; set; }

		public decimal SumVatInBillReturn { get; set; }

		public decimal SumCurrInBillReturn { get; set; }

		public decimal QTYInReturnFromCustomer { get; set; }

		public decimal SumInReturnFromCustomer { get; set; }

		public decimal SumVatInReturnFromCustomer { get; set; }

		public decimal SumCurrInReturnFromCustomer { get; set; }

		public decimal QTYInMove { get; set; }

		public decimal SumInMove { get; set; }

		public decimal SumVatInMove { get; set; }

		public decimal SumCurrInMove { get; set; }

		public decimal QTYInComplectation { get; set; }

		public decimal SumInComplectation { get; set; }

		public decimal SumVatInComplectation { get; set; }

		public decimal SumCurrInComplectation { get; set; }

		public decimal QTYInDeComplectation { get; set; }

		public decimal SumInDeComplectation { get; set; }

		public decimal SumVatInDeComplectation { get; set; }

		public decimal SumCurrInDeComplectation { get; set; }

		public decimal QTYInIncomeRem { get; set; }

		public decimal SumInIncomeRem { get; set; }

		public decimal SumVatInIncomeRem { get; set; }

		public decimal SumCurrInIncomeRem { get; set; }

		public decimal QTYInReval { get; set; }

		public decimal SumInReval { get; set; }

		public decimal SumVatInReval { get; set; }

		public decimal SumCurrInReval { get; set; }

		public decimal QTYOutSale { get; set; }

		public decimal SumOutSale { get; set; }

		public decimal SumVatOutSale { get; set; }

		public decimal SumCurrOutSale { get; set; }

		public decimal QTYOutSaleRetail { get; set; }

		public decimal SumOutSaleRetail { get; set; }

		public decimal SumVatOutSaleRetail { get; set; }

		public decimal SumCurrOutSaleRetail { get; set; }

		public decimal QTYOutReturnToSupplier { get; set; }

		public decimal SumOutReturnToSupplier { get; set; }

		public decimal SumVatOutReturnToSupplier { get; set; }

		public decimal SumCurrOutReturnToSupplier { get; set; }

		public decimal QTYOutExpense { get; set; }

		public decimal SumOutExpense { get; set; }

		public decimal SumVatOutExpense { get; set; }

		public decimal SumCurrOutExpense { get; set; }

		public decimal QTYOutMove { get; set; }

		public decimal SumOutMove { get; set; }

		public decimal SumVatOutMove { get; set; }

		public decimal SumCurrOutMove { get; set; }

		public decimal QTYOutComplectation { get; set; }

		public decimal SumOutComplectation { get; set; }

		public decimal SumVatOutComplectation { get; set; }

		public decimal SumCurrOutComplectation { get; set; }

		public decimal QTYOutDeComplectation { get; set; }

		public decimal SumOutDeComplectation { get; set; }

		public decimal SumVatOutDeComplectation { get; set; }

		public decimal SumCurrOutDeComplectation { get; set; }

		public decimal QTYOutReval { get; set; }

		public decimal SumOutReval { get; set; }

		public decimal SumVatOutReval { get; set; }

		public decimal SumCurrOutReval { get; set; }

		public decimal SaleSumBegin { get; set; }

		public decimal SaleSumVATBegin { get; set; }

		public decimal SaleSumCurrBegin { get; set; }

		public DateOnly IncomeDate { get; set; }

		public int PartyPartID { get; set; } = -1;

		public string PartyPartCaption { get; set; } = "";

		public string PartyPartTaxCode { get; set; } = "";

		public string PartyPartContract { get; set; } = "";

		public string PartyDocNum { get; set; } = "";

		public string PartyDocType { get; set; } = "";

		public DateOnly? PartyDocUsableDate { get; set; }

		public string PartyDocCountry { get; set; } = "";

		public string PartyDocProducer { get; set; } = "";

		public string PartyDocPropertyType1 { get; set; } = "";

		public string PartyDocPropertyValue1 { get; set; } = "";

		public string PartyDocPropertyType2 { get; set; } = "";

		public string PartyDocPropertyValue2 { get; set; } = "";

		public Guid PartyBase { get; set; }

		public int PartyRow { get; set; }

		public MaterialCacheRow MaterialCacheRow { get; set; }

		public ProducerCacheRow ProducerCacheRow { get; set; }

		public override object ReferenceValue(string name, string source)
		{
			if (source.StartsWith("Material"))
			{
				if (MaterialCacheRow != null)
				{
					return MaterialCacheRow.GetReferenceValue(source);
				}
			}
			else if (source.StartsWith("Producer") && ProducerCacheRow != null)
			{
				return ProducerCacheRow.GetReferenceValue(source);
			}
			return null;
		}
	}

	public class Param
	{
		public DateOnly StartDate { get; set; }

		public DateOnly EndDate { get; set; }

		public string Storage { get; set; } = string.Empty;

		public string Group { get; set; } = string.Empty;

		public string GoodCode { get; set; }

		public bool ShowAltUnit { get; set; }

		public bool ShowSumms { get; set; }

		public bool ShowSummsWithVAT { get; set; }

		public bool ShowCurrSumms { get; set; }

		public bool ShowSaleSumms { get; set; }

		public bool ShowParties { get; set; }

		public bool DetailByStorage { get; set; }

		public bool DetailByOper { get; set; }

		public bool ShowAlsoZeroRows { get; set; }
	}

	public override string Name => "GoodsTurnOver";

	public GoodsTurnOverDataProvider(TradeClient client)
		: base((ApiClient)client)
	{
	}
}
