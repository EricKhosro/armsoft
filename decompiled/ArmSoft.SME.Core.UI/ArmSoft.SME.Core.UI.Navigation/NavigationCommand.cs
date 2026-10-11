using ArmSoft.SME.Common.MetaData;

namespace ArmSoft.SME.Core.UI.Navigation;

public class NavigationCommand
{
	public short Command { get; set; }

	public short Subsystem { get; set; }

	public AWPCommandType CommandType { get; set; }

	public string CommandTag { get; set; } = "";
}
