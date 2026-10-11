namespace ArmSoft.Cloud.Client.Edge;

public class DatabaseCreateJobRoutes : JobRoutes<DatabaseCreateRequest, bool>
{
	public DatabaseCreateJobRoutes(EdgeClient client)
		: base((RESTClient)client, "api/DatabaseCreateJob")
	{
	}
}
