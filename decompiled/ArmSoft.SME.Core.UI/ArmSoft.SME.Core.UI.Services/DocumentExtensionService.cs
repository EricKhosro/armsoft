using ArmSoft.SME.Client;
using ArmSoft.SME.Common.MetaData;
using ArmSoft.SME.Common.Models.Documents;

namespace ArmSoft.SME.Core.UI.Services;

public class DocumentExtensionService : IDocumentExtensionService
{
	private static ApiClient Client => Settings.CreateApiClient();

	public DocumentExtensionModel Load(DocumentExtensionRequest request)
	{
		return Client.DocumentExtensions.Load(request);
	}

	public void Store(DocumentExtensionModel documentDefinition)
	{
		Client.DocumentExtensions.Store(documentDefinition);
		Settings.CoreCacheService.DocumentExtensions.Refresh();
	}

	public void Delete(short poTypeDef, short poType)
	{
		Client.DocumentExtensions.Delete(new DocumentExtensionRequest(poTypeDef, poType));
		Settings.CoreCacheService.DocumentExtensions.Refresh();
	}

	public DocumentValidatorExpression GetValidatorExpression(short poTypeDef, short poType)
	{
		return Client.DocumentExtensions.GetValidatorExpression(new DocumentExtensionRequest(poTypeDef, poType));
	}

	public void CreateValidatorExpression(DocumentValidatorExpression validator)
	{
		Client.DocumentExtensions.CreateValidatorExpression(validator);
		Settings.CoreCacheService.DocumentExtensions.Refresh();
	}

	public void StoreValidatorExpression(DocumentValidatorExpression validator)
	{
		Client.DocumentExtensions.StoreValidatorExpression(validator);
		Settings.CoreCacheService.DocumentExtensions.Refresh();
	}

	public DocumentExtensionModelBase LoadExtensionDescription(DocumentExtensionRequest request)
	{
		return Client.DocumentExtensions.LoadExtensionDescription(request);
	}

	public void StoreExtensionDescription(DocumentExtensionModelBase documentDefinition)
	{
		Client.DocumentExtensions.StoreExtensionDescription(documentDefinition);
		Settings.CoreCacheService.DocumentExtensions.Refresh();
	}
}
