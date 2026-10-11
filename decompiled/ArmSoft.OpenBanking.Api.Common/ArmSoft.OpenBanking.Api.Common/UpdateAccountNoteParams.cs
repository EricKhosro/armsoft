namespace ArmSoft.OpenBanking.Api.Common;

public class UpdateAccountNoteParams
{
	public int Id { get; set; }

	public required string Note { get; set; }
}
