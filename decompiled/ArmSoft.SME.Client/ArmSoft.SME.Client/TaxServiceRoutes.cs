using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ArmSoft.SME.Common.Models;
using ArmSoft.SME.Common.Models.ASReport;
using ArmSoft.SME.Common.Models.Documents;

namespace ArmSoft.SME.Client;

public class TaxServiceRoutes
{
	protected readonly ApiClient _client;

	protected virtual string ControllerName => "TaxService";

	public TaxServiceRoutes(ApiClient client)
	{
		_client = client;
	}

	public Task<TaxServiceResponse> MapTaxDocumentsAsync(List<TaxDocument> taxDocuments, DateTime minDate, DateTime maxDate, bool assignTaxInvNumToDocNum, CancellationToken cancellationToken = default(CancellationToken))
	{
		TaxServiceTaxDocumentsRequest requestObject = new TaxServiceTaxDocumentsRequest
		{
			TaxDocuments = taxDocuments,
			MinDate = minDate,
			MaxDate = maxDate,
			AssignTaxInvNumToDocNum = assignTaxInvNumToDocNum
		};
		StringBuilder stringBuilder = _client.CreateURLBuilder("/api/" + ControllerName + "/MapTaxDocuments");
		return _client.ExecuteAsync<TaxServiceResponse>(HttpMethod.Post, stringBuilder.ToString(), cancellationToken, requestObject);
	}

	public TaxServiceResponse MapTaxDocuments(List<TaxDocument> taxDocuments, DateTime minDate, DateTime maxDate, bool assignTaxInvNumToDocNum)
	{
		return AsyncHelper.RunSync(async () => await MapTaxDocumentsAsync(taxDocuments, minDate, maxDate, assignTaxInvNumToDocNum));
	}

	public Task<TaxDocumentRepRow> AssignTaxDocumentDataAsync(Guid isn, string taxSeriaNumber, DateOnly? submitDate, decimal total = 0m, bool assignTaxNumToDocNum = false, short docType = 0, CancellationToken cancellationToken = default(CancellationToken))
	{
		TaxServiceUpdateDataRequest requestObject = new TaxServiceUpdateDataRequest
		{
			ISN = isn,
			TaxSerialNumber = taxSeriaNumber,
			SubmitDate = submitDate,
			Total = total,
			DocType = docType,
			AssignTaxNumToDocNum = assignTaxNumToDocNum
		};
		StringBuilder stringBuilder = _client.CreateURLBuilder("/api/" + ControllerName + "/AssignTaxDocumentData");
		return _client.ExecuteAsync<TaxDocumentRepRow>(HttpMethod.Post, stringBuilder.ToString(), cancellationToken, requestObject);
	}

	public TaxDocumentRepRow AssignTaxDocumentData(Guid isn, string taxSeriaNumber, DateOnly? submitDate, decimal total = 0m, bool assignTaxNumToDocNum = false, short docType = 0)
	{
		return AsyncHelper.RunSync(async () => await AssignTaxDocumentDataAsync(isn, taxSeriaNumber, submitDate, total, assignTaxNumToDocNum, docType));
	}

	public Task<TaxServiceResponse> FindMTMoveDocumentsAsync(List<AccDoc> accDocs, bool assignTaxInvNumToDocNum, CancellationToken cancellationToken = default(CancellationToken))
	{
		TaxServiceAccDocsRequest requestObject = new TaxServiceAccDocsRequest
		{
			AccDocs = accDocs,
			AssignMTMoveInvNums = assignTaxInvNumToDocNum
		};
		StringBuilder stringBuilder = _client.CreateURLBuilder("/api/" + ControllerName + "/FindMTMoveDocuments");
		return _client.ExecuteAsync<TaxServiceResponse>(HttpMethod.Post, stringBuilder.ToString(), cancellationToken, requestObject);
	}

	public TaxServiceResponse FindMTMoveDocuments(List<AccDoc> accDocs, bool assignTaxInvNumToDocNum)
	{
		return AsyncHelper.RunSync(async () => await FindMTMoveDocumentsAsync(accDocs, assignTaxInvNumToDocNum));
	}

	public Task<TaxServiceResponse> MapReceivedTaxDocumentsAsync(List<TaxDocument> taxDocuments, CancellationToken cancellationToken = default(CancellationToken))
	{
		TaxServiceTaxDocumentsRequest requestObject = new TaxServiceTaxDocumentsRequest
		{
			TaxDocuments = taxDocuments
		};
		StringBuilder stringBuilder = _client.CreateURLBuilder("/api/" + ControllerName + "/MapReceivedTaxDocuments");
		return _client.ExecuteAsync<TaxServiceResponse>(HttpMethod.Post, stringBuilder.ToString(), cancellationToken, requestObject);
	}

	public TaxServiceResponse MapReceivedTaxDocuments(List<TaxDocument> taxDocuments)
	{
		return AsyncHelper.RunSync(async () => await MapReceivedTaxDocumentsAsync(taxDocuments));
	}

	public Task<List<TaxDocumentRepRow>> ImportTaxDocumentsAsync(List<TaxDocument> taxDocuments, CancellationToken cancellationToken = default(CancellationToken))
	{
		StringBuilder stringBuilder = _client.CreateURLBuilder("/api/" + ControllerName + "/ImportTaxDocuments");
		return _client.ExecuteAsync<List<TaxDocumentRepRow>>(HttpMethod.Post, stringBuilder.ToString(), cancellationToken, taxDocuments);
	}

	public List<TaxDocumentRepRow> ImportTaxDocuments(List<TaxDocument> taxDocuments)
	{
		return AsyncHelper.RunSync(async () => await ImportTaxDocumentsAsync(taxDocuments));
	}

	public Task<Guid?> GetProductTaxISNAsync(Guid? docISN, int itemId, int rowId = -1, string operation = null, CancellationToken cancellationToken = default(CancellationToken))
	{
		TaxProductRequestModel requestObject = new TaxProductRequestModel
		{
			DocISN = docISN,
			RowId = rowId,
			ItemId = itemId,
			Operation = operation
		};
		StringBuilder stringBuilder = _client.CreateURLBuilder("/api/" + ControllerName + "/GetProductTaxISN");
		return _client.ExecuteAsync<Guid?>(HttpMethod.Post, stringBuilder.ToString(), cancellationToken, requestObject);
	}

	public Guid? GetProductTaxISN(Guid? docISN, int itemId, int rowId = -1, string operation = null)
	{
		return AsyncHelper.RunSync(async () => await GetProductTaxISNAsync(docISN, itemId, rowId, operation));
	}
}
