using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Threading;
using ArmSoft.SME.Client.Data;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Models.Data;
using ArmSoft.SME.Common.Models.Job;
using ArmSoft.SME.Core.UI.Helpers;

namespace ArmSoft.SME.Core.UI.Progress;

public class DataProviderProgressWindow<R, P> : MultilineProgressWindow where R : ExtendableRow, new() where P : class, new()
{
	private const int _dataProcessingCode = 300;

	private const int _extenderProcessingCode = 400;

	public DataProvider<R, P> DataProvider { get; }

	public string ProcessingDataPhaseName { get; set; }

	public P Parameters { get; set; }

	public HashSet<string> Columns { get; set; }

	public List<R> Rows { get; set; }

	protected override bool SupportSeparateDequeuePhase => false;

	public event ProcessDataProviderEventHandler<R> ProcessRows;

	public event PrepareProcessingDataPhaseEventHandler<R> PrepareProcessRowsPhase;

	public DataProviderProgressWindow(DataProvider<R, P> dataProvider, Window owner = null)
	{
		Owner = owner ?? Settings.HostingEnvironment;
		ProcessingDataPhaseName = ArmSoft.SME.Common.Resources.ProcessingData;
		DataProvider = dataProvider;
		dataProvider.PrimaryPhaseEventHandler += PrimaryPhaseEventHandler;
		dataProvider.ProgressEventHandler += ProgressEventHandler;
		dataProvider.ProcessPhaseEventHandler += ProcessPhaseEventHandler;
		BackgroundWorkerDoWork += BackgroundWorker_DoWork;
	}

	private void BackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
	{
		//IL_01d0: Unknown result type (might be due to invalid IL or missing references)
		if (Convert.ToBoolean(CallsRecordingHelper.Config?.RecordCallSteps))
		{
			CallsRecordingHelper.RecordCallStepInFile(DataProvider.GetType().Name + " data provider execute start");
		}
		DataProviderResult<R> dataProviderResult = DataProvider.LongExecute(Parameters, DataProvider.Settings, null, handleEvents: true, Columns);
		if (Convert.ToBoolean(CallsRecordingHelper.Config?.RecordCallSteps))
		{
			CallsRecordingHelper.RecordCallStepInFile(DataProvider.GetType().Name + " data provider execute finished");
		}
		if (Convert.ToBoolean(CallsRecordingHelper.Config?.RecordDataView))
		{
			CallsRecordingHelper.ExportDataProviderResult(DataProvider.GetType().Name, Parameters, dataProviderResult.Rows);
		}
		if (ProcessRows != null)
		{
			DPProgressPhase phase = PrepareProcessRowsPhase?.Invoke(this, new PrepareProcessingDataPhaseArgs<R>
			{
				Result = dataProviderResult
			}) ?? new DPProgressPhase
			{
				Name = ProcessingDataPhaseName,
				StartDate = DateTime.Now
			};
			ProcessDataProviderEventArgs<R> args = new ProcessDataProviderEventArgs<R>
			{
				Cancel = () =>
				{
					//IL_001c: Unknown result type (might be due to invalid IL or missing references)
					((DispatcherObject)this).Dispatcher.BeginInvoke((Delegate)(Action)(() =>
					{
						SetDefaultCancelButtonState();
					}), Array.Empty<object>()).Wait();
				},
				ProcessPhaseRow = (int row) =>
				{
					phase.Row = row;
					ProcessPhase(phase);
				},
				Result = dataProviderResult
			};
			ProcessPhase(phase);
			Thread.Sleep(100);
			((DispatcherObject)this).Dispatcher.BeginInvoke((Delegate)(Action)(() =>
			{
				ProcessRows?.Invoke(this, args);
			}), Array.Empty<object>()).Wait();
			phase.EndDate = DateTime.Now;
			ProcessPhase(phase);
		}
		DataProvider.CheckResult(dataProviderResult);
		Thread.Sleep(100);
	}

	public void PrimaryPhaseEventHandler(object sender, PrimaryPhaseEventArgs e)
	{
		List<string> list = e.Phases.Select((ExecutionPhase x) => x.Name).ToList();
		list.Add(ProcessingDataPhaseName);
		PreparePrimaryPhases(list);
	}

	public void ProgressEventHandler(object sender, DataProviderProgressEventArgs e)
	{
		Process(e.Progress);
		if (!Canceled)
		{
			return;
		}
		bool obj = true;
		int num;
		if (e.Progress?.Details != null)
		{
			num = ((e.Progress.Details.Count > 0) ? 1 : 0);
			if (num != 0)
			{
				obj = e.Progress.Details.Last().Id == 300;
			}
		}
		else
		{
			num = 0;
		}
		e.Cancel(obj);
		if (num == 0)
		{
			throw new ASException(string.Format(ArmSoft.SME.Common.Resources.DataViewProgressCanceled, DataProvider.Name));
		}
	}

	public void ProcessPhaseEventHandler(object sender, ProcessPhaseEventArgs e)
	{
		ProcessPhase(e.Phase);
		e.Canceled = Canceled;
	}

	protected override void SetDefaultCancelButtonState(ProgressPhaseBase phase = null)
	{
		if (phase != null && ((phase as DPProgressPhase).Id == 400 || phase.DisableCancel))
		{
			Canceled = false;
			AbortButton.IsEnabled = false;
			AbortButton.Name = ArmSoft.SME.Common.Resources.Abort;
		}
		else
		{
			base.SetDefaultCancelButtonState(phase);
		}
	}
}
