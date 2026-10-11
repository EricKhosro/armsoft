using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ArmSoft.SME.Client;
using ArmSoft.SME.Client.Models;
using ArmSoft.SME.Common.Models;

namespace ArmSoft.SME.Trade.Client;

public class AdditionalRoutes
{
	private readonly TradeClient client;

	internal AdditionalRoutes(TradeClient client)
	{
		this.client = client;
	}

	public Task<byte[]> GetMTImageAsync(int mtID, string qntUnit, int? userId = null, CancellationToken cancellationToken = default(CancellationToken))
	{
		UrlQuery urlQuery = new UrlQuery();
		urlQuery.Add("mtID", mtID);
		urlQuery.Add("qntUnit", qntUnit);
		urlQuery.Add("userId", userId);
		StringBuilder stringBuilder = client.CreateURLBuilder("/api/Additional/GetGoodImage", urlQuery);
		return client.ExecuteAsync<byte[]>(HttpMethod.Get, stringBuilder.ToString(), cancellationToken);
	}

	public byte[] GetMTImage(int mtID, string qntUnit, int? userId = null)
	{
		return AsyncHelper.RunSync(async () => await GetMTImageAsync(mtID, qntUnit, userId));
	}

	public Task<List<CodeName>> GetDocumentStates(CancellationToken cancellationToken)
	{
		UrlQuery query = new UrlQuery();
		StringBuilder stringBuilder = client.CreateURLBuilder("/api/Additional/GetDocumentStates", query);
		return client.ExecuteAsync<List<CodeName>>(HttpMethod.Get, stringBuilder.ToString(), cancellationToken);
	}

	public Task<DocInfoForPreview> GetDocInfoForPreviewAsync(Guid docISN, CancellationToken cancellationToken = default(CancellationToken))
	{
		UrlQuery urlQuery = new UrlQuery();
		urlQuery.Add("docISN", docISN);
		StringBuilder stringBuilder = client.CreateURLBuilder("/api/Additional/DocInfoForPreview", urlQuery);
		return client.ExecuteAsync<DocInfoForPreview>(HttpMethod.Get, stringBuilder.ToString(), cancellationToken);
	}

	public void SwapTwoMTs(int userID, int orderNum1, int orderNum2, CancellationToken cancellationToken = default(CancellationToken))
	{
		AsyncHelper.RunSync(async () =>
		{
			await SwapTwoMTsAsync(userID, orderNum1, orderNum2, cancellationToken);
		});
	}

	public async Task SwapTwoMTsAsync(int userID, int orderNum1, int orderNum2, CancellationToken cancellationToken = default(CancellationToken))
	{
		UrlQuery urlQuery = new UrlQuery();
		urlQuery.Add("userID", userID);
		urlQuery.Add("orderNum1", orderNum1);
		urlQuery.Add("orderNum2", orderNum2);
		StringBuilder stringBuilder = client.CreateURLBuilder("/api/Additional/SwapTwoMTs", urlQuery);
		await client.ExecuteAsync(HttpMethod.Get, stringBuilder.ToString(), cancellationToken);
	}

	public void SwapTwoFavGroups(int userID, int orderNum1, int orderNum2, CancellationToken cancellationToken = default(CancellationToken))
	{
		AsyncHelper.RunSync(async () =>
		{
			await SwapTwoFavGroupsAsync(userID, orderNum1, orderNum2, cancellationToken);
		});
	}

	public async Task SwapTwoFavGroupsAsync(int userID, int orderNum1, int orderNum2, CancellationToken cancellationToken = default(CancellationToken))
	{
		UrlQuery urlQuery = new UrlQuery();
		urlQuery.Add("userID", userID);
		urlQuery.Add("orderNum1", orderNum1);
		urlQuery.Add("orderNum2", orderNum2);
		StringBuilder stringBuilder = client.CreateURLBuilder("/api/Additional/SwapTwoFavGroups", urlQuery);
		await client.ExecuteAsync(HttpMethod.Get, stringBuilder.ToString(), cancellationToken);
	}

	public List<string> ExistPartnerWithSamePropertiesMsg(string code, string taxCode, string passportNum)
	{
		return AsyncHelper.RunSync(async () => await ExistPartnerWithSamePropertiesMsgAsync(code, taxCode, passportNum));
	}

	public async Task<List<string>> ExistPartnerWithSamePropertiesMsgAsync(string code, string taxCode, string passportNum, CancellationToken cancellationToken = default(CancellationToken))
	{
		UrlQuery urlQuery = new UrlQuery();
		urlQuery.Add("code", code);
		urlQuery.Add("taxCode", taxCode);
		urlQuery.Add("passportNum", passportNum);
		StringBuilder stringBuilder = client.CreateURLBuilder("/api/Additional/ExistPartnerWithSamePropertiesMsg", urlQuery);
		return await client.ExecuteAsync<List<string>>(HttpMethod.Get, stringBuilder.ToString(), cancellationToken);
	}
}
