using ArmSoft.SME.Client;

namespace ArmSoft.SME.Core.UI.Services;

public class TempFSService
{
	protected virtual ApiClient Client => Settings.CreateApiClient();

	public byte[] Download(string fileId)
	{
		return Client.TempFS.Download(fileId);
	}

	public void DeleteTemporaryFile(string fileId)
	{
		Client.TempFS.DeleteTemporaryFile(fileId);
	}
}
