using System.Collections.Generic;
using ArmSoft.SME.Common.PersistableObject;
using ArmSoft.SME.Common.SmartEnums;

namespace ArmSoft.SME.Common.Directories;

public class DirectoryDefinition : PODefinition
{
	public short? NextCodeParamId { get; set; }

	public bool IsClosedAvailable { get; set; }

	public HashSet<short> RelatedCacheIds { get; set; }

	public DirectoryDefinition()
	{
		POTypeDef = POTypeSE.Directory;
	}

	public DirectoryDefinition(DirectoryTypeSE dirType)
	{
		POTypeDef = POTypeSE.Directory;
		POType = dirType.Value;
		CaptionDelegate = dirType.CaptionDelegate;
		FullCaptionDelegate = dirType.FullCaptionDelegate;
	}
}
