namespace ArmSoft.Cloud.Client.AS7X;

public class AvailableDatabase
{
	public int Id { get; set; }

	public string Key { get; set; }

	public string Name { get; set; }

	public string Caption { get; set; }

	public bool PSMode { get; set; }

	public bool IsBlocked { get; set; }

	public string Service { get; set; }

	public string DBService { get; set; }

	public bool DisableOldUI { get; set; }

	public bool AllowNewUI { get; set; }
}
