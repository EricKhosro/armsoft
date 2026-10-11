using ArmSoft.SME.Common.Directories;
using ArmSoft.SME.Trade.Common.SmartEnums;

namespace ArmSoft.SME.Trade.Common.Directories.Definition;

public class TradeDirectoryDefinition : DirectoryDefinition
{
	public TradeDirectoryDefinition(DirectoryTypeSE dirType)
	{
		POType = dirType.Value;
		CaptionDelegate = dirType.CaptionDelegate;
		FullCaptionDelegate = dirType.FullCaptionDelegate;
	}
}
