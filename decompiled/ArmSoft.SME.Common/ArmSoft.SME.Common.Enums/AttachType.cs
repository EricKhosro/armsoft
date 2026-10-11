using System;

namespace ArmSoft.SME.Common.Enums;

[Flags]
public enum AttachType : byte
{
	None = 0,
	File = 1,
	Link = 2,
	Doc = 4,
	All = File | Link | Doc
}
