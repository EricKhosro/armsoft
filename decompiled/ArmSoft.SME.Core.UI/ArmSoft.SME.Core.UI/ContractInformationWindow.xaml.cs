using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Windows;
using System.Windows.Markup;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Extensions;
using ArmSoft.SME.Core.UI.Helpers;
using DevExpress.Xpf.Core;

namespace ArmSoft.SME.Core.UI;

public partial class ContractInformationWindow : ThemedWindow, IComponentConnector
{
	public ContractInformationWindowDelegate CreatePayOrderDelegate { get; }

	public ContractInformationWindow(int dayRemains, decimal nextPayAmount, ContractInformationWindowDelegate createPayOrderDelegate = null)
	{
		InitializeComponent();
		Owner = Settings.HostingEnvironment;
		if (!Settings.IsWin11Theme)
		{
			WindowStyle = WindowStyle.ToolWindow;
		}
		TBLicense.Text = Settings.LoginService.Key;
		TBExpirationDate.Text = (Settings.LoginService.ContractExpirationDate.HasValue ? Settings.LoginService.ContractExpirationDate.ToDateOnly().FormatDDMMYY() : "");
		TBReNewalAmount.Text = nextPayAmount.CurrencyFormat(16, 2) + " " + ArmSoft.SME.Common.Resources.DramWordSmallLetter;
		TBDays.Text = dayRemains.ToString();
		ASFontHelper.SetFontSettings(this);
		CreatePayOrderDelegate = createPayOrderDelegate;
		CreatePayOrdButton.Visibility = ((Settings.SystemType != SystemTypes.Trade && Settings.SystemType != SystemTypes.Accountant) ? Visibility.Collapsed : Visibility.Visible);
	}

	private void CreatePayOrdButton_Click(object sender, RoutedEventArgs e)
	{
		if (CreatePayOrderDelegate != null)
		{
			CreatePayOrderDelegate();
		}
		e.Handled = true;
		if (!IsActive)
		{
			Close();
		}
	}

	private void GetPaymentDetailsButton_Click(object sender, RoutedEventArgs e)
	{
		new GetPaymentDetailsReportViewer().Show();
		e.Handled = true;
		Close();
	}

	[DebuggerNonUserCode]
	[GeneratedCode("PresentationBuildTasks", "10.0.9.0")]
	internal Delegate _CreateDelegate(Type delegateType, string handler)
	{
		return Delegate.CreateDelegate(delegateType, this, handler);
	}
}
