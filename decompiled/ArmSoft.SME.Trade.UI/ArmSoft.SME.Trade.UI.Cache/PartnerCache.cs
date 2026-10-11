using System.Collections.Generic;
using System.Linq;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Exceptions;
using ArmSoft.SME.Common.Models.Data;
using ArmSoft.SME.Common.SmartEnums;
using ArmSoft.SME.Core.UI.DataViews;
using ArmSoft.SME.Core.UI.Services;
using ArmSoft.SME.Trade.Common;
using ArmSoft.SME.Trade.Common.Models.Caches;
using ArmSoft.SME.Trade.Common.SmartEnums;

namespace ArmSoft.SME.Trade.UI.Cache;

public class PartnerCache : CacheDefinition<PartnerCacheRow, string>
{
	public override short CacheId => ArmSoft.SME.Trade.Common.SmartEnums.CacheIdSE.Partners.Value;

	public override bool StoreInLocalStorage => true;

	protected override bool CacheAlsoWithId => true;

	public PartnerCache()
	{
		PartnerCacheRow.GroupUIResolver = (string code) => (!string.IsNullOrWhiteSpace(code)) ? TradeSettings.TradeCacheService.PartnerGroup.GetItem(code, throwExceptionNotExits: true) : null;
		PartnerCacheRow.ExtensionUIResolver = (string code, string fieldName) => ExtensionUIResolverFunction(TradeSettings.TradeCacheService.PartnerExtensions.GetItem(code), fieldName);
	}

	public IReadOnlyList<PartnerCacheRow> GetPartnersList(string partnerType = "")
	{
		return GetList((PartnerCacheRow p) => p.Type == partnerType);
	}

	public IReadOnlyList<PartnerCacheRow> GetPartnersByTaxCode(string taxCode)
	{
		return GetList((PartnerCacheRow p) => p.TaxCode == taxCode);
	}

	public PartnerCacheRow GetItemBySettlAccount(string settlAccount, bool throwExceptionNotExits = false)
	{
		PartnerCacheRow? partnerCacheRow = GetList().FirstOrDefault((PartnerCacheRow p) => p.SettlementAccounts.Any((PartnerCacheRow.SettlementAccountRow q) => q.SettlAccount == settlAccount));
		if ((partnerCacheRow == null) & throwExceptionNotExits)
		{
			throw new RESTException(string.Format(ArmSoft.SME.Common.Resources.CodeNotExistDir, settlAccount, ArmSoft.SME.Common.Resources.PartnerAccountsInBank));
		}
		return partnerCacheRow;
	}

	public List<string> GetAllBusinessAddresses(PartnerCacheRow partner)
	{
		List<string> list = new List<string>();
		if (partner != null)
		{
			if (!string.IsNullOrWhiteSpace(partner.BusinessAddress))
			{
				list.Add(partner.BusinessAddress);
			}
			list.AddRange(partner.BusinessAddresses);
		}
		return list;
	}

	public PartnerCacheRow.ContractRow GetContract(int partnerId, string contractCode, bool throwExceptionNotExits = false)
	{
		PartnerCacheRow itemById = GetItemById(partnerId, throwExceptionNotExits);
		return GetContract(itemById, contractCode, throwExceptionNotExits);
	}

	public PartnerCacheRow.ContractRow GetContract(int partnerId, string contractCode, out PartnerCacheRow partnerCacheRow, bool throwExceptionNotExits = false)
	{
		partnerCacheRow = GetItemById(partnerId, throwExceptionNotExits);
		return GetContract(partnerCacheRow, contractCode, throwExceptionNotExits);
	}

	public PartnerCacheRow.ContractRow GetContract(string partnerCode, string contractCode, out PartnerCacheRow partnerCacheRow, bool throwExceptionNotExits = false)
	{
		partnerCacheRow = GetItem(partnerCode, throwExceptionNotExits);
		return GetContract(partnerCacheRow, contractCode, throwExceptionNotExits);
	}

	public PartnerCacheRow.ContractRow GetContract(string partnerCode, string contractCode, bool throwExceptionNotExits = false)
	{
		PartnerCacheRow item = GetItem(partnerCode, throwExceptionNotExits);
		return GetContract(item, contractCode, throwExceptionNotExits);
	}

	public PartnerCacheRow.ContractRow GetContract(PartnerCacheRow partnerCacheRow, string contractCode, bool throwExceptionNotExits = false)
	{
		PartnerCacheRow.ContractRow contractRow = partnerCacheRow?.Contracts.FirstOrDefault((PartnerCacheRow.ContractRow p) => p.Contract == contractCode);
		if (throwExceptionNotExits && contractRow == null)
		{
			throw new RESTException(string.Format(ArmSoft.SME.Trade.Common.Resources.ErrContractNotExists, contractCode, partnerCacheRow.PartCode));
		}
		return contractRow;
	}

	public PartnerCacheRow.ContractRow GetDefaultContract(string partnerCode, string currCode, out PartnerCacheRow partnerCacheRow, bool throwExceptionNotExits = false)
	{
		partnerCacheRow = GetItem(partnerCode, throwExceptionNotExits);
		return GetDefaultContract(partnerCacheRow, currCode, throwExceptionNotExits);
	}

	public PartnerCacheRow.ContractRow GetDefaultContract(string partnerCode, out PartnerCacheRow partnerCacheRow, bool throwExceptionNotExits = false)
	{
		partnerCacheRow = GetItem(partnerCode, throwExceptionNotExits);
		return GetDefaultContract(partnerCacheRow, null, throwExceptionNotExits);
	}

	public PartnerCacheRow.ContractRow GetDefaultContract(string partnerCode, string currCode = null, bool throwExceptionNotExits = false)
	{
		PartnerCacheRow item = GetItem(partnerCode, throwExceptionNotExits);
		return GetDefaultContract(item, currCode, throwExceptionNotExits);
	}

	public PartnerCacheRow.ContractRow GetDefaultContract(PartnerCacheRow partnerCacheRow, string currCode = null, bool throwExceptionNotExits = false)
	{
		PartnerCacheRow.ContractRow contractRow = (string.IsNullOrWhiteSpace(currCode) ? partnerCacheRow?.Contracts.FirstOrDefault((PartnerCacheRow.ContractRow p) => p.IsDefault) : partnerCacheRow?.Contracts.FirstOrDefault((PartnerCacheRow.ContractRow p) => p.CurrCode == currCode.Trim()));
		if (throwExceptionNotExits && contractRow == null)
		{
			throw new RESTException(string.Format(ArmSoft.SME.Common.Resources.ElemWithCodeNotExists, ArmSoft.SME.Common.Resources.Partner + " - " + partnerCacheRow.PartCode, ArmSoft.SME.Common.Resources.Contract));
		}
		return contractRow;
	}

	public PartnerCacheRow.ContractRow GetContractByDiscountCard(string cardNumber, out PartnerCacheRow partnerCacheRow)
	{
		PartnerCacheRow.ContractRow contractRow = null;
		partnerCacheRow = null;
		foreach (PartnerCacheRow item in GetList())
		{
			contractRow = item.Contracts.FirstOrDefault((PartnerCacheRow.ContractRow p) => p.DiscountCard == cardNumber);
			if (contractRow != null)
			{
				partnerCacheRow = item;
				return contractRow;
			}
		}
		return contractRow;
	}

	public static List<ViewColumn> GetDataViewColumns(string group, string namePrefix, string captionPrefix, bool includeCode = false, bool includeCaption = false, bool includeTaxCode = true, bool onlyContract = false, bool onlyExtendedFields = false)
	{
		List<ViewColumn> list = new List<ViewColumn>();
		if (!onlyExtendedFields)
		{
			if (onlyContract)
			{
				list.Add(new ViewColumn("Contract", captionPrefix + ArmSoft.SME.Common.Resources.Code, "Contract", DataColumnType.String, group, "ContractCache"));
				list.Add(new ViewColumn("Caption", captionPrefix + ArmSoft.SME.Common.Resources.Contract, "Caption", DataColumnType.String, group, "ContractCache"));
				list.Add(new ViewColumn("CurrCode", captionPrefix + ArmSoft.SME.Common.Resources.Curr, "CurrCode", DataColumnType.String, group, "ContractCache"));
				list.Add(new ViewColumn("PriceType", captionPrefix + ArmSoft.SME.Common.Resources.PriceListType, "PriceType", DataColumnType.String, group, "ContractCache"));
				list.Add(new ViewColumn("ReceivablesLimit", captionPrefix + ArmSoft.SME.Trade.Common.Resources.ReceivablesLimit, "ReceivablesLimit", DataColumnType.Decimal, group, "ContractCache"));
				list.Add(new ViewColumn("ContractDate", captionPrefix + ArmSoft.SME.Common.Resources.ContractDate, "ContractDate", DataColumnType.DateTime, group, "ContractCache"));
			}
			else
			{
				if (includeCode)
				{
					list.Add(new ViewColumn(namePrefix + "PartCode", captionPrefix + ArmSoft.SME.Common.Resources.Code, "PartCode", DataColumnType.String, group, "PartnerCache"));
				}
				if (includeCaption)
				{
					list.Add(new ViewColumn(namePrefix + "Caption", captionPrefix + ArmSoft.SME.Common.Resources.Name, "Caption", DataColumnType.String, group, "PartnerCache"));
					list.Add(new ViewColumn(namePrefix + "FullCaption", captionPrefix + ArmSoft.SME.Common.Resources.FullName, "FullCaption", DataColumnType.String, group, "PartnerCache"));
				}
				if (includeTaxCode)
				{
					list.Add(new ViewColumn(namePrefix + "TaxCode", captionPrefix + ArmSoft.SME.Common.Resources.OptionsTaxCode, "TaxCode", DataColumnType.String, group, "PartnerCache"));
				}
				list.Add(new ViewColumn(namePrefix + "Type", captionPrefix + ArmSoft.SME.Common.Resources.Group, "Type", DataColumnType.String, group, "PartnerCache"));
				list.AddRange(PartnerGroupCache.GetDataViewColumns(group, namePrefix, captionPrefix));
				list.Add(new ViewColumn(namePrefix + "Phone", captionPrefix + ArmSoft.SME.Common.Resources.PhoneNumber, "Phone", DataColumnType.String, group, "PartnerCache"));
				list.Add(new ViewColumn(namePrefix + "Email", captionPrefix + ArmSoft.SME.Common.Resources.EMailShort, "Email", DataColumnType.String, group, "PartnerCache"));
				list.Add(new ViewColumn(namePrefix + "Address", captionPrefix + ArmSoft.SME.Common.Resources.LegalAddresses, "Address", DataColumnType.String, group, "PartnerCache"));
				list.Add(new ViewColumn(namePrefix + "Passport", captionPrefix + ArmSoft.SME.Common.Resources.Passport, "Passport", DataColumnType.String, group, "PartnerCache"));
				list.Add(new ViewColumn(namePrefix + "BusinessAddress", captionPrefix + ArmSoft.SME.Common.Resources.BusinessAddresses, "BusinessAddress", DataColumnType.String, group, "PartnerCache"));
				list.Add(new ViewColumn(namePrefix + "ManagerFIO", captionPrefix + ArmSoft.SME.Common.Resources.ManagerFIO, "ManagerFIO", DataColumnType.String, group, "PartnerCache"));
				list.Add(new ViewColumn(namePrefix + "ManagerPost", captionPrefix + ArmSoft.SME.Common.Resources.ManagerPost, "ManagerPost", DataColumnType.String, group, "PartnerCache"));
				list.Add(new ViewColumn(namePrefix + "ChiefaccFIO", captionPrefix + ArmSoft.SME.Common.Resources.ChAccFIO, "ChiefaccFIO", DataColumnType.String, group, "PartnerCache"));
				list.Add(new ViewColumn(namePrefix + "ChiefaccPost", captionPrefix + ArmSoft.SME.Common.Resources.AccountantPost, "ChiefaccPost", DataColumnType.String, group, "PartnerCache"));
				list.Add(new ViewColumn(namePrefix + "SettlAcc", captionPrefix + ArmSoft.SME.Common.Resources.SettlementAccount, "SettlAcc", DataColumnType.String, group, "PartnerCache"));
				list.Add(new ViewColumn(namePrefix + "MainAIM", captionPrefix + ArmSoft.SME.Common.Resources.PaymentMainAim, "MainAIM", DataColumnType.String, group, "PartnerCache"));
			}
		}
		List<ViewColumn> collection = ViewColumn.AddXMLColumns(POTypeSE.Directory.Value, ArmSoft.SME.Trade.Common.SmartEnums.DirectoryTypeSE.Partner.Value, group, "PartnerCache", addGroupToCaption: false);
		list.AddRange(collection);
		return list;
	}
}
