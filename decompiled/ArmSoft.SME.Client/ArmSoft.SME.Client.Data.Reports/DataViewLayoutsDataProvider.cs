using ArmSoft.SME.Common.Enums;

namespace ArmSoft.SME.Client.Data.Reports;

public class DataViewLayoutsDataProvider : DataProvider<DataViewLayoutsDataProvider.DataRow, DataViewLayoutsDataProvider.Param>
{
	public class DataRow : ExtendableRow, IMatchedUpdateKey
	{
		public class DataRowKey
		{
			public int UserID { get; set; }

			public string DataView { get; set; }

			public string Caption { get; set; }
		}

		public string DataView { get; set; }

		public string DataViewCaption { get; set; }

		public string Caption { get; set; }

		public int UserID { get; set; }

		public string UserName { get; set; }

		public DataViewTypes Type { get; set; }

		public bool Matched(object key)
		{
			DataRowKey dataRowKey = (DataRowKey)key;
			if (dataRowKey.UserID == UserID && dataRowKey.DataView == DataView)
			{
				return dataRowKey.Caption == Caption;
			}
			return false;
		}
	}

	public class Param : ExtendableParameter
	{
	}

	public override string Name => "DataViewLayouts";

	public DataViewLayoutsDataProvider(ApiClient client)
		: base(client)
	{
	}
}
