using System;

namespace ArmSoft.SME.Common.PersistableObject;

public class FieldDefinition
{
	private readonly Func<string> _captionDelegate;

	private readonly Func<string> _logCaptionDelegate;

	public string Name { get; }

	public string Caption => _captionDelegate();

	public string LogCaption
	{
		get
		{
			if (_logCaptionDelegate == null)
			{
				return Caption;
			}
			return _logCaptionDelegate();
		}
	}

	public bool IsRequired { get; set; }

	public bool LogChanges { get; }

	public FieldDefinition(string name, Func<string> captionDelegate, bool isRequired = false, bool logChanges = true, Func<string> logCaptionDelegate = null)
	{
		Name = name;
		_captionDelegate = captionDelegate;
		IsRequired = isRequired;
		LogChanges = logChanges;
		_logCaptionDelegate = logCaptionDelegate;
	}
}
