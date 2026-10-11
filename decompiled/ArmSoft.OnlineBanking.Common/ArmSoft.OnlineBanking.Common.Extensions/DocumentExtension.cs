namespace ArmSoft.OnlineBanking.Common.Extensions;

public static class DocumentExtension
{
	public static bool IsCreatedByPartner(this IDocument document)
	{
		if (document.PartnerClientId.HasValue)
		{
			return document.PartnerClientId != document.ClientId;
		}
		return false;
	}

	public static string GetDocumentSubType(this string docType)
	{
		if (string.IsNullOrEmpty(docType))
		{
			return null;
		}
		string[] array = docType.Split('.');
		if (array.Length > 1)
		{
			return array[1];
		}
		return null;
	}
}
