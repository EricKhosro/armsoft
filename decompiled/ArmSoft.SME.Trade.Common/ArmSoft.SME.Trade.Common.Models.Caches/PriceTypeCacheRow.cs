using System.Text.Json;
using ArmSoft.SME.Common.Models.Caches;

namespace ArmSoft.SME.Trade.Common.Models.Caches;

public class PriceTypeCacheRow : CacheRowBase<string>
{
	public string Code { get; set; }

	public string Caption { get; set; }

	public string CurrCode { get; set; }

	public string Round { get; set; }

	public bool VAT { get; set; }

	public override string KeyJSON => JsonSerializer.Serialize(Code);

	public PriceTypeCacheRow()
	{
		Code = string.Empty;
		Caption = string.Empty;
		CurrCode = string.Empty;
		Round = string.Empty;
		VAT = false;
	}

	public PriceTypeCacheRow(string code, string caption, string currCode, string round, bool vat, byte[] ts)
	{
		Code = code;
		Caption = caption;
		CurrCode = currCode;
		Round = round;
		VAT = vat;
		TS = ts;
	}
}
