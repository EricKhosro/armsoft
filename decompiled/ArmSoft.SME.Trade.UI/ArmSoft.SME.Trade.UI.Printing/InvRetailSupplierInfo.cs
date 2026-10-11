using ArmSoft.SME.Common.Printing;

namespace ArmSoft.SME.Trade.UI.Printing;

public class InvRetailSupplierInfo
{
	[PrintDataMember("Name")]
	public string Name { get; set; }

	[PrintDataMember("Address")]
	public string Address { get; set; }

	[PrintDataMember("SettlementAccount")]
	public string SettlementAccount { get; set; }

	[PrintDataMember("Bank")]
	public string Bank { get; set; }

	[PrintDataMember("BankCode")]
	public string BankCode { get; set; }

	[PrintDataMember("OptionsTaxCode")]
	public string TaxCode { get; set; }

	[PrintDataMember("ManagerPost")]
	public string ManagerPost { get; set; }

	[PrintDataMember("ManagerFIO")]
	public string ManagerName { get; set; }

	[PrintDataMember("AccountantPost")]
	public string ChiefAccountantPost { get; set; }

	[PrintDataMember("ChAccFIO")]
	public string ChiefAccountantFIO { get; set; }

	[PrintDataMember("PhoneNumber")]
	public string PhoneNumber { get; set; }
}
