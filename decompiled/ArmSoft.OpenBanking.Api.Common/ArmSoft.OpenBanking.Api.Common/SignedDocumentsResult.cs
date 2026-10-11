namespace ArmSoft.OpenBanking.Api.Common;

public class SignedDocumentsResult
{
	public int IgnoredDocumentsCount { get; set; }

	public string IgnoredDocumentsMessage { get; set; } = string.Empty;
}
