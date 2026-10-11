using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ArmSoft.SME.Common.Models;

namespace ArmSoft.SME.Client.Routes;

public class ImagesRoutes
{
	private readonly ApiClient _client;

	private static string ControllerName => "Images";

	public ImagesRoutes(ApiClient client)
	{
		_client = client;
	}

	public byte[] GetItemImage(short itemType, string imageKey)
	{
		return AsyncHelper.RunSync(async () => await GetItemImageAsync(itemType, imageKey));
	}

	public Task<byte[]> GetItemImageAsync(short itemType, string imageKey, CancellationToken cancellationToken = default(CancellationToken))
	{
		StringBuilder stringBuilder = _client.CreateURLBuilder($"/api/{ControllerName}/GetItemImage/{itemType}/{imageKey}");
		return _client.Download(HttpMethod.Get, stringBuilder.ToString(), cancellationToken);
	}

	public List<ItemImageInfo> GetItemsImagesInfo(short itemType)
	{
		StringBuilder stringBuilder = _client.CreateURLBuilder($"/api/{ControllerName}/GetItemsImagesInfo/{itemType}");
		return _client.Execute<List<ItemImageInfo>>(HttpMethod.Get, stringBuilder.ToString());
	}
}
