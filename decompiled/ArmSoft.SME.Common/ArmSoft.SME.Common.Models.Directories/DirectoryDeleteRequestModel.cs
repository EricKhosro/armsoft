namespace ArmSoft.SME.Common.Models.Directories;

public class DirectoryDeleteRequestModel
{
	public int Id { get; set; }

	public string Key { get; set; }

	public bool WithoutValidation { get; set; }

	public bool ThrowException { get; set; }
}
