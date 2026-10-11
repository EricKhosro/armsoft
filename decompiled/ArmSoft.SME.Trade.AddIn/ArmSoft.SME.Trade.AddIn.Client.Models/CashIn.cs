using System;
using System.Runtime.InteropServices;

namespace ArmSoft.SME.Trade.AddIn.Client.Models;

[ComVisible(true)]
[Guid("799EE996-FC50-44CC-8F2A-23A02B9B333A")]
[ClassInterface(ClassInterfaceType.AutoDual)]
public class CashIn : ExchangeBase
{
	public string OperType { get; set; } = string.Empty;

	public DateTime DocumentDate { get; set; }

	public string DocumentNumber { get; set; } = string.Empty;

	public string CashDesk { get; set; } = string.Empty;

	public string CurrencyCode { get; set; } = string.Empty;

	public decimal CurrencyExchangeRate { get; set; }

	public decimal CurrencyExchangeBase { get; set; }

	public decimal Amount { get; set; }

	public decimal AmountNCC { get; set; }

	public string PartnerCode { get; set; } = string.Empty;

	public string Contract { get; set; } = string.Empty;

	public decimal PartnerAmount { get; set; }

	public string PayerName { get; set; } = string.Empty;

	public string CashOutDesk { get; set; } = string.Empty;

	public string Employee { get; set; } = string.Empty;

	public string AccountCode { get; set; } = string.Empty;

	public string Base { get; set; } = string.Empty;

	public string Appendix { get; set; } = string.Empty;

	public string Comment { get; set; } = string.Empty;

	public string BankAcquiringPartner { get; set; } = string.Empty;

	public string BankAcquiringContract { get; set; } = string.Empty;

	public string BankAcquiringCardIndex { get; set; } = string.Empty;

	public string PaymentId { get; set; } = string.Empty;

	public bool PaymentAccepted { get; set; }

	public string ECRCheckNumber { get; set; } = string.Empty;

	public string CashierShiftNumber { get; set; } = string.Empty;

	public string BaseISN { get; set; } = string.Empty;

	public string ISN { get; set; } = string.Empty;

	public string POSTerminalType { get; set; }

	public short NonCashPaymentMethod { get; set; }

	public string POSTerminalID { get; set; } = string.Empty;

	public string ECRCRN { get; set; } = string.Empty;
}
