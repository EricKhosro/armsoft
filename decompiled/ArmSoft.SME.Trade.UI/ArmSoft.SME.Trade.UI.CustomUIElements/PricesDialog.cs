using System;
using System.Windows;
using ArmSoft.SME.Common;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Core.UI.Controls;
using ArmSoft.SME.Trade.Common;
using ArmSoft.SME.Trade.UI.DropDown;
using ArmSoft.SME.Trade.UI.Helpers;
using DevExpress.Xpf.Editors;

namespace ArmSoft.SME.Trade.UI.CustomUIElements;

public class PricesDialog : DialogWindow
{
	private DropDownControl _priceListType;

	private DropDownControl _vatType;

	private TextEditExt _roundDgr;

	private DropDownControl _discountPriceType;

	private readonly bool _editMode;

	private readonly bool _storageIsRetail;

	private readonly bool _useDiscount;

	private readonly bool _allowChangePriceType;

	private readonly bool _allowChangeDiscountPriceType;

	private readonly bool _allowChangeDiscountPrices;

	public string VATType { get; set; }

	public string PriceListType { get; set; }

	public string PriceListTypeRoundDgr { get; set; }

	public string DiscountPriceListType { get; set; }

	public PricesDialog(Window Owner, string VATType, string PriceListType, string PriceTypeRoundDgr, string DiscountPriceListType, bool StorageIsRetail, bool UseDiscount, bool AllowChangePriceType, bool AllowChangeDiscountPrices, bool AllowChangeDiscountPriceType, bool EditMode)
	{
		base.Owner = Owner;
		this.VATType = VATType;
		this.PriceListType = PriceListType;
		PriceListTypeRoundDgr = PriceTypeRoundDgr;
		this.DiscountPriceListType = DiscountPriceListType;
		_storageIsRetail = StorageIsRetail;
		_useDiscount = UseDiscount;
		_allowChangePriceType = AllowChangePriceType;
		_allowChangeDiscountPriceType = AllowChangeDiscountPriceType;
		_allowChangeDiscountPrices = AllowChangeDiscountPrices;
		_editMode = EditMode;
		InnerInitialize();
	}

	private void InnerInitialize()
	{
		Caption = ArmSoft.SME.Common.Resources.Prices;
		Name = "Prices";
		_vatType = AddDropDownControl("VATType", ArmSoft.SME.Common.Resources.VATCalculationType, new VATTypes(includeVATRetail: false, onlyVATRetailTypes: true, vatRetailCaption: true), isRequired: true);
		_priceListType = AddDropDownControl("PriceType", ArmSoft.SME.Trade.Common.Resources.PriceType, new PriceTypes(), isRequired: true);
		_priceListType.EditValueChanged += new SafeEventHandler<EditValueChangedEventArgs>(PriceListType_EditValueChanged).Handler;
		_roundDgr = AddStringControl("RoundDgr", ArmSoft.SME.Common.Resources.DegreeOfRound, 6);
		_roundDgr.IsReadOnly = true;
		_roundDgr.Text = PriceListTypeRoundDgr;
		_discountPriceType = AddDropDownControl("DiscountPriceType", ArmSoft.SME.Trade.Common.Resources.DiscountPriceType, new PriceTypes());
		if (!_useDiscount)
		{
			((LayoutItemWithRequired)(object)_discountPriceType.Parent).Visibility = Visibility.Collapsed;
		}
		_priceListType.Code = PriceListType;
		if (_storageIsRetail)
		{
			_priceListType.IsRequired = false;
			if (!_useDiscount)
			{
				((LayoutItemWithRequired)(object)_priceListType.Parent).Visibility = Visibility.Collapsed;
				((LayoutItemWithRequired)(object)_roundDgr.Parent).Visibility = Visibility.Collapsed;
			}
		}
		_vatType.Code = Methods.VATAndRetailVATMapping(VATType, oppositeMapping: true);
		_discountPriceType.Code = DiscountPriceListType;
		if (!_editMode)
		{
			_vatType.IsReadOnly = true;
			_priceListType.IsReadOnly = true;
			_discountPriceType.IsReadOnly = true;
			Mode = DialogWindowMode.ViewMode;
		}
		else
		{
			if (!_allowChangePriceType && (!_storageIsRetail || !_allowChangeDiscountPrices))
			{
				_vatType.IsReadOnly = false;
				_priceListType.IsReadOnly = true;
			}
			if (!_allowChangeDiscountPriceType)
			{
				_discountPriceType.IsReadOnly = true;
			}
		}
		SetFocusOnActivation = _vatType;
		WindowValidation += new SafeEventHandler<DialogWindowValidationArgs>(PricesDialog_WindowValidation).Handler;
	}

	private void PricesDialog_WindowValidation(object window, DialogWindowValidationArgs e)
	{
		try
		{
			VATType = Methods.VATAndRetailVATMapping(_vatType.Code, oppositeMapping: false);
			PriceListType = _priceListType.Code;
			if (!string.IsNullOrWhiteSpace(PriceListType))
			{
				PriceListTypeRoundDgr = TradeSettings.CreateTradeClient().CommonServices.GetPriceTypeRoungDgr(PriceListType);
			}
			if (_useDiscount)
			{
				DiscountPriceListType = _discountPriceType.Code;
			}
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
			e.isValid = false;
			e.SetFocusWhenInvalidSave = _vatType;
		}
	}

	private void PriceListType_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		if (PriceHelper.GetPriceListRowVATType(_priceListType.Code, out var _, out var _))
		{
			_vatType.Code = "2";
		}
		if (!string.IsNullOrWhiteSpace(_priceListType.Code))
		{
			_roundDgr.Text = TradeSettings.CreateTradeClient().CommonServices.GetPriceTypeRoungDgr(_priceListType.Code);
		}
	}
}
