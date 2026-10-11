using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Threading;
using AppLife.Api;
using ArmSoft.SME.Client;
using ArmSoft.SME.Common;
using ArmSoft.SME.Core.UI.Helpers;
using DevExpress.Xpf.Core;

namespace ArmSoft.SME.Core.UI;

public partial class AppUpdateWindow : ThemedWindow, IComponentConnector
{
	private enum UpdateState
	{
		ReadyToCheckUpdate,
		GetUpdateInfo,
		ForcedKeyUpdate,
		ReadyToDownload,
		VersionInfoNoFound,
		Downloading,
		DownloadError,
		ReadyToApplyUpdate
	}

	private UpdateState _state;

	private readonly string _recommendedUIVersionString;

	private readonly BackgroundWorker _worker = new BackgroundWorker();

	private readonly string _message = "";

	private readonly bool _useTestUpdate;

	private Version RecommendedUIVersion => new Version(_recommendedUIVersionString + ".0");

	public UpdateController Controller { get; set; }

	public AppUpdateWindow()
	{
		InitializeComponent();
		Settings.ThrowExceptionIfOffline();
		ApiClient apiClient = new ApiClient(Settings.LoginService.ServiceAddress, null, new HttpClient(), null);
		if (ConfigHelper.ASAdminLogin)
		{
			_recommendedUIVersionString = apiClient.AdminCloud.GetUIRecommendedVersion(ADHelper.ASAdminB2CToken, Settings.LoginService.Key).Version;
		}
		else
		{
			_recommendedUIVersionString = apiClient.AppManager.GetUIRecommendedVersion(ADHelper.ADB2CToken, Settings.LoginService.DatabaseId).Version;
		}
		Owner = Settings.HostingEnvironment;
		_useTestUpdate = Settings.IsBetaVersion;
		ASFontHelper.SetFontSettings(this);
		Title = ArmSoft.SME.Common.Resources.UpdateSystem;
		TBApplicationName.Text = Settings.ProgramName;
		TBCurrentVersion.Text = ArmSoft.SME.Common.Resources.VersionCapital + " " + Settings.AppVersionString + (_useTestUpdate ? $".{Settings.AppVersion.Revision}" : "");
		TBKey.Text = ArmSoft.SME.Common.Resources.HardwareKey + " " + Settings.LoginService.Key;
		TBUpdateInformation.Text = ArmSoft.SME.Common.Resources.UpdateInformation;
		LIVersion.Label = ArmSoft.SME.Common.Resources.VersionCapital;
		LIPublished.Label = ArmSoft.SME.Common.Resources.Published;
		LISize.Label = ArmSoft.SME.Common.Resources.Size;
		LISummary.Label = ArmSoft.SME.Common.Resources.Details;
		CancelButton.Content = ArmSoft.SME.Common.Resources.Cancel;
		_state = UpdateState.ReadyToCheckUpdate;
		SizeCalculator.ConfigureLine(this, Line1);
		SetUI();
		if (Settings.SystemType == SystemTypes.SalesManagement)
		{
			Controller = new UpdateController
			{
				ApplicationId = new Guid("d137f73b-9b06-48c7-975b-f218a009c86b"),
				PublicKeyToken = "<RSAKeyValue><Modulus>uUy+MNhTF5dh8XLIWicPl4uLCFW4kBoiGIFODgYonc937eW88G6ZJN69v4ZPR3EtMZEETeFLIxWGgQtKidqlbSMrhjBoGCuHRMbLw63t5vI22noDNeGHlJuyvEscMAWy5cCWprUD38K0r4S8xwjnvzRAuhl8hMoXkVbX+pGXvXk=</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>",
				UpdateLocation = (_useTestUpdate ? "\\\\host2\\Sys\\Developers\\TestEnvironment\\Updates\\PreRelease\\SM_UI" : "https://update.armsoft.am/smUI/"),
				UseHostAssemblyVersion = false,
				ElevationType = UpdateElevationType.Uac
			};
		}
		else if (Settings.SystemType == SystemTypes.Trade)
		{
			Controller = new UpdateController
			{
				ApplicationId = new Guid("13cd4e9c-f1d3-4ed2-ab3d-80aca81678e8"),
				PublicKeyToken = "<RSAKeyValue><Modulus>1ce6z07jUnLQyKD/GZOTPPNVmQQkmIo3B0SrNN0ni+YZ8DBSpExvxQYCuwYXxOi1La8B2IlyZ8lVUxFRIebi4PlcWTQ/3IKJQfPln1blHeFY5BgaaZneSatN1lfPbWfpUDp4bthEcTKOZ+0xZxOgJ2J2gMi8326qwGKxNGxpsx0=</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>",
				UpdateLocation = (_useTestUpdate ? "\\\\host2\\Sys\\Developers\\TestEnvironment\\Updates\\PreRelease\\TradeUI" : "https://update.armsoft.am/tradeUI/"),
				UseHostAssemblyVersion = false,
				ElevationType = UpdateElevationType.Uac
			};
		}
		else if (Settings.SystemType == SystemTypes.Accountant)
		{
			Controller = new UpdateController
			{
				ApplicationId = new Guid("8f6be10e-0467-4360-ac99-9b4dd6a7a55e"),
				PublicKeyToken = "<RSAKeyValue><Modulus>7Ewf+lucPoeJRq7wuoHcW0xGO6d9dSlM9b90lfty2m+NYqP97Z8KdY/yLbDuk2pXLejSZigxRyDfsmSDqHg43Cz5FvVaTik8XKcluwWV8UyzI1GZ7Yo31B7DPtQ6yTXMz05iBaxPXvn+GKF+/o9fIDPtPukIda7E0QrRzLH1kmU=</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>",
				UpdateLocation = (_useTestUpdate ? "\\\\host2\\Sys\\Developers\\TestEnvironment\\Updates\\PreRelease\\AccountantUI" : "https://update.armsoft.am/accountantUI/"),
				UseHostAssemblyVersion = false,
				ElevationType = UpdateElevationType.Uac
			};
		}
		else
		{
			if (Settings.SystemType != SystemTypes.MarketPlace)
			{
				throw new ASException("Invalid system type");
			}
			Controller = new UpdateController
			{
				ApplicationId = new Guid("bff89b4b-972e-4b2f-ba24-5c41a2f2c71a"),
				PublicKeyToken = "<RSAKeyValue><Modulus>xMVglIaVHwCCenJdwoGYYBhP6KKO4Kp7Fjh5Ypa1e3lDSGgeyIc8/bjbDUUroFTgZo1EA/ltGTylvd6NNxnhy1P4AUVUbwm0cLxze6nwGLvl0eYANJRt2uOPhGihmUJ4OIlez6ZozTx268gsVcyKC8QDjU3I3ItLV2gdRjJX6QE=</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>",
				UpdateLocation = "https://update.armsoft.am/marketplace7/",
				UseHostAssemblyVersion = false,
				ElevationType = UpdateElevationType.Uac
			};
		}
		Controller.AutoDetectProxy = true;
		_worker.DoWork += new SafeEventHandler<DoWorkEventArgs>(Worker_DoWork).Handler;
		_worker.RunWorkerCompleted += new SafeEventHandler<RunWorkerCompletedEventArgs>(Worker_RunWorkerCompleted).Handler;
		((DispatcherObject)this).Dispatcher.BeginInvoke((Delegate)new Action(CheckUpdates), Array.Empty<object>());
		Closing += AppUpdateWindow_Closing;
	}

	private void CheckUpdates()
	{
		try
		{
			_state = UpdateState.GetUpdateInfo;
			TBUpdateState.Text = ArmSoft.SME.Common.Resources.CheckForUpdates;
			SetUI();
			_worker.RunWorkerAsync();
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
			TBUpdateState.Text = "";
			_state = UpdateState.ReadyToCheckUpdate;
			SetUI();
		}
	}

	private void Worker_DoWork(object sender, DoWorkEventArgs e)
	{
		LanguageService.RestoreCulture(Settings.GetCurrentCultureName());
	}

	private void Worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
	{
		try
		{
			if (_useTestUpdate || Settings.AppVersion < RecommendedUIVersion)
			{
				TBUpdateState.Text = ArmSoft.SME.Common.Resources.UpdateAvailable;
				TBVersion.Text = _recommendedUIVersionString;
				Controller.CheckForUpdateCompleted += Controller_CheckForUpdateCompleted;
				Controller.CheckForUpdateAsync();
			}
			else
			{
				if (string.IsNullOrWhiteSpace(_message))
				{
					TBUpdateState.Text = ArmSoft.SME.Common.Resources.NoUpdatesAreFound;
				}
				else
				{
					MessageBox.Show(_message, ArmSoft.SME.Common.Resources.CheckForUpdates);
					TBUpdateState.Text = "";
				}
				_state = UpdateState.ReadyToCheckUpdate;
			}
			SetUI();
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
			TBUpdateState.Text = "";
			_state = UpdateState.ReadyToCheckUpdate;
			SetUI();
		}
	}

	private void SetUI()
	{
		//IL_012d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0133: Unknown result type (might be due to invalid IL or missing references)
		//IL_0167: Unknown result type (might be due to invalid IL or missing references)
		//IL_016d: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0251: Unknown result type (might be due to invalid IL or missing references)
		//IL_0257: Unknown result type (might be due to invalid IL or missing references)
		//IL_029b: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		DownloadAnimation48.Visibility = Visibility.Collapsed;
		checkingAnimation48.Visibility = Visibility.Collapsed;
		popupStaticImage.Visibility = Visibility.Visible;
		switch (_state)
		{
		case UpdateState.ReadyToCheckUpdate:
			OkButton.IsEnabled = true;
			CancelButton.IsEnabled = true;
			OkButton.Content = ArmSoft.SME.Common.Resources.Check;
			if (TBUpdateState.Text == ArmSoft.SME.Common.Resources.NoUpdatesAreFound)
			{
				popupStaticImage.Source = ImageService.GetImage("accept_32.svg");
			}
			else
			{
				popupStaticImage.Source = ImageService.GetImage("checking_update_48.svg");
			}
			break;
		case UpdateState.GetUpdateInfo:
			OkButton.IsEnabled = false;
			CancelButton.IsEnabled = false;
			checkingAnimation48.Visibility = Visibility.Visible;
			popupStaticImage.Visibility = Visibility.Collapsed;
			break;
		case UpdateState.ForcedKeyUpdate:
			OkButton.IsEnabled = false;
			CancelButton.IsEnabled = true;
			popupStaticImage.Source = ImageService.GetImage("error_32.svg");
			break;
		case UpdateState.ReadyToDownload:
			OkButton.IsEnabled = true;
			CancelButton.IsEnabled = true;
			popupStaticImage.Source = ImageService.GetImage("software_update_available_32.svg");
			OkButton.Content = ArmSoft.SME.Common.Resources.Download;
			break;
		case UpdateState.VersionInfoNoFound:
			OkButton.IsEnabled = false;
			CancelButton.IsEnabled = true;
			popupStaticImage.Source = ImageService.GetImage("error_32.svg");
			TBUpdateState.Text = string.Format(ArmSoft.SME.Common.Resources.UpdateDescriptionNotFound, _recommendedUIVersionString);
			break;
		case UpdateState.Downloading:
			OkButton.IsEnabled = false;
			CancelButton.IsEnabled = true;
			DownloadAnimation48.Visibility = Visibility.Visible;
			popupStaticImage.Visibility = Visibility.Collapsed;
			TBUpdateState.Text = ArmSoft.SME.Common.Resources.Downloaded + " 0%";
			break;
		case UpdateState.DownloadError:
			OkButton.IsEnabled = false;
			CancelButton.IsEnabled = true;
			popupStaticImage.Source = ImageService.GetImage("error_32.svg");
			TBUpdateState.Text = ArmSoft.SME.Common.Resources.DownloadError;
			break;
		case UpdateState.ReadyToApplyUpdate:
			OkButton.IsEnabled = true;
			CancelButton.IsEnabled = true;
			popupStaticImage.Source = ImageService.GetImage("software_update_available_32.svg");
			TBUpdateState.Text = ArmSoft.SME.Common.Resources.ReadyToInstallUpdate;
			OkButton.Content = ArmSoft.SME.Common.Resources.Install;
			break;
		}
	}

	private void AppUpdateWindow_Closing(object sender, CancelEventArgs e)
	{
		if (_worker.IsBusy)
		{
			e.Cancel = true;
		}
		else if (Controller.IsBusy)
		{
			Controller.CancelAsync();
		}
	}

	private void Controller_CheckForUpdateCompleted(object sender, CheckForUpdateCompletedEventArgs e)
	{
		Controller.CheckForUpdateCompleted -= Controller_CheckForUpdateCompleted;
		if (e.Error != null)
		{
			Settings.ErrorHandler(e.Error);
			TBUpdateState.Text = "";
			_state = UpdateState.ReadyToCheckUpdate;
			SetUI();
		}
		else
		{
			if (e.Cancelled)
			{
				return;
			}
			if (e.Result)
			{
				StringBuilder stringBuilder = new StringBuilder();
				bool flag = false;
				for (int i = 0; i <= Controller.AllUpdates.Count - 1; i++)
				{
					UpdateInformation updateInformation = Controller.AllUpdates[i];
					if (Settings.AppVersion < updateInformation.Version && _useTestUpdate)
					{
						TBVersion.Text = updateInformation.Version.ToString();
						stringBuilder.Insert(0, Environment.NewLine);
						stringBuilder.Insert(0, Environment.NewLine);
						if (updateInformation.SimpleSummary.IndexOf("Version") > 0)
						{
							if (Settings.GetCurrentCultureName().Equals("hy-AM", StringComparison.CurrentCultureIgnoreCase))
							{
								stringBuilder.Insert(0, MemoryExtensions.AsSpan(updateInformation.SimpleSummary, 0, updateInformation.SimpleSummary.IndexOf("Version")));
							}
							else
							{
								stringBuilder.Insert(0, MemoryExtensions.AsSpan(updateInformation.SimpleSummary, updateInformation.SimpleSummary.IndexOf("Version")));
							}
						}
						else
						{
							stringBuilder.Insert(0, updateInformation.SimpleSummary);
						}
						flag = true;
						if (Settings.AppVersion >= updateInformation.Version)
						{
							Controller.Version = new Version(updateInformation.Version.Major, 0, 0, 0);
						}
						else
						{
							Controller.Version = Settings.AppVersion;
						}
						Controller.CurrentUpdate = updateInformation;
						break;
					}
					if (Settings.AppVersion == updateInformation.Version)
					{
						break;
					}
					if (!(updateInformation.Version == RecommendedUIVersion))
					{
						continue;
					}
					stringBuilder.Insert(0, Environment.NewLine);
					stringBuilder.Insert(0, Environment.NewLine);
					if (updateInformation.SimpleSummary.IndexOf("Version") > 0)
					{
						if (Settings.GetCurrentCultureName().Equals("hy-AM", StringComparison.CurrentCultureIgnoreCase))
						{
							stringBuilder.Insert(0, MemoryExtensions.AsSpan(updateInformation.SimpleSummary, 0, updateInformation.SimpleSummary.IndexOf("Version")));
						}
						else
						{
							stringBuilder.Insert(0, MemoryExtensions.AsSpan(updateInformation.SimpleSummary, updateInformation.SimpleSummary.IndexOf("Version")));
						}
					}
					else
					{
						stringBuilder.Insert(0, updateInformation.SimpleSummary);
					}
					flag = true;
					if (Settings.AppVersion >= updateInformation.Version)
					{
						Controller.Version = new Version(updateInformation.Version.Major, 0, 0, 0);
					}
					else
					{
						Controller.Version = Settings.AppVersion;
					}
					Controller.CurrentUpdate = updateInformation;
					break;
				}
				if (flag)
				{
					TBPublished.Text = Controller.CurrentUpdate.PostedDate.ToString("d MMM yyyy");
					TBSize.Text = FormatBytes(Controller.CurrentUpdate.FileSize);
					TBSummary.Text = stringBuilder.ToString();
					_state = UpdateState.ReadyToDownload;
					SetUI();
				}
				else
				{
					_state = UpdateState.VersionInfoNoFound;
					SetUI();
				}
			}
			else
			{
				_state = UpdateState.VersionInfoNoFound;
				SetUI();
			}
		}
	}

	private static string FormatBytes(long bytesCaller)
	{
		if (bytesCaller >= 1099511627776L)
		{
			return ((double)bytesCaller / 1099511627776.0).ToString("0.##") + " TB";
		}
		if (1073741824 <= bytesCaller && bytesCaller <= 1099511627775L)
		{
			return ((double)bytesCaller / 1073741824.0).ToString("0.##") + " GB";
		}
		if (1048576 <= bytesCaller && bytesCaller <= 1073741823)
		{
			return ((double)bytesCaller / 1048576.0).ToString("0.##") + " MB";
		}
		if (1024 <= bytesCaller && bytesCaller <= 1048575)
		{
			return ((double)bytesCaller / 1024.0).ToString("0.##") + " KB";
		}
		if (0 <= bytesCaller && bytesCaller <= 1023)
		{
			return ((double)bytesCaller).ToString("0.##") + " bytes";
		}
		return "";
	}

	private void OkButton_Click(object sender, RoutedEventArgs e)
	{
		if (_state == UpdateState.ReadyToCheckUpdate)
		{
			CheckUpdates();
		}
		else if (_state == UpdateState.ReadyToDownload)
		{
			_state = UpdateState.Downloading;
			SetUI();
			Controller.DownloadUpdateCompleted += Controller_DownloadUpdateCompleted;
			Controller.DownloadUpdateProgressChanged += Controller_DownloadUpdateProgressChanged;
			Controller.DownloadUpdateAsync();
		}
		else if (_state == UpdateState.ReadyToApplyUpdate)
		{
			try
			{
				CheckForBlockingProcesses();
				DialogResult = true;
			}
			catch (Exception ex)
			{
				Settings.ErrorHandler(ex);
			}
		}
	}

	private static void CheckForBlockingProcesses()
	{
		try
		{
			if (Process.GetProcessesByName("excel").Length != 0)
			{
				throw new ASException(ArmSoft.SME.Common.Resources.CloseMSExcel);
			}
		}
		catch (Win32Exception)
		{
		}
		Process currentProcess = Process.GetCurrentProcess();
		string value = NormalizeProcessPath(currentProcess.MainModule?.FileName);
		if (string.IsNullOrWhiteSpace(value))
		{
			return;
		}
		Process[] processesByName = Process.GetProcessesByName(currentProcess.ProcessName);
		foreach (Process process in processesByName)
		{
			if (process.Id == currentProcess.Id)
			{
				continue;
			}
			try
			{
				string text = process.MainModule?.FileName;
				if (string.IsNullOrWhiteSpace(text) || !NormalizeProcessPath(text).Equals(value, StringComparison.OrdinalIgnoreCase))
				{
					continue;
				}
				throw new ASException(string.Format(ArmSoft.SME.Common.Resources.CloseOtherRunningApplications, Settings.ProgramName));
			}
			catch (AccessViolationException)
			{
			}
			catch (Win32Exception)
			{
			}
		}
	}

	private static string NormalizeProcessPath(string path)
	{
		if (string.IsNullOrWhiteSpace(path))
		{
			return "";
		}
		try
		{
			return Path.GetFullPath(path).ToLowerInvariant();
		}
		catch
		{
			return path.ToLowerInvariant();
		}
	}

	private void Controller_DownloadUpdateProgressChanged(object sender, DownloadUpdateProgressChangedEventArgs e)
	{
		TBUpdateState.Text = ArmSoft.SME.Common.Resources.Downloaded + " " + e.ProgressPercentage + "%";
	}

	private void Controller_DownloadUpdateCompleted(object sender, AsyncCompletedEventArgs e)
	{
		Controller.DownloadUpdateCompleted -= Controller_DownloadUpdateCompleted;
		Controller.DownloadUpdateProgressChanged -= Controller_DownloadUpdateProgressChanged;
		if (e.Error != null)
		{
			Settings.ErrorHandler(e.Error);
			_state = UpdateState.DownloadError;
			SetUI();
		}
		else if (!e.Cancelled)
		{
			_state = UpdateState.ReadyToApplyUpdate;
			SetUI();
		}
	}
}
