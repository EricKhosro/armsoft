using ArmSoft.OnlineBanking.Common.Enums;

namespace ArmSoft.SME.Core.UI.InternetBank;

public static class Converter
{
	public static AccountType Convert(this ArmSoft.OnlineBanking.Common.Enums.AccountType accountType)
	{
		return accountType switch
		{
			ArmSoft.OnlineBanking.Common.Enums.AccountType.NotSet => AccountType.NotSet, 
			ArmSoft.OnlineBanking.Common.Enums.AccountType.Settlement => AccountType.Settlement, 
			ArmSoft.OnlineBanking.Common.Enums.AccountType.Card => AccountType.Card, 
			ArmSoft.OnlineBanking.Common.Enums.AccountType.Other => AccountType.Other, 
			_ => AccountType.NotSet, 
		};
	}

	public static AccountAccessLevels Convert(this ArmSoft.OnlineBanking.Common.Enums.AccountAccessLevels accessLevel)
	{
		return accessLevel switch
		{
			ArmSoft.OnlineBanking.Common.Enums.AccountAccessLevels.None => AccountAccessLevels.None, 
			ArmSoft.OnlineBanking.Common.Enums.AccountAccessLevels.View => AccountAccessLevels.View, 
			ArmSoft.OnlineBanking.Common.Enums.AccountAccessLevels.MakeOperation => AccountAccessLevels.MakeOperation, 
			ArmSoft.OnlineBanking.Common.Enums.AccountAccessLevels.ViewDocument => AccountAccessLevels.ViewDocument, 
			ArmSoft.OnlineBanking.Common.Enums.AccountAccessLevels.FullControl => AccountAccessLevels.FullControl, 
			_ => AccountAccessLevels.None, 
		};
	}
}
