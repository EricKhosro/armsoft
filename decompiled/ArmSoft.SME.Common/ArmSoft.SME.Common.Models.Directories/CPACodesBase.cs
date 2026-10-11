using System;

namespace ArmSoft.SME.Common.Models.Directories;

public class CPACodesBase : Directory
{
	public string Type { get; set; }

	public string TypeName { get; set; }

	public DateOnly? MarkInitDate { get; set; }
}
