namespace ArmSoft.SME.Trade.Client.Routes;

public class DataFromInvoiceListRow
{
	public decimal Quantity { get; set; }

	public decimal SalePrice { get; set; }

	public decimal SaleSumm { get; set; }

	public bool VAT { get; set; }

	public string DealType { get; set; }
}
