using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Models.Caches;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Core.UI.Controls;
using ArmSoft.SME.Core.UI.DataViews.Directories;
using ArmSoft.SME.Trade.Common;
using ArmSoft.SME.Trade.Common.Parameters;
using ArmSoft.SME.Trade.UI.CustomUIElements.UserControls;
using DevExpress.Xpf.Editors;

namespace ArmSoft.SME.Trade.UI.CustomUIElements;

public class UserAuthenticationDialogWindow : DialogWindow, IScanable, IMagneticCardReadable
{
	private short _userParameterID;

	public UserAuthenticationDialogWindow(Window ownerWindow, short userParameterID)
	{
		Owner = ownerWindow;
		_userParameterID = userParameterID;
		InnerInitialize();
	}

	private void InnerInitialize()
	{
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		Caption = ArmSoft.SME.Trade.Common.Resources.ConfirmUser;
		Name = "ConfirmUser";
		HelpId = "ConfirmUser";
		ImageEdit myControl = new ImageEdit
		{
			Source = ImageService.GetImage("employee_card_96.svg"),
			IsEnabled = false,
			ShowBorder = false,
			Stretch = Stretch.Fill,
			Height = 96.0,
			Width = 96.0
		};
		Label label = new Label();
		TextBlock textBlock = new TextBlock();
		textBlock.Text = ArmSoft.SME.Trade.Common.Resources.PassMagneticCard;
		textBlock.TextWrapping = TextWrapping.Wrap;
		textBlock.VerticalAlignment = VerticalAlignment.Center;
		textBlock.FontSize = 14.0;
		label.Content = textBlock;
		label.BorderThickness = new Thickness(15.0, 25.0, 0.0, 0.0);
		label.Width = SizeCalculator.Width(300.0);
		label.Height = SizeCalculator.Height(96.0);
		_nextButton.Visibility = Visibility.Collapsed;
		_okButton.Visibility = Visibility.Collapsed;
		_cancelButton.Height = 40.0;
		LayoutGroupExt layoutGroupExt = AddHorizontalLayoutGroup();
		layoutGroupExt.Margin = new Thickness(0.0, 0.0, 0.0, 0.0);
		layoutGroupExt.VerticalAlignment = VerticalAlignment.Center;
		AddControl(layoutGroupExt, "Image", "", myControl);
		AddControl(layoutGroupExt, "TextBox", "", label);
	}

	public override void OnScanner(string scannerData)
	{
		OnMagneticCardReader(scannerData);
	}

	public override void OnMagneticCardReader(string mcrData)
	{
		if (Settings.IsOffline)
		{
			throw new ASException(ArmSoft.SME.Common.Resources.InvalidOperationForBrokenConnection);
		}
		if (!string.IsNullOrWhiteSpace(mcrData))
		{
			if (!MCRInfo.GetInstance().IsValidCard(mcrData))
			{
				throw new ASException(ArmSoft.SME.Trade.Common.Resources.InvalidCardCode + " " + mcrData);
			}
			if (!ReadMagneticCard(mcrData))
			{
				DialogResult = false;
				throw new ASException(ArmSoft.SME.Common.Resources.AccessDenied, 20008);
			}
			DialogResult = true;
		}
	}

	private bool ReadMagneticCard(string value)
	{
		string cardNumber = MCRInfo.GetInstance().GetCardNumber(value);
		bool result = false;
		int userIdByCardNumber = TradeSettings.CreateTradeClient().CommonServices.GetUserIdByCardNumber(cardNumber);
		UsersCacheRow item = Settings.CoreCacheService.User.GetItem(userIdByCardNumber);
		if (item == null)
		{
			throw new ASException(ArmSoft.SME.Trade.Common.Resources.InvalidCardCode + " " + cardNumber);
		}
		if (!item.Login)
		{
			throw new ASException(ArmSoft.SME.Common.Resources.UsersAccessDenied);
		}
		Users.CheckForAvailableSubsystems(item.UserId, item.Name);
		if (_userParameterID == Definitions.AllowPOSRecordReturnFromCustomer.Id)
		{
			result = Settings.ParametersService.GetBoolParamFromService(Definitions.AllowPOSRecordReturnFromCustomer, item.UserId);
		}
		else if (_userParameterID == Definitions.AllowPOSChangePrice.Id)
		{
			result = Settings.ParametersService.GetBoolParamFromService(Definitions.AllowPOSChangePrice, item.UserId);
		}
		else if (_userParameterID == Definitions.AllowPOSDeleteRow.Id)
		{
			result = Settings.ParametersService.GetBoolParamFromService(Definitions.AllowPOSDeleteRow, item.UserId);
		}
		else if (_userParameterID == Definitions.AllowPOSDecreaseQuantity.Id)
		{
			result = Settings.ParametersService.GetBoolParamFromService(Definitions.AllowPOSDecreaseQuantity, item.UserId);
		}
		else if (_userParameterID == Definitions.AllowPOSInputFromKeyboard.Id)
		{
			result = Settings.ParametersService.GetBoolParamFromService(Definitions.AllowPOSInputFromKeyboard, item.UserId);
		}
		else if (_userParameterID == Definitions.AllowPOSRemoveCard.Id)
		{
			result = Settings.ParametersService.GetBoolParamFromService(Definitions.AllowPOSRemoveCard, item.UserId);
		}
		return result;
	}
}
