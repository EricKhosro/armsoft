using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ArmSoft.SME.SM.Common.Models.ACS;

namespace ArmSoft.SME.Client.Routes;

public class ACSRoutes
{
	private readonly ApiClient _client;

	internal ACSRoutes(ApiClient client)
	{
		_client = client;
	}

	public Task<List<ProductInfoForBarCode>> GetProductInfoByBarCodeAsync(string barCode, CancellationToken cancellationToken = default(CancellationToken))
	{
		StringBuilder stringBuilder = _client.CreateURLBuilder("/api/ACS/GetProductInfoByBarCode/" + barCode);
		return _client.ExecuteAsync<List<ProductInfoForBarCode>>(HttpMethod.Get, stringBuilder.ToString(), cancellationToken);
	}

	public List<ProductInfoForBarCode> GetProductInfoByBarCode(string barCode)
	{
		return AsyncHelper.RunSync(async () => await GetProductInfoByBarCodeAsync(barCode));
	}
}
