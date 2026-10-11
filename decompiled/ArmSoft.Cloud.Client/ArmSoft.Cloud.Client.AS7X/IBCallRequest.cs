namespace ArmSoft.Cloud.Client.AS7X;

public class IBCallRequest
{
	public string TaxCode { get; set; }

	public string BankCode { get; set; }

	public IBOperation Op { get; set; }

	public int Count { get; set; }
}
