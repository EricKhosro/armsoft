using ArmSoft.SME.Client.Data.Reports;
using ArmSoft.SME.Core.UI.Documents;

namespace ArmSoft.SME.Core.UI.DataViews.Reports;

[DataView(9)]
public class DatabaseConnections : DataView<DatabaseConnectionsDataProvider.DataRow, DatabaseConnectionsDataProvider.Param>
{
	public override string HelpId => "AdminTools_DBConnections";

	public override bool AllowDelete => true;

	public DatabaseConnections()
	{
		DataProvider = new DatabaseConnectionsDataProvider(Settings.CreateApiClient());
		FillColumnsFromDataProvider();
	}

	public override void Delete()
	{
		if (Settings.HostingEnvironment.CurrentViewPanel != null && Settings.HostingEnvironment.CurrentViewPanel.FocusedRow() != null)
		{
			DatabaseConnectionsDataProvider.DataRow dataRow = (DatabaseConnectionsDataProvider.DataRow)Settings.HostingEnvironment.CurrentViewPanel.FocusedRow();
			if (dataRow != null)
			{
				Settings.CommonCoreService.DeleteDbConnection(dataRow.SPID);
				Settings.HostingEnvironment.CurrentViewPanel.Update(dataRow.SPID);
			}
		}
	}
}
