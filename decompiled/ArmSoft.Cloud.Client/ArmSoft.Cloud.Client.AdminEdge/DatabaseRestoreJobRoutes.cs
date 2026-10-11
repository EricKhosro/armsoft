using ArmSoft.Cloud.Client.Edge;

namespace ArmSoft.Cloud.Client.AdminEdge;

public class DatabaseRestoreJobRoutes : JobRoutes<DatabaseRestoreRequest, bool>
{
	public DatabaseRestoreJobRoutes(EdgeAdminClient client)
		: base((RESTClient)client, "api/AdminDatabaseRestoreJob")
	{
	}
}
