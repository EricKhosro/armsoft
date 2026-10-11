using System.Collections.Generic;
using ArmSoft.SME.Common.Models.ASReport;

namespace ArmSoft.SME.Common.Models;

public class TaxServiceAccDocsRequest
{
	public List<AccDoc> AccDocs { get; set; }

	public bool AssignMTMoveInvNums { get; set; }
}
