namespace ArmSoft.OpenBanking.Api.Common;

public class UpdateSignableObjectParams<TModel> where TModel : SignableObject
{
	public TModel? Model { get; set; }

	public bool IgnoreWarnings { get; set; }
}
