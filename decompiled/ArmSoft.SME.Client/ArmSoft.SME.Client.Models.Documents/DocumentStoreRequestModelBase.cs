using ArmSoft.SME.Common.Models.Documents;

namespace ArmSoft.SME.Client.Models.Documents;

public class DocumentStoreRequestModelBase
{
	public bool ReturnDocument { get; set; } = true;

	public StoreRequest StoreRequest { get; set; }

	public Document Document { get; set; }

	public bool ThrowException { get; set; }
}
