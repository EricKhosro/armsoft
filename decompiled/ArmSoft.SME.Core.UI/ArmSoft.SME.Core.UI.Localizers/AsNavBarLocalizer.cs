using DevExpress.Xpf.NavBar;

namespace ArmSoft.SME.Core.UI.Localizers;

internal class AsNavBarLocalizer : NavBarLocalizer
{
	protected override void PopulateStringTable()
	{
		base.PopulateStringTable();
		AddString(NavBarStringId.NavPaneMenuAddRemoveButtons, "Ղեկավարել");
		AddString(NavBarStringId.NavPaneMenuShowFewerButtons, "Ներքև");
		AddString(NavBarStringId.NavPaneMenuShowMoreButtons, "Վերև");
	}
}
