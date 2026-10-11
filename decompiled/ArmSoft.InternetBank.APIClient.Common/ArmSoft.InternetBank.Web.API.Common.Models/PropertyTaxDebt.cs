using System.Collections.Generic;

namespace ArmSoft.InternetBank.Web.API.Common.Models;

public class PropertyTaxDebt
{
	public string PersonToken { get; set; }

	public string Info { get; set; }

	public string Year { get; set; }

	public decimal DebtAmount { get; set; }

	public decimal DebtAmountToPay { get; set; }

	public decimal FineAmount { get; set; }

	public decimal FineAmountToPay { get; set; }

	public decimal Amount { get; set; }

	public string PropertyType { get; set; }

	public string BankAccount { get; set; }

	public string LastUpdateDate { get; set; }

	public string PropertyOwner { get; set; }

	public List<string> PropertyJointOwners { get; set; }

	public bool IsJoint { get; set; }

	public int PropertyNumber { get; set; }

	public string Share { get; set; }

	public decimal Overpay { get; set; }

	public string VehicleRegistrationCertificateNumber { get; set; }

	public string VehicleIdentificationNumber { get; set; }

	public string VehicleMake { get; set; }

	public string VehicleModel { get; set; }
}
