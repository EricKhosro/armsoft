using ArmSoft.SME.Common;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Core.UI.Controls;
using ArmSoft.SME.Core.UI.DropDowns;
using ArmSoft.SME.Trade.Common;
using ArmSoft.SME.Trade.Common.Parameters;
using ArmSoft.SME.Trade.Common.SmartEnums;
using ArmSoft.SME.Trade.UI.DropDown;

namespace ArmSoft.SME.Trade.UI.DataView;

public class EditSelectedHelper
{
	public static void AddGoodsControls(DialogWindow dialog)
	{
		DialogChangeDemandControls.AddChangeDemandDropDownControl(dialog, "Group", ArmSoft.SME.Common.Resources.Group, new GoodsGroups(), isRequired: true, allowAdditionButtons: false, storeValue: false, isShowComment: true, -1, null, isCheckValue: true, "", storeWithoutValidation: true);
		DialogChangeDemandControls.AddChangeDemandDropDownControl(dialog, "CPACode", ArmSoft.SME.Common.Resources.CPAClassification, new CPACodes(), isRequired: false, allowAdditionButtons: false, storeValue: false, isShowComment: true, -1, null, isCheckValue: true, "", storeWithoutValidation: true);
		if (Settings.ParametersService.EnvironmentalFeePayer())
		{
			DialogChangeDemandControls.AddChangeDemandNumericControl(dialog, "EcoFeeRate", ArmSoft.SME.Common.Resources.EcoFeeRate, 5, 2, isRequired: false, allowNegativeNumbers: false, useCurrencyFormat: true, storeValue: false, null, storeWithoutValidation: true);
		}
		DialogChangeDemandControls.AddChangeDemandDropDownControl(dialog, "AltUnit", ArmSoft.SME.Trade.Common.Resources.ALTQUNIT, new ArmSoft.SME.Trade.UI.DropDown.QntUnits(), isRequired: true);
		DialogChangeDemandControls.AddChangeDemandDropDownControl(dialog, "ExpenseMethod", ArmSoft.SME.Common.Resources.MTExpenseMethod, new SmartEnum(typeof(MTExpenseMethodsSE)), isRequired: true, allowAdditionButtons: false, storeValue: false, isShowComment: true, -1, null, isCheckValue: true, "", storeWithoutValidation: true);
		DialogChangeDemandControls.AddChangeDemandBooleanControl(dialog, "VAT", ArmSoft.SME.Common.Resources.OptionsVAT, storeValue: false, null, storeWithoutValidation: true);
		DialogChangeDemandControls.AddChangeDemandBooleanControl(dialog, "ShowInPriceList", ArmSoft.SME.Trade.Common.Resources.ShowInPriceListes, storeValue: false, null, storeWithoutValidation: true);
		DialogChangeDemandControls.AddChangeDemandBooleanControl(dialog, "IsWeight", ArmSoft.SME.Trade.Common.Resources.WeightItem, storeValue: false, null, storeWithoutValidation: true);
		if (Settings.ParametersService.UseMarking())
		{
			DialogChangeDemandControls.AddChangeDemandBooleanControl(dialog, "CheckMarking", ArmSoft.SME.Trade.Common.Resources.CheckMarking, storeValue: false, null, storeWithoutValidation: true);
		}
		DialogChangeDemandControls.AddChangeDemandDropDownControl(dialog, "MainSupplier", ArmSoft.SME.Trade.Common.Resources.MainSupplier, new PartnersFilter(PartnersFilter.ShowPartnersModeType.CodeCaption, "", isFilterBySuppliers: true));
		DialogChangeDemandControls.AddChangeDemandDropDownControl(dialog, "LabelUnit", ArmSoft.SME.Trade.Common.Resources.LabelUnit, new ArmSoft.SME.Trade.UI.DropDown.QntUnits(), isRequired: false, allowAdditionButtons: false, storeValue: false, isShowComment: true, -1, null, isCheckValue: true, "", storeWithoutValidation: true);
		DialogChangeDemandControls.AddChangeDemandDropDownControl(dialog, "Country", ArmSoft.SME.Common.Resources.Country, new ArmSoft.SME.Trade.UI.DropDown.Countries(), isRequired: false, allowAdditionButtons: false, storeValue: false, isShowComment: true, -1, null, isCheckValue: true, "", storeWithoutValidation: true);
		DialogChangeDemandControls.AddChangeDemandDropDownControl(dialog, "Producer", ArmSoft.SME.Common.Resources.Producer, new Producer(), isRequired: false, allowAdditionButtons: false, storeValue: false, isShowComment: true, -1, null, isCheckValue: true, "", storeWithoutValidation: true);
		DialogChangeDemandControls.AddChangeDemandDropDownControl(dialog, "Property1", ArmSoft.SME.Trade.Common.Resources.Attribute1, new ArmSoft.SME.Trade.UI.DropDown.Properties(), isRequired: false, allowAdditionButtons: false, storeValue: false, isShowComment: true, -1, null, isCheckValue: true, "", storeWithoutValidation: true);
		DialogChangeDemandControls.AddChangeDemandDropDownControl(dialog, "Property2", ArmSoft.SME.Trade.Common.Resources.Attribute2, new ArmSoft.SME.Trade.UI.DropDown.Properties());
		DialogChangeDemandControls.AddChangeDemandNumericControl(dialog, "Discount", ArmSoft.SME.Trade.Common.Resources.Discount + " (%)", 6, 2, isRequired: false, allowNegativeNumbers: true, useCurrencyFormat: true, storeValue: false, null, storeWithoutValidation: true);
		DialogChangeDemandControls.AddChangeDemandNumericControl(dialog, "Extra", ArmSoft.SME.Trade.Common.Resources.Extra, 6, 2, isRequired: false, allowNegativeNumbers: true, useCurrencyFormat: true, storeValue: false, null, storeWithoutValidation: true);
		if (Settings.ParametersService.UseBonus())
		{
			if (Settings.ParametersService.BonusType() == BonusTypeSE.ByPercent.Value)
			{
				DialogChangeDemandControls.AddChangeDemandNumericControl(dialog, "Bonus", string.Format(ArmSoft.SME.Trade.Common.Resources.Bonus, ArmSoft.SME.Trade.Common.Resources.PercentSymbol), 6, 2, isRequired: false, allowNegativeNumbers: true, useCurrencyFormat: true, storeValue: false, null, storeWithoutValidation: true);
			}
			else
			{
				DialogChangeDemandControls.AddChangeDemandNumericControl(dialog, "Point", string.Format(ArmSoft.SME.Trade.Common.Resources.Bonus, ArmSoft.SME.Common.Resources.Unit), 16, 2, isRequired: false, allowNegativeNumbers: true, useCurrencyFormat: true, storeValue: false, null, storeWithoutValidation: true);
			}
			DialogChangeDemandControls.AddChangeDemandBooleanControl(dialog, "IgnoreBonus", ArmSoft.SME.Trade.Common.Resources.IgnoreBonus, storeValue: false, null, storeWithoutValidation: true);
		}
		DialogChangeDemandControls.AddChangeDemandNumericControl(dialog, "Coeff", ArmSoft.SME.Common.Resources.Coefficient, 7, 3, isRequired: false, allowNegativeNumbers: false, useCurrencyFormat: true, storeValue: false, null, storeWithoutValidation: true);
		DialogChangeDemandControls.AddChangeDemandDropDownControl(dialog, "SubstituteItemsGroup", ArmSoft.SME.Trade.Common.Resources.SubstituteItemsGroup, new ArmSoft.SME.Trade.UI.DropDown.SubstituteItemsGroups(), isRequired: false, allowAdditionButtons: false, storeValue: false, isShowComment: true, -1, null, isCheckValue: true, "", storeWithoutValidation: true);
		DialogChangeDemandControls.AddChangeDemandStringControl(dialog, "Description", ArmSoft.SME.Common.Resources.Specification, 255, isRequired: false, storeValue: false, isLeftAlign: false, null, storeWithoutValidation: true);
		DialogChangeDemandControls.AddChangeDemandStringControl(dialog, "CaptionEN", ArmSoft.SME.Trade.Common.Resources.MTNameEn, 200, isRequired: false, storeValue: false, isLeftAlign: false, null, storeWithoutValidation: true);
		DialogChangeDemandControls.AddChangeDemandStringControl(dialog, "CaptionRU", ArmSoft.SME.Trade.Common.Resources.MTNameRu, 200, isRequired: false, storeValue: false, isLeftAlign: false, null, storeWithoutValidation: true);
		DialogChangeDemandControls.AddChangeDemandStringControl(dialog, "AdditionalDescr1", ArmSoft.SME.Trade.Common.Resources.AdditionalDescription + "1", 255, isRequired: false, storeValue: false, isLeftAlign: false, null, storeWithoutValidation: true);
		DialogChangeDemandControls.AddChangeDemandStringControl(dialog, "AdditionalDescr2", ArmSoft.SME.Trade.Common.Resources.AdditionalDescription + "2", 255, isRequired: false, storeValue: false, isLeftAlign: false, null, storeWithoutValidation: true);
		DialogChangeDemandControls.AddLine(dialog);
	}
}
