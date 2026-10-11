using ArmSoft.OnlineBanking.Common.Enums;

namespace ArmSoft.InternetBank.Web.API.Common;

public class DocumentAttachment
{
	public string Name { get; set; }

	public long FileSize { get; set; }

	public byte[] Attachment { get; set; }

	public AttachmentAction Action { get; set; }
}
