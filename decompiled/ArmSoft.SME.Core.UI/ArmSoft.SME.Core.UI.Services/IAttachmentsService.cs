using System;
using System.Collections.Generic;
using ArmSoft.SME.Common.Models.Attachments;

namespace ArmSoft.SME.Core.UI.Services;

public interface IAttachmentsService
{
	List<FileRow> GetFiles(Guid docISN);

	List<LinkRow> GetLinks(Guid docISN);

	void StoreFiles(AttachmentRequestModel attachment);

	void StoreLinks(AttachmentRequestModel attachment);

	void ExportFromDBToFile(Guid docISN, string fileNameWithPath, string fileName);

	DateTime GetLastModifyDate(Guid docISN);

	void ImportFromFileToDB(Guid docISN, string fileName, byte[] file);
}
