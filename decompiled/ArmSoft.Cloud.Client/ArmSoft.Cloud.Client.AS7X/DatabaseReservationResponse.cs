namespace ArmSoft.Cloud.Client.AS7X;

public class DatabaseReservationResponse
{
	public int DbId { get; set; }

	public string DbName { get; set; }

	public string Caption { get; set; }

	public int ServerId { get; set; }

	public string ServerName { get; set; }

	public string EdgeService { get; set; }

	public string SuggestedTemplate { get; set; }
}
