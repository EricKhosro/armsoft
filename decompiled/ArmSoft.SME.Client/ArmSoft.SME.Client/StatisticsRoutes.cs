using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ArmSoft.SME.Common.Models.Statistics;

namespace ArmSoft.SME.Client;

public class StatisticsRoutes
{
	protected readonly ApiClient _client;

	protected string ControllerName => "Statistics";

	public StatisticsRoutes(ApiClient client)
	{
		_client = client;
	}

	public Task StoreCallCountAsync(List<CallCountStatisticStoreRequest> request, CancellationToken cancellationToken = default(CancellationToken))
	{
		StringBuilder stringBuilder = _client.CreateURLBuilder("/api/" + ControllerName + "/StoreCallCount");
		return _client.ExecuteAsync(HttpMethod.Post, stringBuilder.ToString(), cancellationToken, request);
	}

	public void StoreCallCount(List<CallCountStatisticStoreRequest> request)
	{
		AsyncHelper.RunSync(async () =>
		{
			await StoreCallCountAsync(request);
		});
	}

	public Task StoreCallDurationAsync(List<CallDurationStatisticStoreRequest> request, CancellationToken cancellationToken = default(CancellationToken))
	{
		StringBuilder stringBuilder = _client.CreateURLBuilder("/api/" + ControllerName + "/StoreCallDuration");
		return _client.ExecuteAsync(HttpMethod.Post, stringBuilder.ToString(), cancellationToken, request);
	}

	public void StoreCallDuration(List<CallDurationStatisticStoreRequest> request)
	{
		AsyncHelper.RunSync(async () =>
		{
			await StoreCallDurationAsync(request);
		});
	}

	public Task<List<CallCountStatistic>> GetCallCountsAsync(StatisticsGetRequest request, CancellationToken cancellationToken = default(CancellationToken))
	{
		StringBuilder stringBuilder = _client.CreateURLBuilder("/api/" + ControllerName + "/GetCallCounts");
		return _client.ExecuteAsync<List<CallCountStatistic>>(HttpMethod.Post, stringBuilder.ToString(), cancellationToken, request);
	}

	public List<CallCountStatistic> GetCallCounts(StatisticsGetRequest request)
	{
		return AsyncHelper.RunSync(async () => await GetCallCountsAsync(request));
	}

	public Task<List<CallDurationStatistic>> GetCallDurationsAsync(StatisticsGetRequest request, CancellationToken cancellationToken = default(CancellationToken))
	{
		StringBuilder stringBuilder = _client.CreateURLBuilder("/api/" + ControllerName + "/GetCallDurations");
		return _client.ExecuteAsync<List<CallDurationStatistic>>(HttpMethod.Post, stringBuilder.ToString(), cancellationToken, request);
	}

	public List<CallDurationStatistic> GetCallDurations(StatisticsGetRequest request)
	{
		return AsyncHelper.RunSync(async () => await GetCallDurationsAsync(request));
	}

	public Task DeleteCallCountsAsync(StatisticsDeleteRequest request, CancellationToken cancellationToken = default(CancellationToken))
	{
		StringBuilder stringBuilder = _client.CreateURLBuilder("/api/" + ControllerName + "/DeleteCallCounts");
		return _client.ExecuteAsync(HttpMethod.Delete, stringBuilder.ToString(), cancellationToken, request);
	}

	public void DeleteCallCounts(StatisticsDeleteRequest request)
	{
		AsyncHelper.RunSync(async () =>
		{
			await DeleteCallCountsAsync(request);
		});
	}

	public Task DeleteCallDurationsAsync(StatisticsDeleteRequest request, CancellationToken cancellationToken = default(CancellationToken))
	{
		StringBuilder stringBuilder = _client.CreateURLBuilder("/api/" + ControllerName + "/DeleteCallDurations");
		return _client.ExecuteAsync(HttpMethod.Delete, stringBuilder.ToString(), cancellationToken, request);
	}

	public void DeleteCallDurations(StatisticsDeleteRequest request)
	{
		AsyncHelper.RunSync(async () =>
		{
			await DeleteCallDurationsAsync(request);
		});
	}
}
