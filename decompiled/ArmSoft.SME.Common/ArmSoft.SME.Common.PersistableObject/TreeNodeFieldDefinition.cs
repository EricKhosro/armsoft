using System;

namespace ArmSoft.SME.Common.PersistableObject;

public class TreeNodeFieldDefinition : FieldDefinition
{
	public string TreeId { get; }

	public bool OnlyLeaf { get; }

	public TreeNodeFieldDefinition(string name, Func<string> captionDelegate, string treeId, bool onlyLeaf = true, bool isRequired = false, bool logChanges = true, Func<string> logCaptionDelegate = null)
		: base(name, captionDelegate, isRequired, logChanges, logCaptionDelegate)
	{
		TreeId = treeId;
		OnlyLeaf = onlyLeaf;
	}
}
