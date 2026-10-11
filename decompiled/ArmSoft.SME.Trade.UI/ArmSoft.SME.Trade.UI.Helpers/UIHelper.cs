using DevExpress.Xpf.Editors;

namespace ArmSoft.SME.Trade.UI.Helpers;

public class UIHelper
{
	public static string ConcatFieldNumericTextWithSymbolsLeftToRight(string sLeft, string sRight, int precisonLength)
	{
		int length = sLeft.Length;
		int length2 = sRight.Length;
		int num = length + length2;
		char[] array = new char[num - 1 + 1];
		for (int i = 0; i <= num - 1; i++)
		{
			if (i < length)
			{
				array[i] = sLeft[i];
			}
			else
			{
				array[i] = sRight[i - length];
			}
		}
		return RemoveRedundantSymbolsAfterDotOfTextWithPrecision(new string(array), precisonLength);
	}

	public static int GetLastAvailableIndexOfFieldAfterGivenSymbol(string fieldText, string symbol, int lengthAfterSymbol)
	{
		int num = fieldText.IndexOf(symbol);
		if (num == -1)
		{
			return -1;
		}
		return num + symbol.Length + lengthAfterSymbol;
	}

	public static string RemoveRedundantSymbolsOfTextAfterSymbol(string fieldText, string symbol, int lengthAfterSymbol)
	{
		int lastAvailableIndexOfFieldAfterGivenSymbol = GetLastAvailableIndexOfFieldAfterGivenSymbol(fieldText, symbol, lengthAfterSymbol);
		if (lastAvailableIndexOfFieldAfterGivenSymbol == -1)
		{
			return fieldText;
		}
		if (lastAvailableIndexOfFieldAfterGivenSymbol < fieldText.Length)
		{
			return fieldText.Remove(lastAvailableIndexOfFieldAfterGivenSymbol);
		}
		return fieldText;
	}

	public static string RemoveRedundantSymbolsAfterDotOfTextWithPrecision(string fieldText, int lengthAfterSymbol)
	{
		return RemoveRedundantSymbolsOfTextAfterSymbol(fieldText, ".", lengthAfterSymbol);
	}

	public static string RemoveRedundantCommaSymbolsOfText(string fieldText)
	{
		return fieldText.Replace(",", "");
	}

	public static void ForwardSymbolFromPadToTextEditPanel(PopupCalcEdit textEditControl, string value, short precision)
	{
		int num = textEditControl.CaretIndex;
		int selectionLength = textEditControl.SelectionLength;
		string text = textEditControl.Text;
		string displayText = textEditControl.DisplayText;
		int length = textEditControl.DisplayText.Length;
		if (selectionLength == 0)
		{
			if (length >= textEditControl.Mask.Length && (num < displayText.IndexOf(".") || (num == displayText.IndexOf(".") && value != ".")))
			{
				return;
			}
			if (num == length)
			{
				textEditControl.Text = ConcatFieldNumericTextWithSymbolsLeftToRight(text, value, precision);
			}
			else if (num == 0)
			{
				textEditControl.Text = ConcatFieldNumericTextWithSymbolsLeftToRight(value, text, precision);
			}
			else
			{
				if (num > length)
				{
					num = length;
				}
				string fieldText = displayText.Substring(0, num);
				string fieldText2 = displayText.Substring(num);
				fieldText = RemoveRedundantCommaSymbolsOfText(fieldText);
				fieldText2 = RemoveRedundantCommaSymbolsOfText(fieldText2);
				textEditControl.Text = ConcatFieldNumericTextWithSymbolsLeftToRight(ConcatFieldNumericTextWithSymbolsLeftToRight(fieldText, value, precision), fieldText2, precision);
			}
		}
		else
		{
			if (selectionLength < value.Length)
			{
				return;
			}
			textEditControl.SelectedText = value;
			if (selectionLength > 0)
			{
				textEditControl.SelectionLength = 0;
			}
		}
		if (textEditControl.DisplayText[1] == ',' && textEditControl.DisplayText.Length > length)
		{
			num++;
		}
		textEditControl.CaretIndex = num + value.Length;
	}
}
