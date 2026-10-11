using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ArmSoft.SME.Client.DPR.EventArgs;
using ArmSoft.SME.Client.Job;
using ArmSoft.SME.Client.UIRequest;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Enums;
using ArmSoft.SME.Common.Exceptions;
using ArmSoft.SME.Common.Models.Job;
using ArmSoft.SME.Common.Models.UIRequests;

namespace ArmSoft.SME.Client.DPR;

public class DataProcessingRequestHandler
{
	public ApiClient apiClient;

	public event UIRequestEventHandler UIRequestEventHandler;

	public event ProgressEventHandler ProgressEventHandler;

	public DataProcessingRequestHandler(ApiClient apiClient)
	{
		this.apiClient = apiClient;
	}

	public async Task<(bool, R)> ExecuteAsync<T, R>(DPRType type, string name, T request, bool enableUIRequest = true, int retryAttempts = 1, int retryInterval = 0, bool handleEvents = false, Dictionary<int, UIRequestResultBase> uiResponse = null, CancellationToken cancellationToken = default(CancellationToken))
	{
		int iteration = 1;
		DataProcessingRequest dpr = new DataProcessingRequest(apiClient);
		await dpr.EnqueueAsync(type, name, enableUIRequest, request, retryAttempts, retryInterval, uiResponse, cancellationToken);
		if (handleEvents)
		{
			DataProcessingRequestProgressEventArgs progressEventArgs = new DataProcessingRequestProgressEventArgs
			{
				Cancel = dpr.StopExecution
			};
			UIRequestEventArgs uiRequestArgs = new UIRequestEventArgs
			{
				Guid = dpr.Info.Id
			};
			do
			{
				await Task.Delay(ProgressHelper.GetDelayTimeForProgress(ref iteration), cancellationToken);
				DPRProgress dPRProgress = await dpr.RefreshProgressAsync(cancellationToken);
				if (!dpr.IsCanceled && dPRProgress?.UIRequestData != null)
				{
					uiRequestArgs.UIRequestData = dPRProgress.UIRequestData;
					UIRequestEventHandler(this, uiRequestArgs);
					apiClient.DPR.UIRequestResult(uiRequestArgs.UIRequestResult);
				}
				progressEventArgs.Progress = dPRProgress;
				ProgressEventHandler(this, progressEventArgs);
			}
			while (!dpr.IsCompleted);
			UIRequestEventHandler = null;
			ProgressEventHandler = null;
		}
		R item = await dpr.GetResultAsync<R>(delete: true, cancellationToken);
		if (dpr.IsFailed)
		{
			if (dpr.IsShowError)
			{
				if (!dpr.IsRestError)
				{
					dpr.Exception.InnerMessage = string.Format(Resources.DprError, dpr.Name, GetDprTypeName(dpr.Type)) + "\n" + dpr.ErrorText;
				}
				throw dpr.Exception;
			}
			throw new InvisibleException();
		}
		return (!dpr.IsCanceled, item);
	}

	public (bool, R) Execute<T, R>(DPRType type, string name, T request, bool enableUIRequest = true, int retryAttempts = 1, int retryInterval = 0, bool handleEvents = false, Dictionary<int, UIRequestResultBase> uiResponse = null)
	{
		return AsyncHelper.RunSync(async () => await ExecuteAsync<T, R>(type, name, request, enableUIRequest, retryAttempts, retryInterval, handleEvents, uiResponse));
	}

	private string GetDprTypeName(DPRType type)
	{
		if (type == DPRType.Report)
		{
			return Resources.ReportCaption;
		}
		return string.Empty;
	}
}
