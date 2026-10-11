using System;
using System.Runtime.InteropServices;

namespace ArmSoft.SME.Trade.AddIn.Client.Models;

[ComVisible(true)]
[Guid("5B0A50ED-D4EA-47C4-96C7-E828CD10984A")]
[ClassInterface(ClassInterfaceType.AutoDual)]
public class BankIn : ExchangeBase
{
	public string AccountCode { get; set; } = string.Empty;

	public string SettlmentAccount { get; set; } = string.Empty;

	public DateTime DocumentDate { get; set; }

	public string DocumentNumber { get; set; } = string.Empty;

	public string CurrencyCode { get; set; } = string.Empty;

	public decimal CurrencyExchangeRate { get; set; }

	public decimal CurrencyExchangeBase { get; set; }

	public decimal Amount { get; set; }

	public decimal AmountNCC { get; set; }

	public string PartnerCode { get; set; } = string.Empty;

	public string Contract { get; set; } = string.Empty;

	public decimal PartnerAmount { get; set; }

	public string Comment { get; set; } = string.Empty;

	public string BaseISN { get; set; } = string.Empty;

	public string ISN { get; set; } = string.Empty;

	public string OperType { get; set; } = "";

	public string SecondAcc { get; set; } = "";
}
