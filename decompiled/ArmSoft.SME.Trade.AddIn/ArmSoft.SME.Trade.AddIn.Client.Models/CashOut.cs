using System;
using System.Runtime.InteropServices;

namespace ArmSoft.SME.Trade.AddIn.Client.Models;

[ComVisible(true)]
[Guid("CF07F462-F7B0-41D1-9CC3-0F9E6921B58A")]
[ClassInterface(ClassInterfaceType.AutoDual)]
public class CashOut : ExchangeBase
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

	public string CashIntDesk { get; set; } = string.Empty;

	public string Employee { get; set; } = string.Empty;

	public string AccountCode { get; set; } = string.Empty;

	public string ReceiverName { get; set; } = string.Empty;

	public string PassportNumber { get; set; } = string.Empty;

	public string Base { get; set; } = string.Empty;

	public string Appendix { get; set; } = string.Empty;

	public string Comment { get; set; } = string.Empty;

	public string BankAcquiringPartner { get; set; } = string.Empty;

	public string BankAcquiringContract { get; set; } = string.Empty;

	public string BankAcquiringCardIndex { get; set; } = string.Empty;

	public bool PaymentRefundAccepted { get; set; }

	public string ECRReturnCheckNumber { get; set; } = string.Empty;

	public string CashierShiftNumber { get; set; } = string.Empty;

	public string BaseISN { get; set; } = string.Empty;

	public string ISN { get; set; } = string.Empty;
}
