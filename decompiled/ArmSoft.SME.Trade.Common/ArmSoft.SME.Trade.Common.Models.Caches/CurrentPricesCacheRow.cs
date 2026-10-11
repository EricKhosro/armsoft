using System.Text.Json;
using ArmSoft.SME.Common.Models.Caches;

namespace ArmSoft.SME.Trade.Common.Models.Caches;

public class CurrentPricesCacheRow : CacheRowBase<CurrentPricesCacheRow.RowKey>
{
	public record RowKey
	{
		public int MTID { get; set; }

		public string Unit { get; set; }

		public string PriceType { get; set; }

		public RowKey()
		{
		}

		public RowKey(CurrentPricesCacheRow priceInfo)
		{
			MTID = priceInfo.MTID;
			Unit = priceInfo.Unit;
			PriceType = priceInfo.PriceType;
		}
	}

	public int MTID { get; set; }

	public string Unit { get; set; }

	public string PriceType { get; set; }

	public decimal Price { get; set; }

	public override string KeyJSON => JsonSerializer.Serialize(new RowKey
	{
		MTID = MTID,
		Unit = Unit,
		PriceType = PriceType
	});

	public CurrentPricesCacheRow()
	{
		MTID = 0;
		Unit = string.Empty;
		PriceType = string.Empty;
		Price = 0m;
	}

	public CurrentPricesCacheRow(int mtID, string unit, string priceType, decimal price, byte[] ts)
	{
		MTID = mtID;
		Unit = unit;
		PriceType = priceType;
		Price = price;
		TS = ts;
	}
}
