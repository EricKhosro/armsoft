using System;
using System.Collections.Generic;
using System.ComponentModel;
using ArmSoft.OnlineBanking.Common.Enums;

namespace ArmSoft.OpenBanking.Api.Common;

public class SignDocumentsParams
{
	public List<Guid> Ids { get; set; } = new List<Guid>();

	[DefaultValue("")]
	public string Signature { get; set; } = string.Empty;

	[DefaultValue("")]
	public string? SecurityProviderChallenge { get; set; }

	public UserSignatureMode UserSignatureMode { get; set; }
}
