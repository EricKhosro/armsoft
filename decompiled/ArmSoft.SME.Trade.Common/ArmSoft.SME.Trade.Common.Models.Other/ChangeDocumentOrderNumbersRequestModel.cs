using System.Collections.Generic;
using ArmSoft.SME.Trade.Common.Models.DPR;
using ArmSoft.SME.Trade.Common.SmartEnums;

namespace ArmSoft.SME.Trade.Common.Models.Other;

public class ChangeDocumentOrderNumbersRequestModel
{
	public List<ChangeDocumentOrderNumbersProperties> Properties { get; set; }

	public ChangeDocOrderNumTypesSE OpType { get; set; }
}
