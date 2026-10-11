using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ArmSoft.InternetBank.Web.API.Common.Models;
using ArmSoft.OnlineBanking.Common.Enums;

namespace ArmSoft.InternetBank.APIClient;

public class AttachedCardManager : BaseManager
{
	internal AttachedCardManager(Client client)
		: base(client)
	{
	}

	public async Task<string> AttachCardAsync(string returnUrl = null)
	{
		return await client.PostAsync("attachcard", requireAuthorization: true, null, null, returnUrl);
	}

	public async Task<int> AttachCardResultV2Async(string orderId)
	{
		return await client.PostAsync<int>("v2/attachcardresult", requireAuthorization: true, null, null, orderId);
	}

	public async Task<List<AttachedCard>> GetAttachedCardsAsync()
	{
		return await client.GetAsync<List<AttachedCard>>("attachedcards", requireAuthorization: true);
	}

	public async Task<List<AttachedCardTransaction>> GetAttachedCardTransactionsAsync(int id, Period period, DateTime dateFrom, DateTime dateTo, bool showAmd = false, int? count = null, TransactionFilterOption filterOption = TransactionFilterOption.All)
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		dictionary["Period"] = period;
		dictionary["DateFrom"] = dateFrom;
		dictionary["DateTo"] = dateTo;
		dictionary["ShowAMD"] = showAmd;
		dictionary["Count"] = count;
		dictionary["FilterOption"] = filterOption;
		return await client.GetAsync<List<AttachedCardTransaction>>($"attachedCards/{id}/transactions", requireAuthorization: true, null, dictionary);
	}

	public async Task DeleteAttachedCardAsync(int id)
	{
		await client.PostAsync($"attachedCards/{id}/delete", requireAuthorization: true);
	}
}
