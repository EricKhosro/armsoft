using ArmSoft.SME.Common.Models;

namespace ArmSoft.SME.Trade.Common.Models;

public class PriceFormationResponseModel
{
	public string MTCode { get; set; }

	public string Unit { get; set; }

	public ExceptionModel Exception { get; set; }
}
