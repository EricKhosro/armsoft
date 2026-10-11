namespace ArmSoft.SME.Core.AddIn.Client.Models;

public class Database
{
	public int Id { get; set; }

	public string Name { get; set; }

	public string Caption { get; set; }

	public bool IsBlocked { get; set; }

	public bool DisableOldUI { get; set; }

	public string ComboView => Name + " (" + Caption + ")";
}
