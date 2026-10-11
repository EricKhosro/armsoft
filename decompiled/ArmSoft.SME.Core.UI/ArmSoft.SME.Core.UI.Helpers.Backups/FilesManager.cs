using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using ArmSoft.Cloud.Client;
using ArmSoft.Cloud.Client.Edge;

namespace ArmSoft.SME.Core.UI.Helpers.Backups;

public class FilesManager
{
	private const int _bufferSize = 500000;

	private readonly IProgress<ProgressIndicationValue> _progressIndicator;

	private readonly UploadDownloadRequest _request;

	public FilesManager(UploadDownloadRequest request, IProgress<ProgressIndicationValue> progressIndicator = null)
	{
		_progressIndicator = progressIndicator;
		_request = request;
	}

	public async Task<List<string>> DownloadBackupFileAsync(int chunksPerFile = 1, bool copyToTemp = false, CancellationToken ct = default(CancellationToken))
	{
		List<string> list = await DownloadFileCloudAsync(chunksPerFile, copyToTemp, ct);
		if (ct != CancellationToken.None && ct.IsCancellationRequested)
		{
			foreach (string item in list)
			{
				File.Delete(item);
			}
			ct.ThrowIfCancellationRequested();
		}
		return list;
	}

	private Task<List<string>> DownloadFileCloudAsync(int chunksPerFile, bool copyToTemp, CancellationToken ct)
	{
		if (ConfigHelper.ASAdminLogin)
		{
			return DownloadFileCloudForASAdminAsync(chunksPerFile, copyToTemp, ct);
		}
		return DownloadFileCloudForUserAsync(chunksPerFile, copyToTemp, ct);
	}

	private async Task<List<string>> DownloadFileCloudForUserAsync(int chunksPerFile, bool copyToTemp, CancellationToken ct)
	{
		List<string> result = new List<string>();
		long bytesToRead = 0L;
		using (EdgeClient edgeClient = new EdgeClient(Settings.LoginService.EdgeServiceAddress, Settings.GetApplicationToken(), ADHelper.ADB2CToken))
		{
			bytesToRead = edgeClient.GetFileStreamLength(_request.SourceFileName);
		}
		await Task.Run(async () =>
		{
			byte[] array = new byte[500000];
			int bytesRead = 0;
			long offset = 0L;
			do
			{
				int chunksCount = chunksPerFile;
				if (copyToTemp)
				{
					_request.DestinationFileName = Settings.GetTempFileName();
					result.Add(_request.DestinationFileName);
				}
				using FileStream fs = new FileStream(_request.DestinationFileName, FileMode.Append, FileAccess.Write);
				do
				{
					using (EdgeClient edgeClient2 = new EdgeClient(Settings.LoginService.EdgeServiceAddress, Settings.GetApplicationToken(), ADHelper.ADB2CToken))
					{
						DownloadResponse downloadResponse = edgeClient2.Download(new DownloadRequest
						{
							FileName = _request.SourceFileName,
							Offset = offset,
							BufferLength = 500000
						});
						array = downloadResponse.Buffer;
						bytesRead = downloadResponse.BytesRead;
					}
					offset += bytesRead;
					await fs.WriteAsync(MemoryExtensions.AsMemory<byte>(array, 0, bytesRead));
					if (_progressIndicator != null)
					{
						_progressIndicator.Report(new ProgressIndicationValue
						{
							ProgressStep = _request.ProgressStep,
							UIMode = true,
							Percent = (short)((double)offset / (double)bytesToRead * 100.0)
						});
					}
					chunksCount--;
				}
				while (bytesRead > 0 && chunksCount > 0 && (ct == CancellationToken.None || !ct.IsCancellationRequested));
			}
			while (bytesRead > 0 && (ct == CancellationToken.None || !ct.IsCancellationRequested));
			if (!copyToTemp)
			{
				result.Add(_request.DestinationFileName);
			}
		}, ct);
		return result;
	}

	private async Task<List<string>> DownloadFileCloudForASAdminAsync(int chunksPerFile, bool copyToTemp, CancellationToken ct)
	{
		List<string> result = new List<string>();
		long bytesToRead = 0L;
		EdgeAdminClient edgeClient = new EdgeAdminClient(Settings.LoginService.EdgeServiceAddress, ADHelper.ASAdminB2CToken);
		try
		{
			bytesToRead = edgeClient.GetFileStreamLength(Settings.LoginService.Key, _request.SourceFileName);
			await Task.Run(async () =>
			{
				_ = new byte[500000];
				int bytesRead = 0;
				long offset = 0L;
				do
				{
					int chunksCount = chunksPerFile;
					if (copyToTemp)
					{
						_request.DestinationFileName = Settings.GetTempFileName();
						result.Add(_request.DestinationFileName);
					}
					using FileStream fs = new FileStream(_request.DestinationFileName, FileMode.Append, FileAccess.Write);
					do
					{
						DownloadResponse downloadResponse = edgeClient.Download(Settings.LoginService.Key, new DownloadRequest
						{
							FileName = _request.SourceFileName,
							Offset = offset,
							BufferLength = 500000
						});
						byte[] buffer = downloadResponse.Buffer;
						bytesRead = downloadResponse.BytesRead;
						offset += bytesRead;
						await fs.WriteAsync(MemoryExtensions.AsMemory<byte>(buffer, 0, bytesRead));
						if (_progressIndicator != null)
						{
							_progressIndicator.Report(new ProgressIndicationValue
							{
								ProgressStep = _request.ProgressStep,
								UIMode = true,
								Percent = (short)((double)offset / (double)bytesToRead * 100.0)
							});
						}
						chunksCount--;
					}
					while (bytesRead > 0 && chunksCount > 0 && (ct == CancellationToken.None || !ct.IsCancellationRequested));
				}
				while (bytesRead > 0 && (ct == CancellationToken.None || !ct.IsCancellationRequested));
				if (!copyToTemp)
				{
					result.Add(_request.DestinationFileName);
				}
			}, ct);
		}
		finally
		{
			if (edgeClient != null)
			{
				((IDisposable)edgeClient).Dispose();
			}
		}
		return result;
	}

	public async Task UploadBackupFileAsync(CancellationToken ct = default(CancellationToken))
	{
		await UploadFileCloudAsync(ct);
	}

	private async Task UploadFileCloudAsync(CancellationToken ct)
	{
		await Task.Run(async () =>
		{
			using FileStream fsSource = new FileStream(_request.SourceFileName, FileMode.Open, FileAccess.Read);
			byte[] buffer = new byte[500000];
			long bytesToRead = fsSource.Length;
			long offset = 0L;
			int num;
			do
			{
				num = await fsSource.ReadAsync(MemoryExtensions.AsMemory<byte>(buffer, 0, 500000));
				if (ConfigHelper.ASAdminLogin)
				{
					using EdgeAdminClient edgeAdminClient2 = new EdgeAdminClient(Settings.LoginService.EdgeServiceAddress, ADHelper.ASAdminB2CToken);
					edgeAdminClient2.Upload(Settings.LoginService.Key, new UploadRequest
					{
						FileName = _request.DestinationFileName,
						Offset = offset,
						Buffer = buffer,
						BytesLength = num
					});
				}
				else
				{
					using EdgeClient edgeClient2 = new EdgeClient(Settings.LoginService.EdgeServiceAddress, Settings.GetApplicationToken(), ADHelper.ADB2CToken);
					edgeClient2.Upload(new UploadRequest
					{
						FileName = _request.DestinationFileName,
						Offset = offset,
						Buffer = buffer,
						BytesLength = num
					});
				}
				offset += num;
				if (_progressIndicator != null)
				{
					_progressIndicator.Report(new ProgressIndicationValue
					{
						ProgressStep = _request.ProgressStep,
						UIMode = true,
						Percent = (short)((double)offset / (double)bytesToRead * 100.0)
					});
				}
			}
			while (num > 0 && (ct == CancellationToken.None || !ct.IsCancellationRequested));
		}, ct);
		if (!(ct != CancellationToken.None) || !ct.IsCancellationRequested)
		{
			return;
		}
		File.Delete(_request.SourceFileName);
		if (!ct.IsCancellationRequested)
		{
			return;
		}
		if (ConfigHelper.ASAdminLogin)
		{
			using EdgeAdminClient edgeAdminClient = new EdgeAdminClient(Settings.LoginService.EdgeServiceAddress, ADHelper.ASAdminB2CToken);
			edgeAdminClient.DeleteFile(Settings.LoginService.Key, _request.DestinationFileName);
		}
		else
		{
			using EdgeClient edgeClient = new EdgeClient(Settings.LoginService.EdgeServiceAddress, Settings.GetApplicationToken(), ADHelper.ADB2CToken);
			edgeClient.DeleteFile(_request.DestinationFileName);
		}
		ct.ThrowIfCancellationRequested();
	}
}
