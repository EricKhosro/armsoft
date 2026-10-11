namespace ArmSoft.OpenBanking.Api.Common;

public class CreateSignableObjectParams<TModel> where TModel : SignableObject
{
	public required TModel Model { get; set; }

	public bool IgnoreWarnings { get; set; }
}
