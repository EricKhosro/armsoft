using ArmSoft.SME.Core.UI.DataViews;
using ArmSoft.SME.Trade.Client.Data.Directories;

namespace ArmSoft.SME.Trade.UI.DropDown;

public class EMarkReceipApp : DropDownView<EMarkReceiptAppDataProvider.DataRow, EMarkReceiptAppDataProvider.Param>
{
	public override ReloadMode ReloadMode => ReloadMode.Reloadable;

	public EMarkReceipApp()
	{
		DataProvider = new EMarkReceiptAppDataProvider(TradeSettings.CreateTradeClient());
		FillColumnsFromDataProvider();
	}

	public override short ValueMemberLength()
	{
		return 3;
	}

	public override string ValueMember(int index)
	{
		return Data[index].Code;
	}

	public override string CommentMember(int index)
	{
		return Data[index].Name;
	}
}
