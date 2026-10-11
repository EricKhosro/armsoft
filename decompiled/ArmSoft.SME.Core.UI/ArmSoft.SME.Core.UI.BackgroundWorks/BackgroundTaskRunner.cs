using System;
using System.Threading.Tasks;
using Serilog;

namespace ArmSoft.SME.Core.UI.BackgroundWorks;

public class BackgroundTaskRunner : IBackgroundTaskRunner
{
	public event EventHandler<BackgroundTaskEventArgs> StatusChanged;

	public void RunInBackground(Func<Task> backgroundTask)
	{
		Task.Run(async () =>
		{
			try
			{
				OnStatusChanged(BackgroundTaskStatus.Running);
				await backgroundTask();
				OnStatusChanged(BackgroundTaskStatus.Completed);
			}
			catch (Exception ex)
			{
				OnStatusChanged(BackgroundTaskStatus.Failed, ex);
				Log.ForContext<BackgroundTaskRunner>().Error(ex, ex.Message);
			}
		});
	}

	public void RunInBackground(Action backgroundTask)
	{
		Task.Run(() =>
		{
			try
			{
				OnStatusChanged(BackgroundTaskStatus.Running);
				backgroundTask();
				OnStatusChanged(BackgroundTaskStatus.Completed);
			}
			catch (Exception ex)
			{
				OnStatusChanged(BackgroundTaskStatus.Failed, ex);
				Log.ForContext<BackgroundTaskRunner>().Error(ex, ex.Message);
			}
		});
	}

	public void RunInBackground(Action backgroundTask, Action continueWith)
	{
		Task.Run(() =>
		{
			try
			{
				OnStatusChanged(BackgroundTaskStatus.Running);
				backgroundTask();
			}
			catch (Exception ex)
			{
				OnStatusChanged(BackgroundTaskStatus.Failed, ex);
				Log.ForContext<BackgroundTaskRunner>().Error(ex, ex.Message);
			}
		}).ContinueWith((Task t) =>
		{
			try
			{
				OnStatusChanged(BackgroundTaskStatus.Running);
				continueWith();
				OnStatusChanged(BackgroundTaskStatus.Completed);
			}
			catch (Exception ex)
			{
				OnStatusChanged(BackgroundTaskStatus.Failed, ex);
				Log.ForContext<BackgroundTaskRunner>().Error(ex, ex.Message);
			}
		});
	}

	private void OnStatusChanged(BackgroundTaskStatus status, Exception ex = null)
	{
		StatusChanged?.Invoke(this, new BackgroundTaskEventArgs(status, ex));
	}
}
