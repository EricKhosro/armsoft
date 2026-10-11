using ArmSoft.OnlineBanking.Common.Enums;

namespace ArmSoft.OpenBanking.Api.Common;

public class DocumentDetails<TModel> : SignableObjectDetails<TModel> where TModel : Document
{
	public string Type { get; set; } = string.Empty;

	public string DisplayType { get; set; } = string.Empty;

	public string DisplayStateDetailed { get; set; } = string.Empty;

	public CancelRequestState CancelRequestState { get; set; }

	public bool CanBeCanceled { get; set; }

	public string DisplaySubState { get; set; } = string.Empty;
}
