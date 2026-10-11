using System.Collections.Generic;
using System.Linq;
using ArmSoft.SME.Client.Data.Reports;
using ArmSoft.SME.Common;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Core.UI.DataViews.Reports;
using ArmSoft.SME.Core.UI.Documents;

namespace ArmSoft.SME.Trade.UI.DataView.Reports;

[DataView(9)]
public class TradeDatabaseConnections : DatabaseConnections
{
	public override void InitContextFunctions()
	{
		base.InitContextFunctions();
		if (Settings.HostingEnvironment.CurrentViewPanel.FocusedRow() != null)
		{
			Settings.HostingEnvironment.CurrentViewPanel.AddContextFunction("ContextFunctions", "UsersWorkingPeriod", Resources.OptionWorkPeriod, UsersWorkingPeriod);
		}
	}

	private void UsersWorkingPeriod()
	{
		List<int> list = new List<int>();
		if (Settings.HostingEnvironment.CurrentViewPanel.GetSelectedCount() <= 1)
		{
			DatabaseConnectionsDataProvider.DataRow dataRow = new DatabaseConnectionsDataProvider.DataRow();
			dataRow = ((Settings.HostingEnvironment.CurrentViewPanel.GetSelectedCount() != 1) ? ((DatabaseConnectionsDataProvider.DataRow)Settings.HostingEnvironment.CurrentViewPanel.FocusedRow()) : Settings.HostingEnvironment.CurrentViewPanel.GetSelectedRows<DatabaseConnectionsDataProvider.DataRow>().FirstOrDefault());
			if (dataRow != null)
			{
				list.Add(dataRow.UserID);
			}
		}
		else
		{
			foreach (DatabaseConnectionsDataProvider.DataRow selectedRow in Settings.HostingEnvironment.CurrentViewPanel.GetSelectedRows<DatabaseConnectionsDataProvider.DataRow>())
			{
				if (!list.Contains(selectedRow.UserID))
				{
					list.Add(selectedRow.UserID);
				}
			}
		}
		new UsersWorkingPeriods(string.Join(",", list)).Show();
	}

	public override string GetDefaultFunction()
	{
		return "UsersWorkingPeriod";
	}
}
