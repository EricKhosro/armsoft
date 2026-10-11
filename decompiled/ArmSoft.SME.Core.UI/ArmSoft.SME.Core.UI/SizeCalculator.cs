using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using ArmSoft.SME.Common.Extensions;
using DevExpress.Xpf.Core;

namespace ArmSoft.SME.Core.UI;

public class SizeCalculator
{
	public static double WidthForString(int length)
	{
		double num = length switch
		{
			1 => 3.6, 
			2 => 4.5, 
			3 => 5.6, 
			4 => 6.5, 
			5 => 7.6, 
			6 => 8.7, 
			7 => 9.8, 
			8 => 10.8, 
			9 => 11.8, 
			10 => 12.8, 
			11 => 13.8, 
			12 => 14.8, 
			13 => 15.8, 
			14 => 16.8, 
			15 => 17.8, 
			16 => 18.8, 
			17 => 19.8, 
			18 => 20.8, 
			19 => 21.8, 
			20 => 22.8, 
			21 => 23.8, 
			22 => 24.8, 
			23 => 25.8, 
			24 => 26.8, 
			25 => 27.8, 
			_ => 28.8, 
		};
		int num2 = Convert.ToInt32(Settings.GetFontSize());
		if (Settings.IsWin11Theme)
		{
			num *= 1.2;
		}
		switch (num2)
		{
		case 10:
		case 11:
			return (double)num2 * num;
		case 12:
			return (double)num2 * num * 0.92;
		case 13:
			return (double)num2 * num * 0.85;
		default:
			return (double)num2 * num * 0.8;
		}
	}

	public static double WidthForNumber(int length)
	{
		double num;
		if (ApplicationThemeHelper.ApplicationThemeName.InList(LightweightTheme.Office2019ColorfulSpruceLeaves.Name, LightweightTheme.Office2019BlackSpruceLeaves.Name))
		{
			switch (length)
			{
			case 1:
			case 3:
			case 4:
			case 5:
			case 6:
			case 7:
			case 8:
			case 9:
				num = 0.4;
				break;
			case 2:
				num = 0.6;
				break;
			case 10:
			case 11:
			case 12:
				num = 0.5;
				break;
			case 13:
			case 14:
				num = 0.8;
				break;
			default:
				num = 0.9;
				break;
			}
		}
		else
		{
			num = 0.0;
		}
		double num2 = length switch
		{
			1 => 5.0 + num, 
			2 => 5.5 + num, 
			3 => 6.0 + num, 
			4 => 7.0 + num, 
			5 => 7.5 + num, 
			6 => 8.3 + num, 
			7 => 9.0 + num, 
			8 => 9.7 + num, 
			9 => 10.4 + num, 
			10 => 11.0 + num, 
			11 => 11.6 + num, 
			12 => 12.2 + num, 
			13 => 12.8 + num, 
			14 => 13.4 + num, 
			15 => 14.0 + num, 
			16 => 14.6 + num, 
			17 => 15.2 + num, 
			18 => 15.8 + num, 
			19 => 16.4 + num, 
			20 => 17.0 + num, 
			_ => 17.6 + num, 
		};
		int num3 = Convert.ToInt32(Settings.GetFontSize());
		switch (num3)
		{
		case 10:
		case 11:
			return (double)num3 * num2;
		case 12:
			return (double)num3 * num2 * 0.92;
		case 13:
			return (double)num3 * num2 * 0.85;
		default:
			return (double)num3 * num2 * 0.8;
		}
	}

	public static double WidthForDate(bool useLongDate)
	{
		int num = Convert.ToInt32(Settings.GetFontSize());
		if (useLongDate)
		{
			return num switch
			{
				10 => 96.0, 
				11 => 102.0, 
				12 => 105.0, 
				13 => 114.0, 
				_ => 120.0, 
			};
		}
		if (Settings.IsWin11Theme)
		{
			return num switch
			{
				10 => 80.0, 
				11 => 86.0, 
				12 => 92.0, 
				13 => 95.0, 
				_ => 104.0, 
			};
		}
		return num switch
		{
			10 => 70.0, 
			11 => 72.0, 
			12 => 74.0, 
			13 => 76.0, 
			_ => 80.0, 
		};
	}

	public static double WidthForTime(bool showSeconds)
	{
		int num = Convert.ToInt32(Settings.GetFontSize());
		if (showSeconds)
		{
			return num switch
			{
				10 => 73.0, 
				11 => 75.0, 
				12 => 77.0, 
				13 => 80.0, 
				_ => 82.0, 
			};
		}
		return num switch
		{
			10 => 58.0, 
			11 => 60.0, 
			12 => 62.0, 
			13 => 65.0, 
			_ => 68.0, 
		};
	}

	public static double Width(double value)
	{
		switch (Convert.ToInt32(Settings.GetFontSize()))
		{
		case 10:
		case 11:
			return value;
		case 12:
			return value * 1.03;
		case 13:
			return value * 1.06;
		default:
			return value * 1.09;
		}
	}

	public static double Height(double value)
	{
		switch (Convert.ToInt32(Settings.GetFontSize()))
		{
		case 10:
		case 11:
			return value;
		case 12:
			return value * 1.03;
		case 13:
			return value * 1.06;
		default:
			return value * 1.09;
		}
	}

	public static double WidthForMonthYear()
	{
		int num = Convert.ToInt32(Settings.GetFontSize());
		int num2 = 6;
		double num3 = (Settings.IsWin11Theme ? 1.12 : 1.0);
		return num switch
		{
			10 => num3 * 125.0 + (double)num2, 
			11 => num3 * 130.0 + (double)num2, 
			12 => num3 * 135.0 + (double)num2, 
			13 => num3 * 142.0 + (double)num2, 
			_ => num3 * 150.0 + (double)num2, 
		};
	}

	public static void ConfigureLine(Window window, Rectangle line)
	{
		line.SnapsToDevicePixels = true;
		line.Margin = new Thickness(0.0, 3.0, 0.0, 3.0);
		line.Height = 1.0;
		line.Stroke = (Settings.IsDarkTheme ? ((SolidColorBrush)new BrushConverter().ConvertFrom("#606862")) : ((SolidColorBrush)new BrushConverter().ConvertFrom("#ababab")));
		line.Fill = line.Stroke;
	}
}
