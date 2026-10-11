using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ArmSoft.SME.Common.Enums;
using ArmSoft.SME.Common.Models.DPR;
using ArmSoft.SME.Common.Models.Job;
using ArmSoft.SME.Common.Models.UIRequests;

namespace ArmSoft.SME.Client.DPR;

public class DataProcessingRequestRoutes
{
	private readonly ApiClient _client;

	internal DataProcessingRequestRoutes(ApiClient client)
	{
		_client = client;
	}

	public Task<IEnumerable<DPRInfo>> GetListAsync(CancellationToken cancellationToken = default(CancellationToken))
	{
		StringBuilder stringBuilder = _client.CreateURLBuilder("/api/DPR");
		return _client.ExecuteAsync<IEnumerable<DPRInfo>>(HttpMethod.Get, stringBuilder.ToString(), cancellationToken);
	}

	public IEnumerable<DPRInfo> GetList()
	{
		return AsyncHelper.RunSync(async () => await GetListAsync());
	}

	public Task<IEnumerable<DPRInfo>> GetListAsync(DPRType type, CancellationToken cancellationToken = default(CancellationToken))
	{
		StringBuilder stringBuilder = _client.CreateURLBuilder($"/api/DPR/{(short)type}");
		return _client.ExecuteAsync<IEnumerable<DPRInfo>>(HttpMethod.Get, stringBuilder.ToString(), cancellationToken);
	}

	public IEnumerable<DPRInfo> GetList(DPRType type)
	{
		return AsyncHelper.RunSync(async () => await GetListAsync(type));
	}

	public Task<DPRInfoExtended> GetObjectAsync(DPRType type, string name, CancellationToken cancellationToken = default(CancellationToken))
	{
		StringBuilder stringBuilder = _client.CreateURLBuilder($"/api/DPR/{(short)type}/{name}");
		return _client.ExecuteAsync<DPRInfoExtended>(HttpMethod.Get, stringBuilder.ToString(), cancellationToken);
	}

	public DPRInfoExtended GetObject(DPRType type, string name)
	{
		return AsyncHelper.RunSync(async () => await GetObjectAsync(type, name));
	}

	public Task<DPRJobEnqueueResponse> EnqueueAsync(DPRType type, string name, bool enableUIRequest, JobDPRRequest request, CancellationToken cancellationToken = default(CancellationToken))
	{
		UrlQuery urlQuery = new UrlQuery();
		urlQuery.Add("enableUIRequest", enableUIRequest);
		StringBuilder stringBuilder = _client.CreateURLBuilder($"/api/DPR/enqueueWithUIResponse/{(short)type}/{name}", urlQuery);
		JobDPRRequestInner requestObject = new JobDPRRequestInner
		{
			Request = request.Request,
			UIResponse = request.ToInnerRequest()
		};
		return _client.ExecuteAsync<DPRJobEnqueueResponse>(HttpMethod.Post, stringBuilder.ToString(), cancellationToken, requestObject);
	}

	public DPRJobEnqueueResponse Enqueue(DPRType type, string name, bool enableUIRequest, JobDPRRequest request)
	{
		return AsyncHelper.RunSync(async () => await EnqueueAsync(type, name, enableUIRequest, request));
	}

	public Task<T> GetResultAsync<T>(Guid id, bool delete, CancellationToken cancellationToken = default(CancellationToken))
	{
		StringBuilder stringBuilder = _client.CreateURLBuilder("/api/DPR/result");
		return _client.ExecuteAsync<T>(HttpMethod.Post, stringBuilder.ToString(), cancellationToken, new JobResultRequest
		{
			Id = id,
			Delete = delete
		});
	}

	public T GetResult<T>(Guid id, bool delete)
	{
		return AsyncHelper.RunSync(async () => await GetResultAsync<T>(id, delete));
	}

	public Task<DPRProgress> GetProgressAsync(Guid id, CancellationToken cancellationToken = default(CancellationToken))
	{
		StringBuilder stringBuilder = _client.CreateURLBuilder("/api/DPR/progress");
		return _client.ExecuteAsync<DPRProgress>(HttpMethod.Post, stringBuilder.ToString(), cancellationToken, new JobProgressRequest
		{
			Id = id
		});
	}

	public DPRProgress GetProgress(Guid id)
	{
		return AsyncHelper.RunSync(async () => await GetProgressAsync(id));
	}

	public Task StopAsync(Guid id, CancellationToken cancellationToken = default(CancellationToken))
	{
		StringBuilder stringBuilder = _client.CreateURLBuilder("/api/DPR/stop");
		return _client.ExecuteAsync(HttpMethod.Post, stringBuilder.ToString(), cancellationToken, new JobProgressRequest
		{
			Id = id
		});
	}

	public void Stop(Guid id)
	{
		AsyncHelper.RunSync(async () =>
		{
			await StopAsync(id);
		});
	}

	public Task UIRequestResultAsync(UIRequestResult request, CancellationToken cancellationToken = default(CancellationToken))
	{
		StringBuilder stringBuilder = _client.CreateURLBuilder("/api/DPR/uiRequestResult");
		return _client.ExecuteAsync(HttpMethod.Post, stringBuilder.ToString(), cancellationToken, request);
	}

	public void UIRequestResult(UIRequestResult request)
	{
		AsyncHelper.RunSync(async () =>
		{
			await UIRequestResultAsync(request);
		});
	}
}
