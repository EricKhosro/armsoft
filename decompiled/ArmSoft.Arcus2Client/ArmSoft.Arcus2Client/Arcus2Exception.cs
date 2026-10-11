using System;

namespace ArmSoft.Arcus2Client;

public class Arcus2Exception : Exception
{
	public bool IsInternalException { get; set; }

	public int Code { get; set; }

	public Arcus2Exception(string message)
		: base(message)
	{
	}
}
