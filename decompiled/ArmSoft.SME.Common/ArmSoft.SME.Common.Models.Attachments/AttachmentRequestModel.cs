using System;
using System.Collections.Generic;

namespace ArmSoft.SME.Common.Models.Attachments;

public class AttachmentRequestModel
{
	public List<FileRow> FileRows { get; set; }

	public List<LinkRow> LinkRows { get; set; }

	public Guid ISN { get; set; }

	public string Caption { get; set; }
}
