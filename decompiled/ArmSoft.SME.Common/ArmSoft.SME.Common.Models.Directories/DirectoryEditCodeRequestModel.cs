namespace ArmSoft.SME.Common.Models.Directories;

public class DirectoryEditCodeRequestModel
{
	public string Key { get; set; }

	public string NewCode { get; set; }

	public bool WithoutValidation { get; set; }
}
