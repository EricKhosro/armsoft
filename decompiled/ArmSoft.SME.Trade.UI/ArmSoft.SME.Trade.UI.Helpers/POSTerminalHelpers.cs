using System.Collections.Generic;
using System.Linq;
using Ardalis.SmartEnum;
using ArmSoft.SME.Trade.Common;
using ArmSoft.SME.Trade.Common.Models.Caches;
using ArmSoft.SME.Trade.Common.SmartEnums;

namespace ArmSoft.SME.Trade.UI.Helpers;

public class POSTerminalHelpers
{
	public class POSAcquiringInfoAsListItem
	{
		public string Caption { get; private set; }

		public string Device { get; private set; }

		public string Partner { get; private set; }

		public string Contract { get; private set; }

		public int Index { get; private set; }

		public string POSTerminalID { get; private set; }

		public string POSTerminalAddress { get; private set; }

		public short NonCashPaymentMethod { get; private set; }

		public POSAcquiringInfoAsListItem(CashCacheRow.NonCashInfo cashInfo, int index)
		{
			Caption = cashInfo.Caption;
			Device = cashInfo.Device;
			Partner = cashInfo.Partner;
			Contract = cashInfo.Contract;
			POSTerminalID = cashInfo.POSTerminalID;
			POSTerminalAddress = cashInfo.POSTerminalAddress;
			NonCashPaymentMethod = cashInfo.ECRPaymentMethod;
			Index = index;
		}

		public override string ToString()
		{
			string result = "";
			PartnerCacheRow item = TradeSettings.TradeCacheService.Partner.GetItem(Partner);
			if (item != null)
			{
				string pOSTerminalTypeCaptionByCode = GetPOSTerminalTypeCaptionByCode(Device);
				result = Index + ". " + (string.IsNullOrWhiteSpace(Caption) ? item.Caption : Caption) + ((!string.IsNullOrWhiteSpace(pOSTerminalTypeCaptionByCode)) ? (" (" + pOSTerminalTypeCaptionByCode + ")") : "");
			}
			return result;
		}
	}

	public static string GetPOSTerminalTypeCaptionByCode(string code)
	{
		if (SmartEnum<POSTerminalTypesSE, string>.TryFromValue(code, out var result))
		{
			return result.Caption;
		}
		return POSTerminalTypesSE.None.Caption;
	}

	public static string GetPOSTerminalTypeCodeByCaption(string caption)
	{
		if (caption == Resources.ECRPOS)
		{
			return "1";
		}
		if (caption == "ARCUS 2 POS (Ingenico/SUNMI)")
		{
			return "2";
		}
		if (caption == "Possoft Pay POS (Feitian)")
		{
			return "4";
		}
		return "0";
	}

	public static IEnumerable<POSAcquiringInfoAsListItem> GetPOSInfoByCashDesk(string cashDesk)
	{
		List<POSAcquiringInfoAsListItem> list = new List<POSAcquiringInfoAsListItem>();
		CashCacheRow cashCacheRow = TradeSettings.TradeCacheService.Cash.GetList().FirstOrDefault((CashCacheRow x) => x.Code == cashDesk);
		if (cashCacheRow == null)
		{
			return list;
		}
		int num = 1;
		if (cashCacheRow.NonCashInfoDict.TryGetValue(5, out var value) && !string.IsNullOrWhiteSpace(value.Partner))
		{
			list.Add(new POSAcquiringInfoAsListItem(value, num));
			num++;
		}
		if (cashCacheRow.NonCashInfoDict.TryGetValue(6, out value) && !string.IsNullOrWhiteSpace(value.Partner))
		{
			list.Add(new POSAcquiringInfoAsListItem(value, num));
			num++;
		}
		if (cashCacheRow.NonCashInfoDict.TryGetValue(7, out value) && !string.IsNullOrWhiteSpace(value.Partner))
		{
			list.Add(new POSAcquiringInfoAsListItem(value, num));
			num++;
		}
		if (cashCacheRow.NonCashInfoDict.TryGetValue(8, out value) && !string.IsNullOrWhiteSpace(value.Partner))
		{
			list.Add(new POSAcquiringInfoAsListItem(value, num));
			num++;
		}
		return list;
	}

	public static bool IsActivePOSTerminal(string cashDesk)
	{
		return TradeSettings.TradeCacheService.Cash.GetList().FirstOrDefault((CashCacheRow x) => x.Code == cashDesk)?.NonCashInfoList.Any((CashCacheRow.NonCashInfo x) => x.Device == "2" || x.Device == "4") ?? false;
	}
}
