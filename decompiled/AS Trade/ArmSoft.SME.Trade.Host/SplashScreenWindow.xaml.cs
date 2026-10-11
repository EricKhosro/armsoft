using System;
using System.Windows.Markup;
using ArmSoft.SME.Common;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Core.UI.Helpers;
using DevExpress.Xpf.Core;

namespace ArmSoft.SME.Trade.Host;

public partial class SplashScreenWindow : DevExpress.Xpf.Core.SplashScreenWindow, IComponentConnector
{
	public SplashScreenWindow()
	{
		InitializeComponent();
		AppInfo.Text = ArmSoft.SME.Common.Resources.Version + " " + Settings.AppVersionString;
		DateTime now = DateTime.Now;
		int num = now.Year - 1987;
		if (now.Month < 5)
		{
			num--;
		}
		YearNumber.Text = num.ToString();
		ASFontHelper.SetFontSettings(this);
	}
}
