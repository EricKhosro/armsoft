using System.Collections.Generic;
using ArmSoft.SME.Common.Models;
using ArmSoft.SME.Common.Models.Documents;

namespace ArmSoft.SME.Client.Models.Documents;

public class DocumentStoreResponseModelBase
{
	public List<ExceptionModel> Exceptions { get; set; }

	public List<WarningModel> Warnings { get; set; }

	public Document Document { get; set; }
}
