using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ArmSoft.SME.Common.Models.UnboundColumns;

namespace ArmSoft.SME.Client.Routes;

public class UnboundColumnsRoutes
{
	private readonly ApiClient _client;

	internal UnboundColumnsRoutes(ApiClient client)
	{
		_client = client;
	}

	public Task<UnboundColumn> GetAsync(string group, string name, CancellationToken cancellationToken = default(CancellationToken))
	{
		UrlQuery urlQuery = new UrlQuery();
		urlQuery.Add("group", group);
		urlQuery.Add("name", name);
		StringBuilder stringBuilder = _client.CreateURLBuilder("/api/UnboundColumns", urlQuery);
		return _client.ExecuteAsync<UnboundColumn>(HttpMethod.Get, stringBuilder.ToString(), cancellationToken);
	}

	public UnboundColumn Get(string group, string name)
	{
		return AsyncHelper.RunSync(async () => await GetAsync(group, name));
	}

	public Task<List<UnboundColumn>> GetListAsync(string group, CancellationToken cancellationToken = default(CancellationToken))
	{
		UrlQuery urlQuery = new UrlQuery();
		urlQuery.Add("group", group);
		StringBuilder stringBuilder = _client.CreateURLBuilder("/api/UnboundColumns/GetList", urlQuery);
		return _client.ExecuteAsync<List<UnboundColumn>>(HttpMethod.Get, stringBuilder.ToString(), cancellationToken);
	}

	public List<UnboundColumn> GetList(string group)
	{
		return AsyncHelper.RunSync(async () => await GetListAsync(group));
	}

	public Task<bool> StoreAsync(UnboundColumn column, CancellationToken cancellationToken = default(CancellationToken))
	{
		StringBuilder stringBuilder = _client.CreateURLBuilder("/api/UnboundColumns");
		return _client.ExecuteAsync<bool>(HttpMethod.Post, stringBuilder.ToString(), cancellationToken, column);
	}

	public bool Store(UnboundColumn column)
	{
		return AsyncHelper.RunSync(async () => await StoreAsync(column));
	}

	public Task<bool> DeleteAsync(UnboundColumn column, CancellationToken cancellationToken = default(CancellationToken))
	{
		StringBuilder stringBuilder = _client.CreateURLBuilder("/api/UnboundColumns");
		return _client.ExecuteAsync<bool>(HttpMethod.Delete, stringBuilder.ToString(), cancellationToken, column);
	}

	public bool Delete(UnboundColumn column)
	{
		return AsyncHelper.RunSync(async () => await DeleteAsync(column));
	}
}
