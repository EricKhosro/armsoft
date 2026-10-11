using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using ArmSoft.SME.Client;
using ArmSoft.SME.Client.Data;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Extensions;
using ArmSoft.SME.Common.Models.Data;
using ArmSoft.SME.Trade.Common;
using ArmSoft.SME.Trade.Common.Models.Caches;

namespace ArmSoft.SME.Trade.UI.DropDownDataProvider;

public class OfflineGoodsFilterDataProvider : DataProvider<OfflineGoodsFilterDataProvider.DataRow, OfflineGoodsFilterDataProvider.Param>
{
	public class DataRow : ExtendableRow
	{
		public int MTID { get; set; }

		public string MTCode { get; set; }

		public string Caption { get; set; }

		public string CPACode { get; set; }

		public string Group { get; set; }

		public string GroupPath { get; set; }

		public string GroupCaption { get; set; }

		public string Unit { get; set; }

		public string UnitBrief { get; set; }

		public decimal Price { get; set; }

		public string AltUnit { get; set; }

		public string AltUnitBrief { get; set; }

		public string BarCode { get; set; }

		public string Description { get; set; }

		public string TypeName { get; set; }

		public string Country { get; set; }

		public string ExpenseMethodName { get; set; }

		public bool VAT { get; set; }

		public bool ShowInPriceList { get; set; }

		public string FullCaption { get; set; }

		public bool IsWeight { get; set; }

		public bool CheckMarking { get; set; }
	}

	public class Param
	{
		public string PriceType { get; set; }

		public string LikeName { get; set; }
	}

	public override string Name => "OfflineGoodsFilter";

	public OfflineGoodsFilterDataProvider()
		: base((ApiClient)null)
	{
	}

	public override Schema GetSchema()
	{
		Schema schema = new Schema
		{
			Name = Name,
			Caption = ArmSoft.SME.Trade.Common.Resources.MaterialsAndServices
		};
		int num = 10;
		List<Column> list = new List<Column>(num);
		CollectionsMarshal.SetCount(list, num);
		Span<Column> span = CollectionsMarshal.AsSpan(list);
		span[0] = new Column
		{
			Name = "MTCode",
			Caption = ArmSoft.SME.Common.Resources.Code,
			Source = "MTCode"
		};
		span[1] = new Column
		{
			Name = "Caption",
			Caption = ArmSoft.SME.Common.Resources.Name,
			Source = "Caption"
		};
		span[2] = new Column
		{
			Name = "CPACode",
			Caption = ArmSoft.SME.Common.Resources.CPAClassification,
			Source = "CPACode"
		};
		span[3] = new Column
		{
			Name = "Group",
			Caption = ArmSoft.SME.Common.Resources.Group,
			Source = "Group"
		};
		span[4] = new Column
		{
			Name = "GroupCaption",
			Caption = ArmSoft.SME.Common.Resources.GroupName,
			Source = "GroupCaption"
		};
		span[5] = new Column
		{
			Name = "UnitBrief",
			Caption = ArmSoft.SME.Trade.Common.Resources.QntUnit,
			Source = "UnitBrief"
		};
		span[6] = new Column
		{
			Name = "Price",
			Caption = ArmSoft.SME.Common.Resources.MTTAccSalePrice,
			Source = "Price",
			Type = SupportedType.Decimal,
			Format = DecimalExtension.GetCurrencyFormat(16, 2)
		};
		span[7] = new Column
		{
			Name = "BarCode",
			Caption = ArmSoft.SME.Common.Resources.BarCode,
			Source = "BarCode"
		};
		span[8] = new Column
		{
			Name = "Description",
			Caption = ArmSoft.SME.Common.Resources.Specification,
			Source = "Description"
		};
		span[9] = new Column
		{
			Name = "TypeName",
			Caption = ArmSoft.SME.Common.Resources.TypeWord,
			Source = "TypeName"
		};
		List<Column> columns = list;
		schema.Columns = columns;
		return schema;
	}

	public override Task<Schema> GetSchemaAsync(Param param, ExecutionSettings settings, HashSet<string> columns = null, CancellationToken cancellationToken = default(CancellationToken))
	{
		throw new NotImplementedException();
	}

	public override DataProviderResult<DataRow> Execute(Param param, ExecutionSettings settings, List<object> keys = null, bool withoutAccessCheck = false, HashSet<string> columns = null)
	{
		DataProviderResult<DataRow> dataProviderResult = new DataProviderResult<DataRow>
		{
			Rows = new List<DataRow>()
		};
		foreach (GoodsFilterDataProvider.DataRow row in new GoodsFilterDataProvider().GetRows(new GoodsFilterDataProvider.Param
		{
			LikeName = param.LikeName,
			IsShowOnlyInPriceList = true
		}))
		{
			DataRow dataRow = new DataRow
			{
				TypeName = row.TypeName,
				MTID = row.MTID,
				MTCode = row.MTCode,
				Caption = row.Caption,
				CPACode = row.CPACode,
				Group = row.Group,
				GroupPath = row.GroupPath,
				GroupCaption = row.GroupCaption,
				Unit = row.Unit,
				UnitBrief = row.UnitBrief,
				AltUnit = row.AltUnit,
				AltUnitBrief = row.AltUnitBrief,
				BarCode = row.BarCode,
				Description = row.Description,
				Country = row.Country,
				ExpenseMethodName = row.ExpenseMethodName,
				VAT = row.VAT,
				ShowInPriceList = row.ShowInPriceList,
				FullCaption = row.FullCaption,
				IsWeight = row.IsWeight,
				CheckMarking = row.CheckMarking
			};
			CurrentPricesCacheRow item = TradeSettings.TradeCacheService.CurrentPrices.GetItem(new CurrentPricesCacheRow.RowKey
			{
				MTID = dataRow.MTID,
				PriceType = param.PriceType,
				Unit = dataRow.Unit
			});
			if (item != null)
			{
				dataRow.Price = item.Price;
			}
			dataProviderResult.Rows.Add(dataRow);
		}
		return dataProviderResult;
	}

	public override Task<DataProviderResult<DataRow>> ExecuteAsync(Param param, ExecutionSettings settings, List<object> keys = null, bool withoutAccessCheck = false, HashSet<string> columns = null, CancellationToken cancellationToken = default(CancellationToken))
	{
		throw new NotImplementedException();
	}
}
