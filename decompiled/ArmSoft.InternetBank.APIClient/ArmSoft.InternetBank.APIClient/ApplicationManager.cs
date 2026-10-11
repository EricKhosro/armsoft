using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ArmSoft.InternetBank.APIClient.Core;
using ArmSoft.InternetBank.Web.API.Common;
using ArmSoft.InternetBank.Web.API.Common.Models;
using ArmSoft.OnlineBanking.Common.Enums;
using Newtonsoft.Json.Linq;

namespace ArmSoft.InternetBank.APIClient;

public class ApplicationManager : BaseManager
{
	internal ApplicationManager(Client client)
		: base(client)
	{
	}

	public List<Application> GetApplications(DateTime? dateFrom = null, DateTime? dateTo = null, string number = null, string type = null, int? count = null, DocumentState state = DocumentState.NotSet)
	{
		Dictionary<string, object> queryParams = new Dictionary<string, object>
		{
			["Number"] = number,
			["Type"] = type,
			["DateFrom"] = dateFrom,
			["DateTo"] = dateTo,
			["Period"] = Period.Custom,
			["State"] = state,
			["Count"] = count
		};
		return client.Get<List<Application>>("applications", requireAuthorization: true, null, queryParams);
	}

	public async Task<List<Application>> GetApplicationsAsync(DateTime? dateFrom = null, DateTime? dateTo = null, string number = null, string type = null, int? count = null, DocumentState state = DocumentState.NotSet)
	{
		Dictionary<string, object> queryParams = new Dictionary<string, object>
		{
			["Number"] = number,
			["Type"] = type,
			["DateFrom"] = dateFrom,
			["DateTo"] = dateTo,
			["Period"] = Period.Custom,
			["State"] = state,
			["Count"] = count
		};
		return await client.GetAsync<List<Application>>("applications", requireAuthorization: true, null, queryParams);
	}

	public async Task<T> GetNewApplicationAsync<T>(Guid? templateId = null, string applicationName = "", string objectNumber = "") where T : Application, new()
	{
		Dictionary<string, object> queryParams = new Dictionary<string, object>
		{
			["templateId"] = templateId,
			["applicationName"] = applicationName,
			["objectNumber"] = objectNumber
		};
		T val = await client.GetAsync<T>($"applications/{typeof(T).Name}/create", requireAuthorization: true, null, queryParams);
		if (val is CA { CustomFields: not null } cA)
		{
			foreach (CAField customField in cA.CustomFields)
			{
				if (customField.Properties == null)
				{
					continue;
				}
				foreach (string item in new List<string> { "FilterAccountTypes", "FilterCreditTypes", "DataSource" })
				{
					if (customField.Properties.ContainsKey(item))
					{
						switch (item)
						{
						case "FilterAccountTypes":
							customField.Properties[item] = ((JArray)customField.Properties[item]).ToObject<List<AccountType>>();
							break;
						case "FilterCreditTypes":
							customField.Properties[item] = ((JArray)customField.Properties[item]).ToObject<List<CreditType>>();
							break;
						case "DataSource":
							customField.Properties[item] = ((JArray)customField.Properties[item]).ToObject<List<KeyValuePair<string, string>>>();
							break;
						}
					}
				}
			}
		}
		return val;
	}

	public async Task CreateApplicationAsync<T>(T application, bool ignoreWarnings = false) where T : Application
	{
		Dictionary<string, object> queryParams = new Dictionary<string, object> { ["forceUpdate"] = ignoreWarnings };
		await client.PostAsync($"applications/{typeof(T).Name}/create", requireAuthorization: true, null, queryParams, application);
	}

	public async Task<T> GetApplicationAsync<T>(Guid docId) where T : Application, new()
	{
		T val = await client.GetAsync<T>($"applications/{typeof(T).Name}/{docId}/details", requireAuthorization: true);
		if (val is CA { CustomFields: not null } cA)
		{
			foreach (CAField customField in cA.CustomFields)
			{
				if (customField.Properties == null)
				{
					continue;
				}
				foreach (string item in new List<string> { "FilterAccountTypes", "FilterCreditTypes", "DataSource" })
				{
					if (customField.Properties.ContainsKey(item))
					{
						switch (item)
						{
						case "FilterAccountTypes":
							customField.Properties[item] = ((JArray)customField.Properties[item]).ToObject<List<AccountType>>();
							break;
						case "FilterCreditTypes":
							customField.Properties[item] = ((JArray)customField.Properties[item]).ToObject<List<CreditType>>();
							break;
						case "DataSource":
							customField.Properties[item] = ((JArray)customField.Properties[item]).ToObject<List<KeyValuePair<string, string>>>();
							break;
						}
					}
				}
			}
		}
		return val;
	}

	public async Task UpdateApplicationAsync<T>(T application, bool ignoreWarnings = false) where T : Application
	{
		Dictionary<string, object> queryParams = new Dictionary<string, object> { ["forceUpdate"] = ignoreWarnings };
		Dictionary<string, string> headers = new Dictionary<string, string> { ["X-TimeStamp"] = Convert.ToBase64String(application.TimeStamp) };
		await client.PostAsync($"applications/{typeof(T).Name}/{application.DocId}/update", requireAuthorization: true, null, queryParams, application, headers);
	}

	public async Task<List<ApplicationType>> GetUsableApplicationTypesAsync()
	{
		return await client.GetAsync<List<ApplicationType>>("applications/usableTypes", requireAuthorization: true);
	}

	public async Task<List<ApplicationType>> GetUsableApplicationTypesForListAsync(CustomizationListType listType)
	{
		Dictionary<string, object> formParams = new Dictionary<string, object> { ["ListType"] = listType };
		return await client.GetAsync<List<ApplicationType>>("applications/usableTypesForList", requireAuthorization: true, formParams);
	}

	public async Task<List<ApplicationType>> GetUsableApplicationTypesForObject(CustomizationObjectType objectType, string cardType = null, string accountType = null, string currency = null, string contractNumber = null)
	{
		UsableApplicationTypesParams postBody = new UsableApplicationTypesParams
		{
			ObjectType = objectType,
			CardType = cardType,
			AccountType = accountType,
			Currency = currency,
			ContractNumber = contractNumber
		};
		return (await client.PostAsync<ResponseResult<List<ApplicationType>>>("v2/applications/usableTypesForObject", requireAuthorization: true, null, null, postBody)).Value;
	}

	public async Task<string> GetUrlDataSource(string url, string customizationName, string fieldName)
	{
		Dictionary<string, object> postBody = new Dictionary<string, object>
		{
			["Url"] = url,
			["CustomizationName"] = customizationName,
			["FieldName"] = fieldName
		};
		return await client.PostAsync("applications/ca/urlDataSource", requireAuthorization: true, null, null, postBody);
	}

	public async Task<CADetails> GetCustomizedApplicationDetails(Guid documentId)
	{
		Dictionary<string, object> postBody = new Dictionary<string, object> { ["Id"] = documentId };
		return (await client.PostAsync<ResponseResult<CADetails>>("applications/ca/view", requireAuthorization: true, null, null, postBody)).Value;
	}

	public async Task<List<Link>> GetApplicationLinksAsync(Guid id)
	{
		return await client.GetAsync<List<Link>>($"applications/{id}/links", requireAuthorization: true);
	}

	public async Task SendToEditAsync(Guid documentId)
	{
		await client.PostAsync<object>("document/sendToEdit", requireAuthorization: true, null, null, documentId);
	}

	public async Task<byte[]> GetCAAttachmentAsync(Guid documentId, string fieldName)
	{
		DocumentAttachmentParams postBody = new DocumentAttachmentParams
		{
			DocumentId = documentId,
			FieldName = fieldName
		};
		return (await client.PostAsync<ResponseResult<byte[]>>("applications/ca/getDocumentAttachment", requireAuthorization: true, null, null, postBody)).Value;
	}
}
