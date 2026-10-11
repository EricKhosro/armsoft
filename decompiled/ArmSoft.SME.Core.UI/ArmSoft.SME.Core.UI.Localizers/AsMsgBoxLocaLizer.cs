using DevExpress.Xpf.Core;

namespace ArmSoft.SME.Core.UI.Localizers;

internal class AsMsgBoxLocaLizer : DXMessageBoxLocalizer
{
	protected override void PopulateStringTable()
	{
		base.PopulateStringTable();
		AddString(DXMessageBoxStringId.Cancel, "Դադարեցնել");
		AddString(DXMessageBoxStringId.No, "Ոչ");
		AddString(DXMessageBoxStringId.Ok, "Կատարել");
		AddString(DXMessageBoxStringId.Yes, "Այո");
	}
}
