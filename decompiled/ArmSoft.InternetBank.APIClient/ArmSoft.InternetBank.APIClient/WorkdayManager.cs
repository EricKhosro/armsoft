using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ArmSoft.InternetBank.APIClient.Core;
using ArmSoft.InternetBank.Web.API.Common.Models;

namespace ArmSoft.InternetBank.APIClient;

public class WorkdayManager : BaseManager
{
	internal WorkdayManager(Client client)
		: base(client)
	{
	}

	public async Task<Workday> GetNextWorkDayAsync(DateTime date)
	{
		Dictionary<string, object> postBody = new Dictionary<string, object> { ["Date"] = date };
		return (await client.PostAsync<ResponseResult<Workday>>("workday/nextWorkDay", requireAuthorization: true, null, null, postBody)).Value;
	}

	public async Task<Workday> GetNextWorkDayWithoutTimeAsync(DateTime date, bool considerCurrentDay)
	{
		Dictionary<string, object> postBody = new Dictionary<string, object>
		{
			["Date"] = date,
			["ConsiderCurrentDay"] = considerCurrentDay
		};
		return (await client.PostAsync<ResponseResult<Workday>>("workday/nextWorkDayWithoutTime", requireAuthorization: true, null, null, postBody)).Value;
	}
}
