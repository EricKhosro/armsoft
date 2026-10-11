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

public partial class AnalysisReportsView : ModuleView, IComponentConnector
{
	public AnalysisReportsView()
	{
		InitializeComponent();
		AnalysisReportsViewPage.SubSystem = CommonConst.SubSystemTypes.AnalysisReports;
		AnalysisReportsPageTitle.Content = ArmSoft.SME.Common.Resources.AnalysisCaption;
		if (!Settings.LoginService.IsAdmin && Settings.UserAccessService.GetAccessibleSubsystems().Count == 1)
		{
			AnalysisReportsPageTitle.Glyph = null;
		}
		AnalysisReportsPageTitle.VerticalAlignment = VerticalAlignment.Center;
		AnalysisReportsPageTitle.Margin = new Thickness(5.0, 0.0, 0.0, 0.0);
		AnalysisReportsPageTitle.FontSize = 17.0;
		Background = Settings.GetBackgroundBrush();
		foreach (TileBarItem item in BaseView.SystemsUIList(CommonConst.SubSystemTypes.AnalysisReports))
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
