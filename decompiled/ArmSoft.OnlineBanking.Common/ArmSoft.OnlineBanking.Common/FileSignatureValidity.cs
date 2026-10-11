using System;

namespace ArmSoft.OnlineBanking.Common;

[Flags]
public enum FileSignatureValidity : byte
{
	NotSet = 0,
	SignatureIsValid = 1,
	TimeStampIsValid = 2,
	CertificateIsValid = 4,
	Valid = SignatureIsValid | TimeStampIsValid | CertificateIsValid
}
