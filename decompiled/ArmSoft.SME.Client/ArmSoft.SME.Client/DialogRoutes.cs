using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ArmSoft.SME.Common.Models.Dialog;

namespace ArmSoft.SME.Client;

public class DialogRoutes
{
	private readonly ApiClient _client;

	internal DialogRoutes(ApiClient client)
	{
		_client = client;
	}

	public Task<DialogValues> GetValuesAsync(string name, CancellationToken cancellationToken = default(CancellationToken))
	{
		StringBuilder stringBuilder = _client.CreateURLBuilder("/api/Dialog/Values/" + name);
		return _client.ExecuteAsync<DialogValues>(HttpMethod.Get, stringBuilder.ToString(), cancellationToken);
	}

	public DialogValues GetValues(string name)
	{
		return AsyncHelper.RunSync(async () => await GetValuesAsync(name));
	}

	public Task StoreValuesAsync(string name, DialogValues values, CancellationToken cancellationToken = default(CancellationToken))
	{
		StringBuilder stringBuilder = _client.CreateURLBuilder("/api/Dialog/Values/" + name);
		return _client.ExecuteAsync(HttpMethod.Post, stringBuilder.ToString(), cancellationToken, values);
	}

	public void StoreValues(string name, DialogValues values)
	{
		AsyncHelper.RunSync(async () =>
		{
			await StoreValuesAsync(name, values);
		});
	}

	public Task<List<DialogValuesTemplate>> GetTemplatesAsync(string group, bool forCurrentUser, CancellationToken cancellationToken = default(CancellationToken))
	{
		UrlQuery urlQuery = new UrlQuery();
		urlQuery.Add("group", group);
		urlQuery.Add("forCurrentUser", forCurrentUser);
		StringBuilder stringBuilder = _client.CreateURLBuilder("/api/Dialog/Templates", urlQuery);
		return _client.ExecuteAsync<List<DialogValuesTemplate>>(HttpMethod.Get, stringBuilder.ToString(), cancellationToken);
	}

	public List<DialogValuesTemplate> GetTemplates(string group, bool forCurrentUser)
	{
		return AsyncHelper.RunSync(async () => await GetTemplatesAsync(group, forCurrentUser));
	}

	public Task<int> StoreTemplateAsync(DialogValuesTemplateStoreRequest request, CancellationToken cancellationToken = default(CancellationToken))
	{
		StringBuilder stringBuilder = _client.CreateURLBuilder("/api/Dialog/Templates");
		return _client.ExecuteAsync<int>(HttpMethod.Post, stringBuilder.ToString(), cancellationToken, request);
	}

	public int StoreTemplate(DialogValuesTemplateStoreRequest request)
	{
		return AsyncHelper.RunSync(async () => await StoreTemplateAsync(request));
	}

	public Task UpdateTemplatesAsync(DialogValuesTemplatesUpdateRequest request, CancellationToken cancellationToken = default(CancellationToken))
	{
		StringBuilder stringBuilder = _client.CreateURLBuilder("/api/Dialog/Templates/List");
		return _client.ExecuteAsync(HttpMethod.Post, stringBuilder.ToString(), cancellationToken, request);
	}

	public void UpdateTemplates(DialogValuesTemplatesUpdateRequest request)
	{
		AsyncHelper.RunSync(async () =>
		{
			await UpdateTemplatesAsync(request);
		});
	}

	public Task<List<int>> CopyTemplateAsync(int id, IEnumerable<string> userNames, CancellationToken cancellationToken = default(CancellationToken))
	{
		StringBuilder stringBuilder = _client.CreateURLBuilder($"/api/Dialog/Templates/{id}/Copy");
		return _client.ExecuteAsync<List<int>>(HttpMethod.Post, stringBuilder.ToString(), cancellationToken, userNames);
	}

	public List<int> CopyTemplate(int id, IEnumerable<string> userNames)
	{
		return AsyncHelper.RunSync(async () => await CopyTemplateAsync(id, userNames));
	}

	public Task DeleteTemplateAsync(int id, CancellationToken cancellationToken = default(CancellationToken))
	{
		StringBuilder stringBuilder = _client.CreateURLBuilder($"/api/Dialog/Templates/{id}");
		return _client.ExecuteAsync(HttpMethod.Delete, stringBuilder.ToString(), cancellationToken);
	}

	public void DeleteTemplate(int id)
	{
		AsyncHelper.RunSync(async () =>
		{
			await DeleteTemplateAsync(id);
		});
	}

	public Task DeleteTemplatesAsync(IEnumerable<int> ids, CancellationToken cancellationToken = default(CancellationToken))
	{
		StringBuilder stringBuilder = _client.CreateURLBuilder("/api/Dialog/Templates/List");
		return _client.ExecuteAsync(HttpMethod.Delete, stringBuilder.ToString(), cancellationToken, ids);
	}

	public void DeleteTemplates(IEnumerable<int> ids)
	{
		AsyncHelper.RunSync(async () =>
		{
			await DeleteTemplatesAsync(ids);
		});
	}
}
