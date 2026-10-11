namespace ArmSoft.b2b.V2;

public class ProductRequest
{
	public int ActorId { get; set; }

	public bool IncludeClosed { get; set; }

	public bool IncludeUnits { get; set; }
}
