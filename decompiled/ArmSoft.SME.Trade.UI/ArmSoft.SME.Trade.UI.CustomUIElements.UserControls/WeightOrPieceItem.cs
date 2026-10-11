using System;
using ArmSoft.SME.Common.Extensions;

namespace ArmSoft.SME.Trade.UI.CustomUIElements.UserControls;

public abstract class WeightOrPieceItem
{
	public string Prefix;

	public short CodeLength;

	public short Precision;

	public string Unit;

	public bool IsValidBarCode(string scannerData)
	{
		if (scannerData.Length >= 13)
		{
			return scannerData.Substring(0, 2) == Prefix;
		}
		return false;
	}

	public decimal GetQuantity(string barCode)
	{
		return Convert.ToDecimal(Convert.ToDecimal(barCode.Substring(2 + CodeLength, barCode.Length - 2 - CodeLength - 1)) / (decimal)Math.Pow(10.0, Precision)).RoundAs(Precision);
	}

	public string GetCode(string barCode)
	{
		return barCode.Substring(2, CodeLength);
	}

	public abstract void StoreSettings();
}
