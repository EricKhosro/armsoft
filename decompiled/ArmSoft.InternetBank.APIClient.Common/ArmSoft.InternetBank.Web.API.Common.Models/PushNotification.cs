namespace ArmSoft.InternetBank.Web.API.Common.Models;

public class PushNotification
{
	public int UserId { get; set; }

	public string DeviceId { get; set; }

	public string Text { get; set; }

	public byte Priority { get; set; }

	public PushNotification()
	{
		Priority = 7;
	}
}
