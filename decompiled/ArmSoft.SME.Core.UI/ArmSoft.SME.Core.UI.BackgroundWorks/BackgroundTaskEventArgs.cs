using System;

namespace ArmSoft.SME.Core.UI.BackgroundWorks;

public record BackgroundTaskEventArgs(BackgroundTaskStatus TaskStatus = BackgroundTaskStatus.None, Exception Error = null)
{
	public BackgroundTaskStatus TaskStatus { get; } = TaskStatus;

	public Exception Error { get; } = Error;
}
