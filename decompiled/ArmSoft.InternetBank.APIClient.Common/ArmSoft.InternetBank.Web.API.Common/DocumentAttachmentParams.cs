using System;

namespace ArmSoft.InternetBank.Web.API.Common;

public class DocumentAttachmentParams
{
	public Guid DocumentId { get; set; }

	public string FieldName { get; set; }
}
