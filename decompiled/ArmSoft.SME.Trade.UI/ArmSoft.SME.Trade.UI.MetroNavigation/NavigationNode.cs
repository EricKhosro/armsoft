using System.Windows.Media;
using ArmSoft.SME.Core.UI.SmartEnums;
using ArmSoft.SME.Trade.Common.SmartEnums;

namespace ArmSoft.SME.Trade.UI.MetroNavigation;

public class NavigationNode
{
	public UserAccessIdSE Command { get; set; }

	public string Header { get; set; }

	public ImageSource Image { get; set; }

	public FeatureStatusTypeSE TreeNodeType { get; set; }
}
