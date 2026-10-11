namespace ArmSoft.SME.Core.UI.DataViews;

public class DirectoryLinkColumnData
{
	public string CodeColumnName { get; }

	public string CaptionColumnName { get; }

	public short DirectoryType { get; }

	public string DirectoryCaptionColumnName { get; }

	public string DirectoryCaption { get; }

	public bool ShowAsLink { get; }

	public bool ShowInContextMenu { get; }

	public DirectoryLinkColumnData(string codeColumnName, string captionColumnName, short directoryType, string directoryCaptionColumnName, string directoryCaption, bool showAsLink, bool showInContextMenu)
	{
		CodeColumnName = codeColumnName;
		CaptionColumnName = captionColumnName;
		DirectoryType = directoryType;
		DirectoryCaptionColumnName = directoryCaptionColumnName;
		DirectoryCaption = directoryCaption;
		ShowAsLink = showAsLink;
		ShowInContextMenu = showInContextMenu;
	}
}
