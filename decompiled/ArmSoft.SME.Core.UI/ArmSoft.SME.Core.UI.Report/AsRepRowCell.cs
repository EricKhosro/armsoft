using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Media;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Extensions;

namespace ArmSoft.SME.Core.UI.Report;

public class AsRepRowCell
{
	public Dictionary<string, object> StyleCollection = new Dictionary<string, object>();

	public string Name { get; set; }

	public object Value { get; set; }

	public double WidthD { get; set; }

	public string XlsxFormatString { get; set; }

	public bool HasCustomForeGround { get; set; }

	public Color BackGround
	{
		get
		{
			if (StyleCollection.TryGetValue("BackgroundColor", out var value))
			{
				return (Color)value;
			}
			return default;
		}
		set
		{
			if (StyleCollection.ContainsKey("BackgroundColor"))
			{
				StyleCollection["BackgroundColor"] = value;
			}
			else
			{
				StyleCollection.Add("BackgroundColor", value);
			}
		}
	}

	public string ValueToString
	{
		get
		{
			switch (ValueStyle)
			{
			case AsRepColStyle.BooleanStyle:
				if (Value.ToString() == "0" || Value.ToString() == "1" || Value.ToString() == "-1" || Value.ToString() == "True" || Value.ToString() == "False")
				{
					if (Convert.ToBoolean(Value))
					{
						return Resources.Yes;
					}
					return Resources.No;
				}
				return Value.ToString();
			case AsRepColStyle.DateStyle:
				if (Value is DateTime)
				{
					return Convert.ToDateTime(Value).FormatDDMMYY();
				}
				return Value.ToString();
			case AsRepColStyle.CurrencyStyle:
			case AsRepColStyle.NumericStyle:
				if (Value.ToString().IsNumeric())
				{
					Alignment = TextAlignment.Right;
					return Convert.ToDecimal(Value).CurrencyFormat(CurrencyLen, CurrencyPrecision);
				}
				return Value.ToString();
			default:
				return Value.ToString();
			}
		}
	}

	public Color ForeGround
	{
		get
		{
			if (StyleCollection.TryGetValue("ForeGroundColor", out var value))
			{
				return (Color)value;
			}
			return default;
		}
		set
		{
			if (StyleCollection.ContainsKey("ForeGroundColor"))
			{
				StyleCollection["ForeGroundColor"] = value;
			}
			else
			{
				StyleCollection.Add("ForeGroundColor", value);
			}
			HasCustomForeGround = true;
		}
	}

	public Color UserBackGround
	{
		get
		{
			if (StyleCollection.TryGetValue("UserBackGround", out var value))
			{
				return (Color)value;
			}
			return default;
		}
		set
		{
			if (StyleCollection.ContainsKey("UserBackGround"))
			{
				StyleCollection["UserBackGround"] = value;
			}
			else
			{
				StyleCollection.Add("UserBackGround", value);
			}
		}
	}

	public short CurrencyLen
	{
		get
		{
			if (StyleCollection.TryGetValue("CurrencyLen", out var value))
			{
				return Convert.ToInt16(value);
			}
			return 0;
		}
		set
		{
			if (StyleCollection.ContainsKey("CurrencyLen"))
			{
				StyleCollection["CurrencyLen"] = value;
			}
			else
			{
				StyleCollection.Add("CurrencyLen", value);
			}
		}
	}

	public short CurrencyPrecision
	{
		get
		{
			if (StyleCollection.TryGetValue("CurrencyPrecision", out var value))
			{
				return Convert.ToInt16(value);
			}
			return 0;
		}
		set
		{
			if (StyleCollection.ContainsKey("CurrencyPrecision"))
			{
				StyleCollection["CurrencyPrecision"] = value;
			}
			else
			{
				StyleCollection.Add("CurrencyPrecision", value);
			}
		}
	}

	public TextAlignment Alignment
	{
		get
		{
			if (StyleCollection.TryGetValue("Alignment", out var value))
			{
				return (TextAlignment)value;
			}
			return TextAlignment.Left;
		}
		set
		{
			if (StyleCollection.ContainsKey("Alignment"))
			{
				StyleCollection["Alignment"] = value;
			}
			else
			{
				StyleCollection.Add("Alignment", value);
			}
		}
	}

	public AsRepRowStyle CellType { get; set; } = AsRepRowStyle.CaptionRow;

	public AsRepColStyle ValueStyle { get; set; }

	public int FontSize
	{
		get
		{
			if (StyleCollection.TryGetValue("FontSize", out var value))
			{
				return Convert.ToInt32(value);
			}
			return 0;
		}
		set
		{
			if (StyleCollection.ContainsKey("FontSize"))
			{
				StyleCollection["FontSize"] = value;
			}
			else
			{
				StyleCollection.Add("FontSize", value);
			}
		}
	}

	public FontStyle FontStyle
	{
		get
		{
			if (StyleCollection.TryGetValue("FontStyle", out var value))
			{
				return (FontStyle)value;
			}
			return default;
		}
		set
		{
			if (StyleCollection.ContainsKey("FontStyle"))
			{
				StyleCollection["FontStyle"] = value;
			}
			else
			{
				StyleCollection.Add("FontStyle", value);
			}
		}
	}

	public FontWeight FontWeight
	{
		get
		{
			if (StyleCollection.TryGetValue("FontWeight", out var value))
			{
				return (FontWeight)value;
			}
			return default;
		}
		set
		{
			if (StyleCollection.ContainsKey("FontWeight"))
			{
				StyleCollection["FontWeight"] = value;
			}
			else
			{
				StyleCollection.Add("FontWeight", value);
			}
		}
	}

	public ObservableCollection<BorderType> Borders
	{
		get
		{
			if (StyleCollection.TryGetValue("Borders", out var value))
			{
				return value as ObservableCollection<BorderType>;
			}
			return null;
		}
		set
		{
			if (StyleCollection.ContainsKey("Borders"))
			{
				StyleCollection["Borders"] = value;
			}
			else
			{
				StyleCollection.Add("Borders", value);
			}
		}
	}

	public int ColumnSpan { get; set; } = 1;

	public AsRepRowCell()
	{
		Borders = new ObservableCollection<BorderType>();
	}

	public AsRepRowCell(string text, double width)
	{
		Value = text;
		WidthD = width;
		Borders = new ObservableCollection<BorderType>();
	}
}
