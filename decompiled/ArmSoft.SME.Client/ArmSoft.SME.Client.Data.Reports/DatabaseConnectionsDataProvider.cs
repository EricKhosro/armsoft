using System;

namespace ArmSoft.SME.Client.Data.Reports;

public class DatabaseConnectionsDataProvider : DataProvider<DatabaseConnectionsDataProvider.DataRow, DatabaseConnectionsDataProvider.Param>
{
	public class DataRow : ExtendableRow, IMatchedUpdateKey
	{
		public string HostName { get; set; }

		public int UserID { get; set; }

		public string User { get; set; }

		public string UserName { get; set; }

		public string Window { get; set; }

		public string Operation { get; set; }

		public DateTime LoginTime { get; set; }

		public DateTime LastTime { get; set; }

		public short SPID { get; set; }

		public string State { get; set; }

		public short Blocked { get; set; }

		public short Locked { get; set; }

		public string ProgramName { get; set; }

		public bool Matched(object key)
		{
			return (short)key == SPID;
		}
	}

	public class Param : ExtendableParameter
	{
	}

	public override string Name => "DatabaseConnections";

	public DatabaseConnectionsDataProvider(ApiClient client)
		: base(client)
	{
	}
}
