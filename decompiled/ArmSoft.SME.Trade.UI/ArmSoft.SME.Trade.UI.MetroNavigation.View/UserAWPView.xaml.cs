using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Windows;
using System.Windows.Markup;
using ArmSoft.SME.Common;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Core.UI.Helpers;
using ArmSoft.SME.Trade.Common;
using DevExpress.Xpf.Core.MvvmSample;
using DevExpress.Xpf.Navigation;

namespace ArmSoft.SME.Trade.UI.MetroNavigation.View;

public partial class UserAWPView : ModuleView, IComponentConnector
{
	public UserAWPView()
	{
		InitializeComponent();
		UserAWPPage.SubSystem = CommonConst.SubSystemTypes.UserAWP;
		UserAWPPageTitle.Content = ArmSoft.SME.Common.Resources.UserAWP;
		if (!Settings.LoginService.IsAdmin && Settings.UserAccessService.GetAccessibleSubsystems().Count == 1)
		{
			UserAWPPageTitle.Glyph = null;
		}
		UserAWPPageTitle.VerticalAlignment = VerticalAlignment.Center;
		UserAWPPageTitle.Margin = new Thickness(5.0, 0.0, 0.0, 0.0);
		UserAWPPageTitle.FontSize = 17.0;
		Background = Settings.GetBackgroundBrush();
		foreach (TileBarItem item in BaseView.SystemsUIList(CommonConst.SubSystemTypes.UserAWP))
		{
			TileView.TileBar.Items.Add(item);
		}
		ASFontHelper.SetFontSettings(this);
	}

	[DebuggerNonUserCode]
	[GeneratedCode("PresentationBuildTasks", "10.0.9.0")]
	internal Delegate _CreateDelegate(Type delegateType, string handler)
	{
		return Delegate.CreateDelegate(delegateType, this, handler);
	}
}
