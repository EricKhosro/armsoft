using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using ArmSoft.SME.Client.JsonSerialization;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Models.Caches;

namespace ArmSoft.SME.Client;

public class CacheRoutes
{
	private readonly ApiClient _client;

	private readonly JsonSerializerOptions _options;

	protected virtual string ControllerName => "Cache";

	public CacheRoutes(ApiClient client)
	{
		_client = client;
		_options = JsonSerializerFactory.CloneJsonOptions();
		_options.TypeInfoResolver = new DirectoryTypeResolver();
		UpdateJsonSerializerOptions(_options);
	}

	protected virtual void UpdateJsonSerializerOptions(JsonSerializerOptions options)
	{
	}

	public List<short> GetChangedCaches(ChangedCachesRequest request)
	{
		StringBuilder stringBuilder = _client.CreateURLBuilder("/api/" + ControllerName + "/GetChangedCaches");
		return _client.Execute<List<short>>(HttpMethod.Post, stringBuilder.ToString(), request);
	}

	public byte[] GetCachesMetadataFile(List<short> cacheIds)
	{
		StringBuilder stringBuilder = _client.CreateURLBuilder("/api/" + ControllerName + "/GetMetadataFileId");
		string fileId = _client.Execute<string>(HttpMethod.Post, stringBuilder.ToString(), cacheIds);
		return _client.TempFS.Download(fileId);
	}

	public byte[] GetCachesMetadataZipFile(List<short> cacheIds)
	{
		StringBuilder stringBuilder = _client.CreateURLBuilder("/api/" + ControllerName + "/GetMetadataZipFileId");
		string fileId = _client.Execute<string>(HttpMethod.Post, stringBuilder.ToString(), cacheIds);
		return _client.TempFS.Download(fileId);
	}

	public async Task<CacheResponse<T>> RefreshAsync<T>(CacheRequest request, CancellationToken cancellationToken = default(CancellationToken)) where T : CacheRowBase, new()
	{
		StringBuilder stringBuilder = _client.CreateURLBuilder("/api/" + ControllerName + "/Refresh");
		CacheResponse cacheResponse = await _client.ExecuteAsync<CacheResponse>(HttpMethod.Post, stringBuilder.ToString(), cancellationToken, request);
		if (cacheResponse != null)
		{
			return new CacheResponse<T>
			{
				TS = cacheResponse.TS,
				RowsMaxTS = cacheResponse.RowsMaxTS,
				Delete = cacheResponse.Delete,
				Merge = JsonSerializer.Deserialize<List<T>>(cacheResponse.MergeJSON, _options)
			};
		}
		return null;
	}

	public CacheResponse<T> Refresh<T>(CacheRequest request) where T : CacheRowBase, new()
	{
		return AsyncHelper.RunSync(async () => await RefreshAsync<T>(request));
	}

	public Task RefreshServiceCachesAsync(CancellationToken cancellationToken = default(CancellationToken))
	{
		StringBuilder stringBuilder = _client.CreateURLBuilder("/api/" + ControllerName + "/RefreshServiceCaches");
		return _client.ExecuteAsync(HttpMethod.Post, stringBuilder.ToString(), cancellationToken);
	}

	public void RefreshServiceCaches()
	{
		AsyncHelper.RunSync(async () =>
		{
			await RefreshServiceCachesAsync();
		});
	}

	public void ReloadServiceCaches()
	{
		StringBuilder stringBuilder = _client.CreateURLBuilder("/api/" + ControllerName + "/ReloadServiceCaches");
		_client.Execute(HttpMethod.Post, stringBuilder.ToString());
	}
}
