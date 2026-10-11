using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Dropbox.Api;
using Dropbox.Api.Files;
using Dropbox.Api.Stone;

namespace ArmSoft.SME.Core.UI.Helpers.Backups;

public static class DropBoxManager
{
	public static async Task UploadToDropBox(DropboxClient dbx, List<string> files, string fileName, IProgress<ProgressIndicationValue> progress, CancellationToken ct)
	{
		string sessionId = null;
		uint totalLength = 0u;
		uint uploaded = 0u;
		ProgressIndicationValue progressValue = new ProgressIndicationValue
		{
			ProgressStep = ProgressStep.UploadToDropBox,
			Percent = 0
		};
		progress.Report(progressValue);
		foreach (string file in files)
		{
			totalLength += Convert.ToUInt32(new FileInfo(file).Length);
		}
		if (files.Count == 1)
		{
			using FileStream stream = new FileStream(files[0], FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
			await dbx.Files.UploadAsync(new UploadArg("/" + fileName), stream);
			progressValue.Percent = 100;
			progress.Report(progressValue);
		}
		else
		{
			for (int idx = 0; idx <= files.Count - 1; idx++)
			{
				using (FileStream stream = new FileStream(files[idx], FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
				{
					if (idx == 0)
					{
						uint tmpLength = (uint)stream.Length;
						UploadSessionStartResult uploadSessionStartResult = await dbx.Files.UploadSessionStartAsync(new UploadSessionStartArg(), stream);
						uploaded += tmpLength;
						sessionId = uploadSessionStartResult.SessionId;
						progressValue.Percent = (short)((double)uploaded / (double)totalLength * 100.0);
						progress.Report(progressValue);
					}
					else
					{
						UploadSessionCursor cursor = new UploadSessionCursor(sessionId, uploaded);
						if (idx == files.Count - 1)
						{
							await dbx.Files.UploadSessionFinishAsync(cursor, new CommitInfo("/" + fileName), null, stream);
							progressValue.Percent = 100;
							progress.Report(progressValue);
						}
						else
						{
							uint tmpLength = (uint)stream.Length;
							await dbx.Files.UploadSessionAppendV2Async(new UploadSessionAppendArg(cursor), stream);
							uploaded += tmpLength;
							progressValue.Percent = (short)((double)uploaded / (double)totalLength * 100.0);
							progress.Report(progressValue);
						}
					}
				}
				if (ct.IsCancellationRequested)
				{
					break;
				}
			}
		}
		foreach (string file2 in files)
		{
			File.Delete(file2);
		}
		ct.ThrowIfCancellationRequested();
	}

	public static async Task<string> DownloadFromDropBox(DropboxClient dbx, string fileName, int chunkSize, IProgress<ProgressIndicationValue> progress, CancellationToken ct)
	{
		byte[] buffer = new byte[chunkSize];
		uint totalLength = Convert.ToUInt32((await dbx.Files.GetMetadataAsync("/" + fileName)).AsFile.Size);
		uint downloaded = 0u;
		ct.ThrowIfCancellationRequested();
		IDownloadResponse<FileMetadata> downloadResponse = await dbx.Files.DownloadAsync("/" + fileName);
		ct.ThrowIfCancellationRequested();
		Stream stream = await downloadResponse.GetContentAsStreamAsync();
		ct.ThrowIfCancellationRequested();
		ProgressIndicationValue progressValue = new ProgressIndicationValue
		{
			ProgressStep = ProgressStep.DownloadFromDropBox,
			Percent = 0
		};
		string tmpFileName = Settings.GetTempFileName();
		using (FileStream fs = new FileStream(tmpFileName, FileMode.OpenOrCreate, FileAccess.Write))
		{
			int bytesRead = await stream.ReadAsync(MemoryExtensions.AsMemory<byte>(buffer, 0, chunkSize), ct);
			while (bytesRead > 0)
			{
				await fs.WriteAsync(MemoryExtensions.AsMemory<byte>(buffer, 0, bytesRead), ct);
				downloaded += Convert.ToUInt32(bytesRead);
				progressValue.Percent = Convert.ToInt16((double)downloaded / (double)totalLength * 100.0);
				progress.Report(progressValue);
				if (!ct.IsCancellationRequested)
				{
					bytesRead = await stream.ReadAsync(MemoryExtensions.AsMemory<byte>(buffer, 0, chunkSize), ct);
					continue;
				}
				break;
			}
		}
		if (ct.IsCancellationRequested)
		{
			File.Delete(tmpFileName);
			ct.ThrowIfCancellationRequested();
		}
		return tmpFileName;
	}
}
