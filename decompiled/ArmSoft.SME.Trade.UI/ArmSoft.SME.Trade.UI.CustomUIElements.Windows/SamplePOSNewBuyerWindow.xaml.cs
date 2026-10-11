using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Markup;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Extensions;
using ArmSoft.SME.Common.Models.Directories;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Core.UI.Constants;
using ArmSoft.SME.Core.UI.Helpers;
using ArmSoft.SME.Trade.Common;
using ArmSoft.SME.Trade.Common.Models.Caches;
using ArmSoft.SME.Trade.Common.Models.Directories;
using ArmSoft.SME.Trade.Common.Parameters;
using ArmSoft.SME.Trade.Common.SmartEnums;
using ArmSoft.SME.Trade.UI.DropDown;
using ArmSoft.SME.Trade.UI.DropDownDataProvider;
using ArmSoft.SME.Trade.UI.Helpers;
using ArmSoft.SME.Trade.UI.Services;
using DevExpress.Xpf.Core;

namespace ArmSoft.SME.Trade.UI.CustomUIElements.Windows;

public partial class SamplePOSNewBuyerWindow : ThemedWindow, IComponentConnector
{
	private CurrentFocusedElement _currentFocusedElement;

	private decimal _discountPercent;

	public string PartnerCode { get; private set; }

	public SamplePOSNewBuyerWindow(Window owner, string discountCardCode = "")
	{
		InitializeComponent();
		Owner = owner;
		if (!string.IsNullOrWhiteSpace(discountCardCode))
		{
			TE_CardCode.Text = discountCardCode.Trim();
		}
		SetDefaultValues();
		ASFontHelper.SetFontSettings(this);
	}

	private void SetDefaultValues()
	{
		DiscountPercentTextEdit.Text = _discountPercent.ToString();
		TE_Name.Focus();
		_currentFocusedElement = CurrentFocusedElement.Name;
		PartnerCode = "";
	}

	private void SlashButton_Click(object sender, RoutedEventArgs e)
	{
		ForwardSymbolToFocusedField("/");
	}

	private void OKButton_Click(object sender, RoutedEventArgs e)
	{
		OnConfirmButtonPress();
	}

	private void ExitButton_Click(object sender, RoutedEventArgs e)
	{
		Close();
	}

	private void TabButton_Click(object sender, RoutedEventArgs e)
	{
		if (_currentFocusedElement == CurrentFocusedElement.Name)
		{
			TE_PhoneNumber.Focus();
		}
		else if (_currentFocusedElement == CurrentFocusedElement.PhoneNumber)
		{
			TE_EmailAddress.Focus();
		}
		else if (_currentFocusedElement == CurrentFocusedElement.EmailAddress)
		{
			TE_CardCode.Focus();
		}
		else if (_currentFocusedElement == CurrentFocusedElement.DiscountCardCode)
		{
			BE_Group.Focus();
		}
		else if (_currentFocusedElement == CurrentFocusedElement.Group)
		{
			TE_Birthday.Focus();
		}
		else if (_currentFocusedElement == CurrentFocusedElement.Birthday)
		{
			DiscountPercentTextEdit.Focus();
		}
		else if (_currentFocusedElement == CurrentFocusedElement.DiscountPercent)
		{
			TE_Name.Focus();
		}
		else
		{
			TE_Name.Focus();
		}
	}

	private void NumericTouchPadControlButton_Click(object sender, SaleWindowHelper.ButtonClickEventArgs e)
	{
		if (e.ButtonCommand == SaleWindowHelper.ButtonCommands.C)
		{
			CButton_Click();
		}
		else if (e.ButtonCommand == SaleWindowHelper.ButtonCommands.BackSpace)
		{
			BackButton_Click();
		}
		else if (e.ButtonCommand == SaleWindowHelper.ButtonCommands.Numeric)
		{
			ForwardSymbolToFocusedField(((SaleWindowHelper.ButtonNumericClickEventArgs)e).ButtonText);
		}
	}

	private void TE_Name_LostFocus(object sender, RoutedEventArgs e)
	{
		_currentFocusedElement = CurrentFocusedElement.Name;
	}

	private void TE_PhoneNumber_LostFocus(object sender, RoutedEventArgs e)
	{
		_currentFocusedElement = CurrentFocusedElement.PhoneNumber;
	}

	private void TE_EmailAddress_LostFocus(object sender, RoutedEventArgs e)
	{
		_currentFocusedElement = CurrentFocusedElement.EmailAddress;
	}

	private void TE_CardCode_LostFocus(object sender, RoutedEventArgs e)
	{
		_currentFocusedElement = CurrentFocusedElement.DiscountCardCode;
	}

	private void DiscountPercentTextEdit_LostFocus(object sender, RoutedEventArgs e)
	{
		_currentFocusedElement = CurrentFocusedElement.DiscountPercent;
	}

	private void TE_Birthday_LostFocus(object sender, RoutedEventArgs e)
	{
		_currentFocusedElement = CurrentFocusedElement.Birthday;
	}

	private void BE_Group_LostFocus(object sender, RoutedEventArgs e)
	{
		_currentFocusedElement = CurrentFocusedElement.Group;
	}

	private void ThemedWindow_PreviewKeyDown(object sender, KeyEventArgs e)
	{
		try
		{
			if (HotKeys.WithModifier.Control_Enter.Matches(this, e))
			{
				e.Handled = true;
				OnConfirmButtonPress();
			}
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	private void BE_Group_Click(object sender, RoutedEventArgs e)
	{
		PartnerGroups dropDownView = new PartnerGroups();
		TouchGridControlWithExtraButtons<PartnerGroupDataProvider.DataRow> touchGridControlWithExtraButtons = new TouchGridControlWithExtraButtons<PartnerGroupDataProvider.DataRow>(this, dropDownView, "PartnerGroupsTouchDialog");
		if (touchGridControlWithExtraButtons.ShowDialog().Value && touchGridControlWithExtraButtons.SelectedRow != null)
		{
			TE_Group.EditValue = touchGridControlWithExtraButtons.SelectedRow.Code;
		}
	}

	private void Validate()
	{
		int num = 1000;
		if (!string.IsNullOrWhiteSpace(TE_PhoneNumber.Text) && IsPhoneNumberExists())
		{
			throw new ASException(ArmSoft.SME.Trade.Common.Resources.PartnerWithCurrentPhoneNumberExists);
		}
		if (!string.IsNullOrWhiteSpace(TE_CardCode.Text))
		{
			TradeSettings.CreateTradeClient().CommonServices.CheckUserCardsForUnique(TE_CardCode.Text, -1, isForEdit: false);
			TradeSettings.CreateTradeClient().CommonServices.CheckDiscountCardForUnique(TE_CardCode.Text, -1, "", isForEdit: false);
			TradeSettings.CreateTradeClient().CommonServices.CheckEmployeeCardForUnique(TE_CardCode.Text, -1, isForEdit: false);
		}
		if (string.IsNullOrWhiteSpace(DiscountPercentTextEdit.Text))
		{
			_discountPercent = 0m;
		}
		else
		{
			if (!decimal.TryParse(DiscountPercentTextEdit.Text.Trim(), out _discountPercent))
			{
				throw new ASException(string.Format(ArmSoft.SME.Trade.Common.Resources.FieldMustBeNumeric, TB_DiscountPercent.Text));
			}
			if (_discountPercent >= (decimal)num)
			{
				_discountPercent = 0m;
				throw new ASException(string.Format(ArmSoft.SME.Trade.Common.Resources.FieldValueMustBeLessThanParameter, TB_DiscountPercent.Text, num));
			}
		}
		if (!string.IsNullOrWhiteSpace(TE_Birthday.Text))
		{
			if (!DateTime.TryParse(TE_Birthday.Text, out var result))
			{
				throw new ASException(string.Format(ArmSoft.SME.Common.Resources.FieldValueNotMatchFormat, ArmSoft.SME.Common.Resources.DateOfBirth, TE_Birthday.Text, ArmSoft.SME.Common.Resources.DateFormat));
			}
			if (result.Year <= 1900 || result.Year >= 2079)
			{
				throw new ASException(ArmSoft.SME.Common.Resources.InvalidYear);
			}
			TE_Birthday.EditValue = result.ToString("dd/MM/yyyy");
		}
	}

	private Partner FillDocument(string nameOfPartner)
	{
		Partner partner = Settings.DirectoryService.Create<Partner>(DirectoryTypeSE.Partner.Value);
		partner.Code = Settings.ParametersService.NextPartner();
		partner.Name = nameOfPartner;
		partner.FullCaption = nameOfPartner;
		partner.Phone = TE_PhoneNumber.Text;
		partner.Email = TE_EmailAddress.Text;
		partner.Supplier = false;
		partner.Customer = true;
		partner.PartnerContracts.Clear();
		partner.PartnerContracts.Add(PartnerContractHelper.AddDefaultContract(TE_CardCode.Text, _discountPercent, isBonusCard: false, 0m));
		if (!string.IsNullOrWhiteSpace(TE_Birthday.Text))
		{
			partner.Birthday = DateTime.Parse(TE_Birthday.Text).ToDateOnly();
		}
		partner.Type = TE_Group.Text;
		return partner;
	}

	private void AfterStore(Partner partnerDocument)
	{
		PartnerCode = partnerDocument.Code;
		if (Settings.ParametersService.ConnectTob2barmsoftDiscountServer())
		{
			PartnerCacheRow.ContractRow defaultContract = TradeSettings.TradeCacheService.Partner.GetDefaultContract(PartnerCode, out var partnerCacheRow);
			if (defaultContract != null && defaultContract.IsBonusCard && !string.IsNullOrWhiteSpace(defaultContract.DiscountCard))
			{
				DiscountServerService.CreateCardInfo(partnerCacheRow, defaultContract);
				TradeSettings.CreateTradeClient().CommonServices.EditDiscountServerData(partnerDocument.Id, defaultContract.Contract, exportedToDiscountServer: true);
			}
		}
	}

	private void OnConfirmButtonPress()
	{
		try
		{
			string nameOfPartner = GetNameOfPartner();
			if (string.IsNullOrWhiteSpace(nameOfPartner))
			{
				throw new ASException(ArmSoft.SME.Trade.Common.Resources.OneOfTheFieldsMustBeFilledIn);
			}
			Validate();
			Partner partner = FillDocument(nameOfPartner);
			Settings.DirectoryService.Store(partner, new DirStoreRequest
			{
				WithoutAccessCheck = Settings.ParametersService.AllowToRegisterNewBuyer()
			});
			AfterStore(partner);
			DialogResult = true;
			Close();
		}
		catch (Exception ex)
		{
			PartnerCode = "";
			Settings.ErrorHandler(ex);
		}
	}

	private bool IsPhoneNumberExists()
	{
		return TradeSettings.TradeCacheService.Partner.GetList().Any((PartnerCacheRow p) => p.Phone == TE_PhoneNumber.Text);
	}

	private string GetNameOfPartner()
	{
		if (!string.IsNullOrWhiteSpace(TE_Name.Text))
		{
			return TE_Name.Text;
		}
		if (!string.IsNullOrWhiteSpace(TE_PhoneNumber.Text))
		{
			return TE_PhoneNumber.Text;
		}
		if (!string.IsNullOrWhiteSpace(TE_EmailAddress.Text))
		{
			return TE_EmailAddress.Text;
		}
		throw new ASException(ArmSoft.SME.Trade.Common.Resources.OneOfTheFieldsMustBeFilledIn);
	}

	private void BackButton_Click()
	{
		if (_currentFocusedElement == CurrentFocusedElement.Name)
		{
			if (TE_Name.Text.Length > 0)
			{
				string text = TE_Name.Text;
				TE_Name.Text = "";
				TE_Name.Focus();
				TE_Name.Text = text.Left(text.Length - 1);
			}
		}
		else if (_currentFocusedElement == CurrentFocusedElement.PhoneNumber)
		{
			if (TE_PhoneNumber.Text.Length > 0)
			{
				string text = TE_PhoneNumber.Text;
				TE_PhoneNumber.Text = "";
				TE_PhoneNumber.Focus();
				TE_PhoneNumber.Text = text.Left(text.Length - 1);
			}
		}
		else if (_currentFocusedElement == CurrentFocusedElement.EmailAddress)
		{
			if (TE_EmailAddress.Text.Length > 0)
			{
				string text = TE_EmailAddress.Text;
				TE_EmailAddress.Text = "";
				TE_EmailAddress.Focus();
				TE_EmailAddress.Text = text.Left(text.Length - 1);
			}
		}
		else if (_currentFocusedElement == CurrentFocusedElement.DiscountCardCode)
		{
			if (TE_CardCode.Text.Length > 0)
			{
				string text = TE_CardCode.Text;
				TE_CardCode.Text = "";
				TE_CardCode.Focus();
				TE_CardCode.Text = text.Left(text.Length - 1);
			}
		}
		else if (_currentFocusedElement == CurrentFocusedElement.DiscountPercent && DiscountPercentTextEdit.Text.Length > 0)
		{
			string text = DiscountPercentTextEdit.Text;
			DiscountPercentTextEdit.Text = "";
			DiscountPercentTextEdit.Focus();
			DiscountPercentTextEdit.Text = text.Left(text.Length - 1);
		}
	}

	private void CButton_Click()
	{
		if (_currentFocusedElement == CurrentFocusedElement.Name)
		{
			TE_Name.Text = "";
			TE_Name.Focus();
		}
		else if (_currentFocusedElement == CurrentFocusedElement.PhoneNumber)
		{
			TE_PhoneNumber.Text = "";
			TE_PhoneNumber.Focus();
		}
		else if (_currentFocusedElement == CurrentFocusedElement.EmailAddress)
		{
			TE_EmailAddress.Text = "";
			TE_EmailAddress.Focus();
		}
		else if (_currentFocusedElement == CurrentFocusedElement.DiscountCardCode)
		{
			TE_CardCode.Text = "";
			TE_CardCode.Focus();
		}
		else if (_currentFocusedElement == CurrentFocusedElement.DiscountPercent)
		{
			DiscountPercentTextEdit.Text = "";
			DiscountPercentTextEdit.Focus();
		}
		else if (_currentFocusedElement == CurrentFocusedElement.Group)
		{
			TE_Group.Text = "";
			TE_Group.Focus();
		}
		else if (_currentFocusedElement == CurrentFocusedElement.Birthday)
		{
			TE_Birthday.Text = "";
			TE_Birthday.Focus();
		}
	}

	private void ForwardSymbolToFocusedField(string value)
	{
		if (_currentFocusedElement == CurrentFocusedElement.Name)
		{
			string text = TE_Name.Text;
			TE_Name.Text = "";
			TE_Name.Focus();
			TE_Name.Text = text + value;
		}
		else if (_currentFocusedElement == CurrentFocusedElement.PhoneNumber)
		{
			string text = TE_PhoneNumber.Text;
			TE_PhoneNumber.Text = "";
			TE_PhoneNumber.Focus();
			TE_PhoneNumber.Text = text + value;
		}
		else if (_currentFocusedElement == CurrentFocusedElement.EmailAddress)
		{
			string text = TE_EmailAddress.Text;
			TE_EmailAddress.Text = "";
			TE_EmailAddress.Focus();
			TE_EmailAddress.Text = text + value;
		}
		else if (_currentFocusedElement == CurrentFocusedElement.DiscountCardCode)
		{
			string text = TE_CardCode.Text;
			TE_CardCode.Text = "";
			TE_CardCode.Focus();
			TE_CardCode.Text = text + value;
		}
		else if (_currentFocusedElement == CurrentFocusedElement.DiscountPercent)
		{
			string text = DiscountPercentTextEdit.Text;
			DiscountPercentTextEdit.Text = "";
			DiscountPercentTextEdit.Focus();
			DiscountPercentTextEdit.Text = text + value;
		}
		else if (_currentFocusedElement == CurrentFocusedElement.Birthday)
		{
			string text = TE_Birthday.Text;
			TE_Birthday.Text = "";
			TE_Birthday.Focus();
			TE_Birthday.Text = text + value;
		}
	}

	[DebuggerNonUserCode]
	[GeneratedCode("PresentationBuildTasks", "10.0.9.0")]
	internal Delegate _CreateDelegate(Type delegateType, string handler)
	{
		return Delegate.CreateDelegate(delegateType, this, handler);
	}
}
