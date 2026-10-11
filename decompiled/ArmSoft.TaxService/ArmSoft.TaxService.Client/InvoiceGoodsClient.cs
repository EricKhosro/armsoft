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

public class InvoiceGoodsClient
{
	private readonly TaxServiceClient client;

	private const string apiUrl = "/api/goods";

	internal InvoiceGoodsClient(TaxServiceClient client)
	{
		this.client = client;
	}

	public async Task<ArmSoft.TaxService.Models.Invoices.Good> GetAsync(Guid id, bool loadItems = false, bool throwException = true, CancellationToken cancellationToken = default(CancellationToken))
	{
		var requestObject = new { id };
		StringBuilder stringBuilder = client.CreateURLBuilder("/api/goods/goods-by-id");
		ArmSoft.TaxService.Models.Invoices.Good doc = await client.ExecuteAsync<ArmSoft.TaxService.Models.Invoices.Good>(HttpMethod.Post, stringBuilder.ToString(), cancellationToken, requestObject);
		if (loadItems && doc != null)
		{
			ArmSoft.TaxService.Models.Invoices.Good good = doc;
			good.Items = await GetProductsListAsync(id, cancellationToken);
		}
		if ((doc == null) & throwException)
		{
			throw new ApiException(string.Format(Resources.Get(ResourceKey.DocNotExistWithId), id));
		}
		return doc;
	}

	public ArmSoft.TaxService.Models.Invoices.Good Get(Guid id, bool loadItems = false, bool throwException = true)
	{
		return AsyncHelper.RunSync(async () => await GetAsync(id, loadItems, throwException));
	}

	public async Task<List<ArmSoft.TaxService.Models.Invoices.Good.Item>> GetProductsListAsync(Guid invoiceId, CancellationToken cancellationToken = default(CancellationToken))
	{
		var requestObject = new { invoiceId };
		StringBuilder stringBuilder = client.CreateURLBuilder("/api/goods/goods-product-by-invoice-id");
		List<ArmSoft.TaxService.Models.Invoices.Good.Item> list = await client.ExecuteAsync<List<ArmSoft.TaxService.Models.Invoices.Good.Item>>(HttpMethod.Post, stringBuilder.ToString(), cancellationToken, requestObject);
		foreach (ArmSoft.TaxService.Models.Invoices.Good.Item item in list.Where((ArmSoft.TaxService.Models.Invoices.Good.Item p) => p.ClassifierId.HasValue))
		{
			item.Classifier = client.Classifiers.FirstOrDefault((KeyValuePair<string, Guid> p) => p.Value == item.ClassifierId.Value).Key;
		}
		return list;
	}

	public List<ArmSoft.TaxService.Models.Invoices.Good.Item> GetProductsList(Guid invoiceId)
	{
		return AsyncHelper.RunSync(async () => await GetProductsListAsync(invoiceId));
	}

	public Task<object> GetProductCountAsync(Guid invoiceId, CancellationToken cancellationToken = default(CancellationToken))
	{
		var requestObject = new { invoiceId };
		StringBuilder stringBuilder = client.CreateURLBuilder("/api/goods/goods-product-count-by-invoice-id");
		return client.ExecuteAsync<object>(HttpMethod.Post, stringBuilder.ToString(), cancellationToken, requestObject);
	}

	public object GetProductCount(Guid invoiceId)
	{
		return AsyncHelper.RunSync(async () => await GetProductCountAsync(invoiceId));
	}

	public async Task<ArmSoft.TaxService.Models.Invoices.Good> GetAsync(string serialNumber, bool loadItems = false, bool throwException = true, CancellationToken cancellationToken = default(CancellationToken))
	{
		var requestObject = new
		{
			serialNo = serialNumber
		};
		StringBuilder stringBuilder = client.CreateURLBuilder("/api/goods/goods-by-serial-no");
		ArmSoft.TaxService.Models.Invoices.Good doc = await client.ExecuteAsync<ArmSoft.TaxService.Models.Invoices.Good>(HttpMethod.Post, stringBuilder.ToString(), cancellationToken, requestObject);
		if (loadItems && doc != null)
		{
			ArmSoft.TaxService.Models.Invoices.Good good = doc;
			good.Items = await GetProductsListAsync(doc.Id, cancellationToken);
		}
		if ((doc == null) & throwException)
		{
			throw new ApiException(string.Format(Resources.Get(ResourceKey.DocNotExistWithSerialNumber), serialNumber));
		}
		return doc;
	}

	public ArmSoft.TaxService.Models.Invoices.Good Get(string serialNumber, bool loadItems = false, bool throwException = true)
	{
		return AsyncHelper.RunSync(async () => await GetAsync(serialNumber, loadItems, throwException));
	}

	public async Task CreateDraftAsync(ArmSoft.TaxService.Models.Invoices.Good doc, bool rewrite = false, bool fillDataFromBase = false, CancellationToken cancellationToken = default(CancellationToken))
	{
		ModelFieldValidator.Validate(doc);
		doc.Source = Source.API;
		if (fillDataFromBase && doc.IsCorrector)
		{
			ArmSoft.TaxService.Models.Invoices.Good good = Get(doc.CorrectorNo, loadItems: true);
			doc.CorrectorWrittenOfAt = good.IssuedAt;
			doc.BehalfOf = good.BehalfOf;
			foreach (ArmSoft.TaxService.Models.Invoices.Good.Item item in doc.Items)
			{
				ArmSoft.TaxService.Models.Invoices.Good.Item item2;
				if (item.ParentId.HasValue)
				{
					item2 = good.Items.FirstOrDefault((ArmSoft.TaxService.Models.Invoices.Good.Item p) =>
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
					IEnumerable<ArmSoft.TaxService.Models.Invoices.Good.Item> source = good.Items.Where((ArmSoft.TaxService.Models.Invoices.Good.Item p) => p.Name.Trim() == item.Name.Trim() && p.Unit.Trim() == item.Unit.Trim() && Math.Abs(p.UnitPrice - item.UnitPrice) <= 0.1m);
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
				if (item2.VatRate != VatRate.Vat20)
				{
					item.VatAmount = item2.VatAmount;
				}
				if (!item2.IncEnvTax.HasValue || item2.IncEnvTax.Value == 0m)
				{
					item.IncEnvTax = item2.IncEnvTax;
				}
			}
			if (!good.TotalIncEnvTax.HasValue || good.TotalIncEnvTax.Value == 0m)
			{
				doc.TotalIncEnvTax = good.TotalIncEnvTax;
			}
			if (!good.TotalContainerValue.HasValue || good.TotalContainerValue.Value == 0m)
			{
				doc.TotalContainerValue = good.TotalContainerValue;
			}
		}
		if (!doc.IsCorrector || !fillDataFromBase)
		{
			List<ArmSoft.TaxService.Models.Invoices.Good.Item> list = doc.Items.Where((ArmSoft.TaxService.Models.Invoices.Good.Item p) => !string.IsNullOrWhiteSpace(p.Classifier)).ToList();
			if (list.Count > 0)
			{
				foreach (ArmSoft.TaxService.Models.Invoices.Good.Item item3 in list)
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
			ArmSoft.TaxService.Models.Invoices.Good good2 = await GetAsync(doc.Id, loadItems: false, throwException: false, cancellationToken);
			if (good2 == null)
			{
				await GoodsCreateDraftAsync(doc, cancellationToken);
				return;
			}
			if (good2.Status != Status.Draft)
			{
				throw new ApiException(string.Format(Resources.Get(ResourceKey.InvoiceIsNotEditable), EncodeDecodeTypes.GetStatusCaption(good2.Status)));
			}
			await EditDraftAsync(doc, cancellationToken);
		}
		else
		{
			await GoodsCreateDraftAsync(doc, cancellationToken);
		}
	}

	private async Task GoodsCreateDraftAsync(ArmSoft.TaxService.Models.Invoices.Good doc, CancellationToken cancellationToken = default(CancellationToken))
	{
		try
		{
			StringBuilder stringBuilder = client.CreateURLBuilder("/api/goods/goods-create-draft");
			await client.ExecuteAsync(HttpMethod.Post, stringBuilder.ToString(), cancellationToken, doc);
		}
		catch (Exception)
		{
			TaxDoc taxDoc = await new TaxDocsClient(client).GetInvoiceAsync(doc.Id, cancellationToken);
			if (taxDoc == null)
			{
				throw;
			}
			if (taxDoc.InvoiceType == InvoiceType.Goods)
			{
				throw new ApiException(string.Format(Resources.Get(ResourceKey.DocAlreadyExistWithDeliveryDateBuyer), doc.DeliveredAt.FormatDDMMYY(), doc.BuyerName, doc.Id));
			}
			string arg = ((taxDoc.InvoiceType == InvoiceType.None) ? EncodeDecodeTypes.GetAccDocCaption(taxDoc.AccDocType) : EncodeDecodeTypes.GetInvoiceCaption(taxDoc.InvoiceType));
			throw new ApiException(string.Format(Resources.Get(ResourceKey.OtherTypeDocAlreadyExist), taxDoc.DeliveredAt.Value.FormatDDMMYY(), arg, doc.Id));
		}
	}

	public void CreateDraft(ArmSoft.TaxService.Models.Invoices.Good doc, bool rewrite = false, bool fillDataFromBase = false)
	{
		AsyncHelper.RunSync(async () =>
		{
			await CreateDraftAsync(doc, rewrite, fillDataFromBase);
		});
	}

	public async Task EditDraftAsync(ArmSoft.TaxService.Models.Invoices.Good doc, CancellationToken cancellationToken = default(CancellationToken))
	{
		StringBuilder stringBuilder = client.CreateURLBuilder("/api/goods/goods-save-draft");
		await client.ExecuteAsync<ArmSoft.TaxService.Models.Invoices.Good>(HttpMethod.Post, stringBuilder.ToString(), cancellationToken, doc);
	}

	public void EditDraft(ArmSoft.TaxService.Models.Invoices.Good doc)
	{
		AsyncHelper.RunSync(async () =>
		{
			await EditDraftAsync(doc);
		});
	}

	public Task RemoveDraftAsync(Guid id, CancellationToken cancellationToken = default(CancellationToken))
	{
		var requestObject = new { id };
		StringBuilder stringBuilder = client.CreateURLBuilder("/api/goods/goods-remove-draft");
		return client.ExecuteAsync<ArmSoft.TaxService.Models.Invoices.Good>(HttpMethod.Post, stringBuilder.ToString(), cancellationToken, requestObject);
	}

	public void RemoveDraft(Guid id)
	{
		AsyncHelper.RunSync(async () =>
		{
			await RemoveDraftAsync(id);
		});
	}

	public Task CreateProductAsync(ArmSoft.TaxService.Models.Invoices.Good.Item item, CancellationToken cancellationToken = default(CancellationToken))
	{
		StringBuilder stringBuilder = client.CreateURLBuilder("/api/goods/goods-product-create");
		return client.ExecuteAsync<ArmSoft.TaxService.Models.Invoices.Good.Item>(HttpMethod.Post, stringBuilder.ToString(), cancellationToken, item);
	}

	public void CreateProduct(ArmSoft.TaxService.Models.Invoices.Good.Item item)
	{
		AsyncHelper.RunSync(async () =>
		{
			await CreateProductAsync(item);
		});
	}

	public Task EditProductAsync(ArmSoft.TaxService.Models.Invoices.Good.Item item, CancellationToken cancellationToken = default(CancellationToken))
	{
		StringBuilder stringBuilder = client.CreateURLBuilder("/api/goods/goods-product-save");
		return client.ExecuteAsync<ArmSoft.TaxService.Models.Invoices.Good.Item>(HttpMethod.Post, stringBuilder.ToString(), cancellationToken, item);
	}

	public void EditProduct(ArmSoft.TaxService.Models.Invoices.Good.Item item)
	{
		AsyncHelper.RunSync(async () =>
		{
			await EditProductAsync(item);
		});
	}

	public Task RemoveProductAsync(Guid id, CancellationToken cancellationToken = default(CancellationToken))
	{
		var requestObject = new { id };
		StringBuilder stringBuilder = client.CreateURLBuilder("/api/goods/goods-product-remove");
		return client.ExecuteAsync<ArmSoft.TaxService.Models.Invoices.Good.Item>(HttpMethod.Post, stringBuilder.ToString(), cancellationToken, requestObject);
	}

	public void RemoveProduct(Guid id)
	{
		AsyncHelper.RunSync(async () =>
		{
			await RemoveProductAsync(id);
		});
	}

	public void CreateXMLFile(ArmSoft.TaxService.Models.Invoices.Good doc, string fileFullName, bool append = false)
	{
		XMLExport.CreateInvoiceXMLFile(Converter.ConvertGoodToXMLInvoice(doc), fileFullName, append);
	}

	public void CreateXMLFile(List<ArmSoft.TaxService.Models.Invoices.Good> docs, string fileFullName, bool append = false)
	{
		XMLExport.CreateInvoiceXMLFile(Converter.ConvertGoodToXMLInvoice(docs), fileFullName, append);
	}

	public Task UpdateFinalUseAsync(string serialNumber, bool finalUse, CancellationToken cancellationToken = default(CancellationToken))
	{
		var requestObject = new
		{
			serialNo = serialNumber,
			finalUse = finalUse
		};
		StringBuilder stringBuilder = client.CreateURLBuilder("/api/goods/goods-update-final-use");
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
		StringBuilder stringBuilder = client.CreateURLBuilder("/api/goods/goods-remainder");
		return await client.ExecuteAsync<List<ItemRemainder>>(HttpMethod.Post, stringBuilder.ToString(), cancellationToken, requestObject);
	}

	public List<ItemRemainder> GetItemsRemainders(string serialNumber)
	{
		return AsyncHelper.RunSync(async () => await GetItemsRemaindersAsync(serialNumber));
	}
}
