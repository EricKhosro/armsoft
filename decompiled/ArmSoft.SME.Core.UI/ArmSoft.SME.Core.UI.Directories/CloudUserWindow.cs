using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Windows;
using ArmSoft.Cloud.Client;
using ArmSoft.Cloud.Client.AS7X;
using ArmSoft.SME.Client.Data.Reports;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Models.Cloud;
using ArmSoft.SME.Core.UI.Controls;
using ArmSoft.SME.Core.UI.Helpers;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.ComponentModel.__Internals;
using DevExpress.Utils;
using DevExpress.Xpf.Editors;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.LayoutControl;

namespace ArmSoft.SME.Core.UI.Directories;

[DirectoryWindow(236)]
public class CloudUserWindow : DialogWindow
{
	public class AvailableKeyRow : DocumentGridRow
	{
		[ObservableProperty]
		private bool _isAvailable;

		public string Serial { get; set; }

		public int WSLimit { get; set; }

		public int MobileLimit { get; set; }

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public bool IsAvailable
		{
			get
			{
				return _isAvailable;
			}
			set
			{
				if (!EqualityComparer<bool>.Default.Equals(_isAvailable, value))
				{
					OnPropertyChanging(__KnownINotifyPropertyChangingArgs.IsAvailable);
					_isAvailable = value;
					OnPropertyChanged(__KnownINotifyPropertyChangedArgs.IsAvailable);
				}
			}
		}
	}

	protected readonly UsersDataProvider.DataRow _userRow;

	private ArmSoft.SME.Common.Models.Cloud.CloudUserLoginInfo _cloudUserInfo;

	private CustomerInfo _customerInfo;

	protected PopupCalcEditExt _userId;

	private ButtonEditExt _email;

	private ButtonInfo _findButton;

	private TextEditExt _name;

	private TextEditExt _caption;

	private TextEditExt _emailText;

	private CheckEditExt _admin;

	private CheckEditExt _isOwner;

	protected PopupCalcEditExt _innerUserID;

	protected DocumentGridControl _keysGrid;

	protected DocumentGridColumn _mobileLimitColumn;

	private readonly ObservableCollection<AvailableKeyRow> _availableKeys = new ObservableCollection<AvailableKeyRow>();

	private DocumentGridColumn _isAvailableCol;

	public CloudUserWindow(UsersDataProvider.DataRow user, DialogWindowMode mode, Window owner = null)
	{
		Owner = owner;
		Mode = mode;
		_userRow = user;
		ConstructUI();
		LoadData();
		SetControlsReadOnly();
	}

	protected virtual void ConstructUI()
	{
		Caption = ArmSoft.SME.Common.Resources.UserName;
		using CloudClient cloudClient = Settings.CloudClient;
		if (!ConfigHelper.ASAdminLogin && cloudClient.AS7X.IsMeOwnerForCustomer(Settings.LoginService.CustomerId))
		{
			_customerInfo = cloudClient.AS7X.GetCustomerInfo(Settings.LoginService.CustomerId);
		}
		int num;
		if (Mode != DialogWindowMode.EditMode)
		{
			num = ((Mode == DialogWindowMode.ViewMode) ? 1 : 0);
			if (num == 0)
			{
				_email = AddButtonEditControl("Email", ArmSoft.SME.Common.Resources.Email, 50, isRequired: false, 0);
				_findButton = new ButtonInfo
				{
					GlyphKind = GlyphKind.Search
				};
				_findButton.Click += new SafeEventHandler<RoutedEventArgs>(FindButton_Clicked).Handler;
				_email.Buttons.Add(_findButton);
			}
		}
		else
		{
			num = 1;
		}
		_name = AddStringControl("Name", ArmSoft.SME.Common.Resources.LoginName, 50);
		_name.IsReadOnly = true;
		_caption = AddStringControl("NewCaption", ArmSoft.SME.Common.Resources.Description, 50);
		_caption.IsReadOnly = true;
		if (num != 0)
		{
			_emailText = AddStringControl("Email", ArmSoft.SME.Common.Resources.Email, 50);
			_emailText.IsReadOnly = true;
		}
		_userId = AddNumericControl("UserID", ArmSoft.SME.Common.Resources.Number, 7, 0, isRequired: false, allowNegativeNumbers: false, useCurrencyFormat: false);
		_userId.IsReadOnly = true;
		_innerUserID = AddNumericControl("InnerUserID", ArmSoft.SME.Common.Resources.InnerNumber, 7, 0, isRequired: false, allowNegativeNumbers: false, useCurrencyFormat: false);
		PopupCalcEditExt innerUserID = _innerUserID;
		int? num2;
		if (_userRow == null || !_userRow.LocalUserId.HasValue)
		{
			UsersDataProvider.DataRow userRow = _userRow;
			num2 = ((userRow != null) ? new int?(userRow.UserId) : new int?(0));
		}
		else
		{
			num2 = _userRow.LocalUserId.Value;
		}
		innerUserID.EditValue = num2;
		_admin = AddBooleanControl("Admin", ArmSoft.SME.Common.Resources.UserIsAdmin);
		_isOwner = AddBooleanControl("IsOwner", ArmSoft.SME.Common.Resources.IsDbAdmin);
		_isOwner.EditValueChanged += new SafeEventHandler<EditValueChangedEventArgs>(IsOwner_EditValueChanged).Handler;
		if (_customerInfo == null && !ConfigHelper.ASAdminLogin)
		{
			return;
		}
		LayoutGroupExt layoutGroupExt = AddHorizontalLayoutGroup();
		layoutGroupExt.View = LayoutGroupView.GroupBox;
		layoutGroupExt.Header = ArmSoft.SME.Common.Resources.AvailableKeys;
		_keysGrid = AddGridControl(layoutGroupExt, "AvailableKeys");
		_keysGrid.Width = SizeCalculator.Width(450.0);
		_keysGrid.Height = SizeCalculator.Height(120.0);
		_keysGrid.AllowAdd = false;
		_keysGrid.AllowDelete = false;
		_keysGrid.AllowDuplicate = false;
		_keysGrid.ShowButtons = false;
		if (ConfigHelper.ASAdminLogin)
		{
			_availableKeys.Add(new AvailableKeyRow
			{
				Serial = Settings.LoginService.Key,
				WSLimit = 0,
				MobileLimit = 0
			});
		}
		else
		{
			foreach (KeyInfo item2 in _customerInfo.Keys.Where((KeyInfo p) => (int)p.Application == (int)Settings.SystemType))
			{
				_availableKeys.Add(new AvailableKeyRow
				{
					Serial = item2.Serial,
					WSLimit = item2.WSLimit,
					MobileLimit = item2.MobileLimit
				});
			}
		}
		_keysGrid.ItemsSource = _availableKeys;
		_keysGrid.View.AllowMoveColumnToDropArea = false;
		_keysGrid.View.IsColumnMenuEnabled = false;
		_keysGrid.AddStringColumn("Serial", ArmSoft.SME.Common.Resources.Key, 8).AllowEditing = DefaultBoolean.False;
		_keysGrid.AddNumberColumn("WSLimit", ArmSoft.SME.Common.Resources.WorkstationsLimit, 20, 0).AllowEditing = DefaultBoolean.False;
		_mobileLimitColumn = _keysGrid.AddNumberColumn("MobileLimit", ArmSoft.SME.Common.Resources.MobileLimit, 20, 0);
		_mobileLimitColumn.AllowEditing = DefaultBoolean.False;
		_mobileLimitColumn.Visible = false;
		FormatCondition item = new FormatCondition
		{
			Expression = $"[{"Serial"}] = '{Settings.LoginService.Key}'",
			FieldName = "Serial",
			ApplyToRow = true,
			PredefinedFormatName = "BoldText"
		};
		_keysGrid.InnerView.FormatConditions.Add(item);
		_isAvailableCol = _keysGrid.AddBooleanColumn("IsAvailable", ArmSoft.SME.Common.Resources.IsAvailable);
	}

	private void IsOwner_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		if (_isOwner.IsChecked.Value)
		{
			_admin.IsChecked = true;
			_admin.IsReadOnly = true;
		}
		else
		{
			_admin.IsReadOnly = false;
		}
	}

	private void FindButton_Clicked(object sender, RoutedEventArgs e)
	{
		_email.DoValidate();
		EmailChanged();
	}

	private void EmailChanged()
	{
		SetCloudUserInfo(_email.Text.Trim());
		if (_cloudUserInfo == null)
		{
			_name.EditValue = string.Empty;
			_caption.EditValue = string.Empty;
			_userId.EditValue = 0;
		}
		else
		{
			_name.EditValue = _cloudUserInfo.Name;
			_caption.EditValue = _cloudUserInfo.Caption;
			_email.EditValue = _cloudUserInfo.Email;
			_userId.EditValue = _cloudUserInfo.UserId;
			_innerUserID.EditValue = _cloudUserInfo.UserId;
		}
	}

	private void SetCloudUserInfo(string email)
	{
		if (ConfigHelper.ASAdminLogin)
		{
			List<ArmSoft.SME.Common.Models.Cloud.CloudUserLoginInfo> cloudUsersInfo = Settings.CreateApiClient().AdminCloud.GetCloudUsersInfo(ADHelper.ASAdminB2CToken, email);
			if (cloudUsersInfo == null || cloudUsersInfo.Count == 0)
			{
				_cloudUserInfo = null;
			}
			else
			{
				_cloudUserInfo = cloudUsersInfo[0];
			}
			return;
		}
		using CloudClient cloudClient = Settings.CloudClient;
		cloudClient.Token = Settings.GetApplicationToken();
		List<ArmSoft.Cloud.Client.AS7X.CloudUserLoginInfo> cloudUsersInfo2 = cloudClient.AS7X.GetCloudUsersInfo(email);
		if (cloudUsersInfo2 == null || cloudUsersInfo2.Count == 0)
		{
			_cloudUserInfo = null;
		}
		else
		{
			_cloudUserInfo = ConvertAS7XCloudUserLoginInfoToInnerType(cloudUsersInfo2[0]);
		}
	}

	private static ArmSoft.SME.Common.Models.Cloud.CloudUserLoginInfo ConvertAS7XCloudUserLoginInfoToInnerType(ArmSoft.Cloud.Client.AS7X.CloudUserLoginInfo as7XLoginInfo)
	{
		return new ArmSoft.SME.Common.Models.Cloud.CloudUserLoginInfo
		{
			UserId = as7XLoginInfo.UserId,
			Name = as7XLoginInfo.Name,
			Caption = as7XLoginInfo.Caption,
			Email = as7XLoginInfo.Email,
			AvailableKeys = as7XLoginInfo.AvailableKeys
		};
	}

	protected virtual void LoadData()
	{
		if (Mode == DialogWindowMode.EditMode || Mode == DialogWindowMode.ViewMode)
		{
			SetCloudUserInfo(_userRow.Name);
			if (_cloudUserInfo == null)
			{
				_name.EditValue = string.Empty;
				_caption.EditValue = string.Empty;
				_emailText.EditValue = string.Empty;
			}
			else
			{
				_name.EditValue = _cloudUserInfo.Name;
				_caption.EditValue = _cloudUserInfo.Caption;
				_emailText.EditValue = _cloudUserInfo.Email;
				_userId.EditValue = _cloudUserInfo.UserId;
			}
			if (_availableKeys.Count > 0 && _cloudUserInfo.AvailableKeys != null)
			{
				foreach (string key in _cloudUserInfo.AvailableKeys)
				{
					AvailableKeyRow availableKeyRow = _availableKeys.FirstOrDefault((AvailableKeyRow p) => p.Serial == key);
					if (availableKeyRow != null)
					{
						availableKeyRow.IsAvailable = true;
					}
				}
			}
		}
		else
		{
			foreach (AvailableKeyRow availableKey in _availableKeys)
			{
				availableKey.IsAvailable = true;
			}
		}
		if (Mode == DialogWindowMode.EditMode || Mode == DialogWindowMode.ViewMode || Mode == DialogWindowMode.CreateCopyMode)
		{
			if (_userRow != null)
			{
				_admin.IsChecked = _userRow.Admin;
				_isOwner.IsChecked = _userRow.IsDbAdmin;
			}
		}
		else
		{
			_admin.IsChecked = true;
			_isOwner.IsChecked = false;
		}
	}

	protected virtual void SetControlsReadOnly()
	{
		bool flag = false;
		if (Mode != DialogWindowMode.NewMode)
		{
			flag = _userRow.UserId == Settings.LoginService.UserId;
		}
		CheckEditExt admin = _admin;
		if ((Mode == DialogWindowMode.EditMode) & flag)
		{
			goto IL_0068;
		}
		if (Mode == DialogWindowMode.EditMode)
		{
			UsersDataProvider.DataRow userRow = _userRow;
			if (userRow != null && userRow.IsDbAdmin)
			{
				goto IL_0068;
			}
		}
		int isReadOnly = ((!ConfigHelper.ASAdminLogin && _customerInfo == null) ? 1 : 0);
		goto IL_0069;
		IL_0068:
		isReadOnly = 1;
		goto IL_0069;
		IL_0069:
		admin.IsReadOnly = (byte)isReadOnly != 0;
		_isOwner.IsReadOnly = ((Mode == DialogWindowMode.EditMode) & flag) || (!ConfigHelper.ASAdminLogin && _customerInfo == null);
		if (_isAvailableCol != null)
		{
			if (((Mode == DialogWindowMode.EditMode && _userRow != null) & flag) || Mode == DialogWindowMode.ViewMode)
			{
				_isAvailableCol.AllowEditing = DefaultBoolean.False;
			}
			else
			{
				_isAvailableCol.AllowEditing = DefaultBoolean.True;
			}
		}
	}

	protected override void DialogValidation(DialogWindowValidationArgs args)
	{
		base.DialogValidation(args);
		if (Mode == DialogWindowMode.NewMode)
		{
			try
			{
				_email.DoValidate();
				if (string.IsNullOrWhiteSpace(_email.Text))
				{
					throw new ASException(ArmSoft.SME.Common.Resources.CloudUserEMailIsEmpty);
				}
				if (_cloudUserInfo == null || _email.Text != _cloudUserInfo.Email)
				{
					EmailChanged();
				}
				if (_cloudUserInfo == null)
				{
					throw new ASException(string.Format(ArmSoft.SME.Common.Resources.CloudUserWithEmailNotExists, _email.Text.Trim()));
				}
				bool flag;
				if (ConfigHelper.ASAdminLogin)
				{
					flag = Settings.CreateApiClient().AdminCloud.CheckCloudUserExistenceRequest(ADHelper.ASAdminB2CToken, Settings.LoginService.Key, _cloudUserInfo.UserId);
				}
				else
				{
					using CloudClient cloudClient = Settings.CloudClient;
					cloudClient.Token = Settings.GetApplicationToken();
					flag = cloudClient.AS7X.GetUser(Settings.LoginService.Key, _cloudUserInfo.UserId) != null;
				}
				if (flag)
				{
					throw new ASException(string.Format(ArmSoft.SME.Common.Resources.RowWithCodeAlreadyExists, _cloudUserInfo.Name.Trim(), ArmSoft.SME.Common.Resources.UserName));
				}
				Settings.CreateApiClient().UserRoutes.UpdateUserMapping(Convert.ToInt32(_userId.EditValue), Convert.ToInt32(_innerUserID.EditValue));
				UpdateCloudKeyAccess(_cloudUserInfo.UserId, _admin.IsChecked.Value, _isOwner.IsChecked.Value);
				if (!ConfigHelper.ASAdminLogin)
				{
					Settings.CreateApiClient().UserRoutes.UpdateUsers(ADHelper.ADB2CToken);
				}
				else
				{
					Settings.CreateApiClient().UserRoutes.UpdateUsersByASAdmin(ADHelper.ASAdminB2CToken);
				}
				Settings.CoreCacheService.User.Refresh();
				if (Settings.HostingEnvironment.CurrentViewPanel != null)
				{
					Settings.HostingEnvironment.CurrentViewPanel.Update(Convert.ToInt32(_userId.EditValue));
				}
				args.isValid = true;
				return;
			}
			catch (Exception ex)
			{
				args.SetFocusWhenInvalidSave = _email;
				args.isValid = false;
				Settings.ErrorHandler(ex);
				return;
			}
		}
		if (Mode != DialogWindowMode.EditMode)
		{
			return;
		}
		try
		{
			Settings.CreateApiClient().UserRoutes.UpdateUserMapping(Convert.ToInt32(_userId.EditValue), Convert.ToInt32(_innerUserID.EditValue));
			UpdateCloudKeyAccess(_userRow.UserId, _admin.IsChecked.Value, _isOwner.IsChecked.Value);
			if (!ConfigHelper.ASAdminLogin)
			{
				Settings.CreateApiClient().UserRoutes.UpdateUsers(ADHelper.ADB2CToken);
			}
			else
			{
				Settings.CreateApiClient().UserRoutes.UpdateUsersByASAdmin(ADHelper.ASAdminB2CToken);
			}
			Settings.CoreCacheService.User.Refresh();
			if (Settings.HostingEnvironment.CurrentViewPanel != null)
			{
				Settings.HostingEnvironment.CurrentViewPanel.Update(Convert.ToInt32(_userId.EditValue));
			}
			args.isValid = true;
		}
		catch (Exception ex2)
		{
			args.isValid = false;
			Settings.ErrorHandler(ex2);
		}
	}

	private void UpdateCloudKeyAccess(int cloudUserId, bool isAdmin, bool isDBAdmin)
	{
		if (_availableKeys == null)
		{
			return;
		}
		using CloudClient cloudClient = Settings.CloudClient;
		cloudClient.Token = Settings.GetApplicationToken();
		foreach (AvailableKeyRow availableKey in _availableKeys)
		{
			if (ConfigHelper.ASAdminLogin)
			{
				Settings.CreateApiClient().AdminCloud.EditCloudKeyMapping(ADHelper.ASAdminB2CToken, availableKey.Serial, cloudUserId, isAdmin, !availableKey.IsAvailable, isDBAdmin);
			}
			else
			{
				cloudClient.AS7X.EditUser(availableKey.Serial, cloudUserId, isAdmin, !availableKey.IsAvailable, isDBAdmin);
			}
		}
	}
}
