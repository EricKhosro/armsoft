using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ArmSoft.TaxService.Enums;
using ArmSoft.TaxService.Extensions;
using ArmSoft.TaxService.Models;
using ArmSoft.TaxService.Models.Invoices;

namespace ArmSoft.TaxService.Client;

public class InvoiceVatRefundClient
{
	private readonly TaxServiceClient client;

	private const string apiUrl = "/api/vat-refund";

	internal InvoiceVatRefundClient(TaxServiceClient client)
	{
		this.client = client;
	}

	public async Task<VatRefund> GetAsync(Guid id, bool loadItems = false, bool throwException = true, CancellationToken cancellationToken = default(CancellationToken))
	{
		var requestObject = new { id };
		StringBuilder stringBuilder = client.CreateURLBuilder("/api/vat-refund/vat-refund-by-id");
		VatRefund doc = await client.ExecuteAsync<VatRefund>(HttpMethod.Post, stringBuilder.ToString(), cancellationToken, requestObject);
		if (loadItems && doc != null)
		{
			VatRefund vatRefund = doc;
			vatRefund.Items = await GetProductsListAsync(id, cancellationToken);
		}
		if ((doc == null) & throwException)
		{
			throw new ApiException(string.Format(Resources.Get(ResourceKey.DocNotExistWithId), id));
		}
		return doc;
	}

	public VatRefund Get(Guid id, bool loadItems = false, bool throwException = true)
	{
		return AsyncHelper.RunSync(async () => await GetAsync(id, loadItems, throwException));
	}

	public Task<List<VatRefund.Item>> GetProductsListAsync(Guid invoiceId, CancellationToken cancellationToken = default(CancellationToken))
	{
		var requestObject = new { invoiceId };
		StringBuilder stringBuilder = client.CreateURLBuilder("/api/vat-refund/vat-refund-product-by-invoice-id");
		return client.ExecuteAsync<List<VatRefund.Item>>(HttpMethod.Post, stringBuilder.ToString(), cancellationToken, requestObject);
	}

	public List<VatRefund.Item> GetProductsList(Guid invoiceId)
	{
		return AsyncHelper.RunSync(async () => await GetProductsListAsync(invoiceId));
	}

	public Task<object> GetProductCountAsync(Guid invoiceId, CancellationToken cancellationToken = default(CancellationToken))
	{
		var requestObject = new { invoiceId };
		StringBuilder stringBuilder = client.CreateURLBuilder("/api/vat-refund/vat-refund-product-count-by-invoice-id");
		return client.ExecuteAsync<object>(HttpMethod.Post, stringBuilder.ToString(), cancellationToken, requestObject);
	}

	public object GetProductCount(Guid invoiceId)
	{
		return AsyncHelper.RunSync(async () => await GetProductCountAsync(invoiceId));
	}

	public async Task<VatRefund> GetAsync(string serialNumber, bool loadItems = false, CancellationToken cancellationToken = default(CancellationToken))
	{
		var requestObject = new
		{
			serialNo = serialNumber
		};
		StringBuilder stringBuilder = client.CreateURLBuilder("/api/vat-refund/vat-refund-by-serial-no");
		VatRefund doc = await client.ExecuteAsync<VatRefund>(HttpMethod.Post, stringBuilder.ToString(), cancellationToken, requestObject);
		if (loadItems && doc != null)
		{
			VatRefund vatRefund = doc;
			vatRefund.Items = await GetProductsListAsync(doc.Id, cancellationToken);
		}
		if (doc == null)
		{
			throw new ApiException(string.Format(Resources.Get(ResourceKey.DocNotExistWithSerialNumber), serialNumber));
		}
		return doc;
	}

	public VatRefund Get(string serialNumber, bool loadItems = false)
	{
		return AsyncHelper.RunSync(async () => await GetAsync(serialNumber, loadItems));
	}

	public async Task CreateDraftAsync(VatRefund doc, bool rewrite = false, CancellationToken cancellationToken = default(CancellationToken))
	{
		ModelFieldValidator.Validate(doc);
		doc.Source = Source.API;
		if (rewrite)
		{
			VatRefund vatRefund = await GetAsync(doc.Id, loadItems: false, throwException: false, cancellationToken);
			if (vatRefund == null)
			{
				await VatRefundCreateDraftAsync(doc, cancellationToken);
				return;
			}
			if (vatRefund.Status != Status.Draft)
			{
				throw new ApiException(string.Format(Resources.Get(ResourceKey.InvoiceIsNotEditable), EncodeDecodeTypes.GetStatusCaption(vatRefund.Status)));
			}
			await EditDraftAsync(doc, cancellationToken);
		}
		else
		{
			await VatRefundCreateDraftAsync(doc, cancellationToken);
		}
	}

	private async Task VatRefundCreateDraftAsync(VatRefund doc, CancellationToken cancellationToken)
	{
		try
		{
			StringBuilder stringBuilder = client.CreateURLBuilder("/api/vat-refund/vat-refund-create-draft");
			await client.ExecuteAsync(HttpMethod.Post, stringBuilder.ToString(), cancellationToken, doc);
		}
		catch (Exception)
		{
			TaxDoc taxDoc = await new TaxDocsClient(client).GetInvoiceAsync(doc.Id, cancellationToken);
			if (taxDoc == null)
			{
				throw;
			}
			if (taxDoc.InvoiceType == InvoiceType.VatReturn)
			{
				throw new ApiException(string.Format(Resources.Get(ResourceKey.DocAlreadyExistWithDeliveryDate), doc.DeliveredAt.FormatDDMMYY(), doc.Id));
			}
			string arg = ((taxDoc.InvoiceType == InvoiceType.None) ? EncodeDecodeTypes.GetAccDocCaption(taxDoc.AccDocType) : EncodeDecodeTypes.GetInvoiceCaption(taxDoc.InvoiceType));
			throw new ApiException(string.Format(Resources.Get(ResourceKey.OtherTypeDocAlreadyExist), taxDoc.DeliveredAt.Value.FormatDDMMYY(), arg, doc.Id));
		}
	}

	public void CreateDraft(VatRefund doc, bool rewrite = false)
	{
		AsyncHelper.RunSync(async () =>
		{
			await CreateDraftAsync(doc, rewrite);
		});
	}

	public async Task EditDraftAsync(VatRefund doc, CancellationToken cancellationToken = default(CancellationToken))
	{
		StringBuilder stringBuilder = client.CreateURLBuilder("/api/vat-refund/vat-refund-save-draft");
		await client.ExecuteAsync<VatRefund>(HttpMethod.Post, stringBuilder.ToString(), cancellationToken, doc);
	}

	public void EditDraft(VatRefund doc)
	{
		AsyncHelper.RunSync(async () =>
		{
			await EditDraftAsync(doc);
		});
	}

	public Task RemoveDraftAsync(Guid id, CancellationToken cancellationToken = default(CancellationToken))
	{
		var requestObject = new { id };
		StringBuilder stringBuilder = client.CreateURLBuilder("/api/vat-refund/vat-refund-remove-draft");
		return client.ExecuteAsync<VatRefund>(HttpMethod.Post, stringBuilder.ToString(), cancellationToken, requestObject);
	}

	public void RemoveDraft(Guid id)
	{
		AsyncHelper.RunSync(async () =>
		{
			await RemoveDraftAsync(id);
		});
	}

	public Task CreateProductAsync(VatRefund.Item item, CancellationToken cancellationToken = default(CancellationToken))
	{
		StringBuilder stringBuilder = client.CreateURLBuilder("/api/vat-refund/vat-refund-product-create");
		return client.ExecuteAsync<VatRefund.Item>(HttpMethod.Post, stringBuilder.ToString(), cancellationToken, item);
	}

	public void CreateProduct(VatRefund.Item item)
	{
		AsyncHelper.RunSync(async () =>
		{
			await CreateProductAsync(item);
		});
	}

	public Task EditProductAsync(VatRefund.Item item, CancellationToken cancellationToken = default(CancellationToken))
	{
		StringBuilder stringBuilder = client.CreateURLBuilder("/api/vat-refund/vat-refund-product-save");
		return client.ExecuteAsync<VatRefund.Item>(HttpMethod.Post, stringBuilder.ToString(), cancellationToken, item);
	}

	public void EditProduct(VatRefund.Item item)
	{
		AsyncHelper.RunSync(async () =>
		{
			await EditProductAsync(item);
		});
	}

	public Task RemoveProductAsync(Guid id, CancellationToken cancellationToken = default(CancellationToken))
	{
		var requestObject = new { id };
		StringBuilder stringBuilder = client.CreateURLBuilder("/api/vat-refund/vat-refund-product-remove");
		return client.ExecuteAsync<VatRefund.Item>(HttpMethod.Post, stringBuilder.ToString(), cancellationToken, requestObject);
	}

	public void RemoveProduct(Guid id)
	{
		AsyncHelper.RunSync(async () =>
		{
			await RemoveProductAsync(id);
		});
	}
}
