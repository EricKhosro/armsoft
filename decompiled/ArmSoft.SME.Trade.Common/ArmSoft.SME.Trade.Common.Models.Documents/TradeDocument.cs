using System;
using ArmSoft.SME.Common.CustomTypes;
using ArmSoft.SME.Common.Models.Documents;
using ArmSoft.SME.Trade.Common.CustomTypes;

namespace ArmSoft.SME.Trade.Common.Models.Documents;

public class TradeDocument : ArmSoft.SME.Common.Models.Documents.Document
{
	public bool? ChangingActualnessDate { get; set; }

	public bool? MakeAccTransaction { get; set; }

	public bool? MakeGoodsTransaction { get; set; }

	public string CurrCode { get; set; }

	public decimal? Summ { get; set; }

	public string Storage { get; set; }

	public string Info { get; set; }

	public Guid BaseISN { get; set; }

	public int DocOrderNum { get; set; }

	public int PartnerID { get; set; } = -1;

	public int CrPartnerID { get; set; } = -1;

	public int MTID { get; set; } = -1;

	public string ExportedType { get; set; }

	public string InvoiceNumber { get; set; }

	public DateOnly? InvoiceDate { get; set; }

	public int EmplID { get; set; } = -1;

	public decimal? SummNCC { get; set; }

	public PartnerContractGroup PartnerContractGroup { get; set; } = new PartnerContractGroup();

	public ExchangeRate ExchangeRate { get; set; } = new ExchangeRate();

	public string VATType { get; set; }

	public string CashDesk { get; set; }

	public string CashierShiftNumber { get; set; }

	public string SpecField { get; set; }

	public DateOnly? OldDocDate { get; set; }

	public string Owner { get; set; }

	public int? OwnerID { get; set; }

	public int? OldOwnerID { get; set; }

	public Guid TransactionISN { get; set; }

	public bool? IsRegenerateTransactionsRequired { get; set; }

	public bool? DocWithWrongCost { get; set; }
}
