using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows;

namespace ArmSoft.SME.Core.UI.Report;

public class AsRepRow
{
	public Dictionary<string, object> StyleCollection = new Dictionary<string, object>();

	public AsRepRowType RowType { get; set; }

	public string BlockName { get; set; }

	public bool AllBorders { get; set; }

	public AsRepRowStyle RowStyle { get; set; }

	public List<AsRepRowCell> RowCells { get; set; }

	public string RowText { get; set; }

	public object HiddenData { get; set; }

	public bool Rendered { get; set; }

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

	public bool IsTableCaption { get; set; }

	public AsRepRow(AsRepRowType type, AsRepRowStyle style = AsRepRowStyle.TableRow, bool showAllBorders = true)
	{
		RowType = type;
		RowStyle = style;
		AllBorders = showAllBorders;
	}

	public double GetHeight()
	{
		switch (RowStyle)
		{
		case AsRepRowStyle.CaptionRow:
			return 30.0;
		case AsRepRowStyle.StringRow:
		case AsRepRowStyle.SubCaptionRow:
		case AsRepRowStyle.TableCaption:
		case AsRepRowStyle.TableColumnsHeader:
		case AsRepRowStyle.TableRow:
			return 20.0;
		default:
			return 20.0;
		}
	}

	public int FindText(string pattern, bool ignoreCase)
	{
		if (pattern == null || string.IsNullOrEmpty(pattern))
		{
			return -1;
		}
		for (int i = 0; i <= RowCells.Count - 1; i++)
		{
			if (Regex.Matches(RowCells[i].Value.ToString(), pattern, ignoreCase ? RegexOptions.IgnoreCase : RegexOptions.None).Count > 0)
			{
				return i;
			}
			if (RowCells[i].ValueStyle != AsRepColStyle.TextStyle && Regex.Matches(RowCells[i].ValueToString, pattern, ignoreCase ? RegexOptions.IgnoreCase : RegexOptions.None).Count > 0)
			{
				return i;
			}
		}
		return -1;
	}
}
