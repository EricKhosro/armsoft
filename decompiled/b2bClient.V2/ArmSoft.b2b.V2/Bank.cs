namespace ArmSoft.b2b.V2;

public class Bank : CodeName
{
	public string SwiftCode { get; set; }

	public int? SalesPointId { get; set; }

	public StateEnum State { get; set; }
}
