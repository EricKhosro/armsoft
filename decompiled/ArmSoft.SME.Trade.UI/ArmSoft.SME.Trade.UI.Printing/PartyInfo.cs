using System;
using ArmSoft.SME.Common.Printing;

namespace ArmSoft.SME.Trade.UI.Printing;

public class PartyInfo
{
	[PrintDataMember("Specification")]
	public string Description { get; set; }

	[PrintDataMember("IncomeDate")]
	public DateOnly IncomeDate { get; set; }

	[PrintDataMember("UseLife")]
	public DateOnly? UseLife { get; set; }

	[PrintDataMember("Attribute1")]
	public string MTProp1 { get; set; }

	[PrintDataMember("Attribute2")]
	public string MTProp2 { get; set; }

	[PrintDataMember("Supplier")]
	public string Supplier { get; set; }

	[PrintDataMember("Contract")]
	public string Contract { get; set; }

	[PrintDataMember("Country")]
	public string Country { get; set; }

	[PrintDataMember("Producer")]
	public string Producer { get; set; }
}
