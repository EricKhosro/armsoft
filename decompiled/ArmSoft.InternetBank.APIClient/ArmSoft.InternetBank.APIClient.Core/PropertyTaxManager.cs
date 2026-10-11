using System.Collections.Generic;
using System.Threading.Tasks;
using ArmSoft.InternetBank.Web.API.Common.Models;

namespace ArmSoft.InternetBank.APIClient.Core;

public class PropertyTaxManager : BaseManager
{
	internal PropertyTaxManager(Client client)
		: base(client)
	{
	}

	public async Task<List<PropertyTaxDebt>> GetPropertyTaxDebtsAsync(PropertyTaxDebtRequestParams propertyTaxDebtRequestParams)
	{
		Dictionary<string, object> postBody = new Dictionary<string, object>
		{
			["PropertyTaxOwnerSocialCard"] = propertyTaxDebtRequestParams.PropertyTaxOwnerSocialCard,
			["PropertyTaxOwnerTaxCode"] = propertyTaxDebtRequestParams.PropertyTaxOwnerTaxCode,
			["PropertyTaxOwnerLegalStatus"] = propertyTaxDebtRequestParams.PropertyTaxOwnerLegalStatus,
			["PropertyTaxDebtPeriod"] = propertyTaxDebtRequestParams.PropertyTaxDebtPeriod,
			["VehicleRegistrationCertificateNumber"] = propertyTaxDebtRequestParams.VehicleRegistrationCertificateNumber,
			["VehicleLicensePlateNumber"] = propertyTaxDebtRequestParams.VehicleLicensePlateNumber,
			["PropertyTaxCode"] = propertyTaxDebtRequestParams.PropertyTaxCode,
			["PropertyType"] = propertyTaxDebtRequestParams.PropertyType,
			["PropertySearchMethod"] = propertyTaxDebtRequestParams.PropertySearchMethod
		};
		return (await client.PostAsync<ResponseResult<List<PropertyTaxDebt>>>("propertyTaxDebts", requireAuthorization: true, null, null, postBody)).Value;
	}
}
