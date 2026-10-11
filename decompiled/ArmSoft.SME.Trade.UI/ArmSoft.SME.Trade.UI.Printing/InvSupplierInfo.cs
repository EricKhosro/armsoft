using ArmSoft.SME.Common.Printing;

namespace ArmSoft.SME.Trade.UI.Printing;

public class InvSupplierInfo : InvRetailSupplierInfo
{
	[PrintDataMember("LegalAddresses")]
	public string LegalAddresses { get; set; }

	[PrintDataMember("AddressLabel")]
	public string AddressLabel { get; set; }

	[PrintDataMember("BusinessAddresses")]
	public string BusinessAddresses { get; set; }

	[PrintDataMember("TaxCodeSeparated")]
	public TaxCodeSeparatedClass TaxCodeSeparated { get; set; }
}
