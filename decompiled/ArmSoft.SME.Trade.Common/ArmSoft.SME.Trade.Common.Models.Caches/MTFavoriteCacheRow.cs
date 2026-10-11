using System.Text.Json;
using ArmSoft.SME.Common.Models.Caches;

namespace ArmSoft.SME.Trade.Common.Models.Caches;

public class MTFavoriteCacheRow : CacheRowBase<MTFavoriteCacheRow.RowKey>
{
	public class RowKey
	{
		public int Id { get; set; }

		public string UnitCode { get; set; }

		public RowKey()
		{
		}

		public RowKey(int id, string unitCode)
		{
			Id = id;
			UnitCode = unitCode;
		}

		public override int GetHashCode()
		{
			return (UnitCode + Id).GetHashCode();
		}

		public override bool Equals(object obj)
		{
			return Equals(obj as RowKey);
		}

		public bool Equals(RowKey obj)
		{
			if (obj != null)
			{
				if (obj.Id == Id)
				{
					return obj.UnitCode == UnitCode;
				}
				return false;
			}
			return false;
		}
	}

	public int Id { get; set; }

	public string UnitCode { get; set; }

	public string Group { get; set; }

	public string HotKey { get; set; }

	public int OrderNum { get; set; }

	public override string KeyJSON => JsonSerializer.Serialize(new RowKey
	{
		Id = Id,
		UnitCode = UnitCode
	});

	public MTFavoriteCacheRow()
	{
		Id = 0;
		UnitCode = string.Empty;
		Group = string.Empty;
		HotKey = string.Empty;
		OrderNum = 0;
	}

	public MTFavoriteCacheRow(int id, string unitCode, string group, string hotKey, int orderNum, byte[] ts)
	{
		Id = id;
		UnitCode = unitCode;
		Group = group;
		HotKey = hotKey;
		OrderNum = orderNum;
		TS = ts;
	}
}
