using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ArmSoft.SME.Client;

public class CommonCoreRoutes
{
	private readonly ApiClient client;

	protected virtual string ControllerName => "CommonCore";

	public CommonCoreRoutes(ApiClient client)
	{
		this.client = client;
	}

	public Task DeleteDbConnectionAsync(short id, CancellationToken cancellationToken = default(CancellationToken))
	{
		StringBuilder stringBuilder = client.CreateURLBuilder($"/api/{ControllerName}/DeleteDbConnection/{id}");
		return client.ExecuteAsync(HttpMethod.Delete, stringBuilder.ToString(), cancellationToken);
	}

	public void DeleteDbConnection(short id)
	{
		AsyncHelper.RunSync(async () =>
		{
			await DeleteDbConnectionAsync(id);
		});
	}

	public Task<List<int>> GetDatabaseUsersAsync(CancellationToken cancellationToken = default(CancellationToken))
	{
		StringBuilder stringBuilder = client.CreateURLBuilder("/api/" + ControllerName + "/GetDatabaseUsers/");
		return client.ExecuteAsync<List<int>>(HttpMethod.Get, stringBuilder.ToString(), cancellationToken);
	}

	public List<int> GetDatabaseUsers()
	{
		return AsyncHelper.RunSync(async () => await GetDatabaseUsersAsync());
	}

	public Task CreateStoredProceduresAsync(CancellationToken cancellationToken = default(CancellationToken))
	{
		StringBuilder stringBuilder = client.CreateURLBuilder("/api/" + ControllerName + "/CreateStoredProcedures/");
		return client.ExecuteAsync(HttpMethod.Post, stringBuilder.ToString(), cancellationToken);
	}

	public void CreateStoredProcedures()
	{
		AsyncHelper.RunSync(async () =>
		{
			await CreateStoredProceduresAsync();
		});
	}
}
