using System.Collections.Generic;

namespace ArmSoft.SME.Core.UI.MetaData;

public class ContextFunctionDialogLayoutGroup
{
	public bool IsHorizontal { get; set; }

	public List<object> Elements { get; }

	public Dictionary<string, object> Controls { get; }

	internal ContextFunctionDialogLayoutGroup(Dictionary<string, object> controls)
	{
		Controls = controls;
		Elements = new List<object>();
	}

	public ContextFunctionDialogLayoutGroup AddLayoutGroup(bool isHorizontal)
	{
		ContextFunctionDialogLayoutGroup contextFunctionDialogLayoutGroup = new ContextFunctionDialogLayoutGroup(Controls)
		{
			IsHorizontal = isHorizontal
		};
		Elements.Add(contextFunctionDialogLayoutGroup);
		return contextFunctionDialogLayoutGroup;
	}

	public ContextFunctionDialog.BooleanControl AddBooleanControl(string name, string caption)
	{
		ContextFunctionDialog.BooleanControl booleanControl = new ContextFunctionDialog.BooleanControl
		{
			Name = name,
			Caption = caption
		};
		Controls.Add(name, booleanControl);
		Elements.Add(booleanControl);
		return booleanControl;
	}

	public ContextFunctionDialog.StringControl AddStringControl(string name, string caption, short length, bool isRequired = false)
	{
		ContextFunctionDialog.StringControl stringControl = new ContextFunctionDialog.StringControl
		{
			Name = name,
			Caption = caption,
			Length = length,
			IsRequired = isRequired
		};
		Controls.Add(name, stringControl);
		Elements.Add(stringControl);
		return stringControl;
	}

	public ContextFunctionDialog.DateControl AddDateControl(string name, string caption, bool useLongDate = false, bool isRequired = false)
	{
		ContextFunctionDialog.DateControl dateControl = new ContextFunctionDialog.DateControl
		{
			Name = name,
			Caption = caption,
			UseLongDate = useLongDate,
			IsRequired = isRequired
		};
		Controls.Add(name, dateControl);
		Elements.Add(dateControl);
		return dateControl;
	}

	public ContextFunctionDialog.NumericControl AddNumericControl(string name, string caption, short length, short precision = 0, bool isRequired = false, bool allowNegativeNumbers = false, bool useCurrencyFormat = true)
	{
		ContextFunctionDialog.NumericControl numericControl = new ContextFunctionDialog.NumericControl
		{
			Name = name,
			Caption = caption,
			Length = length,
			Precision = precision,
			IsRequired = isRequired,
			AllowNegativeNumbers = allowNegativeNumbers,
			UseCurrencyFormat = useCurrencyFormat
		};
		Controls.Add(name, numericControl);
		Elements.Add(numericControl);
		return numericControl;
	}
}
