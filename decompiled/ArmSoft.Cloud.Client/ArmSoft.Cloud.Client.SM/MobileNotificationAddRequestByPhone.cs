namespace ArmSoft.Cloud.Client.SM;

public class MobileNotificationAddRequestByPhone
{
	public string Phone { get; set; } = "";

	public string Title { get; set; } = "";

	public string Text { get; set; } = "";

	public MobileNotificationChannel Channel { get; set; }
}
