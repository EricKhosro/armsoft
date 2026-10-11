using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Windows.Forms;
using ArmSoft.Cloud.Client;
using ArmSoft.Cloud.Client.AS7X;
using ArmSoft.SME.Core.AddIn.Client.Models;
using Microsoft.Identity.Client;
using Microsoft.Win32;

namespace ArmSoft.SME.Core.AddIn;

public class CloudLoginForm : Form
{
	private AuthenticationResult _authResult;

	private UserLoginResponseV2 _userLoginResponse;

	private List<ArmSoft.SME.Core.AddIn.Client.Models.AvailableDatabase> _dbSourceList;

	private List<Database> _dbList;

	private bool _firstActivated;

	private const string _cloudSaveUserRegKey = "CloudSaveUser";

	private const string _cloudLastLoginRegKey = "CloudLastLogin";

	private const string _cloudLastDBIdRegKey = "CloudLastDBId";

	private const string _cloudLastDBRegKey = "CloudLastDB";

	private const string _lastADB2CLoginRegKey = "LastADB2CLogin";

	private const string _cloudLastLoginASAdminRegKey = "CloudLastASAdminLogin";

	private const string _lastADB2CLoginASAdminRegKey = "LastADB2CLoginASAdmin";

	private const string _cloudSaveUserASAdminRegKey = "CloudSaveUserASAdmin";

	public const string _cloudLastDBIdASAdminRegKey = "CloudLastDBIdASAdmin";

	public const string _cloudLastDBASAdminRegKey = "CloudLastDBASAdmin";

	private readonly string _lastADB2CLogin;

	private readonly string _loginHint;

	private readonly int _lastCloudDBId;

	private IContainer components;

	internal Label LblLogin;

	internal TextBox CloudLoginName;

	internal Button ButtonCloudLoginWnd;

	internal CheckBox SaveUser;

	internal Label LblDatabase;

	internal ComboBox CloudDBList;

	internal Button ButtonOk;

	internal Button ButtonCancel;

	public CloudLoginForm()
	{
		InitializeComponent();
		_firstActivated = true;
		_authResult = null;
		_userLoginResponse = null;
		Text = Resources.Login;
		if (Settings.ASAdminLogin)
		{
			Text += "(AS admin)";
		}
		SaveUser.Text = Resources.SaveUser;
		ButtonOk.Text = Resources.Ok;
		ButtonCancel.Text = Resources.Cancel;
		LblLogin.Text = Resources.User;
		LblDatabase.Text = Resources.SelectDatabase;
		ButtonCloudLoginWnd.Text = Resources.b2bLoginButton;
		ButtonCloudLoginWnd.Enabled = true;
		using (RegistryKey registryKey = Registry.CurrentUser.OpenSubKey(Settings.RegistryKey))
		{
			if (registryKey != null)
			{
				if (Settings.ASAdminLogin)
				{
					SaveUser.Checked = (string)registryKey.GetValue("CloudSaveUserASAdmin", "1") == "1";
					_loginHint = (string)registryKey.GetValue("CloudLastASAdminLogin", "");
					_lastADB2CLogin = (string)registryKey.GetValue("LastADB2CLoginASAdmin", "");
					_lastCloudDBId = (int)registryKey.GetValue("CloudLastDBIdASAdmin", 0);
				}
				else
				{
					SaveUser.Checked = (string)registryKey.GetValue("CloudSaveUser", "1") == "1";
					_lastADB2CLogin = (string)registryKey.GetValue("LastADB2CLogin", "");
					_loginHint = (string)registryKey.GetValue("CloudLastLogin", "");
					_lastCloudDBId = (int)registryKey.GetValue("CloudLastDBId", 0);
				}
			}
			else
			{
				SaveUser.Checked = true;
			}
		}
		ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
		Activated += CloudLoginForm_Activated;
		KeyDown += CloudLoginForm_KeyDown;
		FormClosing += CloudLoginForm_FormClosing;
		ButtonOk.Click += ButtonOk_Click;
		ButtonCloudLoginWnd.Click += ButtonCloudLoginWnd_Click;
	}

	private async void CloudLoginForm_Activated(object sender, EventArgs e)
	{
		if (!_firstActivated)
		{
			return;
		}
		_firstActivated = false;
		IAccount account = null;
		try
		{
			Cursor.Current = Cursors.WaitCursor;
			if (ADHelper.Account == null)
			{
				if (SaveUser.Checked)
				{
					account = await ADHelper.GetAccountAsync(_lastADB2CLogin);
				}
			}
			else
			{
				account = ADHelper.Account;
			}
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
					Cursor.Current = Cursors.Default;
					_authResult = await ADHelper.AcquireTokenInteractiveAsync(null, _loginHint, Handle);
				}
				catch (MsalClientException ex2)
				{
					if (ex2.ErrorCode != "authentication_canceled")
					{
						MessageBox.Show(ex2.Message);
						return;
					}
				}
				catch (MsalServiceException ex3)
				{
					if (ex3.ErrorCode != "access_denied")
					{
						MessageBox.Show(ex3.Message);
					}
				}
				catch (Exception ex4)
				{
					MessageBox.Show(ex4.Message);
					return;
				}
			}
			if (Cursor.Current == Cursors.Default)
			{
				Cursor.Current = Cursors.WaitCursor;
			}
			CloudLoginDone();
		}
		catch (Exception ex5)
		{
			Cursor.Current = Cursors.Default;
			MessageBox.Show(ex5.Message);
		}
		Cursor.Current = Cursors.Default;
	}

	private void CloudLoginDone()
	{
		try
		{
			if (_authResult != null)
			{
				if (Settings.ASAdminLogin)
				{
					ApiClient apiClient = new ApiClient(Settings.ASAdminLoginBaseURL, null, new HttpClient());
					_dbList = apiClient.AdminCloud.GetAvailableDatabases(_authResult.AccessToken, (Settings.SystemType == Settings.SystemTypes.Trade) ? new short?(3) : ((short?)null));
					CloudDBList.DataSource = _dbList.Select((Database p) => p.Name).ToList();
					CloudDBList.Text = _dbList.FirstOrDefault((Database p) => p.Id == _lastCloudDBId)?.Name ?? "";
					CloudLoginName.Text = _authResult.Account.Username;
					return;
				}
				using (RegistryKey registryKey = Registry.CurrentUser.CreateSubKey(Settings.RegistryKey))
				{
					registryKey.SetValue("LastADB2CLogin", ADHelper.GetObjectId(_authResult.Account));
					registryKey.SetValue("CloudSaveUser", SaveUser.Checked ? "1" : "0");
				}
				ADHelper.Account = _authResult.Account;
				using (CloudClient cloudClient = new CloudClient("", Settings.B2BCloudBaseAddress))
				{
					cloudClient.UserToken = _authResult.AccessToken;
					UserLoginRequestV2 request = new UserLoginRequestV2
					{
						Localization = (short)Settings.Localization,
						ProcessId = Settings.ProcessId(),
						Workstation = Environment.MachineName,
						Application = (short)Settings.SystemType
					};
					_userLoginResponse = cloudClient.AS7X.UserLogin(request);
					CloudLoginName.Text = _userLoginResponse.UserName;
				}
				ReadCloudDB();
			}
			else
			{
				ADHelper.Account = null;
				_userLoginResponse = null;
				using (RegistryKey registryKey2 = Registry.CurrentUser.CreateSubKey(Settings.RegistryKey))
				{
					registryKey2.SetValue("LastADB2CLogin", "");
					registryKey2.SetValue("CloudSaveUser", SaveUser.Checked ? "1" : "0");
				}
				CloudDBList.DataSource = new List<ArmSoft.Cloud.Client.AS7X.AvailableDatabase>();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ButtonOk_Click(object sender, EventArgs e)
	{
		if (string.IsNullOrWhiteSpace(CloudDBList.Text))
		{
			MessageBox.Show(Resources.DatabaseNotSelected);
		}
		else
		{
			SelectCloudDB();
		}
		Close();
	}

	private void CloudLoginForm_KeyDown(object sender, KeyEventArgs e)
	{
		if (e.Control & (e.KeyCode == Keys.Return))
		{
			SelectCloudDB();
			e.Handled = true;
		}
	}

	private void SelectCloudDB()
	{
		if (Settings.ASAdminLogin)
		{
			SelectCloudDBForASAdminUser();
		}
		else
		{
			SelectCloudDBForUser();
		}
	}

	private void SelectCloudDBForUser()
	{
		try
		{
			if (_authResult == null || _userLoginResponse == null)
			{
				return;
			}
			ArmSoft.SME.Core.AddIn.Client.Models.AvailableDatabase availableDatabase = _dbSourceList[CloudDBList.SelectedIndex];
			using CloudClient cloudClient = new CloudClient("", Settings.B2BCloudBaseAddress);
			cloudClient.UserToken = _authResult.AccessToken;
			SelectedDBInfo selectedDatabaseInfo = cloudClient.AS7X.GetSelectedDatabaseInfo(availableDatabase.DbId);
			if (selectedDatabaseInfo != null)
			{
				DialogResult = DialogResult.OK;
				Settings.ServiceAddress = availableDatabase.ServiceAddress;
				Settings.IdToken = _authResult.AccessToken;
				Settings.UserName = _userLoginResponse.UserName;
				Settings.Database = availableDatabase.Name;
				Settings.DatabaseId = availableDatabase.DbId;
				Settings.DemoMode = selectedDatabaseInfo.Key.Objective == KeyObjective.Demonstration;
				Settings.IsLoggedIn = true;
				using RegistryKey registryKey = Registry.CurrentUser.CreateSubKey(Settings.RegistryKey);
				registryKey.SetValue("LastADB2CLogin", ADHelper.GetObjectId(_authResult.Account));
				registryKey.SetValue("CloudSaveUser", SaveUser.Checked ? "1" : "0");
				registryKey.SetValue("CloudLastDBId", selectedDatabaseInfo.DBInfo.DBId);
				registryKey.SetValue("CloudLastDB", selectedDatabaseInfo.DBInfo.Database);
				registryKey.SetValue("CloudLastLogin", _userLoginResponse.UserName);
				return;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void SelectCloudDBForASAdminUser()
	{
		try
		{
			if (_authResult == null)
			{
				return;
			}
			string database = "";
			database = CloudDBList.Text;
			if (string.IsNullOrWhiteSpace(database))
			{
				return;
			}
			int? num = _dbList?.FirstOrDefault((Database p) => p.Name == database)?.Id;
			if (!num.HasValue)
			{
				return;
			}
			DatabaseInfo databaseInfo = new ApiClient(Settings.ASAdminLoginBaseURL, null, new HttpClient()).AdminCloud.GetDatabaseInfo(_authResult.AccessToken, num.Value);
			if (databaseInfo != null)
			{
				Settings.ASAdminB2CToken = _authResult.AccessToken;
				string serviceAddress = databaseInfo.ServiceAddress;
				DialogResult = DialogResult.OK;
				Settings.ServiceAddress = serviceAddress;
				Settings.Database = databaseInfo.DbName;
				Settings.DatabaseId = databaseInfo.DbId;
				Settings.IsLoggedIn = true;
				using (RegistryKey registryKey = Registry.CurrentUser.CreateSubKey(Settings.RegistryKey))
				{
					registryKey.SetValue("LastADB2CLoginASAdmin", ADHelper.GetObjectId(_authResult.Account));
					registryKey.SetValue("CloudSaveUserASAdmin", SaveUser.Checked ? "1" : "0");
					registryKey.SetValue("CloudLastDBIdASAdmin", databaseInfo.DbId);
					registryKey.SetValue("CloudLastDBASAdmin", databaseInfo.DbName);
					registryKey.SetValue("CloudLastASAdminLogin", CloudLoginName.Text);
				}
				new ApiClient(Settings.ServiceAddress, Settings.Token, new HttpClient());
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private async void ButtonCloudLoginWnd_Click(object sender, EventArgs e)
	{
		try
		{
			_authResult = await ADHelper.AcquireTokenInteractiveAsync(null, _loginHint, Handle);
			CloudLoginDone();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ReadCloudDB()
	{
		CloudDBList.Visible = true;
		AddCloudDBInDBSourceLIst();
		CloudDBList.DataSource = _dbSourceList;
		CloudDBList.DisplayMember = "ComboView";
		for (int i = 0; i <= _dbSourceList.Count - 1; i++)
		{
			if (_lastCloudDBId == _dbSourceList[i].DbId)
			{
				CloudDBList.SelectedIndex = i;
			}
		}
	}

	private void AddCloudDBInDBSourceLIst()
	{
		_dbSourceList = new List<ArmSoft.SME.Core.AddIn.Client.Models.AvailableDatabase>();
		if (_userLoginResponse == null)
		{
			return;
		}
		foreach (ArmSoft.Cloud.Client.AS7X.AvailableDatabase item in _userLoginResponse.AvailableDatabases.Where((ArmSoft.Cloud.Client.AS7X.AvailableDatabase d) => d.AllowNewUI))
		{
			_dbSourceList.Add(new ArmSoft.SME.Core.AddIn.Client.Models.AvailableDatabase
			{
				Name = item.Name,
				Caption = item.Caption,
				DbId = item.Id,
				ServiceAddress = item.Service
			});
		}
	}

	private void CloudLoginForm_FormClosing(object sender, FormClosingEventArgs e)
	{
		try
		{
			using RegistryKey registryKey = Registry.CurrentUser.CreateSubKey(Settings.RegistryKey);
			registryKey.SetValue("CloudSaveUser", SaveUser.Checked ? "1" : "0");
		}
		catch (Exception)
		{
		}
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		base.Dispose(disposing);
	}

	private void InitializeComponent()
	{
		this.LblLogin = new System.Windows.Forms.Label();
		this.CloudLoginName = new System.Windows.Forms.TextBox();
		this.ButtonCloudLoginWnd = new System.Windows.Forms.Button();
		this.SaveUser = new System.Windows.Forms.CheckBox();
		this.LblDatabase = new System.Windows.Forms.Label();
		this.CloudDBList = new System.Windows.Forms.ComboBox();
		this.ButtonOk = new System.Windows.Forms.Button();
		this.ButtonCancel = new System.Windows.Forms.Button();
		base.SuspendLayout();
		this.LblLogin.AutoSize = true;
		this.LblLogin.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.LblLogin.Location = new System.Drawing.Point(12, 38);
		this.LblLogin.Name = "LblLogin";
		this.LblLogin.Size = new System.Drawing.Size(37, 16);
		this.LblLogin.TabIndex = 10;
		this.LblLogin.Text = "Login";
		this.CloudLoginName.Font = new System.Drawing.Font("Tahoma", 9.75f);
		this.CloudLoginName.Location = new System.Drawing.Point(128, 35);
		this.CloudLoginName.Name = "CloudLoginName";
		this.CloudLoginName.ReadOnly = true;
		this.CloudLoginName.Size = new System.Drawing.Size(190, 23);
		this.CloudLoginName.TabIndex = 11;
		this.ButtonCloudLoginWnd.Font = new System.Drawing.Font("Tahoma", 9.75f);
		this.ButtonCloudLoginWnd.Location = new System.Drawing.Point(324, 35);
		this.ButtonCloudLoginWnd.Name = "ButtonCloudLoginWnd";
		this.ButtonCloudLoginWnd.Size = new System.Drawing.Size(77, 25);
		this.ButtonCloudLoginWnd.TabIndex = 12;
		this.ButtonCloudLoginWnd.UseVisualStyleBackColor = true;
		this.SaveUser.AutoSize = true;
		this.SaveUser.Location = new System.Drawing.Point(15, 77);
		this.SaveUser.Name = "SaveUser";
		this.SaveUser.Size = new System.Drawing.Size(117, 17);
		this.SaveUser.TabIndex = 15;
		this.SaveUser.Text = "Save User Account";
		this.SaveUser.UseVisualStyleBackColor = true;
		this.LblDatabase.AutoSize = true;
		this.LblDatabase.Font = new System.Drawing.Font("Tahoma", 9.75f);
		this.LblDatabase.Location = new System.Drawing.Point(12, 125);
		this.LblDatabase.Name = "LblDatabase";
		this.LblDatabase.Size = new System.Drawing.Size(99, 16);
		this.LblDatabase.TabIndex = 18;
		this.LblDatabase.Text = "Select Database";
		this.CloudDBList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		this.CloudDBList.Font = new System.Drawing.Font("Tahoma", 9.75f);
		this.CloudDBList.FormattingEnabled = true;
		this.CloudDBList.Location = new System.Drawing.Point(128, 125);
		this.CloudDBList.MaxDropDownItems = 5;
		this.CloudDBList.Name = "CloudDBList";
		this.CloudDBList.Size = new System.Drawing.Size(234, 24);
		this.CloudDBList.TabIndex = 19;
		this.ButtonOk.Font = new System.Drawing.Font("Tahoma", 9.75f);
		this.ButtonOk.Location = new System.Drawing.Point(52, 168);
		this.ButtonOk.Name = "ButtonOk";
		this.ButtonOk.Size = new System.Drawing.Size(117, 27);
		this.ButtonOk.TabIndex = 20;
		this.ButtonOk.Text = "OK";
		this.ButtonOk.UseVisualStyleBackColor = true;
		this.ButtonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
		this.ButtonCancel.Font = new System.Drawing.Font("Tahoma", 9.75f);
		this.ButtonCancel.Location = new System.Drawing.Point(197, 170);
		this.ButtonCancel.Name = "ButtonCancel";
		this.ButtonCancel.Size = new System.Drawing.Size(114, 25);
		this.ButtonCancel.TabIndex = 21;
		this.ButtonCancel.Text = "Cancel";
		this.ButtonCancel.UseVisualStyleBackColor = true;
		base.AcceptButton = this.ButtonOk;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.CancelButton = this.ButtonCancel;
		base.ClientSize = new System.Drawing.Size(410, 220);
		base.Controls.Add(this.ButtonCancel);
		base.Controls.Add(this.ButtonOk);
		base.Controls.Add(this.CloudDBList);
		base.Controls.Add(this.LblDatabase);
		base.Controls.Add(this.SaveUser);
		base.Controls.Add(this.ButtonCloudLoginWnd);
		base.Controls.Add(this.CloudLoginName);
		base.Controls.Add(this.LblLogin);
		this.Font = new System.Drawing.Font("Tahoma", 8.25f);
		base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
		base.KeyPreview = true;
		base.MaximizeBox = false;
		base.MinimizeBox = false;
		base.Name = "CloudLoginForm";
		base.ShowIcon = false;
		base.ShowInTaskbar = false;
		base.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
		this.Text = "CloudLoginForm";
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
