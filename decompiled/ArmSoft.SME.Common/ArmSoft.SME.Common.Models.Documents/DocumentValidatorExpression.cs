using System.Collections.Generic;

namespace ArmSoft.SME.Common.Models.Documents;

public class DocumentValidatorExpression : DocumentIdentificationModel
{
	public string Description { get; set; }

	public List<DocumentValidatorExpressionRow> DocumentValidators { get; set; }
}
