namespace ArmSoft.TaxService.Models;

public class TINInfo
{
	public string VatNumber { get; set; } = string.Empty;

	public string VatNumberForDate { get; set; } = string.Empty;

	public string Name { get; set; } = string.Empty;

	public string Address { get; set; } = string.Empty;

	public string Status { get; set; } = string.Empty;

	public string TIN { get; set; } = string.Empty;

	public string OrganizationalType { get; set; } = string.Empty;

	public string ExitCode { get; set; } = string.Empty;

	public string StatusMessage => DecodeTinStatus(Status);

	public static string DecodeTinStatus(string Status)
	{
		return Status switch
		{
			"00" => Resources.Get(ResourceKey.NewRegistered), 
			"01" => Resources.Get(ResourceKey.Active), 
			"02" => Resources.Get(ResourceKey.Bankrupt), 
			"10" => Resources.Get(ResourceKey.Dissolved), 
			"12" => Resources.Get(ResourceKey.Died), 
			"13" => Resources.Get(ResourceKey.IncorrectlyProvided), 
			"14" => Resources.Get(ResourceKey.DissolvedMerged), 
			"15" => Resources.Get(ResourceKey.DissolvedUnited), 
			"18" => Resources.Get(ResourceKey.TimedSuspended), 
			"19" => Resources.Get(ResourceKey.DissolvedProcess), 
			"20" => Resources.Get(ResourceKey.ExtractedFromAccounting), 
			"21" => Resources.Get(ResourceKey.InRiskOfBankruptcy), 
			_ => Resources.Get(ResourceKey.Unknown), 
		};
	}
}
