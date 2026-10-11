using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ArmSoft.SME.Common.Models.Layout;

namespace ArmSoft.SME.Client;

public class LayoutRoutes
{
	private readonly ApiClient _client;

	internal LayoutRoutes(ApiClient client)
	{
		_client = client;
	}

	public Task<List<LayoutRow>> GetListAsync(string name, CancellationToken cancellationToken = default(CancellationToken))
	{
		StringBuilder stringBuilder = _client.CreateURLBuilder("/api/Layout?name=" + name);
		return _client.ExecuteAsync<List<LayoutRow>>(HttpMethod.Get, stringBuilder.ToString(), cancellationToken);
	}

	public List<LayoutRow> GetList(string name)
	{
		return AsyncHelper.RunSync(async () => await GetListAsync(name));
	}

	public Task<bool> StoreAsync(LayoutRow layout, CancellationToken cancellationToken = default(CancellationToken))
	{
		StringBuilder stringBuilder = _client.CreateURLBuilder("/api/Layout");
		return _client.ExecuteAsync<bool>(HttpMethod.Post, stringBuilder.ToString(), cancellationToken, layout);
	}

	public bool Store(LayoutRow layout)
	{
		return AsyncHelper.RunSync(async () => await StoreAsync(layout));
	}

	public Task<bool> DeleteAsync(LayoutRow layout, CancellationToken cancellationToken = default(CancellationToken))
	{
		StringBuilder stringBuilder = _client.CreateURLBuilder("/api/Layout");
		return _client.ExecuteAsync<bool>(HttpMethod.Delete, stringBuilder.ToString(), cancellationToken, layout);
	}

	public bool Delete(LayoutRow layout)
	{
		return AsyncHelper.RunSync(async () => await DeleteAsync(layout));
	}

	public Task<bool> EditCaptionAsync(LayoutRow layout, string caption, CancellationToken cancellationToken = default(CancellationToken))
	{
		UrlQuery urlQuery = new UrlQuery();
		urlQuery.Add("caption", caption);
		StringBuilder stringBuilder = _client.CreateURLBuilder("/api/Layout/EditCaption", urlQuery);
		return _client.ExecuteAsync<bool>(HttpMethod.Post, stringBuilder.ToString(), cancellationToken, layout);
	}

	public bool EditCaption(LayoutRow layout, string caption)
	{
		return AsyncHelper.RunSync(async () => await EditCaptionAsync(layout, caption));
	}

	public Task<string> GetBodyAsync(LayoutRow layout, CancellationToken cancellationToken = default(CancellationToken))
	{
		StringBuilder stringBuilder = _client.CreateURLBuilder("/api/Layout/Body");
		return _client.ExecuteAsync<string>(HttpMethod.Post, stringBuilder.ToString(), cancellationToken, layout);
	}

	public string GetBody(LayoutRow layout)
	{
		return AsyncHelper.RunSync(async () => await GetBodyAsync(layout));
	}

	public bool IsDeleteLayoutRow(string name, string caption)
	{
		UrlQuery urlQuery = new UrlQuery();
		urlQuery.Add("name", name);
		urlQuery.Add("caption", caption);
		StringBuilder stringBuilder = _client.CreateURLBuilder("/api/Layout/IsDeleteLayoutRow", urlQuery);
		return _client.Execute<bool>(HttpMethod.Get, stringBuilder.ToString());
	}

	public bool ExistsLayoutRow(string name, string caption)
	{
		UrlQuery urlQuery = new UrlQuery();
		urlQuery.Add("name", name);
		urlQuery.Add("caption", caption);
		StringBuilder stringBuilder = _client.CreateURLBuilder("/api/Layout/ExistsLayoutRow", urlQuery);
		return _client.Execute<bool>(HttpMethod.Get, stringBuilder.ToString());
	}
}
