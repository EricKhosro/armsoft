using System;

namespace ArmSoft.OnlineBanking.Common.Enums;

[Flags]
public enum VerificationMethods
{
	NotSet = 0,
	Email = 1,
	Sms = 2,
	UserData = 4,
	SecurityProvider = 8,
	UsernamePassword = 0x10,
	PersonIdentification = 0x20,
	ClientRegistrationInformation = 0x40,
	PhoneNumberChecked = 0x80,
	Passport = 0x100
}
