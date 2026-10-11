using System;
using ArmSoft.OnlineBanking.Common.Enums;

namespace ArmSoft.InternetBank.Web.API.Common.Models;

public class BankClientRegistrationParams
{
	public string ClientCode { get; set; }

	public string IpAddress { get; set; }

	public RegistrationFailedStep FailedStep { get; set; }

	public BankClientRegistrationInformationOption SelectedClientBankInformationOption { get; set; }

	public string ClientBankInformation { get; set; }

	public SystemType SystemType { get; set; }

	public DateTime? DateFrom { get; set; }

	public DateTime? DateTo { get; set; }

	public ClientStatus ClientStatus { get; set; }

	public string Mobile { get; set; }

	public Guid? GroupId { get; set; }

	public string Passport { get; set; }

	public bool ShowOnlyIsBiometricCheckPassed { get; set; }

	public BankClientRegistrationParams()
	{
		ClientCode = null;
		IpAddress = null;
		FailedStep = RegistrationFailedStep.NotSet;
		SelectedClientBankInformationOption = BankClientRegistrationInformationOption.NotSet;
		ClientBankInformation = null;
		SystemType = SystemType.NotSet;
		DateFrom = null;
		DateTo = null;
		ClientStatus = ClientStatus.NotSet;
		Mobile = null;
		GroupId = null;
		Passport = null;
		ShowOnlyIsBiometricCheckPassed = false;
	}
}
