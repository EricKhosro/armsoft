using System.Collections.Generic;
using ArmSoft.OnlineBanking.Common.Enums;

namespace ArmSoft.InternetBank.Web.API.Common.Models;

public class ModelWithClientFilter
{
	public List<string> ClientCodes { get; set; }

	public List<ClientStatus> ClientStatuses { get; set; }

	public ResidenceType ClientResidence { get; set; }

	public string ClientBranch { get; set; }

	public List<string> ClientGroups { get; set; }

	public List<string> ClientDepartment { get; set; }

	public bool? IsBankEmployee { get; set; }

	public List<string> UserRoles { get; set; }

	public bool? IsRelatedToBank { get; set; }

	public string MinClientVersion { get; set; }

	public string MaxClientVersion { get; set; }
}
