using ArmSoft.SME.Common.MetaData.Document;
using ArmSoft.SME.Common.Models;

namespace ArmSoft.SME.Client.Data;

public class DocumentExtensionsDataProvider : DataProvider<DocumentExtensionsDataProvider.DataRow, DocumentExtensionsDataProvider.Param>
{
	public class DataRow : ExtendableRow, IRowWithKey, IMatchedUpdateKey
	{
		public short POTypeDef { get; set; }

		public string POTypeDefCaption { get; set; }

		public short POType { get; set; }

		public string POTypeCaption { get; set; }

		public string Description { get; set; }

		public bool Matched(object key)
		{
			DocumentExtensionKey documentExtensionKey = (DocumentExtensionKey)key;
			if (documentExtensionKey.POTypeDef == POTypeDef)
			{
				return documentExtensionKey.POType == POType;
			}
			return false;
		}

		public object GetKey()
		{
			return new DocumentExtensionKey
			{
				POTypeDef = POTypeDef,
				POType = POType
			};
		}

		public string GetCode()
		{
			return "";
		}

		public string GetName()
		{
			return "";
		}
	}

	public class Param : ExtendableParameter
	{
		public short? POTypeDef { get; set; }

		public short? POType { get; set; }
	}

	public override string Name => "DocumentExtensions";

	public DocumentExtensionsDataProvider(ApiClient client)
		: base(client)
	{
	}
}
