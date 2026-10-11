using System;
using System.Collections.Generic;
using System.IO;
using ArmSoft.SME.Client;
using ArmSoft.SME.Common.Models.Attachments;

namespace ArmSoft.SME.Core.UI.Services;

public class AttachmentsService : IAttachmentsService
{
	private static ApiClient Client => Settings.CreateApiClient();

	public List<FileRow> GetFiles(Guid docISN)
	{
		return Client.Attachments.GetFiles(docISN);
	}

	public List<LinkRow> GetLinks(Guid docISN)
	{
		return Client.Attachments.GetLinks(docISN);
	}

	public void StoreFiles(AttachmentRequestModel attachment)
	{
		Client.Attachments.StoreFiles(attachment);
	}

	public void StoreLinks(AttachmentRequestModel attachment)
	{
		Client.Attachments.StoreLinks(attachment);
	}

	public void ExportFromDBToFile(Guid docISN, string fileNameWithPath, string fileName)
	{
		byte[] bytes = Client.Attachments.ExportFromDBToFile(docISN, fileName);
		File.WriteAllBytes(fileNameWithPath, bytes);
	}

	public void ImportFromFileToDB(Guid docISN, string fileName, byte[] file)
	{
		Client.Attachments.ImportFromFileToDB(docISN, fileName, file);
	}

	public DateTime GetLastModifyDate(Guid docISN)
	{
		return Client.Attachments.GetLastModifyDate(docISN);
	}
}
