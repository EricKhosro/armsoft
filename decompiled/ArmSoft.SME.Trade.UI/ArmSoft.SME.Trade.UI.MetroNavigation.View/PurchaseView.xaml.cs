using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Windows;
using System.Windows.Markup;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Core.UI.Helpers;
using ArmSoft.SME.Trade.Common;
using DevExpress.Xpf.Core.MvvmSample;
using DevExpress.Xpf.Navigation;

namespace ArmSoft.SME.Trade.UI.MetroNavigation.View;

public partial class PurchaseView : ModuleView, IComponentConnector
{
	public PurchaseView()
	{
		InitializeComponent();
		PurchasePage.SubSystem = CommonConst.SubSystemTypes.PurchasesSS;
		PurchasePageTitle.Content = ArmSoft.SME.Trade.Common.Resources.Purchases;
		if (!Settings.LoginService.IsAdmin && Settings.UserAccessService.GetAccessibleSubsystems().Count == 1)
		{
			PurchasePageTitle.Glyph = null;
		}
		PurchasePageTitle.VerticalAlignment = VerticalAlignment.Center;
		PurchasePageTitle.Margin = new Thickness(5.0, 0.0, 0.0, 0.0);
		PurchasePageTitle.FontSize = 17.0;
		Background = Settings.GetBackgroundBrush();
		foreach (TileBarItem item in BaseView.SystemsUIList(CommonConst.SubSystemTypes.PurchasesSS))
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
