using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Windows;
using System.Windows.Markup;
using ArmSoft.SME.Common;
using ArmSoft.SME.Core.UI.Helpers;
using ArmSoft.SME.Core.UI.Report;
using DevExpress.Xpf.Core;

namespace ArmSoft.SME.Core.UI;

public partial class WndPageSetup : ThemedWindow, IComponentConnector
{
	private bool IsSpreadSheet { get; set; }

	public string ReportKind
	{
		get
		{
			return PageSetup.ReportKind;
		}
		set
		{
			if (Settings.HostingEnvironment.CurrentPanel is AsSpreadSheet)
			{
				IsSpreadSheet = true;
			}
			else
			{
				IsSpreadSheet = false;
			}
			PageSetup.ReportKind = (IsSpreadSheet ? ("SPS_" + value) : value);
			PageSetup.ActivePanel = Settings.HostingEnvironment.CurrentPanel;
			PageSetup.FillSettings();
		}
	}

	public WndPageSetup()
	{
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		InitializeComponent();
		Title = ArmSoft.SME.Common.Resources.PreviewOptions;
		OkButton.Content = ArmSoft.SME.Common.Resources.Ok;
		CancelButton.Content = ArmSoft.SME.Common.Resources.Cancel;
		Owner = Settings.HostingEnvironment;
		Icon = ImageService.GetImage("page_setup_16.svg");
		ASFontHelper.SetFontSettings(this);
	}

	private void Cancel_clicked(object sender, RoutedEventArgs e)
	{
		DialogResult = false;
	}

	private void Ok_clicked(object sender, RoutedEventArgs e)
	{
		PageSetup.SaveOptions();
		DialogResult = true;
	}

	[DebuggerNonUserCode]
	[GeneratedCode("PresentationBuildTasks", "10.0.9.0")]
	internal Delegate _CreateDelegate(Type delegateType, string handler)
	{
		return Delegate.CreateDelegate(delegateType, this, handler);
	}
}
