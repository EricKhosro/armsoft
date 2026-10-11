using System;
using System.ComponentModel;
using ArmSoft.OnlineBanking.Common.Enums;

namespace ArmSoft.OpenBanking.Api.Common;

public class SignDocumentParams
{
	[DefaultValue("")]
	public Guid Id { get; set; }

	[DefaultValue("")]
	public string Signature { get; set; } = string.Empty;

	[DefaultValue("")]
	public string? SecurityProviderChallenge { get; set; } = string.Empty;

	public byte[]? Version { get; set; } = Array.Empty<byte>();

	public UserSignatureMode UserSignatureMode { get; set; }
}
