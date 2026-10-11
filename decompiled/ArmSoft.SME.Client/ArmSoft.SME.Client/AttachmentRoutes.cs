using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ArmSoft.SME.Common.Models.Attachments;

namespace ArmSoft.SME.Client;

public class AttachmentRoutes
{
	private readonly ApiClient _client;

	internal AttachmentRoutes(ApiClient client)
	{
		_client = client;
	}

	public Task<List<FileRow>> GetFilesAsync(Guid docISN, CancellationToken cancellationToken = default(CancellationToken))
	{
		UrlQuery urlQuery = new UrlQuery();
		urlQuery.Add("docISN", docISN);
		StringBuilder stringBuilder = _client.CreateURLBuilder("/api/Attachment/Files", urlQuery);
		return _client.ExecuteAsync<List<FileRow>>(HttpMethod.Get, stringBuilder.ToString(), cancellationToken);
	}

	public List<FileRow> GetFiles(Guid docISN)
	{
		return AsyncHelper.RunSync(async () => await GetFilesAsync(docISN));
	}

	public Task<List<LinkRow>> GetLinksAsync(Guid docISN, CancellationToken cancellationToken = default(CancellationToken))
	{
		UrlQuery urlQuery = new UrlQuery();
		urlQuery.Add("docISN", docISN);
		StringBuilder stringBuilder = _client.CreateURLBuilder("/api/Attachment/Links", urlQuery);
		return _client.ExecuteAsync<List<LinkRow>>(HttpMethod.Get, stringBuilder.ToString(), cancellationToken);
	}

	public List<LinkRow> GetLinks(Guid docISN)
	{
		return AsyncHelper.RunSync(async () => await GetLinksAsync(docISN));
	}

	public Task StoreFilesAsync(AttachmentRequestModel attachments, CancellationToken cancellationToken = default(CancellationToken))
	{
		StringBuilder stringBuilder = _client.CreateURLBuilder("/api/Attachment/Files");
		return _client.ExecuteAsync(HttpMethod.Post, stringBuilder.ToString(), cancellationToken, attachments);
	}

	public void StoreFiles(AttachmentRequestModel attachments)
	{
		AsyncHelper.RunSync(async () =>
		{
			await StoreFilesAsync(attachments);
		});
	}

	public Task StoreLinksAsync(AttachmentRequestModel attachments, CancellationToken cancellationToken = default(CancellationToken))
	{
		StringBuilder stringBuilder = _client.CreateURLBuilder("/api/Attachment/Links");
		return _client.ExecuteAsync(HttpMethod.Post, stringBuilder.ToString(), cancellationToken, attachments);
	}

	public void StoreLinks(AttachmentRequestModel attachments)
	{
		AsyncHelper.RunSync(async () =>
		{
			await StoreLinksAsync(attachments);
		});
	}

	public Task<byte[]> ExportFromDBToFileAsync(Guid docISN, string fileName, CancellationToken cancellationToken = default(CancellationToken))
	{
		UrlQuery urlQuery = new UrlQuery();
		urlQuery.Add("docISN", docISN);
		urlQuery.Add("fileName", fileName);
		StringBuilder stringBuilder = _client.CreateURLBuilder("/api/Attachment/ExportFromDBToFile", urlQuery);
		return _client.ExecuteAsync<byte[]>(HttpMethod.Get, stringBuilder.ToString(), cancellationToken);
	}

	public byte[] ExportFromDBToFile(Guid docISN, string fileName)
	{
		return AsyncHelper.RunSync(async () => await ExportFromDBToFileAsync(docISN, fileName));
	}

	public Task ImportFromFileToDBAsync(Guid docISN, string fileName, byte[] file, CancellationToken cancellationToken = default(CancellationToken))
	{
		AttachmentFileImportRequestModel requestObject = new AttachmentFileImportRequestModel
		{
			ISN = docISN,
			FileName = fileName,
			File = file
		};
		StringBuilder stringBuilder = _client.CreateURLBuilder("/api/Attachment/ImportFromFileToDB");
		return _client.ExecuteAsync(HttpMethod.Post, stringBuilder.ToString(), cancellationToken, requestObject);
	}

	public void ImportFromFileToDB(Guid docISN, string fileName, byte[] file)
	{
		AsyncHelper.RunSync(async () =>
		{
			await ImportFromFileToDBAsync(docISN, fileName, file);
		});
	}

	public Task<DateTime> GetLastModifyDateAsync(Guid docISN, CancellationToken cancellationToken = default(CancellationToken))
	{
		StringBuilder stringBuilder = _client.CreateURLBuilder($"/api/Attachment/GetLastModifyDate/{docISN}");
		return _client.ExecuteAsync<DateTime>(HttpMethod.Get, stringBuilder.ToString(), cancellationToken);
	}

	public DateTime GetLastModifyDate(Guid docISN)
	{
		return AsyncHelper.RunSync(async () => await GetLastModifyDateAsync(docISN));
	}
}
