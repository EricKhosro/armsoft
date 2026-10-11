using ArmSoft.SME.Common.Enums;

namespace ArmSoft.SME.Common.MetaData;

public class TreeDefinition
{
	public string Name { get; set; }

	public string CaptionId { get; set; }

	public string NodeCaptionId { get; set; }

	public string CodeCaptionId { get; set; } = "";

	public short CodeLen { get; set; }

	public bool Multilevel { get; set; }

	public bool PrefixBased { get; set; }

	public short CaptionLen { get; set; } = 50;

	public bool DocumentBased { get; set; }

	public bool System { get; set; }

	public bool IsLogAvailable { get; set; }

	public bool IsClosedAvailable { get; set; }

	public AttachType AttachmentsTypes { get; set; }

	public string SpecCaptionId { get; set; } = "";

	public short SpecLen { get; set; }

	public int NodesCount { get; set; }

	public short NextCodeParamId { get; set; }

	public short CacheId { get; set; }
}
