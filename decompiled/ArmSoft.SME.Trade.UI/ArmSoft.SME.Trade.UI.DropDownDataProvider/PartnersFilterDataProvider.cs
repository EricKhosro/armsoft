using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ArmSoft.SME.Client;
using ArmSoft.SME.Client.Data;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Models.Data;
using ArmSoft.SME.Trade.Client.Data;
using ArmSoft.SME.Trade.Common;
using ArmSoft.SME.Trade.Common.Models.Caches;

namespace ArmSoft.SME.Trade.UI.DropDownDataProvider;

public class PartnersFilterDataProvider : DataProvider<PartnersFilterDataProvider.DataRow, PartnersFilterDataProvider.Param>
{
	public class DataRow : ExtendableRow, IMatchedUpdateKey, IMatchedFilterKey
	{
		public string TaxCode { get; set; }

		public string PartCode { get; set; }

		public string SettlAcc { get; set; }

		public string Caption { get; set; }

		public string MainAIM { get; set; }

		public string Phone { get; set; }

		public string Email { get; set; }

		public string FullCaption { get; set; }

		public string Type { get; set; }

		public PartnerGroupCacheRow PartnerGroup { get; set; }

		public bool Supplier { get; set; }

		public bool Customer { get; set; }

		public string Address { get; set; }

		public string BusinessAddress { get; set; }

		public string ManagerFIO { get; set; }

		public string ManagerPost { get; set; }

		public string ChiefaccFIO { get; set; }

		public string ChiefaccPost { get; set; }

		public string PartStateRegister { get; set; }

		public string Passport { get; set; }

		public string B2BStatusName { get; set; }

		public object FilterKey => PartCode;

		public bool FilterMatched(object key, object path)
		{
			if (((string)key).Trim() == Type.Trim())
			{
				return true;
			}
			if (PartnerGroup != null)
			{
				return PartnerGroup.Path.StartsWith(((string)path).Trim() + "!");
			}
			return false;
		}

		public bool Matched(object key)
		{
			return (string)key == PartCode;
		}
	}

	public class Param
	{
		public string PartType { get; set; } = "";

		public bool IsFilterBySuppliers { get; set; }

		public bool IsFilterByCustomers { get; set; }

		public List<string> Partners { get; set; }
	}

	public override string Name => "PartnersFilter";

	public PartnersFilterDataProvider()
		: base((ApiClient)null)
	{
	}

	public override Schema GetSchema()
	{
		Schema schema = new Schema
		{
			Name = Name,
			Caption = ArmSoft.SME.Common.Resources.Partners
		};
		List<Column> columns = new List<Column>
		{
			new Column
			{
				Name = "TaxCode",
				Caption = ArmSoft.SME.Common.Resources.OptionsTaxCode,
				Source = "TaxCode"
			},
			new Column
			{
				Name = "PartCode",
				Caption = ArmSoft.SME.Common.Resources.Code,
				Source = "PartCode"
			},
			new Column
			{
				Name = "SettlAcc",
				Caption = ArmSoft.SME.Common.Resources.SettlementAccount,
				Source = "SettlAcc"
			},
			new Column
			{
				Name = "Caption",
				Caption = ArmSoft.SME.Common.Resources.Name,
				Source = "Caption"
			},
			new Column
			{
				Name = "MainAIM",
				Caption = ArmSoft.SME.Common.Resources.PaymentMainAim,
				Source = "MainAIM"
			},
			new Column
			{
				Name = "Phone",
				Caption = ArmSoft.SME.Common.Resources.Phone,
				Source = "Phone"
			},
			new Column
			{
				Name = "Email",
				Caption = ArmSoft.SME.Common.Resources.Email,
				Source = "Email"
			},
			new Column
			{
				Name = "FullCaption",
				Caption = ArmSoft.SME.Common.Resources.FullCaption,
				Source = "FullCaption"
			},
			new Column
			{
				Name = "Type",
				Caption = ArmSoft.SME.Common.Resources.Type,
				Source = "Type"
			},
			new Column
			{
				Name = "Supplier",
				Caption = ArmSoft.SME.Common.Resources.Supplier,
				Source = "Supplier",
				Type = SupportedType.Boolean
			},
			new Column
			{
				Name = "Customer",
				Caption = ArmSoft.SME.Common.Resources.Customer,
				Source = "Customer",
				Type = SupportedType.Boolean
			},
			new Column
			{
				Name = "Address",
				Caption = ArmSoft.SME.Common.Resources.Address,
				Source = "Address"
			},
			new Column
			{
				Name = "BusinessAddress",
				Caption = ArmSoft.SME.Common.Resources.BusinessAddress,
				Source = "BusinessAddress"
			},
			new Column
			{
				Name = "ManagerFIO",
				Caption = ArmSoft.SME.Common.Resources.ManagerFIO,
				Source = "ManagerFIO"
			},
			new Column
			{
				Name = "ManagerPost",
				Caption = ArmSoft.SME.Common.Resources.ManagerPost,
				Source = "ManagerPost"
			},
			new Column
			{
				Name = "ChiefaccFIO",
				Caption = ArmSoft.SME.Common.Resources.ChAccFIO,
				Source = "ChiefaccFIO"
			},
			new Column
			{
				Name = "ChiefaccPost",
				Caption = ArmSoft.SME.Common.Resources.AccountantPost,
				Source = "ChiefaccPost"
			},
			new Column
			{
				Name = "PartStateRegister",
				Caption = ArmSoft.SME.Trade.Common.Resources.PartStateRegister,
				Source = "PartStateRegister"
			},
			new Column
			{
				Name = "Passport",
				Caption = ArmSoft.SME.Common.Resources.Passport,
				Source = "Passport"
			},
			new Column
			{
				Name = "B2BStatusName",
				Caption = ArmSoft.SME.Common.Resources.State,
				Source = "B2BStatusName"
			}
		};
		schema.Columns = columns;
		return schema;
	}

	public override Task<Schema> GetSchemaAsync(Param param, ExecutionSettings settings, HashSet<string> columns = null, CancellationToken cancellationToken = default(CancellationToken))
	{
		throw new NotImplementedException();
	}

	public override DataProviderResult<DataRow> Execute(Param param, ExecutionSettings settings, List<object> keys = null, bool withoutAccessCheck = false, HashSet<string> columns = null)
	{
		DataProviderResult<DataRow> dataProviderResult = new DataProviderResult<DataRow>();
		dataProviderResult.Rows = new List<DataRow>();
		if (settings != null && settings.ReloadData)
		{
			TradeSettings.TradeCacheService.Partner.Refresh();
		}
		IEnumerable<PartnerCacheRow> enumerable = TradeSettings.TradeCacheService.Partner.GetList();
		if (!string.IsNullOrWhiteSpace(param.PartType))
		{
			PartnerGroupCacheRow partnerGroupRow = TradeSettings.TradeCacheService.PartnerGroup.GetItem(param.PartType);
			enumerable = enumerable.Where((PartnerCacheRow p) =>
			{
				PartnerGroupCacheRow groupRow = p.GroupRow;
				return (groupRow != null && groupRow.Path.StartsWith(partnerGroupRow.Path + "!")) || param.PartType == p.Type;
			});
		}
		if (param.IsFilterBySuppliers)
		{
			enumerable = enumerable.Where((PartnerCacheRow p) => p.Supplier);
		}
		else if (param.IsFilterByCustomers)
		{
			enumerable = enumerable.Where((PartnerCacheRow p) => p.Customer);
		}
		if (param.Partners != null && param.Partners.Count != 0)
		{
			enumerable = enumerable.Where((PartnerCacheRow p) => param.Partners.Contains(p.PartCode));
		}
		foreach (PartnerCacheRow item2 in enumerable)
		{
			DataRow item = new DataRow
			{
				TaxCode = item2.TaxCode,
				PartCode = item2.PartCode,
				SettlAcc = item2.SettlAcc,
				Caption = item2.Caption,
				MainAIM = item2.MainAIM,
				Phone = item2.Phone,
				Email = item2.Email,
				FullCaption = item2.FullCaption,
				Type = item2.Type,
				PartnerGroup = item2.GroupRow,
				Supplier = item2.Supplier,
				Customer = item2.Customer,
				Address = item2.Address,
				BusinessAddress = item2.BusinessAddress,
				ManagerFIO = item2.ManagerFIO,
				ManagerPost = item2.ManagerPost,
				ChiefaccFIO = item2.ChiefaccFIO,
				ChiefaccPost = item2.ChiefaccPost,
				PartStateRegister = item2.PartStateRegister,
				Passport = item2.Passport,
				B2BStatusName = item2.B2BStatusName
			};
			dataProviderResult.Rows.Add(item);
		}
		dataProviderResult.Rows = dataProviderResult.Rows.OrderBy((DataRow x) => x.PartCode).ToList();
		return dataProviderResult;
	}

	public override Task<DataProviderResult<DataRow>> ExecuteAsync(Param param, ExecutionSettings settings, List<object> keys = null, bool withoutAccessCheck = false, HashSet<string> columns = null, CancellationToken cancellationToken = default(CancellationToken))
	{
		throw new NotImplementedException();
	}
}
