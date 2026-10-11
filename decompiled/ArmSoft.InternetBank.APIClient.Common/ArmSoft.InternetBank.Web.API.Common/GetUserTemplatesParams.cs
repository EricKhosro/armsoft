using ArmSoft.OnlineBanking.Common.Enums;

namespace ArmSoft.InternetBank.Web.API.Common;

public class GetUserTemplatesParams
{
	public string TemplateName { get; set; }

	public ClientStatus? ClientStatus { get; set; }

	public ResidenceType? ClientResidence { get; set; }

	public string ClientBranch { get; set; }

	public string ClientGroups { get; set; }

	public string ClientDepartments { get; set; }

	public bool? IsBankEmployee { get; set; }

	public bool? IsRelatedToBank { get; set; }

	public string ClientResidenceCountry { get; set; }

	public string ClientBirthplaceCountry { get; set; }

	public string ClientCitizenCountry { get; set; }

	public string ClientCitizenCountry2 { get; set; }

	public GetUserTemplatesParams()
	{
		TemplateName = null;
		ClientStatus = null;
		ClientResidence = null;
		ClientBranch = null;
		ClientGroups = null;
		ClientDepartments = null;
		IsBankEmployee = null;
		IsRelatedToBank = null;
		ClientResidenceCountry = null;
		ClientBirthplaceCountry = null;
		ClientCitizenCountry = null;
		ClientCitizenCountry2 = null;
	}
}
