using System.Collections.Generic;

namespace ArmSoft.SME.Trade.Common.Models.Documents;

public class MTMoveWithConfirmationBase : MTMoveBase
{
	public List<PartyRow> MTExpDocSelectedPartiesList { get; set; } = new List<PartyRow>();

	public string BookNum { get; set; }

	public int BookPage { get; set; }

	public int BookLine { get; set; }

	public string TaxInvoiceSeries { get; set; }

	public string TaxInvoiceNumber { get; set; }

	public string TransportationMethod { get; set; }

	public string Chief { get; set; }

	public string Mediator { get; set; }

	public string Allower { get; set; }

	public string PriceType { get; set; }

	public bool MotorVehicle { get; set; }

	public string Brand { get; set; }

	public string LicensePlate { get; set; }
}
