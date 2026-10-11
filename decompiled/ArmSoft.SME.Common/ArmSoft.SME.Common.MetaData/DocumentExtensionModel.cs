using System.Collections.Generic;
using ArmSoft.SME.Common.Models.Documents;

namespace ArmSoft.SME.Common.MetaData;

public class DocumentExtensionModel : DocumentExtensionModelBase
{
	public string Description { get; set; }

	public List<DocumentValidatorExpressionRow> DocumentValidators { get; set; }
}
