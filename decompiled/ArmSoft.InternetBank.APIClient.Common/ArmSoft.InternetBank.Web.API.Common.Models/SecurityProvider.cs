using System;
using ArmSoft.OnlineBanking.Common.Enums;

namespace ArmSoft.InternetBank.Web.API.Common.Models;

public class SecurityProvider
{
	public string ProviderName { get; set; }

	public string SerialNumber { get; set; }

	public int Id { get; set; }

	public int UserId { get; set; }

	public string AuthenticationMode { get; set; }

	public string SignatureMode { get; set; }

	public DateTime? ActivationDate { get; set; }

	public SecurityProviderUsageState UsageState { get; set; }

	public bool AssignedByOperator { get; set; }

	public string ProviderDisplayName { get; set; }
}
