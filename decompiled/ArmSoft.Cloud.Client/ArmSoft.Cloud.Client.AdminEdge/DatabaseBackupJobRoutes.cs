using ArmSoft.Cloud.Client.Edge;

namespace ArmSoft.Cloud.Client.AdminEdge;

public class DatabaseBackupJobRoutes : JobRoutes<DatabaseBackupRequest, DatabaseBackupResult>
{
	public DatabaseBackupJobRoutes(EdgeAdminClient client)
		: base((RESTClient)client, "api/AdminDatabaseBackupJob")
	{
	}
}
