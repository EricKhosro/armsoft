using System;
using ArmSoft.SME.Common.Models;

namespace ArmSoft.SME.Client.Models.TemplateSubstitution;

public class DocumentSubstituteResponse
{
	public Guid DocISN { get; set; }

	public ExceptionModel Exception { get; set; }
}
