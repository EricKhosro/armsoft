namespace ArmSoft.Cloud.Client.Edge;

public class DatabaseRestoreJobRoutes : JobRoutes<DatabaseRestoreRequest, bool>
{
	public DatabaseRestoreJobRoutes(EdgeClient client)
		: base((RESTClient)client, "api/DatabaseRestoreJob")
	{
	}
}
