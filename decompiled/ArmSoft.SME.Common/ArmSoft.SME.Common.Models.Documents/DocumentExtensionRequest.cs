namespace ArmSoft.SME.Common.Models.Documents;

public class DocumentExtensionRequest : DocumentIdentificationModel
{
	public DocumentExtensionRequest(short poTypeDef, short poType)
	{
		POTypeDef = poTypeDef;
		POType = poType;
	}
}
