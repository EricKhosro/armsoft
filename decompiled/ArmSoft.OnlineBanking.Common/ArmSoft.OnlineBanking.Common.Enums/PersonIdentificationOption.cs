using System;

namespace ArmSoft.OnlineBanking.Common.Enums;

public enum PersonIdentificationOption : byte
{
	NotSet,
	Account,
	PlasticCard,
	CreditAgreement,
	TaxCode,
	[Obsolete]
	PhoneNumber,
	PassportIdentificationId,
	SocialCard
}
