using System;

namespace ArmSoft.SME.Core.UI.BackgroundWorks;

public class NotificationsProcessorBase
{
	protected int _userId;

	public DateTime NotificationsCheckDate { get; set; }

	public DateTime? PreviousNotificationsCheckDate { get; set; }

	public bool IsCheckedServerDate { get; set; }

	public bool IsCheckedUpdateVersion { get; set; }

	public NotificationsProcessorBase()
	{
	}

	public NotificationsProcessorBase(int userId)
	{
		_userId = userId;
	}

	public virtual bool IsNotificationsEnabled()
	{
		return true;
	}
}
