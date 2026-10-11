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
using ArmSoft.TaxService.Models.XML;

namespace ArmSoft.TaxService.Client;

public class AccDocServicesClient
{
	private readonly TaxServiceClient client;

	private const string apiUrl = "/api/acc-doc-services";

	internal AccDocServicesClient(TaxServiceClient client)
	{
		this.client = client;
	}

	public async Task<AccDocServices> GetAsync(Guid id, bool loadItems = false, bool throwException = true, CancellationToken cancellationToken = default(CancellationToken))
	{
		var requestObject = new { id };
		StringBuilder stringBuilder = client.CreateURLBuilder("/api/acc-doc-services/acc-doc-services-by-id");
		AccDocServices doc = await client.ExecuteAsync<AccDocServices>(HttpMethod.Post, stringBuilder.ToString(), cancellationToken, requestObject);
		if (loadItems && doc != null)
		{
			AccDocServices accDocServices = doc;
			accDocServices.Items = await GetProductsListAsync(id, cancellationToken);
		}
		if ((doc == null) & throwException)
		{
			throw new ApiException(string.Format(Resources.Get(ResourceKey.DocNotExistWithId), id));
		}
		return doc;
	}

	public AccDocServices Get(Guid id, bool loadItems = false, bool throwException = true)
	{
		return AsyncHelper.RunSync(async () => await GetAsync(id, loadItems, throwException));
	}

	public Task<List<AccDocServices.Item>> GetProductsListAsync(Guid invoiceId, CancellationToken cancellationToken = default(CancellationToken))
	{
		var requestObject = new { invoiceId };
		StringBuilder stringBuilder = client.CreateURLBuilder("/api/acc-doc-services/acc-doc-services-product-by-invoice-id");
		return client.ExecuteAsync<List<AccDocServices.Item>>(HttpMethod.Post, stringBuilder.ToString(), cancellationToken, requestObject);
	}

	public List<AccDocServices.Item> GetProductsList(Guid invoiceId)
	{
		return AsyncHelper.RunSync(async () => await GetProductsListAsync(invoiceId));
	}

	public Task<object> GetProductCountAsync(Guid invoiceId, CancellationToken cancellationToken = default(CancellationToken))
	{
		var requestObject = new { invoiceId };
		StringBuilder stringBuilder = client.CreateURLBuilder("/api/acc-doc-services/acc-doc-services-product-count-by-invoice-id");
		return client.ExecuteAsync<object>(HttpMethod.Post, stringBuilder.ToString(), cancellationToken, requestObject);
	}

	public object GetProductCount(Guid invoiceId)
	{
		return AsyncHelper.RunSync(async () => await GetProductCountAsync(invoiceId));
	}

	public async Task<AccDocServices> GetAsync(string serialNumber, bool loadItems = false, bool throwException = true, CancellationToken cancellationToken = default(CancellationToken))
	{
		var requestObject = new
		{
			serialNo = serialNumber
		};
		StringBuilder stringBuilder = client.CreateURLBuilder("/api/acc-doc-services/acc-doc-services-by-serial-no");
		AccDocServices doc = await client.ExecuteAsync<AccDocServices>(HttpMethod.Post, stringBuilder.ToString(), cancellationToken, requestObject);
		if (loadItems && doc != null)
		{
			AccDocServices accDocServices = doc;
			accDocServices.Items = await GetProductsListAsync(doc.Id, cancellationToken);
		}
		if ((doc == null) & throwException)
		{
			throw new ApiException(string.Format(Resources.Get(ResourceKey.DocNotExistWithSerialNumber), serialNumber));
		}
		return doc;
	}

	public AccDocServices Get(string serialNumber, bool loadItems = false, bool throwException = true)
	{
		return AsyncHelper.RunSync(async () => await GetAsync(serialNumber, loadItems, throwException));
	}

	public async Task CreateDraftAsync(AccDocServices doc, bool rewrite = false, CancellationToken cancellationToken = default(CancellationToken))
	{
		ModelFieldValidator.Validate(doc);
		doc.Source = Source.API;
		if (doc.InsuringEmployer)
		{
			doc.BuyerHasNoTin = false;
			doc.BuyerIsNatural = false;
			doc.CitizenArmenia = false;
			doc.BuyerSsn = "";
			doc.BuyerIdentityType = ArmSoft.TaxService.Enums.IdentityType.None;
			doc.BuyerIdentityNo = "";
			doc.SocialCostType = ArmSoft.TaxService.Enums.SocialCostType.HealthInsurance;
		}
		if (rewrite)
		{
			AccDocServices accDocServices = await GetAsync(doc.Id, loadItems: false, throwException: false, cancellationToken);
			if (accDocServices == null)
			{
				await AccDocServicesCreateDraftAsync(doc, cancellationToken);
				return;
			}
			if (accDocServices.Status != Status.Draft)
			{
				throw new ApiException(string.Format(Resources.Get(ResourceKey.InvoiceIsNotEditable), EncodeDecodeTypes.GetStatusCaption(accDocServices.Status)));
			}
			await EditDraftAsync(doc, cancellationToken);
		}
		else
		{
			await AccDocServicesCreateDraftAsync(doc, cancellationToken);
		}
	}

	private async Task AccDocServicesCreateDraftAsync(AccDocServices doc, CancellationToken cancellationToken = default(CancellationToken))
	{
		try
		{
			StringBuilder stringBuilder = client.CreateURLBuilder("/api/acc-doc-services/acc-doc-services-create-draft");
			await client.ExecuteAsync(HttpMethod.Post, stringBuilder.ToString(), cancellationToken, doc);
		}
		catch (Exception)
		{
			TaxDoc taxDoc = await new TaxDocsClient(client).GetInvoiceAsync(doc.Id, cancellationToken);
			if (taxDoc == null)
			{
				throw;
			}
			if (taxDoc.AccDocType == AccDocType.AccDocServices)
			{
				throw new ApiException(string.Format(Resources.Get(ResourceKey.DocAlreadyExistWithDeliveryDateBuyer), doc.DeliveredAt.FormatDDMMYY(), doc.BuyerName, doc.Id));
			}
			string arg = ((taxDoc.AccDocType == AccDocType.None) ? EncodeDecodeTypes.GetInvoiceCaption(taxDoc.InvoiceType) : EncodeDecodeTypes.GetAccDocCaption(taxDoc.AccDocType));
			throw new ApiException(string.Format(Resources.Get(ResourceKey.OtherTypeDocAlreadyExist), taxDoc.DeliveredAt.Value.FormatDDMMYY(), arg, doc.Id));
		}
	}

	public void CreateDraft(AccDocServices doc, bool rewrite = false)
	{
		AsyncHelper.RunSync(async () =>
		{
			await CreateDraftAsync(doc, rewrite);
		});
	}

	public async Task EditDraftAsync(AccDocServices doc, CancellationToken cancellationToken = default(CancellationToken))
	{
		StringBuilder stringBuilder = client.CreateURLBuilder("/api/acc-doc-services/acc-doc-services-save-draft");
		await client.ExecuteAsync<AccDocServices>(HttpMethod.Post, stringBuilder.ToString(), cancellationToken, doc);
	}

	public void EditDraft(AccDocServices doc)
	{
		AsyncHelper.RunSync(async () =>
		{
			await EditDraftAsync(doc);
		});
	}

	public Task RemoveDraftAsync(Guid id, CancellationToken cancellationToken = default(CancellationToken))
	{
		var requestObject = new { id };
		StringBuilder stringBuilder = client.CreateURLBuilder("/api/acc-doc-services/acc-doc-services-remove-draft");
		return client.ExecuteAsync<AccDocServices>(HttpMethod.Post, stringBuilder.ToString(), cancellationToken, requestObject);
	}

	public void RemoveDraft(Guid id)
	{
		AsyncHelper.RunSync(async () =>
		{
			await RemoveDraftAsync(id);
		});
	}

	public Task CreateProductAsync(AccDocServices.Item item, CancellationToken cancellationToken = default(CancellationToken))
	{
		StringBuilder stringBuilder = client.CreateURLBuilder("/api/acc-doc-services/acc-doc-services-product-create");
		return client.ExecuteAsync<AccDocServices.Item>(HttpMethod.Post, stringBuilder.ToString(), cancellationToken, item);
	}

	public void CreateProduct(AccDocServices.Item item)
	{
		AsyncHelper.RunSync(async () =>
		{
			await CreateProductAsync(item);
		});
	}

	public Task EditProductAsync(AccDocServices.Item item, CancellationToken cancellationToken = default(CancellationToken))
	{
		StringBuilder stringBuilder = client.CreateURLBuilder("/api/acc-doc-services/acc-doc-services-product-save");
		return client.ExecuteAsync<AccDocServices.Item>(HttpMethod.Post, stringBuilder.ToString(), cancellationToken, item);
	}

	public void EditProduct(AccDocServices.Item item)
	{
		AsyncHelper.RunSync(async () =>
		{
			await EditProductAsync(item);
		});
	}

	public Task RemoveProductAsync(Guid id, CancellationToken cancellationToken = default(CancellationToken))
	{
		var requestObject = new { id };
		StringBuilder stringBuilder = client.CreateURLBuilder("/api/acc-doc-services/acc-doc-services-product-remove");
		return client.ExecuteAsync<AccDocServices.Item>(HttpMethod.Post, stringBuilder.ToString(), cancellationToken, requestObject);
	}

	public void RemoveProduct(Guid id)
	{
		AsyncHelper.RunSync(async () =>
		{
			await RemoveProductAsync(id);
		});
	}

	public void CreateXMLFile(AccDocServices doc, string fileFullName, bool append = false)
	{
		XMLExport.CreateAccDocXMLFile(Converter.ConvertServiceToXMLAccDoc(doc), fileFullName, append);
	}

	public void CreateXMLFile(List<AccDocServices> docs, string fileFullName, bool append = false)
	{
		XMLExport.CreateAccDocXMLFile(Converter.ConvertServiceToXMLAccDoc(docs), fileFullName, append);
	}

	public async Task<List<ItemRemainder>> GetItemsRemaindersAsync(string serialNumber, CancellationToken cancellationToken = default(CancellationToken))
	{
		var requestObject = new
		{
			serialNo = serialNumber
		};
		StringBuilder stringBuilder = client.CreateURLBuilder("/api/acc-doc-services/acc-doc-services-remainder");
		return await client.ExecuteAsync<List<ItemRemainder>>(HttpMethod.Post, stringBuilder.ToString(), cancellationToken, requestObject);
	}

	public List<ItemRemainder> GetItemsRemainders(string serialNumber)
	{
		return AsyncHelper.RunSync(async () => await GetItemsRemaindersAsync(serialNumber));
	}
}
