using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using ArmSoft.SME.Client.DPR;
using ArmSoft.SME.Client.JsonSerialization;
using ArmSoft.SME.Client.Models;
using ArmSoft.SME.Client.Models.Documents;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Enums;
using ArmSoft.SME.Common.Models;
using ArmSoft.SME.Common.Models.DPR;
using ArmSoft.SME.Common.Models.Documents;

namespace ArmSoft.SME.Client;

public class DocumentRoutes
{
	protected readonly ApiClient _client;

	public readonly JsonSerializerOptions Options;

	protected virtual string ControllerName => "Document";

	public DocumentRoutes(ApiClient client)
	{
		_client = client;
		Options = JsonSerializerFactory.CloneJsonOptions();
		Options.Converters.Add(new ObjectConverter());
		Options.TypeInfoResolver = new DocumentTypeResolver();
		UpdateJsonSerializerOptions(Options);
	}

	protected virtual void UpdateJsonSerializerOptions(JsonSerializerOptions options)
	{
	}

	public Task<short> GetDocTypeAsync(Guid isn, CancellationToken cancellationToken = default(CancellationToken))
	{
		StringBuilder stringBuilder = _client.CreateURLBuilder($"/api/{ControllerName}/DocType/{isn}");
		return _client.ExecuteAsync<short>(HttpMethod.Get, stringBuilder.ToString(), cancellationToken);
	}

	public short GetDocType(Guid isn)
	{
		return AsyncHelper.RunSync(async () => await GetDocTypeAsync(isn));
	}

	public int GetChildrenDocsCount(Guid isn)
	{
		return AsyncHelper.RunSync(async () => await GetChildrenDocsCountAsync(isn));
	}

	public Task<int> GetChildrenDocsCountAsync(Guid isn, CancellationToken cancellationToken = default(CancellationToken))
	{
		StringBuilder stringBuilder = _client.CreateURLBuilder($"/api/{ControllerName}/ChildrenDocsCount/{isn}");
		return _client.ExecuteAsync<int>(HttpMethod.Get, stringBuilder.ToString(), cancellationToken);
	}

	public async Task<T> CreateAsync<T>(short type, CancellationToken cancellationToken = default(CancellationToken))
	{
		StringBuilder stringBuilder = _client.CreateURLBuilder($"/api/{ControllerName}/Create/{type}");
		return JsonSerializer.Deserialize<T>(await _client.ExecuteAsync<string>(HttpMethod.Get, stringBuilder.ToString(), cancellationToken), Options);
	}

	public T Create<T>(short type)
	{
		return AsyncHelper.RunSync(async () => await CreateAsync<T>(type));
	}

	public async Task<Document> CreateAsync(short type, CancellationToken cancellationToken = default(CancellationToken))
	{
		StringBuilder stringBuilder = _client.CreateURLBuilder($"/api/{ControllerName}/Create/{type}");
		return JsonSerializer.Deserialize<Document>(await _client.ExecuteAsync<string>(HttpMethod.Get, stringBuilder.ToString(), cancellationToken), Options);
	}

	public Document Create(short type)
	{
		return AsyncHelper.RunSync(async () => await CreateAsync(type));
	}

	public async Task<T> LoadAsync<T>(Guid isn, bool withoutAccessCheck = false, bool withoutGrids = false, CancellationToken cancellationToken = default(CancellationToken))
	{
		UrlQuery urlQuery = new UrlQuery();
		urlQuery.Add("withoutAccessCheck", withoutAccessCheck);
		urlQuery.Add("withoutGrids", withoutGrids);
		StringBuilder stringBuilder = _client.CreateURLBuilder($"/api/{ControllerName}/{isn}", urlQuery);
		return JsonSerializer.Deserialize<T>((await _client.ExecuteAsync<DocumentLoadResponseModel>(HttpMethod.Get, stringBuilder.ToString(), cancellationToken)).JSONDocument, Options);
	}

	public T Load<T>(Guid isn, bool withoutAccessCheck = false, bool withoutGrids = false)
	{
		return AsyncHelper.RunSync(async () => await LoadAsync<T>(isn, withoutAccessCheck, withoutGrids));
	}

	public async Task<Document> LoadAsync(Guid isn, bool withoutAccessCheck = false, bool withoutGrids = false, CancellationToken cancellationToken = default(CancellationToken))
	{
		UrlQuery urlQuery = new UrlQuery();
		urlQuery.Add("withoutAccessCheck", withoutAccessCheck);
		urlQuery.Add("withoutGrids", withoutGrids);
		StringBuilder stringBuilder = _client.CreateURLBuilder($"/api/{ControllerName}/{isn}", urlQuery);
		return JsonSerializer.Deserialize<Document>((await _client.ExecuteAsync<DocumentLoadResponseModel>(HttpMethod.Get, stringBuilder.ToString(), cancellationToken)).JSONDocument, Options);
	}

	public Document Load(Guid isn, bool withoutAccessCheck = false, bool withoutGrids = false)
	{
		return AsyncHelper.RunSync(async () => await LoadAsync(isn, withoutAccessCheck, withoutGrids));
	}

	public async Task<T> LoadAsync<T>(Guid isn, short docType, bool withoutAccessCheck = false, bool withoutGrids = false, CancellationToken cancellationToken = default(CancellationToken))
	{
		UrlQuery urlQuery = new UrlQuery();
		urlQuery.Add("docType", docType);
		urlQuery.Add("withoutAccessCheck", withoutAccessCheck);
		urlQuery.Add("withoutGrids", withoutGrids);
		StringBuilder stringBuilder = _client.CreateURLBuilder($"/api/{ControllerName}/LoadByDocType/{isn}", urlQuery);
		return JsonSerializer.Deserialize<T>((await _client.ExecuteAsync<DocumentLoadResponseModel>(HttpMethod.Get, stringBuilder.ToString(), cancellationToken)).JSONDocument, Options);
	}

	public T Load<T>(Guid isn, short docType, bool withoutAccessCheck = false, bool withoutGrids = false)
	{
		return AsyncHelper.RunSync(async () => await LoadAsync<T>(isn, docType, withoutAccessCheck, withoutGrids));
	}

	public async Task<Document> LoadAsync(Guid isn, short docType, bool withoutAccessCheck = false, bool withoutGrids = false, CancellationToken cancellationToken = default(CancellationToken))
	{
		UrlQuery urlQuery = new UrlQuery();
		urlQuery.Add("docType", docType);
		urlQuery.Add("withoutAccessCheck", withoutAccessCheck);
		urlQuery.Add("withoutGrids", withoutGrids);
		StringBuilder stringBuilder = _client.CreateURLBuilder($"/api/{ControllerName}/LoadByDocType/{isn}", urlQuery);
		return JsonSerializer.Deserialize<Document>((await _client.ExecuteAsync<DocumentLoadResponseModel>(HttpMethod.Get, stringBuilder.ToString(), cancellationToken)).JSONDocument, Options);
	}

	public Document Load(Guid isn, short docType, bool withoutAccessCheck = false, bool withoutGrids = false)
	{
		return AsyncHelper.RunSync(async () => await LoadAsync(isn, docType, withoutAccessCheck, withoutGrids));
	}

	public async Task<Document> LoadAsync(string docNumber, short docType, bool withoutAccessCheck = false, bool withoutGrids = false, CancellationToken cancellationToken = default(CancellationToken))
	{
		UrlQuery urlQuery = new UrlQuery();
		urlQuery.Add("docNumber", docNumber);
		urlQuery.Add("docType", docType);
		urlQuery.Add("withoutAccessCheck", withoutAccessCheck);
		urlQuery.Add("withoutGrids", withoutGrids);
		StringBuilder stringBuilder = _client.CreateURLBuilder("/api/" + ControllerName + "/LoadByDocNumber", urlQuery);
		return JsonSerializer.Deserialize<Document>((await _client.ExecuteAsync<DocumentLoadResponseModel>(HttpMethod.Get, stringBuilder.ToString(), cancellationToken)).JSONDocument, Options);
	}

	public Document Load(string docNumber, short docType, bool withoutAccessCheck = false, bool withoutGrids = false)
	{
		return AsyncHelper.RunSync(async () => await LoadAsync(docNumber, docType, withoutAccessCheck, withoutGrids));
	}

	public async Task<T> CreateCopyAsync<T>(Guid isn, CancellationToken cancellationToken = default(CancellationToken))
	{
		StringBuilder stringBuilder = _client.CreateURLBuilder($"/api/{ControllerName}/CreateCopy/{isn}");
		return JsonSerializer.Deserialize<T>((await _client.ExecuteAsync<DocumentLoadResponseModel>(HttpMethod.Get, stringBuilder.ToString(), cancellationToken)).JSONDocument, Options);
	}

	public T CreateCopy<T>(Guid isn)
	{
		return AsyncHelper.RunSync(async () => await CreateCopyAsync<T>(isn));
	}

	public async Task<Document> CreateCopyAsync(Guid isn, CancellationToken cancellationToken = default(CancellationToken))
	{
		StringBuilder stringBuilder = _client.CreateURLBuilder($"/api/{ControllerName}/CreateCopy/{isn}");
		return JsonSerializer.Deserialize<Document>((await _client.ExecuteAsync<DocumentLoadResponseModel>(HttpMethod.Get, stringBuilder.ToString(), cancellationToken)).JSONDocument, Options);
	}

	public Document CreateCopy(Guid isn)
	{
		return AsyncHelper.RunSync(async () => await CreateCopyAsync(isn));
	}

	public async Task<DocumentStoreResponseModel<T>> StoreAsync<T>(DocumentStoreRequestModel<T> requestModel, CancellationToken cancellationToken = default(CancellationToken)) where T : Document, new()
	{
		DocumentStoreRequestModel requestObject = new DocumentStoreRequestModel
		{
			ReturnDocument = requestModel.ReturnDocument,
			StoreRequest = requestModel.StoreRequest,
			JSONDocument = JsonSerializer.Serialize((Document)requestModel.Document, Options),
			ThrowException = requestModel.ThrowException
		};
		StringBuilder stringBuilder = _client.CreateURLBuilder("/api/" + ControllerName + "/Store");
		DocumentStoreResponseModel documentStoreResponseModel = await _client.ExecuteAsync<DocumentStoreResponseModel>(HttpMethod.Post, stringBuilder.ToString(), cancellationToken, requestObject);
		DocumentStoreResponseModel<T> documentStoreResponseModel2 = new DocumentStoreResponseModel<T>
		{
			Exceptions = documentStoreResponseModel.Exceptions,
			Warnings = documentStoreResponseModel.Warnings
		};
		if (documentStoreResponseModel.JSONDocument != null)
		{
			documentStoreResponseModel2.Document = JsonSerializer.Deserialize<T>(documentStoreResponseModel.JSONDocument, Options);
		}
		return documentStoreResponseModel2;
	}

	public DocumentStoreResponseModel<T> Store<T>(DocumentStoreRequestModel<T> requestModel) where T : Document, new()
	{
		return AsyncHelper.RunSync(async () => await StoreAsync(requestModel));
	}

	public async Task<DocumentStoreResponseModelBase> StoreAsync(DocumentStoreRequestModelBase requestModel, CancellationToken cancellationToken = default(CancellationToken))
	{
		DocumentStoreRequestModel requestObject = new DocumentStoreRequestModel
		{
			ReturnDocument = requestModel.ReturnDocument,
			StoreRequest = requestModel.StoreRequest,
			JSONDocument = JsonSerializer.Serialize(requestModel.Document, Options),
			ThrowException = requestModel.ThrowException
		};
		StringBuilder stringBuilder = _client.CreateURLBuilder("/api/" + ControllerName + "/Store");
		DocumentStoreResponseModel documentStoreResponseModel = await _client.ExecuteAsync<DocumentStoreResponseModel>(HttpMethod.Post, stringBuilder.ToString(), cancellationToken, requestObject);
		DocumentStoreResponseModelBase documentStoreResponseModelBase = new DocumentStoreResponseModelBase
		{
			Exceptions = documentStoreResponseModel.Exceptions,
			Warnings = documentStoreResponseModel.Warnings
		};
		if (documentStoreResponseModel.JSONDocument != null)
		{
			documentStoreResponseModelBase.Document = JsonSerializer.Deserialize<Document>(documentStoreResponseModel.JSONDocument, Options);
		}
		return documentStoreResponseModelBase;
	}

	public DocumentStoreResponseModelBase Store(DocumentStoreRequestModelBase requestModel)
	{
		return AsyncHelper.RunSync(async () => await StoreAsync(requestModel));
	}

	public Task CancelStoreAsync<T>(DocumentStoreRequestModel<T> requestModel, CancellationToken cancellationToken = default(CancellationToken)) where T : Document, new()
	{
		DocumentStoreRequestModel requestObject = new DocumentStoreRequestModel
		{
			StoreRequest = requestModel.StoreRequest,
			JSONDocument = JsonSerializer.Serialize((Document)requestModel.Document, Options)
		};
		StringBuilder stringBuilder = _client.CreateURLBuilder("/api/" + ControllerName + "/CancelStore");
		return _client.ExecuteAsync(HttpMethod.Post, stringBuilder.ToString(), cancellationToken, requestObject);
	}

	public void CancelStore<T>(DocumentStoreRequestModel<T> requestModel) where T : Document, new()
	{
		AsyncHelper.RunSync(async () =>
		{
			await CancelStoreAsync(requestModel);
		});
	}

	public Task<DocumentDeleteResponseModel> DeleteAsync(Guid isn, DocumentDeleteRequestModel request, CancellationToken cancellationToken = default(CancellationToken))
	{
		StringBuilder stringBuilder = _client.CreateURLBuilder($"/api/{ControllerName}/{isn}");
		return _client.ExecuteAsync<DocumentDeleteResponseModel>(HttpMethod.Delete, stringBuilder.ToString(), cancellationToken, request);
	}

	public DocumentDeleteResponseModel Delete(Guid isn, DocumentDeleteRequestModel request)
	{
		return AsyncHelper.RunSync(async () => await DeleteAsync(isn, request));
	}

	public bool ValidateDelete(Guid isn, DocumentValidateDeleteRequestModel requestModel)
	{
		return AsyncHelper.RunSync(async () => await ValidateDeleteAsync(isn, requestModel));
	}

	public async Task<bool> ValidateDeleteAsync(Guid isn, DocumentValidateDeleteRequestModel requestModel, CancellationToken cancellationToken = default(CancellationToken))
	{
		StringBuilder stringBuilder = _client.CreateURLBuilder($"/api/{ControllerName}/ValidateDelete/{isn}");
		return await _client.ExecuteAsync<bool>(HttpMethod.Post, stringBuilder.ToString(), cancellationToken, requestModel);
	}

	public Task ChangeDocStateAsync(Guid isn, byte state, bool regenerateEntries, CancellationToken cancellationToken = default(CancellationToken))
	{
		UrlQuery urlQuery = new UrlQuery();
		urlQuery.Add("regenerateEntries", regenerateEntries);
		StringBuilder stringBuilder = _client.CreateURLBuilder($"/api/{ControllerName}/ChangeDocState/{isn}/{state}", urlQuery);
		return _client.ExecuteAsync(HttpMethod.Post, stringBuilder.ToString(), cancellationToken);
	}

	public void ChangeDocState(Guid isn, byte state, bool regenerateEntries)
	{
		AsyncHelper.RunSync(async () =>
		{
			await ChangeDocStateAsync(isn, state, regenerateEntries);
		});
	}

	public Task<KeyValuePair<string, DocNumOfferSource>> GetNextDocNumAsync(short docType, string relatedInfo, CancellationToken cancellationToken = default(CancellationToken))
	{
		StringBuilder stringBuilder = _client.CreateURLBuilder($"/api/{ControllerName}/GetNextDocNum/{docType}/");
		return _client.ExecuteAsync<KeyValuePair<string, DocNumOfferSource>>(HttpMethod.Post, stringBuilder.ToString(), cancellationToken, relatedInfo);
	}

	public KeyValuePair<string, DocNumOfferSource> GetNextDocNum(short docType, string relatedInfo)
	{
		return AsyncHelper.RunSync(async () => await GetNextDocNumAsync(docType, relatedInfo));
	}

	public Task<byte[]> GetActualTSAsync(Guid isn, CancellationToken cancellationToken = default(CancellationToken))
	{
		StringBuilder stringBuilder = _client.CreateURLBuilder($"/api/{ControllerName}/GetActualTS/{isn}/");
		return _client.ExecuteAsync<byte[]>(HttpMethod.Get, stringBuilder.ToString(), cancellationToken);
	}

	public byte[] GetActualTS(Guid isn)
	{
		return AsyncHelper.RunSync(async () => await GetActualTSAsync(isn));
	}

	public Task WriteLogAsync(Guid isn, DocumentWriteLogRequestModel request, CancellationToken cancellationToken = default(CancellationToken))
	{
		StringBuilder stringBuilder = _client.CreateURLBuilder($"/api/{ControllerName}/WriteLog/{isn}");
		return _client.ExecuteAsync(HttpMethod.Post, stringBuilder.ToString(), cancellationToken, request);
	}

	public void WriteLog(Guid isn, DocumentWriteLogRequestModel request)
	{
		AsyncHelper.RunSync(async () =>
		{
			await WriteLogAsync(isn, request);
		});
	}

	public Task<byte[]> EditFieldsAsync(Guid isn, DocumentEditFieldsRequestModel request, CancellationToken cancellationToken = default(CancellationToken))
	{
		StringBuilder stringBuilder = _client.CreateURLBuilder($"/api/{ControllerName}/EditFields/{isn}");
		return _client.ExecuteAsync<byte[]>(HttpMethod.Post, stringBuilder.ToString(), cancellationToken, request);
	}

	public byte[] EditFields(Guid isn, DocumentEditFieldsRequestModel request)
	{
		return AsyncHelper.RunSync(async () => await EditFieldsAsync(isn, request));
	}

	public Guid GetDocIsnByNumber(string docNumber, short docType)
	{
		return AsyncHelper.RunSync(async () => await GetDocIsnByNumberAsync(docNumber, docType));
	}

	public Task<Guid> GetDocIsnByNumberAsync(string docNumber, short docType, CancellationToken cancellationToken = default(CancellationToken))
	{
		UrlQuery urlQuery = new UrlQuery();
		urlQuery.Add("docNumber", docNumber);
		urlQuery.Add("docType", docType);
		StringBuilder stringBuilder = _client.CreateURLBuilder("/api/" + ControllerName + "/GetDocIsnByNumber", urlQuery);
		return _client.ExecuteAsync<Guid>(HttpMethod.Get, stringBuilder.ToString(), cancellationToken);
	}

	public bool ExistsDocByISN(Guid isn, bool includeDeleted = false)
	{
		return AsyncHelper.RunSync(async () => await ExistsDocByISNAsync(isn, includeDeleted));
	}

	public Task<bool> ExistsDocByISNAsync(Guid isn, bool includeDeleted = false, CancellationToken cancellationToken = default(CancellationToken))
	{
		UrlQuery urlQuery = new UrlQuery();
		urlQuery.Add("isn", isn);
		urlQuery.Add("includeDeleted", includeDeleted);
		StringBuilder stringBuilder = _client.CreateURLBuilder("/api/" + ControllerName + "/ExistsDocByISN", urlQuery);
		return _client.ExecuteAsync<bool>(HttpMethod.Get, stringBuilder.ToString(), cancellationToken);
	}

	public List<Guid> GetChildrenDocISNs(List<Guid> isns, List<short> childDocTypes = null)
	{
		ChildrenDocISNsRequest requestObject = new ChildrenDocISNsRequest
		{
			DocISNs = isns,
			ChildDocTypes = childDocTypes
		};
		StringBuilder stringBuilder = _client.CreateURLBuilder("/api/" + ControllerName + "/GetChildrenDocISNs");
		return _client.Execute<List<Guid>>(HttpMethod.Post, stringBuilder.ToString(), requestObject);
	}

	public List<Guid> GetParentDocISNs(List<Guid> isns, List<short> parentDocTypes = null)
	{
		ParentDocISNsRequest requestObject = new ParentDocISNsRequest
		{
			DocISNs = isns,
			ParentDocTypes = parentDocTypes
		};
		StringBuilder stringBuilder = _client.CreateURLBuilder("/api/" + ControllerName + "/GetParentDocISNs");
		return _client.Execute<List<Guid>>(HttpMethod.Post, stringBuilder.ToString(), requestObject);
	}

	public string GetJSONDocument(Document document)
	{
		return JsonSerializer.Serialize(document, Options);
	}

	public string GetJSONDocument<T>(T document) where T : Document, new()
	{
		return JsonSerializer.Serialize((Document)document, Options);
	}

	public CreateDocumentResponseModel CreateDocumentsFromSelectedRows<T, P>(CreateDocumentFromSelectedRowsRequest<T, P> request, string dprName)
	{
		var (flag, result) = new DataProcessingRequestHandler(_client).Execute<CreateDocumentFromSelectedRowsRequest<T, P>, CreateDocumentFromSelectedRowsResponse>(DPRType.Other, dprName, request);
		if (!flag)
		{
			return new CreateDocumentResponseModel();
		}
		return BuildCreateDocumentResponse(result);
	}

	public CreateDocumentResponseModel BuildCreateDocumentResponse(CreateDocumentFromSelectedRowsResponse result)
	{
		CreateDocumentResponseModel createDocumentResponseModel = new CreateDocumentResponseModel
		{
			CreatedDocsCount = result.CreatedDocsCount,
			EditedDocsCount = result.EditedDocsCount,
			CreatedDocsCountByType = result.CreatedDocsCountByType,
			Exceptions = result.Exceptions,
			Documents = new List<Document>()
		};
		foreach (string jSONDocument in result.JSONDocuments)
		{
			createDocumentResponseModel.Documents.Add(JsonSerializer.Deserialize<Document>(jSONDocument, Options));
		}
		return createDocumentResponseModel;
	}
}
