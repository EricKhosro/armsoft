using System;
using System.Collections.Generic;
using System.Text;

namespace ArmSoft.Cloud.Client.Discount;

public class DiscountRoutes
{
	private readonly CloudClient client;

	internal DiscountRoutes(CloudClient client)
	{
		this.client = client;
	}

	public CardInfo GetCardInfo(string cardNumber)
	{
		StringBuilder stringBuilder = client.CreateURLBuilder("api/Discount/");
		stringBuilder.Append(cardNumber);
		return client.Send<CardInfo>("GET", stringBuilder.ToString());
	}

	public void CreateCardInfo(CardCreateRequest request, bool updateIfExists)
	{
		StringBuilder stringBuilder = client.CreateURLBuilder("api/Discount");
		stringBuilder.Append("?updateIfExists=");
		stringBuilder.Append(updateIfExists);
		client.Send("POST", stringBuilder.ToString(), request);
	}

	public void UpdateCardsSyncState(List<string> request)
	{
		StringBuilder stringBuilder = client.CreateURLBuilder("api/Discount/Sync");
		client.Send("PUT", stringBuilder.ToString(), request);
	}

	public void UpdateCardInfo(CardUpdateRequest request)
	{
		StringBuilder stringBuilder = client.CreateURLBuilder("api/Discount");
		client.Send("PUT", stringBuilder.ToString(), request);
	}

	public List<CardInfo> GetCards(bool? synced = null)
	{
		StringBuilder stringBuilder = client.CreateURLBuilder("api/Discount");
		if (synced.HasValue)
		{
			stringBuilder.Append("?synced=");
			stringBuilder.Append(synced.Value);
		}
		return client.Send<List<CardInfo>>("GET", stringBuilder.ToString());
	}

	public CardTransaction GetTransaction(Guid transactionISN)
	{
		StringBuilder stringBuilder = client.CreateURLBuilder("api/Discount/Transactions/");
		stringBuilder.Append(transactionISN.ToString());
		return client.Send<CardTransaction>("GET", stringBuilder.ToString());
	}

	public List<CardTransaction> GetTransactions(string cardNumber, DateTime? periodStart, DateTime? periodEnd, string cashCode, string shiftNumber, bool fillDocInfo, bool showDeleted)
	{
		UrlQuery urlQuery = new UrlQuery();
		if (!string.IsNullOrWhiteSpace(cardNumber))
		{
			urlQuery.Add("cardNumber", cardNumber);
		}
		if (!string.IsNullOrWhiteSpace(cashCode) && !string.IsNullOrWhiteSpace(shiftNumber))
		{
			urlQuery.Add("cashCode", cashCode);
			urlQuery.Add("shiftNumber", shiftNumber);
		}
		else
		{
			urlQuery.Add("periodStart", periodStart);
			urlQuery.Add("periodEnd", periodEnd);
		}
		urlQuery.Add("fillDocInfo", fillDocInfo);
		urlQuery.Add("showDeleted", showDeleted);
		StringBuilder stringBuilder = client.CreateURLBuilder("api/Discount/Transactions", urlQuery);
		return client.Send<List<CardTransaction>>("GET", stringBuilder.ToString());
	}

	public CardTransaction CreateTransaction(CardTransactionRequest request, TimeSpan? timeout)
	{
		StringBuilder stringBuilder = client.CreateURLBuilder("api/Discount/Transactions");
		return client.Send<CardTransaction>("POST", stringBuilder.ToString(), request, timeout);
	}

	public List<CardTransaction> CreateTransactions(List<CardTransactionRequest> request, TimeSpan? timeout)
	{
		StringBuilder stringBuilder = client.CreateURLBuilder("api/Discount/Transactions/Batch");
		return client.Send<List<CardTransaction>>("POST", stringBuilder.ToString(), request, timeout);
	}

	public void DeleteTransaction(Guid transactionISN)
	{
		StringBuilder stringBuilder = client.CreateURLBuilder("api/Discount/Transactions");
		client.Send("DELETE", stringBuilder.ToString(), transactionISN);
	}

	public void DeleteTransactions(Guid docISN)
	{
		StringBuilder stringBuilder = client.CreateURLBuilder("api/Discount/Transactions/DocISN");
		client.Send("DELETE", stringBuilder.ToString(), docISN);
	}

	public Guid CreateSecret(AppSecret request)
	{
		StringBuilder stringBuilder = client.CreateURLBuilder("api/Discount/Secrets");
		return client.Send<Guid>("POST", stringBuilder.ToString(), request);
	}

	public void UpdateSecret(AppSecret request)
	{
		StringBuilder stringBuilder = client.CreateURLBuilder("api/Discount/Secrets");
		client.Send("PUT", stringBuilder.ToString(), request);
	}

	public List<AppSecret> GetSecrets(Guid? applicationid)
	{
		StringBuilder stringBuilder = client.CreateURLBuilder("api/Discount/Secrets");
		if (applicationid.HasValue)
		{
			StringBuilder stringBuilder2 = stringBuilder;
			StringBuilder.AppendInterpolatedStringHandler handler = new StringBuilder.AppendInterpolatedStringHandler(7, 1, stringBuilder2);
			handler.AppendLiteral("?appId=");
			handler.AppendFormatted(applicationid);
			stringBuilder2.Append(ref handler);
		}
		return client.Send<List<AppSecret>>("GET", stringBuilder.ToString());
	}

	public List<VariantHash> GetHashes()
	{
		StringBuilder stringBuilder = client.CreateURLBuilder("api/Discount/ProductsAdministration");
		return client.Send<List<VariantHash>>("GET", stringBuilder.ToString());
	}

	public void ProductsSynchronization(List<ProductCreateRequest> products)
	{
		StringBuilder stringBuilder = client.CreateURLBuilder("api/Discount/ProductsAdministration");
		client.Send("POST", stringBuilder.ToString(), products);
	}

	public void MakeHidden(List<int> products)
	{
		StringBuilder stringBuilder = client.CreateURLBuilder("api/Discount/ProductsAdministration");
		client.Send("PUT", stringBuilder.ToString(), products);
	}
}
