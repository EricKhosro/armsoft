namespace ArmSoft.SME.Core.UI.Directories;

public interface IDirectoryDialogWindow
{
	int DVPId { get; set; }

	bool? ShowDialog();
}
