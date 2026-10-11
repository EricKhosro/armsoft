using DevExpress.Xpf.Core;

namespace ArmSoft.SME.Core.UI.Localizers;

internal class AsSearchPanelLocaLizer : SearchPanelLocalizer
{
	protected override void PopulateStringTable()
	{
		base.PopulateStringTable();
		AddString(SearchPanelStringId.ButtonText_Replace, "Փոխարինել");
		AddString(SearchPanelStringId.ButtonText_ReplaceAll, "Փոխարինել բոլորը");
		AddString(SearchPanelStringId.ButtonTooltip_Close, "Փակել");
		AddString(SearchPanelStringId.ButtonTooltip_FindNext, "Փնտրել հաջորդը");
		AddString(SearchPanelStringId.ButtonTooltip_FindPrev, "Փնտրել նախորդը");
		AddString(SearchPanelStringId.ButtonTooltip_SearchOptions, "Դրույթներ");
		AddString(SearchPanelStringId.LabelText_Find, "Փնտրել");
		AddString(SearchPanelStringId.LabelText_Replace, "Փոխարինել");
		AddString(SearchPanelStringId.MenuCheckItem_CaseSensative, "Հաշվի առնել տառաչափը");
		AddString(SearchPanelStringId.MenuCheckItem_UseRegularExpression, "Ռեգուլյար արտահայտություն");
		AddString(SearchPanelStringId.MenuCheckItem_WholeWord, "Փնտրել ամբողջ բառը");
	}
}
