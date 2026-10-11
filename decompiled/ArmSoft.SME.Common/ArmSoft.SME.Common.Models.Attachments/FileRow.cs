namespace ArmSoft.SME.Common.Models.Attachments;

public class FileRow : BaseRow
{
	public string FileName { get; set; } = "";

	public string FileNameWithPath { get; set; } = "";

	public decimal Size { get; set; }

	public FileRow Clone()
	{
		return new FileRow
		{
			Description = Description,
			FileName = FileName,
			FileNameWithPath = FileNameWithPath,
			Size = Size,
			IsNew = IsNew
		};
	}
}
