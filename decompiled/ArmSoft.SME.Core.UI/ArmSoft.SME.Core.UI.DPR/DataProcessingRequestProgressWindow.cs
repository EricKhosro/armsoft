using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text.Json;
using System.Threading;
using System.Windows;
using System.Windows.Threading;
using ArmSoft.SME.Client;
using ArmSoft.SME.Client.DPR;
using ArmSoft.SME.Client.DPR.EventArgs;
using ArmSoft.SME.Client.UIRequest;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Enums;
using ArmSoft.SME.Common.Models.Job;
using ArmSoft.SME.Common.Models.UIRequests;
using ArmSoft.SME.Core.UI.Progress;

namespace ArmSoft.SME.Core.UI.DPR;

internal class DataProcessingRequestProgressWindow<T, R> : MultilineProgressWindow
{
	private readonly T _request;

	private readonly string _name;

	private readonly DPRType _type;

	private readonly int _retryAttempts;

	private readonly int _retryInterval;

	private readonly Dictionary<int, UIRequestResultBase> _uiResponse;

	private readonly Dictionary<int, Func<JsonElement, JsonElement>> _customUIRequestsConfig;

	public R Response { get; private set; }

	private DataProcessingRequestHandler DataProcessingRequestHandler { get; set; }

	private IDataProcessingRequestProcessResultPhase<R> ProcessResultPhase { get; }

	public DataProcessingRequestProgressWindow(DPRType type, string name, T request, int retryAttempts, int retryInterval, Dictionary<int, UIRequestResultBase> uiResponse = null, Dictionary<int, Func<JsonElement, JsonElement>> customUIRequestsConfig = null, IDataProcessingRequestProcessResultPhase<R> processResultPhase = null, string title = null, bool hideAbortButton = false, Window owner = null, ApiClient apiClient = null)
	{
		if (apiClient == null)
		{
			apiClient = Settings.CreateApiClient();
		}
		DataProcessingRequestHandler = new DataProcessingRequestHandler(apiClient);
		if (!string.IsNullOrWhiteSpace(title))
		{
			Title = title;
		}
		if (hideAbortButton)
		{
			AbortButton.Visibility = Visibility.Collapsed;
		}
		_type = type;
		_name = name;
		_request = request;
		_retryAttempts = retryAttempts;
		_retryInterval = retryInterval;
		_uiResponse = uiResponse;
		_customUIRequestsConfig = customUIRequestsConfig;
		ProcessResultPhase = processResultPhase;
		DataProcessingRequestHandler.ProgressEventHandler += ProgressEventHandler;
		DataProcessingRequestHandler.UIRequestEventHandler += UIRequestEventHandler;
		Owner = owner ?? Settings.HostingEnvironment;
		BackgroundWorkerDoWork += BackgroundWorker_DoWork;
	}

	private void BackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
	{
		Response = DataProcessingRequestHandler.Execute<T, R>(_type, _name, _request, enableUIRequest: true, _retryAttempts, _retryInterval, handleEvents: true, _uiResponse).Item2;
		if (ProcessResultPhase != null)
		{
			DPRProgressPhase progress = new DPRProgressPhase
			{
				Name = ProcessResultPhase.Caption,
				StartDate = DateTime.Now,
				Total = -1,
				Row = -1,
				DisableCancel = ProcessResultPhase.DisableCancel
			};
			ProcessResultPhase.Process(Response, () =>
			{
				progress.Total = ProcessResultPhase.Total;
				progress.Row = ProcessResultPhase.Row;
				ProcessPhase(progress);
				return Canceled;
			}, ((DispatcherObject)this).Dispatcher);
			Thread.Sleep(100);
			progress.EndDate = DateTime.Now;
			ProcessPhase(progress);
			Thread.Sleep(100);
		}
	}

	public void ProgressEventHandler(object sender, DataProcessingRequestProgressEventArgs e)
	{
		Process(e.Progress);
		if (Canceled)
		{
			e.Cancel();
			if (e.Progress?.Details == null)
			{
				throw new ASException(ArmSoft.SME.Common.Resources.AbortQuery);
			}
		}
	}

	public void UIRequestEventHandler(object sender, UIRequestEventArgs e)
	{
		e.UIRequestResult = ShowUIRequestAndGetResult(e.UIRequestData, e.Guid, _customUIRequestsConfig);
	}
}
