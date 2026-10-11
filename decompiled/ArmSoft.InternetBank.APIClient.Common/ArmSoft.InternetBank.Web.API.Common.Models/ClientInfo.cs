using System;
using System.Collections.Generic;
using ArmSoft.OnlineBanking.Common.Enums;

namespace ArmSoft.InternetBank.Web.API.Common.Models;

public class ClientInfo
{
	public string Code { get; set; }

	public string Name { get; set; }

	public string EName { get; set; }

	public ClientStatus Status { get; set; }

	public string TaxCode { get; set; }

	public string SocialCard { get; set; }

	public bool IsActive { get; set; }

	public bool IsViewOnly { get; set; }

	public string Branch { get; set; }

	public string Group { get; set; }

	public string Passport { get; set; }

	public ResidenceType Residence { get; set; }

	public DateTime? BirthDate { get; set; }

	public string Email { get; set; }

	public string Mobile { get; set; }

	public string City { get; set; }

	public string Country { get; set; }

	public string ECity { get; set; }

	public string Address { get; set; }

	public string EAddress { get; set; }

	public bool IsBankEmployee { get; set; }

	public bool IsRelatedToBank { get; set; }

	public ChargeAccountMode ChargeAccountMode { get; set; }

	public List<Passport> Passports { get; set; }
}
