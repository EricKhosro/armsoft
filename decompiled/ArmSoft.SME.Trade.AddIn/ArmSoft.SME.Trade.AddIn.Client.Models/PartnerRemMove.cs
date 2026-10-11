using System;
using System.Runtime.InteropServices;

namespace ArmSoft.SME.Trade.AddIn.Client.Models;

[ComVisible(true)]
[Guid("700D7AEC-50B9-4788-AB9D-2148B4CE2AFD")]
[ClassInterface(ClassInterfaceType.AutoDual)]
public class PartnerRemMove : ExchangeBase
{
	public DateTime DocumentDate { get; set; }

	public string DocumentNumber { get; set; } = string.Empty;

	public decimal CurrencyExchangeRate { get; set; }

	public decimal CurrencyExchangeBase { get; set; }

	public decimal Amount { get; set; }

	public decimal AmountNCC { get; set; }

	public string PartnerCode { get; set; } = string.Empty;

	public string Contract { get; set; } = string.Empty;

	public string CurrencyCode { get; set; } = string.Empty;

	public string CorrPartnerCode { get; set; } = string.Empty;

	public string CorrContract { get; set; } = string.Empty;

	public string CorrCurrencyCode { get; set; } = string.Empty;

	public string Comment { get; set; } = string.Empty;
}
