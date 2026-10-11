using ArmSoft.SME.Common.Models.Documents;

namespace ArmSoft.SME.Core.UI.MetaData;

public class DocumentExtensionSettings
{
	public DocumentValidatorExpression ValidatorExpression { get; set; }

	public DocumentExtensionDefinition ExtensionDefinition { get; set; }
}
