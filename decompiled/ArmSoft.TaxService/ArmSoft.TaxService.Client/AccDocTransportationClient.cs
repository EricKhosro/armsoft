using System;
using System.Collections.Generic;
using System.Linq;
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

public class AccDocTransportationClient
{
	private readonly TaxServiceClient client;

	private const string apiUrl = "/api/acc-doc-transportation";

	internal AccDocTransportationClient(TaxServiceClient client)
	{
		this.client = client;
	}

	public async Task<AccDocTransportation> GetAsync(Guid id, bool loadItems = false, bool throwException = true, CancellationToken cancellationToken = default(CancellationToken))
	{
		var requestObject = new { id };
		StringBuilder stringBuilder = client.CreateURLBuilder("/api/acc-doc-transportation/acc-doc-transportation-by-id");
		AccDocTransportation doc = await client.ExecuteAsync<AccDocTransportation>(HttpMethod.Post, stringBuilder.ToString(), cancellationToken, requestObject);
		if (loadItems && doc != null)
		{
			AccDocTransportation accDocTransportation = doc;
			accDocTransportation.Items = await GetProductsListAsync(id, cancellationToken);
		}
		if ((doc == null) & throwException)
		{
			throw new ApiException(string.Format(Resources.Get(ResourceKey.DocNotExistWithId), id));
		}
		return doc;
	}

	public AccDocTransportation Get(Guid id, bool loadItems = false, bool throwException = true)
	{
		return AsyncHelper.RunSync(async () => await GetAsync(id, loadItems, throwException));
	}

	public async Task<List<AccDocTransportation.Item>> GetProductsListAsync(Guid invoiceId, CancellationToken cancellationToken = default(CancellationToken))
	{
		var requestObject = new { invoiceId };
		StringBuilder stringBuilder = client.CreateURLBuilder("/api/acc-doc-transportation/acc-doc-transportation-product-by-invoice-id");
		List<AccDocTransportation.Item> list = await client.ExecuteAsync<List<AccDocTransportation.Item>>(HttpMethod.Post, stringBuilder.ToString(), cancellationToken, requestObject);
		foreach (AccDocTransportation.Item item in list.Where((AccDocTransportation.Item p) => p.ClassifierId.HasValue))
		{
			item.Classifier = client.Classifiers.FirstOrDefault((KeyValuePair<string, Guid> p) => p.Value == item.ClassifierId.Value).Key;
		}
		return list;
	}

	public List<AccDocTransportation.Item> GetProductsList(Guid invoiceId)
	{
		return AsyncHelper.RunSync(async () => await GetProductsListAsync(invoiceId));
	}

	public Task<object> GetProductCountAsync(Guid invoiceId, CancellationToken cancellationToken = default(CancellationToken))
	{
		var requestObject = new { invoiceId };
		StringBuilder stringBuilder = client.CreateURLBuilder("/api/acc-doc-transportation/acc-doc-transportation-product-count-by-invoice-id");
		return client.ExecuteAsync<object>(HttpMethod.Post, stringBuilder.ToString(), cancellationToken, requestObject);
	}

	public object GetProductCount(Guid invoiceId)
	{
		return AsyncHelper.RunSync(async () => await GetProductCountAsync(invoiceId));
	}

	public async Task<AccDocTransportation> GetAsync(string serialNumber, bool loadItems = false, bool throwException = true, CancellationToken cancellationToken = default(CancellationToken))
	{
		var requestObject = new
		{
			serialNo = serialNumber
		};
		StringBuilder stringBuilder = client.CreateURLBuilder("/api/acc-doc-transportation/acc-doc-transportation-by-serial-no");
		AccDocTransportation doc = await client.ExecuteAsync<AccDocTransportation>(HttpMethod.Post, stringBuilder.ToString(), cancellationToken, requestObject);
		if (loadItems && doc != null)
		{
			AccDocTransportation accDocTransportation = doc;
			accDocTransportation.Items = await GetProductsListAsync(doc.Id, cancellationToken);
		}
		if ((doc == null) & throwException)
		{
			throw new ApiException(string.Format(Resources.Get(ResourceKey.DocNotExistWithSerialNumber), serialNumber));
		}
		return doc;
	}

	public AccDocTransportation Get(string serialNumber, bool loadItems = false, bool throwException = true)
	{
		return AsyncHelper.RunSync(async () => await GetAsync(serialNumber, loadItems, throwException));
	}

	public async Task CreateDraftAsync(AccDocTransportation doc, bool rewrite = false, CancellationToken cancellationToken = default(CancellationToken))
	{
		ModelFieldValidator.Validate(doc);
		doc.Source = Source.API;
		List<AccDocTransportation.Item> list = doc.Items.Where((AccDocTransportation.Item p) => !string.IsNullOrWhiteSpace(p.Classifier)).ToList();
		if (list.Count > 0)
		{
			foreach (AccDocTransportation.Item item in list)
			{
				if (client.Classifiers.TryGetValue(item.Classifier, out var value))
				{
					item.ClassifierId = value;
					continue;
				}
				throw new ApiException(string.Format(Resources.Get(ResourceKey.WrongClassifier), item.Name, item.Classifier));
			}
		}
		if (rewrite)
		{
			AccDocTransportation accDocTransportation = await GetAsync(doc.Id, loadItems: false, throwException: false, cancellationToken);
			if (accDocTransportation == null)
			{
				await AccDocTransportationCreateDraftAsync(doc, cancellationToken);
				return;
			}
			if (accDocTransportation.Status != Status.Draft)
			{
				throw new ApiException(string.Format(Resources.Get(ResourceKey.InvoiceIsNotEditable), EncodeDecodeTypes.GetStatusCaption(accDocTransportation.Status)));
			}
			await EditDraftAsync(doc, cancellationToken);
		}
		else
		{
			await AccDocTransportationCreateDraftAsync(doc, cancellationToken);
		}
	}

	private async Task AccDocTransportationCreateDraftAsync(AccDocTransportation doc, CancellationToken cancellationToken = default(CancellationToken))
	{
		try
		{
			StringBuilder stringBuilder = client.CreateURLBuilder("/api/acc-doc-transportation/acc-doc-transportation-create-draft");
			await client.ExecuteAsync(HttpMethod.Post, stringBuilder.ToString(), cancellationToken, doc);
		}
		catch (Exception)
		{
			TaxDoc taxDoc = await new TaxDocsClient(client).GetInvoiceAsync(doc.Id, cancellationToken);
			if (taxDoc == null)
			{
				throw;
			}
			if (taxDoc.AccDocType == AccDocType.AccDocTransportation)
			{
				throw new ApiException(string.Format(Resources.Get(ResourceKey.DocAlreadyExistWithId), doc.Id));
			}
			string arg = ((taxDoc.AccDocType == AccDocType.None) ? EncodeDecodeTypes.GetInvoiceCaption(taxDoc.InvoiceType) : EncodeDecodeTypes.GetAccDocCaption(taxDoc.AccDocType));
			throw new ApiException(string.Format(Resources.Get(ResourceKey.OtherTypeDocAlreadyExist), taxDoc.DeliveredAt.Value.FormatDDMMYY(), arg, doc.Id));
		}
	}

	public void CreateDraft(AccDocTransportation doc, bool rewrite = false)
	{
		AsyncHelper.RunSync(async () =>
		{
			await CreateDraftAsync(doc, rewrite);
		});
	}

	public async Task EditDraftAsync(AccDocTransportation doc, CancellationToken cancellationToken = default(CancellationToken))
	{
		StringBuilder stringBuilder = client.CreateURLBuilder("/api/acc-doc-transportation/acc-doc-transportation-save-draft");
		await client.ExecuteAsync<AccDocTransportation>(HttpMethod.Post, stringBuilder.ToString(), cancellationToken, doc);
	}

	public void EditDraft(AccDocTransportation doc)
	{
		AsyncHelper.RunSync(async () =>
		{
			await EditDraftAsync(doc);
		});
	}

	public Task RemoveDraftAsync(Guid id, CancellationToken cancellationToken = default(CancellationToken))
	{
		var requestObject = new { id };
		StringBuilder stringBuilder = client.CreateURLBuilder("/api/acc-doc-transportation/acc-doc-transportation-remove-draft");
		return client.ExecuteAsync<AccDocTransportation>(HttpMethod.Post, stringBuilder.ToString(), cancellationToken, requestObject);
	}

	public void RemoveDraft(Guid id)
	{
		AsyncHelper.RunSync(async () =>
		{
			await RemoveDraftAsync(id);
		});
	}

	public Task CreateProductAsync(AccDocTransportation.Item item, CancellationToken cancellationToken = default(CancellationToken))
	{
		StringBuilder stringBuilder = client.CreateURLBuilder("/api/acc-doc-transportation/acc-doc-transportation-product-create");
		return client.ExecuteAsync<AccDocTransportation.Item>(HttpMethod.Post, stringBuilder.ToString(), cancellationToken, item);
	}

	public void CreateProduct(AccDocTransportation.Item item)
	{
		AsyncHelper.RunSync(async () =>
		{
			await CreateProductAsync(item);
		});
	}

	public Task EditProductAsync(AccDocTransportation.Item item, CancellationToken cancellationToken = default(CancellationToken))
	{
		StringBuilder stringBuilder = client.CreateURLBuilder("/api/acc-doc-transportation/acc-doc-transportation-product-save");
		return client.ExecuteAsync<AccDocTransportation.Item>(HttpMethod.Post, stringBuilder.ToString(), cancellationToken, item);
	}

	public void EditProduct(AccDocTransportation.Item item)
	{
		AsyncHelper.RunSync(async () =>
		{
			await EditProductAsync(item);
		});
	}

	public Task RemoveProductAsync(Guid id, CancellationToken cancellationToken = default(CancellationToken))
	{
		var requestObject = new { id };
		StringBuilder stringBuilder = client.CreateURLBuilder("/api/acc-doc-transportation/acc-doc-transportation-product-remove");
		return client.ExecuteAsync<AccDocTransportation.Item>(HttpMethod.Post, stringBuilder.ToString(), cancellationToken, requestObject);
	}

	public void RemoveProduct(Guid id)
	{
		AsyncHelper.RunSync(async () =>
		{
			await RemoveProductAsync(id);
		});
	}

	public void CreateXMLFile(AccDocTransportation doc, string fileFullName, bool append = false)
	{
		XMLExport.CreateAccDocXMLFile(Converter.ConvertTransportationToXMLAccDoc(doc), fileFullName, append);
	}

	public void CreateXMLFile(List<AccDocTransportation> docs, string fileFullName, bool append = false)
	{
		XMLExport.CreateAccDocXMLFile(Converter.ConvertTransportationToXMLAccDoc(docs), fileFullName, append);
	}
}
