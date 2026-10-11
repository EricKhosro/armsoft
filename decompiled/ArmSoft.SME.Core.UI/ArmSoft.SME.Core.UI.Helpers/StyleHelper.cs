using System.Windows;
using System.Windows.Documents;
using DevExpress.Xpf.Grid;

namespace ArmSoft.SME.Core.UI.Helpers;

public static class StyleHelper
{
	public static Style GetTableRowWithSelectionStyle()
	{
		Style style = new Style(typeof(RowControl));
		Trigger trigger = new Trigger
		{
			Property = RowControl.SelectionStateProperty,
			Value = SelectionState.Focused
		};
		trigger.Setters.Add(new Setter(TextElement.FontWeightProperty, FontWeights.Bold));
		style.Triggers.Add(trigger);
		Trigger trigger2 = new Trigger
		{
			Property = RowControl.SelectionStateProperty,
			Value = SelectionState.Selected
		};
		trigger2.Setters.Add(new Setter(TextElement.FontWeightProperty, FontWeights.Bold));
		style.Triggers.Add(trigger2);
		Trigger trigger3 = new Trigger
		{
			Property = RowControl.SelectionStateProperty,
			Value = SelectionState.FocusedAndSelected
		};
		trigger3.Setters.Add(new Setter(TextElement.FontWeightProperty, FontWeights.Bold));
		style.Triggers.Add(trigger3);
		return style;
	}
}
