using System.Collections.Generic;
using ArmSoft.SME.Common.PersistableObject;
using ArmSoft.SME.Common.SmartEnums;

namespace ArmSoft.SME.Common.Documents;

public class DocDefinition : PODefinition
{
	public bool AllowStoreAsDraft { get; set; } = true;

	public short? NextDocNumParamId { get; set; }

	public bool SetEmptyDocNumAutomatic { get; set; } = true;

	public bool SetDuplicateDocNumAutomatic { get; set; }

	public bool IsDocNumberUnique { get; set; }

	public bool IsDocNumRequired { get; set; } = true;

	public HashSet<short> BaseDocTypes { get; set; }

	public HashSet<short> ChildDocTypes { get; set; }

	public HashSet<short> WeakChildDocTypes { get; set; }

	public DocDefinition()
	{
		POTypeDef = POTypeSE.Document;
		AllowLog = true;
	}

	public DocDefinition(DocumentTypeSE docType)
	{
		POTypeDef = POTypeSE.Document;
		POType = docType.Value;
		CaptionDelegate = docType.CaptionDelegate;
		FullCaptionDelegate = docType.FullCaptionDelegate;
		AllowLog = true;
	}
}
