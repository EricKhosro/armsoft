namespace ArmSoft.SME.Client.Data.Reports;

public class TemplatesDataProvider : DataProvider<TemplatesDataProvider.DataRow, TemplatesDataProvider.Param>
{
	public class DataRow : ExtendableRow, IMatchedUpdateKey
	{
		public string Name { get; set; }

		public string Caption { get; set; }

		public string File { get; set; }

		public short DocType { get; set; }

		public string DocTypeCaption { get; set; }

		public bool Enabled { get; set; }

		public bool IsSystem { get; set; }

		public string FileTypeName { get; set; }

		public bool IsValidFileExtension { get; set; }

		public string FileFormat { get; set; }

		public bool Matched(object key)
		{
			return (string)key == Name;
		}
	}

	public class Param : ExtendableParameter
	{
		public string TemplateType { get; set; }
	}

	public override string Name => "Templates";

	public TemplatesDataProvider(ApiClient client)
		: base(client)
	{
	}
}
