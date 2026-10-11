using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows;
using ArmSoft.SME.Client;
using ArmSoft.SME.Client.Data;
using ArmSoft.SME.Client.Models.b2b;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Models.Caches;
using ArmSoft.SME.Common.Models.Data;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Trade.Common;
using ArmSoft.SME.Trade.Common.Models.Caches;
using ArmSoft.SME.Trade.Common.SmartEnums;

namespace ArmSoft.SME.Trade.UI.DataViewDataProvider;

public class B2BProductsCheckDataProvider : DataProvider<B2BProductsCheckDataProvider.DataRow, B2BProductsCheckDataProvider.Param>
{
	public class DataRow : ExtendableRow
	{
		public int Id { get; set; } = -1;

		public string Code { get; set; } = "";

		public string BarCode { get; set; } = "";

		public string Group { get; set; } = "";

		public string GroupName { get; set; } = "";

		public string CaptionDB { get; set; } = "";

		public string CaptionB2B { get; set; } = "";

		public bool CaptionDifferent { get; set; }

		public string CPACodeDB { get; set; } = "";

		public string CPACodeB2B { get; set; } = "";

		public bool CPACodeDifferent { get; set; }

		public string CPACaptionDB { get; set; } = "";

		public string CPACaptionB2B { get; set; } = "";

		public string UnitDB { get; set; } = "";

		public string UnitB2B { get; set; } = "";

		public bool UnitDifferent { get; set; }

		public bool VATDB { get; set; }

		public bool VATB2B { get; set; }

		public bool VATDifferent { get; set; }
	}

	public class Param
	{
		public string Group { get; set; }

		public bool CheckForCaption { get; set; }

		public bool CheckForUnit { get; set; }

		public bool CheckForCPA { get; set; }

		public bool CheckForVAT { get; set; }
	}

	public override string Name => "B2BProductsCheck";

	public B2BProductsCheckDataProvider()
		: base((ApiClient)null)
	{
	}

	public override Schema GetSchema()
	{
		Schema schema = new Schema();
		schema.Name = Name;
		schema.Caption = ArmSoft.SME.Trade.Common.Resources.DetectedVariations;
		int num = 12;
		List<Column> list = new List<Column>(num);
		CollectionsMarshal.SetCount(list, num);
		Span<Column> span = CollectionsMarshal.AsSpan(list);
		span[0] = new Column
		{
			Name = "Code",
			Caption = ArmSoft.SME.Common.Resources.Code,
			Source = "Code",
			Type = SupportedType.String
		};
		span[1] = new Column
		{
			Name = "BarCode",
			Caption = ArmSoft.SME.Common.Resources.BarCode,
			Source = "BarCode",
			Type = SupportedType.String
		};
		span[2] = new Column
		{
			Name = "Group",
			Caption = ArmSoft.SME.Common.Resources.Group,
			Source = "Group",
			Type = SupportedType.String
		};
		span[3] = new Column
		{
			Name = "GroupName",
			Caption = ArmSoft.SME.Common.Resources.GroupName,
			Source = "GroupName",
			Type = SupportedType.String
		};
		span[4] = new Column
		{
			Name = "CaptionDB",
			Caption = ArmSoft.SME.Common.Resources.Name + " " + ArmSoft.SME.Common.Resources.InDB,
			Source = "CaptionDB",
			Type = SupportedType.String
		};
		span[5] = new Column
		{
			Name = "CaptionB2B",
			Caption = ArmSoft.SME.Common.Resources.Name + " " + ArmSoft.SME.Common.Resources.InB2B,
			Source = "CaptionB2B",
			Type = SupportedType.String
		};
		span[6] = new Column
		{
			Name = "CPACodeDB",
			Caption = ArmSoft.SME.Common.Resources.CPACodeShort + " " + ArmSoft.SME.Common.Resources.InDB,
			Source = "CPACodeDB",
			Type = SupportedType.String
		};
		span[7] = new Column
		{
			Name = "CPACodeB2B",
			Caption = ArmSoft.SME.Common.Resources.CPACodeShort + " " + ArmSoft.SME.Common.Resources.InB2B,
			Source = "CPACodeB2B",
			Type = SupportedType.String
		};
		span[8] = new Column
		{
			Name = "UnitDB",
			Caption = ArmSoft.SME.Common.Resources.QuantityUnitShort + " " + ArmSoft.SME.Common.Resources.InDB,
			Source = "UnitDB",
			Type = SupportedType.String
		};
		span[9] = new Column
		{
			Name = "UnitB2B",
			Caption = ArmSoft.SME.Common.Resources.QuantityUnitShort + " " + ArmSoft.SME.Common.Resources.InB2B,
			Source = "UnitB2B",
			Type = SupportedType.String
		};
		span[10] = new Column
		{
			Name = "VATDB",
			Caption = ArmSoft.SME.Common.Resources.OptionsVAT + " " + ArmSoft.SME.Common.Resources.InDB,
			Source = "VATDB",
			Type = SupportedType.Boolean
		};
		span[11] = new Column
		{
			Name = "VATB2B",
			Caption = ArmSoft.SME.Common.Resources.OptionsVAT + " " + ArmSoft.SME.Common.Resources.InB2B,
			Source = "VATB2B",
			Type = SupportedType.Boolean
		};
		schema.Columns = list;
		schema.Parameters = new _003C_003Ez__ReadOnlyArray<Parameter>(new Parameter[5]
		{
			new Parameter("Group", ArmSoft.SME.Common.Resources.Group, SupportedType.String),
			new Parameter("CheckForCaption", ArmSoft.SME.Common.Resources.Name, SupportedType.Boolean),
			new Parameter("CheckForUnit", ArmSoft.SME.Common.Resources.QuantityUnitShort, SupportedType.Boolean),
			new Parameter("CheckForCPA", ArmSoft.SME.Common.Resources.CPACodeShort, SupportedType.Boolean),
			new Parameter("CheckForVAT", ArmSoft.SME.Common.Resources.OptionsVAT, SupportedType.Boolean)
		});
		return schema;
	}

	public override DataProviderResult<DataRow> Execute(Param param, ExecutionSettings settings, List<object> keys = null, bool withoutAccessCheck = false, HashSet<string> columns = null)
	{
		DataProviderResult<DataRow> dataProviderResult = new DataProviderResult<DataRow>
		{
			Rows = new List<DataRow>()
		};
		Dictionary<int, DataRow> dictionary = new Dictionary<int, DataRow>();
		Dictionary<string, int> dictionary2 = new Dictionary<string, int>();
		IReadOnlyList<QntUnitCacheRow> list = TradeSettings.TradeCacheService.QntUnit.GetList((QntUnitCacheRow x) => x.Code == "303" || x.Code == "001");
		foreach (MaterialCacheRow item2 in TradeSettings.TradeCacheService.Material.GetList((MaterialCacheRow x) => x.Type == GoodsTypesSE.Good.Value && (string.IsNullOrWhiteSpace(param.Group) || x.GroupCode == param.Group)))
		{
			DataRow dataRow = new DataRow
			{
				Code = item2.MTCode,
				Group = item2.GroupCode,
				GroupName = item2.GroupRow.Caption,
				CaptionDB = item2.Caption,
				UnitDB = item2.UnitRow.Brief,
				CPACodeDB = item2.CPACode,
				VATDB = item2.VAT
			};
			CPACodesCacheRow item = ArmSoft.SME.Core.UI.Settings.CoreCacheService.CPACode.GetItem(dataRow.CPACodeDB);
			if (item != null)
			{
				dataRow.CPACaptionDB = item.Name;
			}
			List<MaterialCacheRow.BarCodeRow> barCodes = TradeSettings.TradeCacheService.Material.GetBarCodes(item2, item2.Unit);
			if (barCodes == null || barCodes.Count == 0)
			{
				continue;
			}
			foreach (MaterialCacheRow.BarCodeRow item3 in barCodes)
			{
				dataRow.BarCode = item3.BarCode;
				dictionary2.Add(item3.BarCode, item2.Id);
			}
			dictionary.Add(item2.Id, dataRow);
		}
		List<b2bUnitedProductRowBase> goodsListFromBarCodes = BarcodeFromb2bCloudDialogWindow.GetGoodsListFromBarCodes(dictionary2.Keys.ToList());
		if (goodsListFromBarCodes != null)
		{
			HashSet<int> hashSet = new HashSet<int>();
			foreach (b2bUnitedProductRowBase b2bProduct in goodsListFromBarCodes)
			{
				if (dictionary2.TryGetValue(b2bProduct.Barcode, out var value) && !hashSet.Contains(value))
				{
					DataRow dataRow2 = dictionary[value];
					dataRow2.BarCode = b2bProduct.Barcode;
					dataRow2.CaptionB2B = b2bProduct.Name;
					QntUnitCacheRow qntUnitCacheRow = list.FirstOrDefault((QntUnitCacheRow x) => x.Code == (b2bProduct.IsWeight ? "303" : "001"));
					dataRow2.UnitB2B = ((qntUnitCacheRow == null) ? "" : qntUnitCacheRow.Caption);
					dataRow2.CPACodeB2B = b2bProduct.CPACode;
					dataRow2.CPACaptionB2B = b2bProduct.CPACaption;
					dataRow2.VATB2B = b2bProduct.IsVatTaxable;
					hashSet.Add(value);
				}
			}
		}
		foreach (DataRow value2 in dictionary.Values)
		{
			value2.CaptionDifferent = param.CheckForCaption && string.Compare(value2.CaptionDB.Trim(), value2.CaptionB2B.Trim(), ignoreCase: true) != 0;
			value2.UnitDifferent = param.CheckForUnit && string.Compare(value2.UnitDB.Trim(), value2.UnitB2B.Trim(), ignoreCase: true) != 0;
			value2.CPACodeDifferent = param.CheckForCPA && string.Compare(value2.CPACodeDB.Trim(), value2.CPACodeB2B.Trim(), ignoreCase: true) != 0;
			value2.VATDifferent = param.CheckForVAT && value2.VATDB != value2.VATB2B;
			if (value2.CaptionDifferent || value2.UnitDifferent || value2.CPACodeDifferent || value2.VATDifferent)
			{
				dataProviderResult.Rows.Add(value2);
			}
		}
		if (dataProviderResult.Rows.Count == 0)
		{
			throw new ASException(ArmSoft.SME.Trade.Common.Resources.VarianceNotExist, Convert.ToInt32(MessageBoxImage.Asterisk));
		}
		return dataProviderResult;
	}
}
