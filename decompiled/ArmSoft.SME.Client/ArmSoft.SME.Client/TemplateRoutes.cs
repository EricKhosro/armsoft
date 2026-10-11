using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ArmSoft.SME.Common.Models;
using ArmSoft.SME.Common.Models.Caches;
using ArmSoft.SME.Common.Models.Directories;

namespace ArmSoft.SME.Client;

public class TemplateRoutes
{
	private readonly ApiClient _client;

	protected virtual string ControllerName => "Template";

	public TemplateRoutes(ApiClient client)
	{
		_client = client;
	}

	public Task<Template> LoadAsync(string code, CancellationToken cancellationToken = default(CancellationToken))
	{
		UrlQuery urlQuery = new UrlQuery();
		urlQuery.Add("code", code);
		StringBuilder stringBuilder = _client.CreateURLBuilder("/api/" + ControllerName + "/Load", urlQuery);
		return _client.ExecuteAsync<Template>(HttpMethod.Get, stringBuilder.ToString(), cancellationToken);
	}

	public Template Load(string code)
	{
		return AsyncHelper.RunSync(async () => await LoadAsync(code));
	}

	public Task<byte[]> ExportFileAsync(string name, string templateType, CancellationToken cancellationToken = default(CancellationToken))
	{
		UrlQuery urlQuery = new UrlQuery();
		urlQuery.Add("name", name);
		urlQuery.Add("templateType", templateType);
		StringBuilder stringBuilder = _client.CreateURLBuilder("/api/" + ControllerName + "/ExportFile", urlQuery);
		return _client.ExecuteAsync<byte[]>(HttpMethod.Put, stringBuilder.ToString(), cancellationToken);
	}

	public byte[] ExportFile(string name, string templateType)
	{
		return AsyncHelper.RunSync(async () => await ExportFileAsync(name, templateType));
	}

	public void ImportFile(string name, string templateType, string fileName, byte[] fileContent)
	{
		UrlQuery urlQuery = new UrlQuery();
		urlQuery.Add("name", name);
		urlQuery.Add("templateType", templateType);
		FileImportRequest requestObject = new FileImportRequest
		{
			Name = fileName,
			Content = fileContent
		};
		StringBuilder stringBuilder = _client.CreateURLBuilder("/api/" + ControllerName + "/ImportFile", urlQuery);
		_client.Execute(HttpMethod.Post, stringBuilder.ToString(), requestObject);
	}

	public Task<List<TemplateCacheRowShort>> GetTemplatesCacheAsync(short docType, string fileExtension = null, CancellationToken cancellationToken = default(CancellationToken))
	{
		UrlQuery urlQuery = new UrlQuery();
		urlQuery.Add("docType", docType);
		urlQuery.Add("fileExtension", fileExtension);
		StringBuilder stringBuilder = _client.CreateURLBuilder("/api/" + ControllerName + "/GetTemplatesCache", urlQuery);
		return _client.ExecuteAsync<List<TemplateCacheRowShort>>(HttpMethod.Get, stringBuilder.ToString(), cancellationToken);
	}

	public List<TemplateCacheRowShort> GetTemplatesCache(short docType, string fileExtension = null)
	{
		return AsyncHelper.RunSync(async () => await GetTemplatesCacheAsync(docType, fileExtension));
	}

	public Task<List<TemplateCacheRowShort>> GetTemplatesCacheByDocISNAsync(Guid docISN, string fileExtension = null, CancellationToken cancellationToken = default(CancellationToken))
	{
		UrlQuery urlQuery = new UrlQuery();
		urlQuery.Add("docISN", docISN);
		urlQuery.Add("fileExtension", fileExtension);
		StringBuilder stringBuilder = _client.CreateURLBuilder("/api/" + ControllerName + "/GetTemplatesCacheByDocISN", urlQuery);
		return _client.ExecuteAsync<List<TemplateCacheRowShort>>(HttpMethod.Get, stringBuilder.ToString(), cancellationToken);
	}

	public List<TemplateCacheRowShort> GetTemplatesCacheByDocISN(Guid docISN, string fileExtension = null)
	{
		return AsyncHelper.RunSync(async () => await GetTemplatesCacheByDocISNAsync(docISN, fileExtension));
	}

	public Task CopyFileAsync(string source, string destination, string templateType, CancellationToken cancellationToken = default(CancellationToken))
	{
		UrlQuery urlQuery = new UrlQuery();
		urlQuery.Add("source", source);
		urlQuery.Add("destination", destination);
		urlQuery.Add("templateType", templateType);
		StringBuilder stringBuilder = _client.CreateURLBuilder("/api/" + ControllerName + "/CopyFile", urlQuery);
		return _client.ExecuteAsync(HttpMethod.Put, stringBuilder.ToString(), cancellationToken);
	}

	public void CopyFile(string source, string destination, string templateType)
	{
		AsyncHelper.RunSync(async () =>
		{
			await CopyFileAsync(source, destination, templateType);
		});
	}

	public async Task<Template> StoreAsync(Template request, CancellationToken cancellationToken = default(CancellationToken))
	{
		StringBuilder stringBuilder = _client.CreateURLBuilder("/api/" + ControllerName + "/Store");
		return await _client.ExecuteAsync<Template>(HttpMethod.Post, stringBuilder.ToString(), cancellationToken, request);
	}

	public Template Store(Template request)
	{
		return AsyncHelper.RunSync(async () => await StoreAsync(request));
	}

	public Task DeleteAsync(string code, CancellationToken cancellationToken = default(CancellationToken))
	{
		UrlQuery urlQuery = new UrlQuery();
		urlQuery.Add("code", code);
		StringBuilder stringBuilder = _client.CreateURLBuilder("/api/" + ControllerName + "/Delete", urlQuery);
		return _client.ExecuteAsync(HttpMethod.Delete, stringBuilder.ToString(), cancellationToken);
	}

	public void Delete(string code)
	{
		AsyncHelper.RunSync(async () =>
		{
			await DeleteAsync(code);
		});
	}

	public async Task<byte[]> ExportPrintModelDefinitionAsync(string docType, string fileName, CancellationToken cancellationToken = default(CancellationToken))
	{
		UrlQuery urlQuery = new UrlQuery();
		urlQuery.Add("docType", docType);
		StringBuilder stringBuilder = _client.CreateURLBuilder("/api/" + ControllerName + "/ExportPrintModelDefinition", urlQuery);
		string fileId = await _client.ExecuteAsync<string>(HttpMethod.Get, stringBuilder.ToString(), cancellationToken);
		return await _client.TempFS.DownloadAsync(fileId, cancellationToken);
	}

	public byte[] ExportPrintModelDefinition(string docType, string fileName)
	{
		return AsyncHelper.RunSync(async () => await ExportPrintModelDefinitionAsync(docType, fileName));
	}
}
