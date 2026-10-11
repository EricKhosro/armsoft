namespace ArmSoft.Cloud.Client.AS7X;

public class DatabaseReservationRequest
{
	public string Key { get; set; }

	public string DbName { get; set; }

	public string Caption { get; set; }

	public bool PSMode { get; set; }

	public string AppService { get; set; }
}
