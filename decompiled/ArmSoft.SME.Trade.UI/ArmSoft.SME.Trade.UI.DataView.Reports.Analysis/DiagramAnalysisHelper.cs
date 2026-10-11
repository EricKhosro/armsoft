using System;
using System.Collections.Generic;
using System.Text;
using Ardalis.SmartEnum;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Extensions;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Core.UI.Report;
using ArmSoft.SME.Trade.Common;
using ArmSoft.SME.Trade.Common.Parameters;
using ArmSoft.SME.Trade.Common.SmartEnums;

namespace ArmSoft.SME.Trade.UI.DataView.Reports.Analysis;

public class DiagramAnalysisHelper
{
	public static string GetReportTitle(string captionToolTip, Dictionary<string, AsReport.ReportViewerParameter> parameters)
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append(captionToolTip);
		foreach (AsReport.ReportViewerParameter value in parameters.Values)
		{
			if (!value.IncludeCaptionToolTip)
			{
				continue;
			}
			stringBuilder.AppendLine();
			stringBuilder.Append(value.Caption.PadLeft(3));
			stringBuilder.Append("  - ");
			if (value.Name == "SumType")
			{
				stringBuilder.Append(SumTypeText((CommonConst.SalesSumType)value.Value));
			}
			else if (value.Name == "CalcPeriod")
			{
				stringBuilder.Append(GetPeriodName(SmartEnum<DatePeriodTypeSE, short>.FromValue(Convert.ToInt16(value.Value))));
			}
			else if (value.Name == "GoodsCompareMode")
			{
				stringBuilder.Append(GetRepColName((CommonConst.BestSellersCompareMode)value.Value));
			}
			else if (value.Value is string text)
			{
				if (string.IsNullOrEmpty(text))
				{
					stringBuilder.Append(ArmSoft.SME.Common.Resources.All);
					continue;
				}
				if (value.DataView == null)
				{
					stringBuilder.Append("\"");
					stringBuilder.Append(text);
					stringBuilder.Append("\"");
					continue;
				}
				Dictionary<string, string> keyValuesDict = value.DataView.GetKeyValuesDict();
				if (keyValuesDict == null || !keyValuesDict.ContainsKey(text))
				{
					stringBuilder.Append("\"");
					stringBuilder.Append(text);
					stringBuilder.Append("\"");
				}
				else
				{
					stringBuilder.Append(keyValuesDict[text]);
				}
			}
			else if (value.Value == null)
			{
				stringBuilder.Append(ArmSoft.SME.Common.Resources.All);
			}
			else if (value.Value is bool flag)
			{
				stringBuilder.Append(flag ? ArmSoft.SME.Common.Resources.Yes : ArmSoft.SME.Common.Resources.No);
			}
			else if (string.IsNullOrWhiteSpace(value.Format))
			{
				stringBuilder.Append(value.Value.ToString());
			}
			else
			{
				stringBuilder.Append(string.Format(value.Format, value.Value));
			}
		}
		return stringBuilder.ToString();
	}

	public static string GetRepColName(CommonConst.BestSellersCompareMode Mode)
	{
		return Mode switch
		{
			CommonConst.BestSellersCompareMode.Quantity => ArmSoft.SME.Common.Resources.Quantity, 
			CommonConst.BestSellersCompareMode.Income => ArmSoft.SME.Common.Resources.Income, 
			CommonConst.BestSellersCompareMode.Profit => ArmSoft.SME.Common.Resources.ProfitWord, 
			CommonConst.BestSellersCompareMode.ProfitPercent => ArmSoft.SME.Common.Resources.ProfitPercent, 
			_ => ArmSoft.SME.Common.Resources.ProfitWord, 
		};
	}

	public static string SumTypeText(CommonConst.SalesSumType SumTypePar)
	{
		return SumTypePar switch
		{
			CommonConst.SalesSumType.SumNCC => ArmSoft.SME.Trade.Common.Resources.AnalysSumTypeNcc, 
			CommonConst.SalesSumType.WithVAT => ArmSoft.SME.Trade.Common.Resources.AnalysSumTypeVat, 
			_ => string.Format(ArmSoft.SME.Trade.Common.Resources.AnalysSumTypeCur, Settings.ParametersService.MTCurr()), 
		};
	}

	public static string GetPeriodName(DatePeriodTypeSE periodType)
	{
		if ((short)periodType == DatePeriodTypeSE.Day.Value)
		{
			return ArmSoft.SME.Common.Resources.Day;
		}
		if ((short)periodType == DatePeriodTypeSE.Month.Value)
		{
			return ArmSoft.SME.Common.Resources.Month;
		}
		if ((short)periodType == DatePeriodTypeSE.Week.Value)
		{
			return ArmSoft.SME.Common.Resources.Week;
		}
		if ((short)periodType == DatePeriodTypeSE.Year.Value)
		{
			return ArmSoft.SME.Common.Resources.Year;
		}
		return "";
	}

	public static void DialogValidation(DialogWindow window, DialogWindowValidationArgs args)
	{
		try
		{
			DateOnly date = (DateOnly)window["DateBegin"];
			DateOnly date2 = (DateOnly)window["DateEnd"];
			ValidationService.ValidateWrongPeriod((DateOnly)window["DateBegin"], (DateOnly)window["DateEnd"]);
			object obj = window["PeriodType"];
			if (obj == null)
			{
				return;
			}
			DatePeriodTypeSE datePeriodTypeSE = SmartEnum<DatePeriodTypeSE, short>.FromValue(Convert.ToInt16(obj));
			if ((short)datePeriodTypeSE == DatePeriodTypeSE.Day.Value)
			{
				if (DateOnlyExtension.DateDiff(DateExtension.Interval.Day, date, date2) > 31)
				{
					args.isValid = false;
					args.ErrorContent = ArmSoft.SME.Common.Resources.InvalidDaysCount;
				}
			}
			else if ((short)datePeriodTypeSE == DatePeriodTypeSE.Month.Value)
			{
				if (DateOnlyExtension.DateDiff(DateExtension.Interval.Month, date, date2) > 36)
				{
					args.isValid = false;
					args.ErrorContent = ArmSoft.SME.Common.Resources.InvalidMonthsCount;
				}
			}
			else if ((short)datePeriodTypeSE == DatePeriodTypeSE.Week.Value && DateOnlyExtension.DateDiff(DateExtension.Interval.WeekOfYear, date, date2) > 36)
			{
				args.isValid = false;
				args.ErrorContent = ArmSoft.SME.Common.Resources.InvalidWeeksCount;
			}
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
			args.isValid = false;
		}
	}

	public static string GetGroupName(string group)
	{
		if (string.IsNullOrEmpty(group))
		{
			return "";
		}
		return TradeSettings.TradeCacheService.MTGroup.GetItem(group)?.Caption;
	}

	public static string GetStorageName(string storage)
	{
		if (string.IsNullOrEmpty(storage))
		{
			return "";
		}
		return TradeSettings.TradeCacheService.Storage.GetItem(storage)?.Caption;
	}
}
