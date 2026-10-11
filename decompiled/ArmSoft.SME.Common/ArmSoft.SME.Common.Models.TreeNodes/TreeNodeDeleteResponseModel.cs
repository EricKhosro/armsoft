namespace ArmSoft.SME.Common.Models.TreeNodes;

public class TreeNodeDeleteResponseModel
{
	public string TreeId { get; set; }

	public string Code { get; set; }

	public ExceptionModel Exception { get; set; }
}
