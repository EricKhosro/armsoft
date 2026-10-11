using System.Collections.Generic;
using ArmSoft.SME.Trade.Common.Models.DPR;

namespace ArmSoft.SME.Trade.Common.Models;

public class PriceFormationRequestModel
{
	public List<PriceFormationRequestProperties> Properties { get; set; }

	public bool WithoutValidation { get; set; }
}
