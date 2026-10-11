using System.Text.Json;
using ArmSoft.SME.Common.Models.Caches;

namespace ArmSoft.SME.Trade.Common.Models.Caches;

public class PropertyListCacheRow : CacheRowBase<PropertyListCacheRow.RowKey>
{
	public class RowKey
	{
		public string DestCode { get; set; }

		public string Code { get; set; }

		public RowKey()
		{
		}

		public RowKey(string destCode, string code)
		{
			DestCode = destCode;
			Code = code;
		}

		public override int GetHashCode()
		{
			return (DestCode + Code).GetHashCode();
		}

		public override bool Equals(object obj)
		{
			return Equals(obj as RowKey);
		}

		public bool Equals(RowKey obj)
		{
			if (obj != null)
			{
				if (obj.DestCode == DestCode)
				{
					return obj.Code == Code;
				}
				return false;
			}
			return false;
		}
	}

	public string DestCode { get; set; }

	public string Code { get; set; }

	public string Caption { get; set; }

	public override string KeyJSON => JsonSerializer.Serialize(new RowKey
	{
		DestCode = DestCode,
		Code = Code
	});

	public PropertyListCacheRow()
	{
	}

	public PropertyListCacheRow(string destCode, string code, string caption, byte[] ts)
	{
		DestCode = destCode;
		Code = code;
		Caption = caption;
		TS = ts;
	}
}
