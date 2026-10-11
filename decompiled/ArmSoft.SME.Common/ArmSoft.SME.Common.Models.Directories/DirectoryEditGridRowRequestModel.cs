namespace ArmSoft.SME.Common.Models.Directories;

public class DirectoryEditGridRowRequestModel
{
	public string Key { get; set; }

	public string GridName { get; set; }

	public string OldRowJSON { get; set; }

	public string NewRowJSON { get; set; }

	public byte[] TS { get; set; }
}
