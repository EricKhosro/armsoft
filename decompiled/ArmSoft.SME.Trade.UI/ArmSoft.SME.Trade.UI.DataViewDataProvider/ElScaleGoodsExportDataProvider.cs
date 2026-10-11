using System;
using System.Collections.Generic;
using ArmSoft.SME.Client;
using ArmSoft.SME.Client.Data;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Models.Data;
using ArmSoft.SME.Trade.Common;

namespace ArmSoft.SME.Trade.UI.DataViewDataProvider;

public class ElScaleGoodsExportDataProvider : DataProvider<ElScaleGoodsExportDataProvider.DataRow, ElScaleGoodsExportDataProvider.Param>
{
	public class DataRow : ExtendableRow
	{
		public int ID { get; set; }

		public string CodeInTrade { get; set; }

		public string CodeInScale { get; set; }

		public string Name { get; set; }

		public int NameTextAlignmentID { get; set; }

		public string Ingredients { get; set; }

		public int? PLU { get; set; }

		public bool IsWeight { get; set; }

		public string BarCode { get; set; }

		public int? BarCodePrefix { get; set; }

		public string BasicUnit { get; set; }

		public int GroupCode { get; set; }

		public DateTime BestBefore { get; set; }

		public string CertificationCode { get; set; }

		public decimal Price { get; set; }

		public decimal ShelfLife { get; set; }

		public decimal AdditionalPercent { get; set; }

		public int TareWeight { get; set; }

		public int UnitWeight { get; set; }

		public bool Exported { get; set; }

		public string ErrorComment { get; set; }

		public int LabelN { get; set; }

		public int BarCodeN { get; set; }
	}

	public class Param
	{
		public List<DataRow> Rows { get; set; }
	}

	public override string Name => "ElScaleGoodsExport";

	public ElScaleGoodsExportDataProvider()
		: base((ApiClient)null)
	{
	}

	public override Schema GetSchema()
	{
		Schema schema = new Schema
		{
			Name = Name,
			Caption = ArmSoft.SME.Trade.Common.Resources.ScaleExportedGoods
		};
		List<Column> columns = new List<Column>
		{
			new Column
			{
				Name = "CodeInTrade",
				Caption = ArmSoft.SME.Common.Resources.Code,
				Source = "CodeInTrade"
			},
			new Column
			{
				Name = "PLU",
				Caption = ArmSoft.SME.Trade.Common.Resources.PLUCodeInElScale,
				Source = "PLU",
				Type = SupportedType.String
			},
			new Column
			{
				Name = "CodeInScale",
				Caption = ArmSoft.SME.Trade.Common.Resources.CodeInScale,
				Source = "CodeInScale",
				Type = SupportedType.String
			},
			new Column
			{
				Name = "Name",
				Caption = ArmSoft.SME.Common.Resources.Name,
				Source = "Name"
			},
			new Column
			{
				Name = "Price",
				Caption = ArmSoft.SME.Common.Resources.Price,
				Source = "Price",
				Type = SupportedType.Decimal
			},
			new Column
			{
				Name = "IsWeight",
				Caption = ArmSoft.SME.Common.Resources.IsWeight,
				Source = "IsWeight",
				Type = SupportedType.Boolean
			},
			new Column
			{
				Name = "LabelN",
				Caption = ArmSoft.SME.Trade.Common.Resources.PrintLabelN,
				Source = "LabelN"
			},
			new Column
			{
				Name = "BarCodeN",
				Caption = ArmSoft.SME.Trade.Common.Resources.PrintBarCodeN,
				Source = "BarCodeN"
			},
			new Column
			{
				Name = "BarCodePrefix",
				Caption = ArmSoft.SME.Trade.Common.Resources.Prefix,
				Source = "BarCodePrefix",
				Type = SupportedType.IntegerNullable
			},
			new Column
			{
				Name = "Ingredients",
				Caption = ArmSoft.SME.Common.Resources.Specification,
				Source = "Ingredients"
			},
			new Column
			{
				Name = "Exported",
				Caption = ArmSoft.SME.Common.Resources.Exported,
				Source = "Exported",
				Type = SupportedType.Boolean
			},
			new Column
			{
				Name = "ErrorComment",
				Caption = ArmSoft.SME.Common.Resources.ErrorMessage,
				Source = "ErrorComment"
			}
		};
		schema.Columns = columns;
		return schema;
	}

	public override DataProviderResult<DataRow> Execute(Param param, ExecutionSettings settings, List<object> keys = null, bool withoutAccessCheck = false, HashSet<string> columns = null)
	{
		return new DataProviderResult<DataRow>
		{
			Rows = param.Rows
		};
	}
}
