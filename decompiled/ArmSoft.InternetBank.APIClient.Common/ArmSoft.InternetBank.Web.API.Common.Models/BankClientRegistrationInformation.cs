using System;
using System.Collections.Generic;
using ArmSoft.OnlineBanking.Common.Enums;

namespace ArmSoft.InternetBank.Web.API.Common.Models;

public class BankClientRegistrationInformation
{
	public Guid Id { get; set; }

	public string ClientCode { get; set; }

	public string Username { get; set; }

	public string Email { get; set; }

	public string Mobile { get; set; }

	public BankClientRegistrationInformationOption SelectedClientBankInformationOption { get; set; }

	public string ClientBankInformation { get; set; }

	public string Passport { get; set; }

	public VerificationResult EmailVerificationResult { get; set; }

	public VerificationResult MobileVerificationResult { get; set; }

	public bool IsTermsAccepted { get; set; }

	public string IpAddress { get; set; }

	public bool IsSuccessfullyRegistered { get; set; }

	public SystemType SystemType { get; set; }

	public DateTime CreationDate { get; set; }

	public RegistrationFailedStep FailedStep { get; set; }

	public string BiometricIdentificationId { get; set; }

	public BiometricMediaArchiveStatus BiometricMediaArchiveStatus { get; set; }

	public List<string> BiometricMediaArchivedUrls { get; set; }

	public List<string> BiometricMediaUnarchivedUrls { get; set; }

	public string BiometricMediaArchivationErrorMessage { get; set; }

	public bool? IsBiometricCheckPassed { get; set; }

	public bool? IsDocumentCheckPassed { get; set; }

	public bool? IsQualityCheckPassed { get; set; }

	public bool? IsKYCCheckPassed { get; set; }

	public ClientStatus ClientStatus { get; set; }
}
