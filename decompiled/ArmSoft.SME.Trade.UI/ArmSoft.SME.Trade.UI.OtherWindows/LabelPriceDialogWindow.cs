using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Extensions;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Core.UI.Constants;
using ArmSoft.SME.Core.UI.Controls;
using ArmSoft.SME.Trade.Client.Routes;
using ArmSoft.SME.Trade.Common;
using ArmSoft.SME.Trade.Common.Models.Directories;
using ArmSoft.SME.Trade.UI.DropDown;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Editors;
using DevExpress.Xpf.LayoutControl;

namespace ArmSoft.SME.Trade.UI.OtherWindows;

public class LabelPriceDialogWindow : DialogWindow
{
	public PopupCalcEditExt Coeff;

	private DropDownControl _priceType;

	private PopupCalcEditExt _salePrice;

	private PopupCalcEditExt _unitPrice;

	private readonly int _id;

	public LabelPriceDialogWindow(int id, string unitCode, string labelUnit, decimal labelCoeff, Window owner)
	{
		//IL_0162: Unknown result type (might be due to invalid IL or missing references)
		//IL_0168: Unknown result type (might be due to invalid IL or missing references)
		Name = "LabelPriceDialogWindow";
		Caption = ArmSoft.SME.Trade.Common.Resources.UnitPriceCalcExample;
		_id = id;
		Owner = owner;
		LayoutGroupExt container = AddHorizontalLayoutGroup();
		AddStoreValuesButton(container);
		Coeff = AddNumericControl(container, "Coeff", "1 " + TradeSettings.TradeCacheService.QntUnit.GetItem(unitCode)?.Brief + " = ", 19, 10, isRequired: true);
		Coeff.EditValue = labelCoeff;
		Coeff.EditValueChanged += new SafeEventHandler<EditValueChangedEventArgs>(LabelPriceDialogCoeff_EditValueChanged).Handler;
		AddTextBlock(container, " " + TradeSettings.TradeCacheService.QntUnit.GetItem(labelUnit)?.Brief, TextBlockImage.None);
		LayoutGroupExt layoutGroupExt = AddHorizontalLayoutGroup();
		layoutGroupExt.Padding = new Thickness(12.0);
		layoutGroupExt.Children.Add(new DXImage
		{
			Width = 16.0,
			Height = 16.0,
			Source = ImageService.GetImage("information_16.svg"),
			Margin = new Thickness(0.0, -6.0, 0.0, 0.0)
		});
		layoutGroupExt.Children.Add(new TextBlock
		{
			Text = ArmSoft.SME.Trade.Common.Resources.LabelPriceCalcText,
			Foreground = ColorsAS.Foreground.BlueBrush,
			TextWrapping = TextWrapping.Wrap,
			FontWeight = FontWeights.Thin,
			Margin = new Thickness(0.0, -6.0, 0.0, 0.0)
		});
		LayoutGroupExt container2 = AddVerticalLayoutGroup(null, LayoutGroupView.GroupBox, ArmSoft.SME.Trade.Common.Resources.LabelPricePreview);
		_priceType = AddDropDownControl(container2, "PriceType", ArmSoft.SME.Common.Resources.PriceListType, new PriceTypes(), isRequired: false, allowAdditionButtons: false, storeValue: true);
		_priceType.EditValueChanged += new SafeEventHandler<EditValueChangedEventArgs>(LabelPriceType_EditValueChanged).Handler;
		LayoutGroupExt container3 = AddHorizontalLayoutGroup(container2);
		_salePrice = AddNumericControl(container3, "SalePrice", ArmSoft.SME.Common.Resources.MTTAccSalePrice, 16, 2);
		_salePrice.IsReadOnly = true;
		AddTextBlock(container3, " / 1 " + TradeSettings.TradeCacheService.QntUnit.GetItem(unitCode)?.Brief, TextBlockImage.None);
		LayoutGroupExt container4 = AddHorizontalLayoutGroup(container2);
		_unitPrice = AddNumericControl(container4, "UnitPrice", ArmSoft.SME.Common.Resources.Unit + " " + ArmSoft.SME.Trade.Common.Resources.PriceLowerCase, 16, 2);
		_unitPrice.IsReadOnly = true;
		AddTextBlock(container4, " / 1 " + TradeSettings.TradeCacheService.QntUnit.GetItem(labelUnit)?.Brief, TextBlockImage.None);
	}

	private void LabelPriceType_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		if (!string.IsNullOrWhiteSpace(_priceType.Code))
		{
			CommonServicesRoutes commonServices = TradeSettings.CreateTradeClient().CommonServices;
			int id = _id;
			int num = 1;
			List<string> list = new List<string>(num);
			CollectionsMarshal.SetCount(list, num);
			CollectionsMarshal.AsSpan(list)[0] = _priceType.Code;
			Dictionary<string, MTPrice> mTPricesByTypesAndBaseUnit = commonServices.GetMTPricesByTypesAndBaseUnit(id, list);
			_salePrice.EditValue = mTPricesByTypesAndBaseUnit.FirstOrDefault().Value.Price.RoundAs(2);
			if (Coeff.Value != 0m)
			{
				_unitPrice.EditValue = (mTPricesByTypesAndBaseUnit.FirstOrDefault().Value.Price * 1m / Coeff.Value).RoundAs(2);
			}
			else
			{
				_unitPrice.EditValue = 0m;
			}
		}
		else
		{
			_salePrice.EditValue = 0m;
			_unitPrice.EditValue = 0m;
		}
	}

	private void LabelPriceDialogCoeff_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		if (Coeff.Value != 0m)
		{
			_unitPrice.EditValue = (_salePrice.Value * 1m / Coeff.Value).RoundAs(2);
		}
		else
		{
			_unitPrice.EditValue = 0m;
		}
	}
}
