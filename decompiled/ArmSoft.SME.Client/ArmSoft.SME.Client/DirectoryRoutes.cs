using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using ArmSoft.SME.Client.JsonSerialization;
using ArmSoft.SME.Client.Models.Directories;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Enums;
using ArmSoft.SME.Common.Models.Directories;

namespace ArmSoft.SME.Client;

public class DirectoryRoutes
{
	protected readonly ApiClient _client;

	private readonly JsonSerializerOptions _options;

	protected virtual string ControllerName => "Directory";

	public DirectoryRoutes(ApiClient client)
	{
		_client = client;
		_options = JsonSerializerFactory.CloneJsonOptions();
		_options.Converters.Add(new ObjectConverter());
		_options.TypeInfoResolver = new DirectoryTypeResolver();
		UpdateJsonSerializerOptions(_options);
	}

	protected virtual void UpdateJsonSerializerOptions(JsonSerializerOptions options)
	{
	}

	public async Task<T> CreateAsync<T>(short type, CancellationToken cancellationToken = default(CancellationToken))
	{
		StringBuilder stringBuilder = _client.CreateURLBuilder($"/api/{ControllerName}/Create/{type}");
		return JsonSerializer.Deserialize<T>(await _client.ExecuteAsync<string>(HttpMethod.Get, stringBuilder.ToString(), cancellationToken), _options);
	}

	public T Create<T>(short type)
	{
		return AsyncHelper.RunSync(async () => await CreateAsync<T>(type));
	}

	public async Task<Directory> CreateAsync(short type, CancellationToken cancellationToken = default(CancellationToken))
	{
		StringBuilder stringBuilder = _client.CreateURLBuilder($"/api/{ControllerName}/Create/{type}");
		return JsonSerializer.Deserialize<Directory>(await _client.ExecuteAsync<string>(HttpMethod.Get, stringBuilder.ToString(), cancellationToken), _options);
	}

	public Directory Create(short type)
	{
		return AsyncHelper.RunSync(async () => await CreateAsync(type));
	}

	public T Load<T>(short directoryType, int id, bool withoutAccessCheck = false, bool throwExceptionIfDoesNotExist = true)
	{
		return AsyncHelper.RunSync(async () => await LoadAsync<T>(directoryType, id, withoutAccessCheck, throwExceptionIfDoesNotExist));
	}

	public async Task<T> LoadAsync<T>(short directoryType, int id, bool withoutAccessCheck = false, bool throwExceptionIfDoesNotExist = true, CancellationToken cancellationToken = default(CancellationToken))
	{
		UrlQuery urlQuery = new UrlQuery();
		urlQuery.Add("withoutAccessCheck", withoutAccessCheck);
		urlQuery.Add("throwExceptionIfDoesNotExist", throwExceptionIfDoesNotExist);
		StringBuilder stringBuilder = _client.CreateURLBuilder($"/api/{ControllerName}/LoadById/{directoryType}/{id}", urlQuery);
		return JsonSerializer.Deserialize<T>((await _client.ExecuteAsync<DirectoryLoadResponseModel>(HttpMethod.Get, stringBuilder.ToString(), cancellationToken)).JSONDocument, _options);
	}

	public async Task<Directory> LoadAsync(short directoryType, int id, bool withoutAccessCheck = false, CancellationToken cancellationToken = default(CancellationToken))
	{
		UrlQuery urlQuery = new UrlQuery();
		urlQuery.Add("withoutAccessCheck", withoutAccessCheck);
		StringBuilder stringBuilder = _client.CreateURLBuilder($"/api/{ControllerName}/LoadById/{directoryType}/{id}", urlQuery);
		return JsonSerializer.Deserialize<Directory>((await _client.ExecuteAsync<DirectoryLoadResponseModel>(HttpMethod.Get, stringBuilder.ToString(), cancellationToken)).JSONDocument, _options);
	}

	public Directory Load(short directoryType, int id, bool withoutAccessCheck = false)
	{
		return AsyncHelper.RunSync(async () => await LoadAsync(directoryType, id, withoutAccessCheck));
	}

	public T Load<T>(short directoryType, string key, bool withoutAccessCheck = false, bool throwExceptionIfDoesNotExist = true)
	{
		return AsyncHelper.RunSync(async () => await LoadAsync<T>(directoryType, key, withoutAccessCheck, throwExceptionIfDoesNotExist));
	}

	public async Task<T> LoadAsync<T>(short directoryType, string key, bool withoutAccessCheck = false, bool throwExceptionIfDoesNotExist = true, CancellationToken cancellationToken = default(CancellationToken))
	{
		UrlQuery urlQuery = new UrlQuery();
		urlQuery.Add("withoutAccessCheck", withoutAccessCheck);
		urlQuery.Add("throwExceptionIfDoesNotExist", throwExceptionIfDoesNotExist);
		StringBuilder stringBuilder = _client.CreateURLBuilder($"/api/{ControllerName}/LoadByKey/{directoryType}", urlQuery);
		return JsonSerializer.Deserialize<T>((await _client.ExecuteAsync<DirectoryLoadResponseModel>(HttpMethod.Post, stringBuilder.ToString(), cancellationToken, key)).JSONDocument, _options);
	}

	public async Task<Directory> LoadAsync(short directoryType, string key, bool withoutAccessCheck = false, CancellationToken cancellationToken = default(CancellationToken))
	{
		UrlQuery urlQuery = new UrlQuery();
		urlQuery.Add("withoutAccessCheck", withoutAccessCheck);
		StringBuilder stringBuilder = _client.CreateURLBuilder($"/api/{ControllerName}/LoadByKey/{directoryType}", urlQuery);
		return JsonSerializer.Deserialize<Directory>((await _client.ExecuteAsync<DirectoryLoadResponseModel>(HttpMethod.Post, stringBuilder.ToString(), cancellationToken, key)).JSONDocument, _options);
	}

	public Directory Load(short directoryType, string key, bool withoutAccessCheck = false)
	{
		return AsyncHelper.RunSync(async () => await LoadAsync(directoryType, key, withoutAccessCheck));
	}

	public T CreateCopy<T>(short directoryType, int id, CancellationToken cancellationToken = default(CancellationToken))
	{
		return AsyncHelper.RunSync(async () => await CreateCopyAsync<T>(directoryType, id, cancellationToken));
	}

	public async Task<T> CreateCopyAsync<T>(short directoryType, int id, CancellationToken cancellationToken = default(CancellationToken))
	{
		StringBuilder stringBuilder = _client.CreateURLBuilder($"/api/{ControllerName}/CreateCopyById/{directoryType}/{id}");
		return JsonSerializer.Deserialize<T>((await _client.ExecuteAsync<DirectoryLoadResponseModel>(HttpMethod.Get, stringBuilder.ToString(), cancellationToken)).JSONDocument, _options);
	}

	public async Task<Directory> CreateCopyAsync(short directoryType, int id, CancellationToken cancellationToken = default(CancellationToken))
	{
		StringBuilder stringBuilder = _client.CreateURLBuilder($"/api/{ControllerName}/CreateCopyById/{directoryType}/{id}");
		return JsonSerializer.Deserialize<Directory>((await _client.ExecuteAsync<DirectoryLoadResponseModel>(HttpMethod.Get, stringBuilder.ToString(), cancellationToken)).JSONDocument, _options);
	}

	public Directory CreateCopy(short directoryType, int id)
	{
		return AsyncHelper.RunSync(async () => await CreateCopyAsync(directoryType, id));
	}

	public T CreateCopy<T>(short directoryType, string key, CancellationToken cancellationToken = default(CancellationToken))
	{
		return AsyncHelper.RunSync(async () => await CreateCopyAsync<T>(directoryType, key, cancellationToken));
	}

	public async Task<T> CreateCopyAsync<T>(short directoryType, string key, CancellationToken cancellationToken = default(CancellationToken))
	{
		StringBuilder stringBuilder = _client.CreateURLBuilder($"/api/{ControllerName}/CreateCopyByKey/{directoryType}");
		return JsonSerializer.Deserialize<T>((await _client.ExecuteAsync<DirectoryLoadResponseModel>(HttpMethod.Post, stringBuilder.ToString(), cancellationToken, key)).JSONDocument, _options);
	}

	public async Task<Directory> CreateCopyAsync(short directoryType, string key, CancellationToken cancellationToken = default(CancellationToken))
	{
		StringBuilder stringBuilder = _client.CreateURLBuilder($"/api/{ControllerName}/CreateCopyByKey/{directoryType}");
		return JsonSerializer.Deserialize<Directory>((await _client.ExecuteAsync<DirectoryLoadResponseModel>(HttpMethod.Post, stringBuilder.ToString(), cancellationToken, key)).JSONDocument, _options);
	}

	public Directory CreateCopy(short directoryType, string key)
	{
		return AsyncHelper.RunSync(async () => await CreateCopyAsync(directoryType, key));
	}

	public async Task<DirectoryStoreResponseModel<T>> StoreAsync<T>(DirectoryStoreRequestModel<T> requestModel, CancellationToken cancellationToken = default(CancellationToken)) where T : Directory, new()
	{
		DirectoryStoreRequestModel requestObject = new DirectoryStoreRequestModel
		{
			ReturnDocument = requestModel.ReturnDocument,
			StoreRequest = requestModel.StoreRequest,
			JSONDocument = JsonSerializer.Serialize((Directory)requestModel.Directory, _options),
			ThrowException = requestModel.ThrowException
		};
		StringBuilder stringBuilder = _client.CreateURLBuilder("/api/" + ControllerName + "/Store");
		DirectoryStoreResponseModel directoryStoreResponseModel = await _client.ExecuteAsync<DirectoryStoreResponseModel>(HttpMethod.Post, stringBuilder.ToString(), cancellationToken, requestObject);
		DirectoryStoreResponseModel<T> directoryStoreResponseModel2 = new DirectoryStoreResponseModel<T>
		{
			Exceptions = directoryStoreResponseModel.Exceptions
		};
		if (directoryStoreResponseModel.JSONDocument != null)
		{
			directoryStoreResponseModel2.Directory = JsonSerializer.Deserialize<T>(directoryStoreResponseModel.JSONDocument, _options);
		}
		return directoryStoreResponseModel2;
	}

	public DirectoryStoreResponseModel<T> Store<T>(DirectoryStoreRequestModel<T> requestModel) where T : Directory, new()
	{
		return AsyncHelper.RunSync(async () => await StoreAsync(requestModel));
	}

	public async Task<DirectoryStoreResponseModelBase> StoreAsync(DirectoryStoreRequestModelBase requestModel, CancellationToken cancellationToken = default(CancellationToken))
	{
		DirectoryStoreRequestModel requestObject = new DirectoryStoreRequestModel
		{
			ReturnDocument = requestModel.ReturnDocument,
			StoreRequest = requestModel.StoreRequest,
			JSONDocument = JsonSerializer.Serialize(requestModel.Directory, _options)
		};
		StringBuilder stringBuilder = _client.CreateURLBuilder("/api/" + ControllerName + "/Store");
		DirectoryStoreResponseModel directoryStoreResponseModel = await _client.ExecuteAsync<DirectoryStoreResponseModel>(HttpMethod.Post, stringBuilder.ToString(), cancellationToken, requestObject);
		DirectoryStoreResponseModelBase directoryStoreResponseModelBase = new DirectoryStoreResponseModelBase
		{
			Exceptions = directoryStoreResponseModel.Exceptions
		};
		if (directoryStoreResponseModel.JSONDocument != null)
		{
			directoryStoreResponseModelBase.Directory = JsonSerializer.Deserialize<Directory>(directoryStoreResponseModel.JSONDocument, _options);
		}
		return directoryStoreResponseModelBase;
	}

	public DirectoryStoreResponseModelBase Store(DirectoryStoreRequestModelBase requestModel)
	{
		return AsyncHelper.RunSync(async () => await StoreAsync(requestModel));
	}

	public async Task<DirectoryEditFieldsWithSameValueResponseModel> StoreSelectedAsync<T>(DirectoryStoreSelectedRequestModel<T> requestModel, CancellationToken cancellationToken = default(CancellationToken)) where T : Directory, new()
	{
		DirectoryStoreSelectedRequestModel directoryStoreSelectedRequestModel = new DirectoryStoreSelectedRequestModel
		{
			StoreRequest = requestModel.StoreRequest,
			JSONDocuments = new List<string>()
		};
		foreach (T item in requestModel.Directory)
		{
			directoryStoreSelectedRequestModel.JSONDocuments.Add(JsonSerializer.Serialize((Directory)item, _options));
		}
		StringBuilder stringBuilder = _client.CreateURLBuilder("/api/" + ControllerName + "/StoreSelected");
		return await _client.ExecuteAsync<DirectoryEditFieldsWithSameValueResponseModel>(HttpMethod.Post, stringBuilder.ToString(), cancellationToken, directoryStoreSelectedRequestModel);
	}

	public DirectoryEditFieldsWithSameValueResponseModel StoreSelected<T>(DirectoryStoreSelectedRequestModel<T> requestModel) where T : Directory, new()
	{
		return AsyncHelper.RunSync(async () => await StoreSelectedAsync(requestModel));
	}

	public async Task<ArmSoft.SME.Client.Models.Directories.DirectoryDeleteResponseModel> DeleteAsync(short directoryType, ArmSoft.SME.Client.Models.Directories.DirectoryDeleteRequestModel requestModel, CancellationToken cancellationToken = default(CancellationToken))
	{
		ArmSoft.SME.Client.Models.Directories.DirectoryDeleteRequestModel requestObject = new ArmSoft.SME.Client.Models.Directories.DirectoryDeleteRequestModel
		{
			WithoutValidation = requestModel.WithoutValidation,
			Key = requestModel.Key,
			Id = requestModel.Id,
			ThrowException = requestModel.ThrowException
		};
		StringBuilder stringBuilder = _client.CreateURLBuilder($"/api/{ControllerName}/Delete/{directoryType}");
		ArmSoft.SME.Common.Models.Directories.DirectoryDeleteResponseModel directoryDeleteResponseModel = await _client.ExecuteAsync<ArmSoft.SME.Common.Models.Directories.DirectoryDeleteResponseModel>(HttpMethod.Delete, stringBuilder.ToString(), cancellationToken, requestObject);
		return new ArmSoft.SME.Client.Models.Directories.DirectoryDeleteResponseModel
		{
			Key = requestModel.Key,
			Id = requestModel.Id,
			Exception = directoryDeleteResponseModel.Exception
		};
	}

	public ArmSoft.SME.Client.Models.Directories.DirectoryDeleteResponseModel Delete(short directoryType, ArmSoft.SME.Client.Models.Directories.DirectoryDeleteRequestModel requestModel)
	{
		return AsyncHelper.RunSync(async () => await DeleteAsync(directoryType, requestModel));
	}

	public bool ValidateDelete(short directoryType, DirectoryValidateDeleteRequestModel requestModel)
	{
		return AsyncHelper.RunSync(async () => await ValidateDeleteAsync(directoryType, requestModel));
	}

	public async Task<bool> ValidateDeleteAsync(short directoryType, DirectoryValidateDeleteRequestModel requestModel, CancellationToken cancellationToken = default(CancellationToken))
	{
		StringBuilder stringBuilder = _client.CreateURLBuilder($"/api/{ControllerName}/ValidateDelete/{directoryType}");
		return await _client.ExecuteAsync<bool>(HttpMethod.Post, stringBuilder.ToString(), cancellationToken, requestModel);
	}

	public async Task<bool> EditCodeAsync(short directoryType, DirectoryEditCodeRequestModel requestModel, CancellationToken cancellationToken = default(CancellationToken))
	{
		StringBuilder stringBuilder = _client.CreateURLBuilder($"/api/{ControllerName}/EditCode/{directoryType}");
		return await _client.ExecuteAsync<bool>(HttpMethod.Post, stringBuilder.ToString(), cancellationToken, requestModel);
	}

	public bool EditCode(short directoryType, DirectoryEditCodeRequestModel requestModel)
	{
		return AsyncHelper.RunSync(async () => await EditCodeAsync(directoryType, requestModel));
	}

	public Task AddGridRowAsync<T>(short directoryType, string key, string gridName, List<T> rows, CancellationToken cancellationToken = default(CancellationToken))
	{
		DirectoryAddGridRowRequestModel requestObject = new DirectoryAddGridRowRequestModel
		{
			Key = key,
			GridName = gridName,
			RowsJSON = JsonSerializer.Serialize(rows)
		};
		StringBuilder stringBuilder = _client.CreateURLBuilder($"/api/{ControllerName}/AddGridRow/{directoryType}");
		return _client.ExecuteAsync(HttpMethod.Post, stringBuilder.ToString(), cancellationToken, requestObject);
	}

	public void AddGridRow<T>(short directoryType, string key, string gridName, List<T> rows)
	{
		AsyncHelper.RunSync(async () =>
		{
			await AddGridRowAsync(directoryType, key, gridName, rows);
		});
	}

	public Task EditGridRowAsync<T>(short directoryType, string key, string gridName, T oldRow, T newRow, byte[] ts, CancellationToken cancellationToken = default(CancellationToken))
	{
		DirectoryEditGridRowRequestModel requestObject = new DirectoryEditGridRowRequestModel
		{
			Key = key,
			GridName = gridName,
			OldRowJSON = JsonSerializer.Serialize(oldRow),
			NewRowJSON = JsonSerializer.Serialize(newRow),
			TS = ts
		};
		StringBuilder stringBuilder = _client.CreateURLBuilder($"/api/{ControllerName}/EditGridRow/{directoryType}");
		return _client.ExecuteAsync(HttpMethod.Post, stringBuilder.ToString(), cancellationToken, requestObject);
	}

	public void EditGridRow<T>(short directoryType, string key, string gridName, T oldRow, T newRow, byte[] ts)
	{
		AsyncHelper.RunSync(async () =>
		{
			await EditGridRowAsync(directoryType, key, gridName, oldRow, newRow, ts);
		});
	}

	public Task DeleteGridRowAsync<T>(short directoryType, string key, string gridName, T row, byte[] ts, CancellationToken cancellationToken = default(CancellationToken))
	{
		DirectoryDeleteGridRowRequestModel requestObject = new DirectoryDeleteGridRowRequestModel
		{
			Key = key,
			GridName = gridName,
			RowJSON = JsonSerializer.Serialize(row),
			TS = ts
		};
		StringBuilder stringBuilder = _client.CreateURLBuilder($"/api/{ControllerName}/DeleteGridRow/{directoryType}");
		return _client.ExecuteAsync(HttpMethod.Post, stringBuilder.ToString(), cancellationToken, requestObject);
	}

	public void DeleteGridRow<T>(short directoryType, string key, string gridName, T row, byte[] ts)
	{
		AsyncHelper.RunSync(async () =>
		{
			await DeleteGridRowAsync(directoryType, key, gridName, row, ts);
		});
	}

	public Task<byte[]> GetActualTSAsync(short directoryType, Guid isn, CancellationToken cancellationToken = default(CancellationToken))
	{
		StringBuilder stringBuilder = _client.CreateURLBuilder($"/api/{ControllerName}/GetActualTS/{directoryType}/{isn}/");
		return _client.ExecuteAsync<byte[]>(HttpMethod.Get, stringBuilder.ToString(), cancellationToken);
	}

	public byte[] GetActualTS(short directoryType, Guid isn)
	{
		return AsyncHelper.RunSync(async () => await GetActualTSAsync(directoryType, isn));
	}

	public Task<string> GetNextCodeAsync(short directoryType, NextCodeOfferSource offerSource, string prefix, int maxLength, CancellationToken cancellationToken = default(CancellationToken))
	{
		DirectoryNextCodeRequestModel requestObject = new DirectoryNextCodeRequestModel
		{
			OfferSource = offerSource,
			Prefix = prefix,
			MaxLength = maxLength
		};
		StringBuilder stringBuilder = _client.CreateURLBuilder($"/api/{ControllerName}/GetNextCode/{directoryType}/");
		return _client.ExecuteAsync<string>(HttpMethod.Post, stringBuilder.ToString(), cancellationToken, requestObject);
	}

	public string GetNextCode(short directoryType, NextCodeOfferSource offerSource, string prefix, int maxLength)
	{
		return AsyncHelper.RunSync(async () => await GetNextCodeAsync(directoryType, offerSource, prefix, maxLength));
	}

	public async Task<Directory> LoadByBarCodeAsync(short type, string barCode, CancellationToken cancellationToken = default(CancellationToken))
	{
		UrlQuery urlQuery = new UrlQuery();
		urlQuery.Add("type", type);
		urlQuery.Add("barCode", barCode);
		StringBuilder stringBuilder = _client.CreateURLBuilder("/api/" + ControllerName + "/LoadByBarCode", urlQuery);
		return JsonSerializer.Deserialize<Directory>((await _client.ExecuteAsync<DirectoryLoadResponseModel>(HttpMethod.Get, stringBuilder.ToString(), cancellationToken)).JSONDocument, _options);
	}

	public Directory LoadByBarCode(short type, string barCode)
	{
		return AsyncHelper.RunSync(async () => await LoadByBarCodeAsync(type, barCode));
	}

	public string GetJSONDocument<T>(T document) where T : Directory, new()
	{
		return JsonSerializer.Serialize((Directory)document, _options);
	}
}
