namespace ArmSoft.SME.Common.Models.Documents;

public class DocumentStoreRequestModel
{
	public bool ReturnDocument { get; set; } = true;

	public StoreRequest StoreRequest { get; set; }

	public string JSONDocument { get; set; }

	public bool ThrowException { get; set; }
}
