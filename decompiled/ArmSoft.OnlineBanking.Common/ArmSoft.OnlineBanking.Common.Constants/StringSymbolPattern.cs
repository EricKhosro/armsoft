using ArmSoft.OnlineBanking.Common.Enums;

namespace ArmSoft.OnlineBanking.Common.Constants;

public static class StringSymbolPattern
{
	public const string Numeric = "[0-9]";

	public const string Latin = "[0-9a-zA-Z_]";

	public const string AlphaNumeric = "[0-9a-zA-Z]";

	public const string Swift = "[0-9a-zA-Z/' ()+,.:?{}-]";

	public const string VisaB2BPattern = "^[a-zA-Z0-9.,/][a-zA-Z0-9.,/() -]*$";

	public static string LatArm => "(?![\"])[ -~«»—…Ա-Ֆ՚-՞ա-և։֊]";

	public static string LatArmWithQuotationMark => "[ -~«»—…Ա-Ֆ՚-՞ա-և։֊]";

	public static string LatArmMemo => "[ -~«»—…Ա-Ֆ՚-՞ա-և։֊\t\n\r]";

	public static string FromEnum(ArmSoft.OnlineBanking.Common.Enums.StringSymbolPattern pattern)
	{
		return pattern switch
		{
			ArmSoft.OnlineBanking.Common.Enums.StringSymbolPattern.Numeric => "[0-9]", 
			ArmSoft.OnlineBanking.Common.Enums.StringSymbolPattern.Latin => "[0-9a-zA-Z_]", 
			ArmSoft.OnlineBanking.Common.Enums.StringSymbolPattern.AlphaNumeric => "[0-9a-zA-Z]", 
			ArmSoft.OnlineBanking.Common.Enums.StringSymbolPattern.Swift => "[0-9a-zA-Z/' ()+,.:?{}-]", 
			ArmSoft.OnlineBanking.Common.Enums.StringSymbolPattern.LatArm => LatArm, 
			ArmSoft.OnlineBanking.Common.Enums.StringSymbolPattern.LatArmWithQuotationMark => LatArmWithQuotationMark, 
			ArmSoft.OnlineBanking.Common.Enums.StringSymbolPattern.LatArmMemo => LatArmMemo, 
			ArmSoft.OnlineBanking.Common.Enums.StringSymbolPattern.VisaB2B => "^[a-zA-Z0-9.,/][a-zA-Z0-9.,/() -]*$", 
			_ => string.Empty, 
		};
	}
}
