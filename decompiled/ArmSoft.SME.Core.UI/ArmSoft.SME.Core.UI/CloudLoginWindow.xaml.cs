using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Markup;
using System.Windows.Media;
using ArmSoft.Cloud.Client;
using ArmSoft.Cloud.Client.AS7X;
using ArmSoft.SME.Client;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Models.Cloud;
using ArmSoft.SME.Core.UI.BackgroundWorks;
using ArmSoft.SME.Core.UI.DataViews;
using ArmSoft.SME.Core.UI.HelpClasses;
using ArmSoft.SME.Core.UI.Helpers;
using ArmSoft.SME.Core.UI.Localizers;
using ArmSoft.SME.Core.UI.Services;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Editors;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Identity.Client;
using Microsoft.Win32;
using Serilog;

namespace ArmSoft.SME.Core.UI;

public partial class CloudLoginWindow : ThemedWindow, IComponentConnector, IStyleConnector
{
	public class LanguageComboBoxItem
	{
		public ImageSource Image { get; set; }

		public string LanguageCode { get; set; }

		public string LanguageCaption { get; set; }
	}

	private AuthenticationResult _authResult;

	private UserLoginResponseV2 _userLoginResponse;

	private AvailableDatabase _selectedDatabase;

	private List<AvailableDatabase> _cloudDBSourceList;

	private List<Database> _dbList;

	private readonly BindingList<string> _loginHints;

	private readonly BindingList<User> _users = new BindingList<User>();

	private const int _loginHintsMaxCount = 10;

	private bool _firstActivated;

	private string _loginHint;

	private readonly int _lastCloudDBId;

	private readonly string _lastADB2CLogin;

	public CloudLoginWindow()
	{
		//IL_0122: Unknown result type (might be due to invalid IL or missing references)
		//IL_0128: Unknown result type (might be due to invalid IL or missing references)
		//IL_0310: Unknown result type (might be due to invalid IL or missing references)
		//IL_0316: Unknown result type (might be due to invalid IL or missing references)
		//IL_034e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0354: Unknown result type (might be due to invalid IL or missing references)
		//IL_038c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0392: Unknown result type (might be due to invalid IL or missing references)
		InitializeComponent();
		DataContext = this;
		Owner = Settings.HostingEnvironment;
		_firstActivated = true;
		_authResult = null;
		CloudDBList.CodeEdit.FontSize = 13.0;
		CloudDBList.IsRequired = false;
		CloudDBList.Owner = this;
		CloudDBList.ShowComment = false;
		CloudDBList.InnerCodeEdit.Style = (Style)Resources["DBListInnerCodeEditStyle"];
		string text = "";
		List<User> list = new List<User>();
		string[] valueNames;
		using (RegistryKey registryKey = Registry.CurrentUser.OpenSubKey(Settings.RegistryKey))
		{
			using (RegistryKey registryKey2 = Registry.CurrentUser.CreateSubKey(Settings.RegistryKey + "\\CloudLogIns"))
			{
				if (registryKey2 != null)
				{
					valueNames = registryKey2.GetValueNames();
					foreach (string text2 in valueNames)
					{
						string name = (string)registryKey2.GetValue(text2, "");
						list.Add(new User
						{
							Name = name,
							Mail = text2,
							Image = ImageService.GetImage("user_24in32"),
							UserVisible = "Visible",
							OtherVisible = "Collapsed"
						});
					}
				}
			}
			if (registryKey != null)
			{
				if (ConfigHelper.ASAdminLogin)
				{
					SaveUser.IsChecked = (string)registryKey.GetValue("CloudSaveUserASAdmin", "1") == "1";
					_loginHint = (string)registryKey.GetValue("CloudLastASAdminLogin", "");
					_lastADB2CLogin = (string)registryKey.GetValue("LastADB2CLoginASAdmin", "");
					_lastCloudDBId = (int)registryKey.GetValue("CloudLastDBIdASAdmin", 0);
				}
				else
				{
					SaveUser.IsChecked = (string)registryKey.GetValue("CloudSaveUser", "1") == "1";
					_loginHint = (string)registryKey.GetValue("CloudLastLogin", "");
					_lastADB2CLogin = (string)registryKey.GetValue("LastADB2CLogin", "");
					_ = (string)registryKey.GetValue("LastDB", "");
					text = (string)registryKey.GetValue("CloudLastLogins", "");
					_lastCloudDBId = (ConfigHelper.IsPublicSector ? ((int)registryKey.GetValue("CloudLastPSDBId", 0)) : ((int)registryKey.GetValue("CloudLastDBId", 0)));
				}
			}
			else
			{
				SaveUser.IsChecked = true;
			}
		}
		List<LanguageComboBoxItem> itemsSource = new List<LanguageComboBoxItem>
		{
			new LanguageComboBoxItem
			{
				Image = ImageService.GetImage("flag_armenia_16.svg", isAppPathPreferred: false, null, default, usePalette: false),
				LanguageCode = "hy-AM",
				LanguageCaption = "Հայերեն"
			},
			new LanguageComboBoxItem
			{
				Image = ImageService.GetImage("flag_great_britain_16.svg", isAppPathPreferred: false, null, default, usePalette: false),
				LanguageCode = "en-US",
				LanguageCaption = "English"
			},
			new LanguageComboBoxItem
			{
				Image = ImageService.GetImage("flag_russia_16.svg", isAppPathPreferred: false, null, default, usePalette: false),
				LanguageCode = "ru-RU",
				LanguageCaption = "Russian"
			}
		};
		language.ItemsSource = itemsSource;
		language.Background = Settings.GetEditorBackground();
		language.EditValueChanged += new SafeEventHandler<EditValueChangedEventArgs>(Language_SelectionChanged).Handler;
		language.SelectedIndex = ((!LanguageService.IsArmenian) ? (LanguageService.IsEnglish ? 1 : 2) : 0);
		string[] array = text.Split(',');
		_loginHints = new BindingList<string>();
		if (list != null)
		{
			foreach (User item2 in list)
			{
				if (string.IsNullOrWhiteSpace(item2.Name))
				{
					item2.Name = item2.Mail;
				}
				_users.Add(item2);
			}
		}
		if (!string.IsNullOrWhiteSpace(_loginHint))
		{
			_loginHints.Add(_loginHint.Trim());
		}
		valueNames = array;
		foreach (string text3 in valueNames)
		{
			string trimmedLoginHint = text3.Trim();
			if (!string.IsNullOrEmpty(trimmedLoginHint) && !_loginHints.Any((string hint) => hint == trimmedLoginHint))
			{
				_loginHints.Add(trimmedLoginHint);
			}
			if (_loginHints.Count >= 10)
			{
				break;
			}
		}
		User item = new User
		{
			Other = ArmSoft.SME.Common.Resources.AnotherUser,
			UserVisible = "Collapsed",
			OtherVisible = "Visible"
		};
		_users.Add(item);
		LoginName.ItemsSource = _users;
		Activated += new SafeEventHandler<EventArgs>(CloudLoginWindowBase_Activated).Handler;
		Unloaded += new SafeEventHandler<RoutedEventArgs>(CloudLoginWindow_Unloaded).Handler;
		OkButton.Click += new SafeEventHandler<RoutedEventArgs>(OkButton_Click).Handler;
		CloseTitleButton.Click += new SafeEventHandler<RoutedEventArgs>(CloseButton_Click).Handler;
		LoginName.Spin += new SafeEventHandler<SpinEventArgs>(LoginName_Spin).Handler;
		LoginName.PopupOpening += new SafeEventHandler<RoutedEventArgs>(LoginName_Open).Handler;
		LoginName.PopupClosed += new SafeEventHandler<ClosePopupEventArgs>(LoginName_PopupClosed).Handler;
		CloudDBList.EditValueChanged += new SafeEventHandler<EditValueChangedEventArgs>(CloudDBList_EditValueChanged).Handler;
		ASFontHelper.SetFontSettings(this);
	}

	private void ReloadCaptions()
	{
		TBWorkBegin.Text = ArmSoft.SME.Common.Resources.Login;
		if (ConfigHelper.ASAdminLogin)
		{
			TBWorkBegin.Text += "(AS admin)";
		}
		TBUserName.Text = ArmSoft.SME.Common.Resources.UserName;
		SaveUser.Content = ArmSoft.SME.Common.Resources.SaveUser;
		TBDatabase.Text = ArmSoft.SME.Common.Resources.Database;
		TBAvailableKeys.Text = ArmSoft.SME.Common.Resources.AvailableKeys;
		OkButton.Content = ArmSoft.SME.Common.Resources.Ok;
		CancelButton.Content = ArmSoft.SME.Common.Resources.Cancel;
		foreach (User user in _users)
		{
			if (!string.IsNullOrWhiteSpace(user.Other))
			{
				user.Other = ArmSoft.SME.Common.Resources.AnotherUser;
			}
		}
		SetOfferInfo();
	}

	private void SetLanguageInRegistry(object value)
	{
		try
		{
			RegistryHelper.StoreToRegistry("Culture", value);
			ReloadCaptions();
			Settings.HostingEnvironment.ReloadControlCaptions();
			Localizer.Configure();
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	private void Language_SelectionChanged(object sender, EditValueChangedEventArgs e)
	{
		LanguageComboBoxItem languageComboBoxItem = (LanguageComboBoxItem)e.NewValue;
		SetLanguage(languageComboBoxItem.LanguageCode);
		SetLanguageInRegistry(languageComboBoxItem.LanguageCode);
	}

	private static void SetLanguage(string selectedItem)
	{
		switch (selectedItem)
		{
		case "hy-AM":
			LanguageService.Language = ArmSoft.SME.Common.Language.Armenian;
			break;
		case "en-US":
			LanguageService.Language = ArmSoft.SME.Common.Language.English;
			break;
		case "ru-RU":
			LanguageService.Language = ArmSoft.SME.Common.Language.Russian;
			break;
		}
	}

	private void CloseButton_Click(object sender, RoutedEventArgs e)
	{
		SetDialogResult(result: false);
	}

	private void SetDialogResult(bool result)
	{
		try
		{
			DialogResult = result;
		}
		catch (InvalidOperationException)
		{
		}
	}

	private bool IsModal()
	{
		object obj = typeof(Window).GetField("_showingAsDialog", BindingFlags.Instance | BindingFlags.NonPublic)?.GetValue(this);
		if (obj is bool)
		{
			return (bool)obj;
		}
		return false;
	}

	private async void CloudLoginWindowBase_Activated(object sender, EventArgs e)
	{
		if (!_firstActivated)
		{
			return;
		}
		_firstActivated = false;
		IAccount account = null;
		try
		{
			Mouse.OverrideCursor = Cursors.Wait;
			if (ADHelper.Account == null)
			{
				if (SaveUser.IsChecked.Value)
				{
					account = await ADHelper.GetAccountAsync(_lastADB2CLogin);
				}
			}
			else
			{
				account = ADHelper.Account;
			}
			SetOfferInfo();
			_authResult = null;
			if (account != null)
			{
				try
				{
					_authResult = await ADHelper.AcquireTokenSilentAsync(account);
				}
				catch (Exception)
				{
					_authResult = null;
				}
			}
			if (_authResult == null)
			{
				try
				{
					Mouse.OverrideCursor = null;
					_authResult = await ADHelper.AcquireTokenInteractiveAsync(null, _loginHint, new WindowInteropHelper(this).Handle);
				}
				catch (MsalClientException ex2)
				{
					if (ex2.ErrorCode != "authentication_canceled")
					{
						Settings.ErrorHandler(ex2);
						return;
					}
				}
				catch (MsalServiceException ex3)
				{
					if (ex3.ErrorCode != "access_denied")
					{
						Settings.ErrorHandler(ex3);
						return;
					}
				}
				catch (Exception ex4)
				{
					Settings.ErrorHandler(ex4);
					return;
				}
			}
			if (Mouse.OverrideCursor == null)
			{
				Mouse.OverrideCursor = Cursors.Wait;
			}
			CloudLoginDone();
		}
		catch (Exception ex5)
		{
			Settings.ErrorHandler(ex5);
		}
		Mouse.OverrideCursor = null;
	}

	private void SetOfferInfo()
	{
		OfferStack.Visibility = Visibility.Collapsed;
		if (Settings.SystemType == SystemTypes.Accountant || Settings.SystemType == SystemTypes.Trade || Settings.SystemType == SystemTypes.SalesManagement)
		{
			OfferStack.Visibility = Visibility.Visible;
			TBOffer.Inlines.Clear();
			Hyperlink hyperlink = new Hyperlink(new Run(ArmSoft.SME.Common.Resources.Open))
			{
				Tag = ArmSoft.SME.Common.Resources.Open
			};
			hyperlink.Click += new SafeEventHandler<RoutedEventArgs>(NavigateToOffer).Handler;
			if (Settings.SystemType == SystemTypes.Accountant)
			{
				TBOffer.Inlines.Add(new Run(ArmSoft.SME.Common.Resources.AgreeWithTermsAndConditionsAcc));
			}
			else if (Settings.SystemType == SystemTypes.Trade)
			{
				TBOffer.Inlines.Add(new Run(ArmSoft.SME.Common.Resources.AgreeWithTermsAndConditionsTrade));
			}
			else if (Settings.SystemType == SystemTypes.SalesManagement)
			{
				TBOffer.Inlines.Add(new Run(ArmSoft.SME.Common.Resources.AgreeWithTermsAndConditionsSalesManagement));
			}
			TBOffer.Inlines.Add(new Run(" "));
			TBOffer.Inlines.Add(hyperlink);
		}
	}

	private void CloudLoginDone()
	{
		if (_authResult != null)
		{
			if (ConfigHelper.ASAdminLogin)
			{
				ApiClient apiClient = new ApiClient(ConfigHelper.ASAdminLoginBaseURL, null, new HttpClient(), null);
				_dbList = apiClient.AdminCloud.GetAvailableDatabases(_authResult.AccessToken, (Settings.SystemType == SystemTypes.Trade) ? new short?((short)3) : ((short?)null));
				CloudDBList.DataView = new AvailableDatabases(_dbList);
				CloudDBList.Code = _dbList.FirstOrDefault((Database p) => p.Id == _lastCloudDBId)?.Name ?? "";
				LoginName.Text = _authResult.Account.Username;
				LoginName.DataContext = _authResult.Account.Username;
				return;
			}
			ADHelper.Account = _authResult.Account;
			using (RegistryKey registryKey = Registry.CurrentUser.CreateSubKey(Settings.RegistryKey))
			{
				if (registryKey != null)
				{
					registryKey.SetValue("LastADB2CLogin", ADHelper.GetObjectId(_authResult.Account));
					registryKey.SetValue("CloudSaveUser", SaveUser.IsChecked.Value ? "1" : "0");
				}
			}
			using CloudClient cloudClient = new CloudClient("", Settings.B2BCloudBaseAddress);
			cloudClient.UserToken = _authResult.AccessToken;
			UserLoginRequestV2 request = new UserLoginRequestV2
			{
				Localization = (short)Settings.Localization,
				ProcessId = Settings.ProcessId(),
				Workstation = Environment.MachineName,
				Application = (short)Settings.SystemType
			};
			_userLoginResponse = cloudClient.AS7X.UserLogin(request);
			if (_userLoginResponse == null)
			{
				return;
			}
			RefreshLoginHintsInRegistry();
			StoreOfflineModeInfoInRegistry();
			string text = (string.IsNullOrWhiteSpace(_userLoginResponse.UserDescription) ? _userLoginResponse.UserName : _userLoginResponse.UserDescription);
			using (RegistryKey registryKey2 = Registry.CurrentUser.CreateSubKey(Settings.RegistryKey + "\\CloudLogIns"))
			{
				if (registryKey2 != null)
				{
					string text2 = registryKey2.GetValue(_userLoginResponse.UserName, "").ToString();
					if (string.IsNullOrWhiteSpace(text2) || text2 != text)
					{
						registryKey2.SetValue(_userLoginResponse.UserName, text);
						if (text2 != text && !string.IsNullOrWhiteSpace(text2))
						{
							foreach (User user in _users)
							{
								if (user.Mail == _userLoginResponse.UserName)
								{
									user.Name = text;
								}
							}
						}
						else
						{
							_users.Insert(0, new User
							{
								Name = text,
								Mail = _userLoginResponse.UserName,
								UserVisible = "Visible",
								OtherVisible = "Collapsed"
							});
						}
					}
				}
			}
			if (!_loginHints.Contains(_userLoginResponse.UserName))
			{
				_loginHints.Insert(0, _userLoginResponse.UserName);
			}
			LoginName.Text = text;
			LoginName.DataContext = _userLoginResponse.UserName;
			if (_userLoginResponse.AvailableKeys == null || _userLoginResponse.AvailableKeys.Count == 0)
			{
				TBKeys.Text = "";
				TBAvailableKeys.Text = ArmSoft.SME.Common.Resources.KeyWasNotFound;
			}
			else
			{
				TBKeys.Inlines.Clear();
				int count = _userLoginResponse.AvailableKeys.Count;
				int num = Math.Min(10, count);
				int num2 = count - num;
				bool flag = true;
				foreach (KeyInfoExt item in _userLoginResponse.AvailableKeys.Take(num))
				{
					if (!flag)
					{
						TBKeys.Inlines.Add(new Run(" "));
					}
					flag = false;
					Hyperlink hyperlink = new Hyperlink(new Run(item.Serial))
					{
						Tag = item.Serial
					};
					hyperlink.Click += new SafeEventHandler<RoutedEventArgs>(SerialHyperlinkRequestNavigate).Handler;
					TBKeys.Inlines.Add(hyperlink);
				}
				TBKeys.IsEnabled = true;
				if (num2 > 0)
				{
					TBKeys.Inlines.Add(new Run(" "));
					Hyperlink hyperlink2 = new Hyperlink(new Run($"+{num2} {ArmSoft.SME.Common.Resources.Key.ToLower()}"))
					{
						Tag = "More",
						Foreground = (SolidColorBrush)new BrushConverter().ConvertFrom(Settings.IsDarkTheme ? "#d2dbd5" : "#444444"),
						FontWeight = FontWeights.SemiBold
					};
					hyperlink2.Click += new SafeEventHandler<RoutedEventArgs>(MoreKeysHyperlinkRequestNavigate).Handler;
					TBKeys.Inlines.Add(hyperlink2);
				}
			}
			_cloudDBSourceList = _userLoginResponse.AvailableDatabases;
			if (_cloudDBSourceList == null)
			{
				return;
			}
			_dbList = new List<Database>();
			string lastDBName = "";
			foreach (AvailableDatabase item2 in _cloudDBSourceList.Where((AvailableDatabase d) => d.AllowNewUI && d.PSMode == ConfigHelper.IsPublicSector))
			{
				_dbList.Add(new Database
				{
					Id = item2.Id,
					Name = item2.Name,
					Caption = item2.Caption,
					IsBlocked = item2.IsBlocked,
					DisableOldUI = item2.DisableOldUI
				});
				if (item2.Id == _lastCloudDBId)
				{
					lastDBName = item2.Name;
					_selectedDatabase = item2;
				}
			}
			if (string.IsNullOrWhiteSpace(lastDBName) && _dbList.Count > 0)
			{
				lastDBName = _dbList.OrderBy((Database x) => x.Name).First().Name;
			}
			CloudDBList.DataView = new AvailableDatabases(_dbList);
			if (_cloudDBSourceList.Any((AvailableDatabase p) => p.Name == lastDBName))
			{
				CloudDBList.Code = lastDBName;
			}
			return;
		}
		ADHelper.Account = null;
		_userLoginResponse = null;
		using (RegistryKey registryKey3 = Registry.CurrentUser.CreateSubKey(Settings.RegistryKey))
		{
			if (registryKey3 != null)
			{
				registryKey3.SetValue("LastADB2CLogin", "");
				registryKey3.SetValue("CloudSaveUser", SaveUser.IsChecked.Value ? "1" : "0");
			}
		}
		TBKeys.Text = "";
		CloudDBList.DataView = new AvailableDatabases(new List<Database>());
		LoginName.Text = "";
		LoginName.DataContext = "";
		CloudDBList.Code = "";
	}

	private void StoreOfflineModeInfoInRegistry()
	{
		IEnumerable<string> source = from x in _userLoginResponse.AvailableKeys
			where x.OfflineModeIsAvailable
			select x.Serial;
		if (source.Any())
		{
			RegistryHelper.StoreStringArrayToRegistry("OfflineModeAvailableKeys", source.ToArray());
		}
		else
		{
			RegistryHelper.StoreToRegistry("OfflineModeAvailableKeys", "");
		}
	}

	private void CloudDBList_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		if (e.NewValue != null && !ConfigHelper.ASAdminLogin)
		{
			_selectedDatabase = _cloudDBSourceList?.FirstOrDefault((AvailableDatabase p) => p.Name == (string)e.NewValue);
		}
	}

	private void SerialHyperlinkRequestNavigate(object sender, RoutedEventArgs e)
	{
		if (_authResult != null)
		{
			string serial = ((Hyperlink)sender).Tag.ToString();
			new OnlineLicensesWindow(_userLoginResponse.AvailableKeys.FirstOrDefault((KeyInfoExt key) => key.Serial == serial), _authResult.AccessToken).ShowDialog();
		}
	}

	private void MoreKeysHyperlinkRequestNavigate(object sender, RoutedEventArgs e)
	{
		if (_authResult != null)
		{
			MoreLicenseKeysWindow moreLicenseKeysWindow = new MoreLicenseKeysWindow(_userLoginResponse.AvailableKeys, _authResult.AccessToken);
			moreLicenseKeysWindow.Owner = Settings.HostingEnvironment;
			moreLicenseKeysWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
			moreLicenseKeysWindow.ShowDialog();
		}
	}

	private void NavigateToOffer(object sender, RoutedEventArgs e)
	{
		Process.Start(new ProcessStartInfo
		{
			FileName = "https://www.armsoft.am/data/oferta.pdf",
			UseShellExecute = true
		});
	}

	private void RefreshLoginHintsInRegistry()
	{
		using RegistryKey registryKey = Registry.CurrentUser.CreateSubKey(Settings.RegistryKey);
		if (registryKey != null)
		{
			registryKey.SetValue("CloudLastLogin", _userLoginResponse.UserName);
			string text = (string)registryKey.GetValue("CloudLastLogins", "");
			List<string> list = (from hint in text.Split(',')
				select hint.Trim() into hint
				where !string.IsNullOrEmpty(hint) && hint != _userLoginResponse.UserName
				select hint).Distinct().ToList();
			list.Insert(0, _userLoginResponse.UserName);
			if (list.Count > 10)
			{
				list = list.Take(10).ToList();
			}
			text = string.Join(",", list);
			registryKey.SetValue("CloudLastLogins", text);
		}
	}

	private void LoginName_Spin(object sender, SpinEventArgs e)
	{
		e.Handled = true;
	}

	private void LoginName_Open(object sender, RoutedEventArgs e)
	{
		BindingList<User> bindingList = (BindingList<User>)LoginName.ItemsSource;
		if (bindingList == null)
		{
			return;
		}
		foreach (User item in bindingList)
		{
			ImageSource userImage = ImageService.GetUserImage(item.Name, "", 32);
			item.Image = userImage;
		}
	}

	private async void LoginName_PopupClosed(object sender, ClosePopupEventArgs e)
	{
		if (e.CloseMode == PopupCloseMode.Cancel || (_userLoginResponse != null && LoginName.Text == _userLoginResponse.UserName))
		{
			return;
		}
		if (LoginName.SelectedIndex == _loginHints.Count)
		{
			_loginHint = "";
		}
		else
		{
			User user = (User)LoginName.EditValue;
			_loginHint = ((user.Other == ArmSoft.SME.Common.Resources.AnotherUser) ? "" : user.Mail.Trim());
		}
		try
		{
			await ADHelper.ClearCacheAsync();
			_authResult = null;
			CloudLoginDone();
			try
			{
				_authResult = await ADHelper.AcquireTokenInteractiveAsync(null, _loginHint, new WindowInteropHelper(this).Handle);
			}
			catch (MsalClientException ex)
			{
				if (ex.ErrorCode != "authentication_canceled")
				{
					Settings.ErrorHandler(ex);
					return;
				}
			}
			catch (MsalServiceException ex2)
			{
				if (ex2.ErrorCode != "access_denied")
				{
					Settings.ErrorHandler(ex2);
					return;
				}
			}
			catch (Exception ex3)
			{
				Settings.ErrorHandler(ex3);
				return;
			}
			CloudLoginDone();
		}
		catch (Exception ex4)
		{
			Settings.ErrorHandler(ex4);
		}
	}

	private void OkButton_Click(object sender, RoutedEventArgs e)
	{
		if (ConfigHelper.ASAdminLogin)
		{
			OkButton_ClickForASAdminUser(sender, e);
		}
		else
		{
			OkButton_ClickForUser(sender, e);
		}
	}

	private void OkButton_ClickForUser(object sender, RoutedEventArgs e)
	{
		try
		{
			if (_authResult == null || _userLoginResponse == null)
			{
				return;
			}
			Settings.LoginService.Logout();
			using CloudClient cloudClient = new CloudClient("", Settings.B2BCloudBaseAddress);
			cloudClient.UserToken = _authResult.AccessToken;
			int sessionID = 0;
			Settings.ProcessIdToSessionId(Settings.ProcessId(), ref sessionID);
			int? num;
			if ((_userLoginResponse.AvailableKeys == null) | (_userLoginResponse.AvailableKeys.Count == 0))
			{
				if (MessageBox.Show(ArmSoft.SME.Common.Resources.NoAvailableKeys + "\r\n" + ArmSoft.SME.Common.Resources.WantToWorkWithDemoVersion, Settings.ProgramName, MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
				{
					return;
				}
				_selectedDatabase = cloudClient.AS7X.GrantDemoAccess(new DemoAccessRequest
				{
					System = ConvertSystemTypeToCloud()
				});
				num = _selectedDatabase.Id;
			}
			else
			{
				string database = "";
				if (!CloudDBList.DoValidate())
				{
					return;
				}
				database = CloudDBList.Code;
				if (string.IsNullOrWhiteSpace(database))
				{
					MessageBox.Show(ArmSoft.SME.Common.Resources.NoAvailableDBForUser);
					return;
				}
				num = _dbList?.FirstOrDefault((Database p) => p.Name == database)?.Id;
				if (!num.HasValue)
				{
					return;
				}
			}
			SelectedDBInfo selectedDBInfo = null;
			Guid licenseId = default;
			Mouse.OverrideCursor = Cursors.Wait;
			selectedDBInfo = cloudClient.AS7X.GetSelectedDatabaseInfo(num.Value);
			if (selectedDBInfo == null)
			{
				Mouse.OverrideCursor = null;
				return;
			}
			string message = "";
			int status = 0;
			if (!selectedDBInfo.Key.Disabled)
			{
				if (!cloudClient.AS7X.GetOnlineLicense(selectedDBInfo.Key.Serial, selectedDBInfo.DBInfo.ServerName, selectedDBInfo.DBInfo.Database, Environment.MachineName + "/" + Settings.GetHostName(), Settings.AppVersionString, sessionID, Settings.ProcessId(), ref licenseId, ref message, ref status))
				{
					if (status == 2 && Settings.SystemType == SystemTypes.Trade && cloudClient.AS7X.TryReleaseMyOldLicense(new LicenseReleaseRequest
					{
						Database = selectedDBInfo.DBInfo.Database,
						Key = selectedDBInfo.Key.Serial,
						WorkStaition = Environment.MachineName + "/" + Settings.GetHostName()
					}) && cloudClient.AS7X.GetOnlineLicense(selectedDBInfo.Key.Serial, selectedDBInfo.DBInfo.ServerName, selectedDBInfo.DBInfo.Database, Environment.MachineName + "/" + Settings.GetHostName(), Settings.AppVersionString, sessionID, Settings.ProcessId(), ref licenseId, ref message, ref status))
					{
						Log.Warning("TryReleaseMyOldLicense -> GetOnlineLicense");
					}
					Mouse.OverrideCursor = null;
					if (status != 0)
					{
						if (status != 2 || (Settings.SystemType != SystemTypes.Accountant && Settings.SystemType != SystemTypes.Trade) || selectedDBInfo.Key.WSLimit != 0)
						{
							throw new ASException(message);
						}
						licenseId = Guid.Empty;
						Mouse.OverrideCursor = Cursors.Wait;
					}
				}
			}
			else
			{
				Mouse.OverrideCursor = null;
				if (Settings.SystemType != SystemTypes.Accountant && Settings.SystemType != SystemTypes.Trade)
				{
					throw new ASException(message);
				}
				if (MessageBox.Show(string.Format(ArmSoft.SME.Common.Resources.KeyIsDisabled, selectedDBInfo.Key.Serial.ToString()), "", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.Yes) != MessageBoxResult.Yes)
				{
					return;
				}
				licenseId = Guid.Empty;
				Mouse.OverrideCursor = Cursors.Wait;
			}
			ADHelper.ADB2CToken = _authResult.AccessToken;
			ApplicationServiceConfig.ClearToken();
			string text = ConfigHelper.BaseUrl;
			if (string.IsNullOrWhiteSpace(text))
			{
				text = _selectedDatabase.Service;
			}
			Settings.LoginService.SetLoginInfo(selectedDBInfo, _userLoginResponse, licenseId, text, _selectedDatabase.DBService);
			Settings.OfflineModeIsAvailable = selectedDBInfo.Key.OfflineModeIsAvailable;
			int num2 = 0;
			if (Settings.DocAutoSaveIsAvailable)
			{
				ILocalDBLoginInfo requiredService = Settings.ServiceProvider.GetRequiredService<ILocalDBLoginInfo>();
				requiredService.Database = Settings.LoginService.Database;
				if (requiredService.ExistsDatabase() && Settings.OfflineModeIsAvailable)
				{
					num2 = requiredService.CloudUserId;
				}
				requiredService.CreateDatabaseIfNotExist();
				requiredService.TryUpdateDatabase();
			}
			SQLiteStorageService requiredService2 = Settings.ServiceProvider.GetRequiredService<SQLiteStorageService>();
			requiredService2.StoreInUserBasedLocalStorage = true;
			if (Settings.LoginService.CloudUserId != num2)
			{
				requiredService2.DeleteCachesTables();
			}
			requiredService2.CreateTablesIfNotExists();
			SetLanguage(((LanguageComboBoxItem)language.SelectedItem).LanguageCode);
			AddDataToRegistry(selectedDBInfo, ADHelper.GetObjectId(_authResult.Account), SaveUser.IsChecked.Value ? "1" : "0", _userLoginResponse?.UserName ?? _authResult.Account.Username);
			Settings.IsOffline = false;
			Settings.CreateApiClient();
			Mouse.OverrideCursor = null;
			SetDialogResult(result: true);
		}
		catch (Exception ex)
		{
			if (Mouse.OverrideCursor != null)
			{
				Mouse.OverrideCursor = null;
			}
			Settings.ErrorHandler(ex);
		}
	}

	private void OkButton_ClickForASAdminUser(object sender, RoutedEventArgs e)
	{
		Settings.LoginService.Logout();
		try
		{
			int sessionID = 0;
			Settings.ProcessIdToSessionId(Settings.ProcessId(), ref sessionID);
			string database = "";
			if (!CloudDBList.DoValidate())
			{
				return;
			}
			database = CloudDBList.Code;
			if (string.IsNullOrWhiteSpace(database))
			{
				return;
			}
			int? num = _dbList?.FirstOrDefault((Database p) => p.Name == database)?.Id;
			if (!num.HasValue)
			{
				return;
			}
			Mouse.OverrideCursor = Cursors.Wait;
			DatabaseInfo databaseInfo = new ApiClient(ConfigHelper.ASAdminLoginBaseURL, null, new HttpClient(), null).AdminCloud.GetDatabaseInfo(_authResult.AccessToken, num.Value);
			if (databaseInfo == null)
			{
				Mouse.OverrideCursor = null;
				return;
			}
			ADHelper.ASAdminB2CToken = _authResult.AccessToken;
			ApplicationServiceConfig.ClearToken();
			string text = ConfigHelper.BaseUrl;
			if (string.IsNullOrWhiteSpace(text))
			{
				text = databaseInfo.ServiceAddress;
			}
			Settings.LoginService.SetLoginInfoForASAdmin(databaseInfo, text, databaseInfo.DbServiceAddress);
			if (Settings.DocAutoSaveIsAvailable)
			{
				ILocalDBLoginInfo requiredService = Settings.ServiceProvider.GetRequiredService<ILocalDBLoginInfo>();
				requiredService.Database = Settings.LoginService.Database;
				if (requiredService.ExistsDatabase() && Settings.OfflineModeIsAvailable)
				{
					_ = requiredService.CloudUserId;
				}
				requiredService.CreateDatabaseIfNotExist();
				requiredService.TryUpdateDatabase();
			}
			SQLiteStorageService requiredService2 = Settings.ServiceProvider.GetRequiredService<SQLiteStorageService>();
			requiredService2.StoreInUserBasedLocalStorage = true;
			requiredService2.DeleteCachesTables();
			requiredService2.CreateTablesIfNotExists();
			SetLanguage(((LanguageComboBoxItem)language.SelectedItem).LanguageCode);
			Settings.IsOffline = false;
			Settings.CreateApiClient();
			AddDataForASAdminUser(databaseInfo, _authResult.Account.Username, ADHelper.GetObjectId(_authResult.Account), SaveUser.IsChecked.Value ? "1" : "0");
			Settings.LoginService.ASAdminUserName = _authResult.Account.Username;
			CloudDBUserLogCreationRequest request = new CloudDBUserLogCreationRequest
			{
				AdToken = ADHelper.ASAdminB2CToken,
				ProcessId = Settings.ProcessId(),
				Station = Environment.MachineName,
				DatabaseId = Settings.LoginService.DatabaseId,
				Key = Settings.LoginService.Key,
				UserId = Settings.LoginService.UserId,
				ASUser = Settings.LoginService.ASAdminUserName,
				Action = CloudDBUserLogAction.Login
			};
			Settings.CreateApiClient().AdminCloud.AddCloudDatabaseUserLog(request);
			Mouse.OverrideCursor = null;
			SetDialogResult(result: true);
		}
		catch (Exception ex)
		{
			if (Mouse.OverrideCursor != null)
			{
				Mouse.OverrideCursor = null;
			}
			Settings.ErrorHandler(ex);
		}
	}

	private static void AddDataToRegistry(SelectedDBInfo selectedDatabaseInfo, string objectId, string saveUserIsChecked, string userName)
	{
		new BackgroundTaskRunner().RunInBackground(() =>
		{
			using RegistryKey registryKey = Registry.CurrentUser.CreateSubKey(Settings.RegistryKey);
			if (registryKey != null)
			{
				registryKey.SetValue("LastADB2CLogin", objectId);
				registryKey.SetValue("CloudSaveUser", saveUserIsChecked);
				registryKey.SetValue("DBMode", "1");
				if (ConfigHelper.IsPublicSector)
				{
					registryKey.SetValue("CloudLastPSDBId", selectedDatabaseInfo.DBInfo.DBId);
					registryKey.SetValue("CloudLastPSDB", selectedDatabaseInfo.DBInfo.Database);
				}
				else
				{
					registryKey.SetValue("CloudLastDBId", selectedDatabaseInfo.DBInfo.DBId);
					registryKey.SetValue("CloudLastDB", selectedDatabaseInfo.DBInfo.Database);
				}
				registryKey.SetValue("CloudLastLogin", userName);
				registryKey.SetValue("CloudServerName", selectedDatabaseInfo.DBInfo.ServerName);
				registryKey.SetValue("ConfigFilePath", AppDomain.CurrentDomain.BaseDirectory);
				registryKey.SetValue("LastKeySNRegKey", selectedDatabaseInfo.Key.Serial);
			}
		});
	}

	private static void AddDataForASAdminUser(DatabaseInfo selectedDatabaseInfo, string userName, string objectId, string saveUserIsChecked)
	{
		new BackgroundTaskRunner().RunInBackground(() =>
		{
			using RegistryKey registryKey = Registry.CurrentUser.CreateSubKey(Settings.RegistryKey);
			if (registryKey != null)
			{
				registryKey.SetValue("CloudLastDBIdASAdmin", selectedDatabaseInfo.DbId);
				registryKey.SetValue("CloudLastDBASAdmin", selectedDatabaseInfo.DbName);
				registryKey.SetValue("ConfigFilePath", AppDomain.CurrentDomain.BaseDirectory);
				registryKey.SetValue("CloudLastASAdminLogin", userName);
				registryKey.SetValue("LastADB2CLoginASAdmin", objectId);
				registryKey.SetValue("CloudSaveUserASAdmin", saveUserIsChecked);
			}
		});
	}

	private void CloudLoginWindow_Unloaded(object sender, RoutedEventArgs e)
	{
		try
		{
			using RegistryKey registryKey = Registry.CurrentUser.CreateSubKey(Settings.RegistryKey);
			registryKey?.SetValue("CloudSaveUser", SaveUser.IsChecked.Value ? "1" : "0");
		}
		catch (Exception)
		{
		}
	}

	private void RemoveUser_Button_Click(object sender, RoutedEventArgs e)
	{
		if (!(sender is Button button))
		{
			return;
		}
		string text = button.DataContext.ToString();
		using (RegistryKey registryKey = Registry.CurrentUser.OpenSubKey(Settings.RegistryKey + "\\CloudLogIns", writable: true))
		{
			if (registryKey != null)
			{
				try
				{
					registryKey.DeleteValue(text);
				}
				catch
				{
				}
			}
			if (LoginName.DataContext != null && text == LoginName.DataContext.ToString())
			{
				ADHelper.Account = null;
				_userLoginResponse = null;
				using (RegistryKey registryKey2 = Registry.CurrentUser.CreateSubKey(Settings.RegistryKey))
				{
					if (registryKey2 != null)
					{
						if (ConfigHelper.ASAdminLogin)
						{
							registryKey2.SetValue("LastADB2CLoginASAdmin", "");
							registryKey2.SetValue("CloudLastASAdminLogin", "");
							registryKey2.SetValue("CloudSaveUserASAdmin", SaveUser.IsChecked.Value ? "1" : "0");
						}
						else
						{
							registryKey2.SetValue("LastADB2CLogin", "");
							registryKey2.SetValue("CloudLastLogin", "");
							registryKey2.SetValue("CloudSaveUser", SaveUser.IsChecked.Value ? "1" : "0");
						}
					}
				}
				TBKeys.Text = "";
				CloudDBList.DataView = new AvailableDatabases(new List<Database>());
				LoginName.Text = "";
				LoginName.DataContext = "";
				CloudDBList.Code = "";
			}
		}
		foreach (User user in _users)
		{
			if (user.Mail == text)
			{
				_users.Remove(user);
				break;
			}
		}
	}

	private static SystemType ConvertSystemTypeToCloud()
	{
		return Settings.SystemType switch
		{
			SystemTypes.Trade => SystemType.Trade, 
			SystemTypes.Accountant => SystemType.Accountant, 
			SystemTypes.SalesManagement => SystemType.SalesManagement, 
			SystemTypes.BookKeeper => SystemType.BookKeeper, 
			_ => throw new ArgumentException("Invalid system type"), 
		};
	}

	[DebuggerNonUserCode]
	[GeneratedCode("PresentationBuildTasks", "10.0.9.0")]
	internal Delegate _CreateDelegate(Type delegateType, string handler)
	{
		return Delegate.CreateDelegate(delegateType, this, handler);
	}
}
