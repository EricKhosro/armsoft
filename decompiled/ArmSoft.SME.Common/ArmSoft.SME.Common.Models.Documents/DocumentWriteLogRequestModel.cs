namespace ArmSoft.SME.Common.Models.Documents;

public class DocumentWriteLogRequestModel
{
	public byte State { get; set; }

	public byte Op { get; set; }

	public string Comment { get; set; }
}
