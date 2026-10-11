using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using ArmSoft.Cloud.Client.AdminEdge;
using ArmSoft.Cloud.Client.Edge;

namespace ArmSoft.Cloud.Client;

public class EdgeAdminClient : RESTClient, IDisposable
{
	private ArmSoft.Cloud.Client.AdminEdge.DatabaseBackupJobRoutes databaseBackupJob;

	private ArmSoft.Cloud.Client.AdminEdge.DatabaseRestoreJobRoutes databaseRestoreJob;

	private ArmSoft.Cloud.Client.AdminEdge.DatabaseCreateJobRoutes databaseCreateJob;

	public ArmSoft.Cloud.Client.AdminEdge.DatabaseBackupJobRoutes DatabaseBackupJob
	{
		get
		{
			if (databaseBackupJob == null)
			{
				databaseBackupJob = new ArmSoft.Cloud.Client.AdminEdge.DatabaseBackupJobRoutes(this);
			}
			return databaseBackupJob;
		}
	}

	public ArmSoft.Cloud.Client.AdminEdge.DatabaseRestoreJobRoutes DatabaseRestoreJob
	{
		get
		{
			if (databaseRestoreJob == null)
			{
				databaseRestoreJob = new ArmSoft.Cloud.Client.AdminEdge.DatabaseRestoreJobRoutes(this);
			}
			return databaseRestoreJob;
		}
	}

	public ArmSoft.Cloud.Client.AdminEdge.DatabaseCreateJobRoutes DatabaseCreateJob
	{
		get
		{
			if (databaseCreateJob == null)
			{
				databaseCreateJob = new ArmSoft.Cloud.Client.AdminEdge.DatabaseCreateJobRoutes(this);
			}
			return databaseCreateJob;
		}
	}

	public EdgeAdminClient(string baseAddress, string token)
	{
		BaseAddress = baseAddress;
		Token = token;
	}

	protected override void OnCreateRequestHeader(HttpWebRequest request)
	{
		if (!string.IsNullOrEmpty(Token))
		{
			request.Headers.Add("Authorization", "Bearer " + Token);
		}
	}

	public List<BackupInfo> GetBackupsInfo(string extension, string key = "", string fileName = "")
	{
		UrlQuery urlQuery = new UrlQuery();
		urlQuery.Add("fileName", fileName);
		urlQuery.Add("key", key);
		StringBuilder stringBuilder = CreateURLBuilder("api/Admin/Backups/" + extension, urlQuery);
		return Send<List<BackupInfo>>("GET", stringBuilder.ToString());
	}

	public List<BackupInfoEx> GetBackupsInfoEx(int serverId, string key = "", string fileName = "")
	{
		UrlQuery urlQuery = new UrlQuery();
		urlQuery.Add("serverId", serverId);
		urlQuery.Add("key", key);
		urlQuery.Add("fileName", fileName);
		StringBuilder stringBuilder = CreateURLBuilder("api/Admin/Backups/ByApplication", urlQuery);
		return Send<List<BackupInfoEx>>("GET", stringBuilder.ToString());
	}

	public List<DBSize> CalculateDBSizes(int serverId, TimeSpan? timeout = null)
	{
		StringBuilder stringBuilder = CreateURLBuilder($"api/DB/Server/{serverId}/sizeinfo");
		return Send<List<DBSize>>("GET", stringBuilder.ToString(), null, timeout);
	}

	public List<DBConnectionInfo> GetDBConnections(string dbName)
	{
		UrlQuery urlQuery = new UrlQuery();
		urlQuery.Add("dbName", dbName);
		StringBuilder stringBuilder = CreateURLBuilder("api/Admin/DB/Connection", urlQuery);
		return Send<List<DBConnectionInfo>>("GET", stringBuilder.ToString());
	}

	public void KillDBConnection(string dbName, int spId)
	{
		KillDBConnectionRequest requestObject = new KillDBConnectionRequest
		{
			Database = dbName,
			SPID = spId
		};
		StringBuilder stringBuilder = CreateURLBuilder("api/Admin/DB/Connection");
		Send("DELETE", stringBuilder.ToString(), requestObject);
	}

	public void DeleteFile(string key, string fileName)
	{
		UrlQuery urlQuery = new UrlQuery();
		urlQuery.Add("fileName", fileName);
		urlQuery.Add("key", key);
		StringBuilder stringBuilder = CreateURLBuilder("api/Admin/Files", urlQuery);
		Send("DELETE", stringBuilder.ToString());
	}

	public long GetFileStreamLength(string key, string fileName)
	{
		UrlQuery urlQuery = new UrlQuery();
		urlQuery.Add("fileName", fileName);
		urlQuery.Add("key", key);
		StringBuilder stringBuilder = CreateURLBuilder("api/Admin/Files/StreamLength", urlQuery);
		return Send<long>("GET", stringBuilder.ToString());
	}

	public DownloadResponse Download(string key, DownloadRequest request)
	{
		UrlQuery urlQuery = new UrlQuery();
		urlQuery.Add("key", key);
		StringBuilder stringBuilder = CreateURLBuilder("api/Admin/Files/Download", urlQuery);
		return Send<DownloadResponse>("POST", stringBuilder.ToString(), request);
	}

	public void Upload(string key, UploadRequest request)
	{
		UrlQuery urlQuery = new UrlQuery();
		urlQuery.Add("key", key);
		StringBuilder stringBuilder = CreateURLBuilder("api/Admin/Files/Upload", urlQuery);
		Send("POST", stringBuilder.ToString(), request);
	}

	public string DatabasePath()
	{
		StringBuilder stringBuilder = CreateURLBuilder("api/Admin/Files/database/path");
		return Send<string>("GET", stringBuilder.ToString());
	}

	public string BackupPath()
	{
		StringBuilder stringBuilder = CreateURLBuilder("api/Admin/Files/backup/path");
		return Send<string>("GET", stringBuilder.ToString());
	}

	public void CreateDatabaseDirectory(string dir)
	{
		StringBuilder stringBuilder = CreateURLBuilder("api/Admin/Files/database/createdir/" + dir);
		Send("POST", stringBuilder.ToString());
	}

	public void CreateBackupDirectory(string dir)
	{
		StringBuilder stringBuilder = CreateURLBuilder("api/Admin/Files/backup/createdir/" + dir);
		Send("POST", stringBuilder.ToString());
	}

	public Guid InvokeCompression(CompressionRequest request)
	{
		StringBuilder stringBuilder = CreateURLBuilder("api/Admin/Files/Compress");
		return Send<Guid>("POST", stringBuilder.ToString(), request);
	}

	public DecompressionResponse InvokeDecompression(string fileName, string key)
	{
		UrlQuery urlQuery = new UrlQuery();
		urlQuery.Add("fileName", fileName);
		urlQuery.Add("key", key);
		StringBuilder stringBuilder = CreateURLBuilder("api/Admin/Files/Decompress", urlQuery);
		return Send<DecompressionResponse>("POST", stringBuilder.ToString());
	}

	public short GetProgress(Guid taskId)
	{
		UrlQuery urlQuery = new UrlQuery();
		urlQuery.Add("taskId", taskId);
		StringBuilder stringBuilder = CreateURLBuilder("api/Admin/Files/Progress", urlQuery);
		return Send<short>("GET", stringBuilder.ToString());
	}

	protected virtual void Dispose(bool disposing)
	{
	}

	~EdgeAdminClient()
	{
		Dispose(disposing: false);
	}

	public void Dispose()
	{
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}
}
