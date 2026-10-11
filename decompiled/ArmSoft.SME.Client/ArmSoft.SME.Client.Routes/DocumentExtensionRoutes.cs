using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ArmSoft.SME.Common.MetaData;
using ArmSoft.SME.Common.Models.Documents;

namespace ArmSoft.SME.Client.Routes;

public class DocumentExtensionRoutes
{
	private readonly ApiClient _client;

	internal DocumentExtensionRoutes(ApiClient client)
	{
		_client = client;
	}

	public Task<DocumentExtensionModel> LoadAsync(DocumentExtensionRequest request, CancellationToken cancellationToken = default(CancellationToken))
	{
		StringBuilder stringBuilder = _client.CreateURLBuilder("/api/DocumentExtension");
		return _client.ExecuteAsync<DocumentExtensionModel>(HttpMethod.Post, stringBuilder.ToString(), cancellationToken, request);
	}

	public DocumentExtensionModel Load(DocumentExtensionRequest request)
	{
		return AsyncHelper.RunSync(async () => await LoadAsync(request));
	}

	public Task StoreAsync(DocumentExtensionModel documentDefinition, CancellationToken cancellationToken = default(CancellationToken))
	{
		StringBuilder stringBuilder = _client.CreateURLBuilder("/api/DocumentExtension");
		return _client.ExecuteAsync(HttpMethod.Put, stringBuilder.ToString(), cancellationToken, documentDefinition);
	}

	public void Store(DocumentExtensionModel documentDefinition)
	{
		AsyncHelper.RunSync(async () =>
		{
			await StoreAsync(documentDefinition);
		});
	}

	public Task<DocumentExtensionModelBase> LoadExtensionDescriptionAsync(DocumentExtensionRequest request, CancellationToken cancellationToken = default(CancellationToken))
	{
		StringBuilder stringBuilder = _client.CreateURLBuilder("/api/DocumentExtension/Extension");
		return _client.ExecuteAsync<DocumentExtensionModelBase>(HttpMethod.Post, stringBuilder.ToString(), cancellationToken, request);
	}

	public DocumentExtensionModelBase LoadExtensionDescription(DocumentExtensionRequest request)
	{
		return AsyncHelper.RunSync(async () => await LoadExtensionDescriptionAsync(request));
	}

	public Task StoreExtensionDescriptionAsync(DocumentExtensionModelBase documentExtensionModel, CancellationToken cancellationToken = default(CancellationToken))
	{
		StringBuilder stringBuilder = _client.CreateURLBuilder("/api/DocumentExtension/Extension");
		return _client.ExecuteAsync(HttpMethod.Put, stringBuilder.ToString(), cancellationToken, documentExtensionModel);
	}

	public void StoreExtensionDescription(DocumentExtensionModelBase documentExtensionModel)
	{
		AsyncHelper.RunSync(async () =>
		{
			await StoreExtensionDescriptionAsync(documentExtensionModel);
		});
	}

	public Task StoreScriptAsync(DocumentScriptModel documentScript, CancellationToken cancellationToken = default(CancellationToken))
	{
		StringBuilder stringBuilder = _client.CreateURLBuilder("/api/DocumentExtension/Script");
		return _client.ExecuteAsync(HttpMethod.Put, stringBuilder.ToString(), cancellationToken, documentScript);
	}

	public void StoreScript(DocumentScriptModel documentScript)
	{
		AsyncHelper.RunSync(async () =>
		{
			await StoreScriptAsync(documentScript);
		});
	}

	public Task<DocumentValidatorExpression> GetValidatorExpressionAsync(DocumentExtensionRequest request, CancellationToken cancellationToken = default(CancellationToken))
	{
		StringBuilder stringBuilder = _client.CreateURLBuilder("/api/DocumentExtension/Validator");
		return _client.ExecuteAsync<DocumentValidatorExpression>(HttpMethod.Post, stringBuilder.ToString(), cancellationToken, request);
	}

	public DocumentValidatorExpression GetValidatorExpression(DocumentExtensionRequest request)
	{
		return AsyncHelper.RunSync(async () => await GetValidatorExpressionAsync(request));
	}

	public Task CreateValidatorExpressionAsync(DocumentValidatorExpression validator, CancellationToken cancellationToken = default(CancellationToken))
	{
		UrlQuery urlQuery = new UrlQuery();
		urlQuery.Add("isNew", value: true);
		StringBuilder stringBuilder = _client.CreateURLBuilder("/api/DocumentExtension/Validator", urlQuery);
		return _client.ExecuteAsync(HttpMethod.Put, stringBuilder.ToString(), cancellationToken, validator);
	}

	public void CreateValidatorExpression(DocumentValidatorExpression validator)
	{
		AsyncHelper.RunSync(async () =>
		{
			await CreateValidatorExpressionAsync(validator);
		});
	}

	public Task StoreValidatorExpressionAsync(DocumentValidatorExpression validator, CancellationToken cancellationToken = default(CancellationToken))
	{
		StringBuilder stringBuilder = _client.CreateURLBuilder("/api/DocumentExtension/Validator");
		return _client.ExecuteAsync(HttpMethod.Put, stringBuilder.ToString(), cancellationToken, validator);
	}

	public void StoreValidatorExpression(DocumentValidatorExpression validator)
	{
		AsyncHelper.RunSync(async () =>
		{
			await StoreValidatorExpressionAsync(validator);
		});
	}

	public Task DeleteAsync(DocumentExtensionRequest request, CancellationToken cancellationToken = default(CancellationToken))
	{
		StringBuilder stringBuilder = _client.CreateURLBuilder("/api/DocumentExtension");
		return _client.ExecuteAsync(HttpMethod.Delete, stringBuilder.ToString(), cancellationToken, request);
	}

	public void Delete(DocumentExtensionRequest request)
	{
		AsyncHelper.RunSync(async () =>
		{
			await DeleteAsync(request);
		});
	}
}
