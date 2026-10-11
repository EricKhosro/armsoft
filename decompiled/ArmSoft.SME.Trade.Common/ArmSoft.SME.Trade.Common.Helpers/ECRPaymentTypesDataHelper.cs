using System.Collections.Generic;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Constants;

namespace ArmSoft.SME.Trade.Common.Helpers;

public class ECRPaymentTypesDataHelper
{
	private static readonly List<string> values;

	static ECRPaymentTypesDataHelper()
	{
		values = new List<string>();
		InitValues();
	}

	private static void InitValues()
	{
		values.Clear();
		values.Add(string.Empty);
		values.Add(ArmSoft.SME.Common.Resources.CardPayment);
		values.Add(ArmSoft.SME.Common.Resources.Telcell);
		values.Add(ArmSoft.SME.Common.Resources.EasyPay);
		values.Add(ArmSoft.SME.Common.Resources.MobiDram);
		values.Add(ArmSoft.SME.Common.Resources.Idram);
		values.Add(ArmSoft.SME.Common.Resources.StakProcessing);
		values.Add(ArmSoft.SME.Common.Resources.FastShift);
		values.Add(ArmSoft.SME.Common.Resources.TeamPay);
	}

	public static IEnumerable<string> GetPaymentTypes()
	{
		InitValues();
		return values;
	}

	public static string GetPaymentTypeCodeByCaption(string caption)
	{
		if (caption == ArmSoft.SME.Common.Resources.CardPayment)
		{
			return "1";
		}
		if (caption == ArmSoft.SME.Common.Resources.Telcell)
		{
			return "10";
		}
		if (caption == ArmSoft.SME.Common.Resources.EasyPay)
		{
			return "11";
		}
		if (caption == ArmSoft.SME.Common.Resources.MobiDram)
		{
			return "12";
		}
		if (caption == ArmSoft.SME.Common.Resources.Idram)
		{
			return "13";
		}
		if (caption == ArmSoft.SME.Common.Resources.StakProcessing)
		{
			return "14";
		}
		if (caption == ArmSoft.SME.Common.Resources.FastShift)
		{
			return "15";
		}
		if (caption == ArmSoft.SME.Common.Resources.TeamPay)
		{
			return "16";
		}
		return "0";
	}

	public static string GetPaymentMethodTypeCaptionByCode(short Type)
	{
		return Type switch
		{
			1 => ArmSoft.SME.Common.Constants.Common.NonCashPaymentType.ARCA.ToString(), 
			2 => ArmSoft.SME.Common.Constants.Common.NonCashPaymentType.VISA.ToString(), 
			3 => ArmSoft.SME.Common.Constants.Common.NonCashPaymentType.MASTER.ToString(), 
			4 => ArmSoft.SME.Common.Constants.Common.NonCashPaymentType.AMEX.ToString(), 
			10 => ArmSoft.SME.Common.Constants.Common.NonCashPaymentType.MIR.ToString(), 
			5 => ArmSoft.SME.Common.Resources.PayX, 
			6 => ArmSoft.SME.Common.Resources.Idram, 
			7 => ArmSoft.SME.Common.Resources.InecoPay, 
			8 => ArmSoft.SME.Common.Resources.EasyPay, 
			9 => ArmSoft.SME.Common.Resources.Telcell, 
			11 => ArmSoft.SME.Common.Resources.CardPayment, 
			0 => string.Empty, 
			_ => string.Empty, 
		};
	}

	public static string GetPaymentTypeCaptionByCode(string code)
	{
		return code.Trim() switch
		{
			"1" => ArmSoft.SME.Common.Resources.CardPayment, 
			"10" => ArmSoft.SME.Common.Resources.Telcell, 
			"11" => ArmSoft.SME.Common.Resources.EasyPay, 
			"12" => ArmSoft.SME.Common.Resources.MobiDram, 
			"13" => ArmSoft.SME.Common.Resources.Idram, 
			"14" => ArmSoft.SME.Common.Resources.StakProcessing, 
			"15" => ArmSoft.SME.Common.Resources.FastShift, 
			"16" => ArmSoft.SME.Common.Resources.TeamPay, 
			_ => string.Empty, 
		};
	}
}
