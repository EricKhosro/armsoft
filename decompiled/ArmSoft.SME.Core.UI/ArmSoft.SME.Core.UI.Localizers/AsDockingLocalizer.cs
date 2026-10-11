using DevExpress.Xpf.Docking.Base;

namespace ArmSoft.SME.Core.UI.Localizers;

internal class AsDockingLocalizer : DockingLocalizer
{
	protected override void PopulateStringTable()
	{
		base.PopulateStringTable();
		AddString(DockingStringId.MenuItemFloat, "Լողացող");
		AddString(DockingStringId.MenuItemCloseAllButThis, "Փակել մյուսները");
		AddString(DockingStringId.MenuItemPinTab, "Ամրացնել ներդիրը");
		AddString(DockingStringId.MenuItemDock, "Ամրացնել");
		AddString(DockingStringId.MenuItemAutoHide, "Ավտոմատ փակել");
		AddString(DockingStringId.MenuItemClose, "Փակել");
		AddString(DockingStringId.MenuItemNewHorizontalTabGroup, "Նոր հորիզոնական խումբ");
		AddString(DockingStringId.MenuItemNewVerticalTabGroup, "Նոր ուղղահայաց խումբ");
		AddString(DockingStringId.MenuItemMoveToNextTabGroup, "Տեղավորել էջանիշերի հաջորդ խմբում");
		AddString(DockingStringId.MenuItemMoveToPreviousTabGroup, "Տեղավորել էջանիշերի նախորդ խմբում");
		AddString(DockingStringId.ControlButtonAutoHide, "Ավտոմատ փակել");
		AddString(DockingStringId.ControlButtonClose, "Փակել");
	}
}
