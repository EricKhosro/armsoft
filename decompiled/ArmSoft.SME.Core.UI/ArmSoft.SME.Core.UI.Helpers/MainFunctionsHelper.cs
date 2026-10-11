using System;
using System.Linq;
using System.Net.Http;
using System.Windows;
using ArmSoft.Cloud.Client;
using ArmSoft.Cloud.Client.AS7X;
using ArmSoft.SME.Client;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Parameters;
using ArmSoft.SME.Core.UI.PersistableObject;
using DevExpress.Xpf.Bars;

namespace ArmSoft.SME.Core.UI.Helpers;

public static class MainFunctionsHelper
{
	public static bool CheckOpenWindows
	{
		get
		{
			foreach (Window window in Application.Current.Windows)
			{
				if (window is IPOWindow)
				{
					return true;
				}
			}
			return false;
		}
	}

	public static BarButtonItemLink FindBarItemByNameInFavorites(string itmName)
	{
		foreach (BarItemLink item in Settings.HostingEnvironment.MainRibbonPanel.RibbonPageFavorites.Groups[0].ItemLinks.Cast<BarItemLink>())
		{
			if (item is BarButtonItemLink barButtonItemLink && barButtonItemLink.Item.Name == itmName)
			{
				return barButtonItemLink;
			}
		}
		return null;
	}

	public static BarButtonItem FindBarItemByName(BarManager barManager, string itmName)
	{
		foreach (BarItem item in barManager.Items)
		{
			if (item is BarButtonItem barButtonItem && barButtonItem.Name == itmName)
			{
				return barButtonItem;
			}
		}
		return null;
	}

	public static BarButtonItem FindBarItemByContent(BarManager barManager, string itmName)
	{
		foreach (BarItem item in barManager.Items)
		{
			if (item is BarButtonItem barButtonItem && barButtonItem.Content.ToString() == itmName)
			{
				return barButtonItem;
			}
		}
		return null;
	}

	public static void Activator()
	{
		Application.Current.MainWindow.Activate();
		if (Application.Current.MainWindow.WindowState == WindowState.Minimized)
		{
			Application.Current.MainWindow.WindowState = WindowState.Maximized;
		}
	}

	public static bool IsArmSoftCloudAvailable()
	{
		try
		{
			HttpClient httpClient = new HttpClient();
			AsyncHelper.RunSync(async () => await httpClient.GetStringAsync("https://cloud.armsoft.am/api/7X/APIVersion"));
			AsyncHelper.RunSync(async () => await httpClient.GetStringAsync("https://dbservices.armsoft.am/cloudserver001/swagger/v1/swagger.json"));
			return true;
		}
		catch (Exception)
		{
			return false;
		}
	}

	public static void CheckCloudDatabaseProperties()
	{
		if (Settings.SystemType == SystemTypes.Cabinet || (!(Settings.LoginService.TaxCode != Settings.ParametersService.TaxCode()) && !(Settings.LoginService.CompanyName != Settings.ParametersService.CompanyName()) && !(Settings.LoginService.DBVersion != Settings.ParametersService.DBVersionNew().ToString())))
		{
			return;
		}
		using CloudClient cloudClient = Settings.CloudClient;
		cloudClient.Token = Settings.GetApplicationToken();
		CloudDatabaseUpdateRequest request = new CloudDatabaseUpdateRequest
		{
			CompanyName = Settings.ParametersService.CompanyName(),
			TaxCode = Settings.ParametersService.TaxCode(),
			Version = Settings.ParametersService.DBVersionNew().ToString()
		};
		cloudClient.AS7X.UpdateCloudDatabase(Settings.LoginService.DatabaseId, request);
	}
}
