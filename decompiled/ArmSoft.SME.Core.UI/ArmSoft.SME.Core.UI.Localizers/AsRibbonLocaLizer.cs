using DevExpress.Xpf.Ribbon;

namespace ArmSoft.SME.Core.UI.Localizers;

internal class AsRibbonLocaLizer : RibbonControlLocalizer
{
	protected override void PopulateStringTable()
	{
		base.PopulateStringTable();
		AddString(RibbonControlStringId.RibbonPopupMenuItemText_AddToToolbar, "Ավելացնել արագ հասանելիության վահանակին");
		AddString(RibbonControlStringId.RibbonPopupMenuItemText_ShowQuickAccessToolbarBelowTheRibbon, "Բերել արագ հասանելիության վահանակը ներքևում");
		AddString(RibbonControlStringId.RibbonPopupMenuItemText_ShowQuickAccessToolbarAboveTheRibbon, "Բերել արագ հասանելիության վահանակը վերևում");
		AddString(RibbonControlStringId.RibbonPopupMenuItemText_RemoveFromToolbar, "Հեռացնել արագ հասանելիության վահանակից");
		AddString(RibbonControlStringId.RibbonPopupMenuItemText_MinimizeRibbon, "Նվազեցնել ժապավենը");
		AddString(RibbonControlStringId.RibbonPopupMenuItemText_UseSimplifiedRibbon, "Պարզեցված ժապավեն");
		AddString(RibbonControlStringId.RibbonPopupMenuItemText_UseClassicRibbon, "Դասական ժապավեն");
		AddString(RibbonControlStringId.RibbonCustomizationStrings_CustomizationFormCaption, "Ձևափոխել");
		AddString(RibbonControlStringId.RibbonPopupMenuItemText_CustomizeRibbonMenuItem, "Ձևափոխել");
	}
}
