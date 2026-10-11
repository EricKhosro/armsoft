using System;

namespace ArmSoft.InternetBank.Web.API.Common.Models;

public abstract class BaseNotificationForClient
{
	public LocalizableString Title { get; set; }

	public LocalizableString Text { get; set; }

	public byte Priority { get; set; }

	public DateTime? ExecutionDate { get; set; }

	public DateTime? ExpirationDate { get; set; }

	protected BaseNotificationForClient()
	{
		Priority = 2;
	}
}
