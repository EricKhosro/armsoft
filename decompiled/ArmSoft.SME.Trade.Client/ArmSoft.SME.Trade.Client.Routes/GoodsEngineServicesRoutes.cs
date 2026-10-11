using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ArmSoft.SME.Client;
using ArmSoft.SME.Trade.Common.Models.Documents;

namespace ArmSoft.SME.Trade.Client.Routes;

public class GoodsEngineServicesRoutes
{
	public class PartyRowAndExpMethod
	{
		public string expMethod { get; set; }

		public List<PartyRow> partyRows { get; set; }
	}

	private readonly TradeClient client;

	internal GoodsEngineServicesRoutes(TradeClient client)
	{
		this.client = client;
	}

	public List<PartyRow> DistributeByExpiredDate(DateOnly date, int rowID, string storage, string goodCode, string unit, decimal quantity)
	{
		return AsyncHelper.RunSync(async () => await DistributeByExpiredDateAsync(date, rowID, storage, goodCode, unit, quantity));
	}

	public async Task<List<PartyRow>> DistributeByExpiredDateAsync(DateOnly date, int rowID, string storage, string goodCode, string unit, decimal quantity, CancellationToken cancellationToken = default(CancellationToken))
	{
		UrlQuery urlQuery = new UrlQuery();
		urlQuery.Add("date", date);
		urlQuery.Add("rowID", rowID);
		urlQuery.Add("storage", storage);
		urlQuery.Add("goodCode", goodCode);
		urlQuery.Add("unit", unit);
		urlQuery.Add("quantity", quantity);
		StringBuilder stringBuilder = client.CreateURLBuilder("/api/GoodsEngine/DistributeByExpiredDate", urlQuery);
		return await client.ExecuteAsync<List<PartyRow>>(HttpMethod.Get, stringBuilder.ToString(), cancellationToken);
	}

	public PartyRowAndExpMethod GetParties(string storage, string mtCode, DateOnly docDate)
	{
		return AsyncHelper.RunSync(async () => await GetPartiesAsync(storage, mtCode, docDate));
	}

	private async Task<PartyRowAndExpMethod> GetPartiesAsync(string storage, string mtCode, DateOnly docDate, CancellationToken cancellationToken = default(CancellationToken))
	{
		UrlQuery urlQuery = new UrlQuery();
		urlQuery.Add("storage", storage);
		urlQuery.Add("mtCode", mtCode);
		urlQuery.Add("docDate", docDate);
		StringBuilder stringBuilder = client.CreateURLBuilder("/api/GoodsEngine/GetParties", urlQuery);
		return await client.ExecuteAsync<PartyRowAndExpMethod>(HttpMethod.Get, stringBuilder.ToString(), cancellationToken);
	}

	public bool ExistOperation()
	{
		return AsyncHelper.RunSync(async () => await ExistOperationAsync());
	}

	private async Task<bool> ExistOperationAsync(CancellationToken cancellationToken = default(CancellationToken))
	{
		StringBuilder stringBuilder = client.CreateURLBuilder("/api/GoodsEngine/ExistOperation");
		return await client.ExecuteAsync<bool>(HttpMethod.Get, stringBuilder.ToString(), cancellationToken);
	}
}
