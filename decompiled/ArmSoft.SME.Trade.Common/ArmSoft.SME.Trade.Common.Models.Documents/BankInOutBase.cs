using ArmSoft.SME.Common.Models.InternetBank;

namespace ArmSoft.SME.Trade.Common.Models.Documents;

public class BankInOutBase : TradeDocument
{
	public string AccountInBankCode { get; set; }

	public string PartnerName { get; set; }

	public string OperType { get; set; }

	public string SecondAcc { get; set; }

	public IBTransactionMappingRequest IBKey { get; set; }
}
