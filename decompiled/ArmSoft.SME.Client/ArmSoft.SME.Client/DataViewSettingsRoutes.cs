using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ArmSoft.SME.Common.Models.Data;

namespace ArmSoft.SME.Client;

public class DataViewSettingsRoutes
{
	private readonly ApiClient _client;

	internal DataViewSettingsRoutes(ApiClient client)
	{
		_client = client;
	}

	public Task StoreAsync(DataViewSetting value, CancellationToken cancellationToken = default(CancellationToken))
	{
		StringBuilder stringBuilder = _client.CreateURLBuilder("/api/DataViewSettings");
		return _client.ExecuteAsync(HttpMethod.Post, stringBuilder.ToString(), cancellationToken, value);
	}

	public void Store(DataViewSetting value)
	{
		AsyncHelper.RunSync(async () =>
		{
			await StoreAsync(value);
		});
	}

	public Task DeleteAsync(DataViewSetting value, CancellationToken cancellationToken = default(CancellationToken))
	{
		StringBuilder stringBuilder = _client.CreateURLBuilder("/api/DataViewSettings");
		return _client.ExecuteAsync(HttpMethod.Delete, stringBuilder.ToString(), cancellationToken, value);
	}

	public void Delete(DataViewSetting value)
	{
		AsyncHelper.RunSync(async () =>
		{
			await DeleteAsync(value);
		});
	}

	public Task RemoveAllAsync(string viewName, string groupName, CancellationToken cancellationToken = default(CancellationToken))
	{
		UrlQuery urlQuery = new UrlQuery();
		urlQuery.Add("viewName", viewName);
		urlQuery.Add("groupName", groupName);
		StringBuilder stringBuilder = _client.CreateURLBuilder("/api/DataViewSettings/RemoveAll", urlQuery);
		return _client.ExecuteAsync(HttpMethod.Delete, stringBuilder.ToString(), cancellationToken);
	}

	public void RemoveAll(string viewName, string groupName)
	{
		AsyncHelper.RunSync(async () =>
		{
			await RemoveAllAsync(viewName, groupName);
		});
	}

	public Task<DataViewSetting> LoadAsync(string viewName, string groupName, short id, CancellationToken cancellationToken = default(CancellationToken))
	{
		UrlQuery urlQuery = new UrlQuery();
		urlQuery.Add("viewName", viewName);
		urlQuery.Add("groupName", groupName);
		urlQuery.Add("id", id);
		StringBuilder stringBuilder = _client.CreateURLBuilder("/api/DataViewSettings", urlQuery);
		return _client.ExecuteAsync<DataViewSetting>(HttpMethod.Get, stringBuilder.ToString(), cancellationToken);
	}

	public DataViewSetting Load(string viewName, string groupName, short id)
	{
		return AsyncHelper.RunSync(async () => await LoadAsync(viewName, groupName, id));
	}

	public Task<List<DataViewSetting>> GetListAsync(string viewName, string groupName, CancellationToken cancellationToken = default(CancellationToken))
	{
		UrlQuery urlQuery = new UrlQuery();
		urlQuery.Add("viewName", viewName);
		urlQuery.Add("groupName", groupName);
		StringBuilder stringBuilder = _client.CreateURLBuilder("/api/DataViewSettings/GetList", urlQuery);
		return _client.ExecuteAsync<List<DataViewSetting>>(HttpMethod.Get, stringBuilder.ToString(), cancellationToken);
	}

	public List<DataViewSetting> GetList(string viewName, string groupName)
	{
		return AsyncHelper.RunSync(async () => await GetListAsync(viewName, groupName));
	}
}
