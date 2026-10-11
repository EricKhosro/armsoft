using System;

namespace ArmSoft.SME.Client.Data.Reports;

public class DirectoryLogDataProvider : DataProvider<DirectoryLogDataProvider.DataRow, DirectoryLogDataProvider.Param>
{
	public class DataRow : ExtendableRow
	{
		public DateTime Date { get; set; }

		public int UserID { get; set; }

		public string UserName { get; set; }

		public string OpName { get; set; }

		public string Comment { get; set; }

		public string CompName { get; set; }

		public string AppTypeName { get; set; }

		public string ChangeDescription { get; set; }
	}

	public class Param : ExtendableParameter
	{
		public short DirectoryLogID { get; set; }

		public int ItemID { get; set; }
	}

	public override string Name => "DirectoryLog";

	public DirectoryLogDataProvider(ApiClient client)
		: base(client)
	{
	}
}
