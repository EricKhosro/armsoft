using System;
using System.Threading.Tasks;

namespace ArmSoft.SME.Core.UI.BackgroundWorks;

public interface IBackgroundTaskRunner
{
	event EventHandler<BackgroundTaskEventArgs> StatusChanged;

	void RunInBackground(Func<Task> backgroundTask);

	void RunInBackground(Action backgroundTask);
}
