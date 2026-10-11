namespace ArmSoft.SME.Common.Models.Documents;

public class DocumentValidatorExpressionRow
{
	public string FieldName { get; set; }

	public string FieldCaption { get; set; }

	public bool IsRequired { get; set; }

	public bool IsSystemRequired { get; set; }

	public string UserIds { get; set; }

	public bool Except { get; set; }

	public string Expression { get; set; }
}
