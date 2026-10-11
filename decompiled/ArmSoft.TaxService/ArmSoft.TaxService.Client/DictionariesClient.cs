using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ArmSoft.TaxService.Models.Dictionaries;

namespace ArmSoft.TaxService.Client;

public class DictionariesClient
{
	private readonly TaxServiceClient client;

	private const string apiUrl = "/api/dictionaries";

	internal DictionariesClient(TaxServiceClient client)
	{
		this.client = client;
	}

	public Task<List<Classifier>> GetItemClassifierListAsync(CancellationToken cancellationToken = default(CancellationToken))
	{
		var requestObject = new
		{
			tin = client.Tin
		};
		StringBuilder stringBuilder = client.CreateURLBuilder("/api/dictionaries/item-classifier-list");
		return client.ExecuteAsync<List<Classifier>>(HttpMethod.Post, stringBuilder.ToString(), cancellationToken, requestObject);
	}

	public List<Classifier> GetItemClassifierList()
	{
		return AsyncHelper.RunSync(async () => await GetItemClassifierListAsync());
	}

	public Task<List<Classifier>> GetItemClassifierListAsync(List<Guid> ids, CancellationToken cancellationToken = default(CancellationToken))
	{
		var requestObject = new
		{
			tin = client.Tin,
			ids = ids
		};
		StringBuilder stringBuilder = client.CreateURLBuilder("/api/dictionaries/item-classifier-list-by-ids");
		return client.ExecuteAsync<List<Classifier>>(HttpMethod.Post, stringBuilder.ToString(), cancellationToken, requestObject);
	}

	public List<Classifier> GetItemClassifierList(List<Guid> ids)
	{
		return AsyncHelper.RunSync(async () => await GetItemClassifierListAsync(ids));
	}

	public Task<List<Classifier>> GetItemClassifierFullListAsync(CancellationToken cancellationToken = default(CancellationToken))
	{
		var requestObject = new
		{
			tin = client.Tin
		};
		StringBuilder stringBuilder = client.CreateURLBuilder("/api/dictionaries/item-classifier-full-list");
		return client.ExecuteAsync<List<Classifier>>(HttpMethod.Post, stringBuilder.ToString(), cancellationToken, requestObject);
	}

	public List<Classifier> GetItemClassifierFullList()
	{
		return AsyncHelper.RunSync(async () => await GetItemClassifierFullListAsync());
	}
}
