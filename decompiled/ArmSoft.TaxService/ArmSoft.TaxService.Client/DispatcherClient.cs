using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ArmSoft.TaxService.Enums;
using ArmSoft.TaxService.Models;
using ArmSoft.TaxService.Models.AccDocs;
using ArmSoft.TaxService.Models.Dispatcher;
using ArmSoft.TaxService.Models.Invoices;
using ArmSoft.TaxService.Models.XML;
using ArmSoft.TaxService.Models.XML.AccDoc;

namespace ArmSoft.TaxService.Client;

public class DispatcherClient
{
	private readonly TaxServiceClient client;

	private const string apiUrl = "/api/dispatcher";

	private static readonly string[] separator = new string[1] { "\" : \"" };

	internal DispatcherClient(TaxServiceClient client)
	{
		this.client = client;
	}

	public async Task<SignTaskResponse> CreateSubmittedSignTaskRequestAsync(List<Guid> invoiceIds, CancellationToken cancellationToken = default(CancellationToken))
	{
		SignTaskResponse signTaskResponse = new SignTaskResponse
		{
			RequestId = Guid.NewGuid()
		};
		var requestObject = new
		{
			ids = invoiceIds,
			RequestId = signTaskResponse.RequestId
		};
		StringBuilder stringBuilder = client.CreateURLBuilder("/api/dispatcher/request-issue");
		await client.ExecuteAsync(HttpMethod.Post, stringBuilder.ToString(), cancellationToken, requestObject);
		SignTaskResponse signTaskResponse2 = signTaskResponse;
		signTaskResponse2.State = EncodeDecodeTypes.DecodeSignState(await GetSignTaskStateAsync(signTaskResponse.RequestId));
		signTaskResponse2 = signTaskResponse;
		signTaskResponse2.FailMessage = await GetSignFailedMessageAsync(signTaskResponse.RequestId);
		signTaskResponse2 = signTaskResponse;
		signTaskResponse2.FailedSignTaskItems = await GetFailedListAsync(signTaskResponse.RequestId);
		return signTaskResponse;
	}

	public async Task<SignTaskResponse> CreateSubmittedSignTaskRequestAndSignAsync(List<Guid> invoiceIds, bool alsoSign, PhoneType? phoneType = null, string phone = "", CancellationToken cancellationToken = default(CancellationToken))
	{
		SignTaskResponse signTaskResponse = await CreateSubmittedSignTaskRequestAsync(invoiceIds, cancellationToken);
		if (alsoSign && signTaskResponse.FailedSignTaskItems.Count == 0)
		{
			await SignSubmittedTaskAsync(signTaskResponse, phone, phoneType, cancellationToken);
		}
		return signTaskResponse;
	}

	public SignTaskResponse CreateSubmittedSignTaskRequest(List<Guid> invoiceIds)
	{
		return AsyncHelper.RunSync(async () => await CreateSubmittedSignTaskRequestAsync(invoiceIds));
	}

	public SignTaskResponse CreateSubmittedSignTaskRequestAndSign(List<Guid> invoiceIds, bool alsoSign, PhoneType? phoneType = null, string phone = "", CancellationToken cancellationToken = default(CancellationToken))
	{
		return AsyncHelper.RunSync(async () => await CreateSubmittedSignTaskRequestAndSignAsync(invoiceIds, alsoSign, phoneType, phone, cancellationToken));
	}

	public async Task<SignTaskResponse> CreateReceivedSignTaskRequestAsync(List<Guid> invoiceIds, CancellationToken cancellationToken = default(CancellationToken))
	{
		SignTaskResponse signTaskResponse = new SignTaskResponse
		{
			RequestId = Guid.NewGuid()
		};
		var requestObject = new
		{
			ids = invoiceIds,
			RequestId = signTaskResponse.RequestId
		};
		StringBuilder stringBuilder = client.CreateURLBuilder("/api/dispatcher/request-approve");
		await client.ExecuteAsync(HttpMethod.Post, stringBuilder.ToString(), cancellationToken, requestObject);
		SignTaskResponse signTaskResponse2 = signTaskResponse;
		signTaskResponse2.State = EncodeDecodeTypes.DecodeSignState(await GetSignTaskStateAsync(signTaskResponse.RequestId));
		signTaskResponse2 = signTaskResponse;
		signTaskResponse2.FailMessage = await GetSignFailedMessageAsync(signTaskResponse.RequestId);
		signTaskResponse2 = signTaskResponse;
		signTaskResponse2.FailedSignTaskItems = await GetFailedListAsync(signTaskResponse.RequestId);
		return signTaskResponse;
	}

	public async Task<SignTaskResponse> CreateReceivedSignTaskRequestAndSignAsync(List<Guid> invoiceIds, bool alsoSign, PhoneType? phoneType = null, string phone = "", CancellationToken cancellationToken = default(CancellationToken))
	{
		SignTaskResponse signTaskResponse = await CreateReceivedSignTaskRequestAsync(invoiceIds, cancellationToken);
		if (alsoSign && signTaskResponse.FailedSignTaskItems.Count == 0)
		{
			await SignReceivedTaskAsync(signTaskResponse, phone, phoneType, cancellationToken);
		}
		return signTaskResponse;
	}

	public SignTaskResponse CreateReceivedSignTaskRequest(List<Guid> invoiceIds)
	{
		return AsyncHelper.RunSync(async () => await CreateReceivedSignTaskRequestAsync(invoiceIds));
	}

	public SignTaskResponse CreateReceivedSignTaskRequestAndSign(List<Guid> invoiceIds, bool alsoSign, PhoneType? phoneType = null, string phone = "", CancellationToken cancellationToken = default(CancellationToken))
	{
		return AsyncHelper.RunSync(async () => await CreateReceivedSignTaskRequestAndSignAsync(invoiceIds, alsoSign, phoneType, phone, cancellationToken));
	}

	public async Task<string> GetSignTaskStateAsync(Guid requestId, bool isForSignValidation = true)
	{
		SignTask signTask = await GetSignTaskAsync(requestId);
		string processState = ((!isForSignValidation) ? EncodeDecodeTypes.EncodeSignState(SignState.SignProcessing) : EncodeDecodeTypes.EncodeSignState(SignState.HashCalculating));
		DateTime maxWaitTime = DateTime.Now.AddMinutes(3.0);
		while (signTask.State == processState)
		{
			if (DateTime.Now > maxWaitTime)
			{
				throw new TimeoutException(Resources.Get(ResourceKey.WaitTimeoutExpired));
			}
			Thread.Sleep(1000);
			signTask = await GetSignTaskAsync(requestId);
		}
		return signTask.State;
	}

	public string GetSignTaskState(Guid requestId, bool isForSignValidation = true)
	{
		return AsyncHelper.RunSync(async () => await GetSignTaskStateAsync(requestId, isForSignValidation));
	}

	public async Task<List<FailedSignTaskItems>> GetFailedListAsync(Guid requestId)
	{
		List<SignTaskItemById> list = await GetSignTaskItemByIdAsync(requestId);
		List<FailedSignTaskItems> list2 = new List<FailedSignTaskItems>();
		foreach (SignTaskItemById item in list)
		{
			if (string.IsNullOrWhiteSpace(item.FailMessage) && string.IsNullOrWhiteSpace(item.Warnings))
			{
				continue;
			}
			FailedSignTaskItems failedSignTaskItems = new FailedSignTaskItems
			{
				Id = item.Id,
				InvoiceId = item.InvoiceId,
				SignTaskId = item.SignTaskId,
				InvoiceType = item.InvoiceType,
				Status = item.Status,
				SerialNo = item.SerialNo,
				Errors = new Dictionary<string, string>(),
				Warnings = new List<string>()
			};
			if (!string.IsNullOrWhiteSpace(item.FailMessage))
			{
				string[] array = item.FailMessage.Split('\n');
				foreach (string text in array)
				{
					if (!string.IsNullOrWhiteSpace(text) && text != "{" && text != "}" && text != "  \"code\" : \"fault/client/validation\"," && text != "  \"message\" : \"errors\"," && text != "  \"code\" : \"fault/client/permissions\",")
					{
						string[] array2 = text.Split(separator, StringSplitOptions.None);
						failedSignTaskItems.Errors.Add(array2[0].Remove(0, array2[0].IndexOf('"') + 1), array2[1].Remove(array2[1].LastIndexOf('"')));
					}
				}
			}
			if (!string.IsNullOrWhiteSpace(item.Warnings))
			{
				string[] array = item.Warnings.Split('\n');
				foreach (string text2 in array)
				{
					string text3 = text2.Remove(0, text2.IndexOf('"') + 1);
					text3 = text3.Remove(text3.LastIndexOf('"'));
					failedSignTaskItems.Warnings.Add(text3);
				}
			}
			list2.Add(failedSignTaskItems);
		}
		return list2;
	}

	public List<FailedSignTaskItems> GetFailedList(Guid requestId)
	{
		return AsyncHelper.RunSync(async () => await GetFailedListAsync(requestId));
	}

	public async Task<string> GetSignFailedMessageAsync(Guid requestId)
	{
		string[] array = (await GetSignTaskAsync(requestId)).FailMessage.Split('\n');
		if (array.Length == 1)
		{
			return array[0];
		}
		string[] array2 = array;
		foreach (string text in array2)
		{
			if (text != "{" && text != "}" && !text.Contains("\"code\" : \"fault/server/") && text != "  \"message\" : \"errors\",")
			{
				string[] array3 = text.Split(separator, StringSplitOptions.None);
				return array3[1].Remove(array3[1].LastIndexOf('"'));
			}
		}
		return null;
	}

	public string GetSignFailedMessage(Guid requestId)
	{
		return AsyncHelper.RunSync(async () => await GetSignFailedMessageAsync(requestId));
	}

	public Task<SignTask> GetSignTaskAsync(Guid id, CancellationToken cancellationToken = default(CancellationToken))
	{
		var requestObject = new { id };
		StringBuilder stringBuilder = client.CreateURLBuilder("/api/dispatcher/sign-task-by-id");
		return client.ExecuteAsync<SignTask>(HttpMethod.Post, stringBuilder.ToString(), cancellationToken, requestObject);
	}

	public SignTask GetSignTask(Guid id)
	{
		return AsyncHelper.RunSync(async () => await GetSignTaskAsync(id));
	}

	public async Task<List<SignTaskItemById>> GetSignTaskItemByInvoiceIdAsync(Guid invoiceId, CancellationToken cancellationToken = default(CancellationToken))
	{
		var requestObject = new
		{
			id = invoiceId
		};
		StringBuilder stringBuilder = client.CreateURLBuilder("/api/dispatcher/sign-task-item-by-invoice-id");
		return await client.ExecuteAsync<List<SignTaskItemById>>(HttpMethod.Post, stringBuilder.ToString(), cancellationToken, requestObject);
	}

	public List<SignTaskItemById> GetSignTaskItemByInvoiceId(Guid invoiceId)
	{
		return AsyncHelper.RunSync(async () => await GetSignTaskItemByInvoiceIdAsync(invoiceId));
	}

	public async Task<bool> CanInvoiceBeSignedAsync(Guid invoiceId)
	{
		List<SignTaskItemById> list = await GetSignTaskItemByInvoiceIdAsync(invoiceId);
		if (list == null)
		{
			return true;
		}
		return !list.Exists((SignTaskItemById p) => p.InvoiceId == invoiceId && p.Status == "OK");
	}

	public bool CanInvoiceBeSigned(Guid invoiceId)
	{
		return AsyncHelper.RunSync(async () => await CanInvoiceBeSignedAsync(invoiceId));
	}

	public async Task<List<SignTask>> GetSignTaskListAsync(int pageOffset = 0, int pageLimit = 0, CancellationToken cancellationToken = default(CancellationToken))
	{
		if (pageLimit == 0)
		{
			pageLimit = await GetSignTaskCountAsync(cancellationToken) + 1;
		}
		var requestObject = new
		{
			tin = client.Tin,
			offset = pageOffset,
			limit = pageLimit
		};
		StringBuilder stringBuilder = client.CreateURLBuilder("/api/dispatcher/sign-task-list");
		return await client.ExecuteAsync<List<SignTask>>(HttpMethod.Post, stringBuilder.ToString(), cancellationToken, requestObject);
	}

	public List<SignTask> GetSignTaskList(int pageOffset = 0, int pageLimit = 0)
	{
		return AsyncHelper.RunSync(async () => await GetSignTaskListAsync(pageOffset, pageLimit));
	}

	public async Task<int> GetSignTaskCountAsync(CancellationToken cancellationToken = default(CancellationToken))
	{
		var requestObject = new
		{
			tin = client.Tin
		};
		StringBuilder stringBuilder = client.CreateURLBuilder("/api/dispatcher/sign-task-count");
		return await client.ExecuteAsync<int>(HttpMethod.Post, stringBuilder.ToString(), cancellationToken, requestObject);
	}

	public async Task RemoveSignTaskAsync(Guid id, CancellationToken cancellationToken = default(CancellationToken))
	{
		var requestObject = new
		{
			id = id,
			tin = client.Tin
		};
		StringBuilder stringBuilder = client.CreateURLBuilder("/api/dispatcher/sign-task-remove");
		await client.ExecuteAsync<List<SignTask>>(HttpMethod.Post, stringBuilder.ToString(), cancellationToken, requestObject);
	}

	public void RemoveSignTask(Guid id)
	{
		AsyncHelper.RunSync(async () =>
		{
			await RemoveSignTaskAsync(id);
		});
	}

	public async Task<List<SignTaskItemById>> GetSignTaskItemByIdAsync(Guid requestId, CancellationToken cancellationToken = default(CancellationToken))
	{
		var requestObject = new
		{
			id = requestId
		};
		StringBuilder stringBuilder = client.CreateURLBuilder("/api/dispatcher/sign-task-item-list-by-sign-id");
		return await client.ExecuteAsync<List<SignTaskItemById>>(HttpMethod.Post, stringBuilder.ToString(), cancellationToken, requestObject);
	}

	public List<SignTaskItemById> GetSignTaskItemById(Guid requestId)
	{
		return AsyncHelper.RunSync(async () => await GetSignTaskItemByIdAsync(requestId));
	}

	public async Task<Guid> GetSignTaskRequestIdAsync(Guid invoiceId, CancellationToken cancellationToken = default(CancellationToken))
	{
		var requestObject = new { invoiceId };
		StringBuilder stringBuilder = client.CreateURLBuilder("/api/dispatcher/sign-task-item-check-lock");
		return (await client.ExecuteAsync<InvoiceLockStatus>(HttpMethod.Post, stringBuilder.ToString(), cancellationToken, requestObject)).SignTaskId;
	}

	public Guid GetSignTaskRequestId(Guid invoiceId)
	{
		return AsyncHelper.RunSync(async () => await GetSignTaskRequestIdAsync(invoiceId));
	}

	public async Task SignSubmittedTaskByMTSAsync(Guid requestId, string phone, CancellationToken cancellationToken = default(CancellationToken))
	{
		try
		{
			var requestObject = new { requestId, phone };
			StringBuilder stringBuilder = client.CreateURLBuilder("/api/dispatcher/issue-mts");
			await client.ExecuteAsync(HttpMethod.Post, stringBuilder.ToString(), cancellationToken, requestObject);
		}
		catch (Exception ex) when (ex is ApiException ex2 && ex2.Code == "EntityNotFound")
		{
			throw new ApiException(Resources.Get(ResourceKey.SignStatusChanged));
		}
	}

	public void SignSubmittedTaskByMTS(Guid requestId, string phone)
	{
		AsyncHelper.RunSync(async () =>
		{
			await SignSubmittedTaskByMTSAsync(requestId, phone);
		});
	}

	public async Task SignSubmittedTaskByNikitaAsync(Guid requestId, string phone, CancellationToken cancellationToken = default(CancellationToken))
	{
		try
		{
			var requestObject = new { requestId, phone };
			StringBuilder stringBuilder = client.CreateURLBuilder("/api/dispatcher/issue-nikita");
			await client.ExecuteAsync(HttpMethod.Post, stringBuilder.ToString(), cancellationToken, requestObject);
		}
		catch (Exception ex) when (ex is ApiException ex2 && ex2.Code == "EntityNotFound")
		{
			throw new ApiException(Resources.Get(ResourceKey.SignStatusChanged));
		}
	}

	public void SignSubmittedTaskByNikita(Guid requestId, string phone)
	{
		AsyncHelper.RunSync(async () =>
		{
			await SignSubmittedTaskByNikitaAsync(requestId, phone);
		});
	}

	public async Task SignSubmittedTaskAsync(SignTaskResponse signTaskResponse, string phone = "", PhoneType? phoneType = null, CancellationToken cancellationToken = default(CancellationToken))
	{
		if (!phoneType.HasValue)
		{
			throw new ApiException(Resources.Get(ResourceKey.PhoneOperatorCannotBeEmpty));
		}
		if (string.IsNullOrWhiteSpace(phone))
		{
			throw new ApiException(Resources.Get(ResourceKey.PhoneNumberCannotBeEmpty));
		}
		if (phoneType != PhoneType.Nikita)
		{
			await SignSubmittedTaskByMTSAsync(signTaskResponse.RequestId, phone, cancellationToken);
		}
		else
		{
			await SignSubmittedTaskByNikitaAsync(signTaskResponse.RequestId, phone, cancellationToken);
		}
		SignTaskResponse signTaskResponse2 = signTaskResponse;
		signTaskResponse2.State = EncodeDecodeTypes.DecodeSignState(await GetSignTaskStateAsync(signTaskResponse.RequestId, isForSignValidation: false));
		signTaskResponse2 = signTaskResponse;
		signTaskResponse2.FailMessage = await GetSignFailedMessageAsync(signTaskResponse.RequestId);
	}

	public void SignSubmittedTask(SignTaskResponse signTaskResponse, string phone, PhoneType phoneType)
	{
		AsyncHelper.RunSync(async () =>
		{
			await SignSubmittedTaskAsync(signTaskResponse, phone, phoneType);
		});
	}

	public async Task SignReceivedTaskByMTSAsync(Guid requestId, string phone, CancellationToken cancellationToken = default(CancellationToken))
	{
		try
		{
			var requestObject = new { requestId, phone };
			StringBuilder stringBuilder = client.CreateURLBuilder("/api/dispatcher/approve-mts");
			await client.ExecuteAsync(HttpMethod.Post, stringBuilder.ToString(), cancellationToken, requestObject);
		}
		catch (Exception ex) when (ex is ApiException ex2 && ex2.Code == "EntityNotFound")
		{
			throw new ApiException(Resources.Get(ResourceKey.SignStatusChanged));
		}
	}

	public void SignReceivedTaskByMTS(Guid requestId, string phone)
	{
		AsyncHelper.RunSync(async () =>
		{
			await SignReceivedTaskByMTSAsync(requestId, phone);
		});
	}

	public async Task SignReceivedTaskByNikitaAsync(Guid requestId, string phone, CancellationToken cancellationToken = default(CancellationToken))
	{
		try
		{
			var requestObject = new { requestId, phone };
			StringBuilder stringBuilder = client.CreateURLBuilder("/api/dispatcher/approve-nikita");
			await client.ExecuteAsync(HttpMethod.Post, stringBuilder.ToString(), cancellationToken, requestObject);
		}
		catch (Exception ex) when (ex is ApiException ex2 && ex2.Code == "EntityNotFound")
		{
			throw new ApiException(Resources.Get(ResourceKey.SignStatusChanged));
		}
	}

	public void SignReceivedTaskByNikita(Guid requestId, string phone)
	{
		AsyncHelper.RunSync(async () =>
		{
			await SignReceivedTaskByNikitaAsync(requestId, phone);
		});
	}

	public async Task SignReceivedTaskAsync(SignTaskResponse signTaskResponse, string phone = "", PhoneType? phoneType = null, CancellationToken cancellationToken = default(CancellationToken))
	{
		if (!phoneType.HasValue)
		{
			throw new ApiException(Resources.Get(ResourceKey.PhoneOperatorCannotBeEmpty));
		}
		if (string.IsNullOrWhiteSpace(phone))
		{
			throw new ApiException(Resources.Get(ResourceKey.PhoneNumberCannotBeEmpty));
		}
		if (phoneType != PhoneType.Nikita)
		{
			await SignReceivedTaskByMTSAsync(signTaskResponse.RequestId, phone, cancellationToken);
		}
		else
		{
			await SignReceivedTaskByNikitaAsync(signTaskResponse.RequestId, phone, cancellationToken);
		}
		SignTaskResponse signTaskResponse2 = signTaskResponse;
		signTaskResponse2.State = EncodeDecodeTypes.DecodeSignState(await GetSignTaskStateAsync(signTaskResponse.RequestId, isForSignValidation: false));
		signTaskResponse2 = signTaskResponse;
		signTaskResponse2.FailMessage = await GetSignFailedMessageAsync(signTaskResponse.RequestId);
	}

	public void SignReceivedTask(SignTaskResponse signTaskResponse, string phone, PhoneType phoneType)
	{
		AsyncHelper.RunSync(async () =>
		{
			await SignReceivedTaskAsync(signTaskResponse, phone, phoneType);
		});
	}

	public byte[] GetPdfFile(string serialNumber, bool includeDependents = false)
	{
		return AsyncHelper.RunSync(async () => await GetPdfFileAsync(serialNumber, includeDependents));
	}

	public async Task<byte[]> GetPdfFileAsync(string serialNumber, bool includeDependents = false, CancellationToken cancellationToken = default(CancellationToken))
	{
		Guid id = (await client.Invoice.GetAsync(serialNumber, cancellationToken))?.Id ?? Guid.Empty;
		return await GetPdfFileAsync(id, includeDependents, cancellationToken);
	}

	public async Task<byte[]> GetPdfFileAsync(List<Guid> ids, bool includeDependents = false, CancellationToken cancellationToken = default(CancellationToken))
	{
		var requestObject = new { ids, includeDependents };
		StringBuilder stringBuilder = client.CreateURLBuilder("/api/dispatcher/pdf-export");
		return await client.ExecuteStreamAsync<byte[]>(HttpMethod.Post, stringBuilder.ToString(), cancellationToken, requestObject);
	}

	public async Task<byte[]> GetPdfFileAsync(Guid id, bool includeDependents = false, CancellationToken cancellationToken = default(CancellationToken))
	{
		var requestObject = new
		{
			ids = new List<Guid> { id },
			includeDependents = includeDependents
		};
		StringBuilder stringBuilder = client.CreateURLBuilder("/api/dispatcher/pdf-export");
		return await client.ExecuteStreamAsync<byte[]>(HttpMethod.Post, stringBuilder.ToString(), cancellationToken, requestObject);
	}

	public byte[] GetPdfFile(List<Guid> ids, bool includeDependents = false)
	{
		return AsyncHelper.RunSync(async () => await GetPdfFileAsync(ids, includeDependents));
	}

	public byte[] GetPdfFile(Guid id, bool includeDependents = false)
	{
		return AsyncHelper.RunSync(async () => await GetPdfFileAsync(id, includeDependents));
	}

	public async Task<byte[]> GetXmlFileAsync(List<Guid> ids, CancellationToken cancellationToken = default(CancellationToken))
	{
		var requestObject = new { ids };
		StringBuilder stringBuilder = client.CreateURLBuilder("/api/dispatcher/xml-export");
		return await client.ExecuteStreamAsync<byte[]>(HttpMethod.Post, stringBuilder.ToString(), cancellationToken, requestObject);
	}

	public async Task<byte[]> GetXmlFileAsync(Guid id, CancellationToken cancellationToken = default(CancellationToken))
	{
		var requestObject = new
		{
			ids = new List<Guid> { id }
		};
		StringBuilder stringBuilder = client.CreateURLBuilder("/api/dispatcher/xml-export");
		return await client.ExecuteStreamAsync<byte[]>(HttpMethod.Post, stringBuilder.ToString(), cancellationToken, requestObject);
	}

	public byte[] GetXmlFile(List<Guid> ids)
	{
		return AsyncHelper.RunSync(async () => await GetXmlFileAsync(ids));
	}

	public byte[] GetXmlFile(Guid id)
	{
		return AsyncHelper.RunSync(async () => await GetXmlFileAsync(id));
	}

	public List<InvoiceBase> GetInvoicesFromXML(string fileFullName)
	{
		List<InvoiceBase> list = new List<InvoiceBase>();
		object[] items = XMLExport.GetInvoiceFromXMLFile(fileFullName).Items;
		for (int i = 0; i < items.Length; i++)
		{
			if (items[i] is SignedData signedData && signedData.Data.SignableData is Invoice invoice)
			{
				if (invoice.Type == 1)
				{
					list.Add(Converter.ConvertXMLInvoiceToGood(signedData));
				}
				else if (invoice.Type == 2)
				{
					list.Add(Converter.ConvertXMLInvoiceToExcise(signedData));
				}
				else if (invoice.Type == 3)
				{
					list.Add(Converter.ConvertXMLInvoiceToService(signedData));
				}
				else if (invoice.Type == 4)
				{
					list.Add(Converter.ConvertXMLInvoiceToLeasing(signedData));
				}
			}
		}
		return list;
	}

	public Task ImportXmlAsync(string fileFullName, CancellationToken cancellationToken = default(CancellationToken))
	{
		var requestObject = new
		{
			url = fileFullName,
			tin = client.Tin
		};
		StringBuilder stringBuilder = client.CreateURLBuilder("/api/dispatcher/xml-import");
		return client.ExecuteAsync<object>(HttpMethod.Post, stringBuilder.ToString(), cancellationToken, requestObject);
	}

	public void ImportXml(string fileFullName)
	{
		AsyncHelper.RunSync(async () =>
		{
			await ImportXmlAsync(fileFullName);
		});
	}

	public void CreateInvoicesXMLFile(string fileFullName, List<ArmSoft.TaxService.Models.Invoices.Good> goodDocs = null, List<Excise> exciseDocs = null, List<Service> serviceDocs = null)
	{
		List<Invoice> list = new List<Invoice>();
		if (goodDocs != null)
		{
			list.AddRange(Converter.ConvertGoodToXMLInvoice(goodDocs));
		}
		if (exciseDocs != null)
		{
			list.AddRange(Converter.ConvertExciseToXMLInvoice(exciseDocs));
		}
		if (serviceDocs != null)
		{
			list.AddRange(Converter.ConvertServiceToXMLInvoice(serviceDocs));
		}
		if (list.Count > 0)
		{
			XMLExport.CreateInvoiceXMLFile(list, fileFullName);
		}
	}

	public void CreateAccDocsXMLFile(string fileFullName, List<AccDocGoods> accGoodDocs = null, List<AccDocServices> accServiceDocs = null, List<AccDocTransportation> accTransportationDocs = null)
	{
		List<AccountingDocument> list = new List<AccountingDocument>();
		if (accGoodDocs != null)
		{
			list.AddRange(Converter.ConvertGoodToXMLAccDoc(accGoodDocs));
		}
		if (accServiceDocs != null)
		{
			list.AddRange(Converter.ConvertServiceToXMLAccDoc(accServiceDocs));
		}
		if (accTransportationDocs != null)
		{
			list.AddRange(Converter.ConvertTransportationToXMLAccDoc(accTransportationDocs));
		}
		if (list.Count > 0)
		{
			XMLExport.CreateAccDocXMLFile(list, fileFullName);
		}
	}

	public List<AccDocBase> GetAccDocsFromXML(string fileFullName)
	{
		List<AccDocBase> list = new List<AccDocBase>();
		object[] items = XMLExport.GetAccDocFromXMLFile(fileFullName).Items;
		for (int i = 0; i < items.Length; i++)
		{
			if (items[i] is SignedAccDocData signedAccDocData && signedAccDocData.Data.SignableData is AccountingDocument accountingDocument)
			{
				if (accountingDocument.Type == 1)
				{
					list.Add(Converter.ConvertXMLAccDocToAccDocGood(signedAccDocData));
				}
				else if (accountingDocument.Type == 3)
				{
					list.Add(Converter.ConvertXMLAccDocToAccDocService(signedAccDocData));
				}
				else if (accountingDocument.Type == 2)
				{
					list.Add(Converter.ConvertXMLAccDocToAccDocTransportation(signedAccDocData));
				}
			}
		}
		return list;
	}

	public TaxDocType GetXMLTaxType(string fileFullName)
	{
		return XMLExport.GetXMLTaxType(fileFullName);
	}

	private async Task<FailureResponse> ValidateAndAssignMarkingsAsync(Guid invoiceId, List<string> markings, string tempFileFullName = "", CancellationToken cancellationToken = default(CancellationToken))
	{
		string filePath = (string.IsNullOrWhiteSpace(tempFileFullName) ? Path.Combine(Path.GetTempPath(), invoiceId.ToString() + ".csv") : tempFileFullName);
		ExportMarkings(markings, filePath);
		FailureResponse result = await ValidateAndAssignMarkingsAsync(invoiceId, filePath, cancellationToken);
		RemoveMarkingsFile(filePath);
		return result;
	}

	private async Task<FailureResponse> ValidateAndAssignMarkingsAsync(Guid invoiceId, string fileFullName, CancellationToken cancellationToken = default(CancellationToken))
	{
		FailureResponse response = null;
		try
		{
			UrlQuery urlQuery = new UrlQuery();
			urlQuery.Add("invoiceId", invoiceId);
			StringBuilder stringBuilder = client.CreateURLBuilder("/api/import/csv", urlQuery);
			await client.ExecuteAsync<FailureResponse>(HttpMethod.Post, stringBuilder.ToString(), cancellationToken, null, fileFullName);
		}
		catch (ApiException ex) when (ex.Code == "fault/client/codes")
		{
			response = new FailureResponse
			{
				Code = ex.Code,
				Message = ex.Message,
				Codes = ex.FailedCodes
			};
		}
		return response;
	}

	private async Task AssignMarkingsAsync(Guid invoiceId, List<string> markings, CancellationToken cancellationToken = default(CancellationToken))
	{
		var requestObject = new
		{
			invoiceId = invoiceId,
			codes = markings
		};
		StringBuilder stringBuilder = client.CreateURLBuilder("/api/dispatcher/assign-codes");
		await client.ExecuteAsync(HttpMethod.Post, stringBuilder.ToString(), cancellationToken, requestObject);
	}

	private async Task IssuedAssignMarkingsAsync(string serialNumber, List<string> markings, CancellationToken cancellationToken = default(CancellationToken))
	{
		var requestObject = new
		{
			serialNo = serialNumber,
			codes = markings
		};
		StringBuilder stringBuilder = client.CreateURLBuilder("/api/dispatcher/issued-assign-codes");
		await client.ExecuteAsync(HttpMethod.Post, stringBuilder.ToString(), cancellationToken, requestObject, isEmarkRequest: true);
	}

	public async Task<byte[]> ExportMarkingsAsync(Guid invoiceId, CancellationToken cancellationToken = default(CancellationToken))
	{
		var requestObject = new { invoiceId };
		StringBuilder stringBuilder = client.CreateURLBuilder("/api/dispatcher/csv-export");
		return await client.ExecuteStreamAsync<byte[]>(HttpMethod.Post, stringBuilder.ToString(), cancellationToken, requestObject);
	}

	public byte[] ExportMarkings(Guid invoiceId)
	{
		return AsyncHelper.RunSync(async () => await ExportMarkingsAsync(invoiceId));
	}

	public void ExportMarkings(List<string> markings, string fileFullName)
	{
		WriteLinesAsCsv(fileFullName, markings);
	}

	public static string EscapeField(string field)
	{
		if (field == null)
		{
			return "";
		}
		bool num = field.Contains(",") || field.Contains("\"") || field.Contains("\n") || field.Contains("\r");
		string text = field.Replace("\"", "\"\"");
		if (!num)
		{
			return text;
		}
		return "\"" + text + "\"";
	}

	public static void WriteLinesAsCsv(string fileFullName, IEnumerable<string> lines)
	{
		using FileStream stream = XMLExport.CreateFileStream(fileFullName, FileMode.Create);
		using StreamWriter streamWriter = new StreamWriter(stream);
		foreach (string line in lines)
		{
			streamWriter.WriteLine(EscapeField(line));
		}
	}

	public void RemoveMarkingsFile(string fileFullName)
	{
		try
		{
			if (File.Exists(fileFullName))
			{
				File.Delete(fileFullName);
				return;
			}
			throw new ApiException(string.Format(Resources.Get(ResourceKey.FileDoesNotExist), fileFullName));
		}
		catch (UnauthorizedAccessException)
		{
			throw new ApiException(string.Format(Resources.Get(ResourceKey.IncorrectDirectoryPath), fileFullName));
		}
		catch (IOException)
		{
			throw new ApiException(string.Format(Resources.Get(ResourceKey.IncorrectDirectoryPath), fileFullName));
		}
		catch (ArgumentException)
		{
			throw new ApiException(string.Format(Resources.Get(ResourceKey.IncorrectDirectoryPath), fileFullName));
		}
		catch (NotSupportedException)
		{
			throw new ApiException(string.Format(Resources.Get(ResourceKey.IncorrectDirectoryPath), fileFullName));
		}
	}

	public async Task<FailureResponse> ImportMarkingsAsync(Guid id, List<string> markings, bool rewrite, bool ignoreWarnings = false, string fileFullName = "", CancellationToken cancellationToken = default(CancellationToken))
	{
		FailureResponse response = null;
		TaxDoc taxDoc = await client.Invoice.GetInvoiceAsync(id, cancellationToken);
		if (taxDoc == null)
		{
			throw new ApiException(string.Format(Resources.Get(ResourceKey.DocNotExistWithId), id));
		}
		if (taxDoc.IssuedAt.HasValue)
		{
			if (taxDoc.ApprovedAt.HasValue)
			{
				throw new ApiException(string.Format(Resources.Get(ResourceKey.ImpossibleReflectMarkings), taxDoc.SerialNo));
			}
			await client.Dispatcher.IssuedAssignMarkingsAsync(taxDoc.SerialNo, markings, cancellationToken);
		}
		else
		{
			if (taxDoc.HasCodes)
			{
				if (!rewrite)
				{
					throw new ApiException(Resources.Get(ResourceKey.MarkingsAreAlreadyReflected));
				}
				try
				{
					await client.Dispatcher.RemoveDraftInvoiceMarkingsAsync(id, cancellationToken);
				}
				catch (Exception)
				{
				}
			}
			try
			{
				response = await client.Dispatcher.ValidateAndAssignMarkingsAsync(id, markings, fileFullName, cancellationToken);
			}
			catch (ApiException ex2) when (ex2.Code != "fault/client/codes")
			{
				throw;
			}
			if (ignoreWarnings && response != null)
			{
				await client.Dispatcher.AssignMarkingsAsync(id, markings, cancellationToken);
			}
		}
		return response;
	}

	public FailureResponse ImportMarkings(Guid id, List<string> markings, bool rewrite, bool ignoreWarnings = false, string fileFullName = "")
	{
		return AsyncHelper.RunSync(async () => await ImportMarkingsAsync(id, markings, rewrite, ignoreWarnings, fileFullName));
	}

	public async Task RemoveDraftInvoiceMarkingsAsync(Guid invoiceId, CancellationToken cancellationToken = default(CancellationToken))
	{
		var requestObject = new { invoiceId };
		StringBuilder stringBuilder = client.CreateURLBuilder("/api/dispatcher/remove-emark-codes");
		await client.ExecuteAsync(HttpMethod.Post, stringBuilder.ToString(), cancellationToken, requestObject);
	}

	public void RemoveDraftInvoiceMarkings(Guid invoiceId)
	{
		AsyncHelper.RunSync(async () =>
		{
			await RemoveDraftInvoiceMarkingsAsync(invoiceId);
		});
	}

	public async Task RemoveDraftAsync(AccDocType accDocType, Guid id)
	{
		switch (accDocType)
		{
		case AccDocType.AccDocGoods:
			await client.AccDocGoods.RemoveDraftAsync(id);
			break;
		case AccDocType.AccDocTransportation:
			await client.AccDocTransportation.RemoveDraftAsync(id);
			break;
		case AccDocType.AccDocServices:
			await client.AccDocServices.RemoveDraftAsync(id);
			break;
		case AccDocType.AccDocExportEEU:
			await client.AccDocExportEEU.RemoveDraftAsync(id);
			break;
		case AccDocType.AccDocTraceableG:
			await client.AccDocTraceableG.RemoveDraftAsync(id);
			break;
		}
	}

	public void RemoveDraft(AccDocType accDocType, Guid id)
	{
		AsyncHelper.RunSync(async () =>
		{
			await RemoveDraftAsync(accDocType, id);
		});
	}

	public async Task RemoveDraftAsync(InvoiceType invoiceType, Guid id)
	{
		switch (invoiceType)
		{
		case InvoiceType.Excise:
			await client.Excise.RemoveDraftAsync(id);
			break;
		case InvoiceType.Goods:
			await client.Goods.RemoveDraftAsync(id);
			break;
		case InvoiceType.Services:
			await client.Services.RemoveDraftAsync(id);
			break;
		case InvoiceType.VatReturn:
			await client.VatRefund.RemoveDraftAsync(id);
			break;
		case InvoiceType.Leasing:
			await client.Leasing.RemoveDraftAsync(id);
			break;
		}
	}

	public void RemoveDraft(InvoiceType invoiceType, Guid id)
	{
		AsyncHelper.RunSync(async () =>
		{
			await RemoveDraftAsync(invoiceType, id);
		});
	}
}
