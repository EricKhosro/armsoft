namespace ArmSoft.SME.Common.Models.Directories;

public class Directory : PersistableObject
{
	public int Id { get; set; }

	public string Code { get; set; }

	public string Name { get; set; }

	public bool IsClosed { get; set; }
}
