using System;

namespace ArmSoft.SME.Common.Constants;

public class TimerInterval
{
	public static readonly TimeSpan OnlineLicense = new TimeSpan(0, 0, 10, 0);

	public static readonly TimeSpan Synchronization = new TimeSpan(0, 0, 5, 0);

	public static readonly TimeSpan OfflineDocsSynchronization = new TimeSpan(0, 0, 1, 0);

	public static readonly TimeSpan Statistics = new TimeSpan(0, 1, 0, 0);
}
