namespace ArmSoft.OpenBanking.Api.Common;

public enum SystemType : byte
{
	NotSet = 0,
	ASAccountant = 1,
	ASBookkeeper = 2,
	ASTrade = 3,
	ASMarketPlace = 5,
	ASSalesManagement = 6,
	ASFinMobile = 8,
	ASEnterprise = 99,
	Other = 100
}
