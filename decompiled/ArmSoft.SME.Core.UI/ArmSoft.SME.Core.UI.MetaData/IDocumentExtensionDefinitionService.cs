using ArmSoft.SME.Common.MetaData.Document;

namespace ArmSoft.SME.Core.UI.MetaData;

public interface IDocumentExtensionDefinitionService
{
	DocumentExtensionDefinition Load(short poTypeDef, short poType);

	void Store(DocumentExtensionKey extensionKey, string xml, bool updateCache = true);
}
