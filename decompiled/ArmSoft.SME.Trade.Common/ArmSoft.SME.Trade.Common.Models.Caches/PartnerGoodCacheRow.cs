using System.Text.Json;
using System.Text.Json.Serialization;
using ArmSoft.SME.Common.Models.Caches;

namespace ArmSoft.SME.Trade.Common.Models.Caches;

public class PartnerGoodCacheRow : CacheRowBase<PartnerGoodCacheRow.RowKey>
{
	public class RowKey
	{
		public int PartID { get; set; }

		public string PartItemCode { get; set; }

		public string PartItemUnit { get; set; }

		public RowKey()
		{
		}

		public RowKey(int partId, string partItemCode, string partItemUnit)
		{
			PartID = partId;
			PartItemCode = partItemCode;
			PartItemUnit = partItemUnit;
		}

		public override int GetHashCode()
		{
			return (PartItemCode + PartItemUnit + PartID).GetHashCode();
		}

		public override bool Equals(object obj)
		{
			return Equals(obj as RowKey);
		}

		public bool Equals(RowKey obj)
		{
			if (obj != null && obj.PartID == PartID && obj.PartItemCode == PartItemCode)
			{
				return obj.PartItemUnit == PartItemUnit;
			}
			return false;
		}
	}

	public override string KeyJSON => JsonSerializer.Serialize(new RowKey
	{
		PartID = PartID,
		PartItemCode = PartItemCode,
		PartItemUnit = PartItemUnit
	});

	public int PartID { get; set; }

	public string PartItemCode { get; set; }

	public string PartItemUnit { get; set; }

	public decimal Coeff { get; set; }

	public int MTID { get; set; }

	public string Unit { get; set; }

	public PartnerGoodCacheRow()
	{
	}

	[JsonConstructor]
	public PartnerGoodCacheRow(int partID, string partItemCode, string partItemUnit, decimal coeff, int mtID, string unit, byte[] ts)
	{
		PartID = partID;
		PartItemCode = partItemCode;
		PartItemUnit = partItemUnit;
		Coeff = coeff;
		MTID = mtID;
		Unit = unit;
		TS = ts;
	}
}
