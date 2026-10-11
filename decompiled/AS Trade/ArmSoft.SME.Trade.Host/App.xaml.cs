using System.Reflection;
using System.Windows;
using System.Windows.Threading;
using ArmSoft.SME.Common;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Trade.Client.Models;
using ArmSoft.SME.Trade.Common;
using ArmSoft.SME.Trade.UI;
using DevExpress.Mvvm;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Core.Native;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace ArmSoft.SME.Trade.Host;

public partial class App : Application
{
	public static SplashScreenManager SplashScreen { get; private set; }

	static App()
	{
		CompatibilitySettings.UseLightweightThemes = true;
	}

	protected override void OnStartup(StartupEventArgs e)
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Expected Obj, but got Unknown
		base.OnStartup(e);
		DispatcherUnhandledException += App_DispatcherUnhandledException;
		new TradeSettings().Init(SystemTypes.Trade, Assembly.GetExecutingAssembly(), new Assembly[3]
		{
			typeof(ArmSoft.SME.Trade.Common.Resources).Assembly,
			typeof(Material).Assembly,
			typeof(TradeSettings).Assembly
		}, ConfigureServices);
		Settings.CreateTheme();
		SplashScreen = SplashScreenManager.Create(() => new SplashScreenWindow(), new DXSplashScreenViewModel
		{
			IsIndeterminate = false,
			Status = ""
		});
		SplashScreen.ShowOnStartup();
		SplashScreen.StateChanged += new SafeEventHandler<PropertyValueChangedEventArgs<SplashScreenState>>(SplashScreen_StateChanged).Handler;
	}

	private void SplashScreen_StateChanged(object sender, PropertyValueChangedEventArgs<SplashScreenState> e)
	{
		if (e.NewValue == SplashScreenState.Closing)
		{
			PreloadControlResources();
		}
	}

	private void App_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
	{
		Log.Error(e.Exception, "Unhandled exception");
		MessageBoxHelper.ShowError(e.Exception.Message, ArmSoft.SME.Common.Resources.UnhandledExceptionWord, MessageBoxButton.OK);
		Shutdown();
		e.Handled = true;
	}

	private static void PreloadControlResources()
	{
		ApplicationThemeHelper.Preload(PreloadCategories.Core, PreloadCategories.Controls, PreloadCategories.Grid, PreloadCategories.Docking, PreloadCategories.Dialogs, PreloadCategories.LayoutControl, PreloadCategories.Ribbon);
	}

	private void ConfigureServices(IServiceCollection services)
	{
	}
}
