using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Core.UI.DataViews;
using ArmSoft.SME.Core.UI.Documents;
using ArmSoft.SME.Trade.Client.Data.Directories;
using ArmSoft.SME.Trade.Common;
using ArmSoft.SME.Trade.Common.Models.Directories;
using ArmSoft.SME.Trade.Common.SmartEnums;
using ArmSoft.SME.Trade.UI.Directory;
using DevExpress.Data;

namespace ArmSoft.SME.Trade.UI.DataView;

[DataView(10014)]
public class CashierShift : TradeDirectoryDataView<CashierShiftDataProvider.DataRow, CashierShiftDataProvider.Param>
{
	public override short DirectoryType => DirectoryTypeSE.CashierShift.Value;

	public override string HelpId => "Purchases_Lists_Otherlists_Shifts";

	public override bool AllowCreateCopy => false;

	public CashierShift()
	{
		DataProvider = new CashierShiftDataProvider(TradeSettings.CreateTradeClient());
		FillColumnsFromDataProvider();
	}

	public override void BeforeLoadData(DataViewPanel dvp)
	{
		base.BeforeLoadData(dvp);
		if (!dvp.IsSorted)
		{
			dvp.Set_ColumnSortOrder("CashDesk", ColumnSortOrder.Descending);
			dvp.Set_ColumnSortOrder("ShiftNumber");
		}
	}

	public override void InitContextFunctions()
	{
		base.InitContextFunctions();
		if (!AllowEdit)
		{
			return;
		}
		CashierShiftDataProvider.DataRow currentRow = (CashierShiftDataProvider.DataRow)Settings.HostingEnvironment.CurrentViewPanel.FocusedRow();
		if (currentRow != null && !currentRow.EndDate.HasValue)
		{
			Settings.HostingEnvironment.CurrentViewPanel.AddContextFunction("ContextFunctions", "CloseCashierShift", Resources.CloseCashierShift, () =>
			{
				CloseCashierShift(currentRow);
			});
		}
	}

	private void CloseCashierShift(CashierShiftDataProvider.DataRow currentRow)
	{
		CashierShiftDialogWindow cashierShiftDialogWindow = new CashierShiftDialogWindow((ArmSoft.SME.Trade.Common.Models.Directories.CashierShift)LoadDirectory(), DialogWindowMode.EditMode);
		cashierShiftDialogWindow.DVPId = Settings.HostingEnvironment.CurrentViewPanel.Id;
		cashierShiftDialogWindow.ForClosingCashierShift = true;
		cashierShiftDialogWindow.ShowDialog();
	}
}
