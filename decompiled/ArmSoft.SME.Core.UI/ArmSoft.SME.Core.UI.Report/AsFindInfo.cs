using System.Collections.ObjectModel;

namespace ArmSoft.SME.Core.UI.Report;

public class AsFindInfo
{
	public ObservableCollection<string> History;

	public string Pattern { get; set; }

	public FindDirection SearchDirection { get; set; }

	public bool WholeWord { get; set; }

	public bool IgnoreCase { get; set; }

	public AsFindInfo()
	{
		Pattern = "";
		SearchDirection = FindDirection.Forward;
		WholeWord = false;
		IgnoreCase = true;
		History = new ObservableCollection<string>();
	}
}
