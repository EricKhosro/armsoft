using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using ArmSoft.Cloud.Client;
using ArmSoft.Cloud.Client.AS7X;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.SmartEnums;
using ArmSoft.SME.Core.UI.BackgroundWorks;
using ArmSoft.SME.Core.UI.Constants;
using ArmSoft.SME.Core.UI.Controls;
using ArmSoft.SME.Core.UI.DataViewsData;
using ArmSoft.SME.Core.UI.Documents;
using ArmSoft.SME.Core.UI.DropDowns;
using ArmSoft.SME.Core.UI.StandartWindow;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Editors;

namespace ArmSoft.SME.Core.UI.DataViews;

[DataView(38)]
public class ApiKeys : DataView<ApiKeysDataProvider.DataRow, ApiKeysDataProvider.Param>
{
	private class ApiKeyDialogWindow : DialogWindow
	{
		private readonly DropDownControl _apiKeyType;

		private readonly TextEditExt _description;

		private readonly DropDownControl _user;

		private readonly CheckEditExt _enabled;

		private int _apiKeyId;

		protected override bool SkipReadOnlyControlsValidation => false;

		public ApiKeyDialogWindow(DialogWindowMode mode)
		{
			Mode = mode;
			Caption = ArmSoft.SME.Common.Resources.ApiKey;
			_apiKeyType = AddDropDownControl("ApiKeyType", ArmSoft.SME.Common.Resources.TypeWord, new SmartEnum(typeof(ApiKeyTypesSE)), isRequired: true);
			_apiKeyType.IsReadOnly = Mode != DialogWindowMode.NewMode;
			_description = AddStringControl("Description", ArmSoft.SME.Common.Resources.Description, 50, isRequired: true);
			_user = AddDropDownControl("UserName", ArmSoft.SME.Common.Resources.UserName, new UsersFilter(), isRequired: true);
			_enabled = AddBooleanControl("IsEnabled", ArmSoft.SME.Common.Resources.TemplateEnabled);
			Loaded += new SafeEventHandler<RoutedEventArgs>(ApiKeyDialogWindow_Loaded).Handler;
			WindowValidation += new SafeEventHandler<DialogWindowValidationArgs>(ApiKey_DialogWindowValidation).Handler;
		}

		private void ApiKeyDialogWindow_Loaded(object sender, RoutedEventArgs e)
		{
			if (Mode == DialogWindowMode.NewMode)
			{
				_apiKeyType.Code = ApiKeyTypesSE.PublicApi.Value.ToString();
				_enabled.IsChecked = true;
			}
			else if (Mode == DialogWindowMode.EditMode)
			{
				ApiKeysDataProvider.DataRow dataRow = (ApiKeysDataProvider.DataRow)Settings.HostingEnvironment.CurrentViewPanel.FocusedRow();
				if (dataRow != null)
				{
					_apiKeyType.Code = dataRow.ApiKeyType.ToString();
					_description.Text = dataRow.Description;
					_user.Code = dataRow.UserName;
					_enabled.IsChecked = dataRow.IsEnabled;
					_apiKeyId = dataRow.Id;
				}
			}
			SetFocusOnActivation = _description;
		}

		private void ApiKey_DialogWindowValidation(object sender, DialogWindowValidationArgs args)
		{
			try
			{
				CloudClient client = GetClient();
				if (Mode == DialogWindowMode.NewMode)
				{
					CloudPublicApiKeyCreationRequest request = new CloudPublicApiKeyCreationRequest
					{
						DbId = Settings.LoginService.DatabaseId,
						UserID = Settings.CoreCacheService.User.GetItemByName(_user.Code).CloudUserId,
						Description = _description.Text,
						IsEnabled = _enabled.IsChecked.Value,
						ApiType = (ApiType)Convert.ToInt16(_apiKeyType.Code)
					};
					new ApiSecretDialogWindow(client.AS7X.AddCloudPublicApiKey(request).Secret).ShowDialog();
				}
				else if (Mode == DialogWindowMode.EditMode)
				{
					CloudApiKeyUpdateRequest request2 = new CloudApiKeyUpdateRequest
					{
						Id = _apiKeyId,
						UserID = Settings.CoreCacheService.User.GetItemByName(_user.Code).CloudUserId,
						Description = _description.Text,
						IsEnabled = _enabled.IsChecked.Value
					};
					client.AS7X.EditCloudPublicApiKey(request2);
				}
				Settings.HostingEnvironment.CurrentViewPanel.Refresh();
			}
			catch (Exception ex)
			{
				Settings.ErrorHandler(ex);
				args.isValid = false;
			}
		}
	}

	private class ApiSecretDialogWindow : DialogWindow
	{
		public override ImageSource OkButtonImageSource
		{
			get
			{
				//IL_0009: Unknown result type (might be due to invalid IL or missing references)
				//IL_000f: Unknown result type (might be due to invalid IL or missing references)
				return ImageService.GetImage("accept_16.svg");
			}
		}

		public override string OkButtonText => "OK";

		public ApiSecretDialogWindow(string secretKey)
		{
			//IL_007b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0081: Unknown result type (might be due to invalid IL or missing references)
			//IL_0167: Unknown result type (might be due to invalid IL or missing references)
			//IL_016d: Unknown result type (might be due to invalid IL or missing references)
			Caption = ArmSoft.SME.Common.Resources.ApiKey;
			_cancelButton.Visibility = Visibility.Collapsed;
			LayoutGroupExt layoutGroupExt = AddHorizontalLayoutGroup();
			layoutGroupExt.Padding = new Thickness(12.0);
			layoutGroupExt.Children.Add(new DXImage
			{
				Width = 14.0,
				Height = 14.0,
				Source = ImageService.GetImage("information_16.svg"),
				Margin = new Thickness(0.0, -5.0, 0.0, 0.0)
			});
			layoutGroupExt.Children.Add(new TextBlock
			{
				Text = ArmSoft.SME.Common.Resources.MessageForAPIConnection,
				Foreground = ColorsAS.Foreground.BlueBrush,
				TextWrapping = TextWrapping.Wrap,
				FontWeight = FontWeights.Thin,
				Margin = new Thickness(0.0, -5.0, 0.0, 0.0)
			});
			ButtonEditExt buttonEditExt = AddButtonEditControl("Secret", ArmSoft.SME.Common.Resources.ApiKey, 20, isRequired: false, 1);
			buttonEditExt.IsReadOnly = true;
			buttonEditExt.Text = secretKey;
			DXImage content = new DXImage
			{
				Source = ImageService.GetImage("copy_14.svg")
			};
			ButtonInfo buttonInfo = new ButtonInfo
			{
				GlyphKind = GlyphKind.User,
				Content = content,
				CommandParameter = buttonEditExt
			};
			buttonEditExt.Buttons.Add(buttonInfo);
			buttonInfo.Click += new SafeEventHandler<RoutedEventArgs>(Copy_Click).Handler;
		}

		private void Copy_Click(object sender, RoutedEventArgs e)
		{
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			if (sender is Button { CommandParameter: ButtonEditExt commandParameter })
			{
				Clipboard.SetDataObject(commandParameter.EditText);
				ToastNotificationViewModel toastNotificationViewModel = new ToastNotificationViewModel
				{
					Image = ImageService.GetImage("accept_32.svg"),
					HistoryItem = new HistoryItem(HistoryItem.ItemCategory.Notification),
					Caption = ArmSoft.SME.Common.Resources.KeyNumCopedToBufer,
					Text = ArmSoft.SME.Common.Resources.ApiKey
				};
				toastNotificationViewModel.HistoryItem.Description = toastNotificationViewModel.Text;
				toastNotificationViewModel.HistoryItem.Details = toastNotificationViewModel.Text;
				toastNotificationViewModel.HistoryItem.Icon = "accept_16.svg";
				toastNotificationViewModel.HistoryItem.Id = "ApiKey";
				Settings.HostingEnvironment.ShowNotificationAsync(toastNotificationViewModel);
			}
		}
	}

	public override bool AllowAdd => Settings.LoginService.IsAdmin;

	public override bool AllowEdit => Settings.LoginService.IsAdmin;

	public override bool AllowDelete => Settings.LoginService.IsAdmin;

	public override string HelpId => "AdminTools_Rest_Api_Keys";

	public ApiKeys()
	{
		DataProvider = new ApiKeysDataProvider(Settings.CreateApiClient());
		FillColumnsFromDataProvider();
	}

	public override void InitContextFunctions()
	{
		base.InitContextFunctions();
		Settings.HostingEnvironment.CurrentViewPanel.AddContextSeparator("ContextFunctions");
		Settings.HostingEnvironment.CurrentViewPanel.AddContextFunction("ContextFunctions", "GenerateNewApiKey", Resources.GenerateNewApiKey, GenerateNewApiKey);
	}

	public override void Add()
	{
		new ApiKeyDialogWindow(DialogWindowMode.NewMode).ShowDialog();
	}

	public override void Edit()
	{
		if (Settings.HostingEnvironment.CurrentViewPanel.FocusedRow() != null)
		{
			new ApiKeyDialogWindow(DialogWindowMode.EditMode).ShowDialog();
		}
	}

	public override void Delete()
	{
		ApiKeysDataProvider.DataRow dataRow = (ApiKeysDataProvider.DataRow)Settings.HostingEnvironment.CurrentViewPanel.FocusedRow();
		if (dataRow != null && MessageBox.Show(string.Format(Resources.ConfRecordDel, Resources.ApiKey), Settings.ProgramName, MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No) == MessageBoxResult.Yes)
		{
			GetClient().AS7X.DeleteCloudPublicApiKey(dataRow.Id);
			Settings.HostingEnvironment.CurrentViewPanel.Refresh();
		}
	}

	private void GenerateNewApiKey()
	{
		ApiKeysDataProvider.DataRow dataRow = (ApiKeysDataProvider.DataRow)Settings.HostingEnvironment.CurrentViewPanel.FocusedRow();
		if (dataRow != null)
		{
			new ApiSecretDialogWindow(GetClient().AS7X.GenerateNewSecretForCloudPublicApiKey(dataRow.Id)).ShowDialog();
			Settings.HostingEnvironment.CurrentViewPanel.Refresh();
		}
	}

	private static CloudClient GetClient()
	{
		CloudClient cloudClient = Settings.CloudClient;
		cloudClient.Token = Settings.GetApplicationToken();
		return cloudClient;
	}
}
