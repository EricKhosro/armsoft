using System;

namespace ArmSoft.InternetBank.Web.API.Common.Models;

public class AmioBankBranch
{
	public string Code { get; set; }

	public string Ename { get; set; }

	[Obsolete("Use Name instead.")]
	public string ShortName { get; set; }

	public string Name { get; set; }

	public string BankCode { get; set; }

	public string Address { get; set; }

	public string EAddress { get; set; }

	public string PhoneNumber { get; set; }

	public string CompanyCity { get; set; }

	public string District { get; set; }

	public string Region { get; set; }

	public string Country { get; set; }
}
