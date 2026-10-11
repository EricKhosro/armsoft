using ArmSoft.SME.Common.Printing;

namespace ArmSoft.SME.Trade.UI.Printing;

public class EmployeeInfo : CodeCaption
{
	[PrintDataMember("Address")]
	public string Address { get; set; }

	[PrintDataMember("Phone")]
	public string Phone { get; set; }

	[PrintDataMember("NPassport")]
	public string Passport { get; set; }

	[PrintDataMember("DiscountCardCode")]
	public string EmployeeCard { get; set; }
}
