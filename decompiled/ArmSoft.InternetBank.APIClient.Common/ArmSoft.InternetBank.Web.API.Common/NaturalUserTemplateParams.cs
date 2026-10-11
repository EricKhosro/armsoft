using System.Collections.Generic;
using ArmSoft.InternetBank.Web.API.Common.Models;

namespace ArmSoft.InternetBank.Web.API.Common;

public class NaturalUserTemplateParams
{
	public int TemplateId { get; set; }

	public string ClientCode { get; set; }

	public string Username { get; set; }

	public string NewPassword { get; set; }

	public string TokenSerial { get; set; }

	public List<AccountPermission> AccountPermissions { get; set; }

	public List<CardPermission> CardPermissions { get; set; }

	public List<CreditPermission> CreditPermissions { get; set; }

	public List<DepositPermission> DepositPermissions { get; set; }

	public List<BondPermission> BondPermissions { get; set; }

	public List<PurchasedSecurityPermission> PurchasedSecurityPermissions { get; set; }

	public NaturalUserTemplateParams()
	{
		NewPassword = string.Empty;
	}
}
