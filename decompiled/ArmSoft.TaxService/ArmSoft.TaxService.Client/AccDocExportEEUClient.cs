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

public class AccDocExportEEUClient
{
	private readonly TaxServiceClient client;

	private const string apiUrl = "/api/acc-doc-export-eeu";

	internal AccDocExportEEUClient(TaxServiceClient client)
	{
		this.client = client;
	}

	public async Task<AccDocExportEEU> GetAsync(Guid id, bool loadItems = false, bool throwException = true, CancellationToken cancellationToken = default(CancellationToken))
	{
		var requestObject = new { id };
		StringBuilder stringBuilder = client.CreateURLBuilder("/api/acc-doc-export-eeu/acc-doc-export-eeu-by-id");
		AccDocExportEEU doc = await client.ExecuteAsync<AccDocExportEEU>(HttpMethod.Post, stringBuilder.ToString(), cancellationToken, requestObject);
		if (loadItems && doc != null)
		{
			AccDocExportEEU accDocExportEEU = doc;
			accDocExportEEU.Items = await GetProductsListAsync(id, cancellationToken);
		}
		if ((doc == null) & throwException)
		{
			throw new ApiException(string.Format(Resources.Get(ResourceKey.DocNotExistWithId), id));
		}
		return doc;
	}

	public AccDocExportEEU Get(Guid id, bool loadItems = false, bool throwException = true)
	{
		return AsyncHelper.RunSync(async () => await GetAsync(id, loadItems, throwException));
	}

	public Task<List<AccDocExportEEU.Item>> GetProductsListAsync(Guid invoiceId, CancellationToken cancellationToken = default(CancellationToken))
	{
		var requestObject = new { invoiceId };
		StringBuilder stringBuilder = client.CreateURLBuilder("/api/acc-doc-export-eeu/acc-doc-export-eeu-product-by-invoice-id");
		return client.ExecuteAsync<List<AccDocExportEEU.Item>>(HttpMethod.Post, stringBuilder.ToString(), cancellationToken, requestObject);
	}

	public List<AccDocExportEEU.Item> GetProductsList(Guid invoiceId)
	{
		return AsyncHelper.RunSync(async () => await GetProductsListAsync(invoiceId));
	}

	public Task<object> GetProductCountAsync(Guid invoiceId, CancellationToken cancellationToken = default(CancellationToken))
	{
		var requestObject = new { invoiceId };
		StringBuilder stringBuilder = client.CreateURLBuilder("/api/acc-doc-export-eeu/acc-doc-export-eeu-product-count-by-invoice-id");
		return client.ExecuteAsync<object>(HttpMethod.Post, stringBuilder.ToString(), cancellationToken, requestObject);
	}

	public object GetProductCount(Guid invoiceId)
	{
		return AsyncHelper.RunSync(async () => await GetProductCountAsync(invoiceId));
	}

	public async Task<AccDocExportEEU> GetAsync(string serialNumber, bool loadItems = false, CancellationToken cancellationToken = default(CancellationToken))
	{
		var requestObject = new
		{
			serialNo = serialNumber
		};
		StringBuilder stringBuilder = client.CreateURLBuilder("/api/acc-doc-export-eeu/acc-doc-export-eeu-by-serial-no");
		AccDocExportEEU doc = await client.ExecuteAsync<AccDocExportEEU>(HttpMethod.Post, stringBuilder.ToString(), cancellationToken, requestObject);
		if (loadItems && doc != null)
		{
			AccDocExportEEU accDocExportEEU = doc;
			accDocExportEEU.Items = await GetProductsListAsync(doc.Id, cancellationToken);
		}
		if (doc == null)
		{
			throw new ApiException(string.Format(Resources.Get(ResourceKey.DocNotExistWithSerialNumber), serialNumber));
		}
		return doc;
	}

	public AccDocExportEEU Get(string serialNumber, bool loadItems = false)
	{
		return AsyncHelper.RunSync(async () => await GetAsync(serialNumber, loadItems));
	}

	public async Task CreateDraftAsync(AccDocExportEEU doc, bool rewrite = false, CancellationToken cancellationToken = default(CancellationToken))
	{
		doc.Source = Source.API;
		if (rewrite)
		{
			AccDocExportEEU accDocExportEEU = await GetAsync(doc.Id, loadItems: false, throwException: false, cancellationToken);
			if (accDocExportEEU == null)
			{
				await AccDocExportEEUCreateDraftAsync(doc, cancellationToken);
				return;
			}
			if (accDocExportEEU.Status != Status.Draft)
			{
				throw new ApiException(string.Format(Resources.Get(ResourceKey.InvoiceIsNotEditable), EncodeDecodeTypes.GetStatusCaption(accDocExportEEU.Status)));
			}
			await EditDraftAsync(doc, cancellationToken);
		}
		else
		{
			await AccDocExportEEUCreateDraftAsync(doc, cancellationToken);
		}
	}

	private async Task AccDocExportEEUCreateDraftAsync(AccDocExportEEU doc, CancellationToken cancellationToken = default(CancellationToken))
	{
		try
		{
			StringBuilder stringBuilder = client.CreateURLBuilder("/api/acc-doc-export-eeu/acc-doc-export-eeu-create-draft");
			await client.ExecuteAsync(HttpMethod.Post, stringBuilder.ToString(), cancellationToken, doc);
		}
		catch (Exception)
		{
			TaxDoc taxDoc = await new TaxDocsClient(client).GetInvoiceAsync(doc.Id, cancellationToken);
			if (taxDoc == null)
			{
				throw;
			}
			if (taxDoc.AccDocType == AccDocType.AccDocExportEEU)
			{
				throw new ApiException(string.Format(Resources.Get(ResourceKey.DocAlreadyExistWithDeliveryDateBuyer), doc.DeliveredAt.FormatDDMMYY(), doc.BuyerName, doc.Id));
			}
			string arg = ((taxDoc.AccDocType == AccDocType.None) ? EncodeDecodeTypes.GetInvoiceCaption(taxDoc.InvoiceType) : EncodeDecodeTypes.GetAccDocCaption(taxDoc.AccDocType));
			throw new ApiException(string.Format(Resources.Get(ResourceKey.OtherTypeDocAlreadyExist), taxDoc.DeliveredAt.Value.FormatDDMMYY(), arg, doc.Id));
		}
	}

	public void CreateDraft(AccDocExportEEU doc, bool rewrite = false)
	{
		AsyncHelper.RunSync(async () =>
		{
			await CreateDraftAsync(doc, rewrite);
		});
	}

	public async Task EditDraftAsync(AccDocExportEEU doc, CancellationToken cancellationToken = default(CancellationToken))
	{
		StringBuilder stringBuilder = client.CreateURLBuilder("/api/acc-doc-export-eeu/acc-doc-export-eeu-save-draft");
		await client.ExecuteAsync<AccDocExportEEU>(HttpMethod.Post, stringBuilder.ToString(), cancellationToken, doc);
	}

	public void EditDraft(AccDocExportEEU doc)
	{
		AsyncHelper.RunSync(async () =>
		{
			await EditDraftAsync(doc);
		});
	}

	public Task RemoveDraftAsync(Guid id, CancellationToken cancellationToken = default(CancellationToken))
	{
		var requestObject = new { id };
		StringBuilder stringBuilder = client.CreateURLBuilder("/api/acc-doc-export-eeu/acc-doc-export-eeu-remove-draft");
		return client.ExecuteAsync<AccDocExportEEU>(HttpMethod.Post, stringBuilder.ToString(), cancellationToken, requestObject);
	}

	public void RemoveDraft(Guid id)
	{
		AsyncHelper.RunSync(async () =>
		{
			await RemoveDraftAsync(id);
		});
	}

	public Task CreateProductAsync(AccDocExportEEU.Item item, CancellationToken cancellationToken = default(CancellationToken))
	{
		StringBuilder stringBuilder = client.CreateURLBuilder("/api/acc-doc-export-eeu/acc-doc-export-eeu-product-create");
		return client.ExecuteAsync<AccDocExportEEU.Item>(HttpMethod.Post, stringBuilder.ToString(), cancellationToken, item);
	}

	public void CreateProduct(AccDocExportEEU.Item item)
	{
		AsyncHelper.RunSync(async () =>
		{
			await CreateProductAsync(item);
		});
	}

	public Task EditProductAsync(AccDocExportEEU.Item item, CancellationToken cancellationToken = default(CancellationToken))
	{
		StringBuilder stringBuilder = client.CreateURLBuilder("/api/acc-doc-export-eeu/acc-doc-export-eeu-product-save");
		return client.ExecuteAsync<AccDocExportEEU.Item>(HttpMethod.Post, stringBuilder.ToString(), cancellationToken, item);
	}

	public void EditProduct(AccDocExportEEU.Item item)
	{
		AsyncHelper.RunSync(async () =>
		{
			await EditProductAsync(item);
		});
	}

	public Task RemoveProductAsync(Guid id, CancellationToken cancellationToken = default(CancellationToken))
	{
		var requestObject = new { id };
		StringBuilder stringBuilder = client.CreateURLBuilder("/api/acc-doc-export-eeu/acc-doc-export-eeu-product-remove");
		return client.ExecuteAsync<AccDocExportEEU.Item>(HttpMethod.Post, stringBuilder.ToString(), cancellationToken, requestObject);
	}

	public void RemoveProduct(Guid id)
	{
		AsyncHelper.RunSync(async () =>
		{
			await RemoveProductAsync(id);
		});
	}
}
