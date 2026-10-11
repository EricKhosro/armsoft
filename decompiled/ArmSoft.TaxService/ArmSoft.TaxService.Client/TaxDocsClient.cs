using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ArmSoft.TaxService.Enums;
using ArmSoft.TaxService.Models;

namespace ArmSoft.TaxService.Client;

public class TaxDocsClient
{
	private readonly TaxServiceClient client;

	private const string apiUrl = "/api/invoice";

	internal TaxDocsClient(TaxServiceClient client)
	{
		this.client = client;
	}

	public Task<int> GetCountAsync(string condition, CancellationToken cancellationToken = default(CancellationToken))
	{
		var requestObject = new { condition };
		StringBuilder stringBuilder = client.CreateURLBuilder("/api/invoice/invoice-count");
		return client.ExecuteAsync<int>(HttpMethod.Post, stringBuilder.ToString(), cancellationToken, requestObject);
	}

	public Task<string> GetDocTypeAsync(Guid id, CancellationToken cancellationToken = default(CancellationToken))
	{
		var requestObject = new { id };
		StringBuilder stringBuilder = client.CreateURLBuilder("/api/invoice/invoice-by-id");
		return client.ExecuteAsync<string>(HttpMethod.Post, stringBuilder.ToString(), cancellationToken, requestObject);
	}

	public string GetDocType(Guid id)
	{
		return AsyncHelper.RunSync(async () => await GetDocTypeAsync(id));
	}

	public async Task<TaxDoc> GetInvoiceAsync(Guid id, CancellationToken cancellationToken = default(CancellationToken))
	{
		List<TaxDoc> list = await GetInvoiceListAsync(new List<Guid> { id }, cancellationToken);
		if (list != null && list.Count > 0)
		{
			return list[0];
		}
		return null;
	}

	public TaxDoc GetInvoice(Guid id)
	{
		return AsyncHelper.RunSync(async () => await GetInvoiceAsync(id));
	}

	public Task<List<TaxDoc>> GetInvoiceListAsync(List<Guid> ids, CancellationToken cancellationToken = default(CancellationToken))
	{
		var requestObject = new { ids };
		StringBuilder stringBuilder = client.CreateURLBuilder("/api/invoice/invoice-list-by-id");
		return client.ExecuteAsync<List<TaxDoc>>(HttpMethod.Post, stringBuilder.ToString(), cancellationToken, requestObject);
	}

	public List<TaxDoc> GetInvoiceList(List<Guid> id)
	{
		return AsyncHelper.RunSync(async () => await GetInvoiceListAsync(id));
	}

	public async Task<TaxDoc> GetAsync(string serialNumber, CancellationToken cancellationToken = default(CancellationToken))
	{
		var requestObject = new
		{
			serialNo = serialNumber
		};
		StringBuilder stringBuilder = client.CreateURLBuilder("/api/invoice/invoice-by-serial-no");
		return await client.ExecuteAsync<TaxDoc>(HttpMethod.Post, stringBuilder.ToString(), cancellationToken, requestObject);
	}

	public TaxDoc Get(string serialNumber)
	{
		return AsyncHelper.RunSync(async () => await GetAsync(serialNumber));
	}

	private async Task<List<TaxDoc>> GetListAsync(Status status, string condition, int pageOffset = 0, int pageLimit = 0, string sortCol = "createdAt", bool sortAsc = false, CancellationToken cancellationToken = default(CancellationToken))
	{
		if (pageLimit == 0)
		{
			pageLimit = await GetCountAsync(condition, cancellationToken) + 1;
		}
		var requestObject = new
		{
			condition = condition,
			pageOffset = pageOffset,
			pageLimit = pageLimit,
			sortCol = sortCol,
			sortAsc = sortAsc,
			status = EncodeDecodeTypes.EncodeStatus(status)
		};
		StringBuilder stringBuilder = client.CreateURLBuilder("/api/invoice/invoice-list");
		return await client.ExecuteAsync<List<TaxDoc>>(HttpMethod.Post, stringBuilder.ToString(), cancellationToken, requestObject);
	}

	public async Task<List<TaxDoc>> GetListAsync(ReceivedOrSubmittedType type, Status status, TaxDocType taxDocType = TaxDocType.None, DateTime? createdStartDate = null, DateTime? createdEndDate = null, DateTime? issuedStartDate = null, DateTime? issuedEndDate = null, string supplierTin = "", string supplierName = "", string buyerTin = "", string buyerName = "", string serialNumber = "", DateTime? supplyStartDate = null, DateTime? supplyEndDate = null, int pageOffset = 0, int pageLimit = 0, string sortCol = "createdAt", bool sortAsc = false, CorrectionState correctionState = CorrectionState.None, ApprovalState approvalState = ApprovalState.None, bool showOnlyCurrentUser = false, CancellationState cancellationState = CancellationState.None, List<short> invoiceTypes = null, SupplierTaxRegime supplierTaxRegime = SupplierTaxRegime.None, CancellationToken cancellationToken = default(CancellationToken))
	{
		if ((!createdStartDate.HasValue || !createdEndDate.HasValue) && (!issuedStartDate.HasValue || !issuedEndDate.HasValue) && (!supplyStartDate.HasValue || !supplyEndDate.HasValue))
		{
			if (string.IsNullOrWhiteSpace(serialNumber))
			{
				if (status == Status.Draft)
				{
					throw new ApiException(Resources.Get(ResourceKey.NeedToFillDateTime));
				}
				throw new ApiException(Resources.Get(ResourceKey.NeedToFillDateTimeOrSerialNumber));
			}
			TaxDoc taxDoc = await GetAsync(serialNumber, cancellationToken);
			issuedStartDate = taxDoc?.IssuedAt.Value.Date;
			issuedEndDate = taxDoc?.IssuedAt.Value.Date;
		}
		List<TaxDoc> taxDocs = new List<TaxDoc>();
		(FilterDateType name, DateTime startDate, DateTime endDate) smallestDateInterval = GetSmallestDateInterval(createdStartDate, createdEndDate, issuedStartDate, issuedEndDate, supplyStartDate, supplyEndDate);
		List<(DateTime, DateTime)> dateRange = GetDateRange(smallestDateInterval.startDate, smallestDateInterval.endDate);
		foreach (var item3 in dateRange)
		{
			DateTime item = item3.Item1;
			DateTime item2 = item3.Item2;
			string condition = smallestDateInterval.name switch
			{
				FilterDateType.Created => GetCondition(type, taxDocType, item, item2, issuedStartDate, issuedEndDate, supplierTin, supplierName, buyerTin, buyerName, serialNumber, supplyStartDate, supplyEndDate, correctionState, approvalState, cancellationState, showOnlyCurrentUser, invoiceTypes, supplierTaxRegime), 
				FilterDateType.Issued => GetCondition(type, taxDocType, createdStartDate, createdEndDate, item, item2, supplierTin, supplierName, buyerTin, buyerName, serialNumber, supplyStartDate, supplyEndDate, correctionState, approvalState, cancellationState, showOnlyCurrentUser, invoiceTypes, supplierTaxRegime), 
				FilterDateType.Supply => GetCondition(type, taxDocType, createdStartDate, createdEndDate, issuedStartDate, issuedEndDate, supplierTin, supplierName, buyerTin, buyerName, serialNumber, item, item2, correctionState, approvalState, cancellationState, showOnlyCurrentUser, invoiceTypes, supplierTaxRegime), 
				_ => throw new ApiException("Something went wrong"), 
			};
			List<TaxDoc> list = taxDocs;
			list.AddRange(await GetListAsync(status, condition, pageOffset, pageLimit, sortCol, sortAsc, cancellationToken));
		}
		return taxDocs;
	}

	private static (FilterDateType name, DateTime startDate, DateTime endDate) GetSmallestDateInterval(DateTime? createdStartDate, DateTime? createdEndDate, DateTime? issuedStartDate, DateTime? issuedEndDate, DateTime? supplyStartDate, DateTime? supplyEndDate)
	{
		return new List<(FilterDateType, DateTime?, DateTime?, int?)>
		{
			(FilterDateType.Created, createdStartDate, createdEndDate, (createdStartDate.HasValue && createdEndDate.HasValue) ? new int?((createdEndDate.Value - createdStartDate.Value).Days) : ((int?)null)),
			(FilterDateType.Issued, issuedStartDate, issuedEndDate, (issuedStartDate.HasValue && issuedEndDate.HasValue) ? new int?((issuedEndDate.Value - issuedStartDate.Value).Days) : ((int?)null)),
			(FilterDateType.Supply, supplyStartDate, supplyEndDate, (supplyStartDate.HasValue && supplyEndDate.HasValue) ? new int?((supplyEndDate.Value - supplyStartDate.Value).Days) : ((int?)null))
		}.Where(((FilterDateType name, DateTime? startDate, DateTime? endDate, int? days) i) => i.days.HasValue).OrderBy(((FilterDateType name, DateTime? startDate, DateTime? endDate, int? days) i) => i.days.Value).Select(((FilterDateType name, DateTime? startDate, DateTime? endDate, int? days) i) => (name: i.name, i.startDate.Value, i.endDate.Value))
			.FirstOrDefault();
	}

	private static List<(DateTime startDate, DateTime endDate)> GetDateRange(DateTime startDate, DateTime endDate)
	{
		List<(DateTime, DateTime)> list = new List<(DateTime, DateTime)>();
		DateTime dateTime = startDate;
		while (dateTime <= endDate)
		{
			DateTime dateTime2 = dateTime.AddMonths(6);
			if (dateTime2 > endDate)
			{
				dateTime2 = endDate;
			}
			list.Add((dateTime, dateTime2));
			dateTime = dateTime2.AddDays(1.0);
		}
		return list;
	}

	public List<TaxDoc> GetList(ReceivedOrSubmittedType type, Status status, TaxDocType taxDocType = TaxDocType.None, DateTime? createdStartDate = null, DateTime? createdEndDate = null, DateTime? issuedStartDate = null, DateTime? issuedEndDate = null, string supplierTin = "", string supplierName = "", string buyerTin = "", string buyerName = "", string serialNumber = "", DateTime? supplyStartDate = null, DateTime? supplyEndDate = null, int pageOffset = 0, int pageLimit = 0, string sortCol = "createdAt", bool sortAsc = false, CorrectionState correctionState = CorrectionState.None, ApprovalState approvalState = ApprovalState.None, bool showOnlyCurrentUser = false, CancellationState cancellationState = CancellationState.None, List<short> invoiceTypes = null)
	{
		return AsyncHelper.RunSync(async () => await GetListAsync(type, status, taxDocType, createdStartDate, createdEndDate, issuedStartDate, issuedEndDate, supplierTin, supplierName, buyerTin, buyerName, serialNumber, supplyStartDate, supplyEndDate, pageOffset, pageLimit, sortCol, sortAsc, correctionState, approvalState, showOnlyCurrentUser, cancellationState, invoiceTypes));
	}

	public Task<List<TaxDoc>> GetDraftAccDocsAsync(string supplierTin = "", string supplierName = "", string buyerTin = "", string buyerName = "", DateTime? supplyStartDate = null, DateTime? supplyEndDate = null, bool showOnlyCurrentUser = false, DateTime? createdStartDate = null, DateTime? createdEndDate = null, List<short> accDocTypes = null, CancellationToken cancellationToken = default(CancellationToken))
	{
		return GetListAsync(ReceivedOrSubmittedType.Draft, Status.Draft, TaxDocType.AccDoc, createdStartDate, createdEndDate, null, null, supplierTin, supplierName, buyerTin, buyerName, "", supplyStartDate, supplyEndDate, 0, 0, "createdAt", sortAsc: false, CorrectionState.None, ApprovalState.None, showOnlyCurrentUser, CancellationState.None, accDocTypes, SupplierTaxRegime.None, cancellationToken);
	}

	public List<TaxDoc> GetDraftAccDocs(string supplierTin = "", string supplierName = "", string buyerTin = "", string buyerTinName = "", DateTime? supplyStartDate = null, DateTime? supplyEndDate = null, bool showOnlyCurrentUser = false, DateTime? createdStartDate = null, DateTime? createdEndDate = null, List<short> accDocTypes = null)
	{
		return AsyncHelper.RunSync(async () => await GetDraftAccDocsAsync(supplierTin, supplierName, buyerTin, buyerTinName, supplyStartDate, supplyEndDate, showOnlyCurrentUser, createdStartDate, createdEndDate, accDocTypes));
	}

	public Task<List<TaxDoc>> GetReceivedAccDocsAsync(DateTime? issuedStartDate = null, DateTime? issuedEndDate = null, string supplierTin = "", string supplierName = "", string serialNumber = "", DateTime? supplyStartDate = null, DateTime? supplyEndDate = null, CorrectionState correctionState = CorrectionState.None, ApprovalState approvalState = ApprovalState.None, int pageOffset = 0, int pageLimit = 0, string sortCol = "createdAt", bool sortAsc = false, DateTime? createdStartDate = null, DateTime? createdEndDate = null, List<short> accDocTypes = null, SupplierTaxRegime supplierTaxRegime = SupplierTaxRegime.None, CancellationToken cancellationToken = default(CancellationToken))
	{
		return GetListAsync(ReceivedOrSubmittedType.Received, Status.Approved, TaxDocType.AccDoc, createdStartDate, createdEndDate, issuedStartDate, issuedEndDate, supplierTin, supplierName, "", "", serialNumber, supplyStartDate, supplyEndDate, pageOffset, pageLimit, sortCol, sortAsc, correctionState, approvalState, showOnlyCurrentUser: false, CancellationState.None, accDocTypes, supplierTaxRegime, cancellationToken);
	}

	public List<TaxDoc> GetReceivedAccDocs(DateTime? issuedStartDate = null, DateTime? issuedEndDate = null, string supplierTin = "", string supplierName = "", string serialNumber = "", DateTime? supplyStartDate = null, DateTime? supplyEndDate = null, CorrectionState correctionState = CorrectionState.None, ApprovalState approvalState = ApprovalState.None, int pageOffset = 0, int pageLimit = 0, string sortCol = "createdAt", bool sortAsc = false, DateTime? createdStartDate = null, DateTime? createdEndDate = null, List<short> accDocTypes = null, SupplierTaxRegime supplierTaxRegime = SupplierTaxRegime.None)
	{
		return AsyncHelper.RunSync(async () => await GetReceivedAccDocsAsync(issuedStartDate, issuedEndDate, supplierTin, supplierName, serialNumber, supplyStartDate, supplyEndDate, correctionState, approvalState, pageOffset, pageLimit, sortCol, sortAsc, createdStartDate, createdEndDate, accDocTypes, supplierTaxRegime));
	}

	public Task<List<TaxDoc>> GetSubmittedAccDocsAsync(DateTime? issuedStartDate = null, DateTime? issuedEndDate = null, string supplierTin = "", string supplierName = "", string buyerTin = "", string buyerName = "", string serialNumber = "", DateTime? supplyStartDate = null, DateTime? supplyEndDate = null, CorrectionState correctionState = CorrectionState.None, ApprovalState approvalState = ApprovalState.None, int pageOffset = 0, int pageLimit = 0, string sortCol = "createdAt", bool sortAsc = false, bool showOnlyCurrentUser = false, DateTime? createdStartDate = null, DateTime? createdEndDate = null, List<short> accDocTypes = null, CancellationToken cancellationToken = default(CancellationToken))
	{
		return GetListAsync(ReceivedOrSubmittedType.Submitted, Status.Issued, TaxDocType.AccDoc, createdStartDate, createdEndDate, issuedStartDate, issuedEndDate, supplierTin, supplierName, buyerTin, buyerName, serialNumber, supplyStartDate, supplyEndDate, pageOffset, pageLimit, sortCol, sortAsc, correctionState, approvalState, showOnlyCurrentUser, CancellationState.None, accDocTypes, SupplierTaxRegime.None, cancellationToken);
	}

	public List<TaxDoc> GetSubmittedAccDocs(DateTime? issuedStartDate = null, DateTime? issuedEndDate = null, string supplierTin = "", string supplierName = "", string buyerTin = "", string buyerName = "", string serialNumber = "", DateTime? supplyStartDate = null, DateTime? supplyEndDate = null, CorrectionState correctionState = CorrectionState.None, ApprovalState approvalState = ApprovalState.None, int pageOffset = 0, int pageLimit = 0, string sortCol = "createdAt", bool sortAsc = false, bool showOnlyCurrentUser = false, DateTime? createdStartDate = null, DateTime? createdEndDate = null, List<short> accDocTypes = null)
	{
		return AsyncHelper.RunSync(async () => await GetSubmittedAccDocsAsync(issuedStartDate, issuedEndDate, supplierTin, supplierName, buyerTin, buyerName, serialNumber, supplyStartDate, supplyEndDate, correctionState, approvalState, pageOffset, pageLimit, sortCol, sortAsc, showOnlyCurrentUser, createdStartDate, createdEndDate, accDocTypes));
	}

	public Task<List<TaxDoc>> GetCancelledAccDocsAsync(DateTime? issuedStartDate = null, DateTime? issuedEndDate = null, DateTime? supplyStartDate = null, DateTime? supplyEndDate = null, string buyerTin = "", string buyerName = "", string serialNumber = "", CorrectionState correctionState = CorrectionState.None, CancellationState cancellationState = CancellationState.None, int pageOffset = 0, int pageLimit = 0, string sortCol = "createdAt", bool sortAsc = false, DateTime? createdStartDate = null, DateTime? createdEndDate = null, List<short> accDocTypes = null, CancellationToken cancellationToken = default(CancellationToken))
	{
		return GetListAsync(ReceivedOrSubmittedType.Cancelled, Status.Cancelled, TaxDocType.AccDoc, createdStartDate, createdEndDate, issuedStartDate, issuedEndDate, "", "", buyerTin, buyerName, serialNumber, supplyStartDate, supplyEndDate, pageOffset, pageLimit, sortCol, sortAsc, correctionState, ApprovalState.None, showOnlyCurrentUser: false, cancellationState, accDocTypes, SupplierTaxRegime.None, cancellationToken);
	}

	public List<TaxDoc> GetCancelledAccDocs(DateTime? issuedStartDate = null, DateTime? issuedEndDate = null, DateTime? supplyStartDate = null, DateTime? supplyEndDate = null, string buyerTin = "", string buyerName = "", string serialNumber = "", CorrectionState correctionState = CorrectionState.None, CancellationState cancellationState = CancellationState.None, int pageOffset = 0, int pageLimit = 0, string sortCol = "createdAt", bool sortAsc = false, DateTime? createdStartDate = null, DateTime? createdEndDate = null, List<short> accDocTypes = null)
	{
		return AsyncHelper.RunSync(async () => await GetCancelledAccDocsAsync(issuedStartDate, issuedEndDate, supplyStartDate, supplyEndDate, buyerTin, buyerName, serialNumber, correctionState, cancellationState, pageOffset, pageLimit, sortCol, sortAsc, createdStartDate, createdEndDate, accDocTypes));
	}

	public Task<List<TaxDoc>> GetDraftInvoicesAsync(string supplierTin = "", string supplierName = "", string buyerTin = "", string buyerName = "", DateTime? supplyStartDate = null, DateTime? supplyEndDate = null, bool showOnlyCurrentUser = false, DateTime? createdStartDate = null, DateTime? createdEndDate = null, List<short> invoiceTypes = null, CancellationToken cancellationToken = default(CancellationToken))
	{
		return GetListAsync(ReceivedOrSubmittedType.Draft, Status.Draft, TaxDocType.Invoice, createdStartDate, createdEndDate, null, null, supplierTin, supplierName, buyerTin, buyerName, "", supplyStartDate, supplyEndDate, 0, 0, "createdAt", sortAsc: false, CorrectionState.None, ApprovalState.None, showOnlyCurrentUser, CancellationState.None, invoiceTypes, SupplierTaxRegime.None, cancellationToken);
	}

	public List<TaxDoc> GetDraftInvoices(string supplierTin = "", string supplierName = "", string buyerTin = "", string buyerTinName = "", DateTime? supplyStartDate = null, DateTime? supplyEndDate = null, bool showOnlyCurrentUser = false, DateTime? createdStartDate = null, DateTime? createdEndDate = null, List<short> invoiceTypes = null)
	{
		return AsyncHelper.RunSync(async () => await GetDraftInvoicesAsync(supplierTin, supplierName, buyerTin, buyerTinName, supplyStartDate, supplyEndDate, showOnlyCurrentUser, createdStartDate, createdEndDate, invoiceTypes));
	}

	public Task<List<TaxDoc>> GetReceivedInvoicesAsync(DateTime? issuedStartDate = null, DateTime? issuedEndDate = null, string supplierTin = "", string supplierName = "", string serialNumber = "", DateTime? supplyStartDate = null, DateTime? supplyEndDate = null, CorrectionState correctionState = CorrectionState.None, ApprovalState approvalState = ApprovalState.None, int pageOffset = 0, int pageLimit = 0, string sortCol = "createdAt", bool sortAsc = false, DateTime? createdStartDate = null, DateTime? createdEndDate = null, List<short> invoiceTypes = null, SupplierTaxRegime supplierTaxRegime = SupplierTaxRegime.None, CancellationToken cancellationToken = default(CancellationToken))
	{
		return GetListAsync(ReceivedOrSubmittedType.Received, Status.Approved, TaxDocType.Invoice, createdStartDate, createdEndDate, issuedStartDate, issuedEndDate, supplierTin, supplierName, "", "", serialNumber, supplyStartDate, supplyEndDate, pageOffset, pageLimit, sortCol, sortAsc, correctionState, approvalState, showOnlyCurrentUser: false, CancellationState.None, invoiceTypes, supplierTaxRegime, cancellationToken);
	}

	public List<TaxDoc> GetReceivedInvoices(DateTime? issuedStartDate = null, DateTime? issuedEndDate = null, string supplierTin = "", string supplierName = "", string serialNumber = "", DateTime? supplyStartDate = null, DateTime? supplyEndDate = null, CorrectionState correctionState = CorrectionState.None, ApprovalState approvalState = ApprovalState.None, int pageOffset = 0, int pageLimit = 0, string sortCol = "createdAt", bool sortAsc = false, DateTime? createdStartDate = null, DateTime? createdEndDate = null, List<short> invoiceTypes = null, SupplierTaxRegime supplierTaxRegime = SupplierTaxRegime.None)
	{
		return AsyncHelper.RunSync(async () => await GetReceivedInvoicesAsync(issuedStartDate, issuedEndDate, supplierTin, supplierName, serialNumber, supplyStartDate, supplyEndDate, correctionState, approvalState, pageOffset, pageLimit, sortCol, sortAsc, createdStartDate, createdEndDate, invoiceTypes, supplierTaxRegime));
	}

	public Task<List<TaxDoc>> GetSubmittedInvoicesAsync(DateTime? issuedStartDate = null, DateTime? issuedEndDate = null, string supplierTin = "", string supplierName = "", string buyerTin = "", string buyerName = "", string serialNumber = "", DateTime? supplyStartDate = null, DateTime? supplyEndDate = null, CorrectionState correctionState = CorrectionState.None, ApprovalState approvalState = ApprovalState.None, int pageOffset = 0, int pageLimit = 0, string sortCol = "createdAt", bool sortAsc = false, bool showOnlyCurrentUser = false, DateTime? createdStartDate = null, DateTime? createdEndDate = null, List<short> invoiceTypes = null, CancellationToken cancellationToken = default(CancellationToken))
	{
		return GetListAsync(ReceivedOrSubmittedType.Submitted, Status.Issued, TaxDocType.Invoice, createdStartDate, createdEndDate, issuedStartDate, issuedEndDate, supplierTin, supplierName, buyerTin, buyerName, serialNumber, supplyStartDate, supplyEndDate, pageOffset, pageLimit, sortCol, sortAsc, correctionState, approvalState, showOnlyCurrentUser, CancellationState.None, invoiceTypes, SupplierTaxRegime.None, cancellationToken);
	}

	public List<TaxDoc> GetSubmittedInvoices(DateTime? issuedStartDate = null, DateTime? issuedEndDate = null, string supplierTin = "", string supplierName = "", string buyerTin = "", string buyerName = "", string serialNumber = "", DateTime? supplyStartDate = null, DateTime? supplyEndDate = null, CorrectionState correctionState = CorrectionState.None, ApprovalState approvalState = ApprovalState.None, int pageOffset = 0, int pageLimit = 0, string sortCol = "createdAt", bool sortAsc = false, bool showOnlyCurrentUser = false, DateTime? createdStartDate = null, DateTime? createdEndDate = null, List<short> invoiceTypes = null)
	{
		return AsyncHelper.RunSync(async () => await GetSubmittedInvoicesAsync(issuedStartDate, issuedEndDate, supplierTin, supplierName, buyerTin, buyerName, serialNumber, supplyStartDate, supplyEndDate, correctionState, approvalState, pageOffset, pageLimit, sortCol, sortAsc, showOnlyCurrentUser, createdStartDate, createdEndDate, invoiceTypes));
	}

	public Task<List<TaxDoc>> GetCancelledInvoicesAsync(DateTime? issuedStartDate = null, DateTime? issuedEndDate = null, DateTime? supplyStartDate = null, DateTime? supplyEndDate = null, string buyerTin = "", string buyerName = "", string serialNumber = "", CorrectionState correctionState = CorrectionState.None, CancellationState cancellationState = CancellationState.None, int pageOffset = 0, int pageLimit = 0, string sortCol = "createdAt", bool sortAsc = false, DateTime? createdStartDate = null, DateTime? createdEndDate = null, List<short> invoiceTypes = null, CancellationToken cancellationToken = default(CancellationToken))
	{
		return GetListAsync(ReceivedOrSubmittedType.Cancelled, Status.Cancelled, TaxDocType.Invoice, createdStartDate, createdEndDate, issuedStartDate, issuedEndDate, "", "", buyerTin, buyerName, serialNumber, supplyStartDate, supplyEndDate, pageOffset, pageLimit, sortCol, sortAsc, correctionState, ApprovalState.None, showOnlyCurrentUser: false, cancellationState, invoiceTypes, SupplierTaxRegime.None, cancellationToken);
	}

	public List<TaxDoc> GetCancelledInvoices(DateTime? issuedStartDate = null, DateTime? issuedEndDate = null, DateTime? supplyStartDate = null, DateTime? supplyEndDate = null, string buyerTin = "", string buyerName = "", string serialNumber = "", CorrectionState correctionState = CorrectionState.None, CancellationState cancellationState = CancellationState.None, int pageOffset = 0, int pageLimit = 0, string sortCol = "createdAt", bool sortAsc = false, DateTime? createdStartDate = null, DateTime? createdEndDate = null, List<short> invoiceTypes = null)
	{
		return AsyncHelper.RunSync(async () => await GetCancelledInvoicesAsync(issuedStartDate, issuedEndDate, supplyStartDate, supplyEndDate, buyerTin, buyerName, serialNumber, correctionState, cancellationState, pageOffset, pageLimit, sortCol, sortAsc, createdStartDate, createdEndDate, invoiceTypes));
	}

	private string GetCondition(ReceivedOrSubmittedType rcvSbmtype, TaxDocType taxDocType, DateTime? createdStartDate = null, DateTime? createdEndDate = null, DateTime? issuedStartDate = null, DateTime? issuedEndDate = null, string supplierTin = "", string supplierName = "", string buyerTin = "", string buyerName = "", string serialNumber = "", DateTime? supplyStartDate = null, DateTime? supplyEndDate = null, CorrectionState correctionState = CorrectionState.None, ApprovalState approvalState = ApprovalState.None, CancellationState cancellationState = CancellationState.None, bool showOnlyCurrentUser = true, List<short> invoiceTypes = null, SupplierTaxRegime supplierTaxRegime = SupplierTaxRegime.None)
	{
		string text = "";
		string text2 = "(";
		switch (rcvSbmtype)
		{
		case ReceivedOrSubmittedType.Received:
			text2 = text2 + text + "(#buyerTin = '" + client.Tin + "' and ((#status = '" + EncodeDecodeTypes.EncodeStatus(Status.Issued) + "') or (#status = '" + EncodeDecodeTypes.EncodeStatus(Status.Approved) + "')))";
			text = " and ";
			break;
		case ReceivedOrSubmittedType.Submitted:
			text2 = text2 + text + "((#supplierTin = '" + client.Tin + "' or #beneficiaryTin = '" + client.Tin + "') and ((#status = '" + EncodeDecodeTypes.EncodeStatus(Status.Issued) + "') or (#status = '" + EncodeDecodeTypes.EncodeStatus(Status.Approved) + "')))";
			text = " and ";
			if (showOnlyCurrentUser)
			{
				text2 = text2 + text + " ((#supplierUser = '" + client.Login + "') or (#supplierUser = null))";
				text = " and ";
			}
			break;
		case ReceivedOrSubmittedType.Cancelled:
			text2 = text2 + text + "((#supplierTin = '" + client.Tin + "' or #beneficiaryTin = '" + client.Tin + "' or #buyerTin = '" + client.Tin + "') and (((#status = '" + EncodeDecodeTypes.EncodeStatus(Status.WaitingInvalidationAcceptance) + "') or (#status = '" + EncodeDecodeTypes.EncodeStatus(Status.Cancelled) + "')) or ((#status = '" + EncodeDecodeTypes.EncodeStatus(Status.Approved) + "') and (#cancellationState = '" + EncodeDecodeTypes.EncodeCancellationState(CancellationState.Decline) + "'))))";
			text = " and ";
			break;
		case ReceivedOrSubmittedType.Draft:
			text2 = text2 + text + "((#supplierTin = '" + client.Tin + "') and (#status = '" + EncodeDecodeTypes.EncodeStatus(Status.Draft) + "'))";
			text = " and ";
			if (showOnlyCurrentUser)
			{
				text2 = text2 + text + "((#userName = '" + client.Login + "') or (#userName = null))";
				text = " and ";
			}
			break;
		}
		if (correctionState != CorrectionState.None)
		{
			text2 = text2 + text + "#correctionState = '" + EncodeDecodeTypes.EncodeCorrectionState(correctionState) + "'";
			text = " and ";
		}
		switch (approvalState)
		{
		case ApprovalState.Approved:
			text2 = text2 + text + "(#status = '" + EncodeDecodeTypes.EncodeStatus(Status.Approved) + "' and #approvalState = '" + EncodeDecodeTypes.EncodeApprovalState(ApprovalState.Approved) + "')";
			text = " and ";
			break;
		case ApprovalState.Pending:
			text2 = text2 + text + "(#status = '" + EncodeDecodeTypes.EncodeStatus(Status.Issued) + "' and #approvalState = '" + EncodeDecodeTypes.EncodeApprovalState(ApprovalState.Pending) + "')";
			text = " and ";
			break;
		}
		if (cancellationState == CancellationState.Pending)
		{
			text2 = text2 + text + "(#status = '" + EncodeDecodeTypes.EncodeStatus(Status.WaitingInvalidationAcceptance) + "')";
			text = " and ";
		}
		if (cancellationState == CancellationState.Accept)
		{
			text2 = text2 + text + "(#status = '" + EncodeDecodeTypes.EncodeStatus(Status.Cancelled) + "')";
			text = " and ";
		}
		if (cancellationState == CancellationState.Decline)
		{
			text2 = text2 + text + "((#status = '" + EncodeDecodeTypes.EncodeStatus(Status.Approved) + "') and (#cancellationState = '" + EncodeDecodeTypes.EncodeCancellationState(CancellationState.Decline) + "'))";
			text = " and ";
		}
		switch (taxDocType)
		{
		case TaxDocType.Invoice:
		{
			string text4;
			if (invoiceTypes == null)
			{
				text4 = $"'{EncodeDecodeTypes.EncodeInvoiceType(InvoiceType.Excise)}', '{EncodeDecodeTypes.EncodeInvoiceType(InvoiceType.Goods)}', '{EncodeDecodeTypes.EncodeInvoiceType(InvoiceType.Leasing)}', '{EncodeDecodeTypes.EncodeInvoiceType(InvoiceType.Services)}', '{EncodeDecodeTypes.EncodeInvoiceType(InvoiceType.VatReturn)}'";
			}
			else
			{
				List<string> list2 = new List<string>();
				foreach (short invoiceType in invoiceTypes)
				{
					list2.Add("'" + EncodeDecodeTypes.EncodeInvoiceType((InvoiceType)invoiceType) + "'");
				}
				text4 = string.Join(",", list2);
			}
			text2 = text2 + text + "(#type in [" + text4 + "])";
			text = " and ";
			break;
		}
		case TaxDocType.AccDoc:
		{
			string text3;
			if (invoiceTypes == null)
			{
				text3 = $"'{EncodeDecodeTypes.EncodeAccDocType(AccDocType.AccDocGoods)}', '{EncodeDecodeTypes.EncodeAccDocType(AccDocType.AccDocServices)}', '{EncodeDecodeTypes.EncodeAccDocType(AccDocType.AccDocTransportation)}', '{EncodeDecodeTypes.EncodeAccDocType(AccDocType.AccDocExportEEU)}', '{EncodeDecodeTypes.EncodeAccDocType(AccDocType.AccDocTraceableG)}'";
			}
			else
			{
				List<string> list = new List<string>();
				foreach (short invoiceType2 in invoiceTypes)
				{
					list.Add("'" + EncodeDecodeTypes.EncodeAccDocType((AccDocType)invoiceType2) + "'");
				}
				text3 = string.Join(",", list);
			}
			text2 = text2 + text + "(#type in [" + text3 + "])";
			text = " and ";
			break;
		}
		}
		if (createdStartDate.HasValue)
		{
			text2 = text2 + text + "(#createdAt >= date('" + createdStartDate.Value.Date.ToString("yyyy-MM-dd") + " 00:00:00'))";
			text = " and ";
		}
		if (createdEndDate.HasValue)
		{
			text2 = text2 + text + " (#createdAt <= date('" + createdEndDate.Value.Date.ToString("yyyy-MM-dd") + " 23:59:59'))";
			text = " and ";
		}
		if (issuedStartDate.HasValue)
		{
			text2 = text2 + text + "(#issuedAt >= date('" + issuedStartDate.Value.Date.ToString("yyyy-MM-dd") + " 00:00:00'))";
			text = " and ";
		}
		if (issuedEndDate.HasValue)
		{
			text2 = text2 + text + "(#issuedAt <= date('" + issuedEndDate.Value.Date.ToString("yyyy-MM-dd") + " 23:59:59'))";
			text = " and ";
		}
		if (supplyStartDate.HasValue)
		{
			text2 = text2 + text + "(#deliveredAt >= date('" + supplyStartDate.Value.Date.ToString("yyyy-MM-dd") + " 00:00:00'))";
			text = " and ";
		}
		if (supplyEndDate.HasValue)
		{
			text2 = text2 + text + "(#deliveredAt <= date('" + supplyEndDate.Value.Date.ToString("yyyy-MM-dd") + " 23:59:59'))";
			text = " and ";
		}
		if (!string.IsNullOrWhiteSpace(serialNumber))
		{
			text2 = text2 + text + "(#serialNo like '" + serialNumber + "%')";
			text = " and ";
		}
		if (!string.IsNullOrWhiteSpace(buyerTin))
		{
			text2 = text2 + text + "(#buyerTin like '" + buyerTin + "%')";
			text = " and ";
		}
		if (!string.IsNullOrWhiteSpace(buyerName))
		{
			text2 = text2 + text + "(#buyerName like '" + buyerName + "%')";
			text = " and ";
		}
		if (!string.IsNullOrWhiteSpace(supplierTin))
		{
			text2 = text2 + text + "(#supplierTin like '" + supplierTin + "%')";
			text = " and ";
		}
		if (!string.IsNullOrWhiteSpace(supplierName))
		{
			text2 = text2 + text + "(#supplierName like '" + supplierName + "%')";
			text = " and ";
		}
		return text2 + ")";
	}
}
