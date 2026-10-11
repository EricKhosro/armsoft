using System;

namespace ArmSoft.SME.Common.Constants;

public class TimerStart
{
	public static readonly TimeSpan OnlineLicense = new TimeSpan(0, 0, 3, 0);

	public static readonly TimeSpan Notifications = new TimeSpan(0, 0, 0, 20);

	public static readonly TimeSpan Synchronization = new TimeSpan(0, 0, 0, 40);

	public static readonly TimeSpan OfflineDocsSynchronization = new TimeSpan(0, 0, 2, 0);

	public static readonly TimeSpan Statistics = new TimeSpan(0, 0, 10, 0);
}
