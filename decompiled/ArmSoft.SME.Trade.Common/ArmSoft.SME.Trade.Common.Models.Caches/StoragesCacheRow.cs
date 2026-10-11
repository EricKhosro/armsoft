using System.Text.Json;
using ArmSoft.SME.Common.Models.Caches;

namespace ArmSoft.SME.Trade.Common.Models.Caches;

public class StoragesCacheRow : CacheRowBase<string>
{
	public string Code { get; set; }

	public string Caption { get; set; }

	public string Response { get; set; }

	public string Address { get; set; }

	public bool Retail { get; set; }

	public bool IsDefault { get; set; }

	public override string KeyJSON => JsonSerializer.Serialize(Code);

	public StoragesCacheRow(string code, string caption, string response, string address, bool retail, bool isDefault, byte[] ts)
	{
		Code = code;
		Caption = caption;
		Response = response;
		Address = address;
		Retail = retail;
		IsDefault = isDefault;
		TS = ts;
	}

	public StoragesCacheRow()
	{
	}
}
