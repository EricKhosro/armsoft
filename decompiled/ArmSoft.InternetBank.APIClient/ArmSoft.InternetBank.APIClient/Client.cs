using System;
using System.Threading.Tasks;
using ArmSoft.InternetBank.APIClient.Core;
using ArmSoft.OnlineBanking.Common.Enums;

namespace ArmSoft.InternetBank.APIClient;

public class Client : ClientBase
{
	public PaymentManager PaymentManager { get; internal set; }

	public ApplicationManager ApplicationManager { get; internal set; }

	public NewsManager NewsManager { get; internal set; }

	public CancelRequestManager CancelRequestManager { get; internal set; }

	public AccountManager AccountManager { get; internal set; }

	public TaxAuthorityManager TaxAuthorityManager { get; private set; }

	public CountryManager CountryManager { get; private set; }

	public DirectoryManager DirectoryManager { get; private set; }

	public CardManager CardManager { get; private set; }

	public AttachedCardManager AttachedCardManager { get; private set; }

	public PurchasedSecurityManager PurchasedSecurityManager { get; private set; }

	public DepositManager DepositManager { get; private set; }

	public IssuedBondManager IssuedBondManager { get; private set; }

	public CreditManager CreditManager { get; private set; }

	public BondManager BondManager { get; private set; }

	public MenuManager MenuManager { get; private set; }

	public CommissionManager CommissionManager { get; private set; }

	public MerchantManager MerchantManager { get; private set; }

	public UtilityDebtManager UtilityDebtManager { get; private set; }

	public MobileAuthenticationManager MobileAuthenticationManager { get; private set; }

	public VehicleManager VehicleManager { get; private set; }

	public WorkdayManager WorkdayManager { get; private set; }

	public PensionStatementManager PensionStatementManager { get; private set; }

	public UserSettingsManager UserSettingsManager { get; private set; }

	public UserInfoManager UserInfoManager { get; private set; }

	public OpenCreditManager OpenCreditManager { get; private set; }

	public CashOutByPhoneManager CashOutByPhoneManager { get; private set; }

	public ContactTransferManager ContactTransferManager { get; private set; }

	public FriendLoanManager FriendLoanManager { get; private set; }

	public PropertyTaxManager PropertyTaxManager { get; private set; }

	public CompulsoryEnforcementManager CompulsoryEnforcementManager { get; private set; }

	public GarbageCollectionManager GarbageCollectionManager { get; private set; }

	public QrAuthenticationManager QrAuthenticationManager { get; private set; }

	public PasswordManager PasswordManager { get; private set; }

	public RubTransferManager RubTransferManager { get; private set; }

	public BankClientRegistrationManager BankClientRegistrationManager { get; private set; }

	public MessageManager MessageManager { get; private set; }

	public AswaManager AswaManager { get; private set; }

	public ArcaPayManager ArcaPayManager { get; private set; }

	public SwiftGpiTrackerManager SwiftGpiTrackerManager { get; private set; }

	public TemplateManager TemplateManager { get; private set; }

	public RateManager RateManager { get; private set; }

	public NotificationManager NotificationManager { get; private set; }

	public MapPointManager MapPointManager { get; private set; }

	public Client(string baseUrl, string credentials, string appName, int timeout = 90000, string language = "en-US", string version = null)
		: this(baseUrl, credentials, appName, SystemType.WebApiClient, timeout, language, version)
	{
	}

	public Client(string baseUrl, string credentials, ClientSystemType systemType, string appName, int timeout = 90000, string language = "en-US", string version = null)
		: this(baseUrl, credentials, appName, MapSystemType(systemType), timeout, language, version)
	{
	}

	public Client(Bank bank, ClientSystemType systemType, string appName, int timeout = 90000, string language = "en-US", string version = null)
		: this(Config.ApiUrls[bank], Config.ApiCredentials[bank], appName, MapSystemType(systemType), timeout, language, version)
	{
	}

	internal Client(string baseUrl, string credentials, string deviceId, SystemType systemType, int timeout = 90000, string language = "en-US", string version = null, string deviceModel = null, string devicePlatform = null, string devicePlatformVersion = null)
		: base(baseUrl, credentials, deviceId, systemType, timeout, language, version, deviceModel, devicePlatform, devicePlatformVersion)
	{
		RegisterManagers();
	}

	public bool CheckServiceAccess()
	{
		try
		{
			return ParamManager.IBVersion != "0.0.0";
		}
		catch (Exception)
		{
			return false;
		}
	}

	public async Task CheckAuthentication()
	{
		await PostAsync("authentication/check", requireAuthorization: true);
	}

	private static SystemType MapSystemType(ClientSystemType systemType)
	{
		return systemType switch
		{
			ClientSystemType.ASBank => SystemType.ASBank, 
			ClientSystemType.ASEnterprise => SystemType.ASEnterprise, 
			ClientSystemType.ASHumanResourceManagement => SystemType.ASHumanResourceManagement, 
			ClientSystemType.ASAccountant => SystemType.ASAccountantApplication, 
			ClientSystemType.ASBookkeeper => SystemType.ASBookkeeper, 
			ClientSystemType.ASSalesManagement => SystemType.ASSalesManagement, 
			ClientSystemType.ASOrderApplication => SystemType.ASOrderApplication, 
			ClientSystemType.ASTrade => SystemType.ASTrade, 
			ClientSystemType.ASShop => SystemType.ASShop, 
			ClientSystemType.ASMarketPlace => SystemType.ASMarketPlace, 
			ClientSystemType.ASFinMobile => SystemType.ASFinMobile, 
			_ => SystemType.WebApiClient, 
		};
	}

	private void RegisterManagers()
	{
		PaymentManager = new PaymentManager(this);
		ApplicationManager = new ApplicationManager(this);
		NewsManager = new NewsManager(this);
		CancelRequestManager = new CancelRequestManager(this);
		AccountManager = new AccountManager(this);
		TaxAuthorityManager = new TaxAuthorityManager(this);
		CountryManager = new CountryManager(this);
		DirectoryManager = new DirectoryManager(this);
		CardManager = new CardManager(this);
		AttachedCardManager = new AttachedCardManager(this);
		DepositManager = new DepositManager(this);
		IssuedBondManager = new IssuedBondManager(this);
		CreditManager = new CreditManager(this);
		BondManager = new BondManager(this);
		MenuManager = new MenuManager(this);
		PurchasedSecurityManager = new PurchasedSecurityManager(this);
		CommissionManager = new CommissionManager(this);
		MerchantManager = new MerchantManager(this);
		UtilityDebtManager = new UtilityDebtManager(this);
		VehicleManager = new VehicleManager(this);
		PensionStatementManager = new PensionStatementManager(this);
		UserSettingsManager = new UserSettingsManager(this);
		CompulsoryEnforcementManager = new CompulsoryEnforcementManager(this);
		UserInfoManager = new UserInfoManager(this);
		MobileAuthenticationManager = new MobileAuthenticationManager(this);
		OpenCreditManager = new OpenCreditManager(this);
		FriendLoanManager = new FriendLoanManager(this);
		ContactTransferManager = new ContactTransferManager(this);
		PropertyTaxManager = new PropertyTaxManager(this);
		GarbageCollectionManager = new GarbageCollectionManager(this);
		QrAuthenticationManager = new QrAuthenticationManager(this);
		PasswordManager = new PasswordManager(this);
		RubTransferManager = new RubTransferManager(this);
		BankClientRegistrationManager = new BankClientRegistrationManager(this);
		CashOutByPhoneManager = new CashOutByPhoneManager(this);
		WorkdayManager = new WorkdayManager(this);
		MessageManager = new MessageManager(this);
		AswaManager = new AswaManager(this);
		ArcaPayManager = new ArcaPayManager(this);
		SwiftGpiTrackerManager = new SwiftGpiTrackerManager(this);
		TemplateManager = new TemplateManager(this);
		RateManager = new RateManager(this);
		NotificationManager = new NotificationManager(this);
		MapPointManager = new MapPointManager(this);
	}
}
