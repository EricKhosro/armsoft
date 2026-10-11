using System;

namespace ArmSoft.SME.Trade.Common.Models.Documents;

public class PartyRow
{
	public Guid Base { get; set; }

	public int Row { get; set; }

	public decimal Qty { get; set; }

	public DateOnly IncomeDate { get; set; }

	public int OrderNum { get; set; }

	public decimal Cost { get; set; }

	public decimal Summa { get; set; }

	public decimal ExpQty { get; set; }

	public decimal ExpSumma { get; set; }

	public int GoodID { get; set; }

	public string Storage { get; set; }

	public int GridRow { get; set; }

	public string Comment { get; set; }

	public decimal VATSumm { get; set; }

	public decimal CurrSumm { get; set; }

	public decimal SaleSumm { get; set; }

	public decimal SaleCurrSumm { get; set; }

	public decimal SaleVATSumm { get; set; }

	public decimal DiscSumm { get; set; }

	public decimal DiscCurrSumm { get; set; }

	public decimal DiscVATSumm { get; set; }

	public decimal UsedBonusSumm { get; set; }

	public decimal AccumulatedBonusSumm { get; set; }

	public DateOnly? UseableDate { get; set; }

	public int PartnerID { get; set; } = -1;

	public string PartnerCode { get; set; } = "";

	public string PartnerCaption { get; set; } = "";

	public string Contract { get; set; }

	public string Country { get; set; }

	public string Producer { get; set; }

	public string Property1Code { get; set; }

	public string Property2Code { get; set; }

	public string Property1 { get; set; }

	public string Property2 { get; set; }

	public string ExpMethod { get; set; }

	public PartyRow Clone()
	{
		return new PartyRow
		{
			Base = Base,
			Row = Row,
			Qty = Qty,
			IncomeDate = IncomeDate,
			OrderNum = OrderNum,
			Cost = Cost,
			Summa = Summa,
			ExpQty = ExpQty,
			ExpSumma = ExpSumma,
			GoodID = GoodID,
			Storage = Storage,
			GridRow = GridRow,
			Comment = Comment,
			VATSumm = VATSumm,
			CurrSumm = CurrSumm,
			SaleSumm = SaleSumm,
			SaleCurrSumm = SaleCurrSumm,
			SaleVATSumm = SaleVATSumm,
			DiscCurrSumm = DiscCurrSumm,
			DiscSumm = DiscSumm,
			DiscVATSumm = DiscVATSumm,
			UseableDate = UseableDate,
			PartnerID = PartnerID,
			PartnerCode = PartnerCode,
			Contract = Contract,
			Country = Country,
			Producer = Producer,
			Property1 = Property1,
			Property2 = Property2,
			Property1Code = Property1Code,
			Property2Code = Property2Code,
			ExpMethod = ExpMethod
		};
	}
}
