using System;
using ArmSoft.SME.Client;
using ArmSoft.SME.Client.Data;
using ArmSoft.SME.Trade.Common.Models.Caches;

namespace ArmSoft.SME.Trade.Client.Data.Reports;

public class GoodsRetailAvailabilityDataProvider : DataProvider<GoodsRetailAvailabilityDataProvider.DataRow, GoodsRetailAvailabilityDataProvider.Param>
{
	public class DataRow : ExtendableRow
	{
		public int GoodID { get; set; }

		public string GoodCode { get; set; }

		public string Caption { get; set; }

		public string Unit { get; set; }

		public string UnitBrief { get; set; }

		public decimal Quantity { get; set; }

		public decimal Price { get; set; }

		public decimal Sum { get; set; }

		public string Storage { get; set; }

		public string StorageCaption { get; set; }

		public string GroupCodeCaption { get; set; }

		public byte[] Image { get; set; }

		public bool ImageExists { get; set; }

		public string ImgStr => GoodID + "," + Unit.Trim();

		public MaterialCacheRow MaterialCacheRow { get; set; }

		public ProducerCacheRow ProducerCacheRow { get; set; }

		public override object ReferenceValue(string name, string source)
		{
			if (source.StartsWith("Material"))
			{
				if (MaterialCacheRow != null)
				{
					return MaterialCacheRow.GetReferenceValue(source);
				}
			}
			else if (source.StartsWith("Producer") && ProducerCacheRow != null)
			{
				return ProducerCacheRow.GetReferenceValue(source);
			}
			return null;
		}
	}

	public class Param
	{
		public string GoodCode { get; set; }

		public DateOnly RemDate { get; set; }

		public string Storage { get; set; }

		public string GoodGroup { get; set; }

		public bool ShowAltUnit { get; set; }

		public bool DetailByStorage { get; set; }
	}

	public override string Name => "GoodsRetailAvailability";

	public GoodsRetailAvailabilityDataProvider(TradeClient client)
		: base((ApiClient)client)
	{
	}
}
