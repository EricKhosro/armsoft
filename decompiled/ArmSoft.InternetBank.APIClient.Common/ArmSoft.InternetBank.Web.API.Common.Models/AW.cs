using System;
using Newtonsoft.Json;

namespace ArmSoft.InternetBank.Web.API.Common.Models;

public class AW : AccountPayment
{
	public string VehicleLicensePlateNumber { get; set; }

	public string ThirdPartyName { get; set; }

	public string ThirdPartyPassportOrTaxCode { get; set; }

	public string AswaContractEmail { get; set; }

	public string AswaContractMobile { get; set; }

	public string AswaContractDefaultAccount { get; set; }

	public int InsuranceCompanyId { get; set; }

	public string InsuranceCompany { get; set; }

	public int VehicleUseTypeId { get; set; }

	public string VehicleUseType { get; set; }

	[JsonConverter(typeof(DateConverter))]
	public DateTime InsuranceStartDate { get; set; }

	public int InsuranceDuration { get; set; }

	public string ContractToken { get; set; }

	public string DisplayContractNumber { get; set; }

	public string AswaVehicleModel { get; set; }
}
