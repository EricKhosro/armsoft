using System;

namespace ArmSoft.SME.Client.Data.Reports;

public class DirsLogDataProvider : DataProvider<DirsLogDataProvider.DataRow, DirsLogDataProvider.Param>
{
	public class DataRow : ExtendableRow
	{
		public DateTime Date { get; set; }

		public int UserID { get; set; }

		public string UserName { get; set; }

		public string OpName { get; set; }

		public int ItemID { get; set; }

		public string Code { get; set; }

		public string Name { get; set; }

		public byte DirID { get; set; }

		public string DirName { get; set; }

		public string Comment { get; set; }

		public string CompName { get; set; }

		public string AppTypeName { get; set; }

		public string ChangeDescription { get; set; }
	}

	public class Param : ExtendableParameter
	{
		public DateOnly? OpStartDate { get; set; }

		public DateOnly? OpEndDate { get; set; }

		public string DirType { get; set; }

		public int UserId { get; set; }
	}

	public override string Name => "DirsLog";

	public DirsLogDataProvider(ApiClient client)
		: base(client)
	{
	}
}
