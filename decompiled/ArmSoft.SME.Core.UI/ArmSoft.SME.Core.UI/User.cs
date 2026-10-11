using System.Windows.Media;

namespace ArmSoft.SME.Core.UI;

public class User
{
	public string Name { get; set; } = "";

	public string Mail { get; set; } = "";

	public ImageSource Image { get; set; }

	public string Other { get; set; } = "";

	public string OtherVisible { get; set; } = "";

	public string UserVisible { get; set; } = "";
}
