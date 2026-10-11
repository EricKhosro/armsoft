using System.Collections.Generic;

namespace ArmSoft.SME.Common.Models.DPR;

public class CreateDocumentFromSelectedRowsResponse
{
	public int CreatedDocsCount { get; set; }

	public int EditedDocsCount { get; set; }

	public List<string> JSONDocuments { get; set; }

	public List<SimpleExceptionModel> Exceptions { get; set; }

	public List<DocumentsCountByType> CreatedDocsCountByType { get; set; }
}
