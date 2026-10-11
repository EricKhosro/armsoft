namespace ArmSoft.SME.Trade.AddIn.Client.Models;

public class PartnersMT : ExchangeBase
{
	public string PartnerCode { get; set; }

	public string Type { get; set; }

	public string PartnerItemCode { get; set; }

	public string PartnerItemName { get; set; }

	public string PartnerItemUnitName { get; set; }

	public decimal Coeff { get; set; }

	public string ItemCode { get; set; }

	public string ItemUnitCode { get; set; }
}
