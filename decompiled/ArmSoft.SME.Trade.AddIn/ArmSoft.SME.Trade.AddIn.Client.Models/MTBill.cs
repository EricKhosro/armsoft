using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace ArmSoft.SME.Trade.AddIn.Client.Models;

[ComVisible(true)]
[Guid("9981360B-1BC2-44C2-80AE-061D34BCE2BA")]
[ClassInterface(ClassInterfaceType.AutoDual)]
public class MTBill : ExchangeBase
{
	public string DocumentNumber { get; set; } = string.Empty;

	public DateTime DocumentDate { get; set; }

	public string ECRCheckNumber { get; set; } = string.Empty;

	public string CurrencyCode { get; set; } = string.Empty;

	public string Storage { get; set; } = string.Empty;

	public string CashDesk { get; set; } = string.Empty;

	public string BuyerCode { get; set; } = string.Empty;

	public string BuyerContract { get; set; } = string.Empty;

	public string SalesConsultant { get; set; } = string.Empty;

	public decimal DiscountPercent { get; set; }

	public decimal ReceivedAmount { get; set; }

	public decimal TotalAmount { get; set; }

	public decimal ChangeAmount { get; set; }

	public decimal AmountVAT { get; set; }

	public string Comment { get; set; } = string.Empty;

	public string BankAcquiringPartner { get; set; } = string.Empty;

	public string BankAcquiringContract { get; set; } = string.Empty;

	public string BankAcquiringCardIndex { get; set; } = string.Empty;

	public decimal PlasticCardAmount { get; set; }

	public decimal CreditAmount { get; set; }

	public decimal PayByBonusAmount { get; set; }

	public decimal AddedToBonusAmount { get; set; }

	public decimal PayByPrePaymentAmount { get; set; }

	public string PrePaymentPartner { get; set; } = string.Empty;

	public string PrePaymentContract { get; set; } = string.Empty;

	public decimal SumFromRound { get; set; }

	public string VATType { get; set; } = string.Empty;

	public string CashierShiftNumber { get; set; } = string.Empty;

	public string ISN { get; set; } = string.Empty;

	public bool PrintPartnerTinOnCheck { get; set; }

	public string PhoneNumber { get; set; } = string.Empty;

	public string ECRCRN { get; set; } = string.Empty;

	public string POSTerminalID { get; set; } = string.Empty;

	public string POSTerminalType { get; set; }

	public short NonCashPaymentMethod { get; set; }

	public string PaymentId { get; set; } = string.Empty;

	public bool PaymentAccepted { get; set; }

	public List<MTInvoiceSpecificationRow> Specification { get; set; }

	public MTInvoiceSpecificationRow AddRow()
	{
		if (Specification == null)
		{
			Specification = new List<MTInvoiceSpecificationRow>();
		}
		MTInvoiceSpecificationRow mTInvoiceSpecificationRow = new MTInvoiceSpecificationRow();
		Specification.Add(mTInvoiceSpecificationRow);
		return mTInvoiceSpecificationRow;
	}
}
