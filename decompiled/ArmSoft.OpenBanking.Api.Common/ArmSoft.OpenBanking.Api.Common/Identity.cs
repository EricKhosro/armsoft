using System;
using System.Collections.Generic;
using ArmSoft.OnlineBanking.Common.Enums;
using Newtonsoft.Json;

namespace ArmSoft.OpenBanking.Api.Common;

public class Identity
{
	public int UserId { get; set; }

	public string Username { get; set; } = string.Empty;

	public string FullName { get; set; } = string.Empty;

	public string? ClientCode { get; set; }

	public string? ClientName { get; set; }

	public ClientStatus ClientStatus { get; set; }

	public string? ClientGroup { get; set; }

	public DateTime? LastLoginDate { get; set; }

	public string LastIpAddress { get; set; } = string.Empty;

	public DateTime? LastPasswordChangeDate { get; set; }

	public bool MustChangePassword { get; set; }

	public ChangePasswordReason ChangePasswordReason { get; set; }

	public string SecurityProvider { get; set; } = string.Empty;

	public List<string> AllowedSecurityProviders { get; set; } = new List<string>();

	public bool MustActivateSecurityProvider { get; set; }

	[Obsolete("Use UpdateContactData instead, when mobile min version is 8.9.0.")]
	public bool MustUpdateContactData => UpdateContactData != ClientDataUpdateType.Disabled;

	[Obsolete("Use UpdatePassportData instead, when mobile min version is 8.9.0.")]
	public bool MustUpdatePassportData => UpdatePassportData != ClientDataUpdateType.Disabled;

	[JsonProperty]
	public ClientDataUpdateType UpdateContactData { get; set; }

	[JsonProperty]
	public ClientDataUpdateType UpdatePassportData { get; set; }

	public bool ActivateSecurityProviderOnLogin { get; set; }

	public string PhoneNumber { get; set; } = string.Empty;

	public string Email { get; set; } = string.Empty;

	public string Passport { get; set; } = string.Empty;

	public DateTime BirthDate { get; set; }

	public bool AllowUserVerificationBySMS { get; set; }

	public bool AllowUserVerificationByEmail { get; set; }

	public string ClientPhoneNumber { get; set; } = string.Empty;

	public string ClientEmail { get; set; } = string.Empty;

	public bool IsSecurityProviderRequired { get; set; }

	public bool CanChangeSecurityProvider { get; set; }

	public DevicePushAuthenticationStatus DevicePushAuthenticationStatus { get; set; }

	public bool IsLimited { get; set; }

	public bool IsClientViewOnly { get; set; }

	public IList<string> Permissions { get; set; } = new List<string>();

	public string? TaxCode { get; set; }

	public string? SocialCard { get; set; }

	public string TokenSerial { get; set; } = string.Empty;

	public bool IsSubuser { get; set; }

	public string PartnershipStatus { get; set; } = string.Empty;
}
