using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using ArmSoft.Cloud.Client.Edge;

namespace ArmSoft.Cloud.Client;

public class EdgeClient : RESTClient, IDisposable
{
	private DatabaseBackupJobRoutes databaseBackupJob;

	private DatabaseRestoreJobRoutes databaseRestoreJob;

	private DatabaseCreateJobRoutes databaseCreateJob;

	public string UserToken { get; set; }

	public DatabaseBackupJobRoutes DatabaseBackupJob
	{
		get
		{
			if (databaseBackupJob == null)
			{
				databaseBackupJob = new DatabaseBackupJobRoutes(this);
			}
			return databaseBackupJob;
		}
	}

	public DatabaseRestoreJobRoutes DatabaseRestoreJob
	{
		get
		{
			if (databaseRestoreJob == null)
			{
				databaseRestoreJob = new DatabaseRestoreJobRoutes(this);
			}
			return databaseRestoreJob;
		}
	}

	public DatabaseCreateJobRoutes DatabaseCreateJob
	{
		get
		{
			if (databaseCreateJob == null)
			{
				databaseCreateJob = new DatabaseCreateJobRoutes(this);
			}
			return databaseCreateJob;
		}
	}

	public EdgeClient(string baseAddress, string token, string usertoken)
	{
		BaseAddress = baseAddress;
		Token = token;
		UserToken = usertoken;
	}

	protected override void OnCreateRequestHeader(HttpWebRequest request)
	{
		if (!string.IsNullOrEmpty(Token))
		{
			request.Headers.Add("apiToken", Token);
		}
		if (!string.IsNullOrEmpty(UserToken))
		{
			request.Headers.Add("UserToken", UserToken);
		}
	}

	public int GetAPIVersion()
	{
		StringBuilder stringBuilder = CreateURLBuilder("api/APIVersion");
		return Send<int>("GET", stringBuilder.ToString());
	}

	public string GetServiceVersion()
	{
		StringBuilder stringBuilder = CreateURLBuilder("api/APIVersion/Service");
		return Send<string>("GET", stringBuilder.ToString());
	}

	public List<BackupInfo> GetBackupsInfo(string extension, string fileName = "")
	{
		UrlQuery urlQuery = new UrlQuery();
		urlQuery.Add("fileName", fileName);
		StringBuilder stringBuilder = CreateURLBuilder("api/Backups/" + extension, urlQuery);
		return Send<List<BackupInfo>>("GET", stringBuilder.ToString());
	}

	public List<BackupInfoEx> GetBackupsInfoEx(int serverId, string fileName = "")
	{
		UrlQuery urlQuery = new UrlQuery();
		urlQuery.Add("serverId", serverId);
		urlQuery.Add("fileName", fileName);
		StringBuilder stringBuilder = CreateURLBuilder("api/Backups/ByApplication", urlQuery);
		return Send<List<BackupInfoEx>>("GET", stringBuilder.ToString());
	}

	public void DeleteFile(string fileName)
	{
		UrlQuery urlQuery = new UrlQuery();
		urlQuery.Add("fileName", fileName);
		StringBuilder stringBuilder = CreateURLBuilder("api/Files", urlQuery);
		Send("DELETE", stringBuilder.ToString());
	}

	public Guid InvokeCompression(CompressionRequest request)
	{
		StringBuilder stringBuilder = CreateURLBuilder("api/Files/Compress");
		return Send<Guid>("POST", stringBuilder.ToString(), request);
	}

	public DecompressionResponse InvokeDecompression(string fileName)
	{
		UrlQuery urlQuery = new UrlQuery();
		urlQuery.Add("fileName", fileName);
		StringBuilder stringBuilder = CreateURLBuilder("api/Files/Decompress", urlQuery);
		return Send<DecompressionResponse>("POST", stringBuilder.ToString());
	}

	public short GetProgress(Guid taskId)
	{
		UrlQuery urlQuery = new UrlQuery();
		urlQuery.Add("taskId", taskId);
		StringBuilder stringBuilder = CreateURLBuilder("api/Files/Progress", urlQuery);
		return Send<short>("GET", stringBuilder.ToString());
	}

	public void CreateSubDirectory()
	{
		StringBuilder stringBuilder = CreateURLBuilder("api/Files/SubPath");
		Send("POST", stringBuilder.ToString());
	}

	public long GetFileStreamLength(string fileName)
	{
		UrlQuery urlQuery = new UrlQuery();
		urlQuery.Add("fileName", fileName);
		StringBuilder stringBuilder = CreateURLBuilder("api/Files/StreamLength", urlQuery);
		return Send<long>("GET", stringBuilder.ToString());
	}

	public DownloadResponse Download(DownloadRequest request)
	{
		StringBuilder stringBuilder = CreateURLBuilder("api/Files/Download");
		return Send<DownloadResponse>("POST", stringBuilder.ToString(), request);
	}

	public void Upload(UploadRequest request)
	{
		StringBuilder stringBuilder = CreateURLBuilder("api/Files/Upload");
		Send("POST", stringBuilder.ToString(), request);
	}

	public List<DBSize> CalculateDBSizes(DBListSizeRequest request)
	{
		StringBuilder stringBuilder = CreateURLBuilder("api/DB/sizeinfo");
		return Send<List<DBSize>>("POST", stringBuilder.ToString(), request);
	}

	public void ChangeDBMode(string database, bool isSingleUser)
	{
		DatabaseModeRequest requestObject = new DatabaseModeRequest
		{
			Database = database,
			IsSingleUser = isSingleUser
		};
		StringBuilder stringBuilder = CreateURLBuilder("api/DB/Mode");
		Send("POST", stringBuilder.ToString(), requestObject);
	}

	public List<DBConnectionInfo> GetDBConnections(string dbName)
	{
		UrlQuery urlQuery = new UrlQuery();
		urlQuery.Add("dbName", dbName);
		StringBuilder stringBuilder = CreateURLBuilder("api/DB/Connection", urlQuery);
		return Send<List<DBConnectionInfo>>("GET", stringBuilder.ToString());
	}

	public int GetDBConnectionsCount(string dbName)
	{
		UrlQuery urlQuery = new UrlQuery();
		urlQuery.Add("dbName", dbName);
		StringBuilder stringBuilder = CreateURLBuilder("api/DB/ConnectionsCount", urlQuery);
		return Send<int>("GET", stringBuilder.ToString());
	}

	public void KillDBConnection(string dbName, int spId)
	{
		KillDBConnectionRequest requestObject = new KillDBConnectionRequest
		{
			Database = dbName,
			SPID = spId
		};
		StringBuilder stringBuilder = CreateURLBuilder("api/DB/Connection");
		Send("DELETE", stringBuilder.ToString(), requestObject);
	}

	public void DropDatabase(string dbName)
	{
		StringBuilder stringBuilder = CreateURLBuilder("api/DB/DropDatabase/" + dbName);
		Send("DELETE", stringBuilder.ToString());
	}

	protected virtual void Dispose(bool disposing)
	{
	}

	~EdgeClient()
	{
		Dispose(disposing: false);
	}

	public void Dispose()
	{
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}
}
