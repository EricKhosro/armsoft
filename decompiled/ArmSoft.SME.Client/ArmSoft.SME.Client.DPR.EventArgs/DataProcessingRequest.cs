using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using ArmSoft.SME.Client.Job;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Enums;
using ArmSoft.SME.Common.Models.Job;
using ArmSoft.SME.Common.Models.UIRequests;

namespace ArmSoft.SME.Client.DPR.EventArgs;

public class DataProcessingRequest
{
	private bool isCanceled;

	private bool enqueued;

	private bool showError;

	private DPRType type;

	private string name;

	private bool enableUIRequest;

	private int retryAttempts;

	private int retryInterval;

	private string error;

	private bool restError;

	private DPRProgress progress;

	private DPRJobEnqueueResponse dprInfo;

	private readonly DataProcessingRequestRoutes dprRoutes;

	public string Name => name;

	public DPRType Type => type;

	public bool IsCanceled
	{
		get
		{
			if (IsCancelationSupported)
			{
				return isCanceled;
			}
			return false;
		}
		set
		{
			if (IsCancelationSupported)
			{
				isCanceled = value;
			}
		}
	}

	public bool IsFailed
	{
		get
		{
			CheckIsEnqueued();
			if (progress == null)
			{
				return false;
			}
			return progress.State == JobState.Failed;
		}
	}

	public bool IsCompleted
	{
		get
		{
			CheckIsEnqueued();
			if (progress == null)
			{
				return false;
			}
			if (progress.State != JobState.Failed)
			{
				return progress.State == JobState.Finished;
			}
			return true;
		}
	}

	public bool IsShowError
	{
		get
		{
			CheckIsEnqueued();
			return showError;
		}
	}

	public bool IsCancelationSupported
	{
		get
		{
			CheckIsEnqueued();
			return dprInfo.Info.IsCancellationSupported;
		}
	}

	public bool IsRestError
	{
		get
		{
			CheckIsEnqueued();
			return restError;
		}
	}

	public string ErrorText
	{
		get
		{
			CheckIsEnqueued();
			return error;
		}
	}

	public DPRJobEnqueueResponse Info => dprInfo;

	public ApiException Exception { get; private set; }

	public DataProcessingRequest(ApiClient apiClient)
	{
		dprRoutes = new DataProcessingRequestRoutes(apiClient);
	}

	public async Task EnqueueAsync<T>(DPRType type, string name, bool enableUIRequest, T request, int retryAttempts = 1, int retryInterval = 0, Dictionary<int, UIRequestResultBase> uiResponse = null, CancellationToken cancellationToken = default(CancellationToken))
	{
		this.type = type;
		this.name = name;
		this.enableUIRequest = enableUIRequest;
		JobDPRRequest request2 = new JobDPRRequest
		{
			Request = JsonSerializer.SerializeToElement(request, JsonSerializerFactory.JsonOptions),
			UIResponse = uiResponse
		};
		dprInfo = await dprRoutes.EnqueueAsync(this.type, this.name, this.enableUIRequest, request2, cancellationToken);
		enqueued = true;
		showError = true;
		this.retryAttempts = retryAttempts;
		this.retryInterval = retryInterval;
	}

	public void Enqueue<T>(DPRType type, string name, bool enableUIRequest, T request, int retryAttempts = 1, int retryInterval = 0, Dictionary<int, UIRequestResultBase> uiResponse = null)
	{
		AsyncHelper.RunSync(async () =>
		{
			await EnqueueAsync(type, name, enableUIRequest, request, retryAttempts, retryInterval, uiResponse);
		});
	}

	public async Task<T> GetResultAsync<T>(bool delete = true, CancellationToken cancellationToken = default(CancellationToken))
	{
		_ = 2;
		try
		{
			int iteration = 1;
			CheckIsEnqueued();
			do
			{
				await RefreshProgressAsync(cancellationToken);
				await Task.Delay(ProgressHelper.GetDelayTimeForProgress(ref iteration), cancellationToken);
			}
			while (!IsCompleted);
			return await dprRoutes.GetResultAsync<T>(dprInfo.Id, delete, cancellationToken);
		}
		catch (Exception ex)
		{
			if (IsFailed)
			{
				error = ex.Message;
				if (ex is ApiException ex2)
				{
					Exception = ex2;
					showError = ex2.Visible;
					restError = ex2.IsRestException;
				}
				return default;
			}
			throw;
		}
	}

	public T GetResult<T>(bool delete = true)
	{
		return AsyncHelper.RunSync(async () => await GetResultAsync<T>(delete));
	}

	public async Task<DPRProgress> RefreshProgressAsync(CancellationToken cancellationToken = default(CancellationToken))
	{
		int retryCount = retryAttempts;
		CheckIsEnqueued();
		while (retryCount > -1)
		{
			retryCount--;
			try
			{
				progress = await dprRoutes.GetProgressAsync(dprInfo.Id, cancellationToken);
			}
			catch (Exception ex)
			{
				if (ex != null)
				{
					if (retryCount == 0)
					{
						throw;
					}
					await Task.Delay(retryInterval, cancellationToken);
				}
			}
		}
		return progress;
	}

	public void RefreshProgress()
	{
		AsyncHelper.RunSync(async () => await RefreshProgressAsync());
	}

	public async Task StopExecutionAsync(CancellationToken cancellationToken = default(CancellationToken))
	{
		isCanceled = true;
		if (enqueued)
		{
			await dprRoutes.StopAsync(dprInfo.Id, cancellationToken);
		}
	}

	public void StopExecution()
	{
		AsyncHelper.RunSync(async () =>
		{
			await StopExecutionAsync();
		});
	}

	private void CheckIsEnqueued()
	{
		if (!enqueued)
		{
			throw new Exception(Resources.NotSentExecuting);
		}
	}
}
