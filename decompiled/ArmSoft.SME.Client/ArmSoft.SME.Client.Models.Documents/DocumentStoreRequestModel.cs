using ArmSoft.SME.Common.Models.Documents;

namespace ArmSoft.SME.Client.Models.Documents;

public class DocumentStoreRequestModel<T> where T : Document, new()
{
	public bool ReturnDocument { get; set; } = true;

	public StoreRequest StoreRequest { get; set; }

	public T Document { get; set; }

	public bool ThrowException { get; set; }
}
