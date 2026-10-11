using System;

namespace ArmSoft.OpenBanking.Api.Common;

public class SignableObject : EntityModel<Guid>
{
	public DateTime Date { get; set; }

	public string? FileName { get; set; }

	public Guid? TemplateId { get; set; }
}
