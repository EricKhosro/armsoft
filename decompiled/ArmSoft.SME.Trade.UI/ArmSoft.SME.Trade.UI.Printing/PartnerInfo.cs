using ArmSoft.SME.Common.Printing;

namespace ArmSoft.SME.Trade.UI.Printing;

public class PartnerInfo : CodeCaption
{
	[PrintDataMember("FullCaption")]
	public string FullName { get; set; }

	[PrintDataMember("OptionsTaxCode")]
	public string TaxCode { get; set; }

	[PrintDataMember("IDDocType")]
	public string IDDocType { get; set; }

	[PrintDataMember("Passport")]
	public string Passport { get; set; }

	[PrintDataMember("OptionsStateRegister")]
	public string StateRegNumber { get; set; }

	[PrintDataMember("Address")]
	public string Address { get; set; }

	[PrintDataMember("BusinessAddresses")]
	public string BusAddress { get; set; }

	[PrintDataMember("PhoneNumber")]
	public string PhoneNumber { get; set; }

	[PrintDataMember("Parent")]
	public string GroupParent { get; set; }

	[PrintDataMember("ParentCaption")]
	public string GroupParentCaption { get; set; }

	[PrintDataMember("SettlementAccount")]
	public string SettlementAccount { get; set; }

	[PrintDataMember("ManagerFIO")]
	public string ManagerName { get; set; }

	[PrintDataMember("ManagerPost")]
	public string ManagerPost { get; set; }

	[PrintDataMember("ChAccFIO")]
	public string ChiefAccountantFIO { get; set; }

	[PrintDataMember("AccountantPost")]
	public string ChiefAccountantPost { get; set; }
}
