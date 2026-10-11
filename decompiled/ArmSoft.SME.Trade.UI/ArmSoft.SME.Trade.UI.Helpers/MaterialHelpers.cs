using System;
using System.Collections.Generic;
using ArmSoft.SME.Common.Extensions;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Trade.Common;
using ArmSoft.SME.Trade.Common.Parameters;

namespace ArmSoft.SME.Trade.UI.Helpers;

public class MaterialHelpers
{
	public static decimal GetGoodRemains(bool update, int goodId, string unit, ref Dictionary<(int, string), decimal> availabilityByGoodId, string storage = "")
	{
		try
		{
			decimal num = TradeSettings.TradeCacheService.Material.GetQntUnit(goodId, unit)?.Coef ?? 1m;
			(int, string) key = (goodId, storage);
			decimal num2;
			if (update || !availabilityByGoodId.TryGetValue(key, out var value))
			{
				num2 = TradeSettings.CreateTradeClient().CommonServices.GetGoodAvailability(goodId, storage);
				availabilityByGoodId[key] = num2;
			}
			else
			{
				num2 = value;
			}
			return (num2 / num).RoundAs(3);
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
			return 0m;
		}
	}

	public static string GenerateInnerBarcode()
	{
		short num = 0;
		string text = Settings.ParametersService.InternalBarcodePrefix() + Settings.ParametersService.InternalBarcodeNext();
		if (text.Length != 12)
		{
			throw new ASException(string.Format(Resources.ErrInternalBarcodeLength, 12));
		}
		for (int i = 1; i <= text.Length; i++)
		{
			if (!short.TryParse(MemoryExtensions.AsSpan(text, i - 1, 1), out var result))
			{
				throw new ASException(Resources.ErrInternalBarcodeNumeric);
			}
			num = ((i % 2 != 0) ? ((short)(num + result)) : ((short)(num + (short)(result * 3))));
		}
		return text + (short)((10 - num % 10) % 10);
	}
}
