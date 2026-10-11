namespace ArmSoft.Cloud.Client.Edge;

public class DatabaseBackupJobRoutes : JobRoutes<DatabaseBackupRequest, DatabaseBackupResult>
{
	public DatabaseBackupJobRoutes(EdgeClient client)
		: base((RESTClient)client, "api/DatabaseBackupJob")
	{
	}
}
