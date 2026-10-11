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
using ArmSoft.TaxService.Models.Invoices;
using ArmSoft.TaxService.Models.XML;

namespace ArmSoft.TaxService.Client;

public class InvoiceExciseClient
{
	private readonly TaxServiceClient client;

	private const string apiUrl = "/api/excise";

	internal InvoiceExciseClient(TaxServiceClient client)
	{
		this.client = client;
	}

	public async Task<Excise> GetAsync(Guid id, bool loadItems = false, bool throwException = true, CancellationToken cancellationToken = default(CancellationToken))
	{
		var requestObject = new { id };
		StringBuilder stringBuilder = client.CreateURLBuilder("/api/excise/excise-by-id");
		Excise doc = await client.ExecuteAsync<Excise>(HttpMethod.Post, stringBuilder.ToString(), cancellationToken, requestObject);
		if (loadItems && doc != null)
		{
			Excise excise = doc;
			excise.Items = await GetProductsListAsync(id, cancellationToken);
		}
		if ((doc == null) & throwException)
		{
			throw new ApiException(string.Format(Resources.Get(ResourceKey.DocNotExistWithId), id));
		}
		return doc;
	}

	public Excise Get(Guid id, bool loadItems = false, bool throwException = true)
	{
		return AsyncHelper.RunSync(async () => await GetAsync(id, loadItems, throwException));
	}

	public async Task<List<Excise.Item>> GetProductsListAsync(Guid invoiceId, CancellationToken cancellationToken = default(CancellationToken))
	{
		var requestObject = new { invoiceId };
		StringBuilder stringBuilder = client.CreateURLBuilder("/api/excise/excise-product-by-invoice-id");
		List<Excise.Item> list = await client.ExecuteAsync<List<Excise.Item>>(HttpMethod.Post, stringBuilder.ToString(), cancellationToken, requestObject);
		foreach (Excise.Item item in list.Where((Excise.Item p) => p.ClassifierId.HasValue))
		{
			item.Classifier = client.Classifiers.FirstOrDefault((KeyValuePair<string, Guid> p) => p.Value == item.ClassifierId.Value).Key;
		}
		return list;
	}

	public List<Excise.Item> GetProductsList(Guid invoiceId)
	{
		return AsyncHelper.RunSync(async () => await GetProductsListAsync(invoiceId));
	}

	public Task<object> GetProductCountAsync(Guid invoiceId, CancellationToken cancellationToken = default(CancellationToken))
	{
		var requestObject = new { invoiceId };
		StringBuilder stringBuilder = client.CreateURLBuilder("/api/excise/excise-product-count-by-invoice-id");
		return client.ExecuteAsync<object>(HttpMethod.Post, stringBuilder.ToString(), cancellationToken, requestObject);
	}

	public object GetProductCount(Guid invoiceId)
	{
		return AsyncHelper.RunSync(async () => await GetProductCountAsync(invoiceId));
	}

	public async Task<Excise> GetAsync(string serialNumber, bool loadItems = false, bool throwException = true, CancellationToken cancellationToken = default(CancellationToken))
	{
		var requestObject = new
		{
			serialNo = serialNumber
		};
		StringBuilder stringBuilder = client.CreateURLBuilder("/api/excise/excise-by-serial-no");
		Excise doc = await client.ExecuteAsync<Excise>(HttpMethod.Post, stringBuilder.ToString(), cancellationToken, requestObject);
		if (loadItems && doc != null)
		{
			Excise excise = doc;
			excise.Items = await GetProductsListAsync(doc.Id, cancellationToken);
		}
		if ((doc == null) & throwException)
		{
			throw new ApiException(string.Format(Resources.Get(ResourceKey.DocNotExistWithSerialNumber), serialNumber));
		}
		return doc;
	}

	public Excise Get(string serialNumber, bool loadItems = false, bool throwException = true)
	{
		return AsyncHelper.RunSync(async () => await GetAsync(serialNumber, loadItems, throwException));
	}

	public async Task CreateDraftAsync(Excise doc, bool rewrite = false, bool fillDataFromBase = false, CancellationToken cancellationToken = default(CancellationToken))
	{
		ModelFieldValidator.Validate(doc);
		doc.Source = Source.API;
		if (fillDataFromBase && doc.IsCorrector)
		{
			Excise excise = Get(doc.CorrectorNo, loadItems: true);
			doc.CorrectorWrittenOfAt = excise.IssuedAt;
			doc.BehalfOf = excise.BehalfOf;
			foreach (Excise.Item item in doc.Items)
			{
				Excise.Item item2;
				if (item.ParentId.HasValue)
				{
					item2 = excise.Items.FirstOrDefault((Excise.Item p) =>
					{
						Guid id = p.Id;
						Guid? parentId = item.ParentId;
						return id == parentId;
					});
					if (item2 == null)
					{
						throw new ApiException(string.Format(Resources.Get(ResourceKey.ProductParentIdAbsentInEInvoicing), item.Name));
					}
				}
				else
				{
					IEnumerable<Excise.Item> source = excise.Items.Where((Excise.Item p) => p.Name.Trim() == item.Name.Trim() && p.Unit.Trim() == item.Unit.Trim() && Math.Abs(p.UnitPrice - item.UnitPrice) <= 0.1m);
					if (source.Count() > 1)
					{
						throw new ApiException(Resources.Get(ResourceKey.RepeatedRowsInListNeedToDoFromEInvoicing));
					}
					item2 = source.FirstOrDefault();
					if (item2 == null)
					{
						throw new ApiException(string.Format(Resources.Get(ResourceKey.ProductAbsentInEInvoicing), item.Name, item.Unit, item.UnitPrice));
					}
				}
				item.ClassifierId = item2.ClassifierId;
				item.ParentId = item2.Id;
				item.Name = item2.Name;
				item.Unit = item2.Unit;
				item.UnitPrice = item2.UnitPrice;
				item.Discount = item2.Discount;
				item.VatRate = item2.VatRate;
				item.DealType = item2.DealType;
				if (!item2.ContainerValue.HasValue || item2.ContainerValue.Value == 0m)
				{
					item.ContainerValue = item2.ContainerValue;
				}
				if (!item2.VolumeLiters.HasValue || item2.VolumeLiters.Value == 0m)
				{
					item.VolumeLiters = item2.VolumeLiters;
				}
				if (item2.VatRate != VatRate.Vat20)
				{
					item.VatAmount = item2.VatAmount;
				}
				if (!item2.IncEnvTax.HasValue || item2.IncEnvTax.Value == 0m)
				{
					item.IncEnvTax = item2.IncEnvTax;
				}
			}
			if (!excise.TotalVatAmount.HasValue || excise.TotalVatAmount.Value == 0m)
			{
				doc.TotalVatAmount = excise.TotalVatAmount;
			}
			if (!excise.TotalIncEnvTax.HasValue || excise.TotalIncEnvTax.Value == 0m)
			{
				doc.TotalIncEnvTax = excise.TotalIncEnvTax;
			}
			if (!excise.TotalContainerValue.HasValue || excise.TotalContainerValue.Value == 0m)
			{
				doc.TotalContainerValue = excise.TotalContainerValue;
			}
		}
		if (!doc.IsCorrector || !fillDataFromBase)
		{
			List<Excise.Item> list = doc.Items.Where((Excise.Item p) => !string.IsNullOrWhiteSpace(p.Classifier)).ToList();
			if (list.Count > 0)
			{
				foreach (Excise.Item item3 in list)
				{
					if (client.Classifiers.TryGetValue(item3.Classifier, out var value))
					{
						item3.ClassifierId = value;
						continue;
					}
					throw new ApiException(string.Format(Resources.Get(ResourceKey.WrongClassifier), item3.Name, item3.Classifier));
				}
			}
		}
		if (rewrite)
		{
			Excise excise2 = await GetAsync(doc.Id, loadItems: false, throwException: false, cancellationToken);
			if (excise2 == null)
			{
				await ExciseCreateDraftAsync(doc, cancellationToken);
				return;
			}
			if (excise2.Status != Status.Draft)
			{
				throw new ApiException(string.Format(Resources.Get(ResourceKey.InvoiceIsNotEditable), EncodeDecodeTypes.GetStatusCaption(excise2.Status)));
			}
			await EditDraftAsync(doc, cancellationToken);
		}
		else
		{
			await ExciseCreateDraftAsync(doc, cancellationToken);
		}
	}

	private async Task ExciseCreateDraftAsync(Excise doc, CancellationToken cancellationToken)
	{
		try
		{
			StringBuilder stringBuilder = client.CreateURLBuilder("/api/excise/excise-create-draft");
			await client.ExecuteAsync(HttpMethod.Post, stringBuilder.ToString(), cancellationToken, doc);
		}
		catch (Exception)
		{
			TaxDoc taxDoc = await new TaxDocsClient(client).GetInvoiceAsync(doc.Id, cancellationToken);
			if (taxDoc == null)
			{
				throw;
			}
			if (taxDoc.InvoiceType == InvoiceType.Excise)
			{
				throw new ApiException(string.Format(Resources.Get(ResourceKey.DocAlreadyExistWithDeliveryDateBuyer), doc.DeliveredAt.FormatDDMMYY(), doc.BuyerName, doc.Id));
			}
			string arg = ((taxDoc.InvoiceType == InvoiceType.None) ? EncodeDecodeTypes.GetAccDocCaption(taxDoc.AccDocType) : EncodeDecodeTypes.GetInvoiceCaption(taxDoc.InvoiceType));
			throw new ApiException(string.Format(Resources.Get(ResourceKey.OtherTypeDocAlreadyExist), taxDoc.DeliveredAt.Value.FormatDDMMYY(), arg, doc.Id));
		}
	}

	public void CreateDraft(Excise doc, bool rewrite = false, bool fillDataFromBase = false)
	{
		AsyncHelper.RunSync(async () =>
		{
			await CreateDraftAsync(doc, rewrite, fillDataFromBase);
		});
	}

	public async Task EditDraftAsync(Excise doc, CancellationToken cancellationToken = default(CancellationToken))
	{
		StringBuilder stringBuilder = client.CreateURLBuilder("/api/excise/excise-save-draft");
		await client.ExecuteAsync<Excise>(HttpMethod.Post, stringBuilder.ToString(), cancellationToken, doc);
	}

	public void EditDraft(Excise doc)
	{
		AsyncHelper.RunSync(async () =>
		{
			await EditDraftAsync(doc);
		});
	}

	public Task RemoveDraftAsync(Guid id, CancellationToken cancellationToken = default(CancellationToken))
	{
		var requestObject = new { id };
		StringBuilder stringBuilder = client.CreateURLBuilder("/api/excise/excise-remove-draft");
		return client.ExecuteAsync<Excise>(HttpMethod.Post, stringBuilder.ToString(), cancellationToken, requestObject);
	}

	public void RemoveDraft(Guid id)
	{
		AsyncHelper.RunSync(async () =>
		{
			await RemoveDraftAsync(id);
		});
	}

	public Task CreateProductAsync(Excise.Item item, CancellationToken cancellationToken = default(CancellationToken))
	{
		StringBuilder stringBuilder = client.CreateURLBuilder("/api/excise/excise-product-create");
		return client.ExecuteAsync<Excise.Item>(HttpMethod.Post, stringBuilder.ToString(), cancellationToken, item);
	}

	public void CreateProduct(Excise.Item item)
	{
		AsyncHelper.RunSync(async () =>
		{
			await CreateProductAsync(item);
		});
	}

	public Task EditProductAsync(Excise.Item item, CancellationToken cancellationToken = default(CancellationToken))
	{
		StringBuilder stringBuilder = client.CreateURLBuilder("/api/excise/excise-product-save");
		return client.ExecuteAsync<Excise.Item>(HttpMethod.Post, stringBuilder.ToString(), cancellationToken, item);
	}

	public void EditProduct(Excise.Item item)
	{
		AsyncHelper.RunSync(async () =>
		{
			await EditProductAsync(item);
		});
	}

	public Task RemoveProductAsync(Guid id, CancellationToken cancellationToken = default(CancellationToken))
	{
		var requestObject = new { id };
		StringBuilder stringBuilder = client.CreateURLBuilder("/api/excise/excise-product-remove");
		return client.ExecuteAsync<Excise.Item>(HttpMethod.Post, stringBuilder.ToString(), cancellationToken, requestObject);
	}

	public void RemoveProduct(Guid id)
	{
		AsyncHelper.RunSync(async () =>
		{
			await RemoveProductAsync(id);
		});
	}

	public void CreateXMLFile(Excise doc, string fileFullName, bool append = false)
	{
		XMLExport.CreateInvoiceXMLFile(Converter.ConvertExciseToXMLInvoice(doc), fileFullName, append);
	}

	public void CreateXMLFile(List<Excise> docs, string fileFullName, bool append = false)
	{
		XMLExport.CreateInvoiceXMLFile(Converter.ConvertExciseToXMLInvoice(docs), fileFullName, append);
	}

	public Task UpdateFinalUseAsync(string serialNumber, bool finalUse, CancellationToken cancellationToken = default(CancellationToken))
	{
		var requestObject = new
		{
			serialNo = serialNumber,
			finalUse = finalUse
		};
		StringBuilder stringBuilder = client.CreateURLBuilder("/api/excise/excise-update-final-use");
		return client.ExecuteAsync(HttpMethod.Post, stringBuilder.ToString(), cancellationToken, requestObject);
	}

	public void UpdateFinalUse(string serialNumber, bool finalUse)
	{
		AsyncHelper.RunSync(async () =>
		{
			await UpdateFinalUseAsync(serialNumber, finalUse);
		});
	}

	public async Task<List<ItemRemainder>> GetItemsRemaindersAsync(string serialNumber, CancellationToken cancellationToken = default(CancellationToken))
	{
		var requestObject = new
		{
			serialNo = serialNumber
		};
		StringBuilder stringBuilder = client.CreateURLBuilder("/api/excise/excise-remainder");
		return await client.ExecuteAsync<List<ItemRemainder>>(HttpMethod.Post, stringBuilder.ToString(), cancellationToken, requestObject);
	}

	public List<ItemRemainder> GetItemsRemainders(string serialNumber)
	{
		return AsyncHelper.RunSync(async () => await GetItemsRemaindersAsync(serialNumber));
	}
}
