namespace ArmSoft.SME.Trade.Client.Models;

public class NonCashInfo
{
	public int Type { get; set; }

	public string Token { get; set; }

	public string MerchantID { get; set; }

	public string Partner { get; set; }

	public string Contract { get; set; }

	public string Caption { get; set; }

	public string Device { get; set; }

	public bool StaticQR { get; set; }

	public string Branch { get; set; }

	public string Cashier { get; set; }

	public bool IsSellerScans { get; set; }

	public string POSTerminalID { get; set; }

	public short ECRPaymentMethod { get; set; }
}
