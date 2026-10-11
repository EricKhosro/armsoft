using System;

namespace ArmSoft.OpenBanking.Api.Common;

public class GetNewCancelRequestApplicationParams : GetNewSignableObjectParams
{
	public Guid TargetId { get; set; }
}
