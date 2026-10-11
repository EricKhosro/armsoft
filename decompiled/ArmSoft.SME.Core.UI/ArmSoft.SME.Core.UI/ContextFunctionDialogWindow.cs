using System;
using ArmSoft.SME.Core.UI.MetaData;
using DevExpress.Xpf.LayoutControl;

namespace ArmSoft.SME.Core.UI;

public class ContextFunctionDialogWindow : DialogWindow
{
	public bool Invoke(ContextFunctionDialog cfDialog)
	{
		Caption = cfDialog.Title;
		foreach (object element in cfDialog.Elements)
		{
			if (element is ContextFunctionDialog.BooleanControl booleanControl)
			{
				AddBooleanControl(booleanControl.Name, booleanControl.Caption).IsChecked = booleanControl.Value;
			}
			else if (element is ContextFunctionDialog.StringControl stringControl)
			{
				AddStringControl(stringControl.Name, stringControl.Caption, stringControl.Length, stringControl.IsRequired).EditValue = stringControl.Value;
			}
			else if (element is ContextFunctionDialog.DateControl dateControl)
			{
				AddDateControl(dateControl.Name, dateControl.Caption, dateControl.UseLongDate, dateControl.IsRequired).EditValue = dateControl.Value;
			}
			else if (element is ContextFunctionDialog.NumericControl numericControl)
			{
				AddNumericControl(numericControl.Name, numericControl.Caption, numericControl.Length, numericControl.Precision, numericControl.IsRequired, numericControl.AllowNegativeNumbers, numericControl.UseCurrencyFormat).EditValue = numericControl.Value;
			}
			else if (element is ContextFunctionDialogLayoutGroup contextFunctionDialogLayoutGroup)
			{
				LayoutGroup lg = ((!contextFunctionDialogLayoutGroup.IsHorizontal) ? AddVerticalLayoutGroup() : AddHorizontalLayoutGroup());
				AddControlsInner(lg, contextFunctionDialogLayoutGroup);
			}
		}
		bool? flag = ShowDialog();
		if (flag.HasValue)
		{
			if (flag.Value)
			{
				foreach (string key in cfDialog.Controls.Keys)
				{
					object obj = base[key];
					object obj2 = cfDialog.Controls[key];
					if (obj2 is ContextFunctionDialog.BooleanControl booleanControl2)
					{
						booleanControl2.Value = (bool)obj;
					}
					else if (obj2 is ContextFunctionDialog.StringControl stringControl2)
					{
						stringControl2.Value = (string)obj;
					}
					else if (obj2 is ContextFunctionDialog.DateControl dateControl2)
					{
						dateControl2.Value = (DateOnly?)obj;
					}
					else if (obj2 is ContextFunctionDialog.NumericControl numericControl2)
					{
						numericControl2.Value = (decimal)obj;
					}
				}
			}
			return flag.Value;
		}
		return false;
	}

	private void AddControlsInner(LayoutGroup lg, ContextFunctionDialogLayoutGroup layoutGroup)
	{
		foreach (object element in layoutGroup.Elements)
		{
			if (element is ContextFunctionDialog.BooleanControl booleanControl)
			{
				AddBooleanControl(lg, booleanControl.Name, booleanControl.Caption);
			}
			else if (element is ContextFunctionDialog.StringControl stringControl)
			{
				AddStringControl(lg, stringControl.Name, stringControl.Caption, stringControl.Length, stringControl.IsRequired);
			}
			else if (element is ContextFunctionDialog.DateControl dateControl)
			{
				AddDateControl(lg, dateControl.Name, dateControl.Caption, dateControl.UseLongDate, dateControl.IsRequired);
			}
			else if (element is ContextFunctionDialog.NumericControl numericControl)
			{
				AddNumericControl(lg, numericControl.Name, numericControl.Caption, numericControl.Length, numericControl.Precision, numericControl.IsRequired, numericControl.AllowNegativeNumbers, numericControl.UseCurrencyFormat);
			}
			else if (element is ContextFunctionDialogLayoutGroup layoutGroup2)
			{
				LayoutGroup lg2 = ((!layoutGroup.IsHorizontal) ? AddVerticalLayoutGroup(lg) : AddHorizontalLayoutGroup(lg));
				AddControlsInner(lg2, layoutGroup2);
			}
		}
	}
}
