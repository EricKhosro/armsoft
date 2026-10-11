namespace ArmSoft.SME.Common.Models.Documents;

public class DocumentDeleteRequestModel
{
	public PODeleteRequest DeleteRequest { get; set; }

	public bool ThrowException { get; set; }
}
