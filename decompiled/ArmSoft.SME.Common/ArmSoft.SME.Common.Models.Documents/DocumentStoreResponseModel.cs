using System.Collections.Generic;

namespace ArmSoft.SME.Common.Models.Documents;

public class DocumentStoreResponseModel
{
	public List<ExceptionModel> Exceptions { get; set; }

	public List<WarningModel> Warnings { get; set; }

	public string JSONDocument { get; set; }
}
