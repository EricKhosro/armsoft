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

public class AccDocGoodsClient
{
	private readonly TaxServiceClient client;

	private const string apiUrl = "/api/acc-doc-goods";

	internal AccDocGoodsClient(TaxServiceClient client)
	{
		this.client = client;
	}

	public async Task<AccDocGoods> GetAsync(Guid id, bool loadItems = false, bool throwException = true, CancellationToken cancellationToken = default(CancellationToken))
	{
		var requestObject = new { id };
		StringBuilder stringBuilder = client.CreateURLBuilder("/api/acc-doc-goods/acc-doc-goods-by-id");
		AccDocGoods doc = await client.ExecuteAsync<AccDocGoods>(HttpMethod.Post, stringBuilder.ToString(), cancellationToken, requestObject);
		if (loadItems && doc != null)
		{
			AccDocGoods accDocGoods = doc;
			accDocGoods.Items = await GetProductsListAsync(id, cancellationToken);
		}
		if ((doc == null) & throwException)
		{
			throw new ApiException(string.Format(Resources.Get(ResourceKey.DocNotExistWithId), id));
		}
		return doc;
	}

	public AccDocGoods Get(Guid id, bool loadItems = false, bool throwException = true)
	{
		return AsyncHelper.RunSync(async () => await GetAsync(id, loadItems, throwException));
	}

	public async Task<List<AccDocGoods.Item>> GetProductsListAsync(Guid invoiceId, CancellationToken cancellationToken = default(CancellationToken))
	{
		var requestObject = new { invoiceId };
		StringBuilder stringBuilder = client.CreateURLBuilder("/api/acc-doc-goods/acc-doc-goods-product-by-invoice-id");
		List<AccDocGoods.Item> list = await client.ExecuteAsync<List<AccDocGoods.Item>>(HttpMethod.Post, stringBuilder.ToString(), cancellationToken, requestObject);
		foreach (AccDocGoods.Item item in list.Where((AccDocGoods.Item p) => p.ClassifierId.HasValue))
		{
			item.Classifier = client.Classifiers.FirstOrDefault((KeyValuePair<string, Guid> p) => p.Value == item.ClassifierId.Value).Key;
		}
		return list;
	}

	public List<AccDocGoods.Item> GetProductsList(Guid invoiceId)
	{
		return AsyncHelper.RunSync(async () => await GetProductsListAsync(invoiceId));
	}

	public Task<object> GetProductCountAsync(Guid invoiceId, CancellationToken cancellationToken = default(CancellationToken))
	{
		var requestObject = new { invoiceId };
		StringBuilder stringBuilder = client.CreateURLBuilder("/api/acc-doc-goods/acc-doc-goods-product-count-by-invoice-id");
		return client.ExecuteAsync<object>(HttpMethod.Post, stringBuilder.ToString(), cancellationToken, requestObject);
	}

	public object GetProductCount(Guid invoiceId)
	{
		return AsyncHelper.RunSync(async () => await GetProductCountAsync(invoiceId));
	}

	public async Task<AccDocGoods> GetAsync(string serialNumber, bool loadItems = false, bool throwException = true, CancellationToken cancellationToken = default(CancellationToken))
	{
		var requestObject = new
		{
			serialNo = serialNumber
		};
		StringBuilder stringBuilder = client.CreateURLBuilder("/api/acc-doc-goods/acc-doc-goods-by-serial-no");
		AccDocGoods doc = await client.ExecuteAsync<AccDocGoods>(HttpMethod.Post, stringBuilder.ToString(), cancellationToken, requestObject);
		if (loadItems && doc != null)
		{
			AccDocGoods accDocGoods = doc;
			accDocGoods.Items = await GetProductsListAsync(doc.Id, cancellationToken);
		}
		if ((doc == null) & throwException)
		{
			throw new ApiException(string.Format(Resources.Get(ResourceKey.DocNotExistWithSerialNumber), serialNumber));
		}
		return doc;
	}

	public AccDocGoods Get(string serialNumber, bool loadItems = false, bool throwException = true)
	{
		return AsyncHelper.RunSync(async () => await GetAsync(serialNumber, loadItems, throwException));
	}

	public async Task CreateDraftAsync(AccDocGoods doc, bool rewrite = false, bool fillDataFromBase = false, CancellationToken cancellationToken = default(CancellationToken))
	{
		ModelFieldValidator.Validate(doc);
		doc.Source = Source.API;
		if (fillDataFromBase && doc.IsCorrector)
		{
			AccDocGoods accDocGoods = Get(doc.CorrectorNo, loadItems: true);
			doc.CorrectorWrittenOfAt = accDocGoods.IssuedAt;
			doc.BehalfOf = accDocGoods.BehalfOf;
			foreach (AccDocGoods.Item item in doc.Items)
			{
				AccDocGoods.Item item2;
				if (item.ParentId.HasValue)
				{
					item2 = accDocGoods.Items.FirstOrDefault((AccDocGoods.Item p) =>
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
					IEnumerable<AccDocGoods.Item> source = accDocGoods.Items.Where((AccDocGoods.Item p) => p.Name.Trim() == item.Name.Trim() && p.Unit.Trim() == item.Unit.Trim() && Math.Abs(p.UnitPrice - item.UnitPrice) <= 0.1m);
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
				item.DealType = item2.DealType;
				if (!item2.ContainerValue.HasValue || item2.ContainerValue.Value == 0m)
				{
					item.ContainerValue = item2.ContainerValue;
				}
				if (!item2.ExcTaxAmount.HasValue || item2.ExcTaxAmount.Value == 0m)
				{
					item.ExcTaxAmount = item2.ExcTaxAmount;
				}
				if (!item2.IncEnvTax.HasValue || item2.IncEnvTax.Value == 0m)
				{
					item.IncEnvTax = item2.IncEnvTax;
				}
			}
			if (!accDocGoods.TotalExcTaxAmount.HasValue || accDocGoods.TotalExcTaxAmount.Value == 0m)
			{
				doc.TotalExcTaxAmount = accDocGoods.TotalExcTaxAmount;
			}
			if (!accDocGoods.TotalIncEnvTax.HasValue || accDocGoods.TotalIncEnvTax.Value == 0m)
			{
				doc.TotalIncEnvTax = accDocGoods.TotalIncEnvTax;
			}
			if (!accDocGoods.TotalContainerValue.HasValue || accDocGoods.TotalContainerValue.Value == 0m)
			{
				doc.TotalContainerValue = accDocGoods.TotalContainerValue;
			}
		}
		if (!doc.IsCorrector || !fillDataFromBase)
		{
			List<AccDocGoods.Item> list = doc.Items.Where((AccDocGoods.Item p) => !string.IsNullOrWhiteSpace(p.Classifier)).ToList();
			if (list.Count > 0)
			{
				foreach (AccDocGoods.Item item3 in list)
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
			AccDocGoods accDocGoods2 = await GetAsync(doc.Id, loadItems: false, throwException: false, cancellationToken);
			if (accDocGoods2 == null)
			{
				await AccDocGoodsCreateDraftAsync(doc, cancellationToken);
				return;
			}
			if (accDocGoods2.Status != Status.Draft)
			{
				throw new ApiException(string.Format(Resources.Get(ResourceKey.InvoiceIsNotEditable), EncodeDecodeTypes.GetStatusCaption(accDocGoods2.Status)));
			}
			await EditDraftAsync(doc, cancellationToken);
		}
		else
		{
			await AccDocGoodsCreateDraftAsync(doc, cancellationToken);
		}
	}

	private async Task AccDocGoodsCreateDraftAsync(AccDocGoods doc, CancellationToken cancellationToken = default(CancellationToken))
	{
		try
		{
			StringBuilder stringBuilder = client.CreateURLBuilder("/api/acc-doc-goods/acc-doc-goods-create-draft");
			await client.ExecuteAsync(HttpMethod.Post, stringBuilder.ToString(), cancellationToken, doc);
		}
		catch (Exception)
		{
			TaxDoc taxDoc = await new TaxDocsClient(client).GetInvoiceAsync(doc.Id, cancellationToken);
			if (taxDoc == null)
			{
				throw;
			}
			if (taxDoc.AccDocType == AccDocType.AccDocGoods)
			{
				throw new ApiException(string.Format(Resources.Get(ResourceKey.DocAlreadyExistWithDeliveryDateBuyer), doc.DeliveredAt.FormatDDMMYY(), doc.BuyerName, doc.Id));
			}
			string arg = ((taxDoc.AccDocType == AccDocType.None) ? EncodeDecodeTypes.GetInvoiceCaption(taxDoc.InvoiceType) : EncodeDecodeTypes.GetAccDocCaption(taxDoc.AccDocType));
			throw new ApiException(string.Format(Resources.Get(ResourceKey.OtherTypeDocAlreadyExist), taxDoc.DeliveredAt.Value.FormatDDMMYY(), arg, doc.Id));
		}
	}

	public void CreateDraft(AccDocGoods doc, bool rewrite = false, bool fillDataFromBase = false)
	{
		AsyncHelper.RunSync(async () =>
		{
			await CreateDraftAsync(doc, rewrite, fillDataFromBase);
		});
	}

	public async Task EditDraftAsync(AccDocGoods doc, CancellationToken cancellationToken = default(CancellationToken))
	{
		StringBuilder stringBuilder = client.CreateURLBuilder("/api/acc-doc-goods/acc-doc-goods-save-draft");
		await client.ExecuteAsync<AccDocGoods>(HttpMethod.Post, stringBuilder.ToString(), cancellationToken, doc);
	}

	public void EditDraft(AccDocGoods doc)
	{
		AsyncHelper.RunSync(async () =>
		{
			await EditDraftAsync(doc);
		});
	}

	public Task RemoveDraftAsync(Guid id, CancellationToken cancellationToken = default(CancellationToken))
	{
		var requestObject = new { id };
		StringBuilder stringBuilder = client.CreateURLBuilder("/api/acc-doc-goods/acc-doc-goods-remove-draft");
		return client.ExecuteAsync<AccDocGoods>(HttpMethod.Post, stringBuilder.ToString(), cancellationToken, requestObject);
	}

	public void RemoveDraft(Guid id)
	{
		AsyncHelper.RunSync(async () =>
		{
			await RemoveDraftAsync(id);
		});
	}

	public Task CreateProductAsync(AccDocGoods.Item item, CancellationToken cancellationToken = default(CancellationToken))
	{
		StringBuilder stringBuilder = client.CreateURLBuilder("/api/acc-doc-goods/acc-doc-goods-product-create");
		return client.ExecuteAsync<AccDocGoods.Item>(HttpMethod.Post, stringBuilder.ToString(), cancellationToken, item);
	}

	public void CreateProduct(AccDocGoods.Item item)
	{
		AsyncHelper.RunSync(async () =>
		{
			await CreateProductAsync(item);
		});
	}

	public Task EditProductAsync(AccDocGoods.Item item, CancellationToken cancellationToken = default(CancellationToken))
	{
		StringBuilder stringBuilder = client.CreateURLBuilder("/api/acc-doc-goods/acc-doc-goods-product-save");
		return client.ExecuteAsync<AccDocGoods.Item>(HttpMethod.Post, stringBuilder.ToString(), cancellationToken, item);
	}

	public void EditProduct(AccDocGoods.Item item)
	{
		AsyncHelper.RunSync(async () =>
		{
			await EditProductAsync(item);
		});
	}

	public Task RemoveProductAsync(Guid id, CancellationToken cancellationToken = default(CancellationToken))
	{
		var requestObject = new { id };
		StringBuilder stringBuilder = client.CreateURLBuilder("/api/acc-doc-goods/acc-doc-goods-product-remove");
		return client.ExecuteAsync<AccDocGoods.Item>(HttpMethod.Post, stringBuilder.ToString(), cancellationToken, requestObject);
	}

	public void RemoveProduct(Guid id)
	{
		AsyncHelper.RunSync(async () =>
		{
			await RemoveProductAsync(id);
		});
	}

	public void CreateXMLFile(AccDocGoods doc, string fileFullName, bool append = false)
	{
		XMLExport.CreateAccDocXMLFile(Converter.ConvertGoodToXMLAccDoc(doc), fileFullName, append);
	}

	public void CreateXMLFile(List<AccDocGoods> docs, string fileFullName, bool append = false)
	{
		XMLExport.CreateAccDocXMLFile(Converter.ConvertGoodToXMLAccDoc(docs), fileFullName, append);
	}

	public Task UpdateFinalUseAsync(string serialNumber, bool finalUse, CancellationToken cancellationToken = default(CancellationToken))
	{
		var requestObject = new
		{
			serialNo = serialNumber,
			finalUse = finalUse
		};
		StringBuilder stringBuilder = client.CreateURLBuilder("/api/acc-doc-goods/acc-doc-goods-update-final-use");
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
		StringBuilder stringBuilder = client.CreateURLBuilder("/api/acc-doc-goods/acc-doc-goods-remainder");
		return await client.ExecuteAsync<List<ItemRemainder>>(HttpMethod.Post, stringBuilder.ToString(), cancellationToken, requestObject);
	}

	public List<ItemRemainder> GetItemsRemainders(string serialNumber)
	{
		return AsyncHelper.RunSync(async () => await GetItemsRemaindersAsync(serialNumber));
	}
}
