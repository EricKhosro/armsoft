using System.Collections.Generic;
using System.Linq;
using ArmSoft.SME.Client;
using ArmSoft.SME.Client.Data;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Extensions;
using ArmSoft.SME.Common.Models.Data;
using ArmSoft.SME.Trade.Client.Data.Directories;
using ArmSoft.SME.Trade.Common;

namespace ArmSoft.SME.Trade.UI.DropDownDataProvider;

public class GiftSetRowsDataProvider : DataProvider<GiftSetRowsDataProvider.DataRow, GiftSetRowsDataProvider.Param>
{
	public class DataRow : ExtendableRow
	{
		public string ItemType { get; set; }

		public string MTCode { get; set; }

		public string Caption { get; set; }

		public string QntUnitBrief { get; set; }

		public decimal Quantity { get; set; }

		public decimal Summ { get; set; }

		public DataRow(string itemType, string mtCode, string caption, string qntUnitBrief, decimal quantity, decimal summ)
		{
			ItemType = itemType;
			MTCode = mtCode;
			Caption = caption;
			QntUnitBrief = qntUnitBrief;
			Quantity = quantity;
			Summ = summ;
		}

		public DataRow()
		{
		}
	}

	public class Param
	{
		public List<MTComplectsPartsDataProvider.DataRow> GiftRows { get; set; }
	}

	public override string Name => "GiftSetRows";

	public GiftSetRowsDataProvider()
		: base((ApiClient)null)
	{
	}

	public override Schema GetSchema()
	{
		Schema schema = new Schema
		{
			Name = Name
		};
		List<Column> columns = new List<Column>
		{
			new Column
			{
				Name = "ItemType",
				Caption = ArmSoft.SME.Common.Resources.TypeWord,
				Source = "ItemType"
			},
			new Column
			{
				Name = "MTCode",
				Caption = ArmSoft.SME.Common.Resources.Code,
				Source = "MTCode"
			},
			new Column
			{
				Name = "Caption",
				Caption = ArmSoft.SME.Common.Resources.Name,
				Source = "Caption"
			},
			new Column
			{
				Name = "QntUnitBrief",
				Caption = ArmSoft.SME.Trade.Common.Resources.QntUnit,
				Source = "QntUnitBrief"
			},
			new Column
			{
				Name = "Quantity",
				Caption = ArmSoft.SME.Common.Resources.Quantity,
				Type = SupportedType.Decimal,
				Source = "Quantity",
				Format = DecimalExtension.GetCurrencyFormat(16, 3)
			},
			new Column
			{
				Name = "Summ",
				Caption = ArmSoft.SME.Common.Resources.Price,
				Source = "Summ",
				Type = SupportedType.Decimal,
				Format = DecimalExtension.GetCurrencyFormat(16, 2)
			}
		};
		schema.Columns = columns;
		return schema;
	}

	public override DataProviderResult<DataRow> Execute(Param param, ExecutionSettings settings, List<object> keys = null, bool withoutAccessCheck = false, HashSet<string> columns = null)
	{
		if (settings != null && settings.ReloadData)
		{
			TradeSettings.TradeCacheService.Material.Refresh();
		}
		DataProviderResult<DataRow> dataProviderResult = new DataProviderResult<DataRow>
		{
			Rows = new List<DataRow>()
		};
		foreach (MTComplectsPartsDataProvider.DataRow giftRow in param.GiftRows)
		{
			DataRow item = new DataRow
			{
				ItemType = giftRow.ItemTypeCaption,
				MTCode = giftRow.MTSourceCode,
				Caption = giftRow.MTSourceCaption,
				QntUnitBrief = TradeSettings.TradeCacheService.QntUnit.GetItem(giftRow.QntUnit).Brief,
				Quantity = giftRow.QTY,
				Summ = giftRow.Summ
			};
			dataProviderResult.Rows.Add(item);
		}
		dataProviderResult.Rows = dataProviderResult.Rows.OrderBy((DataRow x) => x.MTCode).ToList();
		return dataProviderResult;
	}
}
