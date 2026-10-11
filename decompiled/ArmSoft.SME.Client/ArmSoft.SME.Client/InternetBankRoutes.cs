using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ArmSoft.SME.Common.Models.Common;
using ArmSoft.SME.Common.Models.InternetBank;

namespace ArmSoft.SME.Client;

public class InternetBankRoutes
{
	protected readonly ApiClient _client;

	public InternetBankRoutes(ApiClient client)
	{
		_client = client;
	}

	public Task<List<ImportedIBTransactionsData>> GetImportedIBTransactionsDataAsync(DateOnly startDate, DateOnly endDate, List<string> accountNumbers, CancellationToken cancellationToken = default(CancellationToken))
	{
		ImportedIBPaymentDataGetRequest requestObject = new ImportedIBPaymentDataGetRequest
		{
			StartDate = startDate,
			EndDate = endDate,
			AccountNumbers = accountNumbers
		};
		StringBuilder stringBuilder = _client.CreateURLBuilder("/api/InternetBank/GetImportedIBTransactionsData");
		return _client.ExecuteAsync<List<ImportedIBTransactionsData>>(HttpMethod.Post, stringBuilder.ToString(), cancellationToken, requestObject);
	}

	public List<ImportedIBTransactionsData> GetImportedIBTransactionsData(DateOnly startDate, DateOnly endDate, List<string> accountNumbers)
	{
		return AsyncHelper.RunSync(async () => await GetImportedIBTransactionsDataAsync(startDate, endDate, accountNumbers));
	}

	public Task AddMappingAsync(IBTransactionMappingRequest request, CancellationToken cancellationToken = default(CancellationToken))
	{
		StringBuilder stringBuilder = _client.CreateURLBuilder("/api/InternetBank/AddMapping");
		return _client.ExecuteAsync(HttpMethod.Post, stringBuilder.ToString(), cancellationToken, request);
	}

	public void AddMapping(IBTransactionMappingRequest request)
	{
		AsyncHelper.RunSync(async () =>
		{
			await AddMappingAsync(request);
		});
	}

	public Task UpdateMatchedAsync(List<IdWithISNData> request, CancellationToken cancellationToken = default(CancellationToken))
	{
		StringBuilder stringBuilder = _client.CreateURLBuilder("/api/InternetBank/UpdateMatched");
		return _client.ExecuteAsync(HttpMethod.Post, stringBuilder.ToString(), cancellationToken, request);
	}

	public void UpdateMatched(List<IdWithISNData> request)
	{
		AsyncHelper.RunSync(async () =>
		{
			await UpdateMatchedAsync(request);
		});
	}

	public Task RemoveMappingAsync(IBTransactionMappingRequest request, CancellationToken cancellationToken = default(CancellationToken))
	{
		StringBuilder stringBuilder = _client.CreateURLBuilder("/api/InternetBank/RemoveMapping");
		return _client.ExecuteAsync(HttpMethod.Post, stringBuilder.ToString(), cancellationToken, request);
	}

	public void RemoveMapping(IBTransactionMappingRequest request)
	{
		AsyncHelper.RunSync(async () =>
		{
			await RemoveMappingAsync(request);
		});
	}

	public Task<IBTransactionMappingRequest> GetIBTransactionKeyAsync(Guid matchedDocISN, CancellationToken cancellationToken = default(CancellationToken))
	{
		StringBuilder stringBuilder = _client.CreateURLBuilder($"/api/InternetBank/GetIBTransactionKey/{matchedDocISN}");
		return _client.ExecuteAsync<IBTransactionMappingRequest>(HttpMethod.Get, stringBuilder.ToString(), cancellationToken);
	}

	public IBTransactionMappingRequest GetIBTransactionKey(Guid matchedDocISN)
	{
		return AsyncHelper.RunSync(async () => await GetIBTransactionKeyAsync(matchedDocISN));
	}

	public Task<List<ImportedTransactionsResponse>> ExistsImportedTransactionsAsync(DateOnly startDate, DateOnly endDate, List<string> accountNumbers, CancellationToken cancellationToken = default(CancellationToken))
	{
		ImportedIBPaymentDataGetRequest requestObject = new ImportedIBPaymentDataGetRequest
		{
			StartDate = startDate,
			EndDate = endDate,
			AccountNumbers = accountNumbers
		};
		StringBuilder stringBuilder = _client.CreateURLBuilder("/api/InternetBank/ExistsImportedTransactions");
		return _client.ExecuteAsync<List<ImportedTransactionsResponse>>(HttpMethod.Post, stringBuilder.ToString(), cancellationToken, requestObject);
	}

	public List<ImportedTransactionsResponse> ExistsImportedTransactions(DateOnly startDate, DateOnly endDate, List<string> accountNumbers)
	{
		return AsyncHelper.RunSync(async () => await ExistsImportedTransactionsAsync(startDate, endDate, accountNumbers));
	}
}
