using ArmSoft.Cloud.Client.Edge;

namespace ArmSoft.Cloud.Client.AdminEdge;

public class DatabaseCreateJobRoutes : JobRoutes<DatabaseCreateRequest, bool>
{
	public DatabaseCreateJobRoutes(EdgeAdminClient client)
		: base((RESTClient)client, "api/AdminDatabaseBackupJob")
	{
	}
}
