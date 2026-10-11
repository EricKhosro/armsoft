using System;
using System.Collections.Generic;
using ArmSoft.SME.Common.Models.SmartEnums;

namespace ArmSoft.SME.Common.PersistableObject;

public class SmartEnumFieldDefinition : FieldDefinition
{
	public Type SmartEnumType { get; }

	public ValueMember ValueMember { get; }

	public List<string> Values { get; }

	public bool PadLeftInUI { get; }

	public char PaddingChar { get; }

	public SmartEnumFieldDefinition(string name, Func<string> captionDelegate, Type smartEnumType, bool isRequired = false, bool logChanges = true, ValueMember valueMember = ValueMember.Code, List<string> values = null, Func<string> logCaptionDelegate = null, string publicApiProperty = "", bool padLeftInUI = false, char paddingChar = '0')
		: base(name, captionDelegate, isRequired, logChanges, logCaptionDelegate)
	{
		SmartEnumType = smartEnumType;
		ValueMember = valueMember;
		Values = values;
		PadLeftInUI = padLeftInUI;
		PaddingChar = paddingChar;
	}
}
