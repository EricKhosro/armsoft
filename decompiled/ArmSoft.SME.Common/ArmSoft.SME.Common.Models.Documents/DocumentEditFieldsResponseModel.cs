using System.Collections.Generic;

namespace ArmSoft.SME.Common.Models.Documents;

public class DocumentEditFieldsResponseModel
{
	public int SuccessCount { get; set; }

	public List<DocumentEditResponseModel> Exceptions { get; set; }
}
