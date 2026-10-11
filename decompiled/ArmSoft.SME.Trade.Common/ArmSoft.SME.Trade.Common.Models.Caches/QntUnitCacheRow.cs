using System.Text.Json;
using ArmSoft.SME.Common.Models.Caches;

namespace ArmSoft.SME.Trade.Common.Models.Caches;

public class QntUnitCacheRow : CacheRowBase<string>
{
	public string Code { get; set; }

	public string Caption { get; set; }

	public string Brief { get; set; }

	public int UnitID { get; set; }

	public override string KeyJSON => JsonSerializer.Serialize(Code);

	public QntUnitCacheRow()
	{
		Code = string.Empty;
		Caption = string.Empty;
		Brief = string.Empty;
		UnitID = 0;
	}

	public QntUnitCacheRow(string code, string caption, string brief, int unitID, byte[] TS)
	{
		Code = code;
		Caption = caption;
		Brief = brief;
		UnitID = unitID;
		base.TS = TS;
	}
}
