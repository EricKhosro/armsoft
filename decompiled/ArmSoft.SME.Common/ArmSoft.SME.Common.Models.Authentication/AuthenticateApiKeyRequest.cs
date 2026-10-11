using System;

namespace ArmSoft.SME.Common.Models.Authentication;

public class AuthenticateApiKeyRequest
{
	public Guid ClientId { get; set; }

	public string Secret { get; set; }

	public int DBId { get; set; }

	public string CompName { get; set; }

	public string Token { get; set; }
}
