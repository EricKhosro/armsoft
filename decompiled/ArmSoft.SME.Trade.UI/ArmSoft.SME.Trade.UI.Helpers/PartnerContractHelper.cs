using ArmSoft.SME.Common.Extensions;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Trade.Common;
using ArmSoft.SME.Trade.Common.Models.Directories;
using ArmSoft.SME.Trade.Common.Parameters;
using ArmSoft.SME.Trade.Common.SmartEnums;

namespace ArmSoft.SME.Trade.UI.Helpers;

public static class PartnerContractHelper
{
	public static Partner.PartnerContractRow AddDefaultContract(string cardCode, decimal discontPercent, bool isBonusCard, decimal bonusPercent)
	{
		Partner.PartnerContractRow partnerContractRow = new Partner.PartnerContractRow
		{
			Caption = Resources.DefaultContract,
			CurrCode = "AMD",
			IsDefault = true,
			DiscountCard = cardCode,
			Percent = discontPercent,
			Contract = StringExtensions.NextDocNum(new string('0', 3), 3)
		};
		if (Settings.ParametersService.UseBonus())
		{
			partnerContractRow.IsBonusCard = isBonusCard;
			if (isBonusCard)
			{
				partnerContractRow.BonusCardValidPeriodStart = Settings.HostingEnvironment.CurrentDate;
			}
			if (Settings.ParametersService.BonusType() == BonusTypeSE.ByPercent.Value)
			{
				partnerContractRow.Bonus = bonusPercent;
			}
			else
			{
				partnerContractRow.Point = bonusPercent;
			}
		}
		return partnerContractRow;
	}
}
