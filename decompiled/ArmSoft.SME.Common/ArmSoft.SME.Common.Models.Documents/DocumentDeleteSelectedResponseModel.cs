using System.Collections.Generic;

namespace ArmSoft.SME.Common.Models.Documents;

public class DocumentDeleteSelectedResponseModel
{
	public int SuccessCount { get; set; }

	public List<DocumentDeleteResponseModel> Exceptions { get; set; }
}
