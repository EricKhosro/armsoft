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
using ArmSoft.TaxService.Models.XML;

namespace ArmSoft.TaxService.Client;

public class InvoiceLeasingClient
{
	private readonly TaxServiceClient client;

	private const string apiUrl = "/api/leasing-act";

	internal InvoiceLeasingClient(TaxServiceClient client)
	{
		this.client = client;
	}

	public async Task<Leasing> GetAsync(Guid id, bool loadItems = false, bool throwException = true, CancellationToken cancellationToken = default(CancellationToken))
	{
		var requestObject = new { id };
		StringBuilder stringBuilder = client.CreateURLBuilder("/api/leasing-act/leasing-act-by-id");
		Leasing doc = await client.ExecuteAsync<Leasing>(HttpMethod.Post, stringBuilder.ToString(), cancellationToken, requestObject);
		if (loadItems && doc != null)
		{
			Leasing leasing = doc;
			leasing.Items = await GetProductsListAsync(id, cancellationToken);
		}
		if ((doc == null) & throwException)
		{
			throw new ApiException(string.Format(Resources.Get(ResourceKey.DocNotExistWithId), id));
		}
		return doc;
	}

	public Leasing Get(Guid id, bool loadItems = false, bool throwException = true)
	{
		return AsyncHelper.RunSync(async () => await GetAsync(id, loadItems, throwException));
	}

	public Task<List<Leasing.Item>> GetProductsListAsync(Guid invoiceId, CancellationToken cancellationToken = default(CancellationToken))
	{
		var requestObject = new { invoiceId };
		StringBuilder stringBuilder = client.CreateURLBuilder("/api/leasing-act/leasing-act-subject-by-invoice-id");
		return client.ExecuteAsync<List<Leasing.Item>>(HttpMethod.Post, stringBuilder.ToString(), cancellationToken, requestObject);
	}

	public List<Leasing.Item> GetProductsList(Guid invoiceId)
	{
		return AsyncHelper.RunSync(async () => await GetProductsListAsync(invoiceId));
	}

	public Task<object> GetProductCountAsync(Guid invoiceId, CancellationToken cancellationToken = default(CancellationToken))
	{
		var requestObject = new { invoiceId };
		StringBuilder stringBuilder = client.CreateURLBuilder("/api/leasing-act/leasing-act-subject-count-by-invoice-id");
		return client.ExecuteAsync<object>(HttpMethod.Post, stringBuilder.ToString(), cancellationToken, requestObject);
	}

	public object GetProductCount(Guid invoiceId)
	{
		return AsyncHelper.RunSync(async () => await GetProductCountAsync(invoiceId));
	}

	public async Task<Leasing> GetAsync(string serialNumber, bool loadItems = false, bool throwException = true, CancellationToken cancellationToken = default(CancellationToken))
	{
		var requestObject = new
		{
			serialNo = serialNumber
		};
		StringBuilder stringBuilder = client.CreateURLBuilder("/api/leasing-act/leasing-act-by-serial-no");
		Leasing doc = await client.ExecuteAsync<Leasing>(HttpMethod.Post, stringBuilder.ToString(), cancellationToken, requestObject);
		if (loadItems && doc != null)
		{
			Leasing leasing = doc;
			leasing.Items = await GetProductsListAsync(doc.Id, cancellationToken);
		}
		if ((doc == null) & throwException)
		{
			throw new ApiException(string.Format(Resources.Get(ResourceKey.DocNotExistWithSerialNumber), serialNumber));
		}
		return doc;
	}

	public Leasing Get(string serialNumber, bool loadItems = false, bool throwException = true)
	{
		return AsyncHelper.RunSync(async () => await GetAsync(serialNumber, loadItems, throwException));
	}

	public async Task CreateDraftAsync(Leasing doc, bool rewrite = false, CancellationToken cancellationToken = default(CancellationToken))
	{
		ModelFieldValidator.Validate(doc);
		doc.Source = Source.API;
		if (rewrite)
		{
			Leasing leasing = await GetAsync(doc.Id, loadItems: false, throwException: false, cancellationToken);
			if (leasing == null)
			{
				await CreateDraftAsync(doc, cancellationToken);
				return;
			}
			if (leasing.Status != Status.Draft)
			{
				throw new ApiException(string.Format(Resources.Get(ResourceKey.InvoiceIsNotEditable), EncodeDecodeTypes.GetStatusCaption(leasing.Status)));
			}
			await EditDraftAsync(doc, cancellationToken);
		}
		else
		{
			await CreateDraftAsync(doc, cancellationToken);
		}
	}

	private async Task CreateDraftAsync(Leasing doc, CancellationToken cancellationToken)
	{
		try
		{
			StringBuilder stringBuilder = client.CreateURLBuilder("/api/leasing-act/leasing-act-create-draft");
			await client.ExecuteAsync(HttpMethod.Post, stringBuilder.ToString(), cancellationToken, doc);
		}
		catch (Exception)
		{
			TaxDoc taxDoc = await new TaxDocsClient(client).GetInvoiceAsync(doc.Id, cancellationToken);
			if (taxDoc == null)
			{
				throw;
			}
			if (taxDoc.InvoiceType == InvoiceType.Leasing)
			{
				throw new ApiException(string.Format(Resources.Get(ResourceKey.DocAlreadyExistWithDeliveryDateBuyer), doc.DeliveredAt.FormatDDMMYY(), doc.LesseeName, doc.Id));
			}
			string arg = ((taxDoc.InvoiceType == InvoiceType.None) ? EncodeDecodeTypes.GetAccDocCaption(taxDoc.AccDocType) : EncodeDecodeTypes.GetInvoiceCaption(taxDoc.InvoiceType));
			throw new ApiException(string.Format(Resources.Get(ResourceKey.OtherTypeDocAlreadyExist), taxDoc.DeliveredAt.Value.FormatDDMMYY(), arg, doc.Id));
		}
	}

	public void CreateDraft(Leasing doc, bool rewrite = false)
	{
		AsyncHelper.RunSync(async () =>
		{
			await CreateDraftAsync(doc, rewrite);
		});
	}

	public async Task EditDraftAsync(Leasing doc, CancellationToken cancellationToken = default(CancellationToken))
	{
		StringBuilder stringBuilder = client.CreateURLBuilder("/api/leasing-act/leasing-act-save-draft");
		await client.ExecuteAsync<Excise>(HttpMethod.Post, stringBuilder.ToString(), cancellationToken, doc);
	}

	public void EditDraft(Leasing doc)
	{
		AsyncHelper.RunSync(async () =>
		{
			await EditDraftAsync(doc);
		});
	}

	public Task RemoveDraftAsync(Guid id, CancellationToken cancellationToken = default(CancellationToken))
	{
		var requestObject = new { id };
		StringBuilder stringBuilder = client.CreateURLBuilder("/api/leasing-act/leasing-act-remove-draft");
		return client.ExecuteAsync<Leasing>(HttpMethod.Post, stringBuilder.ToString(), cancellationToken, requestObject);
	}

	public void RemoveDraft(Guid id)
	{
		AsyncHelper.RunSync(async () =>
		{
			await RemoveDraftAsync(id);
		});
	}

	public Task CreateProductAsync(Leasing.Item item, CancellationToken cancellationToken = default(CancellationToken))
	{
		StringBuilder stringBuilder = client.CreateURLBuilder("/api/leasing-act/leasing-act-subject-create");
		return client.ExecuteAsync<Leasing.Item>(HttpMethod.Post, stringBuilder.ToString(), cancellationToken, item);
	}

	public void CreateProduct(Leasing.Item item)
	{
		AsyncHelper.RunSync(async () =>
		{
			await CreateProductAsync(item);
		});
	}

	public Task EditProductAsync(Leasing.Item item, CancellationToken cancellationToken = default(CancellationToken))
	{
		StringBuilder stringBuilder = client.CreateURLBuilder("/api/leasing-act/leasing-act-subject-save");
		return client.ExecuteAsync<Leasing.Item>(HttpMethod.Post, stringBuilder.ToString(), cancellationToken, item);
	}

	public void EditProduct(Leasing.Item item)
	{
		AsyncHelper.RunSync(async () =>
		{
			await EditProductAsync(item);
		});
	}

	public Task RemoveProductAsync(Guid id, CancellationToken cancellationToken = default(CancellationToken))
	{
		var requestObject = new { id };
		StringBuilder stringBuilder = client.CreateURLBuilder("/api/leasing-act/leasing-act-subject-remove");
		return client.ExecuteAsync<Excise.Item>(HttpMethod.Post, stringBuilder.ToString(), cancellationToken, requestObject);
	}

	public void RemoveProduct(Guid id)
	{
		AsyncHelper.RunSync(async () =>
		{
			await RemoveProductAsync(id);
		});
	}

	public void CreateXMLFile(Leasing doc, string url, bool append = false)
	{
		XMLExport.CreateInvoiceXMLFile(Converter.ConvertLeasingToXMLInvoice(doc), url, append);
	}

	public void CreateXMLFile(List<Leasing> docs, string url, bool append = false)
	{
		XMLExport.CreateInvoiceXMLFile(Converter.ConvertLeasingToXMLInvoice(docs), url, append);
	}
}
