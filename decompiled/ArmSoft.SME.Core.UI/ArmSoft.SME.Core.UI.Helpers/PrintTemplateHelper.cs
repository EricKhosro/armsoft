namespace ArmSoft.SME.Core.UI.Helpers;

public static class PrintTemplateHelper
{
	public static string CombinePrintTemplateName(int documentType, int templateCode)
	{
		if (documentType < 10)
		{
			return $"0{documentType}.{templateCode}";
		}
		return $"{documentType}.{templateCode}";
	}
}
