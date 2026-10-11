using ArmSoft.OnlineBanking.Common.Enums;

namespace ArmSoft.OpenBanking.Api.Common;

public class CreateDocumentParams<TModel> : CreateSignableObjectParams<TModel> where TModel : Document
{
	public bool SaveDocument { get; set; } = true;

	public bool SaveTemplate { get; set; }

	public AttachmentAction AttachmentAction { get; set; }

	public byte[]? Attachment { get; set; }
}
