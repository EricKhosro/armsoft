using System;
using System.Collections.Generic;
using ArmSoft.SME.Trade.Common.CustomTypes;

namespace ArmSoft.SME.Trade.Common.Models.Documents;

public class MTReturnFromCustomerBase : TradeDocument
{
	public class MTRetFromCustSpecificationRow
	{
		public string ItemType { get; set; }

		public string Storage { get; set; }

		public string ItemCode { get; set; }

		public int? ItemID { get; set; } = -1;

		public short? RowNum { get; set; } = 0;

		public string ItemName { get; set; }

		public string UnitCode { get; set; }

		public string UnitBrief { get; set; } = string.Empty;

		public decimal? Quantity { get; set; } = 0m;

		public decimal? Price { get; set; } = 0m;

		public decimal? Discount { get; set; } = 0m;

		public decimal? DiscountPrice { get; set; } = 0m;

		public decimal? UsedBonus { get; set; } = 0m;

		public decimal? AccumulatedBonus { get; set; } = 0m;

		public decimal? Summa { get; set; } = 0m;

		public decimal? DiscountSumma { get; set; } = 0m;

		public decimal? BeginSumma { get; set; } = 0m;

		public decimal? BeginPrice { get; set; } = 0m;

		public decimal? CostPrice { get; set; } = 0m;

		public decimal? CostPriceVAT { get; set; } = 0m;

		public decimal? CurrCostPrice { get; set; } = 0m;

		public string BarCode { get; set; }

		public decimal? EnvFeePercent { get; set; }

		public decimal? EnvFeeSumma { get; set; }

		public bool? VAT { get; set; }

		public string DealType { get; set; } = string.Empty;

		public int? RowID { get; set; }

		public int? BaseRowID { get; set; } = -1;

		public bool? IsPartySelected { get; set; }

		public string RowDescription { get; set; } = string.Empty;

		public string ExternalCode { get; set; } = string.Empty;

		public string RowSpecField { get; set; } = string.Empty;

		public bool? IsDeleted { get; set; } = false;

		public bool? IsGift { get; set; } = false;

		public string ShortName { get; set; } = string.Empty;

		public string FullName { get; set; } = string.Empty;

		public bool? IsStorageRequired { get; set; } = true;

		public Guid PartyBase { get; set; }

		public int? PartyRow { get; set; }

		public DateOnly? UseLife { get; set; }

		public string Country { get; set; } = string.Empty;

		public string Producer { get; set; } = string.Empty;

		public string ProducerCaption { get; set; } = string.Empty;

		public string Property1 { get; set; } = string.Empty;

		public string Property1Caption { get; set; } = string.Empty;

		public string Property2 { get; set; } = string.Empty;

		public string Property2Caption { get; set; } = string.Empty;

		public HashSet<string> MarkingList { get; set; } = new HashSet<string>();

		public bool IsRowMarkable { get; set; }

		public string MarkingStatus { get; set; }

		public string CPACode { get; set; } = "";
	}

	public decimal AccumulatedBonusSum { get; set; }

	public decimal UsedBonusSum { get; set; }

	public PartnerContractGroup BankAcquiring { get; set; } = new PartnerContractGroup();

	public string ECRBaseCheckNum { get; set; } = "";

	public string ECRCheckNum { get; set; } = "";

	public decimal ECRReturnByCardSum { get; set; }

	public decimal ECRReturnByCashSum { get; set; }

	public decimal ECRReturnByPrePaymentSum { get; set; }

	public string ECRReturnCheckNum { get; set; } = "";

	public int MaxRowID { get; set; }

	public string SalesConsultant { get; set; } = "";

	public string DealType { get; set; } = "";

	public decimal SummaPayByBonus { get; set; }

	public decimal SummVAT { get; set; }

	public string PriceType { get; set; } = "";

	public bool PaymentRefundAccepted { get; set; }

	public PartnerContractGroup PrepaymentGroup { get; set; } = new PartnerContractGroup();

	public bool IsDeferred { get; set; }

	public decimal SumFromRound { get; set; }

	public List<MTRetFromCustSpecificationRow> MTList { get; set; } = new List<MTRetFromCustSpecificationRow>();
}
