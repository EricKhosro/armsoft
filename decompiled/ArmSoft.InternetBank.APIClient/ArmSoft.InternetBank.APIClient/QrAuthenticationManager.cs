using System.Threading.Tasks;
using ArmSoft.InternetBank.APIClient.Core;
using ArmSoft.InternetBank.Web.API.Common;
using ArmSoft.InternetBank.Web.API.Common.Models;

namespace ArmSoft.InternetBank.APIClient;

public class QrAuthenticationManager
{
	protected readonly ClientBase client;

	internal QrAuthenticationManager(ClientBase client)
	{
		this.client = client;
	}

	public QrAuthenticationData Generate()
	{
		return client.Post<QrAuthenticationData>("qrAuthentication/generate", requireAuthorization: false);
	}

	public async Task<QrAuthenticationData> GenerateAsync()
	{
		return await client.PostAsync<QrAuthenticationData>("qrAuthentication/generate", requireAuthorization: false);
	}

	public QrAuthenticationCheckResult Check(string authenticationId)
	{
		CheckQrAuthenticationParams postBody = new CheckQrAuthenticationParams
		{
			QrAuthenticationId = authenticationId
		};
		return client.Post<QrAuthenticationCheckResult>("qrAuthentication/check", requireAuthorization: false, null, null, postBody);
	}

	public async Task<QrAuthenticationCheckResult> CheckAsync(string authenticationId)
	{
		CheckQrAuthenticationParams postBody = new CheckQrAuthenticationParams
		{
			QrAuthenticationId = authenticationId
		};
		return await client.PostAsync<QrAuthenticationCheckResult>("qrAuthentication/check", requireAuthorization: false, null, null, postBody);
	}
}
