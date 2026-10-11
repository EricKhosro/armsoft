namespace ArmSoft.SME.Core.AddIn.Client.Models;

public class AvailableDatabase
{
	public string Name { get; set; }

	public string Caption { get; set; }

	public int DbId { get; set; }

	public string ServiceAddress { get; set; }

	public string ComboView => Name + " (" + Caption + ")";
}
