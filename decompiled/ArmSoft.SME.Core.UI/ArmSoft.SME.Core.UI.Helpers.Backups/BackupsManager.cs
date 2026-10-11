using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ArmSoft.Cloud.Client;
using ArmSoft.Cloud.Client.AS7X;
using ArmSoft.Cloud.Client.Edge;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Extensions;
using ArmSoft.SME.Common.Models.Cloud;

namespace ArmSoft.SME.Core.UI.Helpers.Backups;

public class BackupsManager
{
	private readonly IProgress<ProgressIndicationValue> _progressIndicator;

	private readonly BackupRestoreRequest _request;

	private Guid _jobId;

	public BackupsManager(BackupRestoreRequest request, IProgress<ProgressIndicationValue> progressIndicator = null)
	{
		_progressIndicator = progressIndicator;
		_request = request;
	}

	public async Task CreateBackupAsync()
	{
		try
		{
			await CreateCloudBackupAsync();
		}
		catch
		{
			throw;
		}
	}

	public async Task CreateCloudBackupAsync()
	{
		await CreateCloudBackupFile();
		int databaseId = GetCloudDatabaseId();
		if (!_request.DoNotArchiveBackup)
		{
			await CreateCloudBackupArchiveAsync(databaseId);
		}
		UpdateCloudDbLastBackupDate(databaseId);
	}

	private async Task CreateCloudBackupFile()
	{
		JobEnqueueResponse jobEnqueueResponse;
		if (ConfigHelper.ASAdminLogin)
		{
			using EdgeAdminClient edgeAdminClient = new EdgeAdminClient(Settings.LoginService.EdgeServiceAddress, ADHelper.ASAdminB2CToken);
			jobEnqueueResponse = edgeAdminClient.DatabaseBackupJob.Enqueue(new DatabaseBackupRequest
			{
				Database = _request.DatabaseName,
				Filename = _request.FileName,
				IsAutoBackup = _request.IsAutoBackup,
				Comment = _request.Comment
			});
		}
		else
		{
			using EdgeClient edgeClient = new EdgeClient(Settings.LoginService.EdgeServiceAddress, Settings.GetApplicationToken(), ADHelper.ADB2CToken);
			jobEnqueueResponse = edgeClient.DatabaseBackupJob.Enqueue(new DatabaseBackupRequest
			{
				Database = _request.DatabaseName,
				Filename = _request.FileName,
				IsAutoBackup = _request.IsAutoBackup,
				Comment = _request.Comment
			});
		}
		_jobId = jobEnqueueResponse.Id;
		await HandleCloudInfoMessage();
	}

	public void StopBackup()
	{
		if (ConfigHelper.ASAdminLogin)
		{
			using (EdgeAdminClient edgeAdminClient = new EdgeAdminClient(Settings.LoginService.EdgeServiceAddress, ADHelper.ASAdminB2CToken))
			{
				edgeAdminClient.DatabaseBackupJob.Stop(_jobId);
				return;
			}
		}
		using EdgeClient edgeClient = new EdgeClient(Settings.LoginService.EdgeServiceAddress, Settings.GetApplicationToken(), ADHelper.ADB2CToken);
		edgeClient.DatabaseBackupJob.Stop(_jobId);
	}

	private int GetCloudDatabaseId()
	{
		return DatabaseHelper.GetCloudDatabases().FirstOrDefault((DatabaseInfo d) => d.DbName == _request.DatabaseName)?.DbId ?? 0;
	}

	public async Task CreateCloudBackupArchiveAsync(int databaseId)
	{
		if (ConfigHelper.ASAdminLogin)
		{
			await CreateCloudBackupArchiveForASAdminAsync(databaseId);
		}
		else
		{
			await CreateCloudBackupArchiveForUserAsync(databaseId);
		}
	}

	private async Task CreateCloudBackupArchiveForASAdminAsync(int databaseId)
	{
		EdgeAdminClient edgeClient = new EdgeAdminClient(Settings.LoginService.EdgeServiceAddress, ADHelper.ASAdminB2CToken);
		try
		{
			Guid taskId = edgeClient.InvokeCompression(new CompressionRequest
			{
				DbId = databaseId,
				FileName = _request.FileName,
				Extension = Settings.BackupExtension,
				Comment = _request.Comment
			});
			await Task.Run(() =>
			{
				short num = 0;
				while (num < 100)
				{
					num = edgeClient.GetProgress(taskId);
					switch (num)
					{
					case -1:
						throw new ASException(Resources.FileDoesNotExistWithoutName);
					case -2:
						throw new ASException(Resources.RestoreError);
					default:
						if (_progressIndicator != null)
						{
							ProgressIndicationValue value = new ProgressIndicationValue
							{
								ProgressStep = ProgressStep.ZIPBackupFile,
								UIMode = true,
								Percent = num
							};
							_progressIndicator.Report(value);
						}
						break;
					}
				}
			});
		}
		finally
		{
			if (edgeClient != null)
			{
				((IDisposable)edgeClient).Dispose();
			}
		}
	}

	private async Task CreateCloudBackupArchiveForUserAsync(int databaseId)
	{
		EdgeClient edgeClient = new EdgeClient(Settings.LoginService.EdgeServiceAddress, Settings.GetApplicationToken(), ADHelper.ADB2CToken);
		try
		{
			Guid taskId = edgeClient.InvokeCompression(new CompressionRequest
			{
				DbId = databaseId,
				FileName = _request.FileName,
				Extension = Settings.BackupExtension,
				Comment = _request.Comment
			});
			await Task.Run(() =>
			{
				short num = 0;
				while (num < 100)
				{
					num = edgeClient.GetProgress(taskId);
					switch (num)
					{
					case -1:
						throw new ASException(Resources.FileDoesNotExistWithoutName);
					case -2:
						throw new ASException(Resources.RestoreError);
					}
				}
			});
		}
		finally
		{
			if (edgeClient != null)
			{
				((IDisposable)edgeClient).Dispose();
			}
		}
	}

	private static void UpdateCloudDbLastBackupDate(int databaseId)
	{
		if (ConfigHelper.ASAdminLogin)
		{
			Settings.CreateApiClient().AdminCloud.CloudDatabaseBackUpCreated(ADHelper.ASAdminB2CToken, databaseId);
			return;
		}
		using CloudClient cloudClient = Settings.CloudClient;
		cloudClient.Token = Settings.GetApplicationToken();
		cloudClient.AS7X.CloudDatabaseBackUpCreated(databaseId);
	}

	public async Task RestoreBackupAsync()
	{
		if (ConfigHelper.ASAdminLogin)
		{
			await RestoreBackupCloudForASAdminAsync();
		}
		else
		{
			await RestoreBackupCloudForUserAsync();
		}
	}

	private async Task RestoreBackupCloudForASAdminAsync()
	{
		string fileName = string.Empty;
		Guid taskId = Guid.Empty;
		EdgeAdminClient edgeClient = new EdgeAdminClient(Settings.LoginService.EdgeServiceAddress, ADHelper.ASAdminB2CToken);
		try
		{
			_ = 1;
			try
			{
				List<ArmSoft.Cloud.Client.Edge.DBConnectionInfo> dBConnections = edgeClient.GetDBConnections(_request.DatabaseName);
				if ((from p in dBConnections
					group p by p.HostName).Count() > 1)
				{
					throw new ASException(Resources.ErrRestoringMustExclusive);
				}
				Settings.CreateApiClient().AppManager.ChangeDbState(operational: false, Resources.DatabaseRestoring);
				foreach (ArmSoft.Cloud.Client.Edge.DBConnectionInfo item in dBConnections)
				{
					edgeClient.KillDBConnection(_request.DatabaseName, item.SPID);
				}
				if (!_request.DoNotArchiveBackup)
				{
					DecompressionResponse decompressionResponse = edgeClient.InvokeDecompression(_request.FileName, Settings.LoginService.Key);
					taskId = decompressionResponse.TaskId;
					fileName = decompressionResponse.EntryName;
					await Task.Run(() =>
					{
						short num = 0;
						while (num < 100)
						{
							num = edgeClient.GetProgress(taskId);
							switch (num)
							{
							case -1:
								throw new ASException(Resources.FileDoesNotExistWithoutName);
							case -2:
								throw new ASException(Resources.RestoreError);
							default:
								if (_progressIndicator != null)
								{
									ProgressIndicationValue value = new ProgressIndicationValue
									{
										ProgressStep = ProgressStep.ExtractBackupFile,
										UIMode = true,
										Percent = num
									};
									_progressIndicator.Report(value);
								}
								break;
							}
						}
					});
				}
				else
				{
					fileName = _request.FileName;
				}
				await RestoreCloudDatabaseFromBackupAsync(fileName);
				Settings.CreateApiClient().AppManager.ChangeDbState(operational: true, Resources.DatabaseRestoring);
				Settings.CreateApiClient().AdminCloud.AddCloudDatabaseRestoreLog(ADHelper.ASAdminB2CToken, GetCloudDatabaseId());
				Settings.LoginService.CacheStructChanged = true;
			}
			catch
			{
				Settings.CreateApiClient().AppManager.ChangeDbState(operational: true, Resources.DatabaseRestoring);
				throw;
			}
			finally
			{
				if (!string.IsNullOrWhiteSpace(fileName))
				{
					edgeClient.DeleteFile(Settings.LoginService.Key, fileName);
				}
			}
		}
		finally
		{
			if (edgeClient != null)
			{
				((IDisposable)edgeClient).Dispose();
			}
		}
	}

	private async Task RestoreBackupCloudForUserAsync()
	{
		string fileName = "";
		Guid taskId = Guid.Empty;
		EdgeClient edgeClient = new EdgeClient(Settings.LoginService.EdgeServiceAddress, Settings.GetApplicationToken(), ADHelper.ADB2CToken);
		try
		{
			_ = 1;
			try
			{
				List<ArmSoft.Cloud.Client.Edge.DBConnectionInfo> dBConnections = edgeClient.GetDBConnections(_request.DatabaseName);
				if ((from p in dBConnections
					group p by p.HostName).Count() > 1)
				{
					throw new ASException(Resources.ErrRestoringMustExclusive);
				}
				Settings.CreateApiClient().AppManager.ChangeDbState(operational: false, Resources.DatabaseRestoring);
				foreach (ArmSoft.Cloud.Client.Edge.DBConnectionInfo item in dBConnections)
				{
					edgeClient.KillDBConnection(_request.DatabaseName, item.SPID);
				}
				if (!_request.DoNotArchiveBackup)
				{
					DecompressionResponse decompressionResponse = edgeClient.InvokeDecompression(_request.FileName);
					taskId = decompressionResponse.TaskId;
					fileName = decompressionResponse.EntryName;
					await Task.Run(() =>
					{
						short num = 0;
						while (num < 100)
						{
							num = edgeClient.GetProgress(taskId);
							switch (num)
							{
							case -1:
								throw new ASException(Resources.FileDoesNotExistWithoutName);
							case -2:
								throw new ASException(Resources.RestoreError);
							default:
								if (_progressIndicator != null)
								{
									ProgressIndicationValue value = new ProgressIndicationValue
									{
										ProgressStep = ProgressStep.ExtractBackupFile,
										UIMode = true,
										Percent = num
									};
									_progressIndicator.Report(value);
								}
								break;
							}
						}
					});
				}
				else
				{
					fileName = _request.FileName;
				}
				await RestoreCloudDatabaseFromBackupAsync(fileName);
				using CloudClient cloudClient = Settings.CloudClient;
				cloudClient.Token = Settings.GetApplicationToken();
				cloudClient.AS7X.AddCloudDatabaseLog(GetCloudDatabaseId(), new CloudDBLogCreationRequest
				{
					Action = CloudDBLogAction.Restore
				});
				Settings.CreateApiClient().AppManager.ChangeDbState(operational: true, Resources.DatabaseRestoring);
				Settings.LoginService.CacheStructChanged = true;
			}
			catch
			{
				Settings.CreateApiClient().AppManager.ChangeDbState(operational: true, Resources.DatabaseRestoring);
				throw;
			}
			finally
			{
				if (!_request.DoNotArchiveBackup && !string.IsNullOrWhiteSpace(fileName))
				{
					edgeClient.DeleteFile(fileName);
				}
			}
		}
		finally
		{
			if (edgeClient != null)
			{
				((IDisposable)edgeClient).Dispose();
			}
		}
	}

	private async Task RestoreCloudDatabaseFromBackupAsync(string fileName)
	{
		JobEnqueueResponse jobEnqueueResponse;
		if (ConfigHelper.ASAdminLogin)
		{
			using EdgeAdminClient edgeAdminClient = new EdgeAdminClient(Settings.LoginService.EdgeServiceAddress, ADHelper.ASAdminB2CToken);
			jobEnqueueResponse = edgeAdminClient.DatabaseRestoreJob.Enqueue(new DatabaseRestoreRequest
			{
				Database = _request.DatabaseName,
				Filename = fileName.Left(fileName.Length - 4)
			});
		}
		else
		{
			using EdgeClient edgeClient = new EdgeClient(Settings.LoginService.EdgeServiceAddress, Settings.GetApplicationToken(), ADHelper.ADB2CToken);
			jobEnqueueResponse = edgeClient.DatabaseRestoreJob.Enqueue(new DatabaseRestoreRequest
			{
				Database = _request.DatabaseName,
				Filename = fileName.Left(fileName.Length - 4)
			});
		}
		_jobId = jobEnqueueResponse.Id;
		await HandleCloudInfoMessage();
	}

	public async Task CreateBackupFileAsync()
	{
		await CreateCloudBackupFile();
	}

	public void DeleteBackupFile()
	{
		string fileName = $"\\{Settings.LoginService.Key}\\{_request.FileName}.bak";
		if (ConfigHelper.ASAdminLogin)
		{
			using (EdgeAdminClient edgeAdminClient = new EdgeAdminClient(Settings.LoginService.EdgeServiceAddress, ADHelper.ASAdminB2CToken))
			{
				edgeAdminClient.DeleteFile(Settings.LoginService.Key, fileName);
				return;
			}
		}
		using EdgeClient edgeClient = new EdgeClient(Settings.LoginService.EdgeServiceAddress, Settings.GetApplicationToken(), ADHelper.ADB2CToken);
		edgeClient.DeleteFile(fileName);
	}

	public void UpdateCloudDbLastBackupDate()
	{
		UpdateCloudDbLastBackupDate(GetCloudDatabaseId());
	}

	private Task HandleCloudInfoMessage()
	{
		if (ConfigHelper.ASAdminLogin)
		{
			return HandleCloudInfoMessageForASAdminUser();
		}
		return HandleCloudInfoMessageForUser();
	}

	private async Task HandleCloudInfoMessageForUser()
	{
		bool isBackupOperation = _request.Type == OperationTypes.Backup;
		string phaseName = (isBackupOperation ? "Backup percent" : "Restore percent");
		int iteration = 1;
		JobState state = JobState.Enqueued;
		using EdgeClient edgeClient = new EdgeClient(Settings.LoginService.EdgeServiceAddress, Settings.GetApplicationToken(), ADHelper.ADB2CToken);
		JobProgress jobProgress;
		do
		{
			await Task.Delay(GetDelayTimeForProgress(ref iteration));
			jobProgress = (isBackupOperation ? edgeClient.DatabaseBackupJob.GetProgress(_jobId) : edgeClient.DatabaseRestoreJob.GetProgress(_jobId));
			if (jobProgress != null)
			{
				JobProgressPhase jobProgressPhase = jobProgress.Details.FirstOrDefault((JobProgressPhase x) => x.Name == phaseName);
				if (jobProgressPhase != null && _progressIndicator != null)
				{
					ProgressIndicationValue value = new ProgressIndicationValue
					{
						ProgressStep = (isBackupOperation ? ProgressStep.CreateBackup : ProgressStep.RestoreBackup),
						Percent = (short)jobProgressPhase.Row,
						UIMode = true
					};
					_progressIndicator.Report(value);
				}
				state = jobProgress.State;
			}
		}
		while (jobProgress != null && state != JobState.Failed && state != JobState.Finished);
		if (isBackupOperation)
		{
			edgeClient.DatabaseBackupJob.GetResult(_jobId, delete: true);
		}
		else
		{
			edgeClient.DatabaseRestoreJob.GetResult(_jobId, delete: true);
		}
	}

	private async Task HandleCloudInfoMessageForASAdminUser()
	{
		bool isBackupOperation = _request.Type == OperationTypes.Backup;
		string phaseName = (isBackupOperation ? "Backup percent" : "Restore percent");
		int iteration = 1;
		JobState state = JobState.Enqueued;
		using EdgeAdminClient edgeClient = new EdgeAdminClient(Settings.LoginService.EdgeServiceAddress, ADHelper.ASAdminB2CToken);
		JobProgress jobProgress;
		do
		{
			await Task.Delay(GetDelayTimeForProgress(ref iteration));
			jobProgress = (isBackupOperation ? edgeClient.DatabaseBackupJob.GetProgress(_jobId) : edgeClient.DatabaseRestoreJob.GetProgress(_jobId));
			if (jobProgress != null)
			{
				JobProgressPhase jobProgressPhase = jobProgress.Details.FirstOrDefault((JobProgressPhase x) => x.Name == phaseName);
				if (jobProgressPhase != null && _progressIndicator != null)
				{
					ProgressIndicationValue value = new ProgressIndicationValue
					{
						ProgressStep = (isBackupOperation ? ProgressStep.CreateBackup : ProgressStep.RestoreBackup),
						Percent = (short)jobProgressPhase.Row,
						UIMode = true
					};
					_progressIndicator.Report(value);
				}
				state = jobProgress.State;
			}
		}
		while (jobProgress != null && state != JobState.Failed && state != JobState.Finished);
		if (isBackupOperation)
		{
			edgeClient.DatabaseBackupJob.GetResult(_jobId, delete: true);
		}
		else
		{
			edgeClient.DatabaseRestoreJob.GetResult(_jobId, delete: true);
		}
	}

	private static int GetDelayTimeForProgress(ref int iteration)
	{
		if (iteration >= 20)
		{
			return 1000;
		}
		int result = (int)((decimal)iteration / 2m).RoundAs(0) * 100;
		iteration++;
		return result;
	}

	public static void DeleteBackup(string fileName, int dbId)
	{
		if (ConfigHelper.ASAdminLogin)
		{
			using (EdgeAdminClient edgeAdminClient = new EdgeAdminClient(Settings.LoginService.EdgeServiceAddress, ADHelper.ASAdminB2CToken))
			{
				edgeAdminClient.DeleteFile(Settings.LoginService.Key, fileName);
			}
			if (dbId != -1)
			{
				Settings.CreateApiClient().AdminCloud.CloudDatabaseBackUpDeleted(ADHelper.ASAdminB2CToken, dbId);
			}
			return;
		}
		using (EdgeClient edgeClient = new EdgeClient(Settings.LoginService.EdgeServiceAddress, Settings.GetApplicationToken(), ADHelper.ADB2CToken))
		{
			edgeClient.DeleteFile(fileName);
		}
		if (dbId == -1)
		{
			return;
		}
		using CloudClient cloudClient = Settings.CloudClient;
		cloudClient.Token = Settings.GetApplicationToken();
		cloudClient.AS7X.CloudDatabaseBackUpDeleted(dbId);
	}
}
