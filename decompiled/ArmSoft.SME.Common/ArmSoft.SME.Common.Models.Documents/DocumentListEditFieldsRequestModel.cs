using System.Collections.Generic;

namespace ArmSoft.SME.Common.Models.Documents;

public class DocumentListEditFieldsRequestModel
{
	public List<DocumentWithEditFields> DocumentWithFields { get; set; }

	public StoreRequest StoreRequest { get; set; }
}
