using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ArmSoft.InternetBank.APIClient.Core;
using ArmSoft.InternetBank.Web.API.Common;
using ArmSoft.InternetBank.Web.API.Common.Models;

namespace ArmSoft.InternetBank.APIClient;

public class MobileAuthenticationManager : BaseManager
{
	internal MobileAuthenticationManager(Client client)
		: base(client)
	{
	}

	public async Task<List<MobileAuthentication>> GetMobileAuthenticationsAsync()
	{
		return (await client.GetAsync<ResponseResult<List<MobileAuthentication>>>("mobileAuthentications", requireAuthorization: true)).Value;
	}

	public async Task DisconnectMobileAuthenticationAsync(Guid deviceId)
	{
		MobileAuthenticationDisconnectParams postBody = new MobileAuthenticationDisconnectParams
		{
			SessionId = deviceId
		};
		await client.PostAsync("mobileAuthentications/disconnect", requireAuthorization: true, null, null, postBody);
	}
}
