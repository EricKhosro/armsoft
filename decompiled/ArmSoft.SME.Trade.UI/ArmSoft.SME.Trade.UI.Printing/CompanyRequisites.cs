using System.Text.Json.Serialization;
using ArmSoft.SME.Common.Parameters;
using ArmSoft.SME.Common.Printing;
using ArmSoft.SME.Trade.Common.Parameters;

namespace ArmSoft.SME.Trade.UI.Printing;

public class CompanyRequisites
{
	[PrintDataMember("Name")]
	public string Name { get; set; }

	[PrintDataMember("Address")]
	public string Address { get; set; }

	[PrintDataMember("BusinessAddresses")]
	public string BusAddress { get; set; }

	[PrintDataMember("PhoneNumber")]
	public string PhoneNumber { get; set; }

	[PrintDataMember("Email")]
	public string Email { get; set; }

	[PrintDataMember("OptionsTaxCode")]
	public string TaxCode { get; set; }

	[PrintDataMember("OptionsStateRegister")]
	public string StateRegNumber { get; set; }

	[PrintDataMember("ManagerFIO")]
	public string ManagerName { get; set; }

	[PrintDataMember("ManagerPost")]
	public string ManagerPost { get; set; }

	[PrintDataMember("ChAccFIO")]
	public string ChiefAccountantFIO { get; set; }

	[PrintDataMember("AccountantPost")]
	public string ChiefAccountantPost { get; set; }

	[PrintDataMember("OptionCashierFIO")]
	public string CashierFIO { get; set; }

	[JsonConstructor]
	public CompanyRequisites()
	{
	}

	public CompanyRequisites(IParametersService parametersService)
	{
		Name = parametersService?.CompanyName();
		Address = parametersService?.CompanyAddress();
		BusAddress = parametersService?.CompanyBusinessAddress();
		PhoneNumber = parametersService?.PhoneNumber();
		Email = parametersService?.Email();
		TaxCode = parametersService?.TaxCode();
		StateRegNumber = parametersService?.StateRegister();
		ManagerName = parametersService?.ManagerName();
		ManagerPost = parametersService?.ManagerPost();
		ChiefAccountantFIO = parametersService?.ChiefAccountantName();
		ChiefAccountantPost = parametersService?.ChiefAccountantPost();
	}
}
