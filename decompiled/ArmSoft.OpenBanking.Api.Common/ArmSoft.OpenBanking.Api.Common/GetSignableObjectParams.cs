using System;
using System.ComponentModel;

namespace ArmSoft.OpenBanking.Api.Common;

public class GetSignableObjectParams
{
	[DefaultValue("")]
	public Guid Id { get; set; }
}
