using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ArmSoft.InternetBank.Web.API.Common.Models;
using ArmSoft.OnlineBanking.Common.Enums;

namespace ArmSoft.InternetBank.APIClient;

public class UserSettingsManager : BaseManager
{
	internal UserSettingsManager(Client client)
		: base(client)
	{
	}

	public List<AuditLog> GetUserActions(Period period = Period.Custom, DateTime? dateFrom = null, DateTime? dateTo = null)
	{
		Dictionary<string, object> queryParams = new Dictionary<string, object>
		{
			["Period"] = period,
			["DateFrom"] = dateFrom,
			["DateTo"] = dateTo
		};
		return client.Get<List<AuditLog>>("auditlog", requireAuthorization: true, null, queryParams);
	}

	public async Task<List<AuditLog>> GetUserActionsAsync(Period period = Period.Custom, DateTime? dateFrom = null, DateTime? dateTo = null)
	{
		Dictionary<string, object> queryParams = new Dictionary<string, object>
		{
			["Period"] = period,
			["DateFrom"] = dateFrom,
			["DateTo"] = dateTo
		};
		return await client.GetAsync<List<AuditLog>>("auditlog", requireAuthorization: true, null, queryParams);
	}

	public async Task<object> GetUserDefaultAccountAsync()
	{
		return await client.GetAsync<object>("userParams/getDefaultAccount", requireAuthorization: true);
	}

	public async Task SetUserDefaultAccountAsync(string accountNumber)
	{
		await client.PostAsync("userParams/updateDefaultAccount", requireAuthorization: true, null, null, accountNumber);
	}
}
