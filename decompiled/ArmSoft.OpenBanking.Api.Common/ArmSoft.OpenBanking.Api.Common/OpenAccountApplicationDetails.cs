namespace ArmSoft.OpenBanking.Api.Common;

public class OpenAccountApplicationDetails : DocumentDetails<OpenAccountApplication>
{
	public string DisplayAccountTypeName { get; set; } = string.Empty;
}
