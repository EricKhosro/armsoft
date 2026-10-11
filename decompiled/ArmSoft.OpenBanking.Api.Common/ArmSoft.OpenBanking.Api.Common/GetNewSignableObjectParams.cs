using System;
using System.ComponentModel;

namespace ArmSoft.OpenBanking.Api.Common;

public class GetNewSignableObjectParams
{
	[DefaultValue(null)]
	public Guid? TemplateId { get; set; }
}
