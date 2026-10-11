using System;
using System.Collections.Generic;
using System.Text;

namespace ArmSoft.SME.Core.UI.Controls;

public class MultiSelectDropDownControlValidateEventArgs : EventArgs
{
	public bool Handled { get; set; }

	public bool IsValid { get; set; }

	public string ErrorMessage { get; set; }

	public string Value { get; set; }

	public string Comment { get; set; }

	public MultiSelectDropDownControlValidateEventArgs()
	{
		Handled = false;
		IsValid = true;
		ErrorMessage = string.Empty;
		Value = string.Empty;
		Comment = string.Empty;
	}

	public List<string> SplitValues()
	{
		List<string> list = new List<string>();
		string[] array = Value.Split(",");
		for (int i = 0; i <= array.Length - 1; i++)
		{
			string item = array[i].Trim();
			if (!string.IsNullOrWhiteSpace(Value))
			{
				list.Add(item);
			}
		}
		return list;
	}

	public static string StringRepresentation(List<string> values)
	{
		values.Sort();
		StringBuilder stringBuilder = new StringBuilder();
		bool flag = true;
		foreach (string value in values)
		{
			if (!flag)
			{
				stringBuilder.Append(',');
			}
			else
			{
				flag = false;
			}
			stringBuilder.Append(value.Trim());
		}
		return stringBuilder.ToString();
	}
}
