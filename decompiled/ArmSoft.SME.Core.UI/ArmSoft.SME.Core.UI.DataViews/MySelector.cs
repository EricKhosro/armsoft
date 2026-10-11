using System.Windows;
using System.Windows.Controls;
using DevExpress.Xpf.Grid;

namespace ArmSoft.SME.Core.UI.DataViews;

public class MySelector : DataTemplateSelector
{
	public DataTemplate EmptyTemplate { get; set; }

	public override DataTemplate SelectTemplate(object item, DependencyObject container)
	{
		if (((EditGridCellData)item).Column.FieldName == "Selected")
		{
			return EmptyTemplate;
		}
		return base.SelectTemplate(item, container);
	}
}
