namespace ArmSoft.OnlineBanking.Common.Enums;

public enum AswaFailedResult : byte
{
	VehicleNotFound,
	VehicleNotFoundForCurrentClient,
	BonusMalusNotFound,
	VehicleUseTypesNotFound,
	InsuranceCompaniesNotFound,
	BanksNotFound,
	InsuranceCompanyOffersNotFound,
	ConfirmOfferFailed,
	InvalidPhoneNumberFormat,
	InvalidEmailFormat,
	InvalidInsuranceDuration,
	UpdateContractInfoFailed,
	InvalidDefaultAccount,
	InvalidVehicleUseType
}
