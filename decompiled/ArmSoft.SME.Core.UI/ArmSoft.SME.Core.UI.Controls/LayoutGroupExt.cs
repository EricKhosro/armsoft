using System.Windows;
using System.Windows.Input;
using DevExpress.Xpf.Core.Native;
using DevExpress.Xpf.LayoutControl;

namespace ArmSoft.SME.Core.UI.Controls;

public class LayoutGroupExt : LayoutGroup
{
	public class GroupBoxExt : GroupBox
	{
		private FrameworkElement _titleElement;

		public override void OnApplyTemplate()
		{
			if (MaximizeElementVisibility == Visibility.Visible || MinimizeElementVisibility == Visibility.Visible)
			{
				if (_titleElement != null)
				{
					_titleElement.MouseLeftButtonDown -= OnTitleElementMouseLeftButtonDown;
				}
				base.OnApplyTemplate();
				_titleElement = LayoutHelper.FindParentObject<LayoutGroup>(GetTemplateChild("MaximizeElement"));
				if (_titleElement != null)
				{
					_titleElement.MouseLeftButtonDown += OnTitleElementMouseLeftButtonDown;
				}
			}
		}

		private void OnTitleElementMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			State = ((State == GroupBoxState.Normal) ? GroupBoxState.Minimized : GroupBoxState.Normal);
		}
	}

	protected override GroupBox CreateGroupBox()
	{
		return new GroupBoxExt();
	}
}
