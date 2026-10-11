using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ArmSoft.SME.Client;
using ArmSoft.SME.Client.Data;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Extensions;
using ArmSoft.SME.Common.Models.Data;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Trade.Common;
using ArmSoft.SME.Trade.Common.Models.Caches;
using ArmSoft.SME.Trade.Common.Parameters;
using ArmSoft.SME.Trade.Common.SmartEnums;

namespace ArmSoft.SME.Trade.UI.DropDownDataProvider;

public class ContractDataProvider : DataProvider<ContractDataProvider.DataRow, ContractDataProvider.Param>
{
	public class DataRow : ExtendableRow
	{
		public class Key
		{
			public string PartCode { get; set; }

			public string Contract { get; set; }
		}

		public string PartCode { get; set; }

		public string PartCaption { get; set; }

		public string PartTaxCode { get; set; }

		public string PartPhone { get; set; }

		public string PartGroup { get; set; }

		public string PartGroupName { get; set; }

		public string Contract { get; set; }

		public string Caption { get; set; }

		public string CurrCode { get; set; }

		public string PriceType { get; set; }

		public decimal Percent { get; set; }

		public string DiscountCard { get; set; }

		public bool IsBonusCard { get; set; }

		public decimal Bonus { get; set; }

		public decimal Point { get; set; }

		public bool ControlReceivables { get; set; }

		public decimal ReceivablesLimit { get; set; }

		public bool IsDefault { get; set; }

		public DateOnly? ContractDate { get; set; }

		public string Description { get; set; }

		public DateOnly? BonusCardValidPeriodStart { get; set; }

		public DateOnly? BonusCardValidPeriodEnd { get; set; }

		public bool BonusCardCanceled { get; set; }

		public bool ExportedToDiscountServer { get; set; }

		public byte[] TS { get; set; }

		public DataRow(string partCode, string partCaption, string partTaxCode, string partPhone, string partGroup, string partGroupName, string contract, string caption, string currCode, string priceType, decimal percent, string discountCard, bool isBonusCard, decimal bonus, decimal point, bool controlReceivables, decimal receivablesLimit, bool isDefault, DateOnly? contractDate, string description, DateOnly? bonusCardValidPeriodStart, DateOnly? bonusCardValidPeriodEnd, bool bonusCardCanceled, bool exportedToDiscountServer, byte[] ts)
		{
			PartCode = partCode;
			PartCaption = partCaption;
			PartTaxCode = partTaxCode;
			PartPhone = partPhone;
			PartGroup = partGroup;
			PartGroupName = partGroupName;
			Contract = contract;
			Caption = caption;
			CurrCode = currCode;
			PriceType = priceType;
			Percent = percent;
			DiscountCard = discountCard;
			IsBonusCard = isBonusCard;
			Bonus = bonus;
			Point = point;
			ControlReceivables = controlReceivables;
			ReceivablesLimit = receivablesLimit;
			IsDefault = isDefault;
			ContractDate = contractDate;
			Description = description;
			BonusCardValidPeriodStart = bonusCardValidPeriodStart;
			BonusCardValidPeriodEnd = bonusCardValidPeriodEnd;
			BonusCardCanceled = bonusCardCanceled;
			ExportedToDiscountServer = exportedToDiscountServer;
			TS = ts;
		}

		public DataRow()
		{
		}
	}

	public class Param
	{
		public string PartnerCode { get; set; }

		public string CurrCode { get; set; }

		public bool IsExceptCurrCodeNCC { get; set; }

		public bool IsFilterBySuppliers { get; set; }

		public bool IsFilterByCustomers { get; set; }

		public bool ShowOnlyBonusCards { get; set; }
	}

	public override string Name => "PartnersContract";

	public ContractDataProvider()
		: base((ApiClient)null)
	{
	}

	public override Schema GetSchema()
	{
		Schema schema = new Schema
		{
			Name = Name,
			Caption = ArmSoft.SME.Common.Resources.Contract
		};
		string caption = ((ArmSoft.SME.Core.UI.Settings.ParametersService.BonusType() == BonusTypeSE.ByPercent.Value) ? string.Format(ArmSoft.SME.Trade.Common.Resources.Bonus, ArmSoft.SME.Trade.Common.Resources.PercentSymbol) : string.Format(ArmSoft.SME.Trade.Common.Resources.Bonus, ArmSoft.SME.Common.Resources.Unit));
		List<Column> columns = new List<Column>
		{
			new Column
			{
				Name = "PartCode",
				Caption = ArmSoft.SME.Common.Resources.Partner,
				Source = "PartCode"
			},
			new Column
			{
				Name = "PartCaption",
				Caption = ArmSoft.SME.Common.Resources.PartnerName,
				Source = "PartCaption"
			},
			new Column
			{
				Name = "PartTaxCode",
				Caption = ArmSoft.SME.Common.Resources.OptionsTaxCode,
				Source = "PartTaxCode"
			},
			new Column
			{
				Name = "PartPhone",
				Caption = ArmSoft.SME.Common.Resources.Phone,
				Source = "PartPhone"
			},
			new Column
			{
				Name = "PartGroup",
				Caption = ArmSoft.SME.Common.Resources.Group,
				Source = "PartGroup"
			},
			new Column
			{
				Name = "PartGroupName",
				Caption = ArmSoft.SME.Common.Resources.GroupName,
				Source = "PartGroupName"
			},
			new Column
			{
				Name = "Contract",
				Caption = ArmSoft.SME.Common.Resources.Code,
				Source = "Contract"
			},
			new Column
			{
				Name = "Caption",
				Caption = ArmSoft.SME.Common.Resources.Name,
				Source = "Caption"
			},
			new Column
			{
				Name = "CurrCode",
				Caption = ArmSoft.SME.Common.Resources.Curr,
				Source = "CurrCode"
			},
			new Column
			{
				Name = "PriceType",
				Caption = ArmSoft.SME.Trade.Common.Resources.PriceType,
				Source = "PriceType"
			},
			new Column
			{
				Name = "Percent",
				Caption = ArmSoft.SME.Common.Resources.DiscountPercent,
				Source = "Percent",
				Type = SupportedType.Decimal,
				Format = DecimalExtension.GetCurrencyFormat(6, 2)
			},
			new Column
			{
				Name = "DiscountCard",
				Caption = ArmSoft.SME.Trade.Common.Resources.DiscountCardCode,
				Source = "DiscountCard"
			},
			new Column
			{
				Name = "IsBonusCard",
				Caption = ArmSoft.SME.Trade.Common.Resources.BonusCard,
				Source = "IsBonusCard",
				Type = SupportedType.Boolean
			},
			new Column
			{
				Name = "Bonus",
				Caption = caption,
				Source = "Bonus",
				Type = SupportedType.Decimal,
				Format = DecimalExtension.GetCurrencyFormat(6, 2)
			},
			new Column
			{
				Name = "Point",
				Caption = caption,
				Source = "Point",
				Type = SupportedType.Decimal,
				Format = DecimalExtension.GetCurrencyFormat(16, 2)
			},
			new Column
			{
				Name = "ExportedToDiscountServer",
				Caption = ArmSoft.SME.Trade.Common.Resources.ExportedToDiscountServer,
				Source = "ExportedToDiscountServer",
				Type = SupportedType.Boolean
			},
			new Column
			{
				Name = "BonusCardValidPeriodStart",
				Caption = ArmSoft.SME.Trade.Common.Resources.BonusCardValidationPeriod,
				Source = "BonusCardValidPeriodStart",
				Type = SupportedType.DateOnlyNullable
			},
			new Column
			{
				Name = "BonusCardValidPeriodEnd",
				Caption = ArmSoft.SME.Trade.Common.Resources.BonusCardValidationPeriod,
				Source = "BonusCardValidPeriodEnd",
				Type = SupportedType.DateOnlyNullable
			},
			new Column
			{
				Name = "BonusCardCanceled",
				Caption = ArmSoft.SME.Trade.Common.Resources.BonusCardCanceled,
				Source = "BonusCardCanceled",
				Type = SupportedType.Boolean
			},
			new Column
			{
				Name = "ControlReceivables",
				Caption = ArmSoft.SME.Trade.Common.Resources.ControlReceivables,
				Source = "ControlReceivables",
				Type = SupportedType.Boolean
			},
			new Column
			{
				Name = "ReceivablesLimit",
				Caption = ArmSoft.SME.Trade.Common.Resources.ReceivablesLimit,
				Source = "ReceivablesLimit",
				Type = SupportedType.Decimal,
				Format = DecimalExtension.GetCurrencyFormat(16, 2)
			},
			new Column
			{
				Name = "IsDefault",
				Caption = ArmSoft.SME.Common.Resources.Default,
				Source = "IsDefault",
				Type = SupportedType.Boolean
			},
			new Column
			{
				Name = "ContractDate",
				Caption = ArmSoft.SME.Common.Resources.DateWord,
				Source = "ContractDate",
				Type = SupportedType.DateOnlyNullable
			},
			new Column
			{
				Name = "Description",
				Caption = ArmSoft.SME.Trade.Common.Resources.TemplatesDescription,
				Source = "Description"
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
		if (settings != null && settings.ReloadData)
		{
			TradeSettings.TradeCacheService.Partner.Refresh();
		}
		DataProviderResult<DataRow> dataProviderResult = new DataProviderResult<DataRow>
		{
			Rows = new List<DataRow>()
		};
		IEnumerable<PartnerCacheRow> enumerable = (string.IsNullOrWhiteSpace(param.PartnerCode) ? TradeSettings.TradeCacheService.Partner.GetList() : new _003C_003Ez__ReadOnlySingleElementList<PartnerCacheRow>(TradeSettings.TradeCacheService.Partner.GetItem(param.PartnerCode)));
		if (param.IsFilterBySuppliers)
		{
			enumerable = enumerable.Where((PartnerCacheRow x) => x.Supplier);
		}
		if (param.IsFilterByCustomers)
		{
			enumerable = enumerable.Where((PartnerCacheRow x) => x.Customer);
		}
		foreach (PartnerCacheRow item2 in enumerable)
		{
			foreach (PartnerCacheRow.ContractRow contract in item2.Contracts)
			{
				if ((!param.ShowOnlyBonusCards || contract.IsBonusCard) && (string.IsNullOrWhiteSpace(param.CurrCode) || contract.CurrCode == param.CurrCode) && (!param.IsExceptCurrCodeNCC || contract.CurrCode != "AMD"))
				{
					DataRow item = new DataRow(item2.PartCode, item2.Caption, item2.TaxCode, item2.Phone, item2.GroupRow?.Code ?? "", item2.GroupRow?.Caption ?? "", contract.Contract, contract.Caption, contract.CurrCode, contract.PriceType, contract.Percent, contract.DiscountCard, contract.IsBonusCard, contract.Bonus, contract.Point, contract.ControlReceivables, contract.ReceivablesLimit, contract.IsDefault, contract.ContractDate, contract.Description, contract.BonusCardValidPeriodStart, contract.BonusCardValidPeriodEnd, contract.BonusCardCanceled, contract.ExportedToDiscountServer, contract.TS);
					dataProviderResult.Rows.Add(item);
				}
			}
		}
		if (string.IsNullOrWhiteSpace(param.PartnerCode))
		{
			dataProviderResult.Rows = (from x in dataProviderResult.Rows
				orderby x.IsDefault descending, x.Contract
				select x).ToList();
		}
		else
		{
			dataProviderResult.Rows = (from x in dataProviderResult.Rows
				orderby x.PartCode, x.IsDefault descending, x.Contract
				select x).ToList();
		}
		return dataProviderResult;
	}

	public override Task<DataProviderResult<DataRow>> ExecuteAsync(Param param, ExecutionSettings settings, List<object> keys = null, bool withoutAccessCheck = false, HashSet<string> columns = null, CancellationToken cancellationToken = default(CancellationToken))
	{
		throw new NotImplementedException();
	}
}
