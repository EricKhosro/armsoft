using System;

namespace ArmSoft.OpenBanking.Api.Common;

public class EntityModel<TId> where TId : struct
{
	public byte[]? Version { get; set; } = Array.Empty<byte>();

	public virtual TId Id { get; set; }
}
