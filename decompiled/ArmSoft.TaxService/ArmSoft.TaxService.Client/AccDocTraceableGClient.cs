using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ArmSoft.TaxService.Enums;
using ArmSoft.TaxService.Extensions;
using ArmSoft.TaxService.Models;
using ArmSoft.TaxService.Models.AccDocs;

namespace ArmSoft.TaxService.Client;

public class AccDocTraceableGClient
{
	private readonly TaxServiceClient client;

	private const string apiUrl = "/api/acc-doc-traceable-g";

	internal AccDocTraceableGClient(TaxServiceClient client)
	{
		this.client = client;
	}

	public async Task<AccDocTraceableG> GetAsync(Guid id, bool loadItems = false, bool throwException = true, CancellationToken cancellationToken = default(CancellationToken))
	{
		var requestObject = new { id };
		StringBuilder stringBuilder = client.CreateURLBuilder("/api/acc-doc-traceable-g/acc-doc-traceable-g-by-id");
		AccDocTraceableG doc = await client.ExecuteAsync<AccDocTraceableG>(HttpMethod.Post, stringBuilder.ToString(), cancellationToken, requestObject);
		if (loadItems && doc != null)
		{
			AccDocTraceableG accDocTraceableG = doc;
			accDocTraceableG.Items = await GetProductsListAsync(id, cancellationToken);
		}
		if ((doc == null) & throwException)
		{
			throw new ApiException(string.Format(Resources.Get(ResourceKey.DocNotExistWithId), id));
		}
		return doc;
	}

	public AccDocTraceableG Get(Guid id, bool loadItems = false, bool throwException = true)
	{
		return AsyncHelper.RunSync(async () => await GetAsync(id, loadItems, throwException));
	}

	public Task<List<AccDocTraceableG.Item>> GetProductsListAsync(Guid invoiceId, CancellationToken cancellationToken = default(CancellationToken))
	{
		var requestObject = new { invoiceId };
		StringBuilder stringBuilder = client.CreateURLBuilder("/api/acc-doc-traceable-g/acc-doc-traceable-g-product-by-invoice-id");
		return client.ExecuteAsync<List<AccDocTraceableG.Item>>(HttpMethod.Post, stringBuilder.ToString(), cancellationToken, requestObject);
	}

	public List<AccDocTraceableG.Item> GetProductsList(Guid invoiceId)
	{
		return AsyncHelper.RunSync(async () => await GetProductsListAsync(invoiceId));
	}

	public Task<object> GetProductCountAsync(Guid invoiceId, CancellationToken cancellationToken = default(CancellationToken))
	{
		var requestObject = new { invoiceId };
		StringBuilder stringBuilder = client.CreateURLBuilder("/api/acc-doc-traceable-g/acc-doc-traceable-g-product-count-by-invoice-id");
		return client.ExecuteAsync<object>(HttpMethod.Post, stringBuilder.ToString(), cancellationToken, requestObject);
	}

	public object GetProductCount(Guid invoiceId)
	{
		return AsyncHelper.RunSync(async () => await GetProductCountAsync(invoiceId));
	}

	public async Task<AccDocTraceableG> GetAsync(string serialNumber, bool loadItems = false, CancellationToken cancellationToken = default(CancellationToken))
	{
		var requestObject = new
		{
			serialNo = serialNumber
		};
		StringBuilder stringBuilder = client.CreateURLBuilder("/api/acc-doc-traceable-g/acc-doc-traceable-g-by-serial-no");
		AccDocTraceableG doc = await client.ExecuteAsync<AccDocTraceableG>(HttpMethod.Post, stringBuilder.ToString(), cancellationToken, requestObject);
		if (loadItems && doc != null)
		{
			AccDocTraceableG accDocTraceableG = doc;
			accDocTraceableG.Items = await GetProductsListAsync(doc.Id, cancellationToken);
		}
		if (doc == null)
		{
			throw new ApiException(string.Format(Resources.Get(ResourceKey.DocNotExistWithSerialNumber), serialNumber));
		}
		return doc;
	}

	public AccDocTraceableG Get(string serialNumber, bool loadItems = false)
	{
		return AsyncHelper.RunSync(async () => await GetAsync(serialNumber, loadItems));
	}

	public async Task CreateDraftAsync(AccDocTraceableG doc, bool rewrite = false, CancellationToken cancellationToken = default(CancellationToken))
	{
		doc.Source = Source.API;
		if (rewrite)
		{
			AccDocTraceableG accDocTraceableG = await GetAsync(doc.Id, loadItems: false, throwException: false, cancellationToken);
			if (accDocTraceableG == null)
			{
				await AccDocTraceableGCreateDraftAsync(doc, cancellationToken);
				return;
			}
			if (accDocTraceableG.Status != Status.Draft)
			{
				throw new ApiException(string.Format(Resources.Get(ResourceKey.InvoiceIsNotEditable), EncodeDecodeTypes.GetStatusCaption(accDocTraceableG.Status)));
			}
			await EditDraftAsync(doc, cancellationToken);
		}
		else
		{
			await AccDocTraceableGCreateDraftAsync(doc, cancellationToken);
		}
	}

	private async Task AccDocTraceableGCreateDraftAsync(AccDocTraceableG doc, CancellationToken cancellationToken = default(CancellationToken))
	{
		try
		{
			StringBuilder stringBuilder = client.CreateURLBuilder("/api/acc-doc-traceable-g/acc-doc-traceable-g-create-draft");
			await client.ExecuteAsync(HttpMethod.Post, stringBuilder.ToString(), cancellationToken, doc);
		}
		catch (Exception)
		{
			TaxDoc taxDoc = await new TaxDocsClient(client).GetInvoiceAsync(doc.Id, cancellationToken);
			if (taxDoc == null)
			{
				throw;
			}
			if (taxDoc.AccDocType == AccDocType.AccDocTraceableG)
			{
				throw new ApiException(string.Format(Resources.Get(ResourceKey.DocAlreadyExistWithDeliveryDate), doc.DeliveredAt.FormatDDMMYY(), doc.Id));
			}
			string arg = ((taxDoc.AccDocType == AccDocType.None) ? EncodeDecodeTypes.GetInvoiceCaption(taxDoc.InvoiceType) : EncodeDecodeTypes.GetAccDocCaption(taxDoc.AccDocType));
			throw new ApiException(string.Format(Resources.Get(ResourceKey.OtherTypeDocAlreadyExist), taxDoc.DeliveredAt.Value.FormatDDMMYY(), arg, doc.Id));
		}
	}

	public void CreateDraft(AccDocTraceableG doc, bool rewrite = false)
	{
		AsyncHelper.RunSync(async () =>
		{
			await CreateDraftAsync(doc, rewrite);
		});
	}

	public async Task EditDraftAsync(AccDocTraceableG doc, CancellationToken cancellationToken = default(CancellationToken))
	{
		StringBuilder stringBuilder = client.CreateURLBuilder("/api/acc-doc-traceable-g/acc-doc-traceable-g-save-draft");
		await client.ExecuteAsync<AccDocTraceableG>(HttpMethod.Post, stringBuilder.ToString(), cancellationToken, doc);
	}

	public void EditDraft(AccDocTraceableG doc)
	{
		AsyncHelper.RunSync(async () =>
		{
			await EditDraftAsync(doc);
		});
	}

	public Task RemoveDraftAsync(Guid id, CancellationToken cancellationToken = default(CancellationToken))
	{
		var requestObject = new { id };
		StringBuilder stringBuilder = client.CreateURLBuilder("/api/acc-doc-traceable-g/acc-doc-traceable-g-remove-draft");
		return client.ExecuteAsync<AccDocTraceableG>(HttpMethod.Post, stringBuilder.ToString(), cancellationToken, requestObject);
	}

	public void RemoveDraft(Guid id)
	{
		AsyncHelper.RunSync(async () =>
		{
			await RemoveDraftAsync(id);
		});
	}

	public Task CreateProductAsync(AccDocTraceableG.Item item, CancellationToken cancellationToken = default(CancellationToken))
	{
		StringBuilder stringBuilder = client.CreateURLBuilder("/api/acc-doc-traceable-g/acc-doc-traceable-g-product-create");
		return client.ExecuteAsync<AccDocTraceableG.Item>(HttpMethod.Post, stringBuilder.ToString(), cancellationToken, item);
	}

	public void CreateProduct(AccDocTraceableG.Item item)
	{
		AsyncHelper.RunSync(async () =>
		{
			await CreateProductAsync(item);
		});
	}

	public Task EditProductAsync(AccDocTraceableG.Item item, CancellationToken cancellationToken = default(CancellationToken))
	{
		StringBuilder stringBuilder = client.CreateURLBuilder("/api/acc-doc-traceable-g/acc-doc-traceable-g-product-save");
		return client.ExecuteAsync<AccDocTraceableG.Item>(HttpMethod.Post, stringBuilder.ToString(), cancellationToken, item);
	}

	public void EditProduct(AccDocTraceableG.Item item)
	{
		AsyncHelper.RunSync(async () =>
		{
			await EditProductAsync(item);
		});
	}

	public Task RemoveProductAsync(Guid id, CancellationToken cancellationToken = default(CancellationToken))
	{
		var requestObject = new { id };
		StringBuilder stringBuilder = client.CreateURLBuilder("/api/acc-doc-traceable-g/acc-doc-traceable-g-product-remove");
		return client.ExecuteAsync<AccDocTraceableG.Item>(HttpMethod.Post, stringBuilder.ToString(), cancellationToken, requestObject);
	}

	public void RemoveProduct(Guid id)
	{
		AsyncHelper.RunSync(async () =>
		{
			await RemoveProductAsync(id);
		});
	}
}
