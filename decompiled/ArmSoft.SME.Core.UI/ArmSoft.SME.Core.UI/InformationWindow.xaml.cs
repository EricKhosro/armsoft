using System;
using System.Diagnostics;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Threading;
using ArmSoft.SME.Common;
using ArmSoft.SME.Core.UI.Constants;
using ArmSoft.SME.Core.UI.HelpClasses;
using ArmSoft.SME.Core.UI.Helpers;
using DevExpress.Xpf.Core;
using Microsoft.Web.WebView2.Core;
using Microsoft.Win32;

namespace ArmSoft.SME.Core.UI;

public partial class InformationWindow : ThemedWindow, IComponentConnector
{
	private bool _firstPage = true;

	private readonly HeaderItemControl _reloadHeaderItem;

	public InformationWindowCloseDelegate InformationWindowCloseDelegate { get; }

	public InformationWindow(InformationWindowCloseDelegate informationWindowCloseDelegate = null)
	{
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		InitializeComponent();
		InitializeAsync();
		Owner = Settings.HostingEnvironment;
		Title = ArmSoft.SME.Common.Resources.ASNews;
		DontShowNextTimeButton.Content = ArmSoft.SME.Common.Resources.DontShowNextTime;
		CloseButton.Content = ArmSoft.SME.Common.Resources.Close;
		InformationWindowCloseDelegate = informationWindowCloseDelegate;
		if (!Settings.IsWin11Theme)
		{
			WindowStyle = WindowStyle.ToolWindow;
		}
		DXImage content = new DXImage
		{
			Source = ImageService.GetImage("refresh_small_16.svg"),
			Width = 18.0,
			Height = 18.0,
			SnapsToDevicePixels = true,
			UseLayoutRounding = true,
			Stretch = Stretch.Uniform
		};
		_reloadHeaderItem = new HeaderItemControl
		{
			Name = "Reload",
			Content = content,
			ToolTip = ArmSoft.SME.Common.Resources.RefreshTooltip
		};
		ASFontHelper.SetFontSettings(this);
		_reloadHeaderItem.Click += new SafeEventHandler<RoutedEventArgs>(ReloadHeaderItem_Click).Handler;
		KeyDown += new SafeEventHandler<KeyEventArgs>(InformationWindow_KeyDown).Handler;
		HeaderItems.Add(_reloadHeaderItem);
	}

	private async void InitializeAsync()
	{
		_ = 3;
		try
		{
			CoreWebView2Environment environment = await CoreWebView2Environment.CreateAsync(null, Settings.GetTempPath(), new CoreWebView2EnvironmentOptions());
			await webView.EnsureCoreWebView2Async(environment);
			webView.CoreWebView2.Settings.AreBrowserAcceleratorKeysEnabled = false;
			webView.CoreWebView2.Settings.AreDefaultContextMenusEnabled = false;
			webView.CoreWebView2.Settings.AreDevToolsEnabled = false;
			webView.CoreWebView2.Settings.IsZoomControlEnabled = false;
			webView.CoreWebView2.Settings.IsStatusBarEnabled = false;
			await NavigateToAsync(webView.CoreWebView2, Settings.HostingEnvironment.AdsUri.ToString());
			if (webView.Height == 0.0 || double.IsNaN(webView.Height))
			{
				await SetWidthAndHeigh();
			}
			CloseButton.Focus();
		}
		catch (Exception)
		{
		}
	}

	public static (string src, double width, double height) ParseImgAttributes(string html)
	{
		Match match = new Regex("<img[^>]+src\\s*=\\s*['\"]([^'\"]+)['\"][^>]*width\\s*=\\s*['\"](\\d+)['\"][^>]*height\\s*=\\s*['\"](\\d+)['\"][^>]*>", RegexOptions.IgnoreCase).Match(html);
		if (!match.Success)
		{
			throw new InvalidOperationException("No <img> tag with the required attributes found in the provided HTML.");
		}
		string value = match.Groups[1].Value;
		double item = double.Parse(match.Groups[2].Value);
		double item2 = double.Parse(match.Groups[3].Value);
		return (src: value, width: item, height: item2);
	}

	private async Task SetWidthAndHeigh()
	{
		string javaScript = "document.getElementsByTagName('img')[0].height";
		if (int.TryParse(await webView.ExecuteScriptAsync(javaScript), out var result))
		{
			webView.Height = result;
		}
		javaScript = "document.getElementsByTagName('img')[0].width";
		if (int.TryParse(await webView.ExecuteScriptAsync(javaScript), out var result2))
		{
			webView.Width = result2;
		}
	}

	public static Task NavigateToAsync(CoreWebView2 coreWebView2, string url)
	{
		TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>(TaskCreationOptions.RunContinuationsAsynchronously);
		coreWebView2.NavigationCompleted += evt;
		coreWebView2.Navigate(url);
		return tcs.Task;
		void evt(object s, CoreWebView2NavigationCompletedEventArgs args)
		{
			coreWebView2.NavigationCompleted -= evt;
			if (args.IsSuccess)
			{
				tcs.TrySetResult(result: true);
			}
			else
			{
				tcs.TrySetException(new ASException("Navigation failed with error:" + args.WebErrorStatus));
			}
		}
	}

	private void CloseButton_Click(object sender, RoutedEventArgs e)
	{
		Close();
	}

	private void DontShowNextTimeButton_Click(object sender, RoutedEventArgs e)
	{
		DontShowNextTime();
		Close();
	}

	private static void DontShowNextTime()
	{
		RegistryHelper.StoreToRegistry("ShowArmsoftInfo", false);
	}

	public static bool InfoIsActive()
	{
		bool flag = true;
		using RegistryKey registryKey = Registry.CurrentUser.OpenSubKey(Settings.RegistryKey, writable: true);
		if (registryKey != null)
		{
			flag = Convert.ToBoolean(registryKey.GetValue("ShowArmsoftInfo", true));
			if (!flag)
			{
				object value = registryKey.GetValue("ArmsoftInfoLastDate", null);
				if (value is DateTime dateTime)
				{
					flag = DateTime.Now - dateTime >= TimeSpan.FromDays(7);
				}
				else if (value is string value2)
				{
					flag = DateTime.Now - Convert.ToDateTime(value2) >= TimeSpan.FromDays(7);
				}
			}
			if (flag)
			{
				registryKey.SetValue("ArmsoftInfoLastDate", DateTime.Now.Date);
			}
		}
		return flag;
	}

	public static void ShowIfActive(InformationWindowCloseDelegate informationWindowCloseDelegate = null)
	{
		Task.Run(async () =>
		{
			if (InfoIsActive())
			{
				try
				{
					(string, double, double) tuple = ParseImgAttributes(await new HttpClient().GetStringAsync(Settings.HostingEnvironment.AdsUri.ToString()));
					double width = tuple.Item2;
					double height = tuple.Item3;
					((DispatcherObject)Application.Current).Dispatcher.Invoke((Action)(() =>
					{
						InformationWindow informationWindow = new InformationWindow(informationWindowCloseDelegate);
						informationWindow.webView.Height = height;
						informationWindow.webView.Width = width;
						informationWindow.ShowDialog();
					}));
				}
				catch
				{
				}
			}
		});
	}

	private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		Size newSize = e.NewSize;
		double width = newSize.Width;
		newSize = e.NewSize;
		CenterWindowOnScreen(width, newSize.Height);
	}

	private void CenterWindowOnScreen(double windowWidth, double windowHeight)
	{
		double primaryScreenWidth = SystemParameters.PrimaryScreenWidth;
		double primaryScreenHeight = SystemParameters.PrimaryScreenHeight;
		Left = primaryScreenWidth / 2.0 - windowWidth / 2.0;
		Top = primaryScreenHeight / 2.0 - windowHeight / 2.0;
	}

	private void WB_Navigating(object sender, CoreWebView2NavigationStartingEventArgs e)
	{
		if (_firstPage)
		{
			_firstPage = false;
			return;
		}
		e.Cancel = true;
		Process.Start(new ProcessStartInfo
		{
			FileName = e.Uri,
			UseShellExecute = true
		});
	}

	private void ThemedWindow_Closed(object sender, EventArgs e)
	{
		try
		{
			if (InformationWindowCloseDelegate != null)
			{
				InformationWindowCloseDelegate();
			}
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	private void InformationWindow_KeyDown(object sender, KeyEventArgs e)
	{
		if (HotKeys.WithModifier.Control_R.Matches(this, e))
		{
			Reload();
		}
	}

	private void ReloadHeaderItem_Click(object sender, RoutedEventArgs e)
	{
		Reload();
	}

	private async void Reload()
	{
		await webView.CoreWebView2.Profile.ClearBrowsingDataAsync(CoreWebView2BrowsingDataKinds.DiskCache);
		_firstPage = true;
		webView.CoreWebView2.Reload();
	}
}
