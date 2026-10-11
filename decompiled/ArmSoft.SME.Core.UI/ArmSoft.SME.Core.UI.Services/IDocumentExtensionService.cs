using ArmSoft.SME.Common.MetaData;
using ArmSoft.SME.Common.Models.Documents;

namespace ArmSoft.SME.Core.UI.Services;

public interface IDocumentExtensionService
{
	DocumentExtensionModel Load(DocumentExtensionRequest request);

	void Store(DocumentExtensionModel documentDefinition);

	void Delete(short poTypeDef, short docType);

	DocumentExtensionModelBase LoadExtensionDescription(DocumentExtensionRequest request);

	void StoreExtensionDescription(DocumentExtensionModelBase documentDefinition);

	DocumentValidatorExpression GetValidatorExpression(short poTypeDef, short docType);

	void CreateValidatorExpression(DocumentValidatorExpression validator);

	void StoreValidatorExpression(DocumentValidatorExpression validator);
}
