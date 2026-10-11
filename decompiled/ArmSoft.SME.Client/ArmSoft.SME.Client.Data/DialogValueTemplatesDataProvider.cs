namespace ArmSoft.SME.Client.Data;

public class DialogValueTemplatesDataProvider : DataProvider<DialogValueTemplatesDataProvider.DataRow, DialogValueTemplatesDataProvider.Param>
{
	public class DataRow : ExtendableRow, IMatchedUpdateKey
	{
		public int UserId { get; set; }

		public int Id { get; set; }

		public string Caption { get; set; }

		public string Group { get; set; }

		public string UserName { get; set; }

		public string Body { get; set; }

		public bool Matched(object key)
		{
			return Id == (int)key;
		}
	}

	public class Param : ExtendableParameter
	{
		public int? UserId { get; set; }

		public string Group { get; set; }
	}

	public override string Name => "DialogValueTemplates";

	public DialogValueTemplatesDataProvider(ApiClient client)
		: base(client)
	{
	}
}
