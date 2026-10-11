using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Media;

namespace ArmSoft.SME.Core.UI.Report;

public class AsRepColumn
{
	public Dictionary<string, object> StyleCollection = new Dictionary<string, object>();

	public string Caption { get; set; }

	public double WidthD { get; set; }

	public TextAlignment DataAlignment
	{
		get
		{
			if (StyleCollection.TryGetValue("DataAlignment", out var value))
			{
				return (TextAlignment)value;
			}
			return TextAlignment.Left;
		}
		set
		{
			if (StyleCollection.ContainsKey("DataAlignment"))
			{
				StyleCollection["DataAlignment"] = value;
			}
			else
			{
				StyleCollection.Add("DataAlignment", value);
			}
		}
	}

	public TextAlignment CaptionAlignment
	{
		get
		{
			if (StyleCollection.TryGetValue("CaptionAlignment", out var value))
			{
				return (TextAlignment)value;
			}
			return TextAlignment.Left;
		}
		set
		{
			if (StyleCollection.ContainsKey("CaptionAlignment"))
			{
				StyleCollection["CaptionAlignment"] = value;
			}
			else
			{
				StyleCollection.Add("CaptionAlignment", value);
			}
		}
	}

	public string XlsxFormatString { get; set; }

	public int DataFontSize
	{
		get
		{
			if (StyleCollection.TryGetValue("DataFontSize", out var value))
			{
				return Convert.ToInt32(value);
			}
			return 0;
		}
		set
		{
			if (StyleCollection.ContainsKey("DataFontSize"))
			{
				StyleCollection["DataFontSize"] = value;
			}
			else
			{
				StyleCollection.Add("DataFontSize", value);
			}
		}
	}

	public int CaptionFontSize
	{
		get
		{
			if (StyleCollection.TryGetValue("CaptionFontSize", out var value))
			{
				return Convert.ToInt32(value);
			}
			return 0;
		}
		set
		{
			if (StyleCollection.ContainsKey("CaptionFontSize"))
			{
				StyleCollection["CaptionFontSize"] = value;
			}
			else
			{
				StyleCollection.Add("CaptionFontSize", value);
			}
		}
	}

	public FontStyle DataFontStyle
	{
		get
		{
			if (StyleCollection.TryGetValue("DataFontStyle", out var value))
			{
				return (FontStyle)value;
			}
			return default;
		}
		set
		{
			if (StyleCollection.ContainsKey("DataFontStyle"))
			{
				StyleCollection["DataFontStyle"] = value;
			}
			else
			{
				StyleCollection.Add("DataFontStyle", value);
			}
		}
	}

	public FontStyle CaptionFontStyle
	{
		get
		{
			if (StyleCollection.TryGetValue("CaptionFontStyle", out var value))
			{
				return (FontStyle)value;
			}
			return default;
		}
		set
		{
			if (StyleCollection.ContainsKey("CaptionFontStyle"))
			{
				StyleCollection["CaptionFontStyle"] = value;
			}
			else
			{
				StyleCollection.Add("CaptionFontStyle", value);
			}
		}
	}

	public FontWeight DataFontWeight
	{
		get
		{
			if (StyleCollection.TryGetValue("DataFontWeight", out var value))
			{
				return (FontWeight)value;
			}
			return default;
		}
		set
		{
			if (StyleCollection.ContainsKey("DataFontWeight"))
			{
				StyleCollection["DataFontWeight"] = value;
			}
			else
			{
				StyleCollection.Add("DataFontWeight", value);
			}
		}
	}

	public FontWeight CaptionFontWeight
	{
		get
		{
			if (StyleCollection.TryGetValue("CaptionFontWeight", out var value))
			{
				return (FontWeight)value;
			}
			return default;
		}
		set
		{
			if (StyleCollection.ContainsKey("CaptionFontWeight"))
			{
				StyleCollection["CaptionFontWeight"] = value;
			}
			else
			{
				StyleCollection.Add("CaptionFontWeight", value);
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

	public ObservableCollection<BorderType> DataBorders
	{
		get
		{
			if (StyleCollection.TryGetValue("DataBorders", out var value))
			{
				return value as ObservableCollection<BorderType>;
			}
			return null;
		}
		set
		{
			if (StyleCollection.ContainsKey("DataBorders"))
			{
				StyleCollection["DataBorders"] = value;
			}
			else
			{
				StyleCollection.Add("DataBorders", value);
			}
		}
	}

	public ObservableCollection<BorderType> CaptionBorders
	{
		get
		{
			if (StyleCollection.TryGetValue("CaptionBorders", out var value))
			{
				return value as ObservableCollection<BorderType>;
			}
			return null;
		}
		set
		{
			if (StyleCollection.ContainsKey("CaptionBorders"))
			{
				StyleCollection["CaptionBorders"] = value;
			}
			else
			{
				StyleCollection.Add("CaptionBorders", value);
			}
		}
	}

	public AsRepColStyle ValuesStyle
	{
		get
		{
			if (StyleCollection.TryGetValue("ValuesStyle", out var value))
			{
				return (AsRepColStyle)Convert.ToInt32(value);
			}
			return AsRepColStyle.TextStyle;
		}
		set
		{
			if (StyleCollection.ContainsKey("ValuesStyle"))
			{
				StyleCollection["ValuesStyle"] = value;
			}
			else
			{
				StyleCollection.Add("ValuesStyle", value);
			}
		}
	}

	public Color DataBackgroundColor
	{
		get
		{
			if (StyleCollection.TryGetValue("DataBackgroundColor", out var value))
			{
				return (Color)value;
			}
			return default;
		}
		set
		{
			if (StyleCollection.ContainsKey("DataBackgroundColor"))
			{
				StyleCollection["DataBackgroundColor"] = value;
			}
			else
			{
				StyleCollection.Add("DataBackgroundColor", value);
			}
		}
	}

	public Color DataForegroundColor
	{
		get
		{
			if (StyleCollection.TryGetValue("DataForegroundColor", out var value))
			{
				return (Color)value;
			}
			return default;
		}
		set
		{
			if (StyleCollection.ContainsKey("DataForegroundColor"))
			{
				StyleCollection["DataForegroundColor"] = value;
			}
			else
			{
				StyleCollection.Add("DataForegroundColor", value);
			}
		}
	}

	public Color CaptionBackgroundColor
	{
		get
		{
			if (StyleCollection.TryGetValue("CaptionBackgroundColor", out var value))
			{
				return (Color)value;
			}
			return default;
		}
		set
		{
			if (StyleCollection.ContainsKey("CaptionBackgroundColor"))
			{
				StyleCollection["CaptionBackgroundColor"] = value;
			}
			else
			{
				StyleCollection.Add("CaptionBackgroundColor", value);
			}
		}
	}

	public Color CaptionForegroundColor
	{
		get
		{
			if (StyleCollection.TryGetValue("CaptionForegroundColor", out var value))
			{
				return (Color)value;
			}
			return default;
		}
		set
		{
			if (StyleCollection.ContainsKey("CaptionForegroundColor"))
			{
				StyleCollection["CaptionForegroundColor"] = value;
			}
			else
			{
				StyleCollection.Add("CaptionForegroundColor", value);
			}
		}
	}

	public AsRepColumn()
	{
	}

	public AsRepColumn(double width)
	{
		WidthD = width;
	}

	public AsRepColumn(string caption, double width)
	{
		WidthD = width;
		Caption = caption;
	}
}
