using ArmSoft.OnlineBanking.Common.Enums;

namespace ArmSoft.OpenBanking.Api.Common;

public class SignableObjectDetails<TModel> where TModel : SignableObject
{
	public TModel Model { get; set; }

	public string DisplayState { get; set; } = string.Empty;

	public string Comments { get; set; } = string.Empty;

	public DocumentState State { get; set; }

	public long AttachmentSize { get; set; }
}
