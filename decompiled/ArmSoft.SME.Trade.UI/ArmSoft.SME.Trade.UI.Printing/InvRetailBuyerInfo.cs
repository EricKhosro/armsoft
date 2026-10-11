using System;
using ArmSoft.SME.Common.Printing;

namespace ArmSoft.SME.Trade.UI.Printing;

public class InvRetailBuyerInfo
{
	[PrintDataMember("Code")]
	public string Code { get; set; }

	[PrintDataMember("Name")]
	public string Name { get; set; }

	[PrintDataMember("FullCaption")]
	public string FullName { get; set; }

	[PrintDataMember("Address")]
	public string Address { get; set; }

	[PrintDataMember("Parent")]
	public string GroupParent { get; set; }

	[PrintDataMember("ParentCaption")]
	public string GroupParentCaption { get; set; }

	[PrintDataMember("SettlementAccount")]
	public string SettlementAccount { get; set; }

	[PrintDataMember("Bank")]
	public string Bank { get; set; }

	[PrintDataMember("BankCode")]
	public string BankCode { get; set; }

	[PrintDataMember("OptionsTaxCode")]
	public string TaxCode { get; set; }

	[PrintDataMember("OptionsStateRegister")]
	public string StateRegNumber { get; set; }

	[PrintDataMember("IDDocType")]
	public string IDDocType { get; set; }

	[PrintDataMember("NPassport")]
	public string Passport { get; set; }

	[PrintDataMember("Contract")]
	public string Contract { get; set; }

	[PrintDataMember("ContractCode")]
	public string ContractCode { get; set; }

	[PrintDataMember("ContractDate")]
	public string ContractDate { get; set; }

	[PrintDataMember("ContractName")]
	public string ContractName { get; set; }

	[PrintDataMember("DiscountCardCode")]
	public string DiscountCard { get; set; }

	[PrintDataMember("BonusCard")]
	public bool IsBonusCard { get; set; }

	[PrintDataMember("BonusPercent")]
	public decimal Bonus { get; set; }

	[PrintDataMember("BonusPoint")]
	public decimal Point { get; set; }

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

	[PrintDataMember("Email")]
	public string EMail { get; set; }

	[PrintDataMember("DateOfBirth")]
	public DateOnly? BirthDay { get; set; }

	[PrintDataMember("Sex")]
	public string Gender { get; set; }

	[PrintDataMember("GenderName")]
	public string GenderName { get; set; }
}
