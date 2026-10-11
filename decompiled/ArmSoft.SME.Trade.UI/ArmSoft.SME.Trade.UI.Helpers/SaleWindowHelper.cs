using System;
using System.Collections.Generic;
using ArmSoft.SME.Common.Enums;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Trade.Common.Parameters;
using ArmSoft.SME.Trade.Common.SmartEnums;

namespace ArmSoft.SME.Trade.UI.Helpers;

public static class SaleWindowHelper
{
	public enum ButtonCommands
	{
		Exit,
		Print,
		PrintWithCount,
		PrintCheck,
		Return,
		Defer,
		Continue,
		Save,
		Card,
		UseBonus,
		AddToBonus,
		ClearBonus,
		Code,
		BarCode,
		QTY,
		Price,
		DeleteRow,
		BackSpace,
		CompleteAmount,
		C,
		Numeric,
		ClearPayment,
		NotCash,
		Cash,
		MTFavorites,
		SelectMT,
		FocusOnInputField,
		FromPrePay,
		CreditSale,
		Other,
		SaleConsultant,
		PrePayECR,
		PrePayRetECR,
		TaxCode,
		FindBuyer,
		LastECRCheck,
		CashIn,
		CashOut,
		NewBuyer,
		Discount,
		QtyPlus,
		QtyMinus,
		QtyUnit,
		EMarkReceipt,
		SalesDocumentsHistory
	}

	public enum CurrentFocusedField : byte
	{
		None,
		Quantity,
		SalePrice,
		SaleSumm,
		Marking
	}

	public enum CurrentFocusedDiscountControlField : byte
	{
		None,
		DiscountPercent,
		DiscountSumm,
		DiscountedSumm
	}

	public enum PrintMode
	{
		Cash = 1,
		ByCard
	}

	public class ButtonClickEventArgs : EventArgs
	{
		public ButtonCommands ButtonCommand;

		public ButtonClickEventArgs(ButtonCommands buttonCommand)
		{
			ButtonCommand = buttonCommand;
		}
	}

	public class ButtonNumericClickEventArgs : ButtonClickEventArgs
	{
		public string ButtonText;

		public ButtonNumericClickEventArgs(string buttonText)
			: base(ButtonCommands.Numeric)
		{
			ButtonText = buttonText;
		}

		public ButtonNumericClickEventArgs(ButtonCommands buttonCommand, string buttonText)
			: base(buttonCommand)
		{
			ButtonText = buttonText;
		}
	}

	public class PrintButtonClickEventArgs : ButtonClickEventArgs
	{
		public string PrintTemplate;

		public short PrintCopies;

		public PrintButtonClickEventArgs(string template, short copies)
			: base(ButtonCommands.Print)
		{
			PrintTemplate = template;
			PrintCopies = copies;
		}
	}

	public static bool IsDefaultStorageRetail()
	{
		if (Settings.IsOffline)
		{
			return TradeSettings.LocalDBService.IsDefaultStorageRetail;
		}
		bool flag = TradeSettings.TradeCacheService.Storage.GetItem(Settings.ParametersService.DefaultStorage())?.Retail ?? false;
		TradeSettings.LocalDBService.IsDefaultStorageRetail = flag;
		return flag;
	}

	public static KeyValuePair<string, DocNumOfferSource> GetNextDocumentNumberAndRefreshOfflineIfNeeded(DocumentTypeSE docType)
	{
		if (Settings.IsOffline)
		{
			return TradeSettings.LocalDBService.GetNextDocumentNumber(docType);
		}
		KeyValuePair<string, DocNumOfferSource> nextDocNum = Settings.DocumentService.GetNextDocNum(docType, Settings.ParametersService.DefaultCash());
		TradeSettings.LocalDBService.SetNextDocumentNumber(docType, nextDocNum);
		return nextDocNum;
	}

	public static string TooltipOrNull(Func<string> getter)
	{
		string text = getter();
		if (!string.IsNullOrWhiteSpace(text))
		{
			return text;
		}
		return null;
	}
}
