using System;
using System.Collections.Generic;

namespace ArmSoft.SME.Common.PersistableObject;

public class GridDefinition
{
	private readonly Func<string> _captionDelegate;

	public string Name { get; }

	public string Caption => _captionDelegate();

	public bool IsRequired { get; }

	public bool LogChanges { get; }

	public Dictionary<string, FieldDefinition> Columns { get; set; }

	public GridDefinition(string name, Func<string> captionDelegate, bool isRequired = false, bool logChanges = true, Func<string> logCaptionDelegate = null, string publicApiProperty = "")
	{
		Name = name;
		_captionDelegate = captionDelegate;
		IsRequired = isRequired;
		Columns = new Dictionary<string, FieldDefinition>(StringComparer.OrdinalIgnoreCase);
		LogChanges = logChanges;
	}
}
