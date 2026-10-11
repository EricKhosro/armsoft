using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Reflection;
using System.Resources;
using System.Runtime.InteropServices;
using System.ServiceModel;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Xml.Schema;
using System.Xml.XPath;
using ArmSoft.Cloud.Client;
using ArmSoft.ECRConnector;
using ArmSoft.InternetBank.APIClient.Core;
using ArmSoft.OpenBanking.Api.Client.Core;
using ArmSoft.SME.Client;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Exceptions;
using ArmSoft.SME.Common.Extensions;
using ArmSoft.SME.Common.Parameters;
using ArmSoft.SME.Common.SmartEnums;
using ArmSoft.SME.Core.UI.Caches;
using ArmSoft.SME.Core.UI.Extensions;
using ArmSoft.SME.Core.UI.HelpClasses;
using ArmSoft.SME.Core.UI.Helpers;
using ArmSoft.SME.Core.UI.Localizers;
using ArmSoft.SME.Core.UI.MetaData;
using ArmSoft.SME.Core.UI.PersistableObject;
using ArmSoft.SME.Core.UI.Services;
using ArmSoft.SME.Core.UI.SmartEnums;
using ArmSoft.TaxService;
using ArmSoft.b2b.V2;
using CBA.ExchangeRates;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Core.Native;
using DevExpress.Xpf.Editors;
using DevExpress.Xpf.Editors.Themes;
using DevExpress.Xpf.Grid.Themes;
using DevExpress.Xpf.Utils.Themes;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Win32;
using Serilog;
using Serilog.Configuration;
using Serilog.Debugging;
using Serilog.Events;
using Serilog.Extensions.Logging;

namespace ArmSoft.SME.Core.UI;

public class Settings
{
	[StructLayout(LayoutKind.Sequential, Size = 1)]
	private struct ServiceConsts
	{
		public const string FaultInfoCode = "1";

		public const string FaultWarningCode = "2";

		public const string FaultErrorCode = "3";

		public const string FaultFatalErrorCode = "4";
	}

	public const string DateShortFormat = "dd/MM/yy";

	public const string DateLongFormat = "dd/MM/yyyy";

	public const string DateTimeFormatWithoutSeconds = "dd/MM/yy HH:mm";

	public const string FirstSectionName = "ArmSoft";

	public const string SecondSectionName = "ArmenianSoftware";

	private static double _fontSize;

	private static bool? _b2CUsesEmbeddedWebView;

	private static string _localDataFolderPath;

	private const string _resourcesStringSuffix = ".g";

	private bool? _isWindows7;

	private static bool? _isWin11Theme;

	private static string _tempPath;

	private static int? _processId;

	private static string _b2bCloudBaseAddress;

	private const string _win11SMEThemeName = "Win11SMETheme";

	private const string _win11SMEThemeDisplayName = "SME Win 11 theme";

	private static SQLiteDialogService _sqliteDialogService;

	private static SQLiteLayoutService _sqliteLayoutServicee;

	private static string _applicationToken;

	private static int? _expiration;

	private static bool? _offlineModeIsAvailable;

	public static SolidColorBrush RequiredBrush { get; private set; }

	public static SolidColorBrush ReadOnlyBrush { get; private set; }

	public static SolidColorBrush EditorErrorBrush { get; private set; }

	public static SolidColorBrush ReadOnlyEditorErrorBrush { get; private set; }

	public static FontFamily FontFamily { get; private set; }

	public static MainWindowBase HostingEnvironment { get; private set; }

	public static HttpClient HttpClient { get; private set; }

	public static ServiceProvider ServiceProvider { get; private set; }

	internal static HashSet<string> CoreResources { get; private set; }

	internal static HashSet<string> AppResources { get; private set; }

	public static DefinitionMappingsService DefinitionMappingsService => ServiceProvider.GetRequiredService<DefinitionMappingsService>();

	public static IStringResourceService StringResourceService => ServiceProvider.GetRequiredService<IStringResourceService>();

	public static ResourceDictionary GenericResources { get; private set; }

	public static string AppIconPath { get; private set; }

	public static Version AppVersion { get; private set; }

	public static string AppVersionString => $"{AppVersion?.Major}.{AppVersion?.Minor}.{AppVersion?.Build}";

	public static bool IsBetaVersion => AppVersion.Revision > 0;

	public static string RegistryKey
	{
		get
		{
			string text = "Software\\Armenian Software\\";
			switch (SystemType)
			{
			case SystemTypes.Trade:
				text += "AS Trade 9";
				break;
			case SystemTypes.Accountant:
				text += "AS Accountant 9";
				break;
			case SystemTypes.SalesManagement:
				text += "AS SalesManagement 9";
				break;
			case SystemTypes.Cabinet:
				text += "Cabinet";
				break;
			}
			return text;
		}
	}

	public static bool IsDarkTheme { get; private set; }

	public static string ProgramName => SystemType switch
	{
		SystemTypes.Trade => ArmSoft.SME.Common.Resources.ProgramNameTrade, 
		SystemTypes.SalesManagement => ArmSoft.SME.Common.Resources.ProgramNameSalesManagement, 
		SystemTypes.Accountant => ArmSoft.SME.Common.Resources.ProgramNameAccountant + (ConfigHelper.IsPublicSector ? (" (" + StringResourceService.GetStringResource("PublicSector") + ")") : ""), 
		SystemTypes.Cabinet => ArmSoft.SME.Common.Resources.ProgramNameCabinet, 
		_ => "Program Name", 
	};

	public static bool IsWin11Theme
	{
		get
		{
			bool valueOrDefault = _isWin11Theme == true;
			if (!_isWin11Theme.HasValue)
			{
				valueOrDefault = ApplicationTheme() == ApplicationThemeSE.Win11.Value;
				_isWin11Theme = valueOrDefault;
			}
			return _isWin11Theme.Value;
		}
	}

	public static string ProgramInnerName => SystemType switch
	{
		SystemTypes.Trade => "Trade", 
		SystemTypes.Accountant => "Accountant", 
		SystemTypes.SalesManagement => "SalesManagement", 
		SystemTypes.Cabinet => "Cabinet", 
		_ => "Program Name", 
	};

	public static string ApplicationId => "ArmSoft." + ProgramInnerName;

	public static string LocalDataFolderPath
	{
		get
		{
			if (string.IsNullOrWhiteSpace(_localDataFolderPath))
			{
				if (SystemType == SystemTypes.SalesManagement)
				{
					_localDataFolderPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData) + "\\Armenian Software\\SalesManagement\\";
				}
				else if (SystemType == SystemTypes.Trade)
				{
					_localDataFolderPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData) + "\\Armenian Software\\Trade\\";
				}
				else if (SystemType == SystemTypes.Accountant)
				{
					_localDataFolderPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData) + "\\Armenian Software\\Accountant\\";
				}
			}
			return _localDataFolderPath;
		}
	}

	public static string LocalStoragePath => Path.Combine(LocalDataFolderPath, "Local Storage");

	public static string LocalImagesPath => Path.Combine(LocalDataFolderPath, "Images");

	public static string LocalTemplatesPath => "Templates";

	public static SystemTypes SystemType { get; private set; }

	public static Localizations Localization { get; private set; }

	public static string B2BCloudBaseAddress
	{
		get
		{
			if (string.IsNullOrWhiteSpace(_b2bCloudBaseAddress))
			{
				_b2bCloudBaseAddress = "https://cloud.armsoft.am/";
			}
			return _b2bCloudBaseAddress;
		}
	}

	public static bool B2CUsesEmbeddedWebView
	{
		get
		{
			if (!_b2CUsesEmbeddedWebView.HasValue)
			{
				_b2CUsesEmbeddedWebView = true;
			}
			return _b2CUsesEmbeddedWebView.Value;
		}
	}

	public static LoginService LoginService => ServiceProvider.GetRequiredService<LoginService>();

	public static ICacheFunctionsService CacheFunctionsService => ServiceProvider.GetRequiredService<ICacheFunctionsService>();

	public static CoreCacheService CoreCacheService => ServiceProvider.GetRequiredService<CoreCacheService>();

	public static ILocalDB LocalDB => ServiceProvider.GetRequiredService<ILocalDB>();

	public static ILocalDBUserAccessService LocalDBUserAccessService => ServiceProvider.GetRequiredService<ILocalDBUserAccessService>();

	public static IParametersService ParametersService => ServiceProvider.GetRequiredService<IParametersService>();

	public static IUserAccessService UserAccessService => ServiceProvider.GetRequiredService<IUserAccessService>();

	public static IImagesService ImagesService => ServiceProvider.GetRequiredService<IImagesService>();

	public static ICommonCoreService CommonCoreService => ServiceProvider.GetRequiredService<ICommonCoreService>();

	public static ICommonCoreHelper CommonCoreHelper => ServiceProvider.GetRequiredService<ICommonCoreHelper>();

	public static ArmSoft.SME.Core.UI.Services.TaxService TaxService => ServiceProvider.GetRequiredService<ArmSoft.SME.Core.UI.Services.TaxService>();

	public static ITaxServiceHelper TaxServiceHelper => ServiceProvider.GetRequiredService<ITaxServiceHelper>();

	public static IDirectoryService DirectoryService => ServiceProvider.GetRequiredService<IDirectoryService>();

	public static ITreeService TreeService => ServiceProvider.GetRequiredService<ITreeService>();

	public static IDocumentService DocumentService => ServiceProvider.GetRequiredService<IDocumentService>();

	public static ILocalDBDocumentService LocalDBDocumentService => ServiceProvider.GetRequiredService<ILocalDBDocumentService>();

	public static IDataViewService DataViewService => ServiceProvider.GetRequiredService<IDataViewService>();

	public static IDialogService DialogService
	{
		get
		{
			if (IsOffline)
			{
				if (_sqliteDialogService == null)
				{
					_sqliteDialogService = new SQLiteDialogService();
				}
				return _sqliteDialogService;
			}
			return ServiceProvider.GetRequiredService<IDialogService>();
		}
	}

	public static ILayoutService LayoutService
	{
		get
		{
			if (IsOffline)
			{
				if (_sqliteLayoutServicee == null)
				{
					_sqliteLayoutServicee = new SQLiteLayoutService();
				}
				return _sqliteLayoutServicee;
			}
			return ServiceProvider.GetRequiredService<ILayoutService>();
		}
	}

	public static IUnboundColumnsService UnboundColumnsService => ServiceProvider.GetRequiredService<IUnboundColumnsService>();

	public static CloudClient CloudClient
	{
		get
		{
			ThrowExceptionIfOffline();
			return ServiceProvider.GetRequiredService<CloudClientService>().Client;
		}
	}

	public static CloudClient DiscountServerClient
	{
		get
		{
			CloudClient client = ServiceProvider.GetRequiredService<CloudClientService>().Client;
			client.Token = GetApplicationToken();
			return client;
		}
	}

	public static GateSoapClient CBAExchangeRatesClient => ServiceProvider.GetRequiredService<CBAService>().Client;

	public static B2BClient B2BClient => ServiceProvider.GetRequiredService<B2BClientV2Service>().Client;

	public static IInternetBankService InternetBankService => ServiceProvider.GetRequiredService<IInternetBankService>();

	public static IOpenBankingService OpenBankingService => ServiceProvider.GetRequiredService<IOpenBankingService>();

	private bool IsWindows7
	{
		get
		{
			if (!_isWindows7.HasValue)
			{
				OperatingSystem oSVersion = System.Environment.OSVersion;
				_isWindows7 = oSVersion.Platform == PlatformID.Win32NT && oSVersion.Version.Major == 6 && oSVersion.Version.Minor == 1;
			}
			return _isWindows7.Value;
		}
	}

	public static string BackupExtension => SystemType switch
	{
		SystemTypes.Trade => "ast", 
		SystemTypes.MarketPlace => "asmp", 
		SystemTypes.Accountant => ConfigHelper.IsPublicSector ? "asps" : "asb", 
		SystemTypes.SalesManagement => "assm", 
		SystemTypes.InternetTradeAdmin => "assa", 
		SystemTypes.BookKeeper => "asbk", 
		_ => "asnn", 
	};

	public static string NewBackupExtension => "bak";

	public static string DropBoxAppKey => SystemType switch
	{
		SystemTypes.Trade => "2rzubvp6cf5gvd3", 
		SystemTypes.Accountant => "4dfnnwp1ecfuubh", 
		SystemTypes.SalesManagement => "fov74eoah9m9qub", 
		_ => string.Empty, 
	};

	public static bool OfflineModeIsAvailable
	{
		get
		{
			if (!_offlineModeIsAvailable.HasValue)
			{
				string[] array = RegistryHelper.ReadStringArrayFromRegistry("OfflineModeAvailableKeys");
				string text = RegistryHelper.ReadStringFromRegistry("LastKeySNRegKey");
				_offlineModeIsAvailable = MemoryExtensions.Contains<string>((ReadOnlySpan<string>)array, text);
			}
			return _offlineModeIsAvailable.Value;
		}
		set
		{
			_offlineModeIsAvailable = value;
		}
	}

	public static bool DocAutoSaveIsAvailable
	{
		get
		{
			if (SystemType == SystemTypes.Trade)
			{
				return true;
			}
			return false;
		}
	}

	public static bool IsOffline { get; set; }

	public static bool AllowOpInOfflineMode { get; set; }

	[DllImport("Kernel32.dll")]
	public static extern bool ProcessIdToSessionId(int processID, ref int sessionID);

	[DllImport("Shell32.dll")]
	private static extern int SetCurrentProcessExplicitAppUserModelID([MarshalAs(UnmanagedType.LPWStr)] string appID);

	public static ArmSoft.SME.Client.ApiClient CreateApiClient()
	{
		ThrowExceptionIfOffline();
		return LoginService.CreateClient();
	}

	static Settings()
	{
		_applicationToken = "";
		_expiration = null;
		_offlineModeIsAvailable = null;
		FontFamily = new FontFamily("Tahoma");
	}

	private static IConfigurationRoot GetConfiguration(Assembly appAssembly)
	{
		string directoryName = Path.GetDirectoryName(appAssembly.Location);
		return new ConfigurationBuilder().SetBasePath(directoryName).AddJsonFile("appsettings.json", optional: true).Build();
	}

	public void Init(SystemTypes systemType, Assembly appAssembly, Assembly[] localServicesAssemblies, ConfigureServicesDelegate configureServicesDelegate = null)
	{
		SystemType = systemType;
		Localization = Localizations.Armenia;
		AppVersion = appAssembly.GetName().Version;
		IConfigurationRoot configuration = GetConfiguration(appAssembly);
		LanguageService.RestoreCulture(GetCurrentCultureName());
		ConfigureLogger(configuration);
		_localDataFolderPath = configuration.GetSection("Storage").GetSection("Location").Value;
		if (!_b2CUsesEmbeddedWebView.HasValue)
		{
			if (bool.TryParse(configuration["B2CUsesEmbeddedWebView"], out var result))
			{
				_b2CUsesEmbeddedWebView = result;
			}
			else
			{
				_b2CUsesEmbeddedWebView = true;
			}
		}
		if (_b2bCloudBaseAddress == null)
		{
			_b2bCloudBaseAddress = configuration["b2bCloud"];
			if (string.IsNullOrWhiteSpace(_b2bCloudBaseAddress))
			{
				_b2bCloudBaseAddress = "https://cloud.armsoft.am/";
			}
		}
		Localizer.Configure();
		ServiceCollection services = new ServiceCollection();
		ConfigureServices(services, configuration);
		configureServicesDelegate?.Invoke(services);
		ServiceProvider = services.BuildServiceProvider();
		HttpClient = new HttpClient(ConfigureHttp());
		for (int i = 0; i < localServicesAssemblies.Length; i++)
		{
			DefinitionMappingsService.Configure(localServicesAssemblies[i]);
		}
		DefinitionMappingsService.Configure(typeof(IPOWindow).Assembly);
		DefinitionMappingsService.Configure(typeof(ArmSoft.SME.Common.Resources).Assembly);
		DefinitionMappingsService.Configure(typeof(ArmSoft.SME.Client.ApiClient).Assembly);
		AppResources = ReadResources(appAssembly);
		CoreResources = ReadResources(Assembly.GetExecutingAssembly());
		GenericResources = new ResourceDictionary
		{
			Source = new Uri("pack://application:,,,/ArmSoft.SME.Core.UI;component/generic.xaml", UriKind.Absolute)
		};
		switch (SystemType)
		{
		case SystemTypes.Trade:
			AppIconPath = "ASTrade.ico";
			_tempPath = "ASTrade9";
			break;
		case SystemTypes.SalesManagement:
			AppIconPath = "SalesManagement.ico";
			_tempPath = "ASSalesManagement9";
			break;
		case SystemTypes.Accountant:
			AppIconPath = "ASAccountant.ico";
			_tempPath = "ASAccountant9";
			break;
		case SystemTypes.Cabinet:
			AppIconPath = "acs.ico";
			_tempPath = "ACSCabinet";
			break;
		}
		AppIconPath = "pack://application:,,,/Images/" + AppIconPath;
		_tempPath = Path.GetTempPath() + _tempPath + Path.DirectorySeparatorChar;
		IsDarkTheme = IsApplicationThemeDark();
		RequiredBrush = new SolidColorBrush(IsDarkTheme ? Color.FromArgb(byte.MaxValue, 152, 122, 0) : Color.FromArgb(byte.MaxValue, 250, 250, 174));
		ReadOnlyBrush = new SolidColorBrush(Colors.DarkGray);
		EditorErrorBrush = new SolidColorBrush(IsDarkTheme ? Color.FromArgb(byte.MaxValue, 240, 113, 116) : Color.FromArgb(byte.MaxValue, 252, 94, 94));
		ReadOnlyEditorErrorBrush = new SolidColorBrush(IsDarkTheme ? Color.FromArgb(byte.MaxValue, 242, 127, 130) : Color.FromArgb(byte.MaxValue, byte.MaxValue, 102, 102));
		SetSettingsForWindows7();
		CleanTemp();
		EventManager.RegisterClassHandler(typeof(DateEdit), System.Windows.Controls.Control.MouseDoubleClickEvent, new RoutedEventHandler(TextBoxSelectAll));
		EventManager.RegisterClassHandler(typeof(PopupCalcEdit), System.Windows.Controls.Control.MouseDoubleClickEvent, new RoutedEventHandler(TextBoxSelectAll));
	}

	private void SetSettingsForWindows7()
	{
		if (IsWindows7)
		{
			SetCurrentProcessExplicitAppUserModelID("ArmSoft." + SystemType.ToString() + "." + AppVersion.Major + "." + ProcessId());
		}
	}

	private static SocketsHttpHandler ConfigureHttp()
	{
		return new SocketsHttpHandler
		{
			PooledConnectionIdleTimeout = new TimeSpan(0, 5, 0)
		};
	}

	public static string GetCurrentCultureName()
	{
		string text = "";
		using RegistryKey registryKey = Registry.CurrentUser.CreateSubKey(RegistryKey);
		if (registryKey != null)
		{
			text = registryKey.GetValue("Culture", string.Empty).ToString();
			LanguageService.RestoreCulture(text);
		}
		return text;
	}

	protected virtual void ConfigureServices(IServiceCollection services, IConfiguration configuration)
	{
		services.AddSingleton(typeof(IConfiguration), configuration);
		services.AddSingleton<LoginService>();
		services.AddSingleton<DefinitionMappingsService>();
		services.AddSingleton<IAttachmentsService, AttachmentsService>();
		services.AddSingleton<IUIRequestService, UIRequestService>();
		if (SystemType == SystemTypes.Cabinet)
		{
			services.AddSingleton<IDialogService, SQLiteDialogService>();
			services.AddSingleton<ILayoutService, SQLiteLayoutService>();
			services.AddSingleton<IParametersService, SQLiteParametersService>();
		}
		else
		{
			services.AddSingleton<IDialogService, ApiDialogService>();
			services.AddSingleton<ILayoutService, ApiLayoutService>();
			services.AddSingleton<IParametersService, ApiParametersService>();
			services.AddSingleton<IUnboundColumnsService, ApiUnboundColumnsService>();
			services.AddSingleton<IDocumentExtensionService, DocumentExtensionService>();
		}
		services.AddSingleton<IUserAccessService, UserAccessService>();
		services.AddSingleton<ITemplateSubstitutionService, TemplateSubstitutionService>();
		services.AddSingleton<CoreCacheService>();
		services.AddSingleton<IAppUpdateService, AppUpdateService>();
		services.AddScoped<SQLiteStorageService>();
		services.AddScoped<IDirectoryService, DirectoryService>();
		services.AddScoped<IDocumentService, DocumentService>();
		services.AddScoped<IDataViewService, DataViewService>();
		services.AddScoped<IDropDownService, DropDownService>();
		services.AddScoped<ITemplateService, TemplateService>();
		services.AddScoped<ICommonCoreService, CommonCoreService>();
		services.AddScoped<ICacheFunctionsService, CacheFunctionsService>();
		services.AddSingleton((Func<IServiceProvider, ILoggerFactory>)((IServiceProvider serviceProvider) => new SerilogLoggerFactory()));
		services.AddSingleton<ArmSoft.SME.Core.UI.Services.TaxService>();
		services.AddSingleton<IInternetBankService, InternetBankService>();
		services.AddSingleton<IOpenBankingService, OpenBankingService>();
		services.AddTransient<CloudClientService>();
		services.AddTransient<B2BClientV2Service>();
		services.AddTransient<CBAService>();
		services.AddTransient<IAWPService, AWPService>();
		services.AddTransient<IUserScriptService, UserScriptService>();
		services.AddTransient<IDVDService, DVDService>();
		services.AddTransient<ITreeService, TreeService>();
		services.AddTransient<ITreeDefinitionService, TreeDefinitionService>();
		services.AddTransient<IDocumentExtensionDefinitionService, DocumentExtensionDefinitionService>();
		services.AddTransient<IContextFunctionService, ContextFunctionService>();
	}

	private static HashSet<string> ReadResources(Assembly assembly)
	{
		HashSet<string> hashSet = new HashSet<string>(StringComparer.InvariantCultureIgnoreCase);
		if (assembly != null)
		{
			foreach (DictionaryEntry item in new ResourceManager(assembly.GetName().Name + ".g", assembly).GetResourceSet(Thread.CurrentThread.CurrentUICulture, createIfNotExists: true, tryParents: true))
			{
				hashSet.Add(item.Key.ToString().ToLower().Replace("%20", ""));
			}
		}
		return hashSet;
	}

	public static void InitHostingEnvironment(MainWindowBase hostingEnvironment)
	{
		HostingEnvironment = hostingEnvironment;
	}

	public static Brush GetArrowBrush()
	{
		SpinEditThemeKeyExtension resourceKey = new SpinEditThemeKeyExtension
		{
			ResourceKey = SpinEditThemeKeys.ArrowBrushColor,
			ThemeName = ApplicationThemeHelper.ApplicationThemeName
		};
		try
		{
			return new SolidColorBrush((Color)HostingEnvironment.FindResource(resourceKey));
		}
		catch (Exception)
		{
			return null;
		}
	}

	public static Brush GetReportSeparatorBrush()
	{
		if (IsDarkTheme)
		{
			return new SolidColorBrush((Color)ColorConverter.ConvertFromString("#58B8FE"));
		}
		return new SolidColorBrush((Color)ColorConverter.ConvertFromString("#0000FF"));
	}

	public static void CreateTheme()
	{
		if (IsWin11Theme)
		{
			Color applicationColor = GetApplicationColor();
			Dictionary<string, Color> colors = new Dictionary<string, Color>
			{
				{ "Accent", applicationColor },
				{
					"Button.CheckedBackground",
					applicationColor.ChangeColorOpacity(200)
				},
				{
					"Button.CheckedBorder",
					applicationColor.ChangeColorOpacity(180)
				},
				{
					"Button.CheckedHoverBackground",
					applicationColor.ChangeColorOpacity(200)
				},
				{
					"Button.CheckedHoverBorder",
					applicationColor.ChangeColorOpacity(200)
				},
				{
					"Button.CheckedPressedBackground",
					applicationColor.ChangeColorOpacity(200)
				},
				{
					"Button.CheckedPressedBorder",
					applicationColor.ChangeColorOpacity(160)
				},
				{
					"ListItem.SelectionAlt",
					GetActiveRowColor()
				},
				{ "Custom.Blue", applicationColor }
			};
			LightweightTheme lightweightTheme = ((!IsDarkTheme) ? LightweightTheme.OverridePalette(LightweightTheme.Win11Light, "Win11SMETheme", isPaletteTheme: false, "SME Win 11 theme", colors) : LightweightTheme.OverridePalette(LightweightTheme.Win11Dark, "Win11SMETheme", isPaletteTheme: false, "SME Win 11 theme", colors));
			LightweightThemeManager.RegisterTheme(lightweightTheme);
			ApplicationThemeHelper.ApplicationThemeName = lightweightTheme.Name;
			Style style = new Style(typeof(CheckEdit));
			style.Setters.Add(new Setter(CheckEdit.CheckBoxHeightProperty, 16.0));
			style.Setters.Add(new Setter(CheckEdit.CheckBoxWidthProperty, 16.0));
			System.Windows.Application.Current.Resources[typeof(CheckEdit)] = style;
			ThemedWindowThemeKeyExtension key = new ThemedWindowThemeKeyExtension
			{
				ResourceKey = ThemedWindowThemeKeys.WindowBorderThickness
			};
			System.Windows.Application.Current.Resources[key] = new Thickness(0.0);
			AddRowStyleForWin11();
		}
		else if (SystemType == SystemTypes.Trade)
		{
			ApplicationThemeHelper.ApplicationThemeName = (IsDarkTheme ? LightweightTheme.Office2019BlackSpruceLeaves.Name : LightweightTheme.Office2019ColorfulSpruceLeaves.Name);
		}
		else if (SystemType == SystemTypes.SalesManagement)
		{
			ApplicationThemeHelper.ApplicationThemeName = (IsDarkTheme ? LightweightTheme.Office2019BlackBrickwork.Name : LightweightTheme.Office2019ColorfulBrickwork.Name);
		}
		else
		{
			ApplicationThemeHelper.ApplicationThemeName = (IsDarkTheme ? LightweightTheme.Office2019Black.Name : LightweightTheme.Office2019Colorful.Name);
		}
		AddAutomationIds();
	}

	private static void AddAutomationIds()
	{
	}

	private static void AddRowStyleForWin11()
	{
		InplaceBaseEditThemeKeyExtension key = new InplaceBaseEditThemeKeyExtension
		{
			ResourceKey = InplaceBaseEditThemeKeys.TextEditInplaceInactiveTemplate,
			IsThemeIndependent = true
		};
		RenderTextBlock renderTree = new RenderTextBlock
		{
			Name = "PART_Editor",
			Margin = new Thickness(4.0, 3.0, 4.0, 3.0)
		};
		RenderTemplate value = new RenderTemplate
		{
			RenderTree = renderTree
		};
		System.Windows.Application.Current.Resources.Add(key, value);
	}

	public static Brush GetApplicationBrush()
	{
		return new SolidColorBrush(GetApplicationColor());
	}

	private static Color GetApplicationColor()
	{
		switch (SystemType)
		{
		case SystemTypes.Trade:
			if (IsDarkTheme)
			{
				return (Color)LightweightTheme.Office2019BlackSpruceLeaves.Palette["Color.Focused"];
			}
			return (Color)LightweightTheme.Office2019ColorfulSpruceLeaves.Palette["Color.Focused"];
		case SystemTypes.Accountant:
			if (IsDarkTheme)
			{
				return (Color)ColorConverter.ConvertFromString("#80c3fa");
			}
			return (Color)LightweightTheme.Office2019Colorful.Palette["Color.Focused"];
		case SystemTypes.SalesManagement:
			if (IsDarkTheme)
			{
				return (Color)LightweightTheme.Office2019BlackBrickwork.Palette["Color.Focused"];
			}
			return (Color)LightweightTheme.Office2019ColorfulBrickwork.Palette["Color.Focused"];
		default:
			if (IsDarkTheme)
			{
				return (Color)LightweightTheme.Office2019Black.Palette["Color.Focused"];
			}
			return (Color)LightweightTheme.Office2019Colorful.Palette["Color.Focused"];
		}
	}

	public static Brush GetBackgroundBrush()
	{
		switch (SystemType)
		{
		case SystemTypes.Trade:
			if (IsDarkTheme)
			{
				return (Brush)LightweightTheme.Office2019BlackSpruceLeaves.Palette["Brush.Window.Background"];
			}
			return (Brush)LightweightTheme.Office2019ColorfulSpruceLeaves.Palette["Brush.Window.Background"];
		case SystemTypes.SalesManagement:
			if (IsDarkTheme)
			{
				return (Brush)LightweightTheme.Office2019BlackBrickwork.Palette["Brush.Window.Background"];
			}
			return (Brush)LightweightTheme.Office2019ColorfulBrickwork.Palette["Brush.Window.Background"];
		default:
			if (IsDarkTheme)
			{
				return (Brush)LightweightTheme.Office2019Black.Palette["Brush.Window.Background"];
			}
			return (Brush)LightweightTheme.Office2019Colorful.Palette["Brush.Window.Background"];
		}
	}

	public static Brush GetControlBackground()
	{
		switch (SystemType)
		{
		case SystemTypes.Trade:
			if (IsDarkTheme)
			{
				return (Brush)LightweightTheme.Office2019BlackSpruceLeaves.Palette["Brush.Control.Background"];
			}
			return (Brush)LightweightTheme.Office2019ColorfulSpruceLeaves.Palette["Brush.Control.Background"];
		case SystemTypes.SalesManagement:
			if (IsDarkTheme)
			{
				return (Brush)LightweightTheme.Office2019BlackBrickwork.Palette["Brush.Control.Background"];
			}
			return (Brush)LightweightTheme.Office2019ColorfulBrickwork.Palette["Brush.Control.Background"];
		default:
			if (IsDarkTheme)
			{
				return (Brush)LightweightTheme.Office2019Black.Palette["Brush.Control.Background"];
			}
			return (Brush)LightweightTheme.Office2019Colorful.Palette["Brush.Control.Background"];
		}
	}

	public static Brush GetBorderBrush()
	{
		switch (SystemType)
		{
		case SystemTypes.Trade:
			if (IsDarkTheme)
			{
				return (Brush)LightweightTheme.Office2019BlackSpruceLeaves.Palette["Brush.Border"];
			}
			return (Brush)LightweightTheme.Office2019ColorfulSpruceLeaves.Palette["Brush.Border"];
		case SystemTypes.SalesManagement:
			if (IsDarkTheme)
			{
				return (Brush)LightweightTheme.Office2019BlackBrickwork.Palette["Brush.Border"];
			}
			return (Brush)LightweightTheme.Office2019ColorfulBrickwork.Palette["Brush.Border"];
		default:
			if (IsDarkTheme)
			{
				return (Brush)LightweightTheme.Office2019Black.Palette["Brush.Border"];
			}
			return (Brush)LightweightTheme.Office2019Colorful.Palette["Brush.Border"];
		}
	}

	public static Brush GetEditorBackground()
	{
		switch (SystemType)
		{
		case SystemTypes.Trade:
			if (IsDarkTheme)
			{
				return (Brush)LightweightTheme.Office2019BlackSpruceLeaves.Palette["Brush.Editor.Background"];
			}
			return (Brush)LightweightTheme.Office2019ColorfulSpruceLeaves.Palette["Brush.Editor.Background"];
		case SystemTypes.SalesManagement:
			if (IsDarkTheme)
			{
				return (Brush)LightweightTheme.Office2019BlackBrickwork.Palette["Brush.Editor.Background"];
			}
			return (Brush)LightweightTheme.Office2019ColorfulBrickwork.Palette["Brush.Editor.Background"];
		default:
			if (IsDarkTheme)
			{
				return (Brush)LightweightTheme.Office2019Black.Palette["Brush.Editor.Background"];
			}
			return (Brush)LightweightTheme.Office2019Colorful.Palette["Brush.Editor.Background"];
		}
	}

	public static Brush GetForegroundBrush()
	{
		switch (SystemType)
		{
		case SystemTypes.Trade:
			if (IsDarkTheme)
			{
				return (Brush)LightweightTheme.Office2019BlackSpruceLeaves.Palette["Brush.Foreground"];
			}
			return (Brush)LightweightTheme.Office2019ColorfulSpruceLeaves.Palette["Brush.Foreground"];
		case SystemTypes.SalesManagement:
			if (IsDarkTheme)
			{
				return (Brush)LightweightTheme.Office2019BlackBrickwork.Palette["Brush.Foreground"];
			}
			return (Brush)LightweightTheme.Office2019ColorfulBrickwork.Palette["Brush.Foreground"];
		default:
			if (IsDarkTheme)
			{
				return (Brush)LightweightTheme.Office2019Black.Palette["Brush.Foreground"];
			}
			return (Brush)LightweightTheme.Office2019Colorful.Palette["Brush.Foreground"];
		}
	}

	public static Color GetForegroundColor()
	{
		switch (SystemType)
		{
		case SystemTypes.Trade:
			if (IsDarkTheme)
			{
				return (Color)LightweightTheme.Office2019BlackSpruceLeaves.Palette["Color.Foreground"];
			}
			return (Color)LightweightTheme.Office2019ColorfulSpruceLeaves.Palette["Color.Foreground"];
		case SystemTypes.SalesManagement:
			if (IsDarkTheme)
			{
				return (Color)LightweightTheme.Office2019BlackBrickwork.Palette["Color.Foreground"];
			}
			return (Color)LightweightTheme.Office2019ColorfulBrickwork.Palette["Color.Foreground"];
		default:
			if (IsDarkTheme)
			{
				return (Color)LightweightTheme.Office2019Black.Palette["Color.Foreground"];
			}
			return (Color)LightweightTheme.Office2019Colorful.Palette["Color.Foreground"];
		}
	}

	public static Brush GetSelectedRowBrush()
	{
		if (IsDarkTheme)
		{
			return Brushes.DarkCyan;
		}
		return Brushes.Wheat;
	}

	public static Brush GetActiveRowBrush()
	{
		switch (SystemType)
		{
		case SystemTypes.Trade:
			if (IsDarkTheme)
			{
				return (Brush)LightweightTheme.Office2019BlackSpruceLeaves.Palette["Brush.HoverBorder"];
			}
			return (Brush)LightweightTheme.Office2019ColorfulSpruceLeaves.Palette["Brush.HoverBorder"];
		case SystemTypes.SalesManagement:
			if (IsDarkTheme)
			{
				return (Brush)LightweightTheme.Office2019BlackBrickwork.Palette["Brush.HoverBorder"];
			}
			return (Brush)LightweightTheme.Office2019ColorfulBrickwork.Palette["Brush.HoverBorder"];
		default:
			if (IsDarkTheme)
			{
				return (Brush)LightweightTheme.Office2019Black.Palette["Brush.HoverBorder"];
			}
			return (Brush)LightweightTheme.Office2019Colorful.Palette["Brush.HoverBorder"];
		}
	}

	public static Color GetActiveRowColor()
	{
		switch (SystemType)
		{
		case SystemTypes.Trade:
			if (IsDarkTheme)
			{
				return (Color)LightweightTheme.Office2019BlackSpruceLeaves.Palette["Color.HoverBorder"];
			}
			return (Color)LightweightTheme.Office2019ColorfulSpruceLeaves.Palette["Color.HoverBorder"];
		case SystemTypes.SalesManagement:
			if (IsDarkTheme)
			{
				return (Color)LightweightTheme.Office2019BlackBrickwork.Palette["Color.HoverBorder"];
			}
			return (Color)LightweightTheme.Office2019ColorfulBrickwork.Palette["Color.HoverBorder"];
		default:
			if (IsDarkTheme)
			{
				return (Color)LightweightTheme.Office2019Black.Palette["Color.HoverBorder"];
			}
			return (Color)LightweightTheme.Office2019Colorful.Palette["Color.HoverBorder"];
		}
	}

	public static bool IsApplicationThemeDark()
	{
		return RegistryHelper.ReadBoolFromRegistry("ApplicationThemeIsDark");
	}

	public static string ApplicationTheme()
	{
		return RegistryHelper.ReadStringFromRegistry("ApplicationTheme");
	}

	public static double GetFontSize(bool getFromRegistry = false)
	{
		if ((_fontSize == 0.0) | getFromRegistry)
		{
			_fontSize = RegistryHelper.ReadDoubleFromRegistry("FontSize", "", 12.0);
		}
		return _fontSize;
	}

	public static Brush RepBulletBackgroundBrush()
	{
		if (new GridRowThemeKeyExtension
		{
			ResourceKey = GridRowThemeKeys.BorderFocusedBrush,
			ThemeName = ApplicationThemeHelper.ApplicationThemeName
		}.ThemeName == "MetropolisLight")
		{
			return Brushes.LightGray;
		}
		return GetActiveRowBrush();
	}

	public static string GetTempPath()
	{
		EnsureTempPath();
		return _tempPath;
	}

	public static string GetTempFileName(string extension = ".tmp", string prephix = "")
	{
		EnsureTempPath();
		if (!extension.StartsWith('.'))
		{
			extension = "." + extension;
		}
		return _tempPath + prephix + Path.GetRandomFileName() + extension;
	}

	public static string GetTempFileNameByPrephix(string prephix, string extension = ".tmp")
	{
		int num = 2;
		if (!extension.StartsWith('.'))
		{
			extension = "." + extension;
		}
		string text = Path.Combine(_tempPath, prephix + extension);
		EnsureTempPath();
		bool flag = File.Exists(text);
		while (flag)
		{
			text = Path.Combine(_tempPath, prephix + "_" + num + extension);
			flag = File.Exists(text);
			num++;
		}
		return text;
	}

	private static void EnsureTempPath()
	{
		if (!Directory.Exists(_tempPath))
		{
			Directory.CreateDirectory(_tempPath);
		}
	}

	private static void CleanTemp()
	{
		IEnumerable<DirectoryInfo> enumerable = null;
		IEnumerable<FileInfo> enumerable2 = null;
		try
		{
			string tempPath = GetTempPath();
			if (!Directory.Exists(tempPath))
			{
				Directory.CreateDirectory(tempPath);
			}
			DirectoryInfo directoryInfo = new DirectoryInfo(tempPath);
			if (directoryInfo != null)
			{
				enumerable = directoryInfo.EnumerateDirectories();
				enumerable2 = directoryInfo.EnumerateFiles();
			}
		}
		catch (Exception)
		{
		}
		if (enumerable != null)
		{
			foreach (DirectoryInfo item in enumerable)
			{
				try
				{
					item.Delete(recursive: true);
				}
				catch (Exception)
				{
				}
			}
		}
		if (enumerable2 == null)
		{
			return;
		}
		foreach (FileInfo item2 in enumerable2)
		{
			try
			{
				item2.Delete();
			}
			catch (Exception)
			{
			}
		}
	}

	public static void ShowFileUsingDefaultApp(string filePath)
	{
		if (Path.GetExtension(filePath).Substring(1) == PrintTemplateFileFormatSE.PRNX)
		{
			new PreviewWindow(null, filePath, "").Show();
			return;
		}
		Process.Start(new ProcessStartInfo
		{
			FileName = filePath,
			UseShellExecute = true
		});
	}

	public static int ProcessId()
	{
		if (!_processId.HasValue)
		{
			using Process process = Process.GetCurrentProcess();
			_processId = process.Id;
		}
		return _processId.Value;
	}

	public static Guid GetMachineGuid()
	{
		string name = "SOFTWARE\\Microsoft\\Cryptography";
		try
		{
			using RegistryKey registryKey = Registry.LocalMachine.OpenSubKey(name);
			if (registryKey != null)
			{
				object value = registryKey.GetValue("MachineGuid");
				if (value != null)
				{
					return new Guid(value.ToString());
				}
			}
		}
		catch (Exception)
		{
		}
		return Guid.Empty;
	}

	public static string GetHostName()
	{
		try
		{
			return Dns.GetHostName();
		}
		catch (Exception)
		{
			return "";
		}
	}

	public static string GetDisplayResolution()
	{
		try
		{
			string text = "";
			foreach (Screen item in Screen.AllScreens.OrderByDescending((Screen p) => p.Primary))
			{
				text += $", {item.Bounds.Width}x{item.Bounds.Height}";
			}
			return text.Substring(2);
		}
		catch
		{
			return "";
		}
	}

	public static string GetDisplayeScale()
	{
		try
		{
			using Form form = new Form();
			float num = form.CreateGraphics().DpiX / 96f;
			return $"{num * 100f}%";
		}
		catch
		{
			return "";
		}
	}

	private static void TextBoxSelectAll(object sender, RoutedEventArgs e)
	{
		if (sender is DateEdit dateEdit)
		{
			dateEdit.SelectAll();
			e.Handled = true;
		}
		else if (sender is PopupCalcEdit popupCalcEdit)
		{
			popupCalcEdit.SelectAll();
			e.Handled = true;
		}
	}

	public static string ConvertCaptionToControlName(string prefix, string caption)
	{
		return prefix + "_" + caption.Replace(' ', '_').Replace('\\', '_').Replace('/', '_')
			.Replace('.', '_')
			.Replace(':', '_')
			.Replace('-', '_')
			.Replace('(', '_')
			.Replace(')', '_')
			.Replace(',', '_')
			.Replace('%', '_')
			.Replace('\'', '_')
			.Replace('’', '_')
			.Replace('`', '_')
			.Replace('?', '_')
			.Replace('+', '_')
			.Replace('$', '_')
			.Replace('․', '_')
			.Replace('\n', '_')
			.Replace('\r', '_');
	}

	public static void ConfigureLogger(IConfiguration configuration = null, Dictionary<string, string> additionalProperties = null)
	{
		if (configuration == null)
		{
			configuration = ServiceProvider.GetRequiredService<IConfiguration>();
		}
		bool flag = false;
		foreach (IConfigurationSection child in configuration.GetChildren())
		{
			if (child.Key == "Serilog")
			{
				flag = true;
				break;
			}
		}
		LoggerConfiguration loggerConfiguration = new LoggerConfiguration();
		if (flag)
		{
			loggerConfiguration.ReadFrom.Configuration(configuration);
		}
		else
		{
			loggerConfiguration.MinimumLevel.Warning();
			string text = (IsBetaVersion ? "n2BuxfDoHjTGtLDZVzQh" : "UJ8BUsgMYjPEB23CTn2F");
			LoggerSinkConfiguration writeTo = loggerConfiguration.WriteTo;
			string apiKey = text;
			writeTo.Seq("http://93.94.223.178:5341/", LogEventLevel.Verbose, 1000, null, apiKey, null, null, 262144L);
		}
		loggerConfiguration.Enrich.FromLogContext().Enrich.WithMachineName().Enrich.WithProperty("Application", SystemType.ToString() + " WPF client").Enrich.WithProperty("ProcessId", ProcessId()).Enrich.WithProperty("ClientVersion", AppVersion);
		LoginService loginService = ServiceProvider?.GetRequiredService<LoginService>();
		if (loginService != null && loginService.IsLoggedIn)
		{
			loggerConfiguration.Enrich.WithProperty("UserId", loginService.UserId).Enrich.WithProperty("UserName", loginService.UserName).Enrich.WithProperty("ServiceAddress", loginService.ServiceAddress).Enrich.WithProperty("Database", loginService.Database);
			if (additionalProperties != null)
			{
				foreach (KeyValuePair<string, string> additionalProperty in additionalProperties)
				{
					loggerConfiguration.Enrich.WithProperty(additionalProperty.Key, additionalProperty.Value);
				}
			}
		}
		Log.CloseAndFlush();
		Log.Logger = loggerConfiguration.CreateLogger();
		SelfLog.Enable(Console.Error);
	}

	public static void ErrorHandler(Exception ex, bool interactive = true)
	{
		ExceptionInfo exceptionInfo = null;
		bool showDetails = false;
		if (interactive && Mouse.OverrideCursor != null)
		{
			Mouse.OverrideCursor = null;
		}
		_ = HostingEnvironment;
		if (ex is FaultException ex2)
		{
			if (ex2.Code != null && !string.IsNullOrWhiteSpace(ex2.Code.Name))
			{
				string text = ex2.Code.Name.Left(1);
				if (text.InList("1".Left(1), "2".Left(1), "3".Left(1)))
				{
					ex = ((text == "1".Left(1)) ? new ASException(ex2.Message, 64) : ((!(text == "3".Left(1))) ? new ASException(ex2.Message) : new ASException(ex2.Message, 16)));
					showDetails = true;
				}
			}
		}
		else if (ex is EndpointNotFoundException)
		{
			ex = new ASException(ArmSoft.SME.Common.Resources.IBEndpointNotFound + "\r\n" + new string('-', 3) + "\r\n" + ex.Message, 48);
			showDetails = true;
		}
		else if (ex is XmlSchemaValidationException)
		{
			ex = new ASException(ArmSoft.SME.Common.Resources.XmlSchemaValidationError + "\r\n" + new string('-', 3) + "\r\n" + ex.Message, 48);
			showDetails = true;
		}
		else if (ex is ArmSoft.TaxService.ApiException)
		{
			ex = new ASException(ex.Message.Replace(ArmSoft.SME.Common.Resources.EInvoicingError, ""), ex.InnerException)
			{
				Title = ArmSoft.SME.Common.Resources.EInvoicing
			};
		}
		else if (ex is ArmSoft.InternetBank.APIClient.Core.ApiException ex3)
		{
			ex = new ASException(InternetBankHelper.GetIBApiExceptionMsg(ex3), ex.InnerException)
			{
				Title = ArmSoft.SME.Common.Resources.InternetBank
			};
		}
		else if (ex is ArmSoft.OpenBanking.Api.Client.Core.ApiException ex4)
		{
			ex = new ASException(InternetBankHelper.GetOBApiExceptionMsg(ex4), ex.InnerException)
			{
				Title = ArmSoft.SME.Common.Resources.InternetBank
			};
		}
		else if (ex is ArmSoft.Cloud.Client.RESTException ex5)
		{
			if (ex5.StatusCode >= HttpStatusCode.BadRequest && ex5.StatusCode < HttpStatusCode.InternalServerError)
			{
				ex = new ASException(ex.Message);
			}
		}
		else if (ex is B2BException ex6)
		{
			if (ex6.StatusCode == 400 || ex6.StatusCode == 404)
			{
				ex = new ASException(ex.Message);
			}
		}
		else if (ex is Utilities.ECRException { ErrCode: not -1 })
		{
			ex = new ASException(ex.Message);
		}
		if (!(ex is ASException) && !(ex is ArmSoft.SME.Common.Exceptions.RESTException) && !(ex is ArmSoft.SME.Client.ApiException { IsRestException: not false }))
		{
			if (ex.InnerException is SocketException)
			{
				ex = new ASException(ArmSoft.SME.Common.Resources.CloudServicesUnavailable);
			}
			else
			{
				Log.Error(ex, ex.Message);
			}
		}
		else if (ex is ArmSoft.SME.Client.ApiException ex9)
		{
			if (ex9.IsRestException && ex9.StatusCode != 403)
			{
				ex = new ASException(ex9.Message, ex9.FieldName)
				{
					ErrorCode = ex9.Code
				};
			}
			exceptionInfo = new ExceptionInfo
			{
				Details = ex9.ErrorDetails,
				StackTrace = ex9.ServiceStackTrace,
				IsRestException = ex9.IsRestException,
				Message = (ex9.InnerMessage ?? ex9.Message),
				Title = ex9.Title
			};
		}
		else if (ex is ArmSoft.SME.Common.Exceptions.RESTException ex10)
		{
			ex = new ASException(ex10.Message, ex10.FieldName)
			{
				ErrorCode = ex10.Code
			};
		}
		if (interactive && !(ex is InvisibleException) && !(ex is ArmSoft.SME.Common.Exceptions.InvisibleException))
		{
			new ErrorWindow(ex, showDetails, exceptionInfo).ShowDialog();
		}
	}

	internal static bool ShowTransactionsSupport()
	{
		return SystemType switch
		{
			SystemTypes.Accountant => true, 
			SystemTypes.BookKeeper => true, 
			SystemTypes.Trade => true, 
			SystemTypes.SalesManagement => true, 
			SystemTypes.MarketPlace => true, 
			_ => false, 
		};
	}

	public static string GetConfigValue(string settingName, string configFile = "")
	{
		try
		{
			string.IsNullOrWhiteSpace(configFile);
			if (File.Exists(configFile))
			{
				XPathNavigator xPathNavigator = new XPathDocument(configFile).CreateNavigator();
				string format = "/configuration/applicationSettings/{0}.{1}.WPFClient.MySettings/setting[@name='{2}']";
				XPathNavigator xPathNavigator2 = xPathNavigator.SelectSingleNode(string.Format(format, "ArmSoft", SystemType.ToString(), settingName));
				if (xPathNavigator2 != null)
				{
					return xPathNavigator2.Value;
				}
				xPathNavigator2 = xPathNavigator.SelectSingleNode(string.Format(format, "ArmenianSoftware", SystemType.ToString(), settingName));
				if (xPathNavigator2 != null)
				{
					return xPathNavigator2.Value;
				}
			}
		}
		catch (Exception)
		{
		}
		return null;
	}

	public static void RestoreWindowSize(Window wnd, RegistryKey rk, double defaultWidth, double defaultHeight)
	{
		if (double.TryParse(rk.GetValue("Width", defaultWidth).ToString(), out var result))
		{
			if (result > 0.0)
			{
				wnd.Width = result;
			}
			else
			{
				wnd.Width = defaultWidth;
			}
		}
		else
		{
			wnd.Width = defaultWidth;
		}
		if (double.TryParse(rk.GetValue("Height", defaultHeight).ToString(), out var result2))
		{
			if (result2 > 0.0)
			{
				wnd.Height = result2;
			}
			else
			{
				wnd.Height = defaultHeight;
			}
		}
		else
		{
			wnd.Height = defaultHeight;
		}
	}

	public static string GetExportDirectory()
	{
		string text = RegistryHelper.ReadStringFromRegistry("ExportDirectory", "", Directory.GetCurrentDirectory());
		if (!Directory.Exists(text))
		{
			text = Directory.GetCurrentDirectory();
		}
		return text;
	}

	public static string GetApplicationToken()
	{
		if (!string.IsNullOrWhiteSpace(_applicationToken) && ADHelper.CheckExpiration(_expiration.Value, new TimeSpan(0, 30, 0)))
		{
			return _applicationToken;
		}
		_applicationToken = CreateApiClient().ApplicationToken.GetApplicationToken(RESTClient.GetFingerPrint());
		_expiration = ADHelper.GetTokenExpiration(_applicationToken);
		return _applicationToken;
	}

	public static void ClearApplicationToken()
	{
		_applicationToken = "";
		_expiration = null;
	}

	public static void ThrowExceptionIfOffline()
	{
		if (IsOffline && !AllowOpInOfflineMode)
		{
			throw new ASException(ArmSoft.SME.Common.Resources.InvalidOperationForBrokenConnection);
		}
	}
}
