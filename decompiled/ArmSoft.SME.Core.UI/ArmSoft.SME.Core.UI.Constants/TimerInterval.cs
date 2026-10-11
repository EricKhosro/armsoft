using System;

namespace ArmSoft.SME.Core.UI.Constants;

public class TimerInterval
{
	public static readonly TimeSpan Notifications = new TimeSpan(0, 0, 0, 180);

	public static readonly TimeSpan B2BSynchronization = new TimeSpan(0, 0, 15, 0);

	public static readonly TimeSpan OnlineLicense = new TimeSpan(0, 0, 10, 0);

	public static readonly TimeSpan Statistics = new TimeSpan(30, 0, 0, 0);

	public static readonly TimeSpan ChangeCurrentDate = new TimeSpan(1, 0, 0, 0);
}
