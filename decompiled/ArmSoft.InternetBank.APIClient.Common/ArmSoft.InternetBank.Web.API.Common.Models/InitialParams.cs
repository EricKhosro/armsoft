namespace ArmSoft.InternetBank.Web.API.Common.Models;

public class InitialParams
{
	public bool AllowPasswordResetByUser { get; set; }

	public bool PushNotificationsEnabledAndroid { get; set; }

	public bool PushNotificationsEnabledIos { get; set; }

	public int OnlineTransactionTimeout { get; set; }
}
