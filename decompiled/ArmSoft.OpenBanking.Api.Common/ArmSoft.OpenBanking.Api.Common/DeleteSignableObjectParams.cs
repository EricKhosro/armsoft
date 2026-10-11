using System;

namespace ArmSoft.OpenBanking.Api.Common;

public class DeleteSignableObjectParams
{
	public Guid Id { get; set; }

	public byte[] Version { get; set; } = Array.Empty<byte>();
}
