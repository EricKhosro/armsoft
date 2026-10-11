using System;

namespace ArmSoft.SME.Common.Models.Attachments;

public class AttachmentFileImportRequestModel
{
	public Guid ISN { get; set; }

	public string FileName { get; set; }

	public byte[] File { get; set; }
}
