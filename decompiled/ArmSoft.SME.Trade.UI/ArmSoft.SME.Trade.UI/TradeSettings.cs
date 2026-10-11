using System.Runtime.CompilerServices;
using ArmSoft.SME.Common.Helpers;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Core.UI.HelpClasses;
using ArmSoft.SME.Core.UI.Helpers;
using ArmSoft.SME.Core.UI.Services;
using ArmSoft.SME.Core.UI.VersionManagement;
using ArmSoft.SME.Trade.Client;
using ArmSoft.SME.Trade.Common.Helpers;
using ArmSoft.SME.Trade.Common.Models.Caches;
using ArmSoft.SME.Trade.UI.Cache;
using ArmSoft.SME.Trade.UI.CustomUIElements;
using ArmSoft.SME.Trade.UI.Helpers;
using ArmSoft.SME.Trade.UI.Services;
using ArmSoft.SME.Trade.UI.VersionManagement;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ArmSoft.SME.Trade.UI;

public class TradeSettings : Settings
{
	public static PoleDisplayWindow PoleDisplay;

	public static CashCacheRow ECRCurrentCashRow;

	public static TradeCacheService TradeCacheService => Settings.ServiceProvider.GetRequiredService<TradeCacheService>();

	public static LocalDBService LocalDBService => Settings.ServiceProvider.GetRequiredService<LocalDBService>();

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static TradeClient CreateTradeClient()
	{
		return Settings.ServiceProvider.GetRequiredService<TradeClientService>().Client;
	}

	protected override void ConfigureServices(IServiceCollection services, IConfiguration configuration)
	{
		base.ConfigureServices(services, configuration);
		services.AddTransient<TradeClientService>();
		services.AddScoped<IDirectoryService, TradeDirectoryService>();
		services.AddScoped<IDocumentService, TradeDocumentService>();
		services.AddScoped<ITemplateSubstitutionService, TradeTemplateSubstitutionService>();
		services.AddScoped<ICacheFunctionsService, TradeCacheFunctionsService>();
		services.AddSingleton<TradeCacheService>();
		services.AddScoped<IStringResourceService, TradeStringResourceService>();
		services.AddScoped<ITaxServiceHelper, TradeTaxServiceHelper>();
		services.AddSingleton<IPODefinitionHelper, TradePODefinitionHelper>();
		services.AddTransient<IUIManager, UIManager>();
		services.AddScoped<ITemplateService, TradeTemplateService>();
		services.AddSingleton<IImagesService, TradeImagesService>();
		LocalDBService implementationInstance = new LocalDBService();
		services.AddSingleton(implementationInstance);
		services.AddSingleton((ILocalDB)implementationInstance);
		services.AddSingleton((ILocalDBLoginInfo)implementationInstance);
		services.AddSingleton((ILocalDBUserAccessService)implementationInstance);
		services.AddSingleton<ILocalDBDocumentService, LocalDBDocumentService>();
		services.AddSingleton<ICommonCoreHelper, TradeCommonCoreHelper>();
	}
}
