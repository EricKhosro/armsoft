using System.Collections.Generic;
using ArmSoft.SME.Common.Models;
using ArmSoft.SME.Common.Models.DPR;
using ArmSoft.SME.Common.Models.Documents;

namespace ArmSoft.SME.Client.Models;

public class CreateDocumentResponseModel
{
	public int CreatedDocsCount { get; set; }

	public int EditedDocsCount { get; set; }

	public List<DocumentsCountByType> CreatedDocsCountByType { get; set; }

	public List<Document> Documents { get; set; }

	public List<SimpleExceptionModel> Exceptions { get; set; }
}
