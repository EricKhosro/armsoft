using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using ArmSoft.SME.Client.JsonSerialization;
using ArmSoft.SME.Client.Models;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Models.Exchange;

namespace ArmSoft.SME.Client;

public class ExchangeRoutes
{
	private readonly ApiClient _client;

	private readonly JsonSerializerOptions _options;

	protected virtual string ControllerName => "Exchange";

	public ExchangeRoutes(ApiClient client)
	{
		_client = client;
		_options = JsonSerializerFactory.CloneJsonOptions();
		_options.TypeInfoResolver = new ExchangeTypeResolver();
		UpdateJsonSerializerOptions(_options);
	}

	protected virtual void UpdateJsonSerializerOptions(JsonSerializerOptions options)
	{
	}

	public Task<ExchangeEntityInfo> GetEntityInfoAsync(string entityName, CancellationToken cancellationToken = default(CancellationToken))
	{
		StringBuilder stringBuilder = _client.CreateURLBuilder("/api/" + ControllerName + "/EntityInfo/" + entityName);
		return _client.ExecuteAsync<ExchangeEntityInfo>(HttpMethod.Get, stringBuilder.ToString(), cancellationToken);
	}

	public ExchangeEntityInfo GetEntityInfo(string entityName)
	{
		return AsyncHelper.RunSync(async () => await GetEntityInfoAsync(entityName));
	}

	public Task<ExchangeResponse> ImportAsync(ExchangeRequest request, CancellationToken cancellationToken = default(CancellationToken))
	{
		StringBuilder stringBuilder = _client.CreateURLBuilder("/api/Exchange");
		return _client.ExecuteAsync<ExchangeResponse>(HttpMethod.Post, stringBuilder.ToString(), cancellationToken, request);
	}

	public ExchangeResponse Import(ExchangeRequest request)
	{
		return AsyncHelper.RunSync(async () => await ImportAsync(request));
	}

	public Task<ExchangeExportResponse> ExportAllDirectoryAsync(short dirType, CancellationToken cancellationToken = default(CancellationToken))
	{
		StringBuilder stringBuilder = _client.CreateURLBuilder($"/api/{ControllerName}/ExportAllDirectory/{dirType}");
		return _client.ExecuteAsync<ExchangeExportResponse>(HttpMethod.Get, stringBuilder.ToString(), cancellationToken);
	}

	public ExchangeExportResponse ExportAllDirectory(short dirType)
	{
		return AsyncHelper.RunSync(async () => await ExportAllDirectoryAsync(dirType));
	}

	public async Task<byte[]> ExportDirectoryAsync(short dirType, List<string> keys, CancellationToken cancellationToken = default(CancellationToken))
	{
		StringBuilder stringBuilder = _client.CreateURLBuilder($"/api/{ControllerName}/ExportDirectory/{dirType}");
		DirectoryExchangeRequest requestObject = new DirectoryExchangeRequest
		{
			Keys = keys
		};
		string fileId = await _client.ExecuteAsync<string>(HttpMethod.Post, stringBuilder.ToString(), cancellationToken, requestObject);
		return await _client.TempFS.DownloadAsync(fileId, cancellationToken);
	}

	public byte[] ExportDirectory(short dirType, List<string> keys)
	{
		return AsyncHelper.RunSync(async () => await ExportDirectoryAsync(dirType, keys));
	}

	public async Task<byte[]> ExportDirectoryAsync(short dirType, List<int> ids = null, CancellationToken cancellationToken = default(CancellationToken))
	{
		StringBuilder stringBuilder = _client.CreateURLBuilder($"/api/{ControllerName}/ExportDirectory/{dirType}");
		DirectoryExchangeRequest requestObject = new DirectoryExchangeRequest
		{
			Ids = ids
		};
		string fileId = await _client.ExecuteAsync<string>(HttpMethod.Post, stringBuilder.ToString(), cancellationToken, requestObject);
		return await _client.TempFS.DownloadAsync(fileId, cancellationToken);
	}

	public byte[] ExportDirectory(short dirType, List<int> ids = null)
	{
		return AsyncHelper.RunSync(async () => await ExportDirectoryAsync(dirType, ids));
	}

	public async Task<byte[]> ExportDocumentAsync(short docType, Guid docISN, CancellationToken cancellationToken = default(CancellationToken))
	{
		List<DocumentExchangeRow> documentExchanges = new List<DocumentExchangeRow>(1)
		{
			new DocumentExchangeRow
			{
				DocType = docType,
				ISN = docISN
			}
		};
		return await ExportDocumentAsync(documentExchanges, cancellationToken);
	}

	public byte[] ExportDocument(short docType, Guid docISN)
	{
		return AsyncHelper.RunSync(async () => await ExportDocumentAsync(docType, docISN));
	}

	public async Task<byte[]> ExportDocumentAsync(List<DocumentExchangeRow> documentExchanges, CancellationToken cancellationToken = default(CancellationToken))
	{
		StringBuilder stringBuilder = _client.CreateURLBuilder("/api/" + ControllerName + "/ExportDocument");
		DocumentExchangeRequest requestObject = new DocumentExchangeRequest
		{
			DocumentExchanges = documentExchanges.ToList()
		};
		string fileId = await _client.ExecuteAsync<string>(HttpMethod.Post, stringBuilder.ToString(), cancellationToken, requestObject);
		return await _client.TempFS.DownloadAsync(fileId, cancellationToken);
	}

	public byte[] ExportDocument(List<DocumentExchangeRow> request)
	{
		return AsyncHelper.RunSync(async () => await ExportDocumentAsync(request));
	}

	public async Task<byte[]> ExportAsync(ExchangeBase exchange, CancellationToken cancellationToken = default(CancellationToken))
	{
		ExchangeRequest requestObject = new ExchangeRequest
		{
			Entity = JsonSerializer.Serialize(exchange, _options)
		};
		StringBuilder stringBuilder = _client.CreateURLBuilder("/api/" + ControllerName + "/Export");
		string fileId = await _client.ExecuteAsync<string>(HttpMethod.Post, stringBuilder.ToString(), cancellationToken, requestObject);
		return await _client.TempFS.DownloadAsync(fileId, cancellationToken);
	}

	public byte[] Export(ExchangeBase exchange)
	{
		return AsyncHelper.RunSync(async () => await ExportAsync(exchange));
	}

	public async Task<byte[]> ExportAsync(List<ExchangeBase> exchange, CancellationToken cancellationToken = default(CancellationToken))
	{
		ExchangeListRequest exchangeListRequest = new ExchangeListRequest
		{
			Entity = new List<string>()
		};
		foreach (ExchangeBase item in exchange)
		{
			exchangeListRequest.Entity.Add(JsonSerializer.Serialize(item, _options));
		}
		StringBuilder stringBuilder = _client.CreateURLBuilder("/api/" + ControllerName + "/ExportList");
		string fileId = await _client.ExecuteAsync<string>(HttpMethod.Post, stringBuilder.ToString(), cancellationToken, exchangeListRequest);
		return await _client.TempFS.DownloadAsync(fileId, cancellationToken);
	}

	public byte[] Export(List<ExchangeBase> exchange)
	{
		return AsyncHelper.RunSync(async () => await ExportAsync(exchange));
	}

	public Task<ExchangeExportResponse> ExportAllTreeNodeAsync(string treeId, CancellationToken cancellationToken = default(CancellationToken))
	{
		StringBuilder stringBuilder = _client.CreateURLBuilder("/api/" + ControllerName + "/ExportAllTreeNode/" + treeId);
		return _client.ExecuteAsync<ExchangeExportResponse>(HttpMethod.Get, stringBuilder.ToString(), cancellationToken);
	}

	public ExchangeExportResponse ExportAllTreeNode(string treeId)
	{
		return AsyncHelper.RunSync(async () => await ExportAllTreeNodeAsync(treeId));
	}

	public async Task<byte[]> ExportTreeNodeAsync(string treeId, List<string> codes, CancellationToken cancellationToken = default(CancellationToken))
	{
		StringBuilder stringBuilder = _client.CreateURLBuilder("/api/" + ControllerName + "/ExportTreeNode/" + treeId);
		TreeNodeExchangeRequest requestObject = new TreeNodeExchangeRequest
		{
			Codes = codes
		};
		string fileId = await _client.ExecuteAsync<string>(HttpMethod.Post, stringBuilder.ToString(), cancellationToken, requestObject);
		return await _client.TempFS.DownloadAsync(fileId, cancellationToken);
	}

	public byte[] ExportTreeNode(string treeId, List<string> codes)
	{
		return AsyncHelper.RunSync(async () => await ExportTreeNodeAsync(treeId, codes));
	}
}
