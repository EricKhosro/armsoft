using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ArmSoft.InternetBank.APIClient.Core;
using ArmSoft.InternetBank.Web.API.Common.Models;

namespace ArmSoft.InternetBank.APIClient;

public class AswaManager : BaseManager
{
	internal AswaManager(Client client)
		: base(client)
	{
	}

	public async Task<VehicleAswaInformation> SearchVehicle(string plateNumber, string ownerIdNumber)
	{
		Dictionary<string, object> postBody = new Dictionary<string, object>
		{
			["PlateNumber"] = plateNumber,
			["OwnerIdNumber"] = ownerIdNumber
		};
		return (await client.PostAsync<ResponseResult<VehicleAswaInformation>>("aswa/searchVehicle", requireAuthorization: true, null, null, postBody)).Value;
	}

	public async Task<List<AswaVehicleUseType>> GetVehicleUseTypes()
	{
		return (await client.GetAsync<ResponseResult<List<AswaVehicleUseType>>>("aswa/getVehicleUseTypes", requireAuthorization: true)).Value;
	}

	public async Task<List<AswaInsuranceOffer>> GetInsuranceOffers(int vehicleUseTypeId, string phoneNumber, string email, DateTime startDate, int durationInMonths, string accountNumber, string token)
	{
		Dictionary<string, object> postBody = new Dictionary<string, object>
		{
			["VehicleUseTypeId"] = vehicleUseTypeId,
			["PhoneNumber"] = phoneNumber,
			["Email"] = email,
			["StartDate"] = startDate,
			["DurationInMonths"] = durationInMonths,
			["DefaultAccount"] = accountNumber,
			["Token"] = token
		};
		return (await client.PostAsync<ResponseResult<List<AswaInsuranceOffer>>>("aswa/getInsuranceOffers", requireAuthorization: true, null, null, postBody)).Value;
	}
}
