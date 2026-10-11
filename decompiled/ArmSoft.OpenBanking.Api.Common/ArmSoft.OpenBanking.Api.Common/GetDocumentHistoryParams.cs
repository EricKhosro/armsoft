using System;
using System.ComponentModel;

namespace ArmSoft.OpenBanking.Api.Common;

public class GetDocumentHistoryParams
{
	[DefaultValue("")]
	public Guid Id { get; set; }
}
