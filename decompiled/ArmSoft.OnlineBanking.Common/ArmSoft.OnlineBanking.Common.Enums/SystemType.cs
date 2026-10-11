using ArmSoft.OnlineBanking.Common.Attributes;

namespace ArmSoft.OnlineBanking.Common.Enums;

public enum SystemType : byte
{
	NotSet = 0,
	WebSite = 1,
	MobileSite = 2,
	ImportService = 3,
	ASAccountant = 4,
	[SystemType(true)]
	MobileApplication = 5,
	[SystemType(true)]
	AdminWebSite = 6,
	[SystemType(true)]
	WebApiClientAdmin = 7,
	[SystemType(true)]
	WebApiClient = 8,
	CorporateApiClient = 9,
	PushService = 10,
	SyncService = 11,
	WorkerService = 12,
	[SystemType(true)]
	Web = 13,
	[SystemType(true)]
	Android = 14,
	[SystemType(true)]
	IOS = 15,
	[SystemType(true)]
	JuniorApplication = 16,
	[SystemType(true)]
	CreditPlatform = 17,
	[SystemType(true)]
	ASBank = 18,
	[SystemType(true)]
	ASEnterprise = 19,
	[SystemType(true)]
	ASHumanResourceManagement = 20,
	[SystemType(true)]
	ASAccountantApplication = 21,
	[SystemType(true)]
	ASBookkeeper = 22,
	[SystemType(true)]
	ASSalesManagement = 23,
	[SystemType(true)]
	ASOrderApplication = 24,
	[SystemType(true)]
	ASTrade = 25,
	[SystemType(true)]
	ASShop = 26,
	[SystemType(true)]
	ASMarketPlace = 27,
	[SystemType(true)]
	ASFinMobile = 28,
	[SystemType(true)]
	OwnApplication = 29,
	All = byte.MaxValue
}
