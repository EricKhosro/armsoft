namespace ArmSoft.OpenBanking.Api.Common;

public class SignableObjectForEdit<TModel> where TModel : SignableObject
{
	public TModel Model { get; set; }

	public long AttachmentSize { get; set; }

	public bool CanBeSigned { get; set; }
}
