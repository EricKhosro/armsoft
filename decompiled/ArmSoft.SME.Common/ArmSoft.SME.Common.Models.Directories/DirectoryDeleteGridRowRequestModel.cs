namespace ArmSoft.SME.Common.Models.Directories;

public class DirectoryDeleteGridRowRequestModel
{
	public string Key { get; set; }

	public string GridName { get; set; }

	public string RowJSON { get; set; }

	public byte[] TS { get; set; }
}
