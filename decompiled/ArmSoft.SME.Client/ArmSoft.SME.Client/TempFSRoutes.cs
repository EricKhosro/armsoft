using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ArmSoft.SME.Client;

public class TempFSRoutes
{
	private readonly ApiClient _client;

	internal TempFSRoutes(ApiClient client)
	{
		_client = client;
	}

	public Task<byte[]> DownloadAsync(string fileId, CancellationToken cancellationToken = default(CancellationToken))
	{
		StringBuilder stringBuilder = _client.CreateURLBuilder("/api/TempFS/" + fileId);
		return _client.Download(HttpMethod.Get, stringBuilder.ToString(), cancellationToken);
	}

	public byte[] Download(string fileId)
	{
		return AsyncHelper.RunSync(async () => await DownloadAsync(fileId));
	}

	public Task DeleteTemporaryFileAsync(string fileId, CancellationToken cancellationToken = default(CancellationToken))
	{
		StringBuilder stringBuilder = _client.CreateURLBuilder("/api/TempFS/" + fileId);
		return _client.ExecuteAsync(HttpMethod.Delete, stringBuilder.ToString(), cancellationToken);
	}

	public void DeleteTemporaryFile(string fileId)
	{
		AsyncHelper.RunSync(async () =>
		{
			await DeleteTemporaryFileAsync(fileId);
		});
	}
}
