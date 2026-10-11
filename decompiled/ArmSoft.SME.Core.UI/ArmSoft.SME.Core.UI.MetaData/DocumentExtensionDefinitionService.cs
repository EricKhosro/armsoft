using ArmSoft.SME.Common.MetaData.Document;
using ArmSoft.SME.Common.Models.Caches;
using ArmSoft.SME.Common.Models.Documents;

namespace ArmSoft.SME.Core.UI.MetaData;

public class DocumentExtensionDefinitionService : IDocumentExtensionDefinitionService
{
	public DocumentExtensionDefinition Load(short poTypeDef, short poType)
	{
		DocumentExtensionCacheRow item = Settings.CoreCacheService.DocumentExtensions.GetItem(new DocumentExtensionKey(poTypeDef, poType));
		if (item != null)
		{
			return new DocumentExtensionDefinition(item.Xml, new Environment());
		}
		return null;
	}

	public void Store(DocumentExtensionKey extensionKey, string xml, bool updateCache = true)
	{
		DocumentExtensionModelBase documentExtensionModel = new DocumentExtensionModelBase
		{
			POTypeDef = extensionKey.POTypeDef,
			POType = extensionKey.POType,
			ExtensionXml = xml
		};
		Settings.CreateApiClient().DocumentExtensions.StoreExtensionDescription(documentExtensionModel);
		if (updateCache)
		{
			Settings.CoreCacheService.DocumentExtensions.Refresh();
		}
	}
}
