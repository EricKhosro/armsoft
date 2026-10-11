using System;

namespace ArmSoft.SME.Client.Data.Reports;

public class DocumentLogDataProvider : DataProvider<DocumentLogDataProvider.DataRow, DocumentLogDataProvider.Param>
{
	public class DataRow : ExtendableRow
	{
		public DateTime Date { get; set; }

		public int UserID { get; set; }

		public string UserName { get; set; }

		public byte State { get; set; }

		public string StateName { get; set; }

		public string OpName { get; set; }

		public string Comment { get; set; }

		public string CompName { get; set; }

		public string AppTypeName { get; set; }

		public string ChangeDescription { get; set; }
	}

	public class Param : ExtendableParameter
	{
		public Guid ISN { get; set; }
	}

	public override string Name => "DocumentLog";

	public DocumentLogDataProvider(ApiClient client)
		: base(client)
	{
	}
}
