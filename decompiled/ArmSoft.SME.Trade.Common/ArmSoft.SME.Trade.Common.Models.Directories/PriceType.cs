namespace ArmSoft.SME.Trade.Common.Models.Directories;

public class PriceType : TradeDirectory
{
	public string Curr { get; set; }

	public string RoundDegree { get; set; }

	public bool IsWithInVAT { get; set; }

	public bool PriceTypeExistenceIntoContracts { get; set; }

	public bool PriceTypeExistenceIntoPrices { get; set; }
}
