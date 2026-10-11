using ArmSoft.SME.Common.Printing;

namespace ArmSoft.SME.Trade.UI.Printing;

public class InvBuyerInfo : InvRetailBuyerInfo
{
	[PrintDataMember("LegalAddresses")]
	public string LegalAddresses { get; set; }

	[PrintDataMember("AddressLabel")]
	public string AddressLabel { get; set; }

	[PrintDataMember("TaxCodeStateRegNumPassportLabel")]
	public string TaxCodeStateRegNumPassportLabel { get; set; }

	[PrintDataMember("TaxCodeStateRegNumPassport")]
	public string TaxCodeStateRegNumPassport { get; set; }

	[PrintDataMember("TaxCodeSeparated")]
	public TaxCodeSeparatedClass TaxCodeSeparated { get; set; }

	[PrintDataMember("BusinessAddresses")]
	public string BusAddress { get; set; }
}
